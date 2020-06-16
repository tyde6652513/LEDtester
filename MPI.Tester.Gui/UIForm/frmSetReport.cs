using System;
using System.Text;
using System.Windows.Forms;

using MPI.Tester.Data;

namespace MPI.Tester.Gui
{
	public partial class frmSetReport : System.Windows.Forms.Form
	{
		public frmSetReport()
		{
			Console.WriteLine("[frmSetReport], frmSetReport()");

			InitializeComponent();
		}

		#region >>> Private Method <<<

		private void UpdateDataToControls()
		{
			this.lblTesterModel.Text = DataCenter._machineInfo.TesterModel;
			this.lblTesterSN.Text = DataCenter._machineInfo.TesterSN;
			this.lblSpectrometerSN.Text = DataCenter._machineInfo.SpectrometerSN;
			this.lblSphereSN.Text = DataCenter._machineInfo.SphereSN;
            this.lblESDSN.Text = DataCenter._machineInfo.EsdSN;
            this.lblSwitchSystemSN.Text = DataCenter._machineInfo.SwitchSystemSN;
            this.lblMachineName.Text = DataCenter._uiSetting.MachineName;

			if (DataCenter._machineInfo.EPPROMConfigData != null && DataCenter._machineInfo.EPPROMConfigData.Length > 15)
            {	
                this.lblSptIntceptCoeff.Text = DataCenter._machineInfo.EPPROMConfigData[1];
                this.lblSptFirstCoeff.Text = DataCenter._machineInfo.EPPROMConfigData[2];
                this.lblSptSeconfCoeff.Text = DataCenter._machineInfo.EPPROMConfigData[3];
                this.lblSptThirdCoeff.Text = DataCenter._machineInfo.EPPROMConfigData[4];
                this.lblSptDiscribe.Text = DataCenter._machineInfo.EPPROMConfigData[15];
            }
		}

		private void SaveDataToFile()
		{
			AppSystem.SetDataToSystem();
			DataCenter.Save();
		}

		private void frmReportSetting_Load(object sender, EventArgs e)
		{
			this.UpdateDataToControls();
		}

		#endregion

		#region >>> UI Event Handler <<<

		private void frmReportSetting_VisibleChanged(object sender, EventArgs e)
		{
			if (this.Visible == true)
			{
				this.UpdateDataToControls();
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			this.SaveDataToFile();
		}

		#endregion

	}
}