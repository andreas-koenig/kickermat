namespace PluginSystem
{
    /// <summary>
    /// User control which can control plugins.
    /// </summary>    
    sealed partial class PluginSystemUserControl<TPlugin>
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
            
            if (this.PluginInstance != null)
            {
                Plugger.DestroyInstance<TPlugin>(this.PluginInstance);
                this.PluginInstance = null;
            }
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanelBallDetection = new System.Windows.Forms.TableLayoutPanel();
            this.panelPlugin = new System.Windows.Forms.Panel();
            this.ComboBoxPluginTypes = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanelBallDetection.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelBallDetection
            // 
            this.tableLayoutPanelBallDetection.ColumnCount = 1;
            this.tableLayoutPanelBallDetection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBallDetection.Controls.Add(this.panelPlugin, 0, 1);
            this.tableLayoutPanelBallDetection.Controls.Add(this.ComboBoxPluginTypes, 0, 0);
            this.tableLayoutPanelBallDetection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelBallDetection.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelBallDetection.Name = "tableLayoutPanelBallDetection";
            this.tableLayoutPanelBallDetection.RowCount = 2;
            this.tableLayoutPanelBallDetection.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanelBallDetection.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBallDetection.Size = new System.Drawing.Size(788, 134);
            this.tableLayoutPanelBallDetection.TabIndex = 53;
            // 
            // panelPlugin
            // 
            this.panelPlugin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPlugin.Location = new System.Drawing.Point(3, 30);
            this.panelPlugin.Name = "panelPlugin";
            this.panelPlugin.Size = new System.Drawing.Size(782, 101);
            this.panelPlugin.TabIndex = 51;
            // 
            // ComboBoxPluginTypes
            // 
            this.ComboBoxPluginTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxPluginTypes.FormattingEnabled = true;
            this.ComboBoxPluginTypes.Location = new System.Drawing.Point(3, 3);
            this.ComboBoxPluginTypes.Name = "ComboBoxPluginTypes";
            this.ComboBoxPluginTypes.Size = new System.Drawing.Size(476, 21);
            this.ComboBoxPluginTypes.Sorted = true;
            this.ComboBoxPluginTypes.TabIndex = 50;
            this.ComboBoxPluginTypes.SelectedIndexChanged += new System.EventHandler(this.ComboBoxPluginTypes_SelectedIndexChanged);
            // 
            // PluginSystemUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelBallDetection);
            this.Name = "PluginSystemUserControl";
            this.Size = new System.Drawing.Size(788, 134);
            this.tableLayoutPanelBallDetection.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBallDetection;
        private System.Windows.Forms.Panel panelPlugin;
        private System.Windows.Forms.ComboBox ComboBoxPluginTypes;
    }
}