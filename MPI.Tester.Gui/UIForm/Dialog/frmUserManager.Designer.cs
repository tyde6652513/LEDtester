using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.Tester.Gui
{
	public partial class frmUserManager
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( components != null )
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserManager));
            this.lstUserList = new System.Windows.Forms.ListBox();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
			this.labUserName_I18N = new DevComponents.DotNetBar.LabelX();
			this.txtUserName = new DevComponents.DotNetBar.Controls.TextBoxX();
			this.btnNewUser = new DevComponents.DotNetBar.ButtonX();
			this.btnDeleteUser = new DevComponents.DotNetBar.ButtonX();
			this.labUserAuthority_I18N = new DevComponents.DotNetBar.LabelX();
			this.labUserPassword_I18N = new DevComponents.DotNetBar.LabelX();
			this.txtUserPassword = new DevComponents.DotNetBar.Controls.TextBoxX();
			this.labUserInformation_I18N = new DevComponents.DotNetBar.LabelX();
			this.cmbUserAuthority = new DevComponents.DotNetBar.Controls.ComboBoxEx();
			this.btnModifyUser = new DevComponents.DotNetBar.ButtonX();
            this.btnConfirm = new DevComponents.DotNetBar.ButtonX();
            this.grpUserMamager = new System.Windows.Forms.GroupBox();
            this.grpUserMamager.SuspendLayout();
			this.SuspendLayout();
			// 
            // lstUserList
            // 
            this.lstUserList.BackColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(this.lstUserList, "lstUserList");
            this.lstUserList.ForeColor = System.Drawing.Color.DarkOrange;
            this.lstUserList.Name = "lstUserList";
            this.lstUserList.SelectedIndexChanged += new System.EventHandler(this.UpdateUserAuthorityInfo);
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.BackColor = System.Drawing.Color.LightSteelBlue;
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Image = global::MPI.Tester.Gui.Properties.Resources.btnSaveFile_B;
            this.btnSave.ImageFixedSize = new System.Drawing.Size(25, 25);
            this.btnSave.Name = "btnSave";
            this.btnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnSave.Click += new System.EventHandler(this.btnSaveUser_Click);
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.BackColor = System.Drawing.Color.LightSteelBlue;
            resources.ApplyResources(this.btnExit, "btnExit");
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Image = global::MPI.Tester.Gui.Properties.Resources.btnUndo;
            this.btnExit.ImageFixedSize = new System.Drawing.Size(25, 25);
            this.btnExit.Name = "btnExit";
            this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
			// labUserName_I18N
			// 
			this.labUserName_I18N.BackColor = System.Drawing.Color.Gainsboro;
			// 
			// 
			// 
            this.labUserName_I18N.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.labUserName_I18N.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			resources.ApplyResources(this.labUserName_I18N, "labUserName_I18N");
            this.labUserName_I18N.ForeColor = System.Drawing.Color.MidnightBlue;
			this.labUserName_I18N.Name = "labUserName_I18N";
			// 
			// txtUserName
			// 
			// 
			// 
			// 
            this.txtUserName.Border.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.txtUserName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			resources.ApplyResources(this.txtUserName, "txtUserName");
			this.txtUserName.Name = "txtUserName";
			// 
			// btnNewUser
			// 
			this.btnNewUser.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnNewUser.BackColor = System.Drawing.Color.LightSteelBlue;
			resources.ApplyResources(this.btnNewUser, "btnNewUser");
			this.btnNewUser.ForeColor = System.Drawing.Color.Black;
			this.btnNewUser.Image = global::MPI.Tester.Gui.Properties.Resources.btnNewAdd;
			this.btnNewUser.Name = "btnNewUser";
			this.btnNewUser.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
			this.btnNewUser.Click += new System.EventHandler(this.btnNewUser_Click);
			// 
			// btnDeleteUser
			// 
			this.btnDeleteUser.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnDeleteUser.BackColor = System.Drawing.Color.LightSteelBlue;
			resources.ApplyResources(this.btnDeleteUser, "btnDeleteUser");
			this.btnDeleteUser.ForeColor = System.Drawing.Color.Black;
			this.btnDeleteUser.Image = global::MPI.Tester.Gui.Properties.Resources.btnCancel;
			this.btnDeleteUser.Name = "btnDeleteUser";
			this.btnDeleteUser.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
			this.btnDeleteUser.Click += new System.EventHandler(this.btnDeleteUser_Click);
			// 
			// labUserAuthority_I18N
			// 
			this.labUserAuthority_I18N.BackColor = System.Drawing.Color.Gainsboro;
			// 
			// 
			// 
            this.labUserAuthority_I18N.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.labUserAuthority_I18N.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			resources.ApplyResources(this.labUserAuthority_I18N, "labUserAuthority_I18N");
            this.labUserAuthority_I18N.ForeColor = System.Drawing.Color.MidnightBlue;
			this.labUserAuthority_I18N.Name = "labUserAuthority_I18N";
			// 
			// labUserPassword_I18N
			// 
			this.labUserPassword_I18N.BackColor = System.Drawing.Color.Gainsboro;
			// 
			// 
			// 
            this.labUserPassword_I18N.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.labUserPassword_I18N.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			resources.ApplyResources(this.labUserPassword_I18N, "labUserPassword_I18N");
            this.labUserPassword_I18N.ForeColor = System.Drawing.Color.MidnightBlue;
			this.labUserPassword_I18N.Name = "labUserPassword_I18N";
			// 
			// txtUserPassword
			// 
			// 
			// 
			// 
            this.txtUserPassword.Border.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.txtUserPassword.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			resources.ApplyResources(this.txtUserPassword, "txtUserPassword");
			this.txtUserPassword.Name = "txtUserPassword";
			// 
			// labUserInformation_I18N
			// 
			this.labUserInformation_I18N.BackColor = System.Drawing.Color.LightSteelBlue;
			// 
			// 
			// 
            this.labUserInformation_I18N.BackgroundStyle.Class = global::MPI.Tester.Gui.ResourceErr.ERR_10027;
			this.labUserInformation_I18N.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			resources.ApplyResources(this.labUserInformation_I18N, "labUserInformation_I18N");
            this.labUserInformation_I18N.ForeColor = System.Drawing.Color.MidnightBlue;
			this.labUserInformation_I18N.Name = "labUserInformation_I18N";
			// 
			// cmbUserAuthority
			// 
			this.cmbUserAuthority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			resources.ApplyResources(this.cmbUserAuthority, "cmbUserAuthority");
			this.cmbUserAuthority.Name = "cmbUserAuthority";
			this.cmbUserAuthority.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			// 
			// btnModifyUser
			// 
			this.btnModifyUser.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnModifyUser.BackColor = System.Drawing.Color.LightSteelBlue;
			resources.ApplyResources(this.btnModifyUser, "btnModifyUser");
			this.btnModifyUser.ForeColor = System.Drawing.Color.Black;
			this.btnModifyUser.Image = global::MPI.Tester.Gui.Properties.Resources.btnEditBin;
			this.btnModifyUser.Name = "btnModifyUser";
			this.btnModifyUser.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
			this.btnModifyUser.Click += new System.EventHandler(this.btnModifyUser_Click);
			// 
            // btnConfirm
			// 
            this.btnConfirm.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnConfirm.BackColor = System.Drawing.Color.LightSteelBlue;
            resources.ApplyResources(this.btnConfirm, "btnConfirm");
            this.btnConfirm.ForeColor = System.Drawing.Color.Black;
            this.btnConfirm.Image = global::MPI.Tester.Gui.Properties.Resources.btnConfirm;
            this.btnConfirm.ImageFixedSize = new System.Drawing.Size(25, 25);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
			// 
            // grpUserMamager
			// 
            this.grpUserMamager.BackColor = System.Drawing.SystemColors.Control;
            this.grpUserMamager.Controls.Add(this.btnConfirm);
            this.grpUserMamager.Controls.Add(this.lstUserList);
            this.grpUserMamager.Controls.Add(this.cmbUserAuthority);
            this.grpUserMamager.Controls.Add(this.btnModifyUser);
            this.grpUserMamager.Controls.Add(this.txtUserPassword);
            this.grpUserMamager.Controls.Add(this.labUserInformation_I18N);
            this.grpUserMamager.Controls.Add(this.labUserPassword_I18N);
            this.grpUserMamager.Controls.Add(this.btnNewUser);
            this.grpUserMamager.Controls.Add(this.labUserAuthority_I18N);
            this.grpUserMamager.Controls.Add(this.btnDeleteUser);
            this.grpUserMamager.Controls.Add(this.txtUserName);
            this.grpUserMamager.Controls.Add(this.labUserName_I18N);
            resources.ApplyResources(this.grpUserMamager, "grpUserMamager");
            this.grpUserMamager.ForeColor = System.Drawing.Color.DimGray;
            this.grpUserMamager.Name = "grpUserMamager";
            this.grpUserMamager.TabStop = false;
			// 
			// frmUserManager
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::MPI.Tester.Gui.Properties.Resources.mini_hero_bg;
			resources.ApplyResources(this, "$this");
            this.Controls.Add(this.grpUserMamager);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSave);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmUserManager";
			this.Shown += new System.EventHandler(this.frmUserManager_Shown);
            this.grpUserMamager.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.ListBox lstUserList;
		private DevComponents.DotNetBar.ButtonX btnSave;
		private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.ButtonX btnConfirm;
        private DevComponents.DotNetBar.ButtonX btnModifyUser;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbUserAuthority;
        private DevComponents.DotNetBar.LabelX labUserInformation_I18N;
        private DevComponents.DotNetBar.Controls.TextBoxX txtUserPassword;
        private DevComponents.DotNetBar.LabelX labUserPassword_I18N;
        private DevComponents.DotNetBar.LabelX labUserAuthority_I18N;
        private DevComponents.DotNetBar.ButtonX btnDeleteUser;
        private DevComponents.DotNetBar.ButtonX btnNewUser;
        private DevComponents.DotNetBar.Controls.TextBoxX txtUserName;
        private DevComponents.DotNetBar.LabelX labUserName_I18N;
        private System.Windows.Forms.GroupBox grpUserMamager;

	}
}
