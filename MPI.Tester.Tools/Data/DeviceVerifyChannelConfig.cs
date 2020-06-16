using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Tools
{
    [Serializable]
    public class DeviceVerifyChannelConfig : ICloneable
    {
        private object _lockObj;

        private List<DeviceVerifyChannelData> _dataList;

        public DeviceVerifyChannelConfig()
        {
            this._lockObj = new object();

            this._dataList = new List<DeviceVerifyChannelData>();
        }

        #region >>> Public Property <<<

        public DeviceVerifyChannelData this[int channel]
        {
            get
            {
                if (this._dataList == null || this._dataList.Count == 0)
                {
                    return null;
                }

                return this._dataList[channel];
            }
        }

        public List<DeviceVerifyChannelData> Data
        {
            get { return this._dataList; }
            set { lock (this._lockObj) { this._dataList = value; } }
        }

        public int Count
        {
            get { return this._dataList.Count; }
        }

        public int STDChannel
        {
            get
            {
                if (this._dataList == null || this._dataList.Count == 0)
                {
                    return -1;
                }
                
                int channel = 0;

                foreach (DeviceVerifyChannelData data in this._dataList)
                {
                    if (data.IsEnable && data.IsStdCheck)
                    {
                        return channel;
                    }

                    channel++;
                }

                return -1;
            }
        }

        public int[] VerifyChannel
        {
            get
            {
                if (this._dataList == null || this._dataList.Count == 0)
                {
                    return null;
                }

                List<int> lstChannel = new List<int>();

                int channel = 0;

                foreach (DeviceVerifyChannelData data in this._dataList)
                {
                    if (data.IsEnable && data.IsMsrtCheck)
                    {
                        lstChannel.Add(channel);
                    }

                    channel++;
                }

                if (lstChannel.Count != 0)
                {
                    return lstChannel.ToArray();
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        public object Clone()
        {
            DeviceVerifyChannelConfig cloneObj = this.MemberwiseClone() as DeviceVerifyChannelConfig;

            cloneObj._dataList = new List<DeviceVerifyChannelData>();

            foreach (DeviceVerifyChannelData data in this._dataList)
            {
                cloneObj._dataList.Add(data.Clone() as DeviceVerifyChannelData);
            }

            return cloneObj;
        }
    }

    public class DeviceVerifyChannelData : ICloneable
    {
        private object _lockObj;
        private bool _isEnable;
        private bool _isStdCheck;
        private bool _isMsrtCheck;
        private string _descript;

        public DeviceVerifyChannelData()
        {
            this._lockObj = new object();
            this._isEnable = true;
            this._isStdCheck = false;
            this._isMsrtCheck = false;
            this._descript = string.Empty;
        }

        #region >>> Public Property <<<

        public bool IsEnable
        {
            get { return this._isEnable; }
            set { lock (this._lockObj) { this._isEnable = value; } }
        }

        public bool IsStdCheck
        {
            get { return this._isStdCheck; }
            set { lock (this._lockObj) { this._isStdCheck = value; } }
        }

        public bool IsMsrtCheck
        {
            get { return this._isMsrtCheck; }
            set { lock (this._lockObj) { this._isMsrtCheck = value; } }
        }

        public string Description
        {
            get { return this._descript; }
            set { lock (this._lockObj) { this._descript = value; } }
        }

        #endregion

        public object Clone()
        {
            return this.MemberwiseClone() as DeviceVerifyChannelData;
        }

    }
}
