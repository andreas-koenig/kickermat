using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Camera;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webapp.Services;

namespace Kickermat.Controllers.Camera
{
    [Route("api/camera")]
    [ApiController]
    public class CameraController : BaseVideoController
    {
        private readonly PeripheralsService _peripheralsService;

        public CameraController(PeripheralsService peripheralsService)
        {
            _peripheralsService = peripheralsService;
        }

        [HttpGet]
        public IActionResult GetCameras([FromQuery(Name = "name")] string? name)
        {
            if (name != null)
            {
                var camera = _peripheralsService.Cameras
                    .Where(cam => cam.Name.Equals(name))
                    .FirstOrDefault();

                return camera != null
                    ? Ok(camera) as IActionResult
                    : BadRequest($"Camera {name} not found") as IActionResult;
            }

            var cameras = _peripheralsService.Cameras
                .Select(camera => new Camera(camera.Name, camera.PeripheralState, camera.Id));

            return Ok(cameras);
        }

        [HttpGet("video")]
        public async Task<IActionResult> Video([FromQuery(Name = "camera")] string? name)
        {
            if (name == null)
            {
                return BadRequest("Please provide a name for the camera");
            }

            var camera = _peripheralsService.Cameras
                .Where(cam => cam.Name.Equals(name))
                .FirstOrDefault();

            if (camera != null)
            {
                await VideoTask(camera);
                return Ok();
            }

            return BadRequest($"Cannot find camera {name}");
        }
    }
}

