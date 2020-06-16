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
    public partial class frmItemSettingLIV : Form, IConditionUICtrl, IConditionElecCtrl
    {
        private object _lockObj;

        private TestItemData _item;

        private bool _isAutoSelectForceRange;
        private bool _isAutoSelectMsrtRange;
        private bool _isEnableNPLC;
        private bool _isEnableFilter;
        private bool _isEnablePdDetector;
        private bool _isEnableSamplingTest;
        private bool _isEnableSwitchChannel;
        private bool _isEnableMsrtForceValue;

        public frmItemSettingLIV()
        {
            InitializeComponent();
            
            this._lockObj = new object();

            this._item = new LIVTestItem();

            this.cmbSweepMode.Items.Clear();
            this.cmbSweepMode.Items.AddRange(Enum.GetNames(typeof(ESweepMode)));
            this.cmbSweepMode.Items.Remove(ESweepMode.Custom.ToString());
            this.cmbSweepMode.SelectedIndex = 0;

            this.cmbSensingMode.Items.Clear();
            this.cmbSensingMode.Items.AddRange(Enum.GetNames(typeof(ESensingMode)));
            this.cmbSensingMode.SelectedItem = ESensingMode.Fixed.ToString();

            this.cmbForceMode.Items.Clear();
            this.cmbForceMode.Items.Add("Current Source");
            this.cmbForceMode.Items.Add("Voltage Source");
            this.cmbForceMode.SelectedIndex = 0;

            this._isAutoSelectForceRange = false;
            this._isAutoSelectMsrtRange = false;
            this._isEnableNPLC = false;
            this._isEnableFilter = false;

            this._isEnablePdDetector = false;    
            this._isEnableSamplingTest = false;

            this.cmbSweepMode.SelectedIndexChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.dinStartValue.ValueChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.dinEndValue.ValueChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.dinPoints.ValueChanged += new System.EventHandler(this.UpdateDataEventHandler);
         

            this.cmbSensingMode.SelectedIndexChanged += new System.EventHandler(this.UpdateOpticalComponentEventHandler);
            this.chkIsTestOptical.CheckedChanged += new System.EventHandler(this.UpdateOpticalComponentEventHandler);
            this.chkIsEnableDetector.CheckedChanged += new System.EventHandler(this.UpdateOpticalComponentEventHandler);

            this.numSamplingTime.Value = 1000;
            this.numLogScaleTime.Value = 1000;

            this.pnlSamplingTime.Enabled = false;
            this.pnlTestTime.Enabled = false;
            this.pnlLogScaleTime.Enabled = false;
            this._isEnableSwitchChannel = false;
            this._isEnableMsrtForceValue = false;
        }

        public frmItemSettingLIV(TestItemDescription description) : this()
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

                    if (this._isEnablePdDetector && this._isEnableNPLC)
                    {
                        this.pnlDetectorNPLC.Visible = value;
                    }
                }
            }
        }

        public bool IsEnablePDDetector
        {
            get { return this._isEnablePdDetector; }
            set { lock (this._lockObj) { this._isEnablePdDetector = value; } }
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
                    case EItemDescription.SweepStart:
                        {
                            this.dinStartValue.MaxValue = data.MaxValue;
                            this.dinStartValue.MinValue = data.MinValue;
                            this.dinStartValue.Value = data.DefaultValue;
                            this.dinStartValue.DisplayFormat = data.Format;
                            break;
                        }
                    case EItemDescription.SweepEnd:
                        {
                            this.dinEndValue.MaxValue = data.MaxValue;
                            this.dinEndValue.MinValue = data.MinValue;
                            this.dinEndValue.Value = data.DefaultValue;
                            this.dinEndValue.DisplayFormat = data.Format;
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
                    case EItemDescription.SweepRiseCount:
                        {
                            this.dinPoints.MaxValue = (int)data.MaxValue;
                            this.dinPoints.MinValue = (int)data.MinValue;
                            this.dinPoints.Value = (int)data.DefaultValue;

                            break;
                        }
                    case EItemDescription.SweepTurnOffTime:
                        {
                            this.dinTurnOffTime.MaxValue = data.MaxValue;
                            this.dinTurnOffTime.MinValue = data.MinValue;
                            this.dinTurnOffTime.Value = data.DefaultValue;
                            this.dinTurnOffTime.DisplayFormat = data.Format;

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

                            this.pnlDetectorNPLC.Visible = data.IsEnable & data.IsVisible;

                            this.dinDetectorNPLC.MaxValue = data.MaxValue;
                            this.dinDetectorNPLC.MinValue = data.MinValue;
                            this.dinDetectorNPLC.Value = data.DefaultValue;
                            this.dinDetectorNPLC.DisplayFormat = data.Format;

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
                    case EItemDescription.DetectorMsrtRange:
                        {
                            this.dinDetectorMsrtRange.MaxValue = data.MaxValue;
                            this.dinDetectorMsrtRange.MinValue = data.MinValue;
                            this.dinDetectorMsrtRange.Value = data.DefaultValue;
                            this.dinDetectorMsrtRange.DisplayFormat = data.Format;

                            break;
                        }
                    default:
                        break;
                }
            }
        }

        private void UpdateDataEventHandler(object sender, EventArgs e)
        {
            double stepValue = 0.0d;

            ESweepMode mode = (ESweepMode)Enum.Parse(typeof(ESweepMode), this.cmbSweepMode.SelectedItem.ToString());

            switch (mode)
            {
                case ESweepMode.Linear:
                    {
                        stepValue = (this.dinEndValue.Value - this.dinStartValue.Value) / ((int)this.dinPoints.Value - 1);

                        this.txtDisplayStepValue.Text = stepValue.ToString("0.0000");

                        break;
                    }
                case ESweepMode.Log:
                    {
                        if (this.dinStartValue.Value * this.dinEndValue.Value < 0)
                        {
                            this.dinEndValue.Value = this.dinEndValue.Value * -1;
                        }

                        this.txtDisplayStepValue.Text = "Log Step";

                        break;
                    }
            }
        }

        private void UpdateOpticalComponentEventHandler(object sender, EventArgs e)
        {
            // SPT
            if (this.chkIsTestOptical.Checked)
            {
                this.pnlSensingMode.Enabled = true;

                this.pnlFixedTime.Enabled = true;

                this.pnlLimitTime.Enabled = true;


                if (this.cmbSensingMode.SelectedItem.ToString() == ESensingMode.Limit.ToString() || this.cmbSensingMode.SelectedItem.ToString() == ESensingMode.Limit02.ToString())
                {
                    this.pnlFixedTime.Enabled = false;

                    this.pnlLimitTime.Enabled = true;

                    this.chkIsLimitModeFixedSIT.TextColor = System.Drawing.Color.BlueViolet;

                    this.chkIsLimitModeFixedSIT.Enabled = true;
                }
                else //if (this.cmbSensingMode.SelectedItem.ToString() == ESensingMode.Fixed.ToString())
                {
                    this.pnlFixedTime.Enabled = true;

                    this.pnlLimitTime.Enabled = false;

                    this.chkIsLimitModeFixedSIT.TextColor = System.Drawing.Color.Gray;

                    this.chkIsLimitModeFixedSIT.Enabled = false;

                    this.chkIsLimitModeFixedSIT.Checked = false;
                }
            }
            else
            {
                this.pnlSensingMode.Enabled = false;

                this.pnlFixedTime.Enabled = false;

                this.pnlLimitTime.Enabled = false;

                this.chkIsLimitModeFixedSIT.TextColor = System.Drawing.Color.Gray;

                this.chkIsLimitModeFixedSIT.Enabled = false;
            }

            this.chkIsLimitModeFixedSIT.Update();

            // PD Detector
            if (this.chkIsEnableDetector.Checked)
            {
                this.pnlDetectorBiasVoltage.Enabled = true;

                this.pnlDetectorMsrtRange.Enabled = true;

                this.pnlDetectorNPLC.Enabled = true;
            }
            else
            {
                this.pnlDetectorBiasVoltage.Enabled = false;

                this.pnlDetectorMsrtRange.Enabled = false;

                this.pnlDetectorNPLC.Enabled = false;
            }
        }

        private void chkLifeTest_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.chkLifeTest.Checked && !this.chkIsLogScale.Checked)
            {
                this.pnlSamplingTime.Enabled = false;

                this.pnlTestTime.Enabled = false;

                this.pnlLogScaleTime.Enabled = false;

                return;
            }

            if (this.chkLifeTest.Checked)
            {
                this.pnlSamplingTime.Enabled = true;

                this.pnlTestTime.Enabled = true;

                // Log Scale
                if (this.chkIsLogScale.Checked)
                {
                    this.chkIsLogScale.Checked = false;

                    this.pnlLogScaleTime.Enabled = false;
                }
            }
            else
            {
                this.pnlSamplingTime.Enabled = false;

                this.pnlTestTime.Enabled = false;

                // Log Scale
                this.pnlLogScaleTime.Enabled = true;
            }
        }

        private void chkIsLogScale_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.chkLifeTest.Checked && !this.chkIsLogScale.Checked)
            {
                this.pnlSamplingTime.Enabled = false;

                this.pnlTestTime.Enabled = false;

                this.pnlLogScaleTime.Enabled = false;

                return;
            }

            if (this.chkIsLogScale.Checked)
            {
                this.pnlLogScaleTime.Enabled = true;

                // Sampling
                if (this.chkLifeTest.Checked)
                {
                    this.chkLifeTest.Checked = false;

                    this.pnlSamplingTime.Enabled = false;

                    this.pnlTestTime.Enabled = false;
                }
            }
            else
            {
               this.pnlSamplingTime.Enabled = true;

                this.pnlTestTime.Enabled = true;

                // Log Scale
                this.pnlLogScaleTime.Enabled = false;
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
                this.lblStartValueUnit.Text = "mA";

                this.lblStepValueUnit.Text = "mA";

                this.lblEndValueUnit.Text = "mA";

                this.lblMsrtRangeUnit.Text = "V";

                this.lblMsrtClampUnit.Text = "V";

                this.dinStartValue.DisplayFormat = "0.000";

                this.dinEndValue.DisplayFormat = "0.000";

                this.dinMsrtRange.DisplayFormat = "0.0";

                this.dinMsrtClamp.DisplayFormat = "0.0";
            }
            else
            {
                this.lblStartValueUnit.Text = "V";

                this.lblStepValueUnit.Text = "V";

                this.lblEndValueUnit.Text = "V";

                this.lblMsrtRangeUnit.Text = "mA";

                this.lblMsrtClampUnit.Text = "mA";

                this.dinStartValue.DisplayFormat = "0.00";

                this.dinEndValue.DisplayFormat = "0.00";

                this.dinMsrtRange.DisplayFormat = "0.000";

                this.dinMsrtClamp.DisplayFormat = "0.000";
            }
        }

        #endregion

        #region >>> Public Method <<<

        public void RefreshUI()
        {
            this.pnlForceMode.Visible = false;
            
            this.chkIsTestOptical.Checked = true;
            this.chkIsEnableDetector.Checked = false;

            if (this._isEnablePdDetector)
            {
                this.grpPDDetector.Visible = true;
                this.grpOpticalSetting.Dock = DockStyle.Top;
            }
            else
            {
                this.grpPDDetector.Visible = false;
                this.grpOpticalSetting.Dock = DockStyle.Fill;
            }

            if (this._isEnableSamplingTest)
            {
                this.grpSamplingSetting.Visible = true;
                this.grpMsrtSetting.Dock = DockStyle.Top;
            }
            else
            {
                this.grpSamplingSetting.Visible = false;
                this.grpMsrtSetting.Dock = DockStyle.Fill;
            }

            this.dinDetectorBiasVoltage.Value = 0;
        }

        public bool CheckUI(out string msg)
        {
            msg = string.Empty;

            return true;
        }

        public void UpdateCondtionDataToComponent(TestItemData data)
        {
            this._item = (data as LIVTestItem).Clone() as LIVTestItem;

            LIVTestItem livItem = (this._item as LIVTestItem);

            // Elec
            if ((livItem.LIVMsrtType == EMsrtType.LIV))
            {
                this.cmbForceMode.SelectedIndex = 0;
            }
            else
            {
                this.cmbForceMode.SelectedIndex = 1;
            }

            this.dinForceDelay.Value = livItem.LIVProcessDelayTime;

            this.dinForceTime.Value = livItem.LIVForceTime;

            this.dinTurnOffTime.Value = livItem.LIVTurnOffTime;

            this.dinStartValue.Value = livItem.LIVStartValue;

            this.dinEndValue.Value = livItem.LIVStopValue;

            this.dinPoints.Value = (int)livItem.LIVSweepPoints;

            this.dinMsrtRange.Value = livItem.LIVMsrtRange;

            this.dinMsrtClamp.Value = livItem.LIVMsrtProtection;

            this.numMsrtFilterCount.Value = livItem.LIVMsrtFilterCount;

			this.chkIsTestOptical.Checked = livItem.LIVIsTestOptical;

			this.dinNPLC.Value = livItem.LIVMsrtNPLC;

            this.cmbSweepMode.SelectedItem = livItem.LIVSweepMode.ToString();

            //------------------------------------------------------------------------------------
            // SPT
            this.chkIsLimitModeFixedSIT.Checked = livItem.LIVIsLimitModeFixedSIT;

            this.cmbSensingMode.SelectedItem = livItem.LIVSensingMode.ToString();

            this.dinFixIntegralTime.Value = livItem.LIVFixIntegralTime;

            this.dinLimitIntegralTime.Value = livItem.LIVLimitIntegralTime;

            //------------------------------------------------------------------------------------
            // PD Detector
            this.chkIsEnableDetector.Checked = livItem.LIVIsEnableDetector;

            this.dinDetectorBiasVoltage.Value = livItem.LIVDetectorBiasVolt;

            this.dinDetectorMsrtRange.Value = livItem.LIVDetectorMsrtRange;

            this.dinDetectorNPLC.Value = livItem.LIVDetectorNPLC;

            // Sampling Test
            this.chkLifeTest.Checked = livItem.LIVIsLifeTest;

            this.numSamplingTime.Value = livItem.LIVSamplimgTime;

            this.chkIsLogScale.Checked = livItem.LIVIsLogScale;

            this.numLogScaleTime.Value = livItem.LIVLogScaleTime;
        }

        public TestItemData GetConditionDataFromComponent()
        {
            //==============================================================
            // Sourcemeter setting, electric setting
            //==============================================================
            if (this.cmbForceMode.SelectedIndex == 0)
            {
                (this._item as LIVTestItem).LIVMsrtType = EMsrtType.LIV;

                (this._item as LIVTestItem).LIVForceUnit = EAmpUnit.mA.ToString();

                (this._item as LIVTestItem).LIVMsrtUnit = EVoltUnit.V.ToString();
            }
            else
            {
                (this._item as LIVTestItem).LIVMsrtType = EMsrtType.LVI;

                (this._item as LIVTestItem).LIVForceUnit = EVoltUnit.V.ToString();

                (this._item as LIVTestItem).LIVMsrtUnit = EAmpUnit.mA.ToString();
            }

            (this._item as LIVTestItem).LIVProcessDelayTime = this.dinForceDelay.Value;

            (this._item as LIVTestItem).LIVForceDelayTime = 0.0d;

            (this._item as LIVTestItem).LIVStartValue = this.dinStartValue.Value;

            (this._item as LIVTestItem).LIVStopValue = this.dinEndValue.Value;

            (this._item as LIVTestItem).LIVSweepPoints = (uint)this.dinPoints.Value;

            (this._item as LIVTestItem).LIVIsLimitModeFixedSIT = this.chkIsLimitModeFixedSIT.Checked;

            (this._item as LIVTestItem).LIVForceRange = this.dinEndValue.Value;

            (this._item as LIVTestItem).LIVForceTime = this.dinForceTime.Value;

            (this._item as LIVTestItem).LIVTurnOffTime = this.dinTurnOffTime.Value;

            (this._item as LIVTestItem).LIVMsrtRange = this.dinMsrtRange.Value;

            (this._item as LIVTestItem).LIVMsrtProtection = this.dinMsrtClamp.Value;

            (this._item as LIVTestItem).LIVMsrtFilterCount = this.numMsrtFilterCount.Value;

			(this._item as LIVTestItem).LIVMsrtNPLC = this.dinNPLC.Value;
            //==============================================================
            // Spectrometer setting, optical setting
            //==============================================================
            (this._item as LIVTestItem).LIVIsTestOptical = this.chkIsTestOptical.Checked;

			(this._item as LIVTestItem).LIVSensingMode = (ESensingMode)Enum.Parse(typeof(ESensingMode), this.cmbSensingMode.Text);

            (this._item as LIVTestItem).LIVFixIntegralTime = this.dinFixIntegralTime.Value;

            (this._item as LIVTestItem).LIVLimitIntegralTime = this.dinLimitIntegralTime.Value;

            (this._item as LIVTestItem).LIVTrigDelayTime = 0;

            (this._item as LIVTestItem).LIVIsEnableDetector = this.chkIsEnableDetector.Checked & this._isEnablePdDetector;

            (this._item as LIVTestItem).LIVDetectorBiasVolt = this.dinDetectorBiasVoltage.Value;

            (this._item as LIVTestItem).LIVDetectorMsrtRange = this.dinDetectorMsrtRange.Value;

            (this._item as LIVTestItem).LIVDetectorNPLC = this.dinDetectorNPLC.Value;

            //==============================================================
            // Apply Parameter, electric & optical setting
            //==============================================================
            (this._item as LIVTestItem).LIVIsLifeTest = this.chkLifeTest.Checked & this._isEnableSamplingTest;

            (this._item as LIVTestItem).LIVSamplimgTime = this.numSamplingTime.Value;

            (this._item as LIVTestItem).LIVIsLogScale = this.chkIsLogScale.Checked & this._isEnableSamplingTest;

            (this._item as LIVTestItem).LIVLogScaleTime = this.numLogScaleTime.Value;

            (this._item as LIVTestItem).LIVSweepMode = (ESweepMode)Enum.Parse(typeof(ESweepMode), this.cmbSweepMode.SelectedItem.ToString(), true);

            (this._item as LIVTestItem).LIVApplyParameter();

            return this._item;
        }

        #endregion
    }
}
