using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Api.Camera;
using Api.Periphery;
using Microsoft.Extensions.Logging;
using OpenCvSharp;

namespace Video
{
    public class MockCamera : BaseCamera<MatFrame>
    {
        private const int ImageInterval = 100;

        private readonly Mat[] _images = new Mat[]
        {
            Cv2.ImRead(@"C:\Users\Andreas\Desktop\kicker\kicker_wb_gain_1_6.bmp"),
            Cv2.ImRead(@"C:\Users\Andreas\Desktop\kicker\kicker_wb_gain_3.bmp"),
        };

        private CancellationTokenSource _tokenSource;
        private CancellationToken _ct;

        public MockCamera(ILogger<MockCamera> logger)
            : base(logger)
        {
            PeripheralState = PeripheralState.Ready;
        }

        public override PeripheralState PeripheralState { get; set; }

        protected override void StartAcquisition()
        {
            _tokenSource = new CancellationTokenSource();
            _ct = _tokenSource.Token;

            uint counter = 0;

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
                        var img = _images[++counter % _images.Length].Clone();
                        Push(new MatFrame(img));
                    }
                },
                _tokenSource.Token);
        }

        protected override void StopAcquisition()
        {
            _tokenSource.Cancel();
        }
    }
}
