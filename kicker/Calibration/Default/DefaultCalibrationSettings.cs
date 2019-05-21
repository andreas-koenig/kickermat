namespace Calibration.Default
{
    using System.ComponentModel;
    using Calibration.Base;

    /// <summary>
    /// Special settings of the <see cref="DefaultCalibration"/> class.
    /// </summary>
    public sealed class DefaultCalibrationSettings : BasicCalibrationSettings
    {
        /// <summary>
        /// Gets or sets the keeper bar position.
        /// </summary>
        [ReadOnly(true)]
        [Description("The distance between the left border and the keeper bar in pixel. (Readonly because determined by calibration)")]        
        public override int KeeperBarPosition { get; set; }

        /// <summary>
        /// Gets or sets the defense bar position.
        /// </summary>
        [ReadOnly(true)]
        [Description("The distance between the left border and the defense bar in pixel. (Readonly because determined by calibration)")]
        public override int DefenseBarPosition { get; set; }

        /// <summary>
        /// Gets or sets the opponent striker bar position.
        /// </summary>
        [ReadOnly(true)]
        [Description("The distance between the left border and the opponent striker bar in pixel. (Readonly because determined by calibration)")]
        public override int OpponentStrikerBarPosition { get; set; }

        /// <summary>
        /// Gets or sets the midfield bar position.
        /// </summary>
        [ReadOnly(true)]
        [Description("The distance between the left border and the midfield bar in pixel. (Readonly because determined by calibration)")]
        public override int MidfieldBarPosition { get; set; }
    }
}