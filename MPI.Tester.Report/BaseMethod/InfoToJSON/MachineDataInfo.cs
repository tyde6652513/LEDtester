

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;
using Newtonsoft.Json;
using MPI.Tester.Data.LaserData.LaserSource;

namespace MPI.Tester.Report.BaseMethod.InfoToJSON
{
    public class MachineDataInfo
    {
       MachineConfig _mCfg = new MachineConfig();
       MachineInfoData _mInfo = new MachineInfoData();
       public MachineDataInfo(MachineConfig machineCfg , MachineInfoData machineInfo)
       {
            _mCfg = machineCfg;
            _mInfo = machineInfo;
       }

       public string GetStringAsJSONValue()
       {
            string outStr = "";

            Dictionary<string, object> outDic = GetInfoDic();


            outStr = JsonConvert.SerializeObject(outDic, Formatting.Indented);

            return outStr;

        }

       public Dictionary<string, object> GetInfoDic()
       {
           Dictionary<string, object> outDic = new Dictionary<string, object>();

           
           #region SMU
           outDic.Add("SourceMeterModel",_mCfg.SourceMeterModel.ToString());
           if (_mCfg.SourceMeterModel != ESourceMeterModel.NONE)
           {
               Dictionary<string, object> smuDic = new Dictionary<string, object>();

               smuDic.Add("SourceMeterAddress", _mCfg.SourceMeterSN.ToString());
               smuDic.Add("SMU_SN", _mInfo.SourceMeterSN.ToString());
               smuDic.Add("SMU_SWVer", _mInfo.SourceMeterSWVersion.ToString());
               smuDic.Add("SMU_HWVer", _mInfo.SourceMeterHWVersion.ToString());

               smuDic.Add("SrcTurnOffType", _mCfg.SrcTurnOffType.ToString());
               smuDic.Add("SrcSensingMode", _mCfg.SrcSensingMode.ToString());
               if (_mCfg.TesterFunctionType != ETesterFunctionType.Single_Die)
               {
                   smuDic.Add("SourceMeterSN", _mCfg.SourceMeterSN.ToString());
                   smuDic.Add("ChannelConfig", _mCfg.ChannelConfig);
               }
               outDic.Add("SMI", smuDic);
           }
           #endregion

           #region PDSensingMode
           //PDSensingMode
           outDic.Add("PDSensingMode", _mCfg.PDSensingMode.ToString());
           if (_mCfg.PDSensingMode == EPDSensingMode.SrcMeter_2nd)
           {
               Dictionary<string, object> pdDic = new Dictionary<string, object>();
               pdDic.Add("PDSensingAddress", _mCfg.PDDetectorSN.ToString());
               pdDic.Add("PDSensingMode", _mCfg.PDSensingMode.ToString());
               pdDic.Add("PDDetectorSN", _mCfg.PDDetectorSN.ToString());
               pdDic.Add("IsPDDetectorHwTrig", _mCfg.IsPDDetectorHwTrig);
               outDic.Add("PDSensingSetting", pdDic);
           }
           #endregion

           #region SpectrometerModel
           outDic.Add("spetometer", _mCfg.PDSensingMode.ToString());
           if (_mCfg.SpectrometerModel != ESpectrometerModel.NONE)
           {
               Dictionary<string, object> spDic = new Dictionary<string, object>();

               spDic.Add("SpectrometerAddress", _mCfg.SpectrometerSN.ToString());
               spDic.Add("spetometerHWSetting", _mCfg.spetometerHWSetting);
               spDic.Add("SphereSN", _mCfg.SphereSN);
               outDic.Add("SpectrometerSetting", spDic);
           }
           #endregion


           #region Laser
           outDic.Add("LCRModel", _mCfg.LCRModel.ToString());
           if (_mCfg.LCRModel != ELCRModel.NONE)
           {
               Dictionary<string, object> lcrDic = new Dictionary<string, object>();

               lcrDic.Add("LCRMeterAddress", _mCfg.LCRMeterSN.ToString());
               lcrDic.Add("LCRMeterSN", _mInfo.LCRMeterSN);
               lcrDic.Add("LCRMeterSWVersion", _mInfo.LCRMeterSWVersion);
               lcrDic.Add("LCRMeterHWVersion", _mInfo.LCRMeterHWVersion);
               outDic.Add("LCRSetting", lcrDic);
           }
           #endregion

           #region Laser
           if (_mCfg.LaserSrcSysConfig != null)
           {
               Dictionary<string, object> lsDic = new Dictionary<string, object>();

               lsDic.Add("LaserChList", _mCfg.LaserSrcSysConfig.GetLaserInfoList());
               outDic.Add("LaserSetting", lsDic);
           }
           #endregion

           return outDic;
       }


        #region
    
        #endregion

    }
}
