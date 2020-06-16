using System;
using System.Collections.Generic;
using System.Text;


namespace MPI.Comm.TSECommand
{
	/// <summary>
	/// Basic packet format size.
	/// </summary>
	static class TSEPacketFormat
	{
		public const Int32 RESERVED_SIZE	= 1;		// Byte
		public const Int32 COMMAND_ID_SIZE	= 4;		// long
		public const Int32 PACKET_SIZE_SIZE = 4;		// DWORD
		public const Int32 MAX_DATA_SIZE	= 2048;		// DATA
	}

	/// <summary>
	/// Command set for communication with TSE Tester.
	/// </summary>
	public enum ETSECommand : int
	{
		ID_WAFER_IN			= 60000,	// PS  -> T
		ID_TEST_ITEM		= 60001,    // T   -> PS
		ID_WAFER_SCAN_END	= 60002,    // PS  -> T
		ID_SOT				= 60003,    // PS  -> T
		ID_EOT				= 60004,    // T   -> PS

		ID_RETEST			= 60006,    // T   -> PS
		ID_AUTOCONTACT		= 60007,    // PS  -> T
		ID_LOT_END			= 60009,    // PS <-> T

		ID_AUTOSTATUS_STOP	= 60010,    // PS <-> T
		ID_ATTOSTATUS_START	= 60011,    // PS <-> T
		ID_BARCODE_INSERT	= 60012,    // PS <-> T

		ID_AUTOCAL_START	= 60020,	// PS  -> T
		ID_AUTOCAL_END		= 60021,	// PS  -> T
		ID_AUTOCAL_SOT		= 60022,	// PS  -> T
		ID_AUTOCAL_EOT		= 60023,	// T   -> PS
		ID_AUTICAL_FAIL		= 60024,	// T   -> PS 
	}

	/// <summary>
	/// Auto Contact Command.
	/// </summary>
	class CmdAutoContact : TSECommand
	{
		public enum EContact : int
		{ 
			Unknown		= -1,
			Contact		= 1,
			NoContact	= 2,
			TestFail	= 3,
		}

		// Field Name: Contact.
		public const Int32 FIXED_DATA_FIELD_COUNT = 1;
		public const Int32 COMMAND_ID = ( int ) ETSECommand.ID_AUTOCONTACT;

		private const Int32 DATA_LEN = 4;	// 4 bytes
		public const Int32 CONTACT_POS = 0;

		/// <summary>
		/// Constructor.
		/// </summary>
		public CmdAutoContact()
			: base( COMMAND_ID, DATA_LEN )
		{ 
		
		}

		/// <summary>
		/// Contact.
		/// </summary>
		public Int32 Contact
		{
			get 
			{
				if ( ( this.Data != null ) && this.Data.Length.Equals( DATA_LEN ) )
				{
					return BitConverter.ToInt32( this.Data, CONTACT_POS );
				}
				else
				{
					return ( Int32 ) UInt32.MinValue;
				}
			}
			set
			{
				byte[] buf = this.Data;

				Array.Copy( BitConverter.GetBytes( value ), 0, buf, CONTACT_POS, sizeof( Int32 ) );

				this.Data = buf;
			}
		}
	}

	/// <summary>
	/// Barcode Insert Command.
	/// 
	/// typedef struct _PBSORTER_BARCODE_INSERT_PACKET
	///	{
	///		int nErrorCode;  						// 0: Success, 1: Not exist Test File, 2: Not exist Bin File, 
	///												// 3: Test File and Bin File not match
	///		char szTestItemName[MAX_ITEM_NAME];		// Lot No
	///		char szBinFileName[MAX_ITEM_NAME];		// Wafer No
	///	}
	/// </summary>
	class CmdBarcodeInsert : TSECommand
	{
		public enum EErrorCode : int
		{ 
			Unknown						= -1,
			Success						= 0,
			NotExistTestFile			= 1,
			NotExistBinFile				= 2,
			TestFileAndBinFileNotMatch	= 3,
		}

		// Field Name : TestItemName, BinFileName.
		public const Int32 FIXED_DATA_FIELD_COUNT = 2;
		public const Int32 COMMAND_ID = ( int ) ETSECommand.ID_BARCODE_INSERT;

		private const Int32 DATA_LEN = 40;		// sizeof( Int32 ) + MAX_ITEM_NAME + MAX_ITEM_NAME = 44 bytes
		//public const Int32 ERROR_CODE_POS = 0;
		public const Int32 TEST_ITEM_NAME_POS = 0;
		public const Int32 BIN_FILE_NAME_POS = TEST_ITEM_NAME_POS + MAX_ITEM_NAME;

		/// <summary>
		/// Constructor.
		/// </summary>
		public CmdBarcodeInsert()
			: base( COMMAND_ID, DATA_LEN )
		{

		}

		/// <summary>
		/// ErrorCode.
		/// </summary>
        //public Int32 ErrorCode
        //{
        //    get 
        //    {
        //        if ( ( this.Data != null ) && ( this.Data.Length >= TEST_ITEM_NAME_POS ) )
        //        {
        //            return BitConverter.ToInt32( this.Data, ERROR_CODE_POS );
        //        }
        //        else
        //        {
        //            return ( Int32 ) UInt32.MinValue;
        //        }
        //    }
        //    set
        //    {
        //        byte[] buf = this.Data;

        //        Array.Copy( BitConverter.GetBytes( value ), 0, buf, ERROR_CODE_POS, sizeof( Int32 ) );

        //        this.Data = buf;
        //    }
        //}

		/// <summary>
		/// Test Item Name.
		/// </summary>
		public Char[] TestItemName
		{
			get
			{
				Char[] buf = new Char[ MAX_ITEM_NAME ];

				if ( ( this.Data != null ) && ( this.Data.Length >= BIN_FILE_NAME_POS ) )
				{
					for ( Int32 index = 0; index < MAX_ITEM_NAME; index++ )
					{
						buf[ index ] = Convert.ToChar( this.Data[ TEST_ITEM_NAME_POS + index ] );
					}
				}
				else
				{
					for ( Int32 index = 0; index < MAX_ITEM_NAME; index++ )
					{
						buf[ index ] = Char.MinValue;
					}
				}

				return buf;
			}
			set
			{
				Int32 index = 0;
				Int32 maxItemNum = 0;
				Byte[] buf = this.Data;

				maxItemNum = value.Length < MAX_ITEM_NAME ? value.Length : MAX_ITEM_NAME;
				for ( index = 0; index < maxItemNum; index++ )
				{
					Array.Copy( BitConverter.GetBytes( value[ index ] ), 0, buf, TEST_ITEM_NAME_POS + index, sizeof( Byte ) );
				}

				this.Data = buf;
			}
		}

		/// <summary>
		/// Bin File Name.
		/// </summary>
		public Char[] BinFileName
		{
			get
			{
				Char[] buf = new Char[ MAX_ITEM_NAME ];

				if ( ( this.Data != null ) && this.Data.Length.Equals( DATA_LEN ) )
				{
					for ( Int32 index = 0; index < MAX_ITEM_NAME; index++ )
					{
						buf[ index ] = Convert.ToChar( this.Data[ BIN_FILE_NAME_POS + index ] );
					}
				}
				else
				{
					for ( Int32 index = 0; index < MAX_ITEM_NAME; index++ )
					{
						buf[ index ] = Char.MinValue;
					}
				}

				return buf;
			}
			set
			{
				Int32 index = 0;
				Int32 maxItemNum = 0;
				Byte[] buf = this.Data;

				maxItemNum = value.Length < MAX_ITEM_NAME ? value.Length : MAX_ITEM_NAME;
				for ( index = 0; index < maxItemNum; index++ )
				{
					Array.Copy( BitConverter.GetBytes( value[ index ] ), 0, buf, BIN_FILE_NAME_POS + index, sizeof( Byte ) );
				}

				this.Data = buf;
			}
		}
	}

	/// <summary>
	/// EOT Command.
	/// 
	/// typedef struct _PBSORTER_CMD_RESULT_DATA
	///	{
	///		char szResultData[MAX_RESULT_DATA];		// Test Result Data
	///	}
	/// 
	/// typedef struct _PBSORTER_EOT_PACKET
	///	{
	///		int iBIN;				// 1,2,3,4,5,6.......
	///		int iGoodNG;			// 1:Good , 2:NG
	///		int nErrorCode;			// 0 : No Error, 1 : IV_Saturation, 2: WL_Saturation
	///		BOOL bTestResultOpen;	// TRUE : Open, FALSE : Not Open
	///		int iResultDataCount;	// Result Data Count
	///		PBSORTER_CMD_RESULT_DATA  ResultDataList[MAX_TEST_ITEM_LIST];
	///	}
	/// </summary>
	class CmdEOT : TSECommand
	{
		public enum EGoodNG : int
		{ 
			Unknown = -1,
			Good	= 1,
			NG		= 2,
		}

		public enum ETestResultOpen : int
		{ 
			NotOpen = 0,
			Open	= 1,
		}

		// Field Name: Bin, GoodNG, ErrorCode, TestResultOpen, ResultDataCount.
		public const Int32 FIXED_DATA_FIELD_COUNT = 5;
		public const Int32 COMMAND_ID = ( int ) ETSECommand.ID_EOT;

		private const Int32 DATA_LEN = 2024;	// sizeof( Int32 ) + sizeof( Int32 ) + sizeof( Int32 ) + 
												// sizeof( Boolean ) + sizeof( Int32 ) +
												// ( MAX_TEST_ITEM_LIST * MAX_RESULT_DATA ) = 2017 bytes
		public const Int32 BIN_POS = 0;
		public const Int32 GOOD_NG_POS = BIN_POS + sizeof( Int32 );
		public const Int32 RESULT_DATA_COUNT_POS = GOOD_NG_POS + sizeof( Int32 );
		public const Int32 ERROR_CODE_POS = RESULT_DATA_COUNT_POS + sizeof( Int32 );
		public const Int32 TEST_RESULT_OPEN_POS = ERROR_CODE_POS + sizeof( Int32 );
		public const Int32 CLAMP_OVER_POS = TEST_RESULT_OPEN_POS + sizeof( Int32 );
		public const Int32 RESULT_DATA_LIST_POS = CLAMP_OVER_POS + sizeof( Int32 );

        PBSorterCMDResultData _buf;

		public class PBSorterCMDResultData
		{
			public Char[] ResultData;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public CmdEOT()
			: base( COMMAND_ID, DATA_LEN )
		{
            _buf = new PBSorterCMDResultData();
            _buf.ResultData = new Char[MAX_RESULT_DATA];
		}

		/// <summary>
		/// Bin.
		/// </summary>
		public Int32 Bin
		{
			get
			{
				if ( ( this.Data != null ) && ( this.Data.Length >= GOOD_NG_POS ) )
				{
					return BitConverter.ToInt32( this.Data, BIN_POS );
				}
				else
				{
					return ( Int32 ) UInt32.MinValue;
				}
			}
			set
			{
				byte[] buf = this.Data;

				Array.Copy( BitConverter.GetBytes( value ), 0, buf, BIN_POS, sizeof( Int32 ) );

				this.Data = buf;
			}
		}

		/// <summary>
		/// Good NG.
		/// </summary>
		public Int32 GoodNG
		{
			get 
			{
				if ( ( this.Data != null ) && ( this.Data.Length >= RESULT_DATA_COUNT_POS ) )
				{
					return BitConverter.ToInt32( this.Data, GOOD_NG_POS );
				}
				else
				{
					return ( Int32 ) UInt32.MinValue;
				}
			}
			set
			{
				byte[] buf = this.Data;

				Array.Copy( BitConverter.GetBytes( value ), 0, buf, GOOD_NG_POS, sizeof( Int32 ) );

				this.Data = buf;
			}
		}

		/// <summary>
		/// Result Data Count.
		/// </summary>
		public Int32 ResultDataCount
		{
			get
			{
				if ( ( this.Data != null ) && ( this.Data.Length >= ERROR_CODE_POS ) )
				{
					return BitConverter.ToInt32( this.Data, RESULT_DATA_COUNT_POS );
				}
				else
				{
					return ( Int32 ) UInt32.MinValue;
				}
			}
			set
			{
				byte[] buf = this.Data;

				Array.Copy( BitConverter.GetBytes( value ), 0, buf, RESULT_DATA_COUNT_POS, sizeof( Int32 ) );

				this.Data = buf;
			}
		}

		/// <summary>
		/// Error Code.
		/// </summary>
		public Int32 ErrorCode
		{
			get 
			{
				if ( ( this.Data != null ) && ( this.Data.Length >= TEST_RESULT_OPEN_POS ) )
				{
					return BitConverter.ToInt32( this.Data, ERROR_CODE_POS );
				}
				else
				{
					return ( Int32 ) UInt32.MinValue;
				}
			}
			set
			{
				byte[] buf = this.Data;

				Array.Copy( BitConverter.GetBytes( value ), 0, buf, ERROR_CODE_POS, sizeof( Int32 ) );

				this.Data = buf;
			}
		}

		/// <summary>
		/// Test Result Open.
		/// </summary>
		public Boolean TestResultOpen
		{
			get 
			{
				if ( ( this.Data != null ) && ( this.Data.Length >= CLAMP_OVER_POS ) )
				{
					return BitConverter.ToBoolean( this.Data, TEST_RESULT_OPEN_POS );
				}
				else
				{
					return false;
				}
			}
			set
			{
				byte[] buf = this.Data;

				Array.Copy( BitConverter.GetBytes( value ), 0, buf, TEST_RESULT_OPEN_POS, sizeof( Boolean ) );

				this.Data = buf;
			}
		}

		/// <summary>
		/// Test Result Open.
		/// </summary>
		public Boolean ClampOver
		{
			get
			{
				if ( ( this.Data != null ) && ( this.Data.Length >= RESULT_DATA_LIST_POS ) )
				{
					return BitConverter.ToBoolean( this.Data, CLAMP_OVER_POS );
				}
				else
				{
					return false;
				}
			}
			set
			{
				byte[] buf = this.Data;

				Array.Copy( BitConverter.GetBytes( value ), 0, buf, CLAMP_OVER_POS, sizeof( Boolean ) );

				this.Data = buf;
			}
		}

		

		/// <summary>
		/// Result Data List.
		/// </summary>
		public PBSorterCMDResultData[] ResultDataList
		{
			get
			{
				Int32 i = 0;
				Int32 j = 0;
				PBSorterCMDResultData[] buf = null;

				buf = new PBSorterCMDResultData[ MAX_TEST_ITEM_LIST ];

				for ( i = 0; i < buf.Length; i++ )
				{
                    buf[i] = new PBSorterCMDResultData();
					buf[i].ResultData = new Char[ MAX_RESULT_DATA ];
				}

				if ( ( this.Data != null ) && this.Data.Length.Equals( DATA_LEN ) )
				{

					for ( i = 0; i < MAX_TEST_ITEM_LIST; i++ )
					{
						for ( j = 0; j < MAX_RESULT_DATA; j++ )
						{
							buf[ i ].ResultData[ j ] = Convert.ToChar(
								this.Data[ ( RESULT_DATA_LIST_POS + i * MAX_RESULT_DATA + j ) ] );
						}
					}
				}
				else
				{
					for ( i = 0; i < MAX_TEST_ITEM_LIST; i++ )
					{
						for ( j = 0; j < MAX_RESULT_DATA; j++ )
						{
							buf[ i ].ResultData[ j ] = Char.MinValue;
						}
					}
				}

				return buf;
			}
			set
			{
				Int32 i = 0;
				Int32 j = 0;
				Int32 maxItemNumI, maxItemNumJ;
				Byte[] buf = this.Data;

				maxItemNumI = value.Length < MAX_TEST_ITEM_LIST ?
					value.Length : MAX_TEST_ITEM_LIST;

				for ( i = 0; i < maxItemNumI; i++ )
				{
                    if (value[i].ResultData == null)
                        continue;

                    maxItemNumJ = value[ i ].ResultData.Length < MAX_RESULT_DATA ?
						value[ i ].ResultData.Length : MAX_RESULT_DATA;

					for ( j = 0; j < maxItemNumJ; j++ )
					{
						Array.Copy( BitConverter.GetBytes( value[ i ].ResultData[ j ] ), 0, buf,
							( RESULT_DATA_LIST_POS + i * MAX_RESULT_DATA + j ), sizeof( Byte ) );
					}
				}

				this.Data = buf;
			}
		}

        /// <summary>
        /// Result Data List.
        /// </summary>
        public PBSorterCMDResultData ResultData(int i)
        {
            if ((this.Data != null) && this.Data.Length.Equals(DATA_LEN))
            {
                Array.Copy(this.Data, RESULT_DATA_LIST_POS + (i * MAX_RESULT_DATA), _buf.ResultData, 0, _buf.ResultData.Length); 
                //for (int j = 0; j < MAX_RESULT_DATA; j++)
                //{
                //    _buf.ResultData[j] = Convert.ToChar(this.Data[RESULT_DATA_LIST_POS + (i * MAX_RESULT_DATA) + j]);
                //}
            }
            else
            {
                Array.Clear(_buf.ResultData, 0, _buf.ResultData.Length);
                //for (int j = 0; j < MAX_RESULT_DATA; j++)
                //{
                //    _buf.ResultData[j] = Char.MinValue;
                //}
            }

            return _buf;
        }
	}

	/// <summary>
	/// Lot End Command.
	/// </summary>
	class CmdLotEnd : TSECommand
	{
		public const Int32 FIXED_DATA_FIELD_COUNT = 0;
		public const Int32 COMMAND_ID = ( int ) ETSECommand.ID_LOT_END;

		private const Int32 DATA_LEN = 0;					// bytes

		/// <summary>
		/// Constructor.
		/// </summary>
		public CmdLotEnd()
			: base( COMMAND_ID, DATA_LEN )
		{

		}
	}

	/// <summary>
	/// SOT Command.
	/// 
	/// struct _PBSORTER_SOT_PACKET
	///	{
	///		BOOL bExistChip;			// Test Chip exist Judgment
	///		long lWaferPositionX;   	// Wafer Position X
	///		long lWaferPositionY;    	// Wafer Position Y
	///		BOOL bAutoSOT;           	// TRUE: AUTO Test, FALSE: Manual Test 
	///		BOOL bNewChip;           	// TRUE: New Chip, FALSE: Re-Test Chip
	///		int  nProbeIndex;        	// Probe Index value
	///	}
	/// </summary>
	class CmdSOT : TSECommand
	{
		// Field Name: ExistChip, WaferPositionX, WaferPositionY, AutoSOT, NewChip, ProbeIndex.
		public const Int32 FIXED_DATA_FIELD_COUNT = 6;
		public const Int32 COMMAND_ID = ( int ) ETSECommand.ID_SOT;

		private const Int32 DATA_LEN = 24;		// sizeof( Boolean ) + sizeof( Int32 ) + sizeof( Int32 ) + 
												// sizeof( Boolean ) + sizeof( Boolean ) + sizeof( Int32 ) = 15 bytes
		public const Int32 EXIST_CHIP_POS = 0;
		public const Int32 WAFER_POSITION_X_POS = EXIST_CHIP_POS + sizeof( Int32 );
		public const Int32 WAFER_POSITION_Y_POS = WAFER_POSITION_X_POS + sizeof( Int32 );
		public const Int32 AUTO_SOT_POS = WAFER_POSITION_Y_POS + sizeof( Int32 );
		public const Int32 NEW_CHIP_POS = AUTO_SOT_POS + sizeof( Int32 );
		public const Int32 PROBE_INDEX_POS = NEW_CHIP_POS + sizeof( Int32 );

		/// <summary>
		/// Constructor.
		/// </summary>
		public CmdSOT()
			: base( COMMAND_ID, DATA_LEN )
		{

		}

		/// <summary>
		/// Exist Chip.
		/// </summary>
		public Boolean ExistChip
		{
			get 
			{
				if ( ( this.Data != null ) && ( this.Data.Length >= WAFER_POSITION_X_POS ) )
				{
					return BitConverter.ToBoolean( this.Data, EXIST_CHIP_POS );
				}
				else
				{
					return false;
				}
			}
			set
			{
				byte[] buf = this.Data;

				Array.Copy( BitConverter.GetBytes( value ), 0, buf, EXIST_CHIP_POS, sizeof( Boolean ) );

				this.Data = buf;
			}
		}

		/// <summary>
		/// Wafer Position X.
		/// </summary>
		public Int32 WaferPositionX
		{
			get 
			{
				if ( ( this.Data != null ) && ( this.Data.Length >= WAFER_POSITION_Y_POS ) )
				{
					return BitConverter.ToInt32( this.Data, WAFER_POSITION_X_POS );
				}
				else
				{
					return ( Int32 ) UInt32.MinValue;
				}
			}
			set
			{
				byte[] buf = this.Data;

				Array.Copy( BitConverter.GetBytes( value ), 0, buf, WAFER_POSITION_X_POS, sizeof( Int32 ) );

				this.Data = buf;
			}
		}

		/// <summary>
		/// Wafer Position Y.
		/// </summary>
		public Int32 WaferPositionY
		{
			get 
			{
				if ( ( this.Data != null ) && ( this.Data.Length >= AUTO_SOT_POS ) )
				{
					return BitConverter.ToInt32( this.Data, WAFER_POSITION_Y_POS );
				}
				else
				{
					return ( Int32 ) UInt32.MinValue;
				}
			}
			set
			{
				byte[] buf = this.Data;

				Array.Copy( BitConverter.GetBytes( value ), 0, buf, WAFER_POSITION_Y_POS, sizeof( Int32 ) );

				this.Data = buf;
			}
		}

		/// <summary>
		/// Auto SOT.
		/// </summary>
		public Boolean AutoSOT
		{
			get 
			{
				if ( ( this.Data != null ) && ( this.Data.Length >= NEW_CHIP_POS ) )
				{
					return BitConverter.ToBoolean( this.Data, AUTO_SOT_POS );
				}
				else
				{
					return false;
				}
			}
			set
			{
				byte[] buf = this.Data;

				Array.Copy( BitConverter.GetBytes( value ), 0, buf, AUTO_SOT_POS, sizeof( Boolean ) );

				this.Data = buf;
			}
		}

		/// <summary>
		/// New Chip.
		/// </summary>
		public Boolean NewChip
		{
			get 
			{
				if ( ( this.Data != null ) && ( this.Data.Length >= PROBE_INDEX_POS ) )
				{
					return BitConverter.ToBoolean( this.Data, NEW_CHIP_POS );
				}
				else
				{
					return false;
				}
			}
			set
			{
				byte[] buf = this.Data;

				Array.Copy( BitConverter.GetBytes( value ), 0, buf, NEW_CHIP_POS, sizeof( Boolean ) );

				this.Data = buf;
			}
		}

		/// <summary>
		/// Probe Index.
		/// </summary>
		public Int32 ProbeIndex
		{
			get 
			{
				if ( ( this.Data != null ) && ( this.Data.Length.Equals( DATA_LEN ) ) )
				{
					return BitConverter.ToInt32( this.Data, PROBE_INDEX_POS );
				}
				else
				{
					return ( Int32 ) UInt32.MinValue;
				}
			}
			set
			{
				byte[] buf = this.Data;

				Array.Copy( BitConverter.GetBytes( value ), 0, buf, PROBE_INDEX_POS, sizeof( Int32 ) );

				this.Data = buf;
			}
		}
	}

	/// <summary>
	/// Test Item Command.
	/// 
	/// typedef struct _PBSORTER_CMD_ITEM_NAME
	///	{
	///		char szItemName[MAX_ITEM_NAME];					// Test Item Name
	///	} 
	/// 
	/// typedef struct _PBSORTER_ITEM_PACKET
	///	{
	///		int iItemCount;									// Test Item Count
	///		PBSORTER_CMD_ITEM ItemList[MAX_TEST_ITEM_LIST];
	///	}
	/// </summary>
	class CmdTestItem : TSECommand
	{
		// Field Name: ItemCount.
		public const Int32 FIXED_DATA_FIELD_COUNT = 1;
		public const Int32 COMMAND_ID = ( int ) ETSECommand.ID_TEST_ITEM;

		private const Int32 DATA_LEN = 2004;		// sizeof( Int32 ) + ( MAX_TEST_ITEM_LIST * MAX_ITEM_NAME ) = 2004 bytes
		public const Int32 ITEM_COUNT_POS = 0;
		public const Int32 ITEM_LIST_POS = ITEM_COUNT_POS + sizeof( Int32 );

		public struct PBSorterCMDItemName
		{
			public Char[] ItemName;
		}

		public CmdTestItem()
			: base( COMMAND_ID, DATA_LEN )
		{ 
		
		}

		/// <summary>
		/// Item Count.
		/// </summary>
		public Int32 ItemCount
		{
			get 
			{
				if ( ( this.Data != null ) && ( this.Data.Length >= ITEM_LIST_POS ) )
				{
					return BitConverter.ToInt32( this.Data, ITEM_COUNT_POS );
				}
				else
				{
					return ( Int32 ) UInt32.MinValue;
				}
			}
			set
			{
				byte[] buf = this.Data;

				Array.Copy( BitConverter.GetBytes( value ), 0, buf, ITEM_COUNT_POS, sizeof( Int32 ) );

				this.Data = buf;
			}
		}

		/// <summary>
		/// Item List.
		/// </summary>
		public PBSorterCMDItemName[] ItemList
		{
			get
			{
				Int32 i = 0;
				Int32 j = 0;
				PBSorterCMDItemName[] buf = null;

				buf = new PBSorterCMDItemName[ MAX_TEST_ITEM_LIST ];

				for ( i = 0; i < buf.Length; i++ )
				{
					buf[ i ].ItemName = new Char[ MAX_ITEM_NAME ];
				}

				if ( ( this.Data != null ) && this.Data.Length.Equals( DATA_LEN ) )
				{
					for ( i = 0; i < MAX_TEST_ITEM_LIST; i++ )
					{
						for ( j = 0; j < MAX_ITEM_NAME; j++ )
						{
							buf[ i ].ItemName[ j ] = Convert.ToChar(
								this.Data[ ( ITEM_LIST_POS + i * MAX_ITEM_NAME + j ) ] );
						}
					}
				}
				else
				{
					for ( i = 0; i < MAX_TEST_ITEM_LIST; i++ )
					{
						for ( j = 0; j < MAX_ITEM_NAME; j++ )
						{
							buf[ i ].ItemName[ j ] = Char.MinValue;
						}
					}
				}

				return buf;
			}
			set
			{
				Int32 i, j;
				Int32 maxItemNumI, maxItemNumJ;
				Byte[] buf = this.Data;

				maxItemNumI = value.Length < MAX_TEST_ITEM_LIST ?
					value.Length : MAX_TEST_ITEM_LIST;

				for ( i = 0; i < maxItemNumI; i++ )
				{
                    if (value[i].ItemName == null)
                        continue;

                    maxItemNumJ = value[ i ].ItemName.Length < MAX_ITEM_NAME ?
						value[ i ].ItemName.Length : MAX_ITEM_NAME;

					for ( j = 0; j < maxItemNumJ; j++ )
					{
						Array.Copy( BitConverter.GetBytes( value[ i ].ItemName[ j ] ), 0, buf,
							( ITEM_LIST_POS + i * MAX_ITEM_NAME + j ), sizeof( Byte ) );
					}
				}

				this.Data = buf;
			}
		}
	}

	/// <summary>
	/// Wafer In Command.
	/// 
	/// typedef struct _PBSORTER_WAFER_IN_COMMAND
	///	{
	///		char szLotNo[MAX_ITEM_NAME];                         	// Lot No
	///		char szWaferNo[MAX_ITEM_NAME];                      	// Wafer No
	///		char szOperatorName[MAX_ITEM_NAME];                		// Operator Name
	///	} 
	/// </summary>
	class CmdWaferIn : TSECommand
	{
		// Field Name: LotNo, WaferNo, OperatorName.
		public const Int32 FIXED_DATA_FIELD_COUNT = 3;
		public const Int32 COMMAND_ID = ( int ) ETSECommand.ID_WAFER_IN;

		private const Int32 DATA_LEN = 60;		// MAX_ITEM_NAME + MAX_ITEM_NAME + MAX_ITEM_NAME = 60 bytes
		public const Int32 LOT_NO_POS = 0;
		public const Int32 WAFER_NO_POS = LOT_NO_POS + MAX_ITEM_NAME;
		public const Int32 OPERATOR_POS = WAFER_NO_POS + MAX_ITEM_NAME;

		/// <summary>
		/// Constructor.
		/// </summary>
		public CmdWaferIn()
			: base( COMMAND_ID, DATA_LEN )
		{

		}

		/// <summary>
		/// Lot Number.
		/// </summary>
		public Char[] LotNo
		{
			get
			{
				Char[] buf = new Char[ MAX_ITEM_NAME ];

				if ( ( this.Data != null ) && this.Data.Length >= WAFER_NO_POS )
				{
					for ( Int32 index = 0; index < MAX_ITEM_NAME; index++ )
					{
						buf[ index ] = Convert.ToChar( this.Data[ LOT_NO_POS + index ] );
					}
				}
				else
				{
					for ( Int32 index = 0; index < MAX_ITEM_NAME; index++ )
					{
						buf[ index ] = Char.MinValue;
					}
				}

				return buf;
			}
			set
			{
				Int32 index = 0;
				Int32 maxItemNum = 0;
				Byte[] buf = this.Data;

				maxItemNum = value.Length < MAX_ITEM_NAME ? value.Length : MAX_ITEM_NAME;
				for ( index = 0; index < maxItemNum; index++ )
				{
					Array.Copy( BitConverter.GetBytes( value[ index ] ), 0, buf, LOT_NO_POS + index, sizeof( Byte ) );
				}

				this.Data = buf;
			}
		}

		/// <summary>
		/// Wafer Number.
		/// </summary>
		public Char[] WaferNo
		{
			get
			{
				Char[] buf = new Char[ MAX_ITEM_NAME ];

				if ( ( this.Data != null ) && this.Data.Length >= OPERATOR_POS )
				{
					for ( Int32 index = 0; index < MAX_ITEM_NAME; index++ )
					{
						buf[ index ] = Convert.ToChar( this.Data[ WAFER_NO_POS + index ] );
					}
				}
				else
				{
					for ( Int32 index = 0; index < MAX_ITEM_NAME; index++ )
					{
						buf[ index ] = Char.MinValue;
					}
				}

				return buf;
			}
			set
			{
				Int32 index = 0;
				Int32 maxItemNum = 0;
				Byte[] buf = this.Data;

				maxItemNum = value.Length < MAX_ITEM_NAME ? value.Length : MAX_ITEM_NAME;
				for ( index = 0; index < maxItemNum; index++ )
				{
					Array.Copy( BitConverter.GetBytes( value[ index ] ), 0, buf, WAFER_NO_POS + index, sizeof( Byte ) );
				}

				this.Data = buf;
			}
		}

		public Char[] OperatorName
		{
			get
			{
				Char[] buf = new Char[ MAX_ITEM_NAME ];

				if ( ( this.Data != null ) && this.Data.Length.Equals( DATA_LEN ) )
				{
					for ( Int32 index = 0; index < MAX_ITEM_NAME; index++ )
					{
						buf[ index ] = Convert.ToChar( this.Data[ OPERATOR_POS + index ] );
					}
				}
				else
				{
					for ( Int32 index = 0; index < MAX_ITEM_NAME; index++ )
					{
						buf[ index ] = Char.MinValue;
					}
				}

				return buf;
			}
			set
			{
				Int32 index = 0;
				Int32 maxItemNum = 0;
				Byte[] buf = this.Data;

				maxItemNum = value.Length < MAX_ITEM_NAME ? value.Length : MAX_ITEM_NAME;
				for ( index = 0; index < maxItemNum; index++ )
				{
					Array.Copy( BitConverter.GetBytes( value[ index ] ), 0, buf, OPERATOR_POS + index, sizeof( Byte ) );
				}

				this.Data = buf;
			}
		}
	}

	/// <summary>
	///  Wafer Scan End Command.
	/// 
	/// typedef struct _PBSORTER_WAFER_SCAN_END_PACKET
	///	{
	///		char szProductName[MAX_PRODUCT_NAME];	// Product Name
	///		long lChipCount;                        // Chip Count
	///		long lXSize;							// Chip X Size
	///		long lYSize;							// Chip Y Size
	///		long lXMax;								// Wafer X Address Max 
	///		long lXMin;								// Wafer X Address Min
	///		long lYMax;								// Wafer Y Address Max
	///		long lYMin;								// Wafer Y Address Min
	///	}
	/// </summary>
	class CmdWaferScanEnd : TSECommand
	{
		// Field Name: ProductName, ChipCount, XSize,YSize, XMax, XMin, YMax, YMin.
		public const Int32 FIXED_DATA_FIELD_COUNT = 8;
		public const Int32 COMMAND_ID = ( int ) ETSECommand.ID_WAFER_SCAN_END;

		private const Int32 DATA_LEN = 48;		// MAX_PRODUCT_NAME + ( 7 * sizeof( Int32 ) ) = 48 bytes
		public const Int32 PRODUCT_NAME_POS = 0;
		public const Int32 CHIP_COUNT_POS = PRODUCT_NAME_POS + MAX_PRODUCT_NAME;
		public const Int32 X_SIZE_POS = CHIP_COUNT_POS + sizeof( Int32 );
		public const Int32 Y_SIZE_POS = X_SIZE_POS + sizeof( Int32 );
		public const Int32 X_MAX_POS = Y_SIZE_POS + sizeof( Int32 );
		public const Int32 X_MIN_POS = X_MAX_POS + sizeof( Int32 );
		public const Int32 Y_MAX_POS = X_MIN_POS + sizeof( Int32 );
		public const Int32 Y_MIN_POS = Y_MAX_POS + sizeof( Int32 );

		/// <summary>
		/// Constructor.
		/// </summary>
		public CmdWaferScanEnd()
			: base( COMMAND_ID, DATA_LEN )
		{

		}

		/// <summary>
		/// Product Name.
		/// </summary>
		public Char[] ProductName
		{
			get
			{
				Char[] buf = new Char[ MAX_PRODUCT_NAME ];

				if ( ( this.Data != null ) && this.Data.Length >= CHIP_COUNT_POS )
				{
					for ( Int32 index = 0; index < MAX_PRODUCT_NAME; index++ )
					{
						buf[ index ] = Convert.ToChar( this.Data[ PRODUCT_NAME_POS + index ] );
					}
				}
				else
				{
					for ( Int32 index = 0; index < MAX_PRODUCT_NAME; index++ )
					{
						buf[ index ] = Char.MinValue;
					}
				}

				return buf;
			}
			set
			{
				Int32 index;
				Int32 maxItemNum;
				Byte[] buf = this.Data;

				maxItemNum = value.Length < ( CHIP_COUNT_POS - PRODUCT_NAME_POS ) ?
					value.Length : ( CHIP_COUNT_POS - PRODUCT_NAME_POS );
				for ( index = 0; index < maxItemNum; index++ )
				{
					Array.Copy( BitConverter.GetBytes( value[ index ] ), 0, buf, PRODUCT_NAME_POS + index, sizeof( Byte ) );
				}

				this.Data = buf;
			}
		}

		/// <summary>
		/// Chip Count.
		/// </summary>
		public Int32 ChipCount
		{
			get 
			{
				if ( ( this.Data != null ) && ( this.Data.Length >= X_SIZE_POS ) )
				{
					return BitConverter.ToInt32( this.Data, CHIP_COUNT_POS );
				}
				else
				{
					return ( Int32 ) UInt32.MinValue;
				}
			}
			set
			{
				byte[] buf = this.Data;

				Array.Copy( BitConverter.GetBytes( value ), 0, buf, CHIP_COUNT_POS, sizeof( Int32 ) );

				this.Data = buf;
			}
		}

		/// <summary>
		/// X Size.
		/// </summary>
		public Int32 XSize
		{
			get 
			{
				if ( ( this.Data != null ) && ( this.Data.Length >= Y_SIZE_POS ) )
				{
					return BitConverter.ToInt32( this.Data, X_SIZE_POS );
				}
				else
				{
					return ( Int32 ) UInt32.MinValue;
				}
			}
			set
			{
				byte[] buf = this.Data;

				Array.Copy( BitConverter.GetBytes( value ), 0, buf, X_SIZE_POS, sizeof( Int32 ) );

				this.Data = buf;
			}
		}

		/// <summary>
		/// Y Size.
		/// </summary>
		public Int32 YSize
		{
			get 
			{
				if ( ( this.Data != null ) && ( this.Data.Length >= X_MAX_POS ) )
				{
					return BitConverter.ToInt32( this.Data, Y_SIZE_POS );
				}
				else
				{
					return ( Int32 ) UInt32.MinValue;
				}
			}
			set
			{
				byte[] buf = this.Data;

				Array.Copy( BitConverter.GetBytes( value ), 0, buf, Y_SIZE_POS, sizeof( Int32 ) );

				this.Data = buf;
			}
		}

		/// <summary>
		/// X Max.
		/// </summary>
		public Int32 XMax
		{
			get 
			{
				if ( ( this.Data != null ) && ( this.Data.Length >= X_MIN_POS ) )
				{
					return BitConverter.ToInt32( this.Data, X_MAX_POS );
				}
				else
				{
					return ( Int32 ) UInt32.MinValue;
				}
			}
			set
			{
				byte[] buf = this.Data;

				Array.Copy( BitConverter.GetBytes( value ), 0, buf, X_MAX_POS, sizeof( Int32 ) );

				this.Data = buf;
			}
		}

		/// <summary>
		/// X Min.
		/// </summary>
		public Int32 XMin
		{
			get 
			{
				if ( ( this.Data != null ) && ( this.Data.Length >= Y_MAX_POS ) )
				{
					return BitConverter.ToInt32( this.Data, X_MIN_POS );
				}
				else
				{
					return ( Int32 ) UInt32.MinValue;
				}
			}
			set
			{
				byte[] buf = this.Data;

				Array.Copy( BitConverter.GetBytes( value ), 0, buf, X_MIN_POS, sizeof( Int32 ) );

				this.Data = buf;
			}
		}

		/// <summary>
		/// Y Max.
		/// </summary>
		public Int32 YMax
		{
			get 
			{
				if ( ( this.Data != null ) && ( this.Data.Length >= Y_MIN_POS ) )
				{
					return BitConverter.ToInt32( this.Data, Y_MAX_POS );
				}
				else
				{
					return ( Int32 ) UInt32.MinValue;
				}
			}
			set
			{
				byte[] buf = this.Data;

				Array.Copy( BitConverter.GetBytes( value ), 0, buf, Y_MAX_POS, sizeof( Int32 ) );

				this.Data = buf;
			}
		}

		/// <summary>
		/// Y Min.
		/// </summary>
		public Int32 YMin
		{
			get 
			{
				if ( ( this.Data != null ) && ( this.Data.Length.Equals( DATA_LEN ) ) )
				{
					return BitConverter.ToInt32( this.Data, Y_MIN_POS );
				}
				else
				{
					return ( Int32 ) UInt32.MinValue;
				}
			}
			set
			{
				byte[] buf = this.Data;

				Array.Copy( BitConverter.GetBytes( value ), 0, buf, Y_MIN_POS, sizeof( Int32 ) );

				this.Data = buf;
			}
		}
	}
}
