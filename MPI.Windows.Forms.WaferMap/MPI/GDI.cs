using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace MPI.Drawing
{
	#region >>> Enumeration <<<

	public enum ERegionError : int
	{
		/// <summary>
		/// An error occurred. (The previous clipping region is unaffected.)
		/// </summary>
		ERROR = 0,
		/// <summary>
		/// Region is empty.
		/// </summary>
		NULLREGION = 1,
		/// <summary>
		/// Region is a single rectangle.
		/// </summary>
		SIMPLEREGION = 2,
		/// <summary>
		/// Region is more than one rectangle.
		/// </summary>
		COMPLEXREGION = 3,
	}

	public enum ECreateBitmap
	{
		/// <summary>
		/// If clear, the data pointed to by those parameters is not used.
		/// </summary>
		CBM_NULL = 0x00,

		/// <summary>
		/// initialize bitmap, if set, the system uses the data pointed to by the lpbInit 
		/// and lpbmi parameters to initialize the bitmap bits.
		/// </summary>
		CBM_INIT = 0x04
	}

	public enum EFontWeight : int
	{
		FW_DONTCARE = 0,
		FW_THIN = 100,
		FW_EXTRALIGHT = 200,
		FW_LIGHT = 300,
		FW_NORMAL = 400,
		FW_MEDIUM = 500,
		FW_SEMIBOLD = 600,
		FW_BOLD = 700,
		FW_EXTRABOLD = 800,
		FW_HEAVY = 900,
	}

	public enum EFontCharSet : byte
	{
		ANSI_CHARSET = 0,
		DEFAULT_CHARSET = 1,
		SYMBOL_CHARSET = 2,
		GB2312_CHARSET = 134,
		CHINESEBIG5_CHARSET = 136,
		OEM_CHARSET = 255,
		EASTEUROPE_CHARSET = 238,
	}

	public enum EFontPrecision : byte
	{
		OUT_DEFAULT_PRECIS = 0,
		OUT_STRING_PRECIS = 1,
		OUT_CHARACTER_PRECIS = 2,
		OUT_STROKE_PRECIS = 3,
		OUT_TT_PRECIS = 4,
		OUT_DEVICE_PRECIS = 5,
		OUT_RASTER_PRECIS = 6,
		OUT_TT_ONLY_PRECIS = 7,
		OUT_OUTLINE_PRECIS = 8,
		OUT_SCREEN_OUTLINE_PRECIS = 9,
		OUT_PS_ONLY_PRECIS = 10,
	}

	public enum EFontClipPrecision : byte
	{
		CLIP_DEFAULT_PRECIS = 0,
		CLIP_CHARACTER_PRECIS = 1,
		CLIP_STROKE_PRECIS = 2,
		CLIP_MASK = 0xf,
		CLIP_LH_ANGLES = ( 1 << 4 ),
		CLIP_TT_ALWAYS = ( 2 << 4 ),
		CLIP_DFA_DISABLE = ( 4 << 4 ),
		CLIP_EMBEDDED = ( 8 << 4 ),
	}

	public enum EFontQuality : byte
	{
		DEFAULT_QUALITY = 0,
		DRAFT_QUALITY = 1,
		PROOF_QUALITY = 2,
		NONANTIALIASED_QUALITY = 3,
		ANTIALIASED_QUALITY = 4,
		CLEARTYPE_QUALITY = 5,
		CLEARTYPE_NATURAL_QUALITY = 6,
	}

	[Flags]
	public enum EFontPitchAndFamily : byte
	{
		DEFAULT_PITCH = 0,
		FIXED_PITCH = 1,
		VARIABLE_PITCH = 2,
		FF_DONTCARE = ( 0 << 4 ),
		FF_ROMAN = ( 1 << 4 ),
		FF_SWISS = ( 2 << 4 ),
		FF_MODERN = ( 3 << 4 ),
		FF_SCRIPT = ( 4 << 4 ),
		FF_DECORATIVE = ( 5 << 4 ),
	}

	[Flags]
	public enum EPenStyle : uint
	{
		PS_SOLID = 0x00000000, //The pen is solid.
		PS_DASH = 0x00000001, //The pen is dashed.
		PS_DOT = 0x00000002, //The pen is dotted.
		PS_DASHDOT = 0x00000003, //The pen has alternating dashes and dots.
		PS_DASHDOTDOT = 0x00000004, //The pen has alternating dashes and double dots.
		PS_NULL = 0x00000005, //The pen is invisible.
		PS_INSIDEFRAME = 0x00000006,// Normally when the edge is drawn, it’s centred on the outer edge meaning that half the width of the pen is drawn
		// outside the shape’s edge, half is inside the shape’s edge. When PS_INSIDEFRAME is specified the edge is drawn
		//completely inside the outer edge of the shape.
		PS_USERSTYLE = 0x00000007,

		PS_ALTERNATE = 0x00000008,
		PS_STYLE_MASK = 0x0000000F,

		PS_ENDCAP_ROUND = 0x00000000,
		PS_ENDCAP_SQUARE = 0x00000100,
		PS_ENDCAP_FLAT = 0x00000200,
		PS_ENDCAP_MASK = 0x00000F00,

		PS_JOIN_ROUND = 0x00000000,
		PS_JOIN_BEVEL = 0x00001000,
		PS_JOIN_MITER = 0x00002000,
		PS_JOIN_MASK = 0x0000F000,

		PS_COSMETIC = 0x00000000,
		PS_GEOMETRIC = 0x00010000,
		PS_TYPE_MASK = 0x000F0000
	};

	public enum EArcDirection : int
	{
		AD_COUNTERCLOCKWISE = 1,
		AD_CLOCKWISE = 2
	}

	public enum EStockObject
	{
		WHITE_BRUSH = 0,
		LTGRAY_BRUSH = 1,
		GRAY_BRUSH = 2,
		DKGRAY_BRUSH = 3,
		BLACK_BRUSH = 4,
		NULL_BRUSH = 5,
		HOLLOW_BRUSH = NULL_BRUSH,

		WHITE_PEN = 6,
		BLACK_PEN = 7,
		NULL_PEN = 8,

		OEM_FIXED_FONT = 10,
		ANSI_FIXED_FONT = 11,
		ANSI_VAR_FONT = 12,
		SYSTEM_FONT = 13,
		DEVICE_DEFAULT_FONT = 14,

		DEFAULT_PALETTE = 15,

		SYSTEM_FIXED_FONT = 16,
		DEFAULT_GUI_FONT = 17,

		DC_BRUSH = 18,
		DC_PEN = 19,
	}

	[Flags]
	public enum EDrawTextFormat : uint
	{
		DT_DEFAULT = 0x00000000,
		/// <summary>
		/// Left, near 
		/// </summary>
		DT_LEFT = 0x00000000,
		/// <summary>
		/// Center
		/// </summary>
		DT_CENTER = 0x00000001,
		/// <summary>
		/// Right, far
		/// </summary>
		DT_RIGHT = 0x00000002,

		/// <summary>
		/// Top, near
		/// </summary>
		DT_TOP = 0x00000000,
		/// <summary>
		/// VCenter
		/// </summary>
		DT_VCENTER = 0x00000004,
		/// <summary>
		/// Bottom, far
		/// </summary>
		DT_BOTTOM = 0x00000008,

		DT_WORDBREAK = 0x00000010,
		DT_SINGLELINE = 0x00000020,

		DT_EXPANDTABS = 0x00000040,
		DT_TABSTOP = 0x00000080,
		DT_NOCLIP = 0x00000100,
		DT_EXTERNALLEADING = 0x00000200,
		DT_CALCRECT = 0x00000400,
		DT_NOPREFIX = 0x00000800,
		DT_INTERNAL = 0x00001000,

		DT_EDITCONTROL = 0x00002000,
		DT_PATH_ELLIPSIS = 0x00004000,
		DT_END_ELLIPSIS = 0x00008000,

		DT_MODIFYSTRING = 0x00010000,
		DT_RTLREADING = 0x00020000,
		DT_WORD_ELLIPSIS = 0x00040000,
		DT_NOFULLWIDTHCHARBREAK = 0x00080000,

		DT_NULL = 0xFFFFFFFF,
	}

	public enum EBackgroundMode
	{
		NULL = 0,
		TRANSPARENT = 1,
		OPAQUE = 2
	}

	public enum ERasterOperations : uint
	{
		/// <summary> 
		/// dest = source
		/// </summary>
		SRCCOPY = 0x00CC0020,
		/// <summary> 
		/// dest = source OR dest
		/// </summary>
		SRCPAINT = 0x00EE0086,
		/// <summary> 
		/// dest = source AND dest
		/// </summary>
		SRCAND = 0x008800C6,
		/// <summary> 
		/// dest = source XOR dest
		/// </summary>
		SRCINVERT = 0x00660046,
		/// <summary> 
		/// dest = source AND (NOT dest )
		/// </summary>
		SRCERASE = 0x00440328,
		/// <summary> 
		/// dest = (NOT source)
		/// </summary>
		NOTSRCCOPY = 0x00330008,
		/// <summary> 
		/// dest = (NOT src) AND (NOT dest) 
		/// </summary>
		NOTSRCERASE = 0x001100A6,
		/// <summary>
		/// dest = (source AND pattern)
		/// </summary>
		MERGECOPY = 0x00C000CA,
		/// <summary> 
		/// dest = (NOT source) OR dest
		/// </summary>
		MERGEPAINT = 0x00BB0226,
		/// <summary>
		/// dest = pattern
		/// </summary>
		PATCOPY = 0x00F00021,
		/// <summary> 
		/// dest = DPSnoo
		/// </summary>
		PATPAINT = 0x00FB0A09,
		/// <summary> 
		/// dest = pattern XOR dest
		/// </summary>
		PATINVERT = 0x005A0049,
		/// <summary> 
		/// dest = (NOT dest)
		/// </summary>
		DSTINVERT = 0x00550009,
		/// <summary> 
		/// dest = BLACK
		/// </summary>
		BLACKNESS = 0x00000042,
		/// <summary> 
		/// dest = WHITE
		/// </summary>
		WHITENESS = 0x00FF0062,
	};

	public enum ECompression : uint
	{
		BI_RGB = 0,
		BI_RLE8 = 1,
		BI_RLE4 = 2,
		BI_BITFIELDS = 3,
		BI_JPEG = 4,
		BI_PNG = 5,
	};

	/// <summary>
	/// DIB color table identifiers
	/// </summary>
	public enum EDIBColor
	{
		/// <summary>
		/// color table in RGBs
		/// </summary>
		DIB_RGB_COLORS = 0,
		/// <summary>
		/// color table in palette indices
		/// </summary>
		DIB_PAL_COLORS = 1
	}

	/// <summary>
	/// /StretchBlt Modes
	/// </summary>
	public enum EStretchMode
	{
		/// <summary>
		/// Performs a Boolean AND operation using the color values 
		/// for the eliminated and existing pixels.
		/// If the bitmap is a monochrome bitmap, 
		/// this mode preserves black pixels at the expense of white pixels.
		/// </summary>
		BLACKONWHITE = 1,
		/// <summary>
		/// BLACKONWHITE
		/// </summary>
		ANDSCANS = 1,

		/// <summary>
		/// Performs a Boolean OR operation using the color values 
		/// for the eliminated and existing pixels. 
		/// If the bitmap is a monochrome bitmap, this mode preserves white pixels 
		/// at the expense of black pixels.
		/// </summary>
		WHITEONBLACK = 2,
		ORSCANS = 2,

		/// <summary>
		/// Deletes the pixels. 
		/// This mode deletes all eliminated lines 
		/// of pixels without trying to preserve their information.
		/// </summary>
		COLORONCOLOR = 3,
		/// <summary>
		/// Same as COLORONCOLOR.
		/// </summary>
		DELETESCANS = 3,

		/// <summary>
		/// Maps pixels from the source rectangle into blocks of pixels 
		/// in the destination rectangle. 
		/// The average color over the destination block of pixels 
		/// approximates the color of the source pixels.
		/// After setting the HALFTONE stretching mode, 
		/// an application must call the SetBrushOrgEx function 
		/// to set the brush origin. 
		/// If it fails to do so, brush misalignment occurs.
		/// </summary>
		HALFTONE = 4,
	}

	public enum EBrushStyle
	{
		BS_SOLID = 0,
		BS_NULL = 1,
		BS_HOLLOW = BS_NULL,
		BS_HATCHED = 2,
		BS_PATTERN = 3,
		BS_INDEXED = 4,
		BS_DIBPATTERN = 5,
		BS_DIBPATTERNPT = 6,
		BS_PATTERN8X8 = 7,
		BS_DIBPATTERN8X8 = 8,
		BS_MONOPATTERN = 9,
	}

	public enum EHatchStyle
	{
		HS_HORIZONTAL = 0, /* ----- */
		HS_VERTICAL = 1,   /* ||||| */
		HS_FDIAGONAL = 2,  /* \\\\\ */
		HS_BDIAGONAL = 3,  /* ///// */
		HS_CROSS = 4,      /* +++++ */
		HS_DIAGCROSS = 5,  /* xxxxx */
	}

	public enum EXformMode
	{
		MWT_IDENTITY = 1,
		MWT_LEFTMULTIPLY = 2,
		MWT_RIGHTMULTIPLY = 3,

		MWT_MIN = MWT_IDENTITY,
		MWT_MAX = MWT_RIGHTMULTIPLY
	}


	public enum EGraphicsMode
	{
		GM_COMPATIBLE = 1,
		GM_ADVANCED = 2,
	}

	public enum EMappingMode
	{
		MM_TEXT = 1,
		MM_LOMETRIC = 2,
		MM_HIMETRIC = 3,
		MM_LOENGLISH = 4,
		MM_HIENGLISH = 5,
		MM_TWIPS = 6,
		MM_ISOTROPIC = 7,
		MM_ANISOTROPIC = 8,
	}

	#endregion

	public class GDI32
	{
		[DllImport( "gdi32.dll", SetLastError = true )]
		public static extern int SetMapMode( IntPtr hdc, EMappingMode mapMode );

		#region >>> Transform <<<
		[DllImport( "gdi32.dll", SetLastError = true )]
		[return: MarshalAs( UnmanagedType.Bool )]
		public static extern bool GetWorldTransform( IntPtr hdc, out TXFORM xform );

		[DllImport( "gdi32.dll", SetLastError = true )]
		public static extern int SetGraphicsMode( IntPtr hdc, EGraphicsMode mode );

		[DllImport( "gdi32.dll", SetLastError = true )]
		[return: MarshalAs( UnmanagedType.Bool )]
		public static extern bool SetWorldTransform( IntPtr hdc, IntPtr xForm );

		[DllImport( "gdi32.dll" )]
		[return: MarshalAs( UnmanagedType.Bool )]
		public static extern bool SetWorldTransform( IntPtr hdc, ref TXFORM xForm );

		[DllImport( "gdi32.dll" )]
		[return: MarshalAs( UnmanagedType.Bool )]
		public static extern bool ModifyWorldTransform( IntPtr hdc, IntPtr xform, EXformMode mode );

		[DllImport( "gdi32.dll" )]
		[return: MarshalAs( UnmanagedType.Bool )]
		public static extern bool ModifyWorldTransform( IntPtr hdc, ref TXFORM xform, EXformMode mode );

		#endregion
		[DllImport( "gdi32.dll" )]
		public static extern int SetPixel( IntPtr hdc, int x, int y, int color );

		[DllImport( "gdi32.dll" )]
		public static extern int GetPixel( IntPtr hdc, int x, int y );

		#region >>> Bitmap <<<

		[DllImport( "gdi32.dll" )]
		public static extern IntPtr CreateBitmap( int width, int height, uint planes, uint colorBits, IntPtr dataPtr );

		/// <summary>
		/// function creates a bitmap compatible with the device 
		/// that is associated with the specified device context
		/// </summary>
		[DllImport( "gdi32.dll", SetLastError = true )]
		public static extern IntPtr CreateCompatibleBitmap( IntPtr hDC, int width, int height );

		/// <summary>
		/// function creates a compatible bitmap (DDB) from a DIB 
		/// and, optionally, sets the bitmap bits.
		/// </summary>
		[DllImport( "gdi32.dll" )]
		public static extern IntPtr CreateDIBitmap( IntPtr hdc, [In] ref TBITMAPINFOHEADER hdr,
			ECreateBitmap fdwInit, IntPtr lpbInit, [In] ref TBITMAPINFO lpbmi, EDIBColor fuColorUse );

		#endregion

		[DllImport( "gdi32.dll" )]
		public static extern ERegionError SelectClipRgn( IntPtr hdc, IntPtr hrgn );

		[DllImport( "gdi32.dll" )]
		public static extern IntPtr CreateRectRgn( int left, int top, int right, int bottom );

		[DllImport( "gdi32.dll" )]
		public static extern IntPtr CreateDC( string driver, IntPtr device, IntPtr output, IntPtr initData );

		[DllImport( "gdi32.dll" )]
		public static extern IntPtr CreateBrushIndirect( ref LOGBRUSH brush );

		[DllImport( "gdi32.dll" )]
		public static extern IntPtr CreatePenIndirect( ref LOGPEN pen );

		[DllImport( "gdi32.dll" )]
		public static extern int SetDCPenColor( IntPtr hdc, int color );

		[DllImport( "gdi32.dll" )]
		public static extern int SetDCBrushColor( IntPtr hdc, int color );

		[DllImport( "gdi32.dll" )]
		public static extern bool GdiTransparentBlt( IntPtr dstHDC, int dstX, int dstY, int dstWidth, int dstHeight,
			IntPtr srcHDC, int srcX, int srcY, int srcWidth, int srcHeight, int backColor );

		/// <summary>
		/// Translates a DIB's information into device-dependent form
		/// </summary>
		[DllImport( "gdi32.dll" )]
		public static extern int SetDIBits( IntPtr hdc, IntPtr hBitmap, int uStartScan, int cScanLines, IntPtr srcPtr, ref TBITMAPINFO bmi, EDIBColor fuColorUse );

		[DllImport( "gdi32.dll", CharSet = CharSet.Unicode )]
		public static extern bool TextOut( IntPtr hdc, int x, int y, string text, int length );

		[DllImport( "gdi32.dll" )]
		public static extern int GetDCBrushColor( IntPtr hdc );

		[DllImport( "gdi32.dll" )]
		public static extern IntPtr CreateFontIndirect( ref LOGFONT font );

		[DllImport( "gdi32.dll" )]
		public static extern int SetBkMode( IntPtr hdc, EBackgroundMode mode );

		[DllImport( "gdi32.dll" )]
		public static extern int GetBkColor( IntPtr hDC );

		[DllImport( "gdi32.dll" )]
		public static extern int SetBkColor( IntPtr hDC, int color );

		[DllImport( "gdi32.dll" )]
		public static extern int SetTextColor( IntPtr hDC, int color );

		[DllImport( "gdi32.dll", CharSet = CharSet.Auto )]
		public static extern bool GetTextExtentPoint( IntPtr hdc, string text, int length, out Size textSize );

		[DllImport( "gdi32.dll" )]
		public static extern bool Polygon( IntPtr hDC, IntPtr points, int count );

		[DllImport( "gdi32.dll" )]
		public static extern bool Pie( IntPtr hDC, int left, int top, int right, int bottom, int radialX1, int radialY1, int radialX2, int radialY2 );

		[DllImport( "gdi32.dll" )]
		public static extern IntPtr GetStockObject( EStockObject item );

		[DllImport( "gdi32.dll" )]
		public static extern bool Ellipse( IntPtr hDC, int left, int top, int right, int bottom );

		[DllImport( "gdi32.dll" )]
		public static extern bool PolyBezier( IntPtr hDC, IntPtr points, int count );

		[DllImport( "gdi32.dll" )]
		public static extern int SetArcDirection( IntPtr hDC, EArcDirection direction );

		[DllImport( "gdi32.dll" )]
		public static extern bool AngleArc( IntPtr hDC, int x, int y, uint radius, float angleStart, float angleSweep );

		[DllImport( "gdi32.dll" )]
		public static extern bool Arc( IntPtr hDC, int left, int top, int right, int bottom, int arcStartX, int arcStartY, int arcEndX, int arcEndY );

		[DllImport( "gdi32.dll" )]
		public static extern bool Polyline( IntPtr hDC, ref Point point, int count );

		[DllImport( "gdi32.dll" )]
		public static extern bool Polyline( IntPtr hDC, IntPtr points, int count );

		[DllImport( "gdi32.dll" )]
		public static extern bool MoveToEx( IntPtr hDC, int xStart, int yStart, out Point prevPoint );

		[DllImport( "gdi32.dll" )]
		public static extern bool MoveToEx( IntPtr hDC, int xStart, int yStart, IntPtr prevPoint );

		[DllImport( "gdi32.dll" )]
		public static extern bool LineTo( IntPtr hDC, int xEnd, int yEnd );

		[DllImport( "gdi32.dll" )]
		[return: MarshalAs( UnmanagedType.Bool )]
		public static extern bool Rectangle( IntPtr hDC, int left, int top, int right, int bottom );

		/// <summary>
		/// Moves a rectangle from the DIB to a rectangle on the destination surface, 
		/// stretching or compressing as necessary.
		/// </summary>
		[DllImport( "gdi32.dll" )]
		public static extern int StretchDIBits( IntPtr dstHDC, int dstX, int dstY, int dstWidth, int dstHeight,
			int srcX, int srcY, int srcWidth, int srcHeight,
			IntPtr lpBits, ref TBITMAPINFO lpBitsInfo, EDIBColor fuColorUse, ERasterOperations opts );

		[DllImport( "gdi32.dll" )]
		public static extern IntPtr CreatePen( EPenStyle style, int width, int color );

		[DllImport( "gdi32.dll" )]
		public static extern int SetStretchBltMode( IntPtr hDC, EStretchMode mode );

		[DllImport( "gdi32.dll" )]
		public static extern bool DeleteDC( IntPtr hDC );

		[DllImport( "gdi32.dll" )]
		public static extern IntPtr SelectObject( IntPtr hDC, IntPtr hObject );

		[DllImport( "gdi32.dll" )]
		public static extern IntPtr SelectObject( IntPtr hDC, ref LOGFONT hFont );

		[DllImport( "gdi32.dll" )]
		public static extern IntPtr CreateCompatibleDC( IntPtr hDC );

		[DllImport( "gdi32.dll" )]
		public static extern IntPtr CreateDIBSection( IntPtr hDC, ref TBITMAPINFO bmi,
			 EDIBColor fuColorUse, out IntPtr imagePtr, IntPtr hFileMapping, uint dwOffset );

		//Creates a device-dependent bitmap initialized with DIB information

		[DllImport( "gdi32.dll" )]
		public static extern bool BitBlt( IntPtr dstHDC, int dstX, int dstY, int width, int height,
			IntPtr srcHDC, int srcX, int srcY, ERasterOperations opts );

		[DllImport( "gdi32.dll" )]
		public static extern bool StretchBlt( IntPtr dstHDC, int dstX, int dstY,
			 int dstWidth, int dstHeight,
			 IntPtr srcHDC, int srcX, int srcY, int srcWidth, int srcHeight,
			 ERasterOperations opts );


		[DllImport( "gdi32.dll" )]
		public static extern bool DeleteObject( IntPtr hObject );

		/// <summary>
		/// Sets a DIB directly to the output surface
		/// </summary>
		[DllImport( "gdi32.dll", SetLastError = true )]
		public static extern int SetDIBitsToDevice( IntPtr dstHDC, int dstX, int dstY,
			int width, int height,
			int srcX, int srcY,
			int uStartScan, int cScanLines,
			IntPtr lpvBits, ref TBITMAPINFO lpbmi, EDIBColor fuColorUse );

		[DllImport( "gdi32.dll" )]
		static extern bool FillRgn( IntPtr hDC, IntPtr hrgn, IntPtr hbr );

		[DllImport( "gdi32.dll" )]
		public static extern IntPtr CreateSolidBrush( int color );
	}

	public class USER32
	{
		[DllImport( "user32.dll" )]
		public static extern bool GetClientRect( IntPtr hWnd, out Rectangle rect );

		[DllImport( "user32.dll" )]
		public static extern bool GetWindowRect( IntPtr hWnd, out Rectangle rect );

		[DllImport( "user32.dll" )]
		public static extern IntPtr GetWindowDC( IntPtr hWnd );

		[DllImport( "user32.dll" )]
		public static extern bool ReleaseDC( IntPtr hWnd, IntPtr hDC );

		[DllImport( "user32.dll" )]
		public static extern IntPtr BeginPaint( IntPtr hwnd, out IntPtr lpPaint );

		[DllImport( "user32.dll" )]
		public static extern bool EndPaint( IntPtr hWnd, ref IntPtr lpPaint );

		[DllImport( "user32.dll" )]
		public static extern int FillRect( IntPtr hDC, ref Rectangle rect, IntPtr brush );

		[DllImport( "user32.dll" )]
		public static extern bool ClientToScreen( IntPtr hWnd, ref Point point );

		[DllImport( "user32.dll" )]
		public static extern bool OffsetRect( ref Rectangle rect, int dx, int dy );

		[DllImport( "user32.dll", CharSet = CharSet.Unicode )]
		public static extern int DrawText( IntPtr hDC, [MarshalAs( UnmanagedType.LPWStr )]string text,
			int length, ref Rectangle rect, EDrawTextFormat format );
	}

	[StructLayout( LayoutKind.Sequential )]
	public struct TXFORM
	{
		public float M11;
		public float M12;
		public float M21;
		public float M22;
		public float Dx;
		public float Dy;

		public TXFORM( System.Drawing.Drawing2D.Matrix matrix )
		{
			float[] ele = matrix.Elements;
			M11 = ele[0];
			M12 = ele[1];
			M21 = ele[2];
			M22 = ele[3];
			Dx = ele[4];
			Dy = ele[5];
		}
	};

	[StructLayout( LayoutKind.Sequential )]
	public struct TRGBQUAD
	{
		public static readonly int Size = Marshal.SizeOf( typeof( TRGBQUAD ) );

		public byte Blue;
		public byte Green;
		public byte Red;
		public byte Reserved;
	};

	[StructLayout( LayoutKind.Sequential )]
	public struct TBITMAPINFOHEADER
	{
		public static readonly uint Size = ( uint ) Marshal.SizeOf( typeof( TBITMAPINFOHEADER ) );

		public uint biSize;
		public int Width;
		public int Height;
		public ushort biPlanes;
		public ushort biBitCount;
		public uint biCompression;
		public uint biSizeImage;
		public int biXPelsPerMeter;
		public int biYPelsPerMeter;
		public uint biClrUsed;
		public uint biClrImportant;

		public TBITMAPINFOHEADER( ushort plane, ushort bitCount, ECompression compression )
		{
			this.biPlanes = plane;
			this.biBitCount = bitCount;
			this.biCompression = ( uint ) compression;
			this.Width = 0;
			this.Height = 0;
			this.biSizeImage = 0;
			this.biXPelsPerMeter = 0;
			this.biYPelsPerMeter = 0;
			this.biClrUsed = 0;
			this.biClrImportant = 0;
			this.biSize = 0;

			this.biSize = TBITMAPINFOHEADER.Size;
		}
	};

	[StructLayout( LayoutKind.Sequential )]
	public struct TBITMAPINFO
	{
		public TBITMAPINFOHEADER Header;

		[MarshalAs( UnmanagedType.ByValArray, SizeConst = 256 )]
		public TRGBQUAD[] Colors;

		public TBITMAPINFO( ushort plane, ushort bitCount, ECompression compression )
		{
			Colors = new TRGBQUAD[256];
			Header = new TBITMAPINFOHEADER( plane, bitCount, compression );
		}

		public bool EqualSize( int width, int height )
		{
			return ( Header.Width == width && Header.Height == -height );
		}

		public int Width
		{
			get
			{
				return Header.Width;
			}
		}

		public int Height
		{
			get
			{
				return -Header.Height;
			}
		}
	};

	// if we specify CharSet.Auto instead of CharSet.Ansi, then the string will be unreadable
	[StructLayout( LayoutKind.Sequential, CharSet = CharSet.Auto )]
	public struct LOGFONT
	{
		public static readonly int Size = Marshal.SizeOf( typeof( LOGFONT ) );

		public int Height;
		public int Width;

		public int Escapement;
		public int Orientation;

		public EFontWeight Weight;
		public byte Italic;
		public byte Underline;
		public byte StrikeOut;

		public byte Charset;

		public EFontPrecision OutPrecision;
		public EFontClipPrecision ClipPrecision;
		public EFontQuality Quality;
		public EFontPitchAndFamily PitchAndFamily;

		[MarshalAs( UnmanagedType.ByValTStr, SizeConst = 32 )]
		public string FaceName;

		internal LOGFONT( bool dummy )
		{
			Height = 0;
			Width = 0;
			Escapement = 0;
			Orientation = 0;
			Italic = 0;
			Underline = 0;
			StrikeOut = 0;
			Weight = EFontWeight.FW_DONTCARE;
			Charset = ( byte ) EFontCharSet.ANSI_CHARSET;
			OutPrecision = EFontPrecision.OUT_DEFAULT_PRECIS;
			ClipPrecision = EFontClipPrecision.CLIP_DEFAULT_PRECIS;
			Quality = EFontQuality.DEFAULT_QUALITY;
			PitchAndFamily = EFontPitchAndFamily.DEFAULT_PITCH;
			FaceName = string.Empty;
		}
	}

	[StructLayout( LayoutKind.Sequential )]
	public struct LOGBRUSH
	{
		EBrushStyle Style;
		int Color;
		IntPtr Hatch;
	}

	[StructLayout( LayoutKind.Sequential )]
	public struct LOGPEN
	{
		public EPenStyle Style;
		public Point PenWidth;
		public int PenColor;

		public LOGPEN( EPenStyle style, int width, Color color )
		{
			this.Style = style;
			this.PenWidth = new Point( width, 0 );
			this.PenColor = ColorTranslator.ToWin32( color );
		}

		public LOGPEN( bool dummy )
		{
			this.Style = EPenStyle.PS_SOLID | EPenStyle.PS_GEOMETRIC | EPenStyle.PS_ENDCAP_ROUND;
			this.PenWidth = new Point( 1, 0 );
			this.PenColor = ColorTranslator.ToWin32( Color.White );
		}
	}
}
