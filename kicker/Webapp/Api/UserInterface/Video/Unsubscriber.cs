using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapp.Api.UserInterface.Video
{
    public class Unsubscriber<T> : IDisposable
    {
        private ConcurrentDictionary<int, IObserver<T>> _observers;
        private IObserver<T> _observer;

        public Unsubscriber(
            ConcurrentDictionary<int, IObserver<T>> observers, IObserver<T> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_observer != null)
            {
                _observers.TryRemove(_observer.GetHashCode(), out var observer);
            }
        }
    }
}
