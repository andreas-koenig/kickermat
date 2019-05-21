namespace Communication.BasicCanOpen
{
    /// <summary>
    /// Enumeration of all CanOpen errors
    /// </summary>
    public enum CanOpenError
    {
        /// <summary>
        /// No error occoured.
        /// </summary>
        None = 0x00,

        /// <summary>
        /// Error code if receiving a message with an unknown identifier
        /// </summary>
        UnknownMessageIdentifier = 0x01,

        /// <summary>
        /// Fehlercode, wenn der Lesepuffer für SDO Lesenachrichten voll ist
        /// </summary>
        SdoReadBufferFull = 0x02,

        /// <summary>
        /// Fehlercode, wenn der Lesepuffer für NMT Lesenachrichten voll ist
        /// </summary>
        NmtReadBufferFull = 0x03,

        /// <summary>
        /// Fehercode, wenn das Warten auf die Freigabe einer Semaphore nicht erfolgreich war
        /// </summary>
        PendingOnEventFailed = 0x04,

        /// <summary>
        /// Fehercode, wenn eine Semaphore nicht erfolgreich gelöscht werden kann
        /// </summary>
        DeletingEventFailed = 0x05
    }
}