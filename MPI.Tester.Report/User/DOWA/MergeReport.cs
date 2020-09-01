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

namespace MPI.Tester.Report.User.DOWA
{
    partial class Report : ReportBase
    {
        public override EErrorCode MergeFile(string outputPath, List<string> fileList = null)
        {
            Console.WriteLine("[DOWAReport],MergeFile()");

            if (_crKeyMaker == null)
            {
                List<int> colList = new List<int>();
                if (this._resultTitleInfo.ChipIndexIndex >= 0)
                {
                    colList.Add(this._resultTitleInfo.ChipIndexIndex);
                }
                _crKeyMaker = new PosKeyMakerBase(this._resultTitleInfo.ColIndex, this._resultTitleInfo.RowIndex, colList);
            }

            ReportMergerBase merger = new ReportMergerBase(this.UISetting,
                new HeaderFinderBase(this.TitleStrKey, TitleStrShift), ResultTitleInfo, _crKeyMaker);

            merger.OldFirstRow = new List<string>() { "TestTime" };

            merger.SpecialRuleRow = new List<string>() { "FileName", "Item,Bias,BiasUnit,Time(ms)" };

            merger.InfoAppendRow = new List<string>() { "Stage", "Recipe", "TaskFile", "ConditionFileName", "BinFileName" };

            merger.IVFirstKeyList = new List<string>() { "POLAR", "BIN", "AOISIGN", "TestTemp1" };

            List<string> ivFKeyList = new List<string>() { "POLAR", "BIN", "TestTemp1" };
            List<string> cvFKeyList = new List<string>() { "BINCV", "TestTemp2" };

            merger.SamplingFirstKeyList = new List<string>();


            for (int i = 2; i < 20; ++i)
            {
                merger.IVFirstKeyList.Add("MCALC_" + i.ToString());//A/W
                merger.CVFirstKeyList.Add("LCRCP_" + i.ToString());//A/W
            }

            return merger.MergeFile(outputPath, fileList);

        }
    }
}
