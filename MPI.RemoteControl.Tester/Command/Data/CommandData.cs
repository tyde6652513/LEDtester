using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MPI.RemoteControl.Tester
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public class ContactCheckEOTData
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public bool[] Pass;

        public ContactCheckEOTData()
        {
            Pass = new bool[128];
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public class AutoChCalibStartData
    {
        public int MultiDieChannelXCount;
        public int MultiDieChannelYCount;
        public int ErrorCode;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public class AutoChCalibEndData
    {
        public int ErrorCode;
    }
}
