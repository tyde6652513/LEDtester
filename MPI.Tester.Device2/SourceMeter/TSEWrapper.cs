using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MPI.Tester.DeviceCommon;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.TSE
{
    public class TSEWrapper
    {
        private object _lockObj;
        
        private const uint MAX_GAIN_OFFSET_TABLE_LENGTH = 328;
        private const uint MAX_SET_TEST_ITEM_NUMBER = 30;
        private const uint MAX_RESULT_HEAD_LENGTH = 300;
        private const uint MAX_RESULT_DATA_LENGTH = 16;
        private const uint MAX_SWEEP_RESULT_LENGTH = 500;
        private const uint DC_RESULT_LENGTH = 24;

        private static EPMU[] _ePmu = new EPMU[] { EPMU.Normal_Mode, EPMU.HV_Mode };

        private static EVRange[][] _eVoltRange = new EVRange[][]	// [ PMU Index ][ Volt. Range Index ] , unit = V 
												{	
													new EVRange[] { EVRange._5V,     EVRange._10V,    EVRange._20V }, 
													new EVRange[] { EVRange._50V,    EVRange._80V } 
												};

        private static EIRange[][] _eCurrRange = new EIRange[][]  // [ PMU Index ][ Current Range Index ] , unit = A
                                                {	
                                                    new EIRange[] { EIRange._5uA,  EIRange._10uA,  EIRange._100uA,   EIRange._1mA,   EIRange._10mA,   EIRange._100mA,   EIRange._500mA,   EIRange._1A,   EIRange._2A },    
                                                    new EIRange[] { EIRange._5uA,  EIRange._10uA,  EIRange._100uA,   EIRange._1mA,   EIRange._10mA,   EIRange._100mA,   EIRange._500mA } 
                                                };


        private Dictionary<uint, double[]> _dictGainOffsetTable;    // Dictionary of Gain_Offset_Table 
        private List<double[]> _listGainOffsetTable;

        //private ElecDevSetting _devSetting;
        private ElectSettingData[] _elecSetting;

        private RegCmdBase _regBase;
        private RegCmdBase _clearTestItem;
        private RegCmdBase _setTestItem;
        private RegCmdBase _runTestItem;
        private RegCmdBase _spRunTestItem;
        
        private RegCmdBase _stopTestItem;

        private RegCmdBase _triggerOut;
        private RegCmdBase _triggerIn;
        private RegCmdBase _triggerIn1;

        private RegCmdBase _dioOutput;
        private RegCmdBase _dioInput;

        private int _devError;
        private int _devCalMode;
        private bool _isdevSpRunTestItem;
        private bool _isdevPeakFiltering;
        private bool _isGetDCMsrtRawData;
        private int _sweepItemsCount;

        private double[] _calGainOffset;
        private double[] _resultData;
        private double[] _sweepResultData;

        private int[] _forceRangeIndex;
        private int[] _msrtRangeIndex;
        private int[] _msrtCount;

        private uint[] _msrtType;
        private uint[] _msrtPolarity;    
        private uint[] _pmuIndex;       
        private uint[] _sweepCount;

        public TSEWrapper()
        {
            this._lockObj = new object();
            
            this._elecSetting = new ElectSettingData[MAX_SET_TEST_ITEM_NUMBER];

            this._dictGainOffsetTable = new Dictionary<uint, double[]>();
            this._listGainOffsetTable = new List<double[]>();

            this._calGainOffset = new double[2];

            this._resultData = new double[MAX_RESULT_DATA_LENGTH];
            this._sweepResultData = new double[MAX_SWEEP_RESULT_LENGTH];

            this._setTestItem = new CmdSetTestItem();
            this._spRunTestItem = new CmdSpRunTestItem();
            this._runTestItem = new CmdRunTestItem();
            this._clearTestItem = new CmdClearTestItem();
            this._stopTestItem = new CmdStopTestItem();

            this._triggerOut = new CmdRunTriggerOut();
            this._triggerIn = new CmdRunTriggerIn();
            this._triggerIn1 = new CmdRunTriggerIn1();

            this._dioOutput = new CmdRunDioOutput();
            this._dioInput = new CmdRunDioInput();

            this._forceRangeIndex = new int[MAX_SET_TEST_ITEM_NUMBER];
            this._msrtRangeIndex = new int[MAX_SET_TEST_ITEM_NUMBER];
            this._msrtCount = new int[MAX_SET_TEST_ITEM_NUMBER];

            this._msrtPolarity = new uint[MAX_SET_TEST_ITEM_NUMBER];
            this._msrtType = new uint[MAX_SET_TEST_ITEM_NUMBER];  
            this._pmuIndex = new uint[MAX_SET_TEST_ITEM_NUMBER];         
            this._sweepCount = new uint[MAX_SET_TEST_ITEM_NUMBER];

            this._sweepItemsCount = 0;

            this._devError = 0;
            this._devCalMode = 1;   //  None = 0,   bySW = 1,   byFW = 2,                
            this._isdevSpRunTestItem = false;
            this._isdevPeakFiltering = true;
            this._isGetDCMsrtRawData = false;
        }
        
        #region >>> Import DLL <<<

        [DllImport(@"C:\MPI\LEDTester\Driver\PCIInterface.dll")]
        private static extern short PCIInterface_Initial(int CardNumber);

        [DllImport(@"C:\MPI\LEDTester\Driver\PCIInterface.dll")]
        private static extern short PCIInterface_Release();

        [DllImport(@"C:\MPI\LEDTester\Driver\PCIInterface.dll")]
        private static extern short PCIInterface_Reset();

        [DllImport(@"C:\MPI\LEDTester\Driver\PCIInterface.dll")]
        private static extern short PCIInterface_ReadData(ushort Address, out ushort Data);

        [DllImport(@"C:\MPI\LEDTester\Driver\PCIInterface.dll")]
        private static extern short PCIInterface_WriteData(ushort Address, ushort Data, bool isBusy);

        [DllImport(@"C:\MPI\LEDTester\Driver\PCIInterface.dll")]
        private static extern short PCIInterface_WritePort(ushort Data);

        [DllImport(@"C:\MPI\LEDTester\Driver\PCIInterface.dll")]
        private static extern short PCIInterface_ReadPort(out ushort Data);

        [DllImport(@"C:\MPI\LEDTester\Driver\PCIInterface.dll")]
        private static extern void PCIInterface_uSleep(float delay);
        #endregion

        #region >>> Public Proberty <<<

        public List<double[]> GainOffsetTable
        {
            get { return this._listGainOffsetTable; }
        }

        public bool IsSpRunTestItem
        {
            get { return this._isdevSpRunTestItem; }
            set { lock (this._lockObj) { this._isdevSpRunTestItem = value; } }
        }

        public bool IsTHYPeakFiltering
        {
            get { return this._isdevPeakFiltering; }
            set { lock (this._lockObj) { this._isdevPeakFiltering = value; } }
        }

        public bool IsGetDCMsrtRawData
        {
            get { return this._isGetDCMsrtRawData; }
            set { lock (this._lockObj) { this._isGetDCMsrtRawData = value; } }
        }

        public int CalibrationMode
        {
            get { return this._devCalMode; }
            set { lock (this._lockObj) { this._devCalMode = value; } }
        }

        #endregion

        #region >>> Private Method <<<

        private uint DictionaryKeyParser(uint regPolarity, uint calMsrtType, uint regVRange, uint regIRange)
        {
            uint tempKey = 0;
            
            tempKey = (regPolarity << 24);              // PGain / NGain
            tempKey = tempKey | (calMsrtType << 16);    // MsrtType Index
            tempKey = tempKey | regVRange << 8;         // VoltRangeIndex
            tempKey = tempKey | regIRange;              // CurrRangeIndex

            return tempKey;
        }

        private double[] GainOffsetU32ToDouble(uint tempKey, uint[] hGainOffset)
        {
            double[] dGainOffset = new double[2];

            uint calMsrtType = 0;
            uint iRangeIdx = 0;

            iRangeIdx = tempKey & 0x000F;
            calMsrtType = (tempKey >> 16) & 0x000F;

            // Gain Convert
            dGainOffset[0] = TSEConvert.GainOffsetUInt16ToDouble(hGainOffset[0]);

            // Offset Convert
            dGainOffset[1] = TSEConvert.GainOffsetUInt16ToDouble(hGainOffset[1]);

            if ((int)EIRange._500mA == iRangeIdx && ((int)ECalMsrtType.IM == calMsrtType || (int)ECalMsrtType.IS == calMsrtType))
            {
                dGainOffset[1] = dGainOffset[1] * 10.0d;  // IS IM 500mA
            }

            return dGainOffset;
        }

        private double CalForceValue(double value, int index)
        {                                      
            uint tempKey = 0;
            uint polar = 0;
            uint regMsrtType = 0;
            double calValue = 0.0d;

            if (value == 0)
                return 0.0d;

            if (value >= 0)
                polar = (uint)EPolarity.Positive;
            else
                polar = (uint)EPolarity.Negitive;

            value = Math.Abs(value);

            if ((int)ERegMsrtType.FVMI == this._msrtType[index] || (int)ERegMsrtType.FVMISWEEP == this._msrtType[index])
            {
                regMsrtType = (uint)ECalMsrtType.VS;
                tempKey = this.DictionaryKeyParser(polar, regMsrtType, (uint)this._forceRangeIndex[index], (uint)this._msrtRangeIndex[index]);
            }
            else if ((int)ERegMsrtType.FIMV == this._msrtType[index] || (int)ERegMsrtType.FIMVSWEEP == this._msrtType[index] || (int)ERegMsrtType.THY == this._msrtType[index])
            {
                regMsrtType = (uint)ECalMsrtType.IS;
                tempKey = this.DictionaryKeyParser(polar, regMsrtType, (uint)this._msrtRangeIndex[index], (uint)this._forceRangeIndex[index]);
            }

            try
            {
                this._calGainOffset = this._dictGainOffsetTable[tempKey].Clone() as double[];

                if ((int)ECalMsrtType.IS == regMsrtType)
                {
                    if (this._forceRangeIndex[index] < (int)EIRange._1mA)
                    {
                        this._calGainOffset[1] = this._calGainOffset[1] / 1000000.0d;  // uA to A
                    }
                    else if (this._forceRangeIndex[index] >= (int)EIRange._1mA && this._forceRangeIndex[index] < (int)EIRange._1A)
                    {
                        this._calGainOffset[1] = this._calGainOffset[1] / 1000.0d;  // mA to A
                    }
                }

                calValue = (value - this._calGainOffset[1]) / this._calGainOffset[0];
            }
            catch 
            {
                calValue = 0.0d;
            }

            return calValue;
        }

        private double CalMsrtValue(double value, int index)
        {           
            uint tempKey = 0;
  
            double absValue = 0.0d;
            double calValue = 0.0d;
 
            uint regMsrtType = 0;

            absValue = Math.Abs(value);

            if ((int)ERegMsrtType.FVMI == this._msrtType[index] || (int)ERegMsrtType.FVMISWEEP == this._msrtType[index])
            {
                regMsrtType = (uint)ECalMsrtType.IM;
                tempKey = this.DictionaryKeyParser(this._msrtPolarity[index], regMsrtType, (uint)this._forceRangeIndex[index], (uint)this._msrtRangeIndex[index]);
            }
            else if ((int)ERegMsrtType.FIMV == this._msrtType[index] || (int)ERegMsrtType.FIMVSWEEP == this._msrtType[index] || (int)ERegMsrtType.THY == this._msrtType[index])
            {
                regMsrtType = (uint)ECalMsrtType.VM;
                tempKey = this.DictionaryKeyParser(this._msrtPolarity[index], regMsrtType, (uint)this._msrtRangeIndex[index], (uint)this._forceRangeIndex[index]);
            }

            try
            {
                this._calGainOffset = this._dictGainOffsetTable[tempKey].Clone() as double[];

                if ((int)ECalMsrtType.IM == regMsrtType)
                {
                    if (this._msrtRangeIndex[index] < (int)EIRange._1mA)
                    {
                        this._calGainOffset[1] = this._calGainOffset[1] / 1000000.0d;  // uA to A
                    }
                    else if (this._msrtRangeIndex[index] >= (int)EIRange._1mA && this._msrtRangeIndex[index] < (int)EIRange._1A)
                    {
                        this._calGainOffset[1] = this._calGainOffset[1] / 1000.0d;  // mA to A
                    }
                }

                calValue = absValue * this._calGainOffset[0] + this._calGainOffset[1];
            }
            catch
            {
                calValue = 0.0d;
            }

            if (value > 0)
                return calValue;
            else if (value < 0)
                return calValue * (-1);
            else
                return 0.0d;
        }

        private double CalClampValue(double value, double msrtRange, int index)
        {
            uint tempKey = 0;

            double absValue = 0.0d;
            double calValue = 0.0d;
            uint regMsrtType = 0;

            absValue = Math.Abs(value);

            if ((int)ERegMsrtType.FVMI == this._msrtType[index] || (int)ERegMsrtType.FVMISWEEP == this._msrtType[index])
            {
                regMsrtType = (uint)ECalMsrtType.IS;
                tempKey = this.DictionaryKeyParser(this._msrtPolarity[index], regMsrtType, (uint)this._forceRangeIndex[index], (uint)this._msrtRangeIndex[index]);
            }
            else if ((int)ERegMsrtType.FIMV == this._msrtType[index] || (int)ERegMsrtType.FIMVSWEEP == this._msrtType[index] || (int)ERegMsrtType.THY == this._msrtType[index])
            {
                regMsrtType = (uint)ECalMsrtType.VS;
                tempKey = this.DictionaryKeyParser(this._msrtPolarity[index], regMsrtType, (uint)this._msrtRangeIndex[index], (uint)this._forceRangeIndex[index]);
            }

            try
            {
                this._calGainOffset = this._dictGainOffsetTable[tempKey].Clone() as double[];

                if ((int)ECalMsrtType.IS == regMsrtType)
                {
                    if (this._msrtRangeIndex[index] < (int)EIRange._1mA)
                    {
                        this._calGainOffset[1] = this._calGainOffset[1] / 1000000.0d;  // uA to A
                    }
                    else if (this._msrtRangeIndex[index] >= (int)EIRange._1mA && this._msrtRangeIndex[index] < (int)EIRange._1A)
                    {
                        this._calGainOffset[1] = this._calGainOffset[1] / 1000.0d;  // mA to A
                    }
                }

                calValue = (value - this._calGainOffset[1]) / this._calGainOffset[0];
            }
            catch
            {
                calValue = 0.0d;
            }

            if (calValue >= msrtRange)
                calValue = msrtRange;

            return Math.Abs(calValue);
        }

        private double CalSweepStepValue(double startValue, double stepValue, int index)
        {
            return Math.Abs(this.CalForceValue(startValue + stepValue, index) - this.CalForceValue(startValue, index));
        }

        #endregion

        #region >>> Public Method <<<

        #region >>> Protocol <<<

        public int Initialize(int cardNum)
        {
            return PCIInterface_Initial(cardNum);
        }

        public int Release()
        {
            return PCIInterface_Release();
        }

        public int Reset()
        {
            int tempErr = 0;
            tempErr = PCIInterface_Reset();
            System.Threading.Thread.Sleep(1000);    // Wait Time : 1 S 
            return tempErr;
        }

        public int ReadRegisterData(int addr, out uint readData)
        {
            int tempErr = 0;
            ushort tempData = 0;
            tempErr = PCIInterface_ReadData((ushort)addr, out tempData);
            readData = tempData;
            return tempErr;
        }

        public int WriteRegisterData(int addr, int data, bool isBusy)
        {
            return PCIInterface_WriteData((ushort)addr, (ushort)data, isBusy);
        }
       
        public int WritePCIPort(int data)
        {
            int rtn = 0;
            rtn = PCIInterface_WritePort((ushort)data);
            return rtn;
        }

        public int ReadPCIPort(out int data)
        {
            int rtn = 0;
            ushort tempReadData = 0;

            rtn = PCIInterface_ReadPort(out tempReadData);
            data = tempReadData;
            return rtn;
        }

        #endregion

        public int SendCmd(RegCmdBase cmd)
        {
            int rtn = 0;
            rtn = rtn + this.WriteRegisterData(cmd.CmdStartAddr, cmd.CmdID, true);

            if (rtn != 0)
            {
                return rtn;
            }

            if (cmd.ParamLength > 0)
            {
                for (int j = 0; j < cmd.ParamLength; j++)
                {
                    rtn = rtn + this.WriteRegisterData((cmd.ParamStartAddr + j), cmd.GetParamData(j), false);

                    if (rtn != 0)
                    {
                        return rtn;
                    }
                }
            }
            return rtn;
        }

        public string GetSerialNum()
        {
            int errCode = 0;
            uint tempSnMSB = 0;
            uint tempSnLSB = 0;
   
            StringBuilder strBuilder = new StringBuilder();

            errCode = errCode + ReadRegisterData((int)EAddr.DEVICE_ID, out tempSnMSB);       // MSB  FF00: Year,   00FF: Date
            errCode = errCode + ReadRegisterData((int)EAddr.DEVICE_ID + 1, out tempSnLSB);   // LSB  F000: Month,  0FFF: S/N

            strBuilder.AppendFormat("{0:D2}{1:D2}{2:D2}-{3:D4}", ((tempSnMSB >> 8) & 0x0F), ((tempSnLSB >> 12) & 0x000F), (tempSnMSB & 0x00FF), (tempSnLSB & 0x0FFF));

            return strBuilder.ToString();
        }

        public string GetFwVersion()
        {
            int errCode = 0;
            uint tempFwVerMSB = 0;
            uint tempFwVerLSB = 0;

            StringBuilder strBuilder = new StringBuilder();

            errCode = errCode + ReadRegisterData((int)EAddr.FIRMWARE_VERSION, out tempFwVerMSB);       // MSB  (Reserved)
            errCode = errCode + ReadRegisterData((int)EAddr.FIRMWARE_VERSION + 1, out tempFwVerLSB);   // LSB  F:  TSE coustomer num (MPI 1)
            
            strBuilder.AppendFormat("{0:D1}.{1:D1}{2:D1}", ((tempFwVerLSB >> 12) & 0x000F), ((tempFwVerLSB >> 4) & 0x000F), (tempFwVerLSB & 0x000F));

            return strBuilder.ToString();
        }

        public bool GetGainOffsetFromEPPROM()
        {           
            int rtn = 0;
            int regOffset = 0;
            uint tempKey = 0;
            uint regVRange = 0;
            uint regIRange = 0;
            int[] dicKeyArray = new int[MAX_GAIN_OFFSET_TABLE_LENGTH];

            uint[] hexGainOffset = new uint[2];
            double[] gainOffset = new double[2];

            // (1)  Copy EEPROM Gain Offset Data to SRAM 
            if (!this.DeviceAutoCalEnable(ERegAutoCalApply.EEPROMcopy))
            {
                return false;
            }
            
            // (2)  Mapping Gain Offset Data with Key into Dictionary
            for (uint regPloar = 1; regPloar <= Enum.GetNames(typeof(EPolarity)).Length; regPloar++)
            {
                for (uint pmu = 0; pmu < _ePmu.Length; pmu++)
                {                                         
                    for (uint voltIdx = 0; voltIdx < _eVoltRange[pmu].Length; voltIdx++)   
                    {      
                        for (uint calMsrtType = 1; calMsrtType <= Enum.GetNames(typeof(ECalMsrtType)).Length; calMsrtType++)    
                        {                            
                            for (uint currIdx = 0; currIdx < _eCurrRange[pmu].Length; currIdx++)
                            {
                                for (uint idx = 0; idx < hexGainOffset.Length; idx++)
                                {
                                    rtn = ReadRegisterData((int)EAddr.GAIN_OFFSET_START_ADDRESS + regOffset, out hexGainOffset[idx]);
                                    regOffset++;

                                    if (rtn != 0)
                                    {
                                        return false;
                                    }      
                                }

                                regVRange = (uint)_eVoltRange[pmu][voltIdx];
                                regIRange = (uint)_eCurrRange[pmu][currIdx];

                                tempKey = this.DictionaryKeyParser(regPloar, calMsrtType, regVRange, regIRange);
                                gainOffset = this.GainOffsetU32ToDouble(tempKey, hexGainOffset);

                                this._listGainOffsetTable.Add(gainOffset.Clone() as double[]);           // Externel Information
                                this._dictGainOffsetTable.Add(tempKey, gainOffset.Clone() as double[]);  // internel Calibration
                            }
                        }
                    }
                }
            }

            // (3)  F/W AutoCalbySW
            if (!this.DeviceAutoCalEnable(ERegAutoCalApply.bySW))
            {
                return false;
            }
            return true;
        }

        public void ErrorHandling(out string msg)
        {
            uint err = 0;
            this.Reset();
            if (this._devError != (int)ESrcMeterErrorCode.TESTER_NO_ERROR)
            {
                msg = Enum.GetName(typeof(ESrcMeterErrorCode), this._devError);
            }
            else
            {
                this.ReadRegisterData((int)EAddr.ERROR_CODE, out err);
                msg = Enum.GetName(typeof(ESrcMeterErrorCode), err);
            }
            this._devError = (int)ESrcMeterErrorCode.TESTER_NO_ERROR;  // Reset Error status
        }

        public bool ClearTestItem()
        {
            this._sweepItemsCount = 0;
            if (this.SendCmd(this._clearTestItem) != 0)
            {
                return false;
            }
            System.Threading.Thread.Sleep(1);
            return true;
        }

        public bool SetTestItem(ElectSettingData setItem, int idx)  // index : SetTestItemIndex start from 1
        {
            //---------------------------------------------------------------------------------------------------------------------------------------//
            // Test Item No., PMU, ForceRangeIndex, MsrtRangeIndex, MsrtType, Sweep Mode Setting
            //---------------------------------------------------------------------------------------------------------------------------------------//
            this._pmuIndex[idx] = (uint)_ePmu[setItem.PUMIndex];
            (this._setTestItem as CmdSetTestItem).PMU = this._pmuIndex[idx];

            switch (setItem.MsrtType)
            {
                case EMsrtType.FIMV:
                case EMsrtType.FI:
                case EMsrtType.POLAR:
                    (this._setTestItem as CmdSetTestItem).TestItemNum = idx + 1; 
                    this._msrtType[idx] = (int)ERegMsrtType.FIMV;
                    this._forceRangeIndex[idx] = (int)_eCurrRange[setItem.PUMIndex][setItem.ForceRangeIndex];
                    this._msrtRangeIndex[idx] = (int)_eVoltRange[setItem.PUMIndex][setItem.MsrtRangeIndex];

                    (this._setTestItem as CmdSetTestItem).ForceRangeIndex = this._forceRangeIndex[idx];     
                    (this._setTestItem as CmdSetTestItem).MsrtRangeIndex = this._msrtRangeIndex[idx];

                    (this._setTestItem as CmdSetTestItem).MsrtType = ERegMsrtType.FIMV;
                    (this._setTestItem as CmdSetTestItem).SweepMode = ERegSweepMode.None;
                    break;
				//-------------------------------------------------------------------------------
                case EMsrtType.FIMVSWEEP:
                    this._sweepItemsCount++;
                    (this._setTestItem as CmdSetTestItem).TestItemNum =  (this._sweepItemsCount << 8);
        
                    this._msrtType[idx] = (int)ERegMsrtType.FIMVSWEEP;
                    this._sweepCount[idx] = setItem.SweepContCount + setItem.SweepRiseCount + 1;
                    this._forceRangeIndex[idx] = (int)_eCurrRange[setItem.PUMIndex][setItem.ForceRangeIndex];
                    this._msrtRangeIndex[idx] = (int)_eVoltRange[setItem.PUMIndex][setItem.MsrtRangeIndex];

                    (this._setTestItem as CmdSetTestItem).ForceRangeIndex = this._forceRangeIndex[idx];
                    (this._setTestItem as CmdSetTestItem).MsrtRangeIndex = this._msrtRangeIndex[idx];

                    (this._setTestItem as CmdSetTestItem).MsrtType = ERegMsrtType.FIMVSWEEP;
                    (this._setTestItem as CmdSetTestItem).SweepStartValue = this.CalForceValue(setItem.SweepStart, idx);
                    (this._setTestItem as CmdSetTestItem).SweepStepValue = this.CalSweepStepValue(setItem.SweepStart, setItem.SweepStep, idx);
                    (this._setTestItem as CmdSetTestItem).SweepRiseCount = setItem.SweepRiseCount + 1;   // Sweep Start Value 第一道也算一個 Rise

                    if (setItem.SweepContCount == 0)
                    {
                        (this._setTestItem as CmdSetTestItem).SweepMode = ERegSweepMode.Sweep_2;
                        (this._setTestItem as CmdSetTestItem).SweepFlatCount = 0;
                    }
                    else
                    {
                        (this._setTestItem as CmdSetTestItem).SweepMode = ERegSweepMode.LinearIncrease_2;
                        (this._setTestItem as CmdSetTestItem).SweepFlatCount = setItem.SweepContCount;
                    }
                    break;
				//-------------------------------------------------------------------------------
                case EMsrtType.FVMI:
                    (this._setTestItem as CmdSetTestItem).TestItemNum = idx + 1;
                    this._msrtType[idx] = (int)ERegMsrtType.FVMI;
                    this._forceRangeIndex[idx] = (int)_eVoltRange[setItem.PUMIndex][setItem.ForceRangeIndex];
                    this._msrtRangeIndex[idx] = (int)_eCurrRange[setItem.PUMIndex][setItem.MsrtRangeIndex];

                    (this._setTestItem as CmdSetTestItem).ForceRangeIndex = this._forceRangeIndex[idx];     
                    (this._setTestItem as CmdSetTestItem).MsrtRangeIndex = this._msrtRangeIndex[idx];

                    (this._setTestItem as CmdSetTestItem).MsrtType = ERegMsrtType.FVMI;
                    (this._setTestItem as CmdSetTestItem).SweepMode = ERegSweepMode.None;
                    break;
				//-------------------------------------------------------------------------------
                case EMsrtType.FVMISWEEP:
                     this._sweepItemsCount++;
                     (this._setTestItem as CmdSetTestItem).TestItemNum = (this._sweepItemsCount << 8); // (idx + 1) |
                    
                     this._msrtType[idx] = (int)ERegMsrtType.FVMISWEEP;                    
                     this._forceRangeIndex[idx] = (int)_eVoltRange[setItem.PUMIndex][setItem.ForceRangeIndex];               
                     this._msrtRangeIndex[idx] = (int)_eCurrRange[setItem.PUMIndex][setItem.MsrtRangeIndex];

                    (this._setTestItem as CmdSetTestItem).ForceRangeIndex = this._forceRangeIndex[idx];     
                    (this._setTestItem as CmdSetTestItem).MsrtRangeIndex = this._msrtRangeIndex[idx];

                     (this._setTestItem as CmdSetTestItem).MsrtType = ERegMsrtType.FVMISWEEP;                
                     (this._setTestItem as CmdSetTestItem).SweepStartValue = this.CalForceValue(setItem.SweepStart, idx);
                     (this._setTestItem as CmdSetTestItem).SweepStepValue = this.CalSweepStepValue(setItem.SweepStart, setItem.SweepStep, idx);
                     (this._setTestItem as CmdSetTestItem).SweepRiseCount = setItem.SweepRiseCount + 1;

                    if (setItem.SweepContCount == 0)
                    {
                        (this._setTestItem as CmdSetTestItem).SweepMode = ERegSweepMode.Sweep_1;
                        (this._setTestItem as CmdSetTestItem).SweepFlatCount = 0;
                    }
                    else
                    {
                        (this._setTestItem as CmdSetTestItem).SweepMode = ERegSweepMode.LinearIncrease_1;
                        (this._setTestItem as CmdSetTestItem).SweepFlatCount = setItem.SweepContCount;
                    }

                    break;
				//-------------------------------------------------------------------------------
                case EMsrtType.THY:  
                    this._sweepItemsCount++;
                    (this._setTestItem as CmdSetTestItem).TestItemNum = (this._sweepItemsCount << 8);
   
                    this._msrtType[idx] = (int)ERegMsrtType.THY;
                    this._forceRangeIndex[idx] = (int)_eCurrRange[setItem.PUMIndex][setItem.ForceRangeIndex];
                    this._msrtRangeIndex[idx] = (int)_eVoltRange[setItem.PUMIndex][setItem.MsrtRangeIndex];

                    (this._setTestItem as CmdSetTestItem).ForceRangeIndex = this._forceRangeIndex[idx];     
                    (this._setTestItem as CmdSetTestItem).MsrtRangeIndex = this._msrtRangeIndex[idx];

                    (this._setTestItem as CmdSetTestItem).MsrtType = ERegMsrtType.THY;

                    if (this._isdevPeakFiltering)
                    {
                        (this._setTestItem as CmdSetTestItem).SweepMode = ERegSweepMode.THY_2;  // FW calculate peak value and stable value
                        this._sweepCount[idx] = 2;  // for read data, peak value and stable value
                    }
                    else
                    {
                        (this._setTestItem as CmdSetTestItem).SweepMode = ERegSweepMode.THY_1;  // return flat count points, and SW calculate
                        this._sweepCount[idx] = setItem.SweepContCount;
                    }
                    
                    (this._setTestItem as CmdSetTestItem).SweepStartValue = 0.0d;
                    (this._setTestItem as CmdSetTestItem).SweepStepValue = 0.0d;
                    (this._setTestItem as CmdSetTestItem).SweepRiseCount = 0;
                    (this._setTestItem as CmdSetTestItem).SweepFlatCount = setItem.SweepContCount;
                    break;
				//-------------------------------------------------------------------------------
				default :
					return false;
            }

            //---------------------------------------------------------------------------------------------------------------------------------------//
            // Force Value, Clamp, Polarity
            //---------------------------------------------------------------------------------------------------------------------------------------//
            switch (this._devCalMode)
            {
                case (int)ECalApply.None:
                case (int)ECalApply.byFW:
                    (this._setTestItem as CmdSetTestItem).ForceValue = Math.Abs(setItem.ForceValue);
                    break;
                case (int)ECalApply.bySW:

                    if (setItem.MsrtType == EMsrtType.FIMVSWEEP || setItem.MsrtType == EMsrtType.FVMISWEEP)
                    {
                        (this._setTestItem as CmdSetTestItem).ForceValue = Math.Abs(setItem.ForceValue);
                    }
                    else
                    {
                        (this._setTestItem as CmdSetTestItem).ForceValue = this.CalForceValue(setItem.ForceValue, idx);
                    }
                    break;
                default:
                    return false;
            }

            if (setItem.ForceValue >= 0)
            {
                this._msrtPolarity[idx] = (int)EPolarity.Positive;    // for Measure Value Calibration
                (this._setTestItem as CmdSetTestItem).ItemPolarity = EPolarity.Positive;
                (this._setTestItem as CmdSetTestItem).ClampPolarity = EPolarity.Positive;
            }
            else
            {
                this._msrtPolarity[idx] = (int)EPolarity.Negitive;
                (this._setTestItem as CmdSetTestItem).ItemPolarity = EPolarity.Negitive;
                (this._setTestItem as CmdSetTestItem).ClampPolarity = EPolarity.Negitive;
            }

            (this._setTestItem as CmdSetTestItem).ClampValue = this.CalClampValue(setItem.MsrtProtection, setItem.MsrtRange, idx);

            //---------------------------------------------------------------------------------------------------------------------------------------//
            // NPLC, MsrtCount, Source Speed
            //---------------------------------------------------------------------------------------------------------------------------------------//
            // NPLC
            (this._setTestItem as CmdSetTestItem).FilterCount = (int)ENplcCount._60Hz_00 | (int)setItem.MsrtNPLC;

            // MsrtCount
            //(this._setTestItem as CmdSetTestItem).MsrtCount = setItem.MsrtFilterCount;
            //this._msrtCount[idx] = setItem.MsrtFilterCount;

            (this._setTestItem as CmdSetTestItem).MsrtCount = 2;
            this._msrtCount[idx] = 2;

            // Source Speed
            // FIMV Low Current (5uA, 10uA, 100uA, 1mA) cannot apply High Speed.
            if (setItem.MsrtType == EMsrtType.FIMV || setItem.MsrtType == EMsrtType.FI || setItem.MsrtType == EMsrtType.POLAR)
            {
                if (this._forceRangeIndex[idx] <= (int)EIRange._1mA)
                {
                    (this._setTestItem as CmdSetTestItem).SourceSpeed = ESourceSpeed.Normal;
                }
                else
                {
                    (this._setTestItem as CmdSetTestItem).SourceSpeed = ESourceSpeed.High;
                }
            }
            else if (setItem.MsrtType == EMsrtType.THY || setItem.MsrtType == EMsrtType.FIMVSWEEP || setItem.MsrtType == EMsrtType.FVMISWEEP)
            {
                (this._setTestItem as CmdSetTestItem).MsrtCount = 2;
                (this._setTestItem as CmdSetTestItem).SourceSpeed = ESourceSpeed.High;
            }
            else
            {
                (this._setTestItem as CmdSetTestItem).SourceSpeed = ESourceSpeed.High;
            }


            if (setItem.MsrtNPLC != 0)
            {
                (this._setTestItem as CmdSetTestItem).SourceSpeed = ESourceSpeed.Normal;  // Calibration only
            }

            //---------------------------------------------------------------------------------------------------------------------------------------//
            // Force Time, Delay Time, isAutoTurnOff, TurnOff Time, Sweep TurnOff Time 
            //---------------------------------------------------------------------------------------------------------------------------------------//       
            
            if (setItem.MsrtType == EMsrtType.THY)
            {
                (this._setTestItem as CmdSetTestItem).DelayTime = 0.0d;
                (this._setTestItem as CmdSetTestItem).ApplyTime = 0.0d;
                (this._setTestItem as CmdSetTestItem).IsAutoTurnOff = false;
                (this._setTestItem as CmdSetTestItem).TurnOffTime = 0.0d;
            }
            else if (setItem.MsrtType == EMsrtType.FIMVSWEEP || setItem.MsrtType == EMsrtType.FVMISWEEP)
            {
                (this._setTestItem as CmdSetTestItem).DelayTime = 0.0d;
                (this._setTestItem as CmdSetTestItem).ApplyTime = setItem.ForceTime;

                if (setItem.SweepTurnOffTime == 0.0d)
                {
                    (this._setTestItem as CmdSetTestItem).IsAutoTurnOff = false;
                    (this._setTestItem as CmdSetTestItem).SweepTurnOffTime = 0.0d;
                    (this._setTestItem as CmdSetTestItem).TurnOffTime = setItem.TurnOffTime;  // Duty Cycle
                }
                else
                {
                    (this._setTestItem as CmdSetTestItem).IsAutoTurnOff = true;
                    (this._setTestItem as CmdSetTestItem).SweepTurnOffTime = setItem.SweepTurnOffTime;
                    (this._setTestItem as CmdSetTestItem).TurnOffTime = setItem.TurnOffTime;  // Duty Cycle
                }              
            }
            else
            {
                // DC Test Item
                (this._setTestItem as CmdSetTestItem).DelayTime = 0.0d;
                (this._setTestItem as CmdSetTestItem).ApplyTime = setItem.ForceTime;
                (this._setTestItem as CmdSetTestItem).IsAutoTurnOff = setItem.IsAutoTurnOff;
                (this._setTestItem as CmdSetTestItem).TurnOffTime = setItem.TurnOffTime;      // Duty Cycle
                (this._setTestItem as CmdSetTestItem).SweepTurnOffTime = 0.0d;        
            }

            //---------------------------------------------------------------------------------------------------------------------------------------//
            // Device Setting
            //---------------------------------------------------------------------------------------------------------------------------------------//                            
            (this._setTestItem as CmdSetTestItem).WLTriggerEn = false;
            (this._setTestItem as CmdSetTestItem).SweepTableEn = false;

            if (this.SendCmd(this._setTestItem) != 0)
            {
                return false;
            }
            return true;
        }

        public bool RunTestItem(uint itemIndex)
        {
            if (this._devError != 0)   // Error Handling Sequence. 
            {
                return false;
            }

            if (this._isdevSpRunTestItem)
            {
                (this._spRunTestItem as CmdSpRunTestItem).TestItemIndex = (int)itemIndex;

                this._devError = this.SendCmd(this._spRunTestItem);

                if (this._devError != 0)   
                {
                    return false;
                }
            }
            else
            {
                (this._runTestItem as CmdRunTestItem).TestItemIndex = (int)itemIndex;

                this._devError = this.SendCmd(this._runTestItem);

                if (this._devError != 0)  
                {
                    return false;
                }
            }          
            return true;         
        }

        //public double[] GetDCResult(uint itemIndex)
        //{
        //    int errCode = 0;
        //    uint msrtType = 0;
        //    uint dataCount = 0;
        //    uint msrtInfo = 0;
        //    uint forceInfo = 0;
        //    uint bitData = 0;
        //    int addrIndex = 0;

        //    addrIndex = (int)(itemIndex * DC_RESULT_LENGTH);

        //    errCode = this.ReadDataCycle((int)EAddr.RESULT_HEAD + addrIndex, out msrtType);
        //    errCode = this.ReadDataCycle((int)EAddr.RESULT_HEAD + addrIndex + 1, out dataCount);
        //    errCode = this.ReadDataCycle((int)EAddr.RESULT_HEAD + addrIndex + 2, out msrtInfo);
        //    errCode = this.ReadDataCycle((int)EAddr.RESULT_HEAD + addrIndex + 3, out forceInfo);

        //    int pmu = (int)((forceInfo >> 8) & 0xFF);
        //    int msrtPolarity = (int)((msrtInfo >> 8) & 0xFF);
        //    int msrtRangeIndex = (int)(msrtInfo & 0xFF);
        //    int forceRangeIndex = (int)(forceInfo & 0xFF);

        //    if (msrtType == (uint)ETSEMsrtType.FIMV)   // _resultData[] : [0] AvgData, [1] RowData
        //    {
        //        errCode = this.ReadDataCycle(((int)EAddr.RESULT_DATA + addrIndex + (int)dataCount - 1), out bitData); // AVG Data

        //        this._resultData[0] = TSEConvert.UInt16ToDouble(bitData, (EVolRangeIndex)msrtRangeIndex);

        //        if (dataCount != 0)
        //        {
        //            for (int i = 1; i < (dataCount - 1); i++) // Row Data
        //            {
        //                errCode = this.ReadDataCycle(((int)EAddr.RESULT_DATA + addrIndex + i - 0), out bitData);
        //                this._resultData[i] = TSEConvert.UInt16ToDouble((uint)bitData, (EVolRangeIndex)msrtRangeIndex);
        //            }
        //        }
        //    }
        //    else if (msrtType == (uint)ETSEMsrtType.FVMI)
        //    {
        //        errCode = this.ReadDataCycle(((int)EAddr.RESULT_DATA + addrIndex + (int)dataCount - 1), out bitData);

        //        this._resultData[0] = TSEConvert.UInt16ToDouble((uint)bitData, (ECurrentRangeIndex)msrtRangeIndex);

        //        if (dataCount != 0)
        //        {
        //            for (int i = 1; i < (dataCount - 1); i++)
        //            {
        //                errCode = this.ReadDataCycle(((int)EAddr.RESULT_DATA + addrIndex + i - 0), out bitData);
        //                this._resultData[i] = TSEConvert.UInt16ToDouble((uint)bitData, (ECurrentRangeIndex)msrtRangeIndex);
        //            }
        //        }
        //    }

        //    if (dataCount != 0)
        //    {
        //        if (this._devSetting.IsFWCalibration == false)
        //        {
        //            for (uint j = 0; j < (dataCount - 1); j++) // Auto Calibration by S/W
        //            {
        //                this._resultData[j] = this.MsrtValueCal(this._resultData[j], pmu, (int)msrtType, forceRangeIndex, msrtRangeIndex);
        //            }

        //        }

        //        // Polarity
        //        if (msrtPolarity == (int)EPolarity.Negitive)
        //        {
        //            for (uint k = 0; k < (dataCount - 1); k++)
        //            {
        //                this._resultData[k] = this._resultData[k] * (-1);
        //            }
        //        }
        //    }
        //    return this._resultData;
        //}

        public double[] GetDCResult(uint itemIndex)
        {                  
            uint bitData = 0;
            int addrIndex = 0;

            this._devError = 0;

            addrIndex = (int)(itemIndex * DC_RESULT_LENGTH);

            if (this._msrtType[itemIndex] == (uint)ERegMsrtType.FIMV)   // _resultData[] : [0] AvgData, [1] RowData
            {
                this._devError = this.ReadRegisterData(((int)EAddr.DC_RESULT_DATA + addrIndex + (int)this._msrtCount[itemIndex] - 1), out bitData); // AVG Data

                if (this._devError != 0)
                {
                    return this._resultData;
                }

                this._resultData[0] = TSEConvert.UInt16ToDouble(bitData, (EVRange)this._msrtRangeIndex[itemIndex]);

                if (this._isGetDCMsrtRawData)  // Get DC Item Measure Raw Data
                {
                    if (this._msrtCount[itemIndex] > 0)
                    {
                        for (int i = 1; i < (this._msrtCount[itemIndex] - 1); i++)
                        {
                            this._devError = this.ReadRegisterData(((int)EAddr.DC_RESULT_DATA + addrIndex + i - 0), out bitData);
                            this._resultData[i] = TSEConvert.UInt16ToDouble((uint)bitData, (EVRange)this._msrtRangeIndex[itemIndex]);
                        }
                    }
                }           
            }
            else if (this._msrtType[itemIndex] == (uint)ERegMsrtType.FVMI)
            {
                this._devError = this.ReadRegisterData(((int)EAddr.DC_RESULT_DATA + addrIndex + (int)this._msrtCount[itemIndex] - 1), out bitData);

                if (this._devError != 0)
                {
                    return this._resultData;
                }

                this._resultData[0] = TSEConvert.UInt16ToDouble((uint)bitData, (EIRange)this._msrtRangeIndex[itemIndex]);

                if (this._isGetDCMsrtRawData)  // Get DC Item Measure Raw Data
                {
                    if (this._msrtCount[itemIndex] > 0)
                    {
                        for (int i = 1; i < (this._msrtCount[itemIndex] - 1); i++)
                        {
                            this._devError = this.ReadRegisterData(((int)EAddr.DC_RESULT_DATA + addrIndex + i - 0), out bitData);
                            this._resultData[i] = TSEConvert.UInt16ToDouble((uint)bitData, (EIRange)this._msrtRangeIndex[itemIndex]);
                        }
                    }
                }             
            }

            if (this._msrtCount[itemIndex] != 0)
            {
                // Polarity
                if (this._msrtPolarity[itemIndex] == (int)EPolarity.Negitive)
                {
                    for (uint k = 0; k < (this._msrtCount[itemIndex] - 1); k++)
                    {
                        this._resultData[k] = this._resultData[k] * (-1);
                    }

                    if ((this._msrtCount[itemIndex] - 1) == 0)   // For NPLC = 1;
                    {
                        this._resultData[0] = this._resultData[0] * (-1);
                    }
                }
 
                // Auto Calibration by S/W
                if (this._devCalMode == (int)ECalApply.bySW)
                {
                    if (bitData != 0xFFFF)
                    {
                        for (uint j = 0; j < (this._msrtCount[itemIndex] - 1); j++)
                        {
                            this._resultData[j] = this.CalMsrtValue(this._resultData[j], (int)itemIndex);
                        }

                        if ((this._msrtCount[itemIndex] - 1) == 0)   // For NPLC = 1;
                        {
                            this._resultData[0] = this.CalMsrtValue(this._resultData[0], (int)itemIndex);
                        }
                    }
                }
            }
            return this._resultData;
        }

        public double[] GetSweepResult(uint itemIndex)
        {
            double tempData = 0.0d;
            double[] tempSweepResult;
            int errCode = 0;
            uint bitData = 0;

            tempSweepResult = new double[this._sweepCount[itemIndex]];

            for (int i = 0; i < this._sweepCount[itemIndex]; i++)
            {
                if (this._msrtType[itemIndex] == (uint)ERegMsrtType.THY || this._msrtType[itemIndex] == (uint)ERegMsrtType.FIMVSWEEP)
                {                                          
                    errCode = this.ReadRegisterData((int)EAddr.SWEEP_RESULT_DATA + i, out bitData);     
                    tempData = TSEConvert.UInt16ToDouble((uint)bitData, (EVRange)this._msrtRangeIndex[itemIndex]);                   
                }
                else if (this._msrtType[itemIndex] == (uint)ERegMsrtType.FVMISWEEP)
                {                                       
                    errCode = this.ReadRegisterData((int)EAddr.SWEEP_RESULT_DATA + i, out bitData);   
                    tempData = TSEConvert.UInt16ToDouble((uint)bitData, (EIRange)this._msrtRangeIndex[itemIndex]);          
                }

                if (errCode != 0)
                {
                    return this._sweepResultData;
                }
                
                // Polarity
                if (this._msrtPolarity[itemIndex] == (int)EPolarity.Negitive)
                {
                    tempData = tempData * (-1);
                }

                // Auto Calibration by S/W
                if (this._devCalMode == (int)ECalApply.bySW)
                {
                    tempSweepResult[i] = this.CalMsrtValue(tempData, (int)itemIndex);
                }           
            }
            return tempSweepResult;
        }

        public bool TurnOff()
        {
            if (this.SendCmd(this._stopTestItem) != 0)
            {
                return false;
            }     
            return true;
        }

        #region >>> Interface Control <<<

        public bool DioOuput(uint outputData)
        {
            (this._dioOutput as CmdRunDioOutput).PinIndex = (int)outputData;
       
            if (this.SendCmd(this._dioOutput) != 0)
            {
                return false;
            }
            return true;
        }

        public uint DioInput(uint pinIndex)
        {
            uint tempDioInputData = 0;
            // Run Dio Input Cmd
            (this._dioInput as CmdRunDioInput).PinIndex = 0x1111;
            this.SendCmd(this._dioInput);
       
            // Read DIO Input Data from SRAM
            this.ReadRegisterData((int)EAddr.DIO_INPUT, out tempDioInputData);
            return (~tempDioInputData & 0xFFFF);
        }

        public bool SetTrigOutDuration(ETrigSetting pin, double time)
        {
            RegCmdBase SetTriggerOutDuration = new CmdSetTriggerOutDuration();
            (SetTriggerOutDuration as CmdSetTriggerOutDuration).TriggerOutIndex = pin;
            (SetTriggerOutDuration as CmdSetTriggerOutDuration).PeriodTime = time;

            if (this.SendCmd(SetTriggerOutDuration) != 0)
            {
                return false;
            }
            return true;
        }

        public bool TriggerOut(uint pinIndex)
        {
            (this._triggerOut as CmdRunTriggerOut).PinIndex = (int)pinIndex;

            if (this.SendCmd(this._triggerOut) != 0)
            {
                return false;
            }
            return true;
        }

        public bool TriggerIn(uint pinIndex, ETriggerInLatchType LatchType)
        {
            uint tempTriggerInData = 0;
                      
            if (pinIndex == 1) // Pin01 Trigger doesn't need to set the Latch Type
            {
                (this._triggerIn1 as CmdRunTriggerIn1).PinIndex = (int)pinIndex;
                this.SendCmd(this._triggerIn1);
                System.Threading.Thread.Sleep(5);
                this.ReadRegisterData((int)EAddr.TRIGGER_IN_DATA_1, out tempTriggerInData);
            }
            else
            {
                (this._triggerIn as CmdRunTriggerIn).PinIndex = (int)pinIndex;
                (this._triggerIn as CmdRunTriggerIn).LatchType = LatchType;
                this.SendCmd(this._triggerIn);
                this.ReadRegisterData((int)EAddr.TRIGGER_IN_DATA, out tempTriggerInData);
            }
            
            // if there has Trigger input data in SRAM, 
            if ((uint)ETriggerOutIndex.Pin01 == pinIndex)
            {
                if (tempTriggerInData == 0x0001)
                    return true;
            }
            else if ((uint)ETriggerOutIndex.Pin02 == pinIndex)
            {
                if (tempTriggerInData == 0x0002)
                    return true;
            }
            else if ((uint)ETriggerOutIndex.Pin03 == pinIndex)
            {
                if (tempTriggerInData == 0x0004)
                    return true;
            }
            else if ((uint)ETriggerOutIndex.Pin04 == pinIndex)
            {
                if (tempTriggerInData == 0x0008)
                    return true;
            }
            return false;
        }
      
        #endregion

        //public void DeviceConfig(ElecDevSetting setting)
        //{
        //    this._devSetting = setting;
        //}

        //public bool DeviceAutoCalEnable(int autoCalMode)
        //{
        //    this._regBase = new CmdRunAutoCalEnable();
        //    switch (autoCalMode)
        //    {
        //        case (int)ECalApply.None:   
        //            (this._regBase as CmdRunAutoCalEnable).AutoCalDataRead = ERegAutoCalApply.Disable;        
        //            break;
        //        case (int)ECalApply.bySW:
        //            //----------------------------------------------------------------//
        //            // (1) a. Copy the GainOffset Data of EEPROM to SRAM
        //            //     b. Get GainOffset Data from SRAM
        //            //----------------------------------------------------------------//
        //            if (!this.GetGainOffsetFromEPPROM())
        //            {
        //                return false;
        //            }

        //            //----------------------------------------------------------------//
        //            // (2) Active Software Auto Calibration
        //            //----------------------------------------------------------------//  
        //            (this._regBase as CmdRunAutoCalEnable).AutoCalDataRead = ERegAutoCalApply.bySW;
        //            break;
        //        case (int)ECalApply.byFW:
        //            // FW Calibration       
        //            (this._regBase as CmdRunAutoCalEnable).AutoCalDataRead = ERegAutoCalApply.byFW;
        //            break;
        //        default:
        //            return false;           
        //    }

        //    if (this.SendCmd(this._regBase) != 0)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        public bool DeviceAutoCalEnable(ERegAutoCalApply autoCalMode)
        {
            this._regBase = new CmdRunAutoCalEnable();
            (this._regBase as CmdRunAutoCalEnable).AutoCalDataRead = autoCalMode;

            if (this.SendCmd(this._regBase) != 0)
            {
                return false;
            }

            if (ERegAutoCalApply.EEPROMcopy == autoCalMode)
                System.Threading.Thread.Sleep(100);
            
            return true;
        }

        public bool TurnOffRangeChange(ETurnOffRange turnOffRange)
        {
            this._regBase = new CmdRunTurnOffRangeChange();
            (this._regBase as CmdRunTurnOffRangeChange).TurnOffRange = (int)turnOffRange; 

            if (this.SendCmd(this._regBase) != 0)
            {
                return false;
            }
            return true;
        }

        public bool CalSourceReset()
        {
            this._regBase = new CmdRunSourceReset();
            if (this.SendCmd(this._regBase) != 0)
            {
                return false;
            }
            System.Threading.Thread.Sleep(100);
            return true;
        }

        public void Sleep(float delay)
        {
            PCIInterface_uSleep(delay * 1000.0f);
        }

        #endregion

        #region >>> Enumeration <<<

        private enum ECalMsrtType : int
        {
            VS = 1,
            IS = 2,
            VM = 3,
            IM = 4,
        }

        private enum ECalApply : int
        {
            None = 0,
            bySW = 1,
            byFW = 2,
        }

        private enum ESrcMeterErrorCode : int
        {
            TESTER_NO_ERROR  = 0x00000000,

            POWER_ERROR      = 0x00000001,
            TEMP_ERROR       = 0x00000002,
            LOOP_ERROR       = 0x00000003,

            BUSY_TIMEOUT     = 0x00000020,
            TESTER_BUSY      = 0x00000021,
            TESTER_ERROR     = 0x00000022,
            TESTER_ALARM     = 0x00000023,
            TESTER_NOT_READY = 0x00000024,

            RESET_SIGNAL_FALLING_FAIL = 0x00000100,
            RESET_SIGNAL_RISING_FAIL  = 0x00000101,

            READ_DATA_FAIL                    = 0x00000102,
            READ_DATA_DATAREADY_UP_TIMEOUT    = 0x00000103,
            READ_DATA_DATAREADY_DOWN_TIMEOUT  = 0x00000104,
            WRITE_DATA_DATAREADY_UP_TIMEOUT   = 0x00000105,
            WRITE_DATA_DATAREADY_DOWN_TIMEOUT = 0x00000106,
        }
        #endregion
    }
}
