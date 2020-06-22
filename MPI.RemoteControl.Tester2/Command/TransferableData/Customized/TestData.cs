using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.RemoteControl2.Tester.Mpi.Command.Base
{
	public class TestData
	{
		public enum Status
		{
			NoTest = 0,
			Pass = 1,
			Fail = 2,
		}

		private enum DataItem
		{
			TestStatus = 0,
			Column,
			Row,
			Bin,
			ResultCount,
			BinCode,
			TestResults,
		}

		private static int DATA_ITEM_COUNT = Enum.GetValues(typeof(DataItem)).Length;

		private CmdPropertyBased _command;
		private int _nIndex;

		internal TestData(CmdPropertyBased cmd, int nIndex)
		{
			_command = cmd;
			_nIndex = nIndex;
		}

		#region >>> Public property <<<

		/// <summary>
		/// The testing result status.
		/// </summary>
		public Status TestStatus
		{
			get { return (Status)Enum.Parse(typeof(Status), this.GetValue<int>(DataItem.TestStatus, Status.NoTest.GetHashCode()).ToString()); }
			set { this.SetValue<int>(DataItem.TestStatus, value.GetHashCode()); }
		}

		/// <summary>
		/// Column of the tesing unit.
		/// </summary>
		public int Column
		{
			get { return this.GetValue<int>(DataItem.Column, 0); }
			set { this.SetValue<int>(DataItem.Column, value); }
		}

		/// <summary>
		/// Row of the tesing unit.
		/// </summary>
		public int Row
		{
			get { return this.GetValue<int>(DataItem.Row, 0); }
			set { this.SetValue<int>(DataItem.Row, value); }
		}

		/// <summary>
		/// Bin of the tesing unit.
		/// </summary>
		public int Bin
		{
			get { return this.GetValue<int>(DataItem.Bin, 0); }
			set { this.SetValue<int>(DataItem.Bin, value); }
		}

		/// <summary>
		/// Total result count of the tesing unit.
		/// </summary>
		public int TestResultCount
		{
			get { return this.GetValue<int>(DataItem.ResultCount, 0); }
			set { this.SetValue<int>(DataItem.ResultCount, value); }
		}

		/// <summary>
		/// Binning code of the tesing unit.
		/// </summary>
		public string BinCode
		{
			get { return this.GetValue<string>(DataItem.BinCode, ""); }
			set { this.SetValue<string>(DataItem.BinCode, value); }
		}

		/// <summary>
		/// Get a testing results copy.
		/// </summary>
		public double[] TestResults
		{
			get
			{
				double[] results = new double[this.TestResultCount];

				for (int nIndex = 0; nIndex < results.Length; ++nIndex)
					results[nIndex] = this.GetTestResult<double>(nIndex);
				
				return results;
			}
		}

		/// <summary>
		/// Get the testing result with index.
		/// </summary>
		/// <param name="nIndex">This value should be between 0 ~ (ResultCount - 1).</param>
		/// <returns>The testing result value.</returns>
		/// <exception cref="IndexOutOfRangeException"></exception>
		public T GetTestResult<T>(int nIndex)
		{
			ValidateTestResultIndex(nIndex);

			return this.GetValue<T>(FormatTestResultName(nIndex), default(T));
		}

		/// <summary>
		/// Set the testing result with index.
		/// </summary>
		/// <param name="nIndex">This value should be between 0 ~ (TestResultCount - 1).</param>
		/// <param name="dValue">The testing result value.</param>
		/// <exception cref="IndexOutOfRangeException"></exception>
		public void SetTestResult<T>(int nIndex, T value)
		{
			ValidateTestResultIndex(nIndex);

			this.SetValue<T>(FormatTestResultName(nIndex), value);
		}

		#endregion

		#region >>> Private method <<<

		private void ValidateTestResultIndex(int nIndex)
		{
			int nTotalCount = this.TestResultCount;

			if (nIndex < 0 || nIndex >= nTotalCount)
				throw new IndexOutOfRangeException(String.Format("Index:{0} is out of boundary. {1}", nIndex, (nTotalCount == 0) ? "No testing result is available." : String.Format("It should be between:0 ~ {1}.", nTotalCount - 1)));
		}

		private T GetValue<T>(string name, T defaultValue)
		{
			dynamic value = _command.GetProperty(name);

			if (value is T) return value;

			try
			{
				object o = Convert.ChangeType(value, typeof(T));
				
				return o != null ? (T)o : defaultValue;
			}
			catch
			{
			return defaultValue;
		}
		}

		private void SetValue<T>(string name, T value)
		{
			_command.SetProperty(name, value);
		}

		private T GetValue<T>(DataItem item, T defaultValue)
		{
			return GetValue<T>(FormatName(item), defaultValue);
		}

		private void SetValue<T>(DataItem item, T value)
		{
			SetValue<T>(FormatName(item), value);
		}

		private string FormatName(DataItem item)
		{
			//return String.Format("{0}#{1}", item.ToString(), _nIndex);
			return String.Format("T{0}#{1}", item.GetHashCode(), _nIndex);
		}

		private string FormatTestResultName(int nIndex)
		{
			return String.Format("{0}_{1}", FormatName(DataItem.TestResults), nIndex.ToString("X"));
		}

		#endregion
	}
}
