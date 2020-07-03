using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;
using Newtonsoft.Json;

namespace MPI.Tester.Report.User.WAVETEK00
{
    public class BinDataInfo
    {
       SmartBinning _smartBin = new SmartBinning();
       public BinDataInfo(SmartBinning sBin)
       {
            _smartBin = sBin;
       }

       public string GetStringAsJSONValue()
       {
            string outStr = "";

            Dictionary<string, object> outDic = GetInfoDic();


            outStr = JsonConvert.SerializeObject(outDic, Formatting.Indented);

            return outStr;

        }

       public Dictionary<string, object> GetInfoDic()
       {
           Dictionary<string, object> outDic = new Dictionary<string, object>();

           outDic.Add("NGBin", _smartBin.NGBin.GetNGBinInfoList());
           outDic.Add("SideBin", _smartBin.SideBin.GetSideinInfoList());
           outDic.Add("SmartBin", _smartBin.SmartBin.GetSmartBinInfo());
           return outDic;
       }


        #region
    
        #endregion

    }
}
