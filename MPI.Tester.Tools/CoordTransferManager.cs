﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MPI.Tester.Maths;
using MPI.Maths;
using MPI.RemoteControl.Tester;
using MPI.Tester.Tools;



namespace MPI.Tester.Tools
{
    public class CoordTransferManager:Dictionary<string,CoordTransferTool>
    {
        public CoordTransferManager():base()
        {
            CoordTransferTool TransTool = new CoordTransferTool();
            TransTool.TransCoordName = "Tester2ChipID";
            this.Add(TransTool.TransCoordName,TransTool);
 
        }

        public void Add(CoordTransferTool coord)
        {
            string name = coord.TransCoordName ;
            if (name == "" || this.Keys.Contains(name))
            {
                name = this.Count.ToString();
            }
            this.Add(name, coord);
        }

    }

    public class CoordTransferTool:ICloneable,IDisposable
    {
        public List<RefPoint> RefList;
        public string TransCoordName = "";
        public bool IsValid = false;
        public bool IsDefaultTrans = false;

        //MPI.Tester.Maths.ma

        private Matrix _convertMatrix;
        private Matrix _inverseMatrix;
        #region costructor
        public CoordTransferTool()
        {
            _convertMatrix = null;
            Clear();
        }

        public CoordTransferTool(CoordTransferTool refData):this()
        {
            Clear();

            RefList.AddRange(refData.RefList.ToArray());
            TransCoordName = refData.TransCoordName;
            IsValid = refData.IsValid;
            _convertMatrix = refData._convertMatrix.Clone() as Matrix;
            IsDefaultTrans = false;

        }

        public CoordTransferTool(Rectangle recBase, Rectangle recNew, string chipID = "", string remark = ""):this()
        {
            PushData(recBase, recNew, chipID, remark);
            CalcConvertMatrix();
        }
       
        public CoordTransferTool(int fromQuadrant, int toQuadrant)
        {
            Matrix tMat = new Matrix(3,3);
            switch (fromQuadrant)
            {
                case 2:
                    { tMat[0, 0] *= -1; }
                    break;
                case 3:
                    {
                        tMat[0, 0] *= -1;
                        tMat[1, 1] *= -1;
                    }
                    break;
                case 4:
                    { tMat[1, 1] *= -1; }
                    break;
            }
            switch (toQuadrant)
            {
                case 2:
                    { tMat[0, 0] *= -1; }
                    break;
                case 3:
                    {
                        tMat[0, 0] *= -1;
                        tMat[1, 1] *= -1;
                    }
                    break;
                case 4:
                    { tMat[1, 1] *= -1; }
                    break;
            }
            _convertMatrix = tMat;
            _inverseMatrix = tMat.Inverse();

            IsValid = true;
        }
        #endregion

        #region
        public bool PushData(double x_T, double y_T, double x_new, double y_new, string chipID = "",string remark = "")//x,y of tester, xy of new coord
        {
            bool result = true;

            RefPoint rPoint = new RefPoint((int)x_T, (int)y_T, (int)x_new, (int)y_new, chipID);
            rPoint.Remark = remark;

            RefList.Add(rPoint);

            return result;

        }

        public bool PushData(Rectangle recBase,Rectangle recNew,  string chipID = "", string remark = "")//x,y of tester, xy of new coord
        {
            bool result = true;
            if (recBase.Bottom == recBase.Top || recNew.Bottom == recNew.Top)//防止只有一維的矩形出現
            {
                
                int w = recBase.Width;
                Size s = new Size(w, 1);
                Point p = recBase.Location;
                recBase = new Rectangle(p, s);
                Console.WriteLine("[CoordTransferManager] PushData(),Bottom = Top,recBase modify size " + w.ToString() + ",1");

                int wn = recNew.Width;
                Size sn = new Size(wn, 1);
                Point pn = recNew.Location;
                recNew = new Rectangle(pn, sn);
                Console.WriteLine("[CoordTransferManager] PushData(),Bottom = Top,recNew modify size " + wn.ToString() + ",1");
            }

            if (recBase.Left == recBase.Right || recNew.Left == recNew.Right)
            {                
                int h = recBase.Height;
                Size s = new Size(1, h);
                Point p = recBase.Location;
                recBase = new Rectangle(p, s);
                Console.WriteLine("[CoordTransferManager] PushData(),Left = Right,recBase modify size 1," + h.ToString());

                int hn = recNew.Height;
                Size sn = new Size(1, hn);
                Point pn = recNew.Location;
                recNew = new Rectangle(pn, sn);
                Console.WriteLine("[CoordTransferManager] PushData(),Left = Right,recNew modify size 1," + hn.ToString());
            }

            RefPoint ltPoint = new RefPoint(recBase.Left, recBase.Top, recNew.Left, recNew.Top, chipID);
            ltPoint.Remark = remark + "lt";
            RefPoint lbPoint = new RefPoint(recBase.Left, recBase.Bottom, recNew.Left, recNew.Bottom, chipID);
            lbPoint.Remark = remark + "lb";
            RefPoint rtPoint = new RefPoint(recBase.Right, recBase.Top, recNew.Right, recNew.Top, chipID);
            rtPoint.Remark = remark + "rt";
            RefPoint rbPoint = new RefPoint(recBase.Right, recBase.Bottom, recNew.Right, recNew.Bottom, chipID);
            rbPoint.Remark = remark + "rb";

            RefList.Add(ltPoint);
            RefList.Add(lbPoint);
            RefList.Add(rtPoint);
            RefList.Add(rbPoint);

            return result;

        }

        public Matrix CalcConvertMatrix(bool isfarFrom0 = false)
        {
            if (RefList.Count >= 3)
            {
                int row = RefList.Count;
                double[,] srcArr = new double[row, 3];
                double[,] tarArr = new double[row, 3];

                for (int i = 0; i < row; ++i)
                {
                    RefPoint rPoint = RefList[i];

                    srcArr[i, 0] = rPoint.BaseX;
                    srcArr[i, 1] = rPoint.BaseY;
                    srcArr[i, 2] = 1;

                    tarArr[i, 0] = rPoint.NewX;
                    tarArr[i, 1] = rPoint.NewY;
                    tarArr[i, 2] = 1;
                }

                Matrix sMat = new Matrix(srcArr, row, 3);
                Matrix tMat = new Matrix(tarArr, row, 3);

                _convertMatrix = Matrix.LS2GetTranferMatrix(sMat, tMat);


                //20190704
                if (_convertMatrix == null)
                {
                    Console.WriteLine("[CoordTransferManager] CalcConvertMatrix(),_convertMatrix == null ,_convertMatrix = I33");
                    _convertMatrix = new Matrix(3, 3, false);//轉換失效直接使用3*3的基本矩陣
                }

                if (_convertMatrix != null)
                {
                    _inverseMatrix = _convertMatrix.Inverse();
                }

				if ( isfarFrom0 )
				{
					for ( int i = 0; i < 3; ++i )
					{
						for ( int j = 0; j < 3; ++j )
						{
							double val = _convertMatrix[ i, j ];
                            int valInt = (int)(Math.Round(val));

                            //if ( val % 1 > 0.4 )
                            //{ valInt++; }

							_convertMatrix[ i, j ] = ( double ) valInt;
						}
					}
				}

                IsValid = true;
                IsDefaultTrans = false;
            }

            return _convertMatrix;
        }

        public Matrix TransCoord(int x, int y)
        {
            Matrix m31 = new Matrix(3, 1, false);
            m31[0, 0] = x;
            m31[1, 0] = y;
            m31[2, 0] = 1;

            if (_convertMatrix != null)
            {
                return _convertMatrix * m31;
            }
            else
            {
                return m31;
            }

        }

        public Matrix TransCoord(ref int x, ref int y)
        {
            Matrix m31 = TransCoord(x, y);

            x = (int)Math.Round(m31[0, 0]);
            y = (int)Math.Round(m31[1, 0]);

            return m31;
        }

        public Matrix TransCoord(double x, double y)
        {
            Matrix m31 = new Matrix(3, 1, false);
            m31[0, 0] = x;
            m31[1, 0] = y;
            m31[2, 0] = 1;

            if (_convertMatrix != null)
            {
                return _convertMatrix * m31;
            }
            else
            {
                return m31;
            }

        }

        public Point TransCoord2P(int x, int y)
        {
            Matrix m31 = TransCoord(x, y);
            Point  p = new Point((int)Math.Round(m31[0, 0]), (int)Math.Round(m31[1,0]));
            return p;
        }

        public Matrix TransCoord(ref float x, ref float y)
        {
            Matrix m31 = TransCoord(x, y);

            x = (float)m31[0, 0];
            y = (float)m31[1, 0];

            return m31;
        }

        public Matrix TransCoord(ref double x, ref double y)
        {
            Matrix m31 = TransCoord(x, y);

            x = m31[0, 0];
            y = m31[1, 0];

            return m31;
        }

        public Rectangle TransCoord(Rectangle rec)
        {
            double l = rec.Left;
            double r = rec.Right;
            double b = rec.Bottom;
            double t = rec.Top;

            TransCoord(ref l, ref t);
            TransCoord(ref r, ref b);

            return new Rectangle((int)Math.Round(l), (int)Math.Round(b), (int)Math.Round((r - l)), (int)Math.Round((t - b)));
        }

        public Matrix I_TransCoord(double x, double y)
        {
            Matrix m31 = new Matrix(3, 1, false);
            m31[0, 0] = x;
            m31[1, 0] = y;
            m31[2, 0] = 1;

            if (_inverseMatrix != null)
            {
                return _inverseMatrix * m31;
            }
            else
            {
                return m31;
            }

        }

        public Matrix I_TransCoord(float x, float y)
        {
            Matrix m31 = I_TransCoord((double)x, (double)y);

            return m31;
        }

        public void I_TransCoord(ref double x, ref double y)
        {
            Matrix m31 = I_TransCoord(x, y);

            x = m31[0, 0];
            y = m31[1, 0];
        }

        public void I_TransCoord(ref float x, ref float y)
        {
            Matrix m31 = I_TransCoord(x, y);

            x = (float)m31[0, 0];
            y = (float)m31[1, 0];
        }

        public Rectangle I_TransCoord(Rectangle rec)
        {
            double l = rec.Left;
            double r = rec.Right;
            double b = rec.Bottom;
            double t = rec.Top;

            I_TransCoord(ref l, ref t);
            I_TransCoord(ref r, ref b);

            return new Rectangle((int)l, (int)b, (int)(r - l), (int)(t - b));
        }
		public Matrix Shift(ref double x, ref double y)
        {
            Matrix m31 = Shift(_convertMatrix, ref x, ref y);           
            return m31;
        }

        public Matrix I_Shift(ref double x, ref double y)
        {
            Matrix m31 = Shift(_inverseMatrix, ref x, ref y);           
            return m31;
        }

        public Matrix Rotate(ref double x, ref double y)
        {
            Matrix m31 = Rotate(_convertMatrix, ref x, ref y);
            return m31;
        }

        public Matrix I_Rotate(ref double x, ref double y)
        {
            Matrix m31 = Rotate(_convertMatrix, ref x, ref y);
            return m31;
        }

        public Matrix Matrix
        {
            set
            {
                _convertMatrix = value;
                IsDefaultTrans = false;
            }
            get { return _convertMatrix; }
            
        }

        public Matrix I_Matrix
        {
            set
            {
                _inverseMatrix = value;
                IsDefaultTrans = false;
            }
            get { return _inverseMatrix; }

        }
		public Matrix SetMatrixShift(double x, double y)
        {
            _convertMatrix[0, 2] += x;
            _convertMatrix[1, 2] += y;
            _inverseMatrix = _convertMatrix.Inverse();
            IsValid = _inverseMatrix != null;
            return _convertMatrix;
        }

        public Matrix SetMatrixRotate(double degree)
        {
            Matrix tMat = new Maths.Matrix(3, 3);

            double arc = Math.PI*degree/180;


            tMat[0, 0] = Math.Cos(arc);
            tMat[0, 1] = -Math.Sin(arc);
            tMat[1, 0] = Math.Sin(arc);
            tMat[1, 1] = Math.Cos(arc);
            _convertMatrix = _convertMatrix* tMat;
            _inverseMatrix = _convertMatrix.Inverse();
            IsValid = _inverseMatrix != null;
            return _convertMatrix;
        }


        public object Clone()
        {
            CoordTransferTool obj = new CoordTransferTool();
            obj.RefList.AddRange(this.RefList.ToArray());
            obj.TransCoordName = this.TransCoordName;
            obj.IsValid = this.IsValid;
            obj._convertMatrix = this._convertMatrix.Clone() as Matrix;
            if (this._inverseMatrix != null)
            {
                obj._inverseMatrix = this._inverseMatrix.Clone() as Matrix;
            }
            return obj;
        }

        public void Clear()
        {
            RefList = new List<RefPoint>();
            _convertMatrix = new Matrix(3);
            IsValid = true;
            IsDefaultTrans = true;
        }

        public void Dispose()
        {
            Clear();
            _convertMatrix = null;
        }
        #endregion

        #region >>private method<<
        private Matrix Shift(Matrix oprate33Mat,ref double x, ref double y)
        {
            Matrix m31 = new Matrix(3, 1, false);
            m31[0, 0] = x;
            m31[1, 0] = y;
            m31[2, 0] = 1;

            if (oprate33Mat != null)
            {
                m31[0, 0] += oprate33Mat[0, 2];
                x = m31[0, 0];
                m31[1, 0] += oprate33Mat[1, 2];
                y = m31[1, 0];
            }

            return m31;
        }

        public Matrix Rotate(Matrix oprate33Mat, ref double x, ref double y)
        {
            Matrix m31 = new Matrix(3, 1, false);
            m31[0, 0] = x;
            m31[1, 0] = y;
            m31[2, 0] = 1;

            if (oprate33Mat != null)
            {
                Matrix tempMat = oprate33Mat.Clone() as Matrix;
                tempMat[0, 2] = 0;
                tempMat[1, 2] = 0;
                tempMat[2, 0] = 0;
                tempMat[2, 1] = 0;
                Matrix tm31 = tempMat * m31;
                m31 = tm31;
                x = m31[0, 0];
                y = m31[1, 0];
    }

            return m31;
        }
        #endregion
    }


    [Serializable]
    public class RefPoint
    {

        private int _baseX;
        private int _baseY;
        private int _newX;
        private int _newY;
        private string _chipName;
        private string _remark;

        public int BaseX
        {
            get { return _baseX; }
            set { _baseX = value; }
        }

        public int BaseY
        {
            get { return _baseY; }
            set { _baseY = value; }
        }

        public int NewX
        {
            get { return _newX; }
            set { _newX = value; }
        }

        public int NewY
        {
            get { return _newY; }
            set { _newY = value; }
        }

        public string ChipName
        {
            get { return _chipName; }
            set { _chipName = value; }
        }

        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

        public RefPoint()
        {
            _baseX = 0;
            _baseY = 0;
            _newX = 0;
            _newY = 0;
            _chipName = "";
            _remark = "";
        }

        public RefPoint(int bx, int by, int nx, int ny, string chipName)
            : this()
        {
            _baseX = bx;
            _baseY = by;
            _newX = nx;
            _newY = ny;
            _chipName = chipName;
        }
    }

}
