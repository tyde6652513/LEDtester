using System;

namespace MPI.UCF.Forms.Domain
{
	/// <summary>
	/// Die map type.
	/// </summary>
	public enum EDieMapType
	{
		XY = 0,
		RowCol
	}

	public enum EDieStatus
	{
		Normal = 0,
		Erased,
		Picked,
		Missing,
		NotExist,
		Inked,
		Marked,
		Skiped,
		Bad,
	}

	public enum EGradeColorMethod
	{
		KBlendColor,
		Blend2Color,
		WebSafeColor,
		Customized,
	}

}
