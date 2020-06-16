using System;
using System.Collections.Generic;
using System.Text;
using MPI.Communication.Command;

namespace MPI.RemoteControl.Tester
{
	public class Ttst4ipCommand : ICommand<Ttst4ipPacket>
	{
		#region >>> Constant <<<
		public const Int32 MAX_STRING_LENGTH = 256;
		public const Int32 MAX_CRI_ARRAY_COUNT = 16;
        public const Int32 MAX_MEASUREMENT_RESULT_DATA_COUNT = 39;
		public const Int32 MAX_MEASUREMENT_RESULT_LENGTH = 240;
		public const Int32 MAX_MEASUREMENT_RESULT_LIST_COUNT = 273;
		#endregion

		#region >>> Private Field <<<
		private Ttst4ipPacket m_packet;
		#endregion

		#region >>> Public Property <<<
		/// <summary>
		/// ID
		/// </summary>
		public UInt16 CommandID
		{
			get { return this.m_packet.CommandID; }
		}

		/// <summary>
		/// Message ID
		/// </summary>
		public Int32 MessageID
		{
			get { return this.m_packet.MessageID; }
		}

		/// <summary>
		/// Data Length
		/// </summary>
		public UInt32 DataLength
		{
			get { return this.m_packet.DataLength; }
		}

		/// <summary>
		/// Data
		/// </summary>
		public byte[] Data
		{
			get { return this.m_packet.Data; }
			set
			{
				this.m_packet.Data = value;
				this.m_packet.CorrectPacket();
			}
		}

		/// <summary>
		/// Command Packet
		/// </summary>
		public Ttst4ipPacket Packet
		{
			get { return this.m_packet.Clone() as Ttst4ipPacket; }
		}
		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		public Ttst4ipCommand(UInt16 CmdID, UInt32 DataLength)
		{
			this.m_packet = new Ttst4ipPacket(DataLength);
			this.m_packet.CommandID = CmdID;
			this.m_packet.MessageID = new Random().Next(0, 99999);
			this.m_packet.CorrectPacket();
		}

		#region >>> Public Method <<<
		/// <summary>
		/// Get Char Data
		/// </summary>
		public char[] GetCharData(int StartPos, int BufferLength)
		{
			char[] data = new char[BufferLength];

			if (this.Data != null && this.Data.Length >= StartPos + BufferLength)
			{
				Array.Copy(this.Data, StartPos, data, 0, BufferLength);
			}
			else
			{
				Array.Clear(data, 0, data.Length);
			}

			return data;
		}

        /// <summary>
        /// Get Byte Data
        /// </summary>
        public byte[] GetByteData(int StartPos, int BufferLength)
        {
            byte[] data = new byte[BufferLength];

            if (this.Data != null && this.Data.Length >= StartPos + BufferLength)
            {
                Array.Copy(this.Data, StartPos, data, 0, BufferLength);
            }
            else
            {
                Array.Clear(data, 0, data.Length);
            }

            return data;
        }

		/// <summary>
		/// Set Data
		/// </summary>
		public void SetData(int StartPos, int BufferLength, char[] data)
		{
			int DataLength = data.Length < BufferLength ? data.Length : BufferLength;
			byte[] databyte = ASCIIEncoding.ASCII.GetBytes(data);
			byte[] buf = this.Data;
			Array.Copy(databyte, 0, buf, StartPos, DataLength);

			byte[] buf2 = buf;

			if (BitConverter.IsLittleEndian == false)
			{
				buf2 = this.ToBitReverse(buf);
			}

			this.Data = buf2;
		}

		/// <summary>
		/// Get UInt32 Data
		/// </summary>
		protected UInt32 GetUInt32Data(int StartPos)
		{
			if ((this.Data != null) && (this.Data.Length >= StartPos + sizeof(UInt32)))
			{
				return BitConverter.ToUInt32(this.Data, StartPos);
			}
			else
			{
				return 0;
			}
		}

		/// <summary>
		/// Set UInt32 Data
		/// </summary>
		protected void SetUInt32Data(int StartPos, UInt32 value)
		{
			if ((this.Data != null) && (this.Data.Length >= StartPos + sizeof(UInt32)))
			{
				byte[] buf = this.Data;

				Array.Copy(BitConverter.GetBytes(value), 0, buf, StartPos, sizeof(UInt32));

				byte[] buf2 = buf;

				if (BitConverter.IsLittleEndian == false)
				{
					buf2 = this.ToBitReverse(buf);
				}

				this.Data = buf2;
			}
		}

		/// <summary>
		/// Get Int32 Data
		/// </summary>
		protected Int32 GetInt32Data(int StartPos)
		{
			if ((this.Data != null) && (this.Data.Length >= StartPos + sizeof(Int32)))
			{
				return BitConverter.ToInt32(this.Data, StartPos);
			}
			else
			{
				return 0;
			}
		}

		/// <summary>
		/// Set int32 Data
		/// </summary>
		protected void SetInt32Data(int StartPos, Int32 value)
		{
			if ((this.Data != null) && (this.Data.Length >= StartPos + sizeof(Int32)))
			{
				byte[] buf = this.Data;

				Array.Copy(BitConverter.GetBytes(value), 0, buf, StartPos, sizeof(Int32));

				byte[] buf2 = buf;

				if (BitConverter.IsLittleEndian == false)
				{
					buf2 = this.ToBitReverse(buf);
				}

				this.Data = buf2;
			}
		}

		/// <summary>
		/// Get UInt16 Data
		/// </summary>
		protected UInt16 GetUInt16Data(int StartPos)
		{
			if ((this.Data != null) && (this.Data.Length >= StartPos + sizeof(UInt16)))
			{
				return BitConverter.ToUInt16(this.Data, StartPos);
			}
			else
			{
				return 0;
			}
		}

		/// <summary>
		/// Set UInt16 Data
		/// </summary>
		protected void SetUInt16Data(int StartPos, UInt16 value)
		{
			if ((this.Data != null) && (this.Data.Length >= StartPos + sizeof(UInt16)))
			{
				byte[] buf = this.Data;

				Array.Copy(BitConverter.GetBytes(value), 0, buf, StartPos, sizeof(UInt16));

				byte[] buf2 = buf;

				if (BitConverter.IsLittleEndian == false)
				{
					buf2 = this.ToBitReverse(buf);
				}

				this.Data = buf2;
			}
		}

		/// <summary>
		/// Get Int16 Data
		/// </summary>
		protected Int16 GetInt16Data(int StartPos)
		{
			if ((this.Data != null) && (this.Data.Length >= StartPos + sizeof(Int16)))
			{
                return BitConverter.ToInt16(this.Data, StartPos);
			}
			else
			{
				return 0;
			}
		}

		/// <summary>
		/// Set Int16 Data
		/// </summary>
		protected void SetInt16Data(int StartPos, Int16 value)
		{
			if ((this.Data != null) && (this.Data.Length >= StartPos + sizeof(Int16)))
			{
				byte[] buf = this.Data;

				Array.Copy(BitConverter.GetBytes(value), 0, buf, StartPos, sizeof(Int16));

				byte[] buf2 = buf;

				if (BitConverter.IsLittleEndian == false)
				{
					buf2 = this.ToBitReverse(buf);
				}

				this.Data = buf2;
			}
		}

		private byte[] ToBitReverse(byte[] value)
		{
			byte[] buf = new byte[value.Length];

			for (int i = 0; i < value.Length; i++)
			{
				buf[i] = value[value.Length - 1 - i];
			}

			return buf;
		}

		#endregion

		/// <summary>
		/// Serialize command object.
		/// </summary>
		public byte[] Serialize()
		{
			return this.m_packet.Serialize();
		}

		/// <summary>
		/// Deserialize command object.
		/// </summary>
		public bool Deserialize(byte[] data)
		{
			return this.m_packet.Deserialize(data);
		}

		#region >>> IComparable Members <<<

		public int CompareTo(ICommand<Ttst4ipPacket> other)
		{
			if (other == null)
				return -1;

			if (this.CommandID > other.Packet.CommandID)
				return 1;
			else if (this.CommandID < other.Packet.CommandID)
				return -1;

			return 0;
		}

		#endregion

		#region >>> ICloneable Members <<<

		/// <summary>
		/// Clone.
		/// </summary>
		public object Clone()
		{
			return this.MemberwiseClone();
		}

		#endregion

	}

	public class ISJobInfoData
	{
		private string m_OperatorName;
		private string m_JobFileName;
		private string m_TestID;
		private string m_TestComment;
		private string m_CustomResultPath;
		private string m_CustomSpectraPath;

		public ISJobInfoData()
		{
			this.m_OperatorName = "";
			this.m_JobFileName = "";
			this.m_TestID = "";
			this.m_TestComment = "";
			this.m_CustomResultPath = "";
			this.m_CustomSpectraPath = "";
		}

		public ISJobInfoData(string Operator, string JobFileName, string TestID, string TestComment, string CustomResultPath, string CustomSpectraPath)
		{
			this.m_OperatorName = Operator;
			this.m_JobFileName = JobFileName;
			this.m_TestID = TestID;
			this.m_TestComment = TestComment;
			this.m_CustomResultPath = CustomResultPath;
			this.m_CustomSpectraPath = CustomSpectraPath;
		}

		public string OperatorName
		{
			get { return this.m_OperatorName; }
			set { this.m_OperatorName = value; }
		}

		public string JobFileName
		{
			get { return this.m_JobFileName; }
			set { this.m_JobFileName = value; }
		}

		public string TestID
		{
			get { return this.m_TestID; }
			set { this.m_TestID = value; }
		}

		public string TestComment
		{
			get { return this.m_TestComment; }
			set { this.m_TestComment = value; }
		}

		public string CustomResultPath
		{
			get { return this.m_CustomResultPath; }
			set { this.m_CustomResultPath = value; }
		}

		public string CustomSpectraPath
		{
			get { return this.m_CustomSpectraPath; }
			set { this.m_CustomSpectraPath = value; }
		}
	}
}
