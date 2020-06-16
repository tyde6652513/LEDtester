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
    public partial class frmItemSettingOSA : Form, IConditionUICtrl, IConditionElecCtrl
    {
        private object _lockObj;

        private TestItemData _item;
     
        private bool _isAutoSelectForceRange;
        private bool _isAutoSelectMsrtRange;
        private bool _isEnableNPLC;
        private bool _isEnableFilter;
        private bool _isEnableSwitchChannel;
        private bool _isEnableMsrtForceValue;

        public frmItemSettingOSA()
        {
            InitializeComponent();
            
            this._lockObj = new object();

            this._item = new OSATestItem();

            this._isAutoSelectForceRange = false;
            this._isAutoSelectMsrtRange = false;
            this._isEnableNPLC = false;
            this._isEnableFilter = false;
            this._isEnableSwitchChannel = false;
            this._isEnableMsrtForceValue = false;

            //--------------------------------------------------------------------
            // Add EMS9740AnalysisMode
            this.cmbAnalysisMode.Items.Clear();

            foreach (var e in Enum.GetValues(typeof(EMS9740AnalysisMode)))
            {
                this.cmbAnalysisMode.Items.Add(e);
            }

            this.cmbAnalysisMode.SelectedItem = EMS9740AnalysisMode.DFB_LD;

            //--------------------------------------------------------------------
            // Add EMS9740AppDfbSideMode
            this.cmbDfbSideMode.Items.Clear();

            foreach (var e in Enum.GetValues(typeof(EMS9740DfbSideMode)))
            {
                this.cmbDfbSideMode.Items.Add(e);
            }

            this.cmbDfbSideMode.SelectedItem = EMS9740DfbSideMode.Second_Peak;
            
            //--------------------------------------------------------------------
        }

        public frmItemSettingOSA(TestItemDescription description) : this()
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
            this._item = (data as OSATestItem).Clone() as OSATestItem;

            this.dinForceDelay.Value = this._item.ElecSetting[0].ForceDelayTime;

            this.dinForceValue.Value = this._item.ElecSetting[0].ForceValue;

            this.dinForceTime.Value = this._item.ElecSetting[0].ForceTime;

            this.dinMsrtRange.Value = this._item.ElecSetting[0].MsrtRange;

            this.dinMsrtClamp.Value = this._item.ElecSetting[0].MsrtProtection;

            this.numMsrtFilterCount.Value = this._item.ElecSetting[0].MsrtFilterCount;

            //========================================
            // OSA setting, optical setting
            //========================================
            // Gerneral Setting
            this.cmbAnalysisMode.SelectedItem = (this._item as OSATestItem).OsaSettingData.MS9740AnalysisMode;

            this.dinCenterWL.Value = (this._item as OSATestItem).OsaSettingData.CenterWavelength;

            this.dinSpanWL.Value = (this._item as OSATestItem).OsaSettingData.SpanOfWavelength;

            this.dinRefLevel.Value = (this._item as OSATestItem).OsaSettingData.ReferenceLevel;

            this.dinLogDiv.Value = (this._item as OSATestItem).OsaSettingData.LogDiv;

            this.dinResolution.Value = (this._item as OSATestItem).OsaSettingData.Resoluation;

            this.dinVBW.Value = (this._item as OSATestItem).OsaSettingData.VideoBW;


            // Application Setting
            // AP-FP
            this.dinFpSliceLevel.Value = (this._item as OSATestItem).OsaSettingData.FpSliceLevel;

            // AP-DFB
            this.cmbDfbSideMode.SelectedItem = (this._item as OSATestItem).OsaSettingData.MS9740DfbSideMode;

            this.dinDfbStdevFactor.Value = (this._item as OSATestItem).OsaSettingData.DfbStdevFactor;

            this.dinDfbSerachResolution.Value = (this._item as OSATestItem).OsaSettingData.DfbSearchResolution;

            this.dinDfbSliceLevel.Value = (this._item as OSATestItem).OsaSettingData.DfbSliceLevel;
        }

        public TestItemData GetConditionDataFromComponent()
        {
            (this._item as OSATestItem).CreateCoefTable(DataCenter._sysSetting.CoefTableStartWL,
                                                    DataCenter._sysSetting.CoefTableEndWL,
                                                    DataCenter._sysSetting.CoefTableResolution);

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

            //========================================
            // OSA setting, optical setting
            //========================================
            // Gerneral Setting
            (this._item as OSATestItem).OsaSettingData.MS9740AnalysisMode = (EMS9740AnalysisMode)this.cmbAnalysisMode.SelectedItem;

            (this._item as OSATestItem).OsaSettingData.CenterWavelength = this.dinCenterWL.Value;

            (this._item as OSATestItem).OsaSettingData.SpanOfWavelength = this.dinSpanWL.Value;

            (this._item as OSATestItem).OsaSettingData.ReferenceLevel = this.dinRefLevel.Value;

            (this._item as OSATestItem).OsaSettingData.LogDiv = this.dinLogDiv.Value;

            (this._item as OSATestItem).OsaSettingData.Resoluation = this.dinResolution.Value;

            (this._item as OSATestItem).OsaSettingData.VideoBW = this.dinVBW.Value;

            (this._item as OSATestItem).OsaSettingData.SamplingPoints = 501;

            // Application Setting
            // AP-FP
            (this._item as OSATestItem).OsaSettingData.FpSliceLevel = this.dinFpSliceLevel.Value;

            // AP-DFB
            (this._item as OSATestItem).OsaSettingData.MS9740DfbSideMode = (EMS9740DfbSideMode)this.cmbDfbSideMode.SelectedItem;

            (this._item as OSATestItem).OsaSettingData.DfbStdevFactor = this.dinDfbStdevFactor.Value;

            (this._item as OSATestItem).OsaSettingData.DfbSearchResolution = this.dinDfbSerachResolution.Value;

            (this._item as OSATestItem).OsaSettingData.DfbSliceLevel = this.dinDfbSliceLevel.Value;

            return this._item;
        }

        #endregion

        #region >>> UI Ctrl <<<

        private void cmbAnalysisMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            EMS9740AnalysisMode mode = (EMS9740AnalysisMode)this.cmbAnalysisMode.SelectedItem;
            
            switch (mode)
            {
                case EMS9740AnalysisMode.DFB_LD:
                    {
                        this.grpFpSetting.Enabled = false;
                        this.grpDfbSetting.Enabled = true;
                        break;
                    }
                case EMS9740AnalysisMode.FP_LD:
                    {
                        this.grpFpSetting.Enabled = true;
                        this.grpDfbSetting.Enabled = false;
                        break;
                    }
                default:
                    {
                        this.grpFpSetting.Enabled = false;
                        this.grpDfbSetting.Enabled = false;
                        break;
                    }
            }
        }

        #endregion
    }
}
