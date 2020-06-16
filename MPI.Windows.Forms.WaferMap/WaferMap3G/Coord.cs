using System;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace MPI.UCF.Forms.Domain
{
	internal class Coord
	{
		private Matrix _trsf;
		private Point[] _scrCorner;
		private Rectangle _waferRect;
		private Matrix _oldTrsf;

		internal Coord( Rectangle waferRect, Rectangle screenRect )
		{
			_waferRect = waferRect;
			this.setMatrix( ref screenRect );
		}

		internal void ChangeWaferRect( ref Rectangle waferRect )
		{
			_waferRect = waferRect;
			_trsf = null;
			_trsf = new Matrix( _waferRect, _scrCorner );
		}

		internal void ChangeScreenRect( ref Rectangle dispRect )
		{
			this.setMatrix( ref dispRect );
		}

		internal void ChangeScreenSize( Size size )
		{
			Rectangle rect = new Rectangle( Point.Empty, size );
			this.setMatrix( ref rect );
		}

		private void setMatrix( ref Rectangle dispRect )
		{
			_scrCorner = null;
			_scrCorner = new Point[3] { dispRect.Location,
				new Point( dispRect.Right, dispRect.Top ), 
				new Point( dispRect.Left, dispRect.Bottom ) };

			_trsf = new Matrix( _waferRect, _scrCorner );
		}

		#region >>> To Wafer <<<
		internal void ScreenToWafer( ref Rectangle rect )
		{
			Point[] pts = new Point[] { rect.Location };
			_trsf.Invert();
			_trsf.TransformPoints( pts );
			rect.Location = pts[0];
			pts = null;
			pts = new Point[] { new Point( rect.Size ) };
			_trsf.TransformVectors( pts );
			rect.Size = new Size( pts[0] );
			_trsf.Invert();
		}

		internal Point ToWafer( int x, int y )
		{
			Point[] pts = new Point[] { new Point( x, y ) };
			_trsf.Invert();
			_trsf.TransformPoints( pts );
			_trsf.Invert();

			return pts[0];
		}

		internal Point ToWafer( Point location )
		{
			return ToWafer( location.X, location.Y );
		}

		internal void ScreenToWafer( ref Point location )
		{
			Point[] pts = new Point[] { location };
			_trsf.Invert();
			_trsf.TransformPoints( pts );
			_trsf.Invert();

			location = pts[0];
			pts = null;
		}

		#endregion

		#region >>> To Screen <<<

		internal int ScaleToScreen( int chipSize )
		{
			Point[] pts = new Point[] { new Point( chipSize, 0 ) };
			_trsf.TransformVectors( pts );
			return pts[0].X;
		}

		internal void ScaleToScreen( ref Point vector )
		{
			Point[] pts = new Point[] { vector };
			_trsf.TransformVectors( pts );
			vector.X = pts[0].X;
			vector.Y = pts[0].Y;
			pts = null;
		}

		internal void ScaleToScreen( ref Size vector )
		{
			Point[] pts = new Point[] { new Point( vector ) };
			_trsf.TransformVectors( pts );
			vector.Width = pts[0].X;
			vector.Height = pts[0].Y;
			pts = null;
		}

		internal Point ToScreen( int col, int row )
		{
			Point[] pts = new Point[] { new Point( col, row ) };
			_trsf.TransformPoints( pts );
			return pts[0];
		}

		internal Rectangle ToScreen( Point location, int colWidth, int rowHeight )
		{
			Rectangle rect = new Rectangle( location.X, location.Y, colWidth, rowHeight );
			this.WaferToScreen( ref rect );
			return rect;
		}

		internal RectangleF ToScreen( int col, int row, float colWidth, float rowHeight )
		{
			Point[] pts = new Point[1] { new Point( col, row ) };
			_trsf.TransformPoints( pts );
			col = pts[0].X;
			row = pts[0].Y;
			pts = null;

			PointF[] ptsf = new PointF[] { new PointF( colWidth, rowHeight ) };
			_trsf.TransformVectors( ptsf );

			colWidth = ptsf[0].X;
			rowHeight = ptsf[0].Y;

			ptsf = null;

			return new RectangleF( col, row, colWidth, rowHeight );
		}

		internal void WaferToScreen( ref Point location )
		{
			Point[] pts = new Point[] { location };
			_trsf.TransformPoints( pts );
			location.X = pts[0].X;
			location.Y = pts[0].Y;
			pts = null;
		}

		internal void WaferToScreen( ref Rectangle rect )
		{
			Point[] pts = new Point[1] { rect.Location };
			_trsf.TransformPoints( pts );
			rect.X = pts[0].X;
			rect.Y = pts[0].Y;

			pts[0] = new Point( rect.Size );
			_trsf.TransformVectors( pts );

			rect.Size = new Size( pts[0] );
			pts = null;
		}
		#endregion

		internal void ApplyTransform( Graphics g )
		{
			_oldTrsf = g.Transform;
			g.Transform = _trsf;
		}

		internal void RestoreTransform( Graphics g )
		{
			g.Transform = _oldTrsf;
			_oldTrsf = null;
		}
	}
}
