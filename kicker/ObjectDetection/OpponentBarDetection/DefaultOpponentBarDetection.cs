namespace ObjectDetection
{
    using GlobalDataTypes;
    using PluginSystem;

    /// <summary>
    /// Default algorithm for detection opponent bars.
    /// </summary>
    public sealed class DefaultOpponentBarDetection : BasicObjectDetection<DefaultOpponentBarDetectionSettings>, IOpponentBarDetection
    {
        /// <summary>
        /// The returned position.
        /// </summary>
        private readonly Position position;

        /// <summary>
        /// Stores the handle to the label.
        /// </summary>
        private int labelHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultOpponentBarDetection"/> class.
        /// </summary>
        public DefaultOpponentBarDetection()
            : base(1)
        {
            // TODO: Register as service when algorithm is implemented
            ServiceLocator.RegisterService<IOpponentBarDetection>(this);
            this.position = new Position(20, 20, false, false);
        }

        /// <summary>
        /// Gets the selcted player position.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>The position of the selected player.</returns>
        public Position GetPlayerPosition(Player player)
        {
            return this.position;
        }

        /// <summary>
        /// Inits the user control.
        /// </summary>
        public override void InitUserControl()
        {
            base.InitUserControl();
        }

        /// <summary>
        /// Free allocated resources.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
        }

        /// <summary>
        /// Detects the objects.
        /// </summary>
        /// <param name="frameCounter">The frame counter.</param>
        protected override void DetectObjects(ulong frameCounter)
        {
            // TODO: Implement opponent bar detection algorithm (has never been needed)
        }
    }
}