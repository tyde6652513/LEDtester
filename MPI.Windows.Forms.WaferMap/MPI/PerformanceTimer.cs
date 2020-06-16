using System;
using System.Threading;
using System.Runtime.InteropServices;

namespace MPI
{
	/// <summary>
	/// High Performance Counter / Timer
	/// </summary>
	public class HiTimer
	{
		[DllImport( "Kernel32.dll" )]
		private static extern bool QueryPerformanceCounter( out long count );

		[DllImport( "Kernel32.dll" )]
		private static extern bool QueryPerformanceFrequency( out long frequency );

		private readonly static float _fFrequency;

		static HiTimer()
		{
			long freq;
			if ( QueryPerformanceFrequency( out freq ) == false )
				throw new NotSupportedException( "QueryPerformanceFrequency" );

			_fFrequency = ( float ) ( freq / 1000 );
		}

		/// <summary>
		/// Evaluate duration in milli-second
		/// </summary>
		public static float Evaluate( long tick )
		{
			return ( float ) ( HiTimer.Tick - tick ) / _fFrequency;
		}

		public static float EvaluateEx( ref long tick )
		{
			long now = HiTimer.Tick;
			float elapsed = ( ( float ) ( now - tick ) ) / _fFrequency;
			tick = now;
			return elapsed;
		}

		public static long Tick
		{
			get
			{
				long count;

				QueryPerformanceCounter( out count );

				return count;
			}
		}
	}
}
