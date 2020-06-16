using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;

namespace MPI.Tester.MES
{
	public interface IMES
	{
        EErrorCode LoadRecipe(UISetting uiSetting, MachineConfig machineConfig, out string alarmMessage);

        EErrorCode UpLoadRecipe();
	}
}
