using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Configuration;
using Configuration.Parameter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webapp.Services;

namespace Webapp.Controllers
{
    [Route("api/settings")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly SettingsService _settingsService;
        private readonly KickermatService _kickermatService;

        private readonly JsonSerializerOptions _jsonOptions;

        public SettingsController(
            SettingsService settingsService,
            KickermatService kickermatService)
        {
            _settingsService = settingsService;
            _kickermatService = kickermatService;

            _jsonOptions = new JsonSerializerOptions();
            _jsonOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        }

        // POST: api/settings
        public IActionResult UpdateParameter([FromBody] ParameterUpdate update)
        {
            if (update == null)
            {
                return BadRequest();
            }

            if (update.Settings == null)
            {
                return BadRequest(@"Please provide the name of the settings containing the
                    parameter to update");
            }

            if (update.Parameter == null)
            {
                return BadRequest("Please provide the name of the parameter to update");
            }

            if (update.Value == null)
            {
                return BadRequest($"{update.Settings}.{update.Parameter} cannot be set to null");
            }

            try
            {
                var newVal = _settingsService.UpdateParameter(
                    update.Settings, update.Parameter, update.Value);

                return Ok(new UpdateResponse(
                    $"Successfully updated {update.Settings}.{update.Parameter}", newVal));
            }
            catch (UpdateSettingsException ex)
            {
                return BadRequest(new UpdateResponse(ex.Message, ex.Value));
            }
        }

        // GET: api/Settings
        [HttpGet]
        public IActionResult GetSettingsForPlayer(
            [FromQuery(Name = "player")] string? playerName)
        {
            if (playerName == null)
            {
                return BadRequest("No player name was provided!");
            }

            if (!_kickermatService.Players.TryGetValue(playerName, out Type playerType))
            {
                return BadRequest($"Could not load settings: player {playerName} not found");
            }

            var settings = _settingsService.GetSettings(playerType);
            var response = CreateResponse(settings);

            return Ok(response); // new JsonResult(response, _jsonOptions);
        }

        private IEnumerable<SettingsResponse> CreateResponse(IEnumerable<IWriteable> settings)
        {
            var settingsList = new List<SettingsResponse>();

            settings
                .ToList()
                .ForEach((Action<IWriteable>)(writeable =>
                {
                    var attrs = new List<BaseParameterAttribute>();

                    writeable.ValueObject
                        .GetType()
                        .GetProperties()
                        .ToList()
                        .ForEach(property =>
                        {
                            var attr = property.GetCustomAttribute<BaseParameterAttribute>();
                            if (attr != null)
                            {
                                attr.Value = property.GetValue(writeable.ValueObject);
                                attrs.Add(attr);
                            }
                        });

                    var serializedSettings = new SettingsResponse(
                        (writeable.ValueObject as ISettings).Name,
                        attrs);
                    settingsList.Add(serializedSettings);
                }));

            return settingsList;
        }
    }

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

    public class ParameterUpdate
    {
        public string Settings { get; set; }
        
        public string Parameter { get; set; }

        public object Value { get; set; }
    }

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
