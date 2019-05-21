#if CAN_ENABLED
namespace Communication.PlayerControl.DirectCan.Abstraction
{
    using System;
    using System.Collections.Generic;
    using BasicCanOpen;
    using GlobalDataTypes;

    /// <summary>
    /// Enumeration of all NMT states
    /// </summary>
    public enum NmtStateType
    {
        /// <summary>
        /// Node in unknown state.
        /// </summary>
        MotorStateUnknown = 0,

        /// <summary>
        /// Node stopped.
        /// </summary>
        MotorStateNodeStopped = 4,
        
        /// <summary>
        /// Node in operational state.
        /// </summary>
        MotorStateOperational = 5,
        
        /// <summary>
        /// Node in pre-operational state.
        /// </summary>
        MotorStatePreOperational = 127
    }

    /// <summary>
    /// Enumeration of all homing methods.
    /// </summary>
    public enum MotorHomingMethodType
    {
        /// <summary>
        /// Unknown homing method.
        /// </summary>
        MotorHomingMethodUnknown,
       
        /// <summary>
        /// Homing by Maßsetzen.
        /// </summary>
        MotorHomingMethodMasssetzen = 0x23
    }

    /// <summary>
    /// Enumeration of all feature states.
    /// </summary>
    public enum MotorFeatureStateType
    {
        /// <summary>
        /// State of feature is unknown.
        /// </summary>
        MotorFeatureStateUnknown  = 0x2,
       
        /// <summary>
        /// Feature is active.
        /// </summary>
        MotorFeatureStateActive     = 0x1,
        
        /// <summary>
        /// Feature ist not active.
        /// </summary>
        MotorFeatureStateInactive = 0x0
    }

    /// <summary>
    /// Enumeration of all motor opearation modes.
    /// </summary>
    public enum MotorOperationModeType
    {
        /// <summary>
        /// Operation mode unknown.
        /// </summary>
        MotorOperationModeUnknown = 0,

        /// <summary>
        /// Operation mode point to point.
        /// </summary>
        MotorOperationModePointToPoint = 0x01,
    
        /// <summary>
        /// Operation mode velocity profile.
        /// </summary>
        MotorOperationModeVelocityProfile = 0x03,
        
        /// <summary>
        /// Operation mode referencing.
        /// </summary>
        MotorOperationModeReferencing = 0x06,
    }

    /// <summary>
    /// Enumeration of all motor operation sattes.
    /// </summary>
    public enum MotorOperationStatusType
    {
        /// <summary>
        /// Motor is ready to switch on.
        /// </summary>
        MotorOperationStatusNotReadyToSwitchOn,
        
        /// <summary>
        /// Motor is in state opeartion enabled.
        /// </summary>
        MotorOperationStatusOperationEnabled,
        
        /// <summary>
        /// Motor is in state quick stop acitve.
        /// </summary>
        MotorOperationStatusQuickStopActive,
        
        /// <summary>
        /// Motor is ready to switch on.
        /// </summary>
        MotorOperationStatusReadyToSwitchOn,
        
        /// <summary>
        /// Motor is in state switch on disabled.
        /// </summary>
        MotorOperationStatusSwitchOnDisabled,
        
        /// <summary>
        /// Motor is switched on.
        /// </summary>
        MotorOperationStatusSwitchedOn,
        
        /// <summary>
        /// Motor is in fault state.
        /// </summary>
        MotorOperationStatusFault,
        
        /// <summary>
        /// Motor state is unknown.
        /// </summary>
        MotorOperationStatusUnknown
    }

    /// <summary>
    /// Enumeration of all positioning methods.
    /// </summary>
    public enum MotorPositioningMethodType
    {
        /// <summary>
        /// Position is set absolute to zero point.
        /// </summary>
        MotorPositioningMethodAbsolute,
        
        /// <summary>
        /// Position is set absolute to current position.
        /// </summary>
        MotorPositioningMethodRelative
    }

    /// <summary>
    /// Enumeration of all motor roation directions.
    /// </summary>
    public enum MotorRotationDirection
    {
        /// <summary>
        /// Rotate motor clockwise.
        /// </summary>
        Clockwise = 0,
        
        /// <summary>
        /// Rotate motor anti clockwise.
        /// </summary>
        AntiClockwise = 1,
        
        /// <summary>
        /// Rotate direction is unknown.
        /// </summary>
        Unknown = 2
    }

    /// <summary>
    /// Abstraction of motor control functions from CANopen
    /// </summary>
    public class MotorAbstraction : IDisposable
    {        
        /// <summary>
        /// Number of available motors
        /// </summary>
        private const int NumberOfAvailableMotors = 8;              
        
        /// <summary>
        /// Value for R PDO2 enabled.
        /// </summary>
        private const uint ReceivePdo2Enabled = 0x00000300;
        
        /// <summary>
        /// Value for R PDO2 disabled.
        /// </summary>
        private const uint ReceivePdo2Disabled = 0x80000300;        
     
        /// <summary>
        /// Value for T PDO2 enabled.
        /// </summary>
        private const uint TransmitPdo2Enabled = 0x00000280;
     
        /// <summary>
        /// Value for T PDO2 disabled.
        /// </summary>
        private const uint TransmitPdo2Disabled = 0x80000280;

        /// <summary>
        /// Value for enable min position limit.
        /// </summary>
        private const int MinPositionSoftLimitEnabled = 0x0002;
      
        /// <summary>
        /// Value for disable min position limit.
        /// </summary>
        private const int MinPositionSoftLimitDisabled = 0x0000;
    
        /// <summary>
        /// Value for enable max position limit.
        /// </summary>
        private const int MaxPositionSoftLimitEnabled = 0x0001;
      
        /// <summary>
        /// Value for disable max position limit.
        /// </summary>
        private const int MaxPositionSoftLimitDisabled = 0x0000;

        /// <summary>
        /// Bit mask for ready to switch on of status word.
        /// </summary>
        private const ushort DcomStatusWordReadyToSwitchOn = 0x0001 << 0;
   
        /// <summary>
        /// Bit mask for switch on of status word.
        /// </summary>
        private const ushort DcomStatusWordSwitchedOn = 0x0001 << 1;
   
        /// <summary>
        /// Bit mask for operation enable of status word.
        /// </summary>
        private const ushort DcomStatusWordOperationEnabled = 0x0001 << 2;
    
        /// <summary>
        /// Bit mask for fault of status word.
        /// </summary>
        private const ushort DcomStatusWordFault = 0x0001 << 3;
   
        /// <summary>
        /// Bit mask for quick stop of status word.
        /// </summary>
        private const ushort DcomStatusWordQuickStopActive = 0x0001 << 5;
     
        /// <summary>
        /// Bit mask for switch on disable of status word.
        /// </summary>
        private const ushort DcomStatusWordSwitchOnDisabled = 0x0001 << 6;

        /// <summary>
        /// Bit mask for target reaches of status word.
        /// </summary>
        private const ushort DcomStatusWordTargetReached = 0x0001 << 10;
   
        /// <summary>
        /// Bit mask for x end of status word.
        /// </summary>
        private const ushort DcomStatusWordSetHardNotifyForHoming = 0x0001 << 14;

        /// <summary>
        /// Bit mask for switch on of control word.
        /// </summary>
        private const ushort DcomControlWordSwitchOn = 0x0001 << 0;
  
        /// <summary>
        /// Bit mask for enable voltage of control word.
        /// </summary>
        private const ushort DcomControlWordEnableVoltage = 0x0001 << 1;
  
        /// <summary>
        /// Bit mask for quick stop of control word.
        /// </summary>
        private const ushort DcomControlWordQuickStop = 0x0001 << 2;
   
        /// <summary>
        /// Bit mask for enable operation of control word.
        /// </summary>
        private const ushort DcomControlWordEnableOperation = 0x0001 << 3;
  
        /// <summary>
        /// Bit mask for new setpoint of control word.
        /// </summary>
        private const ushort DcomControlWordNewSetpoint = 0x0001 << 4;
    
        /// <summary>
        /// Bit mask for change set immediately of control word.
        /// </summary>
        private const ushort DcomControlWordChangeSetImmediately = 0x0001 << 5;
   
        /// <summary>
        /// Bit mask for positioning relative of control word.
        /// </summary>
        private const ushort DcomControlWordPositioningRelative = 0x0001 << 6;
    
        /// <summary>
        /// Bit mask for fault reset of control word.
        /// </summary>
        private const ushort DcomControlWordFaultReset = 0x0001 << 7;
      
        /// <summary>
        /// Bit mask for halt of control word.
        /// </summary>
        private const ushort DcomControlWordHalt = 0x0001 << 8;

        /// <summary>
        /// Dictionary which contains all used CanOpen objects with its index and subindex
        /// </summary>
        private Dictionary<MotorAbstractionObject, MotorAbstractionObjectEntry> objectDirectory;

        /// <summary>
        /// Used CANopen instance for communication
        /// </summary>
        private CanOpen usedCanOpenInterface = new CanOpen();

        /// <summary>
        /// Initializes a new instance of the <see cref="MotorAbstraction"/> class.
        /// </summary>
        public MotorAbstraction()
        {
            this.Init();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }       

        /// <summary>
        /// Gets the state of the motor NMT.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <returns>The motor NMT state.</returns>
        public NmtStateType GetMotorNmtState(byte motorNumber)
        {
            byte nmtState;
            NmtStateType returnValue;
            if (this.usedCanOpenInterface.SendNmtRequest(motorNumber, out nmtState) == ReturnType.Ok)
            {
                switch (nmtState)
                {
                    case (byte)NmtStateType.MotorStateNodeStopped:
                        returnValue = NmtStateType.MotorStateNodeStopped;
                        break;
                    case (byte)NmtStateType.MotorStateOperational:
                        returnValue = NmtStateType.MotorStateOperational;
                        break;
                    case (byte)NmtStateType.MotorStatePreOperational:
                        returnValue = NmtStateType.MotorStatePreOperational;
                        break;
                    default:
                        throw new MotorAbstractionException(MotorAbstractionError.UnknownNmtState);
                        
                        // break;
                }
            }
            else
            {
                returnValue = NmtStateType.MotorStateUnknown;
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the motor homing method.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <returns>The motor homing method.</returns>
        public MotorHomingMethodType GetMotorHomingMethod(byte motorNumber)
        {
            int homingMethod;
            MotorHomingMethodType returnValue = MotorHomingMethodType.MotorHomingMethodUnknown;
            ushort index = this.objectDirectory[MotorAbstractionObject.TelemecaniqueHomingMethod].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.TelemecaniqueHomingMethod].SubIndex;

            if (this.usedCanOpenInterface.SdoRead(motorNumber, index, subIndex, out homingMethod) == ReturnType.Ok)
            {
                switch (homingMethod)
                {
                    case (int)MotorHomingMethodType.MotorHomingMethodMasssetzen:
                        returnValue = MotorHomingMethodType.MotorHomingMethodMasssetzen;
                        break;
                    default:
                        throw new MotorAbstractionException(MotorAbstractionError.UnknownHomingMethod);
                }
            }
            else
            {
                returnValue = MotorHomingMethodType.MotorHomingMethodUnknown;
            }

            return returnValue;
        }

        /// <summary>
        /// Sets the motor homing method.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <param name="homingMethod">The homing method.</param>
        /// <returns>
        ///     <c>Ok</c> if setting homing method was successfully, else <c>NotOk</c>
        /// </returns>
        public ReturnType SetMotorHomingMethod(byte motorNumber, MotorHomingMethodType homingMethod)
        {
            int writeResponse;
            byte homingMethodValue = 0;
            switch (homingMethod)
            {
                case MotorHomingMethodType.MotorHomingMethodMasssetzen:
                    homingMethodValue = (byte)MotorHomingMethodType.MotorHomingMethodMasssetzen;
                    break;
                case MotorHomingMethodType.MotorHomingMethodUnknown:
                    throw new MotorAbstractionException(MotorAbstractionError.UnknownHomingMethod);
            }

            ushort index = this.objectDirectory[MotorAbstractionObject.TelemecaniqueHomingMethod].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.TelemecaniqueHomingMethod].SubIndex;
            return this.usedCanOpenInterface.SdoWrite1Bytes(motorNumber, index, subIndex, homingMethodValue, out writeResponse);
        }

        /// <summary>
        /// Sets the position for masssetzen (telemecanique motor).
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <param name="position">The position.</param>
        /// <returns>
        ///     <c>Ok</c> if setting position was successfully, else <c>NotOk</c>
        /// </returns>
        public ReturnType SetPositionForMasssetzenTelemecanique(byte motorNumber, int position)
        {
            int writeResponse;
            ushort index = this.objectDirectory[MotorAbstractionObject.TelemecaniquePositionForMasssetzen].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.TelemecaniquePositionForMasssetzen].SubIndex;
            return this.usedCanOpenInterface.SdoWrite4Bytes(motorNumber, index, subIndex, position, out writeResponse);
        }

        /// <summary>
        /// Gets the state of the motor RPDO2.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <returns>The state of RPDO2</returns>
        public MotorFeatureStateType GetMotorReceivePdo2State(byte motorNumber)
        {
            // RoRe: Node-ID noch ausmaskieren und in switch-case umwandeln
            int rpdo2State;
            ushort index = this.objectDirectory[MotorAbstractionObject.ReceivePdo2Parameters].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.ReceivePdo2Parameters].SubIndex;
            if (this.usedCanOpenInterface.SdoRead(motorNumber, index, subIndex, out rpdo2State) == ReturnType.Ok)
            {
                if ((rpdo2State & ReceivePdo2Disabled) == ReceivePdo2Disabled)
                {
                    // Zuerst disabled-Fall behandeln, da werden mehr Bits verwendet :-)
                    return MotorFeatureStateType.MotorFeatureStateInactive;
                }
                else if ((rpdo2State & ReceivePdo2Enabled) == ReceivePdo2Enabled)
                {
                    return MotorFeatureStateType.MotorFeatureStateActive;
                }
                else
                {
                    return MotorFeatureStateType.MotorFeatureStateUnknown;
                }
            }
            else
            {
                return MotorFeatureStateType.MotorFeatureStateUnknown;
            }
        }

        /// <summary>
        /// Sets the state of the motor RPD o2_.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <param name="rpdo2State">State of the RPD o2_.</param>
        /// <returns>
        ///     <c>Ok</c> if the operation was successful, else <c>NotOk.</c>
        /// </returns>
        public ReturnType SetMotorReceivePdo2State(byte motorNumber, MotorFeatureStateType rpdo2State)
        {
            int writeResponse;
            uint rpdo2StateValue = 0;
            switch (rpdo2State)
            {
                case MotorFeatureStateType.MotorFeatureStateActive:
                    rpdo2StateValue = ReceivePdo2Enabled | motorNumber;
                    break;
                case MotorFeatureStateType.MotorFeatureStateInactive:
                    rpdo2StateValue = ReceivePdo2Disabled | motorNumber;
                    break;
                case MotorFeatureStateType.MotorFeatureStateUnknown:
                    throw new MotorAbstractionException(MotorAbstractionError.UnknownRpdo2State);
            }

            ushort index = this.objectDirectory[MotorAbstractionObject.ReceivePdo2Parameters].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.ReceivePdo2Parameters].SubIndex;
            return this.usedCanOpenInterface.SdoWrite4Bytes(motorNumber, index, subIndex, rpdo2StateValue, out writeResponse);
        }

        /// <summary>
        /// Sets the state of the motor TPD o2_.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <param name="transmitPdo2State">State of the transmit pdo2.</param>
        /// <returns>
        ///     <c>Ok</c> if the operation was successful, else <c>NotOk.</c>
        /// </returns>
        public ReturnType SetMotorTransmitPdo2State(byte motorNumber, MotorFeatureStateType transmitPdo2State)
        {
            int writeResponse;
            uint transmitPdo2StateValue = 0;
            switch (transmitPdo2State)
            {
                case MotorFeatureStateType.MotorFeatureStateActive:
                    transmitPdo2StateValue = TransmitPdo2Enabled | motorNumber;
                    break;
                case MotorFeatureStateType.MotorFeatureStateInactive:
                    transmitPdo2StateValue = TransmitPdo2Disabled | motorNumber;
                    break;
                case MotorFeatureStateType.MotorFeatureStateUnknown:
                    throw new MotorAbstractionException(MotorAbstractionError.UnknownTpdo2State);
            }

            ushort index = this.objectDirectory[MotorAbstractionObject.TransmitPdo2Parameters].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.TransmitPdo2Parameters].SubIndex;
            return this.usedCanOpenInterface.SdoWrite4Bytes(motorNumber, index, subIndex, transmitPdo2StateValue, out writeResponse);
        }

        /// <summary>
        /// Sets the motor minimum position limit.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <param name="positionLimit">The position limit.</param>
        /// <returns>
        ///     <c>Ok</c> if the operation was successful, else <c>NotOk.</c>
        /// </returns>
        public ReturnType SetMotorMinimumPositionLimit(byte motorNumber, int positionLimit)
        {
            int writeResponse;
            ushort index = this.objectDirectory[MotorAbstractionObject.MinPositionLimit].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.MinPositionLimit].SubIndex;
            return this.usedCanOpenInterface.SdoWrite4Bytes(motorNumber, index, subIndex, positionLimit, out writeResponse);
        }

        /// <summary>
        /// Sets the motor maximum position limit.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <param name="positionLimit">The position limit.</param>
        /// <returns>
        ///     <c>Ok</c> if the operation was successful, else <c>NotOk.</c>
        /// </returns>
        public ReturnType SetMotorMaximumPositionLimit(byte motorNumber, int positionLimit)
        {
            int writeResponse;
            ushort index = this.objectDirectory[MotorAbstractionObject.MaxPositionLimit].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.MaxPositionLimit].SubIndex;
            return this.usedCanOpenInterface.SdoWrite4Bytes(motorNumber, index, subIndex, positionLimit, out writeResponse);
        }

        /// <summary>
        /// Gets the state of the motor TPD o2_.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <returns>
        ///     <c>Ok</c> if the operation was successful, else <c>NotOk.</c>
        /// </returns>
        public MotorFeatureStateType GetMotorTransmitPdo2State(byte motorNumber)
        {
            // RoRe: Node-ID noch ausmaskieren und in switch-case umwandeln
            int transmitPdo2State;
            ushort index = this.objectDirectory[MotorAbstractionObject.TransmitPdo2Parameters].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.TransmitPdo2Parameters].SubIndex;
            if (this.usedCanOpenInterface.SdoRead(motorNumber, index, subIndex, out transmitPdo2State) == ReturnType.Ok)
            {
                if ((transmitPdo2State & ReceivePdo2Disabled) == ReceivePdo2Disabled)
                {
                    // Zuerst disabled-Fall behandeln, da werden mehr Bits verwendet :-)
                    return MotorFeatureStateType.MotorFeatureStateInactive;
                }
                else if ((transmitPdo2State & TransmitPdo2Enabled) == TransmitPdo2Enabled)
                {
                    return MotorFeatureStateType.MotorFeatureStateActive;
                }
                else
                {
                    return MotorFeatureStateType.MotorFeatureStateUnknown;
                }
            }
            else
            {
                return MotorFeatureStateType.MotorFeatureStateUnknown;
            }
        }

        /// <summary>
        /// Gets the motor minimum position limit.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <param name="readValue">The read value.</param>
        /// <returns>
        ///     <c>Ok</c> if the operation was successful, else <c>NotOk.</c>
        /// </returns>
        public ReturnType GetMotorMinimumPositionLimit(byte motorNumber, out int readValue)
        {
            ushort index = this.objectDirectory[MotorAbstractionObject.MinPositionLimit].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.MinPositionLimit].SubIndex;
            return this.usedCanOpenInterface.SdoRead(motorNumber, index, subIndex, out readValue);
        }

        /// <summary>
        /// Sets the rotation direction.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>
        ///     <c>Ok</c> if the operation was successful, else <c>NotOk.</c>
        /// </returns>
        public ReturnType SetRotationDirection(byte motorNumber, MotorRotationDirection direction)
        {
            int writeResponse;
            ushort index = this.objectDirectory[MotorAbstractionObject.TelemecaniqueRotationDirection].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.TelemecaniqueRotationDirection].SubIndex;
            return this.usedCanOpenInterface.SdoWrite2Bytes(motorNumber, index, subIndex, (ushort)direction, out writeResponse);
        }

        /// <summary>
        /// Gets the rotation direction.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <returns>
        ///     <c>Ok</c> if the operation was successful, else <c>NotOk.</c>
        /// </returns>
        public MotorRotationDirection GetRotationDirection(byte motorNumber)
        {
            // RoRe: Node-ID noch ausmaskieren und in switch-case umwandeln
            int rotationDirection;
            MotorRotationDirection returnValue;
            ushort index = this.objectDirectory[MotorAbstractionObject.TelemecaniqueRotationDirection].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.TelemecaniqueRotationDirection].SubIndex;
            if (this.usedCanOpenInterface.SdoRead(motorNumber, index, subIndex, out rotationDirection) == ReturnType.Ok)
            {
                switch (rotationDirection)
                {
                    case (int)MotorRotationDirection.Clockwise:
                        returnValue = MotorRotationDirection.Clockwise;
                        break;
                    case (int)MotorRotationDirection.AntiClockwise:
                        returnValue = MotorRotationDirection.AntiClockwise;
                        break;
                    default:
                        returnValue = MotorRotationDirection.Unknown;
                        break;
                }
            }
            else
            {
                returnValue = MotorRotationDirection.Unknown;
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the state of the motor minimum position limit.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <returns>
        ///     <c>Ok</c> if the operation was successful, else <c>NotOk.</c>
        /// </returns>
        public MotorFeatureStateType GetMotorMinimumPositionLimitState(byte motorNumber)
        {            
            int minimumPositionSoftLimitState;
            MotorFeatureStateType returnValue;
            ushort index = this.objectDirectory[MotorAbstractionObject.TelemecaniqueSoftLimitParameters].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.TelemecaniqueSoftLimitParameters].SubIndex;
            if (this.usedCanOpenInterface.SdoRead(motorNumber, index, subIndex, out minimumPositionSoftLimitState) == ReturnType.Ok)
            {
                minimumPositionSoftLimitState &= MinPositionSoftLimitEnabled;
                switch (minimumPositionSoftLimitState)
                {
                    case MinPositionSoftLimitDisabled:
                        returnValue = MotorFeatureStateType.MotorFeatureStateInactive;
                        break;
                    case MinPositionSoftLimitEnabled:
                        returnValue = MotorFeatureStateType.MotorFeatureStateActive;
                        break;
                    default:
                        throw new MotorAbstractionException(MotorAbstractionError.UnknownMinPositionLimitState);
                }
            }
            else
            {
                returnValue = MotorFeatureStateType.MotorFeatureStateUnknown;
            }

            return returnValue;
        }

        /// <summary>
        /// Sets the state of the motor minimum position limit.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        ///     <c>Ok</c> if the operation was successful, else <c>NotOk.</c>
        /// </returns>
        public ReturnType SetMotorMinimumPositionLimitState(byte motorNumber, MotorFeatureStateType state)
        {
            int writeResponse;
            int positionSoftLimitState;
            ushort index = this.objectDirectory[MotorAbstractionObject.TelemecaniqueSoftLimitParameters].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.TelemecaniqueSoftLimitParameters].SubIndex;
            if (this.usedCanOpenInterface.SdoRead(motorNumber, index, subIndex, out positionSoftLimitState) == ReturnType.Ok)
            {
                switch (state)
                {
                    case MotorFeatureStateType.MotorFeatureStateInactive:
                        positionSoftLimitState &= ~MinPositionSoftLimitEnabled;
                        break;
                    case MotorFeatureStateType.MotorFeatureStateActive:
                        positionSoftLimitState |= MinPositionSoftLimitEnabled;
                        break;
                    default:
                        // nix machen, Wert unverändert zurückschreiben und Fehler melden
                        throw new MotorAbstractionException(MotorAbstractionError.UnknownMinPositionLimitState);
                }

                return this.usedCanOpenInterface.SdoWrite2Bytes(
                    motorNumber,
                    index,
                    subIndex,
                    positionSoftLimitState, 
                    out writeResponse);
            }
            else
            {
                return ReturnType.NotOk;
            }
        }

        /// <summary>
        /// Gets the state of the motor maximum position limit.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <returns>The state.</returns>
        public MotorFeatureStateType GetMotorMaximumPositionLimitState(byte motorNumber)
        {
            int maximumPositionSoftLimitState;
            MotorFeatureStateType returnValue;
            ushort index = this.objectDirectory[MotorAbstractionObject.TelemecaniqueSoftLimitParameters].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.TelemecaniqueSoftLimitParameters].SubIndex;
            if (this.usedCanOpenInterface.SdoRead(motorNumber, index, subIndex, out maximumPositionSoftLimitState) == ReturnType.Ok)
            {
                maximumPositionSoftLimitState &= MaxPositionSoftLimitEnabled;
                switch (maximumPositionSoftLimitState)
                {
                    case MaxPositionSoftLimitDisabled:
                        returnValue = MotorFeatureStateType.MotorFeatureStateInactive;
                        break;
                    case MaxPositionSoftLimitEnabled:
                        returnValue = MotorFeatureStateType.MotorFeatureStateActive;
                        break;
                    default:
                        throw new MotorAbstractionException(MotorAbstractionError.UnknownMaxPositionLimitState);
                }
            }
            else
            {
                returnValue = MotorFeatureStateType.MotorFeatureStateUnknown;
            }

            return returnValue;
        }

        /// <summary>
        /// Sets the state of the motor maximum position limit.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        ///     <c>Ok</c> if the operation was successful, else <c>NotOk.</c>
        /// </returns>
        public ReturnType SetMotorMaximumPositionLimitState(byte motorNumber, MotorFeatureStateType state)
        {
            int writeResponse;
            int positionSoftLimitState;
            ushort index = this.objectDirectory[MotorAbstractionObject.TelemecaniqueSoftLimitParameters].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.TelemecaniqueSoftLimitParameters].SubIndex;
            if (this.usedCanOpenInterface.SdoRead(motorNumber, index, subIndex, out positionSoftLimitState) == ReturnType.Ok)
            {
                switch (state)
                {
                    case MotorFeatureStateType.MotorFeatureStateInactive:
                        positionSoftLimitState &= ~MaxPositionSoftLimitEnabled;
                        break;
                    case MotorFeatureStateType.MotorFeatureStateActive:
                        positionSoftLimitState |= MaxPositionSoftLimitEnabled;
                        break;
                    default:
                        // nix machen, Wert unverändert zurückschreiben und Fehler melden
                        throw new MotorAbstractionException(MotorAbstractionError.UnknownMaxPositionLimitState);
                }

                return this.usedCanOpenInterface.SdoWrite2Bytes(
                    motorNumber,
                    index,
                    subIndex,
                    positionSoftLimitState, 
                    out writeResponse);
            }
            else
            {
                return ReturnType.NotOk;
            }
        }

        /// <summary>
        /// Gets the motor maximum position limit.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <param name="maximumPosition">The maximum position.</param>
        /// <returns>
        ///     <c>Ok</c> if the operation was successful, else <c>NotOk.</c>
        /// </returns>
        public ReturnType GetMotorMaximumPositionLimit(byte motorNumber, out int maximumPosition)
        {
            ushort index = this.objectDirectory[MotorAbstractionObject.MaxPositionLimit].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.MaxPositionLimit].SubIndex;
            return this.usedCanOpenInterface.SdoRead(motorNumber, index, subIndex, out maximumPosition);
        }

        /// <summary>
        /// Gets the motor position.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <param name="position">The position.</param>
        /// <returns>
        ///     <c>Ok</c> if the operation was successful, else <c>NotOk.</c>
        /// </returns>
        public ReturnType GetMotorPosition(byte motorNumber, out int position)
        {
            ushort index = this.objectDirectory[MotorAbstractionObject.PositionAtPositionInterface].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.PositionAtPositionInterface].SubIndex;
            return this.usedCanOpenInterface.SdoRead(motorNumber, index, subIndex, out position);
        }

        /// <summary>
        /// Gets the motor belastung.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <param name="load">The motor load.</param>
        /// <returns><c>Ok</c> if the operation was successful, else <c>NotOk.</c></returns>
        public ReturnType GetMotorLoad(byte motorNumber, out int load)
        {
            ushort index = this.objectDirectory[MotorAbstractionObject.MotorLoad].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.MotorLoad].SubIndex;
            return this.usedCanOpenInterface.SdoRead(motorNumber, index, subIndex, out load);
        }

        /// <summary>
        /// Gets the motor operation mode.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <returns><c>Ok</c> if the operation was successful, else <c>NotOk.</c></returns>
        public MotorOperationModeType GetMotorOperationMode(byte motorNumber)
        {
            int operationMode;
            MotorOperationModeType returnValue;
            ushort index = this.objectDirectory[MotorAbstractionObject.OperationModeDisplay].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.OperationModeDisplay].SubIndex;
            if (this.usedCanOpenInterface.SdoRead(motorNumber, index, subIndex, out operationMode) == ReturnType.Ok)
            {
                switch (operationMode)
                {
                    case (int)MotorOperationModeType.MotorOperationModePointToPoint:
                        returnValue = MotorOperationModeType.MotorOperationModePointToPoint;
                        break;
                    case (int)MotorOperationModeType.MotorOperationModeVelocityProfile:
                        returnValue = MotorOperationModeType.MotorOperationModeVelocityProfile;
                        break;
                    case (int)MotorOperationModeType.MotorOperationModeReferencing:
                        returnValue = MotorOperationModeType.MotorOperationModeReferencing;
                        break;
                    default:
                        throw new MotorAbstractionException(MotorAbstractionError.UnknownOperationMode);
                }
            }
            else
            {
                returnValue = MotorOperationModeType.MotorOperationModeUnknown;
            }

            return returnValue;
        }

        /// <summary>
        /// Sets the motor TM scale.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <param name="numerator">The numerator.</param>
        /// <param name="denom">The denom.</param>
        /// <returns>
        ///     <c>Ok</c> if the operation was successful, else <c>NotOk.</c>
        /// </returns>
        public ReturnType SetMotorTMScale(byte motorNumber, int numerator, int denom)
        {
            // Wichtig: zuerst Nenner, dann Zähler
            int writeResponse;
            ushort index = this.objectDirectory[MotorAbstractionObject.TelemecaniquePositionScaleDenom].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.TelemecaniquePositionScaleDenom].SubIndex;
            ushort index2 = this.objectDirectory[MotorAbstractionObject.TelemecaniquePositionScaleNum].Index;
            byte subIndex2 = this.objectDirectory[MotorAbstractionObject.TelemecaniquePositionScaleNum].SubIndex;
            if ((this.usedCanOpenInterface.SdoWrite4Bytes(motorNumber, index, subIndex, denom, out writeResponse) == ReturnType.Ok) &&
                (this.usedCanOpenInterface.SdoWrite4Bytes(motorNumber, index2, subIndex2, numerator, out writeResponse) == ReturnType.Ok))
            {
                return ReturnType.Ok;
            }
            else
            {
                return ReturnType.NotOk;
            }
        }

        /// <summary>
        /// Sets the motor position factor numerator.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <param name="numerator">The numerator.</param>
        /// <returns>
        ///     <c>Ok</c> if the operation was successful, else <c>NotOk.</c>
        /// </returns>
        public ReturnType SetMotorPositionFactorNumerator(byte motorNumber, int numerator)
        {
            int writeResponse;
            ushort index = this.objectDirectory[MotorAbstractionObject.FaulhaberPositionScaleNumerator].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.FaulhaberPositionScaleNumerator].SubIndex;
            return this.usedCanOpenInterface.SdoWrite4Bytes(motorNumber, index, subIndex, numerator, out writeResponse);
            
            // while ( this.usedCanOpenInterface.SdoRead(motorNumber,TARGET_POSITION_FACTOR_INDEX, TARGET_POSITION_FACTOR_NUMERATOR_SUBINDEX) != numerator);
            // while ( this.usedCanOpenInterface.SdoRead(motorNumber,TARGET_POSITION_FACTOR_INDEX, TARGET_POSITION_FACTOR_FEED_CONSTANT_SUBINDEX) != FeedConstant);
        }

        /// <summary>
        /// Sets the motor position factor feed constant.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <param name="feedConstant">The feed constant.</param>
        /// <returns><c>Ok</c> if the operation was successful, else <c>NotOk.</c></returns>
        public ReturnType SetMotorPositionFactorFeedConstant(byte motorNumber, int feedConstant)
        {
            int writeResponse;
            ushort index = this.objectDirectory[MotorAbstractionObject.FaulhaberPositionScaleFeedConstant].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.FaulhaberPositionScaleFeedConstant].SubIndex;
            return this.usedCanOpenInterface.SdoWrite4Bytes(motorNumber, index, subIndex, feedConstant, out writeResponse);
        }

        /// <summary>
        /// Gets the motor position factor numerator.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <param name="numerator">The numerator.</param>
        /// <returns>
        ///     <c>Ok</c> if the operation was successful, else <c>NotOk.</c>
        /// </returns>
        public ReturnType GetMotorPositionFactorNumerator(byte motorNumber, out int numerator)
        {
            ushort index = this.objectDirectory[MotorAbstractionObject.FaulhaberPositionScaleNumerator].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.FaulhaberPositionScaleNumerator].SubIndex;
            return this.usedCanOpenInterface.SdoRead(motorNumber, index, subIndex, out numerator);
        }

        /// <summary>
        /// Gets the motor position factor feed constant.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <param name="feedConstant">The feed constant.</param>
        /// <returns><c>Ok</c> if the operation was successful, else <c>NotOk.</c></returns>
        public ReturnType GetMotorPositionFactorFeedConstant(byte motorNumber, out int feedConstant)
        {
            ushort index = this.objectDirectory[MotorAbstractionObject.FaulhaberPositionScaleFeedConstant].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.FaulhaberPositionScaleFeedConstant].SubIndex;
            return this.usedCanOpenInterface.SdoRead(motorNumber, index, subIndex, out feedConstant);
        }

        /// <summary>
        /// Sets the motor operation mode.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <param name="operationMode">The operation mode.</param>
        /// <returns><c>Ok</c> if the operation was successful, else <c>NotOk.</c></returns>
        public ReturnType SetMotorOperationMode(byte motorNumber, MotorOperationModeType operationMode)
        {
            byte operationModeValue = 0;
            int writeResponse;
            switch (operationMode)
            {
                case MotorOperationModeType.MotorOperationModePointToPoint:
                    operationModeValue = (byte)MotorOperationModeType.MotorOperationModePointToPoint;
                    break;
                case MotorOperationModeType.MotorOperationModeVelocityProfile:
                    operationModeValue = (byte)MotorOperationModeType.MotorOperationModeVelocityProfile;
                    break;
                case MotorOperationModeType.MotorOperationModeReferencing:
                    operationModeValue = (byte)MotorOperationModeType.MotorOperationModeReferencing;
                    break;
                default:
                    throw new MotorAbstractionException(MotorAbstractionError.UnknownOperationMode);
            }

            ushort index = this.objectDirectory[MotorAbstractionObject.OperationMode].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.OperationMode].SubIndex;
            return this.usedCanOpenInterface.SdoWrite1Bytes(motorNumber, index, subIndex, operationModeValue, out writeResponse);
        }

        /// <summary>
        /// Gets the motor operation status.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <returns>
        ///     <c>Ok</c> if the operation was successful, else <c>NotOk.</c>
        /// </returns>
        public MotorOperationStatusType GetMotorOperationStatus(byte motorNumber)
        {
            ushort statusWord;
            int readValue;
            MotorOperationStatusType returnValue = MotorOperationStatusType.MotorOperationStatusUnknown;
            ushort index = this.objectDirectory[MotorAbstractionObject.StatusWord].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.StatusWord].SubIndex;
            if (this.usedCanOpenInterface.SdoRead(motorNumber, index, subIndex, out readValue) == ReturnType.Ok)
            {
                statusWord = (ushort)readValue;
                /*if (((statusWord & DcomStatusWordSetHardNotifyForHoming) != DcomStatusWordSetHardNotifyForHoming) && (motorNumber < 11))
                {//wenn X_End nicht aktiv ist, ist der status unbekannt, geht aber nicht bei Faulhaber Motoren, desewegen MotorNumre < 11 
                    return MotorOperationStatusUnknown;
                } */
                if (((statusWord & DcomStatusWordReadyToSwitchOn) == 0x0000) &&
                    ((statusWord & DcomStatusWordSwitchedOn) == 0x0000) &&
                    ((statusWord & DcomStatusWordOperationEnabled) == 0x0000) &&
                    ((statusWord & DcomStatusWordFault) == 0x0000) &&
                    ((statusWord & DcomStatusWordSwitchOnDisabled) == 0x0000))
                {
                    // Ready to switch on (Bit 0) muss 0 sein, Switch on (Bit 1) muss 0 sein, Operation enalbe (Bit 2) muss 0 sein,
                    // Fault (Bit 3) muss 0 sein, Switch on disable (Bit 6) muss 0 sein
                    return MotorOperationStatusType.MotorOperationStatusNotReadyToSwitchOn;
                }
                else if (((statusWord & DcomStatusWordReadyToSwitchOn) == 0x0000) &&
                         ((statusWord & DcomStatusWordSwitchedOn) == 0x0000) &&
                         ((statusWord & DcomStatusWordOperationEnabled) == 0x0000) &&
                         ((statusWord & DcomStatusWordFault) == 0x0000) &&
                         ((statusWord & DcomStatusWordSwitchOnDisabled) == DcomStatusWordSwitchOnDisabled))
                {
                    // Ready to switch on (Bit 0) muss 0 sein, Switch on (Bit 1) muss 0 sein, Operation enalbe (Bit 2) muss 0 sein,
                    // Fault (Bit 3) muss 0 sein, Switch on disable (Bit 6) muss 1 sein
                    return MotorOperationStatusType.MotorOperationStatusSwitchOnDisabled;
                }
                else if (((statusWord & DcomStatusWordReadyToSwitchOn) == DcomStatusWordReadyToSwitchOn) &&
                         ((statusWord & DcomStatusWordSwitchedOn) == 0x0000) &&
                         ((statusWord & DcomStatusWordOperationEnabled) == 0x0000) &&
                         ((statusWord & DcomStatusWordFault) == 0x0000) &&
                         ((statusWord & DcomStatusWordQuickStopActive) == DcomStatusWordQuickStopActive) &&
                         ((statusWord & DcomStatusWordSwitchOnDisabled) == 0x0000))
                {
                    // Ready to switch on (Bit 0) muss 1 sein, Switch on (Bit 1) muss 0 sein, Operation enalbe (Bit 2) muss 0 sein,
                    // Fault (Bit 3) muss 0 sein, Quick stop (Bit 5) muss 1 sein, Switch on disable (Bit 6) muss 0 sein
                    return MotorOperationStatusType.MotorOperationStatusReadyToSwitchOn;
                }
                else if (((statusWord & DcomStatusWordReadyToSwitchOn) == DcomStatusWordReadyToSwitchOn) &&
                         ((statusWord & DcomStatusWordSwitchedOn) == DcomStatusWordSwitchedOn) &&
                         ((statusWord & DcomStatusWordOperationEnabled) == 0x0000) &&
                         ((statusWord & DcomStatusWordFault) == 0x0000) &&
                         ((statusWord & DcomStatusWordQuickStopActive) == DcomStatusWordQuickStopActive) &&
                         ((statusWord & DcomStatusWordSwitchOnDisabled) == 0x0000))
                {
                    // Ready to switch on (Bit 0) muss 1 sein, Switch on (Bit 1) muss 1 sein, Operation enalbe (Bit 2) muss 0 sein,
                    // Fault (Bit 3) muss 0 sein, Quick stop (Bit 5) muss 1 sein, Switch on disable (Bit 6) muss 0 sein
                    return MotorOperationStatusType.MotorOperationStatusSwitchedOn;
                }
                else if (((statusWord & DcomStatusWordReadyToSwitchOn) == DcomStatusWordReadyToSwitchOn) &&
                         ((statusWord & DcomStatusWordSwitchedOn) == DcomStatusWordSwitchedOn) &&
                         ((statusWord & DcomStatusWordOperationEnabled) == DcomStatusWordOperationEnabled) &&
                         ((statusWord & DcomStatusWordFault) == 0x0000) &&
                         ((statusWord & DcomStatusWordQuickStopActive) == DcomStatusWordQuickStopActive) &&
                         ((statusWord & DcomStatusWordSwitchOnDisabled) == 0x0000))
                {
                    // Ready to switch on (Bit 0) muss 1 sein, Switch on (Bit 1) muss 1 sein, Operation enalbe (Bit 2) muss 1 sein,
                    // Fault (Bit 3) muss 0 sein, Quick stop (Bit 5) muss 1 sein, Switch on disable (Bit 6) muss 0 sein
                    return MotorOperationStatusType.MotorOperationStatusOperationEnabled;
                }
                else if (((statusWord & DcomStatusWordReadyToSwitchOn) == DcomStatusWordReadyToSwitchOn) &&
                         ((statusWord & DcomStatusWordSwitchedOn) == DcomStatusWordSwitchedOn) &&
                         ((statusWord & DcomStatusWordOperationEnabled) == DcomStatusWordOperationEnabled) &&
                         ((statusWord & DcomStatusWordFault) == 0x0000) &&
                         ((statusWord & DcomStatusWordQuickStopActive) == 0x0000) &&
                         ((statusWord & DcomStatusWordSwitchOnDisabled) == 0x0000))
                {
                    // Ready to switch on (Bit 0) muss 1 sein, Switch on (Bit 1) muss 1 sein, Operation enalbe (Bit 2) muss 1 sein,
                    // Fault (Bit 3) muss 0 sein, Quick stop (Bit 5) muss 0 sein, Switch on disable (Bit 6) muss 0 sein
                    return MotorOperationStatusType.MotorOperationStatusQuickStopActive;
                }
                else if (((statusWord & DcomStatusWordReadyToSwitchOn) == DcomStatusWordReadyToSwitchOn) &&
                         ((statusWord & DcomStatusWordSwitchedOn) == DcomStatusWordSwitchedOn) &&
                         ((statusWord & DcomStatusWordOperationEnabled) == DcomStatusWordOperationEnabled) &&
                         ((statusWord & DcomStatusWordFault) == DcomStatusWordFault) &&
                         ((statusWord & DcomStatusWordSwitchOnDisabled) == 0x0000))
                {
                    // Ready to switch on (Bit 0) muss 1 sein, Switch on (Bit 1) muss 1 sein, Operation enalbe (Bit 2) muss 1 sein
                    // Fault (Bit 3) muss 1 sein, Switch on disable (Bit 6) muss 0 sein
                    return MotorOperationStatusType.MotorOperationStatusFault;
                }
                else
                {
                    throw new MotorAbstractionException(MotorAbstractionError.UnknownOperationState);
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Sets the target position.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <param name="position">The position.</param>
        /// <returns>
        ///     <c>Ok</c> if the operation was successful, else <c>NotOk.</c>
        /// </returns>
        public ReturnType SetTargetPosition(byte motorNumber, int position)
        {
            int writeResponse;
            ushort index = this.objectDirectory[MotorAbstractionObject.TargetPositionPointToPoint].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.TargetPositionPointToPoint].SubIndex;
            return this.usedCanOpenInterface.SdoWrite4Bytes(motorNumber, index, subIndex, (uint)position, out writeResponse);
        }

        /// <summary>
        /// Gets the motor target reached.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <returns><c>Ok</c> if the operation was successful, else <c>NotOk.</c></returns>
        public bool GetMotorTargetReached(byte motorNumber)
        {
            ushort statusWord;
            int readValue;
            ushort index = this.objectDirectory[MotorAbstractionObject.StatusWord].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.StatusWord].SubIndex;
            if (this.usedCanOpenInterface.SdoRead(motorNumber, index, subIndex, out readValue) == ReturnType.Ok)
            {
                statusWord = (ushort)readValue;
                return (statusWord & DcomStatusWordTargetReached) == DcomStatusWordTargetReached;
            }

            return false;
        }        

        /// <summary>
        /// Inits this instance.
        /// </summary>
        public void Init()
        {
            this.InitObjectDirectory();
        }       

        /// <summary>
        /// Disables the voltage.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <returns><c>Ok</c> if the operation was successful, else <c>NotOk.</c></returns>
        public ReturnType DisableVoltage(byte motorNumber)
        {
            ushort controlWord;
            int readValue;
            int writeResponse;
            
            // Aktuelles ControlWord auslesen
            ushort index = this.objectDirectory[MotorAbstractionObject.ControlWord].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.ControlWord].SubIndex;
            if (this.usedCanOpenInterface.SdoRead(motorNumber, index, subIndex, out readValue) == ReturnType.Ok)
            {
                // Aktuelles ControlWord bearbeiten: Enable Voltage (Bit 1) muss auf 0 gesetzt werden
                readValue &= ~DcomControlWordEnableVoltage;
                controlWord = (ushort)readValue;
                
                // Neues ControlWord zum Motor schicken
                return this.usedCanOpenInterface.SdoWrite2Bytes(motorNumber, index, subIndex, controlWord, out writeResponse);
            }
            else
            {
                return ReturnType.NotOk;
            }
        }

        /// <summary>
        /// Disables the operation.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <returns><c>Ok</c> if the operation was successful, else <c>NotOk.</c></returns>
        public ReturnType DisableOperation(byte motorNumber)
        {
            ushort controlWord;
            int readValue;
            int writeResponse;
            
            // Aktuelles ControlWord auslesen
            ushort index = this.objectDirectory[MotorAbstractionObject.ControlWord].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.ControlWord].SubIndex;
            if (this.usedCanOpenInterface.SdoRead(motorNumber, index, subIndex, out readValue) == ReturnType.Ok)
            {
                // Aktuelles ControlWord bearbeiten: Enable Operation (Bit 3) muss auf 0 gesetzt werden
                readValue &= ~DcomControlWordEnableOperation;
                controlWord = (ushort)readValue;
                
                // Neues ControlWord zum Motor schicken
                return this.usedCanOpenInterface.SdoWrite2Bytes(motorNumber, index, subIndex, controlWord, out writeResponse);
            }
            else
            {
                return ReturnType.NotOk;
            }            
        }

        /// <summary>
        /// Applies the new setpoint.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <param name="changeSetImmediatly">if set to <c>true</c> [change set immediatly].</param>
        /// <param name="motorPositioningMethod">The motor positioning method.</param>
        /// <returns><c>Ok</c> if the operation was successful, else <c>NotOk.</c></returns>
        public ReturnType ApplyNewSetpoint(byte motorNumber, bool changeSetImmediatly, MotorPositioningMethodType motorPositioningMethod)
        {
            int controlWord;
            int newControlWord = -1;
            int writeResponse;
            
            // Aktuelles ControlWord auslesen
            ushort index = this.objectDirectory[MotorAbstractionObject.ControlWord].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.ControlWord].SubIndex;
            while (this.usedCanOpenInterface.SdoRead(motorNumber, index, subIndex, out controlWord) != ReturnType.Ok) 
            { 
            }

            // Aktuelles ControlWord bearbeiten: New Setpoint (Bit 4) muss auf 0 gesetzt werden
            controlWord &= ~DcomControlWordNewSetpoint;
            
            // Neues ControlWord zum Motor schicken
            this.usedCanOpenInterface.SdoWrite2Bytes(motorNumber, index, subIndex, (ushort)controlWord, out writeResponse);
            
            // Warten bis ControlWord übernommen wurde
            while (newControlWord != controlWord)
            {
                if (this.usedCanOpenInterface.SdoRead(motorNumber, index, subIndex, out newControlWord) == ReturnType.NotOk)
                {
                    newControlWord = -1;
                }
            }
            
            // Aktuelles ControlWord bearbeiten: New Setpoint (Bit 4) muss auf 1 gesetzt werden
            newControlWord |= DcomControlWordNewSetpoint;
            
            // Festlegen ob neue Positionierwerte mit Erreichen der Zielposition aktiviert wird (changeSetImmediatly=FALSE) 
            // oder sofort aktiviert wird (changeSetImmediatly=TRUE)
            if (changeSetImmediatly == true)
            {
                newControlWord |= DcomControlWordChangeSetImmediately;
            }
            else
            {
                newControlWord &= ~DcomControlWordChangeSetImmediately;
            }
            
            // Festlegen ob absolut oder relativ positioniert wird
            // für relative Positionierung muss Bit 6 auf 1 gesetzt werden,
            // für absolute Positionierung muss Bit 6 auf 0 gesetzt werden
            if (motorPositioningMethod == MotorPositioningMethodType.MotorPositioningMethodRelative)
            {
                newControlWord |= DcomControlWordPositioningRelative;
            }
            else
            {
                newControlWord &= ~DcomControlWordPositioningRelative;
            }

            // Neues ControlWord zum Motor schicken
            return this.usedCanOpenInterface.SdoWrite2Bytes(motorNumber, index, subIndex, (ushort)controlWord, out newControlWord);
        }

        /// <summary>
        /// Shutdowns the specified motor number.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <returns><c>Ok</c> if the operation was successful, else <c>NotOk.</c></returns>
        public ReturnType Shutdown(byte motorNumber)
        {
            int controlWord;
            int writeResponse;
            
            // Aktuelles ControlWord auslesen
            ushort index = this.objectDirectory[MotorAbstractionObject.ControlWord].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.ControlWord].SubIndex;
            if (this.usedCanOpenInterface.SdoRead(motorNumber, index, subIndex, out controlWord) == ReturnType.Ok)
            {
                // Aktuelles ControlWord bearbeiten: 
                // Switch on (Bit 0) muss auf 0 gesetzt werden
                controlWord &= ~DcomControlWordSwitchOn;
                
                // Enable Voltage (Bit 1) muss auf 1 gesetzt werden
                // Quick stop (Bit 2) muss auf 1 gesetzt werden
                controlWord |= DcomControlWordEnableVoltage | DcomControlWordQuickStop;
                
                // Neues ControlWord zum Motor schicken
                return this.usedCanOpenInterface.SdoWrite2Bytes(motorNumber, index, subIndex, (ushort)controlWord, out writeResponse);
            }
            else
            {
                return ReturnType.NotOk;
            }
        }

        /// <summary>
        /// Resets the node.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        public void ResetNode(byte motorNumber)
        {
            this.usedCanOpenInterface.SendNmtCommand(motorNumber, NmtService.ResetNode);
        }

        /// <summary>
        /// Starts the remote node.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        public void StartRemoteNode(byte motorNumber)
        {
            this.usedCanOpenInterface.SendNmtCommand(motorNumber, NmtService.StartRemoteNode);
        }

        /// <summary>
        /// Enables the operation of a CANopen node by reading out the current control word, 
        /// modifying the neccessary bits for "Enable Operation" and sending it back to the node.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <returns>Ok if enabling operation is successfully, else NotOk</returns>
        public ReturnType EnableOperation(byte motorNumber)
        {
            int controlWord;
            int writeResponse;
            
            // Aktuelles ControlWord auslesen
            ushort index = this.objectDirectory[MotorAbstractionObject.ControlWord].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.ControlWord].SubIndex;
            if (this.usedCanOpenInterface.SdoRead(motorNumber, index, subIndex, out controlWord) == ReturnType.Ok)
            {
                // Aktuelles ControlWord bearbeiten: 
                // Switch on (Bit 0) muss auf 1 gesetzt werden
                // Enable Voltage (Bit 1) muss auf 1 gesetzt werden
                // Quick stop (Bit 2) muss auf 1 gesetzt werden
                // Enable operation (Bit 3) muss auf 1 gesetzt werden
                controlWord |= DcomControlWordSwitchOn | DcomControlWordEnableVoltage | DcomControlWordQuickStop | DcomControlWordEnableOperation;
                
                // Neues ControlWord zum Motor schicken
                return this.usedCanOpenInterface.SdoWrite2Bytes(motorNumber, index, subIndex, (ushort)controlWord, out writeResponse);
            }
            else
            {
                return ReturnType.NotOk;
            }
        }

        /// <summary>
        /// Switches the on.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <returns><c>Ok</c> if the operation was successful, else <c>NotOk.</c></returns>
        public ReturnType SwitchOn(byte motorNumber)
        {
            int controlWord;
            int writeResponse;
            
            // Aktuelles ControlWord auslesen
            ushort index = this.objectDirectory[MotorAbstractionObject.ControlWord].Index;
            byte subIndex = this.objectDirectory[MotorAbstractionObject.ControlWord].SubIndex;
            if (this.usedCanOpenInterface.SdoRead(motorNumber, index, subIndex, out controlWord) == ReturnType.Ok)
            {
                // Aktuelles ControlWord bearbeiten: 
                // Switch on (Bit 0) muss auf 1 gesetzt werden
                // Enable Voltage (Bit 1) muss auf 1 gesetzt werden
                // Quick stop (Bit 2) muss auf 1 gesetzt werden
                // Enable operation (Bit 3) muss auf 1 gesetzt werden
                controlWord |= DcomControlWordSwitchOn | DcomControlWordEnableVoltage | DcomControlWordQuickStop;
                
                // Neues ControlWord zum Motor schicken
                return this.usedCanOpenInterface.SdoWrite2Bytes(motorNumber, index, subIndex, (ushort)controlWord, out writeResponse);
            }
            else
            {
                return ReturnType.NotOk;
            }
        }
        
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.usedCanOpenInterface.Dispose();
            }
        }

        /// <summary>
        /// Inits the object directory.
        /// </summary>
        private void InitObjectDirectory()
        {
            this.objectDirectory = new Dictionary<MotorAbstractionObject, MotorAbstractionObjectEntry>();
            this.objectDirectory.Add(MotorAbstractionObject.TelemecaniqueHomingMethod, new MotorAbstractionObjectEntry(0x6098, 0x00));
            this.objectDirectory.Add(MotorAbstractionObject.TelemecaniquePositionForMasssetzen, new MotorAbstractionObjectEntry(0x301B, 0x16));
            this.objectDirectory.Add(MotorAbstractionObject.ReceivePdo2Parameters, new MotorAbstractionObjectEntry(0x1401, 0x01));
            this.objectDirectory.Add(MotorAbstractionObject.MaxPositionLimit, new MotorAbstractionObjectEntry(0x607D, 0x02));
            this.objectDirectory.Add(MotorAbstractionObject.MinPositionLimit, new MotorAbstractionObjectEntry(0x607D, 0x01));
            this.objectDirectory.Add(MotorAbstractionObject.TelemecaniqueSoftLimitParameters, new MotorAbstractionObjectEntry(0x3006, 0x03));
            this.objectDirectory.Add(MotorAbstractionObject.PositionAtPositionInterface, new MotorAbstractionObjectEntry(0x6064, 0x00));
            this.objectDirectory.Add(MotorAbstractionObject.TargetPositionPointToPoint, new MotorAbstractionObjectEntry(0x607A, 0x00));
            this.objectDirectory.Add(MotorAbstractionObject.OperationMode, new MotorAbstractionObjectEntry(0x6060, 0x00));
            this.objectDirectory.Add(MotorAbstractionObject.ControlWord, new MotorAbstractionObjectEntry(0x6040, 0x00));
            this.objectDirectory.Add(MotorAbstractionObject.StatusWord, new MotorAbstractionObjectEntry(0x6041, 0x00));
            this.objectDirectory.Add(MotorAbstractionObject.TransmitPdo2Parameters, new MotorAbstractionObjectEntry(0x1801, 0x01));
            this.objectDirectory.Add(MotorAbstractionObject.TelemecaniqueRotationDirection, new MotorAbstractionObjectEntry(0x3006, 0x0C));
            this.objectDirectory.Add(MotorAbstractionObject.MotorLoad, new MotorAbstractionObjectEntry(0x301C, 0x1A));
            this.objectDirectory.Add(MotorAbstractionObject.TelemecaniquePositionScaleDenom, new MotorAbstractionObjectEntry(0x3006, 0x07));
            this.objectDirectory.Add(MotorAbstractionObject.TelemecaniquePositionScaleNum, new MotorAbstractionObjectEntry(0x3006, 0x08));
            this.objectDirectory.Add(MotorAbstractionObject.FaulhaberPositionScaleFeedConstant, new MotorAbstractionObjectEntry(0x6093, 0x02));
            this.objectDirectory.Add(MotorAbstractionObject.FaulhaberPositionScaleNumerator, new MotorAbstractionObjectEntry(0x6093, 0x01));
        }
    }
}
#endif