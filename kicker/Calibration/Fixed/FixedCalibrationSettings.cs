namespace Calibration.Fixed
{
    using System.ComponentModel;
    using Calibration.Base;

    /// <summary>
    /// Extended settings of the <see cref="FixedCalibration"/> class.
    /// </summary>
    public sealed class FixedCalibrationSettings : BasicCalibrationSettings
    {
        #region MinPositions

        /// <summary>
        /// Gets or sets the absolute minimum Y position of the Keeper.
        /// </summary>
        [Category("4. Player MinYPositions")]
        [DisplayName("KeeperMin")]
        [Description("The absolute minimum Y position of the Keeper.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int KeeperMin { get; set; }

        /// <summary>
        /// Gets or sets the absolute minimum Y position of DefenseOne.
        /// </summary>
        [Category("4. Player MinYPositions")]
        [DisplayName("DefenseOneMin")]
        [Description("The absolute minimum Y position of DefenseOne.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int DefenseOneMin { get; set; }

        /// <summary>
        /// Gets or sets the absolute minimum Y position of DefenseTwo.
        /// </summary>
        [Category("4. Player MinYPositions")]
        [DisplayName("DefenseTwoMin")]
        [Description("The absolute minimum Y position of DefenseTwo.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int DefenseTowMin { get; set; }

        /// <summary>
        /// Gets or sets the absolute minimum Y position of MidfieldOne.
        /// </summary>
        [Category("4. Player MinYPositions")]
        [DisplayName("MidfieldOneMin")]
        [Description("The absolute minimum Y position of MidfieldOne.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int MidfieldOneMin { get; set; }

        /// <summary>
        /// Gets or sets the absolute minimum Y position of MidfieldTwo.
        /// </summary>
        [Category("4. Player MinYPositions")]
        [DisplayName("MidfieldTwoMin")]
        [Description("The absolute minimum Y position of MidfieldTwo.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int MidfieldTwoMin { get; set; }

        /// <summary>
        /// Gets or sets the absolute minimum Y position of MidfieldThree.
        /// </summary>
        [Category("4. Player MinYPositions")]
        [DisplayName("MidfieldThreeMin")]
        [Description("The absolute minimum Y position of MidfieldThree.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int MidfieldThreeMin { get; set; }

        /// <summary>
        /// Gets or sets the absolute minimum Y position of MidfieldFour.
        /// </summary>
        [Category("4. Player MinYPositions")]
        [DisplayName("MidfieldFourMin")]
        [Description("The absolute minimum Y position of MidfieldFour.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int MidfieldFourMin { get; set; }

        /// <summary>
        /// Gets or sets the absolute minimum Y position of MidfieldFive.
        /// </summary>
        [Category("4. Player MinYPositions")]
        [DisplayName("MidfieldFiveMin")]
        [Description("The absolute minimum Y position of MidfieldFive.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int MidfieldFiveMin { get; set; }

        /// <summary>
        /// Gets or sets the absolute minimum Y position of StrikerOne.
        /// </summary>
        [Category("4. Player MinYPositions")]
        [DisplayName("StrikerOneMin")]
        [Description("The absolute minimum Y position of StrikerOne.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int StrikerOneMin { get; set; }

        /// <summary>
        /// Gets or sets the absolute minimum Y position of StrikerTwo.
        /// </summary>
        [Category("4. Player MinYPositions")]
        [DisplayName("StrikerTwoMin")]
        [Description("The absolute minimum Y position of StrikerTwo.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int StrikerTwoMin { get; set; }

        /// <summary>
        /// Gets or sets the absolute minimum Y position of StrikerThree.
        /// </summary>
        [Category("4. Player MinYPositions")]
        [DisplayName("StrikerThreeMin")]
        [Description("The absolute minimum Y position of StrikerThree.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int StrikerThreeMin { get; set; }

        #endregion

        #region MaxPositions

        /// <summary>
        /// Gets or sets the absolute maximum Y position of the Keeper.
        /// </summary>
        [Category("5. Player MaxYPositions")]
        [DisplayName("KeeperMax")]
        [Description("The absolute maximum Y position of the Keeper.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int KeeperMax { get; set; }

        /// <summary>
        /// Gets or sets the absolute maximum Y position of DefenseOne.
        /// </summary>
        [Category("5. Player MaxYPositions")]
        [DisplayName("DefenseOneMax")]
        [Description("The absolute maximum Y position of DefenseOne.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int DefenseOneMax { get; set; }

        /// <summary>
        /// Gets or sets the absolute maximum Y position of DefenseTwo.
        /// </summary>
        [Category("5. Player MaxYPositions")]
        [DisplayName("DefenseTwoMax")]
        [Description("The absolute maximum Y position of DefenseTwo.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int DefenseTowMax { get; set; }

        /// <summary>
        /// Gets or sets the absolute maximum Y position of MidfieldOne.
        /// </summary>
        [Category("5. Player MaxYPositions")]
        [DisplayName("MidfieldOneMax")]
        [Description("The absolute maximum Y position of MidfieldOne.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int MidfieldOneMax { get; set; }

        /// <summary>
        /// Gets or sets the absolute maximum Y position of MidfieldTwo.
        /// </summary>
        [Category("5. Player MaxYPositions")]
        [DisplayName("MidfieldTwoMax")]
        [Description("The absolute maximum Y position of MidfieldTwo.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int MidfieldTwoMax { get; set; }

        /// <summary>
        /// Gets or sets the absolute maximum Y position of MidfieldThree.
        /// </summary>
        [Category("5. Player MaxYPositions")]
        [DisplayName("MidfieldThreeMax")]
        [Description("The absolute maximum position of MidfieldThree.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int MidfieldThreeMax { get; set; }

        /// <summary>
        /// Gets or sets the absolute maximum Y position of MidfieldFour.
        /// </summary>
        [Category("5. Player MaxYPositions")]
        [DisplayName("MidfieldFourMax")]
        [Description("The absolute maximum Y position of MidfieldFour.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int MidfieldFourMax { get; set; }

        /// <summary>
        /// Gets or sets the absolute maximum Y position of MidfieldFive.
        /// </summary>
        [Category("5. Player MaxYPositions")]
        [DisplayName("MidfieldFiveMax")]
        [Description("The absolute maximum position of MidfieldFive.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int MidfieldFiveMax { get; set; }

        /// <summary>
        /// Gets or sets the absolute maximum Y position of StrikerOne.
        /// </summary>
        [Category("5. Player MaxYPositions")]
        [DisplayName("StrikerOneMax")]
        [Description("The absolute maximum Y position of StrikerOne.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int StrikerOneMax { get; set; }

        /// <summary>
        /// Gets or sets the absolute maximum Y position of StrikerTwo.
        /// </summary>
        [Category("5. Player MaxYPositions")]
        [DisplayName("StrikerTwoMax")]
        [Description("The absolute maximum Y position of StrikerTwo.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int StrikerTwoMax { get; set; }

        /// <summary>
        /// Gets or sets the absolute maximum Y position of StrikerThree.
        /// </summary>
        [Category("5. Player MaxYPositions")]
        [DisplayName("StrikerThreeMax")]
        [Description("The absolute maximum Y position of StrikerThree.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int StrikerThreeMax { get; set; }

        #endregion
    }
}
