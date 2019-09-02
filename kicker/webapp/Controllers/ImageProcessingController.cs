using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageProcessing.BallSearch;
using ImageProcessing.BarSearch;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Webapp.Controllers
{
    [Route("api/imageprocessing")]
    [ApiController]
    public class ImageProcessingController : ControllerBase
    {
        private IBallSearch _ballSearch;
        private IBarSearch _barSearch;

        public ImageProcessingController(IBallSearch ballSearch, IBarSearch barSearch)
        {
            _ballSearch = ballSearch;
            _barSearch = barSearch;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _barSearch.Start();
            return Ok();
        }
    }
}
