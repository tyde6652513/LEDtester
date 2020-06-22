using System;
using System.Collections.Generic;
using System.Text;
using MPI.RemoteControl2.Tester.ConstDefinition;
using MPI.RemoteControl2.Tester.Mpi.Command.Base;

namespace MPI.RemoteControl2.Tester.Mpi.Command
{
	public class CmdQueryInformation : CmdMPIBased
	{
		private static TransferableDataFactory s_DataFactory = new TransferableDataFactory();

		public static MPIDS7600ConstDefinitionBase Const = CmdMPIBased.ConstDefinition;

        // CommandID.
		public const Int32 COMMAND_ID = (int)ETSECommand.ID_QUERY_INFORMATION;

        // Length
        public static Int32 DATA_LEN = 0;

		// Position
		public static int TRANSFERABLE_ITEM_POS = 0;


        /// <summary>
        /// Constructor.
        /// </summary>
		public CmdQueryInformation()
            : base(COMMAND_ID, DATA_LEN)
        {

        }

		public TransferableCommonObjectBase TransferableItemBase
		{
			get
			{
				TransferableCommonObjectBase obj = new TransferableCommonObjectBase();

				if (!base.GetTransferableDataObject(TRANSFERABLE_ITEM_POS, obj)) return null;

				obj = s_DataFactory.CreateObject((ETransferableCommonObject)obj.GetHashCode());

				if (obj == null) return null;

				if (!this.GetTransferableItem(obj)) return null;

				return obj;
			}
		}

		public bool SetTransferableItem(TransferableCommonObjectBase item)
		{
			return base.SetTransferableDataObject(TRANSFERABLE_ITEM_POS, item);
		}

		public bool GetTransferableItem(TransferableCommonObjectBase item)
		{
			return base.GetTransferableDataObject(TRANSFERABLE_ITEM_POS, item);
		}
	}
}
