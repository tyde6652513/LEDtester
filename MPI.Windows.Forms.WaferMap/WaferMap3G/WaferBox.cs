using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;

using MPI;
using MPI.Drawing;

namespace MPI.UCF.Forms.Domain
{
	public delegate Color GradeColorCallback( float value );
	public delegate void SelectionEventHandler( int rowStart, int colStart, int rowEnd, int colEnd );
	public delegate void ChipFocusEventHandler( int row, int column );

	public class WaferBox : System.Windows.Forms.Control
	{
		private const int MIN_RESTORE_TICK = 25;

		public const int MAX_ZOOM_SCALE = 24;
		public const int MIN_ZOOM_SCALE = 1;

		private long _lastMouseWheelTick;
		private long _lastDrawingTick;
		private bool _resizing;

		internal bool fIsDynamic;

		internal Graphics _doubleCanvas;

		internal int fStepScale;

		public event DrawChipEventHandler OnDrawChip;

		protected WaferMap3G _wfParent;

		#region >>> Private Field <<<
		private bool _selecting;
		private bool _selectStart;
		private Rectangle _selectArea;
		private long _lastRestoreTick;
		#endregion

		#region >>> Internal Field <<<

		internal Pen fSelectionPen;
		internal bool fIsSelectable;
		internal BoundaryNotifyHandler fOnAreaSelected;
		#endregion

		public WaferBox()
		{
			fStepScale = MIN_ZOOM_SCALE;

			_lastMouseWheelTick = HiTimer.Tick;
			_lastDrawingTick = _lastMouseWheelTick;
			_lastRestoreTick = _lastMouseWheelTick;

			_selectArea = default( Rectangle );
			fSelectionPen = new Pen( Color.White );

		}

		#region >>> Protected / Override Methode <<<
		protected override void OnHandleCreated( EventArgs e )
		{
			base.OnHandleCreated( e );

			_wfParent = ( ( WaferMap3G ) this.Parent.Parent );

			_doubleCanvas = Graphics.FromHwnd( this.Handle );
			_doubleCanvas.InterpolationMode = InterpolationMode.NearestNeighbor;
			_doubleCanvas.PixelOffsetMode = PixelOffsetMode.HighSpeed;
			_doubleCanvas.CompositingQuality = CompositingQuality.HighSpeed;
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( fSelectionPen != null )
					fSelectionPen.Dispose();
			}

			base.Dispose( disposing );

		}

		protected override void OnSizeChanged( EventArgs e )
		{
			base.OnSizeChanged( e );

			if ( this.Created )
			{
				if ( _doubleCanvas != null )
				{
					_doubleCanvas.Dispose();
					_doubleCanvas = null;
				}

				_doubleCanvas = Graphics.FromHwnd( this.Handle );
				_doubleCanvas.InterpolationMode = InterpolationMode.NearestNeighbor;
				_doubleCanvas.PixelOffsetMode = PixelOffsetMode.HighSpeed;
				_doubleCanvas.CompositingQuality = CompositingQuality.HighSpeed;
			}
		}

		protected override void OnPaint( PaintEventArgs e )
		{
			if ( e.ClipRectangle.IsEmpty == false || _resizing == false && _wfParent != null )
				_wfParent.DrawImage( e.Graphics );
		}

		protected override void OnMouseDown( MouseEventArgs e )
		{
			base.OnMouseDown( e );

			if ( fIsSelectable )
			{
				_selectArea.Location = e.Location;
				_selectArea.Width = 2;
				_selectArea.Height = 2;
				_selectStart = true;
			}
		}

		protected override void OnMouseMove( MouseEventArgs e )
		{
			base.OnMouseMove( e );

			if ( _selectStart == false )
				return;

			if ( e.Button != MouseButtons.Left )
			{
				_selectStart = false;
				return;
			}

			_selecting = true;

			if ( _selectArea.IsEmpty == false ) // repaint last selected area
			{
				this.innerRedraw();
			}

			_selectArea.Width = e.X - _selectArea.X;

			if ( _selectArea.Width < 0 )
			{
				_selectArea.Width = 2;
				return;
			}

			_selectArea.Height = e.Y - _selectArea.Y;
			if ( _selectArea.Height < 0 )
			{
				_selectArea.Height = 2;
				return;
			}

			float tick = HiTimer.Evaluate( _lastRestoreTick );
			if ( tick > MIN_RESTORE_TICK )
			{
				_doubleCanvas.DrawRectangle( fSelectionPen, _selectArea );

				_lastRestoreTick = HiTimer.Tick;
			}
		}

		protected override void OnMouseUp( MouseEventArgs e )
		{
			base.OnMouseUp( e );

			if ( fIsSelectable && _selecting )
			{
				_selecting = false;
				_selectStart = false;

				fOnAreaSelected.Invoke( _selectArea );

				_selectArea.Inflate( 5, 5 );
				this.Invalidate( _selectArea );

				_selectArea = Rectangle.Empty;
			}
		}

		//protected override void OnMouseDown( MouseEventArgs e )
		//{
		//   base.OnMouseDown( e );

		//   if ( e.Button == MouseButtons.Middle )
		//   {
		//      if ( this.Focused == false )
		//         this.Focus();
		//   }
		//}

		//protected override void OnMouseWheel( MouseEventArgs e )
		//{
		//   base.OnMouseWheel( e );

		//   if ( Control.ModifierKeys == Keys.Control )
		//   {
		//      if ( e.Delta < 0 )
		//         this.ZoomIn();
		//      else
		//         this.ZoomOut();

		//      return;
		//   }

		//}

		//protected override void OnMouseLeave( EventArgs e )
		//{
		//   base.OnMouseLeave( e );
		//   if ( this.Focused )
		//      this.Parent.Focus();
		//}

		#endregion

		private void innerOnDrawChip( Graphics g, Rectangle clip, int row, int col )
		{
			if ( OnDrawChip != null )
			{
				GraphicsState gs = g.Save();
				g.Clip = new Region( clip );
				OnDrawChip( g, clip, row, col );
				g.Restore( gs );
				gs = null;
			}
		}

		protected void innerRedraw()
		{
			_wfParent.DrawImage( _doubleCanvas );
		}

		#region >>> Public Method <<<

		public void ClearBackground()
		{
			if ( _doubleCanvas != null )
				_doubleCanvas.Clear( this.BackColor );
		}

		internal void innerResize( Size newSize )
		{
			_resizing = true;
			this.ClientSize = newSize;
			_resizing = false;
		}

		#endregion
	}

	public class WaferFocusableBox : WaferBox
	{
		private const int MOUSE_DRAG_RESOLUTION = 15;

		#region >>> Private Field <<<
		private Rectangle _focusChipRect;
		private bool _isChipClick;
		private bool _dragStart;
		private bool _dragging;

		private int _mouseMoveX;
		private int _mouseMoveY;
		private long _lastDragTick;
		private Pen _focusPen;
		#endregion

		public WaferFocusableBox()
		{
			_focusChipRect = new Rectangle( 0, 0, 1, 1 );
			_lastDragTick = HiTimer.Tick;

			_focusPen = new Pen( Color.Yellow );

		}

		#region >>> Internal Field <<<

		internal event ChipFocusEventHandler fOnChipClick;
		internal event ChipFocusEventHandler fOnChipFocus;
		internal bool fEnableDragToGo;
		internal bool fEnableFocus;
		internal bool fHasLastFocus;
		internal bool fUseChipSnap;
		internal Color fFocusColor;

		#endregion

		internal void innerFocusOn( int row, int column )
		{
			Rectangle screct = _focusChipRect;

			if ( fHasLastFocus )
			{
				this.innerRedraw();
			}

			fHasLastFocus = true;
			_focusChipRect.X = column;
			_focusChipRect.Y = row;

			screct = _focusChipRect;
			_wfParent.fCoord.MineToWorld( ref screct ); // to screen coord
			this.Invalidate( screct );
		}

		private bool testFoucsChip( Point wfXY, ref Point scrXY )
		{
			//_wfdraw.Transorm.MineToWorld( ref wfLocation );

			//int chip_scr_size = _wfdraw.Transorm.ScaleToWorld( 1 );

			//Rectangle scr_rect = new Rectangle( wfLocation, new Size( chip_scr_size, chip_scr_size ) );

			//return scr_rect.Contains( scrLocation );

			Rectangle scr_rect = new Rectangle( wfXY, new Size( 1, 1 ) );
			_wfParent.fCoord.MineToWorld( ref scr_rect );
			//int size = Math.Min( scr_rect.Width, scr_rect.Height );
			//scr_rect.Width = size;
			//scr_rect.Height = size;

			return scr_rect.Contains( scrXY );
		}

		internal bool evalFoucsChip( ref Point pntScrChip )
		{
			Point wf_pnt = pntScrChip;
			_wfParent.fCoord.WorldToMine( ref wf_pnt ); // screen point to wafer point

			if ( testFoucsChip( wf_pnt, ref pntScrChip ) )
			{
				pntScrChip = wf_pnt;
				return true;
			}

			Point pp = wf_pnt;
			pp.Offset( -1, -1 );
			if ( testFoucsChip( pp, ref pntScrChip ) )
			{
				pntScrChip = pp;
				return true;
			}

			pp = wf_pnt;
			pp.Offset( -1, 0 );
			if ( testFoucsChip( pp, ref pntScrChip ) )
			{
				pntScrChip = pp;
				return true;
			}

			pp = wf_pnt;
			pp.Offset( 0, -1 );
			if ( testFoucsChip( pp, ref pntScrChip ) )
			{
				pntScrChip = pp;
				return true;
			}

			pntScrChip = wf_pnt;
			return false;
		}

		#region >>> Protected / Override Method <<<

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( _focusPen != null )
					_focusPen.Dispose();
			}

			base.Dispose( disposing );
		}

		protected override void OnPaint( PaintEventArgs e )
		{
			base.OnPaint( e );

			if ( fHasLastFocus )
			{
				Point pos = _focusChipRect.Location;
				_wfParent.fCoord.MineToWorld( ref pos );

				e.Graphics.DrawRectangle( _focusPen, pos.X, pos.Y, _wfParent._chipScrSize.Width, _wfParent._chipScrSize.Height );
			}
		}

		protected override void OnMouseDown( MouseEventArgs e )
		{
			base.OnMouseDown( e );
			if ( fEnableFocus && e.Button == MouseButtons.Left )
			{
				_isChipClick = true;
				return;
			}

			_isChipClick = false;

			if ( fEnableDragToGo )
			{
				_dragStart = ( e.Button == MouseButtons.Right );
				_dragging = false;

				if ( _dragStart )
				{
					_mouseMoveX = e.X;
					_mouseMoveY = e.Y;
					this.Cursor = Cursors.Hand;
				}
			}
		}

		protected override void OnMouseClick( MouseEventArgs e )
		{
			base.OnMouseClick( e );

			if ( fEnableFocus == false || _isChipClick == false )
				return;

			Rectangle screct = _focusChipRect;

			if ( fHasLastFocus ) // restore last chip rectangle
			{
				base.innerRedraw();
			}

			// foucs on one chip
			Point chip_pos = e.Location;
			this.evalFoucsChip( ref chip_pos );

			_isChipClick = ( _wfParent._wfdb.Contains( chip_pos.Y, chip_pos.X ) ); 	// test if wafer data contains ( row, col )

			if ( _isChipClick == false )
				return;

			_focusChipRect.Location = chip_pos;

			#region >>> draw focus rectangle <<<
			screct = _focusChipRect;
			_wfParent.fCoord.MineToWorld( ref screct );

			//int chip = _wfParent.fChipSize;
			//_doubleCanvas.DrawRectangle( _focusPen, screct.X, screct.Y, chip, chip );

			_doubleCanvas.DrawRectangle( _focusPen, screct.X, screct.Y, _wfParent._chipScrSize.Width, _wfParent._chipScrSize.Height );
			#endregion

			fHasLastFocus = true;

			if ( fOnChipClick != null )
				fOnChipClick.Invoke( chip_pos.Y, chip_pos.X );

		}

		protected override void OnMouseMove( MouseEventArgs e )
		{
			base.OnMouseMove( e );

			if ( this.fOnChipFocus != null )
			{
				Point wfpt = e.Location;
				//if ( evalFoucsChip( ref wfpt ) == true && _wfdb.Contains( ref wfpt ) )
				if ( evalFoucsChip( ref wfpt ) )
				{
					this.fOnChipFocus.Invoke( wfpt.Y, wfpt.X );
				}
			}

			if ( _dragStart == false || e.Button != MouseButtons.Right )
			{
				_dragStart = false;
				return;
			}

			_dragging = true;

			if ( HiTimer.Evaluate( _lastDragTick ) > MOUSE_DRAG_RESOLUTION )
			{
				int scroll_x = ( _mouseMoveX - e.X );

				if ( scroll_x > 20 )
					scroll_x = 20;
				else if ( scroll_x < -20 )
					scroll_x = -20;
				else
					scroll_x = 0;

				int scroll_y = ( _mouseMoveY - e.Y );

				if ( scroll_y > 20 )
					scroll_y = 20;
				else if ( scroll_y < -20 )
					scroll_y = -20;
				else
					scroll_y = 0;

				_wfParent.innerScrollOffset( scroll_x, scroll_y );

				_mouseMoveX = e.X;
				_mouseMoveY = e.Y;

				_lastDragTick = HiTimer.Tick;

			}
		}

		protected override void OnMouseUp( MouseEventArgs e )
		{
			base.OnMouseUp( e );

			if ( _dragging == false || e.Button != MouseButtons.Right )
				return;

			this.Cursor = Cursors.Default;

			_dragging = false;
			_dragStart = false;
		}


		#endregion

		#region >>> Public Method <<<

		public void ClearFocus()
		{
			fHasLastFocus = false;
			_focusChipRect = new Rectangle( 0, 0, 1, 1 );
		}

		#endregion


	}
}
