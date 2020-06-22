using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
namespace MPI.RemoteControl2.Tester.Mpi.Command.Base
{
	public class KeyNameTable : IXmlSerializable, System.Collections.IEnumerator
	{
		private const string NODE_PAIR = "KeyNamePair";
		private const string NODE_KEY_NAME = "KeyName";
		private const string NODE_NAME = "Name";
		private const string NODE_FORMAT = "Format";

		private object _lock;
		private List<string> _keys;
		private List<string> _values;
		private List<string> _format;
		private int _indexor;
		private int _pairCount;

		public KeyNameTable()
		{
			_lock = new object();
			Initialize();
		}

		public bool Add(string Key, string Value)
		{
			return Add(Key, Value, KeyNamePair.DEFAULT_FORMAT);
		}

		public bool Add(string Key, string Value, string Format)
		{
			lock (_lock)
			{
				if (_keys == null || _values == null || _format == null)
				{
					Initialize();
				}
				else
				{
					if (_keys.Contains(Key) || String.IsNullOrEmpty(Key)) return false;
				}

				_keys.Add(Key);

				_values.Add(Value);

				_format.Add(Format);

				_pairCount++;

				return true;
			}
		}

		public KeyNamePair CurrentPair
		{
			get
			{
				KeyNamePair pair = Current as KeyNamePair;
				return pair;
			}
		}

		public int Count
		{
			get
			{
				lock (_lock)
					return _pairCount;
			}
		}

		public object Current
		{
			get
			{
				lock (_lock)
				{
					if (_indexor >= 0 && _indexor < _keys.Count && _indexor < _values.Count && _indexor < _format.Count)
						return new KeyNamePair(_keys[_indexor], _values[_indexor], _format[_indexor]);
					else
						return null;
				}
			}
		}

		/// <summary>
		/// Reset indexor
		/// </summary>
		public void Reset()
		{
			lock (_lock)
				_indexor = -1;
		}

		public bool MoveNext()
		{
			lock (_lock)
			{
				int nNextIndex = _indexor + 1;

				if (nNextIndex >= _keys.Count || nNextIndex >= _values.Count || nNextIndex >= _format.Count) return false;

				_indexor++;

				return true;
			}
		}

		public void Dispose()
		{
			lock (_lock)
				Initialize();
		}

		private void Initialize()
		{
			_keys = new List<string>();
			_values = new List<string>();
			_format = new List<string>();
			_indexor = -1;
			_pairCount = 0;
		}

		public System.Xml.Schema.XmlSchema GetSchema() { return null; }

		public void ReadXml(XmlReader reader)
		{
			if (reader.IsEmptyElement) return;

			XmlDocument doc = new XmlDocument();
			System.Data.DataSet ds = new System.Data.DataSet();
			ds.ReadXml(reader);
			doc.LoadXml(ds.GetXml());

			XmlNodeList list = doc.GetElementsByTagName(NODE_PAIR);
			foreach (XmlNode node in list)
			{
				string KeyName = node[NODE_KEY_NAME].InnerText;
				string Name = node[NODE_NAME].InnerText;
				string Format = node[NODE_FORMAT].InnerText;

				if (String.IsNullOrEmpty(Format)) Format = KeyNamePair.DEFAULT_FORMAT;

				Add(KeyName, Name, Format);
			}
		}

		public void WriteXml(XmlWriter writer)
		{
			lock (_lock)
			{
				List<string>.Enumerator KeyEnumerator = _keys.GetEnumerator();
				List<string>.Enumerator ValueEnumerator = _values.GetEnumerator();
				List<string>.Enumerator FormatEnumerator = _format.GetEnumerator();

				while (KeyEnumerator.MoveNext() && ValueEnumerator.MoveNext() && FormatEnumerator.MoveNext())
				{
					// Write node: KeyNamePair - 1
					writer.WriteStartElement(NODE_PAIR);

					// Write node: KeyName - 2
					writer.WriteStartElement(NODE_KEY_NAME);
					writer.WriteCData(KeyEnumerator.Current);
					// Write node: KeyName - 2
					writer.WriteEndElement();

					// Write node: Name - 3
					writer.WriteStartElement(NODE_NAME);
					writer.WriteCData(ValueEnumerator.Current);
					// Write node: Name - 3
					writer.WriteEndElement();

					// Write node: Format - 4
					writer.WriteStartElement(NODE_FORMAT);
					writer.WriteCData(FormatEnumerator.Current);
					// Write node: Format - 4
					writer.WriteEndElement();

					// Write node: KeyNamePair - 1
					writer.WriteEndElement();
				}

				writer.Flush();
			}
		}
	}
}
