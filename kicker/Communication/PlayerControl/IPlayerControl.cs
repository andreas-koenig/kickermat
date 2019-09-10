namespace Communication.PlayerControl
{
    using GameProperties;

    /// <summary>
    /// Interface für die Ansteuerungen der einzelnen Spieler
    /// </summary>
    public interface IPlayerControl
    {
        /// <summary>
        /// Moves a player.
        /// </summary>
        /// <param name="playerBar">The player bar.</param>
        /// <param name="newPlayerPosition">The new player position.</param>
        /// <param name="waitForResponse">if set to <c>true</c> [wait for response].</param>
        void MovePlayer(Bar playerBar, ushort newPlayerPosition, bool waitForResponse = false);

        /// <summary>
        /// Sets the angle of a bar.
        /// </summary>
        /// <param name="bar">The bar which will be rotated.</param>
        /// <param name="angle">The angle to which the bar is moved (relative to 0).</param>
        /// <param name="waitForResponse">if set to <c>true</c> [wait for response].</param>
        void SetAngle(Bar bar, short angle, bool waitForResponse = false);

        /// <summary>
        /// Stellt alle Spieler waagrecht hin, um den Ball passieren zu lassen.
        /// </summary>
        /// <param name="bar">Die <see cref="Bar"/> die passieren lassen soll.</param>
        /// <param name="wait">Gibt an, ob auf die Bestätigung der Positionierung gewartet werden soll</param>
        void SetPlayerAnglePass(Bar bar, bool wait = false);

        /// <summary>
        /// Stellt alle Spieler senkrecht hin, um den Ball zu blocken.
        /// </summary>
        /// <param name="bar">Die <see cref="Bar"/> die Blocken soll.</param>
        /// <param name="wait">Gibt an, ob auf die Bestätigung der Positionierung gewartet werden soll</param>
        void SetPlayerAngleBlock(Bar bar, bool wait = false);

    }
}
