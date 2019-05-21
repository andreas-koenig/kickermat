namespace Communication.PlayerControl.DirectCan.Abstraction
{
    /// <summary>
    /// Enumeration of MotorAbstraction error codes.
    /// </summary>
    public enum MotorAbstractionError
    {
        /// <summary>
        /// No error occoured.
        /// </summary>
        NoError = 0,

        /// <summary>
        /// Error code for unknown received nmt state
        /// </summary>
        UnknownNmtState = 1,

        /// <summary>
        /// Error code for unknown received homing method
        /// </summary>
        UnknownHomingMethod = 0x02,

        /// <summary>
        /// Error for unknown received RPDO2 state
        /// </summary>
        UnknownRpdo2State = 0x03,

        /// <summary>
        /// Error for unknown received TPDO2 state
        /// </summary>
        UnknownTpdo2State = 0x04,

        /// <summary>
        /// Error for unknown min position limit state
        /// </summary>
        UnknownMinPositionLimitState = 0x05,

        /// <summary>
        /// Error for unknown max position limit state
        /// </summary>
        UnknownMaxPositionLimitState = 0x06,

        /// <summary>
        /// Error for unknown operation mode
        /// </summary>
        UnknownOperationMode = 0x07,

        /// <summary>
        /// Error for unknown operation state
        /// </summary>
        UnknownOperationState = 0x08
    }
}