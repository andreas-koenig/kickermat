using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webapp.Player.Api;
using Webapp.Services;

namespace Webapp.Controllers
{
    [Route("api/kickermat")]
    [ApiController]
    public class KickermatController : ControllerBase
    {
        private readonly KickermatService _kickermatService;
        private readonly SettingsService _settingsService;

        public KickermatController(
            KickermatService kickermatService,
            SettingsService settingsService)
        {
            _kickermatService = kickermatService;
            _settingsService = settingsService;
        }

        [HttpGet("players")]
        public ActionResult<IEnumerable<object>> GetKickermatPlayers()
        {
            var playerAttrs = new List<KickermatPlayerAttribute>();
            foreach (var playerType in _kickermatService.Players.Values)
            {
                playerAttrs.Add(playerType.GetCustomAttribute<KickermatPlayerAttribute>());
            }

            var players = playerAttrs.Select(attr => new
            {
                attr.Name,
                attr.Description,
                attr.Authors,
                attr.Emoji,
            });

            Console.WriteLine(_settingsService.PlayerSettings);

            return Ok(players);
        }
    }
}
