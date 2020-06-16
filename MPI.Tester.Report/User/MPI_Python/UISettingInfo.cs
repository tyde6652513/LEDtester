using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;
using Newtonsoft.Json;

namespace MPI.Tester.Report.User.MPI_Python
{
    public class UISettingInfoConverter
    {
        UISetting _uiSetting = new UISetting();
        TesterSetting _sysSetting = new TesterSetting();
        public UISettingInfoConverter(UISetting uiset, TesterSetting sysSet)
        {
            _uiSetting = uiset;
            _sysSetting = sysSet;
        }

        public string GetStringAsJSONValue()
        {
            string outStr = "";

            Dictionary<string, object> outDic = GetInfoDic();

     

            // UISetting.GetPathWithFolder(pInfo);

            outStr = JsonConvert.SerializeObject(outDic, Formatting.Indented);

            return outStr;

        }

        public Dictionary<string, object> GetInfoDic()
        {
            Dictionary<string, object> outDic = new Dictionary<string, object>();


            outDic.Add("OutathDic", GetPathDic());

            string tsStr = _sysSetting.StartTestTime.ToString("yyyy/MM/dd HH:mm:ss.fff");
            outDic.Add("ProberCoord", _sysSetting.ProberCoord);
            outDic.Add("TesterCoord", _sysSetting.TesterCoord);
            outDic.Add("StartTestTime", tsStr);
            outDic.Add("BinSortingRule", _sysSetting.BinSortingRule);
            outDic.Add("IsBinSortingIncludeMinMax", _sysSetting.IsBinSortingIncludeMinMax);
            outDic.Add("DefaultBinGrade", _sysSetting.DefaultBinGrade);
            outDic.Add("IsSpecBinTableSync", _sysSetting.IsSpecBinTableSync);
            outDic.Add("GroupBinRule", _sysSetting.GroupBinRule);

            outDic.Add("SoftwareVersoin", _uiSetting.SoftwareVersoin);
            outDic.Add("ProberSubRecipe", _uiSetting.ProberSubRecipe);
            outDic.Add("SubPiece", _uiSetting.SubPiece);
            outDic.Add("TaskSheetFileName", _uiSetting.TaskSheetFileName);
            outDic.Add("TestResultFileName", _uiSetting.TestResultFileName);
            outDic.Add("MachineName", _uiSetting.MachineName);
            outDic.Add("ChuckTemprature", _uiSetting.ChuckTemprature);

            outDic.Add("OperatorName", _uiSetting.OperatorName);
            outDic.Add("ProductName", _uiSetting.ProductName);
            outDic.Add("LotNumber", _uiSetting.LotNumber);
            outDic.Add("WaferID", _uiSetting.WaferNumber);
            outDic.Add("Barcode", _uiSetting.Barcode);
            outDic.Add("KeyNumber", _uiSetting.KeyNumber);
            outDic.Add("AuthorityLevel", _uiSetting.AuthorityLevel.ToString());

            outDic.Add("UserIDNumber", _uiSetting.UserIDNumber);
            outDic.Add("EUserID", _uiSetting.UserID.ToString());

            outDic.Add("FormatName", _uiSetting.FormatName);

            outDic.Add("ProberRecipeName", _uiSetting.ProberRecipeName);
            outDic.Add("ProbeMachineName", _uiSetting.ProbeMachineName);
            outDic.Add("IsShowReportCommentsUI", _uiSetting.IsShowReportCommentsUI);

            outDic.Add("TestTimes", _uiSetting.TestTimes);
            outDic.Add("EdgeSensorName", _uiSetting.EdgeSensorName);

            string tStr = _uiSetting.WaferBeginTime.ToString("yyyy/MM/dd HH:mm:ss.fff");
            outDic.Add("WaferBeginTime", tStr);
            outDic.Add("IsAppend", _uiSetting.IsAppend);
            outDic.Add("IsAppendForWaferBegine", _uiSetting.IsAppendForWaferBegine);
            outDic.Add("FileInProcessList", _uiSetting.FileInProcessList);
            outDic.Add("Remark", _uiSetting.Remark);

            outDic.Add("UserDefinedData", GetUserDefindData());
            outDic.Add("PrefixStr", _uiSetting.PrefixStr);
            outDic.Add("ConditionKeyNames", _uiSetting.ConditionKeyNames);
            return outDic;
        }


        #region
        private string GetFullFileName(PathInfo pInfo, string extension = "")
        {
            if (extension == "")
            {
                extension = this._uiSetting.TestResultFileExt;
            }

            string srcPath01 = this._uiSetting.TestResultPath01;

            string tarFolder = _uiSetting.GetPathWithFolder(pInfo);
            string fileName = TestResultFileNameWithoutExt();
            string tarFileName = Path.Combine(tarFolder, fileName + "." + extension);
            return tarFileName;
        }

        private string TestResultFileNameWithoutExt()
        {
            char[] invalidFileChars = Path.GetInvalidFileNameChars();

            foreach (var chr in invalidFileChars)
            {
                if (this._uiSetting.TestResultFileName.Contains(chr))
                {
                    this._uiSetting.TestResultFileName = this._uiSetting.TestResultFileName.Replace(chr.ToString(), "");
                }
            }

            if (this._uiSetting.TestResultFileName == "")
            {
                return DateTime.Now.ToString("yyMMddhhmmss");
            }

            return this._uiSetting.TestResultFileName;
        }

        private Dictionary<string, object> GetPathDic()
        {
            Dictionary<string, object> namePathDic = new Dictionary<string, object>();

            
            namePathDic.Add("OutPathInfo01", new LocalPathInfo(GetFullFileName(_uiSetting.OutPathInfo01), _uiSetting.OutPathInfo01));
            if (_uiSetting.OutPathInfo02.EnablePath)
                namePathDic.Add("OutPathInfo02", new LocalPathInfo(GetFullFileName(_uiSetting.OutPathInfo02), _uiSetting.OutPathInfo02));
            if (_uiSetting.OutPathInfo02.EnablePath)
                namePathDic.Add("OutPathInfo03", new LocalPathInfo(GetFullFileName(_uiSetting.OutPathInfo03), _uiSetting.OutPathInfo03));
            if (_uiSetting.MapPathInfo.EnablePath)
                namePathDic.Add("MapPathInfo", new LocalPathInfo(GetFullFileName(_uiSetting.MapPathInfo), _uiSetting.MapPathInfo));


            return namePathDic;
        }

        private Dictionary<string, object> GetUserDefindData()
        {
            Dictionary<string, object> nameInfoDic = new Dictionary<string, object>();
            nameInfoDic.Add("OutputFileNameFormat01", this._uiSetting.UserDefinedData.OutputFileNameFormat01);
            nameInfoDic.Add("OutputFileNameFormat02", this._uiSetting.UserDefinedData.OutputFileNameFormat02);

            nameInfoDic.Add("ResultItemNameDic", GetUserDefindKeyNameInfo());
            nameInfoDic.Add("ResulKeyList", GetResultKeys());
            return nameInfoDic;
        }

        private Dictionary<string, object> GetUserDefindKeyNameInfo()
        {
            Dictionary<string, object> keyInfoDic = new Dictionary<string, object>();
            foreach (string k in GetResultKeys())
            {
                Dictionary<string, object> kObj = new Dictionary<string, object>();
                TestResultData rItem = this._uiSetting.UserDefinedData[k];
                kObj.Add("Name", rItem.Name);
                kObj.Add("Unit", rItem.Unit);
                kObj.Add("format", rItem.Formate);
                keyInfoDic.Add(k, kObj);
            }
            return keyInfoDic;
        }

        private List<string> GetResultKeys()
        {
            List<string> keyList = new List<string>();
            foreach (var item in this._uiSetting.UserDefinedData.ResultItemNameDic)
            {
                keyList.Add(item.Key);
            }
            return keyList;
        }
        #endregion

        //因為會受到PathInfoConverter影響，無法完整呈現PathInfo資料，因此在這邊轉一手
        internal class LocalPathInfo
        {
            #region
            public string PathName { set; get; }
            public string FullPath { get; set; }
            public bool EnablePath { set; get; }
            public string TestResultPath { set; get; }
            [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
            public ETesterResultCreatFolderType FolderType { set; get; }
            public string FileExt { set; get; }


            #endregion

            #region
            public LocalPathInfo(string FileName, PathInfo pInfo)
            {

                FullPath = FileName;
                EnablePath = pInfo.EnablePath;
                TestResultPath = pInfo.TestResultPath;
                FolderType = pInfo.FolderType;
                FileExt = pInfo.FileExt;
                PathName = pInfo.PathName;
            }

            #endregion

        }

    }
}
