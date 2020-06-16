using System;
using System.Collections.Generic;
using System.Text;
using MPI.RemoteControl.Tester.ConstDefinition;

namespace MPI.RemoteControl.Tester.Command
{
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
    ///		int iBIN;			    // 1,2,3,4,5,6.......
	///     int iGoodNG;		    // 1:Good , 2:NG
	///     int iResultDataCount;	// Result Data Count
	///     int nErrorCode;		    // 0 : No Error, 1 : IV_Saturation, 2: WL_Saturation, 3: IV_WL
	///     BOOL bTestResultOpen;	// TRUE : Open, FALSE : Not Open
    ///     BOOL bClampOver;    	// TRUE : Clamp Over, FALSE : Not Clamp Over
    ///     PBSORTER_CMD_RESULT_DATA  ResultDataList[MAX_TEST_ITEM_LIST];
    ///	}
    /// </summary>
    public class CmdEOT : MPIDS7600Command
    {
        public enum EOTResultCode
        {
            Undefined = -1,
            NoError = 0,
            SaturationIV = 1,
            SaturationWL = 2,
            SaturationIV_WL = 3,
            NFSaturation = 4,  // 20180603 Paul Define, NF count is Saturation and popup the error message
            FFSaturation =5
        }
        
		public static MPIDS7600ConstDefinitionBase Const = MPIDS7600Command.ConstDefinition;

		// CommandID.
        public const Int32 COMMAND_ID = (int)ETSECommand.ID_EOT;

		// Length		// 6 * sizeof(Int32) + MAX_RESULT_DATA * MAX_TEST_ITEM_LIST
		public static Int32 DATA_LEN = 6 * sizeof(Int32) + Const.MAX_RESULT_DATA * Const.MAX_TEST_ITEM_LIST;

        // Position
		public static Int32 BIN_POS = 0;
		public static Int32 GOOD_NG_POS = BIN_POS + sizeof(Int32);
		public static Int32 RESULT_DATA_COUNT_POS = GOOD_NG_POS + sizeof(Int32);
		public static Int32 ERROR_CODE_POS = RESULT_DATA_COUNT_POS + sizeof(Int32);
		public static Int32 TEST_RESULT_OPEN_POS = ERROR_CODE_POS + sizeof(Int32);
		public static Int32 CLAMP_OVER_POS = TEST_RESULT_OPEN_POS + sizeof(Int32);
		public static Int32 RESULT_DATA_LIST_POS = CLAMP_OVER_POS + sizeof(Int32);

        /// <summary>
        /// Constructor.
        /// </summary>
        public CmdEOT()
            : base(COMMAND_ID, DATA_LEN)
        {

        }

        /// <summary>
        /// Bin.
        /// </summary>
        public Int32 Bin
        {
            get
            {
                return this.GetInt32Data(BIN_POS);
            }
            set
            {
                this.SetInt32Data(BIN_POS, value);
            }
        }

        /// <summary>
        /// Good NG.
        /// </summary>
        public Int32 GoodNG
        {
            get
            {
                return this.GetInt32Data(GOOD_NG_POS);
            }
            set
            {
                this.SetInt32Data(GOOD_NG_POS, value);
            }
        }

        /// <summary>
        /// Result Data Count.
        /// </summary>
        public Int32 ResultDataCount
        {
            get
            {
                return this.GetInt32Data(RESULT_DATA_COUNT_POS);
            }
            set
            {
                this.SetInt32Data(RESULT_DATA_COUNT_POS, value);
            }
        }


        /// <summary>
        /// Error Code.
        /// </summary>
        public Int32 nErrorCode
        {
            get
            {
                return this.GetInt32Data(ERROR_CODE_POS);
            }
            set
            {
                this.SetInt32Data(ERROR_CODE_POS, value);
            }
        }

        /// <summary>
        /// Test Result Open.
        /// </summary>
        public Boolean TestResultOpen
        {
            get
            {
                return this.GetBoolData(TEST_RESULT_OPEN_POS);
            }
            set
            {
                this.SetBoolData(TEST_RESULT_OPEN_POS, value);
            }
        }

        /// <summary>
        /// Error Code.
        /// </summary>
        public EOTResultCode ErrorCode
        {
            get
            {
                EOTResultCode code = EOTResultCode.Undefined;

                return Enum.TryParse<EOTResultCode>(nErrorCode.ToString(), out code) ? code : EOTResultCode.Undefined;
            }
            set
            {
                this.nErrorCode = value.GetHashCode();
            }
        }

        /// <summary>
        /// Test Result Open.
        /// </summary>
        public Boolean ClampOver
        {
            get
            {
                return this.GetBoolData(CLAMP_OVER_POS);
            }
            set
            {
                this.SetBoolData(CLAMP_OVER_POS, value);
            }
        }

        /// <summary>
        /// Get Result Data.
        /// </summary>
        public char[] GetResultData(int i)
        {
			if (i < 0 || i >= Const.MAX_TEST_ITEM_LIST) return null;

			return this.GetData(Const.MAX_RESULT_DATA, RESULT_DATA_LIST_POS + (i * Const.MAX_RESULT_DATA));
        }

        /// <summary>
        /// Get Result Data.
        /// </summary>
        public string GetResultDataString(int i)
        {
            return new string(GetResultData(i));
        }

        /// <summary>
        /// Set Result Data.
        /// </summary>
        public void SetResultData(int i, char[] resultData)
        {
			if (i < 0 || i >= Const.MAX_TEST_ITEM_LIST) return;
			//if (resultData.Length > Const.MAX_RESULT_DATA) return;

			int startPos = RESULT_DATA_LIST_POS + (i * Const.MAX_RESULT_DATA);
			Array.Clear(this.Data, startPos, Const.MAX_RESULT_DATA);
            this.SetData(resultData.Length, startPos, resultData); 
        }

        /// <summary>
        /// Set Result Data.
        /// </summary>
        public void SetResultData(int i, string resultData)
        {
            SetResultData(i, resultData.ToCharArray());
        }   
    }
}
