using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Newtonsoft.Json;

namespace MPI.RemoteControl2.Tester.Mpi.Command.Base
{
	public class TestingProperties : TransferableCommonObjectBase
	{
		#region >>> Private field <<<

		private object _lock;
		private KeyValueSerializeTable _table;

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		public TestingProperties()
			: base()
		{
			_lock = new object();

			this.Initialize();
		}

		#region >>> Public property <<<

		/// <summary>
		/// Properties count.
		/// </summary>
		[XmlIgnore]
		[JsonIgnore]
		public int Count
		{
			get
			{
				lock (_lock)
				{
					return (_table != null) ? _table.Count : 0;
				}
			}
		}

		/// <summary>
		/// The testing properties.
		/// </summary>
		[XmlElement("Tbl")]
		[JsonProperty("Tbl")]
		public KeyValueSerializeTable Properties
		{
			get { return _table; }
			set { _table = value; }
		}

		#endregion

		#region >>> Protected property <<<

		protected override int ObjectIdentifiedID
		{
			get
			{
				return (int)ETransferableCommonObject.TestingProperties;
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
				TestingProperties data = JsonConvert.DeserializeObject(context, typeof(TestingProperties)) as TestingProperties;

				if (data == null) return false;

						Initialize();

				_table = data._table;

						return true;
					}
				catch
				{
					return false;
				}
			}

		#endregion

		#region >>> Public method <<<

		/// <summary>
		/// Clone a new instance.
		/// </summary>
		public TestingProperties Clone()
		{
			TestingProperties other = new TestingProperties();

			other._table.CopyFrom(this._table);

			return other;
		}
		
		/// <summary>
		/// Set property with Int32 type value.
		/// </summary>
		public void SetInt(string name, int value)
		{
			this.SetProperty(name, value);
		}

		/// <summary>
		/// Set property with Double type value.
		/// </summary>
		public void SetDbl(string name, double value)
		{
			this.SetProperty(name, value);
		}

		/// <summary>
		/// Set property with String type value.
		/// </summary>
		public void SetString(string name, string value)
		{
			this.SetProperty(name, value);
		}

		/// <summary>
		/// Set property with Boolean type value.
		/// </summary>
		public void SetBoolean(string name, bool value)
		{
			this.SetProperty(name, value);
		}

		/// <summary>
		/// Get property with Int32 type value.
		/// </summary>
		public int GetInt(string name, int defaultValue = 0)
		{
			int value = defaultValue;

			return this.TryGetProperty<int>(name, out value) ? value : defaultValue;
		}

		/// <summary>
		/// Get property with Boolean type value.
		/// </summary>
		public bool GetBoolean(string name, bool defaultValue = false)
		{
			bool value = defaultValue;

			return this.TryGetProperty<bool>(name, out value) ? value : defaultValue;
		}

		/// <summary>
		/// Get property with Double type value.
		/// </summary>
		public double GetDbl(string name, double defaultValue = 0)
		{
			double value = defaultValue;

			return this.TryGetProperty<double>(name, out value) ? value : defaultValue;
		}

		/// <summary>
		/// Get property with String type value.
		/// </summary>
		public string GetString(string name, string defaultValue = "")
		{
			string value = defaultValue;

			return this.TryGetProperty<string>(name, out value) ? value : defaultValue;
		}

		/// <summary>
		/// Determines whether the properties pool contain the property with the specified name.
		/// </summary>
		public bool HasProperty(string name)
		{
			lock (_lock)
			{
				if (name == null) return false;

				return _table.ContainsKey(name);
			}
		}

		#endregion
		
		#region >>> Internal method <<<

		internal dynamic GetProperty(string name)
		{
			KeyValueSerializePair pair = getPropertyPair(name);

			return (pair != null) ? pair.Value : null;
		}

		internal void SetProperty(string name, dynamic property)
		{
			lock (_lock)
			{
				KeyValueSerializePair exist = getPropertyPair(name);

				if (exist != null)
				{
					exist.Value = property;
				}
				else
				{
					_table.Add(name, new KeyValueSerializePair(name, property));
				}
			}
		}

		#endregion

		#region >>> Private method <<<

		private KeyValueSerializePair getPropertyPair(string name)
		{
			lock (_lock)
			{
				if (name == null) return null;

				if (!_table.ContainsKey(name)) return null;

				return _table[name];
			}
		}

		private bool TryGetProperty<T>(string name, out T value)
		{
			value = default(T);

			if (!this.HasProperty(name)) return false;

			dynamic exist = this.GetProperty(name);

			if (exist == null) return false;

			if (exist.GetType() != typeof(T)) return false;

			value = exist;

			return true;
		}

		private void Initialize()
		{
			_table = new KeyValueSerializeTable();
		}

		#endregion

		#region >>> Private static method <<<

		private static bool Equal(KeyValueSerializePair a, KeyValueSerializePair b)
		{
			return Equal(a, b.Key);
		}

		private static bool Equal(KeyValueSerializePair a, string name)
		{
			return a.Key == name;
		}

		#endregion
	}
}
