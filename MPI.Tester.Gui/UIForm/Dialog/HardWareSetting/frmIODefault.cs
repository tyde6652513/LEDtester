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
using System.IO;
using DevComponents.DotNetBar.Controls;


namespace MPI.Tester.Gui
{
    public partial class frmIODefault : Form
    {
        private int _ioQty = 0;
        private IOConfigData _ioSetting = new IOConfigData();

        private bool _isIoInverse = false; // 若IO接上光電開關，因此io由high轉low視為rising    5v  -> DEV - >IO

        public frmIODefault()
        {
            _isIoInverse = false;
            InitializeComponent();

            //btnSave_Click(object sender, EventArgs e)
        }

        public frmIODefault(int ioQty):this()
        {
            _ioQty = ioQty;
            _ioSetting = new IOConfigData(_ioQty);


            //_ioSetting


        }

        public frmIODefault(IOConfigData ioSet, TestItemDescription description)
            : this()
        {
            _ioSetting = ioSet.Clone() as IOConfigData;
            //_ioQty = ioSet.IOList.Count();

            UpdateItemBoudary(description);

            SetDefaultDGV();

            dgvLoadDefaultData();
        }

        private void UpdateItemBoudary(TestItemDescription description)
        {
            foreach (var data in description.Property)
            {
                EItemDescription keyName = (EItemDescription)Enum.Parse(typeof(EItemDescription), data.PropertyKeyName);

                switch (keyName)
                {
                    case EItemDescription.IO_Mode:
                        {
                            IOItemDescription ioData = data as IOItemDescription;
                            foreach(EIOTrig_Mode d in ioData.ModeList)
                            {
                                (this.dgvIODefault.Columns["colMode"] as DataGridViewComboBoxExColumn).Items.Add(d.ToString());
                            }

                        }
                        break;
                    case EItemDescription.IO_Qty:
                        {
                            IOItemDescription ioData = data as IOItemDescription;
                            _ioQty = ioData.IOQty;
                            
                        }
                        break;
                    case EItemDescription.IO_State:
                        {
                            IOItemDescription ioData = data as IOItemDescription;
                            foreach (EIOState d in ioData.StateList)
                            {
                                if (d != EIOState.ASSERT && d != EIOState.WAIT)
                                {
                                    (this.dgvIODefault.Columns["colState"] as DataGridViewComboBoxExColumn).Items.Add(d.ToString());
                                }

                            }
                        }
                        break;
                    case EItemDescription.IO_Inverse:
                        {
                            _isIoInverse = data.DefaultValue > 0 ? true : false;
                        }
                        break;
                }
            }

            List<EIOAct> eList = (from p in Enum.GetNames(typeof(EIOAct))
                                  select ((EIOAct)Enum.Parse(typeof(EIOAct), p))).ToList();
            foreach (EIOAct d in eList)
            {
                (this.dgvIODefault.Columns["colAct"] as DataGridViewComboBoxExColumn).Items.Add(d.ToString());
            }
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            GetDataFromDGV();

            DataCenter._machineConfig.IOConfig = (_ioSetting.Clone() as IOConfigData);

            DataCenter.Save();

            this.Close();
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog SaveFileDialog = new SaveFileDialog();

            SaveFileDialog.Filter = "spc(*.spc)|*.spc";

            SaveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            if (SaveFileDialog.ShowDialog() != DialogResult.OK)
            {
                
                return;
            }

            GetDataFromDGV();

            DataCenter._machineConfig.IOConfig = (_ioSetting.Clone() as IOConfigData);

            DataCenter.Save();

            if (DataCenter._machineConfig.IOConfig.Save(SaveFileDialog.FileName))
            {

            }
            else
            {
                MessageBox.Show("SaveAs Fail");
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "spc(*.spc)|*.spc";

            openFileDialog.Multiselect = false;

            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            if (DataCenter._machineConfig.IOConfig.Load(openFileDialog.FileName))
            {

            }
            else
            {
                MessageBox.Show("Load Fail");
            }

            dgvLoadDefaultData();

        }

        private void dgvIODefault_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            DataGridViewRow dgvR = this.dgvIODefault.Rows[row];

            UpDataRow(dgvR);
        }

        private void GetDataFromDGV()
        {
            foreach (DataGridViewRow dgvR in this.dgvIODefault.Rows)
            {
                int pin = (int)dgvR.Cells["colPin"].Value;

                _ioSetting[pin].DMode = (EIOTrig_Mode)Enum.Parse(typeof(EIOTrig_Mode), dgvR.Cells["colMode"].Value.ToString());

                _ioSetting[pin].DState = (EIOState)Enum.Parse(typeof(EIOState), dgvR.Cells["colState"].Value.ToString());

                _ioSetting[pin].PulseWidth = double.Parse(dgvR.Cells["colPulseWidth"].Value.ToString());

                _ioSetting[pin].IsShow = bool.Parse(dgvR.Cells["colShow"].Value.ToString());

                _ioSetting[pin].Action = (EIOAct)Enum.Parse(typeof(EIOAct), dgvR.Cells["colAct"].Value.ToString()); 

                if (dgvR.Cells["colMark"].Value != null)
                {

                    _ioSetting[pin].Name = (dgvR.Cells["colMark"].Value).ToString();
                }
                else
                {
                    _ioSetting[pin].Name = "";
                }
            }
        }

        private void SetDefaultDGV()
        {
            this.dgvIODefault.Rows.Clear();

            this.dgvIODefault.Rows.Add(_ioQty);

            int counter = 1;

            //dgvIODefault.Columns["colPin"].ReadOnly = true;

            foreach (DataGridViewRow dgvR in this.dgvIODefault.Rows)
            {
                dgvR.Cells["colPin"].Value = counter;

                dgvR.Cells["colMode"].Value = EIOTrig_Mode.TRIG_BYPASS;

                dgvR.Cells["colState"].Value = EIOState.LOW;

                dgvR.Cells["colPulseWidth"].Value = 0;

                dgvR.Cells["colMark"].Value = "";

                dgvR.Cells["colShow"].Value = false;

                dgvR.Cells["colAct"].Value = EIOAct.NONE;

                //List<EIOAct> eList = (from p in Enum.GetNames(typeof(EIOAct))
                //                      select ((EIOAct)Enum.Parse(typeof(EIOAct), p))).ToList();

                //(dgvR.Cells["colAct"] as DataGridViewComboBoxExCell)..AddRange(eList);
                
                ++counter;
            }

            if (_ioSetting.IOList != null && _ioSetting.IOList.Count < _ioQty)
            {
                _ioSetting.SetIOQty(_ioQty);
            }

        }

        private void dgvLoadDefaultData()
        {
            if (DataCenter._machineConfig.SourceMeterModel == ESourceMeterModel.K2600)
            {
                _ioSetting[1].SetDefaultIO(EIOTrig_Mode.TRIG_BYPASS, EIOState.LOW, EIOAct.NONE, 0, "IO_SPT_TRIG_OUT");
                _ioSetting[2].SetDefaultIO(EIOTrig_Mode.TRIG_FALLING, EIOState.HIGH, EIOAct.NONE, 0.0001, "IO_PM_RELAY1");
                _ioSetting[3].SetDefaultIO(EIOTrig_Mode.TRIG_FALLING, EIOState.HIGH, EIOAct.NONE, 0.0005, "IO_PM_RELAY2");
                _ioSetting[4].SetDefaultIO(EIOTrig_Mode.TRIG_FALLING, EIOState.HIGH, EIOAct.NONE, 0.0005, "IO_SMU_ABORT_IN");
                _ioSetting[11].SetDefaultIO(EIOTrig_Mode.TRIG_FALLING, EIOState.HIGH, EIOAct.NONE, 0.0001, "IO_SMU_TRIG_IN");

                _ioSetting[6].SetDefaultIO(EIOTrig_Mode.TRIG_RISING, EIOState.HIGH, EIOAct.NONE, 0.005, "IO_DAQ_ENABLE");
                _ioSetting[7].SetDefaultIO(EIOTrig_Mode.TRIG_RISING, EIOState.HIGH, EIOAct.NONE, 0.005, "IO_RTH_EANBLE");
                _ioSetting[13].SetDefaultIO(EIOTrig_Mode.TRIG_RISING, EIOState.HIGH, EIOAct.NONE, 0.005, "IO_DAQ_TRIG_OUT");
                _ioSetting[14].SetDefaultIO(EIOTrig_Mode.TRIG_RISING, EIOState.HIGH, EIOAct.NONE, 0.0001, "IO_POLAR_SW");



            }
            if ((DataCenter._machineConfig.IOConfig.IOList.Count() >= _ioSetting.IOList.Count()))
            {
                _ioSetting = (DataCenter._machineConfig.IOConfig.Clone() as IOConfigData);
            }

            foreach (IOData ioD in _ioSetting.IOList)
            {
                DataGridViewRow dgvR = this.dgvIODefault.Rows[ioD.PinNum-1];

                dgvR.Cells["colPin"].Value = ioD.PinNum;

                dgvR.Cells["colMode"].Value = ioD.DMode.ToString();

                dgvR.Cells["colState"].Value = ioD.DState.ToString();

                dgvR.Cells["colPulseWidth"].Value = ioD.PulseWidth;

                dgvR.Cells["colMark"].Value = ioD.Name;

                dgvR.Cells["colShow"].Value = ioD.IsShow;

                dgvR.Cells["colAct"].Value = ioD.Action;

                UpDataRow(dgvR);
 
            }
        }



        private  void UpDataRow(DataGridViewRow dgvR)
        {
            EIOTrig_Mode DMode = (EIOTrig_Mode)Enum.Parse(typeof(EIOTrig_Mode), dgvR.Cells["colMode"].Value.ToString());

            EIOAct DAct = (EIOAct)Enum.Parse(typeof(EIOAct), dgvR.Cells["colAct"].Value.ToString());

            EIOState DState = (EIOState)Enum.Parse(typeof(EIOState), dgvR.Cells["colState"].Value.ToString());

            switch (DMode)
            {
                case EIOTrig_Mode.TRIG_BYPASS:

                    dgvR.Cells["colState"].ReadOnly = false;
                    dgvR.Cells["colState"].Style.BackColor = System.Drawing.Color.White;
                    //
                    CheckByPassMode(dgvR, DAct, DState);

                    if (DAct == EIOAct.RISING ||
                            DAct == EIOAct.FALLING)
                    {
                        if (double.Parse(dgvR.Cells["colPulseWidth"].Value.ToString()) < 0.0001)
                        {
                            dgvR.Cells["colPulseWidth"].Value = 0.0001;
                        }
                        dgvR.Cells["colPulseWidth"].ReadOnly = false;
                        dgvR.Cells["colPulseWidth"].Style.BackColor = System.Drawing.Color.White;
                    }
                    else
                    {
                        dgvR.Cells["colPulseWidth"].Value = 0;
                        dgvR.Cells["colPulseWidth"].ReadOnly = true;
                        dgvR.Cells["colPulseWidth"].Style.BackColor = System.Drawing.Color.Silver;
                    }
                    //else if (DAct == EIOAct. ||
                    //        DAct == EIOAct.LEVEL_LOW)


                    break;
                case EIOTrig_Mode.TRIG_FALLING:
                case EIOTrig_Mode.TRIG_RISING:
                    dgvR.Cells["colState"].Value = EIOState.NONE;
                    dgvR.Cells["colPulseWidth"].ReadOnly = false;
                    dgvR.Cells["colPulseWidth"].Style.BackColor = System.Drawing.Color.White;
                    dgvR.Cells["colState"].ReadOnly = true;
                    dgvR.Cells["colState"].Style.BackColor = System.Drawing.Color.Silver;

                    break;
            }
            
        }

            private void CheckByPassMode(DataGridViewRow dgvR, EIOAct DAct, EIOState DState)
            {
                if (DState == EIOState.LOW)
                {
                    if (DAct == EIOAct.RISING ||
                        DAct == EIOAct.LEVEL_HIGH ||
                        DAct == EIOAct.HOLD_HIGH)
                    {
                        dgvR.Cells["colAct"].Style.BackColor = _isIoInverse ? System.Drawing.Color.Red : System.Drawing.Color.White;
                    }
                    else if (DAct == EIOAct.FALLING ||
                        DAct == EIOAct.LEVEL_LOW ||
                        DAct == EIOAct.HOLD_LOW)
                    {
                        dgvR.Cells["colAct"].Style.BackColor = _isIoInverse ? System.Drawing.Color.White : System.Drawing.Color.Red;
                    }
                    //if (_isIoInverse)
                    //{
                    //    if (DAct == EIOAct.RISING ||
                    //        DAct == EIOAct.LEVEL_HIGH ||
                    //        DAct == EIOAct.HOLD_HIGH)
                    //    {
                    //        dgvR.Cells["colAct"].Style.BackColor = System.Drawing.Color.Red;
                    //    }
                    //    else
                    //    {
                    //        dgvR.Cells["colAct"].Style.BackColor = System.Drawing.Color.White;
                    //    }
                    //}
                    //else
                    //{
                    //    if (DAct == EIOAct.FALLING ||
                    //    DAct == EIOAct.LEVEL_LOW ||
                    //    DAct == EIOAct.HOLD_LOW)
                    //    {
                    //        dgvR.Cells["colAct"].Style.BackColor = System.Drawing.Color.Red;
                    //    }
                    //    else
                    //    {
                    //        dgvR.Cells["colAct"].Style.BackColor = System.Drawing.Color.White;
                    //    }
                    //}
                }
                else if (DState == EIOState.HIGH)
                {
                    if (DAct == EIOAct.FALLING ||
                        DAct == EIOAct.LEVEL_LOW ||
                        DAct == EIOAct.HOLD_LOW)
                    {
                        dgvR.Cells["colAct"].Style.BackColor = _isIoInverse ? System.Drawing.Color.Red : System.Drawing.Color.White;
                    }
                    else if (DAct == EIOAct.RISING ||
                            DAct == EIOAct.LEVEL_HIGH ||
                            DAct == EIOAct.HOLD_HIGH)
                    {
                        dgvR.Cells["colAct"].Style.BackColor = _isIoInverse ? System.Drawing.Color.White : System.Drawing.Color.Red;
                    }

                    //if (_isIoInverse)
                    //{
                    //    if (DAct == EIOAct.FALLING ||
                    //    DAct == EIOAct.LEVEL_LOW ||
                    //    DAct == EIOAct.HOLD_LOW)
                    //    {
                    //        dgvR.Cells["colAct"].Style.BackColor = System.Drawing.Color.Red;
                    //    }
                    //    else
                    //    {
                    //        dgvR.Cells["colAct"].Style.BackColor = System.Drawing.Color.White;
                    //    }
                    //}
                    //else
                    //{
                    //    if (DAct == EIOAct.RISING ||
                    //        DAct == EIOAct.LEVEL_HIGH ||
                    //        DAct == EIOAct.HOLD_HIGH)
                    //    {
                    //        dgvR.Cells["colAct"].Style.BackColor = System.Drawing.Color.Red;
                    //    }
                    //    else
                    //    {
                    //        dgvR.Cells["colAct"].Style.BackColor = System.Drawing.Color.White;
                    //    }
                    //}
                }
            }

       

      


    }
}
