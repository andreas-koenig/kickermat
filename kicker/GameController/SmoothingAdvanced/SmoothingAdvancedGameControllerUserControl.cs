using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GameController
{
    public partial class SmoothingAdvancedGameControllerUserControl : UserControl
    {
        public SmoothingAdvancedGameControllerUserControl()
        {
            InitializeComponent();
            this.labelPreciseMovementLeft.Text = Convert.ToString(this.preciseDistanceLeft.Value);
            this.labelPreciseMovementRight.Text = Convert.ToString(this.preciseDistanceRight.Value);
            this.labelThresholdValueLeft.Text = Convert.ToString(this.ThresholdLeft.Value);
            this.labelThresholdValueRight.Text = Convert.ToString(this.ThresholdRight.Value);
        }

        /// <summary>
        /// Handles the Scroll event of the preciseDistanceLeft control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">instance containing the event data.</param>
        private void ScrollDistanceLeft(object sender, EventArgs e)
        {
            this.labelPreciseMovementLeft.Text = this.preciseDistanceLeft.Value.ToString();
        }

        /// <summary>
        /// Handles the Scroll event of the preciseDistanceRight control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">instance containing the event data.</param>
        private void ScrollDistanceRight(object sender, EventArgs e)
        {
            this.labelPreciseMovementRight.Text = this.preciseDistanceRight.Value.ToString();
        }

        /// <summary>
        /// Handles the Scroll event of the ThresholdLeft control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">instance containing the event data.</param>
        private void scrollMaximumThresholdLeft(object sender, EventArgs e)
        {
            this.labelThresholdValueLeft.Text = this.ThresholdLeft.Value.ToString();
        }

        /// <summary>
        /// Handles the Scroll event of the ThresholdRight control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">instance containing the event data.</param>
        private void scrollMaximumThresholdRight(object sender, EventArgs e)
        {
            this.labelThresholdValueRight.Text = this.ThresholdRight.Value.ToString();
        }
    }
}
