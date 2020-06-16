using System;
using System.Drawing;

namespace MPI.UCF.Forms.Domain
{
	/// <summary>
	/// DieDrawingProperty class.
	/// </summary>
	public class DieDrawingProperty : System.ICloneable
	{
		private Color _ForeColor;
		private Color _BackColor;
		private Size _size;
		private float _LineWidth;

		/// <summary>
		/// Constructor.
		/// </summary>
		public DieDrawingProperty( Color foreColor, Color backColor, Size size, float LineWidth )
		{
			_ForeColor = foreColor;
			_BackColor = backColor;
			_size = size;
			_LineWidth = LineWidth;
		}

		/// <summary>
		/// Fore color.
		/// </summary>
		public Color ForeColor
		{
			get
			{
				return _ForeColor;
			}
			set
			{
				_ForeColor = value;
			}
		}

		/// <summary>
		/// Back color.
		/// </summary>
		public Color BackColor
		{
			get
			{
				return _BackColor;
			}
			set
			{
				_BackColor = value;
			}
		}

		/// <summary>
		/// Die size.
		/// </summary>
		public Size Size
		{
			get
			{
				return _size;
			}
			set
			{
				_size = value;
			}
		}

		/// <summary>
		/// Line width.
		/// </summary>
		public float LineWidth
		{
			get
			{
				return _LineWidth;
			}
			set
			{
				_LineWidth = value;
			}
		}

		#region ICloneable Members

		public object Clone()
		{
			return this.MemberwiseClone();
		}

		#endregion
	}
}
