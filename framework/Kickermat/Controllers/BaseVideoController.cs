using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Camera;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kickermat.Controllers
{
    public abstract class BaseVideoController : ControllerBase, IObserver<IFrame>
    {
        private const string BOUNDARY = "camera_image";
        private readonly string _contentType = $"multipart/x-mixed-replace;boundary={BOUNDARY}";

        private readonly byte[] _headerBytes = Encoding.ASCII.GetBytes(
            $"\r\n--{BOUNDARY}\r\nContent-Type: image/jpeg\r\n\r\n");

        private Task _abortTask; // Task that unsubcribes from video source when connection is lost

        protected Task VideoTask(IObservable<IFrame> observable)
        {
            // Send headers
            HttpContext.Response.Headers.Add("Cache-Control", "no-store");
            HttpContext.Response.ContentType = _contentType;
            HttpContext.Response.StatusCode = 200;

            // Subscribe to camera
            var subscription = observable.Subscribe(this);

            Action abortAction = () => AbortVideo(subscription);
            _abortTask = new Task(abortAction);
            Request.HttpContext.RequestAborted.Register(abortAction);

            return _abortTask;
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
        public async void OnNext(IFrame frame)
        {
            try
            {
                Response.Body.WriteAsync(_headerBytes);
                Response.Body.WriteAsync(frame.ToJpg());
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

