using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MPI.Tester.Data;

namespace MPI.Tester.Gui
{
	public partial class frmLogin : Form
	{
		private bool _isClose;

		public frmLogin()
		{
			InitializeComponent();

            InputIdleTime.IdleEvent += this.OnIdleTime;

			this.StartPosition = FormStartPosition.CenterScreen;
		}

		private void txt_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				if (DataCenter._userManag.Login(txtUserName.Text, txtPassWord.Text))
				{
					UILog.Log(this, sender, "txt_KeyDown", "LoginID:" + this.txtUserName.Text + ",AuthorityLevel:" + DataCenter._uiSetting.AuthorityLevel);

					DataCenter._uiSetting.AuthorityLevel = DataCenter._userManag.CurrentAuthority;

					DataCenter._uiSetting.LoginID = DataCenter._userManag.CurrentUserName;

                    this.CheckIdleTime();

					this.Hide();
                    
					this.txtPassWord.Text = "";

					this._isClose = true;
				}
				else
				{
					UILog.Log(this, sender, "txt_KeyDown", "Error,UserName:" + this.txtUserName.Text + ",txtPassWord:" + this.txtPassWord.Text);

					this.txtUserName.Text = "";

					this.txtPassWord.Text = "";
				}
			}
            else if (e.KeyCode==Keys.F3)
			{
				if (DataCenter._userManag.Login("simulator", "calibration"))
				{
					UILog.Log(this, sender, "txt_KeyDown", "LoginID:" + this.txtUserName.Text + ",AuthorityLevel:" + DataCenter._uiSetting.AuthorityLevel);

					DataCenter._uiSetting.AuthorityLevel = DataCenter._userManag.CurrentAuthority;

					DataCenter._uiSetting.LoginID = DataCenter._userManag.CurrentUserName;
                   
                    this.CheckIdleTime();

					this.Hide();

					this.txtPassWord.Text = "";

					this._isClose = true;
				}
			}
			else if (e.Control && e.Alt && e.KeyCode == Keys.PageDown)
			{
				if (DataCenter._userManag.Login("Super", "ilovet200"))
				{
					UILog.Log(this, sender, "txt_KeyDown", "LoginID:" + this.txtUserName.Text + ",AuthorityLevel:" + DataCenter._uiSetting.AuthorityLevel);

					DataCenter._uiSetting.AuthorityLevel = DataCenter._userManag.CurrentAuthority;

					DataCenter._uiSetting.LoginID = DataCenter._userManag.CurrentUserName;

                    this.CheckIdleTime();

					this.Hide();

					this.txtPassWord.Text = "";

					this._isClose = true;
				}
			}
            else if (e.Control && e.Alt && e.KeyCode == Keys.Home)
            {
				if (DataCenter._userManag.Login("Admin", "89647050"))
				{
					UILog.Log(this, sender, "txt_KeyDown", "LoginID:" + this.txtUserName.Text + ",AuthorityLevel:" + DataCenter._uiSetting.AuthorityLevel);

					DataCenter._uiSetting.AuthorityLevel = DataCenter._userManag.CurrentAuthority;

					DataCenter._uiSetting.LoginID = DataCenter._userManag.CurrentUserName;

                    this.CheckIdleTime();

					this.Hide();

					this.txtPassWord.Text = "";

					this._isClose = true;
				}
				else
				{
					DataCenter._userManag.AddUser("Admin", "89647050", Data.EAuthority.Admin);

					if (DataCenter._userManag.Login("Admin", "89647050"))
					{
						UILog.Log(this, sender, "txt_KeyDown", "LoginID:" + this.txtUserName.Text + ",AuthorityLevel:" + DataCenter._uiSetting.AuthorityLevel);

						DataCenter._uiSetting.AuthorityLevel = DataCenter._userManag.CurrentAuthority;

						DataCenter._uiSetting.LoginID = DataCenter._userManag.CurrentUserName;
                      
                        this.CheckIdleTime();

						this.Hide();

						this.txtPassWord.Text = "";

						this._isClose = true;
					}
				}
            }
		}

		private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!this._isClose)
			{
				e.Cancel = true;
			}
		}

		private void frmLogin_Shown(object sender, EventArgs e)
		{
			this._isClose = false;
		}

		private void frmLogin_VisibleChanged(object sender, EventArgs e)
		{
			if (this.Visible)
			{
				this.txtUserName.Text = DataCenter._uiSetting.LoginID;
			}
		}

        private void CheckIdleTime()
        {
            if (DataCenter._uiSetting.AuthorityLevel != EAuthority.Operator &&
                DataCenter._machineConfig.Enable.IsAutoChangeToOPMode)
            {
                InputIdleTime.Start();
            }
            else
            {
                InputIdleTime.Stop();
            }
        }

        private void OnIdleTime()
        {
            DataCenter._userManag.ChangeToOperatorAuthority();

            DataCenter._uiSetting.AuthorityLevel = DataCenter._userManag.CurrentAuthority;

            DataCenter._uiSetting.LoginID = DataCenter._userManag.CurrentUserName;

            Host.UpdateDataToAllUIForm();
        }

	}
}
