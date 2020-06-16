using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.Tester.TestKernel
{
	public enum ECoordSet : int
	{
		First					= 1,
		Second					= 2,
		Third					= 3,
		Fourth					= 4,
	}

    public enum EKernelState : int
    {
        Not_Ready				= -1,
        Ready					= 1,
        Running					= 2,
        Error					= 3,
    }

    public enum ETesterKernelCmd
    { 
        RunTest				    = 1,	// 1-base
		ManualRun				= 2,
        StopTest				= 3,
        ConfirmDataReceived		= 4,
        ResetKernelData			= 5,
		
		CheckMachineHW			= 6,
		ConfirmErrorMsg			= 7,
        GetDarkDataAndSave		= 8,
		ShortTestIF				= 9,
		OpenTestIF				= 10,

		AbortOpenShortTestIF	= 11,
		SimulatorRun			= 12,
        EndTest					= 13,
        ResetMachineHW          = 14,
        RunSingleRetest         = 15,
        RunDeviceVerify         = 18,

        GetPDDarkCurrent        = 19,
        RunLcrCalibration       = 20,
        RunOsaCoupling          = 21,
        RunSrcOutput            = 22,
        RunAttenuator           = 23,
		CameraLiveMode         =101,
        CameraTriggerMode      = 102,
    }
}
