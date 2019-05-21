namespace ImageProcessing.ImageMorphology
{
    partial class EmguImageMorphologyUserControl
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
            this.groupBoxMorphology = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelMorphology = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxSmoothGaussian = new System.Windows.Forms.GroupBox();
            this.numericUpDownSigma2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownSigma1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownKernelHeight = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownKernelWidth = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxSmoothingEnabled = new System.Windows.Forms.CheckBox();
            this.panelMorphologicOperation = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxBorderType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownMorphIterations = new System.Windows.Forms.NumericUpDown();
            this.checkBoxMorphologyEnabled = new System.Windows.Forms.CheckBox();
            this.ComboBoxMorphologicOperation = new System.Windows.Forms.ComboBox();
            this.labelMorphologicOperation = new System.Windows.Forms.Label();
            this.checkBoxNoiseReduction = new System.Windows.Forms.CheckBox();
            this.comboBoxMorphologyMaskType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.numericUpDownMorphMaskWidth = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.numericUpDownMorphMaskHeight = new System.Windows.Forms.NumericUpDown();
            this.groupBoxMorphology.SuspendLayout();
            this.tableLayoutPanelMorphology.SuspendLayout();
            this.groupBoxSmoothGaussian.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSigma2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSigma1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownKernelHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownKernelWidth)).BeginInit();
            this.panelMorphologicOperation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMorphIterations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMorphMaskWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMorphMaskHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxMorphology
            // 
            this.groupBoxMorphology.Controls.Add(this.tableLayoutPanelMorphology);
            this.groupBoxMorphology.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxMorphology.Location = new System.Drawing.Point(0, 0);
            this.groupBoxMorphology.Name = "groupBoxMorphology";
            this.groupBoxMorphology.Size = new System.Drawing.Size(312, 282);
            this.groupBoxMorphology.TabIndex = 102;
            this.groupBoxMorphology.TabStop = false;
            this.groupBoxMorphology.Text = "Morphology Settings";
            // 
            // tableLayoutPanelMorphology
            // 
            this.tableLayoutPanelMorphology.ColumnCount = 1;
            this.tableLayoutPanelMorphology.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMorphology.Controls.Add(this.groupBoxSmoothGaussian, 0, 1);
            this.tableLayoutPanelMorphology.Controls.Add(this.panelMorphologicOperation, 0, 0);
            this.tableLayoutPanelMorphology.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMorphology.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelMorphology.Name = "tableLayoutPanelMorphology";
            this.tableLayoutPanelMorphology.RowCount = 2;
            this.tableLayoutPanelMorphology.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanelMorphology.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMorphology.Size = new System.Drawing.Size(306, 263);
            this.tableLayoutPanelMorphology.TabIndex = 103;
            // 
            // groupBoxSmoothGaussian
            // 
            this.groupBoxSmoothGaussian.Controls.Add(this.numericUpDownSigma2);
            this.groupBoxSmoothGaussian.Controls.Add(this.numericUpDownSigma1);
            this.groupBoxSmoothGaussian.Controls.Add(this.numericUpDownKernelHeight);
            this.groupBoxSmoothGaussian.Controls.Add(this.numericUpDownKernelWidth);
            this.groupBoxSmoothGaussian.Controls.Add(this.label6);
            this.groupBoxSmoothGaussian.Controls.Add(this.label5);
            this.groupBoxSmoothGaussian.Controls.Add(this.label4);
            this.groupBoxSmoothGaussian.Controls.Add(this.label3);
            this.groupBoxSmoothGaussian.Controls.Add(this.checkBoxSmoothingEnabled);
            this.groupBoxSmoothGaussian.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxSmoothGaussian.Location = new System.Drawing.Point(3, 173);
            this.groupBoxSmoothGaussian.Name = "groupBoxSmoothGaussian";
            this.groupBoxSmoothGaussian.Size = new System.Drawing.Size(300, 87);
            this.groupBoxSmoothGaussian.TabIndex = 104;
            this.groupBoxSmoothGaussian.TabStop = false;
            this.groupBoxSmoothGaussian.Text = "SmoothGaussian";
            // 
            // numericUpDownSigma2
            // 
            this.numericUpDownSigma2.Location = new System.Drawing.Point(229, 63);
            this.numericUpDownSigma2.Name = "numericUpDownSigma2";
            this.numericUpDownSigma2.Size = new System.Drawing.Size(66, 20);
            this.numericUpDownSigma2.TabIndex = 8;
            this.numericUpDownSigma2.ValueChanged += new System.EventHandler(this.numericUpDownSigma2_ValueChanged);
            // 
            // numericUpDownSigma1
            // 
            this.numericUpDownSigma1.Location = new System.Drawing.Point(75, 64);
            this.numericUpDownSigma1.Name = "numericUpDownSigma1";
            this.numericUpDownSigma1.Size = new System.Drawing.Size(70, 20);
            this.numericUpDownSigma1.TabIndex = 7;
            this.numericUpDownSigma1.ValueChanged += new System.EventHandler(this.numericUpDownSigma1_ValueChanged);
            // 
            // numericUpDownKernelHeight
            // 
            this.numericUpDownKernelHeight.Location = new System.Drawing.Point(229, 37);
            this.numericUpDownKernelHeight.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownKernelHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownKernelHeight.Name = "numericUpDownKernelHeight";
            this.numericUpDownKernelHeight.Size = new System.Drawing.Size(64, 20);
            this.numericUpDownKernelHeight.TabIndex = 6;
            this.numericUpDownKernelHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownKernelHeight.ValueChanged += new System.EventHandler(this.numericUpDownKernelHeight_ValueChanged);
            // 
            // numericUpDownKernelWidth
            // 
            this.numericUpDownKernelWidth.Location = new System.Drawing.Point(75, 37);
            this.numericUpDownKernelWidth.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownKernelWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownKernelWidth.Name = "numericUpDownKernelWidth";
            this.numericUpDownKernelWidth.Size = new System.Drawing.Size(70, 20);
            this.numericUpDownKernelWidth.TabIndex = 5;
            this.numericUpDownKernelWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownKernelWidth.ValueChanged += new System.EventHandler(this.numericUpDownKernelWidth_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(151, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Sigma 2:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(2, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Sigma 1:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(151, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Kernel height:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Kernerl width:";
            // 
            // checkBoxSmoothingEnabled
            // 
            this.checkBoxSmoothingEnabled.AutoSize = true;
            this.checkBoxSmoothingEnabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxSmoothingEnabled.Location = new System.Drawing.Point(3, 16);
            this.checkBoxSmoothingEnabled.Name = "checkBoxSmoothingEnabled";
            this.checkBoxSmoothingEnabled.Size = new System.Drawing.Size(68, 17);
            this.checkBoxSmoothingEnabled.TabIndex = 0;
            this.checkBoxSmoothingEnabled.Text = "Enabled:";
            this.checkBoxSmoothingEnabled.UseVisualStyleBackColor = true;
            this.checkBoxSmoothingEnabled.CheckedChanged += new System.EventHandler(this.checkBoxSmoothingEnabled_CheckedChanged);
            // 
            // panelMorphologicOperation
            // 
            this.panelMorphologicOperation.Controls.Add(this.numericUpDownMorphMaskHeight);
            this.panelMorphologicOperation.Controls.Add(this.label9);
            this.panelMorphologicOperation.Controls.Add(this.label8);
            this.panelMorphologicOperation.Controls.Add(this.numericUpDownMorphMaskWidth);
            this.panelMorphologicOperation.Controls.Add(this.label7);
            this.panelMorphologicOperation.Controls.Add(this.comboBoxMorphologyMaskType);
            this.panelMorphologicOperation.Controls.Add(this.checkBoxNoiseReduction);
            this.panelMorphologicOperation.Controls.Add(this.label2);
            this.panelMorphologicOperation.Controls.Add(this.comboBoxBorderType);
            this.panelMorphologicOperation.Controls.Add(this.label1);
            this.panelMorphologicOperation.Controls.Add(this.numericUpDownMorphIterations);
            this.panelMorphologicOperation.Controls.Add(this.checkBoxMorphologyEnabled);
            this.panelMorphologicOperation.Controls.Add(this.ComboBoxMorphologicOperation);
            this.panelMorphologicOperation.Controls.Add(this.labelMorphologicOperation);
            this.panelMorphologicOperation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMorphologicOperation.Location = new System.Drawing.Point(3, 3);
            this.panelMorphologicOperation.Name = "panelMorphologicOperation";
            this.panelMorphologicOperation.Size = new System.Drawing.Size(300, 164);
            this.panelMorphologicOperation.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 107;
            this.label2.Text = "BorderType:";
            // 
            // comboBoxBorderType
            // 
            this.comboBoxBorderType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBorderType.FormattingEnabled = true;
            this.comboBoxBorderType.Location = new System.Drawing.Point(104, 85);
            this.comboBoxBorderType.Name = "comboBoxBorderType";
            this.comboBoxBorderType.Size = new System.Drawing.Size(189, 21);
            this.comboBoxBorderType.TabIndex = 106;
            this.comboBoxBorderType.SelectedIndexChanged += new System.EventHandler(this.comboBoxBorderType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(116, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 105;
            this.label1.Text = "Iterations:";
            // 
            // numericUpDownMorphIterations
            // 
            this.numericUpDownMorphIterations.Location = new System.Drawing.Point(175, 5);
            this.numericUpDownMorphIterations.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownMorphIterations.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMorphIterations.Name = "numericUpDownMorphIterations";
            this.numericUpDownMorphIterations.Size = new System.Drawing.Size(62, 20);
            this.numericUpDownMorphIterations.TabIndex = 0;
            this.numericUpDownMorphIterations.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMorphIterations.ValueChanged += new System.EventHandler(this.numericUpDownMorphIterations_ValueChanged);
            // 
            // checkBoxMorphologyEnabled
            // 
            this.checkBoxMorphologyEnabled.AutoSize = true;
            this.checkBoxMorphologyEnabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxMorphologyEnabled.Location = new System.Drawing.Point(3, 5);
            this.checkBoxMorphologyEnabled.Name = "checkBoxMorphologyEnabled";
            this.checkBoxMorphologyEnabled.Size = new System.Drawing.Size(68, 17);
            this.checkBoxMorphologyEnabled.TabIndex = 104;
            this.checkBoxMorphologyEnabled.Text = "Enabled:";
            this.checkBoxMorphologyEnabled.UseVisualStyleBackColor = true;
            this.checkBoxMorphologyEnabled.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // ComboBoxMorphologicOperation
            // 
            this.ComboBoxMorphologicOperation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxMorphologicOperation.FormattingEnabled = true;
            this.ComboBoxMorphologicOperation.Location = new System.Drawing.Point(103, 58);
            this.ComboBoxMorphologicOperation.Name = "ComboBoxMorphologicOperation";
            this.ComboBoxMorphologicOperation.Size = new System.Drawing.Size(190, 21);
            this.ComboBoxMorphologicOperation.TabIndex = 0;
            this.ComboBoxMorphologicOperation.SelectedIndexChanged += new System.EventHandler(this.ComboBoxMorphologicOperation_SelectedIndexChanged);
            // 
            // labelMorphologicOperation
            // 
            this.labelMorphologicOperation.AutoSize = true;
            this.labelMorphologicOperation.Location = new System.Drawing.Point(2, 61);
            this.labelMorphologicOperation.Name = "labelMorphologicOperation";
            this.labelMorphologicOperation.Size = new System.Drawing.Size(95, 13);
            this.labelMorphologicOperation.TabIndex = 103;
            this.labelMorphologicOperation.Text = "Perform Operation:";
            // 
            // checkBoxNoiseReduction
            // 
            this.checkBoxNoiseReduction.AutoSize = true;
            this.checkBoxNoiseReduction.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxNoiseReduction.Location = new System.Drawing.Point(3, 30);
            this.checkBoxNoiseReduction.Name = "checkBoxNoiseReduction";
            this.checkBoxNoiseReduction.Size = new System.Drawing.Size(103, 17);
            this.checkBoxNoiseReduction.TabIndex = 108;
            this.checkBoxNoiseReduction.Text = "Noise reduction:";
            this.checkBoxNoiseReduction.UseVisualStyleBackColor = true;
            this.checkBoxNoiseReduction.CheckedChanged += new System.EventHandler(this.checkBoxNoiseReduction_CheckedChanged);
            // 
            // comboBoxMorphologyMaskType
            // 
            this.comboBoxMorphologyMaskType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMorphologyMaskType.FormattingEnabled = true;
            this.comboBoxMorphologyMaskType.Location = new System.Drawing.Point(104, 112);
            this.comboBoxMorphologyMaskType.Name = "comboBoxMorphologyMaskType";
            this.comboBoxMorphologyMaskType.Size = new System.Drawing.Size(189, 21);
            this.comboBoxMorphologyMaskType.TabIndex = 109;
            this.comboBoxMorphologyMaskType.SelectedIndexChanged += new System.EventHandler(this.comboBoxMorphologyMaskType_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 115);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 13);
            this.label7.TabIndex = 110;
            this.label7.Text = "Morph mask type:";
            // 
            // numericUpDownMorphMaskWidth
            // 
            this.numericUpDownMorphMaskWidth.Location = new System.Drawing.Point(78, 140);
            this.numericUpDownMorphMaskWidth.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownMorphMaskWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMorphMaskWidth.Name = "numericUpDownMorphMaskWidth";
            this.numericUpDownMorphMaskWidth.Size = new System.Drawing.Size(70, 20);
            this.numericUpDownMorphMaskWidth.TabIndex = 111;
            this.numericUpDownMorphMaskWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMorphMaskWidth.ValueChanged += new System.EventHandler(this.numericUpDownMorphMaskWidth_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 142);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 13);
            this.label8.TabIndex = 112;
            this.label8.Text = "Mask width:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(151, 142);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 13);
            this.label9.TabIndex = 113;
            this.label9.Text = "Mask height:";
            // 
            // numericUpDownMorphMaskHeight
            // 
            this.numericUpDownMorphMaskHeight.Location = new System.Drawing.Point(229, 140);
            this.numericUpDownMorphMaskHeight.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownMorphMaskHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMorphMaskHeight.Name = "numericUpDownMorphMaskHeight";
            this.numericUpDownMorphMaskHeight.Size = new System.Drawing.Size(64, 20);
            this.numericUpDownMorphMaskHeight.TabIndex = 114;
            this.numericUpDownMorphMaskHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMorphMaskHeight.ValueChanged += new System.EventHandler(this.numericUpDownMorphMaskHeight_ValueChanged);
            // 
            // EmguImageMorphologyUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxMorphology);
            this.Name = "EmguImageMorphologyUserControl";
            this.Size = new System.Drawing.Size(312, 282);
            this.groupBoxMorphology.ResumeLayout(false);
            this.tableLayoutPanelMorphology.ResumeLayout(false);
            this.groupBoxSmoothGaussian.ResumeLayout(false);
            this.groupBoxSmoothGaussian.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSigma2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSigma1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownKernelHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownKernelWidth)).EndInit();
            this.panelMorphologicOperation.ResumeLayout(false);
            this.panelMorphologicOperation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMorphIterations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMorphMaskWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMorphMaskHeight)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBoxMorphology;
        private System.Windows.Forms.Label labelMorphologicOperation;
        private System.Windows.Forms.ComboBox ComboBoxMorphologicOperation;
        private System.Windows.Forms.GroupBox groupBoxSmoothGaussian;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMorphology;
        private System.Windows.Forms.Panel panelMorphologicOperation;
        private System.Windows.Forms.CheckBox checkBoxMorphologyEnabled;
        private System.Windows.Forms.NumericUpDown numericUpDownMorphIterations;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxBorderType;
        private System.Windows.Forms.CheckBox checkBoxSmoothingEnabled;
        private System.Windows.Forms.NumericUpDown numericUpDownSigma2;
        private System.Windows.Forms.NumericUpDown numericUpDownSigma1;
        private System.Windows.Forms.NumericUpDown numericUpDownKernelHeight;
        private System.Windows.Forms.NumericUpDown numericUpDownKernelWidth;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBoxNoiseReduction;
        private System.Windows.Forms.NumericUpDown numericUpDownMorphMaskHeight;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericUpDownMorphMaskWidth;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxMorphologyMaskType;
    }
}