namespace MPI.Tester.Gui
{
    partial class frmViewPIVGraph
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
            this.pnlPIVGraph = new System.Windows.Forms.Panel();
            this.listBoxFile = new System.Windows.Forms.ListBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.lblSourceFileName = new System.Windows.Forms.Label();
            this.lblSourceFilePath = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pnlPIVGraph
            // 
            this.pnlPIVGraph.Location = new System.Drawing.Point(149, 12);
            this.pnlPIVGraph.Name = "pnlPIVGraph";
            this.pnlPIVGraph.Size = new System.Drawing.Size(748, 342);
            this.pnlPIVGraph.TabIndex = 0;
            // 
            // listBoxFile
            // 
            this.listBoxFile.FormattingEnabled = true;
            this.listBoxFile.ItemHeight = 12;
            this.listBoxFile.Location = new System.Drawing.Point(12, 46);
            this.listBoxFile.Name = "listBoxFile";
            this.listBoxFile.Size = new System.Drawing.Size(131, 364);
            this.listBoxFile.TabIndex = 2;
            this.listBoxFile.SelectedIndexChanged += new System.EventHandler(this.listBoxFile_SelectedIndexChanged);
            // 
            // btnLoad
            // 
            this.btnLoad.Font = new System.Drawing.Font("Arial", 9.75F);
            this.btnLoad.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnLoad.Location = new System.Drawing.Point(12, 9);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(131, 31);
            this.btnLoad.TabIndex = 18;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // lblSourceFileName
            // 
            this.lblSourceFileName.AutoSize = true;
            this.lblSourceFileName.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lblSourceFileName.Location = new System.Drawing.Point(173, 394);
            this.lblSourceFileName.Name = "lblSourceFileName";
            this.lblSourceFileName.Size = new System.Drawing.Size(116, 16);
            this.lblSourceFileName.TabIndex = 19;
            this.lblSourceFileName.Text = "Source File Name:";
            // 
            // lblSourceFilePath
            // 
            this.lblSourceFilePath.AutoSize = true;
            this.lblSourceFilePath.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lblSourceFilePath.Location = new System.Drawing.Point(173, 367);
            this.lblSourceFilePath.Name = "lblSourceFilePath";
            this.lblSourceFilePath.Size = new System.Drawing.Size(109, 16);
            this.lblSourceFilePath.TabIndex = 20;
            this.lblSourceFilePath.Text = "Source File Path:";
            // 
            // frmViewPIVGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::MPI.Tester.Gui.Properties.Resources.mini_hero_bg;
            this.ClientSize = new System.Drawing.Size(909, 422);
            this.Controls.Add(this.lblSourceFileName);
            this.Controls.Add(this.lblSourceFilePath);
            this.Controls.Add(this.listBoxFile);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.pnlPIVGraph);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmViewPIVGraph";
            this.Text = "View PIV Curve";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlPIVGraph;
        private System.Windows.Forms.ListBox listBoxFile;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label lblSourceFileName;
        private System.Windows.Forms.Label lblSourceFilePath;
    }
}