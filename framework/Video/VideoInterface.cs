﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Api.Camera;
using Api.Periphery;
using Api.UserInterface.Video;
using Microsoft.Extensions.Logging;

namespace Video
{
    public class VideoInterface : BaseVideoObservable<IFrame>, IVideoInterface
    {
        private readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();

        private IVideoChannel _channel;
        private IEnumerable<IVideoChannel> _channels;

        public VideoInterface(
            IEnumerable<IVideoChannel> channels, ILoggerFactory loggerFactory, string playerName)
                : base(loggerFactory.CreateLogger($"{playerName}-VideoInterface"))
        {
            _channels = channels;
            _channel = _channels.First();
        }

        public IEnumerable<IVideoChannel> Channels
        {
            get
            {
                _rwLock.EnterReadLock();
                var channels = _channels.Select(ch => ch.Clone() as IVideoChannel);
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

        public IVideoChannel Channel
        {
            get
            {
                _rwLock.EnterReadLock();
                var channel = _channel.Clone() as IVideoChannel;
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

        public new void Push(IFrame image)
        {
            base.Push(image);
        }

        protected override void StartAcquisition()
        {
            // do nothing
        }

        protected override void StopAcquisition()
        {
            // do nothing
        }
    }
}
