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

namespace MPI.Tester.Report.User.InfiniLED
{
	class Report : ReportBase
	{
        private List<double> _lstWL = new List<double>();

		public Report(List<object> objs, bool isReStatistic) : base(objs, isReStatistic)
        {
            this._isImplementLIVReport = true;
            this._isImplementSpectrumReport = false;
		}

		#region >>> Protected Override Method <<<

        protected override void SetResultTitle()
        {
            ////////////////////////////////////////////
            //Write Result Item Title
            ////////////////////////////////////////////

            this.ResultTitleInfo.Clear();

            this.ResultTitleInfo.AddResultData("TEST", "TEST");

            this.ResultTitleInfo.AddResultData("COL", "COL");

            this.ResultTitleInfo.AddResultData("ROW", "ROW");

            this.ResultTitleInfo.AddResultData("BIN", "BIN");

            this.ResultTitleInfo.AddResultData("CHIP_INDEX", "SUBDIE INDEX");

            //-----------------------------------------------------------------------
            this.ResultTitleInfo.AddResultData("PDMIF_1", "PDIFLA1");

            this.ResultTitleInfo.AddResultData("PDMVF_1", "PDVFLA1");

            this.ResultTitleInfo.AddResultData("PDCURRENT_1", "PDi1");

            this.ResultTitleInfo.AddResultData("PDWATT_1", "Pf");

            //-----------------------------------------------------------------------
            this.ResultTitleInfo.AddResultData("PDMIF_2", "PDIFLA2");

            this.ResultTitleInfo.AddResultData("PDMVF_2", "PDVFLA2");

            this.ResultTitleInfo.AddResultData("PDCURRENT_2", "PDi2");

            this.ResultTitleInfo.AddResultData("PDWATT_2", "Pf2");

            //-----------------------------------------------------------------------
            this.ResultTitleInfo.AddResultData("PDMIF_3", "PDIFLA3");

            this.ResultTitleInfo.AddResultData("PDMVF_3", "PDVFLA3");

            this.ResultTitleInfo.AddResultData("PDCURRENT_3", "PDi3");

            this.ResultTitleInfo.AddResultData("PDWATT_3", "Pf3");

            //-----------------------------------------------------------------------
            this.ResultTitleInfo.AddResultData("PDMIF_4", "PDIFLA4");

            this.ResultTitleInfo.AddResultData("PDMVF_4", "PDVFLA4");

            this.ResultTitleInfo.AddResultData("PDCURRENT_4", "PDi4");

            this.ResultTitleInfo.AddResultData("PDWATT_4", "Pf4");

            //-----------------------------------------------------------------------
            this.ResultTitleInfo.AddResultData("PDMIF_5", "PDIFLA5");

            this.ResultTitleInfo.AddResultData("PDMVF_5", "PDVFLA5");

            this.ResultTitleInfo.AddResultData("PDCURRENT_5", "PDi5");

            this.ResultTitleInfo.AddResultData("PDWATT_5", "Pf5");

            //-----------------------------------------------------------------------
            this.ResultTitleInfo.AddResultData("MVF_1", "VF1");

            this.ResultTitleInfo.AddResultData("MVF_2", "VF2");

            this.ResultTitleInfo.AddResultData("MVF_3", "VF3");

            this.ResultTitleInfo.AddResultData("MVF_4", "VF4");

            this.ResultTitleInfo.AddResultData("MVZ_1", "VZ1");

            this.ResultTitleInfo.AddResultData("MVZ_2", "VZ2");

            this.ResultTitleInfo.AddResultData("MIR_1", "IR1");

            this.ResultTitleInfo.AddResultData("MIR_2", "IR2");

            this.ResultTitleInfo.AddResultData("MVFLA_1", "VFLA1");

            this.ResultTitleInfo.AddResultData("MVFLA_2", "VFLA2");

            this.ResultTitleInfo.AddResultData("MVFLA_3", "VFLA3");

            this.ResultTitleInfo.AddResultData("MVFLA_4", "VFLA4");

            this.ResultTitleInfo.AddResultData("MVFLA_5", "VFLA5");

            //-----------------------------------------------------------------------
            if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
            {
                this.ResultTitleInfo.AddResultData("LOP_1", "LOP1");
            }
            else if (this.Product.LOPSaveItem == ELOPSaveItem.lm)
            {
                this.ResultTitleInfo.AddResultData("LM_1", "LOP1");
            }
            else if (this.Product.LOPSaveItem == ELOPSaveItem.watt)
            {
                this.ResultTitleInfo.AddResultData("WATT_1", "LOP1");
            }
            else
            {
                this.ResultTitleInfo.AddResultData("LOP_1", "LOP1");
            }

            this.ResultTitleInfo.AddResultData("WLP_1", "WLP1");

            this.ResultTitleInfo.AddResultData("WLD_1", "WLD1");

            this.ResultTitleInfo.AddResultData("WLC_1", "WLC1");

            this.ResultTitleInfo.AddResultData("HW_1", "HW1");

            this.ResultTitleInfo.AddResultData("PURITY_1", "PURITY1");

            this.ResultTitleInfo.AddResultData("CIEx_1", "X1");

            this.ResultTitleInfo.AddResultData("CIEy_1", "Y1");

            this.ResultTitleInfo.AddResultData("CIEz_1", "Z1");

            this.ResultTitleInfo.AddResultData("ST_1", "ST1");

            this.ResultTitleInfo.AddResultData("INT_1", "INT1");

            //-----------------------------------------------------------------------
            if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
            {
                this.ResultTitleInfo.AddResultData("LOP_2", "LOP2");
            }
            else if (this.Product.LOPSaveItem == ELOPSaveItem.lm)
            {
                this.ResultTitleInfo.AddResultData("LM_2", "LOP2");
            }
            else if (this.Product.LOPSaveItem == ELOPSaveItem.watt)
            {
                this.ResultTitleInfo.AddResultData("WATT_2", "LOP2");
            }
            else
            {
                this.ResultTitleInfo.AddResultData("LOP_2", "LOP2");
            }

            this.ResultTitleInfo.AddResultData("WLP_2", "WLP2");

            this.ResultTitleInfo.AddResultData("WLD_2", "WLD2");

            this.ResultTitleInfo.AddResultData("WLC_2", "WLC2");

            this.ResultTitleInfo.AddResultData("HW_2", "HW2");

            this.ResultTitleInfo.AddResultData("PURITY_2", "PURITY2");

            this.ResultTitleInfo.AddResultData("CIEx_2", "X2");

            this.ResultTitleInfo.AddResultData("CIEy_2", "Y2");

            this.ResultTitleInfo.AddResultData("CIEz_2", "Z2");

            this.ResultTitleInfo.AddResultData("ST_2", "ST2");

            this.ResultTitleInfo.AddResultData("INT_2", "INT2");

            //-----------------------------------------------------------------------
            if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
            {
                this.ResultTitleInfo.AddResultData("LOP_3", "LOP3");
            }
            else if (this.Product.LOPSaveItem == ELOPSaveItem.lm)
            {
                this.ResultTitleInfo.AddResultData("LM_3", "LOP3");
            }
            else if (this.Product.LOPSaveItem == ELOPSaveItem.watt)
            {
                this.ResultTitleInfo.AddResultData("WATT_3", "LOP3");
            }
            else
            {
                this.ResultTitleInfo.AddResultData("LOP_3", "LOP3");
            }

            this.ResultTitleInfo.AddResultData("WLP_3", "WLP3");

            this.ResultTitleInfo.AddResultData("WLD_3", "WLD3");

            this.ResultTitleInfo.AddResultData("WLC_3", "WLC3");

            this.ResultTitleInfo.AddResultData("HW_3", "HW3");

            this.ResultTitleInfo.AddResultData("PURITY_3", "PURITY3");

            this.ResultTitleInfo.AddResultData("CIEx_3", "X3");

            this.ResultTitleInfo.AddResultData("CIEy_3", "Y3");

            this.ResultTitleInfo.AddResultData("CIEz_3", "Z3");

            this.ResultTitleInfo.AddResultData("ST_3", "ST3");

            this.ResultTitleInfo.AddResultData("INT_3", "INT3");

            //-----------------------------------------------------------------------
            if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
            {
                this.ResultTitleInfo.AddResultData("LOP_4", "LOP4");
            }
            else if (this.Product.LOPSaveItem == ELOPSaveItem.lm)
            {
                this.ResultTitleInfo.AddResultData("LM_4", "LOP4");
            }
            else if (this.Product.LOPSaveItem == ELOPSaveItem.watt)
            {
                this.ResultTitleInfo.AddResultData("WATT_4", "LOP4");
            }
            else
            {
                this.ResultTitleInfo.AddResultData("LOP_4", "LOP4");
            }

            this.ResultTitleInfo.AddResultData("WLP_4", "WLP4");

            this.ResultTitleInfo.AddResultData("WLD_4", "WLD4");

            this.ResultTitleInfo.AddResultData("WLC_4", "WLC4");

            this.ResultTitleInfo.AddResultData("HW_4", "HW4");

            this.ResultTitleInfo.AddResultData("PURITY_4", "PURITY4");

            this.ResultTitleInfo.AddResultData("CIEx_4", "X4");

            this.ResultTitleInfo.AddResultData("CIEy_4", "Y4");

            this.ResultTitleInfo.AddResultData("CIEz_4", "Z4");

            this.ResultTitleInfo.AddResultData("ST_4", "ST4");

            this.ResultTitleInfo.AddResultData("INT_4", "INT4");

            //-----------------------------------------------------------------------
            if (this.Product.LOPSaveItem == ELOPSaveItem.mcd)
            {
                this.ResultTitleInfo.AddResultData("LOP_5", "LOP5");
            }
            else if (this.Product.LOPSaveItem == ELOPSaveItem.lm)
            {
                this.ResultTitleInfo.AddResultData("LM_5", "LOP5");
            }
            else if (this.Product.LOPSaveItem == ELOPSaveItem.watt)
            {
                this.ResultTitleInfo.AddResultData("WATT_5", "LOP5");
            }
            else
            {
                this.ResultTitleInfo.AddResultData("LOP_5", "LOP5");
            }

            this.ResultTitleInfo.AddResultData("WLP_5", "WLP5");

            this.ResultTitleInfo.AddResultData("WLD_5", "WLD5");

            this.ResultTitleInfo.AddResultData("WLC_5", "WLC5");

            this.ResultTitleInfo.AddResultData("HW_5", "HW5");

            this.ResultTitleInfo.AddResultData("PURITY_5", "PURITY5");

            this.ResultTitleInfo.AddResultData("CIEx_5", "X5");

            this.ResultTitleInfo.AddResultData("CIEy_5", "Y5");

            this.ResultTitleInfo.AddResultData("CIEz_5", "Z5");

            this.ResultTitleInfo.AddResultData("ST_5", "ST5");

            this.ResultTitleInfo.AddResultData("INT_5", "INT5");
        } 

        protected override EErrorCode WriteReportHeadByUser()
		{
            ////////////////////////////////////////////
            //Write Report Head
            ////////////////////////////////////////////
            this.WriteLine("Output File Name," + this.TestResultFileNameWithoutExt());

            this.WriteLine("Date & time," + this.TesterSetting.EndTestTime.ToString("yyyy/MM/dd HH:mm"));

            this.WriteLine("Product type," + this.UISetting.ProductType);

            this.WriteLine("Barcode," + this.UISetting.Barcode);

            this.WriteLine("Lot Number," + this.UISetting.LotNumber);

            this.WriteLine("Wafer Number," + this.UISetting.WaferNumber);

            this.WriteLine("Operator," + this.UISetting.OperatorName);

            this.WriteLine("First subdie ID," + this.UISetting.SlotNumber);

            this.WriteLine("User Comment,");

            this.WriteLine("");

            //----------------------------------------------------------------------------------------------------------------------------------
            this.WriteLine("Item,Bias,BiasUnit,Time(ms),Compliance,CompUnit,Lower,Upper,Sqr,Gain,Offset,RltUnit");

            string lineTestItemCondition = string.Empty;

            if (this.Product.TestCondition != null &&
                this.Product.TestCondition.TestItemArray != null &&
                this.Product.TestCondition.TestItemArray.Length > 0)
            {
                foreach (var testItem in this.Product.TestCondition.TestItemArray)
                {
                    if (testItem.MsrtResult == null || testItem.MsrtResult.Length == 0 || !testItem.IsEnable || testItem.ElecSetting == null)
                    {
                        continue;
                    }

                    string msrtUnit = testItem.MsrtResult[0].Unit;

                    if (testItem is LOPTestItem && testItem.ElecSetting[0].MsrtType == DeviceCommon.EMsrtType.FVMILOP)
                    {
                        msrtUnit = testItem.MsrtResult[2].Unit;
                    }


                    lineTestItemCondition = string.Empty;

                    lineTestItemCondition += testItem.Name + ",";

                    lineTestItemCondition += testItem.ElecSetting[0].ForceValue + ",";

                    lineTestItemCondition += testItem.ElecSetting[0].ForceUnit + ",";

                    lineTestItemCondition += testItem.ElecSetting[0].ForceTime + ",";

                    lineTestItemCondition += testItem.ElecSetting[0].MsrtProtection + ",";

                    lineTestItemCondition += msrtUnit + ",";

                    lineTestItemCondition += ",";

                    lineTestItemCondition += ",";

                    lineTestItemCondition += ",";

                    if (testItem.GainOffsetSetting != null)
                    {
                        lineTestItemCondition += testItem.GainOffsetSetting[0].Gain + ",";

                        lineTestItemCondition += testItem.GainOffsetSetting[0].Offset + ",";

                        lineTestItemCondition += msrtUnit + ",";
                    }

                    this.WriteLine(lineTestItemCondition);
                }
            }

            this.WriteLine("");
     
			this.WriteLine(this.ResultTitleInfo.TitleStr);

			return EErrorCode.NONE;
		}

		protected override EErrorCode RewriteReportByUser()
		{
			Dictionary<string, string> replaceData = new Dictionary<string, string>();

			string comment = string.Empty;

			comment = "User Comment," + this.UISetting.ReportComments;

			replaceData.Add("User Comment,", comment);

            this.ReplaceReport(replaceData);

			return EErrorCode.NONE;
		}

        // LIV 報表
        protected override EErrorCode WriteReportHeadByUser2()
        {
            if (!this.UISetting.IsEnableSaveLIVData)
            {
                return EErrorCode.NONE;
            }

            this.WriteLine2("DUT,ColX,RowY,Name,I(mA),V(V),L(mW)");

            return EErrorCode.NONE;
        }

        protected override EErrorCode PushDataByUser2(Dictionary<string, double> data)
        {
            if (!this.UISetting.IsEnableSaveLIVData)
            {
                return EErrorCode.NONE;
            }

            string testCount = this.AcquireData.ChipInfo.TestCount.ToString();

            string col = data["COL"].ToString();

            string row = data["ROW"].ToString();
            string name = string.Empty;
            double curr = 0.0d;
            double volt = 0.0d;
            double pow = 0.0d;

            foreach (var liv in this.AcquireData.LIVDataSet)
            {
                if (!liv.IsEnable)
                {
                    continue;
                }

                for (int i = 0; i < liv[ELIVOptiMsrtType.LIVSETVALUE].DataArray.Length; i++)
                {
                    string IntLine = string.Empty;

                    name = liv.Name;

                    curr = liv[ELIVOptiMsrtType.LIVSETVALUE].DataArray[i];

                    volt = liv[ELIVOptiMsrtType.LIVMsrtV].DataArray[i];

                    pow = liv[ELIVOptiMsrtType.LIVPDWATT].DataArray[i];

                    IntLine = string.Format("DUT{0},{1},{2},{3},{4},{5},{6}", testCount, col, row, name, curr, volt, pow);

                    this.WriteLine2(IntLine);
                }
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

            string fileNameWithExt = fileNameWithoutExt + "_LIV.csv";

            string outputPathAndFile = Path.Combine(outPath, fileNameWithExt);

            MPIFile.CopyFile(this.FileFullNameRep2, outputPathAndFile);

            return EErrorCode.NONE;
        }


        //// RelativeSpectrum
        //protected override EErrorCode WriteReportHeadByUser3()
        //{
        //    if (!this.UISetting.IsEnableSaveRelativeSpectrum)
        //    {
        //        return EErrorCode.NONE;
        //    }

        //    this.WriteLine3("Chip,Name,Wavelength");

        //    return EErrorCode.NONE;
        //}

        //protected override EErrorCode PushDataByUser3(Dictionary<string, double> data)
        //{
        //    if (!this.UISetting.IsEnableSaveRelativeSpectrum)
        //    {
        //        return EErrorCode.NONE;
        //    }

        //    string col = data["COL"].ToString();

        //    string row = data["ROW"].ToString();

        //    string key = string.Format("C{0}R{1}", col, row);

        //    foreach (var spectrum in this.AcquireData.SpectrumDataSet)
        //    {
        //        if (!spectrum.IsEnable)
        //        {
        //            continue;
        //        }

        //        uint startWl = this.TesterSetting.OptiDevSetting.StartWavelength;
        //        uint endWl = this.TesterSetting.OptiDevSetting.EndWavelength;

        //        this._lstWL.Clear();

        //        string nameStr = "," + spectrum.Name;

        //        string IntLine = key + nameStr + ",Intensity";

        //        for (int i = 0; i < spectrum.Wavelength.Length; i++)
        //        {
        //            double wl = spectrum.Wavelength[i];

        //            if (wl >= startWl && wl <= endWl)
        //            {
        //                this._lstWL.Add(wl);

        //                IntLine += "," + spectrum.Intensity[i].ToString();
        //            }
        //        }

        //        this.WriteLine3(IntLine);
        //    }

        //    return EErrorCode.NONE;
        //}

        //protected override EErrorCode RewriteReportByUser3()
        //{
        //    if (!this.UISetting.IsEnableSaveRelativeSpectrum)
        //    {
        //        return EErrorCode.NONE;
        //    }

        //    if (!File.Exists(FileFullNameTmp3))
        //    {
        //        return EErrorCode.NONE;
        //    }

        //    string title = "Chip,Name,Wavelength";

        //    if (this._lstWL != null || this._lstWL.Count > 0)
        //    {
        //        foreach (var item in this._lstWL)
        //        {
        //            title += "," + item.ToString();
        //        }
        //    }

        //    Dictionary<string, string> replaceData = new Dictionary<string, string>();

        //    replaceData.Add("Chip,Name,Wavelength", title);

        //    using (StreamReader sr = new StreamReader(this.FileFullNameTmp3, Encoding.Default))
        //    {
        //        using (StreamWriter sw = new StreamWriter(this.FileFullNameRep3))
        //        {
        //            while (sr.Peek() >= 0)
        //            {
        //                string line = sr.ReadLine();

        //                if (replaceData.ContainsKey(line))
        //                {
        //                    line = replaceData[line];
        //                }

        //                sw.WriteLine(line);
        //            }
        //        }
        //    }

        //    return EErrorCode.NONE;
        //}

        //protected override EErrorCode MoveFileToTargetByUser3(EServerQueryCmd cmd)
        //{
        //    if (!this.UISetting.IsEnableSaveRelativeSpectrum)
        //    {
        //        return EErrorCode.NONE;
        //    }

        //    string outPath = this.UISetting.RelativeSpectrumPath;

        //    string fileNameWithoutExt = this.TestResultFileNameWithoutExt();

        //    //Abort
        //    if (cmd == EServerQueryCmd.CMD_TESTER_ABORT)
        //    {
        //        fileNameWithoutExt = fileNameWithoutExt + "_" + DateTime.Now.ToString("yyMMddhhmmss");
        //    }

        //    string fileNameWithExt = fileNameWithoutExt + ".rel";

        //    string outputPathAndFile = Path.Combine(outPath, fileNameWithExt);

        //    MPIFile.CopyFile(this.FileFullNameRep3, outputPathAndFile);

        //    return EErrorCode.NONE;
        //}

		#endregion
	}
}
