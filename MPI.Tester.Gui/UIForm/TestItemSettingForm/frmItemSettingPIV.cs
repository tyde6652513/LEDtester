using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;

using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;
using MPI.Tester.Maths;

namespace MPI.Tester.Gui
{
    public partial class frmItemSettingPIV : Form, IConditionUICtrl, IConditionElecCtrl
    {
        private object _lockObj;

        private EAuthority _authority;
        private EUserID _userID;

        private TestItemData _item;

        private bool _isAutoSelectForceRange;
        private bool _isAutoSelectMsrtRange;
        private bool _isEnableNPLC;
        private bool _isEnableFilter;
        private bool _isEnableSwitchChannel;
        private bool _isEnableMsrtForceValue;
        private uint _maxSwitchingChannelCnt;

        private int _sweepRisingPoints;

        private bool _isDevInPulseRegion;

        private double _dcAndPulseBoundary;
        private double _minPulseWidth;
        private double _maxPulseWidth;

        private double _maxDuty;
        private double _minPulseMsrtRange;
        private double _maxPulseMsrtRange;
        private double _defaultPulseMsrtRange;

        public frmItemSettingPIV()
        {
            InitializeComponent();
            
            this._lockObj = new object();

            this._item = new PIVTestItem();

            this.dinStartValue.ValueChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.dinEndValue.ValueChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.dinStepValue.ValueChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.dinForceTime.ValueChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.dinDutyCycle.ValueChanged += new System.EventHandler(this.UpdateDataEventHandler);

            this.rdbSeSearchByPow.CheckedChanged += new System.EventHandler(this.UpdateCalcEventHandler);
            this.rdbSeSearchByCurr.CheckedChanged += new System.EventHandler(this.UpdateCalcEventHandler);
            this.rdbRsSearchByPow.CheckedChanged += new System.EventHandler(this.UpdateCalcEventHandler);
            this.rdbRsSearchByCurr.CheckedChanged += new System.EventHandler(this.UpdateCalcEventHandler);
            //this.rdbKinkSearchByPow.CheckedChanged += new System.EventHandler(this.UpdatePivEventHandler);
            //this.rdbKinkSearchByCurr.CheckedChanged += new System.EventHandler(this.UpdatePivEventHandler); 
            this.rdbSe2SearchByPow.CheckedChanged += new System.EventHandler(this.UpdateCalcEventHandler);
            this.rdbSe2SearchByCurr.CheckedChanged += new System.EventHandler(this.UpdateCalcEventHandler);
            this.rdbThresholdSearchByPow.CheckedChanged += new System.EventHandler(this.UpdateCalcEventHandler);
            this.rdbThresholdSearchByCurr.CheckedChanged += new System.EventHandler(this.UpdateCalcEventHandler);
            this.rdbLnSearchByPow.CheckedChanged += new System.EventHandler(this.UpdateCalcEventHandler);
            this.rdbLnSearchByCurr.CheckedChanged += new System.EventHandler(this.UpdateCalcEventHandler);


            this.rdbThresholdCalcLR.CheckedChanged += new System.EventHandler(this.UpdateCalcEventHandler);
            this.rdbThresholdCalc2Point.CheckedChanged += new System.EventHandler(this.UpdateCalcEventHandler);
            this.rdbThresholdCalcThr.CheckedChanged += new System.EventHandler(this.UpdateCalcEventHandler);
            this.rdbThresholdCalcPo.CheckedChanged += new System.EventHandler(this.UpdateCalcEventHandler);

            this._isAutoSelectForceRange = false;
            this._isAutoSelectMsrtRange = false;
            this._isEnableNPLC = false;
            this._isEnableFilter = false;
            this._isEnableSwitchChannel = false;
            this._isEnableMsrtForceValue = false;

            //---------------------------------------------------------------------------
            this.cmbSourceFunc.Items.Clear();

            foreach (var e in Enum.GetValues(typeof(ESourceFunc)))
            {
                this.cmbSourceFunc.Items.Add(e);
            }

            //---------------------------------------------------------------------------

            this._isDevInPulseRegion = false;
        }

        public frmItemSettingPIV(TestItemDescription description): this()
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

        public EAuthority Authority
        {
            get { return this._authority; }
            set { lock (this._lockObj) { this._authority = value; } }
        }

        public EUserID UserID
        {
            get { return this._userID; }
            set { lock (this._lockObj) { this._userID = value; } }
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

            //------------------------------------------------------------------------------------------------------------------------
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
                    case EItemDescription.SweepStep:
                        {
                            this.dinStepValue.MaxValue = data.MaxValue;
                            this.dinStepValue.MinValue = data.MinValue;
                            this.dinStepValue.Value = data.DefaultValue;
                            this.dinStepValue.DisplayFormat = data.Format;
                            break;
                        }
                    case EItemDescription.SweepEnd:
                        {
                            if (!isEnableDevPulseRegion)
                            {
                                this.dinEndValue.MaxValue = data.MaxValue;
                            }
                           
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
                    case EItemDescription.OperationMethod:
                        {
                            if (data.DefaultValue == 0.0d)
                            {
                                this.rdbOpClosestpoint.Checked = true;
                                this.rdbOpInterpolation.Checked = false;
                                this.rdbOpOnFittingLine.Checked = false;
                            }
                            else if (data.DefaultValue == 2.0d)
                            {
                                this.rdbOpClosestpoint.Checked = false;
                                this.rdbOpInterpolation.Checked = false;
                                this.rdbOpOnFittingLine.Checked = true;
                            }
                            else
                            {
                                this.rdbOpClosestpoint.Checked = false;
                                this.rdbOpInterpolation.Checked = true;
                                this.rdbOpOnFittingLine.Checked = false;
                            }

                            break;
                        }
                    case EItemDescription.SeMethod:
                        {
                            if (data.DefaultValue == 0.0d)
                            {
                                this.rdbSeCalcLR.Checked = true;
                                this.rdbSeCalc2Point.Checked = false;
                                this.rdbSeCalcAvg.Checked = false;
                            }
                            else if (data.DefaultValue == 2.0d)
                            {
                                this.rdbSeCalcLR.Checked = false;
                                this.rdbSeCalc2Point.Checked = false;
                                this.rdbSeCalcAvg.Checked = true;
                            }
                            else
                            {
                                this.rdbSeCalcLR.Checked = false;
                                this.rdbSeCalc2Point.Checked = true;
                                this.rdbSeCalcAvg.Checked = false;
                            }

                            break;
                        }
                    case EItemDescription.ThresholdMethod:
                        {
                            if (data.DefaultValue == 0.0d)
                            {
                                this.rdbThresholdCalcLR.Checked = true;
                                this.rdbThresholdCalc2Point.Checked = false;
                                this.rdbThresholdCalcThr.Checked = false;
                                this.rdbThresholdCalcPo.Checked = false;
                            }
                            else if (data.DefaultValue == 2.0d)
                            {
                                this.rdbThresholdCalcLR.Checked = false;
                                this.rdbThresholdCalc2Point.Checked = false;
                                this.rdbThresholdCalcThr.Checked = true;
                                this.rdbThresholdCalcPo.Checked = false;
                            }
                            else if (data.DefaultValue == 4.0d)
                            {
                                this.rdbThresholdCalcLR.Checked = false;
                                this.rdbThresholdCalc2Point.Checked = false;
                                this.rdbThresholdCalcThr.Checked = false;
                                this.rdbThresholdCalcPo.Checked = true;
                            }
                            else
                            {
                                this.rdbThresholdCalcLR.Checked = false;
                                this.rdbThresholdCalc2Point.Checked = true;
                                this.rdbThresholdCalcThr.Checked = false;
                                this.rdbThresholdCalcPo.Checked = false;
                            }

                            break;
                        }
                    case EItemDescription.RsMethod:
                        {
                            if (data.DefaultValue == 0.0d)
                            {
                                this.rdbRsCalcLR.Checked = true;
                                this.rdbRsCalc2Point.Checked = false;
                                this.rdbRsCalcAvg.Checked = false;
                            }
                            else if (data.DefaultValue == 2.0d)
                            {
                                this.rdbRsCalcLR.Checked = false;
                                this.rdbRsCalc2Point.Checked = false;
                                this.rdbRsCalcAvg.Checked = true;
                            }
                            else
                            {
                                this.rdbRsCalcLR.Checked = false;
                                this.rdbRsCalc2Point.Checked = true;
                                this.rdbRsCalcAvg.Checked = false;
                            }

                            break;
                        }
                    case EItemDescription.KinkMethod:
                        {
                            if (data.DefaultValue == 0.0d)  // SEK = 0
                            {
                                this.rdbKinkCalcFittingLine.Checked = false;
                                this.rdbKinkCalcSecondOrder.Checked = false;
                                this.rdbKinkCalcSEk.Checked = true;
                                this.rdbKinkCalcDeltaPow.Checked = false;
                                this.rdbKinkCalcRefCurve.Checked = false;
                            }
                            else if (data.DefaultValue == 1.0d) //RefCurve = 1
                            {
                                this.rdbKinkCalcFittingLine.Checked = false;
                                this.rdbKinkCalcSecondOrder.Checked = false;
                                this.rdbKinkCalcSEk.Checked = false;
                                this.rdbKinkCalcDeltaPow.Checked = false;
                                this.rdbKinkCalcRefCurve.Checked = true;
                            }
                            else if (data.DefaultValue == 3.0d)  //DeltaPow = 3
                            {
                                this.rdbKinkCalcFittingLine.Checked = false;
                                this.rdbKinkCalcSecondOrder.Checked = false;
                                this.rdbKinkCalcSEk.Checked = false;
                                this.rdbKinkCalcDeltaPow.Checked = true;
                                this.rdbKinkCalcRefCurve.Checked = false;
                            }
                            else if (data.DefaultValue == 4.0d)  //Second Order = 4
                            {
                                this.rdbKinkCalcFittingLine.Checked = false;
                                this.rdbKinkCalcSecondOrder.Checked = true;
                                this.rdbKinkCalcSEk.Checked = false;
                                this.rdbKinkCalcDeltaPow.Checked = false;
                                this.rdbKinkCalcRefCurve.Checked = false;
                            }
                            else // FittingLine = 2
                            {
                                this.rdbKinkCalcFittingLine.Checked = true;
                                this.rdbKinkCalcSecondOrder.Checked = false;
                                this.rdbKinkCalcSEk.Checked = false;
                                this.rdbKinkCalcDeltaPow.Checked = false;
                                this.rdbKinkCalcRefCurve.Checked = false;
                            }

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
                                this.dinEndValue.MaxValue = data.MaxValue;
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

                    case EItemDescription.DetectorMsrtRange:
                        {
                            this.dinDetectorMsrtRange.MaxValue = data.MaxValue;
                            this.dinDetectorMsrtRange.MinValue = data.MinValue;
                            this.dinDetectorMsrtRange.Value = data.DefaultValue;
                            this.dinDetectorMsrtRange.DisplayFormat = data.Format;
                            this.lblDetectorMsrtRangeUnit.Text = data.Unit;
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        private void UpdateDataEventHandler(object sender, EventArgs e)
        {
            double startValue = this.dinStartValue.Value;
            double endValue = this.dinEndValue.Value;
            double stepValue = this.dinStepValue.Value;

            double forcaTime = this.dinForceTime.Value;
            double duty = this.dinDutyCycle.Value;

            this._sweepRisingPoints = 0;

            //-------------------------------------------------------------------------------
            this._isDevInPulseRegion = false;

            if (endValue > this._dcAndPulseBoundary) //mA
            {
                this._isDevInPulseRegion = true;
                //this.dinMsrtRange.Value = this._defaultPulseMsrtRange;
                //this.dinMsrtClamp.Value = this._defaultPulseMsrtRange;

               // this.dinTurnOffTime.Value = this.CalcTurnOffTimeByDuty(forcaTime, duty);
            }

            this.dinTurnOffTime.Value = GuiCommon.CalcOffTimeByDutyCycle(forcaTime, duty);

            //this.pnlDutyCycle.Visible = this._isPulseRegion;
            //this.dinMsrtRange.Enabled = !this._isPulseRegion;
            //this.dinMsrtClamp.Enabled = !this._isPulseRegion;
           // this.numMsrtFilterCount.Enabled = !this._isPulseRegion;
            this.dinNPLC.Enabled = !this._isDevInPulseRegion;
            this.dinDetectorNPLC.Enabled = !this._isDevInPulseRegion;
            //this.dinTurnOffTime.Enabled = !this._isPulseRegion;
            //-------------------------------------------------------------------------------

            if (stepValue != 0.0d)
            {
                if (endValue >= startValue)
                {
                    this._sweepRisingPoints = (int)((endValue - startValue) / stepValue) + 1;
                }
                else
                {
                    this._sweepRisingPoints = 0;
                }
            }

            if (this._sweepRisingPoints > 0)
            {
                this.txtDisplayPointsValue.Text = this._sweepRisingPoints.ToString("0");
            }
            else
            {
                this.txtDisplayPointsValue.Text = "Na";
            }
        }

        private void UpdateCalcEventHandler(object sender, EventArgs e)
        {
            if (this.rdbSeSearchByPow.Checked)
            {
                this.pnlSeSearchPow1.Visible = true;
                this.pnlSeSearchPow2.Visible = true;
                this.pnlSeSearchCurr1.Visible = false;
                this.pnlSeSearchCurr2.Visible = false;
            }
            else
            {
                this.pnlSeSearchPow1.Visible = false;
                this.pnlSeSearchPow2.Visible = false;
                this.pnlSeSearchCurr1.Visible = true;
                this.pnlSeSearchCurr2.Visible = true;
            }

            if (this.rdbRsSearchByPow.Checked)
            {
                this.pnlRsSearchPow1.Visible = true;
                this.pnlRsSearchPow2.Visible = true;
                this.pnlRsSearchCurr1.Visible = false;
                this.pnlRsSearchCurr2.Visible = false;
            }
            else
            {
                this.pnlRsSearchPow1.Visible = false;
                this.pnlRsSearchPow2.Visible = false;
                this.pnlRsSearchCurr1.Visible = true;
                this.pnlRsSearchCurr2.Visible = true;
            }

            if (this.rdbThresholdSearchByPow.Checked)
            {
                this.pnlThresholdSearchPow1.Visible = true;
                this.pnlThresholdSearchPow2.Visible = true;
                this.pnlThresholdSearchCurr1.Visible = false;
                this.pnlThresholdSearchCurr2.Visible = false;
            }
            else
            {
                this.pnlThresholdSearchPow1.Visible = false;
                this.pnlThresholdSearchPow2.Visible = false;
                this.pnlThresholdSearchCurr1.Visible = true;
                this.pnlThresholdSearchCurr2.Visible = true;
            }

            if (this.rdbSe2SearchByPow.Checked)
            {
                this.pnlSe2SearchPow1.Visible = true;
                this.pnlSe2SearchPow2.Visible = true;
                this.pnlSe2SearchCurr1.Visible = false;
                this.pnlSe2SearchCurr2.Visible = false;
            }
            else
            {
                this.pnlSe2SearchPow1.Visible = false;
                this.pnlSe2SearchPow2.Visible = false;
                this.pnlSe2SearchCurr1.Visible = true;
                this.pnlSe2SearchCurr2.Visible = true;
            }

            if (this.rdbLnSearchByPow.Checked)
            {
                this.pnlLnSearchPow1.Visible = true;
                this.pnlLnSearchPow2.Visible = true;
                this.pnlLnSearchCurr1.Visible = false;
                this.pnlLnSearchCurr2.Visible = false;
            }
            else
            {
                this.pnlLnSearchPow1.Visible = false;
                this.pnlLnSearchPow2.Visible = false;
                this.pnlLnSearchCurr1.Visible = true;
                this.pnlLnSearchCurr2.Visible = true;
            }

            this.pnlThresholdSearchValue.Visible = this.rdbThresholdCalcThr.Checked;
            this.pnlThresholdSearchValue2.Visible = this.rdbThresholdCalcPo.Checked;
        }

        private void cmbSourceFunc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbSourceFunc.SelectedIndex < 0)
            {
                return;
            }
            
            ESourceFunc func = (ESourceFunc)this.cmbSourceFunc.SelectedItem;

            switch (func)
            {
                case ESourceFunc.CW:
                    {
                        this.pnlDutyCycle.Visible = false;
                        this.pnlTurnOffTime.Visible = false;
                        this.pnlStepPulseCnt.Visible = false;
                        this.pnlQcwFilterCount.Visible = false;

                        this.lblNplcAuto.Visible = false;
                        this.dinNPLC.Visible = true;

                        this.lblDetectorNplcAuto1.Visible = false;
                        this.dinDetectorNPLC.Visible = true;

                        this.lblForceTime.Visible = true;
                        this.lblPulseWidth.Visible = false;

                        break;
                    }
                case ESourceFunc.PULSE:
                    {
                        this.pnlDutyCycle.Visible = true;
                        this.pnlTurnOffTime.Visible = true;
                        this.pnlStepPulseCnt.Visible = false;
                        this.pnlQcwFilterCount.Visible = false;

                        this.lblNplcAuto.Visible = true;
                        this.dinNPLC.Visible = false;

                        this.lblDetectorNplcAuto1.Visible = true;
                        this.dinDetectorNPLC.Visible = false;

                        this.lblForceTime.Visible = false;
                        this.lblPulseWidth.Visible = true;
                        break;
                    }
                case ESourceFunc.QCW:
                    {
                        this.pnlDutyCycle.Visible = true;
                        this.pnlTurnOffTime.Visible = true;
                        this.pnlStepPulseCnt.Visible = true;
                        this.pnlQcwFilterCount.Visible = true;

                        this.lblNplcAuto.Visible = true;
                        this.dinNPLC.Visible = false;

                        this.lblDetectorNplcAuto1.Visible = true;
                        this.dinDetectorNPLC.Visible = false;

                        this.lblForceTime.Visible = false;
                        this.lblPulseWidth.Visible = true;
                        break;
                    }
            }
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
            this.cmbSourceFunc.SelectedItem = ESourceFunc.CW;
            
            this.dinIPMovingAvg.Value = 3;
            this.dinIVMovingAvg.Value = 0;
            this.dinDetectorBiasVoltage.Value = 0;

            //this.rdbOpInterpolation.Checked = true;
            //this.rdbSeCalc2Point.Checked = true;
            this.rdbSeSearchByPow.Checked = true;
            //this.rdbRsCalc2Point.Checked = true;
            this.rdbRsSearchByCurr.Checked = true;
           // this.rdbKinkSearchByCurr.Checked = true;
            this.rdbThresholdSearchByPow.Checked = true;
            this.rdbSe2SearchByPow.Checked = true;
            this.rdbLnSearchByCurr.Checked = true;

            this.pnlQcwFilterCount.Visible = false;

            if (this._authority == EAuthority.Super || this._authority == EAuthority.Admin)
            {
                this.grpAdvancedSettings.Visible = true;
            }
            else
            {
                this.grpAdvancedSettings.Visible = false;
            }

            switch (this._userID)
            {
                case EUserID.Luxnet:
                    {
                        this.grpAdvancedSettings.Visible = true;
                        this.rdbSeCalcAvg.Visible = false;
                        this.rdbRsCalcAvg.Visible = false;
                        this.rdbKinkCalcSEk.Visible = false;
                        this.rdbKinkCalcDeltaPow.Visible = false;
                        this.rdbKinkCalcRefCurve.Visible = false;
                        break;
                    }
            }

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

            ESourceFunc func = (ESourceFunc)this.cmbSourceFunc.SelectedItem;

            if (this._sweepRisingPoints <= 0)
            {
                msg = "Invalid Sweep Points, Please Check Start/End Value setting";

                return false;
            }

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

                if (duty > this._maxDuty)
                {
                    msg = string.Format("The Maximum Duty is {0}%.", this._maxDuty);

                    return false;
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
                if (this.numStepPulseCnt.Value < this.numQcwFilterCount.Value)
                {
                    msg = string.Format("The index for calculation out of the step pulse count in QCW Mode.\n");

                    return false;
                }
            }

            return true;
        }

        public void UpdateCondtionDataToComponent(TestItemData data)
        {
            this._item = (data as PIVTestItem).Clone() as PIVTestItem;

            this.dinForceDelay.Value = this._item.ElecSetting[0].ForceDelayTime;

            this.dinForceTime.Value = this._item.ElecSetting[0].ForceTime;

            this.dinEndValue.Value = this._item.ElecSetting[0].SweepStop;  // 順序 EndValue -> StartValue, UpdateDataEventHandler檢查才不會出錯

            this.dinStartValue.Value = this._item.ElecSetting[0].SweepStart;

            this.dinStepValue.Value = this._item.ElecSetting[0].SweepStep;

            //this.dinTurnOffTime.Value = this._item.ElecSetting[0].SweepTurnOffTime;

            this.cmbSourceFunc.SelectedItem = (ESourceFunc)this._item.ElecSetting[0].SourceFunction;

            this.dinDutyCycle.Value = this._item.ElecSetting[0].Duty * 100.0d;

            this.dinMsrtRange.Value = this._item.ElecSetting[0].MsrtRange;

            this.dinMsrtClamp.Value = this._item.ElecSetting[0].MsrtProtection;

            this.numMsrtFilterCount.Value = this._item.ElecSetting[0].MsrtFilterCount;


            double factor = UnitMath.ToSIUnit(lblDetectorMsrtRangeUnit.Text);

            this.dinNPLC.Value = this._item.ElecSetting[0].MsrtNPLC;

            this.dinDetectorNPLC.Value = this._item.ElecSetting[0].DetectorMsrtNPLC;

            this.dinDetectorBiasVoltage.Value = this._item.ElecSetting[0].DetectorBiasValue;

            double val = this._item.ElecSetting[0].DetectorMsrtRange / factor;

            val  = Math.Round(val, 12, MidpointRounding.AwayFromZero);

            this.dinDetectorMsrtRange.Value = val;

            //if (this._item.ElecSetting[0].IsPulseMode)
            //{
            //    this.dinDutyCycle.Value = this.CalcDutyCycleByOffTime(this._item.ElecSetting[0].ForceTime, this._item.ElecSetting[0].SweepTurnOffTime);
            //}

            this.numStepPulseCnt.Value = (int)this._item.ElecSetting[0].PulseCount;

            this.numQcwFilterCount.Value = (int)this._item.ElecSetting[0].CalcMsrtFromPulseIndex;

            //-----------------------------------------------------------------------------------------------------------
            // for Laser Characteristics Calculation setting 
            //-----------------------------------------------------------------------------------------------------------
            this.dinPop.Value = (this._item as PIVTestItem).CalcSetting.Pop;

            this.dinIPMovingAvg.Value = (this._item as PIVTestItem).CalcSetting.PowMovingAverageWindow;

            this.dinIVMovingAvg.Value = (this._item as PIVTestItem).CalcSetting.VoltMovingAverageWindow;

            //-----------------------------------------------------------------------------------------------------------
            // Iop, Vop
            if ((this._item as PIVTestItem).CalcSetting.OperationPointSelection == ELaserPointSelectMode.ClosestPoint)
            {
                this.rdbOpClosestpoint.Checked = true;
                this.rdbOpInterpolation.Checked = false;
                this.rdbOpOnFittingLine.Checked = false;
            }
            else if ((this._item as PIVTestItem).CalcSetting.OperationPointSelection == ELaserPointSelectMode.OnFittingLine)
            {
                this.rdbOpClosestpoint.Checked = false;
                this.rdbOpInterpolation.Checked = false;
                this.rdbOpOnFittingLine.Checked = true;
            }
            else
            {
                this.rdbOpClosestpoint.Checked = false;
                this.rdbOpInterpolation.Checked = true;
                this.rdbOpOnFittingLine.Checked = false;
            }

            //-----------------------------------------------------------------------------------------------------------
            // SE
            if ((this._item as PIVTestItem).CalcSetting.SeCalcMode == ELaserCalcMode.LinearRegression)
            {
                this.rdbSeCalcLR.Checked = true;
                this.rdbSeCalc2Point.Checked = false;
                this.rdbSeCalcAvg.Checked = false;
            }
            else if ((this._item as PIVTestItem).CalcSetting.SeCalcMode == ELaserCalcMode.Average)
            {
                this.rdbSeCalcLR.Checked = false;
                this.rdbSeCalc2Point.Checked = false;
                this.rdbSeCalcAvg.Checked = true;
            }
            else
            {
                this.rdbSeCalcLR.Checked = false;
                this.rdbSeCalc2Point.Checked = true;
                this.rdbSeCalcAvg.Checked = false;
            }

            if ((this._item as PIVTestItem).CalcSetting.SeSearchMode == ELaserSearchMode.byPower)
            {
                this.rdbSeSearchByPow.Checked = true;
                this.rdbSeSearchByCurr.Checked = false;
            }
            else
            {
                this.rdbSeSearchByPow.Checked = false;
                this.rdbSeSearchByCurr.Checked = true;
            }

            if ((this._item as PIVTestItem).CalcSetting.Se2SearchMode == ELaserSearchMode.byPower)
            {
                this.rdbSe2SearchByPow.Checked = true;
                this.rdbSe2SearchByCurr.Checked = false;
            }
            else
            {
                this.rdbSe2SearchByPow.Checked = false;
                this.rdbSe2SearchByCurr.Checked = true;
            }

            this.dinSeSearchCurr1.Value = (this._item as PIVTestItem).CalcSetting.SeSectionLowLimitI * 1000.0d;  // mA
            this.dinSeSearchCurr2.Value = (this._item as PIVTestItem).CalcSetting.SeSectionUpperLimitI * 1000.0d;
            this.dinSeSearchPow1.Value = (this._item as PIVTestItem).CalcSetting.SeSectionLowLimitP;
            this.dinSeSearchPow2.Value = (this._item as PIVTestItem).CalcSetting.SeSectionUpperLimitP;

            this.dinSe2SearchCurr1.Value = (this._item as PIVTestItem).CalcSetting.Se2SectionLowLimitI * 1000.0d;  // mA
            this.dinSe2SearchCurr2.Value = (this._item as PIVTestItem).CalcSetting.Se2SectionUpperLimitI * 1000.0d;
            this.dinSe2SearchPow1.Value = (this._item as PIVTestItem).CalcSetting.Se2SectionLowLimitP;
            this.dinSe2SearchPow2.Value = (this._item as PIVTestItem).CalcSetting.Se2SectionUpperLimitP;

            //-----------------------------------------------------------------------------------------------------------
            // Threshold
            if ((this._item as PIVTestItem).CalcSetting.ThresholdCalcMode == ELaserCalcMode.LinearRegression)
            {
                this.rdbThresholdCalcLR.Checked = true;
                this.rdbThresholdCalc2Point.Checked = false;
                this.rdbThresholdCalcThr.Checked = false;
                this.rdbThresholdCalcPo.Checked = false;
            }
            else if ((this._item as PIVTestItem).CalcSetting.ThresholdCalcMode == ELaserCalcMode.ThresholdValue)  // >= dP/dI
            {
                this.rdbThresholdCalcLR.Checked = false;
                this.rdbThresholdCalc2Point.Checked = false;
                this.rdbThresholdCalcThr.Checked = true;
                this.rdbThresholdCalcPo.Checked = false;
            }
            else if ((this._item as PIVTestItem).CalcSetting.ThresholdCalcMode == ELaserCalcMode.ThresholdValue2) // >= Po
            {
                this.rdbThresholdCalcLR.Checked = false;
                this.rdbThresholdCalc2Point.Checked = false;
                this.rdbThresholdCalcThr.Checked = false;
                this.rdbThresholdCalcPo.Checked = true;
            }
            else
            {
                this.rdbThresholdCalcLR.Checked = false;
                this.rdbThresholdCalc2Point.Checked = true;
                this.rdbThresholdCalcThr.Checked = false;
                this.rdbThresholdCalcPo.Checked = false;
            }

            if ((this._item as PIVTestItem).CalcSetting.ThresholdSearchMode == ELaserSearchMode.byPower)
            {
                this.rdbThresholdSearchByPow.Checked = true;
                this.rdbThresholdSearchByCurr.Checked = false;
            }
            else
            {
                this.rdbThresholdSearchByPow.Checked = false;
                this.rdbThresholdSearchByCurr.Checked = true;
            }

            this.dinThresholdSearchCurr1.Value = (this._item as PIVTestItem).CalcSetting.ThresholdSectionLowLimitI * 1000.0d;  // mA
            this.dinThresholdSearchCurr2.Value = (this._item as PIVTestItem).CalcSetting.ThresholdSectionUpperLimitI * 1000.0d;
            this.dinThresholdSearchPow1.Value = (this._item as PIVTestItem).CalcSetting.ThresholdSectionLowLimitP;
            this.dinThresholdSearchPow2.Value = (this._item as PIVTestItem).CalcSetting.ThresholdSectionUpperLimitP;
            this.dinThresholdSearchValue.Value = (this._item as PIVTestItem).CalcSetting.ThresholdSearchValue; // mW/mA | W/A
            this.dinThresholdSearchValue2.Value = (this._item as PIVTestItem).CalcSetting.ThresholdSearchValue2; // mW/mA | W/A

            //-----------------------------------------------------------------------------------------------------------
            // RS
            if ((this._item as PIVTestItem).CalcSetting.RsCalcMode == ELaserCalcMode.LinearRegression)
            {
                this.rdbRsCalcLR.Checked = true;
                this.rdbRsCalc2Point.Checked = false;
                this.rdbRsCalcAvg.Checked = false;
            }
            else if ((this._item as PIVTestItem).CalcSetting.RsCalcMode == ELaserCalcMode.Average)
            {
                this.rdbRsCalcLR.Checked = false;
                this.rdbRsCalc2Point.Checked = false;
                this.rdbRsCalcAvg.Checked = true;
            }
            else
            {
                this.rdbRsCalcLR.Checked = false;
                this.rdbRsCalc2Point.Checked = true;
                this.rdbRsCalcAvg.Checked = false;
            }

            if ((this._item as PIVTestItem).CalcSetting.RsSearchMode == ELaserSearchMode.byPower)
            {
                this.rdbRsSearchByPow.Checked = true;
                this.rdbRsSearchByCurr.Checked = false;
            }
            else
            {
                this.rdbRsSearchByPow.Checked = false;
                this.rdbRsSearchByCurr.Checked = true;
            }

            this.dinRsSearchCurr1.Value = (this._item as PIVTestItem).CalcSetting.RsSectionLowLimitI * 1000.0d; // mA
            this.dinRsSearchCurr2.Value = (this._item as PIVTestItem).CalcSetting.RsSectionUpperLimitI * 1000.0d;
            this.dinRsSearchPow1.Value = (this._item as PIVTestItem).CalcSetting.RsSectionLowLimitP;
            this.dinRsSearchPow2.Value = (this._item as PIVTestItem).CalcSetting.RsSectionUpperLimitP;

            //-----------------------------------------------------------------------------------------------------------
            // Kink
            if ((this._item as PIVTestItem).CalcSetting.KinkCalcMode == ELaserKinkCalcMode.SEk)  // SEK = 0
            {
                this.rdbKinkCalcFittingLine.Checked = false;
                this.rdbKinkCalcSecondOrder.Checked = false;
                this.rdbKinkCalcSEk.Checked = true;
                this.rdbKinkCalcDeltaPow.Checked = false;
                this.rdbKinkCalcRefCurve.Checked = false;
            }
            else if ((this._item as PIVTestItem).CalcSetting.KinkCalcMode == ELaserKinkCalcMode.RefCurve)  //RefCurve = 1
            {
                this.rdbKinkCalcFittingLine.Checked = false;
                this.rdbKinkCalcSecondOrder.Checked = false;
                this.rdbKinkCalcSEk.Checked = false;
                this.rdbKinkCalcDeltaPow.Checked = false;
                this.rdbKinkCalcRefCurve.Checked = true;
            }
            else if ((this._item as PIVTestItem).CalcSetting.KinkCalcMode == ELaserKinkCalcMode.DeltaPow)   //DeltaPow = 3
            {
                this.rdbKinkCalcFittingLine.Checked = false;
                this.rdbKinkCalcSecondOrder.Checked = false;
                this.rdbKinkCalcSEk.Checked = false;
                this.rdbKinkCalcDeltaPow.Checked = true;
                this.rdbKinkCalcRefCurve.Checked = false;
            }
            else if ((this._item as PIVTestItem).CalcSetting.KinkCalcMode == ELaserKinkCalcMode.SecondOrder)   //Second Order = 4
            {
                this.rdbKinkCalcFittingLine.Checked = false;
                this.rdbKinkCalcSecondOrder.Checked = true;
                this.rdbKinkCalcSEk.Checked = false;
                this.rdbKinkCalcDeltaPow.Checked = false;
                this.rdbKinkCalcRefCurve.Checked = false;
            }
            else // FittingLine = 2
            {
                this.rdbKinkCalcFittingLine.Checked = true;
                this.rdbKinkCalcSecondOrder.Checked = false;
                this.rdbKinkCalcSEk.Checked = false;
                this.rdbKinkCalcDeltaPow.Checked = false;
                this.rdbKinkCalcRefCurve.Checked = false;
            }
 
            this.dinKinkSearchCurr1.Value = (this._item as PIVTestItem).CalcSetting.KinkSectionLowLimitI * 1000.0d;
            this.dinKinkSearchCurr2.Value = (this._item as PIVTestItem).CalcSetting.KinkSectionUpperLimitI * 1000.0d;
            this.dinKinkRatio.Value = (this._item as PIVTestItem).CalcSetting.KinkRatio * 100.0d;

            //-----------------------------------------------------------------------------------------------------------
            // Linearity
            if ((this._item as PIVTestItem).CalcSetting.LnSearchMode == ELaserSearchMode.byPower)
            {
                this.rdbLnSearchByPow.Checked = true;
                this.rdbLnSearchByCurr.Checked = false;
            }
            else
            {
                this.rdbLnSearchByPow.Checked = false;
                this.rdbLnSearchByCurr.Checked = true;
            }

            this.dinLnSearchCurr1.Value = (this._item as PIVTestItem).CalcSetting.LnSectionLowLimitI * 1000.0d;  // mA
            this.dinLnSearchCurr2.Value = (this._item as PIVTestItem).CalcSetting.LnSectionUpperLimitI * 1000.0d;
            this.dinLnSearchPow1.Value = (this._item as PIVTestItem).CalcSetting.LnSectionLowLimitP;
            this.dinLnSearchPow2.Value = (this._item as PIVTestItem).CalcSetting.LnSectionUpperLimitP;

            //-----------------------------------------------------------------------------------------------------------
            // Rollover
            this.dinRollOver.Value = (this._item as PIVTestItem).CalcSetting.Iroll * 1000.0d;

            //-----------------------------------------------------------------------------------------------------------
            // Specific Points
            this.dinSpecificCurr1.Value = (this._item as PIVTestItem).CalcSetting.IfA * 1000.0d; // To mA
            this.dinSpecificCurr2.Value = (this._item as PIVTestItem).CalcSetting.IfB * 1000.0d; // To mA
            this.dinSpecificCurr3.Value = (this._item as PIVTestItem).CalcSetting.IfC * 1000.0d; // To mA

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
            this._item.ElecSetting[0].ForceUnit = EAmpUnit.mA.ToString();

            this._item.ElecSetting[0].ForceDelayTime = this.dinForceDelay.Value;

            this._item.ElecSetting[0].ForceTime = this.dinForceTime.Value;

            this._item.ElecSetting[0].ForceTimeUnit = ETimeUnit.ms.ToString();

            this._item.ElecSetting[0].IsAutoForceRange = false;

            this._item.ElecSetting[0].MsrtRange = this.dinMsrtRange.Value;

            this._item.ElecSetting[0].MsrtProtection = this.dinMsrtClamp.Value;

            this._item.ElecSetting[0].MsrtFilterCount = this.numMsrtFilterCount.Value;

            this._item.ElecSetting[0].MsrtNPLC = this.dinNPLC.Value;

            this._item.ElecSetting[0].MsrtUnit = EVoltUnit.V.ToString();

            //this._item.MsrtResult[0].Unit = EVoltUnit.V.ToString();

            double factor = UnitMath.ToSIUnit(lblDetectorMsrtRangeUnit.Text);

            this._item.ElecSetting[0].IsTrigDetector = true;

            this._item.ElecSetting[0].DetectorMsrtNPLC = this.dinDetectorNPLC.Value;

            this._item.ElecSetting[0].DetectorBiasValue = this.dinDetectorBiasVoltage.Value;

            double val = this.dinDetectorMsrtRange.Value * factor;

            val = Math.Round(val, 12, MidpointRounding.AwayFromZero);

            this._item.ElecSetting[0].DetectorMsrtRange = val;

            //-----------------------------------------------------------------------------------------------------------
            // for FIMV Sweep setting 
            //-----------------------------------------------------------------------------------------------------------
            this._item.ElecSetting[0].SourceFunction = (ESourceFunc)this.cmbSourceFunc.SelectedItem;

            this._item.ElecSetting[0].SweepStart = this.dinStartValue.Value;

            this._item.ElecSetting[0].SweepStop = this.dinEndValue.Value;

            this._item.ElecSetting[0].SweepStep = this.dinStepValue.Value;

            this._item.ElecSetting[0].SweepContCount = 0;

            this._item.ElecSetting[0].SweepRiseCount = (uint)this._sweepRisingPoints;

            this._item.ElecSetting[0].Duty = this.dinDutyCycle.Value / 100.0d;

            if (this._item.ElecSetting[0].SourceFunction == ESourceFunc.CW)
            {
                this._item.ElecSetting[0].SweepTurnOffTime = 0.0d;
            }
            else
            {
                this._item.ElecSetting[0].SweepTurnOffTime = this.dinTurnOffTime.Value;
            }

            this._item.ElecSetting[0].IsPulseMode = this._isDevInPulseRegion;

            this._item.ElecSetting[0].PulseCount = (uint)this.numStepPulseCnt.Value;

            this._item.ElecSetting[0].CalcMsrtFromPulseIndex = (uint)this.numQcwFilterCount.Value;

            //-----------------------------------------------------------------------------------------------------------
            // for Laser Characteristics Calculation setting 
            //-----------------------------------------------------------------------------------------------------------
            (this._item as PIVTestItem).CalcSetting.Pop = this.dinPop.Value;

            (this._item as PIVTestItem).CalcSetting.PowMovingAverageWindow = (int)this.dinIPMovingAvg.Value;

            (this._item as PIVTestItem).CalcSetting.VoltMovingAverageWindow = (int)this.dinIVMovingAvg.Value;

            //-----------------------------------------------------------------------------------------------------------
            // Iop, Vop
            if (this.rdbOpClosestpoint.Checked)
            {
                (this._item as PIVTestItem).CalcSetting.OperationPointSelection = ELaserPointSelectMode.ClosestPoint;
            }
            else if (this.rdbOpOnFittingLine.Checked)
            {
                (this._item as PIVTestItem).CalcSetting.OperationPointSelection = ELaserPointSelectMode.OnFittingLine;
            }
            else
            {
                (this._item as PIVTestItem).CalcSetting.OperationPointSelection = ELaserPointSelectMode.Interpolation;
            }

            //-----------------------------------------------------------------------------------------------------------
            // SE
            if (this.rdbSeCalcLR.Checked)
            {
                (this._item as PIVTestItem).CalcSetting.SeCalcMode = ELaserCalcMode.LinearRegression;
            }
            else if (this.rdbSeCalcAvg.Checked)
            {
                (this._item as PIVTestItem).CalcSetting.SeCalcMode = ELaserCalcMode.Average;
            }
            else
            {
                (this._item as PIVTestItem).CalcSetting.SeCalcMode = ELaserCalcMode.TwoPointsDifference;
            }

            if (this.rdbSeSearchByPow.Checked)
            {
                (this._item as PIVTestItem).CalcSetting.SeSearchMode = ELaserSearchMode.byPower;
            }
            else
            {
                (this._item as PIVTestItem).CalcSetting.SeSearchMode = ELaserSearchMode.byCurrent;
            }

            if (this.rdbSe2SearchByPow.Checked)
            {
                (this._item as PIVTestItem).CalcSetting.Se2SearchMode = ELaserSearchMode.byPower;
            }
            else
            {
                (this._item as PIVTestItem).CalcSetting.Se2SearchMode = ELaserSearchMode.byCurrent;
            }

            (this._item as PIVTestItem).CalcSetting.SeSectionLowLimitI = this.dinSeSearchCurr1.Value / 1000.0d;
            (this._item as PIVTestItem).CalcSetting.SeSectionUpperLimitI = this.dinSeSearchCurr2.Value / 1000.0d;
            (this._item as PIVTestItem).CalcSetting.SeSectionLowLimitP = this.dinSeSearchPow1.Value;
            (this._item as PIVTestItem).CalcSetting.SeSectionUpperLimitP = this.dinSeSearchPow2.Value;

            (this._item as PIVTestItem).CalcSetting.Se2SectionLowLimitI = this.dinSe2SearchCurr1.Value / 1000.0d;
            (this._item as PIVTestItem).CalcSetting.Se2SectionUpperLimitI = this.dinSe2SearchCurr2.Value / 1000.0d;
            (this._item as PIVTestItem).CalcSetting.Se2SectionLowLimitP = this.dinSe2SearchPow1.Value;
            (this._item as PIVTestItem).CalcSetting.Se2SectionUpperLimitP = this.dinSe2SearchPow2.Value;

            //-----------------------------------------------------------------------------------------------------------
            // Threshold
            if (this.rdbThresholdCalcLR.Checked)
            {
                (this._item as PIVTestItem).CalcSetting.ThresholdCalcMode = ELaserCalcMode.LinearRegression;
            }
            else if (this.rdbThresholdCalcThr.Checked)
            {
                (this._item as PIVTestItem).CalcSetting.ThresholdCalcMode = ELaserCalcMode.ThresholdValue;
            }
            else if (this.rdbThresholdCalcPo.Checked)
            {
                (this._item as PIVTestItem).CalcSetting.ThresholdCalcMode = ELaserCalcMode.ThresholdValue2;
            }
            else
            {
                (this._item as PIVTestItem).CalcSetting.ThresholdCalcMode = ELaserCalcMode.TwoPointsDifference;
            }

            if (this.rdbThresholdSearchByPow.Checked)
            {
                (this._item as PIVTestItem).CalcSetting.ThresholdSearchMode = ELaserSearchMode.byPower;
            }
            else
            {
                (this._item as PIVTestItem).CalcSetting.ThresholdSearchMode = ELaserSearchMode.byCurrent;
            }

            (this._item as PIVTestItem).CalcSetting.ThresholdSectionLowLimitI = this.dinThresholdSearchCurr1.Value / 1000.0d;
            (this._item as PIVTestItem).CalcSetting.ThresholdSectionUpperLimitI = this.dinThresholdSearchCurr2.Value / 1000.0d;
            (this._item as PIVTestItem).CalcSetting.ThresholdSectionLowLimitP = this.dinThresholdSearchPow1.Value;
            (this._item as PIVTestItem).CalcSetting.ThresholdSectionUpperLimitP = this.dinThresholdSearchPow2.Value;

            (this._item as PIVTestItem).CalcSetting.ThresholdSearchValue = this.dinThresholdSearchValue.Value;
            (this._item as PIVTestItem).CalcSetting.ThresholdSearchValue2 = this.dinThresholdSearchValue2.Value;

            //-----------------------------------------------------------------------------------------------------------
            // RS
            if (this.rdbRsCalcLR.Checked)
            {
                (this._item as PIVTestItem).CalcSetting.RsCalcMode = ELaserCalcMode.LinearRegression;
            }
            else if (this.rdbRsCalcAvg.Checked)
            {
                (this._item as PIVTestItem).CalcSetting.RsCalcMode = ELaserCalcMode.Average;
            }
            else
            {
                (this._item as PIVTestItem).CalcSetting.RsCalcMode = ELaserCalcMode.TwoPointsDifference;
            }

            if (this.rdbRsSearchByPow.Checked)
            {
                (this._item as PIVTestItem).CalcSetting.RsSearchMode = ELaserSearchMode.byPower;
            }
            else
            {
                (this._item as PIVTestItem).CalcSetting.RsSearchMode = ELaserSearchMode.byCurrent;
            }

            (this._item as PIVTestItem).CalcSetting.RsSectionLowLimitI = this.dinRsSearchCurr1.Value / 1000.0d;
            (this._item as PIVTestItem).CalcSetting.RsSectionUpperLimitI = this.dinRsSearchCurr2.Value / 1000.0d;
            (this._item as PIVTestItem).CalcSetting.RsSectionLowLimitP = this.dinRsSearchPow1.Value;
            (this._item as PIVTestItem).CalcSetting.RsSectionUpperLimitP = this.dinRsSearchPow2.Value;

            //-----------------------------------------------------------------------------------------------------------
            // Kink
            if (this.rdbKinkCalcSEk.Checked)
            {
                (this._item as PIVTestItem).CalcSetting.KinkCalcMode = ELaserKinkCalcMode.SEk;
            }
            else if (this.rdbKinkCalcRefCurve.Checked)
            {
                (this._item as PIVTestItem).CalcSetting.KinkCalcMode = ELaserKinkCalcMode.RefCurve;
            }
            else if (this.rdbKinkCalcDeltaPow.Checked)
            {
                (this._item as PIVTestItem).CalcSetting.KinkCalcMode = ELaserKinkCalcMode.DeltaPow;
            }
            else if (this.rdbKinkCalcSecondOrder.Checked)
            {
                (this._item as PIVTestItem).CalcSetting.KinkCalcMode = ELaserKinkCalcMode.SecondOrder;
            }
            else
            {
                (this._item as PIVTestItem).CalcSetting.KinkCalcMode = ELaserKinkCalcMode.FittingLine;
            }

            (this._item as PIVTestItem).CalcSetting.KinkSectionLowLimitI = this.dinKinkSearchCurr1.Value / 1000.0d;
            (this._item as PIVTestItem).CalcSetting.KinkSectionUpperLimitI = this.dinKinkSearchCurr2.Value / 1000.0d;
            (this._item as PIVTestItem).CalcSetting.KinkRatio = this.dinKinkRatio.Value / 100.0d;

            //-----------------------------------------------------------------------------------------------------------
            // Linearity, Ln

            if (this.rdbLnSearchByPow.Checked)
            {
                (this._item as PIVTestItem).CalcSetting.LnSearchMode = ELaserSearchMode.byPower;
            }
            else
            {
                (this._item as PIVTestItem).CalcSetting.LnSearchMode = ELaserSearchMode.byCurrent;
            }

            (this._item as PIVTestItem).CalcSetting.LnSectionLowLimitI = this.dinLnSearchCurr1.Value / 1000.0d;
            (this._item as PIVTestItem).CalcSetting.LnSectionUpperLimitI = this.dinLnSearchCurr2.Value / 1000.0d;
            (this._item as PIVTestItem).CalcSetting.LnSectionLowLimitP = this.dinLnSearchPow1.Value;
            (this._item as PIVTestItem).CalcSetting.LnSectionUpperLimitP = this.dinLnSearchPow2.Value;

            //-----------------------------------------------------------------------------------------------------------
            // Rollover
            (this._item as PIVTestItem).CalcSetting.Iroll = this.dinRollOver.Value / 1000.0d;

            //-----------------------------------------------------------------------------------------------------------
            // Specific Points
            (this._item as PIVTestItem).CalcSetting.IfA = this.dinSpecificCurr1.Value / 1000.0d; // To mA
            (this._item as PIVTestItem).CalcSetting.IfB = this.dinSpecificCurr2.Value / 1000.0d; // To mA
            (this._item as PIVTestItem).CalcSetting.IfC = this.dinSpecificCurr3.Value / 1000.0d; // To mA
           // (this._item as PIVTestItem).CalcSetting.Ipce = this.dinIpce.Value / 1000.0d;  // To A


            if (this._isEnableSwitchChannel)
            {
                this._item.SwitchingChannel = (uint)this.cmbSelectedChannel.SelectedIndex;
            }

            return this._item;
        }

        #endregion
    }
}
