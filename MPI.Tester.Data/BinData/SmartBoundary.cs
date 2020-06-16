using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Data
{
	[Serializable]
	public class SmartBoundary : ICloneable, IEnumerable<SmartBoundaryData>
	{
		#region >>> Private Property <<<

		private List<SmartBoundaryData> _dataList;

		#endregion

		#region >>> Constructor / Disposor <<<

		public SmartBoundary()
		{
			this._dataList = new List<SmartBoundaryData>();
		}

		#endregion

		#region >>> Public Method <<<

		public bool Add(SmartBoundaryData bin)
		{
			foreach (var item in this._dataList)
			{
				if (bin.KeyName == item.KeyName)
				{
					return false;
				}
			}

			this._dataList.Add(bin);

			return true;
		}

		public void Clear()
		{
			this._dataList.Clear();
		}

		public void Remove(int index)
		{
			if (index >= 0 && index < this._dataList.Count)
			{
				this._dataList.RemoveAt(index);
			}
		}

		public void Remove(string keyName)
		{
			for (int i = 0; i < this._dataList.Count; i++)
			{
				if (keyName == this._dataList[i].KeyName)
				{
					this._dataList.RemoveAt(i);

					return;
				}
			}
		}

		public bool ContainsKey(string keyName)
		{
			for (int i = 0; i < this._dataList.Count; i++)
			{
				if (keyName == this._dataList[i].KeyName)
				{
					return true;
				}
			}

			return false;
		}

		public object Clone()
		{
			SmartBoundary cloneObj = new SmartBoundary();

			foreach (var item in this._dataList)
			{
				cloneObj._dataList.Add(item.Clone() as SmartBoundaryData);
			}

			return cloneObj;
		}

		#endregion

		#region >>> Public Property <<<

		public SmartBoundaryData this[int Index]
		{
			get
			{
				if (Index >= 0 && Index < this._dataList.Count)
				{
					return this._dataList[Index];
				}
				else
				{
					return null;
				}
			}

			set
			{
				if (Index >= 0 && Index < this._dataList.Count)
				{
					this._dataList[Index] = value;

					return;
				}
			}
		}

		public SmartBoundaryData this[string KeyName]
		{
			get
			{
				foreach (var item in this._dataList)
				{
					if (KeyName == item.KeyName)
					{
						return item;
					}
				}

				return null;
			}

			set
			{
				for (int i = 0; i < this._dataList.Count; i++)
				{
					if (KeyName == this._dataList[i].KeyName)
					{
						this._dataList[i] = value;

						return;
					}
				}
			}
		}

		public int Count
		{
			get { return this._dataList.Count; }
		}

		#endregion

		#region >>> IEnumerator Interface <<<

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (System.Collections.IEnumerator)(new SmartBoundaryDataEnum(this._dataList));
		}

		IEnumerator<SmartBoundaryData> IEnumerable<SmartBoundaryData>.GetEnumerator()
		{
			return (IEnumerator<SmartBoundaryData>)(new SmartBoundaryDataEnum(this._dataList));
		}

		#endregion

		#region >>> SmartSideDataEnum Class <<<

		private class SmartBoundaryDataEnum : IEnumerator<SmartBoundaryData>
		{
			#region >>> Private Property <<<

			private int _position;
			private SmartBoundaryData _data;
			private List<SmartBoundaryData> _dataList;

			#endregion

			#region >>> Constructor / Disposor <<<

			public SmartBoundaryDataEnum(List<SmartBoundaryData> dataList)
			{
				this._position = -1;

				this._data = default(SmartBoundaryData);

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

			public SmartBoundaryData Current
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

		#endregion
	}
}
