using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MPI.RemoteControl2.Tester.Mpi.Command.Base
{
	public class TransferableDataFactory
	{
		private Assembly _assembly;
		private Type _type;

		public TransferableDataFactory()
		{
			_type = this.GetType();
			_assembly = Assembly.GetAssembly(_type);
		}

		public TransferableCommonObjectBase CreateObject(ETransferableCommonObject id)
		{
			try
			{
				string typeName = String.Format("{0}.{1}", _type.Namespace, id.ToString());

				object obj = _assembly.CreateInstance(typeName);

				return obj as TransferableCommonObjectBase;
			}
			catch
			{
				return null;
			}
		}
	}
}
