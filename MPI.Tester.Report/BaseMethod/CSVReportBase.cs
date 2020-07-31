using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;
using System.IO;
using MPI.Tester.TestServer;
using MPI.Tester.Maths;
using MPI.Tester.TestKernel;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Report.BaseMethod.HeaderFinder;

namespace MPI.Tester.Report
{
    public abstract partial class ReportBase
    {
        #region >>> Protected Virtual Method <<<

       
        protected virtual EErrorCode RunCommandByUser(EServerQueryCmd cmd)
        {
            switch (cmd)
            {
                case EServerQueryCmd.CMD_TESTER_START:
                    {
                        #region
                        if (this._isAppend)
                        {
                            return EErrorCode.NONE;
                        }
                        else
                        {
                            return this.WriteReportHeadByUser(); // *.Temp
                        }
                        #endregion
                    }
                case EServerQueryCmd.CMD_TESTER_END:
                    {
                        #region
                        EErrorCode code = this.RewriteReportByUser(); // *.CSV

                        if (code != EErrorCode.NONE)
                        {
                            return code;
                        }

                        code = this.ReportInterpolation();

                        if (code != EErrorCode.NONE)
                        {
                            return code;
                        }

                        code = this.MoveFileToTargetByUser(cmd);

                        if (code != EErrorCode.NONE)
                        {
                            return code;
                        }

                        code = PostProcessAfterMoveFile(cmd);
                        if (code != EErrorCode.NONE)
                        {
                            return code;
                        }
                        return EErrorCode.NONE;
                        #endregion
                    }
                case EServerQueryCmd.CMD_TESTER_ABORT:
                    {
                        #region
                        if (this.UISetting.IsAbortSaveFile)
                        {
                            EErrorCode code = this.RewriteReportByUser(); // *.CSV

                            if (code != EErrorCode.NONE)
                            {
                                return code;
                            }

                            code = this.ReportInterpolation();

                            if (code != EErrorCode.NONE)
                            {
                                return code;
                            }

                            code = this.MoveFileToTargetByUser(cmd);

                            return code;
                            //return MergeFile();
                        }
                        else
                        {
                            return EErrorCode.NONE;
                        }
                        #endregion
                    }
                case EServerQueryCmd.CMD_WAFER_FINISH:
                    {
                 #region
                        EErrorCode code = ProcessAfterWaferFinished();
                        if (code != EErrorCode.NONE)
                        {
                            return code;
                        }
                        return EErrorCode.NONE;
                 #endregion
                    }
                    break;
                default:
                #region
                    {
                        return EErrorCode.NONE;
                    }
                #endregion
            }
            return EErrorCode.NONE;
        }

        protected virtual EErrorCode RunCommandByUser2(EServerQueryCmd cmd)
        {
            switch (cmd)
            {
                case EServerQueryCmd.CMD_TESTER_START:
                    {
                        if (this._isAppend)
                        {
                            return EErrorCode.NONE;
                        }
                        else
                        {
                            return this.WriteReportHeadByUser2(); // *.Temp
                        }
                    }
                case EServerQueryCmd.CMD_TESTER_END:
                    {
                        EErrorCode code = this.RewriteReportByUser2();

                        if (code != EErrorCode.NONE)
                        {
                            return code;
                        }

                        return this.MoveFileToTargetByUser2(cmd);
                    }
                case EServerQueryCmd.CMD_TESTER_ABORT:
                    {
                        if (this.UISetting.IsAbortSaveFile)
                        {
                            EErrorCode code = this.RewriteReportByUser2();

                            if (code != EErrorCode.NONE)
                            {
                                return code;
                            }

                            return this.MoveFileToTargetByUser2(cmd);
                        }
                        else
                        {
                            return EErrorCode.NONE;
                        }
                    }
                default:
                    {
                        return EErrorCode.NONE;
                    }
            }
        }

        protected virtual EErrorCode RunCommandByUser3(EServerQueryCmd cmd)
        {
            switch (cmd)
            {
                case EServerQueryCmd.CMD_TESTER_START:
                    {
                        if (this._isAppend)
                        {
                            return EErrorCode.NONE;
                        }
                        else
                        {
                            return this.WriteReportHeadByUser3(); // *.Temp
                        }
                    }
                case EServerQueryCmd.CMD_TESTER_END:
                    {
                        EErrorCode code = this.RewriteReportByUser3();

                        if (code != EErrorCode.NONE)
                        {
                            return code;
                        }

                        return this.MoveFileToTargetByUser3(cmd);
                    }
                case EServerQueryCmd.CMD_TESTER_ABORT:
                    {
                        if (this.UISetting.IsAbortSaveFile)
                        {
                            EErrorCode code = this.RewriteReportByUser3();

                            if (code != EErrorCode.NONE)
                            {
                                return code;
                            }

                            return this.MoveFileToTargetByUser3(cmd);
                        }
                        else
                        {
                            return EErrorCode.NONE;
                        }
                    }
                default:
                    {
                        return EErrorCode.NONE;
                    }
            }
        }

        protected virtual EErrorCode WriteReportHeadByUser2()
        {
            return EErrorCode.NONE;
        }

        protected virtual EErrorCode WriteReportHeadByUser3()
        {
            return EErrorCode.NONE;
        }

        protected virtual EErrorCode PushDataByUser(Dictionary<string, double> data)
        {
            int binSN = (int)data["BINSN"];

            SmartBinDataBase bin = this._smartBinning.GetBinFromSN(binSN);

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

            int index = 0;

            foreach (var item in this._resultTitleInfo)
            {
                if (item.Key == "BIN_CODE")
                {
                    line += binCode;
                }
                else if (item.Key == "BIN_NUMBER")
                {
                    line += binNumber.ToString();
                }
                else if (item.Key == "BIN_GRADE")
                {
                    line += binGrade.ToString();
                }
                else if (data.ContainsKey(item.Key))
                {
                    string format = string.Empty;

                    if (this._uiSetting.UserDefinedData[item.Key] != null)
                    {
                        format = this._uiSetting.UserDefinedData[item.Key].Formate;
                    }

                    line += data[item.Key].ToString(format);
                }

                index++;

                if (index != this._resultTitleInfo.ResultCount)
                {
                    line += this.SpiltChar;
                }
            }

            this.WriteLine(line);

            return EErrorCode.NONE;
        }

        protected virtual EErrorCode PushDataByUser2(Dictionary<string, double> data)
        {
            return EErrorCode.NONE;
        }

        protected virtual EErrorCode PushDataByUser3(Dictionary<string, double> data)
        {
            return EErrorCode.NONE;
        }

        protected virtual EErrorCode RewriteReportByUser2()
        {
            this.ReplaceReport2(new Dictionary<string, string>());

            return EErrorCode.NONE;
        }

        protected virtual EErrorCode RewriteReportByUser3()
        {
            this.ReplaceReport3(new Dictionary<string, string>());

            return EErrorCode.NONE;
        }


        protected virtual EErrorCode MoveFileToTargetByUser(EServerQueryCmd cmd)
        {
            bool isOutputPath02 = false;

            bool isOutputPath03 = false;

            string outPath01 = string.Empty;

            string outPath02 = string.Empty;

            string outPath03 = string.Empty;

           

            if (this._uiSetting.IsManualRunMode)
            {
                outPath01 = GetPathWithFolder(UISetting.ManualOutPathInfo01);

                outPath02 = GetPathWithFolder(UISetting.ManualOutPathInfo02);
                isOutputPath02 = UISetting.ManualOutPathInfo02.EnablePath;

                outPath03 = GetPathWithFolder(UISetting.ManualOutPathInfo03);
                isOutputPath03 = UISetting.ManualOutPathInfo03.EnablePath;
            }
            else
            {
                outPath01 = GetPathWithFolder(UISetting.OutPathInfo01);

                outPath02 = GetPathWithFolder(UISetting.OutPathInfo02);
                isOutputPath02 = UISetting.OutPathInfo02.EnablePath;

                outPath03 = GetPathWithFolder(UISetting.OutPathInfo03);
                isOutputPath03 = UISetting.OutPathInfo03.EnablePath;
            }


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

            fileNameWithExt = fileNameWithoutExt + "." + this._uiSetting.TestResultFileExt;

            string outputPathAndFile01 = Path.Combine(outPath01, fileNameWithExt);

            string outputPathAndFile02 = Path.Combine(outPath02, fileNameWithExt);

            string outputPathAndFile03 = Path.Combine(outPath03, fileNameWithExt);

            MPIFile.CopyFile(this._fileFullNameRep, outputPathAndFile01);

            if (isOutputPath02)
            {
                MPIFile.CopyFile(this._fileFullNameRep, outputPathAndFile02);
            }

            if (isOutputPath03)
            {
                MPIFile.CopyFile(this._fileFullNameRep, outputPathAndFile03);
            }

            return EErrorCode.NONE;
        }

        protected void SetAutoMode(ref bool isOutputPath02, ref bool isOutputPath03, ref string outPath01, ref string outPath02, ref string outPath03, ref ETesterResultCreatFolderType type01, ref ETesterResultCreatFolderType type02, ref ETesterResultCreatFolderType type03)
        {
            isOutputPath02 = this._uiSetting.IsEnablePath02;

            isOutputPath03 = this._uiSetting.IsEnablePath03;

            outPath01 = this._uiSetting.TestResultPath01;

            outPath02 = this._uiSetting.TestResultPath02;

            outPath03 = this._uiSetting.TestResultPath03;

            type01 = this._uiSetting.TesterResultCreatFolderType01;

            type02 = this._uiSetting.TesterResultCreatFolderType02;

            type03 = this._uiSetting.TesterResultCreatFolderType03;
        }

        protected void SetManualMode(ref bool isOutputPath02, ref bool isOutputPath03, ref string outPath01, ref string outPath02, ref string outPath03, ref ETesterResultCreatFolderType type01, ref ETesterResultCreatFolderType type02, ref ETesterResultCreatFolderType type03)
        {
            isOutputPath02 = this._uiSetting.IsEnableManualPath02;

            isOutputPath03 = this._uiSetting.IsEnableManualPath03;

            outPath01 = this._uiSetting.ManualOutputPath01;

            outPath02 = this._uiSetting.ManualOutputPath02;

            outPath03 = this._uiSetting.ManualOutputPath03;

            type01 = this._uiSetting.ManualOutputPathType01;

            type02 = this._uiSetting.ManualOutputPathType02;

            type03 = this._uiSetting.ManualOutputPathType03;
        }

        protected virtual string GetPathWithFolder(string outPath, ETesterResultCreatFolderType type, string searchExt = "")
        {
            string folderName = "";
            if (searchExt == "")
            {
                searchExt = UISetting.TestResultFileExt;
            }
            switch (type)
            {
                default:
                case ETesterResultCreatFolderType.None:
                    break;
                case ETesterResultCreatFolderType.ByLotNumber:
                    folderName = this.UISetting.LotNumber;
                    outPath = Path.Combine(outPath, folderName);
                    break;
                case ETesterResultCreatFolderType.ByMachineName:
                    folderName = this.UISetting.MachineName;
                    outPath = Path.Combine(outPath, folderName);
                    break;
                case ETesterResultCreatFolderType.ByDataTime:
                    folderName = Path.Combine(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
                    outPath = Path.Combine(outPath, folderName);
                    break;
                case ETesterResultCreatFolderType.ByBarcode:
                    folderName = this.UISetting.Barcode;
                    outPath = Path.Combine(outPath, folderName);
                    break;
                case ETesterResultCreatFolderType.ByWaferID:
                    folderName = this.UISetting.WaferNumber;
                    outPath = Path.Combine(outPath, folderName);
                    break;
                case ETesterResultCreatFolderType.ByLotNumber_WaferID:
                    folderName = Path.Combine(this.UISetting.LotNumber, this.UISetting.WaferNumber);
                    outPath = Path.Combine(outPath, folderName);
                    break;

                case ETesterResultCreatFolderType.ByLot_WaferID_Times:
                    folderName = Path.Combine(this.UISetting.LotNumber, this.UISetting.WaferNumber);
                    //string rootFolder = Path.Combine(outPath, folderName);
                    string rootFolder = GetPathWithFolder(outPath, ETesterResultCreatFolderType.ByLotNumber_WaferID);

                    string timesFolder = GetTestTimes(rootFolder, searchExt);
                    if (timesFolder != "")
                    {
                        outPath = Path.Combine(rootFolder, timesFolder);
                    }
                    else
                    {
                        outPath = rootFolder;
                    }
                    break;
            }

            return outPath;
        }

        protected virtual string GetTestTimes(string folderPath, string searchExt)
        {
            string timesFolder = "Time1";
            string fileName = TestResultFileNameWithoutExt();

            if (searchExt == "pmap")//create map
            {
                fileName = UISetting.WaferNumber;
            }

            if (Directory.Exists(folderPath))
            {
                DirectoryInfo d = new DirectoryInfo(folderPath);//Assuming Test is your Folder
                FileInfo[] Files = d.GetFiles("*." + searchExt, SearchOption.AllDirectories);
                int cnt = 1;
                foreach (var file in Files)
                {
                    if (file.Name.StartsWith(fileName))
                    {
                        cnt++;
                    }
                }

                timesFolder = "Time" + cnt.ToString();
            }

            return timesFolder;
        }

        protected virtual EErrorCode MoveFileToTargetByUser2(EServerQueryCmd cmd)
        {
            return EErrorCode.NONE;
        }

        protected virtual EErrorCode MoveFileToTargetByUser3(EServerQueryCmd cmd)
        {
            return EErrorCode.NONE;
        }

        protected virtual EErrorCode MoveFileToPresamlingPath()
        {
            try
            {
                string lot = this._uiSetting.Barcode.Substring(0, 8);

                string wafer = this._uiSetting.Barcode.Replace(lot, string.Empty);

                string fullFileName = Path.Combine(this.UISetting.PreSamplingDataPath, "S" + wafer + ".csv");

                MPIFile.CopyFile(this._fileFullNameRep, fullFileName);
            }
            catch (Exception e)
            {
                Console.WriteLine("[ReportBase], MoveFileToPresamlingPath(), catch:" + e.ToString());
            }

            return EErrorCode.NONE;
        }

        protected virtual string GetPathWithFolder(string outPath, ETesterResultCreatFolderType type)
        {
            string folderName = "";
            switch (type)
            {
                case ETesterResultCreatFolderType.ByDataTime:
                    folderName = Path.Combine(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
                    outPath = Path.Combine(outPath, folderName);
                    break;
                case ETesterResultCreatFolderType.ByLotNumber:
                    folderName = this.UISetting.LotNumber;
                    outPath = Path.Combine(outPath, folderName);
                    break;
                case ETesterResultCreatFolderType.ByMachineName:
                    folderName = this.UISetting.MachineName;
                    outPath = Path.Combine(outPath, folderName);
                    break;
                case ETesterResultCreatFolderType.ByWaferID:
                    folderName = this.UISetting.WaferNumber;
                    outPath = Path.Combine(outPath, folderName);
                    break;
                case ETesterResultCreatFolderType.ByLotNumber_WaferID:
                    folderName = Path.Combine(this.UISetting.LotNumber, this.UISetting.WaferNumber);
                    outPath = Path.Combine(outPath, folderName);
                    break;
            }


            return outPath;
        }


        protected virtual string GetPathWithFolder(PathInfo pInfo)
        {
            string outPath = UISetting.GetPathWithFolder(pInfo);
            return outPath;
        }


        #endregion

        #region >>> Protected Method <<<

        protected void SetResultTitleByDefault()
        {
            this._resultTitleInfo.Clear();

            this._resultTitleInfo.AddResultData("TEST", "TEST");

            this._resultTitleInfo.AddResultData("COL", "PosX");

            this._resultTitleInfo.AddResultData("ROW", "PosY");

            this._resultTitleInfo.AddResultData("BIN", "BIN");

            this._resultTitleInfo.AddResultData("POLAR", "POLAR");

            this._resultTitleInfo.AppendResultData(this._resultData);
        }

        protected EErrorCode WriteReportHeadByDefault()
        {
            ////////////////////////////////////////////
            //Write Report Head
            ////////////////////////////////////////////
            this.WriteLine("FileName"+ this.SpiltChar.ToString()+ "\"" + Path.Combine(this.TestResultFileNameWithoutExt() + "."+this._uiSetting.TestResultFileExt) + "\"");

            this.WriteLine("UserID"+ this.SpiltChar.ToString()+ "\"" + this._uiSetting.UserID.ToString() + "_" + this._uiSetting.FormatName + "\"");

            this.WriteLine("TestTime"+ this.SpiltChar.ToString()+ "\"" + this._testerSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm:ss.fff") + "\"");

            this.WriteLine("EndTime"+ this.SpiltChar.ToString()+ "\"" + "\"");

            this.WriteLine("TesterModel"+ this.SpiltChar.ToString()+ "\"" + this._machineInfo.TesterModel + "/" + this._machineInfo.TesterSN + "\"");

            this.WriteLine("MachineName"+ this.SpiltChar.ToString()+ "\"" + this._uiSetting.MachineName + "\"");

            this.WriteLine("LotNumber"+ this.SpiltChar.ToString()+ "\"" + this._uiSetting.LotNumber + "\"");

            this.WriteLine("Substrate"+ this.SpiltChar.ToString()+ "\"" + this._uiSetting.Substrate + "\"");

            this.WriteLine("TaskFile"+ this.SpiltChar.ToString()+ "\"" + this._uiSetting.TaskSheetFileName + "\"");

            this.WriteLine("Recipe"+ this.SpiltChar.ToString()+ "\"" + this._uiSetting.ProductFileName + "\"");

            this.WriteLine("ConditionFileName"+ this.SpiltChar.ToString()+ "\"" + this._uiSetting.ConditionFileName + "\"");

            this.WriteLine("Filter Wheel"+ this.SpiltChar.ToString()+ "\"" + this._product.ProductFilterWheelPos.ToString() + "\"");

            this.WriteLine("LOPSaveItem"+ this.SpiltChar.ToString()+ "\"" + this._product.LOPSaveItem.ToString() + "\"");

            this.WriteLine("Operator"+ this.SpiltChar.ToString()+ "\"" + this._uiSetting.OperatorName + "\"");

            this.WriteLine("Samples"+ this.SpiltChar.ToString()+ "\"\"");

            this.WriteLine("Unit"+ this.SpiltChar.ToString()+ "\"9999\"");

            this.WriteLine("");

            ////////////////////////////////////////////
            //Write Test Item Info
            ////////////////////////////////////////////
            this.WriteLine("Item,Bias,BiasUnit,Time(ms),Compliance,CompUnit,Lower,Upper,Sqr,Gain,Offset,RltUnit");

            Dictionary<string, GainOffsetData> gainOffsetData = new Dictionary<string, GainOffsetData>();

            if (this.Product.TestCondition != null &&
                this.Product.TestCondition.TestItemArray != null &&
                this.Product.TestCondition.TestItemArray.Length > 0)
            {
                foreach (var testItem in this.Product.TestCondition.TestItemArray)
                {
                    if (testItem.MsrtResult == null || testItem.MsrtResult.Length == 0 || testItem.GainOffsetSetting == null)
                    {
                        continue;
                    }

                    foreach (var data in testItem.GainOffsetSetting)
                    {
                        if (!data.IsEnable || !data.IsVision)
                        {
                            continue;
                        }

                        gainOffsetData.Add(data.KeyName, data);
                    }

                    foreach (var msrtItem in testItem.MsrtResult)
                    {
                        if (!msrtItem.IsEnable || !msrtItem.IsVision || testItem.ElecSetting == null )
                        {
                            continue;
                        }

                        double sqr = 0.0d;

                        double gain = 1.0d;

                        double offset = 0.0d;

                        if (gainOffsetData.ContainsKey(msrtItem.KeyName))
                        {
                            sqr = gainOffsetData[msrtItem.KeyName].Square;

                            gain = gainOffsetData[msrtItem.KeyName].Gain;

                            offset = gainOffsetData[msrtItem.KeyName].Offset;
                        }

                        string line = string.Empty;

                        line += msrtItem.Name;

                        if (testItem.ElecSetting.Length < 1)
                        {
                            line += this.SpiltChar.ToString() + testItem.ElecSetting[0].ForceValue.ToString();

                            line += this.SpiltChar.ToString() + testItem.ElecSetting[0].ForceUnit;

                            line += this.SpiltChar.ToString() + testItem.ElecSetting[0].ForceTime.ToString();

                            line += this.SpiltChar.ToString() + testItem.ElecSetting[0].MsrtProtection.ToString();
                            
                        }
                        else
                        {
                            for (int i = 0; i < 4; ++i)
                            {
                                line += this.SpiltChar.ToString();
                            }
                        }

                        line += this.SpiltChar.ToString() + msrtItem.Unit;

                        line += this.SpiltChar.ToString() + msrtItem.MinLimitValue;

                        line += this.SpiltChar.ToString() + msrtItem.MaxLimitValue;

                        line += this.SpiltChar.ToString() + sqr.ToString();

                        line += this.SpiltChar.ToString() + gain.ToString();

                        line += this.SpiltChar.ToString() + offset.ToString();

                        line += this.SpiltChar.ToString() + testItem.ElecSetting[0].MsrtUnit;

                        this.WriteLine(line);
                    }
                }
            }

            this.WriteLine("");

            ////////////////////////////////////////////
            //Write Gain Offset Info
            ////////////////////////////////////////////
            string lineItem = "Result Item";

            string lineType = "Type";

            string lineSquare = "Square";

            string lineGain = "Gain";

            string lineOffset = "Offset";

            string lineGain2 = "Gain2";

            string lineOffset2 = "Offset2";

            foreach (var item in gainOffsetData)
            {
                lineItem += this.SpiltChar.ToString()+ item.Value.Name;

                lineType += this.SpiltChar.ToString()+ item.Value.Type;

                lineSquare += this.SpiltChar.ToString()+ item.Value.Square;

                lineGain += this.SpiltChar.ToString()+ item.Value.Gain;

                lineOffset += this.SpiltChar.ToString()+ item.Value.Offset;

                lineGain2 += this.SpiltChar.ToString()+ item.Value.Gain2;

                lineOffset2 += this.SpiltChar.ToString()+ item.Value.Offset2;
            }

            this.WriteLine(lineItem);

            this.WriteLine(lineType);

            this.WriteLine(lineSquare);

            this.WriteLine(lineGain);

            this.WriteLine(lineOffset);

            this.WriteLine(lineGain2);

            this.WriteLine(lineOffset2);

            this.WriteLine("");

            ////////////////////////////////////////////
            //Write Result Item Title
            ////////////////////////////////////////////
            this.WriteLine(this._resultTitleInfo.TitleStr);

            return EErrorCode.NONE;
        }

        protected EErrorCode RewriteReportByDefault()
        {
            Dictionary<string, string> replaceData = new Dictionary<string, string>();

            string endTime = "EndTime,\"" + this.TesterSetting.EndTestTime.ToString("yyyy/MM/dd HH:mm:ss.fff") + "\"";

            replaceData.Add("EndTime,\"" + "\"", endTime);

            string testCount = "Samples,\"" + this._checkColRowKey.Count.ToString() + "\"";

            replaceData.Add("Samples,\"" + "\"", testCount);

            this.ReplaceReport(replaceData);

            return EErrorCode.NONE;
        }

        protected EErrorCode MoveWAFFileToTarget(EServerQueryCmd cmd)
        {
            string outPath01 = this._uiSetting.WAFOutputPath01;

            string outPath02 = this._uiSetting.WAFOutputPath02;

            string outPath03 = this._uiSetting.WAFOutputPath03;

            bool isOutputPath02 = this._uiSetting.IsEnableWAFPath02;

            bool isOutputPath03 = this._uiSetting.IsEnableWAFPath03;

            ETesterResultCreatFolderType type01 = ETesterResultCreatFolderType.None;

            ETesterResultCreatFolderType type02 = ETesterResultCreatFolderType.None;

            ETesterResultCreatFolderType type03 = ETesterResultCreatFolderType.None;


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

            fileNameWithExt = fileNameWithoutExt + "." + this._uiSetting.WAFTestResultFileExt;

            string outputPathAndFile01 = Path.Combine(outPath01, fileNameWithExt);

            string outputPathAndFile02 = Path.Combine(outPath02, fileNameWithExt);

            string outputPathAndFile03 = Path.Combine(outPath03, fileNameWithExt);

            MPIFile.CopyFile(this._fileFullNameRep2, outputPathAndFile01);

            if (isOutputPath02)
            {
                MPIFile.CopyFile(this._fileFullNameRep2, outputPathAndFile02);
            }

            if (isOutputPath03)
            {
                MPIFile.CopyFile(this._fileFullNameRep2, outputPathAndFile03);
            }

            return EErrorCode.NONE;
        }

        protected void WriteLine(string line)
        {
            if (this._sw != null)
            {
                this._sw.WriteLine(line);

                this._sw.Flush();
            }
        }

        protected void WriteLine2(string line)
        {
            if (this._sw2 != null)
            {
                this._sw2.WriteLine(line);

                this._sw2.Flush();
            }
        }

        protected void WriteLine3(string line)
        {
            if (this._sw3 != null)
            {
                this._sw3.WriteLine(line);

                this._sw3.Flush();
            }
        }

        protected void ReplaceReport(Dictionary<string, string> replaceData, bool isSkipWritingTestCount = false)
        {
            if (File.Exists(this._fileFullNameTmp))
            {
                this.ReplaceReportData(replaceData, this._fileFullNameTmp, this._fileFullNameRep, isSkipWritingTestCount);
            }
        }

        protected void ReplaceReport2(Dictionary<string, string> replaceData, bool isSkipWritingTestCount = false)
        {
            if (File.Exists(this._fileFullNameTmp2))
            {
                this.ReplaceReportData(replaceData, this._fileFullNameTmp2, this._fileFullNameRep2, isSkipWritingTestCount);
            }
        }

        protected void ReplaceReport3(Dictionary<string, string> replaceData, bool isSkipWritingTestCount = false)
        {
            if (File.Exists(this._fileFullNameTmp3))
            {
                this.ReplaceReportData(replaceData, this._fileFullNameTmp3, this._fileFullNameRep3, isSkipWritingTestCount);
            }
        }

        protected EErrorCode WriteoutMap(string inputFile = "", string outputFile = "", bool isappend = false, bool isTempMap = false)
        {
            if (this.UISetting.IsEnableSaveMap)
            {
                bool isRawData = false;
                int testCount = 0;
                int rawLineCount = 0;

                if (inputFile == "")
                {
                    string fileName = this.TestResultFileNameWithoutExt();

                    inputFile = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, fileName + ReportBase.EXTEN_TMP1);
                }

                if (outputFile == "")
                {
                    outputFile = UISetting.MapPath;

                    string fileName = UISetting.WaferNumber + ".pmap";
                    string mapFolder = "";
                    ETesterResultCreatFolderType ftype = UISetting.MapPathInfo.FolderType;
                    if (isTempMap)
                    {
                        fileName = UISetting.WaferNumber + ".cmap";
                        
                        if(UISetting.MapPathInfo.FolderType == ETesterResultCreatFolderType.ByLot_WaferID_Times)
                        {
                            ftype = ETesterResultCreatFolderType.ByLotNumber_WaferID;
                        }                            
                        mapFolder = GetPathWithFolder(outputFile, ftype);
                    }
                    else
                    {
                        mapFolder = GetPathWithFolder(outputFile, ftype, "pmap");
                    }

                    //GetPathWithFolder
                    //string mapFolder = Path.Combine(outputFile, UISetting.LotNumber);
                    

                    if (!Directory.Exists(mapFolder))
                    {
                        Directory.CreateDirectory(mapFolder);
                    }

                    outputFile = Path.Combine(mapFolder, fileName);
                }

                Console.WriteLine("[ReportBase], WriteoutMap");

                string tempOutputPath = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, "tempMAP.csv");

                if (!File.Exists(inputFile))
                {
                    Console.WriteLine("[ReportBase], WriteoutMap, inputFile not Exit");
                    return EErrorCode.NONE;
                }

                using (StreamReader sr = new StreamReader(inputFile, this._reportData.Encoding))
                {
                    using (StreamWriter sw = new StreamWriter(tempOutputPath, isappend, this._reportData.Encoding))
                    {
                        sw.WriteLine("X,Y,Bin");
                        if (this.TitleStrKey == "")
                        {
                            this.TitleStrKey = ResultTitleInfo.TitleStr;
                        }

                        HeaderFinderBase hf = new HeaderFinderBase(this.TitleStrKey, TitleStrShift);

                        while (sr.Peek() >= 0)
                        {
                            string line = sr.ReadLine();

                            if (isRawData)
                            {
                                rawLineCount++;

                                //if (this._testerSetting.IsCheckRowCol)
                                //{
                                    string[] rawData = line.Split(this.SpiltChar);

                                    string colrowKey = ColRowKeyMaker(rawData);

                                    string outStr = "";
                                    // 把 row.col 和 checkRowCol "raw line count " 相同時, 才會push資料,解決當點重測row,col的問題
                                    if (this._checkColRowKey.ContainsKey(colrowKey) && this._checkColRowKey[colrowKey] == rawLineCount)
                                    {
                                        outStr += rawData[this._resultTitleInfo.ColIndex].ToString() + "," +
                                            rawData[this._resultTitleInfo.RowIndex].ToString() + ",";//always use "," in this file

                                        if (this._resultTitleInfo.BinIndex >= 0)
                                        {
                                            string bin = rawData[this._resultTitleInfo.BinIndex];
                                            int binNum = 0;
                                            if (int.TryParse(bin, out  binNum))
                                            {
                                                outStr += binNum.ToString("0");
                                            }
                                            else
                                            {
                                                outStr += "0";
                                            }
                                        }
                                        testCount++;

                                        sw.WriteLine(outStr);
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                //}
                            }
                            else
                            {
                                
                                //if (line == this._resultTitleInfo.TitleStr)
                                if (hf.CheckIfRowData(line))
                                {
                                    isRawData = true;
                                }
                            }
                        }

                    }
                }

                try
                {
                    File.Copy(tempOutputPath, outputFile, true);
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ReportBase], WriteoutMap Err, catch:" + e.ToString());
                }
            }

            return EErrorCode.NONE;
        }

  
        #endregion
    }
}
