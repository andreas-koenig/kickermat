using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Webapp.Services;

namespace Webapp.Hubs
{
    public class CalibrationHub : Hub
    {
        private CameraCalibrationService _calibrationService;

        private Channel<int> _channel;
        private CancellationToken _cancellationToken;

        public CalibrationHub(CameraCalibrationService calibrationService)
        {
            _calibrationService = calibrationService;
        }

        public ChannelReader<int> StartCalibration(CancellationToken cancellationToken)
        {
            _channel = Channel.CreateUnbounded<int>();
            _cancellationToken = cancellationToken;
            //_calibrationService.StartCalibration(CalibrationDone, ChessboardRecognized);
            //Context.ConnectionAborted.Register(() => _calibrationService.AbortCalibration());

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
            catch (OperationCanceledException)
            {
                //_cameraCalibration.AbortCalibration();
                _channel.Writer.TryComplete();
            }
        }
    }
}
