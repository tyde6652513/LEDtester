using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Data;
using MPI.Tester.Maths;

namespace MPI.Tester.Device.LCRMeter
{
    public class LCRBase : ILCRMeter
    {
        protected IConnect _device;
        protected LCRDevSetting _devSetting;
        protected LCRSettingData[] _lcrSetting;
        protected EDevErrorNumber _errorCode;
        protected List<double>[][] _msrtResult;
        protected string _serialNumber;
        protected string _softwareVersion;
        protected string _hardwareVersion;
        protected LCRMeterSpec _spec;

        protected CmdData _cmdData;
        protected LCRCaliData _caliData;
        protected MPI.PerformanceTimer _pt1;


        #region>>> constructor<<<
        public LCRBase()
        {
            this._devSetting = new LCRDevSetting();

            this._lcrSetting = null;

            this._msrtResult = null;

            this._errorCode = EDevErrorNumber.Device_NO_Error;

            this._serialNumber = string.Empty;

            this._softwareVersion = string.Empty;

            this._hardwareVersion = string.Empty;

            this._cmdData = new CmdData();

            this._pt1 = new PerformanceTimer();
        }
        #endregion

        #region>>public property<<
        public LCRCaliData CaliData
        {
            get
            {
                return _caliData;
            }
        }

        public EDevErrorNumber ErrorNumber
        {
            get { return this._errorCode; }
        }

        public string SerialNumber
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

        public LCRMeterSpec Spec
        {
            get { return this._spec; }
        }

        #endregion

        public virtual void Close()
        { 
        }

        public virtual bool Init(int deviceNum, string sourceMeterSN)
        {
            return true;
        }

        public virtual bool LCRCali(ELCRCaliMode caliMode)
        {
            int nowDataNume0Base = _caliData.NowDataNum - 1;
            bool isSetSingleFreq = _caliData.LoadingList[nowDataNume0Base].Enable;

            string msg = string.Empty;

            ELCRMsrtType typeA;
            ELCRMsrtType typeB;

            GetMsrtTypeA_B(_caliData.TestType, out  typeA, out  typeB);

            switch (caliMode)
            {
                case ELCRCaliMode.Open:
                    if (isSetSingleFreq)
                    {
                        this._device.SendCommand("CORR:SPOT" + (_caliData.NowDataNum).ToString() + ":OPEN");
                    }
                    else
                    {
                        this._device.SendCommand("CORR:OPEN");
                    }

                    this._device.SendCommand("CORR:OPEN:STAT?");
                    this._device.WaitAndGetData(out msg);
                    break;
                case ELCRCaliMode.Short:
                    if (isSetSingleFreq)
                    {
                        this._device.SendCommand("CORR:SPOT" + (_caliData.NowDataNum).ToString() + ":SHOR");
                    }
                    else
                    {
                        this._device.SendCommand("CORR:SHOR");
                    }

                    this._device.SendCommand("CORR:SHOR:STAT?");
                    this._device.WaitAndGetData(out msg);

                    break;
                case ELCRCaliMode.Load:
                    this._device.SendCommand("CORR:SPOT" + (_caliData.NowDataNum).ToString() + ":LOAD");

                    this._device.SendCommand("CORR:LOAD:STAT?");
                    this._device.WaitAndGetData(out msg);

                    break;
            }

            //GetRawDataOfMeterCali();

            //List<double> msrtValList = GetRawDataOfCali();  

            switch (caliMode)
            {
                case ELCRCaliMode.Open:
                    //_caliData.LoadingList[nowDataNume0Base].OpenRaw.ValA = msrtValList[0];
                    //_caliData.LoadingList[nowDataNume0Base].OpenRaw.ValB = msrtValList[1];
                    _caliData.LoadingList[nowDataNume0Base].OpenRaw.UnitA = GetTypeSIUnit(typeA);
                    _caliData.LoadingList[nowDataNume0Base].OpenRaw.UnitB = GetTypeSIUnit(typeB);
                    _caliData.LoadingList[nowDataNume0Base].OpenRaw.MeterUnitA = "S";
                    _caliData.LoadingList[nowDataNume0Base].OpenRaw.MeterUnitB = "S";
                    _caliData.LoadingList[nowDataNume0Base].OpenRaw.CaliLCRTestType = _caliData.TestType;
                    break;

                case ELCRCaliMode.Short:
                    //_caliData.LoadingList[nowDataNume0Base].ShortRaw.ValA = msrtValList[0];
                    //_caliData.LoadingList[nowDataNume0Base].ShortRaw.ValB = msrtValList[1];
                    _caliData.LoadingList[nowDataNume0Base].ShortRaw.UnitA = GetTypeSIUnit(typeA);
                    _caliData.LoadingList[nowDataNume0Base].ShortRaw.UnitB = GetTypeSIUnit(typeB);
                    _caliData.LoadingList[nowDataNume0Base].ShortRaw.MeterUnitA = "Ohm";
                    _caliData.LoadingList[nowDataNume0Base].ShortRaw.MeterUnitB = "Ohm";
                    _caliData.LoadingList[nowDataNume0Base].ShortRaw.CaliLCRTestType = _caliData.TestType;
                    break;

                case ELCRCaliMode.Load:
                    //_caliData.LoadingList[nowDataNume0Base].LoadRaw.ValA = msrtValList[0];
                    //_caliData.LoadingList[nowDataNume0Base].LoadRaw.ValB = msrtValList[1];
                    _caliData.LoadingList[nowDataNume0Base].LoadRaw.UnitA = GetTypeSIUnit(typeA);
                    _caliData.LoadingList[nowDataNume0Base].LoadRaw.UnitB = GetTypeSIUnit(typeB);
                    _caliData.LoadingList[nowDataNume0Base].LoadRaw.MeterUnitA = GetTypeSIUnit(typeA);
                    _caliData.LoadingList[nowDataNume0Base].LoadRaw.MeterUnitB = GetTypeSIUnit(typeB);
                    _caliData.LoadingList[nowDataNume0Base].LoadRaw.CaliLCRTestType = _caliData.TestType;

                    break;
            }


            TurnOff();

            return true;
        }

        public virtual List<double> GetRawDataOfCali()
        {

            //Dictionary<string, double> itemValdic = new Dictionary<string, double>();

            SetEnableTest(false, false, false);

            SetDC(_caliData.Bias);

            SetdisplayMode("DISP:PAGE MEAS");

            SetAC(ELCRSignalMode.Voltage, _caliData.Level);

            SetMsrtSpeed("Long");

            SetType(_caliData.TestType);

            SetBiasDelay(1);

            this._device.SendCommand(":TRIG:IMM");

            #region>>wait Msrt<<

            this._device.SendCommand(":STAT:OPER:COND?");

            string msg1 = string.Empty; 

            while (msg1 == string.Empty || msg1 == "")
            {
                this._device.WaitAndGetData(out msg1);
            }

            #endregion

            this._device.SendCommand(":FETC?");

            string msg = string.Empty;

            this._device.WaitAndGetData(out msg);

            List<double> msrtValList = ParseMsrtData(msg);

            SetEnableTest(_caliData.EnableOpen, _caliData.EnableShort, _caliData.EnableLoad);

            return msrtValList;
 
        }

        public virtual void GetRawDataOfMeterCali()
        {
            string msg = string.Empty;

            this._device.SendCommand("CORR:USE:DATA:SING?");

            _pt1.Reset();

            _pt1.Start();

            while ((msg == string.Empty || msg == "") && _pt1.PeekTimeSpan(ETimeSpanUnit.Second) < 20)//20秒緩衝
            {
                this._device.WaitAndGetData(out msg);
            }

            _pt1.Stop();

            try
            {
                string[] data = msg.Split(',');

                for (int i = 0; i < _caliData.LoadingList.Count(); ++i)
                {
                    LoadingRefData lrd = _caliData.LoadingList[i];
                    lrd.OpenRaw.MeterRowValA = double.Parse(data[6 * i]);
                    lrd.OpenRaw.MeterRowValB = double.Parse(data[6 * i + 1]);
                    lrd.ShortRaw.MeterRowValA = double.Parse(data[6 * i + 2]);
                    lrd.ShortRaw.MeterRowValB = double.Parse(data[6 * i + 3]);
                    lrd.LoadRaw.MeterRowValA = double.Parse(data[6 * i + 4]);
                    lrd.LoadRaw.MeterRowValB = double.Parse(data[6 * i + 5]);

                    double cVal = 0, dVal = 0;
                    G_BToC_D(lrd.OpenRaw.MeterRowValA, lrd.OpenRaw.MeterRowValB, lrd.Freq, out  cVal, out  dVal);
                    lrd.OpenRaw.ValA = cVal;
                    lrd.OpenRaw.ValB = dVal;
                    lrd.OpenRaw.UnitA = "F";

                    double lVal = 0, qVal = double.PositiveInfinity;
                    R_XToL_D(lrd.ShortRaw.MeterRowValA, lrd.ShortRaw.MeterRowValA, lrd.Freq, out  lVal, out  qVal);
                    lrd.ShortRaw.ValA = cVal;
                    lrd.ShortRaw.ValB = dVal;
                    lrd.ShortRaw.UnitA = "H";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[LCRBAse], GetRawDataOfMeterCali, Err:" + e.Message);
            }

        }

        public virtual bool MeterOutput(uint[] activateChannels, uint settingIndex)
        {

            //_pt1.Reset();
            //_pt1.Start();
            this._device.SendCommand(":TRIG:IMM");
            

            double askDataDelay = GetDataDelayTime(settingIndex);

            double totalDelayTime = this._lcrSetting[settingIndex].Point *
                (this._lcrSetting[settingIndex].BiasDelay + askDataDelay);//次*秒
                //(this._lcrSetting[settingIndex].BiasDelay);//次*秒

            string msg1 = string.Empty;

            

            if (totalDelayTime > 5)//粗估時間，超過WaitAndGetData會強迫回傳空字串結束
            {
                //System.Threading.Thread.Sleep((int)((totalDelayTime - 4) * 1000));

                this._device.SendCommand(":STAT:OPER:COND?");

                while (msg1 == string.Empty || msg1 == "")
                {
                    this._device.WaitAndGetData(out msg1);
                }
            }

            this._device.SendCommand(":FETC?");

            string msg = string.Empty;
            
            this._device.WaitAndGetData(out msg);
            //double t = _pt1.PeekTimeSpan(ETimeSpanUnit.MilliSecond);
            //Console.WriteLine("[LCR],WaitAndGetData" + t.ToString());

            this.ParseMsrtData(msg, settingIndex);
            //t = _pt1.PeekTimeSpan(ETimeSpanUnit.MilliSecond);
            //Console.WriteLine("[LCR],ParseMsrtData" + t.ToString());
            return true;
        }

        private double GetDataDelayTime(uint settingIndex)
        {
            double askDataDelay = 0;

            switch (this._lcrSetting[settingIndex].MsrtSpeed)//以E4980A為參考數值
            {
                case ELCRMsrtSpeed.Short:
                    if (this._lcrSetting[settingIndex].Frequency <= 100)
                    {
                        askDataDelay = 0.4;
                    }
                    else if (this._lcrSetting[settingIndex].Frequency <= 1000)
                    {
                        askDataDelay = 0.1;
                    }
                    else
                    {
                        askDataDelay = 0.04;
                    }
                    break;
                case ELCRMsrtSpeed.Medium:
                    if (this._lcrSetting[settingIndex].Frequency <= 100)
                    {
                        askDataDelay = 0.4;
                    }
                    else if (this._lcrSetting[settingIndex].Frequency <= 1000)
                    {
                        askDataDelay = 0.2;
                    }
                    else
                    {
                        askDataDelay = 0.1;
                    }
                    break;
                case ELCRMsrtSpeed.Long:
                default:
                    if (this._lcrSetting[settingIndex].Frequency <= 100)
                    {
                        askDataDelay = 0.6;
                    }
                    else if (this._lcrSetting[settingIndex].Frequency <= 1000)
                    {
                        askDataDelay = 0.4;
                    }
                    else
                    {
                        askDataDelay = 0.3;
                    }
                    break;
            }

            return askDataDelay;
        }

        public virtual bool PreSettingParamToMeter(uint settingIndex)
        {
            return true;
        }

        public virtual bool PreSetBiasListToMeter(uint settingIndex)
        {
            return true;
        }

        public virtual bool SetCaliData(LCRCaliData cData)
        {
            _caliData = cData.Clone() as LCRCaliData;

            int dataNum = cData.NowDataNum;

            string type = SetCaliType(_caliData.TestType);

            string CableLength = SetCableLength(_caliData.CableLength);

            SetEnableTest(_caliData.EnableOpen, _caliData.EnableShort, _caliData.EnableLoad);

            SetLoadData(dataNum - 1);

            return true;
        }

        public virtual bool SetConfigToMeter(LCRDevSetting devSetting)
        {
            this._devSetting = devSetting;

            return true;
        }

        public virtual bool SetParamToMeter(LCRSettingData[] paramSetting)
        {
            this._errorCode = EDevErrorNumber.Device_NO_Error;

            if (paramSetting == null || paramSetting.Length == 0)
            {
                return true;
            }

            int msrtLen = Enum.GetNames(typeof(ELCRMsrtType)).Length;

            this._msrtResult = new List<double>[paramSetting.Length][];

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

                this._msrtResult[i] = new List<double>[msrtLen];
            }

            this._lcrSetting = paramSetting;

            return true;
        }

        public virtual void TurnOff()
        {
            if (this._device != null)
            {
                if (this._devSetting.LCRDCBiasType == ELCRDCBiasType.Internal)
                {
                    this._device.SendCommand("BIAS:VOLT 0");

                    this._cmdData.Bias = "BIAS:VOLT 0";
                }
            }
        }

        public virtual void TurnOff(uint settingIndex)
        {
            if (this._lcrSetting == null || this._lcrSetting.Length == 0)
            {

            }
            else
            {

                
                LCRSettingData data = null;


                //for (int i = (int)settingIndex + 1; i < _lcrSetting.Length; ++i)
                //{
                //    if(this._lcrSetting[i])
                //}
                int tarIndex = (int)settingIndex + 1; //這樣寫遇到測試項目被Disable的時候會有問題，但先這樣處理讓實驗能快速上線
                if (tarIndex < _lcrSetting.Length)
                {
                    data = this._lcrSetting[tarIndex];
                }


                if (data != null && data.BiasDelay < 0)
                {

                }
                else 
                {
                    if (this._devSetting.LCRDCBiasType == ELCRDCBiasType.Internal)
                    {
                        this._device.SendCommand("BIAS:VOLT 0");

                        this._cmdData.Bias = "BIAS:VOLT 0";
                    }
                }
            }
        }

        public virtual bool ParseMsrtData(string msg, uint index)
        {
            string[] data = msg.Split(',');

            double valueA = 0.0d;

            double valueB = 0.0d;

            for (int i = 0; i < this._msrtResult[index].Length; i++)
            {
                this._msrtResult[index][i] = new List<double>(0);
            }

            ELCRMsrtType typeA;
            ELCRMsrtType typeB;
            GetMsrtTypeA_B(this._lcrSetting[index].LCRMsrtType, out typeA, out  typeB);

            int points = this._lcrSetting[index].Point;
            double sweepVal = this._lcrSetting[index].DCBiasStart;
            double sweepStep = this._lcrSetting[index].DCBiasStep;

            if (points > 1)
            {
                for (int i = 0; i < points; ++i)
                {
                    if (double.TryParse(data[i * 4], out valueA) && double.TryParse(data[i * 4+1], out valueB))
                    {
                        this._msrtResult[index][(int)typeA].Add(valueA);
                        this._msrtResult[index][(int)typeB].Add(valueB);

                        if (this._lcrSetting[index].DCBiasMode == ELCRDCBiasMode.Voltage)
                        {
                            this._msrtResult[index][(int)ELCRMsrtType.LCRVDC].Add((sweepVal + sweepStep * i));
                        }
                        else 
                        {
                            this._msrtResult[index][(int)ELCRMsrtType.LCRIDC].Add((sweepVal + sweepStep * i));
                        }
                    }
                }
                return true;
            }
            else
            {

                if (double.TryParse(data[data.Length - 3], out valueA) && double.TryParse(data[data.Length - 2], out valueB))
                {
                    this._msrtResult[index][(int)typeA].Add(valueA);
                    this._msrtResult[index][(int)typeB].Add(valueB);
                    if (this._lcrSetting[index].DCBiasMode == ELCRDCBiasMode.Voltage)
                    {
                        this._msrtResult[index][(int)ELCRMsrtType.LCRVDC].Add(this._lcrSetting[index].DCBiasV);
                    }
                    else
                    {
                        this._msrtResult[index][(int)ELCRMsrtType.LCRIDC].Add(this._lcrSetting[index].DCBiasI);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public virtual List<double> ParseMsrtData(string msg)
        {
            string[] data = msg.Split(',');

            double valueA = 0.0d;

            double valueB = 0.0d;

            List<double> msrtValList = new List<double>();


            if (double.TryParse(data[data.Length - 3], out valueA) && double.TryParse(data[data.Length - 2], out valueB))
            {
                msrtValList.Add(valueA);
                msrtValList.Add(valueB);
            }
            return msrtValList;

            
        }

        public virtual List<List<float>> GetDataFromMeter(uint settingIndex)
        {
            List<List<float>> outll = new List<List<float>>();
 
            //double
           
            for (int itemIndex = 0; itemIndex < this._msrtResult[settingIndex].Length; ++itemIndex)
            {
                List<float> dataList = new List<float>();

                if (this._msrtResult[settingIndex][itemIndex] != null && this._msrtResult[settingIndex][itemIndex].Count > 0)
                {
                    for (int valIndex = 0; valIndex < this._msrtResult[settingIndex][itemIndex].Count; ++valIndex)
                    {
                        dataList.Add( (float)this._msrtResult[settingIndex][itemIndex][valIndex]);
                    }
                }
                outll.Add(dataList);
            }

            return outll;
        }

        public virtual double[] GetDataFromMeter(uint channel, uint settingIndex)
        {
            double[] valArr = new double[this._msrtResult[settingIndex].Length];
            for (int i = 0; i < valArr.Length; ++i)
            {
                if (this._msrtResult[settingIndex][i] != null && this._msrtResult[settingIndex][i].Count > 0)
                {
                    valArr[i] = this._msrtResult[settingIndex][i][0];
                }
                else
                {
                    valArr[i] = 0;
                }
            }
            return valArr;
        }

        //protected virtual double UnitToPower(ECapUnit unitA, ECapUnit unitB)//10^(B-A)
        //{
        //    double num = 10;//10為底數
        //    int powTimes = 1;

        //    powTimes = (int)unitB - (int)unitA;
        //    num = Math.Pow(10, powTimes);
        //    //num = //Math.Pow(Enum.GetValues(unitA)

        //    return num;
        //}

        public class CmdData
        {
            public string Frequency { get; set; }
            public string SingleLevel { get; set; }
            public string Bias { get; set; }
            public string TestType { get; set; }
            public string Speed { get; set; }
            public string Range { get; set; }
            public int Points { get; set; }
            public string TriggerDelay { get; set; }

            public string DisPlayMode { get; set; }
            //public string 
        }

        public void GetMsrtTypeA_B(ELCRTestType msrtType, out ELCRMsrtType typeA, out ELCRMsrtType typeB)
        {
            typeA = ELCRMsrtType.LCRCP;
            typeB = ELCRMsrtType.LCRD;
            switch (msrtType)
            {

                case ELCRTestType.CPD:
                    typeA = ELCRMsrtType.LCRCP;
                    typeB = ELCRMsrtType.LCRD;
                    break;

                case ELCRTestType.CPG:
                    typeA = ELCRMsrtType.LCRCP;
                    typeB = ELCRMsrtType.LCRG;
                    break;
                case ELCRTestType.CPQ:
                    typeA = ELCRMsrtType.LCRCP;
                    typeB = ELCRMsrtType.LCRQ;

                    break;
                case ELCRTestType.CPRP:
                    typeA = ELCRMsrtType.LCRCP;
                    typeB = ELCRMsrtType.LCRRP;
                    break;
                case ELCRTestType.CSD:
                    typeA = ELCRMsrtType.LCRCS;
                    typeB = ELCRMsrtType.LCRD;
                    break;
                case ELCRTestType.CSQ:
                    typeA = ELCRMsrtType.LCRCS;
                    typeB = ELCRMsrtType.LCRQ;
                    break;
                case ELCRTestType.CSRS:
                    typeA = ELCRMsrtType.LCRCS;
                    typeB = ELCRMsrtType.LCRRS;
                    break;
                case ELCRTestType.LPD:
                    typeA = ELCRMsrtType.LCRLP;
                    typeB = ELCRMsrtType.LCRD;
                    break;
                case ELCRTestType.LPG:
                    typeA = ELCRMsrtType.LCRLP;
                    typeB = ELCRMsrtType.LCRG;
                    break;
                case ELCRTestType.LPQ:
                    typeA = ELCRMsrtType.LCRLP;
                    typeB = ELCRMsrtType.LCRQ;
                    break;
                case ELCRTestType.LPRD:
                    typeA = ELCRMsrtType.LCRLP;
                    typeB = ELCRMsrtType.LCRRDC;
                    break;
                case ELCRTestType.LPRP:
                    typeA = ELCRMsrtType.LCRLP;
                    typeB = ELCRMsrtType.LCRRP;
                    break;
                case ELCRTestType.LSD:
                    typeA = ELCRMsrtType.LCRLS;
                    typeB = ELCRMsrtType.LCRD;
                    break;
                case ELCRTestType.LSQ:
                    typeA = ELCRMsrtType.LCRLS;
                    typeB = ELCRMsrtType.LCRQ;
                    break;
                case ELCRTestType.LSRD:
                    typeA = ELCRMsrtType.LCRLS;
                    typeB = ELCRMsrtType.LCRRDC;
                    break;
                case ELCRTestType.LSRS:
                    typeA = ELCRMsrtType.LCRLS;
                    typeB = ELCRMsrtType.LCRRS;
                    break;
                case ELCRTestType.GB:
                    typeA = ELCRMsrtType.LCRG;
                    typeB = ELCRMsrtType.LCRB;
                    break;
                case ELCRTestType.RX:
                    typeA = ELCRMsrtType.LCRR;
                    typeB = ELCRMsrtType.LCRX;
                    break;
                case ELCRTestType.VDID:
                    typeA = ELCRMsrtType.LCRVDC;
                    typeB = ELCRMsrtType.LCRIDC;
                    break;
                case ELCRTestType.YTD:
                    typeA = ELCRMsrtType.LCRY;
                    typeB = ELCRMsrtType.LCRTD;
                    break;
                case ELCRTestType.YTR:
                    typeA = ELCRMsrtType.LCRY;
                    typeB = ELCRMsrtType.LCRTR;
                    break;
                case ELCRTestType.ZTD:
                    typeA = ELCRMsrtType.LCRZ;
                    typeB = ELCRMsrtType.LCRTD;
                    break;
                case ELCRTestType.ZTR:
                    typeA = ELCRMsrtType.LCRZ;
                    typeB = ELCRMsrtType.LCRTR;
                    break;
            }
        }

        public string GetTypeSIUnit(ELCRMsrtType type)
        {
            string outstr = "";

            switch (type)
            {
                case ELCRMsrtType.LCRB:
                case ELCRMsrtType.LCRG:
                case ELCRMsrtType.LCRY:
                    outstr = "S";
                    break;
                case ELCRMsrtType.LCRCP:
                case ELCRMsrtType.LCRCS:
                    outstr = "F";
                    break;
                case ELCRMsrtType.LCRLP:
                case ELCRMsrtType.LCRLS:
                    outstr = "H";
                    break;
                case ELCRMsrtType.LCRR:
                case ELCRMsrtType.LCRRDC:
                case ELCRMsrtType.LCRRP:
                case ELCRMsrtType.LCRRS:
                case ELCRMsrtType.LCRX:
                case ELCRMsrtType.LCRZ:
                    outstr = "Ohm";
                    break;
                case ELCRMsrtType.LCRVDC:
                    outstr = "V";
                    break;
                case ELCRMsrtType.LCRIDC:
                    outstr = "A";
                    break;
                default:
                    outstr = "";
                    break;
                    
            }
            return outstr;


        }

        protected virtual void SetAC(LCRSettingData data)
        {
            //string singleLevel = string.Empty;
            double singleLevel;

            if (data.SignalMode == ELCRSignalMode.Voltage)
            {
                singleLevel = data.SignalLevelV;
                //singleLevel = "VOLT " + data.SignalLevelV + "V";
            }
            else
            {
                singleLevel = data.SignalLevelI;
                //singleLevel = "CURR " + data.SignalLevelI + "A";
            }

            SetAC(data.SignalMode, singleLevel);

            //if (this._cmdData.SingleLevel != singleLevel)
            //{
            //    this._cmdData.SingleLevel = singleLevel;

            //    this._device.SendCommand(singleLevel);
            //}
        }

        protected virtual void SetAC(ELCRSignalMode mode,double level)
        {
            string singleLevel = string.Empty;

            if (mode == ELCRSignalMode.Voltage)
            {
                singleLevel = "VOLT " + level + "V";
            }
            else
            {
                singleLevel = "CURR " + level + "A";
            }

            if (this._cmdData.SingleLevel != singleLevel)
            {
                this._cmdData.SingleLevel = singleLevel;

                this._device.SendCommand(singleLevel);
            }
        }

        protected virtual void SetRange(LCRSettingData data)
        {
            SetRange(data.Range);
        }

        protected virtual void SetRange(int range)
        {
            string msrtRange = string.Empty;

            if (range == 0)
            {
                msrtRange = "FUNC:IMP:RANG:AUTO ON";
            }
            else
            {
                msrtRange = "FUNC:IMP:RANG " + range.ToString();
            }

            if (this._cmdData.Range != msrtRange)
            {
                this._cmdData.Range = msrtRange;

                this._device.SendCommand(msrtRange);
            }
        }

        protected virtual void SetType(LCRSettingData data)
        {
            SetType(data.LCRMsrtType);
        }

        protected virtual void SetType(ELCRTestType type)
        {
            string msrtType = "FUNC:IMP " + type.ToString();
            if (this._cmdData.TestType != msrtType)
            {
                this._cmdData.TestType = msrtType;

                this._device.SendCommand(msrtType);
            }
        }

        protected virtual void SetFreq(LCRSettingData data)
        {
            //string frequency = "FREQ " + data.Frequency;

            //if (this._cmdData.Frequency != frequency)
            //{
            //    this._cmdData.Frequency = frequency;

            //    this._device.SendCommand(frequency);
            //}

            SetFreq(data.Frequency);
        }

        protected virtual void SetFreq(double freq)
        {
            string frequency = "FREQ " + freq.ToString("0");

            if (this._cmdData.Frequency != frequency)
            {
                this._cmdData.Frequency = frequency;

                this._device.SendCommand(frequency);
            }
        }

        protected virtual void SetMsrtSpeed(LCRSettingData data)
        {
            string msrtSpeed = string.Empty;

            switch (data.MsrtSpeed)
            {
                case ELCRMsrtSpeed.Long:
                case ELCRMsrtSpeed.Medium:
                case ELCRMsrtSpeed.Short:
                    {
                        msrtSpeed =  data.MsrtSpeed.ToString();

                        break;
                    }
                default:
                    {
                        msrtSpeed = "Long";

                        break;
                    }
            }

            SetMsrtSpeed(msrtSpeed);
        }

        protected virtual void SetMsrtSpeed(string spd)
        {
            string msrtSpeed = string.Empty;

            msrtSpeed = "APER " + spd;

            if (this._cmdData.Speed != msrtSpeed)
            {
                this._cmdData.Speed = msrtSpeed;

                this._device.SendCommand(msrtSpeed);
            }
        }

        protected virtual void SetDC(LCRSettingData data)
        {
            string bias = ":BIAS:VOLT " + data.DCBiasV;

            if (this._devSetting.LCRDCBiasType == ELCRDCBiasType.Internal && (this._cmdData.Bias != bias || this._cmdData.Points > 1))//上一次是Sweep
            {
                this._cmdData.Points = 1;

                this._cmdData.Bias = bias;

                this._device.SendCommand(bias);

                //this._device.SendCommand("DISP:PAGE MEAS");

                SetdisplayMode("DISP:PAGE MEAS");

                SetBiasDelay(data.BiasDelay);

                if (this._devSetting.LCRDCBiasType == ELCRDCBiasType.Internal)
                {
                    this._device.SendCommand(":BIAS:STAT ON");
                }
            }
            

        }

        protected virtual void SetDC(double volt)
        {
            string bias = ":BIAS:VOLT " + volt.ToString();

            if (this._cmdData.Bias != bias)
            {
                this._cmdData.Bias = bias;

                this._device.SendCommand(bias);
            }
        }

        protected virtual void SetBiasList(LCRSettingData data)
        {
            string bias = string.Empty;

            SetBiasDelay(data.BiasDelay);


            if (data.SignalMode == ELCRSignalMode.Voltage)
            {
                bias += "LIST:BIAS:VOLT ";
                for (int i = 0; i < data.Point; ++i)
                {
                    bias += (data.DCBiasStart + i * data.DCBiasStep).ToString("0.0");
                    if (i != data.Point - 1)
                    {
                        bias += ", ";
                    }
                }
            }
            else
            {
                bias += "LIST:BIAS:CURR ";
                for (int i = 0; i < data.Point; ++i)
                {
                    bias += (data.DCBiasStart + i * data.DCBiasStep).ToString("0.000");
                    if (i != data.Point - 1)
                    {
                        bias += ", ";
                    }
                }
            }

            if (this._devSetting.LCRDCBiasType == ELCRDCBiasType.Internal && this._cmdData.Bias != bias)//上一次是Sweep
            {
                this._cmdData.Points = 1;

                this._cmdData.Bias = bias;

                this._device.SendCommand(bias);

                SetdisplayMode("DISP:PAGE LIST");
                //this._device.SendCommand("DISP:PAGE LIST");///////////////////////////////////////////////////////////////////////必須將"畫面"切到List模式才能正常運作

            }

            //if (this._devSetting.LCRDCBiasType == ELCRDCBiasType.Internal)
            //{
            //    this._device.SendCommand("BIAS:STAT ON");
            //}
        }

        protected virtual void SetBiasDelay(double bDelay)
        {

            if (bDelay >= 0)
            {
                string Stepdelay = ":TRIGger:DELay " + bDelay.ToString("0.###");

                if (this._cmdData.TriggerDelay != Stepdelay)
                {
                    this._cmdData.TriggerDelay = Stepdelay;

                    this._device.SendCommand(Stepdelay);
                }
            }
        }

        protected virtual void SetdisplayMode(string DisPlayMode)
        {

            if (this._cmdData.DisPlayMode != DisPlayMode)
            {
                this._cmdData.DisPlayMode = DisPlayMode;

                this._device.SendCommand(DisPlayMode);
            }
        }

        protected virtual string SetCaliType(ELCRTestType type)
        {
            string theType = "CORR:LOAD:TYPE " + type.ToString();

            this._device.SendCommand(theType);

            return theType;
        }

        protected virtual string SetCableLength(string cableLen)
        {
            string cable = string.Empty;

            switch (cableLen)
            {
                case "0m":
                    cable = "CORR:LENG 0";
                    break;
                case "1m":
                    cable = "CORR:LENG 1";
                    break;
                case "2m":
                    cable = "CORR:LENG 2";
                    break;
                case "4m":
                default:
                    cable = "CORR:LENG 4";
                    break;
            }

            this._device.SendCommand(cable);

            return cable;

        }

        protected virtual void SetEnableTest(bool openT = false, bool shortT = false, bool loadT = false)//for spot test //指定頻率的測試
        {
            string oStr = "CORR:OPEN:STAT " + (openT ? "ON" : "OFF");
            string sStr = "CORR:SHOR:STAT " + (shortT ? "ON" : "OFF");
            string lStr = "CORR:LOAD:STAT " + (loadT ? "ON" : "OFF");

            this._device.SendCommand(oStr);
            this._device.SendCommand(sStr);
            this._device.SendCommand(lStr);
        }

        protected virtual void SetLoadData(int dataNum)
        {
            LoadingRefData lData = _caliData.LoadingList[dataNum];

            this._device.SendCommand("CORR:SPOT" + (dataNum + 1).ToString() + ":STAT " + (lData.Enable ? "ON" : "OFF"));

            string frequency = "CORR:SPOT" + (dataNum + 1).ToString() + ":FREQ " + lData.Freq.ToString();

            this._device.SendCommand(frequency);

            //717;"CORR:SPOT1:LOAD:STAN 100.7,0.0002"

            //string paraA = (lData.RefA * UnitConvertFactor(ECapUnit.F, lData.RefUnit)).ToString("0.0###E-0");

            string paraA = (lData.RefA * MPI.Tester.Maths.UnitMath.UnitConvertFactor(lData.RefUnit, "F")).ToString("0.0###E-0");

            string paraB = lData.RefB.ToString("0.0####");

            this._device.SendCommand("CORR:SPOT" + (dataNum + 1).ToString() + ":LOAD:STAN " + paraA + "," + paraB);

        }

        //R+Xi = 1/(G + iB)
        //R = G/(G^2 + B^2)
        //X = -B/(G^2 + B^2)
        //D = -atan(R/X)
        //導納 -> 電容,開路量測換算用
        protected void G_BToC_D(double g, double b,int freq, out double cVal, out double dVal)
        {
            double r2b2 = g * g + b * b;
            cVal = 0;
            dVal = 0;
            if (r2b2 != 0 && b != 0)
            {
                double r = g / r2b2;
                double x = -b / r2b2;
                dVal = -r / x;
                double z = Math.Sqrt(r * r + x * x);
                double cos_d = Math.Cos(dVal);
                double Z_Project_img = z / cos_d;
                cVal = 1 / (Z_Project_img * 2 * Math.PI * freq);
            }
        }

        protected void R_XToL_D(double r, double x, int freq, out double lVal, out double qVal)
        {
            lVal = 0;
            qVal = double.PositiveInfinity;
            //dVal = 0;
            //q = wl/r
            if (freq != 0)
            {
                lVal = r / (2 * Math.PI * freq);

                //double r = g / r2b2;
                //double x = -b / r2b2;
                //dVal = -r / x;
                //double z = Math.Sqrt(r * r + x * x);
                //double cos_d = Math.Cos(dVal);
                //double Z_Project_img = z / cos_d;
                //cVal = 1 / (Z_Project_img * 2 * Math.PI * freq);
            }
            if (x != 0)
            {
                qVal = r / x;
            }
        }

        public static bool ISThisIPString(string sourceMeterSN)
        {
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
            return isOnlyIPAddressNum;
        }

    }


}
