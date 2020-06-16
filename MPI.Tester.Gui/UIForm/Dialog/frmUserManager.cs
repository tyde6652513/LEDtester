using System;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using MPI.AuthorityControl;
using MPI.Tester.Data;

namespace MPI.Tester.Gui
{
	public partial class frmUserManager : System.Windows.Forms.Form
	{
		private System.Collections.Generic.List<UserRecord> _userRecord;

		private string _addOrModify = "";

		public frmUserManager()
		{
			InitializeComponent();
		}

		#region >>> private Method <<<

		private void InitcmbUserAuthority()
		{
			this.cmbUserAuthority.Items.Clear();

			Array index = Enum.GetValues(typeof(MPI.Tester.Data.EAuthority));

			for (int i = 0; i < index.Length; i++)
			{
				if ((EAuthority)index.GetValue(i) <= DataCenter._uiSetting.AuthorityLevel)
				{
					this.cmbUserAuthority.Items.Add(index.GetValue(i).ToString());
				}
			}

			//this.cmbUserAuthority.Items.AddRange(Enum.GetNames(typeof(MPI.Tester.Data.EAuthority)));
		}

		private void ReloadUserList()
		{
			this.lstUserList.Items.Clear();

			if (this._userRecord.Count <= 0)
				return;

			foreach (UserRecord item in this._userRecord)
			{
				if (item.Name == "simulator")
					continue;

				if (item.AuthorityLevel <= DataCenter._uiSetting.AuthorityLevel)
				{
					this.lstUserList.Items.Add(item.Name);
				}
			}

			if (this.lstUserList.SelectedIndex < 0)
			{
				this.lstUserList.SelectedIndex = 0;
			}
		}

		private void Editable(bool IsEditable)
		{
			if (IsEditable)
			{
				this.txtUserName.ReadOnly = false;
				this.txtUserPassword.ReadOnly = false;
				this.cmbUserAuthority.Enabled = true;

				this.btnNewUser.Enabled = false;
				this.btnModifyUser.Enabled = false;
				this.btnDeleteUser.Enabled = false;
				this.btnConfirm.Enabled = true;
			}
			else
			{
				this.txtUserName.ReadOnly = true;
				this.txtUserPassword.ReadOnly = true;
				this.cmbUserAuthority.Enabled = false;

				this.btnNewUser.Enabled = true;
				this.btnModifyUser.Enabled = true;
				this.btnDeleteUser.Enabled = true;
				this.btnConfirm.Enabled = false;
			}
		}

		#endregion

		#region >>> UI Event Handler <<<

		private void frmUserManager_Shown(object sender, EventArgs e)
		{
			this._userRecord = DataCenter._userManag.GetUserRecordList();

			this.InitcmbUserAuthority();

			this.ReloadUserList();
		}

		private void UpdateUserAuthorityInfo(object sender, EventArgs e)
		{
			string selectedUserName = this.lstUserList.Items[this.lstUserList.SelectedIndex].ToString();

			foreach (UserRecord item in this._userRecord)
			{
				if (selectedUserName == item.Name)
				{
					this.txtUserName.Text = item.Name;
					this.txtUserPassword.Text = item.PassWord;

					for (int i = 0; i < this.cmbUserAuthority.Items.Count; i++)
					{
						if (this.cmbUserAuthority.Items[i].ToString() == item.AuthorityLevel.ToString())
						{
							this.cmbUserAuthority.SelectedIndex = i;
							this.Editable(false);
							return;
						}
					}
				}
			}
		}

		private void btnNewUser_Click(object sender, EventArgs e)
		{
			UILog.Log(this, sender, "btnNewUser_Click");
			
			this.txtUserName.Text = "";
			this.txtUserPassword.Text = "";
			this.cmbUserAuthority.SelectedIndex = 0;

			this._addOrModify = "Add";

			this.Editable(true);
		}

		private void btnDeleteUser_Click(object sender, EventArgs e)
		{
			if (this.lstUserList.SelectedIndex < 0)
				return;

			string selectedUserName = this.lstUserList.Items[this.lstUserList.SelectedIndex].ToString();

			UILog.Log(this, sender, "btnDeleteUser_Click", selectedUserName);

			for (int i = 0; i < this._userRecord.Count; i++ )
			{
				if (selectedUserName == this._userRecord[i].Name)
				{
					this._userRecord.RemoveAt(i);

					this.ReloadUserList();

					return;
				}
			}

			
		}

		private void btnModifyUser_Click(object sender, EventArgs e)
		{
			UILog.Log(this, sender, "btnModifyUser_Click");

			this._addOrModify = "Modify";

			this.Editable(true);
		}

		private void btnConfirm_Click(object sender, EventArgs e)
		{		
			int index = this.cmbUserAuthority.SelectedIndex;
			string strAuthority = this.cmbUserAuthority.Items[index].ToString();
			
			string userName = this.txtUserName.Text;
			string password = this.txtUserPassword.Text;

			MPI.Tester.Data.EAuthority authority = (MPI.Tester.Data.EAuthority)Enum.Parse(typeof(MPI.Tester.Data.EAuthority), strAuthority);

			if (userName == "")
			{
				MessageBox.Show("User Name can't be empty!");
				return;
			}


			//Check User Name is not Repeated
			string selectedName = this.lstUserList.Items[this.lstUserList.SelectedIndex].ToString();
			int indexOfUserRecord = 0;

			for(int i=0; i<this._userRecord.Count; i++)
			{
				if (this._userRecord[i].Name == selectedName)
				{
					indexOfUserRecord = i;
				}
			}
			
			for (int i = 0; i < this._userRecord.Count; i++)
			{
				if (this._addOrModify == "Modify")
				{
					if (i == indexOfUserRecord)
						continue;
				}

				if (this._userRecord[i].Name == userName)
				{
					MessageBox.Show("User Name Repeated!");
					return;
				}
			}

			if (this._addOrModify == "Add")
			{
				UserRecord userRecord = new UserRecord(userName, password, authority);
				this._userRecord.Add(userRecord);

				this.ReloadUserList();
				this.lstUserList.SelectedIndex = this.lstUserList.Items.Count - 1;
			}
			else if (this._addOrModify == "Modify")
			{
				int indexUserList = this.lstUserList.SelectedIndex;

				foreach (UserRecord item in this._userRecord)
				{
					if (item.Name == this.lstUserList.Items[indexUserList].ToString())
					{
						UILog.Log(this, sender, "btnConfirm_Click", "From UserName:" + item.Name + ",password:" + item.PassWord);

						item.Name = this.txtUserName.Text;
						item.PassWord = this.txtUserPassword.Text;
						item.AuthorityLevel = authority;

						break;
					}
				}

				this.ReloadUserList();
				this.lstUserList.SelectedIndex = indexUserList;
			}

			UILog.Log(this, sender, "btnConfirm_Click", "UserName:" + userName + ",password:" + password);

			this.Editable(false);
		}

		private void btnSaveUser_Click(object sender, EventArgs e)
		{
			UILog.Log(this, sender, "btnSaveUser_Click");

			DataCenter._userManag.Remove();

			foreach (UserRecord item in this._userRecord)
			{
				DataCenter._userManag.AddUser(item.Name, item.PassWord, item.AuthorityLevel);
			}

			DataCenter._userManag.SaveFile();
		}

		private void btnExit_Click(object sender, EventArgs e)
		{
			UILog.Log(this, sender, "btnExit_Click");

			this.Hide();
		}

		#endregion
	}
}
