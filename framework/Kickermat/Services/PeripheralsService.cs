using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Api.Camera;
using Api.Periphery;
using Microsoft.Extensions.Logging;

namespace Kickermat.Services
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

            var peripherals = new List<IPeripheral>();
            peripheralTypes.ToList()
                .ForEach(pType =>
                {
                    var peripheral = services.GetService(pType) as IPeripheral;
                    peripherals.Add(peripheral);
                });

            Peripherals = peripherals;

            var cameras = new List<ICamera<IFrame>>();
            var cameraTypes = peripheralTypes.Where(camType =>
            {
                return camType.GetInterfaces().Any(i
                    => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICamera<>));
            });

            cameraTypes.ToList()
                .ForEach(cType =>
                {
                    var camera = services.GetService(cType) as ICamera<IFrame>;
                    cameras.Add(camera);
                });

            Cameras = cameras;
        }

        public IEnumerable<IPeripheral> Peripherals { get; protected set; }

        public IEnumerable<ICamera<IFrame>> Cameras { get; protected set; }
    }
}

