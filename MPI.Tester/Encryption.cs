using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester
{
	//=======================================//
	// LD200與MapAnalyzer務必同步改版
	// 否則會無法讀取報表
	// Version 1.1
	// Modify Date:20180627 
	//=======================================//
	public class Encryption
	{
		public static string GetCode( string[] strAry )
		{
			string str = string.Empty;

			foreach ( var item in strAry )
			{
				str += item;
			}

			return Encryption.Encoder( str );
		}

		public static bool CheckCode( string[] strAry, string code )
		{
			return Encryption.GetCode( strAry ) == code;
		}

		private static string Encoder( string str )
		{
			int code = 0;

			str = "MPI_" + str;

			foreach ( var c in str )
			{
				if ( code % 2 == 0 )
				{
					code += Convert.ToInt32( c );
				}
				else
				{
					code -= Convert.ToInt32( c );
				}
			}

			return code.ToString();
		}
	}
}
