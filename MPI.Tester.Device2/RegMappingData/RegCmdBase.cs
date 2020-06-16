using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.TSE
{	
	public abstract class RegCmdBase
	{
		public const uint MAX_REG_LENGTH	= 0x01FF;
		public const uint MAX_PARAM_LENGTH	= 30;
		public const uint MAX_TABLE_LENGTH	= 2000;

		protected object _lockObj;

		protected UInt16 _cmdStartAddr;
		protected UInt16 _cmdID;

		protected UInt16 _paramStartAddr;
		protected UInt16 _paramLength;
		//protected UInt16[] _params;

		protected UInt16 _tableStartAddr;
		protected UInt16 _tableLength;
		//protected UInt16[] _table;

		protected UInt16[] _regDataArray;

		public RegCmdBase()
		{
			this._lockObj = new object();

			this._cmdStartAddr = Convert.ToUInt16(EAddr.CMD_ID);
			this._paramStartAddr = Convert.ToUInt16(EAddr.PARAMETER);
			this._tableStartAddr = Convert.ToUInt16(EAddr.SWEEP_TABLE);

			this._regDataArray = new UInt16[ MAX_REG_LENGTH ];
		}

		public RegCmdBase(UInt16 cmdID): this()
		{
			this._cmdID = cmdID;
		}

		public RegCmdBase(UInt16 cmdID, UInt16 cmdAddr, UInt16 paramStartAddr, UInt16 tableStartAddr)
			: this(cmdID)
		{
			this._cmdStartAddr = cmdAddr;
			this._paramStartAddr = paramStartAddr;
			this._tableStartAddr = tableStartAddr;
		}

		public RegCmdBase(UInt16 cmdID, UInt16 paramLength, UInt16 tableLength)
			: this(cmdID)
		{
			this._paramLength = paramLength;
			this._tableLength = tableLength;
		}

		#region >>> Public Property <<<

		public UInt16 CmdID
		{
			get { return this._cmdID; }
            set { lock (this._lockObj) { this._cmdID = value; } }
		}

		public UInt16 CmdStartAddr
		{
			get { return this._cmdStartAddr; }
		}

		public UInt16 ParamStartAddr
		{
			get { return this._paramStartAddr; }	
		}

		public UInt16 ParamLength
		{
			get { return this._paramLength; }
            set { lock (this._lockObj) { this._paramLength = value; } }
		}

		public UInt16 TableStartAddr
		{
			get { return this._tableStartAddr; }
		}

		public UInt16 TableLength
		{
			get { return this._tableLength; }
		}

		#endregion

		#region >>> Public Methods <<<

		public UInt16 GetRegData(int index)
		{
			if (index > (int) MAX_REG_LENGTH)
				return 0;

			return this._regDataArray[index];
		}

		public UInt16[] GetRegArray(int start, int length)
		{
			return null;
		}


		public UInt16 GetParamData(int index)
		{
			if (index > (int) MAX_PARAM_LENGTH)
				return 0;

			return this._regDataArray [ this._paramStartAddr + index];
		}

		public UInt16 GetTalbeData(int index)
		{
			if (index > (int)MAX_TABLE_LENGTH)
				return 0;

			return this._regDataArray[ this._tableStartAddr + index];
		}

		public void SetRegData(int index, UInt16 word)
		{
			this._regDataArray[index] = word;
		}

		public void SetParamData(int index, UInt16 word)
		{
			this._regDataArray[ this._paramStartAddr + index] = word;
		}

		public void SetTableData(int index, UInt16 word)
		{
			this._regDataArray[ this._tableStartAddr + index] = word;
		}   
        
		#endregion

	}

    public class TSEConvert
    {       
        public static Int16 TimeLSB(double data)    // ms
        {
            return (Int16) (Convert.ToInt32(data * 1000.0d) & 0xffff);
        }

        public static Int16 TimeMSB(double data)    // ms
        {
            return (Int16) (Convert.ToInt32(data * 1000.0d) >> 16);
        }

        public static double UInt16ToDouble(uint bitData, EIRange currRangeIndex)
        {
            double rtn = 0;
            switch (currRangeIndex)
            {
                case EIRange._5uA:
                    rtn = (Convert.ToDouble(bitData) * 5.0d / 65535.0d) / 1000000.0d;
                    break;
                case EIRange._10uA:
                    rtn = (Convert.ToDouble(bitData) * 10.0d / 65535.0d) / 1000000.0d;                  
                    break;
                case EIRange._100uA:
                    rtn = (Convert.ToDouble(bitData) * 100.0d / 65535.0d) / 1000000.0d;                                   
                    break;
                case EIRange._1mA:
                    rtn = (Convert.ToDouble(bitData) * 1.0d / 65535.0d) / 1000.0d;                 
                    break;
                case EIRange._10mA:
                    rtn = (Convert.ToDouble(bitData) * 10.0d / 65535.0d) / 1000.0d;                  
                    break;
                case EIRange._100mA:
                    rtn = (Convert.ToDouble(bitData) * 100.0d / 65535.0d) / 1000.0d; 
                    break;
                case EIRange._500mA:
                    rtn = (Convert.ToDouble(bitData) * 500.0d / 65535.0d) / 1000.0d; 
                    break;
                case EIRange._1A:
                    rtn = Convert.ToDouble(bitData) * 1.0d / 65535.0d;                  
                    break;
                case EIRange._2A:
                    rtn = Convert.ToDouble(bitData) * 2.0d / 65535.0d;
                    break;
                default:
                    break;
            }
            return rtn;
        }

        public static double UInt16ToDouble(uint bitData, EVRange voltRangeIndex)
        {
            double rtn = 0;
            switch (voltRangeIndex)
            {
                case EVRange._5V:
                    rtn = (Convert.ToDouble(bitData) * 5.0d) / 65535.0d;
                    break;
                case EVRange._10V:
                    rtn = (Convert.ToDouble(bitData) * 10.0d) / 65535.0d;
                    break;
                case EVRange._20V:
                    rtn = (Convert.ToDouble(bitData) * 20.0d) / 65535.0d;
                    break;
                case EVRange._50V:
                    rtn = (Convert.ToDouble(bitData) * 50.0d) / 65535.0d;
                    break;
                case EVRange._80V:
                    rtn = (Convert.ToDouble(bitData) * 100.0d) / 65535.0d;
                    break;
                default:
                    break;
            }
            return rtn;
        }

        public static UInt16 ValueToUInt16(double data, EIRange currentRangeIndex)
        {
            UInt16 rtn = 0 ;
            switch (currentRangeIndex) 
            {              
                case EIRange._5uA:
                    rtn = (UInt16)Convert.ToInt32(65535.0d * data * 1000000.0d / 5.0d);
                    break;
                case EIRange._10uA:
                    rtn = (UInt16)Convert.ToInt32(65535.0d * data * 1000000.0d / 10.0d);
                    break;
                case EIRange._100uA:
                    rtn = (UInt16)Convert.ToInt32(65535.0d * data * 1000000.0d / 100.0d);
                    break;
                case EIRange._1mA:
                    rtn = (UInt16)Convert.ToInt32(65535.0d * data * 1000.0d / 1.0d);
                    break;
                case EIRange._10mA:
                    rtn = (UInt16)Convert.ToInt32(65535.0d * data * 1000.0d / 10.0d);
                    break;
                case EIRange._100mA :
                    rtn = (UInt16)Convert.ToInt32(65535.0d * data * 1000.0d / 100.0d);
                    break;
                case EIRange._500mA:
                    rtn = (UInt16)Convert.ToInt32(65535.0d * data * 1000.0d / 500.0d);
                    break;
                case EIRange._1A:
                    rtn = (UInt16)Convert.ToInt32(65535.0d * data / 1.0d);
                    break;
                case EIRange._2A:
                    rtn = (UInt16)Convert.ToInt32(65535.0d * data / 2.0d);
                    break;
                default :
                    break;
            }
            return rtn;
        }

        public static UInt16 ValueToUInt16(double data, EVRange voltRangeIndex)
        {
            UInt16 rtn = 0;
            switch (voltRangeIndex)
            {             
                case EVRange._5V:
                    rtn = (UInt16)Convert.ToInt32(65535.0d * data / 5.0d);
                    break;
                case EVRange._10V:
                    rtn = (UInt16)Convert.ToInt32(65535.0d * data / 10.0d);
                    break;
                case EVRange._20V:
                    rtn = (UInt16)Convert.ToInt32(65535.0d * data / 20.0d);
                    break;
                case EVRange._50V:
                    rtn = (UInt16)Convert.ToInt32(65535.0d * data / 50.0d);
                    break;
                case EVRange._80V:
                    rtn = (UInt16)Convert.ToInt32(65535.0d * data / 100.0d);
                    break;  
                default:
                    break;
            }
            return rtn;
        }

        public static double GainOffsetUInt16ToDouble(uint bitData)
        {
            // Convert the hex data into double,  + : 0x0000 ~ 0x7FFF
            //                                    - : 0xFFFF ~ 0x8000        
            double rtn = 0;

            if (bitData > 0x7FFF)
            {
                bitData = (~bitData + 1) & 0xFFFF;    // 2's complemet
                rtn = (-1) * Convert.ToDouble(bitData) / 10000.0d;
            }
            else
            {
                rtn = Convert.ToDouble(bitData) / 10000.0d;
            }
            return rtn;
        }
    }
}
