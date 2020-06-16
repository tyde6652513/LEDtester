using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Drawing;

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

		public static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr( -1 );
		public static readonly IntPtr HWND_MESSAGE_WINDOW = new IntPtr( -3 );
		#endregion

		#region >>> IPC Function <<<

		/// <summary>
		/// IPC function.
		/// </summary>
		public class IPC
		{
			#region >>> DLL Import <<<

			[DllImport( KERNEL32 )]
			public static extern WaitObjectResult WaitForSingleObject( IntPtr handle, uint milliseconds );

			[DllImport( KERNEL32, CharSet = CharSet.Unicode, EntryPoint = "CreateMutexW" )]
			public static extern IntPtr CreateMutex( IntPtr lpMutexAttributes, bool initialOwner, string name );

			[DllImport( KERNEL32, CharSet = CharSet.Unicode, EntryPoint = "OpenMutexW" )]
			public static extern IntPtr OpenMutex( uint desiredAccess, bool inheritHandle, string name );

			[DllImport( KERNEL32 )]
			public static extern bool ReleaseMutex( IntPtr handle );

			[DllImport( KERNEL32, CharSet = CharSet.Unicode, EntryPoint = "CreateEventW" )]
			public static extern IntPtr CreateEvent( IntPtr lpEventAttributes, bool manualReset, bool initialState, string name );

			[DllImport( KERNEL32, CharSet = CharSet.Unicode, EntryPoint = "OpenEventW" )]
			public static extern IntPtr OpenEvent( uint desiredAccess, bool inheritHandle, string name );

			[DllImport( KERNEL32 )]
			public static extern bool PulseEvent( IntPtr handle );

			[DllImport( KERNEL32 )]
			public static extern bool SetEvent( IntPtr handle );

			[DllImport( KERNEL32 )]
			public static extern bool ResetEvent( IntPtr handle );

			[DllImport( KERNEL32 )]
			public static extern int InterlockedExchange( IntPtr addr, int value );

			[DllImport( KERNEL32 )]
			public static extern int InterlockedExchange( ref int oldValue, int newValue );
			#endregion

			#region >>> Public Static Method <<<

			public static IntPtr CreateMutex( bool initialOwner, string name )
			{
				return CreateMutex( IntPtr.Zero, initialOwner, name );
			}

			public static IntPtr OpenMutex( string name )
			{
				return OpenMutex( 0, false, name );
			}

			public static IntPtr CreateEvent( bool manualReset, bool initialState, string name )
			{
				return CreateEvent( IntPtr.Zero, manualReset, initialState, name );
			}

			public static IntPtr OpenEvent( string name )
			{
				return OpenEvent( 0, false, name );
			}

			public static void ReleaseAccessLock( ref int flag )
			{
				InterlockedExchange( ref flag, 0 );
			}

			public static bool IsAccessLock( ref int flag )
			{
				return ( InterlockedExchange( ref flag, 1 ) == 1 );
			}

			/// <summary>
			/// Test Atom action is lock or not
			/// </summary>
			/// <param name="addr">ensure this is in shared memory</param>
			public static bool IsAccessLock( IntPtr addr )
			{
				return ( InterlockedExchange( addr, 1 ) == 1 );
/*
			#define LOCK_SET 1
			#define LOCK_CLEAR 0

			int* lock_location = LOCK_LOCATION; // ensure this is in shared memory
			if (InterlockedExchange(lock_location, LOCK_SET) == LOCK_CLEAR)
			{
				 return true; // got the lock
			}
			else
			{
				 return false; // didn't get the lock
			}
*/
			}
			#endregion
		}

		#endregion

		#region >>> Time Function <<<

		/// <summary>
		/// Time function.
		/// </summary>
		public class TimeFunction
		{
			[DllImport( KERNEL32 )]
			public static extern bool GetSystemTime( [In] ref SYSTEMTIME st );

			[DllImport( KERNEL32 )]
			public static extern bool SetSystemTime( [In] ref SYSTEMTIME st );
		}

		#endregion

		#region >>> Mapping File <<<

		/// <summary>
		/// Mapping File function.
		/// </summary>
		public class MappingFile
		{
			[DllImport( KERNEL32, SetLastError = true )]
			public static extern IntPtr CreateFileMapping( IntPtr hFile, FileMapAccess lpSecurityAttributes,
				FileMapProtection flProtect, uint dwMaximumSizeHigh, uint dwMaximumSizeLow, string lpName );

			[DllImport( KERNEL32 )]
			public static extern IntPtr OpenFileMapping( FileMapAccess dwDesiredAccess, bool bInheritHandle, string lpName );

			[DllImport( KERNEL32, SetLastError = true )]
			public static extern bool UnmapViewOfFile( IntPtr lpBaseAddress );

			[DllImport( KERNEL32, SetLastError = true )]
			public static extern IntPtr MapViewOfFile( IntPtr hFileMappingObject, FileMapAccess dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow, uint dwNumberOfBytesToMap );

			[DllImport( KERNEL32, SetLastError = true )]
			public static extern bool FlushViewOfFile( IntPtr lpBaseAddress, uint dwNumberOfBytesToFlush );

			/// <summary>
			/// Share memory.
			/// </summary>
			public class ShareMemory
			{
				/// <summary>
				/// Create share memory.
				/// </summary>
				public static bool CreateSharedMemory( int size, string name, out IntPtr address, out IntPtr handle )
				{
					return CreateSharedMemory( ( uint ) size, name, out address, out handle );
				}

				public static bool CreateSharedMemory( uint size, string name, out IntPtr address, out IntPtr handle )
				{
					address = IntPtr.Zero;

					handle = CreateFileMapping( INVALID_HANDLE_VALUE, 0, FileMapProtection.PageReadWrite | FileMapProtection.SectionCommit, 0, size, name );

					if ( ( handle == IntPtr.Zero ) || ( handle == Win32API.INVALID_HANDLE_VALUE ) )
						return false;

					address = MapViewOfFile( handle, FileMapAccess.Full, 0, 0, size );

					return true;
				}

				/// <summary>
				/// Open share memory.
				/// </summary>
				public static bool OpenSharedMemory( int size, string name, out IntPtr address, out IntPtr handle )
				{
					return OpenSharedMemory( ( uint ) size, name, out address, out  handle );
				}

				public static bool OpenSharedMemory( uint size, string name, out IntPtr address, out IntPtr handle )
				{
					address = IntPtr.Zero;

					handle = OpenFileMapping( FileMapAccess.Full, false, name );

					if ( ( handle == IntPtr.Zero ) || ( handle == Win32API.INVALID_HANDLE_VALUE ) )
						return false;

					address = MapViewOfFile( handle, FileMapAccess.Full, 0, 0, size );

					return true;
				}

				/// <summary>
				/// Close share memory.
				/// </summary>
				public static void CloseShareMemory( IntPtr address, IntPtr handle )
				{
					// unmapping memory
					Win32API.MappingFile.UnmapViewOfFile( address );

					Win32API.CloseHandle( handle );
				}
			}

			public static IntPtr CreateFileMapping( uint lowMaxSize, string name )
			{
				return CreateFileMapping( INVALID_HANDLE_VALUE, 0,
					( FileMapProtection.SectionCommit | FileMapProtection.PageReadWrite ), 0, lowMaxSize, name );
			}

			public static IntPtr OpenFileMapping( string name )
			{
				return ( IntPtr ) OpenFileMapping( FileMapAccess.Full, false, name );
			}

			public static IntPtr MapViewOfFile( IntPtr handle, uint size )
			{
				return MapViewOfFile( handle, FileMapAccess.Full, 0, 0, size );
			}

			public static IntPtr MapViewOfFile( IntPtr handle, uint offset, uint size )
			{
				return MapViewOfFile( handle, FileMapAccess.Full, 0, offset, size );
			}

			//[DllImport( "ADVAPI32.DLL" )]
			//static extern bool InitializeSecurityDescriptor( out SECURITY_ATTRIBUTES attr, uint dwRevision );

			[DllImport( KERNEL32, SetLastError = true )]
			static extern IntPtr CreateFileMapping( IntPtr hFile, ref SECURITY_ATTRIBUTES lpSecurityAttributes,
				FileMapProtection flProtect, uint dwMaximumSizeHigh, uint dwMaximumSizeLow, string lpName );

			public static IntPtr CreateFileMapping( uint size, bool dummySecurity )
			{
				SECURITY_ATTRIBUTES security = new SECURITY_ATTRIBUTES( 1 );
				return CreateFileMapping( INVALID_HANDLE_VALUE, ref security, FileMapProtection.PageReadWrite, 0, size, null );
			}

			public static IntPtr CreateFileMapping( IntPtr handle, FileMapProtection protection, uint size, bool dummySecurity )
			{
				SECURITY_ATTRIBUTES security = new SECURITY_ATTRIBUTES( 1 );
				return CreateFileMapping( handle, ref security, protection, 0, size, null );
			}
		}


		#endregion

		#region >>> Memory <<<

		/// <summary>
		/// Memory function.
		/// </summary>
		public class Memory
		{
			[DllImport( KERNEL32 )]
			public static extern void CopyMemory( IntPtr destination, IntPtr source, uint count );

			[DllImport( KERNEL32 )]
			public static extern void CopyMemory( IntPtr destination, IntPtr source, int count );

			public static void CopyMemory( IntPtr destination, uint offset, IntPtr source, uint count )
			{
				//$RIC, x64
				CopyMemory( new IntPtr( destination.ToInt64() + offset ), source, count );
			}

			public static void CopyMemory( IntPtr dest, uint offset, IntPtr src, uint offset2, uint count )
			{
				CopyMemory( new IntPtr( dest.ToInt64() + offset ),
					new IntPtr( src.ToInt64() + offset2 ), count );
			}

			[DllImport( KERNEL32 )]
			public static extern void MoveMemory( IntPtr destination, IntPtr source, uint count );

			[DllImport( KERNEL32 )]
			public static extern void FillMemory( IntPtr destination, uint count, byte data );

			[DllImport( KERNEL32 )]
			public static extern void ZeroMemory( IntPtr destination, uint count );

			[DllImport( KERNEL32 )]
			public static extern void ZeroMemory( IntPtr dest, int count );

			[DllImport( NTDLL, EntryPoint = "memcpy" )]
			private static extern void WriteShort( IntPtr addrDest, ref short value, int size );

			[DllImport( NTDLL, EntryPoint = "memcpy" )]
			private static extern void ReadShort( out short value, IntPtr addrSrc, int size );

			[DllImport( NTDLL, EntryPoint = "memcpy" )]
			private static extern void WriteInt( IntPtr addrDest, ref int value, int size );

			[DllImport( NTDLL, EntryPoint = "memcpy" )]
			private static extern void ReadInt( out int value, IntPtr addrSrc, int size );

			[DllImport( NTDLL, EntryPoint = "memcpy" )]
			private static extern void WriteFloat( IntPtr addr, ref float value, int size );

			[DllImport( NTDLL, EntryPoint = "memcpy" )]
			private static extern void ReadFloat( out float value, IntPtr addrSrc, int size );

			[DllImport( NTDLL, EntryPoint = "memcpy" )]
			private static extern void WriteLong( IntPtr addrDest, ref long value, int size );

			[DllImport( NTDLL, EntryPoint = "memcpy" )]
			private static extern void ReadLog( out long value, IntPtr addrSrc, int size );

			#region >>> Shortcut function <<<
			public static Int16 ReadInt16( IntPtr addr, uint offset )
			{
				short value;
				ReadShort( out value, ( IntPtr ) ( ( long ) addr + offset ), sizeof( short ) );
				return value;
			}

			public static Int16 ReadInt16( IntPtr addr )
			{
				int value;
				ReadInt( out value, addr, sizeof( short ) );
				return ( Int16 ) value;
			}

			public static Int32 ReadInt32( IntPtr addr )
			{
				int value;
				ReadInt( out value, addr, sizeof( int ) );
				return value;
			}

			public static Int32 ReadInt32( IntPtr addr, uint offset )
			{
				int value;
				ReadInt( out value, ( IntPtr ) ( ( long ) addr + offset ), sizeof( int ) );
				return value;
			}

			public static Int64 ReadInt64( IntPtr addr )
			{
				long value;
				ReadLog( out value, addr, sizeof( long ) );
				return value;
			}

			public static float ReadFloat( IntPtr addr )
			{
				float value;
				ReadFloat( out value, addr, sizeof( float ) );
				return value;
			}

			public static float ReadFloat( IntPtr addr, uint offset )
			{
				float value;
				ReadFloat( out value, ( IntPtr ) ( ( long ) addr + offset ), sizeof( float ) );
				return value;
			}

			public static void ReadFloatArray( IntPtr addr, float[] dataArray )
			{
				IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement( dataArray, 0 );
				CopyMemory( ptr, addr, sizeof( float ) * dataArray.Length );
			}

			public static void ReadFloatArray( IntPtr addr, uint offset, float[] dataArray )
			{
				IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement( dataArray, 0 );
				CopyMemory( ptr, ( IntPtr ) ( ( long ) addr + offset ), sizeof( float ) * dataArray.Length );
			}

			public static void WriteInt16( IntPtr addr, Int16 value )
			{
				WriteShort( addr, ref value, sizeof( short ) );
			}

			public static void WriteInt16( IntPtr addr, uint offset, Int16 value )
			{
				WriteShort( ( IntPtr ) ( ( long ) addr + offset ), ref value, sizeof( short ) );
			}

			public static void WriteInt32( IntPtr addr, Int32 value )
			{
				WriteInt( addr, ref value, sizeof( int ) );
			}

			public static void WriteInt32( IntPtr addr, uint offset, Int32 value )
			{
				WriteInt( ( IntPtr ) ( ( long ) addr + offset ), ref value, sizeof( int ) );
			}

			public static void WriteFloat( IntPtr addr, float value )
			{
				WriteFloat( addr, ref value, sizeof( float ) );
			}

			public static void WriteFloat( IntPtr addr, uint offset, float value )
			{
				WriteFloat( ( IntPtr ) ( ( long ) addr + offset ), ref value, sizeof( float ) );
			}

			public static void WriteFloatArray( IntPtr addr, float[] dataArray )
			{
				IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement( dataArray, 0 );
				CopyMemory( addr, ptr, sizeof( float ) * dataArray.Length );
			}

			public static void WriteFloatArray( IntPtr addr, uint offset, float[] dataArray )
			{
				IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement( dataArray, 0 );
				CopyMemory( ( IntPtr ) ( ( long ) addr + offset ), ptr, sizeof( float ) * dataArray.Length );
			}

			public static void WriteLong( IntPtr addr, long value )
			{
				WriteLong( addr, ref value, sizeof( long ) );
			}

			#region >>> Virtual Memory <<<

			[DllImport( KERNEL32, SetLastError = true )]
			public static extern IntPtr VirtualAlloc( IntPtr lpAddress, uint dwSize, EAllocationType flAllocationType, EMemoryProtection flProtect );

			[DllImport( KERNEL32, SetLastError = true )]
			public static extern bool VirtualFree( IntPtr lpAddress, uint dwSize, EMemoryFreeType dwFreeType );

			#endregion

			#endregion
		}

		#endregion

		#region >>> Window <<<

		/// <summary>
		/// Window function.
		/// </summary>
		public class Window
		{
			#region >>> Behavior <<<

			[DllImport( USER32 )]
			public static extern bool BringWindowToTop( IntPtr hWnd );

			[DllImport( USER32 )]
			public static extern bool UpdateWindow( IntPtr hWnd );

			[DllImport( USER32 )]
			public static extern bool MoveWindow( IntPtr hWnd, int x, int y, int width, int height, bool repaint );

			#endregion

			#region >>> Send Message <<<

			[Flags]
			public enum SmtoFlags : uint
			{
				Normal = 0x00,
				Block = 0x01,
				AbortIfHung = 0x02,
				NoTimeoutIfNotHung = 0x08,
			};

			[DllImport( USER32, SetLastError = true )]
			public static extern IntPtr SendMessage( IntPtr handle, uint message, IntPtr wParam, IntPtr lParam );

			[DllImport( USER32, SetLastError = true )]
			[return: MarshalAs( UnmanagedType.Bool )]
			public static extern bool PostMessage( IntPtr handle, uint message, IntPtr wParam, IntPtr lParam );

			[DllImport( USER32, SetLastError = true )]
			public static extern IntPtr SendMessageTimeout( IntPtr handle, uint message, IntPtr wParam, IntPtr lParam,
				SmtoFlags flags, uint timeout, out IntPtr result );

			[DllImport( USER32 )]
			[return: MarshalAs( UnmanagedType.Bool )]
			public static extern bool SendNotifyMessage( IntPtr handle, uint message, IntPtr wParam, IntPtr lParam );

			[DllImport( USER32 )]
			[return: MarshalAs( UnmanagedType.Bool )]
			public static extern bool InSendMessage();

			[DllImport( USER32 )]
			public static extern bool ReplyMessage( IntPtr result );

			#endregion

			#region >>> Identify <<<
			public delegate bool EnumWindowsProc( IntPtr hWnd, ref TProcessWindow expected );

			[DllImport( USER32 )]
			public static extern uint GetWindowThreadProcessId( IntPtr hWnd, out uint pid );

			[DllImport( USER32 )]
			[return: MarshalAs( UnmanagedType.Bool )]
			public static extern bool IsWindow( IntPtr handle );

			[DllImport( USER32 )]
			[return: MarshalAs( UnmanagedType.Bool )]
			public static extern bool EnumWindows( EnumWindowsProc proc, ref TProcessWindow param );

			[DllImport( USER32 )]
			public static extern IntPtr FindWindow( string className, string windowName );

			[DllImport( USER32, SetLastError = true )]
			public static extern IntPtr FindWindowEx( IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow );

			[DllImport( USER32 )]
			public static extern IntPtr GetDesktopWindow();
			#endregion

			#region >>> Life Cycle <<<

			[DllImport( USER32, SetLastError = true )]
			public static extern IntPtr CreateWindowEx( uint dwExStyle, string lpClassName, string lpWindowName, uint dwStyle,
					  int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam );

			[DllImport( USER32 )]
			public static extern bool DestroyWindow( IntPtr hWnd );

			[DllImport( USER32, SetLastError = true )]
			public static extern IntPtr SetWindowLong( IntPtr hWnd, int nIndex, IntPtr dwNewLong );

			[DllImport( USER32, SetLastError = true )]
			public static extern IntPtr CallWindowProc( IntPtr lpPrevWndFunc, IntPtr handle, uint message, IntPtr wParam, IntPtr lParam );

			#endregion

			[DllImport( USER32 )]
			public static extern bool ClientToScreen( IntPtr hWnd, ref Point point );

			[DllImport( USER32 )]
			public static extern bool OffsetRect( ref TRect rect, int dx, int dy );

		}

		#endregion

		[DllImport( KERNEL32, SetLastError = true )]
		public static extern bool CloseHandle( IntPtr Handle );

		[DllImport( USER32 )]
		public static extern void PostQuitMessage( int exitCode );

		[DllImport( KERNEL32 )]
		public static extern void GetSystemInfo( out SYSTEM_INFO lpSystemInfo );

		[DllImport( KERNEL32, SetLastError = true )]
		public static extern IntPtr CreateFile(
			string lpFileName,
			EFileAccess dwDesiredAccess,
			EFileShare dwShareMode,
			IntPtr lpSecurityAttributes,
			ECreationDisposition dwCreationDisposition,
			EFileAttributes dwFlagsAndAttributes,
			IntPtr hTemplateFile );
	}
}
