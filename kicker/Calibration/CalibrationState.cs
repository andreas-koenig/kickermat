namespace Calibration
{
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
