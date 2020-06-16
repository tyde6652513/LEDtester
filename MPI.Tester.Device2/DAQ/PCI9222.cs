using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Maths;
using System.Diagnostics;

namespace MPI.Tester.Device.SourceMeter
{
    public class PCI9222 : IDAQ
    {
		#region >>> Private Proberty <<<

        private const uint CHANNELCOUNT = 1;

        private ushort _cardID;
        private ushort _channel;
		private DAQSettingData _daqSettingdata;
        private ushort _range;
        private uint[] _sampleCount;
        private double[] _vBuffer;
        private ushort[] _buffer;
        private Stopwatch _timeoutTimer;
      
		#endregion

        #region >>> Constructor / Disposor <<<

        public PCI9222()
        {
            this._cardID = 0;

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

            if (maxRange <= 0.25)
            {
                this._range = PCI_DASK.AD_B_0_25_V;
            }
            else if (maxRange <= 0.5)
            {
                this._range = PCI_DASK.AD_B_0_5_V;
            }
            else if (maxRange <= 1)
            {
                this._range = PCI_DASK.AD_B_1_V;
            }
            else if (maxRange <= 1.25)
            {
                this._range = PCI_DASK.AD_B_1_25_V;
            }
            else if (maxRange <= 2)
            {
                this._range = PCI_DASK.AD_B_2_V;
            }
            else if (maxRange <= 5)
            {
                this._range = PCI_DASK.AD_B_5_V;
            }
            else if (maxRange <= 10)
            {
                this._range = PCI_DASK.AD_B_10_V;
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

            //ushort AI_Config = DSA_DASK.P9527_AI_PseudoDifferential | DSA_DASK.P9527_AI_Coupling_DC;

            //short err = DSA_DASK.DSA_AI_9527_ConfigChannel(this._cardID, this._channel, this._range, AI_Config, true);

            //if (err != DSA_DASK.NoError)
            //{
            //    return false;
            //}

            return true;
        }

        private bool CheckSampleCount(ElectSettingData[] data)
        {			
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

                if (this._sampleCount[i] > 30000)
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
            short result = PCI_DASK.Register_Card(PCI_DASK.PCI_9222, (ushort)this._daqSettingdata.CardNumber);

            if (result < PCI_DASK.NoError)
            {
                return false;
            }

            this._cardID = (ushort)result;

            ////////////////////////////////////////////////////////////////////////////////////////
            // (1) Config Sample Rate
            ////////////////////////////////////////////////////////////////////////////////////////
            uint ScanIntrv = (uint)(80000000 / this._daqSettingdata.SampleRate * CHANNELCOUNT);

            uint SampIntrv = (uint)(80000000 / this._daqSettingdata.SampleRate);

            int err = PCI_DASK.AI_9222_CounterInterval(this._cardID, ScanIntrv, SampIntrv);

            if (err != PCI_DASK.NoError)
            {
                PCI_DASK.Release_Card(this._cardID);

                return false;
            }

            ////////////////////////////////////////////////////////////////////////////////////////
            // (2) Config TRG Config
            ////////////////////////////////////////////////////////////////////////////////////////

			//PCI_DASK.P922x_AI_Differential 需將 AI0 Low 接 AIGND
			ushort ConfigCtrl = PCI_DASK.P922x_AI_Differential | PCI_DASK.P922x_AI_CONVSRC_INT;

            ushort TrigCtrl = PCI_DASK.P922x_AI_TRGMOD_POST | PCI_DASK.P922x_AI_TRGSRC_GPI0;// | DASK.P922x_AI_EnReTigger;

            //重新 Trigger 的次數，設0為無上限
            uint ReTriggerCount = 0;

            bool AutoResetBuf = true;

            err = PCI_DASK.AI_9222_Config(this._cardID, ConfigCtrl, TrigCtrl, ReTriggerCount, AutoResetBuf);

            if (err != PCI_DASK.NoError)
            {
                PCI_DASK.Release_Card(this._cardID);

                return false;
            }

            ////////////////////////////////////////////////////////////////////////////////////////
            // (3) Disable Double Buffer Mode
            ////////////////////////////////////////////////////////////////////////////////////////
            err = PCI_DASK.AI_AsyncDblBufferMode(this._cardID, false);

            if (err < 0)
            {
                PCI_DASK.Release_Card(this._cardID);

                return false;
            }

            return true;
        }

        public void Close()
        {
            uint AccessCnt;

            short err = PCI_DASK.AI_AsyncClear(this._cardID, out AccessCnt);

            if (err != PCI_DASK.NoError)
            {
                Console.Write("AI_AsyncClear Error: %d\n", err);

                PCI_DASK.AI_AsyncClear(this._cardID, out AccessCnt);

                PCI_DASK.AI_ContBufferReset(this._cardID);
            }

            if (err != PCI_DASK.NoError)
            {
                Console.WriteLine("DSA_AI_EventCallBack ERR");
            }

            err = PCI_DASK.Release_Card(0);

            if (err != PCI_DASK.NoError)
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
            this._buffer = new ushort[(int)this._sampleCount[index]];

            ////////////////////////////////////////////////////////////////////////////////////////
            // (5) Set and SampleCount
            ////////////////////////////////////////////////////////////////////////////////////////
            if (this._vBuffer == null || this._vBuffer.Length != this._sampleCount[index])
            {
                this._vBuffer = new double[this._sampleCount[index]];
            }

            ////////////////////////////////////////////////////////////////////////////////////////
            // (6) Config Cont Buffer Setup
            ////////////////////////////////////////////////////////////////////////////////////////
            ushort BufId;

            short err = PCI_DASK.AI_ContBufferReset(this._cardID);

            if (err != PCI_DASK.NoError)
            {
                return false;
            }

            err = PCI_DASK.AI_ContBufferSetup(this._cardID, this._buffer, this._sampleCount[index], out BufId);

            if (err != PCI_DASK.NoError)
            {
                return false;
            }

            unsafe
            {
                err = PCI_DASK.AI_ContReadChannel(this._cardID, this._channel, this._range, this._buffer, this._sampleCount[index], 0/*Ignored*/, PCI_DASK.ASYNCH_OP);
            }
            if (err != PCI_DASK.NoError)
            {
                return false;
            }

            return true;
        }

		public double[] GetDataFromDAQ(uint channel, uint index)
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            // (8) Check whether AI Acquisition is done
            ////////////////////////////////////////////////////////////////////////////////////////
            this._timeoutTimer.Restart();

            bool Stopped = false;

            uint AccessCnt;

            short err;

            do
            {
                err = PCI_DASK.AI_AsyncCheck(this._cardID, out Stopped, out AccessCnt);

                if (err < 0)
                {
                    PCI_DASK.AI_AsyncClear(this._cardID, out AccessCnt);

                    PCI_DASK.AI_ContBufferReset(this._cardID);

                    PCI_DASK.Release_Card(this._cardID);

                    return null;
                }

                if (this._timeoutTimer.ElapsedMilliseconds > 3000)
                {
                    Console.WriteLine("[PCI9222Wrapper], GetDataFromDAQ(), Timeout");

                    return null;
                }

            } while (!Stopped);


            ////////////////////////////////////////////////////////////////////////////////////////
            // (9) Get Data
            ////////////////////////////////////////////////////////////////////////////////////////
            err = PCI_DASK.AI_ContVScale(this._cardID, this._range, this._buffer, this._vBuffer, (int)AccessCnt);

            if (err < 0)
            {
                return null;
            }

            return this._vBuffer;
        }

		#endregion
    }
}
