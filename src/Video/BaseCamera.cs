using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Api.Camera;
using Api.Periphery;
using Microsoft.Extensions.Logging;

namespace Video
{
    public abstract class BaseCamera<T> : BaseVideoObservable<T>, ICamera<T>
        where T : IFrame
    {
        public BaseCamera(ILogger logger)
            : base(logger)
        {
        }

        public abstract string Name { get; }

        public abstract PeripheralState PeripheralState { get; set; }

        public abstract Guid Id { get; }
    }
}

