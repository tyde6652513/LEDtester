namespace MPI.Tester.Gui.UIForm.UserForm.UISetting
{
    partial class frmWAVETEK00Path
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
            this.pgdPath = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // pgdPath
            // 
            this.pgdPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgdPath.HelpVisible = false;
            this.pgdPath.Location = new System.Drawing.Point(0, 0);
            this.pgdPath.Name = "pgdPath";
            this.pgdPath.Size = new System.Drawing.Size(412, 364);
            this.pgdPath.TabIndex = 0;
            // 
            // frmWAVETEK00Path
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(412, 364);
            this.Controls.Add(this.pgdPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmWAVETEK00Path";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmWAVETEK00Path";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid pgdPath;
    }
}