using System;
using System.Collections.Generic;
using System.Text;

using System.IO.Ports;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Device
{
    public class DriverObjConnect : IConnect  // weakness: assume the data length is less than the buffer length
	{
        object _driverObj;

        public DriverObjConnect(object setting)            
		{
            _driverObj = setting;
		}

		#region >>> Public Proberty <<<

		public int LastErrorNum
		{
            get { return 0; }
		}

        public string LastErrorStr
        { get; private set; }

		public string BufferData
		{
			get
			{
				throw new NotImplementedException();
			}
		}

        public object DriverObject { get { return _driverObj; } }

		#endregion

        #region >>> Public Proberty <<<

        #endregion

        #region >>> Public Methos <<<

        public bool Open(out string info)
        {
            info = "";
            return true;
        }

		public bool SendCommand(string cmd)
		{
            return true;
		}

        public bool SendCommand(string cmd, bool isTrim)
        {
            return true;
        }

		public bool QueryCommand(string command)
		{
			throw new NotImplementedException();
		}

		public bool WaitAndGetData(out string result)
		{
            result = "";
            return true;
		}

        public bool WaitAndGetData(out string result, int dataLength)
        {
            result = string.Empty;

            return true;
        }

		public void Close()
		{            
		}

		#endregion
	}
}
