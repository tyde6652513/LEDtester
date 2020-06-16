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
using System.IO;

namespace MPI.Tester.Report.User.UOC
{
	public class Report : ReportBase
	{
        // UOC 8123
        private const string REPORT_INFO_SAVE_PATH = @"C:\MPI\LEDTester\Data\ReportInfo.dat";
        private UOCReportInfo _reportInfo;

        private string _reFileNameWithExt;

        public Report(List<object> objs, bool isReStatistic)
			: base(objs, isReStatistic)
		{
            this._isImplementPIVDataReport = true;
        }

        #region >>> Private Method <<<


        #endregion

        #region >>> Protected Override Method <<<

        protected override void SetResultTitle()
		{
            this.ResultTitleInfo.SetResultData(this.UISetting.UserDefinedData.ResultItemNameDic);
		} 

		protected override EErrorCode WriteReportHeadByUser()
		{
            this._reFileNameWithExt = string.Empty;
            
            ////////////////////////////////////////////
			//Write Report Head
			////////////////////////////////////////////
            this.WriteLine("Output File Name," );

            this.WriteLine("RecipeName," + this.UISetting.TaskSheetFileName);

            this.WriteLine("ToolID," + this.UISetting.MachineName);

            this.WriteLine("OperatorID," + this.UISetting.OperatorName);

            this.WriteLine("StartTime," + this.TesterSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm"));

            this.WriteLine("EndTime," + this.TesterSetting.EndTestTime.ToString("yyyy/MM/dd HH:mm"));

            this.WriteLine("WaferID," + this.UISetting.WaferNumber);

            this.WriteLine("ChipNo," + this.UISetting.ProductType);

            this.WriteLine("MO," + this.UISetting.LotNumber);

            this.WriteLine("Comment,");

            this.WriteLine("Yield,");
            this.WriteLine("Good Die,");
            this.WriteLine("Total Die,");
            this.WriteLine("");
            this.WriteLine("");
            ////////////////////////////////////////////
            // Write Bin Info
            ////////////////////////////////////////////
            string bin = string.Empty;

            if (this.SmartBinning != null)
            {
                if (this.SmartBinning.SmartBin.Count > 0)
                {
                     bin += ",";
                }
            }

            if (bin == string.Empty)
            {
                bin = "0,bin0,bin1,bin2,bin3,bin4,bin5,bin6,bin7,bin8,bin9,bin10";
            }

            this.WriteLine(bin);

            this.WriteLine("Count");
            this.WriteLine("Percent");

            this.WriteLine("");
            this.WriteLine("");
            this.WriteLine("");

            this.WriteLine(this.ResultTitleInfo.TitleStr);

            return EErrorCode.NONE;
		}

		protected override EErrorCode RewriteReportByUser()
		{
            Dictionary<string, string> replaceData = new Dictionary<string, string>();

            string fileName = this.TestResultFileName + ".csv";

            if (!this.UISetting.IsManualRunMode)
            {
                // Auto Run
                this._reportInfo = UOCReportInfo.Deserialize<UOCReportInfo>(REPORT_INFO_SAVE_PATH);

                if (this._reportInfo == null)
                {
                    this._reportInfo = new UOCReportInfo();
                }

                this._reportInfo.CheckToday();

                string mo = this.UISetting.LotNumber;
                string waferID = this.UISetting.WaferNumber;
                string chipNo = this.UISetting.ProductType;
                string testCnt = this._reportInfo.SaveCount.ToString();
                string date = DateTime.Now.ToString("yMMdd");   // 160517

                date = date.Remove(0, 1);  // 60517  remove 1

                fileName = string.Format("{0}_{1}_{2}_{3}_{4}.csv", mo, waferID, chipNo, testCnt, date);

                this._reportInfo.SaveDate = DateTime.Now;

                this._reportInfo.SaveCount++;

                UOCReportInfo.Serialize(REPORT_INFO_SAVE_PATH, this._reportInfo);

                this._reFileNameWithExt = fileName;
            }

            replaceData.Add("Output File Name,", "Output File Name," + fileName);

            //-------------------------------------------------------------------------------------------------
            string comment = string.Empty;

            comment = "Comment," + this.UISetting.ReportComments;

            replaceData.Add("Comment,", comment);

            //-------------------------------------------------------------------------------------------------
            string endTime = "EndTime," + this.TesterSetting.EndTestTime.ToString("yyyy/MM/dd HH:mm");

            replaceData.Add("EndTime,", endTime);

            //-------------------------------------------------------------------------------------------------  
            string totalDieCnt = "Total Die," + this.AcquireData.ChipInfo.TestCount;

            replaceData.Add("Total Die,", totalDieCnt);

            //-------------------------------------------------------------------------------------------------
            string goodDieCnt = "Good Die," + this.AcquireData.ChipInfo.GoodDieCount;

            replaceData.Add("Good Die,", goodDieCnt);

            //-------------------------------------------------------------------------------------------------
            string goodRate = "Yield," + this.AcquireData.ChipInfo.GoodRate.ToString("0.00");

            replaceData.Add("Yield,", goodRate);

            //-------------------------------------------------------------------------------------------------

            this.ReplaceReport(replaceData);

            return EErrorCode.NONE;
		}

        protected override EErrorCode MoveFileToTargetByUser(EServerQueryCmd cmd)
        {
            bool isOutputPath02 = false;

            bool isOutputPath03 = false;

            string outPath01 = string.Empty;

            string outPath02 = string.Empty;

            string outPath03 = string.Empty;

            string fileNameWithExt = this.TestResultFileName + ".csv";

            if (this.UISetting.IsManualRunMode)
            {
                // Manual Run
                isOutputPath02 = this.UISetting.IsEnableManualPath02;

                isOutputPath03 = this.UISetting.IsEnableManualPath03;

                outPath01 = this.UISetting.ManualOutputPath01;

                outPath02 = this.UISetting.ManualOutputPath02;

                outPath03 = this.UISetting.ManualOutputPath03;
            }
            else
            {
                // Auto Run
                if (this._reFileNameWithExt != string.Empty)
                {
                    fileNameWithExt = this._reFileNameWithExt;
                }
                
                isOutputPath02 = this.UISetting.IsEnablePath02;

                isOutputPath03 = this.UISetting.IsEnablePath03;

                outPath01 = this.UISetting.TestResultPath01;

                outPath02 = this.UISetting.TestResultPath02;

                outPath03 = this.UISetting.TestResultPath03;
            }

            //---------------------------------------------------------------------------------
            // Copy Report file to taget path
            //---------------------------------------------------------------------------------
            string outputPathAndFile01 = Path.Combine(outPath01, fileNameWithExt);

            string outputPathAndFile02 = Path.Combine(outPath02, fileNameWithExt);

            string outputPathAndFile03 = Path.Combine(outPath03, fileNameWithExt);

            MPIFile.CopyFile(this.FileFullNameRep, outputPathAndFile01);

            if (isOutputPath02)
            {
                MPIFile.CopyFile(this.FileFullNameRep, outputPathAndFile02);
            }

            if (isOutputPath03)
            {
                MPIFile.CopyFile(this.FileFullNameRep, outputPathAndFile03);
            }

            return EErrorCode.NONE;
        }

        // PIV
        protected override EErrorCode WriteReportHeadByUser2()
        {
            if (!this.UISetting.IsEnableSaveLIVData)
            {
                return EErrorCode.NONE;
            }

            return EErrorCode.NONE;
        }

        protected override EErrorCode PushDataByUser2(Dictionary<string, double> data)
        {
            if (!this.UISetting.IsEnableSaveLIVData)
            {
                return EErrorCode.NONE;
            }

            string col = data["COL"].ToString();

            string row = data["ROW"].ToString();

            string key = string.Format("C{0}R{1}", col, row);

            foreach (var piv in this.AcquireData.PIVDataSet)
            {
                if (!piv.IsEnable)
                {
                    continue;
                }

                string IntLine = string.Empty;

                //-------------------------------------------
                // Curr
                IntLine = key + "," + piv.Name + ",Apply(A)";

                for (int i = 0; i < piv.CurrentData.Length; i++)
                {
                    IntLine += "," + piv.CurrentData[i].ToString();
                }

                this.WriteLine2(IntLine);

                //-------------------------------------------
                // Volt
                IntLine = key + "," + piv.Name + ",Msrt(V)"; ;

                for (int i = 0; i < piv.VoltageData.Length; i++)
                {
                    IntLine += "," + piv.VoltageData[i].ToString();
                }

                this.WriteLine2(IntLine);

                //-------------------------------------------
                // Pow
                IntLine = key + "," + piv.Name + ",Pow(mW)";

                for (int i = 0; i < piv.PowerData.Length; i++)
                {
                    IntLine += "," + piv.PowerData[i].ToString();
                }

                this.WriteLine2(IntLine);
            }

            return EErrorCode.NONE;
        }

        protected override EErrorCode RewriteReportByUser2()
        {
            if (!this.UISetting.IsEnableSaveLIVData)
            {
                return EErrorCode.NONE;
            }

            if (!File.Exists(this.FileFullNameTmp2))
            {
                return EErrorCode.NONE;
            }

            Dictionary<string, string> replaceData = new Dictionary<string, string>();

            using (StreamReader sr = new StreamReader(this.FileFullNameTmp2, Encoding.Default))
            {
                using (StreamWriter sw = new StreamWriter(this.FileFullNameRep2))
                {
                    while (sr.Peek() >= 0)
                    {
                        string line = sr.ReadLine();

                        if (replaceData.ContainsKey(line))
                        {
                            line = replaceData[line];
                        }

                        sw.WriteLine(line);
                    }
                }
            }

            return EErrorCode.NONE;
        }

        protected override EErrorCode MoveFileToTargetByUser2(EServerQueryCmd cmd)
        {
            if (!this.UISetting.IsEnableSaveLIVData)
            {
                return EErrorCode.NONE;
            }

            string outPath = this.UISetting.LIVDataSavePath;

            string fileNameWithoutExt = this.TestResultFileNameWithoutExt();

            //Abort
            if (cmd == EServerQueryCmd.CMD_TESTER_ABORT)
            {
                fileNameWithoutExt = fileNameWithoutExt + "_" + DateTime.Now.ToString("yyMMddhhmmss");
            }

            string fileNameWithExt = fileNameWithoutExt + "_PIV.csv";

            string outputPathAndFile = Path.Combine(outPath, fileNameWithExt);

            MPIFile.CopyFile(this.FileFullNameRep2, outputPathAndFile);

            return EErrorCode.NONE;
        }

		#endregion
	}

    [Serializable]
    internal class UOCReportInfo
    {
        private object _lockObj;
        private DateTime _saveDate;
        private uint _reportSaveCount;

        public UOCReportInfo()
        {
            this._lockObj = new object();
            this._saveDate = DateTime.Now;
            this._reportSaveCount = 1;
        }


        #region >>> Public Proberty <<<

        public DateTime SaveDate
        {
            get { return this._saveDate; }
            set { lock (this._lockObj) { this._saveDate = value; } }
        }

        public uint SaveCount
        {
            get { return this._reportSaveCount;  }
            set { lock (this._lockObj) { this._reportSaveCount = value; } }
        }

        #endregion

        #region >>> Public Method <<<

        public static bool Serialize(string FileName, object Obj)
        {
            try
            {
                if (System.IO.Path.GetExtension(FileName).ToLower() == ".xml")
                {
                    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Obj.GetType());
                    System.Xml.XmlTextWriter xmlTextWriter = new System.Xml.XmlTextWriter(FileName, Encoding.ASCII);
                    x.Serialize(xmlTextWriter, Obj);
                    xmlTextWriter.Close();
                }
                else if (System.IO.Path.GetExtension(FileName).ToLower() == ".dat")
                {
                    using (System.IO.FileStream fileStream = new System.IO.FileStream(FileName, System.IO.FileMode.Create))
                    {
                        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        binaryFormatter.Serialize(fileStream, Obj);

                        fileStream.Close();
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static T Deserialize<T>(string FileName)
        {
            System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();

            try
            {
                object obj = new object();

                if (System.IO.Path.GetExtension(FileName).ToLower() == ".xml")
                {
                    xdoc.Load(FileName);
                    System.Xml.XmlNodeReader reader = new System.Xml.XmlNodeReader(xdoc.DocumentElement);
                    System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    obj = ser.Deserialize(reader);
                }
                else if (System.IO.Path.GetExtension(FileName).ToLower() == ".dat")
                {
                    using (System.IO.FileStream fileStream = new System.IO.FileStream(FileName, System.IO.FileMode.Open))
                    {
                        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        obj = binaryFormatter.Deserialize(fileStream);

                        fileStream.Close();
                    }
                }

                return (T)obj;
            }
            catch
            {
                return default(T);
            }
        }

        public void CheckToday()
        {
            DateTime now = DateTime.Now;

            int today = DateTime.Now.Day;

            if (this._saveDate.Day != today)
            {
                this._reportSaveCount = 1;
            }
        }

        #endregion
    }
}
