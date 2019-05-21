namespace Communication.BasicCanOpen
{
    /// <summary>
    /// Enumeration of all CanOpen command comdes
    /// </summary>
    public enum CanOpenCommandCode
    {
        /// <summary>
        /// No command code
        /// </summary>
        None = 0x00,

        /// <summary>
        /// Command code of SDO write response
        /// </summary>
        WriteResponse = 0x60,

        /// <summary>
        /// Command code or SDO error response
        /// </summary>
        ErrorResponse = 0x80,

        /// <summary>
        /// Command code or SDO read 1 byte response
        /// </summary>
        ReadResponse1Byte = 0x4F,

        /// <summary>
        /// Command code or SDO read 2 byte response
        /// </summary>
        ReadResponse2Byte = 0x4B,

        /// <summary>
        /// Command code or SDO read 3 byte response
        /// </summary>
        ReadResponse3Byte = 0x47,

        /// <summary>
        /// Command code or SDO read 4 byte response
        /// </summary>
        ReadResponse4Byte = 0x43,

        /// <summary>
        /// Command-Codes, die in den R_SDO-Nachrichten verwendet werden Sie beschreiben
        /// die Länge der zu schreibenden Daten bei Schreibbefehlen Unabhängig vom
        /// verwendeten Motor(-hersteller) Der Command-Code zum Lesen von Daten ist
        /// unabhängig von der Datenlänge
        /// Der Command-Code zum Lesen von Daten, ist unabhängig von der Datenlänge
        /// </summary>
        ReadRequest = 0x40,

        /// <summary>
        /// Der Command-Code für SDO-Nachrichten, falls die Datenlänge 1 byte beträgt
        /// </summary>
        WriteRequest1Byte = 0x2F,
        
        /// <summary>
        /// Der Command-Code für SDO-Nachrichten, falls die Datenlänge 2 Bytes beträgt
        /// </summary>
        WriteRequest2Byte = 0x2B,
        
        /// <summary>
        /// Der Command-Code für SDO-Nachrichten, falls die Datenlänge 3 Bytes beträgt
        /// </summary>
        WriteRequest3Byte = 0x27,
       
        /// <summary>
        /// Der Command-Code für SDO-Nachrichten, falls die Datenlänge 4 Bytes beträgt
        /// </summary>
        WriteRequest4Byte = 0x23
    }
}