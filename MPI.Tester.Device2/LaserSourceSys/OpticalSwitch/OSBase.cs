using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using MPI.Tester.Tools;
using MPI.Tester.DeviceCommon.DeviceLogger;
using MPI.Tester.Device;
using MPI.Tester.DeviceCommon;
using MPI.Tester.Data.LaserData.LaserSource;

namespace MPI.Tester.Device.LaserSourceSys.OpticalSwitch
{
    public class OSBase:IOpticalSwitch
    {
        #region >>protected property<<
        protected string preString = "";
        #endregion

        #region >>constructor<<
        public OSBase()
        {
            Address = "";
            Slot = 0;
            NowsysCh = 0;
            ErrorNumber = EDevErrorNumber.Device_NO_Error;
            SerialNumber = "";
            SysDevOutChList = new List<KeyValuePair<int, int>>();
            SysDevInChList = new List<KeyValuePair<int, int>>();
            Spec = new OpticalSwitchSpec();
            Spec.InputCh = new MinMaxValuePair<int>(1, 4);
            Spec.OutputCh = new MinMaxValuePair<int>(1, 1);
            SysChList = new List<int>();
            Connect = null;
        }

        public OSBase( OpticalSwitchConfig OSCfg):this()
        {

            Address =OSCfg.Address;
            Slot = OSCfg.Slot;
            ErrorNumber = EDevErrorNumber.Device_NO_Error;
            SerialNumber = "";
            RelayInfo = new DeviceRelayInfoBase("OSBase",0);

        }
        
        #endregion

        #region >>public property<<

        public string Address { get; set; }
        public int Slot { get; set; }
        public int NowsysCh { get; set; }
        public string SerialNumber { get; set; }
        
        public List<KeyValuePair<int, int>> SysDevOutChList { get; set; }
        public List<KeyValuePair<int, int>> SysDevInChList { get; set; }
        public List<int> SysChList { get; set; }
        public EDevErrorNumber ErrorNumber { get; set; }
        public OpticalSwitchSpec Spec { set; get; }
        public IConnect Connect { get; set; }
        public DeviceRelayInfoBase RelayInfo
        {
            get
            {
                if (LogTool != null && LogTool.LogObject != null)
                { return LogTool.LogObject; }
                return null;
            }
            set
            {
                if (LogTool != null )
                { LogTool.LogObject = value; }
            }
        }
        public DeviceRelayCountTool<DeviceRelayInfoBase> LogTool;
        #endregion

        #region
        public virtual bool Init(string address,IConnect connect)
        {
            Console.WriteLine("[OSBase],Init()");
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

                //RelayInfo.RelayCntDic.Add(chPair.Value.ToString(),0);//這邊根本還沒起始化RelayInfo
            }
            if (Connect == null)
            {
                bool isOnlyIPAddressNum = MPIFile.IsAccessableIP(address);

                if (isOnlyIPAddressNum)
                {
                    LANSettingData lanData = new LANSettingData();

                    lanData.IPAddress = address;

                    Connect = new LANConnect(lanData);
                }
                else
                {
                    Connect = new IVIConnect(address);
                }
            }
            else
            {
                Connect = connect;
                return true;
            }
            string msg = string.Empty;

            Address = address;

            if (!Connect.Open(out msg))
            {
                if (!Connect.Open(out msg))//因為N7761連線非常慢，有時會連線逾時，因此用這個方法來避免
                {
                    ErrorNumber = EDevErrorNumber.LASER_OpticalSwitch_Init_Err;
                }

                return false;
            }

            return true;
        }

        public virtual void Close()
        {
            if (LogTool != null)
            {
                LogTool.Serializ();
            }
        }

        public virtual bool SetToSysCh(int sysCh = 1)
        {
            var inList = SysDevInChList.FindAll(x => x.Key == sysCh);
            var outList = SysDevOutChList.FindAll(x => x.Key == sysCh);
            if (inList.Count > 0 && outList.Count > 0)
            {
                NowsysCh = sysCh;
                
                foreach (var ch in outList)
                {
                    RelayInfo.ChCntAddOnce(ch.Value);
                }
                return true;
            }

            return false;
        }

        public virtual void TurnOff()
        { }

        public virtual bool Push(int sysCh, int inCh, int outCh)
        {
            SysDevInChList.Add(new KeyValuePair<int, int>(sysCh, inCh));
            SysDevOutChList.Add(new KeyValuePair<int, int>(sysCh, outCh));
            SysChList.Add(sysCh);
            return true;
        }

        public void ForceSetRelay(Dictionary<string, object> devLog)
        {
            try
            {
                bool modified = false;
                DeviceRelayInfoBase temp = this.RelayInfo.Clone() as DeviceRelayInfoBase;
                if (LogTool != null && LogTool.LogObject != null && devLog != null)
                {
                    foreach (var dev in devLog)
                    {
                        if (SerialNumber == dev.Key)
                        {
                            DeviceRelayInfoBase dInfo = dev.Value as DeviceRelayInfoBase;
                            foreach (var p in dInfo.RelayCntDic)
                            {
                                if (temp.RelayCntDic.ContainsKey(p.Key))
                                {
                                    if (temp.RelayCntDic[p.Key] != p.Value)
                                    {
                                        modified = true;
                                        temp.RelayCntDic[p.Key] = p.Value;
                                    }
                                }                                
                            }
                        }
                    }

                    if (modified)
                    {
                        LogTool.SerializToXml();
                        LogTool.LogObject = temp;
                    }


                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[OSBase],ForceSetRelay(),exception: " + e.Message);
            }
        }
        #endregion
    }
  
}

