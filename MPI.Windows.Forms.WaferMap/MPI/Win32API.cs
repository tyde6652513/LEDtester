using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace MPI.Win32
{
	/// <summary>
	/// Win32API class.
	/// </summary>
	public class Win32API
	{
		private const string KERNEL32 = "KERNEL32.dll";
		private const string USER32 = "USER32.dll";
		private const string MSVCRT = "MSVCRT.dll";
		private const string GDI32 = "GDI32.dll";
		private const string NTDLL = "NTDLL.dll";

		#region >>> Constant <<<

		public const int INVALID_HANDLE_VALUE = -1;

		#endregion		

		#region >>> Memory <<<

		/// <summary>
		/// Memory function.
		/// </summary>
		public class Memory
		{
            [DllImport(KERNEL32, EntryPoint = "RtlMoveMemory")]
            public static extern void CopyMemory(IntPtr dest, IntPtr src, Int32 count);
            //[DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
            //public static extern IntPtr CopyMemory(IntPtr dest, IntPtr src, int count); 

			[DllImport( KERNEL32, EntryPoint = "RtlMoveMemory" )]
			public static extern void MoveMemory( IntPtr dest, IntPtr src, Int32 count );

			[DllImport( KERNEL32, EntryPoint = "RtlFillMemory" )]
			public static extern void FillMemory( IntPtr dest, Int32 count, Byte data );

			[DllImport( KERNEL32, EntryPoint = "RtlZeroMemory" )]
			public static extern void ZeroMemory( IntPtr dest, Int32 count );
		}

		#endregion
		
		[DllImport( KERNEL32 )]
		public static extern void CloseHandle( IntPtr Handle );

		[DllImport( USER32 )]
		public static extern void PostQuitMessage( int exitCode );

	}
}
