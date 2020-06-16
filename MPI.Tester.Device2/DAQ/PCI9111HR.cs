using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Maths;
using System.Diagnostics;

namespace MPI.Tester.Device.SourceMeter
{
    class PCI9111HR : IDAQ
    {
        private const uint CHANNELCOUNT = 1;

        private ushort _cardID;
        private DAQSettingData _daqSettingdata;
        private ushort _range;
        private uint[] _sampleCount;
        private double[][] _vBuffer;
        private ushort[][] _buffer;
        private ushort _channel;
        private Stopwatch _timeoutTimer;

        private IntPtr[] _pBuffer;

        public PCI9111HR()
        {
            this._cardID = 0;

            this._channel = 0;

            this._timeoutTimer = new Stopwatch();
        }

        #region >>> Public Proberty <<<

        public string HardwareVersion
        {
            get { return "NONE"; }
        }

        public string SoftwareVersion
        {
            get { return "NONE"; }
        }

        public string SerialNumber
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
                    if (maxRange < Math.Abs(item.MsrtRange))
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

            if (maxRange <= 0.625)
            {
                this._range = PCI_DASK.AD_B_0_625_V;
            }
            else if (maxRange <= 1.25)
            {
                this._range = PCI_DASK.AD_B_1_25_V;
            }
            else if (maxRange <= 2.5)
            {
                this._range = PCI_DASK.AD_B_2_5_V;
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

            return true;
        }

        private bool CheckSampleCount(ElectSettingData[] data)
        {
            this._buffer = new ushort[data.Length][];

            this._vBuffer = new double[data.Length][];

            this._sampleCount = new uint[data.Length];

            this._pBuffer = new IntPtr[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].MsrtType == EMsrtType.RTH)
                {
                    double pointSpeed = 1.0d / (double)this._daqSettingdata.SampleRate * 1000.0d;

                    this._sampleCount[i] = (uint)(data[i].RTHIm2ForceTime / pointSpeed - 100);
                }
                else if (data[i].MsrtType == EMsrtType.THY)
                {
                    int count = Convert.ToInt32(Math.Floor((data[i].SweepContCount + 50.0d) / 512.0d));

                    if((data[i].SweepContCount + 50.0d) % 512.0d > 0)
                    {
                        count += 1;
                    }

                    //50 For Shift
                    this._sampleCount[i] = (uint)count * 512;
                }

                if (this._sampleCount[i] >= 0 && this._sampleCount[i] < 30000)
                {
                    this._buffer[i] = new ushort[(int)this._sampleCount[i]];

                    this._vBuffer[i] = new double[(int)this._sampleCount[i]];

                    this._pBuffer[i] = System.Runtime.InteropServices.Marshal.AllocHGlobal(sizeof(uint) * (int)this._sampleCount[i]);
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
            short result = PCI_DASK.Register_Card(PCI_DASK.PCI_9111HR, (ushort)this._daqSettingdata.CardNumber);

            if (result < PCI_DASK.NoError)
            {
                return false;
            }

            this._cardID = (ushort)result;

            ////////////////////////////////////////////////////////////////////////////////////////
            // (1) Config TRG Config
            ////////////////////////////////////////////////////////////////////////////////////////
            int err = PCI_DASK.AI_9111_Config(this._cardID, PCI_DASK.TRIG_INT_PACER, PCI_DASK.P9111_TRGMOD_POST, 1024);

            if (err != PCI_DASK.NoError)
            {
                PCI_DASK.Release_Card(this._cardID);

                return false;
            }

            ////////////////////////////////////////////////////////////////////////////////////////
            // (2) Disable Double Buffer Mode
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
            PCI_DASK.Release_Card(this._cardID);
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
            short err = 0;

            unsafe
            {
                err = PCI_DASK.AI_ContReadChannel(this._cardID, this._channel, this._range, this._pBuffer[index], this._sampleCount[index], this._daqSettingdata.SampleRate, PCI_DASK.ASYNCH_OP);
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

            bool Stopped;

            uint AccessCnt;

            short err;

            do
            {
                err = PCI_DASK.AI_AsyncCheck(this._cardID, out Stopped, out AccessCnt);

                if (err < 0)
                {
                    PCI_DASK.AI_AsyncClear(this._cardID, out AccessCnt);

                    PCI_DASK.Release_Card(this._cardID);

                    return null;
                }

                if (this._timeoutTimer.ElapsedMilliseconds > 3000)
                {
                    Console.WriteLine("[PCI9111HR], GetDataFromDAQ(), Timeout");

                    return null;
                }

            } while (!Stopped);

            err = PCI_DASK.AI_AsyncClear(this._cardID, out AccessCnt);

            if (err < 0)
            {
                return null;
            }

            ////////////////////////////////////////////////////////////////////////////////////////
            // (9) Get Data
            ////////////////////////////////////////////////////////////////////////////////////////
            err = PCI_DASK.AI_ContVScale(this._cardID, this._range, this._pBuffer[index], this._vBuffer[index], (int)AccessCnt);

            if (err < 0)
            {
                return null;
            }

            return this._vBuffer[index];
        }

        #endregion
    }
}
