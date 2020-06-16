using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;

namespace MPI.Tester.Data
{
    public class LCRData : BaseData
    {
        public LCRData()
        {
            this._lockObj = new object();

            this._name = string.Empty;

            this._keyName = string.Empty;

            this._isEnable = false;

            this._type = "Base";

            this._chennel = 0;

            this._baseResultItemDataList = new List<BaseResultData>();
        }

        public LCRData(TestItemData item, int sweepLength, uint ch = 0)//考量到 TestItemData並無原生的變數紀錄掃描點數，因此強迫使用者寫入
            : this()
        {

            this._name = item.Name;

            this._keyName = item.KeyName;

            this._isEnable = item.IsEnable;

            this._type = item.Type.ToString();

            this._chennel = ch;

            foreach (var resultItem in item.MsrtResult)
            {

                BaseResultData data = new BaseResultData(resultItem, sweepLength);

                data.IsEnable = resultItem.IsEnable;

                this._baseResultItemDataList.Add(data);
            }
        }

        public LCRData(LCRSweepTestItem lcritem, uint ch = 0)//考量到 TestItemData並無原生的變數紀錄掃描點數，因此強迫使用者寫入
            : this()
        {

            this._name = lcritem.Name;

            this._keyName = lcritem.KeyName;

            this._isEnable = lcritem.IsEnable;

            this._type = lcritem.Type.ToString();

            this._chennel = ch;

            foreach (var resultItem in lcritem.MsrtResult)
            {
                BaseResultData data = new BaseResultData(resultItem, lcritem.LCRSetting.Point);

                data.IsEnable = resultItem.IsEnable;

                this._baseResultItemDataList.Add(data);
            }
        }

        public override object Clone()
        {
            LCRData cloneObj = new LCRData();

            cloneObj._name = this._name;

            cloneObj._keyName = this._keyName;

            cloneObj._isEnable = this._isEnable;

            cloneObj._type = this._type;

            foreach (var item in this._baseResultItemDataList)
            {
                cloneObj._baseResultItemDataList.Add(item.Clone() as BaseResultData);
            }

            return cloneObj;
        }
    }



}
