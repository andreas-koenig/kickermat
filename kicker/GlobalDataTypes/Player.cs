namespace GlobalDataTypes
{
    using System.Collections.Generic;

    /// <summary>
    /// Alle verfügbaren Spieler. Von links nach rechts wenn man hinter eigenem Tor steht.
    /// </summary>
    public enum Player
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

    /// <summary>
    /// Class for extension methods to the enumeration Player.
    /// </summary>
    public static class PlayerExtensions
    {
        /// <summary>
        /// Returns the bar the specified player belongs to.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>The bar the player belongs to.</returns>
        public static Bar GetBar(this Player player)
        {
            switch (player)
            {
                case Player.Keeper: return Bar.Keeper;
                case Player.DefenseOne: return Bar.Defense;
                case Player.DefenseTwo: return Bar.Defense;
                case Player.MidfieldOne: return Bar.Midfield;
                case Player.MidfieldTwo: return Bar.Midfield;
                case Player.MidfieldThree: return Bar.Midfield;
                case Player.MidfieldFour: return Bar.Midfield;
                case Player.MidfieldFive: return Bar.Midfield;
                case Player.StrikerOne: return Bar.Striker;
                case Player.StrikerTwo: return Bar.Striker;
                case Player.StrikerThree: return Bar.Striker;
            }

            return (Bar)0xFF;
        }

        /// <summary>
        /// Returns the short name of the specified player.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>The short name od the player.</returns>
        public static string ShortName(this Player player)
        {
            switch (player)
            {
                case Player.Keeper: return "K";
                case Player.DefenseOne: return "D1";
                case Player.DefenseTwo: return "D2";
                case Player.MidfieldOne: return "M1";
                case Player.MidfieldTwo: return "M2";
                case Player.MidfieldThree: return "M3";
                case Player.MidfieldFour: return "M4";
                case Player.MidfieldFive: return "M5";
                case Player.StrikerOne: return "S1";
                case Player.StrikerTwo: return "S2";
                case Player.StrikerThree: return "S3";
            }

            return player.ToString();
        }
    }
}