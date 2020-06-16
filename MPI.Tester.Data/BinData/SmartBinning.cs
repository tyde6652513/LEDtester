using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MPI.Tester.Maths;
using System.Xml.Serialization;

namespace MPI.Tester.Data
{
	[Serializable]
	public class SmartBinning : ICloneable, IEnumerable<SmartBinDataBase>
	{
		public const int MaxBinCount = 256;
		public const string CIExyKEY = "CIExy";
		public const string CIEupvpKEY = "CIEupvp";

		#region >>> Private Property <<<

		private SmartBinningData _data;		

		#endregion

		#region >>> Constructor / Disposor <<<

		public SmartBinning()
		{
			this._data = new SmartBinningData();
		}

		#endregion

		#region >>> Private Method <<<

		private int GetAutoBin(int serialNumber)
		{
			int binNumber = 0;

			if (this._data.AutoBinData.ContainsKey(serialNumber))
			{
				binNumber = this._data.AutoBinData[serialNumber];
			}
			else
			{
				binNumber = this._data.AutoBinData.Count + 1;

				this._data.AutoBinData.Add(serialNumber, binNumber);
			}

			return binNumber;
		}

		private void ResetBoundary(ProductData product, Dictionary<string, string> binItemNameDic)
		{
			if (product == null || product.TestCondition == null)
			{
				return;
			}

			if (product.TestCondition.TestItemArray == null || product.TestCondition.TestItemArray.Length == 0)
			{
				return;
			}

			List<string> msrtKey = new List<string>();

			foreach (var testItem in product.TestCondition.TestItemArray)
			{
				if (testItem.MsrtResult == null || testItem.MsrtResult.Length == 0)
				{
					continue;
				}

				///////////////////////////////////////////////////////////////////////
				//CIExy & CIEUpVp key
				///////////////////////////////////////////////////////////////////////
				if (testItem.KeyName.Contains(ETestType.LOPWL.ToString()))
				{
					///////////////////////////////////////////////////////////////////////
					//CIExy
					///////////////////////////////////////////////////////////////////////
					string cieXKey = testItem.KeyName.Replace(ETestType.LOPWL.ToString(), EOptiMsrtType.CIEx.ToString());

					string cieYKey = testItem.KeyName.Replace(ETestType.LOPWL.ToString(), EOptiMsrtType.CIEy.ToString());

					string cieXYFormat = "0.00";

					foreach (var msrtItem in testItem.MsrtResult)
					{
						if (msrtItem.KeyName == cieXKey)
						{
							cieXYFormat = msrtItem.Formate;
						}
					}

					string keyName = testItem.KeyName.Replace(ETestType.LOPWL.ToString(), SmartBinning.CIExyKEY);

					msrtKey.Add(keyName);

					SmartBoundaryData data = null;

					if (this._data.SmartBin.Boundary.ContainsKey(keyName))
					{
						data = this._data.SmartBin.Boundary[keyName];
					}
					else
					{
						data = new SmartBoundaryData();

						data.KeyName = keyName;

						this._data.SmartBin.Boundary.Add(data);
					}

					if (binItemNameDic.ContainsKey(keyName))
					{
						data.Name = binItemNameDic[keyName];
					}
					else
					{
						data.Name = keyName;
					}

					data.Format = cieXYFormat;

					///////////////////////////////////////////////////////////////////////
					//CIEupVp
					///////////////////////////////////////////////////////////////////////
					string cieUpKey = testItem.KeyName.Replace(ETestType.LOPWL.ToString(), EOptiMsrtType.Uprime.ToString());

					string cieVpKey = testItem.KeyName.Replace(ETestType.LOPWL.ToString(), EOptiMsrtType.Vprime.ToString());

					string cieupvpFormat = "0.00";

					foreach (var msrtItem in testItem.MsrtResult)
					{
						if (msrtItem.KeyName == cieUpKey)
						{
							cieupvpFormat = msrtItem.Formate;
						}
					}

					keyName = testItem.KeyName.Replace(ETestType.LOPWL.ToString(), SmartBinning.CIEupvpKEY);

					msrtKey.Add(keyName);

					data = null;

					if (this._data.SmartBin.Boundary.ContainsKey(keyName))
					{
						data = this._data.SmartBin.Boundary[keyName];
					}
					else
					{
						data = new SmartBoundaryData();

						data.KeyName = keyName;

						this._data.SmartBin.Boundary.Add(data);
					}

					if (binItemNameDic.ContainsKey(keyName))
					{
						data.Name = binItemNameDic[keyName];
					}
					else
					{
						data.Name = keyName;
					}

					data.Format = cieupvpFormat;
				}

				///////////////////////////////////////////////////////////////////////
				//Other Msrt Item
				///////////////////////////////////////////////////////////////////////
				foreach (var msrtItem in testItem.MsrtResult)
				{
					if (!msrtItem.IsVision)
					{
						continue;
					}

					msrtKey.Add(msrtItem.KeyName);

					SmartBoundaryData data = null;

					if (this._data.SmartBin.Boundary.ContainsKey(msrtItem.KeyName))
					{
						data = this._data.SmartBin.Boundary[msrtItem.KeyName];
					}
					else
					{
						data = new SmartBoundaryData();

						data.KeyName = msrtItem.KeyName;

						this._data.SmartBin.Boundary.Add(data);
					}

					data.Name = msrtItem.Name;

					data.Format = msrtItem.Formate;

                    
				}
			}

			///////////////////////////////////////////////////////////////////////
			//Remove not Exist Msrt Item
			///////////////////////////////////////////////////////////////////////
			for (int i = 0; i < this._data.SmartBin.Boundary.Count; i++)
			{
				string keyName = this._data.SmartBin.Boundary[i].KeyName;

				if (!msrtKey.Contains(keyName))
				{
					this._data.SmartBin.Boundary.Remove(i);

					i--;
				}
			}
		}

		private void ResetNGBin(ProductData product, Dictionary<string, string> binItemNameDic)
		{
			if (product == null || product.TestCondition == null)
			{
				return;
			}

			if (product.TestCondition.TestItemArray == null || product.TestCondition.TestItemArray.Length == 0)
			{
				return;
			}

			List<string> msrtKey = new List<string>();

			foreach (var testItem in product.TestCondition.TestItemArray)
			{
				if (testItem.MsrtResult == null || testItem.MsrtResult.Length == 0)
				{
					continue;
				}

				foreach (var msrtItem in testItem.MsrtResult)
				{
					if (!msrtItem.IsVision)
					{
						continue;
					}

					msrtKey.Add(msrtItem.KeyName);

					SmartNGData ngBin = null;

					if (this._data.NGBin.ContainsKey(msrtItem.Name))
					{
						ngBin = this._data.NGBin[msrtItem.KeyName];
					}
					else
					{
						ngBin = new SmartNGData(msrtItem.Name);

						ngBin.KeyName = msrtItem.KeyName;

						ngBin.NGLowLimit = msrtItem.MinLimitValue;

						ngBin.NGUpLimit = msrtItem.MaxLimitValue;

						ngBin.IsEnable = false;

						this._data.NGBin.Add(ngBin);
					}

                    ngBin.BoundaryRule = msrtItem.BoundaryRule;//20181210 David NG Bin Boundary Rule follow by TestResultItem

					ngBin.IsTester = testItem.IsEnable & msrtItem.IsEnable;

					ngBin.Name = msrtItem.Name;

					ngBin.Format = msrtItem.Formate;
				}
			}

			for (int i = 0; i < this._data.NGBin.Count; i++)
			{
				string keyName = this._data.NGBin[i].KeyName;

				if (!msrtKey.Contains(keyName))
				{
					this._data.NGBin.Remove(i);

					i--;
				}
			}
		}

		private void ResetSideBin(ProductData product, Dictionary<string, string> binItemNameDic)
		{
			if (product == null || product.TestCondition == null)
			{
				return;
			}

			if (product.TestCondition.TestItemArray == null || product.TestCondition.TestItemArray.Length == 0)
			{
				return;
			}

			List<string> msrtKey = new List<string>();

			foreach (var testItem in product.TestCondition.TestItemArray)
			{
				if (testItem.MsrtResult == null || testItem.MsrtResult.Length == 0)
				{
					continue;
				}

				///////////////////////////////////////////////////////////////////////
				//CIExy & CIEUpVp key
				///////////////////////////////////////////////////////////////////////
				if (testItem.KeyName.Contains(ETestType.LOPWL.ToString()))
				{
					///////////////////////////////////////////////////////////////////////
					//CIExy
					///////////////////////////////////////////////////////////////////////
					string cieXKey = testItem.KeyName.Replace(ETestType.LOPWL.ToString(), EOptiMsrtType.CIEx.ToString());

					string cieYKey = testItem.KeyName.Replace(ETestType.LOPWL.ToString(), EOptiMsrtType.CIEy.ToString());

					string keyName = testItem.KeyName.Replace(ETestType.LOPWL.ToString(), SmartBinning.CIExyKEY);

					string name = string.Empty;

					if (binItemNameDic.ContainsKey(keyName))
					{
						name = binItemNameDic[keyName];
					}
					else
					{
						name = keyName;
					}

					msrtKey.Add(keyName);

					SmartSideData data = null;

					if (this._data.SideBin.ContainsKey(keyName))
					{
						data = this._data.SideBin[keyName];
					}
					else
					{
						data = new SmartSideData(name);

						data.KeyName = keyName;						

						this._data.SideBin.Add(data);
					}

					data.Name = name;

					///////////////////////////////////////////////////////////////////////
					//CIEupVp
					///////////////////////////////////////////////////////////////////////
					string cieUpKey = testItem.KeyName.Replace(ETestType.LOPWL.ToString(), EOptiMsrtType.Uprime.ToString());

					string cieVpKey = testItem.KeyName.Replace(ETestType.LOPWL.ToString(), EOptiMsrtType.Vprime.ToString());

					keyName = testItem.KeyName.Replace(ETestType.LOPWL.ToString(), SmartBinning.CIEupvpKEY);

					name = string.Empty;

					if (binItemNameDic.ContainsKey(keyName))
					{
						name = binItemNameDic[keyName];
					}
					else
					{
						name = keyName;
					}

					msrtKey.Add(keyName);

					if (this._data.SideBin.ContainsKey(keyName))
					{
						data = this._data.SideBin[keyName];
					}
					else
					{
						data = new SmartSideData(data.Name);

						data.KeyName = keyName;

						this._data.SideBin.Add(data);
					}

					data.Name = name;
				}

				///////////////////////////////////////////////////////////////////////
				//Other Msrt Item
				///////////////////////////////////////////////////////////////////////
				foreach (var msrtItem in testItem.MsrtResult)
				{
					if (!msrtItem.IsVision)
					{
						continue;
					}

					msrtKey.Add(msrtItem.KeyName);

					SmartSideData data = null;

					if (this._data.SideBin.ContainsKey(msrtItem.KeyName))
					{
						data = this._data.SideBin[msrtItem.KeyName];
					}
					else
					{
						data = new SmartSideData(msrtItem.Name);

						data.KeyName = msrtItem.KeyName;

						this._data.SideBin.Add(data);
					}

					data.Name = msrtItem.Name;
				}
			}

			///////////////////////////////////////////////////////////////////////
			//Remove not Exist Msrt Item
			///////////////////////////////////////////////////////////////////////
			for (int i = 0; i < this._data.SmartBin.Boundary.Count; i++)
			{
				string keyName = this._data.SmartBin.Boundary[i].KeyName;

				if (!msrtKey.Contains(keyName))
				{
					this._data.SmartBin.Boundary.Remove(i);

					i--;
				}
			}
		}

		#endregion

		#region >>> Public Method <<<

		public int CalculateBin(Dictionary<string, double> rawData, int binSortingRule, out int binSN)
		{
			int binNumber = -1;

			SmartBinDataBase bin = null;

			string sideBinKeyName = string.Empty;

			if (this._data.NGBin.IsNGBin(rawData, out binSN))
			{ 
			}
			else if (this._data.SmartBin.IsInBin(rawData, binSortingRule, out binSN, out sideBinKeyName))
			{ 
			}
			else if (this._data.SideBin.SideBinInfo(sideBinKeyName, out binSN))
			{
			}
			else
			{
				binSN = 0;

				return binNumber;
			}

			bin = this.GetBinFromSN(binSN);

			binNumber = bin.BinNumber;

			if (this._data.IsAutoBin)
			{
				bin.AutoBinNumber = this.GetAutoBin(binSN);
			}
			return binNumber;

		}

        public bool DeBin(int binSN)
        {
            if (this._data.NGBin.DeBin(binSN))
            {
                return true;
            }
            else if (this._data.SmartBin.DeBin(binSN))
            {
                return true;
            }
            else if (this._data.SideBin.DeBin(binSN))
            {
                return true;
            }

            return false;
        }

		public void ResetBinData(ProductData product, Dictionary<string, string> binItemNameDic)
		{
			//this.ResetBoundary(product, binItemNameDic);

			//this.ResetNGBin(product, binItemNameDic);

			//this.ResetSideBin(product, binItemNameDic);
		}

		public bool Save(string fileName)
		{
			bool rtn = true;

			using (FileStream fs = new FileStream(fileName, FileMode.Create))
			{
				BinaryFormatter bf = new BinaryFormatter();

				try
				{
					bf.Serialize(fs, this._data);
				}
				catch
				{
					rtn = false;
				}
			}

			return rtn;
		}

		public bool Load(string fileName)
		{
			if (!File.Exists(fileName))
			{
				return false;
			}

			try
			{
			using (FileStream fs = new FileStream(fileName, FileMode.Open))
			{
				BinaryFormatter bf = new BinaryFormatter();

					SmartBinningData obj = bf.Deserialize(fs) as SmartBinningData;

					if (obj != null)
					{
						this._data = obj;
					}
				}

					return true;
				}
				catch
				{
                //Backup old *.bin File
                MPIFile.CopyFile(fileName, Path.Combine(Constants.Paths.ROOT, "BinData", Path.GetFileName(fileName)));

					return false;
				}
		
		}

		public object Clone()
		{
			SmartBinning cloneObj = new SmartBinning();

			cloneObj._data = this._data.Clone() as SmartBinningData;

			return cloneObj;
		}

		public void Clear()
		{
			this._data.SmartBin.Clear();

			this._data.NGBin.Clear();

			this._data.SideBin.Clear();

			this.ResetStatistic();
		}

		public void ResetStatistic()
		{
			this._data.ClearChipCount();
		}

		public SmartBinDataBase GetBinFromSN(int SerialNumber)
		{
			for (int i = 0; i < this._data.SmartBin.Count; i++)
			{
				if (SerialNumber == this._data.SmartBin[i].SerialNumber)
				{
					return this._data.SmartBin[i];
				}
			}

			for (int i = 0; i < this._data.NGBin.Count; i++)
			{
				if (SerialNumber == this._data.NGBin[i].SerialNumber)
				{
					return this._data.NGBin[i];
				}
			}

			for (int i = 0; i < this._data.SideBin.Count; i++)
			{
				if (SerialNumber == this._data.SideBin[i].SerialNumber)
				{
					return this._data.SideBin[i];
				}
			}

			return null;
		}

		#endregion

		#region >>> Public Property <<<

		public SmartBinDataBase this[int count]
		{
			get
			{
				if (count < 0)
				{
					return null;
				}
				else if (count < this._data.SmartBin.Count)
				{
					return this._data.SmartBin[count];
				}
				else if (count - this._data.SmartBin.Count < this._data.NGBin.Count)
				{
					return this._data.NGBin[count - this._data.SmartBin.Count];
				}
				else if (count - this._data.SmartBin.Count - this._data.NGBin.Count < this._data.SideBin.Count)
				{
					return this._data.SideBin[count - this._data.SmartBin.Count - this._data.NGBin.Count];
				}

				return null;
			}
		}

		public int ChipCount
		{
			get { return this._data.ChipCount; }
		}

		public int Count
		{
			get { return this._data.Count; }
		}		

		public bool IsAutoBin
		{
			get { return this._data.IsAutoBin; }
			set 
			{ 
				this._data.IsAutoBin = value;
				this._data.IsAutoNGBin = value;
				this._data.IsAutoSideBin = value; 
			}
		}

		public bool IsAutoNGBin
		{
			get { return this._data.IsAutoNGBin; }
			set { this._data.IsAutoNGBin = value; }
		}

		public bool IsAutoSideBin
		{
			get { return this._data.IsAutoSideBin; }
			set { this._data.IsAutoSideBin = value; }
		}

		public Statistic StatisticBin
		{
			get
			{
				Statistic data = new Statistic();

				foreach (var item in this)
				{
					data.Push(item.BinNumber);
				}

				return data;
			}
		}

		public SmartBin SmartBin
		{
			get { return this._data.SmartBin; }
			set { this._data.SmartBin = value; }
		}

		public SmartNGBin NGBin
		{
			get { return this._data.NGBin; }
			set { this._data.NGBin = value; }
		}

		public SmartSideBin SideBin
		{
			get { return this._data.SideBin; }
			set { this._data.SideBin = value; }
		}

		public double InBinRate
		{
			get
			{
				double inBinRate = 0.0d;

				if (this.ChipCount > 0)
				{
					inBinRate = (100.0d * (double)this._data.SmartBin.ChipCount) / this.ChipCount;
				}

				return inBinRate;
			}
		}

		public double NGBinRate
		{
			get
			{
				double ngBinRate = 0.0d;

				if (this.ChipCount > 0)
				{
					ngBinRate = (100.0d * (double)this._data.NGBin.ChipCount) / this.ChipCount;
				}

				return ngBinRate;
			}
		}

		public double SideBinRate
		{
			get
			{
				double sideBinRate = 0.0d;

				if (this.ChipCount > 0)
				{
					sideBinRate = (100.0d * (double)this._data.SideBin.ChipCount) / this.ChipCount;
				}

				return sideBinRate;
			}
		}

		#endregion

		#region >>> IEnumerator Interface <<<

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (System.Collections.IEnumerator)(new SmartBinDataBaseEnum(this._data.SmartBin, this._data.NGBin, this._data.SideBin));
		}

		IEnumerator<SmartBinDataBase> IEnumerable<SmartBinDataBase>.GetEnumerator()
		{
			return (IEnumerator<SmartBinDataBase>)(new SmartBinDataBaseEnum(this._data.SmartBin, this._data.NGBin, this._data.SideBin));
		}

		#endregion

		#region >>> SmartNGDataEnum Class <<<

		private class SmartBinDataBaseEnum : IEnumerator<SmartBinDataBase>
		{
			#region >>> Private Property <<<

			private int _position;
			private SmartBinDataBase _data;
			private List<SmartBinDataBase> _dataList;

			#endregion

			#region >>> Constructor / Disposor <<<

			public SmartBinDataBaseEnum(SmartBin smartBin, SmartNGBin smartNGBin, SmartSideBin smartSideBin)
			{
				this._position = -1;

				this._data = default(SmartBinDataBase);

				this._dataList = new List<SmartBinDataBase>();

				foreach (var item in smartBin)
				{
					this._dataList.Add(item);
				}

				foreach (var item in smartNGBin)
				{
					if (item.IsEnable)
					{
						this._dataList.Add(item);
					}
				}
				foreach (var item in smartSideBin)
				{
					if (smartBin.ContainsBoundary(item.KeyName))
					{
						this._dataList.Add(item);
					}
				}
			}

			#endregion

			#region >>> Interface Property <<<

			object System.Collections.IEnumerator.Current
			{
				get { return Current; }
			}

			#endregion

			#region >>> Public Method <<<

			public SmartBinDataBase Current
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
