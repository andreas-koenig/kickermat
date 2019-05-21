namespace Communication.NetworkLayer.Packets.Udp.Enums
{
    /// <summary>
    /// Auflistung aller Initialisierungsstati des Controllers
    /// </summary>
    public enum ControllerStatus
    {
        /// <summary>
        /// Initialisierung abgeschlossen
        /// </summary>
        Ok = 0x00,
        
        /// <summary>
        /// Initialisierung läuft gerade
        /// </summary>
        Running = 0x01,

        /// <summary>
        /// Fehler bei der Initialisierung aufgetreten
        /// </summary>
        Error = 0x02
    }
}