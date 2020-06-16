using System;

namespace MPI.Tester.DeviceCommon
{
    public class RS232SettingData
    {
        private object _lockObj;

        private string _comPortName;
        private int _baudRate;
        private int _dataBits;
        private int _stopBits;
        private string _parity;
        private string _terminator;
        private string _flowCtrl;
        private int _timeOut;

        public RS232SettingData()
        {
            this._lockObj = new object();

			this._comPortName = "COM10";
            this._baudRate = 57600;
			this._dataBits = 8;
            this._stopBits = 1;
            this._parity = "NONE";
            this._terminator = "\x0d";
            this._flowCtrl = "488.1";
            this._timeOut = 1000;
        }

        #region >>> Public Property <<<

        public string ComPortName
        {
            get { return this._comPortName; }
            set { lock (this._lockObj) { this._comPortName = value; } }       
        }

        public int BaudRate
        {
            get { return this._baudRate; }
            set { lock (this._lockObj) { this._baudRate = value; } }
        }

        public int DataBits
        {
            get { return this._dataBits; }
            set { lock (this._lockObj) { this._dataBits = value; } }
        }

        public int StopBits
        {
            get { return this._stopBits; }
            set { lock (this._lockObj) { this._stopBits = value; } }
        }

        public string Parity
        {
            get { return this._parity; }
            set { lock (this._lockObj) { this._parity = value; } }
        }

        public string Terminator
        {
            get { return this._terminator; }
            set { lock (this._lockObj) { this._terminator = value; } }
        }

        public string FlowCtrl
        {
            get { return this._flowCtrl; }
            set { lock (this._lockObj) { this._flowCtrl = value; } }
        }

        public int TimeOut
        {
            get { return this._timeOut; }
            set { lock (this._lockObj) { this._timeOut = value; } }
        }

        #endregion
    }
}
