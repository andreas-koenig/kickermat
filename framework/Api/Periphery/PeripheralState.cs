using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Periphery
{
    /// <summary>
    /// This enum indicates the state of an <see cref="IPeripheral" />.
    /// </summary>
    public enum PeripheralState
    {
        DriversNotInstalled = 0,

        /// <summary>
        /// The peripheral is not connected to the current machine.
        /// </summary>
        NotConnected = 1,

        /// <summary>
        /// The peripheral is currently performing an initialization procedure such
        /// as a calibration.
        /// </summary>
        Initializing = 2,

        /// <summary>
        /// The peripheral suffered an unrecoverable error.
        /// </summary>
        Error = 3,

        /// <summary>
        /// The peripheral is ready for use.
        /// </summary>
        Ready = 4,
    }
}
