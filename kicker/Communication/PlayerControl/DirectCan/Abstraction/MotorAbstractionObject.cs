namespace Communication.PlayerControl.DirectCan.Abstraction
{
    /// <summary>
    /// Enumeration of all CanOpen objects used by the motor abstraction.
    /// </summary>
    public enum MotorAbstractionObject
    {
        /// <summary>
        /// Drivecom control word.
        /// </summary>
        ControlWord,
        
        /// <summary>
        /// Denominaotr (Nenner) of Faulhaber position factor
        /// </summary>
        FaulhaberPositionScaleFeedConstant,
        
        /// <summary>
        /// Numerator (Zähler) of Faulhaber position factor
        /// </summary>
        FaulhaberPositionScaleNumerator,

        /// <summary>
        /// Max position limit.
        /// </summary>
        MaxPositionLimit,

        /// <summary>
        /// Min position limit.
        /// </summary>
        MinPositionLimit,

        /// <summary>
        /// Motor load.
        /// </summary>
        MotorLoad,

        /// <summary>
        /// Operation mode (set).
        /// </summary>
        OperationMode,

        /// <summary>
        /// Operation mode (get).
        /// </summary>
        OperationModeDisplay,

        /// <summary>
        /// Position at position interface.
        /// </summary>
        PositionAtPositionInterface,

        /// <summary>
        /// PDO2 receive parameters.
        /// </summary>
        ReceivePdo2Parameters,

        /// <summary>
        /// Drivecom status word.
        /// </summary>
        StatusWord,

        /// <summary>
        /// Target position for operation mode point to point.
        /// </summary>
        TargetPositionPointToPoint,

        /// <summary>
        /// Homing method of telemecanique motors.
        /// </summary>
        TelemecaniqueHomingMethod,

        /// <summary>
        /// Position for masssetzen of telemecanique motors.
        /// </summary>
        TelemecaniquePositionForMasssetzen,

        /// <summary>
        /// Denominator (Nenner) for position scaling of telemecanique motors.
        /// </summary>
        TelemecaniquePositionScaleDenom,

        /// <summary>
        /// Numerator (Zähler) for position scaling of telemecanique motors.
        /// </summary>
        TelemecaniquePositionScaleNum,

        /// <summary>
        /// Rotation direction of telemecanique motors.
        /// </summary>
        TelemecaniqueRotationDirection,

        /// <summary>
        /// Software limit parameters of telemecanique motors.
        /// </summary>
        TelemecaniqueSoftLimitParameters,

        /// <summary>
        /// PDO2 transmit parameters.
        /// </summary>
        TransmitPdo2Parameters,
    }
}