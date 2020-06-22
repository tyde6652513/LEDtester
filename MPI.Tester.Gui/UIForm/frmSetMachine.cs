using System;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;
using System.Drawing;

namespace MPI.Tester.Gui
{
	public partial class frmSetMachine : System.Windows.Forms.Form
	{
		private delegate void UpdateDataHandler();
        private ChannelConfig _channelConfig;
        MPI.Tester.Gui.UIForm.Dialog.HardWareSetting.frmSMUChannelSetting frmChannelConfig;
		//private List<DevComponents.DotNetBar.Controls.ComboBoxEx> _waferMapItemsList;

		public frmSetMachine()
		{
			Console.WriteLine("[frmSetMachine], frmSetMachine()");

			InitializeComponent();

            this.cmbTesterFunction.SelectedIndexChanged += new System.EventHandler(this.UpdateChannelConfigEventHandler);

            this.cmbSrcMeterModel.SelectedIndexChanged += new System.EventHandler(this.UpdateChannelConfigEventHandler);

            this.cmbSwitchSystemModel.SelectedIndexChanged += new System.EventHandler(this.UpdateChannelConfigEventHandler);

            this._channelConfig = new ChannelConfig();

			this.InitParamAndCompData();
		}

		#region >>> Private Method <<<

		private void InitParamAndCompData()
		{
            this.chkSimulator.Visible = false;

            this.pnlVoltMsrtUnitConfigGroup.Visible = false;
            this.pnlSrcMeterConfigGroup2.Visible = false;
            this.lblInfoTesterSNTitle2.Visible = true;
            this.lblTesterSN2.Visible = true;

            if (DataCenter._rdFunc.RDFuncData.TesterConfigType == ETesterConfigType.PDTester)
            {
                pnlSpetrometer.Visible = false;
                pnlESDConfigGroup.Visible = false;
                pnlSwitchConfigGroup.Visible = false;
            }

            //-----------------------------------------------------------------------------------------------------
			// UI component initialization
			//-----------------------------------------------------------------------------------------------------
			this.cmbTesterCommMode.Items.Clear();
			this.cmbTesterCommMode.Items.AddRange(Enum.GetNames(typeof(ETesterCommMode)));

            this.cmbEQModel.Items.Clear();
            this.cmbEQModel.Items.AddRange(Enum.GetNames(typeof(EEqModel)));

            //-----------------------------------------------------------------------------------------------------
            this.cmbActiveState.Items.Clear();
            this.cmbActiveState.Items.AddRange(Enum.GetNames(typeof(EActiveState)));

            //-----------------------------------------------------------------------------------------------------
			this.cmbSrcMeterModel.Items.Clear();
			this.cmbSrcMeterModel.Items.AddRange(Enum.GetNames(typeof(ESourceMeterModel)));
            this.cmbSrcMeterModel.Items.Remove(ESourceMeterModel.DR2000.ToString());
            this.cmbSrcMeterModel.Items.Remove(ESourceMeterModel.DSPHD.ToString());
            this.cmbSrcMeterModel.Items.Remove(ESourceMeterModel.IT7321.ToString());
            this.cmbSrcMeterModel.Items.Remove(ESourceMeterModel.LDT1A.ToString());
            this.cmbSrcMeterModel.Items.Remove(ESourceMeterModel.RM3542.ToString());
            this.cmbSrcMeterModel.Items.Remove(ESourceMeterModel.T2001L.ToString());
            this.cmbSrcMeterModel.Items.Remove(ESourceMeterModel.N5700.ToString());
            this.cmbSrcMeterModel.Items.Remove(ESourceMeterModel.LDT3A200.ToString());
            this.cmbSrcMeterModel.Items.Remove(ESourceMeterModel.K2400.ToString());
            this.cmbSrcMeterModel.SelectedIndex = 0;

            this.cmbSrcMeterModel2.Items.Clear();
            this.cmbSrcMeterModel2.Items.Add(ESourceMeterModel.NONE.ToString());
            this.cmbSrcMeterModel2.Items.Add(ESourceMeterModel.K2600.ToString());
            this.cmbSrcMeterModel2.Items.Add(ESourceMeterModel.B2900A.ToString());

            //-----------------------------------------------------------------------------------------------------
			this.cmbESDModel.Items.Clear();
            this.cmbESDModel.Items.AddRange(Enum.GetNames(typeof(EESDModel)));
            this.cmbESDModel.Items.Remove(EESDModel.ESD_PLC.ToString());

            //-----------------------------------------------------------------------------------------------------
			this.cmbSptMeterModel.Items.Clear();
			this.cmbSptMeterModel.Items.AddRange(Enum.GetNames(typeof(ESpectrometerModel)));
            this.cmbSptMeterModel.Items.Remove(ESpectrometerModel.LE5400.ToString());
            this.cmbSptMeterModel.Items.Remove(ESpectrometerModel.RS_OP.ToString());

            //-----------------------------------------------------------------------------------------------------
			this.cmbWheelMsrtSource.Items.Clear();
			this.cmbWheelMsrtSource.Items.AddRange(Enum.GetNames(typeof(EWheelMsrtSource)));

            //-----------------------------------------------------------------------------------------------------
            this.cmbSptInterface.Items.Clear();
            this.cmbSptInterface.Items.AddRange(Enum.GetNames(typeof(ESpectrometerInterfaceType)));

            //-----------------------------------------------------------------------------------------------------
            this.cmbSptCalibMode.Items.Clear();
            this.cmbSptCalibMode.Items.AddRange(Enum.GetNames(typeof(ESpectrometerCalibDataMode)));

            //-----------------------------------------------------------------------------------------------------
            this.cmbDAQCardModel.Items.Clear();
            this.cmbDAQCardModel.Items.AddRange(Enum.GetNames(typeof(EDAQModel)));

            //-----------------------------------------------------------------------------------------------------
			this.cmbLCRMeterModel.Items.Clear();
			this.cmbLCRMeterModel.Items.AddRange(Enum.GetNames(typeof(ELCRModel)));

            //-----------------------------------------------------------------------------------------------------
			this.cmbLCRDCBiasSource.Items.Clear();
			this.cmbLCRDCBiasSource.Items.AddRange(Enum.GetNames(typeof(ELCRDCBiasSource)));

            //-----------------------------------------------------------------------------------------------------
            this.cmbLCRDCBiasType.Items.Clear();
            this.cmbLCRDCBiasType.Items.AddRange(Enum.GetNames(typeof(ELCRDCBiasType)));

            //-----------------------------------------------------------------------------------------------------
            this.cmbSwitchSystemModel.Items.Clear();
            this.cmbSwitchSystemModel.Items.AddRange(Enum.GetNames(typeof(ESwitchSystemModel)));

            //-----------------------------------------------------------------------------------------------------
            this.cmbPDsensingMode.Items.Clear();
            this.cmbPDsensingMode.Items.AddRange(Enum.GetNames(typeof(EPDSensingMode)));
            this.cmbPDsensingMode.Items.Remove(EPDSensingMode.DAQ.ToString());

            //-----------------------------------------------------------------------------------------------------
            this.cmbOSAModel.Items.Clear();
            this.cmbOSAModel.Items.AddRange(Enum.GetNames(typeof(EOSAModel)));
            this.cmbOSAModel.Items.Remove(EOSAModel.AQ6370D.ToString());

            //-----------------------------------------------------------------------------------------------------
            this.cmbTesterFunction.Items.Clear();
            this.cmbTesterFunction.Items.AddRange(Enum.GetNames(typeof(ETesterFunctionType)));
            this.cmbTesterFunction.Items.Remove(ETesterFunctionType.Multi_Map.ToString());
            this.cmbTesterFunction.Items.Remove(ETesterFunctionType.Multi_Pad.ToString());
            this.cmbTesterFunction.Items.Remove(ETesterFunctionType.Multi_Terminal.ToString());//20181107David
            this.cmbTesterFunction.SelectedIndex = 0;

            //-----------------------------------------------------------------------------------------------------
            this.cmbVoltMsrtUnit.Items.Clear();
            this.cmbVoltMsrtUnit.Items.AddRange(Enum.GetNames(typeof(EDmmModel)));
            this.cmbVoltMsrtUnit.SelectedIndex = 0;

            //-----------------------------------------------------------------------------------------------------

            if (frmChannelConfig == null)
            {
                frmChannelConfig = new UIForm.Dialog.HardWareSetting.frmSMUChannelSetting();
                //
                //spcChannel.Panel2

                //this.IsMdiContainer = true;
                

                frmChannelConfig.TopLevel = false;

                //frmChannelConfig.Parent = pnlChannel;

                pnlChannel.Controls.Add(frmChannelConfig);

                frmChannelConfig.Dock = DockStyle.Left;

                frmChannelConfig.Location = new Point(0, 0);

                frmChannelConfig.Size = pnlChannel.Size;

                frmChannelConfig.Show();
            }

            //--------------------------
			frmTestResultInstantInfo form = (frmTestResultInstantInfo)FormAgent.RetrieveForm(typeof(frmTestResultInstantInfo));
			form.RegisterUpdateEvent(this.OnTCPIPStateChange);
		}

		private void ChangeAuthority()
		{
			switch (DataCenter._uiSetting.AuthorityLevel)
			{
				case EAuthority.Operator:
				case EAuthority.QC:
                    this.btnSave.Enabled = false;
                    this.gplState.Enabled = false;
                    this.pnlDAQConfigGroup.Visible = false;
                    this.cmbWheelMsrtSource.Visible = false;
                    this.lblWheelMsrtSourceTitle.Visible = false;
                    this.pnlChannel.Visible = false;
					break;
				//-------------------------------------------------------------------
				case EAuthority.Engineer:
					this.btnSave.Enabled = true;
                    this.gplState.Enabled = true;
                    this.pnlDAQConfigGroup.Visible = false;
                    this.cmbWheelMsrtSource.Visible = false;
                    this.lblWheelMsrtSourceTitle.Visible = false;
                    this.pnlChannel.Visible = true;
					break;
				//-------------------------------------------------------------------
				case EAuthority.Admin:
				case EAuthority.Super:
                    this.btnSave.Enabled = true;
                    this.gplState.Enabled = true;
                    this.pnlDAQConfigGroup.Visible = true;
                    this.cmbWheelMsrtSource.Visible = true;
                    this.lblWheelMsrtSourceTitle.Visible = true;
                    this.pnlChannel.Visible = true;
					break;
				//-------------------------------------------------------------------
				default:
                    this.btnSave.Enabled = false;
                    this.gplState.Enabled = false;
                    this.pnlDAQConfigGroup.Visible = false;
                    this.cmbWheelMsrtSource.Visible = false;
                    this.lblWheelMsrtSourceTitle.Visible = false;
                    this.pnlChannel.Visible = false;
					break;
			}

            if (DataCenter._rdFunc.RDFuncData.TesterConfigType == ETesterConfigType.PDTester)
            {
                pnlDAQConfigGroup.Visible = false;
            }
            UpDataChConfigVisible();
		}

        private void UpdateDataToChannelConfigDgv()
        {
            
            if (frmChannelConfig == null)
                return;

            //UpDataChConfigVisible();

            ETesterFunctionType func = (ETesterFunctionType)Enum.Parse(typeof(ETesterFunctionType), this.cmbTesterFunction.SelectedItem.ToString(), true);

            ESourceMeterModel smu = ESourceMeterModel.K2600;
            if(this.cmbSrcMeterModel.SelectedItem != null)
            {
                smu = (ESourceMeterModel)Enum.Parse(typeof(ESourceMeterModel), this.cmbSrcMeterModel.SelectedItem.ToString(), true);
            }

            frmChannelConfig.SetConfig(_channelConfig, func, smu);
            
        }

		private void UpdateDataToControls()
		{
			switch (DataCenter._uiSetting.UIOperateMode)
			{
				case (int)EUIOperateMode.Idle:
					this.ChangeAuthority();
					break;
				//-----------------------------------------------------------------------------
				case (int)EUIOperateMode.AutoRun:
                case (int)EUIOperateMode.ManulRun:
                    this.btnSave.Enabled = false;
                    this.gplState.Enabled = false;
					break;
				//-----------------------------------------------------------------------------
                //case (int)EUIOperateMode.ManulRun:
                //    this.ChangeAuthority();
                //    this.gplState.Enabled = true;
                //    this.btnResetSpectrometer.Enabled = true;
                //    break;
				//-----------------------------------------------------------------------------
				default:
					this.ChangeAuthority();
					break;
			}

            this.cmbTesterCommMode.SelectedItem = DataCenter._machineConfig.TesterCommMode.ToString();
            this.cmbEQModel.SelectedItem = DataCenter._machineConfig.EQModle.ToString();		// 0-base
            this.cmbActiveState.SelectedItem = DataCenter._machineConfig.ActiveState.ToString();		// 0-base

			this.ipAddressInput01.Value = DataCenter._machineConfig.IPAddr01;
			this.numNetPort01.Value = DataCenter._machineConfig.NetPort01;

            this.cmbSrcMeterModel.SelectedItem = DataCenter._machineConfig.SourceMeterModel.ToString();
            this.cmbSrcMeterModel2.SelectedItem = DataCenter._machineConfig.SourceMeterModel2.ToString();  // EML Channel, 暫不開放
            this.cmbESDModel.SelectedItem = DataCenter._machineConfig.ESDModel.ToString();
            this.cmbSptMeterModel.SelectedItem = DataCenter._machineConfig.SpectrometerModel.ToString();
            this.cmbWheelMsrtSource.SelectedItem = DataCenter._machineConfig.WheelMsrtSource.ToString();
            this.cmbDAQCardModel.SelectedItem = DataCenter._machineConfig.DAQModel.ToString();			
            this.cmbLCRMeterModel.SelectedItem = DataCenter._machineConfig.LCRModel.ToString();
            this.cmbLCRDCBiasSource.SelectedItem = DataCenter._machineConfig.LCRDCBiasSource.ToString();
            this.cmbLCRDCBiasType.SelectedItem = DataCenter._machineConfig.LCRDCBiasType.ToString();

			this.txtSrcMeterSN.Text = DataCenter._machineConfig.SourceMeterSN;
            this.txtSrcMeterSN2.Text = DataCenter._machineConfig.SourceMeterSN2;
            this.txtSptMeterSN.Text = DataCenter._machineConfig.SpectrometerSN;
            this.txtSphereSN.Text = DataCenter._machineConfig.SphereSN;

			this.chkSimulator.Checked = DataCenter._machineConfig.Enable.IsSimulator;
			this.chkFilterWheelCheck.Checked = DataCenter._machineConfig.Enable.IsCheckFilterWheel;
			this.chkBarcodePrintf.Checked = DataCenter._machineConfig.Enable.IsBarcodePrint;
			this.chkEnableMapShowGap.Checked = DataCenter._machineConfig.Enable.IsEnableMapShowGap;

            this.cmbSptInterface.SelectedItem = DataCenter._machineConfig.spetometerHWSetting.SptInterfaceType.ToString();
            this.cmbSptCalibMode.SelectedItem = DataCenter._machineConfig.spetometerHWSetting.SpectometerCalibMode.ToString();

            this.numDAQSampleRate.Value = DataCenter._machineConfig.DAQSampleRate;

            this.numDAQCalibrationBufferID.Value = (int)DataCenter._machineConfig.DAQCalibrationBufferID;

            this.cmbPDsensingMode.SelectedItem = DataCenter._machineConfig.PDSensingMode.ToString();

            this.txtPdDetectorSN.Text = DataCenter._machineConfig.PDDetectorSN;

			this.txtLCRDCBiasSourceSN.Text = DataCenter._machineConfig.LCRDCBiasSourceSN;

            this.txtLCRMeterSN.Text = DataCenter._machineConfig.LCRMeterSN;

            this.chkEnablePdSourceHwTrig.Checked = DataCenter._machineConfig.IsPDDetectorHwTrig;

            //-------------------------------------------------------------------------------------------------
            // Roy, 20140326, Multi-Die testing device
            this.cmbSwitchSystemModel.SelectedItem = DataCenter._machineConfig.SwitchSystemModel.ToString();

            this.txtSwitchSystemSN.Text = DataCenter._machineConfig.SwitchSystemSN;

            this.cmbTesterFunction.SelectedItem = DataCenter._machineConfig.TesterFunctionType.ToString();


            this.chkIsAutoChangeToOPMode.Checked = DataCenter._machineConfig.Enable.IsAutoChangeToOPMode;

            this.cmbOSAModel.SelectedIndex = (int)DataCenter._machineConfig.OSAModel;

            this.txtOsaSN.Text = DataCenter._machineConfig.OSASN;

            this.cmbVoltMsrtUnit.SelectedIndex = (int)DataCenter._machineConfig.DmmModel;

            this.txtVoltMsrtUnitSN.Text = DataCenter._machineConfig.DmmSN;

            this._channelConfig = DataCenter._machineConfig.ChannelConfig.Clone() as ChannelConfig;

            this.UpdateDataToChannelConfigDgv();

            this.UpdateMachineInfo();
            //-------------------------------------------------------------------------------------------------
		}

        private void UpdateMachineInfo()
        {
            this.lblSpectrometerSN.Text = DataCenter._machineInfo.SpectrometerSN;
            this.lblSphereSN.Text = DataCenter._machineInfo.SphereSN;
            this.lblESDSN.Text = DataCenter._machineInfo.EsdSN;
            this.lblSwitchSystemSN.Text = DataCenter._machineInfo.SwitchSystemSN;
            this.lblTesterSN.Text = DataCenter._machineInfo.SourceMeterHWVersion + "_" + DataCenter._machineInfo.SourceMeterSN;
            this.lblTesterSN2.Text = DataCenter._machineInfo.SourceMeterHWVersion2 + "_" + DataCenter._machineInfo.SourceMeterSN2;
            this.lblOsaSN.Text = DataCenter._machineInfo.OsaSN;
            this.lblLCRSN.Text = DataCenter._machineInfo.LCRMeterSN;
        }

		private void SaveDataToFile()
		{
            DataCenter._machineConfig.TesterCommMode = (ETesterCommMode)Enum.Parse(typeof(ETesterCommMode), this.cmbTesterCommMode.SelectedItem.ToString(), true);     // 0-base
			DataCenter._machineConfig.IPAddr01 = this.ipAddressInput01.Value;
			DataCenter._machineConfig.NetPort01 = this.numNetPort01.Value;
            DataCenter._machineConfig.EQModle = (EEqModel)this.cmbEQModel.SelectedIndex;
            DataCenter._machineConfig.ActiveState = (EActiveState)this.cmbActiveState.SelectedIndex;

			if (this.ipAddressInput01.Value != null)
			{
				DataCenter._machineConfig.IPAddr01 = this.ipAddressInput01.Value;
			}
			DataCenter._machineConfig.NetPort01 = this.numNetPort01.Value;

			DataCenter._machineConfig.SourceMeterModel = (ESourceMeterModel)Enum.Parse(typeof(ESourceMeterModel), this.cmbSrcMeterModel.SelectedItem.ToString(), true);		// 0-base
             DataCenter._machineConfig.SourceMeterModel2 = (ESourceMeterModel)Enum.Parse(typeof(ESourceMeterModel), this.cmbSrcMeterModel2.SelectedItem.ToString(), true);		// 0-base  // EML Channel, 暫不開放

			DataCenter._machineConfig.ESDModel = (EESDModel)Enum.Parse(typeof(EESDModel), this.cmbESDModel.SelectedItem.ToString(), true);													// 0-base
			DataCenter._machineConfig.SpectrometerModel = (ESpectrometerModel)Enum.Parse(typeof(ESpectrometerModel), this.cmbSptMeterModel.SelectedItem.ToString(), true);		// 0-base


            //-----IS-------
            if (DataCenter._machineConfig.SpectrometerModel == ESpectrometerModel.CAS140)
			{
				DataCenter._machineConfig.SpectrometerSN = txtSptMeterSN.Text;
                DataCenter._machineConfig.SphereSN = this.txtSphereSN.Text;
			}

			DataCenter._machineConfig.SourceMeterSN = txtSrcMeterSN.Text;
            DataCenter._machineConfig.SourceMeterSN2 = txtSrcMeterSN2.Text;
			DataCenter._machineConfig.WheelMsrtSource = (EWheelMsrtSource)Enum.Parse(typeof(EWheelMsrtSource), this.cmbWheelMsrtSource.SelectedItem.ToString(), true);		// 0-base

            DataCenter._machineConfig.DAQModel = (EDAQModel)Enum.Parse(typeof(EDAQModel), this.cmbDAQCardModel.SelectedItem.ToString(), true);		// 0-base
			DataCenter._machineConfig.LCRModel = (ELCRModel)Enum.Parse(typeof(ELCRModel), this.cmbLCRMeterModel.SelectedItem.ToString(), true);// 0-base
			DataCenter._machineConfig.LCRDCBiasSource = (ELCRDCBiasSource)Enum.Parse(typeof(ELCRDCBiasSource), this.cmbLCRDCBiasSource.SelectedItem.ToString(), true);// 0-base
            DataCenter._machineConfig.LCRDCBiasType = (ELCRDCBiasType)Enum.Parse(typeof(ELCRDCBiasType), this.cmbLCRDCBiasType.SelectedItem.ToString(), true);// 0-base

			DataCenter._machineConfig.Enable.IsSimulator = this.chkSimulator.Checked;
			DataCenter._machineConfig.Enable.IsCheckFilterWheel = this.chkFilterWheelCheck.Checked;
			DataCenter._machineConfig.Enable.IsBarcodePrint = this.chkBarcodePrintf.Checked;
			DataCenter._machineConfig.Enable.IsEnableMapShowGap = this.chkEnableMapShowGap.Checked;

            DataCenter._machineConfig.spetometerHWSetting.SptInterfaceType = (ESpectrometerInterfaceType)Enum.Parse(typeof(ESpectrometerInterfaceType), this.cmbSptInterface.SelectedItem.ToString(), true);

            DataCenter._machineConfig.spetometerHWSetting.SpectometerCalibMode = (ESpectrometerCalibDataMode)Enum.Parse(typeof(ESpectrometerCalibDataMode), this.cmbSptCalibMode.SelectedItem.ToString(), true);

            DataCenter._machineConfig.DAQSampleRate = this.numDAQSampleRate.Value;
            DataCenter._machineConfig.DAQCalibrationBufferID = (uint)this.numDAQCalibrationBufferID.Value;

            DataCenter._machineConfig.SwitchSystemModel = (ESwitchSystemModel)Enum.Parse(typeof(ESwitchSystemModel), this.cmbSwitchSystemModel.SelectedItem.ToString(), true);

            DataCenter._machineConfig.SwitchSystemSN = this.txtSwitchSystemSN.Text;

            if (this.pnlPDSensorConfigGroup.Visible)
            {
                DataCenter._machineConfig.PDSensingMode = (EPDSensingMode)Enum.Parse(typeof(EPDSensingMode), this.cmbPDsensingMode.SelectedItem.ToString(), true);
            }
            else
            {
                DataCenter._machineConfig.PDSensingMode = EPDSensingMode.NONE;
            }

            DataCenter._machineConfig.PDDetectorSN = this.txtPdDetectorSN.Text;

			DataCenter._machineConfig.LCRDCBiasSourceSN = this.txtLCRDCBiasSourceSN.Text;

            DataCenter._machineConfig.LCRMeterSN = this.txtLCRMeterSN.Text;

            DataCenter._machineConfig.IsPDDetectorHwTrig = this.chkEnablePdSourceHwTrig.Checked;

            //-------------------------------------------------------------------------------------------------
            // Roy, 20140326, Multi-Die testing device
            DataCenter._machineConfig.TesterFunctionType = (ETesterFunctionType)Enum.Parse(typeof(ETesterFunctionType), this.cmbTesterFunction.SelectedItem.ToString(), true);

            //DataCenter._machineConfig.ChannelConfig.ColXCount = this.numChannelColXCnt.Value;

            //DataCenter._machineConfig.ChannelConfig.RowYCount = this.numChannelRowYCnt.Value;

            DataCenter._machineConfig.Enable.IsAutoChangeToOPMode = this.chkIsAutoChangeToOPMode.Checked;

            DataCenter._machineConfig.OSAModel = (EOSAModel)Enum.Parse(typeof(EOSAModel), this.cmbOSAModel.SelectedItem.ToString(), true);

            DataCenter._machineConfig.OSASN = this.txtOsaSN.Text;

            DataCenter._machineConfig.DmmModel = (EDmmModel)Enum.Parse(typeof(EDmmModel), this.cmbVoltMsrtUnit.SelectedItem.ToString(), true);

            DataCenter._machineConfig.DmmSN = this.txtVoltMsrtUnitSN.Text;

            //if (this.rdbSequenceParallelType.Checked)
            //{
            //    DataCenter._machineConfig.ChannelConfig.TesterSequenceType = ETesterSequenceType.Parallel;
            //}

            if (frmChannelConfig != null && frmChannelConfig.Visible)
            {
                var config = frmChannelConfig.GetChConfig();
                if (config != null)
                {
                    this._channelConfig = config;
                }
            }

            DataCenter._machineConfig.ChannelConfig = this._channelConfig.Clone() as ChannelConfig;
            //-------------------------------------------------------------------------------------------------

            // Just machine data to file
            DataCenter.SaveMechAndDeviceData();
		}

		#endregion
		
		#region >>> UI Event Handler <<<

		private void frmSetMachine_Load(object sender, EventArgs e)
		{
			this.UpdateDataToControls();

            switch (DataCenter._machineConfig.TesterCommMode)
            {
                case ETesterCommMode.BySoftware:
                    {
                        this.lblipAddressInput01Title.Enabled = false;

                        this.ipAddressInput01.Enabled = false;

                        this.lblNetPort01Title.Enabled = false;

                        this.numNetPort01.Enabled = false;

                        this.lblEQModel.Enabled = false;

                        this.cmbEQModel.Enabled = false;

                        this.lblActiveState.Enabled = false;

                        this.cmbActiveState.Enabled = false;

                        this.btnReConnectTCPIP.Enabled = false;

                        this.btnReConnectTCPIP.Visible = false;

                        this.pnlState.Visible = false;

                        break;
                    }
                case ETesterCommMode.TCPIP:
                case ETesterCommMode.TCPIP_MPI:
                    {
                        this.lblipAddressInput01Title.Enabled = true;

                        this.ipAddressInput01.Enabled = true;

                        this.lblNetPort01Title.Enabled = true;

                        this.numNetPort01.Enabled = true;

                        this.lblEQModel.Enabled = false;

                        this.cmbEQModel.Enabled = false;

                        this.lblActiveState.Enabled = false;

                        this.cmbActiveState.Enabled = false;

                        this.btnReConnectTCPIP.Enabled = true;

                        this.btnReConnectTCPIP.Visible = true;

                        this.pnlState.Visible = true;

                        break;
                    }
                case ETesterCommMode.IO:
                    {
                        this.lblipAddressInput01Title.Enabled = false;

                        this.ipAddressInput01.Enabled = false;

                        this.lblNetPort01Title.Enabled = false;

                        this.numNetPort01.Enabled = false;

                        this.lblEQModel.Enabled = true;

                        this.cmbEQModel.Enabled = true;

                        this.lblActiveState.Enabled = true;

                        this.cmbActiveState.Enabled = true;

                        this.btnReConnectTCPIP.Enabled = false;

                        this.btnReConnectTCPIP.Visible = false;

                        this.pnlState.Visible = false;

                        break;
                    }
                default:
                    break;
            }

            this.pnlPDSensorConfigGroup.Visible = DataCenter._sysSetting.SpecCtrl.IsSupportedLOPItem;
            if (DataCenter._rdFunc.RDFuncData.TesterConfigType == ETesterConfigType.PDTester)//作為校正用PD
            {
                this.pnlPDSensorConfigGroup.Visible = true;
            }

            

            //bool isSupportedLCR = DataCenter._sysSetting.SpecCtrl.IsSupportedLCRItem;
            bool isSupportedLCR = true;//20171017 David 強迫顯示，不然會變成無法開啟->無法設定->無法開啟的死循環
            this.pnlLCRConfigGroup.Visible = isSupportedLCR;
            this.lblInfoLCRSNTitle.Visible = isSupportedLCR;
            this.lblLCRSN.Visible = isSupportedLCR;

            bool isSupportedWLOSA = DataCenter._sysSetting.SpecCtrl.IsSupportedWLOSAItem;
            this.pnlOSAConfigGroup.Visible = isSupportedWLOSA;
            this.lblInfoOsaSNTitle.Visible = isSupportedWLOSA;
            this.lblOsaSN.Visible = isSupportedWLOSA;

            this.pnlTesterChannelConfig.Visible = DataCenter._rdFunc.RDFuncData.IsShowTesterChannelConfig;
		}

		private void frmSetMachine_VisibleChanged(object sender, EventArgs e)
		{
			if (this.Visible == true)
			{
				this.UpdateDataToControls();
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
            DialogResult result = TopMessageBox.ShowMessage((int)EMessageCode.CheckIsReStartSystem,"System Setting , Please Restart the application？", "Close", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            //DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show("System Setting , Please Restart the application？", "Close", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
			//FormAgent.ShowAlert( "Close", "Would you close the application？" );
            if (result == DialogResult.OK)
            {
                this.SaveDataToFile();

               // DataCenter._conditionCtrl.ResetProuctChannelSetting(DataCenter._machineConfig.TesterFunctionType, DataCenter._machineConfig.ChannelConfig.ColXCount, DataCenter._machineConfig.ChannelConfig.RowYCount);
                DataCenter.SaveProductFile();

                FormAgent.MainForm.IsClose = true;
                FormAgent.MainForm.Close();
            }
		}

		private void cmbSptMeterModel_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.cmbSptMeterModel.SelectedItem.ToString() == ESpectrometerModel.CAS140.ToString() )		// RS_OP for OPTiMUM spectrometer
			{
                plsptSetting.Enabled = true;
			}
			else
			{
                plsptSetting.Enabled = false;
			}
		}	

		private void cmbSrcMeterModel_SelectedIndexChanged(object sender, EventArgs e)
		{
            this.pnlVoltMsrtUnitConfigGroup.Visible = false;
            this.pnlSrcMeterConfigGroup2.Visible = false;

			if (this.cmbSrcMeterModel.SelectedItem.ToString() == ESourceMeterModel.N5700.ToString()  ||
                this.cmbSrcMeterModel.SelectedItem.ToString() == ESourceMeterModel.DR2000.ToString() ||
				this.cmbSrcMeterModel.SelectedItem.ToString() == ESourceMeterModel.DSPHD.ToString()  ||
				this.cmbSrcMeterModel.SelectedItem.ToString() == ESourceMeterModel.IT7321.ToString() ||
                this.cmbSrcMeterModel.SelectedItem.ToString() == ESourceMeterModel.K2600.ToString()  ||
                this.cmbSrcMeterModel.SelectedItem.ToString() == ESourceMeterModel.LDT3A200.ToString() ||
				this.cmbSrcMeterModel.SelectedItem.ToString() == ESourceMeterModel.RM3542.ToString() ||
                this.cmbSrcMeterModel.SelectedItem.ToString() == ESourceMeterModel.B2900A.ToString() ||
                this.cmbSrcMeterModel.SelectedItem.ToString() == ESourceMeterModel.SS400.ToString())		
			{
				this.txtSrcMeterSN.Enabled = true;

                if (this.cmbSrcMeterModel.SelectedItem.ToString() == ESourceMeterModel.SS400.ToString())
                {
                    this.pnlVoltMsrtUnitConfigGroup.Visible = true;
                    this.pnlSrcMeterConfigGroup2.Visible = true;
                }
			}
			else
			{
				this.txtSrcMeterSN.Enabled = false;
			}
		}

        private void UpdateChannelConfigEventHandler(object sender, EventArgs e)
        {
            if (!UpDataChConfigVisible())
            {
                return;
            }
            //------------------------------------------------------------------------------------------------------
            // Channel Assignment Table Config.
            //------------------------------------------------------------------------------------------------------
            this._channelConfig.AssignmentTable.Clear();
            for (int i = 0; i < this._channelConfig.ColXCount * this._channelConfig.RowYCount; ++i)
            {
                ChannelAssignmentData data = new ChannelAssignmentData();
                if (DataCenter._machineInfo != null && DataCenter._machineInfo.SourceMeterHWVersion != null && DataCenter._machineInfo.SourceMeterHWVersion.Contains("Keithley") &&
           (DataCenter._machineInfo.SourceMeterHWVersion.Contains("2602") ||
            DataCenter._machineInfo.SourceMeterHWVersion.Contains("2612") ||
            DataCenter._machineInfo.SourceMeterHWVersion.Contains("2636")))
                {
                    int rest = i % 2;
                    int num = (int)Math.Floor(((double)i)/2);
                    data.DeviceComPort = "";
                    data.DeviceIpAddress = "192.168.50." + (2 + num).ToString("0");
                    data.DeviceSerialNum = "";
                    data.SourceCH = "A";
                    if (rest == 1)
                    {
                        data.SourceCH = "B";
                    }
                    //data.SourceModel = _smuModel.ToString();
                }
                this._channelConfig.AssignmentTable.Add(data);
            }
            this.UpdateDataToChannelConfigDgv();
        }

        private bool UpDataChConfigVisible()
        {
            if (this.cmbTesterFunction.SelectedItem == null)
            {
                return false;
            }
            ETesterFunctionType func = (ETesterFunctionType)Enum.Parse(typeof(ETesterFunctionType), this.cmbTesterFunction.SelectedItem.ToString(), true);		// 0-base

            this.txtSrcMeterSN.Visible = true;

            switch (func)
            {
                case ETesterFunctionType.Multi_Die:
                case ETesterFunctionType.Multi_Pad:
                    {
                        pnlChannel.Visible = true;
                        txtSrcMeterSN.Visible = false;

                        break;
                    }
                case ETesterFunctionType.Multi_Terminal:
                    {
                        pnlChannel.Visible = true;
                        txtSrcMeterSN.Visible = false;
                        break;
                    }
                default:
                    {
                        pnlChannel.Visible = false;
                        txtSrcMeterSN.Visible = true;
                        break;
                    }
            }
            return true;
        }

		#endregion

		public void UpdateDataToUIForm()
		{
			if (this.InvokeRequired && this.IsHandleCreated)
			{
				this.BeginInvoke(new UpdateDataHandler(UpdateDataToControls), null);		// Run at other TestServer Thread
			}
			else if (this.IsHandleCreated)
			{
				this.UpdateDataToControls();			//  Run at Main Thread
			}
		}

        private void btnResetSpectrometer_Click(object sender, EventArgs e)
        {
            AppSystem.RunCommand(TestKernel.ETesterKernelCmd.ResetMachineHW);
        }

        private void btnSelectSptConfig_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Import CoefTable Data from File";
            openFileDialog.Filter = "INI files (*.ini)|*.ini";
            openFileDialog.FilterIndex = 1;    // default value = 1

            openFileDialog.InitialDirectory = Constants.Paths.SPECTROMETER_CALIBRATION_DATA;
            openFileDialog.FileName = "";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;


            this.txtSptMeterSN.Text = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);
        }

        private void btnLoadSptCorrection_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Import CoefTable Data from File";
            openFileDialog.Filter = "ISC files (*.isc)|*.isc";
            openFileDialog.FilterIndex = 1;    // default value = 1
            openFileDialog.InitialDirectory = Constants.Paths.SPECTROMETER_CALIBRATION_DATA;
            openFileDialog.FileName = "";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;


            this.txtSphereSN.Text = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);

        }

		private void btnReConnectTCPIP_Click(object sender, EventArgs e)
		{
			DataCenter._machineConfig.TesterCommMode = (ETesterCommMode)Enum.Parse(typeof(ETesterCommMode), this.cmbTesterCommMode.SelectedItem.ToString(), true);     // 0-base
			
			DataCenter._machineConfig.IPAddr01 = this.ipAddressInput01.Value;
			
			DataCenter._machineConfig.NetPort01 = this.numNetPort01.Value;

			AppSystem.ReConnectTCPIP();
		}

		private void OnTCPIPStateChange(object sender, EventArgs e)
		{
			TestServer.ETCPClientState state = AppSystem._tcpipClientState;

			if (state == TestServer.ETCPClientState.NONE || state == TestServer.ETCPClientState.ERROR)
			{
				this.picLamp.Image = global::MPI.Tester.Gui.Properties.Resources.Lamp_Red;

				this.lblTCPIPState.Text = "Disconnect";
			}
			else if (state == TestServer.ETCPClientState.CONNECTING)
			{
				this.picLamp.Image = global::MPI.Tester.Gui.Properties.Resources.Lamp_Blue;

				this.lblTCPIPState.Text = "Connecting..";
			}
			else
			{
				this.picLamp.Image = global::MPI.Tester.Gui.Properties.Resources.Lamp_Green;

				this.lblTCPIPState.Text = "Connected";
			}
		}

        private void btnResetChannelData_Click(object sender, EventArgs e)
        {
            if (frmChannelConfig == null)
                return;
            var config = frmChannelConfig.GetChConfig();
            if (config != null)
            {
                this._channelConfig = config;

                DataCenter._conditionCtrl.ResetProuctChannelSetting(DataCenter._machineConfig.TesterFunctionType, DataCenter._machineConfig.ChannelConfig.ColXCount, DataCenter._machineConfig.ChannelConfig.RowYCount);

                DataCenter.SaveProductFile();
            }            
        }

        private void cmbSwitchSystemModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbSwitchSystemModel.SelectedItem.ToString() == ESwitchSystemModel.K3706A.ToString())
            {
                this.txtSwitchSystemSN.Enabled = true;
            }
            else
            {
                this.txtSwitchSystemSN.Enabled = false;
            }
        }

        private void cmbPDsensingMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbPDsensingMode.SelectedItem.ToString() == EPDSensingMode.SrcMeter_2nd.ToString())
            {
                this.chkEnablePdSourceHwTrig.Visible = true;
              
                this.txtPdDetectorSN.Visible = true;
            }
            else if (this.cmbPDsensingMode.SelectedItem.ToString() == EPDSensingMode.DMM_7510.ToString())
            {
                this.txtPdDetectorSN.Visible = true;

                this.chkEnablePdSourceHwTrig.Visible = false;
            }
            else
            {
                this.chkEnablePdSourceHwTrig.Visible = false;
                this.chkEnablePdSourceHwTrig.Checked = false;
             
                this.txtPdDetectorSN.Visible = false;
            }
        }

		private void cmbLCRMeterModel_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.cmbLCRMeterModel.SelectedItem.ToString() == ELCRModel.NONE.ToString())
			{
				this.txtLCRMeterSN.Enabled = false;

				this.cmbLCRDCBiasType.SelectedItem = ELCRDCBiasType.Internal.ToString();

				this.cmbLCRDCBiasType.Enabled = false;
			}
			else
			{
				this.txtLCRMeterSN.Enabled = true;

				this.cmbLCRDCBiasType.Enabled = true;
			}
		}

		private void cmbLCRDCBiasType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.cmbLCRDCBiasType.SelectedItem.ToString() == ELCRDCBiasType.Ext_Other.ToString() ||
				this.cmbLCRDCBiasType.SelectedItem.ToString() == ELCRDCBiasType.Other.ToString())
			{
				this.cmbLCRDCBiasSource.Enabled = true;

				this.txtLCRDCBiasSourceSN.Enabled = true;
			}
			else
			{
				this.cmbLCRDCBiasSource.Enabled = false;

				this.txtLCRDCBiasSourceSN.Enabled = false;
			}
		}

        private void cmbOSAModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.lblOsaSNTitle.Enabled = false;
            this.txtOsaSN.Enabled = false;

            if (this.cmbOSAModel.SelectedItem.ToString() == EOSAModel.MS9740A.ToString())
            {
                //this.lblOsaSNTitle.Enabled = true;
                this.txtOsaSN.Enabled = true;
            }
        }

        private void cmbSrcMeterModel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbSrcMeterModel2.SelectedItem.ToString() == ESourceMeterModel.N5700.ToString() ||
                this.cmbSrcMeterModel2.SelectedItem.ToString() == ESourceMeterModel.DR2000.ToString() ||
                this.cmbSrcMeterModel2.SelectedItem.ToString() == ESourceMeterModel.DSPHD.ToString() ||
                this.cmbSrcMeterModel2.SelectedItem.ToString() == ESourceMeterModel.IT7321.ToString() ||
                this.cmbSrcMeterModel2.SelectedItem.ToString() == ESourceMeterModel.K2600.ToString() ||
                this.cmbSrcMeterModel2.SelectedItem.ToString() == ESourceMeterModel.LDT3A200.ToString() ||
                this.cmbSrcMeterModel2.SelectedItem.ToString() == ESourceMeterModel.RM3542.ToString())
            {
                this.txtSrcMeterSN2.Enabled = true;
            }
            else
            {
                this.txtSrcMeterSN2.Enabled = false;
            }
        }

        private void btnSetLaser_Click(object sender, EventArgs e)
        {
            //TestItemDescription description = DataCenter._sysSetting.SpecCtrl.ItemDescription[ETestType.LCR.ToString()];
            Form frmLaser = null;
            if (DataCenter._uiSetting.UserID == EUserID.Emcore)
            {
                frmLaser = new UIForm.Dialog.HardWareSetting.frmLaserSystem(DataCenter._machineConfig.LaserSrcSysConfig);

                if (frmLaser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    DataCenter._machineConfig.LaserSrcSysConfig = (frmLaser as UIForm.Dialog.HardWareSetting.frmLaserSystem).GetLaserSysConfig();
                }
            }
            else
            {
                frmLaser = new UIForm.Dialog.HardWareSetting.frmLaserSysV2(DataCenter._machineConfig.LaserSrcSysConfig);
                if (frmLaser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    DataCenter._machineConfig.LaserSrcSysConfig = (frmLaser as UIForm.Dialog.HardWareSetting.frmLaserSysV2).GetLaserSysConfig();
                }
            }
           
            if(frmLaser != null)
            {
                frmLaser.Dispose();

                frmLaser.Close(); 
            }
            
        }

        private void pnlChannel_VisibleChanged(object sender, EventArgs e)
        {
            UpdateDataToChannelConfigDgv();
        }

        private void cmbTesterFunction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (frmChannelConfig != null)
            {
                frmChannelConfig.RefteshDGV();
            }
        }
       

	}
}