﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapp.Player.Api
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal class KickermatPlayerAttribute : Attribute
    {
        public KickermatPlayerAttribute(string name, string description, string[] authors,
            char emoji)
        {
            Name = name;
            Description = description;
            Authors = authors;
            Emoji = emoji;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public string[] Authors { get; set; }

        public char Emoji { get; set; }
    }
}
