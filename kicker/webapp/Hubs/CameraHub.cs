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

    public class CameraHub : Hub, IVideoConsumer
    {
        // Inputs
        private IVideoSource _camera;
        private ICameraCalibration _calibration;

        private IVideoSource _selectedSource;

        // SignalR
        private CancellationToken _cancellationToken;
        private Channel<string> _channel;

        public CameraHub(IVideoSource videoSource, ICameraCalibration calibration)
        {
            _camera = videoSource;
            _calibration = calibration;
        }

        public ChannelReader<String> Video(VideoInput input, CancellationToken cancellationToken)
        {
            switch (input)
            {
                case VideoInput.CAMERA:
                    _selectedSource = _camera;
                    break;
                case VideoInput.CAMERA_CALIBRATION:
                    _selectedSource = _calibration;
                    break;
            }

            _channel = Channel.CreateUnbounded<String>();
            _cancellationToken = cancellationToken;

            _selectedSource.StartAcquisition(this);

            return _channel.Reader;
        }

        public async void OnFrameArrived(object sender, FrameArrivedArgs args)
        {
            try
            {
                _cancellationToken.ThrowIfCancellationRequested();
                Context.ConnectionAborted.ThrowIfCancellationRequested();

                byte[] imgBytes;
                Cv2.ImEncode(".jpg", args.Frame.Mat, out imgBytes);
                var base64Img = System.Convert.ToBase64String(imgBytes);
                await _channel.Writer.WriteAsync(base64Img);
            }
            catch (Exception ex)
            {
                _channel.Writer.TryComplete(ex);
                _selectedSource.StopAcquisition(this);
            }
        }

        public async void OnCameraDisconnected(object sender, CameraEventArgs args)
        {
            await Clients.Caller.SendAsync("CameraDisconnected", args.CameraName);
        }

        public async void OnCameraConnected(object sender, CameraEventArgs args)
        {
            await Clients.Caller.SendAsync("CameraConnected", args.CameraName);
        }
    }
}
