using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Data.LaserData.LaserSource;
using MPI.Tester.Device.SourceMeter;

namespace MPI.Tester.Device.LaserSourceSys.OpticalSwitch
{
    public class OpticalSwitchManager
    {

        #region >>private property<<
        LaserSrcSysConfig _laserSysCfg;
        List<IOpticalSwitch> _osList;
        int _nowSysCh = int.MinValue;
        #endregion

        #region >>constructor<<
        public OpticalSwitchManager(LaserSrcSysConfig laserSysCfg)
        {
            _laserSysCfg = laserSysCfg.Clone() as LaserSrcSysConfig;

            ErrorNumber = EDevErrorNumber.Device_NO_Error;
            //AttSetting = null;
            _osList = new List<IOpticalSwitch>();

        }
        #endregion

        #region >>public property<<

        public EDevErrorNumber ErrorNumber { get; set; }

        public Dictionary<string, IConnect> AddressPortDic = new Dictionary<string, IConnect>();//處理單一IP連接多種儀器的狀況
        #endregion
        #region >>public method<<
        public bool Init(Dictionary<string, IConnect> apDic = null, Dictionary<string, object> shareresourceDic = null)
        {
            _osList = new List<IOpticalSwitch>();
            AddressPortDic = apDic;
            if (apDic == null)
            {
                AddressPortDic = new Dictionary<string, IConnect>();
            }
            if (_laserSysCfg != null && _laserSysCfg.OSList != null)
            {
                #region >> create OS Wrapper<<
                foreach (var osCfg in _laserSysCfg.OSList)
                {

                    if (!osCfg.Enable)
                    { continue; }
                    int sysCh = osCfg.LaserSysChannel;
                    int iCh = osCfg.OpticalInputChannel;
                    int oCh = osCfg.OpticalOutputChannel;
                    string add = osCfg.Address;
                    int slot = osCfg.Slot;

                    IOpticalSwitch tarOs = null;
                    foreach (var os1 in _osList)
                    {
                        if (os1.Address == add && os1.Slot == slot)
                        {
                            tarOs = os1;
                            break;
                        }
                    }

                    if (tarOs == null)
                    {
                        switch (osCfg.OpticalSwitchModel)
                        {                            
                            case EOpticalSwitchModel.SimuOS:
                                {
                                    tarOs = new SimuOS(osCfg);
                                }
                                break;
                            case EOpticalSwitchModel.FTBx_9160:
                                {
                                    tarOs = new FTBx_9160(osCfg);
                                }
                                break;
                            case EOpticalSwitchModel.OSW1xN:
                                {
                                    if (shareresourceDic != null && shareresourceDic.ContainsKey("K2600"))
                                    {
                                        Keithley2600 smu = shareresourceDic["K2600"] as Keithley2600;
                                        tarOs = new OSW1xN(osCfg, smu);
                                    }
                                    else
                                    {
                                        tarOs = null;
                                    }
                                }
                                break;
                            case EOpticalSwitchModel.OSW12:
                                {
                                    //continue;
                                    if (shareresourceDic != null && shareresourceDic.ContainsKey("K2600"))
                                    {
                                        Keithley2600 smu = shareresourceDic["K2600"] as Keithley2600;
                                        tarOs = new OSW12(osCfg, smu);
                                    }
                                    else
                                    {
                                        tarOs = null;
                                    }
                                }
                                break;
                            default:
                            case EOpticalSwitchModel.NONE:
                                continue;                                
                        }
                        _osList.Add(tarOs);
                    }

                    if (tarOs != null && 
                        tarOs.Spec.InputCh.InRange(iCh) && tarOs.Spec.OutputCh.InRange(oCh) )
                    {
                        //tarOs.Push(sysCh, iCh, oCh);

                        //同一組IO可能會用來驅動不同的switch，這樣會出現同一ip+slot驅動兩個相同sys channel的狀況
                        switch (osCfg.OpticalSwitchModel)
                        {
                            case EOpticalSwitchModel.OSW12:
                                tarOs.Push(sysCh, iCh, oCh);
                                (tarOs as OSW12).PushChIO(osCfg.LaserSysChannel, osCfg.OpticalOutputChannel, osCfg.IOState);
                                break;
                            case EOpticalSwitchModel.OSW1xN:
                                tarOs.Push(sysCh, iCh, oCh);
                                (tarOs as OSW1xN).PushChIO(osCfg.LaserSysChannel, osCfg.OpticalOutputChannel, osCfg.IOState, osCfg.WatchPinList);
                                break;
                            default:
                                if (!tarOs.SysChList.Contains(sysCh))
                                {
                                    tarOs.Push(sysCh, iCh, oCh);
                                }
                                else
                                {
                                    ErrorNumber = EDevErrorNumber.LASER_OpticalSwitch_Init_Err;
                                    return false;
                                }
                                break;
                        }
                        
                    }
                    else
                    {
                        ErrorNumber = EDevErrorNumber.LASER_OpticalSwitch_Init_Err;
                        return false;
                    }
                }
                #endregion

                #region >>init<<

                foreach (var os in _osList)
                {
                    IConnect cnnect1 = null;
                    bool newIP = true;
                    if (AddressPortDic.ContainsKey(os.Address))
                    {
                        cnnect1 = AddressPortDic[os.Address];
                        newIP = false;
                    }

                    if (!os.Init(os.Address, cnnect1))
                    {
                        ErrorNumber = os.ErrorNumber;
                        return false;
                    }
                    if (newIP && os.Connect != null)
                    {
                        AddressPortDic.Add(os.Address, os.Connect);
                    }
                }
                #endregion
            }
            return true;
        }

        public void TurnOff(List<int> sysChList = null)
        {
            foreach (var os in _osList)
            {
                if (sysChList == null)
                {
                    os.TurnOff();
                }
                else
                {
                    foreach (int sysCh in os.SysChList)
                    {
                        if (sysChList.Contains(sysCh))
                        {
                            os.TurnOff();
                        }
                    }
                }
            }
        }

        public void Close() 
        {
            foreach (var os in _osList)
            {
                os.Close();
            }
        }

        public bool SetParamToOS(int sysCh )
        {
            //if (_nowSysCh != sysCh)
            {
                _nowSysCh = sysCh;
                List<IOpticalSwitch> oList = GetOpticalSwitchFromSysCh(sysCh);
                if (oList != null)
                {
                    foreach (var os in oList)
                    {
                        List<int> opeSysChList = new List<int>();
                        if (os.SysChList.Contains(sysCh))
                        {                            
                            if (!os.SetToSysCh(sysCh))
                            {
                                ErrorNumber = os.ErrorNumber;
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public List<IOpticalSwitch> GetOpticalSwitchFromSysCh(int ch)
        {
            List<IOpticalSwitch> outList = new List<IOpticalSwitch>();
            if (_osList != null)
            {                
                foreach (IOpticalSwitch os in _osList)
                {
                    if (os.SysChList.Contains(ch))
                    {
                        outList.Add(os);
                    }
                }
            }
            return outList;

        }

        public void ForceSetRelay(Dictionary<string, object> devLog)
        {
            if (_osList != null && devLog != null)//目前只有OS有此需求
            {
                foreach (var dev in _osList)
                {
                    dev.ForceSetRelay(devLog);
                }
            }
        }
        #endregion

        #region >>private method<<

        private IOpticalSwitch Find(Predicate<IOpticalSwitch> match)
        {
            IOpticalSwitch osw = null;
            if (_osList != null)
            {
                osw = _osList.Find(match);
            }
            return osw;
        }
        #endregion
    }
}
