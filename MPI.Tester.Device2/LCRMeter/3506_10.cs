using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Data;

namespace MPI.Tester.Device.LCRMeter
{
    public class _3506_10 : LCRBase 
    {
        //private IConnect _device;
        //private LCRDevSetting _devSetting;
        //private LCRSettingData[] _lcrSetting;
        //private EDevErrorNumber _errorCode;
        //private double[][] _msrtResult;
        //private string _serialNumber;
        //private string _softwareVersion;
        //private string _hardwareVersion;
        //private LCRMeterSpec _spec;
        //private LCRCaliData _caliData;
        private CmdData1 _cmdData;

        public _3506_10()
        {
            this._devSetting = new LCRDevSetting();

            this._lcrSetting = null;

            this._msrtResult = null;

            this._errorCode = EDevErrorNumber.Device_NO_Error;

            this._serialNumber = string.Empty;

            this._softwareVersion = string.Empty;

            this._hardwareVersion = string.Empty;

            this._cmdData = new CmdData1();
        }

        public _3506_10(LCRDevSetting Setting)
            : this()
        {
            this._devSetting = Setting;
        }
        /*
        public  string SerialNumber
        {
            get { return this._serialNumber; }
        }

        public string SoftwareVersion
        {
            get { return this._softwareVersion; }
        }

        public string HardwareVersion
        {
            get { return this._hardwareVersion; }
        }

        public LCRSettingData[] LCRSetting
        {
            get { return this._lcrSetting; }
        }

        public EDevErrorNumber ErrorNumber
        {
            get { return this._errorCode; }
        }

        public LCRMeterSpec Spec
        {
            get { return this._spec; }
        }
        public LCRCaliData CaliData
        {
            get { return this._caliData; }
        }*/
        //public bool SetCaliData(LCRCaliData caliData)
        //{
        //    return true;
        //}
        //public bool LCRCali(ELCRCaliMode caliMode)
        //{
        //    return true;
        //}

        private void SetDeviceSpec(string sn)
        {
            this._spec = new LCRMeterSpec();

            this._spec.IsProvideSignalLevelV = true;

            this._spec.IsProvideSignalLevelI = false;

            this._spec.IsProvideDCBiasV = false;

            this._spec.IsProvideDCBiasI = false;

            this._spec.SignalLevelVMin = 0.5;

            this._spec.SignalLevelVMax = 1;

            this._spec.FrequencyMin = 1e3;

            this._spec.FrequencyMax = 1e6;

            this._spec.MsrtSpeedList.Add(ELCRMsrtSpeed.Long);

            this._spec.MsrtSpeedList.Add(ELCRMsrtSpeed.Medium);

            this._spec.MsrtSpeedList.Add(ELCRMsrtSpeed.Short);

            this._spec.TestTypeList.Add(ELCRTestType.CPD);

            this._spec.TestTypeList.Add(ELCRTestType.CSD);

            this._spec.TestTypeList.Add(ELCRTestType.CPQ);

            this._spec.TestTypeList.Add(ELCRTestType.CSQ);
        }

        //private bool  ParseMsrtData(string msg, uint index)
        //{
        //    string[] data = msg.Split(',');

        //    double valueA = 0.0d;

        //    double valueB = 0.0d;

        //    for (int i = 0; i < this._msrtResult[index].Length; i++)
        //    {
        //        this._msrtResult[index][i] = 0.0d;
        //    }

        //    if (double.TryParse(data[0], out valueA) && double.TryParse(data[1], out valueB))
        //    {
        //        switch (this._lcrSetting[index].LCRMsrtType)
        //        {
        //            case ELCRTestType.CPD:
        //                this._msrtResult[index][(int)ELCRMsrtType.LCRCP] = valueA;
        //                this._msrtResult[index][(int)ELCRMsrtType.LCRD] = valueB;
        //                break;
        //            case ELCRTestType.CPQ:
        //                this._msrtResult[index][(int)ELCRMsrtType.LCRCP] = valueA;
        //                this._msrtResult[index][(int)ELCRMsrtType.LCRQ] = valueB;
        //                break;
        //            case ELCRTestType.CSD:
        //                this._msrtResult[index][(int)ELCRMsrtType.LCRCS] = valueA;
        //                this._msrtResult[index][(int)ELCRMsrtType.LCRD] = valueB;
        //                break;
        //            case ELCRTestType.CSQ:
        //                this._msrtResult[index][(int)ELCRMsrtType.LCRCS] = valueA;
        //                this._msrtResult[index][(int)ELCRMsrtType.LCRQ] = valueB;
        //                break;
        //            default:
        //                break;
        //        }

        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        private void SendCommand(string cmd)
        {
            this._device.SendCommand(cmd);
        }

        public override bool Init(int deviceNum, string sourceMeterSN)
        {
            this.SetDeviceSpec(this._serialNumber);

            RS232SettingData rs232Data = new RS232SettingData();

            rs232Data.ComPortName = sourceMeterSN;

            rs232Data.BaudRate = 9600;

            rs232Data.DataBits = 8;

            rs232Data.Parity = "None";

            rs232Data.StopBits = 1;

            rs232Data.TimeOut = 500;

            rs232Data.Terminator = "\n\r";

            string msg = "";

            this._device = new RS232Connect(rs232Data);

            if (!this._device.Open(out msg))
            {
                this._errorCode = EDevErrorNumber.SourceMeterDevice_Init_Err;

                return false;
            }

            this.SendCommand("*IDN?");

            this._device.WaitAndGetData(out msg);

            string[] data = msg.Replace("\n", "").Split(',');

            if (data.Length != 4)
            {
                this._errorCode = EDevErrorNumber.LCRInitFail;

                return false;
            }

            this._serialNumber = data[2];

            this._softwareVersion = "NA";

            this._hardwareVersion = data[3];

            this.SendCommand(":PRESet");

            this.SendCommand("*CLS");

            this.SendCommand(":TRIGger EXTernal");

            this.SendCommand(":AVERaging:STATe ON");

            this.SendCommand(":AVERaging 10");

            this.SendCommand(":CALibration:CABLe 1");

            this.SendCommand(":MEASure:VALid 20"); // 輸出 C + Para2

            return true;
        }

        public override void TurnOff()
        {

        }

        public override bool SetConfigToMeter(LCRDevSetting devSetting)
        {
            this._devSetting = devSetting;

            return true;
        }

        public override bool PreSettingParamToMeter(uint settingIndex)
        {
            if (this._lcrSetting == null || this._lcrSetting.Length == 0)
            {
                return false;
            }

            LCRSettingData data = this._lcrSetting[settingIndex];

            System.Threading.Thread.Sleep((int)data.MsrtDelayTime);

            string frequency = ":FREQuency " + (data.Frequency >= 1e6 ? 1e6 : 1e3);

            string singleLevel = ":LEVel " + (data.SignalLevelV >= 1 ? 1 : 0.5);

            string func1 = string.Empty;

            string func2 = string.Empty;

            string msrtSpeed = string.Empty;

            switch (data.LCRMsrtType)
            {
                case ELCRTestType.CPD:
                    func1 = ":PARAmeter D";
                    func2 = ":CIRCuit PARallel";
                    break;
                case ELCRTestType.CPQ:
                    func1 = ":PARAmeter Q";
                    func2 = ":CIRCuit PARallel";
                    break;
                case ELCRTestType.CSD:
                    func1 = ":PARAmeter D";
                    func2 = ":CIRCuit SERial";
                    break;
                case ELCRTestType.CSQ:
                    func1 = ":PARAmeter Q";
                    func2 = ":CIRCuit SERial";
                    break;
                default:
                    return false;
            }

            switch (data.MsrtSpeed)
            {
                case ELCRMsrtSpeed.Long:
                    msrtSpeed = ":SPEEd SLOW";
                    break;
                case ELCRMsrtSpeed.Medium:
                    msrtSpeed = ":SPEEd NORMal";
                    break;
                case ELCRMsrtSpeed.Short:
                    msrtSpeed = ":SPEEd FAST";
                    break;
                default:
                    msrtSpeed = ":SPEEd SLOW";
                    break;
            }

            double pF_Range = data.Range / 1e15;

            string msrtRange = string.Empty;

            if (data.Frequency < 1e6)
            {
                if (pF_Range == 0)
                {
                    msrtRange = ":RANGe:AUTO ON";
                }
                else if (pF_Range <= 100e-12)
                {
                    msrtRange = ":RANGE 9";
                }
                else if (pF_Range <= 220e-12)
                {
                    msrtRange = ":RANGE 10";
                }
                else if (pF_Range <= 470e-12)
                {
                    msrtRange = ":RANGE 11";
                }
                else if (pF_Range <= 1e-9)
                {
                    msrtRange = ":RANGE 12";
                }
                else if (pF_Range <= 2.2e-9)
                {
                    msrtRange = ":RANGE 13";
                }
                else if (pF_Range <= 4.7e-9)
                {
                    msrtRange = ":RANGE 14";
                }
                else if (pF_Range <= 10e-9)
                {
                    msrtRange = ":RANGE 15";
                }
                else if (pF_Range <= 22e-9)
                {
                    msrtRange = ":RANGE 16";
                }
                else if (pF_Range <= 47e-9)
                {
                    msrtRange = ":RANGE 17";
                }
                else if (pF_Range <= 100e-9)
                {
                    msrtRange = ":RANGE 18";
                }
                else if (pF_Range <= 220e-9)
                {
                    msrtRange = ":RANGE 19";
                }
                else if (pF_Range <= 470e-9)
                {
                    msrtRange = ":RANGE 20";
                }
                else if (pF_Range <= 1e-6)
                {
                    msrtRange = ":RANGE 21";
                }
                else if (pF_Range <= 2.2e-6)
                {
                    msrtRange = ":RANGE 22";
                }
                else if (pF_Range <= 4.7e-6)
                {
                    msrtRange = ":RANGE 23";
                }
                else
                {
                    msrtRange = ":RANGE 24";
                }
            }
            else
            {
                if (pF_Range == 0)
                {
                    msrtRange = ":RANGe:AUTO ON";
                }
                else if (pF_Range <= 220e-15)
                {
                    msrtRange = ":RANGE 1";
                }
                else if (pF_Range <= 470e-15)
                {
                    msrtRange = ":RANGE 2";
                }
                else if (pF_Range <= 1e-12)
                {
                    msrtRange = ":RANGE 3";
                }
                else if (pF_Range <= 2.2e-12)
                {
                    msrtRange = ":RANGE 4";
                }
                else if (pF_Range <= 4.7e-12)
                {
                    msrtRange = ":RANGE 5";
                }
                else if (pF_Range <= 10e-12)
                {
                    msrtRange = ":RANGE 6";
                }
                else if (pF_Range <= 22e-12)
                {
                    msrtRange = ":RANGE 7";
                }
                else if (pF_Range <= 47e-12)
                {
                    msrtRange = ":RANGE 8";
                }
                else if (pF_Range <= 100e-12)
                {
                    msrtRange = ":RANGE 9";
                }
                else if (pF_Range <= 220e-12)
                {
                    msrtRange = ":RANGE 10";
                }
                else if (pF_Range <= 470e-12)
                {
                    msrtRange = ":RANGE 11";
                }
                else
                {
                    msrtRange = ":RANGE 12";
                }
            }

            if (this._cmdData.Frequency != frequency)
            {
                this._cmdData.Frequency = frequency;

                this.SendCommand(frequency);
            }

            if (this._cmdData.SingleLevel != singleLevel)
            {
                this._cmdData.SingleLevel = singleLevel;

                this.SendCommand(singleLevel);
            }

            if (this._cmdData.TestType1 != func1)
            {
                this._cmdData.TestType1 = func1;

                this.SendCommand(func1);
            }

            if (this._cmdData.TestType2 != func2)
            {
                this._cmdData.TestType2 = func2;

                this.SendCommand(func2);
            }

            if (this._cmdData.Speed != msrtSpeed)
            {
                this._cmdData.Speed = msrtSpeed;

                this.SendCommand(msrtSpeed);
            }

            if (this._cmdData.Range != msrtRange)
            {
                this._cmdData.Range = msrtRange;

                this.SendCommand(msrtRange);
            }

            return true;
        }

        /*public override bool SetParamToMeter(LCRSettingData[] paramSetting)
        {
            this._errorCode = EDevErrorNumber.Device_NO_Error;

            if (paramSetting == null || paramSetting.Length == 0)
            {
                return true;
            }

            int msrtLen = Enum.GetNames(typeof(ELCRMsrtType)).Length;

            this._msrtResult = new double[paramSetting.Length][];

            for (int i = 0; i < paramSetting.Length; i++)
            {
                LCRSettingData item = paramSetting[i];

                if (item.SignalMode == ELCRSignalMode.Voltage)
                {
                    if (item.SignalLevelV < this._spec.SignalLevelVMin || item.SignalLevelV > this._spec.SignalLevelVMax)
                    {
                        this._lcrSetting = null;

                        this._msrtResult = null;

                        this._errorCode = EDevErrorNumber.LCRParameterSetting_Err;

                        return false;
                    }
                }
                else
                {
                    if (item.SignalLevelI < this._spec.SignalLevelIMin || item.SignalLevelI > this._spec.SignalLevelIMax)
                    {
                        this._lcrSetting = null;

                        this._msrtResult = null;

                        this._errorCode = EDevErrorNumber.LCRParameterSetting_Err;

                        return false;
                    }
                }

                if (this._devSetting.LCRDCBiasType == ELCRDCBiasType.Internal)
                {
                    if (item.DCBiasMode == ELCRDCBiasMode.Voltage)
                    {
                        if (item.DCBiasV < this._spec.DCBiasVMin || item.DCBiasV > this._spec.DCBiasVMax)
                        {
                            this._lcrSetting = null;

                            this._msrtResult = null;

                            this._errorCode = EDevErrorNumber.LCRParameterSetting_Err;

                            return false;
                        }
                    }
                    else
                    {
                        if (item.DCBiasI < this._spec.DCBiasIMin || item.DCBiasI > this._spec.DCBiasIMax)
                        {
                            this._lcrSetting = null;

                            this._msrtResult = null;

                            this._errorCode = EDevErrorNumber.LCRParameterSetting_Err;

                            return false;
                        }
                    }
                }

                if (item.Frequency < this._spec.FrequencyMin || item.Frequency > this._spec.FrequencyMax)
                {
                    this._lcrSetting = null;

                    this._msrtResult = null;

                    this._errorCode = EDevErrorNumber.LCRParameterSetting_Err;

                    return false;
                }

                this._msrtResult[i] = new double[msrtLen];
            }

            this._lcrSetting = paramSetting;

            return true;
        }*/

        public override bool MeterOutput(uint[] activateChannels, uint settingIndex)
        {
            this.SendCommand("*TRG");

            this.SendCommand(":MEASure?");

            string msg = string.Empty;

            this._device.WaitAndGetData(out msg);

            this.ParseMsrtData(msg, settingIndex);

            return true;
        }

        /*public override double[] GetDataFromMeter(uint channel, uint settingIndex)
        {
            return this._msrtResult[settingIndex];
        }*/

        public override void Close()
        {
            if (this._device != null)
            {
                this.TurnOff();

                this._device.Close();
            }
        }

        private class CmdData1 : CmdData
        {
            public string Frequency { get; set; }
            public string SingleLevel { get; set; }
            public string Bias { get; set; }
            public string TestType1 { get; set; }
            public string TestType2 { get; set; }
            public string Speed { get; set; }
            public string Range { get; set; }
        }
    }
}
