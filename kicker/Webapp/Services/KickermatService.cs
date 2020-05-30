using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Webapp.Player.Api;

namespace Webapp.Services
{
    public sealed class KickermatService
    {
        private readonly object _lock = new object();
        private readonly IServiceProvider _services;

        public KickermatService(IServiceProvider services)
        {
            _services = services;
            Players = CollectPlayers();
        }

        public enum GameState
        {
            NoGame,
            IsRunning,
            IsPaused,
        }

        public Dictionary<string, Type> Players { get; private set; }

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
                        if (Players.TryGetValue(playerName, out Type playerType))
                        {
                            CurrentPlayer = (IKickermatPlayer)_services.GetService(playerType);
                            CurrentPlayer.Startup();
                            CurrentPlayer.Play();
                            State = GameState.IsRunning;
                        }

                        return;
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
                        CurrentPlayer.PauseGame();
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
                        CurrentPlayer.ResumeGame();
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
                        CurrentPlayer.ShutDown();
                        State = GameState.NoGame;
                        return;
                    case GameState.NoGame:
                        throw new KickermatException(
                            "There is no game running that could be stopped");
                }
            }
        }

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
