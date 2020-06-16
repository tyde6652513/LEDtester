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
    public partial class frmItemSettingESD : Form, IConditionUICtrl, IConditionElecCtrl
    {
        private object _lockObj;

        private TestItemData _item;

        private bool _isAutoSelectForceRange;
        private bool _isAutoSelectMsrtRange;
        private bool _isEnableNPLC;
        private bool _isEnableFilter;
        private bool _isEnableSwitchChannel;
        private bool _isEnableMsrtForceValue;

        public frmItemSettingESD()
        {
            InitializeComponent();
            
            this._lockObj = new object();

            this._item = new ESDTestItem();

            this.cmbMode.Items.Clear();
            this.cmbMode.Items.Add(EESDMode.HBM);
            this.cmbMode.Items.Add(EESDMode.MM);
            this.cmbMode.SelectedItem = EESDMode.HBM;

            this.cmbPolarity.Items.Clear();
            this.cmbPolarity.Items.Add(EESDPolarity.P);
            this.cmbPolarity.Items.Add(EESDPolarity.N);
            this.cmbPolarity.SelectedItem = EESDPolarity.N;

            this.numInterval.Value = 0;

            this._isAutoSelectForceRange = false;
            this._isAutoSelectMsrtRange = false;
            this._isEnableNPLC = false;
            this._isEnableFilter = false;
            this._isEnableSwitchChannel = false;
            this._isEnableMsrtForceValue = false;
        }

        public frmItemSettingESD(TestItemDescription description) : this()
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

               
            }
        }

        #endregion

        #region >>> Public Method <<<

        public void RefreshUI()
        {
            this.chkIsJudge.Checked = false;

            this.dinForceDelay.Value = 0.0d;

            this.pnlMsrtRange.Visible = false;

            this.pnlNPLC.Visible = false;

            this.pnlFilterCount.Visible = false;
        }

        public bool CheckUI(out string msg)
        {
            msg = string.Empty;

            return true;
        }

        public void UpdateCondtionDataToComponent(TestItemData data)
        {
            this._item = (data as ESDTestItem).Clone() as ESDTestItem;

            this.cmbMode.SelectedItem = (this._item as ESDTestItem).EsdSetting.Mode;

            this.cmbPolarity.SelectedItem = (this._item as ESDTestItem).EsdSetting.Polarity;

            this.numSingleVolt.Value = (this._item as ESDTestItem).EsdSetting.ZapVoltage;

            this.numInterval.Value = (this._item as ESDTestItem).EsdSetting.IntervalTime;

            this.numCount.Value = (this._item as ESDTestItem).EsdSetting.Count;

            // ESD Judge Item
            this.chkIsJudge.Checked = (this._item as ESDTestItem).IsEnableJudgeItem;

            this.dinForceDelay.Value = this._item.ElecSetting[0].ForceDelayTime;

            this.dinForceValue.Value = Math.Abs(this._item.ElecSetting[0].ForceValue);

            this.dinForceTime.Value = this._item.ElecSetting[0].ForceTime;

            this.dinMsrtRange.Value = this._item.ElecSetting[0].MsrtRange;

            this.dinMsrtClamp.Value = this._item.ElecSetting[0].MsrtProtection;

            this.numMsrtFilterCount.Value = this._item.ElecSetting[0].MsrtFilterCount;

            this.dinNPLC.Value = this._item.ElecSetting[0].MsrtNPLC;
        }

        public TestItemData GetConditionDataFromComponent()
        {
            (this._item as ESDTestItem).EsdSetting.Mode = (EESDMode)this.cmbMode.SelectedItem;

            (this._item as ESDTestItem).EsdSetting.Polarity = (EESDPolarity)this.cmbPolarity.SelectedItem;

            (this._item as ESDTestItem).EsdSetting.ZapVoltage = this.numSingleVolt.Value;

            (this._item as ESDTestItem).EsdSetting.IntervalTime = this.numInterval.Value;

            (this._item as ESDTestItem).EsdSetting.Count = this.numCount.Value;

            // ESD Judge Item
            // apply
            (this._item as ESDTestItem).IsEnableJudgeItem = this.chkIsJudge.Checked;

            this._item.ElecSetting[0].ForceTimeUnit = ETimeUnit.ms.ToString();

            this._item.ElecSetting[0].IsAutoForceRange = this._isAutoSelectForceRange;

            this._item.ElecSetting[0].ForceUnit = EVoltUnit.V.ToString();

            this._item.ElecSetting[0].ForceDelayTime = (double)this.dinForceDelay.Value;

            this._item.ElecSetting[0].ForceValue = (double)this.dinForceValue.Value;

            this._item.ElecSetting[0].ForceTime = (double)this.dinForceTime.Value;

            this._item.ElecSetting[0].ForceTimeUnit = ETimeUnit.ms.ToString();

            // msrt
            this._item.ElecSetting[0].MsrtRange = this.dinMsrtRange.Value;

            this._item.ElecSetting[0].MsrtProtection = this.dinMsrtClamp.Value;

            this._item.ElecSetting[0].MsrtFilterCount = this.numMsrtFilterCount.Value;

            this._item.ElecSetting[0].MsrtUnit = EAmpUnit.uA.ToString();

            this._item.MsrtResult[0].Unit = EAmpUnit.uA.ToString();

            this._item.ElecSetting[0].MsrtNPLC = this.dinNPLC.Value;

            return this._item;
        }

        #endregion

        private void chkIsJudge_CheckedChanged(object sender, EventArgs e)
        {
            this.pnlJudgeItem.Enabled = this.chkIsJudge.Checked;
        }
    }
}
