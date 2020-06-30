using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MPI.Tester.Data
{
	public enum EBaseWindowPosition : int
	{
		Default				= 0,
		Bottom				= 1,
		Right				= 2,
		AutoFind			= 3
	}

	public enum EDisplayAxisSelection : int
	{
		In_Out				= 0,
		Out_In				= 1,
		Time_In				= 2,
		Time_Out			= 3,
	}

	public enum EBtnActionMode : int
	{
		NewTestItem			     = 0,
		UpdateTestItem		     = 1,
		InsertTestItem		     = 2,
		DeleteTestItem		     = 3,
        ChangeTestItemOrder_Up   = 4,
        ChangeTestItemOrder_Down = 5,
        ChangeTestItemEnable     = 6,
		CopyTestItem			 = 7,
	}

	public enum EOutputFileNamePresent
	{
		[XmlEnum(Name = "0")]
		BarCode				= 0,

		[XmlEnum(Name = "1")]
		WaferNum			= 1,

		[XmlEnum(Name = "2")]
		LotNum_WaferNum		= 2,

		[XmlEnum(Name = "3")]
		LotSpaceWafer		= 3,

		[XmlEnum(Name = "4")]
		Customer01			= 4,

		[XmlEnum(Name = "5")]
		Customer02			= 5,

        [XmlEnum(Name = "6")]
        WaferNum_Stage = 6,
	}

	public enum EUIDisplayType : int    // 0-base
	{
		[XmlEnum(Name = "0")]
		MPIStartUI			= 0,

		[XmlEnum(Name = "1")]
		WMStartUI			= 1,
	}

	public enum EUIOperateMode : int
	{
		[XmlEnum(Name = "0")]
		AutoRun		= 0,

		[XmlEnum(Name = "1")]
		ManulRun	= 1,

		[XmlEnum(Name = "2")]
		Idle				= 2,
	}

	public enum EMultiLanguage : int
	{
		[XmlEnum(Name = "0")]
		ENG					= 0,

		[XmlEnum(Name = "1")]
		CHT					= 1,

        [XmlEnum(Name = "2")]
        JPN                 = 2,
	}


}
