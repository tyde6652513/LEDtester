using System;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace MPI.Drawing
{
	public class MCoord : IDisposable
	{
		/*
			x' = x * M11 + y * M21 + Dx; 
			y' = x * M12 + y * M22 + Dy;

			new_x = x.cos(angle) + y.sin(angle)
			new_y = y.cos(angle) - x.sin(angle)
		 * 
		| M11 M12 0 | 
		| M21 M22 0 | 
		| Dx  Dy  1 | 

		 */
		private Matrix _trsf;
		private PointF[] _worldCorner;
		private RectangleF _myRect;
		private Matrix _oldTrsf;

		#region >>> Constructor <<<
		public MCoord( Rectangle mine, Rectangle world )
		{
			_myRect = mine;
            RectangleF rect = new RectangleF(world.Location, world.Size);
            this.setMatrix(ref rect);
		}

        public MCoord(RectangleF mine, RectangleF world)
        {
            _myRect = mine;
            this.setMatrix(ref world);
        }

		#endregion

        #region >>> Public Method <<<
        public void ChangeMyRect( ref Rectangle myRect )
		{
			_myRect = myRect;
			_trsf.Dispose();
			_trsf = null;
			_trsf = new Matrix( _myRect, _worldCorner );
		}

		public void ChangeWorldRect( ref Rectangle worldRect )
		{
			_trsf.Dispose();
			_trsf = null;
            RectangleF rect = new RectangleF(worldRect.Location, worldRect.Size);
            this.setMatrix(ref rect);
		}

		public void ChangeWorldSize( int width, int height )
		{
			_trsf.Dispose();
			_trsf = null;
			RectangleF rect = new RectangleF( 0, 0, width, height );
			this.setMatrix( ref rect );
		}

		public void ChangeWorldSize( Size size )
		{
			_trsf.Dispose();
			_trsf = null;
			RectangleF rect = new RectangleF( PointF.Empty, size );
			this.setMatrix( ref rect );
		}

		public void SetWorldScale( float scaleX, float scaleY )
		{
			float[] ele = _trsf.Elements;			//m11, m12, m21, m22, dx, dy
			ele[0] = scaleX;
			ele[3] = scaleY;

			_trsf.Dispose();
			_trsf = null;
			_trsf = new Matrix( ele[0], ele[1], ele[2], ele[3], ele[4], ele[5] );
			ele = null;
		}

		public void SetWorldScale( float scale )
		{
			SetWorldScale( scale, scale );
        }

        #endregion

        private void setMatrix( ref RectangleF worldRect )
		{
			_worldCorner = null;
			_worldCorner = new PointF[3] { worldRect.Location,
				new PointF( worldRect.Right, worldRect.Top ), 
				new PointF( worldRect.Left, worldRect.Bottom ) };

			_trsf = new Matrix( _myRect, _worldCorner );
		}

		#region >>> To Mine <<<
		public void WorldToMine( ref Rectangle rect )
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

		public Point ToMine( int worldX, int worldY )
		{
			Point[] pts = new Point[] { new Point( worldX, worldY ) };
			_trsf.Invert();
			_trsf.TransformPoints( pts );
			_trsf.Invert();

			return pts[0];
		}

        public PointF ToMine(float worldX, float worldY)
        {
            PointF[] pts = new PointF[] { new PointF(worldX, worldY) };
            _trsf.Invert();
            _trsf.TransformPoints(pts);
            _trsf.Invert();

            return pts[0];
        }

		public Point ToMine( Point worldLocation )
		{
			return ToMine( worldLocation.X, worldLocation.Y );
		}

		public void WorldToMine( ref Point worldLocation )
		{
			Point[] pts = new Point[] { worldLocation };
			_trsf.Invert();
			_trsf.TransformPoints( pts );
			_trsf.Invert();

			worldLocation = pts[0];
			pts = null;
		}

        public void WorldToMine(ref PointF worldLocation)
        {
            PointF[] pts = new PointF[] { worldLocation };
            _trsf.Invert();
            _trsf.TransformPoints(pts);
            _trsf.Invert();

            worldLocation = pts[0];
            pts = null;
        }

		#endregion

		#region >>> To World <<<

		public int ScaleToWorld( int mySize )
		{
			Point[] pts = new Point[] { new Point( mySize, 0 ) };
			_trsf.TransformVectors( pts );
			return pts[0].X;
		}

		public float ScaleToWorld( float mySize )
		{
			PointF[] pts = new PointF[] { new PointF( mySize, 0 ) };
			_trsf.TransformVectors( pts );
			return pts[0].X;
		}

		public void ScaleToWorld( ref Point myVector )
		{
			Point[] pts = new Point[] { myVector };
			_trsf.TransformVectors( pts );
			myVector.X = pts[0].X;
			myVector.Y = pts[0].Y;
			pts = null;
		}

		public void ScaleToWorld( ref Size myVector )
		{
			Point[] pts = new Point[] { new Point( myVector ) };
			_trsf.TransformVectors( pts );
			myVector.Width = pts[0].X;
			myVector.Height = pts[0].Y;
			pts = null;
		}

		public void ScaleToWorld( ref SizeF myVector )
		{
			PointF[] pts = new PointF[] { new PointF( myVector.Width, myVector.Height ) };
			_trsf.TransformVectors( pts );
			myVector.Width = pts[0].X;
			myVector.Height = pts[0].Y;
			pts = null;
		}

		public SizeF ScaleToWorld( int myWidth, int myHeight )
		{
			PointF[] pts = new PointF[] { new PointF( myWidth, myHeight ) };
			_trsf.TransformVectors( pts );
			return new SizeF( pts[0].X, pts[0].Y );
		}

		public Point ToWorld( int myX, int myY )
		{
			Point[] pts = new Point[1] { new Point( myX, myY ) };
			_trsf.TransformPoints( pts );
			return pts[0];
		}

		public PointF ToWorld( float myX, float myY )
		{
			PointF[] pts = new PointF[1] { new PointF( myX, myY ) };
			_trsf.TransformPoints( pts );
			return pts[0];
		}

		public PointF ToWorld( PointF myXY )
		{
			PointF[] pts = new PointF[1] { myXY };
			_trsf.TransformPoints( pts );
			return pts[0];
		}

		public Point ToWorld( Point myXY )
		{
			Point[] pts = new Point[1] { myXY };
			_trsf.TransformPoints( pts );
			return pts[0];
		}

		public Rectangle ToWorld( Point myXY, int myWidth, int myHeight )
		{
			Rectangle rect = new Rectangle( myXY.X, myXY.Y, myWidth, myHeight );
			this.MineToWorld( ref rect );
			return rect;
		}

		public RectangleF ToWorld( int myX, int myY, float myWidth, float myHeight )
		{
			Point[] pts = new Point[1] { new Point( myX, myY ) };
			_trsf.TransformPoints( pts );
			myX = pts[0].X;
			myY = pts[0].Y;
			pts = null;

			PointF[] ptsf = new PointF[] { new PointF( myWidth, myHeight ) };
			_trsf.TransformVectors( ptsf );

			myWidth = ptsf[0].X;
			myHeight = ptsf[0].Y;

			ptsf = null;

			return new RectangleF( myX, myY, myWidth, myHeight );
		}

		public void MineToWorld( ref Point mine )
		{
			Point[] pts = new Point[] { mine };
			_trsf.TransformPoints( pts );
			mine.X = pts[0].X;
			mine.Y = pts[0].Y;
			pts = null;
		}

		public void MineToWorld( ref PointF mine )
		{
			PointF[] pts = new PointF[] { mine };
			_trsf.TransformPoints( pts );
			mine.X = pts[0].X;
			mine.Y = pts[0].Y;
			pts = null;
		}

		public void MineToWorld( ref Rectangle mine )
		{
			Point[] pts = new Point[1] { mine.Location };
			_trsf.TransformPoints( pts );
			mine.X = pts[0].X;
			mine.Y = pts[0].Y;

			pts[0] = new Point( mine.Size );
			_trsf.TransformVectors( pts );

			mine.Size = new Size( pts[0] );
			pts = null;
		}
		#endregion

		public void ApplyTransform( Graphics g )
		{
			_oldTrsf = g.Transform;
			g.Transform = _trsf;
		}

		public void RestoreTransform( Graphics g )
		{
			g.Transform = _oldTrsf;
			_oldTrsf = null;
		}

		public void OffsetMine( int x, int y )
		{
			_trsf.Translate( x, y );
		}

		public Matrix Transform
		{
			get
			{
				return new Matrix( _myRect, _worldCorner );
			}
		}

		public void Dispose()
		{
			_trsf.Dispose();
			if ( _oldTrsf != null )
				_oldTrsf.Dispose();
		}
	}
}
