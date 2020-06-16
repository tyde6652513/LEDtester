using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MPI.Tester.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using MPI.Tester.Maths;
using MPI.Tester.TestServer;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Report.User.Emcore
{
    class Report : ReportBase
    {
        private double overloadForce = 0;
        private string tempFileName = "Normal";

        private enum EOLStatus : int
        {
            lngOlPass = 0,
            lngOlFail = 1,
            lngFailedInitial = 2,
            lngFailedFinal = 3,
        }


        private List<string> otKeyList = new List<string>();

        private Regex EndWithNumPattern = new Regex("[A-Za-z][0-9]+");

        public Report(List<object> objs, bool isReStatistic)
            : base(objs, isReStatistic)
        {
        }

        #region >> protected override<<

        protected override void SetResultTitle()
        {
            this.ResultTitleInfo.SetResultData(this.UISetting.UserDefinedData.ResultItemNameDic);

            if (this.Product.TestCondition.TestItemArray != null)
            {
                foreach (var tData in this.Product.TestCondition.TestItemArray)
                {

                    foreach (var rData in tData.MsrtResult)
                    {
                        if (GlobalFlag.TestMode == ETesterTestMode.Overload)
                        {
                            otKeyList.Add(rData.KeyName);
                        }

                        if (rData.KeyName == "MVZ_7")
                        {
                            overloadForce = Math.Abs(tData.ElecSetting[0].ForceValue / 1000000);//uA-> A
                        }
                    }


                }
            }

            otKeyList.Add("OLTestSequence");

            #region >>Emcore Process<<
            if (GlobalFlag.TestMode == ETesterTestMode.Normal)
            {
                tempFileName = "Normal." + this.UISetting.TestResultFileExt; 
            }
            else
            {
                tempFileName = "Overload." + this.UISetting.TestResultFileExt;
            }
            string tmpFilePath =  Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "TempFile");
            CleanOldDic( tmpFilePath);

            #endregion
        }

        protected override EErrorCode WriteReportHeadByUser()
        {
            this.WriteLine(this.ResultTitleInfo.TitleStr);
            return EErrorCode.NONE;
        }

        protected override EErrorCode PushDataByUser(Dictionary<string, double> data)
        {
            int binSN = (int)data["BINSN"];

            SmartBinDataBase bin = this.SmartBinning.GetBinFromSN(binSN);

            int binGrade = 0;

            int binNumber = 0;

            string binCode = string.Empty;

            if (bin != null)
            {
                binCode = bin.BinCode;

                binNumber = bin.BinNumber;

                if (bin.BinningType == EBinningType.IN_BIN)
                {
                    binGrade = 1;
                }
                else if (bin.BinningType == EBinningType.SIDE_BIN)
                {
                    binGrade = 2;
                }
                else if (bin.BinningType == EBinningType.NG_BIN)
                {
                    binGrade = 3;
                }
            }

            string line = string.Empty;
            int count = 0;
            int length = this.ResultTitleInfo.ResultCount;
            foreach (var item in this.ResultTitleInfo)
            {
                switch (item.Key)
                {
                    case "BIN":
                        {
                            if (data["ISALLPASS"] == 1)
                            {
                                line += "0";
                            }
                            else
                            {
                                line += binNumber.ToString();
                            }
                        }
                        break;
                    case "DeviceType":
                        {
                            line += Product.ProductName; ;
                        }
                        break;
                    case "Name":
                        {
                            if (data["ISALLPASS"] == 1)
                            {
                                line += UISetting.TaskSheetFileName;
                            }
                            else
                            {
                                line += "Fail";
                            }
                        }
                        break;
                    case "WaferNum":
                        {
                            line += UISetting.WaferNumber;
                        }
                        break;
                    case "ChipID":
                        {
                            if (CoordTransTool != null && CoordTransTool.IsValid)
                            {
                                int x, y;
                                x = (int)data["COL"];
                                y = (int)data["ROW"];
                                Matrix mat = CoordTransTool.TransCoord(x, y);

                                string IDStr = XY2ChipID((int)mat[0, 0], (int)mat[1, 0]);

                                line += IDStr;
                            }
                        }
                        break;
                    case "SIDNum":
                        { }
                        break;
                    case "TestTemp1":
                        {
                            if (this.AcquireData.ChipInfo.IsNewChuckData)
                            {
                                string format = string.Empty;

                                if (this.UISetting.UserDefinedData[item.Key] != null)
                                {
                                    format = this.UISetting.UserDefinedData[item.Key].Formate;
                                }
                                line += this.AcquireData.ChipInfo.ChuckTemp.ToString(format);
                            }
                        }
                        break;
                    case "Orientation":
                        {
                            line += 0;
                        }
                        break;
                    case "Grade":
                        {
                            if (data["ISALLPASS"] == 1)
                            {
                                line += binNumber.ToString();
                            }
                            else
                            {
                                line += 0;
                            }
                        }
                        break;
                    case "OpInitial":
                        {
                            line += this.UISetting.OperatorName;
                        }
                        break;
                    case "OLTestStatus":
                        {
                            line += CheckOLTestState(data);
                        }
                        break;
                    case "Vfwd2Result1":
                        {
                            line += CheckVF2PassBySourceKey(item.Key, "UP");
                        }
                        break;
                    case "ShortResult1":
                        {
                            line += CheckVF2PassBySourceKey("Vfwd2Result1", "DOWN");
                        }
                        break;
                    case "VbResult1":
                    case "Vfwd1Result1":
                    case "ResistanceResult1":
                    case "Id1Result1":
                    case "I0Result1":
                    case "VptResult1":
                    case "DeltaVResult1":
                    case "VhetResult1":
                    case "VbmResult1":
                    case "MmaxResult1":
                        {
                            line += CheckIfPassBySourceKey(item.Key);
                        }
                        break;
                    case "StationNum":
                        { line += UISetting.MachineName; }
                        break;
                    case "VbOverloadResult1":
                        {
                            string str = CheckOLTestState(data);
                            if (str == "" || str == "0")
                            {
                                line += 1;
                            }
                            else
                            {
                                line += -1;
                            }
                        }
                        break;
                    case "OLCurrent":
                        {
                            //if (otKeyList.Contains("MVZ_7") && GlobalFlag.TestMode == ETesterTestMode.Normal)
                            if (GlobalFlag.TestMode == ETesterTestMode.Normal)
                            {
                                break;
                            }
                            line += overloadForce.ToString("E2");
                        }
                        break;
                    case "OpenResult1":
                        { line += 1; }
                        break;
                    case "TestDate":
                        {
                            line += DateTime.Now.ToString("dd/MMM/yy hh:mm:ss tt", CultureInfo.CreateSpecificCulture("en-US"));
                        }
                        break;


                    case "ProgramVersion":
                        {
                            line += UISetting.SoftwareVersoin;//"1.2.07.27T-D37"; 
                        }

                        break;

                    default:
                        {
                            if (otKeyList.Contains(item.Key) && GlobalFlag.TestMode == ETesterTestMode.Normal)
                            {
                                break;
                            }
                            if (data.ContainsKey(item.Key))
                            {
                                if(AcquireData.EnableTestResult.Contains(item.Key))
                                {
                                string format = string.Empty;

                                if (this.UISetting.UserDefinedData[item.Key] != null)
                                {
                                    format = this.UISetting.UserDefinedData[item.Key].Formate;
                                }
                                string str = Math.Abs(data[item.Key]).ToString(format);

                                line += str;
                                }
                            }

                        }
                        break;

                }
                count++;
                if (count != length)
                {
                    line += ",";
                }
            }
            int strLength = line.Length;

            //line.Remove(strLength - 1);

            this.WriteLine(line);

            return EErrorCode.NONE;
        }

        protected override EErrorCode RewriteReportByUser()
        {
            return ReplaceReportData(this.FileFullNameTmp, this.FileFullNameRep);
        }

        protected override EErrorCode MoveFileToTargetByUser(EServerQueryCmd cmd)
        {
            bool isOutputPath02 = false;

            bool isOutputPath03 = false;

            string outPath01 = string.Empty;

            string outPath02 = string.Empty;

            string outPath03 = string.Empty;

            ETesterResultCreatFolderType type01 = ETesterResultCreatFolderType.None;

            ETesterResultCreatFolderType type02 = ETesterResultCreatFolderType.None;

            ETesterResultCreatFolderType type03 = ETesterResultCreatFolderType.None;

            if (this.UISetting.IsManualRunMode)
            {
                SetManualMode(ref isOutputPath02, ref isOutputPath03, ref outPath01, ref outPath02, ref outPath03, ref type01, ref type02, ref type03);
            }
            else
            {
                SetAutoMode(ref isOutputPath02, ref isOutputPath03, ref outPath01, ref outPath02, ref outPath03, ref type01, ref type02, ref type03);
            }


            outPath01 = GetPathWithFolder(outPath01, type01);

            outPath02 = GetPathWithFolder(outPath02, type02);

            outPath03 = GetPathWithFolder(outPath03, type03);


            //---------------------------------------------------------------------------------
            // Copy Report file to taget path
            //---------------------------------------------------------------------------------
            string fileNameWithoutExt = this.TestResultFileNameWithoutExt();

            string fileNameWithExt = this.TestResultFileNameWithoutExt();
            
            //Abort
            if (cmd == EServerQueryCmd.CMD_TESTER_ABORT)
            {
                fileNameWithoutExt = fileNameWithoutExt + "_" + DateTime.Now.ToString("yyMMddhhmmss");
            }
            else 
            {
                if (!this.UISetting.IsManualRunMode)//auto run
                {
                    string tempFilePath = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "TempFile", fileNameWithoutExt.Trim(), tempFileName);
                    MPIFile.CopyFile(this.FileFullNameRep, tempFilePath);

					//fileNameWithoutExt = fileNameWithoutExt + "_" + DateTime.Now.ToString( "yyyyMMddhhmmss" );

                    if (GlobalFlag.TestMode == ETesterTestMode.Overload)
                    {
						fileNameWithoutExt = fileNameWithoutExt + "_" + DateTime.Now.ToString( "yyyyMMddhhmmss" );
						//fileNameWithoutExt = fileNameWithoutExt + "_NT";
                    }
                    else if (GlobalFlag.TestMode == ETesterTestMode.Normal)
                    {
						fileNameWithoutExt = fileNameWithoutExt + "_" + DateTime.Now.ToString( "yyyyMMddhhmmss" );
						//fileNameWithoutExt = fileNameWithoutExt + "_OT";
                    }

                }
                
				
            }            

            fileNameWithExt = fileNameWithoutExt + "." + this.UISetting.TestResultFileExt;

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
        
        protected override EErrorCode PostProcessAfterMoveFile(EServerQueryCmd cmd)
        {
            EErrorCode err = EErrorCode.NONE;


            if (GlobalFlag.TestMode == ETesterTestMode.Normal &&
                !this.UISetting.IsManualRunMode)
            {
                string outPath01 = string.Empty;

                ETesterResultCreatFolderType type01 = ETesterResultCreatFolderType.None;

                if (this.UISetting.IsManualRunMode)
                {
                    outPath01 = this.UISetting.ManualOutputPath01;

                    type01 = this.UISetting.ManualOutputPathType01;
                }
                else
                {
                    outPath01 = this.UISetting.TestResultPath01;

                    type01 = this.UISetting.TesterResultCreatFolderType01;
                }

                outPath01 = GetPathWithFolder(outPath01, type01);

                //---------------------------------------------------------------------------------
                // Copy Report file to taget path
                //---------------------------------------------------------------------------------
                string fileNameWithoutExt = this.TestResultFileNameWithoutExt();

                string fileNameWithExt = this.TestResultFileNameWithoutExt();


                fileNameWithExt = fileNameWithoutExt + "." + this.UISetting.TestResultFileExt;

                //Abort
                if (cmd == EServerQueryCmd.CMD_TESTER_ABORT)
                {

                }
                else
                {
                    
                    string tmpFilePath = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "TempFile", fileNameWithoutExt.Trim());
                    string tempFilePathNormal = Path.Combine(tmpFilePath, "Normal." + this.UISetting.TestResultFileExt);
                    string tempFilePathOverload = Path.Combine(tmpFilePath, "Overload." + this.UISetting.TestResultFileExt);
                    if (File.Exists(tempFilePathNormal) && File.Exists(tempFilePathOverload))
                    {

                        string mergTmpPath = Path.Combine(tmpFilePath, "mergeTmp.csv");
                        // cmd,id,outpath,outFileName,ReserveFullDie,IsEnableFile3,path1,2,3
                        System.Diagnostics.Process.Start("C:\\MPI\\MapAnalyzer\\MapAnalyzer.exe", "Merge 9050 " + tmpFilePath + " mergeTmp true false " + tempFilePathNormal + " " + tempFilePathOverload);

						long lineNum =  0 ;
						using ( StreamReader sr = new StreamReader( tempFilePathNormal ) )
						{
							while ( sr.Peek() >= 0 )
							{
								string str = sr.ReadLine();

								lineNum++;
							}
						}

                        string outputPathAndFile01 = Path.Combine(outPath01, fileNameWithExt);

						Console.WriteLine( "[Report Emcore], PostProcessAfterMoveFile(),File Copy");
						for ( int i = 0; i <= lineNum / 1000 + 1; ++i )
						{
							System.Threading.Thread.Sleep( 1000 );
							if ( MPIFile.CopyFile( mergTmpPath, outputPathAndFile01 ) )
							{
								try
								{
									Directory.Delete( tmpFilePath, true );
								}
								catch ( Exception e )
								{
									Console.WriteLine( "[Report Emcore], PostProcessAfterMoveFile(),Folder delet Fail,"
									+ "Excption: " + e.Message + ",Path" + tmpFilePath );
								}

								Console.WriteLine( "File Copy Success");
								break;
								//Directory
							}
							else
							{
								Console.WriteLine( "File Copy delay + " +( i+1 * 1000).ToString() +"ms" );
							}
							
						}
                    }
                    //MPIFile.CopyFile(this.FileFullNameRep, tempFilePath);
                }
            }

            return err;
 
        }
        #endregion

        #region >> private<<
        private bool CleanOldDic(string folderPath)
        {
            bool result = true;
            string tmpFilePath = folderPath;// Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "TempFile");
            if (!Directory.Exists(tmpFilePath))
            {
                Directory.CreateDirectory(tmpFilePath);
                return false;
            }
            List<string> folderList = Directory.GetDirectories(tmpFilePath).ToList();
            TimeSpan tsLim = new TimeSpan(30, 0, 0, 0, 0);//30 days
            foreach (string fPath in folderList)
            {
                TimeSpan ts = DateTime.Now - Directory.GetCreationTime(fPath);

                if (ts > tsLim)
                {
                    Console.WriteLine("[Report Emcore], CleanOldDic(),delet old folder:" + fPath);
                    Directory.Delete(fPath, true);
                }
            }

            return result;
        }
        private string CheckIfPassBySourceKey(string sourceKey)
        {
            string tarName = sourceKey.Replace("Result", "_");

            string[] strArr = tarName.Split('_');
            if (EndWithNumPattern.IsMatch(strArr[0]))
            {
                tarName = strArr[0];
            }
            else
            {
                tarName = strArr[0] + strArr[1];
            }

            string result = "";
            foreach (var rData in AcquireData.OutputTestResult)
            {
                if (rData.Name.ToUpper().StartsWith(tarName.ToUpper()) && rData.IsTested)
                {
                    if (rData.IsPassAndIsTested)
                    {
                        result = "1";
                    }
                    else
                    {
                        result = "-1";
                    }
                    break;
                }
            }
            return result;
        }

        private string CheckVF2PassBySourceKey(string sourceKey, string searchSide)
        {
            string tarName = sourceKey.Replace("Result", "_");

            string[] strArr = tarName.Split('_');
            if (EndWithNumPattern.IsMatch(strArr[0]))
            {
                tarName = strArr[0];
            }
            else
            {
                tarName = strArr[0] + strArr[1];
            }

            string result = "";
            foreach (var rData in AcquireData.OutputTestResult)
            {
                if (rData.Name.ToUpper().StartsWith(tarName.ToUpper()) && rData.IsTested)
                {
                    switch (searchSide.ToUpper())
                    {
                        case "UP":
                        case "U":
                            {
                                result = rData.Value <= rData.MaxLimitValue ? "1" : "-1";
                            }
                            break;
                        case "DOWN":
                        case "D":
                            {
                                result = rData.Value >= rData.MinLimitValue ? "1" : "-1";
                            }
                            break;
                    }
                }
            }
            return result;
        }

        private bool CheckIfPassByTarKey(string TarKey)
        {
            bool result = true;
            foreach (var rData in AcquireData.OutputTestResult)
            {
                if (rData.KeyName == TarKey && rData.IsTested)
                {
                    if (rData.IsPassAndIsTested)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                    break;
                }
            }
            return result;
        }

        private bool CheckIfPassByTarKey(List<string> TarKeyList)
        {
            bool result = true;

            foreach (var tKey in TarKeyList)
            {
                if (!CheckIfPassByTarKey(tKey))
                {
                    return false;
                }
            }
            return result;
        }

        private string CheckOLTestState(Dictionary<string, double> data)
        {
            string result = "";

            if (GlobalFlag.TestMode == ETesterTestMode.Overload)
            {
                List<string> itemNameList = new List<string>();
                itemNameList = (new string[] { "MVF_3", "MVZ_6", "MIR_4" }).ToList();
                bool olResule = CheckIfPassByTarKey(itemNameList);

                if (!olResule)
                {
                    return ((int)EOLStatus.lngFailedInitial).ToString();
                }

                itemNameList = (new string[] { "MVF_4", "MIR_5" }).ToList();
                olResule = CheckIfPassByTarKey(itemNameList);
                if (!olResule)
                {
                    return ((int)EOLStatus.lngFailedFinal).ToString();
                }

                itemNameList = (new string[] { "MCALC_6" }).ToList();
                olResule = CheckIfPassByTarKey(itemNameList);
                if (!olResule)
                {
                    return ((int)EOLStatus.lngOlFail).ToString();
                }

                return ((int)EOLStatus.lngOlPass).ToString();
            }
            else
            {
                result = "";
            }


            return result;
        }

        private EErrorCode ReplaceReportData(string inputFile, string outputFile)
        {
            ///////////////////////////////////////////////////////
            //Set Statistic Data
            ///////////////////////////////////////////////////////
            Dictionary<string, double> data = new Dictionary<string, double>();


            ///////////////////////////////////////////////////////
            //Replace Data And Check Row Col
            ///////////////////////////////////////////////////////
            StreamWriter sw = new StreamWriter(outputFile, false, this._reportData.Encoding);

            StreamReader sr = new StreamReader(inputFile, this._reportData.Encoding);

            bool isRawData = false;

            int rawLineCount = 0;

            int testCount = this.UISetting.UserDefinedData.TestStartIndex;

            Dictionary<string, string[]> keyValArrDic = new Dictionary<string, string[]>();

            String[] refKeyArr = new string[] {"VbResult1",
                    "Vfwd1Result1","Vfwd2Result1","ResistanceResult1","Id1Result1",
                    "I0Result1","VptResult1","DeltaVResult1","VhetResult1","VbmResult1", "MmaxResult1","VbOverloadResult1"};



            List<int> refIndexList = new List<int>();
            refIndexList = (from str in refKeyArr
                            select ResultTitleInfo.GetIndexOfKey(str)).ToList();
            // 開始比對ColRowKey並寫檔
            while (sr.Peek() >= 0)
            {
                string line = sr.ReadLine();

                if (isRawData)
                {

                    rawLineCount++;

                    string[] rawData = line.Split(',');
                    //string[] oriData;
                    string colrowKey = rawData[ResultTitleInfo.ColIndex].ToString() + "_" + rawData[ResultTitleInfo.RowIndex].ToString();

                    // 把 row.col 和 checkRowCol "raw line count " 相同時, 才會push資料,解決當點重測row,col的問題
                    if (this._checkColRowKey.ContainsKey(colrowKey) && this._checkColRowKey[colrowKey] == rawLineCount)
                    {
                        if (keyValArrDic.ContainsKey(colrowKey))//overload test or 
                        {
                            //oriData = keyValArrDic[colrowKey];
                            ReplaceDicData(keyValArrDic, rawData, colrowKey, refIndexList);

                            int index = ResultTitleInfo.GetIndexOfKey("OLTestStatus");

                            if (index >= 0 && rawData[index] != "")//Overload test
                            {
                                double val = 0;
                                if (double.TryParse(rawData[index], out val))
                                {
                                    for (int i = 0; i < refKeyArr.Length; ++i)
                                    {
                                        if (val != 0)
                                        {

                                            if (refKeyArr[i] == "Name")
                                            {
                                                int tarIndex = ResultTitleInfo.GetIndexOfKey(refKeyArr[i]);
                                                keyValArrDic[colrowKey][tarIndex] = "Fail";
                                            }
                                        }
                                    }
                                }
                            }

                            rawData = keyValArrDic[colrowKey];
                        }

                        if (ResultTitleInfo.TestIndex >= 0)
                        {
                            rawData[ResultTitleInfo.TestIndex] = testCount.ToString();

                            line = string.Empty;

                            for (int i = 0; i < rawData.Length; i++)
                            {
                                line += rawData[i];

                                if (i != rawData.Length - 1)
                                {
                                    line += ",";
                                }
                            }
                        }

                        testCount++;
                    }
                    else
                    {
                        ReplaceDicData(keyValArrDic, rawData, colrowKey, refIndexList);

                        continue;
                    }

                }
                else
                {
                    if (line == ResultTitleInfo.TitleStr)
                    {
                        isRawData = true;
                    }
                    else
                    {
                        //if (replaceData.ContainsKey(line))
                        //{
                        //    line = replaceData[line];
                        //}
                    }
                }

                sw.WriteLine(line);
            }

            sr.Close();

            sr.Dispose();

            sw.Close();

            sw.Dispose();

            return EErrorCode.NONE;
        }

        private void ReplaceDicData(Dictionary<string, string[]> keyValArrDic, string[] rawData, string colrowKey, List<int> refIndexList)
        {
            if (!keyValArrDic.ContainsKey(colrowKey))
            {
                keyValArrDic.Add(colrowKey, rawData);
            }
            else
            {
                string str = keyValArrDic[colrowKey].ToString();
            }

            for (int i = 0; i < keyValArrDic[colrowKey].Length; ++i)
            {
                double val = 0;
                string str = keyValArrDic[colrowKey][i];

                double newVal = 0;

                if (double.TryParse(rawData[i], out newVal))
                {
                    if (double.TryParse(str, out val))
                    {
                        if (refIndexList.Contains(i))//小的優先
                        {
                            if (newVal < val)
                            {
                                keyValArrDic[colrowKey][i] = rawData[i];
                            }
                        }
                        else
                        {
                            if (newVal > val)
                            {
                                keyValArrDic[colrowKey][i] = rawData[i];
                            }
                        }
                    }
                    else
                    {
                        keyValArrDic[colrowKey][i] = rawData[i];
                    }
                }
                else
                {
                    if (rawData[i] != "")
                    {
                        keyValArrDic[colrowKey][i] = rawData[i];
                    }
                }

            }
        }

        private string XY2ChipID(int x, int y)
        {
            string outStr = "";

            int ch1 = 0, ch10 = 0;

            //Y = 1    00 01 => AA
            //Y = 26   00 26 => AZ
            //Y = 27   01 01 => BA
            //Y = 52   01 01 => BZ

            ch1 = (y) % 26;
            ch10 = (y - ch1) / 26;
            if (ch1 == 0)
            {
                ch1 = 26;
                ch10 = (y - ch1) / 26;
            }
            string str = Convert.ToChar(65 + ch10).ToString() + Convert.ToChar(64 + ch1).ToString();

            outStr += str + x.ToString("000");


            return outStr;
        }
        #endregion

        #region >> public override<<
 
        #endregion



    }
}
