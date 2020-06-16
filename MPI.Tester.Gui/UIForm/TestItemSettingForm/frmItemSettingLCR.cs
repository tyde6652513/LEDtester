using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;

using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Gui
{
    public partial class frmItemSettingLCR : Form, IConditionUICtrl, IConditionElecCtrl
    {
        private object _lockObj;

        private LCRTestItem _item;

        private bool _isEnableSwitchChannel;
        private bool _isEnableMsrtForceValue;

        public frmItemSettingLCR()
        {
            InitializeComponent();
            
            this._lockObj = new object();

            this._item = new LCRTestItem();

            this._isEnableSwitchChannel = false;
            this._isEnableMsrtForceValue = false;
        }

        public frmItemSettingLCR(TestItemDescription description) : this()
        {
            this.UpdateItemBoudary(description);
        }

        #region >>> Public Property <<<

        public bool IsAutoSelectForceRange
        {
			get;
			set;
        }

        public bool IsAutoSelectMsrtRange
        {
			get;
			set;
        }

		public bool IsVisibleFilterCount
		{
			get;
			set;
		}

        public bool IsVisibleNPLC
        {
			get;
			set;
        }

        public bool IsEnableSwitchChannel
        {
            get { return this._isEnableSwitchChannel; }
            set { lock (this._lockObj) { this._isEnableSwitchChannel = value; } }
        }

        public bool IsEnableMsrtForceValue
        {
            get { return this._isEnableMsrtForceValue; }
            set { lock (this._lockObj) { this._isEnableMsrtForceValue = value; } }
        }

        public uint MaxSwitchingChannelCount
        {
            get;
            set;
        }

        #endregion

        #region >>> Private Method <<<

        private void UpdateItemBoudary(TestItemDescription description)
        {
            if (description == null || description.Count == 0)
            {
                return;
            }

			bool isVisibleDCBiasI = description[EItemDescription.LCR_IsProvideDCBiasI.ToString()].IsVisible;

			bool isVisibleDCBiasV = description[EItemDescription.LCR_IsProvideDCBiasV.ToString()].IsVisible;

			bool isVisibleSignalI = description[EItemDescription.LCR_IsProvideSignalLevelI.ToString()].IsVisible;

			bool isVisibleSignalV = description[EItemDescription.LCR_IsProvideSignalLevelV.ToString()].IsVisible;

			foreach (var data in description.Property)
			{
				EItemDescription keyName = (EItemDescription)Enum.Parse(typeof(EItemDescription), data.PropertyKeyName);

				switch (keyName)
				{
					case EItemDescription.WaitTime:
						{
							this.pnlWaitTime.Enabled = data.IsVisible;
							this.dinWaitTime.MaxValue = data.MaxValue;
							this.dinWaitTime.MinValue = data.MinValue;
							this.dinWaitTime.Value = data.DefaultValue;
							this.dinWaitTime.DisplayFormat = data.Format;
							break;
						}
					case EItemDescription.LCR_SignalLevelV:
						{
                            this.pnlSignalLevelV.Visible = data.IsVisible;
							this.pnlSignalLevelV.Enabled = data.IsVisible;
							this.dinSignalLevelV.MaxValue = data.MaxValue;
							this.dinSignalLevelV.MinValue = data.MinValue;
							this.dinSignalLevelV.Value = data.DefaultValue;
							this.dinSignalLevelV.DisplayFormat = data.Format;
							break;
						}
					case EItemDescription.LCR_SignalLevelI:
						{
                            //this.pnlSignalLevelI.Visible = data.IsVisible;
							this.pnlSignalLevelI.Enabled = data.IsVisible;
							this.dinSignalLevelI.MaxValue = data.MaxValue;
							this.dinSignalLevelI.MinValue = data.MinValue;
							this.dinSignalLevelI.Value = data.DefaultValue;
							this.dinSignalLevelI.DisplayFormat = data.Format;
							break;
						}
					case EItemDescription.LCR_DCBiasV:
						{
                            this.pnlDCBiasV.Visible = data.IsVisible;
							this.pnlDCBiasV.Enabled = data.IsVisible;
							this.dinDCBiasV.MaxValue = data.MaxValue;
							this.dinDCBiasV.MinValue = data.MinValue;
							this.dinDCBiasV.Value = data.DefaultValue;
							this.dinDCBiasV.DisplayFormat = data.Format;
							break;
						}
					case EItemDescription.LCR_DCBiasI:
						{
                            //this.pnlDCBiasI.Visible = data.IsVisible;
							this.pnlDCBiasI.Enabled = data.IsVisible;
							this.dinDCBiasI.MaxValue = data.MaxValue;
							this.dinDCBiasI.MinValue = data.MinValue;
							this.dinDCBiasI.Value = data.DefaultValue;
							this.dinDCBiasI.DisplayFormat = data.Format;
							break;
						}
					case EItemDescription.LCR_Frequency:
						{
							this.pnlFrequency.Enabled = data.IsVisible;
							this.dinFrequency.MaxValue = data.MaxValue;
							this.dinFrequency.MinValue = data.MinValue;
							this.dinFrequency.Value = data.DefaultValue;
							//this.dinFrequency.DisplayFormat = data.Format;
							break;
						}
					case EItemDescription.LCR_TestType:
						{
							this.cmbMsrtMode.Items.Clear();

							LCRItemDescription lcrData = data as LCRItemDescription;

							foreach (var name in lcrData.SupportTestItemList)
							{
								this.cmbMsrtMode.Items.Add(name.ToString());
							}

							break;
						}
					case EItemDescription.LCR_MsrtSpeed:
						{
							this.cmbMsrtSpeed.Items.Clear();

							LCRItemDescription lcrData = data as LCRItemDescription;

							foreach (var name in lcrData.SupportMsrtSpeed)
							{
								this.cmbMsrtSpeed.Items.Add(name.ToString());
							}

							break;
						}
					case EItemDescription.LCR_IsProvideSignalLevelV:
						{
							if (data.IsEnable && isVisibleSignalV)
							{
								this.cmbSignalMode.Items.Add(ELCRSignalMode.Voltage.ToString());
							}

							break;
						}
					case EItemDescription.LCR_IsProvideSignalLevelI:
						{
							if (data.IsEnable && isVisibleSignalI)
							{
								this.cmbSignalMode.Items.Add(ELCRSignalMode.Current.ToString());
							}

							break;
						}
					case EItemDescription.LCR_IsProvideDCBiasV:
						{
							if (data.IsEnable && isVisibleDCBiasV)
							{
								this.cmbDCBiasMode.Items.Add(ELCRDCBiasMode.Voltage.ToString());
							}

							break;
						}
					case EItemDescription.LCR_IsProvideDCBiasI:
						{
							if (data.IsEnable && isVisibleDCBiasI)
							{
								this.cmbDCBiasMode.Items.Add(ELCRDCBiasMode.Current.ToString());
							}

							break;
						}
					case EItemDescription.MsrtRange:
						{
							this.dinMsrtRange.MaxValue = data.MaxValue;
							this.dinMsrtRange.MinValue = data.MinValue;
							this.dinMsrtRange.Value = data.DefaultValue;
							this.dinMsrtRange.DisplayFormat = data.Format;
							break;
						}
					default:
						break;
				}
			}

			if (this.cmbMsrtMode.Items.Count > 0)
			{
				this.cmbMsrtMode.SelectedIndex = 0;
			}
			else
			{
				this.pnlMsrtMode.Enabled = false;
			}

			if (this.cmbSignalMode.Items.Count > 0)
			{
				this.cmbSignalMode.SelectedIndex = 0;
			}
			else
			{
				this.pnlSignalMode.Enabled = false;

				this.pnlSignalLevelI.Enabled = false;

				this.pnlSignalLevelV.Enabled = false;
			}

			if (this.cmbDCBiasMode.Items.Count > 0)
			{
				this.cmbDCBiasMode.SelectedIndex = 0;
			}
			else
			{
				this.pnlDCBiasMode.Visible = false;

				this.pnlDCBiasMode.Enabled = false;

				this.pnlDCBiasI.Enabled = false;

				this.pnlDCBiasV.Enabled = false;
			}

			if (this.cmbMsrtSpeed.Items.Count > 0)
			{
				this.cmbMsrtSpeed.SelectedIndex = 0;
			}
			else
			{
				this.pnlMsrtSpeed.Enabled = false;
			}
        }

        #endregion

        #region >>> Public Method <<<

        public void RefreshUI()
        { 
        }

        public bool CheckUI(out string msg)
        {
            msg = string.Empty;

            return true;
        }

        public void UpdateCondtionDataToComponent(TestItemData data)
        {
            this._item = data.Clone() as LCRTestItem;

			this.cmbMsrtMode.SelectedItem = this._item.LCRSetting.LCRMsrtType.ToString();

			this.dinWaitTime.Value = this._item.LCRSetting.MsrtDelayTime;

			this.cmbSignalMode.SelectedItem = this._item.LCRSetting.SignalMode.ToString();

			this.dinSignalLevelV.Value = this._item.LCRSetting.SignalLevelV;

			this.dinSignalLevelI.Value = this._item.LCRSetting.SignalLevelI * 1000; // UI: mArms

			this.cmbDCBiasMode.SelectedItem = this._item.LCRSetting.DCBiasMode.ToString();

			this.dinDCBiasV.Value = this._item.LCRSetting.DCBiasV;

			this.dinDCBiasI.Value = this._item.LCRSetting.DCBiasI * 1000; // UI: mArms

			this.dinFrequency.Value = this._item.LCRSetting.Frequency ; // UI: Hz

			if (this.cmbMsrtSpeed.Items.Contains(this._item.LCRSetting.MsrtSpeed.ToString()))
			{
				this.cmbMsrtSpeed.SelectedItem = this._item.LCRSetting.MsrtSpeed.ToString();
			}
			else if (this.cmbMsrtSpeed.Items.Count > 0)
			{
				this.cmbMsrtSpeed.SelectedIndex = 0;
			}

			this.dinMsrtRange.Value = this._item.LCRSetting.Range;

            this.dinBiasDelay.Value = this._item.LCRSetting.BiasDelay  ;
        }

        public TestItemData GetConditionDataFromComponent()
        {
			if (this.cmbMsrtMode.Items.Count > 0)
			{
                this._item.LCRSetting.LCRMsrtType = (ELCRTestType)Enum.Parse(typeof(ELCRTestType), this.cmbMsrtMode.SelectedItem.ToString(), true);
			}

			this._item.LCRSetting.MsrtDelayTime = this.dinWaitTime.Value;

			if (this.cmbSignalMode.Items.Count > 0)
			{
				this._item.LCRSetting.SignalMode = (ELCRSignalMode)Enum.Parse(typeof(ELCRSignalMode), this.cmbSignalMode.SelectedItem.ToString(), true);
			}

			this._item.LCRSetting.SignalLevelV = this.dinSignalLevelV.Value;

			this._item.LCRSetting.SignalLevelI = this.dinSignalLevelI.Value / 1000; //UI: mArms

			if (this.cmbDCBiasMode.Items.Count > 0)
			{
				this._item.LCRSetting.DCBiasMode = (ELCRDCBiasMode)Enum.Parse(typeof(ELCRDCBiasMode), this.cmbDCBiasMode.SelectedItem.ToString(), true);
			}

			this._item.LCRSetting.DCBiasV = this.dinDCBiasV.Value;

			this._item.LCRSetting.DCBiasI = this.dinDCBiasI.Value / 1000; //UI: mArms

			this._item.LCRSetting.Frequency = this.dinFrequency.Value ; // UI: Hz

			this._item.LCRSetting.MsrtSpeed = (ELCRMsrtSpeed)Enum.Parse(typeof(ELCRMsrtSpeed), this.cmbMsrtSpeed.SelectedItem.ToString(), true);

			this._item.LCRSetting.Range = (int)this.dinMsrtRange.Value;

			this._item.ElecSetting[0].PUMIndex = (uint)DataCenter._sysSetting.VfPmuIndex;

			this._item.ElecSetting[0].ForceUnit = EVoltUnit.V.ToString();

			this._item.ElecSetting[0].ForceDelayTime = 0;

			this._item.ElecSetting[0].ForceTime = 10;

			this._item.ElecSetting[0].ForceValue = this.dinDCBiasV.Value;

			this._item.ElecSetting[0].ForceTimeUnit = ETimeUnit.ms.ToString();

			this._item.ElecSetting[0].IsAutoForceRange = true;

			this._item.ElecSetting[0].MsrtRange = 1E-2;

			this._item.ElecSetting[0].MsrtProtection = 1;

			this._item.ElecSetting[0].MsrtUnit = EAmpUnit.mA.ToString();

			this._item.ElecSetting[0].MsrtNPLC = 0.01;

			this._item.ElecSetting[0].IsAutoTurnOff = false;

			//this._item.MsrtResult[0].Unit = EAmpUnit.mA.ToString();

            this._item.LCRSetting.BiasDelay = this.dinBiasDelay.Value;

            return this._item;
        }

        #endregion

		private void cmbSignalMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			string signalModeStr = this.cmbSignalMode.SelectedItem.ToString();

			ELCRSignalMode signalMode = (ELCRSignalMode)Enum.Parse(typeof(ELCRSignalMode), signalModeStr, true);

			this.pnlSignalLevelI.Enabled = false;

			this.pnlSignalLevelV.Enabled = false;

			if (signalMode == ELCRSignalMode.Voltage)
			{
				this.pnlSignalLevelV.Enabled = true;
			}
			else
			{
				this.pnlSignalLevelI.Enabled = true;
			}
		}

		private void cmbDCBiasMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			string dcBiasModeStr = this.cmbDCBiasMode.SelectedItem.ToString();

			ELCRDCBiasMode dcBiasMode = (ELCRDCBiasMode)Enum.Parse(typeof(ELCRDCBiasMode), dcBiasModeStr, true);

			this.pnlDCBiasI.Enabled = false;

			this.pnlDCBiasV.Enabled = false;

			if (dcBiasMode == ELCRDCBiasMode.Voltage)
			{
				this.pnlDCBiasV.Enabled = true;
			}
			else
			{
				this.pnlDCBiasI.Enabled = true;
			}
		}
    }
}
