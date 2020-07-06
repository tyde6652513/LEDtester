using System;
using System.Collections;
using System.Text;
using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections.Generic;

using MPI.Comm.TSECommand;

namespace MPI.Tester.TestServer
{
	public class ClientRole: IDisposable
	{
        //const int MAX_COMMAND_LENGTH = 2048;
        //const int HEADER_LEN = 1;
        //const int COMMAND_ID_LEN = 4;
        //const int LENGTH_POS = 5;
        //const int MINIMUM_PACKET_LENGTH = 9


        public const int MINIMUM_PACKET_LENGTH = 9;
		public const byte HEADER = 0x07;

		// header
		public const int HEADER_POS = 0;
		public const int HEADER_LEN = 1;
		// command id
		public const int COMMAND_ID_POS = HEADER_POS + HEADER_LEN;
		public const int COMMAND_ID_LEN = 4;
		// length
		public const int LENGTH_POS = COMMAND_ID_POS + COMMAND_ID_LEN;
		public const int LENGTH_LEN = 4;
		// data
		public const int DATA_POS = LENGTH_POS + LENGTH_LEN;
		public const int DATA_LEN = 2048;

        private const int MAX_RECEIVE_BUFFER_TIMES = 10;

		private System.Net.Sockets.TcpClient xMailManOut;
		private bool isDisposed;

		static private AsyncCallback fOnData;
		private ArrayList txtLog;
		private int _lastErrorNum;
		private string _lastErrorStr;

        //若封包起頭並非識別碼 0x07時，會先搜尋0x07作為真正的封包起頭
        //但此方法無法避免在真正的起頭前現其他的0x07，因此加上確認 MPIDS7600Packet.Command是否在某個區間內的機制
        private List<int> _cmdIDList = null;
        private int _bufferExpandTimes = 0;

		public event EventHandler<TCPClientReceiveEventArgs> TCPClientReceiveEvent;
		public ETCPClientState _lastState;

		public delegate void StateChangeHandler(ETCPClientState state);
		public StateChangeHandler StateChangeEvent;

        protected MPI.PerformanceTimer _pt1;


        //public ClientRole(ArrayList log)
        //{
        //    this.xMailManOut = new TcpClient();
        //    fOnData = new AsyncCallback(this.notifyData);
        //    txtLog = log;
        //}

        public ClientRole(List<int> cmdList = null)
        {
            this.xMailManOut = new TcpClient();
            fOnData = new AsyncCallback(this.notifyData);

            this._lastErrorNum = 0;
            this._lastErrorStr = "NONE";
            this.StateChange(ETCPClientState.NONE);
            this._cmdIDList = (cmdList == null) ? null : new List<int>(cmdList.ToArray());
            this._bufferExpandTimes = 0;
        }

        #region >>> Public Property <<<

        public int LastErrorNum
        {
            get { return this._lastErrorNum; }
        }

        public string LastErrorStr
        {
            get { return this._lastErrorStr; }
        }

		public ETCPClientState LastState
		{
			get { return this._lastState; }
		}


        #endregion

        #region >>> Private Method <<<


        /// <summary>
        ///  Recevie call back function, receive data from TcpClient
        /// </summary>
        private void notifyData(IAsyncResult ar)
		{
            Console.WriteLine("[ClientRole],notifyData()");
            _pt1 = new PerformanceTimer();
            _pt1.Start();
			if (xMailManOut.Connected == false)
				return;

			int iread = 0;
			try
			{
				iread = xMailManOut.GetStream().EndRead(ar);
            }
            #region
            catch (IOException ioe)
			{
				this._lastErrorStr = "[EndRead(IOE) or HW disconnected ]" + ioe.Message.ToString();
                this.StateChange(ETCPClientState.ERROR);
                this._lastErrorNum = 1;
                return;
			}
			catch (Exception ex)
			{
                this._lastErrorStr = "[EndRead(E)]" + ex.Message.ToString();
                this.StateChange(ETCPClientState.ERROR);
                this._lastErrorNum = 1;
                return;
            }
            #endregion

            #region
            if (iread == 0)
			{
                xMailManOut.Close();
                isDisposed = true;

                this._lastErrorStr = "[EndRead] Server disconnect";
                this.StateChange(ETCPClientState.ERROR);
                this._lastErrorNum = 1;
                return;
			}

            if (this.xMailManOut.Connected == false)
            {
                this._lastErrorStr = "[EndRead] Server disconnect";
                this.StateChange(ETCPClientState.ERROR);
                this._lastErrorNum = 1;
                return;
            }
            #endregion

            this.StateChange(ETCPClientState.READED);                       

            byte[] packet = (byte[])ar.AsyncState; 

            int cmdID = -1;
            EClientErr cErr = EClientErr.NONE;
            int tarPackageLen = 0;
            try
            {
                int headerIndex = FindHeaderIndex(packet, out cmdID);

                if (headerIndex >= 0)// got header
                {
                    int length = GetDataLength(packet, headerIndex);
                    tarPackageLen = length + MINIMUM_PACKET_LENGTH;
                    if (packet.Length >= tarPackageLen + headerIndex)
                    {
                        byte[] outpacket = ModifyBuffer(packet, headerIndex, tarPackageLen);

                        _pt1.Stop();
                        //double timeLen = _pt1.GetTimeSpan(ETimeSpanUnit.MilliSecond);
                        int packLen = outpacket.Length;
                        
                        this.beginWait();
                        int delayTime =0;

                        delayTime = 5;  
                         
                        TriggeReciveEvent(outpacket,delayTime);
                        
                    }
                    else
                    {
                        xMailManOut.GetStream().ReadTimeout = 5000;//5秒 timeout
                        List<byte> bList = new List<byte>();
                        bList.AddRange(packet);
                        for (int times = 0; times < MAX_RECEIVE_BUFFER_TIMES; ++times)
                        {
                            byte[] bytes = new byte[tarPackageLen];
                            int len = xMailManOut.GetStream().Read(bytes, 0, tarPackageLen);
                            for (int i = 0; i < len; ++i)
                            {
                                bList.Add(bytes[i]);
                            }
                            packet = bList.ToArray();
                            if (bList.Count == tarPackageLen)
                            {
                                _pt1.Stop();
                                double timeLen = _pt1.GetTimeSpan(ETimeSpanUnit.MilliSecond);
                                int packLen = packet.Length;
                                Console.WriteLine("[ClientRole],time test,len:" + packLen.ToString() + "," + timeLen.ToString());
                                xMailManOut.GetStream().ReadTimeout = System.Threading.Timeout.Infinite;
                                this.beginWait();
                                TriggeReciveEvent(packet);
                                xMailManOut.GetStream().Flush();
                                
                                break;
                            }
                        }
                    }
                }
                else
                {
                    cErr = (EClientErr)(-headerIndex);
                }
            }
            catch (IOException ioe)
            {
                int gotLen = 0;
                if(packet != null)
                {gotLen = packet.Length;}
                this._lastErrorStr = "[ClientRole],notifyData(),Err" + ioe.Message.ToString() + ",Expect Length is " + tarPackageLen.ToString() + ",got buffer length is " + gotLen.ToString();;
                Console.WriteLine(this._lastErrorStr);
                this.StateChange(ETCPClientState.ERROR);
                cErr = EClientErr.TIME_OUT_IN_READ_2ND_BUFFER;                
            }
            catch (Exception ex)
            {
                this._lastErrorStr = "[ClientRole],notifyData(),Err" + ex.Message.ToString();
                this.StateChange(ETCPClientState.ERROR);
                cErr = EClientErr.NOT_DEFINED_ERR;                
            }

            if (cErr != EClientErr.NONE)
            {
                packet = CreateErrBuffer(cErr, packet, cmdID);
                TriggeReciveEvent(packet);
                this.beginWait();
            }


            packet = null;
		}

        private byte[] CreateErrBuffer( EClientErr eErr,byte[] buffer,int cmdId = 0)
        {
            byte[] outBuffer = null;
            int len = 0;
            if (buffer != null)
            { len = buffer.Length; }
            switch (eErr)
            {
                
                case EClientErr.NOT_DEFINED_ERR:
                case EClientErr.PACKAGE_HEADER_NOT_FOUND:
                case EClientErr.TIME_OUT_IN_READ_2ND_BUFFER:
                case EClientErr.CMD_ID_NOT_DEFINDED:
                    #region
                    {
                        List<byte> List = new List<byte>();
                        List.Add((byte)eErr);
                        List.AddRange(BitConverter.GetBytes(cmdId));
                        List.AddRange(BitConverter.GetBytes(len));
                        
                        outBuffer = List.ToArray();
                    }
                    break;
                    #endregion
                default:
                    outBuffer = buffer;
                    break;
            }
            return outBuffer;
        }


		private void echoCommand(byte[] packet)
		{
			this.SendMessage(packet);
		}

		/// <summary>
		/// Begin recevie function by network stream of TcpClient
		/// </summary>
        private void beginWait()
		{
            byte[] echo = new byte[DATA_LEN];
			xMailManOut.GetStream().BeginRead(echo, 0, echo.Length, fOnData, echo);
            this.StateChange(ETCPClientState.READING);
		}

        /// <summary>
        ///  Connect call back function
        /// </summary>
        private void ConnectCallBack(IAsyncResult ar)
        {
            try
            {
                xMailManOut.EndConnect(ar);
            }
            catch (SocketException se)
            {
                this._lastErrorStr = "[Connected] Error accepting socket :" + se.Message;
                this.StateChange(ETCPClientState.ERROR);
                this._lastErrorNum = 1;
                return;
            }

            this.StateChange(ETCPClientState.CONNECTED);
            this.beginWait();
            isDisposed = false;
        }

		private void StateChange(ETCPClientState state)
		{
			if (StateChangeEvent != null)
			{
				StateChangeEvent(state);
			}

			this._lastState = state;
		}

        private void TriggeReciveEvent(byte[] outpacket,int delayInms = 0)
        {
            EventHandler<TCPClientReceiveEventArgs> handlerInstance = TCPClientReceiveEvent;
            if (handlerInstance != null)
            {
                System.Threading.Thread.Sleep(delayInms);
                xMailManOut.GetStream().ReadTimeout = System.Threading.Timeout.Infinite;
                TCPClientReceiveEventArgs theArg = new TCPClientReceiveEventArgs(outpacket);
                handlerInstance(this, theArg);
            }
        }
        /// <summary>
        /// Find header of MPIDS7600Packet
        /// This method check:
        /// 1.index of 0x07 
        /// 2. MPIDS7600Packet.Command in _cmdIDList
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        private int FindHeaderIndex(byte[] buff,out int cmdID)
        {
            cmdID = -1;
            if (buff != null)
            {
                int headerLength = HEADER_LEN + COMMAND_ID_LEN;
                for (int i = 0; i < buff.Length - headerLength; ++i)
                {
                    if (buff[i] == 0x07)
                    {
                        int cmdid = GetCmdID(buff, i + HEADER_LEN);
                        if (CheckIFCMDInCheckList(cmdid)) // cmdid在 _cmdIDList列表 或 _cmdIDList列表不存在
                        {
                            cmdID = cmdid;
                            return i;
                        }
                        else if (cmdID == -1)//若第一次搜尋到0x07，但cmdid不在_cmdIDList列表 =>可能是收到的命令不在現有的命令封包種類中
                        {
                            cmdID = cmdid;
                        }
                    }
                }
            }
            if (cmdID != -1)
            {
                return -(int)EClientErr.CMD_ID_NOT_DEFINDED;
            }
            else
            { return -(int)EClientErr.PACKAGE_HEADER_NOT_FOUND; }
        }

        private int GetDataLength(byte[] buff, int headerPos = -1)
        {

            if (buff != null)
            {
                int cmd = -1;
                if (headerPos < 0)
                { headerPos = FindHeaderIndex(buff, out cmd); }
                int lengthPos = headerPos + HEADER_LEN + COMMAND_ID_LEN;

                if (headerPos >= 0 ) // find header Pos
                {
                    if (buff.Length >= lengthPos + LENGTH_LEN)
                    {
                        int dataLength = BitConverter.ToInt32(buff, lengthPos);
                        return dataLength;
                    }
                }
            }

            return 0;
        }

        private int GetCmdID(byte[] buff,int startPos)
        {
            int cmdID = -1;
            return cmdID = BitConverter.ToInt32(buff, startPos);         
        }

        private bool CheckIFCMDInCheckList(int cmdID)
        {
            if (this._cmdIDList != null)
            {
                return _cmdIDList.Contains(cmdID);
            }
            return true;
        }

        private byte[] ModifyBuffer(byte[] buff, int startPos, int tarPackageLen)
        {
            if(buff != null)
            {
                if (startPos == 0 && tarPackageLen == buff.Length)
                {
                    return buff;
                }
                else
                {
                    byte[] bArr = new byte[tarPackageLen];
                    Array.Copy(buff, startPos, bArr,0, tarPackageLen);
                    return bArr;                    
                }
            }
            return null;
            
        }

        //private bool CheckDataLengthCorrect(byte[] buff, int headerPos = -1)
        //{
        //    if (buff != null)
        //    {
        //        int cmd = -1;
        //        if (headerPos < 0)
        //        { headerPos = FindHeaderIndex(buff, out cmd); }
        //        int lengthPos = headerPos + HEADER_LEN + COMMAND_ID_LEN;

        //        if (headerPos != -1) // find header Pos
        //        {
        //            if (buff.Length >= lengthPos + LENGTH_LEN)
        //            {
        //                int dataLength = GetDataLength(buff, headerPos);

        //                int packetLength = MINIMUM_PACKET_LENGTH + dataLength;

        //                if (buff.Length >= packetLength + headerPos)	// It menas that intput buffer is not null.
        //                {
        //                    return true;
        //                }
        //            }
        //        }
        //    }
        //    return false;
 
        //}
        #endregion

        #region >>> Public Method <<<

        /// <summary>
        /// TCP/IP Client Begin Connet to Server
        /// </summary>
        /// <param name="ipaddress">IP Address</param>
        /// <param name="port">Net Port Number</param>
        /// <returns></returns>
        public bool BeginConnect(string ipaddress, int port)
		{
			if (isDisposed)
			{
				xMailManOut = null;
				xMailManOut = new TcpClient();
			}

			try
			{
                this._lastErrorStr = "NONE";
                this.StateChange(ETCPClientState.CONNECTING);
                this._lastErrorNum = 0;
                xMailManOut.BeginConnect(ipaddress, port, new AsyncCallback(ConnectCallBack), null);
			}
			catch (ObjectDisposedException)
			{
                this._lastErrorStr = "[Connecting] TcpClient is closed";
                this.StateChange(ETCPClientState.ERROR);
                this._lastErrorNum = 1;
				return false;
			}
			catch (SocketException se)
			{
                this._lastErrorStr = "[Connecting] Error accepting socket :" + se.Message;
                this.StateChange(ETCPClientState.ERROR);
                this._lastErrorNum = 1;
				xMailManOut.Close();
				isDisposed = true;
				this._lastState = ETCPClientState.NONE;
				return false;
			}
			catch (Exception)
			{
                this._lastErrorStr = "[Connecting] Exception ";
                this.StateChange(ETCPClientState.ERROR);
                this._lastErrorNum = 1;
				return false;
			}

			return true;
		}

		public void Disconnect()
		{
			if (xMailManOut != null && xMailManOut.Connected)
			{
				xMailManOut.Close();
				isDisposed = true;
				this._lastState = ETCPClientState.NONE;
			}
		}

        /// <summary>
        /// TCP/IP Client side send byte[] packet
        /// </summary>
        /// <param name="data"></param>
		public void SendMessage(byte[] data)
		{
			if (this.xMailManOut == null || this.xMailManOut.Connected == false)
				return;

            if (data != null)
            {
                this.StateChange(ETCPClientState.SENDING);
                xMailManOut.GetStream().Write(data, 0, data.Length);
                this.StateChange(ETCPClientState.SENDED);

                if (xMailManOut.GetStream().DataAvailable)
                {
                    Console.WriteLine("[ClientRole],DataAvailable!!!!!!!!!!!!");
                }
            }
		}

		public void logClient(string msg)
		{
			if ( this.txtLog == null )
                return;
            
            lock (txtLog.SyncRoot)
			{
				txtLog.Add(string.Format("[Client] {0}\r\n", msg)); 
			}
		}

        public bool CheckIfErrBuffer(byte[] data,out string outStr)
        {
            outStr = "";
            if (data == null )
            {
                outStr = "Buffer Is Empty!";
                return true;
            }
            else if (data.Length > 1 && data[0] == (byte)EClientErr.NONE)
            {
                return false;
            }
            else if (data.Length < 9)
            {
                outStr = "Buffer too short!";
                return true;
            }
            else
            {
                EClientErr eErr = (EClientErr)data[0];
                switch (eErr)
                {
                    case EClientErr.NOT_DEFINED_ERR:
                    case EClientErr.PACKAGE_HEADER_NOT_FOUND:
                    case EClientErr.TIME_OUT_IN_READ_2ND_BUFFER:
                    case EClientErr.CMD_ID_NOT_DEFINDED:
                        #region
                        {
                            int cmdID = BitConverter.ToInt32(data, 1);
                            int len = BitConverter.ToInt32(data, 5);

                            outStr += "Error: " + eErr.ToString() + ",CMD_ID: " + cmdID.ToString() + ",Buffer_Length: " + len.ToString();
                            return true;
                        }
                        break;
                        #endregion
                    default:
                        return false;
                        break;
                }
            }

            return false;
        }


		public void Dispose()
		{
			if (xMailManOut != null && xMailManOut.Connected)
				xMailManOut.Close();
        }

        #endregion

    }


    public class TCPClientReceiveEventArgs : EventArgs
    {
        private byte[] _data;

        public TCPClientReceiveEventArgs(byte[] data)
        {
            this._data = data;
        }

        public byte[] Data
        {
            get { return this._data; }
            //set { this._data = value; }
        }

    }

    public enum EClientErr:byte
    {
        
        TIME_OUT_IN_READ_2ND_BUFFER = 1,
        CMD_ID_NOT_DEFINDED = 2,
        PACKAGE_HEADER_NOT_FOUND = 3,
        NOT_DEFINED_ERR = 4,

        NONE = 7,// 0x07

    }
}