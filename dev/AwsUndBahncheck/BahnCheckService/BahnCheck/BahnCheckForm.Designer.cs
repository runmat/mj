namespace BahnCheck
{
    partial class BahnCheckForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lx = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lx
            // 
            this.lx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lx.ForeColor = System.Drawing.Color.Blue;
            this.lx.Location = new System.Drawing.Point(12, 15);
            this.lx.Name = "lx";
            this.lx.Size = new System.Drawing.Size(334, 23);
            this.lx.TabIndex = 0;
            this.lx.Text = "label1";
            this.lx.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BahnCheckForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 41);
            this.Controls.Add(this.lx);
            this.Name = "BahnCheckForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BahnCheckForm";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lx;
    }
}