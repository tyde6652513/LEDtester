using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using MPI.Tester.Data;

namespace MPI.Tester.Report
{
	public enum ERIChipMark : int
	{
		None = 0,

		//AllTest = 1,
		//AllTestAndChipFail = 2,

		SimpleTest = 3,
		WaitReCalSimpleTest = 4,
		ReCalSimpleTestFinish = 5,
		ReCalSimpleTestFail = 6,

		WaitInterpolation = 7,
		WaitInterpolationAndElecFail = 8,
		InterpolationFinish = 9,
		InterpolationFinishAndElecFail = 10,
	}

	public class RIChip
	{
        private int _col = 0;
        private int _row = 0;
		//private bool _isPass = true;
		private ERIChipMark _mark = ERIChipMark.None;
		private Dictionary<string, float> _rawData = new Dictionary<string, float>();

		public RIChip(int col, int row)
		{
            this._col = col;

            this._row = row;
		}

		public int Col
		{
            //get 
            //{
            //    if (this._rawData.ContainsKey(EProberDataIndex.COL.ToString()))
            //    {
            //        return (int)this._rawData[EProberDataIndex.COL.ToString()];
            //    }
            //    else
            //    {
            //        return int.MinValue;
            //    }
            //}

            get { return this._col; }
            set { this._col = value; }
		}

		public int Row
		{
            //get 
            //{
            //    if (this._rawData.ContainsKey(EProberDataIndex.ROW.ToString()))
            //    {
            //        return (int)this._rawData[EProberDataIndex.ROW.ToString()];
            //    }
            //    else
            //    {
            //        return int.MinValue;
            //    }
            //}

            get { return this._row; }
            set { this._row = value; }
        }

        //public bool IsPass
        //{
        //    set { this._isPass = value; }
        //    get { return this._isPass; }
        //}

		public ERIChipMark Mark
		{
			set { this._mark = value; }
			get { return this._mark; }
		}

		public Dictionary<string, float> RawData
		{
			set { this._rawData = value; }
			get { return this._rawData; }
		}
	}
}
