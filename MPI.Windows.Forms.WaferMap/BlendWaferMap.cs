using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MPI.Windows.Forms
{
	public class BlendWaferMap : MPI.Windows.Forms.DynWaferMap
	{
		const int DEFAULT_COLOR_LEVEL = 128;

		#region >>> Private Field <<<

		private WaferDatabase fWaferDatabase;

		private float fDieDimRatio = 1.0f; //W:H = 1:1

		private Color fMaxColor;
		private Color fMinColor;
		private Label lblBlend;

		private int fColorLevel;

		#endregion

		public BlendWaferMap()
			: base()
		{
			InitializeComponent();

			fColorLevel = DEFAULT_COLOR_LEVEL;

			fMaxColor = Color.White;
			fMinColor = Color.Gray;
		}

		#region >>> Public Property / Event <<<

		public float DimensionRatio
		{
			get
			{
				return this.fDieDimRatio;
			}

			///Ratio Range: 1:5(0.1) ~ 5:1(10)
			set
			{
				if ( value < 0.2 || value > 5 )
					return;

				this.fDieDimRatio = value;

				Size size = new Size( DEFAULT_DIE_SIZE, ( int ) ( ( float ) DEFAULT_DIE_SIZE * fDieDimRatio ) );

				Console.WriteLine( "[Dimension: {0:F2}, W:{1}, H:{2} ]", value, size.Width, size.Height );

				fDieProperty = new DieDrawingProperty( Color.White, Color.Red, size, 1.0f );
			}
		}

		public override float MaxLevelValue
		{
			get
			{
				return this.fMaxValue;
			}

			set
			{
				if ( this.fMaxValue == value )
					return;

				this.fMaxValue = value;
				this.lblBlend.Invalidate();
			}
		}

		public override float MinLevelValue
		{
			get
			{
				return this.fMinValue;
			}

			set
			{
				if ( this.fMinValue == value )
					return;

				this.fMinValue = value;
				this.lblBlend.Invalidate();
			}
		}

		public Color MaxLevelColor
		{
			get
			{
				return this.fMaxColor;
			}
			set
			{
				if ( this.fMaxColor == value )
					return;

				this.fMaxColor = value;

				lblBlend.Invalidate();
			}
		}

		public Color MinLevelColor
		{
			get
			{
				return this.fMinColor;
			}
			set
			{
				if ( this.fMinColor == value )
					return;

				this.fMinColor = value;

				lblBlend.Invalidate();
			}
		}

		public int ColorLevel
		{
			get
			{
				return this.fColorLevel;
			}
			set
			{
				if ( this.fColorLevel == value )
					return;

				this.fColorLevel = value;
			}
		}

		#endregion

		#region >>> Private Method <<<

		private float fColorStepR;
		private float fColorStepG;
		private float fColorStepB;
		private float fStepRatio;

		protected override Color getGradeGolor( float value )
		{
			if ( value < fMinValue || value > fMaxValue )
				return Color.Empty;

			int level = ( int ) ( fStepRatio * ( value - fMinValue ) );

			int[] color = new int[]{ ( int ) ( fMinColor.R + this.fColorStepR * ( level ) ),
			 ( int ) ( fMinColor.G + this.fColorStepG * ( level ) ),
			 ( int ) ( fMinColor.B + this.fColorStepB * ( level ) )};

			return Color.FromArgb( color[0], color[1], color[2] );
		}

		private void InitializeComponent()
		{
			this.lblBlend = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblBlend
			// 
			this.lblBlend.BackColor = System.Drawing.Color.Transparent;
			this.lblBlend.Font = new System.Drawing.Font( "Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 0 ) ) );
			this.lblBlend.Location = new System.Drawing.Point( 3, 367 );
			this.lblBlend.Name = "lblBlend";
			this.lblBlend.Size = new System.Drawing.Size( 420, 12 );
			this.lblBlend.TabIndex = 0;
			this.lblBlend.Paint += new System.Windows.Forms.PaintEventHandler( this.lblBlend_Paint );
			// 
			// BlendWaferMap
			// 
			this.Controls.Add( this.lblBlend );
			this.DoubleBuffered = true;
			this.MaxScale = 5;
			this.MinScale = 5;
			this.Name = "BlendWaferMap";
			this.SizeChanged += new System.EventHandler( this.BlendWaferMap_SizeChanged );
			this.Controls.SetChildIndex( this.lblBlend, 0 );
			this.ResumeLayout( false );

		}

		#endregion

		#region >>> Protected / Overrided <<<

		#endregion

		#region >>> Public Method <<<
		/// <summary>
		/// R G B ªº»¼¼W­È
		/// </summary>
		public void CalcColorStep()
		{
			this.fColorStepR = ( float ) ( fMaxColor.R - fMinColor.R ) / ( float ) fColorLevel;
			this.fColorStepG = ( float ) ( fMaxColor.G - fMinColor.G ) / ( float ) fColorLevel;
			this.fColorStepB = ( float ) ( fMaxColor.B - fMinColor.B ) / ( float ) fColorLevel;

			this.fStepRatio = ( float ) this.fColorLevel / ( float ) ( this.fMaxValue - this.fMinValue );
		}

		#endregion

		private void lblBlend_Paint( object sender, PaintEventArgs e )
		{
			if ( this.ClientSize.Height <= 50 || this.ClientSize.Width <= 50 )
				return;

			LinearGradientBrush brush = new LinearGradientBrush( lblBlend.ClientRectangle,
				this.fMinColor, this.fMaxColor, LinearGradientMode.Horizontal );

			e.Graphics.FillRectangle( brush, lblBlend.ClientRectangle );

			e.Graphics.DrawString( fMinValue.ToString( "#0.000" ), lblBlend.Font,
				Pens.White.Brush, 4, 0 );

			string min_str = fMaxValue.ToString( "#0.000" );
			int offset = ( int ) e.Graphics.MeasureString( min_str, lblBlend.Font ).Width;

			e.Graphics.DrawString( fMaxValue.ToString( "#0.000" ), lblBlend.Font,
				Pens.White.Brush, lblBlend.ClientSize.Width - offset, 0 );
		}

		private void BlendWaferMap_SizeChanged( object sender, EventArgs e )
		{
			if ( this.ClientSize.Height <= 100 )
			{
				this.Size = new Size( this.Width, 100 );
				return;
			}

			if ( this.ClientSize.Height <= 100 )
			{
				this.Size = new Size( this.Height, 100 );
				return;
			}

			Rectangle rect = new Rectangle( 3, this.ClientSize.Height - 33,
				this.ClientSize.Width - 24, 12 );

			lblBlend.Location = rect.Location;
			lblBlend.Size = rect.Size;
		}
	}
}
