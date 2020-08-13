using System;

namespace Api.Camera
{
    public interface IFrame : ICloneable, IDisposable
    {
        public byte[] ToBytes();

        public byte[] ToJpg();
    }
}
