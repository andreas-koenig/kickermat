using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Api.Player;

namespace Webapp.Services
{
    public sealed class PlayerService
    {
        private readonly IServiceProvider _services;

        public PlayerService(IServiceProvider services)
        {
            _services = services;
            Players = CollectPlayers();
        }

        public Dictionary<string, IKickermatPlayer> Players { get; }

        private Dictionary<string, IKickermatPlayer> CollectPlayers()
        {
            var players = new Dictionary<string, IKickermatPlayer>();

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.GetInterfaces().Contains(typeof(IKickermatPlayer)))
                {
                    var playerAttr = type.GetCustomAttribute<KickermatPlayerAttribute>();

                    var player = _services.GetService(type) as IKickermatPlayer;
                    players.Add(playerAttr.Name, player);
                }
            }

            return players;
        }
    }
}
