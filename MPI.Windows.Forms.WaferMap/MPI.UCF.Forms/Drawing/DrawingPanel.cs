using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel;

namespace MPI.Drawing.Forms
{
	public class GDIDrawPanel : Control
	{
		protected IntPtr _hwnd;
		protected WindowCanvas _canvas;
		protected ICanvas _theme;

		private Dictionary<string, Bitmap> _layerData;

		private bool _onShowing;

		protected override void OnHandleCreated( EventArgs e )
		{
			this.DoubleBuffered = false;

			base.OnHandleCreated( e );

			//if ( this.DesignMode )
			//    return;

			_hwnd = this.Handle;

			_canvas = new WindowCanvas( _hwnd );
			_canvas.Background = ( uint ) ColorTranslator.ToWin32( base.BackColor );

			_layerData = new Dictionary<string, Bitmap>();
		}

		protected override void OnPaint( PaintEventArgs e )
		{
			if ( this.DesignMode == false && _onShowing == false )
			{
				_onShowing = true;
				//				_canvas.Clip( e.ClipRectangle );
				this.Display();
				//				_canvas.ResetClip();
				_onShowing = false;
			}
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( _canvas != null )
				{
					_canvas.Dispose();
					_canvas = null;
				}
			}

			base.Dispose( disposing );
		}

		public void Clear()
		{
			if ( _theme != null )
				_theme.Clear();

			_canvas.Clear();

			_canvas.DisplayOn( _hwnd );
		}

		public void DrawTheme()
		{
			_canvas.DrawTheme();
		}

		public void Display()
		{
			_canvas.MergeBackgroundLayer();

			foreach ( KeyValuePair<string, Bitmap> item in _layerData )
			{
				if ( item.Value.Tag != null )
					_canvas.DrawImageTransparent( item.Value );
			}

			_canvas.DisplayOn( _hwnd );
		}

		[Browsable( false )]
		public ICanvas Canvas
		{
			get
			{
				return _canvas;
			}
		}

		[Browsable( false )]
		public ICanvas Theme
		{
			get
			{
				if ( _theme == null )
				{
					_theme = ( ICanvas ) _canvas.InitializeTheme();
					_theme.Clear();
				}

				return _theme;
			}
		}

		public override Color BackColor
		{
			get
			{
				if ( _canvas != null )
					return ColorTranslator.FromWin32( ( int ) _canvas.Background );

				return base.BackColor;
			}

			set
			{
				if ( _canvas != null )
					_canvas.Background = ( uint ) ColorTranslator.ToWin32( value );
				else
					base.BackColor = value;
			}
		}

		public void SetLayer( string name, Bitmap image )
		{
			if ( _layerData.ContainsKey( name ) == false )
			{
				if ( image != null )
					_layerData.Add( name, image );

				return;
			}

			// contained and image is null, remove it!
			if ( image == null )
			{
				_layerData.Remove( name );
				return;
			}

			Bitmap temp = _layerData[name];
			if ( image.Equals( temp ) )
				return;

			temp.Dispose();
			_layerData[name] = image;
		}

		public void EnableLayer( string name, bool enabled )
		{
			if ( _layerData.ContainsKey( name ) )
			{
				Bitmap item = _layerData[name];
				//$TRICKY
				item.Tag = ( enabled ) ? string.Empty : null;
				return;
			}
		}

		public void EnableAllLayer( bool enabled )
		{
			string tag = ( enabled ) ? string.Empty : null;
			foreach ( KeyValuePair<string, Bitmap> item in _layerData )
				item.Value.Tag = tag;
		}

		public void ActivateLayer( string name )
		{
			foreach ( KeyValuePair<string, Bitmap> item in _layerData )
			{
				if ( item.Key.Equals( name ) )
					item.Value.Tag = string.Empty;
				else
					item.Value.Tag = null;
			}
		}

		public bool IsEnabledLayer( string name )
		{
			foreach ( KeyValuePair<string, Bitmap> item in _layerData )
			{
				if ( item.Key.Equals( name ) && item.Value.Tag == string.Empty )
					return true;
			}
			return false;
		}

		public void ChangeSize( int width, int height )
		{
			this.Size = new Size( width, height );

			_canvas.Dispose();
			_canvas = null;
			_canvas = new WindowCanvas( _hwnd );
			_canvas.Background = ( uint ) ColorTranslator.ToWin32( base.BackColor );

			if ( _theme != null )
			{
				_theme.Dispose();
				_theme = null;
				_theme = ( ICanvas ) _canvas.InitializeTheme();
			}

		}

		public bool SaveToImage(Bitmap image)
		{
			return this._canvas.SaveToImage(image);
		}
	}
}
