using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api;

namespace Kickermat.Services.Settings
{
    public class UpdateSettingsException : KickermatException
    {
        public UpdateSettingsException()
        {
        }

        public UpdateSettingsException(string message)
            : base(message)
        {
        }

        public UpdateSettingsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public UpdateSettingsException(string message, object value)
            : base(message)
        {
            Value = value;
        }

        public object Value { get; }
    }
}
