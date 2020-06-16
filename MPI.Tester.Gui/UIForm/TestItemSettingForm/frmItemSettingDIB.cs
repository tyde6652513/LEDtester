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
    public partial class frmItemSettingDIB : Form, IConditionUICtrl
    {
        private object _lockObj;

        private TestItemData _item;

        public frmItemSettingDIB()
        {
            InitializeComponent();
            
            this._lockObj = new object();

            this._item = new DIBTestItem();
        }

        #region >>> Public Property <<<

        #endregion

        #region >>> Private Method <<<

        private void UpdateTargetItmes()
        {
            this.cmbTargetItemA.Items.Clear();
            
            // update item to UI
            if (DataCenter._product.TestCondition.TestItemArray != null)
            {
                foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
                {
                    if (item.Type == ETestType.IF)
                    {
                        foreach (TestResultData result in item.MsrtResult)
                        {
                            if (result.IsEnable == true)
                            {
                                this.cmbTargetItemA.Items.Add(result.Name);
                            }
                        }
                    }
                }
            }

            // item A
            if ((this._item as DIBTestItem).ItemKeyNameA != null)
            {
                if (DataCenter._product.TestCondition.TestItemArray != null)
                {
                    foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
                    {
                        if (item.MsrtResult == null)
                            continue;

                        foreach (TestResultData result in item.MsrtResult)
                        {
                            if (result.KeyName == (this._item as DIBTestItem).ItemKeyNameA)
                            {
                                if (this.cmbTargetItemA.Items.Contains(result.Name))
                                {
                                    this.cmbTargetItemA.SelectedItem = result.Name;
                                }
                                else
                                {
                                    this.cmbTargetItemA.SelectedItem = null;
                                }

                                break;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region >>> Public Method <<<

        public void RefreshUI()
        {
            this.UpdateTargetItmes();
        }

        public bool CheckUI(out string msg)
        {
            msg = string.Empty;

            return true;
        }

        public void UpdateCondtionDataToComponent(TestItemData data)
        {
            this._item = (data as DIBTestItem).Clone() as DIBTestItem;

            //this.dinForceDealyDIB.Value = this._item.ElecSetting[0].ForceDelayTime;
            //this.dinForceValueDIB.Value = this._item.ElecSetting[0].ForceValue;
            //this.dinForceTimeDIB.Value = this._item.ElecSetting[0].ForceTime;

            //this.dinMsrtRangeDIB.Value = this._item.ElecSetting[0].MsrtRange;
            //this.dinMsrtClampDIB.Value = this._item.ElecSetting[0].MsrtProtection;
            //this.numMsrtFilterCountDIB.Value = this._item.ElecSetting[0].MsrtFilterCount;

            this.dinFilterBase.Value = (this._item as DIBTestItem).FilterBase;
            this.dinFilterSpec.Value = (this._item as DIBTestItem).FilterSpec;
            this.dinScanCount.Value = (this._item as DIBTestItem).FlatCount;


            this.chkIsOnlyJuadgeSerious.Checked = (this._item as DIBTestItem).IsOnlyJuadeSerious;
        }

        public TestItemData GetConditionDataFromComponent()
        {
            // Special Case
            //if (DataCenter._machineConfig.SourceMeterModel == ESourceMeterModel.LDT1A)
            //{
            //    if (this._item.ElecSetting[0].MsrtFilterCount < 20 && this._item.ElecSetting[0].ForceValue < 0.002)
            //    {
            //        this._item.ElecSetting[0].MsrtFilterCount = 20;
            //    }
            //    this._item.MsrtResult[0].RawValueArray = new double[this._item.ElecSetting[0].MsrtFilterCount];
            //}

            if (this.cmbTargetItemA.SelectedItem != null)
            {
                (this._item as DIBTestItem).ItemNameA = this.cmbTargetItemA.SelectedItem.ToString();

                foreach (TestResultData result in DataCenter._acquireData.OutputTestResult)
                {
                    if (result.Name == (this._item as DIBTestItem).ItemNameA)
                    {
                        (this._item as DIBTestItem).ItemKeyNameA = result.KeyName;
                    }
                }
            }

            (this._item as DIBTestItem).FilterBase = this.dinFilterBase.Value;

            (this._item as DIBTestItem).FilterSpec = this.dinFilterSpec.Value;

            (this._item as DIBTestItem).FlatCount = (int)this.dinScanCount.Value;

            (this._item as DIBTestItem).IsOnlyJuadeSerious = this.chkIsOnlyJuadgeSerious.Checked;

            string targetKeyName = string.Empty;

            foreach (TestItemData itemData in DataCenter._product.TestCondition.TestItemArray)
            {
                if (itemData.MsrtResult == null)
                    continue;

                if (itemData.MsrtResult[0].KeyName == (this._item as DIBTestItem).ItemKeyNameA)
                {
                    this._item.ElecSetting[0].PUMIndex = itemData.ElecSetting[0].PUMIndex;
                    this._item.ElecSetting[0].ForceUnit = EAmpUnit.mA.ToString();
                    this._item.ElecSetting[0].ForceDelayTime = itemData.ElecSetting[0].ForceDelayTime;
                    this._item.ElecSetting[0].ForceValue = itemData.ElecSetting[0].ForceValue;
                    this._item.ElecSetting[0].ForceTime = itemData.ElecSetting[0].ForceTime;

                    this._item.ElecSetting[0].ForceTimeUnit = ETimeUnit.ms.ToString();
                    this._item.ElecSetting[0].IsAutoForceRange = itemData.ElecSetting[0].IsAutoForceRange;

                    this._item.ElecSetting[0].MsrtRange = itemData.ElecSetting[0].MsrtRange;
                    this._item.ElecSetting[0].MsrtProtection = itemData.ElecSetting[0].MsrtProtection;
                    this._item.ElecSetting[0].MsrtFilterCount = itemData.ElecSetting[0].MsrtFilterCount;
                    this._item.ElecSetting[0].MsrtUnit = EVoltUnit.V.ToString();
                }
            }

            return this._item;
        }

        #endregion
    }
}
