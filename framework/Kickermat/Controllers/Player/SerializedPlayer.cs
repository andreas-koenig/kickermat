using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kickermat.Controllers;

namespace Kickermat.Controllers.Player
{
    public class SerializedPlayer : IEntity
    {
        public SerializedPlayer() { }

        public SerializedPlayer(
            string id, string name, string description, string[] authors, Rune emoji)
        {
            Id = id;
            Name = name;
            Description = description;
            Authors = authors;
            Emoji = emoji.ToString();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<string> Authors { get; set; }

        public string Emoji { get; set; }
    }
}
