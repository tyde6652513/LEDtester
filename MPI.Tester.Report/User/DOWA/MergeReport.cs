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

            #region
            if (_crKeyMaker == null)
            {
                List<int> colList = new List<int>();
                if (this._resultTitleInfo.ChipIndexIndex >= 0)
                {
                    colList.Add(this._resultTitleInfo.ChipIndexIndex);
                }
                _crKeyMaker = new PosKeyMakerBase(this._resultTitleInfo.ColIndex, this._resultTitleInfo.RowIndex, colList);
            }
            #endregion

            #region
            string headerStr = "";
            int maxCnt = 4;
            //for (int i = 0; i < 4; ++i)
            int cnt = 0;
            foreach(var mItem in ResultTitleInfo)
            {
                headerStr += mItem.Value + ",";
                if (cnt >= maxCnt)
                {
                    break;
                }
                cnt++;
            }
            #endregion

            #region

            ResultTitleInfo d4rti = new ResultTitleInfo();

            Dictionary<string, string> d4keyNameDic = GetD4KeyHeaderDic(this.UISetting.UserDefinedData.ResultItemNameDic);

            d4rti.SetResultData(d4keyNameDic);
            #endregion

            HeaderFinder_ByStartStr hf = new HeaderFinder_ByStartStr(headerStr, TitleStrShift);

            //ReportMerger merger = new ReportMerger(this.UISetting, hf, ResultTitleInfo, _crKeyMaker,d4rti);

            ReportMerger merger = new ReportMerger(this.UISetting, hf, d4rti, _crKeyMaker );

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

        private Dictionary<string, string> GetD4KeyHeaderDic(Dictionary<string, string> keyNameDic)
        {
            Dictionary<string, string> kvDic = new Dictionary<string, string>();

            if (keyNameDic != null)
            {
                foreach (var p in keyNameDic)
                {
                    string name = p.Value;
                    string key = p.Key;
                    var item = this.UISetting.UserDefinedData[key];
                    if (item != null)
                    {
                        string name1 = item.Name;
                        string unit = item.Unit;
                        name = name1;
                        if (unit != "")
                        { name += "(" + unit + ")"; }
                    }
                    kvDic.Add(key, name);
                }
            }
            return kvDic;
        }

    }
}
