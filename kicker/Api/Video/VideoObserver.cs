using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Video
{
    public class VideoObserver<T> : IObserver<T>
    {
        private Action<T> _onNext;
        private Action _onCompleted;
        private Action<Exception> _onError;

        public VideoObserver(Action<T> onNext, Action? onCompleted, Action<Exception>? onError)
        {
            _onNext = onNext;
            _onCompleted = onCompleted;
            _onError = onError;
        }

        public void OnCompleted()
        {
            _onCompleted?.Invoke();
        }

        public void OnError(Exception error)
        {
            _onError?.Invoke(error);
        }

        public void OnNext(T value)
        {
            _onNext(value);
        }
    }
}
