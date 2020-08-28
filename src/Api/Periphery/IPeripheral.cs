using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Periphery
{
    /// <summary>
    /// This interface has to be implemented by all abstractions for peripheral devices such as
    /// cameras or the PCIe CAN card. It is used to check on the <see cref="PeripheralState"/>
    /// of the particular piece of hardware.
    /// </summary>
    public interface IPeripheral : INamed
    {
        public PeripheralState PeripheralState { get; }
    }
}
