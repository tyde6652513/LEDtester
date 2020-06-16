using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Data;
using MPI.Tester.Maths;

namespace MPI.Tester.Gui
{
    public partial class frmItemSettingLOP : Form, IConditionUICtrl, IConditionElecCtrl
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

        private bool _isSupportedDetectorB;

        private bool _isDevInPulseRegion;

        private double _dcAndPulseBoundary;
        private double _minPulseWidth;
        private double _maxPulseWidth;

        private double _maxDuty;
        private double _minPulseMsrtRange;
        private double _maxPulseMsrtRange;
        private double _defaultPulseMsrtRange;

        public frmItemSettingLOP()
        {
            InitializeComponent();
            
            this._lockObj = new object();

            this._item = new LOPTestItem();

            this.cmbForceMode.Items.Clear();
            this.cmbForceMode.Items.Add("Current Source");
            this.cmbForceMode.Items.Add("Voltage Source");
            this.cmbForceMode.SelectedIndex = 0;

            this._isAutoSelectForceRange = false;
            this._isAutoSelectMsrtRange = false;
            this._isEnableNPLC = false;
            this._isEnableFilter = false;
            this._isEnableSwitchChannel = false;
            this._isEnableMsrtForceValue = false;
            this._maxSwitchingChannelCnt = 0;

            this._isSupportedDetectorB = false;

            //---------------------------------------------------------------------------
            this.cmbSourceFunc.Items.Clear();

            foreach (var e in Enum.GetValues(typeof(ESourceFunc)))
            {
                this.cmbSourceFunc.Items.Add(e);
            }

            this.cmbSourceFunc.Items.Remove(ESourceFunc.QCW);

            //---------------------------------------------------------------------------
            this.cmbSourceFunc.SelectedIndexChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.dinForceValue.ValueChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.dinDutyCycle.ValueChanged += new System.EventHandler(this.UpdateDataEventHandler);
        }

        public frmItemSettingLOP(TestItemDescription description) : this()
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
                        this.pnlDetectorNPLC.Visible = value;
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

        public bool IsSupportedDetectorB
        {
            get { return this._isSupportedDetectorB; }
            set { lock (this._lockObj) { this._isSupportedDetectorB = value; } }
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
                            this.pnlDetectorNPLC.Visible = data.IsEnable & data.IsVisible;

                            this._isEnableNPLC = data.IsEnable;

                            this.dinNPLC.MaxValue = data.MaxValue;
                            this.dinNPLC.MinValue = data.MinValue;
                            this.dinNPLC.Value = data.DefaultValue;
                            this.dinNPLC.DisplayFormat = data.Format;

                            this.dinDetectorNPLC.MaxValue = data.MaxValue;
                            this.dinDetectorNPLC.MinValue = data.MinValue;
                            this.dinDetectorNPLC.Value = data.DefaultValue;
                            this.dinDetectorNPLC.DisplayFormat = data.Format;
                            break;
                        }
                    case EItemDescription.DetectorMsrtRange:
                        {
                            this.dinDetectorMsrtRange.MaxValue = data.MaxValue;
                            this.dinDetectorMsrtRange.MinValue = data.MinValue;
                            this.dinDetectorMsrtRange.Value = data.DefaultValue;
                            this.dinDetectorMsrtRange.DisplayFormat = data.Format;
                            this.lblDetectorMsrtRangeUnit.Text = data.Unit;

                            this.dinDetectorMsrtRangeB.MaxValue = data.MaxValue;
                            this.dinDetectorMsrtRangeB.MinValue = data.MinValue;
                            this.dinDetectorMsrtRangeB.Value = data.DefaultValue;
                            this.dinDetectorMsrtRangeB.DisplayFormat = data.Format;
                            this.lblDetectorMsrtRangeUnitB.Text = data.Unit;

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
                    default:
                        break;
                }
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

            //-------------------------------------------------------------------------------
            this._isDevInPulseRegion = false;

            if (forceValue > this._dcAndPulseBoundary) //mA
            {
                this._isDevInPulseRegion = true;

            }

            //-------------------------------------------------------------------------------
            double offTime = 0.0d;
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
                        this.pnlQcwFilterCount.Visible = false;

                        this.lblNplcAuto.Visible = false;
                        this.dinNPLC.Visible = true;

                        this.lblDetectorNplcAuto1.Visible = false;
                        this.dinDetectorNPLC.Visible = true;

                        this.lblDetectorNplcAuto2.Visible = false;
                        this.dinDetectorNPLCB.Visible = true;

                        offTime = 0.0d;

                        break;
                    }
                case ESourceFunc.PULSE:
                    {
                        this.pnlDutyCycle.Visible = false;
                        this.pnlTurnOffTime.Visible = false;
                        this.pnlPulseCnt.Visible = false;

                        this.lblForceTime.Visible = false;
                        this.lblPulseWidth.Visible = true;
                        this.pnlQcwFilterCount.Visible = false;

                        this.lblNplcAuto.Visible = true;
                        this.dinNPLC.Visible = false;

                        this.lblDetectorNplcAuto1.Visible = true;
                        this.dinDetectorNPLC.Visible = false;

                        this.lblDetectorNplcAuto2.Visible = true;
                        this.dinDetectorNPLCB.Visible = false;

                        if (this._isDevInPulseRegion)
                        {
                            offTime = GuiCommon.CalcOffTimeByDutyCycle(forcaTime, this._maxDuty);
                        }
                        else
                        {
                            offTime = 0.0d;
                        }

                        break;
                    }
                case ESourceFunc.QCW:
                    {
                        this.pnlDutyCycle.Visible = true;
                        this.pnlTurnOffTime.Visible = true;
                        this.pnlPulseCnt.Visible = true;

                        this.lblForceTime.Visible = false;
                        this.lblPulseWidth.Visible = true;

                        this.pnlQcwFilterCount.Visible = true;

                        this.lblNplcAuto.Visible = true;
                        this.dinNPLC.Visible = false;

                        this.lblDetectorNplcAuto1.Visible = true;
                        this.dinDetectorNPLC.Visible = false;

                        this.lblDetectorNplcAuto2.Visible = true;
                        this.dinDetectorNPLCB.Visible = false;

                        offTime = GuiCommon.CalcOffTimeByDutyCycle(forcaTime, duty);

                        break;
                    }
            }

            this.dinTurnOffTime.Value = offTime;
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

        #endregion

        #region >>> Public Method <<<

        public void RefreshUI()
        {
            this.dinDetectorBiasVoltage.Value = 0;
            this.dinDetectorBiasVoltageB.Value = 0;

            this.chkIsEnableDetector.Checked = true;

            bool isVisibleDetectorB = this._isSupportedDetectorB;

            this.chkIsEnableDetectorB.Visible = isVisibleDetectorB;
            this.pnlDetectorBiasVoltageB.Visible = isVisibleDetectorB;
            this.pnlDetectorMsrtRangeB.Visible = isVisibleDetectorB;
            this.pnlDetectorNPLCB.Visible = isVisibleDetectorB;

            this.chkIsEnableDetector.Enabled = isVisibleDetectorB;

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

            this.UpdateDataEventHandler(null, null);
        }

        public bool CheckUI(out string msg)
        {
            msg = string.Empty;

            if (!this.chkIsEnableDetector.Checked && !this.chkIsEnableDetectorB.Checked)
            {
                msg = "No Detector Channel has been selected.";
                return false;
            }

            ESourceFunc func = (ESourceFunc)this.cmbSourceFunc.SelectedItem;

            if (this._isDevInPulseRegion)
            {
                if (func == ESourceFunc.CW)
                {
                    msg = "Invalid Current Range, the range NOT support in CW Mode";
                    return false;
                }
                
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
                if (this.numPulseCnt.Value < this.numQcwFilterCount.Value)
                {
                    msg = string.Format("The index for calculation out of the step pulse count in QCW Mode.\n");

                    return false;
                }
            }

            return true;
        }

        public void UpdateCondtionDataToComponent(TestItemData data)
        {
            this._item = (data as LOPTestItem).Clone() as LOPTestItem;

            if (this._item.ElecSetting[0].MsrtType == EMsrtType.FIMVLOP)
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

            this.cmbSourceFunc.SelectedItem = (ESourceFunc)this._item.ElecSetting[0].SourceFunction;

            this.dinDutyCycle.Value = this._item.ElecSetting[0].Duty * 100.0d;

            this.numPulseCnt.Value = (int)this._item.ElecSetting[0].PulseCount;

            this.numQcwFilterCount.Value = (int)this._item.ElecSetting[0].CalcMsrtFromPulseIndex;

            // Detector CH-A Setting
            this.chkIsEnableDetector.Checked = this._item.ElecSetting[0].IsTrigDetector;

            this.dinDetectorBiasVoltage.Value = this._item.ElecSetting[0].DetectorBiasValue;

            this.dinDetectorNPLC.Value = this._item.ElecSetting[0].DetectorMsrtNPLC;

            double factor = UnitMath.ToSIUnit(lblDetectorMsrtRangeUnit.Text);

            double val = this._item.ElecSetting[0].DetectorMsrtRange / factor;

            val = Math.Round(val, 9, MidpointRounding.AwayFromZero);

            this.dinDetectorMsrtRange.Value = val;

            // Detector CH-B Setting
            this.chkIsEnableDetectorB.Checked = this._item.ElecSetting[0].IsTrigDetector2;

            this.dinDetectorBiasVoltageB.Value = this._item.ElecSetting[0].DetectorBiasValue2 ;

            val = this._item.ElecSetting[0].DetectorMsrtRange2 / factor;

            val = Math.Round(val, 9, MidpointRounding.AwayFromZero);

            this.dinDetectorMsrtRangeB.Value = val;

            this.dinDetectorNPLCB.Value = this._item.ElecSetting[0].DetectorMsrtNPLC2;

            if (!this._isSupportedDetectorB)
            {
                // 若不支援 Dual Detector CH, Detector CH-A 需強制啟動
                this.chkIsEnableDetector.Checked = true;
            }

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
        }

        public TestItemData GetConditionDataFromComponent()
        {
            if (this.cmbForceMode.SelectedIndex == 0)
            {
                //------------------------------------------------------------------------------------
                // FIMVLOP
                //------------------------------------------------------------------------------------
                this._item.ElecSetting[0].MsrtType = EMsrtType.FIMVLOP;

                this._item.ElecSetting[0].ForceUnit = EAmpUnit.mA.ToString();

                this._item.ElecSetting[0].ForceDelayTime = (double)this.dinForceDelay.Value;

                this._item.ElecSetting[0].ForceValue = (double)this.dinForceValue.Value;

                this._item.ElecSetting[0].ForceTime = (double)this.dinForceTime.Value;

                //this._item.ElecSetting[0].ForceRangeIndex = this.cmbForceRange.SelectedIndex;

                this._item.ElecSetting[0].ForceTimeUnit = ETimeUnit.ms.ToString();

                this._item.ElecSetting[0].IsAutoForceRange = this._isAutoSelectForceRange;

                this._item.ElecSetting[0].MsrtRange = this.dinMsrtRange.Value;

                this._item.ElecSetting[0].MsrtProtection = this.dinMsrtClamp.Value;

                this._item.ElecSetting[0].MsrtFilterCount = this.numMsrtFilterCount.Value;

                this._item.ElecSetting[0].MsrtUnit = EVoltUnit.V.ToString();

                this._item.ElecSetting[0].IsAutoTurnOff = true;

                this._item.ElecSetting[0].MsrtNPLC = this.dinNPLC.Value;
            }
            else
            {
                //------------------------------------------------------------------------------------
                // FVMILOP
                //------------------------------------------------------------------------------------
                this._item.ElecSetting[0].MsrtType = EMsrtType.FVMILOP;

                this._item.ElecSetting[0].ForceUnit = EVoltUnit.V.ToString();

                this._item.ElecSetting[0].ForceDelayTime = (double)this.dinForceDelay.Value;

                this._item.ElecSetting[0].ForceValue = (double)this.dinForceValue.Value;

                this._item.ElecSetting[0].ForceTime = (double)this.dinForceTime.Value;

               // this._item.ElecSetting[0].ForceRangeIndex = this.cmbForceRange.SelectedIndex;

                this._item.ElecSetting[0].ForceTimeUnit = ETimeUnit.ms.ToString();

                this._item.ElecSetting[0].IsAutoForceRange = this._isAutoSelectForceRange;

                this._item.ElecSetting[0].MsrtRange = this.dinMsrtRange.Value;

                this._item.ElecSetting[0].MsrtProtection = this.dinMsrtClamp.Value;

                this._item.ElecSetting[0].MsrtFilterCount = this.numMsrtFilterCount.Value;

                this._item.ElecSetting[0].MsrtUnit = EAmpUnit.mA.ToString();

                this._item.ElecSetting[0].IsAutoTurnOff = true;

                this._item.ElecSetting[0].MsrtNPLC = this.dinNPLC.Value;
            }

            this._item.ElecSetting[0].SourceFunction = (ESourceFunc)this.cmbSourceFunc.SelectedItem;

            this._item.ElecSetting[0].TurnOffTime = this.dinTurnOffTime.Value;

            this._item.ElecSetting[0].IsPulseMode = this._isDevInPulseRegion;

            this._item.ElecSetting[0].Duty = this.dinDutyCycle.Value / 100.0d;

            this._item.ElecSetting[0].PulseCount = (uint)this.numPulseCnt.Value;

            this._item.ElecSetting[0].CalcMsrtFromPulseIndex = (uint)this.numQcwFilterCount.Value;

            // Detector CH-A Setting
            this._item.ElecSetting[0].IsTrigDetector = this.chkIsEnableDetector.Checked;

            this._item.ElecSetting[0].DetectorBiasValue = this.dinDetectorBiasVoltage.Value;

            double factor = UnitMath.ToSIUnit(lblDetectorMsrtRangeUnit.Text);

            double val = this.dinDetectorMsrtRange.Value * factor;

            val = Math.Round(val, 9, MidpointRounding.AwayFromZero);

            this._item.ElecSetting[0].DetectorMsrtRange = val;

            this._item.ElecSetting[0].DetectorMsrtNPLC = this.dinDetectorNPLC.Value;

            // Detector CH-B Setting


            this._item.ElecSetting[0].IsTrigDetector2 = this.chkIsEnableDetectorB.Checked;

            this._item.ElecSetting[0].DetectorBiasValue2 = this.dinDetectorBiasVoltageB.Value;

            val = this.dinDetectorMsrtRangeB.Value * factor;

            val = Math.Round(val, 9, MidpointRounding.AwayFromZero);

            this._item.ElecSetting[0].DetectorMsrtRange2 = this.dinDetectorMsrtRangeB.Value * factor;

            this._item.ElecSetting[0].DetectorMsrtNPLC2 = this.dinDetectorNPLCB.Value;

            if (!this._isSupportedDetectorB)
            {
                // 若不支援 Dual Detector CH, Detector CH-A 需強制啟動
                this._item.ElecSetting[0].IsTrigDetector = true;
            }

            if (this._isEnableSwitchChannel)
            {
                this._item.SwitchingChannel = (uint)this.cmbSelectedChannel.SelectedIndex;
            }

            return this._item;
        }

        #endregion

    }
}

