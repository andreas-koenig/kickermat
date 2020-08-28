using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Api.Periphery;
using Microsoft.Extensions.Logging;

namespace Motor
{
    public class MotorController : IPeripheral
    {
        private readonly ILogger _logger;

        // We have reference our callback method through member variables to
        // bind them to the lifetime of the GenieNanoCamera object. Otherwise
        // they could be garbage collected before.
        private readonly CanInterfaceDll.CalibrationDone _calibrationDone;

        private readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();

        private IntPtr _apiHandle = IntPtr.Zero;
        private PeripheralState _state = PeripheralState.Initializing;

        public MotorController(ILogger<MotorController> logger)
        {
            _logger = logger;
            _calibrationDone = CalibrationDone;

            Initialize();
        }

        ~MotorController()
        {
            if (!_apiHandle.Equals(IntPtr.Zero))
            {
                CanInterfaceDll.Destroy(_apiHandle);
            }
        }

        public PeripheralState PeripheralState
        {
            get
            {
                PeripheralState state;
                _rwLock.EnterReadLock();
                state = _state;
                _rwLock.ExitReadLock();
                return state;
            }

            set
            {
                _rwLock.EnterWriteLock();
                _state = value;
                _rwLock.ExitWriteLock();
            }
        }

        public string Name => "Motors";

        public MotorDevice[] CollectDiagnostics()
        {
            return Diagnostics.Collect();
        }

        public void MoveBar(Bar bar, uint position, uint angle)
        {
            if (PeripheralState == PeripheralState.Ready)
            {
                CanInterfaceDll.MoveBar(_apiHandle, bar, (int)position, (int)(angle % 360));
            }
        }

        public void ShiftBar(Bar bar, uint position)
        {
            CanInterfaceDll.MoveBar(_apiHandle, bar, (int)position, -1);
        }

        public void RotateBar(Bar bar, uint angle)
        {
            CanInterfaceDll.MoveBar(_apiHandle, bar, -1, (int)angle);
        }

        protected void Initialize()
        {
            try
            {
                _apiHandle = CanInterfaceDll.Init();
            }
            catch (DllNotFoundException)
            {
                _logger.LogError("Failed to initialize motors: NTCAN drivers not installed");

                PeripheralState = PeripheralState.DriversNotInstalled;
                return;
            }

            if (_apiHandle.Equals(IntPtr.Zero))
            {
                PeripheralState = PeripheralState.NotConnected;
                _logger.LogInformation("No physical CAN interface found");
            }
            else
            {
                _logger.LogInformation("Starting motor calibration...");
                PeripheralState = PeripheralState.Initializing;
                CanInterfaceDll.StartCalibration(_apiHandle, _calibrationDone);
            }
        }

        protected void CalibrationDone(uint minPos, uint maxPos)
        {
            _logger.LogInformation("Motors successfully calibrated");
            PeripheralState = PeripheralState.Ready;
        }
    }
}
