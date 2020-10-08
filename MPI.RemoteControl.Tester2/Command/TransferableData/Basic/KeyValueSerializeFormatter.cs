using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MPI.RemoteControl2.Tester.Mpi.Command.Base
{
	public class KeyValueSerializeFormatter : IFormatProvider, ICustomFormatter
	{
		#region >>> Private field <<<

		private System.Runtime.Serialization.Formatters.Binary.BinaryFormatter _formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

		#endregion

		#region >>> Public static method <<<

		/// <summary>
		/// Convert byte array to string.
		/// </summary>
		public static string ConvertToString( byte[] buffer )
		{
			string temp = "";

			for ( int nIndex = 0; nIndex < buffer.Length; ++nIndex )
			{
				temp += String.Format( "{0}", buffer[ nIndex ].ToString( "X2" ) );
			}

			return temp;
		}

		/// <summary>
		/// Convert string to byte array.
		/// </summary>
		public static byte[] ConvertToBytes( string buffer )
		{
			List<byte> temp = new List<byte>();

			buffer = buffer.PadRight( buffer.Length % 2 + buffer.Length, '\0' );

			int value = 0;
			for ( int nIndex = 0; nIndex < buffer.Length; nIndex += 2 )
			{
				if ( Int32.TryParse( buffer.Substring( nIndex, 2 ), System.Globalization.NumberStyles.HexNumber, null, out value ) )
				{
					temp.Add( ( byte ) value );
				}
			}

			return temp.ToArray();
		}

		#endregion

		#region >>> Public method <<<

		/// <summary>
		/// Converts the value of a specified object to an equivalent string representation using 
		/// specified format and culture-specific formatting information.
		/// </summary>
		public string Format(string format, object arg, IFormatProvider formatProvider)
		{
			if ( arg == null )
				return "";

			if (arg is double || arg is float)
			{
				return ConvertToString(BitConverter.GetBytes((dynamic)arg));
			}

			if (arg is IFormattable)
			{
				return ((IFormattable)arg).ToString(format, formatProvider);
			}
			else if (arg is string)
			{
				return (string)arg;
			}
            else if (arg is bool)
            {
                return arg.ToString();
            }
            else if (arg.GetType().IsSerializable)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    try
                    {
                        _formatter.Serialize(ms, arg);

                        return ConvertToString(ms.ToArray());
                    }
                    catch
                    {
                        return "";
                    }
                }
            }
            else
            {
                return arg.ToString();
            }
		}

		/// <summary>
		/// Returns an object that provides formatting services for the specified type.
		/// </summary>
		public object GetFormat(Type formatType)
		{
			return (formatType == typeof(ICustomFormatter)) ? this : null;
		}

		#endregion
	}
}
