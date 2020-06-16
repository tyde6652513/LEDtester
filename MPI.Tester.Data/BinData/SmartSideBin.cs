using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Data
{
	[Serializable]
	public class SmartSideBin : ICloneable, IEnumerable<SmartSideData>
	{
		#region >>> Private Property <<<

		private List<SmartSideData> _dataList;
		private int _chipCount;

		#endregion

		#region >>> Constructor / Disposor <<<

		public SmartSideBin()
		{
			this._dataList = new List<SmartSideData>();

			this._chipCount = 0;
		}

		#endregion

		#region >>> Public Method <<<

		public bool Add(SmartSideData bin)
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

			this.ClearChipCount();
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

		public bool SideBinInfo(string keyName, out int serialNumber)
		{
			serialNumber = 0;

			foreach (var item in this._dataList)
			{
				if (keyName == item.KeyName)
				{
					serialNumber = item.SerialNumber;

					//Side Bin Chip Count
					item.ChipCount++;

					//Side Bin Totle Chip Count
					this._chipCount++;

					return true;
				}
			}

			return false;
		}

		public void ClearChipCount()
		{
			this._chipCount = 0;

			foreach (var item in this._dataList)
			{
				item.ChipCount = 0;
			}
		}

		public SmartSideData GetDataFromSN(int SerialNumber)
		{
			for (int i = 0; i < this._dataList.Count; i++)
			{
				if (SerialNumber == this._dataList[i].SerialNumber)
				{
					return this._dataList[i];
				}
			}

			return null;
		}

        public bool DeBin(int serialNumber)
        {
            foreach (var item in this._dataList)
            {
                if (serialNumber == item.SerialNumber)
                {
                    item.ChipCount--;
                    this.ChipCount--;
                    return true;
                }
            }
            return false;
        }

        public List<Dictionary<string, object>> GetSideinInfoList()
        {
            List<Dictionary<string, object>> oList = new List<Dictionary<string, object>>();

            foreach (var sideBin in _dataList)
            {
                oList.Add(sideBin.GetBinInfo());
            }
            return oList;
        }

        
		public object Clone()
		{
			SmartSideBin cloneObj = new SmartSideBin();

			cloneObj._chipCount = this._chipCount;

			foreach (var item in this._dataList)
			{
				cloneObj._dataList.Add(item.Clone() as SmartSideData);
			}

			return cloneObj;
		}

		#endregion

		#region >>> Public Property <<<

		public SmartSideData this[int Index]
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

		public SmartSideData this[string KeyName]
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

		public int ChipCount
		{
			get { return this._chipCount; }
			set { { this._chipCount = value; } }
		}

		public int Count
		{
			get { return this._dataList.Count; }
		}

		#endregion

		#region >>> IEnumerator Interface <<<

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (System.Collections.IEnumerator)(new SmartSideDataEnum(this._dataList));
		}

		IEnumerator<SmartSideData> IEnumerable<SmartSideData>.GetEnumerator()
		{
			return (IEnumerator<SmartSideData>)(new SmartSideDataEnum(this._dataList));
		}

		#endregion

		#region >>> SmartSideDataEnum Class <<<

		private class SmartSideDataEnum : IEnumerator<SmartSideData>
		{
			#region >>> Private Property <<<

			private int _position;
			private SmartSideData _data;
			private List<SmartSideData> _dataList;

			#endregion

			#region >>> Constructor / Disposor <<<

			public SmartSideDataEnum(List<SmartSideData> dataList)
			{
				this._position = -1;

				this._data = default(SmartSideData);

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

			public SmartSideData Current
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
