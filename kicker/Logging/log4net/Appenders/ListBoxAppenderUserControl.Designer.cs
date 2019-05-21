namespace Logging.log4net.Appenders
{
    partial class ListBoxAppenderUserControl
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
            this.listBoxLogMessages = new System.Windows.Forms.ListBox();
            this.contextMenuStripClear = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemClear = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripClear.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxLogMessages
            // 
            this.listBoxLogMessages.ContextMenuStrip = this.contextMenuStripClear;
            this.listBoxLogMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxLogMessages.FormattingEnabled = true;
            this.listBoxLogMessages.HorizontalScrollbar = true;
            this.listBoxLogMessages.Location = new System.Drawing.Point(0, 0);
            this.listBoxLogMessages.Name = "listBoxLogMessages";
            this.listBoxLogMessages.Size = new System.Drawing.Size(842, 524);
            this.listBoxLogMessages.TabIndex = 0;
            // 
            // contextMenuStripClear
            // 
            this.contextMenuStripClear.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemClear});
            this.contextMenuStripClear.Name = "contextMenuStripClear";
            this.contextMenuStripClear.Size = new System.Drawing.Size(102, 26);
            // 
            // toolStripMenuItemClear
            // 
            this.toolStripMenuItemClear.Name = "toolStripMenuItemClear";
            this.toolStripMenuItemClear.Size = new System.Drawing.Size(101, 22);
            this.toolStripMenuItemClear.Text = "Clear";
            this.toolStripMenuItemClear.Click += new System.EventHandler(this.ToolStripMenuItemClear_Click);
            // 
            // ListBoxAppenderUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listBoxLogMessages);
            this.Name = "ListBoxAppenderUserControl";
            this.Size = new System.Drawing.Size(842, 533);
            this.contextMenuStripClear.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxLogMessages;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripClear;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemClear;
    }
}
