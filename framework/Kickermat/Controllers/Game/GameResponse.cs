using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Api.Player;
using Kickermat.Controllers.Player;
using Kickermat.Services.Game;

namespace Kickermat.Controllers.Game
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
                Player = new SerializedPlayer(
                    player.GetType().FullName,
                    player.Name,
                    player.Description,
                    player.Authors,
                    player.Emoji);
            }
        }

        public GameState State { get; set; }

        public SerializedPlayer Player { get; set; }
    }
}
