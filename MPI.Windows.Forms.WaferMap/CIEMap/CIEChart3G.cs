using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.ComponentModel;

using MPI;
using MPI.Drawing;
using MPI.Drawing.Forms;

namespace MPI.UCF.Forms.Domain
{
	public class CIEChart3G : GDIDrawPanel
	{
		public delegate void LocusEventHandler( float x, float y );

		public event LocusEventHandler OnLocusOver;

		private PointF[] _spectrumLocus;

		private List<PointF[]> _binRange1931;
		private List<PointF[]> _binRange1976;

		private List<EllipseBinRange> _ellipseBinRange1931;
		private List<EllipseBinRange> _ellipseBinRange1976;

		private PointF[] _planckian;

		private Dictionary<string, Bitmap> _cieXYMap;

		private int _chartSize;

		private Pen _penLocus;
		private Pen _penGridLine;
		private Pen _penPlanckian;
		private Pen _penWavelength;
		private Pen _penBinRange;

		private CIE.EChartType _chartType;
		private CIE.EDrawItem _drawOpts;

        private bool _pointFocus;   // focus new point
        //private PointF _lastPoint;

		private string _activeKey;

		private int oldsize;
		private Dictionary<string, List<PointF>> _cieXYMapPoint;
		private float _ratio;   // total ratio
		private Rectangle _actualArea;
		private string _enabledLayer;
		private bool _magnifyFlag;
		private const string TEMP = "_temp";
        private Rectangle _oldRec;

		#region >>> Contructor / Disposor <<<
		public CIEChart3G()
		{
			_chartSize = 1024;

			_chartType = CIE.EChartType.CIE1931;

			_penGridLine = new Pen( Color.LightGray, 0.5f );

			_penGridLine.DashStyle = DashStyle.Dot;

			_penPlanckian = new Pen( Color.DarkGray );

			_penBinRange = new Pen( Color.Blue );

			_penLocus = new Pen( Color.Red );

			_penWavelength = new Pen( Color.DimGray );

			_drawOpts = CIE.EDrawItem.CEIChart | CIE.EDrawItem.CIExyPoint | CIE.EDrawItem.GridLines | CIE.EDrawItem.Planckian | CIE.EDrawItem.WaveLength | CIE.EDrawItem.BinRange;

            _pointFocus = false;

			_binRange1931 = new List<PointF[]>();
			_binRange1976 = new List<PointF[]>();

			_ellipseBinRange1931 = new List<EllipseBinRange>();
			_ellipseBinRange1976 = new List<EllipseBinRange>();

			_cieXYMap = new Dictionary<string, Bitmap>();

			_selectArea = new Rectangle( 0, 0, 0, 0 );

			_planckian = ( PointF[] ) CIE.Planckian1931.Clone();

			oldsize = 1024;
			_ratio = 1;
			_actualArea = new Rectangle( 0, 0, 0, 0 );
            _enabledLayer = string.Empty;
			_cieXYMapPoint = new Dictionary<string, List<PointF>>();
			_magnifyFlag = false;
            //_lastPoint = new PointF(0, 0);
		}

		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );

			_penGridLine.Dispose();
			_penPlanckian.Dispose();
			_penBinRange.Dispose();
			_penLocus.Dispose();
		}

		#endregion

		#region >> UI override method <<<
		protected override void OnHandleCreated( EventArgs e )
		{
			base.OnHandleCreated( e );

			this.resetLocus();

			this.drawSpectrumChart();
			this.drawThemeLayer();
		}

		protected override void OnResize( EventArgs e )
		{
			_chartSize = Math.Max( this.Width, this.Height );
			oldsize = _chartSize;
			this.Size = new Size( _chartSize, _chartSize );
            _oldRec = new Rectangle(0, 0, this.Width, this.Height);
		}

		protected override void OnSizeChanged( EventArgs e )
		{
			base.OnSizeChanged( e );

			if ( base.Created )
			{
				this.resetLocus();
				this.drawSpectrumChart();
				this.drawThemeLayer();
				this.Display();
			}
		}

		protected override void OnFontChanged( EventArgs e )
		{
			base.OnFontChanged( e );

			if ( base.Created )
				this.drawThemeLayer();
		}

		long _tick = HiTimer.Tick;

		protected override void OnMouseMove( System.Windows.Forms.MouseEventArgs e )
		{
			base.OnMouseMove( e );

			//if ( HiTimer.EvaluateEx( ref _tick ) < 15 )
			//    return;

			float x;
			float y;

            x = (float)(_actualArea.X + e.X) / ((float)_chartSize);
            y = 1.0f - ((float)(_actualArea.Y + e.Y) / ((float)_chartSize));
			
			if ( x <= 1.0 && y <= 1.0 )
			{
				//this.drawThemeLayer();
				base.Theme.Clear();
				string str = string.Format( "X,Y:({0:F5},{1:F5})", x, y );
				Size size;
				_canvas.MeasureString( str, this.Font, out size );

				x = this.Width - size.Width - 10;
				y = 30.0f;
				_theme.DrawString( str, this.Font, Brushes.OrangeRed, x, y );

                this.drawFocusPoint();
			}

			if ( _selectOn )
			{
				if ( _magnifyFlag )
					return;

				int width = e.X - _selectArea.X;
				int height = e.Y - _selectArea.Y;

                if (width < 0 || height < 0)
                    return;

				if ( width < 0 )
					width = 10;

				if ( height < 0 )
					height = 10;

				if ( width != height )
					width = Math.Max( width, height );

				_selectArea.Width = width;
				_selectArea.Height = width;

				_theme.DrawRectangle( Pens.Red, _selectArea );
			}

			this.Display();
		}

		private bool _selectOn;
		private Rectangle _selectArea;

		protected override void OnMouseDown( System.Windows.Forms.MouseEventArgs e )
		{
			base.OnMouseDown( e );

			if ( e.Button == System.Windows.Forms.MouseButtons.Left )
			{
				if ( _magnifyFlag )
					return;

				_selectOn = true;
				_selectArea.Location = e.Location;
				_selectArea.Size = Size.Empty;
			}
		}

		protected override void OnMouseUp( System.Windows.Forms.MouseEventArgs e )
		{
			base.OnMouseUp( e );

			this.magnify();
          
            _selectOn = false;
            _selectArea = Rectangle.Empty;
		}

		protected override void OnMouseDoubleClick( System.Windows.Forms.MouseEventArgs e )
		{
			base.OnMouseDoubleClick( e );

			_selectOn = false;
            _oldRec = new Rectangle(0, 0, this.Width, this.Height);
			this.revertChartSize();
            this.Display();
		}
		#endregion

		#region >>> Public Property <<<

		[Browsable( false )]

		public string ActiveKey
		{
			get
			{
				return _activeKey;
			}
		}
		[Category( "CIEChart" )]
		public CIE.EChartType ChartType
		{
			get
			{
				return _chartType;
			}

			set
			{
				if ( _chartType != value )
				{
					_chartType = value;
					if ( this.Created )
					{
						this.resetLocus();
						this.drawSpectrumChart();
						this.drawThemeLayer();
                        this.switchLayer();
                        this.drawFocusPoint();
						base.Display();
					}
				}
			}
		}

		[Category( "CIEChart" )]
		public Color GridLineColor
		{
			get
			{
				return _penGridLine.Color;
			}

			set
			{
				if ( _penGridLine.Color != value )
				{
					_penGridLine.Color = value;
					if ( this.Created )
					{
						this.drawThemeLayer();
						this.Display();
					}
				}
			}
		}

		[Category( "CIEChart" )]
		public Color PlanckianColor
		{
			get
			{
				return _penPlanckian.Color;
			}
			set
			{
				if ( _penPlanckian.Color != value )
				{
					_penPlanckian.Color = value;
					if ( this.Created )
					{
						this.drawThemeLayer();
						this.Display();
					}
				}
			}
		}

		[Category( "CIEChart" )]
		public Color BinRangeColor
		{
			get
			{
				return _penBinRange.Color;
			}
			set
			{
				if ( _penBinRange.Color != value )
				{
					_penBinRange.Color = value;
					if ( this.Created )
					{
						this.drawThemeLayer();
						this.Display();
					}
				}
			}
		}

		[Category( "CIEChart" )]
		public Color WavelengthColor
		{
			get
			{
				return _penWavelength.Color;
			}
			set
			{
				if ( _penWavelength.Color != value )
				{
					_penWavelength.Color = value;

					if ( this.Created )
					{
						this.drawThemeLayer();
						this.Display();
					}
				}
			}
		}

        [Category("CIEChart")]
        public Color LocusColor
        {
            get
            {
                return _penLocus.Color;
            }
            set
            {
                if (_penLocus.Color != value)
                {
                    _penLocus.Color = value;
                }
            }
        }

        [Category("CIEChart")]
        public bool PointFocus
        {
            get
            {
                return _pointFocus;
            }
            set
            {
                _pointFocus = value;
            }
        }

		[Browsable( false )]
		public CIE.EDrawItem DrawingItem
		{
			get
			{
				return _drawOpts;
			}

			set
			{
				_drawOpts = value;

				if ( this.Created )
				{
					this.drawSpectrumChart();
					this.drawThemeLayer();
                    this.drawFocusPoint();
					this.Display();
				}
			}
		}
		#endregion

		#region >>> Private Method <<<

		private void resetLocus()
		{
			if ( _spectrumLocus != null )
			{
				_spectrumLocus = null;
				//GC.Collect();
			}

			_spectrumLocus = ( PointF[] ) CIE.Locus1931.Clone();

			for ( int i = 0; i < _spectrumLocus.Length; i++ )
				this.toScreenXY( ref _spectrumLocus[i] );
		}

		private void drawGridLines()
		{
			if ( ( _drawOpts & CIE.EDrawItem.GridLines ) != CIE.EDrawItem.GridLines )
				return;

			Font font = new Font( "Arial", 7.0f );

			PointF p1 = new PointF( 0.0f, 0.0f );
			PointF p2 = new PointF( 0.0f, this.Width );

			if ( _chartSize != this.Width )
			{
				float start = ( float ) _actualArea.X / _chartSize;
				float end = ( float ) ( _actualArea.X + this.Width ) / _chartSize;
				float interval = ( end - start ) / 10.0f;
				for ( int x = 0; x < 10; x++ )
				{
					float gap = ( float ) x / 10.0f;
					p2.X = p1.X = this.Width * gap;
					float pos = start + interval * x;
					_canvas.DrawLine( _penGridLine, p1, p2 );
					if ( x != 0 )
						_canvas.DrawString( pos.ToString( "F4" ), font, _penGridLine.Brush, p2.X + 2, p2.Y - 10 );
				}
			}
			else
			{
				for ( int x = 0; x < 10; x++ )
				{
					float gap = ( float ) x / 10.0f;

					p2.X = p1.X = this.Width * gap;

					_canvas.DrawLine( _penGridLine, p1, p2 );
					_canvas.DrawString( gap.ToString( "F4" ), font, _penGridLine.Brush, p2.X + 2, p2.Y - 10 );
				}
			}

			p1.X = 0.0f;
			p2.X = this.Width;

			if ( _chartSize != this.Height )
			{
				float start = ( float ) ( _chartSize - _actualArea.Y ) / _chartSize;
				float end = ( float ) ( _chartSize - _actualArea.Y - this.Height ) / _chartSize;
				float interval = ( start - end ) / 10.0f;
				for ( int y = 0; y < 10; y++ )
				{
					float gap = ( float ) y / 10.0f;
					p2.Y = p1.Y = ( 10 - y ) * this.Height / 10.0f;
					float pos = end + interval * y;
					_canvas.DrawLine( _penGridLine, p1, p2 );
					if ( y != 0 )
						_canvas.DrawString( pos.ToString( "F4" ), font, _penGridLine.Brush, p1.X + 2, p1.Y - 10 );
				}
			}
			else
			{
				for ( int y = 0; y < 10; y++ )
				{
					float gap = ( float ) y / 10.0f;

					p2.Y = p1.Y = ( 10 - y ) * this.Width / 10.0f;

					_canvas.DrawLine( _penGridLine, p1, p2 );
					_canvas.DrawString( gap.ToString( "F4" ), font, _penGridLine.Brush, p1.X + 2, p1.Y - 10 );
				}
			}
			font.Dispose();
		}

		private void drawPlanckian()
		{
			if ( ( _drawOpts & CIE.EDrawItem.Planckian ) == CIE.EDrawItem.Planckian )
			{
				_planckian = ( PointF[] ) CIE.Planckian1931.Clone();
				for ( int i = 0; i < _planckian.Length; i++ )
					this.toScreenXY( ref _planckian[i] );

				PointF[] locus = ( PointF[] ) _planckian.Clone();
				if ( _chartSize != this.Width && _actualArea.Size.IsEmpty == false )
				{
					for ( int i = 0; i < locus.Length; i++ )
					{
						locus[i].X = locus[i].X - _actualArea.X;
						locus[i].Y = locus[i].Y - _actualArea.Y;
					}
				}

				_canvas.DrawBeziers( _penPlanckian, locus );

				locus = null;
			}
		}

		private void drawWavelength()
		{
			if ( ( _drawOpts & CIE.EDrawItem.WaveLength ) != CIE.EDrawItem.WaveLength )
				return;

			short wavelength = 360; //wavelength: 360~700

			this.resetLocus();
			PointF[] locus = _spectrumLocus;
			for ( int i = 1; i < locus.Length; i++ )
			{
				if ( i % 5 != 0 )
					continue;

				wavelength += 5;

				if ( Array.IndexOf<short>( CIE.WavelengthLocus1931, wavelength ) < 0 )
					continue;

				if ( _chartSize != this.Width )
				{
					locus[i].X = locus[i].X - _actualArea.X;
					locus[i].Y = locus[i].Y - _actualArea.Y;
				}
				float x = locus[i].X - 1.0f;
				float y = locus[i].Y - 1.0f;

				_canvas.FillEllipse( _penWavelength.Brush, x, y, 4.0f, 4.0f );
				_canvas.DrawString( wavelength.ToString(), this.Font, _penWavelength.Brush, ( x + 5.0f ), ( y - 2.5f ) );

			}
		}


		private void drawBinRange()
		{
			if ( ( _drawOpts & CIE.EDrawItem.BinRange ) == CIE.EDrawItem.BinRange )
			{
				PointF[] locus;

				List<PointF[]> binRange;
				if ( _chartType == CIE.EChartType.CIE1931 )
					binRange = _binRange1931;
				else
					binRange = _binRange1976;

				foreach ( PointF[] range in binRange )
				{
					locus = ( PointF[] ) range.Clone();
					for ( int i = 0; i < locus.Length; i++ )
					{
						locus[i].X = ( locus[i].X ) * _chartSize;
						locus[i].Y = ( 1.0f - locus[i].Y ) * _chartSize;
						//this.toScreenXY( ref locus[i] );
					}

					if ( _chartSize != this.Width && _actualArea.Size.IsEmpty == false )
					{
						for ( int i = 0; i < locus.Length; i++ )
						{
							locus[i].X = locus[i].X - _actualArea.X;
							locus[i].Y = locus[i].Y - _actualArea.Y;
						}
					}

					_canvas.DrawLines( _penBinRange, locus );
                    _canvas.DrawLine(_penBinRange, locus[0], locus[locus.Length - 1]);
				}

                PointF center = new PointF();
                EllipseBinRange ellipse;

				List<EllipseBinRange> ellipseBinRange;
				if ( _chartType == CIE.EChartType.CIE1931 )
					ellipseBinRange = _ellipseBinRange1931;
				else
					ellipseBinRange = _ellipseBinRange1976;
				foreach ( EllipseBinRange range in ellipseBinRange )
				{
                    ellipse = (EllipseBinRange)range.Clone();
                    ellipse.RadiusX = ellipse.RadiusX * this.Width;
                    ellipse.RadiusY = ellipse.RadiusY * this.Height;
                    center.X = range.CenterX;
                    center.Y = range.CenterY;

                    center.X = (center.X) * _chartSize;
                    center.Y = (1.0f - center.Y) * _chartSize;
					//this.toScreenXY( ref center );

					if ( _chartSize != this.Width && _actualArea.Size.IsEmpty == false )
					{
						center.X = center.X - _actualArea.X;
						center.Y = center.Y - _actualArea.Y;
                        ellipse.RadiusX = ellipse.RadiusX * _ratio;
                        ellipse.RadiusY = ellipse.RadiusY * _ratio;
					}

                    PointF o_pnt = new PointF(center.X, center.Y);
                    _canvas.Transform.RotateAt((-1) * range.Angle, o_pnt);
					_canvas.SetTransform();
                    _canvas.DrawEllipse(Pens.Black, center.X - ellipse.RadiusX, center.Y - ellipse.RadiusY, ellipse.RadiusX * 2, ellipse.RadiusY * 2);
					_canvas.ResetTransform();
				}

			}
		}


		private void drawSpectrumChart()
		{
			if ( ( _drawOpts & CIE.EDrawItem.CEIChart ) == CIE.EDrawItem.CEIChart )
			{
				// Reset all coordinates(_spectrumLocus) of chart color map
				this.resetLocus();

				GraphicsPath gpath = new GraphicsPath();
				gpath.Reset();
				gpath.AddPolygon( _spectrumLocus );

				Region rgn = new Region();
				rgn.MakeEmpty();
				rgn.Union( gpath );

				Bitmap image = new Bitmap( oldsize, oldsize, PixelFormat.Format24bppRgb );

				BitmapData bd = image.LockBits( new Rectangle( 0, 0, oldsize, oldsize ), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb );
				IntPtr ptr = bd.Scan0;

				// Padding byte
				int skip_byte = bd.Stride - oldsize * 3;

				int offset = 0;

				Color backColor = this.BackColor;

				long tick = HiTimer.Tick;

				for ( int y = _actualArea.Top; y < ( _actualArea.Top + this.Height ); y++ )
				{
					for ( int x = _actualArea.Left; x < ( _actualArea.Left + this.Width ); x++ )
					{
						Color color;
						if ( rgn.IsVisible( x, y ) )
						{
							float xr = ( float ) x / ( float ) _chartSize;
							float yr = ( float ) ( _chartSize - y ) / ( float ) _chartSize;
							color = this.evalColor( xr, yr );
						}
						else
						{
							color = backColor; //R, G, B
						}

                        //Marshal.WriteByte(ptr, offset++, color.B);
                        //Marshal.WriteByte(ptr, offset++, color.G);
                        //Marshal.WriteByte(ptr, offset++, color.R);
                        Marshal.WriteInt32(ptr, offset, color.ToArgb() & 0x00FFFFFF);
                        offset += 3;
					}

					offset += skip_byte;
				}

				//Console.WriteLine( "[WriteByte] {0:F4}", HiTimer.Evaluate( tick ) );
				//_canvas.DrawImageData(ptr, PixelFormat.Format24bppRgb);
				_canvas.DrawImageData( bd.Scan0, PixelFormat.Format24bppRgb );
				image.UnlockBits( bd );
				gpath.Dispose();
				rgn.Dispose();
			}
		}

		private void drawThemeLayer()
		{
			base.Theme.Clear(); // trigger the first init

			this.drawPlanckian();

			this.drawWavelength();

			this.drawGridLines();

			this.drawBinRange();

			_canvas.DrawRectangle( Pens.Black, this.ClientRectangle );
		}

		private void magnify()
		{
            if ( _magnifyFlag )
                return;

            if (_selectOn == false || _selectArea.Size.IsEmpty)
                return;

            _canvas.Clear();
            _chartSize = Math.Max(this.Width, this.Height);
            oldsize = _chartSize;
            _ratio = 1;
            _actualArea = Rectangle.Empty;
            _magnifyFlag = false;
            this.clearTempLayer();

            Rectangle newRec = new Rectangle();

            this.toScreenRectangle(ref newRec);

			oldsize = _chartSize;
            _ratio = (float)Math.Round((decimal)this.Width / (decimal)newRec.Width, 4);
			_chartSize = ( int ) Math.Round( _chartSize * _ratio );
			
			MCoord coord = new MCoord( this.ClientRectangle, new Rectangle( 0, 0, _chartSize, _chartSize ) );

			// Transform selected rectangle to resized rectangle
			coord.MineToWorld( ref newRec );

			_actualArea = newRec;

			this.drawSpectrumChart();
			this.drawThemeLayer();

            if (_enabledLayer != string.Empty)
                this.addMagnifiedLayer(_enabledLayer.Substring(0, _enabledLayer.LastIndexOf('_')));

			base.Display();

			//_chartSize = oldsize;
		}

        private void addMagnifiedLayer(string key)
        {
            if (_cieXYMapPoint.ContainsKey(key) == false)
                return;

            Bitmap image = null;
            Graphics g = null;
            string layer = key + TEMP;

            if (_cieXYMap.ContainsKey(layer) == true)
                return;

            // create a new layer
            image = new Bitmap(this.Width, this.Height, PixelFormat.Format24bppRgb);
            g = Graphics.FromImage(image);
            g.Clear(this.BackColor);
            g.Dispose();
            this.SetLayer(layer, image);
            _cieXYMap.Add(layer, image);

            // redraw all points
            image = _cieXYMap[layer];
            g = Graphics.FromImage(image);

            PointF locus;

            List<PointF> list= _cieXYMapPoint[key];
            for (int i = 0; i < list.Count; i++)
            {
                locus = list[i];

                this.toScreenXY(ref locus);

                //If the chart is magnified, draw new point on it
                if (_chartSize != oldsize)
                {
                    locus.X = locus.X - _actualArea.X;
                    locus.Y = locus.Y - _actualArea.Y;
                }

                g.DrawEllipse(_penLocus, locus.X - 1.5f, locus.Y - 1.5f, 3.0f, 3.0f);
            }
            g.Dispose();

            base.EnableAllLayer(false);
            base.EnableLayer(layer, true);
        }

		/// <summary>
		/// Revert chart size to original size
		/// </summary>
		private void revertChartSize()
		{
			_canvas.Clear();
			_chartSize = Math.Max( this.Width, this.Height );
			oldsize = _chartSize;
			_ratio = 1;
			_actualArea = Rectangle.Empty;
			_magnifyFlag = false;

            this.drawSpectrumChart();
            this.drawThemeLayer();
            this.revertLayer();
		}

		private void revertLayer()
		{
            this.clearTempLayer();

            base.EnableLayer(_enabledLayer, true);

		}

        private void switchLayer()
        {
            if (_enabledLayer == string.Empty)
                return;

            string key = string.Empty;
            string layer = string.Empty;

            base.EnableAllLayer(false);

            key = _enabledLayer.Substring(0, _enabledLayer.LastIndexOf('_'));
            layer = key + "_" + ((int)_chartType).ToString();

            if (_chartSize != this.Width)
            {
                this.clearTempLayer();
                this.addMagnifiedLayer(key);
            }
            else
            {
                base.EnableLayer(layer, true);
            }

            _enabledLayer = layer;
        }

        /// <summary>
        /// get the selected rectangle
        /// </summary>
        /// <param name="newRec"></param>
        /// <returns></returns>
        private void toScreenRectangle(ref Rectangle newRec)
        {
            int newWidth = (int)((float)_selectArea.Width / (float)this.Width * (float)_oldRec.Width);
            int newHeight = (int)((float)_selectArea.Height / (float)this.Height * (float)_oldRec.Height);

            int newX = (int)((float)_selectArea.X / (float)this.Width * (float)_oldRec.Width) + _oldRec.X;
            int newY = (int)((float)_selectArea.Y / (float)this.Height * (float)_oldRec.Height) + _oldRec.Y;

            if (newWidth == 0 || newHeight == 0)
            {
                _magnifyFlag = true;
                newRec = _oldRec;
            }
            else
            {
                newRec = new Rectangle(newX, newY, newWidth, newHeight);
                _oldRec = new Rectangle(newX, newY, newWidth, newHeight);
            }
        }

        private void enableFirstLayer()
        {
            string key = string.Empty;
            string layer = string.Empty;
            foreach (KeyValuePair<string, Bitmap> item in _cieXYMap)
            {
                if (base.IsEnabledLayer(item.Key) == true)
                    return;
            }
            
            key = _cieXYMap.First().Key.Substring(0, _cieXYMap.First().Key.LastIndexOf('_'));
            layer = key + "_" + ((int)_chartType).ToString();

            if (_chartSize != this.Width)
                base.EnableLayer(key + TEMP, true);
            else
                base.EnableLayer(layer, true);

            _enabledLayer = layer;
        }

        private void clearTempLayer()
        {
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, Bitmap> item in _cieXYMap)
            {
                if (item.Key.IndexOf(TEMP) >= 0)
                {
                    list.Add(item.Key);
                    this.SetLayer(item.Key, null);
                }
            }

            foreach (string layer in list)
                _cieXYMap.Remove(layer);

            list = null;
        }

        /// <summary>
        /// Redraw the last point
        /// </summary>
        private void drawFocusPoint()
        {
            if (_pointFocus && _cieXYMapPoint.Count > 0)
            {
                string key = _enabledLayer.Substring(0, _enabledLayer.LastIndexOf('_'));
                // 2013.5.23 Stanley ReConfirm this modificatio is correct

                if (this._cieXYMapPoint.ContainsKey(key))
                {
                List<PointF> list = _cieXYMapPoint[key];

                PointF locus = new PointF(list[list.Count - 1].X, list[list.Count - 1].Y);
                this.toScreenXY(ref locus);

                if (_chartSize != oldsize)
                {
                    locus.X = locus.X - _actualArea.X;
                    locus.Y = locus.Y - _actualArea.Y;
                }
                    _theme.FillEllipse(Brushes.Blue, locus.X - 2.5f, locus.Y - 2.5f, 7.0f, 7.0f);
                }
            }
        }

		#endregion

		#region >>> Public Method <<<

		public void CreateHandle()
		{
			base.CreateHandle();
		}

		public void ActiveLOP( string key )
		{
			if ( _cieXYMap.ContainsKey( key ) )
			{
				if ( key == _activeKey )
					return;

				this.ActivateLayer( key );
				_activeKey = key;
			}
		}

		public void ClearMap( string key )
		{
			if ( _cieXYMap.ContainsKey( key ) )
			{
				Bitmap image = _cieXYMap[key];
				Graphics g = Graphics.FromImage( image );
				g.Clear( this.BackColor );
				g.Dispose();
			}
		}

        public void AddXy( string key, float x, float y )
        {
            Bitmap image = null;
            Graphics g = null;
            List<PointF> list = null;
            PointF locus;
            string layer = string.Empty;

            // add new point record list
            if (!_cieXYMapPoint.ContainsKey(key))
            {
                list = new List<PointF>();
                _cieXYMapPoint.Add(key, list);
            }
            // record new point
            list = _cieXYMapPoint[key];
            list.Add(new PointF(x, y));

            //_lastPoint = new PointF(x, y);

            foreach (int type in Enum.GetValues(typeof(CIE.EChartType)))
            {
                layer = key + "_" + type.ToString();

                if (_cieXYMap.ContainsKey(layer) == false)
                {
                    image = new Bitmap(this.Width, this.Height, PixelFormat.Format24bppRgb);

                    g = Graphics.FromImage(image);
                    g.Clear(this.BackColor);
                    g.Dispose();
                    this.SetLayer(layer, image);
                    _cieXYMap.Add(layer, image);

                    // enable the default layer
                    this.enableFirstLayer();
                }

                image = _cieXYMap[layer];

                locus = new PointF(x, y);

                this.toScreenXY(ref locus, (CIE.EChartType)type, this.Width);

                g = Graphics.FromImage(image);

                g.DrawEllipse(_penLocus, locus.X - 1.5f, locus.Y - 1.5f, 3.0f, 3.0f);
                g.Dispose();

                if (_pointFocus)
                {
                    if (_enabledLayer == layer && _chartSize == oldsize)
                    {
                        base.Theme.Clear();
						_theme.FillEllipse(Brushes.Blue, locus.X - 2.5f, locus.Y - 2.5f, 7.0f, 7.0f);
                    }
                }
            }

            if (_chartSize != oldsize)
            {
                if (_enabledLayer.IndexOf(key) == -1)
                    return;

                layer = key + TEMP;
                this.addMagnifiedLayer(key);

                image = _cieXYMap[layer];

                locus = new PointF(x, y);

                this.toScreenXY(ref locus);

                locus.X = locus.X - _actualArea.X;
                locus.Y = locus.Y - _actualArea.Y;

                g = Graphics.FromImage(image);
                g.DrawEllipse(_penLocus, locus.X - 1.5f, locus.Y - 1.5f, 3.0f, 3.0f);
                g.Dispose();

                if (_pointFocus)
                {
                    base.Theme.Clear();
					_theme.FillEllipse(Brushes.Blue, locus.X - 2.5f, locus.Y - 2.5f, 7.0f, 7.0f);
                }
            }
        }

		public void Redraw()
		{
			this.Display();
		}

		//public bool DefineBinRange( PointF[] range )
		//{
		//    //if ( range.Length != 4 )
		//    //    return false;

		//    PointF[] pnts = ( PointF[] ) range.Clone();

		//    _binRange.Add( pnts );

		//    return true;
		//}

		public bool DefineBinRange( PointF[] range, CIE.EChartType type )
		{
            //if ( range.Length != 4 )
            //    return false;

			PointF[] pnts = ( PointF[] ) range.Clone();

			if ( type == CIE.EChartType.CIE1931 )
				_binRange1931.Add( pnts );
			else
				_binRange1976.Add( pnts );

			return true;
		}

		public void ClearBinRange()
		{
			if ( _binRange1931 != null )
				_binRange1931.Clear();

			if ( _binRange1976 != null )
				_binRange1976.Clear();

			if ( _ellipseBinRange1931 != null )
				_ellipseBinRange1931.Clear();

			if ( _ellipseBinRange1976 != null )
				_ellipseBinRange1976.Clear();
		}

        public void RedrawChart()
        {
            _canvas.Clear();

            this.drawSpectrumChart();
            this.drawThemeLayer();
            this.Display();
        }

		public bool DefineEllipseBinRange( EllipseBinRange range, CIE.EChartType type )
		{
			if ( type == CIE.EChartType.CIE1931 )
				_ellipseBinRange1931.Add( range );
			else
				_ellipseBinRange1976.Add( range );

			return true;
		}

        /// <summary>
        /// switch point layer
        /// </summary>
        /// <param name="key"></param>
		public void EnableXyLayer( string key )
		{
			if (!this.IsHandleCreated)
				return;

            if (_theme != null)
                _theme.Clear();

            string layer = string.Empty;

			if ( _chartSize != oldsize )
			{
                _enabledLayer = string.Empty;

                this.clearTempLayer();

                layer = key + TEMP;

                if (_cieXYMap.ContainsKey(layer) == false)
                    this.addMagnifiedLayer(key);
			}
			else
			{
                // disable all layer
                base.EnableAllLayer(false);

                _enabledLayer = string.Empty;

                layer = key + "_" + ((int)_chartType).ToString();
                base.EnableLayer(layer, true);
			}

            _enabledLayer = key + "_" + ((int)_chartType).ToString();

            this.drawFocusPoint();
			base.Display();
		}

        /// <summary>
        /// Release all layers and point records
        /// </summary>
        /// <param name="isResetChart"></param>
		public void Release(bool isResetChart)
		{
            if (!this.IsHandleCreated)
                return;

			foreach ( KeyValuePair<string, Bitmap> item in _cieXYMap )
				base.SetLayer( item.Key, null );

            base.Theme.Clear();
			_cieXYMap.Clear();
			_cieXYMapPoint.Clear();
            _enabledLayer = string.Empty;
            //_lastPoint = PointF.Empty;

            if (isResetChart)
            {
                this.revertChartSize();
            }
            this.Display();
		}

        //public void InitialChartRegion( PointF startP, PointF endP )
        //{
        //    _selectArea.X = ( int ) Math.Round( startP.X * this.Width );
        //    _selectArea.Y = ( int ) Math.Round( ( 1 - startP.Y ) * this.Height );
        //    _selectArea.Width = ( int ) Math.Round( endP.X * this.Width ) - _selectArea.X;
        //    _selectArea.Height = ( int ) Math.Round( ( 1 - endP.Y ) * this.Height ) - _selectArea.Y;

        //    // only support one magnifience
        //    if ( _magnifyFlag )
        //        return;

        //    if ( _selectArea.Width <= 0 || _selectArea.Height <= 0 )
        //        return;

        //    // only support square
        //    if ( _selectArea.Width != _selectArea.Height )
        //        return;

        //    if ( startP.X >= 1.0f || startP.Y >= 1.0f || endP.X >= 1.0f || endP.Y >= 1.0f )
        //        return;

        //    _canvas.Clear();

        //    oldsize = _chartSize;
        //    float ratio = ( float ) Math.Round( 1 / ( endP.X - startP.X ), 4 );
        //    _chartSize = ( int ) Math.Round( _chartSize * ratio );

        //    MCoord coord = new MCoord( this.ClientRectangle, new Rectangle( 0, 0, _chartSize, _chartSize ) );

        //    // Transform selected rectangle to resized rectangle
        //    coord.MineToWorld( ref _selectArea );
        //    _actualArea = _selectArea;

        //    this.drawSpectrumChart();
        //    this.drawThemeLayer();

        //    base.Display();

        //    //_chartSize = oldsize;
        //    _selectArea = Rectangle.Empty;
			
        //}

		#endregion

		#region >>> Protected Method <<<
		protected void toScreenXY( ref PointF locus )
		{
			if ( _chartType == CIE.EChartType.CIE1931 )
			{
				locus.X = ( locus.X ) * _chartSize;
				locus.Y = ( 1.0f - locus.Y ) * _chartSize;
			}
			else
			{
				float v = -2.0f * locus.X + 12.0f * locus.Y + 3.0f;
				locus.X = ( ( 4.0f * locus.X ) / v ) * _chartSize;

				//if ( _chartType == CIE.EChartType.CIE1960 )
					//locus.Y = ( 1.0f - ( 6.0f * locus.Y ) / v ) * _chartSize;
                //else if ( _chartType == CIE.EChartType.CIE1976 )
                locus.Y = (1.0f - (9.0f * locus.Y) / v) * _chartSize;
			}
		}

		protected void toScreenXY( ref PointF locus, int size )
		{
			if ( _chartType == CIE.EChartType.CIE1931 )
			{
				locus.X = ( locus.X ) * size;
				locus.Y = ( 1.0f - locus.Y ) * size;
			}
			else
			{
				float v = -2.0f * locus.X + 12.0f * locus.Y + 3.0f;
				locus.X = ( ( 4.0f * locus.X ) / v ) * size;

				//if ( _chartType == CIE.EChartType.CIE1960 )
					//locus.Y = ( 1.0f - ( 6.0f * locus.Y ) / v ) * size;
				//else if ( _chartType == CIE.EChartType.CIE1976 )
                locus.Y = (1.0f - (9.0f * locus.Y) / v) * size;
			}
		}

        protected void toScreenXY(ref PointF locus, CIE.EChartType type, int size)
        {
            if (type == CIE.EChartType.CIE1931)
            {
                locus.X = (locus.X) * size;
                locus.Y = (1.0f - locus.Y) * size;
            }
            else
            {
                float v = -2.0f * locus.X + 12.0f * locus.Y + 3.0f;
                locus.X = ((4.0f * locus.X) / v) * size;

                //if (type == CIE.EChartType.CIE1960)
                    //locus.Y = (1.0f - (6.0f * locus.Y) / v) * size;
                //else if (type == CIE.EChartType.CIE1976)
                locus.Y = (1.0f - (9.0f * locus.Y) / v) * size;
            }
        }

		protected void evalXy( ref float x, ref float y )
		{
			//if ( _chartType == CIE.EChartType.CIE1960 )
            //{
            //    float v = -2.0f * x + 12.0f * y + 3.0f;

            //    x = ( 4.0f * x ) / v;
            //    y = ( 6.0f * y ) / v;
            //}
			if ( _chartType == CIE.EChartType.CIE1976 )
			{
				float v = -2.0f * x + 12.0f * y + 3.0f;

				x = ( 4.0f * x ) / v;
				y = ( 9.0f * y ) / v;
			}
		}

		protected void evalXy( ref PointF point )
		{
            //if ( _chartType == CIE.EChartType.CIE1960 )
            //{
            //    float v = -2.0f * point.X + 12.0f * point.Y + 3.0f;

            //    point.X = ( 4.0f * point.X ) / v;
            //    point.Y = ( 6.0f * point.Y ) / v;
            //}
			if ( _chartType == CIE.EChartType.CIE1976 )
			{
				float v = -2.0f * point.X + 12.0f * point.Y + 3.0f;

				point.X = ( 4.0f * point.X ) / v;
				point.Y = ( 9.0f * point.Y ) / v;
			}
		}

		protected Color evalColor( float X, float Y )
		{
            //if ( _chartType == CIE.EChartType.CIE1960 )
            //{
            //    float v = 2.0f * X - 8.0f * Y + 4.0f;

            //    X = ( 3.0f * X ) / v;
            //    Y = ( 2.0f * Y ) / v;
            //}
			if ( _chartType == CIE.EChartType.CIE1976 )
			{
				float v = 6.0f * X - 16.0f * Y + 12.0f;

				X = ( 9.0f * X ) / v;
				Y = ( 4.0f * Y ) / v;
			}

			float Z = 1.0f - X - Y;

			float R = +2.25860f * X - 1.03950f * Y - 0.34730f * Z;
			float G = -1.34950f * X + 2.34410f * Y + 0.06960f * Z;
			float B = +0.09100f * X - 0.30460f * Y + 1.27770f * Z;

			R = ( R < 0.0f ) ? 0.0f : ( R > 1.0f ? 1.0f : R );
			G = ( G < 0.0f ) ? 0.0f : ( G > 1.0f ? 1.0f : G );
			B = ( B < 0.0f ) ? 0.0f : ( B > 1.0f ? 1.0f : B );

			float max = Math.Max( Math.Max( R, G ), B );

			R = 255 * R / max;
			G = 255 * G / max;
			B = 255 * B / max;

			return Color.FromArgb( ( int ) R, ( int ) G, ( int ) B );
		}

		#endregion
	}

}
