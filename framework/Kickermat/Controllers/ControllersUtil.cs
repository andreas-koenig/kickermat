using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api;
using Api.Periphery;
using Api.Player;

namespace Kickermat.Controllers
{
    internal static class ControllersUtil
    {
        internal static INamed GetNamedServiceById(
            string id, IEnumerable<IKickermatPlayer> players, IEnumerable<IPeripheral> peripherals)
        {
            var service = players
                .Where(player => player.GetType().FullName.Equals(id))
                .Cast<INamed>()
                .FirstOrDefault();

            if (service != null)
            {
                return service;
            }

            return peripherals
                .Where(p => p.GetType().FullName.Equals(id))
                .FirstOrDefault();
        }
    }
}
