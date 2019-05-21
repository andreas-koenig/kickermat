namespace PositionPrediction.Default
{
    partial class DefaultPositionPredictionUserControl
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.parameters = new System.Windows.Forms.GroupBox();
            this.NumericUpDownMaximumAgeOfLastPosition = new System.Windows.Forms.NumericUpDown();
            this.labelMaximumDifference = new System.Windows.Forms.Label();
            this.NumericUpDownFramesToPredict = new System.Windows.Forms.NumericUpDown();
            this.labelFramesToPredict = new System.Windows.Forms.Label();
            this.Enable = new System.Windows.Forms.GroupBox();
            this.EnablePositionPredictionCheckbox = new System.Windows.Forms.CheckBox();
            this.parameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownMaximumAgeOfLastPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownFramesToPredict)).BeginInit();
            this.Enable.SuspendLayout();
            this.SuspendLayout();
            // 
            // parameters
            // 
            this.parameters.Controls.Add(this.NumericUpDownMaximumAgeOfLastPosition);
            this.parameters.Controls.Add(this.labelMaximumDifference);
            this.parameters.Controls.Add(this.NumericUpDownFramesToPredict);
            this.parameters.Controls.Add(this.labelFramesToPredict);
            this.parameters.Location = new System.Drawing.Point(8, 93);
            this.parameters.Name = "parameters";
            this.parameters.Size = new System.Drawing.Size(315, 80);
            this.parameters.TabIndex = 0;
            this.parameters.TabStop = false;
            this.parameters.Text = "Parameters";
            // 
            // NumericUpDownMaximumAgeOfLastPosition
            // 
            this.NumericUpDownMaximumAgeOfLastPosition.Location = new System.Drawing.Point(252, 45);
            this.NumericUpDownMaximumAgeOfLastPosition.Name = "NumericUpDownMaximumAgeOfLastPosition";
            this.NumericUpDownMaximumAgeOfLastPosition.Size = new System.Drawing.Size(56, 20);
            this.NumericUpDownMaximumAgeOfLastPosition.TabIndex = 3;
            this.NumericUpDownMaximumAgeOfLastPosition.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.NumericUpDownMaximumAgeOfLastPosition.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.NumericUpDownMaximumAgeOfLastPosition.ValueChanged += new System.EventHandler(this.NumericUpDownMaximumAgeOfLastPosition_ValueChanged);
            // 
            // labelMaximumDifference
            // 
            this.labelMaximumDifference.AutoSize = true;
            this.labelMaximumDifference.Location = new System.Drawing.Point(6, 52);
            this.labelMaximumDifference.Name = "labelMaximumDifference";
            this.labelMaximumDifference.Size = new System.Drawing.Size(187, 13);
            this.labelMaximumDifference.TabIndex = 2;
            this.labelMaximumDifference.Text = "maximum age of last detected Position";
            // 
            // NumericUpDownFramesToPredict
            // 
            this.NumericUpDownFramesToPredict.Location = new System.Drawing.Point(252, 19);
            this.NumericUpDownFramesToPredict.Name = "NumericUpDownFramesToPredict";
            this.NumericUpDownFramesToPredict.Size = new System.Drawing.Size(57, 20);
            this.NumericUpDownFramesToPredict.TabIndex = 1;
            this.NumericUpDownFramesToPredict.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.NumericUpDownFramesToPredict.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.NumericUpDownFramesToPredict.ValueChanged += new System.EventHandler(this.NumericUpDownFramesToPredict_ValueChanged);
            // 
            // labelFramesToPredict
            // 
            this.labelFramesToPredict.AutoSize = true;
            this.labelFramesToPredict.Location = new System.Drawing.Point(6, 26);
            this.labelFramesToPredict.Name = "labelFramesToPredict";
            this.labelFramesToPredict.Size = new System.Drawing.Size(135, 13);
            this.labelFramesToPredict.TabIndex = 0;
            this.labelFramesToPredict.Text = "number of frames to predict";
            // 
            // Enable
            // 
            this.Enable.Controls.Add(this.EnablePositionPredictionCheckbox);
            this.Enable.Location = new System.Drawing.Point(8, 16);
            this.Enable.Name = "Enable";
            this.Enable.Size = new System.Drawing.Size(315, 54);
            this.Enable.TabIndex = 1;
            this.Enable.TabStop = false;
            this.Enable.Text = "Enable Position Prediction";
            // 
            // EnablePositionPredictionCheckbox
            // 
            this.EnablePositionPredictionCheckbox.AutoSize = true;
            this.EnablePositionPredictionCheckbox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.EnablePositionPredictionCheckbox.Location = new System.Drawing.Point(9, 22);
            this.EnablePositionPredictionCheckbox.Name = "EnablePositionPredictionCheckbox";
            this.EnablePositionPredictionCheckbox.Size = new System.Drawing.Size(59, 17);
            this.EnablePositionPredictionCheckbox.TabIndex = 1;
            this.EnablePositionPredictionCheckbox.Text = "Enable";
            this.EnablePositionPredictionCheckbox.UseVisualStyleBackColor = true;
            this.EnablePositionPredictionCheckbox.CheckedChanged += new System.EventHandler(this.EnablePositionPredictionCheckbox_CheckedChanged);
            // 
            // PositionPredictionUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Enable);
            this.Controls.Add(this.parameters);
            this.Name = "PositionPredictionUserControl";
            this.Size = new System.Drawing.Size(326, 179);
            this.parameters.ResumeLayout(false);
            this.parameters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownMaximumAgeOfLastPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownFramesToPredict)).EndInit();
            this.Enable.ResumeLayout(false);
            this.Enable.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox parameters;
        private System.Windows.Forms.Label labelFramesToPredict;
        private System.Windows.Forms.NumericUpDown NumericUpDownFramesToPredict;
        private System.Windows.Forms.Label labelMaximumDifference;
        private System.Windows.Forms.NumericUpDown NumericUpDownMaximumAgeOfLastPosition;
        private System.Windows.Forms.GroupBox Enable;
        private System.Windows.Forms.CheckBox EnablePositionPredictionCheckbox;


    }
}