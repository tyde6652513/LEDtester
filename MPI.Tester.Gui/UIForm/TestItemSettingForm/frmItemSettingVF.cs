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
    public partial class frmItemSettingVF : Form, IConditionUICtrl, IConditionElecCtrl
    {
        private object _lockObj;

        private TestItemData _item;

        private bool _isAutoSelectForceRange;
        private bool _isAutoSelectMsrtRange;
        private bool _isEnableNPLC;
        private bool _isEnableFilter;
        private bool _isEnableSwitchChannel;
      
        private uint _maxSwitchingChannelCnt;

        private bool _isVisibleMsrtForceValue;
        private bool _isVisibleRetest;

        public frmItemSettingVF()
        {
            InitializeComponent();

            this._lockObj = new object();

            this._item = new VFTestItem();

            this._isAutoSelectForceRange = false;
            this._isAutoSelectMsrtRange = false;
            this._isEnableNPLC = false;
            this._isEnableFilter = false;
            this._isEnableSwitchChannel = false;
            this._isVisibleMsrtForceValue = false;
            this._maxSwitchingChannelCnt = 0;

            this._isVisibleRetest = false;
        }

        public frmItemSettingVF(TestItemDescription description) : this()
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
            get { return this._isVisibleMsrtForceValue; }
            set { lock (this._lockObj) { this._isVisibleMsrtForceValue = value; } }
        }

        public uint MaxSwitchingChannelCount
        {
            get { return this._maxSwitchingChannelCnt; }
            set { lock (this._lockObj) { this._maxSwitchingChannelCnt = value; } }
        }

        public bool IsEnableRetest
        {
            get { return this._isVisibleRetest; }
            set { lock (this._lockObj) { this._isVisibleRetest = value; } }
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
                            this.dinForceDealy.MaxValue = data.MaxValue;
                            this.dinForceDealy.MinValue = data.MinValue;
                            this.dinForceDealy.Value = data.DefaultValue;
                            this.dinForceDealy.DisplayFormat = data.Format;
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
                    default:
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

        #region >>> UICtrl Method <<<

        private void chkIsEnableMsrtForceValue_CheckedChanged(object sender, EventArgs e)
        {
            this.pnlRetest.Enabled = this.chkIsEnableMsrtForceValue.Checked;
        }


        private void chkEnableRetest_CheckedChanged(object sender, EventArgs e)
        {
            this.numRetestCount.Enabled = this.chkEnableRetest.Checked;
            this.dinRetestThresholdV.Enabled = this.chkEnableRetest.Checked;
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

            this.pnlRetest.Enabled = false;
            this.numRetestCount.Enabled = false;
            this.dinRetestThresholdV.Enabled = false;

            this.pnlRetest.Visible = this._isVisibleMsrtForceValue & this._isVisibleRetest;

            this.pnlMsrtForceValue.Visible = this._isVisibleMsrtForceValue;
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
            this._item = (data as VFTestItem).Clone() as VFTestItem;

            this.dinForceDealy.Value = this._item.ElecSetting[0].ForceDelayTime;

            this.dinForceValue.Value = this._item.ElecSetting[0].ForceValue;

            this.dinForceTime.Value = this._item.ElecSetting[0].ForceTime;

            this.dinMsrtRange.Value = this._item.ElecSetting[0].MsrtRange;

            this.dinMsrtClamp.Value = this._item.ElecSetting[0].MsrtProtection;

            this.numMsrtFilterCount.Value = this._item.ElecSetting[0].MsrtFilterCount;

            this.dinNPLC.Value = this._item.ElecSetting[0].MsrtNPLC;

            this.chkIsEnableMsrtForceValue.Checked = this._item.ElecSetting[0].IsEnableMsrtForceValue & this._isVisibleMsrtForceValue;

            //-------------------------------------------------------------------------------------------------------------------------
            // Retest
            this.chkEnableRetest.Checked = (this._item as VFTestItem).IsEnableRetest & this._isVisibleRetest & this.chkIsEnableMsrtForceValue.Checked;

            this.numRetestCount.Value = (int)(this._item as VFTestItem).RetestCount;

            this.dinRetestThresholdV.Value = (this._item as VFTestItem).RetestThresholdV;

            //-------------------------------------------------------------------------------------------------------------------------
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
            this._item.ElecSetting[0].ForceUnit = EVoltUnit.V.ToString();

            this._item.ElecSetting[0].ForceDelayTime = (double)this.dinForceDealy.Value;

            this._item.ElecSetting[0].ForceValue = (double)this.dinForceValue.Value;

            this._item.ElecSetting[0].ForceTime = (double)this.dinForceTime.Value;

            this._item.ElecSetting[0].ForceTimeUnit = ETimeUnit.ms.ToString();

            this._item.ElecSetting[0].IsAutoForceRange = this._isAutoSelectForceRange;

            //this._item.ElecSetting[0].MsrtRange = this.dinMsrtRange.Value;

            this._item.ElecSetting[0].MsrtProtection = this.dinMsrtClamp.Value;

            if (dinMsrtRange.Visible == false)
            {
                dinMsrtRange.Value = dinMsrtClamp.Value;
            }

            this._item.ElecSetting[0].MsrtRange = this.dinMsrtRange.Value;

            this._item.ElecSetting[0].MsrtFilterCount = this.numMsrtFilterCount.Value;

            this._item.ElecSetting[0].MsrtUnit = EAmpUnit.mA.ToString();

            this._item.ElecSetting[0].MsrtNPLC = this.dinNPLC.Value;

            //this._item.MsrtResult[0].Unit = EAmpUnit.mA.ToString();

            if (this._isEnableSwitchChannel)
            {
                this._item.SwitchingChannel = (uint)this.cmbSelectedChannel.SelectedIndex;
            }

            this._item.ElecSetting[0].IsEnableMsrtForceValue = this.chkIsEnableMsrtForceValue.Checked & this._isVisibleMsrtForceValue;

            //-------------------------------------------------------------------------------------------------------------------------
            // Retest
            (this._item as VFTestItem).IsEnableRetest = this.chkEnableRetest.Checked & this._isVisibleRetest & this._item.ElecSetting[0].IsEnableMsrtForceValue;

            (this._item as VFTestItem).RetestCount = (uint)this.numRetestCount.Value;

            (this._item as VFTestItem).RetestThresholdV = this.dinRetestThresholdV.Value;

            return this._item;
        }

        #endregion


       
    }
}
