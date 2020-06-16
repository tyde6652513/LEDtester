using System;
using System.Collections.Generic;
using System.Text;
using MPI.Communication.Command;

namespace MPI.RemoteControl.Tester
{
	// ========================================================================================================
	//	Packet Definition
	// --------------------------------------------------------------------------------------------------------
	//	1.	Size of Command Block	->	2 bytes	(Byte 0 - 1)
	//	2.	Command ID					->	2 bytes (Byte 2 - 3)
	//	3.	Message ID					->	4 bytes (Byte 4 - 7)
	//	4.	Data bytes					->	65528 bytes (Byte 8 - 65535)
	// ========================================================================================================
	/// <summary>
	/// Ttst4ipPacket class.
	/// </summary>
	public class Ttst4ipPacket : IPacket
	{
		#region >>> Constant Definition <<<

		public const int MINIMUM_PACKET_LENGTH = 8;	// Header: Size(2) + Command ID(2) + Message ID(4) = 8 Bytes

		// Size
		public const int SIZE_POS = 0;
		public const int SIZE_LEN = 2;
		// Command ID
		public const int COMMAND_ID_POS = SIZE_POS + SIZE_LEN;
		public const int COMMAND_ID_LEN = 2;
		// Message ID
		public const int MESSAGE_ID_POS = COMMAND_ID_POS + COMMAND_ID_LEN;
		public const int MESSAGE_ID_LEN = 4;
		// Data
		public const int DATA_POS = MESSAGE_ID_POS + MESSAGE_ID_LEN;
		public const int DATA_LEN = 65528;

		#endregion

		private byte[] m_buffer;
		//private byte[] m_tempBuffer; // Combin Packet
		private UInt16 m_dataLength;

		public Ttst4ipPacket()
		{
			this.m_buffer = new byte[MINIMUM_PACKET_LENGTH];
			this.CorrectPacket();
		}

		public Ttst4ipPacket(UInt32 DataLength)
		{
			this.m_buffer = new byte[MINIMUM_PACKET_LENGTH + DataLength];
			this.CorrectPacket();
		}

		public int PacketLength
		{
			get { return this.m_buffer.Length; }
		}

		public UInt16 Size
		{
			get { return BitConverter.ToUInt16(this.m_buffer, SIZE_POS); }
			set
			{
				byte[] data = BitConverter.GetBytes(value);

				if (data != null)
				{
					Array.Copy(data, 0, this.m_buffer, SIZE_POS, data.Length);
				}
			}
		}

		public UInt16 CommandID
		{
			get { return BitConverter.ToUInt16(this.m_buffer, COMMAND_ID_POS); }
			set
			{
				byte[] data = BitConverter.GetBytes(value);

				if (data != null)
				{
					Array.Copy(data, 0, this.m_buffer, COMMAND_ID_POS, data.Length);
				}
			}
		}

		public Int32 MessageID
		{
			get { return BitConverter.ToInt32(this.m_buffer, MESSAGE_ID_POS); }
			set
			{
				byte[] data = BitConverter.GetBytes(value);

				if (data != null)
				{
					Array.Copy(data, 0, this.m_buffer, MESSAGE_ID_POS, data.Length);
				}
			}
		}

		public UInt16 DataLength
		{
			get { return this.m_dataLength; }
		}

		public byte[] Data
		{
			get
			{
				byte[] data;

				if (this.m_dataLength <= 0)
				{
					return null;
				}

				data = new byte[this.m_dataLength];

				Array.Copy(this.m_buffer, DATA_POS, data, 0, this.m_dataLength);

				return data;
			}
			set
			{
				bool bDataReady = (value == null || value.Length <= 0) ? false : true;

				if (bDataReady)
				{
					if (value.Length == this.DataLength)
					{
						Array.Copy(value, 0, this.m_buffer, DATA_POS, value.Length);
					}
					else
					{
						Ttst4ipPacket packet = new Ttst4ipPacket(Convert.ToUInt32(value.Length));
						packet.Size = this.Size;
						packet.CommandID = this.CommandID;
						packet.MessageID = this.MessageID;
						packet.Data = value;

						this.m_buffer = null;
						this.m_buffer = packet.Serialize();
						packet = null;
					}
				}
				else
				{
					Ttst4ipPacket packet = new Ttst4ipPacket();
					packet.Size = this.Size;
					packet.CommandID = this.CommandID;
					packet.MessageID = this.MessageID;

					this.m_buffer = null;
					this.m_buffer = packet.Serialize();
					packet = null;
				}
			}
		}

		#region >>> Public Method <<<
		/// <summary>
		/// Correct the packet.
		/// </summary>
		public void CorrectPacket()
		{
			// length
			if (this.PacketLength > MINIMUM_PACKET_LENGTH)
				this.m_dataLength = System.Convert.ToUInt16(this.PacketLength - MINIMUM_PACKET_LENGTH);
			else
				this.m_dataLength = 0;

			byte[] size = BitConverter.GetBytes(this.PacketLength);
			Array.Copy(size, 0, this.m_buffer, SIZE_POS, sizeof(Int16));

		}

		/// <summary>
		/// Serialize package.
		/// </summary>
		public byte[] Serialize()
		{
			return (byte[])this.m_buffer.Clone();
		}

		/// <summary>
		/// Deserialize package.
		/// </summary>
		public bool Deserialize(byte[] buffer)
		{
			if (buffer == null)
				return false;

			if (buffer.Length < MINIMUM_PACKET_LENGTH)
				return false;

			UInt16 length = BitConverter.ToUInt16(buffer, SIZE_POS);

			if ((Convert.ToInt32(length)) != buffer.Length)
				return false;

			this.m_buffer = null;
			this.m_buffer = buffer;

			// correct the packet
			this.CorrectPacket();

			return true;
		}

		/// <summary>
		/// Clone.
		/// </summary>
		public object Clone()
		{
			return this.MemberwiseClone();
		}

		/// <summary>
		/// Check the packet.
		/// </summary>
		public bool CheckPacket()
		{
			return ((MINIMUM_PACKET_LENGTH + this.DataLength) == this.m_buffer.Length);
		}

		/// <summary>
		/// Combin Packet.
		/// </summary>
		public byte[] CombinePacket(byte[] buffer)
		{
			return buffer;
		}

		#endregion
	}
}
