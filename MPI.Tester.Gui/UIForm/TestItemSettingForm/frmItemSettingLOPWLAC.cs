using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Data;

namespace MPI.Tester.Gui
{
    public partial class frmItemSettingLOPWLAC : Form, IConditionUICtrl, IConditionElecCtrl
    {
        private object _lockObj;

        private TestItemData _item;

        private bool _isAutoSelectForceRange;
        private bool _isAutoSelectMsrtRange;
        private bool _isEnableNPLC;
        private bool _isEnableFilter;
        private bool _isEnableSwitchChannel;
        private bool _isEnableMsrtForceValue;

        public frmItemSettingLOPWLAC()
        {
            InitializeComponent();
            
            this._lockObj = new object();

            this._item = new LOPWLTestItem();

            this.cmbSensingMode.Items.Clear();
            this.cmbSensingMode.Items.AddRange(Enum.GetNames(typeof(ESensingMode)));
            this.cmbSensingMode.SelectedItem = ESensingMode.Fixed.ToString();

            this._isAutoSelectForceRange = false;
            this._isAutoSelectMsrtRange = false;
            this._isEnableNPLC = false;
            this._isEnableFilter = false;

            this.cmbSensingMode.SelectedIndexChanged += new System.EventHandler(this.UpdateOpticalComponentEventHandler);
            this.chkIsTestOptical.CheckedChanged += new System.EventHandler(this.UpdateOpticalComponentEventHandler);

            this.chkIsTestOptical.Checked = true;
            this._isEnableSwitchChannel = false;
            this._isEnableMsrtForceValue = false;
        }

        public frmItemSettingLOPWLAC(TestItemDescription description) : this()
        {
            this.UpdateItemBoudary(description);
        }

        #region >>> Public Property <<<

        public bool IsAutoSelectForceRange
        {
            get { return this._isAutoSelectForceRange; }
            set { lock (this._lockObj) { this._isAutoSelectForceRange = value; } }
        }

        public bool IsAutoSelectMsrtRange
        {
            get { return this.pnlMsrtRange.Visible; }
            set { lock (this._lockObj) { this.pnlMsrtRange.Visible = value; } }
        }

        public bool IsVisibleFilterCount
        {
            get { return this.pnlFilterCount.Visible; }
            set
            {
                lock (this._lockObj)
                {
                    if (this._isEnableFilter)
                    {
                        this.pnlFilterCount.Visible = value;
                    }
                }
            }
        }

        public bool IsVisibleNPLC
        {
            get { return this.pnlNPLC.Visible; }
            set
            {
                lock (this._lockObj)
                {
                    if (this._isEnableNPLC)
                    {
                        this.pnlNPLC.Visible = value;
                    }
                }
            }
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

            foreach (var data in description.Property)
            {
                EItemDescription keyName = (EItemDescription)Enum.Parse(typeof(EItemDescription), data.PropertyKeyName);

                switch (keyName)
                {
                    case EItemDescription.WaitTime:
                        {
                            this.dinForceDelay.MaxValue = data.MaxValue;
                            this.dinForceDelay.MinValue = data.MinValue;
                            this.dinForceDelay.Value = data.DefaultValue;
                            this.dinForceDelay.DisplayFormat = data.Format;
                            break;
                        }
                    case EItemDescription.ForceValue:
                        {
                            this.dinForceValue.MaxValue = data.MaxValue;
                            this.dinForceValue.MinValue = data.MinValue;
                            this.dinForceValue.Value = data.DefaultValue;
                            this.dinForceValue.DisplayFormat = data.Format;
                            break;
                        }
                    case EItemDescription.ACFrequency:
                        {
                            this.dinForceFrequency.MaxValue = data.MaxValue;
                            this.dinForceFrequency.MinValue = data.MinValue;
                            this.dinForceFrequency.Value = data.DefaultValue;
                            this.dinForceFrequency.DisplayFormat = data.Format;

                            break;
                        }
                    case EItemDescription.ForceTime:
                        {
                            this.dinForceTime.MaxValue = data.MaxValue;
                            this.dinForceTime.MinValue = data.MinValue;
                            this.dinForceTime.Value = data.DefaultValue;
                            this.dinForceTime.DisplayFormat = data.Format;
                            break;
                        }
                    case EItemDescription.MsrtRange:
                        {
                            this.pnlMsrtRange.Visible = data.IsEnable & data.IsVisible;

                            this.dinMsrtRange.MaxValue = data.MaxValue;
                            this.dinMsrtRange.MinValue = data.MinValue;
                            this.dinMsrtRange.Value = data.DefaultValue;
                            this.dinMsrtRange.DisplayFormat = data.Format;
                            break;
                        }
                    case EItemDescription.MsrtClamp:
                        {
                            this.pnlMsrtClamp.Visible = data.IsEnable & data.IsVisible;

                            this.dinMsrtClamp.MaxValue = data.MaxValue;
                            this.dinMsrtClamp.MinValue = data.MinValue;
                            this.dinMsrtClamp.Value = data.DefaultValue;
                            this.dinMsrtClamp.DisplayFormat = data.Format;
                            break;
                        }
                    case EItemDescription.FilterCount:
                        {
                            this.pnlFilterCount.Visible = data.IsEnable & data.IsVisible;

                            this._isEnableFilter = data.IsEnable;

                            this.numMsrtFilterCount.MaxValue = (int)data.MaxValue;
                            this.numMsrtFilterCount.MinValue = (int)data.MinValue;
                            this.numMsrtFilterCount.Value = (int)data.DefaultValue;
                            this.numMsrtFilterCount.DisplayFormat = data.Format;
                            break;
                        }
                    case EItemDescription.NPLC:
                        {
                            this.pnlNPLC.Visible = data.IsEnable & data.IsVisible;

                            this._isEnableNPLC = data.IsEnable;

                            this.dinNPLC.MaxValue = data.MaxValue;
                            this.dinNPLC.MinValue = data.MinValue;
                            this.dinNPLC.Value = data.DefaultValue;
                            this.dinNPLC.DisplayFormat = data.Format;
                            break;
                        }
                    case EItemDescription.IntTimeFix:
                        {
                            this.dinFixIntegralTime.MaxValue = data.MaxValue;
                            this.dinFixIntegralTime.MinValue = data.MinValue;
                            this.dinFixIntegralTime.Value = data.DefaultValue;
                            this.dinFixIntegralTime.DisplayFormat = data.Format;
                            break;
                        }
                    case EItemDescription.IntTimeLimit:
                        {
                            this.dinLimitIntegralTime.MaxValue = data.MaxValue;
                            this.dinLimitIntegralTime.MinValue = data.MinValue;
                            this.dinLimitIntegralTime.Value = data.DefaultValue;
                            this.dinLimitIntegralTime.DisplayFormat = data.Format;
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        private void UpdateOpticalComponentEventHandler(object sender, EventArgs e)
        {
            if (this.chkIsTestOptical.Checked)
            {
                this.pnlSensingMode.Enabled = true;

                this.pnlFixedTime.Enabled = true;

                this.pnlLimitTime.Enabled = true;

                if (this.cmbSensingMode.SelectedItem.ToString() == ESensingMode.Limit.ToString() || this.cmbSensingMode.SelectedItem.ToString() == ESensingMode.Limit02.ToString())
                {
                    this.pnlFixedTime.Enabled = false;

                    this.pnlLimitTime.Enabled = true;
                }
                else if (this.cmbSensingMode.SelectedItem.ToString() == ESensingMode.Fixed.ToString())
                {
                    this.pnlFixedTime.Enabled = true;

                    this.pnlLimitTime.Enabled = false;
                }
            }
            else
            {
                this.pnlSensingMode.Enabled = false;

                this.pnlFixedTime.Enabled = false;

                this.pnlLimitTime.Enabled = false;
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
            this._item = (data as LOPWLTestItem).Clone() as LOPWLTestItem;

            // ElecSettingData
            this.dinForceDelay.Value = this._item.ElecSetting[0].ForceDelayTime;

            this.dinForceValue.Value = this._item.ElecSetting[0].ForceValue;

            this.dinForceFrequency.Value = this._item.ElecSetting[0].ForceValueFrequency;

            this.dinForceTime.Value = this._item.ElecSetting[0].ForceTime;

            this.dinMsrtRange.Value = this._item.ElecSetting[0].MsrtRange;

            this.dinMsrtClamp.Value = this._item.ElecSetting[0].MsrtProtection;

            this.numMsrtFilterCount.Value = this._item.ElecSetting[0].MsrtFilterCount;

            // OptiSetting
            this.chkIsTestOptical.Checked = (this._item as LOPWLTestItem).IsTestOptical;

            this.cmbSensingMode.SelectedItem = (this._item as LOPWLTestItem).OptiSetting.SensingMode.ToString();

            this.dinFixIntegralTime.Value = (this._item as LOPWLTestItem).OptiSetting.FixIntegralTime;

            this.dinLimitIntegralTime.Value = (this._item as LOPWLTestItem).OptiSetting.LimitIntegralTime;

            this.dinTrigDelayTime.Value = (this._item as LOPWLTestItem).OptiSetting.TrigDelayTime;

        }

        public TestItemData GetConditionDataFromComponent()
        {
            (this._item as LOPWLTestItem).CreateCoefTable(DataCenter._sysSetting.CoefTableStartWL,
                                                    DataCenter._sysSetting.CoefTableEndWL,
                                                    DataCenter._sysSetting.CoefTableResolution);

            (this._item as LOPWLTestItem).IsACSourceMeter = true;

            this._item.ElecSetting[0].MsrtType = EMsrtType.FVMI;

            this._item.ElecSetting[0].ForceUnit = EVoltUnit.V.ToString();

            this._item.ElecSetting[0].ForceDelayTime = (double)this.dinForceDelay.Value;

            this._item.ElecSetting[0].ForceValue = (double)this.dinForceValue.Value;

            this._item.ElecSetting[0].ForceValueFrequency = this.dinForceFrequency.Value;

            this._item.ElecSetting[0].ForceTime = (double)this.dinForceTime.Value;

            this._item.ElecSetting[0].ForceTimeUnit = ETimeUnit.ms.ToString();

            this._item.ElecSetting[0].IsAutoForceRange = this._isAutoSelectForceRange;

            this._item.ElecSetting[0].MsrtRange = this.dinMsrtRange.Value;

            this._item.ElecSetting[0].MsrtProtection = this.dinMsrtClamp.Value;

            this._item.ElecSetting[0].MsrtFilterCount = this.numMsrtFilterCount.Value;

            this._item.ElecSetting[0].MsrtUnit = EAmpUnit.mA.ToString();

            this._item.ElecSetting[0].IsAutoTurnOff = false;

            //========================================
            // Spectrometer setting, optical setting
            //========================================
            (this._item as LOPWLTestItem).IsTestOptical = this.chkIsTestOptical.Checked;

            (this._item as LOPWLTestItem).OptiSetting.SensingMode = (ESensingMode)Enum.Parse(typeof(ESensingMode), this.cmbSensingMode.SelectedItem.ToString(), true);

            (this._item as LOPWLTestItem).OptiSetting.FixIntegralTime = this.dinFixIntegralTime.Value;

            (this._item as LOPWLTestItem).OptiSetting.LimitIntegralTime = this.dinLimitIntegralTime.Value;

            (this._item as LOPWLTestItem).OptiSetting.TrigDelayTime = this.dinTrigDelayTime.Value;

            if ((this._item as LOPWLTestItem).OptiSetting.SensingMode == ESensingMode.Fixed)
            {
                this._item.ElecSetting[0].ForceTimeExt = this.dinFixIntegralTime.Value;
            }
            else
            {
                this._item.ElecSetting[0].ForceTimeExt = this.dinLimitIntegralTime.Value;
            }

            return this._item;
        }

        #endregion
    }
}
