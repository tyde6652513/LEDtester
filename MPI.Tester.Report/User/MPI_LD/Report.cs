using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Reflection;
using System.Diagnostics;
using MPI.Tester.Data;
using MPI.Tester.TestServer;
using MPI.Tester.Maths;
using MPI.Tester;
using MPI.Tester.TestKernel;
using MPI.Tester.Data;

namespace MPI.Tester.Report.User.MPI_LD
{
	public class Report : ReportBase
	{
        private const string DEMO_REPORT_PATH = @"D:\LOGs";
        private List<double> _lstWL = new List<double>();
        private const char SPILT_CHAR = ',';

        public Report(List<object> objs, bool isReStatistic): base(objs, isReStatistic)
		{
            this._isImplementPIVDataReport = true;
            this._isImplementSpectrumReport = true;
        }

        #region >>> Full Demo Report Keys <<<

        private List<string> _lstFullDemoReportKeys = new List<string>() 
        {
        // System Parameters
        "TEST","COL","ROL","BIN",

        // Normal TestItem
        "MVF_1","MVF_2","MVZ_1","MVZ_2","MVZ_3","MIR_1","MIR_2",
            
        // PIV TestItem
        "Iop_1","Vop_1","Ith_1","Pth_1","Vth_1","SE_1","SE2_1","RS_1",
        "PfA_1","VfA_1","RdA_1","PceA_1","PfB_1","VfB_1","RdB_1","PceB_1","PfC_1","VfC_1","RdC_1","PceC_1",
        "Linearity_1","Rollover_1","Kink_1","Ikink_1","Pkink_1","Ipk_1","Ppk_1","Vpk_1","Pcepk_1",
        
        // WL TestItem
        "MVFLA_1","WLD_1","WLP_1","HW_1","ST_1","INT_1",
        
        // LOP TestItem
        "PDMVF_1","PDWATT_1","PDCURRENT_1","PDMVF_2","PDWATT_2","PDCURRENT_2","PDMVF_3","PDWATT_3","PDCURRENT_3",
        
        // FFT TestItem
        "FfpVolt_1","FfpAngleAvg_1","FfpAngleXMajor_1","FfpAngleYMinor_1","FfpAngleXMajor2_1","FfpAngleYMinor2_1",
        "FfpDIP_1","FfpCentroidX_1","FfpCentroidY_1","FfpD4SigmaMajor_1","FfpD4SigmaMinor_1",
        
        // NFT TestItem        
        "NfpVolt_1","DieCount_1","DeadDieCount_1","BadDieCount_1",
        "IntensityMax_1","IntensityMin_1","IntensityMedian_1",
        "RatioIntMinDivMedian_1","RatioIntMaxDivMedian_1","RatioIntIQRDivMedian_1","RatioIntDeltaRangeDivMedian_1",
        "DiameterMajorMedian_1","DiameterMajorMax_1","DiameterMajorMin_1","DiameterMajorIQR_1","RatioDiameterMajorIqrDivMedian_1",
        "DiameterMinorMedian_1","DiameterMinorMax_1","DiameterMinorMin_1","DiameterMinorIQR_1","RatioDiameterMinorIqrDivMedian_1",
        "PeakCountMedian_1","PeakCountMax_1","PeakCountMin_1","DPeakCountIQR_1","RatioPeakCountIqrDivMedian_1",
        };

        private Dictionary<string, string> _dicDemoTestResultItem = new Dictionary<string, string>();

        #endregion

        #region >>> Private Method <<<


        #endregion

        #region >>> Protected Override Method <<<

        protected override void SetResultTitle()
		{
            this.ResultTitleInfo.SetResultData(this.UISetting.UserDefinedData.ResultItemNameDic);
        } 

		protected override EErrorCode WriteReportHeadByUser()
		{
            ////////////////////////////////////////////
			//Write Report Head
			////////////////////////////////////////////
            this.WriteLine("FileName," + this.TestResultFileNameWithoutExt() + "." + this.UISetting.TestResultFileExt.ToUpper());

            this.WriteLine("TestTime," + this.TesterSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm"));

            this.WriteLine("TesterNumber," + this.UISetting.MachineName);

            this.WriteLine("Specification," + this.UISetting.TaskSheetFileName + ".src");

            this.WriteLine("Operator," + this.UISetting.OperatorName);

            this.WriteLine("");
            this.WriteLine("");

            this.WriteLine(this.ResultTitleInfo.TitleStr);

            return EErrorCode.NONE;
		}

		protected override EErrorCode RewriteReportByUser()
		{
            return base.RewriteReportByDefault();
		}

        // Full Demo Report
        protected override EErrorCode WriteReportHeadByUser2()
        {
            this._dicDemoTestResultItem.Clear();

            this._dicDemoTestResultItem.Add("TEST", "TEST");
            this._dicDemoTestResultItem.Add("COL", "PosX");
            this._dicDemoTestResultItem.Add("ROW", "PosY");
            this._dicDemoTestResultItem.Add("BIN", "BIN");

            if (this.Product.TestCondition.TestItemArray != null)
                {
                foreach (var testItem in this.Product.TestCondition.TestItemArray)
                    {
                    if (testItem.IsEnable)
        {
                        foreach (var msrtItem in testItem.MsrtResult)
            {
                            this._dicDemoTestResultItem.Add(msrtItem.KeyName, msrtItem.Name);
            }
            }
                }
            }

            string outStr = string.Empty;

            foreach (var item in this._dicDemoTestResultItem)
                        {
                outStr += item.Value + SPILT_CHAR.ToString();            
                        }

            outStr = outStr.Remove(outStr.Length - 1);

            this.WriteLine2(outStr);

                return EErrorCode.NONE;
            }

        protected override EErrorCode PushDataByUser2(Dictionary<string, double> data)
            {
            string line = string.Empty;

            foreach (var item in this._dicDemoTestResultItem)
        {
                if (data.ContainsKey(item.Key))
            {
                    string format = string.Empty;

                    if (this.UISetting.UserDefinedData[item.Key] != null)
            {
                        format = this.UISetting.UserDefinedData[item.Key].Formate;
            }

                    line += data[item.Key].ToString(format) + ",";
                }
                else
                {
                    line += ",";                
                }
                }

            line = line.Remove(line.Length - 1);

            this.WriteLine2(line);

            return EErrorCode.NONE;
        }

        protected override EErrorCode RewriteReportByUser2()
            {
            if (!File.Exists(FileFullNameTmp2))
            {
                return EErrorCode.NONE;
            }

            MPIFile.CopyFile(this.FileFullNameTmp2, this.FileFullNameRep2);

            return EErrorCode.NONE;
        }

        protected override EErrorCode MoveFileToTargetByUser2(EServerQueryCmd cmd)
            {
            string outPath = @"C:\MPI\Temp2\DemoBackup";

            string fileNameWithoutExt = this.TestResultFileNameWithoutExt();

            //Abort
            if (cmd == EServerQueryCmd.CMD_TESTER_ABORT)
            {
                fileNameWithoutExt = fileNameWithoutExt + "_" + DateTime.Now.ToString("yyMMddhhmmss");
            }

            string fileNameWithExt = fileNameWithoutExt + ".csv";

            string outputPathAndFile = Path.Combine(outPath, fileNameWithExt);

            MPIFile.CopyFile(this.FileFullNameRep2, outputPathAndFile);

            //string outputPathAndFile_demo = Path.Combine(DEMO_REPORT_PATH, fileNameWithExt);

            //MPIFile.CopyFile(this.FileFullNameRep2, outputPathAndFile_demo);

            return EErrorCode.NONE;
        }

		#endregion
	}
}
