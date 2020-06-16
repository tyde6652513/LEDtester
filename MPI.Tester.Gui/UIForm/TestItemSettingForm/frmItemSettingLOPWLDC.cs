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
    public partial class frmItemSettingLOPWLDC : Form, IConditionUICtrl, IConditionElecCtrl
    {
        private object _lockObj;

        private TestItemData _item;
     
        private bool _isAutoSelectForceRange;
        private bool _isAutoSelectMsrtRange;
        private bool _isEnableNPLC;
        private bool _isEnableFilter;
        private bool _isEnableSwitchChannel;
        private bool _isEnableMsrtForceValue;
        private uint _maxSwitchingChannelCnt;

        private bool _isDevInPulseRegion;

        private double _dcAndPulseBoundary;
        private double _minPulseWidth;
        private double _maxPulseWidth;

        private double _maxDuty;
        private double _minPulseMsrtRange;
        private double _maxPulseMsrtRange;
        private double _defaultPulseMsrtRange;

        private Dictionary<string, string> _dictRefItem;

        public frmItemSettingLOPWLDC()
        {
            InitializeComponent();
            
            this._lockObj = new object();

            this._item = new LOPWLTestItem();

            this.cmbSensingMode.Items.Clear();
            this.cmbSensingMode.Items.AddRange(Enum.GetNames(typeof(ESensingMode)));
            this.cmbSensingMode.SelectedItem = ESensingMode.Fixed.ToString();

            this.cmbForceMode.Items.Clear();
            this.cmbForceMode.Items.Add("Current Source");
            this.cmbForceMode.Items.Add("Voltage Source");
            this.cmbForceMode.SelectedIndex = 0;

            //---------------------------------------------------------------------------
            this.cmbSourceFunc.Items.Clear();

            foreach (var e in Enum.GetValues(typeof(ESourceFunc)))
            {
                this.cmbSourceFunc.Items.Add(e);
            }
           
            //---------------------------------------------------------------------------

            this._isAutoSelectForceRange = false;
            this._isAutoSelectMsrtRange = false;
            this._isEnableNPLC = false;
            this._isEnableFilter = false;

            this.cmbSourceFunc.SelectedIndexChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.dinForceValue.ValueChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.dinForceTime.ValueChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.dinDutyCycle.ValueChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.dinFixIntegralTime.ValueChanged += new System.EventHandler(this.UpdateDataEventHandler);

            this.cmbSensingMode.SelectedIndexChanged += new System.EventHandler(this.UpdateOpticalComponentEventHandler);
            this.chkIsTestOptical.CheckedChanged += new System.EventHandler(this.UpdateOpticalComponentEventHandler);

            this.chkIsTestOptical.Checked = true;
            this._isEnableSwitchChannel = false;
            this._isEnableMsrtForceValue = false;
            this._maxSwitchingChannelCnt = 0;
        }

        public frmItemSettingLOPWLDC(TestItemDescription description) : this()
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

        public bool IsVisibleForceMode
        {
            get { return this.pnlForceMode.Visible; }
            set { lock (this._lockObj) { this.pnlForceMode.Visible = value; } }
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
            get { return this._maxSwitchingChannelCnt; }
            set { lock (this._lockObj) { this._maxSwitchingChannelCnt = value; } }
        }

        public Dictionary<string, string> RefItem
        {
            set { lock (this._lockObj) { this._dictRefItem = value; } }
        }

        #endregion

        #region >>> Private Method <<<

        private void UpdateItemBoudary(TestItemDescription description)
        {
            if (description == null || description.Count == 0)
            {
                return;
            }

            bool isEnableDevPulseRegion = false;

            if (description[EItemDescription.IsPulseMode.ToString()].DefaultValue == 1.0d)
            {
                isEnableDevPulseRegion = true;
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
                            if (!isEnableDevPulseRegion)
                            {
                                this.dinForceValue.MaxValue = data.MaxValue;
                            }
                            
                            this.dinForceValue.MinValue = data.MinValue;
                            this.dinForceValue.Value = data.DefaultValue;
                            this.dinForceValue.DisplayFormat = data.Format;
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
                    case EItemDescription.PulseDuty:
                        {
                            //this.dinDutyCycle.MaxValue = data.MaxValue;

                            this._maxDuty = data.MaxValue;
                            this.dinDutyCycle.MinValue = data.MinValue;
                            this.dinDutyCycle.Value = data.DefaultValue;
                            this.dinDutyCycle.DisplayFormat = data.Format;

                            break;
                        }
                    case EItemDescription.PulseValue:
                        {
                            if (isEnableDevPulseRegion)
                            {
                                this.dinForceValue.MaxValue = data.MaxValue;
                            }

                            this._dcAndPulseBoundary = data.MinValue;

                            break;
                        }
                    case EItemDescription.PulseWidth:
                        {
                            this._minPulseWidth = data.MinValue;
                            this._maxPulseWidth = data.MaxValue;
                            break;
                        }
                    case EItemDescription.PulseMsrtRange:
                        {
                            this._minPulseMsrtRange = data.MinValue;
                            this._maxPulseMsrtRange = data.MaxValue;
                            this._defaultPulseMsrtRange = data.DefaultValue;

                            break;
                        }
                    case EItemDescription.EnableFloatForceValue:
                        {
                            this.pnlEnableFloatApplyValue.Visible = false;

                            if (data.DefaultValue != 0)
                            {
                                this.pnlEnableFloatApplyValue.Visible = true;
                            }

                            break;
                        }
                    default:
                        break;
                }
            }
        }

        private void UpdateDataEventHandler(object sender, EventArgs e)
        {
            if (!this.IsHandleCreated)
            {
                return;
            }

            if (this.cmbSourceFunc.SelectedIndex < 0)
            {
                return;
            }

            double forceValue = this.dinForceValue.Value;
            double forcaTime = this.dinForceTime.Value;
            double duty = this.dinDutyCycle.Value;
            double offTime = 0.0d;
            double intTime = this.dinFixIntegralTime.Value;
            int pulseCnt = 1;

            this._isDevInPulseRegion = false;

            if (forceValue > this._dcAndPulseBoundary) //mA
            {
                this._isDevInPulseRegion = true;
            }

            ESourceFunc func = (ESourceFunc)this.cmbSourceFunc.SelectedItem;

            switch (func)
            {
                case ESourceFunc.CW:
                    {
                        this.pnlDutyCycle.Visible = false;
                        this.pnlTurnOffTime.Visible = false;
                        this.pnlPulseCnt.Visible = false;

                        this.lblForceTime.Visible = true;
                        this.lblPulseWidth.Visible = false;

                        this.lblNplcAuto.Visible = false;
                        this.dinNPLC.Visible = true;
             
                        break;
                    }
                case ESourceFunc.PULSE:
                    {
                        this.pnlDutyCycle.Visible = false;
                        this.pnlTurnOffTime.Visible = false;
                        this.pnlPulseCnt.Visible = true;

                        this.lblForceTime.Visible = false;
                        this.lblPulseWidth.Visible = true;

                        this.lblNplcAuto.Visible = true;
                        this.dinNPLC.Visible = false;

                        if (this._isDevInPulseRegion)
                        {
                            offTime = GuiCommon.CalcOffTimeByDutyCycle(forcaTime, this._maxDuty);
                        }
                        else
                        {
                            offTime = 0.0d;
                        }
                        pulseCnt = 1;
                        break;
                    }
                case ESourceFunc.QCW:
                    {
                        this.pnlDutyCycle.Visible = true;
                        this.pnlTurnOffTime.Visible = true;
                        this.pnlPulseCnt.Visible = true;

                        this.lblForceTime.Visible = false;
                        this.lblPulseWidth.Visible = true;

                        this.lblNplcAuto.Visible = true;
                        this.dinNPLC.Visible = false;

                        offTime = GuiCommon.CalcOffTimeByDutyCycle(forcaTime, duty);
                        pulseCnt = (int)this.CalcPulseCount(intTime, forcaTime, offTime);
                        break;
                    }
            }
           
            this.dinTurnOffTime.Value = offTime;
            this.numPulseCnt.Value = pulseCnt;
        }

        private void UpdateOpticalComponentEventHandler(object sender, EventArgs e)
        {
            // Spt
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

        private void cmbForceMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.IsHandleCreated)
            {
                return;
            }

            if (this.cmbForceMode.SelectedIndex == 0)
            {
                this.lblForceValueUnit.Text = "mA";

                this.lblMsrtRangeUnit.Text = "V";

                this.lblMsrtClampUnit.Text = "V";

                this.dinForceValue.DisplayFormat = "0.0000";
            }
            else
            {
                this.lblForceValueUnit.Text = "V";

                this.lblMsrtRangeUnit.Text = "mA";

                this.lblMsrtClampUnit.Text = "mA";

                this.dinForceValue.DisplayFormat = "0.00";
            }
        }

        private uint CalcPulseCount(double intTime, double onTime, double offTime)
        {
            uint pulseCnt = 0;

            double period = onTime + offTime;

            if (period != 0)
            {
                pulseCnt = (uint)(intTime / period);
            }

            pulseCnt = pulseCnt >= 2 ? pulseCnt : 2;

            return pulseCnt;
        }

        private void UpedateAvailableChannelsToUI(uint channelCount)
        {
            this.cmbSelectedChannel.Items.Clear();

            if (channelCount == 0)
            {
                return;
            }

            for (int i = 0; i < channelCount; i++)
            {
                this.cmbSelectedChannel.Items.Add((i + 1).ToString());
            }

            this.cmbSelectedChannel.SelectedIndex = 0;
        }

        private void chkIsFloatApplyValue_CheckedChanged(object sender, EventArgs e)
        {
            bool isVisible = this.chkIsFloatApplyValue.Checked;

            this.pnlFloatingWL.Visible = isVisible;
            this.pnlForceValue.Visible = !isVisible;
        }

        #endregion

        #region >>> Public Method <<<

        public void RefreshUI()
        {
            this.pnlDutyCycle.Visible = false;
            this.pnlTurnOffTime.Visible = false;
            this.pnlPulseCnt.Visible = false;

            this.chkIsFloatApplyValue.Checked = false;

            this.cmbSourceFunc.SelectedItem = ESourceFunc.CW;

            if (this._isEnableSwitchChannel)
            {
                this.UpedateAvailableChannelsToUI(this._maxSwitchingChannelCnt);
            }
            else
            {
                this.grpChannel.Visible = false;
                this.grpApplySetting.Dock = DockStyle.Fill;
            }

            //--------------------------------------------------------------
            // Floating Force Value Setting
            this.chkIsFloatApplyValue.Checked = false;
            this.dinOffset.Value = 0.0d;

            this.cmbRefForceValue.Items.Clear();

            if (this._dictRefItem.Count > 0)
            {
                BindingSource bs = new BindingSource();

                bs.DataSource = this._dictRefItem;

                this.cmbRefForceValue.DataSource = bs;

                this.cmbRefForceValue.DisplayMember = "Key";

                this.cmbRefForceValue.ValueMember = "Key";

                this.cmbRefForceValue.SelectedIndex = 0;
            }
        }

        public bool CheckUI(out string msg)
        {
            msg = string.Empty;

            if (this.chkIsFloatApplyValue.Checked)
            {
                if (this._dictRefItem.Count == 0)
                {
                    msg = "Can not find the Measured Value,\nplease cancel this item, and then set test item PIV frist.";

                    return false;
                }
            }


            ESourceFunc func = (ESourceFunc)this.cmbSourceFunc.SelectedItem;

            if (func == ESourceFunc.CW)
            {
                if (this._isDevInPulseRegion)
                {
                    msg = "Invalid Current Range, the range NOT support in CW Mode";
                    return false;
                }
            }
            else
            {
                if (this.cmbSensingMode.SelectedItem.ToString() != ESensingMode.Fixed.ToString())
                {
                    msg = "Only \"Fixed sensing mode\" is available to perform Pulsed output";

                    return false;
                }

                if (this.dinForceTime.Value > this.dinFixIntegralTime.Value)
                {
                    msg = "Pulse width >= Fix Integrating Time";

                    return false;
                }

                if (this.dinFixIntegralTime.Value < 5)
                {
                    msg = "At least 5 ms for Fix Integrating Time in Pulse / QCW Mode";

                    return false;
                }

                if (this._isDevInPulseRegion)
                {
                    double forceTime = this.dinForceTime.Value;

                    if (forceTime < this._minPulseWidth || forceTime > this._maxPulseWidth)
                    {
                        msg = string.Format("Pulse Width Range is from {0}ms to {1}ms.", this._minPulseWidth, this._maxPulseWidth);

                        return false;
                    }

                    double duty = this.dinDutyCycle.Value;

                    if (func == ESourceFunc.QCW)
                    {
                        if (duty > this._maxDuty)
                        {
                            msg = string.Format("The Maximum Duty is {0}%.", this._maxDuty);

                            return false;
                        }
                    }

                    double msrtClamp = this.dinMsrtClamp.Value;

                    if (msrtClamp < this._minPulseMsrtRange || msrtClamp > this._maxPulseMsrtRange)
                    {
                        msg = string.Format("Pulse Msrt. Range/Clamp is from {0}V to {1}V.", this._minPulseMsrtRange, this._maxPulseMsrtRange);

                        return false;
                    }
                }

                if (func == ESourceFunc.QCW)
                {
                    if ((this.dinForceTime.Value + this.dinTurnOffTime.Value) * this.numPulseCnt.Value > this.dinFixIntegralTime.Value)
                    {
                        msg = "The output period exceeds the Fix Integrating Time.";

                        return false;
                    }
                }
            }
          
            return true;
        }

        public void UpdateCondtionDataToComponent(TestItemData data)
        {
            this._item = (data as LOPWLTestItem).Clone() as LOPWLTestItem;

            // ElecSettingData
            if (this._item.ElecSetting[0].MsrtType == EMsrtType.FIMV)
            {
                this.cmbForceMode.SelectedIndex = 0;
            }
            else
            {
                this.cmbForceMode.SelectedIndex = 1;
            }

            this.dinForceDelay.Value = this._item.ElecSetting[0].ForceDelayTime;

            this.dinForceValue.Value = this._item.ElecSetting[0].ForceValue;

            this.dinForceTime.Value = this._item.ElecSetting[0].ForceTime;

            this.dinMsrtRange.Value = this._item.ElecSetting[0].MsrtRange;

            this.dinMsrtClamp.Value = this._item.ElecSetting[0].MsrtProtection;

            this.numMsrtFilterCount.Value = this._item.ElecSetting[0].MsrtFilterCount;

            this.dinNPLC.Value = this._item.ElecSetting[0].MsrtNPLC;

            // OptiSetting
            this.chkIsTestOptical.Checked = (this._item as LOPWLTestItem).IsTestOptical;

            this.cmbSensingMode.SelectedItem = (this._item as LOPWLTestItem).OptiSetting.SensingMode.ToString();

            this.dinFixIntegralTime.Value = (this._item as LOPWLTestItem).OptiSetting.FixIntegralTime;

            this.dinLimitIntegralTime.Value = (this._item as LOPWLTestItem).OptiSetting.LimitIntegralTime;

            this.dinTrigDelayTime.Value = (this._item as LOPWLTestItem).OptiSetting.TrigDelayTime;

            // Pulse mode
            this.cmbSourceFunc.SelectedItem = (ESourceFunc)this._item.ElecSetting[0].SourceFunction;

            this.dinDutyCycle.Value = this._item.ElecSetting[0].Duty * 100.0d;

            this.dinTurnOffTime.Value = this._item.ElecSetting[0].TurnOffTime;

            if (this._isEnableSwitchChannel)
            {
                if (this.cmbSelectedChannel.Items.Count > this._item.SwitchingChannel)
                {
                    this.cmbSelectedChannel.SelectedIndex = (int)this._item.SwitchingChannel;
                }
                else
                {
                    this.cmbSelectedChannel.SelectedIndex = -1;
                }
            }

            if ((this._item as LOPWLTestItem).RefMsrtKeyName != null)
            {
                if (this._dictRefItem.ContainsKey((this._item as LOPWLTestItem).RefMsrtKeyName))
                {
                    this.cmbRefForceValue.SelectedValue = (this._item as LOPWLTestItem).RefMsrtKeyName;
                }
            }

            this.chkIsFloatApplyValue.Checked = (this._item as LOPWLTestItem).IsUseMsrtAsForceValue;

            this.dinOffset.Value = (this._item as LOPWLTestItem).Offset * 1000.0d;

            this.dinMaxRefForceValue.Value = (this._item as LOPWLTestItem).MaxProtectForceValue * 1000.0d;
        }

        public TestItemData GetConditionDataFromComponent()
        {
            (this._item as LOPWLTestItem).CreateCoefTable(DataCenter._sysSetting.CoefTableStartWL,
                                                    DataCenter._sysSetting.CoefTableEndWL,
                                                    DataCenter._sysSetting.CoefTableResolution);

            (this._item as LOPWLTestItem).IsACSourceMeter = false;

            this._item.ElecSetting[0].SourceFunction = (ESourceFunc)this.cmbSourceFunc.SelectedItem;
            this._item.ElecSetting[1].SourceFunction = (ESourceFunc)this.cmbSourceFunc.SelectedItem;

            if (this.cmbForceMode.SelectedIndex == 0)
            {
                //===============================
                // IFWLA, MsrtType = FIMV
                //===============================
                this._item.ElecSetting[0].MsrtType = EMsrtType.FIMV;

                this._item.ElecSetting[0].ForceUnit = EAmpUnit.mA.ToString();

                this._item.ElecSetting[0].ForceDelayTime = (double)this.dinForceDelay.Value;

                this._item.ElecSetting[0].ForceValue = (double)this.dinForceValue.Value;

                this._item.ElecSetting[0].ForceTime = (double)this.dinForceTime.Value;

                //this._item.ElecSetting[0].ForceRangeIndex = this.cmbForceRangeLOPWL.SelectedIndex;

                this._item.ElecSetting[0].ForceTimeUnit = ETimeUnit.ms.ToString();

                this._item.ElecSetting[0].IsAutoForceRange = this._isAutoSelectForceRange;

                this._item.ElecSetting[0].MsrtRange = this.dinMsrtRange.Value;

                this._item.ElecSetting[0].MsrtProtection = this.dinMsrtClamp.Value;

                this._item.ElecSetting[0].MsrtFilterCount = this.numMsrtFilterCount.Value;

                this._item.ElecSetting[0].MsrtUnit = EVoltUnit.V.ToString();

                this._item.ElecSetting[0].MsrtNPLC = this.dinNPLC.Value;

                this._item.ElecSetting[0].IsAutoTurnOff = false;

                this._item.ElecSetting[0].IsPulseMode = this._isDevInPulseRegion;

                this._item.ElecSetting[0].Duty = this.dinDutyCycle.Value / 100.0d;

                this._item.ElecSetting[0].TurnOffTime = this.dinTurnOffTime.Value;

                this._item.ElecSetting[0].PulseCount = (uint)this.numPulseCnt.Value;

                //===============================
                // IFWLB, MsrtType = FIMV
                //===============================
                this._item.ElecSetting[1].MsrtType = EMsrtType.FIMV;

                this._item.ElecSetting[1].ForceUnit = EAmpUnit.mA.ToString();

                this._item.ElecSetting[1].ForceDelayTime = 0.0d;

                this._item.ElecSetting[1].ForceValue = (double)this.dinForceValue.Value;

                this._item.ElecSetting[1].ForceTime = 0.5d;

                this._item.ElecSetting[1].ForceTimeUnit = ETimeUnit.ms.ToString();

                this._item.ElecSetting[1].IsAutoForceRange = this._isAutoSelectForceRange;

                this._item.ElecSetting[1].MsrtRange = this.dinMsrtRange.Value;

                this._item.ElecSetting[1].MsrtProtection = this.dinMsrtClamp.Value;

                this._item.ElecSetting[1].MsrtFilterCount = this.numMsrtFilterCount.Value;

                this._item.ElecSetting[1].MsrtUnit = EVoltUnit.V.ToString();

                this._item.ElecSetting[1].MsrtNPLC = this.dinNPLC.Value;

                this._item.ElecSetting[1].IsAutoTurnOff = true;

                 this._item.ElecSetting[1].TurnOffTime = 0.0d;

                 this._item.ElecSetting[1].IsPulseMode = this._isDevInPulseRegion;
            }
            else
            {
                //===============================
                // VFWLA, MsrtType = FVMI
                //===============================
                this._item.ElecSetting[0].MsrtType = EMsrtType.FVMI;

                this._item.ElecSetting[0].ForceUnit = EVoltUnit.V.ToString();

                this._item.ElecSetting[0].ForceDelayTime = (double)this.dinForceDelay.Value;

                this._item.ElecSetting[0].ForceValue = (double)this.dinForceValue.Value;

                this._item.ElecSetting[0].ForceTime = (double)this.dinForceTime.Value;

                //this._item.ElecSetting[0].ForceRangeIndex = this.cmbForceRangeLOPWL.SelectedIndex;

                this._item.ElecSetting[0].ForceTimeUnit = ETimeUnit.ms.ToString();

                this._item.ElecSetting[0].IsAutoForceRange = this._isAutoSelectForceRange;

                this._item.ElecSetting[0].MsrtRange = this.dinMsrtRange.Value;

                this._item.ElecSetting[0].MsrtProtection = this.dinMsrtClamp.Value;

                this._item.ElecSetting[0].MsrtFilterCount = this.numMsrtFilterCount.Value;

                this._item.ElecSetting[0].MsrtUnit = EAmpUnit.mA.ToString();

                this._item.ElecSetting[0].MsrtNPLC = this.dinNPLC.Value;

                this._item.ElecSetting[0].IsAutoTurnOff = false;

                //===============================
                // VFWLB, MsrtType = FVMI
                //===============================
                this._item.ElecSetting[1].MsrtType = EMsrtType.FVMI;

                this._item.ElecSetting[1].ForceUnit = EVoltUnit.V.ToString();

                this._item.ElecSetting[1].ForceDelayTime = 0.0d;

                this._item.ElecSetting[1].ForceValue = (double)this.dinForceValue.Value;

                this._item.ElecSetting[1].ForceTime = 0.5d;

                this._item.ElecSetting[1].ForceTimeUnit = ETimeUnit.ms.ToString();

                this._item.ElecSetting[1].IsAutoForceRange = this._isAutoSelectForceRange;

                this._item.ElecSetting[1].MsrtRange = this.dinMsrtRange.Value;

                this._item.ElecSetting[1].MsrtProtection = this.dinMsrtClamp.Value;

                this._item.ElecSetting[1].MsrtFilterCount = this.numMsrtFilterCount.Value;

                this._item.ElecSetting[1].MsrtUnit = EAmpUnit.mA.ToString();

                this._item.ElecSetting[1].MsrtNPLC = this.dinNPLC.Value;

                this._item.ElecSetting[1].IsAutoTurnOff = true;
            }

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

            if (this._isEnableSwitchChannel)
            {
                this._item.SwitchingChannel = (uint)this.cmbSelectedChannel.SelectedIndex;
            }

            (this._item as LOPWLTestItem).IsUseMsrtAsForceValue = this.chkIsFloatApplyValue.Checked;

            if (this._dictRefItem.Count > 0)
            {
                (this._item as LOPWLTestItem).RefMsrtKeyName = this.cmbRefForceValue.SelectedValue.ToString();

                (this._item as LOPWLTestItem).RefMsrtName = this._dictRefItem[(this._item as LOPWLTestItem).RefMsrtKeyName];
            }

            (this._item as LOPWLTestItem).Offset = Math.Round((this.dinOffset.Value / 1000.0d), 3, MidpointRounding.AwayFromZero);

            (this._item as LOPWLTestItem).MaxProtectForceValue = Math.Round((this.dinMaxRefForceValue.Value / 1000.0d), 3, MidpointRounding.AwayFromZero);

            return this._item;
        }

        #endregion
    }
}
