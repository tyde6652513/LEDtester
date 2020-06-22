using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MPI.RemoteControl2.Tester.Mpi.Command.Base
{
	public class TransferableCommonObjectBase : ITransferable
	{
		private TransferableDataObject _dataObject;

		public override int GetHashCode() { return this.ObjectIdentifiedID; }

		public TransferableCommonObjectBase()
		{
			_dataObject = new TransferableDataObject();
			_dataObject.IdentifiedID = (int)ETransferableCommonObject.None;
		}

		public string Serialize()
		{
			_dataObject.IdentifiedID = this.ObjectIdentifiedID;
			_dataObject.DataContext = this.SerializeCommonObject();

			return _dataObject.Serialize();
		}

		public bool Deserialize(string context)
		{
			if (_dataObject.Deserialize(context))
			{
				return this.DeserializeCommonObject(_dataObject.DataContext);
			}
			else
			{
				return false;
			}
		}

		protected virtual int ObjectIdentifiedID
		{
			get { return _dataObject.IdentifiedID; }
		}

		protected virtual string SerializeCommonObject()
		{
			return String.Empty;
		}

		protected virtual bool DeserializeCommonObject(string context)
		{
			return true;
		}
	}
}
