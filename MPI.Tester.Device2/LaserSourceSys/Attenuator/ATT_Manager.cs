using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Data.LaserData.LaserSource;

namespace MPI.Tester.Device.LaserSourceSys.Attenuator
{
    public class ATT_Manager 
    {
        #region >>private property<<
        LaserSrcSysConfig _laserSysCfg;
        List<IAttenuator> _attList;
        #endregion

        #region >>constructor<<
        public ATT_Manager(LaserSrcSysConfig laserSysCfg)
        {
            _laserSysCfg = laserSysCfg.Clone() as LaserSrcSysConfig;

            ErrorNumber = EDevErrorNumber.Device_NO_Error;
            AttSetting = null;
            _attList = new List<IAttenuator>();

        }
        #endregion

        #region >>public property<<

        public EDevErrorNumber ErrorNumber { get; set; }

        public AttenuatorSettingData AttSetting { get; set; }//目前假設不會在同一Recipe中切換衰減器設定

        public Dictionary<string, IConnect> AddressPortDic = new Dictionary<string, IConnect>();//處理單一IP連接多種儀器的狀況
        #endregion

        #region >>public method<<
        public bool Init(Dictionary<string, IConnect> apDic = null)
        {
            AddressPortDic = apDic;
            if (apDic == null)
            {
                AddressPortDic = new Dictionary<string, IConnect>();
            }

            _attList = new List<IAttenuator>();
            if (_laserSysCfg != null && _laserSysCfg.AttList != null)
            {
                if (_laserSysCfg.Attenuator != null)//向舊版相容
                {
                    string add1 = _laserSysCfg.Attenuator.Address;
                    ELaserAttenuatorModel type =  _laserSysCfg.Attenuator.AttenuatorModel;
                    int slot1 = _laserSysCfg.Attenuator.Slot;
                    if (_laserSysCfg.ChConfigList.FindIndex(x => x.SysChannel == 1) < 0 &&
                        _laserSysCfg.AttList.FindIndex(x => x.Address == add1 && x.Slot == slot1 && x.AttenuatorModel == type) < 0)
                    {
                        _laserSysCfg.Attenuator.LaserSysChannel = 1;
                        _laserSysCfg.Attenuator.AttChannel = 1;
                        LaserSrcChConfig chCfg = new LaserSrcChConfig();
                        chCfg.AttConfig = _laserSysCfg.Attenuator;
                        _laserSysCfg.ChConfigList.Add(chCfg);
                        
                    }
                }

                #region >> create Att Wrapper<<
                foreach (var attCfg in _laserSysCfg.AttList)
                {
                    if (!attCfg.Enable)
                    {
                        continue;
                    }
                    int lCh = attCfg.LaserSysChannel;
                    int aCh = attCfg.AttChannel;
                    string add = attCfg.Address;
                    int slot = attCfg.Slot;

                    IAttenuator tarAtt = null;
                    foreach (var att1 in _attList)
                    {
                        if (att1.Address == add && att1.Slot == slot)
                        {
                            tarAtt = att1;
                            break;
                        }
                    }

                    if (tarAtt == null)
                    {
                        switch (attCfg.AttenuatorModel)
                        {
                            case ELaserAttenuatorModel.N7760A:
                                {
                                    tarAtt = new N7760A(attCfg);
                                }
                                break;
                            case ELaserAttenuatorModel.FTBx_3500:
                                {
                                    tarAtt = new FTBx_3500(attCfg);
                                }
                                break;
                            case ELaserAttenuatorModel.SimuAtt:
                                {
                                    tarAtt = new SimuAtt(attCfg);
                                }
                                break;
                            default:
                            case ELaserAttenuatorModel.NONE:
                                continue;
                                break;
                        }
                        _attList.Add(tarAtt);
                    }


                    if (tarAtt.Spec.Channel.InRange(aCh) && !tarAtt.SysDevChDic.ContainsKey(lCh) && !tarAtt.SysDevChDic.ContainsValue(aCh))
                    {
                        tarAtt.SysDevChDic.Add(lCh, aCh);
                    }
                    else
                    {
                        //set channel err
                    }
                }
            }
            #endregion

            #region >>init<<

            foreach (var att in _attList)
            {
                bool newIP = true;
                IConnect cnnect1 = null;
                if (AddressPortDic.ContainsKey(att.Address))
                {
                    cnnect1 = AddressPortDic[att.Address];
                    newIP = false;
                }

                if (!att.Init(att.Address, cnnect1))//保持舊介面相容用
                {
                    ErrorNumber = att.ErrorNumber;
                    return false;
                }
                if (newIP)
                {
                    AddressPortDic.Add(att.Address, att.Connect);
                }
            }
            #endregion

            return true;
        }

        public void TurnOff(List<int> sysChList = null)
        {
            foreach (var att in _attList)
            {
                if (sysChList == null)
                {
                    att.TurnOff();
                }
                else
                {
                    foreach (int sysCh in att.SysDevChDic.Keys)
                    {
                        if (sysChList.Contains(sysCh))
                        {
                            att.TurnOff();
                        }
                    }
                }
            }
        }

        public void Close() 
        {
            foreach (var att in _attList)
            {
                att.Close();
            }
        }

        public bool SetParamToAttenuator(List<AttenuatorSettingData> paramSetting)
        {
            foreach (var att in _attList)
            {
                List<AttenuatorSettingData> selPara = (from para in paramSetting
                                                       where att.SysDevChDic.ContainsKey(para.SysChannel)
                                                       select para).ToList();
                if (selPara != null)
                {
                    if (!att.SetParamToAttenuator(selPara))
                    {
                        ErrorNumber = att.ErrorNumber;
                        return false;
                    }
                }
                
            }
            return true;
        }

        public double GetMsrtPower(int sysCh = 1, ELaserPowerUnit unit = ELaserPowerUnit.W)
        {
            double value  = 0;

            foreach (var att in _attList)
            {
                if (att.SysDevChDic.ContainsKey(sysCh))
                {
                    value = att.GetMsrtPower(sysCh, unit);
                    break;
                }
            }
            return value;
        }

        public MinMaxValuePair<double> GetOutputPowerRangeIndBm(int sysCh = 1)
        {
            MinMaxValuePair<double> range = new MinMaxValuePair<double>(-70,10);

            foreach (var att in _attList)
            {
                if (att.SysDevChDic.ContainsKey(sysCh))
                {
                    range = att.GetOutputPowerRangeIndBm(sysCh);
                    break;
                }
            }
            return range;
        }

        public IAttenuator GetAttFromSysCh(int ch)
        {
            if (_attList != null)
            {
                foreach (IAttenuator att in _attList)
                {
                    if (att.SysDevChDic.ContainsKey(ch))
                    {
                        return att;
                    }
                }
            }
            return null;
            
        }
        #endregion

        #region >>private method<<

        private IAttenuator Find(Predicate<IAttenuator> match)
        {
            IAttenuator att = null;
            if (_attList != null)
            {
                att = _attList.Find(match);
            }
            return att;
        }
        #endregion
    }
}
