using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using MPI.Tester.Data;
using MPI.Tester.Gui;
using MPI.Tester.DeviceCommon;
using System.Reflection;

namespace MPI.Tester.Gui.UIForm.Dialog.HardWareSetting
{
    public partial class frmAttenuator : Form
    {
        private AttenuatorSettingData _aPara;

        private AttenuatorSpec _attSpec;

        public event EventHandler<EventArgs> TestItemDataChangeEvent;

        public frmAttenuator()
        {
            InitializeComponent();

            //btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;

            this.TestItemDataChangeEvent += new EventHandler<EventArgs>(Host.TestItemDataChangeEventHandler);

            _aPara = new AttenuatorSettingData();

            _attSpec = new AttenuatorSpec();

            pgdAttenuator.SelectedObject = _aPara;

            SetAPowModeUI(_aPara.APMode.ToString());

            pgdAttenuator.ExpandAllGridItems();

            lblMsrtPowerUnit.Text = _aPara.PowerUnit.ToString();
        }

        public frmAttenuator(TestItemDescription description)
            : this()
        {
            UpdateItemBoudary(description);

            CheckLimit();

            lblMsrtPowerUnit.Text = _aPara.PowerUnit.ToString();
        }

        public frmAttenuator(TestItemDescription description, AttenuatorSettingData attData)
            : this(description)
        {
            _aPara = attData.Clone() as AttenuatorSettingData;

            pgdAttenuator.SelectedObject = _aPara;

            SetAPowModeUI(_aPara.APMode.ToString());

            pgdAttenuator.ExpandAllGridItems();

            CheckLimit();

            pgdAttenuator.Refresh();

            lblMsrtPowerUnit.Text = _aPara.PowerUnit.ToString();
        }

        #region

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

                    case EItemDescription.ATT_Attenuat_Range:
                        {
                            _attSpec.AttenuationRange = new MinMaxValuePair<double>(data.MinValue, data.MaxValue);
                            break;
                        }
                    case EItemDescription.ATT_Power_Range:
                        {
                            _attSpec.PowerRange = new MinMaxValuePair<double>(data.MinValue, data.MaxValue);
                            break;
                        }
                    case EItemDescription.ATT_AvgTime:
                        {
                            _attSpec.AveragingTime = new MinMaxValuePair<double>(data.MinValue, data.MaxValue);
                            break;
                        }
                    case EItemDescription.ATT_TransitionSpeed:
                        {
                            _attSpec.TransitionSpeed = new MinMaxValuePair<double>(data.MinValue, data.MaxValue);
                            break;
                        }
                    case EItemDescription.ATT_WavelengthRange:
                        {
                            _attSpec.WavelengthRange = new MinMaxValuePair<double>(data.MinValue, data.MaxValue);
                            break;
                        }

                    default:
                        break;
                }

            }

        }


        private void CheckLimit()
        {
            _aPara.Speed = _attSpec.TransitionSpeed.SetInRange(_aPara.Speed);

            _aPara.WaveLength = _attSpec.WavelengthRange.SetInRange(_aPara.WaveLength);

            _aPara.AvgTime = _attSpec.AveragingTime.SetInRange(_aPara.AvgTime);

            _aPara.Attenuate.Offset = _attSpec.AttenuationRange.SetInRange(_aPara.Attenuate.Offset);//要注意加上Att offset後的極限，記得另外處理

            _aPara.Attenuate.Set = _attSpec.AttenuationRange.SetInRange(_aPara.Attenuate.Set);//要注意加上offset後的極限，記得另外處理

            _aPara.Power.Offset = _attSpec.PowerRange.SetInRange(_aPara.Power.Offset);//要注意加上Att offset後的極限，記得另外處理


            if (_aPara.PowerUnit == ELaserPowerUnit.W)
            {
                double setW = _aPara.Power.Set;
                double setdBm = dBm2WConverter.W2dBm(setW);

                setdBm  = _attSpec.PowerRange.SetInRange(setdBm);

                _aPara.Power.Set = dBm2WConverter.dBm2W(setdBm);
            }

            pgdAttenuator.Refresh();
        }      

        private void SetAPowModeUI(string nowMode)
        {
            if (nowMode == "Pwoer")
            {
                _aPara.Power.Mode = "Power";
                _aPara.Power.Unit = _aPara.PowerUnit.ToString(); ;
                pgdOutput.SelectedObject = _aPara.Power;
            }
            else if (nowMode == "Attenuator")
            {
                _aPara.Attenuate.Mode = "Attenuator";
                _aPara.Attenuate.Unit = "dB";
                pgdOutput.SelectedObject = _aPara.Attenuate; 
            }
            pgdAttenuator.Refresh();
        }

        private void SetAPowMode2Data(string nowMode)
        {
            //if (nowMode == "Pwoer")
            //{
            //    double value = 0.001;
            //    double.TryParse(txtSetPower.Text, out value);
            //    _aPara.Power.Set = value;
            //}
            //else if (nowMode == "Attenuator")
            //{
            //    double value = 1;
            //    double.TryParse(txtSetPower.Text, out value);
            //    _aPara.Attenuate.Set = value;
            //}

            CheckLimit();
            //pgdAttenuator.Refresh();
        }

        #endregion

        private void btnMsrt_Click(object sender, EventArgs e)
        {
            GlobalFlag.IsReSingleTestMode = true;

            AppSystem.RunAttenuator(ELaserSourceSysAction.ATTENUATOR_MSRT);

            Thread.Sleep(50);

            float val = DataCenter._acquireData.DeviceRunTimeDataSet["ATT"]["ATTPower"].DataArray[0];

            txtMsrt.Text = val.ToString("E");
        }

        //private void btnMsrtPd_Click(object sender, EventArgs e)
        //{
        //    GlobalFlag.IsReSingleTestMode = true;
        //    Dictionary<string ,double> keyValDic = new Dictionary<string,double>();
        //    keyValDic.Add("FT",100);
        //    keyValDic.Add("Bias",2);
        //    keyValDic.Add("Clamp",0.001);


        //    AppSystem.RunAttenuator(ELaserSourceSysAction.MoniterPD_MSRT, keyValDic);

        //    float val = DataCenter._acquireData.DeviceRunTimeDataSet["SMU"]["PdMoniterCurr"].DataArray[0];

        //    textBox1.Text = val.ToString("E");
            
        //}

        private void pgdAttenuator_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            object oldObj = e.OldValue;

            switch (e.ChangedItem.PropertyDescriptor.DisplayName)
            {
                case "Unit of Power":
                    {
                        if (oldObj.ToString() != e.ChangedItem.Value.ToString())
                        {
                            if (e.ChangedItem.Value.ToString() == "W")
                            {
                                _aPara.PowdBm2W();
                                
                            }
                            else if (e.ChangedItem.Value.ToString() == "dBm")
                            {
                                _aPara.PowW2dBm();
                            }
                            pgdAttenuator.Refresh();
                        }

                        lblMsrtPowerUnit.Text = e.ChangedItem.Value.ToString();
                    }
                    break;
                    //------------------------------------------------------------------
                case "Attenuat/Power":
                    {
                        string nowMode = e.ChangedItem.Value.ToString();
                        SetAPowModeUI(nowMode);
                    }
                    break;
                case "Auto Power Contorl":
                    {
                        string nowMode = e.ChangedItem.Value.ToString();
                        if (nowMode == "True")
                        {
                            _aPara.APMode = EAPMode.Pwoer;
                            SetAPowModeUI("Pwoer");
                        }
                        else
                        {
                            _aPara.APMode = EAPMode.Attenuator;
                            SetAPowModeUI("Attenuator");
                        }
                        //_aPara.PowerUnit
                    }
                    break;
            }

            CheckLimit();
        }

        private void txtSetPower_TextChanged(object sender, EventArgs e)
        {
            SetAPowMode2Data(_aPara.APMode.ToString());
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            DataCenter._product.LaserSrcSetting.AttenuatorData = _aPara.Clone() as AttenuatorSettingData;

            DataCenter.SaveProductFile();

            if (this.TestItemDataChangeEvent != null)
            {
                this.TestItemDataChangeEvent(new object(), new EventArgs());
            }
        }



    }
}
