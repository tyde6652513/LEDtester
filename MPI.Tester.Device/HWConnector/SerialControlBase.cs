using System;
using System.Collections.Generic;
using System.Text;

using System.IO.Ports;

namespace MPI.Tester.Device
{
	public class CommandRepliedEventArgs : EventArgs
	{
		private string _data;

		public CommandRepliedEventArgs(string data)
		{
			this._data = data;
		}

		public string Data
		{
			get { return this._data; }
		}
	}

    public abstract class SerialControlBase : IDisposable
    {
        protected StringBuilder _inputBuffer;
        protected SerialPort _port;
        protected char _TerminationChar;
        protected string _TerminationChars;

        public SerialControlBase()
        {
            this._port = new SerialPort();
            this._inputBuffer = new StringBuilder();
        }

        public event EventHandler<CommandRepliedEventArgs> CommandReplied;

        public void Close()
        {
            if (this._port != null)
            {
                this._port.Close();
            }
        }

        public virtual void Dispose()
        { 
        
        }

        public void Open(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits, char terminationChar, Encoding encoding)
        {
            this._port.PortName = portName;
            this._port.BaudRate = baudRate;
            this._port.Parity = parity;
            this._port.DataBits = dataBits;
            this._port.StopBits = stopBits;
            this._port.Encoding = encoding;

            if (this._port.IsOpen == false)
            {
                this._port.Open();
            }

            this._TerminationChar = terminationChar;
        }
		
        public virtual void SendCommand(string command)
        {
            this._inputBuffer = new StringBuilder();
            this._inputBuffer.Append(command);
            this._inputBuffer.Append(this._TerminationChar);
        }

    }

}
