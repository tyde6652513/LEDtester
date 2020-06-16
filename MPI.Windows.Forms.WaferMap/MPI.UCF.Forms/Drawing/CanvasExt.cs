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
			base._dcWidth = width;
			base._dcHeight = height;

			#region >>> Main DC <<<
			// The memory DC is initialized with a mono-chromatic 1x1 pixel bitmap by default.
			_drawDC = GDI32.CreateCompatibleDC( IntPtr.Zero );

			//IntPtr ptr = IntPtr.Zero;
			//offscr_dib = GDI32.CreateDIBSection( _drawDC, ref bmi, 0, out ptr, 0, 0 );
			IntPtr offscr_dib = GDI32.CreateDIBitmap( _drawDC, ref bmi.Header, ECreateBitmap.CBM_NULL,
				IntPtr.Zero, ref bmi, EDIBColor.DIB_RGB_COLORS );

			_oldBMP = GDI32.SelectObject( _drawDC, offscr_dib );

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
			MGraphics.DrawUnscaled( _bufferDC, _dcWidth, _dcHeight, _drawDC );

			if ( _themeUsed )
				MGraphics.DrawLayer( _bufferDC, _dcWidth, _dcHeight, _themeDC, _background );

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

			MGraphics.DrawAutoScaled( hdc, rect.Width, rect.Height, _bufferDC, _dcWidth, _dcHeight );

			USER32.ReleaseDC( target, hdc );
		}

		public override ICanvas InitializeTheme()
		{
			// The memory DC is initialized with a mono-chromatic 1x1 pixel bitmap by default.
			_themeDC = GDI32.CreateCompatibleDC( IntPtr.Zero );

			TBITMAPINFO bmi;
			MGraphics.InitBitmapInfo( 3, _dcWidth, _dcHeight, out bmi );

			// the next step will be to allocate a buffer for the memory DC to write to.
			IntPtr outptr = IntPtr.Zero;
			IntPtr temp_dib = GDI32.CreateDIBSection( _themeDC, ref bmi, EDIBColor.DIB_RGB_COLORS, out outptr, IntPtr.Zero, 0 );

			// mono color
			//_oldThemeDIB = GDI32.CreateDIBitmap( _themeDC, ref bmi.Header, ECreateBitmap.CBM_NULL, IntPtr.Zero, ref bmi, EDIBColor.DIB_RGB_COLORS );

			// associate it with the memory DC with a call to SelectObject.
			_oldThemeBMP = GDI32.SelectObject( _themeDC, temp_dib );

			GDI32.SetBkColor( _themeDC, _background );

			_themeUsed = true;

			return new MCanvas( _themeDC, _dcWidth, _dcHeight, _background );		
		}

		public bool SaveToImage(Bitmap image)
		{
			Graphics g = null;

			try
			{
				g = Graphics.FromImage(image);
			}
			catch
			{
				return false;
			}

			IntPtr hdc = g.GetHdc();
			
			GDI32.BitBlt(hdc, 0, 0, _dcWidth, _dcHeight,
				_bufferDC, 0, 0, ERasterOperations.SRCCOPY);

			g.ReleaseHdc();
			g.Dispose();
			g = null;

			return true;
		}

		public void RotateAt(float angle, PointF point)
		{
			Matrix m = _trsf.ToTransform();
			m.RotateAt(angle, point);
			IntPtr eles = Marshal.UnsafeAddrOfPinnedArrayElement(m.Elements, 0);
			GDI32.SetGraphicsMode(_drawDC, EGraphicsMode.GM_ADVANCED);
			GDI32.SetWorldTransform(_drawDC, eles);
			eles = IntPtr.Zero;
			m.Dispose();
		}

	}

	public class WindowCanvas : GDICanvas
	{
		private IntPtr _hostHwnd;

		public WindowCanvas( IntPtr hwnd )
		{
			Rectangle rect;
			USER32.GetClientRect( hwnd, out rect );

			base._dcWidth = rect.Width;
			base._dcHeight = rect.Height;


			IntPtr hdc = USER32.GetWindowDC( hwnd );
			//// the next step will be to allocate a buffer for the memory DC to write to.	

			#region >>> CreateBitmap M1 <<<
			//TBITMAPINFO bmi;
			//MGraphics.InitBitmapInfo( 3, _dcWidth, _dcHeight, out bmi );
			//_offscrBMP = GDI32.CreateDIBitmap( hdc, ref bmi.Header, ECreateBitmap.CBM_NULL,
			//   IntPtr.Zero, ref bmi, EDIBColor.DIB_RGB_COLORS );

			#endregion

			#region >>> CreateBitmap M2 <<<
			//TBITMAPINFO bmi;
			//MGraphics.InitBitmapInfo( 3, _dcWidth, _dcHeight, out bmi );

			//IntPtr ptr = IntPtr.Zero;
			//_offscrBMP = GDI32.CreateDIBSection( _drawDC, ref bmi, EDIBColor.DIB_RGB_COLORS, out ptr, IntPtr.Zero, 0 );

			#endregion

			#region >>> CreateBitmap M3 (fastest) <<<

			IntPtr offscr_bmp = GDI32.CreateCompatibleBitmap( hdc, _dcWidth, _dcHeight );
			if ( offscr_bmp == IntPtr.Zero )
			{
				Console.Error.WriteLine( "[WindowCanvas::Constructor] Out of memory" );
			}
			#endregion

			#region >>> Main DC <<<
			// associate it with the memory DC with a call to SelectObject.
			_drawDC = GDI32.CreateCompatibleDC( IntPtr.Zero );
			_oldBMP = GDI32.SelectObject( _drawDC, offscr_bmp );

			#endregion

			#region >>> Buffer DC <<<

			IntPtr buffer_bmp = GDI32.CreateCompatibleBitmap( hdc, _dcWidth, _dcHeight );
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

				MGraphics.DrawLayer( hdc, _dcWidth, _dcHeight, _themeDC, _background );

				USER32.ReleaseDC( _hostHwnd, hdc );
			}
		}

		public uint Background
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

			MGraphics.SetLayerDataAutoScaled( _bufferDC, _dcWidth, _dcHeight, bd.Scan0, ref bmi, _background );

			image.UnlockBits( bd );
			bd = null;
		}
	}

}
