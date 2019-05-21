namespace ObjectDetection
{
    using GlobalDataTypes;
    using PluginSystem;
    using System.Drawing;

    /// <summary>
    /// Describes the interface of an own bar detection.
    /// </summary>
    public interface IOwnBarDetection : IObjectDetection
    {
        /// <summary>
        /// Gets the selcted player position.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>The position of the selected player.</returns>
        Position GetPlayerPosition(Player player);

        /// <summary>
        /// Gets the bounding box of a player.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        Rectangle GetPlayerBoundingBox(Player player); 

        /// <summary>
        /// Calibration classes call this method at their start.
        /// </summary>
        void CalibrationStart();
        
        /// <summary>
        /// Calibration classes call this method as they actually start using the detection.
        /// </summary>
        void CalibrationDetection();

        /// <summary>
        /// Calibration classes call this method when they have finished their work.
        /// </summary>        
        void CalibrationFinished();
    }
}