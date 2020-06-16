using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MPI.RemoteControl.Tester
{
    [Serializable]
    public class EDataTestItemTable
    {
        private List<EDataTestItem> _table;

        public EDataTestItemTable()
        {
            _table = new List<EDataTestItem>();
        }

		[XmlElement("Spec")]
        public List<EDataTestItem> Table
		{
			get { return _table; }
			set { _table = value; }
		}
    }
}
