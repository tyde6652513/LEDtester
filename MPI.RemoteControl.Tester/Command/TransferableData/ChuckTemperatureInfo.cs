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
    public class ChuckTemperatureInfo : TransferableCommonObjectBase
    {
        private static XmlDocument s_document = new XmlDocument();
		private static XmlSerializer s_serializer = null;

        private double _chuckTemp;

        public ChuckTemperatureInfo()
			: base()
		{
			if ( s_serializer == null )
				s_serializer = new XmlSerializer( this.GetType() );

			this.Initialize();
		}

		private void Initialize()
		{
            _chuckTemp = 25;
		}


        [XmlElement]
        public double ChuckTemperature
        {
            get { return _chuckTemp; }
            set { _chuckTemp = value; }
        }

        public ChuckTemperatureInfo Clone()
        {
            ChuckTemperatureInfo data = new ChuckTemperatureInfo();

            data.ChuckTemperature = ChuckTemperature;

            return data;
        }

        protected override bool DeserializeCommonObject(string context)
        {
            using (StringReader sr = new StringReader(context))
            {
                try
                {
                    ChuckTemperatureInfo data = s_serializer.Deserialize(sr) as ChuckTemperatureInfo;
                    if (data != null)
                    {
                        Initialize();

                        this.ChuckTemperature = data.ChuckTemperature;


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
                return (int)ETransferableCommonObject.ChuckTemperatureInfo;
            }
        }

    }
}
