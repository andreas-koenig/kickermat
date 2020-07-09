using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Settings.Parameter;

namespace Webapp.Controllers.Settings
{
    public class SettingsResponse
    {
        public string Name { get; set; }

        // Use "object" as type param to enable polymorphic JSON serialization with System.Text.Json
        // https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to#serialize-properties-of-derived-classes
        public IEnumerable<object> Parameters
        { get; set; }

        public SettingsResponse(string name, IEnumerable<BaseParameterAttribute> parameters)
        {
            Name = name;
            Parameters = parameters;
        }
    }
}
