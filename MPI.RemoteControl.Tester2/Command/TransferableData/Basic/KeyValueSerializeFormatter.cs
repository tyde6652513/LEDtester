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

		#region >>> Public method <<<

		public string Format(string format, object arg, IFormatProvider formatProvider)
		{
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

		public object GetFormat(Type formatType)
		{
			return (formatType == typeof(ICustomFormatter)) ? this : null;
		}

		#endregion

		#region >>> Public static method <<<

		public static string ConvertToString(byte[] buffer)
		{
			string temp = "";

			for (int nIndex = 0; nIndex < buffer.Length; ++nIndex)
			{
				temp += String.Format("{0}", buffer[nIndex].ToString("X2"));
			}

			return temp;
		}

		public static byte[] ConvertToBytes(string buffer)
		{
			List<byte> temp = new List<byte>();

			buffer = buffer.PadRight(buffer.Length % 2 + buffer.Length, '\0');

			int value = 0;
			for (int nIndex = 0; nIndex < buffer.Length; nIndex += 2)
			{
				if (Int32.TryParse(buffer.Substring(nIndex, 2), System.Globalization.NumberStyles.HexNumber, null, out value))
				{
					temp.Add((byte)value);
				}
			}

			return temp.ToArray();
		}

		#endregion
	}
}
