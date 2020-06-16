using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPI.Tester.Device;
using MPI.Tester.DeviceCommon;


namespace MPI.Tester.Device.OSA
{
    public class MS9740A : IOSA
    {
        private IConnect _connect = null;

        public static string[] TRACE_ID = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

        private string _serialNum;
        private EDevErrorNumber _errorNum;

        private OsaDevSetting _config;

        private OsaSettingData[] _settingData;
        private OsaSettingData _settingStatus;
        private uint _indexStatus;
        private List<OsaData> _msrtDataArray;

        private uint _totalTriggerCount;

        public MS9740A()
        {
            this._errorNum = EDevErrorNumber.Device_NO_Error;

            this._settingStatus = new OsaSettingData();
            this._msrtDataArray = new List<OsaData>();

            this._config = new OsaDevSetting();
        }

        #region >>> Public Property <<<

        /// <summary>
        /// OSA serial number
        /// </summary>
        public string SerialNumber
        {
            get { return this._serialNum; }
        }

        /// <summary>
        /// OSA Data
        /// </summary>
        public OsaData[] Data
        {
            get { return this._msrtDataArray.ToArray(); }
        }

        public EDevErrorNumber ErrorNumber
        {
            get { return _errorNum; }
        }

        #endregion

        #region >>> Private Method <<<

        private bool CheckErrorStatus()
        {
            string msgCode;
            
            this._connect.SendCommand("ERR?");

            this._connect.WaitAndGetData(out msgCode);

            if (msgCode == "0")
            {
                return true;
            }

            this._errorNum = EDevErrorNumber.OSA_Parameter_Setting_Err;

            Console.WriteLine(string.Format("[OSA], Anritzu MS9740A setting Error: {0}", msgCode));

            return false;
        }

        private void SetOsaParameter(OsaSettingData setting)
        {
            bool isParameterChanged = false;

           

            if (this._settingStatus.CenterWavelength != setting.CenterWavelength)
            {
                this._connect.SendCommand("CNT " + setting.CenterWavelength);

                this._settingStatus.CenterWavelength = setting.CenterWavelength;

                isParameterChanged = true;
            }

            // SPN 0.2 to 1200.0
            if (this._settingStatus.SpanOfWavelength != setting.SpanOfWavelength)
            {
                this._connect.SendCommand("SPN " + setting.SpanOfWavelength);

                this._settingStatus.SpanOfWavelength = setting.SpanOfWavelength;

                isParameterChanged = true;
            }

            // RES 0.03 | 0.05 | 0.07 | 0.1 | 0.2 | 0.5 | 1.0
            if (this._settingStatus.Resoluation != setting.Resoluation)
            {
                this._connect.SendCommand("RES " + setting.Resoluation.ToString());

                this._settingStatus.Resoluation = setting.Resoluation;

                isParameterChanged = true;
            }

            // n = 0.03 | 0.05 | 0.07 | 0.1 |  0.2 | 0.5 | 1.0 
            if (this._settingStatus.LogDiv != setting.LogDiv)
            {
                this._connect.SendCommand("LOG " + setting.LogDiv.ToString());

                this._settingStatus.LogDiv = setting.LogDiv;

                isParameterChanged = true;
            }

            //At relative value display: Reference level (dB) –100.0 to 100.0 
            if (this._settingStatus.ReferenceLevel != setting.ReferenceLevel)
            {
                this._connect.SendCommand("RLV " + setting.ReferenceLevel);

                this._settingStatus.ReferenceLevel = setting.ReferenceLevel;

                isParameterChanged = true;
            }

            //VBW 10HZ|100HZ|200HZ|1KHZ|2KHZ|10KHZ|100KHZ|1MHZ |10|100|200|1000|2000|10000|100000|1000000 
            if (this._settingStatus.VideoBW != setting.VideoBW)
            {
                this._connect.SendCommand("VBW " + (setting.VideoBW * 1000.0d).ToString());

                this._settingStatus.VideoBW = setting.VideoBW;

                isParameterChanged = true;
            }

            // n = 51 | 101 | 251 | 501 | 1001 | 2001 | 5001 
            if (this._settingStatus.SamplingPoints != setting.SamplingPoints)
            {
                this._connect.SendCommand("MPT " + setting.SamplingPoints.ToString());

                this._settingStatus.SamplingPoints = setting.SamplingPoints;

                isParameterChanged = true;
            }

            // _connect.SendCommand("LLV " + "25UW");

            // Analysis Mode
            switch (setting.MS9740AnalysisMode)
            {
                case EMS9740AnalysisMode.ENV:
                    {
                        if (this._settingStatus.MS9740AnalysisMode != setting.MS9740AnalysisMode)
                        {
                            this._connect.SendCommand("ANA ENV,1");
                            isParameterChanged = true;
                        }

                        break;
                    }
                case EMS9740AnalysisMode.SMSR:
                    {
                        if (this._settingStatus.MS9740AnalysisMode != setting.MS9740AnalysisMode)
                        {
                            this._connect.SendCommand("ANA SMSR,2NDPEAK");
                            isParameterChanged = true;
                        }
                      
                        break;
                    }
                case EMS9740AnalysisMode.PWR:
                    {
                        if (this._settingStatus.MS9740AnalysisMode != setting.MS9740AnalysisMode)
                        {
                            this._connect.SendCommand("ANA PWR");
                            isParameterChanged = true;
                        }

                        break;
                    }
                case EMS9740AnalysisMode.FP_LD:
                    {
                        if (this._settingStatus.MS9740AnalysisMode != setting.MS9740AnalysisMode || this._settingStatus.FpSliceLevel != setting.FpSliceLevel)
                        {
                            this._connect.SendCommand(string.Format("AP FP, {0}", setting.FpSliceLevel));

                            this._settingStatus.FpSliceLevel = setting.FpSliceLevel;

                            isParameterChanged = true;
                        }

                        break;
                    }
                case EMS9740AnalysisMode.DFB_LD:
                    {
                        string sideMode = string.Empty;

                        if (this._settingStatus.MS9740AnalysisMode != setting.MS9740AnalysisMode ||
                            this._settingStatus.DfbSliceLevel != setting.DfbSliceLevel ||
                            this._settingStatus.DfbStdevFactor != setting.DfbStdevFactor ||
                            this._settingStatus.DfbSearchResolution != setting.DfbSearchResolution)
                        {
                            switch (setting.MS9740DfbSideMode)
                            {
                                case EMS9740DfbSideMode.Second_Peak:
                                    sideMode = "2NDPEAK";
                                    break;
                                case EMS9740DfbSideMode.Left:
                                    sideMode = "LEFT";
                                    break;
                                case EMS9740DfbSideMode.Right:
                                    sideMode = "RIGHT";
                                    break;
                            }

                            this._connect.SendCommand(string.Format("AP DFB, {0}, {1}, {2}", sideMode, setting.DfbSliceLevel, setting.DfbStdevFactor));
                            this._connect.SendCommand(string.Format("AP DFB, SRES, {0}", setting.DfbSearchResolution));

                            this._settingStatus.DfbSliceLevel = setting.DfbSliceLevel;
                            this._settingStatus.DfbStdevFactor = setting.DfbStdevFactor;
                            this._settingStatus.DfbSearchResolution = setting.DfbSearchResolution;

                            isParameterChanged = true;
                        }
                        break;
                    }
                default:
                    break;
            }

            this._settingStatus.MS9740AnalysisMode = setting.MS9740AnalysisMode;
        }

        private void SetOsaTrace(string traceID, ETraceType type)
        {
            string traceType = type.ToString();
            
            //TTP <trace>,BLANK|CALC|FIX|WRITE
            this._connect.SendCommand(string.Format("TTP {0},{1}", traceID, traceType));
        }

        private bool AcquireOsaResult(uint index)
        {
            this._msrtDataArray[(int)index].Clear();

            bool isValid = true;
            string rtnStr = string.Empty;
            string[] strArray = null;

            EMS9740AnalysisMode mode = this._settingData[index].MS9740AnalysisMode;

            // Query Result Data
            if (mode == EMS9740AnalysisMode.DFB_LD || mode == EMS9740AnalysisMode.FP_LD)
            {
                this._connect.SendCommand("APR?");  // Apllication Mode Response
            }
            else
            {
                this._connect.SendCommand("ANAR?");  // Analysis Mode Response
            }

            if (!this._connect.WaitAndGetData(out rtnStr) || rtnStr == string.Empty)
            {
                return false;
            }

            strArray = rtnStr.Split(',');

            //-----------------------------------------------------------------------------------------
            // Parse Result Data
            double smsrDeltaWl = 0.0d;
            double smsrDeltaLvl = 0.0d;

            double fwhm = 0.0d;
            double wlc = 0.0d;
            double wlp = 0.0d;
            double wlpLvl = 0.0d;
            double wlp2 = 0.0d;
            double wlpLvl2 = 0.0d;
            double ndB = 0.0d;
            double fpModeSpacing = 0.0d;
            double stdev = 0.0d;
            double totalPow = 0.0d;

            double spectrumWidthEnv = 0.0d;
            double spectrumWidthRMS = 0.0d;
            double dfbModeOffset = 0.0d;
            double dfbStopBand = 0.0d;
            double dfbCenterOffset = 0.0d;

            switch (this._settingStatus.MS9740AnalysisMode)
            {
                case EMS9740AnalysisMode.ENV:
                    {
                        isValid &= double.TryParse(strArray[0], out wlc);
                        isValid &= double.TryParse(strArray[1], out spectrumWidthEnv);
                        break;
                    }
                case EMS9740AnalysisMode.SMSR:
                    {
                        isValid &= double.TryParse(strArray[0], out smsrDeltaWl);
                        isValid &= double.TryParse(strArray[1], out smsrDeltaLvl);
                        break;
                    }
                case EMS9740AnalysisMode.PWR:
                    {
                        isValid &= double.TryParse(strArray[0], out totalPow);
                        isValid &= double.TryParse(strArray[1], out wlc);
                        break;
                    }
                case EMS9740AnalysisMode.FP_LD:
                    {
                        isValid &= double.TryParse(strArray[0], out fwhm);
                        isValid &= double.TryParse(strArray[1], out wlc);
                        isValid &= double.TryParse(strArray[2], out wlp);
                        isValid &= double.TryParse(strArray[3], out wlpLvl);
                        isValid &= double.TryParse(strArray[4], out ndB);
                        isValid &= double.TryParse(strArray[5], out fpModeSpacing);
                        isValid &= double.TryParse(strArray[6], out totalPow);
                        isValid &= double.TryParse(strArray[7], out stdev);
                        break;
                    }
                case EMS9740AnalysisMode.DFB_LD:
                    {
                        isValid &= double.TryParse(strArray[0], out smsrDeltaLvl);
                        isValid &= double.TryParse(strArray[1], out spectrumWidthRMS);
                        isValid &= double.TryParse(strArray[2], out wlp);
                        isValid &= double.TryParse(strArray[3], out wlpLvl);
                        isValid &= double.TryParse(strArray[4], out wlp2);
                        isValid &= double.TryParse(strArray[5], out wlpLvl2);
                        isValid &= double.TryParse(strArray[6], out dfbModeOffset);
                        isValid &= double.TryParse(strArray[7], out dfbStopBand);
                        isValid &= double.TryParse(strArray[8], out dfbCenterOffset);
                        isValid &= double.TryParse(strArray[9], out stdev);
                        break;
                    }
                default:
                    break;
            }

            this._msrtDataArray[(int)index].MeanWL = wlc;
            this._msrtDataArray[(int)index].PeakWL = wlp;
            this._msrtDataArray[(int)index].PeakLevel = wlpLvl;

            this._msrtDataArray[(int)index].PeakWL2 = wlp2;
            this._msrtDataArray[(int)index].PeakLevel2 = wlpLvl2;

            this._msrtDataArray[(int)index].FWHM = fwhm;

            this._msrtDataArray[(int)index].TotalPower = totalPow;

            this._msrtDataArray[(int)index].SMSR = smsrDeltaLvl;
            this._msrtDataArray[(int)index].DeltaLamda = smsrDeltaWl;

            this._msrtDataArray[(int)index].Stdev = stdev;
            this._msrtDataArray[(int)index].RMS = spectrumWidthRMS;

            return isValid;
        }

        #endregion

        #region >>> Public Method  <<<

        public bool Init(string ipAddress)
        {
            string info = string.Empty;

            string resourceName = string.Format("TCPIP0::{0}::INSTR", ipAddress);

            this._connect = new IVIConnect(resourceName);

            if (this._connect.Open(out info))
            {
                this._connect.SendCommand("*RST");
                this._connect.SendCommand("*IDN?");

                string rtnStr = string.Empty;

                this._connect.WaitAndGetData(out rtnStr);

                rtnStr = rtnStr.Replace("\r\n", "");

                string[] tempInfo = rtnStr.Split(',');

                if (tempInfo == null)
                {
                    this._errorNum = EDevErrorNumber.OSA_Init_Err;
                    this._connect = null;
                    Console.WriteLine("[MS9740A] Dev Init Fail. Dev Infomation is Empty.");
                    return false;
                }

                string manufacturer = tempInfo[0];
                string model = tempInfo[1];
                string sn = tempInfo[2];
                string ver = tempInfo[3];

                this._serialNum = string.Format("{0}_{1}_{2}", sn, model, ver);

                System.Threading.Thread.Sleep(1000);
            }
            else
            {
                this._errorNum = EDevErrorNumber.OSA_Connection_Failed_Err;
                this._connect = null;
                Console.WriteLine("[MS9740A] Connection Create Fail.");
                return false;
            }

            this._settingStatus = new OsaSettingData();
  
            return true;
        }

        public bool SetConfigToMeter(OsaDevSetting cfg)
        {
            this._config = cfg;
            
            return true;
        }

        public bool SetParaToMeter(OsaSettingData[] parameters)
        {
            this._errorNum = EDevErrorNumber.Device_NO_Error;

            if (this._connect == null)
            {
                this._errorNum = EDevErrorNumber.OSA_Connection_Failed_Err;
                return false;
            }
            
            // MSA
            if (parameters.Length < 1)
            {
                return true;
            }

            if (parameters.Length >= 10)
            {
                this._errorNum = EDevErrorNumber.OSA_Parameter_ExceedBufferSize_Err;
                
                return false;
            }

            this._settingStatus.Reset();

            this._totalTriggerCount = 0;

            this._settingData = parameters.Clone() as OsaSettingData[];

            //this._connect.SendCommand("*RST");

            // WDP AIR | VACUUM
            this._connect.SendCommand("WDP AIR");

            //-------------------------------------------------------------------------------------------------
            this._connect.SendCommand("AP OFF");    // Application Mode OFF
            this._connect.SendCommand("ANA OFF");   // Analysis Mode OFF

            uint activeCount = 0;
            bool isFoundFirstItem = false;

            this._msrtDataArray.Clear();

            foreach (var data in this._settingData)
            {
                data.TraceID = string.Empty;
                
                if (data.IsTrigger)
                {
                    data.TraceID = TRACE_ID[activeCount];
                    
                    if (!isFoundFirstItem)
                    {
                        this.SetOsaParameter(data);  // pre-Set Parameter at first enable item
                        isFoundFirstItem = true;
                    }

                    activeCount++;
                }

                //---------------------------------------------------
                // Create Msrt Result
                this._msrtDataArray.Add(new OsaData(data.TraceID));
            }

            this._totalTriggerCount = activeCount;

            return this.CheckErrorStatus();
        }

        public bool Trigger(uint index)
        {
            if (this._connect == null || this._errorNum != EDevErrorNumber.Device_NO_Error)
            {
                this._errorNum = EDevErrorNumber.OSA_Connection_Failed_Err;
                return false;
            }

            if (this._errorNum != EDevErrorNumber.Device_NO_Error)
            {
                return false;
            }

            if (this._settingData == null || this._settingData.Length == 0 || index > this._settingData.Length - 1)
            {
                this._errorNum = EDevErrorNumber.OSA_Trigger_Failed_Err;
                return false;
            }

            // SetParameter
            this.SetOsaParameter(this._settingData[index]);

            string traceID = this._settingData[index].TraceID;

            if (this._totalTriggerCount > 1)
            {
                this.SetOsaTrace(traceID, ETraceType.WRITE);
            }

            // Trigger
            string rtnStr = string.Empty;

            this._connect.SendCommand("SSI");  // Single Sweep

            this._connect.SendCommand("*OPC?");  

            this._connect.WaitAndGetData(out rtnStr);

            if (rtnStr.Trim() != "1")
            {
                this._errorNum = EDevErrorNumber.OSA_Trigger_Timeout_Err;

                return false;
            }

            if (this._totalTriggerCount > 1)
            {
                this.SetOsaTrace(traceID, ETraceType.FIX);
            }

            if (!this.AcquireOsaResult(index))
            {
                this._errorNum = EDevErrorNumber.OSA_Acquire_Results_Err;
                return false;
            }

            this._connect.SendCommand("*CLS");  // Clear Status, set OPC bit to 0

            return true;
        }

        public bool CalculateMeasureResultData(uint index)
        {
            if (this._connect == null)
            {
                return false;
            }

            string traceID = this._settingData[(int)index].TraceID;

            if (traceID == string.Empty)
            {               
                return false;
            }

            bool isValid = true;
            string rtnStr = string.Empty;

            string[] strArray = null;

            this._connect.SendCommand("TSL " + traceID);

            //-----------------------------------------------------------------------------------------
            // Query Spectrum
            //PerformanceTimer pt = new PerformanceTimer();
            //pt.Reset();
            //pt.Start();
            // Get RES
            double resolution = 0.0d;
           
            //this._connect.SendCommand("RES?");

            //if (!this._connect.WaitAndGetData(out rtnStr) || rtnStr == string.Empty)
            //{
            //    return false;
            //}

            //strArray = rtnStr.Split(',');

            //isValid &= double.TryParse(strArray[0], out resolution);

            //pt.Stop();
            //Console.WriteLine("[OSA, RES? Time = ]" + pt.GetTimeSpan(ETimeSpanUnit.MilliSecond).ToString("0.000"));

            // Get Wavelength   Start wavelength | Stop wavelength | Sampling points
            double startWl = 0.0d;
            double endWl = 0.0d;
            int samplingPoints = 501;

            //pt.Reset();
            //pt.Start();
            //this._connect.SendCommand("DCA?"); 
            this._connect.SendCommand(string.Format("DC{0}?", traceID)); 

            if (!this._connect.WaitAndGetData(out rtnStr) || rtnStr == string.Empty)
            {
                return false;
            }

            strArray = rtnStr.Split(',');

            isValid &= double.TryParse(strArray[0], out startWl);
            isValid &= double.TryParse(strArray[1], out endWl);
            isValid &= int.TryParse(strArray[2], out samplingPoints);

            //pt.Stop();
            //Console.WriteLine("[OSA, DCA? Time = ]" + pt.GetTimeSpan(ETimeSpanUnit.MilliSecond).ToString("0.000"));


            // Get Spectrum
            //pt.Reset();
            //pt.Start();

            //this._connect.SendCommand("DMA?");
            this._connect.SendCommand(string.Format("DM{0}?", traceID)); 

            if (!this._connect.WaitAndGetData(out rtnStr) || rtnStr == string.Empty)
            {
                return false;
            }

            strArray = rtnStr.Replace("\r\n", ",").Split(',');


            double step = (endWl - startWl) / samplingPoints;

            step = Math.Round(step, 3, MidpointRounding.AwayFromZero);

            for (int idx = 0; idx < samplingPoints; idx++)
            {
                this._msrtDataArray[(int)index].Wavelength.Add(startWl + step * idx);

                double tempSpectrum = 0.0d;

                double.TryParse(strArray[idx], out tempSpectrum);

                this._msrtDataArray[(int)index].Spectrum.Add(tempSpectrum);
            }

            //pt.Stop();
            //Console.WriteLine("[OSA, DMA? Time = ]" + pt.GetTimeSpan(ETimeSpanUnit.MilliSecond).ToString("0.000"));


            //-----------------------------------------------------------------------------------------
            //pt.Reset();
            //pt.Start();

           

            //pt.Stop();
            //Console.WriteLine("[OSA, APR? Time = ]" + pt.GetTimeSpan(ETimeSpanUnit.MilliSecond).ToString("0.000"));

            if (this._config.IsSaveRawData)
            {
                this.SaveData();
            }

            return isValid;
        }

        public void Close()
        {
            if (this._connect != null)
            {
                this._connect.Close();
            }
        }

        public void Reset()
        {

        }

        public void SaveData()
        {
            if (this._connect == null)
            {
                return;
            }

            //this._connect.SendCommand(string.Format("PRINT \"{0}\", {1}", "ScreenShot", "D"));
            this._connect.SendCommand("PRINT");
            this._connect.SendCommand("SVCSV");
        }

        public void SweepRepeat()
        {
            if (this._connect == null)
            {
                return;
            }

            this._connect.SendCommand("SRT");  // Repeat Sweep
        }

        public void SweepStop()
        {
            if (this._connect == null)
            {
                return;
            }

            this._connect.SendCommand("SST");   // Stop Sweep
        }

        #endregion
    }

    internal enum ETraceType
    {
        WRITE,
        FIX,
        BLANK,
    }
}
