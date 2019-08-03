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
    public class CameraHub : Hub
    {
        private IVideoSource _videoSource;

        private CancellationToken _cancellationToken;
        private Channel<string> _channel;

        public CameraHub(IVideoSource videoSource)
        {
            _videoSource = videoSource;
        }

        public ChannelReader<String> Video(CancellationToken cancellationToken)
        {
            _videoSource.FrameArrived += FrameArrivedEventHandler;
            _channel = Channel.CreateUnbounded<String>();
            _cancellationToken = cancellationToken;

            return _channel.Reader;
        }

        private async void FrameArrivedEventHandler(object sender, FrameArrivedArgs args)
        {
            try
            {
                _cancellationToken.ThrowIfCancellationRequested();

                byte[] imgBytes;
                Cv2.ImEncode(".jpg", args.Frame.Mat, out imgBytes);
                var base64Img = System.Convert.ToBase64String(imgBytes);
                await _channel.Writer.WriteAsync(base64Img);
            } catch (Exception ex)
            {
                _videoSource.FrameArrived -= FrameArrivedEventHandler;
                _channel.Writer.TryComplete(ex);
            }
        }

        private async Task WriteItemsAsync(
            ChannelWriter<String> writer,
            CancellationToken cancellationToken)
        {
            try
            {
                for (var i = 0; i < 50; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await writer.WriteAsync(i.ToString());
                }
            }
            catch (Exception ex)
            {
                writer.TryComplete(ex);
            }

            writer.TryComplete();
        }
    }
}
