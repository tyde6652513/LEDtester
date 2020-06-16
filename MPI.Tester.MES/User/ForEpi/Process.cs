using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using MPI.Tester.Data;
using MPI.Tester.MES.Data;

namespace MPI.Tester.MES.User.ForEpi
{
	class MESProcess : ProcessBase
	{
		public MESProcess()
			: base()
		{
		}

        protected override Tester.Data.EErrorCode OpenFileAndParse(Tester.Data.UISetting uiSetting, MachineConfig machineConfig)
		{
			string serverConditonFullPath = Path.Combine(uiSetting.MESPath, uiSetting.WeiminUIData.KeyInFileName + ".csv");

			string loaclConditonFullPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, uiSetting.WeiminUIData.KeyInFileName + ".csv");

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

			if (FileNames.Count == 0 || FileNames[0].Length < 8)
			{
				return EErrorCode.MES_ParseFormatError;
			}

			string remark1 = FileNames[0][0];

			string fileName = FileNames[0][1];

			string remark2 = FileNames[0][2];

			string productName = FileNames[0][3];

			string opName = FileNames[0][4];

			string classNumber = FileNames[0][5];

			string codeNumber = FileNames[0][6];

			string TestNumber = FileNames[0][7];

			if (!File.Exists(Path.Combine(uiSetting.ProductPath, TestNumber + "." + Constants.Files.TASK_SHEET_EXTENSION)))
			{
				return EErrorCode.MES_TargetRecipeNoExist;
			}

			if (!File.Exists(Path.Combine(uiSetting.ProductPath, TestNumber + "." + Constants.Files.PRODUCT_FILE_EXTENSION)))
			{
				return EErrorCode.MES_TargetRecipeNoExist;
			}

			if (!File.Exists(Path.Combine(uiSetting.ProductPath, TestNumber + "." + Constants.Files.BIN_FILE_EXTENSION)))
			{
				return EErrorCode.MES_TargetRecipeNoExist;
			}

			uiSetting.WeiminUIData.Remark01 = remark1;

			uiSetting.WeiminUIData.Remark02 = remark2;

			uiSetting.TaskSheetFileName = TestNumber;

			uiSetting.WeiminUIData.ClassNumber = classNumber;

			uiSetting.WeiminUIData.CodeNumber = codeNumber;

			uiSetting.WeiminUIData.KeyInFileName = fileName;

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
	}
}
