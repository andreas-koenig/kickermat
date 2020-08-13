using System;
using System.Collections.Generic;
using System.Text;
using Api.Periphery;

namespace Api.Camera
{
    public interface ICamera<out T> : IObservable<T>, IPeripheral
        where T : IFrame
    {
        public string Name { get; }
    }
}
