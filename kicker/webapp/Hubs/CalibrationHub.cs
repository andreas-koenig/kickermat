using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using ImageProcessing.Calibration;
using Microsoft.AspNetCore.SignalR;

namespace Webapp.Hubs
{
    public class CalibrationHub : Hub
    {
        private ICameraCalibration _cameraCalibration;

        private Channel<int> _channel;
        private CancellationToken _cancellationToken;

        public CalibrationHub(ICameraCalibration calibration)
        {
            this._cameraCalibration = calibration;
        }

        public ChannelReader<int> StartCalibration(CancellationToken cancellationToken)
        {
            _channel = Channel.CreateUnbounded<int>();
            _cancellationToken = cancellationToken;
            _cameraCalibration.StartCalibration(CalibrationDone, ChessboardRecognized);

            return _channel.Reader;
        }

        private async void ChessboardRecognized(int progress)
        {
            try
            {
                _cancellationToken.ThrowIfCancellationRequested();
                Context.ConnectionAborted.ThrowIfCancellationRequested();
                await _channel.Writer.WriteAsync(progress);
            }
            catch (Exception)
            {
                _cameraCalibration.AbortCalibration();
                _channel.Writer.TryComplete();
            }
        }

        private void CalibrationDone(CalibrationResult result)
        {
            // TODO: Save calibration
        }
    }
}
