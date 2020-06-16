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

using DevComponents.DotNetBar.Controls;

namespace MPI.Tester.Gui
{
    public partial class frmItemSettingIO : Form, IConditionUICtrl, IConditionElecCtrl
    {
        private object _lockObj;

        private IOTestItem _item;
        private bool _isEnableSwitchChannel = false;
        private bool _isEnableMsrtForceValue = false;

        private bool _isIoInverse = false; // 若IO接上光電開關，因此io由high轉low視為rising    5v  -> DEV - >IO

        int _ioQty = 0;

        public frmItemSettingIO()
        {
            InitializeComponent();
        }

        public frmItemSettingIO(TestItemDescription description)
            : this()
        {
            _lockObj = new object();
            _isEnableSwitchChannel = false;
            _isEnableMsrtForceValue = false;
            this.UpdateItemBoudary(description);
            SetDefaultDGV();
            dgvLoadDefaultData();

            _item = new IOTestItem();
        }

        #region >>> Public Property <<<

        public bool IsAutoSelectForceRange
        {
            get;
            set;
        }

        public bool IsAutoSelectMsrtRange
        {
            get;
            set;
        }

        public bool IsVisibleFilterCount
        {
            get;
            set;
        }

        public bool IsVisibleNPLC
        {
            get;
            set;
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

        #region >>> Public Method <<<

        public void RefreshUI()
        {
        }

        public bool CheckUI(out string msg)
        {
            msg = string.Empty;

            return true;
        }

        public TestItemData GetConditionDataFromComponent()
        {
            _item.IOSetData.CmdList.Clear();

            foreach (DataGridViewRow dgvR in this.dgvIO.Rows)
            {
                IOCmd cmdData = new IOCmd();

                if (dgvR.Visible == true)
                {
                    cmdData.Pin = int.Parse(dgvR.Cells["colPin"].Value.ToString());

                    cmdData.Mode = (EIOTrig_Mode)Enum.Parse(typeof(EIOTrig_Mode), dgvR.Cells["colMode"].Value.ToString());

                    cmdData.HoldTime = double.Parse(dgvR.Cells["colPulseWidth"].Value.ToString());

                    cmdData.DelayTime = 0;

                    if (bool.Parse((dgvR.Cells["colEn"] as DataGridViewCheckBoxXCell).Value.ToString()) == true)
                    {
                        
                        EIOAct act = (EIOAct)Enum.Parse(typeof(EIOAct), dgvR.Cells["colAct"].Value.ToString());

                        //_isIoInverse 是考量到IO會接上光電開關，因此io由high轉low視為rising    5v  -> DEV - >IO
                        switch (act)
                        {
                            case (EIOAct.HOLD_HIGH):
                                {
                                    cmdData.HoldTime = -1;
                                    cmdData.State = _isIoInverse ? EIOState.LOW : EIOState.HIGH;
                                }
                                break;
                            case (EIOAct.HOLD_LOW):
                                {
                                    cmdData.HoldTime = -1;
                                    cmdData.State = _isIoInverse ? EIOState.HIGH : EIOState.LOW;
                                }
                                break;
                            case (EIOAct.LEVEL_HIGH):
                                {
                                    cmdData.State = _isIoInverse ? EIOState.LOW : EIOState.HIGH;
                                }
                                break;
                            case (EIOAct.LEVEL_LOW):
                                {
                                    cmdData.State = _isIoInverse ? EIOState.HIGH : EIOState.LOW;
                                }
                                break;
                            case (EIOAct.RISING):
                                {
                                    cmdData.State = _isIoInverse ? EIOState.LOW : EIOState.HIGH;
                                }
                                break;
                            case (EIOAct.FALLING):
                                {
                                    cmdData.State = _isIoInverse ? EIOState.HIGH : EIOState.LOW;
                                }
                                break;
                            default:
                                {
                                    cmdData.State = (EIOState)Enum.Parse(typeof(EIOState), dgvR.Cells["colState"].Value.ToString()); ;
                                }
                                break;  

                        }

                    }
                    else 
                    {
                        cmdData.State = (EIOState)Enum.Parse(typeof(EIOState), dgvR.Cells["colState"].Value.ToString());
                    }


                    

                    _item.IOSetData.CmdList.Add(cmdData);
                }
            }


            return this._item;
        }

        public void UpdateCondtionDataToComponent(TestItemData data)//要考量第一次輸入時不知道IO數量的問題
        {
            this._item = data.Clone() as IOTestItem;

            foreach (IOCmd cmdData in _item.IOSetData.CmdList)
            {
                if (DataCenter._machineConfig.IOConfig[cmdData.Pin] == null)
                {
                    continue;
                }
                DataGridViewRow dgvR = this.dgvIO.Rows[cmdData.Pin - 1];



                dgvR.Cells["colPin"].Value = cmdData.Pin;

                dgvR.Cells["colMode"].Value = cmdData.Mode;

                dgvR.Cells["colState"].Value = DataCenter._machineConfig.IOConfig[cmdData.Pin].DState;//cmdData.State;

                //dgvR.Cells["colDelay"].Value = cmdData.DelayTime;

                dgvR.Cells["colPulseWidth"].Value = cmdData.HoldTime < 0 ? 0 : cmdData.HoldTime;

                if (CheckPinOnState(cmdData.Pin, data.Order))
                {
                    dgvR.DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                }
                else
                {
                    dgvR.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                }

                if (cmdData.State != DataCenter._machineConfig.IOConfig[cmdData.Pin].DState)
                {
                    dgvR.Cells["colEn"].Value = true;
                }

            }
            
        }

        #endregion

        #region>>private method<<

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
                            foreach (EIOTrig_Mode d in ioData.ModeList)
                            {
                                (this.dgvIO.Columns["colMode"] as DataGridViewComboBoxExColumn).Items.Add(d.ToString());
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
                                (this.dgvIO.Columns["colState"] as DataGridViewComboBoxExColumn).Items.Add(d.ToString());
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
        }

        private void SetDefaultDGV()
        {
            this.dgvIO.Rows.Clear();

            this.dgvIO.Rows.Add(_ioQty);

            int counter = 1;

            foreach (DataGridViewRow dgvR in this.dgvIO.Rows)
            {
                dgvR.Cells["colPin"].Value = counter;

                dgvR.Cells["colMode"].Value = EIOTrig_Mode.TRIG_BYPASS;

                dgvR.Cells["colState"].Value = EIOState.NONE;

                dgvR.Cells["colPulseWidth"].Value = 0;

                dgvR.Cells["colMark"].Value = "";

                dgvR.Cells["colAct"].Value = (EIOAct.NONE).ToString();

                (dgvR.Cells["colEn"] as DataGridViewCheckBoxXCell).Value = false;

                ++counter;
            }
        }

        private void dgvLoadDefaultData()
        {
            int nowPin = 1;
            bool isHold = CheckPinOnState(nowPin);

            foreach (IOData ioD in DataCenter._machineConfig.IOConfig.IOList)
            {
                DataGridViewRow dgvR = this.dgvIO.Rows[ioD.PinNum - 1];                

                if (!ioD.IsShow || ioD.DMode != EIOTrig_Mode.TRIG_BYPASS)//先不顯示其他操作，後面有時間再改
                {
                    dgvR.Visible = false;
                }
                
                if (ioD.DMode != EIOTrig_Mode.TRIG_BYPASS)
                {
                    dgvR.ReadOnly = true;
                    dgvR.DefaultCellStyle.BackColor = System.Drawing.Color.Silver;

                    //dgvR.Cells["colState"].ReadOnly = false;
                    //dgvR.Cells["colState"].Style.BackColor = System.Drawing.Color.White;
                    //dgvR.Cells["colDelay"].ReadOnly = false;
                    //dgvR.Cells["colDelay"].Style.BackColor = System.Drawing.Color.White;
                    //dgvR.Cells["colPulseWidth"].ReadOnly = false;
                    //dgvR.Cells["colPulseWidth"].Style.BackColor = System.Drawing.Color.White;

                }
                else
                {
                    if (CheckPinOnState(ioD.PinNum))
                    {
                        dgvR.DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                    }
                    else 
                    {
                        dgvR.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                    }
                }

                dgvR.Cells["colPin"].Value = ioD.PinNum;

                UpDataRow(dgvR);

                dgvR.Cells["colMode"].Value = ioD.DMode.ToString();

                dgvR.Cells["colState"].Value = ioD.DState.ToString();

                //dgvR.Cells["colDelay"].Value = 0;

                dgvR.Cells["colPulseWidth"].Value = ioD.PulseWidth;

                dgvR.Cells["colMark"].Value = ioD.Name;

                dgvR.Cells["colMark"].Style.BackColor = System.Drawing.Color.Silver;

                

            }
        }

        private bool CheckPinOnState(int nowPin , int itemNum = -1)
        {
            bool isHold = false;

            if (DataCenter._conditionCtrl.Data.TestItemArray == null)
            {
                return false;
            }

            if (itemNum < 0)
            {
                itemNum = DataCenter._conditionCtrl.Data.TestItemArray.Length;
            }

            if (DataCenter._conditionCtrl.Data.TestItemArray != null)
            {
                for (int i = itemNum -1; i >= 0; --i)
                {
                    TestItemData tD = DataCenter._conditionCtrl.Data.TestItemArray[i];
                    if (tD.ElecSetting != null && 
                        tD.ElecSetting[0].IOSetting != null)
                    {
                        IOCmd cD = tD.ElecSetting[0].IOSetting[nowPin];
                        if (cD != null)
                        {
                            if (cD.State != DataCenter._machineConfig.IOConfig.IOList[nowPin - 1].DState
                                && cD.HoldTime < 0)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return isHold;
        }

        private void UpDataRow(DataGridViewRow dgvR)
        {

            //dgvR.Cells["colAct"].Value = EIOAct.NONE;

            //EIOTrig_Mode mode = (EIOTrig_Mode)Enum.Parse(typeof(EIOTrig_Mode), dgvR.Cells["colMode"].Value.ToString());

            //EIOState state = (EIOState)Enum.Parse(typeof(EIOState), dgvR.Cells["colState"].Value.ToString());
            int index  = int.Parse(dgvR.Cells["colPin"].Value.ToString()) -1;
            IOData iData = DataCenter._machineConfig.IOConfig.IOList[index];


            dgvR.Cells["colAct"].Value = iData.Action.ToString();  
            //if (iData.Action != EIOAct.NONE)
            //{
            //    dgvR.Cells["colAct"].Value = iData.Action.ToString();                
            //}

            if (iData.Action == EIOAct.LEVEL_HIGH ||
                iData.Action == EIOAct.LEVEL_LOW)
            {
                dgvR.Cells["colPulseWidth"].ReadOnly = false;
                dgvR.Cells["colPulseWidth"].Style.BackColor = System.Drawing.Color.White;
            }
            else 
            {
                dgvR.Cells["colPulseWidth"].ReadOnly = true;
                dgvR.Cells["colPulseWidth"].Style.BackColor = System.Drawing.Color.Silver;

                if (iData.Action == EIOAct.HOLD_HIGH ||
                    iData.Action == EIOAct.HOLD_LOW)
                {
                    dgvR.Cells["colPulseWidth"].Value = 0;
                }
                else if (iData.Action == EIOAct.FALLING ||
                    iData.Action == EIOAct.RISING)
                {
                    dgvR.Cells["colPulseWidth"].Value = iData.PulseWidth;
                }
            }


            
            //switch (mode)
            //{
            //    case EIOTrig_Mode.TRIG_BYPASS:
                    
                    
            //        //dgvR.Cells["colState"]
            //        //if (state != EIOState.NONE &&
            //        //    state != EIOState.LOW &&
            //        //    state != EIOState.HIGH)
            //        //{
            //        //    dgvR.Cells["colState"].Value = EIOState.NONE;
            //        //}

            //        break;
            //    case EIOTrig_Mode.TRIG_FALLING:
            //    case EIOTrig_Mode.TRIG_RISING:

            //        //if (state != EIOState.NONE &&
            //        //    state != EIOState.ASSERT &&
            //        //    state != EIOState.WAIT)
            //        //{
            //        //    dgvR.Cells["colState"].Value = EIOState.NONE;
            //        //}

            //        break;
            //}
        }

        #endregion


        private void dgvIO_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            DataGridViewRow dgvR = this.dgvIO.Rows[row];

            UpDataRow(dgvR);
        }
    }
}
