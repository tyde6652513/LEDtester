using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using MPI.Tester.Data;
using MPI.Tester.MES.Data;
using System.Collections;

namespace MPI.Tester.MES.User.Sanan6138
{
    public class MESProcess : ProcessBase
    {
        protected override Tester.Data.EErrorCode OpenFileAndParse(Tester.Data.UISetting uiSetting, MachineConfig machineConfig)
        {
            uiSetting.Barcode = uiSetting.WeiminUIData.OutputFileName;

            string serverFilePath = Path.Combine(uiSetting.MESPath, uiSetting.Barcode + "-resorting.csv");

            string localFilePath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, "tmp.csv");

            if (!File.Exists(serverFilePath))
            {
                Console.WriteLine("[MESProcess], AutoRecipe File is not Exist:" + serverFilePath);

                return EErrorCode.MES_CondDataNotExist;
            }
   
            if (!MPIFile.CopyFile(serverFilePath, localFilePath))
            {
                Console.WriteLine("[MESProcess], Copy File Fail:" + localFilePath);

                return EErrorCode.MES_OpenFileError;
            }

            if (File.Exists(localFilePath))
            {
                MPIFile.DeleteFile(localFilePath);
            }

            List<string[]> file = CSVUtil.ReadCSV(serverFilePath);

            if (file == null)
            {
                Console.WriteLine("[MESProcess], Read File Fail:" + serverFilePath);

                return EErrorCode.MES_OpenFileError;
            }

            if (file.Count < 2 || file[0].Length < 12 || file[1].Length < 12)
            {
                Console.WriteLine("[MESProcess], File Format Error:" + serverFilePath);

                return EErrorCode.MES_ParseFormatError;
            }

            uiSetting.TaskSheetFileName = file[1][11];

            if (uiSetting.IsDeliverProberRecipe)
            {
                GlobalData.ProberRecipeName = "S_" + file[1][1] + "_H_4_";

                if (machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Die)
                {
                    GlobalData.ProberRecipeName += "8";
                }
                else
                {
                    GlobalData.ProberRecipeName += "2";
                }
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
    }
}
