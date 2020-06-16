using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Gui
{
	public partial class frmSetRDFunc : System.Windows.Forms.Form
	{
        private TestItemDescriptionCollections _tempSpecDefinition;
        
        public frmSetRDFunc()
		{
			InitializeComponent();

            this.cmbTesterConfigType.Items.Clear();

            foreach (var e in Enum.GetValues(typeof(ETesterConfigType)))
            {
                this.cmbTesterConfigType.Items.Add(e);
            }

			this.InitUIComponent();
		}

		private void InitUIComponent()
		{
            this.lblSettingInfo.Text = string.Empty;
            
            this.lblUserID.Text = string.Format("User ID: {0}", DataCenter._rdFunc.RDFuncData.UserID);
            
            ///////////////////////////////////////////////////////////////////////////////////////////
            // Parameter Setting
            ///////////////////////////////////////////////////////////////////////////////////////////
			this.numHighSpeedModeDelayTime.Value = (int)DataCenter._rdFunc.RDFuncData.HighSpeedModeDelayTime;

            this.chkIsEnableESDHighSpeedMode.Checked = DataCenter._rdFunc.RDFuncData.IsEnableESDHighSpeedMode;

            this.numESDHighSpeedDelayTime.Value = (int)DataCenter._rdFunc.RDFuncData.ESDHighSpeedDelayTime;

            this.chkIsEnableRTHTestItem.Checked = DataCenter._rdFunc.RDFuncData.IsEnableRTHTestItem;

            this.ipRthAddress.Value = DataCenter._rdFunc.RDFuncData.RTHSrcMeterIPAddress;

			this.chkIsEnableVRDelayTime.Checked = DataCenter._rdFunc.RDFuncData.IsEnableVRDelayTime;

			this.numVRDelayTime.Value = (int)DataCenter._rdFunc.RDFuncData.VRDelayTime;

            this.chkIsEnableKeepRecoveryData.Checked = DataCenter._rdFunc.RDFuncData.IsKeepRecoveryData;

            this.chkIsEnableAbsMsrtIR.Checked = DataCenter._rdFunc.RDFuncData.IsEnableAbsMsrtIR;

            this.dinRTHTdTime.Value = DataCenter._rdFunc.RDFuncData.RTHTdTime;

            this.numMDSeriesDelayTime.Value = (int)DataCenter._rdFunc.RDFuncData.MDSeriesTypeDelayTime;

            this.numMDParallelDelayTime.Value = (int)DataCenter._rdFunc.RDFuncData.MDParallelTypeDelayTime;

            this.cmbTesterConfigType.SelectedItem = DataCenter._rdFunc.RDFuncData.TesterConfigType;

            this.btnSetIO.Visible = (DataCenter._rdFunc.RDFuncData.TesterConfigType == ETesterConfigType.PDTester);

			///////////////////////////////////////////////////////////////////////////////////////////
			// Spectrometer Setting
			///////////////////////////////////////////////////////////////////////////////////////////

            this.InitTestItemSpecList();
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
            DialogResult result;

            if (DataCenter._rdFunc.RDFuncData.UserID != ((int)DataCenter._uiSetting.UserID).ToString("0000"))
            {
                result = MessageBox.Show("UserID 與 RDFuncData UserID不符, 設定資料無法儲存", "SAVE",MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }
            
            result = TopMessageBox.ShowMessage((int)EMessageCode.CheckIsReStartSystem, "System Setting , Please Restart the application？", "Close", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
			
			if (result != DialogResult.OK)
			{
				return;
			}

			///////////////////////////////////////////////////////////////////////////////////////////
			// Parameter Setting
			///////////////////////////////////////////////////////////////////////////////////////////
            DataCenter._rdFunc.RDFuncData.HighSpeedModeDelayTime = this.numHighSpeedModeDelayTime.Value;

            DataCenter._rdFunc.RDFuncData.IsEnableESDHighSpeedMode = this.chkIsEnableESDHighSpeedMode.Checked;

            DataCenter._rdFunc.RDFuncData.ESDHighSpeedDelayTime = this.numESDHighSpeedDelayTime.Value;

            DataCenter._rdFunc.RDFuncData.IsEnableRTHTestItem = this.chkIsEnableRTHTestItem.Checked;

            DataCenter._rdFunc.RDFuncData.RTHSrcMeterIPAddress = this.ipRthAddress.Value;

			DataCenter._rdFunc.RDFuncData.IsEnableVRDelayTime = this.chkIsEnableVRDelayTime.Checked;

			DataCenter._rdFunc.RDFuncData.VRDelayTime = this.numVRDelayTime.Value;

            DataCenter._rdFunc.RDFuncData.IsKeepRecoveryData = this.chkIsEnableKeepRecoveryData.Checked;

            DataCenter._rdFunc.RDFuncData.IsEnableAbsMsrtIR = this.chkIsEnableAbsMsrtIR.Checked;

            DataCenter._rdFunc.RDFuncData.RTHTdTime = this.dinRTHTdTime.Value;

            DataCenter._rdFunc.RDFuncData.MDSeriesTypeDelayTime = this.numMDSeriesDelayTime.Value;

            DataCenter._rdFunc.RDFuncData.MDParallelTypeDelayTime = this.numMDParallelDelayTime.Value;

            DataCenter._rdFunc.RDFuncData.TesterConfigType = (ETesterConfigType)this.cmbTesterConfigType.SelectedItem;



            // Save Test Item Spec
            DataCenter._rdFunc.RDFuncData.SpecDataDefinition = this._tempSpecDefinition.Clone() as TestItemDescriptionCollections;

			DataCenter.Save();

			FormAgent.MainForm.IsClose = true;

			FormAgent.MainForm.Close();
		}

		private void btnReload_Click(object sender, EventArgs e)
		{
			this.InitUIComponent();
		}

        private void btnSetIO_Click(object sender, EventArgs e)
        {
            TestItemDescription description = DataCenter._sysSetting.SpecCtrl.ItemDescription[ETestType.IO.ToString()];

            frmIODefault frmIOCali = new frmIODefault(DataCenter._machineConfig.IOConfig, description);
            frmIOCali.ShowDialog();
            frmIOCali.Dispose();

            frmIOCali.Close();
        }

        #region >>> Test Item Spec Ctrl <<<

        private void InitTestItemSpecList()
        {
            if (DataCenter._rdFunc.RDFuncData.SpecDataDefinition == null || DataCenter._rdFunc.RDFuncData.SpecDataDefinition.Count == 0)
            {
                this._tempSpecDefinition = new TestItemDescriptionCollections();

                // Create Default Items
                //foreach (var keyName in DataCenter._sysSetting.SpecCtrl.TesterDefaultItems)
                //{
                //    this._tempSpecDefinition.Add(DataCenter._sysSetting.SpecCtrl.GetDefaultDescription(keyName.ToString()));
                //}
            }
            else
            {
                this._tempSpecDefinition = DataCenter._rdFunc.RDFuncData.SpecDataDefinition.Clone() as TestItemDescriptionCollections;
            }

            this.UpdateEnableItemList();

            this.UpdateTestItemSpecDgv(null);
        }

        private void UpdateEnableItemList()
        {
            this.lstBoxEnableTestItems.Items.Clear();

            if (this._tempSpecDefinition != null)
            {
                if (this._tempSpecDefinition.Count != 0)
                {
                    foreach (var item in this._tempSpecDefinition.Items)
                    {
                        this.lstBoxEnableTestItems.Items.Add(item.KeyName);
                    }
                }
            }
        }

        private void UpdateTestItemSpecDgv(string keyName)
        {
            List<ItemDescriptionBase> lstDescBase = null;

            int rowCount = 0;

            this.lblSettingInfo.Text = string.Empty;

            this.dgvItemBoundary.SuspendLayout();

            this.dgvItemBoundary.Rows.Clear();

            if (keyName != null)
            {
                if (this._tempSpecDefinition.Count != 0)
                {
                    if (this._tempSpecDefinition.ContainsKeyName(keyName))
                    {
                        // reSpec 找得到 KeyName, 資料從 RdData Clone 出來的 tempSpec 取得
                        lstDescBase = this._tempSpecDefinition[keyName].Property;
                    }
                }

                if (lstDescBase != null)
                {
                    foreach (var data in lstDescBase)
                    {
                        this.dgvItemBoundary.Rows.Add();

						this.dgvItemBoundary.Rows[rowCount].Cells["colIsVisible"].Value = data.IsVisible;
                        this.dgvItemBoundary.Rows[rowCount].Cells["colpropKeyName"].Value = data.PropertyKeyName;
                        this.dgvItemBoundary.Rows[rowCount].Cells["colpropMin"].Value = data.MinValue.ToString(data.Format);
                        this.dgvItemBoundary.Rows[rowCount].Cells["colpropMax"].Value = data.MaxValue.ToString(data.Format);
                        this.dgvItemBoundary.Rows[rowCount].Cells["colpropDefault"].Value = data.DefaultValue.ToString(data.Format);
                        this.dgvItemBoundary.Rows[rowCount].Cells["colpropUnit"].Value = data.Unit;
                        this.dgvItemBoundary.Rows[rowCount].Cells["colpropFormat"].Value = data.Format;

                        rowCount++;
                    }
                }

                if (keyName.Contains("PIV"))
                {
                    this.lblSettingInfo.Text = "OperationMethod [0] Closest [1] interp.; SE/RS Method [0] LR [1] 2Point";
                }
            }

            this.dgvItemBoundary.ResumeLayout();
        }

        private void btnEditEnableItems_Click(object sender, EventArgs e)
        {
            frmSetRDFuncTestItems frm = new frmSetRDFuncTestItems();

            frm.TopLevel = true;

            // Init frm Setting
            if (this._tempSpecDefinition.TestTypeKeyNames.Length != 0)
            {
                frm.UpdateItems(this._tempSpecDefinition.TestTypeKeyNames);
            }
            else
            {
                frm.UpdateItems(DataCenter._sysSetting.SpecCtrl.GetDefaultItems((ETesterConfigType)this.cmbTesterConfigType.SelectedItem));
            }

            DialogResult result = frm.ShowDialog();

            if (result == DialogResult.OK)
            {
                // Update Item Enable List
                string[] rtnItems = frm.GetItems();

                if (rtnItems.Length != 0)
                {
                    // 更新 this._tempSpecDefinition
                    TestItemDescriptionCollections cloneobj = new TestItemDescriptionCollections();

                    foreach (var strKey in rtnItems)
                    {
                        cloneobj.Add(DataCenter._sysSetting.SpecCtrl.GetDefaultDescription(strKey));

                        if (this._tempSpecDefinition.ContainsKeyName(strKey))
                        {
                            cloneobj.OverWrite(strKey, this._tempSpecDefinition[strKey].Clone() as TestItemDescription);
                        }
                    }

                    this._tempSpecDefinition = cloneobj.Clone() as TestItemDescriptionCollections;

                    cloneobj = null;
                }

                this.UpdateEnableItemList();

                this.UpdateTestItemSpecDgv(null);
            }

            frm.Dispose();
        }

        private void lstEnableTestItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lstBoxEnableTestItems.SelectedIndex >= 0)
            {
                string keyName = this.lstBoxEnableTestItems.SelectedItem.ToString();
                
                this.UpdateTestItemSpecDgv(keyName);

                this.lblItemBoundaryTitle.Text = string.Format("{0} Boundary Property", keyName);
            }
            else
            {
                this.lblItemBoundaryTitle.Text = "Test Item Boundary Property";
            }
        }

        private void dgvItemBoundary_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (this.lstBoxEnableTestItems.SelectedIndex < 0 || e.RowIndex < 0 || e.ColumnIndex < 1)
            {
                return;
            }

            bool isValid = true;
            
            string keyName = string.Empty;
            string propKeyName = string.Empty;
			bool isVisible = true;
            double minValue = 0.0d;
            double maxValue = 0.0d;
            double defaultValue = 0.0d;
            string unit = string.Empty;
            string format = string.Empty;

            int digit = 0;

            DataGridViewRow dgvRow = this.dgvItemBoundary.Rows[e.RowIndex];

            //-------------------------------------------------------------------------------------------------------------------------
            keyName = this.lstBoxEnableTestItems.SelectedItem.ToString();

            propKeyName = dgvRow.Cells["colpropKeyName"].Value.ToString();

			isVisible = (bool)dgvRow.Cells["colIsVisible"].Value;

            unit = dgvRow.Cells["colpropUnit"].Value.ToString();

            // Change Format
            format = dgvRow.Cells["colpropFormat"].Value.ToString();

            int pointPos = format.IndexOf('.');

            if (pointPos >= 0)
            {
                digit = format.Length - pointPos - 1;
            }

            format = "0.";

            for (int i = 0; i < digit; i++)
            {
                format += "0";
            }

            // Convert Min / Max / Default
            if (double.TryParse(dgvRow.Cells["colpropMin"].Value.ToString(), out minValue) &&
                double.TryParse(dgvRow.Cells["colpropMax"].Value.ToString(), out maxValue) &&
                double.TryParse(dgvRow.Cells["colpropDefault"].Value.ToString(), out defaultValue))
            {
                if (maxValue < minValue)
                {
                    MessageBox.Show("maxValue < minValue", "Input Value Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    isValid = false;
                }
                else
                {
                    if (defaultValue > maxValue || defaultValue < minValue)
                    {
                        defaultValue = minValue;
                    }
                }

                minValue = Math.Round(minValue, digit, MidpointRounding.AwayFromZero);
                maxValue = Math.Round(maxValue, digit, MidpointRounding.AwayFromZero);
                defaultValue = Math.Round(defaultValue, digit, MidpointRounding.AwayFromZero);

				dgvRow.Cells["colIsVisible"].Value = isVisible;
                dgvRow.Cells["colpropMin"].Value = minValue.ToString("F" + digit.ToString());
                dgvRow.Cells["colpropMax"].Value = maxValue.ToString("F" + digit.ToString());
                dgvRow.Cells["colpropDefault"].Value = defaultValue.ToString("F" + digit.ToString());
                dgvRow.Cells["colpropFormat"].Value = format;
            }
            else
            {
                MessageBox.Show("Numeric", "Input Value Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                isValid = false;
            }

            //-------------------------------------------------------------------------------------------------------------------------
            if (isValid)
            {
                ItemDescriptionBase data = new ItemDescriptionBase();

				data.IsVisible = isVisible;
                data.PropertyKeyName = propKeyName;
                data.MaxValue = maxValue;
                data.MinValue = minValue;
                data.DefaultValue = defaultValue;
                data.Unit = unit;
                data.Format = format;

                if (this._tempSpecDefinition[keyName].ContainsKeyName(propKeyName))
                {
                this._tempSpecDefinition[keyName][propKeyName].OverWrite(data);
            }
            }
            else
            {
                // 不合理的設定, 回覆到原本設定
                dgvRow.Cells["colpropMin"].Value = this._tempSpecDefinition[keyName][propKeyName].MinValue.ToString("F" + digit.ToString());
                dgvRow.Cells["colpropMax"].Value = this._tempSpecDefinition[keyName][propKeyName].MaxValue.ToString("F" + digit.ToString());
                dgvRow.Cells["colpropDefault"].Value = this._tempSpecDefinition[keyName][propKeyName].DefaultValue.ToString("F" + digit.ToString());
                dgvRow.Cells["colpropUnit"].Value = this._tempSpecDefinition[keyName][propKeyName].Unit;
                dgvRow.Cells["colpropFormat"].Value = this._tempSpecDefinition[keyName][propKeyName].Format;
            }
        }

        private void btnResetSelectedItem_Click(object sender, EventArgs e)
        {
            if (this.lstBoxEnableTestItems.SelectedIndex >= 0)
            {
                string keyName = this.lstBoxEnableTestItems.SelectedItem.ToString();

                this._tempSpecDefinition.OverWrite(keyName, DataCenter._sysSetting.SpecCtrl.GetDefaultDescription(keyName));

                this.UpdateTestItemSpecDgv(keyName);
            }
        }

        #endregion

    }
}
