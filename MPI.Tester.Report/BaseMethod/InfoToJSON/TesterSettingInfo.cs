using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;
using Newtonsoft.Json;

namespace MPI.Tester.Report.BaseMethod.InfoToJSON
{
    public class TesterSettingInfoConverter
    {
        TesterSetting _sysSetting = new TesterSetting();
        public TesterSettingInfoConverter(TesterSetting sysSet)
        {
            _sysSetting = sysSet;
        }

        public string GetStringAsJSONValue()
        {
            string outStr = "";

            Dictionary<string, object> outDic = new Dictionary<string, object>();

            string tsStr = _sysSetting.StartTestTime.ToString("yyyy/MM/DD HH:mm:ss.fff");
            outDic.Add("ProberCoord", _sysSetting.ProberCoord);
            outDic.Add("TesterCoord", _sysSetting.TesterCoord);
            outDic.Add("StartTestTime", tsStr);
            outDic.Add("BinSortingRule", _sysSetting.BinSortingRule);
            outDic.Add("IsBinSortingIncludeMinMax", _sysSetting.IsBinSortingIncludeMinMax);
            outDic.Add("DefaultBinGrade", _sysSetting.DefaultBinGrade);
            outDic.Add("IsSpecBinTableSync", _sysSetting.IsSpecBinTableSync);
            outDic.Add("GroupBinRule", _sysSetting.GroupBinRule);
            // UISetting.GetPathWithFolder(pInfo);

            outStr = JsonConvert.SerializeObject(outDic, Formatting.Indented);

            return outStr;

        }


        #region

        #endregion

    }
}
