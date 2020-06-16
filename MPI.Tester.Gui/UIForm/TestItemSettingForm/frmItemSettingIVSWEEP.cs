using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;

using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;
using DevComponents.DotNetBar.Controls;

namespace MPI.Tester.Gui
{
    public partial class frmItemSettingIVSWEEP : Form, IConditionUICtrl, IConditionElecCtrl
    {
        private object _lockObj;

        private IVSweepTestItem _item;

        private bool _isAutoSelectForceRange;
        private bool _isAutoSelectMsrtRange;
        private bool _isEnableNPLC;
        private bool _isEnableFilter;
        private bool _isEnableSwitchChannel;
        private bool _isEnableMsrtForceValue;
        private uint _maxSwitchingChannelCnt;


        private string fUnit = "mA";
        private string mUnit = "V";
        private string tUnit = "ms";

        public frmItemSettingIVSWEEP()
        {
            InitializeComponent();
            
            this._lockObj = new object();

            this._item = new IVSweepTestItem();

            this.cmbSweepMode.Items.Clear();
            this.cmbSweepMode.Items.AddRange(Enum.GetNames(typeof(ESweepMode)));
            this.cmbSweepMode.Items.Remove(ESweepMode.Custom.ToString());
            this.cmbSweepMode.SelectedIndex = 0;

            this.cmbSweepMode.SelectedIndexChanged += new System.EventHandler(this.UpdateDataEventHandler); 
            this.dinStartValue.ValueChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.dinEndValue.ValueChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.dinPoints.ValueChanged += new System.EventHandler(this.UpdateDataEventHandler);

            this._isAutoSelectForceRange = false;
            this._isAutoSelectMsrtRange = false;
            this._isEnableNPLC = false;
            this._isEnableFilter = false;
            this._isEnableSwitchChannel = false;
            this._isEnableMsrtForceValue = false;
            this._maxSwitchingChannelCnt = 0;

            superTabControl1.SelectedTabIndex = 0;
        }

        public frmItemSettingIVSWEEP(TestItemDescription description) : this()
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
            get { return this._maxSwitchingChannelCnt; }
            set { lock (this._lockObj) { this._maxSwitchingChannelCnt = value; } }
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

                            SetColData(cListComp.ColStartValue, data);
                            fUnit = data.Unit;

                            break;
                        }
                    case EItemDescription.SweepEnd:
                        {
                            this.dinEndValue.MaxValue = data.MaxValue;
                            this.dinEndValue.MinValue = data.MinValue;
                            this.dinEndValue.Value = data.DefaultValue;
                            this.dinEndValue.DisplayFormat = data.Format;
                            SetColData(cListComp.ColEndValue, data);
                            fUnit = data.Unit;
                            break;
                        }
                    case EItemDescription.ForceTime:
                        {
                            this.dinForceTime.MaxValue = data.MaxValue;
                            this.dinForceTime.MinValue = data.MinValue;
                            this.dinForceTime.Value = data.DefaultValue;
                            this.dinForceTime.DisplayFormat = data.Format;
                            SetColData(cListComp.ColForceTime, data);
                            tUnit = data.Unit;
                            break;
                        }
                    case EItemDescription.SweepRiseCount:
                        {
                            this.dinPoints.MaxValue = (int)data.MaxValue;
                            this.dinPoints.MinValue = (int)data.MinValue;
                            this.dinPoints.Value = (int)data.DefaultValue;
                            
                            cListComp.ColCnt.Visible = data.IsVisible;
                            cListComp.ColCnt.MaxValue = data.MaxValue;
                            cListComp.ColCnt.MinValue = data.MinValue;
                            cListComp.ColCnt.DisplayFormat = "0";
                            break;
                        }
                    case EItemDescription.SweepTurnOffTime:
                        {
                            this.dinTurnOffTime.MaxValue = data.MaxValue;
                            this.dinTurnOffTime.MinValue = data.MinValue;
                            this.dinTurnOffTime.Value = data.DefaultValue;
                            this.dinTurnOffTime.DisplayFormat = data.Format;
                            SetColData(cListComp.ColOffTime, data);
                            tUnit = data.Unit;
                            break;
                        }
                    case EItemDescription.MsrtRange:
                        {
                            this.pnlMsrtRange.Visible = data.IsEnable & data.IsVisible;

                            this.dinMsrtRange.MaxValue = data.MaxValue;
                            this.dinMsrtRange.MinValue = data.MinValue;
                            this.dinMsrtRange.Value = data.DefaultValue;
                            this.dinMsrtRange.DisplayFormat = data.Format;
                            mUnit = data.Unit;
                            break;
                        }
                    case EItemDescription.MsrtClamp:
                        {
                            this.pnlMsrtClamp.Visible = data.IsEnable & data.IsVisible;

                            this.dinMsrtClamp.MaxValue = data.MaxValue;
                            this.dinMsrtClamp.MinValue = data.MinValue;
                            this.dinMsrtClamp.Value = data.DefaultValue;
                            this.dinMsrtClamp.DisplayFormat = data.Format;
                            SetColData(cListComp.ColClamp, data);
                            mUnit = data.Unit;
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
                            SetColData(cListComp.ColNPLC, data);
                            break;
                        }
                    case EItemDescription.SweepAdvanceMode:
                        {
                            superTabItem2.Visible = data.DefaultValue >= 1;
                            break;
                        }
                    default:
                        break;
                }
            }
			cListComp.SetUnit(fUnit, mUnit, tUnit);
        }

        private void SetColData(DataGridViewDoubleInputColumn colData, ItemDescriptionBase data)
        {
            colData.Enabled = data.IsEnable;
            colData.Visible = data.IsVisible;
            colData.MaxValue = data.MaxValue;
            colData.MinValue = data.MinValue;
            colData.DisplayFormat = data.Format;

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

        private List<double> CreateLog10List(double start, double stop, int points)
        {
            List<double> valList = new List<double>();

            if (start == 0.0d)
            {
                start = 1e-12;
            }

            if (stop == 0.0d)
            {
                stop = 1e-12;
            }

            int polar = 1;

            if (start < 0 || stop < 0)
            {
                polar = -1;
            }

            start = Math.Abs(start);

            stop = Math.Abs(stop);

            double logStepSize = (Math.Log10(stop) - Math.Log10(start)) / (points - 1);

            for (int i = 0; i < points; ++i)
            {
                double val = Math.Pow(10, i) * start;
                valList.Add(val);
            }
            return valList;
        }

        private List<double> CreateLinearList(double start, double stop, int points)
        {
            List<double> valList = new List<double>();

            double step = (stop - start) / (points - 1);

            for (int i = 0; i < points; ++i)
            {
                double val = start + step * i;
                valList.Add(val);
            }

            return valList;
        }

        #endregion

        #region >>> Public Method <<<

        public void RefreshUI()
        {
            if (this._isEnableSwitchChannel)
            {
                this.UpedateAvailableChannelsToUI(this._maxSwitchingChannelCnt);
            }
            else
            {
                this.grpChannel.Visible = false;
                this.grpApplySetting.Dock = DockStyle.Fill;
            }
        }

        public bool CheckUI(out string msg)
        {
            msg = string.Empty;

            if (this._isEnableSwitchChannel)
            {
                if (this.cmbSelectedChannel.SelectedIndex < 0)
                {
                    msg = "Must Select the Switching Channel";
                    return false;
                }
            }

            return true;
        }

        public void UpdateCondtionDataToComponent(TestItemData data)
        {
            this._item = (data as IVSweepTestItem).Clone() as IVSweepTestItem;

            this.cmbSweepMode.SelectedItem = this._item.ElecSetting[0].SweepMode.ToString();

            this.dinForceDelay.Value = this._item.ElecSetting[0].ForceDelayTime;

            this.dinForceTime.Value = this._item.ElecSetting[0].ForceTime;

            this.dinStartValue.Value = this._item.ElecSetting[0].SweepStart;

            this.dinEndValue.Value = this._item.ElecSetting[0].SweepStop;

            this.dinPoints.Value = (int)this._item.ElecSetting[0].SweepRiseCount;

            this.dinTurnOffTime.Value = this._item.ElecSetting[0].SweepTurnOffTime;

            this.dinMsrtRange.Value = this._item.ElecSetting[0].MsrtRange;

            this.dinMsrtClamp.Value = this._item.ElecSetting[0].MsrtProtection;

            this.numMsrtFilterCount.Value = this._item.ElecSetting[0].MsrtFilterCount;

            this.dinNPLC.Value = this._item.ElecSetting[0].MsrtNPLC;

            this.chkIsAutoMsrtRange.Checked = this._item.ElecSetting[0].IsSweepAutoMsrtRange;

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

			if (this._item.IsCustomerizeSweepMode)
            {
                cListComp.SetSweepData(this._item.SweepInfoList);
                superTabControl1.SelectedTabIndex = 1;
            }
            else
            {
                superTabControl1.SelectedTabIndex = 0;
            }

        }

        public TestItemData GetConditionDataFromComponent()
        {
            SweepInfo swi = new SweepInfo();

            swi.StartValue = this.dinStartValue.Value;
            swi.EndValue = this.dinEndValue.Value;
            swi.Cnt = (int)this.dinPoints.Value;
            swi.NPLC = this.dinNPLC.Value;
            swi.Clamp = this.dinMsrtClamp.Value;
            swi.ForceTime = this.dinForceTime.Value;
            swi.OffTime = this.dinTurnOffTime.Value;
            swi.AutoMsrtRange = this.chkIsAutoMsrtRange.Checked;
            swi.Mode = (ESweepMode)Enum.Parse(typeof(ESweepMode), this.cmbSweepMode.SelectedItem.ToString(), true);
            swi.ForceUnit = fUnit;
            swi.MsrtUnit = mUnit;
            swi.TimeUnit = tUnit;


            this._item.IsCustomerizeSweepMode = (superTabControl1.SelectedTabIndex == 1);

            List<double> valList = new List<double>();

            if (this._item.IsCustomerizeSweepMode)
            {
                _item.SweepInfoList = cListComp.GetSweepData();
            }
            else
            {
                _item.SweepInfoList = new List<SweepInfo>();
                _item.SweepInfoList.Add(swi);
            }

            if (_item.SweepInfoList != null && _item.SweepInfoList.Count > 0)
            {
                _item.SweepInfoList[0].ForceDelayTime = dinForceDelay.Value;
            }

            if (this._isEnableSwitchChannel)
            {
                this._item.SwitchingChannel = (uint)this.cmbSelectedChannel.SelectedIndex;
            }
            _item.RefreshElecSetting();

            return this._item;


        }

        #endregion
    }
}
