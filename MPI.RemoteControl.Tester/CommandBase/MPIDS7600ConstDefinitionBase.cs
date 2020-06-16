using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.RemoteControl.Tester.ConstDefinition
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
			this._CmdConstDefinitionType = EMPIDS7600ConstDefinitionType.T200;
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
		public abstract Int32 MAX_PROBE_DIRECTION_COUNT { get; }
	}

	public class MPIDS7600ConstDefinitionNormal : MPIDS7600ConstDefinitionBase
	{
		public MPIDS7600ConstDefinitionNormal()
			: base()
		{
		}

		private const Int32 m_nMAX_DATA = 2048;
		private const Int32 m_nMAX_TEST_ITEM_LIST = 500;
		private const Int32 m_nMAX_ITEM_NAME = 20;
		private const Int32 m_nMAX_RESULT_DATA = 20;
		private const Int32 m_nMAX_PRODUCT_NAME = 20;
		private const Int32 m_nMAX_BIN_GRADE_LIST = 200;
		private const Int32 m_nMAX_BIN_GRADE_NAME = 20;
		private const Int32 m_nMAX_INFO = 20;
		private const Int32 m_nMAX_INFO_LIST = 100;
		private const Int32 m_nMAX_LIST_COUNT = 200;
		private const Int32 m_nMAX_DATA_LENGTH = 20;
		private const Int32 m_nMAX_DIRECTION_COUNT = 0;



		public override int MAX_BIN_GRADE_LIST
		{
			get { return MPIDS7600ConstDefinitionNormal.m_nMAX_BIN_GRADE_LIST; }
		}

		public override int MAX_BIN_GRADE_NAME
		{
			get { return MPIDS7600ConstDefinitionNormal.m_nMAX_BIN_GRADE_NAME; }
		}

		public override int MAX_DATA
		{
			get { return MPIDS7600ConstDefinitionNormal.m_nMAX_DATA; }
		}

		public override int MAX_DATA_LENGTH
		{
			get { return MPIDS7600ConstDefinitionNormal.m_nMAX_DATA_LENGTH; }
		}

		public override int MAX_INFO
		{
			get { return MPIDS7600ConstDefinitionNormal.m_nMAX_INFO; }
		}

		public override int MAX_INFO_LIST
		{
			get { return MPIDS7600ConstDefinitionNormal.m_nMAX_INFO_LIST; }
		}

		public override int MAX_ITEM_NAME
		{
			get { return MPIDS7600ConstDefinitionNormal.m_nMAX_ITEM_NAME; }
		}

		public override int MAX_LIST_COUNT
		{
			get { return MPIDS7600ConstDefinitionNormal.m_nMAX_LIST_COUNT; }
		}

		public override int MAX_PRODUCT_NAME
		{
			get { return MPIDS7600ConstDefinitionNormal.m_nMAX_PRODUCT_NAME; }
		}

		public override int MAX_RESULT_DATA
		{
			get { return MPIDS7600ConstDefinitionNormal.m_nMAX_RESULT_DATA; }
		}

		public override int MAX_TEST_ITEM_LIST
		{
			get { return MPIDS7600ConstDefinitionNormal.m_nMAX_TEST_ITEM_LIST; }
		}

		public override int MAX_PROBE_DIRECTION_COUNT
		{
			get { return MPIDS7600ConstDefinitionNormal.m_nMAX_DIRECTION_COUNT; }
		}
	}

	public class MPIDS7600ConstDefinitionT200 : MPIDS7600ConstDefinitionBase
	{
        // Modify 20170524, Roy
		private const Int32 m_nMAX_DATA = 2048;
        private const Int32 m_nMAX_TEST_ITEM_LIST = 1200;
		private const Int32 m_nMAX_ITEM_NAME = 20;
		private const Int32 m_nMAX_RESULT_DATA = 20;
        private const Int32 m_nMAX_PRODUCT_NAME = 128;	// 20;
		private const Int32 m_nMAX_BIN_GRADE_LIST = 200;
		private const Int32 m_nMAX_BIN_GRADE_NAME = 20;
		private const Int32 m_nMAX_INFO = 20;
		private const Int32 m_nMAX_INFO_LIST = 100;
		private const Int32 m_nMAX_LIST_COUNT = 200;
		private const Int32 m_nMAX_DATA_LENGTH = 128;	//20;
		private const Int32 m_nMAX_DIRECTION_COUNT = 3;


		public MPIDS7600ConstDefinitionT200()
			: base()
		{
		}

		public override int MAX_BIN_GRADE_LIST
		{
			get { return MPIDS7600ConstDefinitionT200.m_nMAX_BIN_GRADE_LIST; }
		}

		public override int MAX_BIN_GRADE_NAME
		{
			get { return MPIDS7600ConstDefinitionT200.m_nMAX_BIN_GRADE_NAME; }
		}

		public override int MAX_DATA
		{
			get { return MPIDS7600ConstDefinitionT200.m_nMAX_DATA; }
		}

		public override int MAX_DATA_LENGTH
		{
			get { return MPIDS7600ConstDefinitionT200.m_nMAX_DATA_LENGTH; }
		}

		public override int MAX_INFO
		{
			get { return MPIDS7600ConstDefinitionT200.m_nMAX_INFO; }
		}

		public override int MAX_INFO_LIST
		{
			get { return MPIDS7600ConstDefinitionT200.m_nMAX_INFO_LIST; }
		}

		public override int MAX_ITEM_NAME
		{
			get { return MPIDS7600ConstDefinitionT200.m_nMAX_ITEM_NAME; }
		}

		public override int MAX_LIST_COUNT
		{
			get { return MPIDS7600ConstDefinitionT200.m_nMAX_LIST_COUNT; }
		}

		public override int MAX_PRODUCT_NAME
		{
			get { return MPIDS7600ConstDefinitionT200.m_nMAX_PRODUCT_NAME; }
		}

		public override int MAX_RESULT_DATA
		{
			get { return MPIDS7600ConstDefinitionT200.m_nMAX_RESULT_DATA; }
		}

		public override int MAX_TEST_ITEM_LIST
		{
			get { return MPIDS7600ConstDefinitionT200.m_nMAX_TEST_ITEM_LIST; }
		}

		public override int MAX_PROBE_DIRECTION_COUNT
		{
			get { return MPIDS7600ConstDefinitionT200.m_nMAX_DIRECTION_COUNT; }
		}
	}

	public class MPIDS7600ConstDefinitionWeimin : MPIDS7600ConstDefinitionBase
	{
		private const Int32 m_nMAX_DATA = 2048;
		private const Int32 m_nMAX_TEST_ITEM_LIST = 100;
		private const Int32 m_nMAX_ITEM_NAME = 20;
		private const Int32 m_nMAX_RESULT_DATA = 20;
		private const Int32 m_nMAX_PRODUCT_NAME = 20;
		private const Int32 m_nMAX_BIN_GRADE_LIST = 200;
		private const Int32 m_nMAX_BIN_GRADE_NAME = 20;
		private const Int32 m_nMAX_INFO = 20;
		private const Int32 m_nMAX_INFO_LIST = 100;
		private const Int32 m_nMAX_LIST_COUNT = 200;
		private const Int32 m_nMAX_DATA_LENGTH = 128;
		private const Int32 m_nMAX_DIRECTION_COUNT = 0;

		public MPIDS7600ConstDefinitionWeimin()
			: base()
		{
		}

		public override int MAX_BIN_GRADE_LIST
		{
			get { return MPIDS7600ConstDefinitionWeimin.m_nMAX_BIN_GRADE_LIST; }
		}

		public override int MAX_BIN_GRADE_NAME
		{
			get { return MPIDS7600ConstDefinitionWeimin.m_nMAX_BIN_GRADE_NAME; }
		}

		public override int MAX_DATA
		{
			get { return MPIDS7600ConstDefinitionWeimin.m_nMAX_DATA; }
		}

		public override int MAX_DATA_LENGTH
		{
			get { return MPIDS7600ConstDefinitionWeimin.m_nMAX_DATA_LENGTH; }
		}

		public override int MAX_INFO
		{
			get { return MPIDS7600ConstDefinitionWeimin.m_nMAX_INFO; }
		}

		public override int MAX_INFO_LIST
		{
			get { return MPIDS7600ConstDefinitionWeimin.m_nMAX_INFO_LIST; }
		}

		public override int MAX_ITEM_NAME
		{
			get { return MPIDS7600ConstDefinitionWeimin.m_nMAX_ITEM_NAME; }
		}

		public override int MAX_LIST_COUNT
		{
			get { return MPIDS7600ConstDefinitionWeimin.m_nMAX_LIST_COUNT; }
		}

		public override int MAX_PRODUCT_NAME
		{
			get { return MPIDS7600ConstDefinitionWeimin.m_nMAX_PRODUCT_NAME; }
		}

		public override int MAX_RESULT_DATA
		{
			get { return MPIDS7600ConstDefinitionWeimin.m_nMAX_RESULT_DATA; }
		}

		public override int MAX_TEST_ITEM_LIST
		{
			get { return MPIDS7600ConstDefinitionWeimin.m_nMAX_TEST_ITEM_LIST; }
		}

		public override int MAX_PROBE_DIRECTION_COUNT
		{
			get { return MPIDS7600ConstDefinitionWeimin.m_nMAX_DIRECTION_COUNT; }
		}
	}
}
