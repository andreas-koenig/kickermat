﻿namespace Communication.NetworkConnections.Packets.Udp.Enums
{
    /// <summary>
    /// Initialisationstates of the controller
    /// </summary>
    public enum ControllerStatus
    {
        /// <summary>
        /// Initialisation finished
        /// </summary>
        Ok = 0x00,

        /// <summary>
        /// Initialisation running
        /// </summary>
        Running = 0x01,

        /// <summary>
        /// Fehler bei der Initialisierung aufgetreten
        /// </summary>
        Error = 0x02,
    }
}