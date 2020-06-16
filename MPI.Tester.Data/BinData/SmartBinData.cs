using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Data
{
	[Serializable]
	public class SmartBinData : SmartBinDataBase
	{
		#region >>> Private Property <<<

		private string _boundarySN;

		#endregion

		#region >>> Constructor / Disposor <<<

		public SmartBinData()
			: base(EBinningType.IN_BIN)
		{
			this.BinCode = "In-Bin";

			this.ChipCount = 0;

			this._boundarySN = string.Empty;
		}

		#endregion

        #region
        //public Dictionary<string, object> GetBinInfo()
        //{
        //    Dictionary<string, object> dic = new Dictionary<string, object>();
        //    dic.Add("BoundarySN", BoundarySN);

        //    dic.Add("BoundaryRule", BoundaryRule.ToString());
        //    return dic;

        //}
        #endregion

        #region >>> Public Property <<<

        public string BoundarySN
		{
			get { return this._boundarySN; }
			set { { this._boundarySN = value; } }
		}

		#endregion
	}
}
