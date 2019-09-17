using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ImageProcessing.BarSearch;
using ImageProcessing.Calibration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using VideoSource;

namespace Webapp.Controllers
{
    public enum VideoSource
    {
        Camera = 0,
        Calibration = 1,
        ImageProcessing = 2
    }

    [Route("api/[controller]")]
    [ApiController]
    public class CameraController : ControllerBase, IVideoConsumer
    {
        private const string BOUNDARY = "camera_image";
        private readonly byte[] _imgHeaderBytes;
        private readonly Task _doneTask;
        private IVideoSource _videoSource;

        // VideoSources
        private readonly IVideoSource _camera;
        private readonly ICameraCalibration _calibration;
        private readonly IBarSearch _barSearch;

        private readonly ILogger<CameraController> _logger;

        public CameraController(
            IVideoSource videoSource,
            ICameraCalibration calibration,
            IBarSearch barSearch,
            ILogger<CameraController> logger)
        {
            _camera = videoSource;
            _calibration = calibration;
            _barSearch = barSearch;

            var imgHeader = String.Format("\r\n--{0}\r\nContent-Type: image/jpeg\r\n\r\n", BOUNDARY);
            _imgHeaderBytes = Encoding.ASCII.GetBytes(imgHeader);

            _logger = logger;
            _doneTask = new Task(() => AbortConnection());
        }

        // GET: api/camera
        [HttpGet]
        public Task Get([FromQuery(Name = "videoSource")] VideoSource source)
        {
            HttpContext.RequestAborted.Register(AbortConnection);
            HttpContext.Response.Headers.Add("Cache-Control", "no-store");
            HttpContext.Response.ContentType = "multipart/x-mixed-replace;boundary=camera_image";
            HttpContext.Response.StatusCode = 400;

            try
            {
                _videoSource = GetVideoSource(source);
                _videoSource.StartAcquisition(this);
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not start camera acquisition: {0}", ex);
                var task = new Task<IActionResult>(() => StatusCode(500));
                task.Start();
                return task;
            }

            return _doneTask;
        }

        [NonAction]
        private IVideoSource GetVideoSource(VideoSource source)
        {
            switch (source)
            {
                case VideoSource.Camera:
                    return _camera;
                case VideoSource.Calibration:
                    return (IVideoSource)_calibration;
                case VideoSource.ImageProcessing:
                    return (IVideoSource)_barSearch;
                default:
                    return _camera; // TODO: Add ImageProcessing
            }
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
            var imgBytes = args.Frame.Mat.ImEncode(".jpg",
                new ImageEncodingParam(ImwriteFlags.JpegQuality, 50));

            Response.Body.Write(_imgHeaderBytes);
            Response.Body.Write(imgBytes, 0, imgBytes.Length);
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
