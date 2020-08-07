using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Api.Periphery;
using Api.Camera;

namespace Video
{
    public abstract class BaseCamera<T> : BaseVideoObservable<T>, ICamera<T>
        where T : IFrame
    {
        private readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();

        public abstract PeripheralState PeripheralState { get; set; }
    }
}
