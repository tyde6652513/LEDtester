using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NationalInstruments;
using NationalInstruments.DAQmx;

namespace MPI.Tester.DeviceCommon
{
	public class NIDAQ : IDAQ
	{
		private string _physicalChannel;
		private double[] _minimumList;
		private double[] _maximumList;
		private double _rate;
		private int[] _samplesPerChannelList;
		private string _triggerSource;
		private string _deviceName;

		private Task _task;
		private AnalogSingleChannelReader _dataReader;
		private DigitalEdgeStartTriggerEdge _digitalEdge;
		
		private double[][] _yDataList;

		private int _timeoutValue;

		private NationalInstruments.DAQmx.Device _device;

		private uint _previousSettingIndex;

		private bool _isStarted;

		private int _actualCnt;

		private IAsyncResult _daqResult;

		public NIDAQ( string deviceName )
		{
			this._deviceName = deviceName;

			this._physicalChannel = this._deviceName + "/ai0";

			this._triggerSource = "/" + this._deviceName + "/PFI0";

			this._timeoutValue = 3000; // unit: ms

			this._digitalEdge = DigitalEdgeStartTriggerEdge.Rising;

			this._device = DaqSystem.Local.LoadDevice( this._deviceName );
		}

		#region >>> Public Property <<<

		public string SerialNumber
		{
			get { return this._device.SerialNumber.ToString(); }
		}

		public string SoftwareVersion
		{
			get { return ( DaqSystem.Local.DriverMajorVersion.ToString() + "." + DaqSystem.Local.DriverMinorVersion.ToString() + "." + DaqSystem.Local.DriverUpdateVersion.ToString() ); }
		}

		public string HardwareVersion
		{
			get { throw new NotImplementedException(); }
		}

		public string[] PhysicalChannelList
		{
			get { return DaqSystem.Local.GetPhysicalChannels( PhysicalChannelTypes.AI, PhysicalChannelAccess.External ); }
		}

		public int Timeout
		{
			get { return this._timeoutValue; }

			set
			{
				this._timeoutValue = value;

				if ( this._task != null )
				{
					this._task.Stream.Timeout = this._timeoutValue;
				}
			}
		}

		public string PhysicalChannel
		{
			get { return this._physicalChannel; }

			set { this._physicalChannel = value; }
		}

		#endregion

		#region >>> Public Method <<<

		public bool Init( DAQSettingData data )
		{
			try
			{
				this._rate = data.SampleRate;

				daqInitialization();

				return true;
			}
			catch ( DaqException ex )
			{
				checkTimeoutError( ex );

				return false;
			}
			catch ( Exception ex )
			{
				return false;
			}
		}

		public bool SetParamToDAQ( ElectSettingData[] data )
		{
			try
			{
				this._minimumList = new double[ data.Length ];
				this._maximumList = new double[ data.Length ];
				this._samplesPerChannelList = new int[ data.Length ];

				for ( int i = 0; i < data.Length; i++ )
				{
					this._minimumList[ i ] = ( -1 ) * checkVoltageRange( data[ i ].MsrtRange );
					this._maximumList[ i ] = checkVoltageRange( data[ i ].MsrtRange );

					this._samplesPerChannelList[ i ] = ( int ) ( this._rate * data[ i ].RTHIm2ForceTime / 1000 );
				}

				this._yDataList = new double[ data.Length ][];

				for ( int i = 0; i < data.Length; i++ )
				{
					this._yDataList[ i ] = new double[ this._samplesPerChannelList[ i ] ];
				}

				this._isStarted = false;

				return true;
			}
			catch ( Exception ex )
			{
				disposeTasks();

				return false;
			}
		}

		public bool SetTrigger( uint settingIndex )
		{
			try
			{
				if ( this._task == null )
				{
					return false;
				}

				if ( checkParameterChange( settingIndex ) )
				{
					this._task.AIChannels[ 0 ].Minimum = this._minimumList[ settingIndex ];
					this._task.AIChannels[ 0 ].Maximum = this._maximumList[ settingIndex ];
					this._task.Timing.SamplesPerChannel = this._samplesPerChannelList[ settingIndex ];

					// commit the task
					this._task.Control( TaskAction.Commit );

					this._isStarted = true;
				}

				this._daqResult = this._dataReader.BeginMemoryOptimizedReadMultiSample( this._samplesPerChannelList[ settingIndex ], null, null, this._yDataList[ settingIndex ] );

				this._previousSettingIndex = settingIndex;

				return true;
			}
			catch ( DaqException ex )
			{
				checkTimeoutError( ex );

				return false;
			}
			catch ( Exception ex )
			{
				disposeTasks();

				return false;
			}
		}

		public double[] GetDataFromDAQ( uint settingIndex )
		{
			try
			{
				this._actualCnt = 0;

				this._dataReader.EndMemoryOptimizedReadMultiSample( this._daqResult, out this._actualCnt );

				if ( this._actualCnt != this._samplesPerChannelList[ settingIndex ] )
				{
					return new double[ 0 ];
				}

				return this._yDataList[ settingIndex ];
			}
			catch ( DaqException ex )
			{
				checkTimeoutError( ex );

				return new double[ 0 ];
			}
			catch ( Exception ex )
			{
				disposeTasks();
				return new double[ 0 ];
			}
		}

		public void Close()
		{
			disposeTasks();
		}

		#endregion

		#region >>> Private Method <<<

		private void disposeTasks()
		{
			this._task.Stop();
			this._task.Control( TaskAction.Abort );
			this._task.Control( TaskAction.Unreserve );

			this._task.Dispose();

			this._task = null;
		}

		private void daqInitialization()
		{
			double initializationMin = -10;
			double initializationMax = 10;
			int initializationSampleCount = 10;

			this._device.Reset();

			this._previousSettingIndex = 0;

			this._task = new Task();
			this._task.Stream.Timeout = this._timeoutValue;

			checkSampleRate();
			this._task.AIChannels.CreateVoltageChannel( this._physicalChannel, "Task", ( AITerminalConfiguration ) ( -1 ), initializationMin, initializationMax, AIVoltageUnits.Volts );
			this._task.Timing.ConfigureSampleClock( "", this._rate, SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples, initializationSampleCount );
			this._task.Triggers.StartTrigger.ConfigureDigitalEdgeTrigger( this._triggerSource, this._digitalEdge );

			this._dataReader = new AnalogSingleChannelReader( this._task.Stream );
			this._dataReader.SynchronizeCallbacks = true;

			// commit the task
			this._task.Control( TaskAction.Commit );
		}

		private bool checkParameterChange( uint currentSettingIndex )
		{
			return !( ( this._minimumList[ currentSettingIndex ] == this._minimumList[ this._previousSettingIndex ] ) && ( this._maximumList[ currentSettingIndex ] == this._maximumList[ this._previousSettingIndex ] ) && ( this._samplesPerChannelList[ currentSettingIndex ] == this._samplesPerChannelList[ this._previousSettingIndex ] ) && this._isStarted );
		}

		private void checkTimeoutError( DaqException ex )
		{
			if ( ex.Error == -200284 ) // read timeout
			{
				this._task.Stop();
				this._task.Control( TaskAction.Abort );
				this._task.Control( TaskAction.Unreserve );
			}
			else
			{
				disposeTasks();
			}
		}

		private void checkSampleRate()
		{
			if ( this._rate < this._device.AIMinimumRate )
			{
				this._rate = this._device.AIMinimumRate;
			}

			if ( this._rate > this._device.AIMaximumSingleChannelRate )
			{
				this._rate = this._device.AIMaximumSingleChannelRate;
			}
		}

		private double checkVoltageRange( double voltageRange )
		{
			double maxRange = this._device.AIVoltageRanges[ this._device.AIVoltageRanges.Length - 1 ];

			if ( Math.Abs( voltageRange ) > maxRange )
			{
				voltageRange = maxRange;
			}

			return Math.Abs( voltageRange );
		}

		#endregion


	}
}
