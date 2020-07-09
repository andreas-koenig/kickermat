using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Video
{
    public class VideoChannel : ICloneable
    {
        public VideoChannel()
        {
        }

        public VideoChannel(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public override bool Equals(object obj)
        {
            return obj is VideoChannel channel &&
                   Name == channel.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}
