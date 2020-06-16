using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;
using MPI.Tester.MES.Data;

namespace MPI.Tester.MES
{
    public abstract class ProcessBase : IMES
	{
		#region >>> protected Property <<<

		public static event EventHandler SaveReportHead;

        protected string _testerRecipeFileName;
        protected string _proberRecipeFileName;
        protected string _testerRecipePath;
        protected StringBuilder _describe;

		#endregion

		#region >>> Constructor / Disposor <<<

		public ProcessBase()
        {
            this._testerRecipeFileName = String.Empty;
            this._proberRecipeFileName = String.Empty;
            this._testerRecipePath = String.Empty;
            this._describe = new StringBuilder();
        }

		#endregion

		#region >>> Public Virtual Method <<<

        public virtual EErrorCode LoadRecipe(UISetting uiSetting, MachineConfig machineConfig, out string alarmMsg)
        {
            EErrorCode rtn = EErrorCode.NONE;

			rtn = this.OpenFileAndParse(uiSetting, machineConfig);

            if (rtn != EErrorCode.NONE)
            {
                alarmMsg = this._describe.ToString();
                return rtn;
            }

            rtn = this.ConverterToMPIFormat();

            if (rtn != EErrorCode.NONE)
            {
                alarmMsg = this._describe.ToString();
                return rtn;
            }

            rtn = this.SaveRecipeToFile();

            alarmMsg = this._describe.ToString();

            return rtn;
        }

        public virtual EErrorCode UpLoadRecipe()
        {
            return EErrorCode.NONE;
        }

		#endregion

		#region >>> protected abstract Method <<<

        protected abstract EErrorCode OpenFileAndParse(UISetting uiSetting, MachineConfig machineConfig);

		protected abstract EErrorCode ConverterToMPIFormat();

		protected abstract EErrorCode SaveRecipeToFile();

		#endregion

		#region >>> protected Method <<<

		public void OnSaveReportHead()
		{
			if (SaveReportHead != null)
			{
				SaveReportHead(null, null);
			}
		}

		#endregion
	}

}
