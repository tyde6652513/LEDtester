using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
    [Serializable]
    public class ConditionCtrl : ICloneable
    {
        private object _lockObj;

        private static ConditionData _data;
        private string[][] _msrtItemNames;
        private string[][] _opticalItemNames;
        private string[][] _sweepItemNames;
        private string[][] _mapItemNamess;

        //private TestItemData _createdTestItem;
        private List<TestItemData> _testItemList;
        private List<TestResultData> _msrtResultList;
        private List<TestResultData> _msrtAndSystemResultList;
        private Dictionary<string, int> _subItemCountDic;

        private Dictionary<int, double[]> _coefTable;
        private Dictionary<string, Dictionary<int, double[]>> _coefTableDic;

        private int[] _LOPItemPos;
        private int[] _LOPWLItemPos;

        private UserData _userData;
        private int[] _subItemCount;

        private ETesterFunctionType _testerFuncType;

        private Dictionary<int, double[]> _systemCoefTable;

        Dictionary<string, double> _msrtData = new Dictionary<string, double>();

        public ConditionCtrl()
        {
            this._lockObj = new object();

            this._testItemList = new List<TestItemData>();
            this._msrtResultList = new List<TestResultData>();
            this._msrtAndSystemResultList = new List<TestResultData>();
            this._subItemCountDic = new Dictionary<string, int>();

            this._coefTableDic = new Dictionary<string, Dictionary<int, double[]>>();
            this._coefTable = new Dictionary<int, double[]>();

            this._msrtItemNames = new string[2][] { new string[0], new string[0] };
            this._opticalItemNames = new string[2][] { new string[0], new string[0] };
            this._sweepItemNames = new string[2][] { new string[0], new string[0] };
            this._mapItemNamess = new string[2][] { new string[0], new string[0] };

            this._subItemCount = new int[Enum.GetNames(typeof(ETestType)).Length];
            this._systemCoefTable = new Dictionary<int, double[]>();

            this._testerFuncType = ETesterFunctionType.Single_Die;
        }

        #region >>> Public Property <<<

        public ConditionData Data
        {
            get { return _data; }
            //set { lock (this._lockObj) { _data = value; } }              
        }

        public TestResultData[] MsrtResultArray
        {
            get { return this._msrtResultList.ToArray(); }  // Roy, 20141202
        }

        public TestResultData[] MsrtAndSystemResultList
        {
            get { return this.GetMsrtAndSystemResultArray(); }
        }

        //public string[][] MsrtItemNames
        //{
        //    get { return this._msrtItemNames; }
        //}

        public string[][] OpticalItemNames
        {
            get { return this._opticalItemNames; }
        }

        public string[][] SweepItemNames
        {
            get { return this._sweepItemNames; }
        }

        //public string[][] MapItemNames
        //{
        //    get{ return this._mapItemNamess; }
        //}

        #endregion

        #region >>> Private Method <<<

        private TestResultData[] GetMsrtResultArray()
        {
            if (this._testItemList == null || this._testItemList.Count == 0)
                return null;

            this._msrtResultList.Clear();

            if (this._testItemList.Count > 0)
            {
                foreach (TestItemData item in this._testItemList)
                {
                    if (item.MsrtResult != null)
                    {
                        this._msrtResultList.AddRange(item.MsrtResult);
                    }
                }
            }

            return this._msrtResultList.ToArray();
        }

        private TestResultData[] GetMsrtAndSystemResultArray()
        {
            this._msrtAndSystemResultList.Clear();

            /////////////////////////////////////////////////////////////////////////////////
            // Add TestResultData from "ESysResultItem"
            /////////////////////////////////////////////////////////////////////////////////
            string[] sysResult = Enum.GetNames(typeof(ESysResultItem));

            TestResultData[] resultArray = new TestResultData[sysResult.Length];

            for (int i = 0; i < resultArray.Length; i++)
            {
                resultArray[i] = new TestResultData(sysResult[i], sysResult[i], "", "0");

                resultArray[i].IsSystemItem = true;

                if (resultArray[i].KeyName == "BIN")
                {
                    resultArray[i].IsEnableMapItem = true;
                }
                else
                {
                    resultArray[i].IsEnableMapItem = false;
                }
            }

            this._msrtAndSystemResultList.AddRange(resultArray);

            /////////////////////////////////////////////////////////////////////////////////
            // Add TestResultData from "EProberDataIndex"
            /////////////////////////////////////////////////////////////////////////////////
            foreach (string str in Enum.GetNames(typeof(EProberDataIndex)))
            {
                TestResultData data = new TestResultData(str, str, "", "0");

                data.IsSystemItem = true;

                data.IsEnableMapItem = false;

                if (str == "CHUCKX" || str == "CHUCKY" || str == "CHUCKZ" ||
                     str == "ES01" || str == "ES02" || str == "ES03" || str == "ES04" || str == "SEQUENCETIME")
                {
                    data.Formate = "0.0000";
                }
                else
                {
                    data.Formate = "0";
                }

                this._msrtAndSystemResultList.Add(data);
            }

            /////////////////////////////////////////////////////////////////////////////////
            // Add TestResultData from "TestItemData"
            /////////////////////////////////////////////////////////////////////////////////	
            if (this._testItemList != null && this._testItemList.Count != 0)
            {
                foreach (TestItemData item in this._testItemList)
                {
                    if (item.MsrtResult != null)
                    {
                        this._msrtAndSystemResultList.AddRange(item.MsrtResult);
                    }
                }
            }

            return this._msrtAndSystemResultList.ToArray();
        }

        private void ResetSubItemKeyName()
        {
            this._subItemCountDic.Clear();
            //this._coefTableDic.Clear();
            string key = string.Empty;

            if (this._testItemList == null)
                return;

            foreach (TestItemData item in this._testItemList)
            {
                key = item.Type.ToString();
                if (this._subItemCountDic.ContainsKey(key) == false)
                {
                    this._subItemCountDic.Add(key, 1);
                }
                else
                {
                    this._subItemCountDic[key]++;
                }
                // paul Delete
                // item.SubItemIndex = this._subItemCountDic[key] - 1;
                // item.Order = order;
                // order++;
            }

            if (this._subItemCountDic.ContainsKey(ETestType.LOP.ToString()))
            {
                this._LOPItemPos = new int[this._subItemCountDic[ETestType.LOP.ToString()]];
            }

            if (this._subItemCountDic.ContainsKey(ETestType.LOPWL.ToString()))
            {
                this._LOPWLItemPos = new int[this._subItemCountDic[ETestType.LOPWL.ToString()]];
            }

        }

        public void ReloadCoefTableList()
        {
            int keyWL;

            this._coefTableDic.Clear();

            foreach (TestItemData item in this._testItemList)
            {
                if (item is LOPWLTestItem)
                {
                    this._coefTable = new Dictionary<int, double[]>();
                    this._coefTable.Clear();

                    for (int i = 0; i < (item as LOPWLTestItem).CoefTable.Length; i++)
                    {
                        keyWL = (int)(item as LOPWLTestItem).CoefTable[i][0] * 1000;		// pm for wavelength	                 
                        this._coefTable.Add(keyWL, (item as LOPWLTestItem).CoefTable[i]);
                    }
                    this._coefTableDic.Add(item.KeyName, this._coefTable);
                }
                else if (item is VLRTestItem)
                {
                    this._coefTable = new Dictionary<int, double[]>();
                    this._coefTable.Clear();

                    for (int i = 0; i < (item as VLRTestItem).CoefTable.Length; i++)
                    {
                        keyWL = (int)(item as VLRTestItem).CoefTable[i][0] * 1000;		// pm for wavelength	                 
                        this._coefTable.Add(keyWL, (item as VLRTestItem).CoefTable[i]);
                    }
                    this._coefTableDic.Add(item.KeyName, this._coefTable);
                }
            }
            this.LoadSystemCoeffByWave(Path.Combine(Constants.Paths.PRODUCT_FILE02, "SysCoeff.cal"));
        }

        private void GetFinalKeyAndName()
        {
            List<string>[] msrtItemNames = new List<string>[2];
            List<string>[] opticalItems = new List<string>[2];
            List<string>[] sweepItems = new List<string>[2];
            List<string>[] livItems = new List<string>[2];

            for (int i = 0; i < 2; i++)
            {
                msrtItemNames[i] = new List<string>();
                msrtItemNames[i].Clear();
                opticalItems[i] = new List<string>();
                opticalItems[i].Clear();
                sweepItems[i] = new List<string>();
                sweepItems[i].Clear();
                livItems[i] = new List<string>();
                livItems[i].Clear();
            }

            if (this._testItemList == null)
            {
                //this._msrtItemNames = null;
                //this._sweepItemNames = null ;
                //this._opticalItemNames = null;
                return;
            }

            foreach (TestItemData item in this._testItemList)
            {
                switch (item.Type) // == ETestType.IVSWEEP || item.Type == ETestType.VISWEEP)
                {
                    case ETestType.IVSWEEP:
                    case ETestType.VISWEEP:
                    case ETestType.THY:
                    case ETestType.RTH:
                    case ETestType.VISCAN:
                    case ETestType.PIV:
                        sweepItems[0].Add(item.KeyName);
                        sweepItems[1].Add(item.Name);
                        break;
                    case ETestType.LOPWL:
                    case ETestType.OSA:
                        opticalItems[0].Add(item.KeyName);
                        opticalItems[1].Add(item.Name);
                        break;
                    case ETestType.VLR:
                        sweepItems[0].Add(item.KeyName);
                        sweepItems[1].Add(item.Name);
                        opticalItems[0].Add(item.KeyName);
                        opticalItems[1].Add(item.Name);
                        break;
                    default:
                        break;
                }

                if (item.MsrtResult != null)
                {
                    foreach (TestResultData data in item.MsrtResult)
                    {
                        msrtItemNames[0].Add(data.KeyName);
                        msrtItemNames[1].Add(data.Name);
                    }
                }
            }

            for (int j = 0; j < 2; j++)
            {
                this._msrtItemNames[j] = msrtItemNames[j].ToArray();
                this._sweepItemNames[j] = sweepItems[j].ToArray();
                this._opticalItemNames[j] = opticalItems[j].ToArray();
            }

            msrtItemNames[0].Add("BIN");
            msrtItemNames[1].Add("BIN");

            this._mapItemNamess[0] = msrtItemNames[0].ToArray();
            this._mapItemNamess[1] = msrtItemNames[1].ToArray();

        }

        private bool LookTableForWave(TestItemData item)
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

            if (_data.CalByWave == ECalBaseWave.By_WLP)
            {
                if (_data.CalLookupWave == ECalLookupWave.Original)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLP].RawValue;       // raw WLP
                }
                else if (_data.CalLookupWave == ECalLookupWave.Corrected)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLP].Value;          // WLP, calibrate by Gain & Offset
                }
            }
            else if (_data.CalByWave == ECalBaseWave.By_WLD)
            {
                if (_data.CalLookupWave == ECalLookupWave.Original)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLD].RawValue;       // raw WLD
                }
                else if (_data.CalLookupWave == ECalLookupWave.Corrected)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLD].Value;          // WLD, calibrate by Gain & Offset
                }
            }
            else if (_data.CalByWave == ECalBaseWave.By_WLC)
            {
                if (_data.CalLookupWave == ECalLookupWave.Original)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLC].RawValue;       // WLC, calibrate by Gain & Offset
                }
                else if (_data.CalLookupWave == ECalLookupWave.Corrected)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLC].Value;          // lookup WLC    
                }
            }

            // 20170725, Roy, laser 沒有 WLD
            if (Math.Abs(targetWL) > 2000.0d)
            {
                return false;
            }

            if (item is LOPWLTestItem)
            {
                if (((item as LOPWLTestItem).CoefWLResolution * 1000000) == 0)
                    return false;


                index = (int)(Math.Floor((targetWL - (item as LOPWLTestItem).CoefStartWL) / (item as LOPWLTestItem).CoefWLResolution));

                WL1 = (item as LOPWLTestItem).CoefStartWL + index * (item as LOPWLTestItem).CoefWLResolution;
                WL2 = WL1 + (item as LOPWLTestItem).CoefWLResolution;
            }
            else if (item is VLRTestItem)
            {
                if (((item as VLRTestItem).CoefWLResolution * 1000000) == 0)
                    return false;


                index = (int)(Math.Floor((targetWL - (item as VLRTestItem).CoefStartWL) / (item as VLRTestItem).CoefWLResolution));

                WL1 = (item as VLRTestItem).CoefStartWL + index * (item as VLRTestItem).CoefWLResolution;
                WL2 = WL1 + (item as VLRTestItem).CoefWLResolution;
            }

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

        private void LoadSystemCoeffByWave(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return;
            }

            Console.WriteLine("[CondCtrl], Enable Hidden Coeff");


            List<string[]> readData = CSVUtil.ReadCSV(fileName);

            int row;
            int startWave = -1;

            for (row = 0; row < readData.Count; row++)
            {
                if (readData[row][0] == "CoefTable")
                {
                    startWave = row + 2;
                    break;
                }
            }

            for (row = startWave; row < readData.Count; row++)
            {
                string[] stringData = readData[row];

                if (stringData[0] == "SectionEnd")
                {
                    break;
                }

                try
                {
                    double[] parseData = ParseStrToDouble2(stringData, 0, 7);
                    this._systemCoefTable.Add((int)(1000 * parseData[0]), parseData);
                }
                catch
                { }

            }
            Console.WriteLine("[CondCtrl], Enable Hidden  Coeff, SUCCESS ");
        }

        private bool LookTableForPower(TestItemData item)
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

            if (_data.CalByWave == ECalBaseWave.By_WLP)
            {
                if (_data.CalLookupWave == ECalLookupWave.Original)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLP].RawValue;       // raw WLP
                }
                else if (_data.CalLookupWave == ECalLookupWave.Corrected)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLP].Value;          // WLP, calibrate by Gain, Offset and lookup table
                }
            }
            else if (_data.CalByWave == ECalBaseWave.By_WLD)
            {
                if (_data.CalLookupWave == ECalLookupWave.Original)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLD].RawValue;       // raw WLD
                }
                else if (_data.CalLookupWave == ECalLookupWave.Corrected)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLD].Value;          // WLD, calibrate by Gain, Offset and lookup table
                }
            }
            else if (_data.CalByWave == ECalBaseWave.By_WLC)
            {
                if (_data.CalLookupWave == ECalLookupWave.Original)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLC].RawValue;       // WLC, calibrate by Gain, Offset and lookup table
                }
                else if (_data.CalLookupWave == ECalLookupWave.Corrected)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLC].Value;          // lookup WLC    
                }
            }

            if (item is LOPWLTestItem)
            {
                if (((item as LOPWLTestItem).CoefWLResolution * 1000000) == 0)
                    return false;

                index = (int)(Math.Floor((targetWL - (item as LOPWLTestItem).CoefStartWL) / (item as LOPWLTestItem).CoefWLResolution));

                WL1 = (item as LOPWLTestItem).CoefStartWL + index * (item as LOPWLTestItem).CoefWLResolution;
                WL2 = WL1 + (item as LOPWLTestItem).CoefWLResolution;
            }
            else if (item is VLRTestItem)
            {
                if (((item as VLRTestItem).CoefWLResolution * 1000000) == 0)
                    return false;

                index = (int)(Math.Floor((targetWL - (item as VLRTestItem).CoefStartWL) / (item as VLRTestItem).CoefWLResolution));

                WL1 = (item as VLRTestItem).CoefStartWL + index * (item as VLRTestItem).CoefWLResolution;
                WL2 = WL1 + (item as VLRTestItem).CoefWLResolution;
            }

            keyWL1 = Convert.ToInt32(WL1 * 1000);
            keyWL2 = Convert.ToInt32(WL2 * 1000);


            if ((!lookupTable.ContainsKey(keyWL1)) && (!lookupTable.ContainsKey(keyWL2)))
                return false;

            coef1 = lookupTable[keyWL1];
            coef2 = lookupTable[keyWL2];

            if (this._systemCoefTable.Count != 0)
            {
                if ((this._systemCoefTable.ContainsKey(keyWL1)) && (this._systemCoefTable.ContainsKey(keyWL2)))
                {
                    double[] sCoef1 = this._systemCoefTable[keyWL1];
                    double[] sCoef2 = this._systemCoefTable[keyWL2];
                    item.MsrtResult[(int)EOptiMsrtType.WATT].Value = item.MsrtResult[(int)EOptiMsrtType.WATT].Value * InterGain(WL1, sCoef1[5], WL2, sCoef2[5], targetWL);
                    // item.MsrtResult[(int)EOptiMsrtType.LM].Value = item.MsrtResult[(int)EOptiMsrtType.LM].Value * InterGain(WL1, coef1[6], WL2, coef2[6], targetWL);
                }
            }
            // LOP
            item.MsrtResult[(int)EOptiMsrtType.LOP].Value = item.MsrtResult[(int)EOptiMsrtType.LOP].Value * InterGain(WL1, coef1[4], WL2, coef2[4], targetWL);

            // Watt
            item.MsrtResult[(int)EOptiMsrtType.WATT].Value = item.MsrtResult[(int)EOptiMsrtType.WATT].Value * InterGain(WL1, coef1[5], WL2, coef2[5], targetWL);

            // LM
            item.MsrtResult[(int)EOptiMsrtType.LM].Value = item.MsrtResult[(int)EOptiMsrtType.LM].Value * InterGain(WL1, coef1[6], WL2, coef2[6], targetWL);

            return true;
        }

        private bool LookTableForPower(int itemPos, int itemPos2)
        {
            double targetWL = 0.0d;
            double WL1 = 0.0d;
            double WL2 = 0.0d;
            int keyWL1 = 0;
            int keyWL2 = 0;
            int index = 0;

            LOPWLTestItem item = null;
            LOPTestItem powerItem = null;

            if (itemPos < this._testItemList.Count && (this._testItemList[itemPos] is LOPWLTestItem))
            {
                item = (this._testItemList[itemPos] as LOPWLTestItem);
            }
            else
            {
                return false;
            }


            if (itemPos2 < this._testItemList.Count && (this._testItemList[itemPos2] is LOPTestItem))
            {
                powerItem = (this._testItemList[itemPos2] as LOPTestItem);
            }


            if (this._coefTableDic.Count == 0)
                return false;

            Dictionary<int, double[]> lookupTable = this._coefTableDic[item.KeyName];

            if (lookupTable == null)
                return false;

            double[] coef1 = null;
            double[] coef2 = null;

            if (_data.CalByWave == ECalBaseWave.By_WLP)
            {
                if (_data.CalLookupWave == ECalLookupWave.Original)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLP].RawValue;       // raw WLP
                }
                else if (_data.CalLookupWave == ECalLookupWave.Corrected)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLP].Value;          // lookup WLP
                }
            }
            else if (_data.CalByWave == ECalBaseWave.By_WLD)
            {
                if (_data.CalLookupWave == ECalLookupWave.Original)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLD].RawValue;       // raw WLD
                }
                else if (_data.CalLookupWave == ECalLookupWave.Corrected)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLD].Value;          // lookup WLD
                }
            }
            else if (_data.CalByWave == ECalBaseWave.By_WLC)
            {
                if (_data.CalLookupWave == ECalLookupWave.Original)
                {
                    targetWL = item.MsrtResult[(int)EOptiMsrtType.WLC].RawValue;       // raw WLC    
                }
                else if (_data.CalLookupWave == ECalLookupWave.Corrected)
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
                return false;

            coef1 = lookupTable[keyWL1];
            coef2 = lookupTable[keyWL2];

            if (powerItem == null)
            {
                // LOP
                item.MsrtResult[(int)EOptiMsrtType.LOP].Value = item.MsrtResult[(int)EOptiMsrtType.LOP].Value * InterGain(WL1, coef1[4], WL2, coef2[4], targetWL);

                // Watt
                item.MsrtResult[(int)EOptiMsrtType.WATT].Value = item.MsrtResult[(int)EOptiMsrtType.WATT].Value * InterGain(WL1, coef1[5], WL2, coef2[5], targetWL);

                // LM
                item.MsrtResult[(int)EOptiMsrtType.LM].Value = item.MsrtResult[(int)EOptiMsrtType.LM].Value * InterGain(WL1, coef1[6], WL2, coef2[6], targetWL);
            }
            else
            {
                // RAW
                powerItem.MsrtResult[0].Value = powerItem.MsrtResult[0].Value * InterGain(WL1, coef1[4], WL2, coef2[4], targetWL);

                // mcd
                powerItem.MsrtResult[1].Value = powerItem.MsrtResult[1].Value * InterGain(WL1, coef1[4], WL2, coef2[4], targetWL);

                // mW
                powerItem.MsrtResult[2].Value = powerItem.MsrtResult[2].Value * InterGain(WL1, coef1[5], WL2, coef2[5], targetWL);

            }

            return true;
        }

        private void ModifyAllItemNames()
        {
            this.ModifyElecSettingName();
            this.ModifyGainSettingName();//必須先做，否則IsNewCreate會先被洗掉
            this.ModifyMsrtItemName();
            
            this.AutoOrderMsrtItemIndex();
            this.AutoRenameMsrtItemAndGianItem();
        }

        private void ModifyElecSettingName()
        {
            foreach (TestItemData item in this._testItemList)
            {
                // Modify the name of "testItem" by "UserData" definition
                if (this._userData != null && this._userData.TestItemNameDic.ContainsKey(item.KeyName))
                {
                    item.Name = this._userData.TestItemNameDic[item.KeyName];
                }
                else // Modify the name of  "testItem" by default rule ( Name = "KeyName Letter" and "KeyName Number" )
                {
                    item.Name = UserData.ExtractKeyNameLetter(item.KeyName) + UserData.ExtractKeyNameNumber(item.KeyName).ToString();
                }

                // Modify the name of "ElecSetting". [ force electric parameters ]
                if (item.ElecSetting != null)
                {
                    foreach (ElectSettingData setting in item.ElecSetting)
                    {
                        if (setting.KeyName.Length == 0)
                            continue;

                        // Modify the  name of "ElecSetting" by "UserData" definition
                        if (this._userData != null && this._userData.ForceItemNameDic.ContainsKey(setting.KeyName))
                        {
                            setting.Name = this._userData.ForceItemNameDic[setting.KeyName];
                        }
                        else  // Modify the name of  "ElecSetting" by default rule ( Name = "KeyName Letter" and "KeyName Number" )
                        {
                            setting.Name = UserData.ExtractKeyNameLetter(setting.KeyName) + UserData.ExtractKeyNameNumber(setting.KeyName).ToString();
                        }
                    }
                }
            }
        }

        private void ModifyGainSettingName()
        {
            string gainStr = string.Empty;
            string keyNameLetter = string.Empty;

            foreach (TestItemData item in this._testItemList)
            {
                if (item.GainOffsetSetting != null && item.IsNewCreateItem)
                {
                    foreach (GainOffsetData data in item.GainOffsetSetting)
                    {
                        if (data.KeyName.Length == 0)
                            continue;

                        // Modify the "GainOffsetSetting" parameters setting by "UserData" definition
                        if (this._userData != null && this._userData[data.KeyName] != null)
                        {
                            data.IsEnable = true;
                            //data.IsEnable = data.IsEnable & this._userData[data.KeyName].IsEnable;
                            if (item.Type == ETestType.LIV)
                            {
                                data.Name = this._userData[data.KeyName].Name;

                                // 20180920
                                data.IsEnable = this._userData[data.KeyName].IsEnable;
                                data.IsVision = data.IsEnable;
                            }
                            else
                            {
                                data.IsEnable = this._userData[data.KeyName].IsEnable;
                                data.IsVision = true;
                                data.Name = this._userData[data.KeyName].Name;
                            }
                        }
                        else // Modify the "GainOffsetSetting" parameters setting by default rule
                        {
                            data.Name = UserData.ExtractKeyNameLetter(data.KeyName) + UserData.ExtractKeyNameNumber(data.KeyName).ToString();
                            keyNameLetter = UserData.ExtractKeyNameLetter(data.KeyName);

                            //if (	this._userData.ResultItemDic.ContainsKey(tempKeyName) ||
                            //        this._userData.ResultItemDic.ContainsKey(tempKeyName + "_1"))
                            if (this._userData[keyNameLetter] != null || this._userData[keyNameLetter + "_1"] != null)
                            {
                                data.IsEnable = true;
                                data.IsVision = true;
                            }
                            else
                            {
                                data.IsEnable = false;
                                data.IsVision = false;
                            }
                        }

                        gainStr = UserData.ExtractKeyNameLetter(data.KeyName);

                        if (gainStr == EOptiMsrtType.CIEz.ToString() ||
                            gainStr == EOptiMsrtType.INT.ToString() ||
                            gainStr == EOptiMsrtType.INTP.ToString() ||
                            //	gainStr == EOptiMsrtType.ST.ToString() ||
                            gainStr == EOptiMsrtType.STR.ToString() ||
                            gainStr == EOptiMsrtType.DWDWP.ToString() ||
                            gainStr == EOptiMsrtType.MVFLD.ToString() ||
                            gainStr == "PDCNT" ||
                            gainStr == ELaserMsrtType.Pop.ToString())
                        {
                            data.IsEnable = false;
                            data.IsVision = false;
                        }
                    }
                }
            }
        }

        private void ModifyMsrtItemName()
        {
            string keyNameLetter = string.Empty;

            Dictionary<string, bool> msrtItemEnable = new Dictionary<string, bool>();
            msrtItemEnable.Clear();

            foreach (TestItemData item in this._testItemList)
            {
                if (item.MsrtResult != null)
                {
                    foreach (TestResultData data in item.MsrtResult)
                    {
                        msrtItemEnable.Add(data.KeyName, data.IsEnable);

                        if (data.KeyName.Length == 0)
                            continue;

                        // Modify the "MsrtResult" parameters setting by "UserData" definition
                        //if (this._userData != null &&  this._userData.ResultItemDic.ContainsKey(data.KeyName))
                        if (this._userData != null && this._userData[data.KeyName] != null)
                        {
                            ////data.IsEnable = data.IsEnable & this._userData[data.KeyName].IsEnable;//會把UI設定的結果洗掉，不過目前只有LCR這種輸出結果較為不固定的有影響，因此先保留
                            //data.IsVision = true;
                            ////data.IsVision = data.IsEnable; //20180919
                            //if (data.Name == data.KeyName)
                            //{
                            //    data.Name = this._userData[data.KeyName].Name;
                            //}
                            //data.Formate = this._userData[data.KeyName].Formate;
                            //data.Unit = this._userData[data.KeyName].Unit;

                            data.IsVision = true;
                            if (item.IsNewCreateItem)
                            {
                                data.IsEnable = data.IsEnable & this._userData[data.KeyName].IsEnable;
                                //!DataCenter._uiSetting.UserDefinedData.IsPermitSetMsrtItemName
                                data.Name = this._userData[data.KeyName].Name;
                                data.Formate = this._userData[data.KeyName].Formate;
                                data.Unit = this._userData[data.KeyName].Unit;
                            }

                        }
                        else // Modify the "MsrtResult" parameters setting by default rule
                        {
                            data.Name = UserData.ExtractKeyNameLetter(data.KeyName) + UserData.ExtractKeyNameNumber(data.KeyName).ToString();
                            keyNameLetter = UserData.ExtractKeyNameLetter(data.KeyName);

                            //if ( this._userData.ResultItemDic.ContainsKey(tempKeyName) ||
                            //     this._userData.ResultItemDic.ContainsKey(tempKeyName + "_1") )
                            if (this._userData[keyNameLetter] != null || this._userData[keyNameLetter + "_1"] != null)
                            {
                                //data.IsEnable = true;
                                //data.IsVision = true;
                            }
                            else
                            {
                                data.IsEnable = false;
                                data.IsVision = false;
                            }
                        }

                        if (item.IsEnable && data.IsVision && data.IsEnable)
                        {
                            data.IsThisItemTested = true;
                        }
                        else
                        {
                            data.IsThisItemTested = false;
                        }
                    }
                }


                item.IsNewCreateItem = false;
            }
        }

        private void CorrectSubItemIndex(ETestType itemType, int delIndex)
        {
            foreach (TestItemData data in this._testItemList)
            {
                if (data.Type == itemType && data.SubItemIndex > delIndex)
                {
                    data.IsNewCreateItem = true;//否則刪除測試項目的時候 name = key
                    data.SubItemIndex -= 1;
                }
            }
        }

        private string[] ExportByChannelGainOffsetData()
        {
            List<string> gainList = new List<string>(50);

            StringBuilder sb = new StringBuilder();

            gainList.Clear();

            gainList.Add("ByChannelGainOffset");
            sb.Append("Channel,KeyName,Name,Sqaure,Gain,Offset");
            gainList.Add(sb.ToString());

            ChannelConditionTable conditionTable = _data.ChannelConditionTable;

            foreach (TestItemData item in this._testItemList)
            {
                if (item.GainOffsetSetting != null)
                {
                    sb.Clear();

                    foreach (GainOffsetData data in item.GainOffsetSetting)
                    {
                        if (!data.IsEnable || !data.IsVision)
                            continue;

                        for (uint channel = 0; channel < conditionTable.Channels.Length; channel++)
                        {
                            if (conditionTable.Channels[channel] == null)
                            {
                                continue;
                            }

                            string name = string.Empty;

                            double square = 0.0d;

                            double gain = 1.0d;

                            double offset = 1.0d;

                            string keyName = string.Empty;

                            if (!conditionTable.IsEnable)
                            {
                                name = data.Name;

                                square = data.Square;

                                gain = data.Gain;

                                offset = data.Offset;

                                keyName = data.KeyName;
                            }
                            else
                            {
                                GainOffsetData coef = conditionTable.Channels[channel].GetByChannelGainOffsetData(data.KeyName);

                                name = coef.Name;

                                square = coef.Square;

                                gain = coef.Gain;

                                offset = coef.Offset;

                                keyName = coef.KeyName;
                            }

                            sb.Append(channel);
                            sb.Append(",");
                            sb.Append(keyName);
                            sb.Append(",");
                            sb.Append(name);
                            sb.Append(",");
                            sb.Append(square.ToString());
                            sb.Append(",");
                            sb.Append(gain.ToString());
                            sb.Append(",");
                            sb.Append(offset.ToString());
                            sb.Append(",");

                            gainList.Add(sb.ToString());
                            sb.Clear();
                        }
                    }
                }
            }

            gainList.Add("SectionEnd");
            gainList.Add("");

            return gainList.ToArray();
        }

        private double UnitConvertFactor(EVoltUnit fromUnit, string toUnit)
        {
            double scale = 0.0d;

            if (Enum.IsDefined(typeof(EVoltUnit), toUnit))
            {
                EVoltUnit tmp = (EVoltUnit)Enum.Parse(typeof(EVoltUnit), toUnit, false);

                scale = Math.Pow(10.0d, fromUnit - tmp);
            }

            return scale;
        }

        private double UnitConvertFactor(EAmpUnit fromUnit, string toUnit)
        {
            double scale = 0.0d;

            if (Enum.IsDefined(typeof(EAmpUnit), toUnit))
            {
                EAmpUnit tmp = (EAmpUnit)Enum.Parse(typeof(EAmpUnit), toUnit, false);

                scale = Math.Pow(10.0d, fromUnit - tmp);
            }

            return scale;
        }

        public void AutoRenameMsrtItemAndGianItem()
        {
            if (_userData.IsAutoReNameVfItemOfLOPWL == false)
            {
                return;
            }

            int index = 1;

            foreach (TestItemData item in this._testItemList)
            {
                if (item.MsrtResult != null)
                {
                    if (item is IFTestItem || item is LOPWLTestItem)
                    {
                        foreach (TestResultData data in item.MsrtResult)
                        {
                            if (data.KeyName.Contains("MVF"))
                            {
                                data.Name = "VF" + index.ToString();

                                index++;
                            }
                            break;
                        }
                    }
                }
            }

            index = 1;

            foreach (TestItemData item in this._testItemList)
            {
                if (item.MsrtResult != null)
                {
                    if (item is IFTestItem || item is LOPWLTestItem)
                    {
                        foreach (GainOffsetData gain in item.GainOffsetSetting)
                        {
                            if (gain.KeyName.Contains("MVF"))
                            {
                                gain.Name = "VF" + index.ToString();

                                index++;
                            }
                            break;
                        }
                    }
                }
            }
        }

        public void AutoOrderMsrtItemIndex()
        {
            if (_userData.IsEnableAutoOrderResultItem == false)
            {
                return;
            }

            int index = 1;

            foreach (TestItemData item in this._testItemList)
            {
                if (item.MsrtResult != null)
                {
                    foreach (TestResultData data in item.MsrtResult)
                    {
                        if (data.IsEnable && data.IsVision)
                        {
                            if (data.KeyName.Contains("ST") || data.KeyName.Contains("INT"))
                            {
                                data.Index = 0;
                            }
                            else
                            {
                                data.Index = index;
                                index++;
                            }
                        }
                        else
                        {
                            data.Index = 0;
                        }

                    }
                }
            }
        }

        #endregion

        #region >>> Public Methods <<<

        public void SetData(ConditionData cond)
        {
            _data = cond;

            if (_data != null)
            {
                this._testItemList.Clear();

                this._msrtResultList.Clear();

                if (_data.TestItemArray != null)
                {
                    this._testItemList.AddRange(_data.TestItemArray);

                    this._subItemCount = new int[Enum.GetNames(typeof(ETestType)).Length];

                    for (int i = 0; i < this._testItemList.Count; i++)
                    {
                        int typeIndex;

                        if (this._testItemList[i].Type == ETestType.LCRSWEEP)
                        {
                            typeIndex = (int)ETestType.LCR;
                        }
                        else
                        {
                            typeIndex = (int)this._testItemList[i].Type;
                        }

                        this._testItemList[i].Order = i;
                        //this._subItemCount[(int)this._testItemList[i].Type]++;
                        this._subItemCount[typeIndex]++;
                    }

                    this.ReloadCoefTableList();

                    this.GetFinalKeyAndName();

                    // Roy, 20141202
                    if (this._testItemList.Count > 0)
                    {
                        foreach (TestItemData item in this._testItemList)
                        {
                            if (item.MsrtResult != null)
                            {
                                this._msrtResultList.AddRange(item.MsrtResult);
                            }
                        }
                    }
                }
                else
                {
                    this._subItemCount = new int[Enum.GetNames(typeof(ETestType)).Length];
                }
            }
        }

        public EErrorCode SetChannelData(ETesterFunctionType func, ETesterSequenceType type, int col, int row, bool isCreateNewTable)
        {
            GlobalFlag.IsProductChannelConditionNotMatch = false;

            this._testerFuncType = func;

            _data.ChannelConditionTable.IsEnable = true;

            if (isCreateNewTable)
            {
                _data.ChannelConditionTable = new ChannelConditionTable(col, row);

                if (func == ETesterFunctionType.Single_Die || func == ETesterFunctionType.Multi_Terminal)
                {
                    _data.ChannelConditionTable.IsEnable = false;
                }
            }
            else
            {
                if (_data.ChannelConditionTable == null)
                {
                    _data.ChannelConditionTable = new ChannelConditionTable(col, row);
                }

                switch (func)
                {
                    case ETesterFunctionType.Multi_Die:
                        {
                            if (_data.ChannelConditionTable.ColXCount != col || _data.ChannelConditionTable.RowYCount != row)
                            {
                                _data.ChannelConditionTable.IsEnable = false;  // Roy, 20140328, conditionTable row/col 與 mechineConfig row/col 不匹配, 則關閉 ConditionTable 功能

                                GlobalFlag.IsProductChannelConditionNotMatch = true;

                                return EErrorCode.ProductChannelConditionNotMatch;
                            }
                            break;
                        }
                    case ETesterFunctionType.Multi_Pad:
                        {
                            if (type == ETesterSequenceType.Parallel)
                            {
                                if (_data.ChannelConditionTable.ColXCount != col || _data.ChannelConditionTable.RowYCount != row)
                                {
                                    _data.ChannelConditionTable.IsEnable = false;  // Roy, 20140328, conditionTable row/col 與 mechineConfig row/col 不匹配, 則關閉 ConditionTable 功能

                                    return EErrorCode.ProductChannelConditionNotMatch;
                                }
                            }
                            break;
                        }
                    default:
                        {
                            _data.ChannelConditionTable.IsEnable = false;   // Roy, 20140328, Single-Die 關閉 ConditionTable 功能
                            break;
                        }
                }
            }

            return EErrorCode.NONE;
        }

        public void AddTestItem(TestItemData item)
        {
            this._testItemList.Add(item);
            //this._createdTestItem = null;

            int typeIndex;

            if (item.Type == ETestType.LCRSWEEP)
            {
                typeIndex = (int)ETestType.LCR;
            }
            else
            {
                typeIndex = (int)item.Type;
            }

            this._subItemCount[typeIndex]++;
            item.SubItemIndex = this._subItemCount[typeIndex] - 1;

            this.ResetSubItemKeyName();
            this.ReloadCoefTableList();
            this.ModifyAllItemNames();
            this.GetFinalKeyAndName();

            for (int i = 0; i < this._testItemList.Count; i++)
            {
                this._testItemList[i].Order = i;
            }
            _data.TestItemArray = this._testItemList.ToArray();


            SetData(_data);//20180116 David
        }

        public void RemoveTestItemAt(int index)
        {
            if (index < 0 || index > (this._testItemList.Count - 1))
                return;

            ETestType itemType = this._testItemList[index].Type;
            int subItemIndex = this._testItemList[index].SubItemIndex;

            int typeIndex;

            if (itemType == ETestType.LCRSWEEP)
            {
                typeIndex = (int)ETestType.LCR;
            }
            else
            {
                typeIndex = (int)itemType;
            }

            this._subItemCount[typeIndex]--;

            this._testItemList[index].Dispose();
            this._testItemList.RemoveAt(index);

            this.CorrectSubItemIndex(itemType, subItemIndex);

            this.ResetSubItemKeyName();
            this.ReloadCoefTableList();
            this.ModifyAllItemNames();
            this.GetFinalKeyAndName();

            for (int i = 0; i < this._testItemList.Count; i++)
            {
                this._testItemList[i].Order = i;
            }

            _data.TestItemArray = this._testItemList.ToArray();
            SetData(_data);//20180116 David
        }

        public void UpdateTestItem(int index, TestItemData item)
        {
            this._testItemList[index] = item;

            this.ModifyAllItemNames();

            _data.TestItemArray = this._testItemList.ToArray();

            SetData(_data);//20180116 David
        }

        public void InsertTestItem(int index, TestItemData item)
        {
            if (index < 0 || index > this._testItemList.Count)
                return;

            this._testItemList.Insert(index, item);
            //this._createdTestItem = null;


            int typeIndex;

            if (item.Type == ETestType.LCRSWEEP)
            {
                typeIndex = (int)ETestType.LCR;
            }
            else
            {
                typeIndex = (int)item.Type;
            }

            this._subItemCount[typeIndex]++;
            item.SubItemIndex = this._subItemCount[typeIndex] - 1;

            this.ResetSubItemKeyName();
            this.ReloadCoefTableList();
            this.ModifyAllItemNames();
            this.GetFinalKeyAndName();

            for (int i = 0; i < this._testItemList.Count; i++)
            {
                this._testItemList[i].Order = i;
            }

            _data.TestItemArray = this._testItemList.ToArray();
            SetData(_data);//20180116 David
        }

        public void ChangeOrder(int index, int director)
        {
            if (index >= 0 && index < this._testItemList.Count)
            {
                if (director == -1 && index >= 1)
                {
                    this._testItemList[index].Order += director;
                    this._testItemList[index + director].Order += (director * (-1));
                }

                if (director == 1 && index < (this._testItemList.Count - 1))
                {
                    this._testItemList[index].Order += director;
                    this._testItemList[index + director].Order += (director * (-1));
                }

                this._testItemList.Sort(delegate(TestItemData item01, TestItemData item02)
                            {
                                return Comparer<int>.Default.Compare(item01.Order, item02.Order);
                            }
                    );

                _data.TestItemArray = this._testItemList.ToArray();
            }
            SetData(_data);//20180116 David
        }

        public bool CalibrateLOPWL(TestItemData optiTestItem, bool isCalibratePower)
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



        public void CalibCalcTestItem(List<TestResultData> rsultList = null)
        {
            if (rsultList == null)
            {
                rsultList = this._msrtResultList;
            }

            // Find Msrt Result Value
            foreach (TestItemData item in this._testItemList)
            {
                if (item.Type == ETestType.CALC && item.IsEnable)
                {
                    item.IsTested = true;
                    foreach (var rData in item.MsrtResult)
                    {
                        rData.IsTested = true;
                    }

                    double val = 0;
                    SimpleCompiler sc = new SimpleCompiler();
                    sc.AssignFindDelegate(GetValue);
                    val = sc.RunCode((item as CALCTestItem).LocalAssemble, rsultList);
                    if (double.IsNaN(val))
                    {
                        item.IsTested = false;
                        foreach (var rData in item.MsrtResult)
                        {
                            rData.IsTested = false;
                        }
                        val = 0;
                    }

                    string rKey = item.MsrtResult[0].KeyName;

                    //rData.Value = val;//這樣寫在多通道會出問題
                    foreach (var rData in rsultList)
                    {
                        if (rData.KeyName == rKey)
                        {
                            rData.RawValue = val;
                            rData.Value = rData.RawValue;

                            break;
                        }
                    }
                    sc.ClearFindDelegate();
                }


                // -----------------------------------------------------------------
                // DIB Test Item 計算的法則
                // using raw data to compute the result of Dib Item.
                // Type = 0 normal
                // Type = 149 delta error , Vf1-Vf5 > 50mV,   Target Vf=10
                // Type = 150 serious error ,Vf5 < base spec, Target Vf=20
                // Type = 150 serious error ,Vf1-Vf5 > 0.3V,  Target Vf=20
                // -----------------------------------------------------------------
                else if (item.Type == ETestType.DIB && item.IsEnable)
                {

                    #region >>DIB<<
                    double firstVf = 0.0d;
                    double firstRawVf = 0.0d;
                    double endRawVf = 0.0d;
                    double targetVf = 0.0d;
                    double deltaVf = 0.0d;
                    string targetVfKeyname = string.Empty;

                    endRawVf = item.MsrtResult[0].RawValue;

                    foreach (TestResultData result in this._msrtResultList)
                    {
                        if ((item as DIBTestItem).ItemKeyNameA == result.KeyName)
                        {
                            firstVf = result.Value;
                            firstRawVf = result.RawValue;
                            targetVfKeyname = result.KeyName;
                            break;
                        }
                    }

                    deltaVf = firstRawVf - endRawVf;

                    item.MsrtResult[1].Value = firstRawVf;
                    item.MsrtResult[2].Value = deltaVf;

                    int dibType = 0;

                    if (firstVf < (item as DIBTestItem).FilterBase)
                    {
                        targetVf = firstVf;
                        dibType = 0;
                    }
                    else if (endRawVf < (item as DIBTestItem).FilterBase)
                    {
                        targetVf = endRawVf;
                        dibType = 150;
                        targetVf = 20;
                    }
                    else
                    {
                        if (Math.Abs(deltaVf) > (item as DIBTestItem).FilterSpec
                            && endRawVf < 2.2d)
                        {
                            // targetVf = 1.0d + deltaVf;
                            if ((item as DIBTestItem).IsOnlyJuadeSerious)
                            {
                                dibType = 0;
                                targetVf = firstVf;
                            }
                            else
                            {
                                dibType = 149;
                                targetVf = 10;
                            }

                            if (Math.Abs(deltaVf) > 0.3)
                            {
                                dibType = 150;
                                targetVf = 20;
                            }
                        }
                        else
                        {
                            targetVf = firstVf;
                            dibType = 0;
                        }
                    }

                    Random rdm = new Random();

                    int randomInt = rdm.Next(-11, 11);

                    double randomDuble = randomInt * 0.001;

                    deltaVf = deltaVf + randomDuble;

                    item.MsrtResult[3].Value = dibType;

                    if (targetVfKeyname != string.Empty)
                    {
                        foreach (TestItemData targetItem in this._testItemList)
                        {
                            if (targetItem.MsrtResult == null)
                                continue;

                            if (targetItem.MsrtResult[0].KeyName == targetVfKeyname)
                            {
                                targetItem.MsrtResult[0].Value = targetVf;
                                break;
                            }
                        }
                    }
                    #endregion
                }
            }
        }


        public double? GetValue(string varName, List<TestResultData> resultList)
        {
            if (resultList == null)
            {
                resultList = this._msrtResultList;
            }
            string[] strArr = varName.Trim().Split(new char[] { ':' });

            switch (strArr[0].Trim())
            {
                case "F":
                case "f":
                    {
                        foreach (TestItemData subItem in this._testItemList)
                        {
                            foreach (TestResultData subResult in subItem.MsrtResult)
                            {
                                if (subResult.Name == strArr[1].Trim())
                                {
                                    if (subResult.IsTested)
                                    {
                                        return subItem.ElecSetting[0].ForceValue;
                                    }
                                }
                            }
                        }
                    }
                    break;
                case "M":
                case "m":
                default:
                    foreach (TestResultData result in resultList)
                    {
                        if (result.Name == strArr[1].Trim())
                        {
                            if (result.IsTested)
                            {
                                return result.Value;
                            }
                        }
                    }
                    break;
            }


            return double.NaN;

        }

        public void CalibCalcTestItem(TestItemData[] dataArray)
        {
            // (01) 填入所有Item量測數據

            _msrtData.Clear();
            List<TestResultData> resultList = new List<TestResultData>();

            foreach (TestItemData item in dataArray)
            {
                if (item.IsEnable)
                {
                    if (item.MsrtResult != null)
                    {
                        foreach (TestResultData data in item.MsrtResult)
                        {
                            _msrtData.Add(data.KeyName, data.Value);
                            resultList.Add(data);
                        }
                    }
                }
            }


            CalibCalcTestItem(resultList);
            //// (02) 抓取CALC Itme 設定的Key name 塞入數值

            //foreach (TestItemData item in dataArray)
            //{
            //    if (item.Type == ETestType.CALC)
            //    {
            //        double valueOfItemA = 0;
            //        double valueOfItemB = 0;
            //        bool isItemAItemExistence = false;
            //        bool isItemBItemExistence = false;

            //        string itemA=(item as CALCTestItem).ItemKeyNameA;

            //        string itemB=(item as CALCTestItem).ItemKeyNameB;

            //        if (_msrtData.ContainsKey(itemA))
            //        {
            //            valueOfItemA = _msrtData[itemA];
            //            isItemAItemExistence = true;
            //        }

            //        if (_msrtData.ContainsKey(itemB))
            //        {
            //            if (itemB != item.MsrtResult[0].KeyName)
            //            {
            //                valueOfItemB = _msrtData[itemB];
            //                isItemBItemExistence = true;
            //            }
            //            else
            //            {
            //                valueOfItemB = (item as CALCTestItem).ValB;
            //                isItemBItemExistence = true;
            //            }

            //        }

            //        item.MsrtResult[0].Value = 0.0d;

            //        if (!isItemAItemExistence || !isItemBItemExistence)
            //        {
            //            return;
            //        }

            //        switch ((item as CALCTestItem).CalcType)
            //        {
            //            case ECalcType.Subtract:
            //                item.MsrtResult[0].Value = valueOfItemA - valueOfItemB;
            //                break;
            //            case ECalcType.DivideBy:
            //                item.MsrtResult[0].Value = (valueOfItemA / valueOfItemB);
            //                break;
            //            case ECalcType.Add:
            //                item.MsrtResult[0].Value = valueOfItemA + valueOfItemB;
            //                break;
            //            case ECalcType.Multiple:
            //                item.MsrtResult[0].Value = (valueOfItemA * valueOfItemB);
            //                break;
            //            //case ECalcType.DeltaR:
            //            //    if (valueOfItemB != valueOfItemA)
            //            //    {
            //            //        item.MsrtResult[0].Value = (applyValueofItemB - applyValueofItemA) / (valueOfItemB - valueOfItemA);  // R = V / I
            //            //    }
            //            //    else
            //            //    {
            //            //        item.MsrtResult[0].Value = 9999.0d;
            //            //    }

            //            //    break;
            //            default:
            //                item.MsrtResult[0].Value = 0;
            //                break;
            //        }
            //    }
            //}
        }

        public void CalibSystemCoef(TestItemData[] dataArray, GainOffsetData[] gainOffset, bool isEnable)
        {
            if (!isEnable)
            {
                return;
            }

            if (gainOffset == null || gainOffset.Length == 0)
            {
                return;
            }

            double value = 0.0d;

            foreach (var item in dataArray)
            {
                if (item.MsrtResult == null || item.GainOffsetSetting == null)
                {
                    continue;
                }

                foreach (var result in item.MsrtResult)
                {
                    foreach (var coef in gainOffset)
                    {
                        if (!coef.IsEnable)
                            continue;

                        if (result.KeyName == coef.KeyName)
                        {
                            value = result.Value;

                            if (value != 0.0d)
                            {
                                value = (value * value * coef.Square) + (value * coef.Gain) + coef.Offset;

                                result.Value = value;
                            }
                        }
                    }
                }
            }
        }

        public int GetSubItemCount(ETestType testType)
        {
            int typeIndex;

            if (testType == ETestType.LCRSWEEP)
            {
                typeIndex = (int)ETestType.LCR;
            }
            else
            {
                typeIndex = (int)testType;
            }

            return this._subItemCount[typeIndex];
            //if ( this._subItemCountDic == null || this._subItemCountDic.Count == 0 )
            //     return 0;

            //if (this._subItemCountDic.ContainsKey(testType.ToString() ) )
            //{
            //    return this._subItemCountDic[testType.ToString() ];
            //}
            //else
            //{
            //     return 0;
            //}
        }

        public double InterGain(double x1, double y1, double x2, double y2, double x)
        {
            double slop = 0.0d;

            if ((x2 - x1) != 0)
            {
                slop = (y2 - y1) / (x2 - x1);
                return slop * (x - x1) + y1;
            }

            return 0.0d;
        }

        public void SetUserDefinedData(UserData userData)
        {
            this._userData = userData;
            this.ModifyAllItemNames();
            this.GetFinalKeyAndName();

        }

        public string[] ExportGainOffsetData(EUserID userID, double startWL, double endWL)
        {
            List<string> gainList = new List<string>(50);
            List<string> coefList = new List<string>(100);
            string[] coefItemNames = Enum.GetNames(typeof(ECoefTableItem));

            gainList.Clear();
            coefList.Clear();

            StringBuilder sb = new StringBuilder();

            StringBuilder sbCoef = new StringBuilder();

            Dictionary<string, string> dicTemplateData = new Dictionary<string, string>();

            Dictionary<string, string> currentEnableItem = new Dictionary<string, string>();

            string CalibTemplatePathAndFileWithExt = string.Format("{0}{1}", "Format", ((int)userID).ToString("0000")) + "-A.cal";

            if (File.Exists(Path.Combine(Constants.Paths.USER_DIR, CalibTemplatePathAndFileWithExt)))
            {
                List<string[]> templateData = CSVUtil.ReadCSV(Path.Combine(Constants.Paths.USER_DIR, "Format6044-A.cal"));

                bool isStart = false;

                for (int row = 0; row < templateData.Count; row++)
                {
                    if (templateData[row][0] == "KeyName" && templateData[row][1] == "Name")
                    {
                        isStart = true;
                        continue;
                    }

                    if (templateData[row][0] == "SectionEnd")
                    {
                        break;
                    }

                    if (isStart)
                    {
                        StringBuilder sb2 = new StringBuilder();

                        foreach (string s in templateData[row])
                        {
                            sb2.Append(s);
                            sb2.Append(",");
                        }

                        dicTemplateData.Add(templateData[row][0], sb2.ToString());
                    }



                }

                gainList.Add("GainOffset");

                gainList.Add("KeyName,Name,Sqaure,Gain,Offset,Gain2,Offset2");

                sb.Clear();

                foreach (TestItemData item in this._testItemList)
                {
                    if (item is ESDTestItem)
                    {
                        sb.Clear();
                        sb.Append(item.KeyName);
                        sb.Append(",");
                        sb.Append(item.Name);
                        sb.Append(",0,");
                        sb.Append((item as ESDTestItem).EsdSetting.GainVolt.ToString());
                        sb.Append(",");
                        sb.Append((item as ESDTestItem).EsdSetting.OffsetVolt.ToString());
                        sb.Append(",1,0");

                        if (!currentEnableItem.ContainsKey(item.KeyName))
                            currentEnableItem.Add(item.KeyName, sb.ToString());

                        continue;
                    }


                    if (item.GainOffsetSetting != null)
                    {
                        GainOffsetData[] data = item.GainOffsetSetting;

                        for (int i = 0; i < data.Length; i++)
                        {
                            if (data[i].IsEnable == false || data[i].IsVision == false)
                                continue;
                            sb.Clear();
                            sb.Append(data[i].KeyName);
                            sb.Append(",");
                            sb.Append(data[i].Name);
                            sb.Append(",");
                            sb.Append(data[i].Square.ToString());
                            sb.Append(",");
                            sb.Append(data[i].Gain.ToString());
                            sb.Append(",");
                            sb.Append(data[i].Offset.ToString());
                            sb.Append(",");
                            sb.Append(data[i].Gain2.ToString());
                            sb.Append(",");
                            sb.Append(data[i].Offset2.ToString());
                            //  sb.Append(",");
                            if (!currentEnableItem.ContainsKey(item.KeyName))
                                currentEnableItem.Add(data[i].KeyName, sb.ToString());
                        }
                    }
                }

                foreach (var data in dicTemplateData)
                {
                    if (currentEnableItem.ContainsKey(data.Key))
                    {
                        gainList.Add(currentEnableItem[data.Key]);
                    }
                    else
                    {
                        gainList.Add(data.Value);
                    }
                }

            }
            else
            {
                gainList.Add("GainOffset");
                sb.Append("KeyName,Name,Sqaure,Gain,Offset,Gain2,Offset2");
                gainList.Add(sb.ToString());

                foreach (TestItemData item in this._testItemList)
                {
                    if (item.GainOffsetSetting != null)
                    {
                        GainOffsetData[] data = item.GainOffsetSetting;

                        sb.Clear();
                        for (int i = 0; i < data.Length; i++)
                        {
                            if (data[i].IsEnable == false || data[i].IsVision == false)
                                continue;

                            sb.Append(data[i].KeyName);
                            sb.Append(",");
                            sb.Append(data[i].Name);
                            sb.Append(",");
                            sb.Append(data[i].Square.ToString());
                            sb.Append(",");
                            sb.Append(data[i].Gain.ToString());
                            sb.Append(",");
                            sb.Append(data[i].Offset.ToString());
                            sb.Append(",");
                            sb.Append(data[i].Gain2.ToString());
                            sb.Append(",");
                            sb.Append(data[i].Offset2.ToString());
                            //  sb.Append(",");

                            gainList.Add(sb.ToString());
                            sb.Clear();
                        }
                    }
                }
            }

            // coeff Table

            foreach (TestItemData item in this._testItemList)
            {
                if (item is LOPWLTestItem || item is VLRTestItem)
                {
                    double[][] coefData = null;

                    if (item is LOPWLTestItem)
                    {
                        coefData = (item as LOPWLTestItem).CoefTable;
                    }
                    else if (item is VLRTestItem)
                    {
                        coefData = (item as VLRTestItem).CoefTable;
                    }

                    double coefStep = (coefData[1][0] - coefData[0][0]);

                    if (coefStep < 0.0d || coefStep == 0.0d)
                        continue;

                    int startIndex = (int)((startWL - coefData[0][0]) / coefStep);
                    int endIndex = (int)((endWL - coefData[0][0]) / coefStep);

                    if (startIndex > endIndex)
                        continue;

                    sbCoef.Append("CoefTable,MinWave,");
                    sbCoef.Append(startWL.ToString());
                    sbCoef.Append(",MaxWave,");
                    sbCoef.Append(endWL.ToString());
                    sbCoef.Append(",KeyName,");
                    sbCoef.Append(item.KeyName);
                    sbCoef.Append(",Name,");
                    sbCoef.Append(item.Name);

                    coefList.Add(sbCoef.ToString());

                    sbCoef.Clear();
                    sbCoef.Append(String.Join(",", coefItemNames));
                    coefList.Add(sbCoef.ToString());

                    sbCoef.Clear();
                    for (int row = startIndex; row <= endIndex; row++)
                    {
                        for (int k = 0; k < coefItemNames.Length; k++)
                        {
                            sbCoef.Append(coefData[row][k].ToString());

                            if (k <= (coefItemNames.Length - 1))
                            {
                                sbCoef.Append(",");
                            }
                        }
                        coefList.Add(sbCoef.ToString());
                        sbCoef.Clear();
                    }
                    coefList.Add("SectionEnd");
                    coefList.Add("");
                }
            }

            gainList.Add("SectionEnd");
            gainList.Add("");
            gainList.AddRange(coefList);

            gainList.AddRange(this.ExportByChannelGainOffsetData());  //20140725, Roy Multi-Die

            return gainList.ToArray();
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

        private double[] ParseStrToDouble2(string[] strData, int startiIndex, int endIndex)
        {
            if (strData == null || startiIndex < 0 || endIndex < 0 || (startiIndex > endIndex) || strData.Length < startiIndex)
            {
                return (new double[1]);
            }

            double[] data = new double[endIndex - startiIndex + 1];

            double parseData = 0.0d;

            for (int i = 0; i < data.Length; i++)
            {
                if ((i + startiIndex) > strData.Length - 1)
                {
                    data[i] = 0.0d;
                    continue;
                }


                if (Double.TryParse(strData[i + startiIndex], out parseData))
                {
                    data[i] = parseData;
                }
                else
                {
                    data[i] = 0.0d;
                }
            }

            for (int i = 0; i < data.Length; i++)
            {
                if (data[1] == 0.0d)
                {
                    data[1] = 1.0d;
                }

                if (data[3] == 0.0d)
                {
                    data[3] = 1.0d;
                }
            }


            return data;
        }

        public bool ImportSptCalibData(string[][] data, ref double[] xaxisCoeff, ref double[] productSptYintCoef, ref double[] productSptYweight)
        {
            xaxisCoeff = new double[4];
            productSptYintCoef = new double[2048];
            productSptYweight = new double[2048];
            double[] parseDataX = new double[xaxisCoeff.Length];
            double[][] parseDataY = new double[2][];
            // double [] productSptYweight=new double[productSptYweight.Length];

            parseDataY[0] = new double[productSptYintCoef.Length];
            parseDataY[1] = new double[productSptYintCoef.Length];
            int startIndex = -1;
            int endIndex = -1;
            for (int row = 0; row < data.Length; row++)
            {																                               	// GainOffset
                if (data[row][0] == "Intercept")
                {
                    if (double.TryParse(data[row][1], out parseDataX[0]) == false)
                    {
                        parseDataX[0] = 0;
                        return false;
                    }
                    continue;
                }
                else if (data[row][0] == "FirstCoefficient")
                {
                    if (double.TryParse(data[row][1], out parseDataX[1]) == false)
                    {
                        parseDataX[1] = 0;
                        return false;
                    }
                    continue;
                }
                else if (data[row][0] == "SecondCoefficient")
                {
                    if (double.TryParse(data[row][1], out parseDataX[2]) == false)
                    {
                        parseDataX[2] = 0;
                        return false;
                    }
                    continue;
                }
                else if (data[row][0] == "ThirdCoefficient")
                {
                    if (double.TryParse(data[row][1], out parseDataX[3]) == false)
                    {
                        parseDataX[3] = 0;
                        return false;
                    }
                    continue;
                }
                else if (data[row][0] == "YintCalibSpectrum")
                {
                    startIndex = row + 1;
                    continue;
                }
                else if (data[row][0] == "End")
                {
                    endIndex = row;
                    continue;
                }
            }

            for (int row = startIndex; row < endIndex; row++)
            {
                if (double.TryParse(data[row][0], out parseDataY[0][row - startIndex]) == false)
                {
                    parseDataY[0][row - startIndex] = 0;
                    return false;
                }
                if (double.TryParse(data[row][1], out parseDataY[1][row - startIndex]) == false)
                {
                    parseDataY[1][row - startIndex] = 0;
                    return false;
                }
            }

            // true 

            xaxisCoeff = parseDataX;
            productSptYintCoef = parseDataY[0];
            productSptYweight = parseDataY[1];
            return true;

        }

        public void ImportCalibrationFile(string[][] data, ref double startWL, ref double endWL, ref string sptCalibFilePath)
        {
            if (data == null)
            {
                //startWL = 0.0d;
                //endWL = 0.0d;
                return;
            }

            int ESDStartIndex = 0;

            List<string[]> ESDSetting = new List<string[]>();

            for (int row = 0; row < data.Length; row++)
            {
                if (data[row][0] == "[ESD]")
                {
                    ESDStartIndex = row + 1;
                    break;
                }
            }

            for (int row = ESDStartIndex; row < data.Length; row++)
            {
                if (data[row][0] == "[Chuck]")
                {
                    break;
                }

                ESDSetting.Add(data[row]);
            }


            double[] parseData = null;
            int coefCount = 0;
            string keyName = string.Empty;
            int sectionDataType = 0;	// empty line

            Dictionary<string, double[]> gainOffsetDic = new Dictionary<string, double[]>(100);
            Dictionary<string, double[][]> coefDic = new Dictionary<string, double[][]>(5);
            Dictionary<string, double> startWaveDic = new Dictionary<string, double>();
            Dictionary<string, double> endWaveDic = new Dictionary<string, double>();

            Dictionary<string, double[]> byChannelGainOffsetDic = new Dictionary<string, double[]>(100);

            List<double[]> sectionCoefList = new List<double[]>();

            gainOffsetDic.Clear();
            coefDic.Clear();
            sectionCoefList.Clear();
            startWaveDic.Clear();
            endWaveDic.Clear();

            byChannelGainOffsetDic.Clear();

            for (int row = 0; row < data.Length; row++)
            {																	// GainOffset
                if (data[row][0] == "GainOffset")				// KeyName,Name,Sqaure,Gain,Offset,Channel
                {
                    sectionDataType = 1;
                    row++;
                    continue;
                }																// for Paul Defined Data
                else if (data[row][0] == "KeyName")			// KeyName,Name,Sqaure,Gain,Offset,Channel
                {
                    sectionDataType = 1;
                    continue;
                }																//	0             , 1			   , 2	   , 3			    , 4	   , 5			    , 6			      , 7		 , 8
                else if (data[row][0] == "CoefTable")		// CoefTable, MinWave, 430, MaxWave, 440, KeyName, LOPWL_1, Name, LopWL01
                {
                    sectionDataType = 2;
                    keyName = data[row][6];

                    if (double.TryParse(data[row][2], out startWL) == false)
                    {
                        startWL = 380;
                    }
                    if (double.TryParse(data[row][4], out endWL) == false)
                    {
                        endWL = 550;
                    }
                    row++;
                    coefCount++;
                    continue;
                }																// for Paul Defined Data
                else if (data[row][0] == "CoefTable_1")		// CoefTable_1,MinWave,430,MaxWave,44
                {
                    coefCount++;
                    sectionDataType = 2;
                    keyName = "LOPWL_" + coefCount.ToString();
                    row++;
                    continue;
                }
                else if (data[row][0] == "ByChannelGainOffset")
                {
                    sectionDataType = 3;
                    continue;
                }
                else if (data[row][0] == "Channel")			// Channel,KeyName,Name,Sqaure,Gain,Offset,Channel
                {
                    sectionDataType = 3;
                    continue;
                }
                else if (data[row][0] == "" || data[row][0] == "SectionEnd")
                {
                    sectionDataType = 0;
                }

                else if (data[row][0] == "SpetrometerCoeffFilePath")
                {
                    sptCalibFilePath = data[row][1];
                }


                if (sectionDataType == 1)
                {
                    // KeyName , Name , Square, Gain , Offset, Gain2, Offset2
                    parseData = this.ParseStrToDouble2(data[row], 2, 6);		// Square = 2, Offset = 4 
                    gainOffsetDic.Add(data[row][0], parseData);
                }
                else if (sectionDataType == 2)
                {
                    //Wave, WLP, WLD, WLC, LOP, WATT, LM
                    parseData = this.ParseStrToDouble2(data[row], 0, 7);
                    sectionCoefList.Add(parseData);
                }
                else if (sectionDataType == 3)
                {
                    // Channel,KeyName,Name,Sqaure,Gain,Offset
                    parseData = this.ParseStrToDouble(data[row], 3, 5);		// Square = 3, Offset = 5 
                    string tempKey = string.Format("{0}@{1}", data[row][1], data[row][0]);
                    byChannelGainOffsetDic.Add(tempKey, parseData);
                }
                else if (sectionDataType == 0)
                {
                    if (sectionCoefList.Count > 0)
                    {
                        coefDic.Add(keyName, sectionCoefList.ToArray());
                        startWaveDic.Add(keyName, sectionCoefList[0][0]);
                        endWaveDic.Add(keyName, sectionCoefList[sectionCoefList.Count - 1][0]);
                        sectionCoefList.Clear();
                    }
                }
            }

            double[] oneRow = null;

            ChannelConditionTable conditionTable = _data.ChannelConditionTable;

            foreach (TestItemData item in this._testItemList)
            {
                if (item.GainOffsetSetting != null)
                {
                    for (int i = 0; i < item.GainOffsetSetting.Length; i++)
                    {
                        if (gainOffsetDic.ContainsKey(item.GainOffsetSetting[i].KeyName) &&
                             item.GainOffsetSetting[i].IsEnable &&
                             item.GainOffsetSetting[i].IsVision)
                        {
                            oneRow = gainOffsetDic[item.GainOffsetSetting[i].KeyName];
                            item.GainOffsetSetting[i].Square = oneRow[0];
                            item.GainOffsetSetting[i].Gain = oneRow[1];
                            item.GainOffsetSetting[i].Offset = oneRow[2];
                            item.GainOffsetSetting[i].Gain2 = oneRow[3];
                            item.GainOffsetSetting[i].Offset2 = oneRow[4];
                            // By Channel Gain Offset
                            if (conditionTable != null && conditionTable.IsEnable)
                            {
                                for (uint channel = 0; channel < conditionTable.Count; channel++)
                                {
                                    string tempKeyName = string.Format("{0}@{1}", item.GainOffsetSetting[i].KeyName, channel);

                                    if (byChannelGainOffsetDic.ContainsKey(tempKeyName))
                                    {
                                        GainOffsetData channelCoef = conditionTable.Channels[channel].GetByChannelGainOffsetData(item.GainOffsetSetting[i].KeyName);

                                        if (channelCoef != null)
                                        {
                                            channelCoef.Square = byChannelGainOffsetDic[tempKeyName][0];
                                            channelCoef.Gain = byChannelGainOffsetDic[tempKeyName][1];
                                            channelCoef.Offset = byChannelGainOffsetDic[tempKeyName][2];
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (item.Type == ETestType.LOPWL && coefDic.ContainsKey(item.KeyName))
                {
                    double[][] coefData = (item as LOPWLTestItem).CoefTable;
                    double[][] importData = coefDic[item.KeyName];

                    int startIndex = (int)((startWaveDic[item.KeyName] - coefData[0][0]) / (item as LOPWLTestItem).CoefWLResolution);

                    for (int m = startIndex; m < coefData.Length; m++)
                    {
                        for (int n = 0; n < importData.Length; n++)
                        {
                            if (Convert.ToInt32(coefData[m][0] * 1000.0d) == Convert.ToInt32(importData[n][0] * 1000.0d))
                            {
                                for (int k = 1; k < importData[0].Length; k++)
                                {
                                    coefData[m][k] = importData[n][k];
                                }
                                break;
                            }
                        }
                    }

                    //if (startWaveDic[item.KeyName] < startWL)
                    //{
                    //    startWL = startWaveDic[item.KeyName];
                    //}

                    //if ( endWaveDic[item.KeyName] > endWL )
                    //{
                    //    endWL = endWaveDic[item.KeyName];
                    //}
                }
                else if (item.Type == ETestType.VLR && coefDic.ContainsKey(item.KeyName))
                {
                    double[][] coefData = (item as VLRTestItem).CoefTable;
                    double[][] importData = coefDic[item.KeyName];

                    int startIndex = (int)((startWaveDic[item.KeyName] - coefData[0][0]) / (item as VLRTestItem).CoefWLResolution);

                    for (int m = startIndex; m < coefData.Length; m++)
                    {
                        for (int n = 0; n < importData.Length; n++)
                        {
                            if (Convert.ToInt32(coefData[m][0] * 1000.0d) == Convert.ToInt32(importData[n][0] * 1000.0d))
                            {
                                for (int k = 1; k < importData[0].Length; k++)
                                {
                                    coefData[m][k] = importData[n][k];
                                }
                                break;
                            }
                        }
                    }

                    //if (startWaveDic[item.KeyName] < startWL)
                    //{
                    //    startWL = startWaveDic[item.KeyName];
                    //}

                    //if ( endWaveDic[item.KeyName] > endWL )
                    //{
                    //    endWL = endWaveDic[item.KeyName];
                    //}
                }

                //if (item.Type == ETestType.ESD)
                //{
                //    foreach (string[] str in ESDSetting)
                //    {
                //        if (str[0] == item.KeyName)
                //        {
                //            (item as ESDTestItem).EsdSetting.GainVolt = Convert.ToDouble(str[2].ToString());
                //            (item as ESDTestItem).EsdSetting.OffsetVolt = Convert.ToInt16(str[3].ToString());
                //        }
                //    }
                //}		
            }
        }

        public string ParseWM617GainOffsetList(ELOPSaveItem lopSaveItem)
        {
            int i = 0;
            bool isFind = false;

            StringBuilder sb01 = new StringBuilder();
            StringBuilder sb02 = new StringBuilder();
            StringBuilder sb03 = new StringBuilder();
            StringBuilder sb04 = new StringBuilder();

            string[] opticalItemKey = new string[] { "LOPWL_1", "LOPWL_2", "LOPWL_3" };
            string[] elecItemKey = new string[] { "IF_1", "IF_2", "IF_3", "IF_4", "IZ_1", "IZ_2", "VR_1" };

            for (i = 0; i < opticalItemKey.Length; i++)
            {
                //---------------------------------------------
                // Parse optical item Gain/Offset
                //---------------------------------------------
                isFind = false;
                foreach (TestItemData item in this._testItemList)
                {
                    if (item.KeyName == opticalItemKey[i])
                    {
                        isFind = true;
                        switch (lopSaveItem)
                        {
                            case ELOPSaveItem.mcd:
                                sb01.Append(item.GainOffsetSetting[(int)EOptiMsrtType.LOP].Square);
                                sb01.Append(",");
                                sb01.Append(item.GainOffsetSetting[(int)EOptiMsrtType.LOP].Gain);
                                sb01.Append(",");
                                sb01.Append(item.GainOffsetSetting[(int)EOptiMsrtType.LOP].Offset);
                                sb01.Append(",");
                                break;
                            case ELOPSaveItem.watt:
                                sb01.Append(item.GainOffsetSetting[(int)EOptiMsrtType.WATT].Square);
                                sb01.Append(",");
                                sb01.Append(item.GainOffsetSetting[(int)EOptiMsrtType.WATT].Gain);
                                sb01.Append(",");
                                sb01.Append(item.GainOffsetSetting[(int)EOptiMsrtType.WATT].Offset);
                                sb01.Append(",");
                                break;
                            case ELOPSaveItem.lm:
                                sb01.Append(item.GainOffsetSetting[(int)EOptiMsrtType.LM].Square);
                                sb01.Append(",");
                                sb01.Append(item.GainOffsetSetting[(int)EOptiMsrtType.LM].Gain);
                                sb01.Append(",");
                                sb01.Append(item.GainOffsetSetting[(int)EOptiMsrtType.LM].Offset);
                                sb01.Append(",");
                                break;
                            default:
                                sb01.Append("0,1,0,");
                                break;
                        }
                        sb03.Append(item.GainOffsetSetting[(int)EOptiMsrtType.WLP].Offset);
                        sb03.Append(",");
                        sb03.Append(item.GainOffsetSetting[(int)EOptiMsrtType.WLD].Offset);
                        sb03.Append(",");
                        sb03.Append(item.GainOffsetSetting[(int)EOptiMsrtType.WLC].Offset);
                        sb03.Append(",");

                        sb04.Append(item.GainOffsetSetting[(int)EOptiMsrtType.CIEx].Offset);
                        sb04.Append(",");
                        sb04.Append(item.GainOffsetSetting[(int)EOptiMsrtType.CIEy].Offset);
                        sb04.Append(",");

                        break;
                    }
                }

                if (isFind == false)
                {
                    sb01.Append("0,1,0,");
                    sb03.Append("0,0,0,");
                    sb04.Append("0,0,");
                }
            }

            for (i = 0; i < elecItemKey.Length; i++)
            {
                //---------------------------------------------
                // Parse electronic item Gain/Offset
                //---------------------------------------------
                isFind = false;
                foreach (TestItemData item in this._testItemList)
                {
                    if (item.KeyName == elecItemKey[i])
                    {
                        isFind = true;
                        sb02.Append(item.GainOffsetSetting[0].Offset);
                        sb02.Append(",");
                        break;
                    }
                }

                if (isFind == false)
                {
                    sb02.Append("0,");
                }
            }

            string combineStr = sb01.ToString() + sb02.ToString() + sb03.ToString() + sb04.ToString();
            string rtnStr = combineStr.Remove(combineStr.Length - 1);

            return rtnStr;
        }

        public void ResetLOPVisionProperty(ELOPSaveItem lopSaveItems)
        {
            string msrtKeyNameLetter = string.Empty;

            if (this._testItemList == null)
                return;

            foreach (TestItemData item in this._testItemList)
            {
                if (item.Type != ETestType.LOPWL)
                    continue;

                if (item.MsrtResult == null || item.GainOffsetSetting == null)
                    break;

                for (int i = 0; i < item.MsrtResult.Length; i++)
                {
                    msrtKeyNameLetter = UserData.ExtractKeyNameLetter(item.MsrtResult[i].KeyName);

                    if (msrtKeyNameLetter != "WATT" && msrtKeyNameLetter != "LOP" && msrtKeyNameLetter != "LM")
                        continue;

                    switch (lopSaveItems)
                    {
                        case ELOPSaveItem.watt:
                            if (msrtKeyNameLetter == "WATT")
                            {
                                item.MsrtResult[i].IsVision = true;
                                item.GainOffsetSetting[i].IsVision = true;
                            }
                            else
                            {
                                item.MsrtResult[i].IsVision = false;
                                item.GainOffsetSetting[i].IsVision = false;
                            }
                            break;
                        //------------------------------------------------------------------------------
                        case ELOPSaveItem.mcd:

                            if (msrtKeyNameLetter == "LOP")
                            {
                                item.MsrtResult[i].IsVision = true;
                                item.GainOffsetSetting[i].IsVision = true;
                            }
                            else
                            {
                                item.MsrtResult[i].IsVision = false;
                                item.GainOffsetSetting[i].IsVision = false;
                            }

                            break;
                        //------------------------------------------------------------------------------
                        case ELOPSaveItem.lm:
                            if (msrtKeyNameLetter == "LM")
                            {
                                item.MsrtResult[i].IsVision = true;
                                item.GainOffsetSetting[i].IsVision = true;
                            }
                            else
                            {
                                item.MsrtResult[i].IsVision = false;
                                item.GainOffsetSetting[i].IsVision = false;
                            }
                            break;
                        //------------------------------------------------------------------------------
                        case ELOPSaveItem.mcd_watt:
                            if (msrtKeyNameLetter == "LOP" || msrtKeyNameLetter == "WATT")
                            {
                                item.MsrtResult[i].IsVision = true;
                                item.GainOffsetSetting[i].IsVision = true;
                            }
                            else
                            {
                                item.MsrtResult[i].IsVision = false;
                                item.GainOffsetSetting[i].IsVision = false;
                            }
                            break;
                        //------------------------------------------------------------------------------
                        case ELOPSaveItem.mcd_lm:
                            if (msrtKeyNameLetter == "LOP" || msrtKeyNameLetter == "LM")
                            {
                                item.MsrtResult[i].IsVision = true;
                                item.GainOffsetSetting[i].IsVision = true;
                            }
                            else
                            {
                                item.MsrtResult[i].IsVision = false;
                                item.GainOffsetSetting[i].IsVision = false;
                            }
                            break;
                        //------------------------------------------------------------------------------
                        case ELOPSaveItem.watt_lm:
                            if (msrtKeyNameLetter == "WATT" || msrtKeyNameLetter == "LM")
                            {
                                item.MsrtResult[i].IsVision = true;
                                item.GainOffsetSetting[i].IsVision = true;
                            }
                            else
                            {
                                item.MsrtResult[i].IsVision = false;
                                item.GainOffsetSetting[i].IsVision = false;
                            }
                            break;
                        //------------------------------------------------------------------------------
                        case ELOPSaveItem.mcd_watt_lm:
                        default:
                            item.MsrtResult[i].IsVision = true;
                            item.GainOffsetSetting[i].IsVision = true;
                            break;
                    }
                }
            }
        }

        public void ResetProuctChannelSetting(ETesterFunctionType type, int colMax, int rowMax)
        {
            if (type == ETesterFunctionType.Multi_Die)
            {
                _data.ChannelConditionTable.RefreshChSetting(colMax, rowMax);

                _data.ChannelConditionTable.UpdateConditionTestItems(type, _data.TestItemArray);
            }
        }

        public void CopyTestItemArrayToEachChannel()
        {
            if (_data != null)
            {
                if (_data.TestItemArray != null)
                {
                    _data.ChannelConditionTable.UpdateConditionTestItems(this._testerFuncType, _data.TestItemArray);
                }
            }
        }

        public object Clone()
        {
            ConditionCtrl obj = this.MemberwiseClone() as ConditionCtrl;

            obj._lockObj = new object();

            return (object)obj;

            //MemoryStream ms = new MemoryStream();
            //BinaryFormatter bf = new BinaryFormatter();
            //bf.Serialize(ms, this);
            //ms.Position = 0;
            //object obj = bf.Deserialize(ms);
            //ms.Close();

            //return obj;
        }

        #endregion
    }
}
