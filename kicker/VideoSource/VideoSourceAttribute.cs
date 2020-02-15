using System;
using System.Collections.Generic;
using System.Text;

namespace VideoSource
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class VideoSourceAttribute : Attribute
    {
        public VideoSourceAttribute(string name, params string[] channels)
        {
            Name = name;
            Channels = channels;
        }

        public string Name { get; protected set; }

        public string[] Channels { get; protected set; }
    }
}
