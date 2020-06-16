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

    public partial class frmLCRCali : Form
    {
        private LCRCaliData _item;
       
        public frmLCRCali()
        {
            InitializeComponent();
        }

        public frmLCRCali(TestItemDescription description) : this()
        {
            this.UpdateItemBoudary(description);

            //this.UpdateCondtionDataToComponent(DataCenter._conditionCtrl.LCRCaliData);

            this.UpdateCondtionDataToComponent(DataCenter._sysCali.SystemCaliData.LCRCaliData);

            //-------------------------------------------------------------------
            if (this.cmbFunc.Items.Count == 0 || this.cmbCable.Items.Count == 0 || this.cmbDataNum.Items.Count == 0)
            {
                this.grpGeneral.Enabled = false;
                this.grpCaliData.Enabled = false;
                this.grpTestCtrl.Enabled = false;
            }

        }

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
                    
                    case EItemDescription.LCR_Frequency:
                        {
                            this.dinFrequency.MaxValue = data.MaxValue ; //20170901 David 一律使用Hz

                            this.dinFrequency.MinValue = data.MinValue ;
                            break;
                        }
                    case EItemDescription.LCR_CaliType:
                        {
                            this.cmbFunc.Items.Clear();

                            LCRItemDescription lcrData = data as LCRItemDescription;

                            if (lcrData.CaliTypeList.Count != 0)
                            {
                                foreach (var name in lcrData.CaliTypeList)
                                {
                                    this.cmbFunc.Items.Add(name.ToString());
                                }

                                this.cmbFunc.SelectedIndex = 0;
                            }
                            break;
                        }
                    case EItemDescription.LCR_CaliDataQty:
                        {
                            this.cmbDataNum.Items.Clear();

                            LCRItemDescription lcrData = data as LCRItemDescription;

                            for(int i = 1;i< lcrData.CaliDataQty+1;++i)
                            {
                                this.cmbDataNum.Items.Add(i.ToString());
                            }

                            break;
                        }
                    case EItemDescription.LCR_CableLength:
                        {
                            this.cmbCable.Items.Clear();

                            LCRItemDescription lcrData = data as LCRItemDescription;

                            if (lcrData.CableLengthList.Count != 0)
                            {
                                foreach (var name in lcrData.CableLengthList)
                                {
                                    this.cmbCable.Items.Add(name.ToString());
                                }

                                this.cmbCable.SelectedIndex = 0;
                            }
                            break;
                        }
                    case EItemDescription.LCR_SignalLevelV:
                        {
                            this.dinLevel.Visible = data.IsVisible;
                            this.dinLevel.Enabled = data.IsVisible;
                            this.dinLevel.MaxValue = data.MaxValue;
                            this.dinLevel.MinValue = data.MinValue;
                            this.dinLevel.Value = data.DefaultValue;
                            this.dinLevel.DisplayFormat = data.Format;
                            break;
                        }
                    case EItemDescription.LCR_DCBiasV:
                        {
                            this.dinBias.Visible = data.IsVisible;
                            this.dinBias.Enabled = data.IsVisible;
                            this.dinBias.MaxValue = data.MaxValue;
                            this.dinBias.MinValue = data.MinValue;
                            this.dinBias.Value = data.DefaultValue;
                            this.dinBias.DisplayFormat = data.Format;
                            break;
                        }
                     
                    default:
                        break;
                }

            }

            cmbRefAUnit.Items.AddRange(Enum.GetNames(typeof(ECapUnit)));
        }

        private void UpDateUIWhenDataChange(int dataNum)
        {
            int nowDataNum = dataNum -1;

            this.chkOpen.Checked = _item.EnableOpen;

            this.chkShort.Checked = _item.EnableShort;

            this.chkLoad.Checked = _item.EnableLoad;

            this.cmbFunc.SelectedItem = _item.TestType.ToString();

            this.cmbCable.SelectedItem = _item.CableLength;

            for (int i = _item.LoadingList.Count; i < cmbDataNum.Items.Count; ++i)
            {
                _item.LoadingList.Add(new LoadingRefData());
            }

            this.dinFrequency.Value = _item.LoadingList[nowDataNum].Freq;

            this.cmbRefAUnit.SelectedItem = _item.LoadingList[nowDataNum].RefUnit.ToString();

            this.dinRefA.Value = _item.LoadingList[nowDataNum].RefA;

            this.dinRefB.Value = _item.LoadingList[nowDataNum].RefB;

            this.chkEnableData.Checked = _item.LoadingList[nowDataNum].Enable;
        }

        #endregion

        #region >>> UI Ctrl <<<

        private void btnSet_Click(object sender, EventArgs e)
        {
            GetConditionDataFromComponent();

            //AppSystem.RunCommand(TestKernel.ETesterKernelCmd.LCR_Set);
            AppSystem.RunLCRCalibration(ELCRCaliMode.Set);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            GetConditionDataFromComponent();
           // AppSystem.RunCommand(TestKernel.ETesterKernelCmd.LCR_Open);
            AppSystem.RunLCRCalibration(ELCRCaliMode.Open);
        }

        private void btnShort_Click(object sender, EventArgs e)
        {
            GetConditionDataFromComponent();
           // AppSystem.RunCommand(TestKernel.ETesterKernelCmd.LCR_Short);
            AppSystem.RunLCRCalibration(ELCRCaliMode.Short);

        }

        private void btnload_Click(object sender, EventArgs e)
        {
            GetConditionDataFromComponent();
            //AppSystem.RunCommand(TestKernel.ETesterKernelCmd.LCR_Load);
            AppSystem.RunLCRCalibration(ELCRCaliMode.Load);
        }

        private void cmbDataNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpDateUIWhenDataChange(int.Parse(cmbDataNum.Text));
        }
        //private void frmLCRCali_Leave(object sender, EventArgs e)
        //{
            
        //}
        private void frmLCRCali_FormClosing(object sender, FormClosingEventArgs e)
        {
            AppSystem.RunLCRCalibration(ELCRCaliMode.DataCollect);
        }
        #endregion

        #region >>> Public Method <<<

        public void UpdateCondtionDataToComponent(LCRCaliData data)
        {
            this._item = data.Clone() as LCRCaliData;

            int nowDataNum = _item.NowDataNum - 1;

            this.cmbDataNum.SelectedItem = _item.NowDataNum.ToString();

            this.chkOpen.Checked = _item.EnableOpen;

            this.chkShort.Checked = _item.EnableShort;

            this.chkLoad.Checked = _item.EnableLoad;

            this.cmbFunc.SelectedItem = _item.TestType.ToString();

            this.cmbCable.SelectedItem = _item.CableLength;

            for (int i = _item.LoadingList.Count; i < cmbDataNum.Items.Count; ++i)
            {
                _item.LoadingList.Add(new LoadingRefData());
            }

            this.dinFrequency.Value = _item.LoadingList[nowDataNum].Freq;

            this.cmbRefAUnit.SelectedItem = _item.LoadingList[nowDataNum].RefUnit.ToString();

            this.dinRefA.Value = _item.LoadingList[nowDataNum].RefA;

            this.dinRefB.Value = _item.LoadingList[nowDataNum].RefB;

            this.dinLevel.Value = _item.Level ;

            this.dinBias.Value = _item.Bias;

            this.chkEnableData.Checked = _item.LoadingList[nowDataNum].Enable;

        }

        public void GetConditionDataFromComponent()
        {
            //this._item = data.Clone() as LCRCaliData;
            int nowDataNum;
            if (int.TryParse(this.cmbDataNum.SelectedItem.ToString(), out nowDataNum))
            {
                nowDataNum--;
                this._item.NowDataNum = nowDataNum + 1;

                _item.EnableOpen = this.chkOpen.Checked;

                _item.EnableShort = this.chkShort.Checked;

                _item.EnableLoad = this.chkLoad.Checked;

                _item.CableLength = this.cmbCable.SelectedItem.ToString();

                _item.TestType = (ELCRTestType)Enum.Parse(typeof(ELCRTestType), this.cmbFunc.SelectedItem.ToString(), true);

                _item.LoadingList[nowDataNum].Freq = (int)this.dinFrequency.Value;

                _item.LoadingList[nowDataNum].RefUnit = (ECapUnit)Enum.Parse(typeof(ECapUnit), this.cmbRefAUnit.SelectedItem.ToString(), true);

                _item.LoadingList[nowDataNum].RefA = this.dinRefA.Value;

                _item.LoadingList[nowDataNum].RefB = this.dinRefB.Value;

                _item.LoadingList[nowDataNum].Enable = this.chkEnableData.Checked;

                _item.Level = this.dinLevel.Value;

                _item.Bias = this.dinBias.Value;

                //DataCenter._conditionCtrl.LCRCaliData = this._item.Clone() as LCRCaliData;

                DataCenter._sysCali.SystemCaliData.LCRCaliData = this._item.Clone() as LCRCaliData;
            }
        }

        #endregion




    }
}
