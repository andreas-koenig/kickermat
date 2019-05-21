namespace ImageProcessing.ImageBinarization
{
    partial class EmguImageBinarizationUserControl
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
            this.panelMaxColor = new System.Windows.Forms.Panel();
            this.NumericUpDownPlane0Min = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDownPlane1Max = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDownPlane1Min = new System.Windows.Forms.NumericUpDown();
            this.lblPlane2 = new System.Windows.Forms.Label();
            this.lblPlane1 = new System.Windows.Forms.Label();
            this.groupBoxColorSettings = new System.Windows.Forms.GroupBox();
            this.CheckBoxPlane2Used = new System.Windows.Forms.CheckBox();
            this.CheckBoxPlane1Used = new System.Windows.Forms.CheckBox();
            this.CheckBoxPlane0Used = new System.Windows.Forms.CheckBox();
            this.lblPlane0 = new System.Windows.Forms.Label();
            this.panelMinColor = new System.Windows.Forms.Panel();
            this.lblColorMin = new System.Windows.Forms.Label();
            this.lblColorMax = new System.Windows.Forms.Label();
            this.NumericUpDownPlane0Max = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDownPlane2Min = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDownPlane2Max = new System.Windows.Forms.NumericUpDown();
            this.checkBoxPlane0Invert = new System.Windows.Forms.CheckBox();
            this.checkBoxPlane1Invert = new System.Windows.Forms.CheckBox();
            this.checkBoxPlane2Invert = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownPlane0Min)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownPlane1Max)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownPlane1Min)).BeginInit();
            this.groupBoxColorSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownPlane0Max)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownPlane2Min)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownPlane2Max)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMaxColor
            // 
            this.panelMaxColor.BackColor = System.Drawing.Color.Red;
            this.panelMaxColor.Location = new System.Drawing.Point(246, 32);
            this.panelMaxColor.Name = "panelMaxColor";
            this.panelMaxColor.Size = new System.Drawing.Size(19, 20);
            this.panelMaxColor.TabIndex = 96;
            // 
            // NumericUpDownPlane0Min
            // 
            this.NumericUpDownPlane0Min.Location = new System.Drawing.Point(39, 58);
            this.NumericUpDownPlane0Min.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NumericUpDownPlane0Min.Name = "NumericUpDownPlane0Min";
            this.NumericUpDownPlane0Min.Size = new System.Drawing.Size(63, 20);
            this.NumericUpDownPlane0Min.TabIndex = 1;
            this.NumericUpDownPlane0Min.ValueChanged += new System.EventHandler(this.NumericUpDownPlane0Min_ValueChanged);
            // 
            // NumericUpDownPlane1Max
            // 
            this.NumericUpDownPlane1Max.Location = new System.Drawing.Point(108, 32);
            this.NumericUpDownPlane1Max.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NumericUpDownPlane1Max.Name = "NumericUpDownPlane1Max";
            this.NumericUpDownPlane1Max.Size = new System.Drawing.Size(63, 20);
            this.NumericUpDownPlane1Max.TabIndex = 3;
            this.NumericUpDownPlane1Max.ValueChanged += new System.EventHandler(this.NumericUpDownPlane1Max_ValueChanged);
            // 
            // NumericUpDownPlane1Min
            // 
            this.NumericUpDownPlane1Min.Location = new System.Drawing.Point(108, 58);
            this.NumericUpDownPlane1Min.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NumericUpDownPlane1Min.Name = "NumericUpDownPlane1Min";
            this.NumericUpDownPlane1Min.Size = new System.Drawing.Size(63, 20);
            this.NumericUpDownPlane1Min.TabIndex = 4;
            this.NumericUpDownPlane1Min.ValueChanged += new System.EventHandler(this.NumericUpDownPlane1Min_ValueChanged);
            // 
            // lblPlane2
            // 
            this.lblPlane2.AutoSize = true;
            this.lblPlane2.Location = new System.Drawing.Point(177, 17);
            this.lblPlane2.Name = "lblPlane2";
            this.lblPlane2.Size = new System.Drawing.Size(28, 13);
            this.lblPlane2.TabIndex = 91;
            this.lblPlane2.Text = "Blue";
            // 
            // lblPlane1
            // 
            this.lblPlane1.AutoSize = true;
            this.lblPlane1.Location = new System.Drawing.Point(105, 16);
            this.lblPlane1.Name = "lblPlane1";
            this.lblPlane1.Size = new System.Drawing.Size(36, 13);
            this.lblPlane1.TabIndex = 90;
            this.lblPlane1.Text = "Green";
            // 
            // groupBoxColorSettings
            // 
            this.groupBoxColorSettings.Controls.Add(this.checkBoxPlane2Invert);
            this.groupBoxColorSettings.Controls.Add(this.checkBoxPlane1Invert);
            this.groupBoxColorSettings.Controls.Add(this.checkBoxPlane0Invert);
            this.groupBoxColorSettings.Controls.Add(this.CheckBoxPlane2Used);
            this.groupBoxColorSettings.Controls.Add(this.CheckBoxPlane1Used);
            this.groupBoxColorSettings.Controls.Add(this.CheckBoxPlane0Used);
            this.groupBoxColorSettings.Controls.Add(this.lblPlane0);
            this.groupBoxColorSettings.Controls.Add(this.panelMinColor);
            this.groupBoxColorSettings.Controls.Add(this.lblColorMin);
            this.groupBoxColorSettings.Controls.Add(this.lblColorMax);
            this.groupBoxColorSettings.Controls.Add(this.NumericUpDownPlane0Max);
            this.groupBoxColorSettings.Controls.Add(this.panelMaxColor);
            this.groupBoxColorSettings.Controls.Add(this.NumericUpDownPlane0Min);
            this.groupBoxColorSettings.Controls.Add(this.NumericUpDownPlane1Max);
            this.groupBoxColorSettings.Controls.Add(this.NumericUpDownPlane1Min);
            this.groupBoxColorSettings.Controls.Add(this.lblPlane2);
            this.groupBoxColorSettings.Controls.Add(this.lblPlane1);
            this.groupBoxColorSettings.Controls.Add(this.NumericUpDownPlane2Min);
            this.groupBoxColorSettings.Controls.Add(this.NumericUpDownPlane2Max);
            this.groupBoxColorSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxColorSettings.Location = new System.Drawing.Point(0, 0);
            this.groupBoxColorSettings.Name = "groupBoxColorSettings";
            this.groupBoxColorSettings.Size = new System.Drawing.Size(281, 133);
            this.groupBoxColorSettings.TabIndex = 100;
            this.groupBoxColorSettings.TabStop = false;
            this.groupBoxColorSettings.Text = "Color Settings";
            // 
            // CheckBoxPlane2Used
            // 
            this.CheckBoxPlane2Used.AutoSize = true;
            this.CheckBoxPlane2Used.Location = new System.Drawing.Point(177, 84);
            this.CheckBoxPlane2Used.Name = "CheckBoxPlane2Used";
            this.CheckBoxPlane2Used.Size = new System.Drawing.Size(51, 17);
            this.CheckBoxPlane2Used.TabIndex = 8;
            this.CheckBoxPlane2Used.Text = "Used";
            this.CheckBoxPlane2Used.UseVisualStyleBackColor = true;
            this.CheckBoxPlane2Used.CheckedChanged += new System.EventHandler(this.CheckBoxPlane2Used_CheckedChanged);
            // 
            // CheckBoxPlane1Used
            // 
            this.CheckBoxPlane1Used.AutoSize = true;
            this.CheckBoxPlane1Used.Location = new System.Drawing.Point(108, 84);
            this.CheckBoxPlane1Used.Name = "CheckBoxPlane1Used";
            this.CheckBoxPlane1Used.Size = new System.Drawing.Size(51, 17);
            this.CheckBoxPlane1Used.TabIndex = 5;
            this.CheckBoxPlane1Used.Text = "Used";
            this.CheckBoxPlane1Used.UseVisualStyleBackColor = true;
            this.CheckBoxPlane1Used.CheckedChanged += new System.EventHandler(this.CheckBoxPlane1Used_CheckedChanged);
            // 
            // CheckBoxPlane0Used
            // 
            this.CheckBoxPlane0Used.AutoSize = true;
            this.CheckBoxPlane0Used.Location = new System.Drawing.Point(39, 84);
            this.CheckBoxPlane0Used.Name = "CheckBoxPlane0Used";
            this.CheckBoxPlane0Used.Size = new System.Drawing.Size(51, 17);
            this.CheckBoxPlane0Used.TabIndex = 2;
            this.CheckBoxPlane0Used.Text = "Used";
            this.CheckBoxPlane0Used.UseVisualStyleBackColor = true;
            this.CheckBoxPlane0Used.CheckedChanged += new System.EventHandler(this.CheckBoxPlane0Used_CheckedChanged);
            // 
            // lblPlane0
            // 
            this.lblPlane0.AutoSize = true;
            this.lblPlane0.Location = new System.Drawing.Point(36, 16);
            this.lblPlane0.Name = "lblPlane0";
            this.lblPlane0.Size = new System.Drawing.Size(27, 13);
            this.lblPlane0.TabIndex = 89;
            this.lblPlane0.Text = "Red";
            // 
            // panelMinColor
            // 
            this.panelMinColor.BackColor = System.Drawing.Color.Red;
            this.panelMinColor.Location = new System.Drawing.Point(246, 58);
            this.panelMinColor.Name = "panelMinColor";
            this.panelMinColor.Size = new System.Drawing.Size(19, 20);
            this.panelMinColor.TabIndex = 97;
            // 
            // lblColorMin
            // 
            this.lblColorMin.AutoSize = true;
            this.lblColorMin.Location = new System.Drawing.Point(6, 60);
            this.lblColorMin.Name = "lblColorMin";
            this.lblColorMin.Size = new System.Drawing.Size(24, 13);
            this.lblColorMin.TabIndex = 87;
            this.lblColorMin.Text = "Min";
            // 
            // lblColorMax
            // 
            this.lblColorMax.AutoSize = true;
            this.lblColorMax.Location = new System.Drawing.Point(6, 34);
            this.lblColorMax.Name = "lblColorMax";
            this.lblColorMax.Size = new System.Drawing.Size(27, 13);
            this.lblColorMax.TabIndex = 85;
            this.lblColorMax.Text = "Max";
            // 
            // NumericUpDownPlane0Max
            // 
            this.NumericUpDownPlane0Max.Location = new System.Drawing.Point(39, 32);
            this.NumericUpDownPlane0Max.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NumericUpDownPlane0Max.Name = "NumericUpDownPlane0Max";
            this.NumericUpDownPlane0Max.Size = new System.Drawing.Size(63, 20);
            this.NumericUpDownPlane0Max.TabIndex = 0;
            this.NumericUpDownPlane0Max.ValueChanged += new System.EventHandler(this.NumericUpDownPlane0Max_ValueChanged);
            // 
            // NumericUpDownPlane2Min
            // 
            this.NumericUpDownPlane2Min.Location = new System.Drawing.Point(177, 58);
            this.NumericUpDownPlane2Min.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NumericUpDownPlane2Min.Name = "NumericUpDownPlane2Min";
            this.NumericUpDownPlane2Min.Size = new System.Drawing.Size(63, 20);
            this.NumericUpDownPlane2Min.TabIndex = 7;
            this.NumericUpDownPlane2Min.ValueChanged += new System.EventHandler(this.NumericUpDownPlane2Min_ValueChanged);
            // 
            // NumericUpDownPlane2Max
            // 
            this.NumericUpDownPlane2Max.Location = new System.Drawing.Point(177, 32);
            this.NumericUpDownPlane2Max.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NumericUpDownPlane2Max.Name = "NumericUpDownPlane2Max";
            this.NumericUpDownPlane2Max.Size = new System.Drawing.Size(63, 20);
            this.NumericUpDownPlane2Max.TabIndex = 6;
            this.NumericUpDownPlane2Max.ValueChanged += new System.EventHandler(this.NumericUpDownPlane2Max_ValueChanged);
            // 
            // checkBoxPlane0Invert
            // 
            this.checkBoxPlane0Invert.AutoSize = true;
            this.checkBoxPlane0Invert.Location = new System.Drawing.Point(39, 108);
            this.checkBoxPlane0Invert.Name = "checkBoxPlane0Invert";
            this.checkBoxPlane0Invert.Size = new System.Drawing.Size(53, 17);
            this.checkBoxPlane0Invert.TabIndex = 98;
            this.checkBoxPlane0Invert.Text = "Invert";
            this.checkBoxPlane0Invert.UseVisualStyleBackColor = true;
            this.checkBoxPlane0Invert.CheckedChanged += new System.EventHandler(this.checkBoxPlane0Invert_CheckedChanged);
            // 
            // checkBoxPlane1Invert
            // 
            this.checkBoxPlane1Invert.AutoSize = true;
            this.checkBoxPlane1Invert.Location = new System.Drawing.Point(108, 108);
            this.checkBoxPlane1Invert.Name = "checkBoxPlane1Invert";
            this.checkBoxPlane1Invert.Size = new System.Drawing.Size(53, 17);
            this.checkBoxPlane1Invert.TabIndex = 99;
            this.checkBoxPlane1Invert.Text = "Invert";
            this.checkBoxPlane1Invert.UseVisualStyleBackColor = true;
            this.checkBoxPlane1Invert.CheckedChanged += new System.EventHandler(this.checkBoxPlane1Invert_CheckedChanged);
            // 
            // checkBoxPlane2Invert
            // 
            this.checkBoxPlane2Invert.AutoSize = true;
            this.checkBoxPlane2Invert.Location = new System.Drawing.Point(177, 108);
            this.checkBoxPlane2Invert.Name = "checkBoxPlane2Invert";
            this.checkBoxPlane2Invert.Size = new System.Drawing.Size(53, 17);
            this.checkBoxPlane2Invert.TabIndex = 100;
            this.checkBoxPlane2Invert.Text = "Invert";
            this.checkBoxPlane2Invert.UseVisualStyleBackColor = true;
            this.checkBoxPlane2Invert.CheckedChanged += new System.EventHandler(this.checkBoxPlane2Invert_CheckedChanged);
            // 
            // CvbImageBinarizationUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxColorSettings);
            this.Name = "CvbImageBinarizationUserControl";
            this.Size = new System.Drawing.Size(281, 133);
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownPlane0Min)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownPlane1Max)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownPlane1Min)).EndInit();
            this.groupBoxColorSettings.ResumeLayout(false);
            this.groupBoxColorSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownPlane0Max)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownPlane2Min)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownPlane2Max)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMaxColor;
        private System.Windows.Forms.NumericUpDown NumericUpDownPlane0Max;
        private System.Windows.Forms.NumericUpDown NumericUpDownPlane0Min;
        private System.Windows.Forms.NumericUpDown NumericUpDownPlane1Max;
        private System.Windows.Forms.NumericUpDown NumericUpDownPlane1Min;
        private System.Windows.Forms.NumericUpDown NumericUpDownPlane2Max;
        private System.Windows.Forms.NumericUpDown NumericUpDownPlane2Min;
        private System.Windows.Forms.Label lblPlane2;
        private System.Windows.Forms.Label lblPlane1;
        private System.Windows.Forms.GroupBox groupBoxColorSettings;
        private System.Windows.Forms.Label lblPlane0;
        private System.Windows.Forms.Panel panelMinColor;
        private System.Windows.Forms.Label lblColorMin;
        private System.Windows.Forms.Label lblColorMax;
        private System.Windows.Forms.CheckBox CheckBoxPlane2Used;
        private System.Windows.Forms.CheckBox CheckBoxPlane1Used;
        private System.Windows.Forms.CheckBox CheckBoxPlane0Used;
        private System.Windows.Forms.CheckBox checkBoxPlane2Invert;
        private System.Windows.Forms.CheckBox checkBoxPlane1Invert;
        private System.Windows.Forms.CheckBox checkBoxPlane0Invert;
    }
}