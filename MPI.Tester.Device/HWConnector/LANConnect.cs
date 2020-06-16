using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Device
{
	/// <summary>
	/// LAN connect class.
	/// </summary>
	public class LANConnect : IConnect  // weakness: assume the data length is less than the buffer length
	{
		private byte[] _readBuffer;
        private string _defaulIpAddr;
		private bool _isPrompt;

		private Socket _socket;
		private LANSettingData _setting;
		private List<byte> _byteBuffer;

        private SocketException _lastErrorNum;
        private string _lastErrorStr;

		public LANConnect()
		{
            this._setting = new LANSettingData();

            this._defaulIpAddr = this._setting.IPAddress;

            this._socket = null;

			this._isPrompt = false;
           
			this._byteBuffer = new List<byte>();
		}

		public LANConnect(LANSettingData setting) : this()
		{
			this._setting = setting;
		}

		#region >>> Public Proberty <<<

		public int LastErrorNum
		{
            get { return 0; }
		}

		public string LastErrorStr
		{
            get { return this._lastErrorStr; }
		}

		public string BufferData
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public bool IsPrompt
		{
            get { return this._isPrompt; }
            set { this._isPrompt = value; }
		}

		#endregion

        #region >>> Public Proberty <<<

        public bool IsIPV4(string ipAddress)
        {
            System.Net.IPAddress ipv4Addr;

            if (!System.Net.IPAddress.TryParse(ipAddress, out ipv4Addr))
                return false;

            return true;
        }

        #endregion

        #region >>> Public Methos <<<

        public bool Open(out string info)
        {
            info = "";

            if (!this.IsIPV4(this._setting.IPAddress))
            {
                this._setting.IPAddress = this._defaulIpAddr;
            }

            //this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //System.Net.Sockets.Socket _socket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        
            this._socket.NoDelay = true;
            //_socket.SendBufferSize = 128; // default 8192
            this._socket.DontFragment = true;
            this._socket.ReceiveTimeout = 15000; // default infinite

            try
            {
                IAsyncResult async = this._socket.BeginConnect(this._setting.IPAddress, this._setting.Port, null, null);

                info = this._socket.RemoteEndPoint.ToString();

                if (this._socket == null)
                {
                    return false;
                }

                this._readBuffer = new byte[this._socket.ReceiveBufferSize];

                if (!async.AsyncWaitHandle.WaitOne(this._setting.TimeOut))
                {
                    this._socket.Close();

                    this._socket = null;

                    return false;
                }
            }
            catch (SocketException e)
            {
                this._lastErrorStr = e.Message;

                this._socket = null;

                return false;
            }
            catch (Exception e)
            {
                this._lastErrorStr = e.Message;

                this._socket = null;

                return false;
            }

            return true;
        }

		public bool SendCommand(string cmd)
		{
            if (this._socket == null)
                return false;

            if (cmd == string.Empty)
            {
                return true;
            }

			cmd = string.Format("{0}\n", cmd);

			try
			{
				this._socket.Send(Encoding.ASCII.GetBytes(cmd));
				return true;
			}
            catch (SocketException e)
			{
                this._lastErrorStr = e.Message;
                this._socket = null;
				return false;
			}
		}

        public bool SendCommand(string cmd, bool isTrim)
        {
            if (this._socket == null)
                return false;

            if (cmd == string.Empty)
            {
                return true;
            }

            if (isTrim)
            {
                cmd = string.Format("{0}\n", cmd);
            }

            try
            {
                this._socket.Send(Encoding.ASCII.GetBytes(cmd));
                return true;
            }
            catch (SocketException e)
            {
                this._lastErrorStr = e.Message;
                this._socket = null;
                return false;
            }
        }

		public bool QueryCommand(string command)
		{
			throw new NotImplementedException();
		}

		public bool WaitAndGetData(out string result)
		{
            result = string.Empty;

            if (this._socket == null)
                return false;

			try
			{
				if (!this._isPrompt)
				{
					int read = 0;
					StringBuilder sb = new StringBuilder( this._socket.ReceiveBufferSize * 3 / 2 );					

					while ( ( read = _socket.Receive( _readBuffer ) ) > 0 )
					{
						sb.Append( Encoding.ASCII.GetString( _readBuffer, 0, read ) );

                        if (_readBuffer[read - 1] == (byte)'\n')
                            break;

						Thread.Sleep( 0 );
					}
					
					result = sb.ToString();
					sb = null;
				}
				else
				{
					string tempString = "initialization"; // initialize the variable "reading" in order to enter the follwing while loop
					string endPart = tempString.Substring( tempString.Length - 5, 5 );
					int bufferInd = 0; // initialize the buffer index
					//string tempString2 = "";
					//string tempString3="";

					while ( endPart != "TSP>\n" && endPart != ">>>>\n" && endPart != "TSP?\n" )
					{
						int read = _socket.Receive( _readBuffer );
						//tempString2 = Encoding.ASCII.GetString( _readBuffer1, 0, readingLength );

						for ( int i = 0; i < read; i++ )
							_byteBuffer.Add( _readBuffer[i] );

						bufferInd += read;

						//tempString3 = Encoding.ASCII.GetString( _readBuffer2.ToArray(), 0, _readBuffer2.Count );
						tempString = Encoding.ASCII.GetString( _byteBuffer.ToArray(), 0, bufferInd );

						endPart = tempString.Substring( tempString.Length - 5, 5 );
					}

					result = tempString;
				}

				return true;
			}
			catch (SocketException e)
			{
                this._lastErrorNum = e;
                this._lastErrorStr = e.Message;
                Console.WriteLine("[LANConnect],WaitAndGetData Err:" + _lastErrorStr);
				return false;
			}
		}

        public bool WaitAndGetData(out string result, int dataLength)
        {
            result = string.Empty;

            if (this._socket == null)
            {
                return false;
            }

            try
            {
                if (!this._isPrompt)
                {
                    int read = 0;

                    string data = string.Empty;

                    StringBuilder sb = new StringBuilder(this._socket.ReceiveBufferSize * 3 / 2);

                    while ((read = _socket.Receive(_readBuffer)) > 0)
                    {
                        data += Encoding.ASCII.GetString(_readBuffer, 0, read);

                        if (data.Split('\n').Length > dataLength)
                        {
                            break;
                        }

                        //sb.Append(Encoding.ASCII.GetString(_readBuffer, 0, read));

                        Thread.Sleep(0);
                    }

                    result = data;

                    sb = null;
                }
                else
                {
                    string tempString = "initialization"; // initialize the variable "reading" in order to enter the follwing while loop
                    string endPart = tempString.Substring(tempString.Length - 5, 5);
                    int bufferInd = 0; // initialize the buffer index
                    //string tempString2 = "";
                    //string tempString3="";

                    while (endPart != "TSP>\n" && endPart != ">>>>\n" && endPart != "TSP?\n")
                    {
                        int read = _socket.Receive(_readBuffer);
                        //tempString2 = Encoding.ASCII.GetString( _readBuffer1, 0, readingLength );

                        for (int i = 0; i < read; i++)
                            _byteBuffer.Add(_readBuffer[i]);

                        bufferInd += read;

                        //tempString3 = Encoding.ASCII.GetString( _readBuffer2.ToArray(), 0, _readBuffer2.Count );
                        tempString = Encoding.ASCII.GetString(_byteBuffer.ToArray(), 0, bufferInd);

                        endPart = tempString.Substring(tempString.Length - 5, 5);
                    }

                    result = tempString;
                }

                return true;
            }
            catch (SocketException e)
            {
                this._lastErrorNum = e;

                this._lastErrorStr = e.Message;

                return false;
            }
        }

		public void Close()
		{
            if (this._socket != null)
            {
                this._socket.Close();
            }
		}

		#endregion
	}
}