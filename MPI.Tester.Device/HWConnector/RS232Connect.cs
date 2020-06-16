using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Device
{
    public class RS232Connect : IConnect
    {
        private SerialPort _sp;
        private string _readBuffer;
        private RS232SettingData _setting;
		//private bool _isQueried;

        private string _lastErrorStr;

        public RS232Connect()
        {
            this._sp = new SerialPort();
            this._setting = new RS232SettingData();
            this._readBuffer = "";
			//this._isQueried = false;
            this._lastErrorStr = "NONE";
        }

        public RS232Connect(RS232SettingData setting) : this()
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
            get { return "NONE"; }
        }

        public string BufferData
        {
            get { return this._readBuffer; }
        }
        #endregion

        #region >>> Private Method <<<

        #endregion 

        #region >>> Public Method <<<

        public bool Open(out string info)
        {
            Parity parity = Parity.None;
            info = "";

            switch (this._setting.Parity)
            {
                case "NONE":
                    parity = Parity.None;
                    break;
                case "ODD":
                    parity = Parity.Odd;
                    break;
                case "EVEN":
                    parity = Parity.Even;
                    break;
                case "MARK":
                    parity = Parity.Mark;
                    break;
                case "SPACE":
                    parity = Parity.Space;
                    break;
                default:
                    parity = Parity.None;
                    break;
            }

            this._sp.PortName = this._setting.ComPortName;
            this._sp.BaudRate = this._setting.BaudRate;
            this._sp.DataBits = this._setting.DataBits;
            this._sp.StopBits = StopBits.One;
            this._sp.ReadTimeout = this._setting.TimeOut;
            this._sp.Parity = parity;

            try
            {
                if (this._sp.IsOpen == false)
                {
                    this._sp.Open();
                }
                else
                {
                    this.Close();
                    this._sp.Open();
                }
            }
            catch (Exception ex)
            {
                this._lastErrorStr = ex.Message;
                info = ex.Message;
                return false;
            }
            finally
            { }

            return true;
        }

        public bool SendCommand(string command)
        {
            try
            {
                this._sp.Write(command + this._setting.Terminator);
            }
            catch (Exception ex)
            {
                this._lastErrorStr = ex.Message;
            }
            return true;
        }

        public bool QueryCommand(string command)
        {
            return true;
        }

        public bool WaitAndGetData(out string result)
        {
            int TrimEnd = 10; // ASCII : "\n" 
            result = "";
         
            try
            {
                int tempValue = new int();  //temporary Value
                string tempString = "";
                do
                {
                    //ReadByte:Synchronously reads one byte from the SerialPort input buffer.
                    tempValue = Convert.ToInt32(this._sp.ReadByte());

                    //Decimal conversion ASCII
                    tempString += Convert.ToString((char)tempValue);

                } while (tempValue != TrimEnd);

                result = tempString;
                return true;
            }
            catch (TimeoutException ex)
            {
                this._lastErrorStr = ex.Message;
                return false;
            }                    
        }

        public void Close()
        {
            this._sp.Close();
        }
        #endregion

    }
}
