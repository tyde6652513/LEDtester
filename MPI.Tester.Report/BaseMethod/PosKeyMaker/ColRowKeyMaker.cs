using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Report.BaseMethod.PosKeyMaker
{
    public class PosKeyMakerBase
    {
        List<int> PosColList = new List<int>();
        public PosKeyMakerBase( List<int> colList)
        {
            PosColList.AddRange(colList.ToArray());
        }

        public virtual string GetPosKey(string[] rawData)
        {
            string colrowKey  = "";//= rawData[this._resultTitleInfo.ColIndex].ToString() + "_" + rawData[this._resultTitleInfo.RowIndex].ToString();
            int length =  PosColList.Count;
            for(int i =0 ; i < length ;++i)
            {
                colrowKey += rawData[PosColList[i]].ToString();
                if(i < length-1)
                {
                    colrowKey += "_";
                }
            }
            return colrowKey;
        }

    }

}
