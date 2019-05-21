namespace ImageProcessing.ImageMorphology
{
    using Emgu.CV.CvEnum;
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// User control for configuration of object detection which uses morphologic operations.
    /// </summary>
    public partial class EmguImageMorphologyUserControl : UserControl
    {
        /// <summary>
        /// The used morphology detection Settings.
        /// </summary>
        private readonly EmguImageMorphologySettings currentSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmguImageMorphologyUserControl"/> class.
        /// </summary>
        /// <param name="usedSettings">The used Settings.</param>
        public EmguImageMorphologyUserControl(EmguImageMorphologySettings usedSettings)
        {
            if (usedSettings == null)
            {
                throw new ArgumentNullException("usedSettings");
            }

            this.InitializeComponent();
            this.InitializeMorphologiOcperationsList();
            this.InitializeMorphologyBorderTypeList();
            InitializeMorphologyMaskTypeCombobox();
            this.currentSettings = usedSettings;
            this.ApplySettingsToUserInterface();
        }

        private void InitializeMorphologyMaskTypeCombobox()
        {
            this.comboBoxMorphologyMaskType.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(MorphologyMaskType)))
            {
                comboBoxMorphologyMaskType.Items.Add(item);
            }
        }

        private void InitializeMorphologyBorderTypeList()
        {
            this.comboBoxBorderType.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(BorderType)))
            {
                this.comboBoxBorderType.Items.Add(item);
            }
        }

        /// <summary>
        /// Initializes the morphologi ocperations list.
        /// </summary>
        private void InitializeMorphologiOcperationsList()
        {
            this.ComboBoxMorphologicOperation.Items.Clear();
            foreach (MorphOp morphologicOperation in Enum.GetValues(typeof(MorphOp)))
            {
                this.ComboBoxMorphologicOperation.Items.Add(morphologicOperation);
            }
        }

        /// <summary>
        /// Applies the Settings to user interface.
        /// </summary>
        private void ApplySettingsToUserInterface()
        {
            this.ComboBoxMorphologicOperation.SelectedItem = this.currentSettings.PerformedMorphologicOperation;
            this.checkBoxMorphologyEnabled.Checked = this.currentSettings.MorphologyEnabled;
            this.comboBoxBorderType.SelectedItem = currentSettings.BorderType;
            numericUpDownMorphMaskHeight.Value = currentSettings.MorphologyMaskHeight;
            numericUpDownMorphMaskWidth.Value = currentSettings.MorphologyMaskWidth;
            numericUpDownMorphIterations.Value = currentSettings.MorphologyIterations;
            comboBoxMorphologyMaskType.SelectedItem = currentSettings.MorphologyMaskType;
            checkBoxNoiseReduction.Checked = currentSettings.NoiseReductionEnabled;
            checkBoxSmoothingEnabled.Checked = currentSettings.PerformGaussianSmoothing;
            numericUpDownKernelHeight.Value = currentSettings.SmoothKernelHeight;
            numericUpDownKernelWidth.Value = currentSettings.SmoothKernelWidth;
            numericUpDownSigma1.Value = currentSettings.SmoothSigma1;
            numericUpDownSigma2.Value = currentSettings.SmoothSigma2;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the comboBoxMorphologicOperation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ComboBoxMorphologicOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ComboBoxMorphologicOperation.SelectedItem != null)
            {
                this.currentSettings.PerformedMorphologicOperation = (MorphOp)this.ComboBoxMorphologicOperation.SelectedItem;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.currentSettings.MorphologyEnabled = checkBoxMorphologyEnabled.Checked;
        }

        private void numericUpDownMorphIterations_ValueChanged(object sender, EventArgs e)
        {
            currentSettings.MorphologyIterations = (int)numericUpDownMorphIterations.Value;
        }

        private void comboBoxBorderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxBorderType.SelectedItem != null && this.comboBoxBorderType.SelectedItem is BorderType)
            {
                this.currentSettings.BorderType = (BorderType)this.comboBoxBorderType.SelectedItem;
            }
        }

        private void numericUpDownKernelWidth_ValueChanged(object sender, EventArgs e)
        {
            currentSettings.SmoothKernelWidth = (int)numericUpDownKernelWidth.Value;
        }

        private void numericUpDownKernelHeight_ValueChanged(object sender, EventArgs e)
        {
            currentSettings.SmoothKernelHeight = (int)numericUpDownKernelHeight.Value;
        }

        private void numericUpDownSigma1_ValueChanged(object sender, EventArgs e)
        {
            currentSettings.SmoothSigma1 = (int)numericUpDownSigma1.Value;
        }

        private void numericUpDownSigma2_ValueChanged(object sender, EventArgs e)
        {
            currentSettings.SmoothSigma2 = (int)numericUpDownSigma2.Value;
        }

        private void checkBoxSmoothingEnabled_CheckedChanged(object sender, EventArgs e)
        {
            currentSettings.PerformGaussianSmoothing = checkBoxSmoothingEnabled.Checked;
        }

        private void numericUpDownMorphMaskWidth_ValueChanged(object sender, EventArgs e)
        {
            currentSettings.MorphologyMaskWidth = (int)numericUpDownMorphMaskWidth.Value;
        }

        private void numericUpDownMorphMaskHeight_ValueChanged(object sender, EventArgs e)
        {
            currentSettings.MorphologyMaskHeight = (int)numericUpDownMorphMaskHeight.Value;
        }

        private void comboBoxMorphologyMaskType_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentSettings.MorphologyMaskType = (MorphologyMaskType)comboBoxMorphologyMaskType.SelectedItem;
        }

        private void checkBoxNoiseReduction_CheckedChanged(object sender, EventArgs e)
        {
            currentSettings.NoiseReductionEnabled = checkBoxNoiseReduction.Checked;
        }
    }
}