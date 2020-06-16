using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MPI.Tester.Data;
using MPI.Tester.Tools;
using System.IO;

namespace MPI.Tester.Gui
{
    public partial class frmDeviceVerify : Form
    {
        private string _sysTaskFileName;

        private DeviceVerifyCtrl _dvCtrl;

        public frmDeviceVerify()
        {
            InitializeComponent();

            this._sysTaskFileName = string.Empty;

            this._dvCtrl = new DeviceVerifyCtrl();
        }

        #region >>> Private Method <<<

        private void LoadTaskFileName(string fileName)
        {
            if (fileName == string.Empty)
            {
                return;
            }
            
            GlobalFlag.IsSuccessCheckFilterWheel = true;

            DataCenter.LoadTaskSheet(fileName);

            WMOperate.WM_ReadCalibrateParamFromSetting();

            AppSystem.SetDataToSystem();

            AppSystem.CheckMachineHW();

            Host.UpdateDataToAllUIForm();
        }

        private void UpdateDataToControls()
        {
            this.lblDeviceVerifyResult.Text = "NONE";
            this.lblDeviceVerifyResult.BackColor = Color.AntiqueWhite;
            
            this.UpdateDataToUI();

            this.UpdateDataToCoefSettingDGV();

            this.UpdateInfoToResultDGV();
        }

        private void UpdateDataToUI()
        {
            this.cmbTaskSheet.Items.Clear();
            this.cmbTaskSheet.Items.AddRange(DataCenter.GetAllFilesList(DataCenter._uiSetting.ProductPath, Constants.Files.TASK_SHEET_EXTENSION));

            this.dgvDutSetting.Rows.Clear();

            for (int i = 0; i < DataCenter._toolConfig.DeviceVerify.Channel.Count; i++)
            {
                this.dgvDutSetting.Rows.Add();

                this.dgvDutSetting.Rows[i].Cells[0].Value = (i + 1).ToString();
                this.dgvDutSetting.Rows[i].Cells[1].Value = (bool)DataCenter._toolConfig.DeviceVerify.Channel[i].IsEnable;
                this.dgvDutSetting.Rows[i].Cells[2].Value = (bool)DataCenter._toolConfig.DeviceVerify.Channel[i].IsStdCheck;
                this.dgvDutSetting.Rows[i].Cells[3].Value = (bool)DataCenter._toolConfig.DeviceVerify.Channel[i].IsMsrtCheck;
                this.dgvDutSetting.Rows[i].Cells[4].Value = DataCenter._toolConfig.DeviceVerify.Channel[i].Description.ToString();
            }

            if (DataCenter._toolConfig.DeviceVerify.Channel.STDChannel != -1)
            {
                this.txtStdSmdName.Text = string.Format("SMD{0}", (DataCenter._toolConfig.DeviceVerify.Channel.STDChannel + 1));
            }
            else
            {
                this.txtStdSmdName.Text = "NONE";
            }

            if (this.cmbTaskSheet.Items.Contains(DataCenter._toolConfig.DeviceVerify.TaskSheetFileName))
            {
                this.cmbTaskSheet.SelectedItem = DataCenter._toolConfig.DeviceVerify.TaskSheetFileName;

                this.txtRecipeFileName.Text = DataCenter._toolConfig.DeviceVerify.TaskSheetFileName;

                // Load TaskFile
                this.LoadTaskFileName(DataCenter._toolConfig.DeviceVerify.TaskSheetFileName);

                this._dvCtrl.Update(DataCenter._product.TestCondition.Clone() as ConditionData);
            }

            this.txtOpticalINT.Text = string.Empty;

            this.txtFilterPos.Text = (DataCenter._product.ProductFilterWheelPos + 1).ToString();

            this.txtBiasRegisterDate.Text = this._dvCtrl.Data.BiasRegisterDate.ToString("yyyy/MM/dd HH:mm");

            // Setting Parameters
            this.numRepeatCount.Value = (int)DataCenter._toolConfig.DeviceVerify.TestRepeatCount;

            this.numRepeatDelay.Value = (int)DataCenter._toolConfig.DeviceVerify.TestRepeatDelay;

            // Setting Path
            this.txtDailyCheckBiasDir.Text = DataCenter._toolConfig.DeviceVerify.BiasValueFilePath;

            this.txtDailyCheckResultDir.Text = DataCenter._toolConfig.DeviceVerify.ResultFilePath;
        }

        private void UpdateDataToCoefSettingDGV()
        {
            this.dgvCoefSetting.Rows.Clear();

            if (this._dvCtrl.Data == null || this._dvCtrl.Data.Count == 0)
            {
                return;
            }

            string keyName = string.Empty;

            this.dgvCoefSetting.SuspendLayout();

            for (int rowCount = 0; rowCount < this._dvCtrl.Data.Count; rowCount++)
            {
                this.dgvCoefSetting.Rows.Add();

                DataGridViewRow row = this.dgvCoefSetting.Rows[rowCount];

                keyName = this._dvCtrl.Data[rowCount].KeyName;

                row.Cells[0].Value = (rowCount + 1).ToString();                  // No.
                row.Cells[1].Value = (bool)this._dvCtrl.Data[rowCount].IsEnable; // Enable
                row.Cells[2].Value = this._dvCtrl.DisplayName[rowCount];         // Name
                row.Cells[3].Value = this._dvCtrl.Data[rowCount].Type;           // Type
                row.Cells[4].Value = this._dvCtrl.Data[keyName].Bias;            // bias
                row.Cells[5].Value = this._dvCtrl.Data[keyName].Spec;            // spec
                row.Cells[6].Value = this._dvCtrl.Data[keyName].Unit;            // unit
                row.Cells[7].Value = keyName;                                    // keyName
            }

            this.dgvCoefSetting.ResumeLayout();
        }

        private void UpdateInfoToResultDGV()
        {
            this.dgvResult.Rows.Clear();

            if (this._dvCtrl.Data == null || this._dvCtrl.Data.Count == 0)
            {
                return;
            }

            string keyName = string.Empty;

            this.dgvResult.SuspendLayout();

            for (int index = 0; index < this._dvCtrl.Data.Count; index++)
            {
                if (!this._dvCtrl.Data[index].IsEnable)
                {
                    continue;
                }

                this.dgvResult.Rows.Add();

                DataGridViewRow row = this.dgvResult.Rows[this.dgvResult.Rows.Count - 1];

                keyName = this._dvCtrl.Data[index].KeyName;

                row.Cells["colResultKeyName"].Value = keyName;
                row.Cells["colResultItem"].Value = this._dvCtrl.DisplayName[index];
                row.Cells["colResultDescrip"].Value = this._dvCtrl.DisplayCondition[index];

                row.Cells["colResultBias"].Value = this._dvCtrl.Data[keyName].Bias.ToString(this._dvCtrl.Data[keyName].Format);
                row.Cells["colResultCoefSpec"].Value = this._dvCtrl.Data[keyName].Spec;

                row.Cells["colResultCoefType"].Value = (int)this._dvCtrl.Data[index].Type;
                row.Cells["colResultGain"].Value = this._dvCtrl.Data[index].Gain;
                row.Cells["colResultOffset"].Value = this._dvCtrl.Data[index].Offset;
            }

            this.txtOpticalINT.Text = this._dvCtrl.OpticalCount.ToString();

            this.dgvResult.ResumeLayout();
        }

        private void UpdateDataToResultDGV(uint channel)
        {
            if (this._dvCtrl.Data == null || this._dvCtrl.Data.Count == 0)
            {
                return;
            }

            string keyName = string.Empty;

            this.dgvResult.SuspendLayout();

            foreach (DataGridViewRow row in this.dgvResult.Rows)
            {
                keyName = row.Cells["colResultKeyName"].Value.ToString();

                string colMsrtKey = string.Format("colResultValue{0}", channel + 1);
                string colDiffKey = string.Format("colResultDiff{0}", channel + 1);
                string colPFKey = string.Format("colResultPF{0}", channel + 1);

                row.Cells[colMsrtKey].Value = this._dvCtrl.Data[keyName].ResultAverageValue.ToString(this._dvCtrl.Data[keyName].Format);

                row.Cells[colDiffKey].Value = this._dvCtrl.Data[keyName].Diff.ToString(this._dvCtrl.Data[keyName].Format);

                row.Cells["colResultGain"].Value = this._dvCtrl.Data[keyName].Gain;
                row.Cells["colResultOffset"].Value = this._dvCtrl.Data[keyName].Offset;

                if (this._dvCtrl.Data[keyName].IsPass)
                {
                    row.Cells[colPFKey].Value = "Pass";
                    row.Cells[colPFKey].Style.BackColor = Color.Empty;
                }
                else
                {
                    row.Cells[colPFKey].Value = "NG";
                    row.Cells[colPFKey].Style.BackColor = Color.Pink;
                }

            }

            this.dgvResult.ResumeLayout();
        }

        private string SelectPath(string title)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            folderBrowserDialog.Description = title;

            folderBrowserDialog.SelectedPath = DataCenter._toolConfig.DCheck.StdFileDir;

            //   folderBrowserDialog.RootFolder = Environment.SpecialFolder.us;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                return folderBrowserDialog.SelectedPath;
            }
            else
            {
                return string.Empty;
            }

        }

        private void ClearDataToResultDGV()
        {
            if (this._dvCtrl.Data == null || this._dvCtrl.Data.Count == 0)
            {
                return;
            }

            this.dgvResult.SuspendLayout();

            foreach (DataGridViewRow row in this.dgvResult.Rows)
            {
                row.Cells["colResultValue1"].Value = "";
                row.Cells["colResultDiff1"].Value = "";
                row.Cells["colResultPF1"].Value = "";
                row.Cells["colResultPF1"].Style.BackColor = Color.Empty;

                row.Cells["colResultValue2"].Value = "";
                row.Cells["colResultDiff2"].Value = "";
                row.Cells["colResultPF2"].Value = "";
                row.Cells["colResultPF2"].Style.BackColor = Color.Empty;

                row.Cells["colResultValue3"].Value = "";
                row.Cells["colResultDiff3"].Value = "";
                row.Cells["colResultPF3"].Value = "";
                row.Cells["colResultPF3"].Style.BackColor = Color.Empty;

                row.Cells["colResultValue4"].Value = "";
                row.Cells["colResultDiff4"].Value = "";
                row.Cells["colResultPF4"].Value = "";
                row.Cells["colResultPF4"].Style.BackColor = Color.Empty;
            }

            this.dgvResult.ResumeLayout();
        }
        
        #endregion

        #region >>> UI Event Handler <<<

        private void frmDeviceVerify_Load(object sender, EventArgs e)
        {
            // Form Load, 讀取 DeviceVerify 的 TaskFileName, 並紀錄生產用的TaskFileName
            
            this._sysTaskFileName = DataCenter._uiSetting.TaskSheetFileName;

            this.tabcChartInfo.SelectedTabIndex = 0;

            this.UpdateDataToControls();
        }

        private void frmDeviceVerify_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Form Colsed, 讀回原本生產的TaskFileName

            this.LoadTaskFileName(this._sysTaskFileName);
        }

        private void btnRunCheck_Click(object sender, EventArgs e)
        {
            if (DataCenter._toolConfig.DeviceVerify.Channel.STDChannel != -1)
            {
                this._dvCtrl.StdSMDChannel = (uint)DataCenter._toolConfig.DeviceVerify.Channel.STDChannel;
            }
            else
            {
                return;
            }

            int[] msrtChannels = DataCenter._toolConfig.DeviceVerify.Channel.VerifyChannel;

            if (msrtChannels == null)
            {
                return;
            }

            DialogResult result = TopMessageBox.ShowMessage((int)EMessageCode.Tools_RunDeviceVerifySquence, "Auto Run Device Verify Sequence ? ", "  Device Verify");

            if (result != DialogResult.OK)
                return;

            AppSystem.SetDataToSystem();

            uint channel = 0;
            bool isAllPass = true;
            int verifyChIndex = 0;

            // Reset P/F
            this.ClearDataToResultDGV();

            this._dvCtrl.StartHistoryRecord();

            for (int seqOrder = 0; seqOrder < DataCenter._toolConfig.DeviceVerify.Channel.Count; seqOrder++)
            {
                if (seqOrder == 0)
                {
                    // STD channel
                    channel = this._dvCtrl.StdSMDChannel;

                    if (AppSystem.RunDeviceVerify(channel, DataCenter._toolConfig.DeviceVerify.TestRepeatCount, DataCenter._toolConfig.DeviceVerify.TestRepeatDelay))
                    {
                        this._dvCtrl.MsrtResltCalculate(channel, AppSystem._bodyDataHeadDic, AppSystem._bodyDataList, SpecCalculatingMode.CreateNewCoef);

                        isAllPass &= this._dvCtrl.IsPass;

                        if (this._dvCtrl.IsPass)
                        {
                            this._dvCtrl.SetCoefficients();

                            this.UpdateDataToResultDGV(channel);
                        }
                        else
                        {
                            this.UpdateDataToResultDGV(channel);

                            TopMessageBox.Show("STD SMD Measuring out off spec", "WARMING");

                            break;
                        }
                    }
                    else
                    {
                        Host.SetErrorCode(EErrorCode.System_Not_Ready);

                        return;
                    }
                }
                else
                {
                    // Verify channel
                    if (verifyChIndex < msrtChannels.Length)
                    {
                        channel = (uint)msrtChannels[verifyChIndex];

                        verifyChIndex++;
                    }
                    else
                    {
                        break;
                    }

                    if (AppSystem.RunDeviceVerify(channel, DataCenter._toolConfig.DeviceVerify.TestRepeatCount, DataCenter._toolConfig.DeviceVerify.TestRepeatDelay))
                    {
                        this._dvCtrl.MsrtResltCalculate(channel, AppSystem._bodyDataHeadDic, AppSystem._bodyDataList, SpecCalculatingMode.ApplyCoef);

                        isAllPass &= this._dvCtrl.IsPass;

                        this.UpdateDataToResultDGV(channel);
                    }
                    else
                    {
                        Host.SetErrorCode(EErrorCode.System_Not_Ready);

                        return;
                    }
                }

                this._dvCtrl.AddHistoryData();
            }

            string fileName = string.Format("{0}_{1}.csv", DataCenter._toolConfig.DeviceVerify.TaskSheetFileName, DateTime.Now.ToString("yyyy_MM_dd_HH_mm"));

            this._dvCtrl.SaveHistoryDataToFile(DataCenter._toolConfig.DeviceVerify.ResultFilePath, fileName);

            if (isAllPass)
            {
                this.lblDeviceVerifyResult.Text = "PASS";
                this.lblDeviceVerifyResult.BackColor = Color.Green;

                this._dvCtrl.Save();
            }
            else
            {
                this.lblDeviceVerifyResult.Text = "NG";
                this.lblDeviceVerifyResult.BackColor = Color.Red;
            }
        }

        private void btnBiasRegister_Click(object sender, EventArgs e)
        {
            DialogResult result = TopMessageBox.ShowMessage((int)EMessageCode.Tools_RunDeviceVerifyBiasRegister, "Reset the Sub-Coef and Run Bias Value Registration Sequence ?", "  Registration");

            if (result != DialogResult.OK)
                return;

            if (DataCenter._toolConfig.DeviceVerify.Channel.STDChannel != -1)
            {
                AppSystem.SetDataToSystem();

                uint channel = (uint)DataCenter._toolConfig.DeviceVerify.Channel.STDChannel;

                if (AppSystem.RunDeviceVerify(channel, DataCenter._toolConfig.DeviceVerify.TestRepeatCount, DataCenter._toolConfig.DeviceVerify.TestRepeatDelay))
                {
                    this._dvCtrl.ResetCoefficients();

                    this._dvCtrl.MsrtResltCalculate(channel, AppSystem._bodyDataHeadDic, AppSystem._bodyDataList, SpecCalculatingMode.BiasRegister);

                    this._dvCtrl.Data.BiasRegisterDate = DateTime.Now;

                    this._dvCtrl.Save();

                    this.txtBiasRegisterDate.Text = this._dvCtrl.Data.BiasRegisterDate.ToString("yyyy/MM/dd HH:mm");

                    this.UpdateInfoToResultDGV();

                    this.UpdateDataToCoefSettingDGV();
                }
                else
                {
                    Host.SetErrorCode(EErrorCode.System_Not_Ready);

                    return;
                }
            }
        }

        private void btnCombineCoef_Click(object sender, EventArgs e)
        {
            DialogResult result = TopMessageBox.ShowMessage((int)EMessageCode.Tools_CombineSysCoef, "Would you Combine sub coef.to System Coef.?", "Combine System Coef");
           
            if (result != DialogResult.OK)
                return;

            UILog.Log(this, sender, "btnCombineCoef_Click");
            
            SystemCaliData sysCalData = DataCenter._sysCali.SystemCaliData;
            
            for (int i = 0; i < this._dvCtrl.Data.Count; i++)
            {
                string keyName = this._dvCtrl.Data[i].KeyName;

                if (sysCalData.ContainsKey(keyName))
                {
                    sysCalData[keyName].IsEnable = this._dvCtrl.Data[i].IsEnable;

                    sysCalData[keyName].Name = this._dvCtrl.Data[i].Name;

                    sysCalData[keyName].Type = this._dvCtrl.Data[i].Type;

                    sysCalData[keyName].Gain = this._dvCtrl.Data[i].Gain;

                    sysCalData[keyName].Offset = this._dvCtrl.Data[i].Offset;
                }
                else
                {
                    GainOffsetData newData = new GainOffsetData(this._dvCtrl.Data[i].IsEnable, this._dvCtrl.Data[i].Type);

                    newData.KeyName = keyName;

                    newData.Name = this._dvCtrl.Data[i].Name;

                    newData.Gain = this._dvCtrl.Data[i].Gain;

                    newData.Offset = this._dvCtrl.Data[i].Offset;

                    sysCalData.ToolFactor.Add(newData);
                }
            }

            DataCenter.SaveSystemCali();

            this.Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            DataCenter._toolConfig.DeviceVerify.TaskSheetFileName = this.cmbTaskSheet.SelectedItem.ToString();

            DataCenter.SaveToolsConfig();

            this.UpdateDataToControls();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DataCenter._toolConfig.DeviceVerify.TaskSheetFileName = this.cmbTaskSheet.SelectedItem.ToString();
            
            string keyName = string.Empty;
            double bias = 0.0d;
            double spec = 0.0d;

            for (int rowCount = 0; rowCount < this.dgvCoefSetting.Rows.Count; rowCount++)
            {
                keyName = this.dgvCoefSetting[7, rowCount].Value.ToString();

                foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
                {
                    if (item.GainOffsetSetting == null || item.GainOffsetSetting.Length == 0)
                    {
                        continue;
                    }

                    foreach (TestResultData result in item.MsrtResult)
                    {
                        if (result.KeyName == keyName)
                        {
                            result.IsEnable = (bool)this.dgvCoefSetting[1, rowCount].Value;

                            this._dvCtrl.Data[keyName].IsEnable = (bool)this.dgvCoefSetting[1, rowCount].Value;
                            this._dvCtrl.Data[keyName].Type = (EGainOffsetType)Enum.Parse(typeof(EGainOffsetType), this.dgvCoefSetting[3, rowCount].Value.ToString(), true); 
                        }
                    }
                }

                double.TryParse(this.dgvCoefSetting[4, rowCount].Value.ToString(), out bias);
                double.TryParse(this.dgvCoefSetting[5, rowCount].Value.ToString(), out spec);

                this._dvCtrl.Data[keyName].Bias = bias;
                this._dvCtrl.Data[keyName].Spec = spec;
            }

            // Setting Parameters
            DataCenter._toolConfig.DeviceVerify.TestRepeatCount = (uint)this.numRepeatCount.Value;

            DataCenter._toolConfig.DeviceVerify.TestRepeatDelay = (uint)this.numRepeatDelay.Value;

            for (int rowCount = 0; rowCount < this.dgvDutSetting.Rows.Count; rowCount++)
            {
                DataCenter._toolConfig.DeviceVerify.Channel[rowCount].IsEnable = (bool)this.dgvDutSetting[1, rowCount].Value;
                DataCenter._toolConfig.DeviceVerify.Channel[rowCount].IsStdCheck = (bool)this.dgvDutSetting[2, rowCount].Value;
                DataCenter._toolConfig.DeviceVerify.Channel[rowCount].IsMsrtCheck = (bool)this.dgvDutSetting[3, rowCount].Value;
                DataCenter._toolConfig.DeviceVerify.Channel[rowCount].Description = this.dgvDutSetting[4, rowCount].Value.ToString();
            }

            // Setting Path
            DataCenter._toolConfig.DeviceVerify.BiasValueFilePath = this.txtDailyCheckBiasDir.Text;

            DataCenter._toolConfig.DeviceVerify.ResultFilePath = this.txtDailyCheckResultDir.Text;

            this._dvCtrl.Save();

            DataCenter.SaveToolsConfig();

            DataCenter._conditionCtrl.CopyTestItemArrayToEachChannel();

            DataCenter.SaveProductFile();

            this.UpdateDataToControls();
        }

        private void btnSettingDailyCheckBias_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("");

            if (path != string.Empty)
            {
                this.txtDailyCheckBiasDir.Text = path;
            }
        }

        private void btnSettingDailyCheckResult_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("");

            if (path != string.Empty)
            {
                this.txtDailyCheckResultDir.Text = path;
            }
        }

        private void dgvDutSetting_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 1)
                return;

            bool isChecked = false;

            if (e.ColumnIndex == 1)  // Enable
            {
                isChecked = (bool)dgvDutSetting[e.ColumnIndex, e.RowIndex].Value;

                dgvDutSetting[e.ColumnIndex, e.RowIndex].Value = !isChecked;

                if ((bool)dgvDutSetting[e.ColumnIndex, e.RowIndex].Value == false)
                {
                    dgvDutSetting[2, e.RowIndex].Value = false;
                    dgvDutSetting[3, e.RowIndex].Value = false;
                }
            }
            else if (e.ColumnIndex == 2)  // STD
            {
                if ((bool)dgvDutSetting[1, e.RowIndex].Value == false)
                {
                    dgvDutSetting[e.ColumnIndex, e.RowIndex].Value = false;

                    return;
                }

                isChecked = (bool)dgvDutSetting[e.ColumnIndex, e.RowIndex].Value;

                dgvDutSetting[e.ColumnIndex, e.RowIndex].Value = !isChecked;

                if ((bool)dgvDutSetting[e.ColumnIndex, e.RowIndex].Value == true)
                {
                    dgvDutSetting[3, e.RowIndex].Value = false;

                    for (int rowCnt = 0; rowCnt < dgvDutSetting.Rows.Count; rowCnt++)
                    {
                        if (rowCnt != e.RowIndex)
                        {
                            dgvDutSetting[e.ColumnIndex, rowCnt].Value = false;
                        }
                    }                 
                }
            }
            else if (e.ColumnIndex == 3)  // STD
            {
                if ((bool)dgvDutSetting[1, e.RowIndex].Value == false || (bool)dgvDutSetting[2, e.RowIndex].Value == true)
                {
                    dgvDutSetting[e.ColumnIndex, e.RowIndex].Value = false;

                    return;
                }

                isChecked = (bool)dgvDutSetting[e.ColumnIndex, e.RowIndex].Value;

                dgvDutSetting[e.ColumnIndex, e.RowIndex].Value = !isChecked;
            }
        }

        #endregion
    }
}
