using System;
using System.Collections.Generic;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Device.SpectroMeter
{
    public class SMUSB : ISpectroMeter
    {
		public const int			CCD2048_PIXEL_LENGTH		= 2048;
        private const int			MIN_INTEGRATION_TIME		= 4;
        private const int			MAX_INTEGRATION_TIME		= 500;
        private const int			DARK_COUNT							= 5000;
		private const int			MAX_COUNT							= 60000;

        private const int			SERIAL_NUMBER_COUNT		= 11;

        private object _lockObj;

        private SMUSBWrapper    _SMUSBM;
        private string _version;
        private string _serialNum;
        private EDevErrorNumber _errorNum;
		
		private OptiDevSetting _devSetting;
		private OptiSettingData[] _paramSetting;
        private OptiData[] _opticalData;
		private double[] _recordIntTime;
		private double[][] _recordIntensityArray;

        private double[] _wavelengthArray;
		private double[] _currentIntensityArray;

        //private double _maxLimitTime;

		private int _triggerMaxCount;
		private double _triggerSpanTime;
		
        private double _attenuatorPercent;

        private PerformanceTimer _pt;

        /// <summary>
        /// Constructor
        /// </summary>
        public SMUSB()
        {
            this._lockObj = new object();

            this._SMUSBM = new SMUSBWrapper();
            this._devSetting = new OptiDevSetting();
			this._paramSetting = new OptiSettingData[1] { new OptiSettingData() };
			this._opticalData = new OptiData[1] { new OptiData() };

            this._version = "NONE";
            this._serialNum = "NONE";
            this._errorNum = EDevErrorNumber.Device_NO_Error;

			this._wavelengthArray = new double[CCD2048_PIXEL_LENGTH];
			this._recordIntensityArray = new double[1][] { new double[CCD2048_PIXEL_LENGTH] };
			this._currentIntensityArray = new double[CCD2048_PIXEL_LENGTH];

			this._triggerMaxCount = DARK_COUNT;
            //this._maxLimitTime = MAX_INTEGRATION_TIME;

            this._attenuatorPercent = 100.0d;
			this._recordIntTime = new double[1] { 5.0d };
            this._pt = new PerformanceTimer();
        } 


        #region >>> Public Property <<<

        /// <summary>
        /// Spectrometer software version 
        /// </summary>
        public string Version
        {
            get { return this._version; }
        }

        /// <summary>
        /// Spectrometer serial number
        /// </summary>
        public string SerialNumber
        {
            get { return this._serialNum; }
        }

        /// <summary>
        ///  Error number of spectrometer
        /// </summary>
        public EDevErrorNumber ErrorNumber
        { 
            get{ return this._errorNum ; }
        }

        /// <summary>
        /// Photometry and colormetry data of spectrometer
        /// </summary>
        public OptiData[] Data
        {
            get { return this._opticalData; }
        }

        public double[] DarkIntensityArray
        {
            get { return null; }
        }
        #endregion

        #region >>> Private Method <<<

        private void ResetData(uint index, uint status)
        {
			if (index < 0 || index >= this._opticalData.Length)
				return;

			if (status == 0)
			{
				this._opticalData[index].CIE1931X = 0.0d;
				this._opticalData[index].CIE1931Y = 0.0d;
				this._opticalData[index].CIE1931Z = 0.0d;
				this._opticalData[index].CIE1931x = 0.0d;
				this._opticalData[index].CIE1931y = 0.0d;
				this._opticalData[index].CIE1931z = 1.0d;
				this._opticalData[index].Purity = 0.0d;
				this._opticalData[index].CCT = 0.0d;

				this._opticalData[index].WLP = 0.0d;
				this._opticalData[index].WLP2 = 0.0d;
				this._opticalData[index].WLPNIR = 0.0d;
				this._opticalData[index].WLD = 0.0d;
				this._opticalData[index].WLCv = 0.0d;
				this._opticalData[index].WLCp = 0.0d;
				this._opticalData[index].FWHM = 0.0d;

				this._opticalData[index].Watt = 0.0d;
				this._opticalData[index].Lm = 0.0d;
				this._opticalData[index].Lx = 0.0d;

				this._opticalData[index].GeneralCRI = 0.0d;
			}
			else if (status == 1)
			{
				this._opticalData[index].CIE1931X = 9999.999d;
				this._opticalData[index].CIE1931Y = 9999.999d;
				this._opticalData[index].CIE1931Z = 9999.999d;
				this._opticalData[index].CIE1931x = 9999.999d;
				this._opticalData[index].CIE1931y = 9999.999d;
				this._opticalData[index].CIE1931z = 9999.999d;
				this._opticalData[index].Purity = 9999.999d;
				this._opticalData[index].CCT = 9999.999d;

				this._opticalData[index].WLP = 9999.999d;
				this._opticalData[index].WLP2 = 9999.999d;
				this._opticalData[index].WLPNIR = 9999.999d;
				this._opticalData[index].WLD = 9999.999d;
				this._opticalData[index].WLCv = 9999.999d;
				this._opticalData[index].WLCp = 9999.999d;
				this._opticalData[index].FWHM = 9999.999d;

				this._opticalData[index].Watt = 9999.999d;
				this._opticalData[index].Lm = 9999.999d;
				this._opticalData[index].Lx = 9999.999d;

				this._opticalData[index].GeneralCRI = 9999.999d;
			}
			else
			{
				this._opticalData[index].CIE1931X = -9999.999d;
				this._opticalData[index].CIE1931Y = -9999.999d;
				this._opticalData[index].CIE1931Z = -9999.999d;
				this._opticalData[index].CIE1931x = -9999.999d;
				this._opticalData[index].CIE1931y = -9999.999d;
				this._opticalData[index].CIE1931z = -9999.999d;
				this._opticalData[index].Purity = -9999.999d;
				this._opticalData[index].CCT = -9999.999d;

				this._opticalData[index].WLP = -9999.999d;
				this._opticalData[index].WLP2 = -9999.999d;
				this._opticalData[index].WLPNIR = -9999.999d;
				this._opticalData[index].WLD = -9999.999d;
				this._opticalData[index].WLCv = -9999.999d;
				this._opticalData[index].WLCp = -9999.999d;
				this._opticalData[index].FWHM = -9999.999d;

				this._opticalData[index].Watt = -9999.999d;
				this._opticalData[index].Lm = -9999.999d;
				this._opticalData[index].Lx = -9999.999d;

				this._opticalData[index].GeneralCRI = -9999.999d;
			}
        }

		private double CalcNextTime(double startTime, double upBoundTime, double currentCount, double targetCount)
        {

			double nextTime = startTime; 
            //double timeStep = 1.0d;            
            double countIncreasRate = 1.0d;
			//double middleCount = 45000;		

			//if ( mode == 0 )
			//{
			//    middleCount = this._devSetting.LimitTargetCount;
			//}
			//else if ( mode == 1 )
			//{
			//    middleCount = (this._devSetting.LimitTargetCount + this._devSetting.LimitLowCount) / 2;
			//}

			if (startTime < ((double) MIN_INTEGRATION_TIME ) )
            {
				startTime = (double)MIN_INTEGRATION_TIME;
            }

			if (upBoundTime > ((double)MAX_INTEGRATION_TIME))
			{ 
				upBoundTime = (double)MAX_INTEGRATION_TIME;
			}
			
			countIncreasRate = currentCount / startTime;

			//if ( currentCount <= DARK_COUNT)	// it may be one dark chip
			//{
			//    nextTime = upBoundTime;
			//}
			//else
			//{
			//    timeStep = (middleCount / countIncreasRate) - startTime;
			//}

			//if ((startTime + timeStep) >= upBoundTime)
			//{
			//    nextTime = upBoundTime;
			//}
			//else
			//{
			//    if (mode == 0)
			//    {
			//        // the default "TargetCount" = 45000.
			//        nextTime = startTime + Math.Round(timeStep, 0, MidpointRounding.AwayFromZero));
			//    }
			//    else if (mode == 1)
			//    {
			//        // the "TargetCount is a little smaller than mode 0.
			//        nextTime = startTime + Math.Ceiling(timeStep));
			//    }
			//}

			nextTime = targetCount / countIncreasRate;

			if (nextTime >= upBoundTime)
			{
				nextTime = upBoundTime;
			}
			else if (nextTime <= MIN_INTEGRATION_TIME)
			{
				nextTime = (double)MIN_INTEGRATION_TIME;
			}
			else
			{
				if (nextTime >= startTime)
				{
					nextTime = Math.Floor(nextTime) ;		//  2.1 => 2  ; 2.0 => 2  ;  1.9  => 1
				}
				else
				{
					nextTime = Math.Ceiling(nextTime);		//	0.9 => 1  ;  1.0 => 1  ;  1.2 => 2
				}
			}

            return nextTime;
        }

		/// <summary>
		///  Trigger the device to get one spectrum intensity
		/// </summary>
		private bool MeterTrigger(double IntegralTime)
		{
			int pState = -1;
			int peakCount = 0;

			int rtn = -1;

			if (this._devSetting.IsEnableTrigger == false)
				return true;

			if (IntegralTime < (double)MIN_INTEGRATION_TIME)
			{
				IntegralTime = (double)MIN_INTEGRATION_TIME;
			}

			if ( IntegralTime > (double) MAX_INTEGRATION_TIME )
			{
				IntegralTime = (double)MAX_INTEGRATION_TIME;
			}

			this._pt.Start();

			rtn = _SMUSBM.SDUSBTrigger((int)IntegralTime, this._currentIntensityArray, ref pState, ref peakCount);

			this._pt.Stop();

			this._triggerSpanTime = this._pt.GetTimeSpan(ETimeSpanUnit.MilliSecond);
			this._triggerMaxCount = peakCount;

			if (this._devSetting.IsGetRawData)
			{
				this._SMUSBM.GetRawData();
			}

			if (0 == rtn)
			{
				this._errorNum = EDevErrorNumber.Device_NO_Error;
				return true;
			}
			else if (1 == rtn)
			{
				this._errorNum = EDevErrorNumber.OPRS_Trigger_Err;
				return false;
			}
			else
			{
				this._errorNum = EDevErrorNumber.OPRS_Trigger_Err;
				return false;
			}
		}

		private int LimitTrigger(uint index)
		{
			bool meterRtn = false;
			double secondIntTime = 0.0d;

			this._SMUSBM.SetMutiTriggerProcess(0, 0);
			//----------------------------------------------------------
			// (1) Firt, Trigger 
			//----------------------------------------------------------
			meterRtn = this.MeterTrigger(this._devSetting.LimitStartTime);

			if (meterRtn == false)		// MeterTrigger has error, the subfunction will set the error code
			{
				this.ResetData(index, 0);
				return -21;
			}

			if (this._devSetting.LimitStartTime != 0.0d)
			{
				this._opticalData[index].Ratio = this._triggerMaxCount / this._devSetting.LimitStartTime;		// Count / ms
				this._opticalData[index].MaxCount = (uint)this._triggerMaxCount;
			}
			else
			{
				this._opticalData[index].Ratio = double.MaxValue;
				this._opticalData[index].MaxCount = (uint)this._triggerMaxCount;
			}

			//----------------------------------------------------------
			// (2) Second Trigger 
			//-----------------------------------------------------------
			secondIntTime = this.CalcNextTime(	this._devSetting.LimitStartTime, 
																	this._paramSetting[index].LimitIntegralTime, 
																	this._triggerMaxCount, 
																	this._devSetting.LimitTargetCount );

			meterRtn = this.MeterTrigger(secondIntTime);
			this._opticalData[index].IntegralTime = secondIntTime;

			if (meterRtn == false)		// MeterTrigger has error, the subfunction will set the error code
			{
				this.ResetData(index, 0);
				return -22;
			}

			if  (this._triggerMaxCount < this._devSetting.LimitLowCount )
			{
				return 21;
			}
			else if  ( this._triggerMaxCount > this._devSetting.LimitHighCount )
			{
				return 22;
			}
			else
			{
				return 20;		// OK
			}

		}

		private int SmartTrigger(uint index)
		{
			bool meterRtn = true;
			int rtnNum = -1;
			bool isAdjustTime = false;
			double countIncreasRate = 1.0d;
			double triggerTime = (double) MIN_INTEGRATION_TIME;

			// (A) First, Trigger by record integration time
			triggerTime=this._recordIntTime[index];
			meterRtn = this.MeterTrigger(triggerTime);
			this._opticalData[index].IntegralTime = triggerTime;

			if (meterRtn == false)		// MeterTrigger has error, the subfunction will set the error code
			{
				this.ResetData(index, 0);
				return -301;
			}

			//-----------------------------------------------------------------------------------------------------------------------
			// [ state = A1 ]  ,  Count01 < Dark  , It is bad chip
			//-----------------------------------------------------------------------------------------------------------------------
			if (this._triggerMaxCount < DARK_COUNT)
			{
				this.ResetData(index, 0);
				rtnNum = 311;
				return rtnNum;	// return successful
			}
			//-----------------------------------------------------------------------------------------------------------------------
			// [ state = A2 ]  ,  Dark < Count01 <= Low  ,  Calculate the NEXT Integration Time
			//-----------------------------------------------------------------------------------------------------------------------
			else if (this._triggerMaxCount > DARK_COUNT && this._triggerMaxCount <= this._devSetting.LimitLowCount)
			{
				// (B) Re-calculate integration time, increase the time and "Second" trigger spectrometer
				triggerTime = this.CalcNextTime(	triggerTime, 
																	this._paramSetting[index].LimitIntegralTime, 
																	this._triggerMaxCount, 
																	this._devSetting.LimitTargetCount);

				meterRtn = this.MeterTrigger(triggerTime);
				this._opticalData[index].IntegralTime = triggerTime;
				if (meterRtn == false)		// MeterTrigger has error, the subfunction will set the error code
				{
					this.ResetData(index, 0);
					return -302;
				}

				//==================================================
				// [ state = A2B0 ]  ,  Low < Count02 <= High	,  It is OK
				//==================================================
				if (this._triggerMaxCount > this._devSetting.LimitLowCount && this._triggerMaxCount <= this._devSetting.LimitHighCount)
				{
					isAdjustTime = true;
					rtnNum = 320;
					if (Convert.ToInt32(triggerTime * 1000) < Convert.ToInt32(this._paramSetting[index].LimitIntegralTime*1000))
					{
						this._recordIntTime[index] = triggerTime;
					}
				}
				//==================================================
				// [ state = A2B1 ]  ,  Count02 <= Low  ,  It is still dark
				//==================================================
				else if (this._triggerMaxCount <= this._devSetting.LimitLowCount)
				{
					this.ResetData(index, 0);
					rtnNum = 321;
					return rtnNum;
				}
				//==================================================
				// [ state = A2B2 ]  ,  Count02 > High  , It jumps to saturation ??
				//==================================================
				else
				{
					this.ResetData(index, 1);
					rtnNum = 322;
					return rtnNum;
				}
			}
			//-----------------------------------------------------------------------------------------------------------------------
			// [ state = A3 ]   ,  Low < Count01 <= High  ,  It is OK
			//-----------------------------------------------------------------------------------------------------------------------
			else if (this._triggerMaxCount > this._devSetting.LimitLowCount && this._triggerMaxCount <= this._devSetting.LimitHighCount)
			{
				isAdjustTime = true;
				rtnNum = 330;
				if (Convert.ToInt32(triggerTime * 1000) < Convert.ToInt32(this._paramSetting[index].LimitIntegralTime * 1000))
				{
					this._recordIntTime[index] = triggerTime;
				}
			}
			//-----------------------------------------------------------------------------------------------------------------------
			// [ state = A4 ]  ,  Count01 > High  ,  Saturation , Calculate the NEXT integration time
			//-----------------------------------------------------------------------------------------------------------------------
			else 
			{
				// (C) Re-calculate integration time, decrese the time and "Second" trigger spectrometer
				triggerTime = this.CalcNextTime(	triggerTime, 
																	this._paramSetting[index].LimitIntegralTime, 
																	this._triggerMaxCount, 
																	(this._devSetting.LimitTargetCount + this._devSetting.LimitLowCount ) / 2.0d);
				
				meterRtn = this.MeterTrigger(triggerTime);
				this._opticalData[index].IntegralTime = triggerTime;
				if (meterRtn == false)		// MeterTrigger has error, the subfunction will set the error code
				{
					this.ResetData(index, 0);
					return -304;
				}

				//==================================================
				// [ state = A4C0 ]  ,  Low < Count03 <= High , It is OK
				//==================================================
				if (this._triggerMaxCount > this._devSetting.LimitLowCount && this._triggerMaxCount <= this._devSetting.LimitHighCount)
				{
					isAdjustTime = true;
					rtnNum = 340;
					this._recordIntTime[index] = triggerTime;
				}
				//==================================================
				// [ state = A4C1 ]  ,  Count03 <= Low , Jump the Low count section ??
				//==================================================
				else if ( this._triggerMaxCount <= this._devSetting.LimitLowCount )
				{
					this.ResetData(index, 0);	// It become dark
					rtnNum = 341;
					return rtnNum;
				}
				//==================================================
				// [ state = A4C2 ]  ,  Count03 > High , It is still saturation ???
				//==================================================
				else
				{
					this.ResetData(index, 1);
					rtnNum = 342;
					return rtnNum;
				}
			}

			if (isAdjustTime == true)
			{
				//-----------------------------------------------------------------------------
				//  isAdjuectTime = true => It is OK state, then
				//  LowCount < this._triggerMaxCounr <= HighCount
				//------------------------------------------------------------------------------
				countIncreasRate = this._triggerMaxCount / triggerTime;

				double deltaCount = this._triggerMaxCount - this._devSetting.LimitLowCount;

				if (deltaCount < countIncreasRate)
				{
					this._recordIntTime[index] += 2.0d;
				}
				else if ( ( deltaCount >= countIncreasRate ) && ( deltaCount < (2.0d * countIncreasRate) ) )
				{
					this._recordIntTime[index] += 1.0d;
				}
				

				deltaCount = this._devSetting.LimitHighCount - this._triggerMaxCount;
				if ( deltaCount < countIncreasRate )
				{
					this._recordIntTime[index] -= 2.0d;
				}
				else if  ( deltaCount >= countIncreasRate && deltaCount < ( 2.0d * countIncreasRate ))
				{
					this._recordIntTime[index] -= 1.0d;
				}
			}

			return  rtnNum;
		}

		private int SmartTriggerOld(uint index)
		{
			bool meterRtn = false;
			int countLevel = 0;

			//if ((Convert.ToInt32(this._recordIntTime[index] * 1000) == Convert.ToInt32(this._maxLimitTime * 1000)) &&
			//        (this._triggerMaxCount < this._devSetting.LimitLowCount))
			//{
			//    this._recordIntTime[index] = this._devSetting.LimitStartTime;
			//}

			meterRtn = this.MeterTrigger(this._recordIntTime[index]);
			this._opticalData[index].Ratio = 1.0d;

			if (meterRtn == false)
			{
				this.ResetData(index, 0 );
				return 0	;
			}

			// countLevel = this._settingData.LimitLowCount
			// countLevel = this._settingData.LimitTargetCount
			countLevel = (this._devSetting.LimitLowCount + this._devSetting.LimitTargetCount) / 2;
			if (this._triggerMaxCount <= countLevel) // <= The "CalcNextTime() function will trace the target count.
			{
				this._recordIntTime[index] = this.CalcNextTime(	this._recordIntTime[index], 
																							this._paramSetting[index].LimitIntegralTime, 
																							this._triggerMaxCount, 
																							this._devSetting.LimitTargetCount);
				meterRtn = this.MeterTrigger(this._recordIntTime[index]);
				this._opticalData[index].Ratio = 10.0d;
				if (meterRtn == false)
				{
					this.ResetData(index, 0);
					return 1;
				}
			}

			countLevel = (this._devSetting.LimitHighCount + this._devSetting.LimitTargetCount) / 2;
			if (this._triggerMaxCount >= countLevel) // <= The "CalcNextTime() function will trace the target count.
			{
				this._recordIntTime[index] = this.CalcNextTime(	this._recordIntTime[index], 
																							this._paramSetting[index].LimitIntegralTime, 
																							this._triggerMaxCount, 
																							this._devSetting.LimitTargetCount);
				meterRtn = this.MeterTrigger(this._recordIntTime[index]);
				this._opticalData[index].Ratio = 20.0d;
				if (meterRtn == false)
				{
					this.ResetData(index, 0);
					return 1;
				}
			}

			// Saturation
			if (this._triggerMaxCount >= this._devSetting.LimitHighCount)
			{
				this._recordIntTime[index] = this._recordIntTime[index] * 0.5d;

				if (this._recordIntTime[index] < this._devSetting.LimitStartTime)
				{
					this._recordIntTime[index] = this._devSetting.LimitStartTime;
				}

				meterRtn = this.MeterTrigger(this._recordIntTime[index]);
				this._opticalData[index].Ratio = 100.0d;

				if (meterRtn == false)
				{
					this.ResetData(index, 0);
					return 2;
				}

				countLevel = (this._devSetting.LimitLowCount + this._devSetting.LimitTargetCount) / 2;
				if (this._triggerMaxCount <= countLevel) // <= The "CalcNextTime() function will trace the target count.
				{
					this._recordIntTime[index] = this.CalcNextTime(	this._recordIntTime[index], 
																								this._paramSetting[index].LimitIntegralTime, 
																								this._triggerMaxCount, 
																								this._devSetting.LimitTargetCount);
					meterRtn = this.MeterTrigger(this._recordIntTime[index]);
					this._opticalData[index].Ratio = 110.0d;
					if (meterRtn == false)
					{
						this.ResetData(index, 0);
						return 3;
					}
				}

				countLevel = (this._devSetting.LimitHighCount + this._devSetting.LimitTargetCount) / 2;
				if (this._triggerMaxCount >= countLevel) // <= The "CalcNextTime() function will trace the target count.
				{
					this._recordIntTime[index] = this.CalcNextTime(	this._recordIntTime[index], 
																								this._paramSetting[index].LimitIntegralTime, 
																								this._triggerMaxCount,
																								this._devSetting.LimitTargetCount);
					meterRtn = this.MeterTrigger(this._recordIntTime[index]);
					this._opticalData[index].Ratio = 120.0d;
					if (meterRtn == false)
					{
						this.ResetData(index, 0);
						return 3;
					}
				}

				// Saturation againg
				if (this._triggerMaxCount >= this._devSetting.LimitHighCount)
				{
					this._opticalData[index].Ratio = 130.0d;
					this.ResetData(index, 1);	// Reset data to saturation status
					return 4;	// impossible
				}
			}

			this._opticalData[index].IntegralTime = this._recordIntTime[index];

			return 5678;
			
		}

        #endregion

        #region >>> Public Methods <<<
        
        /// <summary>
        ///  Initialize the spectrometer 
        /// </summary>
        public bool Init(int deviceNum, string spectrometerSN, string sphereSN )
        {
            int rtn;

			this._errorNum = EDevErrorNumber.Device_NO_Error;

            if ( deviceNum < 0 )
            {
				this._errorNum = EDevErrorNumber.SpectrometerDevice_Init_Err;
                return false;
            }

            string sptSN = spectrometerSN.Substring(0, SERIAL_NUMBER_COUNT);

            rtn = this._SMUSBM.Login(sptSN, deviceNum);

			// if rtn == 6, Re-check the sphere serial number again
			if (rtn == 6 )
			{
				if (this._SMUSBM.LoginFixtrue( sphereSN ) != 0 )
				{
					this._errorNum = EDevErrorNumber.SphereSN_Err;
					return false;
				}
			}
			else if ( 3 == rtn || 4 == rtn || 5 == rtn )
			{
				this._errorNum = EDevErrorNumber.SpectrometerSN_Err;
				return false;
			}
			else if (rtn != 0)
			{
				this._errorNum = EDevErrorNumber.SpectrometerDevice_Init_Err;
				return false;          
			}

			this._version = this._SMUSBM.GetLastVersion();
			this._serialNum = spectrometerSN + "&" + sphereSN;

			this._SMUSBM.SetAllConfig(1);		// Enable all parameters calculation
			this._SMUSBM.SetConfigCRI(0);    // Disable CRI calculation

			return true;

        }

        public bool SetConfigToMeter(OptiDevSetting devSetting)
        {
            if (this._errorNum != EDevErrorNumber.Device_NO_Error) 
                return false;

			this._devSetting = devSetting;

            this._SMUSBM.SetAllConfig(1);

			if (devSetting.IsCalcCRIData == false)
            {
                this._SMUSBM.SetConfigCRI(0);
            }
            else
            {
                this._SMUSBM.SetConfigCRI(1);
            }

            this._SMUSBM.SimulateMode(0, 0);
            this._SMUSBM.SetMutiTriggerProcess(0, 0);

            //------------------------------------------------------------------------------
            // 0: success
            // 1: setting count is lower than  10000
            // 2: setting count is higher than 60000
            // 3: interval between button count and top count is less than 10000
            //------------------------------------------------------------------------------
			if (this._SMUSBM.AIWS(devSetting.AutoLowCount, devSetting.AutoHighCount) != 0)
            {
				this._errorNum = EDevErrorNumber.OPRS_AutoCountSetting_Err;
            }

			this._SMUSBM.SetXaxisOrder(devSetting.XAxisResolution);
			this._SMUSBM.SetYaxisOrder(devSetting.YAxisResolution);

            // AttenuatorPos = index  + 1,  " 1-base " for OPTiMUM design
			this._SMUSBM.SetFixtrueFilterAddress((int)devSetting.AttenuatorPos + 1, ref this._attenuatorPercent);
			
			for (int i = 0; i < this._recordIntTime.Length; i++)
			{
				this._recordIntTime[i] = this._devSetting.LimitStartTime;
			}

			if (this._devSetting.LimitStartTime < MIN_INTEGRATION_TIME)
			{
				this._devSetting.LimitStartTime = MIN_INTEGRATION_TIME;
			}
			return true;
        
        }

		public bool SetParamToMeter(OptiSettingData[] paramSetting)
		{
			int settingCount = 1;

			if (paramSetting == null)
			{
				settingCount = 1;
			}
			else
			{
				this._paramSetting = paramSetting;
				settingCount = this._paramSetting.Length;
			}

			this._opticalData = new OptiData[settingCount];
			this._recordIntensityArray = new double[settingCount][];
			this._recordIntTime = new double[settingCount];

			for (int i = 0; i < settingCount; i++)
			{
				this._opticalData[i] = new OptiData();
				this._recordIntensityArray[i] = new double[CCD2048_PIXEL_LENGTH];
				this._recordIntTime[i] = this._devSetting.LimitStartTime;
			}

			if (paramSetting == null)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

        /// <summary>
        /// Re-conncect to the spectrometer
        /// </summary>
        public bool ReConnect()
        {
            if (this._SMUSBM.ReConnect() != 0)
            {
				this._errorNum = EDevErrorNumber.OPRS_Reconnect_Fail;
                return false;
            }
            else
            {
                this._errorNum = EDevErrorNumber.Device_NO_Error;
                return true;
            }
        }

        /// <summary>
        /// Get wavelength data of the spectrometer 
        /// </summary>
        public double[] GetXWavelength()
        {
            double minWavelength = 0.0d;
            double maxWavelength = 0.0d;
            int length = 0;

            int rtn = -1;

            if (this._errorNum != EDevErrorNumber.Device_NO_Error)    // Trigger function is not OK or spectrometer is in ERROR Status
            {
                this._errorNum = EDevErrorNumber.GetWavelength_Fail;
                this._wavelengthArray = new double[CCD2048_PIXEL_LENGTH];
            }

			for (int index = 0; index < this._recordIntensityArray.Length; index++)
			{
				switch (this._devSetting.IntensityDataMode)
				{
					case 0:
						rtn = this._SMUSBM.GetWavelength(this._wavelengthArray, ref minWavelength, ref maxWavelength, ref length);
						break;
					case 1:
						rtn = this._SMUSBM.GetXYasixData(1, this._wavelengthArray, this._recordIntensityArray[index]);
						break;
					case 2:
						rtn = this._SMUSBM.GetXYasixData(2, this._wavelengthArray, this._recordIntensityArray[index]);
						break;
					case 3:
						rtn = this._SMUSBM.GetXYasixData(3, this._wavelengthArray, this._recordIntensityArray[index]);
						break;
					default:
						rtn = -1;
						break;
				}
			}

            if ( 0 == rtn )
            {
                this._errorNum = EDevErrorNumber.Device_NO_Error;
            }
            else if (1 == rtn)
            {
				this._errorNum = EDevErrorNumber.GetWavelength_Fail;
				this._wavelengthArray = new double[CCD2048_PIXEL_LENGTH];
            }
            else
            {
                this._errorNum = EDevErrorNumber.GetWavelength_Fail;
				this._wavelengthArray = new double[CCD2048_PIXEL_LENGTH];
            }

            return this._wavelengthArray;
        }

        /// <summary>
        /// Get intensity data of the device 
        /// </summary>
        public double[] GetYSpectrumIntensity(uint index)
        {

            int rtn = -1;

            if ( this._errorNum != EDevErrorNumber.Device_NO_Error )              // Trigger function is not OK or spectrometer is in ERROR Status
            {
				this._errorNum = EDevErrorNumber.OPRS_GetIntensity_Fail;
				this._recordIntensityArray[index] = new double[CCD2048_PIXEL_LENGTH];    // Reset to 0.0d
            }

            switch ( this._devSetting.IntensityDataMode )
            {
                case 0:
                    break;
                case 1:
					rtn = this._SMUSBM.GetXYasixData(1, this._wavelengthArray, this._recordIntensityArray[index]);
                    break;
                case 2:
					rtn = this._SMUSBM.GetXYasixData(2, this._wavelengthArray, this._recordIntensityArray[index]);
                    break;
                case 3:
					rtn = this._SMUSBM.GetXYasixData(3, this._wavelengthArray, this._recordIntensityArray[index]);
                    break;
                default:
                    rtn = -1;
                    break;
            }

            if ( 0 == rtn )
            {
                this._errorNum = EDevErrorNumber.Device_NO_Error;
            }
            else if (1 == rtn)
            {
				this._errorNum = EDevErrorNumber.OPRS_GetIntensity_Fail;
				this._recordIntensityArray[index] = new double[CCD2048_PIXEL_LENGTH];    // Reset to 0.0d
            }
            else
            {
				this._errorNum = EDevErrorNumber.OPRS_GetIntensity_Fail;
				this._recordIntensityArray[index] = new double[CCD2048_PIXEL_LENGTH];    // Reset to 0.0d
            }

			return this._recordIntensityArray[index];
        }

		public double[][] GetYSpectrumIntensityAll()
		{
			return this._recordIntensityArray;
		}

        public double[] GetYAbsoluateSpectrum(uint index)
        {
			if (index < 0 || index >= this._recordIntensityArray.Length)
			{
				index = 0;
			}
            return this._recordIntensityArray[index];
        }

		public double[][] GetYAbsoluateSpectrumAll()
		{
			return this._recordIntensityArray;
		}

		public int Trigger(uint index)
		{
			int rtnNum = -1;

			if (this._paramSetting == null)
			{
				this._errorNum = EDevErrorNumber.NoSourceMeterParamSettingData;
				return -12345;
			}

			if (this._devSetting.IsEnableTrigger == false)
				return 12345;

			switch (this._paramSetting[index].SensingMode)
			{
				case ESensingMode.Limit:
					rtnNum = this.LimitTrigger(index);
					break;
				//-----------------------------------------------------------------------
				case ESensingMode.Fixed:
					if (this.MeterTrigger(this._paramSetting[index].FixIntegralTime))
					{
						rtnNum = 10;
					}
					else
					{
						rtnNum = -10;
					}
					this._opticalData[index].IntegralTime = this._paramSetting[index].FixIntegralTime;
					this._opticalData[index].Ratio = 0.0d;
					break;
				//-----------------------------------------------------------------------
				case ESensingMode.Limit02:
					rtnNum = this.SmartTrigger(index);
					break;
				//-----------------------------------------------------------------------	
				default:
					break;
			}

			this._opticalData[index].MaxCount = (uint)this._triggerMaxCount;
			this._opticalData[index].CountPercent = this._triggerMaxCount / MAX_COUNT;
			this._opticalData[index].TriggerStatus = rtnNum;
			Array.Copy(this._currentIntensityArray, this._recordIntensityArray[index], this._currentIntensityArray.Length);

			return rtnNum;
		}				 


		/// <summary>
		///  Calculate photometry and colorimetry information
		/// </summary>
		/// <returns></returns>
		public bool CalculateParameters(uint index)
		{
			double CIEY = 0.0d;
			double CIEx = 0.0d;
			double CIEy = 0.0d;
			double purity = 0.0d;
			double CCT = 0.0d;

			double WLP = 0.0d;
			double WLP2 = 0.0d;
			double WLPNIR = 0.0d;
			double WLD = 0.0d;
			double FWHM = 0.0d;
			double WLCv = 0.0d;
			double WLCp = 0.0d;

			double Watt = 0.0d;
			double Lm = 0.0d;
			double Lx = 0.0d;

			int rtn = 0;

			if (this._devSetting.IsEnableCalc == false)
				return true;

            if (this._opticalData[index].MaxCount < DARK_COUNT)
            {
                ResetData(index, 0);
                return true;
            }
            else if (this._opticalData[index].MaxCount >= MAX_COUNT)
            {
                ResetData(index, 1);
                return true;
            }

			switch (this._devSetting.OperationMode)
			{
				case ESpectrometerOpMode.Normal:     // Normal
					rtn = _SMUSBM.xy(ref CIEY, ref CIEx, ref CIEy);

					rtn = rtn | _SMUSBM.LpLdLL(ref WLP, ref WLPNIR, ref WLD, ref FWHM);
					rtn = rtn | _SMUSBM.Purity(ref purity);
					rtn = rtn | _SMUSBM.LC(1, ref WLCv);
					rtn = rtn | _SMUSBM.LC(2, ref WLCp);
					rtn = rtn | _SMUSBM.CCT(ref CCT);
					rtn = rtn | _SMUSBM.Lx(ref Lx);
					rtn = rtn | _SMUSBM.Lm(ref Lm);
					rtn = rtn | _SMUSBM.Watt(ref Watt);     // mW
					break;
				////------------------------------------------------------------------------------------
				//case EOperatioMode.Test_F01:     // Filter01
				//    rtn = _SMUSBM.TriggerAgain(1);

				//    rtn = rtn | _SMUSBM.xy(ref CIEY, ref CIEx, ref CIEy);
				//    rtn = rtn | _SMUSBM.LpLdLL(ref WLP, ref WLPNIR, ref WLD, ref FWHM);
				//    rtn = rtn | _SMUSBM.Purity(ref purity);
				//    rtn = rtn | _SMUSBM.LC(1, ref WLCv);
				//    rtn = rtn | _SMUSBM.LC(2, ref WLCp);
				//    rtn = rtn | _SMUSBM.CCT(ref CCT);
				//    rtn = rtn | _SMUSBM.Lx(ref Lx);
				//    rtn = rtn | _SMUSBM.Lm(ref Lm);
				//    rtn = rtn | _SMUSBM.Watt(ref Watt);
				//    break;
				////------------------------------------------------------------------------------------
				//case EOperatioMode.Test_F02:     // Filter 02
				//    rtn = _SMUSBM.TriggerAgain(2);

				//    rtn = rtn | _SMUSBM.xy(ref CIEY, ref CIEx, ref CIEy);
				//    rtn = rtn | _SMUSBM.LpLdLL(ref WLP, ref WLPNIR, ref WLD, ref FWHM);
				//    rtn = rtn | _SMUSBM.Purity(ref purity);
				//    rtn = rtn | _SMUSBM.LC(1, ref WLCv);
				//    rtn = rtn | _SMUSBM.LC(2, ref WLCp);
				//    rtn = rtn | _SMUSBM.CCT(ref CCT);
				//    rtn = rtn | _SMUSBM.Lx(ref Lx);
				//    rtn = rtn | _SMUSBM.Lm(ref Lm);
				//    rtn = rtn | _SMUSBM.Watt(ref Watt);
				//    break;
				////------------------------------------------------------------------------------------
				case ESpectrometerOpMode.Filter:     // Filter = Filter01 + Filter 02
					rtn = _SMUSBM.TriggerAgain(1);

					rtn = rtn | _SMUSBM.xy(ref CIEY, ref CIEx, ref CIEy);
					rtn = rtn | _SMUSBM.LpLdLL(ref WLP, ref WLPNIR, ref WLD, ref FWHM);
					rtn = rtn | _SMUSBM.Purity(ref purity);
					rtn = rtn | _SMUSBM.LC(1, ref WLCv);
					rtn = rtn | _SMUSBM.LC(2, ref WLCp);
					rtn = rtn | _SMUSBM.CCT(ref CCT);

					//rtn = rtn | _SMUSBM.Lx(ref Lx);
					//rtn = rtn | _SMUSBM.Lm(ref Lm);
					//rtn = rtn | _SMUSBM.W(ref Watt);

					rtn = _SMUSBM.TriggerAgain(2);
					//rtn = rtn |_SMUSBM.xy(ref CIEY, ref CIEx, ref CIEy);
					//rtn = rtn | _SMUSBM.LpLdLL(ref WLP, ref WLPNIR, ref WLD, ref FWHM);
					//rtn = rtn | _SMUSBM.Purity(ref purity);
					//rtn = rtn | _SMUSBM.LC(1, ref WLCv);
					//rtn = rtn | _SMUSBM.LC(2, ref WLCp);
					//rtn = rtn | _SMUSBM.CCT(ref CCT);
					rtn = rtn | _SMUSBM.Lx(ref Lx);
					rtn = rtn | _SMUSBM.Lm(ref Lm);
					rtn = rtn | _SMUSBM.Watt(ref Watt);     // mW
					break;
				//------------------------------------------------------------------------------------
				default:
					rtn = -1;
					break;
			}

			if (rtn == 0)	// successful
			{
				this._opticalData[index].CIE1931Y = CIEY;
				this._opticalData[index].CIE1931x = CIEx;
				this._opticalData[index].CIE1931y = CIEy;
				this._opticalData[index].CIE1931z = 1 - CIEx - CIEy;
				this._opticalData[index].Purity = purity;
				this._opticalData[index].CCT = CCT;

				this._opticalData[index].WLP = WLP;
				this._opticalData[index].WLP2 = WLP2;
				this._opticalData[index].WLPNIR = WLPNIR;
				this._opticalData[index].WLD = WLD;
				this._opticalData[index].WLCv = WLCv;
				this._opticalData[index].WLCp = WLCp;
				this._opticalData[index].FWHM = FWHM;

				this._opticalData[index].Lx = Lx;
				this._opticalData[index].Lm = Lm;
				this._opticalData[index].Watt = Watt;
			}

			//---------------------------------------------------------------
			// Calculate the color rendering index value
			//---------------------------------------------------------------
			if (this._devSetting.IsCalcCRIData == true)
			{
				double[] specialCRI = new double[OptiData.CRI_SAMPLES_COUNT];
				double generalCRI = 0.0d;

				rtn = rtn | _SMUSBM.CRI(specialCRI, ref generalCRI);

				if (specialCRI.Length == OptiData.CRI_SAMPLES_COUNT)
				{
					Array.Copy(this._opticalData[index].SpecialCRI, specialCRI, specialCRI.Length);
					this._opticalData[index].GeneralCRI = generalCRI;
				}
				else
				{
					rtn = -1;
				}
			}

			if (0 == rtn)	// successful
			{
				this._errorNum = EDevErrorNumber.Device_NO_Error;
				return true;
			}
			else
			{
				this.ResetData(index, 3);	// status = 3, for Optimum bug problem, why can't calculate ??
				this._errorNum = EDevErrorNumber.OPRS_Calculation_Err;
				return false;
			}

		}

		public double[] GetDarkSample(uint count, uint IntTime)
		{
			return null;
		}

        public void Close()
        { 
            
        }

        public string[] GetEPPROMConfigData()
        {
            return null;
        }
        
        #endregion
    }

}
