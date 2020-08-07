using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.UserInterface.Video;
using Api.Camera;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webapp.Services;

namespace Webapp.Controllers.UserInterface.Video
{
    [Route("api/ui/video")]
    [ApiController]
    public class VideoInterfaceController : ControllerBase, IObserver<IFrame>
    {
        private const string BOUNDARY = "camera_image";
        private readonly string _contentType = $"multipart/x-mixed-replace;boundary={BOUNDARY}";

        private readonly byte[] _headerBytes = Encoding.ASCII.GetBytes(
            $"\r\n--{BOUNDARY}\r\nContent-Type: image/jpeg\r\n\r\n");

        private readonly PlayerService _playerService;

        private Task _abortTask; // Task that unsubcribes from video source when connection is lost

        public VideoInterfaceController(PlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet]
        public Task Video([FromQuery(Name = "player")] string? playerName)
        {
            // Validation
            if (playerName == null)
            {
                return new Task(
                    () => BadRequest("Cannot get video: Please specify a player!"));
            }

            if (!_playerService.Players.TryGetValue(playerName, out var player))
            {
                return new Task(
                    () => BadRequest($"Cannot get video: Player {player} does not exist!"));
            }

            if (!(player is IVideoProvider))
            {
                return new Task(
                    () => BadRequest($"Player {player} does not provide a video user interface!"));
            }

            // Send headers
            HttpContext.Response.Headers.Add("Cache-Control", "no-store");
            HttpContext.Response.ContentType = _contentType;
            HttpContext.Response.StatusCode = 200;

            // Subscribe to camera
            var subscription = (player as IVideoProvider)
                .VideoInterface
                .Subscribe(this);

            Action abortAction = () => AbortVideo(subscription);
            _abortTask = new Task(abortAction);
            Request.HttpContext.RequestAborted.Register(abortAction);

            return _abortTask;
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

        [NonAction]
        public void OnCompleted()
        {
            _abortTask?.Start();
        }

        [NonAction]
        public void OnError(Exception error)
        {
            _abortTask.Start();
        }

        [NonAction]
        public void OnNext(IFrame frame)
        {
            try
            {
                Response.Body.WriteAsync(_headerBytes);
                Response.Body.WriteAsync(frame.ToBytes());
            }
            catch
            {
                // TODO: Log exception
                _abortTask.Start();
            }
        }

        [NonAction]
        protected void AbortVideo(IDisposable subscription)
        {
            Response.Body.Close();
            subscription?.Dispose();
            GC.Collect();
        }
    }
}
