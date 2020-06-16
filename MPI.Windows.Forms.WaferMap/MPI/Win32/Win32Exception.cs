using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.Win32
{
	/// <summary>
	/// Win32 exception.
	/// </summary>
	public class Win32Exception : System.Exception
	{
		public Win32Exception()
		{

		}

		public Win32Exception( string message )
			: base( message )
		{

		}
	}

}
