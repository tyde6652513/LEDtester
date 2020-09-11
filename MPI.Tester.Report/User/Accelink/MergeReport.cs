using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MPI.Tester.Data;

using MPI.Tester.Report.BaseMethod.HeaderFinder;
using MPI.Tester.Report.BaseMethod.PosKeyMaker;
using MPI.Tester.Report.BaseMethod.MapReader;
using MPI.Tester.Report.BaseMethod.Merge;

namespace MPI.Tester.Report.User.Accelink
{
    partial class Report : ReportBase
    {
        protected override EErrorCode ProcessAfterWaferFinished()//在WaferFinished後啟動，目前是設計來啟動光磊合檔用
        {
            EErrorCode err = EErrorCode.NONE;

            Console.WriteLine("[Report_Accelink],ProcessAfterWaferFinished()");

            if (!this.UISetting.IsManualRunMode)
            {
                string srcPath01 = GetPathWithFolder(UISetting.OutPathInfo01);

                string mergeOutPath01 = GetPathWithFolder(this.UISetting.MergeFilePath);

                //在sampling才會合併檔案，因此override
                if (Product.CustomerizedSetting.IsMergeReport &&
                    UISetting.MergeFilePath.EnablePath &&
                    UISetting.FileInProcessList != null && UISetting.FileInProcessList.Count > 1)
                {
                    if (Directory.Exists(srcPath01))
                    {
                        string tarFolder = GetPathWithFolder(UISetting.MergeFilePath);

                        string mergeFileNamewithoutExten = this.GetOutputFileName((int)UISetting.EMergeFileNameFormatPresent);

                        string tarPath = Path.Combine(tarFolder, mergeFileNamewithoutExten + "_Merge." + UISetting.TestResultFileExt);

                        List<string> strList = new List<string>();
                        foreach (string str in UISetting.FileInProcessList)
                        {
                            string pStr = Path.Combine(srcPath01, str);
                            strList.Add(pStr);
                        }
                        err = MergeFile(tarPath, strList);
                    }
                    else
                    {
                        Console.WriteLine("[ReportBase],ProcessAfterWaferFinished(),Err: REPORT_Merge_FilePathError");
                        err = EErrorCode.REPORT_Merge_FilePathError;
                    }
                }
            }

            return err;
        }


        public override EErrorCode MergeFile(string outputPath, List<string> fileList = null)
        {
            Console.WriteLine("[AccelinkReport],MergeFile()");

            if (_crKeyMaker == null)
            {
                List<int> colList = new List<int>();
   
                _crKeyMaker = new PosKeyMakerBase(this._resultTitleInfo.ColIndex, this._resultTitleInfo.RowIndex, colList);
            }

            string headerStr = "";
            int maxCnt = 4;
            //for (int i = 0; i < 4; ++i)
            int cnt = 0;
            foreach (var mItem in ResultTitleInfo)
            {
                headerStr += mItem.Value + ",";
                if (cnt >= maxCnt)
                {
                    break;
                }
                cnt++;
            }

            HeaderFinder_ByStartStr hf = new HeaderFinder_ByStartStr(headerStr, TitleStrShift);

            ReportMerger merger = new ReportMerger(this.UISetting, hf, ResultTitleInfo, _crKeyMaker);

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
            if(result == EErrorCode.NONE)
            {
                result =  AddReportCode(outputPath);
            }

            return result;

            //return AddReportCode(this.FileFullNameRep);
        }


    }



}
