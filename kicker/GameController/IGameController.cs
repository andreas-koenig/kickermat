namespace GameController
{
    using GlobalDataTypes;
    using PluginSystem;

    /// <summary>
    /// Interface which describes the game controller
    /// </summary>
    public interface IGameController : IPlayerControl
    {
        /// <summary>
        /// Gets or sets the execution count.
        /// </summary>        
        int ExecutionCount { get; set; }

        /// <summary>
        /// Executes the game controller
        /// </summary>
        /// <param name="ballPosition">The ball position.</param>
        void Run(Position ballPosition);
    }
}