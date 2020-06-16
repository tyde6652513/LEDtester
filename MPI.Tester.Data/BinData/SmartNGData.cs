using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Data
{
	[Serializable]
	public class SmartNGData : SmartBinDataBase
	{
		#region >>> Private Property <<<

		private string _name;
		private string _keyName;
		private bool _isTester;
		private bool _isEnable;
		private double _ngUpLimit;
		private double _ngLowLimit;
		private string _format;

		#endregion

		#region >>> Constructor / Disposor <<<

		public SmartNGData()
			: base(EBinningType.NG_BIN)
		{
			this._name = string.Empty;

			this._keyName = string.Empty;

			this._isTester = true;

			this._isEnable = false;

			this._ngUpLimit = double.MaxValue;

			this._ngLowLimit = double.MinValue;

			this.BinCode = "NG-Bin";

			this.BinNumber = -1;

			this._format = "E2";
		}

		public SmartNGData(string name)
			: this()
		{
			this.BinCode = name + " NG-Bin";
		}

		#endregion

		#region >>> Public Method <<<

		public bool IsNGBin(double value)
		{

            bool isNG = false;

            switch (this.BoundaryRule)
            {
                default:
                case EBinBoundaryRule.LeValL:
                    {
                        if (value < this.NGLowLimit || value >= this.NGUpLimit)
                        {
                            isNG = true;
                        }
                    }
                    break;
                case EBinBoundaryRule.LeValLe:
                    {
                        if ( value < this.NGLowLimit || value > this.NGUpLimit)
                        {
                            isNG = true;
                        }
                    }
                    break;
                case EBinBoundaryRule.LValL:
                    {
                        if (value <= this.NGLowLimit || value >= this.NGUpLimit)
                        {
                            isNG = true;
                        }
                    }
                    break;
                case EBinBoundaryRule.LValLe:
                    {
                        if ( value <= this.NGLowLimit || value > this.NGUpLimit)
                        {
                            isNG = true;
                        }
                    }
                    break;
            }

            if (isNG)
            {
                this.ChipCount++;                
            }
            return isNG;
            //if (value == double.MinValue || value < this.NGLowLimit || value > this.NGUpLimit)
            //{
            //    this.ChipCount++;

            //    return true;
            //}

            //return false;
		}

        public Dictionary<string, object> GetBinInfo()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Name", Name);
            dic.Add("KeyName", KeyName);
            dic.Add("IsRecipeTeste", IsTester);
            dic.Add("IsEnableBinning", IsEnable);
            dic.Add("UpLimit", NGUpLimit);
            dic.Add("LowLimit", NGLowLimit);
            dic.Add("BoundaryRule", BoundaryRule.ToString());
            return dic;

        }
		#endregion

		#region >>> Public Property <<<

		public string Name
		{
			get { return this._name; }
			set { { this._name = value; } }
		}

		public string KeyName
		{
			get { return this._keyName; }
			set { { this._keyName = value; } }
		}

		public bool IsTester
		{
			get { return this._isTester; }
			set { { this._isTester = value; } }
		}

		public bool IsEnable
		{
			get { return this._isEnable; }
			set { { this._isEnable = value; } }
		}

		public double NGUpLimit
		{
			get { return this._ngUpLimit; }
			set { { this._ngUpLimit = value; } }
		}

		public double NGLowLimit
		{
			get { return this._ngLowLimit; }
			set { { this._ngLowLimit = value; } }
		}

		public string Format
		{
			get { return this._format; }
			set { { this._format = value; } }
		}

		#endregion
	}
}
