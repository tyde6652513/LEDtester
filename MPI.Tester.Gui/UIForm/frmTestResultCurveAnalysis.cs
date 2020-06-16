using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Gui
{
    public partial class frmTestResultCurveAnalysis : Form
    {
        private delegate void UpdateDataHandler();
        private delegate void UpdateChartDataHandler();

        private frmSweepGraphComponent _frmSweep1;
        private frmSweepGraphComponent _frmSweep2;
        private frmSweepGraphComponent _frmSweep3;
        private frmSweepGraphComponent _frmSweep4;

        private frmLIVGraphComponent _frmLiv1;
        private frmLIVGraphComponent _frmLiv2;
        private frmPIVGraphComponent _frmPiv;
        private frmSpectrumGraphComponent _frmLDSpectrum;

        private frmLIVGraphComponent _frmTransistor;
        private frmSpectrumGraphComponent _frmTransistorSpectrum;

        private frmLCRGraphComponent _frmLCRSweep1;
        private frmLCRGraphComponent _frmLCRSweep2;

        public frmTestResultCurveAnalysis()
        {
            InitializeComponent();

            this._frmSweep1 = new frmSweepGraphComponent("IVSWEEP_1");
            this._frmSweep2 = new frmSweepGraphComponent("IVSWEEP_2");
            this._frmSweep3 = new frmSweepGraphComponent("IVSWEEP_3");
            this._frmSweep4 = new frmSweepGraphComponent("IVSWEEP_4");

            this._frmLiv1 = new frmLIVGraphComponent(ETestType.LIV);
            this._frmLiv2 = new frmLIVGraphComponent(ETestType.LIV);

            this._frmPiv = new frmPIVGraphComponent();

            this._frmLDSpectrum = new frmSpectrumGraphComponent(ETestType.LOPWL, "TrcaLD01");

            this._frmTransistor = new frmLIVGraphComponent(ETestType.TRANSISTOR);

            this._frmLCRSweep1 = new frmLCRGraphComponent(ETestType.LCRSWEEP);
            this._frmLCRSweep2 = new frmLCRGraphComponent(ETestType.LCRSWEEP);

            this._frmTransistorSpectrum = new frmSpectrumGraphComponent(ETestType.TRANSISTOR, string.Empty);

            this.IncludeChildForm();
        }

        #region >>> Private Method <<<

        private void IncludeChildForm()
        {
            // Sweep
            this.AttachFormToPanel(this._frmSweep1, this.pnlSweepGraph1);
            this.AttachFormToPanel(this._frmSweep2, this.pnlSweepGraph2);
            this.AttachFormToPanel(this._frmSweep3, this.pnlSweepGraph3);
            this.AttachFormToPanel(this._frmSweep4, this.pnlSweepGraph4);

            // LIV
            this.AttachFormToPanel(this._frmLiv1, this.pnlLIVGraph1);
            this.AttachFormToPanel(this._frmLiv2, this.pnlLIVGraph2);

            // PIV
            this.AttachFormToPanel(this._frmPiv, this.pnlPIVGraph);
            this.AttachFormToPanel(this._frmLDSpectrum, this.pnlLDSpectrumGraph);

            // Transistor
            this.AttachFormToPanel(this._frmTransistor, this.pnlTransistorGraph);
            this.AttachFormToPanel(this._frmTransistorSpectrum, this.pnlTransistorSpectrumGraph);

            // LCR
            this.AttachFormToPanel(this._frmLCRSweep1, this.pnlLCRGraph1);
            this.AttachFormToPanel(this._frmLCRSweep2, this.pnlLCRGraph2);
        }

        private void AttachFormToPanel(Form frm, Panel pnl)
        {
            frm.TopLevel = false;
            frm.Parent = pnl;
            frm.Dock = DockStyle.Fill;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Show();
        }

        private void EditTestItemData(string keyName)
        {
            if (keyName == string.Empty)
            {
                return;
            }
            
            int editIndex = -1;

            for (int i = 0; i < DataCenter._product.TestCondition.TestItemArray.Length; i++)
            {
                if (DataCenter._product.TestCondition.TestItemArray[i].KeyName == keyName)
                {
                    editIndex = i;

                    break;
                }
            }

            if (editIndex >= 0)
            {
                //FormAgent.ConditionItemSetting.DialogControl(EBtnActionMode.UpdateTestItem, editIndex);
                FormAgent.ConditionForm.ConditionTableCtrl(EBtnActionMode.UpdateTestItem, editIndex);

            }  
        }

        private void UpdateLIVSettingData()
        {
            if (this.cmbItemLIV.SelectedIndex < 0)
            {
                return;
            }

            if (DataCenter._product.TestCondition.TestItemArray == null || DataCenter._product.TestCondition.TestItemArray.Length == 0)
            {
                return;
            }

            foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
            {
                if (item is LIVTestItem)
                {
                    if (item.KeyName != this.cmbItemLIV.SelectedItem.ToString())
                    {
                        continue;
                    }

                    LIVTestItem livItem = (item as LIVTestItem);

                    this.dinForceDealyLIV.Value = livItem.LIVForceDelayTime;

                    this.dinForceTimeLIV.Value = livItem.LIVForceTime;

                    this.dinTurnOffTimeLIV.Value = livItem.LIVTurnOffTime;

                    this.dinStartValueLIV.Value = livItem.LIVStartValue;

                    this.dinStepValueLIV.Value = livItem.LIVStepValue;

                    this.numRiseCountLIV.Value = (int)livItem.LIVSweepPoints;

                    this.dinMsrtRangeLIV.Value = livItem.LIVMsrtProtection;

                    this.dinEndValueLIV.Value = livItem.LIVStopValue;

                    if (livItem.LIVMsrtType == DeviceCommon.EMsrtType.LIV)
                    {
                        this.lblStartValueUnitLIV.Text = "mA";
                        this.lblEndValueUnitLIV.Text = "mA";
                        this.lblStepValueUnitLIV.Text = "mA";

                        this.lblMsrtRangeUnitLIV.Text = "V";
                    }
                    else
                    {
                        this.lblStartValueUnitLIV.Text = "V";
                        this.lblEndValueUnitLIV.Text = "V";
                        this.lblStepValueUnitLIV.Text = "V";

                        this.lblMsrtRangeUnitLIV.Text = "mA";
                    }

                    return;
                }


            }
            
        }

        private void UpdateTransistorSettingData()
        {
            if (this.cmbItemTransistor.SelectedIndex < 0)
            {
                return;
            }

            if (DataCenter._product.TestCondition.TestItemArray == null || DataCenter._product.TestCondition.TestItemArray.Length == 0)
            {
                return;
            }

            foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
            {
                if (item is TransistorTestItem)
                {
                    if (item.KeyName != this.cmbItemTransistor.SelectedItem.ToString())
                    {
                        continue;
                    }

                    TransistorTestItem trItem = (item as TransistorTestItem);

                    this.dinForceDealyTR.Value = trItem.TRProcessDelayTime;
                    this.dinForceTimeTR.Value = trItem.TRForceTime;
                    this.dinTurnOffTimeTR.Value = trItem.TRTurnOffTime;
                    this.dinSweepPointsTR.Value = (int)trItem.TRSweepPoints;

                    foreach (var data in trItem.TRTerminalDescription)
                    {
                        string info = "None";
                        string strFunc = string.Empty;
                        string strSrcSetting = string.Empty;
                        string strMsrtSetting = string.Empty;
                        string strSMU = string.Empty;

                        if (data.SMU != ESMU.None)
                        {
                            strFunc = data.Description + "\n";
                            strSMU = data.SMU.ToString() + "\n";

                            if (data.MsrtType == EMsrtType.FIMVSWEEP || data.MsrtType == EMsrtType.FVMISWEEP)
                            {
                                if (data.SweepMode == ESweepMode.Linear)
                                {
                                    strSrcSetting = string.Format("Mode = {0}\nStart = {1} {2}\nStep = {3} {4}\nStop = {5} {6}\n", data.SweepMode.ToString(),
                                                                                                                                   data.SweepStart.ToString(), data.ForceUnit,
                                                                                                                                   data.SweepStep.ToString(), data.ForceUnit,
                                                                                                                                   data.SweepStop.ToString(), data.ForceUnit);
                                }
                                else
                                {
                                    strSrcSetting = string.Format("Mode = {0}\n", data.SweepMode.ToString());
                                }
                            }
                            else
                            {
                                strSrcSetting = string.Format("Force Value = {0} {1}\n", data.ForceValue, data.ForceUnit);
                            }

                            strMsrtSetting = string.Format("Msrt Clamp = {0} {1}\n", data.MsrtProtection, data.MsrtUnit);

                            info = strSMU + strFunc + strSrcSetting + strMsrtSetting;
                        }

                        switch (data.TerminalName)
                        {
                            case ETerminalName.Drain:
                                {
                                    this.txtDrain.Text = info;
                                    break;
                                }
                            case ETerminalName.Source:
                                {
                                    this.txtSource.Text = info;
                                    break;
                                }
                            case ETerminalName.Gate:
                                {
                                    this.txtGate.Text = info;
                                    break;
                                }
                            case ETerminalName.Bluk:
                                {
                                    this.txtBluk.Text = info;
                                    break;
                                }
                        }
                    }

                    return;
                }
            }
        }

        private void UpdateLCRSweepSettingData()
        {
            if (this.cmbItemLCR.SelectedIndex < 0)
            {
                return;
            }

            if (DataCenter._product.TestCondition.TestItemArray == null || DataCenter._product.TestCondition.TestItemArray.Length == 0)
            {
                return;
            }

            foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
            {
                if (item is LCRSweepTestItem)
                {
                    if (item.KeyName != this.cmbItemLCR.SelectedItem.ToString())
                    {
                        continue;
                    }

                    LCRSweepTestItem lcrItem = (item as LCRSweepTestItem);

                    this.dinWaitTime.Value = lcrItem.LCRSetting.MsrtDelayTime;

                    this.dinStepDelay.Value = lcrItem.LCRSetting.BiasDelay;

                    this.dinStart.Value = lcrItem.LCRSetting.DCBiasStart;

                    this.dinStep.Value = lcrItem.LCRSetting.DCBiasStep;

                    this.dinEnd.Value = lcrItem.LCRSetting.DCBiasEnd;

                    this.dinCount.Value = lcrItem.LCRSetting.Point;

                    if (lcrItem.LCRSetting.DCBiasMode == ELCRDCBiasMode.Voltage)
                    {
                        this.lblLCRStartUnit.Text = "V";
                        this.lblLCRStepUnit.Text = "V";
                        this.lblLCREndUnit.Text = "V";

                    }
                    else
                    {
                        this.lblLCRStartUnit.Text = "mA";
                        this.lblLCRStepUnit.Text = "mA";
                        this.lblLCREndUnit.Text = "mA";
                    }

                    return;
                }
            }
        }

        private void UpdateDataToControls()
        {
            if (!DataCenter._sysSetting.SpecCtrl.IsSupportedSweepItem)
            {
                this.tabiSweep.Visible = false;
            }

            if (!DataCenter._sysSetting.SpecCtrl.IsSupportedLIVItem )
            {
                this.tabiLIV.Visible = false;
            }

            if ( !DataCenter._sysSetting.SpecCtrl.IsSupportedLCRSweepItem)
            {
                this.tabiLCR.Visible = false;
            }

            if (!DataCenter._sysSetting.SpecCtrl.IsSupportedPIVItem)
            {
                this.tabiPIV.Visible = false;
            }

            if (!DataCenter._sysSetting.SpecCtrl.IsSupportedTransistorItem)
            {
                this.tabiTransistor.Visible = false;
            }

            //----------------------------------------------------------------------------------------------------------------------------------------------
            // Sweep
            this.pnlSweepGraph1.Visible = true;
            this.pnlSweepGraph2.Visible = false;
            this.pnlSweepGraph3.Visible = false;
            this.pnlSweepGraph4.Visible = false;

            if (DataCenter._acquireData.ElecSweepDataSet.Count == 2)
            {
                this.pnlSweepGraph2.Visible = true;
            }
            else if (DataCenter._acquireData.ElecSweepDataSet.Count == 3)
            {
                this.pnlSweepGraph2.Visible = true;
                this.pnlSweepGraph3.Visible = true;
            }
            else if (DataCenter._acquireData.ElecSweepDataSet.Count >= 4)
            {
                this.pnlSweepGraph2.Visible = true;
                this.pnlSweepGraph3.Visible = true;
                this.pnlSweepGraph4.Visible = true;
            }

            this._frmSweep1.UpdateDataToControl(DataCenter._acquireData.ElecSweepDataSet);
            this._frmSweep2.UpdateDataToControl(DataCenter._acquireData.ElecSweepDataSet);
            this._frmSweep3.UpdateDataToControl(DataCenter._acquireData.ElecSweepDataSet);
            this._frmSweep4.UpdateDataToControl(DataCenter._acquireData.ElecSweepDataSet);

            //----------------------------------------------------------------------------------------------------------------------------------------------
            // LD PIV / Spectrum
            this._frmPiv.UpdateDataToControl(DataCenter._acquireData.PIVDataSet);

            this._frmLDSpectrum.UpdateDataToControl(DataCenter._acquireData.SpectrumDataSet);

            //----------------------------------------------------------------------------------------------------------------------------------------------
            // LIV
            this.cmbItemLIV.Items.Clear();
            this.cmbItemTransistor.Items.Clear();
            this.cmbItemLCR.Items.Clear();

            foreach (var keyName in DataCenter._acquireData.LIVDataSet.KeyNames)
            {
                if (keyName.Contains(ETestType.LIV.ToString()))
                {
                    this.cmbItemLIV.Items.Add(keyName);
                }
                else if(keyName.Contains(ETestType.TRANSISTOR.ToString()))
                {
                    this.cmbItemTransistor.Items.Add(keyName);
                }
                else if (keyName.Contains(ETestType.LCRSWEEP.ToString()))
                {
                    this.cmbItemLCR.Items.Add(keyName);
                }
            }

            foreach (var keyName in DataCenter._acquireData.LCRDataSet.KeyNames)
            {
                if (keyName.Contains(ETestType.LCRSWEEP.ToString()))
                {
                    this.cmbItemLCR.Items.Add(keyName);
                }
            }

            if (this.cmbItemLIV.Items.Count > 0)
            {
                this.cmbItemLIV.SelectedIndex = 0;
                this._frmLiv1.UpdateDataToControl(DataCenter._acquireData.LIVDataSet);
                this._frmLiv2.UpdateDataToControl(DataCenter._acquireData.LIVDataSet);

                this.UpdateLIVSettingData();
            }



            //----------------------------------------------------------------------------------------------------------------------------------------------
            // Transistor
            if (this.cmbItemTransistor.Items.Count > 0)
            {
                this.cmbItemTransistor.SelectedIndex = 0;
                this._frmTransistor.UpdateDataToControl(DataCenter._acquireData.LIVDataSet);
                this._frmTransistorSpectrum.UpdateDataToControl(DataCenter._acquireData.LIVDataSet);

                this.UpdateTransistorSettingData();
            }
            //----------------------------------------------------------------------------------------------------------------------------------------------
            // LCR
            if (this.cmbItemLCR.Items.Count > 0)
            {
                this.cmbItemLCR.SelectedIndex = 0;
                this._frmLCRSweep1.UpdateDataToControl(DataCenter._acquireData.LCRDataSet);
                this._frmLCRSweep2.UpdateDataToControl(DataCenter._acquireData.LCRDataSet);

                this.UpdateLCRSweepSettingData();
            }
            


        }

        private void UpdateDataToChartComponent()
        {
            this._frmSweep1.UpdateDataToGraph(DataCenter._acquireData.ElecSweepDataSet);
            this._frmSweep2.UpdateDataToGraph(DataCenter._acquireData.ElecSweepDataSet);
            this._frmSweep3.UpdateDataToGraph(DataCenter._acquireData.ElecSweepDataSet);
            this._frmSweep4.UpdateDataToGraph(DataCenter._acquireData.ElecSweepDataSet);


            this._frmLiv1.UpdateDataToGraph(DataCenter._acquireData.LIVDataSet);
            this._frmLiv2.UpdateDataToGraph(DataCenter._acquireData.LIVDataSet);

            this._frmPiv.UpdateDataToGraph(DataCenter._acquireData.PIVDataSet);
            this._frmLDSpectrum.UpdateDataToGraph(DataCenter._acquireData.SpectrumDataSet);

			this._frmTransistor.UpdateDataToGraph(DataCenter._acquireData.LIVDataSet);
            this._frmTransistorSpectrum.UpdateDataToGraph(DataCenter._acquireData.LIVDataSet);

            this._frmLCRSweep1.UpdateDataToGraph(DataCenter._acquireData.LCRDataSet);
            this._frmLCRSweep2.UpdateDataToGraph(DataCenter._acquireData.LCRDataSet);

            this.UpdatePivResult();
        }

        private void UpdatePivResult()
        {
            if (!this.pnlPivResult.Visible)
            {
                return;
            }

            if (DataCenter._tempCond.TestItemArray == null || DataCenter._tempCond.TestItemArray.Length == 0)
            {
                return;
            }

            if(this._frmPiv.SelectedItem == null)
            {
                return;
            }

            foreach (var item in DataCenter._tempCond.TestItemArray)
            {
                if (item.KeyName == this._frmPiv.SelectedItem)
                {
                    this.dinDisplayIop.Value = item.MsrtResult[(int)ELaserMsrtType.Iop].Value;
                    this.dinDisplayVop.Value = item.MsrtResult[(int)ELaserMsrtType.Vop].Value;
                    this.dinDisplaySE.Value = item.MsrtResult[(int)ELaserMsrtType.SE].Value;
                    this.dinDisplayIth.Value = item.MsrtResult[(int)ELaserMsrtType.Ith].Value;
                    this.dinDisplayRs.Value = item.MsrtResult[(int)ELaserMsrtType.RS].Value;

                    break;
                }
            }
        }

        private void GetPIVRawdata(out List<string[]> lstPIVRawdata)
        {

            string[] strArry;
            lstPIVRawdata = new List<string[]>();

            lstPIVRawdata.Add(new string[] { "Lot No.:" + DataCenter._uiSetting.LotNumber });

            lstPIVRawdata.Add(new string[] { "Model No.:" + DataCenter._machineConfig.TesterModel });

            lstPIVRawdata.Add(new string[] { "Wafer No.:" + DataCenter._uiSetting.WaferNumber });

            foreach (var item in DataCenter._tempCond.TestItemArray)
            {
                if (item.KeyName == this._frmPiv.SelectedItem)
                {
                    lstPIVRawdata.Add(new string[] { "Condition:Pop(mW)=" + (item as PIVTestItem).CalcSetting.Pop 
                                                     + "/If(mA)=" + (item as PIVTestItem).CalcSetting.IfA});

                    lstPIVRawdata.Add(new string[] { "Date:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") });

                    lstPIVRawdata.Add(new string[] { "Ith(mA):" + item.MsrtResult[(int)ELaserMsrtType.Ith].Value});

                    lstPIVRawdata.Add(new string[] { "Pf(mW):" + item.MsrtResult[(int)ELaserMsrtType.PfA].Value });

                    lstPIVRawdata.Add(new string[] { "Iop(mW):" + item.MsrtResult[(int)ELaserMsrtType.Iop].Value });

                    lstPIVRawdata.Add(new string[] { "Rs(ohm):" + item.MsrtResult[(int)ELaserMsrtType.RS].Value });

                    lstPIVRawdata.Add(new string[] { "SE(W/A):" + item.MsrtResult[(int)ELaserMsrtType.SE].Value });

                    lstPIVRawdata.Add(new string[] { "-------------------------------------------" });

                    break;
                }
            }                       

            strArry = new string[] { " Curr.(mA)", "Power(mW)", "Volt.(V)", "SE(W/A)", "RS(ohm)" };

            lstPIVRawdata.Add(strArry);

            for (int i = 0; i < DataCenter._acquireData.PIVDataSet[0].CurrentData.Length; i++)
            {
                strArry = new string[] { DataCenter._acquireData.PIVDataSet[0].CurrentData[i].ToString(),
                                         DataCenter._acquireData.PIVDataSet[0].PowerData[i].ToString(),
                                         DataCenter._acquireData.PIVDataSet[0].VoltageData[i].ToString(),
                                         DataCenter._acquireData.PIVDataSet[0].SeData[i].ToString(),
                                         DataCenter._acquireData.PIVDataSet[0].RsData[i].ToString()};

                lstPIVRawdata.Add(strArry);
            }
        }

        #endregion

        #region >>> UI Ctrl <<<

        private void frmTestResultCurveAnalysis_Load(object sender, EventArgs e)
        {
            this.UpdateDataToControls();

            this.UpdateChartDataToUIForm(null, null);

			if (DataCenter._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Terminal)
			{
				this.btnEditTransistor.Visible = false;
			}
			else
			{
				this.btnEditTransistor.Visible = true;
			}
        }

        //private void btnExportToCSV_Click(object sender, EventArgs e)
        //{
        //    SaveFileDialog saveFileDialog = new SaveFileDialog();
        //    saveFileDialog.Title = "Export Data to File";
        //    saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
        //    saveFileDialog.FilterIndex = 1;    // default value = 1
        //    saveFileDialog.InitialDirectory = Constants.Paths.MPI_TEMP_DIR;

        //    saveFileDialog.FileName = DataCenter._uiSetting.TaskSheetFileName;

        //    if (saveFileDialog.ShowDialog() != DialogResult.OK)
        //        return;

        //    string path = Path.GetDirectoryName(saveFileDialog.FileName);

        //    string fileName = Path.GetFileName(saveFileDialog.FileName);

        //    if (this.tabpSweep.Visible)
        //    {
        //        AppSystem._outputBigData.SaveSweepData(path, fileName, AppSystem._outputBigData.OutputSweepList.Count - 1, 1);  
        //    }
        //    else if (this.tabpLIV.Visible)
        //    {
        //        AppSystem._outputBigData.SaveLIVData(path, fileName, AppSystem._outputBigData.OutputLIVList.Count - 1, 1);
        //    }
        //    else if (this.tabpPIV.Visible)
        //    {
        //        AppSystem._outputBigData.SavePIVData(path, fileName, AppSystem._outputBigData.OutputPIVList.Count - 1, 1);
        //    }
        //}

        private void btnEditLIV_Click(object sender, EventArgs e)
        {
            if (this.cmbItemLIV.SelectedIndex < 0)
            {
                return;
            }
            
            this.EditTestItemData(this.cmbItemLIV.SelectedItem.ToString());

            this.UpdateLIVSettingData();
        }

        private void btnEditTransistor_Click(object sender, EventArgs e)
        {
            if (this.cmbItemTransistor.SelectedIndex < 0)
            {
                return;
            }

            this.EditTestItemData(this.cmbItemTransistor.SelectedItem.ToString());

            this.UpdateTransistorSettingData();
        }

		private void cmbItemTransistor_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.UpdateTransistorSettingData();
		}

        private void btnViewPivCurve_Click(object sender, EventArgs e)
        {
            frmViewPIVGraph frm = new frmViewPIVGraph();

            frm.ShowDialog();

            frm.Dispose();

            frm.Close();
        }

        private void btnViewWlCurve_Click(object sender, EventArgs e)
        {
            frmViewWLGraph frm = new frmViewWLGraph();

            frm.ShowDialog();

            frm.Dispose();

            frm.Close();
        }

        private void btnSavePIVRawdata_Click(object sender, EventArgs e)
        {
            List<string[]> lstPIVdata = new List<string[]>();

            this.GetPIVRawdata(out lstPIVdata);

            SaveFileDialog fileDialog = new SaveFileDialog();

            fileDialog.InitialDirectory = Constants.Paths.MPI_TEMP_DIR2;
            fileDialog.Title = "Save PIV Data to";
            fileDialog.Filter = "CSV files (*.csv)|*.csv";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                this.UpdateLCRSweepSettingData();
            }
        }

        private void cmbItemLCR_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateLCRSweepSettingData();
        }

        #endregion

        #region >>> Public Method <<<

        public void UpdateDataToUIForm()
        {
            if (this.InvokeRequired && this.IsHandleCreated)
            {
                this.BeginInvoke(new UpdateDataHandler(UpdateDataToControls), null);		// Run at other TestServer Thread
            }
            else if (this.IsHandleCreated)
            {
                this.UpdateDataToControls();		// Run at Main Thread
            }
        }

        public void UpdateChartDataToUIForm(object sender, EventArgs e)
        {
            if (this.InvokeRequired && this.IsHandleCreated)
            {
                this.BeginInvoke(new UpdateChartDataHandler(UpdateDataToChartComponent), null);		// Run at other TestServer Thread
            }
            else if (this.IsHandleCreated)
            {
                this.UpdateDataToChartComponent();			// Run at Main Thread
            }
        }

        #endregion



        //private void btnViewPivCurve_Click(object sender, EventArgs e)
        //{
        //    frmViewPIVGraph frm = new frmViewPIVGraph();

        //    frm.ShowDialog();

        //    frm.Dispose();

        //    frm.Close();
        //}


    }
}
