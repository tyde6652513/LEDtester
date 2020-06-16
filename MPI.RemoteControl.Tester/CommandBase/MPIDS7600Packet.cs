using System;
using System.Collections.Generic;
using System.Text;
using MPI.MCF.Communication.Command;

namespace MPI.RemoteControl.Tester
{
    // ========================================================================================================
    //	Packet Definition
    // --------------------------------------------------------------------------------------------------------
    //	1.	Header (0x07)			->	1 byte
    //	2.	Command ID (CMD)		->	4 bytes
    //	3.	Length of bytes (LOB)	->	4 bytes
    //	4.	Data bytes				->	0 ~ 2048 bytes
    // ========================================================================================================
    /// <summary>
    /// MPIDS7600Packet class.
    /// </summary>
	public class MPIDS7600Packet : IPacket, IDisposable
    {
        #region >>> Constant Definition <<<

        public const int MINIMUM_PACKET_LENGTH = 9;
        public const byte HEADER = 0x07;

        // header
        public const int HEADER_POS = 0;
        public const int HEADER_LEN = 1;
        // command id
        public const int COMMAND_ID_POS = HEADER_POS + HEADER_LEN;
        public const int COMMAND_ID_LEN = 4;
        // length
        public const int LENGTH_POS = COMMAND_ID_POS + COMMAND_ID_LEN;
        public const int LENGTH_LEN = 4;
        // data
        public const int DATA_POS = LENGTH_POS + LENGTH_LEN;
        public const int DATA_LEN = 2048;

        #endregion

        // private member
        private byte[] _buffer;
        private byte[] _tempBuffer; // Combin Packet

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPIDS7600Packet()
        {
            this._tempBuffer = null;
            this._buffer = new byte[MINIMUM_PACKET_LENGTH];
            this.CorrectPacket();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPIDS7600Packet(int dataLen)
        {
            this._buffer = new byte[MINIMUM_PACKET_LENGTH + dataLen];
            this.CorrectPacket();
        }

		public void Dispose()
		{
			this._buffer = null;
		}

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPIDS7600Packet(byte[] packet)
        {
            if (packet == null)
                throw new ArgumentNullException();

            if (packet.Length < MINIMUM_PACKET_LENGTH)
                throw new ArgumentException();

            this._buffer = packet;

            // check packet data
            if (! this.CheckPacket())
            {
                this.Data = null;
                this.CorrectPacket();
            }
        }

        #region >>> Public Properties <<<

        /// <summary>
        /// Header.
        /// </summary>
        public byte Header
        {
            get
            {
                return this._buffer[HEADER_POS];
            }
            set
            {
                this._buffer[HEADER_POS] = value;
            }
        }

        /// <summary>
        /// Command.
        /// </summary>
        public int Command
        {
            get
            {
                return BitConverter.ToInt32(this._buffer, COMMAND_ID_POS);
            }
            set
            {
                byte[] data = BitConverter.GetBytes(value);

                if (data != null)
                    Array.Copy(data, 0, this._buffer, COMMAND_ID_POS, data.Length);
            }
        }

        /// <summary>
        /// Data length.
        /// </summary>
        public uint DataLength
        {
            get
            {
                return (uint)BitConverter.ToInt32(this._buffer, LENGTH_POS);
            }
            set
            {
                byte[] data = BitConverter.GetBytes(value);

                if (data != null)
                    Array.Copy(data, 0, this._buffer, LENGTH_POS, data.Length);
            }
        }

        /// <summary>
        /// Data.
        /// </summary>
        public byte[] Data
        {
            get
            {
                byte[] data;
                if (this.DataLength <= 0)
                {
                    return null;
                }

                data = new byte[this.DataLength];

                Array.Copy(this._buffer, DATA_POS, data, 0, data.Length);

                return data;
            }
            set
            {
                bool bDataReady = true;

                if (value == null)
                    bDataReady = false;
                else if (value.Length <= 0)
                    bDataReady = false;

                if (bDataReady)
                {
                    if (value.Length == this.DataLength)
                    {
                        Array.Copy(value, 0, this._buffer, DATA_POS, value.Length);
                    }
                    else
                    {
                        MPIDS7600Packet packet = new MPIDS7600Packet(value.Length);

                        packet.Command = this.Command;
                        packet.DataLength = Convert.ToUInt32(value.Length);
                        packet.Data = value;

                        this._buffer = null;
                        this._buffer = packet.Serialize();

                        packet = null;
                    }
                }
                else
                {
                    MPIDS7600Packet packet = new MPIDS7600Packet();

                    packet.Command = this.Command;
                    packet.DataLength = 0;

                    this._buffer = null;
                    this._buffer = packet.Serialize();

                    packet = null;
                }
            }
        }

        /// <summary>
        /// Packet length.
        /// </summary>
        public int PacketLength
        {
            get
            {
                return this._buffer.Length;
            }
        }

        #endregion

        #region >>> Public Methods <<<

        /// <summary>
        /// Combin Packet.
        /// </summary>
        public byte[] CombinePacket(byte[] buffer)
        {
			if (buffer == null && this._tempBuffer == null) return null;

			if (buffer != null)
			{
            if (this._tempBuffer == null)
            {
                this._tempBuffer = new byte[buffer.Length];

                Array.Copy(buffer, this._tempBuffer, buffer.Length);
            }
            else
            {
                byte[] temp1 = this._tempBuffer;
                this._tempBuffer = new byte[temp1.Length + buffer.Length];

                Array.Copy(temp1, this._tempBuffer, temp1.Length);
                Array.Copy(buffer, 0, this._tempBuffer, temp1.Length, buffer.Length);
            }
			}

            bool foundLength = false;
            int headerPos = Array.IndexOf<byte>(this._tempBuffer, 0x07);
            int lengthPos = headerPos + HEADER_LEN + COMMAND_ID_LEN;
            int dataLength = 0;
            if (headerPos != -1) // find header Pos
            {
                if (this._tempBuffer.Length >= lengthPos + LENGTH_LEN)
                {
                    dataLength = BitConverter.ToInt32(this._tempBuffer, lengthPos);
                    foundLength = true;
                }
            }

            if (foundLength)
            {
                int packetLength = MINIMUM_PACKET_LENGTH + dataLength;
                if (this._tempBuffer.Length >= packetLength)	// It menas that intput buffer is not null.
                {
                    byte[] rtn = new byte[packetLength];
                    Array.Copy(this._tempBuffer, headerPos, rtn, 0, packetLength);

                    if (this._tempBuffer.Length == packetLength)
                    {
                        _tempBuffer = null;
                    }
                    else
                    {
                        int nextHeaderPos = lengthPos + LENGTH_LEN + dataLength;
                        byte[] temp = new byte[this._tempBuffer.Length - nextHeaderPos];

                        Array.Copy(this._tempBuffer, nextHeaderPos, temp, 0, temp.Length);
                        this._tempBuffer = temp;
                    }
                    
                    return rtn; 
                }
            }

            return null;
        }

        /// <summary>
        /// Check the packet.
        /// </summary>
        public bool CheckPacket()
        {
            return ((MINIMUM_PACKET_LENGTH + this.DataLength) == this._buffer.Length);
        }

        /// <summary>
        /// Serialize package.
        /// </summary>
        public byte[] Serialize()
        {
            return (byte[])this._buffer.Clone();
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

            int length = BitConverter.ToInt32(buffer, LENGTH_POS);

            if ((MINIMUM_PACKET_LENGTH + length) != buffer.Length)
                return false;

            this._buffer = null;
            this._buffer = buffer;

            // correct the packet
            this.CorrectPacket();

            return true;
        }

        /// <summary>
        /// Correct the packet.
        /// </summary>
        public void CorrectPacket()
        {
            // header
            this.Header = HEADER;

            // length
            if (this.PacketLength > MINIMUM_PACKET_LENGTH)
                this.DataLength = System.Convert.ToUInt32(this.PacketLength - MINIMUM_PACKET_LENGTH);
            else
                this.DataLength = 0;
        }

        /// <summary>
        /// Clone.
        /// </summary>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
