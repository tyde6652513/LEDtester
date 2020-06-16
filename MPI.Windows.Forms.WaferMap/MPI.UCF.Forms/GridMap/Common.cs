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
	public delegate void DrawChipEventHandler( Graphics g, Rectangle clip, int row, int col );

	public delegate bool ForeachItemHandler( int row, int col, float value );

	public delegate void BoundaryNotifyHandler( Rectangle bound );

	public delegate bool WaferDieItemForeachDeleate( int row, int col, FieldValue values );

	[Serializable]
	public class FieldValue : Dictionary<string, float>, ICloneable
	{
		public FieldValue()
		{

		}

		internal FieldValue( Dictionary<string, float> values )
			: base( values )
		{
		}

		internal FieldValue( FieldValue copy )
			: base( copy )
		{

		}

		public new float this[string key]
		{
			get
			{
				float value;
				if ( this.TryGetValue( key, out value ) )
					return value;

				return float.NaN;
			}

			set
			{
				base[key] = value;
			}
		}

		public FieldValue( string key, float value )
		{
			this.Add( key, value );
		}

		public object Clone()
		{
			return new FieldValue( this );
		}
	}

	public class FieldCounter : Dictionary<string, int>, ICloneable
	{
		internal FieldCounter()
		{

		}

		internal FieldCounter( FieldCounter copy )
			: base( copy )
		{

		}

		public object Clone()
		{
			return new FieldCounter( this );
		}
	}

	public interface ICellData
	{
		float this[int row, int column]
		{
			get;
			set;
		}
	}

	public interface IWaferDb : ICellData, IDisposable
	{
		event BoundaryNotifyHandler OnOutOfBoundary;

		void Reset();

		void AddItem( int row, int col, FieldValue value );

		void AddItem( int row, int col, string key, float value );

		void AddItem( int row, int col, float value );

		void Foreach( ForeachItemHandler callback );

		void Foreach( ForeachItemHandler callback, string symbol );

		bool Contains( int row, int col );

		string[] GetItemNames();

		void ExportCSV( string filepath );

		FieldValue GetValue( int row, int col );

		#region >>> Public Property <<<

		/// <summary>
		/// Set/Get Field Value
		/// </summary>
		/// <param name="row">Row of Die</param>
		/// <param name="col">Column of Die</param>
		/// <param name="key">Key in KeyValue </param>
		/// <returns>WaferDieValue / WaferDieValue.Empty</returns>
		float this[int row, int column, string key]
		{
			get;
			set;
		}

		Rectangle Boundary
		{
			get;
		}

		Rectangle Boundary0
		{
			get;
		}

		string SymbolId
		{
			get;
			set;
		}

		int Count
		{
			get;
		}
		#endregion

	}

	public interface IGridMap
	{
		#region >>> Zooming <<<

		void Zoom( int scale );

		void Zoom( int row, int column );

		void ZoomIn();

		void ZoomOut();

		/// <summary>
		/// Fit to availiable display area
		/// </summary>
		void AutoScale();

		#endregion

		void Draw();

		bool FocusOn( int row, int column );

		bool SaveToImage( string path );

		bool ScrollTo( int row, int col );

		[Category( "UControl::Selection" )]
		bool Selectable
		{
			get;
			set;
		}

		[Category( "UControl::Selection" )]
		Color SelectionColor
		{
			get;
			set;
		}

		[Category( "UControl::Zoom" )]
		bool SelectionZoom
		{
			get;
			set;
		}

		[Category( "UControl::Zoom" )]
		[Browsable( false )]
		int ZoomScale
		{
			get;
		}

		[Category( "UControl::Focus" )]
		Color FocusColor
		{
			get;
			set;
		}

		[Category( "UControl::Focus" )]
		bool FocusBox
		{
			get;
			set;
		}

		[Category( "UControl::Focus" )]
		bool Snap
		{
			get;
			set;
		}

		[Category( "UControl::Navigator" )]
		bool DragToGo
		{
			get;
			set;
		}
	}

	public interface IWaferMap : IGridMap
	{
		#region >>> Value Set/Get <<

		float GetValue( int row, int column, string symbol );

		float GetValue( int row, int column );

		void SetValue( int row, int column, float value );

		void SetValue( int row, int column, string symbol, float value );

		#endregion

		void SetDatabase( IWaferDb db );
	}

	internal struct TRectangle
	{
		internal short Left, Top;
		internal short Right, Bottom;

		internal TRectangle( int left, int top, int right, int bottom )
			: this( ( short ) left, ( short ) top, ( short ) right, ( short ) bottom )
		{

		}

		internal TRectangle( short left, short top, short right, short bottom )
		{
			this.Left = left;
			this.Top = top;
			this.Right = right;
			this.Bottom = bottom;
		}

		internal Rectangle ToRectangle()
		{
			if ( this.Left == short.MinValue )
				this.Left = 0;

			if ( this.Right == short.MaxValue )
				this.Right = 0;

			if ( this.Top == short.MinValue )
				this.Top = 0;

			if ( this.Bottom == short.MaxValue )
				this.Bottom = 0;

			if ( this.Left > this.Right )
			{
				short right = this.Left;
				this.Left = this.Right;
				this.Right = right;
			}

			if ( this.Top > this.Bottom )
			{
				short bottom = this.Top;
				this.Top = this.Bottom;
				this.Bottom = bottom;
			}

			return Rectangle.FromLTRB( this.Left, this.Top, this.Right, this.Bottom );
		}

		internal Rectangle ToRectangle( bool original )
		{
			if ( this.Left == short.MinValue )
				this.Left = 0;

			if ( this.Right == short.MaxValue )
				this.Right = 0;

			if ( this.Top == short.MinValue )
				this.Top = 0;

			if ( this.Bottom == short.MaxValue )
				this.Bottom = 0;

			return Rectangle.FromLTRB( this.Left, this.Top, this.Right, this.Bottom );
		}
	}

}