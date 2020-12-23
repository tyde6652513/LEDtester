using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using MPI.Tester.Data;
using MPI.Tester.TestServer;

namespace MPI.Tester.Report.User.ASE
{
    class Report : ReportBase
    {
        List<TestInfo> ItemOrderList;

        public Report(List<object> objs, bool isReStatistic)
            : base(objs, isReStatistic)
        {
        }

        protected override void SetResultTitle()
        {
            this.ResultTitleInfo.SetResultData(this.UISetting.UserDefinedData.ResultItemNameDic);


            Dictionary<string, string> keyNameDic = new Dictionary<string, string>();
            ItemOrderList = new List<TestInfo>();////直接寫死
            Dictionary<string,string> MsrtkeyNameDic = new Dictionary<string,string> ();

            if (this.Product.TestCondition.TestItemArray != null)
            {
                foreach (TestItemData testItem in this.Product.TestCondition.TestItemArray)
                {
                    if (testItem != null && testItem.IsEnable)
                    {

                        foreach (TestResultData rData in testItem.MsrtResult)
                        {
                            if (rData != null && rData.IsEnable && ResultData.ContainsKey(rData.KeyName))
                            {
                                string name = rData.Name;
                                bool IsShow = rData.IsEnable && testItem.IsEnable;
                                if (IsShow)
                                {
                                    TestInfo tInfo = new TestInfo(rData.KeyName, name, rData.Formate, (float)rData.MaxLimitValue, (float)rData.MinLimitValue);

                                    tInfo.IsShow = IsShow;
                                    ItemOrderList.Add(new TestInfo(rData.KeyName, name, rData.Formate, (float)rData.MaxLimitValue, (float)rData.MinLimitValue));
                                    MsrtkeyNameDic.Add(rData.KeyName, name);
                                }
                            }
                        }
                    }
                }

            }

            this.ResultTitleInfo2.SetResultData(MsrtkeyNameDic);
        }

        protected override EErrorCode WriteReportHeadByUser()
        {
            return this.WriteReportHeadByDefault();
        }

        protected override EErrorCode RewriteReportByUser()
        {
            return this.RewriteReportByDefault();
        }


        protected override EErrorCode MoveFileToTargetByUser(EServerQueryCmd cmd)
        {
            bool isOutputPath03 = false;

            string outPath01 = string.Empty;

            string outPath03 = string.Empty;

            if (this.UISetting.IsManualRunMode)
            {
                outPath01 = GetPathWithFolder(UISetting.ManualOutPathInfo01);

                outPath03 = GetPathWithFolder(UISetting.ManualOutPathInfo03);
                isOutputPath03 = UISetting.ManualOutPathInfo03.EnablePath;
            }
            else
            {
                outPath01 = GetPathWithFolder(UISetting.OutPathInfo01);

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

            fileNameWithExt = fileNameWithoutExt + "." + this.UISetting.TestResultFileExt;

            string outputPathAndFile01 = Path.Combine(outPath01, fileNameWithExt);
            MPIFile.CopyFile(this.FileFullNameRep, outputPathAndFile01);

        
            string outputPathAndFile03 = Path.Combine(outPath03, fileNameWithExt);
            if (isOutputPath03)
            {
                MPIFile.CopyFile(this.FileFullNameRep, outputPathAndFile03);
            }

            
            return EErrorCode.NONE;
        }



        #region>> file 2<<

        protected override EErrorCode WriteReportHeadByUser2()
        {
            string headLine = "";
            string space = "";
            string minLine = "";
            string maxLine = "";

            foreach (TestInfo tData in ItemOrderList)
            {
                headLine += tData.Name + ",";
                space += ",";
                minLine += tData.MinLim + ",";
                maxLine += tData.MaxLim + ",";
            }

            this.WriteLine2(headLine.TrimEnd(','));

            int length = space.Length;

            if (length > 2)
            {
                this.WriteLine2(space.Substring(1));//spec欄位空白
            }

            this.WriteLine2(minLine.TrimEnd(','));

            this.WriteLine2(maxLine.TrimEnd(','));

            return EErrorCode.NONE;
        }

        protected override EErrorCode PushDataByUser2(Dictionary<string, double> data)
        {
            string resultStr = "";
            foreach (TestInfo tData in ItemOrderList)
            {
                string key = tData.Key;
                if (data.ContainsKey(key))
                {
                    resultStr += data[key].ToString(tData.Formate) + ",";
                }
            }
            this.WriteLine2(resultStr.TrimEnd(','));
            return EErrorCode.NONE;

        }

        protected override EErrorCode RewriteReportByUser2()
        {

            List<List<string>> strLL = new List<List<string>>();
            ///////////////////////////////////////////////////////
            //Replace Data And Check Row Col
            ///////////////////////////////////////////////////////
            StreamWriter sw = new StreamWriter(this.FileFullNameRep2, false, Encoding.Default);

            StreamReader sr = new StreamReader(this.FileFullNameTmp2, Encoding.Default);

            int rawLineCount = 0;

            // 開始比對ColRowKey並寫檔
            while (sr.Peek() >= 0)
            {
                List<string> strList = sr.ReadLine().Split(new char[] { ',' }).ToList();


                for (int i = 0; i < strList.Count(); ++i)
                {
                    if (rawLineCount == 0)
                    {
                        strLL.Add(new List<string>());
                    }
                    strLL[i].Add(strList[i]);

                }
                rawLineCount++;
            }

            string outStr = "";
            foreach (List<string> strList in strLL)
            {
                outStr = "";
                foreach (string str in strList)
                {
                    outStr += str + ",";
                }

                sw.WriteLine(outStr.TrimEnd(','));
            }

            sr.Close();

            sr.Dispose();

            sw.Close();

            sw.Dispose();

            return EErrorCode.NONE;
        }

        protected override EErrorCode MoveFileToTargetByUser3(EServerQueryCmd cmd)
        {
            bool isOutputPath02 = false;      

            string outPath01 = string.Empty;

            string outPath02 = string.Empty;

            if (this.UISetting.IsManualRunMode)
            {
                outPath01 = GetPathWithFolder(UISetting.ManualOutPathInfo01);
                outPath02 = GetPathWithFolder(UISetting.ManualOutPathInfo02);
                isOutputPath02 = UISetting.ManualOutPathInfo02.EnablePath;
            }
            else
            {
                outPath01 = GetPathWithFolder(UISetting.OutPathInfo01);
                outPath02 = GetPathWithFolder(UISetting.OutPathInfo02);
                isOutputPath02 = UISetting.OutPathInfo02.EnablePath;
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
            if (outputPathAndFile02 == outputPathAndFile01)
            {
                //避免檔名重複被覆蓋
                fileNameWithExt = fileNameWithoutExt + "_IQC" + "." + this.UISetting.TestResultFileExt;
                outputPathAndFile02 = Path.Combine(outPath02, fileNameWithExt);
            }

            if (isOutputPath02)
            {
                MPIFile.CopyFile(this.FileFullNameRep2, outputPathAndFile02);
            }



            return EErrorCode.NONE;
        }
        #endregion
    }



    internal class TestInfo//: IEquatable<Testinnfo>
    {
        private bool _isShow = false;
        private string _name = "";

        public string Key = "";

        public string Formate = "";

        public float MaxLim = 9999999;
        public float MinLim = -9999999;

        public TestInfo()
        {
            _isShow = false;
        }

        public TestInfo(string key, string name = "", string format = "", float max = 9999999, float min = -9999999)
            : this()
        {
            Key = key;
            if (name == "")
            {
                _isShow = false;
            }
            else
            {
                Name = name;
            }
            Formate = format;
            MaxLim = max;
            MinLim = min;
        }


        public void SetData(string name, string format, float max, float min)
        {
            Name = name;
            Formate = format;
            MaxLim = max;
            MinLim = min;
            _isShow = true;
        }
        #region >>property<<
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                _isShow = true;
            }
        }
        public bool IsShow
        {
            get { return _isShow; }
            set { _isShow = value; }
        }
        #endregion
    }
}
