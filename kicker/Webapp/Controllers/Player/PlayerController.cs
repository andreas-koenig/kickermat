using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webapp.Api.Player;
using Webapp.Services;

namespace Webapp.Controllers.Player
{
    [Route("api/player")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly PlayerService _playerService;

        public PlayerController(PlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<object>> GetKickermatPlayers()
        {
            var playerAttrs = new List<KickermatPlayerAttribute>();
            foreach (var player in _playerService.Players.Values)
            {
                playerAttrs.Add(player.GetType().GetCustomAttribute<KickermatPlayerAttribute>());
            }

            var players = playerAttrs.Select(
                attr => new SerializedPlayer(attr.Name, attr.Description, attr.Authors, attr.Emoji));

            return Ok(players);
        }
    }
}
