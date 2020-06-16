using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace MPI.RemoteControl.Tester
{
	[Serializable]
	public class TransferableDataObject
	{
		private static string DEFAULT_END_STR = "]]>";
		private static string DEFAULT_END_REPLACED_STR = "}}>";
		private XmlDocument _doc = new XmlDocument();
		private static XmlSerializer _serializer = new XmlSerializer(typeof(TransferableDataObject));

		[XmlAttribute]
		public int IdentifiedID;

		[XmlIgnore]
		public string DataContext;

		[XmlElement]
		public XmlCDataSection Data
		{
			get
			{
				string tmpDataContext = DataContext.Replace(DEFAULT_END_STR, DEFAULT_END_REPLACED_STR);
				XmlCDataSection section = _doc.CreateCDataSection(tmpDataContext);
				return section;
			}
			set
			{
				if (value != null && !String.IsNullOrEmpty(value.Data))
				{
					DataContext = value.Data.Replace(DEFAULT_END_REPLACED_STR, DEFAULT_END_STR);
				}
				else
				{
					DataContext = String.Empty;
				}
			}
		}

		public string Serialize()
		{
			using (StringWriter sw = new StringWriter())
			{
				_serializer.Serialize(sw, this);
				return sw.ToString();
			}
		}

		public bool Deserialize(string context)
		{
			using (StringReader sr = new StringReader(context))
			{
				TransferableDataObject obj;

				try
				{
					obj = _serializer.Deserialize(sr) as TransferableDataObject;
				}
				catch
				{
					return false;
				}

				if (obj != null)
				{
					this.IdentifiedID = obj.IdentifiedID;
					this.DataContext = obj.DataContext;
					return true;
				}
				else
				{
					return false;
				}
			}
		}
	}

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
            bool isEmpty = reader.IsEmptyElement;
            if (isEmpty) return;

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

	[Serializable]
	public class ItemSpecTable
	{
		private List<ItemSpec> _table;

		[XmlElement("Spec")]
		public List<ItemSpec> Table
		{
			get { return _table; }
			set { _table = value; }
		}

		public ItemSpecTable()
		{
			_table = new List<ItemSpec>();
		}
	}

	[Serializable]
	public class ItemSpec
	{
		private static XmlDocument _xmlDocument = new XmlDocument();
		private string m_ItemName;
		private string m_ItemUnit;
		private string m_ItemValue;

		[XmlIgnore]
		public string Name
		{
			get { return m_ItemName; }
			set { m_ItemName = value; }
		}

		[XmlIgnore]
		public string Unit
		{
			get { return m_ItemUnit; }
			set { m_ItemUnit = value; }
		}

		[XmlIgnore]
		public string Value
		{
			get { return m_ItemValue; }
			set { m_ItemValue = value; }
		}

		[XmlElement("Name")]
		public XmlCDataSection XmlItemNameSection
		{
			get { return _xmlDocument.CreateCDataSection(m_ItemName); }
			set { m_ItemName = (value == null) ? String.Empty : value.Data; }
		}

		[XmlElement("Unit")]
		public XmlCDataSection XmlItemUnitSection
		{
			get { return _xmlDocument.CreateCDataSection(m_ItemUnit); }
			set { m_ItemUnit = (value == null) ? String.Empty : value.Data; }
		}

		[XmlElement("Value")]
		public XmlCDataSection XmlItemValueSection
		{
			get { return _xmlDocument.CreateCDataSection(m_ItemValue); }
			set { m_ItemValue = (value == null) ? String.Empty : value.Data; }
		}

		public ItemSpec()
		{
			m_ItemName = String.Empty;
			m_ItemUnit = String.Empty;
			m_ItemValue = String.Empty;
		}

		public ItemSpec(string name, string unit, string val)
		{
			m_ItemName = name;
			m_ItemUnit = unit;
			m_ItemValue = val;
		}
	}

    [Serializable]
    public class StageItemTable
    {
        private List<StageItem> _table;

        [XmlElement("ItemTable")]
        public List<StageItem> Table
        {
            get { return _table; }
            set { _table = value; }
        }

        public StageItemTable()
        {
            _table = new List<StageItem>();
        }
    }

    [Serializable]
    public class StageItem
    {
        private static XmlDocument _xmlDocument = new XmlDocument();
        
        [XmlIgnore]
        private int m_StageID;
        [XmlIgnore]
        private int m_ChipID;
        [XmlIgnore]
        private bool m_Activated;

        [XmlIgnore]
        private int m_TestItemCount;
        [XmlIgnore]
        private List<string> m_TestItemList;

        [XmlElement]
        public int StageID
        {
            get { return m_StageID; }
            set { m_StageID = value; }
        }

        [XmlElement]
        public int ChipID
        {
            get { return m_ChipID; }
            set { m_ChipID = value; }
        }

        [XmlElement]
        public bool Activated
        {
            get { return m_Activated; }
            set { m_Activated = value; }
        }

        [XmlElement]
        public int TestItemCount
        {
            get { return m_TestItemCount; }
            set { m_TestItemCount = value; }
        }

        [XmlElement]
        public List<string> TestItemList
        {
            get { return m_TestItemList; }
            set { m_TestItemList = value; }
        }

        public StageItem()
        {
            m_StageID = 1;
            m_ChipID = -1;
            m_Activated = false;

            m_TestItemCount = 0;
            m_TestItemList = new List<string>();
        }

        public StageItem(int stageID, int chipID, bool activated, int testItemCount, List<string> testItemList)
        {
            m_StageID = stageID;
            m_ChipID = chipID;
            m_Activated = activated;

            m_TestItemCount = testItemCount;
            m_TestItemList = testItemList;
        }
    }


    [Serializable]
    public class RefPoint
    {
        private static XmlDocument _xmlDocument = new XmlDocument();

        [XmlIgnore]
        private int _baseX;
        [XmlIgnore]
        private int _baseY;
        [XmlIgnore]
        private int _newX;
        [XmlIgnore]
        private int _newY;
        [XmlIgnore]
        private string _chipName;
        [XmlIgnore]
        private string _remark;

        [XmlElement]
        public int BaseX
        {
            get { return _baseX; }
            set { _baseX = value; }
        }
        [XmlElement]
        public int BaseY
        {
            get { return _baseY; }
            set { _baseY = value; }
        }
        [XmlElement]
        public int NewX
        {
            get { return _newX; }
            set { _newX = value; }
        }
        [XmlElement]
        public int NewY
        {
            get { return _newY; }
            set { _newY = value; }
        }
        [XmlElement]
        public string ChipName
        {
            get { return _chipName; }
            set { _chipName = value; }
        }
        [XmlElement]
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

        public RefPoint()
        {
            _baseX = 0;
            _baseY = 0;
            _newX = 0;
            _newY = 0;
            _chipName = "";
            _remark = "";
        }

        public RefPoint(int bx, int by, int nx,int ny,string chipName)
            : this()
        {
            _baseX = bx;
            _baseY = by;
            _newX = nx;
            _newY = ny;
            _chipName = chipName;
        }
    }

    [Serializable]
    public class RefPointsTable
    {
        private List<RefPoint> _table;

        [XmlElement("RefPointTable")]
        public List<RefPoint> Table
        {
            get { return _table; }
            set { _table = value; }
        }

        public RefPointsTable()
        {
            _table = new List<RefPoint>();
        }
    }

    
}
