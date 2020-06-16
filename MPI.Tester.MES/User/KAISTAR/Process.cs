using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;
using MPI.Tester.MES.Data;

namespace MPI.Tester.MES.User.KAISTAR
{
	class MESProcess : ProcessBase
	{
        protected override Tester.Data.EErrorCode OpenFileAndParse(Tester.Data.UISetting uiSetting, MachineConfig machineConfig)
		{
			string tasFileName = uiSetting.ProductType + uiSetting.LotNumber + uiSetting.Substrate;

			uiSetting.TaskSheetFileName = tasFileName;

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
