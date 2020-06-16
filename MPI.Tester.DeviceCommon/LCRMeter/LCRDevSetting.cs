using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
	public class LCRDevSetting : System.ICloneable
	{
		ELCRDCBiasType _lcrDCBiasType = ELCRDCBiasType.Internal;

		public LCRDevSetting()
		{ 
		
		}

		public object Clone()
		{
			return this.MemberwiseClone();
		}

		public ELCRDCBiasType LCRDCBiasType
		{
			get { return this._lcrDCBiasType; }
			set { this._lcrDCBiasType = value; }
		}
	}
}
