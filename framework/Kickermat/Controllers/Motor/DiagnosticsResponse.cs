using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Periphery;
using Motor;

namespace Kickermat.Controllers.Motor
{
    public class DiagnosticsResponse : IEntity
    {
        public DiagnosticsResponse() { }

        public string Id { get; set; }

        public string Name { get; set; }

        public PeripheralState PeripheralState { get; set; }

        public IEnumerable<MotorDevice> Motors { get; set; }
    }
}

