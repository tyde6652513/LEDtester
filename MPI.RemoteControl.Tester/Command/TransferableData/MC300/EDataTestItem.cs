using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MPI.RemoteControl.Tester
{
    [Serializable]
    public class EDataTestItem : TransferableCommonObjectBase
    {
        private static XmlDocument _xmlDocument = new XmlDocument();
		
        private string _Name;
        private string _Bin;
        private string _Alias;
        private string _Unit;
        private string _Bias;
        private string _Signum;
        private string _LowerSpec;
        private string _UpperSpec;
        private string _LowerValid;
        private string _UpperValid;
        private string _Offset;
        private string _Factor;

        public EDataTestItem()
		{
            _Name = String.Empty;
            _Bin = String.Empty;
            _Alias = String.Empty;
            _Unit = String.Empty;
            _Bias = String.Empty;
            _Signum = String.Empty;
            _LowerSpec = String.Empty;
            _UpperSpec = String.Empty;
            _LowerValid = String.Empty;
            _UpperValid = String.Empty;
            _Offset = String.Empty;
            _Factor = String.Empty;
		}

        public EDataTestItem(string name, string bin, string alias, string unit, string bias, string signum, string lowerSpec, string upperSpec)
		{
            _Name = name;
            _Bin = bin; 
            _Alias = alias;
            _Unit = unit;
            _Bias = bias;
            _Signum = signum;
            _LowerSpec = lowerSpec;
            _UpperSpec = upperSpec;
            _LowerValid = String.Empty;
            _UpperValid = String.Empty;
            _Offset = String.Empty;
            _Factor = String.Empty;
		}

        public EDataTestItem(string name, string bin, string alias, string unit, string bias, string signum, 
            string lowerSpec, string upperSpec, string lowerValid, string upperValid,
            string offset, string factor)
        {
            _Name = name;
            _Bin = bin;
            _Alias = alias;
            _Unit = unit;
            _Bias = bias;
            _Signum = signum;
            _LowerSpec = lowerSpec;
            _UpperSpec = upperSpec;
            _LowerValid = lowerValid;
            _UpperValid = upperValid;
            _Offset = offset;
            _Factor = factor;
        }

		[XmlIgnore]
		public string Name
		{
            get { return _Name; }
            set { _Name = value; }
		}

        [XmlIgnore]
        public string Bin
        {
            get { return _Bin; }
            set { _Bin = value; }
        }

        [XmlIgnore]
        public string Alias
        {
            get { return _Alias; }
            set { _Alias = value; }
        }

		[XmlIgnore]
        public string Unit
		{
            get { return _Unit; }
            set { _Unit = value; }
		}

		[XmlIgnore]
        public string Bias
		{
            get { return _Bias; }
            set { _Bias = value; }
		}

        [XmlIgnore]
        public string Signum
        {
            get { return _Signum; }
            set { _Signum = value; }
        }

        [XmlIgnore]
        public string LowerSpec
        {
            get { return _LowerSpec; }
            set { _LowerSpec = value; }
        }

        [XmlIgnore]
        public string UpperSpec
        {
            get { return _UpperSpec; }
            set { _UpperSpec = value; }
        }

        [XmlIgnore]
        public string LowerValid
        {
            get { return _LowerValid; }
            set { _LowerValid = value; }
        }

        [XmlIgnore]
        public string UpperValid
        {
            get { return _UpperValid; }
            set { _UpperValid = value; }
        }

        [XmlIgnore]
        public string Offset
        {
            get { return _Offset; }
            set { _Offset = value; }
        }

        [XmlIgnore]
        public string Factor
        {
            get { return _Factor; }
            set { _Factor = value; }
        }

		[XmlElement("Name")]
		public XmlCDataSection XmlItemNameSection
		{
            get { return _xmlDocument.CreateCDataSection(_Name); }
            set { _Name = (value == null) ? String.Empty : value.Data; }
		}

        [XmlElement("Bin")]
        public XmlCDataSection XmlItemBinSection
        {
            get { return _xmlDocument.CreateCDataSection(_Bin); }
            set { _Bin = (value == null) ? String.Empty : value.Data; }
        }

        [XmlElement("Alias")]
        public XmlCDataSection XmlItemAliasSection
        {
            get { return _xmlDocument.CreateCDataSection(_Alias); }
            set { _Alias = (value == null) ? String.Empty : value.Data; }
        }
	
        [XmlElement("Unit")]
        public XmlCDataSection XmlItemUnitSection
        {
            get { return _xmlDocument.CreateCDataSection(_Unit); }
            set { _Unit = (value == null) ? String.Empty : value.Data; }
        }

        [XmlElement("Bias")]
        public XmlCDataSection XmlItemBiasSection
        {
            get { return _xmlDocument.CreateCDataSection(_Bias); }
            set { _Bias = (value == null) ? String.Empty : value.Data; }
        }

        [XmlElement("Signum")]
        public XmlCDataSection XmlItemSignumSection
        {
            get { return _xmlDocument.CreateCDataSection(_Signum); }
            set { _Signum = (value == null) ? String.Empty : value.Data; }
        }

        [XmlElement("LowerSpec")]
        public XmlCDataSection XmlItemLowerSpecSection
        {
            get { return _xmlDocument.CreateCDataSection(_LowerSpec); }
            set { _LowerSpec = (value == null) ? String.Empty : value.Data; }
        }

        [XmlElement("UpperSpec")]
        public XmlCDataSection XmlItemUpperSpecSection
        {
            get { return _xmlDocument.CreateCDataSection(_UpperSpec); }
            set { _UpperSpec = (value == null) ? String.Empty : value.Data; }
        }

        [XmlElement("LowerValid")]
        public XmlCDataSection XmlItemLowerValidSection
        {
            get { return _xmlDocument.CreateCDataSection(_LowerValid); }
            set { _LowerValid = (value == null) ? String.Empty : value.Data; }
        }

        [XmlElement("UpperValid")]
        public XmlCDataSection XmlItemUpperValidSection
        {
            get { return _xmlDocument.CreateCDataSection(_UpperValid); }
            set { _UpperValid = (value == null) ? String.Empty : value.Data; }
        }

        [XmlElement("Offset")]
        public XmlCDataSection XmlItemOffsetSection
        {
            get { return _xmlDocument.CreateCDataSection(_Offset); }
            set { _Offset = (value == null) ? String.Empty : value.Data; }
        }

        [XmlElement("Factor")]
        public XmlCDataSection XmlItemFactorSection
        {
            get { return _xmlDocument.CreateCDataSection(_Factor); }
            set { _Factor = (value == null) ? String.Empty : value.Data; }
        }
    }
}
