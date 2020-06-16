using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.Tester.Data
{
    public class MsrtData : System.ICloneable   
    {
        private object _lockObj;

        private Queue<string> _DataMemberQ = new Queue<string>();
        private Queue<string> _DataMemberList = new Queue<string>();

        public MsrtData()
        {
            this._lockObj = new object();
        }

        #region >>> Public Property <<<

        public Queue<string> DataMemberQ
        { 
            get { return _DataMemberQ; }
            set { lock ( this._lockObj ) { _DataMemberQ = value; } }
        }

        public Queue<string> DataMemberList
        {
            get { return _DataMemberList; }
            set { lock ( this._lockObj ) { _DataMemberList = value; } }
        }

        #endregion

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    
}
