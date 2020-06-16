using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Data
{
	[Serializable]
	public class SmartNGBin : ICloneable, IEnumerable<SmartNGData>
	{
		#region >>> Private Property <<<

		private List<SmartNGData> _dataList;
		private int _chipCount;

		#endregion

		#region >>> Constructor / Disposor <<<

		public SmartNGBin()
		{
			this._dataList = new List<SmartNGData>();

			this._chipCount = 0;
		}

		#endregion

		#region >>> Public Method <<<

		public bool Add(SmartNGData bin)
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

		public bool IsNGBin(Dictionary<string, double> rowData, out int serialNumber)
		{
			double value = 0.0d;

			serialNumber = 0;

			foreach (var item in this._dataList)
			{
				if (!item.IsEnable)
				{
					continue;
				}

				if (rowData.ContainsKey(item.KeyName))
				{
					value = rowData[item.KeyName];
				}
				else
				{
                    //20180703 David skip if not tested
                    continue;
                    //serialNumber = item.SerialNumber;

                    ////NG Bin Chip Count
                    //item.ChipCount++;

                    ////NG Bin Totle Chip Count
                    //this._chipCount++;

                    //return true;
				}

				if (item.IsNGBin(value))
				{
					serialNumber = item.SerialNumber;

					//NG Bin Chip Count
                    //item.ChipCount++;//did in item.IsNGBin(value)

					//NG Bin Totle Chip Count
					this._chipCount++;

					return true;
				}
			}

			return false;
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

		public void ClearChipCount()
		{
			this._chipCount = 0;

			foreach (var item in this._dataList)
			{
				item.ChipCount = 0;
			}
		}

		public SmartNGData GetDataFromSN(int SerialNumber)
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

        public List<Dictionary<string, object>> GetNGBinInfoList()
        {
            List<Dictionary<string, object>> oList = new List<Dictionary<string, object>>();

            foreach (var ngBin in _dataList)
            {
                oList.Add(ngBin.GetBinInfo());
            }
            return oList;
        }

		public object Clone()
		{
			SmartNGBin cloneObj = new SmartNGBin();

			cloneObj._chipCount = this._chipCount;

			foreach (var item in this._dataList)
			{
				cloneObj._dataList.Add(item.Clone() as SmartNGData);
			}

			return cloneObj;
		}

		#endregion

		#region >>> Public Property <<<

		public SmartNGData this[int Index]
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

		public SmartNGData this[string KeyName]
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
			return (System.Collections.IEnumerator)(new SmartNGDataEnum(this._dataList));
		}

		IEnumerator<SmartNGData> IEnumerable<SmartNGData>.GetEnumerator()
		{
			return (IEnumerator<SmartNGData>)(new SmartNGDataEnum(this._dataList));
		}

		#endregion

		#region >>> SmartNGDataEnum Class <<<

		private class SmartNGDataEnum : IEnumerator<SmartNGData>
		{
			#region >>> Private Property <<<

			private int _position;
			private SmartNGData _data;
			private List<SmartNGData> _dataList;

			#endregion

			#region >>> Constructor / Disposor <<<

			public SmartNGDataEnum(List<SmartNGData> dataList)
			{
				this._position = -1;

				this._data = default(SmartNGData);

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

			public SmartNGData Current
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
