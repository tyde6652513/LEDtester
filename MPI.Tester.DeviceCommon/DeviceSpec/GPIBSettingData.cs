using System;

namespace MPI.Tester.DeviceCommon
{
    public class GPIBSettingData
    {
        private object _lockObj;

        private int _deviceNumber;
        private int _primaryAddress;
        private int _secondAddress;
        private int _EOTModeFlag;
        private int _EOSModeFlag;
        private int _ioTimeOut;
        private string _protocal;
        private int _bufferSize;

        public GPIBSettingData()
        {
            this._lockObj = new object();

            this._deviceNumber = 0;
            this._primaryAddress = 24;
            this._secondAddress = 0;
            this._EOTModeFlag = 1;
            this._EOSModeFlag = 0;
            this._ioTimeOut = 12;
            this._protocal = "488.1";
            this._bufferSize = 100;
        }

        #region >>> Public Property <<<

        public int DeviceNumber
        {
            get { return this._deviceNumber; }
            set { lock (this._lockObj) { this._deviceNumber = value; } }         
        }

        public int PrimaryAddress
        {
            get { return this._primaryAddress; }
            set { lock (this._lockObj) { this._primaryAddress = value; } }
        }

        public int SecondAddress
        {
            get { return this._secondAddress; }
            set { lock (this._lockObj) { this._secondAddress = value; } }
        }

        public int EOTModeFlag
        {
            get { return this._EOTModeFlag; }
            set { lock (this._lockObj) { this._EOTModeFlag = value; } }
        }

        public int EOSModeFlag
        {
            get { return this._EOSModeFlag; }
            set { lock (this._lockObj) { this._EOSModeFlag = value; } }
        }

		public int IOTimeOut
        {
            get { return this._ioTimeOut; }
			set { lock (this._lockObj) { this._ioTimeOut = value; } }
        }

        public string Protocal
        {
            get { return this._protocal; }
            set { lock (this._lockObj) { this._protocal = value; } }         
        }

        public int BufferSize
        {
            get { return this._bufferSize; }
            set { lock (this._lockObj) { this._bufferSize = value; } }
        }

        #endregion

    }
}
