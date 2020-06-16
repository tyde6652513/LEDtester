using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace MPI.RemoteControl.Tester
{
    [Serializable]
    public class MC300Data : TransferableCommonObjectBase
    {
        private static XmlDocument _document = new XmlDocument();
        private static XmlSerializer _serializer = new XmlSerializer(typeof(MC300Data));

		[XmlIgnore]
        private EDataTestItemTable _EDataTestItemTable;

        public MC300Data() : base()
		{
			Initialize();
		}

		private void Initialize()
		{
            _EDataTestItemTable = new EDataTestItemTable();
		}

		[XmlElement("TestSpecTable")]
        public EDataTestItemTable TestSpecTable
		{
            get { return this._EDataTestItemTable; }
            set { this._EDataTestItemTable = value; }
		}

		protected override bool DeserializeCommonObject(string context)
		{
			using (StringReader sr = new StringReader(context))
			{
				try
				{
                    MC300Data data = _serializer.Deserialize(sr) as MC300Data;
					if (data != null)
					{
						Initialize();

                        this._EDataTestItemTable = data._EDataTestItemTable;
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
				_serializer.Serialize(sw, this);

				return sw.ToString();
			}
		}

		protected override int ObjectIdentifiedID
		{
			get
			{
                return (int)ETransferableCommonObject.MC300Data;
			}
		}
    }
}
