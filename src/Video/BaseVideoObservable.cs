using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Video
{
    public abstract class BaseVideoObservable<T> : IObservable<T>
    {
        // is atomic: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/variables#atomicity-of-variable-references
        protected bool isAquisitionRunning = false;

        private readonly ConcurrentDictionary<int, IObserver<T>> _observers
            = new ConcurrentDictionary<int, IObserver<T>>();

        protected abstract void StartAcquisition();

        protected abstract void StopAcquisition();

        public IDisposable Subscribe(IObserver<T> observer)
        {
            var hash = observer.GetHashCode();

            if (!_observers.ContainsKey(hash))
            {
                _observers.TryAdd(hash, observer);
            }

            if (_observers.Count > 0 && !isAquisitionRunning)
            {
                StartAcquisition();
            }

            return new Unsubscriber(() =>
            {
                if (observer != null)
                {
                    _observers.TryRemove(observer.GetHashCode(), out var removedObserver);
                }

                if (_observers.Count == 0 && isAquisitionRunning)
                {
                    StopAcquisition();
                }
            });
        }

        protected void Push(T image)
        {
            foreach (var observer in _observers.Values)
            {
                observer.OnNext(image);
            }
        }
    }
}
