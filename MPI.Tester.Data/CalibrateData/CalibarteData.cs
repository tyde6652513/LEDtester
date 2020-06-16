using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MPI.Tester.Data
{
    [Serializable]
    public class CalibarteData
    {
        private string _machineName;

        private string _sptSN;

        private  List<string> _gainOffsetKeyName;

        private  List<GainOffsetData> _gainOffsetValue;

        private List<LOPWLParameter> _LOPWLParameter;

        private GainOffsetData[][] _chuckLOPCorrectArray;

        private double[] _chuckResistanceCorrectArray;

        private double _resistance;

        private uint _productFilterWheelPos;

        private string _calibSpectrumFileName;

        private EPolarity _polarity;

        private uint _scanStartWL;

        private uint _scanEndWL;

        public CalibarteData()
        {
            this._machineName = string.Empty;

            this._sptSN = string.Empty;

            _gainOffsetKeyName = new List<string>();

            _gainOffsetValue = new List<GainOffsetData>();

            _LOPWLParameter = new List<LOPWLParameter>();

            _resistance = 0.0d;

            _productFilterWheelPos = 0;

            _chuckResistanceCorrectArray = new double[0];

            _calibSpectrumFileName = string.Empty;

            _polarity = EPolarity.Anode_P;

            _scanStartWL = 380;

            _scanEndWL = 780;
        }


        public string MachineName
        {
            get { return this._machineName; }
            set { this._machineName = value; }
        }

        public EPolarity Polarity
        {
            get { return this._polarity; }
            set { this._polarity = value; }
        }

        public uint ProductFilterWheelPos
		{
			get { return this._productFilterWheelPos; }
			set { this._productFilterWheelPos = value; } 
		}

        public string SpectrometerSN
        {
            get { return this._sptSN; }
            set { this._sptSN = value; }
        }

        public uint ScanStartWL
        {
            get { return this._scanStartWL; }
            set { this._scanStartWL = value;}
        }

        public uint ScanEndWL
        {
            get { return this._scanEndWL; }
            set { this._scanEndWL = value;}
        }

        public GainOffsetData[][] ChuckLOPCorrectArray
        {
            get { return this._chuckLOPCorrectArray; }
            set { this._chuckLOPCorrectArray = value; }
        }

        public double[] ChuckResistanceCorrectArray
        {
            get { return this._chuckResistanceCorrectArray; }
            set { this._chuckResistanceCorrectArray = value; }
        }

        public double Resistance
        {
            get { return this._resistance; }
            set { this._resistance = value; }
        }

        public List<string> GainOffsetKeyName
        {
            get{return this._gainOffsetKeyName;}
            set{this._gainOffsetKeyName=value;}
        }
   
        public List<GainOffsetData> GainOffsetValue
        {
            get{return this._gainOffsetValue;}
            set{this._gainOffsetValue=value;}
        }

        public List<LOPWLParameter> LOPWLParameters
        {
            get { return this._LOPWLParameter; }
            set { this._LOPWLParameter = value; }
        }

        public string CalibSpectrumFileName
        {
            get { return this._calibSpectrumFileName; }
            set { this._calibSpectrumFileName = value; }
        }
    }
}
