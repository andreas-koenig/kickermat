using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Api.Player;
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
            var playerMap = new Dictionary<KickermatPlayerAttribute, IKickermatPlayer>();
            foreach (var player in _playerService.Players.Values)
            {
                playerMap.Add(
                    player.GetType().GetCustomAttribute<KickermatPlayerAttribute>(),
                    player);
            }

            var players = playerMap.Select(
                entry => new SerializedPlayer(
                    entry.Key.Name,
                    entry.Key.Description,
                    entry.Key.Authors,
                    entry.Key.Emoji,
                    entry.Value.Id));

            return Ok(players);
        }
    }
}
