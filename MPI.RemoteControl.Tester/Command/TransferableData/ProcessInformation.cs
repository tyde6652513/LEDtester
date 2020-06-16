using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace MPI.RemoteControl.Tester
{
    [Serializable]
    public class ProcessInformation : TransferableCommonObjectBase
    {
        private static XmlDocument s_document = new XmlDocument();
        private static XmlSerializer s_serializer = null;

        private string _lotID;
        private string _waferID;
        private string _cassetteID;
        private string _slotID;
        private string _proberRecipe;
        private string _testSpecification;
        private string _operator;
        private double _testTemperature;
        private string _station;
        private double _probingDistance;

        public ProcessInformation()
            : base()
        {
            if (s_serializer == null)
                s_serializer = new XmlSerializer(this.GetType());

            this.InitializeData();
        }

        private void InitializeData()
        {
            this._lotID = String.Empty;
            this._waferID = String.Empty;
            this._cassetteID = String.Empty;
            this._slotID = String.Empty;
            this._proberRecipe = String.Empty;
            this._testSpecification = String.Empty;
            this._operator = String.Empty;
            this._testTemperature = 0;
            this._station = String.Empty;
        }

        [XmlIgnore]
        public string LotID
        {
            get { return _lotID; }
            set { _lotID = value; }
        }

        [XmlIgnore]
        public string WaferID
        {
            get { return _waferID; }
            set { _waferID = value; }
        }

        [XmlIgnore]
        public string CassetteID
        {
            get { return _cassetteID; }
            set { _cassetteID = value; }
        }

        [XmlIgnore]
        public string SlotID
        {
            get { return _slotID; }
            set { _slotID = value; }
        }

        [XmlIgnore]
        public string ProberRecipe
        {
            get { return _proberRecipe; }
            set { _proberRecipe = value; }
        }

        [XmlIgnore]
        public string TestSpecification
        {
            get { return _testSpecification; }
            set { _testSpecification = value; }
        }

        [XmlIgnore]
        public string Operator
        {
            get { return _operator; }
            set { _operator = value; }
        }

        [XmlIgnore]
        public double TestTemperature
        {
            get { return _testTemperature; }
            set { _testTemperature = value; }
        }

        [XmlIgnore]
        public double ProbingDistance
        {
            get { return _probingDistance; }
            set { _probingDistance = value; }
        }

        [XmlIgnore]
        public string Station
        {
            get { return _station; }
            set { _station = value; }
        }

        [XmlElement("LotID")]
        public XmlCDataSection _xmlLotID
        {
            get { return s_document.CreateCDataSection(String.IsNullOrEmpty(this.LotID) ? String.Empty : this.LotID); }
            set { this.LotID = (value == null) ? String.Empty : value.Data; }
        }

        [XmlElement("WaferID")]
        public XmlCDataSection _xmlWaferID
        {
            get { return s_document.CreateCDataSection(String.IsNullOrEmpty(this.WaferID) ? String.Empty : this.WaferID); }
            set { this.WaferID = (value == null) ? String.Empty : value.Data; }
        }

        [XmlElement("CassetteID")]
        public XmlCDataSection _xmlCassetteID
        {
            get { return s_document.CreateCDataSection(String.IsNullOrEmpty(this.CassetteID) ? String.Empty : this.CassetteID); }
            set { this.CassetteID = (value == null) ? String.Empty : value.Data; }
        }

        [XmlElement("SlotID")]
        public XmlCDataSection _xmlSlotID
        {
            get { return s_document.CreateCDataSection(String.IsNullOrEmpty(this.SlotID) ? String.Empty : this.SlotID); }
            set { this.SlotID = (value == null) ? String.Empty : value.Data; }
        }

        [XmlElement("ProberRecipe")]
        public XmlCDataSection _xmlProberRecipe
        {
            get { return s_document.CreateCDataSection(String.IsNullOrEmpty(this.ProberRecipe) ? String.Empty : this.ProberRecipe); }
            set { this.ProberRecipe = (value == null) ? String.Empty : value.Data; }
        }

        [XmlElement("TestSpecification")]
        public XmlCDataSection _xmlTestSpecification
        {
            get { return s_document.CreateCDataSection(String.IsNullOrEmpty(this.TestSpecification) ? String.Empty : this.TestSpecification); }
            set { this.TestSpecification = (value == null) ? String.Empty : value.Data; }
        }

        [XmlElement("Operator")]
        public XmlCDataSection _xmlOperator
        {
            get { return s_document.CreateCDataSection(String.IsNullOrEmpty(this.Operator) ? String.Empty : this.Operator); }
            set { this.Operator = (value == null) ? String.Empty : value.Data; }
        }

        [XmlElement("TestTemperature")]
        public XmlCDataSection _xmlTestTemperature
        {
            get { return s_document.CreateCDataSection(this.TestTemperature.ToString("0.0")); }
            set
            {
                double dValue = 0;
                
                if (Double.TryParse(value.Data, out dValue))
                    this.TestTemperature = dValue;
            }
        }
        [XmlElement("ProbingDistance")]
        public XmlCDataSection _xmlProbingDistance
        {
            get { return s_document.CreateCDataSection(this.ProbingDistance.ToString("0.000000")); }
            set
            {
                double dValue = 0;

                if (Double.TryParse(value.Data, out dValue))
                    this.ProbingDistance = dValue;
            }
        }
        [XmlElement("Station")]
        public XmlCDataSection _xmlStation
        {
            get { return s_document.CreateCDataSection(String.IsNullOrEmpty(this.Station) ? String.Empty : this.Station); }
            set { this.Station = (value == null) ? String.Empty : value.Data; }
        }

        public ProcessInformation Clone()
        {
            ProcessInformation info = new ProcessInformation();

            return this.CopyTo(info) ? info : null;
        }

        public bool CopyTo(ProcessInformation info)
        {
            if (info == null) return false;

            info.LotID = this.LotID;
            info.WaferID = this.WaferID;
            info.CassetteID = this.CassetteID;
            info.SlotID = this.SlotID;
            info.ProberRecipe = this.ProberRecipe;
            info.TestSpecification = this.TestSpecification;
            info.Operator = this.Operator;
            info.TestTemperature = this.TestTemperature;
            info.Station = this.Station;
            info.ProbingDistance = this.ProbingDistance;

            return true;
        }

        public bool CopyFrom(ProcessInformation info)
        {
            if (info == null) return false;

            this.LotID = info.LotID;
            this.WaferID = info.WaferID;
            this.CassetteID = info.CassetteID;
            this.SlotID = info.SlotID;
            this.ProberRecipe = info.ProberRecipe;
            this.TestSpecification = info.TestSpecification;
            this.Operator = info.Operator;
            this.TestTemperature = info.TestTemperature;
            this.Station = info.Station;
            this.ProbingDistance = info.ProbingDistance;

            return true;
        }

        protected override bool DeserializeCommonObject(string context)
        {
            using (StringReader sr = new StringReader(context))
            {
                try
                {
                    ProcessInformation data = s_serializer.Deserialize(sr) as ProcessInformation;
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
            using (StringWriter sw = new StringWriter(new StringBuilder()))
            {
                s_serializer.Serialize(sw, this);

                return sw.ToString();
            }
        }

        protected override int ObjectIdentifiedID
        {
            get
            {
                return (int)ETransferableCommonObject.ProcessInformation;
            }
        }
    }
}
