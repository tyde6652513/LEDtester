using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Data
{
	[Serializable]
	public class SmartBinDataBase : ICloneable
	{
		#region >>> Private Property <<<

		private static int _totleBinCount = 0;
		private int _serialNumber;
		private int _binNumber;
		private int _autoBinNumber;
		private string _binCode;
		private uint _chipCount;
		private EBinningType _binningType;
        private EBinBoundaryRule _boundaryRule;

		#endregion

		#region >>> Constructor / Disposor <<<

		private SmartBinDataBase()
		{
			if (SmartBinDataBase._totleBinCount == int.MaxValue)
			{
				SmartBinDataBase._totleBinCount = 0;
			}

			SmartBinDataBase._totleBinCount++;

			this._serialNumber = SmartBinDataBase._totleBinCount;// Guid.NewGuid().ToString().Replace("-", "");

			this._binNumber = -1;

			this._binCode = string.Empty;

			this._chipCount = 0;

			this._binningType = EBinningType.IN_BIN;

            this._boundaryRule = EBinBoundaryRule.LeValL;
		}

		public SmartBinDataBase(EBinningType binningType)
			: this()
		{
			this._binningType = binningType;
		}

		#endregion

		#region >>> Public Method <<<

		public object Clone()
		{
			SmartBinDataBase cloneObj = null;

			//Clone
			if (this is SmartBinData)
			{
				cloneObj = new SmartBinData();

				(cloneObj as SmartBinData).BoundarySN = (this as SmartBinData).BoundarySN;
			}
			else if (this is SmartNGData)
			{
				cloneObj = new SmartNGData();

				(cloneObj as SmartNGData).KeyName = (this as SmartNGData).KeyName;

				(cloneObj as SmartNGData).Name = (this as SmartNGData).Name;

				(cloneObj as SmartNGData).NGLowLimit = (this as SmartNGData).NGLowLimit;

				(cloneObj as SmartNGData).NGUpLimit = (this as SmartNGData).NGUpLimit;

				(cloneObj as SmartNGData).IsTester = (this as SmartNGData).IsTester;

				(cloneObj as SmartNGData).IsEnable = (this as SmartNGData).IsEnable;

				(cloneObj as SmartNGData).Format = (this as SmartNGData).Format;
			}
			else if (this is SmartSideData)
			{
				cloneObj = new SmartSideData();

				(cloneObj as SmartSideData).KeyName = (this as SmartSideData).KeyName;

				(cloneObj as SmartSideData).Name = (this as SmartSideData).Name;
			}

			//cloneObj._serialNumber = this._serialNumber;

			cloneObj._binNumber = this._binNumber;

			cloneObj._binNumber = this._binNumber;

			cloneObj._binCode = this._binCode;

			cloneObj._chipCount = this._chipCount;

            cloneObj.BoundaryRule = this.BoundaryRule;

			return cloneObj;
		}

		#endregion

		#region >>> Public Property <<<

		public int SerialNumber
		{
			get { return this._serialNumber; }
		}

		public int BinNumber
		{
			get { return this._binNumber; }
			set { { this._binNumber = value; } }
		}

		public int AutoBinNumber
		{
			get { return this._autoBinNumber; }
			set { { this._autoBinNumber = value; } }
		}

		public string BinCode
		{
			get { return this._binCode; }
			set { { this._binCode = value; } }
		}

		public uint ChipCount
		{
			get { return this._chipCount; }
			set { { this._chipCount = value; } }
		}

		public EBinningType BinningType
		{
			get { return this._binningType; }
		}

        public EBinBoundaryRule BoundaryRule //20181210 David NG Bin Boundary Rule follow by TestResultItem
        {
            set { _boundaryRule = value; }
            get { return _boundaryRule; }
        }

		#endregion
	}
}
