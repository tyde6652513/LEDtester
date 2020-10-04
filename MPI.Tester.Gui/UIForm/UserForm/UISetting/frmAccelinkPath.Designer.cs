namespace MPI.Tester.Gui.UIForm.UserForm.UISetting
{
    partial class frmAccelinkPath
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
            MPI.Tester.GuiComponent.PathInfo pathInfo2 = new MPI.Tester.GuiComponent.PathInfo();
            MPI.Tester.GuiComponent.PathInfo pathInfo1 = new MPI.Tester.GuiComponent.PathInfo();
            this.pnlMergeFilePath = new System.Windows.Forms.Panel();
            this.btnMerge = new System.Windows.Forms.Button();
            this.pathUIComponent1 = new MPI.Tester.GuiComponent.PathUIComponent();
            this.pucWaferMap = new MPI.Tester.GuiComponent.PathUIComponent();
            this.pnlMergeFilePath.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMergeFilePath
            // 
            this.pnlMergeFilePath.Controls.Add(this.pucWaferMap);
            this.pnlMergeFilePath.Controls.Add(this.btnMerge);
            this.pnlMergeFilePath.Controls.Add(this.pathUIComponent1);
            this.pnlMergeFilePath.Location = new System.Drawing.Point(17, 12);
            this.pnlMergeFilePath.Name = "pnlMergeFilePath";
            this.pnlMergeFilePath.Size = new System.Drawing.Size(700, 294);
            this.pnlMergeFilePath.TabIndex = 2;
            // 
            // btnMerge
            // 
            this.btnMerge.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMerge.Location = new System.Drawing.Point(576, 55);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(108, 32);
            this.btnMerge.TabIndex = 1;
            this.btnMerge.Text = "Merge";
            this.btnMerge.UseVisualStyleBackColor = true;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // pathUIComponent1
            // 
            this.pathUIComponent1.BackColor = System.Drawing.Color.Transparent;
            this.pathUIComponent1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pathUIComponent1.IsChkEnableEditable = true;
            this.pathUIComponent1.IsEnableCheckEn = true;
            this.pathUIComponent1.IsShowType = true;
            this.pathUIComponent1.Location = new System.Drawing.Point(0, 3);
            this.pathUIComponent1.Name = "pathUIComponent1";
            pathInfo2.EnablePath = true;
            pathInfo2.FileExt = "csv";
            pathInfo2.FolderType = MPI.Tester.GuiComponent.ETesterResultCreatFolderType.None;
            pathInfo2.PathName = "Merge File Path";
            pathInfo2.TestResultPath = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.pathUIComponent1.PathInfomation = pathInfo2;
            this.pathUIComponent1.PathName = "Merge File Path";
            this.pathUIComponent1.Size = new System.Drawing.Size(700, 24);
            this.pathUIComponent1.TabIndex = 0;
            // 
            // pucWaferMap
            // 
            this.pucWaferMap.BackColor = System.Drawing.Color.Transparent;
            this.pucWaferMap.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pucWaferMap.IsChkEnableEditable = true;
            this.pucWaferMap.IsEnableCheckEn = true;
            this.pucWaferMap.IsShowType = true;
            this.pucWaferMap.Location = new System.Drawing.Point(0, 106);
            this.pucWaferMap.Name = "pucWaferMap";
            pathInfo1.EnablePath = true;
            pathInfo1.FileExt = "csv";
            pathInfo1.FolderType = MPI.Tester.GuiComponent.ETesterResultCreatFolderType.None;
            pathInfo1.PathName = "WaferMap Path";
            pathInfo1.TestResultPath = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.pucWaferMap.PathInfomation = pathInfo1;
            this.pucWaferMap.PathName = "WaferMap Path";
            this.pucWaferMap.Size = new System.Drawing.Size(700, 24);
            this.pucWaferMap.TabIndex = 2;
            // 
            // frmAccelinkPath
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(734, 318);
            this.Controls.Add(this.pnlMergeFilePath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmAccelinkPath";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmOptoTechPath";
            this.pnlMergeFilePath.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMergeFilePath;
        private GuiComponent.PathUIComponent pathUIComponent1;
        private System.Windows.Forms.Button btnMerge;
        private GuiComponent.PathUIComponent pucWaferMap;
    }
}