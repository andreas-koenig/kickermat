using System;
using System.Collections.Generic;
using System.Text;

namespace Api.UserInterface.Video
{
    public interface IVideoChannel : ICloneable
    {
        public string Name { get; }

        public string Description { get; }
    }
}
