using System;

namespace MPI.Drawing
{
	public enum ERegionError
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

	public enum EFontWeight
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
		PS_INSIDEFRAME = 0x00000006,// Normally when the edge is drawn, it¡¦s centred on the outer edge meaning that half the width of the pen is drawn
		// outside the shape¡¦s edge, half is inside the shape¡¦s edge. When PS_INSIDEFRAME is specified the edge is drawn
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

	public enum EArcDirection
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

		CAPTUREBLT = 0x40000000
	};

	public enum ECompression
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
	/// StretchBlt Modes
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

	public enum EDeviceCap
	{
		/// <summary>
		/// Device driver version
		/// </summary>
		DRIVERVERSION = 0,

		/// <summary>
		/// Device classification
		/// </summary>
		TECHNOLOGY = 2,

		/// <summary>
		/// Horizontal size in millimeters
		/// </summary>
		HORZSIZE = 4,

		/// <summary>
		/// Vertical size in millimeters
		/// </summary>
		VERTSIZE = 6,

		/// <summary>
		/// Horizontal width in pixels
		/// </summary>
		HORZRES = 8,

		/// <summary>
		/// Vertical height in pixels
		/// </summary>
		VERTRES = 10,

		/// <summary>
		/// Number of bits per pixel
		/// </summary>
		BITSPIXEL = 12,

		/// <summary>
		/// Number of planes
		/// </summary>
		PLANES = 14,

		/// <summary>
		/// Number of brushes the device has
		/// </summary>
		NUMBRUSHES = 16,

		/// <summary>
		/// Number of pens the device has
		/// </summary>
		NUMPENS = 18,

		/// <summary>
		/// Number of markers the device has
		/// </summary>
		NUMMARKERS = 20,

		/// <summary>
		/// Number of fonts the device has
		/// </summary>
		NUMFONTS = 22,

		/// <summary>
		/// Number of colors the device supports
		/// </summary>
		NUMCOLORS = 24,

		/// <summary>
		/// Size required for device descriptor
		/// </summary>
		PDEVICESIZE = 26,

		/// <summary>
		/// Curve capabilities
		/// </summary>
		CURVECAPS = 28,

		/// <summary>
		/// Line capabilities
		/// </summary>
		LINECAPS = 30,

		/// <summary>
		/// Polygonal capabilities
		/// </summary>
		POLYGONALCAPS = 32,

		/// <summary>
		/// Text capabilities
		/// </summary>
		TEXTCAPS = 34,

		/// <summary>
		/// Clipping capabilities
		/// </summary>
		CLIPCAPS = 36,

		/// <summary>
		/// Bitblt capabilities
		/// </summary>
		RASTERCAPS = 38,

		/// <summary>
		/// Length of the X leg
		/// </summary>
		ASPECTX = 40,

		/// <summary>
		/// Length of the Y leg
		/// </summary>
		ASPECTY = 42,

		/// <summary>
		/// Length of the hypotenuse
		/// </summary>
		ASPECTXY = 44,

		/// <summary>
		/// Logical pixels inch in X
		/// </summary>
		LOGPIXELSX = 88,

		/// <summary>
		/// Logical pixels inch in Y
		/// </summary>
		LOGPIXELSY = 90,

		/// <summary>
		/// Number of entries in physical palette
		/// </summary>
		SIZEPALETTE = 104,

		/// <summary>
		/// Number of reserved entries in palette
		/// </summary>
		NUMRESERVED = 106,

		/// <summary>
		/// Actual color resolution
		/// </summary>
		COLORRES = 108,

		/// <summary>
		/// Physical Width in device units
		/// </summary>
		PHYSICALWIDTH = 110,

		/// <summary>
		/// Physical Height in device units
		/// </summary>
		PHYSICALHEIGHT = 111,

		/// <summary>
		/// Physical Printable Area x margin
		/// </summary>
		PHYSICALOFFSETX = 112,

		/// <summary>
		/// Physical Printable Area y margin
		/// </summary>
		PHYSICALOFFSETY = 113,

		/// <summary>
		/// Scaling factor x
		/// </summary>
		SCALINGFACTORX = 114,

		/// <summary>
		/// Scaling factor y
		/// </summary>
		SCALINGFACTORY = 115,

		/// <summary>
		/// Current vertical refresh rate of the display device (for displays only) in Hz
		/// </summary>
		VREFRESH = 116,

		/// <summary>
		/// Horizontal width of entire desktop in pixels
		/// </summary>
		DESKTOPVERTRES = 117,

		/// <summary>
		/// Vertical height of entire desktop in pixels
		/// </summary>
		DESKTOPHORZRES = 118,

		/// <summary>
		/// Preferred blt alignment
		/// </summary>
		BLTALIGNMENT = 119,

		/// <summary>
		/// Shading and blending caps
		/// </summary>
		SHADEBLENDCAPS = 120,

		/// <summary>
		/// Color Management caps
		/// </summary>
		COLORMGMTCAPS = 121
	}

	/// <summary>
	/// Content alignment.
	/// </summary>
	public enum ContentAlignments : int
	{
		TopLeft = 0,
		TopCenter,
		TopRight,
		MiddleLeft,
		MiddleCenter,
		MiddleRight,
		BottomLeft,
		BottomCenter,
		BottomRight
	}
}
