using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MPI.RemoteControl2.Tester.Mpi.Command.Base
{
	public class KeyValueSerializeTable : Dictionary<string, KeyValueSerializePair>, IXmlSerializable
	{
		#region >>> Public method <<<

		// Summary:
		//     This method is reserved and should not be used. When implementing the IXmlSerializable
		//     interface, you should return null (Nothing in Visual Basic) from this method,
		//     and instead, if specifying a custom schema is required, apply the System.Xml.Serialization.XmlSchemaProviderAttribute
		//     to the class.
		//
		// Returns:
		//     An System.Xml.Schema.XmlSchema that describes the XML representation of the
		//     object that is produced by the System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)
		//     method and consumed by the System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)
		//     method.
		public XmlSchema GetSchema() { return null; }

		//
		// Summary:
		//     Generates an object from its XML representation.
		//
		// Parameters:
		//   reader:
		//     The System.Xml.XmlReader stream from which the object is deserialized.
		public void ReadXml(XmlReader reader)
		{
			if (reader.IsEmptyElement) return;

			reader.Read();

			while (reader.NodeType != XmlNodeType.EndElement)
			{
				KeyValueSerializePair pair = new KeyValueSerializePair();

				pair.Key = reader.GetAttribute("K");
				pair.ValueType = reader.GetAttribute("T");
				pair.FormattedValue = reader.GetAttribute("V");

				this.Add(pair.Key, pair);

				reader.Read();
			}
		}

		//
		// Summary:
		//     Converts an object into its XML representation.
		//
		// Parameters:
		//   writer:
		//     The System.Xml.XmlWriter stream to which the object is serialized.
		public void WriteXml(XmlWriter writer)
		{
			using (KeyCollection.Enumerator enumerator = this.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					writer.WriteStartElement("P");

					KeyValueSerializePair pair = this[enumerator.Current];

					writer.WriteAttributeString("K", pair.Key);
					writer.WriteAttributeString("T", pair.ValueType);
					writer.WriteAttributeString("V", pair.FormattedValue);

					writer.WriteEndElement();
				}
			}
		}

		public void CopyFrom(KeyValueSerializeTable other)
		{
			using (Dictionary<string, KeyValueSerializePair>.Enumerator enumerator = other.GetEnumerator())
			{
				KeyValuePair<string, KeyValueSerializePair> current;

				while (enumerator.MoveNext())
				{
					current = enumerator.Current;

					if (this.ContainsKey(current.Key))
					{
						this[current.Key] = current.Value;
					}
					else
					{
						this.Add(current.Key, current.Value);
					}
				}
			}
		}

		#endregion
	}
}
