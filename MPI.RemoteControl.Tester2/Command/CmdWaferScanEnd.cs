using System;
using System.Collections.Generic;
using System.Text;
using MPI.RemoteControl2.Tester.ConstDefinition;

namespace MPI.RemoteControl2.Tester.Mpi.Command
{
    /// <summary>
    ///  Wafer Scan End Command.
    /// 
    /// typedef struct _PBSORTER_WAFER_SCAN_END_PACKET
    ///	{
    ///		char szProductName[MAX_PRODUCT_NAME];  // Product Name
    ///		long lChipCount;                       // Chip Count
    ///		long lXSize;						   // Chip X Size
    ///		long lYSize;						   // Chip Y Size
    ///		long lXMax;							   // Wafer X Address Max 
    ///		long lXMin;							   // Wafer X Address Min
    ///		long lYMax;							   // Wafer Y Address Max
    ///		long lYMin;							   // Wafer Y Address Min
	///		int MainDirection;					   // Main probe dircetion, X-Axis = 0, Y-Axis = 1,					// T200 appended
	///		int XInitDirection;				       // X initial direction, Left to Right = 0, Right to Left = 1,	// T200 appended
	///		int YInitDirection;					   // Y initial direction, Up to Down = 0, Down to Up = 1,			// T200 appended
	/// }
    /// </summary>
	public class CmdWaferScanEnd : CmdMPIBased
	{
		public static MPIDS7600ConstDefinitionBase Const = CmdMPIBased.ConstDefinition;

		// CommandID.
		public const Int32 COMMAND_ID = (int)ETSECommand.ID_WAFER_SCAN_END;

		// Length		// MAX_PRODUCT_NAME + 7 * sizeof(Int32)
		public static Int32 DATA_LEN = Const.MAX_PRODUCT_NAME + 7 * sizeof(Int32) + Const.MAX_PROBE_DIRECTION_COUNT * sizeof(Int32);

		// Position
		public static Int32 PRODUCT_NAME_POS = 0;
		public static Int32 CHIP_COUNT_POS = PRODUCT_NAME_POS + Const.MAX_PRODUCT_NAME;
		public static Int32 X_SIZE_POS = CHIP_COUNT_POS + sizeof(Int32);
		public static Int32 Y_SIZE_POS = X_SIZE_POS + sizeof(Int32);
		public static Int32 X_MAX_POS = Y_SIZE_POS + sizeof(Int32);
		public static Int32 X_MIN_POS = X_MAX_POS + sizeof(Int32);
		public static Int32 Y_MAX_POS = X_MIN_POS + sizeof(Int32);
		public static Int32 Y_MIN_POS = Y_MAX_POS + sizeof(Int32);
		public static Int32 PROBE_MOVE_DIR_POS = Y_MIN_POS + sizeof(Int32);
		public static Int32 PROBE_X_INIT_DIR_POS = PROBE_MOVE_DIR_POS + sizeof(Int32);
		public static Int32 PROBE_Y_INIT_DIR_POS = PROBE_X_INIT_DIR_POS + sizeof(Int32);

		/// <summary>
		/// Constructor.
		/// </summary>
		public CmdWaferScanEnd()
			: base(COMMAND_ID, DATA_LEN)
		{

		}

		/// <summary>
		/// Product Name.
		/// </summary>
		public char[] ProductName
		{
			get
			{
				return this.GetData(Const.MAX_PRODUCT_NAME, PRODUCT_NAME_POS);
			}
			set
			{
				this.SetData(Const.MAX_PRODUCT_NAME, PRODUCT_NAME_POS, value);
			}
		}

		/// <summary>
		/// Chip Count.
		/// </summary>
		public Int32 ChipCount
		{
			get
			{
				return this.GetInt32Data(CHIP_COUNT_POS);
			}
			set
			{
				this.SetInt32Data(CHIP_COUNT_POS, value);
			}
		}

		/// <summary>
		/// X Size.
		/// </summary>
		public Int32 XSize
		{
			get
			{
				return this.GetInt32Data(X_SIZE_POS);
			}
			set
			{
				this.SetInt32Data(X_SIZE_POS, value);
			}
		}

		/// <summary>
		/// Y Size.
		/// </summary>
		public Int32 YSize
		{
			get
			{
				return this.GetInt32Data(Y_SIZE_POS);
			}
			set
			{
				this.SetInt32Data(Y_SIZE_POS, value);
			}
		}

		/// <summary>
		/// X Max.
		/// </summary>
		public Int32 XMax
		{
			get
			{
				return this.GetInt32Data(X_MAX_POS);
			}
			set
			{
				this.SetInt32Data(X_MAX_POS, value);
			}
		}

		/// <summary>
		/// X Min.
		/// </summary>
		public Int32 XMin
		{
			get
			{
				return this.GetInt32Data(X_MIN_POS);
			}
			set
			{
				this.SetInt32Data(X_MIN_POS, value);
			}
		}

		/// <summary>
		/// Y Max.
		/// </summary>
		public Int32 YMax
		{
			get
			{
				return this.GetInt32Data(Y_MAX_POS);
			}
			set
			{
				this.SetInt32Data(Y_MAX_POS, value);
			}
		}

		/// <summary>
		/// Y Min.
		/// </summary>
		public Int32 YMin
		{
			get
			{
				return this.GetInt32Data(Y_MIN_POS);
			}
			set
			{
				this.SetInt32Data(Y_MIN_POS, value);
			}
		}

		/// <summary>
		/// Probe main move dir. X:0, Y:1
		/// </summary>
		private Int32 _ProbeMoveDirection
		{
			get
			{
				return this.GetInt32Data(PROBE_MOVE_DIR_POS);
			}
			set
			{
				this.SetInt32Data(PROBE_MOVE_DIR_POS, value);
			}
		}

		/// <summary>
		/// X initial probe direction. Left to Right:0, Right to Left:1
		/// </summary>
		private Int32 _XInitDirection
		{
			get
			{
				return this.GetInt32Data(PROBE_X_INIT_DIR_POS);
			}
			set
			{
				this.SetInt32Data(PROBE_X_INIT_DIR_POS, value);
			}
		}

		/// <summary>
		/// Y initial probe direction. Up to Down:0, Down to Up:1
		/// </summary>
		private Int32 _YInitDirection
		{
			get
			{
				return this.GetInt32Data(PROBE_Y_INIT_DIR_POS);
			}
			set
			{
				this.SetInt32Data(PROBE_Y_INIT_DIR_POS, value);
			}
		}

		/// <summary>
		/// Probe main move direction
		/// </summary>
		public EProbeMainDir ProbeMoveDirection
		{
			get
			{
				return (EProbeMainDir)this._ProbeMoveDirection;
			}
			set
			{
				this._ProbeMoveDirection = (int)value;
			}
		}

		/// <summary>
		/// X initial probe direction.
		/// </summary>
		public EXInitialDir ProbeXInitDirection
		{
			get
			{
				return (EXInitialDir)this._XInitDirection;
			}
			set
			{
				this._XInitDirection = (int)value;
			}
		}

		/// <summary>
		/// Y initial probe direction.
		/// </summary>
		public EYInitialDir ProbeYInitDirection
		{
			get
			{
				return (EYInitialDir)this._YInitDirection;
			}
			set
			{
				this._YInitDirection = (int)value;
			}
		}

		public enum EProbeMainDir
		{
			X = 0,
			Y = 1,
		}

		public enum EXInitialDir
		{
			LeftToRight = 0,
			RightToLeft = 1,
		}

		public enum EYInitialDir
		{
            DownToUp = 0,
			UpToDown = 1,
		}
	}
}
