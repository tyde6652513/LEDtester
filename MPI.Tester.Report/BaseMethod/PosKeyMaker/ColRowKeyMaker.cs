using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using MPI.Tester.Tools;

namespace MPI.Tester.Report.BaseMethod.PosKeyMaker
{
    public class PosKeyMakerBase
    {
        int _colIndex = 0;
        int _rowIndex = 1;
        CoordTransferTool _coordTransTool;
        List<int> _extraPosColList = new List<int>();// subx,suby
        public PosKeyMakerBase(int xIndex, int yIndex, List<int> _extraPosColList = null)
        {
            
            _coordTransTool = new CoordTransferTool();
            _coordTransTool.PushData(1.0, 1.0, 1.0, 1.0);
            _coordTransTool.PushData(-1.0, 1.0, -1.0, 1.0);
            _coordTransTool.PushData(-1.0, -1.0, -1.0, -1.0);
            _coordTransTool.PushData(1.0, -1.0, 1.0, -1.0);//預設單位矩陣，座標輸入/輸出相同
            _coordTransTool.CalcConvertMatrix();
            _colIndex = xIndex;
            _rowIndex = yIndex;
            if (_extraPosColList != null)
            {
                _extraPosColList.AddRange(_extraPosColList.ToArray());
            }
        }
        public PosKeyMakerBase(int xIndex, int yIndex,CoordTransferTool ct ,List<int> _extraPosColList = null):
            this(xIndex, yIndex, _extraPosColList)
        {
            _coordTransTool = ct.Clone() as CoordTransferTool;
        }

        public virtual string GetPosKey(string[] rawData)
        {
            string colrowKey  = "";//= rawData[this._resultTitleInfo.ColIndex].ToString() + "_" + rawData[this._resultTitleInfo.RowIndex].ToString();
            int x = 0, y=0;
            if (int.TryParse(rawData[_colIndex], out x) &&
                int.TryParse(rawData[_rowIndex], out y))
            {
                _coordTransTool.TransCoord(ref x, ref y);

                colrowKey = x.ToString() + "_" + y.ToString();
                int length = _extraPosColList.Count;
                for (int i = 0; i < length; ++i)
                {
                    colrowKey += "_";
                    colrowKey += rawData[_extraPosColList[i]].ToString();

                }
            }
            return colrowKey;
        }

    }

}
