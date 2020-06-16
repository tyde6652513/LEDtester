using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Maths;
using System.Diagnostics;

namespace MPI.Tester.Device.SourceMeter
{
    public class DAQ2213 : IDAQ
    {
		#region >>> Private Proberty <<<

        private const uint CHANNELCOUNT = 1;

        private ushort _cardID;
        private double _actualRate;
        private ushort _channel;
		private DAQSettingData _daqSettingdata;
        private ushort _range;
        private uint[] _sampleCount;
        private double[][] _vBuffer;
        private short[][] _buffer;
        private Stopwatch _timeoutTimer;
      
		#endregion

        #region >>> Constructor / Disposor <<<

        public DAQ2213()
        {
            this._cardID = 0;

            this._actualRate = 0.0d;

            this._channel = 0;

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

            foreach (var item in data)
            {
                if (item.MsrtType == EMsrtType.RTH)
                {
                    if(maxRange < Math.Abs(item.MsrtRange))
                    {
                        maxRange = Math.Abs(item.MsrtRange);
                    }
                }
                else if (item.MsrtType == EMsrtType.THY)
                {
                    if (maxRange < Math.Abs(item.MsrtRange))
                    {
                        maxRange = Math.Abs(item.MsrtRange);
                    }
                }
            }

            if (maxRange <= 1.25)
            {
                this._range = D2K_DASK.AD_B_1_25_V;
            }
            else if (maxRange <= 2.5)
            {
                this._range = D2K_DASK.AD_B_2_5_V;
            }
            else if (maxRange <= 5)
            {
                this._range = D2K_DASK.AD_B_5_V;
            }
            else if (maxRange <= 10)
            {
                this._range = D2K_DASK.AD_B_10_V;
            }
            else
            {
                return false;
            }

            bool isUserDAQ = false;

            foreach (var item in data)
            {
                if (item.MsrtType == EMsrtType.THY || item.MsrtType == EMsrtType.RTH)
                {
                    isUserDAQ = true;
                }
            }

            if (!isUserDAQ)
            {
                return true;
            }

            short err = D2K_DASK.D2K_AI_CH_Config(this._cardID, (short)this._channel, (ushort)(this._range | D2K_DASK.AI_DIFF));

            if (err != D2K_DASK.NoError)
            {
                return false;
            }

            return true;
        }

        private bool CheckSampleCount(ElectSettingData[] data)
        {
            this._buffer = new short[data.Length][];

            this._vBuffer = new double[data.Length][];

            this._sampleCount = new uint[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].MsrtType == EMsrtType.RTH)
                {
                    double pointSpeed = 1.0d / (double)this._daqSettingdata.SampleRate * 1000.0d;

                    this._sampleCount[i] = (uint)(data[i].RTHIm2ForceTime / pointSpeed - 100);
                }
                else if (data[i].MsrtType == EMsrtType.THY)
                {
                    //50 For Shift
                    this._sampleCount[i] = data[i].SweepContCount + 50;
                }

                if (this._sampleCount[i] >= 0 && this._sampleCount[i] < 30000)
                {
                    this._buffer[i] = new short[(int)this._sampleCount[i]];

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

        public bool Init(DAQSettingData data)
        {
            if (data == null)
            {
                return false;
            }

            this._daqSettingdata = data;

            if (this._daqSettingdata.SampleRate > 250000)
            {
                this._daqSettingdata.SampleRate = 250000;
            }

            if (this._daqSettingdata.SampleRate < 100)
            {
                this._daqSettingdata.SampleRate = 100;
            }

            ////////////////////////////////////////////////////////////////////////////////////////
            // (0) Register Card
            ////////////////////////////////////////////////////////////////////////////////////////

            short result;

            try
            {
                result = D2K_DASK.D2K_Register_Card(D2K_DASK.DAQ_2213, (ushort)this._daqSettingdata.CardNumber);

                if (result < D2K_DASK.NoError)
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

            this._cardID = (ushort)result;

            ////////////////////////////////////////////////////////////////////////////////////////
            // (1) Config TRG Config
            ////////////////////////////////////////////////////////////////////////////////////////
            //重新 Trigger 的次數，設0為無上限
            ushort ReTriggerCount = 0;

            uint DelayTrigger = 0;

            short err = D2K_DASK.D2K_AI_Config(this._cardID, D2K_DASK.DAQ2K_AI_ADCONVSRC_Int, D2K_DASK.DAQ2K_AI_TRGSRC_ExtD, DelayTrigger, 0, ReTriggerCount, true);

            if (err != PCI_DASK.NoError)
            {
                D2K_DASK.D2K_Release_Card(0);

                return false;
            }

            ////////////////////////////////////////////////////////////////////////////////////////
            // (2 Disable Double Buffer Mode
            ////////////////////////////////////////////////////////////////////////////////////////
            err = D2K_DASK.D2K_AI_AsyncDblBufferMode(this._cardID, false);

            if (err < 0)
            {
                D2K_DASK.D2K_Release_Card(this._cardID);

                return false;
            }

            return true;
        }

        public void Close()
        {
            short err = D2K_DASK.D2K_Release_Card(0);

            if (err != D2K_DASK.NoError)
            {
                Console.WriteLine("Release_Card ERR");
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
            // (6) Config Cont Buffer Setup
            ////////////////////////////////////////////////////////////////////////////////////////
            ushort BufId;

            short err = D2K_DASK.D2K_AI_ContBufferSetup(this._cardID, this._buffer[index], (uint)this._buffer[index].Length, out BufId);

            if (err != D2K_DASK.NoError)
            {
                return false;
            }

            unsafe
            {
                uint ScanIntrv = (uint)(40000000 / this._daqSettingdata.SampleRate * CHANNELCOUNT);

                uint SampIntrv = (uint)(40000000 / this._daqSettingdata.SampleRate);

                err = D2K_DASK.D2K_AI_ContReadChannel(this._cardID, this._channel, BufId, (uint)this._sampleCount[index], ScanIntrv, SampIntrv, D2K_DASK.ASYNCH_OP);
            }
            if (err != D2K_DASK.NoError)
            {
                return false;
            }

            return true;
        }

        public double[] GetDataFromDAQ(uint index)
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            // (8) Check whether AI Acquisition is done
            ////////////////////////////////////////////////////////////////////////////////////////
            this._timeoutTimer.Restart();

            byte Stopped;

            uint AccessCnt;

            short err;

            do
            {
                err = D2K_DASK.D2K_AI_AsyncCheck(this._cardID, out Stopped, out AccessCnt);

                if (err < 0)
                {
                    D2K_DASK.D2K_Release_Card(this._cardID);

                    return null;
                }

                if (this._timeoutTimer.ElapsedMilliseconds > 3000)
                {
					this._timeoutTimer.Stop();

                    Console.WriteLine("[DAQ2213], GetDataFromDAQ(), Timeout");

                    return null;
                }

            } while (Stopped == 0);

			this._timeoutTimer.Stop();

            ////////////////////////////////////////////////////////////////////////////////////////
            // (9) Get Data
            ////////////////////////////////////////////////////////////////////////////////////////
            err = D2K_DASK.D2K_AI_ContVScale(this._cardID, this._range, this._buffer[index], this._vBuffer[index], (int)AccessCnt);

            if (err < 0)
            {
                return null;
            }

            return this._vBuffer[index];
        }

		#endregion
    }
}
