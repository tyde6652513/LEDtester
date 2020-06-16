using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using MPI.Tester.Data;
using MPI.Tester.TestServer;
using MPI.Tester.Maths;
using MPI.Tester;
using MPI.Tester.TestKernel;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Report.User.Lextar_miniLED
{
    public class Report : ReportBase
    {
        #region >>> Constructor / Disposor <<<

        List<string> keyNameList = new List<string>();

        public Report(List<object> objs, bool isReStatistic)
            : base(objs, isReStatistic)
        {
        }

        #endregion

        protected override EErrorCode WriteReportHeadByUser()
        {			////////////////////////////////////////////
            //Write Report Head
            ////////////////////////////////////////////
            this.WriteLine("FileName," + this.TestResultFileNameWithoutExt() + "." + this.UISetting.TestResultFileExt);

            this.WriteLine("TestTime," + this.TesterSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm"));

            this.WriteLine("LotNumber," + this.UISetting.LotNumber);

            this.WriteLine("WaferNumber," + this.UISetting.WaferNumber);

            this.WriteLine("Recipe," + this.UISetting.TaskSheetFileName);

            this.WriteLine("Filter Wheel," + this.Product.ProductFilterWheelPos.ToString());

            this.WriteLine("Operator," + this.UISetting.OperatorName);

            this.WriteLine("Samples,");

            this.WriteLine("");

            ////////////////////////////////////////////
            //Write Result Item Title
            ////////////////////////////////////////////
            this.WriteLine(this.ResultTitleInfo.TitleStr);

            return EErrorCode.NONE;
        }

        protected override void SetResultTitle()
        {
            this.ResultTitleInfo.Clear();

            this.ResultTitleInfo.AddResultData("TEST", "TEST");

            this.ResultTitleInfo.AddResultData("COL", "PosX");

            this.ResultTitleInfo.AddResultData("ROW", "PosY");

            this.ResultTitleInfo.AddResultData("CHANNEL", "CH");

            this.ResultTitleInfo.AddResultData("BIN", "BIN");

            this.ResultTitleInfo.AddResultData("POLAR", "POLAR");

            this.ResultTitleInfo.AppendResultData(this.ResultData);
        }

        protected override EErrorCode RewriteReportByUser()
        {
            Dictionary<string, string> replaceData = new Dictionary<string, string>();

            string testCount = "Samples," + this.TotalTestCount.ToString();

            replaceData.Add("Samples,", testCount);

            this.ReplaceReport(replaceData);

            return EErrorCode.NONE;
        }
    }
}
