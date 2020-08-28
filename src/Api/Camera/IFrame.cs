using System;

namespace Api.Camera
{
    /// <summary>
    /// This is an common interface for an object that holds a frame. It is used by the
    /// <see cref="ICamera{T}"/> and the <see cref="IVideoInterface"/>.
    /// </summary>
    public interface IFrame : ICloneable, IDisposable
    {
        /// <summary>
        /// Convert the frame to uncompressed byte[] (in PNG format for example)
        /// </summary>
        /// <returns>the uncompressed frame</returns>
        public byte[] ToBytes();

        /// <summary>
        /// Convert the frame to a decently compressed JPG image.
        /// </summary>
        /// <returns>the frame as a JPG image</returns>
        public byte[] ToJpg();
    }
}
