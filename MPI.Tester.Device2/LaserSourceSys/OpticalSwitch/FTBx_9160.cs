using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

using MPI.Tester.Tools;
using MPI.Tester.Device;
using MPI.Tester.DeviceCommon;
using MPI.Tester.Data.LaserData.LaserSource;
using MPI.Tester.DeviceCommon.DeviceLogger;

namespace MPI.Tester.Device.LaserSourceSys.OpticalSwitch
{
    public class FTBx_9160 : OSBase
    {
        
        public FTBx_9160()
            : base()
        { }
        public FTBx_9160(OpticalSwitchConfig OSCfg)
            : base(OSCfg)
        {
            preString = "LINS" + Slot.ToString();
            Spec.InputCh = new MinMaxValuePair<int>(1, 4);
        }

        #region
        public override bool Init(string address, IConnect conect)
        {
            Console.WriteLine("[FTBx_9160],Init()");
            Connect = conect;

            if(!base.Init(address, conect))
            {
                return false;
            }

            if (!TestIfSlotExist())
            {
                Console.WriteLine("[FTBx_9160],Init(),slot not exist");
                ErrorNumber = EDevErrorNumber.LASER_OpticalSwitch_Slot_Not_Exist_Err;
                return false;
            }

            SerialNumber = "LINS" + Slot.ToString();

            string msg = "";


            try
            {
                Connect.SendCommand(preString + ":SNUM?");

                Connect.WaitAndGetData(out msg);

                //string[] data = msg.Trim(new char[] { '\n', '\"' }).Split(',');

                //if (data == null || data.Length != 1)
                //{
                //    ErrorNumber = EDevErrorNumber.LASER_OpticalSwitch_Init_Err;
                //    return false;
                //}

                SerialNumber = msg.Trim(new char[] { '\n', '\"' ,' '});

                Console.WriteLine("[FTBx_9160],Init(),SerialNumber = " + SerialNumber);
            }
            catch (Exception e)
            {
                Console.WriteLine("[FTBx_9160],Init(),Ask SN ,Exception:" + e.Message);
            }

            #region>>spec<<
            Spec.InputCh = new MinMaxValuePair<int>(1, 2);
            Spec.OutputCh = new MinMaxValuePair<int>(1, 1);

            msg = preString +  ":ROUT:PATH:CAT?";
            string[] strArr = msg.Split('x');
            int val1 = 1, val2 = 2;
            if (strArr.Length == 2&&
                int.TryParse(strArr[0],out val1) &&
                int.TryParse(strArr[1], out val2))
            {
                int max = (int)Math.Max(val1, val2);
                int min = (int)Math.Min(val1, val2);

                Spec.InputCh = new MinMaxValuePair<int>(1, max);
                Spec.OutputCh = new MinMaxValuePair<int>(1, min);
            }
            #endregion

            Connect.SendCommand(preString + ":ROUT:SCAN:ADJ:AUTO ON");//optical switch定位精度會比較好

            Connect.SendCommand(preString + ":ROUT:OPEN");//shutter on/off

            Connect.SendCommand(preString + ":ROUT:SCAN:SYNC 1");

            #region >>logobj load<<

            string deviceName = "FTBx_9160_" + SerialNumber;

            try
            {
                LogTool = new DeviceRelayCountTool<DeviceRelayInfoBase>(MPI.Tester.Data.Constants.Paths.LASER_LOG, deviceName);
                LogTool.Deserialize();

                if (LogTool.LogObject == null || LogTool.LogObject.RelayCntDic == null || LogTool.LogObject.RelayCntDic.Count == 0)
                {
                    LogTool.BackupXMLLog();
                    LogTool.LogObject = new DeviceRelayInfoBase(deviceName, Spec.OutputCh.Max);
                }
            }
            catch (Exception e)
            {

                Console.WriteLine("[FTBx_9160],Init(),LogTool.Deserialize(),Exception:" + e.Message);
                LogTool.BackupXMLLog();
            }
            Console.WriteLine("[FTBx_9160],Init(),LogTool.Deserialize()");

            #endregion
            return true;
        }

        public virtual void Close()
        {
            TurnOff();
        }

        public override bool SetToSysCh(int sysCh = 1)
        {
            var inList = SysDevInChList.FindAll(x => x.Key == sysCh);
            if (inList.Count > 0)
            {
                NowsysCh = sysCh;
                //先暫時不處理2in N out 機種的狀況
                foreach (var ch in inList)
                {
                    Connect.SendCommand(preString + ":ROUT:SCAN " + ch.Value.ToString());//shutter on/off

                    Thread.Sleep(10);//延遲加上來，機台多買台

                    //WaitActionFin(30);


					string msg =  preString + ":STATUS?";

                    Connect.SendCommand(msg);//shutter on/off
                    
                    Connect.WaitAndGetData(out msg);
                    //int devCh = -1;
                    //if (int.TryParse(msg.Trim(new char[] { '\n', ' ' }), out devCh) && devCh == ch.Value)
                    if (msg.Contains("READY"))
                    {
                        RelayInfo.ChCntAddOnce(ch.Value);
                    }
                    else
                    {
                        if (msg == null)
                        {
                            msg = "null";
                        }
                        Console.WriteLine("[FTBx_9160],SetToSysCh(),query state,echo: " + msg);
                        ErrorNumber = EDevErrorNumber.LASER_OpticalSwitch_Set_Err;
                        return false;
                    }
                    
                }
 
            }
            return true;
        }

        public override void TurnOff()
        {
            Connect.SendCommand(preString + ":ROUT:CLOSE");
        }
        #endregion

        #region >>private method<<
        private bool WaitActionFin(int milisec)
        {
            bool result = false;
            Stopwatch sw = new Stopwatch();

            sw.Start();
            string msg = "";

            while (!result && sw.ElapsedMilliseconds < (long)milisec)
            {
                msg = "STATUS?";
                Connect.SendCommand(msg);
                Connect.WaitAndGetData(out msg);
                if (msg.Trim() != "BUSY")
                {
                    result = true;
                }
            }

            return result;
        }

        private bool TestIfSlotExist()
        {
            string msg = "";
            Connect.SendCommand("INST:CAT:FULL?");//編號會按照LINS設定

            Connect.WaitAndGetData(out msg);

            bool isSlotExist = false;

            if (msg != null && msg != "")
            {
                string[] sArr = msg.Split(',');
                foreach (string str1 in sArr)
                {
                    if (str1.Trim() == Slot.ToString())
                    {
                        isSlotExist = true;
                        break;
                    }
                }
            }
            return isSlotExist;
        }
        #endregion

    }
}