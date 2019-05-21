namespace Calibration.Base
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Basic settings of the playing field for all calibration classes.
    /// </summary>    
    public class BasicCalibrationSettings
    {
        #region Horizontal

        /// <summary>
        /// Gets or sets the keeper bar position.
        /// </summary>
        [Category("1. Field Horizontal")]
        [DisplayName("(a) KeeperBarPosition")]
        [Description("The distance between the left border and the keeper bar in pixel.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual int KeeperBarPosition { get; set; }

        /// <summary>
        /// Gets or sets the defense bar position.
        /// </summary>
        [Category("1. Field Horizontal")]
        [DisplayName("(b) DefenseBarPosition")]
        [Description("The distance between the left border and the defense bar in pixel.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual int DefenseBarPosition { get; set; }

        /// <summary>
        /// Gets or sets the opponent striker bar position.
        /// </summary>
        [Category("1. Field Horizontal")]
        [DisplayName("(c) OpponentStrikerBarPosition")]
        [Description("The distance between the left border and the opponent striker bar in pixel.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual int OpponentStrikerBarPosition { get; set; }

        /// <summary>
        /// Gets or sets the midfield bar position.
        /// </summary>
        [Category("1. Field Horizontal")]
        [DisplayName("(d) MidfieldBarPosition")]
        [Description("The distance between the left border and the midfield bar in pixel.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual int MidfieldBarPosition { get; set; }

        /// <summary>
        /// Gets or sets the width of the playing field.
        /// </summary>
        [Category("1. Field Horizontal")]
        [DisplayName("(e) PlayingFieldWidth")]
        [Description("The width of the playing field in pixel.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual int PlayingFieldWidth { get; set; }

        #endregion

        #region Vertical

        /// <summary>
        /// Gets or sets the penalty box top.
        /// </summary>
        [Category("2. Field Vertical")]
        [DisplayName("(w) PenaltyBoxTop")]
        [Description("The distance between the top border and the top of the penality box in pixel.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual int PenaltyBoxTop { get; set; }
        
        /// <summary>
        /// Gets or sets the goal area top.
        /// </summary>
        [Category("2. Field Vertical")]
        [DisplayName("(x) GoalAreaTop")]
        [Description("The distance between the top border and the top of the goal area in pixel.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual int GoalAreaTop { get; set; }

        /// <summary>
        /// Gets or sets the goal top.
        /// </summary>
        [Category("2. Field Vertical")]
        [DisplayName("(y) GoalTop")]
        [Description("The distance between the top border and the top of the goal in pixel.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual int GoalTop { get; set; }

        /// <summary>
        /// Gets or sets the height of the playing field.
        /// </summary>
        [Category("2. Field Vertical")]
        [DisplayName("(z) PlayingFieldHeight")]
        [Description("The height of the playing field in pixel.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual int PlayingFieldHeight { get; set; }

        #endregion

        #region Correction

        /// <summary>
        /// Gets or sets the X offset of the playing field's top left corner on the image in pixel.
        /// </summary>
        [Category("3. Image Correction")]
        [DisplayName("PlayingFieldXOffset")]
        [Description("The X offset of the playing field's top left corner on the image in pixel.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual int PlayingFieldXOffset { get; set; }

        /// <summary>
        /// Gets or sets the Y offset of the playing field's top left corner on the image in pixel.
        /// </summary>
        [Category("3. Image Correction")]
        [DisplayName("PlayingFieldYOffset")]
        [Description("The Y offset of the playing field's top left corner on the image in pixel.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual int PlayingFieldYOffset { get; set; }

        /// <summary>
        /// Gets or sets the z distance of the camera above the playing field in mm.
        /// </summary>
        [Category("3. Image Correction")]
        [DisplayName("CameraZDistance (mm)")]
        [Description("The Z distance of the camera above the playing field in mm.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual int CameraZDistance { get; set; }

        /// <summary>
        /// Gets or sets the z distance of the bars above the playing field in mm.
        /// </summary>
        [Category("3. Image Correction")]
        [DisplayName("PlayerZDistance (mm)")]
        [Description("The Z distance of the players heads above the playing field in mm.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual int PlayerZDistance { get; set; }

        /// <summary>
        /// Gets or sets the diameter of the ball in mm. 
        /// </summary>
        [Category("3. Image Correction")]
        [DisplayName("BallDiameter (mm)")]
        [Description("The diameter of the ball in mm.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual int BallDiameter { get; set; }

        #endregion

        /// <summary>
        /// Validates this instance.
        /// </summary>
        internal virtual void Validate()
        {
            // Width and height
            if (this.PlayingFieldWidth < 0)
            {
                this.PlayingFieldWidth = 0;
            }
            
            if (this.PlayingFieldWidth > 10000)
            {
                this.PlayingFieldWidth = 10000;
            }
            
            if (this.PlayingFieldHeight < 0)
            { 
                this.PlayingFieldHeight = 0; 
            }

            if (this.PlayingFieldHeight > 10000)
            {
                this.PlayingFieldHeight = 10000;
            }

            // Horizontal values
            if (this.KeeperBarPosition < 0)
            {
                this.KeeperBarPosition = 0;
            }

            if (this.KeeperBarPosition > this.PlayingFieldWidth / 2)
            {
                this.KeeperBarPosition = this.PlayingFieldWidth / 2;
            }

            if (this.DefenseBarPosition < 0)
            {
                this.DefenseBarPosition = 0;
            }

            if (this.DefenseBarPosition > this.PlayingFieldWidth / 2)
            {
                this.DefenseBarPosition = this.PlayingFieldWidth / 2;
            }

            if (this.OpponentStrikerBarPosition < 0)
            {
                this.OpponentStrikerBarPosition = 0;
            }

            if (this.OpponentStrikerBarPosition > this.PlayingFieldWidth / 2)
            {
                this.OpponentStrikerBarPosition = this.PlayingFieldWidth / 2;
            }

            if (this.MidfieldBarPosition < 0)
            {
                this.MidfieldBarPosition = 0;
            }

            if (this.MidfieldBarPosition > this.PlayingFieldWidth / 2)
            {
                this.MidfieldBarPosition = this.PlayingFieldWidth / 2;
            }

            // Vertical values
            if (this.PenaltyBoxTop < 0)
            {
                this.PenaltyBoxTop = 0;
            }

            if (this.PenaltyBoxTop > this.PlayingFieldHeight / 2)
            {
                this.PenaltyBoxTop = this.PlayingFieldHeight / 2;
            }

            if (this.GoalAreaTop < 0)
            {
                this.GoalAreaTop = 0;
            }

            if (this.GoalAreaTop > this.PlayingFieldHeight / 2)
            {
                this.GoalAreaTop = this.PlayingFieldHeight / 2;
            }

            if (this.GoalTop < 0)
            {
                this.GoalTop = 0;
            }

            if (this.GoalTop > this.PlayingFieldHeight / 2)
            {
                this.GoalTop = this.PlayingFieldHeight / 2;
            }

            // Calibration values
            if (Math.Abs(this.PlayingFieldYOffset) > 1000)
            {
                this.PlayingFieldXOffset = 0;
            }

            if (Math.Abs(this.PlayingFieldYOffset) > 1000)
            {
                this.PlayingFieldXOffset = 0;
            }
            
            if (this.CameraZDistance < 0)
            {
                this.CameraZDistance = 0;
            }

            if (this.CameraZDistance > 10000)
            {
                this.CameraZDistance = 10000;
            }

            if (this.PlayerZDistance < 0)
            {
                this.PlayerZDistance = 0;
            }

            if (this.PlayerZDistance > 1000)
            {
                this.PlayerZDistance = 1000;
            }

            if (this.BallDiameter < 0)
            {
                this.BallDiameter = 0;
            }

            if (this.BallDiameter > 100)
            {
                this.BallDiameter = 100;
            }            
        }        
    }
}