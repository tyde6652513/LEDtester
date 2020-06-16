#define USE_FILE
#undef USE_FILE

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using MPI.Win32;

namespace MPI.UCF.Forms.Domain
{
	public class VirtualMapDb : IWaferDb
	{
		public delegate void ForeachRecordHandler( short row, short column, FieldValue fv );
		public event BoundaryNotifyHandler OnOutOfBoundary;

		#region >>> Constants <<<
		private const int EXPAND_SIZE = 6;
		private const int SMALL_BOUNDARY = 6;

		private const uint MAX_MEASURE_ITEM = 127;
		private const uint MAX_OF_RECORD = 7199369; //prime , 2683*2683
		//private const uint MAX_ROW_COLUMN = 2683;

		private const uint PREVIEW_BLOCK = 1;
		private const uint SIZE_OF_MEASURE = sizeof( float );
		private const uint ROW_COL_COMBO = sizeof( uint );

		#endregion

		#region >>> Private Fields <<<
		private uint _allowedCount;
		private uint _measuredCount;

		private uint _eachRecSize;
		private uint _recordCount;
		private uint _maxBlocks;

		private Dictionary<string, ushort> _itemMap;
		private uint[] _recordIdxMap;
		private IntPtr _vMapPtr, _vMemPtr;

		private uint _recordLo, _recordHi;

		private string _symbolId;

		private IntPtr _VMHandle;

		private TRectangle _boundary;

#if USE_FILE
		private IntPtr _fileHandle;
		private string _tempFile;
#endif
		#endregion

		#region >>> Constructor / Disposor <<<

		public VirtualMapDb()
		{
#if USE_FILE
			_tempFile = Path.GetTempFileName();
			_fileHandle = Win32API.CreateFile( _tempFile, EFileAccess.GenericRead | EFileAccess.GenericWrite,
			   EFileShare.None, IntPtr.Zero, ECreationDisposition.OpenAlways, EFileAttributes.Hidden, IntPtr.Zero );

			if ( _fileHandle.ToInt32() == -1 )
			{
				Console.Error.WriteLine( "[VirtualMapDb] CreateFile, #{0}", Marshal.GetLastWin32Error() );
				throw new Win32Exception( "CreateFile" );
			}
#endif
			_allowedCount = MAX_OF_RECORD;
			_measuredCount = MAX_MEASURE_ITEM;

			//1 page frame (65536) contains 128 test item, which can store 127 measured value ( float:4 bytes)
			_eachRecSize = _measuredCount * SIZE_OF_MEASURE + ROW_COL_COMBO; //256 bytes
			_maxBlocks = ( _allowedCount * _eachRecSize + ( SYSTEM_INFO.ALLOCATION_GRANULARITY - 1 ) ) / SYSTEM_INFO.ALLOCATION_GRANULARITY;

			uint whole_view_size = _maxBlocks * SYSTEM_INFO.ALLOCATION_GRANULARITY;
			Console.WriteLine( "[VirtualMapDb] Going to request {0} MB", ( whole_view_size >> 20 ) );

#if USE_FILE
			FileMapProtection flags = FileMapProtection.PageReadWrite | FileMapProtection.SectionReserve;
			_VMHandle = Win32API.MappingFile.CreateFileMapping( _fileHandle, flags, whole_view_size, true );
#else
			_VMHandle = Win32API.MappingFile.CreateFileMapping( whole_view_size, true );
#endif
			if ( _VMHandle == IntPtr.Zero )
			{
				Console.Error.WriteLine( "[VirtualMapDb] CreateFileMapping, #{0}", Marshal.GetLastWin32Error() );
				this.Dispose();
				throw new Win32Exception( "CreateFileMapping" );
			}

			_symbolId = "Bin";

			if ( this.prealloc() == false )
			{
				this.Dispose();
				throw new InsufficientMemoryException();
			}
		}

		public void Dispose()
		{
			_recordIdxMap = null;
			_itemMap = null;

			bool ok = false;

			if ( _vMapPtr != IntPtr.Zero )
			{
				ok = Win32API.MappingFile.UnmapViewOfFile( _vMapPtr );
				Console.Error.WriteLine( "[VirtualMapDb::Dispose] UnmapViewOfFile:{0}, #{1}", ok, Marshal.GetLastWin32Error() );
			}

			if ( _VMHandle != IntPtr.Zero )
			{
				ok = Win32API.CloseHandle( _VMHandle );
				Console.Error.WriteLine( "[VirtualMapDb::Dispose] FileMapping, CloseHandle:{0}, #{1}", ok, Marshal.GetLastWin32Error() );
			}

#if USE_FILE
			if ( _fileHandle != IntPtr.Zero )
			{
				ok = Win32API.CloseHandle( _fileHandle );
				Console.Error.WriteLine( "[VirtualMapDb::Dispose] File, CloseHandle:{0}, #{1}", ok, Marshal.GetLastWin32Error() );
			}

			if ( _tempFile != null )
				File.Delete( _tempFile );
#endif
		}

		#endregion

		#region >>> Private Method <<<

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

		private void seekToRecord( uint current, bool writeMode )
		{
			if ( _recordLo <= current && current <= _recordHi )
				return;

			uint preview_size = PREVIEW_BLOCK * SYSTEM_INFO.ALLOCATION_GRANULARITY;

			bool ok = Win32API.MappingFile.UnmapViewOfFile( _vMapPtr );
			if ( ok == false )
				Console.Error.WriteLine( "[VirtualMapDb::EnsureCommitRange] UnmapViewOfFile fails" );

			uint preview_record = preview_size / _eachRecSize;
			_recordLo = ( current / preview_record ) * preview_record;
			_recordHi = _recordLo + preview_record - 1;

			uint offset = current * _eachRecSize;
			offset = ( offset / preview_size ) * preview_size;

			_vMapPtr = Win32API.MappingFile.MapViewOfFile( _VMHandle, offset, preview_size );
			_vMemPtr = _vMapPtr;
			if ( _vMemPtr == IntPtr.Zero )
			{
				Console.WriteLine( "[MapViewOfFile] {0}", Marshal.GetLastWin32Error() );
				this.Dispose();
				throw new Win32Exception( "MapViewOfFile" );
			}

		}

		private ushort ensureItemIndex( string key )
		{
			ushort idx;
			if ( _itemMap.ContainsKey( key ) == false )
			{
				idx = ( ushort ) _itemMap.Count;
				_itemMap.Add( key, idx );
			}
			else
			{
				idx = _itemMap[key];
			}

			return idx;
		}

		private void addItemValue( uint recordIndex, uint itemIndex, uint combo, float value )
		{
			this.seekToRecord( recordIndex, true );

			uint offset = ( recordIndex - _recordLo ) * _eachRecSize;
			Win32API.Memory.WriteInt32( _vMemPtr, offset, ( int ) combo );

			offset += ROW_COL_COMBO + ( SIZE_OF_MEASURE * itemIndex );
			Win32API.Memory.WriteFloat( _vMemPtr, offset, value );
		}

		private void setItemValue( uint recordIndex, uint itemIndex, float value )
		{
			this.seekToRecord( recordIndex, true );

			uint offset = ( recordIndex - _recordLo ) * _eachRecSize;
			offset += ROW_COL_COMBO + ( SIZE_OF_MEASURE * itemIndex );
			Win32API.Memory.WriteFloat( _vMemPtr, offset, value );
		}

		private void addItems( uint recordIndex, uint combo, float[] array )
		{
			this.seekToRecord( recordIndex, true );

			uint offset = ( recordIndex - _recordLo ) * _eachRecSize;
			Win32API.Memory.WriteInt32( _vMemPtr, offset, ( int ) combo );

			offset += ROW_COL_COMBO;
			Win32API.Memory.WriteFloatArray( _vMemPtr, offset, array );
		}

		private float[] makeValueList( FieldValue values )
		{
			int count = ( values.Count > _itemMap.Count ) ? values.Count : _itemMap.Count;
			count %= ( int ) _measuredCount;

			float[] array = new float[count];
			for ( int i = array.Length - 1; i >= 0; i-- )
				array[i] = float.MinValue;

			foreach ( KeyValuePair<string, float> item in values )
			{
				ushort item_idx = this.ensureItemIndex( item.Key );
				array[item_idx] = item.Value;
			}

			return array;
		}

		private bool contains( int row, int column, out uint index )
		{
			uint rec_idx = ( uint ) ( ( row << 16 ) | ( ushort ) column );
			rec_idx %= MAX_OF_RECORD;

			index = _recordIdxMap[rec_idx];

			//if ( rec_idx == 0 ) // special case
			//   return ( index != uint.MaxValue );

			//return ( index != 0 );

			return ( index != uint.MaxValue );
		}

		private float getItemValue( uint recordIndex, uint itemIndex )
		{
			this.seekToRecord( recordIndex, true );

			uint offset = ( recordIndex - _recordLo ) * _eachRecSize;
			offset += ROW_COL_COMBO + ( SIZE_OF_MEASURE * itemIndex );
			return Win32API.Memory.ReadFloat( _vMemPtr, offset );
		}

		private bool prealloc()
		{
			uint preview_size = PREVIEW_BLOCK * SYSTEM_INFO.ALLOCATION_GRANULARITY; // default: 64 K

			_vMapPtr = Win32API.MappingFile.MapViewOfFile( _VMHandle, 0, preview_size );
			_vMemPtr = _vMapPtr;

			if ( _vMapPtr == IntPtr.Zero )
			{
				Console.Error.WriteLine( "[VirtualMapDb::Open] MapViewOfFile fail" );
				return false;
			}

			_recordIdxMap = new uint[MAX_OF_RECORD];
			Win32API.Memory.FillMemory( Marshal.UnsafeAddrOfPinnedArrayElement( _recordIdxMap, 0 ), sizeof( uint ) * MAX_OF_RECORD, 0xff );

			_itemMap = new Dictionary<string, ushort>( ( int ) _measuredCount );

			_recordLo = 0;
			_recordHi = ( preview_size / _eachRecSize ) - 1;

			_boundary = new TRectangle( short.MinValue, short.MinValue, short.MaxValue, short.MaxValue );

			return true;
		}

		#endregion

		#region >>> Public Method <<<

		public void Reset()
		{
			uint preview_size = PREVIEW_BLOCK * SYSTEM_INFO.ALLOCATION_GRANULARITY; // default: 64 K

			if ( _vMapPtr != IntPtr.Zero )
			{
				bool ok = false;

				ok = Win32API.MappingFile.UnmapViewOfFile( _vMapPtr );
				Console.Error.WriteLine( "[VirtualMapDb::Reset] UnmapViewOfFile, {0}, #{1}", ok, Marshal.GetLastWin32Error() );
			}

			_vMapPtr = Win32API.MappingFile.MapViewOfFile( _VMHandle, 0, preview_size );
			_vMemPtr = _vMapPtr;

			if ( _vMemPtr == IntPtr.Zero )
			{
				Console.Error.WriteLine( "[VirtualMapDb::Open] MapViewOfFile fail, #{0}", Marshal.GetLastWin32Error() );
				this.Dispose();
				throw new Win32Exception( "MapViewOfFile" );
			}

			_recordCount = 0;

			//Win32API.Memory.ZeroMemory( Marshal.UnsafeAddrOfPinnedArrayElement( _recordIdxMap, 0 ), sizeof( int ) * MAX_OF_RECORD );
			//_recordIdxMap[0] = uint.MaxValue;  //$RIC, special case
			Win32API.Memory.FillMemory( Marshal.UnsafeAddrOfPinnedArrayElement( _recordIdxMap, 0 ), sizeof( uint ) * MAX_OF_RECORD, 0xff );

			_recordLo = 0;
			_recordHi = ( preview_size / _eachRecSize ) - 1;

			_itemMap.Clear();

			_boundary = new TRectangle( short.MinValue, short.MinValue, short.MaxValue, short.MaxValue );
		}

		public bool Contains( int row, int column )
		{
			uint index;
			return this.contains( row, column, out index );
		}

		public void AddItem( int row, int column, float value )
		{
			this.AddItem( row, column, _symbolId, value );
		}

		public void AddItem( int row, int column, string key, float value )
		{
			ushort item_idx = this.ensureItemIndex( key );

			uint combo = ( uint ) ( ( row << 16 ) | ( ushort ) column );
			uint mapping = combo % MAX_OF_RECORD;

			//if ( _recordIdxMap[mapping] == 0 || ( mapping == 0 && _recordIdxMap[0] == uint.MaxValue ) )
			if ( _recordIdxMap[mapping] == uint.MaxValue )
			{
				this.adjustBoundary( row, column );
				this.addItemValue( _recordCount, item_idx, combo, value );
				_recordIdxMap[mapping] = _recordCount;
				_recordCount++;
			}
			else
			{
				uint rec_idx = _recordIdxMap[mapping];
				this.addItemValue( rec_idx, item_idx, combo, value );
			}
		}

		public void AddItem( int row, int column, FieldValue values )
		{
			uint combo = ( uint ) ( ( row << 16 ) | ( ushort ) column );
			uint mapping = combo % MAX_OF_RECORD;

			//if ( _recordIdxMap[mapping] == 0 || ( mapping == 0 && _recordIdxMap[0] == uint.MaxValue ) )
			if ( _recordIdxMap[mapping] == uint.MaxValue )
			{
				//$RIC, change to batch mode
				this.adjustBoundary( row, column );

				float[] list = this.makeValueList( values );
				this.addItems( _recordCount, combo, list );
				_recordIdxMap[mapping] = _recordCount;
				_recordCount++;
			}
			else
			{
				Console.WriteLine( "[Collision] {0},{1}:{2}", row, column, mapping );
			}
		}

		public void BatchAddItems( Dictionary<short, Dictionary<short, FieldValue>> values )
		{
			foreach ( KeyValuePair<short, Dictionary<short, FieldValue>> rows in values )
			{
				short row = rows.Key;
				foreach ( KeyValuePair<short, FieldValue> cols in rows.Value )
				{
					short column = cols.Key;

					uint combo = ( uint ) ( ( row << 16 ) | ( ushort ) column );
					uint mapping = combo % MAX_OF_RECORD;

					//if ( _recordIdxMap[mapping] == 0 || ( mapping == 0 && _recordIdxMap[0] == uint.MaxValue ) )
					if ( _recordIdxMap[mapping] == uint.MaxValue )
					{
						float[] list = this.makeValueList( cols.Value );
						this.addItems( _recordCount, combo, list );
						_recordIdxMap[mapping] = _recordCount;
						_recordCount++;
					}
					else
					{
						Console.WriteLine( "[Collision] {0},{1}:{2}", row, column, mapping );
					}
				}
			}
		}

		public void Foreach( ForeachItemHandler callback )
		{
			Foreach( callback, _symbolId );
		}

		public void ForeachRecord( ForeachRecordHandler callback )
		{
			float[] array = new float[_itemMap.Count];
			FieldValue fv;

			GCHandle gc1 = GCHandle.Alloc( array, GCHandleType.Pinned );
			uint record = 0;

			//if ( _recordIdxMap[0] == uint.MaxValue )
			//   record = 1;

			while ( record < _recordCount )
			{
				fv = new FieldValue();
				this.seekToRecord( record, true );

				uint offset = ( record - _recordLo ) * _eachRecSize;
				uint combo = ( uint ) Win32API.Memory.ReadInt32( _vMemPtr, offset );

				offset += ROW_COL_COMBO;
				Win32API.Memory.ReadFloatArray( _vMemPtr, offset, array );

				foreach ( KeyValuePair<string, ushort> item in _itemMap )
					fv.Add( item.Key, array[item.Value] );

				short row = ( short ) ( combo >> 16 );
				short col = ( short ) combo;
				callback( row, col, fv );

				record++;

				IntPtr dst_ptr = Marshal.UnsafeAddrOfPinnedArrayElement( array, 0 );
				Win32API.Memory.ZeroMemory( dst_ptr, SIZE_OF_MEASURE * _measuredCount );
				fv = null;
			}

			gc1.Free();

		}

		public void Foreach( ForeachItemHandler callback, string symbol )
		{
			if ( _itemMap.ContainsKey( symbol ) == false )
				return;

			ushort item_index = _itemMap[symbol];

			uint record = 0;
			//if ( _recordIdxMap[0] == uint.MaxValue )
			//   record = 1;

			while ( record < _recordCount )
			{
				this.seekToRecord( record, true );

				uint offset = ( record - _recordLo ) * _eachRecSize;
				uint combo = ( uint ) Win32API.Memory.ReadInt32( _vMemPtr, offset ); // row, column

				offset += ROW_COL_COMBO + ( SIZE_OF_MEASURE * item_index );
				float value = Win32API.Memory.ReadFloat( _vMemPtr, offset );

				short row = ( short ) ( combo >> 16 );
				short col = ( short ) combo;
				callback( row, col, value );

				record++;
			}
		}

		public FieldValue GetValue( int row, int column )
		{
			uint rec_idx;
			if ( this.contains( row, column, out rec_idx ) == false )
				return null;

			this.seekToRecord( rec_idx, false );

			uint offset = ( rec_idx - _recordLo ) * _eachRecSize; //combo
			uint combo = ( uint ) Win32API.Memory.ReadInt32( _vMemPtr, offset );

			offset += ROW_COL_COMBO;
			float[] array = new float[_itemMap.Count];
			Win32API.Memory.ReadFloatArray( _vMemPtr, offset, array );

			FieldValue fv = new FieldValue();
			foreach ( KeyValuePair<string, ushort> item in _itemMap )
				fv.Add( item.Key, array[item.Value] );

			return fv;

		}

		#endregion

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

		public int Count
		{
			get
			{
				return ( int ) this._recordCount;
			}
		}

		public float this[int row, int column, string symbol]
		{
			get
			{
				uint rec_idx;

				if ( _itemMap.ContainsKey( symbol ) && this.contains( row, column, out rec_idx ) )
					return this.getItemValue( rec_idx, _itemMap[symbol] );

				return float.NaN;
			}

			set
			{
				uint rec_idx;

				if ( _itemMap.ContainsKey( symbol ) && this.contains( row, column, out rec_idx ) )
					this.setItemValue( rec_idx, _itemMap[symbol], value );
			}
		}

		public string[] GetItemNames()
		{
			string[] array = new string[_itemMap.Count];
			_itemMap.Keys.CopyTo( array, 0 );
			return array;
		}

		internal float this[short row, short column, string symbol, bool quick]
		{
			get
			{
				uint index = ( uint ) ( ( row << 16 ) | ( ushort ) column );
				index %= MAX_OF_RECORD;

				//if ( index == 0 && _recordIdxMap[0] == uint.MaxValue )
				//   return float.NaN;

				index = _recordIdxMap[index];
				if ( index == uint.MaxValue )
					return float.NaN;

				return this.getItemValue( index, _itemMap[symbol] );
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

		public void ExportCSV( string filepath )
		{
			StreamWriter sw = new StreamWriter( filepath );
			sw.WriteLine( string.Join( ",", this.GetItemNames() ) );

			uint item_count = ( uint ) _itemMap.Count;
			float[] items = new float[item_count];

			uint record = 0;
			//if ( _recordIdxMap[0] == uint.MaxValue )
			//   record = 1;

			while ( record < _recordCount )
			{
				this.seekToRecord( record, true );

				uint offset = ( record - _recordLo ) * _eachRecSize;
				uint combo = ( uint ) Win32API.Memory.ReadInt32( _vMemPtr, offset );

				offset += ROW_COL_COMBO;
				Win32API.Memory.ReadFloatArray( _vMemPtr, offset, items );

				for ( int i = 0; i < item_count - 1; i++ )
				{
					float value = items[i];
					if ( value != float.MinValue )
						sw.Write( value );

					sw.Write( ',' );
				}

				sw.WriteLine( items[item_count - 1] );

				record++;
				IntPtr dst_ptr = Marshal.UnsafeAddrOfPinnedArrayElement( items, 0 );
				Win32API.Memory.ZeroMemory( dst_ptr, SIZE_OF_MEASURE * item_count );
			}

			sw.Dispose();
			sw = null;
		}
	}

}
