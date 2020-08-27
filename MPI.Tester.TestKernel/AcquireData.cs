using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using MPI.Tester.Data;

namespace MPI.Tester.TestKernel
{
	[Serializable]
	public class AcquireData : ICloneable
    {
        #region >>> Private Property <<<

        private object _lockObj;
        private bool _isDataChange;
        private ChipInfo _chipInfo;
        private List<TestResultData> _outputTestResult;
        private SpectrumDataSet _spectrumDataSet;
        private ElecSweepDataSet _elecSweepDataSet;
        private LIVDataSet _livDataSet;
        private PIVDataSet _pivDataSet;
        private LCRDataSet _lcrDataSet;
        private DeviceRunTimeDataSet _deviceDataSet;
        private ChannelResultDataSet _channelResultSet;

        private bool _isGetOpticalSamplingSource;
        private uint _opticalSamplingSourceChannel;
        #endregion

        #region >>> Constructor / Disposor <<<

        public AcquireData()
        {
            this._lockObj = new object();

            this._chipInfo = new ChipInfo();

            this._outputTestResult = new List<TestResultData>();

            this._spectrumDataSet = new SpectrumDataSet();

            this._elecSweepDataSet = new ElecSweepDataSet();

            this._livDataSet = new LIVDataSet();

            this._pivDataSet = new PIVDataSet();

            this._lcrDataSet = new LCRDataSet();

            this._deviceDataSet = new DeviceRunTimeDataSet();

            this._channelResultSet = new ChannelResultDataSet();
        }

        #endregion

        #region >>> Public Property <<<

        public TestResultData this[string msrtKeyName]
        {
            get 
            {
                if (this._outputTestResult.Count != 0)
                {
                    foreach (var data in this._outputTestResult)
                    {
                        if (data.KeyName == msrtKeyName)
                        {
                            return data;
                        }
                    }
                }

                return null;
            }
        }

        public bool IsDataChange
        {
            get { return this._isDataChange; }
            set { lock (_lockObj) { this._isDataChange = value; } }
        }

        public ChipInfo ChipInfo
        {
            get { return this._chipInfo; }
            set { lock (_lockObj) { this._chipInfo = value; } }
        }

        public List<TestResultData> OutputTestResult
        {
            get { return this._outputTestResult; }
            set { lock (_lockObj) { this._outputTestResult = value; } }
            }                    

        public SpectrumDataSet SpectrumDataSet
            {
            get { return this._spectrumDataSet; }
            set { lock (_lockObj) { this._spectrumDataSet = value; } }
        }

        public ElecSweepDataSet ElecSweepDataSet
        {
            get { return this._elecSweepDataSet; }
            set { lock (_lockObj) { this._elecSweepDataSet = value; } }
            }

        public LIVDataSet LIVDataSet
        {
            get { return this._livDataSet; }
            set { lock (_lockObj) { this._livDataSet = value; } }
        }

        public PIVDataSet PIVDataSet
        {
            get { return this._pivDataSet; }
            set { lock (_lockObj) { this._pivDataSet = value; } }
        }

        public LCRDataSet LCRDataSet
        {
            get { return this._lcrDataSet; }
            set { lock (_lockObj) { this._lcrDataSet = value; } }
        }

        public DeviceRunTimeDataSet DeviceRunTimeDataSet
        {
            get { return this._deviceDataSet; }
            set { lock (_lockObj) { this._deviceDataSet = value; } }
        }
        public ChannelResultDataSet ChannelResultDataSet
        {
            get { return this._channelResultSet; }
            set { lock (_lockObj) { this._channelResultSet = value; } }
        }

        public List<string> EnableTestResult
        {
            get    
            {
                List<string> rtnList = new List<string>();

                foreach (var item in this._outputTestResult)
                {
                    //if (item.IsEnable)
                    if (item.IsThisItemTested)
                    {
                        rtnList.Add(item.KeyName);
                    }
                }

                return rtnList;
            }
        }

        public string[] ResultItemKeyName
        {
            get
            {
                string[] keys = new string[this._outputTestResult.Count];

                for (int i = 0; i < keys.Length; i++)
                {
                    keys[i] = this._outputTestResult[i].KeyName;
                }
                return keys;
            }
        }

        public bool IsGetOpticalSamplingSource
        {
            get { return this._isGetOpticalSamplingSource; }
            set { lock (_lockObj) { this._isGetOpticalSamplingSource = value; } }
        }

        public uint OpticalSamplingSourceChannel
        {
            get { return this._opticalSamplingSourceChannel; }
            set { lock (_lockObj) { this._opticalSamplingSourceChannel = value; } }
        }

        #endregion

        #region >>> Public Method <<<

        public void SetData(ConditionData condiData, TestResultData[] sysResultItem)
        {
            int channelCount = 1; //default = 1
            
            TestItemData[] testItemDataArray = condiData.TestItemArray;

            ChannelConditionData[] channelCondiArray = condiData.ChannelConditionTable.Channels;
            
            if (testItemDataArray == null)
            {
                return;
            }

            //---------------------------------------------------------------------------------------
            // Clear Data
            //---------------------------------------------------------------------------------------
            this._isDataChange = true;

            this._outputTestResult.Clear();

            this._spectrumDataSet.Clear();

            this._elecSweepDataSet.Clear();

            this._livDataSet.Clear();

            this._pivDataSet.Clear();

            this._lcrDataSet.Clear();
            
            this._channelResultSet.Clear();

            this._deviceDataSet.Clear();

            //---------------------------------------------------------------------------------------
            // Add TestResultData from "TestKernel"
            //---------------------------------------------------------------------------------------
            this._outputTestResult.AddRange(sysResultItem);

            //---------------------------------------------------------------------------------------
            // Add TestResultData from "Prober System"
            //---------------------------------------------------------------------------------------
            foreach (string str in Enum.GetNames(typeof(EProberDataIndex)))
            {
                if (str == "CHUCKX" || str == "CHUCKY" || str == "CHUCKZ" ||
                     str == "ES01" || str == "ES02" || str == "ES03" || str == "ES04" || str == "SEQUENCETIME")
                {
                    TestResultData data = new TestResultData(str, str, "", "0.0000");

                    this._outputTestResult.Add(data);
                }
                else
                {
                    TestResultData data = new TestResultData(str, str, "", "0");

                    this._outputTestResult.Add(data);
                }
            }

            //---------------------------------------------------------------------------------------
            // Create Big Data
            //---------------------------------------------------------------------------------------
            if (channelCondiArray != null)
            {
                channelCount = channelCondiArray.Length;
                // Create channelResultData
                for (uint channel = 0; channel < channelCount; channel++)
                {
                    this._channelResultSet.Add(new ChannelResultData());

                    this._channelResultSet[channel].AddRange(this._outputTestResult.ToArray());
                    
                    ChannelConditionData channelCondi = channelCondiArray[channel];

                    if (channelCondi == null)
                    {
                        continue;
                    }

                    foreach (TestItemData item in channelCondi.Conditions)
                    {
                        if (item.MsrtResult == null)
                        {
                            continue;
                        }

                        foreach (TestResultData data in item.MsrtResult)
                        {
                            this._channelResultSet[channel].Add(data);
                        }
                    }
                }
            }

            this._elecSweepDataSet.ChannelCount = channelCount;
            this._spectrumDataSet.ChannelCount = channelCount;

            foreach (TestItemData item in testItemDataArray)
            {
                if (item.MsrtResult != null)
                {
                    foreach (TestResultData data in item.MsrtResult)
                    {
                        this._outputTestResult.Add(data);
					}
				}

                for (uint channel = 0; channel < channelCount; channel++)
                {
                    switch (item.Type)
                    {
                        case ETestType.IVSWEEP:
                        case ETestType.VISWEEP:
                        case ETestType.THY:
                        case ETestType.RTH:
                        case ETestType.VISCAN:
                            this._elecSweepDataSet.Add(new ElecSweepData(item, channel));
                            break;
                        //---------------------------------------------------------------------------------------
                        case ETestType.LOPWL:
                        case ETestType.OSA:
                            this._spectrumDataSet.Add(new SpectrumData(item, channel));
                            break;
                        //---------------------------------------------------------------------------------------
                        case ETestType.LIV:
						case ETestType.TRANSISTOR:
                            if (channel == 0)
                            {
                                this._livDataSet.Add(new LIVData(item));
                            }
                            break;
                        //---------------------------------------------------------------------------------------
                        case ETestType.PIV:
                            if (channel == 0)
                            {
                                this._pivDataSet.Add(new PIVData(item));
                            }
                            break;
                        //---------------------------------------------------------------------------------------
                        case ETestType.LCRSWEEP:
                            if (channel == 0)
                            {
                                this._lcrDataSet.Add(new LCRData(item, (item as LCRSweepTestItem).LCRSetting.Point));
                            }
                            break;
                        //---------------------------------------------------------------------------------------
                        case ETestType.IO:

                            //this._pivDataSet.Add(new IOData(item, IOTestIte, channel));
                            if (channel == 0)
                            {
                                
                            }
                            break;
                        //---------------------------------------------------------------------------------------
                        default:
                            break;
                    }
                }
                

			}
		}

        public void SetChannelResultToTestResultData(uint channel)
        {
            foreach (TestResultData data in this._outputTestResult)
            {
                data.Value = this._channelResultSet[channel][data.KeyName].Value;
            }
        }

        public void Overwrite(ref AcquireData data)
        {
            //---------------------------------------------------------------------------------------
            // AcquireData is Change, Clone to data
            //---------------------------------------------------------------------------------------
            if (this._isDataChange)       
            {
                data = this.Clone() as AcquireData;

                this._isDataChange = false;

                return;
            }

            //---------------------------------------------------------------------------------------
            // Overwrite ChipInfo
            //---------------------------------------------------------------------------------------
            this._chipInfo.Overwrite(data._chipInfo);

            //---------------------------------------------------------------------------------------
            // Overwrite OutputTestResult
            //---------------------------------------------------------------------------------------
            for (int i = 0; i < data.OutputTestResult.Count; i++)
            {

                //if (!data.OutputTestResult[i].IsEnable)
                //{                    
                //    continue;
                //}

                data.OutputTestResult[i].AdjacentRange = this._outputTestResult[i].AdjacentRange;

                data.OutputTestResult[i].AdjacentType = this._outputTestResult[i].AdjacentType;

                data.OutputTestResult[i].BinGrade = this._outputTestResult[i].BinGrade;

                //data.OutputTestResult[i].ColorSettingData = this._outputTestResult[i].ColorSettingData;

                data.OutputTestResult[i].EnableAdjacent = this._outputTestResult[i].EnableAdjacent;

                data.OutputTestResult[i].EnableColorOutOfRange = this._outputTestResult[i].EnableColorOutOfRange;

                data.OutputTestResult[i].Formate = this._outputTestResult[i].Formate;

                data.OutputTestResult[i].ID = this._outputTestResult[i].ID;

                data.OutputTestResult[i].IsEnable = this._outputTestResult[i].IsEnable;

                data.OutputTestResult[i].IsSkip = this._outputTestResult[i].IsSkip;

                data.OutputTestResult[i].IsVerify = this._outputTestResult[i].IsVerify;

                data.OutputTestResult[i].IsVision = this._outputTestResult[i].IsVision;

                data.OutputTestResult[i].KeyName = this._outputTestResult[i].KeyName;

                data.OutputTestResult[i].MaxLimitValue = this._outputTestResult[i].MaxLimitValue;

                data.OutputTestResult[i].MaxLimitValue2 = this._outputTestResult[i].MaxLimitValue2;

                data.OutputTestResult[i].MinLimitValue = this._outputTestResult[i].MinLimitValue;

                data.OutputTestResult[i].MinLimitValue2 = this._outputTestResult[i].MinLimitValue2;

                data.OutputTestResult[i].Name = this._outputTestResult[i].Name;

                data.OutputTestResult[i].RawValue = this._outputTestResult[i].RawValue;

                data.OutputTestResult[i].IsTested = this._outputTestResult[i].IsTested;

                if (data.OutputTestResult[i].RawValueArray != null)
                {
                    for (int j = 0; j < data.OutputTestResult[i].RawValueArray.Length; j++)
                    {
                        data.OutputTestResult[i].RawValueArray[j] = this._outputTestResult[i].RawValueArray[j];
                    }
                }

                data.OutputTestResult[i].Unit = this._outputTestResult[i].Unit;

                data.OutputTestResult[i].Value = this._outputTestResult[i].Value;
            }

            //---------------------------------------------------------------------------------------
            // Overwrite SpectrumData
            //---------------------------------------------------------------------------------------
            this._spectrumDataSet.Overwrite(data._spectrumDataSet);

            //---------------------------------------------------------------------------------------
            // Overwrite ElecSweepData
            //---------------------------------------------------------------------------------------
            this._elecSweepDataSet.Overwrite(data._elecSweepDataSet);

            //---------------------------------------------------------------------------------------
            // Overwrite LIVData
            //---------------------------------------------------------------------------------------
            this._livDataSet.Overwrite(data._livDataSet);

            //---------------------------------------------------------------------------------------
            // Overwrite PIVData
            //---------------------------------------------------------------------------------------
            this._pivDataSet.Overwrite(data._pivDataSet);

            //---------------------------------------------------------------------------------------
            // Overwrite LCRData
            //---------------------------------------------------------------------------------------
            this._lcrDataSet.Overwrite(data._lcrDataSet);
            //---------------------------------------------------------------------------------------
            // Overwrite Device Runtime Data
            //---------------------------------------------------------------------------------------
            this._deviceDataSet.Overwrite(data._deviceDataSet);
            //---------------------------------------------------------------------------------------
            // Overwrite ChannelResultData
            //---------------------------------------------------------------------------------------
            this._channelResultSet.Overwrite(data._channelResultSet);
        }

        public object Clone()
        {
            AcquireData cloneObj = new AcquireData();

            cloneObj._chipInfo = this._chipInfo.Clone() as ChipInfo;

            foreach (var data in this._outputTestResult)
            {
                cloneObj._outputTestResult.Add(data.Clone() as TestResultData);
            }
            
            foreach (var data in this._spectrumDataSet)
            {
                cloneObj._spectrumDataSet.Add(data.Clone() as SpectrumData);
            }

            foreach (var data in this._elecSweepDataSet)
            {
                cloneObj._elecSweepDataSet.Add(data.Clone() as ElecSweepData);
            }

            foreach (var data in this._livDataSet)
            {
                cloneObj._livDataSet.Add(data.Clone() as LIVData);
            }

            foreach (var data in this._pivDataSet)
            {
                cloneObj._pivDataSet.Add(data.Clone() as PIVData);
            }

            foreach (var data in this._lcrDataSet)
            {
                LCRData lcrData = (data.Clone() as LCRData);
                cloneObj._lcrDataSet.Add(lcrData);
            }

            foreach (var data in this._deviceDataSet)
            {
                DeviceRunTimeData drData = (data.Clone() as DeviceRunTimeData);
                cloneObj._deviceDataSet.Add(drData);
            }
            
            foreach (var data in this._channelResultSet)
            {
                cloneObj._channelResultSet.Add(data.Clone() as ChannelResultData);
            }



            return cloneObj;
        }

        #endregion
    }
}
