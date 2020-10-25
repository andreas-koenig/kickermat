using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Api;
using Api.Player;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Kickermat.Services;
using Kickermat.Services.Game;

namespace Kickermat.Controllers.Game
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
        public IActionResult StartGame([FromQuery(Name = "playerId")] string? playerId)
        {
            if (playerId == null)
            {
                return BadRequest("No player was selected");
            }

            try
            {
                _gameService.StartGame(playerId);

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
