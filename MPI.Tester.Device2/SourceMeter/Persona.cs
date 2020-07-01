using System;
using System.Collections.Generic;
using MPI.Tester.DeviceCommon;
using System.Text;
using System.Xml;
using System.IO;
using System.Linq;

using MPI.Tester.Maths;

using MPI.Tester.Data;

namespace MPI.Tester.Device.SourceMeter
{
    public class Persona : ISourceMeter
    {
        private ElectSettingData[] _eleSetting;
        private EDevErrorNumber _errorNum = EDevErrorNumber.Device_NO_Error;
        private SourceMeterSpec _spec;
        private ElecDevSetting _elecDevSetting;

        #region
        public Persona()
        {
            this._spec = new SourceMeterSpec(_CurrRange, _VoltRange);
            SerialNumber = "Simu001";
            HardwareVersion = "0";
            SoftwareVersion = "0";
        }

        public Persona(ElecDevSetting setting):this()
        {
            _elecDevSetting = setting;
        }
        #endregion

        #region >>public property<<
        public string SerialNumber
        { get; set; }

        public string SoftwareVersion
        { get; set; }

        public string HardwareVersion
        { get; set; }

        public EDevErrorNumber ErrorNumber
        {
            get { return _errorNum; }
        }

        public ElectSettingData[] ElecSetting
        {
            get
            {
                if (this._eleSetting == null)
                {
                    return null;
                }

                ElectSettingData[] data = new ElectSettingData[this._eleSetting.Length];

                for (int i = 0; i < this._eleSetting.Length; i++)
                {
                    data[i] = this._eleSetting[i].Clone() as ElectSettingData;
                }

                return data;
            }
        }

        public SourceMeterSpec Spec
        {
            get { return this._spec; }
        }
        #endregion

        #region>>private <<

        private static double[][] _VoltRange = new double[][]	// [Index ][ Volt. Range Index ] , unit = V
												{	
													new double[] { 0.20d,   2.0d,   20.0d },
                                                    new double[] { 200.0d },
												};

        private static double[][] _CurrRange = new double[][]  // [Index ][ Current Range Index ] , unit = A
												{	
													new double[] { 100e-12d, 1e-9d,  10e-9d,  100e-9d,  1e-6d,  10e-6d,  100e-6d,  1e-3d,    10e-3d,    100e-3d,  1.0d,  1.5d },  
                                                    new double[] { 100e-12d, 1e-9d,  10e-9d,  100e-9d,  1e-6d,  10e-6d,  100e-6d,  1e-3d,    10e-3d,    100e-3d,  },
												};
        #endregion

        #region >>public method<<
        public void Close()
        {
            return; 
        }

        public double[] GetApplyDataFromMeter(uint channel, uint settingIndex)
        {
            double[] darr = new double[] { 1 };
            return darr;//後面再補上
        }

        public double[] GetDataFromMeter(uint channel, uint settingIndex)
        {
            double[] valArr = new double[]{1};
            return valArr;
        }
        public double GetPDDarkSample(int count)
        {
            return 0;
        }
        public double[] GetSweepPointFromMeter(uint channel, uint settingIndex)
        {
            return null;
        }
        public double[] GetSweepResultFromMeter(uint channel, uint itemIndex)
        {
            return null;
        }
        public double[] GetTimeChainFromMeter(uint channel, uint settingIndex)
        {
            return null;
        }
        public bool Init(int deviceNum, string sourceMeterSN)
        {
            return true;
 
        }
        public byte Input(uint point)
        {
            return 0;
        }
        public byte InputB(uint point)
        {
            return 0;
        }
        public bool MeterOutput(uint[] activateChannels, uint settingIndex)
        {
            if (ElecSetting != null && ElecSetting.Length > settingIndex)
            {
                if (ElecSetting[(int)settingIndex] != null && ElecSetting[(int)settingIndex].ForceDelayTime > 0)
                {
                    System.Threading.Thread.Sleep((int)ElecSetting[(int)settingIndex].ForceDelayTime);
                }
            }
            return true;
        }
        public bool MeterOutput(uint[] activateChannels, uint settingIndex, double applyValue)
        {
            if (ElecSetting != null && ElecSetting.Length > settingIndex)
            {
                if (ElecSetting[(int)settingIndex] != null && ElecSetting[(int)settingIndex].ForceDelayTime > 0)
                {
                    System.Threading.Thread.Sleep((int)ElecSetting[(int)settingIndex].ForceDelayTime);
                }
            }
            return true;
        }
        public void Output(uint point, bool active)
        { }
        public void Reset()
        { }
        public bool SetConfigToMeter(ElecDevSetting devSetting)
        {
            return true;
        }
        public bool SetParamToMeter(ElectSettingData[] paramSetting)
        {
            return true;
        }
        public void TurnOff()
        { }

        public void TurnOff(double delay, bool isOpenRelay)
        {
        }

        public bool CheckInterLock()
        {
            return true;
        }

        public void TriggerAssertNFT()
        {
        }
        public byte ReadDiSptNdPos(uint point)
        {
            return 1;
        }
        #endregion

    }
}
