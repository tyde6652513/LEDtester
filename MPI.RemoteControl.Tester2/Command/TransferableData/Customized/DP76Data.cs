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
    public class DP76Data : TransferableCommonObjectBase
    {
        private static XmlDocument s_document = new XmlDocument();
        private static XmlSerializer s_serializer = null;

        [XmlIgnore]
        private string _productLine;
        [XmlIgnore]
        private string _caliFileLine;
        [XmlIgnore]
        private KeyNameTable _testResultList;
        [XmlIgnore]
        private ItemSpecTable _itemSpecTable;

        public DP76Data()
            : base()
        {
            if (s_serializer == null)
                s_serializer = new XmlSerializer(this.GetType());

            this.Initialize();
        }

        private void Initialize()
        {
            this._productLine = string.Empty;

            this._caliFileLine = string.Empty;

            this._testResultList = new KeyNameTable();

            this._itemSpecTable = new ItemSpecTable();
        }

        [XmlIgnore]
        public string ProductLine
        {
            get { return this._productLine; }
            set { this._productLine = value; }
        }

        [XmlElement("ProductLine")]
        public XmlCDataSection XmlProductLine
        {
            get { return s_document.CreateCDataSection(_productLine); }
            set { _productLine = (value == null) ? String.Empty : value.Data; }
        }

        [XmlIgnore]
        public string CaliFileLine
        {
            get { return this._caliFileLine; }
            set { this._caliFileLine = value; }
        }

        [XmlElement("CaliFileLine")]
        public XmlCDataSection XmlCaliFileLine
        {
            get { return s_document.CreateCDataSection(_caliFileLine); }
            set { _caliFileLine = (value == null) ? String.Empty : value.Data; }
        }

        [XmlElement]
        public KeyNameTable TestResultList
        {
            get { return this._testResultList; }
            set { this._testResultList = value; }
        }

        [XmlElement("ItemSpecTable")]
        public ItemSpecTable SpecTable
        {
            get { return this._itemSpecTable; }
            set { this._itemSpecTable = value; }
        }

        public DP76Data Clone()
        {
            DP76Data data = new DP76Data();

            data._productLine = this._productLine;

            data._caliFileLine = this._caliFileLine;

            this._testResultList.Reset();
            while (_testResultList.MoveNext())
            {
                data._testResultList.Add(_testResultList.CurrentPair.Key, _testResultList.CurrentPair.Value);
            }

            return data;
        }

        protected override bool DeserializeCommonObject(string context)
        {
            using (StringReader sr = new StringReader(context))
            {
                try
                {
                    DP76Data data = s_serializer.Deserialize(sr) as DP76Data;
                    if (data != null)
                    {
                        Initialize();

                        this._caliFileLine = data.CaliFileLine;
                        this._productLine = data.ProductLine;
                        this._testResultList = data.TestResultList;
                        this._itemSpecTable = data.SpecTable;
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
                return (int)ETransferableCommonObject.D76Data;
            }
        }
    }
}
