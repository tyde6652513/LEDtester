using System;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MPI.TRY
{
	/// <summary>
	/// XmlFileSerializer class.
	/// </summary>
	public class XmlFileSerializer
	{
		/// <summary>
		/// Color format.
		/// </summary>
		public enum ColorFormat
		{
			NamedColor,
			ARGBColor
		}

		/// <summary>
		/// Serialize.
		/// </summary>
		public static void Serialize( object data, string filename )
		{
			string directory = filename.Substring( 0, filename.IndexOf( Path.GetFileName( filename ) ) );

			// check directory
			if ( !Directory.Exists( directory ) )
				Directory.CreateDirectory( directory );

			// production data
			using ( StreamWriter sw = new StreamWriter( filename ) )
			{
				XmlSerializer xs = new XmlSerializer( data.GetType() );

				XmlTextWriter xw = new XmlTextWriter( sw );

				xs.Serialize( xw, data );

				xw.Close();
			}
		}

		/// <summary>
		/// Deserialize.
		/// </summary>
		public static object Deserialize( Type type, string filename )
		{
			// check file
			if ( !File.Exists( filename ) )
				return null;

			// production data
			object data = null;

			StreamReader sr = null;

			try
			{
				sr = new StreamReader( filename );

				XmlSerializer xs = new XmlSerializer( type );

				XmlTextReader xr = new XmlTextReader( sr );

				data = xs.Deserialize( xr );

				xr.Close();
			}
			catch ( Exception )
			{
				data = null;
			}
			finally
			{
				if ( sr != null )
					sr.Close();
			}

			return data;
		}

		/// <summary>
		/// Serialize color type.
		/// </summary>
		public static string SerializeColor( Color color )
		{
#if ! WindowsCE
			if ( color.IsNamedColor )
				return string.Format( "{0}:{1}",
					ColorFormat.NamedColor, color.Name );
			else
				return string.Format( "{0}:{1}:{2}:{3}:{4}",
					ColorFormat.ARGBColor,
					color.A, color.R, color.G, color.B );
#else
			return string.Format( "{0}:{1}:{2}:{3}:{4}", ColorFormat.ARGBColor, color.A, color.R, color.G, color.B );
#endif
		}

		/// <summary>
		/// Deserialize color type.
		/// </summary>
		public static Color DeserializeColor( string color )
		{
			byte a, r, g, b;

			string[] pieces = color.Split( new char[] { ':' } );

			ColorFormat colorType = ( ColorFormat )
				Enum.Parse( typeof( ColorFormat ), pieces[0], true );

#if ! WindowsCE
			switch ( colorType )
			{
				case ColorFormat.NamedColor:
					return Color.FromName( pieces[1] );

				case ColorFormat.ARGBColor:
					a = byte.Parse( pieces[1] );
					r = byte.Parse( pieces[2] );
					g = byte.Parse( pieces[3] );
					b = byte.Parse( pieces[4] );

					return Color.FromArgb( a, r, g, b );
			}
#else
			a = byte.Parse( pieces[ 1 ] );
			r = byte.Parse( pieces[ 2 ] );
			g = byte.Parse( pieces[ 3 ] );
			b = byte.Parse( pieces[ 4 ] );

			return Color.FromArgb( r, g, b );
#endif

			return Color.Empty;
		}
	}
}
