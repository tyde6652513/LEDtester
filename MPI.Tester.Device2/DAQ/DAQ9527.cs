using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;
using MPI.Tester.DeviceCommon;
using MPI.Tester.Maths;
using System.Diagnostics;

namespace MPI.Tester.Device.SourceMeter
{
    public class DAQ9527 : IDAQ
	{
		#region >>> Private Proberty <<<

        private ushort _cardID;
        private double _actualRate;
        private ushort _channel;
		private DAQSettingData _daqSettingdata;
		private IntPtr[] _pBuffer;
        private ushort _range;
        private uint[] _sampleCount;
        private double[][] _vBuffer;
        private Stopwatch _timeoutTimer;
      
		#endregion

        #region >>> Constructor / Disposor <<<

		public DAQ9527()
        {
            this._cardID = 0;

            this._actualRate = 0.0d;

            this._channel = DSA_DASK.P9527_AI_CH_0;        
    
            this._timeoutTimer = new Stopwatch();
        }

        #endregion

        #region >>> Public Proberty <<<

        public string HardwareVersion
		{
			get { return "DAQ9527"; }
		}

		public string SerialNumber
		{
			get { return "NONE"; }
		}

		public string SoftwareVersion
		{
			get { return "NONE"; }
		}

		#endregion

        #region >>> Private Method <<<

        private bool SetMsrtRange(ElectSettingData[] data)
        {
            double maxRange = 0.0d;

			ushort newMaxRange = 0;

            foreach (var item in data)
            {
                if (item.MsrtType == EMsrtType.RTH ||
                    item.MsrtType == EMsrtType.THY ||
                    item.MsrtType == EMsrtType.VLR)
                {
                    if (maxRange < Math.Abs(item.MsrtRange))
                    {
                        maxRange = Math.Abs(item.MsrtRange);
                    }
                }
            }

            if (maxRange <= 0.316)
            {
				newMaxRange = DSA_DASK.AD_B_0_316_V;
            }
            else if (maxRange <= 1.0)
            {
				newMaxRange = DSA_DASK.AD_B_1_V;
            }
            else if (maxRange <= 3.16)
            {
				newMaxRange = DSA_DASK.AD_B_3_16_V;
            }
            else if (maxRange <= 10.0)
            {
				newMaxRange = DSA_DASK.AD_B_10_V;
            }
            else if (maxRange <= 40.0)
            {
				newMaxRange = DSA_DASK.AD_B_40_V;
            }
            else
            {
                return false;
            }

			if (this._range == newMaxRange)
			{
				return true;
			}
			else
			{
				this._range = newMaxRange;
			}

            bool isUserDAQ = false;

            foreach (var item in data)
            {
                if (item.MsrtType == EMsrtType.THY ||
                    item.MsrtType == EMsrtType.RTH ||
                    item.MsrtType == EMsrtType.VLR)
                {
                    isUserDAQ = true;
                }
            }

            if (!isUserDAQ)
            {
                return true;
            }

            ////////////////////////////////////////////////////////////////////////////////////////
            // (4) Config Channel
            ////////////////////////////////////////////////////////////////////////////////////////
            ushort AI_Config = DSA_DASK.P9527_AI_PseudoDifferential | DSA_DASK.P9527_AI_Coupling_DC;

            short err = DSA_DASK.DSA_AI_9527_ConfigChannel(this._cardID, this._channel, this._range, AI_Config, true);

            if (err != DSA_DASK.NoError)
            {
                return false;
            }

            return true;
        }

        private bool CheckSampleCount(ElectSettingData[] data)
        {
            this._sampleCount = new uint[data.Length];

            this._pBuffer = new IntPtr[data.Length];

            this._vBuffer = new double[data.Length][];

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].MsrtType == EMsrtType.RTH ||
                    data[i].MsrtType == EMsrtType.VLR)
                {
                    double pointSpeed = 1.0d / (double)this._daqSettingdata.SampleRate * 1000.0d;

                    double totalTime = data[i].RTHImForceTime + data[i].RTHIhForceTime + data[i].RTHIm2ForceTime;

                    this._sampleCount[i] = (uint)(totalTime / pointSpeed) + 50;
                }
                else if (data[i].MsrtType == EMsrtType.THY)
                {
					//50 For Shift
                    this._sampleCount[i] = data[i].SweepContCount + 50;
                }

                if (this._sampleCount[i] >= 0)// && this._sampleCount[i] < 30000)
                {
                    this._pBuffer[i] = Marshal.AllocHGlobal(sizeof(uint) * (int)this._sampleCount[i]);

                    this._vBuffer[i] = new double[(int)this._sampleCount[i]];
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

		#endregion

		#region >>> Public Method <<<

        public bool Init(DAQSettingData data, uint channelCount)
        {
            if (data == null)
            {
                return false;
            }

            this._daqSettingdata = data;

            if (this._daqSettingdata.SampleRate > 432000)
            {
                this._daqSettingdata.SampleRate = 432000;
            }

            if (this._daqSettingdata.SampleRate < 100)
            {
                this._daqSettingdata.SampleRate = 100;
            }

            ////////////////////////////////////////////////////////////////////////////////////////
            // (0) Register Card
            ////////////////////////////////////////////////////////////////////////////////////////
            short result = DSA_DASK.DSA_Register_Card(DSA_DASK.PCI_9527, (ushort)this._daqSettingdata.CardNumber);

            if (result < DSA_DASK.NoError)
            {
                return false;
            }

            this._cardID = (ushort)result;

            ////////////////////////////////////////////////////////////////////////////////////////
            // (1) Register Card
            ////////////////////////////////////////////////////////////////////////////////////////
            int err = DSA_DASK.DSA_CAL_SetDefaultBank(this._cardID, (ushort)this._daqSettingdata.DAQCalibrationBufferID);

            if (result < DSA_DASK.NoError)
            {
                return false;
            }

            this._cardID = (ushort)result;

            ////////////////////////////////////////////////////////////////////////////////////////
            // (1) Config Sample Rate
            ////////////////////////////////////////////////////////////////////////////////////////
            err = DSA_DASK.DSA_AI_9527_ConfigSampleRate(this._cardID, this._daqSettingdata.SampleRate, out this._actualRate);

            if (err != DSA_DASK.NoError && err != DSA_DASK.ErrorInvalidDDSPhase)
            {
                return false;
            }

            ////////////////////////////////////////////////////////////////////////////////////////
            // (2) Config TRG Config
            ////////////////////////////////////////////////////////////////////////////////////////
            ushort Trigger_Target = DSA_DASK.P9527_TRG_AI;

            ushort Trigger_Config = DSA_DASK.P9527_TRG_MODE_POST | DSA_DASK.P9527_TRG_SRC_EXTD | DSA_DASK.P9527_TRG_Positive;// | DSA_DASK.P9527_TRG_EnReTigger;

            //重新 Trigger 的次數，設0為無上限
            uint ReTriggerCount = 0;

            uint DelayTrigger = 0;

            err = DSA_DASK.DSA_TRG_Config(this._cardID, Trigger_Target, Trigger_Config, ReTriggerCount, DelayTrigger);

            if (err != DSA_DASK.NoError)
            {
                return false;
            }

            ////////////////////////////////////////////////////////////////////////////////////////
            // (3) Config Async Dbl Buffer Mode
            ////////////////////////////////////////////////////////////////////////////////////////
            err = DSA_DASK.DSA_AI_AsyncDblBufferMode(this._cardID, false);

            if (err != DSA_DASK.NoError)
            {
                return false;
            }

            return true;
        }

        public void Close()
        {
            uint AccessCnt;

            short err = DSA_DASK.DSA_AI_AsyncClear(this._cardID, out AccessCnt);

            if (err != DSA_DASK.NoError)
            {
                Console.Write("DSA_AI_AsyncCheck Error: %d\n", err);

                DSA_DASK.DSA_AI_AsyncClear(this._cardID, out AccessCnt);

                DSA_DASK.DSA_AI_ContBufferReset(this._cardID);
            }

            if (err != DSA_DASK.NoError)
            {
                Console.WriteLine("DSA_AI_EventCallBack ERR");
            }

            err = DSA_DASK.DSA_Release_Card(0);

            if (err != DSA_DASK.NoError)
            {
                Console.WriteLine("DSA_Release_Card ERR");
            }
        }

        public bool SetParamToDAQ(ElectSettingData[] item)
        {
            if (item == null || item.Length == 0)
            {
                return false;
            }

            if (!this.SetMsrtRange(item))
            {
                return false;
            }

            if (!this.CheckSampleCount(item))
            {
                return false;
            }

            return true;
        }

        public bool SetTrigger(uint index)
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            // (5) Config Cont Buffer Setup
            ////////////////////////////////////////////////////////////////////////////////////////
            ushort BufId;

			short err = DSA_DASK.DSA_AI_ContBufferReset(this._cardID);

            if (err != DSA_DASK.NoError)
            {
                return false;
            }

            err = DSA_DASK.DSA_AI_ContBufferSetup(this._cardID, this._pBuffer[index], this._sampleCount[index], out BufId);

            if (err != DSA_DASK.NoError)
            {
                return false;
            }

            unsafe
            {
                err = DSA_DASK.DSA_AI_ContReadChannel(this._cardID, this._channel, 0/*Ignored*/, &BufId, this._sampleCount[index], 0/*Ignored*/, DSA_DASK.ASYNCH_OP);
            }
            if (err != DSA_DASK.NoError)
            {
                return false;
            }

            return true;
        }

		public double[] GetDataFromDAQ(uint channel, uint index)
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            // (6) Check whether AI Acquisition is done
            ////////////////////////////////////////////////////////////////////////////////////////
            this._timeoutTimer.Restart();

            byte Stopped = 0;

            uint AccessCnt;

            short err;

            do
            {
                err = DSA_DASK.DSA_AI_AsyncCheck(this._cardID, out Stopped, out AccessCnt);

                if (err < 0)
                {
                    DSA_DASK.DSA_AI_AsyncClear(this._cardID, out AccessCnt);

                    DSA_DASK.DSA_AI_ContBufferReset(this._cardID);

                    return null;
                }

                if (this._timeoutTimer.ElapsedMilliseconds > 3000)
                {
                    Console.WriteLine("[DAQ9527Wrapper], GetDataFromDAQ(), Timeout");

                    return null;
                }

            } while (Stopped == 0);

            ////////////////////////////////////////////////////////////////////////////////////////
            // (7) Get Data
            ////////////////////////////////////////////////////////////////////////////////////////
            err = DSA_DASK.DSA_AI_ContVScale(this._cardID, this._range, this._pBuffer[index], this._vBuffer[index], (int)this._vBuffer[index].Length);

            if (err < 0)
            {
                return null;
            }

            return this._vBuffer[index];
        }

		#endregion
	}
}
