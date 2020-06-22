using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace MPI.RemoteControl2.Tester.Mpi.Command.Base
{
    [Serializable]
    public class InstrumentInfoBase
    {
        protected static XmlDocument s_doc = new XmlDocument();

        public InstrumentInfoBase()
        {
        }
    }

    public enum SourceMeterManufactory
    {
        MPI = 0,
        Keithely,
    }

    public enum SpectrumMeterManufactory
    {
        MPI = 0,
        Ocean,
        IS,
    }

    [Serializable]
    public class SourceMeterInfo : InstrumentInfoBase
    {
        private SourceMeterManufactory _manufactory;
        private string _model;
        private string _serialNumber;
        private string _firmwareVersion;

        [XmlElement("Brand")]
        public SourceMeterManufactory Brand
        {
            get { return _manufactory; }
            set { _manufactory = value; }
        }

        [XmlIgnore]
        public string Model
        {
            get { return _model; }
            set { _model = value; }
        }

        [XmlElement("Model")]
        public XmlCDataSection XmlModel
        {
            get { return s_doc.CreateCDataSection(String.IsNullOrEmpty(_model) ? String.Empty : _model); }
            set { _model = (String.IsNullOrEmpty(value.Value)) ? String.Empty : value.Value; }
        }

        [XmlIgnore]
        public string SerialNumber
        {
            get { return _serialNumber; }
            set { _serialNumber = value; }
        }

        [XmlElement("SerialNumber")]
        public XmlCDataSection XmlSerialNumber
        {
            get { return s_doc.CreateCDataSection(String.IsNullOrEmpty(_serialNumber) ? String.Empty : _serialNumber); }
            set { _serialNumber = (String.IsNullOrEmpty(value.Value)) ? String.Empty : value.Value; }
        }

        [XmlIgnore]
        public string FirmwareVersion
        {
            get { return _firmwareVersion; }
            set { _firmwareVersion = value; }
        }

        [XmlElement("FirmwareVersion")]
        public XmlCDataSection XmlFirmwareVersion
        {
            get { return s_doc.CreateCDataSection(String.IsNullOrEmpty(_firmwareVersion) ? String.Empty : _firmwareVersion); }
            set { _firmwareVersion = (String.IsNullOrEmpty(value.Value)) ? String.Empty : value.Value; }
        }

        public SourceMeterInfo()
            : base()
        {
            this.Initialize();
        }

        private void Initialize()
        {
            _manufactory = SourceMeterManufactory.MPI;
            _model = String.Empty;
            _firmwareVersion = String.Empty;
            _serialNumber = String.Empty;
        }
    }

    [Serializable]
    public class SpectrumMeterInfo : InstrumentInfoBase
    {
        private SpectrumMeterManufactory _manufactory;
        private string _model;
        private string _serialNumber;


        [XmlElement("Brand")]
        public SpectrumMeterManufactory Brand
        {
            get { return _manufactory; }
            set { _manufactory = value; }
        }

        [XmlIgnore]
        public string Model
        {
            get { return _model; }
            set { _model = value; }
        }

        [XmlElement("Model")]
        public XmlCDataSection XmlModel
        {
            get { return s_doc.CreateCDataSection(String.IsNullOrEmpty(_model) ? String.Empty : _model); }
            set { _model = (String.IsNullOrEmpty(value.Value)) ? String.Empty : value.Value; }
        }

        [XmlIgnore]
        public string SerialNumber
        {
            get { return _serialNumber; }
            set { _serialNumber = value; }
        }

        [XmlElement("SerialNumber")]
        public XmlCDataSection XmlSerialNumber
        {
            get { return s_doc.CreateCDataSection(String.IsNullOrEmpty(_serialNumber) ? String.Empty : _serialNumber); }
            set { _serialNumber = (String.IsNullOrEmpty(value.Value)) ? String.Empty : value.Value; }
        }

        public SpectrumMeterInfo()
            : base()
        {
            this.Initialize();
        }

        private void Initialize()
        {
            _manufactory = SpectrumMeterManufactory.MPI;
            _model = String.Empty;
            _serialNumber = String.Empty;

        }
    }

    [Serializable]
    public class SourceMeterCollector : InstrumentInformationCollector<SourceMeterInfo>
    {
        public SourceMeterCollector()
            : base()
        {
        }
    }

    [Serializable]
    public class SpectrumMeterCollector : InstrumentInformationCollector<SpectrumMeterInfo>
    {
        public SpectrumMeterCollector()
            : base()
        {
        }
    }

    [Serializable]
    public class InstrumentInformationCollector<TInstrument>
        where TInstrument : class
    {
        private object _lock;
        private List<TInstrument> _instrumentInformations;

        [XmlElement]
        public List<TInstrument> Instruments
        {
            get { return _instrumentInformations; }
            set { _instrumentInformations = value; }
        }

        [XmlIgnore]
        public int Count
        {
            get { return (_instrumentInformations == null) ? 0 : _instrumentInformations.Count; }
        }

        public InstrumentInformationCollector()
        {
            _lock = new object();

            _instrumentInformations = new List<TInstrument>();
        }

        public void Add(TInstrument instrument)
        {
            if (_instrumentInformations == null)
                _instrumentInformations = new List<TInstrument>();

            _instrumentInformations.Add(instrument);
        }

        public int DoAction(Action<TInstrument> ActionForEachInstrument)
        {
            if (_instrumentInformations == null || _instrumentInformations.Count <= 0) return 0;

            List<TInstrument>.Enumerator itr = _instrumentInformations.GetEnumerator();

            int nDoActionCount = 0;

            while (itr.MoveNext())
            {
                ActionForEachInstrument((TInstrument)itr.Current);
                nDoActionCount++;
            }

            return nDoActionCount;
        }

        public void Clear()
        {
            if (_instrumentInformations == null)
                _instrumentInformations = new List<TInstrument>();

            _instrumentInformations.Clear();
        }

    }

    [Serializable]
    public class TesterInformation : TransferableCommonObjectBase
    {
        private static XmlDocument s_document = new XmlDocument();
        private static XmlSerializer s_serializer = null;

        private string _calibrationFileName;
        private SourceMeterCollector _sourcemeterCollector;
        private SpectrumMeterCollector _spectrumCollector;

        [XmlIgnore]
        public string CalibrationFileName
        {
            get { return _calibrationFileName; }
            set { _calibrationFileName = value; }
        }

        [XmlElement("CalibrationFileName")]
        public XmlCDataSection XmlCalibrationFileName
        {
            get { return s_document.CreateCDataSection(String.IsNullOrEmpty(_calibrationFileName) ? String.Empty : _calibrationFileName); }
            set { _calibrationFileName = (String.IsNullOrEmpty(value.Value)) ? String.Empty : value.Value; }
        }

        [XmlElement]
        public SourceMeterCollector SourceMeters
        {
            get { return _sourcemeterCollector; }
            set { _sourcemeterCollector = value; }
        }

        [XmlElement]
        public SpectrumMeterCollector SpetrumMeters
        {
            get { return _spectrumCollector; }
            set { _spectrumCollector = value; }
        }

        public TesterInformation()
            : base()
        {
            if (s_serializer == null)
                s_serializer = new XmlSerializer(this.GetType());

            this.InitializeData();
        }

        private void InitializeData()
        {
            this._sourcemeterCollector = new SourceMeterCollector();
            this._spectrumCollector = new SpectrumMeterCollector();
            _calibrationFileName = String.Empty;

        }

        public TesterInformation Clone()
        {
            TesterInformation info = new TesterInformation();

            return this.CopyTo(info) ? info : null;
        }

        public bool CopyTo(TesterInformation info)
        {
            if (info == null) return false;

            info.SourceMeters = this.SourceMeters;
            info.SpetrumMeters = this.SpetrumMeters;
            info.CalibrationFileName = this.CalibrationFileName;

            return true;
        }

        public bool CopyFrom(TesterInformation info)
        {
            if (info == null) return false;

            this.SourceMeters = info.SourceMeters;
            this.SpetrumMeters = info.SpetrumMeters;
            this.CalibrationFileName = info.CalibrationFileName;

            return true;
        }

        protected override bool DeserializeCommonObject(string context)
        {
            using (StringReader sr = new StringReader(context))
            {
                try
                {
                    TesterInformation data = s_serializer.Deserialize(sr) as TesterInformation;
                    if (data != null)
                    {
                        InitializeData();

                        this.CopyFrom(data);

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        protected override string SerializeCommonObject()
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                s_serializer.Serialize(sw, this);

                return sw.ToString();
            }
        }

        protected override int ObjectIdentifiedID
        {
            get
            {
                return (int)ETransferableCommonObject.TesterInformation;
            }
        }
    }
}
