namespace GlobalDataTypes
{
    using System;

    /// <summary>
    /// Auflistung der verfügbaren Stangen.
    /// </summary>
    public enum Bar : ushort
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

    /// <summary>
    /// Class for extension methods to the enumeration Bar.
    /// </summary>
    public static class BarExtensions
    {
        /// <summary>
        /// The list of players on all bars.
        /// </summary>
        private static readonly Player[] ListAllBars = new[] { Player.Keeper, Player.DefenseOne, Player.DefenseTwo, Player.MidfieldOne, Player.MidfieldTwo, Player.MidfieldThree, Player.MidfieldFour, Player.MidfieldFive, Player.StrikerOne, Player.StrikerTwo, Player.StrikerThree };

        /// <summary>
        /// The list of players on the keeper bar.
        /// </summary>
        private static readonly Player[] ListKeeperBar = new[] { Player.Keeper };

        /// <summary>
        /// The list of players on the defense bar.
        /// </summary>
        private static readonly Player[] ListDefenseBar = new[] { Player.DefenseOne, Player.DefenseTwo };

        /// <summary>
        /// The list of players on the midfield bar.
        /// </summary>
        private static readonly Player[] ListMidfieldBar = new[] { Player.MidfieldOne, Player.MidfieldTwo, Player.MidfieldThree, Player.MidfieldFour, Player.MidfieldFive };

        /// <summary>
        /// The list of players on the striker bar.
        /// </summary>
        private static readonly Player[] ListStrikerBar = new[] { Player.StrikerOne, Player.StrikerTwo, Player.StrikerThree };

        /// <summary>
        /// The list of players if an invalid value for the bar was specified.
        /// </summary>
        private static readonly Player[] ListInvalid = new Player[0];

        /// <summary>
        /// Gets the players of a bar.
        /// </summary>
        /// <param name="bar">The selected bar.</param>
        /// <returns>Array with the players of the bar.</returns>
        public static Player[] GetPlayers(this Bar bar)
        {
            switch (bar)
            {
                case Bar.All: return ListAllBars;
                case Bar.Keeper: return ListKeeperBar;
                case Bar.Defense: return ListDefenseBar;
                case Bar.Midfield: return ListMidfieldBar;
                case Bar.Striker: return ListStrikerBar;
            }

            throw new ArgumentException("Invalid bar.");
        }

        /// <summary>
        /// Returns the short name of the specified bar.
        /// </summary>
        /// <param name="bar">The instance of bar.</param>
        /// <returns>The short name of the bar.</returns>
        public static string ShortName(this Bar bar)
        {
            switch (bar)
            {
                case Bar.All: return "All";
                case Bar.Keeper: return "K-Bar";
                case Bar.Defense: return "D-Bar";
                case Bar.Midfield: return "M-Bar";
                case Bar.Striker: return "S-Bar";
            }

            throw new ArgumentException("Invalid bar.");
        }

        /// <summary>
        /// Returns the first player of the specified bar.
        /// </summary>
        /// <param name="bar">The instance of bar.</param>
        /// <returns>The first player of the bar.</returns>
        public static Player FirstPlayer(this Bar bar)
        {
            switch (bar)
            {
                case Bar.All: return Player.Keeper;
                case Bar.Keeper: return Player.Keeper;
                case Bar.Defense: return Player.DefenseOne;
                case Bar.Midfield: return Player.MidfieldOne;
                case Bar.Striker: return Player.StrikerOne;
            }

            throw new ArgumentException("Invalid bar.");
        }

        /// <summary>
        /// Returns the last player of the specified bar.
        /// </summary>
        /// <param name="bar">The instance of bar.</param>
        /// <returns>The last player of the bar.</returns>
        public static Player LastPlayer(this Bar bar)
        {
            switch (bar)
            {
                case Bar.All: return Player.StrikerThree;
                case Bar.Keeper: return Player.Keeper;
                case Bar.Defense: return Player.DefenseTwo;
                case Bar.Midfield: return Player.MidfieldFive;
                case Bar.Striker: return Player.StrikerThree;
            }

            throw new ArgumentException("Invalid bar.");
        }
    }
}
