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
using Player.Classic.Vision;
using Api.Periphery;
using Motor;
using System.Text;

namespace Player.Classic
{
    public class ClassicPlayer : IKickermatPlayer, IVideoProvider, IObserver<MatFrame>
    {
        private readonly IWriteable<ClassicPlayerSettings> _settings;
        private readonly ILogger _logger;

        private readonly VideoInterface _videoInterface;

        private readonly ClassicImageProcessor _imgProcessor;
        private readonly ICamera<MatFrame> _camera;
        private readonly MotorController _motorController;
        private IDisposable _subscription;

        public ClassicPlayer(
            GenieNanoCamera genieNanoCamera,
            MockCamera mockCamera,
            MotorController motorController,
            IWriteable<ClassicPlayerSettings> settings,
            IWriteable<ClassicImageProcessorSettings> imgProcSettings,
            ILogger<ClassicPlayer> logger,
            ILoggerFactory loggerFactory)
        {
            _settings = settings;
            _logger = logger;
            _imgProcessor = new ClassicImageProcessor(imgProcSettings);
            _motorController = motorController;

            var channels = new VideoChannel[]
            {
                new VideoChannel("Image", "Raw Image"),
                new VideoChannel("EdgeDetection", "Image after EdgeDetection"),
            };

            _videoInterface = new VideoInterface(channels, loggerFactory, nameof(ClassicPlayer));

            if (genieNanoCamera.PeripheralState == PeripheralState.Ready)
            {
                _camera = genieNanoCamera;
            }
            else
            {
                _camera = mockCamera;
            }
        }

        public string Name => "Classic Player";

        public string Description =>
            @"This player uses the image processing mechanism and GameController provided
by the framework. It is not finished due to Corona, but feel free to contribute or use it
as a reference for your own implementation.";

        public string[] Authors => new string[] { "Dominik Hagenauer", "Andreas König" };

        public Rune Emoji => new Rune('⚽');

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
                _videoInterface.Push(new MatFrame(frame.Mat));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failure {}", ex);
            }
        }
    }
}
