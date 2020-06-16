using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
    [Serializable]
    public class ChannelAssignmentData
    {
        protected object _lockObj;

        protected string _srcModel;
        protected string _srcChannel;
        protected string _srcAddress;
        protected string _srcComPort;
        protected string _srcSerialNum;

        protected string _switchModel;
        protected int _switchSlot;
        protected uint _switchCh;
        protected string _switchAddress;

        protected short _esdIOCardNumber;
        protected uint _esdADCardChannel;

        public ChannelAssignmentData()
        {
            this._lockObj = new object();
            
            this._switchSlot = 0;     // 0-base
            this._switchCh = 0;      // 0-base
            this._srcModel = "K2600";
            this._srcChannel = "A";
            this._switchModel = "NONE";
            this._srcAddress = "192.168.50.2";
            this._srcComPort = "COM8";
            this._srcSerialNum = "";

            this._switchAddress = "192.168.50.20";

            this._esdIOCardNumber = 0;
            this._esdADCardChannel = 0;
        }

        #region >>> Public Property <<<

        public string SourceModel
        {
            get { return this._srcModel; }
            set { lock (this._lockObj) { this._srcModel = value; } }
        }

        public string SwitchModel
        {
            get { return this._switchModel; }
            set { lock (this._lockObj) { this._switchModel = value; } }
        }

        public string SwitchAddress
        {
            get { return this._switchAddress; }
            set { lock (this._lockObj) { this._switchAddress = value; } }
        }

        public string SourceCH
        {
            get { return this._srcChannel; }
            set { lock (this._lockObj) { this._srcChannel = value; } }
        }

        public int SwitchSlot
        {
            get { return this._switchSlot; }
            set { lock (this._lockObj) { this._switchSlot = value; } }
        }

        public uint SwtichChannel
        {
            get { return this._switchCh; }
            set { lock (this._lockObj) { this._switchCh = value; } }
        }

        public string DeviceIpAddress
        {
            get { return this._srcAddress; }
            set { lock (this._lockObj) { this._srcAddress = value; } }
        }

        public string DeviceComPort
        {
            get { return this._srcComPort; }
            set { lock (this._lockObj) { this._srcComPort = value; } }
        }

        public short ESDIOCardNumber
        {
            get { return this._esdIOCardNumber; }
            set { lock (this._lockObj) { this._esdIOCardNumber = value; } }
        }

        public uint ESDADCardChannel
        {
            get { return this._esdADCardChannel; }
            set { lock (this._lockObj) { this._esdADCardChannel = value; } }
        }

        public string DeviceSerialNum
        {
            get { return this._srcSerialNum; }
            set { lock (this._lockObj) { this._srcSerialNum = value; } }
        }

        #endregion

        #region >>> Public Method <<<

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
