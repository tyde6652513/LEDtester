using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
    public enum ESerializeFormatter
    {
        XML=0,
        BINARY=1,
    }

    public class CalibrationCtrl
    {
        private Dictionary<string, GainOffsetData> _dicGainOffset = new Dictionary<string, GainOffsetData>();

        private Dictionary<string, LOPWLParameter> _dicLOPWLParameter = new Dictionary<string, LOPWLParameter>();

        private CalibarteData _data;

        private Dictionary<string, Dictionary<int, double[]>> _coefTableDic;

        private List<TestItemData> _testItemList;

        private ECalLookupWave _calLookupWave;

        private ECalBaseWave _calByWLType;

        public CalibrationCtrl()
        {
            this._data = new CalibarteData();

            this._dicGainOffset = new Dictionary<string, GainOffsetData>();

            this._dicLOPWLParameter = new Dictionary<string, LOPWLParameter>();

            this._coefTableDic = new Dictionary<string, Dictionary<int, double[]>>();

         //   this._coefTable = new Dictionary<int, double[]>();

            this._testItemList = new List<TestItemData>();

        }

        public void getProductCalibrateData(ProductData pd)
        {
            if (_data != null)
            {
                this._testItemList.Clear();

                if (pd.TestCondition.TestItemArray != null)
                {
                    this._testItemList.AddRange(pd.TestCondition.TestItemArray);

                    this._calByWLType = pd.TestCondition.CalByWave;

                    this._calLookupWave = pd.TestCondition.CalLookupWave;

                    this._data.ChuckLOPCorrectArray = pd.ChuckLOPCorrectArray;

                    this._data.ChuckResistanceCorrectArray = pd.ChuckResistanceCorrectArray;

                    this._data.Resistance = pd.Resistance;

                    this.ReadProductCoefTables();
                }
            } 
        }

        public bool LoadFormCALFile(string path, string fileName ,ESerializeFormatter format )
        {
            string pathAndFile = Path.Combine(path, fileName);

            if (!System.IO.File.Exists(pathAndFile))
                return false;

            this._data = null;

            try
            {
                if(format == ESerializeFormatter.XML)
                {
                    this._data = MPI.Xml.XmlFileSerializer.Deserialize(typeof(CalibarteData), pathAndFile) as CalibarteData;
                }
                else if(format == ESerializeFormatter.BINARY)
                {

                    System.Runtime.Serialization.IFormatter binFmt = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    Stream s = File.Open(Path.Combine(path, fileName), FileMode.Open);

                    this._data = (CalibarteData)binFmt.Deserialize(s);
                }

                //this._data = MPI.Xml.XmlFileSerializer.Deserialize(typeof(CalibarteData), pathAndFile) as CalibarteData;

                if (_data == null)
                {
                    return false;
                }

                for (int i = 0; i < _data.GainOffsetValue.Count; i++)
                {
                    this._dicGainOffset.Add(this._data.GainOffsetValue[i].KeyName ,this._data.GainOffsetValue[i]);
                }

                for (int i = 0; i < _data.LOPWLParameters.Count; i++)
                {
                    this._dicLOPWLParameter.Add(_data.LOPWLParameters[i].KeyName, this._data.LOPWLParameters[i]);
                }

                return true;

            }
            catch
            {
                return false;
            }
        }

        public bool SaveAsFile(string path, string fileName, ESerializeFormatter format)
        {
            if (_data == null)
                return false;

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            try
            {
                this._data.GainOffsetKeyName.Clear();

                this._data.GainOffsetValue.Clear();

                foreach (var item in _dicGainOffset)
                {
                    //  this._data.GainOffsetKeyName.Add(item.Key);

                    this._data.GainOffsetValue.Add(item.Value);
                }

                this._data.LOPWLParameters.Clear();

                foreach (var item in _dicLOPWLParameter)
                {
                    item.Value.KeyName = item.Key;

                    this._data.LOPWLParameters.Add(item.Value);
                }

                if (format == ESerializeFormatter.XML)
                {
                    MPI.Tester.XML.XmlFileSerializer.Serialize2(_data, Path.Combine(path, fileName));
                }
                else if (format == ESerializeFormatter.BINARY)
                {
                    System.Runtime.Serialization.IFormatter binFmt = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    Stream s = File.Open(Path.Combine(path, fileName), FileMode.Create);

                    binFmt.Serialize(s, this._data);

                    s.Close();
                }

                //  MPI.Xml.XmlFileSerializer.Serialize(_data, Path.Combine(path, fileName));

                return true;
            }
            catch
            {
                return true;
            }
        }

        public bool ExtractCalibrateDataAndSave(ProductData pd, string path, string fileName)
        {
            if(pd.TestCondition.TestItemArray == null)
            {
                return false;
            }

            this._data.Polarity = pd.TestCondition.ChipPolarity;

            this._data.ProductFilterWheelPos = pd.ProductFilterWheelPos;

            this._data.Resistance = pd.Resistance;

            this._data.ChuckResistanceCorrectArray = pd.ChuckResistanceCorrectArray;

            this._dicGainOffset = new Dictionary<string, GainOffsetData>();

            this._dicLOPWLParameter = new Dictionary<string, LOPWLParameter>();

            foreach (TestItemData item in pd.TestCondition.TestItemArray)
            {
                if (item.Type == ETestType.LOPWL)
                {
                    LOPWLParameter paramater = new LOPWLParameter();

                    paramater.CoefTable = (item as LOPWLTestItem).CoefTable;

                    paramater.CoefWLResolution = (item as LOPWLTestItem).CoefWLResolution;

                    paramater.CoefTable = (item as LOPWLTestItem).CoefTable;

                    paramater.OptiSetting = (item as LOPWLTestItem).OptiSetting;

                    this._dicLOPWLParameter.Add(item.KeyName, paramater);

                }

                if (item.GainOffsetSetting == null)
                {
                    continue;
                }

                foreach (GainOffsetData gainOffset in item.GainOffsetSetting)
                {
                    if (gainOffset.IsEnable)
                    {
                        this._dicGainOffset.Add(gainOffset.KeyName, gainOffset);
                    }
                }
            }

            this.SaveAsFile(path, fileName, ESerializeFormatter.XML);

            return true;

        }

        public bool CatchDataFromUISetting(UISetting uiSetting,TesterSetting testerSystem)
        {
            this._data.MachineName = uiSetting.MachineName;

            this._data.CalibSpectrumFileName = uiSetting.ImportCalibSptDataFileName;

            this._data.ScanStartWL = testerSystem.OptiDevSetting.StartWavelength;

            this._data.ScanEndWL = testerSystem.OptiDevSetting.EndWavelength;

            return true;
        }

        public void CalibrateResistance(TestItemData item)
        {
            if (!item.IsEnable)
                return;

            if (item.MsrtResult == null)
                return;

            if (item.Type != ETestType.IF && item.Type != ETestType.LOPWL)
            {
                return;
            }

            if (item.Type == ETestType.IF)
            {
                for (int i = 0; i < item.MsrtResult.Length; i++)
                {
                    item.MsrtResult[i].RawValue = item.MsrtResult[i].RawValue - (item.ElecSetting[0].ForceValue * 0.001) * this._data.Resistance;
                }
            }

            if (item.Type == ETestType.LOPWL)
            {
                item.MsrtResult[(int)EOptiMsrtType.MVFLA].RawValue = item.MsrtResult[(int)EOptiMsrtType.MVFLA].RawValue - (item.ElecSetting[0].ForceValue * 0.001) * this._data.Resistance;
                item.MsrtResult[(int)EOptiMsrtType.MVFLB].RawValue = item.MsrtResult[(int)EOptiMsrtType.MVFLB].RawValue - (item.ElecSetting[0].ForceValue * 0.001) * this._data.Resistance;
            }
        }

        public void CalibrateChuckResistance(TestItemData item, int ChuckIndex)
        {
            if (!item.IsEnable)
                return;

            if (item.MsrtResult == null)
                return;

            if (item.Type != ETestType.IF && item.Type != ETestType.LOPWL)
            {
                return;
            }

            if (item.MsrtResult != null && this._data.ChuckResistanceCorrectArray != null)
            {
                int index = ChuckIndex - 1;

                if (index >= 0)
                {
                    if (item.Type == ETestType.IF)
                    {
                        item.MsrtResult[0].RawValue = item.MsrtResult[0].RawValue - (item.ElecSetting[0].ForceValue * 0.001) * this._data.ChuckResistanceCorrectArray[index];
                    }

                    if (item.Type == ETestType.LOPWL)
                    {
                        item.MsrtResult[(int)EOptiMsrtType.MVFLA].RawValue = item.MsrtResult[(int)EOptiMsrtType.MVFLA].RawValue - (item.ElecSetting[0].ForceValue * 0.001) * this._data.ChuckResistanceCorrectArray[index];
                        item.MsrtResult[(int)EOptiMsrtType.MVFLB].RawValue = item.MsrtResult[(int)EOptiMsrtType.MVFLB].RawValue - (item.ElecSetting[0].ForceValue * 0.001) * this._data.ChuckResistanceCorrectArray[index];
                    }
                }
            }
        }

        public bool CalibrateLOPWL(LOPWLTestItem optiTestItem, bool isCalibratePower)
        {
            bool rtn = true;

            rtn &= LookTableForWave(optiTestItem);

            if (isCalibratePower == true)
            {
                rtn &= LookTableForPower(optiTestItem);
            }

            return rtn;
        }

        public void CalibrateChuckLOP(int index, GainOffsetData[][] gainOffset, TestResultData[] optiResult)
        {
            if (index < 0 || index >= gainOffset.Length)
                return;

            // LOP 
            optiResult[(int)EOptiMsrtType.LOP].Value = Math.Pow(optiResult[(int)EOptiMsrtType.LOP].Value, 2.0d) * gainOffset[index][0].Square +
                                                                                       optiResult[(int)EOptiMsrtType.LOP].Value * gainOffset[index][0].Gain +
                                                                                       gainOffset[index][0].Offset;

            // WATT
            optiResult[(int)EOptiMsrtType.WATT].Value = Math.Pow(optiResult[(int)EOptiMsrtType.WATT].Value, 2.0d) * gainOffset[index][1].Square +
                                                                                        optiResult[(int)EOptiMsrtType.WATT].Value * gainOffset[index][1].Gain +
                                                                                        gainOffset[index][1].Offset;

            // LM
            optiResult[(int)EOptiMsrtType.LM].Value = Math.Pow(optiResult[(int)EOptiMsrtType.LM].Value, 2.0d) * gainOffset[index][2].Square +
                                                                                        optiResult[(int)EOptiMsrtType.LM].Value * gainOffset[index][2].Gain +
                                                                                        gainOffset[index][2].Offset;

        }
       
        //private void LoadSystemCoeffByWave(string fileName)
        //{
        //    if (!File.Exists(fileName))
        //    {
        //        return;
        //    }

        //    Console.WriteLine("[CondCtrl], Enable Hidden Coeff");


        //    List<string[]> readData = CSVUtil.ReadCSV(fileName);

        //    int row;
        //    int startWave = -1;

        //    for (row = 0; row < readData.Count; row++)
        //    {
        //        if (readData[row][0] == "CoefTable")
        //        {
        //            startWave = row + 2;
        //            break;
        //        }
        //    }

        //    for (row = startWave; row < readData.Count; row++)
        //    {
        //        string[] stringData = readData[row];

        //        if (stringData[0] == "SectionEnd")
        //        {
        //            break;
        //        }

        //        try
        //        {
        //            double[] parseData = ParseStrToDouble(stringData, 0, 7);
        //            this._systemCoefTable.Add((int)(1000 * parseData[0]), parseData);
        //        }
        //        catch
        //        { }

        //    }
        //    Console.WriteLine("[CondCtrl], Enable Hidden  Coeff, SUCCESS ");
        //}

        private void ReadProductCoefTables()
        {
            int keyWL;

            this._coefTableDic.Clear();

            foreach (TestItemData item in this._testItemList)
            {
                if (item is LOPWLTestItem)
                {
                    Dictionary<int, double[]> coefTable = new Dictionary<int, double[]>();

                    for (int i = 0; i < (item as LOPWLTestItem).CoefTable.Length; i++)
                    {
                        keyWL = (int)(item as LOPWLTestItem).CoefTable[i][0] * 1000;		// pm for wavelength	   

                        coefTable.Add(keyWL, (item as LOPWLTestItem).CoefTable[i]);
                    }
                    this._coefTableDic.Add(item.KeyName, coefTable);
                }
            }
            //this.LoadSystemCoeffByWave(Path.Combine(Constants.Paths.PRODUCT_FILE02, "SysCoeff.cal"));
        }

        private bool LookTableForWave(LOPWLTestItem item)
        {
            double targetWL = 0.0d;
            double WL1 = 0.0d;
            double WL2 = 0.0d;
            int keyWL1 = 0;
            int keyWL2 = 0;
            int index = 0;

            if (this._coefTableDic.Count == 0)
                return false;

            Dictionary<int, double[]> lookupTable = this._coefTableDic[item.KeyName];

            if (lookupTable == null)
                return false;

            double[] coef1 = null;
            double[] coef2 = null;

            if (_calByWLType == ECalBaseWave.By_WLP)
            {
                if (this._calLookupWave == ECalLookupWave.Original)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLP].RawValue;       // raw WLP
                }
                else if (_calLookupWave == ECalLookupWave.Corrected)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLP].Value;          // WLP, calibrate by Gain & Offset
                }
            }
            else if (_calByWLType == ECalBaseWave.By_WLD)
            {
                if (this._calLookupWave == ECalLookupWave.Original)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLD].RawValue;       // raw WLD
                }
                else if (this._calLookupWave == ECalLookupWave.Corrected)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLD].Value;          // WLD, calibrate by Gain & Offset
                }
            }
            else if (_calByWLType == ECalBaseWave.By_WLC)
            {
                if (this._calLookupWave == ECalLookupWave.Original)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLC].RawValue;       // WLC, calibrate by Gain & Offset
                }
                else if (this._calLookupWave == ECalLookupWave.Corrected)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLC].Value;          // lookup WLC    
                }
            }

            if ((item.CoefWLResolution * 1000000) == 0)
                return false;

            index = (int)(Math.Floor((targetWL - item.CoefStartWL) / item.CoefWLResolution));

            WL1 = item.CoefStartWL + index * item.CoefWLResolution;

            WL2 = WL1 + item.CoefWLResolution;

            keyWL1 = Convert.ToInt32(WL1 * 1000);   // look for by wavelength pm

            keyWL2 = Convert.ToInt32(WL2 * 1000);   // look for by wavelength pm     

            if ((!lookupTable.ContainsKey(keyWL1)) && (!lookupTable.ContainsKey(keyWL2)))
                return false;

            coef1 = lookupTable[keyWL1];

            coef2 = lookupTable[keyWL2];

            // WLP
            item.MsrtResult[(int)EOptiMsrtType.WLP].Value = item.MsrtResult[(int)EOptiMsrtType.WLP].Value + InterGain(WL1, coef1[1], WL2, coef2[1], targetWL);

            // WLD
            item.MsrtResult[(int)EOptiMsrtType.WLD].Value = item.MsrtResult[(int)EOptiMsrtType.WLD].Value + InterGain(WL1, coef1[2], WL2, coef2[2], targetWL);

            // WLC
            item.MsrtResult[(int)EOptiMsrtType.WLC].Value = item.MsrtResult[(int)EOptiMsrtType.WLC].Value + InterGain(WL1, coef1[3], WL2, coef2[3], targetWL);

            // HW
            item.MsrtResult[(int)EOptiMsrtType.HW].Value = item.MsrtResult[(int)EOptiMsrtType.HW].Value + InterGain(WL1, coef1[7], WL2, coef2[7], targetWL);

            return true;
        }

        private bool LookTableForPower(LOPWLTestItem item)
        {
            double targetWL = 0.0d;
            double WL1 = 0.0d;
            double WL2 = 0.0d;
            int keyWL1 = 0;
            int keyWL2 = 0;
            int index = 0;

            if (this._coefTableDic.Count == 0)
                return false;

            Dictionary<int, double[]> lookupTable = this._coefTableDic[item.KeyName];

            if (lookupTable == null)
                return false;

            double[] coef1 = null;
            double[] coef2 = null;

            if (_calByWLType == ECalBaseWave.By_WLP)
            {
                if (this._calLookupWave == ECalLookupWave.Original)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLP].RawValue;       // raw WLP
                }
                else if (_calLookupWave == ECalLookupWave.Corrected)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLP].Value;          // WLP, calibrate by Gain & Offset
                }
            }
            else if (_calByWLType == ECalBaseWave.By_WLD)
            {
                if (this._calLookupWave == ECalLookupWave.Original)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLD].RawValue;       // raw WLD
                }
                else if (this._calLookupWave == ECalLookupWave.Corrected)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLD].Value;          // WLD, calibrate by Gain & Offset
                }
            }
            else if (_calByWLType == ECalBaseWave.By_WLC)
            {
                if (this._calLookupWave == ECalLookupWave.Original)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLC].RawValue;       // WLC, calibrate by Gain & Offset
                }
                else if (this._calLookupWave == ECalLookupWave.Corrected)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLC].Value;          // lookup WLC    
                }
            }

            if ((item.CoefWLResolution * 1000000) == 0)
                return false;

            index = (int)(Math.Floor((targetWL - item.CoefStartWL) / item.CoefWLResolution));

            WL1 = item.CoefStartWL + index * item.CoefWLResolution;
            WL2 = WL1 + item.CoefWLResolution;

            keyWL1 = Convert.ToInt32(WL1 * 1000);
            keyWL2 = Convert.ToInt32(WL2 * 1000);


            if ((!lookupTable.ContainsKey(keyWL1)) && (!lookupTable.ContainsKey(keyWL2)))
            {
                return false;
            }

            coef1 = lookupTable[keyWL1];

            coef2 = lookupTable[keyWL2];

            //if (this._systemCoefTable.Count != 0)
            //{
            //    if ((this._systemCoefTable.ContainsKey(keyWL1)) && (this._systemCoefTable.ContainsKey(keyWL2)))
            //    {
            //        double[] sCoef1 = this._systemCoefTable[keyWL1];
            //        double[] sCoef2 = this._systemCoefTable[keyWL2];
            //        item.MsrtResult[(int)EOptiMsrtType.WATT].Value = item.MsrtResult[(int)EOptiMsrtType.WATT].Value * InterGain(WL1, sCoef1[5], WL2, sCoef2[5], targetWL);
            //        // item.MsrtResult[(int)EOptiMsrtType.LM].Value = item.MsrtResult[(int)EOptiMsrtType.LM].Value * InterGain(WL1, coef1[6], WL2, coef2[6], targetWL);
            //    }
            //}
            // LOP
            item.MsrtResult[(int)EOptiMsrtType.LOP].Value = item.MsrtResult[(int)EOptiMsrtType.LOP].Value * InterGain(WL1, coef1[4], WL2, coef2[4], targetWL);

            // Watt
            item.MsrtResult[(int)EOptiMsrtType.WATT].Value = item.MsrtResult[(int)EOptiMsrtType.WATT].Value * InterGain(WL1, coef1[5], WL2, coef2[5], targetWL);

            // LM
            item.MsrtResult[(int)EOptiMsrtType.LM].Value = item.MsrtResult[(int)EOptiMsrtType.LM].Value * InterGain(WL1, coef1[6], WL2, coef2[6], targetWL);

            return true;
        }

        private double InterGain(double x1, double y1, double x2, double y2, double x)
        {
            double slop = 0.0d;

            if ((x2 - x1) != 0)
            {
                slop = (y2 - y1) / (x2 - x1);
                return slop * (x - x1) + y1;
            }

            return 0.0d;
        }

        private double[] ParseStrToDouble(string[] strData, int startiIndex, int endIndex)
        {
            if (strData == null || startiIndex < 0 || endIndex < 0 || (startiIndex > endIndex))
            {
                return (new double[1]);
            }

            double[] data = new double[endIndex - startiIndex + 1];
            double parseData = 0.0d;

            for (int i = 0; i < data.Length; i++)
            {
                if (Double.TryParse(strData[i + startiIndex], out parseData))
                {
                    data[i] = parseData;
                }
                else
                {
                    data[i] = 0.0d;
                }
            }

            return data;
        }

        #region <<<  Public Proerty >>> 

        public CalibarteData Data
        {
            get { return this._data; }
            set { this._data = value; }
        }

        public Dictionary<string, GainOffsetData> DicGainOffset
        {
            get { return this._dicGainOffset; }
            set { this._dicGainOffset = value; }
        }

        public Dictionary<string, LOPWLParameter> DicLOPWLParameter
        {
            get { return this._dicLOPWLParameter; }
            set { this._dicLOPWLParameter = value; }
        }
       
        public GainOffsetData this[string key]
        {
            get
            {
                if (this._dicGainOffset.ContainsKey(key))
                {
                    return this._dicGainOffset[key];
                }
                else
                {
                    return null;
                }
            }
        }

       #endregion
    }

}
