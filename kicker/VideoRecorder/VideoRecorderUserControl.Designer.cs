namespace VideoRecorder
{
    partial class VideoRecorderUserControl
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label labelRecFramesPerSecond;
            System.Windows.Forms.Label labelFramesMinMax;
            System.Windows.Forms.Label labelVideoFile;
            this.numericUpDownRecFramesPerSecond = new System.Windows.Forms.NumericUpDown();
            this.buttonInit = new System.Windows.Forms.Button();
            this.buttonRecord = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.groupBoxVideoRecorderSettings = new System.Windows.Forms.GroupBox();
            this.numericUpDownHeight = new System.Windows.Forms.NumericUpDown();
            this.labelHeight = new System.Windows.Forms.Label();
            this.numericUpDownWidth = new System.Windows.Forms.NumericUpDown();
            this.labelWidth = new System.Windows.Forms.Label();
            this.labelFrameSize = new System.Windows.Forms.Label();
            this.controlToolTips = new System.Windows.Forms.ToolTip(this.components);
            this.labelFileName = new System.Windows.Forms.Label();
            this.labelRecordingTime = new System.Windows.Forms.Label();
            labelRecFramesPerSecond = new System.Windows.Forms.Label();
            labelFramesMinMax = new System.Windows.Forms.Label();
            labelVideoFile = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRecFramesPerSecond)).BeginInit();
            this.groupBoxVideoRecorderSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // labelRecFramesPerSecond
            // 
            labelRecFramesPerSecond.AutoSize = true;
            labelRecFramesPerSecond.Location = new System.Drawing.Point(6, 16);
            labelRecFramesPerSecond.Name = "labelRecFramesPerSecond";
            labelRecFramesPerSecond.Size = new System.Drawing.Size(54, 13);
            labelRecFramesPerSecond.TabIndex = 25;
            labelRecFramesPerSecond.Text = "Frames/s:";
            // 
            // labelFramesMinMax
            // 
            labelFramesMinMax.AutoSize = true;
            labelFramesMinMax.Location = new System.Drawing.Point(145, 16);
            labelFramesMinMax.Name = "labelFramesMinMax";
            labelFramesMinMax.Size = new System.Drawing.Size(46, 13);
            labelFramesMinMax.TabIndex = 30;
            labelFramesMinMax.Text = "(1 - 100)";
            // 
            // labelVideoFile
            // 
            labelVideoFile.AutoSize = true;
            labelVideoFile.Location = new System.Drawing.Point(3, 171);
            labelVideoFile.Name = "labelVideoFile";
            labelVideoFile.Size = new System.Drawing.Size(56, 13);
            labelVideoFile.TabIndex = 31;
            labelVideoFile.Text = "Video File:";
            // 
            // numericUpDownRecFramesPerSecond
            // 
            this.numericUpDownRecFramesPerSecond.Location = new System.Drawing.Point(66, 14);
            this.numericUpDownRecFramesPerSecond.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownRecFramesPerSecond.Name = "numericUpDownRecFramesPerSecond";
            this.numericUpDownRecFramesPerSecond.Size = new System.Drawing.Size(64, 20);
            this.numericUpDownRecFramesPerSecond.TabIndex = 6;
            this.numericUpDownRecFramesPerSecond.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownRecFramesPerSecond.ValueChanged += new System.EventHandler(this.NumericUpDownRecFramesPerSecond_ValueChanged);
            // 
            // buttonInit
            // 
            this.buttonInit.Enabled = false;
            this.buttonInit.Location = new System.Drawing.Point(6, 139);
            this.buttonInit.Name = "buttonInit";
            this.buttonInit.Size = new System.Drawing.Size(75, 23);
            this.buttonInit.TabIndex = 1;
            this.buttonInit.Text = "Init...";
            this.buttonInit.UseVisualStyleBackColor = true;
            this.buttonInit.Click += new System.EventHandler(this.ButtonInit_Click);
            // 
            // buttonRecord
            // 
            this.buttonRecord.Enabled = false;
            this.buttonRecord.Location = new System.Drawing.Point(87, 139);
            this.buttonRecord.Name = "buttonRecord";
            this.buttonRecord.Size = new System.Drawing.Size(75, 23);
            this.buttonRecord.TabIndex = 2;
            this.buttonRecord.Text = "Record";
            this.buttonRecord.UseVisualStyleBackColor = true;
            this.buttonRecord.Click += new System.EventHandler(this.ButtonRecord_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Enabled = false;
            this.buttonStop.Location = new System.Drawing.Point(168, 139);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 3;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.ButtonStop_Click);
            // 
            // groupBoxVideoRecorderSettings
            // 
            this.groupBoxVideoRecorderSettings.Controls.Add(this.numericUpDownHeight);
            this.groupBoxVideoRecorderSettings.Controls.Add(this.labelHeight);
            this.groupBoxVideoRecorderSettings.Controls.Add(this.numericUpDownWidth);
            this.groupBoxVideoRecorderSettings.Controls.Add(this.labelWidth);
            this.groupBoxVideoRecorderSettings.Controls.Add(this.labelFrameSize);
            this.groupBoxVideoRecorderSettings.Controls.Add(labelFramesMinMax);
            this.groupBoxVideoRecorderSettings.Controls.Add(this.numericUpDownRecFramesPerSecond);
            this.groupBoxVideoRecorderSettings.Controls.Add(labelRecFramesPerSecond);
            this.groupBoxVideoRecorderSettings.Location = new System.Drawing.Point(8, 8);
            this.groupBoxVideoRecorderSettings.Name = "groupBoxVideoRecorderSettings";
            this.groupBoxVideoRecorderSettings.Size = new System.Drawing.Size(368, 96);
            this.groupBoxVideoRecorderSettings.TabIndex = 0;
            this.groupBoxVideoRecorderSettings.TabStop = false;
            this.groupBoxVideoRecorderSettings.Text = "Settings";
            // 
            // numericUpDownHeight
            // 
            this.numericUpDownHeight.Location = new System.Drawing.Point(195, 57);
            this.numericUpDownHeight.Maximum = new decimal(new int[] {
            1920,
            0,
            0,
            0});
            this.numericUpDownHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownHeight.Name = "numericUpDownHeight";
            this.numericUpDownHeight.Size = new System.Drawing.Size(84, 20);
            this.numericUpDownHeight.TabIndex = 35;
            this.numericUpDownHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownHeight.ValueChanged += new System.EventHandler(this.numericUpDownHeight_ValueChanged);
            // 
            // labelHeight
            // 
            this.labelHeight.AutoSize = true;
            this.labelHeight.Location = new System.Drawing.Point(148, 58);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(41, 13);
            this.labelHeight.TabIndex = 34;
            this.labelHeight.Text = "Height:";
            // 
            // numericUpDownWidth
            // 
            this.numericUpDownWidth.Location = new System.Drawing.Point(50, 57);
            this.numericUpDownWidth.Maximum = new decimal(new int[] {
            1080,
            0,
            0,
            0});
            this.numericUpDownWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownWidth.Name = "numericUpDownWidth";
            this.numericUpDownWidth.Size = new System.Drawing.Size(80, 20);
            this.numericUpDownWidth.TabIndex = 33;
            this.numericUpDownWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownWidth.ValueChanged += new System.EventHandler(this.numericUpDownWidth_ValueChanged);
            // 
            // labelWidth
            // 
            this.labelWidth.AutoSize = true;
            this.labelWidth.Location = new System.Drawing.Point(6, 59);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(38, 13);
            this.labelWidth.TabIndex = 32;
            this.labelWidth.Text = "Width:";
            // 
            // labelFrameSize
            // 
            this.labelFrameSize.AutoSize = true;
            this.labelFrameSize.Location = new System.Drawing.Point(6, 37);
            this.labelFrameSize.Name = "labelFrameSize";
            this.labelFrameSize.Size = new System.Drawing.Size(60, 13);
            this.labelFrameSize.TabIndex = 31;
            this.labelFrameSize.Text = "Frame size:";
            // 
            // labelFileName
            // 
            this.labelFileName.AutoEllipsis = true;
            this.labelFileName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelFileName.Location = new System.Drawing.Point(0, 197);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(368, 16);
            this.labelFileName.TabIndex = 30;
            // 
            // labelRecordingTime
            // 
            this.labelRecordingTime.AutoSize = true;
            this.labelRecordingTime.Location = new System.Drawing.Point(8, 111);
            this.labelRecordingTime.Name = "labelRecordingTime";
            this.labelRecordingTime.Size = new System.Drawing.Size(81, 13);
            this.labelRecordingTime.TabIndex = 32;
            this.labelRecordingTime.Text = "Recording time:";
            // 
            // VideoRecorderUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelRecordingTime);
            this.Controls.Add(labelVideoFile);
            this.Controls.Add(this.labelFileName);
            this.Controls.Add(this.groupBoxVideoRecorderSettings);
            this.Controls.Add(this.buttonInit);
            this.Controls.Add(this.buttonRecord);
            this.Controls.Add(this.buttonStop);
            this.Name = "VideoRecorderUserControl";
            this.Size = new System.Drawing.Size(387, 217);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRecFramesPerSecond)).EndInit();
            this.groupBoxVideoRecorderSettings.ResumeLayout(false);
            this.groupBoxVideoRecorderSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numericUpDownRecFramesPerSecond;
        private System.Windows.Forms.Button buttonInit;
        private System.Windows.Forms.Button buttonRecord;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.GroupBox groupBoxVideoRecorderSettings;
        private System.Windows.Forms.ToolTip controlToolTips;
        private System.Windows.Forms.Label labelFileName;
        private System.Windows.Forms.NumericUpDown numericUpDownHeight;
        private System.Windows.Forms.Label labelHeight;
        private System.Windows.Forms.NumericUpDown numericUpDownWidth;
        private System.Windows.Forms.Label labelWidth;
        private System.Windows.Forms.Label labelFrameSize;
        private System.Windows.Forms.Label labelRecordingTime;
    }
}