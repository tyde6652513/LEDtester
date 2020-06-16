


using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Threading;

using MPI.Tester.Tools;
using MPI.Tester.Device;
using MPI.Tester.DeviceCommon;
using MPI.Tester.DeviceCommon.DeviceLogger;
using MPI.Tester.Data.LaserData.LaserSource;
using MPI.Tester.Device.SourceMeter;

namespace MPI.Tester.Device.LaserSourceSys.OpticalSwitch
{
    public class OSW1xN : OSBase
    {
        //Dictionary<int, StatePinListPair> _sysChIOSPDic = new Dictionary<int, StatePinListPair>();
        List<SwitchInfo> sInfoList = new List<SwitchInfo>();

        public OSW1xN()
            : base()
        {
            Spec.InputCh = new MinMaxValuePair<int>(1, 14);
            Spec.OutputCh = new MinMaxValuePair<int>(1, 14);
         }
         public OSW1xN(OpticalSwitchConfig OSCfg)
            : base(OSCfg)
        {
            Spec.InputCh = new MinMaxValuePair<int>(1, 14);
            Spec.OutputCh = new MinMaxValuePair<int>(1, 14);
            Address = OSCfg.Address;
        }

         public OSW1xN(OpticalSwitchConfig OSCfg, Keithley2600 smu = null)
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
            Console.WriteLine("[OSW1xN],Init()");
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

            SerialNumber = "OSW1xN";
            foreach (var chPair in SysDevOutChList)
            {
                SerialNumber += "_ch" + chPair.Value.ToString();
            }
            Console.WriteLine("[OSW1xN],Init(),SerialNumber = " + SerialNumber);

            #region >>logobj load<<
            string deviceName = "OSW1xN_" + SerialNumber;

            try
            {
                LogTool = new DeviceRelayCountTool<DeviceRelayInfoBase>(MPI.Tester.Data.Constants.Paths.LASER_LOG, deviceName);
                LogTool.Deserialize();

                if (LogTool.LogObject == null || LogTool.LogObject.RelayCntDic == null || LogTool.LogObject.RelayCntDic.Count == 0)
                {
                    LogTool.BackupXMLLog();
                    LogTool.LogObject = new DeviceRelayInfoBase(deviceName, Spec.OutputCh.Max);
                }
                Console.WriteLine("[OSW1xN],Init(),LogTool.Deserialize()");
            }
            catch (Exception e)
            {

                Console.WriteLine("[OSW1xN],Init(),LogTool.Deserialize(),Exception:" + e.Message);
                LogTool.BackupXMLLog();
            }
            #endregion


            return true;
        }

        public void PushChIO(int sysCh,int outPin, int ioState,List<int> pinList)
        {
            //ioState = ioState==1 ? 1 : 0;//方便UI層可以直接將io state設定為通道編號
            int index = sInfoList.FindIndex(x => x.SysCh == sysCh && x.OutPin == outPin);
            if (index >= 0)
            {
                sInfoList[index] = new SwitchInfo(sysCh, outPin, ioState, pinList);
            }
            else
            {
                sInfoList.Add(new SwitchInfo(sysCh, outPin, ioState, pinList));
            }

            foreach (int p in pinList)//強迫設為high，IO才能正確讀取狀態
            {
                SMU.SetIOInitState(p, true, Address);
            }
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
                        var sInfo = sInfoList.Find(x => x.SysCh == sysCh && x.OutPin == setOutCh);
                        bool InState = SetIo2State(sInfo);

                        if (!InState)
                        {
                            ErrorNumber = EDevErrorNumber.LASER_OpticalSwitch_Set_Err;
                            return false;
                        }
                        else
                        {
                            RelayInfo.ChCntAddOnce(1);//OSW1xN設計比較特殊，因此這邊直接寫死
                            NowsysCh = sysCh;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("[OSW1xN],SetToSysCh exception:" + e.Message);
                        ErrorNumber = EDevErrorNumber.LASER_OpticalSwitch_Set_Err;
                        return false;
                    }
                }
            }
            return true;
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

        public override void TurnOff()
        {
            //Connect.SendCommand(preString + ":ROUT:CLOSE");
        }
        #endregion

        #region
        private bool SetIo2State(SwitchInfo sInfo)//(int setPin,int watchPin,int tarState)
        {

            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();
            bool InState = false;
            int tarState = sInfo.State;

            for (int i = 0; !InState && i < 65; ++i)//預防未來出現64 channel切換的版本
            {
                //int MsrtOutCh = (int)SMU.GetIONState(watchPin);

                byte[] bArr = SMU.GetIONState(Address);
                int MsrtOutCh = 0;

                for (int id = 0; id < sInfo.WatchPinList.Count; ++id)
                {
                    int pin = sInfo.WatchPinList[id];
                    int inverseBit = bArr[pin - 1] == 1 ? 0 : 1;//因電路設計關係，真值表的0,1分別代表True/False
                    /*
                    0 = 1,1
                    1 = 1,0
                    2 = 0,1
                    3 = 0,0
                    */
                    MsrtOutCh += (int)(Math.Pow(2, id) * inverseBit);
                }
                
                if (MsrtOutCh == tarState-1)
                {
                    InState = true;
                    break;
                }
                else
                {
                    SMU.SetIONState(sInfo.OutPin, true);
                    Thread.Sleep(3);
                    SMU.SetIONState(sInfo.OutPin, false);
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
    public class SwitchInfo
    {
        public int SysCh = -1;
        public int State = -1;
        public int OutPin = -1;//1xN會需要複數個InPin，不適合做為辨別
        public List<int> WatchPinList = new List<int>();

        public SwitchInfo()
        { }
        public SwitchInfo(int sCh, int oPin, int state, List<int> wpList = null)
        {
            SysCh = sCh;
            State = state;
            OutPin = oPin;
            if (wpList != null)
            {
                WatchPinList = new List<int>();
                WatchPinList.AddRange(wpList.ToArray());
            }
        }
    }

    //struct StatePinListPair 
    //{
    //    KeyValuePair<int, List<int>> _statePinPair;
    //    public StatePinListPair(int state, List<int> pinList)
    //    {
    //        _statePinPair = new KeyValuePair<int, List<int>>(state, pinList);
    //    }
    //    public int State { get { return _statePinPair.Key; } }
    //    public List<int> PinList { get { return _statePinPair.Value; } }
    //}
}

