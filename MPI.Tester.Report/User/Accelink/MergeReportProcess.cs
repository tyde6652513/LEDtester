using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

using MPI.Tester.Data;

using MPI.Tester.Report.BaseMethod.HeaderFinder;
using MPI.Tester.Report.BaseMethod.PosKeyMaker;
using MPI.Tester.Report.BaseMethod.MapReader;
using MPI.Tester.Report.BaseMethod.Merge;

namespace MPI.Tester.Report.User.Accelink
{
    partial class Report : ReportBase
    {
        const string MAP_SET_EXTEND = "accMap";
        
        protected override EErrorCode ProcessAfterWaferFinished()//在WaferFinished後啟動，目前是設計來啟動合檔用
        {
            EErrorCode err = EErrorCode.NONE;

            Console.WriteLine("[Report_Accelink],ProcessAfterWaferFinished()");

            try
            {
                if (!this.UISetting.IsManualRunMode)
                {
                    string srcPath01 = GetPathWithFolder(UISetting.OutPathInfo01);

                    string mergeOutPath01 = GetPathWithFolder(this.UISetting.MergeFilePath);

                    //在sampling才會合併檔案，因此override
                    if (Product.CustomerizedSetting.IsMergeReport &&
                        UISetting.MergeFilePath.EnablePath &&
                        UISetting.FileInProcessList != null && UISetting.FileInProcessList.Count > 1)
                    {
                        string MergeTarPath = "";
                        if (Directory.Exists(srcPath01))
                        {
                            string tarFolder = GetPathWithFolder(UISetting.MergeFilePath);

                            string mergeFileNamewithoutExten = this.GetOutputFileName((int)UISetting.EFileNameFormatPresent) ;

                            MergeTarPath = Path.Combine(tarFolder, mergeFileNamewithoutExten + "_Merge." + UISetting.TestResultFileExt);

                            List<string> strList = new List<string>();
                            foreach (string str in UISetting.FileInProcessList)
                            {
                                string pStr = Path.Combine(srcPath01, str);
                                strList.Add(pStr);
                            }
                            err = MergeFile(MergeTarPath, strList);
                        }
                        else
                        {
                            Console.WriteLine("[ReportBase],ProcessAfterWaferFinished(),Err: REPORT_Merge_FilePathError");
                            err = EErrorCode.REPORT_Merge_FilePathError;
                        }


                        if (UISetting.PathInfoArr[0].EnablePath)
                        {
                            string folderName = GetPathWithFolder(UISetting.PathInfoArr[0]);
                            string fileName = Path.GetFileNameWithoutExtension(MergeTarPath);
                            string tarFileName = Path.Combine(folderName, fileName + ".jpg");
                            string settingFilePath = GetcustomizePath();

                            AccelinkMapCreator.CreateAcceLinkWaferMap(MergeTarPath, tarFileName, settingFilePath);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[Report_Accelink],ProcessAfterWaferFinished(),Exception:" + e.Message);
            }

            return err;
        }

        #region
        public override EErrorCode MergeFile(string outputPath, List<string> fileList = null)
        {
            Console.WriteLine("[AccelinkReport],MergeFile()");

            if (_crKeyMaker == null)
            {
                List<int> colList = new List<int>();
                _crKeyMaker = new PosKeyMakerBase(this._resultTitleInfo.ColIndex, this._resultTitleInfo.RowIndex, colList);
            }

            ReportMerger merger = new ReportMerger(this.UISetting,
                new HeaderFinderBase(this.TitleStrKey, TitleStrShift), ResultTitleInfo, _crKeyMaker);

            merger.OldFirstRow = new List<string>() { "TestTime" };
            merger.OldFirstRow.Add("TestTime");

            merger.SpecialRuleRow = new List<string>() { "FileName", "Item,Bias,BiasUnit,Time(ms)" };


            merger.InfoAppendRow = new List<string>() { "Stage", "Recipe", "TaskFile", "ConditionFileName","BinFileName" };

            merger.IVFirstKeyList = new List<string>() { "POLAR", "BIN", "AOISIGN", "OCR" };

            merger.SamplingFirstKeyList = new List<string>() ;
            for (int i = 0; i < 20; ++i)
            {
                string str ="VISWVP_" + i.ToString("0");
                merger.SamplingFirstKeyList.Add(str);
            }

            EErrorCode result = merger.MergeFile(outputPath, fileList);
            //if(result == EErrorCode.NONE)
            //{
            //    result =  AddReportCode(outputPath);//報表可能由AOI加工，因此不應加這個
            //}

            return result;

            //return AddReportCode(this.FileFullNameRep);
        }

        public string GetcustomizePath()
        {
            string productFolder = this.UISetting.ProductPath;

            string fileNameWithExt = this.UISetting.ProductFileName + "." + MAP_SET_EXTEND;

            string path = Path.Combine(productFolder, fileNameWithExt);
            return path;
        }

        #endregion


    }


    public static class AccelinkMapCreator
    {
        const string MAP_SET_EXTEND = "accMap";
        const string CSV2PYONVERTER_PATH = @"C:\MPI\LEDTester\Tools\CSVToWaferMap\CSVToWaferMapTool.exe";
         #region
        public static EErrorCode CreateAcceLinkWaferMap(string srcFileName, string tarFileName, string settingFilePath)
        {
            EErrorCode err = EErrorCode.NONE;
            
            Process p = new Process();

            List<SelectItem> sList = CSVToList(settingFilePath);
            
            p.StartInfo.FileName = CSV2PYONVERTER_PATH;


            string str = File.Exists(p.StartInfo.FileName).ToString();
            string s2 = str;
            string sArguments = srcFileName + " " + tarFileName;

            List<string> mapItemList = (from s in sList
                                        where s.ShowOnMap
                                        select s.Name).ToList();

            string mapItemStr = ListToStr(mapItemList);

            List<string> statisticItemList = (from s in sList
                                        where s.ShowOnMap == false
                                        select s.Name).ToList();

            string stItemStr = ListToStr(statisticItemList);

            sArguments += " " + mapItemStr + " " + stItemStr;

            p.StartInfo.Arguments = sArguments;

            p.StartInfo.UseShellExecute = false;

            p.StartInfo.RedirectStandardOutput = true;

            p.StartInfo.RedirectStandardInput = true;

            p.StartInfo.RedirectStandardError = true;

            p.StartInfo.CreateNoWindow = true;

            p.Start();

            return err;
        }

        #endregion

        #region
        private static  List<SelectItem> CSVToList(string srcFileName)
        {
            List<SelectItem> sList = new List<SelectItem>();
            try
            {
                using (StreamReader sr = new StreamReader(srcFileName))
                {
                    while (sr.Peek() >= 0)
                    {
                        string str = sr.ReadLine();
                        string[] strArr = str.Split(',');
                        if (strArr != null && strArr.Length == 2)
                        {
                            bool isShow = true;
                            if (bool.TryParse(strArr[1], out isShow))
                            {
                                SelectItem sData = new SelectItem(strArr[0], isShow);
                                sList.Add(sData);
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[Report_Accelink],CSVToList(),Exception:" + e.Message);
            }

            return sList;
        }

        private static string ListToStr(List<string> sList)
        {
            int length = sList.Count;
            string outStr = ",";
            for (int i = 0; i < length; ++i)
            {
                outStr += "," + sList[i];
            }
            return outStr.TrimStart(',');
        }
        #endregion
 
    }



    public class SelectItem
    {
        public string Name { get; set; }
        public bool ShowOnMap { get; set; }

        public SelectItem()
        {
            Name = "";
            ShowOnMap = false;
        }

        public SelectItem(string name, bool show)
        {
            Name = name;
            ShowOnMap = show;
        }

    }



}
