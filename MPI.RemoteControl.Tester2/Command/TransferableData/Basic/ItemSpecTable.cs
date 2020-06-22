using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MPI.RemoteControl2.Tester.Mpi.Command.Base
{
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
}
