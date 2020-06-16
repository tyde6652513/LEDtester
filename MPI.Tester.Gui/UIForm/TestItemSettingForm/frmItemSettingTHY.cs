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
    public partial class frmItemSettingTHY : Form, IConditionUICtrl, IConditionElecCtrl
    {
        private object _lockObj;

        private TestItemData _item;

        private bool _isAutoSelectForceRange;
        private bool _isAutoSelectMsrtRange;
        private bool _isEnableNPLC;
        private bool _isEnableFilter;
        private bool _isEnableSwitchChannel;
        private bool _isEnableMsrtForceValue;

        public frmItemSettingTHY()
        {
            InitializeComponent();
            
            this._lockObj = new object();

            this._item = new THYTestItem();

            this._isAutoSelectForceRange = false;
            this._isAutoSelectMsrtRange = false;
            this._isEnableNPLC = false;
            this._isEnableFilter = false;
            this._isEnableSwitchChannel = false;
            this._isEnableMsrtForceValue = false;
        }

        public frmItemSettingTHY(TestItemDescription description) : this()
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
                        this.pnlSGFilter.Visible = value;
                        this.pnlMovingAvg.Visible = value;
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
                    case EItemDescription.SweepFlatCount:
                        {
                            this.dinlScanPoints.MaxValue = data.MaxValue;
                            this.dinlScanPoints.MinValue = data.MinValue;
                            this.dinlScanPoints.Value = data.DefaultValue;
                            this.dinlScanPoints.DisplayFormat = data.Format;
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
                    case EItemDescription.SGFilterCount:
                        {
                            this.pnlSGFilter.Visible = data.IsEnable & data.IsVisible;

                            this.dinSGFilterCount.MaxValue = (int)data.MaxValue;
                            this.dinSGFilterCount.MinValue = (int)data.MinValue;
                            this.dinSGFilterCount.Value = (int)data.DefaultValue;
                            break;
                        }
                    case EItemDescription.MovingAvgWindowSize:
                        {
                            this.pnlMovingAvg.Visible = data.IsEnable & data.IsVisible;

                            this.dinMovingAvgWindow.MaxValue = (int)data.MaxValue;
                            this.dinMovingAvgWindow.MinValue = (int)data.MinValue;
                            this.dinMovingAvgWindow.Value = (int)data.DefaultValue;
                            break;
                        }
                    default:
                        break;
                }
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
            this._item = (data as THYTestItem).Clone() as THYTestItem;

            this.dinForceDelay.Value = this._item.ElecSetting[0].ForceDelayTime;

            this.dinForceValue.Value = this._item.ElecSetting[0].ForceValue;

            this.dinlScanPoints.Value = this._item.ElecSetting[0].SweepContCount;

            this.dinMsrtRange.Value = this._item.ElecSetting[0].MsrtRange;

            this.dinMsrtClamp.Value = this._item.ElecSetting[0].MsrtProtection;

            this.numMsrtFilterCount.Value = this._item.ElecSetting[0].MsrtFilterCount;

            this.dinNPLC.Value = this._item.ElecSetting[0].MsrtNPLC;

            this.dinSGFilterCount.Value = this._item.ElecSetting[0].ThySGFilterCount;

            this.dinMovingAvgWindow.Value = this._item.ElecSetting[0].ThyMovingAverageWindow;

            // Special Case
            //this._item.ElecSetting[0].SweepContCount = (uint)Math.Round((double)this._item.ElecSetting[0].SweepContCount / 100.0d) * 100;
        }

        public TestItemData GetConditionDataFromComponent()
        {
            this._item.ElecSetting[0].ForceUnit = EAmpUnit.mA.ToString();

            this._item.ElecSetting[0].ForceDelayTime = this.dinForceDelay.Value;

            this._item.ElecSetting[0].ForceValue = this.dinForceValue.Value;

            this._item.ElecSetting[0].ForceTime = 0.0d;

            this._item.ElecSetting[0].ForceTimeUnit = ETimeUnit.ms.ToString();

            this._item.ElecSetting[0].IsAutoForceRange = false;

            this._item.ElecSetting[0].MsrtRange = this.dinMsrtRange.Value;

            this._item.ElecSetting[0].MsrtProtection = this.dinMsrtClamp.Value;

            this._item.ElecSetting[0].MsrtFilterCount = 0;		// Fast speed

            this._item.ElecSetting[0].MsrtUnit = EVoltUnit.V.ToString();

            this._item.ElecSetting[0].IsAutoTurnOff = true;

            this._item.MsrtResult[0].Unit = EVoltUnit.V.ToString();

            this._item.ElecSetting[0].MsrtNPLC = this.dinNPLC.Value;

            this._item.ElecSetting[0].ThySGFilterCount = (int)this.dinSGFilterCount.Value;

            this._item.ElecSetting[0].ThyMovingAverageWindow = (int)this.dinMovingAvgWindow.Value;

            //---------------------------------------------
            // for THY sweep setting 
            //---------------------------------------------
            this._item.ElecSetting[0].SweepStart = this.dinForceValue.Value;
            this._item.ElecSetting[0].SweepStop = this.dinForceValue.Value;
            this._item.ElecSetting[0].SweepStep = 0.0d;
            this._item.ElecSetting[0].SweepContCount = (uint)this.dinlScanPoints.Value;
            this._item.ElecSetting[0].SweepRiseCount = 0;

            return this._item;
        }

        #endregion
    }
}
