using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Data
{
	[Serializable]
	public class TestGroupCtrl
	{
		private bool[] _indexArray;

		public TestGroupCtrl()
		{
			this._indexArray = new bool[3] { true, true, true };
		}

		public void SetIndexData(bool[] indexList)
		{
			this._indexArray = indexList;
		}

		public bool this[int index]
		{
			get { return this._indexArray[index]; }
			set { this._indexArray[index] = value; }
		}

		public bool[] SelectIndex
		{
			get { return this._indexArray; }
			set { this._indexArray = value; }
		}

        public int Count
        {
            get { return this._indexArray.Length; }
        }
	}
}
