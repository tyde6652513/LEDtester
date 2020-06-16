using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace MPI.Drawing
{
	public interface IDCEnable
	{
		IntPtr GetDC();
		void ReleaseDC();
	}

	public interface ICanvas : IDisposable
	{
		ERegionError Clip( Rectangle rect );

		void ResetClip();

		void DrawCross( Pen pen, int x, int y, int size );
		void DrawImageData( IntPtr srcPtr, PixelFormat format );
		void DrawImageData( IntPtr srcPtr, int srcWidth, int srcHeight, PixelFormat format );

		void DrawDC( IntPtr srcHDC );
		void DrawDC( IntPtr srcHDC, int srcWidth, int srcHeight );

		/// <summary>
		/// Draw image with unscaling
		/// </summary>
		void DrawImage( Bitmap image );
		/// <summary>
		/// Draw image with unscaling
		/// </summary>
		void DrawImage( Bitmap image, int dstX, int dstY );

		/// <summary>
		/// Draw image with scaling
		/// </summary>
		void DrawImage( Bitmap image, Rectangle dstRect );

		/// <summary>
		/// Draw image with scaling
		/// </summary>
		void DrawImage( Bitmap image, int dstX, int dstY, int dstWidth, int dstHeight );

		/// <summary>
		/// Draw image with unscaling
		/// </summary>
		void DrawImageUnscaled( Bitmap image );
		/// <summary>
		/// Draw image with unscaling
		/// </summary>
		void DrawImageUnscaled( Bitmap image, int dstX, int dstY );
		void DrawImageUnscaled( Bitmap image, Rectangle dstRect );

		void Clear();
		void Clear( Color color );

		#region >>> Pixel <<<
		uint SetPixel( int x, int y, Color color );
		Color GetPixel( int x, int y );
		#endregion

		#region >>> Draw Rectangle <<<

		void DrawRectangle( Pen pen, Rectangle rect );

		void DrawRectangle( Pen pen, int x, int y, int width, int height );

		void DrawRectangle( Pen pen, float x, float y, float width, float height );

		void DrawRectangle( Pen pen, RectangleF rect );

		#endregion

		#region >>> Draw Line <<<

		void DrawLine( Pen pen, int x1, int y1, int x2, int y2 );

		void DrawLine( Pen pen, float x1, float y1, float x2, float y2 );

		void DrawLine( Pen pen, PointF pt1, PointF pt2 );

		void DrawLine( Pen pen, Point pt1, Point pt2 );

		void DrawLines( Pen pen, Point[] points );

		void DrawLines( Pen pen, PointF[] points );

		#endregion

		#region >>> Draw Arc <<<

		void DrawArc( Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle );

		void DrawArc( Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle );

		void DrawArc( Pen pen, RectangleF rect, float startAngle, float sweepAngle );

		void DrawArc( Pen pen, Rectangle rect, float startAngle, float sweepAngle );

		#endregion

		#region >>> Draw Bezier <<<

		void DrawBezier( Pen pen, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4 );

		void DrawBezier( Pen pen, PointF pt1, PointF pt2, PointF pt3, PointF pt4 );

		void DrawBezier( Pen pen, Point pt1, Point pt2, Point pt3, Point pt4 );

		void DrawBeziers( Pen pen, PointF[] points );

		void DrawBeziers( Pen pen, Point[] points );

		#endregion

		#region >>> Draw Ellipse <<<

		void DrawEllipse( Pen pen, RectangleF rect );

		void DrawEllipse( Pen pen, float x, float y, float width, float height );

		void DrawEllipse( Pen pen, Rectangle rect );

		void DrawEllipse( Pen pen, int x, int y, int width, int height );

		#endregion

		#region >>> Draw Pie <<<
		void DrawPie( Pen pen, RectangleF rect, float startAngle, float sweepAngle );

		void DrawPie( Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle );

		void DrawPie( Pen pen, Rectangle rect, float startAngle, float sweepAngle );

		void DrawPie( Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle );

		#endregion

		#region >>> Draw Polygon <<<

		void DrawPolygon( Pen pen, PointF[] points );

		void DrawPolygon( Pen pen, Point[] points );

		#endregion

		#region >>> Draw Curve <<<

		//		void DrawPath( Pen pen, GraphicsPath path );

		void DrawCurve( Pen pen, PointF[] points );

		void DrawCurve( Pen pen, Point[] points );

		//void DrawCurve( Pen pen, PointF[] points, float tension );

		//void DrawCurve( Pen pen, Point[] points, float tension );

		//void DrawCurve( Pen pen, PointF[] points, int offset, int numberOfSegments );

		//void DrawCurve( Pen pen, PointF[] points, int offset, int numberOfSegments, float tension );

		//void DrawCurve( Pen pen, Point[] points, int offset, int numberOfSegments, float tension );

		#endregion
		/*
		#region >>> Draw Closed Curve <<<
		void DrawClosedCurve( Pen pen, PointF[] points );

		void DrawClosedCurve( Pen pen, Point[] points );

		void DrawClosedCurve( Pen pen, PointF[] points, float tension, FillMode fillmode );

		void DrawClosedCurve( Pen pen, Point[] points, float tension, FillMode fillmode );

		#endregion
*/

		void FillEllipse( Brush brush, RectangleF rect );

		void FillEllipse( Brush brush, float x, float y, float width, float height );

		void FillEllipse( Brush brush, Rectangle rect );

		void FillEllipse( Brush brush, int x, int y, int width, int height );

		#region >>> Fill Rectangle <<<

		void FillRectangle( Brush brush, RectangleF rect );

		void FillRectangle( Brush brush, float x, float y, float width, float height );

		void FillRectangle( Brush brush, Rectangle rect );

		void FillRectangle( Brush brush, int x, int y, int width, int height );

		#endregion
		/*
		#region >>> Fill Action <<<

		void FillPolygon( Brush brush, PointF[] points );

		void FillPolygon( Brush brush, PointF[] points, FillMode fillMode );

		void FillPolygon( Brush brush, Point[] points );

		void FillPolygon( Brush brush, Point[] points, FillMode fillMode );

		void FillPie( Brush brush, Rectangle rect, float startAngle, float sweepAngle );

		void FillPie( Brush brush, float x, float y, float width, float height, float startAngle, float sweepAngle );

		void FillPie( Brush brush, int x, int y, int width, int height, int startAngle, int sweepAngle );

		void FillPath( Brush brush, GraphicsPath path );

		void FillClosedCurve( Brush brush, PointF[] points );

		void FillClosedCurve( Brush brush, PointF[] points, FillMode fillmode );

		void FillClosedCurve( Brush brush, PointF[] points, FillMode fillmode, float tension );

		void FillClosedCurve( Brush brush, Point[] points );

		void FillClosedCurve( Brush brush, Point[] points, FillMode fillmode );

		void FillClosedCurve( Brush brush, Point[] points, FillMode fillmode, float tension );

		void FillRegion( Brush brush, Region region );

		#endregion
*/
		#region >>> Draw String <<<

		void DrawString( string s, Font font, Brush brush, float x, float y );

		void DrawString( string s, Font font, Brush brush, PointF point );

		void DrawString( string s, Font font, Brush brush, float x, float y, StringFormat format );

		void DrawString( string s, Font font, Brush brush, PointF point, StringFormat format );

		void DrawString( string s, Font font, Brush brush, RectangleF layoutRectangle );

		void DrawString( string s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat format );

		void DrawString( string s, Font font, Brush brush, Rectangle rect );

		void DrawString( string s, Font font, Color color, Rectangle rect );

		void DrawString( string s, Font font, Brush brush, int x, int y );

		void DrawString( string s, Font font, Color color, int x, int y );

		#endregion

		//#region >>> Measure String <<<

		//SizeF MeasureString( string text, Font font, SizeF layoutArea, StringFormat stringFormat, out int charactersFitted, out int linesFilled );

		//SizeF MeasureString( string text, Font font, PointF origin, StringFormat stringFormat );

		//SizeF MeasureString( string text, Font font, SizeF layoutArea );

		//SizeF MeasureString( string text, Font font, SizeF layoutArea, StringFormat stringFormat );

		//SizeF MeasureString( string text, Font font );

		//SizeF MeasureString( string text, Font font, int width );

		//SizeF MeasureString( string text, Font font, int width, StringFormat format );

		//#endregion

		//ICanvas Theme
		//{
		//   get;
		//}

		bool SetTransform();

		void SetWorldTransform( Rectangle worldRect );
		void SetMineTransform( Rectangle mineRect );
		void ResetTransform();

		Matrix Transform
		{
			get;
			set;
		}
	}
}
