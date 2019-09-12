using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communication.PlayerControl;
using Communication.Sets;
using GameProperties;
using GameProperties;

namespace GameController
{

    //TODO: Do FirstPlayer and LastPlayer retunr correct results ?
    public class EmguGameController : BasicGameController
    {
        #region Fields
        private Position _CurrentBallPosition = new Position(), _LastBallPosition = new Position();
        private bool _AttemptToFreeBallToTheLeft = true;
        private readonly Stopwatch _LastAttemptToFreeBallStopwatch = new Stopwatch();
        private readonly Stopwatch _LastMidFieldShotStopwatch = new Stopwatch();
        private readonly Stopwatch _LastStrikerShotStopwatch = new Stopwatch();
        private readonly Stopwatch _LastDefenseShotStopwatch = new Stopwatch();
        private readonly Stopwatch _LastKeeperShotStopwatch = new Stopwatch();
        private readonly Stopwatch _LastBallPositionValidStopwatch = new Stopwatch();
        private readonly Stopwatch _LastMidfieldDefenseDecisionStopwatch = new Stopwatch();
        private int _MidFieldBlockDirection = 1;
        private int _MidFieldBlockOffset = 0;
        private int _MidFieldBlockDuration = 2000;
        private readonly Random _MidFieldRandom = new Random();
        private int _CurrentDefensePosition = 0;
        private EmguGameControllerSettings Settings = new EmguGameControllerSettings();

        IPlayerControl _CommunicationMgr;
        #endregion

        public EmguGameController(IPlayerControl communcationMgr)
        {
            _LastMidFieldShotStopwatch.Start();
            _LastStrikerShotStopwatch.Start();
            _LastKeeperShotStopwatch.Start();
            _LastDefenseShotStopwatch.Start();
            _LastMidfieldDefenseDecisionStopwatch.Start();
            _LastAttemptToFreeBallStopwatch.Start();
            _CommunicationMgr = communcationMgr;
        }

        protected override void Play(Game game)
        {
            _CurrentBallPosition = game.ballPosition.Clone();
            if (_CurrentBallPosition.Valid)
                _LastBallPositionValidStopwatch.Restart();
            //Ballposition bestätigt und innerhalb des Spielbereichs: Normal spielen.
            if (_CurrentBallPosition.Valid && _CurrentBallPosition.InPlayingArea || !_CurrentBallPosition.Valid && _LastBallPositionValidStopwatch.ElapsedMilliseconds < 1000)
            {
                this.SetKeeperPosition(game);
                this.SetDefensePosition(game);
                this.SetMidFieldposition(game);
                this.SetStrikerPosition(game);
            }
            //Ballposition nicht bestätigt, liegt aber noch innerhalb des Spielbereichs. Vermutlich unterhalb einer Stange. Versuchen den Ball von der Stange wegzubekommen.
            else if (!_CurrentBallPosition.Valid && _CurrentBallPosition.InPlayingArea && _LastAttemptToFreeBallStopwatch.ElapsedMilliseconds > 300 && _LastBallPositionValidStopwatch.ElapsedMilliseconds < 5000)
            {
                int area = 40;
                //Get the correct X Positions for all Bars.
                //int strikerBarXPosition = Coach.GetBarXPosition(Bar.Striker);
                //int midfieldBarXPosition = Coach.GetBarXPosition(Bar.Midfield);
                //int defenseBarXPosition = Coach.GetBarXPosition(Bar.Defense);
                //int keeperBarXPosition = Coach.GetBarXPosition(Bar.Keeper);

                //TODO: GameController should work with double in future
                int strikerBarXPosition = (int)game.ownBars.striker.XPosition;
                int midfieldBarXPosition = (int)game.ownBars.midfield.XPosition;
                int defenseBarXPosition = (int)game.ownBars.defense.XPosition;
                int keeperBarXPosition = (int)game.ownBars.keeper.XPosition;

                //Ball ist vermutlich unter der Stürmerstange
                if (_CurrentBallPosition.XPosition < strikerBarXPosition + area && _CurrentBallPosition.XPosition > strikerBarXPosition - area)
                {
                    if (_AttemptToFreeBallToTheLeft)
                    {
                        _CommunicationMgr.SetAngle(game.ownBars.striker, -20);
                        _CommunicationMgr.MovePlayer(game.ownBars.striker, Convert.ToUInt16(game.ownBars.striker.GetPlayerByPosition(0).MinPosition));

                    }
                    else
                    {
                        _CommunicationMgr.SetAngle(game.ownBars.striker, 20);
                        _CommunicationMgr.MovePlayer(game.ownBars.striker, Convert.ToUInt16(game.ownBars.striker.GetPlayerByPosition(0).MaxPosition));
                    }
                    _LastAttemptToFreeBallStopwatch.Restart();
                    _AttemptToFreeBallToTheLeft = !_AttemptToFreeBallToTheLeft;
                }
                //Ball ist vermutlich unter der Mittelfeldstange
                else if (_CurrentBallPosition.XPosition < midfieldBarXPosition + area && _CurrentBallPosition.XPosition > midfieldBarXPosition - area)
                {
                    if (_AttemptToFreeBallToTheLeft)
                    {
                        _CommunicationMgr.SetAngle(game.ownBars.midfield, -20);
                        _CommunicationMgr.MovePlayer(game.ownBars.midfield, Convert.ToUInt16(game.ownBars.midfield.GetPlayerByPosition(0).MinPosition));
                    }
                    else
                    {
                        _CommunicationMgr.SetAngle(game.ownBars.midfield, 20);
                        _CommunicationMgr.MovePlayer(game.ownBars.midfield, Convert.ToUInt16(game.ownBars.midfield.GetPlayerByPosition(0).MaxPosition));
                    }
                    _LastAttemptToFreeBallStopwatch.Restart();
                    _AttemptToFreeBallToTheLeft = !_AttemptToFreeBallToTheLeft;
                }
                //Ball ist vermutlich unter der Defensestange
                else if (_CurrentBallPosition.XPosition < defenseBarXPosition + area && _CurrentBallPosition.XPosition > defenseBarXPosition - area)
                {
                    if (_AttemptToFreeBallToTheLeft)
                    {
                        _CommunicationMgr.SetAngle(game.ownBars.defense, -20);
                        _CommunicationMgr.MovePlayer(game.ownBars.defense, Convert.ToUInt16(game.ownBars.defense.GetPlayerByPosition(0).MinPosition));
                    }
                    else
                    {
                        _CommunicationMgr.SetAngle(game.ownBars.defense, 20);
                        _CommunicationMgr.MovePlayer(game.ownBars.defense, Convert.ToUInt16(game.ownBars.defense.GetPlayerByPosition(0).MaxPosition));
                    }
                    _LastAttemptToFreeBallStopwatch.Restart();
                    _AttemptToFreeBallToTheLeft = !_AttemptToFreeBallToTheLeft;
                }

                //Ball ist vermutlich unter der Torhüterstange
                else if (_CurrentBallPosition.XPosition < keeperBarXPosition + area && _CurrentBallPosition.XPosition > keeperBarXPosition - area)
                {
                    if (_AttemptToFreeBallToTheLeft)
                    {
                        _CommunicationMgr.SetAngle(game.ownBars.keeper, -20);
                        _CommunicationMgr.MovePlayer(game.ownBars.keeper, Convert.ToUInt16(game.ownBars.keeper.GetPlayerByPosition(0).MinPosition));
                    }
                    else
                    {
                        _CommunicationMgr.SetAngle(game.ownBars.keeper, 20);
                        _CommunicationMgr.MovePlayer(game.ownBars.keeper, Convert.ToUInt16(game.ownBars.keeper.GetPlayerByPosition(0).MaxPosition));
                    }
                    _LastAttemptToFreeBallStopwatch.Restart();
                    _AttemptToFreeBallToTheLeft = !_AttemptToFreeBallToTheLeft;
                }
            }
            //Ballposition nicht bestätigt und/oder auserhalb des Spielbereichs.
            else
            {
                //Do nothing
            }
            _LastBallPosition = _CurrentBallPosition;
        }

        private void SetStrikerPosition(Game game)
        {
            int strikerBarXPosition = (int)game.ownBars.striker.XPosition;
            Position ballPos = game.ballPosition;
            Bars ownBars = game.ownBars;

            //Ball hinter Stürmer => Ball durchlassen.
            if (ballPos.XPosition > strikerBarXPosition + Settings.StrikerShootingRange)
            {
                _CommunicationMgr.SetPlayerAnglePass(game.ownBars.striker);
            }
            //Ball vor Stürmer => Ball blocken.
            else
            {
                //Erst wieder blocken wenn der letze Schuss lang genug her ist und der Ball von vorne kommt.
                if (_LastStrikerShotStopwatch.ElapsedMilliseconds > Settings.BlockingMilisecs && ballPos.XPosition < strikerBarXPosition - Settings.StrikerShootingRange / 2)
                {
                    _CommunicationMgr.SetPlayerAngleBlock(game.ownBars.striker);
                }
            }

            int newStrikerPosition = ballPos.YPosition;
            //Ball in Schussreichweite. gegebenenfalls schräg schießen
            if (_LastStrikerShotStopwatch.ElapsedMilliseconds > Settings.ShootingMilisecs && ballPos.XPosition >= strikerBarXPosition - Settings.StrikerShootingRange && ballPos.XPosition < strikerBarXPosition + 15)
            {
                // Punkt B
                double px, py;
                int bx = ballPos.XPosition;
                int by = ballPos.YPosition;

                py = game.playingField.Origin.Y;
                px = -10;

                // Geradengleichung
                // g: y = ((py-by)/(px-bx))*x + ((px*by-bx*py)/(px-bx))
                // Verteidigerstange: x = strikerBarXPosition             
                newStrikerPosition = (int)((((py - by) / (px - bx)) * (strikerBarXPosition) + (((px * by) - (bx * py)) / (px - bx))));
                if (Math.Abs(ballPos.YPosition - newStrikerPosition) > Settings.VerticalShootingRange)
                {
                    if (ballPos.YPosition > game.playingField.Origin.Y)
                        newStrikerPosition = ballPos.YPosition + Settings.VerticalShootingRange;
                    else
                        newStrikerPosition = ballPos.YPosition - Settings.VerticalShootingRange;
                }
            }
            //Ball vor Stürmer, ball nicht in Schussreichweite. Versuche Ball auf Linie zu blockieren.
            else if (ballPos.XPosition < strikerBarXPosition)
            {
                //Ball geht Richtung eigenes Tor.
                if (_LastBallPosition.XPosition < _CurrentBallPosition.XPosition && Math.Abs(_LastBallPosition.XPosition - _CurrentBallPosition.XPosition) > 5)
                {
                    double px = _LastBallPosition.XPosition, py = _LastBallPosition.YPosition, bx = _CurrentBallPosition.XPosition, by = _CurrentBallPosition.YPosition;
                    newStrikerPosition = (int)((((py - by) / (px - bx)) * (strikerBarXPosition) + (((px * by) - (bx * py)) / (px - bx))));
                }
            }

            Player playerToMove = game.ownBars.striker.GetPlayerByPosition(0);
            if (IsYPositionValid(game.ownBars.striker.GetPlayerByPosition(0), newStrikerPosition))
            {
                playerToMove = game.ownBars.striker.GetPlayerByPosition(0);
            }
            else if (IsYPositionValid(game.ownBars.striker.LastPlayer(), newStrikerPosition))
            {
                playerToMove = game.ownBars.striker.LastPlayer();
            }
            else if (IsYPositionValid(game.ownBars.striker.GetPlayerByPosition(2), newStrikerPosition))
            {
                playerToMove = game.ownBars.striker.GetPlayerByPosition(2);
            }
            else if (ballPos.YPosition > game.ownBars.striker.GetPlayerByPosition(2).MaxPosition)
            {
                playerToMove = game.ownBars.striker.GetPlayerByPosition(2);
            }

            if (newStrikerPosition > playerToMove.MaxPosition)
            {
                _CommunicationMgr.MovePlayer(playerToMove.GetBar(), (ushort)playerToMove.MaxPosition);
            }
            else if (newStrikerPosition < playerToMove.MinPosition)
            {
                _CommunicationMgr.MovePlayer(playerToMove.GetBar(), (ushort)playerToMove.MinPosition);
            }
            else
            {
                _CommunicationMgr.MovePlayer(playerToMove.GetBar(), (ushort)newStrikerPosition);
            }

            //Wenn der ball in Reichweite ist und genug Zeit seit dem letzen Schuss vergangen ist schießen.
            if (_LastStrikerShotStopwatch.ElapsedMilliseconds > Settings.ShootingMilisecs && ballPos.XPosition >= strikerBarXPosition - Settings.StrikerShootingRange && ballPos.XPosition < strikerBarXPosition + 15)
            {
                if (Settings.UseOwnBarDetectionToDeterminePlayerPositions)
                {
                    var playerPosition = playerToMove;
                    //Schieße nur wenn der Ball auch in YPosition erreicht wird
                    if (playerPosition.YPosition - Settings.VerticalShootingRange < ballPos.YPosition && playerPosition.YPosition + Settings.VerticalShootingRange > ballPos.YPosition)
                    {
                        _CommunicationMgr.SetAngle(ownBars.striker, 60);
                        _LastStrikerShotStopwatch.Restart();
                    }
                }
                else
                {
                    //TODO: Twice the same behaviour ?
                    _CommunicationMgr.SetAngle(ownBars.striker, 60);
                    _LastStrikerShotStopwatch.Restart();
                }
            }
        }
        private void SetMidFieldposition(Game game)
        {
            int midfieldBarXPosition = (int)game.ownBars.midfield.XPosition;
            Bars ownBars = game.ownBars;
            Position ballPos = game.ballPosition;
            //Ball hinter Mittelfeld. Beine nach oben um Schüsse durchzulassen.
            if (ballPos.XPosition > midfieldBarXPosition + Settings.StrikerShootingRange)
            {
                _CommunicationMgr.SetPlayerAnglePass(ownBars.midfield);
            }
            //Ball vor Mittelfeld => Ball blocken
            else
            {
                //Erst wieder blocken wenn der letze Schuss lang genug her ist
                if (_LastMidFieldShotStopwatch.ElapsedMilliseconds > Settings.BlockingMilisecs)
                {
                    _CommunicationMgr.SetPlayerAngleBlock(ownBars.midfield);
                }
            }

            int newMidfieldPosition = ballPos.YPosition;
            //Wenn der Ball vor der SürmerStange ist soll eine leicht versetze Position nach Zufall eingenommen werden.
            if (ballPos.XPosition < ownBars.striker.XPosition)
            {
                newMidfieldPosition += _MidFieldBlockOffset * _MidFieldBlockDirection;
            }
            //Ansosnten wenn der Ball Vor der MIttelFeldStange ist soll er mittels Geradengleichung abgefangen werden.
            else if (false && ballPos.XPosition < ownBars.midfield.XPosition - Settings.MidFieldShootingRange)
            {
                //Ball bewegt sich auf eigenes Tor zu.
                //TODO: Hard Coded Pixels ?
                if (_LastBallPosition.XPosition < _CurrentBallPosition.XPosition && Math.Abs(_LastBallPosition.XPosition - _CurrentBallPosition.XPosition) > 5)
                {
                    double px = _LastBallPosition.XPosition, py = _LastBallPosition.YPosition, bx = _CurrentBallPosition.XPosition, by = _CurrentBallPosition.YPosition;
                    newMidfieldPosition = (int)((((py - by) / (px - bx)) * (midfieldBarXPosition) + (((px * by) - (bx * py)) / (px - bx))));
                }
            }
            //Setze neue Parameter für die versetze Position nach gewisser Zeit
            if (_LastMidfieldDefenseDecisionStopwatch.ElapsedMilliseconds > _MidFieldBlockDuration)
            {
                _MidFieldBlockDuration = _MidFieldRandom.Next(250, 1000);
                _MidFieldBlockOffset = _MidFieldRandom.Next(5, 20);
                _MidFieldBlockDirection = (_MidFieldRandom.Next() % 2) == 0 ? 1 : -1;
                _LastMidfieldDefenseDecisionStopwatch.Restart();
            }

            //TODO: Use loop and break ?
            Player playerToMove = ownBars.midfield.GetPlayerByPosition(1);
            if (ownBars.midfield.GetPlayerByPosition(1).IsYPositionValid(newMidfieldPosition))
            {
                playerToMove = ownBars.midfield.GetPlayerByPosition(1);
            }
            else if (ownBars.midfield.GetPlayerByPosition(2).IsYPositionValid(newMidfieldPosition))
            {
                playerToMove = ownBars.midfield.GetPlayerByPosition(2);
            }
            else if (ownBars.midfield.GetPlayerByPosition(3).IsYPositionValid(newMidfieldPosition))
            {
                playerToMove = ownBars.midfield.GetPlayerByPosition(3);
            }
            else if (ownBars.midfield.GetPlayerByPosition(4).IsYPositionValid(newMidfieldPosition))
            {
                playerToMove = ownBars.midfield.GetPlayerByPosition(4);
            }
            else if (ownBars.midfield.GetPlayerByPosition(5).IsYPositionValid(newMidfieldPosition))
            {
                playerToMove = ownBars.midfield.GetPlayerByPosition(5);
            }
            else if (ballPos.YPosition > ownBars.midfield.GetPlayerByPosition(5).MaxPosition)
            {
                playerToMove = ownBars.midfield.GetPlayerByPosition(5);
            }

            if (newMidfieldPosition > playerToMove.MaxPosition)
            {
                _CommunicationMgr.MovePlayer(playerToMove.GetBar(), (ushort)playerToMove.MaxPosition);
            }
            else if (newMidfieldPosition < playerToMove.MinPosition)
            {
                _CommunicationMgr.MovePlayer(playerToMove.GetBar(), (ushort)playerToMove.MinPosition);
            }
            else
            {
                _CommunicationMgr.MovePlayer(playerToMove.GetBar(), (ushort)newMidfieldPosition);
            }

            //Wenn der ball in Reichweite ist und genug Zeit seit dem letzen Schuss vergangen ist schießen.
            if (_LastMidFieldShotStopwatch.ElapsedMilliseconds > Settings.ShootingMilisecs && ballPos.XPosition >= midfieldBarXPosition - Settings.MidFieldShootingRange && ballPos.XPosition < midfieldBarXPosition + 15)
            {
                if (Settings.UseOwnBarDetectionToDeterminePlayerPositions)
                {
                    var playerPosition = playerToMove;
                    //Schieße nur wenn der Ball auch in YPosition erreicht wird
                    if (playerPosition.YPosition - Settings.VerticalShootingRange < ballPos.YPosition && playerPosition.YPosition + Settings.VerticalShootingRange > ballPos.YPosition)
                    {
                        _CommunicationMgr.SetAngle(ownBars.midfield, 60);
                        _LastMidFieldShotStopwatch.Restart();
                    }
                }
                else
                {
                    //TODO: Same behaviour twice
                    _CommunicationMgr.SetAngle(ownBars.midfield, 60);
                    _LastMidFieldShotStopwatch.Restart();
                }
            }
        }
        private void SetDefensePosition(Game game)
        {
            int defenseBarXPosition = (int)game.ownBars.defense.XPosition;
            Bars ownBars = game.ownBars;
            Position ballPos = game.ballPosition;
            Bar defense = ownBars.defense;
            //Ball hinter Defense. Beine nach oben um Schüsse durchzulassen.
            if (ballPos.XPosition > defenseBarXPosition + Settings.StrikerShootingRange)
            {
                _CommunicationMgr.SetPlayerAnglePass(defense);
            }
            //Ball vor Defense => blocken
            else
            {
                //Nur blocken wenn letzter Schuss lang genug her ist.
                if (_LastDefenseShotStopwatch.ElapsedMilliseconds > Settings.BlockingMilisecs)
                {
                    if (ballPos.XPosition < defenseBarXPosition - Settings.DefenseShootingRange)
                    {
                        _CommunicationMgr.SetAngle(defense, 12);
                    }                     
                    else
                    {
                        _CommunicationMgr.SetAngle(defense, 0);
                    }
                }
            }

            int newDefensePosition = ballPos.YPosition;
            //Versuche den Weg in Richtung Tormitte zu blocken, wenn der Ball weit genug vor der Verteidigung ist.
            if (ballPos.XPosition < defenseBarXPosition - Settings.DefenseShootingRange)
            {
                // Punkt B
                double px, py;
                int bx = ballPos.XPosition;
                int by = ballPos.YPosition;

                double deltamax = 15;
                //TODO: Length == Height of legacy-code?
                //TODO: What is GoalTop and why is it needed ?
                int GoalTop = 100;
                int z = (int)game.playingField.Length , e = (int)game.playingField.Width, y = GoalTop;
                double delta = ((z / 2.0) - by) / (z / 2.0) * deltamax;
                delta = delta < 0 ? -1 * delta : delta;

                // Punkt P
                int xOffset = game.playingField.PlayingFieldOffset.XPosition;
                int yOffset = game.playingField.PlayingFieldOffset.YPosition;
                px = e + xOffset + delta;
                py = by < ((z / 2) + yOffset) ? ((z + y) / 3.0) + yOffset: ((2.0 / 3.0) * z - ((1.0 / 3.0) * y)) + yOffset;
                //py = 255;

                // Geradengleichung
                // g: y = ((py-by)/(px-bx))*x + ((px*by-bx*py)/(px-bx))
                // Verteidigerstange: x = defenseBarXPosition             
                newDefensePosition = (int)(((py - by) / (px - bx) * defenseBarXPosition) + (((px * by) - (bx * py)) / (px - bx)));
            }

            Player playerToMove = defense.GetPlayerByPosition(1);
            if(defense.GetPlayerByPosition(1).IsYPositionValid(newDefensePosition))
            {
                playerToMove = defense.GetPlayerByPosition(1);
            }
            else if (defense.GetPlayerByPosition(2).IsYPositionValid(newDefensePosition))
            {
                playerToMove = defense.GetPlayerByPosition(2);
            }
            else if (ballPos.YPosition > ownBars.defense.GetPlayerByPosition(2).MaxPosition)
            {
                playerToMove = defense.GetPlayerByPosition(2);
            }

            if (newDefensePosition > playerToMove.MaxPosition)
            {
                _CommunicationMgr.MovePlayer(playerToMove.GetBar(), Convert.ToUInt16(playerToMove.MaxPosition));
            }
            else if (newDefensePosition < playerToMove.MinPosition)
            {
                _CommunicationMgr.MovePlayer(playerToMove.GetBar(), Convert.ToUInt16(playerToMove.MinPosition));
            }
            else
            {
                _CommunicationMgr.MovePlayer(playerToMove.GetBar(), Convert.ToUInt16(newDefensePosition));
            }
            _CurrentDefensePosition = newDefensePosition;
            //Wenn der ball in Reichweite ist und genug Zeit seit dem letzen Schuss vergangen ist schießen.
            if (_LastDefenseShotStopwatch.ElapsedMilliseconds > Settings.ShootingMilisecs && ballPos.XPosition >= defenseBarXPosition - Settings.DefenseShootingRange && ballPos.XPosition < defenseBarXPosition + 15)
            {
                if (Settings.UseOwnBarDetectionToDeterminePlayerPositions)
                {
                    var playerPosition = playerToMove;
                    //Schieße nur wenn der Ball auch in YPosition erreicht wird
                    if (playerPosition.YPosition - Settings.VerticalShootingRange < ballPos.YPosition && playerPosition.YPosition + Settings.VerticalShootingRange > ballPos.YPosition)
                    {
                        _CommunicationMgr.SetAngle(defense, 60);
                        _LastDefenseShotStopwatch.Restart();
                    }
                }
                else
                {
                    _CommunicationMgr.SetAngle(defense, 60);
                    _LastDefenseShotStopwatch.Restart();
                }
            }
        }

        private Stopwatch _KeeperFreeBallStopwatch = new Stopwatch();
        private int _KeeperFreeBallStage = 0;

        private void SetKeeperPosition(Position ballPos)
        {
            int keeperBarXPosition = Coach.GetBarXPosition(Bar.Keeper);
            int defenseBarXPosition = Coach.GetBarXPosition(Bar.Defense);
            int newKeeperPosition = ballPos.YPosition;
            //Ball hinter Torwart => Versuche den Ball wegzuschlagen
            if (ballPos.XPosition > keeperBarXPosition + 10)
            {
                //Ball is reachable for Keeper -> Try to free it.
                if (ballPos.YPosition < Coach.GetPlayerMaxYPosition(Player.Keeper) + Settings.VerticalShootingRange && ballPos.YPosition > Coach.GetPlayerMinYPosition(Player.Keeper) - Settings.VerticalShootingRange)
                {
                    switch (_KeeperFreeBallStage)
                    {
                        //Move Keeper to a safe location in order to not shoot the ball into the own goal.
                        case 0:
                            if (ballPos.YPosition >= _OwnbarDetection.GetPlayerPosition(Player.Keeper).YPosition)
                                Coach.MovePlayerToYPosition(Player.Keeper, Coach.GetPlayerMinYPosition(Player.Keeper));
                            else
                                Coach.MovePlayerToYPosition(Player.Keeper, Coach.GetPlayerMaxYPosition(Player.Keeper));
                            _KeeperFreeBallStopwatch.Restart();
                            _KeeperFreeBallStage++;
                            break;
                        //Set Angle of Keeper in order to reach the ball. 
                        case 1:
                            if (_LastKeeperShotStopwatch.ElapsedMilliseconds > 1500)
                            {
                                Coach.SetPlayerAngle(Bar.Keeper, -50);
                                _LastKeeperShotStopwatch.Restart();
                                _KeeperFreeBallStage++;
                            }
                            break;
                        //Move Keeper to Ballposition.
                        case 2:
                            if (_KeeperFreeBallStopwatch.ElapsedMilliseconds > 500)
                            {
                                if (newKeeperPosition > Coach.GetPlayerMaxYPosition(Player.Keeper))
                                    Coach.MovePlayerToYPosition(Player.Keeper, Coach.GetPlayerMaxYPosition(Player.Keeper));
                                else if (newKeeperPosition < Coach.GetPlayerMinYPosition(Player.Keeper))
                                    Coach.MovePlayerToYPosition(Player.Keeper, Coach.GetPlayerMinYPosition(Player.Keeper));
                                else
                                    Coach.MovePlayerToYPosition(Player.Keeper, newKeeperPosition);
                                _LastKeeperShotStopwatch.Restart();
                                _KeeperFreeBallStage++;
                            }
                            break;
                        //Try to shoot Ball.
                        case 3:
                            if (_LastKeeperShotStopwatch.ElapsedMilliseconds > 500)
                            {
                                Coach.SetPlayerAngle(Bar.Keeper, 12);
                                _KeeperFreeBallStage++;
                                _KeeperFreeBallStopwatch.Restart();
                            }
                            break;
                        //All stages complete -> Reset.
                        case 4:
                            if (_LastKeeperShotStopwatch.ElapsedMilliseconds > 500)
                            {
                                _KeeperFreeBallStage = 0;
                                _KeeperFreeBallStopwatch.Reset();
                            }
                            break;
                        default:
                            _KeeperFreeBallStage = 0;
                            _KeeperFreeBallStopwatch.Reset();
                            break;
                    }
                    return;
                }
                //ball is not reachable -> Reset Stages.
                else
                {
                    _KeeperFreeBallStage = 0;
                    _KeeperFreeBallStopwatch.Reset();
                }
                return;
            }
            //Reset the Stages for Freeing the Ball.
            else
            {
                _KeeperFreeBallStage = 0;
                _KeeperFreeBallStopwatch.Reset();
            }

            //Blockieren wenn der letze Schuss lang genug her ist.
            if (_LastKeeperShotStopwatch.ElapsedMilliseconds > Settings.BlockingMilisecs)
            {
                if (ballPos.XPosition < defenseBarXPosition)
                    Coach.SetPlayerAngle(Bar.Keeper, 12);
                else
                {
                    //Ball kommt von der Seite. Stelle den Winkel so, dass das Tor zugemacht wird.
                    if (!Coach.IsYPositionValid(Bar.Keeper, ballPos.YPosition))
                        Coach.SetPlayerAngle(Bar.Keeper, -40);
                    else
                        Coach.SetPlayerAngle(Bar.Keeper, 0);
                }
            }
            //Ball vor Defense. Versuche den Ball relativ zur Tormitte zu folgen und den Weg damit zu blockieren.
            if (ballPos.XPosition < defenseBarXPosition - 20)
            {
                // Punkt B
                double px, py;
                int bx = ballPos.XPosition;
                int by = ballPos.YPosition;

                double deltamax = 15.0;
                int z = Coach.PlayingFieldHeight, e = Coach.PlayingFieldWidth, y = Coach.GoalTop;
                double delta = ((z / 2.0) - by) / (z / 2.0) * deltamax;
                delta = delta < 0 ? -1 * delta : delta;

                // Punkt P versetzt zum Punkt für Verteidigung.
                px = e + Coach.PlayingFieldOffset.XPosition + delta;
                if (by < ((z / 2) + Coach.PlayingFieldOffset.YPosition))
                {
                    py = ((5.0 / 6.0) * z) - ((2.0 / 3.0) * y) + Coach.PlayingFieldOffset.YPosition;
                }
                else
                {
                    py = ((1.0 / 6.0) * z) + ((2.0 / 3.0) * y) + Coach.PlayingFieldOffset.YPosition;
                }

                //Korrigiere Koordinaten.
                var pos1 = new Position((int)px, (int)py, true, true);
                SwissKnife.ParallaxCorrection(Coach.CameraLongZ, Coach.PlayerShortZ, Coach.PlayingFieldCenter, pos1);

                px = pos1.XPosition;
                py = pos1.YPosition;

                //Wenn Ball vor dem Mittelfeld ist keine ganz so extreme position einnehmen, sondern eher zur Tormitte.
                if (ballPos.XPosition < Coach.GetBarXPosition(Bar.Midfield))
                    py = Coach.PlayingFieldCenter.YPosition;

                // Geradengleichung
                // g: y = ((py-by)/(px-bx))*x + ((px*by-bx*py)/(px-bx))
                // Tormannstange: x = keeperBarXPosition

                // Wenn Ball zwischen Defense and Midfield, dann nimmt Keeper modifizierte dynamische Defense-Position ein
                if (ballPos.XPosition > Coach.GetBarXPosition(Bar.Midfield))
                {
                    // Bessere Keeperposition um schräge Schüsse zu verhindern
                    newKeeperPosition = (int)(((py - by) / (px - bx) * keeperBarXPosition) + (((px * by) - (bx * py)) / (px - bx)) * 1.04);
                }
                // Andernfalls
                else
                {
                    newKeeperPosition = (int)(((py - by) / (px - bx) * keeperBarXPosition) + (((px * by) - (bx * py)) / (px - bx)));
                }
            }

            if (newKeeperPosition > Coach.GetPlayerMaxYPosition(Player.Keeper))
                Coach.MovePlayerToYPosition(Player.Keeper, Coach.GetPlayerMaxYPosition(Player.Keeper));
            else if (newKeeperPosition < Coach.GetPlayerMinYPosition(Player.Keeper))
                Coach.MovePlayerToYPosition(Player.Keeper, Coach.GetPlayerMinYPosition(Player.Keeper));
            else
                Coach.MovePlayerToYPosition(Player.Keeper, newKeeperPosition);

            //Wenn der ball in Reichweite ist und genug Zeit seit dem letzen Schuss vergangen ist schießen.
            if (_LastKeeperShotStopwatch.ElapsedMilliseconds > Settings.ShootingMilisecs && ballPos.XPosition >= Coach.GetBarXPosition(Bar.Keeper) - Settings.KeeperShootingRange && ballPos.XPosition < Coach.GetBarXPosition(Bar.Keeper) + 15)
            {
                if (Settings.UseOwnBarDetectionToDeterminePlayerPositions)
                {
                    var keeperPosition = _OwnbarDetection.GetPlayerPosition(Player.Keeper);
                    //Schieße nur wenn der Ball auch in YPosition erreicht wird
                    if (keeperPosition.YPosition - Settings.VerticalShootingRange < ballPos.YPosition && keeperPosition.YPosition + Settings.VerticalShootingRange > ballPos.YPosition)
                    {
                        Coach.SetPlayerAngle(Bar.Keeper, 60);
                        _LastKeeperShotStopwatch.Restart();
                    }
                }
                else
                {
                    Coach.SetPlayerAngle(Bar.Keeper, 60);
                    _LastKeeperShotStopwatch.Restart();
                }
            }
        }

        /// <summary>
        /// Determines whether [is position valid] [the specified player].
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="position">The position.</param>
        /// <returns>
        ///     <c>true</c> if [is position valid] [the specified player]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsYPositionValid(Player player, int position)
        {
            return position >= player.MinPosition &&
                   position <= player.MinPosition;
        }

        /// <summary>
        /// Determines whether [is position valid] [the specified bar].
        /// </summary>
        /// <param name="bar">The bar which is checked.</param>
        /// <param name="position">The y position for the check.</param>
        /// <returns>
        ///     <c>true</c> if [is position valid] [the specified bar]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsYPositionValid(Bar bar, int position)
        {
            return position >= bar.FirstPlayer().MinPosition &&
                   position <= bar.LastPlayer().MaxPosition;
        }

    }
}
