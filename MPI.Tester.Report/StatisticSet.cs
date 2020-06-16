using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Maths;
using MPI.Tester.Data;
using MPI.Tester.TestKernel;

namespace MPI.Tester.Report
{
	public class StatisticSet
	{
		#region >>> Private Property <<<

		private int _testCount;
		private int _goodCount01;
		private int _failCount01;
		private int _goodCount02;
		private int _failCount02;
		private int _goodCount03;
		private int _failCount03;
		private Dictionary<string, TestResultData> _resultData;
		private Dictionary<string, Statistic> _dataSys;
		private Dictionary<string, Statistic> _dataA01;
		private Dictionary<string, Statistic> _dataS01;
		private Dictionary<string, Statistic> _dataA02;
		private Dictionary<string, Statistic> _dataS02;
		private Dictionary<string, Statistic> _dataS03;
		private Dictionary<string, Statistic> _dataA03;
		private Dictionary<string, int> _upperCountSingle01;
		private Dictionary<string, int> _underCountSingle01;
		private Dictionary<string, int> _upperCountSingle02;
		private Dictionary<string, int> _underCountSingle02;
		private Dictionary<string, int> _upperCountSingle03;
		private Dictionary<string, int> _underCountSingle03;

		#endregion

		#region >>> Constructor / Disposor <<<

		public StatisticSet()
		{
			this._testCount = 0;

			this._goodCount01 = 0;

			this._failCount01 = 0;

			this._goodCount02 = 0;

			this._failCount02 = 0;

			this._goodCount03 = 0;

			this._failCount03 = 0;

			this._resultData = new Dictionary<string, TestResultData>();

			this._dataSys = new Dictionary<string, Statistic>();

			this._dataA01 = new Dictionary<string, Statistic>();

			this._dataS01 = new Dictionary<string, Statistic>();

			this._dataA02 = new Dictionary<string, Statistic>();

			this._dataS02 = new Dictionary<string, Statistic>();

			this._dataA03 = new Dictionary<string, Statistic>();

			this._dataS03 = new Dictionary<string, Statistic>();

			this._upperCountSingle01 = new Dictionary<string, int>();

			this._underCountSingle01 = new Dictionary<string, int>();

			this._upperCountSingle02 = new Dictionary<string, int>();

			this._underCountSingle02 = new Dictionary<string, int>();

			this._upperCountSingle03 = new Dictionary<string, int>();

			this._underCountSingle03 = new Dictionary<string, int>();
		}

		#endregion

		#region >>> Public Method <<<

		public void SetData(TestItemData[] testItemDataArray)
		{
			this._testCount = 0;

			this._goodCount01 = 0;

			this._failCount01 = 0;

			this._goodCount02 = 0;

			this._failCount02 = 0;

			this._goodCount03 = 0;

			this._failCount03 = 0;

			this._resultData.Clear();

			this._dataSys.Clear();

			this._dataA01.Clear();

			this._dataS01.Clear();

			this._dataA02.Clear();

			this._dataS02.Clear();

			this._dataA03.Clear();

			this._dataS03.Clear();

			this._upperCountSingle01.Clear();

			this._underCountSingle01.Clear();

			this._upperCountSingle02.Clear();

			this._underCountSingle02.Clear();

			this._upperCountSingle03.Clear();

			this._underCountSingle03.Clear();

			if (testItemDataArray == null || testItemDataArray.Length == 0)
			{
				return;
			}

			foreach (string keyName in Enum.GetNames(typeof(ESysResultItem)))
			{
				this._dataSys.Add(keyName, new Statistic());

				this._dataA01.Add(keyName, new Statistic());

				this._dataS01.Add(keyName, new Statistic());

				this._dataA02.Add(keyName, new Statistic());

				this._dataS02.Add(keyName, new Statistic());

				this._dataA03.Add(keyName, new Statistic());

				this._dataS03.Add(keyName, new Statistic());
			}

			foreach (var testItem in testItemDataArray)
			{
				if (!testItem.IsEnable || testItem.MsrtResult == null || testItem.MsrtResult.Length == 0)
				{
					continue;
				}

				foreach (var data in testItem.MsrtResult)
				{
					this._dataSys.Add(data.KeyName, new Statistic());

					if (!data.IsEnable || !data.IsVision)
					{
						continue;
					}

					this._resultData.Add(data.KeyName, data);

					this._dataA01.Add(data.KeyName, new Statistic());

					this._dataS01.Add(data.KeyName, new Statistic());

					this._dataA02.Add(data.KeyName, new Statistic());

					this._dataS02.Add(data.KeyName, new Statistic());

					this._dataA03.Add(data.KeyName, new Statistic());

					this._dataS03.Add(data.KeyName, new Statistic());

					this._upperCountSingle01.Add(data.KeyName, 0);

					this._underCountSingle01.Add(data.KeyName, 0);

					this._upperCountSingle02.Add(data.KeyName, 0);

					this._underCountSingle02.Add(data.KeyName, 0);

					this._upperCountSingle03.Add(data.KeyName, 0);

					this._underCountSingle03.Add(data.KeyName, 0);
				}
			}
		}

		public void SetData(TestResultData[] data)
		{
			this._testCount = 0;

			this._goodCount01 = 0;

			this._failCount01 = 0;

			this._goodCount02 = 0;

			this._failCount02 = 0;

			this._goodCount03 = 0;

			this._failCount03 = 0;

			this._resultData.Clear();

			this._dataSys.Clear();

			this._dataA01.Clear();

			this._dataS01.Clear();

			this._dataA02.Clear();

			this._dataS02.Clear();

			this._dataA03.Clear();

			this._dataS03.Clear();

			this._upperCountSingle01.Clear();

			this._underCountSingle01.Clear();

			this._upperCountSingle02.Clear();

			this._underCountSingle02.Clear();

			this._upperCountSingle03.Clear();

			this._underCountSingle03.Clear();

			if (data == null || data.Length == 0)
			{
				return;
			}

			foreach (string keyName in Enum.GetNames(typeof(ESysResultItem)))
			{
				this._dataSys.Add(keyName, new Statistic());

				this._dataA01.Add(keyName, new Statistic());

				this._dataS01.Add(keyName, new Statistic());

				this._dataA02.Add(keyName, new Statistic());

				this._dataS02.Add(keyName, new Statistic());

				this._dataA03.Add(keyName, new Statistic());

				this._dataS03.Add(keyName, new Statistic());
			}

			foreach (var item in data)
			{
				this._dataSys.Add(item.KeyName, new Statistic());

				this._resultData.Add(item.KeyName, item);

				this._dataA01.Add(item.KeyName, new Statistic());

				this._dataS01.Add(item.KeyName, new Statistic());

				this._dataA02.Add(item.KeyName, new Statistic());

				this._dataS02.Add(item.KeyName, new Statistic());

				this._dataA03.Add(item.KeyName, new Statistic());

				this._dataS03.Add(item.KeyName, new Statistic());

				this._upperCountSingle01.Add(item.KeyName, 0);

				this._underCountSingle01.Add(item.KeyName, 0);

				this._upperCountSingle02.Add(item.KeyName, 0);

				this._underCountSingle02.Add(item.KeyName, 0);

				this._upperCountSingle03.Add(item.KeyName, 0);

				this._underCountSingle03.Add(item.KeyName, 0);
			}
		}

		public void Push(Dictionary<string, double> data)
		{
			this._testCount++;

			//System Data: ESysResultItem
			foreach (var item in this._dataSys)
			{
				item.Value.Push(data[item.Key]);
			}

			bool isAllPass01 = true;

			bool isAllPass02 = true;

			bool isAllPass03 = true;

			//S01 Statistic
			foreach (var item in data)
			{
				if (!this._resultData.ContainsKey(item.Key) || !this._resultData[item.Key].IsVerify)
				{
					continue;
				}

				string format = string.Empty;

				if (this._resultData.ContainsKey(item.Key))
				{
					format = this._resultData[item.Key].Formate;
				}

				double value = Convert.ToDouble(data[item.Key].ToString(format));

				double max = this._resultData[item.Key].MaxLimitValue;

				double min = this._resultData[item.Key].MinLimitValue;

				if (value > max)
				{
					this._upperCountSingle01[item.Key]++;

					isAllPass01 = false;
				}
				else if (value < min)
				{
					this._underCountSingle01[item.Key]++;

					isAllPass01 = false;
				}
				else if (value >= min && value <= max)
				{
					this._dataS01[item.Key].Push(value);
				}
			}

			//S02 Statistic
			foreach (var item in data)
			{
				if (!this._resultData.ContainsKey(item.Key))
				{
					continue;
				}

				string format = string.Empty;

				if (this._resultData.ContainsKey(item.Key))
				{
					format = this._resultData[item.Key].Formate;
				}

				double value = Convert.ToDouble(data[item.Key].ToString(format));

				double max = this._resultData[item.Key].MaxLimitValue2;

				double min = this._resultData[item.Key].MinLimitValue2;

				if (value > max)
				{
					this._upperCountSingle02[item.Key]++;

					isAllPass02 = false;
				}
				else if (value < min)
				{
					this._underCountSingle02[item.Key]++;

					isAllPass02 = false;
				}
				else if (value >= min && value <= max)
				{
					this._dataS02[item.Key].Push(value);
				}
			}

			//S03 Statistic
			foreach (var item in data)
			{
				if (!this._resultData.ContainsKey(item.Key))
				{
					continue;
				}

				string format = string.Empty;

				if (this._resultData.ContainsKey(item.Key))
				{
					format = this._resultData[item.Key].Formate;
				}

				double value = Convert.ToDouble(data[item.Key].ToString(format));

				double max = this._resultData[item.Key].MaxLimitValue3;

				double min = this._resultData[item.Key].MinLimitValue3;

				if (value > max)
				{
					this._upperCountSingle03[item.Key]++;

					isAllPass03 = false;
				}
				else if (value < min)
				{
					this._underCountSingle03[item.Key]++;

					isAllPass03 = false;
				}
				else if (value >= min && value <= max)
				{
					this._dataS03[item.Key].Push(value);
				}
			}

			//All01 Statistic
			if (isAllPass01)
			{
				this._goodCount01++;

				foreach (var item in this._dataA01)
				{
					item.Value.Push(data[item.Key]);
				}
			}
			else
			{
				this._failCount01++;
			}

			//All02 Statistic
			if (isAllPass02)
			{
				this._goodCount02++;

				foreach (var item in this._dataA02)
				{
					item.Value.Push(data[item.Key]);
				}
			}
			else
			{
				this._failCount02++;
			}

			//All03 Statistic
			if (isAllPass03)
			{
				this._goodCount03++;

				foreach (var item in this._dataA03)
				{
					item.Value.Push(data[item.Key]);
				}
			}
			else
			{
				this._failCount03++;
			}
		}

		public double GoodRateS01(string keyName)
		{
			if (this._dataS01.ContainsKey(keyName) && this._testCount != 0)
			{
				return (double)this._dataS01[keyName].Count * 100.0d / (double)this._testCount;
			}
			else
			{
				return 0;
			}
		}

		public double GoodRateS02(string keyName)
		{
			if (this._dataS02.ContainsKey(keyName) && this._testCount != 0)
			{
				return (double)this._dataS02[keyName].Count * 100.0d / (double)this._testCount;
			}
			else
			{
				return 0;
			}
		}

		public double GoodRateS03(string keyName)
		{
			if (this._dataS03.ContainsKey(keyName) && this._testCount != 0)
			{
				return (double)this._dataS03[keyName].Count * 100.0d / (double)this._testCount;
			}
			else
			{
				return 0;
			}
		}

		public int UpperCountS01(string keyName)
		{
			if (this._upperCountSingle01.ContainsKey(keyName))
			{
				return this._upperCountSingle01[keyName];
			}
			else
			{
				return 0;
			}
		}

		public int UnderCountS01(string keyName)
		{
			if (this._underCountSingle01.ContainsKey(keyName))
			{
				return this._underCountSingle01[keyName];
			}
			else
			{
				return 0;
			}
		}

		public int UpperCountS02(string keyName)
		{
			if (this._upperCountSingle02.ContainsKey(keyName))
			{
				return this._upperCountSingle02[keyName];
			}
			else
			{
				return 0;
			}
		}

		public int UnderCountS02(string keyName)
		{
			if (this._underCountSingle02.ContainsKey(keyName))
			{
				return this._underCountSingle02[keyName];
			}
			else
			{
				return 0;
			}
		}

		public int UpperCountS03(string keyName)
		{
			if (this._upperCountSingle03.ContainsKey(keyName))
			{
				return this._upperCountSingle03[keyName];
			}
			else
			{
				return 0;
			}
		}

		public int UnderCountS03(string keyName)
		{
			if (this._underCountSingle03.ContainsKey(keyName))
			{
				return this._underCountSingle03[keyName];
			}
			else
			{
				return 0;
			}
		}

		public double UpperRateS01(string keyName)
		{
			if (this._testCount == 0)
			{
				return 0.0d;
			}

			return (double)this.UpperCountS01(keyName) / (double)this.TestCount * 100.0d;
		}

		public double UnderRateS01(string keyName)
		{
			if (this._testCount == 0)
			{
				return 0.0d;
			}

			return (double)this.UnderCountS01(keyName) / (double)this.TestCount * 100.0d;
		}

		public double UpperRateS02(string keyName)
		{
			if (this._testCount == 0)
			{
				return 0.0d;
			}

			return (double)this.UpperCountS02(keyName) / (double)this.TestCount * 100.0d;
		}

		public double UnderRateS02(string keyName)
		{
			if (this._testCount == 0)
			{
				return 0.0d;
			}

			return (double)this.UnderCountS02(keyName) / (double)this.TestCount * 100.0d;
		}

		public double UpperRateS03(string keyName)
		{
			if (this._testCount == 0)
			{
				return 0.0d;
			}

			return (double)this.UpperCountS03(keyName) / (double)this.TestCount * 100.0d;
		}

		public double UnderRateS03(string keyName)
		{
			if (this._testCount == 0)
			{
				return 0.0d;
			}

			return (double)this.UnderCountS03(keyName) / (double)this.TestCount * 100.0d;
		}

		public void ResetStatisticData()
		{
			foreach (var item in this._dataSys)
			{
				item.Value.Clear();
			}

			foreach (var item in this._dataA01)
			{
				item.Value.Clear();
			}

			foreach (var item in this._dataS01)
			{
				item.Value.Clear();
			}

			foreach (var item in this._dataA02)
			{
				item.Value.Clear();
			}

			foreach (var item in this._dataS02)
			{
				item.Value.Clear();
			}

			foreach (var item in this._dataA03)
			{
				item.Value.Clear();
			}

			foreach (var item in this._dataS03)
			{
				item.Value.Clear();
			}
		}

		#endregion

		#region >>> Public Property <<<

		public int GoodCount01
		{
			get { return this._goodCount01; }
		}

		public int FailCount01
		{
			get { return this._failCount01; }
		}

		public int GoodCount02
		{
			get { return this._goodCount02; }
		}

		public int FailCount02
		{
			get { return this._failCount02; }
		}

		public int GoodCount03
		{
			get { return this._goodCount03; }
		}

		public int FailCount03
		{
			get { return this._failCount03; }
		}

		public double GoodRateA01
		{
			get 
			{
				if (this._testCount == 0)
				{
					return 0.0d;
				}

				return (double)this._goodCount01 / (double)this._testCount * 100.0d;
			}
		}

		public double FailRateA01
		{
			get 
			{
				if (this._testCount == 0)
				{
					return 0.0d;
				}

				return (double)this._failCount01 / (double)this._testCount * 100.0d; 
			}
		}

		public double GoodRateA02
		{
			get 
			{
				if (this._testCount == 0)
				{
					return 0.0d;
				}

				return (double)this._goodCount02 / (double)this._testCount * 100.0d;
			}
		}

		public double FailRateA02
		{
			get 
			{
				if (this._testCount == 0)
				{
					return 0.0d;
				}

				return (double)this._failCount02 / (double)this._testCount * 100.0d;
			}
		}

		public double GoodRateA03
		{
			get
			{
				if (this._testCount == 0)
				{
					return 0.0d;
				}

				return (double)this._goodCount03 / (double)this._testCount * 100.0d;
			}
		}

		public double FailRateA03
		{
			get
			{
				if (this._testCount == 0)
				{
					return 0.0d;
				}

				return (double)this._failCount03 / (double)this._testCount * 100.0d;
			}
		}

		public Statistic this[string keyName, EStatisticType type]
		{
			get
			{
				Statistic data = null;

				switch (type)
				{
					case EStatisticType.All01:
						{
							if (this._dataA01.ContainsKey(keyName))
							{
								data = this._dataA01[keyName];
							}

							break;
						}
					case EStatisticType.All02:
						{
							if (this._dataA02.ContainsKey(keyName))
							{
								data = this._dataA02[keyName];
							}

							break;
						}
					case EStatisticType.All03:
						{
							if (this._dataA03.ContainsKey(keyName))
							{
								data = this._dataA03[keyName];
							}

							break;
						}
					case EStatisticType.Single01:
						{
							if (this._dataS01.ContainsKey(keyName))
							{
								data = this._dataS01[keyName];
							}

							break;
						}
					case EStatisticType.Single02:
						{
							if (this._dataS02.ContainsKey(keyName))
							{
								data = this._dataS02[keyName];
							}

							break;
						}
					case EStatisticType.Single03:
						{
							if (this._dataS03.ContainsKey(keyName))
							{
								data = this._dataS03[keyName];
							}

							break;
						}
					case EStatisticType.System:
						{
							if (this._dataSys.ContainsKey(keyName))
							{
								data = this._dataSys[keyName];
							}

							break;
						}
					default:
						{
							break;
						}
				}

				if (data == null)
				{
					data = new Statistic();
				}

				return data;
			}
		}

		public int TestCount
		{
			get { return this._testCount; }
		}

		#endregion
	}
}
