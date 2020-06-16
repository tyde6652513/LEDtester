using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MPI.Tester.Gui
{
	public static class InputIdleTime
	{
		#region >>> DLL Inport <<<

		[DllImport("user32.dll")]
		private static extern bool GetLastInputInfo(ref LastInputInfo plii);

		#endregion

		#region >>> Struct Define <<<

		[StructLayout(LayoutKind.Sequential)]
		private struct LastInputInfo
		{
			[MarshalAs(UnmanagedType.U4)]
			public int cbSize;

			[MarshalAs(UnmanagedType.U4)]
			public uint dwTime;
		}

		#endregion

		#region >>> Delegate Define <<<

		public delegate void IdleEventHandler();

		#endregion

		#region >>> Private Property <<<

		private static bool _isStart = false;
        private static int _idleTime = 5000;
        //private static int _idleTime = 360000; // unit : ms => default 6分鐘後自動登出
		private static Timer _timer = new System.Windows.Forms.Timer();

		#endregion

		#region >>> Public Property <<<

		public static event IdleEventHandler IdleEvent;

		public static bool IsStart
		{
			get { return InputIdleTime._isStart; }
		}

		#endregion

		#region >>> Private Static Method <<<

		private static int GetIdleTick()
		{
			LastInputInfo lastInputInfo = new LastInputInfo();

			lastInputInfo.cbSize = Marshal.SizeOf(lastInputInfo);

			if (!InputIdleTime.GetLastInputInfo(ref　lastInputInfo))
			{
				return 0;
			}

			return Environment.TickCount - (int)lastInputInfo.dwTime;
		}

		private static void Timer_Tick(object sender, EventArgs e)
		{
			InputIdleTime._timer.Stop();

			if (InputIdleTime.GetIdleTick() >= InputIdleTime._idleTime)
			{
				if (InputIdleTime.IdleEvent != null)
				{
					InputIdleTime.Stop();

					InputIdleTime.IdleEvent();
				}
			}

			InputIdleTime._timer.Start();
		}

		#endregion

		#region >>> Public Static Method <<<

		public static void Start()
		{
			if (InputIdleTime._isStart)
			{
				return;
			}

			InputIdleTime._timer.Tick += InputIdleTime.Timer_Tick;

			InputIdleTime._timer.Interval = 100;

			InputIdleTime._timer.Start();

			InputIdleTime._isStart = true;
		}

		public static void Stop()
		{
			InputIdleTime._timer.Stop();

			InputIdleTime._timer.Tick -= InputIdleTime.Timer_Tick;

			InputIdleTime._isStart = false;
		}

		#endregion
	}
}
