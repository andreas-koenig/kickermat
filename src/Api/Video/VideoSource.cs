using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OpenCvSharp;
using Api.UserInterface;
using Api.UserInterface.Video;

namespace Api.Video
{
    public class VideoSource : IVideoSource<byte[]>
    {
        private readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();
        private VideoChannel _channel;
        private IEnumerable<VideoChannel> _channels;

        private ConcurrentDictionary<int, IObserver<byte[]>> _observers
            = new ConcurrentDictionary<int, IObserver<byte[]>>();

        private ImageEncodingParam _imgEncoding
            = new ImageEncodingParam(ImwriteFlags.JpegQuality, 50);

        public VideoSource(IEnumerable<VideoChannel> channels)
        {
            _channels = channels;
            _channel = _channels.First();
        }

        public string Type { get; } = "VideoInterface";

        public IEnumerable<VideoChannel> Channels
        {
            get
            {
                _rwLock.EnterReadLock();
                var channels = _channels.Select(ch => ch.Clone() as VideoChannel);
                _rwLock.ExitReadLock();
                return channels;
            }

            protected set
            {
                _rwLock.EnterWriteLock();
                _channels = value;
                _rwLock.ExitWriteLock();
            }
        }

        public VideoChannel Channel
        {
            get
            {
                _rwLock.EnterReadLock();
                var channel = _channel.Clone() as VideoChannel;
                _rwLock.ExitReadLock();
                return channel;
            }

            set
            {
                _rwLock.EnterWriteLock();
                _channel = value;
                _rwLock.ExitWriteLock();
            }
        }

        public IDisposable Subscribe(IObserver<byte[]> observer)
        {
            var hash = observer.GetHashCode();

            if (!_observers.ContainsKey(hash))
            {
                _observers.TryAdd(hash, observer);
            }

            return new Unsubscriber<byte[]>(_observers, observer);
        }

        public void Push(Mat image)
        {
            var bytes = image.ImEncode(".jpg", _imgEncoding);
            foreach (var observer in _observers.Values)
            {
                observer.OnNext(bytes);
            }
        }
    }
}
