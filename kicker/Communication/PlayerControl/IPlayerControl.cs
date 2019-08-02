namespace Communication.PlayerControl
{
    using GlobalDataTypes;

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
        void MovePlayer(Bar playerBar, ushort newPlayerPosition, bool waitForResponse);

        /// <summary>
        /// Sets the angle of a bar.
        /// </summary>
        /// <param name="bar">The bar which will be rotated.</param>
        /// <param name="angle">The angle to which the bar is moved (relative to 0).</param>
        void SetAngle(Bar bar, short angle);

        /// <summary>
        /// Sets the angle of a bar.
        /// </summary>
        /// <param name="bar">The bar which will be rotated.</param>
        /// <param name="angle">The angle to which the bar is moved (relative to 0).</param>
        /// <param name="waitForResponse">if set to <c>true</c> [wait for response].</param>
        void SetAngle(Bar bar, short angle, bool waitForResponse);
    }
}