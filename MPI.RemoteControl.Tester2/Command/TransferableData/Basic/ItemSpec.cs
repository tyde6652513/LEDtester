using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MPI.RemoteControl2.Tester.Mpi.Command.Base
{
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
}
