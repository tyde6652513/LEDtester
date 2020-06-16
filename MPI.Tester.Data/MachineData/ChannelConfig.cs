using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
    [Serializable]
    public class ChannelConfig : ICloneable
    {
        #region >>> Private Property <<<

        private object _lockObj;
        private int _rowCnt;
        private int _colCnt;
        private int _slotCnt;
        private ETesterSequenceType _seqType;
        private List<ChannelAssignmentData> _assigmntTable;
        private int _channelCnt;

        #endregion

        #region >>> Constructor / Disposor <<<

        public ChannelConfig()
        {
            this._lockObj = new object();

            this._seqType = ETesterSequenceType.Parallel;

            this._rowCnt = 1;

            this._colCnt = 1;

            this._slotCnt = 1;

            this._assigmntTable = new List<ChannelAssignmentData>();
        }

        public ChannelConfig(int row, int col) : this()
        {
            this._assigmntTable.Clear();

            this._rowCnt = row;

            this._colCnt = col;
        }

        public ChannelConfig(int row, int col,int slot)
            : this( row,  col)
        {
            this._slotCnt = slot;
        }

        #endregion

        #region >>> Public Property <<<

        public ETesterSequenceType TesterSequenceType
        {
            get { return this._seqType; }
            set { lock (this._lockObj) { this._seqType = value; } }
        }

        public int RowYCount
        {
            get { return this._rowCnt; }
            set { lock (this._lockObj) { this._rowCnt = value; } }
        }

        public int ColXCount
        {
            get { return this._colCnt; }
            set { lock (this._lockObj) { this._colCnt = value; } }
        }

        public int ChannelCount
        {
            get
            {
                return this._assigmntTable.Count > 0 ? this._assigmntTable.Count : 1;
            }
        }

        public int SlotCount
        {
            get { return this._slotCnt; }
            set { lock (this._lockObj) { this._slotCnt = value; } }
            //get {
            //    List<int> slotNum = (from p in _assigmntTable
            //                         group p by p.SwitchSlot into g
            //                         select g.Key).ToList();
            //    return slotNum.Count;
            //}
        }

        public List<ChannelAssignmentData> AssignmentTable
        {
            get { return this._assigmntTable; }
            set { lock (this._lockObj) { this._assigmntTable = value; } }
        }

        #endregion

        #region >>> Public Method <<<

        public bool CheckIpAddress(string ipAddr)
        {
            foreach (ChannelAssignmentData data in this._assigmntTable)
            {
                // 檢查 IP Address 是否重複
                if (data.DeviceIpAddress == ipAddr)
                {
                    return false;
                }
            }

            return true;
        }

        public bool CheckDeviceChannel(int slot, int channel)
        {
            foreach (ChannelAssignmentData data in this._assigmntTable)
            {
                // 檢查 Device Channel 是否重複
                if (data.SwitchSlot == slot && data.SwtichChannel == channel)
                {
                    return false;
                }
            }

            return true;
        }

        public bool CheckDeviceComPort(string comPort)
        {
            foreach (ChannelAssignmentData data in this._assigmntTable)
            {
                // 檢查 Device Comport 是否重複
                if (data.DeviceComPort == comPort)
                {
                    return false;
                }
            }

            return true;
        }

        public bool CheckSMUChannel(string ip, string smuCh)
        {
            foreach (ChannelAssignmentData data in this._assigmntTable)
            {
                // 檢查 Device Channel 是否重複
                if (data.DeviceIpAddress == ip && data.SourceCH == smuCh)
                {
                    return false;
                }
            }
            return true;
        }
		public SlotInfo GetSlotinfo( int slotNum)
        {//SlotInfo
            SlotInfo sData = null;
            for (int i = 0; i < _assigmntTable.Count; ++i)
            {
                if (_assigmntTable[i].SwitchSlot == slotNum)
                {
                    if (sData == null)
                    {
                        sData = new SlotInfo(_assigmntTable[i]);
                    }

                    sData.DutSwitchDic.Add(i,_assigmntTable[i].SwtichChannel);
 
                }
 
            }
            return sData;
        }

        public List<int> GetDutChKListBySlot(int slot)
        {
            List<int> resutList = new List<int>();
            for (int i = 0; i < _assigmntTable.Count; ++i)
            {
                if (_assigmntTable[i].SwitchSlot == slot)
                {
                    resutList.Add(i);
                }
            }
            return resutList;
           // resutList =(from data in this._assigmntTable
           //where data.SwitchSlot == slot
           //select data.);
            
        }
        public object Clone()
        {
            ChannelConfig cloneObj = new ChannelConfig(this._rowCnt, this._colCnt);

            cloneObj._seqType = this._seqType;

            cloneObj._rowCnt = this._rowCnt;

            cloneObj._colCnt = this._colCnt;

            foreach (ChannelAssignmentData data in this._assigmntTable)
            {
                cloneObj._assigmntTable.Add(data.Clone() as ChannelAssignmentData);
            }

            return cloneObj;
        }

        #endregion
    }

    public class SlotInfo : ChannelAssignmentData
    {

        public Dictionary<int, uint> DutSwitchDic;
        //public string DutChStr;
        //public string SwitchChStr;

        public SlotInfo()
        {
            DutSwitchDic = new Dictionary<int, uint>();
            //DutChStr = string.Empty;
            //SwitchChStr = string.Empty;
        }
        public SlotInfo(ChannelAssignmentData chData):this()
        {
            //protected int _switchSlot;
             _srcModel = chData.SourceModel;
             _srcChannel = chData.SourceCH;
             _srcAddress = chData.DeviceIpAddress;
             _srcSerialNum = DeviceSerialNum;

             _switchModel = chData.SwitchModel;
             _switchSlot = chData.SwitchSlot;
             _switchAddress = chData.SwitchAddress;
             _esdIOCardNumber = chData.ESDIOCardNumber;
             _esdADCardChannel = chData.ESDADCardChannel;
        }

        public string DutChStr
        {
            get
            {
                string str = "";

                foreach (int ch in DutSwitchDic.Keys)//save as 0 base
                {
                    str += (ch +1).ToString() + ",";//show as 1 base
                }

                return str.TrimEnd(',');
            }
        }

        public string SwitchChStr
        {
            get
            {
                string str = "";

                foreach (KeyValuePair<int,uint> ch in DutSwitchDic)
                {
                    str += ch.Value.ToString() + ",";
                }

                return str.TrimEnd(',');
            }
        }
 
    }
}
