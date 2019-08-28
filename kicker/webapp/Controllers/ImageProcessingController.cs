using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageProcessing.BallSearch;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Webapp.Controllers
{
    [Route("api/imageprocessing")]
    [ApiController]
    public class ImageProcessingController : ControllerBase
    {
        private IBallSearch _ballSearch;

        public ImageProcessingController(IBallSearch ballSearch)
        {
            _ballSearch = ballSearch;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _ballSearch.Start();
            return Ok();
        }
    }
}
