using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Maths
{
    public static class SavitzkyGolayFilterCoefficientGenerator
    {
        public static double[] CoefficientGenerator(uint ML, uint MR, int degree, int nthDerivative, double xInterval, out int errorCode)
        {
            // ML = left window size
            // MR = right window size
            // degree = polynomial order
            // derivative 微分次數

            // notice: the number of coefficient = ML+MR+1

            // error code
            // 0: the coefficient calculation succeeded
            // -1: the number of sample points is not enough
            // -2: the nth derivative is larger than the degree
            // -3: at least one of the parameters (ML, MR, degree, nthDerivative, xInterval) is negative
            // -4: the matrix transpose operation error
            // -5: the matrix multiplication error
            // -6: the inverse matrix error

            // variable declaration
            int N;
            int coefficientNum;
            int factorial;
            double scaleFactor;
            double[] coefficients;
            double[][] VandermondeMatrix;
            double[][] VandermondeMatrixTranspose;
            double[][] ATA;
            double[][] ATAInverse;
            double[][] coefficientMatrix;

            if (degree > (ML + MR + 1))
            {
                errorCode = -1;

                coefficients = new double[0];

                return coefficients;
            }

            if (nthDerivative > degree)
            {
                errorCode = -2;

                coefficients = new double[0];

                return coefficients;
            }

            if ((ML < 0) || (MR < 0) || (degree < 0) || (nthDerivative < 0) || (xInterval < 0))
            {
                errorCode = -3;

                coefficients = new double[0];

                return coefficients;
            }

            coefficientNum = (int)(ML + MR + 1);

            N = degree;

            VandermondeMatrix = new double[ML + MR + 1][];

            for (int i = 0; i < ML + MR + 1; i++)
            {
                VandermondeMatrix[i] = new double[N + 1];
            }

            for (int i = 0; i < (ML + MR + 1); i++)
            {
                for (int j = 0; j < N + 1; j++)
                {
                    VandermondeMatrix[i][j] = Math.Pow((i - ML), j);
                }
            }

            VandermondeMatrixTranspose = MatrixTranspose(VandermondeMatrix);

            ATA = MatrixMultiplication(VandermondeMatrixTranspose, VandermondeMatrix, out errorCode);

            if (errorCode != 0)
            {
                errorCode = -5;

                coefficients = new double[0];

                return coefficients;
            }

            ATAInverse = GaussJordanElimination(ATA, out errorCode);

            if (errorCode != 0)
            {
                errorCode = -6;

                coefficients = new double[0];

                return coefficients;
            }

            coefficientMatrix = MatrixMultiplication(ATAInverse, VandermondeMatrixTranspose, out errorCode);

            if (errorCode != 0)
            {
                errorCode = -5;

                coefficients = new double[0];

                return coefficients;
            }

            coefficients = new double[coefficientNum];

            factorial = 1; // initialization

            for (int i = nthDerivative; i >= 1; i--) // if nthDerivative = 0, factorial = 1
            {
                factorial *= i;
            }

            scaleFactor = 0;

            scaleFactor = Math.Pow(xInterval, nthDerivative);

            for (int i = 0; i < coefficientNum; i++)
            {
                coefficients[i] = coefficientMatrix[nthDerivative][i] * factorial / scaleFactor;
            }

            errorCode = 0;

            return coefficients;
        }

        private static double[][] MatrixTranspose(double[][] input)
        {
            double[][] result = new double[input[0].Length][];

            for (int i = 0; i < input[0].Length; i++)
            {
                result[i] = new double[input.Length];
            }

            for (int i = 0; i < input[0].Length; i++)
            {
                for (int j = 0; j < input.Length; j++)
                {
                    result[i][j] = input[j][i];
                }
            }

            return result;
        }

        private static double[][] MatrixMultiplication(double[][] matrixA, double[][] matrixB, out int errorCode)
        {
            // error code
            // 0: matrix multiplication succeeded
            // -1: the matrix dimensions are not consistent with the matrix multiplication opration

            double[][] result;

            if (matrixA[0].Length != matrixB.Length)
            {
                errorCode = -1;

                result = new double[0][];

                return result;
            }

            result = new double[matrixA.Length][];

            for (int i = 0; i < matrixA.Length; i++)
            {
                result[i] = new double[matrixB[0].Length];
            }

            // matrix multiplication
            for (int i = 0; i < result.Length; i++)
            {
                for (int j = 0; j < result[0].Length; j++)
                {
                    result[i][j] = 0;

                    for (int k = 0; k < matrixA[0].Length; k++)
                    {
                        result[i][j] += matrixA[i][k] * matrixB[k][j];
                    }
                }
            }

            errorCode = 0;

            return result;
        }

        private static double[][] GaussJordanElimination(double[][] inputMatrix, out int errorCode)
        {
            ///////////////////////////////////////////////////////////////////////////////////////////////
            // --------------------------------------------------------------------------------------------
            // return value definition
            // 0: succeeded, the solution is found
            // -1: argument error
            // -2: Special case: there is no solution or there are infinitely many solutions
            // --------------------------------------------------------------------------------------------
            ///////////////////////////////////////////////////////////////////////////////////////////////

            ///////////////////////////////////////////////////////////////////////////////////////////////
            // --------------------------------------------------------------------------------------------
            // variable declaration
            int rowNum;
            int columnNum;

            int inputColumnEnd;
            int rhsColumnStart;
            int rhsColumnEnd;
            int isSpecialCase;
            double pivot;
            int zeroPrecisionDigits;
            double zeroPrecision;
            int rowIndex;
            int columnIndex;

            int tempColumnIndex;
            int tempRowIndex;
            double rowRatio;
            int rowCount;
            double pivotRatio;
            int pivotIndex;

            // --------------------------------------------------------------------------------------------
            double[][] result;

            double[][] matrixAugmented;

            double[] tempRow;

            ///////////////////////////////////////////////////////////////////////////////////////////////
            // --------------------------------------------------------------------------------------------
            #region // Collapse Region >>> create the augmented matrix <<<

            rowNum = inputMatrix.Length;

            columnNum = inputMatrix[0].Length * 2;

            inputColumnEnd = inputMatrix[0].Length - 1;

            rhsColumnStart = inputMatrix[0].Length;

            rhsColumnEnd = columnNum - 1;

            matrixAugmented = new double[rowNum][];

            for (int i = 0; i < rowNum; i++)
            {
                matrixAugmented[i] = new double[columnNum];
            }

            for (int i = 0; i < rowNum; i++)
            {
                for (int j = 0; j < columnNum; j++)
                {
                    if (j <= inputColumnEnd)
                    {
                        matrixAugmented[i][j] = inputMatrix[i][j];
                    }
                    else
                    {
                        if (i == (j - rhsColumnStart))
                        {
                            matrixAugmented[i][j] = 1;
                        }
                        else
                        {
                            matrixAugmented[i][j] = 0;
                        }
                    }
                }
            }

            #endregion
            // --------------------------------------------------------------------------------------------
            ///////////////////////////////////////////////////////////////////////////////////////////////

            ///////////////////////////////////////////////////////////////////////////////////////////////
            // --------------------------------------------------------------------------------------------
            #region // Collapse Region >>> forward elimination <<<

            isSpecialCase = 0; // used as a boolean variable, 0: not the special case, 1: special case

            pivot = 0;

            zeroPrecisionDigits = 14;

            zeroPrecision = Math.Pow(0.1, zeroPrecisionDigits);

            rowIndex = 0;

            columnIndex = 0;

            tempColumnIndex = 0;

            tempRowIndex = 0;

            for (rowIndex = 0; rowIndex < rowNum; rowIndex++) // rowIndex = the corresponding pivot index
            {
                pivot = matrixAugmented[rowIndex][rowIndex];

                // -------------------------- check the non-zero pivot first ----------------------------------
                if (Math.Abs(pivot) <= zeroPrecision)
                {
                    // we need to switch the current row with a row below the current row which has a non-zero pivot
                    // we can scan the rows below the current row until we find a row with a non-zero pivot

                    tempRow = matrixAugmented[rowIndex];

                    for (tempRowIndex = rowIndex + 1; tempRowIndex < rowNum; tempRowIndex++)
                    {
                        pivot = matrixAugmented[tempRowIndex][rowIndex];

                        if (Math.Abs(pivot) > zeroPrecision)
                        {
                            // swith the rows
                            matrixAugmented[rowIndex] = matrixAugmented[tempRowIndex];

                            matrixAugmented[tempRowIndex] = tempRow;

                            break;
                        }
                    }

                    if (Math.Abs(pivot) <= zeroPrecision)
                    {
                        isSpecialCase = 1;

                        break; // out of the main loop
                    }
                }
                // --------------------------------------------------------------------------------------------

                // ------------------------------- elimination process ----------------------------------------
                for (int i = rowIndex + 1; i < rowNum; i++) // here, i index = the eliminated row-index
                {
                    rowRatio = matrixAugmented[i][rowIndex] / pivot;

                    matrixAugmented[i][rowIndex] = 0;

                    for (columnIndex = rowIndex + 1; columnIndex < columnNum; columnIndex++)
                    {
                        matrixAugmented[i][columnIndex] = matrixAugmented[i][columnIndex] - rowRatio * matrixAugmented[rowIndex][columnIndex];
                    }
                }
                // --------------------------------------------------------------------------------------------
            }

            // --------------------------- normalize each row with the corresponding pivot ---------------------------
            if (isSpecialCase == 0)
            {
                double normalization = 0;

                pivotIndex = 0;

                for (pivotIndex = 0; pivotIndex < rowNum; pivotIndex++)
                {
                    normalization = matrixAugmented[pivotIndex][pivotIndex];

                    for (columnIndex = pivotIndex; columnIndex < columnNum; columnIndex++)
                    {
                        matrixAugmented[pivotIndex][columnIndex] = matrixAugmented[pivotIndex][columnIndex] / normalization;
                    }
                }
            }
            else
            {
                errorCode = -2;

                result = new double[0][];

                return result;
            }
            // -------------------------------------------------------------------------------------------------------

            #endregion
            // --------------------------------------------------------------------------------------------
            ///////////////////////////////////////////////////////////////////////////////////////////////

            ///////////////////////////////////////////////////////////////////////////////////////////////
            // --------------------------------------------------------------------------------------------
            #region // Collapse Region >>> back substitution <<<

            pivotIndex = rowNum - 1; // initialization

            for (rowCount = rowNum - 1; rowCount >= 1; rowCount--)
            {
                for (int i = 0; i < rowCount; i++)
                {
                    pivotRatio = matrixAugmented[i][pivotIndex]; // assume the pivot is normalized to 1

                    matrixAugmented[i][pivotIndex] = 0;

                    for (int j = rhsColumnStart; j <= rhsColumnEnd; j++)
                    {
                        matrixAugmented[i][j] = matrixAugmented[i][j] - pivotRatio * matrixAugmented[pivotIndex][j];
                    }
                }

                pivotIndex--;
            }

            #endregion
            // --------------------------------------------------------------------------------------------
            ///////////////////////////////////////////////////////////////////////////////////////////////

            ///////////////////////////////////////////////////////////////////////////////////////////////
            // --------------------------------------------------------------------------------------------
            #region // Collapse Region >>> copy the solution from the augmented matrix to the result matrix <<<

            result = new double[rowNum][];

            for (int i = 0; i < rowNum; i++)
            {
                result[i] = new double[columnNum / 2];

                for (int j = rhsColumnStart; j <= rhsColumnEnd; j++)
                {
                    result[i][j - rhsColumnStart] = matrixAugmented[i][j];
                }
            }

            #endregion
            // --------------------------------------------------------------------------------------------
            ///////////////////////////////////////////////////////////////////////////////////////////////

            errorCode = 0;

            return result;
        }
    }

    public class SavitzkyGolayFilter2
    {
        public SavitzkyGolayFilter2(uint ML, uint MR, int order, int derivative = 0, double xInterval = 1)
        {
            this.GenerateSGCoef(ML, MR, order, derivative, xInterval);
        }

        public SavitzkyGolayFilter2(uint window, int order, int derivative = 0, double xInterval = 1)
        {
            this.GenerateSGCoef(window, order, derivative, xInterval);
        }

        private bool _isCoefGenerated = false;

        private int _errorCode = 0;

        private int _ML = 0;

        private int _MR = 0;

        private double _XInterval = 1;

        private double[] _SGCoef = null;

        public bool IsCoefGenerated
        {
            get { return _isCoefGenerated; }
        }

        public int ErrorCode
        {
            get { return _errorCode; }
        }

        public void GenerateSGCoef(uint ML, uint MR, int order, int derivative = 0, double xInterval = 1)
        {
            _ML = (int)ML;

            _MR = (int)MR;

            _XInterval = xInterval;

            _SGCoef = SavitzkyGolayFilterCoefficientGenerator.CoefficientGenerator(ML, MR, order, derivative, _XInterval, out _errorCode);

            if (_errorCode != 0)
            {
                _isCoefGenerated = false;
            }
            else
            {
                _isCoefGenerated = true;
            }
        }

        public void GenerateSGCoef(uint window, int order, int derivative = 0, double xInterval = 1)
        {
            if (window % 2 == 0)
            {
                _isCoefGenerated = false;

                return;
            }

            uint halfWindow;

            halfWindow = (window - 1) / 2;

            _ML = (int)halfWindow;

            _MR = (int)halfWindow;

            _XInterval = xInterval;

            _SGCoef = SavitzkyGolayFilterCoefficientGenerator.CoefficientGenerator(halfWindow, halfWindow, order, derivative, _XInterval, out _errorCode);

            if (_errorCode != 0)
            {
                _isCoefGenerated = false;
            }
            else
            {
                _isCoefGenerated = true;
            }
        }

        public double[] Filter(double[] data)
        {
            if (_isCoefGenerated == false)
            {
                return data;
            }

            double[] tempSum = new double[data.Length];

            int window = _ML + _MR + 1;

            for (int i = _ML; i < (data.Length - _MR); i++)
            {

                for (int j = 0; j < window; j++)
                {
                    tempSum[i] += _SGCoef[j] * data[i - _ML + j];
                }

            }

            return tempSum;
        }

        public double[][] Filter2(double[][] data, int RowNo)
        {
            double[][] SGoutput = new double[data.Length][];

            for (int i = 0; i < data.Length; i++)
            {
                SGoutput[i] = new double[3];
            }

            if (_isCoefGenerated == false)
            {
                return data;
            }

            double[] tempSum = new double[data.Length];

            int window = _ML + _MR + 1;

            for (int i = _ML; i < (data.Length - _MR); i++)
            {

                for (int j = 0; j < window; j++)
                {
                    tempSum[i] += _SGCoef[j] * data[i - _ML + j][RowNo];
                }

            }

            for (int i = 0; i < data.Length; i++)
            {
                SGoutput[i][0] = data[i][0];

                SGoutput[i][1] = data[i][1];

                SGoutput[i][2] = tempSum[i];
            }

            return SGoutput;
        }
    }
}
