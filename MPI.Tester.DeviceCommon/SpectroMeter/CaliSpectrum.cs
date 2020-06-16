using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MPI.Tester.DeviceCommon
{
    [Serializable]
    public class SpectroCaliData
    {
		public const int DEFAULT_CCD_PIXEL_LENGTH = 2048;

		private object _lockObj;
        private string _seriesNum;
        private double[][] _caliSpectrum;
        private double[] _defaultDark;        
        private double[] _rawResponse;
        private double _mWCoeff;
        private double _mcdCoeff;
        private double _darkAvg;
        private string _caliDate;
        private double _lightPowerFactor;
        private double[] _baseCalibSpectrum;
        private int  _baseCalibIndex;

        public SpectroCaliData(int ccdPixelLength = DEFAULT_CCD_PIXEL_LENGTH)
        {
			this._lockObj = new object();
            this._seriesNum ="NONE";
            this._caliDate = "NONE";           
            this._caliSpectrum = new double[5][];

            for (int i = 0; i < 5; i++)
            {
                this._caliSpectrum[i] = new double[ccdPixelLength];
                for (int k = 0; k < ccdPixelLength; k++)
                {
                    this._caliSpectrum[i][k] = 0;
                }
            }
            this._defaultDark = new double[ccdPixelLength];
            this._rawResponse = new double[ccdPixelLength];
            this._mWCoeff = 1;
            this._mcdCoeff = 1;
            this._darkAvg = 1;
            this._lightPowerFactor = 1;
            this._baseCalibSpectrum = new double[ccdPixelLength];
            this._baseCalibIndex = 1;
        }

		#region >>> Public Property <<<

		public string SeriesNum
        {
            get { return this._seriesNum; }
            set { lock (this._lockObj) { this._seriesNum = value; } }
        }

        public string CalibrationDate
        {
            get { return this._caliDate; }
            set { lock (this._lockObj) { this._caliDate = value; } }
        }

        public double[][] CaliSpectrumArray
        {
            get { return this._caliSpectrum; }
            set { lock (this._lockObj) { this._caliSpectrum = value; } }
        }

        public double[] DefaultDarkArray
        {
            get { return this._defaultDark; }
            set { lock (this._lockObj) { this._defaultDark = value; } }
        }

        public double[] RawCCDResponse
        {
            get { return this._rawResponse; }
            set { lock (this._lockObj) { this._rawResponse = value; } }
        }

        public double DarkAvg
        {
            get { return this._darkAvg; }
            set { lock (this._lockObj) { this._darkAvg = value; } }
        }

        public double MWCoeff
        {
            get { return this._mWCoeff; }
            set { lock (this._lockObj) { this._mWCoeff = value; } }
        }
        public double MCDCoeff
        {
            get { return this._mcdCoeff; }
            set { lock (this._lockObj) { this._mcdCoeff = value; } }
        }

        public double LightPowerFactor
        {
            get { return this._lightPowerFactor; }
            set { lock (this._lockObj) { this._lightPowerFactor = value; } }
        }

        public double[] BaseCalibSpectrum
        {
            get { return this._baseCalibSpectrum; }
            set { lock (this._lockObj) { this._baseCalibSpectrum = value; } }
        }

        public int BaseCalibIndex
        {
            get { return this._baseCalibIndex; }
            set { lock (this._lockObj) { this._baseCalibIndex = value; } }
        }
		#endregion
	}
}
