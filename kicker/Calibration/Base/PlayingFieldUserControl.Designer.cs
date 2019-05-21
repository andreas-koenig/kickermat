namespace Calibration.Base
{
    partial class PlayingFieldUserControl
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
            this.tableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.panelLeftSide = new System.Windows.Forms.Panel();
            this.pictureBoxPlayingField = new System.Windows.Forms.PictureBox();
            this.panelRightSide = new System.Windows.Forms.Panel();
            this.propertyGridPlayingField = new System.Windows.Forms.PropertyGrid();
            this.tableLayout.SuspendLayout();
            this.panelLeftSide.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPlayingField)).BeginInit();
            this.panelRightSide.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayout
            // 
            this.tableLayout.ColumnCount = 2;
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 570F));
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout.Controls.Add(this.panelLeftSide, 0, 0);
            this.tableLayout.Controls.Add(this.panelRightSide, 1, 0);
            this.tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayout.Location = new System.Drawing.Point(0, 0);
            this.tableLayout.Name = "tableLayout";
            this.tableLayout.RowCount = 1;
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout.Size = new System.Drawing.Size(880, 400);
            this.tableLayout.TabIndex = 2;
            // 
            // panelLeftSide
            // 
            this.panelLeftSide.Controls.Add(this.pictureBoxPlayingField);
            this.panelLeftSide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLeftSide.Location = new System.Drawing.Point(3, 3);
            this.panelLeftSide.Name = "panelLeftSide";
            this.panelLeftSide.Size = new System.Drawing.Size(564, 394);
            this.panelLeftSide.TabIndex = 2;
            // 
            // pictureBoxPlayingField
            // 
            this.pictureBoxPlayingField.BackColor = System.Drawing.Color.White;
            this.pictureBoxPlayingField.Image = global::Calibration.Resources.PlayingField;
            this.pictureBoxPlayingField.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxPlayingField.Name = "pictureBoxPlayingField";
            this.pictureBoxPlayingField.Size = new System.Drawing.Size(561, 390);
            this.pictureBoxPlayingField.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxPlayingField.TabIndex = 1;
            this.pictureBoxPlayingField.TabStop = false;
            // 
            // panelRightSide
            // 
            this.panelRightSide.Controls.Add(this.propertyGridPlayingField);
            this.panelRightSide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRightSide.Location = new System.Drawing.Point(573, 3);
            this.panelRightSide.Name = "panelRightSide";
            this.panelRightSide.Size = new System.Drawing.Size(304, 394);
            this.panelRightSide.TabIndex = 3;
            // 
            // propertyGridPlayingField
            // 
            this.propertyGridPlayingField.Dock = System.Windows.Forms.DockStyle.Top;
            this.propertyGridPlayingField.Location = new System.Drawing.Point(0, 0);
            this.propertyGridPlayingField.Name = "propertyGridPlayingField";
            this.propertyGridPlayingField.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGridPlayingField.Size = new System.Drawing.Size(304, 394);
            this.propertyGridPlayingField.TabIndex = 2;
            // 
            // PlayingFieldUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayout);
            this.Name = "PlayingFieldUserControl";
            this.Size = new System.Drawing.Size(880, 400);
            this.tableLayout.ResumeLayout(false);
            this.panelLeftSide.ResumeLayout(false);
            this.panelLeftSide.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPlayingField)).EndInit();
            this.panelRightSide.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayout;
        private System.Windows.Forms.Panel panelLeftSide;
        private System.Windows.Forms.PictureBox pictureBoxPlayingField;
        private System.Windows.Forms.Panel panelRightSide;
        private System.Windows.Forms.PropertyGrid propertyGridPlayingField;
    }
}