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

        public override EErrorCode MergeFile(string outputPath, List<string> fileList = null)
        {
            Console.WriteLine("[AccelinkReport],MergeFile()");

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
            return merger.MergeFile(outputPath, fileList);
        }


    }
}
