using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using MPI.Win32;

namespace MPI.Drawing
{
	public class MGraphics
	{
		#region >>> HBitmap Helper <<<

		public static bool InitBitmapInfo( Image image, out TBITMAPINFO bmi )
		{
			return InitBitmapInfo( image.PixelFormat, image.Width, image.Height, out bmi );
		}

		internal static bool InitBitmapInfo( Image image, int width, int height, out TBITMAPINFO bmi )
		{
			return InitBitmapInfo( image.PixelFormat, width, height, out bmi );
		}

		public static bool InitBitmapInfo( PixelFormat format, int width, int height, out TBITMAPINFO bmi )
		{
			ushort channel;
			if ( format == PixelFormat.Format8bppIndexed )
			{
				channel = 1;
			}
			else if ( format == PixelFormat.Format24bppRgb )
			{
				channel = 3;
			}
			else
			{
				bmi = new TBITMAPINFO();
				return false;
			}

			InitBitmapInfo( channel, width, height, out bmi );
			return true;
		}

		public static void InitBitmapInfo( ushort channel, int width, int height, out TBITMAPINFO bmi )
		{
			bmi = new TBITMAPINFO( 1, ( ushort ) ( channel * 8 ), ECompression.BI_RGB );

			bmi.Header.Width = width;
			bmi.Header.Height = -height;

			//set palette for gray
			if ( channel == 1 )
			{
				for ( short i = 1; i < 256; i++ )
				{
					bmi.Colors[i].Blue = ( byte ) i;
					bmi.Colors[i].Green = ( byte ) i;
					bmi.Colors[i].Red = ( byte ) i;
				}
			}

		}

		public static IntPtr CreateHBitmap( IntPtr dstHdc, Bitmap image )
		{
			TBITMAPINFO bmi;
			IntPtr out_ptr = IntPtr.Zero;

			MGraphics.InitBitmapInfo( image, out bmi );

			IntPtr hbmp = GDI32.CreateDIBSection( dstHdc, ref bmi, EDIBColor.DIB_RGB_COLORS, out out_ptr, IntPtr.Zero, 0 );
			BitmapData bd = image.LockBits( new Rectangle( 0, 0, image.Width, image.Height ), ImageLockMode.ReadOnly, image.PixelFormat );
			MPI.Win32.Win32API.Memory.CopyMemory( out_ptr, bd.Scan0, ( uint ) bmi.DataSize );
			image.UnlockBits( bd );
			bd = null;

			return hbmp;
		}

		#endregion

		public static void Clear( IntPtr hdc, uint backColor, int width, int height )
		{
			FillRectangle( hdc, 0, 0, width, height, backColor );
		}

		public static void Clear2( IntPtr hdc, uint backColor, int width, int height )
		{
			MGraphics.FillRectangle( hdc, 0, 0, width, height, backColor );
		}

		public static void FillRectangle( IntPtr hdc, int x, int y, int width, int height, uint fillColor )
		{
			IntPtr old = MGraphics.SetBrushColor( hdc, fillColor );
			GDI32.Rectangle( hdc, ( x - 1 ), ( y - 1 ), ( x + width + 1 ), ( y + height + 1 ) );
			GDI32.SelectObject( hdc, old );
		}

		public static void DrawImageUnscaled( IntPtr dstHdc, ref Rectangle dstRect, Bitmap srcImage )
		{
			TBITMAPINFO src_bmi;
			if ( MGraphics.InitBitmapInfo( srcImage.PixelFormat, srcImage.Width, srcImage.Height, out src_bmi ) == false )
				throw new ArgumentException( "Unsupported Format", "image" );

			BitmapData bd = srcImage.LockBits( new Rectangle( Point.Empty, srcImage.Size ), ImageLockMode.ReadOnly, srcImage.PixelFormat );

			int scan_line = Math.Min( dstRect.Height, srcImage.Height );

			scan_line = GDI32.SetDIBitsToDevice( dstHdc, dstRect.X, dstRect.Y, dstRect.Width, dstRect.Height,
				0, 0, 0, scan_line, bd.Scan0, ref src_bmi, EDIBColor.DIB_RGB_COLORS );

			srcImage.UnlockBits( bd );

			bd = null;
		}

		/// <summary>
		/// Draw Image Unscaled
		/// </summary>
		/// <param name="dstHdc"></param>
		/// <param name="srcImage"></param>
		/// <returns></returns>
		public static int DrawImageUnscaled( IntPtr dstHdc, Bitmap srcImage )
		{
			return DrawImageAutoScaled( dstHdc, srcImage.Width, srcImage.Height, srcImage );
		}

		/// <summary>
		/// Draw DC, auto scale, and clip
		/// </summary>
		public static bool DrawAutoScaled( IntPtr dstDC, int dstWidth, int dstHeight,
			IntPtr srcDC, int srcX, int srcY, int srcWidth, int srcHeight )
		{
			if ( dstWidth == srcWidth && dstHeight == srcHeight )
			{
				return GDI32.BitBlt( dstDC, 0, 0, dstWidth, dstHeight,
					srcDC, srcX, srcY, ERasterOperations.SRCCOPY );
			}
			else
			{
				GDI32.SetStretchBltMode( dstDC, EStretchMode.DELETESCANS );

				return GDI32.StretchBlt( dstDC, 0, 0, dstWidth, dstHeight,
					srcDC, srcX, srcY, srcWidth, srcHeight, ERasterOperations.SRCCOPY );
			}
		}

		/// <summary>
		/// Draw DC, auto scale
		/// </summary>
		public static bool DrawAutoScaled( IntPtr dstDC, int dstWidth, int dstHeight,
			IntPtr srcDC, int srcWidth, int srcHeight )
		{
			if ( dstWidth == srcWidth && dstHeight == srcHeight )
			{
				return GDI32.BitBlt( dstDC, 0, 0, dstWidth, dstHeight, srcDC, 0, 0, ERasterOperations.SRCCOPY );
			}
			else
			{
				GDI32.SetStretchBltMode( dstDC, EStretchMode.DELETESCANS );
				return GDI32.StretchBlt( dstDC, 0, 0, dstWidth, dstHeight,
					srcDC, 0, 0, srcWidth, srcHeight, ERasterOperations.SRCCOPY );
			}
		}

		public static bool DrawUnscaled( IntPtr dstDC, int width, int height, IntPtr srcDC )
		{
			return GDI32.BitBlt( dstDC, 0, 0, width, height, srcDC, 0, 0, ERasterOperations.SRCCOPY );
		}

		public static bool DrawScaled( IntPtr dstDC, int dstWidth, int dstHeight, IntPtr srcDC, int srcWidth, int srcHeight )
		{
			GDI32.SetStretchBltMode( dstDC, EStretchMode.DELETESCANS );
			return GDI32.StretchBlt( dstDC, 0, 0, dstWidth, dstHeight,
				srcDC, 0, 0, srcWidth, srcHeight, ERasterOperations.SRCCOPY );
		}

		/// <summary>
		/// Draw Layer Unscaled
		/// </summary>
		public static void DrawLayer( IntPtr dstDC, int width, int height, IntPtr themeDC, uint backColor )
		{
			// ColorTranslator.ToWin32( Color.Black ): 0
			GDI32.GdiTransparentBlt( dstDC, 0, 0, width, height,
				themeDC, 0, 0, width, height, backColor );
		}

		public static void SetLayerDataAutoScaled( IntPtr dstHdc, int dstWidth, int dstHeight, IntPtr srcImage, ref TBITMAPINFO srcBmi, uint backColor )
		{
			IntPtr overlay_dc = GDI32.CreateCompatibleDC( dstHdc );
			IntPtr overlay_bmp;
			IntPtr overlay_temp;

			// Copy image data to DC
			if ( srcBmi.EqualSize( dstWidth, dstHeight ) )
			{
				#region >>> Method 1 fastest <<<

				IntPtr out_ptr = IntPtr.Zero;
				overlay_bmp = GDI32.CreateDIBSection( dstHdc, ref srcBmi, EDIBColor.DIB_RGB_COLORS, out out_ptr, IntPtr.Zero, 0 );
				//scan = GDI32.SetDIBits( overlay_dc, overlay_bmp, 0, dstHeight, srcImage, ref srcBmi, EDIBColor.DIB_RGB_COLORS );
				MPI.Win32.Win32API.Memory.CopyMemory( out_ptr, srcImage, ( uint ) ( dstWidth * dstHeight * 3 ) );

				overlay_temp = GDI32.SelectObject( overlay_dc, overlay_bmp );

				#endregion

				#region >>> Method 2 <<<

				//overlay_bmp = GDI32.CreateCompatibleBitmap( dstDC, dstWidth, dstHeight );
				//GDI32.SelectObject( overlay_dc, overlay_bmp );

				//scan = GDI32.SetDIBitsToDevice( overlay_dc, 0, 0, dstWidth, dstHeight,
				//      0, 0, 0, srcBmi.Height, srcImage, ref srcBmi, EDIBColor.DIB_RGB_COLORS );

				#endregion

				#region >>> Method 3 <<<
				//overlay_bmp = GDI32.CreateDIBitmap( dstDC, ref srcBmi.Header, ECreateBitmap.CBM_INIT,
				//            srcImage, ref srcBmi, EDIBColor.DIB_RGB_COLORS );

				//GDI32.SelectObject( overlay_dc, overlay_bmp );

				#endregion

				#region >>> Method 4 <<<

				//overlay_bmp = GDI32.CreateDIBitmap( dstDC, ref srcBmi.Header, ECreateBitmap.CBM_NULL,
				//            IntPtr.Zero, ref srcBmi, EDIBColor.DIB_RGB_COLORS );

				//GDI32.SelectObject( overlay_dc, overlay_bmp );

				////scan = GDI32.SetDIBitsToDevice( overlay_dc, 0, 0, dstWidth, dstHeight,
				////      0, 0, 0, srcBmi.Height, srcImage, ref srcBmi, EDIBColor.DIB_RGB_COLORS );

				//scan = GDI32.SetDIBits( overlay_dc, overlay_bmp, 0, dstHeight, srcImage, ref srcBmi, EDIBColor.DIB_RGB_COLORS );

				#endregion

				#region >>> Method 5 <<<

				//overlay_bmp = GDI32.CreateCompatibleBitmap( dstDC, dstWidth, dstHeight );

				//overlay_bmp = GDI32.CreateDIBitmap( dstDC, ref srcBmi.Header, ECreateBitmap.CBM_NULL,
				//            IntPtr.Zero, ref srcBmi, EDIBColor.DIB_RGB_COLORS );

				//GDI32.SelectObject( overlay_dc, overlay_bmp );

				//GDI32.SetDIBits( overlay_dc, overlay_bmp, 0, dstHeight, srcImage, ref srcBmi, EDIBColor.DIB_RGB_COLORS );

				#endregion
			}
			else
			{
				//overlay_bmp = GDI32.CreateCompatibleBitmap( dstDC, dstWidth, dstHeight );
				IntPtr out_ptr = IntPtr.Zero;
				overlay_bmp = GDI32.CreateDIBSection( dstHdc, ref srcBmi, EDIBColor.DIB_RGB_COLORS, out out_ptr, IntPtr.Zero, 0 );

				overlay_temp = GDI32.SelectObject( overlay_dc, overlay_bmp );

				GDI32.SetStretchBltMode( overlay_dc, EStretchMode.DELETESCANS );
				GDI32.StretchBlt( overlay_dc, 0, 0, dstWidth, dstHeight,
					overlay_dc, 0, 0, srcBmi.Width, srcBmi.Height, ERasterOperations.SRCCOPY );
			}

			// draw overlay on dstDC
			GDI32.GdiTransparentBlt( dstHdc, 0, 0, dstWidth, dstHeight, overlay_dc,
				0, 0, srcBmi.Width, srcBmi.Height, backColor );

			GDI32.SelectObject( overlay_dc, overlay_temp ); // strike out 'overlay_bmp', 'overlay_bmp' is free
			GDI32.DeleteObject( overlay_bmp );
			GDI32.DeleteDC( overlay_dc );

		}

		internal static void SetLayerDataUnscaled( IntPtr dstHdc, IntPtr srcImage, ref TBITMAPINFO srcBmi, uint backColor )
		{
			SetLayerDataAutoScaled( dstHdc, srcBmi.Width, srcBmi.Height, srcImage, ref srcBmi, backColor );
		}

		public static IntPtr SetBrushColor( IntPtr hdc, uint color )
		{
			IntPtr old = GDI32.SelectObject( hdc, GDI32.GetStockObject( EStockObject.DC_BRUSH ) );
			GDI32.SetDCBrushColor( hdc, color );
			return old;
		}

		public static void SetPixel( IntPtr hdc, int x, int y, uint color )
		{
			GDI32.SetPixel( hdc, x, y, color );
		}

		public static void SetPixel( IntPtr hdc, int x, int y, Color color )
		{
			GDI32.SetPixel( hdc, x, y, ( uint ) ColorTranslator.ToWin32( color ) );
		}

		public static Color GetPixel( IntPtr hdc, int x, int y )
		{
			return ColorTranslator.FromWin32( ( int ) GDI32.GetPixel( hdc, x, y ) );
		}

		public static void DrawRectangle( IntPtr hdc, int x, int y, int width, int height )
		{
			GDI32.Rectangle( hdc, x, y, ( x + width + 1 ), ( y + height + 1 ) );
		}

		public static void DrawRectangle( IntPtr hdc, ref Rectangle rect )
		{
			GDI32.Rectangle( hdc, rect.X, rect.Y, ( rect.Right + 1 ), ( rect.Bottom + 1 ) );
		}

		public static void DrawRectangle( IntPtr hdc, ref Rectangle rect, uint drawColor )
		{
			IntPtr old = SetPenColor( hdc, drawColor );
			GDI32.Rectangle( hdc, rect.X, rect.Y, ( rect.Right + 1 ), ( rect.Bottom + 1 ) );
			GDI32.SelectObject( hdc, old );
		}

		public static void DrawRectangle( IntPtr hdc, int x, int y, int width, int height, uint drawColor )
		{
			IntPtr old = SetPenColor( hdc, drawColor );
			GDI32.Rectangle( hdc, x, y, ( x + width + 1 ), ( y + height + 1 ) );
			GDI32.SelectObject( hdc, old );
		}

		public static void FillRectangle( IntPtr hdc, int x, int y, int width, int height )
		{
			GDI32.Rectangle( hdc, ( x - 1 ), ( y - 1 ), ( x + width + 1 ), ( y + height + 1 ) );
		}

		public static void DrawCross( IntPtr hdc, int x, int y, int size )
		{
			int x1 = x - size, y1 = y - size;
			int x2 = x + size, y2 = y + size;

			DrawLine( hdc, x1, y1, x2, y2 );
			DrawLine( hdc, x1, y2, x2, y1 );
		}

		public static void DrawLine( IntPtr hdc, int x1, int y1, int x2, int y2 )
		{
			GDI32.MoveToEx( hdc, x1, y1, IntPtr.Zero );
			GDI32.LineTo( hdc, x2, y2 );
		}

		public static void DrawLine( IntPtr hdc, int x1, int y1, int x2, int y2, uint drawColor )
		{
			IntPtr old = SetPenColor( hdc, drawColor );
			DrawLine( hdc, x1, y1, x2, y2 );
			GDI32.SelectObject( hdc, old );
		}

		public static void DrawLines( IntPtr hdc, Point[] points )
		{
			GDI32.Polyline( hdc, Marshal.UnsafeAddrOfPinnedArrayElement( points, 0 ), points.Length );
		}

		public static void DrawLines( IntPtr hdc, Point[] points, uint drawColor )
		{
			IntPtr old = SetPenColor( hdc, drawColor );
			GDI32.Polyline( hdc, Marshal.UnsafeAddrOfPinnedArrayElement( points, 0 ), points.Length );
			GDI32.SelectObject( hdc, old );
		}

		public static void DrawArc( IntPtr hdc, RectangleF rect, float startAngle, float sweepAngle )
		{
			float w = rect.Width / 2;
			float h = rect.Height / 2;
			float x0 = rect.X + w;
			float y0 = rect.Y + h;

			int left2 = ( int ) ( x0 + w * ( float ) Math.Cos( sweepAngle * Math.PI / 180 ) );
			int top2 = ( int ) ( y0 + h * ( float ) Math.Sin( sweepAngle * Math.PI / 180 ) );
			int right2 = ( int ) ( x0 + w * ( float ) Math.Cos( startAngle * Math.PI / 180 ) );
			int bottom2 = ( int ) ( y0 + h * ( float ) Math.Sin( startAngle * Math.PI / 180 ) );

			GDI32.Arc( hdc, ( int ) rect.Left, ( int ) rect.Top, ( int ) rect.Right, ( int ) rect.Bottom,
				left2, top2, right2, bottom2 );
		}

		public static void DrawArc( IntPtr hdc, RectangleF rect, float startAngle, float sweepAngle, uint drawColor )
		{
			IntPtr old = SetPenColor( hdc, drawColor );
			DrawArc( hdc, rect, startAngle, sweepAngle );
			GDI32.SelectObject( hdc, old );
		}

		public static void DrawBeziers( IntPtr hdc, Point[] points )
		{
			GDI32.PolyBezier( hdc, Marshal.UnsafeAddrOfPinnedArrayElement( points, 0 ), points.Length );
		}

		public static void DrawBeziers( IntPtr hdc, Point[] points, uint drawColor )
		{
			IntPtr old = SetPenColor( hdc, drawColor );
			GDI32.PolyBezier( hdc, Marshal.UnsafeAddrOfPinnedArrayElement( points, 0 ), points.Length );
			GDI32.SelectObject( hdc, old );
		}

		public static void DrawEllipse( IntPtr hdc, int x, int y, int width, int height )
		{
			GDI32.Ellipse( hdc, x, y, ( x + width + 1 ), ( y + height + 1 ) );
		}

		public static void DrawEllipse( IntPtr hdc, int x, int y, int width, int height, uint drawColor )
		{
			IntPtr old = SetPenColor( hdc, drawColor );
			GDI32.Ellipse( hdc, x, y, ( x + width + 1 ), ( y + height + 1 ) );
			GDI32.SelectObject( hdc, old );
		}

		public static void FillEllipse( IntPtr hdc, int x, int y, int width, int height )
		{
			GDI32.Ellipse( hdc, x, y, ( x + width + 1 ), ( y + height + 1 ) );
		}

		public static void FillEllipse( IntPtr hdc, int x, int y, int width, int height, uint fillColor )
		{
			IntPtr old = MGraphics.SetBrushColor( hdc, fillColor );
			GDI32.Ellipse( hdc, x, y, ( x + width + 1 ), ( y + height + 1 ) );
			GDI32.SelectObject( hdc, old );
		}

		public static void DrawPie( IntPtr hdc, RectangleF rect, float startAngle, float sweepAngle, uint drawColor )
		{
			IntPtr old = SetPenColor( hdc, drawColor );
			float w = rect.Width / 2;
			float h = rect.Height / 2;
			float x0 = rect.X + w;
			float y0 = rect.Y + h;

			int left2 = ( int ) ( x0 + w * ( float ) Math.Cos( sweepAngle * Math.PI / 180 ) );
			int right2 = ( int ) ( x0 + w * ( float ) Math.Cos( startAngle * Math.PI / 180 ) );

			int top2 = ( int ) ( y0 + h * ( float ) Math.Sin( sweepAngle * Math.PI / 180 ) );
			int bottom2 = ( int ) ( y0 + h * ( float ) Math.Sin( startAngle * Math.PI / 180 ) );

			//$RIC, BUG
			bool ok = GDI32.Pie( hdc, ( int ) rect.Left, ( int ) rect.Top, ( int ) rect.Right, ( int ) rect.Bottom,
				left2, top2, right2, bottom2 );

			GDI32.SelectObject( hdc, old );
		}

		public static void DrawPolygon( IntPtr hdc, Point[] points )
		{
			GDI32.Polygon( hdc, Marshal.UnsafeAddrOfPinnedArrayElement( points, 0 ), points.Length );
		}

		public static void DrawPolygon( IntPtr hdc, Point[] points, uint drawColor )
		{
			IntPtr old = SetPenColor( hdc, drawColor );
			GDI32.Polygon( hdc, Marshal.UnsafeAddrOfPinnedArrayElement( points, 0 ), points.Length );
			GDI32.SelectObject( hdc, old );
		}

		#region >>> Draw String <<<

		public static void DrawString( IntPtr hdc, string s, Font font, uint color, ref Rectangle rect, EDrawTextFormat format )
		{
			IntPtr old = MGraphics.SetFont( hdc, font );

			GDI32.SetBkMode( hdc, EBackgroundMode.TRANSPARENT );

			color = GDI32.SetTextColor( hdc, color );

			if ( format != EDrawTextFormat.DT_NULL )
			{
				if ( rect.Size.IsEmpty )
				{
					Size size;
					GDI32.GetTextExtentPoint( hdc, s, s.Length, out size );
					rect.Size = size;
				}

				USER32.DrawText( hdc, s, s.Length, ref rect, format );
			}
			else
				GDI32.TextOut( hdc, rect.X, rect.Y, s, s.Length );

			GDI32.SetTextColor( hdc, color );

			MGraphics.ResetFont( hdc, old );
		}

		public static void MeasureString( IntPtr hdc, string text, out Size size )
		{
			GDI32.GetTextExtentPoint( hdc, text, text.Length, out size );
		}

		public static void MeasureString( IntPtr hdc, string text, Font font, out Size size )
		{
			IntPtr old = MGraphics.SetFont( hdc, font );
			GDI32.GetTextExtentPoint( hdc, text, text.Length, out size );
			MGraphics.ResetFont( hdc, old );
		}

		#endregion

		internal static void SetPen( ref LOGPEN logPen, Pen pen )
		{
			logPen.PenColor = ( uint ) ColorTranslator.ToWin32( pen.Color );
			logPen.PenWidth.X = ( int ) pen.Width;
			logPen.Style = ( EPenStyle ) ( ( uint ) pen.DashStyle );
		}

		public static IntPtr SetPen( IntPtr hdc, Pen pen )
		{
			LOGPEN log_pen = new LOGPEN( true );
			MGraphics.SetPen( ref log_pen, pen );
			return GDI32.SelectObject( hdc, GDI32.CreatePenIndirect( ref log_pen ) );
		}

		public static void ResetPen( IntPtr hdc, IntPtr oldPen )
		{
			GDI32.DeleteObject( GDI32.SelectObject( hdc, oldPen ) );
		}

		public static IntPtr SetPenColor( IntPtr hdc, uint color )
		{
			IntPtr old = GDI32.SelectObject( hdc, GDI32.GetStockObject( EStockObject.DC_PEN ) );
			GDI32.SelectObject( hdc, GDI32.GetStockObject( EStockObject.NULL_BRUSH ) );
			GDI32.SetDCPenColor( hdc, color );
			return old;
		}

		public static void SetFont( ref LOGFONT logFont, Font font )
		{
			//_font.Height = -MulDiv(PointSize, GetDeviceCaps(hDC, LOGPIXELSY), 72);
			logFont.Height = font.Height;
			logFont.Charset = font.GdiCharSet;
			logFont.FaceName = font.Name;
			logFont.Italic = ( byte ) ( font.Italic ? 0x01 : 0x00 );
			logFont.Underline = ( byte ) ( font.Underline ? 1 : 0 );
			logFont.Weight = ( font.Bold ? EFontWeight.FW_BOLD : EFontWeight.FW_NORMAL );
			logFont.StrikeOut = ( byte ) ( font.Strikeout ? 1 : 0 );
			logFont.PitchAndFamily = EFontPitchAndFamily.FF_DONTCARE;
			logFont.Quality = EFontQuality.DEFAULT_QUALITY;
		}

		public static IntPtr SetFont( IntPtr hdc, Font font )
		{
			LOGFONT log_font = new LOGFONT( true );
			MGraphics.SetFont( ref log_font, font );
			return GDI32.SelectObject( hdc, GDI32.CreateFontIndirect( ref log_font ) );
		}

		public static void ResetFont( IntPtr hdc, IntPtr oldFont )
		{
			GDI32.DeleteObject( GDI32.SelectObject( hdc, oldFont ) );
		}

		/// <summary>
		/// Draw Image, scaled by <paramref name="image"/>
		/// </summary>
		/// <param name="dstHdc">target HDC</param>
		/// <param name="image">source Image</param>
		/// <returns>scan line</returns>
		public static int DrawImageAutoScaled( IntPtr dstHdc, int dstWidth, int dstHeight, Bitmap image )
		{
			TBITMAPINFO src_bmi;
			MGraphics.InitBitmapInfo( image, out src_bmi );

			BitmapData bd = image.LockBits( new Rectangle( 0, 0, image.Width, image.Height ), ImageLockMode.ReadOnly, image.PixelFormat );

			int scan;
			if ( src_bmi.EqualSize( dstWidth, dstHeight ) )
				scan = SetDataUnscaled( dstHdc, ref src_bmi, bd.Scan0 );
			else
				scan = SetDataScaled( dstHdc, dstWidth, dstHeight, ref src_bmi, bd.Scan0 );

			image.UnlockBits( bd );

			return scan;
		}

		/// <summary>
		/// Set content with specified <typeparamref name="TBITMAPINFO"> and image data.
		/// </summary>
		/// <param name="hdc"></param>
		/// <param name="srcBmi">source <typeparamref name="TBITMAPINFO"></param>
		/// <param name="srcPtr">image data</param>
		/// <returns>scan line</returns>
		internal static int SetDataUnscaled( IntPtr dstHdc, ref TBITMAPINFO srcBmi, IntPtr srcPtr )
		{
			return GDI32.SetDIBitsToDevice( dstHdc, 0, 0, srcBmi.Header.Width, srcBmi.Header.Height, 0, 0, 0, srcBmi.Header.Height, srcPtr, ref srcBmi, EDIBColor.DIB_RGB_COLORS );
		}

		internal static int SetDataScaled( IntPtr dstHdc, int dstWidth, int dstHeight, ref TBITMAPINFO srcBmi, IntPtr srcPtr )
		{
			GDI32.SetStretchBltMode( dstHdc, EStretchMode.DELETESCANS );

			return GDI32.StretchDIBits( dstHdc, 0, 0, dstWidth, dstHeight,
				0, 0, srcBmi.Header.Width, ( -srcBmi.Header.Height ), srcPtr, ref srcBmi, EDIBColor.DIB_RGB_COLORS, ERasterOperations.SRCCOPY );
		}

	}

	public struct TCanvasInfo
	{
		public static readonly TCanvasInfo Empty = new TCanvasInfo();

		public IntPtr Hdc;
		public short Width;
		public short Height;
		/// <summary>
		/// Color Channel: 1-GrayScale, 3:RGB, 2: Unknown
		/// </summary>
		public byte Colors;

		private IntPtr SelectedBmp;
		private IntPtr OldBmp;
		public IntPtr DataBits;

		internal uint DataSize
		{
			get
			{
				return ( uint ) ( this.Width * this.Height * this.Colors );
			}
		}

		internal void GetBitmapInfo( out TBITMAPINFO bmi )
		{
			MGraphics.InitBitmapInfo( this.Colors, this.Width, this.Height, out bmi );
		}

		internal bool RetriveDataBits( ref IntPtr dataBits )
		{
			TBITMAPINFO bmi;
			MGraphics.InitBitmapInfo( this.Colors, this.Width, this.Height, out bmi );

			int scan = GDI32.GetDIBits( this.Hdc, this.SelectedBmp, 0, ( uint ) this.Height,
				dataBits, ref bmi, EDIBColor.DIB_RGB_COLORS );

			if ( scan == 0 )
			{
				Console.Error.WriteLine( "[TCanvasInfo::GetDIBits] {0}", Marshal.GetLastWin32Error() );
				return false;
			}

			return true;
		}

		internal Bitmap MakeBitmap( ref IntPtr dataBits )
		{
			PixelFormat fmt = ( this.Colors == 3 ) ? PixelFormat.Format24bppRgb : PixelFormat.Format8bppIndexed;

			Bitmap image = new Bitmap( this.Width, this.Height, fmt );
			BitmapData bd = image.LockBits( new Rectangle( 0, 0, this.Width, this.Height ), ImageLockMode.WriteOnly, fmt );

			Win32API.Memory.CopyMemory( bd.Scan0, dataBits, this.DataSize );

			if ( this.Colors == 1 )
			{
				ColorPalette cp = image.Palette;
				for ( int i = 0; i < 256; i++ )
					cp.Entries[i] = Color.FromArgb( 255, i, i, i );

				image.Palette = cp;
			}

			image.UnlockBits( bd );
			bd = null;

			return image;
		}

		public void SetDataBits( IntPtr dataBits, uint size )
		{
			uint data_size = this.DataSize;
			if ( size > data_size )
				size = data_size;

			Win32API.Memory.CopyMemory( this.DataBits, dataBits, size );
		}

		public void SetImage( Bitmap image )
		{
			BitmapData bd = image.LockBits( new Rectangle( Point.Empty, image.Size ), ImageLockMode.ReadOnly, image.PixelFormat );
			Win32API.Memory.CopyMemory( this.DataBits, bd.Scan0, this.DataSize );
			image.UnlockBits( bd );
			bd = null;
		}

		#region >>> Public Static Method <<<

		#region >>> Create && Destory <<<
		public static bool CreateCanvas( IntPtr parentDC, IntPtr sharedBase, uint offset, ref TCanvasInfo canvas )
		{
			canvas.DataBits = IntPtr.Zero;
			canvas.Hdc = GDI32.CreateCompatibleDC( parentDC );

			TBITMAPINFO bmi;
			MGraphics.InitBitmapInfo( canvas.Colors, canvas.Width, canvas.Height, out bmi );
			canvas.SelectedBmp = GDI32.CreateDIBSection( canvas.Hdc, ref bmi, EDIBColor.DIB_RGB_COLORS,
				out canvas.DataBits, sharedBase, offset );

			//$RIC, Bitmap will keep unchanged since it has been created
			//IntPtr ptr = new IntPtr( sharedBase.ToInt32() + offset );
			//canvas.SelectedBmp = GDI32.CreateDIBitmap( parentDC, ref bmi.Header, ECreateBitmap.CBM_INIT, ptr, ref bmi, EDIBColor.DIB_RGB_COLORS );

			if ( canvas.SelectedBmp == IntPtr.Zero )
			{
				Console.Error.WriteLine( "[TCanvasInfo::CreateDIBSection] {0}", Marshal.GetLastWin32Error() );
				GDI32.DeleteDC( canvas.Hdc );
				return false;
			}

			canvas.OldBmp = GDI32.SelectObject( canvas.Hdc, canvas.SelectedBmp );
			return true;
		}

		public static bool CreateCanvas( IntPtr parentDC, ref TCanvasInfo canvas )
		{
			return TCanvasInfo.CreateCanvas( parentDC, IntPtr.Zero, 0, ref canvas );
		}

		public static bool CreateLayer( ref TCanvasInfo theme, Color backColor )
		{
			return TCanvasInfo.CreateLayer( ref theme, ( uint ) ColorTranslator.ToWin32( backColor ), 3 );
		}

		public static bool CreateLayer( ref TCanvasInfo theme, uint backColor, byte colors )
		{
			theme.Colors = colors;

			if ( TCanvasInfo.CreateCanvas( IntPtr.Zero, ref theme ) )
			{
				GDI32.SetBkColor( theme.Hdc, backColor );
				return true;
			}

			return false;
		}

		public static bool CreateCanvasFromHwnd( IntPtr hwnd, ref TCanvasInfo canvas )
		{
			IntPtr hdc = USER32.GetWindowDC( hwnd );
			if ( hdc != IntPtr.Zero )
			{
				bool ok = TCanvasInfo.CreateCanvas( hdc, IntPtr.Zero, 0, ref canvas );
				USER32.ReleaseDC( hwnd, hdc );
				return ok;
			}

			return false;
		}

		public static bool CreateBufferCanvas( IntPtr parentDC, ref TCanvasInfo bufferCanvas )
		{
			bufferCanvas.SelectedBmp = GDI32.CreateCompatibleBitmap( parentDC, bufferCanvas.Width, bufferCanvas.Height );
			if ( bufferCanvas.SelectedBmp == IntPtr.Zero )
				return false;

			bufferCanvas.Hdc = GDI32.CreateCompatibleDC( parentDC );
			bufferCanvas.OldBmp = GDI32.SelectObject( bufferCanvas.Hdc, bufferCanvas.SelectedBmp );

			return true;
		}

		public static void DestoryCanvas( ref TCanvasInfo canvas )
		{
			if ( canvas.Hdc != IntPtr.Zero )
			{
				if ( canvas.OldBmp != IntPtr.Zero )
					GDI32.DeleteObject( GDI32.SelectObject( canvas.Hdc, canvas.OldBmp ) );

				GDI32.DeleteDC( canvas.Hdc );// $RIC, cause SelectedBmp is selected by DC, delete it first
			}
			else
				if ( canvas.SelectedBmp != IntPtr.Zero )
					GDI32.DeleteObject( canvas.SelectedBmp );

			canvas = TCanvasInfo.Empty;
		}

		#endregion

		public static void DisplayUnscaled( IntPtr hostHwnd, int hostWidth, int hostHeight, ref TCanvasInfo canvas, int x, int y )
		{
			IntPtr hdc = USER32.GetWindowDC( hostHwnd );
			GDI32.BitBlt( hdc, 0, 0, hostWidth, hostHeight,
				canvas.Hdc, x, y, ERasterOperations.SRCCOPY );
			USER32.ReleaseDC( hostHwnd, hdc );
		}

		public static void DisplayUnscaled( IntPtr hostHwnd, int hostWidth, int hostHeight, ref TCanvasInfo canvas )
		{
			IntPtr hdc = USER32.GetWindowDC( hostHwnd );
			GDI32.BitBlt( hdc, 0, 0, hostWidth, hostHeight,
				canvas.Hdc, 0, 0, ERasterOperations.SRCCOPY );
			USER32.ReleaseDC( hostHwnd, hdc );
		}

		public static void Display( IntPtr hostHwnd, int hostWidth, int hostHeight,
			ref TCanvasInfo srcCanvas, int srcX, int srcY )
		{
			IntPtr hdc = USER32.GetWindowDC( hostHwnd );
			if ( hdc != IntPtr.Zero )
			{
				MGraphics.DrawAutoScaled( hdc, hostWidth, hostHeight,
				  srcCanvas.Hdc, srcX, srcY, srcCanvas.Width, srcCanvas.Height );

				USER32.ReleaseDC( hostHwnd, hdc );
			}
		}

		public static void Display( IntPtr hostHwnd, int hostWidth, int hostHeight,
			ref TCanvasInfo srcCanvas, ref Rectangle clipRect )
		{
			IntPtr hdc = USER32.GetWindowDC( hostHwnd );
			if ( hdc != IntPtr.Zero )
			{
				MGraphics.DrawAutoScaled( hdc, hostWidth, hostHeight,
					srcCanvas.Hdc, clipRect.X, clipRect.Y, clipRect.Width, clipRect.Height );

				USER32.ReleaseDC( hostHwnd, hdc );
			}
		}

		public static void Display( IntPtr hostHwnd, int hostWidth, int hostHeight, ref TCanvasInfo srcCanvas )
		{
			IntPtr hdc = USER32.GetWindowDC( hostHwnd );
			if ( hdc != IntPtr.Zero )
			{
				MGraphics.DrawAutoScaled( hdc, hostWidth, hostHeight,
				  srcCanvas.Hdc, 0, 0, srcCanvas.Width, srcCanvas.Height );

				USER32.ReleaseDC( hostHwnd, hdc );
			}
		}

		public static void DrawOnUnscale( ref TCanvasInfo canvasBase, ref TCanvasInfo canvas, int srcX, int srcY )
		{
			GDI32.BitBlt( canvasBase.Hdc, 0, 0, canvasBase.Width, canvasBase.Height,
				canvas.Hdc, srcX, srcY, ERasterOperations.SRCCOPY );
		}

		public static void DrawOnUnscale( ref TCanvasInfo canvasBase, ref TCanvasInfo canvas )
		{
			GDI32.BitBlt( canvasBase.Hdc, 0, 0, canvasBase.Width, canvasBase.Height,
				canvas.Hdc, 0, 0, ERasterOperations.SRCCOPY );
		}

		public static void DrawOn( ref TCanvasInfo canvasBase, ref TCanvasInfo canvas )
		{
			MGraphics.DrawAutoScaled( canvasBase.Hdc, canvasBase.Width, canvasBase.Height,
					canvas.Hdc, canvas.Width, canvas.Height );
		}

		public static void DrawOn( ref TCanvasInfo canvasBase, ref TCanvasInfo canvas, ref Rectangle clipRect )
		{
			MGraphics.DrawAutoScaled( canvasBase.Hdc, canvasBase.Width, canvasBase.Height,
					canvas.Hdc, clipRect.X, clipRect.Y, clipRect.Width, clipRect.Height );
		}

		public static void DrawOn( ref TCanvasInfo canvasBase, ref TCanvasInfo canvas, int srcX, int srcY )
		{
			MGraphics.DrawAutoScaled( canvasBase.Hdc, canvasBase.Width, canvasBase.Height,
					canvas.Hdc, srcX, srcY, canvas.Width, canvas.Height );
		}

		/// <summary>
		/// Draw Layer Unscaled
		/// </summary>
		public static void DrawLayer( ref TCanvasInfo canvasBase, ref TCanvasInfo canvasLayer )
		{
			MGraphics.DrawLayer( canvasBase.Hdc, canvasBase.Width, canvasBase.Height, canvasLayer.Hdc, 0 );
		}

		public static Bitmap DumpImage( ref TCanvasInfo canvas )
		{
			IntPtr outptr = IntPtr.Zero;
			Bitmap image = null;

			if ( canvas.DataBits == IntPtr.Zero )
			{
				outptr = Marshal.AllocHGlobal( ( int ) canvas.DataSize );
				if ( canvas.RetriveDataBits( ref outptr ) == false )
				{
					Marshal.FreeHGlobal( outptr );
					return null;
				}

				image = canvas.MakeBitmap( ref outptr );
				Marshal.FreeHGlobal( outptr );
			}
			else
			{
				outptr = canvas.DataBits;
				image = canvas.MakeBitmap( ref outptr );
			}

			return image;
		}

		public static void Clear( ref TCanvasInfo canvas, uint backColor )
		{
			MGraphics.Clear( canvas.Hdc, backColor, canvas.Width, canvas.Height );
		}

		public static void Clear( ref TCanvasInfo canvas, Color backColor )
		{
			MGraphics.Clear( canvas.Hdc, ( uint ) ColorTranslator.ToWin32( backColor ), canvas.Width, canvas.Height );
		}

		#endregion

		public bool IsEmpty
		{
			get
			{
				return this.Equals( TCanvasInfo.Empty );
			}
		}
	}

}
