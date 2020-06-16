using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

using MPI.Tester.Data;
using MPI.Tester.TestKernel;
using MPI.Tester.DeviceCommon;
using MPI.Tester.Tools;
using MPI.Tester.MES;

namespace MPI.Tester.Gui
{
	public static class DataCenter
	{
		//----------------------------------------------------------------------------------------------------------------
		public static Data.MachineConfig _machineConfig;
		public static Data.UISetting _uiSetting;
		public static Data.TesterSetting _sysSetting;
		public static Data.ProductData _product;
		public static Data.MapData _mapData;
		public static Data.ConditionData _tempCond = new ConditionData();
		public static Data.UserManageCtrl _userManag = new UserManageCtrl(Constants.Paths.DATA_FILE, Constants.Files.USER_MANAGE_FILE);
        public static Data.RDFunc _rdFunc = new RDFunc();
        public static Data.SystemCali _sysCali = new SystemCali();
		//----------------------------------------------------------------------------------------------------------------
		public static Data.ConditionCtrl _conditionCtrl = new ConditionCtrl();
		public static Data.SmartBinning _smartBinning = new SmartBinning();
		//----------------------------------------------------------------------------------------------------------------

		//public static DeviceCommon.DeviceConfig _deviceConfig;
		//public static DeviceCommon.DeviceSpec _deviceSpec;
		public static bool _bStandAlone = false;
		//----------------------------------------------------------------------------------------------------------------
		public static AcquireData _acquireData;
		public static SystemStatus _sysStatus;
		public static MachineInfoData _machineInfo;

        public static Tools.FilesCompare _fileCompare = new FilesCompare();

        public static Tools.SysConfig _toolConfig = new SysConfig();
        public static MESProcess _mesProcess = new MESProcess();

		#region >>> Private Method <<<

		private static void GetTheMachineUserList()
		{
			DirectoryInfo di = new DirectoryInfo(Constants.Paths.USER_DIR);

            if (di.Exists)
            {
                FileInfo[] fileInfos = di.GetFiles("User*");

                string[] strS = new string[fileInfos.Length];
                for (int i = 0; i < strS.Length; i++)
                {
                    strS[i] = fileInfos[i].Name.Remove(fileInfos[i].Name.IndexOf('.'));
                    strS[i] = strS[i].Replace("User", "");
                }

                _uiSetting.UserIDList = strS;
            }
            else
            {
                _uiSetting.UserIDList = new string[1] { "0000" };	// default UserData = null
				Host.SetErrorCode(EErrorCode.NoUserDataInFolder);
                Host.SetErrorCode(EErrorCode.NoUserDataInFolder);
            }
		}		

		private static bool CheckRootPath(string path)
		{
			if ((path.Length >= Constants.Paths.ROOT.Length) &&
			(path.Substring(0, Constants.Paths.ROOT.Length) == Constants.Paths.ROOT))
			{
				return false;
			}

			return true;
		}

        private static void ResetProductMsrtResults()
        {
            if (_product == null || _product.TestCondition == null)
            {
                return;
            }

            if (_product.TestCondition.TestItemArray == null)
            {
                return;
            }

            int pivIndex = 1;  // keyName base 1

            foreach (var item in _product.TestCondition.TestItemArray)
            {
                if (item is PIVTestItem)
                {
                    TestResultData[] cloneResultObj = new TestResultData[item.MsrtResult.Length];
                    GainOffsetData[] cloneCoefObj = new GainOffsetData[item.GainOffsetSetting.Length];

                    for (int i = 0; i < cloneResultObj.Length; i++)
                    {
                        cloneResultObj[i] = item.MsrtResult[i].Clone() as TestResultData;
                    }

                    for (int i = 0; i < cloneCoefObj.Length; i++)
                    {
                        cloneCoefObj[i] = item.GainOffsetSetting[i].Clone() as GainOffsetData;
                    }


                    (item as PIVTestItem).ResetMsrtItems(pivIndex);

                    for (int i = 0; i < item.MsrtResult.Length; i++)
                    {
                        foreach (var oldObj in cloneResultObj)
                        {
                            if (item.MsrtResult[i].KeyName == oldObj.KeyName)
                            {
                                item.MsrtResult[i] = oldObj.Clone() as TestResultData;

                                break;
                            }
                        }
                    }

                    for (int i = 0; i < item.GainOffsetSetting.Length; i++)
                    {
                        foreach (var oldObj in cloneCoefObj)
                        {
                            if (item.GainOffsetSetting[i].KeyName == oldObj.KeyName)
                            {
                                item.GainOffsetSetting[i] = oldObj.Clone() as GainOffsetData;

                                break;
                            }
                        }
                    }

                    pivIndex++;
                }
            }
        }

		#endregion

		#region >>> Public Methods <<<

		public static bool Open()
		{
			Console.WriteLine("[DataCenter], Open()");

			string pathAndFile = string.Empty;

            //-----------------------------------------------------------------------------------------------------------------------
			// (1) Load machine configuration file
			pathAndFile = Path.Combine(Constants.Paths.DATA_FILE,Constants.Files.MACHINE_DATA);
			_machineConfig = MPI.Xml.XmlFileSerializer.Deserialize( typeof( MachineConfig ), pathAndFile ) as MachineConfig;

			if ( _machineConfig == null )
			{
				_machineConfig = new MachineConfig();
			}

#if (!DebugVer)
            _machineConfig.Enable.IsSimulator = false;
#endif

			pathAndFile = Path.Combine(Constants.Paths.DATA_FILE, Constants.Files.DEVICE_SPEC);

            //_deviceSpec = MPI.Xml.XmlFileSerializer.Deserialize(typeof(DeviceSpec), pathAndFile) as DeviceSpec;
            //if ( _deviceSpec == null )
            //{
            //    _deviceSpec = new DeviceSpec();
            //}

            //-----------------------------------------------------------------------------------------------------------------------
			// (3) Load system parameters setting
			if (LoadSysParam() == false)
			{
				Host.SetErrorCode(EErrorCode.LoadTesterSettingFail);
				_sysSetting = new TesterSetting();
			}

            LoadSysCail();

            //-----------------------------------------------------------------------------------------------------------------------
			// (4) Load UI setting data 
			if (LoadUISetting() == false)
			{
				Host.SetErrorCode(EErrorCode.LoadUISettingFail);
				_uiSetting = new UISetting();
			}

			GetTheMachineUserList();

            //-----------------------------------------------------------------------------------------------------------------------
			// (5) Load user data 
			if (LoadUserData(_uiSetting.UserID) == false)
			{
				Host.SetErrorCode(EErrorCode.LoadUserDataFail);
			}

            LoadRDFuncParam();  // 20150609, 需先取得UserID

            _sysSetting.SpecCtrl.SetSupportedItems(_machineConfig, _rdFunc.RDFuncData.SpecDataDefinition, _rdFunc.RDFuncData.TesterConfigType);

            //-----------------------------------------------------------------------------------------------------------------------
			// (6) Load "TaskSheet" file , and use the xml data to load "Product File" and "Bin Data File"
			LoadTaskSheet( _uiSetting.TaskSheetFileName );

			WMOperate.WM_ReadCalibrateParamFromSetting();

            //-----------------------------------------------------------------------------------------------------------------------
			// (7) Import Bin Data and Import Calibrate Data
            //if (DataCenter._uiSetting.ImportCalibrateFileName == "")
            //{
            //    ImportCalibrateData(DataCenter._uiSetting.ProductPath02, DataCenter._uiSetting.ImportCalibrateFileName + ".cal");
            //}

            //if (DataCenter._uiSetting.ImportBinFileName == "")
            //{
            //    ImportBinTable	(DataCenter._uiSetting.ProductPath02, DataCenter._uiSetting.ImportBinFileName + ".sr2");
            //}

            //-----------------------------------------------------------------------------------------------------------------------
            // (7) Load Tools Config Data
            LoadToolsSysConfig();

            //-----------------------------------------------------------------------------------------------------------------------            
			CompareTestItemAndBinItem();

			_userManag.OpenFile();

			_userManag.Login("gpt", "gpt");

			EAuthority ee = _userManag.CurrentAuthority;
			string name = _userManag.CurrentUserName;

			return true;
		}

		public static bool ChangeItemNameFromUserDefine()
		{
			if (_uiSetting.UserDefinedData.FormatNames != null)
			{
				// (1) Reset Definded Name by FormatName
				bool isHasFormat = false;
				foreach (string str in _uiSetting.UserDefinedData.FormatNames)
				{
					if (str == _uiSetting.FormatName)
					{
						isHasFormat = true;
						if (_uiSetting.UserDefinedData.ResetUserDefinedName( _uiSetting.FormatName) == false)
						{
							Host.SetErrorCode(EErrorCode.ResetUserDataFail);
							return false;
						}
						break;
					}
				}

				if (isHasFormat == false)
				{
					Host.SetErrorCode(EErrorCode.NoMatchUserFormat);
				}

				// (2) Set to CondtionCtrl and update those names
				_conditionCtrl.SetUserDefinedData( _uiSetting.UserDefinedData );

				return true;
			}
			else
			{
				return false;
			}
		}

		public static void CompareTestItemAndBinItem()
		{
			if ( _conditionCtrl.MsrtResultArray == null )
				return;

			List<string> msrtItemList = new List<string>();

			msrtItemList.Clear();

			foreach ( TestResultData data in _conditionCtrl.MsrtResultArray )
			{
				if ( data.IsEnable == true )
				{
					msrtItemList.Add( data.KeyName );
				}
			}

			_smartBinning.ResetBinData(_product, _uiSetting.UserDefinedData.BinItemNameDic);
		}

		public static bool LoadSysParam()
		{

			string pathAndFile = Path.Combine(Constants.Paths.DATA_FILE, Constants.Files.TESTER_SETTING);

			if (!System.IO.File.Exists(pathAndFile))
				return false;

			try
			{
				_sysSetting = MPI.Xml.XmlFileSerializer.Deserialize(typeof(TesterSetting), pathAndFile) as TesterSetting;
				//_sysSetting = MPI.TRY.XmlFileSerializer.Deserialize(typeof(TesterSetting), pathAndFile) as TesterSetting;

				if (_sysSetting == null)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
			catch
			{
				return false;
			}
		}

        public static bool LoadRDFuncParam()
        {
            string userID = ((int)_uiSetting.UserID).ToString("0000");

            string rdFileNameWithExtension = string.Format("{0}{1}.{2}", Constants.Files.RDFUNC_FILENAME, userID, Constants.Files.RDFUNC_FILE_EXTENSION);

            string pathAndFile = Path.Combine(Constants.Paths.DATA_FILE, rdFileNameWithExtension);

            _rdFunc.RDFuncData.UserID = userID;  // 這時的RDFuncData為 new 出來的 default Data

            if (_rdFunc.Open(pathAndFile))
            {
                // 檢查 RdFunc Spectrl 相容性
                _rdFunc.CheckSpecCtrlCompatibility();
                
                return true;
            }
            else
            {
                return false;
            }
        }

		public static bool LoadUISetting()
		{

			string pathAndFile = Path.Combine(Constants.Paths.DATA_FILE, Constants.Files.UI_SETTING);

			if (!System.IO.File.Exists(pathAndFile))
				return false;

			try
			{
				_uiSetting = MPI.Xml.XmlFileSerializer.Deserialize(typeof(UISetting), pathAndFile) as UISetting;

				if (_uiSetting == null)
				{
					return false;
				}
				else
				{
                    _uiSetting.SoftwareVersoin = Host.GetProgramVersion();
					return true;
				}
			}
			catch
			{
				return false;
			}
		}

		public static bool LoadUserData(EUserID user)
		{
			if (_uiSetting.UserDefinedData.LoadUserDefineData(user))
			{
				if (_uiSetting.UserDefinedData.FormatNames == null || _uiSetting.UserDefinedData.FormatNames.Length == 0)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
			else
			{
				return false;
			}
		}

        public static bool CheckTaskSheetIsExist(string fileName)
        {
           // string fileNameWithExt = fileName + "." + Constants.Files.TASK_SHEET_EXTENSION;
            string dir = string.Empty;

            if (DataCenter._uiSetting.IsEnableRunMesSystem && DataCenter._uiSetting.IsConverterTasksheet)
            {
                dir = Constants.Paths.MES_FILE_PATH;
            }
            else
            {
                dir = DataCenter._uiSetting.ProductPath;
            }

            DirectoryInfo sourceDir = new DirectoryInfo(DataCenter._uiSetting.ProductPath);
            FileSystemInfo[] infos = sourceDir.GetFileSystemInfos("*.ts");

            foreach (FileSystemInfo item in infos)
            {
                if (Path.GetFileNameWithoutExtension(item.Name).ToUpper() == fileName.ToUpper())
                {
                    return true;
                }
            }
            return false;

            //string pathAndFile = Path.Combine(dir, fileName + ".ts");

            //if (File.Exists(pathAndFile))
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

		public static bool LoadTaskSheet(string fileName,bool isLoadByProber = false)
		{
			Console.WriteLine("[DataCenter], LoadTaskSheet(), Start");

			XmlDocument xmlDoc = new XmlDocument();
			string oneFileName = null;
			string oneFileNameExt = null;
			string oneFileNamePath = null;
			string oneFullName = null;
            bool rtn = true;

			string fileNameWithExt = fileName + "." + Constants.Files.TASK_SHEET_EXTENSION;
            string pathAndFile = string.Empty;

            if (DataCenter._uiSetting.IsEnableRunMesSystem && DataCenter._uiSetting.IsConverterTasksheet)
            {
                pathAndFile = Path.Combine(Constants.Paths.MES_FILE_PATH, fileNameWithExt);
            }
            else
            {
                pathAndFile = Path.Combine(DataCenter._uiSetting.ProductPath, fileNameWithExt);
            }
           
			XmlElement root = null;

            if (File.Exists(pathAndFile) == false)
            {
                if (!isLoadByProber)
                {
                    Host.SetErrorCode(EErrorCode.LoadTaskSheetFail);
                    CreateTaskSheet(Constants.Files.DEFAULT_FILENAME);
                    CreateProductFile(Constants.Files.DEFAULT_FILENAME);
                    CreateMapDataFile(Constants.Files.DEFAULT_FILENAME);
                    CreateBinDataFile(Constants.Files.DEFAULT_FILENAME);
                }
				return false;
            }

            Console.WriteLine("[DataCenter], LoadTaskSheet(), File Path : " + pathAndFile);

			//---------------------------------------------------------------------------------------
			// (1) Load "TaskSheet" XML File 
			//---------------------------------------------------------------------------------------
			try
			{
				xmlDoc.Load(pathAndFile);
				root = xmlDoc.DocumentElement;
			}
			catch
			{
                if (!isLoadByProber)
                {
                    Host.SetErrorCode(EErrorCode.LoadTaskSheetFail);
                    CreateTaskSheet(Constants.Files.DEFAULT_FILENAME);
                    CreateProductFile(Constants.Files.DEFAULT_FILENAME);
                    CreateMapDataFile(Constants.Files.DEFAULT_FILENAME);
                    CreateBinDataFile(Constants.Files.DEFAULT_FILENAME);
                }
				return false;
			}

			if (root == null)
			{
                if (!isLoadByProber)
                {
                    Host.SetErrorCode(EErrorCode.LoadTaskSheetFail);
                    CreateTaskSheet(Constants.Files.DEFAULT_FILENAME);
                    CreateProductFile(Constants.Files.DEFAULT_FILENAME);
                    CreateMapDataFile(Constants.Files.DEFAULT_FILENAME);
                    CreateBinDataFile(Constants.Files.DEFAULT_FILENAME);
                }
				return false;
			}

			_uiSetting.TaskSheetFileName = fileName;

			//---------------------------------------------------------------------------------------
			// (2) Get  "Product File Name" and Load Product File
			//---------------------------------------------------------------------------------------
			XmlNode node = root.SelectSingleNode("FileInfo[Type='Product']");
			if (node == null 
				|| node.SelectSingleNode("Name") == null 
				|| node.SelectSingleNode("Name").InnerText == ""
				|| node.SelectSingleNode("Ext").InnerText == ""
				|| node.SelectSingleNode("Path").InnerText == "")
			{
				Host.SetErrorCode(EErrorCode.LoadProductFileFail);

				CreateProductFile(Constants.Files.DEFAULT_FILENAME);

                rtn &= false;
			}
			else
			{
				oneFileName = node.SelectSingleNode("Name").InnerText;
				oneFileNameExt = node.SelectSingleNode("Ext").InnerText;
				oneFileNamePath = node.SelectSingleNode("Path").InnerText;

				if (string.IsNullOrEmpty(oneFileNamePath))
				{
					oneFileNamePath = Constants.Paths.PRODUCT_FILE;
				}

				oneFullName = Path.Combine(oneFileNamePath, oneFileName + "." + oneFileNameExt);

				if (LoadProductFile(oneFullName) == false)
				{
					Host.SetErrorCode(EErrorCode.LoadProductFileFail);
					CreateProductFile(Constants.Files.DEFAULT_FILENAME);
                    rtn &= false;
				}
			}

			//---------------------------------------------------------------------------------------
			// (3) Get "Import Calibrated Data File Name" ( GainOffset and Coef. Table )
			//---------------------------------------------------------------------------------------
			node = root.SelectSingleNode("FileInfo[Type='ImportCalibrateData']");
			if (node == null || node.SelectSingleNode("Name") == null || node.SelectSingleNode("Name").InnerText == "")
			{
				DataCenter._uiSetting.ImportCalibrateFileName = "";
			}
			else
			{
				DataCenter._uiSetting.ImportCalibrateFileName = node.SelectSingleNode("Name").InnerText;
			}

            //---------------------------------------------------------------------------------------
            // (4) Get "Import BinTable File Name" 
            //---------------------------------------------------------------------------------------
            node = root.SelectSingleNode("FileInfo[Type='ImportBin']");
            if (node == null || node.SelectSingleNode("Name") == null || node.SelectSingleNode("Name").InnerText == "")
            {
                DataCenter._uiSetting.ImportBinFileName = "";
            }
            else
            {
               DataCenter._uiSetting.ImportBinFileName = node.SelectSingleNode("Name").InnerText;
            }


            if (DataCenter._uiSetting.ImportCalibrateFileName != "")
            {
                DataCenter.ImportCalibrateData(DataCenter._uiSetting.ProductPath02, DataCenter._uiSetting.ImportCalibrateFileName + ".cal");
                rtn &= true;
            }

			//---------------------------------------------------------------------------------------
			// (5) Get "Bin Data File Name" and Load Bin Data File
			//---------------------------------------------------------------------------------------
            node = root.SelectSingleNode("FileInfo[Type='BinData']");
            if (node == null
                || node.SelectSingleNode("Name") == null
                || node.SelectSingleNode("Name").InnerText == ""
                || node.SelectSingleNode("Ext").InnerText == ""
                || node.SelectSingleNode("Path").InnerText == "")
            {
                Host.SetErrorCode(EErrorCode.LoadBinDataFileFail);
                CreateBinDataFile(Constants.Files.DEFAULT_FILENAME);
                rtn &= false;
            }
            else
            {
                oneFileName = node.SelectSingleNode("Name").InnerText;
                oneFileNameExt = node.SelectSingleNode("Ext").InnerText;
                oneFileNamePath = node.SelectSingleNode("Path").InnerText;

                if (string.IsNullOrEmpty(oneFileNamePath))
                {
                    oneFileNamePath = Constants.Paths.PRODUCT_FILE;
                }

                oneFullName = Path.Combine(oneFileNamePath, oneFileName + "." + oneFileNameExt);

                if (LoadBinDataFile(oneFullName) == false)
                {
					Host.SetErrorCode(EErrorCode.LoadBinDataFileFail);

					//Change *.bin format from *.xml to *.dat
                    //CreateBinDataFile(Constants.Files.DEFAULT_FILENAME);
					CreateBinDataFile(Path.GetFileNameWithoutExtension(oneFullName));

                    rtn &= false;
                }
            }

			//---------------------------------------------------------------------------------------
			// (6) Get  "MapData File Name" and Load MapData File
			//---------------------------------------------------------------------------------------
			node = root.SelectSingleNode("FileInfo[Type='MapData']");

			if (node == null
				|| node.SelectSingleNode("Name") == null
				|| node.SelectSingleNode("Name").InnerText == ""
				|| node.SelectSingleNode("Ext").InnerText == ""
				|| node.SelectSingleNode("Path").InnerText == "")
			{

                // 當map recipe 不存在時，Show alarm 再 Create Default Color。

				Host.SetErrorCode(EErrorCode.LoadMapDataFileFail);

				CreateMapDataFile(fileName);

                XmlElement itemElem = xmlDoc.CreateElement("FileInfo");
                root.AppendChild(itemElem);

                XmlElement itemData = xmlDoc.CreateElement("Type");
                itemData.InnerText = "MapData";
                itemElem.AppendChild(itemData);

                itemData = xmlDoc.CreateElement("Name");

                itemData.InnerText = fileName;
                itemElem.AppendChild(itemData);

                itemData = xmlDoc.CreateElement("Ext");
                itemData.InnerText = Constants.Files.MAPDATA_FILE_EXTENSION;
                itemElem.AppendChild(itemData);

                itemData = xmlDoc.CreateElement("Path");
                itemData.InnerText = DataCenter._uiSetting.ProductPath;
                itemElem.AppendChild(itemData);

                xmlDoc.Save(pathAndFile);

				Console.WriteLine("[DataCenter], LoadTaskSheet(), Map node == null");

				rtn &= false;
			}
			else
			{
				oneFileName = node.SelectSingleNode("Name").InnerText;

				oneFileNameExt = node.SelectSingleNode("Ext").InnerText;

				oneFileNamePath = node.SelectSingleNode("Path").InnerText;

				if (string.IsNullOrEmpty(oneFileNamePath))
				{
					oneFileNamePath = Constants.Paths.PRODUCT_FILE;

					Console.WriteLine("[DataCenter], LoadTaskSheet(), Map oneFileNamePath IsNullOrEmpty");
				}

				oneFullName = Path.Combine(oneFileNamePath, oneFileName + "." + oneFileNameExt);

				if (LoadMapDataFile(oneFullName) == false)
				{
					Host.SetErrorCode(EErrorCode.LoadMapDataFileFail);

					CreateMapDataFile(fileName);

					Console.WriteLine("[DataCenter], LoadTaskSheet(), Map File Path : " + fileName);

					rtn &= false;
				}
			}

            DataCenter.loadMapGradeColor();

            return rtn;
		}

		public static bool LoadProductFile(string fullFileName)
		{
			Console.WriteLine("[DataCenter], LoadProductFile(), Start");

			if (!System.IO.File.Exists(fullFileName))
			{
				return false;
			}

			Console.WriteLine("[DataCenter], LoadProductFile(), File Path : " + fullFileName);

			try
			{
				_product = MPI.Xml.XmlFileSerializer.Deserialize(typeof(ProductData), fullFileName) as ProductData;

				if (_product == null)
				{
					return false;
				}

                //_product.TestCondition
			}
			catch
			{
				return false;
			}

			_uiSetting.ProductFileName = Path.GetFileNameWithoutExtension(fullFileName);

            ResetProductMsrtResults();

			//---------------------------------------------------------------------------------------
			// (1) Set the "TestCondition" array of product data to "_conditionCtrl"
			//---------------------------------------------------------------------------------------
			_conditionCtrl.SetData(_product.TestCondition);

            Host.SetErrorCode(_conditionCtrl.SetChannelData(_machineConfig.TesterFunctionType, _machineConfig.ChannelConfig.TesterSequenceType, 
                                                              _machineConfig.ChannelConfig.ColXCount, _machineConfig.ChannelConfig.RowYCount, false));
			//---------------------------------------------------------------------------------------
			// (2) Change item name of "_conditionCtrl" by UserData definition
			//---------------------------------------------------------------------------------------
			ChangeItemNameFromUserDefine();
			//---------------------------------------------------------------------------------------
			// (3) Reset LOP vision property of "_product" data by LopSaveSelect
			//---------------------------------------------------------------------------------------
			_conditionCtrl.ResetLOPVisionProperty(_product.LOPSaveItem);

			//---------------------------------------------------------------------------------------
            // (4) Reset default msrt range value by machine config (由 TestItemSpec 取代)
			//---------------------------------------------------------------------------------------
			//_conditionCtrl.ResetMsrtRangeBySrcMeter(_uiSetting, _machineConfig);

            ModifyCoefTableRange();

            ConfirmTestItemIsEnable();

            if (_uiSetting.IsTestResultPathByTaskSheet && _product.PathManager != null && _uiSetting != null)
            {
                _uiSetting.Overwrite(_product.PathManager);                
 
            }

			return true;
		}

		public static bool LoadMapDataFile(string fullFileName)
		{
			Console.WriteLine("[DataCenter], LoadMapDataFile(), Start");

			if (!System.IO.File.Exists(fullFileName))
			{
				return false;
			}

			Console.WriteLine("[DataCenter], LoadMapDataFile(), File Path : " + fullFileName);

			try
			{
				_mapData = MPI.Xml.XmlFileSerializer.Deserialize(typeof(MapData), fullFileName) as MapData;

				if (_mapData == null)
				{
					Console.WriteLine("[DataCenter], LoadMapDataFile(), _mapData = null");

					return false;
				}

				Console.WriteLine("[DataCenter], LoadMapDataFile(), LoadMap OK");
			}
			catch(Exception e)
			{
				Console.WriteLine("[DataCenter], LoadMapDataFile(), " + e.ToString());

				return false;
			}

			_uiSetting.MapDataFileName = Path.GetFileNameWithoutExtension(fullFileName);

            loadMapGradeColor();

			return true;
		}

		public static void loadMapGradeColor()
		{
			Dictionary<string, TestResultData> resultDataList = new Dictionary<string, TestResultData>();

			//Add BIN Result
			TestResultData bin = new TestResultData();

			bin.IsEnable = true;

			bin.KeyName = ESysResultItem.BIN.ToString();

			bin.Name = ESysResultItem.BIN.ToString();

			bin.Formate = "0";

			resultDataList.Add(bin.KeyName, bin);

			if (DataCenter._product != null)
			{
				if (DataCenter._product.TestCondition.TestItemArray != null)
				{
					foreach (var testItem in DataCenter._product.TestCondition.TestItemArray)
					{
						if (testItem.MsrtResult == null || !testItem.IsEnable)
						{
							continue;
						}

						foreach (var resultItem in testItem.MsrtResult)
						{
							if (!resultItem.IsEnable || !resultItem.IsVision)
								continue;

							if (resultItem.KeyName.Contains("LOP") && !DataCenter._product.LOPSaveItem.ToString().Contains("mcd"))
								continue;

							if (resultItem.KeyName.Contains("WATT") && !DataCenter._product.LOPSaveItem.ToString().Contains("watt"))
								continue;

							if (resultItem.KeyName.Contains("LM") && !DataCenter._product.LOPSaveItem.ToString().Contains("lm"))
								continue;

							resultDataList.Add(resultItem.KeyName, resultItem);
						}
					}
				}
			}

			List<ColorSettingData> dataList = DataCenter._mapData.ColorSetting.DataList;

			for (int i = 0; i < dataList.Count; i++)
			{
				if (!resultDataList.ContainsKey(dataList[i].KeyName))
				{
					DataCenter._mapData.ColorSetting.Remove(dataList[i].KeyName);

					i--;
				}
			}

			foreach (var item in resultDataList)
			{
				if (DataCenter._mapData.ColorSetting.ContainsKey(item.Key))
				{
					DataCenter._mapData.ColorSetting[item.Key].IsEnable = item.Value.IsEnable;
				}
				else
				{
					ColorSettingData data = new ColorSettingData();

					data.IsEnable = item.Value.IsEnable;

					data.KeyName = item.Value.KeyName;

					data.Name = item.Value.Name;

					data.Formate = item.Value.Formate;

					DataCenter._mapData.ColorSetting.DataList.Add(data);
				}
			}

			bool isBinExist = false;

			foreach (var item in DataCenter._mapData.ColorSetting.DataList)
			{
				if (item.KeyName == ESysResultItem.BIN.ToString())
				{
					isBinExist = true;
				}
			}

			if (!isBinExist)
			{
				ColorSettingData data = new ColorSettingData();

				data.IsEnable = true;

				data.KeyName = ESysResultItem.BIN.ToString();

				data.Name = ESysResultItem.BIN.ToString();

				data.Formate = "0";

				DataCenter._mapData.ColorSetting.DataList.Add(data);
			}

			List<AutoColorFilterData> autoColorDataList = DataCenter._mapData.FilterSpec.DataList;

			for (int i = 0; i < autoColorDataList.Count; i++)
			{
				if (!resultDataList.ContainsKey(autoColorDataList[i].KeyName))
				{
					DataCenter._mapData.FilterSpec.Remove(autoColorDataList[i].KeyName);

					i--;
				}
			}

			foreach (var item in resultDataList)
			{
				if (DataCenter._mapData.FilterSpec.ContainsKey(item.Key))
				{
					DataCenter._mapData.FilterSpec[item.Key].IsEnable = item.Value.IsEnable;
				}
				else
				{
					AutoColorFilterData data = new AutoColorFilterData(item.Value.KeyName, item.Value.Name);

					data.IsEnable = item.Value.IsEnable;

					data.KeyName = item.Value.KeyName;

					data.Name = item.Value.Name;

					DataCenter._mapData.FilterSpec.DataList.Add(data);
				}
			}

			MPI.Windows.Forms.BinGradeColorSet.Initialize(_mapData);

			DataCenter.SaveMapDataFile();
		}

		public static bool LoadBinDataFile(string fullFileName)
		{
			Console.WriteLine("[DataCenter], LoadBinDataFile(), Start");

			if (_smartBinning.Load(fullFileName))
			{
				Console.WriteLine("[DataCenter], LoadBinDataFile(), File Path : " + fullFileName);

				_uiSetting.BinDataFileName = Path.GetFileNameWithoutExtension(fullFileName);

				return true;
			}
			else
			{
				_smartBinning.Clear();

				return false;
			}
		}

		public  static void LoadToolsSysConfig()
		{
			string pathAndFile = Path.Combine(Constants.Paths.TOOLS_DIR, "SysConfig.xml");

			if (File.Exists(pathAndFile))
			{
				_toolConfig = MPI.Xml.XmlFileSerializer.Deserialize(typeof(SysConfig), pathAndFile) as SysConfig;
			}

			if (_toolConfig == null)
			{
				_toolConfig = new SysConfig();
			}
		}

		public static string[] GetAllFilesList(string directory, string extendStr)
		{
			DirectoryInfo di = new DirectoryInfo(directory);
			if (di.Exists == false)
			{
				return new string[1] { "" };
			}

			FileInfo[] fileInfos = di.GetFiles("*." + extendStr);

			string[] strS = new string[fileInfos.Length];
			for (int i = 0; i < strS.Length; i++)
			{
				strS[i] = Path.GetFileNameWithoutExtension(fileInfos[i].Name);//.Remove(fileInfos[i].Name.IndexOf('.'));
			}
			return strS;
		}

        public static void NewTaskSheet(string fileName)
        {
			Console.WriteLine("[DataCenter], NewTaskSheet()");

            if (File.Exists(Path.Combine(DataCenter._uiSetting.ProductPath, fileName + "." + Constants.Files.TASK_SHEET_EXTENSION)))
            {
                Host.SetErrorCode(EErrorCode.TaskSheetFileExisted);

                return;
            }

            ELOPSaveItem lopSaveItem = DataCenter._product.LOPSaveItem;
            DataCenter._uiSetting.TaskSheetFileName = fileName;
            DataCenter.CreateTaskSheet(fileName);
            DataCenter.CreateBinDataFile(fileName);
            DataCenter.CreateProductFile(fileName);
			DataCenter.CreateMapDataFile(fileName);
            DataCenter._product.LOPSaveItem = lopSaveItem;
            DataCenter.Save();
        }

		public static void CreateTaskSheet(string fileName, bool sameName = true)
		{
			Console.WriteLine("[DataCenter], CreateTaskSheet()");

			XmlDocument xmlDoc = new XmlDocument();

			string fileNameWithExt = fileName + "." + Constants.Files.TASK_SHEET_EXTENSION;
			string pathAndFile = Path.Combine(DataCenter._uiSetting.ProductPath, fileNameWithExt);
			string str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
			 "<?xml-stylesheet type=\"text/xsl\"" +
			 " href=\"FormatA.xslt\"?><TaskSheet></TaskSheet>";

			xmlDoc.LoadXml(str);
			XmlElement root = xmlDoc.DocumentElement;

			//----------------------------------------------
			// (1) Product File
			//----------------------------------------------
			XmlElement itemElem = xmlDoc.CreateElement("FileInfo");
			//itemElem.SetAttribute("Type", "Product");
			root.AppendChild(itemElem);

			XmlElement itemData = xmlDoc.CreateElement("Type");
			itemData.InnerText = "Product";
			itemElem.AppendChild(itemData);

			itemData = xmlDoc.CreateElement("Name");

			if (sameName)
				itemData.InnerText = fileName;
			else 
				itemData.InnerText = DataCenter._uiSetting.ProductFileName;
			
			itemElem.AppendChild(itemData);

			itemData = xmlDoc.CreateElement("Ext");
			itemData.InnerText = Constants.Files.PRODUCT_FILE_EXTENSION;
			itemElem.AppendChild(itemData);

			itemData = xmlDoc.CreateElement("Path");
			itemData.InnerText = DataCenter._uiSetting.ProductPath;
			itemElem.AppendChild(itemData);

			//----------------------------------------------
			// (2) Bin Data File
			//----------------------------------------------
			itemElem = xmlDoc.CreateElement("FileInfo");
			//itemElem.SetAttribute("Type", "Bin");
			root.AppendChild(itemElem);

			itemData = xmlDoc.CreateElement("Type");
			itemData.InnerText = "BinData";
			itemElem.AppendChild(itemData);

			itemData = xmlDoc.CreateElement("Name");

			if (sameName)
				itemData.InnerText = fileName;
			else
				itemData.InnerText = DataCenter._uiSetting.BinDataFileName;

			itemElem.AppendChild(itemData);

			itemData = xmlDoc.CreateElement("Ext");
			itemData.InnerText = Constants.Files.BIN_FILE_EXTENSION;
			itemElem.AppendChild(itemData);

			itemData = xmlDoc.CreateElement("Path");
			itemData.InnerText = DataCenter._uiSetting.ProductPath;
			itemElem.AppendChild(itemData);

			//----------------------------------------------
			// (3) Import Calibrate Data
			//----------------------------------------------
			itemElem = xmlDoc.CreateElement("FileInfo");
			root.AppendChild(itemElem);

			itemData = xmlDoc.CreateElement("Type");
			itemData.InnerText = "ImportCalibrateData";
			itemElem.AppendChild(itemData);

			itemData = xmlDoc.CreateElement("Name");
			itemData.InnerText = DataCenter._uiSetting.ImportCalibrateFileName;
			itemElem.AppendChild(itemData);

			itemData = xmlDoc.CreateElement("Ext");
			itemData.InnerText = "cal";
			itemElem.AppendChild(itemData);

			itemData = xmlDoc.CreateElement("Path");
			itemData.InnerText = DataCenter._uiSetting.ProductPath02;
			itemElem.AppendChild(itemData);

			//----------------------------------------------
			// (4) Import Bin Table
			//----------------------------------------------
			itemElem = xmlDoc.CreateElement("FileInfo");
			//itemElem.SetAttribute("Type", "Bin");
			root.AppendChild(itemElem);

			itemData = xmlDoc.CreateElement("Type");
			itemData.InnerText = "ImportBin";
			itemElem.AppendChild(itemData);

			itemData = xmlDoc.CreateElement("Name");
			itemData.InnerText = DataCenter._uiSetting.ImportBinFileName;
			itemElem.AppendChild(itemData);

			itemData = xmlDoc.CreateElement("Ext");
			itemData.InnerText = "sr2";
			itemElem.AppendChild(itemData);

			itemData = xmlDoc.CreateElement("Path");
			itemData.InnerText = DataCenter._uiSetting.ProductPath02;
			itemElem.AppendChild(itemData);

			//----------------------------------------------
			// (4) Import CalibSptData
			//----------------------------------------------
			itemElem = xmlDoc.CreateElement("FileInfo");
			//itemElem.SetAttribute("Type", "Bin");
			root.AppendChild(itemElem);

			itemData = xmlDoc.CreateElement("Type");
			itemData.InnerText = "ImportSptCoeff";
			itemElem.AppendChild(itemData);

			itemData = xmlDoc.CreateElement("Name");
			itemData.InnerText = DataCenter._uiSetting.ImportCalibSptDataFileName;
			itemElem.AppendChild(itemData);

			itemData = xmlDoc.CreateElement("Ext");
			itemData.InnerText = "spt";
			itemElem.AppendChild(itemData);

			itemData = xmlDoc.CreateElement("Path");
			itemData.InnerText = DataCenter._uiSetting.ProductPath02;
			itemElem.AppendChild(itemData);
	
			//----------------------------------------------
			// (5) MapData File
			//----------------------------------------------
			itemElem = xmlDoc.CreateElement("FileInfo");
			//itemElem.SetAttribute("Type", "Product");
			root.AppendChild(itemElem);

			itemData = xmlDoc.CreateElement("Type");
			itemData.InnerText = "MapData";
			itemElem.AppendChild(itemData);

			itemData = xmlDoc.CreateElement("Name");

			if (sameName)
				itemData.InnerText = fileName;
			else
				itemData.InnerText = DataCenter._uiSetting.MapDataFileName;

			itemElem.AppendChild(itemData);

			itemData = xmlDoc.CreateElement("Ext");
			itemData.InnerText = Constants.Files.MAPDATA_FILE_EXTENSION;
			itemElem.AppendChild(itemData);

			itemData = xmlDoc.CreateElement("Path");
			itemData.InnerText = DataCenter._uiSetting.ProductPath;
			itemElem.AppendChild(itemData);
	

			if (Directory.Exists(DataCenter._uiSetting.MapDataFileName) == false)
			{
				Directory.CreateDirectory(DataCenter._uiSetting.ProductPath);
			}

			xmlDoc.Save(pathAndFile);
			_uiSetting.TaskSheetFileName = fileName;
		}

		public static void CreateProductFile(string fileName)
		{
			Console.WriteLine("[DataCenter], CreateProductFile()");

			_uiSetting.ProductFileName = fileName;
			_product = new ProductData();

			_conditionCtrl.SetData(_product.TestCondition);

            // Roy, 20140814, Create New ChannelConditionTable by ChannelConfig Setting
            _conditionCtrl.SetChannelData(_machineConfig.TesterFunctionType, _machineConfig.ChannelConfig.TesterSequenceType, _machineConfig.ChannelConfig.ColXCount, _machineConfig.ChannelConfig.RowYCount, true);
		}

		public static void CreateMapDataFile(string fileName)
		{
			Console.WriteLine("[DataCenter], CreateMapDataFile()");

			_uiSetting.MapDataFileName = fileName;

			_mapData = new MapData();

            //if (_product != null && _product.TestCondition.TestItemArray != null)
            //{
            //    foreach (var testItem in _product.TestCondition.TestItemArray)
            //    {
            //        if (testItem.MsrtResult != null && testItem.IsEnable)
            //        {
            //            _mapData.ColorSetting.EnableColorOutOfRange = testItem.MsrtResult[0].EnableColorOutOfRange;

            //            foreach (var resultItem in testItem.MsrtResult)
            //            {
            //                if (resultItem.IsEnable && resultItem.IsVision)
            //                {
            //                    ColorSettingData data = resultItem.ColorSettingData.Clone() as ColorSettingData;

            //                    data.KeyName = resultItem.KeyName;

            //                    data.Name = resultItem.Name;

            //                    data.Formate = resultItem.Formate;

            //                    _mapData.ColorSetting.DataList.Add(data);
            //                }
            //            }
            //        }
            //    }
            //}
		}

		public static void CreateBinDataFile(string fileName)
		{
			Console.WriteLine("[DataCenter], CreateBinDataFile()");

			_uiSetting.BinDataFileName = fileName;	
		
			_smartBinning.Clear();

			SaveBinFile();
		}

		public static void SaveMechAndDeviceData()
		{
			string pathAndFile = string.Empty;

            if (DataCenter._rdFunc.RDFuncData.TesterConfigType != ETesterConfigType.PDTester)
            {
                DataCenter._machineConfig.IOConfig = new IOConfigData(0);
            }

			pathAndFile = Path.Combine(Constants.Paths.DATA_FILE, Constants.Files.MACHINE_DATA);
			MPI.Xml.XmlFileSerializer.Serialize(_machineConfig, pathAndFile);

            //pathAndFile = Path.Combine(Constants.Paths.DATA_FILE, Constants.Files.DEVICE_SPEC);
            //MPI.Xml.XmlFileSerializer.Serialize(_deviceSpec, pathAndFile);
		}

		public static void SaveUISettingToFile()
		{
            if (_uiSetting.IsTestResultPathByTaskSheet)
            {
                _product.PathManager = new OutputPathManager();
                _product.PathManager.Overwrite(_uiSetting);
            }
            else
            {
                _product.PathManager = null;//若未啟用IsTestResultPathByTaskSheet，節省空間，也避免後續誤將預設的路徑把先前的洗掉
            }

			if (!System.IO.Directory.Exists(Constants.Paths.DATA_FILE))
			{
				System.IO.Directory.CreateDirectory(Constants.Paths.DATA_FILE);
			}

			try
			{
				MPI.Xml.XmlFileSerializer.Serialize(_uiSetting, Path.Combine(Constants.Paths.DATA_FILE, Constants.Files.UI_SETTING));
			}
			catch
			{

			}
		}

        public static void SaveRDFuncParam()
        {    
            string userID = ((int)_uiSetting.UserID).ToString("0000");

            if (userID == _rdFunc.RDFuncData.UserID)
            {
                string rdFileNameWithExtension = string.Format("{0}{1}.{2}", Constants.Files.RDFUNC_FILENAME, userID, Constants.Files.RDFUNC_FILE_EXTENSION);

                string pathAndFile = Path.Combine(Constants.Paths.DATA_FILE, rdFileNameWithExtension);

                _rdFunc.Save(pathAndFile);
            }
        }

		public static void SaveSystemSettingToFile()
		{
			if (!System.IO.Directory.Exists(Constants.Paths.DATA_FILE))
			{
				System.IO.Directory.CreateDirectory(Constants.Paths.DATA_FILE);
			}

			try
			{
				MPI.Xml.XmlFileSerializer.Serialize(_sysSetting, Path.Combine(Constants.Paths.DATA_FILE, Constants.Files.TESTER_SETTING));
			}
			catch
			{

			}
		}

		public static void SaveBinFile()
		{
			string path = string.Empty;

			if (DataCenter._uiSetting.IsEnableRunMesSystem && DataCenter._uiSetting.IsConverterTasksheet)
			{
				path = Constants.Paths.MES_FILE_PATH;
			}
			else
			{
				path = _uiSetting.ProductPath;
			}

			string fullFileName = Path.Combine(path, _uiSetting.BinDataFileName + "." + Constants.Files.BIN_FILE_EXTENSION);

            //foreach (var sData in _smartBinning)
            //{
            //    sData.BoundaryRule
            //}
			
			_smartBinning.Save(fullFileName);
		}        

		public static void SaveProductFile()
		{
			Console.WriteLine("[DataCenter], SaveProductFile()");

            string path = string.Empty;

            if (DataCenter._uiSetting.IsEnableRunMesSystem && DataCenter._uiSetting.IsConverterTasksheet)
            {
                path = Constants.Paths.MES_FILE_PATH;
            }
            else
            {
                path = _uiSetting.ProductPath;
            }	

            string fileNameWithExt = _uiSetting.ProductFileName + "." + Constants.Files.PRODUCT_FILE_EXTENSION;

			Console.WriteLine("[DataCenter], SaveProductFile(), fileNameWithExt = " + fileNameWithExt);

			if (string.IsNullOrEmpty(path))
			{
				path = Constants.Paths.PRODUCT_FILE;
			}

			Console.WriteLine("[DataCenter], SaveProductFile(), path = " + path);

			if (!System.IO.Directory.Exists(path))
			{
				Console.WriteLine("[DataCenter], SaveProductFile(), CreateDirectory");

				System.IO.Directory.CreateDirectory(path);
			}

			try
			{
				Console.WriteLine("[DataCenter], SaveProductFile(), Serialize");

				MPI.Xml.XmlFileSerializer.Serialize(_product, Path.Combine(path, fileNameWithExt));

				Console.WriteLine("[DataCenter], SaveProductFile(), Serialize OK");
			}
			catch (Exception e)
			{
				Console.WriteLine("[DataCenter], SaveProductFile()," + e.ToString());
			}
		}

		public static void SaveMapDataFile()
		{
			Console.WriteLine("[DataCenter], SaveMapDataFile()");

			string path = string.Empty;

			if (DataCenter._uiSetting.IsEnableRunMesSystem && DataCenter._uiSetting.IsConverterTasksheet)
			{
				path = Constants.Paths.MES_FILE_PATH;
			}
			else
			{
				path = _uiSetting.ProductPath;
			}

			string fileNameWithExt = _uiSetting.MapDataFileName + "." + Constants.Files.MAPDATA_FILE_EXTENSION;

			if (string.IsNullOrEmpty(path))
			{
				path = Constants.Paths.PRODUCT_FILE;
			}

			if (!System.IO.Directory.Exists(path))
			{
				System.IO.Directory.CreateDirectory(path);
			}

			try
			{
				MPI.Xml.XmlFileSerializer.Serialize(_mapData, Path.Combine(path, fileNameWithExt));

				Console.WriteLine("[DataCenter], SaveMapDataFile(), Path = " + Path.Combine(path, fileNameWithExt));
			}
			catch(Exception e)
			{
				Console.WriteLine("[DataCenter], SaveMapDataFile()," + e.ToString());
			}
		}

        public static void SaveToolsConfig()
        {
            string pathAndFile = Path.Combine(Constants.Paths.TOOLS_DIR, "SysConfig.xml");
            if (_toolConfig == null)
            {
                _toolConfig = new SysConfig();
            }
            MPI.Xml.XmlFileSerializer.Serialize(_toolConfig, pathAndFile);
        }

		public static void ModifyCoefTableRange()
		{ 
			// Check the StartWL, EndWL, ResoluionWL, 

			if (_product == null || _product.TestCondition == null || _product.TestCondition.TestItemArray == null)
				return;

			foreach (TestItemData item in _product.TestCondition.TestItemArray)
			{
				if (item.Type == ETestType.LOPWL)
				{
					if ((item as LOPWLTestItem).CoefStartWL != _sysSetting.CoefTableStartWL ||
							(item as LOPWLTestItem).CoefEndWL != _sysSetting.CoefTableEndWL ||
							(item as LOPWLTestItem).CoefWLResolution != _sysSetting.CoefTableResolution)
					{
						double[][] oldTable = new double[(item as LOPWLTestItem).CoefTable.Length][];
						for (int i = 0; i < oldTable.Length; i++)
						{
							oldTable[i] = new double[ (item as LOPWLTestItem).CoefTable[0].Length ];
							Array.Copy((item as LOPWLTestItem).CoefTable[i], oldTable[i], (item as LOPWLTestItem).CoefTable[0].Length);
						}

						(item as LOPWLTestItem).CreateCoefTable(_sysSetting.CoefTableStartWL, _sysSetting.CoefTableEndWL, _sysSetting.CoefTableResolution);

                        for (int m = 0; m < (item as LOPWLTestItem).CoefTable.Length; m++)
                        {
                            for (int n = 0; n < oldTable.Length; n++)
                            {
                                if ((item as LOPWLTestItem).CoefTable[m][0] == oldTable[n][0])
                                {
                                    Array.Copy(oldTable[n], (item as LOPWLTestItem).CoefTable[m], oldTable[0].Length);
                                }
                            }
                        }
					}
				}
			}						
		}

        public static void ConfirmTestItemIsEnable()
        {
			if (_product == null || _product.TestCondition == null || _product.TestCondition.TestItemArray == null)
				return;

            uint electSettingOrder = 0;

            foreach (TestItemData item in _product.TestCondition.TestItemArray)
            {
                if (!item.IsEnable)
                {
                    if (item.MsrtResult == null)
                        continue;

                    foreach (TestResultData data in item.MsrtResult)
                    {
                        data.IsVerify = false;
                        data.IsSkip = false;
                    }
                }

                if (item.ElecSetting != null)
                {
                    foreach (var eData in item.ElecSetting)
                    {
                        eData.Order = electSettingOrder;
                        electSettingOrder++;
                    }
                }

                if (!_sysSetting.SpecCtrl.CheckTestItemTypeEnable(item.Type.ToString()))
                {
                    item.IsDeviceSetEnable = false;

                    continue;
                }
                else
                {
                    item.IsDeviceSetEnable = true;
                }

                //----------------------------------------------------------------------------------
                switch (item.Type)
                {
                    case ETestType.IFH:
                    case ETestType.IF:
                    
                    case ETestType.VF:
                    case ETestType.LOP:
                    case ETestType.VAC:
                        {
                            if (item.ElecSetting[0].ForceTime < 0.000001)
                            {
                                item.IsDeviceSetEnable = false;
                            }

                            break;
                        }
                    //----------------------------------------------------------------------------------
                    case ETestType.LOPWL:
                        {
                            if (item.ElecSetting[0].ForceTime < 0.000001)
                            {
                                item.IsDeviceSetEnable = false;
                            }

                            if (_sysSetting.SpecCtrl.IsAcTestItem)
                            {
                                if (!(item as LOPWLTestItem).IsACSourceMeter)
                                {
                                    item.IsDeviceSetEnable = false;
                                }
                            }
                            else
                            {
                                if ((item as LOPWLTestItem).IsACSourceMeter)
                                {
                                    item.IsDeviceSetEnable = false;
                                }
                            }

                            if ((item as LOPWLTestItem).IsUseMsrtAsForceValue)
                            {
                                int refOrder = -1;
                                bool refEnable = false;

                                foreach (var subItem in _product.TestCondition.TestItemArray)
                                {
                                    if (subItem.KeyName.Contains("PIV"))
                                    {
                                        foreach (var msrt in subItem.MsrtResult)
                                        {
                                            if (msrt.KeyName == (item as LOPWLTestItem).RefMsrtKeyName)
                                            {
                                                refOrder = subItem.Order;

                                                refEnable = subItem.IsEnable;

                                                break;
                                            }
                                        }

                                        if (refOrder >= 0)
                                        {
                                            break;
                                        }
                                    }
                                }

                                if (refOrder < 0)
                                {
                                    item.IsDeviceSetEnable = false;
                                }
                                else
                                {
                                    if (!refEnable)
                                    {
                                        item.IsDeviceSetEnable = false;
                                    }

                                    if (item.Order < refOrder)
                                    {
                                        item.IsDeviceSetEnable = false;
                                    }
                                }
                            }

                            break;
                        }
                    //----------------------------------------------------------------------------------
                    case ETestType.DVF:
                        {
                            if ((item.ElecSetting[0].ForceTime < 0.000001) ||
                                (item.ElecSetting[1].ForceTime < 0.000001) ||
                                (item.ElecSetting[2].ForceTime < 0.000001))
                            {
                                item.IsDeviceSetEnable = false;
                            }

                            break;
                        }
                    //----------------------------------------------------------------------------------
                    case ETestType.LIV:
                        {
                            if ((item as LIVTestItem).LIVForceTime < 0.000001 ||
                                ((item as LIVTestItem).LIVSweepPoints == 0))
                            {
                                item.IsDeviceSetEnable = false;
                            }

                            break;
                        }
                    case ETestType.TRANSISTOR:
                        {
                            if (_machineConfig.SpectrometerModel == ESpectrometerModel.NONE && (item as TransistorTestItem).TRIsTestOptical)
                            {
                                item.IsDeviceSetEnable = false;
                                break;
                            }

                            if (_machineConfig.PDSensingMode == EPDSensingMode.NONE && (item as TransistorTestItem).TRIsEnableDetector)
                            {
                                item.IsDeviceSetEnable = false;
                                break;
                            }

                            if (_machineConfig.TesterFunctionType != ETesterFunctionType.Multi_Terminal)
                            {
                                item.IsDeviceSetEnable = false;
                                break;
                            }

                            int deviceCount = _machineConfig.ChannelConfig.ChannelCount;

                            foreach (var data in (item as TransistorTestItem).TRTerminalDescription)
                            {
                                if (data.SMU != ESMU.None)
                                {
                                    if ((int)data.SMU < deviceCount)
                                    {

                                    }
                                    else
                                    {
                                        item.IsDeviceSetEnable = false;
                                        break;
                                    }
                                }
                            }

                            break;
                        }
                    //----------------------------------------------------------------------------------
                    case ETestType.VR:
                        {
                            if (item.ElecSetting[0].ForceTime < 0.000001)
                            {
                                item.IsDeviceSetEnable = false;
                            }

                            if ((item as VRTestItem).IsUseVzAsForceValue)
                            {
                                int refOrder = -1;
                                bool refEnable = false;

                                foreach (var subItem in _product.TestCondition.TestItemArray)
                                {
                                    foreach (var msrt in subItem.MsrtResult)
                                    {
                                        if (msrt.KeyName == (item as VRTestItem).RefVzKeyName)
                                        {
                                            refOrder = subItem.Order;

                                            refEnable = subItem.IsEnable;

                                            break;
                                        }
                                    }

                                    if (refOrder >= 0)
                                    {
                                        break;
                                    }
                                }

                                if (refOrder < 0)
                                {
                                    item.IsDeviceSetEnable = false;
                                }
                                else 
                                {
                                    if (!refEnable)
                                    {
                                        item.IsDeviceSetEnable = false;
                                    }
                                    
                                    if (item.Order < refOrder)
                                    {
                                        item.IsDeviceSetEnable = false;
                                    }
                                }
                            }

                            break;
                        }
                    //----------------------------------------------------------------------------------
                    case ETestType.IZ:
                        {
                            if (item.ElecSetting[0].ForceTime < 0.000001)
                            {
                                item.IsDeviceSetEnable = false;
                            }

                            if ((item as IZTestItem).IsUseIrAsForceValue)
                            {
                                int refOrder = -1;
                                bool refEnable = false;

                                foreach (var subItem in _product.TestCondition.TestItemArray)
                                {
                                    foreach (var msrt in subItem.MsrtResult)
                                    {
                                        if (msrt.KeyName == (item as IZTestItem).RefIrKeyName)
                                        {
                                            refOrder = subItem.Order;

                                            refEnable = subItem.IsEnable;

                                            break;
                                        }
                                    }

                                    if (refOrder >= 0)
                                    {
                                        break;
                                    }
                                }

                                if (refOrder < 0)
                                {
                                    item.IsDeviceSetEnable = false;
                                }
                                else
                                {
                                    if (!refEnable)
                                    {
                                        item.IsDeviceSetEnable = false;
                                    }

                                    if (item.Order < refOrder)
                                    {
                                        item.IsDeviceSetEnable = false;
                                    }
                                }
                            }

                            break;
                        }
                        break;
                    //----------------------------------------------------------------------------------
                    case ETestType.LCR:
                        if ((item as LCRTestItem).LCRSetting.Frequency == 0 || (item as LCRTestItem).LCRSetting.SignalLevelV == 0)
                        {
                            item.IsDeviceSetEnable = false;
                        }

                        break;
                    //-----------------------------------------------------------///////////////////////////////////////////////////20170904 David
                    case ETestType.LCRSWEEP:
                        if ((item as LCRSweepTestItem).LCRSetting.Frequency == 0 || (item as LCRSweepTestItem).LCRSetting.SignalLevelV == 0)
                        {
                            item.IsDeviceSetEnable = false;
                        }

                        break;
                        //-----------------------------------------------------------
                    case ETestType.OSA:
                        {
                            if (item.ElecSetting[0].ForceTime < 0.000001)
                            {
                                item.IsDeviceSetEnable = false;
                            }
                            else
                            {
                                item.IsDeviceSetEnable = true;
                            }

                            break;
                        }
                    //-----------------------------------------------------------
                    default:
                        break;
                }
            }
        }

        public static bool ImportSptXYCalibrationData(string sptCalibFilePath)
        {
            string[][] importData = null;
            DataCenter._product.ProductSptXwaveCoef = null;
            DataCenter._product.ProductSptYintCoef = null;
            DataCenter._product.ProductSptYweight = null;
            DataCenter._product.SptCalibPathAndFile = sptCalibFilePath;

            double[] sptXaxisCoefficient = null;
            double[] sptYaxisCalibArray = null;
            double[] sptYaxisWeightArray = null;

            if (File.Exists(sptCalibFilePath) == false)
            {
                //Host.SetErrorCode(EErrorCode.ImportSpectrometerCalibDataFail;
                DataCenter._product.SptCalibPathAndFile = "";
                return false;
            }
            DataCenter._product.SptCalibPathAndFile = sptCalibFilePath;

            if (CSVUtil.ReadFromCSV(sptCalibFilePath, out importData) == false)
            {
              //  Host.SetErrorCode(EErrorCode.ImportSpectrometerCalibDataFail;
                DataCenter._product.SptCalibPathAndFile = "";
                return false;
            }

            if (DataCenter._conditionCtrl.ImportSptCalibData(importData, ref sptXaxisCoefficient, ref  sptYaxisCalibArray, ref sptYaxisWeightArray) == true)
            {
                DataCenter._product.ProductSptXwaveCoef = sptXaxisCoefficient;
                DataCenter._product.ProductSptYintCoef = sptYaxisCalibArray;
                DataCenter._product.ProductSptYweight = sptYaxisWeightArray;
                return true;
            }
            else
            {
                return false;
            }

        }

        public static bool ImportChuckData(string[][] readData)
        {
            if (DataCenter._machineConfig.TesterCommMode != ETesterCommMode.TCPIP)
            {
                return true;
            }

            int NumbersOfChuck = 8;

            try
            {
                int chuckStartIndex = 0;

                for (int row = 0; row < readData.Length; row++)
                {
                    if (readData[row][0] == "[CHUCK]")
                    {
                        chuckStartIndex = row + 2;
                        break;
                    }
                }

                string[][] dataArray = new string[NumbersOfChuck][];

                for (int i = 0; i < NumbersOfChuck; i++)
                {
                    dataArray[i] = readData[chuckStartIndex+i];
                }

                for (int i = 0; i < NumbersOfChuck; i++)
                {
                    string[] content = dataArray[i];
                    double[] value = new double[content.Length];

                    if (content.Length != 5)
                    {
                        continue;
                    }

                    for (int k = 0; k < content.Length; k++)
                    {
                        double.TryParse(content[k], out value[k]);
                    }

                    GainOffsetData[] LOPCorrectArray = DataCenter._product.ChuckLOPCorrectArray[i];

                    for (int k = 0; k < LOPCorrectArray.Length; k++)
                    {
                        GainOffsetData gainOffset = LOPCorrectArray[k];
                        gainOffset.Gain = value[k+1];
                        // sb.Append(temp.KeyName);
                    }

                    DataCenter._product.ChuckResistanceCorrectArray[i] = value[4];
                }

                return true;
            }
            catch
            {
               return false;
            }

        }

        public static bool ImportDescribeData(string[][] data)
        {
            for (int row = 0; row < data.Length; row++)
            {
                if (data[row].Length>1)			
                {
                    switch (data[row][0])
                    {
                        case "SptSerialNumbers":
                            break;
                        case "MachineName":
                            //DataCenter._uiSetting.MachineName = data[row][1];
                            break;
                        case "ChipPolarity" :
                            //int chipPolarity=0;
                            //int.TryParse(data[row][1],out chipPolarity);               
                            //DataCenter._product.TestCondition.ChipPolarity = (EPolarity)chipPolarity;
                            break;
                        case "LOPSaveItem":   // 0 mcd // 1 : mw
                            //int lOPSaveItem = 0;
                            //int.TryParse(data[row][1], out lOPSaveItem);
                            //DataCenter._product.LOPSaveItem = (ELOPSaveItem)lOPSaveItem;
                            break;
                        case "FilterWheelPos":
                            uint filterWheelPos = 0;
                            uint.TryParse(data[row][1], out filterWheelPos);
                            if (filterWheelPos >= 1)
                            {
                                DataCenter._product.ProductFilterWheelPos = filterWheelPos - 1;
                            }
                            break;
                        case "CalByWave":
                            int calByWave = 0;
                            int.TryParse(data[row][1], out calByWave);
                            DataCenter._product.TestCondition.CalByWave = (ECalBaseWave)calByWave;
                            break;
                        case "Resistance":
                            double resistance = 0;
                            double.TryParse(data[row][1], out resistance);
                            DataCenter._product.Resistance = resistance;
                            break;
                       case "SacnStartWavelength":
                            uint startWave = 0;
                            uint.TryParse(data[row][1], out startWave);
                            DataCenter._sysSetting.OptiDevSetting.StartWavelength = startWave;
                            break;
                       case "SacnEndWavelength":
                            uint endWave = 0;
                            uint.TryParse(data[row][1], out endWave);
                            DataCenter._sysSetting.OptiDevSetting.EndWavelength = endWave;
                            break;
                       case "LOPWL_1":
                       case "LOPWL_2":
                       case "LOPWL_3":
                            //string[] content = data[row];

                            //if (content.Length != 4)
                            //{
                            //    continue;
                            //}

                            //uint mode = 0;
                            //uint.TryParse(content[1], out mode);
                            //double fixedTime=0.0d;
                            //double.TryParse(content[2], out fixedTime);
                            //double limitTime = 0.0d;
                            //double.TryParse(content[3], out limitTime);

                            //if (DataCenter._product.TestCondition.TestItemArray == null)
                            //{
                            //    continue;
                            //}

                            // foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
                            // {
                            //     if (item is LOPWLTestItem)
                            //     {
                            //         if (item.KeyName == data[row][0])
                            //         {
                            //             if (mode == 1)
                            //             {
                            //                 (item as LOPWLTestItem).OptiSetting.SensingMode = ESensingMode.Fixed;
                            //             }
                            //             else
                            //             {
                            //                 (item as LOPWLTestItem).OptiSetting.SensingMode = ESensingMode.Limit;
                            //             }

                            //             (item as LOPWLTestItem).OptiSetting.LimitIntegralTime = limitTime;
                            //             (item as LOPWLTestItem).OptiSetting.FixIntegralTime = fixedTime;

                            //             break;
                            //         }
                            //     }
                            // }
                            
                            break;


                        default :
                            break;
                    }
                }							
            }
            return true;
        }

		public static void ImportCalibrateData(string path, string fileNameWithExt)
		{
			string[][] importData = null;
			double startWL = DataCenter._product.DispCoefStartWL;
			double endWL = DataCenter._product.DispCoefEndWL;
            string sptCalibFilePath = "";

            if (CSVUtil.ReadFromCSV(Path.Combine(path, fileNameWithExt), out importData) == false)
			{
				Host.SetErrorCode(EErrorCode.ImportCalibrateDatafFail);
				return;
			}

            if (!ImportDescribeData(importData))
            {
                Host.SetErrorCode(EErrorCode.ImportCalibrateNotMathSystem);
                return;
            }

            //if (!ImportDescribeData(importData))
            //{
            //    Host.SetErrorCode(EErrorCode.ImportCalibrateNotMathSystem;
            //    return;
            //}

            if (!ImportChuckData(importData))
            {
                Host.SetErrorCode(EErrorCode.ImportChuckCalibDataFile);
                return;
            }   

            DataCenter._conditionCtrl.ImportCalibrationFile(importData, ref startWL, ref endWL, ref sptCalibFilePath);
            DataCenter._product.DispCoefStartWL = startWL;
            DataCenter._product.DispCoefEndWL = endWL;

            if (DataCenter._uiSetting.ShowMapKeyName != "")
            {
                string str = DataCenter._uiSetting.ShowMapKeyName;

                if(str.IndexOf("_")>0)
                {
                    string keyIndex = str.Substring(str.IndexOf("_"));

                    string itemName = str.Remove(str.IndexOf("_"));

                    if (itemName == "LOP" || itemName == "WATT" || itemName == "LM")
                    {
                        switch (DataCenter._product.LOPSaveItem)
                        {
                            case ELOPSaveItem.mcd:
                                itemName = "LOP";
                                break;
                            case ELOPSaveItem.watt:
                                itemName = "WATT";
                                break;
                            case ELOPSaveItem.lm:
                                itemName = "LM";
                                break;
                            default:
                                break;
                        }
                    }
                    DataCenter._uiSetting.ShowMapKeyName = itemName + keyIndex;
                }
                else
                {
                    DataCenter._uiSetting.ShowMapKeyName = str;
                }
            }

            //    DataCenter._uiSetting.ShowMapKeyName = itemName + keyIndex;
            //}



            //---------------------------------------------------------------------------------------
            // (1) Set the "TestCondition" array of product data to "_conditionCtrl"
            //---------------------------------------------------------------------------------------
            _conditionCtrl.SetData(_product.TestCondition);
            //---------------------------------------------------------------------------------------
            // (2) Change item name of "_conditionCtrl" by UserData definition
            //---------------------------------------------------------------------------------------
            ChangeItemNameFromUserDefine();
            //---------------------------------------------------------------------------------------
            // (3) Reset LOP vision property of "_product" data by LopSaveSelect
            //---------------------------------------------------------------------------------------
            _conditionCtrl.ResetLOPVisionProperty(_product.LOPSaveItem);

            //---------------------------------------------------------------------------------------
            // (4) Reset default msrt range value by machine config (由 TestItemSpec 取代)
            //---------------------------------------------------------------------------------------
            //_conditionCtrl.ResetMsrtRangeBySrcMeter(_uiSetting, _machineConfig);

            ModifyCoefTableRange();

            ConfirmTestItemIsEnable();

            if (sptCalibFilePath == "")
            {
                return;
            }
            else if (sptCalibFilePath == "NONE")
            {
                return;
            }
            else
            {
                if (!ImportSptXYCalibrationData(sptCalibFilePath))
                {
                    Host.SetErrorCode(EErrorCode.ImportSpectrometerCalibDataFail);
                }
            }
		}

		public static bool ImportBinTable(string path, string fileNameWithExt)
		{
					return false;
				}
	
        public static void ImportSptCalibData(string path, string fileNameWithExt)
        {
            string[][] importData = null;
            double[] sptXaxisCoefficient = DataCenter._product.ProductSptXwaveCoef;
            double[] sptYaxisCalibArray = DataCenter._product.ProductSptYintCoef;
            double[] sptYaxisWeightArray = DataCenter._product.ProductSptYintCoef;

            if (CSVUtil.ReadFromCSV(Path.Combine(path, fileNameWithExt), out importData) == false)
            {
                Host.SetErrorCode(EErrorCode.ImportCalibrateDatafFail);
                return;
            }

            if (DataCenter._conditionCtrl.ImportSptCalibData(importData, ref sptXaxisCoefficient, ref  sptYaxisCalibArray, ref sptYaxisWeightArray) == true)          
            {
                   DataCenter._product.ProductSptXwaveCoef = sptXaxisCoefficient;
                   DataCenter._product.ProductSptYintCoef = sptYaxisCalibArray;
                   DataCenter._product.ProductSptYweight=sptYaxisWeightArray;
            }
           // double startWL = DataCenter._product.DispCoefStartWL;
           // double endWL = DataCenter._product.DispCoefEndWL;

        }

		public static void ExportCalibrateData(string path, string fileNameWithExt)
		{
            string[] exportData = DataCenter._conditionCtrl.ExportGainOffsetData(DataCenter._uiSetting.UserID,DataCenter._product.DispCoefStartWL, DataCenter._product.DispCoefEndWL);
          
            StringBuilder sb = new StringBuilder();

            List<string> description = new List<string>(10);

            string[] chuckData = new string[10];

            description.Add("[ESD]");

            if (DataCenter._product.TestCondition.TestItemArray != null)
            {
                //foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
                //{
                //    if (item.Type == ETestType.ESD)
                //    {
                //        sb.Clear();

                //        sb.Append(item.KeyName);

                //        sb.Append(",");

                //        sb.Append(item.Name);

                //        sb.Append(",");

                //        sb.Append((item as ESDTestItem).EsdSetting.GainVoltCH1.ToString());

                //        sb.Append(",");

                //        sb.Append((item as ESDTestItem).EsdSetting.OffsetVoltCH1.ToString());

                //        description.Add(sb.ToString());
                //    }
                //}
            }

            sb.Clear();

            description.Add("[CHUCK]");

            sb.Append("ChuckIndex,LOP(mcd),LOP(mW),LOP(LM),Resistence");

            description.Add(sb.ToString());

            for (int i = 0; i < 8; i++)
            {
                GainOffsetData[] LOPCorrectArray = DataCenter._product.ChuckLOPCorrectArray[i];
                sb.Clear();
                sb.Append(i + 1);
                sb.Append(",");

                for (int k = 0; k < LOPCorrectArray.Length; k++)
                {
                    GainOffsetData temp = LOPCorrectArray[k];
                    sb.Append(temp.Gain);
                    sb.Append(",");
                }

                sb.Append(DataCenter._product.ChuckResistanceCorrectArray[i]);
                description.Add(sb.ToString());
            }
          //  StringBuilder sb = new StringBuilder();
            sb.Clear();
            description.Add("");
            description.Add("[Description]");
            // SptSerialNumbers
            sb.Clear();
            sb.Append("MachineName");
            sb.Append(",");
            sb.Append(DataCenter._uiSetting.MachineName);
            description.Add(sb.ToString());
            sb.Clear();
            sb.Append("SptSerialNumbers");
            sb.Append(",");
            sb.Append(DataCenter._machineInfo.SpectrometerSN);
            description.Add(sb.ToString());
            // ChipPolarity
            sb.Clear();
            sb.Append("ChipPolarity");
            sb.Append(",");
            sb.Append((int)DataCenter._product.TestCondition.ChipPolarity);
            description.Add(sb.ToString());        
            sb.Clear();
            // LOPSaveItem
            sb.Append("LOPSaveItem");
            sb.Append(",");
            sb.Append((int)DataCenter._product.LOPSaveItem);
            description.Add(sb.ToString());
            sb.Clear();
            // FilterWheelPos
            sb.Append("FilterWheelPos");
            sb.Append(",");
            sb.Append(DataCenter._product.ProductFilterWheelPos+1);
            description.Add(sb.ToString());
            sb.Clear();
            // CalByWave
            sb.Append("CalByWave");
            sb.Append(",");
            sb.Append((int)DataCenter._product.TestCondition.CalByWave);
            description.Add(sb.ToString());
            sb.Clear();
            // Resistance
            sb.Append("Resistance");
            sb.Append(",");
            sb.Append(DataCenter._product.Resistance.ToString());
            description.Add(sb.ToString());
            sb.Clear();
            // SacnStartWavelength
            sb.Append("SacnStartWavelength");
            sb.Append(",");
            sb.Append((int)DataCenter._sysSetting.OptiDevSetting.StartWavelength);
            description.Add(sb.ToString());
            sb.Clear();
            // SacnEndWavelength
            sb.Append("SacnEndWavelength");
            sb.Append(",");
            sb.Append((int)DataCenter._sysSetting.OptiDevSetting.EndWavelength);
            description.Add(sb.ToString());
            sb.Clear();

            if (DataCenter._product.TestCondition.TestItemArray != null)
            {
            foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
            {
                if (item is LOPWLTestItem)
                {
                    sb.Append(item.KeyName);
                    sb.Append(",");
                    int sensMode = (int)(item as LOPWLTestItem).OptiSetting.SensingMode;
                    sb.Append(sensMode.ToString());
                    sb.Append(",");
                    sb.Append((item as LOPWLTestItem).OptiSetting.FixIntegralTime.ToString());
                    sb.Append(",");
                    sb.Append((item as LOPWLTestItem).OptiSetting.LimitIntegralTime.ToString());
                    description.Add(sb.ToString());
                    sb.Clear();
                }
            }
            }


            // SpetrometerCoeffFilePath
            string exportSptXYCalibrationPath="";

             if (DataCenter._product.SptCalibPathAndFile == "")
             {
                 exportSptXYCalibrationPath = "SpetrometerCoeffFilePath,NONE";
             }
             else
             {
                 exportSptXYCalibrationPath = "SpetrometerCoeffFilePath," + DataCenter._product.SptCalibPathAndFile;
             }
             description.Add(exportSptXYCalibrationPath);

            // SacnEndWavelength
          
			try
			{
				using (StreamWriter sw = new StreamWriter(Path.Combine(path,fileNameWithExt), false))
				{
                    foreach (string str in exportData)
                    {
                        sw.WriteLine(str);
                    }
                    foreach (string str in description)
                    {
                        sw.WriteLine(str);
                    }
                   // sw.WriteLine(exportSptXYCalibrationPath);
					sw.Close();
				}
			}
			catch 
			{
				Host.SetErrorCode(EErrorCode.ExportCalibrateDataFail);
				return;
			}
		}

		public static void ExportBinTable(string path, string fileNameWithExt)
		{
	
		}

        public static void ChangeMapRowColOfTester(ref int  Xcoord, ref int Ycoord)
        {
            switch (_sysSetting.TesterCoord)
            {
                case (int)ECoordSet.First:
                    Ycoord *= (-1);
                    break;
                case (int)ECoordSet.Second:
                    Ycoord *= (-1);
                    Xcoord *= (-1);
                    break;
                case (int)ECoordSet.Third:
                    Xcoord *= (-1);
                    break;
                case (int)ECoordSet.Fourth:
                    break;
                default:
                    break;
            }
        }

        public static void ChangeMapRowCol(int left, int top, int right, int bottom, int refCol, int refRow, int theta)
        {
            switch (_sysSetting.ProberCoord)
            {
                case (int)ECoordSet.Second:
                    left *= (-1);
                    right *= (-1);
                    break;
                case (int)ECoordSet.Third:
                    left *= (-1);
                    right *= (-1);
                    top *= (-1);
                    bottom *= (-1);
                    break;
                case (int)ECoordSet.Fourth:
                    top *= (-1);
                    bottom *= (-1);
                    break;
                default:
                    break;
            }


            //Map固定為第四象限
            //top *= (-1);
            //bottom *= (-1);

            switch (_sysSetting.TesterCoord)
            {
                case (int)ECoordSet.Second:
                    left *= (-1);
                    right *= (-1);
                    break;
                case (int)ECoordSet.Third:
                    left *= (-1);
                    right *= (-1);
                    top *= (-1);
                    bottom *= (-1);
                    break;
                case (int)ECoordSet.Fourth:
                    top *= (-1);
                    bottom *= (-1);
                    break;
                default:
                    break;
            }

            //ChangeMapRowColOfTester(ref refCol, ref refRow);

            /////////////////這段是關鍵////////////////////////////
            //由於map ui的座標系為第四項限，因此假設Tester在第一象限若要tester座標top系與map上方重合，會需要將tester 的top對應到map的bottom


            switch (_sysSetting.TesterCoord)
            {
                case (int)ECoordSet.First:
                    {
                        int tmp = top;
                        top = bottom ;
                        bottom = tmp;
                    }
                    break;
                case (int)ECoordSet.Second:
                    {
                        int tmp = top;
                        top = bottom;
                        bottom = tmp;

                        tmp = left;
                        left = right;
                        right = tmp;
                    }
                    break;
                case (int)ECoordSet.Third:
                    {
                        int tmp = left;
                        left = right;
                        right = tmp;
                    }
                    break;
                case (int)ECoordSet.Fourth:
                    break;
                default:
                    break;
            }
            /////////////////這段是關鍵////////////////////////////

            left += refCol;
            top += refRow;
            right += refCol;
            bottom += refRow;

             //--------------------------------------------------------------------
            // 先推算出新的 Boundary, 再做座標旋轉
            // x' = x * cos(Theta) - y * sin(Theta)
            // y' = x * sin(Theta) + y * cos(Theta)

            int tempL = left;   // Xmin
            int tempT = top;    // Ymin
            int tempR = right;  // Xmax
            int tempB = bottom; // Ymax

            if (theta == (int)ECoordinateRotation.CW90)
            {
                left = tempB * (-1);
                top = tempL;
                right = tempT * (-1);
                bottom = tempR;
                Console.WriteLine("[DataCenter], ChangeMapRowCol(), Theta = CW90");
            }
            else if (theta == (int)ECoordinateRotation.CW180)
            {
                left = tempR * (-1);
                top = tempB * (-1);
                right = tempL * (-1);
                bottom = tempT * (-1);
                Console.WriteLine("[DataCenter], ChangeMapRowCol(), Theta = CW180");
            }
            else if (theta == (int)ECoordinateRotation.CW270)
            {
                left = tempT;
                top = tempR * (-1);
                right = tempB;
                bottom = tempL * (-1);
                Console.WriteLine("[DataCenter], ChangeMapRowCol(), Theta = CW270");
            }
            //--------------------------------------------------------------------

            _uiSetting.WaferMapLeft = left; // ColX min
            _uiSetting.WaferMapTop = top;  // RowY min
            _uiSetting.WaferMapRight = right;
            _uiSetting.WaferMapBottom = bottom;
        }

        public static void ChangeRowColToProbe(ref int col, ref int row)
		  {
			  switch (DataCenter._sysSetting.ProberCoord)
			  {
				  case (int)ECoordSet.Second:
					  col *= (-1);
					  break;
				  case (int)ECoordSet.Third:
					  col *= (-1);
					  row *= (-1);
					  break;
				  case (int)ECoordSet.Fourth:
					  row *= (-1);
					  break;
				  default:
					  break;
			  }

			  switch (DataCenter._sysSetting.TesterCoord)
			  {
				  case (int)ECoordSet.Second:
					  col *= (-1);
					  break;
				  case (int)ECoordSet.Third:
					  col *= (-1);
					  row *= (-1);
					  break;
				  case (int)ECoordSet.Fourth:
					  row *= (-1);
					  break;
				  default:
					  break;
			  }
		  }

        public static void MoveDATFileToBackup()
        {
            if (!Directory.Exists(Constants.Paths.MPI_TEMP_DIR2))
            {
                Directory.CreateDirectory(Constants.Paths.MPI_TEMP_DIR2);
            }

  
            try
            {
                DirectoryInfo dir = new DirectoryInfo(Constants.Paths.LEDTESTER_TEMP_DIR);

                FileSystemInfo[] infos = dir.GetFileSystemInfos("*.dat");

                for (int i = 0; i < infos.Length; i++)
                {
                    string sourceFile = infos[i].FullName;

                    string sourceFileName = infos[i].Name;

                    string targetFile = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, sourceFileName);

                    if (File.Exists(targetFile))
                    {
                        File.Delete(targetFile);
                    }

                    File.Copy(sourceFile, targetFile);

                    File.Delete(sourceFile);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.ToString());

            }
        }
			
		public static void Save()
		{
            SaveRDFuncParam();
			SaveMechAndDeviceData();
			SaveUISettingToFile();
			SaveSystemSettingToFile();
			SaveProductFile();
			SaveBinFile();
            SaveMapDataFile();
            SaveSystemCali();
            Host._MPIStorage.SaveReportHeadToFile();
		}

		public static void Close()
		{
			Save();
		}

        public static void SaveAsRecipe()
        {

        }

        public static bool LoadSysCail()
        {
            string pathAndFile = Path.Combine(Constants.Paths.DATA_FILE, Constants.Files.SYSCAL_FILE);

            return _sysCali.Open(pathAndFile);
        }

        public static void SaveSystemCali()
        {
            string pathAndFile = Path.Combine(Constants.Paths.DATA_FILE, Constants.Files.SYSCAL_FILE);

            _sysCali.Save(pathAndFile);
        }

		#endregion

	}
}