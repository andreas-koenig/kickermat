using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Player;
using Api.Settings;
using Api.UserInterface.Video;
using Api.Camera;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using Video;
using Video.Dalsa;
using Webapp.Player.Classic.Vision;
using Api.Periphery;

namespace Webapp.Player.Classic
{
    [KickermatPlayer(
        "Classic Player",
        "This player uses the image processing mechanism and GameController provided by the framework",
        new string[] { "Dominik Hagenauer", "Andreas König" },
        '⚽')
    ]
    public class ClassicPlayer : IKickermatPlayer, IVideoProvider, IObserver<MatFrame>
    {
        private readonly IWriteable<ClassicPlayerSettings> _settings;
        private readonly ILogger _logger;

        private readonly VideoInterface _videoInterface;

        private readonly ClassicImageProcessor _imgProcessor;
        private readonly ICamera<MatFrame> _camera;
        private IDisposable _subscription;

        public ClassicPlayer(
            GenieNanoCamera genieNanoCamera,
            MockCamera mockCamera,
            IWriteable<ClassicPlayerSettings> settings,
            IWriteable<ClassicImageProcessorSettings> imgProcSettings,
            ILogger<ClassicPlayer> logger)
        {
            _settings = settings;
            _logger = logger;
            _imgProcessor = new ClassicImageProcessor(imgProcSettings);

            var channels = new VideoChannel[]
            {
                new VideoChannel("Image", "Raw Image"),
                new VideoChannel("EdgeDetection", "Image after EdgeDetection"),
            };

            _videoInterface = new VideoInterface(channels);

            if (genieNanoCamera.PeripheralState == PeripheralState.Ready)
            {
                _camera = genieNanoCamera;
            }
            else
            {
                _camera = mockCamera;
            }
        }

        public IVideoInterface VideoInterface
        {
            get
            {
                return _videoInterface;
            }
        }

        public void Start()
        {
            // Subscribe to VideoSource etc.
            _subscription = _camera.Subscribe(this);
            _logger.LogInformation("ClassicPlayer started");
        }

        public void Stop()
        {
            // Unsubscribe from VideoSource etc.
            _subscription.Dispose();
            _logger.LogInformation("ClassicPlayer stopped");
        }

        public void Pause()
        {
            _logger.LogInformation("ClassicPlayer paused");
        }

        public void Resume()
        {
            _logger.LogInformation("ClassicPlayer resumed");
        }

        public void OnCompleted()
        {
            _subscription.Dispose();
            _logger.LogInformation("VideoSource completed");
        }

        public void OnError(Exception error)
        {
            _logger.LogError("VideoSource had an error {Error}", error);
        }

        public void OnNext(MatFrame frame)
        {
            try
            {
                //var img = _imgProcessor.ProcessFrame(frame.Mat);
                _videoInterface.Push(frame.Mat.ToJpg());
            } catch (Exception ex)
            {
                _logger.LogError("Failure {}", ex);
            }
        }
    }
}
