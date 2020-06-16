using System;

namespace MPI.Tester.DeviceCommon
{
    public class DeviceSpec : ICloneable
    {
        private object _lockObj;

        private int _totalSoureMeterChannel;
    
		private string _version;
        private SourceMeterSpec[] _srcMeterSpecs;
        private SpectrometerSpec[]  _sptMeterSpecs;
		private ESDSpec[] _esdSpec;

        /// <summary>
        /// Constructor
        /// </summary>
		public DeviceSpec()
        {
            this._lockObj = new object();

            TotalSoureMeterChannel = 1;

			this._version = "V0.1";

			this._srcMeterSpecs = new SourceMeterSpec[2] { new SourceMeterSpec(), new SourceMeterSpec() };
			this._sptMeterSpecs = new SpectrometerSpec[2] { new SpectrometerSpec(), new SpectrometerSpec() };
			this._esdSpec = new ESDSpec[2] { new ESDSpec(), new ESDSpec() };
        }

        #region >>> Public Property <<<

		private string Version
		{
			get { return this._version; }
			set { lock (this._lockObj) { this._version = value; } }  	
		}

        private int TotalSoureMeterChannel
        {
            get { return this._totalSoureMeterChannel; }
            set { lock (this._lockObj) { this._totalSoureMeterChannel = value; } }    
        }

		public SourceMeterSpec[] SourceMeters
        {
            get { return this._srcMeterSpecs; }
			set { lock (this._lockObj) { this._srcMeterSpecs = value; } }
        }

		public SpectrometerSpec[] Spectrometers
        {
            get { return this._sptMeterSpecs; }
			set { lock (this._lockObj) { this._sptMeterSpecs = value; } }
        }

		public ESDSpec[] ESDSpecs
		{
			get { return this._esdSpec; }
			set { lock (this._lockObj) { this._esdSpec = value; } }
		}

        #endregion

		#region >>> Public Method <<<

		public object Clone()
		{
			return this.MemberwiseClone();
		}

		#endregion

    }
}
