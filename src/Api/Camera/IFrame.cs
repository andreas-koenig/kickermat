using System;

namespace Api.Camera
{
    public interface IFrame : ICloneable
    {
        public byte[] ToBytes();
    }
}
