using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;
using MPI.Tester.TestServer;
using System.IO;
using MPI.Tester.Data.CalibrateData;
using MPI.Tester;

namespace MPI.Tester.Report.User.ITRI_OLED
{
	class Report : ReportBase
	{
		#region >>> Constructor / Disposor <<<

        private double[] _luminanceData;

        //private SearchTargetLuminace _searchTargetLuminacne;

		public Report(List<object> objs, bool isReStatistic)
			: base(objs, isReStatistic)
		{
		}

		#endregion

		#region >>> Protected Override Method <<<

		protected override EErrorCode WriteReportHeadByUser()
		{
			////////////////////////////////////////////
			//Write Report Head
			////////////////////////////////////////////
			this.WriteLine("StartTime," + this.TesterSetting.StartTestTime.ToString("yyyyMMddHHmm"));

            this.WriteLine("EndTime,");

			this.WriteLine("FileName," + this.UISetting.TestResultFileName);

			this.WriteLine("Equipment," + this.UISetting.MachineName);

			this.WriteLine("操作者," + this.UISetting.OperatorName);

            this.WriteLine("OLED Panel Test Result");

            this.WriteLine("Current,Voltage,Luminace,Uniformity,Homogeneity");

         //   this.WriteLine(this.UISetting.LuminanceTestStringData);

            this.WriteLine(Environment.NewLine);
 
            this.ResultTitleInfo.SetResultData(this.UISetting.UserDefinedData.ResultItemNameDic);

			this.WriteLine(this.ResultTitleInfo.TitleStr);

			return EErrorCode.NONE;
		}

        protected override EErrorCode RunCommandByUser(EServerQueryCmd cmd, List<object> objs)
        {
            switch (cmd)
            {
                case EServerQueryCmd.CMD_TESTER_START:
                    {
                        return this.WriteReportHeadByUser(); // *.Temp
                    }
                case EServerQueryCmd.CMD_TESTER_END:
                case EServerQueryCmd.CMD_TESTER_ABORT:
                    {
                        EErrorCode code = this.RewriteReportByUser(objs); // *.CSV

                        if (code != EErrorCode.NONE)
                        {
                            return code;
                        }

                        return this.MoveFileToTargetByUser(cmd);
                    }
                default:
                    {
                        return EErrorCode.NONE;
                    }
            }
        }

        private  EErrorCode RewriteReportByUser(List<object> objs)
        {
            Dictionary<string, string> replaceData = new Dictionary<string, string>();

            List<double[]> spectrum = new List<double[]>();

            if (objs != null)
            {
                foreach (var item in objs)
                {
                    //if (item is SearchTargetLuminace)
                    //{
                    //    this._searchTargetLuminacne = item as SearchTargetLuminace;

                    //    this._luminanceData = (item as SearchTargetLuminace).ResultData;
                    //}

                    if (item is List<double[]>)
                    {
                        spectrum = (item as List<double[]>);
                    }
                }
            }

             string data=string.Empty;

             //if (this._luminanceData != null)
             //{
             //   data = this._luminanceData[(int)ELuminanceType.FCurrentA].ToString("0.000") + "," +
             //                        this._luminanceData[(int)ELuminanceType.VoltageA].ToString("0.000") + "," +
             //                        this._luminanceData[(int)ELuminanceType.Luminance].ToString("0.000") + "," +
             //                        this._luminanceData[(int)ELuminanceType.Uniformity].ToString("0.000") + "," +
             //                        this._luminanceData[(int)ELuminanceType.Homogeneity].ToString("0.000");
             //}
             //else
             //{

             //}

            this.WriteLine(data);

            string spectrumstr = string.Empty;

            replaceData.Add("LuminanceDataSet,", data);

            replaceData.Add("SpectrumSet,", spectrumstr);

            this.TesterSetting.EndTestTime = DateTime.Now;

            string endTime = "EndTime," + this.TesterSetting.EndTestTime.ToString("yyyyMMddHHmm");

            replaceData.Add("EndTime,", endTime);

            this.ReplaceReport(replaceData, spectrum, false);

            return EErrorCode.NONE;
        }

		#endregion
	}
}
