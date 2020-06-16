using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
    [Serializable]
    public class ESDHardwareInfo
    {
        private object _lockObj;

        private string _serialNumber;
        private EESDMechineType _hwMechineType;
        private EESDVersion _hwVersion;
        private int _hwNumber;

        private int _daRatio;
        private int _hvChargeDelayTime;

        private EESDCalibrationMode _caliMode;

        private double _hbmCaliGain01;
        private double _hbmCaliOffSet01;
        private double _hbmCaliGain02;
        private double _hbmCaliOffSet02;

        private double _mmCaliGain01;
        private double _mmCaliOffSet01;
        private double _mmCaliGain02;
        private double _mmCaliOffSet02;

        private ESDGainTable _anodeTable;
        private ESDGainTable _cathodeTable;

        private double _hbmMinVolt;
        private double _hbmMaxVolt;
        private double _mmMinVolt;
        private double _mmMaxVolt;

		private DateTime _date;
		private DateTime _date2;
		private DateTime _date3;
		private DateTime _date4;

		private string _zapBoxSN;
		private string _zapBoxSN2;
		private string _zapBoxSN3;
		private string _zapBoxSN4;

        private long _hbmRelayCount;
        private long _mmRelayCount;

        private long _hbmRelayCount2;
        private long _mmRelayCount2;

        private long _hbmRelayCount3;
        private long _mmRelayCount3;

        private long _hbmRelayCount4;
        private long _mmRelayCount4;

        private List<ESDInfoHistory> _history;

        public ESDHardwareInfo()
        {
            this._lockObj = new object();

            this._serialNumber = "";
            this._hwMechineType = EESDMechineType.ESD_2000;
            this._hwVersion = EESDVersion.Ver1;
            this._hwNumber = 0;

            this._daRatio = 1200;
            this._hvChargeDelayTime = 1000; //mS

            this._caliMode = EESDCalibrationMode.ByGain;

            this._hbmCaliGain01 = 1.0d;
            this._hbmCaliOffSet01 = 0.0d;
            this._hbmCaliGain02 = 1.0d;
            this._hbmCaliOffSet02 = 0.0d;

            this._mmCaliGain01 = 1.0d;
            this._mmCaliOffSet01 = 0.0d;
            this._mmCaliGain02 = 1.0d;
            this._mmCaliOffSet02 = 0.0d;

            this._hbmMinVolt = 0.0d;
            this._hbmMaxVolt = 4100.0d;
            this._mmMinVolt = 0.0d;
            this._mmMaxVolt = 1000.0d;

			this._date = DateTime.Now;
			this._date2 = DateTime.Now;
			this._date3 = DateTime.Now;
			this._date4 = DateTime.Now;

			this._zapBoxSN = string.Empty;
			this._zapBoxSN2 = string.Empty;
			this._zapBoxSN3 = string.Empty;
			this._zapBoxSN4 = string.Empty;

            this._hbmRelayCount = 0;
            this._mmRelayCount = 0;

            this._hbmRelayCount2 = 0;
            this._mmRelayCount2 = 0;

            this._hbmRelayCount3 = 0;
            this._mmRelayCount3 = 0;

            this._hbmRelayCount4 = 0;
            this._mmRelayCount4 = 0;

            this._history = new List<ESDInfoHistory>();
        }

        #region >>> Public Proberty <<<

        public string SerialNumber
        {
            get { return this._serialNumber; }
            set { lock (this._lockObj) { this._serialNumber = value; } }
        }

        public EESDMechineType HWMechineType
        {
            get { return this._hwMechineType; }
            set { lock (this._lockObj) { this._hwMechineType = value; } }
        }

        public EESDVersion HWVersion
        {
            get { return this._hwVersion; }
            set { lock (this._lockObj) { this._hwVersion = value; } }
        }

        public int HWNumber
        {
            get { return this._hwNumber; }
            set { lock (this._lockObj) { this._hwNumber = value; } }
        }

        public int DARatio
        {
            get { return this._daRatio; }
            set { lock (this._lockObj) { this._daRatio = value; } }
        }

        public int HVChargeDelayTime
        {
            get { return this._hvChargeDelayTime; }
            set { lock (this._lockObj) { this._hvChargeDelayTime = value; } }
        }

        public double HBMCaliGain01
        {
            get { return this._hbmCaliGain01; }
            set { lock (this._lockObj) { this._hbmCaliGain01 = value; } }
        }

        public double HBMCaliOffSet01
        {
            get { return this._hbmCaliOffSet01; }
            set { lock (this._lockObj) { this._hbmCaliOffSet01 = value; } }
        }

        public double HBMCaliGain02
        {
            get { return this._hbmCaliGain02; }
            set { lock (this._lockObj) { this._hbmCaliGain02 = value; } }
        }

        public double HBMCaliOffSet02
        {
            get { return this._hbmCaliOffSet02; }
            set { lock (this._lockObj) { this._hbmCaliOffSet02 = value; } }
        }

        public double MMCaliGain01
        {
            get { return this._mmCaliGain01; }
            set { lock (this._lockObj) { this._mmCaliGain01 = value; } }
        }

        public double MMCaliOffSet01
        {
            get { return this._mmCaliOffSet01; }
            set { lock (this._lockObj) { this._mmCaliOffSet01 = value; } }
        }

        public double MMCaliGain02
        {
            get { return this._mmCaliGain02; }
            set { lock (this._lockObj) { this._mmCaliGain02 = value; } }
        }

        public double MMCaliOffSet02
        {
            get { return this._mmCaliOffSet02; }
            set { lock (this._lockObj) { this._mmCaliOffSet02 = value; } }
        }

        public double HBMMinVolt
        {
            get { return this._hbmMinVolt; }
            set { lock (this._lockObj) { this._hbmMinVolt = value; } }
        }

        public double HBMMaxVolt
        {
            get { return this._hbmMaxVolt; }
            set { lock (this._lockObj) { this._hbmMaxVolt = value; } }
        }

        public double MMMinVolt
        {
            get { return this._mmMinVolt; }
            set { lock (this._lockObj) { this._mmMinVolt = value; } }
        }

        public double MMMaxVolt
        {
            get { return this._mmMaxVolt; }
            set { lock (this._lockObj) { this._mmMaxVolt = value; } }
        }

		public DateTime Date
		{
			get { return this._date; }
			set { lock (this._lockObj) { this._date = value; } }
		}

		public DateTime Date2
		{
			get { return this._date2; }
			set { lock (this._lockObj) { this._date2 = value; } }
		}

		public DateTime Date3
		{
			get { return this._date3; }
			set { lock (this._lockObj) { this._date3 = value; } }
		}

		public DateTime Date4
		{
			get { return this._date4; }
			set { lock (this._lockObj) { this._date4 = value; } }
		}

		public string ZapBoxSN
		{
			get { return this._zapBoxSN; }
			set { lock (this._lockObj) { this._zapBoxSN = value; } }
		}

		public string ZapBoxSN2
		{
			get { return this._zapBoxSN2; }
			set { lock (this._lockObj) { this._zapBoxSN2 = value; } }
		}

		public string ZapBoxSN3
		{
			get { return this._zapBoxSN3; }
			set { lock (this._lockObj) { this._zapBoxSN3 = value; } }
		}

		public string ZapBoxSN4
		{
			get { return this._zapBoxSN4; }
			set { lock (this._lockObj) { this._zapBoxSN4 = value; } }
		}

        public long HBMRelayCount
        {
            get { return this._hbmRelayCount; }
            set { lock (this._lockObj) { this._hbmRelayCount = value; } }
        }

        public long MMRelayCount
        {
            get { return this._mmRelayCount; }
            set { lock (this._lockObj) { this._mmRelayCount = value; } }
        }

        public long HBMRelayCount2
        {
            get { return this._hbmRelayCount2; }
            set { lock (this._lockObj) { this._hbmRelayCount2 = value; } }
        }

        public long MMRelayCount2
        {
            get { return this._mmRelayCount2; }
            set { lock (this._lockObj) { this._mmRelayCount2 = value; } }
        }

        public long HBMRelayCount3
        {
            get { return this._hbmRelayCount3; }
            set { lock (this._lockObj) { this._hbmRelayCount3 = value; } }
        }

        public long MMRelayCount3
        {
            get { return this._mmRelayCount3; }
            set { lock (this._lockObj) { this._mmRelayCount3 = value; } }
        }

        public long HBMRelayCount4
        {
            get { return this._hbmRelayCount4; }
            set { lock (this._lockObj) { this._hbmRelayCount4 = value; } }
        }

        public long MMRelayCount4
        {
            get { return this._mmRelayCount4; }
            set { lock (this._lockObj) { this._mmRelayCount4 = value; } }
        }

        public EESDCalibrationMode CaliMode
        {
            get { return this._caliMode; }
            set { lock (this._lockObj) { this._caliMode = value; } }
        }

        public ESDGainTable CaliAnode
        {
            get { return this._anodeTable; }
            set { lock (this._lockObj) { this._anodeTable = value; } }
        }

        public ESDGainTable CaliCathode
        {
            get { return this._cathodeTable; }
            set { lock (this._lockObj) { this._cathodeTable = value; } }
        }

        public List<ESDInfoHistory> History
        {
            get { return this._history; }
            set { lock (this._lockObj) { this._history = value; } }
        }

        #endregion

        public static bool Serialize(string FileName, object Obj)
        {
            try
            {
                if (System.IO.Path.GetExtension(FileName).ToLower() == ".xml")
                {
                    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Obj.GetType());
                    System.Xml.XmlTextWriter xmlTextWriter = new System.Xml.XmlTextWriter(FileName, Encoding.ASCII);
                    x.Serialize(xmlTextWriter, Obj);
                    xmlTextWriter.Close();
                }
                else if (System.IO.Path.GetExtension(FileName).ToLower() == ".dat")
                {
                    using (System.IO.FileStream fileStream = new System.IO.FileStream(FileName, System.IO.FileMode.Create))
                    {
                        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        binaryFormatter.Serialize(fileStream, Obj);

                        fileStream.Close();
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static T Deserialize<T>(string FileName)
        {
            System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();

            try
            {
                object obj = new object();

                if (System.IO.Path.GetExtension(FileName).ToLower() == ".xml")
                {
                    xdoc.Load(FileName);
                    System.Xml.XmlNodeReader reader = new System.Xml.XmlNodeReader(xdoc.DocumentElement);
                    System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    obj = ser.Deserialize(reader);
                }
                else if (System.IO.Path.GetExtension(FileName).ToLower() == ".dat")
                {
                    using (System.IO.FileStream fileStream = new System.IO.FileStream(FileName, System.IO.FileMode.Open))
                    {
                        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        obj = binaryFormatter.Deserialize(fileStream);

                        fileStream.Close();
                    }
                }

                return (T)obj;
            }
            catch
            {
                return default(T);
            }
        }
    }

    [Serializable]
    public class ESDInfoHistory
    {
        private object _lockObj;

        private DateTime _date;
		private DateTime _date2;
		private DateTime _date3;
		private DateTime _date4;

		private string _zapBoxSN;
		private string _zapBoxSN2;
		private string _zapBoxSN3;
		private string _zapBoxSN4;

        private long _hbmRelayCount;
        private long _mmRelayCount;

        private long _hbmRelayCount2;
        private long _mmRelayCount2;

        private long _hbmRelayCount3;
        private long _mmRelayCount3;

        private long _hbmRelayCount4;
        private long _mmRelayCount4;

        public ESDInfoHistory()
        {
            this._lockObj = new object();

            this._date = DateTime.Now;
			this._date2 = DateTime.Now;
			this._date3 = DateTime.Now;
			this._date4 = DateTime.Now;

			this._zapBoxSN = string.Empty;
			this._zapBoxSN2 = string.Empty;
			this._zapBoxSN3 = string.Empty;
			this._zapBoxSN4 = string.Empty;

            this._hbmRelayCount = 0;
            this._mmRelayCount = 0;
        }

        public ESDInfoHistory(long hbmCount, long mmCount) : this()
        {
            this._date = DateTime.Now;
			this._date2 = DateTime.Now;
			this._date3 = DateTime.Now;
			this._date4 = DateTime.Now;

			this._zapBoxSN = string.Empty;
			this._zapBoxSN2 = string.Empty;
			this._zapBoxSN3 = string.Empty;
			this._zapBoxSN4 = string.Empty;

            this._hbmRelayCount = hbmCount;
            this._mmRelayCount = mmCount;
        }

        public ESDInfoHistory(long hbmCount,  long mmCount, 
                              long hbmCount2, long mmCount2,
                              long hbmCount3, long mmCount3,
                              long hbmCount4, long mmCount4) : this()
        {
            this._date = DateTime.Now;
			this._date2 = DateTime.Now;
			this._date3 = DateTime.Now;
			this._date4 = DateTime.Now;

			this._zapBoxSN = string.Empty;
			this._zapBoxSN2 = string.Empty;
			this._zapBoxSN3 = string.Empty;
			this._zapBoxSN4 = string.Empty;

            this._hbmRelayCount = hbmCount;
            this._mmRelayCount = mmCount;

            this._hbmRelayCount2 = hbmCount2;
            this._mmRelayCount2 = mmCount2;

            this._hbmRelayCount3 = hbmCount3;
            this._mmRelayCount3 = mmCount3;

            this._hbmRelayCount4 = hbmCount4;
            this._mmRelayCount4 = mmCount4;
        }

        public DateTime Date
        {
            get { return this._date; }
            set { lock (this._lockObj) { this._date = value; } }
        }

		public DateTime Date2
		{
			get { return this._date2; }
			set { lock (this._lockObj) { this._date2 = value; } }
		}

		public DateTime Date3
		{
			get { return this._date3; }
			set { lock (this._lockObj) { this._date3 = value; } }
		}

		public DateTime Date4
		{
			get { return this._date4; }
			set { lock (this._lockObj) { this._date4 = value; } }
		}

		public string ZapBoxSN
		{
			get { return this._zapBoxSN; }
			set { lock (this._lockObj) { this._zapBoxSN = value; } }
		}

		public string ZapBoxSN2
		{
			get { return this._zapBoxSN2; }
			set { lock (this._lockObj) { this._zapBoxSN2 = value; } }
		}

		public string ZapBoxSN3
		{
			get { return this._zapBoxSN3; }
			set { lock (this._lockObj) { this._zapBoxSN3 = value; } }
		}

		public string ZapBoxSN4
		{
			get { return this._zapBoxSN4; }
			set { lock (this._lockObj) { this._zapBoxSN4 = value; } }
		}

        public long HBMRelayCount
        {
            get { return this._hbmRelayCount; }
            set { lock (this._lockObj) { this._hbmRelayCount = value; } }
        }

        public long MMRelayCount
        {
            get { return this._mmRelayCount; }
            set { lock (this._lockObj) { this._mmRelayCount = value; } }
        }

        public long HBMRelayCount2
        {
            get { return this._hbmRelayCount2; }
            set { lock (this._lockObj) { this._hbmRelayCount2 = value; } }
        }

        public long MMRelayCount2
        {
            get { return this._mmRelayCount2; }
            set { lock (this._lockObj) { this._mmRelayCount2 = value; } }
        }

        public long HBMRelayCount3
        {
            get { return this._hbmRelayCount3; }
            set { lock (this._lockObj) { this._hbmRelayCount3 = value; } }
        }

        public long MMRelayCount3
        {
            get { return this._mmRelayCount3; }
            set { lock (this._lockObj) { this._mmRelayCount3 = value; } }
        }

        public long HBMRelayCount4
        {
            get { return this._hbmRelayCount4; }
            set { lock (this._lockObj) { this._hbmRelayCount4 = value; } }
        }

        public long MMRelayCount4
        {
            get { return this._mmRelayCount4; }
            set { lock (this._lockObj) { this._mmRelayCount4 = value; } }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
