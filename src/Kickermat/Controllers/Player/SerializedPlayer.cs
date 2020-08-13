using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapp.Controllers.Player
{
    public class SerializedPlayer
    {
        public SerializedPlayer() { }

        public SerializedPlayer(
            string name, string description, string[] authors, char emoji, Guid id)
        {
            Name = name;
            Description = description;
            Authors = authors;
            Emoji = emoji;
            Id = id;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<string> Authors { get; set; }

        public char Emoji { get; set; }

        public Guid Id { get; }
    }
}
