using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;
using Newtonsoft.Json;
using MPI.Tester.Data.ChannelCoordTable;

namespace MPI.Tester.Report.BaseMethod.InfoToJSON
{
    public class ProductDataInfoConverter
    {
        ProductData _productData = new ProductData();
        ChannelPosShiftTable<int> _chTable = new ChannelPosShiftTable<int>();
        public ProductDataInfoConverter(ProductData pData, ChannelPosShiftTable<int> chTable)
        {
            _productData = pData;
            _chTable = chTable;

        }

       public Dictionary<string, object> GetInfoDic()
       {
           Dictionary<string, object> outDic = new Dictionary<string, object>();


           outDic.Add("ProductName", _productData.ProductName);
           outDic.Add("ChannelLayerPosShiftTable", _chTable);
           if (_productData.TestCondition != null && _productData.TestCondition.TestItemArray != null)
           {
               outDic.Add("TestCondition", _productData.TestCondition.GetItemInfoList());
           }
           return outDic;
       }

       public string GetStringAsJSONValue()
       {
           string outStr = "";

           Dictionary<string, object> outDic = GetInfoDic();
           
           // UISetting.GetPathWithFolder(pInfo);

           outStr = JsonConvert.SerializeObject(outDic, Formatting.Indented);

           return outStr;
 
       }


        #region
       private List<object> GetTestCondition()
       {
           List<object> itemList = new List<object>();


           if (_productData.TestCondition != null && _productData.TestCondition.TestItemArray != null)
           { }


           return itemList;
       }
        #endregion

    }
}
