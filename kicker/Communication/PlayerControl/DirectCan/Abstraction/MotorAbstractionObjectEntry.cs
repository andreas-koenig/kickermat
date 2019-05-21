namespace Communication.PlayerControl.DirectCan.Abstraction
{
    /// <summary>
    /// Container for index and subindex of a CanOpen object.
    /// </summary>
    public class MotorAbstractionObjectEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MotorAbstractionObjectEntry"/> class.
        /// </summary>
        /// <param name="index">The index of the object.</param>
        /// <param name="subIndex">The subIndex of the object.</param>
        public MotorAbstractionObjectEntry(ushort index, byte subIndex)
        {
            this.Index = index;
            this.SubIndex = subIndex;
        }

        /// <summary>
        /// Gets or sets the index of the object.
        /// </summary>
        /// <value>The index of the object.</value>
        public ushort Index { get; set; }

        /// <summary>
        /// Gets or sets the subIndex of the object
        /// </summary>
        /// <value>The subIndex of the object.</value>
        public byte SubIndex { get; set; }
    }
}