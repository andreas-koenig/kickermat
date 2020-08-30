using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Settings.Parameter;

namespace Kickermat.Controllers.Settings
{
    public class SettingsResponse
    {
        public SettingsResponse(
            string settingsId, string name, IEnumerable<BaseParameterAttribute> parameters)
        {
            Id = settingsId;
            Name = name;
            Parameters = parameters;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        // Use "object" as type param to enable polymorphic JSON serialization with System.Text.Json
        // https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to#serialize-properties-of-derived-classes
        public IEnumerable<object> Parameters { get; set; }
    }
}
