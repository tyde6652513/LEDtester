using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;

namespace MPI.Tester.Report.User.MPI
{
	class Report : ReportBase
	{
		public Report(List<object> objs, bool isReStatistic)
			: base(objs, isReStatistic)
		{
		}

		protected override void SetResultTitle()
		{
			this.SetResultTitleByDefault();
		}

		protected override EErrorCode WriteReportHeadByUser()
		{
			return this.WriteReportHeadByDefault();
		}

		protected override EErrorCode RewriteReportByUser()
		{
			return this.RewriteReportByDefault();
		}
	}
}
