using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using MPI.RemoteControl.Tester.ConstDefinition;

namespace MPI.RemoteControl.Tester.Command
{
    /// <summary>
    /// MutilDie EOT Command.
    /// 
    /// typedef struct _PBSORTER_CMD_RESULT_DATA
    ///	{
    ///		double szResultData[MAX_RESULT_DATA];		// Test Result Data
    ///	}
    /// 
    /// typedef struct DIE_DATA
    ///	{
    ///     int nCol;
    ///     int nRow;
    ///     int nBin;
    ///     BOOL bPass;
    ///     int nItemCount;
    ///     char szBinCode[MAX_ITEM_NAME]; 
    ///     PBSORTER_CMD_RESULT_DATA  ResultDataList[MAX_TEST_ITEM_LIST];
    ///	}
    /// 
    /// typedef struct MUTIL_DIE_TEST_DATA
    ///	{
    ///     int nDieCount;
    ///     DIE_TEST_DATA  DieData[128];
    ///	}
    public class CmdMutiDieEOT : MPIDS7600Command
    {
        public static MPIDS7600ConstDefinitionBase Const = MPIDS7600Command.ConstDefinition;

        // Const
        public const Int32 MAX_CH_NUM = 128;

        // CommandID.
        public const Int32 COMMAND_ID = (int)ETSECommand.ID_MUTIL_DIE_EOT;

        // Length
        public static Int32 DIE_DATA_LEN = (5 * sizeof(Int32)) + Const.MAX_ITEM_NAME + (sizeof(double) * Const.MAX_TEST_ITEM_LIST);
        public static Int32 DATA_LEN = sizeof(Int32) + DIE_DATA_LEN * MAX_CH_NUM;

        // Position
        public static Int32 DIE_COUNT_POS = 0;
        public static Int32 COL_POS = 0;
        public static Int32 ROW_POS = COL_POS + sizeof(Int32);
        public static Int32 BIN_POS = ROW_POS + sizeof(Int32);
        public static Int32 PASS_POS = BIN_POS + sizeof(Int32);
        public static Int32 ITEM_COUNT_POS = PASS_POS + sizeof(Int32);
        public static Int32 BIN_CODE = ITEM_COUNT_POS + sizeof(Int32);
        public static Int32 RESULT_DATA_LIST_POS = BIN_CODE + sizeof(Int32);

        /// <summary>
        /// Constructor.
        /// </summary>
        public CmdMutiDieEOT()
            : base(COMMAND_ID, DATA_LEN)
        {
        }

        /// <summary>
        /// Die Count
        /// </summary>
        public Int32 DieCount
        {
            get
            {
                return this.GetInt32Data(DIE_COUNT_POS);
            }
            set
            {
                this.SetInt32Data(DIE_COUNT_POS, value);
            }
        }

        private byte[] DoubleToString(double[] source)
        {
            // Initialize unmanged memory to hold the array.
            int size = Marshal.SizeOf(source[0]) * source.Length;

            IntPtr pnt = Marshal.AllocHGlobal(size);

            byte[] szDestination = new byte[size];
            try
            {
                // Copy the array to unmanaged memory.
                Marshal.Copy(source, 0, pnt, source.Length);
                Marshal.Copy(pnt, szDestination, 0, szDestination.Length);

            }
            finally
            {
                // Free the unmanaged memory.
                Marshal.FreeHGlobal(pnt);
            }

            return szDestination;
        }

        private static Byte[] StructToBytes(Object structure)
        {
            Int32 size = Marshal.SizeOf(structure);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(structure, buffer, false);
                Byte[] bytes = new Byte[size];
                Marshal.Copy(buffer, bytes, 0, size);

                return bytes;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        private static Object BytesToStruct(Byte[] bytes, Type strcutType)
        {
            Int32 size = Marshal.SizeOf(strcutType);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);

                return Marshal.PtrToStructure(buffer, strcutType);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        public bool SetMutilDieData(MUTIL_DIE_DATA_LIST dieData)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            double t1, t2, t3;

            sw.Reset();
            sw.Start();

            byte[] bytes = StructToBytes(dieData);
            this.SetData(bytes.Length, 0, bytes);

            sw.Stop();
            t1 = sw.ElapsedMilliseconds;

            return true;
        }

        public MUTIL_DIE_DATA_LIST GetMutilDieData()
        {
            MUTIL_DIE_DATA_LIST dieDataList = new MUTIL_DIE_DATA_LIST();

            dieDataList = (MUTIL_DIE_DATA_LIST)BytesToStruct(this.Data, typeof(MUTIL_DIE_DATA_LIST));

            return dieDataList;
        }
    }
}
