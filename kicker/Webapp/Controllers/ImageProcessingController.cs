using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageProcessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Webapp.Controllers
{
    [Route("api/imageprocessing")]
    [ApiController]
    public class ImageProcessingController : ControllerBase
    {
        private IImageProcessor _imageProcessor;

        public ImageProcessingController(IImageProcessor imageProcessor)
        {
            _imageProcessor = imageProcessor;
        }

        [HttpGet]
        public IActionResult Get()
        {
            // _barSearch.Start();
            return Ok();
        }
    }
}
