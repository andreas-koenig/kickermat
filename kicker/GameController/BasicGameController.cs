namespace GameController
{
    using System.Windows.Forms;
    using Coach;
    using GlobalDataTypes;
    using PluginSystem;
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

            ServiceLocator.RegisterUpdateCallback<ICoach>(this.UpdateCoach);
            this.Coach = ServiceLocator.LocateService<ICoach>();
            this.UpdateCoach(this.Coach);
        }

        /// <summary>
        /// Gets or sets the user control of the plugin.
        /// </summary>
        /// <value>The user control of the plugin.</value>
        public UserControl SettingsUserControl { get; protected set; }

        /// <summary>
        /// Gets or sets the execution count.
        /// </summary>
        /// <value>The execution count.</value>
        public int ExecutionCount { get; set; }

        /// <summary>
        /// Gets the coach which controls the players..
        /// </summary>
        /// <value>The coach.</value>
        protected ICoach Coach { get; private set; }

        /// <summary>
        /// Gets the range the ball needs to be to shot within.
        /// Should be at least the radius of the ball (in pixel).
        /// </summary>
        protected int ShootingRange { get; private set; }

        /// <summary>
        /// Executes the game controller.
        /// </summary>
        /// <param name="ballpos">The ball position.</param>
        public void Run(Position ballpos)
        {
            this.ExecutionCount++;
            if (Game.IsGameRunning)
                this.Play(ballpos);
        }

        /// <summary>
        /// Must be overriden by sub classes to implement a playing algorithm.
        /// </summary>
        /// <param name="ballpos">The ball position.</param>
        protected abstract void Play(Position ballpos);

        /// <summary>
        /// Updates the coach.
        /// </summary>
        /// <param name="newCoach">The new coach.</param>
        private void UpdateCoach(ICoach newCoach)
        {
            this.Coach = newCoach;
        }
    }
}