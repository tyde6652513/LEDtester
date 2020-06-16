using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using MPI.Tester.Data;
using MPI.Tester.TestServer;

namespace MPI.Tester.Report.User.WAVETEK00
{
    partial class Report : ReportBase
    {
        EErrorCode OutPutFile3()
        {
            string tempFileName = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, WAVETEK_Tmp);

            string tarFileName = GetFile3Name();

           

            wtSw = new StreamWriter(tempFileName, false, this._reportData.Encoding);

            CreateFile3Head();

            PushDataToFile3();

            wtSw.Close();

            wtSw.Dispose();

            wtSw = null;

            bool result = true;

            if (UISetting.PathInfoArr[4].EnablePath)
            {
                string fullTarName1 = Path.Combine(UISetting.PathInfoArr[4].TestResultPath, tarFileName);
                if (!CopyFileOrBackUp(tempFileName, tarFileName, fullTarName1))
                    result = false;
            }

            if (UISetting.PathInfoArr[5].EnablePath)
            {
                string fullTarName2 = Path.Combine(UISetting.PathInfoArr[5].TestResultPath, tarFileName);
                if (!CopyFileOrBackUp(tempFileName, tarFileName, fullTarName2))
                    result = false;
            }

            MPIFile.DeleteFile(tempFileName);

            if (result == false)
            {
                return EErrorCode.REPORT_Customize_File_Create_Fail;
            }
            return EErrorCode.NONE;
        }

        private string GetFile3Name()
        {
            string fileName = UISetting.WaferNumber;
            if (UISetting.PathInfoArr != null )
            {
                fileName = UISetting.LotNumber + "_" +
                    UISetting.WaferNumber + "_" +
                    UISetting.SubPiece + "_";
                string startTime = "";
                if (SetDataInfo.ContainsKey("TestStartTime"))
                {
                    DateTime dt;
                    DateTime.TryParse(SetDataInfo["TestStartTime"], out dt);
                    startTime = dt.ToString("yyMMdd_HHmmss");
                    fileName += startTime + "_";
                }
                fileName += UISetting.TaskSheetFileName + "_" +
                    UISetting.MachineName + "_";
                if (IsCvTest)
                {
                    fileName += "CP2";
                }
                else
                {
                    fileName += "CP1";
                }
                fileName += ".txt";

            }
            return fileName;
        }

        private EErrorCode CreateFile3Head()
        {

            DateTime time;
            string startTimeStr = "", endTimeStr = "";
            //if (DateTime.TryParse(SetDataInfo["TestStartTime"], out time))
            //{
            //    startTimeStr = time.ToString("yyyy_MM_dd HH:mm:ss");
            //}

            startTimeStr = this._refSearchTime.ToString("yyyy_MM_dd HH:mm:ss ");
            if (DateTime.TryParse(SetDataInfo["TestEndTime"], out time))
            {
                endTimeStr = time.ToString("yyyy/MM/dd HH:mm:ss");
            }
            wtSw.WriteLine( startTimeStr);

            wtSw.WriteLine("      Prog Name:    " + UISetting.TaskSheetFileName);
            wtSw.WriteLine("       Job Name:    " + UISetting.TaskSheetFileName);
            wtSw.WriteLine("            Lot:    " + UISetting.LotNumber);
            wtSw.WriteLine("       Operator:    " + UISetting.OperatorName);
            wtSw.WriteLine("      Test Mode:    Production");
            wtSw.WriteLine("      Node Name:    " + UISetting.MachineName);
            wtSw.WriteLine("      Part Type:    " + Product.ProductName);
            wtSw.WriteLine("    Channel map:    Chans");
            wtSw.WriteLine("    Environment:    ");

            wtSw.WriteLine("");
            wtSw.WriteLine("    Site Number:");

            string siteStr = "         0";
            int chNum = 0;
            if (SetDataInfo.ContainsKey("ChannelQty"))
            {
                if (int.TryParse(SetDataInfo["ChannelQty"], out chNum))
                {
                    for (int i = 1; i < chNum; ++i)
                    {
                        siteStr += "," + i.ToString("0");
                    }
 
                }
            }
            wtSw.WriteLine("         0");
            wtSw.WriteLine("");


            return EErrorCode.NONE;
        }

        private EErrorCode PushDataToFile3()
        {
            int testCount = 1;
            int tesitemCount = dieLogDic.Count;
            string lotNum = UISetting.LotNumber;
            string slotNum = UISetting.WaferNumber;


            foreach (var gData in baseDieGropuDic)
            {
                string ch = testCount.ToString();
                if (gData.Value.ChDieDic.Count > 1)
                {
                    ch += "," + (testCount + 1).ToString();
                }
                wtSw.WriteLine("    Device#: " + ch);
                wtSw.WriteLine("");
                wtSw.WriteLine("==PIR==");
                wtSw.WriteLine("  Part ID......... : 0");
                wtSw.WriteLine("  Site #.......... : " + (gData.Value.ChDieDic.Count).ToString());


                string[] strArr = gData.Key.Split('_');
                wtSw.WriteLine("  X coordinate.... : " + strArr[0]);//gData.Value.X.ToString());
                wtSw.WriteLine("  Y coordinate.... : " + strArr[1]);//gData.Value.Y.ToString());

                string title = "Number\tSite\tResult\tTest Name\tChannel\tLow\t\tMeasured\tHigh\t\tForce\t\tLoc";
                wtSw.WriteLine(title);
                int itemCount = 1;
                //int failCounter = 0;
                foreach (var mInfo in MsrtkeyInfoDic)
                {
                    //failCounter = 0;
                    List<DieLog> dllist = (from dl in gData.Value.ChDieDic.Values
                                               orderby dl.Ch
                                               select dl).ToList();
                    //foreach (var dLog in gData.Value.ChDieDic)
                    foreach (var dLog in dllist)
                    {
                        string passStr = "PASS";

                        string str = itemCount + "\t" + (dLog.Ch - 1).ToString();
                        string format = mInfo.Value.MsrtData.Formate;
                        int index = mInfo.Value.ColIndex;

                        if ((mInfo.Value.MsrtData.IsInSpec(dLog.RawData[index])))
                        {
                            passStr = "PASS";
                            dLog.PassTestCnt++;
                        }
                        else
                        {
                            //failCounter++;
                            passStr = "FAIL";
                            dLog.FailTestCnt++;
                        }
                        str += "\t" + passStr;
                        str += "\t" + mInfo.Value.GetCustomerName_1base(3);
                        str += "\t\t" + (dLog.Ch);//有疑慮
                        str += "\t" + mInfo.Value.MsrtData.MinLimitValue.ToString("0.000000") + " " + mInfo.Value.MsrtData.Unit;

                        str += "\t" + dLog.RawData[index] + " " + mInfo.Value.MsrtData.Unit;

                        str += "\t" + mInfo.Value.MsrtData.MaxLimitValue.ToString("0.000000") + " " + mInfo.Value.MsrtData.Unit;
                        string forceStr = "";
                        if (mInfo.Value.TestData is LCRTestItem)
                        {
                            forceStr = (mInfo.Value.TestData as LCRTestItem).LCRSetting.DCBiasV.ToString(format);
                        }
                        else
                        {
                            forceStr = mInfo.Value.TestData.ElecSetting[0].ForceValue.ToString("0.000000");
                        }
                        str += "\t" + forceStr + " " + mInfo.Value.TestData.ElecSetting[0].ForceUnit;
                        str += "\t 0";
                        wtSw.WriteLine(str);
                        
                        
                    }
                    itemCount++;
                    
                    wtSw.WriteLine("======");
                }

                testCount += gData.Value.ChDieDic.Count;
                wtSw.WriteLine("");
                wtSw.WriteLine("==PRR==");
                wtSw.WriteLine("");
                wtSw.WriteLine(" Site Failed tests/Executed tests");
                wtSw.WriteLine("------------------------------------");
                foreach (var dieData in gData.Value.ChDieDic)
                {
                    wtSw.WriteLine("    " + (dieData.Value.Ch - 1).ToString() + "\t\t" + dieData.Value.FailTestCnt + "\t" + (MsrtkeyInfoDic.Count));
                }
                wtSw.WriteLine("");
                wtSw.WriteLine(" Site    Sort     Bin");
                wtSw.WriteLine("------------------------------------");
                foreach (var dieData in gData.Value.ChDieDic)
                {
                    wtSw.WriteLine("    " + (dieData.Value.Ch - 1).ToString() + "\t\t" + dieData.Value.Bin + "\t" + dieData.Value.Bin);
                }
                wtSw.WriteLine("");
                wtSw.WriteLine("=========================================================================");

            }


            //foreach (var dieData in dieLogDic)
            //{

            //    wtSw.WriteLine("    Device#: "+testCount);
            //    wtSw.WriteLine("");
            //    wtSw.WriteLine("==PIR==");
            //    wtSw.WriteLine("  Part ID......... : 0");
            //    wtSw.WriteLine("  Site #.......... : " + (dieData.Value.Ch).ToString());
            //    wtSw.WriteLine("  X coordinate.... : " + dieData.Value.X.ToString());
            //    wtSw.WriteLine("  Y coordinate.... : " + dieData.Value.Y.ToString());

            //    string title = "Number\tSite\tResult\tTest Name\tChannel\tLow\t\tMeasured\tHigh\t\tForce\t\tLoc";
            //    wtSw.WriteLine(title);
            //    int itemCount = 1;
            //    int failCounter = 0;
            //    foreach (var mInfo in MsrtkeyInfoDic)
            //    {
            //        failCounter = 0;
            //        string passStr = "PASS";

            //        string str = itemCount + "\t" + (dieData.Value.Ch - 1).ToString();
            //        string format = mInfo.Value.MsrtData.Formate;
            //        int index = mInfo.Value.ColIndex;

            //        if((mInfo.Value.MsrtData.IsInSpec(dieData.Value.RawData[index])))
            //        {
            //            passStr = "PASS";
            //        }
            //        else
            //        {
            //            failCounter++;
            //            passStr = "FAIL";
            //        }
            //        str +=  "\t"+ passStr;
            //        str += "\t" + mInfo.Value.MsrtName;
            //        str += "\t\t" + dieData.Value.Ch;
            //        str += "\t" + mInfo.Value.MsrtData.MinLimitValue.ToString("0.0000") +" "+ mInfo.Value.MsrtData.Unit;

            //        str += "\t" + dieData.Value.RawData[index] + " "+ mInfo.Value.MsrtData.Unit;

            //        str += "\t" + mInfo.Value.MsrtData.MaxLimitValue.ToString("0.0000") + " " + mInfo.Value.MsrtData.Unit;
            //        string forceStr = "";
            //        if (mInfo.Value.TestData is LCRTestItem)
            //        {
            //            forceStr = (mInfo.Value.TestData as LCRTestItem).LCRSetting.DCBiasV.ToString(format);
            //        }
            //        else
            //        {
            //            forceStr = mInfo.Value.TestData.ElecSetting[0].ForceValue.ToString("0.0000");
            //        }
            //        str += "\t" + forceStr +" "+ mInfo.Value.TestData.ElecSetting[0].ForceUnit;
            //        str += "\t 0";
            //        wtSw.WriteLine(str);
            //        wtSw.WriteLine("======");
                    
            //        itemCount++;
            //    }

            //    wtSw.WriteLine("==PRR==");
            //    wtSw.WriteLine("");
            //    wtSw.WriteLine(" Site Failed tests/Executed tests");
            //    wtSw.WriteLine("------------------------------------");
            //    wtSw.WriteLine("    " + (dieData.Value.Ch - 1).ToString() + "\t\t" + failCounter + "\t" + (itemCount - 1));
            //    wtSw.WriteLine("");
            //    wtSw.WriteLine(" Site    Sort     Bin");
            //    wtSw.WriteLine("------------------------------------");
            //    wtSw.WriteLine("    " + (dieData.Value.Ch - 1).ToString() + "\t\t" + dieData.Value.Bin + "\t" + dieData.Value.Bin);
            //    wtSw.WriteLine("=========================================================================");

            //    testCount++;
                
            //}
            return EErrorCode.NONE;
        }
    }
}
