using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;
using Ivi.Visa.Interop;

namespace MPI.Tester.Device
{
	public class IVIConnect : IConnect
	{
        private Ivi.Visa.Interop.FormattedIO488 _ioObj;
		private string _resourceName;

        private int TIMEOUT_IVI_OPEN  = 30000;
        private int TIMEOUT_IVI_QUERY = 50000;

		public IVIConnect()
		{
			this._ioObj = new FormattedIO488();

			this._resourceName = string.Empty;
		}

        public IVIConnect(string resourceName) : this()
		{
            this._resourceName = resourceName;
		}

		#region >>> Public Proberty <<<

		public int LastErrorNum
		{
			get { return 0; }
		}

		public string LastErrorStr
		{
			get { return "NONE"; }
		}

		public string BufferData
		{
			get { return "NONE"; }
		}

		#endregion

		#region >>> Private Method <<<

        public bool Open(out string Information)
        {
            try
            {
                ResourceManager rmSession = new ResourceManager();

                this._ioObj.IO = rmSession.Open(this._resourceName, AccessMode.NO_LOCK, TIMEOUT_IVI_OPEN, "") as IMessage;

                this._ioObj.IO.Timeout = TIMEOUT_IVI_QUERY;

                Information = "";

                return true;
            }
            catch (Exception e)
            {
                Information = e.ToString();

                Console.WriteLine("[IviConnect], Open()," + e.ToString());

                return false;
            }
            finally
            { }
        }

		public bool SendCommand(string command)
		{
			try
			{
                if (command != string.Empty)
                {
                    this._ioObj.WriteString(command);
                }

				return true;
			}
			catch
			{
				return false;
			}
		}

		public bool QueryCommand(string command)
		{
			return true;
		}

		public bool WaitAndGetData(out string result)
		{
			try
			{
				result = this._ioObj.ReadString().Trim();

				return true;
			}
			catch(Exception e)
			{
				result = "Time Out!";

                Console.WriteLine("[IviConnect], WaitAndGetData()," + e.ToString());

				return false;
			}
			finally
			{ }
		}

		public void Close()
		{
			try
			{
				this._ioObj.IO.Close();
			}
			catch(Exception e)
			{
                Console.WriteLine("[IviConnect], Close()," + e.ToString());
			}
			finally
			{ }
		}

		#endregion
	}
}
