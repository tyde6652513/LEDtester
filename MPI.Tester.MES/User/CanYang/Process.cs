using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using MPI.Tester.Data;
using MPI.Tester.MES.Data;

namespace MPI.Tester.MES.User.CanYang
{
    class MESProcess : ProcessBase
    {
        private string FILE_EXTEND = ".csv";

        public MESProcess()
            : base()
        {
        }

        protected override EErrorCode OpenFileAndParse(UISetting uiSetting, MachineConfig machineConfig)
        {
            string serverConditonFullPath = Path.Combine(uiSetting.MESPath, uiSetting.Barcode + FILE_EXTEND);

            string loaclConditonFullPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, uiSetting.Barcode + FILE_EXTEND);

            if(!MPIFile.CopyFile(serverConditonFullPath, loaclConditonFullPath))
            {
                return EErrorCode.MES_CondDataNotExist;
            }

            List<string[]> contents = CSVUtil.ReadCSV(loaclConditonFullPath);

            if (contents == null || contents.Count < 1 || contents[0].Length < 3)
            {
                return EErrorCode.MES_OpenFileError;
            }

            uiSetting.LotNumber = contents[0][1];

            uiSetting.TaskSheetFileName = contents[0][2];

            uiSetting.ProberRecipeName = contents[0][2];

            GlobalData.ProberRecipeName = contents[0][2];
          
            uiSetting.WeiminUIData.SpecificationRemark = contents[0][2];

            Console.WriteLine("[MESProcess], LotNumber, " + contents[0][1]);

            Console.WriteLine("[MESProcess], TaskSheetFileName, " + contents[0][2]);

            Console.WriteLine("[MESProcess], ProberRecipeName, " + contents[0][2]);

            Console.WriteLine("[MESProcess], SpecificationRemark, " + contents[0][2]);

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
