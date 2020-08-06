using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotorController;

namespace Webapp.Controllers
{
    [Route("api/motor")]
    [ApiController]
    public class MotorDiagnosticsController : ControllerBase
    {
        // GET: api/Motor
        [HttpGet]
        public ActionResult<IEnumerable<Motor>> GetDiagnostics()
        {
            var motors = Diagnostics.Collect();

            return Ok(motors);
        }
    }
}
