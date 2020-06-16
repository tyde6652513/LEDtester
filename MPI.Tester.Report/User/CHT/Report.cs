using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using MPI.Tester.Data;

using MPI.Tester.TestServer;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Report.User.CHT
{
	class Report : ReportBase
	{
        Dictionary<string, double> maxData;

        Dictionary<string, double> minData;

        Dictionary<string, string> _sweepItemDic;

        private List<string> _colRow;

        private List<string> _itemKeys;

        private int _maxLCRCount = 0;

		public Report(List<object> objs, bool isReStatistic)
			: base(objs, isReStatistic)
		{
		}

        protected override void SetResultTitle()
		{
			//

            if (UISetting.FormatName == "Format-B")
            {
                this.SetResultTitleByDefault();                
            }
            else 
            {
                this.ResultTitleInfo.SetResultData(this.UISetting.UserDefinedData.ResultItemNameDic);
            }

            maxData = new Dictionary<string, double>();

            minData = new Dictionary<string, double>();

            _colRow = new List<string>();

            GetMaxLCRCount();

            ResetResultDic();

    //        if (this.Product.TestCondition != null &&
    //this.Product.TestCondition.TestItemArray != null &&
    //this.Product.TestCondition.TestItemArray.Length > 0)
    //        {
    //            foreach (var testItem in this.Product.TestCondition.TestItemArray)
    //            {
    //                if (testItem.MsrtResult == null || testItem.MsrtResult.Length == 0 || testItem.GainOffsetSetting == null)
    //                {
    //                    continue;
    //                }

    //                foreach (var data in testItem.GainOffsetSetting)
    //                {
    //                    if (!data.IsEnable || !data.IsVision)
    //                    {
    //                        continue;
    //                    }
    //                    maxData.Add(data.KeyName, -999999);

    //                    minData.Add(data.KeyName, 999999);
    //                }
    //            }
    //        }
            //_itemKeys = this.ResultTitleInfo.
		}

        protected override EErrorCode WriteReportHeadByUser()
		{
            ////////////////////////////////////////////
            //Write Report Head
            ////////////////////////////////////////////
            this.WriteLine("FileName,\"" + Path.Combine(this.TestResultFileNameWithoutExt(), this.UISetting.TestResultFileExt) + "\"");

            this.WriteLine("UserID,\"" + this.UISetting.UserID.ToString() + "_" + this.UISetting.FormatName + "\"");

            this.WriteLine("TestTime,\"" + this.TesterSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm") + "\"");

            this.WriteLine("EndTime,\"" + "\"");

            this.WriteLine("TesterModel,\"" + this.MachineInfo.TesterModel + "/" + this.MachineInfo.TesterSN + "\"");

            this.WriteLine("MachineName,\"" + this.UISetting.MachineName + "\"");

            this.WriteLine("LotNumber,\"" + this.UISetting.LotNumber + "\"");

            this.WriteLine("Substrate,\"" + this.UISetting.Substrate + "\"");

            this.WriteLine("TaskFile,\"" + this.UISetting.TaskSheetFileName + "\"");

            this.WriteLine("Recipe,\"" + this.UISetting.ProductFileName + "\"");

            this.WriteLine("ConditionFileName,\"" + this.UISetting.ConditionFileName + "\"");

            this.WriteLine("BinFileName,\"" + this.UISetting.BinDataFileName + "\"");            

            this.WriteLine("Filter Wheel,\"" + this.Product.ProductFilterWheelPos.ToString() + "\"");

            this.WriteLine("LOPSaveItem,\"" + this.Product.LOPSaveItem.ToString() + "\"");

            this.WriteLine("Operator,\"" + this.UISetting.OperatorName + "\"");

            this.WriteLine("Samples,\"\"");

            this.WriteLine("Unit,\"9999\"");

            this.WriteLine("");

            //Write LCR Cali Info
            //if (UISetting.FormatName == "Format-B")
            //{
            //    List<string> caliList = GetLcrCaliList();
            //    foreach (string str in caliList)
            //    {
            //        this.WriteLine(str);
            //    }
            //}

            ////////////////////////////////////////////
            //Write Test Item Info
            ////////////////////////////////////////////
            this.WriteLine("Item,Bias,BiasUnit,Time(ms),Compliance,CompUnit,Lower,Upper,Sqr,Gain,Offset,RltUnit");

            Dictionary<string, GainOffsetData> gainOffsetData = new Dictionary<string, GainOffsetData>();

            //Dictionary<string, double> maxData = new Dictionary<string, double>();

            //Dictionary<string, double> minData = new Dictionary<string, double>();

            #region

            //List<string> titleItemList = this.ResultTitleInfo.TitleStr.Split(new char[] { ',' }).ToList();

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

                        if (!testItem .IsEnable|| !data.IsEnable || !data.IsVision || testItem.MsrtResult[GetMsrtIndex(testItem, data.KeyName)].IsEnable == false)
                        {
                            continue;
                        }

                        gainOffsetData.Add(data.KeyName, data);

                        //maxData.Add(data.KeyName, -999999);

                        //minData.Add(data.KeyName, 999999);
                    }

                    foreach (var msrtItem in testItem.MsrtResult)
                    {
                        if (!testItem.IsEnable || !msrtItem.IsEnable || !msrtItem.IsVision || testItem.ElecSetting == null)
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

                        line += "," + testItem.ElecSetting[0].ForceValue.ToString();

                        line += "," + testItem.ElecSetting[0].ForceUnit;

                        line += "," + testItem.ElecSetting[0].ForceTime.ToString();

                        line += "," + testItem.ElecSetting[0].MsrtProtection.ToString();

                        //line += "," + testItem.ElecSetting[0].MsrtUnit;
                        line += "," + FindMsrtUnit(msrtItem.KeyName);

                        line += "," + msrtItem.MinLimitValue;

                        line += "," + msrtItem.MaxLimitValue;

                        line += "," + sqr.ToString();

                        line += "," + gain.ToString();

                        line += "," + offset.ToString();

                        //line += "," + testItem.ElecSetting[0].MsrtUnit;
                        line += "," + FindMsrtUnit(msrtItem.KeyName);

                        this.WriteLine(line);
                    }
                }
            }
            #endregion

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

            string lineMin = "Min";

            string lineMax = "Max";

            foreach (var item in gainOffsetData)
            {
                lineItem += "," + item.Value.Name;

                lineType += "," + item.Value.Type;

                lineSquare += "," + item.Value.Square;

                lineGain += "," + item.Value.Gain;

                lineOffset += "," + item.Value.Offset;

                lineGain2 += "," + item.Value.Gain2;

                lineOffset2 += "," + item.Value.Offset2;

                TestResultData result;
                if (GetResultData(item.Value.KeyName, out  result))
                {
                    lineMin += "," + result.MinLimitValue;

                    lineMax += "," + result.MaxLimitValue;

                }
                else 
                {
                    lineMin += ",";

                    lineMax += ",";
                }
                
            }

            this.WriteLine(lineItem);

            this.WriteLine(lineType);

            this.WriteLine(lineSquare);

            this.WriteLine(lineGain);

            this.WriteLine(lineOffset);

            this.WriteLine(lineGain2);

            this.WriteLine(lineOffset2);

            this.WriteLine(lineMin);

            this.WriteLine(lineMax);

            this.WriteLine("");

            ////////////////////////////////////////////
            //Write Result Item Title
            ////////////////////////////////////////////
            this.WriteLine(this.ResultTitleInfo.TitleStr);

            return EErrorCode.NONE;
		}

        private  int  GetMsrtIndex(TestItemData testItem, string keyName)
        {
            int resultIndex = 0;
            foreach (TestResultData trd in testItem.MsrtResult)
            {
                if (trd.KeyName == keyName)
                {
                    return resultIndex;
                }
                resultIndex++;
            }
            return 0;
        }

        private bool GetResultData(string keyName, out TestResultData result)
        {
            result = null;
            foreach (TestItemData ti in this.Product.TestCondition.TestItemArray)
            {
                foreach (TestResultData tr in ti.MsrtResult)
                {
                    if (tr.KeyName == keyName)
                    {
                        result = tr;
                        return true;
                    }
                }
            }

            return false;
        }

        protected override EErrorCode PushDataByUser(Dictionary<string, double> data)
		{
			int binSN = (int)data["BINSN"];

			SmartBinDataBase bin = this.SmartBinning.GetBinFromSN(binSN);

			int binGrade = 0;

			int binNumber = 0;

			string binCode = string.Empty;

            GetBinNum(binSN, out  binCode, out  binNumber, out  binGrade);

			string line = string.Empty;

			int index = 0;

			foreach (var item in this.ResultTitleInfo)
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

					if (this.UISetting.UserDefinedData[item.Key] != null)
					{
						format = this.UISetting.UserDefinedData[item.Key].Formate;
					}
                    checkMaxMin(item.Key, data[item.Key]);

					line += data[item.Key].ToString(format);
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

        protected override EErrorCode RewriteReportByUser()
		{
            Dictionary<string, string> replaceData = new Dictionary<string, string>();

            string endTime = "EndTime,\"" + this.TesterSetting.EndTestTime.ToString("yyyy/MM/dd HH:mm") + "\"";

            replaceData.Add("EndTime,\"" + "\"", endTime);

            string testCount = "Samples,\"" + this.TotalTestCount.ToString() + "\"";

            replaceData.Add("Samples,\"" + "\"", testCount);


            //string maxVal = "Max" + getMaxValList();

            //replaceData.Add("Max", maxVal);

            //string minVal = "Min" + getMinValList();

            //replaceData.Add("Min", minVal);

            this.ReplaceReport(replaceData);

            return EErrorCode.NONE;
		}



        protected override EErrorCode WriteReportHeadByUser2()
        {
            ////////////////////////////////////////////
            //Write Report Head
            ////////////////////////////////////////////
            this.WriteLine2("FileName,\"" + Path.Combine(this.TestResultFileNameWithoutExt(), this.UISetting.TestResultFileExt) + "\"");

            this.WriteLine2("UserID,\"" + this.UISetting.UserID.ToString() + "_" + this.UISetting.FormatName + "\"");

            this.WriteLine2("TestTime,\"" + this.TesterSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm") + "\"");

            this.WriteLine2("EndTime,\"" + "\"");

            this.WriteLine2("TesterModel,\"" + this.MachineInfo.TesterModel + "/" + this.MachineInfo.TesterSN + "\"");

            this.WriteLine2("MachineName,\"" + this.UISetting.MachineName + "\"");

            this.WriteLine2("LotNumber,\"" + this.UISetting.LotNumber + "\"");

            this.WriteLine2("Substrate,\"" + this.UISetting.Substrate + "\"");

            this.WriteLine2("TaskFile,\"" + this.UISetting.TaskSheetFileName + "\"");

            this.WriteLine2("Recipe,\"" + this.UISetting.ProductFileName + "\"");

            this.WriteLine2("ConditionFileName,\"" + this.UISetting.ConditionFileName + "\"");

            this.WriteLine2("BinFileName,\"" + this.UISetting.BinDataFileName + "\"");

            this.WriteLine2("Filter Wheel,\"" + this.Product.ProductFilterWheelPos.ToString() + "\"");

            this.WriteLine2("LOPSaveItem,\"" + this.Product.LOPSaveItem.ToString() + "\"");

            this.WriteLine2("Operator,\"" + this.UISetting.OperatorName + "\"");

            this.WriteLine2("Samples,\"\"");

            this.WriteLine2("Unit,\"9999\"");

            this.WriteLine2("");



            //Write LCR Cali Info
            //if (UISetting.FormatName == "Format-B")
            //{
            //    List<string> caliList = GetLcrCaliList();
            //    foreach (string str in caliList)
            //    {
            //        this.WriteLine2(str);
            //    }
            //}

            ////////////////////////////////////////////
            //Write Test Item Info
            ////////////////////////////////////////////
            this.WriteLine2("Item,Bias,BiasUnit,Time(ms),Compliance,CompUnit,Lower,Upper,Sqr,Gain,Offset,RltUnit");

            Dictionary<string, GainOffsetData> gainOffsetData = new Dictionary<string, GainOffsetData>();

            //Dictionary<string, double> maxData = new Dictionary<string, double>();

            //Dictionary<string, double> minData = new Dictionary<string, double>();

            #region

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
                        if (!data.IsEnable || !data.IsVision || testItem.MsrtResult[GetMsrtIndex(testItem, data.KeyName)].IsEnable == false)
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

                        if (!_sweepItemDic.ContainsKey(msrtItem.KeyName) )
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

                        line += "," + testItem.ElecSetting[0].ForceValue.ToString();

                        line += "," + testItem.ElecSetting[0].ForceUnit;

                        line += "," + testItem.ElecSetting[0].ForceTime.ToString();

                        line += "," + testItem.ElecSetting[0].MsrtProtection.ToString();

                        //line += "," + testItem.ElecSetting[0].MsrtUnit;
                        line +=  "," + FindMsrtUnit(msrtItem.KeyName);

                        line += "," + msrtItem.MinLimitValue;

                        line += "," + msrtItem.MaxLimitValue;

                        line += "," + sqr.ToString();

                        line += "," + gain.ToString();

                        line += "," + offset.ToString();

                        //line += "," + testItem.ElecSetting[0].MsrtUnit;
                        line += "," + FindMsrtUnit(msrtItem.KeyName);

                        this.WriteLine2(line);
                    }
                }
            }
            #endregion

            this.WriteLine2("");

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

            string lineMin = "Min";

            string lineMax = "Max";

            foreach (var item in gainOffsetData)
            {
                if (!_sweepItemDic.ContainsValue(item.Value.Name))
                {
                    continue;
                }

                lineItem += "," + item.Value.Name;

                lineType += "," + item.Value.Type;

                lineSquare += "," + item.Value.Square;

                lineGain += "," + item.Value.Gain;

                lineOffset += "," + item.Value.Offset;

                lineGain2 += "," + item.Value.Gain2;

                lineOffset2 += "," + item.Value.Offset2;

                TestResultData result;
                if (GetResultData(item.Value.KeyName, out  result))
                {
                    lineMin += "," + result.MinLimitValue;

                    lineMax += "," + result.MaxLimitValue;

                }
                else
                {
                    lineMin += ",";

                    lineMax += ",";
                }
            }

            this.WriteLine2(lineItem);

            this.WriteLine2(lineType);

            this.WriteLine2(lineSquare);

            this.WriteLine2(lineGain);

            this.WriteLine2(lineOffset);

            this.WriteLine2(lineGain2);

            this.WriteLine2(lineOffset2);

            this.WriteLine2(lineMin);

            this.WriteLine2(lineMax);

            this.WriteLine2("");

            ////////////////////////////////////////////
            //Write Result Item Title
            ////////////////////////////////////////////
            //this.WriteLine2(this.ResultTitleInfo.TitleStr);
            string titleStr = string.Empty;//"TEST,PosX,PosY,BIN,POLAR";

            int counter = 0;

            foreach (KeyValuePair<string, string> knPair in _sweepItemDic)
            {
                titleStr += knPair.Value;

                ++counter;

                if (counter != _sweepItemDic.Count)
                {
                    titleStr += ",";
                }
            }
            this.WriteLine2(titleStr);

            return EErrorCode.NONE;
        }

        protected override EErrorCode PushDataByUser2(Dictionary<string, double> data)
        {
            int  binSN = (int)data["BINSN"];

            SmartBinDataBase bin = this.SmartBinning.GetBinFromSN(binSN);

            int binGrade = 0;

            int binNumber = 0;

            string binCode = string.Empty;

            GetBinNum(binSN, out  binCode, out  binNumber, out  binGrade);

            string binResult = GetBinResult(data, binGrade, binNumber, binCode);


            string test = data["TEST"].ToString();

            string col = data["COL"].ToString();

            string row = data["ROW"].ToString();

            string polar = data["POLAR"].ToString();

            string key = "X" + col + "Y" + row;

            if (!this._colRow.Contains(key))
            {
                this._colRow.Add(key);
            }

            if (this._maxLCRCount == 0)
            {
                return EErrorCode.NONE;
            }

            for (int resultCount = 0; resultCount < this._maxLCRCount; resultCount++)
            {
                //string line = test + "," + col + "," + row;//20170624 Daivd 因ByLot輸出報表前兩項為Wafer ID,Site兩個欄位(Site = TEST)
                string line = test + "," + col + "," + row + "," + binResult + "," + polar;

                foreach (LCRData LcrList in this.AcquireData.LCRDataSet)
                {
                    if (!LcrList.IsEnable)
                    {
                        continue;
                    }

                    foreach (var resultPair in this._sweepItemDic)
                    {
                       // LIVResultItemData livData = livNum[resultKey.Key];
                        foreach (var LcrData in LcrList)
                        {
                            //string str = livData.KeyName.Remove(livData.KeyName.IndexOf("_"));

                            string str = LcrData.KeyName;

                            if (str != resultPair.Key)
                            {
                                continue;
                            }

                            line += ",";

                            if (LcrData.DataArray.Length > resultCount)
                            {
                                line += LcrData.DataArray[resultCount].ToString(LcrData.Formate);
                            }
                        }
                    }
                }

                this.WriteLine2(line);
            }

            return EErrorCode.NONE;
        }

       
        protected override EErrorCode RewriteReportByUser2()
        {
            Dictionary<string, string> replaceData = new Dictionary<string, string>();

            string endTime = "EndTime,\"" + this.TesterSetting.EndTestTime.ToString("yyyy/MM/dd HH:mm") + "\"";

            replaceData.Add("EndTime,\"" + "\"", endTime);

            string testCount = "Samples,\"" + this.TotalTestCount.ToString() + "\"";

            replaceData.Add("Samples,\"" + "\"", testCount);


            //string maxVal = "Max" + getMaxValList();

            //replaceData.Add("Max", maxVal);

            //string minVal = "Min" + getMinValList();

            //replaceData.Add("Min", minVal);

            this.ReplaceReport2(replaceData);

            return EErrorCode.NONE;
        }

        protected override EErrorCode MoveFileToTargetByUser2(EServerQueryCmd cmd)
        {
            bool isOutputPath02 = false;

            string outPath01 = string.Empty;

            string outPath02 = string.Empty;

            ETesterResultCreatFolderType type01 = ETesterResultCreatFolderType.None;

            ETesterResultCreatFolderType type02 = ETesterResultCreatFolderType.None;

            if (this.UISetting.IsManualRunMode)
            {
                outPath01 = this.UISetting.ManualOutputPath01;

                type01 = this.UISetting.ManualOutputPathType01;

                isOutputPath02 = this.UISetting.IsEnableManualPath02;

                outPath02 = this.UISetting.ManualOutputPath02;

                type02 = this.UISetting.ManualOutputPathType02;
            }
            else
            {
                outPath01 = this.UISetting.TestResultPath01;

                type01 = this.UISetting.TesterResultCreatFolderType01;

                isOutputPath02 = this.UISetting.IsEnablePath02;

                outPath02 = this.UISetting.TestResultPath02;

                type02 = this.UISetting.TesterResultCreatFolderType02;

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

            string fileName = this.TestResultFileNameWithoutExt();

            string _fileFullNameRep = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, fileName + ".rep2");

            if (isOutputPath02)
            {
                if (outputPathAndFile01 == outputPathAndFile02)
                {
                    outputPathAndFile02 = Path.Combine(outPath02, fileNameWithoutExt +"_Sweep"+ "." + this.UISetting.TestResultFileExt);
                }
                MPIFile.CopyFile(_fileFullNameRep, outputPathAndFile02);
            }


            return EErrorCode.NONE;
        }

        private void checkMaxMin(string keyName, double val)
        {
            if (maxData.ContainsKey(keyName) && maxData[keyName] < val)
            {
                maxData[keyName] = val;
            }

            if (minData.ContainsKey(keyName) && minData[keyName] > val)
            {
                minData[keyName] = val;
            }
        }

        private string getMaxValList()
        {
            string outStr = string.Empty;
            foreach (KeyValuePair<string, double> keyVal in maxData)
            {
                outStr += "," + keyVal.Value.ToString("0.####"); 
            }
            return outStr;
        }

        private string getMinValList()
        {
            string outStr = string.Empty;
            foreach (KeyValuePair<string, double> keyVal in minData)
            {
                outStr += "," + keyVal.Value.ToString("0.####");
            }
            return outStr;
        }

        private void GetBinNum(int binSN, out string binCode, out int binNumber, out int binGrade)
        {
            //int binSN = (int)data["BINSN"];

            SmartBinDataBase bin = this.SmartBinning.GetBinFromSN(binSN);

            binGrade = 0;

            binNumber = 0;

            binCode = string.Empty;

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
        }

        //private void GetMaxLIVCount()
        //{
        //    foreach (var LcrList in this.AcquireData.LCRDataSet)
        //    {
        //        if (!LcrList.IsEnable)
        //        {
        //            continue;
        //        }

        //        foreach (var LcrData in LcrList)
        //        {
        //            if (this._maxLCRCount < LcrData.DataArray.Length)
        //            {
        //                this._maxLCRCount = LcrData.DataArray.Length;
        //            }
        //        }
        //    }
        //}

        private void GetMaxLCRCount()
        {
            foreach (var LcrList in this.AcquireData.LCRDataSet)
            {
                if (!LcrList.IsEnable)
                {
                    continue;
                }

                foreach (var LcrData in LcrList)
                {
                    if (this._maxLCRCount < LcrData.DataArray.Length)
                    {
                        this._maxLCRCount = LcrData.DataArray.Length;
                    }
                }
            }
        }


        private string FindMsrtUnit(string keyName)
        {
            string result = string.Empty;
            if (this.UISetting.UserDefinedData[keyName] != null)
            {
                result = this.UISetting.UserDefinedData[keyName].Unit;
            }

            return result;
        }

        private string GetBinResult(Dictionary<string, double> data, int binGrade, int binNumber, string binCode)
        {
            string binResult = data["BIN"].ToString("0");

            foreach (var item in this.ResultTitleInfo)
            {
                if (item.Key == "BIN_CODE")
                {
                    binResult = binCode;
                    break;
                }
                else if (item.Key == "BIN_NUMBER")
                {
                    binResult = binNumber.ToString();
                    break;
                }
                else if (item.Key == "BIN_GRADE")
                {
                    binResult = binGrade.ToString();
                    break;
                }
            }
            return binResult;
        }

        private void ResetResultDic()
        {
            this._sweepItemDic = new Dictionary<string, string>();

            Dictionary<string, string> MD = UISetting.UserDefinedData.MsrtDisplayItemDic;

            if (MD != null && MD.Count > 5)
            {
                this._sweepItemDic.Add("TEST", MD["TEST"]);
                this._sweepItemDic.Add("COL", MD["COL"]);
                this._sweepItemDic.Add("ROW", MD["ROW"]);
                this._sweepItemDic.Add("BIN", MD["BIN"]);
                this._sweepItemDic.Add("POLAR", MD["POLAR"]);


                if (this.Product.TestCondition != null &&
                    this.Product.TestCondition.TestItemArray != null &&
                    this.Product.TestCondition.TestItemArray.Length > 0)
                {
                    foreach (var testItem in this.Product.TestCondition.TestItemArray)
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

                            if (IsLCRListItem(msrtItem.KeyName))
                            {
                                this._sweepItemDic.Add(msrtItem.KeyName, msrtItem.Name);
                            }
                        }
                    }
                }
            }
        }

        bool IsLCRListItem(string keyName)
        {
            foreach (LCRData ls in this.AcquireData.LCRDataSet)
            {
                foreach (var va in ls)
                {
                    if (va.KeyName == keyName)
                    {
                        return true;
                    }
                }
            }
            return false;
 
        }

        List<string> GetLcrCaliList()
        {
            List<string> outStrList = new List<string>();

            //LCRCaliData cData = ConditionCtrl.LCRCaliData;

            LCRCaliData cData = SysCali.SystemCaliData.LCRCaliData;

            int dataNum = cData.NowDataNum;


            string type = "CaliType:," + cData.TestType.ToString();

            outStrList.Add(type);

            string CableLength = "Cable Length:," + cData.CableLength.ToString();

            outStrList.Add(CableLength);

            string caliBias = "Cali Bias:," + cData.Bias.ToString() + "V";

            outStrList.Add(caliBias);

            string caliVolt = "Cali Osc:," + cData.Level.ToString() + "V";

            outStrList.Add(caliVolt);

            string openCali = "OPEN:," + (cData.EnableOpen ? "On" : "OFF");

            outStrList.Add(openCali);

            string shortCali = "SHORT:," + (cData.EnableShort ? "On" : "OFF");

            outStrList.Add(shortCali);

            string loadCali = "Load:," + (cData.EnableLoad ? "On" : "OFF");

            if (cData.EnableLoad)
            {
                loadCali += ",Ref_A: " + cData.LoadingList[dataNum - 1].RefA + " " + cData.LoadingList[dataNum - 1].RefUnit + ",Ref_B: " + cData.LoadingList[dataNum - 1].RefB;
            }

            outStrList.Add(loadCali);

            return outStrList;
            //outStrList 
            
        }

	}
}
