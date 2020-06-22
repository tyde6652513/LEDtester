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
    public class LaserBarProberInfo : TransferableCommonObjectBase
    {
        private static XmlDocument s_document = new XmlDocument();
        private static XmlSerializer s_serializer = null;

        private double _dTemperature;

        [XmlElement("ProbingTemperature")]
        public double Temperature
        {
            get { return _dTemperature; }
            set { _dTemperature = value; }
        }

        public LaserBarProberInfo()
            : base()
        {
            if (s_serializer == null)
                s_serializer = new XmlSerializer(this.GetType());

            this.InitializeData();
        }

        private void InitializeData()
        {
            _dTemperature = 0;
        }

        public LaserBarProberInfo Clone()
        {
            LaserBarProberInfo info = new LaserBarProberInfo();

            return this.CopyTo(info) ? info : null;
        }

        public bool CopyTo(LaserBarProberInfo info)
        {
            if (info == null) return false;

            info.Temperature = this.Temperature;

            return true;
        }

        public bool CopyFrom(LaserBarProberInfo info)
        {
            if (info == null) return false;

            this.Temperature = info.Temperature;

            return true;
        }

        protected override bool DeserializeCommonObject(string context)
        {
            using (StringReader sr = new StringReader(context))
            {
                try
                {
                    LaserBarProberInfo data = s_serializer.Deserialize(sr) as LaserBarProberInfo;
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
                return (int)ETransferableCommonObject.LaserBarProberInfo;
            }
        }
    }
}
