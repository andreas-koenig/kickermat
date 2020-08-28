﻿using System;
using System.Collections.Generic;
using System.Text;
using Api.Periphery;

namespace Api.Camera
{
    /// <summary>
    /// This interface serves as an abstraction for a physical camera. All ICamera implementations
    /// are automatically registered with the framework and provided as services for 
    /// dependency injection.
    /// </summary>
    /// <typeparam name="T">The concrete type of the <see cref="IFrame"/> implementation</typeparam>
    public interface ICamera<out T> : IObservable<T>, IPeripheral
        where T : IFrame
    {
        // Marker interface
        // TODO: implement methods for calibration
    }
}
