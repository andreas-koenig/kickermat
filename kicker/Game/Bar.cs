using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{  
    class Bar
    {
        //TODO: Datatypes for length and angle
        //TODO: Set AccessModifiers etc ...
        /// <summary>
        /// Lists all bars and their code for creating datagrams
        /// </summary>
        public enum Type : ushort
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
        public class Player
        {
            /// <summary>
            /// Lists all players and their code for creating datagrams
            /// </summary>
            public enum Type
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

            private Type playerType;

            //TODO: Axis?
            int minPosition { get; set; }
            int MaxPosition { get; set; }
            int angle { get; set; }

            // Until now it seems to be necessary to access the related bar of an player to satisfy the communication-interface
            private Bar relatedBar;

            public Player(Type playerType, Bar relatedBar)
            {
                this.playerType = playerType;
                this.relatedBar = relatedBar;
            }

            public Bar GetBar()
            {
                return this.relatedBar;

                //TODO: Is this retunr needed ?
                //return (Bar)0xFF;
            }
            public string GetLabel()
            {
                switch (playerType)
                {
                    case Type.Keeper: return "K";
                    case Type.DefenseOne: return "D1";
                    case Type.DefenseTwo: return "D2";
                    case Type.MidfieldOne: return "M1";
                    case Type.MidfieldTwo: return "M2";
                    case Type.MidfieldThree: return "M3";
                    case Type.MidfieldFour: return "M4";
                    case Type.MidfieldFive: return "M5";
                    case Type.StrikerOne: return "S1";
                    case Type.StrikerTwo: return "S2";
                    case Type.StrikerThree: return "S3";
                }

                return "LabelUnset";

            }
        }

        private Type barSelection;
        int position { get; set; }
        private List<Player> players;
        int angle { get; set; }
        
        public Bar(Type barSelection)
        {
            this.barSelection = barSelection;

            switch(this.barSelection)
            {
                case Type.All: players = new List<Player>() { new Player(Player.Type.Keeper, this) , new Player(Player.Type.DefenseOne, this), new Player(Player.Type.DefenseTwo, this), new Player(Player.Type.MidfieldOne, this), new Player(Player.Type.MidfieldTwo, this), new Player(Player.Type.MidfieldThree, this), new Player(Player.Type.MidfieldFour, this), new Player(Player.Type.MidfieldFive, this), new Player(Player.Type.StrikerOne, this), new Player(Player.Type.StrikerTwo, this), new Player(Player.Type.StrikerThree, this) };
                    break;
                case Type.Keeper: players = new List<Player>() { new Player(Player.Type.Keeper, this) };
                    break;
                case Type.Defense: players = new List<Player>() { new Player(Player.Type.DefenseOne, this), new Player(Player.Type.DefenseTwo, this) };
                    break;
                case Type.Midfield: players = new List<Player>() { new Player(Player.Type.MidfieldOne, this), new Player(Player.Type.MidfieldTwo, this), new Player(Player.Type.MidfieldThree, this), new Player(Player.Type.MidfieldFour, this), new Player(Player.Type.MidfieldFive, this) };
                    break;
                case Type.Striker: players = new List<Player>() { new Player(Player.Type.StrikerOne, this), new Player(Player.Type.StrikerTwo, this), new Player(Player.Type.StrikerThree, this) };
                    break;
                default: throw new ArgumentException("Invalid bar.");
            }
        }

        public List<Player> GetPlayers()
        {
            return players;
        }

        /// <summary>
        /// Returns the first player of the specified bar.
        /// </summary>
        /// <param name="bar">The instance of bar.</param>
        public Player FirstPlayer()
        {
            return players.First<Player>();
        }

        /// <summary>
        /// Returns the last player of the specified bar.
        /// </summary>
        /// <returns>The last player of the bar.</returns>
        public Player LastPlayer()
        {
            return players.Last<Player>();
        }
        public string GetLabel()
        {
            switch (barSelection)
            {
                case Type.All: return "All";
                case Type.Keeper: return "K-Bar";
                case Type.Defense: return "D-Bar";
                case Type.Midfield: return "M-Bar";
                case Type.Striker: return "S-Bar";
            }

            throw new ArgumentException("Invalid bar.");
        }
    }
}
