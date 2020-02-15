using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ImageProcessing;
using ImageProcessing.Calibration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using VideoSource;

namespace Webapp.Controllers
{
    public enum VideoSourceType
    {
        Camera = 0,
        Calibration = 1,
        ImageProcessing = 2,
    }

    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase, IVideoConsumer
    {
        private const string BOUNDARY = "camera_image";
        private readonly byte[] _imgHeaderBytes;
        private readonly Task _doneTask;

        private readonly ILogger<VideoController> _logger;

        // VideoSources
        private readonly IVideoSource _camera;
        private readonly ICameraCalibration _calibration;
        private readonly IImageProcessor _imageProcessor;
        private IVideoSource _videoSource;

        public VideoController(
            IVideoSource videoSource,
            ICameraCalibration calibration,
            IImageProcessor imageProcessor,
            ILogger<VideoController> logger)
        {
            _camera = videoSource;
            _calibration = calibration;
            _imageProcessor = imageProcessor;

            var imgHeader = string.Format("\r\n--{0}\r\nContent-Type: image/jpeg\r\n\r\n", BOUNDARY);
            _imgHeaderBytes = Encoding.ASCII.GetBytes(imgHeader);

            _logger = logger;
            _doneTask = new Task(() => AbortConnection());
        }

        // GET: api/camera
        [HttpGet("{sourceStr:videoSourceType}")]
        public Task Get([FromRoute] string sourceStr)
        {
            HttpContext.RequestAborted.Register(AbortConnection);
            HttpContext.Response.Headers.Add("Cache-Control", "no-store");
            HttpContext.Response.ContentType = "multipart/x-mixed-replace;boundary=camera_image";
            HttpContext.Response.StatusCode = 400;

            if (!Enum.TryParse<VideoSourceType>(sourceStr, true, out VideoSourceType sourceType))
            {
                return new Task(() => BadRequest($"{sourceStr} is no valid video source!"));
            }

            _videoSource = GetVideoSource(sourceType);

            try
            {
                _videoSource.StartAcquisition(this);
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not start camera acquisition: {0}", ex);
                var task = new Task(() => Response.StatusCode = 500);
                task.Start();
                return task;
            }

            return _doneTask;
        }

        [HttpGet("{sourceStr:videoSourceType}/channels")]
        public IActionResult GetChannels([FromRoute] string sourceStr)
        {
            if (!Enum.TryParse<VideoSourceType>(sourceStr, true, out VideoSourceType sourceType))
            {
                return BadRequest($"{sourceStr} is no valid video source!");
            }

            var videoSource = GetVideoSource(sourceType);
            var attrs = videoSource.GetType().GetCustomAttributes(typeof(VideoSourceAttribute), true);

            if (attrs.Length == 0)
            {
                return Ok(Array.Empty<string>());
            }

            return Ok(((VideoSourceAttribute)attrs[0]).Channels);
        }

        [NonAction]
        public void OnCameraConnected(object sender, CameraEventArgs args)
        {
            // Not needed
        }

        [NonAction]
        public void OnCameraDisconnected(object sender, CameraEventArgs args)
        {
            _doneTask.Start();
        }

        [NonAction]
        public void OnFrameArrived(object sender, FrameArrivedArgs args)
        {
            var imgBytes = args.Frame.Mat.ImEncode(
                ".jpg", new ImageEncodingParam(ImwriteFlags.JpegQuality, 50));

            Response.Body.Write(_imgHeaderBytes);
            Response.Body.Write(imgBytes, 0, imgBytes.Length);
        }

        [NonAction]
        private IVideoSource GetVideoSource(VideoSourceType videoSource)
        {
            switch (videoSource)
            {
                case VideoSourceType.Camera:
                    return _camera;
                case VideoSourceType.Calibration:
                    return (IVideoSource)_calibration;
                case VideoSourceType.ImageProcessing:
                    return (IVideoSource)_imageProcessor;
                default:
                    return _camera; // TODO: Add ImageProcessing
            }
        }

        [NonAction]
        private void AbortConnection()
        {
            _videoSource.StopAcquisition(this);
            Response.Body.Close();
            GC.Collect();
        }
    }
}
