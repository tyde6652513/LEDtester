using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.IO;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MPI.Drawing;

namespace MPI.UCF.Forms.Domain
{
	public interface IGradeRender
	{
		Color GetGradeColor( float value );
		Color GetGradeColor( int row, int column );
	}

	public abstract class GradeRenderBase : IGradeRender, IDisposable
	{
		protected int _gradeMax;

		public GradeRenderBase()
		{
			MinLevelColor = Color.Red;
			MaxLevelColor = Color.Blue;

			MinLevelValue = 0;
			MaxLevelValue = 100;
		}

		public abstract EGradeColorMethod GradeMethod
		{
			get;
			set;
		}

		public Color MaxLevelColor;
		public Color MinLevelColor;

		public float MaxLevelValue;
		public float MinLevelValue;

		protected virtual int evalGrade( float value )
		{
			if ( float.IsNaN( value ) || value < this.MinLevelValue || value > this.MaxLevelValue ) // not in range
				return -1;

			float range = this.MaxLevelValue - this.MinLevelValue;
			float offset = value - this.MinLevelValue;
			int grade = ( int ) ( offset * ( float ) _gradeMax / range ) - 1;

			if ( grade < 0 )
				grade = 0;

			return grade;
		}

		public abstract Color GetGradeColor( float value );

		public abstract Color GetGradeColor( int row, int column );

		public abstract void OwnerDraw( Graphics g, Point ptDraw, int cellSize, int row, int column );

		public static void DrawInvalid( Graphics g, int x, int y, int cellSize, EDieStatus status )
		{
			switch ( status )
			{
				case EDieStatus.Bad:
				{
					// draw bad die pattern
					Pen pen = Pens.White;
					float d = cellSize / 4f;
					g.DrawLine( pen, x + d, y, x, y + d );
					g.DrawLine( pen, x + 2 * d, y, x, y + 2 * d );
					g.DrawLine( pen, x + 3 * d, y, x, y + 3 * d );
					g.DrawLine( pen, x + 4 * d, y, x, y + 4 * d );

					pen = null;
				}
				break;
				case EDieStatus.Erased:
				{
					g.FillRectangle( Brushes.Black, x, y, cellSize, cellSize );
				}
				break;
				case EDieStatus.Inked:
				{
					g.FillRectangle( Pens.White.Brush, x + cellSize / 8, y + cellSize / 8, cellSize / 3, cellSize / 3 );
				}
				break;
				case EDieStatus.Marked:
				{
					// draw marked pattern
					float w, h, offsetX, offsetY;

					if ( cellSize >= 5 )
					{
						w = Math.Max( cellSize / 5, 1f );
						offsetX = 2 * w;
					}
					else if ( cellSize >= 3 )
					{
						w = Math.Max( cellSize / 5, 1f );
						offsetX = 1;
					}
					else
					{
						return;
					}

					if ( cellSize >= 5 )
					{
						h = Math.Max( cellSize / 5, 1f );
						offsetY = 2 * h;
					}
					else if ( cellSize >= 3 )
					{
						h = Math.Max( cellSize / 5, 1f );
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
					Point p1 = new Point( ( x + 1 ), ( y + 1 ) );
					Point p2 = new Point( ( x + cellSize - 1 ), ( y + cellSize - 1 ) );

					g.DrawLine( Pens.Red, p1, p2 );
					g.DrawLine( Pens.Red, p1.X, p2.Y, p2.X, p1.Y );
				}
				break;
				case EDieStatus.NotExist:
				{
					// draw missing pattern
					Point p1 = new Point( x, y );
					Point p2 = new Point( ( x + cellSize ), ( y + cellSize ) );

					g.DrawLine( Pens.White, p1, p2 );
					g.DrawLine( Pens.White, p1.X, p2.Y, p2.X, p1.Y );

				}
				break;
				case EDieStatus.Picked:
				{
					// draw missing pattern
					float dx = cellSize / 2;
					float dy = cellSize / 2;

					Pen pen = Pens.White;

					g.DrawLine( pen, x + dx, y + 1, x + dx, y + cellSize - 2 );
					g.DrawLine( pen, x + 1, y + dx, x + cellSize - 2, y + dx );
				}
				break;
				case EDieStatus.Skiped:
				{
					g.DrawRectangle( Pens.White, x + ( cellSize / 6 ), y + ( cellSize / 6 ), cellSize * 4 / 6, cellSize * 4 / 6 );
				}
				break;
			}
		}

		public virtual void Dispose()
		{

		}
	}

	/// <summary>
	/// Web 125 safe color
	/// </summary>
	public class WebColorRender : GradeRenderBase
	{
		private static Color[] _WebColors;
		private ICellData _cellData;
		public const int WEB_COLOR_GRADE = 125;

		static WebColorRender()
		{
			_WebColors = new Color[WEB_COLOR_GRADE];

			int idx = 0;
			for ( int b = 0x33; b <= 0xff; b += 0x33 )
			{
				for ( int r = 0x33; r <= 0xff; r += 0x33 )
				{
					for ( int g = 0x33; g <= 0xff; g += 0x33 )
						_WebColors[idx++] = Color.FromArgb( r, g, b );
				}
			}
		}

		public override EGradeColorMethod GradeMethod
		{
			get
			{
				return EGradeColorMethod.WebSafeColor;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public WebColorRender( ICellData cellData )
		{
			_gradeMax = WEB_COLOR_GRADE;
			_cellData = cellData;
		}

		public override Color GetGradeColor( float value )
		{
			int grade = base.evalGrade( value );
			if ( grade < 0 )
				return Color.Empty;

			return _WebColors[grade];
		}

		public override Color GetGradeColor( int row, int column )
		{
			float value = _cellData[row, column];
			return this.GetGradeColor( value );
		}

		public override void OwnerDraw( Graphics g, Point ptDraw, int size, int row, int column )
		{

		}

		public static Color GetColor( int grade )
		{
			grade %= WEB_COLOR_GRADE;
			return _WebColors[grade];
		}
	}

	public abstract class BlendColorRender : GradeRenderBase, IDisposable
	{
		private Bitmap _blendImage;
		private EGradeColorMethod _gradeMethod;
		private LinearGradientBrush _brush;

		public BlendColorRender( EGradeColorMethod method )
		{
			if ( method != EGradeColorMethod.Blend2Color && method != EGradeColorMethod.KBlendColor )
				throw new NotSupportedException( method.ToString() );

			base._gradeMax = 1024;
			_gradeMethod = method;
		}

		private void initializeBrush()
		{
			ColorBlend blend = new ColorBlend();

			if ( this.GradeMethod == EGradeColorMethod.Blend2Color )
			{
				blend.Colors = new Color[] { base.MinLevelColor, base.MaxLevelColor };
				blend.Positions = new float[] { 0.0f, 1.0f };
			}
			else //if ( _gradeMethod == EGradeColorMethod.KBlendColor )
			{
				base.MinLevelColor = Color.Red;
				base.MaxLevelColor = Color.Blue;
				blend.Colors = new Color[] { Color.Red, Color.Orange, Color.Yellow, Color.YellowGreen, Color.Cyan, Color.Purple, Color.Blue };
				blend.Positions = new float[] { 0.0f, 0.17f, 0.33f, 0.50f, 0.66f, 0.83f, 1.0f };
			}

			_brush = new LinearGradientBrush( new Rectangle( 0, 0, _gradeMax, 2 ),
				base.MinLevelColor, base.MaxLevelColor, LinearGradientMode.Horizontal );

			_brush.WrapMode = WrapMode.Tile;
			_brush.GammaCorrection = true;
			_brush.InterpolationColors = blend;
		}

		public override EGradeColorMethod GradeMethod
		{
			get
			{
				return _gradeMethod;
			}

			set
			{
				if ( value == EGradeColorMethod.WebSafeColor )
					throw new NotSupportedException();

				_gradeMethod = value;
			}
		}

		public override void Dispose()
		{
			if ( _blendImage != null )
			{
				_blendImage.Dispose();
				_blendImage = null;
			}

			if ( _brush != null )
			{
				_brush.Dispose();
				_brush = null;
			}
		}

		public override Color GetGradeColor( float value )
		{
			int grade = base.evalGrade( value );

			if ( grade < 0 )
				return Color.Empty;

			if ( _brush == null )
				this.initializeBrush();

			if ( _blendImage == null )
			{
				_blendImage = new Bitmap( _gradeMax, 1 );
				using ( Graphics g = Graphics.FromImage( _blendImage ) )
					g.FillRectangle( _brush, new Rectangle( 0, 0, _gradeMax, 1 ) );
			}

			return _blendImage.GetPixel( grade, 0 );
		}

		public override void OwnerDraw( Graphics g, Point ptDraw, int cellSize, int row, int column )
		{

		}

		public void DrawKBlendValue( Graphics g, Font font, int posY, int width )
		{
			float step_value = ( base.MaxLevelValue - base.MinLevelValue ) / 6.0f;
			float step_width = width / 6.5f;
			float mark_width = g.MeasureString( "00.000", font ).Width;

			float pos_x = 1;
			float value = this.MinLevelValue;
			for ( int i = 0; i < 6; i++ )
			{
				g.DrawString( value.ToString( "00.00" ), font, Pens.White.Brush, pos_x, posY );
				value += step_value;
				pos_x += step_width;
			}

			pos_x = width - mark_width;
			g.DrawString( base.MaxLevelValue.ToString( "00.00" ), font, Pens.White.Brush, pos_x, posY );
		}

		public void PaintBlend( Graphics g, Rectangle rect )
		{
			if ( _brush == null )
				this.initializeBrush();

			g.FillRectangle( _brush, rect );
		}
	}

}