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

namespace MPI.Tester.Report.User.ETI
{
	public class Report : ReportBase
	{
        private StreamWriter _sw1;
        private StreamWriter _sw2;
        private StreamWriter _sw3;
        private StreamWriter _sw4;

        private string sweepFileFullNameTmp1;
        private string sweepFileFullNameTmp2;
        private string sweepFileFullNameTmp3;
        private string sweepFileFullNameTmp4;

        public Report(List<object> objs, bool isReStatistic): base(objs, isReStatistic)
		{
            this._isImplementSweepDataReport = true;

            this.sweepFileFullNameTmp1 = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "NTLM1.temp");
            this.sweepFileFullNameTmp2 = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "NTLM2.temp");
            this.sweepFileFullNameTmp3 = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "NTLM3.temp");
            this.sweepFileFullNameTmp4 = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "NTLM4.temp");
        }

        #region >>> Private Method <<<

        private void OpenWriter()
        {
            this.CloseWriter();

            if (File.Exists(this.sweepFileFullNameTmp1))
            {
                MPIFile.DeleteFile(this.sweepFileFullNameTmp1);
            }

            if (File.Exists(this.sweepFileFullNameTmp2))
            {
                MPIFile.DeleteFile(this.sweepFileFullNameTmp2);
            }

            if (File.Exists(this.sweepFileFullNameTmp3))
            {
                MPIFile.DeleteFile(this.sweepFileFullNameTmp3);
            }

            if (File.Exists(this.sweepFileFullNameTmp4))
            {
                MPIFile.DeleteFile(this.sweepFileFullNameTmp4);
            }

            this._sw1 = new StreamWriter(this.sweepFileFullNameTmp1, true, Encoding.Default);
            this._sw2 = new StreamWriter(this.sweepFileFullNameTmp2, true, Encoding.Default);
            this._sw3 = new StreamWriter(this.sweepFileFullNameTmp3, true, Encoding.Default);
            this._sw4 = new StreamWriter(this.sweepFileFullNameTmp4, true, Encoding.Default);
        }

        private void CloseWriter()
        {
            if (this._sw1 != null)
            {
                this._sw1.Close();

                this._sw1.Dispose();

                this._sw1 = null;
            }

            if (this._sw2 != null)
            {
                this._sw2.Close();

                this._sw2.Dispose();

                this._sw2 = null;
            }

            if (this._sw3 != null)
            {
                this._sw3.Close();

                this._sw3.Dispose();

                this._sw3 = null;
            }

            if (this._sw4 != null)
            {
                this._sw4.Close();

                this._sw4.Dispose();

                this._sw4 = null;
            }
        }

        private void Writer1(string str)
        {
            if (this._sw1 != null)
            {
                this._sw1.WriteLine(str);

                this._sw1.Flush();
            }
        }

        private void Writer2(string str)
        {
            if (this._sw2 != null)
            {
                this._sw2.WriteLine(str);

                this._sw2.Flush();
            }
        }

        private void Writer3(string str)
        {
            if (this._sw3 != null)
            {
                this._sw3.WriteLine(str);

                this._sw3.Flush();
            }
        }

        private void Writer4(string str)
        {
            if (this._sw4 != null)
            {
                this._sw4.WriteLine(str);

                this._sw4.Flush();
            }
        }

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

        // NTLM (Sweep)
        protected override EErrorCode WriteReportHeadByUser2()
        {
            if (!this.UISetting.IsEnableSaveSweepData)
            {
                return EErrorCode.NONE;
            }

            this.OpenWriter();
            
            return EErrorCode.NONE;
        }
        
        protected override EErrorCode PushDataByUser2(Dictionary<string, float> data)
        {
            if (!this.UISetting.IsEnableSaveSweepData)
            {
                return EErrorCode.NONE;
            }

            if (this._sw1 == null)
            {
                this._sw1 = new StreamWriter(this.sweepFileFullNameTmp1, true, Encoding.Default);
            }

            if (this._sw2 == null)
            {
                this._sw2 = new StreamWriter(this.sweepFileFullNameTmp2, true, Encoding.Default);
            }

            if (this._sw3 == null)
            {
                this._sw3 = new StreamWriter(this.sweepFileFullNameTmp3, true, Encoding.Default);
            }

            if (this._sw4 == null)
            {
                this._sw4 = new StreamWriter(this.sweepFileFullNameTmp4, true, Encoding.Default);
            }

            string test = data["TEST"].ToString();
            string col = data["COL"].ToString();
            string row = data["ROW"].ToString();
           
            foreach(var sweep in this.AcquireData.ElecSweepDataSet)
            {
                if (!sweep.IsEnable)
                {
                    continue;
                }

                string name = sweep.Name;

                switch(sweep.KeyName)
                {
                    case "IVSWEEP_1":
                        {
                            // Write Header Description
                            this.Writer1(string.Format("MeasureType,{0}", name));
                            this.Writer1(string.Format("Die X,{0}", col));
                            this.Writer1(string.Format("Die Y,{0}", row));
                            this.Writer1(string.Format("Die No.,{0}", test));
                            this.Writer1(string.Format("Bin,255"));
                            this.Writer1(string.Format("{0}.Voltage(V),{1}.Current(A)", name, name));

                            for (int i = 0; i < sweep.ApplyData.Length; i++)
                            {
                                this.Writer1(string.Format("{0},{1}", sweep.SweepData[i].ToString("0.0000"), sweep.ApplyData[i].ToString("0.0000")));
                            }

                            this.Writer1("");

                            break;
                        }
                    case "IVSWEEP_2":
                        {
                            // Write Header Description
                            this.Writer2(string.Format("MeasureType,{0}", name));
                            this.Writer2(string.Format("Die X,{0}", col));
                            this.Writer2(string.Format("Die Y,{0}", row));
                            this.Writer2(string.Format("Die No.,{0}", test));
                            this.Writer2(string.Format("Bin,255"));
                            this.Writer2(string.Format("{0}.Voltage(V),{1}.Current(A)", name, name));

                            for (int i = 0; i < sweep.ApplyData.Length; i++)
                            {
                                this.Writer2(string.Format("{0},{1}", sweep.SweepData[i].ToString("0.0000"), sweep.ApplyData[i].ToString("0.0000")));
                            }

                            this.Writer2("");

                            break;
                        }
                    case "IVSWEEP_3":
                        {
                            // Write Header Description
                            this.Writer3(string.Format("MeasureType,{0}", name));
                            this.Writer3(string.Format("Die X,{0}", col));
                            this.Writer3(string.Format("Die Y,{0}", row));
                            this.Writer3(string.Format("Die No.,{0}", test));
                            this.Writer3(string.Format("Bin,255"));
                            this.Writer3(string.Format("{0}.Voltage(V),{1}.Current(A)", name, name));

                            for (int i = 0; i < sweep.ApplyData.Length; i++)
                            {
                                this.Writer3(string.Format("{0},{1}", sweep.SweepData[i].ToString("0.0000"), sweep.ApplyData[i].ToString("0.0000")));
                            }

                            this.Writer3("");

                            break;
                        }
                    case "IVSWEEP_4":
                        {
                            // Write Header Description
                            this.Writer4(string.Format("MeasureType,{0}", name));
                            this.Writer4(string.Format("Die X,{0}", col));
                            this.Writer4(string.Format("Die Y,{0}", row));
                            this.Writer4(string.Format("Die No.,{0}", test));
                            this.Writer4(string.Format("Bin,255"));
                            this.Writer4(string.Format("{0}.Voltage(V),{1}.Current(A)", name, name));

                            for (int i = 0; i < sweep.ApplyData.Length; i++)
                            {
                                this.Writer4(string.Format("{0},{1}", sweep.SweepData[i].ToString("0.0000"), sweep.ApplyData[i].ToString("0.0000")));
                            }

                            this.Writer4("");

                            break;
                        }          
                }
            }

             return EErrorCode.NONE;
        }

        protected override EErrorCode RewriteReportByUser2()
        {
            if (!this.UISetting.IsEnableSaveSweepData)
            {
                return EErrorCode.NONE;
            }
            
            return EErrorCode.NONE;
        }

        protected override EErrorCode MoveFileToTargetByUser2(EServerQueryCmd cmd)
        {
            if (!this.UISetting.IsEnableSaveSweepData)
            {
                return EErrorCode.NONE;
            }

            if (this.AcquireData.ElecSweepDataSet.Count == 0)
            {
                this.CloseWriter();

                return EErrorCode.NONE;
            }

            string outPath = this.UISetting.SweepDataSavePath;

            string fileNameWithoutExt = this.TestResultFileNameWithoutExt();

            //Abort
            if (cmd == EServerQueryCmd.CMD_TESTER_ABORT)
            {
                fileNameWithoutExt = fileNameWithoutExt + "_" + DateTime.Now.ToString("yyMMddhhmmss");
            }

            string fileNameWithExt1 = fileNameWithoutExt + "_NTLM1.SWP";
            string fileNameWithExt2 = fileNameWithoutExt + "_NTLM2.SWP";
            string fileNameWithExt3 = fileNameWithoutExt + "_NTLM3.SWP";
            string fileNameWithExt4 = fileNameWithoutExt + "_NTLM4.SWP";

            string outputPathAndFile1 = Path.Combine(outPath, fileNameWithExt1);
            string outputPathAndFile2 = Path.Combine(outPath, fileNameWithExt2);
            string outputPathAndFile3 = Path.Combine(outPath, fileNameWithExt3);
            string outputPathAndFile4 = Path.Combine(outPath, fileNameWithExt4);

            MPIFile.CopyFile(this.sweepFileFullNameTmp1, outputPathAndFile1);
            MPIFile.CopyFile(this.sweepFileFullNameTmp2, outputPathAndFile2);
            MPIFile.CopyFile(this.sweepFileFullNameTmp3, outputPathAndFile3);
            MPIFile.CopyFile(this.sweepFileFullNameTmp4, outputPathAndFile4);

            this.CloseWriter();

            return EErrorCode.NONE;
        }


		#endregion
	}
}
