using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kickermat.Controllers.Settings
{
    public class ParameterUpdate
    {
        public string SettingsId { get; set; }

        public string Parameter { get; set; }

        public object Value { get; set; }
    }
}
