using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;
using System.IO;

namespace MPI.Tester.MES.User.ChangeLight
{
	class MESProcess : ProcessBase
	{
        protected override EErrorCode OpenFileAndParse(UISetting uiSetting, MachineConfig machineConfig)
		{
			if (uiSetting.ImportCalibrateFileName == "")
			{
				return EErrorCode.MES_LoadCalibFileError;
			}

			int lotStart = 0;
			int lotEnd = 0;
			int SpecStart = 0;
			int SpecEnd = 0;

			if (!int.TryParse(uiSetting.WeiminUIData.Remark01, out lotStart) || lotStart == 0)
			{
				lotStart = 1;
				uiSetting.WeiminUIData.Remark01 = "1";
			}

			if (!int.TryParse(uiSetting.WeiminUIData.Remark02, out lotEnd) || lotEnd == 0)
			{
				lotEnd = 10;
				uiSetting.WeiminUIData.Remark02 = "10";
			}

			if (!int.TryParse(uiSetting.WeiminUIData.Remark03, out SpecStart) || SpecStart == 0)
			{
				SpecStart = 1;
				uiSetting.WeiminUIData.Remark03 = "1";
			}

			if (!int.TryParse(uiSetting.WeiminUIData.Remark04, out SpecEnd) || SpecEnd == 0)
			{
				SpecEnd = 10;
				uiSetting.WeiminUIData.Remark04 = "10";
			}

			//OutputFileName = "ASDFGHJKL"
			//lotStart = 3, lotEnd = 6
			//LotNum = "DFGH" //from index 3 to index 6
			if (uiSetting.Barcode.Length > (lotStart - 1) && lotEnd >= lotStart)
			{
				int startIndex = lotStart - 1;

				int len = lotEnd - lotStart + 1;

				if (len > uiSetting.Barcode.Length - startIndex)
				{
					len = uiSetting.Barcode.Length - startIndex;
				}

				uiSetting.WeiminUIData.CustomerNote01 = uiSetting.Barcode.Substring(startIndex, len);
			}
			else
			{
				uiSetting.WeiminUIData.CustomerNote01 = string.Empty;

				return EErrorCode.MES_BarcodeError;
			}

			if (uiSetting.Barcode.Length > (SpecStart - 1) && SpecEnd >= SpecStart)
			{
				int startIndex = SpecStart - 1;

				int len = SpecEnd - SpecStart + 1;

				if (len > uiSetting.Barcode.Length - startIndex)
				{
					len = uiSetting.Barcode.Length - startIndex;
				}

				uiSetting.WeiminUIData.CustomerNote02 = uiSetting.Barcode.Substring(startIndex, len);
			}
			else
			{
				uiSetting.WeiminUIData.CustomerNote02 = string.Empty;

				return EErrorCode.MES_BarcodeError;
			}

            uiSetting.WeiminUIData.OutputFileName = uiSetting.Barcode;

			if (uiSetting.WeiminUIData.WMTestMode == 3)
			{
				if (uiSetting.WaferNumber.Length > 2 && uiSetting.WaferNumber[0] == 'J' && uiSetting.WaferNumber[1] == 'K')
				{
					return EErrorCode.NONE;
				}
				else
				{
					return EErrorCode.MES_BarcodeError;
				}
			}

			if (uiSetting.MESMachineName == EEqModelName.P7602)
			{
			uiSetting.TaskSheetFileName = uiSetting.WeiminUIData.CustomerNote02;
			}
			else
			{
				//---------------------------------------------------------------------------------------------
				//20160304 Stanley 
				//if (uiSetting.WeiminUIData.WMTestMode != 2)
				//{
				//    uiSetting.TaskSheetFileName = uiSetting.WeiminUIData.CustomerNote02;
				//}

				//片号,状态,站点,Recipe,型号,抛档路径
				//B000803140,Run,IPQC,N82W0713B2G15C,CLAB0713BBC05WUF,\\192.168.0.86\MES Loader Data\Chip\IPQC\SourceFolder\

				string serverConditonFullPath = Path.Combine(uiSetting.MESPath, uiSetting.Barcode + ".csv");

				string loaclConditonFullPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, uiSetting.Barcode + ".csv");

				DriveInfo driveInfo = new DriveInfo(serverConditonFullPath);

				if (!driveInfo.IsReady)
				{
					return EErrorCode.MES_ServerConnectFail;
				}

				if (File.Exists(serverConditonFullPath))
				{
					MPIFile.DeleteFile(loaclConditonFullPath);

					MPIFile.CopyFile(serverConditonFullPath, loaclConditonFullPath);
				}
				else
				{
					return EErrorCode.MES_CondDataNotExist;
				}

				List<string[]> FileNames = Tool.ToolBox.ReadCSV(loaclConditonFullPath);

				if (FileNames == null)
				{
					return EErrorCode.MES_OpenFileError;
				}

				if (FileNames.Count < 2 || FileNames[0].Length < 5 || FileNames[1].Length < 5)
				{
					return EErrorCode.MES_ParseFormatError;
				}

				if (FileNames[1][0] != uiSetting.Barcode)
				{
					return EErrorCode.MES_ParseFormatError;
				}

				if (FileNames[1][1] != "Run")
				{
					return EErrorCode.MES_ParseFormatError;
				}

				uiSetting.TaskSheetFileName = FileNames[1][3];

				uiSetting.TestResultPath01 = FileNames[1][5];
			}
            
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
