using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using ImageProcessing.Calibration;
using Microsoft.AspNetCore.SignalR;
using OpenCvSharp;
using VideoSource;

namespace Webapp.Hubs
{
    public enum VideoInput
    {
        CAMERA = 0,
        CAMERA_CALIBRATION = 1
    }

    public class CameraHub : Hub
    {
        private ICameraConnectionHandler _connectionHandler;

        public CameraHub(ICameraConnectionHandler connectionHandler)
        {
            _connectionHandler = connectionHandler;
        }

        public ChannelReader<String> Video(VideoInput input, CancellationToken cancellationToken)
        {
            var channel = Channel.CreateUnbounded<String>();

            try
            {
                _connectionHandler.StartStream(input, Context.ConnectionId, channel,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                var hubException = new HubException(ex.Message);
                channel.Writer.TryComplete(hubException);
            }

            return channel.Reader;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _connectionHandler.AbortConnection(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
