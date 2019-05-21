namespace ObjectSearch.BlobSearch
{
    /// <summary>
    /// User control zur Einstellung der Parameter für die Blob-Erkennung
    /// </summary>
    partial class BlobSearchUserControl
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
            this.groupBoxBlobSettings = new System.Windows.Forms.GroupBox();
            this.numericUpDownAOIHeight = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownAOIWidth = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownObjectCount = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.labelMinimumNumberOfObjects = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblBlobFiltersMax = new System.Windows.Forms.Label();
            this.lblBlobFiltersMin = new System.Windows.Forms.Label();
            this.lblDistanceToSearchNextBlob = new System.Windows.Forms.Label();
            this.NumericUpDownMinBlobArea = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDownDistanceToSearchNextBlob = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDownMaxBlobArea = new System.Windows.Forms.NumericUpDown();
            this.lblBlobSize = new System.Windows.Forms.Label();
            this.groupBoxBlobSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAOIHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAOIWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownObjectCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownMinBlobArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownDistanceToSearchNextBlob)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownMaxBlobArea)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxBlobSettings
            // 
            this.groupBoxBlobSettings.Controls.Add(this.numericUpDownAOIHeight);
            this.groupBoxBlobSettings.Controls.Add(this.label5);
            this.groupBoxBlobSettings.Controls.Add(this.label4);
            this.groupBoxBlobSettings.Controls.Add(this.numericUpDownAOIWidth);
            this.groupBoxBlobSettings.Controls.Add(this.label2);
            this.groupBoxBlobSettings.Controls.Add(this.numericUpDownObjectCount);
            this.groupBoxBlobSettings.Controls.Add(this.label3);
            this.groupBoxBlobSettings.Controls.Add(this.labelMinimumNumberOfObjects);
            this.groupBoxBlobSettings.Controls.Add(this.label1);
            this.groupBoxBlobSettings.Controls.Add(this.lblBlobFiltersMax);
            this.groupBoxBlobSettings.Controls.Add(this.lblBlobFiltersMin);
            this.groupBoxBlobSettings.Controls.Add(this.lblDistanceToSearchNextBlob);
            this.groupBoxBlobSettings.Controls.Add(this.NumericUpDownMinBlobArea);
            this.groupBoxBlobSettings.Controls.Add(this.NumericUpDownDistanceToSearchNextBlob);
            this.groupBoxBlobSettings.Controls.Add(this.NumericUpDownMaxBlobArea);
            this.groupBoxBlobSettings.Controls.Add(this.lblBlobSize);
            this.groupBoxBlobSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxBlobSettings.Location = new System.Drawing.Point(0, 0);
            this.groupBoxBlobSettings.Name = "groupBoxBlobSettings";
            this.groupBoxBlobSettings.Size = new System.Drawing.Size(431, 181);
            this.groupBoxBlobSettings.TabIndex = 100;
            this.groupBoxBlobSettings.TabStop = false;
            this.groupBoxBlobSettings.Text = "Blob Settings";
            // 
            // numericUpDownAOIHeight
            // 
            this.numericUpDownAOIHeight.Location = new System.Drawing.Point(205, 155);
            this.numericUpDownAOIHeight.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownAOIHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownAOIHeight.Name = "numericUpDownAOIHeight";
            this.numericUpDownAOIHeight.Size = new System.Drawing.Size(55, 20);
            this.numericUpDownAOIHeight.TabIndex = 92;
            this.numericUpDownAOIHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownAOIHeight.ValueChanged += new System.EventHandler(this.numericUpDownAOIHeight_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(157, 157);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 91;
            this.label5.Text = "Height:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 157);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 90;
            this.label4.Text = "Width :";
            // 
            // numericUpDownAOIWidth
            // 
            this.numericUpDownAOIWidth.Location = new System.Drawing.Point(64, 155);
            this.numericUpDownAOIWidth.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownAOIWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownAOIWidth.Name = "numericUpDownAOIWidth";
            this.numericUpDownAOIWidth.Size = new System.Drawing.Size(63, 20);
            this.numericUpDownAOIWidth.TabIndex = 89;
            this.numericUpDownAOIWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownAOIWidth.ValueChanged += new System.EventHandler(this.numericUpDownAOIWidth_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 139);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 88;
            this.label2.Text = "Area of interest:";
            // 
            // numericUpDownObjectCount
            // 
            this.numericUpDownObjectCount.Location = new System.Drawing.Point(83, 113);
            this.numericUpDownObjectCount.Name = "numericUpDownObjectCount";
            this.numericUpDownObjectCount.Size = new System.Drawing.Size(63, 20);
            this.numericUpDownObjectCount.TabIndex = 87;
            this.numericUpDownObjectCount.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 86;
            this.label3.Text = "Object count";
            // 
            // labelMinimumNumberOfObjects
            // 
            this.labelMinimumNumberOfObjects.AutoSize = true;
            this.labelMinimumNumberOfObjects.Location = new System.Drawing.Point(6, 96);
            this.labelMinimumNumberOfObjects.Name = "labelMinimumNumberOfObjects";
            this.labelMinimumNumberOfObjects.Size = new System.Drawing.Size(254, 13);
            this.labelMinimumNumberOfObjects.TabIndex = 85;
            this.labelMinimumNumberOfObjects.Text = "Minimum number of objects to find in area of interest:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(154, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 84;
            this.label1.Text = "Distance in pixel";
            // 
            // lblBlobFiltersMax
            // 
            this.lblBlobFiltersMax.AutoSize = true;
            this.lblBlobFiltersMax.Location = new System.Drawing.Point(6, 34);
            this.lblBlobFiltersMax.Name = "lblBlobFiltersMax";
            this.lblBlobFiltersMax.Size = new System.Drawing.Size(52, 13);
            this.lblBlobFiltersMax.TabIndex = 73;
            this.lblBlobFiltersMax.Text = "Max Area";
            // 
            // lblBlobFiltersMin
            // 
            this.lblBlobFiltersMin.AutoSize = true;
            this.lblBlobFiltersMin.Location = new System.Drawing.Point(6, 60);
            this.lblBlobFiltersMin.Name = "lblBlobFiltersMin";
            this.lblBlobFiltersMin.Size = new System.Drawing.Size(49, 13);
            this.lblBlobFiltersMin.TabIndex = 75;
            this.lblBlobFiltersMin.Text = "Min Area";
            // 
            // lblDistanceToSearchNextBlob
            // 
            this.lblDistanceToSearchNextBlob.AutoSize = true;
            this.lblDistanceToSearchNextBlob.Location = new System.Drawing.Point(154, 18);
            this.lblDistanceToSearchNextBlob.Name = "lblDistanceToSearchNextBlob";
            this.lblDistanceToSearchNextBlob.Size = new System.Drawing.Size(205, 13);
            this.lblDistanceToSearchNextBlob.TabIndex = 83;
            this.lblDistanceToSearchNextBlob.Text = "Filter by minimal disatance between Blobs:";
            // 
            // NumericUpDownMinBlobArea
            // 
            this.NumericUpDownMinBlobArea.Location = new System.Drawing.Point(64, 57);
            this.NumericUpDownMinBlobArea.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.NumericUpDownMinBlobArea.Name = "NumericUpDownMinBlobArea";
            this.NumericUpDownMinBlobArea.Size = new System.Drawing.Size(63, 20);
            this.NumericUpDownMinBlobArea.TabIndex = 1;
            this.NumericUpDownMinBlobArea.ValueChanged += new System.EventHandler(this.NumericUpDownMinBlobSize_ValueChanged);
            // 
            // NumericUpDownDistanceToSearchNextBlob
            // 
            this.NumericUpDownDistanceToSearchNextBlob.Location = new System.Drawing.Point(244, 32);
            this.NumericUpDownDistanceToSearchNextBlob.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.NumericUpDownDistanceToSearchNextBlob.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.NumericUpDownDistanceToSearchNextBlob.Name = "NumericUpDownDistanceToSearchNextBlob";
            this.NumericUpDownDistanceToSearchNextBlob.Size = new System.Drawing.Size(63, 20);
            this.NumericUpDownDistanceToSearchNextBlob.TabIndex = 9;
            this.NumericUpDownDistanceToSearchNextBlob.ValueChanged += new System.EventHandler(this.NumericUpDownDistanceToSearchNextBlob_ValueChanged);
            // 
            // NumericUpDownMaxBlobArea
            // 
            this.NumericUpDownMaxBlobArea.Location = new System.Drawing.Point(64, 32);
            this.NumericUpDownMaxBlobArea.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.NumericUpDownMaxBlobArea.Name = "NumericUpDownMaxBlobArea";
            this.NumericUpDownMaxBlobArea.Size = new System.Drawing.Size(63, 20);
            this.NumericUpDownMaxBlobArea.TabIndex = 0;
            this.NumericUpDownMaxBlobArea.ValueChanged += new System.EventHandler(this.NumericUpDownMaxBlobSize_ValueChanged);
            // 
            // lblBlobSize
            // 
            this.lblBlobSize.AutoSize = true;
            this.lblBlobSize.Location = new System.Drawing.Point(6, 16);
            this.lblBlobSize.Name = "lblBlobSize";
            this.lblBlobSize.Size = new System.Drawing.Size(70, 13);
            this.lblBlobSize.TabIndex = 77;
            this.lblBlobSize.Text = "Filter by area:";
            // 
            // BlobSearchUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxBlobSettings);
            this.Name = "BlobSearchUserControl";
            this.Size = new System.Drawing.Size(431, 181);
            this.groupBoxBlobSettings.ResumeLayout(false);
            this.groupBoxBlobSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAOIHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAOIWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownObjectCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownMinBlobArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownDistanceToSearchNextBlob)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownMaxBlobArea)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxBlobSettings;
        private System.Windows.Forms.Label lblBlobFiltersMax;
        private System.Windows.Forms.Label lblBlobFiltersMin;
        private System.Windows.Forms.Label lblDistanceToSearchNextBlob;
        private System.Windows.Forms.NumericUpDown NumericUpDownMinBlobArea;
        private System.Windows.Forms.NumericUpDown NumericUpDownDistanceToSearchNextBlob;
        private System.Windows.Forms.NumericUpDown NumericUpDownMaxBlobArea;
        private System.Windows.Forms.Label lblBlobSize;
        private System.Windows.Forms.NumericUpDown numericUpDownObjectCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelMinimumNumberOfObjects;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownAOIHeight;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownAOIWidth;
        private System.Windows.Forms.Label label2;
    }
}