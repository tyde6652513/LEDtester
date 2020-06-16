namespace MPI.Tester.Gui.UIForm.UserForm.UISetting
{
    partial class frmOptoTechPath
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
            this.btnKeyInData = new DevComponents.DotNetBar.ButtonX();
            this.txtKeyInDataPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblFormatTitle = new DevComponents.DotNetBar.LabelX();
            this.btnMerge = new System.Windows.Forms.Button();
            this.pathUIComponent1 = new MPI.Tester.GuiComponent.PathUIComponent();
            this.pucLaserPower = new MPI.Tester.GuiComponent.PathUIComponent();
            this.pnlMergeFilePath.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMergeFilePath
            // 
            this.pnlMergeFilePath.Controls.Add(this.pucLaserPower);
            this.pnlMergeFilePath.Controls.Add(this.btnKeyInData);
            this.pnlMergeFilePath.Controls.Add(this.txtKeyInDataPath);
            this.pnlMergeFilePath.Controls.Add(this.lblFormatTitle);
            this.pnlMergeFilePath.Controls.Add(this.btnMerge);
            this.pnlMergeFilePath.Controls.Add(this.pathUIComponent1);
            this.pnlMergeFilePath.Location = new System.Drawing.Point(17, 12);
            this.pnlMergeFilePath.Name = "pnlMergeFilePath";
            this.pnlMergeFilePath.Size = new System.Drawing.Size(700, 294);
            this.pnlMergeFilePath.TabIndex = 2;
            // 
            // btnKeyInData
            // 
            this.btnKeyInData.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnKeyInData.AntiAlias = true;
            this.btnKeyInData.BackColor = System.Drawing.Color.Transparent;
            this.btnKeyInData.ColorTable = DevComponents.DotNetBar.eButtonColor.Office2007WithBackground;
            this.btnKeyInData.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnKeyInData.Location = new System.Drawing.Point(532, 116);
            this.btnKeyInData.Name = "btnKeyInData";
            this.btnKeyInData.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(4);
            this.btnKeyInData.Size = new System.Drawing.Size(25, 25);
            this.btnKeyInData.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnKeyInData.TabIndex = 455;
            this.btnKeyInData.Tag = "4";
            this.btnKeyInData.Text = "...";
            this.btnKeyInData.Click += new System.EventHandler(this.btnKeyInData_Click);
            // 
            // txtKeyInDataPath
            // 
            this.txtKeyInDataPath.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.txtKeyInDataPath.Border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtKeyInDataPath.Border.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtKeyInDataPath.Border.BorderBottomWidth = 1;
            this.txtKeyInDataPath.Border.BorderColor = System.Drawing.Color.DarkSalmon;
            this.txtKeyInDataPath.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtKeyInDataPath.Border.BorderLeftWidth = 1;
            this.txtKeyInDataPath.Border.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtKeyInDataPath.Border.BorderRightWidth = 1;
            this.txtKeyInDataPath.Border.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtKeyInDataPath.Border.BorderTopWidth = 1;
            this.txtKeyInDataPath.Border.Class = "";
            this.txtKeyInDataPath.Border.CornerDiameter = 4;
            this.txtKeyInDataPath.Border.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.txtKeyInDataPath.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.txtKeyInDataPath.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtKeyInDataPath.Location = new System.Drawing.Point(205, 116);
            this.txtKeyInDataPath.Name = "txtKeyInDataPath";
            this.txtKeyInDataPath.Size = new System.Drawing.Size(321, 24);
            this.txtKeyInDataPath.TabIndex = 454;
            this.txtKeyInDataPath.Tag = "8";
            this.txtKeyInDataPath.Text = "KeyInDataPath";
            // 
            // lblFormatTitle
            // 
            this.lblFormatTitle.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.lblFormatTitle.BackgroundStyle.Class = "";
            this.lblFormatTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblFormatTitle.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblFormatTitle.ForeColor = System.Drawing.Color.Black;
            this.lblFormatTitle.Location = new System.Drawing.Point(3, 117);
            this.lblFormatTitle.Name = "lblFormatTitle";
            this.lblFormatTitle.Size = new System.Drawing.Size(132, 25);
            this.lblFormatTitle.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.lblFormatTitle.TabIndex = 301;
            this.lblFormatTitle.Text = "Key In Data Path";
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
            pathInfo2.TestResultPath = "";
            this.pathUIComponent1.PathInfomation = pathInfo2;
            this.pathUIComponent1.PathName = "Merge File Path";
            this.pathUIComponent1.Size = new System.Drawing.Size(700, 24);
            this.pathUIComponent1.TabIndex = 0;
            // 
            // pucLaserPower
            // 
            this.pucLaserPower.BackColor = System.Drawing.Color.Transparent;
            this.pucLaserPower.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pucLaserPower.IsChkEnableEditable = true;
            this.pucLaserPower.IsEnableCheckEn = true;
            this.pucLaserPower.IsShowType = true;
            this.pucLaserPower.Location = new System.Drawing.Point(0, 178);
            this.pucLaserPower.Name = "pucLaserPower";
            pathInfo1.EnablePath = true;
            pathInfo1.FileExt = "csv";
            pathInfo1.FolderType = MPI.Tester.GuiComponent.ETesterResultCreatFolderType.None;
            pathInfo1.PathName = "Laser Log Data";
            pathInfo1.TestResultPath = "";
            this.pucLaserPower.PathInfomation = pathInfo1;
            this.pucLaserPower.PathName = "Laser Log Data";
            this.pucLaserPower.Size = new System.Drawing.Size(700, 24);
            this.pucLaserPower.TabIndex = 456;
            // 
            // frmOptoTechPath
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(734, 318);
            this.Controls.Add(this.pnlMergeFilePath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmOptoTechPath";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmOptoTechPath";
            this.pnlMergeFilePath.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMergeFilePath;
        private GuiComponent.PathUIComponent pathUIComponent1;
        private System.Windows.Forms.Button btnMerge;
        private DevComponents.DotNetBar.LabelX lblFormatTitle;
        private DevComponents.DotNetBar.Controls.TextBoxX txtKeyInDataPath;
        private DevComponents.DotNetBar.ButtonX btnKeyInData;
        private GuiComponent.PathUIComponent pucLaserPower;
    }
}