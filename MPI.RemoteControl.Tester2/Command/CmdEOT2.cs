using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPI.RemoteControl2.Tester.ConstDefinition;
using MPI.RemoteControl2.Tester.Mpi.Command.Base;

namespace MPI.RemoteControl2.Tester.Mpi.Command
{
	[Serializable]
	public class CmdEOT2 : CmdPropertyBased
	{
		public enum Error
		{
			Unknown = -1,
			NoError = 0,
		}

		public static MPIDS7600ConstDefinitionBase Const = CmdMPIBased.ConstDefinition;

		// CommandID.
		public const Int32 COMMAND_ID = (int)ETSECommand.ID_EOT2;

		// Length
		public static Int32 DATA_LEN = 1 * sizeof(Int32);

		// Position
		public static Int32 ERROR_CODE_POS = 0;

		private const string UNIT_COUNT = "UNIT_COUNT";

		/// <summary>
		/// Constructor.
		/// </summary>
		public CmdEOT2()
			: base(COMMAND_ID, DATA_LEN)
		{
		}

		#region >>> Public property <<<

		/// <summary>
		/// This means some error occurred in this testing. Value 0 means no error occurred.
		/// </summary>
		public Error ErrorCode
		{
			get
			{
				int nValue = this.GetInt32Data(ERROR_CODE_POS);

				Error err = Error.NoError;

				return Enum.TryParse<Error>(nValue.ToString(), out err) ? err : Error.Unknown;
			}
			set
			{
				this.SetInt32Data(ERROR_CODE_POS, value.GetHashCode());
			}
		}

		/// <summary>
		/// Total testing result count.
		/// </summary>
		public int TestDateCount
		{
			get { return this.GetIntegerProperty(UNIT_COUNT); }
			set { this.SetIntegerProperty(UNIT_COUNT, value); }
		}

		/// <summary>
		/// Retrieve the testing result data with specified index.
		/// </summary>
		/// <param name="nIndex">This value should be between 0 ~ (TestUnitCount - 1).</param>
		public TestData this[int nIndex]
		{
			get
			{
				int nTotalCount = this.TestDateCount;

				if (nIndex < 0 || nIndex >= nTotalCount)
					throw new IndexOutOfRangeException(String.Format("Index:{0} is out of boundary. {1}", nIndex, (nTotalCount == 0) ? "No testing result is available." : String.Format("It should be between:0 ~ {1}.", nTotalCount - 1)));

				return new TestData(this, nIndex);
			}
		}

		/// <summary>
		/// Retrieve the testing result data with single die testing. (i.e. this[0])
		/// </summary>
		public TestData Single
		{
			get
			{
				if (this.TestDateCount < 1) 
					this.TestDateCount = 1;

				return this[0];
			}
		}

		#endregion
	}
}
