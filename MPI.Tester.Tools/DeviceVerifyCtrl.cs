using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;
using System.IO;

namespace MPI.Tester.Tools
{
    public class DeviceVerifyCtrl
    {
        public const string SPEC_DATA_FILE_PATH = Constants.Paths.TOOLS_DIR;
        public const string SPEC_DATA_FILE_NAME = "DeviceVerify.spec";

        private string[] _reportDataHead = new string[] { "Item", "Condition", "Bias", "Spec", "GainB", "OffsetB", "GainA", "OffsetA",
                                                          "SMD1_Avg", "SMD1_Max", "SMD1_Min", "SMD1_Diff", "SMD1_PF", 
                                                          "SMD2_Avg", "SMD2_Max", "SMD2_Min", "SMD2_Diff","SMD2_PF", 
                                                          "SMD3_Avg", "SMD3_Max", "SMD3_Min", "SMD3_Diff","SMD3_PF" };
        private object _lockObj;

        private List<string> _lstNameDisplay;
        private List<string> _lstCondiDisplay;

        private DeviceVerifySpec _dvSpec;

        private uint _stdSmdChannel;
        private uint _channel;
        private bool _isPass;
        private int _lopwlINT;

        private Dictionary<string, List<string>> _dicResultHistory;

        public DeviceVerifyCtrl()
        {
            this._lockObj = new object();

            this._lstNameDisplay = new List<string>();
            this._lstCondiDisplay = new List<string>();

            this._dvSpec = new DeviceVerifySpec();
            this._stdSmdChannel = 0;
            this._channel = 0;
            this._isPass = false;

            this._dicResultHistory = new Dictionary<string, List<string>>();

            for (int i = 0; i < this._reportDataHead.Length; i++)
            {
                this._dicResultHistory.Add(this._reportDataHead[i], new List<string>());
            }

        }

        #region >>> Public Property <<<

        public string[] DisplayName
        {
            get { return this._lstNameDisplay.ToArray(); }
        }

        public string[] DisplayCondition
        {
            get { return this._lstCondiDisplay.ToArray(); }
        }

        public DeviceVerifySpec Data
        {
            get { return this._dvSpec; }
        }

        public bool IsPass
        {
            get { return this._isPass; }
        }

        public uint StdSMDChannel
        {
            get { return this._stdSmdChannel; }
            set { lock (this._lockObj) { this._stdSmdChannel = value; } }
        }

        public int OpticalCount
        {
            get { return this._lopwlINT; }
        }


        #endregion

        #region >>> Private Method <<<

    
        #endregion

        #region >>> Public Method <<<

        public void Update(ConditionData data)
        {
            // (1) Load spec info from file
            string pathAndFileNameWithExt = Path.Combine(SPEC_DATA_FILE_PATH, SPEC_DATA_FILE_NAME);

            DeviceVerifySpec loadSpec = new DeviceVerifySpec(); ;

            if (File.Exists(pathAndFileNameWithExt))
            {
                try
                {
                    loadSpec = MPI.Xml.XmlFileSerializer.Deserialize(typeof(DeviceVerifySpec), pathAndFileNameWithExt) as DeviceVerifySpec;
                }
                catch
                {
                    loadSpec = new DeviceVerifySpec();

                    Console.WriteLine("[DeviceVerifyCtrl] Deserialize Fail! ");
                }
            }

            this._lstCondiDisplay.Clear();
            this._lstNameDisplay.Clear();

            bool isPowerMsrt;
            string coefKeyName = string.Empty;
            string coefName = string.Empty;
            string condiInfo = string.Empty;
            int index = 0;

            this._dvSpec.Data.Clear();

            this._dvSpec.BiasRegisterDate = loadSpec.BiasRegisterDate;

            foreach (TestItemData item in data.TestItemArray)
            {
                if (item.ElecSetting == null || item.ElecSetting.Length == 0)
                {
                    continue;
                }

                if (item.GainOffsetSetting == null || item.GainOffsetSetting.Length == 0)
                {
                    continue;
                }

                condiInfo = item.ElecSetting[0].ForceValue.ToString() + item.ElecSetting[0].ForceUnit.ToString();

                index = 0;

                foreach (GainOffsetData coef in item.GainOffsetSetting)
                {
                    if (coef.IsVision)
                    {
                        isPowerMsrt = false;

                        coefName = coef.Name;

                        if ((item is LOPWLTestItem) && item.GainOffsetSetting[index].KeyName.IndexOf("_") > 0)
                        {
                            coefKeyName = item.GainOffsetSetting[index].KeyName.Remove(item.GainOffsetSetting[index].KeyName.IndexOf("_"));

                            switch (coefKeyName)
                            {
                                case "LOP":        // EOptiMsrtType.LOP :
                                    coefName = coef.Name + "(mcd)";
                                    isPowerMsrt = true;
                                    break;
                                //------------------------------------------------
                                case "WATT":       // EOptiMsrtType.WATT
                                    coefName = coef.Name + "(mW)";
                                    isPowerMsrt = true;
                                    break;
                                //------------------------------------------------
                                case "LM":         // EOptiMsrtType.LM
                                    coefName = coef.Name + "(lm)";
                                    isPowerMsrt = true;
                                    break;
                                //------------------------------------------------
                            }
                        }

                        if (loadSpec[coef.KeyName] == null)
                        {
                            DeviceVerifySpecData newSpec = new DeviceVerifySpecData(coef.KeyName, coef.Name, item.MsrtResult[index].Formate, item.MsrtResult[index].Unit);

                            newSpec.IsEnable = item.IsEnable & item.MsrtResult[index].IsEnable;

                            if (isPowerMsrt)
                            {
                                newSpec.Type = EGainOffsetType.Gain;
                            }
                            else
                            {
                                newSpec.Type = EGainOffsetType.Offset;
                            }

                            this._dvSpec.Data.Add(newSpec);
                        }
                        else
                        {
                            this._dvSpec.Data.Add(loadSpec[coef.KeyName].Clone() as DeviceVerifySpecData);

                            this._dvSpec[coef.KeyName].IsEnable = item.IsEnable & item.MsrtResult[index].IsEnable;
                        }

                        this._lstNameDisplay.Add(coefName);

                        this._lstCondiDisplay.Add(condiInfo);
                    }

                    index++;
                }
            }
                        
        }

        public void Save()
        {
            MPI.Tester.XML.XmlFileSerializer.Serialize(this._dvSpec, Path.Combine(SPEC_DATA_FILE_PATH, SPEC_DATA_FILE_NAME));
        }

        public void ResetCoefficients()
        {
            if (this._dvSpec == null || this._dvSpec.Count == 0)
            {
                return;
            }

            foreach (DeviceVerifySpecData data in this._dvSpec.Data)
            {
                data.Gain = 1.0d;
 
                data.Offset = 0.0d;
            }
        }

        public void SetCoefficients()
        {
            if (this._dvSpec == null || this._dvSpec.Count == 0)
            {
                return;
            }

            foreach (DeviceVerifySpecData data in this._dvSpec.Data)
            {
                data.SetCoefficient();
            }
        }

        public bool MsrtResltCalculate(uint channel, Dictionary<string, int> keyDic, List<float[]> results, SpecCalculatingMode mode)
        {
            this._lopwlINT = 0;
            
            int dataIndex = 0;

            bool isAllPass = true;

            this._channel = channel;

            List<float> lstResult = new List<float>();

            foreach (DeviceVerifySpecData data in this._dvSpec.Data)
            {
                if (!data.IsEnable)
                    continue;
                
                lstResult.Clear();

                if (keyDic.ContainsKey(data.KeyName))
                {
                    dataIndex = keyDic[data.KeyName];

                    switch (mode)
                    {
                        case SpecCalculatingMode.BiasRegister:
                            {
                                for (int i = 0; i < results.Count; i++)
                                {
                                    lstResult.Add(results[i][dataIndex]);
                                }
                                
                                data.Bias = lstResult.Average();

                                break;
                            }
                        case SpecCalculatingMode.CreateNewCoef:
                            {
                                for (int i = 0; i < results.Count; i++)
                                {
                                    lstResult.Add(results[i][dataIndex]);
                                }
                                
                                data.ResultAverageValue = lstResult.Average();

                                data.ResultMaxValue = lstResult.Max();

                                data.ResultMinValue = lstResult.Min();

                                data.Calculate();

                                break;
                            }
                        case SpecCalculatingMode.ApplyCoef:
                            {
                                for (int i = 0; i < results.Count; i++)
                                {
                                    double result = data.Gain * results[i][dataIndex] + data.Offset;

                                    lstResult.Add((float)result);
                                }

                                data.ResultAverageValue = lstResult.Average();

                                data.ResultMaxValue = lstResult.Max();

                                data.ResultMinValue = lstResult.Min();

                                data.Calculate();

                                break;
                            }
                    }

                    isAllPass &= data.IsPass;
                }
                else
                {
                    this._isPass = false;

                    return false;
                }
            }

            // LOPWL INT
            foreach (KeyValuePair<string, int> kvp in keyDic)
            {
                if (kvp.Key.Contains(EOptiMsrtType.INT.ToString()))
                {
                    this._lopwlINT = (int)results[0][kvp.Value];

                    break;
                }
            }

            this._isPass = isAllPass;

            return true;
        }

        public void StartHistoryRecord()
        {
            foreach (KeyValuePair<string, List<string>> pair in this._dicResultHistory)
            {
                pair.Value.Clear();
            }

            string itemKey = this._reportDataHead[0];
            string condiKey = this._reportDataHead[1];
            string biasKey = this._reportDataHead[2];
            string specKey = this._reportDataHead[3];
            string gainBKey = this._reportDataHead[4];
            string offsetBKey = this._reportDataHead[5];

            int index = 0;

            foreach (DeviceVerifySpecData data in this._dvSpec.Data)
            {
                if (!data.IsEnable)
                    continue;

                this._dicResultHistory[itemKey].Add(this._lstNameDisplay[index]);
                this._dicResultHistory[condiKey].Add(this._lstCondiDisplay[index]);
                this._dicResultHistory[biasKey].Add(data.Bias.ToString(data.Format));
                this._dicResultHistory[specKey].Add(data.Spec.ToString(data.Format));
                this._dicResultHistory[gainBKey].Add(data.Gain.ToString("0.0000"));
                this._dicResultHistory[offsetBKey].Add(data.Offset.ToString("0.0000"));

                index++;
            }
        }

        public void AddHistoryData()
        {
            string avgKey = string.Format("SMD{0}_Avg", this._channel + 1);
            string maxKey = string.Format("SMD{0}_Max", this._channel + 1);
            string minKey = string.Format("SMD{0}_Min", this._channel + 1);
            string diffKey = string.Format("SMD{0}_Diff", this._channel + 1);
            string pfKey = string.Format("SMD{0}_PF", this._channel + 1);
            string gainAKey = this._reportDataHead[6];
            string offsetAKey = this._reportDataHead[7];

            foreach (DeviceVerifySpecData data in this._dvSpec.Data)
            {
                if (!data.IsEnable)
                    continue;

                this._dicResultHistory[avgKey].Add(data.ResultAverageValue.ToString(data.Format));
                this._dicResultHistory[maxKey].Add(data.ResultMaxValue.ToString(data.Format));
                this._dicResultHistory[minKey].Add(data.ResultMinValue.ToString(data.Format));
                this._dicResultHistory[diffKey].Add(data.Diff.ToString(data.Format));

                if (data.IsPass)
                {
                    this._dicResultHistory[pfKey].Add("PASS");
                }
                else
                {
                    this._dicResultHistory[pfKey].Add("NG");
                }

                if (this._channel == this._stdSmdChannel)
                {
                    this._dicResultHistory[gainAKey].Add(data.Gain.ToString("0.0000"));
                    this._dicResultHistory[offsetAKey].Add(data.Offset.ToString("0.0000"));
                }
            }
        }

        public void SaveHistoryDataToFile(string path, string fileNameWithExt)
        {
            List<string[]> strArrayList = new List<string[]>(300);
            List<string> lineStr = new List<string>(50);

            try
            {
                //-----------------------------------------------------------------------
                // (1) Title Section
                //-----------------------------------------------------------------------
                lineStr.Clear();
                lineStr.Add("Test Date");
                lineStr.Add(DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
                strArrayList.Add(lineStr.ToArray());

                lineStr.Clear();
                lineStr.Add("Register Date");
                lineStr.Add(this.Data.BiasRegisterDate.ToString("yyyy/MM/dd HH:mm"));
                strArrayList.Add(lineStr.ToArray());

                lineStr.Clear();
                lineStr.Add("STD SMD");
                lineStr.Add("SMD" + (this._stdSmdChannel + 1).ToString());
                strArrayList.Add(lineStr.ToArray());

                lineStr.Clear();
                lineStr.Add("");
                strArrayList.Add(lineStr.ToArray());

                //-----------------------------------------------------------------------
                // (2) Write Data To File
                //-----------------------------------------------------------------------
                // Add Report Head
                int count = 0;

                List<string[]> tempList = new List<string[]>();

                for (int i = 0; i < this._reportDataHead.Length; i++)
                {
                    count = 0;
                    
                    string strHead = this._reportDataHead[i];

                    string[] strArray = new string[this._dicResultHistory[strHead].Count + 1];  // +1 Head

                    strArray[0] = strHead;

                    foreach (string str in this._dicResultHistory[strHead])
                    {
                        strArray[count + 1] = str;
                        
                        count++;
                    }

                    tempList.Add(strArray);
                }

                strArrayList.AddRange(CSVUtil.Transpose(tempList));

                if (!CSVUtil.WriteCSV(Path.Combine(path, fileNameWithExt), strArrayList))
                {
                    return;
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be opened because it was locked by another process. {0}", e.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        #endregion
    }

    public enum SpecCalculatingMode : int
    {
        None = 0,
        CreateNewCoef = 1,
        ApplyCoef = 2,
        BiasRegister = 3,
    }


}
