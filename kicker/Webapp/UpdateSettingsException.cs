using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapp
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
