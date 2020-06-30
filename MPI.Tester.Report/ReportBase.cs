using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;
using System.IO;
using MPI.Tester.TestServer;
using MPI.Tester.Maths;
using MPI.Tester.TestKernel;
using MPI.Tester.Tools;
using MPI.Tester.Report.BaseMethod.HeaderFinder;
using MPI.Tester.Report.BaseMethod.PosKeyMaker;

namespace MPI.Tester.Report
{
	public  abstract partial class ReportBase
	{
		//System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
		//sw.Restart();
		//sw.Stop();
		//time = sw.ElapsedMilliseconds;

		#region >>> Private Property <<<

		protected const string EXTEN_TMP1 = ".tmp";
		protected const string EXTEN_TMP2 = ".tmp2";
		protected const string EXTEN_TMP3 = ".tmp3";
        protected const string EXTEN_TMPS1 = ".tmps";
        protected const string EXTEN_TMPS2 = ".tmps2";
        protected const string EXTEN_TMPS3 = ".tmps3";
        protected const string EXTEN_TMPS4 = ".tmps4";
        protected const string EXTEN_TMPSPA = ".tmpspa";
        protected const string EXTEN_TMPSPR = ".tmpspr";
		protected const string EXTEN_REP1 = ".rep";
		protected const string EXTEN_REP2 = ".rep2";
		protected const string EXTEN_REP3 = ".rep3";
        protected const string EXTEN_REPS1 = ".reps";
        protected const string EXTEN_REPS2 = ".reps2";
        protected const string EXTEN_REPS3 = ".reps3";
        protected const string EXTEN_REPS4 = ".reps4";
        protected const string EXTEN_REPSPA = ".repspa";
        protected const string EXTEN_REPSPR = ".repspr";
        protected const string EXTEN_LASERLOG = ".tmplaser";
        protected bool _isReStatistic;
		protected bool _isAppend;
		protected int _appendFileTestCount;
		protected StreamWriter _sw;
		protected StreamWriter _sw2;
		protected StreamWriter _sw3;
		protected StreamReader _sr;
		private string _fileFullNameTmp = "";
		private string _fileFullNameTmp2 = "";
		private string _fileFullNameTmp3 = "";
		private string _fileFullNameRep = "";
		private string _fileFullNameRep2 = "";
		private string _fileFullNameRep3 = "";
		protected ResultTitleInfo _resultTitleInfo;
        protected ResultTitleInfo _resultTitleInfo2;
        protected ResultTitleInfo _resultTitleInfo3;
		private Dictionary<string, TestResultData> _resultData;
		protected Dictionary<string, int> _checkColRowKey;
		private ProductData _product;
		private UISetting _uiSetting;
		private TesterSetting _testerSetting;
		private SmartBinning _smartBinning;
		private MachineInfoData _machineInfo;
		private MachineConfig _machineConfig;        
		private AcquireData _acquireData;
		private Dictionary<string, ColRow> _findChannel1ColRow;
        protected ColRow _lastColRow;
		private IRICalcMath _riCalcMath = new RIMathsA();
        protected ReportProjectData _reportData;
        protected OptiReportData _optiReportData;
        //private ConditionCtrl _conditionData;
        protected SystemCali _sysCali;

        protected CoordTransferTool _customizeCoordTransferTool;
        protected CoordTransferTool _p2tCoordTransferTool;
 		protected int TitleStrShift = 0;
        protected char _spiltChar;
        protected string _titleStrKey; // When reload temp file, use this key to find title row.
        protected bool _saveMapAtAbort = true;
        protected PosKeyMakerBase _crKeyMaker = null;
        //protected bool _isRetest = false;

		#endregion

		#region >>> Constructor / Disposor <<<

		public ReportBase(List<object> objs, bool isReStatistic)
		{
            Console.WriteLine("[ReportBase], Create !! ");

			this._isReStatistic = false;

			this._isAppend = false;

			this._product = null;

			this._uiSetting = null;

			this._testerSetting = null;

			this._smartBinning = null;

			this._machineInfo = null;

			this._resultData = new Dictionary<string, TestResultData>();

			this._resultTitleInfo = new ResultTitleInfo();

			this._resultTitleInfo2 = new ResultTitleInfo();

			this._resultTitleInfo3 = new ResultTitleInfo();

			this._checkColRowKey = new Dictionary<string, int>();

			this._findChannel1ColRow = new Dictionary<string,ColRow>();

			this._isImplementLIVReport = false;

			this._isImplementSpectrumReport = false;

			this._isImplementSweepDataReport = false;

			this._isImplementPIVDataReport = false;

            this._spiltChar = (",").ToCharArray()[0];

			foreach (var item in objs)
			{
				if (item is UISetting)
				{
					this._uiSetting = item as UISetting;
				}
				else if (item is TesterSetting)
				{
					this._testerSetting = item as TesterSetting;
				}
				else if (item is ProductData)
				{
					this._product = item as ProductData;
				}
				else if (item is MachineInfoData)
				{
					this._machineInfo = item as MachineInfoData;
				}
				else if (item is MachineConfig)
				{
					this._machineConfig = item as MachineConfig;
				}
				else if (item is SmartBinning)
				{
					this._smartBinning = item as SmartBinning;
				}
				else if (item is AcquireData)
				{
					this._acquireData = item as AcquireData;
				}
				else if (item is ReportProjectData)
				{
					this._reportData = item as ReportProjectData;
				}
                else if (item is SystemCali)
                {
                    this._sysCali = item as SystemCali;
                }
			}
            _titleStrKey = "";

			this.ResetResultData();

			this.SetResultTitle();

			this.ClearTempFile();

			this._optiReportData = new OptiReportData(this.ResultTitleInfo);

            this._customizeCoordTransferTool = new CoordTransferTool();

            RemarkList = new List<string>();

            IsLogFileCreated = false;

            _crKeyMaker = null ;

            //_isRetest = false;
		}

		#endregion 

		#region >>> Private Method <<<

		private void ClearTempFile()
		{ 
			try
			{
				if (!Directory.Exists(Constants.Paths.MPI_TEMP_DIR2))
				{
					Directory.CreateDirectory(Constants.Paths.MPI_TEMP_DIR2);
				}

				DirectoryInfo di = new DirectoryInfo(Constants.Paths.MPI_TEMP_DIR2);

				List<FileInfo> fiList = new List<FileInfo>();

				fiList.AddRange(di.GetFiles("*" + ReportBase.EXTEN_TMP1));

				fiList.AddRange(di.GetFiles("*" + ReportBase.EXTEN_TMP2));

				fiList.AddRange(di.GetFiles("*" + ReportBase.EXTEN_TMP3));

				fiList.AddRange(di.GetFiles("*" + ReportBase.EXTEN_REP1));

				fiList.AddRange(di.GetFiles("*" + ReportBase.EXTEN_REP2));

				fiList.AddRange(di.GetFiles("*" + ReportBase.EXTEN_REP3));

				DateTime now = DateTime.Now;

				foreach (var fi in fiList)
				{
					TimeSpan ts = now - fi.CreationTime;

					if (ts.Days > this._uiSetting.UserDefinedData.ReportTempFileKeepDays)
					{
						MPIFile.DeleteFile(fi.FullName);
					}
				}
			}
			catch(Exception e)
			{
				Console.WriteLine("[ReportBase], ClearTempFile(), " + e.ToString());
			}
		}

		private void ResetResultData()
		{
			this._resultData = new Dictionary<string, TestResultData>();

			if (this._product.TestCondition != null &&
				this._product.TestCondition.TestItemArray != null &&
				this._product.TestCondition.TestItemArray.Length > 0)
			{
				foreach (var testItem in this._product.TestCondition.TestItemArray)
				{
					if (testItem.MsrtResult == null || testItem.MsrtResult.Length == 0 || !testItem.IsEnable)
					{
						continue;
					}

					foreach (var msrtItem in testItem.MsrtResult)
					{
						if (!msrtItem.IsEnable || !msrtItem.IsVision)
						{
							continue;
						}

						this._resultData.Add(msrtItem.KeyName, msrtItem);
					}
				}
			}
		}

		private void OpenFile()
		{
			this.CloseFile();

            this.ReLoadTmpFile();

			if (!Directory.Exists(Constants.Paths.MPI_TEMP_DIR2))
			{
				Directory.CreateDirectory(Constants.Paths.MPI_TEMP_DIR2);
			}

			string fileName = this.TestResultFileNameWithoutExt();

			if (this._testerSetting.IsPresampling && this._uiSetting.Barcode.Length > 8)
			{
				string lot = this._uiSetting.Barcode.Substring(0, 8);

				string wafer = this._uiSetting.Barcode.Replace(lot, string.Empty);

				fileName = "S" + wafer;
			}

			this._fileFullNameTmp = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, fileName + ReportBase.EXTEN_TMP1);

			this._fileFullNameTmp2 = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, fileName + ReportBase.EXTEN_TMP2);

			this._fileFullNameTmp3 = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, fileName + ReportBase.EXTEN_TMP3);

            //this._fileFullNameTmpS01 = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, fileName + ReportBase.EXTEN_TMPS1);

            //this._fileFullNameTmpS02 = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, fileName + ReportBase.EXTEN_TMPS2);

            this._fileFullNameTmpS03 = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, fileName + ReportBase.EXTEN_TMPS3);

            this._fileFullNameTmpS04 = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, fileName + ReportBase.EXTEN_TMPS4);

            this._fileFullNameTmpSpR = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, fileName + ReportBase.EXTEN_TMPSPR);

            this._fileFullNameTmpSpA = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, fileName + ReportBase.EXTEN_TMPSPA);

			this._fileFullNameRep = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, fileName + ReportBase.EXTEN_REP1);

			this._fileFullNameRep2 = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, fileName + ReportBase.EXTEN_REP2);

			this._fileFullNameRep3 = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, fileName + ReportBase.EXTEN_REP3);

            //this._fileFullNameRepS01 = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, fileName + ReportBase.EXTEN_REPS1);

            //this._fileFullNameRepS02 = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, fileName + ReportBase.EXTEN_REPS2);

            this._fileFullNameRepS03 = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, fileName + ReportBase.EXTEN_REPS3);

            this._fileFullNameRepS04 = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, fileName + ReportBase.EXTEN_REPS4);

            this._fileFullNameRepSpR = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, fileName + ReportBase.EXTEN_REPSPR);

            this._fileFullNameRepSpA = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, fileName + ReportBase.EXTEN_REPSPA);

			this._findChannel1ColRow.Clear();

			this._isAppend = false;

			//==========================
			// OverWrite 檔案就不存在
			// 如果是Append檔案存在，就會執行下面Function
			//==========================				
			if (File.Exists(this._fileFullNameTmp))
			{
				if (this.TesterSetting.IsPresampling)
				{
					this.DeleteTempFile(false, true, true);
				}
				else
				{
					int lastCol = 0;

					int lastRow = 0;

					if (this.LoadTempFileData(out lastCol, out lastRow))
					{
						this._isAppend = true;
					}
					else
					{
						this.DeleteTempFile(false, true, true);
					}
				}
			}



			this._sw = new StreamWriter(this._fileFullNameTmp, true, this._reportData.Encoding);

			this._sw2 = new StreamWriter(this._fileFullNameTmp2, true, this._reportData.Encoding);

			this._sw3 = new StreamWriter(this._fileFullNameTmp3, true, this._reportData.Encoding);

            //this._swS01 = new StreamWriter(this._fileFullNameTmpS01, true, this._reportData.Encoding);

            //this._swS02 = new StreamWriter(this._fileFullNameTmpS02, true, this._reportData.Encoding);

            this._swS03 = new StreamWriter(this._fileFullNameTmpS03, true, this._reportData.Encoding);

            this._swSpR = new StreamWriter(this._fileFullNameTmpSpR, true, this._reportData.Encoding);

            this._swSpA = new StreamWriter(this._fileFullNameTmpSpA, true, this._reportData.Encoding);
		}

        protected virtual void ReLoadTmpFile()
        {
            ///////////////////////////////////////////////////////
            //Replace Data And Check Row Col
            ///////////////////////////////////////////////////////
            if (this._fileFullNameTmp == null || this._fileFullNameTmp == "")
            {
                string fileName = this.TestResultFileNameWithoutExt();

                if (this._testerSetting.IsPresampling && this._uiSetting.Barcode.Length > 8)
                {
                    string lot = this._uiSetting.Barcode.Substring(0, 8);

                    string wafer = this._uiSetting.Barcode.Replace(lot, string.Empty);

                    fileName = "S" + wafer;
                }

                this._fileFullNameTmp = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, fileName + ReportBase.EXTEN_TMP1);

            }

            if (!File.Exists(this._fileFullNameTmp))
            {
                string fileName = this.TestResultFileNameWithoutExt();

                return;
            }

            try
            {
                using (StreamReader srCheckRowCol = new StreamReader(this._fileFullNameTmp, this._reportData.Encoding))
                {
                    bool isRawData = false;

                    int rawLineCount = 0;

                    this._checkColRowKey.Clear();

                    HeaderFinder hf = new HeaderFinder(this.TitleStrKey, TitleStrShift);

                    //重繞tmp檔取得 ColRowkey 對應的第幾筆數據
                    while (srCheckRowCol.Peek() >= 0)
                    {
                        string line = srCheckRowCol.ReadLine();

                        if (isRawData)
                        {
                            rawLineCount++;

                            string[] rawData = line.Split(this.SpiltChar);

                            string colrowKey = ColRowKeyMaker(rawData);

                            if (this._checkColRowKey.ContainsKey(colrowKey))
                            {
                                this._checkColRowKey[colrowKey] = rawLineCount;
                            }
                            else
                            {
                                this._checkColRowKey.Add(colrowKey, rawLineCount);
                            }
                        }
                        else
                        {
                            //if (line == this.ResultTitleInfo.TitleStr.Replace(",", this.SpiltChar.ToString()))
                            if (hf.CheckIfRowData(line))
                            {
                                isRawData = true;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                File.Delete(this._fileFullNameTmp);

                string fileName = this.TestResultFileNameWithoutExt();

                this._checkColRowKey.Clear();

                return;
            }
        }

        protected string ColRowKeyMaker(string[] rawData)
        {
            if (_crKeyMaker == null)
            {
                List<int> colList = new List<int>();
                colList.Add(this._resultTitleInfo.ColIndex);
                colList.Add(this._resultTitleInfo.RowIndex);
                if (this._resultTitleInfo.ChipIndexIndex >= 0)
                {
                    colList.Add(this._resultTitleInfo.ChipIndexIndex);
                }
                _crKeyMaker = new PosKeyMakerBase(colList);
            }

            return _crKeyMaker.GetPosKey(rawData);
        }

        // Roy++
		private void DeleteTempFile(bool isBackupTmp, bool isDeleteTmp, bool isDeleteRep)
		{
			if (isDeleteTmp)
			{
				string backupFullName = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "Backup.csv");

				string backupFullName2 = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "Backup2.csv");

				string backupFullName3 = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "Backup3.csv");

				MPIFile.CopyFile(this._fileFullNameRep, backupFullName);

				MPIFile.CopyFile(this._fileFullNameRep2, backupFullName2);

				MPIFile.CopyFile(this._fileFullNameRep3, backupFullName3);

                string backupFullNameS01 = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "BackupS01.csv");

                string backupFullNameS02 = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "BackupS02.csv");

                string backupFullNameS03 = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "BackupS03.csv");

                string backupFullNameS04 = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "BackupS04.csv");

                string backupFullNameSpR = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "BackupSpR.csv");

                string backupFullNameSpA = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "BackupSpA.csv");

                //MPIFile.CopyFile(this._fileFullNameRepS01, backupFullNameS01);

                //MPIFile.CopyFile(this._fileFullNameRepS02, backupFullNameS02);

                MPIFile.CopyFile(this._fileFullNameRepS03, backupFullNameS03);

                MPIFile.CopyFile(this._fileFullNameRepS04, backupFullNameS04);

                MPIFile.CopyFile(this._fileFullNameRepSpR, backupFullNameSpR);

                MPIFile.CopyFile(this._fileFullNameRepSpA, backupFullNameSpA);
			}

			if (isDeleteRep)
			{
				MPIFile.DeleteFile(this._fileFullNameRep);

				MPIFile.DeleteFile(this._fileFullNameRep2);

				MPIFile.DeleteFile(this._fileFullNameRep3);

                //MPIFile.DeleteFile(this._fileFullNameRepS01);

                //MPIFile.DeleteFile(this._fileFullNameRepS02);

                MPIFile.DeleteFile(this._fileFullNameRepS03);

                MPIFile.DeleteFile(this._fileFullNameRepS04);

                MPIFile.DeleteFile(this._fileFullNameRepSpR);

                MPIFile.DeleteFile(this._fileFullNameRepSpA);
			}

			if (isDeleteTmp)
			{
                //if (this._fileFullNameTmp != null)
                //{
                //    File.Copy(this._fileFullNameTmp, this._fileFullNameTmp + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                //}

				MPIFile.DeleteFile(this._fileFullNameTmp);

				MPIFile.DeleteFile(this._fileFullNameTmp2);

				MPIFile.DeleteFile(this._fileFullNameTmp3);

                //MPIFile.DeleteFile(this._fileFullNameTmpS01);

                //MPIFile.DeleteFile(this._fileFullNameTmpS02);

                MPIFile.DeleteFile(this._fileFullNameTmpS03);

                MPIFile.DeleteFile(this._fileFullNameTmpS04);

                MPIFile.DeleteFile(this._fileFullNameTmpSpR);

                MPIFile.DeleteFile(this._fileFullNameTmpSpA);
			}
		}		

		private void RecordColRow(int test, int col, int row, int ch)
		{
			string key = col.ToString() + "_" + row.ToString();

			if(ch > 0)
			{
				if(ch == 1)
				{
					this._lastColRow = new ColRow(col, row);
				}

				if (this._findChannel1ColRow.ContainsKey(key))
				{
					this._findChannel1ColRow[key] = this._lastColRow;
				}
				else
				{
					this._findChannel1ColRow.Add(key, this._lastColRow);
				}
			}
		}

		private void RecordColRow(Dictionary<string, double> data)
		{
			int col = (int)data[EProberDataIndex.COL.ToString()];

			int row = (int)data[EProberDataIndex.ROW.ToString()];

			data[ESysResultItem.TEST.ToString()] += this._appendFileTestCount;

			int test = (int)data[ESysResultItem.TEST.ToString()];

			int ch = (int)data[ESysResultItem.CHANNEL.ToString()];

			this.RecordColRow(test, col, row, ch);
		}

		private void ReStatistic(string[] rawData, Dictionary<string, double> data)
		{
			int index = 0;

			foreach (var item in this._resultTitleInfo)
			{
				double value = 0.0d;

				if (double.TryParse(rawData[index], out value))
				{
					data[item.Key] = value;
				}

				index++;
			}

			ReportBase._statisticSet.Push(data);
		}

		protected virtual void ReplaceReportData(Dictionary<string, string> replaceData, string inputFile, string outputFile, bool isSkipWritingTestCount = false)
		{
			///////////////////////////////////////////////////////
			//Set Statistic Data
			///////////////////////////////////////////////////////
			Dictionary<string, double> data = new Dictionary<string, double>();

			if (this._isReStatistic)
			{
				ReportBase._statisticSet.ResetStatisticData();

				foreach (string keyName in Enum.GetNames(typeof(ESysResultItem)))
				{
					data.Add(keyName, 0);
				}

				for (int i = 0; i < this._resultTitleInfo.ResultCount; i++)
				{
					string keyName = string.Empty;

					int index = 0;

					foreach (var item in this._resultTitleInfo)
					{
						if (index == i)
						{
							keyName = item.Key;
						}

						index++;
					}

					if (!data.ContainsKey(keyName))
					{
						data.Add(keyName, 0);
					}
				}
			}

			///////////////////////////////////////////////////////
			//Replace Data And Check Row Col
			///////////////////////////////////////////////////////
            if (outputFile == string.Empty)
            {
                return;
            }

			StreamWriter sw = new StreamWriter(outputFile, false, this._reportData.Encoding);

			StreamReader sr = new StreamReader(inputFile, this._reportData.Encoding);

			bool isRawData = false;

			int rawLineCount = 0;

			int testCount = this._uiSetting.UserDefinedData.TestStartIndex;

            int shiftCount = TitleStrShift;

            HeaderFinder hf = new HeaderFinder(this.TitleStrKey, TitleStrShift);
            // 開始比對ColRowKey並寫檔
			while (sr.Peek() >= 0)
			{
				string line = sr.ReadLine();

				if (isRawData)
				{
					if (this._testerSetting.IsCheckRowCol)
					{
						rawLineCount++;

						string[] rawData = line.Split(this.SpiltChar);

                        string colrowKey = ColRowKeyMaker(rawData);
                        
                         // 把 row.col 和 checkRowCol "raw line count " 相同時, 才會push資料,解決當點重測row,col的問題
                         if (this._checkColRowKey.ContainsKey(colrowKey) && this._checkColRowKey[colrowKey] == rawLineCount)
						{
							// Check Col Row And ReStatist
							if (this._isReStatistic)
							{
								this.ReStatistic(rawData, data);
							}

							//Rewrite TEST
							if (this._resultTitleInfo.TestIndex >= 0)
							{
								rawData[this._resultTitleInfo.TestIndex] = testCount.ToString();

								line = string.Empty;

								for (int i = 0; i < rawData.Length; i++)
								{
                                    if (isSkipWritingTestCount)
                                    {
                                        if (this._resultTitleInfo.TestIndex != i)
                                        {
                                            line += rawData[i];
                                        }
                                    }
                                    else
                                    {
									line += rawData[i];
                                    }

									if (i != rawData.Length - 1)
									{
                                        line += this.SpiltChar;
									}
								}
							}

							testCount++;
						}
						else
						{
							continue;
						}
					}
					else
					{
						// Don't Check Col Row But ReStatistic
						if (this._isReStatistic)
						{
                            string[] rawData = line.Split(this.SpiltChar);

							this.ReStatistic(rawData, data);
						}
					}
				}
				else
				{
                    if (hf.CheckIfRowData(line))
					{
						isRawData = true;
					}
					else
					{
						if (replaceData.ContainsKey(line))
						{
							line = replaceData[line];
						}
					}
				}

				sw.WriteLine(line);
			}

			sr.Close();

			sr.Dispose();

			sw.Close();

			sw.Dispose();
		}

		#endregion

		#region >>> Protected Abstract Method <<<

		protected abstract void SetResultTitle();

		protected abstract EErrorCode WriteReportHeadByUser();

		protected abstract EErrorCode RewriteReportByUser();
	
		#endregion

		#region >>> Protected Virtual Method <<<

		protected virtual bool LoadTempFileData(out int lastCol, out int lastRow)
		{
			lastCol = int.MinValue;

			lastRow = int.MinValue;

			if (!this.IsTempFileExist)
			{
				return false;
			}

			bool isRawData = false;

			try
			{
				///////////////////////////////////////////////////////
				//Set Statistic Data
				///////////////////////////////////////////////////////
				Dictionary<string, double> data = new Dictionary<string, double>();

				if (this._isReStatistic)
				{
					ReportBase._statisticSet.ResetStatisticData();

					foreach (string keyName in Enum.GetNames(typeof(ESysResultItem)))
					{
						data.Add(keyName, 0);
					}

					for (int i = 0; i < this._resultTitleInfo.ResultCount; i++)
					{
						string keyName = string.Empty;

						int index = 0;

						foreach (var item in this._resultTitleInfo)
						{
							if (index == i)
							{
								keyName = item.Key;
							}

							index++;
						}

						if (!data.ContainsKey(keyName))
						{
							data.Add(keyName, 0);
						}
					}
				}

				///////////////////////////////////////////////////////
				//Load File
				///////////////////////////////////////////////////////
				string tmp = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, this.TestResultFileNameWithoutExt() + ReportBase.EXTEN_TMP1);

				this._appendFileTestCount = 0;

				this._checkColRowKey.Clear();
                HeaderFinder hf = new HeaderFinder(this.TitleStrKey, TitleStrShift);

				using (StreamReader sr = new StreamReader(tmp, this._reportData.Encoding))
				{
					while (sr.Peek() >= 0)
					{
						string line = sr.ReadLine();

						if (isRawData)
						{
							this._appendFileTestCount++;

							string[] item = line.Split(',');

							int col = int.Parse(item[this._resultTitleInfo.ColIndex]);

							int row = int.Parse(item[this._resultTitleInfo.RowIndex]);

							int test = int.Parse(item[this._resultTitleInfo.TestIndex]);

							//this.ChangeRowColCoord(ref col, ref row);

							int ch = 0;

							if (this._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Die)
							{
								if (this._resultTitleInfo.CHIndex > 0)
								{
									ch = (int)double.Parse(item[this._resultTitleInfo.CHIndex]);

									if (ch == 1)
									{
										lastCol = col;

										lastRow = row;
									}
								}

								this.RecordColRow(test, col, row, ch);
							}
							else
							{
								lastCol = col;

								lastRow = row;

								this.RecordColRow(test, col, row, 1);
							}

							if (this._isReStatistic)
							{
								string[] rawData = line.Split(this.SpiltChar);

								this.ReStatistic(rawData, data);
							}
						}
						else
						{
                            if (hf.CheckIfRowData(line))
							{
								isRawData = true;
							}
						}
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("[ReportBase], LoadTempFileData, catch:" + e.ToString());
			}

			if (isRawData && this._appendFileTestCount != 0)
			{
				return true;
			}
			else
			{
				this._appendFileTestCount = 0;

				return false;
			}
		}

		protected virtual EErrorCode ReportInterpolation()
		{
			try
			{
				if (!this._uiSetting.UserDefinedData.IsShowReportInterpolation || !this.TesterSetting.IsEnableReportInterpolation)
				{
					return EErrorCode.NONE;
				}

				if (this.FileFullNameRep == "")
				{
					Console.WriteLine("[ReportBase], ReportReportInterpolation(), File is not exist:" + this.FileFullNameRep);

					return EErrorCode.NONE;
				}

				//--------------------------------------------------------------
				// Backup Report
				//--------------------------------------------------------------
				string fileName = Path.GetFileNameWithoutExtension(this.FileFullNameRep) + ".csv";

				string backupFileFullName = Path.Combine(this.UISetting.RIBackupFilePath, fileName);

				if (this.UISetting.IsEnableRIBackupFilePath)
				{
					MPIFile.CopyFile(this.FileFullNameRep, backupFileFullName);
				}

				//--------------------------------------------------------------
				// Combine Report
				//--------------------------------------------------------------
				string tmpFileFullName = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "tmp.csv");

				if (this.UISetting.RILoadReportMode != ERILoadReportMode.None)
				{
					string loadFileFullName = Path.Combine(this.UISetting.RILoadReportFilePath, fileName);

					if (File.Exists(loadFileFullName))
					{
						string localTmpFileFullName = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "loadTmp.csv");

						MPIFile.DeleteFile(localTmpFileFullName);

						MPIFile.CopyFile(loadFileFullName, localTmpFileFullName);

						string optiReport = string.Empty;

						string elecReport = string.Empty;

						if (this.UISetting.RILoadReportMode == ERILoadReportMode.LoadElecReport)
						{
							optiReport = this.FileFullNameRep;

							elecReport = localTmpFileFullName;
						}
						else if (this.UISetting.RILoadReportMode == ERILoadReportMode.LoadOptiReport)
						{
							optiReport = localTmpFileFullName;

							elecReport = this.FileFullNameRep;
						}

						if (this._optiReportData.LoadOptiFileData(optiReport))
						{
							using (StreamReader sr = new StreamReader(elecReport, Encoding.Default))
							{
								using (StreamWriter sw = new StreamWriter(tmpFileFullName, false, Encoding.Default))
								{
									bool isRawData = false;

									while (sr.Peek() >= 0)
									{
										string line = sr.ReadLine();

										if (isRawData)
										{
											string[] items = line.Split(',');

											line = string.Empty;

											this._optiReportData.WriteOptiData(items);

											for (int i = 0; i < items.Length; i++)
											{
												line += items[i];

												if (i != items.Length - 1)
												{
													line += ",";
												}
											}
										}
										else if (line == this.ResultTitleInfo.TitleStr.Replace(",", this.SpiltChar.ToString()))
										{
											isRawData = true;
										}

										sw.WriteLine(line);
									}
								}
							}
						}

						MPIFile.DeleteFile(this.FileFullNameRep);

						MPIFile.CopyFile(tmpFileFullName, this.FileFullNameRep);

						MPIFile.DeleteFile(tmpFileFullName);

					}
					else
					{
						Console.WriteLine("[ReportBase], ReportReportInterpolation(), File is not exist:" + loadFileFullName);
					}
				}

				//--------------------------------------------------------------
				// Report Interpolation
				//--------------------------------------------------------------
                if (this.RICalcMath.ClacProcess(this.UISetting, this.TesterSetting, this.Product, this.FileFullNameRep, tmpFileFullName))
                {
                    MPIFile.DeleteFile(this.FileFullNameRep);

                    MPIFile.CopyFile(tmpFileFullName, this.FileFullNameRep);

                    MPIFile.DeleteFile(tmpFileFullName);
                }
                else
                {
                    return EErrorCode.SaveFileFail;
                }

				//--------------------------------------------------------------
				// ReStatistic
				//--------------------------------------------------------------
				if (!File.Exists(this.UISetting.RIReportSpecFile))
				{
					return EErrorCode.NONE;
				}

				Dictionary<string, double> data = new Dictionary<string, double>();

				foreach (string keyName in Enum.GetNames(typeof(ESysResultItem)))
				{
					data.Add(keyName, 0.0d);
				}

				if (this.Product != null && this.Product.TestCondition != null && this.Product.TestCondition.TestItemArray != null)
				{
					foreach (var testItem in this.Product.TestCondition.TestItemArray)
					{
						if (!testItem.IsEnable || testItem.MsrtResult == null || testItem.MsrtResult.Length == 0)
						{
							continue;
						}

						foreach (var result in testItem.MsrtResult)
						{
							data.Add(result.KeyName, 0.0d);
						}
					}
				}

				string[] keyNameList = new string[this.ResultTitleInfo.ResultCount];

				int index = 0;

				foreach (var item in this.ResultTitleInfo)
				{
					keyNameList[index] = item.Key;

					index++;
				}

				using (StreamReader sr = new StreamReader(FileFullNameRep, Encoding.Default))
				{
					bool isRawData = false;

					while (sr.Peek() >= 0)
					{
						string line = sr.ReadLine();

						if (isRawData)
						{
							string[] items = line.Split(',');

							for (int i = 0; i < keyNameList.Length; i++)
							{
								string keyName = keyNameList[i];

								double value = 0.0d;

								double.TryParse(items[i], out value);

								data[keyName] = value;
							}

							ReportBase.PushStatisticData(data);
						}
                        else if (line == this.ResultTitleInfo.TitleStr.Replace(",", this.SpiltChar.ToString()))
						{
							isRawData = true;

							TestResultData[] results = this.GetSpecData(this.UISetting.RIReportSpecFile, this.UISetting.FormatName, this.UISetting.TaskSheetFileName);

							ReportBase.ResetStatisticData(results);
						}
					}
				}

				return EErrorCode.NONE;
			}
			catch (Exception e)
			{
				Console.WriteLine("[ReportBase], ReportReportInterpolation(), catch: " + e.ToString());

				return EErrorCode.SaveFileFail;
			}
		}

		protected virtual TestResultData[] GetSpecData(string fileFullName, string format, string recipeName)
		{
			try
			{
				// for 6037
				List<TestResultData> dataList = new List<TestResultData>();

				Dictionary<string, string> itemNameToKey = new Dictionary<string, string>();

				itemNameToKey.Add("VZ1", "MVZ_1");

				itemNameToKey.Add("IR", "MIR_1");

				itemNameToKey.Add("IR2", "MIR_2");

				itemNameToKey.Add("LOP1", "LOP_1");

				itemNameToKey.Add("WLD", "WLD_1");

				itemNameToKey.Add("WLP", "WLP_1");

				itemNameToKey.Add("HW", "HW_1");

				itemNameToKey.Add("CIEX", "CIEx_1");

				itemNameToKey.Add("CIEY", "CIEy_1");

				itemNameToKey.Add("Purity", "PURITY_1");

				itemNameToKey.Add("LOP3", "LOP_3");

				string localFullFile = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "Spec.csv");

				if (File.Exists(localFullFile))
				{
					MPIFile.DeleteFile(localFullFile);
				}

				MPIFile.CopyFile(fileFullName, localFullFile);

				List<string[]> file = CSVUtil.ReadCSV(localFullFile);

				if (file.Count < 2 || file[0].Length < 2 || file[0][0] != "Recipe")
				{
					return null;
				}

				TestResultData wld = new TestResultData();

				for (int i = 1; i < file.Count; i++)
				{
					if (file[i][0] != recipeName)
					{
						continue;
					}

					for (int j = 1; j < file[i].Length; j++)
					{
						TestResultData item = new TestResultData();

						if (file[0][j] == "VF1")
						{
							if (format == "Format-Single")
							{
								item.KeyName = "MVFLA_1";
							}
							else
							{
								item.KeyName = "MVF_1";
							}
						}
						else if (file[0][j] == "VF2")
						{
							if (format == "Format-Single")
							{
								item.KeyName = "MVF_1";
							}
							else
							{
								item.KeyName = "MVF_2";
							}
						}
						else if (file[0][j] == "VF3")
						{
							if (format == "Format-Single")
							{
								item.KeyName = "MVF_2";
							}
							else
							{
								item.KeyName = "MVF_3";
							}
						}
						else if (file[0][j] == "WLD")
						{
							wld.KeyName = "WLD_1";

							string[] value = file[i][j].Split(';');

							double min = 0.0d;

							double max = 0.0d;

							if (double.TryParse(value[0], out min) && double.TryParse(value[1], out max))
							{
								wld.MinLimitValue = min;

								wld.MaxLimitValue = max;
							}
						}
						else if (file[0][j] == "WD (6nm)")
						{
							wld.KeyName = "WLD_1";

							string[] value = file[i][j].Split(';');

							double min = 0.0d;

							double max = 0.0d;

							if (double.TryParse(value[0], out min) && double.TryParse(value[1], out max))
							{
								wld.MinLimitValue2 = min;

								wld.MaxLimitValue2 = max;
							}
						}
						else if (file[0][j] == "WD (10nm)")
						{
							wld.KeyName = "WLD_1";

							string[] value = file[i][j].Split(';');

							double min = 0.0d;

							double max = 0.0d;

							if (double.TryParse(value[0], out min) && double.TryParse(value[1], out max))
							{
								wld.MinLimitValue3 = min;

								wld.MaxLimitValue3 = max;
							}
						}
						else if (itemNameToKey.ContainsKey(file[0][j]))
						{
							item.KeyName = itemNameToKey[file[0][j]];
						}

						if (item.KeyName != "")
						{
							string[] value = file[i][j].Split(';');

							double min = 0.0d;

							double max = 0.0d;

							if (double.TryParse(value[0], out min) && double.TryParse(value[1], out max))
							{
								item.IsEnable = true;

								item.IsVerify = true;

								item.IsVision = true;

								item.MinLimitValue = min;

								item.MaxLimitValue = max;

								dataList.Add(item);
							}
						}
					}
				}

				if (wld.KeyName != "")
				{
					wld.IsEnable = true;

					wld.IsVerify = true;

					wld.IsVision = true;

					dataList.Add(wld);
				}

				return dataList.ToArray();
			}
			catch
			{
				return null;
			}
		}

        
 		protected virtual EErrorCode AddReportCode(string filePath)
        {
            return EErrorCode.NONE;
        }

		#endregion

		#region >>> Protected Method <<<

		

		#endregion

		#region >>> Public Virtual Method <<<

		public virtual string TestResultFileNameWithoutExt()
		{
			char[] invalidFileChars = Path.GetInvalidFileNameChars();

			foreach(var chr in invalidFileChars)
			{
				if (this._uiSetting.TestResultFileName.Contains(chr))
				{
                    this._uiSetting.TestResultFileName = this._uiSetting.TestResultFileName.Replace(chr.ToString(), "");					
				}
			}

            if(this._uiSetting.TestResultFileName == "")
            {
                return DateTime.Now.ToString("yyMMddhhmmss");
            }

			return this._uiSetting.TestResultFileName;
		}

		public virtual bool IsOutputReportExist()
		{
			bool exist = false;

			string outPath01 = string.Empty;

			string outPath02 = string.Empty;

			string outPath03 = string.Empty;

			string fileNameWithExt = this.UISetting.TestResultFileName + "." + this.UISetting.TestResultFileExt;

			if (this.UISetting.IsManualRunMode)
			{
				outPath01 = this.UISetting.ManualOutputPath01;

				outPath02 = this.UISetting.ManualOutputPath02;

				outPath03 = this.UISetting.ManualOutputPath03;

                outPath01 = GetPathWithFolder(outPath01, this.UISetting.ManualOutputPathType01);

                outPath02 = GetPathWithFolder(outPath02, this.UISetting.ManualOutputPathType02);

                outPath03 = GetPathWithFolder(outPath03, this.UISetting.ManualOutputPathType03);


				outPath01 = Path.Combine(outPath01, fileNameWithExt);

				outPath02 = Path.Combine(outPath02, fileNameWithExt);

				outPath03 = Path.Combine(outPath03, fileNameWithExt);

				if (File.Exists(outPath01) && this.UISetting.IsEnableManualPath01)
				{
					exist = true;
				}

				if (this.UISetting.IsRunDailyCheckMode)
				{

					exist = false;
				}
			}
			else
			{
				outPath01 = this.UISetting.TestResultPath01;

				outPath02 = this.UISetting.TestResultPath02;

				outPath03 = this.UISetting.TestResultPath03;

                outPath01 = GetPathWithFolder(outPath01, this.UISetting.TesterResultCreatFolderType01);

                outPath02 = GetPathWithFolder(outPath02, this.UISetting.TesterResultCreatFolderType02);

                outPath03 = GetPathWithFolder(outPath03, this.UISetting.TesterResultCreatFolderType03);

				outPath01 = Path.Combine(outPath01, fileNameWithExt);

				outPath02 = Path.Combine(outPath02, fileNameWithExt);

				outPath03 = Path.Combine(outPath03, fileNameWithExt);

				if (File.Exists(outPath01) && this.UISetting.IsEnablePath01)
				{
					exist = true;
				}
				else if (File.Exists(outPath02) && this.UISetting.IsEnablePath02)
				{
					exist = true;
				}
				else if (File.Exists(outPath03) && this.UISetting.IsEnablePath03)
				{
					exist = true;
				}
			}

			return exist;
		}

        public virtual EErrorCode CombineSimulatorReportInterpolation(string optiReport, string elecReport)
        {
            //-----------------------------------------------------
            // Backup RI Info
            //-----------------------------------------------------
            bool tmpIsBackup = this.UISetting.IsEnableRIBackupFilePath;

            ERILoadReportMode tmpRILoadReportMode = this.UISetting.RILoadReportMode;

            string tmpLoadFileFullPath = this.UISetting.RILoadReportFilePath;

            string tmpRepFile = this.FileFullNameRep;

            //-----------------------------------------------------
            // Change Info and copy opti/elec report
            //-----------------------------------------------------
            this.UISetting.IsEnableRIBackupFilePath = false;

            this.UISetting.RILoadReportMode = ERILoadReportMode.LoadOptiReport;

            string optiTempFile = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "siTmp.csv");

            this.UISetting.RILoadReportFilePath = Constants.Paths.MPI_TEMP_DIR2;

            this.FileFullNameRep = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "siTmp" + ReportBase.EXTEN_REP1);

            this.FileFullNameRep2 = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "siStatiTmp" + ReportBase.EXTEN_REP2);

            //-----------------------------------------------------
            // RI and move file
            //-----------------------------------------------------
            EErrorCode err = EErrorCode.NONE;

            if (MPIFile.CopyFile(optiReport, optiTempFile) && MPIFile.CopyFile(elecReport, this.FileFullNameRep))
            {
                err = this.ReportInterpolation();

                if (err == EErrorCode.NONE)
                {
                    err = this.MoveFileToTargetByUser(EServerQueryCmd.CMD_TESTER_END);
                }
            }
            else
            {
                err = EErrorCode.SaveFileFail;
            }

            //-----------------------------------------------------
            // Load RI Info
            //-----------------------------------------------------
            this.UISetting.IsEnableRIBackupFilePath = tmpIsBackup;

            this.UISetting.RILoadReportMode = tmpRILoadReportMode;

            this.UISetting.RILoadReportFilePath = tmpLoadFileFullPath;

            this.FileFullNameRep = tmpRepFile;

            return err;
        }

        public virtual EErrorCode SimulatorReportInterpolation(string report)
        {
            //-----------------------------------------------------
            // Backup RI Info
            //-----------------------------------------------------
            bool tmpIsBackup = this.UISetting.IsEnableRIBackupFilePath;

            ERILoadReportMode tmpRILoadReportMode = this.UISetting.RILoadReportMode;

            string tmpLoadFileFullPath = this.UISetting.RILoadReportFilePath;

            string tmpRepFile = this.FileFullNameRep;

            //-----------------------------------------------------
            // Change Info and copy opti/elec report
            //-----------------------------------------------------
            this.UISetting.IsEnableRIBackupFilePath = false;

            this.UISetting.RILoadReportMode = ERILoadReportMode.None;

            this.UISetting.RILoadReportFilePath = Constants.Paths.MPI_TEMP_DIR2;

            this.FileFullNameRep = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "siTmp" + ReportBase.EXTEN_REP1);

            this.FileFullNameRep2 = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "siStatiTmp" + ReportBase.EXTEN_REP2);

            //-----------------------------------------------------
            // RI and move file
            //-----------------------------------------------------
            EErrorCode err = EErrorCode.NONE;

            if (MPIFile.CopyFile(report, this.FileFullNameRep))
            {
                err = this.ReportInterpolation();

                if (err == EErrorCode.NONE)
                {
                    err = this.MoveFileToTargetByUser(EServerQueryCmd.CMD_TESTER_END);
                }
            }
            else
            {
                err = EErrorCode.SaveFileFail;
            }

            //-----------------------------------------------------
            // Load RI Info
            //-----------------------------------------------------
            this.UISetting.IsEnableRIBackupFilePath = tmpIsBackup;

            this.UISetting.RILoadReportMode = tmpRILoadReportMode;

            this.UISetting.RILoadReportFilePath = tmpLoadFileFullPath;

            this.FileFullNameRep = tmpRepFile;

            return err;
        }

        public virtual string GetOutputFileName()
        {
            TesterSetting.StartTestTime = DateTime.Now;

            UISetting.TestResultFileName = "";

            switch (UISetting.FileNameFormatPresent)
            {
                case (int)EOutputFileNamePresent.WaferNum:
                    UISetting.TestResultFileName = UISetting.WaferNumber;
                    break;
                //-------------------------------------------------------------------------
                case (int)EOutputFileNamePresent.BarCode:
                    UISetting.TestResultFileName = UISetting.Barcode;
                    break;
                //-------------------------------------------------------------------------
                case (int)EOutputFileNamePresent.LotSpaceWafer:
                    UISetting.TestResultFileName = UISetting.LotNumber + " " + UISetting.WaferNumber;
                    break;
                //-------------------------------------------------------------------------
                case (int)EOutputFileNamePresent.LotNum_WaferNum:
                    UISetting.TestResultFileName = UISetting.LotNumber + "_" + UISetting.WaferNumber;
                    break;
                //-------------------------------------------------------------------------
                case (int)EOutputFileNamePresent.WaferNum_Stage:
                    UISetting.TestResultFileName = UISetting.WaferNumber;
                    if (this.Product.TestCondition != null)
                    {
                        UISetting.TestResultFileName += "_" + this.Product.TestCondition.TestStage.ToString();
                    }
                    break;
                //-------------------------------------------------------------------------
                case (int)EOutputFileNamePresent.Customer01:
                    UISetting.TestResultFileName = ParseOutputFileName(1);
                    break;
                //-------------------------------------------------------------------------
                case (int)EOutputFileNamePresent.Customer02:
                    UISetting.TestResultFileName = ParseOutputFileName(2);
                    break;
                //-------------------------------------------------------------------------
                default:
                    UISetting.TestResultFileName = UISetting.Barcode;
                    break;
            }

            UISetting.TestResultFileName = UISetting.TestResultFileName ;

            if (UISetting.UIDisplayType == (int)EUIDisplayType.WMStartUI)
            {
                UISetting.TestResultFileName = UISetting.WeiminUIData.KeyInFileName;
            }

            return UISetting.TestResultFileName;
        }   

        public virtual string GetFullFileNameWithPathAndExt(int fileNum = 1)
        {
            string fileFullName = "";
            string path = GetFilePath(fileNum);
            string fileName = this.TestResultFileName + "." + UISetting.TestResultFileExt;
            //s
            fileFullName = Path.Combine(path, fileName);
            return fileFullName;
        }

        public virtual EErrorCode CloneFileToTemp()
        {
            EErrorCode error = EErrorCode.NONE;

            error = CloneCsvReportToTemp();

            if (error != EErrorCode.NONE)
            {
                return error;
            }

            error = CloneCsvReport2ToTemp();

            if (error != EErrorCode.NONE)
            {
                return error;
            }

            error = CloneCsvReport3ToTemp();

            if (error != EErrorCode.NONE)
            {
                return error;
            }

            return error;
        }

        public virtual EErrorCode CloneCsvReportToTemp()
        {
            //_isRetest = true;
            string tmp = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, this.TestResultFileNameWithoutExt() + ReportBase.EXTEN_TMP1);
            try
            {
                File.Copy(GetFullFileNameWithPathAndExt(), tmp);
            }
            catch (Exception e)
            {
                Console.WriteLine("[REPORT],CloneFileToTemp(),err: " + e.Message);
                return EErrorCode.REPORT_File_To_Temp_Fail;
            }
            return EErrorCode.NONE;
        }

        public virtual EErrorCode CloneCsvReport2ToTemp()
        {
            return EErrorCode.NONE;
        }

        public virtual EErrorCode CloneCsvReport3ToTemp()
        {
            return EErrorCode.NONE;
        }


		#endregion

		#region >>> Public Method <<< 

		public EErrorCode RunCommand(EServerQueryCmd cmd)
		{
			///////////////////////////////////////////////////////////////
			// Open or Append File
			///////////////////////////////////////////////////////////////
			if (cmd == EServerQueryCmd.CMD_OVERWRITE_TESTER_OUTPUT_FILE)
			{
				this.DeleteTempFile(false, true, true);
			}
			else if (cmd == EServerQueryCmd.CMD_APPEND_TESTER_OUTPUT_FILE)
			{
                if (UISetting.IsEnableSaveMap)
                {
                    this.ReLoadTmpFile();

                    this.WriteoutMap(isTempMap:true);
                }
				//this.LoadTempFileData(true);
			}
			else if (cmd == EServerQueryCmd.CMD_TESTER_START)
			{
				this.OpenFile();

				this._testerSetting.StartTestTime = DateTime.Now;
			}
			else if (cmd == EServerQueryCmd.CMD_TESTER_END )
			{
                GlobalFlag.TestMode = ETesterTestMode.Normal;//20180905 David避免測試失敗強迫結束寫檔後還是overload

				this.CloseFile();

				this.ReLoadTmpFile();

                if (UISetting.IsEnableSaveMap && !UISetting.IsManualRunMode )
                {
                    this.WriteoutMap();
                }

				this._testerSetting.EndTestTime = DateTime.Now;
			}
            else if ( cmd == EServerQueryCmd.CMD_TESTER_ABORT)
            {
                GlobalFlag.TestMode = ETesterTestMode.Normal;//20180905 David避免測試失敗強迫結束寫檔後還是overload

                this.CloseFile();

                this.ReLoadTmpFile();

                if (UISetting.IsEnableSaveMap && !UISetting.IsManualRunMode)
                {
                    if (_saveMapAtAbort )
                    {
                        this.WriteoutMap();
                    }
                }

                this._testerSetting.EndTestTime = DateTime.Now;
            }

			///////////////////////////////////////////////////////////////
			// Run Command By User
			///////////////////////////////////////////////////////////////
			EErrorCode code = this.RunCommandByUser(cmd);

			if (code != EErrorCode.NONE)
			{
				return code;
			}

			code = this.RunCommandByUser2(cmd);

			if (code != EErrorCode.NONE)
			{
				return code;
			}

			code = this.RunCommandByUser3(cmd);
            if (code != EErrorCode.NONE)
            {
                return code;
            }

            if (UISetting.IsEnableSweepPath && AcquireData.LIVDataSet != null)
            {
                code = this.RunCommandByUserS01(cmd);
                if (code != EErrorCode.NONE)
                {
                    return code;
                }
            }
			if (UISetting.IsEnableSaveLIVData && AcquireData.LIVDataSet != null)
            {
                code = this.RunCommandByUserS02(cmd);

                if (code != EErrorCode.NONE)
                {
                    return code;
                }
            }

            if (UISetting.IsEnableSaveLIVData && AcquireData.PIVDataSet != null)
            {
                code = this.RunCommandByUserS03(cmd);

                if (code != EErrorCode.NONE)
                {
                    return code;
                }
            }

            if (UISetting.IsEnableSweepPath && AcquireData.ElecSweepDataSet != null)
            {
                code = this.RunCommandByUserS04(cmd);

                if (code != EErrorCode.NONE)
                {
                    return code;
                }
            }
            if (UISetting.IsEnableSaveRelativeSpectrum && AcquireData.SpectrumDataSet != null)
            {
                code = this.RunCommandByUserSpR(cmd);
                if (code != EErrorCode.NONE)
                {
                    return code;
                }
            }
            if (UISetting.IsEnableSaveAbsoluteSpectrum && AcquireData.SpectrumDataSet != null)
            {
                code = this.RunCommandByUserSpA(cmd);
                if (code != EErrorCode.NONE)
                {
                    return code;
                }
            }

            
            
			///////////////////////////////////////////////////////////////
			// Backup File And Clear temp File
			///////////////////////////////////////////////////////////////
			if (cmd == EServerQueryCmd.CMD_TESTER_END)
			{
				this.DeleteTempFile(true, true, true);
			}

			return code;
		}

		public EErrorCode Push(Dictionary<string, double> data,bool IsMultoiStage)
		{
			if(this._sw == null)
			{
				this.OpenFile();

				//當點重測 TEST Index 不會歸零
				this._appendFileTestCount = 0;
			}
			
			this.RecordColRow(data);

            EErrorCode code;

            code = this.PushDataByUser(data);

			if (code != EErrorCode.NONE)
			{
				return code;
			}

			code = this.PushDataByUser2(data);

			if (code != EErrorCode.NONE)
			{
				return code;
			}

            code = this.PushDataByUser3(data);

            if (code != EErrorCode.NONE)
            {
                return code;
            }

            if (this.UISetting.IsEnableSaveLIVData)
            {
                code = this.PushDataByUserS01(data, IsMultoiStage);
            if (code != EErrorCode.NONE)
            {
                return code;
            }
            }

            if (this.UISetting.IsEnableSaveLIVDataPath02)
            {
                code = this.PushDataByUserS02(data, IsMultoiStage);

                if (code != EErrorCode.NONE)
                {
                    return code;
                }
            }

            if (this.UISetting.IsEnableSaveLIVDataPath03)
            {
                code = this.PushDataByUserS03(data, IsMultoiStage);

                if (code != EErrorCode.NONE)
                {
                    return code;
                }
            }

            if (this.UISetting.IsEnableSweepPath)
            {
                code = this.PushDataByUserS04(data, IsMultoiStage);

                if (code != EErrorCode.NONE)
                {
                    return code;
                }
            }

            if (this.UISetting.IsEnableSaveRelativeSpectrum)
            {
                code = this.PushDataByUserSpR(data, IsMultoiStage);

                if (code != EErrorCode.NONE)
                {
                    return code;
                }
            }

            if (this.UISetting.IsEnableSaveAbsoluteSpectrum)
            {
                code = this.PushDataByUserSpA(data, IsMultoiStage);

                if (code != EErrorCode.NONE)
                {
                    return code;
                }
            }

            return EErrorCode.NONE;
		}

		public bool IsCanAppendFile(out int lastCol, out int lastRow)
		{
			return this.LoadTempFileData(out lastCol, out lastRow);
		}

 		public virtual bool OpenReport(string fileFullName)
		{
			try
			{
				if (this._sr != null)
				{
					this._sr.Close();

					this._sr.Dispose();

					this._sr = null;
				}

				this._sr = new StreamReader(fileFullName, this._reportData.Encoding);

				while (this._sr.Peek() >= 0)
				{
					string line = this._sr.ReadLine();

                    if (line == this.ResultTitleInfo.TitleStr.Replace(",", this.SpiltChar.ToString()))
					{
                        this._sr.Close();

                        this._sr.Dispose();

                        this._sr = null;

						return true;
					}
				}

				this._sr.Close();

				this._sr.Dispose();

				this._sr = null;

				return false;
			}
			catch (Exception e)
			{
				Console.WriteLine("[ReportBase], OpenReport, catch:" + e.ToString());

				return false;
			}
		}

        public virtual Dictionary<string, float> ReadLine()
		{
			if (this._sr == null)
			{
				return null;
			}

			try
			{
				if (this._sr.Peek() >= 0)
				{
					string line = this._sr.ReadLine();

					string[] items = line.Split(',');

					int index = 0;

					Dictionary<string, float> data = new Dictionary<string, float>();

					foreach (var item in this._resultTitleInfo)
					{
						float value = 0.0f;

						float.TryParse(items[index], out value);

						data.Add(item.Key, value);

						index++;
					}

					return data;
				}
				else
				{
					this._sr.Close();

					this._sr.Dispose();

					this._sr = null;

					return null;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("[ReportBase], ReadLine, catch:" + e.ToString());

				return null;
			}
		}

		public ColRow GetChannel1ColRow(int col, int row)
		{
			string key = col.ToString() + "_" + row.ToString();

			if(this._findChannel1ColRow == null || !this._findChannel1ColRow.ContainsKey(key))
			{
				return null;
			}
			else
			{
				return this._findChannel1ColRow[key];
			}
		}

        public void CloseFile()
        {
            if (this._sw != null)
            {
                this._sw.Close();

                this._sw.Dispose();

                this._sw = null;
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

            //if (this._swS01 != null)
            //{
            //    this._swS01.Close();

            //    this._swS01.Dispose();

            //    this._swS01 = null;
            //}
            //if (this._swS02 != null)
            //{
            //    this._swS02.Close();

            //    this._swS02.Dispose();

            //    this._swS02 = null;
            //}
            if (this._swS03 != null)
            {
                this._swS03.Close();

                this._swS03.Dispose();

                this._swS03 = null;
            }

            if (this._swSpR != null)
            {
                this._swSpR.Close();

                this._swSpR.Dispose();

                this._swSpR = null;
            }
            if (this._swSpA != null)
            {
                this._swSpA.Close();

                this._swSpA.Dispose();

                this._swSpA = null;
            }
        }

        public string ParseOutputFileName(int index)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();

            string[] fileFormatStrArray = null;

            if (index == 1)
            {
                fileFormatStrArray = UISetting.UserDefinedData.OutputFileNameFormat01;
            }
            else if (index == 2)
            {
                fileFormatStrArray = UISetting.UserDefinedData.OutputFileNameFormat02;
            }
            else
            {
                return "BarCode";
            }

            if (fileFormatStrArray == null)
            {
                return "BarCode";
            }

            foreach (string str in fileFormatStrArray)
            {
                if (str.ToUpper() == "BarCode".ToUpper())
                {
                    sb.Append(UISetting.Barcode);
                    continue;
                }
                else if (str.ToUpper() == "LotNum".ToUpper())
                {
                    sb.Append(UISetting.LotNumber);
                    continue;
                }
                else if (str.ToUpper() == "WaferNum".ToUpper())
                {
                    sb.Append(UISetting.WaferNumber);
                    continue;
                }
                else if (str.ToUpper() == "StartTime".ToUpper())
                {
                    sb.Append(this.TesterSetting.StartTestTime.ToString("yyMMddHHmmss"));
                    continue;
                }
                else if (str.ToUpper() == "MachineName".ToUpper())
                {
                    sb.Append(UISetting.MachineName);
                    continue;
                }
                else if (str.ToUpper() == "OperatorName".ToUpper())
                {
                    sb.Append(UISetting.OperatorName);
                    continue;
                }
                else if (str.ToUpper() == "Substrate".ToUpper())
                {
                    sb.Append(UISetting.Substrate);
                    continue;
                }
                else if (str.ToUpper() == "ProductType".ToUpper())
                {
                    sb.Append(UISetting.ProductType);
                    continue;
                }
                else if (str.ToUpper() == "WMRemark1".ToUpper())
                {
                    sb.Append(UISetting.WeiminUIData.Remark01);
                    continue;
                }
                else if (str.ToUpper() == "WMRemark2".ToUpper())
                {
                    sb.Append(UISetting.WeiminUIData.Remark02);
                    continue;
                }
                else if (str.ToUpper() == "WMRemark3".ToUpper())
                {
                    sb.Append(UISetting.WeiminUIData.Remark03);
                    continue;
                }
                else if (str.ToUpper() == "WMDeviceNumber".ToUpper())
                {
                    sb.Append(UISetting.WeiminUIData.DeviceNumber);
                    continue;
                }
                else if (str.ToUpper() == "WMSpecification".ToUpper())
                {
                    sb.Append(UISetting.WeiminUIData.Specification);
                    continue;
                }
                else
                {
                    sb.Append(str);
                }

            }
            return sb.ToString();
        }

		public string GetFullPath(string path)
        {
            string resultPath = path;

            switch (this.UISetting.FileNameFormatPresent)
            {
                case (int)EOutputFileNamePresent.WaferNum:
                    resultPath = Path.Combine(resultPath, this.UISetting.WaferNumber);
                    break;
                //-------------------------------------------------------------------------
                case (int)EOutputFileNamePresent.BarCode:
                    resultPath = Path.Combine(resultPath, this.UISetting.Barcode);
                    break;
                //-------------------------------------------------------------------------
                case (int)EOutputFileNamePresent.LotSpaceWafer:
                    resultPath = Path.Combine(resultPath, this.UISetting.LotNumber + " " + this.UISetting.WaferNumber);
                    break;
                //-------------------------------------------------------------------------
                case (int)EOutputFileNamePresent.LotNum_WaferNum:
                    resultPath = Path.Combine(resultPath, this.UISetting.LotNumber + "_" + this.UISetting.WaferNumber);
                    break;
                //-------------------------------------------------------------------------
                case (int)EOutputFileNamePresent.Customer01:
                    {
                        string tempStr = ParseOutputFileName(1);
                        resultPath = Path.Combine(resultPath, tempStr);
                    }
                    break;
                //-------------------------------------------------------------------------
                case (int)EOutputFileNamePresent.Customer02:
                    {
                        string tempStr = ParseOutputFileName(2);
                        resultPath = Path.Combine(resultPath, tempStr);
                    }
                    break;
                //-------------------------------------------------------------------------
                default:

                    break;
            }

            return resultPath;
        }

        public string GetFilePath(int pathNum = 1)
        {
            string outPath = string.Empty;

            ETesterResultCreatFolderType type = ETesterResultCreatFolderType.None;

            switch (pathNum)
            {
                default:
                case (1):
                    {
                        outPath = this._uiSetting.TestResultPath01;

                        type = this._uiSetting.TesterResultCreatFolderType01;
                    }
                    break;
                case (2):
                    {
                        outPath = this._uiSetting.TestResultPath02;

                        type = this._uiSetting.TesterResultCreatFolderType02;
                    }
                    break;
                case (3):
                    {
                        outPath = this._uiSetting.TestResultPath03;

                        type = this._uiSetting.TesterResultCreatFolderType03;
                    }
                    break;
            }


            outPath = GetPathWithFolder(outPath, type);

            return outPath;
        }

        public EErrorCode CreateMapFromTemp()
        {
            Console.WriteLine("[REPORT],CreateMapFromTemp()");
            try
            {
                this.ReLoadTmpFile();

                this.WriteoutMap(isTempMap : true);
            }
            catch (Exception e)
            {
                Console.WriteLine("[REPORT],CreateMapFromTemp(),err: " + e.Message);
                return EErrorCode.REPORT_Temp_To_Map_Fail;
            }
            return EErrorCode.NONE;
        }

        public string GetFullPathWithFolder(string path, ETesterResultCreatFolderType FolderType)
        {
            string outPath = path;

            switch (FolderType)
            {
                case ETesterResultCreatFolderType.ByMachineName:
                    outPath = Path.Combine(outPath, UISetting.MachineName);
                    break;
                //-------------------------------------------------------------------------
                case ETesterResultCreatFolderType.ByLotNumber:
                    outPath = Path.Combine(outPath, UISetting.LotNumber);
                    break;
                //-------------------------------------------------------------------------
                case ETesterResultCreatFolderType.ByDataTime:

                    outPath = Path.Combine(outPath, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
                    break;
                //-------------------------------------------------------------------------
                case ETesterResultCreatFolderType.ByBarcode:

                    outPath = Path.Combine(outPath, UISetting.Barcode);
                    break;
                //-------------------------------------------------------------------------
                default:

                    break;
            }

            return outPath;
        }

        public virtual EErrorCode MergeFile( string outputPath ,List<string> fileList = null)
        {
            Console.WriteLine("[ReportBase],MergeFile()");
            List<string> headerText = new List<string>();
            Dictionary<int, string> colNameDic = new Dictionary<int, string>();
            Dictionary<string, List<string>> posStrListDic = new Dictionary<string, List<string>>();
            if (fileList != null && fileList.Count > 1)
            {
                for (int fCnt = 0; fCnt < fileList.Count && File.Exists(fileList[fCnt]); ++fCnt)
                {
                    #region
                    using (StreamReader sr = new StreamReader(fileList[fCnt]))
                    {
                        bool isRawData = false;
                        HeaderFinder hf = new HeaderFinder(this.TitleStrKey, TitleStrShift);
                        while (sr.Peek() >= 0)
                        {
                            string line = sr.ReadLine();
                            if (isRawData)
                            {
                                string[] rawData = line.Split(this.SpiltChar);

                                string colrowKey = ColRowKeyMaker(rawData);

                                if (!posStrListDic.ContainsKey(colrowKey))
                                {
                                    List<string> sList = new List<string>();
                                    sList.AddRange(rawData);
                                    posStrListDic.Add(colrowKey, sList);
                                }
                                else
                                {
                                    for (int i = 0; i < rawData.Length; ++i)
                                    {
                                        List<string> refList = posStrListDic[colrowKey];
                                        if (rawData[i] != null && rawData[i] != "")
                                        {
                                            refList[i] = rawData[i];
                                        }
                                    }
                                }
                            }
                            else
                            {
                                #region >>header<<
                                if (hf.CheckIfRowData(line))
                                {
                                    Console.WriteLine("[ReportBase],MergeFile(),found data header of" + fileList[fCnt]);
                                    isRawData = true;
                                    if (fCnt == 0)
                                    {
                                        string[] strArr = line.Split(this.SpiltChar);
                                        {
                                            for (int i = 0; i < strArr.Length; ++i)
                                            {
                                                colNameDic.Add(i, strArr[i]);
                                            }
                                        }
                                    }

                                    
                                }

                                if (fCnt == 0)
                                {
                                    headerText.Add(line);
                                }
                                #endregion
                            }
                        }

                    }
                    #endregion
                }

                Console.WriteLine("[ReportBase],MergeFile(),write out data");
                string mergeTmpPath = @"C:\MPI\Temp2\mergeTemp.csv";
                using (StreamWriter sw = new StreamWriter(mergeTmpPath))
                {
                    foreach (string str in headerText)
                    {
                        sw.WriteLine(str);
                    }
                    foreach (var p in posStrListDic.Values)
                    {
                        string outStr = "";
                        int legnth = p.Count;
                        int cnt = 1;
                        foreach (string str in p)
                        {
                            outStr += str;
                            if (cnt < legnth) { outStr += this.SpiltChar.ToString(); }
                            ++cnt;
                        }
                        sw.WriteLine(outStr);
                    }
                }
                if (File.Exists(mergeTmpPath))
                {
                    string fileName = Path.GetFileName(mergeTmpPath);
                    string backupFullName = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, fileName);
                    MPIFile.CopyFile(mergeTmpPath, outputPath);
                }
                MPIFile.CopyFile(mergeTmpPath, outputPath);
            }

            return EErrorCode.NONE;
        }

        public virtual void  PushRemarkInfo(string str)
        {
            RemarkList.Add(str);
        }
		#endregion

		#region >>> Protected Property <<<

        protected string TitleStrKey
        {
            set { _titleStrKey = ""; }
            get
            {
                if (_titleStrKey == "")
                {
                    if (ResultTitleInfo != null)
                    {
                        _titleStrKey = ResultTitleInfo.TitleStr;
                    }
                }
                return _titleStrKey;
            }
        }

		protected bool _isImplementLIVReport;

		protected bool _isImplementSpectrumReport;

		protected bool _isImplementSweepDataReport;

		protected bool _isImplementPIVDataReport;

		protected bool IsAppend
		{
			get { return this._isAppend; }
		}

        protected int TotalTestCount
        {
            get { return this._checkColRowKey.Count; }
        }

        protected string FileFullNameTmp
        {
            get { return this._fileFullNameTmp; }
            set { this._fileFullNameTmp = value; }
        }

        protected string FileFullNameTmp2
        {
            get { return this._fileFullNameTmp2; }
            set { this._fileFullNameTmp2 = value; }
        }

        protected string FileFullNameTmp3
        {
            get { return this._fileFullNameTmp3; }
            set { this._fileFullNameTmp3 = value; }
        }

		protected string FileFullNameRep
		{
			get { return this._fileFullNameRep; }
			set { this._fileFullNameRep = value; }
		}

		protected string FileFullNameRep2
		{
			get { return this._fileFullNameRep2; }
			set { this._fileFullNameRep2 = value; }
		}

		protected string FileFullNameRep3
		{
			get { return this._fileFullNameRep3; }
			set { this._fileFullNameRep3 = value; }
		}

		protected ProductData Product
		{
			get { return this._product; }
		}

		protected UISetting UISetting
		{
			get { return this._uiSetting; }
		}

		protected TesterSetting TesterSetting
		{
			get { return this._testerSetting; }
		}

		protected SmartBinning SmartBinning
		{
			get { return this._smartBinning; }
		}

		protected MachineInfoData MachineInfo
		{
			get { return this._machineInfo; }
		}

		protected MachineConfig MachineConfig
		{
			get { return this._machineConfig; }
		}
        protected SystemCali SysCali
        {
            get { return this._sysCali; }
        }
        

		protected Dictionary<string, TestResultData> ResultData
		{
			get { return this._resultData; }
			set { this._resultData = value; }
		}

		protected ResultTitleInfo ResultTitleInfo
		{
			get { return this._resultTitleInfo; }
		}

		protected ResultTitleInfo ResultTitleInfo2
		{
			get { return this._resultTitleInfo2; }
		}

		protected ResultTitleInfo ResultTitleInfo3
		{
			get { return this._resultTitleInfo3; }
		}

		protected AcquireData AcquireData
		{
			get { return this._acquireData; }
		}

		protected IRICalcMath RICalcMath
		{
			get { return this._riCalcMath; }
		}

        protected char SpiltChar
        {
            get { return this._spiltChar; }
            set { { this._spiltChar = value; } }
        }

        protected bool IsLogFileCreated { get; set; }

        protected List<string> RemarkList { get; set; }
		#endregion

		#region >>> Public Property <<<

		public string TestResultFileName
		{
			get { return this.TestResultFileNameWithoutExt(); }
		}

		public bool IsTempFileExist
		{
            get { return File.Exists(TempFullFileName); }
		}

        public string TempFullFileName
        {
            get { return Path.Combine(Constants.Paths.MPI_TEMP_DIR2, this.TestResultFileNameWithoutExt() + ReportBase.EXTEN_TMP1); }
		}

		public bool IsImplementLIVReport
		{
			get { return this._isImplementLIVReport; }
		}

		public bool IsImplementSpectrumReport
		{
			get { return this._isImplementSpectrumReport; }
		}

		public bool IsImplementSweepDataReport
		{
			get { return this._isImplementSweepDataReport; }
		}

		public bool IsImplementPIVDataReport
		{
			get { return this._isImplementPIVDataReport; }
		}

        public CoordTransferTool CustomizeCoordTransTool
        {
            get { return _customizeCoordTransferTool; }

            set {  _customizeCoordTransferTool = value; }
        }

        public CoordTransferTool CoordTransTool
        {
            get { return _p2tCoordTransferTool; }

            set { _p2tCoordTransferTool = value; }
        }

        
		#endregion

		#region >>> StatisticSet <<<

		private static StatisticSet _statisticSet = new StatisticSet();

		public static void ResetStatisticData(ProductData product)
		{
			ReportBase._statisticSet.SetData(product.TestCondition.TestItemArray);
		}

		public static void ResetStatisticData(TestResultData[] results)
		{
			ReportBase._statisticSet.SetData(results);
		}

		public static void PushStatisticData(Dictionary<string, double> data)
		{
			ReportBase._statisticSet.Push(data);
		}

		public static Statistic StatisticData(string keyName, EStatisticType type)
		{
			return ReportBase._statisticSet[keyName, type];
		}

		public static Statistic StatisticData(ESysResultItem keyName, EStatisticType type)
		{
			return ReportBase._statisticSet[keyName.ToString(), type];
		}

		public static double GoodRateS01(string keyName)
		{
			return ReportBase._statisticSet.GoodRateS01(keyName);
		}

		public static double GoodRateS02(string keyName)
		{
			return ReportBase._statisticSet.GoodRateS02(keyName);
		}

		public static double GoodRateS03(string keyName)
		{
			return ReportBase._statisticSet.GoodRateS03(keyName);
		}

		public static double UpperCountS01(string keyName)
		{
			return ReportBase._statisticSet.UpperCountS01(keyName);
		}

		public static double UnderCountS01(string keyName)
		{
			return ReportBase._statisticSet.UnderCountS01(keyName);
		}

		public static double UpperCountS02(string keyName)
		{
			return ReportBase._statisticSet.UpperCountS02(keyName);
		}

		public static double UnderCountS02(string keyName)
		{
			return ReportBase._statisticSet.UnderCountS02(keyName);
		}

		public static double UpperCountS03(string keyName)
		{
			return ReportBase._statisticSet.UpperCountS03(keyName);
		}

		public static double UnderCountS03(string keyName)
		{
			return ReportBase._statisticSet.UnderCountS03(keyName);
		}

		public static double UpperRateS01(string keyName)
		{
			return ReportBase._statisticSet.UpperRateS01(keyName);
		}

		public static double UnderRateS01(string keyName)
		{
			return ReportBase._statisticSet.UnderRateS01(keyName);
		}

		public static double UpperRateS02(string keyName)
		{
			return ReportBase._statisticSet.UpperRateS02(keyName);
		}

		public static double UnderRateS02(string keyName)
		{
			return ReportBase._statisticSet.UnderRateS02(keyName);
		}

		public static double UpperRateS03(string keyName)
		{
			return ReportBase._statisticSet.UpperRateS03(keyName);
		}

		public static double UnderRateS03(string keyName)
		{
			return ReportBase._statisticSet.UnderRateS03(keyName);
		}

		public static int GoodCount01
		{
			get { return ReportBase._statisticSet.GoodCount01; }
		}

		public static int FailCount01
		{
			get { return ReportBase._statisticSet.FailCount01; }
		}

		public static int GoodCount02
		{
			get { return ReportBase._statisticSet.GoodCount02; }
		}

		public static int FailCount02
		{
			get { return ReportBase._statisticSet.FailCount02; }
		}

		public static int GoodCount03
		{
			get { return ReportBase._statisticSet.GoodCount03; }
		}

		public static int FailCount03
		{
			get { return ReportBase._statisticSet.FailCount03; }
		}

		public static double GoodRateA01
		{
			get { return ReportBase._statisticSet.GoodRateA01; }
		}

		public static double FailRateA01
		{
			get { return ReportBase._statisticSet.FailRateA01; }
		}

		public static double GoodRateA02
		{
			get { return ReportBase._statisticSet.GoodRateA02; }
		}

		public static double FailRateA02
		{
			get { return ReportBase._statisticSet.FailRateA02; }
		}

		public static double GoodRateA03
		{
			get { return ReportBase._statisticSet.GoodRateA03; }
		}

		public static double FailRateA03
		{
			get { return ReportBase._statisticSet.FailRateA03; }
		}

		public static int TestCount
		{
			get { return ReportBase._statisticSet.TestCount; }
		}

		#endregion
	}

	#region >>> Report Class <<<

	public class ReportProjectData
	{
		private Encoding _encoding;

		public ReportProjectData()
		{
			this._encoding = Encoding.Default;
		}

		public Encoding Encoding
		{
			get { return this._encoding; }
			set { this._encoding = value; }
		}
	}

	public class ColRow
	{
		private int _col;
		private int _row; 

		public ColRow(int col, int row)
		{
			this._col = col;

			this._row = row;
		}

		public int Col { get { return this._col; } }
		public int Row { get { return this._row; } }
	}

	public class OptiReportData
	{
		private int _contcIndex;

		private List<int> _optiIndexList;

		private ResultTitleInfo _resultTitleInfo;

		private Dictionary<string, Dictionary<int, string>> _data;

		public OptiReportData(ResultTitleInfo resultTitleInfo)
		{
			this._resultTitleInfo = resultTitleInfo;

			this._data = new Dictionary<string, Dictionary<int, string>>();

			//-------------------------------------------------------------
			// get opti msrt result index in report
			//-------------------------------------------------------------
			string[] msrtType = Enum.GetNames(typeof(EOptiMsrtType));

			int index = 0;

			this._optiIndexList = new List<int>();

			foreach (var item in resultTitleInfo)
			{
				string key = item.Key;

				if (key.IndexOf('_') > 0)
				{
					key = key.Remove(key.IndexOf('_'));
				}

				if (key == "TestChipGroup")
				{
					this._contcIndex = index;
				}

				foreach (var type in msrtType)
				{
					if (key == type && key != "MVFLA")
					{
						this._optiIndexList.Add(index);

						break;
					}
				}

				index++;
			}
		}

		public bool LoadOptiFileData(string fileFullName)
		{
			try
			{
				//-------------------------------------------------------------
				// get data from report
				//-------------------------------------------------------------
				this._data.Clear();

				using (StreamReader sr = new StreamReader(fileFullName, Encoding.Default))
				{
					bool isRawData = false;

					string t = this._resultTitleInfo.TitleStr;

					while (sr.Peek() >= 0)
					{
						string line = sr.ReadLine();

						if (isRawData)
						{
							string[] items = line.Split(',');

							string col = items[this._resultTitleInfo.ColIndex];

							string row = items[this._resultTitleInfo.RowIndex];

							string key = col + "_" + row;

							Dictionary<int, string> raw = new Dictionary<int, string>();

							for (int i = 0; i < this._optiIndexList.Count; i++)
							{
								int index = this._optiIndexList[i];

								raw.Add(index, items[index]);
							}

							this._data.Add(key, raw);

						}
						else if (line == t)
						{
							isRawData = true;
						}
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("[OptiReportData], Open(), catch:" + e.ToString());

				return false;
			}

			return true;
		}

		public bool WriteOptiData(string[] items)
		{
			try
			{
				string col = items[this._resultTitleInfo.ColIndex];

				string row = items[this._resultTitleInfo.RowIndex];

				string key = col + "_" + row;

				if (this._data.ContainsKey(key))
				{
					Dictionary<int, string> raw = this._data[key];

					//items[this._contcIndex] = "1";

					foreach (var item in raw)
					{
						int index = item.Key;

						string value = item.Value;

						items[index] = value;
					}
				}
				else
				{
					//items[this._contcIndex] = "2";
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("[OptiReportData], WriteOptiData(), catch:" + e.ToString());

				return false;
			}

			return true;
		}
	}

	#endregion
}
