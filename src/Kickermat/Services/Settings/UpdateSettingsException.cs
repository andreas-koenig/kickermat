using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api;

namespace Webapp.Services.Settings
{
    public class UpdateSettingsException : KickermatException
    {
        public UpdateSettingsException(string message, object value)
            : base(message)
        {
            Value = value;
        }

        public object Value { get; }
    }
}
