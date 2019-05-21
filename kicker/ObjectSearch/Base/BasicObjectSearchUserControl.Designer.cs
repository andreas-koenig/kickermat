namespace ObjectSearch.Base
{
    partial class BasicObjectSearchUserControl
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
            this.tabControlObjectSearch = new System.Windows.Forms.TabControl();
            this.tabPageObjectSearch = new System.Windows.Forms.TabPage();
            this.tabPagePreparation = new System.Windows.Forms.TabPage();
            this.tabControlObjectSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlObjectSearch
            // 
            this.tabControlObjectSearch.Controls.Add(this.tabPageObjectSearch);
            this.tabControlObjectSearch.Controls.Add(this.tabPagePreparation);
            this.tabControlObjectSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlObjectSearch.Location = new System.Drawing.Point(0, 0);
            this.tabControlObjectSearch.Name = "tabControlObjectSearch";
            this.tabControlObjectSearch.SelectedIndex = 0;
            this.tabControlObjectSearch.Size = new System.Drawing.Size(545, 461);
            this.tabControlObjectSearch.TabIndex = 0;
            // 
            // tabPageObjectSearch
            // 
            this.tabPageObjectSearch.Location = new System.Drawing.Point(4, 22);
            this.tabPageObjectSearch.Name = "tabPageObjectSearch";
            this.tabPageObjectSearch.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageObjectSearch.Size = new System.Drawing.Size(537, 435);
            this.tabPageObjectSearch.TabIndex = 0;
            this.tabPageObjectSearch.Text = "Object Search";
            this.tabPageObjectSearch.UseVisualStyleBackColor = true;
            // 
            // tabPagePreparation
            // 
            this.tabPagePreparation.Location = new System.Drawing.Point(4, 22);
            this.tabPagePreparation.Name = "tabPagePreparation";
            this.tabPagePreparation.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePreparation.Size = new System.Drawing.Size(537, 435);
            this.tabPagePreparation.TabIndex = 1;
            this.tabPagePreparation.Text = "Preparation";
            this.tabPagePreparation.UseVisualStyleBackColor = true;
            // 
            // BasicObjectSearchUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.tabControlObjectSearch);
            this.Name = "BasicObjectSearchUserControl";
            this.Size = new System.Drawing.Size(545, 461);
            this.tabControlObjectSearch.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlObjectSearch;
        private System.Windows.Forms.TabPage tabPageObjectSearch;
        private System.Windows.Forms.TabPage tabPagePreparation;

    }
}
