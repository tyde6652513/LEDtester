namespace MPI.Tester.Gui
{
    partial class frmViewWLGraph
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
            this.btnLoad = new System.Windows.Forms.Button();
            this.listBoxFile = new System.Windows.Forms.ListBox();
            this.pnlWLGraph = new System.Windows.Forms.Panel();
            this.lblSourceFileName = new System.Windows.Forms.Label();
            this.lblSourceFilePath = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnLoad
            // 
            this.btnLoad.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoad.Location = new System.Drawing.Point(12, 9);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(131, 31);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // listBoxFile
            // 
            this.listBoxFile.FormattingEnabled = true;
            this.listBoxFile.ItemHeight = 12;
            this.listBoxFile.Location = new System.Drawing.Point(12, 46);
            this.listBoxFile.Name = "listBoxFile";
            this.listBoxFile.Size = new System.Drawing.Size(131, 364);
            this.listBoxFile.TabIndex = 1;
            this.listBoxFile.SelectedIndexChanged += new System.EventHandler(this.listBoxFile_SelectedIndexChanged);
            // 
            // pnlWLGraph
            // 
            this.pnlWLGraph.Font = new System.Drawing.Font("Bookshelf Symbol 7", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.pnlWLGraph.Location = new System.Drawing.Point(149, 12);
            this.pnlWLGraph.Name = "pnlWLGraph";
            this.pnlWLGraph.Size = new System.Drawing.Size(694, 338);
            this.pnlWLGraph.TabIndex = 2;
            // 
            // lblSourceFileName
            // 
            this.lblSourceFileName.AutoSize = true;
            this.lblSourceFileName.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lblSourceFileName.Location = new System.Drawing.Point(173, 394);
            this.lblSourceFileName.Name = "lblSourceFileName";
            this.lblSourceFileName.Size = new System.Drawing.Size(116, 16);
            this.lblSourceFileName.TabIndex = 21;
            this.lblSourceFileName.Text = "Source File Name:";
            // 
            // lblSourceFilePath
            // 
            this.lblSourceFilePath.AutoSize = true;
            this.lblSourceFilePath.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lblSourceFilePath.Location = new System.Drawing.Point(173, 367);
            this.lblSourceFilePath.Name = "lblSourceFilePath";
            this.lblSourceFilePath.Size = new System.Drawing.Size(109, 16);
            this.lblSourceFilePath.TabIndex = 22;
            this.lblSourceFilePath.Text = "Source File Path:";
            // 
            // frmViewWLGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::MPI.Tester.Gui.Properties.Resources.mini_hero_bg;
            this.ClientSize = new System.Drawing.Size(859, 422);
            this.Controls.Add(this.lblSourceFileName);
            this.Controls.Add(this.lblSourceFilePath);
            this.Controls.Add(this.pnlWLGraph);
            this.Controls.Add(this.listBoxFile);
            this.Controls.Add(this.btnLoad);
            this.Name = "frmViewWLGraph";
            this.Text = "View Wavelength";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.ListBox listBoxFile;
        private System.Windows.Forms.Panel pnlWLGraph;
        private System.Windows.Forms.Label lblSourceFileName;
        private System.Windows.Forms.Label lblSourceFilePath;
    }
}