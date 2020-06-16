using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MPI.Tester.Device.SourceMeter
{
	public class WheWrapper
	{
		#region >>> Import DLL <<<

		[DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "TESTER_INIT")]
		private static extern int TESTER_INIT();

		[DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "TESTER_CLOSE")]
		private static extern int TESTER_CLOSE();

		[DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "TESTER_RESET")]
		private static extern void TESTER_RESET();
		
		[DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "WAIT")]
		private static extern void WAIT(double delay);		// unit in mSec, Reso. 200nS

		[DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "PMU_CONNECT")]
		private static extern void PMU_CONNECT();

		[DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "PMU_DISCONNECT")]
		private static extern void PMU_DISCONNECT();

		[DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "VCOM")]
		private static extern void VCOM(double volt, double delay);		//volt -10V ~ +10V]

		[DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "PMU_FV")]
		private static extern void PMU_FV(double volt,  int vrng, int irng, double iclh, double icll, double delay);		

		[DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "PMU_FI")]
		private static extern void PMU_FI(double current, int vrng, int irng, double vclh, double vcll, double delay);	

		[DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "PMU_MV")]
		private static extern double PMU_MV(double delay);		

		[DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "PMU_AMV")]
		private static extern double PMU_AMV(double delay);		

		[DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "PMU_MI")]
		private static extern double PMU_MI(double delay);

		[DllImport(@"C:\MPI\LEDTester\Driver\TESTAPI.dll", EntryPoint = "PMU_AMI")]
		private static extern double PMU_AMI(double delay);

		#endregion

		#region >>> Public Method <<<

		public int Init()
		{
			return TESTER_INIT();
		}

		public int Close()
		{
			return TESTER_CLOSE();
		}

		public void Reset()
		{
			TESTER_RESET();
		}

		public void HWTimerWait(double delay)
		{
			WAIT(delay);
		}

		public void Connect()
		{
			PMU_CONNECT();
		}

		public void DisConnect()
		{
			PMU_DISCONNECT();
		}

		public void CommonVolt(double volt, double delay)
		{
			VCOM(volt, delay);
		}			

		public void ForceV(double volt, int voltRange, int currentRange, double currentClampHigh, double currentClampLow, double delay)
		{
			PMU_FV(volt, voltRange, currentRange, currentClampHigh, currentClampLow, delay);
		}

		public void ForceI(double current, int voltRange, int currentRange, double voltClampHigh, double voltClampLow, double delay)
		{
			PMU_FI(current, voltRange, currentRange, voltClampHigh, voltClampLow, delay);
		}

		public double MeasureV(double delay)
		{
			return PMU_MV(delay);
		}

		public double MeasureAvgV(double delay)
		{
			return PMU_AMV(delay);
		}

		public double MeasureI(double delay)
		{
			return PMU_MI(delay);
		}

		public double MeasureAvgI(double delay)
		{
			return PMU_AMI(delay);
		}

		#endregion
	}
}
