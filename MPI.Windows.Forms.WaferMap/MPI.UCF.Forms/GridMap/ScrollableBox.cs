using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using MPI.Drawing;
using System.Runtime.InteropServices;
using MPI.Tester.Tools;

namespace MPI.UCF.Forms.Drawing
{
	[Flags]
	public enum EScrollChangeFlag : byte
	{
		None = 0x00,
		Horizontal = 0x01,
		Vertical = 0x02
	}

	public abstract class SelectableBoxBase : Control
	{
		[Flags]
		internal enum EStatus : uint
		{
			None = 0x0000,
			EnableSelect = 0x0001,
			SelectStart = 0x0002,
			IsSelecting = 0x0004,
		}

		private EStatus _status;
		private int _selectingX, _selectingY;
		private Rectangle _selectArea;

		protected int _mouseX, _mouseY;
		protected bool _isMouseDown;
		protected uint _selColor;
		private const int DOT_SIZE = 3;

		#region >>> Constructor / Disposor <<<

		public SelectableBoxBase()
		{
			_status = EStatus.EnableSelect;

			// force paint event in OnPaint, suppress OnPaintBackground
			this.SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true );
			this.SetStyle( ControlStyles.UserPaint, true );
			this.SetStyle( ControlStyles.DoubleBuffer, false );
			this.SelectionColor = Color.Yellow;
		}

		#endregion

		#region >>> Public Property <<<

		[Category( "UControl::Selection" )]
		public bool Selectable
		{
			get
			{
				return statusTest( EStatus.EnableSelect );
			}
			set
			{
				this.setStatus( EStatus.EnableSelect, value );
			}
		}

		[Category( "UControl::Selection" )]
		public Color SelectionColor
		{
			get
			{
				return ColorTranslator.FromWin32( ( int ) _selColor );
			}

			set
			{
				_selColor = ( uint ) ColorTranslator.ToWin32( value );
			}
		}

		protected override Size DefaultSize
		{
			get
			{
				return new Size( 360, 240 );
			}
		}
		#endregion

		#region >>> UI Protected / Override Method <<<

		protected override void OnMouseDown( MouseEventArgs e )
		{
			base.OnMouseDown( e );

			_mouseX = e.X;
			_mouseY = e.Y;
			_isMouseDown = true;

			if ( e.Button == MouseButtons.Left || e.Button == MouseButtons.Right && statusTest( EStatus.EnableSelect ) )
			{
				_selectingX = _mouseX;
				_selectingY = _mouseY;
				this.setStatus( EStatus.SelectStart, true );
				this.Invalidate(); //$RIC, tricky
			}

		}

		protected override void OnMouseMove( MouseEventArgs e )
		{
			base.OnMouseMove( e );

			if ( statusTest( EStatus.SelectStart ) )
			{
				this.setStatus( EStatus.IsSelecting, true );
				this.doSelecting( e.Location );
				this.Invalidate();
				return;
			}
		}

		protected override void OnMouseUp( MouseEventArgs e )
		{
			base.OnMouseUp( e );

			//if ( _isMouseDown && e.Button == MouseButtons.Left )
			//   _isMouseDown = false;

			if ( _isMouseDown  )
			   _isMouseDown = false;

			if ( statusTest( EStatus.IsSelecting ) )
			{
				this.setStatus( EStatus.IsSelecting | EStatus.SelectStart, false );
				InnerOnSelection( _selectArea );
				_selectArea = Rectangle.Empty;
				this.Invalidate( false );
			}
			else if ( this.statusTest( EStatus.SelectStart ) )
			{
				this.setStatus( EStatus.SelectStart, false );
				_selectArea = Rectangle.Empty;
			}
		}

		protected override void OnMouseLeave( EventArgs e )
		{
			base.OnMouseLeave( e );

			if ( _isMouseDown )
				_isMouseDown = false;
		}

		protected override void OnPaint( PaintEventArgs e )
		{
			if ( statusTest( EStatus.IsSelecting ) )
			{
				if ( _selectArea.Width > 5 || _selectArea.Height > 5 )
				{
					IntPtr hdc = e.Graphics.GetHdc();
					MGraphics.DrawRectangle( hdc, ref _selectArea, _selColor );
					for ( int x = _selectArea.Left; x < _selectArea.Right; x += DOT_SIZE )
						for ( int y = _selectArea.Top; y < _selectArea.Bottom; y += DOT_SIZE )
							MGraphics.SetPixel( hdc, x, y, 0x00ffffff );
					e.Graphics.ReleaseHdc();
				}
			}

			base.OnPaint( e );
		}

		#endregion

		#region >>> Private Method <<<

		private void doSelecting( Point mouse )
		{
			int x = mouse.X;
			int width = Math.Abs( x - _selectingX );

			if ( width < 3 )
				return;

			int y = mouse.Y;
			int height = Math.Abs( y - _selectingY );
			if ( height < 3 )
				return;

			if ( x > _selectingX )
				x = _selectingX;

			if ( y > _selectingY )
				y = _selectingY;

			_selectArea = new Rectangle( x, y, width, height );
		}

		internal bool statusTest( EStatus item )
		{
			return ( ( _status & item ) == item );
		}

		internal void setStatus( EStatus item, bool onOff )
		{
			_status = ( onOff ) ? ( _status | item ) : ( _status & ( _status ^ item ) );
		}

		#endregion

		protected void enableSelect( bool enabled )
		{
			setStatus( EStatus.EnableSelect, enabled );
		}

		protected virtual void InnerOnSelection( Rectangle area )
		{

		}
	}

	public class ScrollableDrawBox : SelectableBoxBase
	{
		[Flags]
		internal enum EInnerStatus : uint
		{
			None = 0x0000,
			Instanced = 0x0001,
			Disposed = 0x0002,
			GraphicsSet = 0x0004,
			HandledCreated = 0x0008,
			CoordDetermined = 0x0010,
			OwnerDrawing = 0x0020,

			EnablePan = 0x0040,
			PanningStart = 0x0080,
			IsPanning = 0x0100,
		}

		private bool _rendering;
		private bool _invertGrap = true;

		private EInnerStatus _status;
		private IntPtr _handle;
		private int _panStartX, _panStartY;
		private int _panningX, _panningY;

		protected VScrollBar _vScroll;
		protected HScrollBar _hScroll;

		protected int _scrollBuffer;
		protected TCanvasInfo _canvas;

        protected CoordTransferTool _coordData2UI;
		//protected MCoord _coord;
		protected int _scrollX, _scrollY;
		protected int _graphicsWidth, _graphicsHeight;
		protected uint _backColor;

		internal static Cursor _curGrab, _curGrabbing;

		#region >>> Constructor / Disposor <<<
		static ScrollableDrawBox()
		{
			_curGrab = Cursors.Hand;
			_curGrabbing = Cursors.Hand;
		}

		public ScrollableDrawBox()
		{
			//_graphicsX = -1;
			//_graphicsY = -1;

			InitializeComponent();

			this.statusOn( EInnerStatus.Instanced | EInnerStatus.EnablePan, true );
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
				TCanvasInfo.DestoryCanvas( ref _canvas );

			base.Dispose( disposing );
		}

		#endregion

		#region >>> UI <<<

		private void InitializeComponent()
		{
			_vScroll = new VScrollBar();
			_vScroll.Visible = false;
			_vScroll.Scroll += this.OnScrollbarScroll;
			_vScroll.Dock = DockStyle.Right;

			_hScroll = new HScrollBar();
			_hScroll.Visible = false;
			_hScroll.Scroll += this.OnScrollbarScroll;
			_hScroll.Dock = DockStyle.Bottom;

			this.Size = new Size( 100, 100 );

			this.Controls.Add( _vScroll );
			this.Controls.Add( _hScroll );

			_scrollBuffer = SystemInformation.HorizontalScrollBarHeight;
		}

		#endregion

		protected virtual void OnScrollbarScroll( object sender, ScrollEventArgs e )
		{
			this.saveScrollPosition();
		}

		#region >>> Inner Event Callback <<<

		protected override void OnPaint( PaintEventArgs e )
		{
			if ( base.Created == false )
				return;

			Graphics g = e.Graphics;
			Rectangle area = e.ClipRectangle;
			IntPtr display_hdc = g.GetHdc();

			if ( statusTest( EStatus.IsSelecting ) ) //$RIC, fast redraw while selecting
				area.Location = Point.Empty;

			GDI32.BitBlt( display_hdc, area.X, area.Y, area.Width, area.Height,
							  _canvas.Hdc, area.X, area.Y, ERasterOperations.SRCCOPY );

			g.ReleaseHdc();
			g = null;

			base.OnPaint( e );
		}

		protected override void OnMouseDown( MouseEventArgs e )
		{
			base.OnMouseDown( e );

			if ( e.Button == MouseButtons.Middle && statusTest( EInnerStatus.EnablePan ) )
			{
				_panStartX = _mouseX;
				_panStartY = _mouseY;
				this.statusOn( EInnerStatus.PanningStart );
				this.Cursor = _curGrab;
			}
		}

		protected override void OnMouseMove( MouseEventArgs e )
		{
			base.OnMouseMove( e );

			if ( statusTest( EInnerStatus.PanningStart ) )
			{
				this.statusOn( EInnerStatus.IsPanning );
				_panStartX = e.X;
				_panStartY = e.Y;
				_panningX = _hScroll.Value;
				_panningY = _vScroll.Value;
				this.statusOff( EInnerStatus.PanningStart );
				this.Cursor = _curGrabbing;
			}

			if ( this.statusTest( EInnerStatus.IsPanning ) )
			{
				this.doPanning( e.Location );
			}

		}

		protected override void OnMouseUp( MouseEventArgs e )
		{
			base.OnMouseUp( e );

			this.Cursor = Cursors.Default;

			if ( statusTest( EInnerStatus.IsPanning ) )
			{
				this.statusOff( EInnerStatus.IsPanning );
			}
			else if ( this.statusTest( EInnerStatus.PanningStart ) )
			{
				this.statusOff( EInnerStatus.PanningStart );
			}

			if ( e.Button == MouseButtons.Middle )
			{
				if ( this.Focused == false )
					this.Focus();
			}
		}

		protected override bool IsInputKey( Keys keyData )
		{
			if ( ( keyData & Keys.Right ) == Keys.Right || ( keyData & Keys.Left ) == Keys.Left || ( keyData & Keys.Up ) == Keys.Up || ( keyData & Keys.Down ) == Keys.Down )
				return true;

			if ( ( keyData & Keys.PageDown ) == Keys.PageDown || ( keyData & Keys.PageUp ) == Keys.PageUp )
				return true;

			return base.IsInputKey( keyData );
		}

		protected override void OnKeyDown( KeyEventArgs e )
		{
			base.OnKeyDown( e );

			int offset_x = 0;
			int offset_y = 0;
			switch ( e.KeyCode )
			{
				case Keys.Left:
				offset_x = -( e.Modifiers == Keys.Control ? _hScroll.LargeChange : _hScroll.SmallChange );
				break;
				case Keys.Right:
				offset_x = ( e.Modifiers == Keys.Control ? _hScroll.LargeChange : _hScroll.SmallChange );
				break;
				case Keys.Up:
				offset_y = -( e.Modifiers == Keys.Control ? _vScroll.LargeChange : _vScroll.SmallChange );
				break;
				case Keys.Down:
				offset_y = ( e.Modifiers == Keys.Control ? _vScroll.LargeChange : _vScroll.SmallChange );
				break;
				case Keys.PageDown:
				offset_y = _vScroll.LargeChange;
				break;
				case Keys.PageUp:
				offset_y = -_vScroll.LargeChange;
				break;
			}

			if ( offset_x != 0 || offset_y != 0 )
			{
				this.setScrollPosition( offset_x, offset_y, true );
				this.saveScrollPosition();
				this.Refresh();
			}
		}

		protected override void OnHandleCreated( EventArgs e )
		{
			base.OnHandleCreated( e );

			_handle = this.Handle;

			this.createGraphics( this.Width, this.Height, 3 );
			this.SetGraphicsSize( this.Width, this.Height );
		}

		protected override void OnBackColorChanged( EventArgs e )
		{
			base.OnBackColorChanged( e );
			_backColor = ( uint ) ColorTranslator.ToWin32( this.BackColor );
		}

		#endregion

		#region >>> Private Method <<<

		private void doPanning( Point mouseCur )
		{
			int x = ( mouseCur.X - _panStartX );
			int y = ( mouseCur.Y - _panStartY );

			if ( x == 0 && y == 0 )
				return;

			if ( _invertGrap )
			{
				x = _panningX - x;
				y = _panningY - y;
			}
			else
			{
				x = _panningX + x;
				y = _panningY + y;
			}

			this.setScrollPosition( x, y, false );
			OnScrollbarScroll( this, null );
		}

		#endregion

		#region >>> Status Test <<<

		internal bool statusTest( EInnerStatus item )
		{
			return ( ( _status & item ) == item );
		}

		internal void statusOn( EInnerStatus item, bool onOff )
		{
			_status = ( onOff ) ? ( _status | item ) : ( _status & ( _status ^ item ) );
		}

		internal void statusOn( EInnerStatus item )
		{
			_status |= item;
		}

		internal void statusOff( EInnerStatus item )
		{
			_status &= ( _status ^ item );
		}

		#endregion

		#region >>> Protected / Virtual Method <<<

		protected virtual bool innerOwnerDraw( Graphics bufferG, Rectangle area, ref TCanvasInfo srcCanvas )
		{
			return false;
		}

		protected virtual void createGraphics( int width, int height, int colors )
		{
			_canvas.Width = ( short ) width;
			_canvas.Height = ( short ) height;
			_canvas.Colors = ( byte ) colors;

			TCanvasInfo.CreateCanvasFromHwnd( _handle, ref _canvas );
		}

		protected void clear()
		{
			if ( _canvas.IsEmpty == false )
			{
				MGraphics.Clear( _canvas.Hdc, _backColor, _canvas.Width, _canvas.Height );
			}
		}

		protected EScrollChangeFlag setScrollPosition( int x, int y, bool isOffset )
		{
			EScrollChangeFlag flag = EScrollChangeFlag.None;
			Size scrn = this.ClientSize;
			int hscroll = _hScroll.Maximum;
			int vscroll = _vScroll.Maximum;
			int hvalue = _hScroll.Value;
			int vvalue = _vScroll.Value;

			if ( isOffset )
			{
				x += hvalue;
				y += vvalue;
			}

			x = ( x * hscroll ) / ( hscroll + scrn.Width );
			y = ( y * vscroll ) / ( vscroll + scrn.Height );

			if ( _hScroll.Visible )
			{
				if ( x <= _hScroll.Minimum )
					x = _hScroll.Minimum;
				else if ( x >= hscroll )
					x = hscroll;

				if ( x != hvalue )
				{
					_hScroll.Value = x;
					flag |= EScrollChangeFlag.Horizontal;
				}
			}

			if ( _vScroll.Visible )
			{
				if ( y <= _vScroll.Minimum )
					y = _vScroll.Minimum;
				else if ( y >= vscroll )
					y = vscroll;

				if ( y != vvalue )
				{
					_vScroll.Value = y;
					flag |= EScrollChangeFlag.Vertical;
				}
			}

			return flag;
		}

		[DllImport( "user32.dll", CharSet = CharSet.Auto, ExactSpelling = true )]
		static extern int SetScrollInfo( IntPtr hWnd, EScrollbar fnBar, ref SCROLLINFO si, bool redraw );

		[Flags]
		internal enum EScrollbarInfo : uint
		{
			/// <summary>
			/// Sets the scroll range to the value specified in the nMin and nMax members
			/// </summary>
			SIF_RANGE = 0x0001,

			/// <summary>
			/// Sets the scroll page to the value specified in the nPage member
			/// </summary>
			SIF_PAGE = 0x0002,

			/// <summary>
			/// Sets the scroll position to the value specified in the nPos member
			/// </summary>
			SIF_POS = 0x0004,

			/// <summary>
			/// Disables the scroll bar instead of removing it,
			/// if the scroll bar's new parameters make the scroll bar unnecessary.
			/// </summary>
			SIF_DISABLENOSCROLL = 0x0008,
			SIF_TRACKPOS = 0x0010,
			SIF_ALL = ( SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS )
		}

		[Flags]
		internal enum EScrollbar : uint
		{
			/// <summary>
			/// Sets the parameters of the window's standard horizontal scroll bar.
			/// </summary>
			SB_HORZ = 0,
			/// <summary>
			/// Sets the parameters of the window's standard vertical scroll bar.
			/// </summary>
			SB_VERT = 1,
			/// <summary>
			/// Sets the parameters of a scroll bar control.
			/// The hwnd must be the handle to the scroll bar control.
			/// </summary>
			SB_CTL = 2,
			SB_BOTH = 3,
		}

		[StructLayout( LayoutKind.Sequential )]
		internal struct SCROLLINFO
		{
			public int cbSize;
			public EScrollbarInfo fMask;
			public int nMin;
			public int nMax;
			public int nPage;
			public int nPos;
			public int nTrackPos;

			internal static readonly int Size = Marshal.SizeOf( typeof( SCROLLINFO ) );
		}

		protected void innerUpdateScrollbar( System.Windows.Forms.ScrollBar bar )
		{
			SCROLLINFO si = new SCROLLINFO();
			si.cbSize = SCROLLINFO.Size;
			si.fMask = EScrollbarInfo.SIF_ALL;
			si.nMin = bar.Minimum;
			si.nMax = bar.Maximum;
			si.nPage = bar.LargeChange;
			si.nPos = bar.Value;
			si.nTrackPos = 0;
			SetScrollInfo( bar.Handle, EScrollbar.SB_CTL, ref si, true );
		}

		protected void updateScrollBar( EScrollChangeFlag flags )
		{
			if ( flags == EScrollChangeFlag.None )
				return;

			if ( ( flags & EScrollChangeFlag.Horizontal ) == EScrollChangeFlag.Horizontal )
				innerUpdateScrollbar( _hScroll );

			if ( ( flags & EScrollChangeFlag.Vertical ) == EScrollChangeFlag.Vertical )
				innerUpdateScrollbar( _vScroll );

			this.saveScrollPosition();
		}

		protected void resetScrollPosition()
		{
			_hScroll.Value = _scrollX = 0;
			_vScroll.Value = _scrollY = 0;
		}

		protected void retoreScrollPosition()
		{
			this.setScrollPosition( _scrollX, _scrollY, false );
		}

		protected void saveScrollPosition()
		{
			_scrollX = _hScroll.Value;
			_scrollY = _vScroll.Value;
		}

		protected void setScrollChange( int smallX, int smallY, int largeX, int largeY )
		{
			_hScroll.SmallChange = smallX;
			_hScroll.LargeChange = largeX;

			_vScroll.SmallChange = smallY;
			_vScroll.LargeChange = largeY;
		}

		protected void getScrollbar( out int width, out int height )
		{
			width = ( _vScroll.Visible ) ? _vScroll.Width : 0;
			height = ( _hScroll.Visible ) ? _hScroll.Height : 0;
		}

		protected void setScrollBuffer( int buffer )
		{
			_scrollBuffer = SystemInformation.HorizontalScrollBarHeight;
			_scrollBuffer += buffer * 2;
		}

		#endregion

		public virtual void SetGraphicsSize( int width, int height )
		{
			_graphicsWidth = width;
			_graphicsHeight = height;

			if ( width > this.Width )
			{
				_hScroll.Maximum = width - this.Width + _scrollBuffer;
				_hScroll.Value = 0;
				_hScroll.Visible = true;
			}
			else
			{
				_hScroll.Visible = false;
			}

			if ( height > this.Height )
			{
				_vScroll.Maximum = height - this.Height + _scrollBuffer;
				_vScroll.Value = 0;
				_vScroll.Visible = true;
			}
			else
			{
				_vScroll.Visible = false;
			}

			this.statusOn( EInnerStatus.GraphicsSet );
		}
	}

	public class ScrollableImageBox : ScrollableDrawBox
	{
		public void SetImage( Bitmap image )
		{
			if ( _canvas.IsEmpty == false )
				TCanvasInfo.DestoryCanvas( ref _canvas );

			int colors = 3;

			if ( image.PixelFormat == PixelFormat.Format32bppRgb )
				colors = 4;
			else if ( image.PixelFormat == PixelFormat.Format8bppIndexed )
				colors = 1;

			base.createGraphics( image.Width, image.Height, colors );
			_canvas.SetImage( image );
			this.SetGraphicsSize( image.Width, image.Height );
		}
	}


}
