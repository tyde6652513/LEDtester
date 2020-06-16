using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Data
{
	[Serializable]
	public class SmartBinningData
	{
		#region >>> Private Property <<<

		private bool _isAutoBin;
		private bool _isAutoNGBin;
		private bool _isAutoSideBin;
		private Dictionary<int, int> _autoBinData;
		private SmartBin _bin;
		private SmartNGBin _ngBin;
		private SmartSideBin _sideBin;

		#endregion

		#region >>> Constructor / Disposor <<<

		public SmartBinningData()
		{
			this._isAutoBin = false;

			this._isAutoNGBin = false;

			this._isAutoSideBin = false;

			this._autoBinData = new Dictionary<int, int>();

			this._bin = new SmartBin();

			this._ngBin = new SmartNGBin();

			this._sideBin = new SmartSideBin();
		}

		#endregion

		#region >>> Public Method <<<

		public void ClearChipCount()
		{
			this._autoBinData.Clear();

			this._bin.ClearChipCount();

			this._ngBin.ClearChipCount();

			this._sideBin.ClearChipCount();
		}

		public object Clone()
		{
			SmartBinningData cloneObj = new SmartBinningData();

			cloneObj._isAutoBin = this._isAutoBin;

			cloneObj._isAutoNGBin = this._isAutoNGBin;

			cloneObj._isAutoSideBin = this._isAutoSideBin;

			foreach (var item in this._autoBinData)
			{
				cloneObj._autoBinData.Add(item.Key, item.Value);
			}

			cloneObj._bin = this._bin.Clone() as SmartBin;

			cloneObj._ngBin = this._ngBin.Clone() as SmartNGBin;

			cloneObj._sideBin = this._sideBin.Clone() as SmartSideBin;

			return cloneObj;
		}

		#endregion

		#region >>> Public Property <<<

		public int ChipCount
		{
			get 
			{
				int chipCount = 0;

				chipCount += this._bin.ChipCount;

				chipCount += this._ngBin.ChipCount;

				chipCount += this._sideBin.ChipCount;

				return chipCount; 
			}
		}

		public int Count
		{
			get 
			{
				int binCount = 0;

				binCount += this._bin.Count;

				binCount += this._ngBin.Count;

				binCount += this._sideBin.Count;

				return binCount; 
			}
		}

		public bool IsAutoBin
		{
			get { return this._isAutoBin; }
			set { this._isAutoBin = value; }
		}

		public bool IsAutoNGBin
		{
			get { return this._isAutoNGBin; }
			set { this._isAutoNGBin = value; }
		}

		public bool IsAutoSideBin
		{
			get { return this._isAutoSideBin; }
			set { this._isAutoSideBin = value; }
		}

		public Dictionary<int, int> AutoBinData
		{
			get { return this._autoBinData; }
			set { this._autoBinData = value; }
		}

		public SmartBin SmartBin
		{
			get { return this._bin; }
			set { this._bin = value; }
		}

		public SmartNGBin NGBin
		{
			get { return this._ngBin; }
			set { this._ngBin = value; }
		}

		public SmartSideBin SideBin
		{
			get { return this._sideBin; }
			set { this._sideBin = value; }
		}

		#endregion
	}
}
