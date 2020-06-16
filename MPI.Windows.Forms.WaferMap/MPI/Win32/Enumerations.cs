using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.Win32
{
	///// <summary>
	///// ProtectionTypes.
	///// </summary>
	//public enum ProtectionTypes : int
	//{
	//   PageReadOnly = 0x2,
	//   PageReadWrite = 0x4,
	//   PageWriteCopy = 0x8
	//}

	///// <summary>
	///// SectionTypes.
	///// </summary>
	//public enum SectionTypes : int
	//{
	//   SecCommit = 0x8000000,
	//   SecImage = 0x1000000,
	//   SecNoCache = 0x10000000,
	//   SecReserve = 0x4000000
	//}

	/// <summary>
	/// AccessTypes.
	/// </summary>
	[Flags]
	public enum FileMapAccess : uint
	{
		Copy = 0x0001,
		Write = 0x0002,
		Read = 0x0004,
		ReadWrite = Read | Write,
		CopyWriteRead = 0x000F0007,
		Execute = 0x0008,
		Extend = 0x0010,
		Full = 0x000F001F,
	}

	/// <summary>
	/// Protection Types.
	/// </summary>
	[Flags]
	public enum FileMapProtection : uint
	{
		PageReadonly = 0x0002,
		PageReadWrite = 0x0004,
		PageWriteCopy = 0x0008,
		PageExecuteRead = 0x0020,
		PageExecuteReadWrite = 0x0040,
		SectionCommit = 0x08000000,
		SectionImage = 0x01000000,
		SectionNoCache = 0x10000000,
		SectionReserve = 0x04000000
	}

	/// <summary>
	/// Wait object result.
	/// </summary>
	public enum WaitObjectResult : uint
	{
		WAIT_ABANDONED = 0x00000080,
		WAIT_OBJECT_0 = 0x00000000,
		WAIT_TIMEOUT = 0x00000102,
	}

	public enum WmEnum : uint
	{
		WM_NULL = 0x00,
		WM_CREATE = 0x01,
		WM_DESTROY = 0x02,
		WM_MOVE = 0x03,
		WM_SIZE = 0x05,
		WM_ACTIVATE = 0x06,
		WM_SETFOCUS = 0x07,
		WM_KILLFOCUS = 0x08,
		WM_ENABLE = 0x0A,
		WM_SETREDRAW = 0x0B,
		WM_SETTEXT = 0x0C,
		WM_GETTEXT = 0x0D,
		WM_GETTEXTLENGTH = 0x0E,
		WM_PAINT = 0x0F,
		WM_CLOSE = 0x10,
		WM_QUERYENDSESSION = 0x11,
		WM_QUIT = 0x12,
		WM_QUERYOPEN = 0x13,
		WM_ERASEBKGND = 0x14,
		WM_SYSCOLORCHANGE = 0x15,
		WM_ENDSESSION = 0x16,
		WM_SYSTEMERROR = 0x17,
		WM_SHOWWINDOW = 0x18,
		WM_CTLCOLOR = 0x19,
		WM_WININICHANGE = 0x1A,
		WM_SETTINGCHANGE = 0x1A,
		WM_DEVMODECHANGE = 0x1B,
		WM_ACTIVATEAPP = 0x1C,
		WM_FONTCHANGE = 0x1D,
		WM_TIMECHANGE = 0x1E,
		WM_CANCELMODE = 0x1F,
		WM_SETCURSOR = 0x20,
		WM_MOUSEACTIVATE = 0x21,
		WM_CHILDACTIVATE = 0x22,
		WM_QUEUESYNC = 0x23,
		WM_GETMINMAXINFO = 0x24,
		WM_PAINTICON = 0x26,
		WM_ICONERASEBKGND = 0x27,
		WM_NEXTDLGCTL = 0x28,
		WM_SPOOLERSTATUS = 0x2A,
		WM_DRAWITEM = 0x2B,
		WM_MEASUREITEM = 0x2C,
		WM_DELETEITEM = 0x2D,
		WM_VKEYTOITEM = 0x2E,
		WM_CHARTOITEM = 0x2F,

		WM_SETFONT = 0x30,
		WM_GETFONT = 0x31,
		WM_SETHOTKEY = 0x32,
		WM_GETHOTKEY = 0x33,
		WM_QUERYDRAGICON = 0x37,
		WM_COMPAREITEM = 0x39,
		WM_COMPACTING = 0x41,
		WM_WINDOWPOSCHANGING = 0x46,
		WM_WINDOWPOSCHANGED = 0x47,
		WM_POWER = 0x48,
		WM_COPYDATA = 0x4A,
		WM_CANCELJOURNAL = 0x4B,
		WM_NOTIFY = 0x4E,
		WM_INPUTLANGCHANGEREQUEST = 0x50,
		WM_INPUTLANGCHANGE = 0x51,
		WM_TCARD = 0x52,
		WM_HELP = 0x53,
		WM_USERCHANGED = 0x54,
		WM_NOTIFYFORMAT = 0x55,
		WM_CONTEXTMENU = 0x7B,
		WM_STYLECHANGING = 0x7C,
		WM_STYLECHANGED = 0x7D,
		WM_DISPLAYCHANGE = 0x7E,
		WM_GETICON = 0x7F,
		WM_SETICON = 0x80,

		WM_NCCREATE = 0x81,
		WM_NCDESTROY = 0x82,
		WM_NCCALCSIZE = 0x83,
		WM_NCHITTEST = 0x84,
		WM_NCPAINT = 0x85,
		WM_NCACTIVATE = 0x86,
		WM_GETDLGCODE = 0x87,
		WM_NCMOUSEMOVE = 0xA0,
		WM_NCLBUTTONDOWN = 0xA1,
		WM_NCLBUTTONUP = 0xA2,
		WM_NCLBUTTONDBLCLK = 0xA3,
		WM_NCRBUTTONDOWN = 0xA4,
		WM_NCRBUTTONUP = 0xA5,
		WM_NCRBUTTONDBLCLK = 0xA6,
		WM_NCMBUTTONDOWN = 0xA7,
		WM_NCMBUTTONUP = 0xA8,
		WM_NCMBUTTONDBLCLK = 0xA9,

		WM_KEYFIRST = 0x100,
		WM_KEYDOWN = 0x100,
		WM_KEYUP = 0x101,
		WM_CHAR = 0x102,
		WM_DEADCHAR = 0x103,
		WM_SYSKEYDOWN = 0x104,
		WM_SYSKEYUP = 0x105,
		WM_SYSCHAR = 0x106,
		WM_SYSDEADCHAR = 0x107,
		WM_KEYLAST = 0x108,

		WM_IME_STARTCOMPOSITION = 0x10D,
		WM_IME_ENDCOMPOSITION = 0x10E,
		WM_IME_COMPOSITION = 0x10F,
		WM_IME_KEYLAST = 0x10F,

		WM_INITDIALOG = 0x110,
		WM_COMMAND = 0x111,
		WM_SYSCOMMAND = 0x112,
		WM_TIMER = 0x113,
		WM_HSCROLL = 0x114,
		WM_VSCROLL = 0x115,
		WM_INITMENU = 0x116,
		WM_INITMENUPOPUP = 0x117,
		WM_MENUSELECT = 0x11F,
		WM_MENUCHAR = 0x120,
		WM_ENTERIDLE = 0x121,

		WM_CTLCOLORMSGBOX = 0x132,
		WM_CTLCOLOREDIT = 0x133,
		WM_CTLCOLORLISTBOX = 0x134,
		WM_CTLCOLORBTN = 0x135,
		WM_CTLCOLORDLG = 0x136,
		WM_CTLCOLORSCROLLBAR = 0x137,
		WM_CTLCOLORSTATIC = 0x138,

		WM_MOUSEFIRST = 0x200,
		WM_MOUSEMOVE = 0x200,
		WM_LBUTTONDOWN = 0x201,
		WM_LBUTTONUP = 0x202,
		WM_LBUTTONDBLCLK = 0x203,
		WM_RBUTTONDOWN = 0x204,
		WM_RBUTTONUP = 0x205,
		WM_RBUTTONDBLCLK = 0x206,
		WM_MBUTTONDOWN = 0x207,
		WM_MBUTTONUP = 0x208,
		WM_MBUTTONDBLCLK = 0x209,
		WM_MOUSEWHEEL = 0x20A,
		WM_MOUSEHWHEEL = 0x20E,

		WM_PARENTNOTIFY = 0x210,
		WM_ENTERMENULOOP = 0x211,
		WM_EXITMENULOOP = 0x212,
		WM_NEXTMENU = 0x213,
		WM_SIZING = 0x214,
		WM_CAPTURECHANGED = 0x215,
		WM_MOVING = 0x216,
		WM_POWERBROADCAST = 0x218,
		WM_DEVICECHANGE = 0x219,

		WM_MDICREATE = 0x220,
		WM_MDIDESTROY = 0x221,
		WM_MDIACTIVATE = 0x222,
		WM_MDIRESTORE = 0x223,
		WM_MDINEXT = 0x224,
		WM_MDIMAXIMIZE = 0x225,
		WM_MDITILE = 0x226,
		WM_MDICASCADE = 0x227,
		WM_MDIICONARRANGE = 0x228,
		WM_MDIGETACTIVE = 0x229,
		WM_MDISETMENU = 0x230,
		WM_ENTERSIZEMOVE = 0x231,
		WM_EXITSIZEMOVE = 0x232,
		WM_DROPFILES = 0x233,
		WM_MDIREFRESHMENU = 0x234,

		WM_IME_SETCONTEXT = 0x281,
		WM_IME_NOTIFY = 0x282,
		WM_IME_CONTROL = 0x283,
		WM_IME_COMPOSITIONFULL = 0x284,
		WM_IME_SELECT = 0x285,
		WM_IME_CHAR = 0x286,
		WM_IME_KEYDOWN = 0x290,
		WM_IME_KEYUP = 0x291,

		WM_MOUSEHOVER = 0x2A1,
		WM_NCMOUSELEAVE = 0x2A2,
		WM_MOUSELEAVE = 0x2A3,

		WM_CUT = 0x300,
		WM_COPY = 0x301,
		WM_PASTE = 0x302,
		WM_CLEAR = 0x303,
		WM_UNDO = 0x304,

		WM_RENDERFORMAT = 0x305,
		WM_RENDERALLFORMATS = 0x306,
		WM_DESTROYCLIPBOARD = 0x307,
		WM_DRAWCLIPBOARD = 0x308,
		WM_PAINTCLIPBOARD = 0x309,
		WM_VSCROLLCLIPBOARD = 0x30A,
		WM_SIZECLIPBOARD = 0x30B,
		WM_ASKCBFORMATNAME = 0x30C,
		WM_CHANGECBCHAIN = 0x30D,
		WM_HSCROLLCLIPBOARD = 0x30E,
		WM_QUERYNEWPALETTE = 0x30F,
		WM_PALETTEISCHANGING = 0x310,
		WM_PALETTECHANGED = 0x311,

		WM_HOTKEY = 0x312,
		WM_PRINT = 0x317,
		WM_PRINTCLIENT = 0x318,

		WM_HANDHELDFIRST = 0x358,
		WM_HANDHELDLAST = 0x35F,
		WM_PENWINFIRST = 0x380,
		WM_PENWINLAST = 0x38F,
		WM_COALESCE_FIRST = 0x390,
		WM_COALESCE_LAST = 0x39F,
		WM_DDE_FIRST = 0x3E0,
		WM_DDE_INITIATE = 0x3E0,
		WM_DDE_TERMINATE = 0x3E1,
		WM_DDE_ADVISE = 0x3E2,
		WM_DDE_UNADVISE = 0x3E3,
		WM_DDE_ACK = 0x3E4,
		WM_DDE_DATA = 0x3E5,
		WM_DDE_REQUEST = 0x3E6,
		WM_DDE_POKE = 0x3E7,
		WM_DDE_EXECUTE = 0x3E8,
		WM_DDE_LAST = 0x3E8,

		WM_USER = 0x0400,
		WM_APP = 0x8000,
		WM_APP_END = 0xBFFF,
	}

	[Flags]
	public enum EFileAccess : uint
	{
		//
		// Standart Section
		//

		AccessSystemSecurity = 0x1000000,   // AccessSystemAcl access type
		MaximumAllowed = 0x2000000,     // MaximumAllowed access type

		Delete = 0x10000,
		ReadControl = 0x20000,
		WriteDAC = 0x40000,
		WriteOwner = 0x80000,
		Synchronize = 0x100000,

		StandardRightsRequired = 0xF0000,
		StandardRightsRead = ReadControl,
		StandardRightsWrite = ReadControl,
		StandardRightsExecute = ReadControl,
		StandardRightsAll = 0x1F0000,
		SpecificRightsAll = 0xFFFF,

		FILE_READ_DATA = 0x0001,        // file & pipe
		FILE_LIST_DIRECTORY = 0x0001,       // directory
		FILE_WRITE_DATA = 0x0002,       // file & pipe
		FILE_ADD_FILE = 0x0002,         // directory
		FILE_APPEND_DATA = 0x0004,      // file
		FILE_ADD_SUBDIRECTORY = 0x0004,     // directory
		FILE_CREATE_PIPE_INSTANCE = 0x0004, // named pipe
		FILE_READ_EA = 0x0008,          // file & directory
		FILE_WRITE_EA = 0x0010,         // file & directory
		FILE_EXECUTE = 0x0020,          // file
		FILE_TRAVERSE = 0x0020,         // directory
		FILE_DELETE_CHILD = 0x0040,     // directory
		FILE_READ_ATTRIBUTES = 0x0080,      // all
		FILE_WRITE_ATTRIBUTES = 0x0100,     // all

		//
		// Generic Section
		//
		GenericRead = 0x80000000,
		GenericWrite = 0x40000000,
		GenericExecute = 0x20000000,
		GenericAll = 0x10000000,

		SPECIFIC_RIGHTS_ALL = 0x00FFFF,
		FILE_ALL_ACCESS =
		StandardRightsRequired |
		Synchronize |
		0x1FF,

		FILE_GENERIC_READ =
		StandardRightsRead |
		FILE_READ_DATA |
		FILE_READ_ATTRIBUTES |
		FILE_READ_EA |
		Synchronize,

		FILE_GENERIC_WRITE =
		StandardRightsWrite |
		FILE_WRITE_DATA |
		FILE_WRITE_ATTRIBUTES |
		FILE_WRITE_EA |
		FILE_APPEND_DATA |
		Synchronize,

		FILE_GENERIC_EXECUTE =
		StandardRightsExecute |
		  FILE_READ_ATTRIBUTES |
		  FILE_EXECUTE |
		  Synchronize
	}

	[Flags]
	public enum EFileShare : uint
	{
		/// <summary>
		///
		/// </summary>
		None = 0x00000000,
		/// <summary>
		/// Enables subsequent open operations on an object to request read access.
		/// Otherwise, other processes cannot open the object if they request read access.
		/// If this flag is not specified, but the object has been opened for read access, the function fails.
		/// </summary>
		Read = 0x00000001,
		/// <summary>
		/// Enables subsequent open operations on an object to request write access.
		/// Otherwise, other processes cannot open the object if they request write access.
		/// If this flag is not specified, but the object has been opened for write access, the function fails.
		/// </summary>
		Write = 0x00000002,
		/// <summary>
		/// Enables subsequent open operations on an object to request delete access.
		/// Otherwise, other processes cannot open the object if they request delete access.
		/// If this flag is not specified, but the object has been opened for delete access, the function fails.
		/// </summary>
		Delete = 0x00000004
	}

	public enum ECreationDisposition : uint
	{
		/// <summary>
		/// Creates a new file. The function fails if a specified file exists.
		/// </summary>
		New = 1,
		/// <summary>
		/// Creates a new file, always.
		/// If a file exists, the function overwrites the file, clears the existing attributes, combines the specified file attributes,
		/// and flags with FILE_ATTRIBUTE_ARCHIVE, but does not set the security descriptor that the SECURITY_ATTRIBUTES structure specifies.
		/// </summary>
		CreateAlways = 2,
		/// <summary>
		/// Opens a file. The function fails if the file does not exist.
		/// </summary>
		OpenExisting = 3,
		/// <summary>
		/// Opens a file, always.
		/// If a file does not exist, the function creates a file as if dwCreationDisposition is CREATE_NEW.
		/// </summary>
		OpenAlways = 4,
		/// <summary>
		/// Opens a file and truncates it so that its size is 0 (zero) bytes. The function fails if the file does not exist.
		/// The calling process must open the file with the GENERIC_WRITE access right.
		/// </summary>
		TruncateExisting = 5
	}

	[Flags]
	public enum EFileAttributes : uint
	{
		Readonly = 0x00000001,
		Hidden = 0x00000002,
		System = 0x00000004,
		Directory = 0x00000010,
		Archive = 0x00000020,
		Device = 0x00000040,
		Normal = 0x00000080,
		Temporary = 0x00000100,
		SparseFile = 0x00000200,
		ReparsePoint = 0x00000400,
		Compressed = 0x00000800,
		Offline = 0x00001000,
		NotContentIndexed = 0x00002000,
		Encrypted = 0x00004000,
		Write_Through = 0x80000000,
		Overlapped = 0x40000000,
		NoBuffering = 0x20000000,
		RandomAccess = 0x10000000,
		SequentialScan = 0x08000000,
		DeleteOnClose = 0x04000000,
		BackupSemantics = 0x02000000,
		PosixSemantics = 0x01000000,
		OpenReparsePoint = 0x00200000,
		OpenNoRecall = 0x00100000,
		FirstPipeInstance = 0x00080000
	}

	#region >>> Virtual Memory <<<

	[Flags]
	public enum EAllocationType : uint
	{
		COMMIT = 0x1000,
		RESERVE = 0x2000,
		RESET = 0x80000,
		TOP_DOWN = 0x100000,
		PHYSICAL = 0x400000,
		LARGE_PAGES = 0x20000000,
	}

	[Flags]
	public enum EMemoryProtection : uint
	{
		NOACCESS = 0x01,
		READONLY = 0x02,
		READWRITE = 0x04,
		WRITECOPY = 0x08,
		EXECUTE = 0x10,
		EXECUTE_READ = 0x20,
		EXECUTE_READWRITE = 0x40,
		EXECUTE_WRITECOPY = 0x80,
	}

	public enum EMemoryFreeType
	{
		MEM_DECOMMIT = 0x4000,
		MEM_RELEASE = 0x8000,
	}
	#endregion

}
