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

namespace MPI.Tester.Report.User.Luxnet
{
	public class Report : ReportBase
	{
        private List<double> _lstWL = new List<double>();

		public Report( List<object> objs, bool isReStatistic )
			: base( objs, isReStatistic )
		{
            this._isImplementPIVDataReport = true;
            this._isImplementSpectrumReport = true;
        }

        #region >>> Protected Override Method <<<

        protected override void SetResultTitle()
		{
            this.ResultTitleInfo.SetResultData(this.UISetting.UserDefinedData.ResultItemNameDic);
        } 

		protected override EErrorCode PushDataByUser( Dictionary<string, double> data )
		{
			int binSN = ( int ) data[ "BINSN" ];

			SmartBinDataBase bin = SmartBinning.GetBinFromSN( binSN );

			int binGrade = 0;

			int binNumber = 0;

			string binCode = string.Empty;

			if ( bin != null )
			{
				binCode = bin.BinCode;

				binNumber = bin.BinNumber;

				if ( bin.BinningType == EBinningType.IN_BIN )
				{
					binGrade = 1;
				}
				else if ( bin.BinningType == EBinningType.SIDE_BIN )
				{
					binGrade = 2;
				}
				else if ( bin.BinningType == EBinningType.NG_BIN )
				{
					binGrade = 3;
				}
			}

			string line = string.Empty;

			int index = 0;

			foreach ( var item in ResultTitleInfo )
			{
				if ( item.Key == "BIN_CODE" )
				{
					line += binCode;
				}
				else if ( item.Key == "BIN_NUMBER" )
				{
					line += binNumber.ToString();
				}
				else if ( item.Key == "BIN_GRADE" )
				{
					line += binGrade.ToString();
				}
				else if ( item.Key == "BIN_RESULT" )
				{
					switch ( binGrade )
					{
						case 0:
							break;
						case 1:
							line += "OK";
							break;
						default:
							line += "NG";
							break;
					}
				}
				else if ( data.ContainsKey( item.Key ) )
				{
					string format = string.Empty;

					if ( UISetting.UserDefinedData[ item.Key ] != null )
					{
						format = UISetting.UserDefinedData[ item.Key ].Formate;
					}

					line += data[ item.Key ].ToString( format );
				}

				index++;

				if ( index != ResultTitleInfo.ResultCount )
				{
					line += ",";
				}
			}

			this.WriteLine( line );

			return EErrorCode.NONE;
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
			this.WriteLine( "LotID," + this.UISetting.LotNumber );
			this.WriteLine( "WaferID," + this.UISetting.WaferNumber );
            this.WriteLine("Operator," + this.UISetting.OperatorName);
			this.WriteLine( "Temperature," + ( ( int ) this.UISetting.StartTemp ).ToString() );
            this.WriteLine("Factor," + this.Product.PdDetectorFactor);
            this.WriteLine("");

			this.WriteLine( "Condition" );
			this.WriteConditionLine();
			this.WriteLine( "" );

            this.WriteLine(this.ResultTitleInfo.TitleStr);

            return EErrorCode.NONE;
		}

		protected override EErrorCode RewriteReportByUser()
		{
            return base.RewriteReportByDefault();
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
            if (!this.UISetting.IsEnableSaveRelativeSpectrum)
            {
                return EErrorCode.NONE;
            }

            string outPath = this.UISetting.RelativeSpectrumPath;

            string fileNameWithoutExt = this.TestResultFileNameWithoutExt();

            //Abort
            if (cmd == EServerQueryCmd.CMD_TESTER_ABORT)
            {
                fileNameWithoutExt = fileNameWithoutExt + "_" + DateTime.Now.ToString("yyMMddhhmmss");
            }

            string fileNameWithExt = fileNameWithoutExt + ".rel";

            string outputPathAndFile = Path.Combine(outPath, fileNameWithExt);

            MPIFile.CopyFile(this.FileFullNameRep2, outputPathAndFile);

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

            MPIFile.CopyFile(this.FileFullNameRep3, outputPathAndFile);

            return EErrorCode.NONE;
        }

		#endregion
		
		#region >>> Private Method <<<

		private void WriteConditionLine()
		{
			if ( this.Product.TestCondition != null &&
				this.Product.TestCondition.TestItemArray != null &&
				this.Product.TestCondition.TestItemArray.Length > 0 )
			{
				foreach ( var testItem in this.Product.TestCondition.TestItemArray )
				{
					if ( testItem.MsrtResult == null || testItem.MsrtResult.Length == 0 || !testItem.IsEnable )
					{
						continue;
					}

						foreach ( var msrtItem in testItem.MsrtResult )
						{
						string lineItem = string.Empty;

							if ( !msrtItem.IsEnable || !msrtItem.IsVision )
							{
								continue;
							}

						if ( testItem is PIVTestItem )
						{
							switch ( msrtItem.KeyName )
							{
								case "Ith_1":
									lineItem += testItem.MsrtResult[ ( int ) ELaserMsrtType.Ith ].Name;
							lineItem += ",";
									lineItem += "P1=" + ( testItem as PIVTestItem ).CalcSetting.ThresholdSectionLowLimitP.ToString() + "mW";
									lineItem += ",";
									lineItem += "P2=" + ( testItem as PIVTestItem ).CalcSetting.ThresholdSectionUpperLimitP.ToString() + "mW";
									lineItem += ",";
									lineItem += "Po>=" + ( testItem as PIVTestItem ).CalcSetting.ThresholdSearchValue2.ToString() + "mW";
									break;

								case "Vth_1":
									lineItem += testItem.MsrtResult[ ( int ) ELaserMsrtType.Vth ].Name;
									lineItem += ",";
									lineItem += "P1=" + ( testItem as PIVTestItem ).CalcSetting.ThresholdSectionLowLimitP.ToString() + "mW";
									lineItem += ",";
									lineItem += "P2=" + ( testItem as PIVTestItem ).CalcSetting.ThresholdSectionUpperLimitP.ToString() + "mW";
									lineItem += ",";
									lineItem += "Po>=" + ( testItem as PIVTestItem ).CalcSetting.ThresholdSearchValue2.ToString() + "mW";
									break;

								case "SE_1":
									lineItem += testItem.MsrtResult[ ( int ) ELaserMsrtType.SE ].Name;

									if ( ( testItem as PIVTestItem ).CalcSetting.SeSearchMode == Data.ELaserSearchMode.byPower )
			{
										lineItem += ",";
										lineItem += "P1=" + ( testItem as PIVTestItem ).CalcSetting.SeSectionLowLimitP.ToString() + "mW";
										lineItem += ",";
										lineItem += "P2=" + ( testItem as PIVTestItem ).CalcSetting.SeSectionUpperLimitP.ToString() + "mW";
									}
									else
					{
										lineItem += ",";
										lineItem += "I1=" + ( testItem as PIVTestItem ).CalcSetting.SeSectionLowLimitI.ToString() + "mA";
										lineItem += ",";
										lineItem += "I2=" + ( testItem as PIVTestItem ).CalcSetting.SeSectionUpperLimitI.ToString() + "mA";
					}
									break;

								case "Vop_1":
									lineItem += testItem.MsrtResult[ ( int ) ELaserMsrtType.Vop ].Name;
									lineItem += ",";
									lineItem += "Pop=" + ( testItem as PIVTestItem ).CalcSetting.Pop.ToString() + "mW";
									break;

								case "Iop_1":
									lineItem += testItem.MsrtResult[ ( int ) ELaserMsrtType.Iop ].Name;
									lineItem += ",";
									lineItem += "Pop=" + ( testItem as PIVTestItem ).CalcSetting.Pop.ToString() + "mW";
									break;

								case "RS_1":
									lineItem += testItem.MsrtResult[ ( int ) ELaserMsrtType.RS ].Name;

									if ( ( testItem as PIVTestItem ).CalcSetting.RsSearchMode == Data.ELaserSearchMode.byPower )
						{
										lineItem += ",";
										lineItem += "P1=" + ( ( testItem as PIVTestItem ).CalcSetting.RsSectionLowLimitP * 1000 ).ToString() + "mW";
										lineItem += ",";
										lineItem += "P2=" + ( ( testItem as PIVTestItem ).CalcSetting.RsSectionUpperLimitP * 1000 ).ToString() + "mW";
									}
									else
							{
										lineItem += ",";
										lineItem += "I1=" + ( ( testItem as PIVTestItem ).CalcSetting.RsSectionLowLimitI * 1000 ).ToString() + "mA";
										lineItem += ",";
										lineItem += "I2=" + ( ( testItem as PIVTestItem ).CalcSetting.RsSectionUpperLimitI * 1000 ).ToString() + "mA";
							}
									break;

								case "Ppk_1":
									lineItem += testItem.MsrtResult[ ( int ) ELaserMsrtType.Ppk ].Name;
									lineItem += ",";
									lineItem += ( testItem as PIVTestItem ).ElecSetting[ 0 ].SweepStop.ToString() + "mA";
									break;

								case "PfA_1":
									lineItem += testItem.MsrtResult[ ( int ) ELaserMsrtType.PfA ].Name;
									lineItem += ",";
									lineItem += "IfA=" + ( ( testItem as PIVTestItem ).CalcSetting.IfA * 1000 ).ToString() + "mA";
									break;

								case "VfA_1":
									lineItem += testItem.MsrtResult[ ( int ) ELaserMsrtType.VfA ].Name;
									lineItem += ",";
									lineItem += "IfA=" + ( ( testItem as PIVTestItem ).CalcSetting.IfA * 1000 ).ToString() + "mA";
									break;

								case "PfB_1":
									lineItem += testItem.MsrtResult[ ( int ) ELaserMsrtType.PfB ].Name;
									lineItem += ",";
									lineItem += "IfB=" + ( ( testItem as PIVTestItem ).CalcSetting.IfB * 1000 ).ToString() + "mA";
									break;

								case "VfB_1":
									lineItem += testItem.MsrtResult[ ( int ) ELaserMsrtType.VfB ].Name;
									lineItem += ",";
									lineItem += "IfB=" + ( ( testItem as PIVTestItem ).CalcSetting.IfB * 1000 ).ToString() + "mA";
									break;

								case "PfC_1":
									lineItem += testItem.MsrtResult[ ( int ) ELaserMsrtType.PfC ].Name;
									lineItem += ",";
									lineItem += "IfC=" + ( ( testItem as PIVTestItem ).CalcSetting.IfC * 1000 ).ToString() + "mA";
									break;

								case "VfC_1":
									lineItem += testItem.MsrtResult[ ( int ) ELaserMsrtType.VfC ].Name;
									lineItem += ",";
									lineItem += "IfC=" + ( ( testItem as PIVTestItem ).CalcSetting.IfC * 1000 ).ToString() + "mA";
									break;

								default:
									continue;
							}
						}
						else
						{
							lineItem += msrtItem.Name;
							lineItem += ",";
							lineItem += testItem.ElecSetting[ 0 ].ForceValue.ToString() + testItem.ElecSetting[ 0 ].ForceUnit;
						}

						this.WriteLine( lineItem );

					}
				}
			}
		}

		#endregion
	}
}
