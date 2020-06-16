using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Tools;
using MPI.Tester.Device;
using MPI.Tester.DeviceCommon;
using MPI.Tester.Data.LaserData.LaserSource;
using MPI.Tester.DeviceCommon.DeviceLogger;

namespace MPI.Tester.Device.LaserSourceSys.OpticalSwitch
{
    public class SimuOS : OSBase
    {
        #region >>protected property<<
        #endregion

        #region >>constructor<<
        public SimuOS():base()
        {
        }

        public SimuOS(OpticalSwitchConfig OSCfg)
            : this()
        {
            Address = OSCfg.Address;
            Slot = OSCfg.Slot;
            ErrorNumber = EDevErrorNumber.Device_NO_Error;
            SerialNumber = "aaa";
            
        }

        #endregion

        #region
        public override bool Init(string address, IConnect conect)
        {
            Connect = null;
            foreach (var chPair in SysDevInChList)
            {
                if (!Spec.InputCh.InRange(chPair.Value))
                {
                    return false;
                }
            }

            foreach (var chPair in SysDevOutChList)
            {
                if (!Spec.OutputCh.InRange(chPair.Value))
                {
                    return false;
                }
            }
            SerialNumber = address.Replace(".","");

            #region >>logobj load<<
            try
            {
                LogTool = new DeviceRelayCountTool<DeviceRelayInfoBase>(MPI.Tester.Data.Constants.Paths.LASER_LOG, "SimuOS_" + SerialNumber);
                LogTool.Deserialize();

                if (LogTool.LogObject == null || LogTool.LogObject.RelayCntDic == null || LogTool.LogObject.RelayCntDic.Count == 0)
                {
                    LogTool.BackupXMLLog();
                    LogTool.LogObject = new DeviceRelayInfoBase("SimuOS_" + SerialNumber, Spec.OutputCh.Max);
                } Console.WriteLine("[SimuOS],Init(),LogTool.Deserialize()");
            }
            catch (Exception e)
            {

                Console.WriteLine("[SimuOS],Init(),LogTool.Deserialize(),Exception:" + e.Message);
                LogTool.BackupXMLLog();
            }
            #endregion
            return true;
        }
     
        #endregion
    }

   
}
