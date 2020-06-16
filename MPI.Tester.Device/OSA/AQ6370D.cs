using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPI.Tester.Device;
using MPI.Tester.DeviceCommon;

namespace OSATestProgram
{
    public class AQ6370D:IOSA
    {
        private class AQ6370DCommand
        {
            public const string SET_MODEL_AQ6370D = "CFORM1";

            public const string SET_WAV_CENTER = ":sens:wav:cent ";

            public const string SET_WAV_SPAN = ":sens:wav:span ";

            public const string SET_WAVE_RESOLUTION = ":SENSE:BANDWIDTH:RESOLUTION ";

            public const string SET_AVERAGE_COUNT = ":SENSe:AVERage:COUNt ";

            public const string SET_WAVE_START = ":SENSe:WAVelength:STARt ";

            public const string SET_WAVE_STOP = ":SENSe:WAVelength:STOP ";


            /// <Function Defination>
            /// NHLD = NORMAL HOLD
            /// NAUT = NORMAL AUTO
            /// NORMal = NORMAL
            /// MID = MID
            /// HIGH1 = HIGH1 or HIGH1/CHOP
            /// HIGH2 = HIGH2 or HIGH2/CHOP
            /// HIGH3 = HIGH3 or HIGH3/CHOP
            /// </Function Defination>
            public const string SET_SENS_MODE = ":sens:sens ";

            public const string SET_SWEEP_POINT_AUTO = ":sens:sweep:points:auto on"; // Sampling Point = AUTO

            /// <Function Defination>
            /// Sets/queries the reference level of the main scale of the level axis. 
            /// :DISPLAY:TRACE:Y1:RLEVEL -30dbm
            /// </Function Defination>
            public const string SET_REF_LEVEL=":DISPLAY:TRACE:Y1:RLEVEL ";  //  -30dbm 

            /// <Function Defination>
            /// Sets/queries the reference level of the main scale of the level axis. 
            /// :DISPLAY:TRACE:Y1:RPOSITION 10DIV
            /// </Function Defination>
             public const string SET_REF_LOGDIV=":DISPLAY:TRACE:Y1:RPOSITION ";  // 10DIV 


            AQ6370DCommand()
            {

            }

        }

        private IConnect _connect = null;

        private GPIBSettingData _setting = new GPIBSettingData();

        private string _serialNum;

        private OsaData _data = new OsaData();

        private OsaParaSetting _paraSettingData = new OsaParaSetting();

        public AQ6370D()
        {

        }

        public bool Init(int PrimaryAddress, int deviceNum)
        {
            this._setting = new GPIBSettingData();

            this._setting.PrimaryAddress = PrimaryAddress;

            this._setting.DeviceNumber = deviceNum;

            this._connect = new GPIBConnect(this._setting);

            string inf = string.Empty;

            this._connect.Open(out inf);

            _serialNum = inf;

            return true;
        }

        public bool SetConfigToMeter(OsaDevSetting cfg)
        {
            return true;
        }

        public bool SetParaToMeter(OsaParaSetting parameter)
        {
            this._paraSettingData = parameter;

            this._connect.SendCommand("*RST");

            this._connect.SendCommand("*CFORM1");

            this._connect.SendCommand(":init:smode SINGle");

            this._connect.SendCommand("*CLS");

            string centerWavelength = parameter.CenterWavelength + "nm";

            this._connect.SendCommand(AQ6370DCommand.SET_WAV_CENTER + centerWavelength);

            this._connect.SendCommand(AQ6370DCommand.SET_WAV_SPAN + parameter.SpanOfWavelength+"nm");

            this._connect.SendCommand(AQ6370DCommand.SET_WAVE_RESOLUTION + parameter.Resoluation+"nm");

            this._connect.SendCommand(AQ6370DCommand.SET_SENS_MODE + ESenseMode.MID.ToString());

            this._connect.SendCommand(AQ6370DCommand.SET_REF_LEVEL + parameter.ReferenceLevel+"dbm");

            this._connect.SendCommand(AQ6370DCommand.SET_REF_LOGDIV + parameter.LogDiv + "DIV");

            return true;
        }



        public bool Trigger(OsaParaSetting setting)
        {
            

            return true;
        }

        public bool Trigger()
        {
            //this._connect.SendCommand(":init");       

            return true;
        }


        public bool CalculateMeasureResultData()
        {
            this._connect.SendCommand("*OPC");

            string analysisNode = this._paraSettingData.AnalysisMode.ToString();

            string str = string.Empty;

            //this._connect.QueryCommand(":stat:oper:even?");

            //this._connect.WaitAndGetData(out str);

            //this._connect.QueryCommand("*OPC?");

            //this._connect.WaitAndGetData(out str);

            // this._connect.SendCommand(":calc:category SMSR"); //   :CALCulate:CATegory<wsp>SMSR|8

            this._connect.SendCommand(":calc:category " + analysisNode);//  ' Spectrum width analysis(THRESH type)

            this._connect.SendCommand(":CALCULATE:PARAMETER:"+analysisNode+":TH 10.00DB");//  ' Spectrum width analysis(THRESH type)

            this._connect.SendCommand(":CALCULATE:PARAMETER:"+analysisNode+":K 2.00");//  ' Spectrum width analysis(THRESH type)

            this._connect.SendCommand(":calc");   //  ' Analysis Execute

            str = string.Empty;

            this._connect.QueryCommand(":calc:data?");

           // this._connect.QueryCommand(":calc:DATA:CWAVelengths?");

            this._connect.WaitAndGetData(out str);

            return true;
        }

        public void Close()
        {
            
        }

        public void Reset()
        {

        }

        #region >>> Public Property <<<

        /// <summary>
        /// OSA serial number
        /// </summary>
        public string SerialNumber
        {
            get { return this._serialNum; }
        }

        /// <summary>
        /// OSA Data
        /// </summary>
        public OsaData Data
        {
            get { return this._data; }
        }

        #endregion


    }
}
