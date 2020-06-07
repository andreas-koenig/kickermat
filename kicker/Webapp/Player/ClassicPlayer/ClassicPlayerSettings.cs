using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Configuration;
using ImageProcessing;
using VideoSource.Dalsa;

namespace Webapp.Player
{
    public class ClassicPlayerSettings : ISettings
    {
        public string Name => "ClassicPlayerSettings";

        [NumberParameter("Test", "A simple test parameter", 10, 0, 20, 1)]
        public double TestNumber { get; set; }
    }
}
