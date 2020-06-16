using System;
using System.Runtime.InteropServices;

namespace MPI.Win32
{
	[StructLayout( LayoutKind.Sequential )]
	public struct SYSTEMTIME
	{
		public short wYear;
		public short wMonth;
		public short wDayOfWeek;
		public short wDay;
		public short wHour;
		public short wMinute;
		public short wSecond;
		public short wMilliseconds;
	}

	public struct TProcessWindow
	{
		public uint PId;
		public IntPtr WHandle;
	};

	[StructLayout( LayoutKind.Sequential )]
	public struct TRect
	{
		public int Left;
		public int Top;

		public int Right;
		public int Bottom;

		public TRect( int left, int top, int right, int bottom )
		{
			this.Left = left;
			this.Top = top;
			this.Right = right;
			this.Bottom = bottom;
		}

		public TRect( int width, int height )
		{
			this.Left = 0;
			this.Top = 0;
			this.Right = width;
			this.Bottom = height;
		}

		public TRect( IntPtr hwnd, int width, int height )
			: this( width, height )
		{
			System.Drawing.Point pnt = new System.Drawing.Point();
			Win32.Win32API.Window.ClientToScreen( hwnd, ref pnt );
			Win32.Win32API.Window.OffsetRect( ref this, pnt.X, pnt.Y );
		}

		public void Anchor( IntPtr hwnd )
		{
			System.Drawing.Point pnt = new System.Drawing.Point();
			Win32.Win32API.Window.ClientToScreen( hwnd, ref pnt );

			this.Right -= this.Left;
			this.Bottom -= this.Top;
			this.Left = pnt.X;
			this.Top = pnt.Y;
			this.Right += this.Left;
			this.Bottom += this.Top;
		}

		public int Width
		{
			get
			{
				return this.Right - this.Left;
			}
		}

		public int Height
		{
			get
			{
				return this.Bottom - this.Top;
			}
		}
	}

	[StructLayout( LayoutKind.Sequential )]
	public struct SYSTEM_INFO
	{
		internal PROCESSOR_ARCHITECTURE uProcessorInfo;
		public uint dwPageSize;
		public IntPtr lpMinimumApplicationAddress;
		public IntPtr lpMaximumApplicationAddress;
		public IntPtr dwActiveProcessorMask;
		public uint dwNumberOfProcessors;
		public uint dwProcessorType;
		public uint dwAllocationGranularity;
		public ushort dwProcessorLevel;
		public ushort dwProcessorRevision;

		public static readonly uint ALLOCATION_GRANULARITY;  // default normaly: 64K

		static SYSTEM_INFO()
		{
			SYSTEM_INFO info;
			Win32API.GetSystemInfo( out info );
			ALLOCATION_GRANULARITY = info.dwAllocationGranularity;
		}
	}

	[StructLayout( LayoutKind.Explicit )]
	internal struct PROCESSOR_ARCHITECTURE
	{
		[FieldOffset( 0 )]
		internal uint dwOemId;
	
		[FieldOffset( 0 )]
		internal ushort wProcessorArchitecture;

		[FieldOffset( 2 )]
		internal ushort wReserved;
	}

	[StructLayout( LayoutKind.Sequential, Pack = 1 )]
	internal struct SECURITY_ATTRIBUTES
	{
		uint Length;
		IntPtr Descriptor;
		bool Inherited;

		internal SECURITY_ATTRIBUTES( uint revision )
		{
			this.Length = 1;
			this.Descriptor = IntPtr.Zero;
			this.Inherited = false;
		}
	}

}
