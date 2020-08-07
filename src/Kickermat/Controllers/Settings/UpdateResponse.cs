using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapp.Controllers.Settings
{
    public class UpdateResponse
    {
        public UpdateResponse(string message, object value)
        {
            Message = message;
            Value = value;
        }

        public string Message { get; set; }

        public object Value { get; set; }
    }
}
