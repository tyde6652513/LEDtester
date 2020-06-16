using System;
using System.Collections.Generic;
using System.Text;

using MPI.Tester.Data;

namespace MPI.Tester.TestKernel
{
    public class SystemStatus
    {
        private object _lockObj;

		private EKernelState _state;
		private EErrorCode _errorCode;

        public SystemStatus()
        { 
            this._lockObj = new object();

			this._state = EKernelState.Not_Ready;
			this._errorCode = EErrorCode.NONE;
        }

        #region >>> Public Property <<<

		public EKernelState State
		{
			get { return this._state; }
			set { lock (this._lockObj) { this._state = value; } }
		}

		public EErrorCode ErrorCode
		{
			get { return this._errorCode; }
			set { lock (this._lockObj) { this._errorCode = value; } }
		}

        #endregion
    }
}
