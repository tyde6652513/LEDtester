using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;
using System.IO;
using MPI.Tester.TestServer;
using MPI.Tester.Maths;
using MPI.Tester.TestKernel;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Report.BaseMethod.HeaderFinder;
using MPI.Tester.Report.BaseMethod.PosKeyMaker;
using MPI.Tester.Report.BaseMethod.Merge;

namespace MPI.Tester.Report
{
    public abstract partial class ReportBase
    {
        protected virtual EErrorCode PostProcessAfterMoveFile(EServerQueryCmd cmd)//在MoveFileToTarget後啟動，目前是設計來啟動合檔用
        {
            EErrorCode err = EErrorCode.NONE;

            return err;
        }

        protected virtual EErrorCode ProcessAfterWaferFinished()//在WaferFinished後啟動，目前是設計來啟動光磊合檔用
        {
            EErrorCode err = EErrorCode.NONE;

            Console.WriteLine("[ReportBase],ProcessAfterWaferFinished()");

            if (!this.UISetting.IsManualRunMode)
            {
                string srcPath01 = GetPathWithFolder(UISetting.OutPathInfo01);

                string mergeOutPath01 = GetPathWithFolder(this.UISetting.MergeFilePath);

                if (UISetting.MergeFilePath.EnablePath &&
                    UISetting.FileInProcessList != null && UISetting.FileInProcessList.Count > 1)
                {
                    if (Directory.Exists(srcPath01))
                    {
                        string tarFolder = GetPathWithFolder(UISetting.MergeFilePath);

                        string mergeFileNamewithoutExten = this.GetOutputFileName((int)UISetting.EMergeFileNameFormatPresent);

                        string tarPath = Path.Combine(tarFolder, mergeFileNamewithoutExten + "." + UISetting.TestResultFileExt);

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

        public virtual EErrorCode MergeFile(string outputPath, List<string> fileList = null)
        {
            Console.WriteLine("[ReportBase],MergeFile()");

            if (_crKeyMaker == null)
            {
                List<int> colList = new List<int>();
                if (this._resultTitleInfo.ChipIndexIndex >= 0)
                {
                    colList.Add(this._resultTitleInfo.ChipIndexIndex);
                }
                _crKeyMaker = new PosKeyMakerBase(this._resultTitleInfo.ColIndex, this._resultTitleInfo.RowIndex, colList);
            }

            ReportMerger merger = new ReportMerger(this.UISetting,
                new HeaderFinderBase(this.TitleStrKey, TitleStrShift), ResultTitleInfo, _crKeyMaker);

            return merger.MergeFile(outputPath, fileList);
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
    }
}
