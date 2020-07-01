using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using MPI.Tester;
using MPI.Tester.Data;
using MPI.Tester.CompoCommon;
using MPI.Tester.Compo.DIDOCard;
using MPI.Tester.Compo.ADCard;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Device.LaserSourceSys;
using MPI.Tester.Tools;
using MPI.Tester.Device.SourceMeter;
using MPI.Tester.Device.SpectroMeter;
using MPI.Tester.Device.ESD;
using MPI.Tester.Device.LCRMeter;
using MPI.Tester.Device.SwitchSystem;
using MPI.Tester.Device.IOCard;
using MPI.Tester.Maths.ColorMath;
using MPI.Tester.Device.PostCalc;
using MPI.Tester.Device.Pulser;
using System.Threading;


namespace MPI.Tester.TestKernel
{
    public abstract partial class TesterKernelBase
    {
        #region >>> Pivate Property <<<


        #endregion

        #region >>> Protected Property <<<

        protected object _lockObj;

        protected ConditionCtrl _condCtrl;
        protected SmartBinning _binCtrl;
        protected ProductData _product;
        protected MachineConfig _machineConfig;
        protected TesterSetting _sysSetting;
        protected RDFunc _rdFunc;
        protected SystemCali _sysCali;

        protected PortAccess _IOPort;


        protected MPI.PerformanceTimer _pt1;
        protected MPI.PerformanceTimer _ptOpenShortTimeOut;
        protected PerformanceTimer _ptTestTime;
        protected MPI.PerformanceTimer _sysDelayTimer;

        protected IESDDevice _esdDevice;
        protected ISourceMeter _srcMeter;
        protected ISpectroMeter _sptMeter;
        protected ISwitch _switchDevice;
        protected IADCard _ADCard;
        protected ILCRMeter _lcrMeter;
        protected ISourceMeter _lcrBias;
        protected IOSA _osaDevice;
        protected ILaserPostCalc _laserPostCalc;

        protected TestResultData[] _sysResultItem;

        protected bool _isMoveDataToStorage;
        protected bool _isTestSuccess;
        protected double _chipPolarity;
        protected int _darkCorrectCount;
        protected uint _testSequencElecCount;
        protected double[] _darkSample = null;
        protected Dictionary<string, double> _binCalcData;
        protected int _skipGetDarkCounts;
        protected bool _isStopTest;
        protected DataVerify _dataVerify;
        protected bool _isTriggerSptErr;
        protected List<int> _repeatTestIndexs;
        protected uint _skipCount;
        protected List<uint> _srcSyncTrigger;
        protected int _preTypeState;
        protected uint _testSquenceOptCount;

        protected bool _isTVSTesting = false;
        protected bool _isQCSetting = false;
        protected bool _isManualTest = false;
        protected double _detectorPolarity = 1;
        protected PassRateCheck _passRateCheck;

        protected AdjacentCheck _adjacentCheck;

        protected TestItemData _ESDContactCheckItem;

        protected KernelCmdData _cmdData;
        protected AcquireData _acquireData;
        protected bool _isTesterKernelOpen;
        protected SystemStatus _status;
        protected MachineInfoData _machineInfo;
        protected ReTestManager _rManager;

        public LaserSourceSystem _laserSrcSys;
        public CoordTransferTool P2TcoordTransTool;
        //for multiDie
        protected bool[] _isChannelHasDie;

        protected MPI.PerformanceTimer _timerrrr;

        protected double _darkMeam = 0;

        protected TestItemData _openShortContactItem;

        protected int[] _chipGroup;

        protected KernelSequenceManagement _seqManagement;

        protected IOStateCheck _ioStateCheck;

        #endregion

        #region >>> Public Event Handler <<<

        public event EventHandler<EventArgs> FinishTestAndCalcEvent;
        public event ErrorCodeEventHandler ErrorCodeEvent;

        #endregion

        #region >>> Constructor / Disposor <<<

        public TesterKernelBase()
        {
            this._lockObj = new object();

            this._acquireData = new AcquireData();

            this._machineInfo = new MachineInfoData();

            this._rdFunc = new RDFunc();

            this._isTestSuccess = false;

            string[] strArray = Enum.GetNames(typeof(ESysResultItem));

            this._sysResultItem = new TestResultData[strArray.Length];

            for (int i = 0; i < this._sysResultItem.Length; i++)
            {
                TestResultData data = new TestResultData(strArray[i], strArray[i], "", "0");
                data.IsSystemItem = true;
                this._sysResultItem[i] = data;
            }

            this._chipPolarity = 1.0d;

            this._testSequencElecCount = 0;

            this._srcSyncTrigger = new List<uint>();

            this._pt1 = new PerformanceTimer();
            this._ptOpenShortTimeOut = new PerformanceTimer();

            this._cmdData = new KernelCmdData(40);

            this._isTesterKernelOpen = false;
            this._status = new SystemStatus();
            this._ptTestTime = new PerformanceTimer();

            GlobalFlag.TestMode = ETesterTestMode.Normal;
            this._binCalcData = new Dictionary<string, double>(50);
            this._skipGetDarkCounts = 200;
            this._dataVerify = new DataVerify();
            this._isTriggerSptErr = false;
            this._repeatTestIndexs = new List<int>();
            this._skipCount = 0;

            this._passRateCheck = new PassRateCheck();

            _timerrrr = new PerformanceTimer();

            _laserSrcSys = new LaserSourceSystem();

            P2TcoordTransTool = null;

            _rManager = new ReTestManager();

            _openShortContactItem = null;

            _ioStateCheck = new IOStateCheck();
        }

        #endregion

        #region >>> Public Property <<<

        public KernelCmdData CmdData
        {
            get { return this._cmdData; }
            //set { lock (this._lockObj) { this._cmdData = value; } }
        }

        public AcquireData Data
        {
            get { return this._acquireData; }
        }

        public MachineInfoData MachineInfo
        {
            get { return this._machineInfo; }
        }

        public SystemStatus Status
        {
            get { return this._status; }
        }

        public int TesterCoord { get { return this._sysSetting.TesterCoord; } }
        #endregion

        #region >>> Public Method <<<

        public void Stop()
        {
            if (this._status.State == EKernelState.Running)
            {
                this._status.State = EKernelState.Ready;
            }
        }

        public double RunAttMoniter(AttenuatorSettingData attSet)
        {
            double val = 0;
            if (SetAttenuator(attSet))
            {
                val = this._laserSrcSys.AttManager.GetMsrtPower(attSet.SysChannel,ELaserPowerUnit.dBm);
                if (this._laserSrcSys.AttManager.ErrorNumber != EDevErrorNumber.Device_NO_Error)
                {
                    this._laserSrcSys.ErrorNumber = this._laserSrcSys.AttManager.ErrorNumber;
                    this.SetErrorCode(this._laserSrcSys.ErrorNumber);
                }
            }
            return val;
        }

        public double RunPowerMeter( PowerMeterSettingData pmSet)
        {
             double val = 0;
            if (this._laserSrcSys != null &&
                this._laserSrcSys.PowerMeterManager != null )
            {
                val = this._laserSrcSys.PowerMeterManager.GetMsrtPower(pmSet, (int)_chipPolarity);
                if (this._laserSrcSys.PowerMeterManager.ErrorNumber != EDevErrorNumber.Device_NO_Error)
                {
                    this._laserSrcSys.ErrorNumber = this._laserSrcSys.PowerMeterManager.ErrorNumber;
                    this.SetErrorCode(this._laserSrcSys.ErrorNumber);
                }
            }
            return val;
        }

        public bool SetAttenuator(AttenuatorSettingData attSet)
        {
            if (this._laserSrcSys != null &&
              this._laserSrcSys.AttManager != null)
            {
                List<AttenuatorSettingData> aList = new List<AttenuatorSettingData>();
                aList.Add(attSet);
                this._laserSrcSys.SetParamToAttenuator(aList);
                if (this._laserSrcSys.AttManager.ErrorNumber != EDevErrorNumber.Device_NO_Error)
                {
                    this._laserSrcSys.ErrorNumber = this._laserSrcSys.AttManager.ErrorNumber;
                    this.SetErrorCode(this._laserSrcSys.ErrorNumber);
                    return false;
                }
            }
            return true;
        }

        public bool Switch2OpticalCh(int sysCh)
        {
            if (this._laserSrcSys != null &&
                this._laserSrcSys.OSManager != null)
            {
                if (!this._laserSrcSys.SetParamToOS(sysCh))
                {
                    this.SetErrorCode(this._laserSrcSys.ErrorNumber);
                }
            }
            return true;
        }

        public bool RunAutoLaserCompensate(out string compensateSetStr)
        {
            string pOutStr = "", aOutStr = "",tOutStr = "";
            compensateSetStr = "";
            bool result = true;
            if (this._condCtrl.Data != null &&
    this._condCtrl.Data.TestItemArray != null)
            {                
                //foreach (TestItemData item in this._condCtrl.Data.TestItemArray)
                for(int i = 0 ; i  <this._condCtrl.Data.TestItemArray.Length;++i)
                {
                    TestItemData item = this._condCtrl.Data.TestItemArray[i];
                    if (item.IsEnable && item.Type == ETestType.LaserSource)
                    {
                        string pStr = "", aStr = "", tStr = "";
                        double pmVal = -1;
                        if (item.MsrtResult != null && item.MsrtResult.Length >= 2)
                        {
                            pmVal = item.MsrtResult[1].Value;
                        }
                        TestItemData tItem = RunAutoLaserCompensate(item as LaserSourceTestItem, out aStr, out pStr, out tStr, pmVal);
                        this._condCtrl.Data.TestItemArray[i] = tItem != null ? tItem : this._condCtrl.Data.TestItemArray[i];
                        if (tItem == null)
                        {
                            result = false;
                        }
                        pOutStr += pStr + ",";
                        aOutStr += aStr + ",";
                        tOutStr += tStr + ",";
                    }
                }
                compensateSetStr = aOutStr + pOutStr + tOutStr;
            }

            return result;

        }

        public LaserSourceTestItem RunAutoLaserCompensate(LaserSourceTestItem item, out string AttStr, out string PMStr, out string timeStr, double pmVal = -1)
        {
            LaserSourceTestItem rItem = item.Clone() as LaserSourceTestItem;
            int loopMaxTimes = 10;
            double p_gain = 0.99;
            double i_gain = 0.001;
            double errLog = 0;
            AttStr = "";
            PMStr = "";
            timeStr = "";

            MPI.PerformanceTimer ptimer = new PerformanceTimer();

            if (rItem.LaserSourceSet != null && rItem.LaserSourceSet.AutoTuneVOASetting != null)
            {
                LaserSourceSysSettingData lss = rItem.LaserSourceSet;
                string pName = rItem.MsrtResult[1].Name;
                string aName = rItem.MsrtResult[2].Name;
                ptimer.Start();
                Switch2OpticalCh(lss.SysChannel);
                if (item.ElecSetting != null && item.ElecSetting.Length > 0)
                {
                    System.Threading.Thread.Sleep((int)item.ElecSetting[0].ForceDelayTime);
                }
                //SetAttenuator(lss.AttenuatorData);
                bool needSet =true;
                int i = 0;
                double tarVal = lss.PowerMeterSetting.TarPower;
                if (pmVal < 0)//若有從外部給power meter量測值，就不須再測一次
                {
                    pmVal = RunPowerMeter(lss.PowerMeterSetting);
                }
                //double errLim = lss.PowerMeterSetting.Tolerence / 100;
                double trigerLim = rItem.LaserSourceSet.AutoTuneVOASetting.TuneVOATriggerLimit / 100;
                double tMax = tarVal * (1 + trigerLim);
                double tMin = tarVal * (1 - trigerLim);
                double abs_Val = Math.Abs(pmVal);
                if (tMin < abs_Val && abs_Val < tMax)
                {
                    i = 0;// in spec 不須進行 Auto Tune                    
                }
                else
                {
                    //由於觸發誤差 = 收斂範圍，會出現收斂完下一點馬上又跑掉的狀況
                    //因此將收斂範圍設定為觸發範圍的一半
                    //根據經驗，1080會有1%左右的高頻震盪，因此收斂範圍的下限設為1%，避免系統追著高頻震盪無法收斂
                    double errTol = Math.Abs(rItem.LaserSourceSet.AutoTuneVOASetting.TuneVOATriggerLimit / 100);
                    double errLim = errTol > 0.01 ? errTol / 2 : 0.01;
                    double tMax_2 = tarVal * (1 + errLim);
                    double tMin_2 = tarVal * (1 - errLim);

                    for (i = 0; i < loopMaxTimes && needSet; ++i)
                    {
                        pmVal = RunPowerMeter(lss.PowerMeterSetting);
                        if (tarVal != 0)
                        {
                            double r_tRatial = pmVal / tarVal;
                            double r_tDiff = pmVal - tarVal;
                            abs_Val = Math.Abs(pmVal);
                            if (tMin_2 < abs_Val && abs_Val < tMax_2)
                            {
                                needSet = false;
                                break;
                            }
                            else
                            {
                                if (lss.AttenuatorData.APMode == EAPMode.Attenuator)
                                {
                                    double shiftdB = MPI.Tester.Maths.UnitMath.Decimal2dB(r_tRatial * p_gain);
                                    double tempB = shiftdB;
                                    shiftdB += errLog;//PI- controll
                                    lss.AttenuatorData.Attenuate.Set += shiftdB;//衰減越大光越小，所以加上誤差dB值
                                    errLog += tempB * i_gain;
                                }
                                else
                                {
                                    double shiftPow = r_tDiff;
                                    double tempB = r_tDiff;
                                    lss.AttenuatorData.Power.Set -= r_tDiff * p_gain + errLog;//抵銷衰減   //PI- controll
                                    errLog += tempB * i_gain;
                                }
                                SetAttenuator(lss.AttenuatorData);
                                //System.Threading.Thread.Sleep(200);//確保att已切完;改為在ATT內延遲
                            }

                        }
                    }
                }
                //ptimer.Stop();//include switch , first power meter check and auto tune for loop
                timeStr = rItem.Name + "_Loop:" + i.ToString();// +"," + rItem.Name + "_Span(ms):" + ptimer.GetTimeSpan(ETimeSpanUnit.MilliSecond);
                AttStr += aName + "_Set:" + lss.AttenuatorData.Attenuate.Set.ToString("E3") ;
                PMStr += pName + "_After:" + pmVal.ToString("E3") ;
                if (i >= loopMaxTimes)
                {
                    this.SetErrorCode(EErrorCode.LASER_AutoSetAttenuator_Fail_Err, timeStr + "," + AttStr +","+ PMStr);
                    rItem = null;
                }

            }
            
            return rItem;
        }

        public string GetAttMoniterInfo()
        {
            string outStr = "";
            if (this._laserSrcSys != null &&
                this._laserSrcSys.AttManager != null &&
                this._condCtrl.Data != null &&
                this._condCtrl.Data.TestItemArray != null)
            {
                foreach (TestItemData item in this._condCtrl.Data.TestItemArray)
                {
                    if (item.IsEnable && item.Type == ETestType.LaserSource && (item as LaserSourceTestItem).LaserSourceSet.AttenuatorData != null)
                    {
                        int sysCh = (item as LaserSourceTestItem).LaserSourceSet.AttenuatorData.SysChannel;
                        
                        IAttenuator att = this._laserSrcSys.AttManager.GetAttFromSysCh(sysCh);


                        string name = (item as LaserSourceTestItem).LaserSourceSet.ChName;
                        if (item.MsrtResult != null && item.MsrtResult.Length >= 3)
                        {
                            name = item.MsrtResult[2].Name;
                        }
                        outStr += name + ":";

                        if (att != null && att.Spec.HavePowerControlMode)
                        {
                            Switch2OpticalCh(sysCh);
                            if (item.ElecSetting != null && item.ElecSetting.Length > 0)
                            {
                                System.Threading.Thread.Sleep((int)item.ElecSetting[0].ForceDelayTime);
                            }
                            //outStr += (item as LaserSourceTestItem).LaserSourceSet.ChName + ":";

                            double val = this._laserSrcSys.AttManager.GetMsrtPower(sysCh, ELaserPowerUnit.dBm);//這邊單位跟上面不一樣!
                            if (item.MsrtResult != null && item.MsrtResult.Length >= 3)
                            {
                                item.MsrtResult[2].RawValue = val;
                                item.MsrtResult[2].Value = val;
                            }
                            outStr += val.ToString("E3") + ",";
                        }
                        else
                        {
                            outStr += (0).ToString("E3") + ",";
                        }
                    }
                }
            }
            return outStr;
        }

        public string GetPowerMeterInfo(ref bool isPass, out string errMsg)
        {
            isPass = true;
            string outStr = "";
            errMsg = "";
            if (this._laserSrcSys != null &&
                this._laserSrcSys.PowerMeterManager != null &&
                this._condCtrl.Data != null &&
                this._condCtrl.Data.TestItemArray != null)
            {
                foreach (TestItemData item in this._condCtrl.Data.TestItemArray)
                {
                    if (item.IsEnable && item.Type == ETestType.LaserSource && (item as LaserSourceTestItem).LaserSourceSet.PowerMeterSetting != null)
                    {
                        int sysCh = (item as LaserSourceTestItem).LaserSourceSet.PowerMeterSetting.SysChannel;
                        
                        IPowerMeter pm = this._laserSrcSys.PowerMeterManager.GetPowerMeterFromSysCh(sysCh);

                        string name = (item as LaserSourceTestItem).LaserSourceSet.ChName;
                        if (item.MsrtResult != null && item.MsrtResult.Length >= 2)
                        {
                            name = item.MsrtResult[1].Name;
                        }
                        outStr += name + ":";

                        if (pm != null)
                        {
                            Switch2OpticalCh(sysCh);
                            if (item.ElecSetting != null && item.ElecSetting.Length > 0)
                            {
                                System.Threading.Thread.Sleep((int)item.ElecSetting[0].ForceDelayTime);
                            }

                            double val = this._laserSrcSys.PowerMeterManager.GetMsrtPower((item as LaserSourceTestItem).LaserSourceSet.PowerMeterSetting, (int)this._chipPolarity);

                            //double val = pm.GetMsrtPower((item as LaserSourceTestItem).LaserSourceSet.PowerMeterSetting, (int)this._chipPolarity);
                            if (item.MsrtResult != null && item.MsrtResult.Length >= 2)
                            {
                                item.MsrtResult[1].RawValue = val;
                                item.MsrtResult[1].Value = val;
                            }
                            double tarVal = (item as LaserSourceTestItem).LaserSourceSet.PowerMeterSetting.TarPower;
                            double errLim = (item as LaserSourceTestItem).LaserSourceSet.PowerMeterSetting.Tolerence / 100;//%
                            double uLimit = Math.Abs(tarVal * (1 + errLim));
                            double lLimit = Math.Abs(tarVal * (1 - errLim));
                            outStr += val.ToString("E3") + ",";
                            if (val < lLimit ||uLimit < val)
                            {
                                isPass = false;
                                errMsg += outStr;
                            }
                        }
                        else
                        {
                            outStr +=(0).ToString("E3")+",";
                        }
                    }
                }
                //if (outStr != "")
                //{
                //    outStr = "PowerMeter:" + outStr.TrimEnd(',');
                //}

                //if (!isPass)
                //{
                //    this.SetErrorCode(EErrorCode.LASER_PowerMeter_CheckPower_Fail_Err, errMsg);

                //    Console.WriteLine("[TesterKernel_Base], SourceMeterDevice Init Err");
                //}
            }            
            return outStr;
        }

        public MinMaxValuePair<double> GetAttPowRangeIndBm(int sysCh)
        {
            MinMaxValuePair<double> range = new MinMaxValuePair<double>(-70, 10);
            if (this._laserSrcSys != null &&
                this._laserSrcSys.AttManager != null)
            {
                range = this._laserSrcSys.AttManager.GetOutputPowerRangeIndBm(sysCh);
            }
            return range;
        }

        public bool SetOpticalSwitchToDefault()
        {
            if ((this._laserSrcSys != null &&
                this._laserSrcSys.OSManager != null &&
                _machineConfig.LaserSrcSysConfig != null &&
                _machineConfig.LaserSrcSysConfig.ChConfigList != null))
            {
                foreach (var chCfg in _machineConfig.LaserSrcSysConfig.ChConfigList)
                {
                    if (chCfg.IsDefauleChannel)
                    {
                        Switch2OpticalCh(chCfg.SysChannel);
                    }
                }

            }
            return true;
        }

        public string GetAttSetInfo()
        {
            string outStr = "";
            if (this._laserSrcSys != null &&
                this._laserSrcSys.AttManager != null &&
                this._condCtrl.Data != null &&
                this._condCtrl.Data.TestItemArray != null)
            {
                foreach (TestItemData item in this._condCtrl.Data.TestItemArray)
                {
                    if (item.IsEnable && item.Type == ETestType.LaserSource && (item as LaserSourceTestItem).LaserSourceSet.AttenuatorData != null)
                    {
                        string name = (item as LaserSourceTestItem).LaserSourceSet.ChName;
                        if (item.MsrtResult != null && item.MsrtResult.Length >= 3)
                        {
                            name = item.MsrtResult[2].Name;
                        }
                        outStr += name + "_Set" + ":";

                        double setAttVal = (item as LaserSourceTestItem).LaserSourceSet.AttenuatorData.Attenuate.Set;
                        outStr += setAttVal.ToString("E3") + ",";

                    }
                }
            }
            return outStr;
        }

        public Dictionary<string, object> LoadDeviceRelayCnt()
        {
            if (_laserSrcSys != null)
            {
                _machineInfo.SNDeviceRelayDic = new Dictionary<string, object>();
                var devLogDic = _laserSrcSys.GetDevLog();
                if (devLogDic != null && _machineInfo.SNDeviceRelayDic != null)
                {
                    foreach (var dev in devLogDic)
                    {
                        _machineInfo.SNDeviceRelayDic.Add(dev.Key, (dev.Value as MPI.Tester.DeviceCommon.DeviceLogger.DeviceRelayInfoBase).Clone() as
                            MPI.Tester.DeviceCommon.DeviceLogger.DeviceRelayInfoBase);
                    }
                }
            }
            return _machineInfo.SNDeviceRelayDic;

        }

        public void ForceSetDeviceRelayCnt(Dictionary<string, object> devLogDic)//UI硬改
        {
            if (_laserSrcSys != null)
            {
                _laserSrcSys.SetDevLog(devLogDic);
                LoadDeviceRelayCnt();
            }
        }
      

        #endregion

        #region >>> Protected Method <<<
        
        protected void LoadMachineCfgFile()
        {
            string pathAndFile = System.IO.Path.Combine(Constants.Paths.DATA_FILE, Constants.Files.MACHINE_DATA);
            this._machineConfig = MPI.Xml.XmlFileSerializer.Deserialize(typeof(MachineConfig), pathAndFile) as MachineConfig;

            if (this._machineConfig == null)
            {
                this._machineConfig = new MachineConfig();
            }
        }

        public void CheckMachineHW()
        {
            GlobalFlag.IsSuccessCheckFilterWheel = true;

            GlobalData.HwFilterWheelPos = -1;

            if (this._machineConfig.Enable.IsCheckFilterWheel == false)
            {
                return;
            }

            uint filterPos = 0;

            if (this._machineConfig.WheelMsrtSource == EWheelMsrtSource.LPT)
            {
                // 0x1F, 0x2F, 0x3F, 0x4F, 0x5F = Postion 1 ~ 5 , 0xFF = no signal input
                filterPos = (uint)this._IOPort.Input(0x379);
                filterPos = (filterPos >> 4);
            }
            else if (this._machineConfig.WheelMsrtSource == EWheelMsrtSource.SourceMeterIO)
            {
                if (this._srcMeter != null)
                {
                    filterPos = (uint)this._srcMeter.InputB(0);

                    GlobalData.HwFilterWheelPos = (int)filterPos;
                }
                else
                {
                    this.SetErrorCode(EErrorCode.SourceMeterDevice_Init_Err);

                    Console.WriteLine("[TesterKernel_Base], SourceMeterDevice Init Err");

                    GlobalFlag.IsSuccessCheckFilterWheel = false;

                    return;
                }
            }

            this._sysSetting.OptiDevSetting.AttenuatorPos = this._product.ProductFilterWheelPos;

            if (this._sysSetting.OptiDevSetting.AttenuatorPos != (filterPos - 1))     // 0-base
            {
                Console.WriteLine("[TesterKernel_Base], FilterWheel Is Not Match");
                this.SetErrorCode(EErrorCode.FilterWheelSettingErr);
                GlobalFlag.IsSuccessCheckFilterWheel = false;
            }
        }

        protected virtual void GetDarkDataAndSave(ISpectroMeter spt = null, string StageId = "")
        {
            if (spt == null)
            {
                spt = this._sptMeter;
            }
            double[] darkArray = spt.GetDarkSample(5, 8000);

            string pathAndFileNameWithExt = @"C:\MPI\LEDTester\Spectrometer\DarkArray" + StageId + ".dat";

            using (StreamWriter sw = new StreamWriter(pathAndFileNameWithExt, false))
            {
                foreach (double data in darkArray)
                {
                    sw.WriteLine(data.ToString());
                }
                sw.Close();
            }
        }

        protected virtual void WaitTimeOut(UInt32 timeOut)
        {
            _pt1.Start();

            do
            {
                if (Convert.ToInt32(_pt1.PeekTimeSpan(ETimeSpanUnit.MilliSecond)) >= timeOut)
                {
                    _pt1.Stop();
                    return;
                }

                System.Windows.Forms.Application.DoEvents();

            } while (Convert.ToInt32(_pt1.PeekTimeSpan(ETimeSpanUnit.MilliSecond)) < timeOut);

            _pt1.Stop();

        }

        protected virtual bool CheckAllDeviceErrorState(bool isSetErrUI = true)//MS 擴充
        {
            bool rtn = true;

            if (this._sptMeter != null)
            {
                if (this._sptMeter.ErrorNumber == EDevErrorNumber.Device_NO_Error)
                {
                    rtn &= true;
                }
                else
                {
                    rtn &= false;
                    if (isSetErrUI)
                        this.SetErrorCode(this._sptMeter.ErrorNumber);
                }
            }

            if (this._srcMeter != null)
            {
                if (this._srcMeter.ErrorNumber == EDevErrorNumber.Device_NO_Error)
                {
                    rtn &= true;
                }
                else
                {
                    rtn &= false;
                    if (isSetErrUI)
                        this.SetErrorCode(this._srcMeter.ErrorNumber);
                }
            }

            if (this._esdDevice != null)
            {
                if (this._esdDevice.ErrorNumber == EDevErrorNumber.Device_NO_Error)
                {
                    rtn &= true;
                }
                else
                {
                    rtn &= false;
                    if (isSetErrUI)
                        this.SetErrorCode(this._esdDevice.ErrorNumber);
                }
            }

            if (this._lcrMeter != null)
            {
                if (this._lcrMeter.ErrorNumber == EDevErrorNumber.Device_NO_Error)
                {
                    rtn &= true;
                }
                else
                {
                    rtn &= false;
                    if (isSetErrUI)
                        this.SetErrorCode(this._lcrMeter.ErrorNumber);
                }
            }

            if (this._lcrBias != null)
            {
                if (this._lcrBias.ErrorNumber == EDevErrorNumber.Device_NO_Error)
                {
                    rtn &= true;
                }
                else
                {
                    rtn &= false;
                    if (isSetErrUI)
                        this.SetErrorCode(this._lcrBias.ErrorNumber);
                }
            }

            if (this._switchDevice != null)
            {
                if (this._switchDevice.ErrorNumber == EDevErrorNumber.Device_NO_Error)
                {
                    rtn &= true;
                }
                else
                {
                    rtn &= false;
                    if (isSetErrUI)
                        this.SetErrorCode(this._switchDevice.ErrorNumber);
                }
            }

            return rtn;
        }

        protected virtual void ResetKernelData()
        {
            //------------------------------------------------------------------------------------------------------
            // (1 Add TestResultData from "TestItemArray" of condtion data
            //------------------------------------------------------------------------------------------------------
            if (this._condCtrl.Data != null && this._condCtrl.Data.TestItemArray != null)
            {
                this._acquireData.SetData(this._condCtrl.Data, this._sysResultItem);
            }

            if (this._machineConfig.LaserSrcSysConfig != null)
            {
                if (this._machineConfig.LaserSrcSysConfig.Attenuator.AttenuatorModel != ELaserAttenuatorModel.NONE)
                {
                    DeviceRunTimeData drtData = new DeviceRunTimeData("ATT", "ATTPower", 1);

                    this._acquireData.DeviceRunTimeDataSet.Add(drtData);
                }
                if(this._machineConfig.LaserSrcSysConfig.MoniterPDSMU != null )
                {
                    DeviceRunTimeData drtData = new DeviceRunTimeData("SMU", "PdMoniterCurr", 1);

                    this._acquireData.DeviceRunTimeDataSet.Add(drtData);
                }
            }

            //--------------------------------------------------------------------------------------------------------
            // (2) Set the variable of system kernel  
            //--------------------------------------------------------------------------------------------------------
            this._darkCorrectCount = 0;
            this._isMoveDataToStorage = true;
            this._acquireData.ChipInfo.GoodDieCount = 0;
            this._acquireData.ChipInfo.FailDieCount = 0;
            this._acquireData.ChipInfo.TestCount = 0;
            // Paul Add
            this._sysResultItem[(int)ESysResultItem.TEST].Value = 0;

            //---------------------------------------------------------------------------------------
            // (3) Reset  Multi-Die Required information
            //---------------------------------------------------------------------------------------

            this._srcSyncTrigger.Clear();

            this._srcSyncTrigger.Add(0);  // Add Default srcMeter
        }

        protected virtual void SimulateProcess(int colX = int.MinValue, int rowY = int.MinValue)
        {
            double[][][] xyzt;
            if (colX == int.MinValue && rowY == int.MinValue)
            {
                rowY = -10;
                colX = 0;
                Math.DivRem((int)this._sysResultItem[(int)ESysResultItem.TEST].Value, 20, out colX);
                colX -= 10;

                rowY = (int)Math.Floor(this._sysResultItem[(int)ESysResultItem.TEST].Value / 20.0d);
                rowY -= 5;
            }

            Random random = new Random();

            if (this._condCtrl.Data.TestItemArray == null)
                return;

            bool isLoadPivRawData = false;

            List<double> pivPow = new List<double>();
            List<double> pivCurr = new List<double>();
            List<double> pivVolt = new List<double>();

            if (File.Exists(@"C:\MPI\LEDTester\Temp\simPIV.csv"))
            {
                List<string[]> lstTemp = MPI.Tester.CSVUtil.ReadCSV(@"C:\MPI\LEDTester\Temp\simPIV.csv");

                if (lstTemp != null)
                {
                    double current = 0.0d;
                    double voltage = 0.0d;
                    double power = 0.0d;

                    foreach (var row in lstTemp)
                    {
                        double.TryParse(row[0], out current);      // A
                        double.TryParse(row[1], out power);    // mW
                        double.TryParse(row[2], out voltage);  // V

                        pivPow.Add(power);
                        pivCurr.Add(current);
                        pivVolt.Add(voltage);
                    }

                    isLoadPivRawData = true;
                }
            }

            foreach (TestItemData item in this._condCtrl.Data.TestItemArray)
            {
                switch (item.Type)
                {
                    case ETestType.IF:
                        {
                            item.MsrtResult[0].RawValue = random.NextDouble() * 3.0d;
                            item.MsrtResult[0].Value = item.MsrtResult[0].RawValue;
                            break;
                        }
                    //------------------------------------------------------------------------------
                    case ETestType.LOPWL:
                        {
                            foreach (TestResultData data in item.MsrtResult)
                            {
                                data.RawValue = random.NextDouble() * 1000.0d;
                                data.Value = data.RawValue;
                            }
                            item.MsrtResult[(int)EOptiMsrtType.CIEx].RawValue = random.NextDouble();
                            item.MsrtResult[(int)EOptiMsrtType.CIEy].RawValue = random.NextDouble();
                            item.MsrtResult[(int)EOptiMsrtType.CIEx].Value = item.MsrtResult[(int)EOptiMsrtType.CIEx].RawValue;
                            item.MsrtResult[(int)EOptiMsrtType.CIEy].Value = item.MsrtResult[(int)EOptiMsrtType.CIEy].RawValue;
                            break;
                        }
                    //------------------------------------------------------------------------------
                    case ETestType.PIV:
                        {
                            if (isLoadPivRawData)
                            {
                                #region >>> Laser Characteristic Calculation <<<

                                if (this._laserPostCalc == null)
                                {
                                    this._laserPostCalc = new MpiLaserPostCalc();
                                }


                                this._laserPostCalc.CalibratedPowerFactor = _product.PdDetectorFactor;

                                this._laserPostCalc.SettingData = (item as PIVTestItem).CalcSetting.Clone() as LaserCalcSetting;


                                this._laserPostCalc.CalcParameter(pivPow.ToArray(), pivCurr.ToArray(), pivVolt.ToArray());

                                item.MsrtResult[(int)ELaserMsrtType.Pop].Value = this._laserPostCalc.CharacteristicResults.Pop;
                                item.MsrtResult[(int)ELaserMsrtType.Iop].Value = this._laserPostCalc.CharacteristicResults.Iop * 1000.0d;     // A -> mA
                                item.MsrtResult[(int)ELaserMsrtType.Vop].Value = this._laserPostCalc.CharacteristicResults.Vop;  // V -> V
                                item.MsrtResult[(int)ELaserMsrtType.Pceop].Value = this._laserPostCalc.CharacteristicResults.Pceop;  // %
                                item.MsrtResult[(int)ELaserMsrtType.Imop].Value = this._laserPostCalc.CharacteristicResults.Imop * 1000.0d;     // A -> mA

                                item.MsrtResult[(int)ELaserMsrtType.Ipk].Value = this._laserPostCalc.CharacteristicResults.Ipk * 1000.0d;     // A -> mA
                                item.MsrtResult[(int)ELaserMsrtType.Ppk].Value = this._laserPostCalc.CharacteristicResults.Ppk;   // mW       
                                item.MsrtResult[(int)ELaserMsrtType.Vpk].Value = this._laserPostCalc.CharacteristicResults.Vpk;   // V   
                                item.MsrtResult[(int)ELaserMsrtType.Impk].Value = this._laserPostCalc.CharacteristicResults.Impk * 1000.0d;     // A -> mA
                                item.MsrtResult[(int)ELaserMsrtType.Pcepk].Value = this._laserPostCalc.CharacteristicResults.Pcepk;  // %

                                item.MsrtResult[(int)ELaserMsrtType.Pth].Value = this._laserPostCalc.CharacteristicResults.Pth; // mW
                                item.MsrtResult[(int)ELaserMsrtType.Ith].Value = this._laserPostCalc.CharacteristicResults.Ith * 1000.0d; // A -> mA
                                item.MsrtResult[(int)ELaserMsrtType.Vth].Value = this._laserPostCalc.CharacteristicResults.Vth; // V

                                item.MsrtResult[(int)ELaserMsrtType.SE].Value = this._laserPostCalc.CharacteristicResults.SE;   // W/A
                                item.MsrtResult[(int)ELaserMsrtType.SE2].Value = this._laserPostCalc.CharacteristicResults.SE2;   // W/A

                                item.MsrtResult[(int)ELaserMsrtType.RS].Value = this._laserPostCalc.CharacteristicResults.Rs;   // ohm 

                                item.MsrtResult[(int)ELaserMsrtType.Kink].Value = this._laserPostCalc.CharacteristicResults.Kink;
                                item.MsrtResult[(int)ELaserMsrtType.Ikink].Value = this._laserPostCalc.CharacteristicResults.Ikink * 1000.0d; // A -> mA
                                item.MsrtResult[(int)ELaserMsrtType.Pkink].Value = this._laserPostCalc.CharacteristicResults.Pkink;
                                // item.MsrtResult[(int)ELaserMsrtType.Icod].Value = this._laserPostCalc.CharacteristicResults.Icod * 1000.0d;  // A -> mA
                                // item.MsrtResult[(int)ELaserMsrtType.Pcod].Value = this._laserPostCalc.CharacteristicResults.Pcod;
                                item.MsrtResult[(int)ELaserMsrtType.Linearity].Value = this._laserPostCalc.CharacteristicResults.Linearity;  // %
                                item.MsrtResult[(int)ELaserMsrtType.Linearity2].Value = this._laserPostCalc.CharacteristicResults.Linearity2;  // %

                                item.MsrtResult[(int)ELaserMsrtType.Rollover].Value = this._laserPostCalc.CharacteristicResults.Rollover;  // %

                                item.MsrtResult[(int)ELaserMsrtType.Icod].Value = this._laserPostCalc.CharacteristicResults.Icod * 1000.0d; // A -> mA
                                item.MsrtResult[(int)ELaserMsrtType.Pcod].Value = this._laserPostCalc.CharacteristicResults.Pcod;  // mW

                                item.MsrtResult[(int)ELaserMsrtType.PfA].Value = this._laserPostCalc.CharacteristicResults.PfA;
                                item.MsrtResult[(int)ELaserMsrtType.VfA].Value = this._laserPostCalc.CharacteristicResults.VfA;  // V -> V
                                item.MsrtResult[(int)ELaserMsrtType.RdA].Value = this._laserPostCalc.CharacteristicResults.RdA;   // ohm 
                                item.MsrtResult[(int)ELaserMsrtType.PceA].Value = this._laserPostCalc.CharacteristicResults.PceA;  // %

                                item.MsrtResult[(int)ELaserMsrtType.PfB].Value = this._laserPostCalc.CharacteristicResults.PfB;
                                item.MsrtResult[(int)ELaserMsrtType.VfB].Value = this._laserPostCalc.CharacteristicResults.VfB;  // V -> V
                                item.MsrtResult[(int)ELaserMsrtType.RdB].Value = this._laserPostCalc.CharacteristicResults.RdB;   // ohm 
                                item.MsrtResult[(int)ELaserMsrtType.PceB].Value = this._laserPostCalc.CharacteristicResults.PceB;  // %

                                item.MsrtResult[(int)ELaserMsrtType.PfC].Value = this._laserPostCalc.CharacteristicResults.PfC;
                                item.MsrtResult[(int)ELaserMsrtType.VfC].Value = this._laserPostCalc.CharacteristicResults.VfC;  // V -> V
                                item.MsrtResult[(int)ELaserMsrtType.RdC].Value = this._laserPostCalc.CharacteristicResults.RdC;   // ohm 
                                item.MsrtResult[(int)ELaserMsrtType.PceC].Value = this._laserPostCalc.CharacteristicResults.PceC;  // %

                                this._acquireData.PIVDataSet[item.KeyName].PowerData = pivPow.ToArray();
                                this._acquireData.PIVDataSet[item.KeyName].CurrentData = pivCurr.ToArray();
                                this._acquireData.PIVDataSet[item.KeyName].VoltageData = pivVolt.ToArray();
                                this._acquireData.PIVDataSet[item.KeyName].SeData = this._laserPostCalc.Curve.SeData;
                                this._acquireData.PIVDataSet[item.KeyName].RsData = this._laserPostCalc.Curve.RsData;
                                this._acquireData.PIVDataSet[item.KeyName].PceData = this._laserPostCalc.Curve.PceData;

                                #endregion
                            }

                            break;
                        }
                    default:
                        {
                            if (item.MsrtResult != null)
                            {
                                foreach (TestResultData data in item.MsrtResult)
                                {
                                    data.RawValue = random.NextDouble() * 1000.0d;
                                    data.Value = data.RawValue;
                                }
                            }

                            break;
                        }
                }


            }

            foreach (TestResultData data in this._acquireData.OutputTestResult)
            {
                if (data.KeyName == EProberDataIndex.COL.ToString())
                {
                    data.Value = (double)colX;
                    this._acquireData.ChipInfo.ColX = (int)colX;
                }
                else if (data.KeyName == EProberDataIndex.ROW.ToString())
                {
                    data.Value = (double)rowY;
                    this._acquireData.ChipInfo.RowY = (int)rowY;
                }
            }

            if (this._acquireData.ElecSweepDataSet.Count != 0)
            {
                xyzt = new double[this._acquireData.ElecSweepDataSet.Count][][];

                for (int i = 0; i < this._acquireData.ElecSweepDataSet.Count; i++)
                {
                    xyzt[i] = new double[3][];

                    xyzt[i][0] = new double[1000];
                    xyzt[i][1] = new double[1000];
                    xyzt[i][2] = new double[1000];
                }

                int keyIndex = 0;
                double scale = 1.0d;

                foreach (string s in this._acquireData.ElecSweepDataSet.KeyNames)
                {
                    for (int j = 0; j < xyzt[0][0].Length; j++)
                    {
                        xyzt[keyIndex][0][j] = (j + colX * 20) * scale;
                        xyzt[keyIndex][1][j] = scale * (Math.Sin((j / 1000.0d) * 2 * Math.PI) + scale * Math.Cos(Math.PI + scale));
                        xyzt[keyIndex][2][j] = scale * 100 * (Math.Sin((j / 1000.0d) * 4 * Math.PI + 0.2) + 0.5 * Math.Cos(scale + Math.PI));
                    }

                    keyIndex++;

                    scale += 2.0d;
                }
            }
        }

        protected virtual void CalcBinGrade(TestItemData[] itemArray = null)
        {
            int binSN = 0;

            int finalBinGrade;

            bool isAllItemPass = true;

            bool isAllItemPass02 = true;

            bool isAllItemPass03 = true;

            if (itemArray == null)
            {
                itemArray = this._condCtrl.Data.TestItemArray;
                if (itemArray == null)
                    return;
            }

            this._binCalcData.Clear();
            if (!GlobalFlag.IsReSingleTestMode)
            {
                SolveRetestDUT();
            }

            lock (this)
            {
                if (this._isTestSuccess == true)
                {
                    foreach (TestItemData item in itemArray)
                    {
                        if (item.MsrtResult != null)
                        {
                            int index = 0;

                            foreach (TestResultData data in item.MsrtResult)
                            {
                                string formatValue = data.Value.ToString(data.Formate);

                                //this._binCalcData.Add(data.KeyName, Convert.ToDouble(formatValue));///???

                                if (data.IsEnable && data.IsVerify && data.IsVision && data.IsTested)//20180702 David
                                {
                                    isAllItemPass &= data.IsPass;
                                }

                                if (data.IsEnable && data.IsVerify && data.IsVision && data.IsTested)
                                {
                                    isAllItemPass02 &= data.IsPass02;
                                }

                                if (data.IsEnable && data.IsVerify && data.IsVision && data.IsTested)
                                {
                                    isAllItemPass03 &= data.IsPass03;
                                }

                                if (data.IsEnable && data.IsTested)
                                //if (data.IsEnable )
                                {
                                    this._binCalcData.Add(data.KeyName, Convert.ToDouble(formatValue));
                                }
                                else
                                {
                                    string str = data.KeyName;
                                }
                                index++;
                            }
                        }
                    }

                    finalBinGrade = this._binCtrl.CalculateBin(this._binCalcData, (int)this._sysSetting.BinSortingRule, out binSN); // 1-base Gilbert Error
                }
                else
                {
                    finalBinGrade = 0;	// TestProcess == fail , return fail bin  Gilbert Error
                    isAllItemPass = false;
                    isAllItemPass02 = false;
                    isAllItemPass03 = false;
                }
            }


            if (_sysSetting.IsCalcANSIAndGB)
            {
                foreach (TestItemData item in this._condCtrl.Data.TestItemArray)
                {
                    if (!(item is LOPWLTestItem) || item.MsrtResult == null)
                    {
                        continue;
                    }

                    TestResultData cieXItem = null;

                    TestResultData cieYItem = null;

                    TestResultData ansiSDCMItem = null;

                    TestResultData ansiNearCCTItem = null;

                    TestResultData ansiNearSDCMItem = null;

                    TestResultData gbSDCMItem = null;

                    TestResultData gbNearCCTItem = null;

                    TestResultData gbNearSDCMItem = null;

                    foreach (TestResultData data in item.MsrtResult)
                    {
                        if (data.KeyName.Contains(EOptiMsrtType.CIEx.ToString()))
                        {
                            cieXItem = data;
                        }
                        else if (data.KeyName.Contains(EOptiMsrtType.CIEy.ToString()))
                        {
                            cieYItem = data;
                        }
                        else if (data.KeyName.Contains(EOptiMsrtType.ANSISDCM.ToString()))
                        {
                            ansiSDCMItem = data;
                        }
                        else if (data.KeyName.Contains(EOptiMsrtType.ANSINEARCCT.ToString()))
                        {
                            ansiNearCCTItem = data;
                        }
                        else if (data.KeyName.Contains(EOptiMsrtType.ANSINEARSDCM.ToString()))
                        {
                            ansiNearSDCMItem = data;
                        }
                        else if (data.KeyName.Contains(EOptiMsrtType.GBSDCM.ToString()))
                        {
                            gbSDCMItem = data;
                        }
                        else if (data.KeyName.Contains(EOptiMsrtType.GBNEARCCT.ToString()))
                        {
                            gbNearCCTItem = data;
                        }
                        else if (data.KeyName.Contains(EOptiMsrtType.GBNEARSDCM.ToString()))
                        {
                            gbNearSDCMItem = data;
                        }
                    }

                    int digitsX = cieXItem.Formate.Length - 2;

                    if (digitsX < 0)
                    {
                        digitsX = 0;
                    }

                    int digitsY = cieYItem.Formate.Length - 2;

                    if (digitsY < 0)
                    {
                        digitsY = 0;
                    }

                    double pX = Math.Round(cieXItem.Value, digitsX);

                    double pY = Math.Round(cieYItem.Value, digitsY);

                    int ansiSDCM = SDCMFunc.ANSI376(pX, pY, _sysSetting.ANSI376);

                    int gbSDCM = SDCMFunc.GB10682(pX, pY, _sysSetting.GB10682);

                    int ansiNearSDCM = 0;

                    int gbNearSDCM = 0;

                    EANSI376 ansiNearCCT = SDCMFunc.ANSI376(pX, pY, out ansiNearSDCM);

                    EGB10682 gbNearType = SDCMFunc.GB10682(pX, pY, out gbNearSDCM);

                    ansiSDCMItem.Value = ansiSDCM;

                    ansiNearCCTItem.Value = (int)ansiNearCCT;

                    ansiNearSDCMItem.Value = ansiNearSDCM;

                    gbSDCMItem.Value = gbSDCM;

                    gbNearCCTItem.Value = (int)gbNearType;

                    gbNearSDCMItem.Value = gbNearSDCM;
                }
            }


            if (this._sysSetting.IsEnableAdjacentError && isAllItemPass)
            {
                EAdjacentResult result = _adjacentCheck.Push(this._acquireData.ChipInfo.TestCount, this._acquireData.ChipInfo.ColX, this._acquireData.ChipInfo.RowY, this._condCtrl.Data.TestItemArray);

                this._acquireData.ChipInfo.AdjacentResult = result;

                if (result != EAdjacentResult.NONE && result != EAdjacentResult.PASS)
                {
                    //this._acquireData.IR[(int)EDataIR.IsAdjacentError] = 3; // Disable Call Prboer Clean needle
                    _adjacentCheck.ChangeMapRowCol();  // Return To Prober 3rd Coord.
                    this._acquireData.ChipInfo.ReTestColX = _adjacentCheck.ReTestCoordX;
                    this._acquireData.ChipInfo.ReTestRowY = _adjacentCheck.ReTestCoordY;
                }
            }

            // Pass Rate Check 

            this._passRateCheck.Push(this._condCtrl.Data.TestItemArray);

            if (this._passRateCheck.IsStopTest)
            {
                GlobalFlag.IsPassRateCheckSuccess = false;

                this.SetErrorCode(EErrorCode.PassRateCheckFail, this._passRateCheck.ErrorMsg);
            }
            else
            {
                GlobalFlag.IsPassRateCheckSuccess = true;
            }

            //--------------------------------------------------------
            // Eanble Pass and Fail count calculation by Bin Grade
            //--------------------------------------------------------
            if (this._sysSetting.IsCountPassFailByBinGrade)
            {
                SmartBinDataBase bin = this._binCtrl.GetBinFromSN(binSN);

                if (bin == null || this._binCtrl.GetBinFromSN(binSN).BinningType != EBinningType.IN_BIN)
                {
                    isAllItemPass = false;
                }
                else
                {
                    isAllItemPass = true;
                }
            }

            if (this._sysSetting.IsEnableSettingDefaultBinGrade)
            {
                if (this._binCtrl.SmartBin.Count == 0)
                {
                    finalBinGrade = this._sysSetting.DefaultBinGrade;
                }
            }

            //this._binGrade.Value = finalBinGrade;
            this._sysResultItem[(int)ESysResultItem.BIN].Value = finalBinGrade;

            this._sysResultItem[(int)ESysResultItem.BINSN].Value = binSN;

            this._acquireData.ChipInfo.BinGrade = finalBinGrade;

            // this._DataIR[(int)EDataIR.BinGrade] = finalBinGrade;

            //--------------------------------------------------------
            // Pass and Fail count calculation by
            // MsrtItem's low limit and upper limit
            //--------------------------------------------------------
            if (isAllItemPass)
            {
                this._acquireData.ChipInfo.GoodDieCount++;

                this._acquireData.ChipInfo.IsPass = true;

                this._sysResultItem[(int)ESysResultItem.ISALLPASS].Value = 1;

                this._sysResultItem[(int)ESysResultItem.ISFAIL].Value = 0;

                this._skipCount++;
            }
            else
            {
                this._acquireData.ChipInfo.FailDieCount++;

                this._acquireData.ChipInfo.IsPass = false;

                this._sysResultItem[(int)ESysResultItem.ISALLPASS].Value = 2;

                this._sysResultItem[(int)ESysResultItem.ISFAIL].Value = 1;

                this._skipCount = 0;
            }

            if (this._acquireData.ChipInfo.Channel < this._acquireData.ChannelResultDataSet.Count)
            {
                this._acquireData.ChannelResultDataSet[this._acquireData.ChipInfo.Channel].IsPass = this._acquireData.ChipInfo.IsPass;
            }
            else
            {
                this._acquireData.ChannelResultDataSet[0].IsPass = this._acquireData.ChipInfo.IsPass;
            }


            if (isAllItemPass02)
            {
                this._sysResultItem[(int)ESysResultItem.ISALLPASS02].Value = 1;
            }
            else
            {
                this._sysResultItem[(int)ESysResultItem.ISALLPASS02].Value = 2;
            }

            if (isAllItemPass03)
            {
                this._sysResultItem[(int)ESysResultItem.ISALLPASS03].Value = 1;
            }
            else
            {
                this._sysResultItem[(int)ESysResultItem.ISALLPASS03].Value = 2;
            }

            if (this._acquireData.ChipInfo.TestCount == 0)
            {
                this._acquireData.ChipInfo.GoodRate = 0.0d;
            }
            else
            {
                this._acquireData.ChipInfo.GoodRate = ((double)this._acquireData.ChipInfo.GoodDieCount) * 100.0d / ((double)this._acquireData.ChipInfo.TestCount);
            }

            _rManager.Push(_acquireData.ChipInfo.ColX, _acquireData.ChipInfo.RowY, binSN, isAllItemPass);
        }

        protected virtual void SolveRetestDUT()
        {
            var dut = _rManager.FindDUT(_acquireData.ChipInfo.ColX, _acquireData.ChipInfo.RowY);
            if (dut != null)// dut is tested 
            {
                this._sysResultItem[(int)ESysResultItem.TEST].Value--;
                this._acquireData.ChipInfo.TestCount = (int)this._sysResultItem[(int)ESysResultItem.TEST].Value;

                if (dut.PASS)
                {
                    _acquireData.ChipInfo.GoodDieCount--;
                }
                else
                {
                    _acquireData.ChipInfo.FailDieCount--;
                }

                this._binCtrl.DeBin(dut.BinSN);

            }
        }

        protected virtual bool OpenShortIFTest(bool isTurnOnTest, bool isShortTest)
        {
            double[] rtnData;
            bool isFinishTest = false;

            if (this._srcMeter == null)
                return false;

            this._ptOpenShortTimeOut.Start();

            while (isTurnOnTest && (!isFinishTest) && Convert.ToInt32(this._ptOpenShortTimeOut.PeekTimeSpan(ETimeSpanUnit.MilliSecond)) < 5000)
            {
                this._srcMeter.MeterOutput(null, this._testSequencElecCount);
                rtnData = this._srcMeter.GetDataFromMeter(0, this._testSequencElecCount);

                if (isShortTest && rtnData[0] < this._condCtrl.Data.OpenShortIFTestItem.MsrtResult[0].MaxLimitValue)
                {
                    isFinishTest = true;
                }

                if ((!isShortTest) && rtnData[0] > this._condCtrl.Data.OpenShortIFTestItem.MsrtResult[0].MaxLimitValue)
                {
                    isFinishTest = false;
                }
            }

            this._ptOpenShortTimeOut.Stop();

            if (isTurnOnTest == false)
                return false;

            if (isFinishTest == false)
                return true;
            else
                return false;
        }

        protected virtual void SimulatorRun()
        {
            int repeatCount = this._cmdData.IntData[1];
            int dieRepeatTestDelay = this._cmdData.IntData[2];

            int colX = 0;
            int rowY = 0;
            int len = (int)Math.Floor(Math.Sqrt(repeatCount));

            for (int repeatIndex = 0; repeatIndex < repeatCount; repeatIndex++)
            {
                if (repeatIndex != 0)
                {
                    this.WaitTimeOut((uint)dieRepeatTestDelay);
                }

                this.SimulateProcess(colX, rowY);

                this.GetTestedDataFromDevice();

                this._sysResultItem[(int)ESysResultItem.TEST].Value++;

                if (rowY < len - 1)
                {
                    rowY++;
                }
                else
                {
                    colX++;
                    rowY = 0;
                }
            }
        }

        protected virtual bool RunTestSequence(bool isReTest = false)
        {
            return true;
        }

        protected virtual void RunSingleRetest()
        {
            _isManualTest = true;

            bool rtn = false;
            this._sysSetting.IsEnableDarkCorrect = true;
            this._darkCorrectCount = 0;
            this.WaitTimeOut(50);          // Delay 250ms for the first chip run test

            if (this._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Die)
            {
                for (int i = 0; i < this._isChannelHasDie.Length; i++)
                {
                    this._isChannelHasDie[i] = true;
                }
            }

            rtn = this.RunTestSequence();

            if (this._esdDevice != null)
            {
                //WriteHardwareInfo
                this._esdDevice.ResetToSafeStatus();
            }
            if (rtn == false)
            {
                return;
            }
            this.GetTestedDataFromDevice();

            this.EndTestSequence();

            System.Windows.Forms.Application.DoEvents();

            _isManualTest = false;
        }

        protected virtual void EndTestSequence()
        {
            if (this._esdDevice != null && this._esdDevice.HardwareInfo != null)
            {
                this._esdDevice.ResetToSafeStatus();
                Console.WriteLine("[ESD State Record], HBM Relay Cnt = {0}; MM Relay Cnt = {1}", this._esdDevice.HardwareInfo.HBMRelayCount, this._esdDevice.HardwareInfo.MMRelayCount);
            }
        }

        protected virtual void ChangePolar()
        {
            if (this._product.TestCondition.ChipPolarity == EPolarity.Cathode_N)
            {
                this._product.TestCondition.ChipPolarity = EPolarity.Anode_P;
            }
            else if (this._product.TestCondition.ChipPolarity == EPolarity.Anode_P)
            {
                this._product.TestCondition.ChipPolarity = EPolarity.Cathode_N;
            }

            //--------------------------------------------------------------------------------------------------------
            // (1) Fisrt, set and transfer relative parameter to local variable 
            //--------------------------------------------------------------------------------------------------------
            if (this._product.TestCondition.ChipPolarity == EPolarity.Anode_P)
            {
                this._chipPolarity = 1.0d;
                this._sysResultItem[(int)ESysResultItem.POLAR].Value = 1.0d;
            }
            else if (this._product.TestCondition.ChipPolarity == EPolarity.Cathode_N)
            {
                this._chipPolarity = -1.0d;
                this._sysResultItem[(int)ESysResultItem.POLAR].Value = 2.0d;
            }
            else
            {
                this._chipPolarity = 1.0d;
                this._sysResultItem[(int)ESysResultItem.POLAR].Value = 0.0d;
            }

            //-------------------------------------------------------------------
            // 尋找 ESDTestItem上一道電，並將 IsNextIsESDTestItem = true
            //-------------------------------------------------------------------
            if (this._condCtrl.Data != null && this._condCtrl.Data.TestItemArray != null)
            {
                for (int i = 0; i < this._condCtrl.Data.TestItemArray.Length; i++)
                {
                    TestItemData item = this._condCtrl.Data.TestItemArray[i];

                    if (item is ESDTestItem)
                    {
                        if (this._condCtrl.Data.TestItemArray.Length < 2 || i - 1 < 0)
                        {
                            continue;
                        }

                        for (int j = i - 1; j >= 0; j--)
                        {
                            TestItemData last = this._condCtrl.Data.TestItemArray[j];

                            if (!last.IsEnable || last.ElecSetting == null)
                            {
                                continue;
                            }

                            if (last is ESDTestItem && !(last as ESDTestItem).IsEnableJudgeItem)
                            {
                                continue;
                            }

                            for (int k = 0; k < last.ElecSetting.Length; k++)
                            {
                                if (k == last.ElecSetting.Length - 1)
                                {
                                    last.ElecSetting[k].IsNextIsESDTestItem = true;
                                }
                            }

                            break;
                        }
                    }
                    else
                    {
                        if (item.ElecSetting != null)
                        {
                            foreach (var data in item.ElecSetting)
                            {
                                data.IsNextIsESDTestItem = false;
                            }
                        }
                    }
                }
            }

            //--------------------------------------------------------------------------------------------------------
            // (2) Count the elecTestItem
            //      then covert the positive and negative value by the chip polarity
            //--------------------------------------------------------------------------------------------------------
            List<ElectSettingData> elecSettingList = new List<ElectSettingData>();

            List<ESDSettingData> esdSettingList = new List<ESDSettingData>();

            if (this._condCtrl.Data != null)
            {
                if (this._condCtrl.Data.TestItemArray != null)
                {
                    foreach (TestItemData item in this._condCtrl.Data.TestItemArray)
                    {
                        if (item.ElecSetting != null)
                        {
                            foreach (ElectSettingData data in item.ElecSetting)
                            {
                                elecSettingList.Add(data.ConvertUnitTo(EAmpUnit.A.ToString(), EVoltUnit.V.ToString(), this._chipPolarity));
                            }
                        }

                        if (item is ESDTestItem)
                        {
                            ESDSettingData esdSetting = (item as ESDTestItem).EsdSetting.Clone() as ESDSettingData;

                            esdSetting.IsEnable = item.IsEnable;

                            // Chage the ESD polarity, if the chip polarity is changed
                            if (this._product.TestCondition.ChipPolarity == EPolarity.Cathode_N)
                            {
                                esdSetting.Polarity = EESDPolarity.N;
                            }
                            else
                            {
                                esdSetting.Polarity = EESDPolarity.P;
                            }

                            esdSettingList.Add(esdSetting.Clone() as ESDSettingData);
                        }
                    }
                }
            }

            //--------------------------------------------------------------------------------------------------------
            // (4) Set "SourceMeter" config and parameters data 
            //--------------------------------------------------------------------------------------------------------
            if (this._machineConfig.SourceMeterModel != ESourceMeterModel.NONE && this._srcMeter != null)
            {
                if (this._srcMeter.SetParamToMeter(elecSettingList.ToArray()) == false)
                {
                    this.SetErrorCode(this._srcMeter.ErrorNumber);
                }
            }

            //--------------------------------------------------------------------------------------------------------
            // (5) Set "ESD" config and parameters data 
            //--------------------------------------------------------------------------------------------------------
            if (this._machineConfig.ESDModel != EESDModel.NONE && this._esdDevice != null)
            {
                //-----------------------------------------------------------------------------------
                //  20131205 Roy
                //  新增 ESD 正常版 與 快速版(含 ESD 放慢之 Delay Time) 切換
                //-----------------------------------------------------------------------------------
                this._esdDevice.SetConfigToMeter(this._sysSetting.EsdDevSetting);

                if (this._esdDevice.SetParamToMeter(esdSettingList.ToArray()) == false)
                {
                    this.SetErrorCode(this._esdDevice.ErrorNumber);
                }

                this._esdDevice.ResetToSafeStatus();
            }
        }

        protected virtual bool LoadRDFuncParam()
        {
            string pathAndFile = Path.Combine(Constants.Paths.DATA_FILE, Constants.Files.RDFUNC_FILENAME);

            this._rdFunc.Open(pathAndFile);

            return true;
        }

        protected virtual void CalibrateResistance(TestItemData item)
        {
            //----------------------------------------------------------------------
            // Resistance Calibration
            //----------------------------------------------------------------------
            if (item.MsrtResult != null)
            {
                if (item.Type == ETestType.IF)
                {
                    for (int i = 0; i < item.MsrtResult.Length; i++)
                    {
                        item.MsrtResult[i].RawValue = item.MsrtResult[i].RawValue - (item.ElecSetting[0].ForceValue * 0.001) * this._product.Resistance;
                    }
                }
                else if (item.Type == ETestType.LOPWL)
                {
                    item.MsrtResult[(int)EOptiMsrtType.MVFLA].RawValue = item.MsrtResult[(int)EOptiMsrtType.MVFLA].RawValue - (item.ElecSetting[0].ForceValue * 0.001) * this._product.Resistance;
                    item.MsrtResult[(int)EOptiMsrtType.MVFLB].RawValue = item.MsrtResult[(int)EOptiMsrtType.MVFLB].RawValue - (item.ElecSetting[0].ForceValue * 0.001) * this._product.Resistance;
                }
            }

            //----------------------------------------------------------------------
            // Calibrate VF value by chuck resistance correction 
            //----------------------------------------------------------------------
            if (this._machineConfig.TesterCommMode == ETesterCommMode.TCPIP)
            {
                try
                {
                    if (item.MsrtResult != null && this._product.ChuckResistanceCorrectArray != null)
                    {
                        int index = ((int)this._sysResultItem[(int)ESysResultItem.CHUCKINDEX].Value) - 1;

                        if (index >= 0)
                        {
                            if (item.Type == ETestType.IF)
                            {
                                item.MsrtResult[0].RawValue = item.MsrtResult[0].RawValue - (item.ElecSetting[0].ForceValue * 0.001) * this._product.ChuckResistanceCorrectArray[index];
                            }

                            if (item.Type == ETestType.LOPWL)
                            {
                                if (index < this._product.ChuckResistanceCorrectArray.Length)
                                {
                                    item.MsrtResult[(int)EOptiMsrtType.MVFLA].RawValue = item.MsrtResult[(int)EOptiMsrtType.MVFLA].RawValue - (item.ElecSetting[0].ForceValue * 0.001) * this._product.ChuckResistanceCorrectArray[index];
                                    item.MsrtResult[(int)EOptiMsrtType.MVFLB].RawValue = item.MsrtResult[(int)EOptiMsrtType.MVFLB].RawValue - (item.ElecSetting[0].ForceValue * 0.001) * this._product.ChuckResistanceCorrectArray[index];
                                }
                            }
                        }
                    }
                }catch(Exception e)
                {}
            }
        }

        protected virtual void GetPDDarkCurrent()//MS 待修改
        {
            ISourceMeter srcMeter = this._srcMeter;

            int rem = -1;

            switch (this._sysSetting.PDDarkCorrectMode)
            {
                case EPDDarkCorrectMode.None:
                    rem = -1;
                    break;
                case EPDDarkCorrectMode.Normal:
                    Math.DivRem(this._darkCorrectCount, 200, out rem);
                    break;
                case EPDDarkCorrectMode.High:
                    Math.DivRem(this._darkCorrectCount, 1, out rem);
                    break;
                case EPDDarkCorrectMode.Low:
                    Math.DivRem(this._darkCorrectCount, 50000, out rem);
                    break;
                default:
                    rem = -1;
                    break;
            }

            if (this._machineConfig.PDSensingMode == EPDSensingMode.SrcMeter_2nd)
            {
                if (rem == 0)
                {
                    double darkCurrent = srcMeter.GetPDDarkSample(5);
                    Console.WriteLine("[TesterKernel_Base], GetPDDarkDataAndSave(), PD Dark:" + darkCurrent.ToString());
                }
                else if (rem == -1)
                {
                    //Reset dark current
                    srcMeter.GetPDDarkSample(0);
                }
            }
        }

        protected virtual void GetDarkIntensityData()
        {
            int rem = 0;

            //--------------------------------------------------------------------
            //  Get current dark intensity data for each 200 chips
            //--------------------------------------------------------------------
            if (this._sysSetting.IsEnableDarkCorrect == true)
            {
                Math.DivRem(this._darkCorrectCount, this._skipGetDarkCounts, out rem);
                if (this._sptMeter != null && rem == 0)
                {
                    this._darkCorrectCount = 0;
                    this._darkSample = this._sptMeter.GetDarkSample(5, 8000);
                }
            }

            //--------------------------------------------------------------------
            //  Get PD current dark intensity data for each 200 chips
            //--------------------------------------------------------------------
            this.GetPDDarkCurrent();

            if (this._sysSetting.IsEnableDarkCorrect || this._sysSetting.PDDarkCorrectMode != EPDDarkCorrectMode.None)
            {
                this._darkCorrectCount++;
            }
        }

        protected virtual void SetMeasuredRawValueToValue(TestItemData item)
        {
            //----------------------------------------------------------------------
            // Copy meaureed raw value to value
            //----------------------------------------------------------------------
            if (item.MsrtResult != null)
            {
                for (int i = 0; i < item.MsrtResult.Length; i++)
                {
                    item.MsrtResult[i].Value = item.MsrtResult[i].RawValue;
                }
            }
        }

        protected virtual void CalibrateMeasureResult(TestItemData item)
        {

            //----------------------------------------------------------------------
            // TestItem is not enable, reset meaureed raw value = 0.0d
            //----------------------------------------------------------------------
            if (item.IsEnable == false && item.MsrtResult != null)
            {
                for (int i = 0; i < item.MsrtResult.Length; i++)
                {
                    item.MsrtResult[i].RawValue = 0.0d;

                    item.MsrtResult[i].Value = 0.0d;
                }

                return;
            }

            //----------------------------------------------------------------------
            // Calibrate the measured data and decide the next step. 
            // If the test resuult enable "skip function",
            //----------------------------------------------------------------------

            if (this._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Die &&
                item is LOPWLTestItem)
            {
                return;
            }

            item.Calibrate();

            if (this._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Die)
            {
                // Calibration By Channel
                if (this._condCtrl.Data.ChannelConditionTable.IsApplyByChannelCompensate)
                {
                    if (item.MsrtResult != null)
                    {
                        for (int i = 0; i < item.MsrtResult.Length; i++)
                        {
                            item.MsrtResult[i].RawValue = item.MsrtResult[i].Value;
                        }

                        item.CalibrateByChannel();

                    }
                }
            }



            // 20161121, Roy Modifiy
            if (item.Type == ETestType.TRANSISTOR || item.Type == ETestType.LIV)
            {
                item.CalibrateLIVSweepData(this._acquireData.LIVDataSet[item.KeyName]);
            }

            if (item.MsrtResult != null && !_isTVSTesting)
            {
                if (item.Type != ETestType.DVF && item.Type != ETestType.LCR && item.Type != ETestType.OSA)//20170904 David
                //if (item.Type != ETestType.DVF && item.Type != ETestType.LCR && item.Type != ETestType.LCRSWEEP && item.Type != ETestType.OSA)
                {
                    foreach (TestResultData data in item.MsrtResult)
                    {
                        if (data.Value < 0.0d)
                        {
                            data.Value = 0.0d;
                        }
                    }
                }
            }
        }

        protected virtual void ResetItemMsrtResultValueToZero(TestItemData[] testItemArray, uint startIndex = 0, bool isForceClearNotTest = false)
        {
            TestItemData item = null;
            //----------------------------------------------------------
            // Reset "TestResult" of the no tested item as ZERO
            //----------------------------------------------------------
            for (uint k = startIndex; k < this._condCtrl.Data.TestItemArray.Length; k++)
            {
                item = testItemArray[k];
                if (item.IsTested && isForceClearNotTest)
                {
                    // 測試過，保留原本的數值
                    continue;
                }
                else
                {
                    if (item.MsrtResult != null)
                    {
                        foreach (TestResultData data in item.MsrtResult)
                        {
                            data.RawValue = 0.0d;

                            data.Value = 0.0d;
                        }
                    }
                }
            }
        }

        protected virtual void SetContactCheck(List<ElectSettingData> elecSettingList, ref uint electSettingOrder)
        {
            this._openShortContactItem = this.CreateOpenShortCheckItem();

            if (this._openShortContactItem != null)
            {
                bool isNextIsESDTestItem = false;

                if (this._condCtrl.Data != null && this._condCtrl.Data.TestItemArray != null)
                {
                    if (this._condCtrl.Data.TestItemArray.Length > 0 && this._condCtrl.Data.TestItemArray[0] is ESDTestItem)
                    {
                        isNextIsESDTestItem = this._condCtrl.Data.TestItemArray[0].IsEnable;
                    }
                }

                this._openShortContactItem.ElecSetting[0].IsNextIsESDTestItem = isNextIsESDTestItem;

                foreach (ElectSettingData data in this._openShortContactItem.ElecSetting)
                {
                    data.Order = electSettingOrder;

                    elecSettingList.Add(data.ConvertUnitTo(EAmpUnit.A.ToString(), EVoltUnit.V.ToString(), this._chipPolarity));

                    electSettingOrder = 1;
                }

                if (this._sysSetting.contactCheckCFG._isEsdItemContactCheck)
                {
                    this._ESDContactCheckItem = this._openShortContactItem;
                }
                else
                {
                    this._ESDContactCheckItem = null;
                }
            }
        }

        protected virtual bool IsGroupSkip(TestItemData item, int channel = -1)
        {
            if (item is LOPWLTestItem)
            {
                (item as LOPWLTestItem).IsTestElecDontTestLOPWL = false;
            }

            if (item.MsrtResult != null)
            {
                foreach (var result in item.MsrtResult)
                {
                    result.IsGroupSkip = false;
                }
            }

            //if (!this._sysSetting.IsEnableTestGroup)
            //{
            //    return false;
            //}

            //if (!this._product.CustomerizedSetting.IsEnableTestGroup)
            //{
            //    return false;
            //}

            //	0:defalut全測, 1:G1(全測), 2:G2(抽測)
            int group = 1;

            if (channel >= 0)
            {
                group = this._chipGroup[channel];
            }
            else
            {
                group = (int)this._cmdData.DoubleData[(uint)EProberDataIndex.TestChipGroup];
            }



            if (group < 1)
            {
                // probe Defalut or Tester ManaulRun
                return false;
            }
            else if (group > item.TestGroupCtrl.Count)
            {
                return false;
            }
            else
            {
                if (item.TestGroupCtrl[group - 1])
                {
                    return false;
                }
                else
                {
                    if (item is LOPWLTestItem)
                    {
                        (item as LOPWLTestItem).IsTestElecDontTestLOPWL = group == 2 & item is LOPWLTestItem & this._sysSetting.IsEnableReportInterpolation;
                    }

                    foreach (var data in item.MsrtResult)
                    {
                        data.IsGroupSkip = true;

                        if ((item is LOPWLTestItem) && (item as LOPWLTestItem).IsTestElecDontTestLOPWL)
                        {
                            if (data.KeyName.Contains(EOptiMsrtType.MVFLA.ToString()))
                            {
                                data.IsGroupSkip = false;
                            }
                        }
                    }

                    if ((item is LOPWLTestItem) && (item as LOPWLTestItem).IsTestElecDontTestLOPWL)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        protected virtual bool SetSpectrometerXYCali(List<OptiSettingData> optiSettingList)
        {
            if (this._sysSetting.OptiDevSetting.IsUseProductXaxisCoeff == true)
            {
                this._sysSetting.OptiDevSetting.SptXaxisCoefficientByProduct = this._product.ProductSptXwaveCoef;
            }

            if (this._sysSetting.OptiDevSetting.IsUseProductYaxisCalib == true)
            {
                this._sysSetting.OptiDevSetting.SptYaxisCalibArrayByProduct = this._product.ProductSptYintCoef;
            }

            if (this._sysSetting.OptiDevSetting.isUseProductYaxisWeight == true)
            {
                this._sysSetting.OptiDevSetting.SptYaxisWeightArrayByProduct = this._product.ProductSptYweight;
            }

            this._sysSetting.OptiDevSetting.IsUseMPISpam2 = true;


            if (this._machineConfig.SpectrometerModel != ESpectrometerModel.NONE && this._sptMeter != null)
            {
                //-----------------------------------------------------------------------------------
                // Re-Initialize the spectrometer for every cycle run. Gilbert
                //-----------------------------------------------------------------------------------
                if (this._machineConfig.SpectrometerModel == ESpectrometerModel.USB2000P)
                {
                    this._skipGetDarkCounts = 200;

                    switch (this._sysSetting.OptiDevSetting.DarkCorrectMode)
                    {
                        case EDarkCorrectMode.Normal:
                            this._skipGetDarkCounts = 200;
                            break;
                        case EDarkCorrectMode.High:
                            this._skipGetDarkCounts = 1;  // Each Testing need to do dark Correct.
                            break;
                        case EDarkCorrectMode.Low:
                            this._skipGetDarkCounts = 50000;
                            break;
                    }
                    this._sptMeter.Init(0, this._machineConfig.SpectrometerSN, this._machineConfig.SphereSN);
                }
                else  // CAS140,CAS120,LE5400
                {
                    this._skipGetDarkCounts = 50000;
                }

                if (this._sptMeter.SetConfigToMeter(this._sysSetting.OptiDevSetting) == false)
                {
                    this.SetErrorCode(this._sptMeter.ErrorNumber);
                    return false;
                }

                if (this._sptMeter.SetParamToMeter(optiSettingList.ToArray()) == false)
                {
                    this.SetErrorCode(this._sptMeter.ErrorNumber);
                    return false;
                }

            }

            return true;
        }

        protected virtual TestItemData CreateOpenShortCheckItem(bool ischeckESD = false)
        {
            if (this._sysSetting.contactCheckCFG._isEnableContactCheck)
            {
                TestItemData contactCheckItem = new IFTestItem();

                contactCheckItem.ElecSetting[0].ForceValue = this._sysSetting.contactCheckCFG._contactApplyCurrentValue * 0.001;

                contactCheckItem.ElecSetting[0].ForceTime = this._sysSetting.contactCheckCFG._contactApplyForceTime;

                contactCheckItem.ElecSetting[0].Order = 0;

                if (this._sysSetting.contactCheckCFG._contactSpecMax > 4.0d || this._sysSetting.contactCheckCFG._contactSpecMin > 4.0d)
                {
                    double maxCompliance = Math.Max(this._sysSetting.contactCheckCFG._contactSpecMax, this._sysSetting.contactCheckCFG._contactSpecMin);

                    if (maxCompliance <= 20.0d)
                    {
                        contactCheckItem.ElecSetting[0].MsrtRange = maxCompliance;

                        contactCheckItem.ElecSetting[0].MsrtProtection = maxCompliance;
                    }
                }
                else
                {
                    contactCheckItem.ElecSetting[0].MsrtRange = 4;

                    contactCheckItem.ElecSetting[0].MsrtProtection = 4.0d;
                }

                contactCheckItem.MsrtResult[0].IsEnable = true;

                contactCheckItem.MsrtResult[0].IsVerify = true;

                contactCheckItem.MsrtResult[0].MaxLimitValue = this._sysSetting.contactCheckCFG._contactSpecMax;

                contactCheckItem.MsrtResult[0].MinLimitValue = this._sysSetting.contactCheckCFG._contactSpecMin;

                if (ischeckESD)
                {
                    if (this._sysSetting.contactCheckCFG._isEsdItemContactCheck)
                    {
                        this._ESDContactCheckItem = contactCheckItem;
                    }
                    else
                    {
                        this._ESDContactCheckItem = null;
                    }

                    bool isNextIsESDTestItem = false;

                    if (this._condCtrl.Data != null && this._condCtrl.Data.TestItemArray != null)
                    {
                        if (this._condCtrl.Data.TestItemArray.Length > 0 && this._condCtrl.Data.TestItemArray[0] is ESDTestItem)
                        {
                            isNextIsESDTestItem = this._condCtrl.Data.TestItemArray[0].IsEnable;
                        }
                    }

                    contactCheckItem.ElecSetting[0].IsNextIsESDTestItem = isNextIsESDTestItem;
                }
                return contactCheckItem;

                //elecSettingList.Add(contactCheckItem.ElecSetting[0].ConvertUnitTo(EAmpUnit.A.ToString(), EVoltUnit.V.ToString(), this._chipPolarity));
            }
            else
            {
                this._ESDContactCheckItem = null;
            }
            return null;
        }

        protected virtual void FillFakeValueToResultItem(TestItemData[] testItemArray)
        {
            TestItemData item = null;
            //----------------------------------------------------------
            // Reset "TestResult" of the no tested item as ZERO
            //----------------------------------------------------------
            for (uint k = 0; k < testItemArray.Length; k++)
            {
                item = testItemArray[k];

                if (item.IsTested)
                {
                    // 測試過，保留原本的數值
                    continue;
                }
                else
                {
                    // 當OpenShort沒有執行此沒有第一個Item當作Open/Short測試。
                    if (item.Type == ETestType.IF || item.Type == ETestType.LOPWL)
                    {
                        item.MsrtResult[0].RawValue = item.ElecSetting[0].MsrtProtection;
                        item.MsrtResult[0].Value = item.ElecSetting[0].MsrtProtection;
                    }

                    if (item.Type == ETestType.VR)
                    {
                        item.MsrtResult[0].RawValue = item.ElecSetting[0].MsrtProtection;
                        item.MsrtResult[0].Value = item.ElecSetting[0].MsrtProtection;
                    }

                    if (item.Type == ETestType.IZ)
                    {
                        if (this._sysSetting.contactCheckCFG._isEnableVzFillRandomValue)
                        {
                            Random rd = new Random();

                            double randomVz = 5 * rd.NextDouble();

                            item.MsrtResult[0].RawValue = randomVz;

                            item.MsrtResult[0].Value = randomVz;
                        }
                        else
                        {
                            item.MsrtResult[0].RawValue = 0;
                            item.MsrtResult[0].Value = 0;
                        }
                    }
                }
            }
        }

        protected void SetErrorCode(EDevErrorNumber devErrorCode, string errorMsg = "")
        {
            EErrorCode errorCode = EErrorCode.NONE;

            if (Enum.IsDefined(typeof(EErrorCode), (int)devErrorCode))
            {
                errorCode = (EErrorCode)devErrorCode;
            }
            else
            {
                errorCode = EErrorCode.UndefinedErrorCode;
            }

            this.SetErrorCode(errorCode, errorMsg);
        }

        protected void SetErrorCode(EErrorCode errorCode, string errorMsg = "")
        {
            if (errorCode == EErrorCode.NONE)
            {
                return;
            }

            this.Status.ErrorCode = errorCode;

            if (this.ErrorCodeEvent != null)
            {
                this.ErrorCodeEvent(new ErrorCodeEventArgs(errorCode, errorMsg));
            }

            this.Status.ErrorCode = EErrorCode.NONE;

            if (this._status.State != EKernelState.Not_Ready)
            {
                this.Status.State = EKernelState.Error;
            }
        }

        protected void Fire_FinishTestAndCalcEvent(System.EventArgs e)
        {
            EventHandler<EventArgs> handlerInstance = this.FinishTestAndCalcEvent;

            if (handlerInstance != null)
            {
                handlerInstance(new object(), e);
            }
        }

        protected void SetIOCheck()
        {
            _ioStateCheck.Clear();
            _ioStateCheck.IsNeedCheckIO = false;

            if (_srcMeter != null && _srcMeter is Keithley2600 && this._machineConfig.IOConfig != null && _machineConfig.IOConfig.IOList != null)
            {
                foreach (var ioSet in _machineConfig.IOConfig.IOList)
                {
                    if (ioSet.DMode == EIOTrig_Mode.READ && ioSet.IsShow && ioSet.Name != "")//使用IsShow作為判別是否啟用的基準
                    {
                        string tempStr = ioSet.Name;

                        if (tempStr.Length > 2)
                        {
                            int index = tempStr.IndexOf("R_");
                            if (index >= 0)
                            {

                                string keyStr = tempStr.Substring(index + 2);

                                foreach (var ioSet2 in _machineConfig.IOConfig.IOList)
                                {
                                    if (ioSet2.Name.StartsWith(keyStr))
                                    {
                                        //_ioStateCheck.Add(ioSet.PinNum, ioSet2.PinNum);
                                        if (this._condCtrl.Data.TestItemArray != null)
                                        {
                                            foreach (TestItemData item in this._condCtrl.Data.TestItemArray)
                                            {
                                                if (item.Type == ETestType.IO && item.IsEnable &&
                                                    (item as IOTestItem).IOSetData.CmdList.FindIndex(x => x.Pin == ioSet2.PinNum) >= 0) //抓最後一組
                                                {
                                                    if (_ioStateCheck.Watch_OrderDic.ContainsKey(ioSet.PinNum))
                                                    {
                                                        _ioStateCheck.Watch_OrderDic[ioSet.PinNum] = item.ElecSetting[0].Order;
                                                        _ioStateCheck.Watch_ExceptStateDic[ioSet.PinNum] = (ioSet2.State == EIOState.HIGH ? 1 : 0);
                                                    }
                                                    else
                                                    {
                                                        _ioStateCheck.Watch_OrderDic.Add(ioSet.PinNum, item.ElecSetting[0].Order);
                                                        _ioStateCheck.Watch_ExceptStateDic.Add(ioSet.PinNum, (ioSet2.State == EIOState.HIGH ? 1 : 0));
                                                    }

                                                    _ioStateCheck.IsNeedCheckIO = true;
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
        }

        protected void CheckIOState(uint srcChannel)
        {
            if (_srcMeter != null && this._srcMeter is Keithley2600)
            {
                (this._srcMeter as Keithley2600).SetDefaultIO();

                //_ioStateCheck
                if (_ioStateCheck.IsNeedCheckIO)
                {
                    foreach (var readPin_ in _ioStateCheck.Watch_ExceptStateDic)
                    {
                        for (int i = 0; i < 2; ++i)
                        {
                            byte bRes = (_srcMeter as Keithley2600).GetIONState(readPin_.Key);

                            if ((int)bRes != readPin_.Value)
                            {
                                List<uint> TrigList = new List<uint>(new uint[] { srcChannel });

                                _srcMeter.MeterOutput(TrigList.ToArray(), _ioStateCheck.Watch_OrderDic[readPin_.Key]);

                            }
                            else
                            {
                                break;
                            }
                        }

                    }

                }
            }
        }

        protected void SetPolarity()
        {
            if (this._product.TestCondition.ChipPolarity == EPolarity.Anode_P)
            {
                this._chipPolarity = 1.0d;
                this._sysResultItem[(int)ESysResultItem.POLAR].Value = 1.0d;
            }
            else if (this._product.TestCondition.ChipPolarity == EPolarity.Cathode_N)
            {
                this._chipPolarity = -1.0d;
                this._sysResultItem[(int)ESysResultItem.POLAR].Value = 2.0d;
            }
            else
            {
                this._chipPolarity = 1.0d;
                this._sysResultItem[(int)ESysResultItem.POLAR].Value = 0.0d;
            }
        }

        protected TestItemData CreateOpenShortCheckItem()
        {
            if (this._sysSetting.contactCheckCFG._isEnableContactCheck)
            {
                TestItemData contactCheckItem = new IFTestItem();

                contactCheckItem.ElecSetting[0].ForceValue = this._sysSetting.contactCheckCFG._contactApplyCurrentValue * 0.001;

                contactCheckItem.ElecSetting[0].ForceTime = this._sysSetting.contactCheckCFG._contactApplyForceTime;

                contactCheckItem.ElecSetting[0].Order = 0;

                if (this._sysSetting.contactCheckCFG._contactSpecMax > 4.0d || this._sysSetting.contactCheckCFG._contactSpecMin > 4.0d)
                {
                    double maxCompliance = Math.Max(this._sysSetting.contactCheckCFG._contactSpecMax, this._sysSetting.contactCheckCFG._contactSpecMin);

                    if (maxCompliance <= 20.0d)
                    {
                        contactCheckItem.ElecSetting[0].MsrtRange = maxCompliance;

                        contactCheckItem.ElecSetting[0].MsrtProtection = maxCompliance;
                    }
                }
                else
                {
                    contactCheckItem.ElecSetting[0].MsrtRange = 4;

                    contactCheckItem.ElecSetting[0].MsrtProtection = 4.0d;
                }

                contactCheckItem.MsrtResult[0].IsEnable = true;

                contactCheckItem.MsrtResult[0].IsVerify = true;

                contactCheckItem.MsrtResult[0].MaxLimitValue = this._sysSetting.contactCheckCFG._contactSpecMax;

                contactCheckItem.MsrtResult[0].MinLimitValue = this._sysSetting.contactCheckCFG._contactSpecMin;

                return contactCheckItem;
            }
            else
            {
                return null;
            }
        }

        protected void GetTestGroupStr()
        {
            if (this.CmdData.StringData[3] != null)
            {
                this._acquireData.ChipInfo.GroupName = this.CmdData.StringData[3];
            }
            else
            {
                this._acquireData.ChipInfo.GroupName = "";
            }

            //this._acquireData.ChipInfo.GroupIndex = (int)this.CmdData.DoubleData[(uint)EProberDataIndex.TestGroupIndex];
        }

        protected void RunAttenuatorMsrt()
        {
            if (this._laserSrcSys != null &&
                this._laserSrcSys.AttManager != null)
            {
                double val = this._laserSrcSys.AttManager.GetMsrtPower(1, _product.LaserSrcSetting.AttenuatorData.PowerUnit);

                _acquireData.DeviceRunTimeDataSet["ATT"]["ATTPower"].DataArray[0] = (float)val;

                Fire_FinishTestAndCalcEvent(null);
            }
        }

        protected virtual ElecDevSetting SetElecDevsetting(ElecDevSetting devSet = null)
        {
            ElecDevSetting devSetting = devSet;

            if (devSet == null)
            {
                devSetting = new ElecDevSetting();
            }

            devSetting.IsFastPolar = this._machineConfig.IsFastPolar;

            devSetting.DAQModel = this._machineConfig.DAQModel;

            devSetting.IsEnableRTH = this._rdFunc.RDFuncData.IsEnableRTHTestItem;

            devSetting.RTHDeviceIP = this._rdFunc.RDFuncData.RTHSrcMeterIPAddress;

            devSetting.DAQSampleRate = this._machineConfig.DAQSampleRate;

            devSetting.DAQCalibrationBufferID = this._machineConfig.DAQCalibrationBufferID;

            devSetting.SrcTurnOffType = (ESrcTurnOffType)this._rdFunc.RDFuncData.SrcTurnOffType;

            if (devSetting.SrcTurnOffType == ESrcTurnOffType.TestEnd)
            {
                devSetting.SrcTurnOffType = ESrcTurnOffType.EachTestItem;
            }

            devSetting.IsDevicePeakFiltering = this._machineConfig.Enable.IsEnableSrcFirmwareCalcTHY;

            devSetting.TurnOffRangeIBackToDefault = this._rdFunc.RDFuncData.IsTurnOffRangeIBackToDefault;

            devSetting.IsDevicePeakFiltering = this._machineConfig.Enable.IsEnableSrcFirmwareCalcTHY;

            devSetting.TurnOffRangeIBackToDefault = this._rdFunc.RDFuncData.IsTurnOffRangeIBackToDefault;

            devSetting.IsSettingReverseCurrentRange = this._rdFunc.RDFuncData.IsSettingReverseCurrentRange;

            devSetting.ReverseCurrentApplyRange = this._rdFunc.RDFuncData.ReverseCurrentApplyRange;

            devSetting.DetectorMsrtDevice = this._machineConfig.PDSensingMode;

            devSetting.DetectorDeviceIP = this._machineConfig.PDDetectorSN;

            devSetting.IsDetectorHwTrig = this._machineConfig.IsPDDetectorHwTrig;

            devSetting.SrcSensingMode = this._machineConfig.SrcSensingMode;

            devSetting.LCRDCBiasType = this._machineConfig.LCRDCBiasType;

            devSetting.LCRDCBiasSource = this._machineConfig.LCRDCBiasSource;

            devSetting.LCRDCBiasSourceIP = this._machineConfig.LCRDCBiasSourceSN;

            devSetting.SourceMeterModel = this._machineConfig.SourceMeterModel;

            devSetting.VoltMsrtDevice = this._machineConfig.DmmModel;

            devSetting.VoltMsrtDeviceIP = this._machineConfig.DmmSN;

            devSetting.ReverseSrcDevModel = this._machineConfig.SourceMeterModel2;  // for pulser

            devSetting.ReverseSrcDevIP = this._machineConfig.SourceMeterSN2;

            devSetting.IOSetting = this._machineConfig.IOConfig;

            if (this._machineConfig.LaserSrcSysConfig.MoniterPDSMU != null)
            {
                devSetting.PDMonitorSMU = this._machineConfig.LaserSrcSysConfig.MoniterPDSMU.Clone() as SourceMeterAssignmentData;
            }
            else
            {
                devSetting.PDMonitorSMU = null;
            }

            return devSetting;
        }

        protected virtual bool SetSrcMeter(ElecDevSetting devSetting)
        {

            _seqManagement = new KernelSequenceManagement(this._machineConfig.ChannelConfig.TesterSequenceType);

            _seqManagement.Set(this._machineConfig.ChannelConfig.AssignmentTable);


            switch (this._machineConfig.SourceMeterModel)
            {
                case ESourceMeterModel.NONE:
                    this._srcMeter = null;
                    break;
                case ESourceMeterModel.LDT1A:
                    this._srcMeter = new LDT1A();
                    break;
                //-----------------------------------------------------------------
                case ESourceMeterModel.K2400:
                    devSetting.HWConnectorType = EHWConnectType.GPIB;
                    this._srcMeter = new Keithley2400(devSetting);
                    break;
                //-----------------------------------------------------------------
                case ESourceMeterModel.K2600:

                    devSetting.HWConnectorType = EHWConnectType.TCPIP;

                    if ((this._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Terminal) ||
                        (this._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Die &&
                        this._machineConfig.ChannelConfig.TesterSequenceType == ETesterSequenceType.Parallel) ||
                        (this._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Die &&
                        this._machineConfig.ChannelConfig.TesterSequenceType == ETesterSequenceType.Series))
                    {
                        devSetting.SrcTriggerMode = ESMUTriggerMode.PMDT;

                        devSetting.Assignment = _seqManagement.SrcAssignment;
                    }
                    else
                    {
                        devSetting.SrcTriggerMode = ESMUTriggerMode.Single;
                    }

                    //_ioStateCheck.Clear();
                    //_ioStateCheck.IsNeedCheckIO = false;
                    //if(devSetting.IOSetting != null && devSetting.IOSetting.IOList != null)
                    //{
                    //    foreach(var ioSet in devSetting.IOSetting.IOList)
                    //    {
                    //        if(ioSet.DMode == EIOTrig_Mode.READ)
                    //        {
                    //            string tempStr = ioSet.Name;

                    //            int index = tempStr.LastIndexOf("_R");

                    //            string keyStr = tempStr.Substring(0, index);

                    //            foreach (var ioSet2 in devSetting.IOSetting.IOList)
                    //            {
                    //                if (ioSet2.Name.StartsWith(keyStr))
                    //                {
                    //                    //_ioStateCheck.Add(ioSet.PinNum, ioSet2.PinNum);

                    //                    _ioStateCheck.IsNeedCheckIO = true;
                    //                }
                    //            }
                    //        }
                    //    }
                    //}

                    this._srcMeter = new Keithley2600(devSetting);
                    break;
                //-----------------------------------------------------------------
                case ESourceMeterModel.T2001L:
                    devSetting.HWConnectorType = EHWConnectType.NONE;
                    this._srcMeter = new T2001L(devSetting);
                    break;
                //-----------------------------------------------------------------
                case ESourceMeterModel.DR2000:
                    devSetting.HWConnectorType = EHWConnectType.RS232;
                    this._srcMeter = new DR2000(devSetting);
                    break;
                //-----------------------------------------------------------------
                case ESourceMeterModel.N5700:
                    devSetting.HWConnectorType = EHWConnectType.USB;
                    this._srcMeter = new N5751A(devSetting);
                    break;
                //-----------------------------------------------------------------
                case ESourceMeterModel.DSPHD:
                    devSetting.HWConnectorType = EHWConnectType.RS232;
                    this._srcMeter = new DSPHD(devSetting);
                    break;
                //-----------------------------------------------------------------
                case ESourceMeterModel.IT7321:
                    devSetting.HWConnectorType = EHWConnectType.USB;
                    this._srcMeter = new IT7321(devSetting);
                    break;
                //-----------------------------------------------------------------
                case ESourceMeterModel.LDT3A200:
                    devSetting.HWConnectorType = EHWConnectType.USB;
                    if (this._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Die)
                    {
                        if (this._machineConfig.ChannelConfig.TesterSequenceType == ETesterSequenceType.Parallel)
                        {
                            devSetting.SrcTriggerMode = ESMUTriggerMode.PMDT;

                            devSetting.Assignment = _seqManagement.SrcAssignment;
                        }
                        else
                        {
                            devSetting.SrcTriggerMode = ESMUTriggerMode.Single;
                        }
                    }

                    try
                    {
                        this._srcMeter = new LDT3A200(devSetting);
                    }
                    catch
                    {
                        //this.SetErrorCode(EErrorCode.SourceMeterDllMissing_Err);

                        return false;
                    }
                    break;
                //-----------------------------------------------------------------
                case ESourceMeterModel.K2520:
                    devSetting.HWConnectorType = EHWConnectType.GPIB;
                    this._srcMeter = new Keithley2520(devSetting);
                    break;
                //-----------------------------------------------------------------
                case ESourceMeterModel.RM3542:
                    devSetting.HWConnectorType = EHWConnectType.RS232;
                    this._srcMeter = new RM3542(devSetting);
                    break;
                //-----------------------------------------------------------------
                case ESourceMeterModel.B2900A:     // 20170627, Roy
                    devSetting.HWConnectorType = EHWConnectType.TCPIP;
                    this._srcMeter = new KeysightB2900A(devSetting);
                    break;
                //-----------------------------------------------------------------
                case ESourceMeterModel.SS400:     // 20180110, Roy
                    devSetting.HWConnectorType = EHWConnectType.TCPIP;
                    this._srcMeter = new MPI.Tester.Device.Pulser.SS400(devSetting);
                    break;
                //-----------------------------------------------------------------
                case ESourceMeterModel.Persona:
                    devSetting.HWConnectorType = EHWConnectType.TCPIP;
                    this._srcMeter = new Persona(devSetting);
                    break;
                //-----------------------------------------------------------------
                default:
                    this._srcMeter = null;
                    break;
            }

            if (this._srcMeter != null)
            {
                if (this._srcMeter.Init(0, this._machineConfig.SourceMeterSN))
                {
                    this._machineInfo.TesterSN = this._srcMeter.SerialNumber + "_ " +
                                                                    this._srcMeter.HardwareVersion + "_" +
                                                                    this._srcMeter.SoftwareVersion;

                    this._machineInfo.IsSrcInitSuccess = true;
                    this._machineInfo.SourceMeterSN = this._srcMeter.SerialNumber;
                    this._machineInfo.SourceMeterHWVersion = this._srcMeter.HardwareVersion;
                    this._machineInfo.SourceMeterSWVersion = this._srcMeter.SoftwareVersion;
                    this._srcMeter.Reset();

                    //if (this._srcMeter is Keithley2600)
                    //{
                    //    this._machineInfo.SourceMeterSN2 = (this._srcMeter as Keithley2600).SerialNumber2;
                    //    this._machineInfo.SourceMeterHWVersion2 = (this._srcMeter as Keithley2600).HardwareVersion2;
                    //    this._machineInfo.SourceMeterSWVersion2 = (this._srcMeter as Keithley2600).SoftwareVersion2;
                    //}
                    //else if (this._srcMeter is SS400)
                    //{
                    //    this._machineInfo.DmmSN1 = (this._srcMeter as SS400).DmmSerialNumber1;
                    //    this._machineInfo.DmmSN2 = (this._srcMeter as SS400).DmmSerialNumber2;
                    //    this._machineInfo.SourceMeterSN2 = (this._srcMeter as SS400).SmuSerialNumber;

                    //}
                    if (this._srcMeter is Keithley2520)
                    {
                        this._machineInfo.SourceMeterSN2 = (this._srcMeter as Keithley2520).SerialNumber;
                    }

                    Console.WriteLine(string.Format("[TesterKernel_Base], Init(), SourceMeter: {0}, {1}", this._srcMeter.HardwareVersion, this._srcMeter.SerialNumber));
                }
                else
                {
                    this.SetErrorCode(this._srcMeter.ErrorNumber);

                    Console.WriteLine(string.Format("[TesterKernel_Base], Init(), SourceMeter Init Fail (Err{0})", (int)this._srcMeter.ErrorNumber));
                }

                this._machineInfo.SourceMeterSpec = this._srcMeter.Spec;

                if (this._srcMeter.ErrorNumber != EDevErrorNumber.Device_NO_Error)
                {
                    this._srcMeter = null;

                    return false;
                }
            }

            return true;
        }

        protected virtual bool SetSimuMode()
        {
            if (this._machineConfig.Enable.IsSimulator == true)
            {
                this._machineInfo.TesterSN = "Sim. Tester SN";
                this._machineInfo.SpectrometerSN = "Sim. Spectrometer SN";
                this._machineInfo.SphereSN = "Sim. Sphere SN";

                this._status.State = EKernelState.Ready;
                this._sptMeter = null;
                this._srcMeter = null;
                this._lcrMeter = null;
                this._lcrBias = null;
                return true;
            }
            return false;
        }

        protected virtual bool SetSptMeter()
        {
            switch (this._machineConfig.SpectrometerModel)
            {
                case ESpectrometerModel.RS_OP:
                    this._machineInfo.TesterModel = "T180";
                    this._sptMeter = new SMUSB();
                    break;
                //-----------------------------------------------------------------
                case ESpectrometerModel.USB2000P:
                    this._machineInfo.TesterModel = "USB2000+";
                    this._sptMeter = new USB2000P(this._machineConfig.spetometerHWSetting);
                    break;
                //-----------------------------------------------------------------
                case ESpectrometerModel.CAS140:
                    this._machineInfo.TesterModel = "IS";
                    this._sptMeter = new MPI.Tester.Device.SpectroMeter.IS.CAS4(this._machineConfig.spetometerHWSetting);
                    break;
                //-----------------------------------------------------------------
                case ESpectrometerModel.LE5400:
                    this._machineInfo.TesterModel = "LE5400";
                    this._sptMeter = new MPI.Tester.Device.SpectroMeter.LE5400.LE5400();
                    break;
                //-----------------------------------------------------------------
                case ESpectrometerModel.NONE:
                    this._machineInfo.TesterModel = "NO Spectrometer";
                    this._sptMeter = null;
                    break;
                //-----------------------------------------------------------------
                case ESpectrometerModel.HR2000P:
                    this._machineInfo.TesterModel = "HR2000+";
                    this._sptMeter = new MPI.Tester.Device.SpectroMeter.HR2000P(this._machineConfig.spetometerHWSetting);
                    break;
                //-----------------------------------------------------------------
                case ESpectrometerModel.HR4000:
                    this._machineInfo.TesterModel = "HR4000";
                    this._sptMeter = new MPI.Tester.Device.SpectroMeter.HR4000(this._machineConfig.spetometerHWSetting);
                    break;
                //-----------------------------------------------------------------
                default:
                    this._sptMeter = null;
                    break;
            }

            if (this._sptMeter != null)
            {
                if (this._sptMeter.Init(0, this._machineConfig.SpectrometerSN, this._machineConfig.SphereSN))
                {
                    this._machineInfo.IsSptInitSuccess = true;
                    this._sptMeter.GetXWavelength();
                    this._machineInfo.SpectrometerSN = this._sptMeter.SerialNumber;
                    this._machineInfo.SphereSN = string.Copy(this._machineConfig.SphereSN);
                    this._machineInfo.EPPROMConfigData = this._sptMeter.GetEPPROMConfigData();

                    Console.WriteLine(string.Format("[TesterKernel_Base], Init(), Spectrometer: {0}", this._sptMeter.SerialNumber));
                }
                else
                {
                    this.SetErrorCode(this._sptMeter.ErrorNumber);

                    Console.WriteLine(string.Format("[TesterKernel_Base], Init(), Spectrometer Init Fail (Err{0})", (int)this._sptMeter.ErrorNumber));

                    this._sptMeter = null;

                    return false;
                }
            }
            return true;
        }

        protected virtual ESDDevSetting SetESDDevSetting(ESDDevSetting esdSet = null)
        {
            ESDDevSetting esdSetting = esdSet;

            if (esdSet == null)
            {
                esdSetting = new ESDDevSetting();
            }
            esdSetting.ChannelAssignment = _machineConfig.ChannelConfig.AssignmentTable;

            esdSetting.IsHighSpeedMode = this._rdFunc.RDFuncData.IsEnableESDHighSpeedMode;

            esdSetting.HighSpeedDelayTime = this._rdFunc.RDFuncData.ESDHighSpeedDelayTime;

            esdSetting.PrechargeWaitTime = this._rdFunc.RDFuncData.ESDPrechargeWaitTime;
            return esdSetting;
        }

        protected virtual bool SetEsdDevice(ESDDevSetting esdSetting)
        {
            switch (this._machineConfig.ESDModel)
            {
                //case EESDModel.ESD_PLC:
                //    this._esdDevice = null;
                //    break;
                //-----------------------------------------------------------------
                case EESDModel.ESD_PCA:
                    this._esdDevice = new ESDCtrlPCI(esdSetting);

                    if (this._esdDevice.Open())
                    {
                        this._machineInfo.IsEsdInitSuccess = true;
                        this._machineInfo.EsdSN = this._esdDevice.SerialNumber;
                        this._machineInfo.EsdHbmRelayCount = this._esdDevice.HardwareInfo.HBMRelayCount;
                        this._machineInfo.EsdMmRelayCount = this._esdDevice.HardwareInfo.MMRelayCount;
                        this._machineInfo.EsdHbmRelayCount2 = this._esdDevice.HardwareInfo.HBMRelayCount2;
                        this._machineInfo.EsdMmRelayCount2 = this._esdDevice.HardwareInfo.MMRelayCount2;
                        this._machineInfo.EsdHbmRelayCount3 = this._esdDevice.HardwareInfo.HBMRelayCount3;
                        this._machineInfo.EsdMmRelayCount3 = this._esdDevice.HardwareInfo.MMRelayCount3;
                        this._machineInfo.EsdHbmRelayCount4 = this._esdDevice.HardwareInfo.HBMRelayCount4;
                        this._machineInfo.EsdMmRelayCount4 = this._esdDevice.HardwareInfo.MMRelayCount4;
                    }
                    else
                    {
                        this.SetErrorCode(this._esdDevice.ErrorNumber);
                        this._esdDevice = null;
                        return false;
                    }
                    break;
                //-----------------------------------------------------------------
                case EESDModel.NONE:
                    this._esdDevice = null;
                    break;
                //-----------------------------------------------------------------
                default:
                    this._esdDevice = null;
                    break;
            }
            return true;
        }

        protected virtual bool SetSwitchBox()
        {
            SwitchSettingData switchSetting = new SwitchSettingData();

            switch (this._machineConfig.SwitchSystemModel)
            {
                case ESwitchSystemModel.NONE:
                    this._switchDevice = null;
                    break;
                case ESwitchSystemModel.MFB:
                    this._switchDevice = new MultiFuncBox(switchSetting);
                    break;
                case ESwitchSystemModel.K3706A:
                    this._switchDevice = new K3706A(switchSetting);
                    break;
                default:
                    this._switchDevice = null;
                    break;
            }

            if (this._switchDevice != null)
            {
                if (this._switchDevice.Init(this._machineConfig.SwitchSystemSN))
                {
                    this._machineInfo.IsSwitchInitSuccess = true;

                    this._machineInfo.SwitchSystemSN = this._switchDevice.SerialNumber;

                    this._machineInfo.MaxSwitchingChannelCount = this._switchDevice.MaxSwitchingChannelCount;

                    //this._machineInfo.SwitchChannelNames = this._switchDevice.SwitchChannelNames;

                    this._switchDevice.EnableCH(0);  // Single Die can use Switch (Enable channel 0)

                    Console.WriteLine(string.Format("[TesterKernel_Base], Init(), Switch System: {0}", this._switchDevice.SerialNumber));
                }
                else
                {
                    this.SetErrorCode(this._switchDevice.ErrorNumber);

                    Console.WriteLine(string.Format("[TesterKernel_Base], Init(), Switch System Init Fail (Err{0})", (int)this._switchDevice.ErrorNumber));

                    this._switchDevice = null;

                    return false;
                }
            }
            return true;
        }

        protected virtual LCRDevSetting SetLCRDevSetting(LCRDevSetting lcrDevSett = null)
        {
            LCRDevSetting lcrDevSetting = lcrDevSett;
            if (lcrDevSett == null)
            {
                lcrDevSetting = new LCRDevSetting();
            }
            lcrDevSetting.LCRDCBiasType = this._machineConfig.LCRDCBiasType;

            return lcrDevSetting;
        }

        protected virtual bool SetLcrMeter(LCRDevSetting lcrDevSetting, ElecDevSetting devSetting)
        {
            switch (this._machineConfig.LCRModel)
            {
                case ELCRModel.LCR4284A:
                    this._lcrMeter = new AGILENT_4284A(lcrDevSetting);
                    break;
                case ELCRModel.E4980A:
                    this._lcrMeter = new E4980A(lcrDevSetting);
                    break;
                //-----------------------------------------------------------------
                case ELCRModel.WK4100:
                    this._lcrMeter = new WK4100(lcrDevSetting);
                    break;
                //-----------------------------------------------------------------
                case ELCRModel.WK6500:
                    this._lcrMeter = new WK6500(lcrDevSetting);
                    break;
                //-----------------------------------------------------------------
                case ELCRModel.eWK6101:
                    this._lcrMeter = new eWK6101(lcrDevSetting);
                    break;
                //-----------------------------------------------------------------
                case ELCRModel.IM3536:
                    this._lcrMeter = new IM3536(lcrDevSetting);
                    break;
                //-----------------------------------------------------------------
                case ELCRModel._3506_10:
                    this._lcrMeter = new _3506_10(lcrDevSetting);
                    break;
                //-----------------------------------------------------------------
                case ELCRModel.HP4278A:
                    this._lcrMeter = new HP4278A(lcrDevSetting);
                    break;
                //-----------------------------------------------------------------
                default:
                    this._lcrMeter = null;
                    break;
            }

            if (this._lcrMeter != null)
            {
                Console.WriteLine("[TesterKernel_Base], Init(), LCR: " + this._machineConfig.LCRModel.ToString());

                if (this._lcrMeter.Init(0, this._machineConfig.LCRMeterSN))
                {
                    this._machineInfo.IsLcrInitSuccess = true;
                    Console.WriteLine("[TesterKernel_Base], Init(), LCR init Success");
                }
                else
                {
                    Console.WriteLine("[TesterKernel_Base], Init(), LCR Init Fail");

                    this.SetErrorCode(this._lcrMeter.ErrorNumber);
                }

                this._machineInfo.LCRMeterSpec = this._lcrMeter.Spec;
                this._machineInfo.TesterSN = this._lcrMeter.SerialNumber + "_ " +
                                                this._lcrMeter.HardwareVersion + "_" +
                                                this._lcrMeter.SoftwareVersion;

                this._machineInfo.LCRMeterSN = this._lcrMeter.SerialNumber;
                this._machineInfo.LCRMeterHWVersion = this._lcrMeter.HardwareVersion;
                this._machineInfo.LCRMeterSWVersion = this._lcrMeter.SoftwareVersion;

                if (this._lcrMeter.ErrorNumber != EDevErrorNumber.Device_NO_Error)
                {
                    this._lcrMeter = null;

                    return false;
                }

                if (this._machineConfig.LCRDCBiasType == ELCRDCBiasType.Ext_Other ||
                this._machineConfig.LCRDCBiasType == ELCRDCBiasType.Other)
                {
                    switch (this._machineConfig.LCRDCBiasSource)
                    {
                        //-----------------------------------------------------------------
                        case ELCRDCBiasSource.K2600:
                            if (this._machineConfig.SourceMeterModel != ESourceMeterModel.K2600)
                            {
                                devSetting.HWConnectorType = EHWConnectType.TCPIP;
                                devSetting.SrcTriggerMode = ESMUTriggerMode.Single;
                                this._lcrBias = new Keithley2600(devSetting);
                            }
                            break;
                        //-----------------------------------------------------------------
                        case ELCRDCBiasSource.N5700:
                            devSetting.HWConnectorType = EHWConnectType.USB;
                            this._lcrBias = new N5751A(devSetting);
                            break;
                        //-----------------------------------------------------------------
                        default:
                            this._lcrBias = null;
                            this.SetErrorCode(this._lcrBias.ErrorNumber);
                            break;
                    }

                    if (this._lcrBias != null)
                    {
                        if (this._lcrBias.Init(0, this._machineConfig.LCRDCBiasSourceSN))
                        {
                            this._lcrBias.Reset();

                            Console.WriteLine(string.Format("[TesterKernel_Base], Init(), _lcrBias: {0}, {1}", this._lcrBias.HardwareVersion, this._lcrBias.SerialNumber));
                        }
                        else
                        {
                            this.SetErrorCode(this._lcrBias.ErrorNumber);

                            Console.WriteLine(string.Format("[TesterKernel_Base], Init(), _lcrBias Init Fail (Err{0})", (int)this._lcrBias.ErrorNumber));
                        }

                        this._machineInfo.DCBiasSpec = this._lcrBias.Spec;

                        if (this._lcrBias.ErrorNumber != EDevErrorNumber.Device_NO_Error)
                        {
                            this._lcrBias = null;

                            return false;
                        }
                    }

                    this._machineInfo.SourceMeterSN = this._srcMeter.SerialNumber;
                    this._machineInfo.SourceMeterHWVersion = this._srcMeter.HardwareVersion;
                    this._machineInfo.SourceMeterSWVersion = this._srcMeter.SoftwareVersion;
                }
                else
                {
                    this._lcrBias = null;
                }
            }

            return true;
        }

        protected virtual bool SetOSA()
        {
            switch (this._machineConfig.OSAModel)
            {
                case EOSAModel.NONE:
                    {
                        this._osaDevice = null;
                        break;
                    }
                case EOSAModel.MS9740A:
                    {
                        this._osaDevice = new MPI.Tester.Device.OSA.MS9740A();
                        break;
                    }
            }

            if (this._osaDevice != null)
            {
                if (this._osaDevice.Init(this._machineConfig.OSASN))
                {
                    this._machineInfo.IsOsaInitSuccess = true;
                    this._machineInfo.OsaSN = this._osaDevice.SerialNumber;
                    Console.WriteLine(string.Format("[TesterKernel_Base], Init(), OSA: {0}", this._osaDevice.SerialNumber));
                }
                else
                {
                    this.SetErrorCode(this._osaDevice.ErrorNumber);

                    Console.WriteLine(string.Format("[TesterKernel_Base], Init(), OSA Init Fail (Err{0})", (int)this._osaDevice.ErrorNumber));

                    this._osaDevice = null;
                }
            }
            return true;
        }

        protected virtual bool SetLaserSource()
        {
            Dictionary<string, object> keyObjDic = new Dictionary<string, object>();
            if (_srcMeter != null && _srcMeter is Keithley2600) { keyObjDic.Add("K2600", (_srcMeter as Keithley2600)); }

            if (_machineConfig.LaserSrcSysConfig != null)
            {
                _machineConfig.LaserSrcSysConfig.ModifyDevSetCh();
            }
            if (_laserSrcSys.Init(_machineConfig.LaserSrcSysConfig, keyObjDic))
            {
                //if()
                this._machineInfo.ChLaserSysSpecDic = _laserSrcSys.GetChSpecDic();

                var devLogDic = _laserSrcSys.GetDevLog();
                if (devLogDic != null && _machineInfo.SNDeviceRelayDic != null)
                {
                    foreach (var dev in devLogDic)
                    {
                        _machineInfo.SNDeviceRelayDic.Add(dev.Key, (dev.Value as MPI.Tester.DeviceCommon.DeviceLogger.DeviceRelayInfoBase).Clone() as
                            MPI.Tester.DeviceCommon.DeviceLogger.DeviceRelayInfoBase);
                    }
                }
            }
            else
            {
                if (_laserSrcSys.ErrorNumber != EDevErrorNumber.Device_NO_Error)
                {
                    this.SetErrorCode(_laserSrcSys.ErrorNumber);
                    Console.WriteLine(string.Format("[TesterKernel_Base], SetLaserSource(), _laserSrcSys Init Fail (Err{0})", _laserSrcSys.ErrorNumber.ToString()));
                    _laserSrcSys = null;
                    return false;
                }
            }

            return true;
        }

     
        #endregion

        #region >>> Public Virtual Method <<<
        public virtual void Init<T, K>(T config, K rdFunc)
        {
            //---------------------------------------------------------------------------------------
            // (1) Load the machine config file
            //---------------------------------------------------------------------------------------
            this.LoadMachineCfgFile();

            //this.LoadRDFuncParam();
            this._rdFunc = rdFunc as RDFunc;
            //---------------------------------------------------------------------------------------
            // (2) System set to simulator mode
            //---------------------------------------------------------------------------------------
            bool isSimu = SetSimuMode();
            if (isSimu)
            {
                return;
            }
            //return false;
            //---------------------------------------------------------------------------------------
            // (3) Create Source Meter Instance and Initialize it 
            //---------------------------------------------------------------------------------------
            ElecDevSetting devSetting = SetElecDevsetting();

            if (!SetSrcMeter(devSetting))
            {
                return;
            }

            //---------------------------------------------------------------------------------------
            // (4) Create the spectrometer instance and initialize it, 
            //      Then test the trigger function, and get data from sepectrometer
            //		Get the wavelength of each pixel at spectrometer 
            //---------------------------------------------------------------------------------------
            if (!SetSptMeter())
            {
                return;
            }
            //---------------------------------------------------------------------------------------
            // (5) Create ESDCtrl Instance and Initialize it 
            //---------------------------------------------------------------------------------------
            ESDDevSetting esdSetting = SetESDDevSetting();

            if (!SetEsdDevice(esdSetting))
            {
                return;
            }
            //---------------------------------------------------------------------------------------
            // (6) Create SwitchBox Instance and Initialize it 
            //---------------------------------------------------------------------------------------
            if (!SetSwitchBox())
            {
                return;
            }
            //---------------------------------------------------------------------------------------
            // (10) Initialize PrintPort 
            //---------------------------------------------------------------------------------------
            this._IOPort = new PortAccess();

            //---------------------------------------------------------------------------------------
            // (11) Initialize LaserPostCalc 
            //---------------------------------------------------------------------------------------
            this._laserPostCalc = new MpiLaserPostCalc();

            //---------------------------------------------------------------------------------------
            // (12) All hardware are created and finish intialization. 
            //      The kernel system transfer to read state
            //---------------------------------------------------------------------------------------
            this._status.State = EKernelState.Ready;

            return;
        }

        public virtual void Close()
        {
            if (this._srcMeter != null)
            {
                this._srcMeter.Close();
            }

            if (this._sptMeter != null)
            {
                this._sptMeter.Close();
            }

            if (this._esdDevice != null)
            {
                this._esdDevice.Close();
            }

            if (this._switchDevice != null)
            {
                this._switchDevice.Close();
            }

            if (this._ADCard != null)
            {
                this._ADCard.DO(0, 0);
                this._ADCard.Close();
            }

            if (this._laserSrcSys != null)
            {
                this._laserSrcSys.Close();
            }

            this._status.State = EKernelState.Not_Ready;
        }

        public virtual void ResetTesterCond()
        {
            //this.CheckDeviceErrorState();
            return;
        }

        public virtual void FireDataWriteEvent<T>(T arg) { }

        public virtual string[] GetBinGradeNames()
        {
            string[] str = new string[this._binCtrl.SmartBin.Count];

            for (int i = 0; i < str.Length; i++)
            {
                str[i] = this._binCtrl.SmartBin[i].BinCode;
            }

            return str;
        }

        #endregion

        #region >>> Public Abstract Method <<<

        public abstract bool SetSysData<T, M>(T sysData, M sysCail);
        public abstract bool SetCondionData<T, K, M>(T condition, K product, M binSort);
        public abstract bool RunCommand(int command);
        public abstract void GetTestedDataFromDevice();


        #endregion
    }

    public class ReTestManager : Dictionary<string, DUTInfo>
    {
        public ReTestManager()
            : base()
        { }

        public void Push(int col, int row, int binSN, bool pass)
        {
            DUTInfo di = new DUTInfo();
            di.COL = col;
            di.ROW = row;
            di.BinSN = binSN;
            di.PASS = pass;
            string key = "X" + col.ToString() + "Y" + row.ToString();
            if (this.ContainsKey(key))
            {
                this[key] = di;
            }
            else
            {
                this.Add(key, di);
            }
        }

        public DUTInfo FindDUT(int col, int row)
        {
            string key = "X" + col.ToString() + "Y" + row.ToString();
            if (this.ContainsKey(key))
            {
                return this[key];
            }
            return null;
        }

    }
    public class DUTInfo
    {
        public int COL { set; get; }
        public int ROW { set; get; }
        public bool PASS { set; get; }
        public int BinSN { set; get; }
    }

    public class IOStateCheck
    {

        public IOStateCheck()
        {
            Watch_ExceptStateDic = new Dictionary<int, int>();
            Watch_OrderDic = new Dictionary<int, uint>();
            IsNeedCheckIO = false;
        }

        public void Clear()
        {
            Watch_OrderDic.Clear();
            Watch_ExceptStateDic.Clear();
        }
        public Dictionary<int, int> Watch_ExceptStateDic { set; get; }
        public Dictionary<int, uint> Watch_OrderDic { set; get; }
        public bool IsNeedCheckIO { set; get; }

    }
}

		
