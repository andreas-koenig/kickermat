using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Api;
using Api.Player;
using Api.Settings;

namespace Kickermat.Services.Game
{
    public sealed class GameService
    {
        private readonly object _lock = new object();
        private readonly IServiceProvider _services;
        private readonly PlayerService _playerService;

        public GameService(IServiceProvider services, PlayerService playerService)
        {
            _services = services;
            _playerService = playerService;
        }

        public GameState State { get; private set; }

        public IKickermatPlayer CurrentPlayer { get; private set; }

        public void StartGame(string playerId)
        {
            lock (_lock)
            {
                switch (State)
                {
                    case GameState.IsRunning:
                        throw new KickermatException("The Kickermat is already playing");
                    case GameState.IsPaused:
                        throw new KickermatException("The current game is paused");
                    case GameState.NoGame:
                        if (_playerService.Players.TryGetValue(playerId, out var player))
                        {
                            CurrentPlayer = player;
                            CurrentPlayer.Start();
                            State = GameState.IsRunning;
                            return;
                        }

                        throw new KickermatException(
                            $"Cannot start game: Player {playerId} does not exist");
                }
            }
        }

        public void StopGame()
        {
            lock (_lock)
            {
                switch (State)
                {
                    case GameState.IsRunning:
                    case GameState.IsPaused:
                        CurrentPlayer.Stop();
                        State = GameState.NoGame;
                        CurrentPlayer = null;
                        return;
                    case GameState.NoGame:
                        throw new KickermatException(
                            "There is no game running that could be stopped");
                }
            }
        }
    }
}
