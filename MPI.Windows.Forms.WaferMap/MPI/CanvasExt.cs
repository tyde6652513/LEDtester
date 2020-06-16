using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;

namespace MPI.Drawing
{
	public class GDICanvas : MCanvas
	{
		internal IntPtr _themeDC;
		protected IntPtr _oldThemeBMP;

		protected IntPtr _bufferDC;
		internal IntPtr _oldBufferBMP;

		protected bool _themeUsed;

		internal GDICanvas()
		{

		}

		internal GDICanvas( int width, int height, ushort channel )
		{
			TBITMAPINFO bmi;
			MGraphics.InitBitmapInfo( channel, width, height, out bmi );

			this.createCanvas( width, height, ref bmi );
		}

		protected void createCanvas( int width, int height, ref TBITMAPINFO bmi )
		{
			base._canvasWidth = width;
			base._canvasHeight = height;

			#region >>> Main DC <<<
			// The memory DC is initialized with a mono-chromatic 1x1 pixel bitmap by default.
			_canvasDC = GDI32.CreateCompatibleDC( IntPtr.Zero );

			//IntPtr ptr = IntPtr.Zero;
			//offscr_dib = GDI32.CreateDIBSection( _canvasDC, ref bmi, 0, out ptr, 0, 0 );
			IntPtr offscr_dib = GDI32.CreateDIBitmap( _canvasDC, ref bmi.Header, ECreateBitmap.CBM_NULL,
				IntPtr.Zero, ref bmi, EDIBColor.DIB_RGB_COLORS );

			_oldCanvasBMP = GDI32.SelectObject( _canvasDC, offscr_dib );

			#endregion

			#region >>> Buffer DC <<<
			_bufferDC = GDI32.CreateCompatibleDC( IntPtr.Zero );

			//ptr = IntPtr.Zero;
			//buffer_bmp = GDI32.CreateDIBSection( _bufferDC, ref bmi, 0, out ptr, 0, 0 );
			IntPtr buffer_bmp = GDI32.CreateDIBitmap( _bufferDC, ref bmi.Header, ECreateBitmap.CBM_NULL,
				IntPtr.Zero, ref bmi, EDIBColor.DIB_RGB_COLORS );

			_oldBufferBMP = GDI32.SelectObject( _bufferDC, buffer_bmp );

			#endregion
		}

		public override void Dispose()
		{
			if ( _themeDC != IntPtr.Zero )
			{
				if ( _oldThemeBMP != IntPtr.Zero )
				{
					GDI32.DeleteObject( GDI32.SelectObject( _themeDC, _oldThemeBMP ) );
					_oldThemeBMP = IntPtr.Zero;
				}

				GDI32.DeleteDC( _themeDC );
				_themeDC = IntPtr.Zero;
			}

			if ( _bufferDC != IntPtr.Zero )
			{
				if ( _oldBufferBMP != IntPtr.Zero )
				{
					GDI32.DeleteObject( GDI32.SelectObject( _bufferDC, _oldBufferBMP ) );
					_oldBufferBMP = IntPtr.Zero;
				}

				GDI32.DeleteDC( _bufferDC );
				_bufferDC = IntPtr.Zero;
			}


			base.Dispose();
		}

		public void MergeBackgroundLayer()
		{
			MGraphics.DrawDC( _bufferDC, _canvasWidth, _canvasHeight, _canvasDC );

			if ( _themeUsed )
				MGraphics.DrawTheme( _bufferDC, _canvasWidth, _canvasHeight, _themeDC, _background );

		}
		/// <summary>
		/// flip background DC to target window
		/// </summary>
		/// <param name="targetWindow"></param>
		public void DisplayOn( IntPtr target )
		{
			Rectangle rect;
			USER32.GetClientRect( target, out rect );

			IntPtr hdc = USER32.GetWindowDC( target );

			if ( rect.Width == _canvasWidth && rect.Height == _canvasHeight )
				MGraphics.DrawDC( hdc, _canvasWidth, _canvasHeight, _bufferDC );
			else
				MGraphics.StretchDrawDC( hdc, rect.Width, rect.Height, _bufferDC, _canvasWidth, _canvasHeight );


			USER32.ReleaseDC( target, hdc );
		}

		public override ICanvas InitializeTheme()
		{
			// The memory DC is initialized with a mono-chromatic 1x1 pixel bitmap by default.
			_themeDC = GDI32.CreateCompatibleDC( IntPtr.Zero );

			TBITMAPINFO bmi;
			MGraphics.InitBitmapInfo( 3, _canvasWidth, _canvasHeight, out bmi );

			// the next step will be to allocate a buffer for the memory DC to write to.
			IntPtr outptr = IntPtr.Zero;
			IntPtr temp_dib = GDI32.CreateDIBSection( _themeDC, ref bmi, EDIBColor.DIB_RGB_COLORS, out outptr, IntPtr.Zero, 0 );

			// mono color
			//_oldThemeDIB = GDI32.CreateDIBitmap( _themeDC, ref bmi.Header, ECreateBitmap.CBM_NULL, IntPtr.Zero, ref bmi, EDIBColor.DIB_RGB_COLORS );

			// associate it with the memory DC with a call to SelectObject.
			_oldThemeBMP = GDI32.SelectObject( _themeDC, temp_dib );

			GDI32.SetBkColor( _themeDC, _background );

			_themeUsed = true;

			return new MCanvas( _themeDC, _canvasWidth, _canvasHeight, _background );		
		}
	}

	public class ImageCanvas : GDICanvas
	{
		internal ImageCanvas( Bitmap image )
		{
			TBITMAPINFO bmi;
			if ( MGraphics.InitBitmapInfo( image, out bmi ) == false )
				throw new ArgumentException( "Unsupported Format", "image" );

			base.createCanvas( image.Width, image.Height, ref bmi );

			MGraphics.DrawImage( _canvasDC, image, ref bmi );

			// associate it with the memory DC with a call to SelectObject.
			//GDI32.SelectObject( _canvasDC, _offscrDIB );
		}
	}

	public class WindowCanvas : GDICanvas
	{
		private IntPtr _hostHwnd;

		public WindowCanvas( IntPtr hwnd )
		{
			Rectangle rect;
			USER32.GetClientRect( hwnd, out rect );

			base._canvasWidth = rect.Width;
			base._canvasHeight = rect.Height;


			IntPtr hdc = USER32.GetWindowDC( hwnd );
			//// the next step will be to allocate a buffer for the memory DC to write to.	

			#region >>> CreateBitmap M1 <<<
			//TBITMAPINFO bmi;
			//MGraphics.InitBitmapInfo( 3, _canvasWidth, _canvasHeight, out bmi );
			//_offscrBMP = GDI32.CreateDIBitmap( hdc, ref bmi.Header, ECreateBitmap.CBM_NULL,
			//   IntPtr.Zero, ref bmi, EDIBColor.DIB_RGB_COLORS );

			#endregion

			#region >>> CreateBitmap M2 <<<
			//TBITMAPINFO bmi;
			//MGraphics.InitBitmapInfo( 3, _canvasWidth, _canvasHeight, out bmi );

			//IntPtr ptr = IntPtr.Zero;
			//_offscrBMP = GDI32.CreateDIBSection( _canvasDC, ref bmi, EDIBColor.DIB_RGB_COLORS, out ptr, IntPtr.Zero, 0 );

			#endregion

			#region >>> CreateBitmap M3 (fastest) <<<

			IntPtr offscr_bmp = GDI32.CreateCompatibleBitmap( hdc, _canvasWidth, _canvasHeight );
			if ( offscr_bmp == IntPtr.Zero )
			{
				Console.Error.WriteLine( "[WindowCanvas::Constructor] Out of memory" );
			}
			#endregion

			#region >>> Main DC <<<
			// associate it with the memory DC with a call to SelectObject.
			_canvasDC = GDI32.CreateCompatibleDC( IntPtr.Zero );
			_oldCanvasBMP = GDI32.SelectObject( _canvasDC, offscr_bmp );

			#endregion

			#region >>> Buffer DC <<<

			IntPtr buffer_bmp = GDI32.CreateCompatibleBitmap( hdc, _canvasWidth, _canvasHeight );
			// associate it with the memory DC with a call to SelectObject.
			_bufferDC = GDI32.CreateCompatibleDC( IntPtr.Zero );
			_oldBufferBMP = GDI32.SelectObject( _bufferDC, buffer_bmp );

			#endregion

			USER32.ReleaseDC( hwnd, hdc );

			_hostHwnd = hwnd;
		}

		/// <summary>
		/// Flip background DC to screen
		/// </summary>
		public void Display()
		{
			if ( _hostHwnd != IntPtr.Zero )
			{
				base.DisplayOn( _hostHwnd );
			}
		}

		public void DrawTheme()
		{
			if ( _themeUsed )
			{
				IntPtr hdc = USER32.GetWindowDC( _hostHwnd );

				MGraphics.DrawTheme( hdc, _canvasWidth, _canvasHeight, _themeDC, _background );

				USER32.ReleaseDC( _hostHwnd, hdc );
			}
		}

		public int Background
		{
			get
			{
				return _background;
			}

			set
			{
				_background = value;
			}
		}

		public void DrawImageTransparent( Bitmap image )
		{
			TBITMAPINFO bmi;
			if ( MGraphics.InitBitmapInfo( image, out bmi ) == false )
				throw new ArgumentException( "Unsupported Format", "image" );

			BitmapData bd = image.LockBits( new Rectangle( Point.Empty, image.Size ), ImageLockMode.ReadOnly, image.PixelFormat );

			MGraphics.DrawImageTransparent( _bufferDC, _canvasWidth, _canvasHeight,
				bd.Scan0, ref bmi, base._background );

			image.UnlockBits( bd );
			bd = null;

		}
	}

}
