using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Device.LaserSourceSys.Attenuator
{
    public class AttenuatorBase : IAttenuator
    {

        #region >>protected property<<
        protected string _serialNumber;
        protected string _softwareVersion;
        protected string _hardwareVersion;
        protected AttenuatorSpec _spec;
        protected Dictionary<int, AttenuatorSettingData> _sysChSetDic;
        protected AttenuatorSettingData _attSetting;
        protected IConnect _device;
        protected EDevErrorNumber _errorCode;

        #endregion

        #region
        public string SerialNumber { get { return _serialNumber; } }
        public string SoftwareVersion { get { return _softwareVersion; } }
        public string HardwareVersion { get { return _hardwareVersion; } }
        public string Address { get; set; }
        public int Slot { get; set; }

        public EDevErrorNumber ErrorNumber { get { return _errorCode; } }

        public AttenuatorSpec Spec { get { return _spec; } }
        public AttenuatorSettingData AttSetting { get { return _attSetting; } }

        public Dictionary<int, int> SysDevChDic { set; get; }//光機系統channel/儀器channel ,考量到UI識別以及儀器普遍都是1 base，此處使用1 base
        public IConnect Connect { get { return _device; } }
        #endregion

        #region
        public AttenuatorBase()
        {
            Slot = 0;
            _serialNumber = "";
            _softwareVersion = "";
            _hardwareVersion = "";
            _errorCode = EDevErrorNumber.Device_NO_Error;
            _spec = new AttenuatorSpec();
            _attSetting = new AttenuatorSettingData();
            _device = null;
            SysDevChDic = new Dictionary<int, int>();
            _spec.Channel = new MinMaxValuePair<int>(1, 1);
            Address = "";
            SysDevChDic.Add(1, 1);


            _spec.WavelengthRange = new MinMaxValuePair<double>(1260, 1640);
            _spec.Channel = new MinMaxValuePair<int>(1, 1);

            _spec.AttenuationRange = new MinMaxValuePair<double>(0, 35);
            _spec.AveragingTime = new MinMaxValuePair<double>(0.002, 1);
            _spec.MaxInputPower = 23;//dbm
            _spec.PowerRange = new MinMaxValuePair<double>(-50, 20);
            _spec.SettlingTime = 200;//power mode, 20ms for attenuator mode
            _spec.TransitionSpeed = new MinMaxValuePair<double>(0.1, 1000);
            _spec.HavePowerControlMode = false;
        }

        public virtual bool Init(string address, IConnect connect)
        {
            bool isOnlyIPAddressNum = MPI.Tester.Device.LCRMeter.LCRBase.ISThisIPString(address);

            if (connect == null)
            {
                if (isOnlyIPAddressNum)
                {
                    LANSettingData lanData = new LANSettingData();

                    lanData.IPAddress = address;

                    this._device = new LANConnect(lanData);
                }
                else
                {
                    this._device = new IVIConnect(address);
                }
            }
            else
            {
                this._device = connect;
            }
            string msg = string.Empty;

            Address = address;

            if (!this._device.Open(out msg))
            {
                if (!this._device.Open(out msg))//因為N7761連線非常慢，有時會連線逾時，因此用這個方法來避免
                {
                    this._errorCode = EDevErrorNumber.LASER_Attenuator_Init_Err;
                }

                return false;
            }

            return true;
        }

        public virtual void TurnOff()
        {
        }

        public virtual void Close()
        {
            this._device.Close();
        }
        public virtual bool SetParamToAttenuator(List<AttenuatorSettingData> paramSetting)
        {
            if (_sysChSetDic == null)
            {
                _sysChSetDic = new Dictionary<int, AttenuatorSettingData>();
            }
            if (paramSetting != null && _sysChSetDic != null)
            {
                foreach (var para in paramSetting)
                {
                    if (_sysChSetDic.ContainsKey(para.SysChannel))
                    {
                        _sysChSetDic[para.SysChannel] = para.Clone() as AttenuatorSettingData;
                    }
                    else
                    {
                        _sysChSetDic.Add(para.SysChannel, (para.Clone() as AttenuatorSettingData));
                    }
                }
 
            }
            return true; ;
        }

        public virtual double GetMsrtPower(int sysCh = 1,ELaserPowerUnit unit = ELaserPowerUnit.W)
        {
            return 0;
        }

        public virtual MinMaxValuePair<double> GetOutputPowerRangeIndBm(int ch)
        {
            return new MinMaxValuePair<double>(-70, 10);
        }
        #endregion
        #region

        protected virtual void QueryValue(ref string msg, ref double val)
        {
            _device.SendCommand(msg);
            _device.WaitAndGetData(out msg);
            if (double.TryParse(msg, out val)) { }
        }
        #endregion

    }
}
