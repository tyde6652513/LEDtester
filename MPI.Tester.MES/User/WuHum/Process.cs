using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using MPI.Tester.Data;
using MPI.Tester.MES.Data;

namespace MPI.Tester.MES.User.WuHum
{
    class MESProcess : ProcessBase
    {
        private const string COND_FILE_NANE = "HCTestCondition.CSV";
        private const string RESORTING_FILE_NANE = "HCResortingSpec.CSV";

		private ProductData _currentProduct;

		private string[] _standRecipeStr;

        private UserData _userData;

        public MESProcess()
            : base()
        {

        }

        protected override EErrorCode OpenFileAndParse(UISetting uiSetting, MachineConfig machineConfig)
        {
			Console.WriteLine("[WuHum Process], OpenFileAndParse()");

            EErrorCode rtn = EErrorCode.NONE;

			if (uiSetting.WaferNumber.Length < 5)
			{
				Console.WriteLine("[WuHum Process],Barcode Length < 5");

				return EErrorCode.MES_BarcodeError;
			}

            string serverConditonFullPath = Path.Combine(uiSetting.MESPath, COND_FILE_NANE);
            string loaclConditonFullPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, COND_FILE_NANE);

			string serverResortFullPath = Path.Combine(uiSetting.MESPath, RESORTING_FILE_NANE);
			string loaclResortFullPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, RESORTING_FILE_NANE);

			//Copy File
			if(!MPIFile.CopyFile(serverConditonFullPath, loaclConditonFullPath) ||
				!MPIFile.CopyFile(serverResortFullPath, loaclResortFullPath))
			{
				Console.WriteLine("[WuHum Process]," + serverConditonFullPath + " or " + serverResortFullPath + "is not exist");

				return EErrorCode.MES_ReferenceDataNotExist;
			}

            //Ex: S7G7430687602 -> Get "S7"
			List<string[]> condFileName = Tool.ToolBox.ReadCSV(loaclConditonFullPath);

			if (condFileName == null)
            {
				Console.WriteLine("[WuHum Process],condFileName:" + condFileName + " is null");

                return EErrorCode.MES_OpenFileError;
            }

			//Ex: S7G7430687602 -> Get "G74"
			List<string[]> resortFileName = Tool.ToolBox.ReadCSV(loaclResortFullPath);

			if (resortFileName == null)
			{
				Console.WriteLine("[WuHum Process],sortFileName:" + resortFileName + " is null");

				return EErrorCode.MES_OpenFileError;
			}

			//Get Cond key
			string condKey = uiSetting.WaferNumber.Remove(2);

			foreach (string[] key in condFileName)
            {
				if (key[0] == "TestCondID" || condKey.Length != key[0].Length)
                    continue;

				if (condKey == key[0])
                {
					Console.WriteLine("[WuHum Process],condKey:" + condKey);
					Console.WriteLine("[WuHum Process],Recipe Name:" + key[1]);
                    this._testerRecipeFileName = key[1];
				    this._standRecipeStr=key;
                    break;
                }
            }

            if (this._testerRecipeFileName == String.Empty)
            {
				Console.WriteLine("[WuHum Process],MES_NotMatchRecipe");
                return EErrorCode.MES_NotMatchRecipe;
            }

			//Get Resort key
			string remark3 = string.Empty;
			string resortKey = uiSetting.WaferNumber.Substring(2, 3);

			foreach (string[] key in resortFileName)
			{
				if (key[0] == "ResortingID" || resortKey.Length != key[0].Length)
					continue;

				if (resortKey == key[0])
				{
					Console.WriteLine("[WuHum Process],resortKey:" + resortKey);
					Console.WriteLine("[WuHum Process],Remark3:" + key[1]);
					remark3 = key[1];
					break;
				}
			}

            //if (remark3 == String.Empty)
            //{
            //    this._describe.AppendLine("HCResortingSpec 檔案找不到相對應的Bin表");

            //    Console.WriteLine("[WuHum Process],Remark3:" + remark3);
            //    return EErrorCode.MES_NotMatchRecipe;
            //}

            string serverStandardRecipeFullPath = Path.Combine(uiSetting.StandRecipePath, this._testerRecipeFileName + ".csv");

            string loaclStandardRecipeFullPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, this._testerRecipeFileName + ".csv");


            if (!File.Exists(Path.Combine(uiSetting.ProductPath, this._testerRecipeFileName + "." + Constants.Files.TASK_SHEET_EXTENSION)))
            {
					Console.WriteLine("[WuHum Process],MES_TargetRecipeNoExist");
                    return EErrorCode.MES_TargetRecipeNoExist;
            }
			else
			{
                if (uiSetting.IsCheckStandardRecipe)
                {
                    if (!MPIFile.CopyFile(serverStandardRecipeFullPath, loaclStandardRecipeFullPath))
                    {
                        Console.WriteLine("[WuHum Process],Copy Stand Recipe Error");
                        this._describe.AppendLine("標準檔案的條件檔不存在 : " + serverStandardRecipeFullPath);
                        return EErrorCode.MES_NotMatchRecipe;
                    }

                    if (!CheckIsMatchStandardRecipe(loaclStandardRecipeFullPath,this._testerRecipeFileName))
                    {
                        Console.WriteLine("[WuHum Process],MES_Not Math Standard Recipe");
                        return EErrorCode.MES_NotMatchRecipe;
                    }
                }
			}

            switch (rtn)
            {
                case EErrorCode.NONE:
                    uiSetting.TaskSheetFileName = this._testerRecipeFileName;
					uiSetting.WeiminUIData.Remark03 = remark3;
                    break;
                default:
                    break;
            }

            return rtn;
        }

        protected override EErrorCode ConverterToMPIFormat()
        {
            return EErrorCode.NONE;
        }

        protected override EErrorCode SaveRecipeToFile()
        {
            return EErrorCode.NONE;
        }


		  private ProductData DeserializeProduct(string recipeFileAndPath)
		  {
			  ProductData pd = null;

			  if (!System.IO.File.Exists(recipeFileAndPath))
				  return pd;

			  try
			  {
				  pd = MPI.Xml.XmlFileSerializer.Deserialize(typeof(ProductData), recipeFileAndPath) as ProductData;

				  if (pd == null)
				  {
					  return pd;
				  }

				  return pd;
			  }
			  catch
			  {
				  return pd;
			  }
		  }

		  private bool CheckIsMatchStandardRecipe(string fullStdPath,string recipe)
		  {
              List<string[]> data = CSVUtil.ReadCSV(fullStdPath);

             //if(this._standRecipeStr==null)
             // {
             //    return false;
             // }

              Dictionary<string, double[]> stdRecipe = new Dictionary<string, double[]>();

              Dictionary<string, TestResultData> stdResultItemSpc = new Dictionary<string, TestResultData>();
            
              data.RemoveAt(0);

              data.RemoveAt(data.Count-1);

              foreach(string[] array in data)
              {
                  if (array.Length < 7)
                  {
                      break;
                  }

                  string keyName=string.Empty;

                  string resultKeyName = string.Empty;

                  TestItemData tid;


                  switch(array[1])
                  {
                      case "ESD1":
                          keyName="ESD_1";
                          break;
                      case "IR1":
                           keyName="VR_1";
                           resultKeyName = "MIR_1";
                         break;
                     case "VZ1":
                           keyName="IZ_1";
                           resultKeyName = "MVZ_1";
                            break;
                      case "VF2":
                           keyName="IF1_1";
                           resultKeyName = "MVF_1";
                         break;
                     case "VF1":
                          keyName="LOPWL_1";
                          resultKeyName = "MVFLA_1";
                         break;
                     case "VF3":
                           keyName="LOPWL_2";
                           resultKeyName = "MVFLA_2";
                          break;
                      case "VF4":
                           keyName="LOPWL_3";
                           resultKeyName = "MVFLA_3";
                         break;

                      default:
                         break;
                  }


                  if (keyName != string.Empty)
                  {
                      
                      double[] content = new double[5];

                      content[0] = double.Parse(array[2]);
                      content[1] = double.Parse(array[3]);
                      content[2] = double.Parse(array[4]);
                      content[3] = double.Parse(array[5]);
                      content[4] = double.Parse(array[6]);

                      stdRecipe.Add(keyName, content);
                  }

                  if (resultKeyName != string.Empty)
                  {
                      TestResultData trd = new TestResultData();

                      trd.KeyName = keyName;

                      double[] content = new double[5];

                      double value = 0.0d;

                      double.TryParse(array[6], out value);

                      trd.MaxLimitValue = value;

                      value = 0.0d;

                      double.TryParse(array[5], out value);

                      trd.MinLimitValue = value;

                      if (trd.MaxLimitValue == 0.0d)
                      {
                          trd.IsVerify = false;
                      }
                      else
                      {
                          trd.IsVerify = true;
                      }

                      stdResultItemSpc.Add(resultKeyName, trd);
                  }
              }

              foreach (string[] array in data)
              {

                  if (array.Length < 7)
                  {
                      break;
                  }

                  string keyName = string.Empty;

                  switch (array[1])
                  {
                      case "LOP1":
                          if (array[7] == "MW")
                          {
                              keyName = "WATT_1";
                          }
                          else
                          {
                              keyName = "LOP_1";
                          }
                          break;

                      case "WLP1":
                          keyName = "WLP_1";
                          break;
                      case "WLD1":
                          keyName = "WLD_1";
                          break;
                      case "HW1":
                          keyName = "HW_1";
                          break;
                  }

                  if (keyName != string.Empty)
                  {
                      TestResultData trd = new TestResultData();

                      trd.KeyName = keyName;

                      double[] content = new double[5];

                      double value = 0.0d;

                      double.TryParse(array[6], out value);

                      trd.MaxLimitValue = value;

                      value = 0.0d;

                      double.TryParse(array[5], out value);

                      trd.MinLimitValue = value;

                      if (trd.MaxLimitValue == 0.0d)
                      {
                          trd.IsVerify = false;
                      }
                      else
                      {
                          trd.IsVerify = true;
                      }

                      stdResultItemSpc.Add(keyName, trd);
                  }
              }


			  recipe = recipe + "." + Constants.Files.PRODUCT_FILE_EXTENSION;

			  string recipeFullPath = Path.Combine(Constants.Paths.PRODUCT_FILE, recipe);

              if (File.Exists(recipeFullPath))
              {
                  ProductData pd = DeserializeProduct(recipeFullPath);

                  double[] applyData = null;

                  foreach (TestItemData item in pd.TestCondition.TestItemArray)
                  {
                      switch (item.KeyName)             
                      {
                          case "ESD_1":

                              if (stdRecipe.ContainsKey("ESD_1"))
                              {
                                  applyData = stdRecipe["ESD_1"];

                                  double EsdVolt = (item as ESDTestItem).EsdSetting.ZapVoltage;

                                  bool isStdEnable = false;

                                  if (applyData[0] != 0.0d)
                                  {
                                      isStdEnable = true;
                                  }

                                  if (isStdEnable)
                                  {
                                      if (-EsdVolt != applyData[0])
                                      {
                                          this._describe.AppendLine("機台端ESD設定電壓和標準檔案不一致");
                                          return false;
                                      }

                                      if (isStdEnable != item.IsEnable)
                                      {
                                          this._describe.AppendLine("標準Recipe啟動ESD量測，請確認機台端是否開啟");
                                          return false;
                                      }

                                  }
                                  else
                                  {
                                      if (isStdEnable != item.IsEnable)
                                      {
                                          this._describe.AppendLine("標準Recipe未啟動ESD量測，請確認機台端是否關閉");
                                          return false;
                                      }
                                  }

                              }

                              break;
                          //=============================================

                          case "IF1_1":
                          case "LOPWL_1":
                          case "LOPWL_2":
                          case "LOPWL_3":

                              if (stdRecipe.ContainsKey(item.KeyName))
                              {
                                  double forceValue = item.ElecSetting[0].ForceValue;

                                  double forceTime = item.ElecSetting[0].ForceTime;

                                  double compliance = item.ElecSetting[0].MsrtProtection;

                                  applyData = stdRecipe[item.KeyName];
                                  
                                  // Bias,Time,Compliance

                                  if (forceValue == applyData[0])
                                  {

                                  }
                                  else
                                  {
                                      this._describe.AppendLine(item.Name + " : 機台端設定值和標準檔案不一致");
                                      return false;
                                  }

                                  if ( forceTime == applyData[1])
                                  {

                                  }
                                  else
                                  {
                                      this._describe.AppendLine(item.Name + " : 機台端輸出時間(Force Time)和標準檔案不一致");
                                      return false;
                                  }

                              }

                              break;
                          //=============================================
                          case "VR_1":
                          case "IZ_1":

                              if (stdRecipe.ContainsKey(item.KeyName))
                              {
                                  double forceValue = (item.ElecSetting[0].ForceValue * -1);

                                  double forceTime = item.ElecSetting[0].ForceTime;

                                  double compliance = item.ElecSetting[0].MsrtProtection;

                                  applyData = stdRecipe[item.KeyName];

                                  // Bias,Time,Compliance

                                  if (forceValue != applyData[0])                             
                                  {
                                      this._describe.AppendLine(item.Name + " : 機台端設定值和標準檔案不一致");
                                      this._describe.AppendLine("標準檔案 : "+ applyData[0].ToString() + item.ElecSetting[0].ForceUnit);
                                      return false;
                                  }

                                  if (forceTime != applyData[1])
                                  {
                                      this._describe.AppendLine(item.Name + " : 機台端設定時間和標準檔案不一致");
                                      this._describe.AppendLine("標準檔案 : " + applyData[1].ToString()+" ms");
                                      return false;
                                  }
             

                                  if ( compliance != applyData[2])
                                  {
                                      this._describe.AppendLine(item.Name + " : 機台端Compliance和標準檔案不一致");
                                      this._describe.AppendLine("標準檔案 : " + applyData[2].ToString()+ item.ElecSetting[0].MsrtUnit);
                                      return false;
                                  }

                              }
                              break;
                          //=============================================
                      }

                      if(item.MsrtResult!=null)
                      {
                          foreach (TestResultData trd in item.MsrtResult)
                          {
                              if (stdResultItemSpc.ContainsKey(trd.KeyName))
                              {
                                  if (trd.IsVerify != stdResultItemSpc[trd.KeyName].IsVerify  ||                    
                                      trd.MinLimitValue != stdResultItemSpc[trd.KeyName].MinLimitValue ||
                                      trd.MaxLimitValue != stdResultItemSpc[trd.KeyName].MaxLimitValue
                                      )
                                  {
                                      this._describe.AppendLine(trd.Name + " : 機台端上下限值和標準檔案不一致");
                                      return false;
                                  }
                              }
                          }
                      }
                  }
              }

             return true;
		  }


		  private enum ERecipeItem : int
		  {
		    	IF1=4,
				IF1T=5,
				IF2=6,
				IF2T=7,
				IF3=8,	
				IF3T=9,
				IZ1=19,
				IZ1T=20,
				VR1 = 23,
				VR1T = 24,
				IFWL1 =31,	
				IFWL1T=32,
				ESDVoltage=53,
				ESDInterval=55,
		  }
    }
}
