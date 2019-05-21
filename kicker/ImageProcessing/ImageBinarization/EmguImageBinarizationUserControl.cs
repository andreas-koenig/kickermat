namespace ImageProcessing.ImageBinarization
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Utilities;

    /// <summary>
    /// User control for setting up image binarization.
    /// </summary>
    public partial class EmguImageBinarizationUserControl : UserControl
    {
        /// <summary>
        /// The Settings used by the current instance for binarization.
        /// </summary>
        private readonly EmguImageBinarizationSettings imageBinarizationSettings;


        /// <summary>
        /// Initializes a new instance of the <see cref="EmguImageBinarizationUserControl"/> class.
        /// </summary>
        /// <param name="planeDescription">The plane description.</param>
        /// <param name="settings">The Settings.</param>
        /// <param name="colorModell">The color modell.</param>
        public EmguImageBinarizationUserControl(string[] planeDescription, EmguImageBinarizationSettings settings)
        {
            if (planeDescription == null)
            {
                throw new ArgumentNullException("planeDescription");
            }

            if (planeDescription.Length != 3)
            {
                throw new ArgumentException("planeDescription must have a length of 3");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            this.imageBinarizationSettings = settings;
            this.InitializeComponent();

            this.lblPlane0.Text = planeDescription[0];
            this.lblPlane1.Text = planeDescription[1];
            this.lblPlane2.Text = planeDescription[2];

            this.ApplySettingsToUserInterface();
        }

        /// <summary>
        /// Lädt alle Einstellungen der verwalteten Objekterkennung in die Benutzeroberfläche
        /// </summary>
        public void ApplySettingsToUserInterface()
        {
            this.CheckBoxPlane0Used.Checked = this.imageBinarizationSettings.BinarizePlane[0];
            this.NumericUpDownPlane0Max.Value = this.imageBinarizationSettings.ColorMaxValues[0];
            this.NumericUpDownPlane0Min.Value = this.imageBinarizationSettings.ColorMinValues[0];
            this.CheckBoxPlane1Used.Checked = this.imageBinarizationSettings.BinarizePlane[1];
            this.NumericUpDownPlane1Max.Value = this.imageBinarizationSettings.ColorMaxValues[1];
            this.NumericUpDownPlane1Min.Value = this.imageBinarizationSettings.ColorMinValues[1];
            this.CheckBoxPlane2Used.Checked = this.imageBinarizationSettings.BinarizePlane[2];
            this.NumericUpDownPlane2Max.Value = this.imageBinarizationSettings.ColorMaxValues[2];
            this.NumericUpDownPlane2Min.Value = this.imageBinarizationSettings.ColorMinValues[2];
            this.checkBoxPlane0Invert.Checked = this.imageBinarizationSettings.InvertPlane[0];
            this.checkBoxPlane1Invert.Checked = this.imageBinarizationSettings.InvertPlane[1];
            this.checkBoxPlane2Invert.Checked = this.imageBinarizationSettings.InvertPlane[2];

            // Updated enabled state of controls
            this.CheckBoxPlane0Used_CheckedChanged(this, new EventArgs());
            this.CheckBoxPlane1Used_CheckedChanged(this, new EventArgs());
            this.CheckBoxPlane2Used_CheckedChanged(this, new EventArgs());
        }

        /// <summary>
        /// Handles the ValueChanged event of the updownPlane0Max control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void NumericUpDownPlane0Max_ValueChanged(object sender, EventArgs e)
        {
            this.imageBinarizationSettings.ColorMaxValues[0] = (byte)this.NumericUpDownPlane0Max.Value;
            this.DisplaySelectedColorInUserInterface();
        }

        /// <summary>
        /// Handles the ValueChanged event of the updownPlane0Min control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void NumericUpDownPlane0Min_ValueChanged(object sender, EventArgs e)
        {
            this.imageBinarizationSettings.ColorMinValues[0] = (byte)this.NumericUpDownPlane0Min.Value;
            this.DisplaySelectedColorInUserInterface();
        }

        /// <summary>
        /// Handles the ValueChanged event of the updownPlane1Max control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void NumericUpDownPlane1Max_ValueChanged(object sender, EventArgs e)
        {
            this.imageBinarizationSettings.ColorMaxValues[1] = (byte)this.NumericUpDownPlane1Max.Value;
            this.DisplaySelectedColorInUserInterface();
        }

        /// <summary>
        /// Handles the ValueChanged event of the updownPlane1Min control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void NumericUpDownPlane1Min_ValueChanged(object sender, EventArgs e)
        {
            this.imageBinarizationSettings.ColorMinValues[1] = (byte)this.NumericUpDownPlane1Min.Value;
            this.DisplaySelectedColorInUserInterface();
        }

        /// <summary>
        /// Handles the ValueChanged event of the updownPlane2Max control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void NumericUpDownPlane2Max_ValueChanged(object sender, EventArgs e)
        {
            this.imageBinarizationSettings.ColorMaxValues[2] = (byte)this.NumericUpDownPlane2Max.Value;
            this.DisplaySelectedColorInUserInterface();
        }

        /// <summary>
        /// Handles the ValueChanged event of the updownPlane2Min control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void NumericUpDownPlane2Min_ValueChanged(object sender, EventArgs e)
        {
            this.imageBinarizationSettings.ColorMinValues[2] = (byte)this.NumericUpDownPlane2Min.Value;
            this.DisplaySelectedColorInUserInterface();
        }

        /// <summary>
        /// Displays the selected color in user interface.
        /// </summary>
        private void DisplaySelectedColorInUserInterface()
        {
            byte maxValue0 = 0;
            byte minValue0 = 0;
            byte maxValue1 = 0;
            byte minValue1 = 0;
            byte maxValue2 = 0;
            byte minValue2 = 0;

            //switch (this.currentColorModell)
            //{
            //    case Cvb.Image.TColorModel.RGB:
            //        maxValue0 = 255;
            //        minValue0 = 0;
            //        maxValue1 = 255;
            //        minValue1 = 0;
            //        maxValue2 = 255;
            //        minValue2 = 0;
            //        break;
            //    case Cvb.Image.TColorModel.HLS:
            //        maxValue0 = 255;
            //        minValue0 = 0;
            //        maxValue1 = 127;
            //        minValue1 = 127;
            //        maxValue2 = 255;
            //        minValue2 = 0;
            //        break;
            //    default:
            //        throw new NotSupportedException(this.currentColorModell + " not supported");
            //}

            if (this.CheckBoxPlane0Used.Checked)
            {
                maxValue0 = Convert.ToByte(this.NumericUpDownPlane0Max.Value);
                minValue0 = Convert.ToByte(this.NumericUpDownPlane0Min.Value);
            }

            if (this.CheckBoxPlane1Used.Checked)
            {
                maxValue1 = Convert.ToByte(this.NumericUpDownPlane1Max.Value);
                minValue1 = Convert.ToByte(this.NumericUpDownPlane1Min.Value);
            }

            if (this.CheckBoxPlane2Used.Checked)
            {
                maxValue2 = Convert.ToByte(this.NumericUpDownPlane2Max.Value);
                minValue2 = Convert.ToByte(this.NumericUpDownPlane2Min.Value);
            }

            this.panelMaxColor.BackColor = Color.FromArgb(maxValue0, maxValue1, maxValue2);
            this.panelMinColor.BackColor = Color.FromArgb(minValue0, minValue1, minValue2);

            //switch (this.currentColorModell)
            //{
            //    case Cvb.Image.TColorModel.RGB:
            //        this.panelMaxColor.BackColor = Color.FromArgb(maxValue0, maxValue1, maxValue2);
            //        this.panelMinColor.BackColor = Color.FromArgb(minValue0, minValue1, minValue2);
            //        break;
            //    case Cvb.Image.TColorModel.HLS:
            //        this.panelMaxColor.BackColor = SwissKnife.HLStoRGB(maxValue0, maxValue1, maxValue2);
            //        this.panelMinColor.BackColor = SwissKnife.HLStoRGB(minValue0, minValue1, minValue2);
            //        break;
            //    default:
            //        throw new NotSupportedException(this.currentColorModell + " not supported");
            //}
        }

        /// <summary>
        /// Handles the CheckedChanged event of the CheckBoxPlane0Used control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CheckBoxPlane0Used_CheckedChanged(object sender, EventArgs e)
        {
            this.CheckForAtLeastOneCheckedCheckBox(this.CheckBoxPlane0Used);
            this.imageBinarizationSettings.BinarizePlane[0] = this.CheckBoxPlane0Used.Checked;
            this.NumericUpDownPlane0Max.Enabled = this.CheckBoxPlane0Used.Checked;
            this.NumericUpDownPlane0Min.Enabled = this.CheckBoxPlane0Used.Checked;
            this.DisplaySelectedColorInUserInterface();
        }

        /// <summary>
        /// Checks for at least one checked check box.
        /// </summary>
        /// <param name="selectedCheckBox">The selected check box.</param>
        private void CheckForAtLeastOneCheckedCheckBox(CheckBox selectedCheckBox)
        {
            if ((this.CheckBoxPlane0Used.Checked == false) &&
                (this.CheckBoxPlane1Used.Checked == false) &&
                (this.CheckBoxPlane2Used.Checked == false))
            {
                SwissKnife.ShowInformation(this, "At least one plane must be selected.");
                selectedCheckBox.Checked = true;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the CheckBoxPlane1Used control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CheckBoxPlane1Used_CheckedChanged(object sender, EventArgs e)
        {
            this.CheckForAtLeastOneCheckedCheckBox(this.CheckBoxPlane1Used);
            this.imageBinarizationSettings.BinarizePlane[1] = this.CheckBoxPlane1Used.Checked;
            this.NumericUpDownPlane1Max.Enabled = this.CheckBoxPlane1Used.Checked;
            this.NumericUpDownPlane1Min.Enabled = this.CheckBoxPlane1Used.Checked;
            this.DisplaySelectedColorInUserInterface();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the CheckBoxPlane2Used control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CheckBoxPlane2Used_CheckedChanged(object sender, EventArgs e)
        {
            this.CheckForAtLeastOneCheckedCheckBox(this.CheckBoxPlane2Used);
            this.imageBinarizationSettings.BinarizePlane[2] = this.CheckBoxPlane2Used.Checked;
            this.NumericUpDownPlane2Max.Enabled = this.CheckBoxPlane2Used.Checked;
            this.NumericUpDownPlane2Min.Enabled = this.CheckBoxPlane2Used.Checked;
            this.DisplaySelectedColorInUserInterface();
        }

        private void checkBoxPlane0Invert_CheckedChanged(object sender, EventArgs e)
        {
            this.imageBinarizationSettings.InvertPlane[0] = this.checkBoxPlane0Invert.Checked;
            this.DisplaySelectedColorInUserInterface();
        }

        private void checkBoxPlane1Invert_CheckedChanged(object sender, EventArgs e)
        {
            this.imageBinarizationSettings.InvertPlane[1] = this.checkBoxPlane1Invert.Checked;
            this.DisplaySelectedColorInUserInterface();
        }

        private void checkBoxPlane2Invert_CheckedChanged(object sender, EventArgs e)
        {
            this.imageBinarizationSettings.InvertPlane[2] = this.checkBoxPlane2Invert.Checked;
            this.DisplaySelectedColorInUserInterface();
        }
    }
}