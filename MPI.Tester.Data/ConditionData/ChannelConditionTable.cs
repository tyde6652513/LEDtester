using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
    [Serializable]
    public class ChannelConditionTable : ICloneable
    {
        #region >>> Private Property <<<

        private object _lockObj;
        private bool _isEnable;
        private bool _isApplyByChannelCompensate;
        private int _rowCnt;
        private int _colCnt;
        private ChannelConditionData[] _dataArray;

        #endregion

        #region >>> Constructor / Disposor <<<

        public ChannelConditionTable()
        {
            this._lockObj = new object();

            this._rowCnt = 1;

            this._colCnt = 1;

            this._dataArray = new ChannelConditionData[this._rowCnt * this._colCnt];
        }

        public ChannelConditionTable(int colMax, int rowMax) : this()
        {
            this.RefreshChSetting(colMax, rowMax);
        }

        #endregion

        #region >>> Public Property <<<

        public bool IsEnable
        {
            get { return this._isEnable; }
            set { lock (this._lockObj) { this._isEnable = value; } }
        }

        public bool IsApplyByChannelCompensate
        {
            get { return this._isApplyByChannelCompensate; }
            set { lock (this._lockObj) { this._isApplyByChannelCompensate = value; } }
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

        public int Count
        {
            get { return this._rowCnt * this._colCnt; }
        }

        public ChannelConditionData[] Channels
        {
            get { return this._dataArray; }
            set { lock (this._lockObj) { this._dataArray = value; } }
        }

        public double[] GetItemEachChannelData(string keyName)
        {
            double[] rtnArray = new double[_dataArray.Length];

            for (int ch = 0; ch < this._dataArray.Length; ch++)
            {
                for (int itemIndex = 0;itemIndex < this._dataArray[ch].Conditions.Count; itemIndex ++)
                {
                    if (this._dataArray[ch].Conditions[itemIndex].MsrtResult != null)
                    {
                        for (int m = 0; m < this._dataArray[ch].Conditions[itemIndex].MsrtResult.Length; m++)
                    {
                        if (this._dataArray[ch].Conditions[itemIndex].MsrtResult[m].KeyName == keyName)
                        {
                            rtnArray[ch] = this._dataArray[ch].Conditions[itemIndex].MsrtResult[m].Value;
                            break;
                        }
                    }
                }
            }
            }

            return rtnArray;
        }

        #endregion

        #region >>> Public Method <<<

        public uint GetChannelByOrder(uint order)
        {
            uint channel = 0;

            foreach (var data in this._dataArray)
            {
                if (data.Order == order)
                {
                    return data.Channel;
                }
            }

            return channel;
        }

        public void RefreshChSetting(int colMax, int rowMax)
        {
            this._isEnable = colMax * rowMax == 1 ? false : true;   // Single-Die 不開啟Table功能

            this._isApplyByChannelCompensate = this._isEnable;

            this._rowCnt = rowMax;

            this._colCnt = colMax;

            this._dataArray = null;

            this._dataArray = new ChannelConditionData[this._rowCnt * this._colCnt];

            for (int i = 0; i < this._dataArray.Length; i++)
            {
                this._dataArray[i] = new ChannelConditionData();

                this._dataArray[i].Order = (uint)i;  // default order
            }
        }

        public void UpdateConditionTestItems(ETesterFunctionType type, TestItemData[] testItemArray)
        {
            if (type == ETesterFunctionType.Single_Die || type == ETesterFunctionType.Multi_Terminal)
            {
                return;
            }
            
            if (this._isEnable == false)
            {
                return;
            }

            if (testItemArray == null)
            {
                return;
            }

            if (testItemArray.Length == 0)
            {
                foreach (ChannelConditionData data in this._dataArray)
                {
                    data.Conditions.Clear();
                }

                return;
            }

            ChannelConditionData cloneCondi = new ChannelConditionData();

            uint electSettingOrder;

            for (uint channel = 0; channel < this._dataArray.Length; channel++)
            {
                cloneCondi.Conditions.Clear();

                cloneCondi.IsEnable = this._dataArray[channel].IsEnable;  // Ch的開起狀態

                cloneCondi.Channel = channel;

                cloneCondi.Order = this._dataArray[channel].Order;

                //=================================
                // 原本TestItemArray資料 Clone至 Channel Table
                // 原本TestItem產品的Gain也在此Copy下來
                //=================================
                electSettingOrder = 0;

                foreach (TestItemData cloneObj in testItemArray)
                {
                    if (cloneObj.ElecSetting != null)
                    {
                        foreach (ElectSettingData data in cloneObj.ElecSetting)
                        {
                            data.Order = electSettingOrder;

                            electSettingOrder++;
                        }
                    }

                    cloneCondi.Conditions.Add(cloneObj.Clone() as TestItemData);
                }

                for (int i = 0; i < testItemArray.Length; i++)
                {
                    TestItemData fromItemData = testItemArray[i];

                    TestItemData toItemData = cloneCondi.Conditions[i];

                    //-------------------------------------------------------------------------------------
                    // (1) Update TestItemData Array to ChannelConditionData
                    //-------------------------------------------------------------------------------------
                    switch (type)
                    {
                        case ETesterFunctionType.Multi_Die:
                            {
                                toItemData.IsUserSetEnable = fromItemData.IsEnable;

                                break;
                            }
                        case ETesterFunctionType.Multi_Pad:
                            {
                                if (this._dataArray[channel].ContainKey(fromItemData.KeyName))
                                {
                                    toItemData.IsUserSetEnable = this._dataArray[channel][fromItemData.KeyName].IsEnable;
                                }
                                else
                                {
                                    toItemData.IsUserSetEnable = fromItemData.IsEnable;
                                }
                                
                                break;
                            }
                    }

                    //-------------------------------------------------------------------------------------
                    // (2) Update TestResultItem Array to ChannelConditionItemData.GainOffset
                    //-------------------------------------------------------------------------------------
                    if (fromItemData.GainOffsetSetting != null)
                    {
                        if (fromItemData.GainOffsetSetting.Length != 0)
                        {
                            if (toItemData.ByChannelGainOffsetSetting == null || 
                                toItemData.ByChannelGainOffsetSetting.Length != fromItemData.GainOffsetSetting.Length)
                            {
                                toItemData.ByChannelGainOffsetSetting = new GainOffsetData[fromItemData.GainOffsetSetting.Length];
                            }

                            for (int j = 0; j < fromItemData.GainOffsetSetting.Length; j++)
                            {
                                GainOffsetData data = fromItemData.GainOffsetSetting[j];

                                toItemData.ByChannelGainOffsetSetting[j] = data.Clone() as GainOffsetData;

                                if (this._dataArray[channel].ContainsGainOffset(data.KeyName))
                                {
                                    toItemData.ByChannelGainOffsetSetting[j].Gain = this._dataArray[channel][fromItemData.KeyName].ByChannelGainOffsetSetting[j].Gain;

                                    toItemData.ByChannelGainOffsetSetting[j].Offset = this._dataArray[channel][fromItemData.KeyName].ByChannelGainOffsetSetting[j].Offset;

                                    toItemData.ByChannelGainOffsetSetting[j].Gain2 = 1;

                                    toItemData.ByChannelGainOffsetSetting[j].Offset2 = 0;

                                    toItemData.ByChannelGainOffsetSetting[j].Gain3 = 1;

                                    toItemData.ByChannelGainOffsetSetting[j].Offset3 = 0;
                                }
                                else
                                {
                                    toItemData.ByChannelGainOffsetSetting[j].Gain = 1.0d;

                                    toItemData.ByChannelGainOffsetSetting[j].Offset = 0.0d;

                                    toItemData.ByChannelGainOffsetSetting[j].Gain2 = 1;

                                    toItemData.ByChannelGainOffsetSetting[j].Offset2 = 0;

                                    toItemData.ByChannelGainOffsetSetting[j].Gain3 = 1;

                                    toItemData.ByChannelGainOffsetSetting[j].Offset3 = 0;
                                }
                            }
                        }
                    }
                }

                this._dataArray[channel].Update(cloneCondi.Clone() as ChannelConditionData);
            }

            cloneCondi = null;
        }

        public object Clone()
        {
            ChannelConditionTable cloneObj = new ChannelConditionTable();

            cloneObj._dataArray = new ChannelConditionData[this._rowCnt * this._colCnt];

            cloneObj._isEnable = this._isEnable;

            cloneObj._isApplyByChannelCompensate = this._isApplyByChannelCompensate;

            cloneObj._colCnt = this._colCnt;

            cloneObj._rowCnt = this._rowCnt;

            for (int i = 0; i < cloneObj._dataArray.Length; i++)
            {
                cloneObj._dataArray[i] = this._dataArray[i].Clone() as ChannelConditionData;
            }
     
            return cloneObj;
        }

        #endregion
    }
    
    [Serializable]
    public class ChannelConditionData : ICloneable
    {
        #region >>> Private Property <<<

        private object _lockObj;
        private bool _isEnable;
        private uint _order;
        private uint _channel;
        private List<TestItemData> _dataList;

        #endregion

        #region >>> Constructor / Disposor <<<

        public ChannelConditionData()
        {
            this._lockObj = new object();

            this._isEnable = true;

            this._order = 0;

            this._dataList = new List<TestItemData>();
        }

        #endregion

        #region >>> Public Property <<<

        public bool IsEnable
        {
            get { return this._isEnable; }
            set { lock (this._lockObj) { this._isEnable = value; } }
        }

        public uint Order
        {
            get { return this._order; }
            set { lock (this._lockObj) { this._order = value; } }
        }

        public uint Channel
        {
            get { return this._channel; }
            set { lock (this._lockObj) { this._channel = value; } }
        }

        public List<TestItemData> Conditions
        {
            get { return this._dataList; }
            set { lock (this._lockObj) { this._dataList = value; } }
        }

        public TestItemData this[string keyName]
        {
            get
            {
                if (this._dataList == null || this._dataList.Count == 0)
                {
                    return null;
                }

                foreach (TestItemData item in this._dataList)
                {
                    if (item.KeyName == keyName)
                    {
                        return item;
                    }
                }

                return null;
            }
        }

        #endregion

        #region >>> Public Method <<<

        public void Update(ChannelConditionData data)
        {
            this._isEnable = data.IsEnable;

            this._channel = data.Channel;

            this._order = data.Order;

            this._dataList.Clear();

            foreach (TestItemData item in data.Conditions)
            {
                this._dataList.Add(item);
            }
        }

        public bool ContainKey(string keyName)
        {
            if (this._dataList == null || this._dataList.Count == 0)
            {
                return false;
            }
            
            foreach (TestItemData item in this._dataList)
            {
                if (item.KeyName == keyName)
                {
                    return true;
                }
            }

            return false;
        }

        public bool ContainsGainOffset(string keyName)
        {
            if (this._dataList == null || this._dataList.Count == 0)
            {
                return false;
            }

            foreach (TestItemData item in this._dataList)
            {
                if (item.ByChannelGainOffsetSetting == null || item.ByChannelGainOffsetSetting.Length == 0)
                    continue;

                foreach (GainOffsetData data in item.ByChannelGainOffsetSetting)
                {
                    if (data.KeyName == keyName)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public GainOffsetData GetByChannelGainOffsetData(string keyName)
        {
            if (this._dataList == null || this._dataList.Count == 0)
            {
                return null;
            }

            foreach (TestItemData item in this._dataList)
            {
                if (item.ByChannelGainOffsetSetting == null || item.ByChannelGainOffsetSetting.Length == 0)
                    continue;

                foreach (GainOffsetData data in item.ByChannelGainOffsetSetting)
                {
                    if (data.KeyName == keyName)
                    {
                        return data;
                    }
                }
            }

            return null;
        }

        public object Clone()
        {
            ChannelConditionData cloneObj = this.MemberwiseClone() as ChannelConditionData;

            cloneObj._dataList = new List<TestItemData>();

            foreach (TestItemData data in this._dataList)
            {
                cloneObj._dataList.Add(data.Clone() as TestItemData);
            }

            return cloneObj;
        }

        #endregion
    }
}
