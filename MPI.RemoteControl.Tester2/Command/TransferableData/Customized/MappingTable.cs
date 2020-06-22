using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;

using Newtonsoft.Json;

namespace MPI.RemoteControl2.Tester.Mpi.Command.Base
{
	public class MappingTable : TransferableCommonObjectBase
	{
		#region >>> Private field <<<
		private KeyValueSerializeTable _table;
		#endregion

		public MappingTable()
			: base()
		{
			_table = new KeyValueSerializeTable();
		}

		#region >>> Public property <<<

		[XmlElement("Tbl")]
		[JsonProperty("Tbl")]
		public KeyValueSerializeTable Table
		{
			get { return _table; }
			set { _table = value; }
		}

		/// <summary>
		/// Get the number of added locations.
		/// </summary>
		[XmlIgnore]		
		[JsonIgnore]
		public int Count
		{
			get
			{
				lock (_table)
				{
					KeyValueSerializePair count = null;
					
					if (!_table.TryGetValue("C", out count)) return 0;
					
					return (count.Value is int) ? count.Value : 0;
				}
			}

			private set
			{
				lock (_table)
				{
					if (!_table.ContainsKey("C"))
					{
						_table.Add("C", new KeyValueSerializePair("C", value));
					}
					else
					{
						_table["C"].Value = value;
					}
				}
			}
		}

		/// <summary>
		/// Get the number of all layers.
		/// </summary>
		[XmlIgnore]
		[JsonIgnore]
		public int Layers
		{
			get
			{
				lock (_table)
				{
					KeyValueSerializePair layers = null;

					if (!_table.TryGetValue("L", out layers)) return 0;

					return (layers.Value is int) ? layers.Value : 0;
				}
			}

			private set
			{
				lock (_table)
				{
					if (!_table.ContainsKey("L"))
					{
						_table.Add("L", new KeyValueSerializePair("L", value));
					}
					else
					{
						_table["L"].Value = value;
					}
				}
			}
		}

		/// <summary>
		/// Get the location with specified index.
		/// </summary>
		[XmlIgnore]
		[JsonIgnore]
		public Point this[int nIndex]
		{
			get
			{
				Point pt;

				if (!this.Get(nIndex, out pt))
					throw new IndexOutOfRangeException(String.Format("Index:{0} is out of range.", nIndex));

				return pt;
			}
		}

		#endregion

		#region >>> Public method <<<

		/// <summary>
		/// Add a new location with column and row. (<paramref name="pt"/>.X is column, <paramref name="pt"/>.Y is row.)
		/// </summary>
		/// <returns>The added index in the table.</returns>
		public int Add(Point pt)
		{
			return this.Add(pt.X, pt.Y);
		}

		/// <summary>
		/// Add a new location with column and row.
		/// </summary>
		/// <returns>The added index in the table.</returns>
		public int Add(int col, int row)
		{
			lock (_table)
			{
				int nIndex = this.Count;

				string key = FormatIndexKey(nIndex);

				_table.Add(key, new KeyValueSerializePair(key, String.Format("{0},{1}", col, row)));

				this.Count = nIndex + 1;

				return nIndex;
			}
		}

		/// <summary>
		/// Get the location with specified index.
		/// </summary>
		public bool Get(int nIndex, out Point pt)
		{
			lock (_table)
			{
				pt = Point.Empty;
				string key = FormatIndexKey(nIndex);

				if (!_table.ContainsKey(key))
					return false;

				if (!TryParseRCValue(_table[key].Value as string, out pt))
					return false;

				return true;
			}
		}

		/// <summary>
		/// Get the location with specified index.
		/// </summary>
		public bool Get(int nIndex, out int column, out int row)
		{
			Point pt = Point.Empty;

			column = pt.X;
			row = pt.Y;

			if (!this.Get(nIndex, out pt)) return false;

			column = pt.X;
			row = pt.Y;

			return true;
		}

		/// <summary>
		/// Clear all existed locations.
		/// </summary>
		public void Clear()
		{
			lock (_table)
				_table.Clear();
		}

		#endregion

		#region >>> Protected property <<<

		protected override int ObjectIdentifiedID
		{
			get
			{
				return (int)ETransferableCommonObject.MappingTable;
			}
		}

		#endregion

		#region >>> Protected method <<<

		protected override string SerializeCommonObject()
		{
			return JsonConvert.SerializeObject(this);
		}

		protected override bool DeserializeCommonObject(string context)
		{
			try
			{
				MappingTable tbl = JsonConvert.DeserializeObject(context, typeof(MappingTable)) as MappingTable;

				if (tbl == null) return false;

				this._table = new KeyValueSerializeTable();
				this._table = tbl._table;

				return true;
			}
			catch
			{
				return false;
			}
		}

		#endregion

		#region >>> Private static method <<<

		private static string FormatIndexKey(int nIndex)
		{
			return String.Format("#{0}", nIndex);
		}

		private static string FormatRCValue(Point pt)
		{
			return FormatRCValue(pt.X, pt.Y);
		}

		private static string FormatRCValue(int col, int row)
		{
			return String.Format("{0},{1}", col, row);
		}

		private static bool TryParseRCValue(string value, out Point pt)
		{
			pt = Point.Empty;

			if (value == null) return false;

			string[] items = value.Split(",".ToCharArray());
			if (items.Length < 2) return false;

			int column = 0;
			if (!Int32.TryParse(items[0], out column)) return false;

			int row = 0;
			if (!Int32.TryParse(items[1], out row)) return false;

			pt = new Point(column, row);

			return true;
		}
		
		#endregion
	}
}
