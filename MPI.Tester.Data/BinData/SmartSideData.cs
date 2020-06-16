using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Data
{
	[Serializable]
	public class SmartSideData : SmartBinDataBase
	{
		#region >>> Private Property <<<

		private string _name;
		private string _keyName;

		#endregion

		#region >>> Constructor / Disposor <<<

		public SmartSideData()
			: base(EBinningType.SIDE_BIN)
		{
			this._name = string.Empty;

			this._keyName = string.Empty;

			this.BinCode = "Side-Bin";

			this.BinNumber = -1;
		}

		public SmartSideData(string name)
			: this()
		{
			this.BinCode = name + " Side-Bin";
		}

        public Dictionary<string, object> GetBinInfo()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Name", Name);
            dic.Add("KeyName", KeyName);
            return dic;

        }

		#endregion

		#region >>> Public Property <<<

		public string Name
		{
			get { return this._name; }
			set { { this._name = value; } }
		}

		public string KeyName
		{
			get { return this._keyName; }
			set { { this._keyName = value; } }
		}

		#endregion
	}
}
