using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OpenCvSharp;
using VideoSource;

namespace Webapp.Hubs
{
    internal class ClientVideoConsumer : IVideoConsumer
    {
        private IVideoSource _videoSource;

        private string _clientId;
        private IHubContext<CameraHub> _context;
        private Channel<string> _channel;
        private CancellationToken _cancellationToken;

        public ClientVideoConsumer(string clientId, IHubContext<CameraHub> context)
        {
            _clientId = clientId;
            _context = context;
        }

        public void StartStream(IVideoSource videoSource, Channel<string> channel,
            CancellationToken cancellationToken)
        {
            _videoSource = videoSource;
            _channel = channel;
            _cancellationToken = cancellationToken;

            try
            {
                _videoSource.StartAcquisition(this);
            }
            catch (VideoSourceException ex)
            {
                _videoSource.StopAcquisition(this);
                var hubException = new HubException(ex.Message);
                _channel.Writer.TryComplete(hubException);
            }
        }

        public void AbortConnection()
        {
            _videoSource.StopAcquisition(this);
        }

        public void OnCameraConnected(object sender, CameraEventArgs args)
        {
            _context.Clients.Client(_clientId).SendAsync("CameraConnected", args.CameraName);
        }

        public void OnCameraDisconnected(object sender, CameraEventArgs args)
        {
            _context.Clients.Client(_clientId).SendAsync("CameraDisconnected", args.CameraName);
        }

        public async void OnFrameArrived(object sender, FrameArrivedArgs args)
        {
            try
            {
                _cancellationToken.ThrowIfCancellationRequested();

                byte[] imgBytes;
                Cv2.ImEncode(".jpg", args.Frame.Mat, out imgBytes);
                var base64Img = System.Convert.ToBase64String(imgBytes);

                await _channel.Writer.WriteAsync(base64Img);
            }
            catch (Exception ex)
            {
                _videoSource.StopAcquisition(this);
                _channel.Writer.TryComplete(ex);
            }
        }
    }
}
