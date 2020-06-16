using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MPI.Tester.Data
{
	public enum EWheelMsrtSource : int 
	{
		[XmlEnum(Name = "0")]
		LPT					= 0,					
				
		[XmlEnum(Name = "1")]
		SourceMeterIO		= 1,
	}

    public enum ETesterFunctionType : int
    {
        [XmlEnum(Name = "0")]
        Single_Die = 0,

        [XmlEnum(Name = "1")]
        Multi_Die = 1,

        [XmlEnum(Name = "2")]
        Multi_Pad = 2,

        [XmlEnum(Name = "3")]
        Multi_Map = 3,

        [XmlEnum(Name = "4")]
        Multi_Terminal = 4,
    }

    public enum ETesterSequenceType : int
    {
        [XmlEnum(Name = "1")]
        Series = 1,

        [XmlEnum(Name = "2")]
        Parallel = 2,
    }
}
