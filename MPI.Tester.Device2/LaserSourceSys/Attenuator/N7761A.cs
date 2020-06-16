using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Data.LaserData.LaserSource;



namespace MPI.Tester.Device.LaserSourceSys.Attenuator
{
    class N7761A:AttenuatorBase , IAttenuator
    {
        public N7761A()
            : base()
        {
            Spec.HavePowerControlMode = true;
        }
        public N7761A(AttenuatorConfig cfg):this()
        {
            Address = cfg.Address;
            Slot = cfg.Slot;
            SysDevChDic = new Dictionary<int, int>();
            SysDevChDic.Add(cfg.LaserSysChannel, cfg.AttChannel);
            
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
            this._serialNumber = data[2];

            this._softwareVersion = "NA";

            this._hardwareVersion = data[3];

            _device.SendCommand("syst:vers?");            

            this._device.SendCommand("*CLS");
            //this._device.SendCommand("*RST");
            _device.WaitAndGetData(out msg);

            this._softwareVersion = msg;

            return true;
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
            bool retult=  false;
            if (paraList != null && paraList.Count > 0)
            {
                AttenuatorSettingData para = paraList[0];

                retult = ForceSetPara(para);

                //_attSetting = para.Clone() as AttenuatorSettingData;
            }

            //sens1:pow:atim 1s
            return retult; 
        }

        public bool ForceSetPara(AttenuatorSettingData para)
        {
            string msg = "";

            this._device.SendCommand("OUTP1:APOW " + (para.APowerOn ? "ON" : "OFF"));//shutter on/off after power on/off

            this._device.SendCommand("INP1:WAV " + para.WaveLength.ToString("0.0000") + "E-009");//nm

            this._device.SendCommand("INP1:ATT:SPE " + para.Speed.ToString("0.000"));

            this._device.SendCommand("OUTP1:ATIM " + para.AvgTime.ToString("0.000") + "s");

            this._device.SendCommand("OUTP1:POW:UN " + (int)para.PowerUnit);


            this._device.SendCommand("OUTP1:STAT " + (para.Output ? "ON" : "OFF"));//shutter on/off

            this._device.SendCommand("OUTP1:POW:CONTR " + (para.PowerContorll ? "ON" : "OFF"));//power controll on/off

            this._device.SendCommand("OUTP1:APMode " + (para.APMode == EAPMode.Attenuator ? "0" : "1"));


            if (para.APMode == EAPMode.Attenuator)
            {
                this._device.SendCommand("INP1:OFFS " + (para.Attenuate.Offset).ToString() + "dB");

                this._device.SendCommand("INP1:ATT " + (para.Attenuate.Set).ToString() + "dB");
            }
            else if (para.APMode == EAPMode.Pwoer)
            {
                this._device.SendCommand("OUTP1:POW:OFFS " + (para.Power.Offset).ToString() + "dB");

                this._device.SendCommand("OUTP1:POW " + (para.Power.Set).ToString());
            }


            this._device.SendCommand("syst:err?");

            _device.WaitAndGetData(out msg);

            string msg1 = msg.TrimEnd('\n').Split(',')[1].Trim('\"');
            if (msg1 != "No error")
            {
                _errorCode = EDevErrorNumber.Device_NO_Error;
                return true;
            }

            Console.WriteLine("[N7761A], ForceSetPara Err: " + msg);
            _errorCode = EDevErrorNumber.LASER_Attenuator_ParaSet_Err;

            return true;
        }


        public override double GetMsrtPower(int sysCh = 0)
        {
            string msg = "";

            this._device.SendCommand("read1:pow?");

            _device.WaitAndGetData(out msg);

            string[] data = msg.TrimEnd().Split(',');

            double val = 0;
            if (data != null && data.Length > 0 &&
                double.TryParse(data[0], out val))

            {
                return val;
            }            

            return 0;
        }
    }
}
