using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MPI.Tester.Data;

using MPI.Tester.Report.BaseMethod.HeaderFinder;
using MPI.Tester.Report.BaseMethod.PosKeyMaker;

namespace MPI.Tester.Report.User.DOWA
{
    partial class Report : ReportBase
    {
        ETestStage _stg = ETestStage.IV;
        Dictionary<string, AOISignItem> _posAOIDic = new Dictionary<string, AOISignItem>();
        bool useFormatA = true;

       // Dictionary<string, MapDieBase> _keyMDic;

        public Report(List<object> objs, bool isReStatistic)
            : base(objs, isReStatistic)
        {
           // _keyMDic = new Dictionary<string, MapDieBase>();
        }

        protected override void SetResultTitle()
        {
            Dictionary<string, string> knDic = GetKeyHeaderDic(this.UISetting.UserDefinedData.ResultItemNameDic);

            this.ResultTitleInfo.SetResultData(knDic);

            _stg = this.Product.TestCondition.TestStage;

            useFormatA = UISetting.FormatName.Contains("Format-A");
            //CoordTransTool
        }

        protected override EErrorCode WriteReportHeadByUser()
        {
            EErrorCode errCode = EErrorCode.NONE;

            if (useFormatA)
            {
                errCode = WriteReportHeadByUser_FormatA();
            }
            else
            {
                errCode = WriteReportHeadByUser_FormatD();
            }

            return errCode;
        }

        protected override EErrorCode PushDataByUser(Dictionary<string, double> data)
        {
            EErrorCode errCode = EErrorCode.NONE;

            if (useFormatA)
            {
                errCode = PushDataByUser_FormatA(data);
            }
            else
            {
                errCode = PushDataByUser_FormatD(data);
            }            

            return EErrorCode.NONE;
        }

        protected override EErrorCode RewriteReportByUser()
        {
            EErrorCode errCode = EErrorCode.NONE;

            if (useFormatA)
            {
                errCode = RewriteReportByUser_FormatA();
            }
            else
            {
                errCode = RewriteReportByUser_FormatD();
            }
            return errCode;
        }

        #region >>private mehtod<<
        private Dictionary<string, string> GetKeyHeaderDic(Dictionary<string, string> keyNameDic)
        {
            Dictionary<string, string> kvDic = new Dictionary<string, string>();

            if (keyNameDic != null)
            {
                foreach (var p in keyNameDic)
                {
                    string name = p.Value;
                    string key = p.Key;

                    bool found = false;

                    TestResultData rData = AcquireData[key];
                    if (rData != null)
                    {
                        found = true;
                        if (rData.Unit != "")
                        {
                            name = rData.Name + "(" + rData.Unit + ")";
                        }
                    }

                    if (!found)
                    {
                        var item = this.UISetting.UserDefinedData[key];
                        if (item != null)
                        {
                            string name1 = item.Name;
                            string unit = item.Unit;
                            name = name1;
                            if (unit != "")
                            { name += "(" + unit + ")"; }
                        }
                    }
                    kvDic.Add(key, name);
                }
            }
            return kvDic;
        }

        #endregion

     
    }
}
