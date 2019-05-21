#if CAN_ENABLED
namespace Communication.PlayerControl.DirectCan
{
    using System;
    using Control;
    using GlobalDataTypes;

    /// <summary>
    /// Class for controlling the players with an directly connected USB2CAN interface
    /// </summary>
    public class DirectCanPlayerControl : IPlayerControl, IDisposable
    {
        /// <summary>
        /// The <see cref="MotorControl"/> instance which is used for sending messages to the motors.
        /// </summary>
        private MotorControl currentMotorControl = new MotorControl();

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Moves the player.
        /// </summary>
        /// <param name="playerBar">The bar which will be moved.</param>
        /// <param name="newPlayerPosition">The new position of the bar.</param>
        public void MovePlayer(Bar playerBar, ushort newPlayerPosition)
        {
        }

        /// <summary>
        /// Moves a player.
        /// </summary>
        /// <param name="playerBar">The player bar.</param>
        /// <param name="newPlayerPosition">The new player position.</param>
        /// <param name="waitForResponse">if set to <c>true</c> [wait for response].</param>
        public void MovePlayer(Bar playerBar, ushort newPlayerPosition, bool waitForResponse)
        {
        }

        /// <summary>
        /// Sets the angle of a bar.
        /// </summary>
        /// <param name="bar">The bar which will be rotated.</param>
        /// <param name="angle">The angle to which the bar is moved (relative to 0).</param>
        public void SetAngle(Bar bar, short angle)
        {
        }

        /// <summary>
        /// Sets the angle of a bar.
        /// </summary>
        /// <param name="bar">The bar which will be rotated.</param>
        /// <param name="angle">The angle to which the bar is moved (relative to 0).</param>
        /// <param name="waitForResponse">if set to <c>true</c> [wait for response].</param>
        public void SetAngle(Bar bar, short angle, bool waitForResponse)
        {
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.currentMotorControl != null)
                {
                    this.currentMotorControl.Dispose();
                }
            }
        }               
    }
}
#endif