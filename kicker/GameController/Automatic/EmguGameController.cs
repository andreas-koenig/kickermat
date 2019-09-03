using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;

namespace GameController
{
    public class EmguGameController : BasicGameController
    {
        #region Fields
        private Position _CurrentBallPosition = new Position(), _LastBallPosition = new Position();
        private IOwnBarDetection _OwnbarDetection;
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
        #endregion

        public EmguGameController()
        {
            _LastMidFieldShotStopwatch.Start();
            _LastStrikerShotStopwatch.Start();
            _LastKeeperShotStopwatch.Start();
            _LastDefenseShotStopwatch.Start();
            _LastMidfieldDefenseDecisionStopwatch.Start();
            _LastAttemptToFreeBallStopwatch.Start();
            _OwnbarDetection = ServiceLocator.LocateService<IOwnBarDetection>();
        }

        protected override void Play(Game.Position ballPos)
        {
            _CurrentBallPosition = ballPos.Clone();
            if (_CurrentBallPosition.Valid)
                _LastBallPositionValidStopwatch.Restart();
            //Ballposition bestätigt und innerhalb des Spielbereichs: Normal spielen.
            if (_CurrentBallPosition.Valid && _CurrentBallPosition.InPlayingArea || !_CurrentBallPosition.Valid && _LastBallPositionValidStopwatch.ElapsedMilliseconds < 1000)
            {
                this.SetKeeperPosition(_CurrentBallPosition);
                this.SetDefensePosition(_CurrentBallPosition);
                this.SetMidFieldposition(_CurrentBallPosition);
                this.SetStrikerPosition(_CurrentBallPosition);
            }
            //Ballposition nicht bestätigt, liegt aber noch innerhalb des Spielbereichs. Vermutlich unterhalb einer Stange. Versuchen den Ball von der Stange wegzubekommen.
            else if (!_CurrentBallPosition.Valid && _CurrentBallPosition.InPlayingArea && _LastAttemptToFreeBallStopwatch.ElapsedMilliseconds > 300 && _LastBallPositionValidStopwatch.ElapsedMilliseconds < 5000)
            {
                int area = 40;
                //Get the correct X Positions for all Bars.
                int strikerBarXPosition = Coach.GetBarXPosition(Bar.Striker);
                int midfieldBarXPosition = Coach.GetBarXPosition(Bar.Midfield);
                int defenseBarXPosition = Coach.GetBarXPosition(Bar.Defense);
                int keeperBarXPosition = Coach.GetBarXPosition(Bar.Keeper);
                //Ball ist vermutlich unter der Stürmerstange
                if (_CurrentBallPosition.XPosition < strikerBarXPosition + area && _CurrentBallPosition.XPosition > strikerBarXPosition - area)
                {
                    if (_AttemptToFreeBallToTheLeft)
                    {
                        Coach.SetPlayerAngle(Bar.Striker, -20);
                        Coach.MovePlayerToYPosition(Player.StrikerOne, Coach.GetPlayerMinYPosition(Player.StrikerOne), false);
                    }
                    else
                    {
                        Coach.SetPlayerAngle(Bar.Striker, 20);
                        Coach.MovePlayerToYPosition(Player.StrikerOne, Coach.GetPlayerMaxYPosition(Player.StrikerOne), false);
                    }
                    _LastAttemptToFreeBallStopwatch.Restart();
                    _AttemptToFreeBallToTheLeft = !_AttemptToFreeBallToTheLeft;
                }
                //Ball ist vermutlich unter der Mittelfeldstange
                else if (_CurrentBallPosition.XPosition < midfieldBarXPosition + area && _CurrentBallPosition.XPosition > midfieldBarXPosition - area)
                {
                    if (_AttemptToFreeBallToTheLeft)
                    {
                        Coach.SetPlayerAngle(Bar.Midfield, -20);
                        Coach.MovePlayerToYPosition(Player.MidfieldOne, Coach.GetPlayerMinYPosition(Player.MidfieldOne));
                    }
                    else
                    {
                        Coach.SetPlayerAngle(Bar.Midfield, 20);
                        Coach.MovePlayerToYPosition(Player.MidfieldOne, Coach.GetPlayerMaxYPosition(Player.MidfieldOne));
                    }
                    _LastAttemptToFreeBallStopwatch.Restart();
                    _AttemptToFreeBallToTheLeft = !_AttemptToFreeBallToTheLeft;
                }
                //Ball ist vermutlich unter der Defensestange
                else if (_CurrentBallPosition.XPosition < defenseBarXPosition + area && _CurrentBallPosition.XPosition > defenseBarXPosition - area)
                {
                    if (_AttemptToFreeBallToTheLeft)
                    {
                        Coach.SetPlayerAngle(Bar.Defense, -20);
                        Coach.MovePlayerToYPosition(Player.DefenseOne, Coach.GetPlayerMinYPosition(Player.DefenseOne));
                    }
                    else
                    {
                        Coach.SetPlayerAngle(Bar.Defense, 20);
                        Coach.MovePlayerToYPosition(Player.DefenseOne, Coach.GetPlayerMaxYPosition(Player.DefenseOne));
                    }
                    _LastAttemptToFreeBallStopwatch.Restart();
                    _AttemptToFreeBallToTheLeft = !_AttemptToFreeBallToTheLeft;
                }
                //Ball ist vermutlich unter der Torhüterstange
                else if (_CurrentBallPosition.XPosition < keeperBarXPosition + area && _CurrentBallPosition.XPosition > keeperBarXPosition - area)
                {
                    if (_AttemptToFreeBallToTheLeft)
                    {
                        Coach.SetPlayerAngle(Bar.Keeper, -20);
                        Coach.MovePlayerToYPosition(Player.Keeper, Coach.GetPlayerMinYPosition(Player.Keeper));
                    }
                    else
                    {
                        Coach.SetPlayerAngle(Bar.Keeper, 20);
                        Coach.MovePlayerToYPosition(Player.Keeper, Coach.GetPlayerMaxYPosition(Player.Keeper));
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

        private void SetStrikerPosition(Position ballPos)
        {
            int strikerBarXPosition = Coach.GetBarXPosition(Bar.Striker);
            //Ball hinter Stürmer => Ball durchlassen.
            if (ballPos.XPosition > strikerBarXPosition + Settings.StrikerShootingRange)
            {
                Coach.SetPlayerAnglePass(Bar.Striker);
            }
            //Ball vor Stürmer => Ball blocken.
            else
            {
                //Erst wieder blocken wenn der letze Schuss lang genug her ist und der Ball von vorne kommt.
                if (_LastStrikerShotStopwatch.ElapsedMilliseconds > Settings.BlockingMilisecs && ballPos.XPosition < strikerBarXPosition - Settings.StrikerShootingRange / 2)
                {
                    Coach.SetPlayerAngleBlock(Bar.Striker);
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

                py = Coach.PlayingFieldCenter.YPosition;
                px = -10;

                // Geradengleichung
                // g: y = ((py-by)/(px-bx))*x + ((px*by-bx*py)/(px-bx))
                // Verteidigerstange: x = strikerBarXPosition             
                newStrikerPosition = (int)((((py - by) / (px - bx)) * (strikerBarXPosition) + (((px * by) - (bx * py)) / (px - bx))));
                if (Math.Abs(ballPos.YPosition - newStrikerPosition) > Settings.VerticalShootingRange)
                {
                    if (ballPos.YPosition > Coach.PlayingFieldCenter.YPosition)
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

            Player playerToMove = Player.StrikerOne;
            if (Coach.IsYPositionValid(Player.StrikerOne, newStrikerPosition))
            {
                playerToMove = Player.StrikerOne;
            }
            else if (Coach.IsYPositionValid(Player.StrikerTwo, newStrikerPosition))
            {
                playerToMove = Player.StrikerTwo;
            }
            else if (Coach.IsYPositionValid(Player.StrikerThree, newStrikerPosition))
            {
                playerToMove = Player.StrikerThree;
            }
            else if (ballPos.YPosition > Coach.GetPlayerMaxYPosition(Player.StrikerThree))
            {
                playerToMove = Player.StrikerThree;
            }

            if (newStrikerPosition > Coach.GetPlayerMaxYPosition(playerToMove))
                Coach.MovePlayerToYPosition(playerToMove, Coach.GetPlayerMaxYPosition(playerToMove));
            else if (newStrikerPosition < Coach.GetPlayerMinYPosition(playerToMove))
                Coach.MovePlayerToYPosition(playerToMove, Coach.GetPlayerMinYPosition(playerToMove));
            else
                Coach.MovePlayerToYPosition(playerToMove, newStrikerPosition);

            //Wenn der ball in Reichweite ist und genug Zeit seit dem letzen Schuss vergangen ist schießen.
            if (_LastStrikerShotStopwatch.ElapsedMilliseconds > Settings.ShootingMilisecs && ballPos.XPosition >= strikerBarXPosition - Settings.StrikerShootingRange && ballPos.XPosition < strikerBarXPosition + 15)
            {
                if (Settings.UseOwnBarDetectionToDeterminePlayerPositions)
                {
                    var playerPosition = _OwnbarDetection.GetPlayerPosition(playerToMove);
                    //Schieße nur wenn der Ball auch in YPosition erreicht wird
                    if (playerPosition.YPosition - Settings.VerticalShootingRange < ballPos.YPosition && playerPosition.YPosition + Settings.VerticalShootingRange > ballPos.YPosition)
                    {
                        Coach.SetPlayerAngle(Bar.Striker, 60);
                        _LastStrikerShotStopwatch.Restart();
                    }
                }
                else
                {
                    Coach.SetPlayerAngle(Bar.Striker, 60);
                    _LastStrikerShotStopwatch.Restart();
                }
            }
        }
        private void SetMidFieldposition(Position ballPos)
        {
            int midfieldBarXPosition = Coach.GetBarXPosition(Bar.Midfield);
            //Ball hinter Mittelfeld. Beine nach oben um Schüsse durchzulassen.
            if (ballPos.XPosition > midfieldBarXPosition + Settings.StrikerShootingRange)
            {
                Coach.SetPlayerAnglePass(Bar.Midfield);
            }
            //Ball vor Mittelfeld => Ball blocken
            else
            {
                //Erst wieder blocken wenn der letze Schuss lang genug her ist
                if (_LastMidFieldShotStopwatch.ElapsedMilliseconds > Settings.BlockingMilisecs)
                    Coach.SetPlayerAngleBlock(Bar.Midfield);
            }

            int newMidfieldPosition = ballPos.YPosition;
            //Wenn der Ball vor der SürmerStange ist soll eine leicht versetze Position nach Zufall eingenommen werden.
            if (ballPos.XPosition < Coach.GetBarXPosition(Bar.Striker))
            {
                newMidfieldPosition += _MidFieldBlockOffset * _MidFieldBlockDirection;
            }
            //Ansosnten wenn der Ball Vor der MIttelFeldStange ist soll er mittels Geradengleichung abgefangen werden.
            else if (false && ballPos.XPosition < Coach.GetBarXPosition(Bar.Midfield) - Settings.MidFieldShootingRange)
            {
                //Ball bewegt sich auf eigenes Tor zu.
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

            Player playerToMove = Player.MidfieldOne;
            if (Coach.IsYPositionValid(Player.MidfieldOne, newMidfieldPosition))
            {
                playerToMove = Player.MidfieldOne;
            }
            else if (Coach.IsYPositionValid(Player.MidfieldTwo, newMidfieldPosition))
            {
                playerToMove = Player.MidfieldTwo;
            }
            else if (Coach.IsYPositionValid(Player.MidfieldThree, newMidfieldPosition))
            {
                playerToMove = Player.MidfieldThree;
            }
            else if (Coach.IsYPositionValid(Player.MidfieldFour, newMidfieldPosition))
            {
                playerToMove = Player.MidfieldFour;
            }
            else if (Coach.IsYPositionValid(Player.MidfieldFive, newMidfieldPosition))
            {
                playerToMove = Player.MidfieldFive;
            }
            else if (ballPos.YPosition > Coach.GetPlayerMaxYPosition(Player.MidfieldFive))
            {
                playerToMove = Player.MidfieldFive;
            }

            if (newMidfieldPosition > Coach.GetPlayerMaxYPosition(playerToMove))
                Coach.MovePlayerToYPosition(playerToMove, Coach.GetPlayerMaxYPosition(playerToMove));
            else if (newMidfieldPosition < Coach.GetPlayerMinYPosition(playerToMove))
                Coach.MovePlayerToYPosition(playerToMove, Coach.GetPlayerMinYPosition(playerToMove));
            else
                Coach.MovePlayerToYPosition(playerToMove, newMidfieldPosition);

            //Wenn der ball in Reichweite ist und genug Zeit seit dem letzen Schuss vergangen ist schießen.
            if (_LastMidFieldShotStopwatch.ElapsedMilliseconds > Settings.ShootingMilisecs && ballPos.XPosition >= midfieldBarXPosition - Settings.MidFieldShootingRange && ballPos.XPosition < midfieldBarXPosition + 15)
            {
                if (Settings.UseOwnBarDetectionToDeterminePlayerPositions)
                {
                    var playerPosition = _OwnbarDetection.GetPlayerPosition(playerToMove);
                    //Schieße nur wenn der Ball auch in YPosition erreicht wird
                    if (playerPosition.YPosition - Settings.VerticalShootingRange < ballPos.YPosition && playerPosition.YPosition + Settings.VerticalShootingRange > ballPos.YPosition)
                    {
                        Coach.SetPlayerAngle(Bar.Midfield, 60);
                        _LastMidFieldShotStopwatch.Restart();
                    }
                }
                else
                {
                    Coach.SetPlayerAngle(Bar.Midfield, 60);
                    _LastMidFieldShotStopwatch.Restart();
                }
            }
        }
        private void SetDefensePosition(Position ballPos)
        {
            int defenseBarXPosition = Coach.GetBarXPosition(Bar.Defense);
            //Ball hinter Defense. Beine nach oben um Schüsse durchzulassen.
            if (ballPos.XPosition > defenseBarXPosition + Settings.StrikerShootingRange)
            {
                Coach.SetPlayerAnglePass(Bar.Defense);
            }
            //Ball vor Defense => blocken
            else
            {
                //Nur blocken wenn letzter Schuss lang genug her ist.
                if (_LastDefenseShotStopwatch.ElapsedMilliseconds > Settings.BlockingMilisecs)
                {
                    if (ballPos.XPosition < defenseBarXPosition - Settings.DefenseShootingRange)
                        Coach.SetPlayerAngle(Bar.Defense, 12);
                    else
                        Coach.SetPlayerAngle(Bar.Defense, 0);
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

                int z = Coach.PlayingFieldHeight, e = Coach.PlayingFieldWidth, y = Coach.GoalTop;
                double delta = ((z / 2.0) - by) / (z / 2.0) * deltamax;
                delta = delta < 0 ? -1 * delta : delta;

                // Punkt P
                px = e + Coach.PlayingFieldOffset.XPosition + delta;
                py = by < ((z / 2) + Coach.PlayingFieldOffset.YPosition) ? ((z + y) / 3.0) + Coach.PlayingFieldOffset.YPosition : ((2.0 / 3.0) * z - ((1.0 / 3.0) * y)) + Coach.PlayingFieldOffset.YPosition;
                //py = 255;

                //Korrigiere Koordinaten. 
                var pos1 = new Position((int)px, (int)py, true, true);
                SwissKnife.ParallaxCorrection(Coach.CameraLongZ, Coach.PlayerShortZ, Coach.PlayingFieldCenter, pos1);

                px = pos1.XPosition;
                py = pos1.YPosition;

                // Geradengleichung
                // g: y = ((py-by)/(px-bx))*x + ((px*by-bx*py)/(px-bx))
                // Verteidigerstange: x = defenseBarXPosition             
                newDefensePosition = (int)(((py - by) / (px - bx) * defenseBarXPosition) + (((px * by) - (bx * py)) / (px - bx)));
            }

            Player playerToMove = Player.DefenseOne;
            if (Coach.IsYPositionValid(Player.DefenseOne, newDefensePosition))
            {
                playerToMove = Player.DefenseOne;
            }
            else if (Coach.IsYPositionValid(Player.DefenseTwo, newDefensePosition))
            {
                playerToMove = Player.DefenseTwo;
            }
            else if (ballPos.YPosition > Coach.GetPlayerMaxYPosition(Player.DefenseTwo))
            {
                playerToMove = Player.DefenseTwo;
            }

            if (newDefensePosition > Coach.GetPlayerMaxYPosition(playerToMove))
                Coach.MovePlayerToYPosition(playerToMove, Coach.GetPlayerMaxYPosition(playerToMove));
            else if (newDefensePosition < Coach.GetPlayerMinYPosition(playerToMove))
                Coach.MovePlayerToYPosition(playerToMove, Coach.GetPlayerMinYPosition(playerToMove));
            else
                Coach.MovePlayerToYPosition(playerToMove, newDefensePosition);

            _CurrentDefensePosition = newDefensePosition;
            //Wenn der ball in Reichweite ist und genug Zeit seit dem letzen Schuss vergangen ist schießen.
            if (_LastDefenseShotStopwatch.ElapsedMilliseconds > Settings.ShootingMilisecs && ballPos.XPosition >= defenseBarXPosition - Settings.DefenseShootingRange && ballPos.XPosition < defenseBarXPosition + 15)
            {
                if (Settings.UseOwnBarDetectionToDeterminePlayerPositions)
                {
                    var playerPosition = _OwnbarDetection.GetPlayerPosition(playerToMove);
                    //Schieße nur wenn der Ball auch in YPosition erreicht wird
                    if (playerPosition.YPosition - Settings.VerticalShootingRange < ballPos.YPosition && playerPosition.YPosition + Settings.VerticalShootingRange > ballPos.YPosition)
                    {
                        Coach.SetPlayerAngle(Bar.Defense, 60);
                        _LastDefenseShotStopwatch.Restart();
                    }
                }
                else
                {
                    Coach.SetPlayerAngle(Bar.Defense, 60);
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

        public void LoadConfiguration(string xmlFileName)
        {
            var settings = SettingsSerializer.LoadSettingsFromXml<EmguGameControllerSettings>(xmlFileName);
            if (settings != null)
            {
                Settings = settings;
            }
            UserControl.SetSettings(Settings);
        }

        public void SaveConfiguration(string xmlFileName)
        {
            SettingsSerializer.SaveSettingsToXml<EmguGameControllerSettings>(Settings, xmlFileName);
        }

        public void InitUserControl()
        {
            if (UserControl != null)
                UserControl.SetSettings(Settings);
        }
    }
}
