using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Linq;

using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Gui   
{
	public class Storage
	{
		private object _lockObj;

		private XmlDocument _xmlDoc;
        private XmlNode _xmlDocBodyHeader;
		private string _xslFileName;
        private Dictionary<string, string> _statisticsData;
        private int _seperateSize;
        private Thread _runThread;
        private Thread _cmdThread;
        private bool _runThreadSuccess;
		private frmProgressBar _frmProgressBar;
        private frmReportComments _frmReportComments;
        private const bool OLD_VERSION_BODYDATALIST = true;

        public Storage()
        {
            this._lockObj = new object();

            this._xslFileName = string.Empty;
            this._xmlDoc = null;
            this._statisticsData = new Dictionary<string, string>();
            this._seperateSize = 50000;
            this._runThread = null;
            this._cmdThread = null;
            this._runThreadSuccess = false;
			MES.ProcessBase.SaveReportHead += new EventHandler(this.SaveReportHeadToFile);
        }

		#region >>> Private Method <<<

		private string GetHeader(int idx)
        {
            string sHeader = "";
            int idx1 = 0;
            int idx2 = 0;
            do
            {
                idx1 = idx % 26;
                sHeader = Convert.ToChar('A' + idx1) + sHeader;
                idx2 = idx / 26;
                idx = idx2 - 1;
            } while (idx2 >= 1);
            return sHeader;
        }

		private bool AppendUserDataToXml()
		{
			Console.WriteLine("[Storage], AppendUserDataToXml()");

			if (this._xmlDoc == null)
			{
				this._xmlDoc = new XmlDocument();
			}

			string pathAndFileWithExt = string.Format("{0}{1}", "User", ((int)DataCenter._uiSetting.UserID).ToString("0000")) + ".xml";
			string pathAndFile = Path.Combine(Constants.Paths.USER_DIR, pathAndFileWithExt);

			XmlDocument xmlUser = new XmlDocument();

			if (File.Exists(pathAndFile) == false)
				return false;

			xmlUser.Load(pathAndFile);

			XmlElement user = (xmlUser.SelectSingleNode("/UserDefine") as XmlElement);

			XmlNode rootNode = this._xmlDoc.DocumentElement;

			//----------------------------------------------------------
			// Append "UserData" element to XML
			//----------------------------------------------------------
			XmlNode userDataNode = xmlUser.SelectSingleNode("/UserDefine/UserData");
			XmlNode importedUserDataNode = this._xmlDoc.ImportNode(userDataNode, true);
			((XmlElement)importedUserDataNode).SetAttribute("id", user.SelectSingleNode("/UserDefine").Attributes["id"].Value);
			((XmlElement)importedUserDataNode).SetAttribute("version", user.SelectSingleNode("/UserDefine").Attributes["version"].Value);
			rootNode.AppendChild(importedUserDataNode);

			//----------------------------------------------------------
			// Append "Format" element to XML
			//----------------------------------------------------------
			XmlNodeList nodes = xmlUser.SelectNodes("/UserDefine/Formats/*");
		

           	//----------------------------------------------------------
			// By Recipe 選擇輸出報表格式
			//----------------------------------------------------------

            string formatName = DataCenter._uiSetting.FormatName;

            if (DataCenter._uiSetting.IsTestResultPathByTaskSheet)
            {
                bool isMatch=false;

                foreach (string s in DataCenter._uiSetting.UserDefinedData.FormatNames)
                {
                    if (s == DataCenter._product.OutputFileFormat)
                    {
                        isMatch = true;
                    }
                }

                if (isMatch)
                {
                    formatName = DataCenter._product.OutputFileFormat;
                }
            }

			//string xslFileName = string.Empty;
			foreach (XmlNode node in nodes)
			{
				if ((node as XmlElement).GetAttribute("name") == formatName)
				{
					//(rootNode as XmlElement).SetAttribute("format", formatName);
					XmlNode importedNode = this._xmlDoc.ImportNode(node, true);
					this._xslFileName = (importedNode as XmlElement).GetAttribute("file");
					rootNode.AppendChild(importedNode);
					break;
				}
			}
			return true;
		}

        private bool WriteReportHead()
        {
			Console.WriteLine("[Storage], WriteReportHead()");

			if ( this._xmlDoc == null )
				return false;
			
			//--------------------------------------------------------------------------------
			// Append "Setting" element to XML
			//--------------------------------------------------------------------------------
			XmlNode rootNode = this._xmlDoc.DocumentElement;

            //--------------------------------------------------------------------------------
            // Append "ItemData" element to XML
			//--------------------------------------------------------------------------------
            XmlElement itemDataElem = this._xmlDoc.CreateElement("ItemData");
            rootNode.AppendChild(itemDataElem);

			XmlElement itemData = this._xmlDoc.CreateElement("TesterModel");
			itemData.InnerText = DataCenter._machineInfo.TesterModel;
            itemDataElem.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("TesterSN");
			itemData.InnerText = DataCenter._machineInfo.TesterSN;
            itemDataElem.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("SourceMeterSN");
			itemData.InnerText = DataCenter._machineInfo.SourceMeterSN;
            itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("MachineName");
			itemData.InnerText = DataCenter._uiSetting.MachineName;
			itemDataElem.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("LotNumber");
			itemData.InnerText = DataCenter._uiSetting.LotNumber;
            itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("WaferNumber");
			itemData.InnerText = DataCenter._uiSetting.WaferNumber;
			itemDataElem.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("OperatorName");
			itemData.InnerText = DataCenter._uiSetting.OperatorName;
            itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("ReporterName");
			itemData.InnerText = DataCenter._uiSetting.ReporterName;
			itemDataElem.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("ProductType");
            itemData.InnerText = DataCenter._uiSetting.ProductType;
            itemDataElem.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("Barcode");
            itemData.InnerText = DataCenter._uiSetting.Barcode;
            itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("Substrate");
			itemData.InnerText = DataCenter._uiSetting.Substrate;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("LoginID");
			itemData.InnerText = DataCenter._uiSetting.LoginID;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("ProductPath");
			itemData.InnerText = DataCenter._uiSetting.ProductPath;
			itemDataElem.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("TaskSheetFileName");
            itemData.InnerText = DataCenter._uiSetting.TaskSheetFileName;
            itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("ProductFileName");
			itemData.InnerText = DataCenter._uiSetting.ProductFileName + "." + Constants.Files.PRODUCT_FILE_EXTENSION;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("MapDataFileName");
			itemData.InnerText = DataCenter._uiSetting.MapDataFileName + "." + Constants.Files.MAPDATA_FILE_EXTENSION;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("ConditionFileName");
			itemData.InnerText = DataCenter._uiSetting.ConditionFileName + "." + Constants.Files.CONDITION_FILE_EXTENSION;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("BinFileName");
			itemData.InnerText = DataCenter._uiSetting.BinDataFileName + "." + Constants.Files.BIN_FILE_EXTENSION;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("TestResultFileName");
			itemData.InnerText = DataCenter._uiSetting.TestResultFileName;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("TestResultFileExt");
			itemData.InnerText = DataCenter._uiSetting.TestResultFileExt;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("ProductName");
			itemData.InnerText = DataCenter._uiSetting.ProductName;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("ImportCalibrateFileName");
			itemData.InnerText = DataCenter._uiSetting.ImportCalibrateFileName;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("ImportBinFileName");
			itemData.InnerText = DataCenter._uiSetting.ImportBinFileName;
			itemDataElem.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("WaferPcs");
            itemData.InnerText = DataCenter._uiSetting.WaferPcs.ToString("00");
            itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("ProberRecipeName");
			itemData.InnerText = DataCenter._uiSetting.ProberRecipeName;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("ProbingCount1");
			itemData.InnerText = DataCenter._uiSetting.ProbingCount1.ToString();
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("ProbingCount2");
			itemData.InnerText = DataCenter._uiSetting.ProbingCount2.ToString();
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("ProbingCount3");
			itemData.InnerText = DataCenter._uiSetting.ProbingCount3.ToString();
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("ProbingCount4");
			itemData.InnerText = DataCenter._uiSetting.ProbingCount4.ToString();
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("TotalSacnCounts");
			itemData.InnerText = DataCenter._uiSetting.TotalSacnCounts.ToString();
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("TestResultPath01");
			itemData.InnerText = DataCenter._uiSetting.TestResultPath01;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("TestResultPath02");
			itemData.InnerText = DataCenter._uiSetting.TestResultPath02;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("TestResultPath03");
			itemData.InnerText = DataCenter._uiSetting.TestResultPath03;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("IsEnablePath01");
			itemData.InnerText = DataCenter._uiSetting.IsEnablePath01.ToString();
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("IsEnablePath02");
			itemData.InnerText = DataCenter._uiSetting.IsEnablePath02.ToString();
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("IsEnablePath03");
			itemData.InnerText = DataCenter._uiSetting.IsEnablePath03.ToString();
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("ManualOutputPath01");
			itemData.InnerText = DataCenter._uiSetting.ManualOutputPath01;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("ManualOutputPath02");
			itemData.InnerText = DataCenter._uiSetting.ManualOutputPath02;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("ManualOutputPath03");
			itemData.InnerText = DataCenter._uiSetting.ManualOutputPath03;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("IsEnableManualPath01");
			itemData.InnerText = DataCenter._uiSetting.IsEnableManualPath01.ToString();
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("IsEnableManualPath02");
			itemData.InnerText = DataCenter._uiSetting.IsEnableManualPath02.ToString();
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("IsEnableManualPath03");
			itemData.InnerText = DataCenter._uiSetting.IsEnableManualPath03.ToString();
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("WAFOutputPath01");
			itemData.InnerText = DataCenter._uiSetting.WAFOutputPath01;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("WAFOutputPath02");
			itemData.InnerText = DataCenter._uiSetting.WAFOutputPath02;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("WAFOutputPath03");
			itemData.InnerText = DataCenter._uiSetting.WAFOutputPath03;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("IsEnableWAFPath01");
			itemData.InnerText = DataCenter._uiSetting.IsEnableWAFPath01.ToString();
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("IsEnableWAFPath02");
			itemData.InnerText = DataCenter._uiSetting.IsEnableWAFPath02.ToString();
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("IsEnableWAFPath03");
			itemData.InnerText = DataCenter._uiSetting.IsEnableWAFPath03.ToString();
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("STATOutputPath01");
			itemData.InnerText = DataCenter._uiSetting.STATOutputPath01;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("STATOutputPath02");
			itemData.InnerText = DataCenter._uiSetting.STATOutputPath02;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("STATOutputPath03");
			itemData.InnerText = DataCenter._uiSetting.STATOutputPath03;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("IsEnableSTATPath01");
			itemData.InnerText = DataCenter._uiSetting.IsEnableSTATPath01.ToString();
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("IsEnableSTATPath02");
			itemData.InnerText = DataCenter._uiSetting.IsEnableSTATPath02.ToString();
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("IsEnableSTATPath03");
			itemData.InnerText = DataCenter._uiSetting.IsEnableSTATPath03.ToString();
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("StartTestTime");
			itemData.SetAttribute("year", DataCenter._sysSetting.StartTestTime.Year.ToString());
			itemData.SetAttribute("month", DataCenter._sysSetting.StartTestTime.Month.ToString());
			itemData.SetAttribute("day", DataCenter._sysSetting.StartTestTime.Day.ToString());
			itemData.SetAttribute("hour", DataCenter._sysSetting.StartTestTime.Hour.ToString());
			itemData.SetAttribute("hour12", DataCenter._sysSetting.StartTestTime.ToString("tt hh"));
			itemData.SetAttribute("minute", DataCenter._sysSetting.StartTestTime.Minute.ToString());
			itemData.SetAttribute("second", DataCenter._sysSetting.StartTestTime.Second.ToString());
			itemData.InnerText = DataCenter._sysSetting.StartTestTime.ToString();
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("EndTestTime");
			itemData.SetAttribute("year", DataCenter._sysSetting.EndTestTime.Year.ToString());
			itemData.SetAttribute("month", DataCenter._sysSetting.EndTestTime.Month.ToString());
			itemData.SetAttribute("day", DataCenter._sysSetting.EndTestTime.Day.ToString());
			itemData.SetAttribute("hour", DataCenter._sysSetting.EndTestTime.Hour.ToString());
			itemData.SetAttribute("hour12", DataCenter._sysSetting.EndTestTime.ToString("tt hh"));
			itemData.SetAttribute("minute", DataCenter._sysSetting.EndTestTime.Minute.ToString());
			itemData.SetAttribute("second", DataCenter._sysSetting.EndTestTime.Second.ToString());
			itemData.InnerText = DataCenter._sysSetting.EndTestTime.ToString();
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("SpanTestTime");
			TimeSpan span;
			if (DataCenter._sysSetting.EndTestTime > DataCenter._sysSetting.StartTestTime)
			{
				span = DataCenter._sysSetting.EndTestTime - DataCenter._sysSetting.StartTestTime;
			}
			else
			{
                if (DataCenter._sysSetting.StartTestTime.Year < 2010)
                {
                    DataCenter._sysSetting.StartTestTime = DateTime.Now;
                }
                
                span = DateTime.Now - DataCenter._sysSetting.StartTestTime;
			}
			itemData.SetAttribute("day", span.Days.ToString());
			itemData.SetAttribute("hour", span.Hours.ToString());
			itemData.SetAttribute("minute", span.Minutes.ToString());
			itemData.SetAttribute("second", span.Seconds.ToString());
			itemData.InnerText = Convert.ToInt32(span.TotalSeconds).ToString();
			itemDataElem.AppendChild(itemData);


			itemData = this._xmlDoc.CreateElement("IsSingleLOPItem");
			itemData.InnerText = Convert.ToInt32(DataCenter._product.IsSingleLOPItem).ToString();
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("LOPSaveItem");
			itemData.InnerText = DataCenter._product.LOPSaveItem.ToString();
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("CalByWave");
			itemData.InnerText = DataCenter._product.TestCondition.CalByWave.ToString();
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("FilterWheelPos");			
			itemData.InnerText = (DataCenter._product.ProductFilterWheelPos + 1).ToString();
            itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("UserID");
			itemData.InnerText = DataCenter._uiSetting.UserDefinedData.ID.ToString("0000");
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("UserDataVer");
			itemData.InnerText = DataCenter._uiSetting.UserDefinedData.Version;
			itemDataElem.AppendChild(itemData);

            string TestCount = DataCenter._acquireData.ChipInfo.TestCount.ToString();
			string GoodDieCount = DataCenter._acquireData.ChipInfo.GoodDieCount.ToString();
			string FailDieCount = DataCenter._acquireData.ChipInfo.FailDieCount.ToString();
			string GoodRate = DataCenter._acquireData.ChipInfo.GoodRate.ToString("00.00");

            // Add By Alec 2013.02.01 //
            itemData = this._xmlDoc.CreateElement("TotalScanCounts");
            itemData.InnerText = DataCenter._uiSetting.TotalSacnCounts.ToString();
            itemDataElem.AppendChild(itemData);
            // Add By Alec 2013.02.01 END//

			itemData = this._xmlDoc.CreateElement("TestCount");
			itemData.InnerText = TestCount;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("GoodDieCount");
			itemData.InnerText = GoodDieCount;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("FailDieCount");
			itemData.InnerText = FailDieCount;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("GoodRate");
			itemData.InnerText = GoodRate;
			itemDataElem.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("ReportComments");
            itemData.InnerText = DataCenter._uiSetting.ReportComments;
            itemDataElem.AppendChild(itemData);

            //--------------------------------------------------------------------------------
            // Weimin format Data
            //--------------------------------------------------------------------------------
			itemData = this._xmlDoc.CreateElement("WMProductName");
			itemData.InnerText = DataCenter._uiSetting.WeiminUIData.ProductName;
			itemDataElem.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("WMDeviceNumber");
            itemData.InnerText = DataCenter._uiSetting.WeiminUIData.DeviceNumber;
            itemDataElem.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("WMSpecification");
            DataCenter._uiSetting.WeiminUIData.Specification = DataCenter._uiSetting.TaskSheetFileName;
            itemData.InnerText = DataCenter._uiSetting.WeiminUIData.Specification;
            itemDataElem.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("WMSpecificationRemark");
            itemData.InnerText = DataCenter._uiSetting.WeiminUIData.SpecificationRemark;
            itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("WMSampleBins");
			itemData.InnerText = DataCenter._uiSetting.WeiminUIData.SampleBins;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("WMSampleStandard");
			itemData.InnerText = DataCenter._uiSetting.WeiminUIData.SampleStandard;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("WMSampleLevel");
			itemData.InnerText = DataCenter._uiSetting.WeiminUIData.SampleLevel;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("WMTotalTested");
			itemData.InnerText = DataCenter._uiSetting.WeiminUIData.TotalTested;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("WMSamples");
			itemData.InnerText = DataCenter._uiSetting.WeiminUIData.Samples;
			itemDataElem.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("WMRemark01");
            itemData.InnerText = DataCenter._uiSetting.WeiminUIData.Remark01;
            itemDataElem.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("WMRemark02");
            itemData.InnerText = DataCenter._uiSetting.WeiminUIData.Remark02;
            itemDataElem.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("WMRemark03");
            itemData.InnerText = DataCenter._uiSetting.WeiminUIData.Remark03;
            itemDataElem.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("WMRemark04");
            itemData.InnerText = DataCenter._uiSetting.WeiminUIData.Remark04;
            itemDataElem.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("WMCustomerID");
            itemData.InnerText = DataCenter._uiSetting.WeiminUIData.CustomerID;
            itemDataElem.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("WMCustomer");
            itemData.InnerText = DataCenter._uiSetting.WeiminUIData.Customer;
            itemDataElem.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("WMCustomerNote01");
            itemData.InnerText = DataCenter._uiSetting.WeiminUIData.CustomerNote01;
            itemDataElem.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("WMCustomerNote02");
            itemData.InnerText = DataCenter._uiSetting.WeiminUIData.CustomerNote02;
            itemDataElem.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("WMCustomerNote03");
            itemData.InnerText = DataCenter._uiSetting.WeiminUIData.CustomerNote03;
            itemDataElem.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("WMCustomerRemark01");
            itemData.InnerText = DataCenter._uiSetting.WeiminUIData.CustomerRemark01;
            itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("WMLotNumber");
			itemData.InnerText = DataCenter._uiSetting.WeiminUIData.LotNumber;
			itemDataElem.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("WMClassNumber");
            itemData.InnerText = DataCenter._uiSetting.WeiminUIData.ClassNumber;
            itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("WMCodeNumber");
			itemData.InnerText = DataCenter._uiSetting.WeiminUIData.CodeNumber;
			itemDataElem.AppendChild(itemData);

			itemData = this._xmlDoc.CreateElement("WMSerialNumber");
			itemData.InnerText = DataCenter._uiSetting.WeiminUIData.SerialNumber;
			itemDataElem.AppendChild(itemData);			

            itemData = this._xmlDoc.CreateElement("WMTestMode");
            itemData.InnerText = DataCenter._uiSetting.WeiminUIData.WMTestMode.ToString();
            itemDataElem.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("Target");
            itemData.InnerText = this.CalcTarget();
            itemDataElem.AppendChild(itemData);

            if (OLD_VERSION_BODYDATALIST)
            {
                bool isExistPassFailIndex = false;

                int passFailColIndex = -1;

                int index = 0;

                foreach (TestResultData data in DataCenter._acquireData.OutputTestResult)
                {
                    if (data.KeyName == "ISALLPASS")
                    {
                        passFailColIndex = index;

                        isExistPassFailIndex = true;

                        break;
                    }
                    index++;
                }

                if (isExistPassFailIndex)
                {
                    int passCount = 0;

                    int failCount = 0;

                    foreach (float[] data in AppSystem._bodyDataList)
                    {
                        if (data[passFailColIndex] == 1.0d)
                        {
                            passCount++;
                        }
                        else
                        {
                            failCount++;
                        }
                    }

                    itemData = this._xmlDoc.CreateElement("CalcGoodDieCount");

                    itemData.InnerText = passCount.ToString();

                    itemDataElem.AppendChild(itemData);

                    itemData = this._xmlDoc.CreateElement("CalcFailDieCount");

                    itemData.InnerText = failCount.ToString();

                    itemDataElem.AppendChild(itemData);
                }
            }
            else 
            {
                if (AppSystem._bodyDataDic.ContainsKey("ISALLPASS"))
                {
                    int passFailColIndex = AppSystem._bodyDataDic["ISALLPASS"][1];

                    int passCount = 0;

                    int failCount = 0;

                    foreach (float[] data in AppSystem._bodyDataList)
                    {
                        if (data[passFailColIndex] == 1.0d)
                        {
                            passCount++;
                        }
                        else
                        {
                            failCount++;
                        }
                    }

                    itemData = this._xmlDoc.CreateElement("CalcGoodDieCount");

                    itemData.InnerText = passCount.ToString();

                    itemDataElem.AppendChild(itemData);

                    itemData = this._xmlDoc.CreateElement("CalcFailDieCount");

                    itemData.InnerText = failCount.ToString();

                    itemDataElem.AppendChild(itemData);
                }
            }

            itemData = this._xmlDoc.CreateElement("DetectorFactor");
            itemData.InnerText = DataCenter._product.PdDetectorFactor.ToString();
            itemDataElem.AppendChild(itemData);

			//--------------------------------------------------------------------------------
            // Append "Tables/" element to XML
			//--------------------------------------------------------------------------------
            XmlElement tablesElem = this._xmlDoc.CreateElement("Tables");
            rootNode.AppendChild(tablesElem);

			//--------------------------------------------------------------------------------
			// Create "Table/TestItem/*" elements to XML
			//--------------------------------------------------------------------------------
			XmlElement testItemTable = this._xmlDoc.CreateElement("TestItem");
			tablesElem.AppendChild(testItemTable);

			//--------------------------------------------------------------------------------
			// Create "Table/ForceElec/*" elements to XML
			//--------------------------------------------------------------------------------
			XmlElement forceTable = this._xmlDoc.CreateElement("ForceElec");
			tablesElem.AppendChild(forceTable);

            //--------------------------------------------------------------------------------
            // Create "Table/EsdElec/*" elements to XML
            //--------------------------------------------------------------------------------
            XmlElement esdTable = this._xmlDoc.CreateElement("EsdElec");
            tablesElem.AppendChild(esdTable);

			//--------------------------------------------------------------------------------
			// Create "Table/GainOffset/*" elements to XML
			//--------------------------------------------------------------------------------
			XmlElement gainOffsetTable = this._xmlDoc.CreateElement("GainOffsetSetting");
			tablesElem.AppendChild(gainOffsetTable);

            //--------------------------------------------------------------------------------
            // Create "Table/MsrtItemInfo/*" elements to XML
            //--------------------------------------------------------------------------------
            XmlElement msrtItemInfoTable = this._xmlDoc.CreateElement("MsrtItemInfo");
            tablesElem.AppendChild(msrtItemInfoTable);

			//--------------------------------------------------------------------------------
			// Create "Table/CeofTables/*" elements to XML
			//--------------------------------------------------------------------------------
			XmlElement coefTables = this._xmlDoc.CreateElement("CoefTables");
			tablesElem.AppendChild(coefTables);

			int testItemIndex = 0;
			int forceElecIndex = 0;
			int gainOffsetIndex = 0;
			int coefIndex = 0;

			if (DataCenter._product.TestCondition.TestItemArray != null)
			{
				foreach (TestItemData data in DataCenter._product.TestCondition.TestItemArray)
				{
					//--------------------------------------
					// Test Item Data
					//--------------------------------------
					XmlElement testItemElem = this._xmlDoc.CreateElement(this.GetHeader(testItemIndex));
					testItemElem.InnerText = data.KeyName;
					testItemElem.SetAttribute("name", data.Name);
					testItemElem.SetAttribute("enable", Convert.ToInt32(data.IsEnable).ToString());
					testItemTable.AppendChild(testItemElem);
					testItemIndex++;

					//--------------------------------------
					// Force Electronic Data
					//--------------------------------------
					if (data.ElecSetting != null)
					{
						foreach (ElectSettingData set in data.ElecSetting)
						{
							XmlElement forceElem = this._xmlDoc.CreateElement(this.GetHeader(forceElecIndex));                        
							forceElem.InnerText = set.KeyName;
							forceElem.SetAttribute("name", set.Name);
							forceElem.SetAttribute("enable", Convert.ToInt32(data.IsEnable).ToString());
							forceElem.SetAttribute("unit", set.ForceUnit.ToString());
                            if (data.IsEnable == true)
                            {
                                forceElem.SetAttribute("value", Math.Abs(set.ForceValue).ToString());
                                forceElem.SetAttribute("time", set.ForceTime.ToString());
                            }
                            else
                            {
                                forceElem.SetAttribute("value", "0");
                                forceElem.SetAttribute("time", "0");
                            }				
							forceTable.AppendChild(forceElem);
							forceElecIndex++;
						}
					}

                    //--------------------------------------
					// ESD Setting Data
					//--------------------------------------
                    if (data is ESDTestItem)
                    {
                        XmlElement esdElem = this._xmlDoc.CreateElement(this.GetHeader(0));    // Only one Esd Setting
                        XmlElement esdElemZapList = this._xmlDoc.CreateElement("Value");
                        XmlElement esdElemPolar = this._xmlDoc.CreateElement("Polarity");
                        XmlElement esdElemJudgeList = this._xmlDoc.CreateElement("JudgeItem");

                        esdElem.InnerText = (data as ESDTestItem).EsdSetting.KeyName;
                        esdElem.SetAttribute("name", data.Name);
                        esdElem.SetAttribute("enable", Convert.ToInt32(data.IsEnable).ToString());
                        esdElem.SetAttribute("unit", "V");
                        esdElem.SetAttribute("mode", (data as ESDTestItem).EsdSetting.Mode.ToString());
                        esdElem.SetAttribute("gainVolt", (data as ESDTestItem).EsdSetting.GainVolt.ToString());
                        esdElem.SetAttribute("offsetVolt", (data as ESDTestItem).EsdSetting.OffsetVolt.ToString());
                        esdElem.SetAttribute("interval", (data as ESDTestItem).EsdSetting.IntervalTime.ToString());
                        esdElem.SetAttribute("count", (data as ESDTestItem).EsdSetting.Count.ToString());
                    }

					//--------------------------------------
					// Gain Offset Data
					//--------------------------------------
					if (data.GainOffsetSetting != null)
					{
						GainOffsetData[] gainOffsetdata = data.GainOffsetSetting;

						for (int i = 0; i < gainOffsetdata.Length; i++)
						{
							XmlElement gainOffsetElem = this._xmlDoc.CreateElement(this.GetHeader(gainOffsetIndex));
							gainOffsetElem.InnerText = gainOffsetdata[i].KeyName;
							gainOffsetElem.SetAttribute("name", gainOffsetdata[i].Name.ToString());
							gainOffsetElem.SetAttribute("Type", ((int)gainOffsetdata[i].Type).ToString());
							gainOffsetElem.SetAttribute("Square", gainOffsetdata[i].Square.ToString());
							gainOffsetElem.SetAttribute("Gain", gainOffsetdata[i].Gain.ToString());
							gainOffsetElem.SetAttribute("Offset", gainOffsetdata[i].Offset.ToString());
							gainOffsetTable.AppendChild(gainOffsetElem);
							gainOffsetIndex++;
						}
					}

                    //--------------------------------------
                    //  MsrtItemInfo include All Item
                    //--------------------------------------

                    if (data.GainOffsetSetting != null && data.ElecSetting != null)
                    {
                        TestResultData[] testResultdata = data.MsrtResult;
                        GainOffsetData[] gainOffsetdata = data.GainOffsetSetting;

                        for (int i = 0; i < testResultdata.Length; i++)
                        {
                            if (testResultdata[i].IsEnable != true)
                            {
                                continue;
                            }
      
                            XmlElement msrtItemElem = this._xmlDoc.CreateElement("A");
                            msrtItemElem.InnerText = testResultdata[i].KeyName;
                            msrtItemElem.SetAttribute("Name", testResultdata[i].Name.ToString());
                            msrtItemElem.SetAttribute("ForceIsEnable", Convert.ToInt32(data.IsEnable).ToString());
                            msrtItemElem.SetAttribute("ForceName", data.Name);
                            msrtItemElem.SetAttribute("ForceKeyName", data.KeyName);
                            msrtItemElem.SetAttribute("ForceItemOrder", data.Order.ToString());

                            // Electric Setting ForceValue ForceTime
                            int idx = 1;
                            foreach (ElectSettingData set in data.ElecSetting)
                            {
                                msrtItemElem.SetAttribute("ForceUnit_" + idx.ToString(), set.ForceUnit.ToString());
                                msrtItemElem.SetAttribute("ForceValue_" + idx.ToString(), Math.Abs(set.ForceValue).ToString());
                                msrtItemElem.SetAttribute("ForceTime_" + idx.ToString(), set.ForceTime.ToString());
                                msrtItemElem.SetAttribute("ForceCompliance_" + idx.ToString(), set.MsrtProtection.ToString());
                                msrtItemElem.SetAttribute("ForceComplianceUnit_" + idx.ToString(), set.MsrtUnit.ToString());
                                idx++;
                            }

                            if (data is LOPWLTestItem)
                            {
                                if ((data as LOPWLTestItem).OptiSetting.SensingMode == ESensingMode.Fixed)
                                {
                                    msrtItemElem.SetAttribute("ForceSptIntegralTime", (data as LOPWLTestItem).OptiSetting.FixIntegralTime.ToString());
                                }
                                else
                                {
                                    msrtItemElem.SetAttribute("ForceSptIntegralTime", (data as LOPWLTestItem).OptiSetting.LimitIntegralTime.ToString());
                                }
                            }
                            else
                            {
                                msrtItemElem.SetAttribute("ForceSptIntegralTime", "0");
                            }

                            msrtItemElem.SetAttribute("MsrtMax", testResultdata[i].MaxLimitValue.ToString());
                            msrtItemElem.SetAttribute("MsrtMin", testResultdata[i].MinLimitValue.ToString());
                            msrtItemElem.SetAttribute("MsrtIsVerify", Convert.ToInt32(testResultdata[i].IsVerify).ToString());
                            msrtItemElem.SetAttribute("MsrtIsEnable", Convert.ToInt32(testResultdata[i].IsEnable).ToString());
                            msrtItemElem.SetAttribute("MsrtUnit", testResultdata[i].Unit.ToString());
                         
                            for (int k= 0; k < gainOffsetdata.Length; k++)
                            {
                                if (gainOffsetdata[k].KeyName == testResultdata[i].KeyName)
                                {
                                    msrtItemElem.SetAttribute("MsrtType", ((int)gainOffsetdata[i].Type).ToString());
                                    msrtItemElem.SetAttribute("MsrtSquare", gainOffsetdata[i].Square.ToString());
                                    msrtItemElem.SetAttribute("MsrtGain", gainOffsetdata[i].Gain.ToString());
                                    msrtItemElem.SetAttribute("MsrtOffset", gainOffsetdata[i].Offset.ToString());
                                    break;
                                }
                            }
                            msrtItemInfoTable.AppendChild(msrtItemElem);
                        }
           
                    }

					//--------------------------------------
					// Coef. Data 
					//--------------------------------------
					if (data is LOPWLTestItem)
					{
						double[][] coefData = (data as LOPWLTestItem).CoefTable;
						double coefStep = ( coefData[1][0] - coefData[0][0] );

						if (  coefStep < 0.0d || coefStep == 0.0d )
							continue;

						int startIndex = (int) (( DataCenter._product.DispCoefStartWL - coefData[0][0] ) / coefStep);
						int endIndex = (int) (( DataCenter._product.DispCoefEndWL - coefData[0][0] ) / coefStep);

						if (startIndex > endIndex)
							continue;

						XmlElement coef = this._xmlDoc.CreateElement("Coef");
						coefTables.AppendChild(coef);

						coef.SetAttribute("num", coefIndex.ToString());
						coef.SetAttribute("keyname", data.KeyName);
						coef.SetAttribute("name", data.Name);
						coef.SetAttribute("StartWL", DataCenter._product.DispCoefStartWL.ToString());
						coef.SetAttribute("EndWL", DataCenter._product.DispCoefEndWL.ToString());

						for (int row = startIndex; row <= endIndex ; row++)
						{
							XmlElement coefRowElem = this._xmlDoc.CreateElement("R" + (row + 1 - startIndex).ToString("D3"));
							coefRowElem.InnerText = coefData[row][0].ToString();
							for (int k = 0; k < coefData[0].Length; k++)
							{
								coefRowElem.SetAttribute("W" + k.ToString(), coefData[row][k].ToString());
							}
							coef.AppendChild(coefRowElem);
						}
						coefIndex++;
					}
				}
			}			
			//--------------------------------------------------------------------------------
            // Create "Table/CoeffTable/*" elements to XML 
			// <Paul Add 201110117>
			//--------------------------------------------------------------------------------
            XmlElement coeffTable = this._xmlDoc.CreateElement("CoefTablePaul");
            tablesElem.AppendChild(coeffTable);

			coefIndex = 0;
            double[][] coefData2 = null;
			TestItemData[] testItem2 = DataCenter._product.TestCondition.TestItemArray;

			if (DataCenter._product.TestCondition.TestItemArray != null)
			{
				foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
				{
					if (item is LOPWLTestItem)
					{
						coefData2 = (item as LOPWLTestItem).CoefTable;

						int startWL = (int)DataCenter._product.DispCoefStartWL;
						int endWL = (int)DataCenter._product.DispCoefEndWL;

						startWL = startWL - 350;
						endWL = endWL - 350;

						if (startWL < 0)
						{
							startWL = 0;
						}

						//=================
						// paul add 2011-03-28 

						if (endWL > coefData2.Length)
						{
							endWL = coefData2.Length - 1;
						}

						for (int col = startWL; col < endWL + 1; col++)
						{
							XmlElement newElem3;
							newElem3 = this._xmlDoc.CreateElement(this.GetHeader(coefIndex));
							newElem3.InnerText = coefData2[col][0].ToString();
							for (int k = 0; k < coefData2[0].Length; k++)
							{
								newElem3.SetAttribute("W" + k.ToString(), coefData2[col][k].ToString());
							}
							coeffTable.AppendChild(newElem3);
							coefIndex++;
						}
					}
				}
			}

			//--------------------------------------------------------------------------------
			// Create "Table/CalibrateChip/*" elements to XML 
			// <Paul Add 201110117>
			//--------------------------------------------------------------------------------
			XmlElement calibrateChip = this._xmlDoc.CreateElement("CalibrateChip");

			tablesElem.AppendChild(calibrateChip);

			if (DataCenter._product.TestCondition.TestItemArray != null)
			{
				foreach (var chip in DataCenter._product.CaliChipValue.ChipList)
				{
					XmlElement chipElement = this._xmlDoc.CreateElement("Chip");

					calibrateChip.AppendChild(chipElement);

					foreach (var rawValue in chip.ValueList)
					{
						XmlElement rawElement = this._xmlDoc.CreateElement("Value");

						chipElement.AppendChild(rawElement);

						rawElement.InnerText = rawValue.KeyName;

						rawElement.SetAttribute("name", rawValue.Name);

						rawElement.SetAttribute("isEnable", rawValue.IsEnable.ToString());

						rawElement.SetAttribute("stdValue", rawValue.StdValue.ToString());

						rawElement.SetAttribute("msrtValue", rawValue.MsrtValue.ToString());

						//itemData = this._xmlDoc.CreateElement("IsEnable");

						//itemData.InnerText = rawValue.IsEnable.ToString();

						//rawElement.AppendChild(itemData);

						//itemData = this._xmlDoc.CreateElement("KeyName");

						//itemData.InnerText = rawValue.KeyName;

						//rawElement.AppendChild(itemData);

						//itemData = this._xmlDoc.CreateElement("Name");

						//itemData.InnerText = rawValue.Name;

						//rawElement.AppendChild(itemData);

						//itemData = this._xmlDoc.CreateElement("StdValue");

						//itemData.InnerText = rawValue.StdValue.ToString();

						//rawElement.AppendChild(itemData);

						//itemData = this._xmlDoc.CreateElement("MsrtValue");

						//itemData.InnerText = rawValue.MsrtValue.ToString();

						//rawElement.AppendChild(itemData);
					}
				}
			}

			//--------------------------------------------------------------------------------
			// Append "Binning" element to XML
			//--------------------------------------------------------------------------------
			XmlElement BinningElem = this._xmlDoc.CreateElement("Binning");

			rootNode.AppendChild(BinningElem);

			for (int i = 0; i < DataCenter._smartBinning.Count; i++)
			{
				SmartBinDataBase bin = DataCenter._smartBinning[i];

				XmlElement binDataElem = this._xmlDoc.CreateElement("Bin");
				BinningElem.AppendChild(binDataElem);

				itemData = this._xmlDoc.CreateElement("BinningType");
				itemData.InnerText = bin.BinningType.ToString();
				binDataElem.AppendChild(itemData);

				itemData = this._xmlDoc.CreateElement("BinCode");
				itemData.InnerText = bin.BinCode;
				binDataElem.AppendChild(itemData);

				itemData = this._xmlDoc.CreateElement("BinNumber");
				itemData.InnerText = bin.BinNumber.ToString();
				binDataElem.AppendChild(itemData);

				itemData = this._xmlDoc.CreateElement("BinCount");
				itemData.InnerText = bin.ChipCount.ToString();
				binDataElem.AppendChild(itemData);

				double rate = 0.0d;

				if (DataCenter._smartBinning.ChipCount != 0)
				{
					rate = ((double)bin.ChipCount * 100.0d) / (double)DataCenter._smartBinning.ChipCount;
				}

				itemData = this._xmlDoc.CreateElement("Rate");
				itemData.InnerText = rate.ToString();
				binDataElem.AppendChild(itemData);
			}

			return true;        
        }

		private void CreateBodyHeader(int headIndex, XmlNodeList resultItemList, XmlElement bodyHeaderElems)
		{
			if (DataCenter._uiSetting.IsEnableFloatReport)
			{
                TestResultData[] resultArray = DataCenter._acquireData.OutputTestResult.ToArray();

				for (int i = 0; i < resultArray.Length; i++)
				{
					bool isShow = true;

					foreach (var item in Enum.GetNames(typeof(MPI.Tester.Data.ESysResultItem)))
					{
						if (item == "TEST" || item == "BIN" || item == "POLAR" || item == "ISALLPASS")
						{
							continue;
						}

						if (resultArray[i].KeyName == item)
						{
							isShow = false;

							break;
						}
					}

                    foreach (var item in Enum.GetNames(typeof(MPI.Tester.Data.EProberDataIndex)))
					{
						if (item == "ROW" || item == "COL")
						{
							continue;
						}

						if (resultArray[i].KeyName == item)
						{
							isShow = false;

							break;
						}
					}

					if (!resultArray[i].IsEnable || !resultArray[i].IsVision || !isShow)
					{
						continue;
					}

					XmlElement headerElem = this._xmlDoc.CreateElement(this.GetHeader(headIndex));

					headerElem = this._xmlDoc.CreateElement(this.GetHeader(headIndex));

					headerElem.InnerText = resultArray[i].KeyName;

					headerElem.SetAttribute("name", resultArray[i].Name);

					headerElem.SetAttribute("enable", resultArray[i].IsEnable.ToString());

					headerElem.SetAttribute("unit", resultArray[i].Unit);

					headerElem.SetAttribute("format", resultArray[i].Formate);

					headerElem.SetAttribute("nullValue", "");

					headerElem.SetAttribute("index", "");

					headerElem.SetAttribute("min", "-99999");

					headerElem.SetAttribute("max", "99999");

					string itemName = UserData.ExtractKeyNameLetter(resultArray[i].KeyName);

					if (itemName == EOptiMsrtType.LOP.ToString() ||
							itemName == EOptiMsrtType.WATT.ToString() ||
							itemName == EOptiMsrtType.LM.ToString())
					{
						switch (DataCenter._product.LOPSaveItem)
						{
							case ELOPSaveItem.mcd:
								if (itemName == EOptiMsrtType.LOP.ToString())
								{
									bodyHeaderElems.AppendChild(headerElem);
									headIndex++;
								}
								break;
							//------------------------------------------------------
							case ELOPSaveItem.watt:
								if (itemName == EOptiMsrtType.WATT.ToString())
								{
									bodyHeaderElems.AppendChild(headerElem);
									headIndex++;
								}
								break;
							//------------------------------------------------------
							case ELOPSaveItem.lm:
								if (itemName == EOptiMsrtType.LM.ToString())
								{
									bodyHeaderElems.AppendChild(headerElem);
									headIndex++;
								}
								break;
							//------------------------------------------------------
							case ELOPSaveItem.mcd_lm:
								if (itemName == EOptiMsrtType.LM.ToString() || itemName == EOptiMsrtType.LOP.ToString())
								{
									bodyHeaderElems.AppendChild(headerElem);
									headIndex++;
								}
								break;
							//------------------------------------------------------
							case ELOPSaveItem.watt_lm:
								if (itemName == EOptiMsrtType.LM.ToString() || itemName == EOptiMsrtType.WATT.ToString())
								{
									bodyHeaderElems.AppendChild(headerElem);
									headIndex++;
								}
								break;
							//------------------------------------------------------
							case ELOPSaveItem.mcd_watt:
								if (itemName == EOptiMsrtType.LOP.ToString() || itemName == EOptiMsrtType.WATT.ToString())
								{
									bodyHeaderElems.AppendChild(headerElem);
									headIndex++;
								}
								break;
							//------------------------------------------------------
							case ELOPSaveItem.mcd_watt_lm:
								if (itemName == EOptiMsrtType.LM.ToString() || itemName == EOptiMsrtType.LOP.ToString() || itemName == EOptiMsrtType.WATT.ToString())
								{
									bodyHeaderElems.AppendChild(headerElem);
									headIndex++;
								}
								break;
							//------------------------------------------------------
							default:
								break;
						}
					}
					else
					{
						bodyHeaderElems.AppendChild(headerElem);

						headIndex++;
					}
				}
			}
			else
			{
			string itemName = string.Empty;
			XmlElement headerElem;
			XmlNode msrtNode = this._xmlDoc.DocumentElement.SelectSingleNode("//TesterOutput/Format/MsrtDisplayItem/*[position() = 1]");

			string xPath = "//TesterOutput/Format/MsrtDisplayItem/*[node()=";
			string xPathKey = string.Empty;

			//-----------------------------------------------------------------------------------------------------
			//	Create the "/Body/Header"  node  following the "ResultItem"  or 
			//	"ExtResultItem" which are defined by UserData
			//-----------------------------------------------------------------------------------------------------
			foreach (XmlNode node in resultItemList)
			{
				xPathKey = xPath + "'" + (node as XmlElement).InnerText + "']";
				msrtNode = this._xmlDoc.DocumentElement.SelectSingleNode(xPathKey);

				if (msrtNode != null)
				{
					headerElem = this._xmlDoc.CreateElement(this.GetHeader(headIndex));
					headerElem.InnerText = (node as XmlElement).InnerText;
					headerElem.SetAttribute("name", (msrtNode as XmlElement).GetAttribute("name"));
					headerElem.SetAttribute("enable", (msrtNode as XmlElement).GetAttribute("enable"));
					headerElem.SetAttribute("unit", (msrtNode as XmlElement).GetAttribute("unit"));
					headerElem.SetAttribute("format", (msrtNode as XmlElement).GetAttribute("format"));
                    headerElem.SetAttribute("nullValue", (msrtNode as XmlElement).GetAttribute("nullValue"));
					headerElem.SetAttribute("index", "");
					headerElem.SetAttribute("min", "-99999");
					headerElem.SetAttribute("max", "99999");
				}
				else
				{
					headerElem = this._xmlDoc.CreateElement(this.GetHeader(headIndex));
					headerElem.InnerText = (node as XmlElement).InnerText;
					headerElem.SetAttribute("name", (node as XmlElement).GetAttribute("name"));
					headerElem.SetAttribute("enable", (node as XmlElement).GetAttribute("enable"));
					headerElem.SetAttribute("unit", (node as XmlElement).GetAttribute("unit"));
					headerElem.SetAttribute("format", (node as XmlElement).GetAttribute("format"));
                    headerElem.SetAttribute("nullValue", (node as XmlElement).GetAttribute("nullValue"));
					headerElem.SetAttribute("index", "");
					headerElem.SetAttribute("min", "-99999");
					headerElem.SetAttribute("max", "99999");
				}

				itemName = UserData.ExtractKeyNameLetter(node.InnerText);

				if (	itemName == EOptiMsrtType.LOP.ToString() ||
						itemName == EOptiMsrtType.WATT.ToString() ||
						itemName == EOptiMsrtType.LM.ToString())
				{
					switch (DataCenter._product.LOPSaveItem)
					{
						case ELOPSaveItem.mcd:
							if (itemName == EOptiMsrtType.LOP.ToString())
							{
								bodyHeaderElems.AppendChild(headerElem);
								headIndex++;
							}
							break;
						//------------------------------------------------------
						case ELOPSaveItem.watt:
							if (itemName == EOptiMsrtType.WATT.ToString())
							{
								bodyHeaderElems.AppendChild(headerElem);
								headIndex++;
							}
							break;
						//------------------------------------------------------
						case ELOPSaveItem.lm:
							if (itemName == EOptiMsrtType.LM.ToString())
							{
								bodyHeaderElems.AppendChild(headerElem);
								headIndex++;
							}
							break;
						//------------------------------------------------------
						case ELOPSaveItem.mcd_lm:
							if (itemName == EOptiMsrtType.LM.ToString() || itemName == EOptiMsrtType.LOP.ToString())
							{
								bodyHeaderElems.AppendChild(headerElem);
								headIndex++;
							}
							break;
						//------------------------------------------------------
						case ELOPSaveItem.watt_lm:
							if (itemName == EOptiMsrtType.LM.ToString() || itemName == EOptiMsrtType.WATT.ToString())
							{
								bodyHeaderElems.AppendChild(headerElem);
								headIndex++;
							}
							break;
						//------------------------------------------------------
						case ELOPSaveItem.mcd_watt:
							if (itemName == EOptiMsrtType.LOP.ToString() || itemName == EOptiMsrtType.WATT.ToString())
							{
								bodyHeaderElems.AppendChild(headerElem);
								headIndex++;
							}
							break;
						//------------------------------------------------------
						case ELOPSaveItem.mcd_watt_lm:
							if (itemName == EOptiMsrtType.LM.ToString() || itemName == EOptiMsrtType.LOP.ToString() || itemName == EOptiMsrtType.WATT.ToString())
							{
								bodyHeaderElems.AppendChild(headerElem);
								headIndex++;
							}
							break;
						//------------------------------------------------------
						default:
							break;
					}
				}
				else
				{
					bodyHeaderElems.AppendChild(headerElem);
					headIndex++;
				}
			}
		}
		}

		private void CreateBodyGainOffsetAndResetHeaderFormat(XmlElement bodyHeaderElems, XmlElement bodyGainOffsetElems)
		{
			XmlElement oneGainElem;
			int index = 0;
			bool isFindOutputItem = false;

			//-----------------------------------------------------------------------------------------------------
			// Set TestResult Format and Index to  "/Body/Header/*" nodelist and  Create 
			// GainOffset value following each node of "/Body/Header/*" nodelist
			//-----------------------------------------------------------------------------------------------------
			foreach (XmlElement elem in bodyHeaderElems)
			{
				//--------------------------------------------------------------------------------------
				// (1) Create the /Body/GainOffset Items following /Body/Header,
				//		and set square, gain and offset value to /Body/GainOffset01
				//--------------------------------------------------------------------------------------
				bool isFindGainOffset = false;
				foreach (TestItemData testItems in DataCenter._product.TestCondition.TestItemArray)
				{
					if (testItems.GainOffsetSetting != null)
					{
						GainOffsetData[] data = testItems.GainOffsetSetting;

						for (int i = 0; i < data.Length; i++)
						{
							if (elem.InnerText == data[i].KeyName)
							{
								oneGainElem = this._xmlDoc.CreateElement("G");
								oneGainElem.InnerText = data[i].KeyName;
								oneGainElem.SetAttribute("name", data[i].Name.ToString());
								oneGainElem.SetAttribute("enable", (Convert.ToInt32(data[i].IsEnable)).ToString());
								oneGainElem.SetAttribute("type", ((int)data[i].Type).ToString());
								oneGainElem.SetAttribute("square", data[i].Square.ToString());
								oneGainElem.SetAttribute("gain", data[i].Gain.ToString());
								oneGainElem.SetAttribute("offset", data[i].Offset.ToString());
                                oneGainElem.SetAttribute("gain2", data[i].Gain2.ToString());
                                oneGainElem.SetAttribute("offset2", data[i].Offset2.ToString());
                                oneGainElem.SetAttribute("gain3", data[i].Gain3.ToString());
                                oneGainElem.SetAttribute("offset3", data[i].Offset3.ToString());
								bodyGainOffsetElems.AppendChild(oneGainElem);
								isFindGainOffset = true;
								break;
							}
						}
					}
				}

				if (isFindGainOffset == false)
				{
					oneGainElem = this._xmlDoc.CreateElement("G");
					oneGainElem.InnerText = elem.InnerText;
					oneGainElem.SetAttribute("name", elem.GetAttribute("name"));
					oneGainElem.SetAttribute("type", "");
					oneGainElem.SetAttribute("square", "0");
					oneGainElem.SetAttribute("gain", "1");
					oneGainElem.SetAttribute("offset", "0");
                    oneGainElem.SetAttribute("gain2", "1");
                    oneGainElem.SetAttribute("offset2", "0");
                    oneGainElem.SetAttribute("gain3", "1");
                    oneGainElem.SetAttribute("offset3", "0");
					bodyGainOffsetElems.AppendChild(oneGainElem);
				}

				//--------------------------------------------------------------------------------------
				// (2) Set Output TestResult Format and Index  to /Body/Header 
				//--------------------------------------------------------------------------------------
				if (DataCenter._product.TestCondition == null || DataCenter._product.TestCondition.TestItemArray == null)
					break;

				index = 0;

				isFindOutputItem = false;

				foreach (TestResultData data in DataCenter._acquireData.OutputTestResult)
				{
					if (elem.InnerText == data.KeyName)
					{
						isFindOutputItem = true;
						if (elem.GetAttribute("name") == "")
						{
							elem.SetAttribute("name", data.Name);
						}

						elem.SetAttribute("enable", (Convert.ToInt32(data.IsEnable)).ToString());

						elem.SetAttribute("format", data.Formate);

                        if (OLD_VERSION_BODYDATALIST == true)
                        {
                            elem.SetAttribute("index", index.ToString());
                        }
                        else 
                        {
                            elem.SetAttribute("index", AppSystem._bodyDataDic[data.KeyName][1].ToString());
                        }

						if (data.IsEnable == true && data.IsVerify == true)
						{
							elem.SetAttribute("min", data.MinLimitValue.ToString(data.Formate));

							elem.SetAttribute("max", data.MaxLimitValue.ToString(data.Formate));		//paul 20101204 modify min==>max
						}
						else
						{
							elem.SetAttribute("min", "0");

							elem.SetAttribute("max", "0");
						}
						break;
					}
					index++;
				}

				if (isFindOutputItem == false)
				{
					elem.SetAttribute("enable", "0");
					elem.SetAttribute("min", "0");
					elem.SetAttribute("max", "0");
				}

				//--------------------------------------------------------------------------------------
				// (3) Reset the keyName for single LOPWL Test item
				//--------------------------------------------------------------------------------------
				if (elem.GetAttribute("setting") != "" && elem.GetAttribute("num") != "")
				{
					elem.InnerText = "LOP_" + elem.GetAttribute("num");
				}
			}
		}

        private void WriteXML(string fileName)
        {
            XmlTextWriter xtw = null;
            xtw = new XmlTextWriter(fileName, null);
            xtw.Formatting = System.Xml.Formatting.None;
            this._xmlDoc.Save(xtw);
            xtw.Close();
        }

        private void SaveOutXtmp(string fileName)
        {
            this._runThread = new Thread(() => this.WriteXML(fileName));

            this._runThread.Priority = ThreadPriority.BelowNormal;

            this._runThread.Start();

            this._runThread.Join();
        }

        private void CreateBodyDataAndSetValue(XmlElement bodyHeaderElems, XmlElement bodyDataElems, XmlElement bodyDataExElems)
		{
			string indexStr;

			int elemIndex = 0;

            //------------------------------------------------------------------------------------------------------------------------------------------------------
            // add by Alec for Seperate XML
			//------------------------------------------------------------------------------------------------------------------------------------------------------
            string xmlResultOutPathAndFile = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, Constants.Files.OUTPUT_XML_TEMP);

            int index = 0;

            int fileIndex = 0;

            string tempFileNameWithIndex = null;

            string[] tempFolderFiles = Directory.GetFiles(Constants.Paths.LEDTESTER_TEMP_DIR);

            foreach (string fileName in tempFolderFiles)
            {
				if (fileName.Contains(Constants.Files.OUTPUT_XML_TEMP))
                {
					if (!MPIFile.DeleteFile(fileName))
					{
						return;
					}
                }
            }

				foreach (float[] data in AppSystem._bodyDataList)
				{
					//------------------------------------------------------------------------------------------------------------------------------------------------------
					// add by Alec for Seperate XML
					//------------------------------------------------------------------------------------------------------------------------------------------------------
					index++;

					XmlElement indexData = this._xmlDoc.CreateElement("row");

					if (DataCenter._uiSetting.IsEnableFloatReport)
					{
						TestResultData[] resultArray = DataCenter._acquireData.OutputTestResult.ToArray();

						int rawDataIndex = 0;

						for (int i = 0; i < resultArray.Length; i++)
						{
							bool isShow = true;

							foreach (var item in Enum.GetNames(typeof(MPI.Tester.Data.ESysResultItem)))
							{
								if (item == "TEST" || item == "BIN" || item == "POLAR" || item == "ISALLPASS")
								{
									continue;
								}

								if (resultArray[i].KeyName == item)
								{
									isShow = false;

									break;
								}
							}

                            foreach (var item in Enum.GetNames(typeof(MPI.Tester.Data.EProberDataIndex)))
							{
								if (item == "ROW" || item == "COL")
					        {
									continue;
								}

								if (resultArray[i].KeyName == item)
					        {
									isShow = false;

									break;
								}
							}

							if (!resultArray[i].IsEnable || !resultArray[i].IsVision || !isShow)
						{
								continue;
							}

							string strData = ((double)data[i]).ToString(resultArray[i].Formate);

							indexData.SetAttribute(this.GetHeader(rawDataIndex), strData);

							rawDataIndex++;
						}
						}
						else
						{
						foreach (XmlElement elem in bodyHeaderElems)
						{
							indexStr = elem.GetAttribute("index");

							string strData = string.Empty;

							if (indexStr == null || indexStr == "" || elem.GetAttribute("enable") == "0")
							{
								strData = elem.GetAttribute("nullValue");
							}
							else
							{
								elemIndex = int.Parse(indexStr);

								strData = ((double)data[elemIndex]).ToString(elem.GetAttribute("format"));
							}

							indexData.SetAttribute(elem.Name, strData);
						}
					}

					bodyDataElems.AppendChild(indexData);

					//------------------------------------------------------------------------------------------------------------------------------------------------------
					// add by Alec for Seperate XML
					//------------------------------------------------------------------------------------------------------------------------------------------------------				
					if ((index % this._seperateSize) == 0)
					{
						tempFileNameWithIndex = xmlResultOutPathAndFile + fileIndex.ToString();

						fileIndex++;

						this.SaveOutXtmp(tempFileNameWithIndex);

						bodyDataElems.RemoveAll();

						bodyDataExElems.RemoveAll();

						GC.Collect();

						this._frmProgressBar.ProgressBarValue++;
					}
				}

				//------------------------------------------------------------------------------------------------------------------------------------------------------
				// add by Alec for Seperate XML
				//------------------------------------------------------------------------------------------------------------------------------------------------------
				tempFileNameWithIndex = xmlResultOutPathAndFile + fileIndex.ToString();

				this.SaveOutXtmp(tempFileNameWithIndex);

				bodyDataElems.RemoveAll();

				bodyDataExElems.RemoveAll();

				GC.Collect();

            this.SaveOutXtmp(xmlResultOutPathAndFile);

            this._xmlDoc.RemoveAll();

            GC.Collect();

			this._frmProgressBar.ProgressBarValue++;
		}

        private bool WriteCoeffDataToXMLBody()
        {
            XmlNode rootNode = this._xmlDoc.DocumentElement;

            XmlElement BodyElem = this._xmlDoc.CreateElement("Body");
            rootNode.AppendChild(BodyElem);

            XmlElement bodyHeader = this._xmlDoc.CreateElement("Header");
            BodyElem.AppendChild(bodyHeader);

            XmlElement bodyGainOffset = this._xmlDoc.CreateElement("GainOffset");
            BodyElem.AppendChild(bodyGainOffset);

            int count = this._xmlDoc.DocumentElement.SelectNodes("//TesterOutput/Format/ResultItem/*").Count;
            this.CreateBodyHeader(0, this._xmlDoc.DocumentElement.SelectNodes("//TesterOutput/Format/ResultItem/*"), bodyHeader);

            if (DataCenter._product.TestCondition.TestItemArray != null)
            {
                this.CreateBodyGainOffsetAndResetHeaderFormat(bodyHeader, bodyGainOffset);
            }
            return true;
        }

		private bool WriteDataToXmlBodyAndOutputXtmp(bool isSetBodyData = true)
		{
			Console.WriteLine("[Storage], WriteDataToXmlBodyAndOutputXtmp()");

			if ( this._xmlDoc == null )
				return false;

			XmlNode rootNode = this._xmlDoc.DocumentElement;
			//--------------------------------------------------------------------------------------------------
			// (1) Create "/Body" element to XML
			//		Append "/Body/Header", "/Body/GainOffset" and /Body/Data" elements to XML
			//--------------------------------------------------------------------------------------------------
			XmlElement BodyElem = this._xmlDoc.CreateElement("Body");
			rootNode.AppendChild(BodyElem);

			XmlElement bodyHeader = this._xmlDoc.CreateElement("Header");
			BodyElem.AppendChild(bodyHeader);

			XmlElement bodyGainOffset = this._xmlDoc.CreateElement("GainOffset");
			BodyElem.AppendChild(bodyGainOffset);

			XmlElement bodyData = this._xmlDoc.CreateElement("Data");
			BodyElem.AppendChild(bodyData);

            XmlElement bodyDataEx = this._xmlDoc.CreateElement("DataEx");
            BodyElem.AppendChild(bodyDataEx);

			//-----------------------------------------------------------------------------------------------------
			// (2) Create each element of "Body/Header/*" to XML by "//TesterOutput/Format/ResultItem/*
			//		and "//TesterOutput/Format/ExtResultItem/*"
			//-----------------------------------------------------------------------------------------------------
			int count =  this._xmlDoc.DocumentElement.SelectNodes("//TesterOutput/Format/ResultItem/*").Count;
			this.CreateBodyHeader(0, this._xmlDoc.DocumentElement.SelectNodes("//TesterOutput/Format/ResultItem/*"), bodyHeader);
			if (DataCenter._uiSetting.IsExtResultItem && !DataCenter._uiSetting.IsEnableFloatReport)
			{
				this.CreateBodyHeader(count, this._xmlDoc.DocumentElement.SelectNodes("//TesterOutput/Format/ExtResultItem/*"), bodyHeader);
			}

			//-----------------------------------------------------------------------------------------------------
			// (3) If the test item is disable, then also set test result to be disable. 
			//-----------------------------------------------------------------------------------------------------
			if (DataCenter._product.TestCondition.TestItemArray != null)
			{
				foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
				{
					if (item.MsrtResult != null)
					{
						foreach (TestResultData data in item.MsrtResult)
						{
							foreach (TestResultData data2 in DataCenter._acquireData.OutputTestResult)
							{
								if (data2.KeyName == data.KeyName && item.IsEnable == false)
								{
                                    if (item is LIVTestItem && !data.IsVision)
                                    {
                                        break;
                                    }
									
									data.IsEnable = false;
									
									break;
								}
							}
						}
					}
				}
			}

			//-----------------------------------------------------------------------------------------------------
			// (4) Set TestResult Format and Index to  "/Body/Header/*" nodelist and  Create 
			//		GainOffset value following each node of "/Body/Header/*" nodelist
			//-----------------------------------------------------------------------------------------------------
            if (DataCenter._product.TestCondition.TestItemArray != null)
            {
                this.CreateBodyGainOffsetAndResetHeaderFormat(bodyHeader, bodyGainOffset);
            }
			
			//-----------------------------------------------------------------------------------------------------
			// (5) Create "TesterOutput/StatisticsData" elements to XML and Set Value, for Print fuction
			//-----------------------------------------------------------------------------------------------------
			string reportFileWithExt = string.Format("{0}{1}", "Format-", ((int)DataCenter._uiSetting.UserID).ToString("0000")) + ".txt";
			string reportPathAndFile = Path.Combine(Constants.Paths.USER_DIR, reportFileWithExt);

            string printFileWithExt = string.Format("{0}{1}", "PrintFormat", ((int)DataCenter._uiSetting.UserID).ToString("0000")) + ".xslt";
            string printPathAndFile = Path.Combine(Constants.Paths.BARCODE_PRINT_DIR, printFileWithExt);

			if (File.Exists(reportPathAndFile) || File.Exists(printPathAndFile) || DataCenter._uiSetting.IsWriteStatisticsDataToXmlHead)
			{
				this.WriteStatisticsDataToXmlHead();
			}

            //-----------------------------------------------------------------------------------------------------
            // (6) Create "Body/Data/*" elements to XML and Set Value by index, then Output to *.Xtmp# files
            //-----------------------------------------------------------------------------------------------------
			if (isSetBodyData)
			{
                this._xmlDocBodyHeader = bodyHeader.Clone();

            	this.CreateBodyDataAndSetValue(bodyHeader, bodyData, bodyDataEx);
			}

			return true;
		}

		private void MyDeleteDirectory(string targetDir)
		{			
			string[] files = Directory.GetFiles(targetDir);

			string[] dirs = Directory.GetDirectories(targetDir);

			foreach (string file in files)
			{
				File.SetAttributes(file, FileAttributes.Normal);

				MPIFile.DeleteFile(file);
			}

			foreach (string dir in dirs)
			{
				MyDeleteDirectory(dir);
			}

			MPIFile.DeleteDirectory(targetDir, false);

		}

		private void MyDeleteFiles(string targetDir)
		{
			if (File.Exists(targetDir) == false)
				return;

			string[] files = Directory.GetFiles(targetDir);

			foreach (string file in files)
			{
				File.SetAttributes(file, FileAttributes.Normal);

				MPIFile.DeleteFile(file);
			}
		}

        private bool UseCoefXsltToTransferFormatAndSaveFile(string CoeffXsltFile)
        {
            string xmlResultOutPathAndFile = Path.Combine(Constants.Paths.COEFFICIENT_DIR, Constants.Files.OUTPUT_XML_TEMP); // XML File
            
			string csvResultOutPathAndFile = Path.Combine(Constants.Paths.COEFFICIENT_DIR, Constants.Files.OUTPUT_CSV_TEMP + "3"); // OUTPUT File
         
            DirectoryInfo coeffDir = new DirectoryInfo(Constants.Paths.COEFFICIENT_DIR);

            if (!coeffDir.Exists)
            {
                coeffDir.Create();
            }

            XmlTextWriter xtw = new XmlTextWriter(xmlResultOutPathAndFile, null);
            
			xtw.Formatting = System.Xml.Formatting.None;
            
			this._xmlDoc.Save(xtw);
            
			xtw.Close();

            XslCompiledTransform xsl = new XslCompiledTransform();
  
            try
            {
                xsl.Load(CoeffXsltFile);
                
				xsl.Transform(xmlResultOutPathAndFile, csvResultOutPathAndFile);
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be opened because it was locked by another process. {0}", e.ToString());

                return false;
            }
            catch (Exception E)
            {
				Console.WriteLine("The file could not be opened because it was locked by another process. {0}", E.ToString());

                return false;
            }

            //-----------------------------------------------------------------------------------------------------
			// (2) Save Coefficient To File
			//-----------------------------------------------------------------------------------------------------
            string SaveCoefficentFilePath = Path.Combine(Constants.Paths.COEFFICIENT_DIR, DataCenter._uiSetting.TaskSheetFileName+".csv");

			if (!Directory.Exists(Constants.Paths.COEFFICIENT_DIR))
            {
				Directory.CreateDirectory(Constants.Paths.COEFFICIENT_DIR);
            }

            if (File.Exists(SaveCoefficentFilePath))
            {
                List<string[]> data= CSVUtil.ReadCSV(SaveCoefficentFilePath);

                if (data != null)
                {
                    List<string[]> currentData = CSVUtil.ReadCSV(csvResultOutPathAndFile);

                    List<string[]> combineData = new List<string[]>();

                    combineData.AddRange(currentData);

                    combineData.Add(new string[1] { " " });

                    combineData.AddRange(data);

                    CSVUtil.WriteCSV(SaveCoefficentFilePath, combineData);
                }
                else
                {
                    return false;
                }
            }
            else
            {
				MPIFile.CopyFile(csvResultOutPathAndFile, SaveCoefficentFilePath);
            }

			MPIFile.DeleteFile(xmlResultOutPathAndFile);

			MPIFile.DeleteFile(csvResultOutPathAndFile);

            return true;
        }

        private void RunXslTransfer(XslCompiledTransform xsl, string inputFileName, string outputFileName)
        {
            try
            {
                xsl.Transform(inputFileName, outputFileName);
            }
            catch (IOException e)
            {
                Console.WriteLine("[Storage],RunXslTransfer(),The file could not be opened because it was locked by another process. {0}", e.ToString());

                return;
            }
            catch (Exception E)
            {
                Console.WriteLine("[Storage],RunXslTransfer(),The file could not be opened because it was locked by another process. {0}", E.ToString());

                return;
            }

            this._runThreadSuccess = true;
        }

        private void CreateTransferThreadAndWait(XslCompiledTransform xsl, string inputFileName, string outputFileName)
        {
            this._runThreadSuccess = false;

            this._runThread = new Thread(() => this.RunXslTransfer(xsl, inputFileName, outputFileName));

            this._runThread.Priority = ThreadPriority.BelowNormal;

            this._runThread.Start();

            this._runThread.Join();

            GC.Collect();
        }

        private void TransferXmlToOtmp()
        {
            Console.WriteLine("[Storage], TransferXmlToOtmp()");

            //------------------------------------------------------------------------------------------------------------------------------------
            // (1) Transfer the xml file to cvs file format
            //------------------------------------------------------------------------------------------------------------------------------------
            // Save the document to a file and auto-indent the output.
            // BE Careful !! , VS2005 xsl transform have bug for "empty element"
            //
            // this._xmlDoc.Save(Constants.Paths.TEMP_DIR + Constants.Files.OUTPUT_TEMP);
            // save data by XmlTestWriter and set the formatting = indent
            //--------------------------------------------------------------------------------------------------------------------------------------

            string xmlResultOutPathAndFile = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, Constants.Files.OUTPUT_XML_TEMP);

            string csvResultOutPathAndFile = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, Constants.Files.OUTPUT_CSV_TEMP);

            string[] sysTempFolderFiles = Directory.GetFiles(Constants.Paths.LEDTESTER_TEMP_DIR);

            int xtmpCount = -1;

            foreach (string fileName in sysTempFolderFiles)
            {
                if (fileName.Contains(Constants.Files.OUTPUT_CSV_TEMP))
                {
                    if (!MPIFile.DeleteFile(fileName))
                    {
                        return;
                    }
                }

                if (fileName.Contains(Constants.Files.OUTPUT_XML_TEMP))
                {
                    xtmpCount++;
                }
            }

            XslCompiledTransform xsl = new XslCompiledTransform();

            xsl.Load(Path.Combine(Constants.Paths.USER_DIR, this._xslFileName + ".xslt"));

            this.CreateTransferThreadAndWait(xsl, xmlResultOutPathAndFile, csvResultOutPathAndFile);

            if (this._runThreadSuccess == false)
            {
                return;
            }

            this._frmProgressBar.ProgressBarValue++;

                for (int i = 0; i < xtmpCount; i++)
                {
                    string inputFileName = xmlResultOutPathAndFile + i.ToString();

                    string outputFileName = csvResultOutPathAndFile + i.ToString();

                    this.CreateTransferThreadAndWait(xsl, inputFileName, outputFileName);

                    if (this._runThreadSuccess == false)
                    {
                        return;
                    }

                    this._frmProgressBar.ProgressBarValue++;
                }

            string sorterFile = Path.Combine(Constants.Paths.USER_DIR, this._xslFileName + "-Sorter.xslt");

            if (!File.Exists(sorterFile))
            {
                return;
            }

            string csvSorterResultOutPathAndFile = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, Constants.Files.OUTPUT_CSV_TEMP_SORTER);

            xsl = new XslCompiledTransform();

            xsl.Load(sorterFile);

            this.CreateTransferThreadAndWait(xsl, xmlResultOutPathAndFile, csvSorterResultOutPathAndFile);

            if (this._runThreadSuccess == false)
            {
                return;
            }

            for (int i = 0; i < xtmpCount; i++)
            {
                string inputFileName = xmlResultOutPathAndFile + i.ToString();

                string outputFileName = csvSorterResultOutPathAndFile + i.ToString();

                this.CreateTransferThreadAndWait(xsl, inputFileName, outputFileName);

                if (this._runThreadSuccess == false)
                {
                    return;
                }

                this._frmProgressBar.ProgressBarValue++;
            }
        }

		private bool MergeOtmpFiles(string path, string fileName)
		{
			//Check Directory is Exist
			if (!Directory.Exists(path))
			{
                Console.WriteLine("[Storage],MergeOtmpFiles()," + path + ",Path is not Exist!");

				return false;
			}

			//Get Report List
			List<string> fileInfo = new List<string>(Directory.GetFiles(path));

			List<string> reportFiles = new List<string>();

			string fileNameAndPath = Path.Combine(path, fileName);

			for (int i = 0; i < fileInfo.Count; i++)
			{
				string str = fileNameAndPath + i.ToString();

				if (fileInfo.Contains(str))
				{
					reportFiles.Add(str);
				}
				else
				{
					break;
				}
			}

			//Check Report Files Count
			if (reportFiles.Count == 0)
			{
                Console.WriteLine("[Storage],MergeOtmpFiles(), Report Files.Count = 0!");

				return false;
			}

			try
			{
				List<string[]> report = CSVUtil.ReadCSV(fileNameAndPath);

				//Get key
				StreamWriter sw = new StreamWriter(fileNameAndPath, true, Encoding.Default);

				//Append Files
				foreach (var item in reportFiles)
				{
					int row = 0;

					foreach (var line in File.ReadLines(item, Encoding.Default))
					{
						if (row >= report.Count)
						{
							sw.WriteLine(line);
						}

						row++;
					}
				}

				sw.Close();

				return true;
			}
			catch (Exception e)
			{
                Console.WriteLine("[Storage],MergeOtmpFiles()," + e.ToString());

				return false;
			}
		}

		private string ParseOutputFileName(int index)
		{
			StringBuilder sb = new StringBuilder();
			sb.Clear();

			string []fileFormatStrArray = null;

			if ( index ==  1 )
			{
				fileFormatStrArray = DataCenter._uiSetting.UserDefinedData.OutputFileNameFormat01;
			}
			else if (  index ==  2 )
			{
				fileFormatStrArray = DataCenter._uiSetting.UserDefinedData.OutputFileNameFormat02;
			}
			else
			{
				return  "BarCode";
			}

			if (fileFormatStrArray == null)
			{
				return "BarCode";
			}

			foreach( string str in fileFormatStrArray )
			{
				if ( str.ToUpper() == "BarCode".ToUpper() )
				{
					sb.Append(DataCenter._uiSetting.Barcode);
					continue;
				}
				else if (str.ToUpper() == "LotNum".ToUpper() )
				{
					sb.Append(DataCenter._uiSetting.LotNumber);
					continue;
				}
				else if (str.ToUpper() == "WaferNum".ToUpper() )
				{
					sb.Append(DataCenter._uiSetting.WaferNumber);
					continue;
				}
				else if (str.ToUpper() == "StartTime".ToUpper() )
				{
					sb.Append(DataCenter._sysSetting.StartTestTime.ToString("yyMMddHHmmss"));
					continue;
				}
				else if (str.ToUpper() == "MachineName".ToUpper() )
                {
                    sb.Append(DataCenter._uiSetting.MachineName);
                    continue;
                }
				else if (str.ToUpper() == "OperatorName".ToUpper() )
				{
					sb.Append(DataCenter._uiSetting.OperatorName);
					continue;
				}
				else if (str.ToUpper() == "Substrate".ToUpper())
				{
					sb.Append(DataCenter._uiSetting.Substrate);
					continue;
				}
				else if (str.ToUpper() == "ProductType".ToUpper())
				{
					sb.Append(DataCenter._uiSetting.ProductType);
					continue;
				}
				else if (str.ToUpper() == "WMRemark1".ToUpper())
				{
					sb.Append(DataCenter._uiSetting.WeiminUIData.Remark01);
					continue;
				}
				else if (str.ToUpper() == "WMRemark2".ToUpper())
				{
					sb.Append(DataCenter._uiSetting.WeiminUIData.Remark02);
					continue;
				}
				else if (str.ToUpper() == "WMRemark3".ToUpper())
				{
					sb.Append(DataCenter._uiSetting.WeiminUIData.Remark03);
					continue;
				}
				else if (str.ToUpper() == "WMDeviceNumber".ToUpper())
				{
					sb.Append(DataCenter._uiSetting.WeiminUIData.DeviceNumber);
					continue;
				}
				else if (str.ToUpper() == "WMSpecification".ToUpper())
				{
					sb.Append(DataCenter._uiSetting.WeiminUIData.Specification);
					continue;
				}
                else if (str.ToUpper() == "SubPiece".ToUpper())
                {
                    sb.Append(DataCenter._uiSetting.SubPiece);
                    continue;
                }
				else
				{
					sb.Append(str);
				}
                

			}
			return sb.ToString();
		}

        private void GenerateOutPath(string rootPath01, string rootPath02, string rootPath03, out string path01, out string path02, out string path03)
		{
			string parseStr = "A1N119299025-17";
			string a = DataCenter._uiSetting.TestResultFileName;
			StringBuilder sb = new StringBuilder();
			sb.Clear();
			sb.Append(@"\");

			switch (DataCenter._uiSetting.UserID)
			{
				case EUserID.Sanan:
					{
						string year = "";
						string month = "";
						string day = "";
						year = "20" + parseStr.Substring(3, 2);
						month = Convert.ToInt32(parseStr.Substring(5, 1), 16).ToString("00");
						day = parseStr.Substring(6, 2);
						sb.Append(year);
						sb.Append(@"\");
						sb.Append(year + "_" + month);
						sb.Append(@"\");
						sb.Append(year + "_" + month + "_" + day);
						sb.Append(@"\");
						if (DataCenter._uiSetting.WeiminUIData.WMTestMode == (int)EWMTestMode.FullyTest)
						{
							sb.Append("GaN_Mapping");
						}
						else if (DataCenter._uiSetting.WeiminUIData.WMTestMode == (int)EWMTestMode.SampleTest)
						{
							sb.Append("GaN");
						}
						else if (DataCenter._uiSetting.WeiminUIData.WMTestMode == (int)EWMTestMode.ESDTest)
						{
							sb.Append("ESD");
						}
						else
						{
							sb.Append(@"\");
						}

						path01 = rootPath01;
						path02 = rootPath02 + sb.ToString();
                        path03 = rootPath03 + sb.ToString();
					}
					break;
				//----------------------------------------------------------------------------------
				default :
					path01 = rootPath01;
					path02 = rootPath02;
                    path03 = rootPath03;
					break;
			}
		}

        private bool MoveFileToTarget(bool isEndTest)
        {
            Console.WriteLine("[Storage], MoveFileToTarget()");

            //---------------------------------------------------------------------------------
            // Move OutputTemp.xtmp# Files To C:\MPI\Share (For Print)
            //---------------------------------------------------------------------------------
            string[] shareFolderFiles = Directory.GetFiles(Constants.Paths.MPI_SHARE_DIR);

            foreach (string fileName in shareFolderFiles)
            {
                if (fileName.Contains(Constants.Files.OUTPUT_XML_TEMP))
                {
                    MPIFile.DeleteFile(fileName);
                }
            }

            string[] tempFolderFiles = Directory.GetFiles(Constants.Paths.LEDTESTER_TEMP_DIR);

            foreach (string fileName in tempFolderFiles)
            {
                if (fileName.Contains(Constants.Files.OUTPUT_XML_TEMP))
                {
                    string xmlHeadFile = fileName.Replace(Constants.Paths.LEDTESTER_TEMP_DIR, Constants.Paths.MPI_SHARE_DIR);

                    MPIFile.CopyFile(fileName, xmlHeadFile);
                }
            }

            if (DataCenter._machineConfig.Enable.IsBarcodePrint)
            {
                this.PrintLabel(null);
            }

            //---------------------------------------------------------------------------------
            // Get All Output Path
            //---------------------------------------------------------------------------------
            string csvResultOutPathAndFile = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, Constants.Files.OUTPUT_CSV_TEMP);

            bool isOutputPath02 = false;
            bool isOutputPath03 = false;
            string outPath01 = string.Empty;
            string outPath02 = string.Empty;
            string outPath03 = string.Empty;

            if (DataCenter._uiSetting.IsManualRunMode)
            {
                outPath01 = DataCenter._uiSetting.ManualOutputPath01;
                outPath02 = DataCenter._uiSetting.ManualOutputPath02;
                outPath03 = DataCenter._uiSetting.ManualOutputPath03;

                if (DataCenter._uiSetting.ManualOutputPathType01 == ETesterResultCreatFolderType.ByLotNumber)
                {
                    outPath01 = Path.Combine(outPath01, DataCenter._uiSetting.LotNumber);
                }
                else if (DataCenter._uiSetting.ManualOutputPathType01 == ETesterResultCreatFolderType.ByMachineName)
                {
                    outPath01 = Path.Combine(outPath01, DataCenter._uiSetting.MachineName);
                }
                else if (DataCenter._uiSetting.ManualOutputPathType01 == ETesterResultCreatFolderType.ByDataTime)
                {
                    outPath01 = Path.Combine(outPath01, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
                }

                if (DataCenter._uiSetting.ManualOutputPathType02 == ETesterResultCreatFolderType.ByLotNumber)
                {
                    outPath02 = Path.Combine(outPath02, DataCenter._uiSetting.LotNumber);
                }
                else if (DataCenter._uiSetting.ManualOutputPathType02 == ETesterResultCreatFolderType.ByMachineName)
                {
                    outPath02 = Path.Combine(outPath02, DataCenter._uiSetting.MachineName);
                }
                else if (DataCenter._uiSetting.ManualOutputPathType02 == ETesterResultCreatFolderType.ByDataTime)
                {
                    outPath02 = Path.Combine(outPath02, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
                }

                if (DataCenter._uiSetting.ManualOutputPathType03 == ETesterResultCreatFolderType.ByLotNumber)
                {
                    outPath03 = Path.Combine(outPath03, DataCenter._uiSetting.LotNumber);
                }
                else if (DataCenter._uiSetting.ManualOutputPathType03 == ETesterResultCreatFolderType.ByMachineName)
                {
                    outPath03 = Path.Combine(outPath03, DataCenter._uiSetting.MachineName);
                }
                else if (DataCenter._uiSetting.ManualOutputPathType03 == ETesterResultCreatFolderType.ByDataTime)
                {
                    outPath03 = Path.Combine(outPath03, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
                }
            }
            else
            {
                outPath01 = DataCenter._uiSetting.TestResultPath01;
                outPath02 = DataCenter._uiSetting.TestResultPath02;
                outPath03 = DataCenter._uiSetting.TestResultPath03;

                if (DataCenter._uiSetting.TesterResultCreatFolderType01 == ETesterResultCreatFolderType.ByLotNumber)
                {
                    outPath01 = Path.Combine(outPath01, DataCenter._uiSetting.LotNumber);
                }
                else if (DataCenter._uiSetting.TesterResultCreatFolderType01 == ETesterResultCreatFolderType.ByMachineName)
                {
                    outPath01 = Path.Combine(outPath01, DataCenter._uiSetting.MachineName);
                }
                else if (DataCenter._uiSetting.TesterResultCreatFolderType01 == ETesterResultCreatFolderType.ByDataTime)
                {
                    outPath01 = Path.Combine(outPath01, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
                }

                if (DataCenter._uiSetting.TesterResultCreatFolderType02 == ETesterResultCreatFolderType.ByLotNumber)
                {
                    outPath02 = Path.Combine(outPath02, DataCenter._uiSetting.LotNumber);
                }
                else if (DataCenter._uiSetting.TesterResultCreatFolderType02 == ETesterResultCreatFolderType.ByMachineName)
                {
                    outPath02 = Path.Combine(outPath02, DataCenter._uiSetting.MachineName);
                }
                else if (DataCenter._uiSetting.TesterResultCreatFolderType02 == ETesterResultCreatFolderType.ByDataTime)
                {
                    outPath02 = Path.Combine(outPath02, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
                }

                if (DataCenter._uiSetting.TesterResultCreatFolderType03 == ETesterResultCreatFolderType.ByLotNumber)
                {
                    outPath03 = Path.Combine(outPath03, DataCenter._uiSetting.LotNumber);
                }
                else if (DataCenter._uiSetting.TesterResultCreatFolderType03 == ETesterResultCreatFolderType.ByMachineName)
                {
                    outPath03 = Path.Combine(outPath03, DataCenter._uiSetting.MachineName);
                }
                else if (DataCenter._uiSetting.TesterResultCreatFolderType03 == ETesterResultCreatFolderType.ByDataTime)
                {
                    outPath03 = Path.Combine(outPath03, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
                }
            }

            this.GenerateOutPath(outPath01, outPath02, outPath03,
                                            out outPath01, out outPath02, out outPath03);

            //---------------------------------------------------------------------------------
            // Check Output path Eanble
            //---------------------------------------------------------------------------------
            if (DataCenter._uiSetting.IsManualRunMode)
            {
                if (outPath02 != outPath01 &&
                    DataCenter._uiSetting.IsEnableManualPath02)
                {
                    isOutputPath02 = true;
                }

                if (outPath03 != outPath01 &&
                    outPath03 != outPath02 &&
                    DataCenter._uiSetting.IsEnableManualPath03)
                {
                    isOutputPath03 = true;
                }
            }
            else
            {
                if (outPath02 != outPath01 &&
                    DataCenter._uiSetting.IsEnablePath02)
                {
                    isOutputPath02 = true;
                }

                if (outPath03 != outPath01 &&
                    outPath03 != outPath02 &&
                    DataCenter._uiSetting.IsEnablePath03)
                {
                    isOutputPath03 = true;
                }
            }

            //---------------------------------------------------------------------------------
            // Copy Report file to taget path
            //---------------------------------------------------------------------------------
            string fileNameWithoutExt = DataCenter._uiSetting.TestResultFileName;
            string fileNameWithExt = DataCenter._uiSetting.TestResultFileName;

            if (DataCenter._uiSetting.UIDisplayType == (int)EUIDisplayType.WMStartUI)
            {
                fileNameWithoutExt = DataCenter._uiSetting.WeiminUIData.KeyInFileName;
            }

            //Abort
            if (!isEndTest && DataCenter._uiSetting.UserID != EUserID.HCSemiTek)
            {
                fileNameWithoutExt = fileNameWithoutExt + "_" + DateTime.Now.ToString("yyMMddhhmmss");
            }

            fileNameWithExt = fileNameWithoutExt + "." + DataCenter._uiSetting.TestResultFileExt;

            string outputPathAndFile01 = Path.Combine(outPath01, fileNameWithExt);
            string outputPathAndFile02 = Path.Combine(outPath02, fileNameWithExt);
            string outputPathAndFile03 = Path.Combine(outPath03, fileNameWithExt);

            MPIFile.CopyFile(csvResultOutPathAndFile, outputPathAndFile01);

            if (isOutputPath02)
            {
                if (DataCenter._uiSetting.UserID == EUserID.HCSemiTek
                    || DataCenter._uiSetting.UserID == EUserID.Lumitek)
                {
                    if (isEndTest)
                    {
                        MPIFile.CopyFile(csvResultOutPathAndFile, outputPathAndFile02);
                    }
                }
                else
                {
                    MPIFile.CopyFile(csvResultOutPathAndFile, outputPathAndFile02);
                }
            }

            if (isOutputPath03)
            {
                if (DataCenter._uiSetting.UserID == EUserID.HCSemiTek
                     || DataCenter._uiSetting.UserID == EUserID.Lumitek)
                {
                    if (isEndTest)
                    {
                        MPIFile.CopyFile(csvResultOutPathAndFile, outputPathAndFile03);
                    }
                }
                else
                {
                    MPIFile.CopyFile(csvResultOutPathAndFile, outputPathAndFile03);
                }
            }

            //---------------------------------------------------------------------------------
            // Copy Sorter file to taget path
            //---------------------------------------------------------------------------------
            csvResultOutPathAndFile = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, Constants.Files.OUTPUT_CSV_TEMP_SORTER);

            string sorterFile = Path.Combine(Constants.Paths.USER_DIR, this._xslFileName + "-Sorter.xslt");

            if (File.Exists(csvResultOutPathAndFile) && File.Exists(sorterFile))
            {
                Console.WriteLine("[Storage], SaveWAFReport() , Start Copy File");

                //================
                // Paul Edit 2013.11.27
                //================

                string WAFfileNameWithExt = fileNameWithoutExt + "." + DataCenter._uiSetting.WAFTestResultFileExt;

                string WAFOutputPath01 = DataCenter._uiSetting.WAFOutputPath01;

                string WAFOutputPath02 = DataCenter._uiSetting.WAFOutputPath02;

                string WAFOutputPath03 = DataCenter._uiSetting.WAFOutputPath03;

                if (DataCenter._uiSetting.WAFTesterResultCreatFolderType01 == ETesterResultCreatFolderType.ByLotNumber)
                {
                    WAFOutputPath01 = Path.Combine(WAFOutputPath01, DataCenter._uiSetting.LotNumber);
                }
                else if (DataCenter._uiSetting.WAFTesterResultCreatFolderType01 == ETesterResultCreatFolderType.ByMachineName)
                {
                    WAFOutputPath01 = Path.Combine(WAFOutputPath01, DataCenter._uiSetting.MachineName);
                }
                else if (DataCenter._uiSetting.WAFTesterResultCreatFolderType01 == ETesterResultCreatFolderType.ByDataTime)
                {
                    WAFOutputPath01 = Path.Combine(WAFOutputPath01, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
                }

                if (DataCenter._uiSetting.WAFTesterResultCreatFolderType02 == ETesterResultCreatFolderType.ByLotNumber)
                {
                    WAFOutputPath02 = Path.Combine(WAFOutputPath02, DataCenter._uiSetting.LotNumber);
                }
                else if (DataCenter._uiSetting.WAFTesterResultCreatFolderType02 == ETesterResultCreatFolderType.ByMachineName)
                {
                    WAFOutputPath02 = Path.Combine(WAFOutputPath02, DataCenter._uiSetting.MachineName);
                }
                else if (DataCenter._uiSetting.WAFTesterResultCreatFolderType02 == ETesterResultCreatFolderType.ByDataTime)
                {
                    WAFOutputPath02 = Path.Combine(WAFOutputPath02, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
                }

                if (DataCenter._uiSetting.WAFTesterResultCreatFolderType03 == ETesterResultCreatFolderType.ByLotNumber)
                {
                    WAFOutputPath03 = Path.Combine(WAFOutputPath03, DataCenter._uiSetting.LotNumber);
                }
                else if (DataCenter._uiSetting.WAFTesterResultCreatFolderType03 == ETesterResultCreatFolderType.ByMachineName)
                {
                    WAFOutputPath03 = Path.Combine(WAFOutputPath03, DataCenter._uiSetting.MachineName);
                }
                else if (DataCenter._uiSetting.WAFTesterResultCreatFolderType03 == ETesterResultCreatFolderType.ByDataTime)
                {
                    WAFOutputPath03 = Path.Combine(WAFOutputPath03, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
                }

                string sorterOutputPathAndFile01 = Path.Combine(WAFOutputPath01, WAFfileNameWithExt);

                string sorterOutputPathAndFile02 = Path.Combine(WAFOutputPath02, WAFfileNameWithExt);

                string sorterOutputPathAndFile03 = Path.Combine(WAFOutputPath03, WAFfileNameWithExt);

                if (DataCenter._uiSetting.IsEnableWAFPath01)
                {
                    MPIFile.CopyFile(csvResultOutPathAndFile, sorterOutputPathAndFile01);
                }

                if (DataCenter._uiSetting.IsEnableWAFPath02)
                {
                    MPIFile.CopyFile(csvResultOutPathAndFile, sorterOutputPathAndFile02);
                }

                if (DataCenter._uiSetting.IsEnableWAFPath03)
                {
                    MPIFile.CopyFile(csvResultOutPathAndFile, sorterOutputPathAndFile03);
                }

                Console.WriteLine("[Storage], SaveWAFReport() , End Copy File");
            }

            //---------------------------------------------------------------------------------
            // Save Statistics Report & Copy file to taget path
            //---------------------------------------------------------------------------------
            string reportFileWithExt = string.Format("{0}{1}", "Format-", ((int)DataCenter._uiSetting.UserID).ToString("0000")) + ".txt";

            string reportPathAndFile = Path.Combine(Constants.Paths.USER_DIR, reportFileWithExt);

            string outFileName = string.Format("{0}.{1}", DataCenter._uiSetting.TestResultFileName, DataCenter._uiSetting.STATTestResultFileExt);

            if (DataCenter._uiSetting.UserID == EUserID.HCSemiTek)
            {
                outFileName = string.Format("{0}.{1}", DataCenter._uiSetting.TestResultFileName + "-Data", DataCenter._uiSetting.STATTestResultFileExt);
            }

            string temp = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, Constants.Files.STATISTICS_TEMP);

            string StatisticsOutPath01 = DataCenter._uiSetting.STATOutputPath01;

            string StatisticsOutPath02 = DataCenter._uiSetting.STATOutputPath02;

            string StatisticsOutPath03 = DataCenter._uiSetting.STATOutputPath03;

            if (DataCenter._uiSetting.STATTesterResultCreatFolderType01 == ETesterResultCreatFolderType.ByLotNumber)
            {
                StatisticsOutPath01 = Path.Combine(StatisticsOutPath01, DataCenter._uiSetting.LotNumber);
            }
            else if (DataCenter._uiSetting.STATTesterResultCreatFolderType01 == ETesterResultCreatFolderType.ByMachineName)
            {
                StatisticsOutPath01 = Path.Combine(StatisticsOutPath01, DataCenter._uiSetting.MachineName);
            }
            else if (DataCenter._uiSetting.STATTesterResultCreatFolderType01 == ETesterResultCreatFolderType.ByDataTime)
            {
                StatisticsOutPath01 = Path.Combine(StatisticsOutPath01, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
            }

            if (DataCenter._uiSetting.STATTesterResultCreatFolderType02 == ETesterResultCreatFolderType.ByLotNumber)
            {
                StatisticsOutPath02 = Path.Combine(StatisticsOutPath02, DataCenter._uiSetting.LotNumber);
            }
            else if (DataCenter._uiSetting.STATTesterResultCreatFolderType02 == ETesterResultCreatFolderType.ByMachineName)
            {
                StatisticsOutPath02 = Path.Combine(StatisticsOutPath02, DataCenter._uiSetting.MachineName);
            }
            else if (DataCenter._uiSetting.STATTesterResultCreatFolderType02 == ETesterResultCreatFolderType.ByDataTime)
            {
                StatisticsOutPath02 = Path.Combine(StatisticsOutPath02, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
            }

            if (DataCenter._uiSetting.STATTesterResultCreatFolderType03 == ETesterResultCreatFolderType.ByLotNumber)
            {
                StatisticsOutPath03 = Path.Combine(StatisticsOutPath03, DataCenter._uiSetting.LotNumber);
            }
            else if (DataCenter._uiSetting.STATTesterResultCreatFolderType03 == ETesterResultCreatFolderType.ByMachineName)
            {
                StatisticsOutPath03 = Path.Combine(StatisticsOutPath03, DataCenter._uiSetting.MachineName);
            }
            else if (DataCenter._uiSetting.STATTesterResultCreatFolderType03 == ETesterResultCreatFolderType.ByDataTime)
            {
                StatisticsOutPath03 = Path.Combine(StatisticsOutPath03, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
            }

            string StatisticsOutPathAndFile01 = Path.Combine(StatisticsOutPath01, outFileName);

            string StatisticsOutPathAndFile02 = Path.Combine(StatisticsOutPath02, outFileName);

            string StatisticsOutPathAndFile03 = Path.Combine(StatisticsOutPath03, outFileName);

            if (File.Exists(reportPathAndFile))
            {
                this.SaveStatisticsReport(reportPathAndFile, temp);

                Console.WriteLine("[Storage], SaveStatisticsReport() ," + temp);

                if (DataCenter._uiSetting.IsEnableSTATPath01)
                {
                    MPIFile.CopyFile(temp, StatisticsOutPathAndFile01);
                }

                if (DataCenter._uiSetting.IsEnableSTATPath02 && (StatisticsOutPathAndFile02 != StatisticsOutPathAndFile01))
                {
                    if (DataCenter._uiSetting.UserID == EUserID.HCSemiTek
                        || DataCenter._uiSetting.UserID == EUserID.Lumitek)
                    {
                        if (isEndTest)
                        {
                            MPIFile.CopyFile(temp, StatisticsOutPathAndFile02);
                        }
                    }
                    else
                    {
                        MPIFile.CopyFile(temp, StatisticsOutPathAndFile02);
                    }
                }

                if (DataCenter._uiSetting.IsEnableSTATPath03 && (StatisticsOutPathAndFile03 != StatisticsOutPathAndFile01)
                                                             && (StatisticsOutPathAndFile03 != StatisticsOutPathAndFile02))
                {
                    if (DataCenter._uiSetting.UserID == EUserID.HCSemiTek
                         || DataCenter._uiSetting.UserID == EUserID.Lumitek)
                    {
                        if (isEndTest)
                        {
                            MPIFile.CopyFile(temp, StatisticsOutPathAndFile03);
                        }
                    }
                    else
                    {
                        MPIFile.CopyFile(temp, StatisticsOutPathAndFile03);
                    }
                }
            }

            //---------------------------------------------------------------------------------
            // Copy file By TaskSheet
            //---------------------------------------------------------------------------------
            if (DataCenter._uiSetting.IsTestResultPathByTaskSheet && DataCenter._product.TestResultPathByTaskSheet != string.Empty)
            {
                //一般報表
                string outPathByTaskSheet = DataCenter._product.TestResultPathByTaskSheet;

                if (DataCenter._uiSetting.ByRecipeResultCreateFolderType == ETesterResultCreatFolderType.ByLotNumber)
                {
                    outPathByTaskSheet = Path.Combine(outPathByTaskSheet, DataCenter._uiSetting.LotNumber);
                }
                else if (DataCenter._uiSetting.ByRecipeResultCreateFolderType == ETesterResultCreatFolderType.ByMachineName)
                {
                    outPathByTaskSheet = Path.Combine(outPathByTaskSheet, DataCenter._uiSetting.MachineName);
                }
                else if (DataCenter._uiSetting.ByRecipeResultCreateFolderType == ETesterResultCreatFolderType.ByDataTime)
                {
                    outPathByTaskSheet = Path.Combine(outPathByTaskSheet, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
                }

                string outFileFullName = Path.Combine(outPathByTaskSheet, fileNameWithExt);

                string sourceFile = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, Constants.Files.OUTPUT_CSV_TEMP);

                MPIFile.CopyFile(sourceFile, outFileFullName);

                //Sorter報表
                string sorterSourceFile = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, Constants.Files.OUTPUT_CSV_TEMP_SORTER);

                string sorterFormat = Path.Combine(Constants.Paths.USER_DIR, this._xslFileName + "-Sorter.xslt");

                if (File.Exists(sorterSourceFile) && File.Exists(sorterFormat))
                {
                    string sorterOutFileFullName = Path.Combine(DataCenter._product.TestResultPathByTaskSheet, fileNameWithoutExt + ".WAF");

                    MPIFile.CopyFile(sorterSourceFile, sorterOutFileFullName);
                }

                //統計報表
                if (File.Exists(reportPathAndFile))
                {
                    string staticOutFileFullName = Path.Combine(DataCenter._product.TestResultPathByTaskSheet, outFileName);

                    MPIFile.CopyFile(temp, staticOutFileFullName);
                }
            }

            DataCenter._uiSetting.IsManualRunMode = false;

            return true;
        }

        private void SaveErrMsrtValueToCSV()
        {
            // Gilbert, 20121223, 儲存量測項目中的 RawDataArray
            Console.WriteLine("[Storage], SaveErrMsrtValueToCSV()");

            if (DataCenter._uiSetting.IsEnableSaveErrMsrt == false)
                return;

            string dir = DataCenter._uiSetting.AbsoluteSpectrumPath;

            if (!MPIFile.CreateDirectory(dir))
			{
				Host.SetErrorCode(EErrorCode.SaveFileFail);

				return;
			}

            string fileAndPath = Path.Combine(dir, DataCenter._uiSetting.TestResultFileName + "." + "msrt");

            double[][] csvData = AppSystem._rawDataList.ToArray();

            CSVUtil.WriteCSV(fileAndPath, false, AppSystem._rawDataHead);
            CSVUtil.WriteToCSV(fileAndPath, true, csvData);

        }

        private void SaveSpectrumArrayToCSV()
        {
            Console.WriteLine("[Storage], SaveSpectrumArrayToCSV()");

            AppSystem._outputBigData.SaveSpectrumData(DataCenter._uiSetting.AbsoluteSpectrumPath, DataCenter._uiSetting.TestResultFileName);
        }

        private bool WriteStatisticsDataToXmlHead()
        {
            this._statisticsData.Clear();

            string pathAndFileWithExt = string.Format("{0}{1}", "User", ((int)DataCenter._uiSetting.UserID).ToString("0000")) + ".xml";

            string pathAndFile = Path.Combine(Constants.Paths.USER_DIR, pathAndFileWithExt);

            /////////////////////////////////////////////////////////////////////////////
            //Get User Defined Data
            /////////////////////////////////////////////////////////////////////////////
            XmlDocument xmlUser = new XmlDocument();

            xmlUser.Load(pathAndFile);

            XmlNode factorNode = xmlUser.SelectSingleNode("/UserDefine/Formats/Format/MsrtDisplayItem/C[@name='Factor']");

            if (factorNode != null)
            {
                string Factor = factorNode.InnerXml;

                this._statisticsData.Add("Factor", Factor);
            }

            XmlNode rsNode = xmlUser.SelectSingleNode("/UserDefine/Formats/Format/MsrtDisplayItem/C[@name='Rs(ohm)']");

            if (rsNode != null)
            {
                string Rs = rsNode.InnerXml;

                this._statisticsData.Add("Rs", Rs);
            }

            this._statisticsData.Add("Barcode", DataCenter._uiSetting.Barcode);

            this._statisticsData.Add("LotNumber", DataCenter._uiSetting.LotNumber);

            this._statisticsData.Add("StartTimeCht", DataCenter._sysSetting.StartTestTime.ToString());

            this._statisticsData.Add("EndTimeCht", DataCenter._sysSetting.EndTestTime.ToString());

            this._statisticsData.Add("StartTime", DataCenter._sysSetting.StartTestTime.ToString("yyMMddHHmmss"));

            this._statisticsData.Add("EndTime", DataCenter._sysSetting.EndTestTime.ToString("yyMMddHHmmss"));

            this._statisticsData.Add("EndTime02", DataCenter._sysSetting.EndTestTime.ToString("yyyy-MM-dd HH:mm:ss"));

            this._statisticsData.Add("OperatorName", DataCenter._uiSetting.OperatorName);

            this._statisticsData.Add("TaskSheetFileName", DataCenter._uiSetting.TaskSheetFileName);

            this._statisticsData.Add("MachineName", DataCenter._uiSetting.MachineName);

            this._statisticsData.Add("ImportCalibrateFileName", DataCenter._uiSetting.ImportCalibrateFileName);

            this._statisticsData.Add("waferNumer", DataCenter._uiSetting.WaferNumber);

            this._statisticsData.Add("ProductType", DataCenter._uiSetting.ProductType);

            this._statisticsData.Add("WaferPcs", DataCenter._uiSetting.WaferPcs.ToString("00"));

            this._statisticsData.Add("Tab", "\t");


            if (DataCenter._tempCond.TestItemArray == null)
            {
                return true;
            }

            /////////////////////////////////////////////////////////////////////////////
            // Add Test Item Data
            /////////////////////////////////////////////////////////////////////////////
            TestItemData[] testItems = DataCenter._product.TestCondition.TestItemArray;

            if (testItems == null)
            {
                return true;
            }

            foreach (TestItemData item in testItems)
            {
                string Value = "";

                if (item.Type == ETestType.ESD)
                {
                    if (item.IsEnable)
                    {
                        Value = Math.Abs((item as ESDTestItem).EsdSetting.ZapVoltage).ToString();
                    }
                    else
                    {
                        Value = "0";
                    }

                    this._statisticsData.Add(item.KeyName, Value);

                    continue;
                }

                if (item.ElecSetting == null)
                {
                    continue;
                }

                if (item.IsEnable)
                {
                    Value = Math.Abs(item.ElecSetting[0].ForceValue).ToString();
                }
                else
                {
                    Value = "0";
                }

                this._statisticsData.Add(item.KeyName, Value);

                if (item.Type == ETestType.IF)
                {
                    string Value2 = (1000 * Math.Abs(item.ElecSetting[0].ForceValue)).ToString();

                    this._statisticsData.Add(item.KeyName + "_uA", Value2);
                }
            }

            /////////////////////////////////////////////////////////////////////////////
            // Add TestResultData
            /////////////////////////////////////////////////////////////////////////////
            TestResultData[] outputTestResult = DataCenter._acquireData.OutputTestResult.ToArray();

            Dictionary<string, Maths.Statistic> statisticDataS = new Dictionary<string, Maths.Statistic>();

            Dictionary<string, Maths.Statistic> statisticDataS2 = new Dictionary<string, Maths.Statistic>();

            Dictionary<string, Maths.Statistic> statisticDataA = new Dictionary<string, Maths.Statistic>();

            Dictionary<string, Maths.Statistic> statisticDataA2 = new Dictionary<string, Maths.Statistic>();

            int isAllPassIndex = 0;

            int isAllPassIndex2 = 0;

            for (int i = 0; i < outputTestResult.Length; i++)
            {
                if (outputTestResult[i].KeyName == "ISALLPASS")
                {
                    isAllPassIndex = i;
                }

                if (outputTestResult[i].KeyName == "ISALLPASS02")
                {
                    isAllPassIndex2 = i;
                }

                statisticDataS.Add(outputTestResult[i].KeyName, new Maths.Statistic());

                statisticDataS2.Add(outputTestResult[i].KeyName, new Maths.Statistic());

                statisticDataA.Add(outputTestResult[i].KeyName, new Maths.Statistic());

                statisticDataA2.Add(outputTestResult[i].KeyName, new Maths.Statistic());
            }

            int chipCount = 0;

            uint over = 0;

            uint under = 0;

            List<int> skipIndex = new List<int>();

                chipCount = AppSystem._bodyDataCount;

            int goodDieNum = 0;

            for (int i = 0; i < chipCount; i++)
            {
                float[] dataList = null;

                    dataList = AppSystem._bodyDataList[i];

                if (dataList != null)
                {

                    if (dataList.Length < DataCenter._acquireData.OutputTestResult.Count)
                    {
                        return false;
                    }

                    for (int j = 0; j < DataCenter._acquireData.OutputTestResult.Count; j++)
                    {
                        TestResultData item = DataCenter._acquireData.OutputTestResult[j];

                        int digits = item.Formate.Length - 2;

                        if (digits < 0)
                        {
                            digits = 0;
                        }

                        dataList[j] = (float)Math.Round(dataList[j], digits);

                        if (item.IsEnable == false || item.IsVision == false)
                        {
                            continue;
                        }

                        if (item.IsVerify)
                        {
                            if (dataList[j] < item.MinLimitValue)
                            {
                                under++;
                            }
                            else if (dataList[j] > item.MaxLimitValue)
                            {
                                over++;
                            }

                            // in Spec boundary
                            if (dataList[j] >= item.MinLimitValue && dataList[j] <= item.MaxLimitValue)
                            {
                                statisticDataS[item.KeyName].Push(dataList[j]);
                            }

                            // in Spec2 boundary
                            if (dataList[j] >= item.MinLimitValue2 && dataList[j] <= item.MaxLimitValue2)
                            {
                                statisticDataS2[item.KeyName].Push(dataList[j]);
                            }
                        }

                        // Spec1 All
                        if (dataList[isAllPassIndex] == 1.0d)
                        {
                            statisticDataA[item.KeyName].Push(dataList[j]);
                        }

                        // Spec2 All
                        if (dataList[isAllPassIndex2] == 1.0d)
                        {
                            statisticDataA2[item.KeyName].Push(dataList[j]);
                        }
                    }
                }
            }

            foreach (var item in outputTestResult)
            {
                ////////////////////////////////////////////////////////////////
                // 單獨統計, Over: 大於最大值晶粒數, Under: 小於最小值晶粒數
                ////////////////////////////////////////////////////////////////
                this._statisticsData.Add(item.KeyName + "-Over", over.ToString());

                this._statisticsData.Add(item.KeyName + "-Under", under.ToString());

                ////////////////////////////////////////////////////////////////
                // 單獨統計
                ////////////////////////////////////////////////////////////////
                this._statisticsData.Add(item.KeyName + "-MinS", statisticDataS[item.KeyName].Min.ToString(item.Formate));

                this._statisticsData.Add(item.KeyName + "-MaxS", statisticDataS[item.KeyName].Max.ToString(item.Formate));

                this._statisticsData.Add(item.KeyName + "-AvgS", statisticDataS[item.KeyName].Mean.ToString(item.Formate));

                this._statisticsData.Add(item.KeyName + "-StdS", statisticDataS[item.KeyName].STDEV.ToString(item.Formate));

                if (statisticDataS[item.KeyName].Count == 0)
                {
                    this._statisticsData.Add(item.KeyName + "-GRS", "00.00");
                }
                else
                {
                    this._statisticsData.Add(item.KeyName + "-GRS", ((double)statisticDataS[item.KeyName].Count * 100.0d / (double)(chipCount - skipIndex.Count)).ToString("00.00"));
                }

                ////////////////////////////////////////////////////////////////
                // 單獨統計2
                ////////////////////////////////////////////////////////////////
                this._statisticsData.Add(item.KeyName + "-Min2S", statisticDataS2[item.KeyName].Min.ToString(item.Formate));

                this._statisticsData.Add(item.KeyName + "-Max2S", statisticDataS2[item.KeyName].Max.ToString(item.Formate));

                this._statisticsData.Add(item.KeyName + "-Avg2S", statisticDataS2[item.KeyName].Mean.ToString(item.Formate));

                this._statisticsData.Add(item.KeyName + "-Std2S", statisticDataS2[item.KeyName].STDEV.ToString(item.Formate));

                if (statisticDataS[item.KeyName].Count == 0)
                {
                    this._statisticsData.Add(item.KeyName + "-GRS2", "00.00");
                }
                else
                {
                    this._statisticsData.Add(item.KeyName + "-GRS2", ((double)statisticDataS2[item.KeyName].Count * 100.0d / (double)(chipCount - skipIndex.Count)).ToString("00.00"));
                }

                ////////////////////////////////////////////////////////////////
                // 全部統計
                ////////////////////////////////////////////////////////////////
                this._statisticsData.Add(item.KeyName + "-MinA", statisticDataA[item.KeyName].Min.ToString(item.Formate));

                this._statisticsData.Add(item.KeyName + "-MaxA", statisticDataA[item.KeyName].Max.ToString(item.Formate));

                this._statisticsData.Add(item.KeyName + "-AvgA", statisticDataA[item.KeyName].Mean.ToString(item.Formate));

                this._statisticsData.Add(item.KeyName + "-StdA", statisticDataA[item.KeyName].STDEV.ToString(item.Formate));

                ////////////////////////////////////////////////////////////////
                // 全部統計2
                ////////////////////////////////////////////////////////////////
                this._statisticsData.Add(item.KeyName + "-MinA2", statisticDataA2[item.KeyName].Min.ToString(item.Formate));

                this._statisticsData.Add(item.KeyName + "-MaxA2", statisticDataA2[item.KeyName].Max.ToString(item.Formate));

                this._statisticsData.Add(item.KeyName + "-AvgA2", statisticDataA2[item.KeyName].Mean.ToString(item.Formate));

                this._statisticsData.Add(item.KeyName + "-StdA2", statisticDataA2[item.KeyName].STDEV.ToString(item.Formate));
            }

            /////////////////////////////////////////////////////////////////////////////
            //Get Produce Info.
            /////////////////////////////////////////////////////////////////////////////
            int goodDieCount2 = statisticDataA2.Count;

            int failDieCount2 = chipCount - statisticDataA2.Count;

            int chipTotalCount = (chipCount - skipIndex.Count);  // 全部晶粒數-重複晶粒數

            int numGoodDieCount = goodDieNum;

            string TestCount = chipTotalCount.ToString();

            string GoodDieCount = numGoodDieCount.ToString();

            string FailDieCount = (chipCount - skipIndex.Count - numGoodDieCount).ToString();

            string GoodRate = string.Empty;

            string FailRate;

            string FailRate2;

            string GoodRate2;

            if (TestCount != "0")
            {
                FailRate = ((100.0d * double.Parse(FailDieCount)) / double.Parse(TestCount)).ToString("00.00");

                FailRate2 = ((100.0d * (double)(failDieCount2)) / double.Parse(TestCount)).ToString("00.00");

                GoodRate2 = ((100.0d * (double)(goodDieCount2)) / double.Parse(TestCount)).ToString("00.00");

                GoodRate = ((100.0d * (double)(numGoodDieCount)) / double.Parse(TestCount)).ToString("00.00");
            }
            else
            {
                GoodRate = "00.00";

                FailRate = "00.00";

                FailRate2 = "00.00";

                GoodRate2 = "00.00";
            }

            this._statisticsData.Add("TestCount", TestCount);

            this._statisticsData.Add("GoodDieCount", GoodDieCount);

            this._statisticsData.Add("FailDieCount", FailDieCount);

            this._statisticsData.Add("GoodRate", GoodRate);

            this._statisticsData.Add("FailRate", FailRate);

            this._statisticsData.Add("GoodDieCount2", goodDieCount2.ToString());

            this._statisticsData.Add("FailDieCount2", failDieCount2.ToString());

            this._statisticsData.Add("GoodRate2", GoodRate2);

            this._statisticsData.Add("FailRate2", FailRate2);

            this._statisticsData.Add("Total", "100");

            //--------------------------------------------------------------------------------
            // Append "ItemData" element to XML
            //--------------------------------------------------------------------------------
            XmlNode rootNode = this._xmlDoc.DocumentElement;

            XmlElement itemDataElem = this._xmlDoc.CreateElement("StatisticsData");

            rootNode.AppendChild(itemDataElem);

            foreach (var item in this._statisticsData)
            {
                XmlElement itemData = this._xmlDoc.CreateElement(item.Key);

                itemData.InnerText = item.Value;

                itemDataElem.AppendChild(itemData);
            }

            Console.WriteLine("[Storage], WriteStatisticsDataToXmlHead() , Statistics Data Complete");

            return true;
        }
        /// <summary>
        /// Calculate percentile of a sorted data set
        /// </summary>
        private  double percentile(double[] sortedData, double percent)
        {
            if (sortedData.Length < 2)
                return 0.0d;

            if (percent >= 100.0d) return sortedData[sortedData.Length - 1];

            double position = (double)(sortedData.Length + 1) * percent / 100.0;
            double leftNumber = 0.0d, rightNumber = 0.0d;

            double n = percent / 100.0d * (sortedData.Length - 1) + 1.0d;

            if (position >= 1)
            {
                leftNumber = sortedData[(int)System.Math.Floor(n) - 1];
                rightNumber = sortedData[(int)System.Math.Floor(n)];
            }
            else
            {
                leftNumber = sortedData[0]; // first data
                rightNumber = sortedData[1]; // first data
            }

            if (leftNumber == rightNumber)
                return leftNumber;
            else
            {
                double part = n - System.Math.Floor(n);
                return leftNumber + part * (rightNumber - leftNumber);
            }
        } 

		private bool SaveStatisticsReport(string Openpath, string Savepath)
        {
			if (File.Exists(Openpath) == false)
			{
				return false;
			}

			return CSVUtil.ConverterUesrReport(Openpath, Savepath, (char)0x01, (char)0x02, this._statisticsData, DataCenter._product.LOPSaveItem.ToString());
        }

		private void BackupPrintFile()
		{
			try
			{
				string printFile = Path.Combine(Constants.Paths.BARCODE_PRINT_DIR, Constants.Files.BARCODE_PRINTER_SETTING_FILE);

				if (!File.Exists(printFile))
				{
					return;
				}

				string backupDir = Constants.Paths.BARCODE_PRINT_BACKUP_DIR;

				DirectoryInfo dirinfo = new DirectoryInfo(backupDir);

                if (!dirinfo.Exists)
                {
                    dirinfo.Create();
                }

				//Delete
				DirectoryInfo[] dirList = dirinfo.GetDirectories();

				foreach (DirectoryInfo item in dirList)
				{
					DateTime dateTime;

					if (!DateTime.TryParse(item.Name, out  dateTime))
						continue;

					//Calculate DateTime
					TimeSpan ts1 = new TimeSpan(dateTime.Ticks);
					TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
					TimeSpan ts = ts1.Subtract(ts2).Duration();

					if (ts.Days > 30)
					{
						this.MyDeleteDirectory(item.FullName);
					}
				}

				//Copy
				backupDir = Path.Combine(backupDir, DateTime.Now.ToString("yyyy-MM-dd"));

                dirinfo = new DirectoryInfo(backupDir);

                if (!dirinfo.Exists)
                {
                    dirinfo.Create();
                }

                string backupFileName = Path.Combine(backupDir, "Print_" + DateTime.Now.ToString("HHmmss") + ".xml");

                MPIFile.DeleteFile(backupFileName);

				MPIFile.CopyFile(printFile, backupFileName);
			}
			catch (IOException e)
			{
				Console.WriteLine("The file could not be opened because it was locked by another process. {0}", e.ToString());

				return;
			}
			catch (Exception E)
			{
				Console.WriteLine("The file could not be opened because it was locked by another process. {0}", E.ToString());

				return;
			}
		}

        private bool CalcSeperateSize()
        {
            Console.WriteLine("[Storage],CalcSeperateSize()");

            int memorySize = 0;
            //----------------------------------------------------------------------------------------------------------------------------------
			//  Asuume We Need 3404 KB For Header without Body Data, is't about 435,712 double.
			//  Assume We Need 68.314 B For Each Attribute In Row, it's about 8 double (mutiplier)
			//  This assume is estimated by translating a 6074.xtmp with header(300KB) and 12500 Datas, which is total 38,660 KB 
            //----------------------------------------------------------------------------------------------------------------------------------
			int headerOffset = 435712;
            int sizePerNodeOrAttribute = 8;
			float safeFactor = 1.1f;
			List<double> testMemory = null;
			this._seperateSize = 50000;

			//(Assume DataEx has Esd IR x 20 + VZ x 20 each row)
			int bodyCount = DataCenter._uiSetting.UserDefinedData.ResultItemNameDic.Count + 40;

			bool isOK = false;

			while (this._seperateSize > 500 && !isOK)
            {
                try
                {
					// this memorySize estimate how much double needs for create xmlDoc and translate by xslt
					//         =   (SetRowNums * attribuesPerRow * NumOfDoublePerAttribute + HeaderOffset) * safeFactor
					memorySize = (int)(((this._seperateSize * bodyCount) * sizePerNodeOrAttribute + headerOffset) * safeFactor);

                    Console.WriteLine("[Storage],CalcSeperateSize(), Seperate Size : {0}", this._seperateSize);

					Console.WriteLine("[Storage],CalcSeperateSize(), Body Nodes : {0} per row", bodyCount);

                    Console.WriteLine("[Storage],CalcSeperateSize(), Ausme Node Size : {0} double", sizePerNodeOrAttribute);

					Console.WriteLine("[Storage],CalcSeperateSize(), Ausme Use Memory Size Per Seperate File : {0} double", memorySize);

					testMemory = new List<double>(memorySize);

					for (int i = 0; i < memorySize; i++)
                    {
						testMemory.Add(0.123d);
                    }

					isOK = true;
                }
                catch 
                {
					if (testMemory != null)
					{
						testMemory.Clear();

						testMemory = null;
					}

                    this._seperateSize /= 2;

                    Console.WriteLine("[Storage],CalcSeperateSize(), catch and reduce seperate size : {0}", this._seperateSize);
                }
				finally
				{
					if (testMemory != null)
					{
						testMemory.Clear();

						testMemory = null;
            }

					GC.Collect();
				}

            }

			return isOK;
        }

		private void DeleteMESFile()
		{
			switch (DataCenter._uiSetting.UserID)
			{
				case EUserID.LPC00:
					{
						string testStartFilePath = Path.Combine(DataCenter._uiSetting.MESPath, "StartTest.csv");

						MPIFile.DeleteFile(testStartFilePath);
					}
					break;
				default:
					break;
			}
		}

        //private void SaveSpectrumArrayToCSV()
        //{
        //    AppSystem._outputBigData.SaveSpectrumData(DataCenter._uiSetting.AbsoluteSpectrumPath, DataCenter._uiSetting.TestResultFileName);
        //}

		private void SaveAllSweepData()
		{
            if (GlobalFlag.IsEnableEndTest == true)
            {
                string savePath = DataCenter._uiSetting.SweepOutputPath;

                bool result = AppSystem._outputBigData.SaveSweepData(savePath, DataCenter._uiSetting.TestResultFileName, 0, AppSystem._outputBigData.OutputSweepList.Count);

                if (!result)
                {
                    Host.SetErrorCode(EErrorCode.SaveFileFail);

                    return;
                }
            }
		}

        private void SaveLIVRawDataToCSV()
        {
            string savePath = DataCenter._uiSetting.LIVDataSavePath;

            bool result = AppSystem._outputBigData.SaveLIVData(savePath, DataCenter._uiSetting.TestResultFileName, 0, AppSystem._outputBigData.OutputLIVList.Count);

            if (!result)
            {
                Host.SetErrorCode(EErrorCode.SaveFileFail);

                return;
            }
        }

        private void SavePIVRawDataToCSV()
        {
            string savePath = DataCenter._uiSetting.LIVDataSavePath;

            bool result = AppSystem._outputBigData.SavePIVData(savePath, DataCenter._uiSetting.TestResultFileName, 0, AppSystem._outputBigData.OutputPIVList.Count);

            if (!result)
            {
                Host.SetErrorCode(EErrorCode.SaveFileFail);

                return;
            }
        }

        private void AutoPopReportCommentsUI()
        {

            if (DataCenter._uiSetting.IsShowReportCommentsUI && GlobalFlag.IsEnableEndTest == false)
            {
                DataCenter._uiSetting.ReportComments = string.Empty;

                Console.WriteLine("[Storage], AutoPopReportCommentsUI()");

                if (this._frmReportComments == null || this._frmReportComments.IsDisposed)
                {
                    this._frmReportComments = new frmReportComments();
                }


                if (_cmdThread == null)
                {
                    _cmdThread = new Thread(Showdialog);
                    _cmdThread.Start();
                }
                else if (!_cmdThread.IsAlive)
                {
                    _cmdThread.Abort();
                    _cmdThread = new Thread(Showdialog);
                    _cmdThread.Start();
                }

            }
            else
            {
                GlobalFlag.IsEnableEndTest = true;
            }
        }

        private void Showdialog()
        {
            GlobalFlag.IsEnableEndTest = false;
            this._frmReportComments.ShowDialog();

            this._frmReportComments.Close();

            this._frmReportComments.Dispose();

            System.Threading.Thread.Sleep(500);
            GlobalFlag.IsEnableEndTest = true;
        }

		#endregion

		#region >>> Public Method <<<

		public void GenerateOutputFileName()
		{
			DataCenter._sysSetting.StartTestTime = DateTime.Now;

            DataCenter._uiSetting.TestResultFileName = "";
			
            switch (DataCenter._uiSetting.FileNameFormatPresent)
            {
                case (int)EOutputFileNamePresent.WaferNum:
                    DataCenter._uiSetting.TestResultFileName = DataCenter._uiSetting.WaferNumber;
                    break;
                //-------------------------------------------------------------------------
                case (int)EOutputFileNamePresent.BarCode:
                    DataCenter._uiSetting.TestResultFileName = DataCenter._uiSetting.Barcode;
                    break;
                //-------------------------------------------------------------------------
                case (int)EOutputFileNamePresent.LotSpaceWafer:
                    DataCenter._uiSetting.TestResultFileName = DataCenter._uiSetting.LotNumber + " " + DataCenter._uiSetting.WaferNumber;
                    break;
                //-------------------------------------------------------------------------
                case (int)EOutputFileNamePresent.LotNum_WaferNum:
                    DataCenter._uiSetting.TestResultFileName = DataCenter._uiSetting.LotNumber + "_" + DataCenter._uiSetting.WaferNumber;
                    break;
                    
                //-------------------------------------------------------------------------
                case (int)EOutputFileNamePresent.WaferNum_Stage:
                    DataCenter._uiSetting.TestResultFileName = DataCenter._uiSetting.WaferNumber;
                    if (DataCenter._product.TestCondition != null)
                    {
                        DataCenter._uiSetting.TestResultFileName += "_" + DataCenter._product.TestCondition.TestStage.ToString();
                    }
                    break;
                //-------------------------------------------------------------------------
                case (int)EOutputFileNamePresent.Customer01:
                    DataCenter._uiSetting.TestResultFileName = ParseOutputFileName(1);
                    break;
				//-------------------------------------------------------------------------
				case (int)EOutputFileNamePresent.Customer02:
					DataCenter._uiSetting.TestResultFileName = ParseOutputFileName(2);
					break;
				//-------------------------------------------------------------------------
                default:
					DataCenter._uiSetting.TestResultFileName = DataCenter._uiSetting.Barcode;
                    break;
            }

            if (DataCenter._uiSetting.UIDisplayType == (int)EUIDisplayType.WMStartUI)
            {
                DataCenter._uiSetting.TestResultFileName=DataCenter._uiSetting.WeiminUIData.KeyInFileName;
            }
		}

        public bool CheckTestResultFolderIsReady()
        {
            string outPath01 = DataCenter._uiSetting.TestResultPath01;
            string outPath02 = DataCenter._uiSetting.TestResultPath02;
            string outPath03 = DataCenter._uiSetting.TestResultPath03;

            //Drive is Ready
			try
			{
				if (DataCenter._uiSetting.IsEnablePath01)
				{
                    DriveInfo driveInfo = new DriveInfo(outPath01);
					
                    if (!driveInfo.IsReady)
                    {
                        return false;
                    }
				}

				if (DataCenter._uiSetting.IsEnablePath02)
				{
					DriveInfo driveInfo = new DriveInfo(outPath02);

					if (!driveInfo.IsReady)
                    {
                        return false;
                    }
				}

				if (DataCenter._uiSetting.IsEnablePath03)
				{
					DriveInfo driveInfo = new DriveInfo(outPath03);

					if (!driveInfo.IsReady)
                    {
                        return false;
                    }
				}

                return true;
            }
			catch (Exception e)
			{
				Console.WriteLine("[Storage], CheckTestResultFolderIsReady()" + e.ToString());

				return false;
			}
        }

        public bool IsExistTestOutputFileName(bool isDeleteExistFile = false)
		{
			bool exist = false;
			string outPath01 = string.Empty;
			string outPath02 = string.Empty;
			string outPath03 = string.Empty;
			string fileNameWithExt = DataCenter._uiSetting.TestResultFileName + "." + DataCenter._uiSetting.TestResultFileExt;

            if (DataCenter._uiSetting.IsManualRunMode)
            {
				outPath01 = DataCenter._uiSetting.ManualOutputPath01;
				outPath02 = DataCenter._uiSetting.ManualOutputPath02;
				outPath03 = DataCenter._uiSetting.ManualOutputPath03;

				if (DataCenter._uiSetting.ManualOutputPathType01 == ETesterResultCreatFolderType.ByLotNumber)
				{
					outPath01 = Path.Combine(outPath01, DataCenter._uiSetting.LotNumber);
				}
				else if (DataCenter._uiSetting.ManualOutputPathType01 == ETesterResultCreatFolderType.ByMachineName)
				{
					outPath01 = Path.Combine(outPath01, DataCenter._uiSetting.MachineName);
				}
				else if (DataCenter._uiSetting.ManualOutputPathType01 == ETesterResultCreatFolderType.ByDataTime)
				{
					outPath01 = Path.Combine(outPath01, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
				}

				if (DataCenter._uiSetting.ManualOutputPathType02 == ETesterResultCreatFolderType.ByLotNumber)
				{
					outPath02 = Path.Combine(outPath02, DataCenter._uiSetting.LotNumber);
				}
				else if (DataCenter._uiSetting.ManualOutputPathType02 == ETesterResultCreatFolderType.ByMachineName)
				{
					outPath02 = Path.Combine(outPath02, DataCenter._uiSetting.MachineName);
				}
				else if (DataCenter._uiSetting.ManualOutputPathType02 == ETesterResultCreatFolderType.ByDataTime)
				{
					outPath02 = Path.Combine(outPath02, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
				}

				if (DataCenter._uiSetting.ManualOutputPathType03 == ETesterResultCreatFolderType.ByLotNumber)
				{
					outPath03 = Path.Combine(outPath03, DataCenter._uiSetting.LotNumber);
				}
				else if (DataCenter._uiSetting.ManualOutputPathType03 == ETesterResultCreatFolderType.ByMachineName)
				{
					outPath03 = Path.Combine(outPath03, DataCenter._uiSetting.MachineName);
				}
				else if (DataCenter._uiSetting.ManualOutputPathType03 == ETesterResultCreatFolderType.ByDataTime)
				{
					outPath03 = Path.Combine(outPath03, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
				}

				outPath01 = Path.Combine(outPath01, fileNameWithExt);
				outPath02 = Path.Combine(outPath02, fileNameWithExt);
				outPath03 = Path.Combine(outPath03, fileNameWithExt);

				if (File.Exists(outPath01) && DataCenter._uiSetting.IsEnableManualPath01)
                {
                exist = true;

					if (isDeleteExistFile)
					{
						MPIFile.DeleteFile(outPath01);
					}
                }

				if(DataCenter._uiSetting.IsRunDailyCheckMode)
				{

					exist = false;
				}


                // 如果日校正程序單點測皆為直接覆蓋

					 //exist = false;
                //else if (File.Exists(outPath02) && DataCenter._uiSetting.IsEnableManualPath02)
                //{
                //    exist = true;

                //    if (isDeleteExistFile)
                //    {
                //        MPIFile.DeleteFile(outPath02);
                //    }
                //}
                //else if (File.Exists(outPath03) && DataCenter._uiSetting.IsEnableManualPath03)
                //{
                //    exist = true;

                //    if (isDeleteExistFile)
                //    {
                //        MPIFile.DeleteFile(outPath03);
                //    }
                //}
            }
            else
            {
				outPath01 = DataCenter._uiSetting.TestResultPath01;
				outPath02 = DataCenter._uiSetting.TestResultPath02;
				outPath03 = DataCenter._uiSetting.TestResultPath03;

				if (DataCenter._uiSetting.TesterResultCreatFolderType01 == ETesterResultCreatFolderType.ByLotNumber)
                {
					outPath01 = Path.Combine(outPath01, DataCenter._uiSetting.LotNumber);
				}
				else if (DataCenter._uiSetting.TesterResultCreatFolderType01 == ETesterResultCreatFolderType.ByMachineName)
				{
					outPath01 = Path.Combine(outPath01, DataCenter._uiSetting.MachineName);
				}
				else if (DataCenter._uiSetting.TesterResultCreatFolderType01 == ETesterResultCreatFolderType.ByDataTime)
				{
					outPath01 = Path.Combine(outPath01, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
                }

				if (DataCenter._uiSetting.TesterResultCreatFolderType02 == ETesterResultCreatFolderType.ByLotNumber)
				{
					outPath02 = Path.Combine(outPath02, DataCenter._uiSetting.LotNumber);
				}
				else if (DataCenter._uiSetting.TesterResultCreatFolderType02 == ETesterResultCreatFolderType.ByMachineName)
				{
					outPath02 = Path.Combine(outPath02, DataCenter._uiSetting.MachineName);
				}
				else if (DataCenter._uiSetting.TesterResultCreatFolderType02 == ETesterResultCreatFolderType.ByDataTime)
				{
					outPath02 = Path.Combine(outPath02, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
				}

				if (DataCenter._uiSetting.TesterResultCreatFolderType03 == ETesterResultCreatFolderType.ByLotNumber)
				{
					outPath03 = Path.Combine(outPath03, DataCenter._uiSetting.LotNumber);
				}
				else if (DataCenter._uiSetting.TesterResultCreatFolderType03 == ETesterResultCreatFolderType.ByMachineName)
				{
					outPath03 = Path.Combine(outPath03, DataCenter._uiSetting.MachineName);
				}
				else if (DataCenter._uiSetting.TesterResultCreatFolderType03 == ETesterResultCreatFolderType.ByDataTime)
				{
					outPath03 = Path.Combine(outPath03, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
				}

				outPath01 = Path.Combine(outPath01, fileNameWithExt);
				outPath02 = Path.Combine(outPath02, fileNameWithExt);
				outPath03 = Path.Combine(outPath03, fileNameWithExt);

				if (File.Exists(outPath01) && DataCenter._uiSetting.IsEnablePath01)
                {
                    exist = true;

					if (isDeleteExistFile)
					{
						MPIFile.DeleteFile(outPath01);
                }
                }
				else if (File.Exists(outPath02) && DataCenter._uiSetting.IsEnablePath02)
                {
                    exist = true;

					if (isDeleteExistFile)
					{
						MPIFile.DeleteFile(outPath02);
					}
                }
				else if (File.Exists(outPath03) && DataCenter._uiSetting.IsEnablePath03)
                {
                    exist = true;

					if (isDeleteExistFile)
					{
						MPIFile.DeleteFile(outPath03);
					}
                }
            }

			return exist;
		}

        public bool SaveTestCoefficientToFile()
        {
            bool rtn = true;

            string pathAndFileWithExt = string.Format("{0}{1}", "Format", ((int)DataCenter._uiSetting.UserID).ToString("0000")) + "-A-Coefficient.xslt";

            string CoefficientFile = Path.Combine(Constants.Paths.USER_DIR, pathAndFileWithExt);

            if (!File.Exists(CoefficientFile))
                return true;
  
            DataCenter._sysSetting.EndTestTime = DateTime.Now;

            this._xmlDoc = new XmlDocument();
            string str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                         "<?xml-stylesheet type=\"text/xsl\"" +
                         " href=\"File.xslt\"?><TesterOutput></TesterOutput>";
            this._xmlDoc.LoadXml(str);

            rtn &= this.AppendUserDataToXml();
            rtn &= this.WriteReportHead();
            rtn &= this.WriteCoeffDataToXMLBody();

            if (rtn == true)
            {
                return this.UseCoefXsltToTransferFormatAndSaveFile(CoefficientFile);
            }
            return rtn;
        }
		public void SaveSweepRawData()
        {
            Console.WriteLine("[Storage],SaveRawData()");
            
            this.AutoPopReportCommentsUI();

            this.SaveSpectrumArrayToCSV();

            this.SaveAllSweepData();

            this.SaveLIVRawDataToCSV();

            this.SavePIVRawDataToCSV();
        }

        public bool SaveTestResultToFile(bool isEndTest)
		{
            this.AutoPopReportCommentsUI();
            
            Console.WriteLine("[Storage],SaveTestResultToFile() , isEndTest : " + isEndTest.ToString());

            if (!Report.ReportProcess.IsImplementSpectrumReport)
            {
                this.SaveSpectrumArrayToCSV();
            }

            if (!Report.ReportProcess.IsImplementSweepDataReport)
            {
                this.SaveAllSweepData();
            }

            if (!Report.ReportProcess.IsImplementLIVReport)
            {
                this.SaveLIVRawDataToCSV();
            }

            if (!Report.ReportProcess.IsImplementPIVDataReport)
            {
                this.SavePIVRawDataToCSV();
            }

            if (Report.ReportProcess.IsImplement)
            {
                return true;
            }

            if (DataCenter._uiSetting.IsEnableAutoClearMapAndCIEChart)
            {
                AppSystem.ClearMapAndCIEChart();
            }

            GC.Collect();

			bool rtn = true;

			DataCenter._sysSetting.EndTestTime = DateTime.Now;

            //---------------------------------------------------------------------------------------------
            // add by Alec for Check if Memory is Enough
            //---------------------------------------------------------------------------------------------
			//if (this.CalcSeperateSize() != true)
			//{
			//    Host.SetErrorCode(EErrorCode.NotEnoughMemoryToSaveReportFile;
			//    Console.WriteLine("[Storage],SaveTestResultToFile() , Not Enough Memory To Save Report File ");
			//    //return false;
			//}

			this._seperateSize = 10000;

			this._xmlDoc = new XmlDocument();

			string str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
						 "<?xml-stylesheet type=\"text/xsl\"" +
						 " href=\"File.xslt\"?><TesterOutput></TesterOutput>";

			this._xmlDoc.LoadXml(str);

			rtn &= this.AppendUserDataToXml();

			rtn &= this.WriteReportHead();

			//---------------------------------------------------------------------------------------------
            // add by Alec for Seperate XML
			//---------------------------------------------------------------------------------------------
            int magnification = 2;

            string sorterFile = Path.Combine(Constants.Paths.USER_DIR, this._xslFileName + "-Sorter.xslt");

			if (File.Exists(sorterFile))
			{
				magnification++;
			}

            int maxValue = (AppSystem._bodyDataList.Count / this._seperateSize + 2) * magnification + 1;

			this._frmProgressBar = new frmProgressBar();

            string progressBarTitle = string.Empty;

            if (DataCenter._uiSetting.MultiLanguage == (int)EMultiLanguage.CHT)
            {
                progressBarTitle = "檔案輸出中...請等待寫檔完畢後在操作";
            }
            else
            {
                progressBarTitle = "Creating report...Please wait until processing is complete";
            }

            this._frmProgressBar.Start(maxValue, progressBarTitle);

			System.Threading.Thread.Sleep(1000);

			this._frmProgressBar.TopMost = true;

			this._frmProgressBar.TopMost = false;

			rtn &= this.WriteDataToXmlBodyAndOutputXtmp();

			if (rtn == true)
			{
               // this.SaveSpectrumArrayToCSV();

                this.SaveErrMsrtValueToCSV();

                this.TransferXmlToOtmp();

				//this.SaveAllSweepData();

                //this.SaveLIVRawDataToCSV();

               // this.SavePIVRawDataToCSV();

				//---------------------------------------------------------------------------------------------
                // add by Alec for Seperate XML
				//---------------------------------------------------------------------------------------------
                this._runThread = new Thread(() => this.MergeOtmpFiles(Constants.Paths.LEDTESTER_TEMP_DIR, Constants.Files.OUTPUT_CSV_TEMP));

                this._runThread.Priority = ThreadPriority.BelowNormal;

                this._runThread.Start();

                this._runThread.Join();

				this._frmProgressBar.ProgressBarValue++;

                this._runThread = new Thread(() => this.MergeOtmpFiles(Constants.Paths.LEDTESTER_TEMP_DIR, Constants.Files.OUTPUT_CSV_TEMP_SORTER));

                this._runThread.Priority = ThreadPriority.BelowNormal;

                this._runThread.Start();

                this._runThread.Join();

				this._frmProgressBar.ProgressBarValue++;

				rtn = this.MoveFileToTarget(isEndTest);

				this._frmProgressBar.ProgressBarValue = this._frmProgressBar.ProgressBarMax;
			}

			this._frmProgressBar.Close();

			this._frmProgressBar.Dispose();

			Console.WriteLine("[Storage],SaveTestResultToFile() , rtn = " + rtn.ToString());

			this.DeleteMESFile();

			return rtn;
		}

		public void SaveReportHeadToFile(object o, EventArgs e)
		{
			bool rtn = true;

			DataCenter._sysSetting.StartTestTime = DateTime.Now;

			this._xmlDoc = new XmlDocument();

			string str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
						 "<?xml-stylesheet type=\"text/xsl\"" +
						 " href=\"File.xslt\"?><TesterOutput></TesterOutput>";

			this._xmlDoc.LoadXml(str);

			rtn &= this.AppendUserDataToXml();

			rtn &= this.WriteReportHead();

			rtn &= this.WriteDataToXmlBodyAndOutputXtmp(false);

			try
			{
				if (!Directory.Exists(Constants.Paths.MPI_SHARE_DIR))
				{
					Directory.CreateDirectory(Constants.Paths.MPI_SHARE_DIR);
				}

				string xmlHeadFile = Path.Combine(Constants.Paths.MPI_SHARE_DIR, Constants.Files.OUTPUT_XML_TEMP);

				XmlTextWriter xtw = new XmlTextWriter(xmlHeadFile, null);

				xtw.Formatting = System.Xml.Formatting.None;

				this._xmlDoc.Save(xtw);

				xtw.Close();
			}
			catch (IOException ex)
			{
				Console.WriteLine("The file could not be opened because it was locked by another process. {0}", ex.ToString());
			}
			catch (Exception E)
			{
				Console.WriteLine("The file could not be opened because it was locked by another process. {0}", E.ToString());
			}
		}

        public bool SaveReportHeadToFile()
        {
            bool rtn = true;

            this._xmlDoc = new XmlDocument();

            string str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                         "<?xml-stylesheet type=\"text/xsl\"" +
                         " href=\"File.xslt\"?><TesterOutput></TesterOutput>";

            this._xmlDoc.LoadXml(str);

            rtn &= this.AppendUserDataToXml();

            rtn &= this.WriteReportHead();

            try
            {
				if (!Directory.Exists(Constants.Paths.MPI_SHARE_DIR))
                {
					Directory.CreateDirectory(Constants.Paths.MPI_SHARE_DIR);
                }

				string xmlHeadFile = Path.Combine(Constants.Paths.MPI_SHARE_DIR, Constants.Files.OUTPUT_XML_TEMP);

                XmlTextWriter xtw = new XmlTextWriter(xmlHeadFile, null);

                xtw.Formatting = System.Xml.Formatting.None;

                this._xmlDoc.Save(xtw);

                xtw.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be opened because it was locked by another process. {0}", e.ToString());

                return false;
            }
            catch (Exception E)
            {
				Console.WriteLine("The file could not be opened because it was locked by another process. {0}", E.ToString());

                return false;
            }

            return rtn;
        }

		public void Open()
		{
			//_threadWriteData = new Thread(new ThreadStart (DataWriteThreadProc) );
			//_threadWriteData.Start();
		}
		
		public void Close()
		{
			
#if ( !DebugVer )
			this.MyDeleteFiles(Constants.Paths.LEDTESTER_TEMP_DIR);
#endif
			///_threadWriteData.Abort();
		}

        public bool PrintLabel(string[] data)
        {
			string printFile = Path.Combine(Constants.Paths.BARCODE_PRINT_DIR, Constants.Files.BARCODE_PRINTER_SETTING_FILE);

			string sourceDataFile = Path.Combine(Constants.Paths.MPI_SHARE_DIR, Constants.Files.OUTPUT_XML_TEMP);

			string PrintProgram = Path.Combine(Constants.Paths.BARCODE_PRINT_DIR, Constants.Files.BARCODE_PRINTER_PROGRAM);

			string PrintProgramLib = Path.Combine(Constants.Paths.BARCODE_PRINT_DIR, Constants.Files.BARCODE_PRINTER_PROGRAM_LIB);

			string dataFile = Path.Combine(Constants.Paths.BARCODE_PRINT_DIR, Constants.Files.OUTPUT_XML_TEMP);

			string xsltFile = Constants.Paths.BARCODE_PRINT_DIR;

			string id = ((int)DataCenter._uiSetting.UserID).ToString();

            if (DataCenter._product.BarcodePrintFormat != "")
            {
                xsltFile = Path.Combine(xsltFile, "PrintFormat" + DataCenter._product.BarcodePrintFormat.Replace("Format", id) + ".xslt");
            }
            else
            {
                xsltFile = Path.Combine(xsltFile, "PrintFormat" +  id + ".xslt");
            }

			if (!File.Exists(xsltFile))
			{
				xsltFile = Path.Combine(Constants.Paths.BARCODE_PRINT_DIR, "PrintFormat" + ((int)DataCenter._uiSetting.UserID).ToString("0000") + ".xslt");
			}

			Console.WriteLine("[Storage],PrintLabel(), xsltFile:" + xsltFile);

            //Check File Existed and copy file to "C:\MPI\LEDTester\Print\"
            if (!File.Exists(PrintProgram) || !File.Exists(PrintProgramLib))
            {
				Console.WriteLine("[Storage],PrintLabel(), PrintProgram || PrintProgramLib is not Exist!");

                Host.SetErrorCode(EErrorCode.PRINT_PrintProgramNotExist);

                return false;
            }

            if (!File.Exists(sourceDataFile))
            {
				Console.WriteLine("[Storage],PrintLabel(), sourceDataFile:" + sourceDataFile + ", is not Exist!");

                Host.SetErrorCode(EErrorCode.PRINT_SourceDataFileNotExist);

                return false;
            }

            if (!File.Exists(xsltFile))
            {
				Console.WriteLine("[Storage],PrintLabel(), xsltFile:" + xsltFile + ", is not Exist!");

                Host.SetErrorCode(EErrorCode.PRINT_XsltFileNotExist);

                return false;
            }

            MPIFile.DeleteFile(dataFile);

			if(!MPIFile.CopyFile(sourceDataFile, dataFile))
			{
				Host.SetErrorCode(EErrorCode.PRINT_SourceDataFileNotExist);

				return false;
			}

			int TubeNum = 0;
			int TubeBinNum = 0;
			int TubeCount = 0;
			string TubeID = "";
			string PartID = "";
			int tubePullNumber = 1;
			string tubePullDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

			if (data != null && data.Length >= 5)
			{
				int.TryParse(data[0], out TubeNum);
				int.TryParse(data[1], out TubeBinNum);
				int.TryParse(data[2], out TubeCount);
				TubeID = data[3];
				PartID = data[4];
				int.TryParse(data[5], out tubePullNumber);
			}

			Console.WriteLine("[Storage],PrintLabel(), TubeNum:" + TubeNum.ToString());

			Console.WriteLine("[Storage],PrintLabel(), TubeBinNum:" + TubeBinNum.ToString());

			Console.WriteLine("[Storage],PrintLabel(), TubeCount:" + TubeCount.ToString());

			Console.WriteLine("[Storage],PrintLabel(), TubeID:" + TubeID);

			Console.WriteLine("[Storage],PrintLabel(), PartID:" + PartID);

			Console.WriteLine("[Storage],PrintLabel(), TubePullNumber:" + tubePullNumber);

			Console.WriteLine("[Storage],PrintLabel(), TubePullDateTime:" + tubePullDateTime);

            //Insert Tube Information
            try
            {
                XmlDataDocument dataXmlDoc = new XmlDataDocument();

                dataXmlDoc.Load(dataFile);

                XmlNode item = dataXmlDoc.SelectSingleNode("//TesterOutput/ItemData");

                XmlElement itemData = dataXmlDoc.CreateElement("TubeNum");
                itemData.InnerText = TubeNum.ToString();
                item.AppendChild(itemData as XmlNode);

                itemData = dataXmlDoc.CreateElement("TubeBinNum");
                itemData.InnerText = TubeBinNum.ToString();
                item.AppendChild(itemData as XmlNode);

                itemData = dataXmlDoc.CreateElement("TubeCount");
                itemData.InnerText = TubeCount.ToString();
                item.AppendChild(itemData as XmlNode);

                itemData = dataXmlDoc.CreateElement("TubeID");
				itemData.InnerText = TubeID.ToString();
                item.AppendChild(itemData as XmlNode);

                itemData = dataXmlDoc.CreateElement("PartID");
				itemData.InnerText = PartID.ToString();
                item.AppendChild(itemData as XmlNode);

				itemData = dataXmlDoc.CreateElement("TubePullNumber");
				itemData.InnerText = tubePullNumber.ToString();
				item.AppendChild(itemData as XmlNode);

				itemData = dataXmlDoc.CreateElement("TubePullDateTime");
				itemData.InnerText = tubePullDateTime.ToString();
				item.AppendChild(itemData as XmlNode);

				string binName = TubeBinNum.ToString();

				foreach (var bin in DataCenter._smartBinning)
				{
					if (TubeBinNum == bin.BinNumber)
					{
						binName = bin.BinCode;

						break;
					}
				}
					
                itemData = dataXmlDoc.CreateElement("BinName");
                itemData.InnerText = TubeBinNum.ToString();
                item.AppendChild(itemData as XmlNode);

                dataXmlDoc.Save(dataFile);

                //Transform print file from Format.xslt to Print.xml
                XslTransform xsl = new XslTransform();
                xsl.Load(xsltFile);
                xsl.Transform(dataFile, printFile);

                this.BackupPrintFile();
            }
            catch
            {
                Host.SetErrorCode(EErrorCode.PRINT_XslTransformFail);
                return false;
            }

            //Call Print Program
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.Arguments = printFile;
            process.StartInfo.FileName = PrintProgram;
            process.StartInfo.WorkingDirectory = Path.GetDirectoryName(printFile);
            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

            try
            {
                if (!process.Start())
                {
                    return false;
                }

                process.WaitForExit(5000);
                return true;
            }
            catch
            {
                Host.SetErrorCode(EErrorCode.PRINT_PrintFail);
                return false;
            }
            finally
            {
                process.Dispose();
                process = null;
            }
        }

        public string CalcTarget()
        {
            //Get Date Time
            DateTime dt = DataCenter._sysSetting.StartTestTime;
            DateTime dtBase = new DateTime(2012, 2, 21, 0, 0, 0);

            //Calculate Target
            TimeSpan ts1 = new TimeSpan(dt.Ticks);
            TimeSpan ts2 = new TimeSpan(dtBase.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();

            int day = ts.Days % 256;

            if (dt.Hour >= 12)
            {
                day++;
            }

            return ((double)day * 52.34158).ToString();
        }

        #endregion
    }
}
