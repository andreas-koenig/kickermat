namespace Game
{
    //TODO: Werden hier wirklich Motoren kalibriert oder ist nur die Reichweite der Spieler aus Sicht der NET-Anwendung wichtig?
    /// <summary>
    /// Definiert die Stati der Motorkalibrierung
    /// </summary>
    public enum CalibrationState
    {
        /// <summary>
        /// Motorkalibrierung ist gerade aktiv
        /// </summary>
        Running,
       
        /// <summary>
        /// Motorkalibrierung ist abgeschlossen
        /// </summary>
        Finished,
       
        /// <summary>
        /// Fehler in der Motorkalibrierung
        /// </summary>
        Error,
    }
}
