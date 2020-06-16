using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using MPI.Tester.Data;
using MPI.Tester.MES.Data;

namespace MPI.Tester.MES.User.EverVision
{
	class MESProcess : ProcessBase
	{

        private string _companyKeyWord = string.Empty;

		public MESProcess()
			: base()
		{

		}

        protected override EErrorCode OpenFileAndParse(UISetting uiSetting, MachineConfig machineConfig)
		{
			//Barcode: "D30322362T130319362"

            this._companyKeyWord = string.Empty;

            if (uiSetting.Barcode.Contains("T"))
            {
                this._companyKeyWord = "T";
            }
            else if (uiSetting.Barcode.Contains("G"))
            {
                this._companyKeyWord = "G";
            }
            else if (uiSetting.Barcode.Contains("H"))
            {
                this._companyKeyWord = "H";
            }
            else if (uiSetting.Barcode.Contains("L"))
            {
                this._companyKeyWord = "L";
            }
            else if (uiSetting.Barcode.Contains("M"))
            {
                this._companyKeyWord = "M";
            }

			//Get "D30322362"

            if (this._companyKeyWord == string.Empty)
            {
                this._describe.AppendLine("找不到依照客戶定義的KeyWord");
                return EErrorCode.MES_BarcodeError;
            }

			string fileName = uiSetting.Barcode.Remove(uiSetting.Barcode.IndexOf(this._companyKeyWord)) + ".csv";

			string serverConditonFullPath = Path.Combine(uiSetting.MESPath, fileName);

			string loaclConditonFullPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, fileName);

			//Check File is Exist and Copy to Local
			DriveInfo driveInfo = new DriveInfo(serverConditonFullPath);

			if (!driveInfo.IsReady)
			{
				return EErrorCode.MES_ServerConnectFail;
			}

			if (File.Exists(serverConditonFullPath))
			{
				if (!MPIFile.DeleteFile(loaclConditonFullPath))
				{
					return EErrorCode.MES_SaveRecipeToFileError;
				}

				if (!MPIFile.CopyFile(serverConditonFullPath, loaclConditonFullPath))
				{
					return EErrorCode.MES_SaveRecipeToFileError;
				}
			}
			else
			{
				return EErrorCode.MES_ReferenceDataNotExist;
			}

			//Get File Info
			List<string[]> file = Tool.ToolBox.ReadCSV(loaclConditonFullPath);

			if (file == null)
			{
				return EErrorCode.MES_OpenFileError;
			}

			string productName = string.Empty;

			string customerRemark = string.Empty;

			string deviceNumber = string.Empty;

			string lotNumber = string.Empty;

			string customerNote1 = string.Empty;

			string customerNote3 = string.Empty;

			string ramark1 = string.Empty;

			string ramark2 = string.Empty;
			
			foreach (string[] line in file)
			{
				if (line.Length < 2)
				{
					continue;
				}

				if(line[0] == "P-Lot NO.")
				{
					productName = line[1];
				}
				else if (line[0] == "WIP NO.")
				{
					customerRemark = line[1];
				}
				else if (line[0] == "Part NO.")
				{
					deviceNumber = line[1];
				}
				else if (line[0] == "C-Lot NO.")
				{
					lotNumber = line[1];
				}
				else if (line[0] == "Test Condition File")
				{
					customerNote1 = line[1];
				}
				else if (line[0] == "Bin Table File")
				{
					customerNote3 = line[1];
				}
				else if (line[0] == "Product")
				{
					ramark2 = line[1];
				}
				else if (line[0] == "Model No.")
				{
					ramark1 = line[1];
				}
			}

			if (productName == string.Empty ||
				customerRemark == string.Empty ||
				deviceNumber == string.Empty ||
				lotNumber == string.Empty ||
				customerNote1 == string.Empty ||
				customerNote3 == string.Empty ||
				ramark2 == string.Empty ||
				ramark1 == string.Empty)
			{
				return EErrorCode.MES_ParseFormatError;
			}

			uiSetting.ProductName = productName;

            uiSetting.WeiminUIData.ProductName = productName;

			uiSetting.WeiminUIData.CustomerRemark01 = customerRemark;

			uiSetting.WeiminUIData.DeviceNumber = deviceNumber;

			uiSetting.LotNumber = lotNumber;

			uiSetting.WeiminUIData.CustomerNote01 = customerNote1;

			uiSetting.WeiminUIData.CustomerNote03 = customerNote3;

            uiSetting.WeiminUIData.CustomerNote02 = "ESD :P->N(MODEL=0,VOLTAGE=0,TIMES=0,INTERVAL=0)  N->P(MODEL=1,VOLTAGE=2500,TIMES=2,INTERVAL=0)";

			uiSetting.WeiminUIData.Remark02 = ramark2;

			uiSetting.WeiminUIData.Remark01 = ramark1;

            uiSetting.WeiminUIData.Remark03="%Report=" + ramark1;

            uiSetting.WeiminUIData.LotNumber = lotNumber;

            uiSetting.WeiminUIData.KeyInFileName = uiSetting.Barcode;

            uiSetting.WeiminUIData.CodeNumber = uiSetting.OperatorName;

			//Save Recipe Name
			//uiSetting.ProductPath = Path.GetDirectoryName(customerNote1).Replace("\\\\", "\\");

			uiSetting.TaskSheetFileName = Path.GetFileNameWithoutExtension(customerNote1);

			uiSetting.ProberRecipeName = ramark2;

			GlobalData.ProberRecipeName = ramark2;

			Console.WriteLine("[MESProcess], TaskSheetFileName, " + Path.GetFileNameWithoutExtension(fileName));

			Console.WriteLine("[MESProcess], ProberRecipeName, " + ramark2);

			//driveInfo = new DriveInfo(uiSetting.ProductPath);

			//if (!driveInfo.IsReady && !MPIFile.CreatDirectory(uiSetting.ProductPath))
			//{
			//    return EErrorCode.MES_SaveRecipeToFileError;
			//}

			return EErrorCode.NONE;
		}

		protected override EErrorCode ConverterToMPIFormat()
		{
			return EErrorCode.NONE;
		}

		protected override EErrorCode SaveRecipeToFile()
		{
			return EErrorCode.NONE;
		}
	}
}
