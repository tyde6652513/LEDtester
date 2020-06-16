using System;
using System.Collections.Generic;
using System.Text;
using MPI.RemoteControl.Tester.ConstDefinition;

namespace MPI.RemoteControl.Tester.Command
{
    /// <summary>
    /// Test Item Command.
    /// 
    /// typedef struct _PBSORTER_CMD_ITEM_NAME
    ///	{
    ///		char szItemName[MAX_ITEM_NAME];					// Test Item Name
    ///	} 
    /// 
    /// typedef struct _PBSORTER_ITEM_PACKET
    ///	{
    ///		int iItemCount;									// Test Item Count
    ///		PBSORTER_CMD_ITEM ItemList[MAX_TEST_ITEM_LIST];
    ///	}
    /// </summary>
    public class CmdTestItem : MPIDS7600Command
    {
		public static MPIDS7600ConstDefinitionBase Const = MPIDS7600Command.ConstDefinition;

		// CommandID.
        public const int COMMAND_ID = (int)ETSECommand.ID_TEST_ITEM;

		// Length		// sizeof(Int32) + MAX_TEST_ITEM_LIST * MAX_ITEM_NAME
		public static int DATA_LEN = sizeof(Int32) + Const.MAX_TEST_ITEM_LIST * Const.MAX_ITEM_NAME;

        // Position
		public static int ITEM_COUNT_POS = 0;
		public static int ITEM_LIST_POS = ITEM_COUNT_POS + sizeof(Int32);

        public CmdTestItem()
            : base(COMMAND_ID, DATA_LEN)
        {
 
        }

        /// <summary>
        /// Item Count.
        /// </summary>
        public Int32 ItemCount
        {
            get
            {
                return this.GetInt32Data(ITEM_COUNT_POS);
            }
            set
            {
                this.SetInt32Data(ITEM_COUNT_POS, value);
            }
        }

        /// <summary>
        /// Get Test Item Name.
        /// </summary>
        public char[] GetItemName(int i)
        {
			if (i < 0 || i >= Const.MAX_TEST_ITEM_LIST) return null;

			return this.GetData(Const.MAX_ITEM_NAME, ITEM_LIST_POS + (i * Const.MAX_ITEM_NAME));
        }

        /// <summary>
        /// Get Test Item Name. 0-based
        /// </summary>
        public string GetItemNameString(int i)
        {
            return new string(GetItemName(i));
        }

        /// <summary>
        /// Set Test Item Name.
        /// </summary>
        public void SetItemName(int i, char[] itemName)
        {
			if (i < 0 || i >= Const.MAX_TEST_ITEM_LIST) return;
			if (itemName.Length > Const.MAX_ITEM_NAME) return;

			int startPos = ITEM_LIST_POS + (i * Const.MAX_ITEM_NAME);
			Array.Clear(this.Data, startPos, Const.MAX_ITEM_NAME);
            this.SetData(itemName.Length, startPos, itemName);
        }

        /// <summary>
        /// Set Test Item Name.
        /// </summary>
        public void SetItemName(int i, string itemName)
        {
            SetItemName(i, itemName.ToCharArray());
        }
    }
}
