using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester;

namespace MPI.Tester.DeviceCommon
{
    [Serializable]
    public class ESDGainTable
    {
        private object _lockObj;

        private static int[] GAIN_TABLE_HBM_RANGE = new int[] { 100, 250, 500, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000 };
        private static int[] GAIN_TABLE_MM_RANGE = new int[] { 50, 100, 200, 300, 400, 500, 600, 700, 800, 900 };

        private ESDGainData[] _hbmTable;
        private ESDGainData[] _mmTable;
        
        public ESDGainTable()
        {
            this._lockObj = new object();

            this._hbmTable = new ESDGainData[GAIN_TABLE_HBM_RANGE.Length - 1];

            this._mmTable = new ESDGainData[GAIN_TABLE_MM_RANGE.Length - 1];

            for (int i = 0; i < this._hbmTable.Length; i++)
            {
                this._hbmTable[i] = new ESDGainData(GAIN_TABLE_HBM_RANGE[i], GAIN_TABLE_HBM_RANGE[i + 1] - 1);
            }

            for (int i = 0; i < this._mmTable.Length; i++)
            {
                this._mmTable[i] = new ESDGainData(GAIN_TABLE_MM_RANGE[i], GAIN_TABLE_MM_RANGE[i + 1] - 1);
            }
        }

        #region >>> Public Proberty <<<

        public ESDGainData[] HBM
        {
            get { return this._hbmTable; }
            set { lock (this._lockObj) { this._hbmTable = value; } }
        }

        public ESDGainData[] MM
        {
            get { return this._mmTable; }
            set { lock (this._lockObj) { this._mmTable = value; } }
        }

        #endregion

        #region >>> Public Method <<<

        //public bool LoadDataFromFile(string PathAndFileName)
        //{
        //    string[][] importData = null;




        //    try
        //    {
        //        // Import HBM Data
        //        int startIndex = 0;

        //        for (int row = 0; row < importData.Length; row++)
        //        {
        //            if (importData[row][0] == "[HBM]")
        //            {
        //                startIndex = row + 2;
        //                break;
        //            }
        //        }

        //        string[][] dataArray = new string[this._hbmTable.Length][];

        //        for (int i = 0; i < this._hbmTable.Length; i++)
        //        {
        //            dataArray[i] = importData[startIndex + i];
        //        }

        //        for (int i = 0; i < this._hbmTable.Length; i++)
        //        {
        //            string[] content = dataArray[i];
        //            double[] value = new double[content.Length];

        //            for (int k = 0; k < content.Length; k++)
        //            {
        //                double.TryParse(content[k], out value[k]);
        //            }

        //            this._hbmTable[i].LowerBoundary = (int)value[0];
        //            this._hbmTable[i].UpperBoundary = (int)value[1];
        //            this._hbmTable[i].Gain = value[2];
        //        }

        //        // Import MM Data
        //        startIndex = 0;

        //        for (int row = 0; row < importData.Length; row++)
        //        {
        //            if (importData[row][0] == "[MM]")
        //            {
        //                startIndex = row + 2;
        //                break;
        //            }
        //        }

        //        dataArray = new string[this._mmTable.Length][];

        //        for (int i = 0; i < this._mmTable.Length; i++)
        //        {
        //            dataArray[i] = importData[startIndex + i];
        //        }

        //        for (int i = 0; i < this._mmTable.Length; i++)
        //        {
        //            string[] content = dataArray[i];
        //            double[] value = new double[content.Length];

        //            for (int k = 0; k < content.Length; k++)
        //            {
        //                double.TryParse(content[k], out value[k]);
        //            }

        //            this._mmTable[i].LowerBoundary = (int)value[0];
        //            this._mmTable[i].UpperBoundary = (int)value[1];
        //            this._mmTable[i].Gain = value[2];
        //        }

        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public bool SaveDataToFile()
        //{
        //    return true;
        //}

        #endregion
    }

    [Serializable]
    public class ESDGainData
    {
        private object _lockObj;
        private int _minBoundary;
        private int _maxBoundary;
        private double _gain;

        public ESDGainData()
        {
            this._lockObj = new object();
        }

        public ESDGainData(int lowerBoundary, int upperBoundary)
        {
            this._lockObj = new object();

            this._gain = 1.0d;

            this._maxBoundary = upperBoundary;

            this._minBoundary = lowerBoundary;
        }

        #region >>> Public Proberty <<<

        public int UpperBoundary
        {
            get { return this._maxBoundary; }
            set { lock (this._lockObj) { this._maxBoundary = value; } }
        }

        public int LowerBoundary
        {
            get { return this._minBoundary; }
            set { lock (this._lockObj) { this._minBoundary = value; } }
        }

        public double Gain
        {
            get { return this._gain; }
            set { lock (this._lockObj) { this._gain = value; } }
        }

        #endregion
    }
}
