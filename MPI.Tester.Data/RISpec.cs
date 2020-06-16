using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MPI.Tester.Data
{
	[Serializable]
	public class RISpec : IEnumerable<RISpecItem>
	{
		private RISepcData _riSpecData = new RISepcData();

		public RISpec()
		{ }

		public bool Load(string fileFullName)
		{
			bool rtn = true;

			if (!System.IO.File.Exists(fileFullName))
			{
				return false;
			}

			try
			{
				this._riSpecData = MPI.Xml.XmlFileSerializer.Deserialize(typeof(RISepcData), fileFullName) as RISepcData;
			}
			catch
			{
				return false;
			}

			return rtn;
		}

		public bool Save(string fileFullName)
		{
			bool rtn = true;

			try
			{
				MPI.Xml.XmlFileSerializer.Serialize(this._riSpecData, fileFullName);
			}
			catch
			{
				return false;
			}

			return rtn;
		}

		public void Add(RISpecItem data)
		{
			this._riSpecData.SpecItemList.Add(data);
		}

		public void AddRange(RISpecItem[] datas)
		{
			this._riSpecData.SpecItemList.AddRange(datas);
		}

		public void Remove(string keyName)
		{
			for (int i = 0; i < this._riSpecData.SpecItemList.Count; i++)
			{
				if (this._riSpecData.SpecItemList[i].KeyName == keyName)
				{
					this._riSpecData.SpecItemList.RemoveAt(i);

					return;
				}
			}
		}

		public void Clear()
		{
			this._riSpecData.SpecItemList.Clear();
		}

		public bool ContainsKey(string keyName)
		{
			for (int i = 0; i < this._riSpecData.SpecItemList.Count; i++)
			{
				if (this._riSpecData.SpecItemList[i].KeyName == keyName)
				{
					return true;
				}
			}

			return false;
		}

		public RISpecItem this[string keyName]
		{
			get
			{
				foreach (var item in this._riSpecData.SpecItemList)
				{
					if (item.KeyName == keyName)
					{
						return item;
					}
				}

				return null;
			}
		}

		public int Count
		{
			get { return this._riSpecData.SpecItemList.Count; }
		}

		public List<string> EnableRIKeyNameItemList
		{
			get { return this._riSpecData.EnableRIKeyNameItemList; }
		}

		public RISpec Clone()
		{
			RISpec data = new RISpec();

			data._riSpecData = this._riSpecData.Clone();

			return data;
		}

		public string RecipeName
		{
			get { return this._riSpecData.RecipeName; }
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (System.Collections.IEnumerator)(new RISpecDataEnum(this._riSpecData.SpecItemList));
		}

		IEnumerator<RISpecItem> IEnumerable<RISpecItem>.GetEnumerator()
		{
			return (IEnumerator<RISpecItem>)(new RISpecDataEnum(this._riSpecData.SpecItemList));
		}

		private class RISpecDataEnum : IEnumerator<RISpecItem>
		{
			#region >>> Private Property <<<

			private int _position;
			private RISpecItem _data;
			private List<RISpecItem> _dataList;

			#endregion

			#region >>> Constructor / Disposor <<<

			public RISpecDataEnum(List<RISpecItem> dataList)
			{
				this._position = -1;

				this._data = default(RISpecItem);

				this._dataList = dataList;
			}

			#endregion

			#region >>> Interface Property <<<

			object System.Collections.IEnumerator.Current
			{
				get { return Current; }
			}

			#endregion

			#region >>> Public Method <<<

			public RISpecItem Current
			{
				get { return this._data; }
			}

			public bool MoveNext()
			{
				if (++this._position >= this._dataList.Count)
				{
					return false;
				}
				else
				{
					this._data = this._dataList[this._position];
				}

				return true;
			}

			public void Reset()
			{
				this._position = -1;
			}

			public void Dispose()
			{
			}

			#endregion
		}
	}

	[Serializable]
	public class RISepcData
	{
		private string _recipeName = string.Empty;
		private List<string> _enableRIKeyNameItemList = new List<string>();
		private List<RISpecItem> _specItemList = new List<RISpecItem>();

		public RISepcData()
		{ }

		public string RecipeName
		{
			get { return this._recipeName; }
			set { this._recipeName = value; }
		}

		public List<string> EnableRIKeyNameItemList
		{
			get { return this._enableRIKeyNameItemList; }
			set { this._enableRIKeyNameItemList = value; }
		}

		public List<RISpecItem> SpecItemList
		{
			get { return this._specItemList; }
			set { this._specItemList = value; }
		}

		public RISepcData Clone()
		{
			RISepcData data = new RISepcData();

			data._recipeName = this._recipeName;

			foreach (var item in this._specItemList)
			{
				data._specItemList.Add(item.Clone());
			}

			return data;
		}
	}

	[Serializable]
	public class RISpecItem
	{
		private string _keyName = string.Empty;
		private bool _isEnableReportInterpolation = false;
		private bool _isEnableVerify = false;
		private bool _isOptItem = false;
		private ERISpecMode _specMode = ERISpecMode.STD;
		private float _std = 3.0f;
		private float _min = 0.0f;
		private float _max = 999.0f;

		public RISpecItem()
		{ }

		public string KeyName
		{
			get { return this._keyName; }
			set
			{
				if (value.Contains("_"))
				{
					string key = value.Remove(value.IndexOf("_"));

					this._isOptItem = Enum.GetNames(typeof(EOptiMsrtType)).Contains(key);
				}
				else
				{
					this._isOptItem = false;
				}

				this._keyName = value;
			}
		}

		public bool IsEnableReportInterpolation
		{
			get { return this._isEnableReportInterpolation; }
			set { this._isEnableReportInterpolation = value; }
		}

		public bool IsEnableVerify
		{
			get { return this._isEnableVerify; }
			set { this._isEnableVerify = value; }
		}

		public bool IsOptItem
		{
			get { return this._isOptItem; }
		}

		public ERISpecMode SpecMode
		{
			get { return this._specMode; }
			set { this._specMode = value; }
		}

		public float STD
		{
			get { return this._std; }
			set { this._std = value; }
		}

		public float Min
		{
			get { return this._min; }
			set { this._min = value; }
		}

		public float Max
		{
			get { return this._max; }
			set { this._max = value; }
		}

		public RISpecItem Clone()
		{
			RISpecItem data = new RISpecItem();

			data._keyName = this._keyName;

			data._isEnableVerify = this._isEnableVerify;

			data._isEnableReportInterpolation = this._isEnableReportInterpolation;

			data._isOptItem = this._isOptItem;

			data._specMode = this._specMode;

			data._std = this._std;

			data._min = this._min;

			data._max = this._max;

			return data;
		}
	}
}
