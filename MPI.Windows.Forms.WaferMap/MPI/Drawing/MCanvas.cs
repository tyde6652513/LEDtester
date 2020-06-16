using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace MPI.Drawing
{
	public class MCanvas : ICanvas
	{
		private IntPtr _oldPen;
		private IntPtr _oldFont;

		protected MMatrix _trsf;

		protected IntPtr _drawDC;
		protected IntPtr _oldBMP;

		/// <summary>
		/// Default: Black color ( 0 )
		/// </summary>
		protected uint _background;
		protected int _dcWidth;
		protected int _dcHeight;

		private LOGFONT _logFont;
		private LOGPEN _logPen;

		protected MCanvas()
		{
			_logFont = new LOGFONT( true );
			_logPen = new LOGPEN( true );
			_trsf = new MMatrix();
		}

		public MCanvas( IntPtr hdc, int width, int height, uint bgColor )
			: this()
		{
			_drawDC = hdc;
			_dcWidth = width;
			_dcHeight = height;

			_background = bgColor;


			// Set the mapping mode to LOENGLISH. This moves the  
			// client area origin from the upper left corner of the  
			// window to the lower left corner (this also reorients  
			// the y-axis so that drawing operations occur in a true  
			// Cartesian space). It guarantees portability so that  
			// the object drawn retains its dimensions on any display.  

			//GDI32.SetMapMode( hDC, MM_LOENGLISH ); 
		}

		public MCanvas( IntPtr hdc, IntPtr bmp, int width, int height, uint bgColor )
			: this( hdc, width, height, bgColor )
		{
			_oldBMP = bmp;
		}

		public MCanvas( IntPtr hdc, int width, int height )
			: this( hdc, width, height, 0 )
		{

		}

		public MCanvas( IntPtr hdc )
		{
			_drawDC = hdc;
			_dcWidth = GDI32.GetDeviceCaps( hdc, EDeviceCap.HORZRES );
			_dcHeight = GDI32.GetDeviceCaps( hdc, EDeviceCap.VERTRES );
		}

		public virtual void Dispose()
		{

		}

		public virtual ERegionError Clip( Rectangle rect )
		{
			return GDI32.SelectClipRgn( _drawDC, GDI32.CreateRectRgn( rect.Left, rect.Top, rect.Right, rect.Bottom ) );
		}

		public virtual void ResetClip()
		{
			GDI32.SelectClipRgn( _drawDC, IntPtr.Zero );
		}

		#region >>> Clear <<<

		public void Clear()
		{
			this.Clear( _background );
		}

		public void Clear( Color color )
		{
			this.Clear( ( uint ) ColorTranslator.ToWin32( color ) );
		}

		public virtual void Clear( uint color )
		{
			MGraphics.Clear2( _drawDC, _background, _dcWidth, _dcHeight );
		}
		#endregion

		public virtual void DrawCross( Pen pen, int x, int y, int size )
		{
			this.SetPen( _drawDC, pen );

			GDI32.MoveToEx( _drawDC, x - size, y - size, IntPtr.Zero );
			GDI32.LineTo( _drawDC, x + size, y + size );

			GDI32.MoveToEx( _drawDC, x - size, y + size, IntPtr.Zero );
			GDI32.LineTo( _drawDC, x + size, y - size );

			this.ResetPen( _drawDC );
		}

		public virtual void DrawCrossLine( Color color )
		{
			IntPtr old = GDI32.SelectObject( _drawDC, GDI32.GetStockObject( EStockObject.DC_PEN ) );
			GDI32.SelectObject( _drawDC, GDI32.GetStockObject( EStockObject.NULL_BRUSH ) );
			GDI32.SetDCPenColor( _drawDC, ( uint ) ColorTranslator.ToWin32( color ) );

			int half_h = _dcHeight >> 1;
			int half_w = _dcWidth >> 1;
			MGraphics.DrawLine( _drawDC, 0, half_h, _dcWidth, half_h );
			MGraphics.DrawLine( _drawDC, half_w, 0, half_w, _dcHeight );

			GDI32.SelectObject( _drawDC, old );
		}

		#region >>> Pixel <<<
		public virtual uint SetPixel( int x, int y, Color color )
		{
			return GDI32.SetPixel( _drawDC, x, y, ( uint ) ColorTranslator.ToWin32( color ) );
		}

		public virtual Color GetPixel( int x, int y )
		{
			return ColorTranslator.FromWin32( ( int ) GDI32.GetPixel( _drawDC, x, y ) );
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
			this.SetPen( _drawDC, pen );
			GDI32.SelectObject( _drawDC, GDI32.GetStockObject( EStockObject.NULL_BRUSH ) );
			GDI32.Rectangle( _drawDC, x, y, ( x + width + 1 ), ( y + height + 1 ) );
			this.ResetPen( _drawDC );
		}

		public void DrawRectangle( Color drawColor, int x, int y, int width, int height )
		{
			IntPtr old = GDI32.SelectObject( _drawDC, GDI32.GetStockObject( EStockObject.DC_PEN ) );
			GDI32.SelectObject( _drawDC, GDI32.GetStockObject( EStockObject.NULL_BRUSH ) );
			GDI32.SetDCPenColor( _drawDC, ( uint ) ColorTranslator.ToWin32( drawColor ) );
			GDI32.Rectangle( _drawDC, x, y, ( x + width + 1 ), ( y + height + 1 ) );
			GDI32.SelectObject( _drawDC, old );
		}

		public void DrawRectangle( Color drawColor, Rectangle rect )
		{
			IntPtr old = GDI32.SelectObject( _drawDC, GDI32.GetStockObject( EStockObject.DC_PEN ) );
			GDI32.SelectObject( _drawDC, GDI32.GetStockObject( EStockObject.NULL_BRUSH ) );
			GDI32.SetDCPenColor( _drawDC, ( uint ) ColorTranslator.ToWin32( drawColor ) );
			GDI32.Rectangle( _drawDC, rect.Left, rect.Top, rect.Right, rect.Bottom );
			GDI32.SelectObject( _drawDC, old );
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
			this.SetPen( _drawDC, pen );
			MGraphics.DrawLine( _drawDC, x1, y1, x2, y2 );
			this.ResetPen( _drawDC );
		}

		public void DrawLine( Color color, int x1, int y1, int x2, int y2 )
		{
			IntPtr old = GDI32.SelectObject( _drawDC, GDI32.GetStockObject( EStockObject.DC_PEN ) );
			GDI32.SelectObject( _drawDC, GDI32.GetStockObject( EStockObject.NULL_BRUSH ) );
			GDI32.SetDCPenColor( _drawDC, ( uint ) ColorTranslator.ToWin32( color ) );
			MGraphics.DrawLine( _drawDC, x1, y1, x2, y2 );
			GDI32.SelectObject( _drawDC, old );
		}

		public virtual void DrawLines( Pen pen, Point[] points )
		{
			if ( points != null )
			{
				this.SetPen( _drawDC, pen );
				GDI32.Polyline( _drawDC, Marshal.UnsafeAddrOfPinnedArrayElement( points, 0 ), points.Length );
				this.ResetPen( _drawDC );
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
			this.SetPen( _drawDC, pen );

			float w = rect.Width / 2;
			float h = rect.Height / 2;
			float x0 = rect.X + w;
			float y0 = rect.Y + h;

			int left2 = ( int ) ( x0 + w * ( float ) Math.Cos( sweepAngle * Math.PI / 180 ) );
			int right2 = ( int ) ( x0 + w * ( float ) Math.Cos( startAngle * Math.PI / 180 ) );
			int top2 = ( int ) ( y0 + h * ( float ) Math.Sin( sweepAngle * Math.PI / 180 ) );
			int bottom2 = ( int ) ( y0 + h * ( float ) Math.Sin( startAngle * Math.PI / 180 ) );

			GDI32.Arc( _drawDC, ( int ) rect.Left, ( int ) rect.Top, ( int ) rect.Right, ( int ) rect.Bottom,
				left2, top2, right2, bottom2 );

			this.ResetPen( _drawDC );
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
			this.SetPen( _drawDC, pen );
			MCanvas.DrawBeziers( _drawDC, points );
			this.ResetPen( _drawDC );
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
			this.SetPen( _drawDC, pen );
			GDI32.SelectObject( _drawDC, GDI32.GetStockObject( EStockObject.NULL_BRUSH ) );
			GDI32.Ellipse( _drawDC, x, y, ( x + width + 1 ), ( y + height + 1 ) );
			this.ResetPen( _drawDC );
		}

		public void DrawEllipse( Color color, Rectangle rect )
		{
			IntPtr old = GDI32.SelectObject( _drawDC, GDI32.GetStockObject( EStockObject.DC_PEN ) );
			GDI32.SelectObject( _drawDC, GDI32.GetStockObject( EStockObject.NULL_BRUSH ) );
			GDI32.SetDCPenColor( _drawDC, ( uint ) ColorTranslator.ToWin32( color ) );
			GDI32.Ellipse( _drawDC, rect.Left, rect.Top, rect.Right, rect.Bottom );
			GDI32.SelectObject( _drawDC, old );
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
			this.SetPen( _drawDC, pen );

			float w = rect.Width / 2;
			float h = rect.Height / 2;
			float x0 = rect.X + w;
			float y0 = rect.Y + h;

			int left2 = ( int ) ( x0 + w * ( float ) Math.Cos( sweepAngle * Math.PI / 180 ) );
			int right2 = ( int ) ( x0 + w * ( float ) Math.Cos( startAngle * Math.PI / 180 ) );

			int top2 = ( int ) ( y0 + h * ( float ) Math.Sin( sweepAngle * Math.PI / 180 ) );
			int bottom2 = ( int ) ( y0 + h * ( float ) Math.Sin( startAngle * Math.PI / 180 ) );

			GDI32.SelectObject( _drawDC, GDI32.GetStockObject( EStockObject.NULL_BRUSH ) );

			//$RIC, BUG
			bool ok = GDI32.Pie( _drawDC, ( int ) rect.Left, ( int ) rect.Top, ( int ) rect.Right, ( int ) rect.Bottom,
				left2, top2, right2, bottom2 );

			this.ResetPen( _drawDC );
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
			this.SetPen( _drawDC, pen );

			IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement( points, 0 );

			//IntPtr old = GDI32.SelectObject( _canvasDC, GDI32.GetStockObject( EStockObject.NULL_BRUSH ) );

			GDI32.Polygon( _drawDC, ptr, points.Length );

			//GDI32.SelectObject( _canvasDC, old );

			this.ResetPen( _drawDC );
		}


		#endregion

		#region >>> Draw Curve ( Not implemented ) <<<

		public void DrawPath( Pen pen, GraphicsPath path )
		{
			throw new NotImplementedException();
		}

		public void DrawCurve( Pen pen, PointF[] points )
		{
			throw new NotImplementedException();
		}

		public void DrawCurve( Pen pen, Point[] points )
		{
			throw new NotImplementedException();
		}

		public void DrawCurve( Pen pen, PointF[] points, float tension )
		{
			throw new NotImplementedException();
		}

		public void DrawCurve( Pen pen, Point[] points, float tension )
		{
			throw new NotImplementedException();
		}

		public void DrawCurve( Pen pen, PointF[] points, int offset, int numberOfSegments )
		{
			this.DrawCurve( pen, points, offset, numberOfSegments, 0.5f );
		}

		public void DrawCurve( Pen pen, PointF[] points, int offset, int numberOfSegments, float tension )
		{
			throw new NotImplementedException();
		}

		public void DrawCurve( Pen pen, Point[] points, int offset, int numberOfSegments, float tension )
		{
			throw new NotImplementedException();
		}

		#endregion

		#region >>> Draw Closed Curve ( Not Implemented ) <<<
		public void DrawClosedCurve( Pen pen, PointF[] points )
		{
			throw new NotImplementedException();
		}

		public void DrawClosedCurve( Pen pen, Point[] points )
		{
			throw new NotImplementedException();
		}

		public void DrawClosedCurve( Pen pen, PointF[] points, float tension, FillMode fillmode )
		{
			throw new NotImplementedException();
		}

		public void DrawClosedCurve( Pen pen, Point[] points, float tension, FillMode fillmode )
		{
			throw new NotImplementedException();
		}

		#endregion

		#region >>> Fill Ellipse <<<

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
			IntPtr old = GDI32.SelectObject( _drawDC, GDI32.GetStockObject( EStockObject.DC_BRUSH ) );
			GDI32.SetDCBrushColor( _drawDC, MCanvas.GetBrushColor( brush ) );
			GDI32.Ellipse( _drawDC, x, y, ( x + width + 1 ), ( y + height + 1 ) );
			GDI32.SelectObject( _drawDC, old );
		}

		public void FillEllipse( Color backColor, Rectangle rect )
		{
			IntPtr old = GDI32.SelectObject( _drawDC, GDI32.GetStockObject( EStockObject.DC_BRUSH ) );
			GDI32.SetDCBrushColor( _drawDC, ( uint ) ColorTranslator.ToWin32( backColor ) );
			GDI32.Ellipse( _drawDC, rect.Left, rect.Top, rect.Right, rect.Bottom );
			GDI32.SelectObject( _drawDC, old );
		}

		#endregion

		#region >>> Fill Rectangle <<<

		public void FillRectangle( Brush brush, RectangleF rect )
		{
			this.FillRectangle( MCanvas.GetBrushColor( brush ),
				( int ) rect.Left, ( int ) rect.Top, ( int ) rect.Right, ( int ) rect.Bottom );
		}

		public void FillRectangle( Brush brush, float x, float y, float width, float height )
		{
			this.FillRectangle( MCanvas.GetBrushColor( brush ),
				( int ) x, ( int ) y, ( int ) ( x + width ), ( int ) ( y + height ) );
		}

		public void FillRectangle( Brush brush, Rectangle rect )
		{
			this.FillRectangle( MCanvas.GetBrushColor( brush ),
				rect.Left, rect.Top, rect.Right, rect.Bottom );
		}

		public virtual void FillRectangle( Brush brush, int x, int y, int width, int height )
		{
			this.FillRectangle( MCanvas.GetBrushColor( brush ),
				x, y, x + width, y + height );
		}

		public virtual void FillRectangle( Color fillColor, Rectangle rect )
		{
			this.FillRectangle( ( uint ) ColorTranslator.ToWin32( fillColor ),
				rect.Left, rect.Top, rect.Right, rect.Bottom );
		}

		public virtual void FillRectangle( Color fillColor, int x, int y, int width, int height )
		{
			this.FillRectangle( ( uint ) ColorTranslator.ToWin32( fillColor ),
				x, y, x + width, y + height );
		}

		public virtual void FillRectangle( uint fillColor, int left, int top, int right, int bottom )
		{
			IntPtr old = GDI32.SelectObject( _drawDC, GDI32.GetStockObject( EStockObject.DC_BRUSH ) );
			GDI32.SetDCBrushColor( _drawDC, fillColor );
			//GDI32.Rectangle( _drawDC, ( x - 1 ), ( y - 1 ), ( x + width + 1 ), ( y + height + 1 ) );
			GDI32.Rectangle( _drawDC, left, top, right, bottom );
			GDI32.SelectObject( _drawDC, old );
		}

		#endregion

		#region >>> Fill Polygon ( Not Implemented ) <<<

		public void FillPolygon( Brush brush, PointF[] points )
		{
			this.FillPolygon( brush, points, FillMode.Alternate );
		}

		public void FillPolygon( Brush brush, PointF[] points, FillMode fillMode )
		{
			throw new NotImplementedException();
		}

		public void FillPolygon( Brush brush, Point[] points )
		{
			this.FillPolygon( brush, points, FillMode.Alternate );
		}

		public void FillPolygon( Brush brush, Point[] points, FillMode fillMode )
		{
			throw new NotImplementedException();
		}

		#endregion

		#region >>> Fill Pie ( Note Implemented ) <<<

		public void FillPie( Brush brush, Rectangle rect, float startAngle, float sweepAngle )
		{
			this.FillPie( brush, ( float ) rect.X, ( float ) rect.Y, ( float ) rect.Width, ( float ) rect.Height, startAngle, sweepAngle );
		}

		public void FillPie( Brush brush, float x, float y, float width, float height, float startAngle, float sweepAngle )
		{
			throw new NotImplementedException();
		}

		public void FillPie( Brush brush, int x, int y, int width, int height, int startAngle, int sweepAngle )
		{
			throw new NotImplementedException();
		}

		#endregion

		public void FillPath( Brush brush, GraphicsPath path )
		{
			throw new NotImplementedException();
		}

		public void FillRegion( Brush brush, Region region )
		{
			throw new NotImplementedException();
		}

		#region >>> Fill Coled Curve ( Note Implemented ) <<<
		public void FillClosedCurve( Brush brush, PointF[] points )
		{
			throw new NotImplementedException();
		}

		public void FillClosedCurve( Brush brush, PointF[] points, FillMode fillmode )
		{
			this.FillClosedCurve( brush, points, fillmode, 0.5f );
		}

		public void FillClosedCurve( Brush brush, PointF[] points, FillMode fillmode, float tension )
		{
			throw new NotImplementedException();
		}

		public void FillClosedCurve( Brush brush, Point[] points )
		{
			throw new NotImplementedException();
		}

		public void FillClosedCurve( Brush brush, Point[] points, FillMode fillmode )
		{
			this.FillClosedCurve( brush, points, fillmode, 0.5f );
		}

		public void FillClosedCurve( Brush brush, Point[] points, FillMode fillmode, float tension )
		{
			throw new NotImplementedException();
		}
		#endregion

		#region >>> Draw String without rectangle <<<

		public void DrawString( string s, Font font, Brush brush, PointF point )
		{
			Rectangle rect = new Rectangle( ( int ) point.X, ( int ) point.Y, 0, 0 );
			MGraphics.DrawString( _drawDC, s, font, MCanvas.GetBrushColor( brush ), ref rect, EDrawTextFormat.DT_NULL );
		}

		public void DrawString( string s, Font font, Brush brush, int x, int y )
		{
			Rectangle rect = new Rectangle( x, y, 0, 0 );
			MGraphics.DrawString( _drawDC, s, font, MCanvas.GetBrushColor( brush ), ref rect, EDrawTextFormat.DT_NULL );
		}

		public void DrawString( string s, Font font, Color color, int x, int y )
		{
			Rectangle rect = new Rectangle( x, y, 0, 0 );
			MGraphics.DrawString( _drawDC, s, font, ( uint ) ColorTranslator.ToWin32( color ), ref rect, EDrawTextFormat.DT_NULL );
		}

		#endregion

		#region >>> Draw String with rectangle <<<

		public void DrawString( string text, Font font, Brush brush, Rectangle rect )
		{
			MGraphics.DrawString( _drawDC, text, font, MCanvas.GetBrushColor( brush ), ref rect, EDrawTextFormat.DT_DEFAULT );
		}

		public void DrawString( string text, Font font, Color color, Rectangle rect )
		{
			MGraphics.DrawString( _drawDC, text, font, ( uint ) ColorTranslator.ToWin32( color ), ref rect, EDrawTextFormat.DT_DEFAULT );
		}

		public void DrawString( string s, Font font, Brush brush, int x, int y, StringFormat format )
		{
			Rectangle rect = new Rectangle( x, y, 0, 0 );
			this.DrawString( s, font, brush, rect, format );
		}

		public void DrawString( string s, Font font, Brush brush, Point point, StringFormat format )
		{
			Rectangle rect = new Rectangle( point, Size.Empty );
			this.DrawString( s, font, brush, rect, format );
		}

		public void DrawString( string s, Font font, Brush brush, Rectangle layoutRectangle, StringFormat format )
		{
			this.DrawString( s, font, MCanvas.GetBrushColor( brush ), ref layoutRectangle, format );
		}

		protected virtual void DrawString( string s, Font font, uint color, ref Rectangle rect, StringFormat format )
		{
			EDrawTextFormat fmt = EDrawTextFormat.DT_DEFAULT;
			if ( format != null )
			{
				fmt = fmt | ( EDrawTextFormat ) ( ( uint ) format.Alignment | ( uint ) EDrawTextFormat.DT_VCENTER ); //v-align, 0x00(0x00), 0x04(0x01), 0x08(0x02)
				fmt = fmt | ( EDrawTextFormat ) format.LineAlignment;
			}

			MGraphics.DrawString( _drawDC, s, font, color, ref rect, fmt );
		}

		#region >>> Measurement <<<
		public void MeasureString( string s, out Size size )
		{
			GDI32.GetTextExtentPoint( _drawDC, s, s.Length, out size );
		}

		public void MeasureString( string s, Font font, out Size size )
		{
			this.SetFont( _drawDC, font );
			GDI32.GetTextExtentPoint( _drawDC, s, s.Length, out size );
			this.ResetFont( _drawDC );
		}

		#endregion

		#region >>> Not Implemented <<<

		public void DrawString( string s, Font font, Brush brush, float x, float y )
		{
			Rectangle rect = new Rectangle( ( int ) x, ( int ) y, 0, 0 );
			MGraphics.DrawString( _drawDC, s, font, MCanvas.GetBrushColor( brush ), ref rect, EDrawTextFormat.DT_NULL );
		}

		public void DrawString( string s, Font font, Brush brush, float x, float y, StringFormat format )
		{
			throw new NotImplementedException();
		}

		public void DrawString( string s, Font font, Brush brush, PointF point, StringFormat format )
		{
			throw new NotImplementedException();
		}

		public void DrawString( string s, Font font, Brush brush, RectangleF layoutRectangle )
		{
			throw new NotImplementedException();
		}

		public void DrawString( string s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat format )
		{
			throw new NotImplementedException();
		}

		#endregion

		#endregion

		internal static uint GetBrushColor( Brush brush )
		{
			SolidBrush sb = brush as SolidBrush;
			uint color = 0;

			if ( sb != null )
			{
				color = ( uint ) ColorTranslator.ToWin32( sb.Color );
			}
			else
			{
				Pen pen = new Pen( brush );
				color = ( uint ) ColorTranslator.ToWin32( pen.Color );
				pen.Dispose();
				pen = null;
			}

			return color;
		}

		internal static void SetBrushColor( IntPtr hdc, Color color )
		{
			GDI32.SetDCBrushColor( hdc, ( uint ) ColorTranslator.ToWin32( color ) );
		}

		internal void SetPen( IntPtr hdc, Pen pen )
		{
			_oldPen = MGraphics.SetPen( hdc, pen );
		}

		internal void ResetPen( IntPtr hdc )
		{
			GDI32.DeleteObject( GDI32.SelectObject( hdc, _oldPen ) );
		}

		internal void SetFont( IntPtr hdc, Font font )
		{
			MGraphics.SetFont( ref _logFont, font );
			_oldFont = GDI32.SelectObject( hdc, GDI32.CreateFontIndirect( ref _logFont ) );
		}

		internal void ResetFont( IntPtr hdc )
		{
			MGraphics.ResetFont( hdc, _oldFont );
		}

		#region >>> Draw Image Data <<<

		public void DrawImageData( IntPtr srcPtr, PixelFormat format )
		{
			this.DrawImageData( srcPtr, _dcWidth, _dcHeight, format );
		}

		public virtual void DrawImageData( IntPtr srcPtr, int srcWidth, int srcHeight, PixelFormat format )
		{
			TBITMAPINFO src_bmi;
			if ( MGraphics.InitBitmapInfo( format, srcWidth, srcHeight, out src_bmi ) == false )
				throw new ArgumentException( "Unsupported Format", "format" );

			Rectangle rect = new Rectangle( 0, 0, _dcWidth, _dcHeight );
			MCanvas.DrawImageData( _drawDC, ref rect, srcPtr, ref src_bmi );
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
			this.DrawDC( srcDC, _dcWidth, _dcHeight );
		}

		public virtual void DrawDC( IntPtr srcDC, int srcWidth, int srcHeight )
		{
			MCanvas.DrawDC( _drawDC, _dcWidth, _dcHeight, srcDC, srcWidth, srcHeight );
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

			MCanvas.DrawImageData( _drawDC, ref dstRect, bd.Scan0, ref src_bmi );

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
			Rectangle rect = new Rectangle( dstX, dstY, _dcHeight, _dcHeight );
			MGraphics.DrawImageUnscaled( _drawDC, ref rect, image );
		}

		#endregion

		public void SaveImage( string path )
		{
			throw new NotImplementedException();
		}

		public virtual ICanvas InitializeTheme()
		{
			return null;
		}

		public bool SetTransform()
		{
			IntPtr ele = Marshal.UnsafeAddrOfPinnedArrayElement( _trsf.GetElements(), 0 );
			GDI32.SetGraphicsMode( _drawDC, EGraphicsMode.GM_ADVANCED );
			return GDI32.SetWorldTransform( _drawDC, ele );
		}

		/// <summary>
		/// Restore last transform
		/// </summary>
		public void ResetTransform()
		{
			_trsf.Reset();
			GDI32.ModifyWorldTransform( _drawDC, IntPtr.Zero, EXformMode.MWT_IDENTITY );
			GDI32.SetGraphicsMode( _drawDC, EGraphicsMode.GM_COMPATIBLE );
		}

		public Matrix Transform
		{
			get
			{
				return _trsf.ToTransform();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		private Rectangle _mineRect;
		private Rectangle _worldRect;

		public void SetWorldTransform( Rectangle world )
		{
			_trsf = new MMatrix( _mineRect, world );
			_worldRect = world;
		}

		public void SetMineTransform( Rectangle mine )
		{
			_trsf = new MMatrix( _mineRect, _worldRect );
			_mineRect = mine;
		}
	}
}
