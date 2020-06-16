using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.Tester.Gui
{
	#region >>> Message ID <<<

	/// <summary>
	/// Message ID.
	/// </summary>
	public enum EMessage		// 0 ~ 299
	{
		Processing = 0,
		Finish = 1,
		OK = 5,
		Cancel = 6,
		OpenFile = 7,
		SaveFile = 8,
		FileName = 9,
		NotFound = 10,
		FileSaveComplete = 11,
		Start = 12,
		End = 13,
		Fail = 14,
		Success = 15,
		Exist = 16,
		Empty = 17,
		Handled = 18,
	}

	/// <summary>
	/// Question message ID.
	/// </summary>
	public enum EQuestionMsg	// 300 ~ 499
	{
		AreYouSure					= 300,
		DoYouWantToDeleteFile,
		DoYouWantToSaveFile,
		DoYouWantToOpenFile,
	}

	/// <summary>
	/// Warning message ID.
	/// </summary>
	public enum EWarningMsg		// 500 ~ 699
	{
		IllegalParameter			= 500,
		OutOfRange,
		FileNotFound,
		IllegalUserName,
		IllegalPassword,
		IllegalData,
		NoTarget,
		OperationFailed,
		DeleteFileFailed,
		UserNameExist,
		CantDeleteCurrentUser,
		CantDeleteCurrentRecipe,
		PathNotFound,
		UnauthorizedAccess,
		IllegalFileName,
	}

	/// <summary>
	/// Input message ID.
	/// </summary>
	public enum EInputMsg		// 700 ~ 899
	{
		UserName = 0,
	}

	/// <summary>
	/// Help message ID.
	/// </summary>
	public enum EHelpMsg		// 900 ~ 999
	{
	}

    public enum EBaseFormDisplayUI	// 900 ~ 999
    {     
        HideAll					= 0,
        ResultForm				= 1,
        OperatorForm			= 2,
        ConditionForm       	= 3,
        SystemSettingForm		= 4,
        CalibrationForm			= 5,
        SetProductForm          = 6,
        BinSettingForm          = 7,
        UISettingForm           = 8,
        SpectrumForm			= 9,
        MachineForm				= 10,
		RunningForm          =11,
		ShowResult				= 12,
    }


    public enum EPopUpUIForm	// 900 ~ 999
    {
        BarcodeSettingForm = 0,
        DailyCheckingForm = 1,
        DailyCheckingSettingForm = 2,
        AutoChannelCalibrationForm=3,
        AnalysisForm = 15,

        UserComments = 21,
    }

	#endregion

}
