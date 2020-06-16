using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Data.LaserData.LaserSource;



namespace MPI.Tester.Device.LaserSourceSys.Attenuator
{
    class FTBx_3500 : AttenuatorBase, IAttenuator
    {

        protected string preString = "";
        double _LastAttSet = 0;
        public FTBx_3500()
            : base()
        {
            Spec.HavePowerControlMode = false;
        }
        public FTBx_3500(AttenuatorConfig cfg)
              : this()
        {
            Address = cfg.Address;
            Slot = cfg.Slot;
            SysDevChDic = new Dictionary<int, int>();
            SysDevChDic.Add(cfg.LaserSysChannel, cfg.AttChannel);
            preString = "LINS" + Slot.ToString();

        }

        public override bool Init( string address, IConnect connect)
        {
            string msg = "";
            base.Init(address, connect);
            if (!base.Init(address, connect))
            {
                this._errorCode = EDevErrorNumber.LASER_Attenuator_Init_Err;
                return false;
            }
            
            if (!TestIfSlotExist())
            {
                this._errorCode = EDevErrorNumber.LASER_Attenuator_Slot_Not_Exist_Err;
                return false;
            }


            _device.SendCommand(preString + ":SNUM?");

            _device.WaitAndGetData(out msg);

            string[] data = msg.Trim(new char[] { '\n', '\"' }).Split(',');

            if (data == null || data.Length != 1)
            {
                this._errorCode = EDevErrorNumber.LASER_Attenuator_Init_Err;
                return false;
            }

            this._serialNumber = data[0];

            #region>>spec<<
            Spec.HavePowerControlMode = false;


            msg = preString + ":CONT:MODE:CAT?";
            _device.SendCommand(msg);
            _device.WaitAndGetData(out msg);

            string str = msg.ToUpper();
            Spec.HavePowerControlMode = (str.Contains("POWER"));


            double min = 1E-9, max = 70;
            // _device.SendCommand(preString+":SNUM?");
            _device.SendCommand(preString + ":CONT:MODE ATT");
            msg = preString + ":INP:ATT? MIN";
            QueryValue(ref msg, ref min);
            msg = preString + ":INP:ATT? MAX";
            QueryValue(ref msg, ref max);
            Spec.AttenuationRange = new MinMaxValuePair<double>(min, max);

            min = -70;
            max = -10;
            if (Spec.HavePowerControlMode)
            {
                msg = preString + ":OUTP:POW? MIN";
                QueryValue(ref msg, ref min);
                msg = preString + ":OUTP:POW? MAX";
                QueryValue(ref msg, ref max);
            }
            Spec.PowerRange = new MinMaxValuePair<double>(min, max);

            min = 700;
            max = 1350;
            msg = preString + ":INP:WAV? MIN";
            QueryValue(ref msg, ref min);
            msg = preString + ":INP:WAV? MAX";
            QueryValue(ref msg, ref max);
            Spec.WavelengthRange = new MinMaxValuePair<double>(min * 1E9, max * 1E9);
            #endregion

            this._device.SendCommand(preString + ":OUTP:STAT ON");//shutter on/off

            return true;
        }
            
        public override void TurnOff()
        {
            this._device.SendCommand(preString + ":OUTP:STAT OFF");//shutter on/off
        }


        public override void Close()
        {
            if (_device != null)
            {
                TurnOff();
                //this._device.Close();
            }
        }

        public override bool SetParamToAttenuator(List<AttenuatorSettingData> paraList)
        {
            base.SetParamToAttenuator(paraList);
            bool retult=  true;
            if (paraList != null && paraList.Count > 0)
            {
                foreach (var para in paraList)
                {
                    if (SysDevChDic.ContainsKey(para.SysChannel))
                    {
                        //preString
                        string str = preString;

                        str += ":OUTP:STAT ";
                        str += para.APowerOn ? "ON" : "OFF";
                        this._device.SendCommand(str);

                        this._device.SendCommand(preString + ":INP:WAV " + para.WaveLength.ToString("0") + "NM");//nm

                        this._device.SendCommand(preString + ":OUTP:APM ABS");
                        if (para.APMode == EAPMode.Attenuator)
                        {
                            this._device.SendCommand(preString + ":CONT:MODE ATTENUATION");
                            this._device.SendCommand(preString + ":INP:ATT " + para.Attenuate.Set.ToString("0.00") );

                            double attDelay = 0;
                            // spec : 1dB need 160ms,10dB 515ms
                            //from user_guide_ftbx-3500_english_-1071151

                            if (Math.Abs(_LastAttSet - para.Attenuate.Set) > 0)
                            {
                                attDelay = 120 + 40 * Math.Abs(_LastAttSet - para.Attenuate.Set);
                                System.Threading.Thread.Sleep((int)attDelay);
                            }

                            _LastAttSet = para.Attenuate.Set;
                            string msg = preString + ":INP:ATT?";
                            _device.SendCommand(msg);
                            _device.WaitAndGetData(out msg);

                        }
                        else
                        {
                            this._device.SendCommand(preString + ":CONT:MODE POW");

                            double outVal = para.Power.Set;
                

                            this._device.SendCommand(preString + ":OUTP:POW " + para.Power.Set.ToString("0.00#####"));
                            if (para.PowerContorll)
                            {
                                this._device.SendCommand(preString + ":OUTP:ALC:STAT 1");
                                this._device.SendCommand(preString + ":OUTP:DTO DEF");
                            }
                            else { this._device.SendCommand(preString + ":OUTP:ALC:STAT 0"); }
                            System.Threading.Thread.Sleep(515);
 
                        }        
                    }
                }
            }

            //sens1:pow:atim 1s
            return retult; 
        }

        public override double GetMsrtPower(int sysCh = 1, ELaserPowerUnit unit = ELaserPowerUnit.W)
        {
            string msg = "";
            if (Spec.HavePowerControlMode)
            {
                this._device.SendCommand(preString + ":READ:POW:DC?");

                _device.WaitAndGetData(out msg);

                string[] data = msg.TrimEnd().Split(',');

                double val = 0;
                if (data != null && data.Length > 0 &&
                    double.TryParse(data[0], out val))
                {
                    if (unit == ELaserPowerUnit.W)
                    {
                        val = MPI.Tester.Maths.UnitMath.dBm2W(val);
                        //val = MPI.Tester.Maths.UnitMath.W2dBm(val);
                    }

                    return val;
                }
            }
            return 0;
        }

        public override MinMaxValuePair<double> GetOutputPowerRangeIndBm(int ch)
        {
            double min = -70;
            double max = -10;
            string msg = "";
            if (Spec.HavePowerControlMode)
            {
                msg = preString + ":OUTP:POW? MIN";
                QueryValue(ref msg, ref min);
                msg = preString + ":OUTP:POW? MAX";
                QueryValue(ref msg, ref max);
            }
            MinMaxValuePair<double> range = new MinMaxValuePair<double>(min, max);

            return range;
        }

        #region >>private method<<
        private bool TestIfSlotExist()
        {
            string msg = "";
            _device.SendCommand("INST:CAT:FULL?");//編號會按照LINS設定

            _device.WaitAndGetData(out msg);

            bool isSlotExist = false;

            if (msg != null && msg != "")
            {
                string[] sArr = msg.Split(',');
                foreach (string str1 in sArr)
                {
                    if (str1.Trim() == Slot.ToString())
                    {
                        isSlotExist = true;
                        break;
                    }
                }
            }
            return isSlotExist;
        }

        #endregion
    }

}
