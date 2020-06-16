using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MPI.AuthorityControl;
using MPI.Tester.Data;
using MPI.Tester.TestKernel;

using MPI.UCF;

namespace MPI.Tester.Gui
{
	public partial class frmMain : System.Windows.Forms.Form
	{
		private ToolStripButton _pushBtn;
		private Point _oldWndPoint;
		private Point _clickPoint;
		private bool _isClose;
		private object _lockObj;
		private static AlertCustom _alertMessage = new AlertCustom();

		public frmMain()
		{
			InitializeComponent();

			this._oldWndPoint = new Point();

			this._clickPoint = new Point();

			this.btnSetting.Click += new System.EventHandler(this.Buttons_Click);
			this.btnBinSetting.Click += new System.EventHandler(this.Buttons_Click);
			this.btnTestResult.Click += new System.EventHandler(this.Buttons_Click);
			this.btnTool.Click += new System.EventHandler(this.Buttons_Click);

			Host.ErrorCodeEvent += this.OnErrorCodeEvent;

			this.Left = DataCenter._uiSetting.MainFormLeftPos;

			this.Top = DataCenter._uiSetting.MainFormTopPos;

			this.TopMost = true;

			this._lockObj = new object();
		}

		#region >>> Private Method <<<

		private void OnErrorCodeEvent(ErrorCodeEventArgs e)
		{
			if (this.InvokeRequired)
			{
				this.BeginInvoke(new ErrorCodeEventHandler(CheckErrorCode), e);
			}
			else
			{
				this.CheckErrorCode(e);
			}
		}

		private void CheckErrorCode(ErrorCodeEventArgs e)
		{
            if (e.ErrorCode == EErrorCode.NONE)
            {
                return;
            }

			// Has one Error Code and never show the message

			string strKey = string.Format("ERR_{0:D5}", (int)e.ErrorCode, e.ErrorCode);

			string msg = "Error Code =" + (int)e.ErrorCode + Environment.NewLine + Host.RM.GetString(strKey);

			string hint = Host.RM.GetString(strKey);

			_alertMessage.Title = strKey;

			_alertMessage.Message = msg;

			string errorHandler = Host.resourceManagerMessage.GetString(strKey);

			errorHandler += Host._alarmDescribe.ToString();

			errorHandler += e.ErrorMsg;

			Host._alarmDescribe = new StringBuilder();

			_alertMessage.ErrorHandler = errorHandler;

			AlarmBox.Show((int)e.ErrorCode, "ERROR", hint, e.ErrorCode.ToString(), errorHandler.Split('\n'));			
		}

		#endregion

		#region >>> Public Method <<<

		public void SetKernelErrorCodeEvent()
		{
			AppSystem._MPITesterKernel.ErrorCodeEvent += this.OnErrorCodeEvent;
		}

		public void SetAuthorityLevel(EAuthority level)
		{
			switch (level)
			{
				case EAuthority.Operator:
				case EAuthority.QC:
					// this.btnOperate.Visible = false;
					this.btnCondition.Visible = true;
					this.btnTestResult.Visible = true;
					this.btnSetting.Visible = true;
					this.btnTool.Visible = false;
					this.btnBinSetting.Visible = false;
					this.btnExitApp.Visible = true;
					this.btnExitApp.Enabled = false;
					break;
				//------------------------------------------------------------------------------------------------
				case EAuthority.Engineer:
				case EAuthority.Admin:
					//this.btnOperate.Visible = true;
					this.btnCondition.Visible = true;
					this.btnTestResult.Visible = true;
					this.btnSetting.Visible = true;
					this.btnTool.Visible = false;
					this.btnBinSetting.Visible = false;
					this.btnExitApp.Visible = true;
					this.btnExitApp.Enabled = true;
					break;
				//------------------------------------------------------------------------------------------------
				case EAuthority.Super:
					//   this.btnOperate.Visible = false;
					this.btnCondition.Visible = true;
					this.btnTestResult.Visible = true;
					this.btnSetting.Visible = true;
					this.btnTool.Enabled = true;
					this.btnBinSetting.Visible = true;
					this.btnExitApp.Visible = true;
					this.btnExitApp.Enabled = true;
					break;
				//------------------------------------------------------------------------------------------------
				default:
					//  this.btnOperate.Visible = false;
					this.btnCondition.Visible = true;
					this.btnTestResult.Visible = true;
					this.btnSetting.Visible = false;
					this.btnTool.Visible = false;
					this.btnBinSetting.Visible = false;
					this.btnExitApp.Visible = true;
					this.btnExitApp.Enabled = true;
					break;
			}
		}

		public void ExitApp()
		{
			DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show("\nWould you close the application？\n\n", "Close", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

			//FormAgent.ShowAlert( "Close", "Would you close the application？" );
			if (result == DialogResult.OK)
			{
				//DataCenter.Save();
				DataCenter._uiSetting.MainFormLeftPos = this.Left;
				DataCenter._uiSetting.MainFormTopPos = this.Top;
				this._isClose = true;
				this.Close();
			}

			Host.ErrorCodeEvent -= this.OnErrorCodeEvent;
			AppSystem._MPITesterKernel.ErrorCodeEvent -= this.OnErrorCodeEvent;
		}

		#endregion

		#region >>> UI Event Handler <<<

		private void frmMain_Load(object sender, EventArgs e)
		{
			this.Hide();
			AppSystem.Fire_SwitchUIEvent((int)EBaseFormDisplayUI.ConditionForm);
		}

		private void tlsMain_MouseDown(object sender, MouseEventArgs e)
		{
			this._oldWndPoint.X = this.Left;
			this._oldWndPoint.Y = this.Right;
			this._clickPoint.X = e.X;
			this._clickPoint.Y = e.Y;
		}

		private void tlsMain_MouseMove(object sender, MouseEventArgs e)
		{
			if (this.tlsMain.Capture == true && e.Button == MouseButtons.Left)
			{
				if (this.Left < (Screen.PrimaryScreen.Bounds.Width - this.Size.Width))
				{
					this.Left = e.X + this._oldWndPoint.X - this._clickPoint.X;
				}
				else
				{
					this.Left = Screen.PrimaryScreen.Bounds.Width - this.Size.Width - 10;
				}

				if (this.Top < (Screen.PrimaryScreen.Bounds.Height - this.Size.Height))
				{
					this.Top = e.Y + this._oldWndPoint.Y - this._clickPoint.Y;
				}
				else
				{
					this.Top = Screen.PrimaryScreen.Bounds.Height - this.Size.Height - 10;
				}

				this._oldWndPoint.X = this.Left;
				this._oldWndPoint.Y = this.Top;
			}
		}

		private void btnExitApp_Click(object sender, EventArgs e)
		{
			ExitApp();
		}

		private void Buttons_Click(object sender, EventArgs e)
		{
			if (this._pushBtn == null)
			{
				this._pushBtn = (sender as ToolStripButton);
				//this._pushBtn.BackColor = Color.LightPink;
			}
			else
			{
				this._pushBtn.BackColor = Color.Transparent;
				this._pushBtn = (sender as ToolStripButton);
				//(sender as ToolStripButton).BackColor = Color.LightPink;
			}

			string tag = (sender as ToolStripButton).Tag.ToString();
			FormAgent.SwitchForm(tag);
			this.TopMost = true;
		}

		private void frmMain_VisibleChanged(object sender, EventArgs e)
		{
			if (this.Visible)
			{
				this.Size = this.tlsMain.Size;

				this.Hide();
				AppSystem.Fire_SwitchUIEvent((int)EBaseFormDisplayUI.ConditionForm);

				//if (this.Location.X > 1080)
				//{ 
				//    this.Location = new Point(1080, this.Location.Y);
				//}

				this.Location = new Point(100, 100);
			}
		}

		private void tlsMain_MouseHover(object sender, EventArgs e)
		{
			this.Activate();
			this.Focus();
		}

		private void frmMain_Shown(object sender, EventArgs e)
		{
			this._isClose = false;
		}

		private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!this._isClose)
			{
				e.Cancel = true;
			}
		}

		private void btnCondition_Click(object sender, EventArgs e)
		{
			AppSystem.Fire_SwitchUIEvent((int)EBaseFormDisplayUI.ConditionForm);
			this.Hide();
		}

		#endregion

		#region >>> Public Properties <<<

		public bool IsClose
		{
			get { return this._isClose; }
			set { lock (this._lockObj) { this._isClose = value; } }
		}

		#endregion
	}
}