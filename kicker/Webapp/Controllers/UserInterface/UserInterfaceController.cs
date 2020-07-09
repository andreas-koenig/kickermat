using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webapp.Api.Player;
using Webapp.Api.UserInterface;
using Webapp.Api.UserInterface.Video;
using Webapp.Api.Video;
using Webapp.Services;

namespace Webapp.Controllers.UserInterface
{
    [Route("api/ui")]
    [ApiController]
    public class UserInterfaceController : ControllerBase
    {
        private PlayerService _playerService;

        public UserInterfaceController(PlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet]
        public IActionResult GetUserInterfaces([FromQuery(Name = "player")] string? playerName)
        {
            if (playerName == null)
            {
                return BadRequest("Cannot get user interfaces: Please specify a player!");
            }

            if (!_playerService.Players.TryGetValue(playerName, out var player))
            {
                return BadRequest(
                    $"Cannot get user interfaces: Player {playerName} does not exist!");
            }

            var interfaces = new List<UserInterfaceType>();
            player.GetType()
                .GetInterfaces()
                .ToList()
                .ForEach(t =>
                {
                    var attr = t.GetCustomAttribute<UserInterfaceAttribute>(true);
                    if (attr != null)
                    {
                        interfaces.Add((attr as UserInterfaceAttribute).Type);
                    }
                });

            return Ok(interfaces);
        }

        [HttpGet("{userInterface}")]
        public object GetUserInterface(
            [FromRoute] UserInterfaceType userInterface,
            [FromQuery(Name = "player")] string? playerName)
        {
            if (playerName == null)
            {
                return BadRequest("Cannot get user interface: Please specify a player!");
            }

            if (!_playerService.Players.TryGetValue(playerName, out var player))
            {
                return BadRequest($"Cannot get user interface: Player {player} does not exist!");
            }

            return userInterface switch
            {
                UserInterfaceType.Video => Video(player),
                _ => BadRequest(),
            };
        }

        [NonAction]
        private Task Video(IKickermatPlayer player)
        {
            // Send headers
            var boundary = "camera_image";
            HttpContext.Response.Headers.Add("Cache-Control", "no-store");
            HttpContext.Response.ContentType = $"multipart/x-mixed-replace;boundary={boundary}";
            HttpContext.Response.StatusCode = 400;
            var headerBytes = Encoding.ASCII.GetBytes(
                $"\r\n--{boundary}\r\nContent-Type: image/jpeg\r\n\r\n");

            // Define what to do on camera error/completion
            IDisposable subscription = null;
            var abortAction = new Action(() =>
            {
                Response.Body.Close();
                subscription?.Dispose();
                GC.Collect();
            });
            var abortTask = new Task(abortAction);

            // Subscribe to camera
            var observer = new VideoObserver<byte[]>(
                (bytes) =>
                {
                    Response.Body.Write(headerBytes);
                    Response.Body.Write(bytes);
                },
                () => abortTask.Start(),
                (ex) => abortTask.Start());

            subscription = (player as IVideoInterface<byte[]>)
                .GetVideoSource()
                .Subscribe(observer);

            Request.HttpContext.RequestAborted.Register(abortAction);

            return abortTask;
        }
    }
}
