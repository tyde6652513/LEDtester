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

        EErrorCode OutPutFile4()
        {
            string tempFileName = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, WAVETEK_Tmp);

            string tarFileName = GetFile4Name();
            
            wtSw = new StreamWriter(tempFileName, false, this._reportData.Encoding);

            CreateFile4Head();

            PushDataToFile4();

            wtSw.Close();

            wtSw.Dispose();

            wtSw = null;

            bool result = true;

            if (UISetting.PathInfoArr[6].EnablePath)
            {
                string fullTarName1 = Path.Combine(UISetting.PathInfoArr[6].TestResultPath, tarFileName);
                if (!CopyFileOrBackUp(tempFileName, tarFileName, fullTarName1))
                    result = false;
            }

            if (UISetting.PathInfoArr[7].EnablePath)
            {
                string fullTarName2 = Path.Combine(UISetting.PathInfoArr[7].TestResultPath, tarFileName);
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

        private string GetFile4Name()
        {
            string fileName = UISetting.WaferNumber;
            if (UISetting.PathInfoArr != null)
            {
                fileName = UISetting.LotNumber + UISetting.WaferNumber + "_";

                string startTime = "";
                if (SetDataInfo.ContainsKey("TestStartTime"))
                {
                    DateTime dt;
                    DateTime.TryParse(SetDataInfo["TestStartTime"], out dt);
                    startTime = dt.ToString("yyyyMMddHHmmss");
                    fileName += startTime;
                }

                if (IsCvTest)
                {
                    fileName += ".CP2";
                }
                else
                {
                    fileName += ".CP1";
                }

            }
            return fileName;
        }

        private EErrorCode CreateFile4Head()
        {
            DateTime time;
            string startTimeStr = "", endTimeStr = "";
            //if (DateTime.TryParse(SetDataInfo["TestStartTime"], out time))
            //{
            //    startTimeStr = time.ToString("yyyy/MM/dd HH:mm:ss");
            //}

            startTimeStr = this.UISetting.WaferBeginTime.ToString("yyyy/MM/dd HH:mm:ss ");
            if (DateTime.TryParse(SetDataInfo["TestEndTime"], out time))
            {
                endTimeStr = time.ToString("yyyy/MM/dd HH:mm:ss");
            }

            Dictionary<int, long> ChDieDic = GetDieNumInCh();

            wtSw.WriteLine("[BOF]");
            wtSw.WriteLine("PRODUCT ID          : " + Product.ProductName);
            wtSw.WriteLine("LOT ID              : " + UISetting.LotNumber);
            wtSw.WriteLine("WAFER ID            : " + UISetting.WaferNumber);
            wtSw.WriteLine("FLOW ID             : CP" + (IsCvTest ? "2" : "1"));
            wtSw.WriteLine("START TIME          : " + startTimeStr);
            wtSw.WriteLine("STOP TIME           : " + endTimeStr);
            wtSw.WriteLine("SUBCON              : UMC01");
            wtSw.WriteLine("TESTER NAME         : " + UISetting.MachineName);
            wtSw.WriteLine("TEST PROGRAM        : " + UISetting.TaskSheetFileName);
            wtSw.WriteLine("LOAD BOARD ID       : ");
            wtSw.WriteLine("PROBE CARD BOARD ID : ");
            string esName = "";
            if (SetDataInfo.ContainsKey("EdgeSensorName"))
            {
                esName = SetDataInfo["EdgeSensorName"];
            }
            wtSw.WriteLine("PROBE CARD ID       : " + esName);
            string chNum = "1";
            if (SetDataInfo.ContainsKey("ChannelQty"))
            {
                chNum = SetDataInfo["ChannelQty"];
            }
            wtSw.WriteLine("SITE NUM            : " + chNum);
            wtSw.WriteLine("DUT ID         : X");
            wtSw.WriteLine("DUT DIFF NUM   : 40");
            wtSw.WriteLine("OPERATOR ID    : " + UISetting.OperatorName);

            string passDieStr = dieLogDic.Count + " (SITE : ";
            foreach (var chDieNum in ChDieDic)
            {
                passDieStr += chDieNum.Value + ",";
            }
            passDieStr = passDieStr.TrimEnd(',') + ")";

            wtSw.WriteLine("TESTED DIE     : " + passDieStr);
            double yield = 0;
            if (dieLogDic.Count != 0)
            {
                double passCne = (double)GetPassCount();
                yield = ( passCne / (double)dieLogDic.Count);
            }
            wtSw.WriteLine("PASS DIE            : " + GetPassCount());
            wtSw.WriteLine("YIELD               : " + yield.ToString("P2"));
            wtSw.WriteLine("SOURCE NOTCH        : DOWN");


            int rowDiff = Math.Abs(top - down) + 1;
            int colDiff = Math.Abs(right - left) + 1;

            wtSw.WriteLine("MAP ROW             : " + rowDiff);
            wtSw.WriteLine("MAP COLUMN          : " + colDiff);
            wtSw.WriteLine("MAP BIN LENGTH : 2");
            wtSw.WriteLine("");
            CreateBinTable();
            wtSw.WriteLine("");
            return EErrorCode.NONE;
        }

        private EErrorCode PushDataToFile4()
        {
            wtSw.WriteLine("[SOFT BIN MAP]");

            string hstr = "    ";
            string tstr = "    ";
            string ostr = "    ";

            for (int i = 1; i <= right; ++i)
            {
                hstr += "  " + GetIndexNum(i, 3);
                tstr += "  " + GetIndexNum(i, 2);
                ostr += "  " + GetIndexNum(i, 1);
            }
            wtSw.WriteLine(hstr);
            wtSw.WriteLine(tstr);
            wtSw.WriteLine(ostr);

            //List<DieInfo> orderList = (from die in dieDic
            //                           orderby die.Value.X
            //                           orderby die.Value.Y
            //                           select die.Value).ToList();

            for (int y = 1; y <= top; y++)
            {
                string rowStr = y.ToString().PadLeft(3, '0') + " ";
                for (int x = 1; x <= right; x++)
                {
                    string keystr = x.ToString() + "_" + y.ToString();
                    if (dieLogDic.ContainsKey(keystr))
                    {
                        rowStr += dieLogDic[keystr].Bin.ToString().PadLeft(3, ' ');
                    }
                    else
                    {
                        rowStr += "   ";
                    }

                }
                wtSw.WriteLine(rowStr);
            }


            DateTime dts, dte;
            string testTime = "";
            if (SetDataInfo.ContainsKey("TestStartTime") && SetDataInfo.ContainsKey("TestEndTime"))
            {
                dts = UISetting.WaferBeginTime;
                if (//DateTime.TryParse(SetDataInfo["TestStartTime"], out dts) &&
                    DateTime.TryParse(SetDataInfo["TestEndTime"], out dte))
                {
                    TimeSpan ts = new TimeSpan(dte.Ticks - dts.Ticks);
                    testTime = ts.ToString(@"hh\:mm\:ss");
                }
            }

            wtSw.WriteLine("");
            wtSw.WriteLine("[EXTENSION]");
            wtSw.WriteLine("");
            wtSw.WriteLine("[EOF]");
            wtSw.WriteLine("");
            wtSw.WriteLine("WaferCycle Time : " + testTime);

            long testCnt = dieLogDic.Count();
            long retestCnt = dieLogDic.Count(x => x.Value.IsRetest);
            wtSw.WriteLine("Retest Die : " + testCnt);

            float rePercent = ((float)retestCnt / (float)testCnt);
            wtSw.WriteLine("Retest Rate: " + rePercent.ToString("P"));
            //[EXTENSION]

            //[EOF]

            //WaferCycle Time : 02:15:05
            //Retest Die : 0
            //Retest Rate: 0.00%
            return EErrorCode.NONE;
        }

        private string GetIndexNum(int value, int index)
        {
            double fValue = (double)value;
            double baseNum = Math.Pow(10, index - 1);
            double devNum = fValue / baseNum;
            double outVal = Math.Floor(devNum);
            outVal = (outVal % 10);

            return outVal.ToString("0");

        }

        private int GetPassCount()
        {
            int passNum = 0;
            foreach (var die in dieLogDic)
            {
                if (die.Value.IsPass)
                {
                    passNum++;
                }
            }

            return passNum;

        }

        private Dictionary<int, long> GetDieNumInCh()
        {
            Dictionary<int, long> chPassDic = new Dictionary<int, long>();

            foreach (var die in dieLogDic)
            {
                if (!chPassDic.ContainsKey(die.Value.Ch))
                {
                    chPassDic.Add(die.Value.Ch, 0);
                }
                chPassDic[die.Value.Ch]++;
            }
            return chPassDic;
        }

        private void CreateBinTable()
        {

            int chNum = 1;
            if (SetDataInfo.ContainsKey("ChannelQty"))
            {
                int.TryParse(SetDataInfo["ChannelQty"], out chNum);
            }

            int allDieQty = dieLogDic.Count;
            Dictionary<int, Dictionary<int, long>> binStatic = CalcBinStatic();

            wtSw.WriteLine("[SOFT BIN]");
            string tittleStr = "        BINNAME, DIENUM,  YIELD,";
            for (int i = 1; i <= chNum; ++i)
            {
                tittleStr += "  SITE" + (i).ToString() + ",";
            }
            tittleStr += " DESCRIPTION";
            wtSw.WriteLine(tittleStr);

            List<SmartBinData> bList = new List<SmartBinData>();
            //foreach (var bData in this.SmartBinning.SmartBin)
            //{
            //    bList.Add(bData);
            //}
            //bList.OrderBy(x => x.BinNumber);

            bList = (from data in this.SmartBinning.SmartBin
                     orderby data.BinNumber
                     select data).ToList();

            string emptyStr = "    BIN,      0,      0," + 0.ToString("P2").PadLeft(7, ' ') + ",";
            for (int i = 1; i <= chNum; ++i)
            {
                emptyStr += "      0,";
            }

            emptyStr += " {[AVI SKIP DIE]}";
            wtSw.WriteLine(emptyStr);
            //foreach (var bData in this.SmartBinning.SmartBin)

            List<int> usedBinList = new List<int>();
            foreach (var bData in bList)
            {
                string bingStr = "    BIN,";
                bingStr += bData.BinNumber.ToString().PadLeft(7, ' ') + ",";
                if (!usedBinList.Contains(bData.BinNumber))
                {
                    if (binStatic.ContainsKey(bData.BinNumber))
                    {
                        long dieInBin = binStatic[bData.BinNumber].Sum(x => x.Value);
                        bingStr += dieInBin.ToString().PadLeft(7, ' ') + ",";
                        double yield = (double)dieInBin / (double)allDieQty;
                        bingStr += yield.ToString("P2").PadLeft(7, ' ') + ",";

                        for (int i = 1; i <= chNum; ++i)
                        {
                            if (binStatic[bData.BinNumber].ContainsKey(i))
                            {
                                bingStr += binStatic[bData.BinNumber][i].ToString().PadLeft(7, ' ') + ",";
                            }
                            else
                            {
                                bingStr += "      0,";
                            }
                        }                        
                    }
                    else
                    {
                        bingStr += "      0,  0.00%,";
                        for (int i = 1; i <= chNum; ++i)
                        {
                            bingStr += "      0,";
                        }
                    }

                    bingStr += " {[" + bData.BinCode + "]}";

                    wtSw.WriteLine(bingStr);

                    usedBinList.Add(bData.BinNumber);
                }

            }

        }

        private Dictionary<int, Dictionary<int, long>> CalcBinStatic()
        {
            Dictionary<int, Dictionary<int, long>> binDic = new Dictionary<int, Dictionary<int, long>>();

            foreach (var die in dieLogDic)
            {
                int bin = die.Value.Bin;
                int ch = die.Value.Ch;
                if (!binDic.ContainsKey(bin))
                {
                    Dictionary<int, long> chDic = new Dictionary<int, long>();
                    binDic.Add(bin, chDic);
                }

                if (!binDic[bin].ContainsKey(ch))
                {
                    binDic[bin].Add(ch, 0);
                }
                binDic[bin][ch]++;
            }
            return binDic;
        }


    }
}
