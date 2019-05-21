namespace PositionPrediction.Default
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// User Control for Position Prediction
    /// </summary>
    public partial class DefaultPositionPredictionUserControl : UserControl
    {
        /// <summary>
        /// The settings used by the instance.
        /// </summary>
        private readonly DefaultPositionPredictionSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultPositionPredictionUserControl"/> class.
        /// </summary>
        /// <param name="settings">The Settings.</param>
        public DefaultPositionPredictionUserControl(DefaultPositionPredictionSettings settings)
        {
            this.InitializeComponent();
            this.settings = settings;
            this.ApplySettingsToUserInterface();            
        }

        /// <summary>
        /// Applies the Settings to user interface.
        /// </summary>
        private void ApplySettingsToUserInterface()
        {
            this.NumericUpDownFramesToPredict.Value = this.settings.FramesToPredict;
            this.NumericUpDownMaximumAgeOfLastPosition.Value = this.settings.MaximumDifference;
            this.EnablePositionPredictionCheckbox.Checked = this.settings.PredictionEnabled;
        }

        /// <summary>
        /// Handles the ValueChanged event of the framesToPredictUpDown1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void NumericUpDownFramesToPredict_ValueChanged(object sender, EventArgs e)
        {
            this.settings.FramesToPredict = (int)this.NumericUpDownFramesToPredict.Value;
        }

        /// <summary>
        /// Maximums the difference changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void NumericUpDownMaximumAgeOfLastPosition_ValueChanged(object sender, EventArgs e)
        {
            this.settings.MaximumDifference = (int)this.NumericUpDownMaximumAgeOfLastPosition.Value;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the EnablePositionPredictionCheckbox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void EnablePositionPredictionCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            this.settings.PredictionEnabled = this.EnablePositionPredictionCheckbox.Checked;
        }
    }
}