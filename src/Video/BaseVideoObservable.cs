using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Api.Camera;
using Microsoft.Extensions.Logging;

namespace Video
{
    public abstract class BaseVideoObservable<T> : IObservable<T>
        where T : IFrame
    {
        private readonly ILogger _logger;

        private readonly ConcurrentDictionary<int, IObserver<T>> _observers
            = new ConcurrentDictionary<int, IObserver<T>>();

        // is atomic: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/variables#atomicity-of-variable-references
        private bool _isAcquisitionRunning = false;

        public BaseVideoObservable(ILogger logger)
        {
            _logger = logger;
        }

        protected bool IsAcquisitionRunning
        {
            get => _isAcquisitionRunning;
            private set => _isAcquisitionRunning = value;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            var hash = observer.GetHashCode();

            if (!_observers.ContainsKey(hash))
            {
                _observers.TryAdd(hash, observer);

                _logger.LogDebug($"New observer (total: {_observers.Count}");
            }

            if (_observers.Count > 0 && !_isAcquisitionRunning)
            {
                _isAcquisitionRunning = true;
                StartAcquisition();
                _logger.LogInformation("Acquisition started");
            }

            return new Unsubscriber(() =>
            {
                if (observer != null)
                {
                    _observers.TryRemove(observer.GetHashCode(), out var removedObserver);
                }

                if (_observers.Count == 0 && _isAcquisitionRunning)
                {
                    StopAcquisition();
                    _isAcquisitionRunning = false;
                    _logger.LogInformation("Acquisition stopped");
                }
            });
        }

        protected void Push(T image)
        {
            foreach (var observer in _observers.Values)
            {
                ThreadPool.QueueUserWorkItem((state) => observer.OnNext((T)image.Clone()));
            }
        }

        protected abstract void StartAcquisition();

        protected abstract void StopAcquisition();
    }
}
