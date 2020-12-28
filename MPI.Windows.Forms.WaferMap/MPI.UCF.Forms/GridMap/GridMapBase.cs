using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using MPI.Drawing;
using MPI.UCF.Forms.Drawing;

namespace MPI.UCF.Forms.Domain
{
	public abstract class GridMapBase : ScrollableDrawBox, IGridMap
	{
		[Flags]
		protected enum EGMStatus : uint
		{
			None = 0x00u,
			EnableFocusBox = 0x01u,
			HasFocus = 0x02u,
			EraseFocusBox = 0x04u,
			EnableCellHover = 0x08u,
			HasCellHover = 0x10u,
			EnableSelectionZoom = 0x20u,
		}

		private bool _fullDraw;

		public const int MIN_ZOOM_SCALE = 1;
		public const int MAX_ZOOM_SCALE = 120;

		protected uint _focusColor, _hoverColor;
		protected int _cellSize;
		protected Point _cellFocus, _cellHover;
		protected Rectangle _boundary;

		protected GradeRenderBase _gradeRender;

		protected EGMStatus _gmStatus;

		#region >>> Constructor / Disposor <<

		internal GridMapBase()
		{
			_cellSize = 1;
			_gmStatus = EGMStatus.EnableSelectionZoom;
			this.FocusColor = Color.Red;
			this.HoverColor = Color.White;
            this.OwnerDraw = false ;
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				this.OnChipClick = null;
				this.OnChipFocus = null;
				this.OnSelection = null;

				if ( _coordData2UI != null )
				{
					this.statusOff( EInnerStatus.CoordDetermined );
					_coordData2UI.Dispose();
					_coordData2UI = null;
				}

				if ( _gradeRender != null )
					_gradeRender.Dispose();
			}

			base.Dispose( disposing );
		}

		#endregion

		#region >>> Public Event <<<

		[Category( "UControl" )]
		public event ChipFocusEventHandler OnChipClick;

		[Category( "UControl" )]
		public event ChipFocusEventHandler OnChipFocus;

		[Category( "UControl" )]
		public event BoundaryNotifyHandler OnSelection;

		#endregion

		public virtual void Zoom( int scale )
		{

		}

		public virtual void Zoom( int row, int column )
		{

		}

		public void ZoomIn()
		{
			if ( base.statusTest( EInnerStatus.CoordDetermined ) == false )
				throw new InvalidOperationException( "SetLayout first" );

			if ( _cellSize != MAX_ZOOM_SCALE )
			{
				this.Zoom( _cellSize + 1 );
			}
		}

		public void ZoomOut()
		{
			if ( _cellSize != MIN_ZOOM_SCALE )
			{
				this.Zoom( _cellSize - 1 );
			}
		}

		public virtual void AutoScale()
		{

		}

		public virtual bool ScrollTo( int row, int col )
		{
			return true;
		}

		/// <summary>
		/// invoke "InnerOwnerDraw" method
		/// </summary>
		public void Draw()
		{
			this.clearCanvas();

			if ( OwnerDraw )
			{
				Graphics canvas_g = null;
                canvas_g = Graphics.FromHdcInternal(_canvas.Hdc);
				//$RIC
				bool owner_draw = this.innerOwnerDraw( canvas_g, this.ClientRectangle, ref _canvas );
				canvas_g.Dispose();
			}
			else
				this.innerOwnerDraw( null, this.ClientRectangle, ref _canvas );

			//this.Invalidate();
			base.Refresh();
			_fullDraw = true;
		}

		public void DrawOne( int row, int col )
		{
            Rectangle area = new Rectangle(col, row, 1, 1);
            //_coordData2UI.ToWorld( ref area );
            if (_coordData2UI != null)
            {
                area = _coordData2UI.TransCoord(area);
                area.Offset(-_scrollX, -_scrollY);

                if (OwnerDraw)
                {
                    Graphics canvas_g = Graphics.FromHdcInternal(_canvas.Hdc);
                    bool owner_draw = this.innerOwnerDraw(canvas_g, area, ref _canvas);
                    canvas_g.Dispose();
                }
                else
                {
                    this.innerOwnerDraw(null, area, ref _canvas);
                }

                this.Invalidate(area);
                _fullDraw = false;
            }
		}

		public bool SaveToImage( string file )
		{
			bool ok = true;
			Bitmap bmp = new Bitmap( _graphicsWidth, _graphicsHeight, PixelFormat.Format24bppRgb );
			this.paintToImage( bmp );

			try
			{
				bmp.Save( file );
			}
			catch ( IOException arg )
			{
				Console.Error.WriteLine( "[ScrollableWaferMap::SaveToImage] {0}", arg );
				ok = false;
			}

			bmp.Dispose();
			bmp = null;

			return ok;
		}

		public bool FocusOn( int row, int column )
		{
			if ( base.statusTest( EInnerStatus.CoordDetermined ) == false )
				throw new InvalidOperationException( "SetLayout first" );

			//$RIC,Restore
			this.RedrawOne( _cellFocus.Y, _cellFocus.X );
			_cellFocus = new Point( column, row );
			this.statusOn( EGMStatus.HasFocus );

			//Point pos = _coordData2UI.ToWorld( column, row );

            Point pos = _coordData2UI.TransCoord2P(column, row);

			EScrollChangeFlag flags = base.setScrollPosition( pos.X, pos.Y, false );
			if ( flags != EScrollChangeFlag.None )
				base.updateScrollBar( flags );

			//this.Refresh();
			//$RIC, redraw small area
			this.RedrawOne( _cellFocus.Y, _cellFocus.X );

			return ( flags != EScrollChangeFlag.None );
		}

		public void RedrawOne( int row, int col )
		{
			Rectangle area = new Rectangle( col, row, 1, 1 );
			//_coordData2UI.ToWorld( ref area );
            area = _coordData2UI.TransCoord( area);
			this.Invalidate( area );
		}

		/// <summary>
		/// Redraw area in row / column
		/// </summary>
		/// <param name="area">Row / Column area</param>
		public void RedrawArea( Rectangle area )
		{
			area.Inflate( 1, 1 );
			//_coordData2UI.ToWorld( ref area );
            area = _coordData2UI.TransCoord(area);
			area.Offset( -_scrollX, -_scrollY );
			this.Invalidate( area );
		}

		public void SetGradeRender( GradeRenderBase render )
		{
			if ( render != null )
				_gradeRender = render;
		}

		#region >>> Public Property <<<

		[Category( "UControl" )]
		public bool SelectionZoom
		{
			get
			{
				return this.statusTest( EGMStatus.EnableSelectionZoom );
			}
			set
			{
				this.statusOn( EGMStatus.EnableSelectionZoom, value );
			}
		}

		[Category( "UControl" )]
		public Color FocusColor
		{
			get
			{
				return ColorTranslator.FromWin32( ( int ) _focusColor );
			}
			set
			{
				_focusColor = ( uint ) ColorTranslator.ToWin32( value );
			}
		}

		[Category( "UControl" )]
		public bool FocusBox
		{
			get
			{
				return this.statusTest( EGMStatus.EnableFocusBox );
			}
			set
			{
				this.statusOn( EGMStatus.EnableFocusBox, value );
				if ( value == false )
				{
					_cellFocus = Point.Empty;
					this.Invalidate();
				}
			}
		}

		[Category( "UControl" )]
		public bool Snap
		{
			get
			{
				return this.statusTest( EGMStatus.EnableCellHover );
			}

			set
			{
				this.statusOn( EGMStatus.EnableCellHover, value );
				if ( value == false )
				{
					this.statusOff( EGMStatus.HasCellHover );
					this.Invalidate();
				}
			}
		}

		[Category( "UControl" )]
		public Color HoverColor
		{
			get
			{
				return ColorTranslator.FromWin32( ( int ) _hoverColor );
			}
			set
			{
				_hoverColor = ( uint ) ColorTranslator.ToWin32( value );
			}
		}

		[Category( "UControl" )]
		public bool DragToGo
		{
			get
			{
				return base.statusTest( ScrollableDrawBox.EInnerStatus.EnablePan );
			}
			set
			{
				base.statusOn( ScrollableDrawBox.EInnerStatus.EnablePan, value );
			}
		}

		[Browsable( false )]
		public int ZoomScale
		{
			get
			{
				return _cellSize;
			}
		}

		public bool OwnerDraw;

		#endregion

		#region >>> Focus Function <<<

		private void drawFocusBox( IntPtr hdc, uint color, ref Point focus )
		{
			//Point point = _coordData2UI.ToWorld( focus );
            Point point = _coordData2UI.TransCoord2P(focus.X,focus.Y);
			point.Offset( -_scrollX, -_scrollY );
			if ( 0 <= point.X && point.X <= ( int ) _canvas.Width && 0 <= point.Y && point.Y <= ( int ) _canvas.Height )
			{
				MGraphics.DrawRectangle( hdc, point.X - 2, point.Y - 2,
					_cellSize + 3, _cellSize + 3, color );
			}
		}

		private void redrawFocusBox( ref Point focus )
		{
			Rectangle area = new Rectangle( focus, Size.Empty );
			area.Inflate( 2, 2 );
			//_coordData2UI.ToWorld( ref area );
            area = _coordData2UI.TransCoord( area);
			area.Offset( -_scrollX, -_scrollY );
			this.Invalidate( area );
		}

		private void setFocusBox( Point mouse, EGMStatus status, ref Point focusPos )
		{
			// restore orignal focus
			this.redrawFocusBox( ref focusPos );

			float x = mouse.X + _scrollX;
			float y = mouse.Y + _scrollY;

			//_coordData2UI.ToMine( ref x, ref y );
            _coordData2UI.I_TransCoord(ref x, ref y);
			x = ( x > 0 ) ? ( int ) Math.Floor( x ) : -( int ) Math.Ceiling( -x );
			y = ( y > 0 ) ? ( int ) Math.Floor( y ) : -( int ) Math.Ceiling( -y );

			focusPos = new Point( ( int ) x, ( int ) y );
			this.statusOn( status );

			// draw new focus
			this.redrawFocusBox( ref focusPos );
		}

		#endregion

		#region >>> UI Override Handler <<<

		protected override void OnPaint( PaintEventArgs e )
		{
			base.OnPaint( e );

			IntPtr hdc = e.Graphics.GetHdc();
			if ( this.statusTest( EGMStatus.EnableFocusBox | EGMStatus.HasFocus ) ) // 回復 focus 外框
			{
				drawFocusBox( hdc, _focusColor, ref _cellFocus );
			}

			if ( this.statusTest( EGMStatus.HasCellHover ) )
			{
				if ( _cellFocus != _cellHover )
					this.drawFocusBox( hdc, _hoverColor, ref _cellHover );
			}

			e.Graphics.ReleaseHdc();

		}

		protected override void OnMouseWheel( MouseEventArgs e )
		{
			base.OnMouseWheel( e );
			//按著中鍵，再滾輪，可縮放
			if ( base.statusTest( EInnerStatus.EnablePan ) && ( Form.ModifierKeys == Keys.Control ) )
			{
				if ( e.Delta < 0 )
				{
					this.ZoomOut();
					return;
				}

				this.ZoomIn();
			}
		}

		protected override void OnMouseMove( MouseEventArgs e )
		{
			if ( e.Button != MouseButtons.None )
			{
				base.OnMouseMove( e );
				return;
			}

			if ( statusTest( EInnerStatus.CoordDetermined ) && statusTest( EGMStatus.EnableCellHover ) )
			{
				this.setFocusBox( e.Location, EGMStatus.HasCellHover, ref _cellHover );
				this.raiseFocusEvent( _cellHover.Y, _cellHover.X );
			}
		}

		protected override void OnMouseUp( MouseEventArgs e )
		{
			base.OnMouseUp( e );

			if ( statusTest( EInnerStatus.CoordDetermined ) && statusTest( EGMStatus.EnableFocusBox ) )
			{
				if ( e.Button == MouseButtons.Left )
				{
					this.setFocusBox( e.Location, EGMStatus.HasFocus, ref _cellFocus );
					this.raiseClickEvent( _cellFocus.Y, _cellFocus.X );
				}
			}
		}

		#endregion

		#region >>> Protected Method <<<

		protected override void InnerOnSelection( Rectangle area )
		{
			if ( statusTest( EInnerStatus.CoordDetermined ) == false )
				return;

			Rectangle grid_area = area;
			grid_area.Offset( _scrollX, _scrollY ); // $RIC, offset

			//_coordData2UI.ToMine( ref grid_area );
            grid_area = _coordData2UI.I_TransCoord( grid_area);


			int grid_width = grid_area.Width;
			if ( grid_width == 0 )
				grid_width = MIN_ZOOM_SCALE;

			int grid_height = grid_area.Height;
			if ( grid_height == 0 )
				grid_height = MIN_ZOOM_SCALE;

			if ( this.statusTest( EGMStatus.EnableSelectionZoom ) && area.Width > 5 && area.Height > 5 )
			{
				float num = Math.Abs( ( float ) _boundary.Width / ( float ) grid_width );
				num = ( float ) Math.Ceiling( ( double ) Math.Max( num, ( float ) _boundary.Height / ( float ) grid_height ) );
				this.selectionZoom( ( int ) num, grid_area.Y, grid_area.X );
			}

			if ( this.OnSelection != null )
				this.OnSelection( grid_area );
		}

		protected override void OnScrollbarScroll( object sender, ScrollEventArgs e )
		{
			if ( e == null || e.NewValue != e.OldValue )
			{
				base.OnScrollbarScroll( sender, e );
				this.Draw();
			}
		}

		protected abstract void paintToImage( Bitmap image );

		protected abstract void calcGraphicsSize();

		protected void drawCell( IntPtr hdc, int x, int y, int cellSize, uint color )
		{
			if ( cellSize == 1 )
			{
				GDI32.SetPixel( hdc, x, y, color );
				return;
			}
			else if ( cellSize == 2 )
			{
				GDI32.SetPixel( hdc, x, y, color );
				GDI32.SetPixel( hdc, x + 1, y, color );
				GDI32.SetPixel( hdc, x, y + 1, color );
				GDI32.SetPixel( hdc, x + 1, y + 1, color );
				return;
			}

			GDI32.SetDCBrushColor( hdc, color );
			GDI32.SetDCPenColor( hdc, color );
			GDI32.Rectangle( hdc, x, y, x + cellSize, y + cellSize );

			if ( cellSize > 5 ) // gray border
			{
				GDI32.SetDCPenColor( hdc, 0x999999 );
				GDI32.Rectangle( hdc, x, y, x + cellSize, y + cellSize );
			}
		}

		protected bool statusTest( EGMStatus item )
		{
			return ( _gmStatus & item ) == item;
		}

		protected void statusOn( EGMStatus item )
		{
			_gmStatus |= item;
		}

		protected void statusOff( EGMStatus item )
		{
			_gmStatus &= ( _gmStatus ^ item );
		}

		protected void statusOn( EGMStatus item, bool onOff )
		{
			_gmStatus = ( onOff ? ( _gmStatus | item ) : ( _gmStatus & ( _gmStatus ^ item ) ) );
		}

		protected void clearCanvas()
		{
			TCanvasInfo.Clear( ref _canvas, _backColor );
		}

		protected virtual void selectionZoom( int scale, int row, int column )
		{
			if ( scale > 0 && _cellSize != scale )
			{
				_cellSize += scale; // append ?
				this.calcGraphicsSize();

				//Point pos = _coordData2UI.ToWorld( column, row );
                Point pos = _coordData2UI.TransCoord2P(column, row);
				//pos.Offset( -this.Width / 2, -this.Height / 2 );
				EScrollChangeFlag flags = base.setScrollPosition( pos.X, pos.Y, false );
				this.statusOn( EGMStatus.HasFocus );
				_cellFocus = new Point( column, row );

				base.updateScrollBar( flags );

				this.Draw();
			}
		}

		protected void raiseClickEvent( int row, int column )
		{
			if ( OnChipClick != null )
				OnChipClick( row, column );
		}

		protected void raiseFocusEvent( int row, int column )
		{
			if ( OnChipFocus != null )
				OnChipFocus( row, column );
		}

		#endregion
	}
}
