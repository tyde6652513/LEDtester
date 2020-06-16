using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using MPI.Windows.Forms;
using MPI.Tester.Data;
using MPI.Tester.TestKernel;


namespace MPI.Tester.Gui
{
	public partial class frmTestResultInstantInfo : System.Windows.Forms.Form
	{
		
		TimeSpan _spanWaferTestTime;

		public frmTestResultInstantInfo()
		{
			Console.WriteLine("[frmTestResultInstantInfo], frmTestResultInstantInfo()");

			InitializeComponent();

			this._spanWaferTestTime = new TimeSpan(0,0,0);
		}

		#region >>> Public Method <<<

		public void RegisterUpdateEvent( EventHandler e )
		{
			this.tmrUpdate.Tick += e;
		}

		#endregion

		#region >>> UI Event Handler <<<

		private void UpdateTimer_Tick(object sender, EventArgs e)
		{
            this.lblBinData.Text = DataCenter._acquireData.ChipInfo.BinGrade.ToString();
            this.lblColXData.Text = DataCenter._acquireData.ChipInfo.ColX.ToString();
            this.lblRowYData.Text = DataCenter._acquireData.ChipInfo.RowY.ToString();

            this.lblTestCountData.Text = DataCenter._acquireData.ChipInfo.TestCount.ToString();
            this.lblPassCountData.Text = DataCenter._acquireData.ChipInfo.GoodDieCount.ToString();
            this.lblFailCountData.Text = DataCenter._acquireData.ChipInfo.FailDieCount.ToString();
            this.lblPassRateData.Text = DataCenter._acquireData.ChipInfo.GoodRate.ToString("00.00") + "%";

            if (DataCenter._sysSetting.StartTestTime > DataCenter._sysSetting.EndTestTime)
            {
                this._spanWaferTestTime = System.DateTime.Now - DataCenter._sysSetting.StartTestTime;
                lblStartTimeData.Text = DataCenter._sysSetting.StartTestTime.ToString(@"yyyy/MM/dd hh\:mm\:ss");
            }

            this.lblRunTimeData.Text = this._spanWaferTestTime.ToString(@"hh\:mm\:ss");

            System.Windows.Forms.Application.DoEvents();

            //if (DataCenter._sysSetting.StartTestTime > DataCenter._sysSetting.EndTestTime)
            //{
            //    this._spanWaferTestTime = System.DateTime.Now - DataCenter._sysSetting.StartTestTime;
            //}

            //this.lblRunTimeData.Text = this._spanWaferTestTime.ToString(@"hh\:mm\:ss");

            //System.Windows.Forms.Application.DoEvents();




			if (DataCenter._userManag.CurrentAuthority == EAuthority.Admin || DataCenter._userManag.CurrentAuthority == EAuthority.Super)
			{
				this.lblTestTimeData.Text = DataCenter._acquireData.ChipInfo.TestTime.ToString("0000.0");
			}
			else
			{
				this.lblTestTimeData.Text ="0.0";
			}

		}

		private void frmTestResultInstantInfo_VisibleChanged(object sender, EventArgs e)
		{
			if (this.Visible == true)
			{
				this.tmrUpdate.Enabled = true;
			}
		}

		private void frmTestResultInstantInfo_Deactivate(object sender, EventArgs e)
		{
			this.tmrUpdate.Enabled = false;
		}

		#endregion
	}
}