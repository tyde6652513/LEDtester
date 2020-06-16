namespace MPI.Tester.Gui.UIForm.UIComponent
{
    partial class frmPathInfo
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
            this.lblPathName = new DevComponents.DotNetBar.LabelX();
            this.txtPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnPath = new DevComponents.DotNetBar.ButtonX();
            this.cmbFolderType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.txtExt = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.chkEnablePath = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.SuspendLayout();
            // 
            // lblPathName
            // 
            this.lblPathName.BackColor = System.Drawing.Color.MistyRose;
            // 
            // 
            // 
            this.lblPathName.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.lblPathName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblPathName.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblPathName.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblPathName.ForeColor = System.Drawing.Color.Black;
            this.lblPathName.Location = new System.Drawing.Point(0, 0);
            this.lblPathName.Name = "lblPathName";
            this.lblPathName.Size = new System.Drawing.Size(160, 25);
            this.lblPathName.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.lblPathName.TabIndex = 408;
            this.lblPathName.Text = "Output Path  (1)";
            // 
            // txtPath
            // 
            this.txtPath.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.txtPath.Border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtPath.Border.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtPath.Border.BorderBottomWidth = 1;
            this.txtPath.Border.BorderColor = System.Drawing.Color.DarkSalmon;
            this.txtPath.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtPath.Border.BorderLeftWidth = 1;
            this.txtPath.Border.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtPath.Border.BorderRightWidth = 1;
            this.txtPath.Border.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtPath.Border.BorderTopWidth = 1;
            this.txtPath.Border.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.txtPath.Border.CornerDiameter = 4;
            this.txtPath.Border.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.txtPath.Dock = System.Windows.Forms.DockStyle.Right;
            this.txtPath.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.txtPath.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtPath.Location = new System.Drawing.Point(196, 0);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(260, 24);
            this.txtPath.TabIndex = 409;
            this.txtPath.Tag = "8";
            this.txtPath.Text = "Test Result Path 01";
            // 
            // btnPath
            // 
            this.btnPath.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnPath.AntiAlias = true;
            this.btnPath.BackColor = System.Drawing.Color.Transparent;
            this.btnPath.ColorTable = DevComponents.DotNetBar.eButtonColor.Office2007WithBackground;
            this.btnPath.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPath.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnPath.Location = new System.Drawing.Point(456, 0);
            this.btnPath.Name = "btnPath";
            this.btnPath.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(4);
            this.btnPath.Size = new System.Drawing.Size(25, 25);
            this.btnPath.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnPath.TabIndex = 410;
            this.btnPath.Tag = "4";
            this.btnPath.Text = "...";
            this.btnPath.Click += new System.EventHandler(this.btnPath_Click);
            // 
            // cmbFolderType
            // 
            this.cmbFolderType.Dock = System.Windows.Forms.DockStyle.Right;
            this.cmbFolderType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFolderType.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.cmbFolderType.ForeColor = System.Drawing.Color.Black;
            this.cmbFolderType.Location = new System.Drawing.Point(481, 0);
            this.cmbFolderType.Name = "cmbFolderType";
            this.cmbFolderType.Size = new System.Drawing.Size(130, 26);
            this.cmbFolderType.TabIndex = 411;
            // 
            // txtExt
            // 
            this.txtExt.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.txtExt.Border.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtExt.Border.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtExt.Border.BorderBottomWidth = 1;
            this.txtExt.Border.BorderColor = System.Drawing.Color.DarkSalmon;
            this.txtExt.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtExt.Border.BorderLeftWidth = 1;
            this.txtExt.Border.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtExt.Border.BorderRightWidth = 1;
            this.txtExt.Border.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtExt.Border.BorderTopWidth = 1;
            this.txtExt.Border.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.txtExt.Border.CornerDiameter = 4;
            this.txtExt.Border.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.txtExt.Dock = System.Windows.Forms.DockStyle.Right;
            this.txtExt.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.txtExt.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtExt.Location = new System.Drawing.Point(611, 0);
            this.txtExt.Name = "txtExt";
            this.txtExt.Size = new System.Drawing.Size(99, 24);
            this.txtExt.TabIndex = 413;
            this.txtExt.Tag = "8";
            // 
            // chkEnablePath
            // 
            // 
            // 
            // 
            this.chkEnablePath.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10002;
            this.chkEnablePath.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkEnablePath.Checked = true;
            this.chkEnablePath.CheckSignSize = new System.Drawing.Size(20, 20);
            this.chkEnablePath.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnablePath.CheckValue = "Y";
            this.chkEnablePath.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkEnablePath.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.chkEnablePath.Location = new System.Drawing.Point(160, 0);
            this.chkEnablePath.Name = "chkEnablePath";
            this.chkEnablePath.Size = new System.Drawing.Size(23, 25);
            this.chkEnablePath.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.chkEnablePath.TabIndex = 423;
            this.chkEnablePath.TextVisible = false;
            // 
            // frmPathInfo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(710, 25);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.btnPath);
            this.Controls.Add(this.cmbFolderType);
            this.Controls.Add(this.txtExt);
            this.Controls.Add(this.chkEnablePath);
            this.Controls.Add(this.lblPathName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmPathInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmPathInfo";
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX lblPathName;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPath;
        private DevComponents.DotNetBar.ButtonX btnPath;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbFolderType;
        private DevComponents.DotNetBar.Controls.TextBoxX txtExt;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkEnablePath;
    }
}