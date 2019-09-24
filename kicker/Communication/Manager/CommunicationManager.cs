namespace Communication.Manager
{
    using System;
    using Calibration;
    using Communication.NetworkLayer;
    using GameProperties;
    using PlayerControl;

    /// <summary>
    /// Manages the existing Communication-Interfaces
    /// </summary>
    public sealed class CommunicationManager : IDisposable
    {

        public IPlayerControl PlayerControl { private set; get; }

        public ICalibrationControl CalibrationControl { private set; get; }

        public INetworkLayer NetworkLayer { private set; get; }

        //internal CommunicationManagerSettings Settings { get; set; }

        /// <summary>
        /// Free allocated resources.
        /// </summary>
        public void Dispose()
        {
            //TODO: Disconnect
        }

        public CommunicationManager(IPlayerControl playerControl, ICalibrationControl calibrationControl, INetworkLayer networkLayer)
        {
            PlayerControl = playerControl;
            CalibrationControl = calibrationControl;
            NetworkLayer = networkLayer;
            
        }

        public void Connect()
        {
           //TODO: Moq
           //NetworkLayer.Connect();
        }

        public void Disconnect()
        {
            //TODO: Moq
            //NetworkLayer.Connect();
        }

        void MovePlayer(Bar bar, ushort newPlayerPosition, bool waitForResponse)
        {
            PlayerControl.MovePlayer(bar, newPlayerPosition, false);
        }

        void SetAngle(Bar bar, short angle, bool waitForResponse)
        {
            PlayerControl.SetAngle(bar, angle, waitForResponse);
        }

        void SetPlayerAnglePass(Bar bar, bool wait = false)
        {
            PlayerControl.SetAngle(bar, -90, wait);
        }

        public void SetPlayerAngleBlock(Bar bar, bool wait = false)
        {
            PlayerControl.SetAngle(bar, 0, wait);
        }
    }
}
