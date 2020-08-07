using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Api.Periphery;
using Microsoft.Extensions.Logging;

namespace Webapp.Services
{
    public class PeripheralsService
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _services;
        private readonly IEnumerable<Type> _peripheralTypes;

        public PeripheralsService(IServiceProvider services, IEnumerable<Type> peripheralTypes)
        {
            _services = services;
            _peripheralTypes = peripheralTypes;
            _logger = services.GetService(typeof(ILogger<PeripheralsService>)) as ILogger;

            var count = peripheralTypes.Count();
            if (count > 0)
            {
                var peripherals = peripheralTypes.Select(p => $"- {p.FullName}")
                    .Aggregate((p1, p2) => $"{p1}\n{p2}");
                _logger.LogInformation(
                    $"Registered {count} peripherals:\n{peripherals}");
            }
            else
            {
                _logger.LogInformation("No peripherals registered");
            }
        }

        public IEnumerable<IPeripheral> Peripherals { get; protected set; }
    }
}
