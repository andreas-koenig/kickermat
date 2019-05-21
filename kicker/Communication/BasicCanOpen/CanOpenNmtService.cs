namespace Communication.BasicCanOpen
{
    using System;

    /// <summary>
    /// Enumeration of available NMT (Network managent) command.
    /// </summary>
    [Flags]
    public enum NmtService
    {
        /// <summary>
        /// Starts a remote node.
        /// </summary>
        StartRemoteNode = 0x01,

        /// <summary>
        /// Stops a remote node.
        /// </summary>
        StopRemoteNode = 0x02,

        /// <summary>
        /// Sets a node to pre-operational state.
        /// </summary>
        EnterPreOperational = 0x80,

        /// <summary>
        /// Resets a node.
        /// </summary>
        ResetNode = 0x81,

        /// <summary>
        /// Resets communication of a node.
        /// </summary>
        ResetCommunication = 0x82
    }
}