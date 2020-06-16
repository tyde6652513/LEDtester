using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Data.CalibrateData
{
	[Serializable]
	public class CalibrateChipValue
	{
		private List<CaliChip> _chipList;

		public CalibrateChipValue()
		{
			this._chipList = new List<CaliChip>();
		}

		public List<CaliChip> ChipList
		{
			get { return this._chipList; }
			set { this._chipList = value; }
		}
	}

	[Serializable]
	public class CaliChip
	{
		private List<CaliValue> _valueList;

		public CaliChip()
		{
			this._valueList = new List<CaliValue>();
		}

		public CaliValue this[string keyName]
		{
			get
			{
				foreach (var item in this._valueList)
				{
					if (item.KeyName == keyName)
					{
						return item;
					}
				}

				return null;
			}
		}

		public List<CaliValue> ValueList
		{
			get { return this._valueList; }
			set { this._valueList = value; }
		}
	}

	[Serializable]
	public class CaliValue
	{
		private bool _isEnable;
		private string _name;
		private string _keyName;
		private double _stdValue;
		private double _msrtValue;

		public CaliValue()
		{
			this._isEnable = true;

			this._name = string.Empty;

			this._keyName = string.Empty;

			this._stdValue = 0.0d;

			this._msrtValue = 0.0d;
		}

		public bool IsEnable
		{
			get { return this._isEnable; }
			set { this._isEnable = value; }
		}

		public string Name
		{
			get { return this._name; }
			set { this._name = value; }
		}

		public string KeyName
		{
			get { return this._keyName; }
			set { this._keyName = value; }
		}

		public double StdValue
		{
			get { return this._stdValue; }
			set { this._stdValue = value; }
		}

		public double MsrtValue
		{
			get { return this._msrtValue; }
			set { this._msrtValue = value; }
		}
	}
}
