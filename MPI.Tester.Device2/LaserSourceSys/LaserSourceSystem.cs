using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Data;
using MPI.Tester.Data.LaserData.LaserSource;
using MPI.Tester.Device.LaserSourceSys.Attenuator;
using MPI.Tester.Device.LaserSourceSys.OpticalSwitch;
using MPI.Tester.Device.LaserSourceSys.PowerMeter;

namespace MPI.Tester.Device.LaserSourceSys
{
    public class LaserSourceSystem
    {      
        LaserSrcSysConfig _config;

        public ATT_Manager AttManager;
        public OpticalSwitchManager OSManager;
        public PowerMeterManager PowerMeterManager;
        public ILaserSource LaserSource { set; get; }
        public bool UseSMUPdMonitor { set; get; }
        public EDevErrorNumber ErrorNumber { set; get; }
        
        #region >>constuctor<<
        public LaserSourceSystem()
        {
            ErrorNumber = EDevErrorNumber.Device_NO_Error;
            LaserSource = null;
            AttManager = null;
            //Attenuator = null;
            UseSMUPdMonitor = false;
            _config = new LaserSrcSysConfig();
        }
        #endregion

        #region >>public method<<
        /// <summary>
        /// 起始化,放在SMU後面才能註冊SMU的IO
        /// </summary>
        public bool Init(LaserSrcSysConfig config, Dictionary<string, object> shareresourceDic = null)
        {
            if (config.MoniterPDSMU != null && config.MoniterPDSMU.Model != "")
            {
                UseSMUPdMonitor = true;
            }

            _config = config.Clone() as LaserSrcSysConfig ;

            Dictionary<string, IConnect> addConnectDic = new Dictionary<string, IConnect>();

            //因為ThorLa的MP400在其他TCPIP連線後被啟動會造成當機(WSAStartup起始化失敗) <=c++ lib? WTF?
            #region>>PowerMeter<<
            PowerMeterManager = new PowerMeterManager(_config);

            if (PowerMeterManager.Init(addConnectDic, shareresourceDic) == false)
            {
                if (PowerMeterManager.ErrorNumber != EDevErrorNumber.Device_NO_Error)
                {
                    ErrorNumber = PowerMeterManager.ErrorNumber;
                    return false;
                }
            }
            #endregion

            #region >>AttManager<<
            AttManager = new ATT_Manager(_config);
            if (AttManager.Init(addConnectDic) == false)
            {
                ErrorNumber = AttManager.ErrorNumber;
                AttManager = null;
                return false;
            }
            #endregion

            #region >>OpticalSwitch<<
            OSManager = new OpticalSwitchManager(_config);

            if (OSManager.Init(addConnectDic, shareresourceDic) == false)
            {
                ErrorNumber = OSManager.ErrorNumber;
                OSManager = null;
                return false;
            }
            #endregion


            return true;
        }
       
        public bool TurnOff(List<int> sysChList = null)
        {
            if (AttManager != null)
            {
                AttManager.TurnOff(sysChList);
            }
            if (OSManager != null)
            {
                OSManager.TurnOff(sysChList);
            }
            if(PowerMeterManager != null)
            {
                PowerMeterManager.TurnOff(sysChList);
            }
            return true;
        }

        public bool TurnOffAttenuator()
        {
            return true;
        }

        public bool TurnOffLaser()
        {
            return true;
        }

        public bool Close()
        {
            if (AttManager != null)
            {
                AttManager.Close();
            }
            if (OSManager != null)
            {
                OSManager.Close();
            }
             if(PowerMeterManager != null)
            {
                PowerMeterManager.Close();
            }
            return true;
        }

        public Dictionary<int, LaserSourceSpec> GetChSpecDic()
        {
            Dictionary<int, LaserSourceSpec> chSpecDic = new Dictionary<int, LaserSourceSpec>();
            if(_config.ChConfigList != null)
            {
                foreach (var ch in _config.ChConfigList)
                {
                    int sysCh = ch.SysChannel;
                    LaserSourceSpec lsSpec = new LaserSourceSpec();
                    
                    if (AttManager.GetAttFromSysCh(sysCh) != null)
                    {
                        AttenuatorSpec attspec = AttManager.GetAttFromSysCh(sysCh).Spec.Clone() as AttenuatorSpec;
                        lsSpec.AttSpec = attspec;
                    }

                    if (OSManager.GetOpticalSwitchFromSysCh(sysCh) != null)
                    {
                        List < IOpticalSwitch >osLit = OSManager.GetOpticalSwitchFromSysCh(sysCh);
                        List<OpticalSwitchSpec> specList = new List<OpticalSwitchSpec>();
                        foreach (var os in osLit)
                        {
                            specList.Add(os.Spec.Clone() as OpticalSwitchSpec);
                        }
                        lsSpec.OpticalSwitchSpec = new List<OpticalSwitchSpec>();
                        lsSpec.OpticalSwitchSpec.AddRange(specList.ToArray()); 
                    }
                    if (PowerMeterManager.GetPowerMeterFromSysCh(sysCh) != null)
                    {
                        PowerMeterSpec pmspec = PowerMeterManager.GetPowerMeterFromSysCh(sysCh).Spec.Clone() as PowerMeterSpec;
                        lsSpec.PowerMeterSpec = pmspec;
                    }
                    chSpecDic.Add(sysCh, lsSpec);
                }
            }
            

            return chSpecDic;
        }

        public Dictionary<string, object> GetDevLog()
        {
            Dictionary<string, Object> chLogDic = new Dictionary<string, Object>();
            if (_config.ChConfigList != null)
            {
                foreach (var ch in _config.ChConfigList)
                {
                    int sysCh = ch.SysChannel;

                    if (OSManager.GetOpticalSwitchFromSysCh(sysCh) != null)
                    {
                        List<IOpticalSwitch> osLit = OSManager.GetOpticalSwitchFromSysCh(sysCh);
                        foreach (var os in osLit)
                        {
                            if (!chLogDic.ContainsKey(os.SerialNumber))
                            {
                                if (os is OSBase)
                                {
                                    chLogDic.Add(os.SerialNumber, (os as OSBase).RelayInfo.Clone() );
                                }
                            }
                            //specList.Add(os.Spec.Clone() as OpticalSwitchSpec);
                        }
                    }

                }
            }

            return chLogDic;
        }

        public void SetDevLog(Dictionary<string, object> devLogDic)
        {
            if (OSManager != null)
            {
                OSManager.ForceSetRelay(devLogDic);
            }
        
        }

        public bool SetParamToAttenuator(List<AttenuatorSettingData> paramSetting)
        {
            if (AttManager != null)
            {
                if (!AttManager.SetParamToAttenuator(paramSetting))
                {
                    ErrorNumber = AttManager.ErrorNumber;
                    return false;
                }
            }
            return true;
        }

        public bool SetParamToOS(int sysCh)
        {
            if (OSManager != null)
            {
                if (!OSManager.SetParamToOS(sysCh))
                {
                    ErrorNumber = OSManager.ErrorNumber;
                    return false;
                }                
            }
            return true;
        }
        #endregion
    }


}
