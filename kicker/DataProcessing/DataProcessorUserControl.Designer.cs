namespace DataProcessing
{
    partial class DataProcessorUserControl
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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tabPagePositionPrediction = new System.Windows.Forms.TabPage();
            this.tabPageGameController = new System.Windows.Forms.TabPage();
            this.tabPageOwnBarDetection = new System.Windows.Forms.TabPage();
            this.tabPageOpponentBarDetection = new System.Windows.Forms.TabPage();
            this.tabPageBallDetection = new System.Windows.Forms.TabPage();
            this.tabPageCalibration = new System.Windows.Forms.TabPage();
            this.tabControlDataProcessor = new System.Windows.Forms.TabControl();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabControlDataProcessor.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(194, 74);
            this.panel1.TabIndex = 51;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(3, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(194, 21);
            this.comboBox1.TabIndex = 50;
            // 
            // tabPagePositionPrediction
            // 
            this.tabPagePositionPrediction.Location = new System.Drawing.Point(4, 22);
            this.tabPagePositionPrediction.Name = "tabPagePositionPrediction";
            this.tabPagePositionPrediction.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePositionPrediction.Size = new System.Drawing.Size(793, 519);
            this.tabPagePositionPrediction.TabIndex = 5;
            this.tabPagePositionPrediction.Text = "Position Prediction";
            this.tabPagePositionPrediction.UseVisualStyleBackColor = true;
            // 
            // tabPageGameController
            // 
            this.tabPageGameController.Location = new System.Drawing.Point(4, 22);
            this.tabPageGameController.Name = "tabPageGameController";
            this.tabPageGameController.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGameController.Size = new System.Drawing.Size(793, 519);
            this.tabPageGameController.TabIndex = 4;
            this.tabPageGameController.Text = "Game Controller";
            this.tabPageGameController.UseVisualStyleBackColor = true;
            // 
            // tabPageOwnBarDetection
            // 
            this.tabPageOwnBarDetection.Location = new System.Drawing.Point(4, 22);
            this.tabPageOwnBarDetection.Name = "tabPageOwnBarDetection";
            this.tabPageOwnBarDetection.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOwnBarDetection.Size = new System.Drawing.Size(793, 519);
            this.tabPageOwnBarDetection.TabIndex = 2;
            this.tabPageOwnBarDetection.Text = "Own Bar Detection";
            this.tabPageOwnBarDetection.UseVisualStyleBackColor = true;
            // 
            // tabPageOpponentBarDetection
            // 
            this.tabPageOpponentBarDetection.Location = new System.Drawing.Point(4, 22);
            this.tabPageOpponentBarDetection.Name = "tabPageOpponentBarDetection";
            this.tabPageOpponentBarDetection.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOpponentBarDetection.Size = new System.Drawing.Size(793, 519);
            this.tabPageOpponentBarDetection.TabIndex = 1;
            this.tabPageOpponentBarDetection.Text = "Opponent Bar Detection";
            this.tabPageOpponentBarDetection.UseVisualStyleBackColor = true;
            // 
            // tabPageBallDetection
            // 
            this.tabPageBallDetection.AutoScroll = true;
            this.tabPageBallDetection.Location = new System.Drawing.Point(4, 22);
            this.tabPageBallDetection.Name = "tabPageBallDetection";
            this.tabPageBallDetection.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBallDetection.Size = new System.Drawing.Size(793, 519);
            this.tabPageBallDetection.TabIndex = 0;
            this.tabPageBallDetection.Text = "Ball Detection";
            this.tabPageBallDetection.UseVisualStyleBackColor = true;
            // 
            // tabPageCalibration
            // 
            this.tabPageCalibration.Location = new System.Drawing.Point(4, 22);
            this.tabPageCalibration.Name = "tabPageCalibration";
            this.tabPageCalibration.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCalibration.Size = new System.Drawing.Size(793, 519);
            this.tabPageCalibration.TabIndex = 6;
            this.tabPageCalibration.Text = "Calibration";
            this.tabPageCalibration.UseVisualStyleBackColor = true;
            // 
            // tabControlDataProcessor
            // 
            this.tabControlDataProcessor.Controls.Add(this.tabPageCalibration);
            this.tabControlDataProcessor.Controls.Add(this.tabPageBallDetection);
            this.tabControlDataProcessor.Controls.Add(this.tabPageOpponentBarDetection);
            this.tabControlDataProcessor.Controls.Add(this.tabPageOwnBarDetection);
            this.tabControlDataProcessor.Controls.Add(this.tabPageGameController);
            this.tabControlDataProcessor.Controls.Add(this.tabPagePositionPrediction);
            this.tabControlDataProcessor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlDataProcessor.Location = new System.Drawing.Point(6, 6);
            this.tabControlDataProcessor.Name = "tabControlDataProcessor";
            this.tabControlDataProcessor.SelectedIndex = 0;
            this.tabControlDataProcessor.Size = new System.Drawing.Size(801, 545);
            this.tabControlDataProcessor.TabIndex = 0;
            // 
            // DefaultDataProcessorUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlDataProcessor);
            this.Name = "DefaultDataProcessorUserControl";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.Size = new System.Drawing.Size(813, 557);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tabControlDataProcessor.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TabPage tabPagePositionPrediction;
        private System.Windows.Forms.TabPage tabPageGameController;
        private System.Windows.Forms.TabPage tabPageOwnBarDetection;
        private System.Windows.Forms.TabPage tabPageOpponentBarDetection;
        private System.Windows.Forms.TabPage tabPageBallDetection;
        private System.Windows.Forms.TabPage tabPageCalibration;
        private System.Windows.Forms.TabControl tabControlDataProcessor;

    }
}