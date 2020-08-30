using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api
{
    public class KickermatException : Exception
    {
        public KickermatException(string message)
            : base(message)
        {
        }

        public KickermatException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected KickermatException() { }
    }
}
