namespace ObjectSearch.BlobSearch
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// User control for setting up the blob detection
    /// </summary>
    public partial class BlobSearchUserControl : UserControl
    {
        /// <summary>
        /// The current Settings.
        /// </summary>
        private readonly BlobSearchSettings currentSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobSearchUserControl"/> class.
        /// </summary>
        /// <param name="useSettings">The use Settings.</param>
        public BlobSearchUserControl(BlobSearchSettings useSettings)
        {
            this.InitializeComponent();
            this.currentSettings = useSettings;

            this.ApplyBlobSettingsToUserControl();
        }

        /// <summary>
        /// Applies the BLOB Settings to user control.
        /// </summary>
        private void ApplyBlobSettingsToUserControl()
        {
            this.NumericUpDownMaxBlobArea.Value = this.currentSettings.MaxBlobArea;
            this.NumericUpDownMinBlobArea.Value = this.currentSettings.MinBlobArea;
            this.numericUpDownObjectCount.Value = this.currentSettings.MinimumNumberOfObjectsToFind;
            this.NumericUpDownDistanceToSearchNextBlob.Value = this.currentSettings.DistanceToSearchNextBlob;
            this.numericUpDownAOIHeight.Value = currentSettings.AreaOfInterestHeight;
            this.numericUpDownAOIWidth.Value = currentSettings.AreaOfInterestWidth;
        }

        /// <summary>
        /// Handles the ValueChanged event of the updownMaxBlobSize control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void NumericUpDownMaxBlobSize_ValueChanged(object sender, EventArgs e)
        {
            this.currentSettings.MaxBlobArea = Convert.ToInt32(this.NumericUpDownMaxBlobArea.Value);
        }

        /// <summary>
        /// Handles the ValueChanged event of the updownMinBlobSize control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void NumericUpDownMinBlobSize_ValueChanged(object sender, EventArgs e)
        {
            this.currentSettings.MinBlobArea = Convert.ToInt32(this.NumericUpDownMinBlobArea.Value);
        }

        /// <summary>
        /// Handles the ValueChanged event of the updownDistanceToSearchNextBlob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void NumericUpDownDistanceToSearchNextBlob_ValueChanged(object sender, EventArgs e)
        {
            this.currentSettings.DistanceToSearchNextBlob = (int)this.NumericUpDownDistanceToSearchNextBlob.Value;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.currentSettings.MinimumNumberOfObjectsToFind = (int)this.numericUpDownObjectCount.Value;
        }

        private void numericUpDownAOIWidth_ValueChanged(object sender, EventArgs e)
        {
            this.currentSettings.AreaOfInterestWidth = (int)numericUpDownAOIWidth.Value;
        }

        private void numericUpDownAOIHeight_ValueChanged(object sender, EventArgs e)
        {
            currentSettings.AreaOfInterestHeight = (int)numericUpDownAOIHeight.Value;
        }
    }
}