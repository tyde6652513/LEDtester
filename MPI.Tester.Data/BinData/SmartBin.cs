using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Data
{
	[Serializable]
	public class SmartBin : ICloneable, IEnumerable<SmartBinData>
	{
		#region >>> Private Property <<<

		private List<SmartBinData> _dataList;
		private SmartBoundary _boundary;
		private int _chipCount;

		#endregion

		#region >>> Constructor / Disposor <<<

		public SmartBin()
		{
			this._boundary = new SmartBoundary();

			this._dataList = new List<SmartBinData>();

			this._chipCount = 0;
		}

		#endregion

		#region >>> Private Method <<<
		#endregion

		#region >>> Public Method <<<

		public bool IsInBin(Dictionary<string, double> rowData, int binSortingRule, out int serialNumber, out string sideBinKeyName)
		{
			serialNumber = 0;

			sideBinKeyName = string.Empty;

			string snStr = string.Empty;

			foreach (var item in this._boundary)
			{
				//Check In Boundary
				string boundarySerilaNumber = string.Empty;


				if (item.Count == 0)
				{
					continue;
				}

				if (item.IsInBoundary(rowData, binSortingRule, out boundarySerilaNumber))
				{
					snStr += boundarySerilaNumber;
				}
				else
				{
					sideBinKeyName = item.KeyName;

					return false;
				}
			}

			foreach (var item in this._dataList)
			{
				if (snStr == item.BoundarySN)
				{
					serialNumber = item.SerialNumber;

					//Smart Bin Chip Count
					item.ChipCount++;

					//Smart Bin Totle Chip Count
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

		public bool ContainsBoundary(string keyName)
		{
			foreach (var item in this._boundary)
			{
				if (item.KeyName == keyName)
				{
					return true;
				}
			}

			return false;
		}

		public void CreateBinTable()
		{
			this._dataList.Clear();

			if (this._boundary.Count == 0)
			{
				return;
			}

			int totalBinCount = 1;

			foreach (var item in this._boundary)
			{
				if (item.Count != 0)
				{
					totalBinCount *= item.Count;
				}
			}

			for (int bin = 0; bin < totalBinCount; bin++)
			{
				SmartBinData binData = new SmartBinData();

				binData.BinCode = string.Empty;

				binData.BinNumber = bin + 1;

				this._dataList.Add(binData);
			}

			int repeatCount = 1;

			for (int i = 0; i < this._boundary.Count; i++)
			{
				if (i > 0)
				{
					if (this._boundary[i - 1].Count != 0)
					{
						repeatCount *= this._boundary[i - 1].Count;
					}
				}

				for (int bin = 0; bin < totalBinCount; bin++)
				{
					if (this._boundary[i].Count != 0)
					{
						int index = (bin / repeatCount) % this._boundary[i].Count;

						this._dataList[bin].BinCode += this._boundary[i][index].BoundaryCode;

						this._dataList[bin].BoundarySN += this._boundary[i][index].SerialNumber;
					}
				}
			}

			//this._dataList.Clear();

			//int totalBinCount = 1;

			//List<int> table = new List<int>();

			//for (int i = 0; i < this._boundaryList.Count; i++)
			//{
			//    table.Add(0);

			//    totalBinCount = totalBinCount * this._boundaryList[i].Boundary.Count;
			//}

			//for (int count = 0; count < totalBinCount; count++)
			//{
			//    SmartBinData binData = new SmartBinData();

			//    binData.BinCode = string.Empty;

			//    binData.BinNumber = count + 1;

			//    string binCode = string.Empty;

			//    for (int col = 0; col < this._boundaryList.Count; col++)
			//    {
			//        int index = table[col];

			//        binData.BoundarySNList.Add(this._boundaryList[col].Boundary[index].SerialNumber);

			//        binData.BinCode += (this._boundaryList[col].Boundary[index].BoundaryCode);

			//        if (col == 0)
			//        {
			//            table[col]++;
			//        }
			//    }

			//    for (int i = 0; i < this._boundaryList.Count; i++)
			//    {
			//        if (table[i] == this._boundaryList[i].Boundary.Count)
			//        {
			//            if (i < this._boundaryList.Count - 1)
			//            {
			//                table[i] = 0;

			//                table[i + 1]++;
			//            }
			//        }
			//    }

			//    this._dataList.Add(binData);
			//}
		}

		public void ClearChipCount()
		{
			this._chipCount = 0;

			foreach (var item in this._dataList)
			{
				item.ChipCount = 0;
			}
		}

		public void Clear()
		{
			this._dataList.Clear();

			this._boundary.Clear();

			this.ClearChipCount();
		}

		public SmartBinData GetDataFromSN(int SerialNumber)
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

        public Dictionary<int, SmartBinInfo> GetSmartBinInfo()
        {

            Dictionary<int, SmartBinInfo> binInfoDic = new Dictionary<int, SmartBinInfo>();


            int totalBinCount = 1;

            foreach (var item in this._boundary)
            {
                if (item.Count != 0)
                {
                    totalBinCount *= item.Count;
                }
            }

            int repeatCount = 1;

            for (int i = 0; i < this._boundary.Count; i++)
            {
                if (i > 0)
                {
                    if (this._boundary[i - 1].Count != 0)
                    {
                        repeatCount *= this._boundary[i - 1].Count;
                    }
                }

                for (int bin = 0; bin < totalBinCount; bin++)
                {
                    if (!binInfoDic.ContainsKey(bin))
                    {
                        binInfoDic.Add(bin, new SmartBinInfo(this._dataList[bin]));
                    }
                    if (this._boundary[i].Count != 0)
                    {
                        int index = (bin / repeatCount) % this._boundary[i].Count;
                        if (this._boundary[i][index] is SmartLowUp)
                        { binInfoDic[bin].PushBound(this._boundary[i].KeyName, (SmartLowUp)this._boundary[i][index]); }
                       
                    }
                }
            }

            return binInfoDic;
        }

		public object Clone()
		{
			SmartBin cloneObj = new SmartBin();

			cloneObj._chipCount = this._chipCount;

			foreach (var item in this._dataList)
			{
				cloneObj._dataList.Add(item.Clone() as SmartBinData);
			}

			foreach (var item in this._boundary)
			{
				cloneObj._boundary.Add(item.Clone() as SmartBoundaryData);
			}

			return cloneObj;
		}

		#endregion

		#region >>> Public Property <<<

		public SmartBinData this[int Index]
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

		public SmartBoundary Boundary
		{
			get { return this._boundary; }

			set { this._boundary = value; }
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
			return (System.Collections.IEnumerator)(new SmartBinDataEnum(this._dataList));
		}

		IEnumerator<SmartBinData> IEnumerable<SmartBinData>.GetEnumerator()
		{
			return (IEnumerator<SmartBinData>)(new SmartBinDataEnum(this._dataList));
		}

		#endregion

		#region >>> SmartBinDataEnum Class <<<

		private class SmartBinDataEnum : IEnumerator<SmartBinData>
		{
			#region >>> Private Property <<<

			private int _position;
			private SmartBinData _data;
			private List<SmartBinData> _dataList;

			#endregion

			#region >>> Constructor / Disposor <<<

			public SmartBinDataEnum(List<SmartBinData> dataList)
			{
				this._position = -1;

				this._data = default(SmartBinData);

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

			public SmartBinData Current
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

    public class SmartBinInfo
    {
        public int SN{get;set;}
        public int BinNum { get; set; }
        public string BinCode { get; set; }
        public Dictionary<string, BoundSpec> KeyBoundaryDic { get; set; }
        
        public SmartBinInfo(SmartBinData bInfo)
        {
            SN = bInfo.SerialNumber;
            BinNum = bInfo.BinNumber;
            BinCode = bInfo.BinCode;
            KeyBoundaryDic = new Dictionary<string, BoundSpec>();
        }

        public void PushBound(string key,SmartLowUp sup)
        {
            if (KeyBoundaryDic.ContainsKey(key))
            {
                KeyBoundaryDic[key] = new BoundSpec(sup);
            }
            else
            {
                KeyBoundaryDic.Add(key,new BoundSpec(sup));
            }
        }       
    }

    public class BoundSpec
    {
        public double Up { get; set; }
        public double Low { get; set; }
        public string Rule { get; set; }

        public BoundSpec(SmartLowUp slu)
        {
            Up = slu.UpLimit;
            Low = slu.LowLimit;
            Rule = slu.BoundaryRule.ToString();
        }

    }
}
