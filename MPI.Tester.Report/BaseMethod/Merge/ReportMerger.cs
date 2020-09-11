using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MPI.Tester.Data;

using MPI.Tester.Report.BaseMethod.HeaderFinder;
using MPI.Tester.Report.BaseMethod.PosKeyMaker;
using MPI.Tester.Report.BaseMethod.MapReader;


namespace MPI.Tester.Report.BaseMethod.Merge
{
    public class ReportMerger
    {

        List<string[]> _headerArrList = new List<string[]>();
        List<string> _testConditionRow = new List<string>();
        Dictionary<string, string[]> _posStrArrDic = new Dictionary<string, string[]>();  
        ETestStage stageOfParsingFile = ETestStage.IV;
        string _mergeFileNamewithoutExten = DateTime.Now.ToString("Merge_yyyyMMddHHmmss");

        protected List<int> _ivFirstCol = new List<int>();//搜尋數量不多，用list省事
        protected List<int> _cvFirstCol = new List<int>();//搜尋數量不多，用list省事
        protected List<int> _samplingFirstCol = new List<int>();//搜尋數量不多，用list省事

        protected List<string> _d4headerList = new List<string>();
        protected List<string> _headerList = new List<string>();

        HeaderFinderBase _headerFinder;
        UISetting _uiSetting;
        ResultTitleInfo _d4ResultTitleInfo;
        PosKeyMakerBase _crKeyMaker;
        //ResultTitleInfo _d4ResultTitleInfo = null;//給可重新命名的檔頭做為參考用



        #region
        //改成default ResultTitleInfo是為了merge 時能夠確認該欄位是否被改過名稱
        public ReportMerger(UISetting uiset, HeaderFinderBase hf, ResultTitleInfo d4Rti, PosKeyMakerBase posMaker)
        {
            _uiSetting = uiset;
            _headerFinder = hf;
            _d4ResultTitleInfo = d4Rti;
            _crKeyMaker = posMaker;

            _headerList = new List<string>(_d4ResultTitleInfo.TitleStr.Split(this.SpiltChar));

            _d4headerList = new List<string>(_d4ResultTitleInfo.TitleStr.Split(this.SpiltChar));

            if (_crKeyMaker == null)
            {
                List<int> colList = new List<int>();
                if (this._d4ResultTitleInfo.ChipIndexIndex >= 0)
                {
                    colList.Add(this._d4ResultTitleInfo.ChipIndexIndex);
                }
                _crKeyMaker = new PosKeyMakerBase(this._d4ResultTitleInfo.ColIndex, this._d4ResultTitleInfo.RowIndex, colList);
            }
        }
        #endregion

        #region public property
        public List<string> OldFirstRow = new List<string>();
        public List<string> SpecialRuleRow = new List<string>();
        public List<string> InfoAppendRow = new List<string>();
        public List<string> IVFirstKeyList = new List<string>();
        public List<string> CVFirstKeyList = new List<string>();
        public List<string> SamplingFirstKeyList = new List<string>();
        public string DUTCountKey = "TEST";
        public int DUTCountCol = -1;
        public char SpiltChar = ',';
        #endregion

        #region public method
        public EErrorCode MergeFile(string outputPath, List<string> fileList = null)
        {
            Console.WriteLine("[ReportMerger],MergeFile()");         

            _mergeFileNamewithoutExten = Path.GetFileNameWithoutExtension(outputPath);

            _cvFirstCol = GetKeyColList(CVFirstKeyList);

            _ivFirstCol = GetKeyColList(IVFirstKeyList);

            _samplingFirstCol = GetKeyColList(SamplingFirstKeyList);

            List<string> sList = new List<string>() { DUTCountKey };
            DUTCountCol = GetKeyColList(sList)[0]; 
            _posStrArrDic = new Dictionary<string, string[]>();
            //List<string> headerText = new List<string>();
            List<string> nameList = new List<string>();
            Dictionary<int, string> colNameDic = new Dictionary<int, string>();


            _testConditionRow = new List<string>();

            if (fileList != null && fileList.Count > 1)
            {
                List<string> fileOrderByLastWriteTime = (from path in fileList
                                                         where File.Exists(path)
                                                         orderby File.GetLastWriteTime(path)
                                                         select path).ToList();//按照產出時間先後排序
                fileList = fileOrderByLastWriteTime;
                for (int fCnt = 0; fCnt < fileList.Count; ++fCnt)
                {
                    Console.WriteLine("[ReportMerger],MergeFile(),read file:" + fileList[fCnt]);

                    _headerFinder.Reset();

                    using (StreamReader sr = new StreamReader(fileList[fCnt]))
                    {
                        int nowRow = 0;
                        EParsingState state = EParsingState.TesterInfo;

                        while (sr.Peek() >= 0)
                        {
                            string line = sr.ReadLine();
                            switch (state)
                            {
                                case EParsingState.TesterInfo:
                                    state = ParseTestInfo(line, fCnt, nowRow);
                                    break;
                                case EParsingState.TestCondition:
                                    state = ParseTestCondition(line, _headerFinder);
                                    break;
                                case EParsingState.MsrtData:
                                    state = ParseMsrtData(line);
                                    break;
                            }
                            nowRow++;
                        }
                    }
                }
            }

            Console.WriteLine("[ReportMerger],MergeFile(),write out data");
            string mergeTmpPath = @"C:\MPI\Temp2\mergeTemp.csv";
            if (File.Exists(mergeTmpPath))
            {
                MPIFile.DeleteFile(mergeTmpPath);
            }
            using (StreamWriter sw = new StreamWriter(mergeTmpPath, append: false))
            {
                #region
                foreach (var sArr in _headerArrList)
                {
                    #region
                    string outStr = "";
                    if (sArr != null)
                    {
                        int arrLen = sArr.Length;
                        for (int i = 0; i < sArr.Length; ++i)
                        {
                            outStr += sArr[i];
                            if (i < arrLen - 1)
                            { outStr += this.SpiltChar.ToString(); }
                        }
                    }
                    sw.WriteLine(outStr);
                    #endregion
                }
                sw.Flush();
                foreach (var str in _testConditionRow)
                {
                    sw.WriteLine(str);
                }
                sw.WriteLine("");


                string headerStr = "";

                for (int i = 0; i < _headerList.Count; ++i)
                {
                    headerStr += _headerList[i];
                    if (i < _headerList.Count - 1)
                    {
                        headerStr += ",";
                    }
                }
                sw.WriteLine(headerStr);

                //sw.WriteLine(this._resultTitleInfo.TitleStr);
                sw.Flush();
                int DUTCount = 1;
                foreach (var p in _posStrArrDic.Values)
                {
                    string outStr = "";
                    int legnth = p.Length;
                    int cnt = 0;
                    foreach (string str in p)
                    {
                        if (cnt == DUTCountCol)
                        {
                            outStr += (DUTCount).ToString();
                        }
                        else
                        {
                            outStr += str;
                        }
                        if (cnt < legnth-1) { outStr += this.SpiltChar.ToString(); }
                        ++cnt;
                    }
                    sw.WriteLine(outStr);
                    sw.Flush(); 
                    ++DUTCount;
                }
                #endregion
            }
            if (File.Exists(outputPath))//檢查備份
            {
                string fileName = Path.GetFileName(outputPath);
                string backupFullName = Path.Combine(Constants.Paths.MPI_BACKUP_DIR, fileName);
                MPIFile.CopyFile(outputPath, backupFullName);
            }
            MPIFile.CopyFile(mergeTmpPath, outputPath);

            return EErrorCode.NONE;
        }
        #endregion

        #region protected method
        protected EParsingState ParseTestInfo(string line, int fCnt, int nowRow)
        {
            EParsingState state = EParsingState.TesterInfo;
            if (fCnt == 0)
            {
                #region
                if (line != "")
                {
                    if (line.StartsWith("Item,Bias,BiasUnit,Time(ms)"))
                    {
                        Console.WriteLine("[ReportMerger],ParseTestInfo(),state to EParsingState.TestCondition");

                        state = EParsingState.TestCondition;
                    }
                    string[] strArr = line.Split(this.SpiltChar);
                    if (line.StartsWith("FileName"))
                    {
                        if (strArr.Length >= 2)
                        {
                            string fileName = _mergeFileNamewithoutExten + "." + this._uiSetting.TestResultFileExt;
                            strArr[1] = fileName;
                        }

                    }
                    _headerArrList.Add(strArr);
                }
                else
                {
                    _headerArrList.Add(null);
                }
                #endregion
            }
            else
            {
                #region >>header merge<<
                if (line != "" && _headerArrList[nowRow] != null)
                {
                    string[] strArr = line.Split(this.SpiltChar);
                    string[] oldStrArr = _headerArrList[nowRow];

                    if (IsStartInRefList(line, InfoAppendRow))
                    {
                        if (strArr != null && oldStrArr != null &&
                            strArr.Length >= 2 && oldStrArr.Length >= 2)
                        {
                            #region
                            string key = strArr[0];
                            string mergeStr = "";
                            for (int i = 1; i < oldStrArr.Length; ++i)
                            {
                                if (oldStrArr[i] == "" || oldStrArr[i] == "\"")
                                {
                                    continue;
                                }
                                string clearStr = oldStrArr[i].Replace("\"", "");
                                mergeStr += clearStr + ",";
                            }
                            for (int i = 1; i < strArr.Length; ++i)
                            {
                                if (strArr[i] == "" || strArr[i] == "\"")
                                {
                                    continue;
                                }
                                string clearStr = strArr[i].Replace("\"", "");
                                mergeStr += clearStr + ",";
                            }
                            #endregion
                            _headerArrList[nowRow] = new string[] { key, ("\"" + mergeStr.TrimEnd(',') + "\"") };
                        }

                        if (line.StartsWith("Stage"))
                        {
                            string tStr = line.ToUpper();
                            if (tStr.Contains("LCR"))
                            {
                                stageOfParsingFile = ETestStage.LCR;
                            }
                            else if (tStr.Contains("_Sampling"))
                            {
                                 stageOfParsingFile = ETestStage.Sampling;
                            }
                            else if (tStr.Contains("_Sampling1"))
                            {
                                stageOfParsingFile = ETestStage.Sampling1;
                            }
                            else if (tStr.Contains("_Sampling2"))
                            {
                                stageOfParsingFile = ETestStage.Sampling2;
                            }
                            else
                            {
                                stageOfParsingFile = ETestStage.IV;
                            }
                            Console.WriteLine("[ReportMerger],ParseTestInfo(),stageOfParsingFile:" + stageOfParsingFile.ToString());
                        }
                    }
                    else if (IsStartInRefList(line, OldFirstRow))
                    {
                        #region
                        if (strArr.Length == oldStrArr.Length)
                        {
                            for (int i = 0; i < strArr.Length && i < oldStrArr.Length; ++i)
                            {
                                if (oldStrArr[i] != "")
                                {
                                    strArr[i] = oldStrArr[i];
                                }
                            }
                            _headerArrList[nowRow] = strArr;
                        }
                        #endregion
                    }
                    else if (IsStartInRefList(line, SpecialRuleRow))
                    {
                        if (line.StartsWith("Item,Bias,BiasUnit,Time(ms)"))
                        {
                            Console.WriteLine("[ReportMerger],ParseTestInfo(),state to EParsingState.TestCondition");

                            state = EParsingState.TestCondition;
                        }
                    }
                    else
                    {
                        _headerArrList[nowRow] = strArr;
                    }
                }
                #endregion

            }
            return state;
        }

        protected EParsingState ParseTestCondition(string line, HeaderFinderBase hf)
        {
            EParsingState state = EParsingState.TestCondition;

            if (line != null )
                {
                    if (line.Trim() != "")
                    {
                        if (hf.IsFitTarStr(line))
                        {
                            string[] rawData = line.Split(this.SpiltChar);

                            for (int i = 0; rawData != null && i < _d4headerList.Count && i<rawData.Length; ++i)
                            {
                                if (rawData[i] != _d4headerList[i])
                                {
                                    _headerList[i] = rawData[i];
                                }
                            }
                        }
                    }

                    if (hf.CheckIfRowData(line))
                    {
                        Console.WriteLine("[ReportMerger],ParseTestInfo(),state to EParsingState.TestCondition");
                        state = EParsingState.MsrtData;
                    }
                    else
                    {
                        if (line.Trim() != "")
                        {
                            if (!_testConditionRow.Contains(line))
                                _testConditionRow.Add(line);
                        }
                    }
            }

            return state;
        }

        protected EParsingState ParseMsrtData(string line)
        {
            EParsingState state = EParsingState.MsrtData;

            string[] rawData = line.Split(this.SpiltChar);

            string colrowKey = _crKeyMaker.GetPosKey(rawData);

            if (!_posStrArrDic.ContainsKey(colrowKey))
            {
                _posStrArrDic.Add(colrowKey, rawData);
            }
            else
            {
                string[] tempArr = _posStrArrDic[colrowKey];
                int minLen = Math.Min(rawData.Length, tempArr.Length);
                for (int i = 0; i < minLen; ++i)
                {
                    if (rawData[i] != null)
                    {

                        if (rawData[i] != "")
                        {
                            if (tempArr[i] == "")
                            {
                                tempArr[i] = rawData[i];
                            }
                            else
                            {
                                switch (stageOfParsingFile)
                                {
                                    case ETestStage.IV:
                                        {
                                            if (_ivFirstCol.Contains(i))
                                            {
                                                tempArr[i] = rawData[i];
                                            }
                                        }
                                        break;
                                    case ETestStage.LCR:
                                        {
                                            if (_cvFirstCol.Contains(i))
                                            {
                                                tempArr[i] = rawData[i];
                                            }
                                        }
                                        break;
                                    case ETestStage.Sampling:
                                    case ETestStage.Sampling1:
                                    case ETestStage.Sampling2:
                                        {
                                            if (_samplingFirstCol.Contains(i))
                                            {
                                                tempArr[i] = rawData[i];
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
                _posStrArrDic[colrowKey] = tempArr;
            }

            return state;
        }

        protected bool IsStartInRefList(string strIn, List<string> strList)
        {
            if (strList != null)
            {
                foreach (string str in strList)
                {
                    if (strIn.StartsWith(str))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected List<int> GetKeyColList(List<string> keyList)
        {
            List<int> colList = new List<int>();

            foreach (string key in keyList)
            {
                int col = _d4ResultTitleInfo.GetIndexOfKey(key);
                if (col >= 0)
                {
                    colList.Add(col);
                }
            }
            return colList;
        }

        
        #endregion
        
    }

}
