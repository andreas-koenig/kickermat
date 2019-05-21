namespace GameController
{
    partial class SmoothingAdvancedGameControllerUserControl
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
            this.labelDistanceLeft = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelPreciseMovementRight = new System.Windows.Forms.Label();
            this.preciseDistanceRight = new System.Windows.Forms.TrackBar();
            this.labelDistanceRight = new System.Windows.Forms.Label();
            this.labelPreciseMovementLeft = new System.Windows.Forms.Label();
            this.preciseDistanceLeft = new System.Windows.Forms.TrackBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelThresholdValueRight = new System.Windows.Forms.Label();
            this.ThresholdRight = new System.Windows.Forms.TrackBar();
            this.labelMaxThresholdRight = new System.Windows.Forms.Label();
            this.labelThresholdValueLeft = new System.Windows.Forms.Label();
            this.ThresholdLeft = new System.Windows.Forms.TrackBar();
            this.labelMaxThresholdLeft = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.preciseDistanceRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.preciseDistanceLeft)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ThresholdRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThresholdLeft)).BeginInit();
            this.SuspendLayout();
            // 
            // labelDistanceLeft
            // 
            this.labelDistanceLeft.AutoSize = true;
            this.labelDistanceLeft.Location = new System.Drawing.Point(6, 25);
            this.labelDistanceLeft.Name = "labelDistanceLeft";
            this.labelDistanceLeft.Size = new System.Drawing.Size(73, 13);
            this.labelDistanceLeft.TabIndex = 0;
            this.labelDistanceLeft.Text = "Distance Left:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelPreciseMovementRight);
            this.groupBox1.Controls.Add(this.preciseDistanceRight);
            this.groupBox1.Controls.Add(this.labelDistanceRight);
            this.groupBox1.Controls.Add(this.labelPreciseMovementLeft);
            this.groupBox1.Controls.Add(this.preciseDistanceLeft);
            this.groupBox1.Controls.Add(this.labelDistanceLeft);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(549, 82);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Precise Movement Distance";
            // 
            // labelPreciseMovementRight
            // 
            this.labelPreciseMovementRight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelPreciseMovementRight.Location = new System.Drawing.Point(143, 44);
            this.labelPreciseMovementRight.Name = "labelPreciseMovementRight";
            this.labelPreciseMovementRight.Size = new System.Drawing.Size(41, 15);
            this.labelPreciseMovementRight.TabIndex = 44;
            this.labelPreciseMovementRight.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // preciseDistanceRight
            // 
            this.preciseDistanceRight.AutoSize = false;
            this.preciseDistanceRight.LargeChange = 1;
            this.preciseDistanceRight.Location = new System.Drawing.Point(187, 44);
            this.preciseDistanceRight.Maximum = 7;
            this.preciseDistanceRight.Name = "preciseDistanceRight";
            this.preciseDistanceRight.Size = new System.Drawing.Size(350, 16);
            this.preciseDistanceRight.TabIndex = 43;
            this.preciseDistanceRight.Value = 1;
            this.preciseDistanceRight.Scroll += new System.EventHandler(this.ScrollDistanceRight);
            // 
            // labelDistanceRight
            // 
            this.labelDistanceRight.AutoSize = true;
            this.labelDistanceRight.Location = new System.Drawing.Point(6, 47);
            this.labelDistanceRight.Name = "labelDistanceRight";
            this.labelDistanceRight.Size = new System.Drawing.Size(80, 13);
            this.labelDistanceRight.TabIndex = 42;
            this.labelDistanceRight.Text = "Distance Right:";
            // 
            // labelPreciseMovementLeft
            // 
            this.labelPreciseMovementLeft.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelPreciseMovementLeft.Location = new System.Drawing.Point(143, 22);
            this.labelPreciseMovementLeft.Name = "labelPreciseMovementLeft";
            this.labelPreciseMovementLeft.Size = new System.Drawing.Size(41, 15);
            this.labelPreciseMovementLeft.TabIndex = 41;
            this.labelPreciseMovementLeft.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // preciseDistanceLeft
            // 
            this.preciseDistanceLeft.AutoSize = false;
            this.preciseDistanceLeft.LargeChange = 1;
            this.preciseDistanceLeft.Location = new System.Drawing.Point(187, 22);
            this.preciseDistanceLeft.Maximum = 7;
            this.preciseDistanceLeft.Name = "preciseDistanceLeft";
            this.preciseDistanceLeft.Size = new System.Drawing.Size(350, 16);
            this.preciseDistanceLeft.TabIndex = 1;
            this.preciseDistanceLeft.Value = 2;
            this.preciseDistanceLeft.Scroll += new System.EventHandler(this.ScrollDistanceLeft);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelThresholdValueRight);
            this.groupBox2.Controls.Add(this.ThresholdRight);
            this.groupBox2.Controls.Add(this.labelMaxThresholdRight);
            this.groupBox2.Controls.Add(this.labelThresholdValueLeft);
            this.groupBox2.Controls.Add(this.ThresholdLeft);
            this.groupBox2.Controls.Add(this.labelMaxThresholdLeft);
            this.groupBox2.Location = new System.Drawing.Point(3, 91);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(549, 82);
            this.groupBox2.TabIndex = 45;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Movement Threshold";
            // 
            // labelThresholdValueRight
            // 
            this.labelThresholdValueRight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelThresholdValueRight.Location = new System.Drawing.Point(143, 44);
            this.labelThresholdValueRight.Name = "labelThresholdValueRight";
            this.labelThresholdValueRight.Size = new System.Drawing.Size(41, 15);
            this.labelThresholdValueRight.TabIndex = 44;
            this.labelThresholdValueRight.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ThresholdRight
            // 
            this.ThresholdRight.AutoSize = false;
            this.ThresholdRight.LargeChange = 1;
            this.ThresholdRight.Location = new System.Drawing.Point(187, 44);
            this.ThresholdRight.Maximum = 30;
            this.ThresholdRight.Name = "ThresholdRight";
            this.ThresholdRight.Size = new System.Drawing.Size(350, 16);
            this.ThresholdRight.TabIndex = 43;
            this.ThresholdRight.Value = 17;
            this.ThresholdRight.Scroll += new System.EventHandler(this.scrollMaximumThresholdRight);
            // 
            // labelMaxThresholdRight
            // 
            this.labelMaxThresholdRight.AutoSize = true;
            this.labelMaxThresholdRight.Location = new System.Drawing.Point(6, 47);
            this.labelMaxThresholdRight.Name = "labelMaxThresholdRight";
            this.labelMaxThresholdRight.Size = new System.Drawing.Size(132, 13);
            this.labelMaxThresholdRight.TabIndex = 42;
            this.labelMaxThresholdRight.Text = "Maximum Threshold Right:";
            // 
            // labelThresholdValueLeft
            // 
            this.labelThresholdValueLeft.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelThresholdValueLeft.Location = new System.Drawing.Point(143, 22);
            this.labelThresholdValueLeft.Name = "labelThresholdValueLeft";
            this.labelThresholdValueLeft.Size = new System.Drawing.Size(41, 15);
            this.labelThresholdValueLeft.TabIndex = 41;
            this.labelThresholdValueLeft.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ThresholdLeft
            // 
            this.ThresholdLeft.AutoSize = false;
            this.ThresholdLeft.LargeChange = 1;
            this.ThresholdLeft.Location = new System.Drawing.Point(187, 22);
            this.ThresholdLeft.Maximum = 30;
            this.ThresholdLeft.Name = "ThresholdLeft";
            this.ThresholdLeft.Size = new System.Drawing.Size(350, 16);
            this.ThresholdLeft.TabIndex = 1;
            this.ThresholdLeft.Value = 17;
            this.ThresholdLeft.Scroll += new System.EventHandler(this.scrollMaximumThresholdLeft);
            // 
            // labelMaxThresholdLeft
            // 
            this.labelMaxThresholdLeft.AutoSize = true;
            this.labelMaxThresholdLeft.Location = new System.Drawing.Point(6, 25);
            this.labelMaxThresholdLeft.Name = "labelMaxThresholdLeft";
            this.labelMaxThresholdLeft.Size = new System.Drawing.Size(125, 13);
            this.labelMaxThresholdLeft.TabIndex = 0;
            this.labelMaxThresholdLeft.Text = "Maximum Threshold Left:";
            // 
            // SmoothingAdvancedGameControllerUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "SmoothingAdvancedGameControllerUserControl";
            this.Size = new System.Drawing.Size(560, 180);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.preciseDistanceRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.preciseDistanceLeft)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ThresholdRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThresholdLeft)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelDistanceLeft;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelPreciseMovementLeft;
        private System.Windows.Forms.Label labelPreciseMovementRight;
        private System.Windows.Forms.Label labelDistanceRight;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labelThresholdValueRight;
        private System.Windows.Forms.Label labelMaxThresholdRight;
        private System.Windows.Forms.Label labelThresholdValueLeft;
        private System.Windows.Forms.Label labelMaxThresholdLeft;
        public System.Windows.Forms.TrackBar preciseDistanceLeft;
        public System.Windows.Forms.TrackBar preciseDistanceRight;
        public System.Windows.Forms.TrackBar ThresholdRight;
        public System.Windows.Forms.TrackBar ThresholdLeft;
    }
}
