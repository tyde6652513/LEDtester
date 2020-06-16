using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Windows.Forms;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Data;

namespace MPI.Tester.Device.LCRMeter
{
    public class HP4278A : LCRBase 
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
        private const int LOAD_DATA_QTY =1;

        private CmdData1 _cmdData1;

        public HP4278A()
        {
            this._devSetting = new LCRDevSetting();

            this._lcrSetting = null;

            this._msrtResult = null;

            this._errorCode = EDevErrorNumber.Device_NO_Error;

            this._serialNumber = string.Empty;

            this._softwareVersion = string.Empty;

            this._hardwareVersion = string.Empty;

            //this._cmdData = new CmdData();

            this._caliData = new LCRCaliData(1);

        }

        public HP4278A(LCRDevSetting Setting)
            : this()
        {
            this._devSetting = Setting;
        }

        /*public string SerialNumber
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
        public override bool SetCaliData(LCRCaliData cData)
        {
            _caliData = cData.Clone() as LCRCaliData;

            int dataNum = _caliData.NowDataNum;

            string type = SetCaliType(_caliData.TestType);

            string CableLength = SetCableLength(_caliData.CableLength);

            SetEnableTest(_caliData.EnableOpen, _caliData.EnableShort, _caliData.EnableLoad);

            SetLoadData(0);


            return true;
        }

        public override bool LCRCali(ELCRCaliMode caliMode)
        {
            string singleLevel = "OSC=" + _caliData.Level.ToString("0.0");

            this._cmdData1.SingleLevel = singleLevel;

            this._device.SendCommand(singleLevel);

            switch (caliMode)
            {
                case ELCRCaliMode.Open:
                    this._device.SendCommand("XOP");
                    break;
                case ELCRCaliMode.Short:
                    this._device.SendCommand("XSH");
                    break;
                case ELCRCaliMode.Load:
                    this._device.SendCommand("XSTD");
                    break;
            }

            TurnOff();
            return true;
        }

        private  void SetDeviceSpec(string sn)
        {
            this._spec = new LCRMeterSpec();

            this._spec.IsProvideSignalLevelV = true;

            this._spec.IsProvideSignalLevelI = false;

            this._spec.IsProvideDCBiasV = false;

            this._spec.IsProvideDCBiasI = false;

            this._spec.SignalLevelVMin = 0.1;

            this._spec.SignalLevelVMax = 1;

            this._spec.SignalLevelIMin = 0;

            this._spec.SignalLevelIMax = 0;

            this._spec.FrequencyMin = 1e3;//kHz

            this._spec.FrequencyMax = 1e6;//kHz

            this._spec.DCBiasVMin = 0;

            this._spec.DCBiasVMax = 0;

            this._spec.DCBiasIMin = 0;

            this._spec.DCBiasIMax = 0;

            this._spec.MsrtSpeedList.Add(ELCRMsrtSpeed.Long);

            this._spec.MsrtSpeedList.Add(ELCRMsrtSpeed.Medium);

            this._spec.MsrtSpeedList.Add(ELCRMsrtSpeed.Short);

            this._spec.TestTypeList.Add(ELCRTestType.CPD);

            this._spec.TestTypeList.Add(ELCRTestType.CPQ);

            this._spec.TestTypeList.Add(ELCRTestType.CPG);

            this._spec.TestTypeList.Add(ELCRTestType.CSD);

            this._spec.TestTypeList.Add(ELCRTestType.CSQ);

            this._spec.TestTypeList.Add(ELCRTestType.CSRS);


            this._spec.CaliTypeList.Add(ELCRTestType.CPD);

            this._spec.CaliTypeList.Add(ELCRTestType.CPG);

            this._spec.CaliTypeList.Add(ELCRTestType.CSD);



            this._spec.FreqList.AddRange((new int[] { 1000, 1000000 }));

            this._spec.CableLenList.AddRange((new string[] { "0m",  "1m", "2m" }));

            this._spec.CaliDataQty = 1;

        }

        /*private override bool ParseMsrtData(string msg, uint index)
        {
            string[] data = msg.Split(',');

            double valueA = 0.0d;

            double valueB = 0.0d;

            for (int i = 0; i < this._msrtResult[index].Length; i++)
            {
                this._msrtResult[index][i] = 0.0d;
            }

            if (double.TryParse(data[0], out valueA) && double.TryParse(data[1], out valueB))
            {
                switch (this._lcrSetting[index].LCRMsrtType)
                {
                    case ELCRTestType.CPD:
                        this._msrtResult[index][(int)ELCRMsrtType.LCRCP] = valueA;
                        this._msrtResult[index][(int)ELCRMsrtType.LCRD] = valueB;
                        break;
                    case ELCRTestType.CPG:
                        this._msrtResult[index][(int)ELCRMsrtType.LCRCP] = valueA;
                        this._msrtResult[index][(int)ELCRMsrtType.LCRG] = valueB;
                        break;
                    case ELCRTestType.CPQ:
                        this._msrtResult[index][(int)ELCRMsrtType.LCRCP] = valueA;
                        this._msrtResult[index][(int)ELCRMsrtType.LCRQ] = valueB;
                        break;
                    case ELCRTestType.CSD:
                        this._msrtResult[index][(int)ELCRMsrtType.LCRCS] = valueA;
                        this._msrtResult[index][(int)ELCRMsrtType.LCRD] = valueB;
                        break;
                    case ELCRTestType.CSQ:
                        this._msrtResult[index][(int)ELCRMsrtType.LCRCS] = valueA;
                        this._msrtResult[index][(int)ELCRMsrtType.LCRQ] = valueB;
                        break;
                    case ELCRTestType.CSRS:
                        this._msrtResult[index][(int)ELCRMsrtType.LCRCS] = valueA;
                        this._msrtResult[index][(int)ELCRMsrtType.LCRRS] = valueB;
                        break;
                    default:
                        break;
                }

                return true;
            }
            else
            {
                return false;
            }
        }*/

        public override bool Init(int deviceNum, string sourceMeterSN)
        {
            this.SetDeviceSpec(this._serialNumber);

            //"GPIB0::0x0957::0x0909::MY46413167::0::INSTR";

            this._device = new IVIConnect(sourceMeterSN);

            string msg = string.Empty;

            if (!this._device.Open(out msg))
            {
                this._errorCode = EDevErrorNumber.LCRInitFail;

                return false;
            }

            this._device.SendCommand("*IDN?");

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

            //this._device.SendCommand("*RST;*CLS");

            //this._device.SendCommand("KEY LOCK UNLOCK");
            //MessageBox.Show("確認點1");
            this._device.SendCommand("TRIG2");
            //MessageBox.Show("確認點2");
            this._device.SendCommand("OSC=0.1");



            return true;
        }

        public override void TurnOff()
        {
            string singleLevel = "OSC=0.0";

            this._cmdData1.SingleLevel = singleLevel;

            this._device.SendCommand(singleLevel);
        }

        //public bool SetConfigToMeter(LCRDevSetting devSetting)
        //{
        //    this._devSetting = devSetting;

        //    return true;
        //}

        public override bool PreSettingParamToMeter(uint settingIndex)
        {
            if (this._lcrSetting == null || this._lcrSetting.Length == 0)
            {
                return false;
            }

            LCRSettingData data = this._lcrSetting[settingIndex];

            System.Threading.Thread.Sleep((int)data.MsrtDelayTime);

            string frequency = string.Empty;
            string highAcc = string.Empty;


            if (data.Frequency <= 1000)
            {
                // 1K Hz
                highAcc = "HIAC0";
                frequency = "FREQ1";
                //MessageBox.Show("1KHztest");
            }
            else
            {
                // 1M Hz

                frequency = "FREQ2";
                if (data.HighAcc)
            {
                    highAcc = "HIAC1";
            }
                else
            {
                    highAcc = "HIAC0";
            }
                //MessageBox.Show("1MHztest");
            }



            string singleLevel = "OSC="+ data.SignalLevelV.ToString("0.0");

            string msrtType = GetMsrtType(data.LCRMsrtType);

            string msrtSpeed = GetMsrtSpeed(data.MsrtSpeed);

            string msrtRange = GetRangeStr(data, ref highAcc);

            if (this._cmdData1.Frequency != frequency)
            {
                this._cmdData1.Frequency = frequency;

                this._device.SendCommand(frequency);
            }

            if (this._cmdData1.SingleLevel != singleLevel)
            {
                this._cmdData1.SingleLevel = singleLevel;

                this._device.SendCommand(singleLevel);
            }

            if (this._cmdData1.TestType != msrtType)
            {
                this._cmdData1.TestType = msrtType;

                this._device.SendCommand(msrtType);
            }

            if (this._cmdData1.Speed != msrtSpeed)
            {
                this._cmdData1.Speed = msrtSpeed;

                this._device.SendCommand(msrtSpeed);
            }

            if (this._cmdData1.Range != msrtRange)
            {
                this._cmdData1.Range = msrtRange;

                this._device.SendCommand(msrtRange);
            }
            if (this._cmdData1.HighAcc != highAcc)
            {
                this._cmdData1.HighAcc = highAcc;

                this._device.SendCommand(highAcc);
            }

            ////////////////////////////////////////////////////////////////////////

            return true;
        }

        /*public bool SetParamToMeter(LCRSettingData[] paramSetting)
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
            this._device.SendCommand("*TRG");

            //20170312 david
            switch (this._cmdData1.Speed)
            {
                case ("ITIM1"):
            System.Threading.Thread.Sleep(30);
                    break;
                case ("ITIM2"):
                    System.Threading.Thread.Sleep(40);
                    break;
                case ("ITIM3"):
                default:
                    System.Threading.Thread.Sleep(75);
                    break;                    
            }
            

            this._device.SendCommand("DATA?");

            string msg = string.Empty;

            this._device.WaitAndGetData(out msg);

            this.ParseMsrtData(msg, settingIndex);

            return true;
        }

        //public double[] GetDataFromMeter(uint channel, uint settingIndex)
        //{
        //    return this._msrtResult[settingIndex];
        //}

        public void Close()
        {
            if (this._device != null)
            {
                this.TurnOff();

                this._device.Close();
            }
        }


        #region >>>private method<<<


        private string SetCaliType(ELCRTestType type)
        {
            string theType = string.Empty;

            if (type.ToString().EndsWith("D"))
            {
                theType = "SPAR1";
            }
            else
            {
                theType = "SPAR2";
            }

            this._device.SendCommand(theType);

            return theType;
        }


        private string GetMsrtType(ELCRTestType type)
        {
            string msrtType = string.Empty;

            if (type == ELCRTestType.CPD)
            {
                msrtType = "MPAR1";
            }
            else if (type == ELCRTestType.CPQ)
            {
                msrtType = "MPAR2";
            }
            else if (type == ELCRTestType.CPG)
            {
                msrtType = "MPAR3";
            }
            else if (type == ELCRTestType.CSD)
            {
                msrtType = "MPAR4";
            }
            else if (type == ELCRTestType.CSQ)
            {
                msrtType = "MPAR5";
            }
            else if (type == ELCRTestType.CSRS)
            {
                msrtType = "MPAR6";
            }

            return msrtType;

        }

        private string GetRangeStr(LCRSettingData data, ref string highAcc)
        {
            String msrtRange = "";

            if (data.Frequency <= 1000)
            {
                if (data.Range == 0)
                {
                    msrtRange = "RA0";
                }
                else if (data.Range <= 100)
                {
                    msrtRange = "RA1";
                }
                else if (data.Range <= 1e3)
                {
                    msrtRange = "RA2";
                }
                else if (data.Range <= 10e3)
                {
                    msrtRange = "RA3";
                }
                else if (data.Range <= 100e3)
                {
                    msrtRange = "RA4";
                }
                else if (data.Range <= 1e6)
                {
                    msrtRange = "RA5";
                }
                else if (data.Range <= 10e6)
                {
                    msrtRange = "RA6";
                }
                else if (data.Range <= 100e6)
                {
                    msrtRange = "RA7";
                }
                else
                {
                    msrtRange = "RA0";
                }
            }
            else
            {
                if (highAcc == "HIAC0")
                {
                    if (data.Range == 0)
                    {
                        msrtRange = "RB0";
                    }
                    else if (data.Range <= 1)
                    {
                        msrtRange = "RB1";
                    }
                    else if (data.Range <= 2)
                    {
                        msrtRange = "RB2";
                    }
                    else if (data.Range <= 4)
                    {
                        msrtRange = "RB3";
                    }
                    else if (data.Range <= 8)
                    {
                        msrtRange = "RB4";
                    }
                    else if (data.Range <= 16)
                    {
                        msrtRange = "RB5";
                    }
                    else if (data.Range <= 32)
                    {
                        msrtRange = "RB6";
                    }
                    else if (data.Range <= 642)
                    {
                        msrtRange = "RB7";
                    }
                    else if (data.Range <= 128)
                    {
                        msrtRange = "RB8";
                    }
                    else if (data.Range <= 256)
                    {
                        msrtRange = "RB9";
                    }
                    else if (data.Range <= 512)
                    {
                        msrtRange = "RB10";
                    }
                    else if (data.Range <= 1024)
                    {
                        msrtRange = "RB11";
                    }
                    else
                    {
                        msrtRange = "RB0";
                    }
                }
                else
                {
                    int theRange = 0;
                    for (int power = 0; power < 12; power++)//手冊上說最大到2^11 pF = 2048 pF
                    {
                        theRange = (int)Math.Pow(2, power);
                        if (data.Range < (double)theRange)
                        {
                            msrtRange = "RC=" + theRange.ToString() + "E-12";
                            break;
                        }
                        else
                        {

                            theRange = 0;
                        }
                    }

                    if (theRange == 0)//保護機制，以免超過2^11直接掛掉，超過2048直接解除HIAC0改1K模式
                    {
                       // MessageBox.Show("超過2048uF，自動降為1K模式");
                        data.Frequency = 1000;
                        highAcc = "HIAC0";
                        msrtRange = "RA0";
                    }

                }

            }

            return msrtRange;
        }

        private string GetMsrtSpeed(ELCRMsrtSpeed speed)
        {
            string msrtSpeed = string.Empty;

            switch (speed)
            {
                case ELCRMsrtSpeed.Long:
                    {
                        msrtSpeed = "ITIM3";

                        break;
                    }
                case ELCRMsrtSpeed.Medium:
                    {
                        msrtSpeed = "ITIM2";

                        break;
                    }
                case ELCRMsrtSpeed.Short:
                    {
                        msrtSpeed = "ITIM1";

                        break;
                    }
                default:
                    {
                        msrtSpeed = "ITIM3";

                        break;
                    }
            }
            return msrtSpeed;
        }

        private string  SetCableLength(string cableLen)
        {
            string cable = string.Empty;

            switch (cableLen)
            {
                case "0m":
                    cable = "CABL0";
                    break;
                case "1m":
                    cable = "CABL1";
                    break;
                case "2m":
                    cable = "CABL2";
                    break; 
            }

            this._device.SendCommand(cable);

            return cable;

        }



        private void SetEnableTest(bool openT = false, bool shortT = false, bool loadT = false)
        {
            string oStr = openT ? "OPEN1" : "OPEN0";
            string sStr = shortT ? "SHOR1" : "SHOR0";
            string lStr = loadT ? "STD1" : "STD1";

            this._device.SendCommand(oStr);
            this._device.SendCommand(sStr);
            this._device.SendCommand(lStr);
        }

        private void SetLoadData(int dataNum)
        {
            LoadingRefData lData = _caliData.LoadingList[dataNum];

            string frequency = lData.Freq <= 1000 ? "FREQ1" : "FREQ2";

            if (this._cmdData1.Frequency != frequency)
            {
                this._cmdData1.Frequency = frequency;

                this._device.SendCommand(frequency);
            }

            string paraA = "CSTD=";

            paraA += (lData.RefA * UnitToPower(ECapUnit.F, lData.RefUnit)).ToString("0.0###E-0");

            this._device.SendCommand(paraA);

            string paraB = "DSTD=";

            paraB +=lData.RefB.ToString("0.0###");

            this._device.SendCommand(paraB);
            
        }


        private double UnitToPower(ECapUnit unitA, ECapUnit unitB)//10^(B-A)
        {
            double num = 10;//10為底數
            int powTimes = 1;

            powTimes = (int)unitB-(int)unitA  ;
            num = Math.Pow(10, powTimes);
            //num = //Math.Pow(Enum.GetValues(unitA)

            return num;
        }

        #endregion

        private class CmdData1:CmdData
        {
            //public string Frequency { get; set; }
            //public string SignaleLevel { get; set; }
            //public string Bias { get; set; }
            //public string TestType { get; set; }
            //public string Speed { get; set; }
            //public string Range { get; set; }
            //201703008 david
            public string HighAcc { get; set; }
        }
    }
}