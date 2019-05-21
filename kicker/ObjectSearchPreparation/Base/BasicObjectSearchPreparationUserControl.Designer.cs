namespace ObjectSearchPreparation.Base
{
    partial class BasicObjectSearchPreparationUserControl
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
            this.panelModuleSpecificControls = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelModuleSpecificControls
            // 
            this.panelModuleSpecificControls.AutoScroll = true;
            this.panelModuleSpecificControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelModuleSpecificControls.Location = new System.Drawing.Point(0, 0);
            this.panelModuleSpecificControls.Name = "panelModuleSpecificControls";
            this.panelModuleSpecificControls.Size = new System.Drawing.Size(161, 140);
            this.panelModuleSpecificControls.TabIndex = 4;
            // 
            // BasicImageBinarizationUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.panelModuleSpecificControls);
            this.Name = "BasicImageBinarizationUserControl";
            this.Size = new System.Drawing.Size(161, 140);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelModuleSpecificControls;
    }
}
