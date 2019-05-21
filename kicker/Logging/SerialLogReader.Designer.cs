namespace Logging
{
    partial class SerialLogReader
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
            this.listBoxLogging = new System.Windows.Forms.ListBox();
            this.contextMenuStripListBoxLogging = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemClear = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSave = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CheckBoxLoggingEnabled = new System.Windows.Forms.CheckBox();
            this.labelStopBits = new System.Windows.Forms.Label();
            this.comboBoxStopBits = new System.Windows.Forms.ComboBox();
            this.labelParity = new System.Windows.Forms.Label();
            this.comboBoxParity = new System.Windows.Forms.ComboBox();
            this.labelDataBits = new System.Windows.Forms.Label();
            this.comboBoxDataBits = new System.Windows.Forms.ComboBox();
            this.labelBaudrate = new System.Windows.Forms.Label();
            this.comboBoxBaudrate = new System.Windows.Forms.ComboBox();
            this.labelPort = new System.Windows.Forms.Label();
            this.comboBoxSerialPorts = new System.Windows.Forms.ComboBox();
            this.SerialPortLogging = new System.IO.Ports.SerialPort(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.contextMenuStripListBoxLogging.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.listBoxLogging, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(614, 385);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // listBoxLogging
            // 
            this.listBoxLogging.ContextMenuStrip = this.contextMenuStripListBoxLogging;
            this.listBoxLogging.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxLogging.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxLogging.FormattingEnabled = true;
            this.listBoxLogging.ItemHeight = 14;
            this.listBoxLogging.Location = new System.Drawing.Point(3, 56);
            this.listBoxLogging.Name = "listBoxLogging";
            this.listBoxLogging.Size = new System.Drawing.Size(608, 326);
            this.listBoxLogging.TabIndex = 1;
            // 
            // contextMenuStripListBoxLogging
            // 
            this.contextMenuStripListBoxLogging.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                                                                                                            this.ToolStripMenuItemClear,
                                                                                                            this.ToolStripMenuItemSave});
            this.contextMenuStripListBoxLogging.Name = "contextMenuStripListBoxLogging";
            this.contextMenuStripListBoxLogging.Size = new System.Drawing.Size(153, 70);
            // 
            // ToolStripMenuItemClear
            // 
            this.ToolStripMenuItemClear.Name = "ToolStripMenuItemClear";
            this.ToolStripMenuItemClear.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItemClear.Text = "Clear";
            this.ToolStripMenuItemClear.Click += new System.EventHandler(this.ToolStripMenuItemClear_Click);
            // 
            // ToolStripMenuItemSave
            // 
            this.ToolStripMenuItemSave.Name = "ToolStripMenuItemSave";
            this.ToolStripMenuItemSave.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItemSave.Text = "Save...";
            this.ToolStripMenuItemSave.Click += new System.EventHandler(this.ToolStripMenuItemSave_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CheckBoxLoggingEnabled);
            this.panel1.Controls.Add(this.labelStopBits);
            this.panel1.Controls.Add(this.comboBoxStopBits);
            this.panel1.Controls.Add(this.labelParity);
            this.panel1.Controls.Add(this.comboBoxParity);
            this.panel1.Controls.Add(this.labelDataBits);
            this.panel1.Controls.Add(this.comboBoxDataBits);
            this.panel1.Controls.Add(this.labelBaudrate);
            this.panel1.Controls.Add(this.comboBoxBaudrate);
            this.panel1.Controls.Add(this.labelPort);
            this.panel1.Controls.Add(this.comboBoxSerialPorts);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(608, 47);
            this.panel1.TabIndex = 2;
            // 
            // CheckBoxLoggingEnabled
            // 
            this.CheckBoxLoggingEnabled.AutoSize = true;
            this.CheckBoxLoggingEnabled.Location = new System.Drawing.Point(509, 19);
            this.CheckBoxLoggingEnabled.Name = "CheckBoxLoggingEnabled";
            this.CheckBoxLoggingEnabled.Size = new System.Drawing.Size(65, 17);
            this.CheckBoxLoggingEnabled.TabIndex = 10;
            this.CheckBoxLoggingEnabled.Text = "Enabled";
            this.CheckBoxLoggingEnabled.UseVisualStyleBackColor = true;
            this.CheckBoxLoggingEnabled.CheckedChanged += new System.EventHandler(this.CheckBoxLoggingEnabled_CheckedChanged);
            // 
            // labelStopBits
            // 
            this.labelStopBits.AutoSize = true;
            this.labelStopBits.Location = new System.Drawing.Point(407, 3);
            this.labelStopBits.Name = "labelStopBits";
            this.labelStopBits.Size = new System.Drawing.Size(52, 13);
            this.labelStopBits.TabIndex = 9;
            this.labelStopBits.Text = "Stop Bits:";
            // 
            // comboBoxStopBits
            // 
            this.comboBoxStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStopBits.FormattingEnabled = true;
            this.comboBoxStopBits.Location = new System.Drawing.Point(407, 19);
            this.comboBoxStopBits.Name = "comboBoxStopBits";
            this.comboBoxStopBits.Size = new System.Drawing.Size(95, 21);
            this.comboBoxStopBits.TabIndex = 8;
            // 
            // labelParity
            // 
            this.labelParity.AutoSize = true;
            this.labelParity.Location = new System.Drawing.Point(306, 3);
            this.labelParity.Name = "labelParity";
            this.labelParity.Size = new System.Drawing.Size(33, 13);
            this.labelParity.TabIndex = 7;
            this.labelParity.Text = "Parity";
            // 
            // comboBoxParity
            // 
            this.comboBoxParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxParity.FormattingEnabled = true;
            this.comboBoxParity.Location = new System.Drawing.Point(306, 19);
            this.comboBoxParity.Name = "comboBoxParity";
            this.comboBoxParity.Size = new System.Drawing.Size(95, 21);
            this.comboBoxParity.TabIndex = 6;
            // 
            // labelDataBits
            // 
            this.labelDataBits.AutoSize = true;
            this.labelDataBits.Location = new System.Drawing.Point(205, 3);
            this.labelDataBits.Name = "labelDataBits";
            this.labelDataBits.Size = new System.Drawing.Size(53, 13);
            this.labelDataBits.TabIndex = 5;
            this.labelDataBits.Text = "Data Bits:";
            // 
            // comboBoxDataBits
            // 
            this.comboBoxDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDataBits.FormattingEnabled = true;
            this.comboBoxDataBits.Location = new System.Drawing.Point(205, 19);
            this.comboBoxDataBits.Name = "comboBoxDataBits";
            this.comboBoxDataBits.Size = new System.Drawing.Size(95, 21);
            this.comboBoxDataBits.TabIndex = 4;
            // 
            // labelBaudrate
            // 
            this.labelBaudrate.AutoSize = true;
            this.labelBaudrate.Location = new System.Drawing.Point(104, 3);
            this.labelBaudrate.Name = "labelBaudrate";
            this.labelBaudrate.Size = new System.Drawing.Size(53, 13);
            this.labelBaudrate.TabIndex = 3;
            this.labelBaudrate.Text = "Baudrate:";
            // 
            // comboBoxBaudrate
            // 
            this.comboBoxBaudrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBaudrate.FormattingEnabled = true;
            this.comboBoxBaudrate.Location = new System.Drawing.Point(104, 19);
            this.comboBoxBaudrate.Name = "comboBoxBaudrate";
            this.comboBoxBaudrate.Size = new System.Drawing.Size(95, 21);
            this.comboBoxBaudrate.TabIndex = 2;
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(3, 3);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(29, 13);
            this.labelPort.TabIndex = 1;
            this.labelPort.Text = "Port:";
            // 
            // comboBoxSerialPorts
            // 
            this.comboBoxSerialPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSerialPorts.FormattingEnabled = true;
            this.comboBoxSerialPorts.Location = new System.Drawing.Point(3, 19);
            this.comboBoxSerialPorts.Name = "comboBoxSerialPorts";
            this.comboBoxSerialPorts.Size = new System.Drawing.Size(95, 21);
            this.comboBoxSerialPorts.TabIndex = 0;
            // 
            // SerialPortLogging
            // 
            this.SerialPortLogging.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SerialPortLogging_DataReceived);
            // 
            // SerialLogReader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SerialLogReader";
            this.Size = new System.Drawing.Size(614, 385);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.contextMenuStripListBoxLogging.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.IO.Ports.SerialPort SerialPortLogging;
        private System.Windows.Forms.ListBox listBoxLogging;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBoxSerialPorts;
        private System.Windows.Forms.Label labelBaudrate;
        private System.Windows.Forms.ComboBox comboBoxBaudrate;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.CheckBox CheckBoxLoggingEnabled;
        private System.Windows.Forms.Label labelStopBits;
        private System.Windows.Forms.ComboBox comboBoxStopBits;
        private System.Windows.Forms.Label labelParity;
        private System.Windows.Forms.ComboBox comboBoxParity;
        private System.Windows.Forms.Label labelDataBits;
        private System.Windows.Forms.ComboBox comboBoxDataBits;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripListBoxLogging;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemClear;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSave;
    }
}