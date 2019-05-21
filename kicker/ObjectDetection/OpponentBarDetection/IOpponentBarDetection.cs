namespace ObjectDetection
{
    using GlobalDataTypes;
    using PluginSystem;

    /// <summary>
    /// Describes the interface of an opponent bar detection.
    /// </summary>
    public interface IOpponentBarDetection : IObjectDetection
    {
        /// <summary>
        /// Gets the selcted player position.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>The position of the selected player.</returns>
        Position GetPlayerPosition(Player player);
    }
}