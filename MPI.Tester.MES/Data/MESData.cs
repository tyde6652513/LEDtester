using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.MES;
using MPI.Tester.Data;
using System.Xml;
using System.IO;

namespace MPI.Tester.MES.Data
{
	public class TaskSheet
	{
		#region >>> private Property <<<

		private object _lockObj;
		private string _type;
		private string _name;
		private string _ext;
		private string _path;

		#endregion

		#region >>> Constructor / Disposor <<<

		private TaskSheet()
		{
			this._lockObj = new object();
			this._type = "";
			this._name = "";
			this._ext = "";
			this._path = "";
		}

		public TaskSheet(string type)
			: this()
		{
			this._type = type;
		}

		#endregion

		#region >>> public Property <<<

		public string Type
		{
			get { return this._type; }
			//set { lock (this._lockObj) { this._type = value; } }
		}

		public string Name
		{
			get { return this._name; }
			set { lock (this._lockObj) { this._name = value; } }
		}

		public string Ext
		{
			get { return this._ext; }
			set { lock (this._lockObj) { this._ext = value; } }
		}

		public string Path
		{
			get { return this._path; }
			set { lock (this._lockObj) { this._path = value; } }
		}

		#endregion
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

		public void CreateTaskSheet()
		{
			XmlDocument xmlDoc = new XmlDocument();

			string fileNameWithExt = this._fileName + "." + Constants.Files.TASK_SHEET_EXTENSION;
			string pathAndFile = Path.Combine(Constants.Paths.MES_FILE_PATH, fileNameWithExt);
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

        public void CreateTaskSheet(string path)
        {
            XmlDocument xmlDoc = new XmlDocument();

            string fileNameWithExt = this._fileName + "." + Constants.Files.TASK_SHEET_EXTENSION;
            string pathAndFile = Path.Combine(path, fileNameWithExt);

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

            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
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
