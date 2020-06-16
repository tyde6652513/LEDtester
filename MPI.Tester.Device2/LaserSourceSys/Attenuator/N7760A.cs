using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Data.LaserData.LaserSource;



namespace MPI.Tester.Device.LaserSourceSys.Attenuator
{
    class N7760A:AttenuatorBase , IAttenuator
    {
        
        public N7760A()
            : base()
        {
            Spec.HavePowerControlMode = true;
        }
        public N7760A(AttenuatorConfig cfg):this()
        {
            Address = cfg.Address;
            Slot = cfg.Slot;
            SysDevChDic = new Dictionary<int, int>();
            SysDevChDic.Add(cfg.LaserSysChannel, cfg.AttChannel);
            //devCh = cfg.AttChannel;
            
        }

        public override bool Init( string address, IConnect connect)
        {            
            string msg = "";
            base.Init( address,connect);
            if (this._errorCode == EDevErrorNumber.LASER_Attenuator_Init_Err)
            {
                return false;
            }

            _device.SendCommand("*IDN?");

            _device.WaitAndGetData(out msg);

            string[] data = msg.Replace("\n", "").Split(',');

            if (data.Length != 4)
            {
                this._errorCode = EDevErrorNumber.LASER_Attenuator_Init_Err;

                return false;
            }
            string type = data[1];

            switch (type.Trim())
            {
                default:
                case "N7761A":
                    { Spec.Channel = new MinMaxValuePair<int>(1, 1); }
                    break;
                case "N7762A":
                case "N7766A":
                    { Spec.Channel = new MinMaxValuePair<int>(1, 2); }
                    break;
                case "N7764A":
                case "N7768A":
                    { Spec.Channel = new MinMaxValuePair<int>(1, 4); }
                    break;
                    //
            }

            this._serialNumber = data[2];

            this._softwareVersion = "NA";

            this._hardwareVersion = data[3];

            _device.SendCommand("syst:vers?");            

            this._device.SendCommand("*CLS");
            //this._device.SendCommand("*RST");
            _device.WaitAndGetData(out msg);

            this._softwareVersion = msg;

            MinMaxValuePair<double> p = QueryRange("INP1:WAV?");
            Spec.WavelengthRange = new MinMaxValuePair<double>(p.Min * 1E9, p.Max * 1E9);
            Spec.HavePowerControlMode = true;
            Spec.PowerRange = QueryRange("INP1:WAV?");
            this._device.SendCommand("OUTP1:POW:UN DBM");
            Spec.PowerRange = QueryRange("OUTP1:POW?");
            Spec.TransitionSpeed = QueryRange("INP1:ATT:SPE?");
            return true;
        }

        private MinMaxValuePair<double> QueryRange(string str)
        {
            double min = 0;
            double max = 0;
            string msg = str+" MIN";
            QueryValue(ref msg, ref min);
            msg = str+" MAX";
            QueryValue(ref msg, ref max);
            return  new MinMaxValuePair<double>(min , max);
        }

        public override void TurnOff()
        {
            this._device.SendCommand("OUTP1:STAT OFF");//shutter on/off
        }


        public override void Close()
        {
            this._device.Close();
        }

        public override bool SetParamToAttenuator(List<AttenuatorSettingData> paraList)
        {
            bool retult= base.SetParamToAttenuator(paraList);
            if (paraList != null && paraList.Count > 0)
            {
                foreach (AttenuatorSettingData para in paraList)
                {
                    if (!ForceSetPara(para))
                    { return false; }
                }               
            }
            return retult; 
        }

        public bool ForceSetPara(AttenuatorSettingData para)
        {

            int sysCh = para.SysChannel;
            string chStr  = "1";
            if (SysDevChDic.ContainsKey(sysCh))
            {
                chStr = SysDevChDic[sysCh].ToString();
            }
            else { return false; }

            string msg = "";

            msg = "OUTP" + chStr+  ":APOW " + (para.APowerOn ? "ON" : "OFF");

            this._device.SendCommand(msg);//shutter on/off after power on/off

            msg = "INP" + chStr+  ":WAV " + para.WaveLength.ToString("0.0000") + "E-009";
            this._device.SendCommand(msg);//nm

            msg = "INP" + chStr+  ":ATT:SPE " + para.Speed.ToString("0.000");
            this._device.SendCommand(msg);

            msg = "OUTP" + chStr+  ":ATIM " + para.AvgTime.ToString("0.000") + "s";
            this._device.SendCommand(msg);
            msg = "OUTP" + chStr+  ":POW:UN " + (int)para.PowerUnit;
            this._device.SendCommand(msg);

            msg = "OUTP" + chStr+  ":STAT " + (para.Output ? "ON" : "OFF");
            this._device.SendCommand(msg);//shutter on/off
            msg = "OUTP" + chStr+  ":POW:CONTR " + (para.PowerContorll ? "ON" : "OFF");
            this._device.SendCommand(msg);//power controll on/off
            msg = "OUTP" + chStr+  ":APMode " + (para.APMode == EAPMode.Attenuator ? "0" : "1");
            this._device.SendCommand(msg);


            if (para.APMode == EAPMode.Attenuator)
            {
                msg = "INP" + chStr+  ":OFFS " + (para.Attenuate.Offset).ToString() + "dB";
                this._device.SendCommand(msg);
                msg = "INP" + chStr+  ":ATT " + (para.Attenuate.Set).ToString() + "dB";
                this._device.SendCommand(msg);
            }
            else if (para.APMode == EAPMode.Pwoer)
            {
                msg = "OUTP" + chStr+  ":POW:OFFS " + (para.Power.Offset).ToString() + "dB";
                this._device.SendCommand(msg);
                msg = "OUTP" + chStr+  ":POW " + (para.Power.Set).ToString();
                this._device.SendCommand(msg);
            }

            msg = "syst:err?";
            this._device.SendCommand(msg);

            _device.WaitAndGetData(out msg);

            string msg1 = msg.TrimEnd('\n').Split(',')[1].Trim('\"');
            if (msg1 == "No error")
            {
                _errorCode = EDevErrorNumber.Device_NO_Error;
                return true;
            }

            Console.WriteLine("[N7760A], ForceSetPara Err: " + msg);
            _errorCode = EDevErrorNumber.LASER_Attenuator_ParaSet_Err;

            return false;
        }


        public override double GetMsrtPower(int sysCh = 1, ELaserPowerUnit unit = ELaserPowerUnit.W)
        {
            string chStr = "1";
            if (SysDevChDic.ContainsKey(sysCh))
            {
                chStr = SysDevChDic[sysCh].ToString();
            }
            else { return -1; }

            string msg = "read" + chStr + ":pow?";

            this._device.SendCommand(msg);

            _device.WaitAndGetData(out msg);

            string[] data = msg.TrimEnd().Split(',');

            double val = 0;
            if (data != null && data.Length > 0 &&
                double.TryParse(data[0], out val))

            {
                if (_sysChSetDic[sysCh].PowerUnit == ELaserPowerUnit.W)
                {
                    val = Math.Abs(val);
                }
                if (_sysChSetDic[sysCh].PowerUnit != unit)
                {
                    if (unit == ELaserPowerUnit.dBm)
                    {
                        val = MPI.Tester.Maths.UnitMath.W2dBm(val);
                    }
                    else
                    {
                        val = MPI.Tester.Maths.UnitMath.dBm2W(val);
                    }
                }

                return val;
            }            

            return 0;
        }

        public override MinMaxValuePair<double> GetOutputPowerRangeIndBm(int ch)
        {
            return new MinMaxValuePair<double>(-70, 10);
        }
    }
}
