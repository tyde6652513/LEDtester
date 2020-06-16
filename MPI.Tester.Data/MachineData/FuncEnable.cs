using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MPI.Tester.Data
{
	public class FuncEnable
	{
		private object _lockObj;

		private bool _isSimulator;

		private bool _isInstantGetData;
		private bool _isCheckFilterWheel;
		private bool _isHWTrigSpectrometer;
		private bool _isEnableBarcodePrint;
		private bool _isEnableMapShowGap;
        private bool _isEnableFastThyModule;
        private bool _isEnableSrcFirmwareCalcTHY;

        private bool _isAutoChangeToOPMode;
		private bool _isEnableProtectionModule;

		public FuncEnable()
		{
			this._lockObj = new object();

			this._isSimulator = true;

			this._isInstantGetData = true;
			this._isCheckFilterWheel = true;
			this._isHWTrigSpectrometer = false;

			this._isEnableBarcodePrint = false;

			this._isEnableMapShowGap = true;

            this._isEnableFastThyModule = false;

            this._isEnableSrcFirmwareCalcTHY = false;

            this._isAutoChangeToOPMode = false;

			this._isEnableProtectionModule = false;
		}
		//-----------------------------------------------------------------------
		// 3.2 Primitive datatypes
		// .....
		// 3.2.2 boolean
		// [Definition:]  boolean has the ·value space· required to support the mathematical concept of binary-valued logic: {true, false}. 
		// 3.2.2.1 Lexical representation
		// An instance of a datatype that is defined as ·boolean· can have the following legal literals {true, false, 1, 0}. 
		// 3.2.2.2 Canonical representation
		// The canonical representation for boolean is the set of literals {true, false}. 
		//
		// ref. http://www.w3.org/TR/xmlschema-2/#boolean
		//-----------------------------------------------------------------------

		public int EnableSimulator
		{
			get { return this._isSimulator ? 1 : 0; }
			set { lock (this._lockObj) { this._isSimulator = Convert.ToBoolean(value); } }
		}

		[XmlIgnore]
		public bool IsSimulator
		{
			get { return this._isSimulator; }
			set { lock (this._lockObj) { this._isSimulator = value; } }
		}	

		public int EnableInstantGetData
		{
			get { return this._isInstantGetData ? 1 : 0; }
			set { lock (this._lockObj) { this._isInstantGetData = Convert.ToBoolean(value); } }
		}

		[XmlIgnore]
		public bool IsInstantGetData
		{
			get { return this._isInstantGetData; }
			set { lock (this._lockObj) { this._isInstantGetData = value; } }
		}

		public int EnableCheckFilterWheel
		{
			get { return this._isCheckFilterWheel ? 1 : 0; }
			set { lock (this._lockObj) { this._isCheckFilterWheel = Convert.ToBoolean(value); } }
		}

		[XmlIgnore]
		public bool IsCheckFilterWheel
		{
			get { return this._isCheckFilterWheel; }
			set { lock (this._lockObj) { this._isCheckFilterWheel = value; } }
		}

		public int EnableHWTrigSpectrometer
		{
			get { return this._isHWTrigSpectrometer ? 1 : 0; }
			set { lock (this._lockObj) { this._isHWTrigSpectrometer = Convert.ToBoolean(value); } }
		}

		public int EnableBarcodePrint
		{
			get { return this._isEnableBarcodePrint ? 1 : 0; }
			set { lock (this._lockObj) { this._isEnableBarcodePrint = Convert.ToBoolean(value); } }
		}

		public int EnableMapShowGap
		{
			get { return this._isEnableMapShowGap ? 1 : 0; }
			set { lock (this._lockObj) { this._isEnableMapShowGap = Convert.ToBoolean(value); } }
		}

		[XmlIgnore]
		public bool IsBarcodePrint
		{
			get { return this._isEnableBarcodePrint; }
			set { lock (this._lockObj) { this._isEnableBarcodePrint = value; } }
		}

		[XmlIgnore]
		public bool IsEnableMapShowGap
		{
			get { return this._isEnableMapShowGap; }
			set { lock (this._lockObj) { this._isEnableMapShowGap = value; } }
		}

		[XmlIgnore]
		public bool IsHWTrigSpectrometer
		{
			get { return this._isHWTrigSpectrometer; }
			set { lock (this._lockObj) { this._isHWTrigSpectrometer = value; } }
		}

        public int EnableFastThyModule
        {
            get { return this._isEnableFastThyModule ? 1 : 0; }
            set { lock (this._lockObj) { this._isEnableFastThyModule = Convert.ToBoolean(value); } }
        }

        [XmlIgnore]
        public bool IsEnableFastThyModule
        {
            get { return this._isEnableFastThyModule; }
            set { lock (this._lockObj) { this._isEnableFastThyModule = value; } }
        }

        public bool IsEnableSrcFirmwareCalcTHY
        {
            get { return this._isEnableSrcFirmwareCalcTHY; }
            set { this._isEnableSrcFirmwareCalcTHY = value; }
        }

        public bool IsAutoChangeToOPMode
        {
            get { return this._isAutoChangeToOPMode; }
            set { lock (this._lockObj) { this._isAutoChangeToOPMode = value; } }
        }

		public bool IsEnableProtectionModule
		{
			get { return this._isEnableProtectionModule; }
			set { lock (this._lockObj) { this._isEnableProtectionModule = value; } }
		}
	}
}
