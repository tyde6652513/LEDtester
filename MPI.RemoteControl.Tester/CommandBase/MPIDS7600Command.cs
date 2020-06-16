using System;
using System.Collections.Generic;
using System.Text;
using MPI.MCF.Communication.Command;
using MPI.RemoteControl.Tester.ConstDefinition;
using System.Reflection;
using System.IO;
using System.Xml.Serialization;

namespace MPI.RemoteControl.Tester
{
    /// <summary>
    /// MPIDS7600Command class.
    /// </summary>
    public abstract class MPIDS7600Command : ICommand<MPIDS7600Packet>, IDisposable
    {
        // const		// Normal
		//public const Int32 MAX_DATA = 2048;
		//public const Int32 MAX_TEST_ITEM_LIST = 100;
		//public const Int32 MAX_ITEM_NAME = 20;
		//public const Int32 MAX_RESULT_DATA = 20;
		//public const Int32 MAX_PRODUCT_NAME = 20;
		//public const Int32 MAX_BIN_GRADE_LIST = 200;
		//public const Int32 MAX_BIN_GRADE_NAME = 20;

		//public const Int32 MAX_INFO = 20;
		//public const Int32 MAX_INFO_LIST = 100;

		//public const Int32 MAX_LIST_COUNT = 200;
		//public const Int32 MAX_DATA_LENGTH = 20;

        // private field
        private MPIDS7600Packet _packet;

		private const string Namespace = "MPI.RemoteControl.Tester.ConstDefinition";
		private const string InstanceName = ".MPIDS7600ConstDefinition";

		private static MPIDS7600ConstDefinitionBase _sConstDefinition;

        public static MPIDS7600ConstDefinitionBase ConstDefinition
        {
            get
            {
                //try
                //{
                //    if (MPIDS7600Command._sConstDefinition != null) return MPIDS7600Command._sConstDefinition;

                //    // string path = Environment.CurrentDirectory + "\\Data\\ProtocolConfig.xml";
                //    string path = "C:\\MPISystem\\Data\\ProtocolConfig.xml";

                //    Console.WriteLine("Read Protocol Config File:" + path);

                //    Configuration config = (Configuration)MPIDS7600Command.Deserialize(path, typeof(Configuration));

                //    string assemblyName = Namespace + InstanceName + config.CmdConstDefinitionType.ToString();

                //    MPIDS7600Command._sConstDefinition = (MPIDS7600ConstDefinitionBase)Assembly.Load(Namespace).CreateInstance(assemblyName);	//, false, BindingFlags.ExactBinding, null, new Object[] { }, null, null);

                //    if (MPIDS7600Command._sConstDefinition == null) MPIDS7600Command._sConstDefinition = new MPIDS7600ConstDefinitionNormal();

                //    return MPIDS7600Command._sConstDefinition;
                //}
                //catch
                //{
                //    MPIDS7600Command._sConstDefinition = new MPIDS7600ConstDefinitionNormal();

                //    // string path = Environment.CurrentDirectory + "\\Data\\ProtocolConfig.xml";
                //    string path = "C:\\MPISystem\\Data\\ProtocolConfig.xml";

                //    MPIDS7600Command.Serialization(path, new Configuration());

                //    return MPIDS7600Command._sConstDefinition;
                //}

                    MPIDS7600Command._sConstDefinition = new MPIDS7600ConstDefinitionT200();

                    return MPIDS7600Command._sConstDefinition;
                }
            }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPIDS7600Command(int cmdID, int dataLen)
        {
            // initial command
            this._packet = new MPIDS7600Packet(dataLen);

            this.Command = cmdID;
        }

		public void Dispose()
		{
			this._packet.Dispose();
		}

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

        /// <summary>
        /// Packet.
        /// </summary>
        public MPIDS7600Packet Packet
        {
            get
            {
                return this._packet.Clone() as MPIDS7600Packet;
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
                return (int)this._packet.DataLength;
            }
        }

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
        public bool Deserialize(byte[] data)
        {
            return this._packet.Deserialize(data);
        }

        #endregion

        #region >>> Protected Method <<<

		protected char[] GetDataBuffer(int bufferLength, int startPos)
		{
			char[] buf = new char[bufferLength];
			byte[] data = this.Data;

			if (data != null && data.Length >= startPos + bufferLength)
			{
				Array.Copy(data, startPos, buf, 0, buf.Length);
			}
			else
			{
				Array.Clear(buf, 0, buf.Length);
			}

			return buf;
		}

        /// <summary>
        /// Get Data
        /// </summary>
        protected char[] GetData(int bufferLength, int startPos)
        {
            char[] buf = new char[bufferLength];
			byte[] data = this.Data;

			if ((data != null) && (data.Length >= startPos + bufferLength))
            {
				Array.Copy(data, startPos, buf, 0, buf.Length);
                
                int endIndex = Array.IndexOf(buf, '\0');
                if (endIndex > 0 && endIndex < 19)
                {
                    Array.Clear(buf, endIndex, bufferLength - endIndex);
                }
            }
            else
            {
                Array.Clear(buf, 0, buf.Length);
            }

            return buf;
        }

        /// <summary>
        /// Set Data
        /// </summary>
        protected bool SetData(int bufferLength, int startPos, char[] data)
        {
            if (this.Data != null && this.Data.Length >= startPos + bufferLength)
            {
                int maxItemNum = 0;
                maxItemNum = data.Length < bufferLength ? data.Length : bufferLength;
                byte[] buf = this.Data;
                Array.Copy(ASCIIEncoding.ASCII.GetBytes(data), 0, buf, startPos, maxItemNum);
                this.Data = buf;
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Set byte Data
        /// </summary>
        protected bool SetData(int bufferLength, int startPos, byte[] data)
        {
            if (this.Data != null && this.Data.Length >= startPos + bufferLength)
            {
                int maxItemNum = 0;
                maxItemNum = data.Length < bufferLength ? data.Length : bufferLength;
                byte[] buf = this.Data;
                Array.Copy(data, 0, buf, startPos, maxItemNum);
                this.Data = buf;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get int32 Data
        /// </summary>
        protected int GetInt32Data(int startPos)
        {
			byte[] data = this.Data;

			if ((data != null) && (data.Length >= startPos + sizeof(int)))
            {
				unsafe
            {
					int num = 0;

					IntPtr numPtr = new IntPtr((byte*)&num);

					System.Runtime.InteropServices.Marshal.Copy(data, startPos, numPtr, sizeof(Int32));

					return num;
				}
				// return BitConverter.ToInt32(data, startPos);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Set int32 Data
        /// </summary>
        protected bool SetInt32Data(int startPos, int value)
        {
            if ((this.Data != null) && (this.Data.Length >= startPos + sizeof(int)))
            {
                byte[] buf = this.Data;
                Array.Copy(BitConverter.GetBytes(value), 0, buf, startPos, sizeof(int));
                this.Data = buf;
				return true;
			}
			else
			{
				return false;
            }
        }

        /// <summary>
        /// Get int32 Data
        /// </summary>
        protected bool GetBoolData(int startPos)
        {
			byte[] data = this.Data;

			if ((data != null) && (data.Length >= startPos + sizeof(bool)))
            {
				return (data[startPos] != 0);

				//return BitConverter.ToBoolean(data, startPos);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Set int32 Data
        /// </summary>
        protected bool SetBoolData(int startPos, bool value)
        {
            if ((this.Data != null) && (this.Data.Length >= startPos + sizeof(bool)))
            {
                byte[] buf = this.Data;
                Array.Copy(BitConverter.GetBytes(value), 0, buf, startPos, sizeof(bool));
                this.Data = buf;
				return true;
            }
			else
			{
				return false;
			}
        }

		protected bool SetTransferableDataObject(int startPos, TransferableCommonObjectBase commonObject)
		{
			int cmdID  = this.Command;
			char[] array = commonObject.Serialize().ToCharArray();
			int length = (array != null) ? array.Length : 0;

			this._packet = new MPIDS7600Packet(length);

			this.Command = cmdID;

			return this.SetData(length, startPos, array);
		}

		protected bool GetTransferableDataObject(int startPos, TransferableCommonObjectBase commonObject)
		{
			if (commonObject == null) commonObject = new TransferableCommonObjectBase();

			char[] array = this.GetData(this.DataLenth, startPos);

			// TransferableCommonObjectBase obj = new TransferableCommonObjectBase();

			return commonObject.Deserialize(new string(array));
        }

        #endregion

		#region >>> Private Method <<<

		public static object Deserialize(string filename, Type type)
		{
			if (File.Exists(filename) == false)
			{
				//Assembly assem = Assembly.Load(type.Namespace);
				//return assem.CreateInstance(type.ToString());
				return null;
			}

			try
			{
				using (FileStream fs = new FileStream(filename, FileMode.Open))
				{
					XmlSerializer xml = new XmlSerializer(type);
					object obj = xml.Deserialize(fs);
					fs.Close();
					return obj;
				}
			}
			catch
			{
				return null;
			}
		}

		public static void Serialization(string filename, object obj)
		{
			string dir = Path.GetDirectoryName(filename);

			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}

			using (TextWriter writer = new StreamWriter(filename, false))
			{
				XmlSerializer serializer = new XmlSerializer(obj.GetType());
				serializer.Serialize(writer, obj);
			}
		}

		#endregion

		#region >>> IComparable Members <<<

		public int CompareTo(ICommand<MPIDS7600Packet> other)
        {
            if (other == null)
                return -1;

            var command = other as MPIDS7600Command;

            if ( command == null )
                return -1;

            if (this.CommandID > command.CommandID)
                return 1;
            else if (this.CommandID < command.CommandID)
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
}
