using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;

namespace MPI.UCF.Forms.Domain
{
	using ValueList = List<float>;
	using ColumnList = Dictionary<short, List<float>>;
	using ItemValueDefine = Dictionary<string, short>;

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
				if ( this.ContainsKey( key ) == false )
					return float.NaN;

				return base[key];
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

	public class WaferDatabase : IDisposable
	{
		#region >>> Constants <<<
		private const int EXPAND_SIZE = 6;
		private const int SMALL_BOUNDARY = 6;
		#endregion

		public event BoundaryNotifyHandler OnOutOfBoundary;

		#region >>> Private Field <<<

		private bool _autoBoundary;
		private FieldCounter _fieldCounter;
		private Dictionary<short, ColumnList> _store; // row, <col, float,float....>
		private Dictionary<string, short> _itemMap;
		private string _symbolId;
		#endregion

		#region >>> Internal Filed <<<
		internal EGrowthDirection fGrowthDir;
		internal Rectangle fBoundary;
		#endregion

		#region >>> Constructor / Disposor <<<

		public WaferDatabase()
			: this( 300 )
		{

		}

		public WaferDatabase( int capacity )
		{
			_symbolId = "Bin";
			_itemMap = new Dictionary<string, short>();
			_itemMap.Add( "Bin", 0 );

			_store = new Dictionary<short, ColumnList>( capacity );
			_fieldCounter = new FieldCounter();
			_autoBoundary = true;

			fBoundary = Rectangle.FromLTRB( 0, 0, SMALL_BOUNDARY, SMALL_BOUNDARY ); // left & top fixed
			fGrowthDir = EGrowthDirection.None;
		}

		public void Dispose()
		{
			if ( _store != null )
			{
				_store.Clear();
				_store = null;
			}

			if ( _fieldCounter != null )
				_fieldCounter.Clear();
		}

		#endregion

		#region >>> Private Function <<<

		/// <summary>
		/// Adjust Boundary accroding to expansion direction
		/// </summary>
		private void adjustBoundary( short row, short col )
		{
			int left = fBoundary.Left;
			int top = fBoundary.Top;
			int right = fBoundary.Right;
			int bottom = fBoundary.Bottom;

			if ( fGrowthDir == EGrowthDirection.None )
			{
				if ( fBoundary.Contains( col, row ) )
					return;

				//this.adjustBoundaryRectangle( row, col );
				this.adjustBoundaryRectangle( row, col, ref left, ref top, ref right, ref bottom );
				return;
			}

			bool outofbound = false;

			if ( fGrowthDir == EGrowthDirection.Upward )
				outofbound = this.adjustBoundaryUpward( row, col, ref left, ref top, ref right, ref bottom );
			else if ( fGrowthDir == EGrowthDirection.Downward )
				outofbound = this.adjustBoundaryDownward( row, col, ref left, ref top, ref right, ref bottom );
			else if ( fGrowthDir == EGrowthDirection.Rightward )
				outofbound = this.adjustBoundaryRightward( row, col, ref left, ref top, ref right, ref bottom );
			else if ( fGrowthDir == EGrowthDirection.Leftward )
				outofbound = this.adjustBoundaryLeftward( row, col, ref left, ref top, ref right, ref bottom );

			if ( outofbound )
			{
				fBoundary = Rectangle.FromLTRB( left, top, right, bottom );

				if ( OnOutOfBoundary != null )
				{
					OnOutOfBoundary.Invoke( fBoundary );
				}
			}
		}

		/// <summary>
		/// Adjust Boundary accroding to expansion direction from Bottom->Top(Y-), BOTTOM fixed
		/// </summary>
		private bool adjustBoundaryUpward( int row, int col, ref int left, ref int top, ref int right, ref int bottom )
		{
			bool outofbound = false;
			if ( col <= left ) // left forward, adjust to left-up corner 
			{
				int diff = ( left - col ) + EXPAND_SIZE;
				left -= diff;
				top -= diff;
				outofbound = true;
			}
			else if ( col >= right ) // right forward, adjust to right-up corner 
			{
				int diff = ( col - right ) + EXPAND_SIZE;
				right += diff;
				top -= diff;
				outofbound = true;
			}

			if ( row <= top ) // upward, adjust to center-up
			{
				int diff = ( top - row ) + EXPAND_SIZE;
				top -= diff;
				diff = ( int ) Math.Ceiling( ( float ) diff / 2f );
				left -= diff;
				right += diff;
				outofbound = true;
			}
			else if ( row >= bottom ) // downward // special case
			{
				bottom = row;
				outofbound = true;
			}

			return outofbound;
		}

		/// <summary>
		/// Adjust Boundary accroding to direction for Top->Bottom(Y+), TOP fixed
		/// </summary>
		private bool adjustBoundaryDownward( int row, int col, ref int left, ref int top, ref int right, ref int bottom )
		{
			bool outofbound = false;
			if ( col <= left ) // left forward, adjust to left-bottom corner
			{
				int diff = ( left - col ) + EXPAND_SIZE;
				left -= diff;
				bottom += diff;
				outofbound = true;
			}
			else if ( col >= right ) // right forward, adjust to right-bottom corner
			{
				int diff = ( col - right ) + EXPAND_SIZE;
				right += diff;
				bottom += diff;
				outofbound = true;
			}

			if ( row <= top ) // upward //special case
			{
				top = row;
				outofbound = true;
			}
			else if ( row >= bottom ) // downward, adjust to left & right
			{
				int diff = ( row - bottom ) + EXPAND_SIZE;
				bottom += diff;

				diff = ( int ) Math.Ceiling( ( float ) diff / 2f );

				left -= diff;
				right += diff;
				outofbound = true;
			}

			return outofbound;
		}

		/// <summary>
		/// Adjust Boundary accroding to direction for Left->Right(X+), LEFT fixed
		/// </summary>
		private bool adjustBoundaryRightward( int row, int col, ref int left, ref int top, ref int right, ref int bottom )
		{
			bool outofbound = false;
			if ( col <= left ) // leftward, special case
			{
				left = col;
				outofbound = true;
			}
			else if ( col >= right ) // right forward, adjust right-center
			{
				int diff = ( col - right ) + EXPAND_SIZE;
				right += diff;

				diff = ( int ) Math.Ceiling( ( float ) diff / 2f );

				top -= diff;
				bottom += diff;
				outofbound = true;
			}

			if ( row <= top ) // upward, adjust to right-top corner
			{
				int diff = ( top - row ) + EXPAND_SIZE;
				top -= diff;
				right += diff;
				outofbound = true;
			}
			else if ( row >= bottom ) // downward, adjust to right-bottom corner
			{
				int diff = ( bottom - row ) + EXPAND_SIZE;
				bottom += diff;
				right += diff;
				outofbound = true;
			}

			return outofbound;
		}

		/// <summary>
		/// Adjust Boundary accroding to direction for Right->Left(X-), RIGHT fixed
		/// </summary>
		private bool adjustBoundaryLeftward( int row, int col, ref int left, ref int top, ref int right, ref int bottom )
		{
			bool outofbound = false;
			if ( col <= left ) // left forward, adjust left-center
			{
				int diff = ( left - col ) + EXPAND_SIZE;
				left -= diff;

				diff = ( int ) Math.Ceiling( ( float ) diff / 2f );

				top -= diff;
				bottom += diff;
				outofbound = true;
			}
			else if ( col >= right ) // right forward
			{
				right = col;
				outofbound = true;
			}

			if ( row <= top ) // upward, adjust left-top corner
			{
				int diff = ( top - row ) + EXPAND_SIZE;
				top -= diff;
				left -= diff;
				outofbound = true;
			}
			else if ( row >= bottom ) // downward, adjust left-bottom corner
			{
				int diff = ( bottom - row ) + EXPAND_SIZE;
				bottom += diff;
				left -= diff;
				outofbound = true;
			}

			return outofbound;
		}

		/// <summary>
		/// adjustBoundary byRect
		/// </summary>
		private bool adjustBoundaryRectangle( int row, int col, ref int left, ref int top, ref int right, ref int bottom )
		{
			bool outofrange = false;

			if ( col <= left )
			{
				left = col;
				outofrange = true;
			}
			else if ( col >= right )
			{
				right = col;
				outofrange = true;
			}

			if ( row <= top )
			{
				top = row;
				outofrange = true;
			}
			else if ( row >= bottom )
			{
				bottom = row;
				outofrange = true;
			}

			if ( outofrange )
			{
				int width = right - left;
				int height = bottom - top;

				int diff = width - height;
				int mod = diff % 2;

				if ( diff > 0 ) // width > height
				{
					diff = diff / 2;
					top -= diff;
					bottom += diff + mod;
				}
				else if ( diff < 0 ) // height > width
				{
					diff = -diff / 2;
					left -= diff;
					right += diff - mod;
				}

				fBoundary = Rectangle.FromLTRB( left, top, right, bottom );
			}

			return false;
		}

		private bool adjustBoundaryRectangle2( int row, int col )
		{
			int left = fBoundary.Left;
			int top = fBoundary.Top;

			int width = fBoundary.Width;
			int height = fBoundary.Height;
			bool outofbound = false;

			if ( col <= left ) // left forward, expand top and bottom side
			{
				left = col - 1;
				outofbound = true;
			}

			if ( row <= top ) // upward, expand left and right side
			{
				top = row - 1;
				outofbound = true;
			}

			if ( outofbound )
			{
				fBoundary = new Rectangle( left, top, width, height );
			}

			int right = fBoundary.Right;
			int bottom = fBoundary.Bottom;
			outofbound = false;
			if ( col >= right ) // right forward, expand top and bottom side
			{
				right = col + 1;
				outofbound = true;
			}

			if ( row >= bottom ) // downward, adjust to left & right
			{
				bottom = row + 1;
				outofbound = true;
			}

			if ( outofbound )
			{
				fBoundary = Rectangle.FromLTRB( left, top, right, bottom );
			}

			return outofbound;
		}

		private short ensureFieldNameExists( string name )
		{
			short idx = -1;

			if ( _itemMap.ContainsKey( name ) == false )
			{
				idx = ( short ) _itemMap.Count;
				_itemMap.Add( name, idx );
			}
			else
			{
				idx = _itemMap[name];
			}

			return idx;
		}

		private ColumnList getColumnList( short row )
		{
			ColumnList colist = null;

			if ( _store.ContainsKey( row ) )
			{
				colist = _store[row];
			}
			else
			{
				colist = new ColumnList();
				_store.Add( row, colist );
			}

			return colist;
		}

		private ValueList getValueList( ColumnList colist, short col )
		{
			ValueList valist = null;
			if ( colist.ContainsKey( col ) )
			{
				valist = colist[col];
			}
			else
			{
				valist = new ValueList();
				colist.Add( col, valist );
			}

			return valist;
		}

		#endregion

		internal void innerSetStart( short row, short column )
		{
			_autoBoundary = true;

			int left = fBoundary.Left;
			int top = fBoundary.Top;
			int right = fBoundary.Right;
			int bottom = fBoundary.Bottom;

			if ( fGrowthDir == EGrowthDirection.Upward ) // row/bottom: fixed
			{
				left = ( column - SMALL_BOUNDARY );
				top = ( row - SMALL_BOUNDARY );
				fBoundary = Rectangle.FromLTRB( left, top, column, row + 2 );
			}
			else if ( fGrowthDir == EGrowthDirection.Leftward )
			{
				left = ( column - SMALL_BOUNDARY );
				top = ( row - SMALL_BOUNDARY );
				fBoundary = Rectangle.FromLTRB( left, top, column + 2, row );
			}
			else if ( fGrowthDir == EGrowthDirection.Downward || fGrowthDir == EGrowthDirection.Rightward )
			{
				fBoundary.X = column;
				fBoundary.Y = row; //fixed
			}
		}

		internal void innerSetBoundary( Rectangle rect )
		{
			_autoBoundary = false;
			fBoundary = rect;
		}

		internal bool Contains( ref Point location )
		{
			return this.Contains( location.Y, location.X );
		}

		#region >>> Public Method <<<

		public int GetIndex( int row, int col )
		{
			float num = this[row, col, "_INDEX_"];
			int result;

			if ( float.IsNaN( num ) )
				result = -1;
			else
				result = ( int ) num;
			return result;
		}

		public void Reset()
		{
			if ( _store != null && _store.Count > 0 )
			{
				_store.Clear();
				_fieldCounter.Clear();
			}

			if ( _autoBoundary )
			{
				fBoundary = new Rectangle( 0, 0, SMALL_BOUNDARY, SMALL_BOUNDARY ); // left & top fixed
			}
		}

		public void AddItem( short row, short col, FieldValue value )
		{
			lock ( _store )
			{
				ValueList valist = this.getValueList( this.getColumnList( row ), col );

				foreach ( KeyValuePair<string, float> pair in value )
				{
					short idx = this.ensureFieldNameExists( pair.Key );
					if ( idx < valist.Count )
					{
						valist[idx] = pair.Value;
					}
					else
					{
						while ( valist.Count < idx )
							valist.Add( float.NaN ); //add empty item

						valist.Add( pair.Value );
					}
				}
			}

			if ( _autoBoundary )
				this.adjustBoundary( row, col );
		}

		public void AddItem( short row, short col, string key, float value )
		{
			lock ( _store )
			{
				short idx = this.ensureFieldNameExists( key );

				ValueList valist = this.getValueList( this.getColumnList( row ), col );

				if ( idx < valist.Count )
				{
					valist[idx] = value;
				}
				else
				{
					while ( valist.Count < idx )
						valist.Add( float.NaN ); //add empty item

					valist.Add( value );
				}
			}

			if ( _autoBoundary )
				this.adjustBoundary( row, col );
		}

		public void Foreach( ForeachItemHandler callback )
		{
			Foreach( callback, _symbolId );
		}

		public void Foreach( ForeachItemHandler callback, string symbol )
		{
			if ( _itemMap.ContainsKey( symbol ) == false )
				return;

			lock ( _store )
			{
				short idx = _itemMap[symbol];

				foreach ( KeyValuePair<short, ColumnList> rowpair in _store )
				{
					int row = rowpair.Key;
					ColumnList value_items = rowpair.Value;

					foreach ( KeyValuePair<short, ValueList> colpair in value_items )
					{
						int col = colpair.Key;
						ValueList valist = colpair.Value;
						float value = valist[idx];
						callback( row, col, value );
					}
				}
			}
		}

		public bool Contains( int row, int col )
		{
			lock ( _store )
			{
				short r = ( short ) ( row );
				return ( _store.ContainsKey( r ) ? _store[r].ContainsKey( ( short ) ( col ) ) : false );
			}
		}

		public void CalcFieldCount()
		{
			_fieldCounter.Clear();

			foreach ( KeyValuePair<short, ColumnList> rows in _store )
			{
				foreach ( KeyValuePair<short, ValueList> cols in rows.Value )
				{
					ValueList valist = cols.Value;

					foreach ( KeyValuePair<string, short> pair in _itemMap )
					{
						short idx = pair.Value;
						string key = pair.Key;

						if ( idx < valist.Count )
						{
							if ( _fieldCounter.ContainsKey( key ) )
								_fieldCounter[key]++;
							else
								_fieldCounter.Add( key, 1 );
						}
					}
				}
			}
		}

		#endregion

		#region >>> Public Property <<<
		/// <summary>
		/// Set/Get Field Value
		/// </summary>
		/// <param name="row">Row of Die</param>
		/// <param name="col">Column of Die</param>
		/// <param name="key">Key in KeyValue </param>
		/// <returns>WaferDieValue / WaferDieValue.Empty</returns>
		public float this[int row, int column, string key]
		{
			get
			{
				lock ( _store )
				{
					if ( _itemMap.ContainsKey( key ) && this.Contains( row, column ) )
					{
						short idx = _itemMap[key];
						return _store[( short ) ( row )][( short ) ( column )][idx];
					}

					return float.NaN;
				}
			}

			set
			{
				lock ( _store )
				{
					if ( _itemMap.ContainsKey( key ) && this.Contains( row, column ) )
					{
						short idx = _itemMap[key];
						_store[( short ) ( row )][( short ) ( column )][idx] = value;
					}
				}
			}
		}

		internal float this[short row, short column, string symbol, bool quick]
		{
			get
			{
				lock ( _store )
				{
					return _store[row][column][_itemMap[symbol]];
				}
			}
		}

		public float this[int row, int column]
		{
			get
			{
				return this[row, column, _symbolId];
			}

			set
			{
				this[row, column, _symbolId] = value;
			}
		}

		//public WaferDieItem GetWaferDie( int row, int column )
		//{
		//   if ( _isAdding == false )
		//   {
		//      if ( this.Contains( row, column ) )
		//         return new WaferDieItem( ( short ) row, ( short ) column, _store[( short ) row][( short ) column] );
		//   }

		//   return WaferDieItem.Empty;
		//}

		public FieldCounter Counters
		{
			get
			{
				return _fieldCounter.Clone() as FieldCounter;
			}
		}

		public Rectangle Boundary
		{
			get
			{
				return fBoundary;
			}
		}
		#endregion

	}

}
