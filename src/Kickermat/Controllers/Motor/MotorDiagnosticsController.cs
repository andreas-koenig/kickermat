using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Motor;

namespace Kickermat.Controllers.Motor
{
    [Route("api/motor")]
    [ApiController]
    public class MotorDiagnosticsController : ControllerBase
    {
        private readonly MotorController _motorController;

        public MotorDiagnosticsController(MotorController motorController)
        {
            _motorController = motorController;
        }

        // GET: api/Motor
        [HttpGet]
        public ActionResult<DiagnosticsResponse> GetDiagnostics()
        {
            var motors = _motorController.CollectDiagnostics();
            var diagnosticsResponse = new DiagnosticsResponse()
            {
                Id = _motorController.GetType().FullName,
                Name = "Motor Controller",
                PeripheralState = _motorController.PeripheralState,
                Motors = motors,
            };

            return Ok(diagnosticsResponse);
        }
    }
}

