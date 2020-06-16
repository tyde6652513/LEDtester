using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Device.SourceMeter;
using MPI.Tester.DeviceCommon;
using MPI.Tester.Data.LaserData.LaserSource;

namespace MPI.Tester.Device.LaserSourceSys.PowerMeter
{
    public class PowerMeterManager
    {
        #region >>private property<<
        LaserSrcSysConfig _laserSysCfg;
        List<IPowerMeter> _pmList;
        #endregion

        #region >>constructor<<
        public PowerMeterManager(LaserSrcSysConfig laserSysCfg)
        {
            _laserSysCfg = laserSysCfg.Clone() as LaserSrcSysConfig;

            ErrorNumber = EDevErrorNumber.Device_NO_Error;
            //AttSetting = null;
            _pmList = new List<IPowerMeter>();

        }
        #endregion

        #region >>public property<<

        public EDevErrorNumber ErrorNumber { get; set; }

        //public AttenuatorSettingData AttSetting { get; set; }//目前假設不會在同一Recipe中切換衰減器設定

        public Dictionary<string, IConnect> AddressPortDic = new Dictionary<string, IConnect>();//處理單一IP連接多種儀器的狀況
        #endregion
        #region >>public method<<
        public bool Init(Dictionary<string, IConnect> apDic = null, Dictionary<string, object> shareresourceDic = null)
        {
            AddressPortDic = apDic;
            if (apDic == null)
            {
                AddressPortDic = new Dictionary<string, IConnect>();
            }

            _pmList = new List<IPowerMeter>();
            if (_laserSysCfg != null && _laserSysCfg.PowerMeterList != null)
            {             

                #region >> create Att Wrapper<<
                foreach (var pmCfg in _laserSysCfg.PowerMeterList)
                {
                    if (!pmCfg.Enable)
                    { continue; }
                    int lCh = pmCfg.LaserSysChannel;
                    int mCh = pmCfg.PowerMeterChannel;
                    string add = pmCfg.Address;
                    int slot = pmCfg.Slot;

                    IPowerMeter tarpm= null;
                    foreach (var pm1 in _pmList)
                    {
                        if (pm1.Address == add && pm1.Slot == slot)
                        {
                            tarpm = pm1;
                            break;
                        }
                    }

                    if (tarpm == null)
                    {
                        switch (pmCfg.PowerMeterModel)
                        {
                            case EPowerMeter.PM101:
                            case EPowerMeter.PM400:
                                {
                                    tarpm = new PM400(pmCfg);
                                }
                                break;
                            case EPowerMeter.SimuPowerMeter:
                                {
                                    tarpm = new SimuPowerMeter(pmCfg);
                                }
                                break;
                            case EPowerMeter.FTBx_1750:
                                {
                                    tarpm = new FTBx_1750(pmCfg);
                                }
                                break;
                            case EPowerMeter.K2600:
                                {
                                    if (shareresourceDic != null && shareresourceDic.ContainsKey("K2600"))
                                    {
                                        Keithley2600 smu = shareresourceDic["K2600"] as Keithley2600;
                                        tarpm = new K2600PowerMeter(pmCfg, smu);
                                    }
                                }
                                break;
                            default:
                            case EPowerMeter.NONE:
                                continue;
                                
                        }
                        if (tarpm != null)
                        {
                            _pmList.Add(tarpm);
                        }
                    }


                    if (!(tarpm != null && tarpm.Push(lCh, mCh)))
                    {
                        ErrorNumber = tarpm.ErrorNumber;
                        return false;
                    }                  
                }
                #endregion

                #region >>init<<

                foreach (var pm in _pmList)
                {
                    bool newIP = true;
                    IConnect cnnect1 = null;
                    if (AddressPortDic.ContainsKey(pm.Address))
                    {
                        cnnect1 = AddressPortDic[pm.Address];
                        newIP = false;
                    }

                    if (!pm.Init(cnnect1))
                    {
                        ErrorNumber = pm.ErrorNumber;
                        return false;
                    }
                    if (newIP && pm.Connect != null)
                    {
                        AddressPortDic.Add(pm.Address, pm.Connect);
                    }
                }
                #endregion
            }
            return true;
        }

        public void TurnOff(List<int> sysChList = null)
        {
            foreach (var pm in _pmList)
            {
                if (sysChList == null)
                {
                    pm.TurnOff();
                }
                else
                {
                    foreach (int sysCh in pm.SysChDevDic.Keys)
                    {
                        if (sysChList.Contains(sysCh))
                        {
                            pm.TurnOff();
                        }
                    }
                }
            }
        }

        public void Close()
        {
            foreach (var att in _pmList)
            {
                att.Close();
            }
        }


        public double GetMsrtPower(PowerMeterSettingData pms,int polar )
        {
            double value = 0;
            int sysCh = pms.SysChannel;

            foreach (var pm in _pmList)
            {
                if (pm.SysChDevDic.ContainsKey(sysCh))
                {
                    value = pm.GetMsrtPower(pms, polar);
                    if (pm.ErrorNumber != EDevErrorNumber.Device_NO_Error)
                    {
                        ErrorNumber = pm.ErrorNumber;
                    }
                    break;
                }
            }
            return value;
        }

        public IPowerMeter GetPowerMeterFromSysCh(int ch)
        {
            if (_pmList != null)
            {
                foreach (IPowerMeter pm in _pmList)
                {
                    if (pm.SysChDevDic.ContainsKey(ch))
                    {
                        return pm;
                    }
                }
            }
            return null;

        }
        #endregion

        #region >>private method<<

        private IPowerMeter Find(Predicate<IPowerMeter> match)
        {
            IPowerMeter pm = null;
            if (_pmList != null)
            {
                pm = _pmList.Find(match);
            }
            return pm;
        }
        #endregion
    }
}
