 using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
    [Serializable]
    public abstract class TestItemData : ICloneable, IDisposable
    {
        protected object _lockObj;

        protected string _id;						// Primary Key
        protected string _keyName;			// Identification marking of the test item
        protected string _name;					// Display information of the test item

        protected ETestType _type;  
        protected int _order;
        protected int _subItemIndex;
        protected bool _isEnable;
        protected bool _isTested;

		protected ElectSettingData[] _elecSetting;					// [ ElecSettingItemIndex ]
        protected TestResultData[] _msrtResult;					// [ ResultItemIndex ]		
        protected GainOffsetData[] _gainOffsetSetting;			// [ Type (LOP, WLP, WLC, WLD, x, y ...) = ResultItemIndex ]
        protected GainOffsetData[] _byChannelGainOffset;        // 區域補償係數
		protected TestGroupCtrl _testGroupCtrl;

        protected uint _switchingChannel;

        public TestItemData()
        {
            this._lockObj = new object();

            this._id = DateTime.Now.ToString("yyyyMMddHHmmss");
            this.KeyName = string.Empty;
            this.Name = string.Empty;

            this._type = ETestType.IF;

            this._order = 0;
            this._subItemIndex = 0;
            this._isEnable = true;
            this._isTested = false;

            this._gainOffsetSetting = null;
            this._msrtResult = null;

			this._testGroupCtrl = new TestGroupCtrl();

            this._switchingChannel = 0;

            IsNewCreateItem = false;

            IsUserSetEnable = true;
            IsDeviceSetEnable = true;
        }

        #region >>> Public Property <<< 

        /// <summary>
        /// Unique ID, or Primary key of the test item in system
        /// </summary>               
        public string ID
        {
            get { return this._id; }
            set { lock (this._lockObj) { this._id = value; } }     
        }

        /// <summary>
        /// Identification marking of the test item
        /// </summary>
        public string KeyName
        {
            get { return this._keyName; }
            set { lock (this._lockObj) { this._keyName = value; } }       
        }

        /// <summary>
        /// Display information of the test item       
        /// </summary>
        public string Name
        {
            get { return this._name; }
            set { lock (this._lockObj) { this._name = value; } }
        }

        /// <summary>
        /// The type of test item, IF, VF, ...
        /// </summary>
        public ETestType Type
        {
            get { return this._type; }
            set { lock (this._lockObj) { this._type = value; } }
        }

        /// <summary>
        /// The order of test item
        /// </summary>
        public int Order
        {
            get { return this._order; }
            set { lock (this._lockObj) { this._order = value; } }
        }

        /// <summary>
        /// Item index for grouped test items  
        /// </summary>
        public int SubItemIndex
        {
            get { return this._subItemIndex; }
            set { lock (this._lockObj) 
					{ 
						this._subItemIndex = value;
						this.ResetKeyName();
					} 
				}
        }

        /// <summary>
        ///  Is enable the test item
        /// </summary>
		//public bool IsEnable
		public bool IsEnable
        {
            get { bool enable = IsUserSetEnable && IsDeviceSetEnable;
            return enable;
            }
            //get { return this._isEnable; }
            //set { lock (this._lockObj) { this._isEnable = value; } }        
        }

        public bool IsUserSetEnable { get; set; }

        public bool IsDeviceSetEnable { get; set; }

		/// <summary>
		/// Electrical setting items
		/// </summary>
		public ElectSettingData[] ElecSetting
		{
			get { return this._elecSetting; }
			set { lock (this._lockObj) { this._elecSetting = value; } }
		}
        
        /// <summary>
        /// Test result or measured result for the test item
        /// </summary>
        public TestResultData[] MsrtResult
        {
            get { return this._msrtResult; }
            set 
				{ 
					lock (this._lockObj) 
					{
						if (this._msrtResult.Length == value.Length)
						{
							this._msrtResult = value;
						}
						else if (this._msrtResult.Length > value.Length)	// Add new result item from constructure 
						{
							Array.Copy(value, this._msrtResult, value.Length);
						}
						else
						{
							Array.Copy(value, this._msrtResult, this._msrtResult.Length);
						}
					} 
				}
        }

        /// <summary>
        /// Gain and offset settings per channel and index 
        /// </summary>
        public GainOffsetData[] GainOffsetSetting
        {
            get { return this._gainOffsetSetting; }
            set 
				{ 
					lock (this._lockObj) 
					{
							if (this._gainOffsetSetting.Length == value.Length)
							{
							this._gainOffsetSetting = value;
							}
							else if (this._gainOffsetSetting.Length > value.Length)	// Add new result item from constructure 
							{
								Array.Copy(value, this._gainOffsetSetting, value.Length);
							}
							else
							{
								Array.Copy(value, this._gainOffsetSetting, this._gainOffsetSetting.Length);
							}
					} 
				}
        }

        /// <summary>
        /// Regional Compensate Gain and offset settings per channel and index 
        /// </summary>
        public GainOffsetData[] ByChannelGainOffsetSetting
        {
            get { return this._byChannelGainOffset; }
            set { lock (this._lockObj) { this._byChannelGainOffset = value; } }
        }

        public bool IsTested
        {
            get { return this._isTested; }
            set { lock (this._lockObj) { this._isTested = value; } }
        }

		public TestGroupCtrl TestGroupCtrl
		{
			get { return this._testGroupCtrl; }
			set { lock (this._lockObj) { this._testGroupCtrl = value; } }
		}

        public uint SwitchingChannel
        {
            get { return this._switchingChannel; }
            set { lock (this._lockObj) { this._switchingChannel = value; } }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool IsNewCreateItem
        {
            set;
            get;
        }

        #endregion

        #region >>> Public Method <<<

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }

		protected virtual void ResetKeyName() 
        {
            int num = this._subItemIndex + 1;     // 0-base

            this.KeyName = this._type.ToString() + "_" + num.ToString();
        }

        public bool CalibrateByChannel()
        {
            double v = 0.0d;

            if (this._byChannelGainOffset == null || this._msrtResult == null)
                return true;

            if (this._byChannelGainOffset.Length == this._msrtResult.Length)
            {
                for (int i = 0; i < this._byChannelGainOffset.Length; i++)
                {
                    v = this._msrtResult[i].RawValue;

                    if (this._byChannelGainOffset[i].IsEnable == false || v == 0)
                        continue;

                    v = (v * v * this._byChannelGainOffset[i].Square) + (v * this._byChannelGainOffset[i].Gain) + this._byChannelGainOffset[i].Offset;

                    v = (v * v * this._byChannelGainOffset[i].Square2) + (v * this._byChannelGainOffset[i].Gain2) + this._byChannelGainOffset[i].Offset2;

                    this._msrtResult[i].Value = (v * v * this._byChannelGainOffset[i].Square3) + (v * this._byChannelGainOffset[i].Gain3) + this._byChannelGainOffset[i].Offset3;
                }
            }

            return true; 
        }

        public bool Calibrate()
        {
            double v = 0.0d;

            if (this._msrtResult != null)
            {
                for (int i = 0; i < this._msrtResult.Length; i++)
                {
                    this._msrtResult[i].Value = this._msrtResult[i].RawValue;
                }
            }

            if ( this._gainOffsetSetting == null || this._msrtResult == null )
                return true;

            if (this.GainOffsetSetting.Length == this._msrtResult.Length)
            {
                for (int i = 0; i < this._msrtResult.Length; i++)
                {
                    v = this._msrtResult[i].RawValue;

                    if (this._gainOffsetSetting[i].IsEnable == false || v == 0)
                        continue;

                    v = (v * v * this._gainOffsetSetting[i].Square) + (v * this._gainOffsetSetting[i].Gain) + this._gainOffsetSetting[i].Offset;

                    v = (v * v * this._gainOffsetSetting[i].Square2) + (v * this._gainOffsetSetting[i].Gain2) + this._gainOffsetSetting[i].Offset2;

                    this._msrtResult[i].Value = (v * v * this._gainOffsetSetting[i].Square3) + (v * this._gainOffsetSetting[i].Gain3) + this._gainOffsetSetting[i].Offset3;
                }
            } 
            return true;
        }

        public bool Calibrate(int index)
        {
            double v = 0.0d;

            if (this._gainOffsetSetting == null || this._msrtResult == null)
                return true;

            if (this.GainOffsetSetting.Length == this._msrtResult.Length)
            {
                v = this._msrtResult[index].RawValue;

                if (this._gainOffsetSetting[index].IsEnable == false || v == 0)
                    return true;

                v = (v * v * this._gainOffsetSetting[index].Square) + (v * this._gainOffsetSetting[index].Gain) + this._gainOffsetSetting[index].Offset;

                v = (v * v * this._gainOffsetSetting[index].Square2) + (v * this._gainOffsetSetting[index].Gain2) + this._gainOffsetSetting[index].Offset2;

                this._msrtResult[index].Value = (v * v * this._gainOffsetSetting[index].Square3) + (v * this._gainOffsetSetting[index].Gain3) + this._gainOffsetSetting[index].Offset3;
            }
            return true;
        }

        public bool CalibrateLIVSweepData(LIVData livData)
        {
            double v = 0.0d;

            if (this._gainOffsetSetting == null || this._msrtResult == null)
            {
                return true;
            }

            if (this.GainOffsetSetting.Length == this._msrtResult.Length)
            {
                for (int i = 0; i < this._msrtResult.Length; i++)
                {
                    if (this._gainOffsetSetting[i].IsEnable == false)
                    {
                        continue;
                    }

                    if (this._msrtResult[i].KeyName.Contains(ELIVOptiMsrtType.LIVWATTTD.ToString()) ||
                        this._msrtResult[i].KeyName.Contains(ELIVOptiMsrtType.LIVLMTD.ToString()) ||
                        this._msrtResult[i].KeyName.Contains(ETransistorOptiMsrtType.TRWATTTD.ToString()) ||
                        this._msrtResult[i].KeyName.Contains(ETransistorOptiMsrtType.TRLMTD.ToString()))
                    {
                        if (this._msrtResult[i].RawValue == 0)
                        {
                            continue;
                        }

                        v = this._msrtResult[i].RawValue;

                        v = (v * v * this._gainOffsetSetting[i].Square) + (v * this._gainOffsetSetting[i].Gain) + this._gainOffsetSetting[i].Offset;

                        v = (v * v * this._gainOffsetSetting[i].Square2) + (v * this._gainOffsetSetting[i].Gain2) + this._gainOffsetSetting[i].Offset2;

                        this._msrtResult[i].Value = (v * v * this._gainOffsetSetting[i].Square3) + (v * this._gainOffsetSetting[i].Gain3) + this._gainOffsetSetting[i].Offset3;
                    }
                    else
                    {
                        float[] data = livData[this._msrtResult[i].KeyName].DataArray;

                        for (int j = 0; j < data.Length; j++)
                        {
                            data[j] = (float)((data[j] * data[j] * this._gainOffsetSetting[i].Square) + (data[j] * this._gainOffsetSetting[i].Gain) + this._gainOffsetSetting[i].Offset);

                            data[j] = (float)((data[j] * data[j] * this._gainOffsetSetting[i].Square2) + (data[j] * this._gainOffsetSetting[i].Gain2) + this._gainOffsetSetting[i].Offset2);

                            data[j] = (float)((data[j] * data[j] * this._gainOffsetSetting[i].Square3) + (data[j] * this._gainOffsetSetting[i].Gain3) + this._gainOffsetSetting[i].Offset3);
                        }

                        this._msrtResult[i].Value = data[data.Length - 1];
                    }
                }
            }

            return true;
        }

        public virtual Dictionary<string, object> GetItemInfo()
        {
            
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ID", ID);
            dic.Add("KeyName", KeyName);
            dic.Add("Name", Name);
            dic.Add("Type", Type.ToString());
            dic.Add("Order", Order);
            dic.Add("IsEnable", IsEnable);

            if (_msrtResult != null && _msrtResult.Length > 0)
            {
                List<Dictionary<string, object>> msrtItemInfoList = new List<Dictionary<string, object>>();
                for (int i = 0; i < _msrtResult.Length; ++i)
                {
                    if (_msrtResult[i].IsVision)//沒顯示就不要放了
                    {
                        msrtItemInfoList.Add(_msrtResult[i].GetTestResultDataInfo());
                    }
                }
                dic.Add("TestInfo", GetTestItemForceSetting());
                dic.Add("MsrtResult", msrtItemInfoList);
                
            }
            //ElecSetting 以及 GainOffsetSetting應在子類別被override實作
            
            //

            return dic;
        }

        public virtual object Clone()
        {
			// return this.MemberwiseClone();

			MemoryStream ms = new MemoryStream();
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(ms, this);
			ms.Position = 0;
			object obj = bf.Deserialize(ms);
			ms.Close();
			return obj;
        }

        #endregion

        #region >>Protected mehtod<<

        protected virtual object GetTestItemForceSetting()//LCR,Calc等記得要覆寫掉
        {
            Dictionary<string, object> kObj = new Dictionary<string, object>();
            if (_elecSetting != null && _elecSetting.Length > 0)
            {
                ElectSettingData eSet = _elecSetting[0];
                kObj.Add("MsrtType", eSet.MsrtType.ToString());
                kObj.Add("ForceValue", eSet.ForceValue);
                kObj.Add("ForceUnit", eSet.ForceUnit);
                kObj.Add("ForceTime", eSet.ForceTime);
                kObj.Add("ForceTimeUnit", eSet.ForceTimeUnit);
                kObj.Add("MsrtProtection", eSet.MsrtProtection);
                kObj.Add("MsrtUnit", eSet.MsrtUnit);       
            }


            return kObj;
        }
        protected void SetMsrtNameAsKey()
        {
            if (_msrtResult != null)
            {
                for (int i = 0; i < _msrtResult.Length; ++i)
                {
                    _msrtResult[i].Name = _msrtResult[i].KeyName;

                }
            }
        }
        #endregion
    }
}
