namespace GameController
{
    using Game;

    /// <summary>
    /// Implementation of basic functionality of a game controller
    /// </summary>
    public abstract class BasicGameController : IGameController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicGameController"/> class.
        /// </summary>
        protected BasicGameController()
        {
            this.ShootingRange = 30;
        }

        /// <summary>
        /// Gets or sets the execution count.
        /// </summary>
        /// <value>The execution count.</value>
        //public int ExecutionCount { get; set; }

        /// <summary>
        /// Gets the range the ball needs to be to shot within.
        /// Should be at least the radius of the ball (in pixel).
        /// </summary>
        protected int ShootingRange { get; private set; }

        public abstract void MovePlayer(Bar playerBar, ushort newPlayerPosition, bool waitForResponse);

        /// <summary>
        /// Executes the game controller.
        /// </summary>
        /// <param name="ballpos">The ball position.</param>
        public void Run(Position ballpos)
        {
            //this.ExecutionCount++;
            if (Game.IsGameRunning)
                this.Play(ballpos);
        }

        public abstract void SetAngle(Bar bar, short angle, bool waitForResponse);

        /// <summary>
        /// Must be overriden by sub classes to implement a playing algorithm.
        /// </summary>
        /// <param name="ballpos">The ball position.</param>
        protected abstract void Play(Position ballpos);
    }
}
