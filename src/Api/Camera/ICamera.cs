using System;
using System.Collections.Generic;
using System.Text;
using Api.Periphery;

namespace Api.Camera
{
    public interface ICamera<T> : IObservable<T>, IPeripheral
        where T : IFrame
    {
    }
}
