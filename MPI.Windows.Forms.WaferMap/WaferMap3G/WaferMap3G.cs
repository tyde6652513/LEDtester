using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

using MPI;
using MPI.Drawing;

namespace MPI.UCF.Forms.Domain
{
	public enum EGradeColorMethod
	{
		KBlendColor = 0,
		Blend2Color = 1,
	}

	public enum EGrowthDirection
	{
		None = -1,
		Downward = 0,
		Upward = 1,
		Rightward = 2,
		Leftward = 3
	}

	public class WaferMap3G : Control
	{
		#region >>> Private Field <<<
		private bool _dataAccept;

		private float _maxValue;
		private float _minValue;

		private int _labelMinSize;

		private bool _showBlendBar;

		private EGradeColorMethod _gradeMethod;
		private EDieStatus _invalidStatus;

		private Color _colorMax;
		private Color _colorMin;

		private short _lastColumn;
		private short _lastRow;


		private bool _imageReady;
		private bool _hasFirstChip;
		private bool _allowAreaZoom;

		private float _zoomScale = WaferBox.MIN_ZOOM_SCALE;
		private int _stepScale;
		private const float SCALE_STEP = 0.5f;

		#endregion

		#region >>> UI Component <<<
		private Bitmap BlendImage;
		private Label BlendLabel;
		private ScrollableControl ScrollPanel;
		private WaferFocusableBox Box;
		#endregion

		private string _symbolId;
		private Pen _erasedPen;
		private Pen _chipPen;
		private bool _isChipScaled;
		private bool _isSeamless;
		#region >>> Internal Field <<<

		internal SizeF _chipScrSize;
		internal WaferDatabase _wfdb;
		internal MCoord fCoord;
		internal Graphics _waferGraphics;
		internal Bitmap _waferImage;

		internal DrawChipEventHandler OnDrawChip;

		#endregion

		#region >>> Constructor / Disposor <<<

		public WaferMap3G()
		{
			InitializeComponent();

			this.BackColor = Color.Black;

			_gradeMethod = EGradeColorMethod.KBlendColor;
			_colorMin = Color.Red;
			_colorMax = Color.Blue;
			_maxValue = 100f;
			_dataAccept = true;
			_showBlendBar = true;

			_invalidStatus = EDieStatus.NotExist;

			fCoord = new MCoord( new Rectangle( 0, 0, 10, 10 ), new Rectangle( 0, 0, 10, 10 ) );
			_stepScale = WaferBox.MIN_ZOOM_SCALE;
			_zoomScale = WaferBox.MIN_ZOOM_SCALE;
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_dataAccept = false;
				_imageReady = false;

				if ( _chipPen != null )
					_chipPen.Dispose();

				if ( _erasedPen != null )
					_erasedPen.Dispose();

				if ( _waferGraphics != null )
					_waferGraphics.Dispose();

				if ( _waferImage != null )
					_waferImage.Dispose();
			}

			base.Dispose( disposing );
		}

		private void InitializeComponent()
		{
			this.BlendLabel = new System.Windows.Forms.Label();
			this.Box = new MPI.UCF.Forms.Domain.WaferFocusableBox();
			this.ScrollPanel = new ScrollableControl();
			this.SuspendLayout();

			// 
			// lblBlend
			// 
			this.BlendLabel.BackColor = System.Drawing.Color.Transparent;
			this.BlendLabel.Font = new System.Drawing.Font( "Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 0 ) ) );
			this.BlendLabel.Location = new System.Drawing.Point( 3, 367 );
			this.BlendLabel.Name = "lblBlend";
			this.BlendLabel.Size = new System.Drawing.Size( 420, 12 );
			this.BlendLabel.TabIndex = 0;
			this.BlendLabel.Paint += new System.Windows.Forms.PaintEventHandler( this.BlendLabel_Paint );
			this.BlendLabel.Dock = DockStyle.Bottom;

			//
			// pnlParent
			// 
			this.ScrollPanel.AutoScroll = true;
			this.ScrollPanel.Dock = DockStyle.Fill;
			this.ScrollPanel.Controls.Add( this.Box );

			this.Controls.Add( this.ScrollPanel );
			this.Controls.Add( this.BlendLabel );

			this.DoubleBuffered = false;
			this.Name = "WaferMap3G";
			this.SizeChanged += new System.EventHandler( this.WaferMap_SizeChanged );
			this.ResumeLayout();
		}

		#endregion

		#region >>> Inner UI Event Handler <<<

		protected override void OnHandleCreated( EventArgs e )
		{
			base.OnHandleCreated( e );

			if ( this.DesignMode == false )
				this.resetBlendImage(); // make sure that lblBlend is ready.

			Box.fOnAreaSelected += new BoundaryNotifyHandler( this.OnAreaSelection );

			_erasedPen = new Pen( Box.BackColor );
			_chipPen = new Pen( Color.White );
		}

		//protected override void OnKeyDown( KeyEventArgs e )
		//{
		//   base.OnKeyDown( e );

		//   if ( e.KeyCode == Keys.Left )
		//      this.innerScrollOffset( -1, 0 );
		//   else if ( e.KeyCode == Keys.Right )
		//      this.innerScrollOffset( 1, 0 );
		//   else if ( e.KeyCode == Keys.Up )
		//      this.innerScrollOffset( 0, -1 );
		//   else if ( e.KeyCode == Keys.Down )
		//      this.innerScrollOffset( 0, 1 );
		//}

		private void BlendLabel_Paint( object sender, PaintEventArgs e )
		{
			if ( BlendImage != null )
				e.Graphics.DrawImageUnscaled( BlendImage, 0, 0 );
		}

		private void WaferMap_SizeChanged( object sender, EventArgs e )
		{
			if ( this.ClientRectangle.Width <= _labelMinSize || this.ClientRectangle.Height <= _labelMinSize )
			{
				this.Size = new Size( _labelMinSize, _labelMinSize );
			}

			Rectangle rect = new Rectangle( 3, this.ClientSize.Height - 33, this.ClientSize.Width - 24, 12 );
			BlendLabel.Location = rect.Location;
			BlendLabel.Size = rect.Size;
			this.Box.Size = this.ClientSize;
			this.resetBlendImage();
		}

		#endregion

		#region >>> Wafer Draw <<<

		/// <summary>
		/// BeginDraw by Square
		/// </summary>
		private void beginDraw( Rectangle waferBound )
		{
			if ( this.Box.Size.IsEmpty )
				return;

			_imageReady = false;

			Rectangle disp_rect = this.Box.ClientRectangle;
			int image_size = disp_rect.Width;

			if ( _waferImage != null )
			{
				_waferImage.Dispose();
				_waferImage = null;
			}

			_waferImage = new Bitmap( image_size, image_size, PixelFormat.Format24bppRgb ); // size of wafer image is the same with CanvasLabel

			if ( _waferGraphics != null )
			{
				_waferGraphics.Dispose();
				_waferGraphics = null;
			}

			_waferGraphics = Graphics.FromImage( _waferImage );
			_waferGraphics.InterpolationMode = InterpolationMode.NearestNeighbor;
			_waferGraphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
			_waferGraphics.Clear( this.Box.BackColor );

			if ( fCoord != null )
			{
				fCoord.Dispose();
				fCoord = null;
			}

			fCoord = new MCoord( waferBound, this.Box.ClientRectangle );

		}

		private void endDraw()
		{
			_imageReady = true;
		}

		public void DrawImage( Graphics g )
		{
			if ( _imageReady )
			{
				if ( _stepScale > WaferBox.MIN_ZOOM_SCALE )
					g.DrawImage( _waferImage, this.Box.ClientRectangle );
				else
					g.DrawImageUnscaled( _waferImage, this.Box.ClientRectangle );
			}
		}

		#endregion

		#region >>> Public Event / Delegate Method <<<

		[Browsable( false )]
		public GradeColorCallback GradeColorGetter;

		[Category( "WaferMap" )]
		public event ChipFocusEventHandler OnChipClick
		{
			add
			{
				Box.fOnChipClick += value;
			}
			remove
			{
				Box.fOnChipClick -= value;
			}
		}

		[Category( "WaferMap" )]
		public event ChipFocusEventHandler OnChipFocus
		{
			add
			{
				Box.fOnChipFocus += value;
			}

			remove
			{
				Box.fOnChipFocus -= value;
			}
		}

		[Category( "WaferMap" )]
		public event BoundaryNotifyHandler OnSelection
		{
			add
			{
				Box.fOnAreaSelected += value;
			}

			remove
			{
				Box.fOnAreaSelected -= value;
			}

		}
		#endregion

		#region >>> Public Property <<<

		[Category( "WaferMap::Selection" )]
		public bool Selectable
		{
			get
			{
				return Box.fIsSelectable;
			}
			set
			{
				Box.fIsSelectable = value;
			}
		}

		[Category( "WaferMap::Selection" )]
		public bool SelectionZoom
		{
			get
			{
				return _allowAreaZoom;
			}

			set
			{
				_allowAreaZoom = value;
			}
		}

		[Category( "WaferMap::Selection" )]
		public Color SelectionColor
		{
			get
			{
				return Box.fSelectionPen.Color;
			}

			set
			{
				try
				{
					Box.fSelectionPen.Color = value;
				}
				catch ( ArgumentException ae )
				{
					Console.Error.WriteLine( "[WaferMap::SelectionColor] " + ae.Message );
				}
			}
		}

		[Category( "WaferMap" )]
		public Color MaxLevelColor
		{
			get
			{
				return _colorMax;
			}
			set
			{
				if ( _colorMax == value )
					return;

				_colorMax = value;

				this.resetBlendImage();
			}
		}

		[Category( "WaferMap" )]
		public Color MinLevelColor
		{
			get
			{
				return _colorMin;
			}
			set
			{
				if ( _colorMin == value )
					return;

				_colorMin = value;

				this.resetBlendImage();
			}
		}

		[Category( "WaferMap" )]
		public bool DynamicMode
		{
			get
			{
				return Box.fIsDynamic;
			}
			set
			{
				Box.fIsDynamic = value;

				if ( _wfdb == null )
					return;

				if ( value == true )
				{
					_wfdb.OnOutOfBoundary -= this.redrawAll;
					_wfdb.OnOutOfBoundary += this.redrawAll;
				}
				else
				{
					_wfdb.OnOutOfBoundary -= this.redrawAll;
				}
			}
		}

		[Category( "WaferMap" )]
		public string SymbolId
		{
			get
			{
				return _symbolId;
			}

			set
			{
				_symbolId = value;
			}
		}

		[Category( "WaferMap" )]
		public float MaxLevelValue
		{
			get
			{
				return _maxValue;
			}

			set
			{
				if ( _maxValue == value )
					return;

				_maxValue = value;

				this.resetBlendImage();
			}
		}

		[Category( "WaferMap" )]
		public float MinLevelValue
		{
			get
			{
				return _minValue;
			}

			set
			{
				if ( _minValue == value )
					return;

				_minValue = value;

				this.resetBlendImage();
			}
		}

		[Category( "WaferMap" )]
		public bool BlendBar
		{
			get
			{
				return _showBlendBar;
			}
			set
			{
				_showBlendBar = value;
				BlendLabel.Visible = value;
			}
		}

		[Category( "WaferMap" )]
		public bool ScaledChip
		{
			get
			{
				return _isChipScaled;
			}
			set
			{
				_isChipScaled = value;
			}
		}

		[Category( "WaferMap" )]
		public bool Seamless
		{
			get
			{
				return _isSeamless;
			}
			set
			{
				_isSeamless = value;
			}
		}

		[Category( "WaferMap" ), Browsable( false )]
		public EGradeColorMethod GradeMethod
		{
			get
			{
				return _gradeMethod;
			}

			set
			{
				_gradeMethod = value;
				if ( _showBlendBar )
					this.resetBlendImage();
			}
		}

		[Category( "WaferMap::Focus" )]
		public Color FocusColor
		{
			get
			{
				return Box.fFocusColor;
			}

			set
			{
				if ( Box.fFocusColor != value )
					Box.fFocusColor = value;
			}
		}

		[Category( "WaferMap::Focus" )]
		public bool FocusBox
		{
			get
			{
				return Box.fEnableFocus;
			}

			set
			{
				Box.fEnableFocus = value;

				if ( value == false )
				{
					Box.fHasLastFocus = false;
					this.Cursor = Cursors.Default;
				}
				else
				{
					this.Cursor = Cursors.Cross;
				}
			}
		}

		[Category( "WaferMap::Misc" )]
		public bool Snap
		{
			get
			{
				return Box.fUseChipSnap;
			}

			set
			{
				Box.fUseChipSnap = value;
			}
		}

		[Category( "WaferMap::Misc" )]
		public bool DragToGo
		{
			get
			{
				return Box.fEnableDragToGo;
			}
			set
			{
				Box.fEnableDragToGo = value;
			}
		}

		[Category( "WaferMap::Misc" )]
		public EDieStatus InvalidStatus
		{
			get
			{
				return _invalidStatus;
			}
			set
			{
				_invalidStatus = value;
			}
		}

		[Browsable( false )]
		public int ZoomScale
		{
			get
			{
				return _stepScale;
			}
		}

		/// <summary>
		/// Erase die color.
		/// </summary>
		[Browsable( false )]
		public Color EraseDieColor
		{
			get
			{
				if ( _erasedPen != null )
					return _erasedPen.Color;

				return Color.Empty;
			}

			set
			{
				if ( _erasedPen != null )
					_erasedPen.Color = value;
			}
		}

		[Browsable( false )]
		public EGrowthDirection GrowthDirection
		{
			get
			{
				if ( _wfdb != null )
					return _wfdb.fGrowthDir;

				return EGrowthDirection.Downward;
			}

			set
			{
				if ( _wfdb != null )
					_wfdb.fGrowthDir = value;
			}
		}

		#endregion

		#region >>> Private Method <<<

		private void resetBlendImage()
		{
			if ( this.Created == false )
				return;

			if ( BlendImage != null )
			{
				BlendImage.Dispose();
				BlendImage = null;
			}

			BlendImage = new Bitmap( this.BlendLabel.Width, this.BlendLabel.Height );
			using ( Graphics g = Graphics.FromImage( BlendImage ) )
			{
				this.paintBlendColor( g );

				if ( _showBlendBar )
				{
					this.drawBlendValue( g );
					BlendLabel.Invalidate();
				}
			}
		}

		private void paintBlendColor( Graphics g )
		{
			ColorBlend blend = new ColorBlend();

			if ( _gradeMethod == EGradeColorMethod.Blend2Color )
			{
				blend.Colors = new Color[] { _colorMin, _colorMax };
				blend.Positions = new float[] { 0.0f, 1.0f };
			}
			else //if ( _gradeMethod == EGradeColorMethod.KBlendColor )
			{
				_colorMin = Color.Red;
				_colorMax = Color.Blue;
				blend.Colors = new Color[] { Color.Red, Color.Orange, Color.Yellow, Color.YellowGreen, Color.Cyan, Color.Purple, Color.Blue };
				blend.Positions = new float[] { 0.0f, 0.17f, 0.33f, 0.50f, 0.66f, 0.83f, 1.0f };
			}

			LinearGradientBrush brush = new LinearGradientBrush( BlendLabel.ClientRectangle, _colorMin, _colorMax, LinearGradientMode.Horizontal );
			brush.WrapMode = WrapMode.Tile;
			brush.GammaCorrection = true;
			brush.InterpolationColors = blend;

			g.FillRectangle( brush, 0, 0, BlendImage.Width, BlendImage.Height );

			brush.Dispose();
			brush = null;
		}

		private void drawBlendValue( Graphics g )
		{
			float step_value = ( _maxValue - _minValue ) / 6.0f;
			float step_width = BlendImage.Width / 6.5f;
			float mark_width = g.MeasureString( "00.000", this.BlendLabel.Font ).Width;

			_labelMinSize = ( int ) ( mark_width * 7.8f );

			float pos_x = 1;
			float value = _minValue;
			for ( int i = 0; i < 6; i++ )
			{
				g.DrawString( value.ToString( "00.00" ), BlendLabel.Font, Pens.White.Brush, pos_x, 0 );
				value += step_value;
				pos_x += step_width;
			}

			pos_x = BlendImage.Width - mark_width;
			g.DrawString( _maxValue.ToString( "00.00" ), BlendLabel.Font, Pens.White.Brush, pos_x, 0 );

		}

		private void redrawAll( Rectangle bound )
		{
			lock ( _waferGraphics )
			{
				this.beginDraw( bound );
				this.calcChipPixel();
				_wfdb.Foreach( this.DrawOne, _symbolId );
			}

			this.endDraw();

			Box.Invalidate();
		}

		private void fitScaledZooming()
		{
			Box.innerResize( this.ClientSize );

			lock ( _waferGraphics )
			{
				this.beginDraw( _wfdb.fBoundary );

				this.calcChipPixel();

				_wfdb.Foreach( this.DrawOne, _symbolId );
			}


			Box.fStepScale = WaferBox.MIN_ZOOM_SCALE;
			_zoomScale = WaferBox.MIN_ZOOM_SCALE;
			_stepScale = WaferBox.MIN_ZOOM_SCALE;
			this.endDraw();

			Box.Invalidate();
		}

		private void innerAreaZoom( ref Rectangle waferRect )
		{
			if ( waferRect.Width < 5 && waferRect.Height < 5 )
				return;

			// calc scale: wafer bounds vs. selection bounds
			float scale = ( float ) _wfdb.fBoundary.Width / ( float ) waferRect.Width;
			scale = ( float ) Math.Ceiling( Math.Max( scale, ( ( float ) _wfdb.fBoundary.Height / ( float ) waferRect.Height ) ) );

			this.Zoom( ( int ) scale );

			this.innerScrollTo( waferRect.X, waferRect.Y );
		}

		/// <summary>
		/// Scroll to position by column & row
		/// </summary>
		private bool innerScrollTo( int column, int row )
		{
			Point pos = fCoord.ToWorld( column, row );

			bool scroll = false;
			if ( ScrollPanel.HorizontalScroll.Minimum < pos.X && pos.X < ScrollPanel.HorizontalScroll.Maximum )
			{
				ScrollPanel.HorizontalScroll.Value = pos.X;
				scroll = true;
			}

			if ( ScrollPanel.VerticalScroll.Minimum < pos.Y && pos.Y < ScrollPanel.VerticalScroll.Maximum )
			{
				ScrollPanel.VerticalScroll.Value = pos.Y;
				scroll = true;
			}

			return scroll;
		}

		internal void innerScrollOffset( int offsetX, int offsetY )
		{
			if ( offsetX != 0 )
			{
				int h = ScrollPanel.HorizontalScroll.Value;
				int min_h = ScrollPanel.HorizontalScroll.Minimum;
				int max_h = ScrollPanel.HorizontalScroll.Maximum;

				h += offsetX;
				if ( h <= min_h )
					h = min_h;

				if ( h >= max_h )
					h = max_h;

				if ( h != ScrollPanel.HorizontalScroll.Value )
					ScrollPanel.HorizontalScroll.Value = h;
			}

			if ( offsetY != 0 )
			{
				int v = ScrollPanel.VerticalScroll.Value;
				int min_v = ScrollPanel.VerticalScroll.Minimum;
				int max_v = ScrollPanel.VerticalScroll.Maximum;

				v += offsetY;
				if ( v <= min_v )
					v = min_v;

				if ( v >= max_v )
					v = max_v;

				if ( v != ScrollPanel.VerticalScroll.Value )
					ScrollPanel.VerticalScroll.Value = v;
			}
		}

		/// <summary>
		/// Zoom with screen rectangle
		/// </summary>
		private void OnAreaSelection( Rectangle scrArea )
		{
			if ( _allowAreaZoom )
			{
				fCoord.WorldToMine( ref scrArea );
				// to wafer coord
				this.innerAreaZoom( ref scrArea );
			}
		}

		private void calcChipPixel()
		{
			_chipScrSize = new SizeF( 1f, 1f );
			fCoord.ScaleToWorld( ref _chipScrSize );
			if ( _isChipScaled )
			{
				if ( _chipScrSize.Width <= 2 )
					_chipScrSize.Width = 1;
				else if ( _chipScrSize.Width == 3 )
					_chipScrSize.Width = 2;
				else
					_chipScrSize.Width -= 1;

				if ( _chipScrSize.Height <= 2 )
					_chipScrSize.Height = 1;
				else if ( _chipScrSize.Height == 3 )
					_chipScrSize.Height = 2;
				else
					_chipScrSize.Height -= 1;

				return;
			}

			float size = Math.Min( _chipScrSize.Width, _chipScrSize.Height );
			SizeF chip = new SizeF( 1f, 1f );
			if ( _isSeamless )
			{
				if ( size <= 1.0f )
					size = 1.0f;
				else if ( size <= 2.0f )
					size = 2.0f;
				else if ( size <= 3.0f )
					size = 3.0f;
				else
					size -= 1.0f;
			}
			else
			{
				size = ( int ) size;
				if ( size <= 2 )
					size = 1;
				else if ( size == 3 )
					size = 2;
				else
					size -= 2;
			}

			_chipScrSize.Width = ( int ) size;
			_chipScrSize.Height = ( int ) size;

		}

		#endregion

		#region >>> DrawOne <<<

		public bool DrawOne( int row, int col, float value )
		{
			return innerDrawOne( _waferGraphics, row, col, value );
		}

		public void DrawPattern( int row, int column )
		{
			//_wfdb[row, column, "Bin"] = float.NaN;
			this.DrawOne( row, column, float.NaN );

			Rectangle rect = new Rectangle( column, row, 1, 1 );
			fCoord.MineToWorld( ref rect );
			rect.Inflate( 1, 1 );

			Box.Invalidate( rect );
		}

		#endregion

		#region >>> RedrawOne <<<

		public void RedrawOne()
		{
			this.RedrawOne( _lastRow, _lastColumn );
		}

		public void RedrawOne( int row, int column )
		{
			if ( _imageReady )
			{
				float value = _wfdb[( short ) row, ( short ) column, _symbolId, true];

				lock ( _waferGraphics )
				{
					this.innerDrawOne( _waferGraphics, row, column, value );
				}

				PointF pos = new PointF( ( float ) column, ( float ) row );
				fCoord.MineToWorld( ref pos );

				Box.Invalidate( new Region( new RectangleF( pos.X, pos.Y, _chipScrSize.Width, _chipScrSize.Height ) ) );
			}
		}

		internal bool innerDrawOne( Graphics g, int row, int col, float value )
		{
			Color grade_color = this.GetGradeColor( value );

			if ( grade_color == Color.Empty ) // invalid die
			{
				this.drawInvalid( row, col );
				return true;
			}

			PointF pos = fCoord.ToWorld( ( float ) col, ( float ) row );

			lock ( _chipPen )
			{
				try
				{
					_chipPen.Color = grade_color;
					g.FillRectangle( _chipPen.Brush, pos.X, pos.Y, _chipScrSize.Width, _chipScrSize.Height );
				}
				catch ( Exception ae )
				{
					Console.Error.WriteLine( "[WaferMap::InnerDrawOne] " + ae.Message );
					return true;
				}
			}

			if ( OnDrawChip != null )
				OnDrawChip( g, Rectangle.Empty, row, col );

			return true;
		}

		#endregion

		#region >>> DrawInvalid <<<

		public void DrawInvalid( int row, int column )
		{
			if ( _dataAccept )
			{
				_wfdb.AddItem( ( short ) row, ( short ) column, _symbolId, float.NaN );

				this.drawInvalid( row, column );

				Rectangle rect = new Rectangle( column, row, 1, 1 );

				fCoord.MineToWorld( ref rect );
				Box.Invalidate( rect );
			}
		}

		private void drawInvalid( Graphics g, ref Rectangle clip, EDieStatus status )
		{
			float x = clip.X;
			float y = clip.Y;
			float size = clip.Width;

			switch ( status )
			{
				case EDieStatus.Bad:
				{
					// draw bad die pattern
					Pen pen = Pens.White;
					float d = size / 4f;
					g.DrawLine( pen, x + d, y, x, y + d );
					g.DrawLine( pen, x + 2 * d, y, x, y + 2 * d );
					g.DrawLine( pen, x + 3 * d, y, x, y + 3 * d );
					g.DrawLine( pen, x + 4 * d, y, x, y + 4 * d );

					pen = null;
				}
				break;
				case EDieStatus.Erased:
				{
					//lock ( _erasedPen )
					{
						g.FillRectangle( _erasedPen.Brush, clip );
					}
				}
				break;
				case EDieStatus.Inked:
				{
					g.FillRectangle( Pens.White.Brush, x + size / 8, y + size / 8, size / 3, size / 3 );
				}
				break;
				case EDieStatus.Marked:
				{
					// draw marked pattern
					float w, h, offsetX, offsetY;

					if ( size >= 5 )
					{
						w = Math.Max( size / 5, 1f );
						offsetX = 2 * w;
					}
					else if ( size >= 3 )
					{
						w = Math.Max( size / 5, 1f );
						offsetX = 1;
					}
					else
					{
						return;
					}

					if ( size >= 5 )
					{
						h = Math.Max( size / 5, 1f );
						offsetY = 2 * h;
					}
					else if ( size >= 3 )
					{
						h = Math.Max( size / 5, 1f );
						offsetY = 1;
					}
					else
					{
						return;
					}

					g.FillRectangle( Pens.White.Brush, x + offsetX, y + offsetY, w, h );
				}
				break;
				case EDieStatus.Missing:
				{
					Point p1 = new Point( ( int ) ( x + 1 ), ( int ) ( y + 1 ) );
					Point p2 = new Point( ( int ) ( x + size - 1 ), ( int ) ( y + size - 1 ) );

					g.DrawLine( Pens.Red, p1, p2 );
					g.DrawLine( Pens.Red, p1.X, p2.Y, p2.X, p1.Y );
				}
				break;
				case EDieStatus.NotExist:
				{
					// draw missing pattern
					Point p1 = new Point( ( int ) x, ( int ) y );
					Point p2 = new Point( ( int ) ( x + size ), ( int ) ( y + size ) );

					g.DrawLine( Pens.White, p1, p2 );
					g.DrawLine( Pens.White, p1.X, p2.Y, p2.X, p1.Y );

				}
				break;
				case EDieStatus.Picked:
				{
					// draw missing pattern
					float dx = size / 2;
					float dy = size / 2;

					Pen pen = Pens.White;

					g.DrawLine( pen, x + dx, y + 1, x + dx, y + size - 2 );
					g.DrawLine( pen, x + 1, y + dx, x + size - 2, y + dx );
				}
				break;
				case EDieStatus.Skiped:
				{
					g.DrawRectangle( Pens.White, x + ( size / 6 ), y + ( size / 6 ), size * 4 / 6, size * 4 / 6 );
				}
				break;
			}
		}

		private void drawInvalid( int row, int column )
		{
			Rectangle invalid_rect = new Rectangle( column, row, 1, 1 );
			fCoord.MineToWorld( ref invalid_rect );
			this.drawInvalid( _waferGraphics, ref invalid_rect, _invalidStatus );
		}

		#endregion

		#region >>> Public Method <<<

		public Color GetGradeColor( float value )
		{
			if ( GradeColorGetter != null )
				return GradeColorGetter.Invoke( value );

			if ( ( _minValue <= value && value <= _maxValue ) )
			{
				int x = ( int ) ( ( ( value - _minValue ) * ( float ) BlendImage.Width ) / ( _maxValue - _minValue ) ) - 1;
				if ( x < 0 )
					x = 0;

				return BlendImage.GetPixel( x, 1 );
			}

			return Color.Empty;
		}

		public void Draw()
		{
			if ( _waferGraphics == null )
			{
				this.beginDraw( _wfdb.fBoundary );
				this.calcChipPixel();
				_wfdb.Foreach( this.DrawOne, _symbolId );
			}
			else
			{
				lock ( _waferGraphics )
				{
					this.beginDraw( _wfdb.fBoundary );
					this.calcChipPixel();
					_wfdb.Foreach( this.DrawOne, _symbolId );
				}
			}
			this.endDraw();

			Box.Invalidate();
		}

		public void AddOne( int row, int column, FieldValue value )
		{
			if ( _dataAccept )
			{
				_dataAccept = false;

				if ( _hasFirstChip == false )
				{
					_hasFirstChip = true;
					_wfdb.innerSetStart( ( short ) row, ( short ) column );
					fCoord.ChangeMyRect( ref _wfdb.fBoundary );
				}

				_wfdb.AddItem( ( short ) row, ( short ) column, value );
				_lastColumn = ( short ) column;
				_lastRow = ( short ) row;
				_dataAccept = true;
			}
		}

		public void AddOne( int row, int column, float value )
		{
			if ( _dataAccept )
			{
				_dataAccept = false;
				if ( _hasFirstChip == false )
				{
					_hasFirstChip = true;
					_wfdb.innerSetStart( ( short ) row, ( short ) column );
					fCoord.ChangeMyRect( ref _wfdb.fBoundary );
				}

				_wfdb.AddItem( ( short ) row, ( short ) column, _symbolId, value );
				_lastColumn = ( short ) column;
				_lastRow = ( short ) row;
				_dataAccept = true;
			}
		}

		public void SetBoundary( Rectangle rect )
		{
			_wfdb.innerSetBoundary( rect );
		}

		public void Start()
		{
			this.Start( false );
		}

		public void Start( bool dynamic )
		{
			_dataAccept = true;
			this.DynamicMode = dynamic;

			Box.ClearFocus();
			Box.ClearBackground();

			_hasFirstChip = false;

			if ( dynamic )
			{
				Box.Size = this.Size;
				this.beginDraw( _wfdb.fBoundary );
				this.calcChipPixel();
				this.endDraw();
			}
		}

		public void Stop()
		{
			_dataAccept = false;
			this.DynamicMode = false;
		}

		#region >>> Value Set/Get <<
		public float GetValue( int row, int column, string symbol )
		{
			return _wfdb[( short ) row, ( short ) column, symbol];
		}

		public float GetValue( int row, int column )
		{
			return _wfdb[( short ) row, ( short ) column];
		}

		public void SetValue( int row, int column, float value )
		{
			_wfdb[( short ) row, ( short ) column] = value;
		}

		public void SetValue( int row, int column, string symbol, float value )
		{
			_wfdb[( short ) row, ( short ) column, symbol] = value;
		}

		#endregion

		public void SetDatabase( WaferDatabase wdb )
		{
			if ( wdb != null )
				_wfdb = wdb;
		}

		public void FocusOn( int row, int column )
		{
			if ( _imageReady == false )
				return;

			Box.innerFocusOn( row, column );
		}

		public bool SaveToImage( string path )
		{
			if ( _waferImage == null )
				return false;

			if ( System.IO.File.Exists( path ) == false )
			{
				string folder = System.IO.Path.GetDirectoryName( path );
				if ( Kit.IOHandler.MakeFolder( folder, false ) == false )
					return false;
			}

			try
			{
				_waferImage.Save( path );
			}
			catch ( Exception ex )
			{
				Console.Error.WriteLine( "[WaferMap3G::SaveImage] " + ex.Message );
				return false;
			}

			return true;
		}

		public bool ScrollTo( int row, int col )
		{
			return this.innerScrollTo( col, row );
		}

		public void ClearBackground()
		{
			if ( _imageReady )
				Box.ClearBackground();
		}
		#endregion

		#region >>> Zooming <<<

		public void Zoom( int scale )
		{
			if ( _imageReady == false )
				return;

			if ( scale == WaferBox.MIN_ZOOM_SCALE )
			{
				this.fitScaledZooming();
				return;
			}

			if ( WaferBox.MIN_ZOOM_SCALE < scale && scale <= WaferBox.MAX_ZOOM_SCALE )
			{
				_zoomScale = ( float ) ( scale + 1 ) / 2f;
			}
			else
			{
				if ( _stepScale != WaferBox.MAX_ZOOM_SCALE )
				{
					_zoomScale = ( float ) ( WaferBox.MAX_ZOOM_SCALE + 1 ) / 2f;
				}
			}

			//$RIC, keep the current row/column ralated to scroll position
			Point current = new Point( this.ScrollPanel.HorizontalScroll.Value, this.ScrollPanel.VerticalScroll.Value );
			fCoord.WorldToMine( ref current );

			int size = ( int ) ( _zoomScale * Box.Parent.Width );

			Size new_size = new Size( size, size );
			Box.innerResize( new_size );

			lock ( _waferGraphics )
			{
				this.beginDraw( _wfdb.fBoundary );
				this.calcChipPixel();
				_wfdb.Foreach( this.DrawOne, _symbolId );
			}

			_stepScale = scale;
			Box.fStepScale = scale;

			this.endDraw();

			this.innerScrollTo( current.X, current.Y );
			Box.Invalidate();
		}

		public void Zoom( int row, int column )
		{
			if ( _imageReady == false )
				return;

			Rectangle rect = new Rectangle( column, row, 10, 10 );
			rect.Offset( -5, -5 );
			this.innerAreaZoom( ref rect );
		}

		public void ZoomIn()
		{
			if ( _stepScale < WaferBox.MAX_ZOOM_SCALE )
			{
				this.Zoom( _stepScale + 1 );
			}
		}

		public void ZoomOut()
		{
			if ( _stepScale != WaferBox.MIN_ZOOM_SCALE )
				this.Zoom( _stepScale - 1 );
		}

		/// <summary>
		/// Reset to best viewing scale
		/// </summary>
		public void ResetScale()
		{
			this.AutoScale();
		}

		/// <summary>
		/// Fit to availiable display area
		/// </summary>
		public void AutoScale()
		{
			if ( _imageReady )
			{
				this.Zoom( WaferBox.MIN_ZOOM_SCALE );
			}
		}

		#endregion
	}
}
