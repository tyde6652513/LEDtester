using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Maths
{
    public class Matrix : ICloneable
    {
        double[,] _matrix;
        int _col, _row;
        object objLock = new object();

        public Matrix()
        {
            _col = 0;
            _row = 0;
            _matrix = null;
        }

        public Matrix(int row, int col, bool isZero = false)
            : this()
        {
            _col = col;
            _row = row;
            _matrix = CreateNM2DArray(row, col, isZero);
        }
        public Matrix(int row, bool isZero = false)
            : this(row, row, isZero)
        {
        }

        public Matrix(double[,] rMat, int row, int col)
            : this()
        {
            if (rMat != null && col * row == rMat.Length)
            {
                _matrix = CloneMat(rMat, row, col);
                _col = col;
                _row = row;
            }
        }


        public Matrix(Matrix rMat)
            : this()
        {
            _matrix = CloneMat(rMat.getMatrix(), rMat.RowSize, rMat.ColSize);
            _col = rMat.ColSize;
            _row = rMat.RowSize;
        }

        #region >>public property<<

        public int ColSize
        {
            get
            {
                return _col;
            }
        }

        public int RowSize
        {
            get
            {
                return _row;
            }
        }

        public double this[int row, int col]
        {
            get { return (row <= _row) && (col <= _col) ? _matrix[row, col] : double.NaN; }
            set
            {
                lock (objLock)
                {
                    if ((row <= _row) && (col <= _col))
                    {
                        _matrix[row, col] = value;
                    }
                }
            }
        }
        #endregion

        #region >>private method <<


        private double[,] CloneMat(double[,] rMat, int row, int col)
        {
            double[,] obj = new double[row, col];

            if (rMat != null)
            {
                for (int y = 0; y < row; ++y)
                {
                    for (int x = 0; x < col; ++x)
                    {
                        obj[y, x] = rMat[y, x];
                    }
                }
            }
            return obj;
        }

        private double[,] PrepatreGauss(double[,] lMat, double[,] rMat, int row, int col)
        {
            double[,] obj = new double[row, col * 2];

            if (rMat != null && lMat != null &&
                lMat.Length == rMat.Length)
            {
                for (int y = 0; y < row; ++y)
                {
                    for (int x = 0; x < col; ++x)
                    {
                        obj[y, x] = lMat[y, x];
                        obj[y, x + col] = rMat[y, x];
                    }
                }
            }
            return obj;
        }

        private double[,] PrepatreGauss(int row)
        {
            double[,] obj = new double[row, row * 2];
            int col = row;
            Matrix rMat = new Matrix(row);

            if (rMat != null && _matrix != null &&
                _matrix.Length == rMat._matrix.Length)
            {
                for (int y = 0; y < row; ++y)
                {
                    for (int x = 0; x < col; ++x)
                    {
                        obj[y, x] = _matrix[y, x];
                        obj[y, x + col] = rMat[y, x];
                    }
                }
            }
            return obj;
        }

        private double[,] getMatrix()
        {
            return _matrix;
        }

        private double[,] CreateAdjointMarix(int shiftRow, int shiftCol, int oriMatrixSize, int window = 0)
        {

            if (window == 0)
            {
                window = oriMatrixSize;
            }
            double[,] tMat = new double[window, window];

            for (int y = 0; y < window; ++y)
            {
                for (int x = 0; x < window; ++x)
                {
                    tMat[y, x] = _matrix[(y + shiftRow + 1) % oriMatrixSize, (x + shiftCol + 1) % oriMatrixSize];
                }
            }
            return tMat;
        }

        private double[,] CreateSubMatrix(int shiftRow, int shiftCol, int window)
        {
            double[,] tMat = new double[window, window];

            for (int y = 0; y < window; ++y)
            {
                for (int x = 0; x < window; ++x)
                {
                    tMat[y, x] = _matrix[(y + shiftRow), (x + shiftCol)];
                }
            }
            return tMat;
        }

        private Matrix GauseUpMat(bool isTransTo1 = false)
        {
            double baseFactor = 1, opFactor = 1;
            double[,] iMat = (this.Clone() as Matrix).getMatrix();

            for (int refR = 0; refR < _row; refR++)
            {
                if (isTransTo1)
                {
                    baseFactor = iMat[refR, refR];
                    for (int col = refR; col < _row; col++)
                    {
                        iMat[refR, col] /= baseFactor;
                    }
                }

                for (int opR = refR + 1; opR < _row; opR++)
                {
                    opFactor = iMat[opR, refR] / iMat[refR, refR];
                    for (int col = refR; col < _col; col++)
                    {
                        iMat[opR, col] -= iMat[refR, col] * opFactor;
                    }
                }
            }

            return new Matrix(iMat, _row, _col);
        }




        #endregion


        #region >>public method <<
        public double DET()
        {
            double val = 1;

            Matrix mat = GauseUpMat();

            for (int i = 0; i < _row; ++i)
            {
                val *= mat[i, i];
            }
            //if (ColSize > 0 && RowSize > 0 && ColSize == RowSize)
            //{
            //    for (int x = 0; x < _col; x++)
            //    {
            //        int factor = (x + 1) % 2 == 0 ? -1 : 1;
            //        double det =  DET(0, x, _col, _col - 1);
            //        val += factor * _matrix[0, x] * det;
            //    }
            //}
            return val;
        }

        public Matrix Inverse(double detMinval = 0.00001)//to prevent float error ,int other words, prevent not linear independent enough
        {
            double[,] iMat = null;

            double baseFactor = 1, opFactor = 1;

            if (Math.Abs(DET()) > detMinval)
            {
                iMat = PrepatreGauss(_row);

#if DEBUG
                Matrix tmat = new Matrix(iMat,_row,_col*2);
                //PrintMatrix(iMat,col:_col*2);
                tmat.PrintMatrix();
                Console.WriteLine("");

#endif

                for (int refR = 0; refR < _row; refR++)
                {
                    baseFactor = iMat[refR, refR];
                    for (int col = refR; col < _row * 2; col++)
                    {
                        iMat[refR, col] /= baseFactor;
                    }

                    for (int opR = refR + 1; opR < _row; opR++)
                    {
                        opFactor = iMat[opR, refR];
                        for (int col = refR; col < _col * 2; col++)
                        {
                            iMat[opR, col] -= iMat[refR, col] * opFactor;
                        }
                    }
                }

                for (int refR = _row - 1; refR > 0; refR--)
                {

                    for (int opR = refR - 1; opR >= 0; opR--)
                    {
                        opFactor = iMat[opR, refR];
                        for (int col = 0; col < _col * 2; col++)
                        {
                            iMat[opR, col] -= iMat[refR, col] * opFactor;
                        }
                    }
                }
#if DEBUG
                PrintMatrix(iMat, col: _col * 2);
                Console.WriteLine("");
#endif
                Matrix fullMat = new Matrix(iMat, _row, _col * 2);

                return new Matrix(fullMat.CreateSubMatrix(0, _col, _col), _row, _col);
            }

            return null;
        }

        public Matrix Transpose()
        {
            Matrix transMat = new Matrix(_col, _row, true);

            for (int y = 0; y < _row; y++)
            {
                for (int x = 0; x < _col; x++)
                {
                    transMat[x, y] = this[y, x];
                }
            }
            return transMat;
        }

        public void PrintMatrix(double[,] iMat = null, string format = "0.00", int row = 0, int col = 0)
        {
            if (iMat == null)
            {
                iMat = _matrix;
            }
            if (row == 0)
            {
                row = _row;
            }
            if (col == 0)
            {
                col = _col;
            }
            for (int y = 0; y < row; ++y)
            {
                string outStr = "";
                for (int x = 0; x < col; ++x)
                {
                    outStr += (iMat[y, x].ToString(format) + ",");
                }

                Console.WriteLine(outStr);
            }
        }

        public string ToString(double[,] iMat = null, string format = "0.00", int row = 0, int col = 0)
        {
            string outStr = "";
            if (iMat == null)
            {
                iMat = _matrix;
            }
            if (row == 0)
            {
                row = _row;
            }
            if (col == 0)
            {
                col = _col;
            }
            for (int y = 0; y < row; ++y)
            {
                string outStr1 = "";
                for (int x = 0; x < col; ++x)
                {
                    outStr1 += (iMat[y, x].ToString(format) + ",");
                }

                outStr += "\n" + outStr1;
            }

            return outStr;
        }

        public double DET(int shiftRow, int shiftCol, int size, int windowSize)
        {
            if (ColSize > 0 && RowSize > 0 && ColSize == RowSize)
            {
                int i;
                double[,] tMat = CreateAdjointMarix(shiftRow, shiftCol, size, windowSize);

                if (windowSize == 1)
                {
                    return tMat[0, 0];
                }

                if (windowSize == 2)
                {
                    return tMat[0, 0] * tMat[1, 1] - tMat[1, 0] * tMat[0, 1];
                }
                double sum = 0;
                for (i = 0; i < windowSize; i++)
                {
                    int factor = (shiftRow + 1 + i) * (shiftCol + 1) % 2 == 0 ? -1 : 1;
                    sum += tMat[0, 0] * DET(_row - windowSize, shiftCol + i, size, windowSize - 1);
                }

                return sum;

            }
            return 0;
        }

        //2D coord should be as mat[N,3]  
        //like
        //[15,10,1]
        //[3,2,1] 
        //[30,20,1]
        //[12,8,1]

        public static Matrix LS2GetTranferMatrix(Matrix sMat, Matrix tMat)
        {
            Matrix transMat = null;

            if (sMat != null && tMat != null &&
                sMat.RowSize == tMat.RowSize &&
                sMat.ColSize == tMat.ColSize)
            {
                Matrix lsSMat = sMat.Transpose() * sMat;
#if DEBUG
                Console.WriteLine("lsSMat");
                lsSMat.PrintMatrix();
#endif

                Matrix lstMat = tMat.Transpose() * sMat;
#if DEBUG
                Console.WriteLine("lstMat");
                lstMat.PrintMatrix();
#endif
                Matrix lsSMat_Inv = lsSMat.Inverse();//lsSMat.Inverse();
#if DEBUG
                Console.WriteLine("lsSMat_Inv");
                lsSMat_Inv.PrintMatrix();
                Console.WriteLine("");
#endif
                transMat = lstMat * lsSMat_Inv;
            }
            return transMat;
        }

        public static double[,] CreateNM2DArray(int N, int M, bool isZero = false)//Row,Col, is Identity matrix
        {
            double[,] mat = new double[N, M];
            for (int y = 0; y < N; ++y)
            {
                for (int x = 0; x < M; ++x)
                {
                    mat[y, x] = x == y && !isZero ? 1 : 0;//Identity matrix
                }
            }
            return mat;
        }

        public object Clone()
        {
            double[,] mat = CloneMat(_matrix, _row, _col);

            Matrix obj = new Matrix(mat, _row, _col);

            return obj;
        }

        #endregion

        #region >>operator<<
        public static Matrix operator +(Matrix a, Matrix b) //c = a + b
        {
            int col = Math.Max(a.ColSize, b.ColSize);
            int row = Math.Max(a.RowSize, b.RowSize);
            Matrix cMat = new Matrix(row, col);

            for (int y = 0; y < a.RowSize; y++)
            {
                for (int x = 0; x < a.ColSize; x++)
                {
                    cMat[y, x] += a[y, x];
                }
            }
            for (int y = 0; y < b.RowSize; y++)
            {
                for (int x = 0; x < b.ColSize; x++)
                {
                    cMat[y, x] += b[y, x];
                }
            }
            return new Matrix(cMat);
        }

        public static Matrix operator -(Matrix a, Matrix b) //c = a - b
        {
            int col = Math.Max(a.ColSize, b.ColSize);
            int row = Math.Max(a.RowSize, b.RowSize);
            Matrix cMat = new Matrix(row, col);

            for (int y = 0; y < a.RowSize; y++)
            {
                for (int x = 0; x < a.ColSize; x++)
                {
                    cMat[y, x] += a[y, x];
                }
            }
            for (int y = 0; y < b.RowSize; y++)
            {
                for (int x = 0; x < b.ColSize; x++)
                {
                    cMat[y, x] -= b[y, x]; // Subtraction
                }
            }
            return new Matrix(cMat);
        }

        public static Matrix operator *(Matrix a, Matrix b) //c = a cross b
        {
            if (a.ColSize == b.RowSize)
            {
                int col = b.ColSize;
                int row = a.RowSize;
                int length = a.ColSize;
                Matrix cMat = new Matrix(row, col);

                for (int y = 0; y < a.RowSize; y++)
                {
                    for (int x = 0; x < b.ColSize; x++)
                    {
                        cMat[y, x] = 0;
                        for (int i = 0; i < length; ++i)
                        {
                            cMat[y, x] += a[y, i] * b[i, x];
                        }
                    }
                }

                return cMat;
            }
            return null;
        }

        public static Matrix operator *(Matrix a, double b) //c = a * b(const)
        {
            int col = a.ColSize;
            int row = a.RowSize;
            int length = a.ColSize;
            Matrix cMat = new Matrix(row, col);

            for (int y = 0; y < row; y++)
            {
                for (int x = 0; x < col; x++)
                {
                    cMat[y, x] = a[y, x] * b;
                }
            }

            return cMat;
        }

        public static Matrix operator *(double a, Matrix b) //c = a(const) * b)
        {
            return b * a;
        }
        #endregion



    }
}
