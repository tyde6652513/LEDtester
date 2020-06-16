using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MPI.RemoteControl.Tester
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct TEST_RESULT_DATA
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 500)]
        public double[] ResultData;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct DIE_DATA
    {
        public int Col;
        public int Row;
        public int Bin;
        public int Pass;
        public int DataCount;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string BinCode;
 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 500)]
        public double[] TestResults;

        public void Init()
        {
            TestResults = new double[500];
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct MUTIL_DIE_DATA_LIST
    {
        public int Count;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public DIE_DATA[] DieDataArray;

        public void Init()
        {
            DieDataArray = new DIE_DATA[128];

            for (int i = 0; i < DieDataArray.Length; ++i)
            {
                DieDataArray[i].Init();
            }
        }
    }
}
