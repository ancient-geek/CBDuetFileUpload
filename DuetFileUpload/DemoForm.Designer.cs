namespace CamBamPlugIn
{
    partial class DemoForm
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
            this.lbl_info = new System.Windows.Forms.Label();
            this.lbl_info2 = new System.Windows.Forms.Label();
            this.lbl_units = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_info
            // 
            this.lbl_info.AutoSize = true;
            this.lbl_info.Location = new System.Drawing.Point(110, 29);
            this.lbl_info.Name = "lbl_info";
            this.lbl_info.Size = new System.Drawing.Size(289, 13);
            this.lbl_info.TabIndex = 1;
            this.lbl_info.Text = "Well... That\'s just about the minimum reqirement for a plugin.";
            // 
            // lbl_info2
            // 
            this.lbl_info2.AutoSize = true;
            this.lbl_info2.Location = new System.Drawing.Point(110, 51);
            this.lbl_info2.Name = "lbl_info2";
            this.lbl_info2.Size = new System.Drawing.Size(190, 13);
            this.lbl_info2.TabIndex = 2;
            this.lbl_info2.Text = "Your GUI would appear in this window.";
            // 
            // lbl_units
            // 
            this.lbl_units.AutoSize = true;
            this.lbl_units.Location = new System.Drawing.Point(110, 83);
            this.lbl_units.Name = "lbl_units";
            this.lbl_units.Size = new System.Drawing.Size(34, 13);
            this.lbl_units.TabIndex = 3;
            this.lbl_units.Text = " Units";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::CamBamPlugIn.Properties.Resources.beano;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(96, 96);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // DemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 120);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lbl_units);
            this.Controls.Add(this.lbl_info2);
            this.Controls.Add(this.lbl_info);
            this.Name = "DemoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Demo Plugin";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_info;
        private System.Windows.Forms.Label lbl_info2;
        private System.Windows.Forms.Label lbl_units;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}