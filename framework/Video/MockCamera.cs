using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Resources.Extensions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Api.Camera;
using Api.Periphery;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace Video
{
    public class MockCamera : BaseCamera<MatFrame>
    {
        private const int ImageInterval = 100;
        private readonly ILogger _logger;

        private readonly List<Mat> _images;

        private CancellationTokenSource _tokenSource;
        private CancellationToken _ct;

        public MockCamera(ILogger<MockCamera> logger)
            : base(logger)
        {
            PeripheralState = PeripheralState.Ready;

            _images = LoadImages();
        }

        public override PeripheralState PeripheralState { get; set; }

        public override string Name => "Mock Camera";

        protected override void StartAcquisition()
        {
            _tokenSource = new CancellationTokenSource();
            _ct = _tokenSource.Token;

            int counter = 0;

            Task.Run(
                async () =>
                {
                    while (true)
                    {
                        if (_ct.IsCancellationRequested)
                        {
                            return;
                        }

                        await Task.Delay(ImageInterval).ConfigureAwait(true);

                        // Alternate images so you can see that it is working
                        var img = _images[++counter % _images.Count].Clone();
                        Push(new MatFrame(img));
                    }
                },
                _tokenSource.Token);
        }

        protected override void StopAcquisition()
        {
            _tokenSource.Cancel();
        }

        private List<Mat> LoadImages()
        {
            var images = new List<Mat>();

            try
            {
                var imgResourceStream = GetType().Assembly
                    .GetManifestResourceStream("Video.Properties.Resources.resources");

                using var reader = new DeserializingResourceReader(imgResourceStream);
                foreach (var item in reader)
                {
                    var bitmap = ((DictionaryEntry)item).Value as Bitmap;
                    var mat = BitmapConverter.ToMat(bitmap);
                    images.Add(mat);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not load image resources from assembly: {0}", ex.Message);
            }

            return images;
        }
    }
}
