using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using System.Xml;

namespace MPI
{
	public class Kit
	{
		public static int ParseInt( string value, int defVal )
		{
			int output = ( int.TryParse( value, out output ) ) ? output : defVal;

			return output;
		}

		public static int ParseInt( string value )
		{
			return ParseInt( value, 0 );
		}

		public static bool ParseBoolean( string value )
		{
			return "true".Equals( value, StringComparison.OrdinalIgnoreCase );
		}

		/// <summary>
		/// Parse Float with string value
		/// </summary>
		/// <param name="value">string value</param>
		/// <returns>return default value if fail</returns>
		public static float ParseFloat( string value, float defVal )
		{
			float output = ( float.TryParse( value, out output ) ) ? output : defVal;

			return output;
		}

		/// <summary>
		/// Parse Float with string value
		/// </summary>
		/// <param name="value">string value</param>
		/// <returns>return float.NaN if fail</returns>
		public static float ParseFloat( string value )
		{
			return ParseFloat( value, float.NaN );
		}

		public static double ParseDouble( string value, double defVal )
		{
			double output = ( double.TryParse( value, out output ) ) ? output : defVal;

			return output;
		}

		public static double ParseDouble( string value )
		{
			return ParseDouble( value, double.NaN );
		}

		public static Color ParseColor( string value, Color defVal )
		{
			Color output = Color.FromName( value );
			if ( output.IsKnownColor )
				return output;

			long argb;
			if ( long.TryParse( value, out argb ) )
				return Color.FromArgb( ( int ) argb );

			return defVal;
		}

		public static Color ParseColor( string value )
		{
			return ParseColor( value, Color.Empty );
		}

		public static bool IsMatchIPAddress( IPAddress ip )
		{
			NetworkInterface[] ifs = NetworkInterface.GetAllNetworkInterfaces();
			if ( ifs == null || ifs.Length == 0 )
				return false;

			foreach ( NetworkInterface ni in ifs )
			{
				IPInterfaceProperties ifp = ni.GetIPProperties();
				UnicastIPAddressInformationCollection ct = ifp.UnicastAddresses;

				if ( ct == null || ct.Count == 0 )
					continue;

				if ( ct[0].Address.AddressFamily != AddressFamily.InterNetwork )
					continue;

				if ( ct[0].Address.Equals( ip ) )
					return true;
			}

			return false;
		}

		public static object ParseEnum( string value, Enum defValue )
		{
			if ( string.IsNullOrEmpty( value ) )
				return defValue;

			Type etype = defValue.GetType();
			string[] names = Enum.GetNames( etype );

			for ( int i = names.Length - 1; i >= 0; i-- )
			{
				if ( names[i].Equals( value, StringComparison.OrdinalIgnoreCase ) )
					return Enum.GetValues( etype ).GetValue( i );
			}

			return defValue;
		}

		public static T ParseEnum<T>( string value, T defValue )
		{
			try
			{
				return ( T ) Enum.Parse( typeof( T ), value, true );
			}
			catch
			{
				return defValue;
			}
		}

		public static Enum ParseEnum( Type enumType, string value )
		{
			if ( enumType == null )
				return null;

			try
			{
				return ( Enum ) Enum.Parse( enumType, value, true );
			}
			catch
			{
				return null;
			}
		}

		public static Enum ParseEnum( string enumTypeString, string value )
		{
			return ParseEnum( Type.GetType( enumTypeString, false ), value );
		}

		public static void CopyStructToByte<T>( ref T value, byte[] data )
		{
			GCHandle gch = GCHandle.Alloc( data, GCHandleType.Pinned );
			Marshal.StructureToPtr( value, gch.AddrOfPinnedObject(), true );
			gch.Free();
		}

		//public static void StructToIntPtr<T>( ref T value, byte[] temp, out IntPtr destPtr )
		//{
		//   GCHandle gch = GCHandle.Alloc( temp, GCHandleType.Pinned );
		//   destPtr = gch.AddrOfPinnedObject();
		//   Marshal.StructureToPtr( value, destPtr, true );
		//   gch.Free();
		//}

		public static byte[] StructToByte<T>( ref T value )
		{
			byte[] data = new byte[Marshal.SizeOf( value )];
			GCHandle gch = GCHandle.Alloc( data, GCHandleType.Pinned );
			Marshal.StructureToPtr( value, gch.AddrOfPinnedObject(), true );
			gch.Free();
			return data;
		}

		public static void ByteToStruct<T>( out T value, byte[] data )
		{
			GCHandle gch = GCHandle.Alloc( data, GCHandleType.Pinned );
			value = ( T ) Marshal.PtrToStructure( gch.AddrOfPinnedObject(), typeof( T ) );
			gch.Free();
		}

		public static T ByteToStruct<T>( byte[] data )
		{
			T value;
			GCHandle gch = GCHandle.Alloc( data, GCHandleType.Pinned );
			value = ( T ) Marshal.PtrToStructure( gch.AddrOfPinnedObject(), typeof( T ) );
			gch.Free();
			return value;
		}

		public class IOHandler
		{
			public static Stream OpenFileStream( string filePath )
			{
				return OpenFileStream( filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read );
			}

			public static Stream OpenFileStream( string filePath, FileMode mode )
			{
				return OpenFileStream( filePath, mode, FileAccess.ReadWrite, FileShare.Read );
			}

			public static Stream OpenFileStream( string filePath, FileMode mode, FileAccess access )
			{
				return OpenFileStream( filePath, mode, access, FileShare.Read );
			}

			public static Stream OpenFileStream( string filePath, FileMode mode, FileAccess access, FileShare share )
			{
				FileStream file_stream = null;
				try
				{
					file_stream = new FileStream( filePath, mode, access, share );
				}
				catch ( Exception ex )
				{
					Console.Error.WriteLine( "[Zipper::OpenFileStream] " + ex.Message );
				}

				return file_stream;
			}

			public static bool CopyStream( Stream srcStream, Stream dstStream )
			{
				return CopyStream( srcStream, dstStream, 4096 );
			}

			public static bool CopyStream( Stream srcStream, Stream dstStream, int bufferSize )
			{
				int read = 0;
				byte[] buffer = new byte[bufferSize];

				try
				{
					while ( ( read = srcStream.Read( buffer, 0, bufferSize ) ) > 0 )
						dstStream.Write( buffer, 0, read );
				}
				catch ( Exception ex )
				{
					Console.Error.WriteLine( "[Zipper::CopyStream] " + ex.Message );
					return false;
				}

				return true;
			}

			/// <summary>
			/// Make Folder
			/// </summary>
			/// <param name="folder"></param>
			/// <param name="forceEmpty">true for clear existed items.</param>
			/// <returns></returns>
			public static bool MakeFolder( string folder, bool forceEmpty )
			{
				if ( Directory.Exists( folder ) )
				{
					if ( forceEmpty == false )
						return true;

					try
					{
						Directory.Delete( folder, true );
					}
					catch ( Exception ex )
					{
						Console.Error.WriteLine( "[Serializer::MakeFolder] " + ex.Message );
						return false;
					}
				}

				try
				{

					Directory.CreateDirectory( folder );
				}
				catch ( Exception ex )
				{
					Console.Error.WriteLine( "[Serializer::MakeFolder] " + ex.Message );
					return false;
				}

				return true;
			}

			public static bool RemoveFolder( string folder, bool deep )
			{
				if ( Directory.Exists( folder ) )
				{
					try
					{
						Directory.Delete( folder, deep );
					}
					catch ( Exception ex )
					{
						Console.Error.WriteLine( "[Serializer::RemoveFolder] " + ex.Message );
						return false;
					}
				}

				return true;
			}

		}

		public class Serializer
		{
			public static byte[] Pack( object data )
			{
				byte[] temp = null;

				MemoryStream ms = new MemoryStream();
				{
					BinaryFormatter serializer = new BinaryFormatter();
					try
					{
						serializer.Serialize( ms, data );
						temp = ms.ToArray();
					}
					catch ( Exception e )
					{
						Console.Error.WriteLine( "[MPI::Serializer::Pack] " + e.Message );
					}

					ms.Close();
					ms.Dispose();
					serializer = null;
				}

				return temp;
			}

			public static bool Pack( object data, string filePath )
			{
				if ( data == null )
					return false;

				if ( Kit.IOHandler.MakeFolder( Path.GetDirectoryName( filePath ), false ) == false )
					return false;

				FileStream fs = null;
				{
					BinaryFormatter serializer = new BinaryFormatter();
					try
					{
						fs = new FileStream( filePath, FileMode.Create );
						serializer.Serialize( fs, data );
					}
					catch ( Exception e )
					{
						Console.Error.WriteLine( "[MPI::Serializer::Pack] " + e.Message );
						return false;
					}
					finally
					{
						if ( fs != null )
						{
							fs.Close();
							fs.Dispose();
						}
					}
					serializer = null;
				}

				return true;
			}

			public static object Unpack( Stream ms )
			{
				object newobj = null;
				BinaryFormatter deserializer = new BinaryFormatter();

				try
				{
					newobj = deserializer.Deserialize( ms );
				}
				catch ( Exception e )
				{
					Console.Error.WriteLine( "[MPI::Serializer::Unpack] " + e.Message );
				}

				deserializer = null;
				return newobj;
			}

			public static object Unpack( byte[] raw )
			{
				MemoryStream ms = new MemoryStream( raw );

				object obj = Serializer.Unpack( ms );

				ms.Close();
				ms.Dispose();
				ms = null;

				return obj;
			}

			public static object Unpack( string filePath )
			{
				Stream ms = IOHandler.OpenFileStream( filePath, FileMode.Open, FileAccess.Read );
				if ( ms == null )
					return null;

				object obj = Serializer.Unpack( ms );

				ms.Close();
				ms.Dispose();
				ms = null;
				return obj;
			}
		}
	}
}
