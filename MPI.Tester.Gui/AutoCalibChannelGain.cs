using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPI.Tester.Data;
using System.Linq;
using MPI.Tester.Tools;

namespace MPI.Tester.Gui
{
    class chipTestData
    {
        public Dictionary<string, float> Data { get; set; }

        public uint channel  { get; set; }

        public string rowColKey { get; set; }
    }

    public class AutoCalibChannelGain
    {
        private bool _isEnableRun = false;

        private string _targetRowCol = string.Empty;

        private string _UIShowRowColKey = string.Empty;

        private uint numbersOfChannel = 0;

        private List<string> _rowColKeyList = new List<string>();

        private List<chipTestData> _testRawData = new List<chipTestData>();

        private Dictionary<string, GainOffset[]> _calibrationData = new Dictionary<string, GainOffset[]>(); // array represent channel index。

        private Dictionary<string, TestResultData> _enableItem = new Dictionary<string, TestResultData>();

        private Dictionary<string, List<chipTestData>>_theSameRowColTestData = new Dictionary<string, List<chipTestData>>();

        private bool _isCalibrationOK = true;

        public AutoCalibChannelGain()
        {

        }

        public Dictionary<string, GainOffset[]> GainOffset
        {
            get
            {
                return this._calibrationData;
            }
        }

        public string CalibRowColKey
        {
            get
            {
                return this._UIShowRowColKey;
            }
        }


        public bool Start(uint channels, ProductData pd)
        {
            _testRawData.Clear();

            _rowColKeyList.Clear();

            _enableItem.Clear();

            this.numbersOfChannel = channels;

            if (channels != pd.TestCondition.ChannelConditionTable.Count)
            {
                _isEnableRun = false;

                return false;       
            }

            foreach (TestItemData item in pd.TestCondition.TestItemArray)
            {
                if (!item.IsEnable)
                    continue;

                if (item.MsrtResult == null)
                {
                    continue;
                }

                foreach (TestResultData data in item.MsrtResult)
                {
                    string[] keyname=data.KeyName.Split('_');

                    if (keyname[0] == "WATT" || keyname[0] == "LOP")
                    {
                        this._enableItem.Add(data.KeyName, data);
                    }
                }
            }

            Console.WriteLine("[AutoCalibChannelGain], Start(), ");

            _isEnableRun = true;

            return true;
        }

        public void End()
        {
            _isEnableRun = false;

            this.FetechData();

            this._isCalibrationOK=this.Calculate();

            _UIShowRowColKey = _targetRowCol;

            _targetRowCol = string.Empty;

            Console.WriteLine("[AutoCalibChannelGain], End(), Clear Data");
        }

        public bool Push(int colX, int rowY,uint channel, Dictionary<string, float> result)
        {
            if (!_isEnableRun)
            {
                return true;
            }

            string rowColKey = "X" + colX.ToString() + "Y" + rowY.ToString();

            if (_targetRowCol == string.Empty)
            {
                _targetRowCol = rowColKey;

                if (!_rowColKeyList.Contains(rowColKey))
                {
                    _rowColKeyList.Add(rowColKey);
                }
            }

            chipTestData testData = new chipTestData();

            testData.channel = channel;

            testData.rowColKey = rowColKey;

            testData.Data = result;
 
            // Channel = numbersOfChannel & 回到原始點 ，表示已經執行完成一個Run。

            if (channel == this.numbersOfChannel && _targetRowCol == rowColKey)
            {
                _targetRowCol = string.Empty;
            }

            _testRawData.Add(testData);

            return true;
        }

        private GainOffset[] CalcSingleChipChFator(string key, List<chipTestData> differentChannelData)
        {
            //==============================
            // 計算出每一顆在不同Channel的數據
            //==============================

            GainOffset[] gainOffset = new GainOffset[differentChannelData.Count];

            float[] contentData = new float[differentChannelData.Count];
   
            double baseValue = -1;

            foreach (chipTestData rawData in differentChannelData)
            {
                if (rawData.Data.ContainsKey("ISALLPASS"))
                {
                    if (rawData.Data["ISALLPASS"] == 2.0f)
                    {
                        return null;
                    }
                }

                if (rawData.Data.ContainsKey(key))
                {
                    float aa = rawData.Data[key];

                    if (rawData.channel >= 0)
                    {
                        uint channel = rawData.channel;

                        contentData[channel] = aa;
                    }
                }
            }

            baseValue = contentData[0];

            if (baseValue < 0.0001)
            {
                return null;
            }
            else
            {
                for (int i = 0; i < gainOffset.Length; i++)
                {
                    gainOffset[i] = new GainOffset();

                    gainOffset[i].Value = contentData[i];

                    gainOffset[i].Gain = baseValue/contentData[i];

                }

                return gainOffset;
            }
        }

        private bool Calculate()
        {
            //foreach (string keyPos in this._rowColKeyList)
            //{
            //    if (this._theSameRowColTestData.ContainsKey(keyPos))
            //    {
            //        List<chipTestData> data = this._theSameRowColTestData[keyPos];

            //        foreach (var result in this._enableItem)
            //        {
            //            _calibrationData.Add(result.Key, this.CalculateFactor(result.Key, data));
            //        }
            //    }
            //}

            _calibrationData.Clear();

            if (this._theSameRowColTestData.ContainsKey(this._targetRowCol))
            {
                List<chipTestData> data = this._theSameRowColTestData[this._targetRowCol];

                foreach (var result in this._enableItem)
                {
                    if (this.CalcSingleChipChFator(result.Key, data) != null)
                    {
                        _calibrationData.Add(result.Key, this.CalcSingleChipChFator(result.Key, data));
                    }
                }
            }

            if (this._calibrationData.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }


        }

        private void FetechData()
        {
            this._theSameRowColTestData.Clear();

            var var = from rawData in this._testRawData
                      let byRowCol = rawData.rowColKey
                      group new { rawData } by byRowCol into RowColGroup
                      select RowColGroup;

            foreach (var rowCol in var)
            {
                List<chipTestData> data = new List<chipTestData>();

                foreach (var rawData in rowCol)
                {
                    data.Add(rawData.rawData);
                }

                this._theSameRowColTestData.Add(rowCol.Key, data);
            }
        }

    }
}
