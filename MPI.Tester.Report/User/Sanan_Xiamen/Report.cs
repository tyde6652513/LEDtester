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

namespace MPI.Tester.Report.User.Sanan_Xiamen
{
	public class Report : ReportBase
	{
        private List<double> _lstWL = new List<double>();

        public Report(List<object> objs, bool isReStatistic): base(objs, isReStatistic)
		{
            this._isImplementPIVDataReport = true;
            this._isImplementSpectrumReport = true;
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
            ////////////////////////////////////////////
			//Write Report Head
			////////////////////////////////////////////

            if (this.UISetting.FormatName == "Format-EEL")
            {
                this.WriteLine("FileName," + this.TestResultFileNameWithoutExt() + "." + this.UISetting.TestResultFileExt.ToUpper());

                this.WriteLine("TestTime," + this.TesterSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm"));

                this.WriteLine("TesterNumber," + this.UISetting.MachineName);

                this.WriteLine("Specification," + this.UISetting.TaskSheetFileName + ".src");

                this.WriteLine("Temperature,");

                this.WriteLine("Operator," + this.UISetting.OperatorName);

                this.WriteLine("");
                this.WriteLine("");
            }
            else
            {
                this.WriteLine("FileName," + this.TestResultFileNameWithoutExt() + "." + this.UISetting.TestResultFileExt.ToUpper());

                this.WriteLine("TestTime," + this.TesterSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm"));

                this.WriteLine("TesterNumber," + this.UISetting.MachineName);

                this.WriteLine("Specification," + this.UISetting.TaskSheetFileName + ".src");

                this.WriteLine("Operator," + this.UISetting.OperatorName);

                this.WriteLine("");
                this.WriteLine("");
            }


            this.WriteLine(this.ResultTitleInfo.TitleStr);

            return EErrorCode.NONE;
		}

		protected override EErrorCode RewriteReportByUser()
		{
            Dictionary<string, string> replaceData = new Dictionary<string, string>();

            string temperature = "Temperature," + GlobalData.ProberTemperature.ToString("0.00");

            replaceData.Add("Temperature,", temperature);

            this.ReplaceReport(replaceData);

            return EErrorCode.NONE;
		}

        // RelativeSpectrum
        protected override EErrorCode WriteReportHeadByUser2()
        {
            if (!this.UISetting.IsEnableSaveRelativeSpectrum)
            {
                return EErrorCode.NONE;
            }

            this.WriteLine2("Chip,Name,Wavelength");

            return EErrorCode.NONE;
        }

        protected override EErrorCode PushDataByUser2(Dictionary<string, double> data)
        {
            if (!this.UISetting.IsEnableSaveRelativeSpectrum)
            {
                return EErrorCode.NONE;
            }

            string col = data["COL"].ToString();

            string row = data["ROW"].ToString();

            string key = string.Format("C{0}R{1}", col, row);

            foreach (var spectrum in this.AcquireData.SpectrumDataSet)
            {
                if (!spectrum.IsEnable)
                {
                    continue;
                }

                uint startWl = this.TesterSetting.OptiDevSetting.StartWavelength;
                uint endWl = this.TesterSetting.OptiDevSetting.EndWavelength;

                this._lstWL.Clear();

                string nameStr = "," + spectrum.Name;

                string IntLine = key + nameStr + ",Intensity";

                for (int i = 0; i < spectrum.Wavelength.Length; i++)              
                {
                    double wl = spectrum.Wavelength[i];

                    if (wl >= startWl && wl <= endWl)
                    {
                        this._lstWL.Add(wl);

                        IntLine += "," + spectrum.Intensity[i].ToString();
                    }
                }

                this.WriteLine2(IntLine);
            }

             return EErrorCode.NONE;
        }

        protected override EErrorCode RewriteReportByUser2()
        {
            if (!this.UISetting.IsEnableSaveRelativeSpectrum)
            {
                return EErrorCode.NONE;
            }

            if (!File.Exists(FileFullNameTmp2))
            {
                return EErrorCode.NONE; 
            }
            
            string title = "Chip,Name,Wavelength";

            if (this._lstWL != null || this._lstWL.Count >0)
            {
                foreach (var item in this._lstWL)
                {
                    title += "," + item.ToString();
                }
            }

            Dictionary<string, string> replaceData = new Dictionary<string, string>();

            replaceData.Add("Chip,Name,Wavelength", title);

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
            string fileNameWithoutExt = this.TestResultFileNameWithoutExt();

            //Abort
            if (cmd == EServerQueryCmd.CMD_TESTER_ABORT)
            {
                fileNameWithoutExt = fileNameWithoutExt + "_" + DateTime.Now.ToString("yyMMddhhmmss");
            }

            string fileNameWithExt = fileNameWithoutExt + ".rel";

            if (this.UISetting.IsEnableSaveRelativeSpectrum)
            {
                string outPath = this.UISetting.RelativeSpectrumPath;

                string outputPathAndFile = Path.Combine(outPath, fileNameWithExt);

                MPIFile.CopyFile(this.FileFullNameRep2, outputPathAndFile);
            }

            if (this.UISetting.IsEnableSaveRelativeSpectrum02)
            {
                string outPath02 = this.UISetting.RelativeSpectrumPath02;

                string outputPathAndFile02 = Path.Combine(outPath02, fileNameWithExt);

                MPIFile.CopyFile(this.FileFullNameRep2, outputPathAndFile02);
            }

            if (this.UISetting.IsEnableSaveRelativeSpectrum03)
            {
                string outPath03 = this.UISetting.RelativeSpectrumPath03;

                string outputPathAndFile03 = Path.Combine(outPath03, fileNameWithExt);

                MPIFile.CopyFile(this.FileFullNameRep2, outputPathAndFile03);
            }
          
            return EErrorCode.NONE;
        }

        // PIV
        protected override EErrorCode WriteReportHeadByUser3()
        {
            if (!this.UISetting.IsEnableSaveLIVData)
            {
                return EErrorCode.NONE;
            }

            return EErrorCode.NONE;
        }

        protected override EErrorCode PushDataByUser3(Dictionary<string, double> data)
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

                this.WriteLine3(IntLine);

                //-------------------------------------------
                // Volt
                IntLine = key + "," + piv.Name + ",Msrt(V)";;

                for (int i = 0; i < piv.VoltageData.Length; i++)
                {
                    IntLine += "," + piv.VoltageData[i].ToString();
                }

                this.WriteLine3(IntLine);

                //-------------------------------------------
                // Pow
                IntLine = key + "," + piv.Name + ",Pow(mW)";

                for (int i = 0; i < piv.PowerData.Length; i++)
                {
                    IntLine += "," + piv.PowerData[i].ToString();
                }

                this.WriteLine3(IntLine);
            }

            return EErrorCode.NONE;
        }

        protected override EErrorCode RewriteReportByUser3()
        {
            if (!this.UISetting.IsEnableSaveLIVData)
            {
                return EErrorCode.NONE;
            }

            if (!File.Exists(this.FileFullNameTmp3))
            {
                return EErrorCode.NONE;
            }

            Dictionary<string, string> replaceData = new Dictionary<string, string>();

            using (StreamReader sr = new StreamReader(this.FileFullNameTmp3, Encoding.Default))
            {
                using (StreamWriter sw = new StreamWriter(this.FileFullNameRep3))
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

        protected override EErrorCode MoveFileToTargetByUser3(EServerQueryCmd cmd)
        {
            
            string fileNameWithoutExt = this.TestResultFileNameWithoutExt();

            //Abort
            if (cmd == EServerQueryCmd.CMD_TESTER_ABORT)
            {
                fileNameWithoutExt = fileNameWithoutExt + "_" + DateTime.Now.ToString("yyMMddhhmmss");
            }

            string fileNameWithExt = fileNameWithoutExt + "_PIV.csv";


            if (this.UISetting.IsEnableSaveLIVData)
            {
                string outPath = this.UISetting.LIVDataSavePath;

                string outputPathAndFile = Path.Combine(outPath, fileNameWithExt);
                
                MPIFile.CopyFile(this.FileFullNameRep3, outputPathAndFile);
            }

            if (this.UISetting.IsEnableSaveLIVDataPath02)
            {
                string outPath02 = this.UISetting.LIVDataSavePath02;

                string outputPathAndFile02 = Path.Combine(outPath02, fileNameWithExt);

                MPIFile.CopyFile(this.FileFullNameRep3, outputPathAndFile02);
            }

            if (this.UISetting.IsEnableSaveLIVDataPath03)
            {
                string outPath03 = this.UISetting.LIVDataSavePath03;

                string outputPathAndFile03 = Path.Combine(outPath03, fileNameWithExt);

                MPIFile.CopyFile(this.FileFullNameRep3, outputPathAndFile03);
            }

            return EErrorCode.NONE;
        }

		#endregion
	}
}
