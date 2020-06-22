using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPI.RemoteControl.Tester.Mpi.Command;

namespace MPI.RemoteControl.Tester.Mpi.Channel.Misc
{
    /// <summary>
    /// Ready EOT signal for Weimin tester
    /// </summary>
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
