namespace MPI.Tester.Gui.UIForm.Dialog.HardWareSetting.LaserSource
{
    partial class frmOpticalSwitchComp
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btnSwitchThisCh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.richTextBox1.Location = new System.Drawing.Point(0, 63);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(250, 211);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
            // 
            // btnSwitchThisCh
            // 
            this.btnSwitchThisCh.Font = new System.Drawing.Font("PMingLiU", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSwitchThisCh.ForeColor = System.Drawing.Color.Black;
            this.btnSwitchThisCh.Location = new System.Drawing.Point(26, 1);
            this.btnSwitchThisCh.Name = "btnSwitchThisCh";
            this.btnSwitchThisCh.Size = new System.Drawing.Size(198, 31);
            this.btnSwitchThisCh.TabIndex = 6;
            this.btnSwitchThisCh.Text = "Switch to this channel";
            this.btnSwitchThisCh.UseVisualStyleBackColor = true;
            this.btnSwitchThisCh.Click += new System.EventHandler(this.btnSwitchThisCh_Click);
            // 
            // frmOpticalSwitchComp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(250, 274);
            this.Controls.Add(this.btnSwitchThisCh);
            this.Controls.Add(this.richTextBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmOpticalSwitchComp";
            this.Text = "frmOpticalSwitch";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btnSwitchThisCh;
    }
}