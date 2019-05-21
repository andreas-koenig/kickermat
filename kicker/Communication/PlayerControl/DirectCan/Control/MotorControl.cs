#if CAN_ENABLED
namespace Communication.PlayerControl.DirectCan.Control
{
    using System;
    using System.Threading;
    using Abstraction;
    using GlobalDataTypes;
    using NetworkLayer.Packets.Udp.Enums;

    /// <summary>
    /// Klasse für die Motoransteuerung
    /// </summary>
    public class MotorControl : IDisposable
    {
        /// <summary>
        /// The used scale demon for telemecanique motors.
        /// </summary>
        private const int TelemecaniquePositionScaleDenominator = 16384;

        /// <summary>
        /// Used motor abstraction.
        /// </summary>
        private readonly MotorAbstraction usedMotorAbstraction = new MotorAbstraction();

        /// <summary>
        /// Lenght in motor steps which is used as puffer at the lower positions.
        /// </summary>
        private readonly int[] entspannungsschrittMinPosition = new[] { 1000, 1000, 1000, 1000 };

        /// <summary>
        /// Lenght in motor steps which is used as puffer at the upper positions.
        /// </summary>
        private readonly int[] entspannungsschrittMaxPosition = new[] { 1850, 2150, 1600, 1850 };

        /// <summary>
        /// Stores the maximum positions during calibration.
        /// </summary>
        private readonly int[] maximumPosition = new[] { 0, 0, 0, 0 };        

        /// <summary>
        /// Gets the init status.
        /// </summary>
        /// <value>The init status.</value>
        public ControllerStatus InitStatus { get; private set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Motors the control_ init.
        /// </summary>
        public void Init()
        {
            this.InitStatus = ControllerStatus.Running;
            byte telemecaniqueMotorNumber;
            byte faulhaberMotorNumber;

            // InitStatus NotFinished
            this.InitStatus = ControllerStatus.Running;

            for (telemecaniqueMotorNumber = 11; telemecaniqueMotorNumber < 15; telemecaniqueMotorNumber++)
            {
                this.InitTelemecaniqueMotor(telemecaniqueMotorNumber);
            }

            this.CalibrationTelemecaniqueMotor();

            for (faulhaberMotorNumber = 1; faulhaberMotorNumber < 5; faulhaberMotorNumber++)
            {
                this.InitFaulhaberMotor(faulhaberMotorNumber);
            }

            // InitStatus Finished
            this.InitStatus = ControllerStatus.Ok;
        }       

        /// <summary>
        /// Moves the bar.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <param name="position">The position.</param>
        /// <param name="waitForResponse">if set to <c>true</c> [wait for response].</param>
        /// <returns>
        ///     <c>Ok</c> if the operation was successful, else <c>NotOk.</c>
        /// </returns>
        public ReturnType MoveBar(byte motorNumber, int position, bool waitForResponse)
        {
            checked
            {
                if ((position > this.maximumPosition[motorNumber - 11]) || (position < 0))
                {
                    throw new MotorControlException();
                }
            }

            this.usedMotorAbstraction.SetTargetPosition(motorNumber, position);
            this.usedMotorAbstraction.ApplyNewSetpoint(motorNumber, true, MotorPositioningMethodType.MotorPositioningMethodAbsolute);
            if (waitForResponse)
            {
                while (this.usedMotorAbstraction.GetMotorTargetReached(motorNumber) == false)
                {
                }
            }

            return ReturnType.Ok;
        }

        /// <summary>
        /// Rotates the bar.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <param name="angle">The angle.</param>
        /// <param name="waitForResponse">if set to <c>true</c> [wait for response].</param>
        /// <returns>
        ///     <c>Ok</c> if the operation was successful, else <c>NotOk.</c>
        /// </returns>
        public ReturnType RotateBar(byte motorNumber, short angle, bool waitForResponse)
        {
            this.usedMotorAbstraction.SetTargetPosition(motorNumber, angle);
            this.usedMotorAbstraction.ApplyNewSetpoint(motorNumber, true, MotorPositioningMethodType.MotorPositioningMethodAbsolute);
            if (waitForResponse)
            {
                while (this.usedMotorAbstraction.GetMotorTargetReached(motorNumber) == false)
                {
                }
            }

            return ReturnType.Ok;
        }       

        /// <summary>
        /// Sets the faulhaber zero point.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <param name="angle">The angle.</param>
        public void SetFaulhaberZeroPoint(byte motorNumber, short angle)
        {
            // Betriebsmodus Referenzierung aktiveren, nur in diesem Modus ist Maßsetzen möglich
            this.usedMotorAbstraction.SetMotorOperationMode(motorNumber, MotorOperationModeType.MotorOperationModeReferencing);
            while (this.usedMotorAbstraction.GetMotorOperationMode(motorNumber) != MotorOperationModeType.MotorOperationModeReferencing)
            {
            }

            throw new NotImplementedException();
            
            // Rro: muss noch fertiggestellt werden

            /*
            // .. und schreiben es in den Motor
            CANopen_SDO_Write4Bytes(motorNumber , FH_HOMING_OFFSET_INDEX , FH_HOMING_OFFSET_SUBINDEX , (uInt32)Angle);

            // Maßsetzen aktiviren, um Nullpunkt festlegen zu können
            CANopen_SDO_Write1Bytes(motorNumber , TM_HMP_HM_METHOD_INDEX , TM_HMP_HM_METHOD_SUBINDEX , TM_HMP_HM_METHOD_MASSSETZEN);
            while (this.usedMotorAbstraction.GetMotorHomingMethod(motorNumber) != MotorHomingMethodMasssetzen);

            //this.usedMotorAbstraction.ApplyNewSetpoint(motorNumber);
            this.usedMotorAbstraction.ApplyNewSetpoint(motorNumber, true, MotorPositioningMethodAbsolute);
            // RoRe: muss man hier noch auf was warten?

            // Abschließend wieder Punkt zu Punkt aktivieren
            this.usedMotorAbstraction.SetMotorOperationMode(motorNumber, MotorOperationModePointToPoint);
            while (this.usedMotorAbstraction.GetMotorOperationMode(motorNumber) != MotorOperationModePointToPoint);

            // Neuen Nullpunkt anfahren
            RotateBar(motorNumber, 0, 0);
            */
        }

        /// <summary>
        /// Gotoes the min max telemecanique motor.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <param name="position">The position.</param>
        /// <returns>Current controllerstatus</returns>
        public ControllerStatus GotoMinMaxTelemecaniqueMotor(byte motorNumber, int position)
        {
            switch (position)
            {
                case 0:
                    // position 0 ist OK, es muss nix mehr geändert werden
                    break;
                case 255:
                    if (this.usedMotorAbstraction.GetMotorMaximumPositionLimit(motorNumber, out position) != ReturnType.Ok)
                    {
                        return ControllerStatus.Error;
                    }

                    break;
            }

            this.usedMotorAbstraction.SetTargetPosition(motorNumber, position);
            this.usedMotorAbstraction.ApplyNewSetpoint(motorNumber, true, MotorPositioningMethodType.MotorPositioningMethodAbsolute);
            while (this.usedMotorAbstraction.GetMotorTargetReached(motorNumber) == false)
            {
            }

            return ControllerStatus.Ok;
        }

        /// <summary>
        /// Scales the telemecanique motor.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <param name="pixel">The pixel.</param>
        public void ScaleTelemecaniqueMotor(byte motorNumber, int pixel)
        {
            int oldMaximumPositionLimit;
            int oldMinimumPositionLimit;
            int oldMaximumPosition;
            int skalierungsfaktorNenner;

            int tempPositionLimitForRead = 0;

            checked
            {
                this.maximumPosition[motorNumber - 11] = pixel;
            }

            this.usedMotorAbstraction.DisableVoltage(motorNumber);
            while (this.usedMotorAbstraction.GetMotorOperationStatus(motorNumber) != MotorOperationStatusType.MotorOperationStatusReadyToSwitchOn)
            {
            }

            this.usedMotorAbstraction.SetMotorMaximumPositionLimitState(motorNumber, MotorFeatureStateType.MotorFeatureStateInactive);
            while (this.usedMotorAbstraction.GetMotorMaximumPositionLimitState(motorNumber) != MotorFeatureStateType.MotorFeatureStateInactive)
            {
            }

            this.usedMotorAbstraction.SetMotorMinimumPositionLimitState(motorNumber, MotorFeatureStateType.MotorFeatureStateInactive);
            while (this.usedMotorAbstraction.GetMotorMinimumPositionLimitState(motorNumber) != MotorFeatureStateType.MotorFeatureStateInactive)
            {
            }

            while (this.usedMotorAbstraction.GetMotorMaximumPositionLimit(motorNumber, out oldMaximumPositionLimit) != ReturnType.Ok)
            {
            }

            checked
            {
                oldMaximumPosition = oldMaximumPositionLimit - this.entspannungsschrittMaxPosition[motorNumber - 11];
            }

            int newMaximumPositonLimit = (pixel * oldMaximumPositionLimit) / oldMaximumPosition;
            checked
            {
                this.entspannungsschrittMaxPosition[motorNumber - 11] = newMaximumPositonLimit - pixel;
            }

            while (this.usedMotorAbstraction.GetMotorMinimumPositionLimit(motorNumber, out oldMinimumPositionLimit) != ReturnType.Ok)
            {
            }

            int newMinimumPositonLimit = (pixel * oldMinimumPositionLimit) / oldMaximumPosition;
            checked
            {
                this.entspannungsschrittMinPosition[motorNumber - 11] = newMinimumPositonLimit * (-1);
            }

            // pixel Einheiten pro Motorumdrehung
            checked
            {
                skalierungsfaktorNenner = (pixel * TelemecaniquePositionScaleDenominator) / oldMaximumPosition;
            }

            this.usedMotorAbstraction.SetMotorTMScale(motorNumber, 1, skalierungsfaktorNenner);

            this.usedMotorAbstraction.SetMotorMaximumPositionLimit(motorNumber, newMaximumPositonLimit);
            while (tempPositionLimitForRead != newMaximumPositonLimit)
            {
                this.usedMotorAbstraction.GetMotorMaximumPositionLimit(motorNumber, out tempPositionLimitForRead);
            }

            this.usedMotorAbstraction.SetMotorMinimumPositionLimit(motorNumber, newMinimumPositonLimit);
            while (tempPositionLimitForRead != newMinimumPositonLimit)
            {
                this.usedMotorAbstraction.GetMotorMinimumPositionLimit(motorNumber, out tempPositionLimitForRead);
            }

            this.usedMotorAbstraction.SetMotorMaximumPositionLimitState(motorNumber, MotorFeatureStateType.MotorFeatureStateActive);
            while (this.usedMotorAbstraction.GetMotorMaximumPositionLimitState(motorNumber) != MotorFeatureStateType.MotorFeatureStateActive)
            {
            }

            this.usedMotorAbstraction.SetMotorMinimumPositionLimitState(motorNumber, MotorFeatureStateType.MotorFeatureStateActive);
            while (this.usedMotorAbstraction.GetMotorMinimumPositionLimitState(motorNumber) != MotorFeatureStateType.MotorFeatureStateActive)
            {
            }

            this.usedMotorAbstraction.EnableOperation(motorNumber);
            while (this.usedMotorAbstraction.GetMotorOperationStatus(motorNumber) != MotorOperationStatusType.MotorOperationStatusOperationEnabled)
            {
            }
        }

        /********************************************************************************************************
         *  Telemecanique Funktionen
         ********************************************************************************************************/

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.usedMotorAbstraction.Dispose();
            }
        }

        /// <summary>
        /// Calculates the array index of a motor.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        /// <returns>The array index of the motor.</returns>
        private static uint MOTOR_NUMBER_ARRAY_INDEX(uint motorNumber)
        {
            return motorNumber - 11;
        }

        /// <summary>
        /// OSs the time dly HMSM.
        /// </summary>
        /// <param name="hours">The hours.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <param name="milliseconds">The milliseconds.</param>
        private static void OSTimeDlyHMSM(int hours, int minutes, int seconds, int milliseconds)
        {
            Thread.Sleep(new TimeSpan(0, hours, minutes, seconds, milliseconds));
        }

        /********************************************************************************************************
        *  Faulhaber Funktionen
        ********************************************************************************************************/

        /// <summary>
        /// Inits the faulhaber motor.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        private void InitFaulhaberMotor(byte motorNumber)
        {
            // Umrechnungsfaktor für die Skalierung der Faulhaber Motoren in Winkel
            const ushort FH_NUMBER_OF_STEPS = 41375;

            int numerator = 0;
            int feedConstant = 0;

            this.usedMotorAbstraction.StartRemoteNode(motorNumber);

            this.usedMotorAbstraction.DisableVoltage(motorNumber);
            while (this.usedMotorAbstraction.GetMotorOperationStatus(motorNumber) != MotorOperationStatusType.MotorOperationStatusSwitchOnDisabled)
            {
            }

            this.usedMotorAbstraction.Shutdown(motorNumber);
            while (this.usedMotorAbstraction.GetMotorOperationStatus(motorNumber) != MotorOperationStatusType.MotorOperationStatusReadyToSwitchOn)
            {
            }

            this.usedMotorAbstraction.StartRemoteNode(motorNumber);
            while (this.usedMotorAbstraction.GetMotorNmtState(motorNumber) != NmtStateType.MotorStateOperational)
            {
            }

            this.usedMotorAbstraction.SwitchOn(motorNumber);
            while (this.usedMotorAbstraction.GetMotorOperationStatus(motorNumber) != MotorOperationStatusType.MotorOperationStatusSwitchedOn)
            {
            }

            this.usedMotorAbstraction.EnableOperation(motorNumber);
            while (this.usedMotorAbstraction.GetMotorOperationStatus(motorNumber) != MotorOperationStatusType.MotorOperationStatusOperationEnabled)
            {
            }

            this.usedMotorAbstraction.SetMotorOperationMode(motorNumber, MotorOperationModeType.MotorOperationModePointToPoint);
            while (this.usedMotorAbstraction.GetMotorOperationMode(motorNumber) != MotorOperationModeType.MotorOperationModePointToPoint)
            {
            }

            // Setze Positions Faktor so, dass der Motor direkt mit Winkelangaben positioniert werden kann
            this.usedMotorAbstraction.SetMotorPositionFactorNumerator(motorNumber, FH_NUMBER_OF_STEPS);
            while (numerator != FH_NUMBER_OF_STEPS)
            {
                this.usedMotorAbstraction.GetMotorPositionFactorNumerator(motorNumber, out numerator);
            }

            this.usedMotorAbstraction.SetMotorPositionFactorFeedConstant(motorNumber, 360);
            while (feedConstant != 360)
            {
                this.usedMotorAbstraction.GetMotorPositionFactorFeedConstant(motorNumber, out feedConstant);
            }

            this.usedMotorAbstraction.SetTargetPosition(motorNumber, 360);
            this.usedMotorAbstraction.ApplyNewSetpoint(motorNumber, true, MotorPositioningMethodType.MotorPositioningMethodRelative);
            while (this.usedMotorAbstraction.GetMotorTargetReached(motorNumber) == false)
            {
            }

            // RoRe: Am Schluss wieder zum Nullpunkt zurück, damit die Kalibrierung das nicht machen muss
            this.usedMotorAbstraction.SetTargetPosition(motorNumber, 0);
            this.usedMotorAbstraction.ApplyNewSetpoint(motorNumber, true, MotorPositioningMethodType.MotorPositioningMethodAbsolute);
            while (this.usedMotorAbstraction.GetMotorTargetReached(motorNumber) == false)
            {
            }
        }

        /// <summary>
        /// Inits the telemecanique motor.
        /// </summary>
        /// <param name="motorNumber">The motor number.</param>
        private void InitTelemecaniqueMotor(byte motorNumber)
        {
            // Schritt 1: Motor abschalten und herunterfahren (es kann sein, dass der Motor bereits aktiviert ist
            // und "Start Remote node" nicht den erforderlichen Betriebsmodus meldet)
            this.usedMotorAbstraction.DisableVoltage(motorNumber);
            while (this.usedMotorAbstraction.GetMotorOperationStatus(motorNumber) != MotorOperationStatusType.MotorOperationStatusReadyToSwitchOn)
            {
            }

            this.usedMotorAbstraction.Shutdown(motorNumber);
            while (this.usedMotorAbstraction.GetMotorOperationStatus(motorNumber) != MotorOperationStatusType.MotorOperationStatusReadyToSwitchOn)
            {
            }

            // Schritt 2: Zurücksetzten des Knotens (Reset) und warten bis der Motor im Zustand PreOperational ist
            // RoRe: rausgenommen, weil der Motor sonst selbständig 00 als NMT-Status überträgt und diser nicht 
            // ausgewertet werden kann ->Fehler
            // this.usedMotorAbstraction.ResetNode(motorNumber);
            // while (this.usedMotorAbstraction.GetMotorNMTState(motorNumber)!= MotorState_PreOperational);

            // Schritt 3: Start remote node senden und warten bis der Motor im Zustand Operational ist
            this.usedMotorAbstraction.StartRemoteNode(motorNumber);
            while (this.usedMotorAbstraction.GetMotorNmtState(motorNumber) != NmtStateType.MotorStateOperational)
            {
            }

            this.usedMotorAbstraction.SwitchOn(motorNumber);
            while (this.usedMotorAbstraction.GetMotorOperationStatus(motorNumber) != MotorOperationStatusType.MotorOperationStatusSwitchedOn)
            {
            }

            // Schritt 4: Start Remote Node abgeschlossen, Enable Operation
            this.usedMotorAbstraction.EnableOperation(motorNumber);
            while (this.usedMotorAbstraction.GetMotorOperationStatus(motorNumber) != MotorOperationStatusType.MotorOperationStatusOperationEnabled)
            {
            }

            // RoRe: auskommentiert, weil nicht erlaubt wenn Endstufe aktiviert ist
            // Implementierung tbd weil Drehrichtung jetzt im EEPROM gespeichert ist
            // und eine Übernahme der neuen Drehrichtung erst nach einem Neustart stattfindet
            /*if(motorNumber == 11)
            {
                this.usedMotorAbstraction.SetRotationDirection( motorNumber, 1);
                while( this.usedMotorAbstraction.GetRotationDirection( motorNumber) !=1);
            } */

            /*  //Schritt 5: Enable Operation abgeschlossen, R_PDO2 aktivieren
                this.usedMotorAbstraction.SetMotorRPDO2_State(motorNumber, MotorFeatureStateActive);
                while(this.usedMotorAbstraction.GetMotorRPDO2_State(motorNumber) != MotorFeatureStateActive);

                // Schritt 6: R_PDO2 aktivieren abgeschlossen, T_PDO2 aktivieren
                this.usedMotorAbstraction.SetMotorTPDO2_State(motorNumber, MotorFeatureStateActive);
                while(this.usedMotorAbstraction.GetMotorTPDO2_State(motorNumber) != MotorFeatureStateActive);  */

            // Schritt 7: Enable Operation abgeschlossen, Betriebsart PtP aktivieren
            this.usedMotorAbstraction.SetMotorOperationMode(motorNumber, MotorOperationModeType.MotorOperationModePointToPoint);
            while (this.usedMotorAbstraction.GetMotorOperationMode(motorNumber) != MotorOperationModeType.MotorOperationModePointToPoint)
            {
            }
        }              

        /// <summary>
        /// Calibrations the telemecanique motor.
        /// </summary>
        private void CalibrationTelemecaniqueMotor()
        {
            const byte CALIBRATION_NOT_STARTED = 0x00;
            const byte CALIBRATION_VOLTAGE_DISABLED_1 = CALIBRATION_NOT_STARTED + 1;
            const byte CALIBRATION_POS_LIMITS_DISABLED = CALIBRATION_VOLTAGE_DISABLED_1 + 1;
            const byte CALIBRATION_PARAMETER_SET = CALIBRATION_POS_LIMITS_DISABLED + 1;
            const byte CALIBRATION_OPERATION_ENABLED_1 = CALIBRATION_PARAMETER_SET + 1;
            const byte CALIBRATION_START_POSITION_SAVED = CALIBRATION_OPERATION_ENABLED_1 + 1;
            const byte CALIBRATION_POINT_TO_POINT_ENABLED_1 = CALIBRATION_START_POSITION_SAVED + 1;
            const byte CALIBRATION_BELASTUNG_0_REACHED_1 = CALIBRATION_POINT_TO_POINT_ENABLED_1 + 1;
            const byte CALIBRATION_NEG_STEP_SIZE_SET = CALIBRATION_BELASTUNG_0_REACHED_1 + 1;
            const byte CALIBRATION_LOWEST_POSITION_REACHED = CALIBRATION_NEG_STEP_SIZE_SET + 1;
            const byte CALIBRATION_LOWEST_POSITION_SAVED = CALIBRATION_LOWEST_POSITION_REACHED + 1;
            const byte CALIBRATION_START_POS_STEP_SIZE_SET = CALIBRATION_LOWEST_POSITION_SAVED + 1;
            const byte CALIBRATION_START_POSITION_REACHED_1 = CALIBRATION_START_POS_STEP_SIZE_SET + 1;
            const byte CALIBRATION_BELASTUNG_0_REACHED_2 = CALIBRATION_START_POSITION_REACHED_1 + 1;
            const byte CALIBRATION_POS_STEP_SIZE_SET = CALIBRATION_BELASTUNG_0_REACHED_2 + 1;
            const byte CALIBRATION_HIGHEST_POSITION_REACHED = CALIBRATION_POS_STEP_SIZE_SET + 1;
            const byte CALIBRATION_HIGHEST_POSITION_SAVED = CALIBRATION_HIGHEST_POSITION_REACHED + 1;
            const byte CALIBRATION_START_POSITION_REACHED_2 = CALIBRATION_HIGHEST_POSITION_SAVED + 1;
            const byte CALIBRATION_REFERENCING_ENABLED = CALIBRATION_START_POSITION_REACHED_2 + 1;
            const byte CALIBRATION_CERO_POINT_SET = CALIBRATION_REFERENCING_ENABLED + 1;
            const byte CALIBRATION_VOLTAGE_DISABLED_2 = CALIBRATION_CERO_POINT_SET + 1;
            const byte CALIBRATION_SOFTLIMITS_SET = CALIBRATION_VOLTAGE_DISABLED_2 + 1;
            const byte CALIBRATION_OPERATION_ENABLED_2 = CALIBRATION_SOFTLIMITS_SET + 1;
            const byte CALIBRATION_SOFTLIMITS_ENABLED = CALIBRATION_OPERATION_ENABLED_2 + 1;
            const byte CALIBRATION_POINT_TO_POINT_ENABLED_2 = CALIBRATION_SOFTLIMITS_ENABLED + 1;
            const byte CALIBRATION_CENTER_REACHED = CALIBRATION_POINT_TO_POINT_ENABLED_2 + 1;
            const byte CALIBRATION_FINISHED = CALIBRATION_CENTER_REACHED + 1;

            int stepSize = 40;
            ushort belastungslimit = 2;
            int currentMotorLoad = 0;
            ushort anzahlVonZyklenUnterBelastungsLimit = 10;
            byte[] calibrationStep = new byte[] { 0, 0, 0, 0 };
            ushort timeToWait = 2;  // in Millisekunden, Warnung nur in VS, in Keil ist alles gut
            ushort[] motorLoadCounter = new ushort[] { 0, 0, 0, 0 };
            int tempPositionForRead = 0;
            int[] startPosition = new int[] { 0, 0, 0, 0 };
            int[] minimalePosition = new int[] { 0, 0, 0, 0 };
            int[] aktuelleSollPosition = new int[] { 0, 0, 0, 0 };
            int[] minimumPositionLimit = new int[] { 0, 0, 0, 0 };
            int[] maximumPositionLimit = new int[] { 0, 0, 0, 0 };
            int[] mittelpunkt = new int[] { 0, 0, 0, 0 };
            byte motorNumber = 10;
            int[] tempPosition = new int[] { 0, 0, 0, 0 };

            // Defaultwert des Nenneners für die Skalierung des Telemecanique Motors
            while ((calibrationStep[0] != CALIBRATION_FINISHED) ||
                   (calibrationStep[1] != CALIBRATION_FINISHED) ||
                   (calibrationStep[2] != CALIBRATION_FINISHED) ||
                   (calibrationStep[3] != CALIBRATION_FINISHED))
            {
                motorNumber++;
                if (motorNumber > 14)
                {
                    motorNumber = 11;
                }

                switch (calibrationStep[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)])
                {
                    case CALIBRATION_NOT_STARTED:
                        // Betriebsmodus Referenzierung aktiveren, nur in diesem Modus ist Maßsetzen möglich
                        this.usedMotorAbstraction.SetMotorOperationMode(motorNumber, MotorOperationModeType.MotorOperationModeReferencing);
                        while (this.usedMotorAbstraction.GetMotorOperationMode(motorNumber) != MotorOperationModeType.MotorOperationModeReferencing)
                        {
                        }

                        // Spannung abschalten, damit sich der Motor in einer entspannten Position befindet
                        this.usedMotorAbstraction.DisableVoltage(motorNumber);
                        while (this.usedMotorAbstraction.GetMotorOperationStatus(motorNumber) != MotorOperationStatusType.MotorOperationStatusReadyToSwitchOn)
                        {
                        }

                        calibrationStep[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]++;
                        break;
                    case CALIBRATION_VOLTAGE_DISABLED_1:
                        // Minimales Positionslimit deaktivieren, könnte ja sein dass von vorheriger Kalibrierung noch Einstellungen vorhanden sind
                        this.usedMotorAbstraction.SetMotorMinimumPositionLimitState(motorNumber, MotorFeatureStateType.MotorFeatureStateInactive);
                        while (this.usedMotorAbstraction.GetMotorMinimumPositionLimitState(motorNumber) != MotorFeatureStateType.MotorFeatureStateInactive)
                        {
                        }

                        // Maximales Positionslimit deaktivieren, könnte ja sein dass von vorheriger Kalibrierung noch Einstellungen vorhanden sind
                        this.usedMotorAbstraction.SetMotorMaximumPositionLimitState(motorNumber, MotorFeatureStateType.MotorFeatureStateInactive);

                        // Skalierung wieder auf Defaultwert zurücksetzen
                        this.usedMotorAbstraction.SetMotorTMScale(motorNumber, 1, TelemecaniquePositionScaleDenominator);

                        while (this.usedMotorAbstraction.GetMotorMaximumPositionLimitState(motorNumber) != MotorFeatureStateType.MotorFeatureStateInactive)
                        {
                        }

                        calibrationStep[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]++;
                        break;
                    case CALIBRATION_POS_LIMITS_DISABLED:
                        throw new NotImplementedException();
                        /*
                //Beschleunigungsrampe festlegen
                CANopen_SDO_Write4Bytes(motorNumber,PROFILE_ACCELERATION, 0, 10000);
                //Verzoegerungsrampe festlegen
                CANopen_SDO_Write4Bytes(motorNumber, PROFILE_DECELERATION, 0, 10000);
                //Maximale Drehzahl festlegen, Maximalwert ist 13200
                CANopen_SDO_Write4Bytes(motorNumber, MAX_PROFILE_VELOCITY, 0, 13200);
                //Drehzahl festlegen, Maximalwert ist wert in RAMPn_max
                CANopen_SDO_Write4Bytes(motorNumber, PROFILE_VELOCITY, 0, 8000);
                calibrationStep[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]++;
                
                break;     */
                    case CALIBRATION_PARAMETER_SET:
                        // Wenn Motor entspannt ist, Spannung wieder einschalten
                        this.usedMotorAbstraction.EnableOperation(motorNumber);
                        while (this.usedMotorAbstraction.GetMotorOperationStatus(motorNumber) != MotorOperationStatusType.MotorOperationStatusOperationEnabled)
                        {
                        }

                        calibrationStep[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]++;
                        break;
                    case CALIBRATION_OPERATION_ENABLED_1:
                        // Startposition sichern, zu dieser Position wird nach dem Kalibrieren zurückgekehrt
                        if (this.usedMotorAbstraction.GetMotorPosition(motorNumber, out tempPositionForRead) == ReturnType.Ok)
                        {
                            startPosition[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)] = tempPositionForRead;
                            calibrationStep[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]++;
                        }

                        break;
                    case CALIBRATION_START_POSITION_SAVED:
                        // In den Betriebsmodus Punkt zu Punkt wechseln, erforderlich damit sich die Motoren bewegen
                        this.usedMotorAbstraction.SetMotorOperationMode(motorNumber, MotorOperationModeType.MotorOperationModePointToPoint);
                        while (this.usedMotorAbstraction.GetMotorOperationMode(motorNumber) != MotorOperationModeType.MotorOperationModePointToPoint)
                        {
                        }

                        calibrationStep[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]++;
                        break;
                    case CALIBRATION_POINT_TO_POINT_ENABLED_1:
                        // Wartezeit dazwischen, weil Motorbelastung sich nur sehr langsam ändert
                        OSTimeDlyHMSM(0, 0, 0, timeToWait);
                        if (this.usedMotorAbstraction.GetMotorLoad(motorNumber, out currentMotorLoad) == ReturnType.Ok)
                        {
                            if (currentMotorLoad < belastungslimit)
                            {
                                motorLoadCounter[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]++;
                                if (motorLoadCounter[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)] > anzahlVonZyklenUnterBelastungsLimit)
                                {
                                    motorLoadCounter[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)] = 0;
                                    calibrationStep[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]++;
                                }
                            }
                        }

                        break;
                    case CALIBRATION_BELASTUNG_0_REACHED_1:
                        // Suche des Minimalen Positionslimits: Soweit zurück gehen, bis die Motorbelastung ansteigt
                        this.usedMotorAbstraction.SetTargetPosition(motorNumber, -stepSize);
                        calibrationStep[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]++;
                        break;
                    case CALIBRATION_NEG_STEP_SIZE_SET:
                        // Wartezeit dazwischen, weil Motorbelastung sich nur sehr langsam ändert
                        OSTimeDlyHMSM(0, 0, 0, timeToWait);
                        if (this.usedMotorAbstraction.GetMotorLoad(motorNumber, out currentMotorLoad) == ReturnType.Ok)
                        {
                            if (currentMotorLoad < belastungslimit)
                            {
                                this.usedMotorAbstraction.ApplyNewSetpoint(motorNumber, false, MotorPositioningMethodType.MotorPositioningMethodRelative);
                            }
                            else
                            {
                                // um 2500 Umdrehungen wieder zurück gehen um die Mechanik zu entlasten
                                /*this.usedMotorAbstraction.SetTargetPosition(motorNumber, entspannungsschrittMinPosition[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]);
                                this.usedMotorAbstraction.ApplyNewSetpoint(motorNumber, false, MotorPositioningMethodRelative);*/
                                while (this.usedMotorAbstraction.GetMotorTargetReached(motorNumber) == false)
                                {
                                }

                                // in den naechsten Zustand wechseln
                                calibrationStep[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]++;
                            }
                        }

                        break;
                    case CALIBRATION_LOWEST_POSITION_REACHED:
                        // Minimale Position speichern, wird später zum Setzen des Nullpunktes und der Softlimits benötigt
                        if (this.usedMotorAbstraction.GetMotorPosition(motorNumber, out tempPositionForRead) == ReturnType.Ok)
                        {
                            minimalePosition[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)] = tempPositionForRead;
                            calibrationStep[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]++;
                        }

                        break;
                    case CALIBRATION_LOWEST_POSITION_SAVED:
                        // Langsam zurück zur Startposition einstellen 
                        this.usedMotorAbstraction.SetTargetPosition(motorNumber, (stepSize + stepSize));

                        // RoRe und ES: nur bis zur halben Startposition aber mindestens um 1000 Umdrehungen zurück gehen, 
                        // weil der Motor beim Starten der Kalibirerung
                        // am Anschlag war und dann die Belastung am oberen/unteren Ende nicht mehr zurückgeht
                        if (this.usedMotorAbstraction.GetMotorPosition(motorNumber, out tempPositionForRead) == ReturnType.Ok)
                        {
                            if (((startPosition[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)] / 2) - tempPositionForRead) < 1000)
                            {
                                tempPosition[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)] = tempPositionForRead + 1000;
                            }
                            else
                            {
                                tempPosition[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)] = startPosition[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)] / 2;
                            }

                            calibrationStep[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]++;
                        }

                        break;
                    case CALIBRATION_START_POS_STEP_SIZE_SET:
                        // Zurück zur Startposition
                        if (this.usedMotorAbstraction.GetMotorPosition(motorNumber, out tempPositionForRead) == ReturnType.Ok)
                        {
                            if (tempPositionForRead < tempPosition[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)])
                            {
                                this.usedMotorAbstraction.ApplyNewSetpoint(motorNumber, false, MotorPositioningMethodType.MotorPositioningMethodRelative);
                            }
                            else
                            {
                                // in den naechsten Zustand wechseln
                                calibrationStep[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]++;
                            }
                        }

                        break;
                    case CALIBRATION_START_POSITION_REACHED_1:
                        // Warten bis Motorbelastung zurückgegangen ist
                        OSTimeDlyHMSM(0, 0, 0, timeToWait);
                        if (this.usedMotorAbstraction.GetMotorLoad(motorNumber, out currentMotorLoad) == ReturnType.Ok)
                        {
                            if (currentMotorLoad < belastungslimit)
                            {
                                motorLoadCounter[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]++;
                                if (motorLoadCounter[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)] > anzahlVonZyklenUnterBelastungsLimit)
                                {
                                    motorLoadCounter[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)] = 0;
                                    calibrationStep[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]++;
                                }
                            }
                        }

                        break;
                    case CALIBRATION_BELASTUNG_0_REACHED_2:
                        // Suche des Endes des Spielfeldes, soweit vorwärts gehen, bis die Motorbelastung ansteigt
                        this.usedMotorAbstraction.SetTargetPosition(motorNumber, stepSize);
                        calibrationStep[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]++;
                        break;
                    case CALIBRATION_POS_STEP_SIZE_SET:
                        // Wartezeit dazwischen, weil Motorbelastung sich nur sehr langsam ändert
                        OSTimeDlyHMSM(0, 0, 0, timeToWait);
                        if (this.usedMotorAbstraction.GetMotorLoad(motorNumber, out currentMotorLoad) == ReturnType.Ok)
                        {
                            if (currentMotorLoad < belastungslimit)
                            {
                                this.usedMotorAbstraction.ApplyNewSetpoint(motorNumber, false, MotorPositioningMethodType.MotorPositioningMethodRelative);
                            }
                            else
                            {
                                // wieder zurück gehen um die Mechanik zu entlasten
                                /*this.usedMotorAbstraction.SetTargetPosition(motorNumber, entspannungsschrittMaxPosition[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]);
                                this.usedMotorAbstraction.ApplyNewSetpoint(motorNumber, false, MotorPositioningMethodRelative);*/
                                while (this.usedMotorAbstraction.GetMotorTargetReached(motorNumber) == false)
                                {
                                }

                                calibrationStep[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]++;
                            }
                        }

                        break;
                    case CALIBRATION_HIGHEST_POSITION_REACHED:
                        // Maximale Position speichern, wird später zum Setzen des Nullpunktes und der Softlimits benötigt
                        if (this.usedMotorAbstraction.GetMotorPosition(motorNumber, out tempPositionForRead) == ReturnType.Ok)
                        {
                            this.maximumPosition[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)] = tempPositionForRead;
                            calibrationStep[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]++;
                        }

                        break;
                    case CALIBRATION_HIGHEST_POSITION_SAVED:
                        // Zurück zur Startposition
                        this.usedMotorAbstraction.SetTargetPosition(motorNumber, startPosition[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]);
                        this.usedMotorAbstraction.ApplyNewSetpoint(motorNumber, true, MotorPositioningMethodType.MotorPositioningMethodAbsolute);
                        while (this.usedMotorAbstraction.GetMotorTargetReached(motorNumber) == false)
                        {
                        }

                        calibrationStep[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]++;
                        break;
                    case CALIBRATION_START_POSITION_REACHED_2:
                        // Betriebsmodus Referenzierung aktiveren, nur in diesem Modus ist Maßsetzen möglich
                        this.usedMotorAbstraction.SetMotorOperationMode(motorNumber, MotorOperationModeType.MotorOperationModeReferencing);
                        while (this.usedMotorAbstraction.GetMotorOperationMode(motorNumber) != MotorOperationModeType.MotorOperationModeReferencing)
                        {
                        }

                        calibrationStep[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]++;
                        break;
                    case CALIBRATION_REFERENCING_ENABLED:
                        // Durch Maßsetzen wird die aktuelle Motorposition auf den Positionswert im Parameter HMp_setpusr gesetzt. 
                        // Dadurch wird auch der Nullpunkt definiert.
                        // Also rechnen wir HMp_setpusr aus ...
                        aktuelleSollPosition[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)] = (minimalePosition[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)] * -1) + startPosition[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)];
                        aktuelleSollPosition[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)] -= this.entspannungsschrittMinPosition[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)];

                        // .. und schreiben es in den Motor
                        this.usedMotorAbstraction.SetPositionForMasssetzenTelemecanique(motorNumber, aktuelleSollPosition[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]);

                        // Maßsetzen aktiviren, um Nullpunkt festlegen zu können
                        this.usedMotorAbstraction.SetMotorHomingMethod(motorNumber, MotorHomingMethodType.MotorHomingMethodMasssetzen);
                        while (this.usedMotorAbstraction.GetMotorHomingMethod(motorNumber) != MotorHomingMethodType.MotorHomingMethodMasssetzen)
                        {
                        }

                        this.usedMotorAbstraction.ApplyNewSetpoint(motorNumber, true, MotorPositioningMethodType.MotorPositioningMethodAbsolute);

                        // RoRe: muss man hier noch auf was warten?
                        calibrationStep[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]++;
                        break;
                    case CALIBRATION_CERO_POINT_SET:
                        // Spannung abschalten, sonst können keine Positionslimits gesetzt werden
                        this.usedMotorAbstraction.DisableVoltage(motorNumber);
                        while (this.usedMotorAbstraction.GetMotorOperationStatus(motorNumber) != MotorOperationStatusType.MotorOperationStatusReadyToSwitchOn)
                        {
                        }

                        calibrationStep[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]++;
                        break;
                    case CALIBRATION_VOLTAGE_DISABLED_2:
                        // Untere Positionsgrenze setzten, immer Toleranz mit einbeziehen
                        minimumPositionLimit[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)] = 0 - this.entspannungsschrittMinPosition[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)];
                        while (this.usedMotorAbstraction.SetMotorMinimumPositionLimit(motorNumber, minimumPositionLimit[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]) != ReturnType.Ok)
                        {
                        }

                        // Obere Positionsgrenze setzen, immer Toleranz mit einbeziehen 
                        maximumPositionLimit[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)] = this.maximumPosition[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)] - minimalePosition[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)];
                        while (this.usedMotorAbstraction.SetMotorMinimumPositionLimit(motorNumber, maximumPositionLimit[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]) != ReturnType.Ok)
                        {
                        }

                        calibrationStep[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]++;
                        break;
                    case CALIBRATION_SOFTLIMITS_SET:
                        // Wenn Positionslimits gesetzt sind, kann Spannung wieder eingeschalten werden
                        this.usedMotorAbstraction.EnableOperation(motorNumber);
                        while (this.usedMotorAbstraction.GetMotorOperationStatus(motorNumber) != MotorOperationStatusType.MotorOperationStatusOperationEnabled)
                        {
                        }

                        calibrationStep[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]++;
                        break;
                    case CALIBRATION_OPERATION_ENABLED_2:
                        // Minimales Positionslimit aktivieren
                        this.usedMotorAbstraction.SetMotorMinimumPositionLimitState(motorNumber, MotorFeatureStateType.MotorFeatureStateActive);
                        while (this.usedMotorAbstraction.GetMotorMinimumPositionLimitState(motorNumber) != MotorFeatureStateType.MotorFeatureStateActive)
                        {
                        }

                        // Maximales Positionslimit aktivieren
                        this.usedMotorAbstraction.SetMotorMaximumPositionLimitState(motorNumber, MotorFeatureStateType.MotorFeatureStateActive);
                        while (this.usedMotorAbstraction.GetMotorMaximumPositionLimitState(motorNumber) != MotorFeatureStateType.MotorFeatureStateActive)
                        {
                        }

                        calibrationStep[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]++;
                        break;
                    case CALIBRATION_SOFTLIMITS_ENABLED:
                        // Abschließend wieder Punkt zu Punkt aktivieren
                        this.usedMotorAbstraction.SetMotorOperationMode(motorNumber, MotorOperationModeType.MotorOperationModePointToPoint);
                        while (this.usedMotorAbstraction.GetMotorOperationMode(motorNumber) != MotorOperationModeType.MotorOperationModePointToPoint)
                        {
                        }

                        calibrationStep[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]++;
                        break;
                    case CALIBRATION_POINT_TO_POINT_ENABLED_2:

                        // Und dann Stange mittig positionieren, >>1 is das gleiche wie /2
                        mittelpunkt[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)] = (maximumPositionLimit[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)] - this.entspannungsschrittMaxPosition[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]) >> 1;
                        this.usedMotorAbstraction.SetTargetPosition(motorNumber, mittelpunkt[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]);

                        this.usedMotorAbstraction.ApplyNewSetpoint(motorNumber, true, MotorPositioningMethodType.MotorPositioningMethodAbsolute);
                        calibrationStep[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]++;
                        break;
                    case CALIBRATION_CENTER_REACHED:
                        Console.WriteLine("*******************************************************************************\r\n");
                        Console.WriteLine("* Motor %i                                                                    *\r\n", motorNumber);
                        Console.WriteLine("*******************************************************************************\r\n");
                        Console.WriteLine("startPosition : %ld\r\n", startPosition[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]);
                        Console.WriteLine("minimalePosition : %ld\r\n", minimalePosition[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]);
                        Console.WriteLine("maximumPosition : %ld\r\n", this.maximumPosition[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]);
                        Console.WriteLine("aktuelleSollPosition : %ld\r\n", aktuelleSollPosition[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]);
                        Console.WriteLine("minimumPositionLimit : %ld\r\n", minimumPositionLimit[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]);
                        Console.WriteLine("maximumPositionLimit : %ld\r\n", maximumPositionLimit[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]);
                        Console.WriteLine("PositionsLimitToleranz Min : %ld\r\n", this.entspannungsschrittMinPosition[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]);
                        Console.WriteLine("PositionsLimitToleranz Max : %ld\r\n", this.entspannungsschrittMaxPosition[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]);
                        Console.WriteLine("mittelpunkt : %ld\r\n", mittelpunkt[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]);
                        calibrationStep[MOTOR_NUMBER_ARRAY_INDEX(motorNumber)]++;
                        break;
                    case CALIBRATION_FINISHED:
                        break;
                }
            }
        }       
    }
}
#endif