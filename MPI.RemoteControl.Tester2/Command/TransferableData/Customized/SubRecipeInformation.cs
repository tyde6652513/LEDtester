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
    public class SubRecipeInformation : TransferableCommonObjectBase
    {
        private static XmlDocument s_document = new XmlDocument();
        private static XmlSerializer s_serializer = null;

        private string _mainRecipeName;
        private string _subRecipeName;
        private int _subRecipeIndex;
        private double _temperature;
        private int _slotNumber;
		private string _laserID;

        public SubRecipeInformation()
            : base()
        {
            if (s_serializer == null)
                s_serializer = new XmlSerializer(this.GetType());

            this.InitializeData();
        }

        private void InitializeData()
        {
            _mainRecipeName = String.Empty;
            _subRecipeName = String.Empty;
            _subRecipeIndex = 0;
            _temperature = 0;
            _slotNumber = 0;
			_laserID = String.Empty;
        }

        [XmlIgnore]
        public string MainRecipeName
        {
            get { return _mainRecipeName; }
            set { _mainRecipeName = value; }
        }

        [XmlIgnore]
        public string SubRecipeName
        {
            get { return _subRecipeName; }
            set { _subRecipeName = value; }
        }

		[XmlIgnore]
		public string LaserID
		{
			get { return _laserID; }
			set { _laserID = value; }
		}

        [XmlElement("MainRecipeName")]
        public XmlCDataSection _xmlMainRecipeName
        {
            get { return s_document.CreateCDataSection(String.IsNullOrEmpty(_mainRecipeName) ? String.Empty : _mainRecipeName); }
            set { _mainRecipeName = (value == null) ? String.Empty : value.Data; }
        }

        [XmlElement("SubRecipeName")]
        public XmlCDataSection _xmlSubRecipeName
        {
            get { return s_document.CreateCDataSection(String.IsNullOrEmpty(_subRecipeName) ? String.Empty : _subRecipeName); }
            set { _subRecipeName = (value == null) ? String.Empty : value.Data; }
        }

		[XmlElement("LaserID")]
		public XmlCDataSection _xmlLaserID
		{
			get { return s_document.CreateCDataSection(String.IsNullOrEmpty(_laserID) ? String.Empty : _laserID); }
			set { _laserID = (value == null) ? String.Empty : value.Data; }
		}

        [XmlElement("SubRecipeIndex")]
        public int SubRecipeIndex
        {
            get { return _subRecipeIndex; }
            set { _subRecipeIndex = value; }
        }

        [XmlElement("Temperature")]
        public double Temperature
        {
            get { return _temperature; }
            set { _temperature = value; }
        }

        [XmlElement("SlotNumber")]
        public int SlotNumber
        {
            get { return _slotNumber; }
            set { _slotNumber = value; }
        }

        public SubRecipeInformation Clone()
        {
            SubRecipeInformation info = new SubRecipeInformation();

            return this.CopyTo(info) ? info : null;
        }

        public bool CopyTo(SubRecipeInformation info)
        {
            if (info == null) return false;

            info.MainRecipeName = this.MainRecipeName;
            info.SubRecipeName = this.SubRecipeName;
            info.SubRecipeIndex = this.SubRecipeIndex;
            info.Temperature = this.Temperature;
            info.SlotNumber = this.SlotNumber;
			info.LaserID = this.LaserID;

            return true;
        }

        public bool CopyFrom(SubRecipeInformation info)
        {
            if (info == null) return false;

            this.MainRecipeName = info.MainRecipeName;
            this.SubRecipeName = info.SubRecipeName;
            this.SubRecipeIndex = info.SubRecipeIndex;
            this.Temperature = info.Temperature;
            this.SlotNumber = info.SlotNumber;
			this.LaserID = info.LaserID;

            return true;
        }

        protected override bool DeserializeCommonObject(string context)
        {
            try
            {
                using (StringReader sr = new StringReader(context))
                {
                    SubRecipeInformation info = s_serializer.Deserialize(sr) as SubRecipeInformation;

                    if (info != null)
                    {
                        this.InitializeData();

                        this.CopyFrom(info);

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
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
            get { return (int)ETransferableCommonObject.SubRecipeInformation; }
        }
    }

}
