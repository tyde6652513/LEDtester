using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MPI.Drawing
{
	public class GDI32
	{
		[DllImport( "gdi32.dll", SetLastError = true )]
		public static extern int SetMapMode( IntPtr hdc, EMappingMode mapMode );

		#region >>> Transform <<<
		[DllImport( "gdi32.dll", SetLastError = true )]
		[return: MarshalAs( UnmanagedType.Bool )]
		public static extern bool GetWorldTransform( IntPtr hdc, out MMatrix matrix );

		[DllImport( "gdi32.dll", SetLastError = true )]
		public static extern EGraphicsMode SetGraphicsMode( IntPtr hdc, EGraphicsMode mode );

		[DllImport( "gdi32.dll", SetLastError = true )]
		[return: MarshalAs( UnmanagedType.Bool )]
		public static extern bool SetWorldTransform( IntPtr hdc, IntPtr xForm );

		[DllImport( "gdi32.dll", SetLastError = true )]
		[return: MarshalAs( UnmanagedType.Bool )]
		public static extern bool SetWorldTransform( IntPtr hdc, ref MMatrix matrix );

		[DllImport( "gdi32.dll", SetLastError = true )]
		[return: MarshalAs( UnmanagedType.Bool )]
		public static extern bool ModifyWorldTransform( IntPtr hdc, IntPtr xform, EXformMode mode );

		[DllImport( "gdi32.dll", SetLastError = true )]
		[return: MarshalAs( UnmanagedType.Bool )]
		public static extern bool ModifyWorldTransform( IntPtr hdc, ref MMatrix xform, EXformMode mode );

		#endregion

		[DllImport( "gdi32.dll" )]
		public static extern uint SetPixel( IntPtr hdc, int x, int y, uint color );

		[DllImport( "gdi32.dll" )]
		public static extern uint GetPixel( IntPtr hdc, int x, int y );

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

		#region >>> Output Clip Region <<<

		[DllImport( "gdi32.dll" )]
		public static extern ERegionError SelectClipRgn( IntPtr hdc, IntPtr hrgn );

		[DllImport( "gdi32.dll" )]
		public static extern int GetClipRgn( IntPtr hdc, IntPtr hrgn );

		[DllImport( "gdi32.dll" )]
		public static extern IntPtr CreateRectRgn( int left, int top, int right, int bottom );

		[DllImport( "gdi32.dll" )]
		public static extern int GetRgnBox( IntPtr hrgn, out Rectangle rect );

		[DllImport( "gdi32.dll" )]
		public static extern IntPtr CreateRectRgnIndirect( ref Rectangle rect );

		#endregion

		#region >>> Drawing Attribute <<<

		[DllImport( "gdi32.dll" )]
		public static extern IntPtr CreatePen( EPenStyle style, int width, uint color );

		[DllImport( "gdi32.dll" )]
		public static extern IntPtr CreatePenIndirect( ref LOGPEN pen );

		[DllImport( "gdi32.dll" )]
		public static extern IntPtr CreateFontIndirect( ref LOGFONT font );

		[DllImport( "gdi32.dll" )]
		public static extern uint SetDCPenColor( IntPtr hdc, uint color );

		[DllImport( "gdi32.dll" )]
		public static extern uint GetDCPenColor( IntPtr hdc );

		[DllImport( "gdi32.dll" )]
		public static extern IntPtr CreateBrushIndirect( ref LOGBRUSH brush );

		[DllImport( "gdi32.dll" )]
		public static extern uint SetDCBrushColor( IntPtr hdc, uint color );

		[DllImport( "gdi32.dll" )]
		public static extern uint GetDCBrushColor( IntPtr hdc );


		[DllImport( "gdi32.dll" )]
		public static extern EBackgroundMode SetBkMode( IntPtr hdc, EBackgroundMode mode );

		[DllImport( "gdi32.dll" )]
		public static extern uint GetBkColor( IntPtr hDC );

		[DllImport( "gdi32.dll" )]
		public static extern uint SetBkColor( IntPtr hDC, uint color );

		#endregion

		#region >>> BLT <<<
		[DllImport( "gdi32.dll" )]
		public static extern int SetStretchBltMode( IntPtr hDC, EStretchMode mode );

		[DllImport( "gdi32.dll" )]
		public static extern EStretchMode GetStretchBltMode( IntPtr hDC );

		[DllImport( "gdi32.dll" )]
		public static extern bool GdiTransparentBlt( IntPtr dstHDC, int dstX, int dstY, int dstWidth, int dstHeight,
			IntPtr srcHDC, int srcX, int srcY, int srcWidth, int srcHeight, uint backColor );

		/// <summary>
		/// Moves a rectangle from the DIB to a rectangle on the destination surface, 
		/// stretching or compressing as necessary.
		/// </summary>
		[DllImport( "gdi32.dll" )]
		public static extern int StretchDIBits( IntPtr dstHDC, int dstX, int dstY, int dstWidth, int dstHeight,
			int srcX, int srcY, int srcWidth, int srcHeight,
			IntPtr lpBits, ref TBITMAPINFO lpBitsInfo, EDIBColor fuColorUse, ERasterOperations opts );


		[DllImport( "gdi32.dll" )]
		public static extern bool BitBlt( IntPtr dstHDC, int dstX, int dstY, int width, int height,
			IntPtr srcHDC, int srcX, int srcY, ERasterOperations opts );

		[DllImport( "gdi32.dll" )]
		public static extern bool StretchBlt( IntPtr dstHDC, int dstX, int dstY,
			 int dstWidth, int dstHeight,
			 IntPtr srcHDC, int srcX, int srcY, int srcWidth, int srcHeight,
			 ERasterOperations opts );

		[DllImport( "gdi32.dll" )]
		public static extern bool PlgBlt( IntPtr hdcDest, IntPtr lpPoint, IntPtr hdcSrc,
			int nXSrc, int nYSrc, int nWidth, int nHeight, IntPtr hbmMask, int xMask,
			int yMask );

		#endregion

		/// <summary>
		/// Translates a DIB's information into device-dependent form
		/// </summary>
		[DllImport( "gdi32.dll" )]
		public static extern int SetDIBits( IntPtr hdc, IntPtr hBitmap, uint uStartScan, uint cScanLines,
			[In] IntPtr srcPtr, ref TBITMAPINFO bmi, EDIBColor fuColorUse );

		[DllImport( "gdi32.dll" )]
		public static extern int GetDIBits( IntPtr hdc, IntPtr hBitmap, uint uStartScan, uint cScanLines,
			[Out] IntPtr outPtr, ref TBITMAPINFO bmi, EDIBColor fuColorUse );

		#region >>> Text <<<
		[DllImport( "gdi32.dll", CharSet = CharSet.Unicode )]
		public static extern bool TextOut( IntPtr hdc, int x, int y, string text, int length );

		[DllImport( "gdi32.dll" )]
		public static extern uint SetTextColor( IntPtr hDC, uint color );

		[DllImport( "gdi32.dll", CharSet = CharSet.Auto )]
		public static extern bool GetTextExtentPoint( IntPtr hdc, string text, int length, out Size textSize );

		#endregion

		#region >>> Drawing <<<
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
		public static extern EArcDirection SetArcDirection( IntPtr hDC, EArcDirection direction );

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

		#endregion

		[DllImport( "gdi32.dll" )]
		public static extern IntPtr CreateDC( string driver, IntPtr device, IntPtr output, IntPtr initData );

		[DllImport( "gdi32.dll" )]
		public static extern bool DeleteDC( IntPtr hDC );

		[DllImport( "gdi32.dll" )]
		public static extern IntPtr SelectObject( IntPtr hDC, IntPtr hObject );

		[DllImport( "gdi32.dll" )]
		public static extern int GetObject( IntPtr hDC, int buffSize, out IntPtr hObject );

		[DllImport( "gdi32.dll" )]
		public static extern IntPtr SelectObject( IntPtr hDC, ref LOGFONT hFont );

		[DllImport( "gdi32.dll" )]
		public static extern IntPtr CreateCompatibleDC( IntPtr hDC );

		[DllImport( "gdi32.dll", SetLastError = true )]
		public static extern IntPtr CreateDIBSection( IntPtr hDC, ref TBITMAPINFO bmi,
			 EDIBColor fuColorUse, out IntPtr imagePtr, IntPtr hFileMapping, uint dwOffset );

		//Creates a device-dependent bitmap initialized with DIB information

		[DllImport( "gdi32.dll" )]
		public static extern bool DeleteObject( IntPtr hObject );

		/// <summary>
		/// Sets a DIB directly to the output surface
		/// </summary>
		[DllImport( "gdi32.dll", SetLastError = true )]
		public static extern int SetDIBitsToDevice( IntPtr dstHDC, int dstX, int dstY, int width, int height,
			int srcX, int srcY, int uStartScan, int cScanLines,
			IntPtr lpvBits, ref TBITMAPINFO lpbmi, EDIBColor fuColorUse );

		[DllImport( "gdi32.dll" )]
		static extern bool FillRgn( IntPtr hDC, IntPtr hrgn, IntPtr hbr );

		[DllImport( "gdi32.dll" )]
		public static extern IntPtr CreateSolidBrush( int color );

		[DllImport( "gdi32.dll" )]
		public static extern int GetDeviceCaps( IntPtr hdc, EDeviceCap index );
	}

	public class USER32
	{
		[DllImport( "user32.dll" )]
		public static extern IntPtr GetDesktopWindow();

		[DllImport( "user32.dll" )]
		public static extern bool GetClientRect( IntPtr hWnd, out Rectangle rect );

		[DllImport( "user32.dll" )]
		public static extern bool GetWindowRect( IntPtr hWnd, out Rectangle rect );

		/// <summary>
		/// Retrieves the device context (DC) for the entire window, 
		/// including title bar, menus, and scroll bars.
		/// </summary>
		[DllImport( "user32.dll" )]
		public static extern IntPtr GetWindowDC( IntPtr hWnd );

		/// <summary>
		/// Retrieves a handle to a display device context (DC) for the client area 
		/// of a specified window or for the entire screen.
		/// </summary>
		[DllImport( "user32.dll" )]
		public static extern IntPtr GetDC( IntPtr hWnd );

		[DllImport( "user32.dll" )]
		public static extern bool ReleaseDC( IntPtr hWnd, IntPtr hDC );

		[DllImport( "user32.dll" )]
		public static extern IntPtr BeginPaint( IntPtr hWnd, out PAINTSTRUCT lpPaint );

		[DllImport( "user32.dll" )]
		public static extern bool EndPaint( IntPtr hWnd, ref PAINTSTRUCT lpPaint );

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

		public int DataSize
		{
			get
			{
				return Header.Width * ( -Header.Height ) * Header.biBitCount >> 3;
			}
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

		public LOGFONT( bool dummy )
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
		/// <summary>
		/// DWORD COLORREF
		/// </summary>
		uint Color;
		/// <summary>
		/// ULONG_PTR lbHatch
		/// </summary>
		uint Hatch;
	}

	[StructLayout( LayoutKind.Sequential )]
	public struct LOGPEN
	{
		public EPenStyle Style;
		public Point PenWidth;
		public uint PenColor;

		public LOGPEN( EPenStyle style, int width, Color color )
		{
			this.Style = style;
			this.PenWidth = new Point( width, 0 );
			this.PenColor = ( uint ) ColorTranslator.ToWin32( color );
		}

		public LOGPEN( bool dummy )
		{
			this.Style = EPenStyle.PS_SOLID | EPenStyle.PS_GEOMETRIC | EPenStyle.PS_ENDCAP_ROUND;
			this.PenWidth = new Point( 1, 0 );
			this.PenColor = ( uint ) ColorTranslator.ToWin32( Color.White );
		}
	}

	[StructLayout( LayoutKind.Sequential, Pack = 1 )]
	public struct PAINTSTRUCT
	{
		IntPtr hdc;
		bool fErase;
		Rectangle rcPaint;
		bool fRestore;
		bool fIncUpdate;

		[MarshalAs( UnmanagedType.ByValArray, SizeConst = 32 )]
		byte[] rgbReserved;

		public void Initialize()
		{
			rgbReserved = new byte[32];
		}
	}

}
