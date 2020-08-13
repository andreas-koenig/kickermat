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
        internal static IIdentifiable GetIdentifiable(
            Guid id, IEnumerable<IKickermatPlayer> players, IEnumerable<IPeripheral> peripherals)
        {
            var identifiable = players
                .Where(player => player.Id.Equals(id))
                .Cast<IIdentifiable>()
                .FirstOrDefault();

            if (identifiable != null)
            {
                return identifiable;
            }

            return peripherals
                .Where(p => p.Id.Equals(id))
                .FirstOrDefault();
        }
    }
}
