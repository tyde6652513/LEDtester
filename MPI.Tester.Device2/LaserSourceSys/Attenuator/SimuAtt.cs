using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPI.Tester.DeviceCommon;
using MPI.Tester.Data.LaserData.LaserSource;

namespace MPI.Tester.Device.LaserSourceSys.Attenuator
{
    public class SimuAtt : AttenuatorBase
    {

        #region >><<
       

        #endregion

        #region
       
        #endregion

        public SimuAtt():base()
        {
            Spec.HavePowerControlMode = true;
        }

        public SimuAtt(AttenuatorConfig cfg)
            : this()
        {
            Address = cfg.Address;
            Slot = cfg.Slot;
            SysDevChDic = new Dictionary<int, int>();
            //SysDevChDic.Add(cfg.LaserSysChannel, cfg.AttChannel);            
        }

        public override bool Init( string address,IConnect connect)
        {
            _device = null;
            Address = address;

            return true;
        }
        public override void TurnOff()
        {
        }
        public override void Close()
        {

        }

        public override double GetMsrtPower(int sysCh = 1, ELaserPowerUnit unit = ELaserPowerUnit.W)
        {
            return sysCh*10;
        }

    }

}
