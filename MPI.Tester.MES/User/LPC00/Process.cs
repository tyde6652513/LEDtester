using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using MPI.Tester.Data;
using MPI.Tester.MES.Data;

using System.Xml;
using System.Xml.Xsl;

namespace MPI.Tester.MES.User.LPC00
{
	class MESProcess : ProcessBase
	{
		private Tester.Data.UISetting _uiSetting;

		public MESProcess()
			: base()
		{
		}

        protected override Tester.Data.EErrorCode OpenFileAndParse(Tester.Data.UISetting uiSetting, MachineConfig machineConfig)
		{
			string tasFilePath = Path.Combine(uiSetting.MESPath, uiSetting.MachineName + ".TAS");

			if (!File.Exists(tasFilePath))
			{
				Console.WriteLine("[MESProcess], TAS File is not Exist:" + tasFilePath);

				return EErrorCode.MES_CondDataNotExist;
			}

			List<string[]> tasFile = CSVUtil.ReadCSV(tasFilePath);

			if (tasFile == null)
			{
				Console.WriteLine("[MESProcess], Read TAS File Fail:" + tasFilePath);

				return EErrorCode.MES_OpenFileError;
			}

			if (tasFile.Count < 0 || tasFile[0].Length < 10)
			{
				Console.WriteLine("[MESProcess], TAS File Format Error:" + tasFilePath);

				return EErrorCode.MES_ParseFormatError;
			}

			uiSetting.TaskSheetFileName = tasFile[0][0];

			uiSetting.WeiminUIData.SpecificationRemark = tasFile[0][1];

			uiSetting.ProductName = tasFile[0][2];

			uiSetting.WeiminUIData.DeviceNumber = tasFile[0][3];

			uiSetting.LotNumber = tasFile[0][4];

			uiSetting.OperatorName = tasFile[0][5];

            uiSetting.WeiminUIData.Remark01 = tasFile[0][6];

			uiSetting.ReporterName = tasFile[0][7];

			uiSetting.WeiminUIData.ClassNumber = tasFile[0][8];

            uiSetting.WeiminUIData.Remark02 = tasFile[0][9];

            // FC-24545HW	45X45/1|B@SA	LP-VC05-1304112BP-SC_53	FV5D411253	JAE2AA1-130515B02	261	  0V	            22	  A	   LPFBD45A2_FLF
            // 测试规格档	　 x                     外延号	                              基板号	          批号	                        工号	  Remark1　	　x　  x	   Remark2　
            // 0                   1                     2                                       3                  4                             5      6                7    8      9

 
			this._uiSetting = uiSetting;

			return EErrorCode.NONE;
		}

		protected override Tester.Data.EErrorCode ConverterToMPIFormat()
		{
			Console.WriteLine("[MESProcess], ConverterToMPIFormat");

			string xmlResultOutPathAndFile = Path.Combine(Constants.Paths.MPI_SHARE_DIR, Constants.Files.OUTPUT_XML_TEMP);

            string csvResultOutPathAndFile = Path.Combine(this._uiSetting.MESPath, "StartTest.csv");

			//////////////////////////////////////////
			// Save xtemp to C:\MPI\Share
			//////////////////////////////////////////
			MPIFile.DeleteFile(xmlResultOutPathAndFile);

			this.OnSaveReportHead();

			if (!File.Exists(xmlResultOutPathAndFile))
			{
				Console.WriteLine("[MESProcess], xmlResultOutPathAndFile is not Exist:" + xmlResultOutPathAndFile);

				return EErrorCode.MES_LoadTaskError;
			}

			//////////////////////////////////////////
			// Transfer Xml To CSV
			//////////////////////////////////////////
			string xslFileName = string.Empty;

			XmlNodeList nodes = this._uiSetting.UserDefinedData.XmlUserDefine.SelectNodes("/UserDefine/Formats/*");

			foreach (XmlNode node in nodes)
			{
				if ((node as XmlElement).GetAttribute("name") == this._uiSetting.FormatName)
				{
					xslFileName = (node as XmlElement).GetAttribute("file");
				}
			}

			XslCompiledTransform xsl = new XslCompiledTransform();

			xsl.Load(Path.Combine(Constants.Paths.USER_DIR, xslFileName + ".xslt"));

			xsl.Transform(xmlResultOutPathAndFile, csvResultOutPathAndFile);

			return EErrorCode.NONE;
		}

		protected override Tester.Data.EErrorCode SaveRecipeToFile()
		{
			string tasFilePath = Path.Combine(this._uiSetting.MESPath, this._uiSetting.MachineName + ".TAS");

			MPIFile.DeleteFile(tasFilePath);

			return EErrorCode.NONE;
		}
	}
}
