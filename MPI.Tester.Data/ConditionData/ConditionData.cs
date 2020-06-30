using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using Newtonsoft.Json;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
	[Serializable]
	public class ConditionData  : ICloneable
	{
		private object _lockObj;

		private EPolarity _chipPolarity;
		private ECalMode _calMode;
		private ECalBaseWave _calByWLType;
		private ECalLookupWave _calLookupWave;
		private ESensingMode _sensingMode;
        private ETestStage _testStage;

		      
		private TestItemData[] _testItemArray;
		private bool _isUseDefinedName;
		private IFTestItem _openShortIFTestItem;
        private string _wm617GainOffsetList;

        private ChannelConditionTable _channelCondiTable;

        private int _numberOfEnableTestItem;

		/// <summary>
		///  Constructor
		/// </summary>
		public ConditionData()
		{
			this._lockObj = new object();

			this._chipPolarity = EPolarity.Anode_P;
			this._calMode = ECalMode.GainOffset;
			this._calByWLType = ECalBaseWave.By_WLD;
			this._calLookupWave = ECalLookupWave.Original;
			this._sensingMode = ESensingMode.Limit;

			this._isUseDefinedName = true;
			this._openShortIFTestItem = new IFTestItem();
            this._wm617GainOffsetList = "0,1,0,0,1,0,0,1,0," +
                                        "0,0,0,0,0,0,0," +
                                        "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0";
            // LOP Sqare, Gain, Offset x 3 = 9 items
            // Offset of VF1, VF2, VF3, VF4, VZ1, VZ2, IR = 7 items
            // Offset of WLP, WLD, WLC x 3 = 9 items
            // Offset of x, x x 3 = 6 items

            this._channelCondiTable = new ChannelConditionTable();

            this._numberOfEnableTestItem = 0;
            _testStage = ETestStage.IV;

		}

		#region >>> Public Property <<<

		public EPolarity ChipPolarity
		{
			get { return this._chipPolarity; }
			set { lock (this._lockObj) { this._chipPolarity = value; } }
		}

		public ECalMode CalMode
		{
			get { return this._calMode; }
			set { lock (this._lockObj) { this._calMode = value; } }
		}

		public ECalBaseWave CalByWave
		{
			get { return this._calByWLType; }
			set { lock (this._lockObj) {this._calByWLType = value; } }
		}

		public ECalLookupWave CalLookupWave
		{
			get { return this._calLookupWave; }
			set { lock (this._lockObj) {this._calLookupWave = value; } }
		}

		public ESensingMode SensingMode
		{
			get { return this._sensingMode; }
			set { lock (this._lockObj) { this._sensingMode = value; } }
		}
        public ETestStage TestStage
        {
            get { return this._testStage; }
            set { lock (this._lockObj) { this._testStage = value; } }
        }
        
		//[System.Xml.Serialization.XmlIgnore]
		[XmlArrayItem(typeof(TestItemData)),
		XmlArrayItem(typeof(IFHTestItem)), 
		XmlArrayItem(typeof(IFTestItem)), 
		XmlArrayItem(typeof(IZTestItem)),
		XmlArrayItem(typeof(VFTestItem)), 
		XmlArrayItem(typeof(VRTestItem)),
		XmlArrayItem(typeof(DVFTestItem)),
        XmlArrayItem(typeof(IVSweepTestItem)), 
		XmlArrayItem(typeof(VISweepTestItem)),
		XmlArrayItem(typeof(LOPTestItem)), 
		XmlArrayItem(typeof(LOPWLTestItem)),
		XmlArrayItem(typeof(ESDTestItem)),
        XmlArrayItem(typeof(THYTestItem)), 
		XmlArrayItem(typeof(CALCTestItem)), 
		XmlArrayItem(typeof(DIBTestItem)),
		XmlArrayItem(typeof(VacTestItem)),
		XmlArrayItem(typeof(PolarTestItem)),
        XmlArrayItem(typeof(RTestItem)),
		XmlArrayItem(typeof(RTHTestItem)),
		XmlArrayItem(typeof(LIVTestItem)),
        XmlArrayItem(typeof(VIScanTestItem)),
        XmlArrayItem(typeof(PIVTestItem)),
        XmlArrayItem(typeof(TransistorTestItem)),
		XmlArrayItem(typeof(LCRTestItem)),
        XmlArrayItem(typeof(LCRSweepTestItem)),
		XmlArrayItem(typeof(OSATestItem)),
		XmlArrayItem(typeof(ContactCheckTestItem)),
        XmlArrayItem(typeof(IOTestItem)),
        XmlArrayItem(typeof(LaserSourceTestItem))]
        
		public TestItemData[] TestItemArray
		{
			get { return this._testItemArray; }
			set { lock (this._lockObj) { this._testItemArray = value; } }
		}

		public bool IsUseDefinedName
		{
			get { return this._isUseDefinedName; }
			set { lock (this._lockObj) { this._isUseDefinedName = value; } }
		}

		public IFTestItem OpenShortIFTestItem
		{
			get { return this._openShortIFTestItem; }
			set { lock (this._lockObj) { this._openShortIFTestItem = value; } }		
		}

        public string WM617GainOffsetList
        {
            get { return this._wm617GainOffsetList; }
            set { lock (this._lockObj) { this._wm617GainOffsetList = value; } }
        }

        public ChannelConditionTable ChannelConditionTable
        {
            get { return this._channelCondiTable; }
            set { lock (this._lockObj) { this._channelCondiTable = value; } }
        }


		#endregion

        #region >>> Public Method <<<
        public virtual List<Dictionary<string, object>> GetItemInfoList()
        {
            List<Dictionary<string, object>> itemInfoList = new List<Dictionary<string, object>>();
            if (TestItemArray != null && TestItemArray.Length > 0)
            {
                for (int i = 0; i < TestItemArray.Length; ++i)
                {
                    itemInfoList.Add(TestItemArray[i].GetItemInfo());
                }
            }

            return itemInfoList;
        }

        public TestItemData FindTestItemByMsrtKey(string KeyName)
        {            
            foreach (var tData in TestItemArray)
            {
                foreach (var rData in tData.MsrtResult)
                {
                    if (KeyName == rData.KeyName)
                    {
                        //bR = rData.BoundaryRule;
                        return tData;
                    }
                }
            }
            return null;
        }

        public TestResultData FindMsrtDataFromMsrtKey(string KeyName)
        {
           
            foreach (var tData in TestItemArray)
            {
                foreach (var rData in tData.MsrtResult)
                {
                    if (KeyName == rData.KeyName)
                    {
                        return rData;
                    }
                }
            }
            return null;
        }

        public object Clone()
		{
			MemoryStream ms = new MemoryStream();
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(ms, this);
			ms.Position = 0;
			object obj = bf.Deserialize(ms);
			ms.Close();

			return obj;
		}
		
		#endregion
	}


}
