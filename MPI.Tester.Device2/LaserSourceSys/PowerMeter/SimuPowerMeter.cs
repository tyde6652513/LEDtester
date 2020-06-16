using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;
using Thorlabs.TLPM_32.Interop;
using MPI.Tester.Device;
using MPI.Tester.DeviceCommon;
using MPI.Tester.Data.LaserData.LaserSource;
using MPI.Tester.Device.SourceMeter;


namespace MPI.Tester.Device.LaserSourceSys.PowerMeter
{
    public class SimuPowerMeter : IPowerMeter
    {
        #region >>constructor<<
        

        public SimuPowerMeter()
        {
            Address = "";
            Slot = 0;
            NowsysCh = 0;
            ErrorNumber = EDevErrorNumber.Device_NO_Error;
            SerialNumber = "";
            SysChDevDic = new Dictionary<int, int>();

            Spec = new PowerMeterSpec();
            Spec.Channel = new MinMaxValuePair<int>(1, 1);
            
            Connect = null;
        }

        public SimuPowerMeter(PowerMeterConfig PMCfg)
            : this()
        {

            Address = PMCfg.Address;
            Slot = PMCfg.Slot;
            ErrorNumber = EDevErrorNumber.Device_NO_Error;
            SerialNumber = "";
            
        }

        #endregion

        #region >>public property<<

        public string Address { get; set; }
        public int Slot { get; set; }
        public int NowsysCh { get; set; }
        public string SerialNumber { get; private set; }
        public string SoftwareVersion { get; private set; }
        public string HardwareVersion { get; private set; }

        public Dictionary<int, int> SysChDevDic { get; set; }
        public EDevErrorNumber ErrorNumber { get; set; }
        public PowerMeterSpec Spec { set; get; }
        public IConnect Connect { get;set; }
        #endregion

        public bool Init(IConnect conect)
        {           
            Connect = null;


            foreach (var chPair in SysChDevDic)
            {
                if (!Spec.Channel.InRange(chPair.Value))
                {
                    ErrorNumber = EDevErrorNumber.LASER_PowerMeter_Init_Err;
                    return false;
                }
            }

            return true; 
        }

        public bool Push(int sysCh, int devCh)
        {
            if (Spec.Channel.InRange(devCh) && !SysChDevDic.ContainsKey(sysCh))
            {
                SysChDevDic.Add(sysCh, devCh);
            }
            else
            {
                ErrorNumber = EDevErrorNumber.LASER_PowerMeter_Init_Err;
                return false;
            }
            return true;
        }

        public double GetMsrtPower(PowerMeterSettingData pData, int Polarity)//return in W
        {
            return pData.TarPower*1.5 * pData.SysGain; 
        }

        public void Close()
        { }

        public void TurnOff()
        { }
    }
}
