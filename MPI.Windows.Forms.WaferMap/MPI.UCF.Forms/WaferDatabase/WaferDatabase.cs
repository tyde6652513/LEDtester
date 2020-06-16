using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MPI.UCF.Forms.Domain
{
	using System.IO;
	using ColumnList = Dictionary<short, List<float>>;
	using ValueList = List<float>;

	public class WaferDatabase : IWaferDb
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

		internal TRectangle _boundary;

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
			//			_itemMap.Add( "Bin", 0 );

			_store = new Dictionary<short, ColumnList>( capacity );
			_fieldCounter = new FieldCounter();
			_autoBoundary = true;

			_boundary = new TRectangle( short.MinValue, short.MinValue, short.MaxValue, short.MaxValue ); // left & top fixed
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
		/// Adjust Boundary
		/// </summary>
		private void adjustBoundary( int row, int col )
		{
			bool outofbound = false;
			if ( col > _boundary.Left )
			{
				_boundary.Left = ( short ) col;
				outofbound = true;
			}
			else if ( col < _boundary.Right )
			{
				_boundary.Right = ( short ) col;
				outofbound = true;
			}

			if ( row > _boundary.Top )
			{
				_boundary.Top = ( short ) row;
				outofbound = true;
			}
			else if ( row < _boundary.Bottom )
			{
				_boundary.Bottom = ( short ) row;
				outofbound = true;
			}

			if ( outofbound && OnOutOfBoundary != null )
			{
				OnOutOfBoundary( _boundary.ToRectangle() );
			}
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

		private ColumnList getColumnList( int row )
		{
			ColumnList colist = null;

			if ( _store.ContainsKey( ( short ) row ) )
			{
				colist = _store[( short ) row];
			}
			else
			{
				colist = new ColumnList();
				_store.Add( ( short ) row, colist );
			}

			return colist;
		}

		private ValueList getValueList( ColumnList colist, int col )
		{
			ValueList valist = null;
			if ( colist.ContainsKey( ( short ) col ) )
			{
				valist = colist[( short ) col];
			}
			else
			{
				valist = new ValueList();
				colist.Add( ( short ) col, valist );
			}

			return valist;
		}

		#endregion

		#region >>> Public Method <<<

		public int GetIndex( int row, int col )
		{
			float num = this[( short ) row, ( short ) col, "_INDEX_"];
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
				//$Ric, 
				_itemMap.Clear();
				//				_itemMap.Add( "Bin", 0 );
			}

			if ( _autoBoundary )
			{
				_boundary = new TRectangle( short.MinValue, short.MinValue, short.MaxValue, short.MaxValue ); // left & top fixed
			}

		}

		public void AddItem( int row, int col, FieldValue value )
		{
//			lock ( _store )
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

		public void AddItem( int row, int col, float value )
		{
			this.AddItem( row, col, _symbolId, value );
		}

		public void AddItem( int row, int col, string key, float value )
		{
//			lock ( _store )
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

//			lock ( _store )
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
//			lock ( _store )
			{
				short r = ( short ) ( row );
				return ( _store.ContainsKey( r ) ? _store[r].ContainsKey( ( short ) ( col ) ) : false );
			}
		}

		public string[] GetItemNames()
		{
			string[] array = new string[_itemMap.Count];
			_itemMap.Keys.CopyTo( array, 0 );
			return array;
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

		public FieldValue GetValue( int row, int col )
		{
			ColumnList colist;
			if ( _store.TryGetValue( ( short ) row, out colist ) )
			{
				ValueList vl;
				if ( colist.TryGetValue( ( short ) col, out vl ) )
				{
					FieldValue fv = new FieldValue();
					foreach ( KeyValuePair<string, short> item in _itemMap )
					{
						short idx = item.Value; // some item might contain the same id
						if ( idx < vl.Count )
							fv.Add( item.Key, vl[item.Value] );
					}

					return fv;
				}
			}

			return null;
		}

		public void ExportCSV( string filepath )
		{
			StreamWriter sw = new StreamWriter( filepath );
			sw.WriteLine( string.Join( ",", this.GetItemNames() ) );

			int item_count = _itemMap.Count;
			float[] items = new float[item_count];

			foreach ( KeyValuePair<short, ColumnList> rowpair in _store )
			{
				int row = rowpair.Key;
				ColumnList value_items = rowpair.Value;

				foreach ( KeyValuePair<short, ValueList> colpair in value_items )
				{
					int col = colpair.Key;
					ValueList vals = colpair.Value;

					for ( int idx = 0; idx < item_count - 1; idx++ )
					{
						float value = vals[idx];
						if ( value != float.MinValue )
							sw.Write( value );

						sw.Write( ',' );
					}

					sw.WriteLine( vals[item_count - 1] );
				}
			}


			sw.Dispose();
			sw = null;
		}

		private ValueList getValueList( int row, int col )
		{
			ColumnList colist;
			if ( _store.TryGetValue( ( short ) row, out colist ) )
			{
				ValueList vl;
				colist.TryGetValue( ( short ) col, out vl );
				return vl;
			}

			return null;
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
//				lock ( _store )
				{
					short idx;
					if ( _itemMap.TryGetValue( key, out idx ) )
					{
						ValueList val = this.getValueList( row, column );

						if ( val != null && idx < val.Count )
							return val[idx];
					}

					return float.NaN;
				}
			}

			set
			{
//				lock ( _store )
				{
					short idx;
					if ( _itemMap.TryGetValue( key, out idx ) )
					{
						ValueList val = this.getValueList( row, column );

						if ( val != null && idx < val.Count )
							val[idx] = value;
					}
				}
			}
		}

		internal float this[short row, short column, string symbol, bool quick]
		{
			get
			{
//				lock ( _store )
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

		public int Count
		{
			get
			{
				int count = 0;
//				lock ( _store )
				{
					foreach ( ColumnList vl in _store.Values )
						count += vl.Count;
				}

				return count;
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
				return _boundary.ToRectangle();
			}
		}

		public Rectangle Boundary0
		{
			get
			{
				return _boundary.ToRectangle( true );
			}
		}

		public string SymbolId
		{
			get
			{
				return _symbolId;
			}

			set
			{
				_symbolId = value;
			}
		}
		#endregion

	}

}
