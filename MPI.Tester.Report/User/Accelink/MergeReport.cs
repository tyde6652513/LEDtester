using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MPI.Tester.Data;

using MPI.Tester.Report.BaseMethod.HeaderFinder;
using MPI.Tester.Report.BaseMethod.PosKeyMaker;
using MPI.Tester.Report.BaseMethod.MapReader;

namespace MPI.Tester.Report.User.Accelink
{
    partial class Report : ReportBase
    {
        List<string> oldFirstRow = new List<string>();
        List<string> spRuleRow = new List<string>();
        List<string> infoAppendRow = new List<string>();
        List<string[]> headerArrList = new List<string[]>();
        List<string> testConditionRow = new List<string>();
        Dictionary<string, string[]> posStrArrDic = new Dictionary<string, string[]>();

        List<int> ivFirstCol = new List<int>();//搜尋數量不多，用list省事

        ETestStage stageOfParsingFile = ETestStage.IV;
        string _mergeFileNamewithoutExten = DateTime.Now.ToString("Merge_yyyyMMddHHmmss");


        #region
        public override EErrorCode MergeFile(string outputPath, List<string> fileList = null)
        {
            Console.WriteLine("[AccelinkReport],MergeFile()");
            oldFirstRow = new List<string>();
            oldFirstRow.Add("TestTime");
            spRuleRow = new List<string>();
            spRuleRow.Add("FileName");

            spRuleRow.Add("Item,Bias,BiasUnit,Time(ms)");

            infoAppendRow = new List<string>();
            infoAppendRow.Add("Stage");
            infoAppendRow.Add("Recipe");
            


            _mergeFileNamewithoutExten = Path.GetFileNameWithoutExtension(outputPath);

            List<string> ivFKeyList = new List<string>() { "POLAR", "BIN", "AOISIGN" };
         
            ivFirstCol = GetKeyColList(ivFKeyList);

            posStrArrDic = new Dictionary<string, string[]>();
            //List<string> headerText = new List<string>();
            List<string> nameList = new List<string>();
            Dictionary<int, string> colNameDic = new Dictionary<int, string>();


            testConditionRow = new List<string>();

            if (fileList != null && fileList.Count > 1)
            {
                List<string> fileOrderByLastWriteTime = (from path in fileList
                                                where File.Exists(path)
                                                orderby File.GetLastWriteTime(path)
                                                select path).ToList();//按照產出時間先後排序
                fileList = fileOrderByLastWriteTime;
                for (int fCnt = 0; fCnt < fileList.Count ; ++fCnt)
                {
                    Console.WriteLine("[AccelinkReport],MergeFile(),read file:" + fileList[fCnt]);

                    HeaderFinder hf = new HeaderFinder(this.TitleStrKey, TitleStrShift);
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
                                    state = ParseTestCondition(line, hf);
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

            Console.WriteLine("[ReportAccelink],MergeFile(),write out data");
            string mergeTmpPath = @"C:\MPI\Temp2\mergeTemp.csv";
            if (File.Exists(mergeTmpPath))
            {
                MPIFile.DeleteFile(mergeTmpPath);
            }
            using (StreamWriter sw = new StreamWriter(mergeTmpPath, append: false))
            {
                #region
                foreach (var sArr in headerArrList)
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
                foreach (var str in testConditionRow)
                {
                    sw.WriteLine(str);
                }
                sw.WriteLine("");

                sw.WriteLine(this._resultTitleInfo.TitleStr);
                sw.Flush();
                foreach (var p in posStrArrDic.Values)
                {
                    string outStr = "";
                    int legnth = p.Length;
                    int cnt = 1;
                    foreach (string str in p)
                    {
                        outStr += str;
                        if (cnt < legnth) { outStr += this.SpiltChar.ToString(); }
                        ++cnt;
                    }
                    sw.WriteLine(outStr);
                    sw.Flush();
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

        private List<int> GetKeyColList(List<string> keyList)
        {
            List<int> colList = new List<int>();

            foreach (string key in keyList)
            {
                int col = ResultTitleInfo.GetIndexOfKey(key);
                if (col >= 0)
                {
                    colList.Add(col);
                }
            }
            return colList;
        }
        #endregion

        #region
        private EParsingState ParseTestInfo(string line, int fCnt, int nowRow)
        {
            EParsingState state = EParsingState.TesterInfo;
            if (fCnt == 0)
            {
                #region
                if (line != "")
                {
                    if (line.StartsWith("Item,Bias,BiasUnit,Time(ms)"))
                    {
                        Console.WriteLine("[Report],ParseTestInfo(),state to EParsingState.TestCondition");

                        state = EParsingState.TestCondition;
                    }
                    string[] strArr = line.Split(this.SpiltChar);
                    if (line.StartsWith("FileName"))
                    {
                        if (strArr.Length >= 2)
                        {
                            string fileName = _mergeFileNamewithoutExten + "." + UISetting.TestResultFileExt;
                            strArr[1] = fileName;
                        }

                    }
                    headerArrList.Add(strArr);
                }
                else
                {
                    headerArrList.Add(null);
                }
                #endregion
            }
            else
            {
                #region >>header merge<<
                if (line != "" && headerArrList[nowRow] != null)
                {
                    string[] strArr = line.Split(this.SpiltChar);
                    string[] oldStrArr = headerArrList[nowRow];

                    if (IsStartInRefList(line, infoAppendRow))
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
                            headerArrList[nowRow] = new string[] { key,  ("\"" + mergeStr.TrimEnd(',') + "\"") };
                        }

                        if (line.StartsWith("Stage"))
                        {
                            string tStr = line.ToUpper();
                            if (tStr.Contains("LCR"))
                            {
                                stageOfParsingFile = ETestStage.LCR;
                            }
                            else
                            {
                                stageOfParsingFile = ETestStage.IV;
                            }
                            Console.WriteLine("[Report],ParseTestInfo(),stageOfParsingFile:" + stageOfParsingFile.ToString());
                        }
                    }
                    else if (IsStartInRefList(line, oldFirstRow))
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
                            headerArrList[nowRow] = strArr;
                        }
                        #endregion
                    }
                    else if (IsStartInRefList(line, spRuleRow))
                    {
                        if (line.StartsWith("Item,Bias,BiasUnit,Time(ms)"))
                        {
                            Console.WriteLine("[Report],ParseTestInfo(),state to EParsingState.TestCondition");
                            
                            state = EParsingState.TestCondition;
                        }
                    }
                    else
                    {
                        headerArrList[nowRow] = strArr;
                    }
                }
                #endregion

            }
            return state;
        }

        private EParsingState ParseTestCondition(string line,HeaderFinder hf)
        {
            EParsingState state = EParsingState.TestCondition;

            if (hf.CheckIfRowData(line))
            {
                Console.WriteLine("[Report],ParseTestInfo(),state to EParsingState.TestCondition");
                state = EParsingState.MsrtData;
            }
            else
            {
                if (line != null && line.Trim() != "")
                {
                    if (!testConditionRow.Contains(line))
                        testConditionRow.Add(line);
                }
            }

            return state;
        }

        private EParsingState ParseMsrtData(string line)
        {
            EParsingState state = EParsingState.MsrtData;

            string[] rawData = line.Split(this.SpiltChar);

            string colrowKey = ColRowKeyMaker(rawData);

            if (!posStrArrDic.ContainsKey(colrowKey))
            {
                posStrArrDic.Add(colrowKey, rawData);
            }
            else
            {
                string[] tempArr = posStrArrDic[colrowKey];
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
                            else if (stageOfParsingFile == ETestStage.IV && ivFirstCol.Contains(i))
                            {
                                tempArr[i] = rawData[i];
                            }
                        }
                    }
                }
                posStrArrDic[colrowKey] =tempArr ;
            }

            return state;
        }

        #endregion
        #region
        internal enum EParsingState:int
        {
            TesterInfo = 1,
            TestCondition = 2,
            MsrtData = 3,
        }
        #endregion

    }
}
