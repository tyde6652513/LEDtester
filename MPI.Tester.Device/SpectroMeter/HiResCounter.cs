using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace MPI.Tester.Device.SpectroMeter
{
    public class HiResCounter
    {
        [DllImport("kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(ref   Int64 lpFrequency);

        [DllImport("kernel32.dll")]
        private static extern bool QueryPerformanceCounter(ref   Int64 lpPerformanceCount);

        private long _freq;
        private long _start;
        private long _end;

        public HiResCounter()
        {
            _freq = 0;
            _start = 0;
            _end = 0;

            if (!QueryPerformanceFrequency(ref   _freq))
            {
                Console.WriteLine("not   supported.");
                return;
            }
        }

        public void start()
        {
            if (!QueryPerformanceCounter(ref   _start))
            {
                Console.WriteLine("query   start   failed.");
                return;
            }
        }

        public void stop()
        {
            if (!QueryPerformanceCounter(ref   _end))
            {
                Console.WriteLine("query   end   failed.");
                return;
            }
        }

        public double getDuration()
        {
            //Console.WriteLine("frequency   =   {0},   total   time   {1}",
            //    frequency, (double)(end - start) / ((double)frequency));
            if (_freq == 0)
                return 0.0;

            return ( (double)( _end - _start) ) / ((double) _freq) ;


        } 

    }
}
