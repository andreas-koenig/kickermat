namespace ObjectDetection
{
    sealed partial class BasicObjectDetectionUserControl
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxGeneralSettings = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxEnableDetection = new System.Windows.Forms.CheckBox();
            this.buttonExecute = new System.Windows.Forms.Button();
            this.panelModuleSpecificControls = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxUpdateDisplay = new System.Windows.Forms.CheckBox();
            this.imageBox1 = new Emgu.CV.UI.ImageBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBoxGeneralSettings.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(880, 420);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.groupBoxGeneralSettings, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panelModuleSpecificControls, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(434, 414);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // groupBoxGeneralSettings
            // 
            this.groupBoxGeneralSettings.Controls.Add(this.tableLayoutPanel4);
            this.groupBoxGeneralSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxGeneralSettings.Location = new System.Drawing.Point(3, 3);
            this.groupBoxGeneralSettings.Name = "groupBoxGeneralSettings";
            this.groupBoxGeneralSettings.Size = new System.Drawing.Size(428, 50);
            this.groupBoxGeneralSettings.TabIndex = 2;
            this.groupBoxGeneralSettings.TabStop = false;
            this.groupBoxGeneralSettings.Text = "General Settings";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tableLayoutPanel4.Controls.Add(this.checkBoxEnableDetection, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.buttonExecute, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(422, 31);
            this.tableLayoutPanel4.TabIndex = 86;
            // 
            // checkBoxEnableDetection
            // 
            this.checkBoxEnableDetection.AutoSize = true;
            this.checkBoxEnableDetection.Checked = true;
            this.checkBoxEnableDetection.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnableDetection.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBoxEnableDetection.Location = new System.Drawing.Point(3, 3);
            this.checkBoxEnableDetection.Name = "checkBoxEnableDetection";
            this.checkBoxEnableDetection.Size = new System.Drawing.Size(108, 25);
            this.checkBoxEnableDetection.TabIndex = 1;
            this.checkBoxEnableDetection.Text = "Enable Detection";
            this.checkBoxEnableDetection.UseVisualStyleBackColor = true;
            this.checkBoxEnableDetection.CheckedChanged += new System.EventHandler(this.CheckBoxEnableDetection_CheckedChanged);
            // 
            // buttonExecute
            // 
            this.buttonExecute.Location = new System.Drawing.Point(344, 3);
            this.buttonExecute.Name = "buttonExecute";
            this.buttonExecute.Size = new System.Drawing.Size(75, 23);
            this.buttonExecute.TabIndex = 85;
            this.buttonExecute.Text = "Execute";
            this.buttonExecute.UseVisualStyleBackColor = true;
            this.buttonExecute.Click += new System.EventHandler(this.ButtonExecute_Click);
            // 
            // panelModuleSpecificControls
            // 
            this.panelModuleSpecificControls.AutoScroll = true;
            this.panelModuleSpecificControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelModuleSpecificControls.Location = new System.Drawing.Point(3, 60);
            this.panelModuleSpecificControls.Name = "panelModuleSpecificControls";
            this.panelModuleSpecificControls.Size = new System.Drawing.Size(428, 351);
            this.panelModuleSpecificControls.TabIndex = 3;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel5, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.imageBox1, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(443, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(434, 414);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.Controls.Add(this.checkBoxUpdateDisplay, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 387);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(428, 24);
            this.tableLayoutPanel5.TabIndex = 7;
            // 
            // checkBoxUpdateDisplay
            // 
            this.checkBoxUpdateDisplay.AutoSize = true;
            this.checkBoxUpdateDisplay.Location = new System.Drawing.Point(3, 3);
            this.checkBoxUpdateDisplay.Name = "checkBoxUpdateDisplay";
            this.checkBoxUpdateDisplay.Size = new System.Drawing.Size(98, 17);
            this.checkBoxUpdateDisplay.TabIndex = 5;
            this.checkBoxUpdateDisplay.Text = "Update Display";
            this.checkBoxUpdateDisplay.UseVisualStyleBackColor = true;
            this.checkBoxUpdateDisplay.CheckedChanged += new System.EventHandler(this.CheckBoxUpdateDisplay_CheckedChanged);
            // 
            // imageBox1
            // 
            this.imageBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBox1.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.PanAndZoom;
            this.imageBox1.Location = new System.Drawing.Point(3, 3);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(428, 378);
            this.imageBox1.TabIndex = 2;
            this.imageBox1.TabStop = false;
            // 
            // BasicObjectDetectionUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "BasicObjectDetectionUserControl";
            this.Size = new System.Drawing.Size(880, 420);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBoxGeneralSettings.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBoxGeneralSettings;
        private System.Windows.Forms.Button buttonExecute;
        private System.Windows.Forms.CheckBox checkBoxEnableDetection;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Panel panelModuleSpecificControls;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.CheckBox checkBoxUpdateDisplay;
        private Emgu.CV.UI.ImageBox imageBox1;
    }
}