using System;
using System.Collections.Generic;
using System.Linq;
using static GameProperties.BarType;

namespace GameProperties
{
    // TODO: Datatypes for length and angle
    // TODO: Set AccessModifiers etc ...

    /// <summary>
    /// Lists all bars and their code for creating datagrams.
    /// </summary>
    public enum BarType : ushort
    {
        /// <summary>
        /// All bars are used.
        /// </summary>
        All = 0x00,

        /// <summary>
        /// Stange des Tormanns.
        /// </summary>
        Keeper = 0x01,

        /// <summary>
        /// Stange der Abwehrspieler.
        /// </summary>
        Defense = 0x02,

        /// <summary>
        /// Stange der Mittelfeldspieler.
        /// </summary>
        Midfield = 0x03,

        /// <summary>
        /// Stange der Stürmer.
        /// </summary>
        Striker = 0x04,
    }

    public class Bars
    {
        public Bar Keeper { get; set; } = new Bar(BarType.Keeper);

        public Bar Defense { get; set; } = new Bar(BarType.Defense);

        public Bar Midfield { get; set; } = new Bar(BarType.Midfield);

        public Bar Striker { get; set; } = new Bar(BarType.Striker);

        // TODO: "All" needed for Calibration ?
    }

    public class Bar
    {
        private List<Player> _players;

        public Bar(BarType barSelection)
        {
            BarSelection = barSelection;

            switch (BarSelection)
            {
                case BarType.All:
                    _players = new List<Player>()
                    {
                        new Player(Player.PlayerType.Keeper, this),
                        new Player(Player.PlayerType.DefenseOne, this),
                        new Player(Player.PlayerType.DefenseTwo, this),
                        new Player(Player.PlayerType.MidfieldOne, this),
                        new Player(Player.PlayerType.MidfieldTwo, this),
                        new Player(Player.PlayerType.MidfieldThree, this),
                        new Player(Player.PlayerType.MidfieldFour, this),
                        new Player(Player.PlayerType.MidfieldFive, this),
                        new Player(Player.PlayerType.StrikerOne, this),
                        new Player(Player.PlayerType.StrikerTwo, this),
                        new Player(Player.PlayerType.StrikerThree, this),
                    };
                    break;
                case BarType.Keeper:
                    _players = new List<Player>() { new Player(Player.PlayerType.Keeper, this) };
                    break;
                case BarType.Defense:
                    _players = new List<Player>()
                    {
                        new Player(Player.PlayerType.DefenseOne, this),
                        new Player(Player.PlayerType.DefenseTwo, this),
                    };
                    break;
                case BarType.Midfield:
                    _players = new List<Player>()
                    {
                        new Player(Player.PlayerType.MidfieldOne, this),
                        new Player(Player.PlayerType.MidfieldTwo, this),
                        new Player(Player.PlayerType.MidfieldThree, this),
                        new Player(Player.PlayerType.MidfieldFour, this),
                        new Player(Player.PlayerType.MidfieldFive, this),
                    };
                    break;
                case BarType.Striker:
                    _players = new List<Player>()
                    {
                        new Player(Player.PlayerType.StrikerOne, this),
                        new Player(Player.PlayerType.StrikerTwo, this),
                        new Player(Player.PlayerType.StrikerThree, this),
                    };
                    break;
                default: throw new ArgumentException("Invalid bar.");
            }
        }

        public BarType BarSelection { get; set; }

        public int Angle { get; set; }

        public double XPosition
        {
            get { return _players.First().XPosition; }
        }

        public List<Player> GetPlayers()
        {
            return _players;
        }

        public Player GetPlayerByPosition(int index)
        {
            return _players.ElementAt(index);
        }

        /// <summary>
        /// Returns the first player of the specified bar.
        /// </summary>
        /// <param name="bar">The instance of bar.</param>
        /// <returns>The first player of the bar.</returns>
        public Player FirstPlayer()
        {
            return _players.First<Player>();
        }

        /// <summary>
        /// Returns the last player of the specified bar.
        /// </summary>
        /// <returns>The last player of the bar.</returns>
        public Player LastPlayer()
        {
            return _players.Last<Player>();
        }

        public string GetLabel()
        {
            switch (BarSelection)
            {
                case BarType.All: return "All";
                case BarType.Keeper: return "K-Bar";
                case BarType.Defense: return "D-Bar";
                case BarType.Midfield: return "M-Bar";
                case BarType.Striker: return "S-Bar";
            }

            throw new ArgumentException("Invalid bar.");
        }

        // TODO: Maybe this can be substituted by some GetNearestPlayer() method

        /// <summary>
        /// Determines whether [is position valid] [the specified bar].
        /// </summary>
        /// <param name="position">The y position for the check.</param>
        /// <returns>
        ///     <c>true</c> if [is position valid] [the specified bar]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsYPositionValid(int position)
        {
            return position >= FirstPlayer().MinPosition &&
                   position <= LastPlayer().MaxPosition;
        }
    }

    public class Player
    {
        private PlayerType _playerType;
        /* Note: Until now it seems to be necessary to access the related bar
           of a player to satisfy the communication-interface */
        private Bar _relatedBar;

        public Player(PlayerType playerType, Bar relatedBar)
        {
            _playerType = playerType;
            _relatedBar = relatedBar;
        }

        /// <summary>
        /// Lists all players and their code for creating datagrams.
        /// </summary>
        public enum PlayerType
        {
            /// <summary>
            /// The keeper.
            /// </summary>
            Keeper = 0x00,

            /// <summary>
            /// The left defender.
            /// </summary>
            DefenseOne = 0x01,

            /// <summary>
            /// The right defender.
            /// </summary>
            DefenseTwo = 0x02,

            /// <summary>
            /// The left midfielder.
            /// </summary>
            MidfieldOne = 0x3,

            /// <summary>
            /// The seconds midfielder.
            /// </summary>
            MidfieldTwo = 0x04,

            /// <summary>
            /// The third midfielder.
            /// </summary>
            MidfieldThree = 0x05,

            /// <summary>
            /// The fourth midfielder.
            /// </summary>
            MidfieldFour = 0x06,

            /// <summary>
            /// The right midfielder.
            /// </summary>
            MidfieldFive = 0x07,

            /// <summary>
            /// The left striker.
            /// </summary>
            StrikerOne = 0x08,

            /// <summary>
            /// The central striker.
            /// </summary>
            StrikerTwo = 0x09,

            /// <summary>
            /// The right striker.
            /// </summary>
            StrikerThree = 0x0A,
        }

        // TODO: Axis?
        public double MinPosition { get; set; }

        public double MaxPosition { get; set; }

        public double XPosition { get; internal set; }

        public double YPosition { get; internal set; }

        public int Angle { get; set; }

        public Bar GetBar()
        {
            return _relatedBar;

            // TODO: Is this return needed ?
            // return (Bar)0xFF;
        }

        // TODO: Set Label at object creation?
        public string GetLabel()
        {
            switch (_playerType)
            {
                case PlayerType.Keeper: return "K";
                case PlayerType.DefenseOne: return "D1";
                case PlayerType.DefenseTwo: return "D2";
                case PlayerType.MidfieldOne: return "M1";
                case PlayerType.MidfieldTwo: return "M2";
                case PlayerType.MidfieldThree: return "M3";
                case PlayerType.MidfieldFour: return "M4";
                case PlayerType.MidfieldFive: return "M5";
                case PlayerType.StrikerOne: return "S1";
                case PlayerType.StrikerTwo: return "S2";
                case PlayerType.StrikerThree: return "S3";
            }

            return "LabelUnset";
        }

        // TODO: Maybe this can later on be substituted by some GetNearestPlayer() method

        /// <summary>
        /// Determines whether [is position valid] [the specified player].
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>
        ///     <c>true</c> if [is position valid] [the specified player]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsYPositionValid(int position)
        {
            return position >= MinPosition &&
                   position <= MaxPosition;
        }
    }
}
