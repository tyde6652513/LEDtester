using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester;
using MPI.Tester.Data;
using System.IO;

namespace MPI.Tester.Gui
{
  public class PreSamplingCheck
    {
        private Dictionary<string, Dictionary<string, float>> _samplingData = new Dictionary<string, Dictionary<string, float>>(); // col to outputData
       
        private Dictionary<string, Dictionary<string, float>> _msrtData = new Dictionary<string, Dictionary<string, float>>(); // col to outputData

        private Dictionary<string, TestResultData> _enableItem = new Dictionary<string, TestResultData>();

        private TestItemData[] _testItemdata =null;

        private bool _isEnableRun = false;

        private int _errorCount = 0;

        private uint _continousErrorCountSpec = 2;

        private string previousRowColKey=string.Empty;

        private List<string[]> _tempdata = new List<string[]>();

        private List<string[]> _tempdata2 = new List<string[]>();
         
        private bool _isCheckAllPassData=false;

        private const uint _extendRange = 0;

        private ESamplingMointorMode _mode=ESamplingMointorMode.ValueType;


        public PreSamplingCheck()
        {

        }

        public bool Start(UISetting uiSetting,ProductData pd)
        {
            _isEnableRun = uiSetting.IsEnableMonitorPreSamplingData;

            _continousErrorCountSpec = pd.SamplingMonitorConsecutiveErrCount;

            _isCheckAllPassData = pd.IsAdjacentAllItemPassCheck;

            _mode = (ESamplingMointorMode)(int)pd.SamplingMonitorMode;

            if (_isEnableRun == false)
            {
                return true;
            }

            this._samplingData.Clear();

            this._msrtData.Clear();

            this.previousRowColKey = string.Empty;

            this._tempdata.Clear();

            this._tempdata2.Clear();

            //===============
            // Create sampling File Name By WaferID
            //===============

            //string fileNameAndPath = Path.Combine(uiSetting.PreSamplingDataPath, "S" + uiSetting.WaferNumber + ".csv");

            ////string lot = uiSetting.Barcode.Substring(0, 8);

            ////string wafer = uiSetting.Barcode.Replace(lot, string.Empty);

            //string loaclFileAndPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, "S" + uiSetting.WaferNumber + ".csv");

            if (uiSetting.Barcode.Length <10)
            {
                this._isEnableRun = false;

                return false;
            }

            string lot = uiSetting.Barcode.Substring(0, 8);

            string wafer = uiSetting.Barcode.Replace(lot, string.Empty);

            string fileNameAndPath = Path.Combine(uiSetting.PreSamplingDataPath, "S" + wafer + ".csv");

            string loaclFileAndPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, "S" + wafer + ".csv");

            if (File.Exists(fileNameAndPath))
            {
                MPIFile.CopyFile(fileNameAndPath, loaclFileAndPath);
            }
            else
            {
                Console.WriteLine("[PreSamplingCheck],File is Not Exist");
            }

            Console.WriteLine("[PreSamplingCheck],Start()");

            //===============
            // Set Product Setting Spec
            //===============
            _testItemdata = pd.TestCondition.TestItemArray;

            this._enableItem.Clear();

            foreach (TestItemData item in this._testItemdata)
            {
                if (!item.IsEnable)
                    continue;

                if (item.MsrtResult == null)
                {
                    continue;
                }

                foreach (TestResultData data in item.MsrtResult)
                {
                    if (data.EnableSamplingMonitor)
                    {
                        this._enableItem.Add(data.KeyName, data);
                    }
                }
            }

            //===============
            // Read Sampling File
            //===============

            string[] LOPSaveItem = pd.LOPSaveItem.ToString().Split('_');

            Dictionary<string, string> resultItemNameDic = uiSetting.UserDefinedData.ResultItemNameDic;

            List<string> resultItem = new List<string>();

            List<string> resultItemKey = new List<string>();

            foreach (var Item in resultItemNameDic)
            {
                if (Item.Key.Contains("LOP"))
                {
                    if (!LOPSaveItem.Contains("mcd"))
                    {
                        continue;
                    }
                }

                if (Item.Key.Contains("WATT"))
                {
                    if (!LOPSaveItem.Contains("watt"))
                    {
                        continue;
                    }
                }

                if (Item.Key.Contains("LM"))
                {
                    if (!LOPSaveItem.Contains("lm"))
                    {
                        continue;
                    }
                }

                resultItem.Add(Item.Value);

                resultItemKey.Add(Item.Key);
            }

            //Find the report data row
            List<string[]> report = CSVUtil.ReadCSV(loaclFileAndPath);

            if (report == null)
            {
                this._isEnableRun = false;

                return false;
            }

            string compareStr = String.Empty;

            foreach (string str in resultItem)
            {
                compareStr += str;
            }

            List<string[]> removeRow = new List<string[]>();

            foreach (string[] rowStr in report)
            {
                if (rowStr.Length != resultItem.Count)
                {
                    removeRow.Add(rowStr);
                    continue;
                }

                string strTemp = String.Empty;

                foreach (string str in rowStr)
                {
                    strTemp += str;
                }

                if (strTemp == compareStr)
                {
                    removeRow.Add(rowStr);
                    break;
                }
                else
                {
                    removeRow.Add(rowStr);
                }
            }


            foreach (string[] temp in removeRow)
            {
                report.Remove(temp);
            }


            for (int i = 0; i < report.Count; i++)
            {
                Dictionary<string, float> result = new Dictionary<string, float>();

                int row = 0;

                int col = 0;

                int test = 0;

                for (int j = 0; j < resultItemKey.Count; j++)
                {
                    float data = 0.0f;
                    float.TryParse(report[i][j], out data);

                    result.Add(resultItemKey[j], data);

                    if (resultItemKey[j] == "TEST")
                        test = (int)data;

                    if (resultItemKey[j] == "COL")
                        col = (int)data;

                    if (resultItemKey[j] == "ROW")
                        row = (int)data;
                }

                string rowColKey = "X" + col.ToString() + "Y" + row.ToString();

                this._samplingData.Add(rowColKey, result);

                if (_extendRange >= 1)
                {
                    for (int e = 0; e < _extendRange; e++)
                    {
                        e++;

                        string extendrowColKeyRight = "X" + (col + e).ToString() + "Y" + row.ToString();

                        string extendrowColKeyleft = "X" + (col - e).ToString() + "Y" + row.ToString();

                        if (!this._samplingData.ContainsKey(extendrowColKeyRight))
                        {
                            this._samplingData.Add(extendrowColKeyRight, result);
                        }

                        if (!this._samplingData.ContainsKey(extendrowColKeyleft))
                        {
                            this._samplingData.Add(extendrowColKeyleft, result);
                        }
                    }
                }


            }

            Console.WriteLine("[PreSamplingCheck], Start(), sampling Data Count = " + this._samplingData.Count.ToString());

            if (this._samplingData.Count == 0)
            {
                TopMessageBox.Show("No Find Sampling Data");

                this._isEnableRun = false;

                return false;
            }

            return true;        
        }

        public void End()
        {
            if (!_isEnableRun)
		    {
                return;
            }

            string fileNameAndPath = Path.Combine(@"C:\MPI\LEDTester\Temp", DataCenter._uiSetting.WaferNumber + "_checking.csv");

            CSVUtil.WriteCSV(fileNameAndPath, this._tempdata);

            //CSVUtil.WriteCSV(@"C:\MPI\LEDTester\Temp\234v.csv", this._tempdata2);

            this._samplingData.Clear();

            this._msrtData.Clear();

            this.previousRowColKey = string.Empty;

            Console.WriteLine("[PreSamplingCheck], End(), Clear Data");

		}

        public bool Push(int colX, int rowY, Dictionary<string, float> result)
        {
            try
            {
                if (!this._isEnableRun)
                {
                    return true;
                }

                //=======================
                // Test Data meet spec (max/min)
                // then run this fun()
                //=======================

                if (_isCheckAllPassData)
                {
                    if (result.ContainsKey("ISALLPASS"))
                    {
                        if (result["ISALLPASS"] == 2.0f)
                        {
                            return true;
                        }
                    }
                }

                if (result.ContainsKey("MIR_1"))
                {
                    if (result["MIR_1"] > 1.0f)
                    {
                        return true;
                    }
                }

                if (result.ContainsKey("TEST"))
                {
                    if (result["TEST"] < 500.0f)
                    {
                        return true;
                    }
                }

                bool isPass = true;

                if (_mode == ESamplingMointorMode.ValueType)
                {
                    isPass=CheckValueData(colX, rowY, result);
                }
                else
                {
                    isPass = this.CheckGradientData(colX, rowY, result);
                }

                // 當PASS的情況下，把計數次數歸0
                // Fail 繼續去Count

                if (isPass)
                {
                    this._errorCount = 0;
                }
                else
                {
                    this._errorCount++;
                }

                bool finalResultPass = true;

                if (_errorCount == _continousErrorCountSpec)
                {
                    finalResultPass = false;

                    this._errorCount = 0;

                    Console.WriteLine("alarm (" + colX.ToString() + "," + rowY.ToString() + ")");
                }

                return finalResultPass;
            }
            catch
            {
                Console.WriteLine("[PreSamplingCheck], Push(), Try Catch Error");
                return true;
            }
        }

        private bool CheckValueData(int colX, int rowY, Dictionary<string, float> result)
        {
            bool isPass = true;

            string rowColKey = "X" + colX.ToString() + "Y" + rowY.ToString();

            if (this._samplingData.ContainsKey(rowColKey))
            {
                Dictionary<string, float> sampleResult = _samplingData[rowColKey];

                foreach (var item in this._enableItem)
                {
                    if (sampleResult.ContainsKey(item.Key))
                    {
                        string[] store = new string[6];

                        string key = item.Key;

                        double delta = result[key] - sampleResult[key];

                        double scale = 0.0d;

                        if (result[key] == 0.0f)
                        {
                            scale = 0.0d;
                        }
                        else if (sampleResult[key] == 0.0f)
                        {
                            scale = double.MaxValue;
                        }
                        else
                        {
                            scale = result[key] / sampleResult[key];
                        }

                        store[0] = rowY.ToString();

                        store[1] = result[key].ToString();

                        store[2] = sampleResult[key].ToString();

                        store[3] = scale.ToString();

                        store[4] = delta.ToString();

                        // Type 1 : Gain 
                        if (item.Value.SamplingMonitorType == 1)
                        {
                            if (Math.Abs(scale - 1) > (item.Value.SamplingMonitorRange * 0.01) && result[key] > 0.01)
                            {
                                isPass &= false;

                                Console.WriteLine(key.ToString() + "(" + colX.ToString() + "," + rowY.ToString() + ")  Value Error =  " + scale.ToString());

                            }
                        }
                        else
                        {
                            if (Math.Abs(delta) > (item.Value.SamplingMonitorRange))
                            {
                                isPass &= false;
                            }

                        }

                        store[5] = isPass.ToString();

                        this._tempdata.Add(store);
                    }
                }
            }

            return isPass;
        }

        private bool CheckGradientData(int colX, int rowY, Dictionary<string, float> result)
        {
            bool isPass = true;

            string rowColKey = "X" + colX.ToString() + "Y" + rowY.ToString();

            if (this._samplingData.ContainsKey(rowColKey))
           {
                //  抽測的資料
                Dictionary<string, float> sampleResult = _samplingData[rowColKey];

                Dictionary<string, float> previousSampleResult = null;

                Dictionary<string, float> previousMsrtResult = null;

                if (this._samplingData.ContainsKey(this.previousRowColKey))
                {
                    //抽測的上一筆資料 
                    previousSampleResult = _samplingData[previousRowColKey];
                }

                if (this._msrtData.ContainsKey(this.previousRowColKey))
                {
                    //點測的上一筆資料
                    previousMsrtResult = _msrtData[previousRowColKey];
                }

                previousRowColKey = rowColKey;

                _msrtData.Add(rowColKey, result);

                if (previousMsrtResult == null || previousSampleResult == null)
                {
                    return true;
                }
               
                // Row 的誤差大於5排布卡

                if (previousMsrtResult.ContainsKey("ROW") && result.ContainsKey("ROW"))
                {
                    float deltaRows = Math.Abs(previousMsrtResult["ROW"] - result["ROW"]);

                    if (deltaRows > 5)
                    {
                        return true;
                    }
                }

                foreach (var item in this._enableItem)
                {
                    if (sampleResult.ContainsKey(item.Key))
                    {
                        string key = item.Key;

                        // (抽測) 抓取前一筆的資料和這筆資料的差值
                        double SamplingDelta = previousSampleResult[key] - sampleResult[key];

                        // (量測) 抓取前一筆的資料和這筆資料的差值
                        double MsrtDelta = previousMsrtResult[key] - result[key];
               
                        double SamplingScale = 0.0d;

                        double MsrtScale = 0.0d;

                        if (previousSampleResult[key] == 0.0f)
                        {
                            SamplingScale = 0.0d;
                        }
                        else if (sampleResult[key] == 0.0f)
                        {
                            SamplingScale = double.MaxValue;
                        }
                        else
                        {
                            SamplingScale = previousSampleResult[key] / sampleResult[key];
                        }

                        // Msrt

                        if (previousMsrtResult[key] == 0.0f)
                        {
                            MsrtScale = 0.0d;
                        }
                        else if (result[key] == 0.0f)
                        {
                            MsrtScale = double.MaxValue;
                        }
                        else
                        {
                            MsrtScale = previousMsrtResult[key] / result[key];
                        }

                        double delta = MsrtDelta - SamplingDelta;

                        double scale = (MsrtScale / SamplingScale);

                        //=================================
                        // 針對抽測和全測時，測試數值都是0的檢查。
                        //=================================

                        if (double.IsNaN(scale))
                        {
                            scale = 0;
                        }

                        string[] store = new string[3];

                        store[0] = rowY.ToString();

                        store[1] = scale.ToString("0.000");
                        
                      

                            // Type 1 : Gain 
                        if (item.Value.SamplingMonitorType == 1)
                        {
                            if (Math.Abs(scale - 1) > (item.Value.SamplingMonitorRange * 0.01)
                                && result[key] > 0.01
                                && previousMsrtResult[key] > 0.01)
                                {                         
                                   isPass &= false;

                                   Console.WriteLine(key.ToString() + "(" + colX.ToString() + "," + rowY.ToString() + ")  Gradient Error =  " + scale.ToString());
                                }
                        }
                        else
                        {
                            if (Math.Abs(delta) > (item.Value.SamplingMonitorRange))
                            {
                                isPass &= false;
                            }
                        }

                        store[2] = isPass.ToString();

                        DataCenter._sysSetting.PreSamplingMonitorInfo = "Sampling : " +store[0] + "," + store[1] + "," + store[2];

                        this._tempdata.Add(store);

                        }
                    }
                }

            return isPass;
        }
      
        private bool CheckSmplingDataIsCorrect()
        {
            foreach (var data in this._samplingData)
            {
                float value = data.Value["COL"];

                if (value != 0.0f)
                {
                    return false;
                }

            }

            return true;
        }
    }
}
