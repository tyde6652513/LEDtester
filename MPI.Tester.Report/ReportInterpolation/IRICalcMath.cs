using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;

namespace MPI.Tester.Report
{
	public interface IRICalcMath
	{
		bool ClacProcess(UISetting uiSetting, TesterSetting testerSetting, ProductData productData, string inputFileFullName, string outputFileFullName);
	}
}
