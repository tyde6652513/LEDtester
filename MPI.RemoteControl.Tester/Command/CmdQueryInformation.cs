using System;
using System.Collections.Generic;
using System.Text;
using MPI.RemoteControl.Tester.ConstDefinition;

namespace MPI.RemoteControl.Tester.Command
{
	public class CmdQueryInformation : MPIDS7600Command
	{
		public static MPIDS7600ConstDefinitionBase Const = MPIDS7600Command.ConstDefinition;

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
				return base.GetTransferableDataObject(TRANSFERABLE_ITEM_POS, obj) ? obj : null;
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
