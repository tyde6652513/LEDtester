using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;
using System.IO.Ports;
using MPI.Tester.Device.SourceMeter.LDT3ALib;
using System.IO;
using MPI.Tester.Data;

namespace MPI.Tester.Device.SourceMeter
{
	public class LDT3A200 : ISourceMeter
	{
		private object _lockObj;

        public SourceMeterSpec Spec { get; set; }

        private bool _isApplyAdditionalForceTime;
        private const double ADDITIONAL_FORCE_TIME = 5.0d; // unit = ms

        private const double THRESHOLD_6V_TO_8V = 5.8d;

		private const uint MASTER = 0;
        

		#region >>> private Constan Property <<<

		private const uint PMU_COUNT = 3;
		private const uint MAX_ITEM_SETTING_LENGTH = 50;

		private const double MAX_APPLY_TIME = 0.1d;   // ms
		private const double MIN_APPLY_TIME = 5000.0d;

		private const int DEVICE_CONNECT_MAX_RETRY_COUNT = 10;

		private const double FIRMWARE_TIMER_RESOLUTION = 0.01; // unit: ms

		private static double[] FIRMWARE_NPLC = new double[] { 0.001d, 0.003d, 0.005d, 0.01d, 0.02d, 0.04d, 0.08d, 0.15d, 0.3d, 0.5d, 0.6d, 0.8d, 0.9d, 1.0d };


		#endregion

		// PMU = 0 (Normal), PMU = 1 (HV)   
		private static double[][] _voltRange = new double[][]	// [ PMU Index ][ Volt. Range Index ] , unit = V 
												{	
													new double[] { 6.0d,    15.0d }, 
													new double[] { 20.0d },
                                                    new double[] { 200.0d } 
												};

		private static double[][] _currRange = new double[][]  // [ PMU Index ][ Current Range Index ] , unit = A
												{	
													new double[] { 0.000001d,     0.00001d,     0.0001d,      0.001d,      0.01d,    0.1d,    0.2d,    1.0d,     2.0d,    3.0d },    
													new double[] { 0.000001d,     0.00001d,     0.0001d,      0.001d,      0.01d,    0.1d,    0.2d,    1.0d,     2.0d },
                                                    new double[] { 0.000001d,     0.00001d,     0.0001d,      0.001d,      0.01d,    0.1d,    0.2d } 
												};

		private static uint[] _ampOrder = new uint[4] { 1000, 10000, 100000, 1000000 };


		private ElectSettingData[] _elcSetting;
		private ElecDevSetting _devSetting;
		private EDevErrorNumber _errorNum;

		private List<double[][]> _applyData;  	//  [ Setting Item Index ] [ raw data or sweep data length ] 
		private List<double[][]> _acquireData;
		private List<double[][]> _timeChain;
		private List<double[][]> _sweepResult;

		private string _hwVersion;
		private string _swVersion;
		private string _serialNum;

		private Dictionary<uint, LDT3A_Lib> _deviceDic;  // uint : DeviceID

        private double[] _rtnValue = new double[10];

		private double _calcIntervelTime;

        private PerformanceTimer _pt = new PerformanceTimer();

		private List<StableTimeData> _cFile;

		public LDT3A200()
		{
			this._elcSetting = new ElectSettingData[MAX_ITEM_SETTING_LENGTH];

			this._hwVersion = "HW NONE";
			this._swVersion = "SW NONE";
			this._serialNum = "SN NONE";

			this._devSetting = new ElecDevSetting();

			this._acquireData = new List<double[][]>();

			this._timeChain = new List<double[][]>();

			this._sweepResult = new List<double[][]>();

			this._applyData = new List<double[][]>();

            this._isApplyAdditionalForceTime = false;

            this._cFile = new List<StableTimeData>();
		}

		public LDT3A200(ElecDevSetting setting) : this()
		{
			this._devSetting = setting;

			this._deviceDic = new Dictionary<uint, LDT3A_Lib>();
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
			get { return this._hwVersion; }
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

        public bool IsApplyAdditionalForceTime
        {
            get { return this._isApplyAdditionalForceTime; }
            set { this._isApplyAdditionalForceTime = value; }
        }

		#endregion

		#region >>> Private Method <<<

		private bool InitDevice(uint deviceID, string comPort)
		{
			string errMsg = string.Empty;

			int retryCnt = 0;

			do
			{
				this._deviceDic[deviceID].Connect(comPort, out errMsg);

				if (errMsg == string.Empty)
				{
					break;
				}

				System.Threading.Thread.Sleep(100);

				retryCnt++;
			}
			while (retryCnt < DEVICE_CONNECT_MAX_RETRY_COUNT);

			if (errMsg == string.Empty && this._deviceDic[deviceID].LDT3A200USB != null)
			{
				this._deviceDic[deviceID].ResetCom();

				//string[] verInfo = this._deviceDic[deviceID].Version();

                this._deviceDic[deviceID].GetDeviceInfo();

				 this._deviceDic[deviceID].NPLC_Order = 3;  // default = 3

				this._deviceDic[deviceID].Dump_Flash_Calibration();
				//this._srcWrapper.Parameter[20] = 150;
				//this._srcWrapper.Parameter[22] = 100;
				//this._srcWrapper.Parameter[24] = 70;
				//this._srcWrapper.Parameter[34] = 30;
				//this._srcWrapper.parameter(this._srcWrapper.Parameter);

                Console.WriteLine("[LDT3A Device], Initialize Success, Port: {0}, Connect Retry: {1}", comPort, retryCnt);

                this._serialNum = this._deviceDic[deviceID].DEV_SN;
                this._hwVersion = this._deviceDic[deviceID].HW_VER;
                this._swVersion = this._deviceDic[deviceID].DLL_VER;
                Console.WriteLine("[LDT3A Device], SN {0}, HW ver. {1}", this._serialNum, this._hwVersion);

                if (this._devSetting.SrcTriggerMode == ESMUTriggerMode.Multiple)
                {
                    if (this._serialNum != this._devSetting.Assignment[(int)deviceID].SerialNumber)
                    {
                        this._errorNum = EDevErrorNumber.SourceMeterDevice_HW_Err;

                        if (errMsg == string.Empty)
                        {
                            errMsg = "DeviceSerialNum not match";
                        }

                        Console.WriteLine("[LDT3A200 Device], Initialize Fail, Assignment SN:" + this._devSetting.Assignment[(int)deviceID].SerialNumber + ", Device SN:" + this._serialNum);

                        return false;
                    }
                }

                this._deviceDic[deviceID].TurnOn();

                this._deviceDic[deviceID].IsLog = this._devSetting.IsEnableDeviceLog;

				return true;
			}
			else
			{
				this._errorNum = EDevErrorNumber.SourceMeterDevice_HW_Err;

				if (errMsg == string.Empty)
				{
					errMsg = "Can not Find Device";
				}

				Console.WriteLine("[LDT3A200 Device], Initialize Fail,    Port: {0}, Error: {1}, Connect Retry: {2}", comPort, errMsg, retryCnt);

				return false;
			}
		}

		private bool FindPMUIndex(ElectSettingData setting)
		{
			bool[] forcePMUArray = new bool[PMU_COUNT];
			bool[] msrtPMUArray = new bool[PMU_COUNT];
			int[] forceIndexArray = new int[PMU_COUNT];
			int[] msrtIndexArray = new int[PMU_COUNT];

			setting.MsrtRange = setting.MsrtProtection;

			for (uint pmuIndex = 0; pmuIndex < PMU_COUNT; pmuIndex++)
			{
				switch (setting.MsrtType)
				{
					case EMsrtType.THY:
					case EMsrtType.FIMV:
					case EMsrtType.FI:
					case EMsrtType.POLAR:
					case EMsrtType.FIMVLOP:
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

		private void THYResultCalculate(uint deviceID, uint index)
		{
			// Calculate the stable value
			int startIndex = this._acquireData[(int)deviceID][index].Length - 1;
			int endIndex = startIndex - 20;
			double sumStable = 0.0d;

			//------------------------------------------------------------------------------------------------------------
			// (1) 除去負數
			//------------------------------------------------------------------------------------------------------------
			for (int i = 0; i < this._acquireData[(int)deviceID][index].Length; i++)
			{
				if (this._elcSetting[index].ForceValue > 0)
				{
					if (this._acquireData[(int)deviceID][index][i] < 0)
					{
						this._acquireData[(int)deviceID][index][i] = 0.0d;
					}
				}
				else
				{
					if (this._acquireData[(int)deviceID][index][i] > 0)
					{
						this._acquireData[(int)deviceID][index][i] = 0.0d;
					}
				}
			}

			//------------------------------------------------------------------------------------------------------------
			// (2) Moving Average
			//------------------------------------------------------------------------------------------------------------
			this._acquireData[(int)deviceID][index] = this.MovingAverage(this._acquireData[(int)deviceID][index], this._elcSetting[index].ThyMovingAverageWindow);

			//------------------------------------------------------------------------------------------------------------
			// (3) Calculate THY
			//------------------------------------------------------------------------------------------------------------
			this._sweepResult[(int)deviceID][index][(int)ETHYResultItem.MaxPeak] = 0.0d;
			this._sweepResult[(int)deviceID][index][(int)ETHYResultItem.MinPeak] = 0.0d;

			for (int i = 0; i < this._acquireData[(int)deviceID][index].Length; i++)
			{
				// Found the Max Peak Value
				if (Math.Abs(this._acquireData[(int)deviceID][index][i]) > this._sweepResult[(int)deviceID][index][(int)ETHYResultItem.MaxPeak])
				{
					this._sweepResult[(int)deviceID][index][(int)ETHYResultItem.MaxPeak] = Math.Abs(this._acquireData[(int)deviceID][index][i]);
				}

				// Found the Min Peak Value
				if (Math.Abs(this._acquireData[(int)deviceID][index][i]) < this._sweepResult[(int)deviceID][index][(int)ETHYResultItem.MinPeak])
				{
					this._sweepResult[(int)deviceID][index][(int)ETHYResultItem.MinPeak] = Math.Abs(this._acquireData[(int)deviceID][index][i]);
				}
			}

			int count = 0;

			// Found the Stable Value
			for (count = startIndex; count >= 0 && count > endIndex; count--)
			{
				sumStable += this._acquireData[(int)deviceID][index][count];
			}

			if (count != 0)
			{
				this._sweepResult[(int)deviceID][index][(int)ETHYResultItem.StableValue] = Math.Abs(sumStable) / 20.0d;
			}
			else if (count == 0 && this._acquireData[(int)deviceID][index].Length >= 1)
			{
				this._sweepResult[(int)deviceID][index][(int)ETHYResultItem.StableValue] = Math.Abs(sumStable) / ((double)this._acquireData[(int)deviceID][index].Length);
			}

			this._sweepResult[(int)deviceID][index][(int)ETHYResultItem.MaxToStable] = this._sweepResult[(int)deviceID][index][(int)ETHYResultItem.MaxPeak] - this._sweepResult[(int)deviceID][index][(int)ETHYResultItem.StableValue];

			if (this._sweepResult[(int)deviceID][index][(int)ETHYResultItem.MaxPeak] > this._sweepResult[(int)deviceID][index][(int)ETHYResultItem.StableValue])
			{
				this._sweepResult[(int)deviceID][index][(int)ETHYResultItem.OverShoot] = this._sweepResult[(int)deviceID][index][(int)ETHYResultItem.MaxPeak] - this._sweepResult[(int)deviceID][index][(int)ETHYResultItem.StableValue];
			}
			else
			{
				this._sweepResult[(int)deviceID][index][(int)ETHYResultItem.OverShoot] = 0.0d;
			}

			//------------------------------------------------------------------------------------------------------------
			// (3) 掃描點數超過 1200 點, Found the MTHYVDA & MTHYVDB
			//------------------------------------------------------------------------------------------------------------
			if (this._acquireData[(int)deviceID][index].Length >= 1200)
			{
				double section1 = 0;

				double section2 = 0;

				for (int i = 450; i < 470; i++)
				{
					section1 += this._acquireData[(int)deviceID][index][i];
				}

				section1 = Math.Abs(section1 / 20);

				for (int i = 980; i < 1000; i++)
				{
					section2 += this._acquireData[(int)deviceID][index][i];
				}

				section2 = Math.Abs(section2 / 20);

				double mthyda = Math.Abs(this._sweepResult[(int)deviceID][index][(int)ETHYResultItem.MaxPeak] - section2);

				double mthydb = Math.Abs(section1 - section2);

				if (mthyda > mthydb)
				{
					this._sweepResult[(int)deviceID][index][(int)ETHYResultItem.MTHYVDA] = mthyda;

					this._sweepResult[(int)deviceID][index][(int)ETHYResultItem.MTHYVDB] = mthydb;
				}
				else
				{
					this._sweepResult[(int)deviceID][index][(int)ETHYResultItem.MTHYVDA] = mthydb;

					this._sweepResult[(int)deviceID][index][(int)ETHYResultItem.MTHYVDB] = mthyda;
				}
			}
		}

		private double[] MovingAverage(double[] data, int windows)
		{
			if (data == null || data.Length < windows || windows <= 0)
			{
				return data;
			}

			double[] afterFilter = new double[data.Length];

			int lastindex = 0;

			for (int i = 0; i < data.Length; i++)
			{
				double sumWindows = 0;

				if (i <= (data.Length - windows))
				{
					lastindex = i;

					for (int j = 0; j <= windows - 1; j++)
					{
						sumWindows += data[i + j];
					}

					afterFilter[i] = sumWindows / windows;
				}
				else
				{
					afterFilter[i] = afterFilter[lastindex];
				}
			}

			return afterFilter;
		}

		private void SetRegTimeBase(uint deviceID, double time, out byte bTime)
		{
			byte timeBase = 10;
			double resolution = 0.1d;  // ms

            if (this._isApplyAdditionalForceTime)
            {
                time += ADDITIONAL_FORCE_TIME;
            }

			// F/W Time Base Setting
			for (int i = 10; i < (int)Byte.MaxValue; i++)
			{
				timeBase = (byte)i;

				resolution = Math.Round(timeBase * FIRMWARE_TIMER_RESOLUTION, 3, MidpointRounding.AwayFromZero);

				double timeMaxLimit = (int)Byte.MaxValue * resolution;

				if (time >= resolution && time <= timeMaxLimit)
				{
					break;
				}
			}

			bTime = (byte)(time / FIRMWARE_TIMER_RESOLUTION / timeBase - 1);

			this._deviceDic[deviceID].SetTimeBase(timeBase);
		}

		private void SetRegNPLC(uint deviceID, double nplc)
		{
			byte bNplc;

			for (bNplc = 0; bNplc < FIRMWARE_NPLC.Length; bNplc++)
			{
				if (nplc <= FIRMWARE_NPLC[bNplc])
				{
					break;
				}
			}

			 this._deviceDic[deviceID].NPLC_Order = bNplc;
		}

		private bool RunTestItem(uint deviceID, uint index)
		{
			ElectSettingData item = this._elcSetting[index];

            int polar = 1;

			double forceValue = 0.0f;
            double forceTime = 0.0d;
            double msrtClamp = 0.0f;

			if (item.ForceValue < 0)
			{
				polar = -1;
			}

            if (this._devSetting.IsEnableDeviceLog)
            {
                Console.WriteLine("[LDT3A] " + item.KeyName);
            }
                        
			// Start SrcMeter Test Sequence
			switch (item.MsrtType)
			{
				case EMsrtType.FI:
				case EMsrtType.FIMV:
					{
						forceValue = Math.Abs(item.ForceValue);  // I

                        forceTime = item.ForceTime + this.MinStableTime(forceValue, item.ForceTime);

						msrtClamp = Math.Abs(item.MsrtProtection) * polar; // V

                        if (Math.Abs(item.MsrtProtection) == 8.0d)
                        {
                            msrtClamp = 6.0d * polar; // V
                        }

						this.SetRegNPLC(deviceID, item.MsrtNPLC);

                        if (forceValue < 10e-6)
                        {
                            //-------------------------------------------------------------------------------------------------------
                            // IF 小於 10uA 的case 
                            //-------------------------------------------------------------------------------------------------------

                            //if (forceTime <= 3.0d)
                            //{
                            //    forceTime += 2.0d;  // 如果 IF 小於 10uA, Force Time 偷加 2ms
                            //}

                            // CASE 原先寫法C:\Work\_code\_San\T200_MultiDie_Ver1.2.8.13M_R17-PESD_20161105PM1900_測試版\MPI.RemoteControl.Tester\bin\
                             this._deviceDic[deviceID].Multi_Drive_New(msrtClamp, forceValue, "A", forceTime, item.IsAutoTurnOff);

                            //  this._deviceDic[deviceID].Multi_Drive_New(3, E_VRange._6V, forceValue, E_IRange._1uA, 1, false);

                          //    this._deviceDic[deviceID].Multi_Drive_New(msrtClamp, E_VRange._6V, 10e-4, E_IRange._1mA, forceTime, false);

                            // CASE 1, 新增Function,  1uA 電流用固定 1uA 檔位執行
                            //   this._deviceDic[deviceID].Multi_Drive_New(msrtClamp, E_VRange._6V, forceValue, E_IRange._1uA, forceTime, item.IsAutoTurnOff);

                            // CASE 2, 新增Function,  1uA 電流用固定 10uA 檔位執行
                            //this._deviceDic[deviceID].Multi_Drive_New(msrtClamp, E_VRange._6V, forceValue, E_IRange._10uA, forceTime, item.IsAutoTurnOff);

                            // CASE 3, 新增Function,  1uA 電流用固定 100uA 檔位執行
                           //  this._deviceDic[deviceID].Multi_Drive_New(msrtClamp, E_VRange._6V, forceValue, E_IRange._1mA, forceTime, item.IsAutoTurnOff);
                        }
                        else
                        {
                            this._deviceDic[deviceID].Multi_Drive_New(msrtClamp, forceValue, "A", forceTime, item.IsAutoTurnOff);
                        }

						break;
					}
				//------------------------------------------------------------------------------------------------------------------------------------------------ 
				case EMsrtType.FVMI:
					{
                        forceValue = Math.Abs(item.ForceValue) * polar;  // V

                        forceTime = item.ForceTime;

                        msrtClamp = Math.Abs((float)item.MsrtProtection); // I

						this.SetRegNPLC(deviceID, item.MsrtNPLC);

                        this._deviceDic[deviceID].Multi_Drive_New(forceValue, msrtClamp, "A", forceTime, item.IsAutoTurnOff);

						break;
					}
				//------------------------------------------------------------------------------------------------------------------------------------------------ 
				case EMsrtType.THY:
					{
						double interval = (item.SweepContCount * FIRMWARE_TIMER_RESOLUTION) * 2;  // 200 10us

						this._calcIntervelTime = interval;

                        forceValue = Math.Abs(item.SweepStart);  // I	

                        if (Math.Abs(item.MsrtProtection) == 8.0d)
                        {
                            msrtClamp = 6.0d * polar; // V
                        }

                        this._deviceDic[deviceID].Multi_Drive_New(msrtClamp, forceValue, "A", interval, item.IsAutoTurnOff);

						break;
					}
				//------------------------------------------------------------------------------------------------------------------------------------------------ 
				case EMsrtType.FIMVLOP:
					{
						// 只會有 Single-Die的使用場合

						forceValue = Math.Abs((float)item.ForceValue);

						msrtClamp = Math.Abs((float)item.MsrtProtection) * polar;

						//this._deviceDic[deviceID].SetGain(this.FindADGain((uint)item.MsrtRangeIndex02));

						this.SetRegNPLC(deviceID, item.MsrtNPLC);

						//this.SetRegTimeBase(deviceID, item.ForceTime, out applyTime);

						//this._regResult = this._deviceDic[deviceID].Single_Sequence(msrtClamp, forceValue, "A", applyTime, FIRMWARE_MSRT_NON_FLOATING, turnOffMode);

						////double.TryParse(this._regResult[(int)ERegDCResultHeader.VS_Diff_Value], out msrtResult);

						////double.TryParse(this._regResult[(int)ERegDCResultHeader.IF_Value], out applyValue);

						////this._acquireData[index][0] = msrtResult;

						////this._applyData[index][0] = Math.Abs(applyValue) * polar;

						if (this._elcSetting[index].IsTrigDetector)
						{
							ushort[] result = this._deviceDic[MASTER].ADMA();

							this._acquireData[(int)deviceID][index][0] = result[3];
						}

						break;
					}
				//------------------------------------------------------------------------------------------------------------------------------------------------ 
				default:
					return false;
			}

			return true;
		}

		private bool AcquireMsrtData(uint[] deviceIDs, uint index)
		{
			bool isBusy;

			int polar = 1;

			double applyValue = 0.0d;

			double msrtResult = 0.0d;

			if (this._elcSetting[index].ForceValue < 0)
			{
				polar = -1;
			}

            double clamp = Math.Abs(this._elcSetting[index].MsrtProtection);

			foreach (var id in deviceIDs)
			{
				isBusy = true;

				switch (this._elcSetting[index].MsrtType)
				{
					case EMsrtType.FI:
					case EMsrtType.FIMV:
						{
                            this._rtnValue = this._deviceDic[id].Multi_Drive_GetData();

                            msrtResult = this._rtnValue[(int)ERtnValue.VDD];

                            applyValue = this._rtnValue[(int)ERtnValue.IFN];

                            if (clamp == 8.0d)
                            {
                                if (Math.Abs(msrtResult) >= THRESHOLD_6V_TO_8V)
                                {
                                    msrtResult = clamp * polar;
                                }
                            }

                            msrtResult = this.ChannelOffset(this._elcSetting[index].ForceValue, msrtResult, id); 

                            this._acquireData[(int)id][index][0] = msrtResult;

                            this._applyData[(int)id][index][0] = applyValue;

							break;
						}
					//------------------------------------------------------------------------------------------------------------------------------------------------ 
					case EMsrtType.FVMI:
						{
                            this._rtnValue = this._deviceDic[id].Multi_Drive_GetData();

                            msrtResult = this._rtnValue[(int)ERtnValue.IFN];

                            applyValue = this._rtnValue[(int)ERtnValue.VDD];

                            this._acquireData[(int)id][index][0] = msrtResult;

							this._applyData[(int)id][index][0] = applyValue;

							break;
						}
					//------------------------------------------------------------------------------------------------------------------------------------------------ 
					case EMsrtType.THY:
						{
							if (!this._devSetting.IsDevicePeakFiltering)
							{
								//----------------------------------------------------------------------------------------------------------
                                // S/W Calc THY,  (F/W return Curve data)
                                //----------------------------------------------------------------------------------------------------------
                                double[] dVanodeArray = null;

                                double[] dVcathodeArray = null;


                                this._deviceDic[id].Multi_BeginReadCurve((uint)ERegADChannel.VSP, (this._elcSetting[index].SweepContCount + 1));

                                do
                                {
                                    dVanodeArray = this._deviceDic[id].Multi_GetCurveData();
                
                                    if (dVanodeArray != null)
                                    {
                                        isBusy = false;
                                    }
                                }
                                while (isBusy == true);

                                isBusy = true;

                                this._deviceDic[id].Multi_BeginReadCurve((uint)ERegADChannel.VSN, (this._elcSetting[index].SweepContCount + 1));

                                do
                                {
                                    dVcathodeArray = this._deviceDic[id].Multi_GetCurveData();
                                
                                    if (dVcathodeArray != null)
                                    {
                                        isBusy = false;
                                    }
                                }
                                while (isBusy == true);

                                for (int i = 0; i < this._elcSetting[index].SweepContCount; i++)
                                {
                                    this._acquireData[(int)id][index][i] = (dVanodeArray[i + 1] - dVcathodeArray[i + 1]);
                                }

                                for (int i = 0; i < this._elcSetting[index].SweepContCount; i++)
                                {
                                    this._acquireData[(int)id][index][i] = (dVanodeArray[i + 1] - dVcathodeArray[i + 1]);
                                }

								this.THYResultCalculate(id, index);
							}
							else
							{
								//-----------------------------------------------------------------
                                // F/W Calc THY, (F/W return Vpp & Vss)
                                //-----------------------------------------------------------------
								double maxPeak = 0.0d;

								double stable = 0.0d;

								double diff = 0.0d;

                                this._deviceDic[id].Multi_GetFwCalcThyResult();

                                maxPeak = this._deviceDic[id].RtnValue[(int)ERtnFwCalcThyValue.VDD_Peak];

                                stable = this._deviceDic[id].RtnValue[(int)ERtnFwCalcThyValue.VDD_Stable];

                                diff = this._deviceDic[id].RtnValue[(int)ERtnFwCalcThyValue.VDD_Diff];

                                if (clamp == 8.0d)
                                {
                                    if (maxPeak >= THRESHOLD_6V_TO_8V || stable >= THRESHOLD_6V_TO_8V)
                                    {
                                        maxPeak = clamp * polar;
                                        stable = clamp * polar;
                                    }
                                }

								this._sweepResult[(int)id][index][(int)ETHYResultItem.MaxPeak] = maxPeak;

								this._sweepResult[(int)id][index][(int)ETHYResultItem.StableValue] = stable;

								this._sweepResult[(int)id][index][(int)ETHYResultItem.MaxToStable] = this._sweepResult[(int)id][index][(int)ETHYResultItem.MaxPeak] - this._sweepResult[(int)id][index][(int)ETHYResultItem.StableValue];

								this._sweepResult[(int)id][index][(int)ETHYResultItem.OverShoot] = diff;

								if (this._elcSetting[index].SweepContCount >= 1200)
								{
									double section1 = 0;

									double section2 = 0;

									// Section1 450 ~ 470
                                    string[] thy02data01 = this._deviceDic[id].Multi_2Thy_Getdata(450);

									Double.TryParse(thy02data01[0], out section1);

									// Section2 980 ~ 1000
                                    string[] thy02data02 = this._deviceDic[id].Multi_2Thy_Getdata(980);

									Double.TryParse(thy02data02[0], out section2);

									double mthyda = Math.Abs(this._sweepResult[(int)id][index][(int)ETHYResultItem.MaxPeak] - section2);

									double mthydb = Math.Abs(section1 - section2);

									if (mthyda > mthydb)
									{
										this._sweepResult[(int)id][index][(int)ETHYResultItem.MTHYVDA] = mthyda;

										this._sweepResult[(int)id][index][(int)ETHYResultItem.MTHYVDB] = mthydb;
									}
									else
									{
										this._sweepResult[(int)id][index][(int)ETHYResultItem.MTHYVDA] = mthydb;

										this._sweepResult[(int)id][index][(int)ETHYResultItem.MTHYVDB] = mthyda;
									}
								}
							}
							break;
						}
					//------------------------------------------------------------------------------------------------------------------------------------------------ 
					case EMsrtType.FIMVLOP:
						{
                            //double.TryParse(this._regResult[(int)ERegDCResultHeader.VS_Diff_Value], out msrtResult);

                            //double.TryParse(this._regResult[(int)ERegDCResultHeader.IF_Value], out applyValue);

                            //this._acquireData[(int)id][index][0] = msrtResult;

                            //this._applyData[(int)id][index][0] = Math.Abs(applyValue) * polar;

                            //if (this._elcSetting[index].IsTrigSMUB)
                            //{
                            //    ushort[] result = this._deviceDic[MASTER].ADMA();

                            //    this._acquireData[(int)id][index][0] = result[3];
                            //}

							break;
						}
					//------------------------------------------------------------------------------------------------------------------------------------------------ 
					default:
						return false;
				}
			}

			return true;
		}

		private byte FindADGain(uint power)
		{
			byte order = (byte)(_ampOrder.Length - 1);

			double deltaValue = 0.0d;

			for (int i = 0; i < _ampOrder.Length; i++)
			{
				deltaValue = Math.Pow(10, power) - _ampOrder[i];

				if (deltaValue < 0.0d || Math.Abs(deltaValue) <= Double.Epsilon)
				{
					order = (byte)i;

					break;
				}

			}

			return order;
		}

        private void LoadStableTimeFromCFile()
        {
            this._cFile.Clear();

            string filePath = Path.Combine(Constants.Paths.DATA_FILE, "CFile.csv");

            List<string[]> file = CSVUtil.ReadCSV(filePath);

            if (file == null)
            {
                return;
            }

            foreach (var item in file)
            {
                // Min Range     Max Range   Set Force Time      Change to Force Time    CH Offset
                // 0.000001      0.00001     0.001               0.003                   1:0;2:1.6;3:0;4:0
                // 1uA ≦ X < 10uA

                if (item.Length < 4)
                {
                    continue;
                }

                double minRange = double.MinValue;

                double maxRange = double.MaxValue;

                bool isParseOK = true;

                isParseOK &= double.TryParse(item[0], out minRange);

                isParseOK &= double.TryParse(item[1], out maxRange);

                if (isParseOK)
                {
                    double setForceTime = 0.0d;

                    double changeToForceTime = 0.0d;

                    if (double.TryParse(item[2], out setForceTime) && double.TryParse(item[3], out changeToForceTime))
                    {
                        this._cFile.Add(new StableTimeData(minRange, maxRange, setForceTime, changeToForceTime));

                        Console.WriteLine("[LDT3A200], CFile:" + minRange + "," + maxRange + "," + setForceTime + "," + changeToForceTime);

                        string offsetStr = string.Empty;

                        if (item.Length >= 5)
                        {
                            string[] chsStr = item[4].Split(';');

                            foreach (var data in chsStr)
                            {
                                string[] chStr = data.Split(':');

                                int ch = -1;

                                double offset = 0.0d;

                                if (chStr.Length == 2 && int.TryParse(chStr[0], out ch) && double.TryParse(chStr[1], out offset))
                                {
                                    if (ch == 1)
                                    {
                                        this._cFile[this._cFile.Count - 1].CH1_Offset = offset;
                                    }

                                    if (ch == 2)
                                    {
                                        this._cFile[this._cFile.Count - 1].CH2_Offset = offset;
                                    }

                                    if (ch == 3)
                                    {
                                        this._cFile[this._cFile.Count - 1].CH3_Offset = offset;
                                    }

                                    if (ch == 4)
                                    {
                                        this._cFile[this._cFile.Count - 1].CH4_Offset = offset;
                                    }
                                }

                                Console.WriteLine("[LDT3A200], CFile:" + offset);
                            }
                        }
                    }
                }

            }
        }

        private double MinStableTime(double forceValue, double forceTime)
        {
            forceValue = Math.Abs(forceValue);

            foreach (var item in this._cFile)
            {
                if (forceValue >= item.Lower && forceValue < item.Upper)
                {
                    if (item.SetForceTime == forceTime)
                    {
                        return item.ChangeToForceTime;
                    }
                }
            }

            return 0.0d;
        }

        private double ChannelOffset(double forceValue, double msrtValue, uint ch)
        {
            ch = ch + 1; // 0 base to 1 base

            forceValue = Math.Abs(forceValue);

            foreach (var item in this._cFile)
            {
                if (forceValue >= item.Lower && forceValue < item.Upper)
                {
                    if (ch == 1)
                    {
                        return msrtValue + item.CH1_Offset;
                    }

                    if (ch == 2)
                    {
                        return msrtValue + item.CH2_Offset;
                    }

                    if (ch == 3)
                    {
                        return msrtValue + item.CH3_Offset;
                    }

                    if (ch == 4)
                    {
                        return msrtValue + item.CH4_Offset;
                    }
                }
            }

            return msrtValue;
        }

		#endregion

		#region >>> Public Method <<<

		public bool Init(int deviceNum, string sourceMeterSN)
		{
			bool rtn = true;

			string errMsg = string.Empty;

            if (this._devSetting.IsEnableDeviceLog)
            {
                Console.WriteLine("[LDT3A] Device Log Start...");
            }


			switch (this._devSetting.SrcTriggerMode)
			{
				case ESMUTriggerMode.Single:
					{
						this._deviceDic.Add(MASTER, new LDT3A_Lib());

						rtn = this.InitDevice(MASTER, sourceMeterSN);

						break;
					}
				case ESMUTriggerMode.Multiple:
					{
						for (uint devNum = 0; devNum < this._devSetting.Assignment.Count; devNum++)
						{
                            sourceMeterSN = this._devSetting.Assignment[(int)devNum].ConnectionPort;

							this._deviceDic.Add(devNum, new LDT3A_Lib());

							rtn &= this.InitDevice(devNum, sourceMeterSN);
						}

						break;
					}
				default:
					{
						return false;
					}
			}

			return rtn;
		}

		public void Reset()
		{

		}

		public void Close()
		{
			if (this._deviceDic != null)
			{
				foreach (var device in this._deviceDic)
				{
					device.Value.TurnOff();

					device.Value.SetStatus(0);
				}

				foreach (var device in this._deviceDic)
				{
					device.Value.Disconnect();
				}
			}
		}

		public bool SetConfigToMeter(ElecDevSetting devSetting)
		{
			return true;
		}

		public bool SetParamToMeter(ElectSettingData[] eleSetting)
		{
            this.LoadStableTimeFromCFile();

			this._elcSetting = eleSetting;

			this._errorNum = EDevErrorNumber.Device_NO_Error;

			double[] sweepPoints = null;

			if (eleSetting.Length == 0)
			{
				return true;
			}

			if (eleSetting.Length > MAX_ITEM_SETTING_LENGTH)
			{
				this._errorNum = EDevErrorNumber.ParameterLengthExcessBufferSize;
				return false;
			}

			////////////////////////////////////////////////////////////////////////////////////////
			// (1) Create Buffer
			////////////////////////////////////////////////////////////////////////////////////////
			this._applyData.Clear();

			this._acquireData.Clear();

			this._sweepResult.Clear();

			this._timeChain.Clear();

			for (uint deviceID = 0; deviceID < this._deviceDic.Count; deviceID++)
			{
				this._applyData.Add(new double[eleSetting.Length][]);

				this._acquireData.Add(new double[eleSetting.Length][]);

				this._sweepResult.Add(new double[eleSetting.Length][]);

				this._timeChain.Add(new double[eleSetting.Length][]);
			}

			////////////////////////////////////////////////////////////////////////////////////////
			// (2) Set Test Item to Meter
			////////////////////////////////////////////////////////////////////////////////////////

			for (uint deviceID = 0; deviceID < this._deviceDic.Count; deviceID++)
			{
				for (int itemIdx = 0; itemIdx < eleSetting.Length; itemIdx++)
				{
					ElectSettingData item = this._elcSetting[itemIdx].Clone() as ElectSettingData;

					//----------------------------------------------------------------------------------------------------
					// (1) Find the PMUIndex, Force Range Index, Msrt Range Index
					//----------------------------------------------------------------------------------------------------
					if (!this.FindPMUIndex(item))
					{
						this._acquireData = null;
						this._sweepResult = null;
						this._timeChain = null;
						this._errorNum = EDevErrorNumber.NoMatchRangeIndex;
						return false;
					}

					//----------------------------------------------------------------------------------------------------
					// (2) Set Test Item to Meter
					//----------------------------------------------------------------------------------------------------

					switch (item.MsrtType)
					{
						case EMsrtType.FIMVSWEEP:
						case EMsrtType.FVMISWEEP:
							{
								this._acquireData[(int)deviceID][itemIdx] = new double[item.SweepRiseCount + item.SweepContCount];
								this._applyData[(int)deviceID][itemIdx] = new double[1];

								this._elcSetting[itemIdx].IsAutoTurnOff = true;

								this._timeChain[(int)deviceID][itemIdx] = new double[item.SweepContCount + item.SweepRiseCount + 1];   // 第一道也算一個 Rise Cnt
								this._applyData[(int)deviceID][itemIdx] = new double[item.SweepContCount + item.SweepRiseCount + 1];

								sweepPoints = new double[item.SweepContCount + item.SweepRiseCount + 1];

								for (int j = 0; j < (item.SweepRiseCount + item.SweepContCount + 1); j++)
								{
									if (j == 0)
									{
										this._timeChain[(int)deviceID][itemIdx][j] = item.ForceTime;
									}
									else
									{
										this._timeChain[(int)deviceID][itemIdx][j] = item.ForceTime + j * (item.ForceTime + item.SweepTurnOffTime);
									}

									if (j < item.SweepRiseCount)
									{
										sweepPoints[j] = Math.Round((item.SweepStart + j * item.SweepStep), 6);
									}
									else
									{
										sweepPoints[j] = item.SweepStop;
									}

								}

								item.SweepCustomValue = sweepPoints;

								this._elcSetting[itemIdx].SweepCustomValue = sweepPoints;

								break;
							}
						case EMsrtType.THY:
							{
								this._acquireData[(int)deviceID][itemIdx] = new double[item.SweepRiseCount + item.SweepContCount];
								this._applyData[(int)deviceID][itemIdx] = new double[1];

								this._elcSetting[itemIdx].IsAutoTurnOff = true;

								this._sweepResult[(int)deviceID][itemIdx] = new double[Enum.GetNames(typeof(ETHYResultItem)).Length];

								this._timeChain[(int)deviceID][itemIdx] = new double[item.SweepContCount];

								sweepPoints = new double[item.SweepContCount];

								for (int j = 0; j < item.SweepContCount; j++)
								{
									this._timeChain[(int)deviceID][itemIdx][j] = Math.Round(FIRMWARE_TIMER_RESOLUTION * j, 6);

									sweepPoints[j] = item.ForceValue;
								}

								item.SweepCustomValue = sweepPoints;

								this._elcSetting[itemIdx].SweepCustomValue = sweepPoints;

								break;
							}
						default:
							{
								this._acquireData[(int)deviceID][itemIdx] = new double[1];
								this._applyData[(int)deviceID][itemIdx] = new double[1];

								break;
							}
					}

					this._applyData[(int)deviceID][itemIdx][0] = Math.Abs(item.ForceValue);
				}
			}

			return true;
		}

		public bool MeterOutput(uint[] activateChannels, uint itemIndex)
		{
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

            System.Threading.Thread.Sleep((int)this._elcSetting[itemIndex].ForceDelayTime);

			foreach (var deviceID in activateChannels)
			{
				if (!this.RunTestItem(deviceID, itemIndex))
				{
					this._errorNum = EDevErrorNumber.MeterOutput_Ctrl_Err;

					return false;
				}
			}

			this.AcquireMsrtData(activateChannels, itemIndex);

			return true;
		}

		public double[] GetDataFromMeter(uint channel, uint itemIndex)
		{
			if (this._elcSetting == null || itemIndex > this._elcSetting.Length - 1)
			{
				double[] data = new double[0];
				this._errorNum = EDevErrorNumber.SourceMeterIndexSetting_Err;
				return data;
			}

			return this._acquireData[(int)channel][itemIndex];
		}

		public double[] GetApplyDataFromMeter(uint channel, uint itemIndex)
		{
			if (this._elcSetting == null || itemIndex > this._elcSetting.Length)
			{
				double[] data = new double[0];
				this._errorNum = EDevErrorNumber.SourceMeterIndexSetting_Err;
				return data;
			}

			return this._applyData[(int)channel][itemIndex];
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

			return this._sweepResult[(int)channel][itemIndex];
		}

		public double[] GetTimeChainFromMeter(uint channel, uint settingIndex)
		{
			return this._timeChain[(int)channel][settingIndex];
		}

		public void TurnOff()
		{
			if (this._deviceDic != null)
			{
                if (this._devSetting.IsEnableDeviceLog)
                {
                    Console.WriteLine("[LDT3A] TurnOff");
                }

                foreach (var device in this._deviceDic)
				{

                    device.Value.Multi_Drive_New(0, E_VRange._6V, 0, E_IRange._10uA,0.1, true);
 
                    // device.Value.Multi_Drive_New(0, 0, "A", 0.1, true);
				}
			}
		}

		public void TurnOff(double delay, bool isOpenRelay)
		{
			this.TurnOff();

			System.Threading.Thread.Sleep((int)delay);
		}

		public void Output(uint point, bool active)
		{

		}

		public byte InputB(uint point)
		{
			if (this._deviceDic == null)
			{
				return 0x00;
			}

			byte[] bArray = this._deviceDic[MASTER].Read_DI();

			//bArray[2] = 1;

			return ((byte)(bArray[2] & 0x07));   // 3 pins
		}

		public byte Input(uint point)
		{
			throw new NotImplementedException();
		}

        public bool CheckInterLock()
        {
            return false;
        }

        public double GetPDDarkSample(int count)
        {
            return 0;
        }
        public bool MeterOutput(uint[] activateChannels, uint index, double applyValue)
        {
            return true;
        }

		#endregion

        private class StableTimeData
        {

            private StableTimeData()
            {
                this.Lower = 0;
                this.Upper = 0;
                this.SetForceTime = 0;
                this.ChangeToForceTime = 0;
                this.CH1_Offset = 0;
                this.CH1_Offset = 0;
                this.CH1_Offset = 0;
                this.CH1_Offset = 0;
            }

            public StableTimeData(double lower, double upper, double setForceValue, double changeToForceTime)
                : this()
            {
                this.Lower = lower;
                this.Upper = upper;
                this.SetForceTime = setForceValue;
                this.ChangeToForceTime = changeToForceTime;
            }

            public double Lower { get; set; }
            public double Upper { get; set; }
            public double SetForceTime { get; set; }
            public double ChangeToForceTime { get; set; }

            public double CH1_Offset { get; set; }
            public double CH2_Offset { get; set; }
            public double CH3_Offset { get; set; }
            public double CH4_Offset { get; set; }
        }
	}
}
