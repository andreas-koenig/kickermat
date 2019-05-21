namespace Coach
{
    using GlobalDataTypes;

    /// <summary>
    /// Interface which must be implemented for a coach.
    /// </summary>
    public interface ICoach
    {
        int CameraLongZ { get; }
        int PlayerShortZ { get; }
        int BallShortZ { get; }
        /// <summary>
        /// Gets the width of the playing field.
        /// </summary>
        int PlayingFieldWidth { get; }

        /// <summary>
        /// Gets the height of the playing field.
        /// </summary>
        int PlayingFieldHeight { get; }

        /// <summary>
        /// Gets the goal top.
        /// </summary>
        int GoalTop { get; }

        /// <summary>
        /// Gets the goal bottom.
        /// </summary>
        int GoalBottom { get; }

        /// <summary>
        /// Gets the goal area top.
        /// </summary>
        int GoalAreaTop { get; }

        /// <summary>
        /// Gets the goal area bottom.
        /// </summary>
        int GoalAreaBottom { get; }

        /// <summary>
        /// Gets the penalty box top.
        /// </summary>
        int PenaltyBoxTop { get; }

        /// <summary>
        /// Gets the penalty box bottom.
        /// </summary>
        int PenaltyBoxBottom { get; }

        /// <summary>
        /// Gets the position of the top left corner of the playing field.
        /// </summary>
        Position PlayingFieldOffset { get; }

        /// <summary>
        /// Gets the position of the center of the playing field.
        /// </summary>
        Position PlayingFieldCenter { get; }

        /// <summary>
        /// Moves the player to y position.
        /// </summary>
        /// <param name="player">The player to move.</param>
        /// <param name="positionY">The desired y position.</param>
        void MovePlayerToYPosition(Player player, int positionY);

        /// <summary>
        /// Moves the player to y position.
        /// </summary>
        /// <param name="player">The player to move.</param>
        /// <param name="positionY">The desired y position.</param>
        /// <param name="wait">if set to <c>true</c> wait until finished.</param>
        void MovePlayerToYPosition(Player player, int positionY, bool wait);

        /// <summary>
        /// Setz den Spielerwinkel für die entsprechende Stange.
        /// </summary>
        /// <param name="bar">Die Stange deren Winkel gesetzt werden soll.</param>
        /// <param name="angle">Der zu setzende Winkel.</param>
        void SetPlayerAngle(Bar bar, short angle);

        /// <summary>
        /// Setz den Spielerwinkel für die entsprechende Stange.
        /// </summary>
        /// <param name="bar">Die Stange deren  Winkel gesetzt werden soll.</param>
        /// <param name="angle">Der zu setzende Winkel.</param>
        /// <param name="wait">Blocking auf Antwort warten.</param>
        void SetPlayerAngle(Bar bar, short angle, bool wait);

        /// <summary>
        /// Stellt alle Spieler senkrecht hin, um den Ball zu blocken.
        /// </summary>
        /// <param name="bar">Die <see cref="Bar"/> die Blocken soll.</param>
        void SetPlayerAngleBlock(Bar bar);

        /// <summary>
        /// Stellt alle Spieler senkrecht hin, um den Ball zu blocken.
        /// </summary>
        /// <param name="bar">Die <see cref="Bar"/> die Blocken soll.</param>
        /// <param name="wait">Gibt an, ob auf die Bestätigung der Positionierung gewartet werden soll</param>
        void SetPlayerAngleBlock(Bar bar, bool wait);

        /// <summary>
        /// Stellt alle Spieler waagrecht hin, um den Ball passieren zu lassen.
        /// </summary>
        /// <param name="bar">Die <see cref="Bar"/> die passieren lassen soll.</param>
        void SetPlayerAnglePass(Bar bar);

        /// <summary>
        /// Stellt alle Spieler waagrecht hin, um den Ball passieren zu lassen.
        /// </summary>
        /// <param name="bar">Die <see cref="Bar"/> die passieren lassen soll.</param>
        /// <param name="wait">Gibt an, ob auf die Bestätigung der Positionierung gewartet werden soll</param>
        void SetPlayerAnglePass(Bar bar, bool wait);

        /// <summary>
        /// Determines whether an y position is valid for the specified bar.
        /// </summary>
        /// <param name="bar">The bar to check.</param>
        /// <param name="position">The y position.</param>
        /// <returns>
        /// <c>true</c> if the y position is valid for the specified bar; otherwise <c>false</c>.
        /// </returns>
        bool IsYPositionValid(Bar bar, int position);

        /// <summary>
        /// Determines whether an y position is valid for the specified player.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="position">The y position.</param>
        /// <returns>
        /// <c>true</c> if the y position is valid for the specified player; otherwise <c>false</c>.
        /// </returns>
        bool IsYPositionValid(Player player, int position);

        /// <summary>
        /// Gets the bar x position.
        /// </summary>
        /// <param name="bar">The bar whose x position is requested.</param>
        /// <returns>The bar x position.</returns>
        int GetBarXPosition(Bar bar);

        /// <summary>
        /// Gets the player minimum y position.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>The min position of the player.</returns>
        int GetPlayerMinYPosition(Player player);

        /// <summary>
        /// Gets the player maximum y position.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>The max position of the player.</returns>
        int GetPlayerMaxYPosition(Player player);

        /// <summary>
        /// Gets the y distance between players.
        /// </summary>
        /// <param name="firstPlayer">The first player.</param>
        /// <param name="secondPlayer">The second player.</param>
        /// <returns>The y distance between first and second player.</returns>
        int GetYDistanceBetweenPlayers(Player firstPlayer, Player secondPlayer);
    }
}
