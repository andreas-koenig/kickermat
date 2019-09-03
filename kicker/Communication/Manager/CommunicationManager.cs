namespace Communication.Manager
{
    using System;
    using Calibration;
    using PlayerControl;
    using Sets;

    /// <summary>
    /// Manages the existing Communication-Interfaces
    /// </summary>
    public sealed class CommunicationManager : IDisposable, ICommunicationSet
    {
        /// <summary>
        /// Occurs when [communication set changed].
        /// </summary>
        public event EventHandler<NewCommunicationSetEventArgs> CommunicationSetChanged;        

        /// <summary>
        /// Gets the currently used communication set.
        /// </summary>
        public ICommunicationSet CommunicationSet { get; private set; }

        /// <summary>
        /// Gets the player control.
        /// </summary>
        /// <value>The player control.</value>
        public IPlayerControl PlayerControl => CommunicationSet.PlayerControl;

        /// <summary>
        /// Gets the calibration control.
        /// </summary>
        /// <value>The calibration control.</value>
        public ICalibrationControl CalibrationControl => CommunicationSet.CalibrationControl;

        /// <summary>
        /// Gets or sets the Settings.
        /// </summary>
        /// <value>The Settings.</value>
        internal CommunicationManagerSettings Settings { get; set; }

        /// <summary>
        /// Free allocated resources.
        /// </summary>
        public void Dispose()
        {

        }

        /// <summary>
        /// Inits the communication set.
        /// </summary>
        /// <param name="newCommunicationSet">The new communication set.</param>
        internal void InitCommunicationSet(ICommunicationSet newCommunicationSet)
        {
            this.Settings.CommunicationSet = newCommunicationSet.GetType();
            this.CommunicationSet = newCommunicationSet;

            if (this.CommunicationSetChanged != null)
            {
                this.CommunicationSetChanged(this, new NewCommunicationSetEventArgs(newCommunicationSet));
            }
        }

        public void Connect()
        {
            CommunicationSet.Connect();
        }

        public void Disconnect()
        {
            CommunicationSet.Disconnect();
        }
    }
}
