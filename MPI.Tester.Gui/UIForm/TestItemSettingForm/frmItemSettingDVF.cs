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
    public partial class frmItemSettingDVF : Form, IConditionUICtrl, IConditionElecCtrl
    {
        private object _lockObj;

        private TestItemData _item;

        private bool _isAutoSelectForceRange;
        private bool _isAutoSelectMsrtRange;
        private bool _isEnableNPLC;
        private bool _isEnableFilter;
        private bool _isEnableSwitchChannel;

        public frmItemSettingDVF()
        {
            InitializeComponent();
            
            this._lockObj = new object();

            this._item = new DVFTestItem();

            this._isAutoSelectForceRange = false;
            this._isAutoSelectMsrtRange = false;
            this._isEnableNPLC = false;
            this._isEnableFilter = false;
            this._isEnableSwitchChannel = false;
        }

        public frmItemSettingDVF(TestItemDescription description) : this()
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
            get;
            set;
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

                            this.dinForceValue2.MaxValue = data.MaxValue;
                            this.dinForceValue2.MinValue = data.MinValue;
                            this.dinForceValue2.Value = data.DefaultValue;
                            this.dinForceValue2.DisplayFormat = data.Format;

                            this.dinForceValue3.MaxValue = data.MaxValue;
                            this.dinForceValue3.MinValue = data.MinValue;
                            this.dinForceValue3.Value = data.DefaultValue;
                            this.dinForceValue3.DisplayFormat = data.Format;
                            break;
                        }
                    case EItemDescription.ForceTime:
                        {
                            this.dinForceTime.MaxValue = data.MaxValue;
                            this.dinForceTime.MinValue = data.MinValue;
                            this.dinForceTime.Value = data.DefaultValue;
                            this.dinForceTime.DisplayFormat = data.Format;

                            this.dinForceTime2.MaxValue = data.MaxValue;
                            this.dinForceTime2.MinValue = data.MinValue;
                            this.dinForceTime2.Value = data.DefaultValue;
                            this.dinForceTime2.DisplayFormat = data.Format;

                            this.dinForceTime3.MaxValue = data.MaxValue;
                            this.dinForceTime3.MinValue = data.MinValue;
                            this.dinForceTime3.Value = data.DefaultValue;
                            this.dinForceTime3.DisplayFormat = data.Format;
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
            this._item = (data as DVFTestItem).Clone() as DVFTestItem;

            this.dinForceDelay.Value = this._item.ElecSetting[0].ForceDelayTime;

            this.dinForceValue.Value = this._item.ElecSetting[0].ForceValue;

            this.dinForceValue2.Value = this._item.ElecSetting[1].ForceValue;

            this.dinForceValue3.Value = this._item.ElecSetting[2].ForceValue;

            this.dinForceTime.Value = this._item.ElecSetting[0].ForceTime;

            this.dinForceTime2.Value = this._item.ElecSetting[1].ForceTime;

            this.dinForceTime3.Value = this._item.ElecSetting[2].ForceTime;

            this.dinMsrtRange.Value = this._item.ElecSetting[0].MsrtRange;

            this.dinMsrtClamp.Value = this._item.ElecSetting[0].MsrtProtection;

            this.numMsrtFilterCount.Value = this._item.ElecSetting[0].MsrtFilterCount;

            this.dinNPLC.Value = this._item.ElecSetting[0].MsrtNPLC;
        }

        public TestItemData GetConditionDataFromComponent()
        {
            // #0
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

            this._item.ElecSetting[0].MsrtNPLC = this.dinNPLC.Value;

            this._item.MsrtResult[0].Unit = EVoltUnit.V.ToString();

            // #1
            this._item.ElecSetting[1].ForceUnit = EAmpUnit.mA.ToString();

            this._item.ElecSetting[1].ForceDelayTime = 0.0d;

            this._item.ElecSetting[1].ForceValue = (double)this.dinForceValue2.Value;

            this._item.ElecSetting[1].ForceTime = (double)this.dinForceTime2.Value;

            //this._item.ElecSetting[1].ForceRangeIndex = this.cmbForceRange.SelectedIndex;

            this._item.ElecSetting[1].ForceTimeUnit = ETimeUnit.ms.ToString();

            this._item.ElecSetting[1].IsAutoForceRange = this._isAutoSelectForceRange;

            this._item.ElecSetting[1].MsrtRange = this.dinMsrtRange.Value;

            this._item.ElecSetting[1].MsrtProtection = this.dinMsrtClamp.Value;

            this._item.ElecSetting[1].MsrtFilterCount = this.numMsrtFilterCount.Value;

            this._item.ElecSetting[1].MsrtUnit = EVoltUnit.V.ToString();

            this._item.ElecSetting[1].MsrtNPLC = this.dinNPLC.Value;

            this._item.MsrtResult[1].Unit = EVoltUnit.V.ToString();

            // #2
            this._item.ElecSetting[2].ForceUnit = EAmpUnit.mA.ToString();

            this._item.ElecSetting[2].ForceDelayTime = 0.0d;

            this._item.ElecSetting[2].ForceValue = (double)this.dinForceValue3.Value;

            this._item.ElecSetting[2].ForceTime = (double)this.dinForceTime3.Value;

            //this._item.ElecSetting[2].ForceRangeIndex = this.cmbForceRange.SelectedIndex;

            this._item.ElecSetting[2].ForceTimeUnit = ETimeUnit.ms.ToString();

            this._item.ElecSetting[2].IsAutoForceRange = this._isAutoSelectForceRange;

            this._item.ElecSetting[2].MsrtRange = this.dinMsrtRange.Value;

            this._item.ElecSetting[2].MsrtProtection = this.dinMsrtClamp.Value;

            this._item.ElecSetting[2].MsrtFilterCount = this.numMsrtFilterCount.Value;

            this._item.ElecSetting[2].MsrtUnit = EVoltUnit.V.ToString();

            this._item.ElecSetting[2].MsrtNPLC = this.dinNPLC.Value;

            this._item.MsrtResult[2].Unit = EVoltUnit.V.ToString();

            return this._item;
        }

        #endregion
    }
}
