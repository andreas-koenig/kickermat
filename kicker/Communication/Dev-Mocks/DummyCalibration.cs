namespace Communication.Calibration
{
    using System;
    using Communication.NetworkLayer.Packets.Udp.Enums;
    using Game;

    /// <summary>
    /// Implementation of a dummy calibration call.
    /// </summary>
    public sealed class DummyCalibration : ICalibrationControl
    {
        /// <summary>
        /// Gets the init status.
        /// </summary>
        /// <value>The init status.</value>
        public ControllerStatus InitStatus
        {
            get { return ControllerStatus.Ok; }
        }

        /// <summary>
        /// Moves all bars to their maximum positions.
        /// </summary>
        /// <returns>
        /// true if the operation has been successfully, else false
        /// </returns>
        public void MoveAllBarsToMaximumPosition()
        {
            
        }

        /// <summary>
        /// Moves all bars to their minimum positions.
        /// </summary>
        /// <returns>
        /// true if the operation has been successfully, else false
        /// </returns>
        public void MoveAllBarsToMinimumPosition()
        {
  
        }

        /// <summary>
        /// Sets the bar length in pixel.
        /// </summary>
        /// <param name="selectedBar">The selected bar.</param>
        /// <param name="barLengthInPixel">The bar length in pixel.</param>
        public void SetBarLengthInPixel(Bar selectedBar, ushort barLengthInPixel)
        {
            // Do nothing
        }

        /// <summary>
        /// Sets the bar angle for zero.
        /// </summary>
        /// <param name="selectedBar">The selected bar.</param>
        /// <param name="angle">The angle.</param>
        public void SetBarAngleForZero(Bar selectedBar, int angle)
        {
            // Do nothing
        }

        public void SetAllAnglesAndPositionsToZero()
        {

        }
    }
}
