using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using MPI.Tester.Data;
using MPI.Tester.MES.Data;

namespace MPI.Tester.MES.User.EnRay
{
    class MESProcess : ProcessBase
    {
        public const string COND_FILE_NANE = "EnrayTestCondition.CSV";
        private string _WaferID ;
        private string _LotID;

        public MESProcess()
        {
            this._WaferID = String.Empty;
            this._LotID = String.Empty;
        }

        protected override EErrorCode OpenFileAndParse(UISetting uiSetting, MachineConfig machineConfig)
        {
            EErrorCode rtn = EErrorCode.NONE;
            string serverConditonFullPath = Path.Combine(uiSetting.MESPath, COND_FILE_NANE);
            string loaclConditonFullPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, COND_FILE_NANE);

            DriveInfo driveInfo = new DriveInfo(serverConditonFullPath);

            if (!driveInfo.IsReady)
            {
                Console.WriteLine("[MESProcess], OpenFileAndParse, " + EErrorCode.MES_ServerConnectFail.ToString());
                return EErrorCode.MES_ServerConnectFail;
            }

            if (File.Exists(serverConditonFullPath) == false)
            {
                Console.WriteLine("[MESProcess], OpenFileAndParse, " + EErrorCode.MES_CondDataNotExist.ToString());
                return EErrorCode.MES_CondDataNotExist;
            }


            if (uiSetting.Barcode.Length < 8)
                return EErrorCode.MES_BarcodeError;

            if (File.Exists(serverConditonFullPath))
            {

				MPIFile.DeleteFile(loaclConditonFullPath);

				MPIFile.CopyFile(serverConditonFullPath, loaclConditonFullPath);
            }

            try
            {
                //Ex: C23B02C01333#15-A2023791Q -> Get "C01333"
                List<string[]> FileNames = Tool.ToolBox.ReadCSV(loaclConditonFullPath);

                if (FileNames == null)
                {
                    Console.WriteLine("[MESProcess], OpenFileAndParse, " + EErrorCode.MES_OpenFileError.ToString());
                    return EErrorCode.MES_OpenFileError;
                }
                //string testerRecipefileName = String.Empty;
                //string proberRecipefileName = String.Empty;
                 string targerStr = uiSetting.Barcode.Substring(0, 6);
                 this._WaferID = uiSetting.Barcode.Remove(0, uiSetting.Barcode.IndexOf("-") + 1);
                 this._LotID = uiSetting.Barcode.Substring(6, uiSetting.Barcode.IndexOf("#") - 6);

                foreach (string[] key in FileNames)
                {
                    if (key[0] == targerStr)
                    {
                        if (key.Length >= 3)
                        {
                            this._testerRecipeFileName = key[1];
                            this._proberRecipeFileName = key[2];
                            break;
                        }
                    }
                }
                //rtn= EErrorCode.NONE;
            }
            catch
            {
                return EErrorCode.MES_BarcodeError;
            }

            if (this._testerRecipeFileName == String.Empty)
            {
                Console.WriteLine("[MESProcess], OpenFileAndParse, " + EErrorCode.MES_NotMatchRecipe.ToString());
                return EErrorCode.MES_NotMatchRecipe;
            }

            // Check Targat

            if (!File.Exists(Path.Combine(uiSetting.ProductPath, this._testerRecipeFileName + "." + Constants.Files.TASK_SHEET_EXTENSION)))
            {
                Console.WriteLine("[MESProcess], OpenFileAndParse, " + EErrorCode.MES_TargetRecipeNoExist.ToString());
                return EErrorCode.MES_TargetRecipeNoExist;
            }

            // Update Data To UI Setting

            switch (rtn)
            {
                case EErrorCode.NONE:
                    Console.WriteLine("[MESProcess], OpenFileAndParse, SUCCESS");
                    GlobalData.ProberRecipeName = "";
                    uiSetting.TaskSheetFileName = this._testerRecipeFileName;
                    uiSetting.ProberRecipeName =this._proberRecipeFileName ;
                    uiSetting.LotNumber = this._LotID;
                    uiSetting.WaferNumber = this._WaferID;
                    GlobalData.ProberRecipeName = this._proberRecipeFileName;
                    Console.WriteLine("[MESProcess], TaskSheetFileName, " + this._testerRecipeFileName);
                    Console.WriteLine("[MESProcess], ProberRecipeName, " + this._proberRecipeFileName);
                    Console.WriteLine("[MESProcess], LotNumber, " + this._LotID);
                    Console.WriteLine("[MESProcess], ProberRecipeName, " + this._WaferID);
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
