namespace Communication.NetworkConnections.Packets.Udp.Enums
{
    using System;

    /// <summary>
    /// Bitmap for the bits of the PositionValid and AnswerRequested bytes in UDP messages.
    /// </summary>
    [Flags]
    public enum PositionBits : byte
    {
        /// <summary>
        /// No bit is selected.
        /// </summary>
        None = 0x00,

        /// <summary>
        /// Bit of the keeper position.
        /// </summary>
        KeeperPosition = 1 << 0,

        /// <summary>
        /// Bit of the keeper angle.
        /// </summary>
        KeeperAngle = 1 << 1,

        /// <summary>
        /// Bit of the defense position.
        /// </summary>
        DefensePosition = 1 << 2,

        /// <summary>
        /// Bit of the defense angle.
        /// </summary>
        DefenseAngle = 1 << 3,

        /// <summary>
        /// Bit of the midfield position.
        /// </summary>
        MidfieldPosition = 1 << 4,

        /// <summary>
        /// Bit of the midfield angle.
        /// </summary>
        MidfieldAngle = 1 << 5,

        /// <summary>
        /// Bit of the striker position.
        /// </summary>
        StrikerPosition = 1 << 6,

        /// <summary>
        /// Bit of the striker angle.
        /// </summary>
        StrikerAngle = 1 << 7,

        /// <summary>
        /// All bits are selected.
        /// </summary>
        All = 0xFF,
    }
}
