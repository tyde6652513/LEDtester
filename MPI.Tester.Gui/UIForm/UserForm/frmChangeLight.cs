using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MPI.Tester.Gui
{
    public partial class frmChangeLight : Form
    {
		private bool _isUpdate;

        public frmChangeLight()
        {
            InitializeComponent();

            Host.OnTestItemChangeEvent += new EventHandler(this.UpdateDataToUI);

			Host.OnBarcCodeSaveEvent += new EventHandler(this.SaveData);
        }

        private void frmChangeLight_Load(object sender, EventArgs e)
        {
			this.GetLotNumAndSpec();

            this.UpdateDataToUI(null,null);
        }

        private void GetLotNumAndSpec()
        {
            int lotStart = 0;
            int lotEnd = 0;
            int SpecStart = 0;
            int SpecEnd = 0;

            if (!int.TryParse(DataCenter._uiSetting.WeiminUIData.Remark01, out lotStart) || lotStart == 0)
            {
                lotStart = 1;
                DataCenter._uiSetting.WeiminUIData.Remark01 = "1";
            }

            if (!int.TryParse(DataCenter._uiSetting.WeiminUIData.Remark02, out lotEnd) || lotEnd == 0)
            {
                lotEnd = 10;
                DataCenter._uiSetting.WeiminUIData.Remark02 = "10";
            }

            if (!int.TryParse(DataCenter._uiSetting.WeiminUIData.Remark03, out SpecStart) || SpecStart == 0)
            {
                SpecStart = 1;
                DataCenter._uiSetting.WeiminUIData.Remark03 = "1";
            }

            if (!int.TryParse(DataCenter._uiSetting.WeiminUIData.Remark04, out SpecEnd) || SpecEnd == 0)
            {
                SpecEnd = 10;
                DataCenter._uiSetting.WeiminUIData.Remark04 = "10";
            }            

            //OutputFileName = "ASDFGHJKL"
            //lotStart = 3, lotEnd = 6
            //LotNum = "DFGH" //from index 3 to index 6
            if (DataCenter._uiSetting.WeiminUIData.OutputFileName.Length > (lotStart - 1) && lotEnd >= lotStart)
            {
                int startIndex = lotStart - 1;

                int len = lotEnd - lotStart + 1;

                if (len > DataCenter._uiSetting.WeiminUIData.OutputFileName.Length - startIndex)
                {
                    len = DataCenter._uiSetting.WeiminUIData.OutputFileName.Length - startIndex;
                }

                DataCenter._uiSetting.WeiminUIData.CustomerNote01 = DataCenter._uiSetting.WeiminUIData.OutputFileName.Substring(startIndex, len);
            }
            else
            {
                DataCenter._uiSetting.WeiminUIData.CustomerNote01 = string.Empty;
            }

            if (DataCenter._uiSetting.WeiminUIData.OutputFileName.Length > (SpecStart - 1) && SpecEnd >= SpecStart)
            {
                int startIndex = SpecStart - 1;

                int len = SpecEnd - SpecStart + 1;

                if (len > DataCenter._uiSetting.WeiminUIData.OutputFileName.Length - startIndex)
                {
                    len = DataCenter._uiSetting.WeiminUIData.OutputFileName.Length - startIndex;
                }

                DataCenter._uiSetting.WeiminUIData.CustomerNote02 = DataCenter._uiSetting.WeiminUIData.OutputFileName.Substring(startIndex, len);
            }
            else
            {
                DataCenter._uiSetting.WeiminUIData.CustomerNote02 = string.Empty;
            }

			if (DataCenter._uiSetting.WeiminUIData.WMTestMode != 0 && DataCenter._uiSetting.WeiminUIData.WMTestMode != 1)
			{
				DataCenter._uiSetting.WeiminUIData.WMTestMode = 0;
			}

			DataCenter.SaveUISettingToFile();
        }

        private void SaveData(object o, EventArgs e)
        {
            DataCenter._uiSetting.WeiminUIData.OutputFileName = this.txtOutputFileName.Text;

			DataCenter._uiSetting.WaferNumber = this.txtWaferNumber.Text;

			DataCenter._uiSetting.WeiminUIData.CustomerNote01 = this.txtLotNumber.Text;

			DataCenter._uiSetting.WeiminUIData.Remark01 = this.numLotStart.Value.ToString();

			DataCenter._uiSetting.WeiminUIData.Remark02 = this.numLotEnd.Value.ToString();

			DataCenter._uiSetting.WeiminUIData.CustomerNote02 = this.txtSpec.Text;

			DataCenter._uiSetting.WeiminUIData.Remark03 = this.numSpecStart.Value.ToString();

			DataCenter._uiSetting.WeiminUIData.Remark04 = this.numSpecEnd.Value.ToString();

			DataCenter._uiSetting.OperatorName = this.txtOperatorName.Text;

			DataCenter._uiSetting.WeiminUIData.WMTestMode = this.rdoCOW.Checked ? 1 : 0;

            this.GetLotNumAndSpec();

			DataCenter.SaveUISettingToFile();
        }

        private void UpdateDataToUI(object o, EventArgs e)
        {
			if (!this.IsHandleCreated)
			{
				return;
			}

			this._isUpdate = true;

			this.txtOutputFileName.Text = DataCenter._uiSetting.WeiminUIData.OutputFileName;

            this.txtWaferNumber.Text = DataCenter._uiSetting.WaferNumber;

			this.txtLotNumber.Text = DataCenter._uiSetting.WeiminUIData.CustomerNote01;

			this.numLotStart.Value = int.Parse(DataCenter._uiSetting.WeiminUIData.Remark01);

			this.numLotEnd.Value = int.Parse(DataCenter._uiSetting.WeiminUIData.Remark02);

			this.txtSpec.Text = DataCenter._uiSetting.WeiminUIData.CustomerNote02;

			this.numSpecStart.Value = int.Parse(DataCenter._uiSetting.WeiminUIData.Remark03);

			this.numSpecEnd.Value = int.Parse(DataCenter._uiSetting.WeiminUIData.Remark04);

			this.txtOperatorName.Text = DataCenter._uiSetting.OperatorName;

			if (DataCenter._uiSetting.WeiminUIData.WMTestMode == 0)
			{
				this.rdoCOT.Checked = true;
			}
			else
			{
				this.rdoCOW.Checked = true;
			}

			this._isUpdate = false;            
        }

        private void UI_DataChanged(object sender, EventArgs e)
        {
            if (!this.IsHandleCreated | this._isUpdate)
            {
                return;
            }

            this.SaveData(null, null);

            this.UpdateDataToUI(null, null);
        }
    }
}
