namespace Game
{
    using System;
    using System.Collections.Generic;

    public class Game
    {
        /// <summary>
        /// Lock object to make class thread-safe.
        /// </summary>
        private static readonly object lockObject = new object();

        //TODO: Should the GameController get a list of ownPlayers?
        private readonly List<Bar> ownBars;
        private readonly List<Bar> opponentBars;

        public CalibrationState State { get; protected set; }
        public int playingFieldWidth { get; private set; }
        public int playingFieldLength { get; private set; }

        /// <summary>
        /// Sets the dimensions of the playing field.
        /// </summary>
        /// <param name="width">The width of the playing field.</param>
        /// <param name="height">The height of the playing field.</param>
        public void SetFieldDimensions(int width, int length)
        {
            this.playingFieldWidth = width;
            this.playingFieldLength = length;
        }

        /// <summary>
        /// Gets the position of the top left corner of the playing field.
        /// </summary>
        public Position PlayingFieldOffset { get; private set; }

        /// <summary>
        /// Gets the position of the center of the playing field.
        /// </summary>
        public Position PlayingFieldCenter { get; private set; }

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
