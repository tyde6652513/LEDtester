using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPI.RemoteControl2.Tester.Mpi.Command.Base;
using MPI.RemoteControl2.Tester.Mpi.Command;

namespace MPI.RemoteControl2.Tester.Mpi.Command
{
	[Serializable]
	public class CmdPropertyBased : CmdMPIBased
	{
		protected TestingProperties _tbl;
		private readonly int TABLE_POS;

		public CmdPropertyBased(int cmdID, int dataLen)
			: base(cmdID, dataLen)
		{
			TABLE_POS = dataLen;
		}

		#region >>> Public method <<<

		public T GetPropertyValue<T>(string name, T Default = default(T))
		{
			dynamic value = this.GetProperty(name);
			
			if (value is T) return value;

			return Default;
		}

		public void SetPropertyValue<T>(string name, T value)
		{
			this.SetProperty(name, value);
		}

		public bool GetBooleanProperty(string name, bool bDefault = false)
		{
			return GetPropertyValue<bool>(name, bDefault);
		}

		public int GetIntegerProperty(string name, int nDefault = 0)
		{
			return GetPropertyValue<int>(name, nDefault);
		}

		public double GetDoubleProperty(string name, double dDefault = 0)
		{
			return GetPropertyValue<double>(name, dDefault);
		}

		public string GetStringProperty(string name, string Default = "")
		{
			return GetPropertyValue<string>(name, Default);
		}

		public void SetIntegerProperty(string name, int value)
		{
			this.SetProperty(name, value);
		}

		public void SetDblProperty(string name, double value)
		{
			this.SetProperty(name, value);
		}

		public void SetBooleanProperty(string name, bool value)
		{
			this.SetProperty(name, value);
		}

		public void SetStringProperty(string name, string value)
		{
			this.SetProperty(name, value);
		}

		public override byte[] Serialize()
		{
			if (_tbl != null)
				base.SetTransferableDataObject(TABLE_POS, _tbl);

			return base.Serialize();
		}

		public override bool Deserialize(byte[] data)
		{
			if (!base.Deserialize(data)) return false;

			TestingProperties properties = new TestingProperties();

			if (base.GetTransferableDataObject(TABLE_POS, properties))
			{
				_tbl = properties;
			}

			return true;
		}

		#endregion

		#region >>> Internal method <<<

		internal bool SetProperty(string name, dynamic property)
		{
			if (_tbl == null)
				_tbl = new TestingProperties();

			_tbl.SetProperty(name, property);

			return true;
		}

		internal dynamic GetProperty(string name)
		{
			if (_tbl == null) return null;

			return _tbl.GetProperty(name);
		}

		#endregion
	}
}
