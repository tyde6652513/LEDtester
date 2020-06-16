using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

using MPI.Tester.Tools;
using MPI.Tester.Device;
using MPI.Tester.DeviceCommon;
using MPI.Tester.DeviceCommon.DeviceLogger;
using MPI.Tester.Data.LaserData.LaserSource;
using MPI.Tester.Device.SourceMeter;

namespace MPI.Tester.Device.LaserSourceSys.OpticalSwitch
{
    public class OSW12 : OSBase
    {
        //Dictionary<int, int> _sysChIOStateDic = new Dictionary<int, int>();
        List<SwitchInfo> sInfoList = new List<SwitchInfo>();
         public OSW12()
            : base()
        {
            Spec.InputCh = new MinMaxValuePair<int>(1, 14);
            Spec.OutputCh = new MinMaxValuePair<int>(1, 14);
         }
         public OSW12(OpticalSwitchConfig OSCfg)
            : base(OSCfg)
        {
            Spec.InputCh = new MinMaxValuePair<int>(1, 14);
            Spec.OutputCh = new MinMaxValuePair<int>(1, 14);
            Address = OSCfg.Address;
        }

         public OSW12(OpticalSwitchConfig OSCfg, Keithley2600 smu = null)
            : this()
        {
            SMU = smu;
            Address = OSCfg.Address;
        }
        #region
        public Keithley2600 SMU = null;
        #endregion

        #region
        public override bool Init(string address, IConnect conect)
        {
            Console.WriteLine("[OSW12],Init()");
            Connect = conect;
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

            SerialNumber = "OSW12";
            foreach (var chPair in SysDevOutChList)
            {
                SerialNumber += "_ch" + chPair.Value.ToString();
            }
            Console.WriteLine("[OSW12],Init(),SerialNumber = " + SerialNumber);

            #region >>logobj load<<
            string deviceName = "OSW12_" + SerialNumber;

            try
            {
                LogTool = new DeviceRelayCountTool<DeviceRelayInfoBase>(MPI.Tester.Data.Constants.Paths.LASER_LOG, deviceName);
                LogTool.Deserialize();

                if (LogTool.LogObject == null || LogTool.LogObject.RelayCntDic == null || LogTool.LogObject.RelayCntDic.Count == 0)
                {
                    LogTool.BackupXMLLog();
                    LogTool.LogObject = new DeviceRelayInfoBase(deviceName, Spec.OutputCh.Max);
                }
                Console.WriteLine("[OSW12],Init(),LogTool.Deserialize()");
            }
            catch (Exception e)
            {

                Console.WriteLine("[OSW12],Init(),LogTool.Deserialize(),Exception:" + e.Message);
                LogTool.BackupXMLLog();
            }
            
            #endregion


            return true;
        }

        //public void PushChIO(int sysCh, int ioState)
        //{
        //    ioState = ioState==1 ? 1 : 0;//方便UI層可以直接將io state設定為通道編號
        //    if (!_sysChIOStateDic.ContainsKey(sysCh))
        //    {
        //        _sysChIOStateDic.Add(sysCh, ioState);
        //    }
        //    else
        //    {
        //        _sysChIOStateDic[sysCh]=  ioState;
        //    }
        //}

        public void PushChIO(int sysCh, int outPin, int ioState)
        {
            //ioState = ioState==1 ? 1 : 0;//方便UI層可以直接將io state設定為通道編號
            int index = sInfoList.FindIndex(x => x.SysCh == sysCh && x.OutPin == outPin);
            if (index >= 0)
            {
                sInfoList[index] = new SwitchInfo(sysCh, outPin, ioState);
            }
            else
            {
                sInfoList.Add(new SwitchInfo(sysCh, outPin, ioState));
            }
        }

        
        public override bool Push(int sysCh, int inCh, int outCh)
        {
            SysDevInChList.Add(new KeyValuePair<int, int>(sysCh, inCh));
            SysDevOutChList.Add(new KeyValuePair<int, int>(sysCh, outCh));

            SMU.SetIOInitState(inCh, true);
            SMU.SetIOInitState(outCh, true);
            SysChList.Add(sysCh);
            return true;
        }

        public override bool SetToSysCh(int sysCh = 1)
        {
            var inList = SysDevInChList.FindAll(x => x.Key == sysCh);
            if (inList.Count > 0)
            {
                foreach (var ch in inList)
                {
                    try
                    {
                        var outP = SysDevOutChList.Find(x => x.Key == sysCh);
                        int setOutCh = outP.Value;
                        int tarIOState = sInfoList.Find(x => x.SysCh == sysCh && x.OutPin == setOutCh).State;//_sysChIOSPDic[sysCh].State;
                        bool InState = SetIo2State(setOutCh, ch.Value, tarIOState);

                        if (!InState)
                        {
                            ErrorNumber = EDevErrorNumber.LASER_OpticalSwitch_Set_Err;
                            return false;
                        }
                        else
                        {
                            RelayInfo.ChCntAddOnce(1);//osw12設計比較特殊，因此這邊直接寫死
                            NowsysCh = sysCh;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("[OSW12],SetToSysCh exception:" + e.Message);
                        ErrorNumber = EDevErrorNumber.LASER_OpticalSwitch_Set_Err;
                        return false;
                    }
                }
            }
            return true;
        }

        public override void TurnOff()
        {
            //Connect.SendCommand(preString + ":ROUT:CLOSE");
        }
        #endregion

        #region
        private bool SetIo2State(int setPin,int watchPin,int tarState)
        {

            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();
            bool InState = false;
            for (int i = 0; !InState && i < 3; ++i)
            {
                int MsrtOutCh = (int)SMU.GetIONState(watchPin,Address);
                //SMU.GetIONState(Address);
                if (MsrtOutCh == tarState)
                {
                    InState = true;
                    break;
                }
                else
                {
                    SMU.SetIONState(setPin, true);
                    Thread.Sleep(3);
                    SMU.SetIONState(setPin, false);
                }
                Thread.Sleep(10);//延遲加上來，機台多買台
            }
            //long time = stopWatch.ElapsedMilliseconds;
            //Console.WriteLine(time.ToString());
            if (!InState)
            {
                ErrorNumber = EDevErrorNumber.LASER_OpticalSwitch_Set_Err;
                return false;
            }
            return true;

        }
        #endregion

    }
}
