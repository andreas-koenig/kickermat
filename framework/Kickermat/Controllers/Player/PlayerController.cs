using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Api.Player;
using Kickermat.Services;

namespace Kickermat.Controllers.Player
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
        public ActionResult<IEnumerable<SerializedPlayer>> GetKickermatPlayers()
        {
            var players = new List<SerializedPlayer>();
            _playerService.Players.Values
                .ToList()
                .ForEach(player => players.Add(
                    new SerializedPlayer(
                        player.GetType().FullName,
                        player.Name,
                        player.Description,
                        player.Authors,
                        player.Emoji)));

            return Ok(players);
        }
    }
}

