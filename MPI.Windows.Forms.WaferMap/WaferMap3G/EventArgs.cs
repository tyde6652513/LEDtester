using System;
using System.Drawing;
using System.Windows.Forms;

namespace MPI.UCF.Forms.Domain
{


	/// <summary>
	/// Finish selection event argument.
	/// </summary>
	public class FinishSelectionEventArgs : EventArgs
	{
		private Rectangle _window;

		/// <summary>
		/// Constructor.
		/// </summary>
		public FinishSelectionEventArgs( Rectangle window )
		{
			_window = window;
		}

		public Rectangle Window
		{
			get
			{
				return _window;
			}
			set
			{
				_window = value;
			}
		}
	}


	/// <summary>
	/// Focus box changed event argument.
	/// </summary>
	public class FocusBoxChangedEventArgs : FinishSelectionEventArgs
	{
		private MouseEventArgs _mouseEventArgs;

		/// <summary>
		/// Constructor.
		/// </summary>
		public FocusBoxChangedEventArgs( Rectangle window, MouseEventArgs e )
			: base( window )
		{
			_mouseEventArgs = e;
		}

		public MouseEventArgs MouseEventArgs
		{
			get
			{
				return _mouseEventArgs;
			}
			set
			{
				_mouseEventArgs = value;
			}
		}
	}

	/// <summary>
	/// SelectingEventArgs class.
	/// </summary>
	public class SelectingEventArgs : EventArgs
	{
		private bool _isCancel;

		/// <summary>
		/// Constructor.
		/// </summary>
		public SelectingEventArgs()
		{
			_isCancel = false;
		}

		/// <summary>
		/// Cancel.
		/// </summary>
		public bool Cancel
		{
			get
			{
				return _isCancel;
			}
			set
			{
				_isCancel = value;
			}
		}
	}


	/// <summary>
	/// DieLocationEventArgs class.
	/// </summary>
	public class DieFocusEventArgs : EventArgs
	{
		private int _row;
		private int _col;
		private MouseEventArgs _mouseArgs;
		/// <summary>
		/// Constructor.
		/// </summary>
		public DieFocusEventArgs( int row, int col, MouseEventArgs mouse )
		{
			_row = row;
			_col = col;
		}

		public int Row
		{
			get
			{
				return _row;
			}
		}

		public int Col
		{
			get
			{
				return _col;
			}
		}

		public MouseEventArgs MouseArgs
		{
			get
			{
				return _mouseArgs;
			}
		}

	}
}
