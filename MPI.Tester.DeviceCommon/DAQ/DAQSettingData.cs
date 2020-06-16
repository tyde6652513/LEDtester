using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
	public class DAQSettingData : ICloneable
	{
		private object _lockObj;
        private uint _cardNumber;
		private uint _sampleRate;
        private uint _daqCalibrationBufferID;

		public DAQSettingData()
		{
			this._lockObj = new object();

            this._cardNumber = 0;

			this._sampleRate = 100000;

            this._daqCalibrationBufferID = 0;
		}

        public uint CardNumber
        {
            get { return this._cardNumber; }
            set { lock (this._lockObj) { this._cardNumber = value; } }
        }

		public uint SampleRate
		{
			get { return this._sampleRate; }
			set { lock (this._lockObj) { this._sampleRate = value; } }
		}

        public uint DAQCalibrationBufferID
        {
            get { return this._daqCalibrationBufferID; }
            set { lock (this._lockObj) { this._daqCalibrationBufferID = value; } }
        }

		public object Clone()
		{
			return this.MemberwiseClone();
		}
	}
}
