using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Webapp.Controllers.Player;
using Webapp.Player.Api;
using Webapp.Services.Game;

namespace Webapp.Controllers.Game
{
    public class GameResponse
    {
        public GameResponse()
        {
        }

        public GameResponse(GameState state, IKickermatPlayer player)
        {
            State = state;

            if (player == null)
            {
                Player = null;
            }
            else
            {
                var attr = player.GetType().GetCustomAttribute<KickermatPlayerAttribute>();
                Player = new SerializedPlayer(
                    attr.Name, attr.Description, attr.Authors, attr.Emoji);
            }
        }

        public GameState State { get; set; }

        public SerializedPlayer Player { get; set; }
    }
}
