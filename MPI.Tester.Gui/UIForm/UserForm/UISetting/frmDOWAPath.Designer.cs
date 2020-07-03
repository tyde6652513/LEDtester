namespace MPI.Tester.Gui.UIForm.UserForm.UISetting
{
    partial class frmDowaPath
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
            MPI.Tester.GuiComponent.PathInfo pathInfo4 = new MPI.Tester.GuiComponent.PathInfo();
            MPI.Tester.GuiComponent.PathInfo pathInfo5 = new MPI.Tester.GuiComponent.PathInfo();
            MPI.Tester.GuiComponent.PathInfo pathInfo6 = new MPI.Tester.GuiComponent.PathInfo();
            this.pathUIComponent1 = new MPI.Tester.GuiComponent.PathUIComponent();
            this.pnlBinMapPath = new System.Windows.Forms.Panel();
            this.lblFileNamePresentTitle = new DevComponents.DotNetBar.LabelX();
            this.cmbFileNamePresent = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.pucLaserPower = new MPI.Tester.GuiComponent.PathUIComponent();
            this.btnMerge = new System.Windows.Forms.Button();
            this.pucMergeFilePath = new MPI.Tester.GuiComponent.PathUIComponent();
            this.pnlBinMapPath.SuspendLayout();
            this.SuspendLayout();
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
            pathInfo4.EnablePath = true;
            pathInfo4.FileExt = "csv";
            pathInfo4.FolderType = MPI.Tester.GuiComponent.ETesterResultCreatFolderType.None;
            pathInfo4.PathName = "Binマップ保存位置";
            pathInfo4.TestResultPath = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.pathUIComponent1.PathInfomation = pathInfo4;
            this.pathUIComponent1.PathName = "Binマップ保存位置";
            this.pathUIComponent1.Size = new System.Drawing.Size(700, 24);
            this.pathUIComponent1.TabIndex = 0;
            // 
            // pnlBinMapPath
            // 
            this.pnlBinMapPath.Controls.Add(this.lblFileNamePresentTitle);
            this.pnlBinMapPath.Controls.Add(this.cmbFileNamePresent);
            this.pnlBinMapPath.Controls.Add(this.pucLaserPower);
            this.pnlBinMapPath.Controls.Add(this.btnMerge);
            this.pnlBinMapPath.Controls.Add(this.pucMergeFilePath);
            this.pnlBinMapPath.Controls.Add(this.pathUIComponent1);
            this.pnlBinMapPath.Location = new System.Drawing.Point(12, 12);
            this.pnlBinMapPath.Name = "pnlBinMapPath";
            this.pnlBinMapPath.Size = new System.Drawing.Size(700, 294);
            this.pnlBinMapPath.TabIndex = 1;
            // 
            // lblFileNamePresentTitle
            // 
            this.lblFileNamePresentTitle.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.lblFileNamePresentTitle.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblFileNamePresentTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblFileNamePresentTitle.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblFileNamePresentTitle.ForeColor = System.Drawing.Color.Black;
            this.lblFileNamePresentTitle.Location = new System.Drawing.Point(0, 89);
            this.lblFileNamePresentTitle.Name = "lblFileNamePresentTitle";
            this.lblFileNamePresentTitle.Size = new System.Drawing.Size(147, 25);
            this.lblFileNamePresentTitle.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.lblFileNamePresentTitle.TabIndex = 459;
            this.lblFileNamePresentTitle.Text = "合成名基準";
            // 
            // cmbFileNamePresent
            // 
            this.cmbFileNamePresent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFileNamePresent.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.cmbFileNamePresent.ForeColor = System.Drawing.Color.Black;
            this.cmbFileNamePresent.Location = new System.Drawing.Point(206, 88);
            this.cmbFileNamePresent.Name = "cmbFileNamePresent";
            this.cmbFileNamePresent.Size = new System.Drawing.Size(271, 26);
            this.cmbFileNamePresent.TabIndex = 458;
            // 
            // pucLaserPower
            // 
            this.pucLaserPower.BackColor = System.Drawing.Color.Transparent;
            this.pucLaserPower.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pucLaserPower.IsChkEnableEditable = true;
            this.pucLaserPower.IsEnableCheckEn = true;
            this.pucLaserPower.IsShowType = true;
            this.pucLaserPower.Location = new System.Drawing.Point(0, 135);
            this.pucLaserPower.Name = "pucLaserPower";
            pathInfo5.EnablePath = true;
            pathInfo5.FileExt = "csv";
            pathInfo5.FolderType = MPI.Tester.GuiComponent.ETesterResultCreatFolderType.None;
            pathInfo5.PathName = "レーザデータ位置";
            pathInfo5.TestResultPath = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.pucLaserPower.PathInfomation = pathInfo5;
            this.pucLaserPower.PathName = "レーザデータ位置";
            this.pucLaserPower.Size = new System.Drawing.Size(700, 24);
            this.pucLaserPower.TabIndex = 457;
            // 
            // btnMerge
            // 
            this.btnMerge.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMerge.Location = new System.Drawing.Point(588, 84);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(108, 32);
            this.btnMerge.TabIndex = 2;
            this.btnMerge.Text = "Merge";
            this.btnMerge.UseVisualStyleBackColor = true;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // pucMergeFilePath
            // 
            this.pucMergeFilePath.BackColor = System.Drawing.Color.Transparent;
            this.pucMergeFilePath.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pucMergeFilePath.IsChkEnableEditable = true;
            this.pucMergeFilePath.IsEnableCheckEn = true;
            this.pucMergeFilePath.IsShowType = true;
            this.pucMergeFilePath.Location = new System.Drawing.Point(0, 52);
            this.pucMergeFilePath.Name = "pucMergeFilePath";
            pathInfo6.EnablePath = true;
            pathInfo6.FileExt = "csv";
            pathInfo6.FolderType = MPI.Tester.GuiComponent.ETesterResultCreatFolderType.None;
            pathInfo6.PathName = "合成結果位置";
            pathInfo6.TestResultPath = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.pucMergeFilePath.PathInfomation = pathInfo6;
            this.pucMergeFilePath.PathName = "合成結果位置";
            this.pucMergeFilePath.Size = new System.Drawing.Size(700, 24);
            this.pucMergeFilePath.TabIndex = 1;
            // 
            // frmDowaPath
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(734, 318);
            this.Controls.Add(this.pnlBinMapPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmDowaPath";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmWAVETEK00Path";
            this.pnlBinMapPath.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GuiComponent.PathUIComponent pathUIComponent1;
        private System.Windows.Forms.Panel pnlBinMapPath;
        private GuiComponent.PathUIComponent pucMergeFilePath;
        private System.Windows.Forms.Button btnMerge;
        private GuiComponent.PathUIComponent pucLaserPower;
        private DevComponents.DotNetBar.LabelX lblFileNamePresentTitle;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbFileNamePresent;

    }
}