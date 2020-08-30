using System;
using System.Linq;
using System.Threading.Tasks;
using Api.UserInterface.Video;
using Kickermat.Controllers;
using Microsoft.AspNetCore.Mvc;
using Video;
using Kickermat.Services;

namespace Kickermat.Controllers.UserInterface.Video
{
    [Route("api/ui/video")]
    [ApiController]
    public class VideoInterfaceController : BaseVideoController
    {
        private readonly PlayerService _playerService;

        public VideoInterfaceController(PlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet]
        public async Task<IActionResult> Video([FromQuery(Name = "playerId")] string? playerId)
        {
            // Validation
            if (playerId == null)
            {
                return BadRequest("Cannot get video: Please specify a player!");
            }

            if (!_playerService.Players.TryGetValue(playerId, out var player))
            {
                return BadRequest($"Cannot get video: Player {playerId} does not exist!");
            }

            if (!(player is IVideoProvider))
            {
                return BadRequest($"Player {playerId} does not provide a video user interface!");
            }

            await VideoTask((player as IVideoProvider).VideoInterface);
            return Ok();
        }

        [HttpGet("channel")]
        public IActionResult GetChannels([FromQuery(Name = "playerId")] string? playerId)
        { 
            if (playerId == null)
            {
                return BadRequest("Cannot get video channels: Please specify a player!");
            }

            if (!_playerService.Players.TryGetValue(playerId, out var player))
            {
                return BadRequest($"Cannot get video channels: Player {playerId} does not exist!");
            }

            if (!(player is IVideoProvider))
            {
                BadRequest($"Player {playerId} does not provide a video user interface!");
            }

            var videoInterface = (player as IVideoProvider).VideoInterface;

            return Ok(new ChannelsResponse(videoInterface.Channels, videoInterface.Channel));
        }

        [HttpPost("channel")]
        public IActionResult SwitchChannel(
            [FromQuery(Name = "playerId")] string? playerId,
            [FromBody] VideoChannel? channel)
        {
            if (playerId == null)
            {
                return BadRequest("Cannot switch video channels: Please specify a player!");
            }

            if (!_playerService.Players.TryGetValue(playerId, out var player))
            {
                return BadRequest($"Cannot switch video channel: Player {playerId} does not exist!");
            }

            if (channel == null)
            {
                return BadRequest(@$"Cannot switch video channel for player {playerId}:
                    Please specify a channel!");
            }

            if (!(player is IVideoProvider))
            {
                BadRequest($"Player {player} does not provide a video user interface!");
            }

            var videoSource = (player as IVideoProvider).VideoInterface;
            if (!videoSource.Channels.Contains(channel))
            {
                return BadRequest($@"Cannot switch to Channel {channel.Name} for player {playerId}:
                    Channel does not exist!");
            }

            videoSource.Channel = channel;

            return Ok();
        }
    }
}

