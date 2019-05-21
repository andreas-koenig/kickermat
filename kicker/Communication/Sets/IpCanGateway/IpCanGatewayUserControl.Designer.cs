namespace Communication.Sets.IpCanGateway
{
    partial class IpCanGatewayUserControl
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
            this.groupBoxGatewaySettings = new System.Windows.Forms.GroupBox();
            this.propertyGridSettings = new System.Windows.Forms.PropertyGrid();
            this.groupBoxGatewaySettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxGatewaySettings
            // 
            this.groupBoxGatewaySettings.Controls.Add(this.propertyGridSettings);
            this.groupBoxGatewaySettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxGatewaySettings.Location = new System.Drawing.Point(0, 0);
            this.groupBoxGatewaySettings.Name = "groupBoxGatewaySettings";
            this.groupBoxGatewaySettings.Size = new System.Drawing.Size(750, 371);
            this.groupBoxGatewaySettings.TabIndex = 0;
            this.groupBoxGatewaySettings.TabStop = false;
            this.groupBoxGatewaySettings.Text = "Gateway Settings";
            // 
            // propertyGridSettings
            // 
            this.propertyGridSettings.Dock = System.Windows.Forms.DockStyle.Left;
            this.propertyGridSettings.Location = new System.Drawing.Point(3, 16);
            this.propertyGridSettings.Name = "propertyGridSettings";
            this.propertyGridSettings.Size = new System.Drawing.Size(271, 352);
            this.propertyGridSettings.TabIndex = 0;
            // 
            // IpCanGatewayUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxGatewaySettings);
            this.Name = "IpCanGatewayUserControl";
            this.Size = new System.Drawing.Size(750, 371);
            this.groupBoxGatewaySettings.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxGatewaySettings;
        private System.Windows.Forms.PropertyGrid propertyGridSettings;
    }
}