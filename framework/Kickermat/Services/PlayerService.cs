using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Api.Player;
using Microsoft.Extensions.Logging;

namespace Kickermat.Services
{
    public sealed class PlayerService
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _services;

        public PlayerService(IServiceProvider services, IEnumerable<Type> playerTypes)
        {
            _services = services;
            Players = CollectPlayers(playerTypes);
            _logger = services.GetService(typeof(ILogger<PlayerService>)) as ILogger;

            var count = Players.Count;
        }

        /// <summary>
        /// A dictionary of all players with the full type name as the key.
        /// </summary>
        public Dictionary<string, IKickermatPlayer> Players { get; }

        private Dictionary<string, IKickermatPlayer> CollectPlayers(IEnumerable<Type> playerTypes)
        {
            var players = new Dictionary<string, IKickermatPlayer>();

            foreach (var type in playerTypes)
            {
                if (type.GetInterfaces().Contains(typeof(IKickermatPlayer)))
                {
                    var player = _services.GetService(type) as IKickermatPlayer;
                    players.Add(type.FullName, player);
                }
            }

            return players;
        }
    }
}
