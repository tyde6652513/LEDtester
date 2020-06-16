using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPI.Tester.Maths;
using MPI.Tester.Data;

namespace MPI.Tester.TestKernel
{
	public class PassRateCheck
	{
		private bool _isEnable = false;

		private Dictionary<string, Statistic> _dicStatistic = new Dictionary<string, Statistic>();

		private double testCount = 0;

		private bool _isStop;

		private string _errMsg;

		private EPassRateCheckNGMode _mode;

		private double _minCount;

		public PassRateCheck()
		{
			_isStop = false;

			this._errMsg = string.Empty;

			_mode = EPassRateCheckNGMode.STOP_TEST;

			_isEnable = false;

			_minCount = 10;
		}

		public bool IsStopZapESD
		{
			get
			{
				if (this._mode == EPassRateCheckNGMode.STOP_ZAP_ESD)
				{
					return this._isStop;
				}
				else
				{
					return false;
				}
			}
		}

		public bool IsStopTest
		{
			get
			{
				if (this._mode == EPassRateCheckNGMode.STOP_TEST)
				{
					return this._isStop;
				}
				else
				{
					return false;
				}
			}
		}

		public string ErrorMsg
		{
			get { return this._errMsg; }
		}

		public bool Start(MPI.Tester.Data.TesterSetting setting, TestItemData[] data)
		{
			_isStop = false;

			this._mode = setting.PassRateCheckMode;

			this._isEnable = false;

            if (!setting.IsEnablePassRateCheck || data == null)
			{
				this._isEnable = false;

				return true;
			}

			this._minCount = setting.MinCountOfRunningPassRateCheck;

			this._isEnable = true;

			testCount = 0;

			_dicStatistic.Clear();

			foreach (TestItemData item in data)
			{
				if (item.MsrtResult != null)
				{
					for (int i = 0; i < item.MsrtResult.Length; i++)
					{
						if (item.MsrtResult[i].IsEnablePassRateCheck && item.MsrtResult[i].MinPassRatePercent > 1)
						{
							_dicStatistic.Add(item.MsrtResult[i].KeyName, new Statistic());
						}
					}
				}
			}

			return true;

		}

		public bool Start(bool isEnable, EPassRateCheckNGMode mode, TestItemData[] data)
		{
			_isStop = false;

			this._mode = mode;

			this._isEnable = false;

            if (!isEnable || data == null)
			{
				this._isEnable = false;

				return true;
			}

			this._isEnable = true;

			testCount = 0;

			_dicStatistic.Clear();

			foreach (TestItemData item in data)
			{
				if (item.MsrtResult != null)
				{
					for (int i = 0; i < item.MsrtResult.Length; i++)
					{
						if (item.MsrtResult[i].IsEnablePassRateCheck && item.MsrtResult[i].MinPassRatePercent > 1)
						{
							_dicStatistic.Add(item.MsrtResult[i].KeyName, new Statistic());
						}
					}
				}
			}

			return true;
		}

		public bool End()
		{
			return true;
		}

		public bool Push(TestItemData[] data)
		{
			if (_isEnable == false)
			{
				this._isStop = false;

				return true;
			}

			bool isPass = true;

			this._errMsg = string.Empty;

			testCount++;

			foreach (TestItemData item in data)
			{
				if (item.MsrtResult != null)
				{
					for (int i = 0; i < item.MsrtResult.Length; i++)
					{
						if (item.MsrtResult[i].IsEnablePassRateCheck)
						{
							if (this._dicStatistic.ContainsKey(item.MsrtResult[i].KeyName))
							{
								if (item.MsrtResult[i].IsPass)
								{
									this._dicStatistic[item.MsrtResult[i].KeyName].Push(item.MsrtResult[i].Value);
								}
								// Check Pass Rate is meet Spec // 

								double passRate = 0.0d;

								if (testCount >= this._minCount)
								{
									passRate = (this._dicStatistic[item.MsrtResult[i].KeyName].Count / testCount) * 100;
								}
								else
								{
									passRate = 100.0d;
								}

								if (passRate < item.MsrtResult[i].MinPassRatePercent)
								{
									isPass &= false;

									this._errMsg = item.Name + " good rate fail.";
								}
							}
						}
					}
				}
			}

			this._isStop = !isPass;

			return isPass;
		}
	}
}
