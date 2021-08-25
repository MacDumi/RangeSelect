namespace RangeSelect
{
    partial class SelectionRangeSlider
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelLabels = new System.Windows.Forms.Panel();
            this.labelMax = new System.Windows.Forms.Label();
            this.labelMin = new System.Windows.Forms.Label();
            this.panelBG = new System.Windows.Forms.Panel();
            this.panelLabels.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelLabels
            // 
            this.panelLabels.BackColor = System.Drawing.Color.White;
            this.panelLabels.Controls.Add(this.labelMax);
            this.panelLabels.Controls.Add(this.labelMin);
            this.panelLabels.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelLabels.Location = new System.Drawing.Point(0, 188);
            this.panelLabels.Name = "panelLabels";
            this.panelLabels.Size = new System.Drawing.Size(376, 17);
            this.panelLabels.TabIndex = 2;
            // 
            // labelMax
            // 
            this.labelMax.AutoSize = true;
            this.labelMax.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelMax.ForeColor = System.Drawing.Color.Black;
            this.labelMax.Location = new System.Drawing.Point(341, 0);
            this.labelMax.Name = "labelMax";
            this.labelMax.Size = new System.Drawing.Size(35, 13);
            this.labelMax.TabIndex = 2;
            this.labelMax.Text = "label1";
            // 
            // labelMin
            // 
            this.labelMin.AutoSize = true;
            this.labelMin.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelMin.ForeColor = System.Drawing.Color.Black;
            this.labelMin.Location = new System.Drawing.Point(0, 0);
            this.labelMin.Name = "labelMin";
            this.labelMin.Size = new System.Drawing.Size(35, 13);
            this.labelMin.TabIndex = 1;
            this.labelMin.Text = "label1";
            // 
            // panelBG
            // 
            this.panelBG.BackColor = System.Drawing.Color.Transparent;
            this.panelBG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBG.Location = new System.Drawing.Point(0, 0);
            this.panelBG.Name = "panelBG";
            this.panelBG.Size = new System.Drawing.Size(376, 188);
            this.panelBG.TabIndex = 3;
            // 
            // SelectionRangeSlider
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panelBG);
            this.Controls.Add(this.panelLabels);
            this.Name = "SelectionRangeSlider";
            this.Size = new System.Drawing.Size(376, 205);
            this.panelLabels.ResumeLayout(false);
            this.panelLabels.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelLabels;
        private System.Windows.Forms.Panel panelBG;
        private System.Windows.Forms.Label labelMax;
        private System.Windows.Forms.Label labelMin;
    }
}
