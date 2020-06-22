using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MPI.RemoteControl2.Tester.Mpi.Command.Base
{
	[Serializable]
	public class KeyNamePair
	{
		public const string DEFAULT_FORMAT = "0.000";
		private static XmlDocument _xmlDocument = new XmlDocument();
		private static XmlSerializer _serializer = new XmlSerializer(typeof(KeyNamePair));

		[XmlIgnore]
		public string Key;
		[XmlIgnore]
		public string Value;
		[XmlIgnore]
		public string Format;

		[XmlElement("Key")]
		public XmlCDataSection XmlKeySection
		{
			get { return _xmlDocument.CreateCDataSection(Key); }
			set { Key = (value == null) ? String.Empty : value.Data; }
		}

		[XmlElement("Value")]
		public XmlCDataSection XmlValueSection
		{
			get { return _xmlDocument.CreateCDataSection(Value); }
			set { Value = (value == null) ? String.Empty : value.Data; }
		}

		[XmlElement("Format")]
		public XmlCDataSection XmlFormatSection
		{
			get { return _xmlDocument.CreateCDataSection(Format); }
			set { Format = (value == null) ? String.Empty : value.Data; }
		}

		public KeyNamePair()
		{
			Key = String.Empty;
			Value = String.Empty;
			Format = DEFAULT_FORMAT;
		}

		public KeyNamePair(string key, string value)
		{
			Key = key;
			Value = value;
			Format = DEFAULT_FORMAT;
		}

		public KeyNamePair(string key, string value, string format)
		{
			Key = key;
			Value = value;
			Format = format;
		}
	}
}
