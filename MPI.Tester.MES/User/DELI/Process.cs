using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using MPI.Tester.Data;
using MPI.Tester.MES.Data;

using System.Xml;
using System.Xml.Xsl;

namespace MPI.Tester.MES.User.DELI
{
	class MESProcess : ProcessBase
	{
		private Tester.Data.UISetting _uiSetting;

		public MESProcess()
			: base()
		{
		}

        protected override EErrorCode OpenFileAndParse(Tester.Data.UISetting uiSetting, MachineConfig machineConfig)
		{				
			GlobalData.ProberRecipeName = "";

            try
            {
                string recipeName = uiSetting.LotNumber.Substring(1, 4); // 取第 2, 3, 4, 5 字元

                recipeName += uiSetting.LotNumber.Substring(13, 1); // 取第 14 字元

                uiSetting.TaskSheetFileName = recipeName;// +"." + Constants.Files.TASK_SHEET_EXTENSION;

                uiSetting.ProberRecipeName = recipeName;

                GlobalData.ProberRecipeName = recipeName;
            }
            catch
            {
                Console.WriteLine("[MESProcess], OpenFileAndParse, " + EErrorCode.MES_LoadTaskError.ToString());

                return EErrorCode.MES_LoadTaskError;
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
