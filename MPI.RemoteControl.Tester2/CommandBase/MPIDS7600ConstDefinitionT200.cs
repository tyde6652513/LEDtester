using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.RemoteControl2.Tester.ConstDefinition
{
	public class MPIDS7600ConstDefinitionT200 : MPIDS7600ConstDefinitionBase
	{
		private const Int32 m_nMAX_DATA = 2048;
		private const Int32 m_nMAX_TEST_ITEM_LIST = 1200;   // 500;
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

		public override EMPIDS7600ConstDefinitionType ProtocolType
		{
			get { return EMPIDS7600ConstDefinitionType.T200; }
		}
	}
}
