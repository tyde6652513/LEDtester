using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Data
{
    public class DeviceRunTimeDataSet: BaseDataSet<DeviceRunTimeData>
    {
        #region >>> Constructor / Disposor <<<

        public DeviceRunTimeDataSet()
            : base()
        {

        }
        #endregion
    }

    public class DeviceRunTimeData: BaseData
    {

        public DeviceRunTimeData(string key,string MsrtKey, int length)
        {
            this._lockObj = new object();

            this._name = key;

            this._keyName = key;

            this._isEnable = true;

            this._type = "Base";

            this._chennel = 0;

            this._baseResultItemDataList = new List<BaseResultData>();

            BaseResultData bResult = CreateTempBRD(MsrtKey, MsrtKey, length);

            _baseResultItemDataList.Add(bResult);
        }

        private static BaseResultData CreateTempBRD(string key, string name, int length)
        {
            TestResultData item = new TestResultData();

            item.Name = name;

            item.KeyName = key;

            item.IsEnable = true;

            item.Formate = "0.######";

            item.Unit = "dBm";

            BaseResultData bResult = new BaseResultData(item, length);
            return bResult;
        }

        public override object Clone()
        {
            DeviceRunTimeData cloneObj = this.MemberwiseClone() as DeviceRunTimeData;

            cloneObj._baseResultItemDataList = new List<BaseResultData>();

            foreach (var item in this._baseResultItemDataList)
            {
                cloneObj._baseResultItemDataList.Add(item.Clone() as BaseResultData);
            }

            return cloneObj;
        }
        
    }
}
