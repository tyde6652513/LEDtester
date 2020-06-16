using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using MPI.Tester.Data;
using MPI.Tester.MES.Data;


namespace MPI.Tester.MES.User.AquaLite
{
    class MESProcess : ProcessBase
    {
        private const string COND_FILE_NANE = "WATestCondition.csv";
        private bool _isRunDailyCheckMode;

        public MESProcess()
            : base()
        {

        }

        protected override EErrorCode OpenFileAndParse(UISetting uiSetting, MachineConfig machineConfig)
        {
            EErrorCode rtn = EErrorCode.NONE;

            string serverConditonFullPath = Path.Combine(uiSetting.MESPath, COND_FILE_NANE);
            string loaclConditonFullPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, COND_FILE_NANE);

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

            if (uiSetting.WaferNumber.Length < 5)
                return EErrorCode.MES_BarcodeError;

            //Ex: QC2FC1202-885_RP-H222904 -> Get "FC"
            // string fileKey = DataCenter._uiSetting.Barcode.Substring(0, 2);

            // string fileKey = DataCenter._uiSetting.Barcode.Remove(3);


            List<string[]> FileNames = Tool.ToolBox.ReadCSV(loaclConditonFullPath);

            if (FileNames == null)
            {
                return EErrorCode.MES_OpenFileError;
            }

            this._testerRecipeFileName = String.Empty;

            foreach (string[] key in FileNames)
            {
                if (key[0] == "SpecID" || uiSetting.WaferNumber.Length < key[0].Length)
                    continue;

                string fileKey = uiSetting.WaferNumber.Remove(key[0].Length);
                char[] subfileKey = uiSetting.WaferNumber.ToCharArray();
                int indexKeyChar = key[0].Length;

                if (fileKey == key[0] && subfileKey[indexKeyChar] == 'F')
                {
                    this._testerRecipeFileName = key[1];
                    break;
                }
            }

            if (this._testerRecipeFileName == String.Empty)
            {
                if (uiSetting.WaferNumber.Contains("GLD") || uiSetting.WaferNumber.Contains("Golden"))
                {
                    this._isRunDailyCheckMode = true;
                    rtn = EErrorCode.NONE;
                }
                else
                {
                    this._isRunDailyCheckMode = false;
                    rtn = EErrorCode.MES_NotMatchRecipe;
                }
            }
            else
            {
                rtn = EErrorCode.NONE;
            }

            if (!File.Exists(Path.Combine(uiSetting.ProductPath,this._testerRecipeFileName + "." + Constants.Files.TASK_SHEET_EXTENSION)))
            {
                return EErrorCode.MES_TargetRecipeNoExist;
            }

            switch (rtn)
            {
                case EErrorCode.NONE:
                    uiSetting.TaskSheetFileName = this._testerRecipeFileName;
                    uiSetting.IsRunDailyCheckMode = this._isRunDailyCheckMode;
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
    }
}
