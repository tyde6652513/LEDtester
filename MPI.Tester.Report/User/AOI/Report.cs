using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using MPI.Tester.Data;
using MPI.Tester.TestServer;

namespace MPI.Tester.Report.User.AOI
{
	class Report : ReportBase
	{

        //private StreamWriter _sw1;
        //private StreamWriter _sw2;
        //private StreamWriter _sw3;
        //private StreamWriter _sw4;

        Dictionary<string, DieInfo> PosGroupDic ;
        private int groupSize = 1;
		public Report(List<object> objs, bool isReStatistic)
			: base(objs, isReStatistic)
		{
            
		}
        #region >>protected override<<
        protected override void SetResultTitle()
		{
			//this.SetResultTitleByDefault();
            this.ResultTitleInfo.SetResultData(this.UISetting.UserDefinedData.ResultItemNameDic);
            Dictionary<string, DieInfo>tmp = GetGroupInfo();//接續點測會用到
            if(tmp != null)
            {
                PosGroupDic = tmp;
            }
            
		}
        
        protected override EErrorCode WriteReportHeadByUser()
        {
            PosGroupDic = GetGroupInfo();
           
            
            ////////////////////////////////////////////
            //Write Report Head
            ////////////////////////////////////////////
            this.WriteLine("FileName,\"" + this.TestResultFileNameWithoutExt() + "," + this.UISetting.TestResultFileExt + "\"");//Path.Combine();

            this.WriteLine("UserID,\"" + this.UISetting.UserID.ToString() + "_" + this.UISetting.FormatName + "\"");

            this.WriteLine("TestTime,\"" + this.TesterSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm") + "\"");

            this.WriteLine("EndTime,\"" + "\"");

            this.WriteLine("TesterModel,\"" + MachineInfo.TesterModel + "/" + MachineInfo.TesterSN + "\"");

            this.WriteLine("MachineName,\"" + this.UISetting.MachineName + "\"");

            this.WriteLine("LotNumber,\"" + this.UISetting.LotNumber + "\"");

            this.WriteLine("Substrate,\"" + this.UISetting.Substrate + "\"");

            this.WriteLine("TaskFile,\"" + this.UISetting.TaskSheetFileName + "\"");

            this.WriteLine("Recipe,\"" + this.UISetting.ProductFileName + "\"");

            this.WriteLine("ConditionFileName,\"" + this.UISetting.ConditionFileName + "\"");

            this.WriteLine("Filter Wheel,\"" + Product.ProductFilterWheelPos.ToString() + "\"");

            this.WriteLine("LOPSaveItem,\"" + Product.LOPSaveItem.ToString() + "\"");

            this.WriteLine("Operator,\"" + this.UISetting.OperatorName + "\"");

            this.WriteLine("Temperature,\"" + this.UISetting.ChuckTemprature.ToString("0.#") + "\"");

            this.WriteLine("Samples,\"" + "\"");

            this.WriteLine("Unit,\"9999\"");

            this.WriteLine("");

            ////////////////////////////////////////////
            //Write Test Item Info
            ////////////////////////////////////////////
            this.WriteLine("Msrt Item,Force Item,Bias,BiasUnit,Time(ms),Compliance,CompUnit,Lower,Upper,Sqr,Gain,Offset,RltUnit");

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
                        if (!msrtItem.IsEnable || !msrtItem.IsVision || testItem.ElecSetting == null)
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

                        line += "," + testItem.Name;

                        if (testItem is IZTestItem)
                        {
                            if (testItem.ElecSetting[0].IsFloatForceValue)
                            {
                                line += "," + (testItem as IZTestItem).RefIrName + "*" + (testItem as IZTestItem).Factor.ToString("0.0#") + " - " + (testItem as IZTestItem).Offset.ToString("0.0#");
                            }
                            else 
                            {
                                line += ",-" + testItem.ElecSetting[0].ForceValue.ToString();
                            }
                        }
                        else if (testItem is VRTestItem )
                        {
                            if (testItem.ElecSetting[0].IsFloatForceValue)
                            {
                                line += "," + (testItem as VRTestItem).RefVzName + "*" + (testItem as VRTestItem).Factor.ToString("0.0#") + " - " + (testItem as VRTestItem).Offset.ToString("0.0#");
                            }
                            else
                            {
                                line += ",-" + testItem.ElecSetting[0].ForceValue.ToString();
                            }
                        }
                        else 
                        {
                            line += "," + testItem.ElecSetting[0].ForceValue.ToString();
                        }

                        line += "," + testItem.ElecSetting[0].ForceUnit;

                        line += "," + testItem.ElecSetting[0].ForceTime.ToString();

                        line += "," + testItem.ElecSetting[0].MsrtProtection.ToString();

                        line += "," + testItem.ElecSetting[0].MsrtUnit;

                        line += "," + msrtItem.MinLimitValue;

                        line += "," + msrtItem.MaxLimitValue;

                        line += "," + sqr.ToString();

                        line += "," + gain.ToString();

                        line += "," + offset.ToString();

                        line += "," + testItem.ElecSetting[0].MsrtUnit;

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
                lineItem += "," + item.Value.Name;

                lineType += "," + item.Value.Type;

                lineSquare += "," + item.Value.Square;

                lineGain += "," + item.Value.Gain;

                lineOffset += "," + item.Value.Offset;

                lineGain2 += "," + item.Value.Gain2;

                lineOffset2 += "," + item.Value.Offset2;
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
            this.WriteLine(ResultTitleInfo.TitleStr);

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

            int index = 0;

            foreach (KeyValuePair<string, string> item in this.ResultTitleInfo)
            {

                switch (item.Key)
                {
                    case "BIN_CODE":
                        line += binCode;
                        break;
                    case "BIN_NUMBER":
                        line += binNumber.ToString();
                        break;
                    case "BIN_GRADE":
                        line += binGrade.ToString();
                        break;
                    case "TestGroup":

                        {
                            string pos = data["COL"] + "_" + data["ROW"];
                            if (PosGroupDic != null && PosGroupDic.ContainsKey(pos))
                            {
                                line += PosGroupDic[pos].GroupStr;
                            }
                                //PosGroupDic
                        }
                        //if (AcquireData.ChipInfo.GroupName != null)
                        //{
                        //    line += AcquireData.ChipInfo.GroupName;//還不確定是傳文字或是數字
                        //}
                        break;
                    case "TestGroupIndex":
                        //line += AcquireData.ChipInfo.GroupIndex.ToString();
                        break;
                    case "ISALLPASS":
                        if (data[item.Key] == 1)
                        {
                            line += "1";
                        }
                        else 
                        {
                            line += "0";
                        }
                        break;
                    case "NOWTIME":
                        line += DateTime.Now.ToString("yyyyMMddHHmmss"); 
                        break;
                    default:
                        {
                            if (data.ContainsKey(item.Key))
                            {
                                string format = string.Empty;

                                if (this.UISetting.UserDefinedData[item.Key] != null)
                                {
                                    format = this.UISetting.UserDefinedData[item.Key].Formate;
                                }
                                if (data[item.Key] != double.NegativeInfinity &&
                                    data[item.Key] != double.PositiveInfinity &&
                                    data[item.Key] != double.NaN)
                                {
                                    line += data[item.Key].ToString(format);
                                }
                                else
                                {
                                    line += "NaN";
                                }
                            }
                        }
                        break;
                }
                index++;

                if (index != this.ResultTitleInfo.ResultCount)
                {
                    line += ",";
                }
            }

            this.WriteLine(line);

            return EErrorCode.NONE;
        }
        //protected override EErrorCode RewriteReportByUser()
        //{
        //    RewriteReportByDefault();

        //    return EErrorCode.NONE;
        //}
        protected override EErrorCode RewriteReportByUser()
        {
            Dictionary<string, string> replaceData = new Dictionary<string, string>();

            string endTime = "EndTime,\"" + this.TesterSetting.EndTestTime.ToString("yyyy/MM/dd HH:mm") + "\"";

            replaceData.Add("EndTime,\"" + "\"", endTime);

            string testCount = "Samples,\"" + this._checkColRowKey.Count.ToString() + "\"";

            replaceData.Add("Samples,\"" + "\"", testCount);

            string realTarPath = this.FileFullNameRep;

            this.FileFullNameRep = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, this.TestResultFileNameWithoutExt() + ".OutputTemp");

            this.ReplaceReport(replaceData);

            this.ReplaceReportWithGroup( FileFullNameRep, realTarPath,FileFullNameRep2);//會重新再掃一次，但可確保ReplaceReport該執行的功能有被正常執行

            FileFullNameRep = realTarPath;

            return EErrorCode.NONE;
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
                isOutputPath02 = this.UISetting.IsEnableManualPath02;

                isOutputPath03 = this.UISetting.IsEnableManualPath03;

                outPath01 = this.UISetting.ManualOutputPath01;

                outPath02 = this.UISetting.ManualOutputPath02;

                outPath03 = this.UISetting.ManualOutputPath03;

                type01 = this.UISetting.ManualOutputPathType01;

                type02 = this.UISetting.ManualOutputPathType02;

                type03 = this.UISetting.ManualOutputPathType03;
            }
            else
            {
                isOutputPath02 = this.UISetting.IsEnablePath02;

                isOutputPath03 = this.UISetting.IsEnablePath03;

                outPath01 = this.UISetting.TestResultPath01;

                outPath02 = this.UISetting.TestResultPath02;

                outPath03 = this.UISetting.TestResultPath03;

                type01 = this.UISetting.TesterResultCreatFolderType01;

                type02 = this.UISetting.TesterResultCreatFolderType02;

                type03 = this.UISetting.TesterResultCreatFolderType03;
            }

            if (type01 == ETesterResultCreatFolderType.ByLotNumber)
            {
                outPath01 = Path.Combine(outPath01, this.UISetting.LotNumber);
            }
            else if (type01 == ETesterResultCreatFolderType.ByMachineName)
            {
                outPath01 = Path.Combine(outPath01, this.UISetting.MachineName);
            }
            else if (type01 == ETesterResultCreatFolderType.ByDataTime)
            {
                outPath01 = Path.Combine(outPath01, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
            }

            if (type02 == ETesterResultCreatFolderType.ByLotNumber)
            {
                outPath02 = Path.Combine(outPath02, this.UISetting.LotNumber);
            }
            else if (type02 == ETesterResultCreatFolderType.ByMachineName)
            {
                outPath02 = Path.Combine(outPath02, this.UISetting.MachineName);
            }
            else if (type02 == ETesterResultCreatFolderType.ByDataTime)
            {
                outPath02 = Path.Combine(outPath02, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
            }

            if (type03 == ETesterResultCreatFolderType.ByLotNumber)
            {
                outPath03 = Path.Combine(outPath03, this.UISetting.LotNumber);
            }
            else if (type03 == ETesterResultCreatFolderType.ByMachineName)
            {
                outPath03 = Path.Combine(outPath03, this.UISetting.MachineName);
            }
            else if (type03 == ETesterResultCreatFolderType.ByDataTime)
            {
                outPath03 = Path.Combine(outPath03, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
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

            fileNameWithExt = fileNameWithoutExt + "." + this.UISetting.TestResultFileExt;



            string outputPathAndFile01 = Path.Combine(outPath01, fileNameWithExt);

            string outputPathAndFile02 = Path.Combine(outPath02, fileNameWithExt);

            string outputPathAndFile03 = Path.Combine(outPath03, fileNameWithExt);



            MPIFile.CopyFile(FileFullNameRep, outputPathAndFile01);

            if (isOutputPath02)
            {
                if (outPath01 == outPath02)//會被覆蓋
                {
                    outputPathAndFile02 = Path.Combine(outPath02, fileNameWithoutExt + "_sor." + this.UISetting.TestResultFileExt);
                }
                MPIFile.CopyFile(FileFullNameRep2, outputPathAndFile02);
            }

            if (isOutputPath03)
            {
                MPIFile.CopyFile(FileFullNameRep, outputPathAndFile03);
            }

            return EErrorCode.NONE;
        }

        protected override EErrorCode PushDataByUserS04(Dictionary<string, double> data, bool isMS)
        {

            string posKey = data["COL"] + "_" + data["ROW"];
            string chipID = "_";
            if (PosGroupDic != null && PosGroupDic.ContainsKey(posKey))
            {
                chipID += PosGroupDic[posKey].GroupStr;
            }

            string pos = "_C(" + data["COL"].ToString() + ")_R(" + data["ROW"].ToString() + ")";

            if (chipID == "_")
            {
                chipID = pos;
            }

            string path = GetFullPathWithFolder(this.UISetting.SweepOutputPath, this.UISetting.SweepOutputPathType);

            if (this.UISetting.IsEnableSweepPath)
            {
                if (this.AcquireData.ElecSweepDataSet.Count != 0)
                {
                    ElecSweepDataSet sweepSet = this.AcquireData.ElecSweepDataSet;
                    foreach (ElecSweepData sData in sweepSet)
                    {
                        if (EErrorCode.NONE != DefaultPushVI_IVSweep(sData, chipID, path))
                        {
                            return EErrorCode.SaveFileFail;
                        }
                    }
                }
            }

            return EErrorCode.NONE;
        }

    

        #endregion

        #region >>for output 1<<
        private EErrorCode ReplaceReportWithGroup(string sourcePath, string tarPath1,string tarPath2)
        {
            //PosGroupDic;

            //Dictionary<string, bool> GroupIsPassDic = new Dictionary<string, bool>();

            int outputDataCounter = 1;

            //Dictionary<string, DieInfo> PosGroupDic = GetGroupInfo();

            GroupInfoManager GM = CheckIfAllGroupPass(sourcePath, PosGroupDic);//同一Group不同Die可能不是被連續的點測，因此要全部先掃過

            bool isOutputPath02 = false;
            if (this.UISetting.IsManualRunMode)
            {
                isOutputPath02 = this.UISetting.IsEnableManualPath02;
            }
            else 
            {
                isOutputPath02 = this.UISetting.IsEnablePath02;
            }


            if (File.Exists(sourcePath))
            {
                /////////////////////////////////////////////////////////////////////////////////////////////////////////
                //ReplaceReportData(Dictionary<string, string> replaceData, string inputFile, string outputFile)
                Dictionary<string, string> data = new Dictionary<string, string>();

                
                ///////////////////////////////////////////////////////
                //Replace Data And Check Row Col
                ///////////////////////////////////////////////////////
                StreamWriter sw = new StreamWriter(tarPath1);

                StreamWriter sw2 = new StreamWriter(tarPath2);

                StreamReader sr = new StreamReader(sourcePath);

                bool isRawData = false;

                int groupBinIndex = -1;

                //int PassIndex = -1;

                int GroupStrIndex = -1;

                int colIndex = this.ResultTitleInfo.ColIndex;

                int rowIndex = this.ResultTitleInfo.RowIndex;


                //string nowGroupStr = "";

                List<string[]> readStrArrList = new List<string[]>();

                // 開始比對ColRowKey並寫檔
                while (sr.Peek() >= 0)
                {
                    string line = sr.ReadLine();

                    if (isRawData)
                    {
                        string[] rawData = line.Split(',');

                        string colrowKey = rawData[this.ResultTitleInfo.ColIndex].ToString() + "_" + rawData[this.ResultTitleInfo.RowIndex].ToString();

                        string nowGroupName = "";
                        //redStrArrList
                        int p = sr.Peek();

                        if (PosGroupDic != null)
                        {
                            if (PosGroupDic.ContainsKey(colrowKey))
                            {
                                rawData[GroupStrIndex] = PosGroupDic[colrowKey].GroupStr;

                                nowGroupName = rawData[GroupStrIndex].Split('.')[0];
                            }
                        }


                        if (GM.NameGroupDic != null)
                        {
                            if (GM.NameGroupDic.ContainsKey(nowGroupName))
                            {
                                switch (this.TesterSetting.GroupBinRule)
                                {
                                    case EGroupBinRule.MAX:
                                    default:
                                        rawData[groupBinIndex] = GM.NameGroupDic[nowGroupName].MaxGroupBin;
                                        break;
                                    case EGroupBinRule.MIN:
                                        rawData[groupBinIndex] = GM.NameGroupDic[nowGroupName].MinGroupBin;
                                        break;
                                    case EGroupBinRule.SAME:
                                        rawData[groupBinIndex] = GM.NameGroupDic[nowGroupName].SameBin;
                                        break;
                                }
                                
                            }
                            else
                            {
                                rawData[groupBinIndex] = "0";
                            }
                        }

                        line = string.Empty;
                        foreach (string str in rawData)
                        {
                            line += str + ",";
                        }

                        line = line.TrimEnd(',');

                        sw.WriteLine(line);

                        sw2.WriteLine(line);

                        outputDataCounter++;
                    }
                    else
                    {
                        if (line == this.ResultTitleInfo.TitleStr)
                        {
                            string[] strArr = line.Split(new char[] { ',' });

                            #region >>find index<<
                            for (int i = 0; i < strArr.Length; ++i)
                            {
                                //this.UISetting.UserDefinedData.ResultItemNameDic
                                if (strArr[i] == this.UISetting.UserDefinedData.ResultItemNameDic["TestGroupBin"])
                                {
                                    groupBinIndex = i;
                                }
                                else if (strArr[i] == this.UISetting.UserDefinedData.ResultItemNameDic["TestGroup"])
                                {
                                    GroupStrIndex = i;
                                }

                            }
                            #endregion

                            sw2.WriteLine("GROUP_SIZE," + groupSize.ToString());
                            isRawData = true;
                            
                        }
                        //else 
                        //{
                        //    sw2.WriteLine(line);
                        //}
                        sw2.WriteLine(line);
                        sw.WriteLine(line);

                        if (isRawData)
                        {
                            sw2.WriteLine("MapData");
                        }

                        
                    }
                }
                
                foreach (GroupInfo gData in GM.NameGroupDic.Values)
                {
                    if (isOutputPath02 && !gData.IsPass)
                    {
                        foreach (string str in gData.GetFailStrList(ref outputDataCounter, this.TesterSetting.GroupBinRule))
                        {
                            sw2.WriteLine(str);
                        }
                        //sw2.WriteLine(gData.GetStr(outputDataCounter, "1"));
                        //outputDataCounter++;
                    }
                }

                sr.Close();

                sr.Dispose();

                sw.Close();

                sw.Dispose();

                sw2.Close();

                sw2.Dispose();
            }


            /////////////////////////////////////////////////////////////////////////////////////////////////////////

            return EErrorCode.NONE;
        }

        private GroupInfoManager CheckIfAllGroupPass(string sourcePath, Dictionary<string, DieInfo> PosGroupDic)
        {
            List<int> ignoreIndexList = new List<int>();
            GroupInfoManager GM = new GroupInfoManager(this.UISetting.UserDefinedData.ResultItemNameDic);
            GM.groupSize = this.groupSize;
            if (File.Exists(sourcePath) && PosGroupDic != null)
            {
                StreamReader sr = new StreamReader(sourcePath);

                bool isRawData = false;
                int isPassIndex = -1;
                int dataLength = 0;
                int groupBinIndex = -1;
                int GroupStrIndex = -1;

                while (sr.Peek() >= 0)
                {
                    string line = sr.ReadLine();

                    if (isRawData)
                    {
                        string[] rawData = line.Split(',');
                        string colrowKey = rawData[this.ResultTitleInfo.ColIndex].ToString() + "_" + rawData[this.ResultTitleInfo.RowIndex].ToString();
                        string groupName = "";
                        int GroupInex = -1;

                        if (PosGroupDic != null)
                        {
                            if (PosGroupDic.ContainsKey(colrowKey))
                            {
                                groupName = PosGroupDic[colrowKey].GroupStr.Split('.')[0];
                                int.TryParse(PosGroupDic[colrowKey].GroupStr.Split('.')[1],out GroupInex);

                                bool isPass = (rawData[isPassIndex] == "1");

                                if (!GM.NameGroupDic.ContainsKey(groupName))
                                {
                                    GroupInfo GI = new GroupInfo(groupName, this.ResultTitleInfo.ColIndex, this.ResultTitleInfo.RowIndex, isPassIndex, GroupStrIndex, groupBinIndex, dataLength, PosGroupDic, groupSize);
                                    GI.BinIndex = ResultTitleInfo.BinIndex;
                                    //GI.IgnoreIndex = GM.IgnoreIndex;
                                    GM.NameGroupDic.Add(groupName, GI);
                                    isPass = true;
                                }


                                if (GM.NameGroupDic[groupName].PassCount >= 0 && isPass &&
                                GroupInex > 0 && GroupInex <= groupSize)//pass
                                {
                                    GM.NameGroupDic[groupName].PassCount++;
                                    GM.NameGroupDic[groupName].PushData(GroupInex - 1, rawData);
                                    //if (GM.NameGroupDic[groupName].IsPass)
                                    //{
                                    //    GM.NameGroupDic.Remove(groupName);
                                    //}
                                }
                                else
                                {
                                    GM.NameGroupDic[groupName].PassCount = -1;
                                }


                            }
                        }
                    }
                    else
                    {
                        if (line == this.ResultTitleInfo.TitleStr)
                        {
                            string[] strArr = line.Split(new char[] { ',' });
                            dataLength = strArr.Length;
                            #region >>find index<<
                            Dictionary<string, string> KeyNameDic = this.UISetting.UserDefinedData.ResultItemNameDic;

                            for (int i = 0; i < strArr.Length; ++i)
                            {
                                if (strArr[i] == KeyNameDic["ISALLPASS"])
                                {
                                    isPassIndex = i;
                                    ignoreIndexList.Add(i);
                                }
                                if (strArr[i] == this.UISetting.UserDefinedData.ResultItemNameDic["TestGroupBin"])
                                {
                                    groupBinIndex = i;
                                }
       
                                else if (strArr[i] == this.UISetting.UserDefinedData.ResultItemNameDic["TestGroup"])
                                {
                                    GroupStrIndex = i;
                                }
                                else if (GM.IsNameInIgnoreList(strArr[i]))
                                {
                                    ignoreIndexList.Add(i);
                                }

                            }

                            Console.WriteLine("[AO-Inc Report], ColIndex:" + this.ResultTitleInfo.ColIndex.ToString());
                            Console.WriteLine("[AO-Inc Report], RowIndex:" + this.ResultTitleInfo.RowIndex.ToString());
                            Console.WriteLine("[AO-Inc Report], DataLength:" + dataLength.ToString());


                            GM.IgnoreIndex.Clear();
                            GM.IgnoreIndex.AddRange(ignoreIndexList);
                            #endregion

                            isRawData = true;
                        }
                    }
                }

                sr.Close();

                sr.Dispose();
            }
            return GM;
        }

        private Dictionary<string, DieInfo> GetGroupInfo()
        {
            Dictionary<string, DieInfo> PosGroupDic = null;

            if (UISetting.IsEnableMergeAOIFile)
            {
                //string strAdd = @"C:\FAE\Product\" + UISetting.ProberRecipeName + ".eva.csv";
                //string oriGroupPath = @"C:\FAE\Product\" + UISetting.ProberRecipeName + ".csv";
                string oriGroupPath =  Path.Combine(Constants.Paths.PROBER_PRODUCT_DIR, UISetting.ProberRecipeName + ".wme.csv"); 
                Console.WriteLine("[AO-Inc Report], ReplaceReportWithGroup(), oriGroupPath :" + oriGroupPath);
                if (UISetting.IsEnableLaodGroupData)
                {
                    oriGroupPath = Path.Combine(UISetting.GroupDataPath, UISetting.ProberRecipeName + ".wme.csv"); //UISetting.GroupDataPath + "\\" + UISetting.ProberRecipeName + ".csv";
                }

                //oriGroupPath = @"C:\FAE\Product\aaa.eva.csv";//4顆1Group 20180308 David測試先寫死
                //oriGroupPath = @"C:\FAE\Product\bbb.eva.csv";//1顆1Group

                //string oriGroupPath = @"C:\FAE\Product\aaa.eva.csv";
                string copyGPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, "OCR.csv");

                PosGroupDic = new Dictionary<string, DieInfo>();

                int Pcoord = 1;//後續會要求顧客提供的 map 檔為 第1象限
                try
                {
                    File.Copy(oriGroupPath, copyGPath,true);
                    Console.WriteLine("[AO-Inc Report], ReplaceReportWithGroup(), copy to :" + copyGPath);
                }
                catch (Exception e)
                {
                    Console.WriteLine("[AO-Inc Report], ReplaceReportWithGroup(), Err:" + e.Message);
                    return null;
                }
                //StreamWriter swG = new StreamWriter(@"C:\MPI\LEDTester\Log\posGroup.csv");

                StreamReader srG = new StreamReader(copyGPath);


                //Console.WriteLine("[AO-Inc Report], X_shift:" + AcquireData.ChipInfo.TransCol.ToString());
                //Console.WriteLine("[AO-Inc Report], Y_shift:" + AcquireData.ChipInfo.TransRow.ToString());

                while (srG.Peek() >= 0)
                {
                    string line = srG.ReadLine();

                    string[] strArr = line.Split(',');

                    DieInfo di = new DieInfo(strArr[4]);

                    if (groupSize < di.GroupIndex)
                    {
                        groupSize = di.GroupIndex;
                    }

                    int x = int.Parse(strArr[0]);
                    int y = int.Parse(strArr[1]);

                    ChangeCoord(Pcoord, ref x, ref y);

                    string key = x.ToString() + "_" + y.ToString();

                    PosGroupDic.Add(key, di);

                    //swG.WriteLine(key + "," + di.GroupStr);
                }
                srG.Close();

                srG.Dispose();

                //swG.Close();
                //swG.Dispose();

                File.Delete(copyGPath);
            }
            if (PosGroupDic != null)
            {
                Console.WriteLine("[AO-Inc Report], WriteReportHeadByUser PosGroupDic count :" + PosGroupDic.Count);
            }
            else
            {
                Console.WriteLine("[AO-Inc Report], WriteReportHeadByUser PosGroupDic is null");
            }

            return PosGroupDic;
        }

        private void ChangeCoord(int Pcoord, ref int x, ref int y)//from wme map => tester coord
        {
            int dx = x - AcquireData.ChipInfo.TransCol;
            int dy = y - AcquireData.ChipInfo.TransRow;

            //全部轉到第一象限
            //switch (Pcoord)
            //{
            //    case 2:
            //        dx *= (-1);
            //        break;
            //    case 3:
            //        dx *= (-1);
            //        dy *= (-1);
            //        break;
            //    case 4:
            //        dy *= (-1);
            //        break;
            //}


            switch (this.UISetting.TesterCoord)
            {
                case 2:
                    dx *= (-1);
                    break;
                case 3:
                    dx *= (-1);
                    dy *= (-1);
                    break;
                case 4:
                    dy *= (-1);
                    break;
            }
            x = dx + AcquireData.ChipInfo.TransCol;
            y = dy + AcquireData.ChipInfo.TransRow;
        }

        private bool CheckAllDiePass(int PassIndex, List<string[]> redStrArrList)
        {
            bool isGroupPass = true;
            foreach (string[] strArr in redStrArrList)
            {
                string testStr = strArr[PassIndex];
                isGroupPass = testStr == "1" ? true : false;
                if (!isGroupPass)
                {
                    break;
                }
            }
            return isGroupPass;
        }

        internal class DieInfo
        {
            public DieInfo(string str)
            {
                GroupStr = str;
                _groupName = GroupStr.Split('.')[0];
            }
            public string GroupStr;

            private string _groupName;

            public string GroupName
            {
                get { return _groupName; }
            }

            public int GroupIndex
            {
                get {
                    int num = -1;
                    if (int.TryParse(GroupStr.Split('.')[1], out num))
                    { }
                    return num;
                }
            }
        }


        #endregion 


        internal class GroupInfoManager
        {
            public int groupSize = 1;
            public List<String> IgnoreStrList = new List<String>(new string[] { "TEST", "COL", "ROW", "ISALLPASS", "TestGroup", "TestGroupBin", "CONTA", "CONTC", "CHANNEL" });
            public Dictionary<string, GroupInfo> NameGroupDic = new Dictionary<string, GroupInfo>();
            public List<int> IgnoreIndex = new List<int>();
            Dictionary<string, string> KeyNameDic;

            public GroupInfoManager(Dictionary<string, string>KNDic)
            {
                KeyNameDic = KNDic;
            }

            public bool IsNameInIgnoreList(string name)
            {
                foreach (string str in IgnoreStrList)
                {
                    if (KeyNameDic.ContainsKey(str) &&
                        name == KeyNameDic[str])
                    {
                        return true;
                    }
                }
                return false;
            }

            public string MakeResultTitle(string titleStr)
            {
                string str = "";
                List<string> strList = titleStr.Split(new char[] { ',' }).ToList();
                str += "Index,"  + KeyNameDic["COL"] + "," + KeyNameDic["ROW"] + "," + KeyNameDic["TestGroup"] + "," + KeyNameDic["ISALLPASS"] + "," + KeyNameDic["TestGroupBin"];
                for (int j = 1; j <= groupSize; ++j)
                    for (int i = 0; i < strList.Count; ++i)
                    {
                        if (!IgnoreIndex.Contains(i))
                        {
                            str += "," + strList[i] + "_" + j.ToString();
                        }
                    }
                return str;
            } 
        }

        internal class GroupInfo
        {
            public int BinIndex = 3;
            public int PassCount = 0;
            public string GroupName = "";
            public int _colIndex,_rowIndex,_groupBinIndex,_passIndex,_groupStrIndex ;
            public List<int> _binList;

            public List<String>[] outStrListArr = new List<String>[1];//4 Dies
            public List<int> IgnoreIndex = new List<int>();
            public int dataLength = 0;
            public Dictionary<string, DieInfo> PosDieDic = new Dictionary<string, DieInfo>();
            private bool isGetGroup = false;

            //private int _maxBin = -9999;
            //private int _minBin = 9999;
            private int groupSize;

            public GroupInfo(string str, int xCol, int yCol, int length, Dictionary<string, DieInfo> posdieDic, int groupSize)
            {
                GroupName = str;

                PosDieDic = posdieDic;
                this._colIndex = xCol;
                this._rowIndex = yCol;
                dataLength = length;
                this.groupSize = groupSize;
                outStrListArr = new List<String>[groupSize];
                _binList = new List<int>();
                for (int i = 0; i < groupSize; ++i)
                {
                    outStrListArr[i] = new List<string>();
                    for (int j = 0; j < dataLength; ++j)
                    {
                        outStrListArr[i].Add("");
                    }
                }
            }
            public GroupInfo(string str, int xCol, int yCol, int passIndex, int groupStrIndex, int groupBinIndex, int length, Dictionary<string, DieInfo> posdieDic, int groupSize)
                : this(str, xCol, yCol, length, posdieDic, groupSize)
            {                
                _passIndex = passIndex;
                _groupStrIndex = groupStrIndex;
                _groupBinIndex = groupBinIndex;
                
            }
            public void PushData(int index,string[] strList)
            {
                //outStrListArr[index].AddRange(strList);
                
                if (!isGetGroup)
                {
                    string colRowKey = strList[_colIndex] + "_" + strList[_rowIndex];
                    string gName = "";
                    int gNum = -1;
                    if (PosDieDic.ContainsKey(colRowKey))
                    {
                        gName = PosDieDic[colRowKey].GroupName;
                        gNum = PosDieDic[colRowKey].GroupIndex;

                        if (gNum > 0)
                        {
                            KeyValuePair<int, int> xyPair = GetRefPos(strList,gNum);
                            int refX = xyPair.Key;
                            int refY = xyPair.Value;
                            for (int i = 0; i < groupSize; ++i)
                            {
                                for (int j = 0; j < dataLength ; ++j)
                                {
                                    outStrListArr[i][j] = "";
                                    if (j == _colIndex )
                                    {
                                        outStrListArr[i][j] += (refX + i).ToString();
                                    }
                                    else if (j == _rowIndex )
                                    {
                                        outStrListArr[i][j] += refY.ToString();
                                    }
                                    else if (j == _groupBinIndex )
                                    {
                                        outStrListArr[i][j] += "0";
                                    }
                                    else if (j == _passIndex )
                                    {
                                        outStrListArr[i][j] += "0";
                                    }
                                    else if (j == _groupStrIndex )
                                    {
                                        outStrListArr[i][j] += GroupName + "." + (i + 1).ToString();
                                    }
                                }
                            }
                        } 
                    }
                    isGetGroup = true;
                }

                int bin = 9999;
                if (int.TryParse(strList[BinIndex], out bin))
                {
                    //if (bin > this._maxBin)
                    //{
                    //    this._maxBin = bin;
                    //}
                    //else if (bin < this._minBin)
                    //{
                    //    this._minBin = bin;
                    //}

                    if (!_binList.Contains(bin))
                    {
                        _binList.Add(bin);
                    }
                }

                outStrListArr[index] = null;
            }

            public string GetStr(int index,string bin,List<int> passIndex = null)
            {
                if(passIndex == null)
                {
                    passIndex = IgnoreIndex;
                }
                //Index	PosX	PosY	Group	ISPASS	GroupBin
                string pos = "";
                pos = GetPos();

                string outStr = index.ToString() + "," + pos + "," + GroupName + ".1,";// + bin;//x,y,bin(pass)
                if(IsPass)
                {
                    outStr += "1,1";
                    if (outStrListArr != null)
                    {
                        for (int i = 0; i < groupSize; ++i)
                        {
                            if (outStrListArr[i] != null)
                            {
                                for (int j = 0; j < dataLength; ++j)
                                {
                                    if (passIndex == null ||
                                        passIndex.Contains(j) ||
                                        outStrListArr[i].Count < dataLength)
                                    {
                                        continue;
                                    }
                                    outStr += "," + outStrListArr[i][j];
                                }
                            }
                            else { return outStr; }

                        }
                    }
                    else { return outStr; }
                }
                else
                {
                    outStr += "0,0";
                    for (int i = 0; i < groupSize * dataLength; ++i)
                    {
                        outStr += ",";
                    }
                }



                return outStr;
            }

            public List<string> GetFailStrList(ref int index, EGroupBinRule GroupBinRule)
            {
                List<string> outList = new List<string>();
                //int refY = int.MinValue;
                //int refX = int.MinValue;

                string GroupBin = "";
                switch (GroupBinRule)
                {
                    case EGroupBinRule.MAX:
                    default:
                        GroupBin = MaxGroupBin;
                        break;
                    case EGroupBinRule.MIN:
                        GroupBin = MinGroupBin;
                        break;
                    case EGroupBinRule.SAME:
                        GroupBin = SameBin;
                        break;
                }


                for (int i = 0; i < groupSize; ++i)//find
                {
                    if (outStrListArr[i] != null)
                    {
                        string outStr = "";
                        outStrListArr[i][0] = index.ToString();
                        outStrListArr[i][_groupBinIndex] = GroupBin;
                        foreach(string str in outStrListArr[i])
                        {
                            outStr += str + ",";
                        }
                        outStr = outStr.TrimEnd(',');
                        outList.Add(outStr);
                        index++;
                    }
                }

                return outList;

            }

            private string GetPos()
            {
                string pos = "";
                for (int i = 0; i < outStrListArr.Length; ++i)
                {
                    if (outStrListArr[i] != null && outStrListArr[i].Count == dataLength)
                    {
                        int x = int.MinValue;
                        if (!int.TryParse(outStrListArr[i][_colIndex], out x))
                        {
                            continue;
                        }
                        x -= i;//第一顆壞時，取第二第三顆時，X換算回第一顆的位置

                        if (x < 0)
                        { x = (x - 3); }
                        pos = (x / groupSize).ToString() + "," + outStrListArr[i][_rowIndex];
                        //x = x + 4
                    }
                }
                return pos;
            }

            private KeyValuePair<int,int> GetRefPos(bool isShow = false)
            {
                KeyValuePair<int, int> xyPair = new KeyValuePair<int, int>();
                int i = 0;
                for (; i < outStrListArr.Length; ++i)
                {
                    if (outStrListArr[i] != null && outStrListArr[i].Count == dataLength)
                    {
                        int x = int.MinValue;
                        if (!int.TryParse(outStrListArr[i][_colIndex], out x))
                        {
                            continue;
                        }
                        x -= i;//第一顆壞時，取第二第三顆時，X換算回第一顆的位置
                        int y = int.MinValue;
                        if (!int.TryParse(outStrListArr[i][_rowIndex], out y))
                        {
                            continue;
                        }
                        xyPair= new KeyValuePair<int, int>(x,y);


                        break;
                    }
                }

                return xyPair;
            }


            private KeyValuePair<int, int> GetRefPos(string[] strList, int index)
            {
                KeyValuePair<int, int> xyPair = new KeyValuePair<int, int>();
                if (strList != null)
                {
                    int x = int.MinValue;
                    if (!int.TryParse(strList[_colIndex], out x))
                    {
                    }
                    x = x - index + 1;
                    int y = int.MinValue;
                    if (!int.TryParse(strList[_rowIndex], out y))
                    {
                    }
                    xyPair = new KeyValuePair<int, int>(x, y);
                }
                return xyPair;
            }

            public bool IsPass
            {
                get
                {
                    if (PassCount != groupSize)
                    {
                        return false;
                    }
                    return true;
                }
                set 
                {
                    if (value)
                    {
                        PassCount = groupSize;
                    }
                    else 
                    {
                        PassCount = -1;
                    }
                }
            }

            #region >>property<<
            public string MaxGroupBin
            {
                get
                {
                    if (IsPass && _binList != null && _binList.Count > 0)
                    {
                        return this._binList.Max().ToString();
                    }
                    else
                    {
                        return "0";
                    }
                }
            }
            public string MinGroupBin
            {
                get
                {
                    if (IsPass && _binList != null && _binList.Count > 0)
                    {
                        return this._binList.Min().ToString();
                    }
                    else
                    {
                        return "0";
                    }
                }
            }
            public string SameBin
            {
                get
                {
                    if (IsPass && _binList != null && _binList.Count ==1)
                    {
                        return this._binList[0].ToString();
                    }
                    else
                    {
                        return "0";
                    }
                }
            }
            //public string GroupBin
            //{
            //    get {
            //        if (IsPass)
            //        {                        
            //            return _maxBin.ToString();
            //        }
            //        else 
            //        {
            //            return "0";
            //        }
            //    }//目前客戶要以最大的為準
            //}
            #endregion

        }

	}
}
