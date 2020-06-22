using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using Newtonsoft.Json;

namespace MPI.RemoteControl2.Tester.Mpi.Command.Base
{
	[Serializable]
	public class KeyValueSerializePair
	{
		#region >>> Private static field <<<

		private static KeyValueSerializeFormatter s_formatter = new KeyValueSerializeFormatter();
		private static Tuple<Dictionary<Type, string>, Dictionary<string, Type>> s_tbl = CreateLookupTable();

		#endregion

		#region >>> Private field <<<

		private string _key;
		private string _type;
		private dynamic _value;

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		public KeyValueSerializePair()
			: this( null, null )
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public KeyValueSerializePair( string key, dynamic value )
		{
			this.Key = key;
			this.Value = value;
		}

		#region >>> Public property <<<

		/// <summary>
		/// The value of this pair.
		/// </summary>
		[XmlIgnore]
		[JsonIgnore]
		public dynamic Value
		{
			get { return _value; }
			set
			{
				_value = value;

				// _type = (_value == null) ? "" : FormatType(_value.GetType());
				_type = ( _value == null ) ? "" : FormatType( _value );
			}
		}

		/// <summary>
		/// The key of this pair.
		/// </summary>
		[XmlElement( "K" )]
		[JsonProperty( "K" )]
		public string Key
		{
			get
			{
				return this._key;
			}
			set
			{
				this._key = ( value == null ) ? "" : value;
			}
		}

		/// <summary>
		/// The formatted type name for the pair value.
		/// </summary>
		[XmlElement( "T" )]
		[JsonProperty( "T" )]
		public string ValueType
		{
			get
			{
				// return FormatType(this.Value.GetType());
				return FormatType( this.Value );
			}
			set
			{
				_type = ( value == null ) ? "" : value;
			}
		}

		/// <summary>
		/// The formatted value.
		/// </summary>
		[XmlElement( "V" )]
		[JsonProperty( "V" )]
		public string FormattedValue
		{
			get
			{
				return String.Format( s_formatter, "{0}", ( object ) this.Value );
			}
			set
			{
				if ( String.IsNullOrEmpty( _type ) )
					return;

				string typeName = ParseTypeName( _type );

				switch ( typeName )
				{
					case "System.Boolean":
						{
							bool bVal = false;
							if ( Boolean.TryParse( value, out bVal ) )
								this.Value = bVal;
							break;
						}
					case "System.Byte":
						{
							byte val = 0;
							if ( byte.TryParse( value, out val ) )
								this.Value = val;
							break;
						}
					case "System.Int32":
						{
							int nVal = 0;
							if ( Int32.TryParse( value, out nVal ) )
								this.Value = nVal;
							break;
						}
					case "System.Int16":
						{
							Int16 nVal = 0;
							if ( Int16.TryParse( value, out nVal ) )
								this.Value = nVal;
							break;
						}
					case "System.Int64":
						{
							Int64 nVal = 0;
							if ( Int64.TryParse( value, out nVal ) )
								this.Value = nVal;
							break;
						}
					case "System.Single":
						{
							this.Value = BitConverter.ToSingle( KeyValueSerializeFormatter.ConvertToBytes( value ), 0 );
							break;
						}
					case "System.Double":
						{
							this.Value = BitConverter.ToDouble( KeyValueSerializeFormatter.ConvertToBytes( value ), 0 );
							break;
						}
					case "System.String":
						{
							this.Value = ( value == null ) ? "" : value;
							break;
						}
					default:
						{
							System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

							using ( MemoryStream ms = new MemoryStream( KeyValueSerializeFormatter.ConvertToBytes( value ) ) )
							{
								try
								{
									this.Value = bf.Deserialize( ms );
								}
								catch
								{
								}
							}
							break;
						}
				}
			}
		}

		#endregion

		#region >>> Private static method <<<

		private static Tuple<Dictionary<Type, string>, Dictionary<string, Type>> CreateLookupTable()
		{
			Dictionary<Type, string> tblType = new Dictionary<Type, string>();

			tblType.Add( typeof( bool ), "B" );
			tblType.Add( typeof( byte ), "b" );
			tblType.Add( typeof( Int32 ), "i3" );
			tblType.Add( typeof( Int16 ), "i1" );
			tblType.Add( typeof( Int64 ), "i6" );
			tblType.Add( typeof( float ), "f" );
			tblType.Add( typeof( double ), "d" );
			tblType.Add( typeof( string ), "s" );

			Dictionary<string, Type> tblKey = new Dictionary<string, Type>();

			tblKey.Add( "B", typeof( bool ) );
			tblKey.Add( "b", typeof( byte ) );
			tblKey.Add( "i3", typeof( Int32 ) );
			tblKey.Add( "i1", typeof( Int16 ) );
			tblKey.Add( "i6", typeof( Int64 ) );
			tblKey.Add( "f", typeof( float ) );
			tblKey.Add( "d", typeof( double ) );
			tblKey.Add( "s", typeof( string ) );

			return new Tuple<Dictionary<Type, string>, Dictionary<string, Type>>( tblType, tblKey );
		}

		private static string FormatType( object o )  // Type type)
		{
			if ( o == null )
				return "";

			Type type = o.GetType();

			if ( type == null )
				return "";

			return s_tbl.Item1.ContainsKey( type ) ? s_tbl.Item1[ type ] : "";
		}

		private static string ParseTypeName( string key )
		{
			if ( key == null )
				return null;

			return s_tbl.Item2.ContainsKey( key ) ? s_tbl.Item2[ key ].ToString() : key;
		}

		#endregion
	}
}
