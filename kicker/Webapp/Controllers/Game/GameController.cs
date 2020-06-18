using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webapp.Player.Api;
using Webapp.Services;
using Webapp.Services.Game;

namespace Webapp.Controllers.Game
{
    [Route("api/game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;

        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public ActionResult<GameResponse> GetGameState()
        {
            var resp = new GameResponse(
                _gameService.State,
                _gameService.CurrentPlayer);

            return Ok(resp);
        }

        [HttpPost("start")]
        public IActionResult StartGame([FromQuery(Name = "player")] string? player)
        {
            if (player == null)
            {
                return BadRequest("No player was selected");
            }

            try
            {
                _gameService.StartGame(player);

                var resp = new GameResponse(
                    _gameService.State,
                    _gameService.CurrentPlayer);

                return Ok(resp);
            }
            catch (KickermatException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("stop")]
        public IActionResult StopGame()
        {
            try
            {
                _gameService.StopGame();

                var resp = new GameResponse(
                    _gameService.State,
                    _gameService.CurrentPlayer);

                return Ok(resp);
            }
            catch (KickermatException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
