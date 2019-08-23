using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using ImageProcessing.Calibration;
using ImageProcessing.Preprocessing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using VideoSource;

namespace Webapp.Hubs
{
    internal class CameraConnectionHandler : ICameraConnectionHandler
    {
        private ILogger<CameraConnectionHandler> _logger;

        private IHubContext<CameraHub> _context;
        private Dictionary<string, ClientVideoConsumer> _clients;

        private IVideoSource _camera;
        private IPreprocessor _preprocessor;

        public CameraConnectionHandler(IHubContext<CameraHub> context, IVideoSource camera,
            ICameraCalibration cameraCalibration, ILogger<CameraConnectionHandler> logger,
            IPreprocessor preprocessor)
        {
            _logger = logger;

            _context = context;
            _clients = new Dictionary<string, ClientVideoConsumer>();

            _camera = camera;
            _preprocessor = preprocessor;
        }

        public void StartStream(VideoInput videoInput, string clientId, Channel<string> channel,
            CancellationToken cancellationToken)
        {
            IVideoSource videoSource;
            switch (videoInput)
            {
                case VideoInput.Camera:
                    videoSource = _camera;
                    break;
                case VideoInput.Preprocessing:
                    videoSource = _preprocessor;
                    break;
                default:
                    videoSource = _camera;
                    break;
            }

            var client = new ClientVideoConsumer(clientId, _context);
            string start = _clients.Remove(clientId) ? "Restarted" : "Started";
            _clients.Add(clientId, client);

            try
            {
                client.StartStream(videoSource, channel, cancellationToken);
                _logger.LogInformation("{} stream for client {}", start, clientId);
            }
            catch (HubException ex)
            {
                _logger.LogInformation("Failed to start stream for client {}", clientId);
                throw ex;
            }
        }

        public void AbortConnection(string clientId)
        {
            _clients[clientId].AbortConnection();
            _clients.Remove(clientId);

            _logger.LogInformation("Aborted connection to client {}", clientId);
        }
    }
}
