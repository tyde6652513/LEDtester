using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;

namespace MPI.Drawing
{
	public class MGraphics
	{
		#region >>> Internal helper <<<

		public static bool InitBitmapInfo( Image image, out TBITMAPINFO bmi )
		{
			return InitBitmapInfo( image.PixelFormat, image.Width, image.Height, out bmi );
		}

		public static bool InitBitmapInfo( Image image, int width, int height, out TBITMAPINFO bmi )
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

		public static bool StretchDrawDC( IntPtr dstDC, int dstWidth, int dstHeight, IntPtr srcDC, int srcWidth, int srcHeight )
		{
			GDI32.SetStretchBltMode( dstDC, EStretchMode.DELETESCANS );
			return GDI32.StretchBlt( dstDC, 0, 0, dstWidth, dstHeight, srcDC, 0, 0, srcWidth, srcHeight, ERasterOperations.SRCCOPY );
		}

		public static void DrawDC( IntPtr dstDC, int dstWidth, int dstHeight, IntPtr srcDC )
		{
			GDI32.BitBlt( dstDC, 0, 0, dstWidth, dstHeight, srcDC, 0, 0, ERasterOperations.SRCCOPY );
		}

		public static void DrawTheme( IntPtr dstDC, int width, int height, IntPtr themeDC, int backColor )
		{
			// ColorTranslator.ToWin32( Color.Black ): 0
			GDI32.GdiTransparentBlt( dstDC, 0, 0, width, height, themeDC, 0, 0, width, height, backColor );
		}

		public static int DrawImageTransparent( IntPtr dstDC, int dstWidth, int dstHeight, IntPtr srcImage, ref TBITMAPINFO srcBmi, int backColor )
		{
			//long tick = HiTimer.Tick;

			IntPtr overlay_dc = GDI32.CreateCompatibleDC( dstDC );
			IntPtr overlay_bmp;
			IntPtr overlay_temp;
			int scan = 0;

			// Copy image data to DC
			if ( srcBmi.EqualSize( dstWidth, dstHeight ) )
			{
				#region >>> Method 1 fastest <<<

				IntPtr out_ptr = IntPtr.Zero;
				overlay_bmp = GDI32.CreateDIBSection( dstDC, ref srcBmi, EDIBColor.DIB_RGB_COLORS, out out_ptr, IntPtr.Zero, 0 );
				//scan = GDI32.SetDIBits( overlay_dc, overlay_bmp, 0, dstHeight, srcImage, ref srcBmi, EDIBColor.DIB_RGB_COLORS );
				MPI.Win32.Win32API.Memory.CopyMemory( out_ptr, srcImage, dstWidth * dstHeight * 3 );

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
				overlay_bmp = GDI32.CreateDIBSection( dstDC, ref srcBmi, EDIBColor.DIB_RGB_COLORS, out out_ptr, IntPtr.Zero, 0 );

				overlay_temp = GDI32.SelectObject( overlay_dc, overlay_bmp );

				GDI32.SetStretchBltMode( overlay_dc, EStretchMode.DELETESCANS );
				GDI32.StretchBlt( overlay_dc, 0, 0, dstWidth, dstHeight,
					overlay_dc, 0, 0, srcBmi.Width, srcBmi.Height, ERasterOperations.SRCCOPY );
			}

			//GDI32.SetDIBits( src_dc, src_bmp, 0, srcBmi.Height, srcImage, ref srcBmi, EDIBColor.DIB_RGB_COLORS );

			//Console.WriteLine( "[SetDIBits] {0:F4}", HiTimer.Evaluate( tick ) );

			// draw overlay on dstDC
			GDI32.GdiTransparentBlt( dstDC, 0, 0, dstWidth, dstHeight, overlay_dc,
				0, 0, srcBmi.Width, srcBmi.Height, backColor );

			GDI32.SelectObject( overlay_dc, overlay_temp );
			GDI32.DeleteObject( overlay_bmp );
			GDI32.DeleteDC( overlay_dc );
			return scan;
		}

		public static int DrawImage( IntPtr hdc, Bitmap srcImage, ref TBITMAPINFO srcBmi )
		{
			int width = srcImage.Width;
			int height = srcImage.Height;

			BitmapData bd = srcImage.LockBits( new Rectangle( 0, 0, width, height ), ImageLockMode.ReadOnly, srcImage.PixelFormat );

			int scan = GDI32.SetDIBitsToDevice( hdc, 0, 0, width, height, 0, 0, 0, height, bd.Scan0, ref srcBmi, EDIBColor.DIB_RGB_COLORS );

			srcImage.UnlockBits( bd );

			bd = null;

			return scan;
		}

		public static int DrawImage( IntPtr hdc, int dstWidth, int dstHeight, ref TBITMAPINFO srcBmi, IntPtr srcPtr )
		{
			return GDI32.SetDIBitsToDevice( hdc, 0, 0, dstWidth, dstHeight, 0, 0, 0, srcBmi.Header.Height, srcPtr, ref srcBmi, EDIBColor.DIB_RGB_COLORS );
		}

		public static int StretchImage( IntPtr hdc, int dstWidth, int dstHeight, ref TBITMAPINFO srcBmi, IntPtr srcPtr, int srcWidth, int srcHeight )
		{
			GDI32.SetStretchBltMode( hdc, EStretchMode.DELETESCANS );
			int scan_line = GDI32.StretchDIBits( hdc, 0, 0, dstWidth, dstHeight,
				0, 0, srcWidth, srcHeight,
				srcPtr, ref srcBmi, EDIBColor.DIB_RGB_COLORS, ERasterOperations.SRCCOPY );

			return scan_line;
		}

		#endregion

		#region >>> Public Static Method <<<

		public static ICanvas CreateFromImage( Bitmap image )
		{
			return new ImageCanvas( image );
		}

		public static ICanvas CreateFromHwnd( IntPtr hwnd )
		{
			return new WindowCanvas( hwnd );
		}

		public static ICanvas CreateFromHdc( IntPtr hdc, int width, int height )
		{
			return new MCanvas( hdc, width, height );
		}

		public static ICanvas Create( int width, int height, PixelFormat format )
		{
			ushort channel = 3;
			if ( format == PixelFormat.Indexed )
				channel = 1;

			return new GDICanvas( width, height, channel );
		}
		#endregion
	}

	public class MCanvas : ICanvas
	{
		private IntPtr _hOldPen;
		private IntPtr _hOldFont;

		protected Matrix _trsf;

		protected IntPtr _canvasDC;
		protected IntPtr _oldCanvasBMP;

		/// <summary>
		/// Default: Black color ( 0 )
		/// </summary>
		protected int _background;
		protected int _canvasWidth;
		protected int _canvasHeight;

		private LOGFONT _logFont;
		private LOGPEN _logPen;


		protected MCanvas()
		{
			_logFont = new LOGFONT( true );
			_logPen = new LOGPEN( true );
			_trsf = new Matrix();
			_worldCorner = new Point[] { Point.Empty, Point.Empty, Point.Empty };
		}

		public MCanvas( IntPtr hdc, int width, int height, int bgColor )
			: this()
		{
			_canvasDC = hdc;
			_canvasWidth = width;
			_canvasHeight = height;

			_background = bgColor;


			// Set the mapping mode to LOENGLISH. This moves the  
			// client area origin from the upper left corner of the  
			// window to the lower left corner (this also reorients  
			// the y-axis so that drawing operations occur in a true  
			// Cartesian space). It guarantees portability so that  
			// the object drawn retains its dimensions on any display.  

			//GDI32.SetMapMode( hDC, MM_LOENGLISH ); 
		}

		public MCanvas( IntPtr hdc, IntPtr bmp, int width, int height, int bgColor )
			: this( hdc, width, height, bgColor )
		{
			_oldCanvasBMP = bmp;
		}

		public MCanvas( IntPtr hdc, int width, int height )
			: this( hdc, width, height, 0 )
		{

		}

		public virtual void Dispose()
		{
			if ( _canvasDC != IntPtr.Zero )
			{
				if ( _oldCanvasBMP != IntPtr.Zero )
				{
					GDI32.DeleteObject( GDI32.SelectObject( _canvasDC, _oldCanvasBMP ) );
					_oldCanvasBMP = IntPtr.Zero;
				}

				GDI32.DeleteDC( _canvasDC );
				_canvasDC = IntPtr.Zero;
			}

			if ( _trsf != null )
			{
				_trsf.Dispose();
				_trsf = null;
			}
		}

		public virtual ERegionError Clip( Rectangle rect )
		{
			return GDI32.SelectClipRgn( _canvasDC, GDI32.CreateRectRgn( rect.Left, rect.Top, rect.Right, rect.Bottom ) );
		}

		public virtual void ResetClip()
		{
			GDI32.SelectClipRgn( _canvasDC, IntPtr.Zero );
		}

		#region >>> Clear <<<

		public void Clear()
		{
			this.Clear( _background );
		}

		public void Clear( Color color )
		{
			this.Clear( ColorTranslator.ToWin32( color ) );
		}

		public virtual void Clear( int color )
		{
			GDI32.SetDCBrushColor( _canvasDC, color );
			IntPtr old = GDI32.SelectObject( _canvasDC, GDI32.GetStockObject( EStockObject.DC_BRUSH ) );
			GDI32.Rectangle( _canvasDC, 0, 0, _canvasWidth, _canvasHeight );
			GDI32.SelectObject( _canvasDC, old );
		}
		#endregion

		public virtual void DrawCross( Pen pen, int x, int y, int size )
		{
			this.SetPen( _canvasDC, pen );

			GDI32.MoveToEx( _canvasDC, x - size, y - size, IntPtr.Zero );
			GDI32.LineTo( _canvasDC, x + size, y + size );

			GDI32.MoveToEx( _canvasDC, x - size, y + size, IntPtr.Zero );
			GDI32.LineTo( _canvasDC, x + size, y - size );

			this.ResetPen( _canvasDC );
		}

		#region >>> Pixel <<<
		public virtual int SetPixel( int x, int y, Color color )
		{
			return GDI32.SetPixel( _canvasDC, x, y, ColorTranslator.ToWin32( color ) );
		}

		public virtual Color GetPixel( int x, int y )
		{
			return ColorTranslator.FromWin32( GDI32.GetPixel( _canvasDC, x, y ) );
		}
		#endregion

		#region >>> Draw Rectangle <<<

		public void DrawRectangle( Pen pen, float x, float y, float width, float height )
		{
			this.DrawRectangle( pen, ( int ) x, ( int ) y, ( int ) width, ( int ) height );
		}

		public void DrawRectangle( Pen pen, Rectangle rect )
		{
			this.DrawRectangle( pen, rect.X, rect.Y, rect.Width, rect.Height );
		}

		public void DrawRectangle( Pen pen, RectangleF rect )
		{
			this.DrawRectangle( pen, ( int ) rect.X, ( int ) rect.Y, ( int ) rect.Width, ( int ) rect.Height );
		}

		public virtual void DrawRectangle( Pen pen, int x, int y, int width, int height )
		{
			this.SetPen( _canvasDC, pen );
			IntPtr old = GDI32.SelectObject( _canvasDC, GDI32.GetStockObject( EStockObject.NULL_BRUSH ) );
			GDI32.Rectangle( _canvasDC, x, y, ( x + width + 1 ), ( y + height + 1 ) );
			GDI32.SelectObject( _canvasDC, old );
			this.ResetPen( _canvasDC );
		}
		#endregion

		#region >>> Draw Line <<<

		public void DrawLine( Pen pen, PointF pt1, PointF pt2 )
		{
			this.DrawLine( pen, ( int ) pt1.X, ( int ) pt1.Y, ( int ) pt2.X, ( int ) pt2.Y );
		}

		public void DrawLine( Pen pen, Point pt1, Point pt2 )
		{
			this.DrawLine( pen, pt1.X, pt1.Y, pt2.X, pt2.Y );
		}

		public void DrawLine( Pen pen, float x1, float y1, float x2, float y2 )
		{
			this.DrawLine( pen, ( int ) x1, ( int ) y1, ( int ) x2, ( int ) y2 );
		}

		public void DrawLines( Pen pen, PointF[] points )
		{
			if ( points != null )
			{
				Point[] pts = new Point[points.Length];
				for ( int i = pts.Length - 1; i >= 0; i-- )
				{
					pts[i].X = ( int ) points[i].X;
					pts[i].Y = ( int ) points[i].Y;
				}

				this.DrawLines( pen, pts );
				pts = null;
			}
		}

		public virtual void DrawLine( Pen pen, int x1, int y1, int x2, int y2 )
		{
			this.SetPen( _canvasDC, pen );
			GDI32.MoveToEx( _canvasDC, x1, y1, IntPtr.Zero );
			GDI32.LineTo( _canvasDC, x2, y2 );
			this.ResetPen( _canvasDC );
		}

		public virtual void DrawLines( Pen pen, Point[] points )
		{
			if ( points != null )
			{
				this.SetPen( _canvasDC, pen );
				GDI32.Polyline( _canvasDC, Marshal.UnsafeAddrOfPinnedArrayElement( points, 0 ), points.Length );
				this.ResetPen( _canvasDC );
			}
		}

		#endregion

		#region >>> Draw Arc <<<

		public void DrawArc( Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle )
		{
			RectangleF rect = new RectangleF( x, y, width, height );
			this.DrawArc( pen, rect, startAngle, sweepAngle );
		}

		public void DrawArc( Pen pen, Rectangle rect, float startAngle, float sweepAngle )
		{
			RectangleF rect2 = new RectangleF( rect.Location, rect.Size );
			this.DrawArc( pen, rect2, startAngle, sweepAngle );
		}

		public void DrawArc( Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle )
		{
			RectangleF rect = new RectangleF( x, y, width, height );
			this.DrawArc( pen, rect, startAngle, sweepAngle );
		}

		public virtual void DrawArc( Pen pen, RectangleF rect, float startAngle, float sweepAngle )
		{
			this.SetPen( _canvasDC, pen );

			float w = rect.Width / 2;
			float h = rect.Height / 2;
			float x0 = rect.X + w;
			float y0 = rect.Y + h;

			int left2 = ( int ) ( x0 + w * ( float ) Math.Cos( sweepAngle * Math.PI / 180 ) );
			int right2 = ( int ) ( x0 + w * ( float ) Math.Cos( startAngle * Math.PI / 180 ) );

			int top2 = ( int ) ( y0 + h * ( float ) Math.Sin( sweepAngle * Math.PI / 180 ) );
			int bottom2 = ( int ) ( y0 + h * ( float ) Math.Sin( startAngle * Math.PI / 180 ) );

			GDI32.SetArcDirection( _canvasDC, EArcDirection.AD_CLOCKWISE );
			bool ok = GDI32.Arc( _canvasDC, ( int ) rect.Left, ( int ) rect.Top, ( int ) rect.Right, ( int ) rect.Bottom,
				left2, top2, right2, bottom2 );

			this.ResetPen( _canvasDC );
		}

		#endregion

		#region >>> Draw Bezier <<<

		public void DrawBezier( Pen pen, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4 )
		{
			Point[] points = new Point[] { new Point( ( int ) x1, ( int ) y1 ), 	new Point( ( int ) x2, ( int ) y2 ), 
				new Point( ( int ) x3, ( int ) y3 ), new Point( ( int ) x4, ( int ) y4 ) };

			this.DrawBeziers( pen, points );
		}

		public void DrawBezier( Pen pen, PointF pt1, PointF pt2, PointF pt3, PointF pt4 )
		{
			Point[] points = new Point[] { new Point( ( int ) pt1.X, ( int ) pt1.Y ), 	new Point( ( int ) pt2.X, ( int ) pt2.Y ), 
				new Point( ( int ) pt3.X, ( int ) pt3.Y ), new Point( ( int ) pt4.X, ( int ) pt4.Y ) };
			this.DrawBeziers( pen, points );
		}

		public void DrawBezier( Pen pen, Point pt1, Point pt2, Point pt3, Point pt4 )
		{
			Point[] points = new Point[] { pt1, pt2, pt3, pt4 };
			this.DrawBeziers( pen, points );
		}

		public void DrawBeziers( Pen pen, PointF[] points )
		{
			if ( points != null )
			{
				Point[] ary = new Point[points.Length];
				for ( int i = points.Length - 1; i >= 0; i-- )
					ary[i] = new Point( ( int ) points[i].X, ( int ) points[i].Y );

				DrawBeziers( pen, ary );

				ary = null;
			}
		}

		public virtual void DrawBeziers( Pen pen, Point[] points )
		{
			this.SetPen( _canvasDC, pen );
			MCanvas.DrawBeziers( _canvasDC, points );
			this.ResetPen( _canvasDC );
		}

		internal static void DrawBeziers( IntPtr hdc, Point[] points )
		{
			IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement( points, 0 );
			GDI32.PolyBezier( hdc, ptr, points.Length );
		}
		#endregion

		#region >>> Draw Ellipse <<<

		public void DrawEllipse( Pen pen, RectangleF rect )
		{
			this.DrawEllipse( pen, rect.X, rect.Y, rect.Width, rect.Height );
		}

		public void DrawEllipse( Pen pen, float x, float y, float width, float height )
		{
			this.DrawEllipse( pen, ( int ) x, ( int ) y, ( int ) width, ( int ) height );
		}

		public void DrawEllipse( Pen pen, Rectangle rect )
		{
			this.DrawEllipse( pen, rect.X, rect.Y, rect.Width, rect.Height );
		}

		public virtual void DrawEllipse( Pen pen, int x, int y, int width, int height )
		{
			this.SetPen( _canvasDC, pen );
			IntPtr old = GDI32.SelectObject( _canvasDC, GDI32.GetStockObject( EStockObject.NULL_BRUSH ) );
			GDI32.Ellipse( _canvasDC, x, y, ( x + width ), ( y + height ) );
			GDI32.SelectObject( _canvasDC, old );
			this.ResetPen( _canvasDC );
		}

		#endregion

		#region >>> Draw Pie (BUG) <<<
		public void DrawPie( Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle )
		{
			RectangleF rect = new RectangleF( x, y, width, height );
			this.DrawPie( pen, rect, startAngle, sweepAngle );
		}

		public void DrawPie( Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle )
		{
			RectangleF rect = new RectangleF( x, y, width, height );
			this.DrawPie( pen, rect, startAngle, sweepAngle );
		}

		public void DrawPie( Pen pen, Rectangle rect, float startAngle, float sweepAngle )
		{
			RectangleF rect2 = new RectangleF( rect.Location, rect.Size );
			this.DrawPie( pen, rect2, startAngle, sweepAngle );
		}

		public virtual void DrawPie( Pen pen, RectangleF rect, float startAngle, float sweepAngle )
		{
			this.SetPen( _canvasDC, pen );

			float w = rect.Width / 2;
			float h = rect.Height / 2;
			float x0 = rect.X + w;
			float y0 = rect.Y + h;

			int left2 = ( int ) ( x0 + w * ( float ) Math.Cos( sweepAngle * Math.PI / 180 ) );
			int right2 = ( int ) ( x0 + w * ( float ) Math.Cos( startAngle * Math.PI / 180 ) );

			int top2 = ( int ) ( y0 + h * ( float ) Math.Sin( sweepAngle * Math.PI / 180 ) );
			int bottom2 = ( int ) ( y0 + h * ( float ) Math.Sin( startAngle * Math.PI / 180 ) );

			IntPtr old = GDI32.SelectObject( _canvasDC, GDI32.GetStockObject( EStockObject.NULL_BRUSH ) );

			//$RIC, BUG
			bool ok = GDI32.Pie( _canvasDC, ( int ) rect.Left, ( int ) rect.Top, ( int ) rect.Right, ( int ) rect.Bottom,
				left2, top2, right2, bottom2 );

			GDI32.SelectObject( _canvasDC, old );

			this.ResetPen( _canvasDC );
		}

		#endregion

		#region >>> Draw Polygon <<<

		public void DrawPolygon( Pen pen, PointF[] points )
		{
			if ( points != null )
			{
				Point[] ary = new Point[points.Length];
				for ( int i = points.Length - 1; i >= 0; i-- )
					ary[i] = new Point( ( int ) points[i].X, ( int ) points[i].Y );

				this.DrawPolygon( pen, ary );
				ary = null;
			}
		}

		public virtual void DrawPolygon( Pen pen, Point[] points )
		{
			this.SetPen( _canvasDC, pen );

			IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement( points, 0 );

			IntPtr old = GDI32.SelectObject( _canvasDC, GDI32.GetStockObject( EStockObject.NULL_BRUSH ) );

			GDI32.Polygon( _canvasDC, ptr, points.Length );

			GDI32.SelectObject( _canvasDC, old );

			this.ResetPen( _canvasDC );
		}


		#endregion

		#region >>> Draw Curve ( Not implemented )<<<

		//public void DrawPath( Pen pen, GraphicsPath path )
		//{

		//}

		public void DrawCurve( Pen pen, PointF[] points )
		{

		}

		public void DrawCurve( Pen pen, Point[] points )
		{

		}

		//public void DrawCurve( Pen pen, PointF[] points, float tension )
		//{

		//}

		//public void DrawCurve( Pen pen, Point[] points, float tension )
		//{

		//}

		//public void DrawCurve( Pen pen, PointF[] points, int offset, int numberOfSegments )
		//{
		//   this.DrawCurve( pen, points, offset, numberOfSegments, 0.5f );
		//}

		//public void DrawCurve( Pen pen, PointF[] points, int offset, int numberOfSegments, float tension )
		//{

		//}


		//public void DrawCurve( Pen pen, Point[] points, int offset, int numberOfSegments, float tension )
		//{

		//}

		#endregion


		/*	
				#region >>> Draw Closed Curve ( Not Implemented )<<<
				public void DrawClosedCurve( Pen pen, PointF[] points )
				{

				}

				public void DrawClosedCurve( Pen pen, Point[] points )
				{

				}

				public void DrawClosedCurve( Pen pen, PointF[] points, float tension, FillMode fillmode )
				{

				}

				public void DrawClosedCurve( Pen pen, Point[] points, float tension, FillMode fillmode )
				{

				}

				#endregion
		*/

		#region >>> FillEllipse <<<

		public void FillEllipse( Brush brush, float x, float y, float width, float height )
		{
			this.FillEllipse( brush, ( int ) x, ( int ) y, ( int ) width, ( int ) height );
		}

		public void FillEllipse( Brush brush, Rectangle rect )
		{
			this.FillEllipse( brush, rect.X, rect.Y, rect.Width, rect.Height );
		}

		public void FillEllipse( Brush brush, RectangleF rect )
		{
			this.FillEllipse( brush, ( int ) rect.X, ( int ) rect.Y, ( int ) rect.Width, ( int ) rect.Height );
		}

		public virtual void FillEllipse( Brush brush, int x, int y, int width, int height )
		{
			int color = MCanvas.GetBrushColor( brush );
			GDI32.SetDCBrushColor( _canvasDC, color );
			IntPtr old = GDI32.SelectObject( _canvasDC, GDI32.GetStockObject( EStockObject.DC_BRUSH ) );
			GDI32.Ellipse( _canvasDC, x, y, ( x + width ), ( y + height ) );
			GDI32.SelectObject( _canvasDC, old );
		}

		#endregion

		#region >>> Fill Rectangle <<<

		public void FillRectangle( Brush brush, RectangleF rect )
		{
			this.FillRectangle( brush, ( int ) rect.X, ( int ) rect.Y, ( int ) rect.Width, ( int ) rect.Height );
		}

		public void FillRectangle( Brush brush, float x, float y, float width, float height )
		{
			this.FillRectangle( brush, ( int ) x, ( int ) y, ( int ) width, ( int ) height );
		}

		public void FillRectangle( Brush brush, Rectangle rect )
		{
			this.FillRectangle( brush, rect.X, rect.Y, rect.Width, rect.Height );
		}

		public virtual void FillRectangle( Brush brush, int x, int y, int width, int height )
		{
			int color = GetBrushColor( brush );
			GDI32.SetDCBrushColor( _canvasDC, color );
			IntPtr old = GDI32.SelectObject( _canvasDC, GDI32.GetStockObject( EStockObject.DC_BRUSH ) );

			GDI32.Rectangle( _canvasDC, x, y, ( x + width + 1 ), ( y + height + 1 ) );
			GDI32.SelectObject( _canvasDC, old );
		}

		#endregion

		/*
		#region >>> Fill Action <<<

		public void FillPolygon( Brush brush, PointF[] points )
		{
			this.FillPolygon( brush, points, FillMode.Alternate );
		}

		public void FillPolygon( Brush brush, PointF[] points, FillMode fillMode )
		{

		}

		public void FillPolygon( Brush brush, Point[] points )
		{
			this.FillPolygon( brush, points, FillMode.Alternate );
		}

		public void FillPolygon( Brush brush, Point[] points, FillMode fillMode )
		{

		}

		public void FillPie( Brush brush, Rectangle rect, float startAngle, float sweepAngle )
		{
			this.FillPie( brush, ( float ) rect.X, ( float ) rect.Y, ( float ) rect.Width, ( float ) rect.Height, startAngle, sweepAngle );
		}

		public void FillPie( Brush brush, float x, float y, float width, float height, float startAngle, float sweepAngle )
		{

		}

		public void FillPie( Brush brush, int x, int y, int width, int height, int startAngle, int sweepAngle )
		{

		}

		public void FillPath( Brush brush, GraphicsPath path )
		{

		}

		public void FillClosedCurve( Brush brush, PointF[] points )
		{

		}

		public void FillClosedCurve( Brush brush, PointF[] points, FillMode fillmode )
		{
			this.FillClosedCurve( brush, points, fillmode, 0.5f );
		}

		public void FillClosedCurve( Brush brush, PointF[] points, FillMode fillmode, float tension )
		{

		}

		public void FillClosedCurve( Brush brush, Point[] points )
		{

		}

		public void FillClosedCurve( Brush brush, Point[] points, FillMode fillmode )
		{
			this.FillClosedCurve( brush, points, fillmode, 0.5f );
		}

		public void FillClosedCurve( Brush brush, Point[] points, FillMode fillmode, float tension )
		{

		}

		public void FillRegion( Brush brush, Region region )
		{

		}

		#endregion
*/

		#region >>> Draw String without rectangle <<<

		public void DrawString( string s, Font font, Brush brush, int x, int y )
		{
			Rectangle rect = new Rectangle( x, y, 0, 0 );
			this.DrawString( s, font, MCanvas.GetBrushColor( brush ), ref rect, EDrawTextFormat.DT_NULL );
		}

		public void DrawString( string s, Font font, Brush brush, float x, float y )
		{
			Rectangle rect = new Rectangle( ( int ) x, ( int ) y, 0, 0 );
			this.DrawString( s, font, MCanvas.GetBrushColor( brush ), ref rect, EDrawTextFormat.DT_NULL );
		}

		public void DrawString( string s, Font font, Brush brush, PointF point )
		{
			Rectangle rect = new Rectangle( ( int ) point.X, ( int ) point.Y, 0, 0 );
			this.DrawString( s, font, MCanvas.GetBrushColor( brush ), ref rect, EDrawTextFormat.DT_NULL );
		}

		public void DrawString( string s, Font font, Color color, int x, int y )
		{
			Rectangle rect = new Rectangle( x, y, 0, 0 );
			this.DrawString( s, font, ColorTranslator.ToWin32( color ), ref rect, EDrawTextFormat.DT_NULL );
		}
		#endregion

		#region >>> Draw String with rectangle <<<

		public void DrawString( string s, Font font, Brush brush, Rectangle rect )
		{
			this.DrawString( s, font, MCanvas.GetBrushColor( brush ), ref rect, EDrawTextFormat.DT_DEFAULT );
		}

		public void DrawString( string s, Font font, Color color, Rectangle rect )
		{
			this.DrawString( s, font, ColorTranslator.ToWin32( color ), ref rect, EDrawTextFormat.DT_DEFAULT );
		}

		public void DrawString( string s, Font font, Brush brush, RectangleF layoutRectangle )
		{
			Rectangle rect = new Rectangle( ( int ) layoutRectangle.X, ( int ) layoutRectangle.Y,
				( int ) layoutRectangle.Width, ( int ) layoutRectangle.Height );

			this.DrawString( s, font, MCanvas.GetBrushColor( brush ), ref rect, EDrawTextFormat.DT_DEFAULT );
		}

		public void DrawString( string s, Font font, Brush brush, float x, float y, StringFormat format )
		{
			RectangleF rect = new RectangleF( x, y, 0, 0 );
			this.DrawString( s, font, brush, rect, format );
		}

		public void DrawString( string s, Font font, Brush brush, PointF point, StringFormat format )
		{
			RectangleF rect = new RectangleF( point, SizeF.Empty );
			this.DrawString( s, font, brush, rect, format );
		}

		public void DrawString( string s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat format )
		{
			EDrawTextFormat fmt = EDrawTextFormat.DT_DEFAULT;
			if ( format != null )
			{
				fmt = fmt | ( EDrawTextFormat ) ( ( uint ) format.Alignment | ( uint ) EDrawTextFormat.DT_VCENTER ); //v-align, 0x00(0x00), 0x04(0x01), 0x08(0x02)
				fmt = fmt | ( EDrawTextFormat ) format.LineAlignment;
			}

			Rectangle rect = new Rectangle( ( int ) layoutRectangle.X, ( int ) layoutRectangle.Y,
				( int ) layoutRectangle.Width, ( int ) layoutRectangle.Height );

			this.DrawString( s, font, MCanvas.GetBrushColor( brush ), ref rect, fmt );
		}

		protected virtual void DrawString( string s, Font font, int color, ref Rectangle rect, EDrawTextFormat format )
		{
			this.SetFont( _canvasDC, font );

			GDI32.SetBkMode( _canvasDC, EBackgroundMode.TRANSPARENT );

			color = GDI32.SetTextColor( _canvasDC, color );

			if ( format != EDrawTextFormat.DT_NULL )
			{
				if ( rect.Size.IsEmpty )
				{
					Size size;
					GDI32.GetTextExtentPoint( _canvasDC, s, s.Length, out size );
					rect.Size = size;
				}

				USER32.DrawText( _canvasDC, s, s.Length, ref rect, format );
			}
			else
				GDI32.TextOut( _canvasDC, rect.X, rect.Y, s, s.Length );

			GDI32.SetTextColor( _canvasDC, color );

			this.ResetFont( _canvasDC );
		}

		public void MeasureString( string s, out Size size )
		{
			GDI32.GetTextExtentPoint( _canvasDC, s, s.Length, out size );
		}

		public void MeasureString( string s, Font font, out Size size )
		{
			this.SetFont( _canvasDC, font );
			GDI32.GetTextExtentPoint( _canvasDC, s, s.Length, out size );
			this.ResetFont( _canvasDC );
		}

		#endregion

		#region >>> Measure String <<<
		//public SizeF MeasureString( string text, Font font, SizeF layoutArea, StringFormat stringFormat, out int charactersFitted, out int linesFilled )
		//{
		//   if ( text == null || text.Length == 0 )
		//   {
		//      charactersFitted = 0;
		//      linesFilled = 0;
		//      return SizeF.Empty;
		//   }

		//   charactersFitted = 0;
		//   linesFilled = 0;
		//   return SizeF.Empty;
		//}

		//public SizeF MeasureString( string text, Font font, PointF origin, StringFormat stringFormat )
		//{
		//   if ( text == null || text.Length == 0 )
		//   {
		//      return SizeF.Empty;
		//   }

		//   RectangleF gPRECTF = default( Rectangle );
		//   RectangleF gPRECTF2 = default( Rectangle );

		//   gPRECTF.X = origin.X;
		//   gPRECTF.Y = origin.Y;
		//   gPRECTF.Width = 0f;
		//   gPRECTF.Height = 0f;

		//   //int num2;
		//   //int num3;
		//   //int num = SafeNativeMethods.Gdip.GdipMeasureString( new HandleRef( this, this.NativeGraphics ), text, text.Length, new HandleRef( font, font.NativeFont ), ref gPRECTF, new HandleRef( stringFormat, ( stringFormat == null ) ? IntPtr.Zero : stringFormat.nativeFormat ), ref gPRECTF2, out num2, out num3 );
		//   //if ( num != 0 )
		//   //{
		//   //   throw SafeNativeMethods.Gdip.StatusException( num );
		//   //}
		//   return gPRECTF2.Size;
		//}

		//public SizeF MeasureString( string text, Font font, SizeF layoutArea )
		//{
		//   return this.MeasureString( text, font, layoutArea, null );
		//}

		//public SizeF MeasureString( string text, Font font, SizeF layoutArea, StringFormat stringFormat )
		//{
		//   if ( text == null || text.Length == 0 )
		//   {
		//      return SizeF.Empty;
		//   }

		//   RectangleF gPRECTF = new RectangleF( 0f, 0f, layoutArea.Width, layoutArea.Height );
		//   RectangleF gPRECTF2 = default( RectangleF );
		//   //int num2;
		//   //int num3;
		//   //int num = SafeNativeMethods.Gdip.GdipMeasureString( new HandleRef( this, this.NativeGraphics ), text, text.Length, new HandleRef( font, font.NativeFont ), ref gPRECTF, new HandleRef( stringFormat, ( stringFormat == null ) ? IntPtr.Zero : stringFormat.nativeFormat ), ref gPRECTF2, out num2, out num3 );
		//   //if ( num != 0 )
		//   //{
		//   //   throw SafeNativeMethods.Gdip.StatusException( num );
		//   //}
		//   return gPRECTF2.Size;
		//}

		//public SizeF MeasureString( string text, Font font )
		//{
		//   return this.MeasureString( text, font, new SizeF( 0f, 0f ) );
		//}

		//public SizeF MeasureString( string text, Font font, int width )
		//{
		//   return this.MeasureString( text, font, new SizeF( ( float ) width, 999999f ) );
		//}

		//public SizeF MeasureString( string text, Font font, int width, StringFormat format )
		//{
		//   return this.MeasureString( text, font, new SizeF( ( float ) width, 999999f ), format );
		//}
		#endregion

		internal static int GetBrushColor( Brush brush )
		{
			Pen pen = new Pen( brush );
			int color = ColorTranslator.ToWin32( pen.Color );
			pen.Dispose();
			pen = null;
			return color;
		}

		internal static void SetBrushColor( IntPtr hdc, Color color )
		{
			GDI32.SetDCBrushColor( hdc, ColorTranslator.ToWin32( color ) );
		}

		internal void SetPen( IntPtr hdc, Pen pen )
		{
			_logPen.PenColor = ColorTranslator.ToWin32( pen.Color );
			_logPen.PenWidth.X = ( int ) pen.Width;
			_logPen.Style = ( EPenStyle ) ( ( uint ) pen.DashStyle );
			_hOldPen = GDI32.SelectObject( hdc, GDI32.CreatePenIndirect( ref _logPen ) );
		}

		internal void ResetPen( IntPtr hdc )
		{
			GDI32.DeleteObject( GDI32.SelectObject( hdc, _hOldPen ) );
		}

		internal void SetFont( IntPtr hdc, Font font )
		{
			//_font.Height = -MulDiv(PointSize, GetDeviceCaps(hDC, LOGPIXELSY), 72);
			_logFont.Height = font.Height;
			_logFont.Charset = font.GdiCharSet;
			_logFont.FaceName = font.Name;
			_logFont.Italic = ( byte ) ( font.Italic ? 0x01 : 0x00 );
			_logFont.Underline = ( byte ) ( font.Underline ? 1 : 0 );
			_logFont.Weight = ( font.Bold ? EFontWeight.FW_BOLD : EFontWeight.FW_NORMAL );
			_logFont.StrikeOut = ( byte ) ( font.Strikeout ? 1 : 0 );
			_logFont.PitchAndFamily = EFontPitchAndFamily.FF_DONTCARE;
			//_logFont.Quality = EFontQuality.NONANTIALIASED_QUALITY;
			_logFont.Quality = EFontQuality.ANTIALIASED_QUALITY;
			_hOldFont = GDI32.SelectObject( hdc, GDI32.CreateFontIndirect( ref _logFont ) );
		}

		internal void ResetFont( IntPtr hdc )
		{
			GDI32.DeleteObject( GDI32.SelectObject( hdc, _hOldFont ) );
		}

		#region >>> Draw Image Data <<<

		public void DrawImageData( IntPtr srcPtr, PixelFormat format )
		{
			this.DrawImageData( srcPtr, _canvasWidth, _canvasHeight, format );
		}

		public virtual void DrawImageData( IntPtr srcPtr, int srcWidth, int srcHeight, PixelFormat format )
		{
			TBITMAPINFO src_bmi;
			if ( MGraphics.InitBitmapInfo( format, srcWidth, srcHeight, out src_bmi ) == false )
				throw new ArgumentException( "Unsupported Format", "format" );

			Rectangle rect = new Rectangle( 0, 0, _canvasWidth, _canvasHeight );
			MCanvas.DrawImageData( _canvasDC, ref rect, srcPtr, ref src_bmi );
		}

		internal static int DrawImageData( IntPtr dstDC, ref Rectangle dstRect, IntPtr srcPtr, ref TBITMAPINFO srcBmi )
		{
			int scan_line;

			if ( srcBmi.EqualSize( dstRect.Width, dstRect.Height ) )
			{
				scan_line = GDI32.SetDIBitsToDevice( dstDC, dstRect.X, dstRect.Y, dstRect.Width, dstRect.Height,
					0, 0, 0, srcBmi.Height,
					srcPtr, ref srcBmi, EDIBColor.DIB_RGB_COLORS );
			}
			else
			{
				GDI32.SetStretchBltMode( dstDC, EStretchMode.DELETESCANS );

				scan_line = GDI32.StretchDIBits( dstDC, dstRect.X, dstRect.Y, dstRect.Width, dstRect.Height,
					0, 0, srcBmi.Width, srcBmi.Height,
					srcPtr, ref srcBmi, EDIBColor.DIB_RGB_COLORS, ERasterOperations.SRCCOPY );
			}

			return scan_line;
		}

		#endregion

		#region >>> Draw DC <<<
		public void DrawDC( IntPtr srcDC )
		{
			this.DrawDC( srcDC, _canvasWidth, _canvasHeight );
		}

		public virtual void DrawDC( IntPtr srcDC, int srcWidth, int srcHeight )
		{
			MCanvas.DrawDC( _canvasDC, _canvasWidth, _canvasHeight, srcDC, srcWidth, srcHeight );
		}

		internal static void DrawDC( IntPtr dstDC, int dstWidth, int dstHeight, IntPtr srcDC, int srcWidth, int srcHeight )
		{
			if ( dstWidth == srcWidth && dstHeight == srcHeight )
			{
				GDI32.BitBlt( dstDC, 0, 0, srcWidth, srcHeight, srcDC, 0, 0, ERasterOperations.SRCCOPY );
			}
			else
			{
				GDI32.SetStretchBltMode( dstDC, EStretchMode.DELETESCANS );
				GDI32.StretchBlt( dstDC, 0, 0, dstWidth, dstHeight, srcDC, 0, 0, srcWidth, srcHeight, ERasterOperations.SRCCOPY );
			}
		}

		#endregion

		#region >>> Draw Image <<<

		public void DrawImage( Bitmap image )
		{
			this.DrawImageUnscaled( image, 0, 0 );
		}

		public void DrawImage( Bitmap image, int dstX, int dstY )
		{
			this.DrawImageUnscaled( image, dstX, dstY );
		}

		public void DrawImage( Bitmap image, int dstX, int dstY, int dstWidth, int dstHeight )
		{
			this.DrawImage( image, new Rectangle( dstX, dstY, dstWidth, dstHeight ) );
		}

		public virtual void DrawImage( Bitmap image, Rectangle dstRect )
		{
			TBITMAPINFO src_bmi;
			if ( MGraphics.InitBitmapInfo( image.PixelFormat, image.Width, image.Height, out src_bmi ) == false )
				throw new ArgumentException( "Unsupported Format", "format" );

			BitmapData bd = image.LockBits( new Rectangle( Point.Empty, image.Size ), ImageLockMode.ReadOnly, image.PixelFormat );

			MCanvas.DrawImageData( _canvasDC, ref dstRect, bd.Scan0, ref src_bmi );

			image.UnlockBits( bd );
			bd = null;
		}

		public void DrawImageUnscaled( Bitmap image )
		{
			this.DrawImageUnscaled( image, 0, 0 );
		}

		public void DrawImageUnscaled( Bitmap image, Rectangle dstRect )
		{
			this.DrawImageUnscaled( image, dstRect.X, dstRect.Y );
		}

		public virtual void DrawImageUnscaled( Bitmap image, int dstX, int dstY )
		{
			Rectangle rect = new Rectangle( dstX, dstY, _canvasHeight, _canvasHeight );
			MCanvas.DrawImageUnscaled( _canvasDC, ref rect, image );
		}

		internal static void DrawImageUnscaled( IntPtr dstDC, ref Rectangle dstRect, Bitmap srcImage )
		{
			TBITMAPINFO src_bmi;
			if ( MGraphics.InitBitmapInfo( srcImage.PixelFormat, srcImage.Width, srcImage.Height, out src_bmi ) == false )
				throw new ArgumentException( "Unsupported Format", "image" );

			BitmapData bd = srcImage.LockBits( new Rectangle( Point.Empty, srcImage.Size ), ImageLockMode.ReadOnly, srcImage.PixelFormat );

			int scan_line = Math.Min( dstRect.Height, srcImage.Height );

			scan_line = GDI32.SetDIBitsToDevice( dstDC, dstRect.X, dstRect.Y, dstRect.Width, dstRect.Height,
				0, 0, 0, scan_line,
				bd.Scan0, ref src_bmi, EDIBColor.DIB_RGB_COLORS );

			srcImage.UnlockBits( bd );

			bd = null;
		}
		#endregion

		public void SaveImage( string path )
		{

		}

		public virtual ICanvas InitializeTheme()
		{
			return null;
		}

		public bool SetTransform()
		{
			IntPtr ele = Marshal.UnsafeAddrOfPinnedArrayElement( _trsf.Elements, 0 );
			GDI32.SetGraphicsMode( _canvasDC, EGraphicsMode.GM_ADVANCED );
			return GDI32.SetWorldTransform( _canvasDC, ele );
		}

		/// <summary>
		/// Restore last transform
		/// </summary>
		public void ResetTransform()
		{
			_trsf.Reset();
			GDI32.ModifyWorldTransform( _canvasDC, IntPtr.Zero, EXformMode.MWT_IDENTITY );
			GDI32.SetGraphicsMode( _canvasDC, EGraphicsMode.GM_COMPATIBLE );
		}

		public Matrix Transform
		{
			get
			{
				return _trsf;
			}
			set
			{
				_trsf = value;
			}
		}

		private Rectangle _myRect;
		private Point[] _worldCorner;

		public void SetWorldTransform( Rectangle worldRect )
		{
			_worldCorner = new Point[3] { worldRect.Location,
				new Point( worldRect.Right, worldRect.Top ), 
				new Point( worldRect.Left, worldRect.Bottom ) };

			_trsf = new Matrix( _myRect, _worldCorner );
		}

		public void SetMineTransform( Rectangle mineRect )
		{
			_trsf = new Matrix( _myRect, _worldCorner );
		}
	}
}
