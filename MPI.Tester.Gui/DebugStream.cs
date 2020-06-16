using System;
using System.Xml;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text;
using System.IO;

namespace MPI.MCF.Log
{
	public class DebugStreamWriter : System.IO.StreamWriter
	{
		/// <summary>
		/// Debug Info File Size: 8 MB
		/// </summary>
		private const int DEFAULT_DEBUG_SIZE = 1024 * 8000;
		/// <summary>
		/// Recommended flush interval: 2 seconds
		/// </summary>
		private const int DEFAULT_FLUSH_INTERVAL = 2 * 1000;
		private long _lastWriteTick;

		public DebugStreamWriter( string path )
			: this( path, DEFAULT_DEBUG_SIZE )
		{
		}

		public DebugStreamWriter( string path, int bufferSize )
			: base( path, false, Encoding.UTF8, bufferSize )
		{
			base.AutoFlush = true;
			_lastWriteTick = HiTimer.Tick;
		}

		public override Encoding Encoding
		{
			get
			{
				return Encoding.UTF8;
			}
		}

		/// <summary>
		/// Provide function for console ( debug level ) 
		/// </summary>
		/// <param name="value"></param>
		public override void Write( string value )
		{
			base.Write( DateTime.Now.ToString( "yyyy/MM/dd HH:mm:ss.ffff," ) );
			base.Write( value );
			// force to flush, auto
			this.Flush();
		}

		public override void WriteLine( string value )
		{
			base.Write( DateTime.Now.ToString( "yyyy/MM/dd HH:mm:ss.ffff," ) );
			base.WriteLine( value );
			// force to flush, auto
			this.Flush();
		}

		public override void Flush()
		{
			if ( HiTimer.Evaluate( _lastWriteTick ) > DEFAULT_FLUSH_INTERVAL )
			{
				base.Flush();

				if ( base.BaseStream.Position > DEFAULT_DEBUG_SIZE )
					base.BaseStream.Position = 0;

				_lastWriteTick = HiTimer.Tick;
			}
		}

		private static TextWriter _debugWtr;
		private static TextWriter _oldOut;
		private static TextWriter _errorWtr;
		private static TextWriter _oldErr;

		public static void Transfer(string appPath, string fileName,int day)
		{
			_oldOut = Console.Out;
			_oldErr = Console.Error;


			string newPath = Path.Combine(appPath, DateTime.Now.ToString("yyyy-MM-dd"));

			if (!Directory.Exists(newPath))
			{
				Directory.CreateDirectory(newPath);
			}

			string fullName = Path.Combine(newPath, fileName);
			fullName = Path.ChangeExtension(fullName, ".txt");
			_debugWtr = TextWriter.Synchronized(new DebugStreamWriter(fullName));

            //fullName = Path.ChangeExtension(fullName, "error");
            //_errorWtr = TextWriter.Synchronized(new DebugStreamWriter(fullName));

			Console.SetOut( _debugWtr );
			//Console.SetError( _errorWtr );

            DeleteFile(day, appPath);
		}

		public static void Restore()
		{
			if ( _debugWtr != null )
			{
				_debugWtr.Dispose();
				_debugWtr = null;
				Console.SetOut( _oldOut );
			}

			if ( _errorWtr != null )
			{
				_errorWtr.Dispose();
				_errorWtr = null;
				Console.SetError( _oldErr );
			}

		}

		private static void DeleteFile(int day, string dir)
		{
			DirectoryInfo dirinfo = new DirectoryInfo(dir);

			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}

			DirectoryInfo[] dirList = dirinfo.GetDirectories();

			foreach (DirectoryInfo item in dirList)
			{
				DateTime dateTime;

				if (!DateTime.TryParse(item.Name, out  dateTime))
					continue;

				//Calculate DateTime
				TimeSpan ts1 = new TimeSpan(dateTime.Ticks);
				TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
				TimeSpan ts = ts1.Subtract(ts2).Duration();

				if (ts.Days > 30)
				{
					DeleteDirectory(item.FullName);
				}
			}
		}

		private static void DeleteDirectory(string targetDir)
		{
			string[] files = Directory.GetFiles(targetDir);
			string[] dirs = Directory.GetDirectories(targetDir);
			foreach (string file in files)
			{
				File.SetAttributes(file, FileAttributes.Normal);
				File.Delete(file);
			}
			foreach (string dir in dirs)
			{
				DeleteDirectory(dir);
			}
			Directory.Delete(targetDir, false);

		}
	}
}
