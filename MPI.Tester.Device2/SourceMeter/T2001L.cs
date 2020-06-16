using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using MPI.Tester.DeviceCommon;

using MPI.Tester.Device.SourceMeter.TSE;

namespace MPI.Tester.Device.SourceMeter
{
    public class T2001L : ISourceMeter
    {
        private object _lockObj;

        #region >>> private Constan Property <<<

        private const uint PMU_COUNT      = 2;
        private const int PMU_NORMAL_MODE = 0;
        private const int PMU_HV_MODE     = 1;

        private const uint MAX_ITEM_SETTING_LENGTH      = 30;
        private const uint MAX_RESULT_HEAD_LENGTH       = 300;
        private const uint MAX_RESULT_DATA_LENGTH       = 320;

        private const uint MAX_SWEEP_ITEMS_COUNT        = 4;
        private const int MAX_THY_POINTS                = 500;
        private const int MAX_SWEEP_MODE_POINTS         = 500;
        private const int MAX_LINEAR_INCREASE_POINTS    = 250;
         
        private const double DEVICE_THY_SCAN_FREQUENCY  = 0.000011d;  // T2001L THY Scan : 11 us      

        private const int DUTY_HV_CURRENT_INDEX       = 6;    // 500 mA
        private const int DUTY_NORMAL_CURRENT_INDEX_1 = 7;    // 1 A
        private const int DUTY_NORMAL_CURRENT_INDEX_2 = 8;    // 2 A

        private const double DUTY_RATE_1 = 0.5d;  // 50% Duty Cycle
        private const double DUTY_RATE_2 = 0.1d;  // 10% Duty Cycle
        private const double DUTY_MAX_APPLY_TIME_1 = 100.0d;   // ms
        private const double DUTY_MAX_APPLY_TIME_2 = 1000.0d;

        #endregion

        // PMU = 0 (Normal), PMU = 1 (HV)   
        private static double[][] _voltRange = new double[][]	// [ PMU Index ][ Volt. Range Index ] , unit = V 
												{	
													new double[] { 5.0d,     10.0d,     20.0d }, 
													new double[] { 50.0d,    80.0d } 
												};

        private static double[][] _currRange = new double[][]  // [ PMU Index ][ Current Range Index ] , unit = A
												{	
													new double[] { 0.000005d,     0.00001d,     0.0001d,      0.001d,      0.01d,    0.1d,    0.5d,    1.0d,     2.0d },    
													new double[] { 0.000005d,     0.00001d,     0.0001d,      0.001d,      0.01d,    0.1d,    0.5d } 
												};

        private TSEWrapper _tseWrapper;
        private ElectSettingData[] _elcSetting;
        private ElecDevSetting _devSetting;
        private EDevErrorNumber _errorNum;

        private bool _isAutoTurnOff;
        private int _sweepItemsCount;

        private double[][] _acquireData;
        private double[][] _sweepResult;
        private double[][] _timeChain;
        private double[][] _applyData;				//  [ Setting Item Index ] [ raw data or sweep data length ] 
       
        private string _hwVersion;
        private string _swVersion;
        private string _serialNum;

        private uint _polarModeIndex;

        private SourceMeterSpec _spec;

        public T2001L()
        {
            this._tseWrapper = new TSEWrapper();
            
            this._elcSetting = new ElectSettingData[MAX_ITEM_SETTING_LENGTH];

            this._hwVersion = "HW NONE";
            this._swVersion = "SW NONE";
            this._serialNum = "SN NONE";
          
            this._devSetting = new ElecDevSetting();

            this._isAutoTurnOff = true;
            this._acquireData = null;

            this._polarModeIndex = 0;

            this._spec = new SourceMeterSpec();
        }

        public T2001L(ElecDevSetting setting) : this()
        {
			this._devSetting = setting;

            this._devSetting.DeviceCalibrationMode = 1;     // [0] None, [1] by SW, [2] by FW
            this._devSetting.IsDevicePeakFiltering = true;  // FW THY Peak Filtering
        }

        #region >>> Public Proberty <<<

        public string SerialNumber
        {
            get { return this._serialNum; }
        }

        public string SoftwareVersion
        {
            get { return this._swVersion; }
        }

        public string HardwareVersion
        {
            get { return ("T2001L"); }
        }

        public EDevErrorNumber ErrorNumber
        {
            get { return this._errorNum; }
        }

        public ElectSettingData[] ElecSetting
        {
            get 
            {
                if (this._elcSetting == null)
                {
                    return null;
                }

                ElectSettingData[] data = new ElectSettingData[this._elcSetting.Length];

                for (int i = 0; i < this._elcSetting.Length; i++)
                {
                    data[i] = this._elcSetting[i].Clone() as ElectSettingData;
                }

                return data;
            }
        }

        public SourceMeterSpec Spec
        {
            get { return this._spec; }
        }

        #endregion

        #region >>> Private Method <<<

        private bool FindPMUIndex(ElectSettingData setting)
        {
            bool[] forcePMUArray = new bool[PMU_COUNT];
            bool[] msrtPMUArray = new bool[PMU_COUNT];
            int[] forceIndexArray = new int[PMU_COUNT];
            int[] msrtIndexArray = new int[PMU_COUNT];

            for (uint pmuIndex = 0; pmuIndex < PMU_COUNT; pmuIndex++)
            {
                switch (setting.MsrtType)
                {
                    case EMsrtType.THY:
                    case EMsrtType.FIMV:
                    case EMsrtType.FI:
                    case EMsrtType.POLAR:
                        forceIndexArray[pmuIndex] = this.FindCurrentIndex(pmuIndex, setting.ForceValue);
                        if (forceIndexArray[pmuIndex] == -1)
                        {
                            forcePMUArray[pmuIndex] = false;
                        }
                        else
                        {
                            forcePMUArray[pmuIndex] = true;
                        }

                        msrtIndexArray[pmuIndex] = this.FindVoltageIndex(pmuIndex, setting.MsrtRange);
                        if (msrtIndexArray[pmuIndex] == -1)
                        {
                            msrtPMUArray[pmuIndex] = false;
                        }
                        else
                        {
                            msrtPMUArray[pmuIndex] = true;
                        }
                        break;
         
                    //------------------------------------------------------------------------------------------------------
                    case EMsrtType.FVMI:
                        forceIndexArray[pmuIndex] = this.FindVoltageIndex(pmuIndex, setting.ForceValue);
                        if (forceIndexArray[pmuIndex] == -1)
                        {
                            forcePMUArray[pmuIndex] = false;
                        }
                        else
                        {
                            forcePMUArray[pmuIndex] = true;
                        }

                        msrtIndexArray[pmuIndex] = this.FindCurrentIndex(pmuIndex, setting.MsrtRange);
                        if (msrtIndexArray[pmuIndex] == -1)
                        {
                            msrtPMUArray[pmuIndex] = false;
                        }
                        else
                        {
                            msrtPMUArray[pmuIndex] = true;
                        }
                        break;
                    //------------------------------------------------------------------------------------------------------                
                    case EMsrtType.FIMVSWEEP:
                        forceIndexArray[pmuIndex] = this.FindCurrentIndex(pmuIndex, setting.SweepStop);
                        if (forceIndexArray[pmuIndex] == -1)
                        {
                            forcePMUArray[pmuIndex] = false;
                        }
                        else
                        {
                            forcePMUArray[pmuIndex] = true;
                        }

                        msrtIndexArray[pmuIndex] = this.FindVoltageIndex(pmuIndex, setting.MsrtRange);
                        if (msrtIndexArray[pmuIndex] == -1)
                        {
                            msrtPMUArray[pmuIndex] = false;
                        }
                        else
                        {
                            msrtPMUArray[pmuIndex] = true;
                        }
                        break;
                    //------------------------------------------------------------------------------------------------------
                    case EMsrtType.FVMISWEEP:
                        forceIndexArray[pmuIndex] = this.FindVoltageIndex(pmuIndex, setting.SweepStop);
                        if (forceIndexArray[pmuIndex] == -1)
                        {
                            forcePMUArray[pmuIndex] = false;
                        }
                        else
                        {
                            forcePMUArray[pmuIndex] = true;
                        }

                        msrtIndexArray[pmuIndex] = this.FindCurrentIndex(pmuIndex, setting.MsrtRange);
                        if (msrtIndexArray[pmuIndex] == -1)
                        {
                            msrtPMUArray[pmuIndex] = false;
                        }
                        else
                        {
                            msrtPMUArray[pmuIndex] = true;
                        }
                        break;
                    //------------------------------------------------------------------------------------------------------
                    default:
                        forcePMUArray[pmuIndex] = false;
                        msrtPMUArray[pmuIndex] = false;
                        forceIndexArray[pmuIndex] = -1;
                        msrtIndexArray[pmuIndex] = -1;
                        break;
                }
            }

            uint i = 0;
            for (i = 0; i < PMU_COUNT; i++)
            {
                if (forcePMUArray[i] == true && msrtPMUArray[i] == true)
                    break;
            }

            if (i == PMU_COUNT)
            {
                return false;
            }
            else
            {
                setting.PUMIndex = i;
                setting.ForceRangeIndex = forceIndexArray[i];
                setting.MsrtRangeIndex = msrtIndexArray[i];

                switch (setting.MsrtType)
                {
                    case EMsrtType.FI:
                    case EMsrtType.FIMV:
                    case EMsrtType.FIMVSWEEP:
                    case EMsrtType.THY:
                    case EMsrtType.POLAR:
                        setting.ForceRange = _currRange[setting.PUMIndex][setting.ForceRangeIndex];
                        break;
                    case EMsrtType.FV:
                    case EMsrtType.FVMI:
                    case EMsrtType.FVMISWEEP:
                        setting.ForceRange = _voltRange[setting.PUMIndex][setting.ForceRangeIndex];
                        break;
                }          
                return true;
            }
        }

        private int FindCurrentIndex(uint pmuIndex, double current)
        {
            int index = 0;
            double deltaValue = 0.0d;

            if (pmuIndex >= _currRange.Length)
                return -1;

            for (index = 0; index < _currRange[pmuIndex].Length; index++)
            {
                deltaValue = Math.Abs(current) - _currRange[pmuIndex][index];
                if (deltaValue < 0.0d || Math.Abs(deltaValue) <= Double.Epsilon)
                    break;
            }

            if (index == _currRange[pmuIndex].Length)
            {
                return -1;
            }
            else
            {
                return index;
            }
        }

        private int FindVoltageIndex(uint pmuIndex, double voltage)
        {
            int index = 0;
            double deltaValue = 0.0d;

            if (pmuIndex >= _voltRange.Length)
                return -1;

            for (index = 0; index < _voltRange[pmuIndex].Length; index++)
            {
                deltaValue = Math.Abs(voltage) - _voltRange[pmuIndex][index];

                if (deltaValue < 0.0d || Math.Abs(deltaValue) <= Double.Epsilon)
                    break;
            }

            if (index == _voltRange[pmuIndex].Length)
            {
                return -1;
            }
            else
            {
                return index;
            }
        }

        private bool CheckClampAndMsrtRange(ElectSettingData setting)
        {                
            if (Math.Abs(setting.MsrtProtection) > Math.Abs(setting.MsrtRange))
            {
                setting.MsrtProtection = setting.MsrtRange;
                return false;
            }

            if (setting.MsrtType == EMsrtType.FIMVSWEEP || setting.MsrtType == EMsrtType.FVMISWEEP)
            {
                // IVSweep, Force Value (Sweep End Clamp) 需隨著極性作切換

                if (setting.SweepStart >= 0)
                {
                    setting.ForceValue = setting.ForceRange;
                }
                else
                {
                    setting.ForceValue = setting.ForceRange * (-1);
                }                                            
            }

            return true;
        }

        private bool CheckSweepSetting(ElectSettingData setting)
        {
            if (setting.MsrtType == EMsrtType.FIMVSWEEP || setting.MsrtType == EMsrtType.FVMISWEEP)
            {
                this._sweepItemsCount++;
                if (setting.SweepContCount == 0)
                {
                    // FW Sweep_Mode 1 & 2
                    if (setting.SweepRiseCount > MAX_SWEEP_MODE_POINTS)
                    {
                        return false;
                    }
                }
                else
                {
                    // FW Linear Increase_Mode 1 & 2
                    if (setting.SweepRiseCount > MAX_LINEAR_INCREASE_POINTS || setting.SweepContCount > MAX_LINEAR_INCREASE_POINTS)
                    {
                        return false;
                    }
                }
            }
            else if (setting.MsrtType == EMsrtType.THY)
            {
                this._sweepItemsCount++;           
                if (setting.SweepContCount > MAX_THY_POINTS)
                {
                    return false;
                }
            }

            if (this._sweepItemsCount > MAX_SWEEP_ITEMS_COUNT)   // 最多只能有 4 道 Sweep Item
                return false;
   
            return true;
        }

        private void THYResultCalculate(uint itemIndex)
        {
            // Calculate the stable value
            int startIndex = this._acquireData[itemIndex].Length - 1;
            int endIndex = startIndex - 20;
            int index = 0;
            double sumStable = 0.0d;

            this._sweepResult[itemIndex][(int)ETHYResultItem.MaxPeak] = 0.0d;
            this._sweepResult[itemIndex][(int)ETHYResultItem.MinPeak] = 0.0d;

            for (int i = 0; i < this._acquireData[itemIndex].Length; i++)
            {
                // Found the Max Peak Value
                if (Math.Abs(this._acquireData[itemIndex][i]) > this._sweepResult[itemIndex][(int)ETHYResultItem.MaxPeak])
                {
                    this._sweepResult[itemIndex][(int)ETHYResultItem.MaxPeak] = Math.Abs(this._acquireData[itemIndex][i]);
                }

                // Found the Min Peak Value
                if (Math.Abs(this._acquireData[itemIndex][i]) < this._sweepResult[itemIndex][(int)ETHYResultItem.MinPeak])
                {
                    this._sweepResult[itemIndex][(int)ETHYResultItem.MinPeak] = Math.Abs(this._acquireData[itemIndex][i]);
                }
            }

            // Found the Stable Value
            for (index = startIndex; index >= 0 && index > endIndex; index--)
            {
                sumStable += this._acquireData[itemIndex][index];
            }

            if (index != 0)
            {
                this._sweepResult[itemIndex][(int)ETHYResultItem.StableValue] = Math.Abs(sumStable) / 20.0d;
            }
            else if (index == 0 && this._acquireData[itemIndex].Length >= 1)
            {
                this._sweepResult[itemIndex][(int)ETHYResultItem.StableValue] = Math.Abs(sumStable) / ((double)this._acquireData[itemIndex].Length);
            }

            this._sweepResult[itemIndex][(int)ETHYResultItem.MaxToStable] = this._sweepResult[itemIndex][(int)ETHYResultItem.MaxPeak] - this._sweepResult[itemIndex][(int)ETHYResultItem.StableValue];

            if (this._sweepResult[itemIndex][(int)ETHYResultItem.MaxPeak] > this._sweepResult[itemIndex][(int)ETHYResultItem.StableValue])
            {
                this._sweepResult[itemIndex][(int)ETHYResultItem.OverShoot] = this._sweepResult[itemIndex][(int)ETHYResultItem.MaxPeak] - this._sweepResult[itemIndex][(int)ETHYResultItem.StableValue];
            }
            else
            {
                this._sweepResult[itemIndex][(int)ETHYResultItem.OverShoot] = 0.0d;
            }           
        }

        private void GetTestResultData(uint itemIndex)
        {
            if (this._elcSetting[itemIndex].MsrtType == EMsrtType.FIMV || this._elcSetting[itemIndex].MsrtType == EMsrtType.FVMI || this._elcSetting[itemIndex].MsrtType == EMsrtType.POLAR)
            {
                this._acquireData[itemIndex][0] = this._tseWrapper.GetDCResult(itemIndex)[0];
            }
            else
            {
                // Protocol Read Cycle TimeOut = 9 s, 此 Delay 確保 Sweep 做完後才取值
                
                double delay = (this._elcSetting[itemIndex].SweepRiseCount + this._elcSetting[itemIndex].SweepContCount) * this._elcSetting[itemIndex].SweepTurnOffTime;
                System.Threading.Thread.Sleep((int)delay);

                this._acquireData[itemIndex] = this._tseWrapper.GetSweepResult(itemIndex);
            }
        }

        private bool DutyRateCalc(ElectSettingData setting, int index)
        {
            double tempTurnOffTime = 0.0d;
            double totalApplyTime = 0.0d;

            // Normal Mode (PMU = 0), HV Mode (PMU = 1)
            switch (setting.MsrtType)
            {
                case EMsrtType.FIMV:
                case EMsrtType.FVMI:
                case EMsrtType.FI:
                case EMsrtType.POLAR:
                    if (setting.IsAutoTurnOff)  // 一般 DC Item 測試項目
                    {
                        if (this._isAutoTurnOff)  // 判斷上一道 Test Item 是否 Source-Off
                        {
                            if ((setting.PUMIndex == PMU_HV_MODE && setting.ForceRangeIndex == DUTY_HV_CURRENT_INDEX) ||   // HV Mode, 輸出/量測 電流 500 mA
                             (setting.PUMIndex == PMU_HV_MODE && setting.MsrtRangeIndex == DUTY_HV_CURRENT_INDEX) ||
                             setting.ForceRangeIndex == DUTY_NORMAL_CURRENT_INDEX_1 || setting.MsrtRangeIndex == DUTY_NORMAL_CURRENT_INDEX_1 ||  //Normal Mode, 輸出/量測 電流 1A, 2A
                             setting.ForceRangeIndex == DUTY_NORMAL_CURRENT_INDEX_2 || setting.MsrtRangeIndex == DUTY_NORMAL_CURRENT_INDEX_2)
                            {
                                if (setting.ForceTime < DUTY_MAX_APPLY_TIME_1)   // duty rate 50%, 0 ms ~ 100 ms
                                {
                                    tempTurnOffTime = (setting.ForceTime / DUTY_RATE_1) - setting.ForceTime;
                                }
                                else if (setting.ForceTime >= DUTY_MAX_APPLY_TIME_1 && setting.ForceTime <= DUTY_MAX_APPLY_TIME_2) // duty rate 10%, 100 ms ~ 1000 ms
                                {
                                    tempTurnOffTime = (setting.ForceTime / DUTY_RATE_2) - setting.ForceTime;
                                }
                                else // the apply time over 1 S
                                {
                                    return false;
                                }
                            }
                        }
                        else  // 此道為 關閉 LOPWL 之第二道電 
                        {
                            if ((setting.PUMIndex == PMU_HV_MODE && setting.ForceRangeIndex == DUTY_HV_CURRENT_INDEX) ||   // HV 50V_500mA, 80V_500mA
                             (setting.PUMIndex == PMU_HV_MODE && setting.MsrtRangeIndex == DUTY_HV_CURRENT_INDEX) ||
                             setting.ForceRangeIndex == DUTY_NORMAL_CURRENT_INDEX_1 || setting.MsrtRangeIndex == DUTY_NORMAL_CURRENT_INDEX_1 ||  //Normal_Mode 5V_1A 2A, 10V_1A 2A, 20V_1A 2A 
                             setting.ForceRangeIndex == DUTY_NORMAL_CURRENT_INDEX_2 || setting.MsrtRangeIndex == DUTY_NORMAL_CURRENT_INDEX_2)
                            {
                                totalApplyTime = setting.ForceTime + this._elcSetting[index - 1].ForceTime + this._elcSetting[index - 1].ForceTimeExt + 10.0d;   // 10 ms for manual run calc time

                                if (totalApplyTime < DUTY_MAX_APPLY_TIME_1)   // duty rate 50%, 0 ms ~ 100 ms
                                {
                                    tempTurnOffTime = (totalApplyTime / DUTY_RATE_1) - totalApplyTime;
                                }
                                else if (totalApplyTime >= DUTY_MAX_APPLY_TIME_1 && totalApplyTime <= DUTY_MAX_APPLY_TIME_2) // duty rate 10%, 100 ms ~ 1000 ms
                                {
                                    tempTurnOffTime = (totalApplyTime / DUTY_RATE_2) - totalApplyTime;
                                }
                                else // 有 Duty Cycle 之測試項目, 最大 Force Time 為 1 秒
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    this._isAutoTurnOff = setting.IsAutoTurnOff;   // record preious isAutoTurnOff
                    break;
                case EMsrtType.FIMVSWEEP:
                case EMsrtType.FVMISWEEP:
                    // HV Sweep Duty Cycle 
                    if ((setting.PUMIndex == PMU_HV_MODE && setting.ForceRangeIndex == DUTY_HV_CURRENT_INDEX) ||   // HV 50V_500mA, 80V_500mA in ISVM and VSIM
                         (setting.PUMIndex == PMU_HV_MODE && setting.MsrtRangeIndex == DUTY_HV_CURRENT_INDEX) ||
                         setting.ForceRangeIndex == DUTY_NORMAL_CURRENT_INDEX_1 || setting.MsrtRangeIndex == DUTY_NORMAL_CURRENT_INDEX_1 ||  //Normal_Mode 5V_1A 2A, 10V_1A 2A, 20V_1A 2A in ISVM and VSIM
                         setting.ForceRangeIndex == DUTY_NORMAL_CURRENT_INDEX_2 || setting.MsrtRangeIndex == DUTY_NORMAL_CURRENT_INDEX_2)
                    {
                        double sweepTimeSpan = 0.0d;

                        if (setting.SweepTurnOffTime == 0)   // 連續輸出之 Sweep, 需以 全部 Sweep 作動時間 計算 Duty Cycle
                        {
                            sweepTimeSpan = (setting.SweepContCount + setting.SweepRiseCount) * (setting.ForceTime + setting.SweepTurnOffTime);

                            if (sweepTimeSpan < DUTY_MAX_APPLY_TIME_1)   // duty rate 50%, 0 ms ~ 100 ms
                            {
                                tempTurnOffTime = (sweepTimeSpan / DUTY_RATE_1) - sweepTimeSpan;
                            }
                            else if (sweepTimeSpan >= DUTY_MAX_APPLY_TIME_1 && sweepTimeSpan <= DUTY_MAX_APPLY_TIME_2) // duty rate 10%, 100 ms ~ 1000 ms
                            {
                                tempTurnOffTime = (sweepTimeSpan / DUTY_RATE_2) - sweepTimeSpan;
                            }
                            else  // 有 Duty Cycle 之測試項目, 最大 Force Time 為 1 秒
                            {
                                return false;
                            }
                        }
                        else  // 脈衝輸出之 Sweep, 需計算 單道 Sweep Point 之 Duty Cycle
                        {
                            if (setting.ForceTime < DUTY_MAX_APPLY_TIME_1)   // duty rate 50%, 0 ms ~ 100 ms
                            {
                                tempTurnOffTime = (setting.ForceTime / DUTY_RATE_1) - setting.ForceTime;
                            }
                            else if (setting.ForceTime >= DUTY_MAX_APPLY_TIME_1 && setting.ForceTime <= DUTY_MAX_APPLY_TIME_2) // duty rate 10%, 100 ms ~ 1000 ms
                            {
                                tempTurnOffTime = (setting.ForceTime / DUTY_RATE_2) - setting.ForceTime;
                            }
                            else // 有 Duty Cycle 之測試項目, 最大 Force Time 為 1 秒
                            {
                                return false;
                            }

                            if (tempTurnOffTime > setting.SweepTurnOffTime)
                            {
                                setting.SweepTurnOffTime = tempTurnOffTime;
                            }
                        }
                    }
                    break;
            }

            setting.TurnOffTime = tempTurnOffTime;      
            return true;
        }

        private void PMUChangeProtection(int index)
        {
            if (index + 1 > this._elcSetting.Length - 1)
            {
                return;
            }
            else
            {
                // ElecSetting PMU  [0] Normal Mode; [1] HV Mode  
                if (this._elcSetting[index].PUMIndex == 0 && this._elcSetting[index + 1].PUMIndex == 1)
                {
                    // PMU Normal -> HV, 保護時間 3 ms
                    if (this._elcSetting[index].TurnOffTime < 2.0d)
                    {
                        this._elcSetting[index].TurnOffTime = 2.0d;
                    }
                }
                else if (this._elcSetting[index].PUMIndex == 1 && this._elcSetting[index + 1].PUMIndex == 0)
                {
                    // PMU HV -> Normal, 保護時間 3 ms
                    if (this._elcSetting[index].TurnOffTime < 2.0d)
                    {
                        this._elcSetting[index].TurnOffTime = 2.0d;
                    }
                }
                else if (this._elcSetting[index].PUMIndex == 1 && this._elcSetting[index + 1].PUMIndex == 1)
                {
                    // PMU HV -> HV, 保護時間 2 ms
                    if (this._elcSetting[index].TurnOffTime < 1.0d)
                    {
                        this._elcSetting[index].TurnOffTime = 1.0d;
                    }
                }
            }
        }

        private ElectSettingData[] CheckPolarMode(ElectSettingData[] settingData)
        {
            bool isPolarMode = false;

            foreach (var item in settingData)
            { 
                if(item.MsrtType == EMsrtType.POLAR)
                {
                    isPolarMode = true;

                    break;
                }
            }

            if (isPolarMode)
            {
                ElectSettingData[] newSettingData = new ElectSettingData[settingData.Length * 2];

                for (int i = 0; i < settingData.Length; i++)
                {
                    newSettingData[i] = settingData[i];
                }

                for (int i = 0; i < settingData.Length; i++)
                {
                    newSettingData[i + settingData.Length] = settingData[i].Clone() as ElectSettingData;

                    newSettingData[i + settingData.Length].ForceValue *= -1;
                }

                return newSettingData;
            }
            else
            {
                return settingData;
            }
        }

        private bool WriteParamToMeter(ElectSettingData[] settingData)
        {
            ElectSettingData setItem;
            int itemIdx = 0;
            double[] sweepPoints = null;
      
            this._elcSetting = settingData;  
            this._errorNum = EDevErrorNumber.Device_NO_Error;

            this._sweepItemsCount = 0;

            if (settingData.Length == 0)
            {
                return true;
            }

            if (settingData.Length > MAX_ITEM_SETTING_LENGTH)
            {
                this._errorNum = EDevErrorNumber.ParameterLengthExcessBufferSize;
                return false;
            }

            this._acquireData = new double[settingData.Length][];
            this._sweepResult = new double[settingData.Length][];
            this._timeChain = new double[settingData.Length][];
            this._applyData = new double[settingData.Length][];

            // Clear Test Item
            this._tseWrapper.ClearTestItem();
       

            for (int i = 0; i < settingData.Length; i++)
            {              
                setItem = settingData[i];
                itemIdx = i;

                //----------------------------------------------------------------------------------------------------
                // (1) Check the Max Value of Sweep Points 
                //----------------------------------------------------------------------------------------------------
                if (!this.CheckSweepSetting(setItem))
                {
                    this._acquireData = null;
                    this._sweepResult = null;
                    this._timeChain = null;
                    this._errorNum = EDevErrorNumber.SweepPointsSetting_Err;
                    return false;
                }

                //----------------------------------------------------------------------------------------------------
                // (2) Find the PMUIndex, Force Range Index, Msrt Range Index
                //----------------------------------------------------------------------------------------------------
                if (!this.FindPMUIndex(setItem))
                {
                    this._acquireData = null;
                    this._sweepResult = null;
                    this._timeChain = null;
                    this._errorNum = EDevErrorNumber.NoMatchRangeIndex;
                    return false;
                }
         
                //----------------------------------------------------------------------------------------------------
                // (3) Check clamp value 
                //----------------------------------------------------------------------------------------------------
                if (!this.CheckClampAndMsrtRange(setItem))
                {
                    this._acquireData = null;
                    this._sweepResult = null;
                    this._timeChain = null;
                    this._errorNum = EDevErrorNumber.ClampValueSetting_Err;
                    return false;
                }
            
                //----------------------------------------------------------------------------------------------------
                // (4) Duty Rate Calculation and PMU Change Protection Time
                //----------------------------------------------------------------------------------------------------
                if (!this.DutyRateCalc(setItem, itemIdx))
                {
                    this._acquireData = null;
                    this._sweepResult = null;
                    this._timeChain = null;
                    this._errorNum = EDevErrorNumber.DutyRate_Err;
                    return false;
                }

                //----------------------------------------------------------------------------------------------------
                // (5) Duty Rate Calculation and PMU Change Protection Time
                //----------------------------------------------------------------------------------------------------
                this.PMUChangeProtection(itemIdx);

                //----------------------------------------------------------------------------------------------------
                // (6) Set Parameter into T2001L
                //----------------------------------------------------------------------------------------------------
                if (!this._tseWrapper.SetTestItem(setItem, itemIdx))
                {
                    this._acquireData = null;
                    this._sweepResult = null;
                    this._timeChain = null;
                    this._errorNum = EDevErrorNumber.ParameterSetting_Err;
                    return false;
                }

                //----------------------------------------------------------------------------------------------------
                // (7) THY Parameter & Sweep Function Calulate
                //----------------------------------------------------------------------------------------------------
                if (setItem.MsrtType == EMsrtType.FIMV || setItem.MsrtType == EMsrtType.FVMI || setItem.MsrtType == EMsrtType.POLAR)
                {
                    this._acquireData[itemIdx] = new double[1];
                    this._applyData[itemIdx] = new double[1];
                }
                else if (setItem.MsrtType == EMsrtType.FIMVSWEEP || setItem.MsrtType == EMsrtType.FVMISWEEP || setItem.MsrtType == EMsrtType.THY)
                {
                    this._acquireData[itemIdx] = new double[setItem.SweepRiseCount + setItem.SweepContCount];		// include sweep start point
                    this._applyData[itemIdx] = new double[1];
                }

                if (setItem.MsrtType == EMsrtType.THY)
                {
                    this._elcSetting[itemIdx].IsAutoTurnOff = true;
                    this._sweepResult[itemIdx] = new double[Enum.GetNames(typeof(ETHYResultItem)).Length];

                    if (this._devSetting.IsDevicePeakFiltering)
                    {
                        this._timeChain[itemIdx] = new double[2];
                        sweepPoints = new double[2];
                        setItem.SweepCustomValue = sweepPoints;
                    }
                    else
                    {                        
                        this._timeChain[itemIdx] = new double[setItem.SweepContCount];
                        
                        sweepPoints = new double[setItem.SweepContCount];

                        for (int j = 0; j < setItem.SweepContCount; j++)
                        {
                            this._timeChain[itemIdx][j] = Math.Round(DEVICE_THY_SCAN_FREQUENCY * j, 6);

                            sweepPoints[j] = setItem.ForceValue;
                        }
                        setItem.SweepCustomValue = sweepPoints;
                    }
                }
                else if (setItem.MsrtType == EMsrtType.FIMVSWEEP || setItem.MsrtType == EMsrtType.FVMISWEEP)
                {
                    this._elcSetting[itemIdx].IsAutoTurnOff = true;
                    this._timeChain[itemIdx] = new double[setItem.SweepContCount + setItem.SweepRiseCount + 1];   // 第一道也算一個 Rise Cnt
                    this._applyData[itemIdx] = new double[setItem.SweepContCount + setItem.SweepRiseCount + 1];

                    sweepPoints = new double[setItem.SweepContCount + setItem.SweepRiseCount + 1];

                    for (int j = 0; j < (setItem.SweepRiseCount + setItem.SweepContCount + 1); j++)
                    {
                        if (j == 0)
                        {
                            this._timeChain[itemIdx][j] = setItem.ForceTime;
                        }
                        else 
                        {
                            this._timeChain[itemIdx][j] = setItem.ForceTime + j * (setItem.ForceTime + setItem.SweepTurnOffTime); 
                        }

                        if (j < setItem.SweepRiseCount)
                        {
                            sweepPoints[j] = Math.Round((setItem.SweepStart + j * setItem.SweepStep), 6);
                        }
                        else
                        {
                            sweepPoints[j] = setItem.SweepStop;
                        }
            
                    }
                    setItem.SweepCustomValue = sweepPoints;
                }

                this._applyData[itemIdx][0] = Math.Abs(setItem.ForceValue);
            }

            return true;
        }

        #endregion

        #region >>> Public Method <<<

		public bool Init(int devNum, string sourceMeterSN)
        {        
            try
            {
                if (this._tseWrapper.Initialize(0) == 0)
                {
                    this.Reset();

                    this._serialNum = this._tseWrapper.GetSerialNum();
                    this._swVersion = this._tseWrapper.GetFwVersion();
     
                    if (!this._tseWrapper.GetGainOffsetFromEPPROM())
                    {
                        this._errorNum = EDevErrorNumber.SourceMeterDevice_Init_Err;
                        return false;
                    }
                }
                else
                {
                    this._errorNum = EDevErrorNumber.SourceMeterDevice_Init_Err;
                    return false;
                }
                return true;
            }
            catch
            {
                this._errorNum = EDevErrorNumber.SourceMeterDevice_HW_Err;
                return false;
            }       
        }

        public void Reset()
        {
            // DIO Reset/Pin
            this._tseWrapper.Reset();     
        }

        public void Close()
        {
            this._tseWrapper.Release();
        }

        public bool SetConfigToMeter(ElecDevSetting devSetting)
        {
            return true;
        }

        public bool SetParamToMeter(ElectSettingData[] settingData)
        {
            if (this._devSetting.IsFastPolar)
            {
                settingData = this.CheckPolarMode(settingData);
            }

            return this.WriteParamToMeter(settingData);
        }

        public bool MeterOutput(uint[] activateChannels, uint itemIndex)
        {
            string msg= "";

            if (itemIndex == 0)
            {
                this._polarModeIndex = 0;
            }

            itemIndex = itemIndex + 1 + this._polarModeIndex;

            if (this._errorNum != EDevErrorNumber.Device_NO_Error)
            {
                return false;
            }

            if (this._elcSetting == null || this._elcSetting.Length == 0)
            {
                this._errorNum = EDevErrorNumber.NoSourceMeterParamSettingData;
                return false;
            }

            if (itemIndex > this._elcSetting.Length)
            {
                this._errorNum = EDevErrorNumber.SourceMeterIndexSetting_Err;
                return false;
            }

            System.Threading.Thread.Sleep((int)this._elcSetting[itemIndex - 1].ForceDelayTime);

            if (!this._tseWrapper.RunTestItem(itemIndex))
            {
                this._tseWrapper.ErrorHandling(out msg);
                this._errorNum = EDevErrorNumber.MeterOutput_Ctrl_Err;
                return false;
            }
 
            if (this._devSetting.IsFastPolar && this._elcSetting[itemIndex - 1].MsrtType == EMsrtType.POLAR)
            {
                double rawData = this._tseWrapper.GetDCResult(itemIndex - 1)[0];

                if (this._elcSetting[itemIndex - 1].ForceValue < 0)
                {
                    rawData *= -1;
                }

				if (rawData > this._elcSetting[itemIndex - 1].PolarThresholdVoltage)
                {
                    if (this._polarModeIndex == 0)
                    {
                        this._polarModeIndex = (uint)(this._elcSetting.Length / 2);
                    }
                    else
                    {
                        this._polarModeIndex = 0;
                    }
                }
            }

            return true;
        }

        public bool MeterOutput(uint[] activateChannels, uint itemIndex, double applyValue)
        {
            return true;
        }

        public double[] GetDataFromMeter(uint channel, uint itemIndex)
        {
            itemIndex = itemIndex + this._polarModeIndex;

            if (this._elcSetting == null || itemIndex > this._elcSetting.Length - 1)
            {
                double[] data = new double[0];
                this._errorNum = EDevErrorNumber.SourceMeterIndexSetting_Err;
                return data;
            }

            this.GetTestResultData(itemIndex);
         
            if (this._polarModeIndex != 0)
            {
                if (this._elcSetting[itemIndex].MsrtType == EMsrtType.POLAR)
                {
                    this._acquireData[itemIndex][0] = 0.0d;
                }
                else
                {
                    this._acquireData[itemIndex][0] *= -1;
                }
            }

            return this._acquireData[itemIndex];
        }

        public double[] GetApplyDataFromMeter(uint channel, uint itemIndex)
        {

            if (this._elcSetting == null || itemIndex > this._elcSetting.Length)
            {
                double[] data = new double[0];
                this._errorNum = EDevErrorNumber.SourceMeterIndexSetting_Err;
                return data;
            }

            return this._applyData[itemIndex];
        }

        public double[] GetSweepPointFromMeter(uint channel, uint itemIndex)
        {
            if (itemIndex > this._elcSetting.Length - 1)
            {
                double[] data = new double[0];
                this._errorNum = EDevErrorNumber.SourceMeterIndexSetting_Err;
                return data;
            }

            return this._elcSetting[itemIndex].SweepCustomValue;
        }

        public double[] GetSweepResultFromMeter(uint channel, uint itemIndex)
        {
            if (itemIndex > this._elcSetting.Length - 1)
            {
                double[] data = new double[0];
                this._errorNum = EDevErrorNumber.SourceMeterIndexSetting_Err;
                return data;
            }

            if (this._devSetting.IsDevicePeakFiltering)
            {
                this._sweepResult[itemIndex][(int)ETHYResultItem.MaxPeak] = Math.Abs(this._acquireData[itemIndex][0]);
                this._sweepResult[itemIndex][(int)ETHYResultItem.StableValue] = Math.Abs(this._acquireData[itemIndex][1]);
                this._sweepResult[itemIndex][(int)ETHYResultItem.MaxToStable] = Math.Abs(this._sweepResult[itemIndex][(int)ETHYResultItem.MaxPeak] - this._sweepResult[itemIndex][(int)ETHYResultItem.StableValue]);
                this._sweepResult[itemIndex][(int)ETHYResultItem.OverShoot] = Math.Abs(this._sweepResult[itemIndex][(int)ETHYResultItem.MaxPeak] - this._sweepResult[itemIndex][(int)ETHYResultItem.StableValue]);
            }
            else
            {
                this.THYResultCalculate(itemIndex);
            }

            return this._sweepResult[itemIndex];
        }

        public double[] GetTimeChainFromMeter(uint channel, uint settingIndex)
        {         
            return this._timeChain[settingIndex];
        }

        public void TurnOff()
        {
            if (this._elcSetting.Length != 0)
            {
                if (this._elcSetting[this._elcSetting.Length - 1].IsAutoTurnOff == false)
                {
                    this._tseWrapper.TurnOff();
                }
            }
        }

        public void TurnOff(double delay, bool isOpenRelay)
        {
            if (this._elcSetting.Length != 0)
            {
                if (this._elcSetting[this._elcSetting.Length - 1].IsAutoTurnOff == false)
                {
                    this._tseWrapper.TurnOff();
                }
            }
            System.Threading.Thread.Sleep((int)delay);
            //this._tseWrapper.Sleep((float)delay);
        }

        public void Output(uint point, bool active)
        {
            
        }

        public byte InputB(uint point)
        {         
            uint inputData = this._tseWrapper.DioInput(0);
            return ((byte)(inputData & 0x07));   // 3 pins
        }

        public byte Input(uint point)
        {
            throw new NotImplementedException();
        }

        public double GetPDDarkSample(int count)
        {
            return 0.0d;
        }

        public bool CheckInterLock()
        {
            return true;
        }

        #endregion
    }
}
