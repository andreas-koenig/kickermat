namespace Kicker
{
    using Utilities;
    using System.Threading;

    /// <summary>
    /// Das Hauptformular der Benutzeroberfläche.
    /// </summary>
    sealed partial class FormMain
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

            Thread.Sleep(1000);

            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
            System.Windows.Forms.ToolStripSplitButton toolStripOpen;
            System.Windows.Forms.StatusStrip statusStripMainForm;
            System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
            System.Windows.Forms.ToolStrip toolStripMainForm;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.toolStripOpenDriver = new System.Windows.Forms.ToolStripMenuItem();
            this.prosilicaVimbaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripOpenVideo = new System.Windows.Forms.ToolStripMenuItem();
            this.staticImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusLabelLoadedFile = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControlMainForm = new System.Windows.Forms.TabControl();
            this.tabPageVideoSource = new System.Windows.Forms.TabPage();
            this.tabPageDataProcessing = new System.Windows.Forms.TabPage();
            this.tabPageVideoRecorder = new System.Windows.Forms.TabPage();
            this.tabPageCommunication = new System.Windows.Forms.TabPage();
            this.tabPageLogging = new System.Windows.Forms.TabPage();
            this.tabPageGatewayMessages = new System.Windows.Forms.TabPage();
            this.toolStripOpenLast = new System.Windows.Forms.ToolStripButton();
            this.toolStripSaveSettings = new System.Windows.Forms.ToolStripButton();
            this.toolStripRecalibrate = new System.Windows.Forms.ToolStripButton();
            this.toolStripStartAquisition = new System.Windows.Forms.ToolStripButton();
            this.toolStripStopAquisition = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripPlay = new System.Windows.Forms.ToolStripButton();
            this.toolStripStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSnap = new System.Windows.Forms.ToolStripButton();
            this.toolStripSave = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripSaveRaw = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSaveRgb = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            toolStripOpen = new System.Windows.Forms.ToolStripSplitButton();
            statusStripMainForm = new System.Windows.Forms.StatusStrip();
            tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            toolStripMainForm = new System.Windows.Forms.ToolStrip();
            statusStripMainForm.SuspendLayout();
            tableLayoutPanel.SuspendLayout();
            this.tabControlMainForm.SuspendLayout();
            toolStripMainForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripOpen
            // 
            toolStripOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripOpen.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripOpenDriver,
            this.toolStripOpenVideo,
            this.staticImageToolStripMenuItem});
            toolStripOpen.Image = global::Kicker.Properties.Resources.openHS;
            toolStripOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripOpen.Name = "toolStripOpen";
            toolStripOpen.Size = new System.Drawing.Size(32, 22);
            toolStripOpen.Text = "toolStripOpen";
            toolStripOpen.ToolTipText = "Open new Driver or Video";
            // 
            // toolStripOpenDriver
            // 
            this.toolStripOpenDriver.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.prosilicaVimbaToolStripMenuItem});
            this.toolStripOpenDriver.Name = "toolStripOpenDriver";
            this.toolStripOpenDriver.Size = new System.Drawing.Size(139, 22);
            this.toolStripOpenDriver.Text = "Camera";
            // 
            // prosilicaVimbaToolStripMenuItem
            // 
            this.prosilicaVimbaToolStripMenuItem.Name = "prosilicaVimbaToolStripMenuItem";
            this.prosilicaVimbaToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.prosilicaVimbaToolStripMenuItem.Text = "Prosilica_Vimba";
            this.prosilicaVimbaToolStripMenuItem.Click += new System.EventHandler(this.prosilicaVimbaToolStripMenuItem_Click);
            // 
            // toolStripOpenVideo
            // 
            this.toolStripOpenVideo.Name = "toolStripOpenVideo";
            this.toolStripOpenVideo.Size = new System.Drawing.Size(139, 22);
            this.toolStripOpenVideo.Text = "Vide&o...";
            this.toolStripOpenVideo.Click += new System.EventHandler(this.ToolStripOpenVideo_Click);
            // 
            // staticImageToolStripMenuItem
            // 
            this.staticImageToolStripMenuItem.Name = "staticImageToolStripMenuItem";
            this.staticImageToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.staticImageToolStripMenuItem.Text = "Static Image";
            this.staticImageToolStripMenuItem.Click += new System.EventHandler(this.staticImageToolStripMenuItem_Click);
            // 
            // statusStripMainForm
            // 
            statusStripMainForm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabelLoadedFile});
            statusStripMainForm.Location = new System.Drawing.Point(0, 711);
            statusStripMainForm.Name = "statusStripMainForm";
            statusStripMainForm.Size = new System.Drawing.Size(1182, 22);
            statusStripMainForm.TabIndex = 3;
            statusStripMainForm.Text = "statusStrip1";
            // 
            // statusLabelLoadedFile
            // 
            this.statusLabelLoadedFile.Name = "statusLabelLoadedFile";
            this.statusLabelLoadedFile.Size = new System.Drawing.Size(83, 17);
            this.statusLabelLoadedFile.Text = "No File loaded";
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 1;
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel.Controls.Add(this.tabControlMainForm, 0, 0);
            tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel.Location = new System.Drawing.Point(0, 25);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 1;
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 686F));
            tableLayoutPanel.Size = new System.Drawing.Size(1182, 686);
            tableLayoutPanel.TabIndex = 4;
            // 
            // tabControlMainForm
            // 
            this.tabControlMainForm.Controls.Add(this.tabPageVideoSource);
            this.tabControlMainForm.Controls.Add(this.tabPageDataProcessing);
            this.tabControlMainForm.Controls.Add(this.tabPageVideoRecorder);
            this.tabControlMainForm.Controls.Add(this.tabPageCommunication);
            this.tabControlMainForm.Controls.Add(this.tabPageLogging);
            this.tabControlMainForm.Controls.Add(this.tabPageGatewayMessages);
            this.tabControlMainForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMainForm.HotTrack = true;
            this.tabControlMainForm.Location = new System.Drawing.Point(3, 3);
            this.tabControlMainForm.Name = "tabControlMainForm";
            this.tabControlMainForm.SelectedIndex = 0;
            this.tabControlMainForm.Size = new System.Drawing.Size(1176, 680);
            this.tabControlMainForm.TabIndex = 1;
            // 
            // tabPageVideoSource
            // 
            this.tabPageVideoSource.AutoScroll = true;
            this.tabPageVideoSource.Location = new System.Drawing.Point(4, 22);
            this.tabPageVideoSource.Name = "tabPageVideoSource";
            this.tabPageVideoSource.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageVideoSource.Size = new System.Drawing.Size(1168, 654);
            this.tabPageVideoSource.TabIndex = 0;
            this.tabPageVideoSource.Text = "Video Source";
            this.tabPageVideoSource.UseVisualStyleBackColor = true;
            // 
            // tabPageDataProcessing
            // 
            this.tabPageDataProcessing.Location = new System.Drawing.Point(4, 22);
            this.tabPageDataProcessing.Name = "tabPageDataProcessing";
            this.tabPageDataProcessing.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDataProcessing.Size = new System.Drawing.Size(978, 654);
            this.tabPageDataProcessing.TabIndex = 1;
            this.tabPageDataProcessing.Text = "Data Processing";
            this.tabPageDataProcessing.UseVisualStyleBackColor = true;
            // 
            // tabPageVideoRecorder
            // 
            this.tabPageVideoRecorder.Location = new System.Drawing.Point(4, 22);
            this.tabPageVideoRecorder.Name = "tabPageVideoRecorder";
            this.tabPageVideoRecorder.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageVideoRecorder.Size = new System.Drawing.Size(978, 654);
            this.tabPageVideoRecorder.TabIndex = 3;
            this.tabPageVideoRecorder.Text = "Video Recorder";
            this.tabPageVideoRecorder.UseVisualStyleBackColor = true;
            // 
            // tabPageCommunication
            // 
            this.tabPageCommunication.Location = new System.Drawing.Point(4, 22);
            this.tabPageCommunication.Name = "tabPageCommunication";
            this.tabPageCommunication.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCommunication.Size = new System.Drawing.Size(978, 654);
            this.tabPageCommunication.TabIndex = 4;
            this.tabPageCommunication.Text = "Communication";
            this.tabPageCommunication.UseVisualStyleBackColor = true;
            // 
            // tabPageLogging
            // 
            this.tabPageLogging.Location = new System.Drawing.Point(4, 22);
            this.tabPageLogging.Name = "tabPageLogging";
            this.tabPageLogging.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLogging.Size = new System.Drawing.Size(978, 654);
            this.tabPageLogging.TabIndex = 2;
            this.tabPageLogging.Text = "Logging";
            this.tabPageLogging.UseVisualStyleBackColor = true;
            // 
            // tabPageGatewayMessages
            // 
            this.tabPageGatewayMessages.Location = new System.Drawing.Point(4, 22);
            this.tabPageGatewayMessages.Name = "tabPageGatewayMessages";
            this.tabPageGatewayMessages.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGatewayMessages.Size = new System.Drawing.Size(978, 654);
            this.tabPageGatewayMessages.TabIndex = 5;
            this.tabPageGatewayMessages.Text = "Gateway Messages";
            this.tabPageGatewayMessages.UseVisualStyleBackColor = true;
            // 
            // toolStripMainForm
            // 
            toolStripMainForm.CanOverflow = false;
            toolStripMainForm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            toolStripOpen,
            this.toolStripOpenLast,
            this.toolStripSaveSettings,
            this.toolStripRecalibrate,
            toolStripSeparator3,
            this.toolStripStartAquisition,
            this.toolStripStopAquisition,
            this.toolStripSeparator4,
            this.toolStripPlay,
            this.toolStripStop,
            this.toolStripSnap,
            toolStripSeparator2,
            this.toolStripSave});
            toolStripMainForm.Location = new System.Drawing.Point(0, 0);
            toolStripMainForm.Name = "toolStripMainForm";
            toolStripMainForm.Size = new System.Drawing.Size(1182, 25);
            toolStripMainForm.TabIndex = 2;
            // 
            // toolStripOpenLast
            // 
            this.toolStripOpenLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripOpenLast.Image = global::Kicker.Properties.Resources.lastHS;
            this.toolStripOpenLast.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripOpenLast.Name = "toolStripOpenLast";
            this.toolStripOpenLast.Size = new System.Drawing.Size(23, 22);
            this.toolStripOpenLast.Text = "toolStripOpenLast";
            this.toolStripOpenLast.ToolTipText = "Open last Driver or Video again";
            this.toolStripOpenLast.Click += new System.EventHandler(this.ToolStripOpenLast_Click);
            // 
            // toolStripSaveSettings
            // 
            this.toolStripSaveSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSaveSettings.Image = global::Kicker.Properties.Resources.saveRed;
            this.toolStripSaveSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSaveSettings.Name = "toolStripSaveSettings";
            this.toolStripSaveSettings.Size = new System.Drawing.Size(23, 22);
            this.toolStripSaveSettings.Text = "toolStripSaveSettings";
            this.toolStripSaveSettings.ToolTipText = "Save all Module Settings";
            this.toolStripSaveSettings.Click += new System.EventHandler(this.ToolStripSaveSettings_Click);
            // 
            // toolStripRecalibrate
            // 
            this.toolStripRecalibrate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripRecalibrate.Enabled = false;
            this.toolStripRecalibrate.Image = global::Kicker.Properties.Resources.ReloadHS;
            this.toolStripRecalibrate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripRecalibrate.Name = "toolStripRecalibrate";
            this.toolStripRecalibrate.Size = new System.Drawing.Size(23, 22);
            this.toolStripRecalibrate.Text = "toolStripRecalibrate";
            this.toolStripRecalibrate.ToolTipText = "Repeat Calibration w/o reloading Driver";
            this.toolStripRecalibrate.Click += new System.EventHandler(this.ToolStripRecalibrate_Click);
            // 
            // toolStripStartAquisition
            // 
            this.toolStripStartAquisition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripStartAquisition.Enabled = false;
            this.toolStripStartAquisition.Image = global::Kicker.Properties.Resources.StartAquisition;
            this.toolStripStartAquisition.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripStartAquisition.Name = "toolStripStartAquisition";
            this.toolStripStartAquisition.Size = new System.Drawing.Size(23, 22);
            this.toolStripStartAquisition.Text = "Start acquisition";
            this.toolStripStartAquisition.Click += new System.EventHandler(this.toolStripStartAquisition_Click);
            // 
            // toolStripStopAquisition
            // 
            this.toolStripStopAquisition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripStopAquisition.Enabled = false;
            this.toolStripStopAquisition.Image = global::Kicker.Properties.Resources.StopAquisition;
            this.toolStripStopAquisition.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripStopAquisition.Name = "toolStripStopAquisition";
            this.toolStripStopAquisition.Size = new System.Drawing.Size(23, 22);
            this.toolStripStopAquisition.Text = "Stop acquisition";
            this.toolStripStopAquisition.Click += new System.EventHandler(this.toolStripStopAquisition_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripPlay
            // 
            this.toolStripPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripPlay.Enabled = false;
            this.toolStripPlay.Image = global::Kicker.Properties.Resources.PlayHS;
            this.toolStripPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripPlay.Name = "toolStripPlay";
            this.toolStripPlay.Size = new System.Drawing.Size(23, 22);
            this.toolStripPlay.Text = "toolStripPlay";
            this.toolStripPlay.ToolTipText = "Play continously";
            this.toolStripPlay.Click += new System.EventHandler(this.ToolStripPlay_Click);
            // 
            // toolStripStop
            // 
            this.toolStripStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripStop.Enabled = false;
            this.toolStripStop.Image = global::Kicker.Properties.Resources.StopHS;
            this.toolStripStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripStop.Name = "toolStripStop";
            this.toolStripStop.Size = new System.Drawing.Size(23, 22);
            this.toolStripStop.Text = "toolStripStop";
            this.toolStripStop.ToolTipText = "Stop playing";
            this.toolStripStop.Click += new System.EventHandler(this.ToolStripStop_Click);
            // 
            // toolStripSnap
            // 
            this.toolStripSnap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSnap.Enabled = false;
            this.toolStripSnap.Image = global::Kicker.Properties.Resources.PauseHS;
            this.toolStripSnap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSnap.Name = "toolStripSnap";
            this.toolStripSnap.Size = new System.Drawing.Size(23, 22);
            this.toolStripSnap.Text = "toolStripSnap";
            this.toolStripSnap.ToolTipText = "Snap - Run for a single frame";
            this.toolStripSnap.Click += new System.EventHandler(this.ToolStripSnap_Click);
            // 
            // toolStripSave
            // 
            this.toolStripSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSave.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSaveRaw,
            this.toolStripSaveRgb});
            this.toolStripSave.Enabled = false;
            this.toolStripSave.Image = global::Kicker.Properties.Resources.saveHS;
            this.toolStripSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSave.Name = "toolStripSave";
            this.toolStripSave.Size = new System.Drawing.Size(29, 22);
            this.toolStripSave.Text = "toolStripSave";
            this.toolStripSave.ToolTipText = "Save Image";
            // 
            // toolStripSaveRaw
            // 
            this.toolStripSaveRaw.Enabled = false;
            this.toolStripSaveRaw.Name = "toolStripSaveRaw";
            this.toolStripSaveRaw.Size = new System.Drawing.Size(171, 22);
            this.toolStripSaveRaw.Text = "Save R&AW Image...";
            this.toolStripSaveRaw.Click += new System.EventHandler(this.ToolStripSaveRaw_Click);
            // 
            // toolStripSaveRgb
            // 
            this.toolStripSaveRgb.Enabled = false;
            this.toolStripSaveRgb.Name = "toolStripSaveRgb";
            this.toolStripSaveRgb.Size = new System.Drawing.Size(171, 22);
            this.toolStripSaveRgb.Text = "Save RG&B Image...";
            this.toolStripSaveRgb.Click += new System.EventHandler(this.ToolStripSaveRgb_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1182, 733);
            this.Controls.Add(tableLayoutPanel);
            this.Controls.Add(statusStripMainForm);
            this.Controls.Add(toolStripMainForm);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Kicker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.ResizeBegin += new System.EventHandler(this.FormMain_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.FormMain_ResizeEnd);
            statusStripMainForm.ResumeLayout(false);
            statusStripMainForm.PerformLayout();
            tableLayoutPanel.ResumeLayout(false);
            this.tabControlMainForm.ResumeLayout(false);
            toolStripMainForm.ResumeLayout(false);
            toolStripMainForm.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStripButton toolStripPlay;
        private System.Windows.Forms.ToolStripButton toolStripStop;
        private System.Windows.Forms.ToolStripStatusLabel statusLabelLoadedFile;
        private System.Windows.Forms.ToolStripMenuItem toolStripOpenDriver;
        private System.Windows.Forms.ToolStripMenuItem toolStripOpenVideo;
        private System.Windows.Forms.TabPage tabPageVideoSource;
        private System.Windows.Forms.TabPage tabPageDataProcessing;
        private System.Windows.Forms.TabPage tabPageVideoRecorder;
        private System.Windows.Forms.TabPage tabPageLogging;
        private System.Windows.Forms.ToolStripMenuItem toolStripSaveRaw;
        private System.Windows.Forms.ToolStripMenuItem toolStripSaveRgb;
        private System.Windows.Forms.TabPage tabPageCommunication;
        private System.Windows.Forms.ToolStripButton toolStripSnap;
        private System.Windows.Forms.TabPage tabPageGatewayMessages;
        private System.Windows.Forms.ToolStripButton toolStripOpenLast;
        private System.Windows.Forms.ToolStripButton toolStripRecalibrate;
        private System.Windows.Forms.ToolStripDropDownButton toolStripSave;
        private System.Windows.Forms.ToolStripButton toolStripSaveSettings;
        private System.Windows.Forms.TabControl tabControlMainForm;
        private System.Windows.Forms.ToolStripMenuItem prosilicaVimbaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem staticImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripStartAquisition;
        private System.Windows.Forms.ToolStripButton toolStripStopAquisition;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    }
}

