using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.RemoteControl2.Tester.ConstDefinition
{
	public enum EMPIDS7600ConstDefinitionType
	{
		Normal = 0,
		T200,
		Weimin,
	}

	[Serializable]
	public class Configuration
	{
		private EMPIDS7600ConstDefinitionType _CmdConstDefinitionType;

		public Configuration()
		{
			this._CmdConstDefinitionType = EMPIDS7600ConstDefinitionType.Normal;
		}

		public EMPIDS7600ConstDefinitionType CmdConstDefinitionType
		{
			get { return this._CmdConstDefinitionType; }
			set { this._CmdConstDefinitionType = value; }
		}
	}

	public abstract class MPIDS7600ConstDefinitionBase
	{
		public MPIDS7600ConstDefinitionBase()
		{
		}

		public abstract Int32 MAX_DATA { get;}
		public abstract Int32 MAX_TEST_ITEM_LIST { get;}
		public abstract Int32 MAX_ITEM_NAME { get;}
		public abstract Int32 MAX_RESULT_DATA { get;}
		public abstract Int32 MAX_PRODUCT_NAME { get;}
		public abstract Int32 MAX_BIN_GRADE_LIST { get;}
		public abstract Int32 MAX_BIN_GRADE_NAME { get;}
		public abstract Int32 MAX_INFO { get;}
		public abstract Int32 MAX_INFO_LIST { get;}
		public abstract Int32 MAX_LIST_COUNT { get;}
		public abstract Int32 MAX_DATA_LENGTH { get;}
		public abstract Int32 MAX_PROBE_DIRECTION_COUNT { get;}
		public abstract EMPIDS7600ConstDefinitionType ProtocolType { get; }
	}
}
