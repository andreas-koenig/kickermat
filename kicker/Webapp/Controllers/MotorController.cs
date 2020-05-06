using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Webapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotorController : ControllerBase
    {
        // GET: api/Motor
        [HttpGet]
        public ActionResult<IEnumerable<MotorDiagnostics>> GetDiagnostics()
        {
            var diagnostics = new MotorDiagnostics[]
            {
                new MotorDiagnostics("Telemecanique", "Shift", "Keeper", 1, "Operational", "", "", "Point-to-Point"),
                new MotorDiagnostics("Telemecanique", "Shift", "Defense", 2, "Initialization", "", "", "Point-to-Point"),
                new MotorDiagnostics("Telemecanique", "Shift", "Midfield", 3, "Pre-Operational", "", "", "Point-to-Point"),
                new MotorDiagnostics("Telemecanique", "Shift", "Striker", 4, "Operational", "", "", "Point-to-Point"),
                new MotorDiagnostics("Faulhaber", "Rotation", "Keeper", 10, "Operational", "", "", "Profile Position Mode"),
                new MotorDiagnostics("Faulhaber", "Rotation", "Defense", 11, "Initialization", "", "", "Profile Position Mode"),
                new MotorDiagnostics("Faulhaber", "Rotation", "Midfield", 12, "Stopped", "", "", "Profile Position Mode"),
                new MotorDiagnostics("Faulhaber", "Rotation", "Striker", 12, "Operational", "", "", "Profile Position Mode"),
            };

            return Ok(diagnostics);
        }
    }

    public class MotorDiagnostics
    {
        public MotorDiagnostics() { }

        public MotorDiagnostics(string modell, string function, string bar, byte canOpenId,
            string nmtState, string error, string operatingState, string operatingMode)
        {
            Modell = modell;
            Function = function;
            Bar = bar;
            CanOpenId = canOpenId;
            NmtState = nmtState;
            Error = error;
            OperatingState = operatingState;
            OperatingMode = operatingMode;
        }

        public string Modell { get; set; }

        public string Function { get; set; }

        public string Bar { get; set; }

        public byte CanOpenId { get; set; }

        public string NmtState { get; set; }

        public string Error { get; set; }

        public string OperatingState { get; set; }

        public string OperatingMode { get; set; }
    }
}
