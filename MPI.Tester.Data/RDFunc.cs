using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

using System.Net.NetworkInformation;

using MPI.Tester;

namespace MPI.Tester.Data
{
    public class RDFunc
    {
        private RDFuncData _data;

        private MACAdressLocker _macData;

        public RDFunc()
        {
            this._data = new RDFuncData();

            this._macData = new MACAdressLocker();
        }

        #region >>> Public Property <<<

        public RDFuncData RDFuncData
        {
            get { return this._data; }
            set { this._data = value; }
        }

        #endregion

        #region >>> Public Method <<<

        public bool Save(string filePath)
        {
            bool rtn = true;

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();

                try
                {
                    bf.Serialize(fs, this._data);
                }
                catch (Exception e)
                {
                    Console.WriteLine("[RDFunc], Save(), Fail:" + e.ToString());

                    rtn = false;
                }
            }

            return rtn;
        }

        public bool SaveAs(string userID, string filePath)
        {
            bool rtn = true;

            RDFuncData cloneObj = this._data.Clone() as RDFuncData;

            cloneObj.UserID = userID;

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();

                try
                {
                    bf.Serialize(fs, cloneObj);
                }
                catch (Exception e)
                {
                    Console.WriteLine("[RDFunc], SaveAs(), Fail:" + e.ToString());

                    rtn = false;
                }
            }

            return rtn;
        }

        public bool Open(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return false;
            }

            string userID = this._data.UserID;

            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();

                try
                {
                    this._data = bf.Deserialize(fs) as RDFuncData;

                    MACLockerInfo mac = this._macData.GetMacAdressStatus();

                    if (mac != null)
                    {
                        Console.WriteLine("[RDFunc], Open(), MACLockerInfo : " + mac.MacAdressSN);

                        this._data.HighSpeedModeDelayTime = mac.Delay;

                        this._data.IsEnableESDHighSpeedMode = mac.IsESDHighSpeedMode;

                        this._data.ESDHighSpeedDelayTime = mac.ESDDeleay;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("[RDFunc], Open(), Fail:" + e.ToString());

                    return false;
                }
            }

            if (this._data != null)
            {
                if (userID != this._data.UserID)
                {
                    this._data = new RDFuncData(userID);

                    Console.WriteLine("[RDFunc], UserID Not Match, File ID:" + this._data.UserID + " UI ID:" + userID);

                    return false;
                }
            }


            return true;
        }
        
        public void CheckSpecCtrlCompatibility() // 檢查 SpecCtrl 相容性
        {
            if (this._data.SpecDataDefinition != null && this._data.SpecDataDefinition.Count > 0)
            {
                foreach (var item in this._data.SpecDataDefinition.Items)
                {
                    TestItemDescription compareItem = TesterSpecDescription.CreateDefaultItemDescription(item.KeyName);

                    foreach (var prop in compareItem.Property)
                    {
                        if (!item.ContainsKeyName(prop.PropertyKeyName))
                        {
                            // CreateDefaultItemDescription中, 若有新增的 Properity
                            // 需檢察 讀取進來的 RdData 是否含有這一個描述, 不然會造成 RDFunc 無法描述這一個item

                            item.Add(compareItem[prop.PropertyKeyName]);
                        }
                    }
                }
            }
        }

        #endregion

    }

    [Serializable]
    public class RDFuncData : ICloneable
    {
        private string _s9999;
        
        private bool _b0001;
        private bool _b0002;
       // private bool _b0003;  // 原本為 isEnableLIVItem
        //private bool _b0004;
        private double _d0001;
		private double _d0002;
		private double _d0003;
        
        //private string _s0001;
        private bool _b0006;
        private bool _b0007;
        private bool _b0008;
        private bool _b0009;
        private bool _b0010;

        //private double _d0004;
        private double _d0005;
 		private double _d0006;

        private double _d0010;
        private double _d0011;
		private double _d0012;
        private double _d0013; //optical switch delay

        private int _i0001;

        private ETesterConfigType _e0001;

		//SRC Setting
		private bool _b0100;
		private bool _b0101;

		private double _d0106;

        private TestItemDescriptionCollections _obj001;

 		private bool _b0020;
		private bool _b0021;
		private bool _b0022;

        public RDFuncData()
        {
            this._b0001 = true;

            this._b0002 = false;

            //this._b0003 = false;

            //this._b0004 = false;

            this._b0006 = false;

            this._b0007 = false;

            this._b0008 = false;

            this._b0009 = false;

            this._b0010 = false;

            this._i0001 = 0;

            //this._d0001 = 15.0d;

            this._d0002 = 8.0d;

            this._d0003 = 5.0d;

            this._d0013 = 0.0d;

            //this._d0004 = 1.6d;

            this._d0005 = 100.0d;

            //this._s0001 = "192.168.50.99";

            this._d0006 = 100.0d;

            this._d0010 = 5.0d; // Multi-Die Series Type Delay Time (Per Channel)

            this._d0011 = 3.0d; // Multi-Die Parallel Type Delay Time (Per Channel)

			this._d0012 = 1.0d; // VFD Gain

			this._b0100 = false;

			this._b0101 = false;

			this._d0106 = 0.1;

            this._e0001 = ETesterConfigType.LEDTester;

            this._b0020 = false;
			this._b0021 = false;
			this._b0022 = false;
        }

        public RDFuncData(string userID) : this()
        {
            this._s9999 = userID;
        }

        public string UserID
        {
            get { return this._s9999; }
            set { this._s9999 = value; }
        }

		public bool IsEnableVRDelayTime
		{
			get { return this._b0001; }
			set { this._b0001 = value; }
		}

        public bool IsEnableESDHighSpeedMode
        {
            get { return this._b0002; }
            set { this._b0002 = value; }
        }

        //public bool IsEnableRTHTestItem
        //{
        //    get { return this._b0004; }
        //    set { this._b0004 = value; }
        //}

		public bool IsKeepRecoveryData
		{
			get { return this._b0006; }
			set { this._b0006 = value; }
		}

		public bool IsEnableAbsMsrtIR
		{
			get { return this._b0007; }
			set { this._b0007 = value; }
		}

		public int SrcTurnOffType
		{
			get { return this._i0001; }
			set { this._i0001 = value; }
		}

        public double ESDHighSpeedDelayTime
        {
            get { return this._d0001; }
            set { this._d0001 = value; }
        }

		public double HighSpeedModeDelayTime
		{
			get { return this._d0002; }
			set { this._d0002 = value; }
		}

		public double VRDelayTime
		{
			get { return this._d0003; }
			set { this._d0003 = value; }
		}

        public double OpticalSwitchDelay
        {
            get { return this._d0013; }
            set { this._d0013 = value; }
        }
        //public string RTHSrcMeterIPAddress
        //{
        //    get { return this._s0001; }
        //    set { this._s0001 = value; }
        //}

        //public double RTHTdTime
        //{
        //    get { return this._d0004; }
        //    set { this._d0004 = value; }
        //}

        public double ESDPrechargeWaitTime
        {
            get { return this._d0005; }
            set { this._d0005 = value; }
        }

        public double MDSeriesTypeDelayTime
        {
            get { return this._d0010; }
            set { this._d0010 = value; }
        }

        public double MDParallelTypeDelayTime
        {
            get { return this._d0011; }
            set { this._d0011 = value; }
        }

        public bool IsEnableForceMode
        {
            get { return this._b0008; }
            set { this._b0008 = value; }
        }

		public bool IsTurnOffRangeIBackToDefault
        {
			  get { return this._b0100; }
			  set { this._b0100 = value; }
        }

		public bool IsSettingReverseCurrentRange
        {
			  get { return this._b0101; }
			  set { this._b0101 = value; }
        }

		public double ReverseCurrentApplyRange
        {
			  get { return this._d0106; }
			  set { this._d0106 = value; }
        }

        public TestItemDescriptionCollections SpecDataDefinition
        {
            get { return this._obj001; }
            set { this._obj001 = value; }
        }

        public bool IsShowTesterChannelConfig
        {
            get { return this._b0009; }
            set { this._b0009 = value; }
        }

        public bool IsShowContinousProbing
        {
            get { return this._b0010; }
            set { this._b0010 = value; }
        }

        public ETesterConfigType TesterConfigType
        {
            get { return this._e0001; }
            set { this._e0001 = value; }
        }

		public double VFDGain
		{
			get { return this._d0010; }
			set { this._d0010 = value; }
		}

        public bool IsEnableSrcMeterLog
        {
            get { return this._b0020; }
            set { this._b0020 = value; }
        }

		public bool IsDisableMVFLB
		{
			get { return this._b0021; }
			set { this._b0021 = value; }
		}

		public bool IsEnableVFDGain
		{
			get { return this._b0022; }
			set { this._b0022 = value; }
		}

        public object Clone()
        {
            RDFuncData cloneObj = this.MemberwiseClone() as RDFuncData;

            cloneObj._obj001 = this._obj001.Clone() as TestItemDescriptionCollections;

            return cloneObj;
        }
    }
}
