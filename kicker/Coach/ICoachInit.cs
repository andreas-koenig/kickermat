namespace Coach
{
    using GlobalDataTypes;

    /// <summary>
    /// Initialization interface which must be implemented by a coach.
    /// </summary>
    public interface ICoachInit : ICoach
    {
        /// <summary>
        /// Sets the bar x position.
        /// </summary>
        /// <param name="bar">The bar whose x position is set.</param>
        /// <param name="position">The position.</param>
        void SetBarXPosition(Bar bar, int position);

        /// <summary>
        /// Sets the player maximum y position.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="position">The position.</param>
        void SetPlayerMaxYPosition(Player player, int position);

        /// <summary>
        /// Sets the player minimum y position.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="position">The position.</param>
        void SetPlayerMinYPosition(Player player, int position);

        /// <summary>
        /// Sets the dimensions of the playing field.
        /// </summary>
        /// <param name="width">The width of the playing field.</param>
        /// <param name="height">The height of the playing field.</param>
        void SetFieldDimensions(int width, int height);

        /// <summary>
        /// Sets the offset of the playing field on the image.
        /// </summary>
        /// <param name="fieldOffset">The position of the top left corner of the playing field.</param>
        void SetFieldOffset(Position fieldOffset);

        /// <summary>
        /// Sets the center of the playing field on the image.
        /// </summary>
        /// <param name="centerPosition">The position of the center of the playing field.</param>
        void SetFieldCenter(Position centerPosition);

        /// <summary>
        /// Sets the values of vertial markings on the playing field. 
        /// </summary>
        /// <param name="goalTop">The y position of the goal top.</param>
        /// <param name="goalBottom">The y position of the goal bottom.</param>
        /// <param name="goalAreaTop">The y position of the goal area top.</param>
        /// <param name="goalAreaBottom">The y position of the goal area bottom.</param>
        /// <param name="penaltyBoxTop">The y position of the penalty box top.</param>
        /// <param name="penaltyBoxBottom">The y position of the penalty area bottom.</param>
        void SetVerticalValues(int goalTop, int goalBottom, int goalAreaTop, int goalAreaBottom, int penaltyBoxTop, int penaltyBoxBottom);

        void SetParallaxCorrectionParams(int cameraLongZ, int playerShortZ, int ballShortZ);
    }
}
