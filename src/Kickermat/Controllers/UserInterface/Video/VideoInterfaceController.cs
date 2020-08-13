using System;
using System.Linq;
using System.Threading.Tasks;
using Api.UserInterface.Video;
using Kickermat.Controllers;
using Microsoft.AspNetCore.Mvc;
using Webapp.Services;

namespace Webapp.Controllers.UserInterface.Video
{
    [Route("api/ui/video")]
    [ApiController]
    public class VideoInterfaceController : BaseVideoController
    {
        private readonly PlayerService _playerService;
        private readonly PeripheralsService _peripheralsService;

        public VideoInterfaceController(
            PlayerService playerService, PeripheralsService peripheralsService)
        {
            _playerService = playerService;
            _peripheralsService = peripheralsService;
        }

        [HttpGet]
        public async Task<IActionResult> Video([FromQuery(Name = "player")] string? playerName)
        {
            // Validation
            if (playerName == null)
            {
                return BadRequest("Cannot get video: Please specify a player!");
            }

            if (!_playerService.Players.TryGetValue(playerName, out var player))
            {
                return BadRequest($"Cannot get video: Player {player} does not exist!");
            }

            if (!(player is IVideoProvider))
            {
                return BadRequest($"Player {player} does not provide a video user interface!");
            }

            await VideoTask((player as IVideoProvider).VideoInterface);
            return Ok();
        }

        [HttpGet("channel")]
        public IActionResult GetChannels([FromQuery(Name = "player")] string? playerName)
        { 
            if (playerName == null)
            {
                return BadRequest("Cannot get video channels: Please specify a player!");
            }

            if (!_playerService.Players.TryGetValue(playerName, out var player))
            {
                return BadRequest($"Cannot get video channels: Player {player} does not exist!");
            }

            if (!(player is IVideoProvider))
            {
                BadRequest($"Player {player} does not provide a video user interface!");
            }

            var videoInterface = (player as IVideoProvider).VideoInterface;

            return Ok(new ChannelsResponse(videoInterface.Channels, videoInterface.Channel));
        }

        [HttpPost("channel")]
        public IActionResult SwitchChannel(
            [FromQuery(Name = "player")] string? playerName,
            [FromBody] IVideoChannel? channel)
        {
            if (playerName == null)
            {
                return BadRequest("Cannot switch video channels: Please specify a player!");
            }

            if (!_playerService.Players.TryGetValue(playerName, out var player))
            {
                return BadRequest($"Cannot switch video channel: Player {player} does not exist!");
            }

            if (channel == null)
            {
                return BadRequest(@$"Cannot switch video channel for player {player}:
                    Please specify a channel!");
            }

            if (!(player is IVideoProvider))
            {
                BadRequest($"Player {player} does not provide a video user interface!");
            }

            var videoSource = (player as IVideoProvider).VideoInterface;
            if (!videoSource.Channels.Contains(channel))
            {
                return BadRequest($@"Cannot switch to Channel {channel.Name} for player {player}:
                    Channel does not exist!");
            }

            videoSource.Channel = channel;

            return Ok();
        }
    }
}

