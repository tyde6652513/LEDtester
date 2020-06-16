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
    public partial class frmItemSettingIF : Form, IConditionUICtrl, IConditionElecCtrl
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

        public frmItemSettingIF()
        {
            InitializeComponent();
            
            this._lockObj = new object();

            this._item = new IFTestItem();

            this._isAutoSelectForceRange = false;
            this._isAutoSelectMsrtRange = false;
            this._isEnableNPLC = false;
            this._isEnableFilter = false;
            this._isEnableSwitchChannel = false;
            this._isEnableMsrtForceValue = false;
            this._maxSwitchingChannelCnt = 0;
        }

        public frmItemSettingIF(TestItemDescription description) : this()
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

            this.pnlMsrtForceValue.Visible = this._isEnableMsrtForceValue;
        }

        public bool CheckUI(out string msg)
        {
            msg = string.Empty;

            if (this._isEnableSwitchChannel)
            {
                if(this.cmbSelectedChannel.SelectedIndex < 0)
                {
                    msg = "Must Select the Switching Channel";
                    return false;
                }
            }

            return true;
        }

        public void UpdateCondtionDataToComponent(TestItemData data)
        {
            this._item = (data as IFTestItem).Clone() as IFTestItem;

            this.dinForceDelay.Value = this._item.ElecSetting[0].ForceDelayTime;

            this.dinForceValue.Value = this._item.ElecSetting[0].ForceValue;

            this.dinForceTime.Value = this._item.ElecSetting[0].ForceTime;

            this.dinMsrtRange.Value = this._item.ElecSetting[0].MsrtRange;

            this.dinMsrtClamp.Value = this._item.ElecSetting[0].MsrtProtection;

            this.numMsrtFilterCount.Value = this._item.ElecSetting[0].MsrtFilterCount;

            this.dinNPLC.Value = this._item.ElecSetting[0].MsrtNPLC;

            this.chkIsEnableMsrtForceValue.Checked = this._item.ElecSetting[0].IsEnableMsrtForceValue & this._isEnableMsrtForceValue;

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

            this._item.ElecSetting[0].ForceDelayTime = (double)this.dinForceDelay.Value;

            this._item.ElecSetting[0].ForceValue = (double)this.dinForceValue.Value;

            this._item.ElecSetting[0].ForceTime = (double)this.dinForceTime.Value;

            //this._item.ElecSetting[0].ForceRangeIndex = this.cmbForceRange.SelectedIndex;

            this._item.ElecSetting[0].ForceTimeUnit = ETimeUnit.ms.ToString();

            this._item.ElecSetting[0].IsAutoForceRange = this._isAutoSelectForceRange;

            if (dinMsrtRange.Visible == false)
            {
                dinMsrtRange.Value = dinMsrtClamp.Value;
            }

            this._item.ElecSetting[0].MsrtRange = this.dinMsrtRange.Value;

            this._item.ElecSetting[0].MsrtProtection = this.dinMsrtClamp.Value;

            this._item.ElecSetting[0].MsrtFilterCount = this.numMsrtFilterCount.Value;

            this._item.ElecSetting[0].MsrtUnit = EVoltUnit.V.ToString();

            this._item.ElecSetting[0].MsrtNPLC = this.dinNPLC.Value;

            //this._item.MsrtResult[0].Unit = EVoltUnit.V.ToString();

            this._item.ElecSetting[0].IsEnableMsrtForceValue = this.chkIsEnableMsrtForceValue.Checked & this._isEnableMsrtForceValue;

            if (this._isEnableSwitchChannel)
            {
                this._item.SwitchingChannel = (uint)this.cmbSelectedChannel.SelectedIndex;
            }
           
            return this._item;
        }

        #endregion
    }
}

