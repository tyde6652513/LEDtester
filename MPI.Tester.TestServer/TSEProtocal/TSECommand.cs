using System;
using System.Collections.Generic;
using System.Text;



namespace MPI.Comm.TSECommand
{
	/// <summary>
	/// TSECommand class.
	/// </summary>
	public abstract class TSECommand : System.ICloneable, IComparable<TSECommand>
	{
		public const Int32 MAX_DATA				= 2048;
		public const Int32 MAX_TEST_ITEM_LIST	= 100;
		public const Int32 MAX_ITEM_NAME		= 20;
		public const Int32 MAX_RESULT_DATA		= 20;
		public const Int32 MAX_PRODUCT_NAME		= 20;

		public const Int32 BASE_FIELD_COUNT		= 3;	// Field Name: Reserved, Command, Packet Size.

		// private field
		private TSEPacket _packet;

		/// <summary>
		/// Constructor.
		/// </summary>
		public TSECommand( int cmdID, int dataLen )
		{
			// initial command
			this._packet = new TSEPacket( dataLen );

			this.Command = cmdID;
		}

		#region >>> Internal Properties <<<

		/// <summary>
		/// Packet.
		/// </summary>
		internal TSEPacket Packet
		{
			get 
			{ 
				return this._packet.Clone() as TSEPacket; 
			}
		}

		#endregion

		#region >>> Protected Properties <<<

		/// <summary>
		/// Command id.
		/// </summary>
		protected int Command
		{
			get 
			{ 
				return this._packet.Command; 
			}
			set
			{
				this._packet.Command = value;
				this._packet.CorrectPacket();
			}
		}

		/// <summary>
		/// Command data.
		/// </summary>
		protected byte[] Data
		{
			get 
			{ 
				return this._packet.Data; 
			}
			set
			{
				this._packet.Data = value;
				this._packet.CorrectPacket();
			}
		}

		/// <summary>
		/// Data length.
		/// </summary>
		protected int DataLenth
		{
			get 
			{ 
				return ( int ) this._packet.DataLength; 
			}
		}

		/// <summary>
		/// Checksum.
		/// </summary>
		//protected uint CheckSum
		//{
		//    get { return this._packet.CheckSum; }
		//}

		/// <summary>
		/// Raw data.
		/// </summary>
		protected byte[] RawData
		{
			get 
			{ 
				return this._packet.Serialize(); 
			}
		}

		#endregion

		#region >>> Public Properties <<<

		/// <summary>
		/// Command id.
		/// </summary>
		public int CommandID
		{
			get 
			{ 
				return this._packet.Command;
			}
		}

		#endregion

		#region >>> Public Method <<<

		/// <summary>
		/// Serialize command object.
		/// </summary>
		public byte[] Serialize()
		{
			return this._packet.Serialize();
		}

		/// <summary>
		/// Deserialize command object.
		/// </summary>
		public bool Deserialize( byte[] data )
		{
			return this._packet.Deserialize( data );
		}

		#endregion

		#region >>> IComparable Members <<<
	
		public int CompareTo( TSECommand other )
		{
			if ( other == null )
				return -1;

			if ( this.CommandID > other.CommandID )
				return 1;
			else if ( this.CommandID < other.CommandID )
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

	// ============================================Packet Definition===========================================
	// --------------------------------------------------------------------------------------------------------
	//	1.	Header (0x07)			->	1 byte
	//	2.	Command ID (CMD)		->	4 bytes
	//	3.	Length of bytes (LOB)	->	4 bytes
	//	4.	Data bytes				->	0 ~ 2048 bytes
	// ========================================================================================================
	/// <summary>
	/// TSEPacket class.
	/// </summary>
	public class TSEPacket : ICloneable
	{
		#region >>> Constant Definition <<<

		public const int MINIMUM_PACKET_LENGTH = 9;

		public const byte HEADER = 0x07;

		// start byte
		public const int HEADER_POS = 0;
		public const int HEADER_LEN = 1;
		// command id
		public const int COMMAND_ID_POS = 1;
		public const int COMMAND_ID_LEN = 4;
		// length
		public const int LENGTH_POS = 5;
		public const int LENGTH_LEN = 4;
		// data
		public const int DATA_POS = 9;
		public const int DATA_LEN = 2037;
		// frame check sum
		//public const int CHECK_SUM_BACK_POS = 5;
		//public const int CHECK_SUM_LEN = 4;
		// stop byte
		//public const int REAR_BACK_POS = 0;
		//public const int REAR_LEN = 1;

		#endregion

		// private member
		private byte[] _buffer;

		/// <summary>
		/// Constructor.
		/// </summary>
		public TSEPacket()
		{
			this._buffer = new byte[ MINIMUM_PACKET_LENGTH ];
			this.CorrectPacket();
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public TSEPacket( int dataLen )
		{
			this._buffer = new byte[ MINIMUM_PACKET_LENGTH + dataLen ];
			this.CorrectPacket();
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public TSEPacket( byte[] packet )
		{
			if ( packet == null )
				throw new ArgumentNullException();

			if ( packet.Length < MINIMUM_PACKET_LENGTH )
				throw new ArgumentException();

			this._buffer = packet;

			// check packet data
			if ( !this.CheckPacket() )
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
				return this._buffer[ HEADER_POS ];
			}
			set 
			{ 
				this._buffer[ HEADER_POS ] = value;
			}
		}

		/// <summary>
		/// Rear.
		/// </summary>
		//public byte Rear
		//{
		//    get { return this._buffer[ this._buffer.Length - 1 - REAR_BACK_POS ]; }
		//    set { this._buffer[ this._buffer.Length - 1 - REAR_BACK_POS ] = value; }
		//}

		/// <summary>
		/// Command.
		/// </summary>
		public int Command
		{
			get 
			{ 
				return BitConverter.ToInt32( this._buffer, COMMAND_ID_POS );
			}
			set
			{
				byte[] data = BitConverter.GetBytes( value );

				if ( data != null )
					Array.Copy( data, 0, this._buffer, COMMAND_ID_POS, data.Length );
			}
		}

		/// <summary>
		/// Data length.
		/// </summary>
		public uint DataLength
		{
			get 
			{ 
				return ( uint ) BitConverter.ToInt32( this._buffer, LENGTH_POS );
			}
			set
			{
				byte[] data = BitConverter.GetBytes( value );

				if ( data != null )
					Array.Copy( data, 0, this._buffer, LENGTH_POS, data.Length );
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
				if ( this.DataLength <= 0 )
				{
					return null;
				}

				data = new byte[ this.DataLength ];

				Array.Copy( this._buffer, DATA_POS, data, 0, data.Length );

				return data;
			}
			set
			{
				bool bDataReady = true;

				if ( value == null )
					bDataReady = false;
				else if ( value.Length <= 0 )
					bDataReady = false;

				if ( bDataReady )
				{
					if ( value.Length == this.DataLength )
					{
						Array.Copy( value, 0, this._buffer, DATA_POS, value.Length );
					}
					else
					{
						TSEPacket packet = new TSEPacket( value.Length );

						packet.Command = this.Command;
						packet.DataLength = Convert.ToUInt32( value.Length );
						packet.Data = value;

						this._buffer = null;
						this._buffer = packet.Serialize();

						packet = null;
					}
				}
				else
				{
					TSEPacket packet = new TSEPacket();

					packet.Command = this.Command;
					packet.DataLength = 0;

					this._buffer = null;
					this._buffer = packet.Serialize();

					packet = null;
				}
			}
		}

		/// <summary>
		/// CheckSum.
		/// </summary>
		//public uint CheckSum
		//{
		//    get { return ( uint ) BitConverter.ToInt32( this._buffer, this._buffer.Length - CHECK_SUM_BACK_POS ); }
		//    set
		//    {
		//        byte[] data = BitConverter.GetBytes( value );

		//        if ( data != null )
		//            Array.Copy( data, 0, this._buffer, this._buffer.Length - CHECK_SUM_BACK_POS, data.Length );
		//    }
		//}

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
		/// Serialize package.
		/// </summary>
		public byte[] Serialize()
		{
			return ( byte[] ) this._buffer.Clone();
		}

		/// <summary>
		/// Deserialize package.
		/// </summary>
		public bool Deserialize( byte[] buffer )
		{
			if ( buffer == null )
				return false;

			if ( buffer.Length < MINIMUM_PACKET_LENGTH )
				return false;

			int length = BitConverter.ToInt32( buffer, LENGTH_POS );

			if ( ( MINIMUM_PACKET_LENGTH + length ) != buffer.Length )
				return false;

			this._buffer = null;
			this._buffer = buffer;

			// correct the packet
			this.CorrectPacket();

			return true;
		}

		/// <summary>
		/// Check the packet.
		/// </summary>
		public bool CheckPacket()
		{
			return ( ( MINIMUM_PACKET_LENGTH + this.DataLength ) == this._buffer.Length );
		}

		/// <summary>
		/// Correct the packet.
		/// </summary>
		public void CorrectPacket()
		{
			// header
			this.Header = HEADER;

			// length
			if ( this.PacketLength > MINIMUM_PACKET_LENGTH )
				this.DataLength = System.Convert.ToUInt32( this.PacketLength - MINIMUM_PACKET_LENGTH );
			else
				this.DataLength = 0;

			//this.CheckSum = this.CalculateChecksum();

			// rear
			//this.Rear = REAR;
		}

		/// <summary>
		/// Calculate checksum.
		/// </summary>
		public uint CalculateChecksum()
		{
			// command
			byte[] data = new byte[ COMMAND_ID_LEN + LENGTH_LEN + this.DataLength ];

			Array.Copy( this._buffer, COMMAND_ID_POS, data, 0, data.Length );

			return ( uint ) MPI.Crc32.GetCrc32( data );
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

	/// <summary>
	/// CommandAgent class.
	/// </summary>
    //public abstract class TSECommandAgent : IDisposable
    //{
    //    private TSEPacketAgent _packetAgent;

    //    // event
    //    public event EventHandler<TSECommandReceivedEventArg> CommandReceived;

    //    /// <summary>
    //    /// Constructor.
    //    /// </summary>
    //    public TSECommandAgent()
    //    {
    //        this.CommandReceived = null;
    //    }

    //    #region >>> Public Properties <<<

    //    /// <summary>
    //    /// Packet agent.
    //    /// </summary>
    //    public TSEPacketAgent PacketAgent
    //    {
    //        get 
    //        { 
    //            return this._packetAgent;
    //        }
    //    }

    //    #endregion

    //    #region >>> Private Methods <<<

    //    private void _packetAgent_PacketReceived( object sender, TSEPacketReceivedEventArg e )
    //    {
    //        TSECommand cmd = this.CommandFactory( e.Packet.Command );

    //        if ( cmd != null )
    //        {
    //            // command deserialize
    //            if ( cmd.Deserialize( e.Packet.Serialize() ) )
    //            {
    //                // Gilbert
    //                // fire event
    //                if ( this.CommandReceived != null )
    //                    this.CommandReceived( this, new TSECommandReceivedEventArg( cmd ) );
    //            }
    //        }
    //    }

    //    #endregion

    //    #region >>> Public Methods <<<

    //    /// <summary>
    //    /// Initialize.
    //    /// </summary>
    //    public bool Open( TSEPacketAgent agent )
    //    {
    //        if ( agent == null )
    //            return false;

    //        this._packetAgent = agent;
    //        this._packetAgent.PacketReceived += new EventHandler<TSEPacketReceivedEventArg>( _packetAgent_PacketReceived );

    //        return true;
    //    }

    //    /// <summary>
    //    /// Close.
    //    /// </summary>
    //    public void Close()
    //    {
    //        if ( this._packetAgent != null )
    //            this._packetAgent.Close();
    //    }

    //    /// <summary>
    //    /// Dispose.
    //    /// </summary>
    //    public void Dispose()
    //    {
    //        this.Close();
    //    }

    //    /// <summary>
    //    /// Send packet.
    //    /// </summary>
    //    public Boolean Send( TSECommand command )
    //    {
    //        TSEPacketAgent.EResultOfSend resultOfSend;

    //        if ( this._packetAgent != null )
    //        {
    //            resultOfSend = this._packetAgent.Send( command.Packet );

    //            if ( resultOfSend.Equals( TSEPacketAgent.EResultOfSend.Success ) )
    //            {
    //                return true;
    //            }
    //        }

    //        return false;
    //    }

    //    /// <summary>
    //    /// Command factory.
    //    /// </summary>
    //    public abstract TSECommand CommandFactory( int CommandID );

    //    #endregion

    //}

	/// <summary>
	/// CommandReceivedEventArg class.
	/// </summary>
	public class TSECommandReceivedEventArg : EventArgs
	{
		private TSECommand _command;

		/// <summary>
		/// Constructor.
		/// </summary>
		public TSECommandReceivedEventArg( TSECommand command )
		{
			this._command = command;
		}

		/// <summary>
		/// Command.
		/// </summary>
		public TSECommand Command
		{
			get { return this._command; }
		}
	}

    ///// <summary>
    ///// PacketAgent class.
    ///// </summary>
    //public class TSEPacketAgent : IDisposable
    //{
    //    public enum EResultOfSend
    //    {
    //        Success = 0,
    //        NullBuffer = 1,
    //        EmptyBuffer = 2,
    //        NoConnection = 3,
    //        BreakConnection = 4,
    //    }

    //    private TransportAgent _transport;

    //    // event
    //    public event EventHandler<TSEPacketReceivedEventArg> PacketReceived;

    //    /// <summary>
    //    /// Constructor.
    //    /// </summary>
    //    public TSEPacketAgent( TransportAgent transport )
    //    {
    //        if ( transport == null )
    //            throw new ArgumentNullException();

    //        this.PacketReceived = null;

    //        this._transport = transport;
    //        this._transport.DataReceived += new EventHandler( _transport_DataReceived );
    //    }

    //    #region >>> Private Methods <<<

    //    private void _transport_DataReceived( object sender, EventArgs e )
    //    {
    //        if ( this._transport.Transport.Available > 0 )
    //        {
    //            byte[] buffer = new byte[ this._transport.Transport.Available ];

    //            this._transport.Transport.Read( buffer, 0, buffer.Length );

    //            TSEPacket packet = new TSEPacket( buffer );

    //            if ( packet.CheckPacket() )
    //            {
    //                // Gilbert
    //                // fire event
    //                if ( this.PacketReceived != null )
    //                    this.PacketReceived( this, new TSEPacketReceivedEventArg( packet ) );
    //            }
    //        }
    //    }

    //    #endregion

    //    #region >>> Public Properties <<<

    //    /// <summary>
    //    /// Transport agent.
    //    /// </summary>
    //    public TransportAgent TransportAgent
    //    {
    //        get 
    //        { 
    //            return this._transport; 
    //        }
    //    }

    //    #endregion

    //    #region >>> Public Methods <<<

    //    /// <summary>
    //    /// Close.
    //    /// </summary>
    //    public void Close()
    //    {
    //        if ( this._transport != null )
    //            this._transport.Close();
    //    }

    //    /// <summary>
    //    /// Dispose.
    //    /// </summary>
    //    public void Dispose()
    //    {
    //        this.Close();
    //    }

    //    /// <summary>
    //    /// Send packet.
    //    /// </summary>
    //    public EResultOfSend Send( TSEPacket packet )
    //    {
    //        byte[] buffer = packet.Serialize();

    //        if ( buffer == null )
    //            return EResultOfSend.NullBuffer;

    //        if ( buffer.Length == 0 )
    //            return EResultOfSend.EmptyBuffer;

    //        if ( this._transport.Transport != null )
    //        {
    //            try
    //            {
    //                this._transport.Transport.Write( buffer, 0, buffer.Length );

    //                return EResultOfSend.Success;
    //            }
    //            catch
    //            {
    //                return EResultOfSend.BreakConnection;
    //            }
    //        }
    //        else
    //        {
    //            return EResultOfSend.NoConnection;
    //        }
    //    }

    //    #endregion
    //}

	/// <summary>
	/// PacketReceivedEventArg class.
	/// </summary>
	public class TSEPacketReceivedEventArg : EventArgs
	{
		private TSEPacket _packet;

		/// <summary>
		/// Constructor.
		/// </summary>
		public TSEPacketReceivedEventArg( TSEPacket packet )
		{
			this._packet = packet;
		}

		/// <summary>
		/// Packet
		/// </summary>
		public TSEPacket Packet
		{
			get 
			{ 
				return this._packet; 
			}
		}
	}
}
