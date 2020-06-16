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
    public class PreOverloadTestInfo : TransferableCommonObjectBase
	{
		private static XmlDocument s_document = new XmlDocument();
		private static XmlSerializer s_serializer = null;

		[XmlIgnore]
		private RefPointsTable _refTable;
        [XmlIgnore]
        private RefPointsTable _p2tRefTable;
		[XmlIgnore]
		private EOverloadTestMode _testMode;

		public PreOverloadTestInfo()
			: base()
		{
			if ( s_serializer == null )
				s_serializer = new XmlSerializer( this.GetType() );

			this.Initialize();
		}

		private void Initialize()
		{
			_refTable = new RefPointsTable();
            _p2tRefTable = new RefPointsTable(); ;
			_testMode = EOverloadTestMode.Normal;
		}


		[XmlElement]
		public RefPointsTable RefTable
		{
			get { return this._refTable; }
			set { this._refTable = value; }
		}

        [XmlElement]
        public RefPointsTable Prober2TesterRefTable
        {
            get { return this._p2tRefTable; }
            set { this._p2tRefTable = value; }
        }

		[XmlElement]
		public EOverloadTestMode TestMode
		{
			get { return this._testMode; }
			set { this._testMode = value; }
		}

		public PreOverloadTestInfo Clone()
		{
			PreOverloadTestInfo data = new PreOverloadTestInfo();

			data.RefTable.Table.AddRange( this.RefTable.Table.ToArray() );//deep copy

            data.Prober2TesterRefTable.Table.AddRange(this.Prober2TesterRefTable.Table.ToArray());//deep copy
            

			data.TestMode = this.TestMode;

			return data;
		}

		protected override bool DeserializeCommonObject( string context )
		{
			using ( StringReader sr = new StringReader( context ) )
			{
				try
				{
					PreOverloadTestInfo data = s_serializer.Deserialize( sr ) as PreOverloadTestInfo;
					if ( data != null )
					{
						Initialize();

						this.TestMode = data.TestMode;

						this.RefTable.Table.AddRange( data.RefTable.Table.ToArray() );//deep copy

                        this.Prober2TesterRefTable.Table.AddRange(data.Prober2TesterRefTable.Table.ToArray());//deep copy

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
			using ( StringWriter sw = new StringWriter( sb ) )
			{
				s_serializer.Serialize( sw, this );

				return sw.ToString();
			}
		}

		protected override int ObjectIdentifiedID
		{
			get
			{
				return ( int ) ETransferableCommonObject.PreOverloadTestInfo;
			}
		}
	}

	public enum EOverloadTestMode
	{
		Normal = 1,
		Overload = 2,
	}
}
