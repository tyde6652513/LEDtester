using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;
using MPI.Tester.MES.Data;
using System.IO;
using System.Xml;

namespace MPI.Tester.MES.User.GPI
{
	class MESProcess : ProcessBase
	{
        protected override Tester.Data.EErrorCode OpenFileAndParse(Tester.Data.UISetting uiSetting, MachineConfig machineConfig)
		{
			bool isDeleteAMIFile = (GlobalData.ProberRecipeName != "");

			//string fileName = uiSetting.WaferNumber + ".ami";

            //string serverConditonFullPath = Path.Combine(uiSetting.MESPath, fileName);

            if(!Directory.Exists(uiSetting.MESPath))
            {
                Console.WriteLine("[MESProcess], OpenFileAndParse(), Auto Recipe Path not Exist:" +  uiSetting.MESPath);

				this._describe.AppendLine("Auto Recipe Path not Exist:" +  uiSetting.MESPath);

                return EErrorCode.MES_LoadTaskError;
            }

            DirectoryInfo di = new DirectoryInfo(uiSetting.MESPath);

            FileInfo[] fis = di.GetFiles("*.ami");

            if (fis.Length != 1)
            {
				Console.WriteLine("[MESProcess], OpenFileAndParse(), ami file not find or Too much");

				this._describe.AppendLine("ami file not find or Too much");

                foreach (var item in fis)
                {
                    Console.WriteLine("[MESProcess], OpenFileAndParse(), file Name:" + item.FullName);
                }

                return EErrorCode.MES_LoadTaskError;
            }

            string fileName = Path.GetFileName(fis[0].FullName);

			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fis[0].FullName);

            string serverConditonFullPath = fis[0].FullName;

			string loaclConditonFullPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, fileName);

			DriveInfo driveInfo = new DriveInfo(serverConditonFullPath);

			if (!driveInfo.IsReady)
			{
				Console.WriteLine("[MESProcess], OpenFileAndParse(), drive is not Ready:" + serverConditonFullPath);

				this._describe.AppendLine("drive is not Ready:" + serverConditonFullPath);

				return EErrorCode.MES_LoadTaskError;
			}

			if (File.Exists(serverConditonFullPath))
			{
				MPIFile.CopyFile(serverConditonFullPath, loaclConditonFullPath);
			}
			else
			{
				Console.WriteLine("[MESProcess], OpenFileAndParse(), ami is not exits:" + serverConditonFullPath);

				this._describe.AppendLine("ami is not exits:" + serverConditonFullPath);

				return EErrorCode.MES_LoadTaskError;
			}

			List<string[]> file = Tool.ToolBox.ReadCSV(loaclConditonFullPath);

			if (file == null)
			{
				this._describe.AppendLine("ami open fail:" + serverConditonFullPath);

				return EErrorCode.MES_LoadTaskError;
			}

			//PROBE_TYPE,WAFER_NO,JOB_IN_DATETIME,CONFIGFILE_NAME1,CONFIGFILE_NAME2,CONFIGFILE_NAME3,CONFIGFILE_NAME4,JOB_IN_USER,MACHINE_NO,MPI_PRD
			//1,B215211-07029,2015/4/15 14:48,BB(2632)-140911.cal,BP-2632(240mA).mcg,ESD-HBM-500.eeg,2632k.gcg,v0133,3PBH070,B2632CQPA
			if (file.Count < 2 || file[0].Length != 10 || file[1].Length != 10)
			{
				Console.WriteLine("[MESProcess], OpenFileAndParse(), file.Count < 2 || file[0].Length != 10 || file[1].Length != 10");

				this._describe.AppendLine("ami format error:" + serverConditonFullPath);

				return EErrorCode.MES_LoadTaskError;
			}

			//TAPE_NO(Wafer number)
			if (file[1][1] != fileNameWithoutExtension)
			{
				Console.WriteLine("[MESProcess], OpenFileAndParse(), file[1][1] != uiSetting.WaferNumber," + file[1][1] + ", " + fileNameWithoutExtension);

				this._describe.AppendLine("wafer number not match, file name:" + serverConditonFullPath + ", file data:" + file[1][1]);

				return EErrorCode.MES_LoadTaskError;
			}

			//CONFIGFILE_NAME1(*.cal File)
			string localFile = Path.Combine(uiSetting.ProductPath02, file[1][3]);

			if (!File.Exists(localFile))
			{
				Console.WriteLine("[MESProcess], OpenFileAndParse(), cal file is not exist:" + localFile);

				this._describe.AppendLine("cal file is not exist:" + localFile);

				return EErrorCode.MES_LoadTaskError;
			}			

			//CONFIGFILE_NAME2(*.mcg File = *.pd File)
			string serverFile = Path.Combine(uiSetting.MESPath2, file[1][4]);

			localFile = Path.Combine(uiSetting.ProductPath, Path.GetFileNameWithoutExtension(serverFile) + "." + Constants.Files.PRODUCT_FILE_EXTENSION);

			driveInfo = new DriveInfo(serverFile);

			if (driveInfo.IsReady)
			{
				if (!MPIFile.CopyFile(serverFile, localFile))
				{
					Console.WriteLine("[MESProcess], OpenFileAndParse(), Copy File Error" + serverFile);

					this._describe.AppendLine("Copy File Error:" + serverFile);

					return EErrorCode.MES_LoadTaskError;
				}
			}
			else
			{
				Console.WriteLine("[MESProcess], OpenFileAndParse(), drive is not Ready:" + serverFile);

				this._describe.AppendLine("drive is not Ready:" + serverFile);

				return EErrorCode.MES_LoadTaskError;
			}

			//CONFIGFILE_NAME4(*.gcg File = *.map File)
			serverFile = Path.Combine(uiSetting.MESPath2, file[1][6]);

			localFile = Path.Combine(uiSetting.ProductPath, Path.GetFileNameWithoutExtension(serverFile) + "." + Constants.Files.MAPDATA_FILE_EXTENSION);

			driveInfo = new DriveInfo(serverFile);

			if (driveInfo.IsReady)
			{
				if (!MPIFile.CopyFile(serverFile, localFile))
			{
					Console.WriteLine("[MESProcess], OpenFileAndParse(), Copy File Error" + serverFile);

					this._describe.AppendLine("Copy File Error:" + serverFile);

					return EErrorCode.MES_LoadTaskError;
				}
			}
			else
			{
				Console.WriteLine("[MESProcess], OpenFileAndParse(), !driveInfo.IsReady:" + serverFile);

				this._describe.AppendLine("drive is not Ready:" + serverFile);

				return EErrorCode.MES_LoadTaskError;
			}

			uiSetting.WaferNumber = fileNameWithoutExtension;

			//PROBE_TYPE
			uiSetting.WeiminUIData.Remark01 = file[1][0];

			//JOB_IN_DATETIME
			uiSetting.WeiminUIData.Remark02 = file[1][2];

			//CONFIGFILE_NAME3(*.eeg File = Remark2)
			uiSetting.WeiminUIData.Remark03 = file[1][5];

			//JOB_IN_USER
			uiSetting.OperatorName = file[1][7];

			//MACHINE_NO
			uiSetting.MachineName = file[1][8];

			//MPI_PRD
			uiSetting.ProberRecipeName = file[1][9];

			GlobalData.ProberRecipeName = file[1][9];

			try
			{
				TaskSheetCtrl taskSheetCtrl = new TaskSheetCtrl();

				taskSheetCtrl.FileName = Path.GetFileNameWithoutExtension(file[1][4]);

				taskSheetCtrl.SetProduct(Path.GetFileNameWithoutExtension(file[1][4]), uiSetting.ProductPath);

				taskSheetCtrl.SetBinData(Path.GetFileNameWithoutExtension(file[1][4]), uiSetting.ProductPath);

				taskSheetCtrl.SetMapData(Path.GetFileNameWithoutExtension(file[1][6]), uiSetting.ProductPath);

				taskSheetCtrl.SetImportCalibrateData(Path.GetFileNameWithoutExtension(file[1][3]), uiSetting.ProductPath);

				taskSheetCtrl.SetImportBin("", uiSetting.ProductPath);

				taskSheetCtrl.SetImportSptCoeff("", uiSetting.ProductPath);

				taskSheetCtrl.CreateTaskSheet(Path.Combine(uiSetting.ProductPath, Path.GetFileNameWithoutExtension(file[1][4]) + "." + Constants.Files.TASK_SHEET_EXTENSION));

				MPI.Tester.Data.SmartBinning bin = new SmartBinning();

				bin.Save(Path.Combine(uiSetting.ProductPath, Path.GetFileNameWithoutExtension((file[1][4]))) + "." + Constants.Files.BIN_FILE_EXTENSION);
			}
			catch(Exception e)
			{
				Console.WriteLine("[MESProcess], OpenFileAndParse(), Create task file fail:" + e.ToString());

				this._describe.AppendLine("Create task file fail");

				return EErrorCode.MES_LoadTaskError;
			}

			uiSetting.TaskSheetFileName = Path.GetFileNameWithoutExtension(file[1][4]);

			if (isDeleteAMIFile)
			{
				string backFile = Path.Combine(uiSetting.MESBackupPath, Path.GetFileName(serverConditonFullPath));

				MPIFile.CopyFile(serverConditonFullPath, backFile);

				MPIFile.DeleteFile(serverConditonFullPath);
			}

			return EErrorCode.NONE;
		}

		protected override Tester.Data.EErrorCode ConverterToMPIFormat()
		{
			return EErrorCode.NONE;
		}

		protected override Tester.Data.EErrorCode SaveRecipeToFile()
		{
			return EErrorCode.NONE;
		}

		public class TaskSheetCtrl
		{
			#region >>> private Property <<<

			private object _lockObj;
			private string _fileName;
			private Dictionary<string, TaskSheet> _taskSheetDic;

			#endregion

			#region >>> Constructor / Disposor <<<

			public TaskSheetCtrl()
			{
				this._lockObj = new object();

				this._fileName = "";
				this._taskSheetDic = new Dictionary<string, TaskSheet>();
				this._taskSheetDic.Add("Product", new TaskSheet("Product"));
				this._taskSheetDic.Add("BinData", new TaskSheet("BinData"));
				this._taskSheetDic.Add("MapData", new TaskSheet("MapData"));
				this._taskSheetDic.Add("ImportCalibrateData", new TaskSheet("ImportCalibrateData"));
				this._taskSheetDic.Add("ImportBin", new TaskSheet("ImportBin"));
				this._taskSheetDic.Add("ImportSptCoeff", new TaskSheet("ImportSptCoeff"));
			}

			#endregion

			#region >>> Private Method <<<

			private TaskSheet DeepClone(TaskSheet taskSheet)
			{
				TaskSheet ts = new TaskSheet(taskSheet.Type);
				ts.Name = taskSheet.Name;
				ts.Ext = taskSheet.Ext;
				ts.Path = taskSheet.Path;

				return ts;
			}

			private TaskSheet GetProduct()
			{
				return this.DeepClone(this._taskSheetDic["Product"]);
			}

			private TaskSheet GetBinData()
			{
				return this.DeepClone(this._taskSheetDic["BinData"]);
			}

			private TaskSheet GetMapData()
			{
				return this.DeepClone(this._taskSheetDic["MapData"]);
			}

			private TaskSheet GetImportCalibrateData()
			{
				return this.DeepClone(this._taskSheetDic["ImportCalibrateData"]);
			}

			private TaskSheet GetImportBin()
			{
				return this.DeepClone(this._taskSheetDic["ImportBin"]);
			}

			private TaskSheet GetImportSptCoeff()
			{
				return this.DeepClone(this._taskSheetDic["ImportSptCoeff"]);
			}

			#endregion

			#region >>> public Method <<<

			public void SetProduct(string name, string path)
			{
				this._taskSheetDic["Product"].Name = name;
				this._taskSheetDic["Product"].Ext = Constants.Files.PRODUCT_FILE_EXTENSION;
				this._taskSheetDic["Product"].Path = path;
			}

			public void SetBinData(string name, string path)
			{
				this._taskSheetDic["BinData"].Name = name;
				this._taskSheetDic["BinData"].Ext = Constants.Files.BIN_FILE_EXTENSION;
				this._taskSheetDic["BinData"].Path = path;
			}

			public void SetMapData(string name, string path)
			{
				this._taskSheetDic["MapData"].Name = name;
				this._taskSheetDic["MapData"].Ext = Constants.Files.MAPDATA_FILE_EXTENSION;
				this._taskSheetDic["MapData"].Path = path;
			}

			public void SetImportCalibrateData(string name, string path)
			{
				this._taskSheetDic["ImportCalibrateData"].Name = name;
				this._taskSheetDic["ImportCalibrateData"].Ext = "cal";
				this._taskSheetDic["ImportCalibrateData"].Path = path;
			}

			public void SetImportBin(string name, string path)
			{
				this._taskSheetDic["ImportBin"].Name = name;
				this._taskSheetDic["ImportBin"].Ext = "sr2";
				this._taskSheetDic["ImportBin"].Path = path;
			}

			public void SetImportSptCoeff(string name, string path)
			{
				this._taskSheetDic["ImportSptCoeff"].Name = name;
				this._taskSheetDic["ImportSptCoeff"].Ext = "spt";
				this._taskSheetDic["ImportSptCoeff"].Path = path;
			}

			public void CreateTaskSheet(string pathAndFile)
			{
				XmlDocument xmlDoc = new XmlDocument();

				string str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
				 "<?xml-stylesheet type=\"text/xsl\"" +
				 " href=\"FormatA.xslt\"?><TaskSheet></TaskSheet>";

				xmlDoc.LoadXml(str);
				XmlElement root = xmlDoc.DocumentElement;

				//----------------------------------------------
				// (1) Product File
				//----------------------------------------------
				XmlElement itemElem = xmlDoc.CreateElement("FileInfo");
				root.AppendChild(itemElem);

				XmlElement itemData = xmlDoc.CreateElement("Type");
				itemData.InnerText = "Product";
				itemElem.AppendChild(itemData);

				TaskSheet ts = this.GetProduct();

				itemData = xmlDoc.CreateElement("Name");
				itemData.InnerText = ts.Name;
				itemElem.AppendChild(itemData);

				itemData = xmlDoc.CreateElement("Ext");
				itemData.InnerText = ts.Ext;
				itemElem.AppendChild(itemData);

				itemData = xmlDoc.CreateElement("Path");
				itemData.InnerText = ts.Path;
				itemElem.AppendChild(itemData);

				//----------------------------------------------
				// (2) Bin Data File
				//----------------------------------------------
				itemElem = xmlDoc.CreateElement("FileInfo");
				root.AppendChild(itemElem);

				itemData = xmlDoc.CreateElement("Type");
				itemData.InnerText = "BinData";
				itemElem.AppendChild(itemData);

				ts = this.GetBinData();

				itemData = xmlDoc.CreateElement("Name");
				itemData.InnerText = ts.Name;
				itemElem.AppendChild(itemData);

				itemData = xmlDoc.CreateElement("Ext");
				itemData.InnerText = ts.Ext;
				itemElem.AppendChild(itemData);

				itemData = xmlDoc.CreateElement("Path");
				itemData.InnerText = ts.Path;
				itemElem.AppendChild(itemData);

				//----------------------------------------------
				// (3) Map Data File
				//----------------------------------------------
				itemElem = xmlDoc.CreateElement("FileInfo");
				//itemElem.SetAttribute("Type", "Bin");
				root.AppendChild(itemElem);

				itemData = xmlDoc.CreateElement("Type");
				itemData.InnerText = "MapData";
				itemElem.AppendChild(itemData);

				ts = this.GetMapData();

				itemData = xmlDoc.CreateElement("Name");
				itemData.InnerText = ts.Name;
				itemElem.AppendChild(itemData);

				itemData = xmlDoc.CreateElement("Ext");
				itemData.InnerText = ts.Ext;
				itemElem.AppendChild(itemData);

				itemData = xmlDoc.CreateElement("Path");
				itemData.InnerText = ts.Path;
				itemElem.AppendChild(itemData);

				//----------------------------------------------
				// (4) Import Calibrate Data
				//----------------------------------------------
				itemElem = xmlDoc.CreateElement("FileInfo");
				root.AppendChild(itemElem);

				itemData = xmlDoc.CreateElement("Type");
				itemData.InnerText = "ImportCalibrateData";
				itemElem.AppendChild(itemData);

				ts = this.GetImportCalibrateData();

				itemData = xmlDoc.CreateElement("Name");
				itemData.InnerText = ts.Name;
				itemElem.AppendChild(itemData);

				itemData = xmlDoc.CreateElement("Ext");
				itemData.InnerText = ts.Ext;
				itemElem.AppendChild(itemData);

				itemData = xmlDoc.CreateElement("Path");
				itemData.InnerText = ts.Path;
				itemElem.AppendChild(itemData);

				//----------------------------------------------
				// (5) Import Bin Table
				//----------------------------------------------
				itemElem = xmlDoc.CreateElement("FileInfo");
				//itemElem.SetAttribute("Type", "Bin");
				root.AppendChild(itemElem);

				itemData = xmlDoc.CreateElement("Type");
				itemData.InnerText = "ImportBin";
				itemElem.AppendChild(itemData);

				ts = this.GetImportBin();

				itemData = xmlDoc.CreateElement("Name");
				itemData.InnerText = ts.Name;
				itemElem.AppendChild(itemData);

				itemData = xmlDoc.CreateElement("Ext");
				itemData.InnerText = ts.Ext;
				itemElem.AppendChild(itemData);

				itemData = xmlDoc.CreateElement("Path");
				itemData.InnerText = ts.Path;
				itemElem.AppendChild(itemData);

				//----------------------------------------------
				// (6) Import Import CalibSptData
				//----------------------------------------------
				itemElem = xmlDoc.CreateElement("FileInfo");
				//itemElem.SetAttribute("Type", "Bin");
				root.AppendChild(itemElem);

				itemData = xmlDoc.CreateElement("Type");
				itemData.InnerText = "ImportSptCoeff";
				itemElem.AppendChild(itemData);

				ts = this.GetImportSptCoeff();

				itemData = xmlDoc.CreateElement("Name");
				itemData.InnerText = ts.Name;
				itemElem.AppendChild(itemData);

				itemData = xmlDoc.CreateElement("Ext");
				itemData.InnerText = ts.Ext;
				itemElem.AppendChild(itemData);

				itemData = xmlDoc.CreateElement("Path");
				itemData.InnerText = ts.Path;
				itemElem.AppendChild(itemData);

				if (Directory.Exists(Constants.Paths.MES_FILE_PATH) == false)
				{
					Directory.CreateDirectory(Constants.Paths.MES_FILE_PATH);
				}

				xmlDoc.Save(pathAndFile);
			}

			#endregion

			#region >>> public Property <<<

			public string FileName
			{
				get { return this._fileName; }
				set { lock (this._lockObj) { this._fileName = value; } }
			}

			#endregion

		}
	}
}
