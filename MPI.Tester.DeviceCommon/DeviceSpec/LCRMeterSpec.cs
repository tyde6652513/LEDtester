using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace MPI.Tester.DeviceCommon
{
	[Serializable]
    public class LCRMeterSpec : ICloneable
	{
        bool _isProvideSignalLevelV;
        bool _isProvideSignalLevelI;
		private bool _isProvideDCBiasV;
		private bool _isProvideDCBiasI;

		private double _signalLevelVMin;
		private double _signalLevelVMax;
		private double _signalLevelIMin;
		private double _signalLevelIMax;
		
		private double _frequencyMin;
		private double _frequencyMax;
		private double _dcBiasVMin;
		private double _dcBiasVMax;
		private double _dcBiasIMin;
		private double _dcBiasIMax;
		private List<ELCRMsrtSpeed> _msrtSpeedList;
		private List<ELCRTestType> _testTypeList;
        private List<ELCRTestType> _caliTypeList;

        private List<int> _freqList;
        private List<string> _cableLList;

        //private List<LCRCaliData> _caliDataList;

        private uint _caliDataQty;

		public LCRMeterSpec()
		{
			this._isProvideSignalLevelV = true;
			this._isProvideSignalLevelI = false;
			this._isProvideDCBiasV = true;
			this._isProvideDCBiasI = false;

			this._signalLevelVMin = 0.05;
			this._signalLevelVMax = 2;
			this._signalLevelIMin = 0.01;
			this._signalLevelIMax = 0.02;
			this._frequencyMin = 20;
			this._frequencyMax = 1000000;
			this._dcBiasVMin = 0;
			this._dcBiasVMax = 2;
			this._dcBiasIMin = 0;
			this._dcBiasIMax = 0;
			this._msrtSpeedList = new List<ELCRMsrtSpeed>();
			this._testTypeList = new List<ELCRTestType>();
            this._caliTypeList = new List<ELCRTestType>();

            this._freqList = new List<int>();
            this._cableLList = new List<string>(); ;

            //this._caliDataList = new List<LCRCaliData> ();


            this._caliDataQty = 1;
		}

        #region >>>property<<<

		public bool IsProvideSignalLevelV
		{
			get { return this._isProvideSignalLevelV; }
			set { this._isProvideSignalLevelV = value; }
		}

		public bool IsProvideSignalLevelI
		{
			get { return this._isProvideSignalLevelI; }
			set { this._isProvideSignalLevelI = value; }
		}

		public bool IsProvideDCBiasV
		{
			get { return this._isProvideDCBiasV; }
			set { this._isProvideDCBiasV = value; }
		}

		public bool IsProvideDCBiasI
		{
			get { return this._isProvideDCBiasI; }
			set { this._isProvideDCBiasI = value; }
		}

		public double SignalLevelVMin
		{
			get { return this._signalLevelVMin; }
			set { this._signalLevelVMin = value; }
		}

		public double SignalLevelVMax
		{
			get { return this._signalLevelVMax; }
			set { this._signalLevelVMax = value; }
		}

		public double SignalLevelIMin
		{
			get { return this._signalLevelIMin; }
			set { this._signalLevelIMin = value; }
		}

		public double SignalLevelIMax
		{
			get { return this._signalLevelIMax; }
			set { this._signalLevelIMax = value; }
		}

		public double FrequencyMin
		{
			get { return this._frequencyMin; }
			set { this._frequencyMin = value; }
		}

		public double FrequencyMax
		{
			get { return this._frequencyMax; }
			set { this._frequencyMax = value; }
		}

		public double DCBiasVMin
		{
			get { return this._dcBiasVMin; }
			set { this._dcBiasVMin = value; }
		}

		public double DCBiasVMax
		{
			get { return this._dcBiasVMax; }
			set { this._dcBiasVMax = value; }
		}

		public double DCBiasIMin
		{
			get { return this._dcBiasIMin; }
			set { this._dcBiasIMin = value; }
		}

		public double DCBiasIMax
		{
			get { return this._dcBiasIMax; }
			set { this._dcBiasIMax = value; }
		}

		public List<ELCRMsrtSpeed> MsrtSpeedList
		{
			get { return this._msrtSpeedList; }
			set { this._msrtSpeedList = value; }
		}

		public List<ELCRTestType> TestTypeList
		{
			get { return this._testTypeList; }
			set { this._testTypeList = value; }
		}
        public List<ELCRTestType> CaliTypeList
		{
            get { return this._caliTypeList; }
            set { this._caliTypeList = value; }
		}
        

        public uint CaliDataQty
        {
            get { return this._caliDataQty; }
            set { this._caliDataQty = value; }
		}

        public List<int> FreqList
		{
			get { return this._freqList; }
			set { this._freqList = value; }
		}

        public List<string> CableLenList
		{
			get { return this._cableLList; }
			set { this._cableLList = value; }
		}

        //public List<LCRCaliData> CaliDataList
        //{
        //    get { return this._caliDataList; }
        //    set { this._caliDataList = value; }
        //}

        #endregion

		public object Clone()
		{
			LCRMeterSpec cloneObj = new LCRMeterSpec();

			cloneObj._isProvideSignalLevelV = this._isProvideSignalLevelV;

			cloneObj._isProvideSignalLevelI = this._isProvideSignalLevelI;

			cloneObj._isProvideDCBiasV = this._isProvideDCBiasV;

			cloneObj._isProvideDCBiasI = this._isProvideDCBiasI;

			cloneObj._signalLevelVMin = this._signalLevelVMin;

			cloneObj._signalLevelVMax = this._signalLevelVMax;

			cloneObj._signalLevelIMin = this._signalLevelIMin;

			cloneObj._signalLevelIMax = this._signalLevelIMax;

			cloneObj._frequencyMin = this._frequencyMin;

			cloneObj._frequencyMax = this._frequencyMax;

			cloneObj._dcBiasVMin = this._dcBiasVMin;

			cloneObj._dcBiasVMax = this._dcBiasVMax;

			cloneObj._dcBiasIMin = this._dcBiasIMin;

			cloneObj._dcBiasIMax = this._dcBiasIMax;

            cloneObj._caliDataQty = this._caliDataQty;

			foreach (var item in this._msrtSpeedList)
			{ 
				cloneObj._msrtSpeedList.Add(item);
			}

			foreach (var item in this._testTypeList)
			{
				cloneObj._testTypeList.Add(item);
			}

            foreach (var item in this._caliTypeList)
			{
                cloneObj._caliTypeList.Add(item);
			}

            foreach (var item in this._freqList)
            {
                cloneObj._freqList.Add(item);
            }

            foreach (var item in this._cableLList)
            {
                cloneObj._cableLList.Add(item);
            }

            

            //foreach (var item in this.CaliDataList)
            //{
            //    cloneObj.CaliDataList.Add(item);
            //}
            


			return cloneObj;
		}
	}
}
