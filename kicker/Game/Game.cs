namespace Game
{
    using System;

    /// <summary>
    /// Class which provides information about the current game state.
    /// </summary>
    public static class Game
    {
        /// <summary>
        /// Lock object to make class thread-safe.
        /// </summary>
        private static readonly object lockObject = new object();

        /// <summary>
        /// Occurs when the game is started.
        /// </summary>
        public static event EventHandler GameStarted;

        /// <summary>
        /// Occurs when the game is stopped.
        /// </summary>
        public static event EventHandler GameStopped;

        /// <summary>
        /// Starts the game.
        /// </summary>
        public static void StartGame()
        {
            lock (lockObject)
            {
                IsGameRunning = true;
                if (GameStarted != null)
                {
                    GameStarted(null, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Stops the game.
        /// </summary>
        public static void StopGame()
        {
            lock (lockObject)
            {
                IsGameRunning = false;
                if (GameStopped != null)
                {
                    GameStopped(null, new EventArgs());
                }
            }
        }

        public static bool IsGameRunning { get; private set; } = false;
    }
}
