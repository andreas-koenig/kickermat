using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Configuration;
using Webapp.Player;
using Webapp.Player.Api;

namespace Webapp.Services.Game
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

        public void StartGame(string playerName)
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
                        if (_playerService.Players.TryGetValue(playerName, out var playerType))
                        {
                            CurrentPlayer = (IKickermatPlayer)_services.GetService(playerType);
                            CurrentPlayer.Start();
                            State = GameState.IsRunning;
                            return;
                        }

                        throw new KickermatException(
                            $"Cannot start game: Player {playerName} does not exist");
                }
            }
        }

        public void PauseGame()
        {
            lock (_lock)
            {
                switch (State)
                {
                    case GameState.IsRunning:
                        CurrentPlayer.Pause();
                        State = GameState.IsPaused;
                        return;
                    case GameState.IsPaused:
                        return;
                    case GameState.NoGame:
                        throw new KickermatException(
                            "There is no game running that could be paused");
                }
            }
        }

        public void ResumeGame()
        {
            lock (_lock)
            {
                switch (State)
                {
                    case GameState.IsPaused:
                        CurrentPlayer.Resume();
                        State = GameState.IsRunning;
                        return;
                    case GameState.IsRunning:
                        throw new KickermatException("The game is already running");
                    case GameState.NoGame:
                        throw new KickermatException(
                            "There is no game running that could be resumed");
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
