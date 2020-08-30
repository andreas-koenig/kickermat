using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Camera;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Kickermat.Services;

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
        public IActionResult GetCameras([FromQuery(Name = "id")] string? cameraId)
        {
            if (cameraId != null)
            {
                var camera = _peripheralsService.Cameras
                    .Where(cam => cam.GetType().FullName.Equals(cameraId))
                    .FirstOrDefault();

                if (camera == null)
                {
                    return BadRequest($"Camera {cameraId} not found") as IActionResult;
                }

                return Ok(new Camera(
                    camera.GetType().FullName,
                    camera.Name,
                    camera.PeripheralState));
            }

            var cameras = _peripheralsService.Cameras
                .Select(camera => new Camera(
                    camera.GetType().FullName,
                    camera.Name,
                    camera.PeripheralState));

            return Ok(cameras);
        }

        [HttpGet("video")]
        public async Task<IActionResult> Video([FromQuery(Name = "id")] string? cameraId)
        {
            if (cameraId == null)
            {
                return BadRequest("Please provide an id for the camera");
            }

            var camera = _peripheralsService.Cameras
                .Where(cam => cam.GetType().FullName.Equals(cameraId))
                .FirstOrDefault();

            if (camera != null)
            {
                await VideoTask(camera);
                return Ok();
            }

            return BadRequest($"Cannot find camera {cameraId}");
        }
    }
}

