using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Webapp.Player.Api;

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

        public Dictionary<string, Type> Players { get; }

        private Dictionary<string, Type> CollectPlayers()
        {
            var players = new Dictionary<string, Type>();

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.GetInterfaces().Contains(typeof(IKickermatPlayer)))
                {
                    var playerAttr = type.GetCustomAttribute<KickermatPlayerAttribute>();
                    players.Add(playerAttr.Name, type);
                }
            }

            return players;
        }
    }
}
