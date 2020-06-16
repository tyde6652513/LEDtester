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


namespace MPI.Tester.Gui.UIForm.Dialog.HardWareSetting
{
    public partial class frmLCRCaliVer2 : Form
    {
        private LCRCaliData _item;

        public frmLCRCaliVer2()
        {
            InitializeComponent();
        }

        public frmLCRCaliVer2(TestItemDescription description)
            : this()
        {
            this.UpdateItemBoudary(description);

            //this.UpdateCondtionDataToComponent(DataCenter._conditionCtrl.LCRCaliData);

            this.UpdateCondtionDataToComponent(DataCenter._sysCali.SystemCaliData.LCRCaliData);

            //-------------------------------------------------------------------
            if (this.cmbFunc.Items.Count == 0 || this.cmbCable.Items.Count == 0 )
            {
                this.grpGeneral.Enabled = false;
                this.grpCaliData.Enabled = false;
                this.grpTestCtrl.Enabled = false;
            }

        }

        public void GetConditionDataFromComponent()
        {
            //this._item = data.Clone() as LCRCaliData;
            int nowDataNum = 0; //用不到，但為了向前相容先填0
            //if (int.TryParse(this.cmbDataNum.SelectedItem.ToString(), out nowDataNum))
            //{
            //    nowDataNum--;
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

            _item.LoadingList[nowDataNum].LoadRaw.ValA = this.dinRefA.Value;

            _item.LoadingList[nowDataNum].LoadRaw.ValB = this.dinRefB.Value;

            _item.LoadingList[nowDataNum].LoadRaw.MeterUnitA = this.cmbRefAUnit.SelectedItem.ToString();
            _item.LoadingList[nowDataNum].LoadRaw.MeterUnitB = "";

            _item.LoadingList[nowDataNum].Enable = true;//強迫啟動

            _item.Level = dInLevel.Value;

            _item.Bias = 0;//用不到，但為了向前相容先填0

            //DataCenter._conditionCtrl.LCRCaliData = this._item.Clone() as LCRCaliData;

            DataCenter._sysCali.SystemCaliData.LCRCaliData = this._item.Clone() as LCRCaliData;
            //}
        }


        #region>>private method<<

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
                            this.dinFrequency.MaxValue = data.MaxValue; //20170901 David 一律使用Hz

                            this.dinFrequency.MinValue = data.MinValue;
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
                    //case EItemDescription.LCR_CaliDataQty:
                    //    {
                    //        this.cmbDataNum.Items.Clear();

                    //        LCRItemDescription lcrData = data as LCRItemDescription;

                    //        for (int i = 1; i < lcrData.CaliDataQty + 1; ++i)
                    //        {
                    //            this.cmbDataNum.Items.Add(i.ToString());
                    //        }

                    //        break;
                    //    }
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
                    case EItemDescription.LCR_Enable_Open_Cali:
                        {
                            if (data.DefaultValue > 0)
                            {
                                btnOpen.Visible = true;
                                chkOpen.Visible = true;
                            }
                            else 
                            {
                                btnOpen.Visible = false;
                                chkOpen.Visible = false;
                            }
                            break;
                        }
                    case EItemDescription.LCR_Enable_Short_Cali:
                        {
                            if (data.DefaultValue > 0)
                            {
                                btnShort.Visible = true;
                                chkShort.Visible = true;
                            }
                            else
                            {
                                btnShort.Visible = false;
                                chkShort.Visible = false;
                            }
                            break;
                        }
                    case EItemDescription.LCR_Enable_Load_Cali:
                        {
                            if (data.DefaultValue > 0)
                            {
                                chkLoad.Visible = true;
                                btnload.Visible = true;
                                grpCaliData.Visible = true;
                            }
                            else
                            {
                                chkLoad.Visible = false;
                                btnload.Visible = false;
                                grpCaliData.Visible = false;
                            }
                            break;
                        }
                    case EItemDescription.LCR_SignalLevelV:
                        {
                            dInLevel.MaxValue = data.MaxValue;
                            dInLevel.MinValue = data.MinValue;
                            dInLevel.DisplayFormat = data.Format;
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
            int nowDataNum = dataNum - 1;

            this.chkOpen.Checked = _item.EnableOpen;

            this.chkShort.Checked = _item.EnableShort;

            this.chkLoad.Checked = _item.EnableLoad;

            this.cmbFunc.SelectedItem = _item.TestType.ToString();

            this.cmbCable.SelectedItem = _item.CableLength;

            //for (int i = _item.LoadingList.Count; i < cmbDataNum.Items.Count; ++i)
            //{
            //    _item.LoadingList.Add(new LoadingRefData());
            //}

            this.dinFrequency.Value = _item.LoadingList[nowDataNum].Freq;

            this.cmbRefAUnit.SelectedItem = _item.LoadingList[nowDataNum].RefUnit.ToString();

            this.dinRefA.Value = _item.LoadingList[nowDataNum].RefA;

            this.dinRefB.Value = _item.LoadingList[nowDataNum].RefB;

            //this.chkEnableData.Checked = _item.LoadingList[nowDataNum].Enable;
        }

        private void RefreshRichTextBox(bool aquireDataFromCMeter = false)
        {
            if (aquireDataFromCMeter)
            {
                AppSystem.RunLCRCalibration(ELCRCaliMode.DataCollect);
            }
            this.UpdateCondtionDataToComponent(DataCenter._sysCali.SystemCaliData.LCRCaliData);
            
            Console.WriteLine("[frmLCRCaliVer2],RefreshRichTextBox");            
        }
        #endregion


        #region

        public void UpdateCondtionDataToComponent(LCRCaliData data)
        {
            this._item = data.Clone() as LCRCaliData;

            int nowDataNum = _item.NowDataNum - 1;

            //this.cmbDataNum.SelectedItem = _item.NowDataNum.ToString();

            this.chkOpen.Checked = _item.EnableOpen;

            this.chkShort.Checked = _item.EnableShort;

            this.chkLoad.Checked = _item.EnableLoad;

            this.cmbFunc.SelectedItem = _item.TestType.ToString();

            this.cmbCable.SelectedItem = _item.CableLength;

            //for (int i = _item.LoadingList.Count; i < cmbDataNum.Items.Count; ++i)
            //{
            //    _item.LoadingList.Add(new LoadingRefData());
            //}

            this.dinFrequency.Value = _item.LoadingList[nowDataNum].Freq;

            this.cmbRefAUnit.SelectedItem = _item.LoadingList[nowDataNum].RefUnit.ToString();

            this.dinRefA.Value = _item.LoadingList[nowDataNum].RefA;

            this.dinRefB.Value = _item.LoadingList[nowDataNum].RefB;

            //this.dinLevel.Value = _item.Level;

            //this.dinBias.Value = _item.Bias;

            //this.chkEnableData.Checked = _item.LoadingList[nowDataNum].Enable;
            string str = "Calibration Infomation:\n" + _item.ToString();
            Console.WriteLine("[frmLCRCaliVer2],UpdateCondtionDataToComponent," + str);
            rtbCaliResult.Clear();
            rtbCaliResult.Text = str;

        }

        #endregion

        private void btnOpen_Click(object sender, EventArgs e)
        {
            GetConditionDataFromComponent();
            AppSystem.RunLCRCalibration(ELCRCaliMode.Open);
            RefreshRichTextBox(true);
            Console.WriteLine("[frmLCRCaliVer2],Open Calibrate finished");
            MessageBox.Show("Open Calibrate finished");
        }

        private void btnShort_Click(object sender, EventArgs e)
        {
            GetConditionDataFromComponent();
            AppSystem.RunLCRCalibration(ELCRCaliMode.Short);
            RefreshRichTextBox(true);
            Console.WriteLine("[frmLCRCaliVer2],Short Calibrate finished");
            MessageBox.Show("Short Calibrate finished");
        }

        private void btnload_Click(object sender, EventArgs e)
        {
            GetConditionDataFromComponent();
            AppSystem.RunLCRCalibration(ELCRCaliMode.Load);
            RefreshRichTextBox(true);
            Console.WriteLine("[frmLCRCaliVer2],Load Calibrate finished");
            MessageBox.Show("Load Calibrate finished");
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            GetConditionDataFromComponent();
            AppSystem.RunLCRCalibration(ELCRCaliMode.Set);
            Console.WriteLine("[frmLCRCaliVer2],Set Cali Data finished");
            RefreshRichTextBox();
        }
    }
}
