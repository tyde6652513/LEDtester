using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Device;
using MPI.Tester.DeviceCommon;
using MPI.Tester.Data.LaserData.LaserSource;
using MPI.Tester.Device.SourceMeter;

namespace MPI.Tester.Device.LaserSourceSys.PowerMeter
{
    public class K2600PowerMeter : IPowerMeter
    {
         #region >>constructor<<
        

        public K2600PowerMeter()
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

        public K2600PowerMeter(PowerMeterConfig PMCfg, Keithley2600 smu = null)
            : this()
        {

            Address = PMCfg.Address;
            Slot = PMCfg.Slot;
            ErrorNumber = EDevErrorNumber.Device_NO_Error;
            SerialNumber = "";
            //Push(PMCfg.LaserSysChannel, PMCfg.PowerMeterChannel);
            SMU = smu;
            if (SMU != null && SMU.Spec != null)
            {
                Spec.SMUSpec = SMU.Spec.Clone() as SourceMeterSpec;
            }
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
        public Keithley2600 SMU = null;
        #endregion

        public bool Init(IConnect conect)
        {
            Connect = null;
            foreach (var chPair in SysChDevDic)
            {
                if (!Spec.Channel.InRange(chPair.Value))
                {
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
            double val = 0;

            ElectSettingData ele = new ElectSettingData();
            ele.MsrtType = EMsrtType.FVMI;

            double factor = MPI.Tester.Maths.UnitMath.ToSIUnit(pData.MsrtUnit);
            ele.ForceValue = pData.ForceValue * Polarity ;
            ele.ForceTime = 1000;//ms
            ele.MsrtNPLC = 1;
            ele.MsrtProtection = pData.Clamp * factor;
            val = SMU.MsrtPdMonitor(ele);
            SMU.TurnOff();
            if (pData.PDGain != 0)
            {
                val = Math.Abs(val / pData.PDGain);
            }
            val *= pData.SysGain;
            return val;
 
        }

        public void Close()
        { }


        public void TurnOff()
        { }
    }
}
