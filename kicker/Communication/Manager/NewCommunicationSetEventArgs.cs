namespace Communication.Manager
{
    using System;
    using Sets;

    /// <summary>
    /// Event arguments for a new communication set.
    /// </summary>
    public class NewCommunicationSetEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewCommunicationSetEventArgs"/> class.
        /// </summary>
        /// <param name="newSet">The new set.</param>
        public NewCommunicationSetEventArgs(ICommunicationSet newSet)
        {
            this.NewCommunicationSet = newSet;
        }

        /// <summary>
        /// Gets the new communication set.
        /// </summary>
        /// <value>The new communication set.</value>
        public ICommunicationSet NewCommunicationSet { get; private set; }
    }
}