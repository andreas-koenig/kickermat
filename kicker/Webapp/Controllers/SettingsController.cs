using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Configuration;
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

        public SettingsController(
            SettingsService settingsService,
            KickermatService kickermatService)
        {
            _settingsService = settingsService;
            _kickermatService = kickermatService;
        }

        // GET: api/Settings
        [HttpGet]
        public ActionResult<IEnumerable<SerializedSettings>> GetSettingsForPlayer(
            [FromQuery(Name = "player")] string playerName)
        {
            if (!_kickermatService.Players.TryGetValue(playerName, out Type playerType))
            {
                return BadRequest($"Could not load settings: player {playerName} not found");
            }

            var settings = _settingsService.GetSettings(playerType);

            return Ok(SerializeSettings(settings));
        }

        private IEnumerable<SerializedSettings> SerializeSettings(IEnumerable<IWriteable> settings)
        {
            var settingsList = new List<SerializedSettings>();

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

                    var serializedSettings = new SerializedSettings(
                        (writeable.ValueObject as ISettings).Name,
                        attrs);
                    settingsList.Add(serializedSettings);
                }));

            return settingsList;
        }
    }

    public class SerializedSettings
    {
        public string Name { get; set; }

        public IEnumerable<BaseParameterAttribute> Settings { get; set; }

        public SerializedSettings(string name, IEnumerable<BaseParameterAttribute> settings)
        {
            Name = name;
            Settings = settings;
        }
    }
}
