using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Windows.Forms;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Data;
using MPI.Tester.Maths;

namespace MPI.Tester.Device.LCRMeter
{
    public class AGILENT_4284A : LCRBase 
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
        //private CmdData _cmdData;

        public AGILENT_4284A()
        {
            //this._devSetting = new LCRDevSetting();

            //this._lcrSetting = null;

            //this._msrtResult = null;

            //this._errorCode = EDevErrorNumber.Device_NO_Error;

            //this._serialNumber = string.Empty;

            //this._softwareVersion = string.Empty;

            //this._hardwareVersion = string.Empty;

            //this._cmdData = new CmdData();
        }

        public AGILENT_4284A(LCRDevSetting Setting)
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

        public override bool SetCaliData(LCRCaliData caliData)
        {
            _caliData = caliData.Clone() as LCRCaliData;

            int dataNum = _caliData.NowDataNum;

            string type = SetCaliType(_caliData.TestType);

            string CableLength = SetCableLength(_caliData.CableLength);

            SetEnableTest(_caliData.EnableOpen, _caliData.EnableShort, _caliData.EnableLoad);

            SetLoadData(dataNum - 1);

            return true;
        }

        public override bool LCRCali(ELCRCaliMode caliMode)
        {

            bool isSetSingleFreq = _caliData.LoadingList[_caliData.NowDataNum - 1].Enable;

            if (this._devSetting.LCRDCBiasType == ELCRDCBiasType.Internal)
            {
                double biasVal = _caliData.Bias;

                string biasStr = ":BIAS:VOLT " + biasVal.ToString("0.0######");

                this._device.SendCommand(biasStr);

                this._cmdData.Bias = biasStr;
            }

            double voltVal = _caliData.Level;

            string voltStr = "VOLT " + voltVal.ToString("0.0######") + "V";

            this._device.SendCommand(voltStr);

            this._cmdData.SingleLevel = voltStr;

            switch (caliMode)
            {
                case ELCRCaliMode.Open:
                    if (isSetSingleFreq)
                    {
                        this._device.SendCommand("CORR:SPOT" + (_caliData.NowDataNum).ToString() + ":OPEN");

                        this._device.SendCommand("CORR:OPEN:STAT?");
                    }
                    else
                    {
                        this._device.SendCommand("CORR:OPEN");
                    }

                    break;
                case ELCRCaliMode.Short:

                    if (isSetSingleFreq)
                    {
                        this._device.SendCommand("CORR:SPOT" + (_caliData.NowDataNum).ToString() + ":SHOR");

                        this._device.SendCommand("CORR:SHOR:STAT?");
                    }
                    else
                    {
                        this._device.SendCommand("CORR:SHOR");
                    }
                    break;
                case ELCRCaliMode.Load:
                    this._device.SendCommand("CORR:SPOT" + (_caliData.NowDataNum).ToString() + ":LOAD");

                    this._device.SendCommand("CORR:LOAD:STAT?");
                    break;
            }

            string msg = string.Empty;
            this._device.WaitAndGetData(out msg);
            TurnOff();

            return true;
        }

        private void SetDeviceSpec(string sn)
        {
            this._spec = new LCRMeterSpec();

            //設定儀器工作條件
            this._spec.IsProvideSignalLevelV = true;

            this._spec.IsProvideSignalLevelI = false;

            this._spec.IsProvideDCBiasV = true;

            this._spec.IsProvideDCBiasI = false;

            this._spec.SignalLevelVMin = 5e-3;

            this._spec.SignalLevelVMax = 20;//預設HiPow為on

            this._spec.SignalLevelIMin = 50e-3;//目前單位是寫死為mA，先暫時用mA為單位

            this._spec.SignalLevelIMax = 200;//預設HiPow為on

            this._spec.FrequencyMin = 20;

            this._spec.FrequencyMax = 1e8;

            this._spec.DCBiasVMin = -40;

            this._spec.DCBiasVMax = 40;//預設HiPow為on

            this._spec.DCBiasIMin = -100;

            this._spec.DCBiasIMax = 100;//預設HiPow為on

            this._spec.MsrtSpeedList.Add(ELCRMsrtSpeed.Long);

            this._spec.MsrtSpeedList.Add(ELCRMsrtSpeed.Medium);

            this._spec.MsrtSpeedList.Add(ELCRMsrtSpeed.Short);

            this._spec.TestTypeList.Add(ELCRTestType.CPD);

            this._spec.TestTypeList.Add(ELCRTestType.CPQ);

            this._spec.TestTypeList.Add(ELCRTestType.CPG);

            this._spec.TestTypeList.Add(ELCRTestType.CPRP);

            this._spec.TestTypeList.Add(ELCRTestType.CSD);

            this._spec.TestTypeList.Add(ELCRTestType.CSQ);

            this._spec.TestTypeList.Add(ELCRTestType.CSRS);


            foreach (ELCRTestType lcrT in _spec.TestTypeList)
            {
                this._spec.CaliTypeList.Add(lcrT);
 
            }

            this._spec.FreqList.AddRange((new int[] { 1000, 1000000 }));

            this._spec.CableLenList.AddRange((new string[] { "0m", "1m", "2m" ,"4m"}));

            this._spec.CaliDataQty = 3;

        }

        /*private bool ParseMsrtData(string msg, uint index)
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
					case ELCRTestType.CPRP:
						this._msrtResult[index][(int)ELCRMsrtType.LCRCP] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRRP] = valueB;
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
					case ELCRTestType.LPD:
						this._msrtResult[index][(int)ELCRMsrtType.LCRLP] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRD] = valueB;
						break;
					case ELCRTestType.LPG:
						this._msrtResult[index][(int)ELCRMsrtType.LCRLP] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRG] = valueB;
						break;
					case ELCRTestType.LPQ:
						this._msrtResult[index][(int)ELCRMsrtType.LCRLP] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRQ] = valueB;
						break;
					case ELCRTestType.LPRD:
						this._msrtResult[index][(int)ELCRMsrtType.LCRLP] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRRDC] = valueB;
						break;
					case ELCRTestType.LPRP:
						this._msrtResult[index][(int)ELCRMsrtType.LCRLP] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRRP] = valueB;
						break;
					case ELCRTestType.LSD:
						this._msrtResult[index][(int)ELCRMsrtType.LCRLS] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRD] = valueB;
						break;
					case ELCRTestType.LSQ:
						this._msrtResult[index][(int)ELCRMsrtType.LCRLS] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRQ] = valueB;
						break;
					case ELCRTestType.LSRD:
						this._msrtResult[index][(int)ELCRMsrtType.LCRLS] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRRDC] = valueB;
						break;
					case ELCRTestType.LSRS:
						this._msrtResult[index][(int)ELCRMsrtType.LCRLS] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRRS] = valueB;
						break;
					case ELCRTestType.GB:
						this._msrtResult[index][(int)ELCRMsrtType.LCRG] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRB] = valueB;
						break;
					case ELCRTestType.RX:
						this._msrtResult[index][(int)ELCRMsrtType.LCRR] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRX] = valueB;
						break;
					case ELCRTestType.VDID:
						this._msrtResult[index][(int)ELCRMsrtType.LCRVDC] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRIDC] = valueB;
						break;
					case ELCRTestType.YTD:
						this._msrtResult[index][(int)ELCRMsrtType.LCRY] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRTD] = valueB;
						break;
					case ELCRTestType.YTR:
						this._msrtResult[index][(int)ELCRMsrtType.LCRY] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRTR] = valueB;
						break;
					case ELCRTestType.ZTD:
						this._msrtResult[index][(int)ELCRMsrtType.LCRZ] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRTD] = valueB;
						break;
					case ELCRTestType.ZTR:
						this._msrtResult[index][(int)ELCRMsrtType.LCRZ] = valueA;
						this._msrtResult[index][(int)ELCRMsrtType.LCRTR] = valueB;
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

            //this._device = new IVIConnect(sourceMeterSN);
            bool isOnlyIPAddressNum = false;

            string[] ip = sourceMeterSN.Split('.');

            if (ip.Length == 4)
            {
                isOnlyIPAddressNum = true;

                foreach (var item in ip)
                {
                    int num = 0;

                    isOnlyIPAddressNum &= int.TryParse(item, out num);
                }
            }

            if (isOnlyIPAddressNum)
            {
                LANSettingData lanData = new LANSettingData();

                lanData.IPAddress = sourceMeterSN;

                this._device = new LANConnect(lanData);
            }
            else
            {

                this._device = new IVIConnect(sourceMeterSN);
            }

            string msg = string.Empty;

            if (!this._device.Open(out msg))
            {
                this._errorCode = EDevErrorNumber.LCRInitFail;

                return false;
            }


            this._device.SendCommand("*RST;*CLS");

            System.Threading.Thread.Sleep(10);

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

            this._device.SendCommand("*RST;*CLS");
            //this._device.SendCommand("SYST:KLOCK ON");

            //this._device.SendCommand("MEM:DIM DBUF,1");
            this._device.SendCommand("TRIG:SOUR BUS");

            this._device.SendCommand("OUTP:HPOW ON");//強迫啟用 option 001

            this._device.SendCommand("BIAS:STAT ON");

            this._device.SendCommand("VOLT 0");
            //this._device.SendCommand("COMP OFF");
            //this._device.SendCommand("MEM:FILL DBUF");
            
            //this._device.SendCommand("INIT:CONT OFF");

            this._device.SendCommand("INIT:CONT ON");
            //this._device.SendCommand("ABORT;");


            this._device.SendCommand(":LIST:MODE SEQ");
            //this._device.SendCommand("ABORT;");

            //this._device.SendCommand("TRIG:SOUR BUS");

            this._serialNumber = data[2];

            this._softwareVersion = "NA";

            this._hardwareVersion = data[3];

            


            return true;
        }

        public override void TurnOff()
        {

            if (this._devSetting.LCRDCBiasType == ELCRDCBiasType.Internal)
            {
                this._device.SendCommand(":BIAS:VOLT 0.0");

                this._cmdData.Bias = ":BIAS:VOLT 0.0";
                
            }
            //if (this._devSetting.LCRDCBiasType == ELCRDCBiasType.Internal)
            //{
            //    this._device.SendCommand(":BIAS:VOLTage 0");

            //    this._cmdData.Bias = ":BIAS:VOLTage 0";
            //}

            //this._device.SendCommand(":LEVel:VOLTage 10e-3");

            //this._cmdData.SingleLevel = ":LEVel:VOLTage 10e-3";

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
            
            SetFreq( data);
            
            SetType(data);

            SetAC(data);

            SetDC(data);

            SetRange(data);

            SetMsrtSpeed(data);


            return true;
        }

        public override bool PreSetBiasListToMeter(uint settingIndex)
        {
            if (this._lcrSetting == null || this._lcrSetting.Length == 0)
            {
                return false;
            }

            LCRSettingData data = this._lcrSetting[settingIndex];

            System.Threading.Thread.Sleep((int)data.MsrtDelayTime);


            SetType(data);

            SetFreq(data);

            SetMsrtSpeed(data);

            SetRange(data);

            SetAC(data);

            SetBiasList(data);

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
        /*public override bool  MeterOutput(uint[] activateChannels, uint settingIndex)
        {
            this._device.SendCommand("*TRG");
            //this._device.SendCommand("A,B,C");
            //this._device.SendCommand("TRIGGER:IMMEDIATE");
            //this._device.SendCommand("FETCH?");
            //確認下是否要sleep
            //System.Threading.Thread.Sleep(500);
            //this._device.SendCommand("MEM:READ? DBUF");            
            string msg = string.Empty;
            this._device.WaitAndGetData(out msg);
            this._device.SendCommand("MEM:CLE DBUF");

            //this._device.SendCommand("MEM:READ? DBUF");
            //string msg1 = string.Empty;
            //this._device.WaitAndGetData(out msg1);
            this.ParseMsrtData(msg, settingIndex);

            return true;
        }*/

        //public double[] GetDataFromMeter(uint channel, uint settingIndex)
        //{
        //    return this._msrtResult[settingIndex];
        //}

        public override void Close()
        {
            if (this._device != null)
            {
                this.TurnOff();

                if (this._devSetting.LCRDCBiasType == ELCRDCBiasType.Internal)
                {
                    this._device.SendCommand(":BIAS:STATe OFF");
                }
                this._device.SendCommand("INITiate:CONTinuous OFF");

                this._device.SendCommand(":TRIGger:SOURce INT");


                //this._device.SendCommand("SYST:KLOCK OFF");

                this._device.SendCommand("CLS*;*RST");

                this._device.Close();
            }
        }

        private string GetMsrtType(ELCRTestType type)
        {
            string msrtType = string.Empty;

            msrtType = "FUNC:IMP " + type.ToString();

            return msrtType;

        }

        private string SetCableLength(string cableLen)
        {
            string cable = string.Empty;

            switch (cableLen)
            {
                case "0m":
                    cable = "CORR:LENG 0M";
                    break;
                case "1m":
                    cable = "CORR:LENG 1M";
                    break;
                case "2m":
                    cable = "CORR:LENG 2M";
                    break;
                case "4m":
                default:
                    cable = "CORR:LENG 4M";
                    break;
                

            }

            this._device.SendCommand(cable);

            return cable;

        }

        private void SetEnableTest(bool openT = false, bool shortT = false, bool loadT = false)//for spot test //指定頻率的測試
        {
            string oStr = "CORR:OPEN:STAT " + (openT ? "ON" : "OFF");
            string sStr = "CORR:SHOR:STAT " + (shortT ? "ON" : "OFF");
            string lStr = "CORR:LOAD:STAT " + (loadT ? "ON" : "OFF");

            this._device.SendCommand(oStr);
            this._device.SendCommand(sStr);
            this._device.SendCommand(lStr);
        }

        private void SetLoadData(int dataNum)
        {
            LoadingRefData lData = _caliData.LoadingList[dataNum];

            this._device.SendCommand("CORR:SPOT" + (dataNum + 1).ToString() + ":STAT " + (lData.Enable ? "ON" : "OFF"));

            string frequency = "CORR:SPOT" + (dataNum + 1).ToString() + ":FREQ " + (lData.Freq <= 1000000 ? lData.Freq.ToString("0") + "HZ" : "1MHZ");

            this._device.SendCommand(frequency);

            //717;"CORR:SPOT1:LOAD:STAN 100.7,0.0002"

            string paraA = (lData.RefA * MPI.Tester.Maths.UnitMath.UnitConvertFactor(lData.RefUnit, "F")).ToString("0.0###E-0");

            string paraB = lData.RefB.ToString("0.0####");

            this._device.SendCommand("CORR:SPOT" + (dataNum +1).ToString()+ ":LOAD:STAN " + paraA + "," + paraB);

        }

        private string SetCaliType(ELCRTestType type)
        {
            string theType = "CORR:LOAD:TYPE " + type.ToString();

            this._device.SendCommand(theType);

            return theType;
        }


        protected override void SetFreq(LCRSettingData data)
        {
            string frequency = string.Empty;
            if (data.Frequency <= this._spec.FrequencyMin)
            {
                frequency = "FREQ MIN";
            }
            else if (data.Frequency >= this._spec.FrequencyMax)
            {
                frequency = "FREQ MAX";
            }
            else
            {
                frequency = "FREQ " + data.Frequency.ToString();
            }


            if (this._cmdData.Frequency != frequency)
            {
                //this._device.SendCommand("ABORT;:INIT");

                this._cmdData.Frequency = frequency;

                this._device.SendCommand(frequency);

            }
        }

        protected override void SetType(LCRSettingData data)
        {
            string msrtType = "FUNC:IMP " + data.LCRMsrtType.ToString();

            if (this._cmdData.TestType != msrtType)
            {
                this._cmdData.TestType = msrtType;

                this._device.SendCommand(msrtType);
            }
        }

        protected override void SetAC(LCRSettingData data)
        {
            string singleLevel = string.Empty;

            if (data.SignalMode == ELCRSignalMode.Voltage)
            {
                if (data.SignalLevelV <= this._spec.SignalLevelVMin)
                {
                    singleLevel = "VOLT MIN";
                }
                else if (data.SignalLevelV > this._spec.SignalLevelVMax)
                {
                    singleLevel = "VOLT MAX";
                }
                else
                {
                    //if (data.SignalLevelV < 1)
                    //{
                    //    singleLevel = "VOLT " + (data.SignalLevelV * 1000).ToString("0") + "MV";
                    //}
                    //else
                    //{
                    //    singleLevel = "VOLT " + (data.SignalLevelV).ToString("0.0") + "V";
                    //}

                    singleLevel = "VOLT " + (data.SignalLevelV).ToString("0.###") + "V";
                }
            }
            else
            {
                singleLevel = "CURR " + data.SignalLevelI + "A";
            }

            if (this._cmdData.SingleLevel != singleLevel)
            {
                this._cmdData.SingleLevel = singleLevel;

                this._device.SendCommand(singleLevel);
            }



        }

        protected override void SetMsrtSpeed(LCRSettingData data)
        {
            string msrtSpeed = string.Empty;

            switch (data.MsrtSpeed)
            {
                case ELCRMsrtSpeed.Long:
                    msrtSpeed = "APER LONG";
                    break;
                case ELCRMsrtSpeed.Medium:
                    msrtSpeed = "APER MED";
                    break;
                case ELCRMsrtSpeed.Short:
                    msrtSpeed = "APER SHOR";
                    break;
                default:
                    msrtSpeed = "APER MED";
                    break;
            }

            if (this._cmdData.Speed != msrtSpeed)
            {
                this._cmdData.Speed = msrtSpeed;

                this._device.SendCommand(msrtSpeed);
            }
        }

        protected override void SetDC(LCRSettingData data)
        {
            string biasV = string.Empty;

            if (data.DCBiasV <= this._spec.DCBiasVMin)
            {
                biasV = "BIAS:VOLT MIN";
            }
            else if (data.DCBiasV > this._spec.DCBiasVMax)
            {
                biasV = "BIAS:VOLT MAX";
            }
            else
            {


                biasV = "BIAS:VOLT " + (data.DCBiasV).ToString("0.##") ;
            }

            if (this._devSetting.LCRDCBiasType == ELCRDCBiasType.Internal && (this._cmdData.Bias != biasV || this._cmdData.Points > 1))//上一次是Sweep
            {
                this._cmdData.Points = 1;

                this._cmdData.Bias = biasV;

                this._device.SendCommand(biasV);

                SetBiasDelay(data.BiasDelay);

                SetdisplayMode("DISP:PAGE MEAS");

                //this._device.SendCommand("DISP:PAGE MEAS");
            }

            //if (this._devSetting.LCRDCBiasType == ELCRDCBiasType.Internal)
            //{
            //    this._device.SendCommand("BIAS:STAT ON");
            //}

        }

        protected override void SetRange(LCRSettingData data)
        {
            string msrtRange = string.Empty;
            string msrtAuto = string.Empty;


            if (data.Range > 0 && data.Range <= 100000)
            {
                msrtAuto = "FUNC:IMP:RANG:AUTO OFF";
                msrtRange = "FUNC:IMP:RANG " + data.Range + "ohm";
                if (this._cmdData.Range != msrtRange)
                {
                    this._cmdData.Range = msrtRange;

                    this._device.SendCommand(msrtAuto);

                    this._device.SendCommand(msrtRange);
                }
            }
            else 
            {
                msrtRange = "FUNC:IMP:RANG:AUTO ON";
                if (this._cmdData.Range != msrtRange)
                {
                    this._cmdData.Range = msrtRange;

                    this._device.SendCommand(msrtRange);
                }
            }


        }

        protected override void SetBiasDelay(double bDelay)
        {
            string Stepdelay = "TRIG:DEL " + bDelay.ToString("0.###");

            if (this._cmdData.TriggerDelay != Stepdelay)
            {
                this._cmdData.TriggerDelay = Stepdelay;

                this._device.SendCommand(Stepdelay);
            }
        }

        //private class CmdData
        //{
        //    public string Frequency { get; set; }
        //    public string SingleLevel { get; set; }
        //    public string Bias { get; set; }
        //    public string TestType { get; set; }
        //    public string Speed { get; set; }
        //    public string Range { get; set; }
        //    public string MsrtAuto { get; set; }
        //    //201703008 david
        //    public string HighAcc { get; set; }
        //    public string BiasOn  { get; set; }
        //}
    
    }
}
