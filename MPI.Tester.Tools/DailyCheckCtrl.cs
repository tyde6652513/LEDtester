using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Xml;

using MPI.Tester.Data;

namespace MPI.Tester.Tools
{


    public class DailyCheckCtrl : FilesCompare
    {
        private const int SPEC_LENGTH = 7;

        private DailyCheckSpecInfo _dailySpecInfo = new DailyCheckSpecInfo();
        private Dictionary<string, FilterData> _dailyFilterDic=new Dictionary<string,FilterData>();
        private Dictionary<string, DailyResultData> _data;
        private string[] _everyDieName;
        private string[] _userDefineKeyName;
        private string[] _displayTitleName;
        private bool[] _everyDieResult;
        private bool _finalResult = false;
        private int _everyDieOutSpecCount = 0;
        private string _resultDescribe=string.Empty;

        public DailyCheckCtrl(string[] userDefineKeyName)
        {
            this._userDefineKeyName = userDefineKeyName;
        }

        #region >>> Public Method <<<

        public bool Init(string pathAndFile, string formatName)
        {
            if (this.LoadCurrentFormat(pathAndFile, formatName))
            {
                this.ParseTitleData();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void FilterOrignalData()
        {
            if (this._dtStd == null || this._dtMsrt == null)
                return;

            this.SetFilterData();

            string filterExpress = string.Empty;
            DataRow[] rows = null;

            foreach (KeyValuePair<string, FilterData> kvp in this._dailyFilterDic)
            {
                int stdFilterCounts = 0;
                int msrtFilterCounts = 0;

                if (kvp.Value.IsEnable == true)
                {
                    filterExpress = kvp.Key + "<" + kvp.Value.Min.ToString() + " or " + kvp.Key + ">" + kvp.Value.Max.ToString();

                    // filter std. data 
                    rows = this._dtStd.Select(filterExpress);
                    if (rows != null)
                    {
                        for (int i = 0; i < rows.Length; i++)
                        {
                            rows[i].Delete();
                            stdFilterCounts++;
                        }
                    }

                    // filter Msrt. Data
                    rows = this._dtMsrt.Select(filterExpress);
                    if (rows != null)
                    {
                        for (int i = 0; i < rows.Length; i++)
                        {
                            rows[i].Delete();
                            msrtFilterCounts++;
                        }
                    }
                }
            }
            this._dtStd.AcceptChanges();
            this._dtMsrt.AcceptChanges();	
        }


        //public EErrorCode LoadTestFile(string pathAndFile, int index)
        //{
           

        //}

        public  bool LoadRecipeSpecFromFile( EUserID userId, string pathAndFile, string recipeFileName)
        {
            if (!File.Exists(pathAndFile))
            {
                return false;
            }

            _dailySpecInfo = new DailyCheckSpecInfo();


            switch (userId)
            {
                case EUserID.AquaLite:
                    
                    List<string[]> temp = MPI.Tester.CSVUtil.ReadCSV(pathAndFile);
                    List<string[]> data = new List<string[]>();

                    if (temp == null)
                    {
                        return false;
                    }
                    //
                    data.Add(temp[0]);
                    int titleStartIndex = -1;
                    for (int idx = 0; idx < temp.Count; idx++)
                    {
                        if (temp[idx][0] == recipeFileName)
                        {
                            titleStartIndex = idx;
                            break;
                        }
                    }

                    if (titleStartIndex == -1)
                    {
                        return false;
                    }

                    string[] titleStrName = this._titleName.Values.ToArray();

                    if (titleStartIndex != -1)
                    {
                        this._dailyFilterDic = new Dictionary<string, FilterData>();

                        for (int idx = titleStartIndex; idx < (titleStartIndex + SPEC_LENGTH); idx++)
                        {
                            data.Add(temp[idx]);
                        }

                        for (int i = 1; i < data[0].Length; i++)
                        {
                            string itemName = "";

                            if (data[1][i] != "")
                            {
                                itemName = data[0][i];
                                string everyDieL = data[2][i];
                                string everyDieH = data[3][i];
                                string avgLow = data[4][i];
                                string avgHight = data[5][i];
                                string min = data[6][i];
                                string max = data[7][i];
                              //  string clacType = data[1][i];

                                foreach (KeyValuePair<string, string> kvp in this._titleName)
                                {
                                    if (kvp.Value == itemName)
                                    {
                                        DailyWatchSpec spec = new DailyWatchSpec(kvp.Key, kvp.Value);
                                        double everyDieLValue = -1;
                                        double everyDieHValue = -1;
                                        double reCalibSpec = -1;
                                        double maxValue = -1;
                                        double minValue = -1;
                                        int calcType = 0;
                                        double.TryParse(everyDieL, out everyDieLValue);
                                        double.TryParse(everyDieH, out everyDieHValue);
                                        double.TryParse(avgLow, out reCalibSpec);
                                        double.TryParse(max, out maxValue);
                                        double.TryParse(min, out minValue);
                                        int.TryParse(data[1][i], out calcType);
                                        spec.EverDieLowerSpec = -1*everyDieLValue;
                                        spec.EverDieHighSpec = everyDieHValue;
                                        spec.ReCalibSpec = reCalibSpec;
                                        spec.MaxValue = maxValue;
                                        spec.MinValue = minValue;
                                        spec.CriteriaType = calcType;

                                        if (calcType == 0)
                                        {
                                            spec.IsEnable = false;
                                        }
                                        else
                                        {
                                            spec.IsEnable = true;
                                        }
                                        // Create Filter Data
                                        FilterData filterData=new FilterData();

                                        if (maxValue != 0)
                                        {
                                            filterData.IsEnable = true;
                                        }
                                        else
                                        {
                                            filterData.IsEnable = false;
                                        }
                               
                                        filterData.Max=maxValue;
                                        filterData.Min=minValue;
                                        _dailyFilterDic.Add(kvp.Key, filterData);
                                        if (this._titleIndex.ContainsKey(kvp.Key))
                                        {
                                            spec.DataColIndex = this._titleIndex[kvp.Key];
                                        }
                                        this._dailySpecInfo.Data.Add(kvp.Key, spec);
                                        break;
                                    }
                                }

                            }
                        }
                    }

                    break;
                case EUserID.Eti:
                    break;
                default:
                    return false;
            }


            return true;
        }

        private bool LoadSpecByWMMode(string pathAndFile, string recipeFileName)
        {

            _dailySpecInfo = new DailyCheckSpecInfo();

            List<string[]> temp = MPI.Tester.CSVUtil.ReadCSV(pathAndFile);
            List<string[]> data = new List<string[]>();
            List<int> dataLength = new List<int>();

            if (temp == null)
            {
                return false;
            }

            data.Add(temp[0]);
            dataLength.Add(temp[0].Length);

            int titleStartIndex = -1;

            for (int idx = 0; idx < temp.Count; idx++)
            {
                if (temp[idx][0] == recipeFileName)
                {
                    titleStartIndex = idx;
                    break;
                }
            }

            if (titleStartIndex == -1)
            {
                Console.WriteLine("[DailyCheck], Open Criterion Not Find Macth Recipe");
                return false;
            }

            string[] titleStrName = this._titleName.Values.ToArray();

            if (titleStartIndex != -1)
            {
                try
                {
                    for (int idx = titleStartIndex; idx < (titleStartIndex + SPEC_LENGTH); idx++)
                    {
                        data.Add(temp[idx]);
                        dataLength.Add(temp[idx].Length);
                    }

                    this._dailyFilterDic = new Dictionary<string, FilterData>();

                    int dataMinLength = dataLength.Min();

                    for (int i = 1; i < dataMinLength; i++)
                    {
                        string itemName = "";

                        if (data[1][i] != "")
                        {
                            itemName = data[0][i];
                            string everyDieL = data[2][i];
                            string everyDieH = data[3][i];
                            string avgLow = data[4][i];
                            string avgHight = data[5][i];
                            string min = data[6][i];
                            string max = data[7][i];

                            foreach (KeyValuePair<string, string> kvp in this._titleName)
                            {
                                if (kvp.Value == itemName)
                                {
                                    DailyWatchSpec spec = new DailyWatchSpec(kvp.Key, kvp.Value);
                                    double everyDieLValue = -1;
                                    double everyDieHValue = -1;
                                    double reCalibSpec = -1;
                                    double maxValue = -1;
                                    double minValue = -1;
                                    int calcType = 0;
                                    double.TryParse(everyDieL, out everyDieLValue);
                                    double.TryParse(everyDieH, out everyDieHValue);
                                    double.TryParse(avgLow, out reCalibSpec);
                                    double.TryParse(max, out maxValue);
                                    double.TryParse(min, out minValue);
                                    int.TryParse(data[1][i], out calcType);
                                    spec.EverDieLowerSpec = -1 * everyDieLValue;
                                    spec.EverDieHighSpec = everyDieHValue;
                                    spec.ReCalibSpec = reCalibSpec;
                                    spec.MaxValue = maxValue;
                                    spec.MinValue = minValue;
                                    spec.CriteriaType = calcType;

                                    if (calcType == 0)
                                    {
                                        spec.IsEnable = false;
                                    }
                                    else
                                    {
                                        spec.IsEnable = true;
                                    }
                                    // Create Filter Data
                                    FilterData filterData = new FilterData();

                                    if (maxValue != 0)
                                    {
                                        filterData.IsEnable = true;
                                    }
                                    else
                                    {
                                        filterData.IsEnable = false;
                                    }

                                    filterData.Max = maxValue;
                                    filterData.Min = minValue;
                                    this._dailyFilterDic.Add(kvp.Key, filterData);
                                    if (this._titleIndex.ContainsKey(kvp.Key))
                                    {
                                        spec.DataColIndex = this._titleIndex[kvp.Key];
                                    }
                                    this._dailySpecInfo.Data.Add(kvp.Key, spec);
                                    break;
                                }
                            }

                        }
                    }
                }
                catch
                {
                    Console.WriteLine("[DailyCheck], Open Criterion UnDefied ERROR");
                    return false;
                }
            }
            return true;
        }

        private bool LoadSpecByMPIMode(string pathAndFile, string recipeFileName)
        {
            pathAndFile = Path.Combine(Constants.Paths.TOOLS_DIR, recipeFileName + ".xml"); // XML File
            return OpenDailyCheckSpecData(pathAndFile);
        }

        private bool OpenDailyCheckSpecData(string fileNameAndPath)
        {
            if (File.Exists(fileNameAndPath) == false)
            {
                return false;
            }

            this._dailySpecInfo.Data.Clear();
            this._dailySpecInfo = MPI.Xml.XmlFileSerializer.Deserialize(typeof(DailyCheckSpecInfo), fileNameAndPath) as DailyCheckSpecInfo;
            this._dailySpecInfo.PushData(this._titleName);


            foreach (DailyWatchSpec spec in this._dailySpecInfo.Data.Values)
            {
                if (this._titleIndex.ContainsKey(spec.KeyName))
                {
                    spec.DataColIndex = this._titleIndex[spec.KeyName];

                    if (spec.IsEnable == false && spec.IsEnbaleFilter == false)
                    {

                    }

                }

            }



            return true;
        }

        public bool LoadCriterion(EDailyCheckSpecBy type, string sourcePathAndFile, string pathAndFile, string recipeFileName, string testFileName,EUserID userId)
        {
            if (type == EDailyCheckSpecBy.RECIPE)
            {
                switch (userId)
                {
                    case EUserID.AquaLite:
                    case EUserID.EPITOP:
                        MPIFile.CopyFile(sourcePathAndFile, pathAndFile);
                        return LoadSpecByWMMode(pathAndFile, recipeFileName);
                    default:
                        return LoadSpecFromMPIMode(type, recipeFileName, testFileName);
                }
            }
            else
            {
                return LoadSpecFromMPIMode(type, recipeFileName, testFileName);
            }
        }

        public bool LoadSpecFromMPIMode(EDailyCheckSpecBy type, string recipeFileName, string testFileName)
        {
            string pathAndFile = string.Empty;

            switch (type)
            {
                case EDailyCheckSpecBy.RECIPE:
                      pathAndFile = Path.Combine(Constants.Paths.TOOLS_DC_DIR, recipeFileName + ".xml"); // XML File
                    break;
                case EDailyCheckSpecBy.TestFileName:
                      pathAndFile = Path.Combine(Constants.Paths.TOOLS_DC_FILE_SPEC_DIR, testFileName + ".xml"); // XML File
                    break;
            }

            if (!OpenDailyCheckSpecData(pathAndFile))
            {
                return false;
            }
            return true;
        }

        public void CrateItemAndCaculate()
        {
            if (this._dtCompare == null || this._dtCompare.Rows.Count==0)
                return;

            double[] xIn = null;
            double[] yOut = null;
            double[] delta=null;
            this._data = new Dictionary<string, DailyResultData>();

            this._everyDieName = new string[this._dtCompare.Rows.Count];
            this._everyDieResult = new bool [this._dtCompare.Rows.Count];

            for (int row = 0; row < this._dtCompare.Rows.Count; row++)
            {
                this._everyDieName[row] = this._dtCompare.Rows[row][0].ToString();
            }

            foreach (DailyWatchSpec spec in this._dailySpecInfo.Data.Values)
            {
                int itemIndex = 0;
                xIn = new double[this._dtCompare.Rows.Count];
                yOut = new double[this._dtCompare.Rows.Count];
                delta = new double[this._dtCompare.Rows.Count];
                //
                for (int row = 0; row < this._dtCompare.Rows.Count; row++)
                {
                    xIn[row] = (double)this._dtCompare.Rows[row][spec.DataColIndex * 3 + 2];	// msrt index = 2, 5, 8,
                    yOut[row] = (double)this._dtCompare.Rows[row][spec.DataColIndex * 3 + 1];	// std index  = 1, 4, 7, 
                  //  delta[row]=(double)this._dtCompare.Rows[row][spec.DataColIndex * 3 + 3];	// std index  = 1, 4, 7, 
                }

                for (int col = 0; col < xIn.Length; col++)
                {
                    if (spec.CriteriaType == 2)
                    {
                        if (xIn[col] == 0)
                        {
                            delta[col] = 0;
                        }
                        else
                        {
                            delta[col] = 100 * ((yOut[col] / xIn[col]) - 1);
                        }
                    }
                    else
                    {
                        delta[col] = yOut[col] - xIn[col];
                    }
                }

                DailyResultData dItemData = new DailyResultData();
                dItemData.DataArray = yOut;
                dItemData.KeyName = spec.KeyName;
                dItemData.Name = spec.Name;
                dItemData.EverDieHighSpec = spec.EverDieHighSpec;
                dItemData.EverDieLowerSpec = spec.EverDieLowerSpec;
                dItemData.Caculate(false,0);
                this._data.Add("S_" + spec.KeyName, dItemData);
                // ------------------------------------------------------------------------------------------
                dItemData = new DailyResultData();
                dItemData.DataArray = xIn;
                dItemData.KeyName = spec.KeyName;
                 dItemData.Name = spec.Name;
                dItemData.EverDieHighSpec = spec.EverDieHighSpec;
                dItemData.EverDieLowerSpec = spec.EverDieLowerSpec;
                dItemData.Caculate(false,0);
                this._data.Add("M_" + spec.KeyName, dItemData);
                // -----------------------------------------------------------------------------------------
                dItemData = new DailyResultData();
                dItemData.DataArray = delta;
                dItemData.KeyName = spec.KeyName;
                dItemData.Name = spec.Name;
                dItemData.EverDieHighSpec = spec.EverDieHighSpec;
                dItemData.EverDieLowerSpec = spec.EverDieLowerSpec;
                dItemData.AvgHighSpec = spec.ReCalibSpec;
                dItemData.AvgLowSpec = -1*spec.ReCalibSpec;
                dItemData.Spec = spec;
                dItemData.IsEnable = spec.IsEnable;
                if (spec.CriteriaType == 0)
                {
                    dItemData.Caculate(false, 0);
                }
                else
                {
                    dItemData.Caculate(true, spec.CriteriaType);
                }
                this._data.Add("D_" + spec.KeyName, dItemData);
                itemIndex++;
            }
        }

        public void FilterAndCompareData()
        {
            this.SetFilterData();
            this.FilterOrignalData();
        }

        public EDailyCheckResult GetCalcResult(bool isCheckEveryDie)
        {
            if (this._dtCompare == null || this._dtCompare.Rows.Count == 0)
                return EDailyCheckResult.NoCompareData;

            bool isResultOutSpec = false;
            int numbersOfdieOutSpec = 0;
            this._everyDieOutSpecCount = 0;

            foreach (DailyResultData result in this._data.Values)
            {
                if (!result.IsEnable)
                {
                    continue;
                }

                if (result.IsPASS == false && result.IsRunCalculate == true)
                {
                    isResultOutSpec = true;
                    this._finalResult = !isResultOutSpec;
                }
            }

            if (!isCheckEveryDie && isResultOutSpec==true)
            {
                return EDailyCheckResult.AvgBiasOutSpec;
            }


            for (int i = 0; i < this._everyDieName.Length; i++)
            {
                bool isOutSpec=false;

                foreach (DailyResultData result in this._data.Values)
                {
                    if (result.IsOutSpec[i] == true)
                    {
                        isOutSpec = result.IsOutSpec[i];
                    }             
                }
                this._everyDieResult[i] = isOutSpec;

                if (isOutSpec)
                {
                    numbersOfdieOutSpec++;
                }
            }

            if (isCheckEveryDie)
            {
                this._everyDieOutSpecCount = numbersOfdieOutSpec;
                if (numbersOfdieOutSpec >  this._dailySpecInfo.ToleranceOutSpecCount)
                {
                    isResultOutSpec = true;
                    this._finalResult = !isResultOutSpec;
                    return EDailyCheckResult.BoundayOutSpec;
                }
            }

            if (this._dailySpecInfo.MinCountAccept!=0&&        
                this._dtCompare.Rows.Count < this._dailySpecInfo.MinCountAccept)
            {
                isResultOutSpec = true;
                this._finalResult = !isResultOutSpec;
                return EDailyCheckResult.LessThanMinAcceptDies;
            }
     
             this._finalResult= !isResultOutSpec;
            return EDailyCheckResult.PASS;
        }

        public void AutoTuneFactor()
        {
           // if(_data.ContainsKey)

        }

        #endregion

        #region >>> Public Property <<<

        public Dictionary<string, DailyResultData> Data
        {
            get { return this._data; }
            set { lock (this._lockObj) { this._data = value; } }
        }

        public DailyCheckSpecInfo SpecInfo
        {
            get { return this._dailySpecInfo; }
        }

        public string[] EveryDieName
        {
            get { return this._everyDieName; }
        }

        public string[] UserDefineKeyName
        {
            get { return this._userDefineKeyName; }
        }

        public string[] UserDefineName
        {
            get { return this._displayTitleName; }
        }

        public int NumbersOfDie
        {
            get { return this.EveryDieName.Length; }
        }

        public bool[] EveryDieResult
        {
            get { return this._everyDieResult; }
        }

        public int EveryDieOutSpecCounts
        {
            get { return this._everyDieOutSpecCount; }
        }

         public  bool  FinalResult
        {
            get { return this._finalResult; }
        }

         public string ResultDescribe
         {
             get { return this._resultDescribe; }
         }

        #endregion

        #region >>> Private Method <<<

        private void ParseTitleData()
        {
            this._displayTitleName = new string[this._userDefineKeyName.Length];

            for (int i = 0; i < this._userDefineKeyName.Length; i++)
            {
                string keyname = this._userDefineKeyName[i].Substring(this._userDefineKeyName[i].IndexOf("_") + 1);
                string s = this._userDefineKeyName[i].Remove(this._userDefineKeyName[i].IndexOf("_") + 1);

                if (this._titleName.ContainsKey(keyname))
                {
                    string name = s + this._titleName[keyname];
                    this._displayTitleName[i] = name;
                }
                else
                {
                    this._displayTitleName[i] = this._userDefineKeyName[i];
                }
            }
        }

        private void SetFilterData()
        {
            this._dailyFilterDic.Clear();

            foreach (DailyWatchSpec spec in this._dailySpecInfo.Data.Values)
            {
                FilterData filterData = new FilterData();

                filterData.IsEnable = spec.IsEnbaleFilter;

                if (spec.IsEnbaleFilter)
                {
                    filterData.Max = spec.MaxValue;
                    filterData.Min = spec.MinValue;
                }
                this._dailyFilterDic.Add(spec.KeyName, filterData);
            }
        }

        #endregion
    }
}
