namespace Communication.BasicCanOpen
{
    /// <summary>
    /// Enumeration of all message base identifiers of CanOpen
    /// </summary>
    public enum CanOpenMessageBaseIdentifier
    {
        /// <summary>
        /// No Identifier
        /// </summary>
        NmtStartStopService = 0x00,

        /// <summary>
        /// message base ID for EMCY receive objects.
        /// </summary>
        Emcy = 0x080,

        /// <summary>
        /// message base ID for NMT objects.
        /// </summary>
        NmtErrorControl = 0x700,

        /// <summary>
        /// message base ID for SDO receive objects.
        /// </summary>
        ReceiveSdo = 0x600,
        
        /// <summary>
        /// message base ID for SDO transmit ojbects.
        /// </summary>
        TransmitSdo = 0x580,
        
        /// <summary>
        /// message base ID for PDO1 receive objects.
        /// </summary>
        ReceivePdo1 = 0x200,

        /// <summary>
        /// message base ID for PDO1 transmit ojbects.
        /// </summary>
        TransmitPdo1 = 0x180,

        /// <summary>
        /// message base ID for PDO2 receive objects.
        /// </summary>
        ReceivePdo2 = 0x300,

        /// <summary>
        /// message base ID for PDO2 transmit ojbects.
        /// </summary>
        TransmitPdo2 = 0x280,

        /// <summary>
        /// message base ID for PDO3 receive objects.
        /// </summary>
        ReceivePdo3 = 0x400,

        /// <summary>
        /// message base ID for PDO3 transmit ojbects.
        /// </summary>
        TransmitPdo3 = 0x380,
    }
}