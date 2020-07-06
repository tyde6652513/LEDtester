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
    public class ProductDataInfoConverter
    {
        ProductData _productData = new ProductData();
       public ProductDataInfoConverter(ProductData pData)
       {
           _productData = pData;
       }

       public Dictionary<string, object> GetInfoDic()
       {
           Dictionary<string, object> outDic = new Dictionary<string, object>();


           outDic.Add("ProductName", _productData.ProductName);
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
