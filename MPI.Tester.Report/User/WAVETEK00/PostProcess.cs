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

        Dictionary<string, string> SetDataInfo;

        Dictionary<string, DieLog> dieLogDic;

        Dictionary<string, GroupInfo> baseDieGropuDic;

        List<DieRawInfo> dieListByTime;

        const string WAVETEK_Tmp = "wtkTmp.data";

        Dictionary<int, Point> ch_YshiftDic;

        StreamWriter wtSw;
        int pass_fail_Index = -1;

        int top, down, left, right;

        EErrorCode LoadRepfile(string fullFileName)
        {
            SetDataInfo = new Dictionary<string, string>();
            dieLogDic = new Dictionary<string, DieLog>();
            dieListByTime = new List<DieRawInfo>();
            baseDieGropuDic = new Dictionary<string, GroupInfo>();
            if (File.Exists(fullFileName))
            {
                string extStr =  Path.GetExtension(fullFileName);
                string backUpFie = Path.GetFileNameWithoutExtension(fullFileName);
                backUpFie += DateTime.Now.ToString("yyMMdd_HHmmss");

                MPIFile.CopyFile(fullFileName, backUpFie);

                #region >>load row data<<
                using (StreamReader srCheckRowCol = new StreamReader(fullFileName, this._reportData.Encoding))
                {
                    bool isRawData = false;

                    pass_fail_Index = -1;
                    int count = 0;
                    foreach (var pData in this.UISetting.UserDefinedData.ResultItemNameDic)
                    {
                        if (pData.Key != ESysResultItem.ISALLPASS.ToString())
                        {
                            count++;
                        }
                        else
                        {
                            pass_fail_Index = count;
                            break;
                        }
                    }

                    //重繞tmp檔取得 ColRowkey 對應的第幾筆數據
                    while (srCheckRowCol.Peek() >= 0)
                    {
                        string line = srCheckRowCol.ReadLine();

                        if (isRawData)
                        {
                            string[] rawData = line.Split(',');

                            string colrowKey = rawData[this.ResultTitleInfo.ColIndex].ToString() + "_" + rawData[this.ResultTitleInfo.RowIndex].ToString();

                            DieRawInfo dInfo = new DieRawInfo();
                            int.TryParse(rawData[this.ResultTitleInfo.ColIndex], out dInfo.X);
                            int.TryParse(rawData[this.ResultTitleInfo.RowIndex], out dInfo.Y);
                            int.TryParse(rawData[this.ResultTitleInfo.BinIndex], out dInfo.Bin);
                            int.TryParse(rawData[this.ResultTitleInfo.CHIndex], out dInfo.Ch);
                            float.TryParse(rawData[this.ResultTitleInfo.SeqTimeIndex], out dInfo.TestTime);

                            if (rawData[pass_fail_Index].StartsWith("1"))
                            {
                                dInfo.IsPass = true;
                            }
                            else
                            {
                                dInfo.IsPass = false;
                            }

                            dInfo.RawData = rawData.Clone() as string[];

                            dieListByTime.Add(dInfo);

                            int ListCnt = dieListByTime.Count;

                            if (dieLogDic.ContainsKey(colrowKey))
                            {
                                dieLogDic[colrowKey].Push(dieListByTime[ListCnt-1]);
                            }
                            else
                            {
                                DieLog dl = new DieLog(dieListByTime[ListCnt - 1]);
                                dieLogDic.Add(colrowKey, dl);
                            }
                        }
                        else
                        {
                            if (line == this.ResultTitleInfo.TitleStr)
                            {
                                isRawData = true;
                            }

                            string[] strArr = line.Split(',');
                            if (strArr.Length == 2)
                            {
                                SetDataInfo.Add(strArr[0], strArr[1]);
                            }
                            else if (strArr.Length == 1)
                            {
                                SetDataInfo.Add(strArr[0], "");
                            }

                        }
                    }
                }
                #endregion
                GetChannel2YShift();
                GetGroupDic();
            }
            ComputeBoundary(out top, out down, out left, out right);
            return EErrorCode.NONE;
        }

        EErrorCode PostProcess(string fullFileName)
        {

            EErrorCode err = EErrorCode.NONE; 
            LoadRepfile(fullFileName);

            if (SetDataInfo == null ||
                SetDataInfo.Count == 0)
            {
                return EErrorCode.REPORT_ConvertFail;
            }


            try
            {

                if (UISetting.PathInfoArr[0].EnablePath || UISetting.PathInfoArr[1].EnablePath)
                {
                    EErrorCode tErr = OutPutFile1();
                    if (tErr != EErrorCode.NONE)
                        err = tErr;
                }

                if (UISetting.PathInfoArr[2].EnablePath || UISetting.PathInfoArr[3].EnablePath)
                {
                    EErrorCode tErr = OutPutFile2();
                    if (tErr != EErrorCode.NONE)
                        err = tErr;
                }
                if (UISetting.PathInfoArr[4].EnablePath || UISetting.PathInfoArr[5].EnablePath)
                {
                    EErrorCode tErr = OutPutFile3();
                    if (tErr != EErrorCode.NONE)
                        err = tErr;
                }
                if (UISetting.PathInfoArr[6].EnablePath || UISetting.PathInfoArr[7].EnablePath)
                {
                    EErrorCode tErr = OutPutFile4();
                    if (tErr != EErrorCode.NONE)
                        err = tErr;
                }
                if (UISetting.PathInfoArr[8].EnablePath || UISetting.PathInfoArr[9].EnablePath)
                {
                    EErrorCode tErr = OutPutFile5();
                    if (tErr != EErrorCode.NONE)
                        err = tErr;
                }
                if (UISetting.PathInfoArr[10].EnablePath || UISetting.PathInfoArr[11].EnablePath)
                {
                    EErrorCode tErr = OutPutFile6();
                    if (tErr != EErrorCode.NONE)
                        err = tErr;
                }

                if (err != EErrorCode.NONE)
                {
                    return err;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[Report WTK], PostProcess(), catch:" + e.ToString());
                return EErrorCode.REPORT_ConvertFail;
            }
            return EErrorCode.NONE;
        }

        private void ComputeBoundary(out int top, out int down, out int left, out int right)
        {
            top = int.MinValue;
            down = int.MaxValue;
            left = int.MaxValue;
            right = int.MinValue;
            foreach (var die in dieLogDic)
            {
                if (top < die.Value.Y)
                { top = die.Value.Y; }
                if (down > die.Value.Y)
                { down = die.Value.Y; }

                if (left > die.Value.X)
                { left = die.Value.X; }
                if (right < die.Value.X)
                { right = die.Value.X; }

            }
        }

        private void GetChannel2YShift()
        {
            int colXCount = this.MachineConfig.ChannelConfig.ColXCount;
            int channel = this.MachineConfig.ChannelConfig.ChannelCount;

            int deltaColX = 0;
            int deltaRowY = 0;

            ch_YshiftDic = new Dictionary<int, Point>();

            ch_YshiftDic.Add(0, new Point(0, 0));
            ch_YshiftDic.Add(1, new Point(0,0));
            if (channel == 0 ||
                channel == 1)
            {
                //ch_YshiftDic.Add(1, 0);
            }
            else
            {
                for (int i = 1; i < channel; ++i)
                {
                    deltaRowY = Math.DivRem(i, colXCount, out deltaColX);
                    //if (deltaColX == 0)
                    //{
                    //    deltaRowY--;
                    //}
                    //deltaRowY *= (-1);

                    switch (this.TesterSetting.TesterCoord)
                    {
                        case 1:
                        case 2:
                            deltaRowY *= (-1);
                            break;
                    }

                    switch (this.TesterSetting.ProberCoord)
                    {
                        case 2:
                            deltaColX *= (-1);
                            break;
                        case 3:
                            deltaColX *= (-1);
                            deltaRowY *= (-1);
                            break;
                        case 4:
                            deltaRowY *= (-1);
                            break;
                        default:
                            break;
                    }

                    switch (this.TesterSetting.TesterCoord)
                    {
                        case 2:
                            deltaColX *= (-1);
                            break;
                        case 3:
                            deltaColX *= (-1);
                            deltaRowY *= (-1);
                            break;
                        case 4:
                            deltaRowY *= (-1);
                            break;
                        default:
                            break;
                    }

                    ch_YshiftDic.Add(i + 1, new Point(deltaColX, deltaRowY));
                    // 推算剩餘的 Channel Row/Col
                }
            }
            
        }

        private Point GetBase(int col, int row, int ch)
        {
            //if(ch_YshiftDic != null)
            if (ch == 0 ||
                ch == 1)
            {
                return new Point(col, row);
            }
            int x = col - ch_YshiftDic[ch].X;
            int y = row - ch_YshiftDic[ch].Y;
            return new Point(x, y);
        }

        private void GetGroupDic()
        {
            foreach (var dieData in dieLogDic)
            {
                Point bp = GetBase(dieData.Value.X, dieData.Value.Y, dieData.Value.Ch);
                string key = bp.X.ToString() + "_" + bp.Y.ToString();
                if (!baseDieGropuDic.ContainsKey(key))
                {
                    baseDieGropuDic.Add(key, new GroupInfo(bp.X, bp.Y));
                }
                baseDieGropuDic[key].Push(dieData.Value);

                //baseDieGropuDic
            }
        }

        class DieLog
        {
            public List<DieRawInfo> DieList;

            public int FailTestCnt = 0;
            public int PassTestCnt = 0;
            public DieLog(DieRawInfo dInfo)
            {
                DieList = new List<DieRawInfo>();
                Push(dInfo);
            }

            public void Push(DieRawInfo dInfo)
            {
                DieList.Add(dInfo);
            }

            public void ResetCounter()
            { 
                FailTestCnt = 0;
                PassTestCnt = 0;
            }

            #region>>property<<
            private int index 
            {
                get { return DieList.Count -1; }
            }

            public int ListLength
            {
                get { return DieList.Count; }
            }
            
            public int X
            {
                get { return DieList[index].X; }
            }
            public int Y
            {
                get { return DieList[index].Y; }
            }
            public int Bin
            {
                get { return DieList[index].Bin; }
            }
            public int Ch
            {
                get { return DieList[index].Ch; }
            }
            public float TestTime
            {
                get { return DieList[index].TestTime; }
            }
            public DateTime StartTime
            {
                get { return DieList[index].StartTime; }
            }
            public string[] RawData
            {
                get { return DieList[index].RawData; }
            }
            public bool IsPass
            {
                get { return DieList[index].IsPass; }
            }

            public bool IsRetest
            {
                get { return DieList.Count > 1 ? true : false; }
            }
            #endregion
        }
     
        class DieRawInfo
        {
            public int X;
            public int Y;
            public int Bin;
            public int Ch;
            public float TestTime;
            public DateTime StartTime;
            public string[] RawData;
            public bool IsPass;

            public DieRawInfo()
            {
                X = int.MinValue;
                Y = int.MinValue;
                Bin = int.MinValue;
                Ch = int.MinValue;
                TestTime = float.MinValue;
                IsPass = true;
                RawData = null;
                StartTime = DateTime.Now;
            }

            

        }

        class MsrtInfo
        {
            public TestResultData MsrtData;

            public TestItemData TestData;

            public int ColIndex = 0;

            public MsrtInfo(TestResultData mData, TestItemData tData,int colIndex)
            {
                MsrtData = mData.Clone() as TestResultData;
                TestData = tData.Clone() as TestItemData;
                ColIndex = colIndex;
            }

            #region public property
            public string MsrtKey
            {
                get { return MsrtData.KeyName; }
            }
            public string MsrtName
            {
                get { return MsrtData.Name; }
            }

            public string TestKey
            {
                get { return TestData.KeyName; }
            }
            public string TestName
            {
                get { return TestData.Name; }
            }

            //public List<string> NameList
            //{
            //    get;
            //    set;
            //}

            #endregion

            #region>>public method<<
            public string GetCustomerName_1base(int i)
            {
                //if (NameList != null && NameList.Count >= i)
                //{
                //    return NameList[i-1];
                //}
                return MsrtData.Name;
 
            }
            #endregion
        }

        class Point
        {
            public int X, Y;
            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        class GroupInfo
        {
            public int RefCol, RefRow;
            public Dictionary<int, DieLog> ChDieDic;
            public GroupInfo(int col, int row)
            {
                RefCol = col;
                RefRow = row;
                ChDieDic = new Dictionary<int, DieLog>();                
            }

            public void Push(DieLog dl)
            {
                int channel = dl.Ch;
                if(ChDieDic.ContainsKey(channel))
                {
                    ChDieDic[channel] = dl;
                }
                else
                {
                    ChDieDic.Add(channel,dl);
                }
            }

            public void ResetCounter()
            {
                foreach (var d in ChDieDic)
                {
                    d.Value.ResetCounter();
                }
            }

            public int X
            {
                get 
                {
                    if (ChDieDic.Count != 0)
                    {
                        if (ChDieDic.ContainsKey(1))
                        {
                            return ChDieDic[1].X;
                        }
                        else if (ChDieDic.ContainsKey(0))
                        {
                            return ChDieDic[0].X;
                        }
                        else
                        {
                            foreach (var d in ChDieDic)
                            {
                                return d.Value.X;
                            }
                        }
                    }

                    return RefCol;
                }
            }

            public int Y
            {
                get
                {
                    if (ChDieDic.Count != 0)
                    {
                        if (ChDieDic.ContainsKey(1))
                        {
                            return ChDieDic[1].Y;
                        }
                        else if (ChDieDic.ContainsKey(0))
                        {
                            return ChDieDic[0].Y;
                        }
                        else
                        {
                            foreach (var d in ChDieDic)
                            {
                                return d.Value.Y;
                            }
                        }
                    }

                    return RefRow;
                }
            }
            
        }

        //class TestResultDataEx : TestResultData
        //{
        //    public List<string> NameList { get; set; }

        //    public TestResultDataEx()
        //        : base()
        //    {
        //        NameList = new List<string>();
        //    }
        //    public TestResultDataEx(TestResultData rData)
        //    {
        //        base = rData as TestResultDataEx;
        //    }

        //    public TestResultDataEx(string unit, string formatStr)
        //        : base(unit, formatStr)
        //    {
        //        NameList = new List<string>();
        //    }

        //    public TestResultDataEx(string keyName, string name, string unit, string formatStr) :
        //        base(keyName, name, unit, formatStr)
        //    {
        //        NameList = new List<string>();
        //    }

        //    public object Clone()
        //    {
        //        //--------------------------------
        //        // All field is value type
        //        //--------------------------------

        //        TestResultDataEx cloneObj = (TestResultDataEx)this.MemberwiseClone();

        //        cloneObj.NameList = new List<string>();
        //        if (this.NameList != null)
        //        {
        //            foreach (var nData in NameList)
        //            {
        //                cloneObj.NameList.Add(nData.ToString());
        //            }                    
        //        }

        //        return cloneObj;
        //    }
            
        //}
    }
}
