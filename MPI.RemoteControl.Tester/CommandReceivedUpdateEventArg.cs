using System;
using System.Collections.Generic;
using System.Text;
using MPI.RemoteControl.Tester.Command; 

namespace MPI.RemoteControl.Tester
{
    public class CommandReceivedUpdateEventArg : EventArgs
    {
        private List<string> _list;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="datas"></param>
        public CommandReceivedUpdateEventArg()
        {
            this._list = new List<string>();
        }

        /// <summary>
        /// Add string 
        /// </summary>
        public void Add(string str)
        {
            this._list.Add(str);
        }

        /// <summary>
        /// Data List.
        /// </summary>
        public List<string> List
        {
            get
            {
                return this._list;
            }
        }
    }

    public class LotInfoEventArg : EventArgs
    {
        private CmdLotInfo _lotInfo;

        /// <summary>
        /// Constructor.
        /// </summary>
        public LotInfoEventArg(CmdLotInfo lotInfo)
        {
            this._lotInfo = lotInfo;
        }

        /// <summary>
        /// Data List.
        /// </summary>
        public CmdLotInfo LotInfo
        {
            get
            {
                return this._lotInfo;
            }
        }
    }

    public class WaferInEventArg : EventArgs
    {
        private int _slot;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="datas"></param>
        public WaferInEventArg(int slot)
        {
            this._slot = slot;
        }

        /// <summary>
        /// Slot Number.
        /// </summary>
        public int Slot
        {
            get
            {
                return this._slot;
            }
        }
    }

    public class TestItemEventArg : EventArgs
    {
        private CmdTestItem _testItem;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="datas"></param>
        public TestItemEventArg(CmdTestItem testItem)
        {
            this._testItem = testItem;
        }

        /// <summary>
        /// Data List.
        /// </summary>
        public CmdTestItem TestItem
        {
            get
            {
                return this._testItem;
            }
        }
    }

    public class BinGradeEventArg : EventArgs
    {
        private CmdBinGrade _binGrade;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="datas"></param>
        public BinGradeEventArg(CmdBinGrade binGrade)
        {
            this._binGrade = binGrade;
        }

        /// <summary>
        /// Data List.
        /// </summary>
        public CmdBinGrade BinGrade
        {
            get
            {
                return this._binGrade;
            }
        }
    }

    public class AutoEOTEventArg : EventArgs
    {
        private int _dieIndex;
        private int _chuckIndex;
        private CmdEOT _EOT; 

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="datas"></param>
        public AutoEOTEventArg(int dieIndex, int chuckIndex, CmdEOT EOT)
        {
            this._dieIndex = dieIndex; 
            this._chuckIndex = chuckIndex;
            this._EOT = EOT;
        }

        /// <summary>
        /// DieIndex.
        /// </summary>
        public int DieIndex
        {
            get
            {
                return this._dieIndex;
            }
        }

        /// <summary>
        /// ChuckIndex.
        /// </summary>
        public int ChuckIndex
        {
            get
            {
                return this._chuckIndex;
            }
        }

        /// <summary>
        /// EOT.
        /// </summary>
        public CmdEOT EOT
        {
            get
            {
                return this._EOT;
            }
        }
    }

	public class REOTEventArg : EventArgs
	{
		private CmdREOT _cmdREOT;

		public REOTEventArg(CmdREOT cmd)
		{
			this._cmdREOT = cmd;
		}

		public CmdREOT REOT
		{
			get { return this._cmdREOT; }
		}
	}
}
