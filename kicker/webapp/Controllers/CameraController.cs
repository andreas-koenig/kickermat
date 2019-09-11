using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using VideoSource;

namespace Webapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CameraController : ControllerBase, IVideoConsumer
    {
        private const string BOUNDARY = "camera_image";
        private readonly byte[] _imgHeaderBytes;
        private readonly Task _doneTask;
        private readonly IVideoSource _videoSource;

        private readonly ILogger<CameraController> _logger;

        public CameraController(IVideoSource videoSource, ILogger<CameraController> logger)
        {
            _videoSource = videoSource;
            var imgHeader = String.Format("\r\n--{0}\r\nContent-Type: image/jpeg\r\n\r\n", BOUNDARY);
            _imgHeaderBytes = Encoding.ASCII.GetBytes(imgHeader);

            _logger = logger;
            _doneTask = new Task(() => AbortConnection());
        }

        // GET: api/Camera
        [HttpGet]
        public Task Get()
        {
            HttpContext.RequestAborted.Register(AbortConnection);
            HttpContext.Response.Headers.Add("Cache-Control", "no-store");
            HttpContext.Response.ContentType = "multipart/x-mixed-replace;boundary=camera_image";
            HttpContext.Response.StatusCode = 400;

            try
            {
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

            args.Frame.Release();
        }

        [NonAction]
        private void AbortConnection()
        {
            _videoSource.StopAcquisition(this);
            Response.Body.Close();
        }
    }
}
