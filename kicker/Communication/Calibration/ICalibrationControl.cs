﻿namespace Communication.Calibration
{
    using GlobalDataTypes;
    using NetworkLayer.Packets.Udp.Enums;

    /// <summary>
    /// Describes the interface which must be used for classes which implement calibration of the image processing
    /// </summary>
    public interface ICalibrationControl
    {
        /// <summary>
        /// Gets the init status.
        /// </summary>
        /// <value>The init status.</value>
        ControllerStatus InitStatus { get; }

        /// <summary>
        /// Moves all bars to their maximum positions.
        /// </summary>
        /// <returns>
        ///     <c>Ok</c> if the operation was successful, else <c>NotOk.</c>
        /// </returns>
        ReturnType MoveAllBarsToMaximumPosition();

        /// <summary>
        /// Moves all bars to their minimum positions.
        /// </summary>
        /// <returns>
        ///     <c>Ok</c> if the operation was successful, else <c>NotOk.</c>
        /// </returns>
        ReturnType MoveAllBarsToMinimumPosition();      
 
        /// <summary>
        /// Sets the bar length in pixel.
        /// </summary>
        /// <param name="selectedBar">The selected bar.</param>
        /// <param name="barLengthInPixel">The bar length in pixel.</param>
        void SetBarLengthInPixel(Bar selectedBar, ushort barLengthInPixel);

        /// <summary>
        /// Sets the bar angle for zero.
        /// </summary>
        /// <param name="selectedBar">The selected bar.</param>
        /// <param name="angle">The angle.</param>
        void SetBarAngleForZero(Bar selectedBar, int angle);

        /// <summary>
        /// Sets a bar to a specified angle.
        /// </summary>
        /// <param name="selectedBar">The selected bar.</param>
        /// <param name="angle">The angle to set.</param>
        /// <returns>
        ///     <c>Ok</c> if the operation was successful, else <c>NotOk.</c>
        /// </returns>
        ReturnType SetAllAnglesAndPositionsToZero();
    }
}