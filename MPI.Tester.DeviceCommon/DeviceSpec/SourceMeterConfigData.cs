using System;

namespace MPI.Tester.DeviceCommon
{
    public class SourceMeterConfigData : ICloneable
    {
        private object _lockObj;			                        

        private int _ID;
        private string _model;
		private string _venderID;
        private string _commType;
		private int _pmuCount;

        private RangeSettingData[][] _forceVoltRangeArray;          // [ PMU_Number ][ Range_Setting ]
        private RangeSettingData[][] _forceCurrentRangeArray;

        private RangeSettingData[][] _msrtVoltRangeArray;
        private RangeSettingData[][] _msrtCurrentRangeArray;

        /// <summary>
        /// Constructor
        /// </summary>
        public SourceMeterConfigData()
        {
            this._lockObj = new object();

            this._ID = 0;
			this._venderID = "W01";
            this._model = "LDT1A";
            this._commType = "PCI";

			this._pmuCount = 2;

            this._forceVoltRangeArray = new RangeSettingData[2][] 
			{   
				new RangeSettingData[] { new RangeSettingData() },
                new RangeSettingData[3] { new RangeSettingData() , new RangeSettingData() ,new RangeSettingData() }
			};

            this._forceCurrentRangeArray = new RangeSettingData[][]
            {
				new RangeSettingData[] { new RangeSettingData() }
             };
          
        }

        #region >>> Public Property <<<

        public int ID
        {
            get { return this._ID; }
            set { lock (this._lockObj) { this._ID = value; } }        
        }

        public string Model
        {
            get { return this._model; }
            set { lock (this._lockObj) { this._model = value; } }
        }

		public string VenderID
		{
			get { return this._venderID; }
			set { lock (this._lockObj) { this._venderID = value; } }
		}

        public string CommType
        {
            get { return this._commType; }
            set { lock (this._lockObj) { this._commType = value; } }        
        }

		public int PMUCount
		{
			get { return this._pmuCount; }
			set { lock (this._lockObj) { this._pmuCount = value; } }     
		}

        public RangeSettingData [][] ForceVoltRangeArray
        {
            get { return this._forceVoltRangeArray; }
            set { lock (this._lockObj) { this._forceVoltRangeArray = value; } }            
        }

        public RangeSettingData[][] ForceCurrentRangeArray
        {
            get { return this._forceCurrentRangeArray; }
            set { lock (this._lockObj) { this._forceCurrentRangeArray = value; } }
        }

        public RangeSettingData [][] MsrtVoltRangeArray
        {
            get { return this._msrtVoltRangeArray; }
            set { lock (this._lockObj) { this._msrtVoltRangeArray = value; } }
        }


        public RangeSettingData[][] MsrtCurrentRangeArray
        {
            get { return this._msrtCurrentRangeArray; }
            set { lock (this._lockObj) { this._msrtCurrentRangeArray = value; } }
        }

		//public int PMUCount
		//{
		//    // Gilbert 
		//    // All array have the same length for PMUCount. force / sencing , volt / current
		//    get { return this._forceCurrentRangeArray.Length; }
		//}

        #endregion

        #region >>> Public Method <<<

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
