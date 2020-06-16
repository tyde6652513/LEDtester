using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Data;

namespace MPI.Tester.Device.LCRMeter
{
	public class E4980A : LCRBase 
	{
		public E4980A():base()
		{
            //this._devSetting = new LCRDevSetting();

            //this._lcrSetting = null;

            //this._msrtResult = null;

            //this._errorCode = EDevErrorNumber.Device_NO_Error;

            //this._serialNumber = string.Empty;

            //this._softwareVersion = string.Empty;

            //this._hardwareVersion = string.Empty;

            //this._cmdData = new CmdData();

            this._caliData = new LCRCaliData(200);
		}

		public E4980A(LCRDevSetting Setting)
			: this()
		{
			this._devSetting = Setting;

            
		}

		private void SetDeviceSpec(string sn)
		{
			this._spec = new LCRMeterSpec();

			this._spec.IsProvideSignalLevelV = true;

			this._spec.IsProvideSignalLevelI = true;

			this._spec.IsProvideDCBiasV = true;

			this._spec.IsProvideDCBiasI = false;

			this._spec.SignalLevelVMin = 0;

			this._spec.SignalLevelVMax = 2;

			this._spec.SignalLevelIMin = 0;

			this._spec.SignalLevelIMax = 0.02;

			this._spec.FrequencyMin = 20;

			this._spec.FrequencyMax = 2e6;

			this._spec.DCBiasVMin = -40;

			this._spec.DCBiasVMax = 40;

			this._spec.DCBiasIMin = 0;

			this._spec.DCBiasIMax = 0;

			this._spec.MsrtSpeedList.Add(ELCRMsrtSpeed.Long);

			this._spec.MsrtSpeedList.Add(ELCRMsrtSpeed.Medium);

			this._spec.MsrtSpeedList.Add(ELCRMsrtSpeed.Short);

            //foreach (var item in Enum.GetNames(typeof(ELCRTestType)))
            //{
            //    ELCRTestType type = (ELCRTestType)Enum.Parse(typeof(ELCRTestType), item);

            //    this._spec.TestTypeList.Add(type);

            //    this._spec.CaliTypeList.Add(type);
            //}
            this._spec.TestTypeList.Clear();
            this._spec.TestTypeList.Add(ELCRTestType.CPD);
            this._spec.TestTypeList.Add(ELCRTestType.CSD);
            this._spec.TestTypeList.Add(ELCRTestType.RX);
            this._spec.TestTypeList.Add(ELCRTestType.ZTD);

            this._spec.CaliTypeList.Clear();
            this._spec.CaliTypeList.AddRange(this._spec.TestTypeList.ToArray());

            //////////////////////////////////////////////caliData

            this._spec.CableLenList.AddRange((new string[] { "0m", "1m", "2m","4m" }));

            this._spec.CaliDataQty = 200;
		}

        public override bool Init(int deviceNum, string sourceMeterSN)
		{
			this.SetDeviceSpec(this._serialNumber);

			// check is ip address 
            bool isOnlyIPAddressNum = ISThisIPString(sourceMeterSN);

			if (isOnlyIPAddressNum)
			{
				LANSettingData lanData = new LANSettingData();

				lanData.IPAddress = sourceMeterSN;

				this._device = new LANConnect(lanData);
			}
			else
			{
				this._device = new IVIConnect(sourceMeterSN);
			}

			string msg = string.Empty;

			if (!this._device.Open(out msg))
			{
                if (!this._device.Open(out msg))
                {
                    this._errorCode = EDevErrorNumber.LCRInitFail;

                    return false;
                }
			}

			this._device.SendCommand("*IDN?");

			this._device.WaitAndGetData(out msg);

			string[] data = msg.Replace("\n", "").Split(',');

			if (data.Length != 4)
			{
				this._errorCode = EDevErrorNumber.LCRInitFail;

				return false;
			}

			this._serialNumber = data[2];

			this._softwareVersion = "NA";

			this._hardwareVersion = data[3];

			this._device.SendCommand("*RST;*CLS");

			this._device.SendCommand("SYST:KLOCK ON");

			this._device.SendCommand(":TRIG:SOUR BUS");			

			this._device.SendCommand("VOLT 0");

            this._device.SendCommand("BIAS:STAT ON");

			this._device.SendCommand(":BIAS:VOLT 0");

            this._device.SendCommand(":AMPL:ALC ON");

            this._device.SendCommand(":INIT:CONT ON");

            this._device.SendCommand(":BIAS:STAT ON");

            this._device.SendCommand(":LIST:MODE SEQ");

            SetAC(ELCRSignalMode.Voltage, 0.03);

            //this._device.SendCommand(":DISP:ENAB 0");

            //this._device.SendCommand(":APER 10");
            
			return true;
		}
        public override void TurnOff()
		{
            if (this._devSetting.LCRDCBiasType == ELCRDCBiasType.Internal)
            {
                this._device.SendCommand("BIAS:VOLT 0");

                this._cmdData.Bias = "BIAS:VOLT 0";
            }

            //this._device.SendCommand("VOLT 0");

            //this._cmdData.SingleLevel = "VOLT 0";//先註解掉
		}

        public override bool PreSettingParamToMeter(uint settingIndex)
		{
			if (this._lcrSetting == null || this._lcrSetting.Length == 0)
			{
				return false;
			}

			LCRSettingData data = this._lcrSetting[settingIndex];

            if (data.MsrtDelayTime > 0)
            {
                System.Threading.Thread.Sleep((int)data.MsrtDelayTime);
            }

            SetAC(data);

            SetDC(data);

            SetType(data);

            SetFreq(data);            

            SetRange(data);

            SetMsrtSpeed(data);

            return true;
		}

        public override bool PreSetBiasListToMeter(uint settingIndex)
        {
            if (this._lcrSetting == null || this._lcrSetting.Length == 0)
            {
                return false;
            }

            LCRSettingData data = this._lcrSetting[settingIndex];


            System.Threading.Thread.Sleep((int)data.MsrtDelayTime);

            SetAC(data);

            SetBiasList(data);

            SetType(data);

            SetFreq(data);

            SetMsrtSpeed(data);

            SetRange(data);

            return true;
        }


        public override void Close()
		{
			if (this._device != null)
			{
				this.TurnOff();

				if (this._devSetting.LCRDCBiasType == ELCRDCBiasType.Internal)
				{
					this._device.SendCommand(":BIAS:STATe OFF");
				}
                this._device.SendCommand("INITiate:CONTinuous OFF");

                this._device.SendCommand(":TRIGger:SOURce INT");	

                this._device.SendCommand("SYST:KLOCK OFF");


                this._device.SendCommand(":DISP:ENAB 1");
                

                this._device.SendCommand("CLS*;*RST");

				this._device.Close();
			}
		}

        #region >>private method<<

        #endregion


    }
}
