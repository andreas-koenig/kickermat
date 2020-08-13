using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Api;
using Api.Settings;
using Api.Settings.Parameter;
using Kickermat.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webapp.Services;
using Webapp.Services.Settings;

namespace Webapp.Controllers.Settings
{
    [Route("api/settings")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly SettingsService _settingsService;
        private readonly PlayerService _playerService;
        private readonly PeripheralsService _peripheralsService;

        private readonly JsonSerializerOptions _jsonOptions;

        public SettingsController(
            SettingsService settingsService,
            PlayerService playerService,
            PeripheralsService peripheralsService)
        {
            _settingsService = settingsService;
            _playerService = playerService;
            _peripheralsService = peripheralsService;

            _jsonOptions = new JsonSerializerOptions();
            _jsonOptions.PropertyNameCaseInsensitive = true;
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
        public IActionResult GetSettings(
            [FromQuery(Name = "id")] Guid id)
        {
            if (id == null)
            {
                return BadRequest("No id was provided!");
            }

            var identifiable = ControllersUtil.GetIdentifiable(
                id, _playerService.Players.Values, _peripheralsService.Peripherals);
            if (identifiable == default)
            {
                return BadRequest($"No configurable object found for ID {id}");
            }

            var settings = _settingsService.GetSettings(identifiable.GetType());
            var response = CreateResponse(settings);

            return Ok(response);
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
}
