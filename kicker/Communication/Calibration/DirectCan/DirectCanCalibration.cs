namespace Communication.Calibration.DirectCan
{
    using GlobalDataTypes;
    using NetworkLayer.Packets.Udp.Enums;

    /// <summary>
    /// Ermöglicht eine Kalibrierung über ein direkt an die Motoren angeschlossenes USB-CAN Interface
    /// </summary>
    public class DirectCanCalibration : ICalibrationControl
    {
        /// <summary>
        /// Gets the init status.
        /// </summary>
        /// <value>The init status.</value>
        public ControllerStatus InitStatus
        {
            get
            {
                return ControllerStatus.Error;
            }
        }

        /// <summary>
        /// Moves all bars to their maximum positions.
        /// </summary>
        /// <returns>
        /// true if the operation has been successfully, else false
        /// </returns>
        public ReturnType MoveAllBarsToMaximumPosition()
        {
            return ReturnType.NotOk;
        }

        /// <summary>
        /// Moves all bars to their minimum positions.
        /// </summary>
        /// <returns>
        /// true if the operation has been successfully, else false
        /// </returns>
        public ReturnType MoveAllBarsToMinimumPosition()
        {
            return ReturnType.NotOk;
        }       

        /// <summary>
        /// Sets the bar length in pixel.
        /// </summary>
        /// <param name="selectedBar">The selected bar.</param>
        /// <param name="barLengthInPixel">The bar length in pixel.</param>
        public void SetBarLengthInPixel(Bar selectedBar, ushort barLengthInPixel)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Sets the bar angle for zero.
        /// </summary>
        /// <param name="selectedBar">The selected bar.</param>
        /// <param name="angle">The angle.</param>
        public void SetBarAngleForZero(Bar selectedBar, int angle)
        {
            throw new System.NotImplementedException();
        }

        public ReturnType SetAllAnglesAndPositionsToZero()
        {
            throw new System.NotImplementedException();
        }
    }
}