using System;
using System.Collections.Generic;
using System.Text;

namespace VideoSource
{
    public class Channel
    {
        public Channel() { }

        public Channel(string id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        // A unique identifier (for URLs)
        public string Id { get; set; }

        // A human-friendly name
        public string Name { get; set; }

        // A description of what the channel displays
        public string Description { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Channel)
            {
                return ((Channel)obj).Id.Equals(Id);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
