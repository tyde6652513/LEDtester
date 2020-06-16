using System;

namespace MPI.Tester.DeviceCommon
{
	public class LANSettingData
	{
		private string _ipAddress;
		private int _port;
		private int _timeOut;
		private int _bufferSize;

		public LANSettingData()
		{
			this._ipAddress = "192.168.50.2";
			this._port = 5025;
			this._bufferSize = 512;
			this._timeOut = 5000;
		}

		#region >>> Public Property <<<

		public string IPAddress
		{
            get { return this._ipAddress; }
            set { this._ipAddress = value; }			
		}

		public int Port
		{
            get { return this._port; }
            set { this._port = value; }
			
		}

        public int BufferSize
        {
            get { return this._bufferSize; }
            set { this._bufferSize = value; }
        }

		public int TimeOut
		{
            get { return this._timeOut; }
            set { this._timeOut = value; }
		}
	
		#endregion
	}
}
