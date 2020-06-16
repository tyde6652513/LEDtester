using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.Tester.TestServer
{
    public class ServerQueryEventArg : EventArgs
    {
        private int _cmdID;
        private double[] _bufferData;
        private string[] _strData;

        public ServerQueryEventArg(int CmdID, double[] bufferData, string[] strData)
        {
            this._cmdID = CmdID;
            this._bufferData = bufferData;
            this._strData = strData;
        }

        public ServerQueryEventArg()
        { 
        }

        public int CmdID
        {
            get { return this._cmdID;  }
            set { this._cmdID = value; }
        }

        public double[] BufferData
        {
            get { return this._bufferData;  }
            set { this._bufferData = value; }
        }

        public string[] StrData 
        {
            get { return this._strData; }
            set { this._strData = value; }
        }
    }
}
