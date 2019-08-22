using System;
using System.Collections.Generic;
using System.Text;

namespace Configuration
{
    public class KickerParameterException : Exception
    {
        public KickerParameterException(string message) : base(message) { }
    }
}
