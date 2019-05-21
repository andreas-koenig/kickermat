namespace Communication.BasicCan
{
    /// <summary>
    /// Enumeration of all available CAN message identifiers
    /// </summary>
    public enum MessageId
    {
        /// <summary>
        /// Nicht benutzt
        /// </summary>
        Unused = 0,

        /// <summary>
        /// The message identifier for a move bar request
        /// </summary>        
        MoveBarRequestBaseId = 0x10,

        /// <summary>
        /// The message identifier for a move bar response
        /// </summary>
        MoveBarResponseBaseId = 0x20,

        /// <summary>
        /// The message identifier for a rotate bar request
        /// </summary>
        RotateBarRequestBaseId = 0x30,

        /// <summary>
        /// The message identifier for a rotate bar response
        /// </summary>
        RotateBarResponseBaseId = 0x40,

        /// <summary>
        /// The message identifier for a position request
        /// </summary>
        PositionRequestBaseId = 0x50,

        /// <summary>
        /// The message identifier for a position response
        /// </summary>
        PositionResponseBaseId = 0x60,

        /// <summary>
        /// Die CAN Message-ID, die für Anfrage des Init-Status verwenet wird
        /// </summary>
        InitStatusRequestBaseId = 0x210,
        
        /// <summary>
        /// Die CAN Message-ID, die für Antwort des Init-Status verwenet wird
        /// </summary>
        InitStatusResponseBaseId = 0x220,

        /// <summary>
        /// Die CAN Message-ID, die für das Setzen der minimalen/maximalen Stangenposition verwendet wird
        /// </summary>
        MinMaxPositionRequestBaseId = 0x230,
       
        /// <summary>
        /// Die CAN Message-ID, die für als Antwort auf das Setzen der minimalen/maximalen Stangenposition verwendet wird
        /// </summary>
        MinMaxPositionResponseBaseId = 0x240,
        
        /// <summary>
        /// Die CAN Message-ID, die für das Setzen der Stangenlänge benutzt wird
        /// </summary>
        SetBarLengthInPixelRequestBaseId = 0x250,
        
        /// <summary>
        /// Die CAN Message-ID, die für das Setzen des Winkels für den Nullpunkt verwendet wird
        /// </summary>
        SetAngelForZeroPointRequestBaseId = 0x260
    }
}