using System;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MPI.Drawing
{
    /// <summary>
    /// TXFORM
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MMatrix
    {
        public bool testMode;
        public float M11, M12, M21, M22, Dx, Dy;

        public float iM11, iM12, iM21, iM22;

        /*
            x' = cos(q) * (x1-x0) - sin(q) * (y1-y0) + x0;
            x' = cos(q) * x1 - cos(q) * x0 - sin(q) * y1 + sin(q) * y0 + x0;
            x' = x1 * cos(q) - y1 * sin(q) + x0 - cos(q)*x0 + sin(q)*y0;
	 
            y' = sin(q) * (x1-x0) + cos(q) * (y1-y0) + y0; 
            y' = sin(q) * x1 - sin(q) * x0 + cos(q) * y1 - cos(q)*y0 + y0;
            y' = x1 * sin(q) + y1 * cos(q) + y0 - cos(q)*y0 - sin(q)*x0; 	 

            M11 = cos(q);
            M12 = sin(q);
            M21 = -sin(q);
            M22 = cos(q);
            Dx = x0 – cos(q) * x0 + sin(q) * y0;
            Dy = y0 – cos(q) * y0 - sin(q) * x0;

                         | M11 M12 0 | 
            |x, y, 1| | M21 M22 0 | = |x*M11 + y*M21 + Dx, x*M12 + y*M22 + Dy, 1|
                         | Dx  Dy  1 |  
		
            x' = x * M11 + y * M21 + Dx; 
            y' = x * M12 + y * M22 + Dy;
	 
         */
        internal MMatrix(int a)
        {
            M11 = 0.0f;
            M12 = 0.0f;
            M21 = 0.0f;
            M22 = 0.0f;

            Dx = 0.0f;
            Dy = 0.0f;

            iM11 = 0.0f;
            iM12 = 0.0f;
            iM21 = 0.0f;
            iM22 = 0.0f;
            testMode = true;
        }

        internal MMatrix(ref RectangleF mine, ref RectangleF world)
            : this(1)
        {
            M11 = (world.Width / mine.Width); //scale H
            M12 = 0.0f;
            M21 = 0.0f;
            M22 = (world.Height / mine.Height); //scale V

            float wx = (world.Left + world.Right) / 2;
            float mx = (mine.Left + mine.Right) / 2;
            float wy = (world.Top + world.Bottom) / 2;
            float my = (mine.Top + mine.Bottom) / 2;

            if (false)
            {
                Dx = wx - (M11 * mx);
                Dy = wy - (M22 * my);

                //Dy = wy - world.Height ;
                //if (M22 > 0)
                //{
                //    Dy = wy + (M22 * my);
                //}
            }
            else
            {
                Dx = world.Left - (M11 * mine.Left); // offset X
                Dy = world.Top - (M22 * mine.Top);  // offset Y
            }
            ComputeInverse();
        }

        internal MMatrix(ref Rectangle mine, ref RectangleF world)
            : this(1)
        {
            M11 = (world.Width / (float)mine.Width); //scale H
            M12 = 0f;
            M21 = 0f;
            M22 = (world.Height / (float)mine.Height); //scale V

            Dx = world.Left - (M11 * mine.Left); // offset X
            Dy = world.Top - (M22 * mine.Top);  // offset Y
            ComputeInverse();
        }

        internal MMatrix(ref Rectangle mine, ref Rectangle world)
            : this(1)
        {
            M11 = ((float)world.Width / (float)mine.Width); //scale H
            M12 = 0f;
            M21 = 0f;
            M22 = ((float)world.Height / (float)mine.Height); //scale V

            Dx = world.Left - (M11 * mine.Left); // offset X
            Dy = world.Top - (M22 * mine.Top);  // offset Y
            ComputeInverse();
        }

        public MMatrix(Rectangle mine, Rectangle world)
            : this(ref mine, ref world)
        {

        }

        public MMatrix(RectangleF mine, RectangleF world)
            : this(ref mine, ref world)
        {

        }

        public MMatrix(bool dummy)
            : this(1)
        {
            M11 = 1.0f;
            M12 = 0.0f;
            M21 = 0.0f;
            M22 = 1.0f;
            Dx = 0.0f;
            Dy = 0.0f;
            ComputeInverse();
        }

        public MMatrix(System.Drawing.Drawing2D.Matrix matrix)
            : this(1)
        {
            float[] ele = matrix.Elements;
            M11 = ele[0];
            M12 = ele[1];
            M21 = ele[2];
            M22 = ele[3];
            Dx = ele[4];
            Dy = ele[5];
            ComputeInverse();
        }

        #region >>> To World Coord <<<

        public void ToWorld(ref float x, ref float y)
        {
            float tx = x * M11 + y * M21 + Dx;
            float ty = x * M12 + y * M22 + Dy;
            x = tx;
            y = ty;
            //x = x * M11 + y * M21 + Dx;
            //y = x * M12 + y * M22 + Dy;
            //y = y * M12 + y * M22 + Dy;
        }

        public void ToWorld(ref PointF position)
        {
            float x = position.X;
            float y = position.Y;

            position = new PointF(x * M11 + y * M21 + Dx,
            y * M12 + y * M22 + Dy);
        }

        public void ToWorldSize(ref float width, ref float height)
        {
            float x = width * M11 + height * M12;
            float y = width * M21 + height * M22;
            //width *= M11;
            //height *= M22;
            width = x;
            height = y;
        }

        public void SetWorldScale(float scaleX, float scaleY)
        {
            M11 = scaleX;
            M22 = scaleY;
        }

        public SizeF WorldScale
        {
            get
            {
                return new SizeF(M11, M22);
            }
        }

        public void OffsetWorld(float x, float y) //world ?
        {
            Dx += x;
            Dy += y;
        }

        #endregion

        #region >>> To Mine Coord <<<

        public void OffsetMine(float x, float y)
        {
            Dx += x * M11 + y * M21;
            Dy += x * M12 + y * M22;
            //Dx += x * M11 + y * M21 + Dx;
            //Dy += x * M12 + y * M22 + Dy;
            //Dy += y * M12 + y * M22 + Dy;
        }

        public void ToMine(ref float x, ref float y)
        {
            //x = x / M11 + y * M21 - Dx / M11;
            //y = y * M12 + y / M22 - Dy / M22;

            float tx = x;
            float ty = y;
            x = (tx - Dx) * iM11 + ty * iM12;
            y = (ty - Dy) * iM22 + tx * iM21;
        }

        public void ToMine(ref int x, ref int y)
        {
            //x = ( int ) ( x / M11 + y * M21 - Dx / M11 );
            //y = ( int ) ( y * M12 + y / M22 - Dy / M22 );
            float tx = x;
            float ty = y;
            x = (int)((tx - Dx) * iM11 + ty * iM12);
            y = (int)((ty - Dy) * iM22 + tx * iM21);
        }

        public void ToMine(ref PointF point)
        {
            float x = point.X;
            float y = point.Y;

            ToMine(ref x, ref y);

            point.X = x;
            point.Y = y;

            //point.X = x / M11 + y * M21 - Dx / M11;
            //point.Y = y * M12 + y / M22 - Dy / M22;
        }

        public PointF ToMine(PointF point)
        {
            float x = point.X;
            float y = point.Y;

            ToMine(ref x, ref y);

            return new PointF(x, y);
        }

        public void ToMineSize(ref int width, ref int height)
        {

            float x = width;
            float y = height;

            width = (int)((x) * iM11 + y * iM12);
            height = (int)((y) * iM22 + x * iM21);

            //width = ( int ) ( width / M11 );
            //height = ( int ) ( height / M22 );
        }

        public void ToMineSize(ref float width, ref float height)
        {
            float x = width;
            float y = height;

            width = ((x) * iM11 + y * iM12);
            height = ((y) * iM22 + x * iM21);

            //width /= M11;
            //height /= M22;
        }

        public SizeF ToMineSize(float width, float height)
        {
            float x = width;
            float y = height;

            float tx = ((x) * iM11 + y * iM12);
            float ty = ((y) * iM22 + x * iM21);

            return new SizeF(tx, ty);
        }

        #endregion

        private static readonly float M_PI = 3.14159265359f / 180f;
        public void Rotate(int angle)
        {
            float degree = angle * M_PI;
            float cos = (float)Math.Cos(degree);
            float sin = (float)Math.Sin(degree);

            //M11 = ( world.Width / mine.Width ); //scale H
            //M22 = ( world.Height / mine.Height ); //scale V

            M12 = sin * M22;
            M21 = -sin * M11;

            M11 *= cos;
            M22 *= cos;

            ComputeInverse();
        }

        public Matrix ToTransform()
        {
            return new Matrix(M11, M12, M21, M22, Dx, Dy);
        }

        public float[] GetElements()
        {
            return new float[] { M11, M12, M21, M22, Dx, Dy };
        }

        public void Reset()
        {
            M11 = 1.0f;
            M12 = 0.0f;
            M21 = 0.0f;
            M22 = 1.0f;
            Dx = 0.0f;
            Dy = 0.0f;
            ComputeInverse();
        }

        /// <summary>
        /// Flip based by Y Axis
        /// </summary>
        public void FlipHorizontal()
        {
            // invert X
            // unchanged Y
            M11 = -M11;
            M12 = -M12;
        }

        /// <summary>
        /// Flip based by X Axis
        /// </summary>
        public void FlipVertical()
        {
            M22 = -M22;
            M21 = -M21;
        }

        public float[][] Get2DMatrix(float aM11, float aM21, float aM12, float aM22)
        {
            float[][] M = new float[2][] { new float[] { 0, 0 }, new float[] { 0, 0 } };
            M[0][0] = aM11;
            M[1][0] = aM21;
            M[0][1] = aM12;
            M[1][1] = aM22;
            return M;
        }

        internal void ComputeInverse()
        {
            float[][] M = Get2DMatrix(M11, M21, M12, M22);

            //float[][] iM = Get2DMatrix(iM11, iM21, iM12, iM22);

            float det = Determinant22(M);
            if (det != 0)
            {
                iM11 = M[1][1] / det;
                iM12 = -M[0][1] / det;
                iM21 = -M[1][0] / det;
                iM22 = M[0][0] / det;
            }

        }

        internal float Determinant22(float[][] aM)
        {
            float det = 0;
            det = aM[0][0] * aM[1][1] - aM[1][0] * aM[0][1];
            return det;
        }

    }
    
    public class MCoord : IDisposable
    {
        private MMatrix _trsf;
        private PointF[] _worldCorner;
        private RectangleF _myRect;
        private Matrix _oldTrsf;

        #region >>> Constructor <<<
        public MCoord(Rectangle mine, Rectangle world)
        {
            _myRect = mine;
            Console.WriteLine("[MCoord],MCoord( Rectangle mine, Rectangle world ),_myRect Height/Width:" + _myRect.Height.ToString() + "," + _myRect.Width.ToString());
            RectangleF rect = new RectangleF(world.Location, world.Size);
            this.setMatrix(ref rect);
        }

        public MCoord(RectangleF mine, RectangleF world)
        {
            _myRect = mine;
            Console.WriteLine("[MCoord],MCoord( RectangleF mine, RectangleF world ),_myRect Height/Width:" + _myRect.Height.ToString() + "," + _myRect.Width.ToString());
            this.setMatrix(ref world);
        }

        #endregion
        public MMatrix M_Trsf
        { get { return _trsf; }
        set {  _trsf = value; }}


        #region >>> Public Method <<<
        public void ChangeMyRect(ref Rectangle myRect)
        {
            _myRect = myRect;
            Console.WriteLine("[MCoord],ChangeMyRect( ref Rectangle myRect ),_myRect Height/Width:" + _myRect.Height.ToString() + "," + _myRect.Width.ToString());
            RectangleF rect = RectangleF.FromLTRB(_worldCorner[0].X, _worldCorner[0].Y,
                _worldCorner[1].X, _worldCorner[2].Y);

            _trsf = new MMatrix(ref _myRect, ref rect);
        }

        public void ChangeWorldRect(ref Rectangle worldRect)
        {
            RectangleF rect = new RectangleF(worldRect.Location, worldRect.Size);
            this.setMatrix(ref rect);
        }

        public void ChangeWorldSize(int width, int height)
        {
            RectangleF rect = new RectangleF(0, 0, width, height);
            this.setMatrix(ref rect);
        }

        public void ChangeWorldSize(Size size)
        {
            RectangleF rect = new RectangleF(PointF.Empty, size);
            this.setMatrix(ref rect);
        }

        public void SetWorldScale(float scaleX, float scaleY)
        {
            _trsf.SetWorldScale(scaleX, scaleY);
        }

        public void SetWorldScale(float scale)
        {
            SetWorldScale(scale, scale);
        }

        #endregion

        private void setMatrix(ref RectangleF worldRect)
        {
            _worldCorner = null;
            _worldCorner = new PointF[3] { worldRect.Location,
			   new PointF( worldRect.Right, worldRect.Top ), 
			   new PointF( worldRect.Left, worldRect.Bottom ) };

            //_trsf = new Matrix( _myRect, _worldCorner );
            _trsf = new MMatrix(ref _myRect, ref worldRect);
            //Console.WriteLine("[MCoord], setMatrix( ref RectangleF worldRect ),_myRect Height/Width:" + _myRect.Height.ToString() + "," + _myRect.Width.ToString());
            //Console.WriteLine("[MCoord], setMatrix( ref RectangleF worldRect ),M11/M22:" + _trsf.M11.ToString() + "," + _trsf.M22.ToString());
        }

        #region >>> To Mine <<<

        public Point ToMine(int worldX, int worldY)
        {
            _trsf.ToMine(ref worldX, ref worldY);
            return new Point(worldX, worldY);
        }

        public PointF ToMine(float worldX, float worldY)
        {
            _trsf.ToMine(ref worldX, ref worldY);
            return new PointF(worldX, worldY);
        }

        public Point ToMine(Point worldLocation)
        {
            return ToMine(worldLocation.X, worldLocation.Y);
        }

        public void ToMine(ref float worldX, ref float worldY)
        {
            _trsf.ToMine(ref worldX, ref worldY);
        }

        public void ToMine(ref Rectangle rect)
        {
            int x = rect.X;
            int y = rect.Y;
            int w = rect.Width;
            int h = rect.Height;

            _trsf.ToMine(ref x, ref y);
            _trsf.ToMineSize(ref w, ref h);

            rect = new Rectangle(x, y, w, h);
        }

        public void ToMine(ref Point worldLocation)
        {
            int x = worldLocation.X;
            int y = worldLocation.Y;
            _trsf.ToMine(ref x, ref y);

            worldLocation = new Point(x, y);
        }

        public void ToMine(ref PointF worldLocation)
        {
            float x = worldLocation.X;
            float y = worldLocation.Y;
            _trsf.ToMine(ref x, ref y);

            worldLocation = new PointF(x, y);
        }

        public void ToMine(ref SizeF worldSize)
        {
            float w = worldSize.Width;
            float h = worldSize.Height;
            _trsf.ToMineSize(ref w, ref h);

            worldSize = new SizeF(w, h);
        }
        #endregion

        #region >>> To World <<<

        public int ScaleToWorld(int mySize)
        {
            //Point[] pts = new Point[] { new Point( mySize, 0 ) };
            //_trsf.TransformVectors( pts );
            //return pts[0].X;

            float size_x = mySize;
            float size = 0;
            _trsf.ToWorldSize(ref size_x, ref size);
            Console.WriteLine("[MCoord], ScaleToWorld( int mySize ),M11/M22:" + _trsf.M11.ToString() + "," + _trsf.M22.ToString());
            
            return (int)size_x;
        }

        public float ScaleToWorld(float mySize)
        {
            //PointF[] pts = new PointF[] { new PointF( mySize, 0 ) };
            //_trsf.TransformVectors( pts );
            //return pts[0].X;
            float size = 0;
            _trsf.ToWorldSize(ref mySize, ref size);
            Console.WriteLine("[MCoord], ScaleToWorld( int mySize ),M11/M22:" + _trsf.M11.ToString() + "," + _trsf.M22.ToString());
            return mySize;
        }

        public void ScaleToWorld(ref Point myVector)
        {
            //Point[] pts = new Point[] { myVector };
            //_trsf.TransformVectors( pts );
            //myVector = pts[0];
            //pts = null;

            float w = myVector.X;
            float h = myVector.Y;
            _trsf.ToWorldSize(ref w, ref h);
            Console.WriteLine("[MCoord], ScaleToWorld( ref Point myVector),M11/M22:" + _trsf.M11.ToString() + "," + _trsf.M22.ToString());
            myVector = new Point((int)w, (int)h);
        }

        public void ScaleToWorld(ref Size myVector)
        {
            //Point[] pts = new Point[] { new Point( myVector ) };
            //_trsf.TransformVectors( pts );
            //myVector.Width = pts[0].X;
            //myVector.Height = pts[0].Y;
            //pts = null;

            float w = myVector.Width;
            float h = myVector.Height;
            _trsf.ToWorldSize(ref w, ref h);
            Console.WriteLine("[MCoord], ScaleToWorldref Size myVector(),M11/M22:" + _trsf.M11.ToString() + "," + _trsf.M22.ToString());
            myVector = new Size((int)w, (int)h);
        }

        public void ScaleToWorld(ref SizeF myVector)
        {
            //PointF[] pts = new PointF[] { new PointF( myVector.Width, myVector.Height ) };
            //_trsf.TransformVectors( pts );
            //myVector.Width = pts[0].X;
            //myVector.Height = pts[0].Y;
            //pts = null;

            float w = myVector.Width;
            float h = myVector.Height;
            _trsf.ToWorldSize(ref w, ref h);

            Console.WriteLine("[MCoord], ScaleToWorld(ref SizeF myVector),M11/M22:" + _trsf.M11.ToString() + "," + _trsf.M22.ToString());

            Console.WriteLine("[MCoord], ScaleToWorld(ref SizeF myVector),dx/dy:" + _trsf.Dx.ToString() + "," + _trsf.Dy.ToString());
            myVector = new SizeF(w, h);
        }

        public SizeF ScaleToWorld(int myWidth, int myHeight)
        {
            //PointF[] pts = new PointF[] { new PointF( myWidth, myHeight ) };
            //_trsf.TransformVectors( pts );
            //return new SizeF( pts[0].X, pts[0].Y );
            float w = myWidth;
            float h = myHeight;
            _trsf.ToWorldSize(ref w, ref h);
            Console.WriteLine("[MCoord], ScaleToWorld(int myWidth, int myHeight ),M11/M22:" + _trsf.M11.ToString() + "," + _trsf.M22.ToString());
            return new SizeF(w, h);
        }

        public Point ToWorld(int myX, int myY)
        {
            //Point[] pts = new Point[1] { new Point( myX, myY ) };
            //_trsf.TransformPoints( pts );
            //return pts[0];

            float x = myX;
            float y = myY;

            _trsf.ToWorld(ref x, ref y);
            Console.WriteLine("[MCoord], ScaleToWorld(int myX, int myY ),M11/M22:" + _trsf.M11.ToString() + "," + _trsf.M22.ToString());
            return new Point((int)x, (int)y);
        }

        public PointF ToWorld(float myX, float myY)
        {
            //PointF[] pts = new PointF[1] { new PointF( myX, myY ) };
            //_trsf.TransformPoints( pts );
            //return pts[0];

            _trsf.ToWorld(ref myX, ref myY);
            Console.WriteLine("[MCoord], ScaleToWorld(float myX, float myY  ),M11/M22:" + _trsf.M11.ToString() + "," + _trsf.M22.ToString());
            return new PointF(myX, myY);
        }

        public PointF ToWorld(PointF myXY)
        {
            //PointF[] pts = new PointF[1] { myXY };
            //_trsf.TransformPoints( pts );
            //return pts[0];

            float x = myXY.X;
            float y = myXY.Y;
            _trsf.ToWorld(ref x, ref y);
            Console.WriteLine("[MCoord], ScaleToWorld( PointF myXY  ),M11/M22:" + _trsf.M11.ToString() + "," + _trsf.M22.ToString());
            return new PointF(x, y);
        }

        public Point ToWorld(Point myXY)
        {
            //Point[] pts = new Point[1] { myXY };
            //_trsf.TransformPoints( pts );
            //return pts[0];

            float x = myXY.X;
            float y = myXY.Y;
            _trsf.ToWorld(ref x, ref y);
            return new Point((int)x, (int)y);
        }

        public Rectangle ToWorld(Point myXY, int myWidth, int myHeight)
        {
            //Rectangle rect = new Rectangle( myXY.X, myXY.Y, myWidth, myHeight );
            //this.MineToWorld( ref rect );
            //return rect;

            float x = myXY.X;
            float y = myXY.Y;
            float w = myWidth;
            float h = myHeight;
            _trsf.ToWorld(ref x, ref y);
            _trsf.ToWorldSize(ref w, ref h);
            return new Rectangle((int)x, (int)y, (int)w, (int)h);
        }

        public RectangleF ToWorld(int myX, int myY, float myWidth, float myHeight)
        {
            //Point[] pts = new Point[1] { new Point( myX, myY ) };
            //_trsf.TransformPoints( pts );
            //myX = pts[0].X;
            //myY = pts[0].Y;
            //pts = null;

            //PointF[] ptsf = new PointF[] { new PointF( myWidth, myHeight ) };
            //_trsf.TransformVectors( ptsf );

            //myWidth = ptsf[0].X;
            //myHeight = ptsf[0].Y;

            //ptsf = null;

            //return new RectangleF( myX, myY, myWidth, myHeight );

            float x = myX;
            float y = myY;
            _trsf.ToWorld(ref x, ref y);
            _trsf.ToWorldSize(ref myWidth, ref myHeight);
            return new RectangleF(x, y, myWidth, myHeight);
        }

        public void ToWorld(ref Point mine)
        {
            //Point[] pts = new Point[] { mine };
            //_trsf.TransformPoints( pts );
            //mine = pts[0];
            //pts = null;

            float x = mine.X;
            float y = mine.Y;
            _trsf.ToWorld(ref x, ref y);
            mine = new Point((int)x, (int)y);
        }

        public void ToWorld(ref PointF mine)
        {
            //PointF[] pts = new PointF[] { mine };
            //_trsf.TransformPoints( pts );
            //mine = pts[0];
            //pts = null;

            float x = mine.X;
            float y = mine.Y;
            _trsf.ToWorldSize(ref x, ref y);
            mine = new PointF(x, y);
        }

        public void ToWorld(ref Rectangle mine)
        {
            //float x = mine.X;
            //float y = mine.Y;
            //float w = mine.Height;
            //float h = mine.Width;

            //_trsf.ToWorld( ref x, ref y );
            //_trsf.ToWorldSize( ref w, ref h );
            //mine = new Rectangle( ( int ) x, ( int ) y, ( int ) w, ( int ) h );

            float l = mine.Left;
            float t = mine.Top;
            float r = mine.Right;
            float b = mine.Bottom;

            _trsf.ToWorld(ref l, ref t);
            _trsf.ToWorld(ref r, ref b);
            mine = Rectangle.FromLTRB((int)l, (int)t, (int)r, (int)b);
        }

        public void ToWorld(ref int myX, ref int myY)
        {
            float x = myX;
            float y = myY;
            _trsf.ToWorld(ref x, ref y);
            myX = (int)x;
            myY = (int)y;
        }

        public void ToWorld(ref float myX, ref float myY)
        {
            _trsf.ToWorld(ref myX, ref myY);
        }

        #endregion

        public void ApplyTransform(Graphics g)
        {
            _oldTrsf = g.Transform;
            g.Transform = _trsf.ToTransform();
        }

        public void RestoreTransform(Graphics g)
        {
            g.Transform.Dispose();
            g.Transform = _oldTrsf;

            _oldTrsf = null;
        }

        public void OffsetMine(int x, int y)
        {
            _trsf.OffsetMine(x, y);
        }

        public Matrix Transform
        {
            get
            {
                return _trsf.ToTransform();
            }
        }

        public void Dispose()
        {
            if (_oldTrsf != null)
                _oldTrsf.Dispose();
        }

        public void Rotate(int angle)
        {
            _trsf.Rotate(angle);
        }
    }
}
