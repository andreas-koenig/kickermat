using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using VideoSource.Dalsa;
using Api.Player;
using Api.UserInterface.Video;
using Api.Video;
using Api.Settings;

namespace Webapp.Player
{
    [KickermatPlayer(
        "Classic Player",
        "This player uses the image processing mechanism and GameController provided by the framework",
        new string[] { "Dominik Hagenauer", "Andreas König" },
        '⚽')
    ]
    public class ClassicPlayer : IKickermatPlayer, IVideoInterface<byte[]>
    {
        private readonly IWriteable<ClassicPlayerSettings> _settings;
        private readonly ILogger _logger;

        private readonly IVideoSource<byte[]> _videoSource;
        private Task _videoTask;
        private CancellationTokenSource _tokenSource;
        private CancellationToken _ct;
        private Mat _img1 = Cv2.ImRead(@"C:\Users\Andreas\Desktop\kicker\kicker_wb_gain_1_6.bmp");
        private Mat _img2 = Cv2.ImRead(@"C:\Users\Andreas\Desktop\kicker\kicker_wb_gain_3.bmp");

        public ClassicPlayer(
            IWriteable<ClassicPlayerSettings> settings,
            IWriteable<DalsaSettings> dalsaSettings,
            ILogger<ClassicPlayer> logger)
        {
            _settings = settings;
            _logger = logger;

            var channels = new VideoChannel[]
            {
                new VideoChannel("Image", "Raw Image"),
                new VideoChannel("EdgeDetection", "Image after EdgeDetection"),
            };

            _videoSource = new Api.Video.VideoSource(channels);
        }

        public void Start()
        {
            _logger.LogInformation("ClassicPlayer started");

            _tokenSource = new CancellationTokenSource();
            _ct = _tokenSource.Token;
            _videoTask = Task.Run(
                async () =>
                {
                    while (true)
                    {
                        if (_ct.IsCancellationRequested)
                        {
                            return;
                        }

                        await Task.Delay(100);
                        var img = _videoSource.Channel.Name.Equals("Image")
                            ? _img1
                            : _img2;
                        (_videoSource as Api.Video.VideoSource).Push(img);
                    }
                },
                _tokenSource.Token);
        }

        public void Stop()
        {
            _logger.LogInformation("ClassicPlayer stopped");
            _tokenSource.Cancel();
        }

        public void Pause()
        {
            _logger.LogInformation("ClassicPlayer paused");
        }

        public void Resume()
        {
            _logger.LogInformation("ClassicPlayer resumed");
        }

        public IVideoSource<byte[]> GetVideoSource()
        {
            return _videoSource;
        }
    }
}
