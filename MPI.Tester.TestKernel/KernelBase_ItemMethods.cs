using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Threading;

using MPI.Tester;
using MPI.Tester.Data;
using MPI.Tester.CompoCommon;
using MPI.Tester.Compo.DIDOCard;
using MPI.Tester.Compo.ADCard;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Device.SourceMeter;
using MPI.Tester.Device.SpectroMeter;
using MPI.Tester.Device.ESD;
using MPI.Tester.Device.LCRMeter;
using MPI.Tester.Device.SwitchSystem;
using MPI.Tester.Device.IOCard;
using MPI.Tester.Device.Pulser;
using MPI.Tester.Maths.ColorMath;
using MPI.Tester.Maths;

using MPI.Tester.Device.LaserSourceSys;

using MPI.Tester.Device.PostCalc;

namespace MPI.Tester.TestKernel
{
    public abstract partial class TesterKernelBase
    {
        protected virtual double[] POLAR(TestItemData item, uint srcMeterItemIndex, uint srcChannel)
        {
            List<uint> TrigList = new List<uint>(new uint[] { srcChannel });
            srcMeterItemIndex += (uint)item.ElecSetting.Length;
            return POLAR(item, srcMeterItemIndex, TrigList);
        }
        protected virtual double[] POLAR(TestItemData item, uint srcMeterItemIndex, List<uint> TrigList = null, ISourceMeter srcMeter = null)
        {
            double[] sourceMeterReadData = null;

            uint srcChannel = 0;

            if (TrigList == null)
            {
                TrigList = this._srcSyncTrigger;
            }
            if (srcMeter == null)
            {
                srcMeter = this._srcMeter;
            }

            if (item.IsEnable && srcMeter != null)
            {
                srcMeter.MeterOutput(TrigList.ToArray(), srcMeterItemIndex);

                if (this._machineConfig.Enable.IsInstantGetData == true)
                {
                    item.MsrtResult[0].Unit = "V";

                    sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex);

                    sourceMeterReadData[0] = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[0].Unit) * this._chipPolarity;

                    if (sourceMeterReadData[0] > item.ElecSetting[0].MsrtRange)
                    {
                        sourceMeterReadData[0] = item.ElecSetting[0].MsrtRange;
                    }

                    item.MsrtResult[0].RawValueArray = sourceMeterReadData;

                    item.MsrtResult[0].RawValue = sourceMeterReadData[0];
                }
            }

            if (item.MsrtResult[0].RawValue > item.ElecSetting[0].PolarThresholdVoltage)
            {
                this.ChangePolar();
            }

            return sourceMeterReadData;
        }

        protected virtual double[] IFH(TestItemData item, uint srcMeterItemIndex, uint srcChannel)
        {
            List<uint> TrigList = new List<uint>(new uint[] { srcChannel });

            srcMeterItemIndex += (uint)item.ElecSetting.Length;
            return IFH(item, srcMeterItemIndex, TrigList);
        }
        protected virtual double[] IFH(TestItemData item, uint srcMeterItemIndex, List<uint> trigList = null, ISourceMeter srcMeter = null)
        {
            if (trigList == null)
            {
                trigList = this._srcSyncTrigger;
            }
            if (srcMeter == null)
            {
                srcMeter = this._srcMeter;
            }

            if (item.IsEnable && srcMeter != null)
            {
                srcMeter.MeterOutput(trigList.ToArray(), srcMeterItemIndex);
            }
            return new double[] { 1 };//強迫回傳以免判斷為量測失敗
        }

        protected virtual double[] IF(TestItemData item, uint srcMeterItemIndex, uint srcChannel)
        {
            List<uint> TrigList = new List<uint>(new uint[] { srcChannel });

            srcMeterItemIndex += (uint)item.ElecSetting.Length;
            return IF(item, srcMeterItemIndex, TrigList);
        }
        protected virtual double[] IF(TestItemData item, uint srcMeterItemIndex, List<uint> trigList = null, ISourceMeter srcMeter = null)
        {
            if (trigList == null)
            {
                trigList = this._srcSyncTrigger;
            }
            if (srcMeter == null)
            {
                srcMeter = this._srcMeter;
            }
            double[] sourceMeterReadData = null;

            uint srcChannel = 0;

            if (item.IsEnable && srcMeter != null)
            {
                srcMeter.MeterOutput(trigList.ToArray(), srcMeterItemIndex);

                if (this._machineConfig.Enable.IsInstantGetData == true)
                {
                    for (int i = 0; i < trigList.Count; i++)
                    {
                        srcChannel = trigList[i];

                        sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex);

                        //item.MsrtResult[0].Unit = "V";

                        for (int j = 0; j < sourceMeterReadData.Length; j++)
                        {
                            sourceMeterReadData[j] = sourceMeterReadData[j] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[0].Unit) * this._chipPolarity;

                            double range = item.ElecSetting[0].MsrtRange * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[0].Unit);
                            if (sourceMeterReadData[j] > range)
                            {
                                sourceMeterReadData[j] = range;
                            }
                        }

                        item.MsrtResult[0].RawValueArray = sourceMeterReadData;

                        item.MsrtResult[0].RawValue = sourceMeterReadData[0];

                        SyncDataToChannel(item, srcChannel);

                    }
                }
            }
            return sourceMeterReadData;
        }

        protected virtual double[] IZ(TestItemData item, uint srcMeterItemIndex, uint srcChannel)
        {
            List<uint> TrigList = new List<uint>(new uint[] { srcChannel });

            srcMeterItemIndex += (uint)item.ElecSetting.Length;
            return IZ(item, srcMeterItemIndex, TrigList);
        }
        protected virtual double[] IZ(TestItemData item, uint srcMeterItemIndex, List<uint> trigList = null, ISourceMeter srcMeter = null, int stageNum = -1)
        {
            if (trigList == null)
            {
                trigList = this._srcSyncTrigger;
            }
            if (srcMeter == null)
            {
                srcMeter = this._srcMeter;
            }
            double[] sourceMeterReadData = null;
            
            if (item.IsEnable && srcMeter != null)
            {                
                if (this._machineConfig.Enable.IsInstantGetData == true)
                {
                    bool isCanGetData = true;

                    if ((item as IZTestItem).IsUseIrAsForceValue)//multi Die會有問題
                    {
                        isCanGetData = FloatIZ(item, srcMeterItemIndex, trigList, srcMeter, isCanGetData);
                    }
                    else
                    {
                        srcMeter.MeterOutput(trigList.ToArray(), srcMeterItemIndex);
                    }
                    //////////////////////////////////////////////////////////////////////////////////////////////////////

                    //item.MsrtResult[0].Unit = "V";

                    double clamp = item.ElecSetting[0].MsrtProtection * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[0].Unit);
                    // Get Msrt ForceValue
                    if (isCanGetData)
                    {
                        for (int i = 0; i < trigList.Count; i++)
                        {
                            uint srcChannel = trigList[i];

                            sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex);

                            item.MsrtResult[0].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[0].Unit) * this._chipPolarity * (-1.0d);

                            item.MsrtResult[1].RawValue = srcMeter.GetApplyDataFromMeter(srcChannel, srcMeterItemIndex)[0] * this._chipPolarity;


                            double clampVal = item.ElecSetting[0].MsrtProtection * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[0].Unit);
                            if (this._sysSetting.IsEnableLDT1ASoftwareClamp &&
                                item.MsrtResult[0].RawValue > clampVal)
                            {
                                item.MsrtResult[0].RawValue = clampVal;
                            }

                            if (item.MsrtResult[0].RawValue < 0.0d)
                            {
                                item.MsrtResult[0].RawValue = 0.0d;
                            }

                            if (this._isTVSTesting)
                            {
                                item.MsrtResult[0].RawValue = item.MsrtResult[0].RawValue * -1;
                            }

                            SyncDataToChannel(item, srcChannel);
                        }
                    }
                    else
                    {
                        item.MsrtResult[0].RawValue = clamp;
                        item.MsrtResult[1].RawValue = 0.0d;
                    }
                }                 
            }
            
            return sourceMeterReadData;
        }

        protected virtual bool FloatIZ(TestItemData item, uint srcMeterItemIndex, List<uint> trigList, ISourceMeter srcMeter, bool isCanGetData)
        {
            TestResultData data = null;

            string msrtKeyName = (item as IZTestItem).RefIrKeyName;

            data = this._acquireData[msrtKeyName];

            if (data == null)
            {
                Console.WriteLine("[FloatIZ] ,this._acquireData[] don't have " + msrtKeyName);
                return false;
            }

            double unitFactor = UnitMath.UnitConvertFactor(EAmpUnit.A, data.Unit);
            double applyValue = Math.Abs((data.Value * (item as IZTestItem).Factor - (item as IZTestItem).Offset)) * -1.0d / unitFactor;

            if (srcMeter is Keithley2600)
            {
                if (applyValue <= 0.0d)
                {
                    (srcMeter as Keithley2600).MeterOutput(trigList.ToArray(), srcMeterItemIndex, applyValue * this._chipPolarity);
                }
                else
                {
                    isCanGetData = false;
                }
            }
            else
            {
                Console.WriteLine("[IZ] ,_srcMeter is not Keithley2600");
                return false;
            }

            return isCanGetData;
        }


        protected virtual double[] VF(TestItemData item, uint srcMeterItemIndex, uint srcChannel)
        {
            List<uint> TrigList = new List<uint>(new uint[] { srcChannel });

            srcMeterItemIndex += (uint)item.ElecSetting.Length;
            return VF(item, srcMeterItemIndex, TrigList);
        }
        protected virtual double[] VF(TestItemData item, uint srcMeterItemIndex, List<uint> trigList = null, ISourceMeter srcMeter = null)
        {
            if (trigList == null)
            {
                trigList = this._srcSyncTrigger;
            }
            if (srcMeter == null)
            {
                srcMeter = this._srcMeter;
            }
            double[] sourceMeterReadData = null;

            if (item.IsEnable && srcMeter != null)
            {
                uint count = 0;
                while (count <= (item as VFTestItem).RetestCount)
                {
                    srcMeter.MeterOutput(trigList.ToArray(), srcMeterItemIndex);
                    int passDounter = 0;
                    for (int i = 0; i < trigList.Count; i++)
                    {
                        uint srcChannel = trigList[i];

                        if (this._machineConfig.Enable.IsInstantGetData == true)
                        {
                            sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex);

                            // item.MsrtResult[0].Unit = item.ElecSetting[0].MsrtUnit.ToString();
                            //item.MsrtResult[0].Unit = "mA";

                            if (this._isTVSTesting)
                            {
                                item.MsrtResult[0].Unit = "uA";
                            }

                            //item.MsrtResult[0].Unit = "uA";
                            item.MsrtResult[0].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EAmpUnit.A, item.MsrtResult[0].Unit) * this._chipPolarity;

                            // Get Msrt ForceValue
                            item.MsrtResult[1].RawValue = srcMeter.GetApplyDataFromMeter(srcChannel, srcMeterItemIndex)[0] * this._chipPolarity;


                            SyncDataToChannel(item, srcChannel);

                            if ((item as VFTestItem).IsEnableRetest)
                            {                                
                                if (item.MsrtResult[1].RawValue <= (item as VFTestItem).RetestThresholdV)
                                {
                                    passDounter++;
                                }
                            }

                            count++;                            
                        }
                    }

                    if ((item as VFTestItem).IsEnableRetest)
                    {
                        if (passDounter == trigList.Count)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }

                    
                }
            }

            return sourceMeterReadData;
        }

        protected virtual double[] VR(TestItemData item, uint srcMeterItemIndex, uint srcChannel)
        {
            List<uint> TrigList = new List<uint>(new uint[] { srcChannel });
            srcMeterItemIndex += (uint)item.ElecSetting.Length;
            return VR(item, srcMeterItemIndex, TrigList);
        }
        protected virtual double[] VR(TestItemData item, uint srcMeterItemIndex, List<uint> trigList = null, ISourceMeter srcMeter = null, int stageNum = -1)
        {
            if (trigList == null)
            {
                trigList = this._srcSyncTrigger;
            }
            if (srcMeter == null)
            {
                srcMeter = this._srcMeter;
            }

            double[] sourceMeterReadData = null;

            if (item.IsEnable && srcMeter != null)
            {
                if (_preTypeState == (int)ETestType.IZ && this._rdFunc.RDFuncData.IsEnableVRDelayTime)
                {
                    if (this._rdFunc.RDFuncData.VRDelayTime > 0)
                    {
                        srcMeter.TurnOff(this._rdFunc.RDFuncData.VRDelayTime, false);        // delay 5.0 ms cool the chip
                    }
                }

                bool isCanGetData = true;

                if ((item as VRTestItem).IsUseVzAsForceValue)//multi Die會有問題
                {
                    isCanGetData = FloatVR(item, srcMeterItemIndex, trigList, srcMeter, isCanGetData);
                }
                else
                {
                    srcMeter.MeterOutput(trigList.ToArray(), srcMeterItemIndex);
                }


                if (this._machineConfig.Enable.IsInstantGetData == true)
                {
                    double clamp = item.ElecSetting[0].MsrtProtection * UnitMath.UnitConvertFactor(EAmpUnit.uA, item.MsrtResult[0].Unit);
                    for (int i = 0; i < trigList.Count; i++)
                    {
                        uint srcChannel = trigList[i];
                        if (isCanGetData)
                        {
                            sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex);

                            item.MsrtResult[0].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EAmpUnit.A, item.MsrtResult[0].Unit) * this._chipPolarity * (-1.0d);

                            if (clamp < item.MsrtResult[0].RawValue)
                            {
                                item.MsrtResult[0].RawValue = clamp;
                            }

                            // RD Fun 
                            // 直接強迫Msrt IR 加上為 絕對值，使其不等於0 

                            if (this._rdFunc.RDFuncData.IsEnableAbsMsrtIR)
                            {
                                item.MsrtResult[0].RawValue = Math.Abs(item.MsrtResult[0].RawValue);
                            }

                            if (item.MsrtResult[0].RawValue < 0.0d)
                            {
                                item.MsrtResult[0].RawValue = 0.0d;
                            }

                            item.MsrtResult[0].RawValueArray = sourceMeterReadData;

                            if (this._isTVSTesting)
                            {
                                item.MsrtResult[0].RawValue = item.MsrtResult[0].RawValue * -1;
                            }

                            // Get Msrt ForceValue
                            item.MsrtResult[1].RawValue = srcMeter.GetApplyDataFromMeter(srcChannel, srcMeterItemIndex)[0] * this._chipPolarity;
                        }
                        else
                        {
                            item.MsrtResult[0].RawValue = clamp;
                            item.MsrtResult[1].RawValue = 0.0d;
                        }
                        SyncDataToChannel(item, srcChannel);
                    }
                }
            }

            return sourceMeterReadData;
        }

        protected virtual bool FloatVR(TestItemData item, uint srcMeterItemIndex, List<uint> trigList, ISourceMeter srcMeter, bool isCanGetData)
        {
            TestResultData data = null;

            string msrtKeyName = (item as VRTestItem).RefVzKeyName;

            data = this._acquireData[msrtKeyName];

            if (data == null)
            {
                Console.WriteLine("[FloatVR] ,this._acquireData[] don't have " + msrtKeyName);
                return false;
                //break;
            }
            double unitFactor = UnitMath.UnitConvertFactor(EVoltUnit.V, data.Unit);

            double applyValue = (data.Value * (item as VRTestItem).Factor - (item as VRTestItem).Offset) * -1.0d / unitFactor;

            if (srcMeter is Keithley2600)
            {
                if (applyValue <= 0.0d)
                {
                    (srcMeter as Keithley2600).MeterOutput(trigList.ToArray(), srcMeterItemIndex, applyValue * this._chipPolarity);
                }
                else
                {
                    isCanGetData = false;
                }
            }
            else
            {
                Console.WriteLine("[FloatVR] ,_srcMeter is not Keithley2600");
                return false;
            }
            return isCanGetData;
        }

        protected virtual double[] IVSweep(TestItemData item, uint srcMeterItemIndex, uint srcChannel)
        {
            List<uint> TrigList = new List<uint>(new uint[] { srcChannel });
            return IVSweep(item, srcMeterItemIndex, TrigList);
        }
        protected virtual double[] IVSweep(TestItemData item, uint srcMeterItemIndex, List<uint> trigList = null, ISourceMeter srcMeter = null)
        {
            if (trigList == null)
            {
                trigList = this._srcSyncTrigger;
            }
            if (srcMeter == null)
            {
                srcMeter = this._srcMeter;
            }
            uint dutChannel = this._acquireData.ChipInfo.Channel;
            double[] sourceMeterReadData = null;

            if (item.IsEnable && srcMeter != null)
            {
                //++++++++++++++++++++++++++++++++
                // Get sweep data from source meter,
                // Copy to "chartData"
                //++++++++++++++++++++++++++++++++

                Thread.Sleep((int)item.ElecSetting[0].ForceDelayTime);
                if (item.ElecSetting != null && item.ElecSetting.Length > 0)
                {
                    for (int chIndex = 0; chIndex < trigList.Count; chIndex++)
                    {

                        MPI.PerformanceTimer tm1 = new PerformanceTimer();
                        tm1.Start();
                        List<double> timeList = new List<double>();
                        List<double> srcList = new List<double>();
                        List<double> msrtList = new List<double>();

                        List<double> devList = new List<double>();
                        for (int j = 0; j < item.ElecSetting.Length; ++j)
                        {
                            uint tarSMUItemIndex = (uint)(srcMeterItemIndex + j);
                            double st = tm1.PeekTimeSpan(ETimeSpanUnit.MilliSecond);
                            srcMeter.MeterOutput(trigList.ToArray(), tarSMUItemIndex);

                            if (this._machineConfig.Enable.IsInstantGetData == true)
                            {

                                uint srcChannel = trigList[chIndex];
                                if (this._machineConfig.ChannelConfig.TesterSequenceType == ETesterSequenceType.Parallel)
                                {
                                    dutChannel = srcChannel;
                                }
                                else
                                {
                                    dutChannel = this._acquireData.ChipInfo.Channel - 1;
                                }
                                double[] timeChain = srcMeter.GetTimeChainFromMeter(srcChannel, tarSMUItemIndex).Clone() as double[];  // t : time   (ms)
                                double[] applyData = srcMeter.GetSweepPointFromMeter(srcChannel, tarSMUItemIndex).Clone() as double[]; // x : input  (A)
                                double[] sweepData = srcMeter.GetDataFromMeter(srcChannel, tarSMUItemIndex).Clone() as double[];       // y : output (V)

                                for (int k = 0; k < applyData.Length; k++)
                                {
                                    applyData[k] = applyData[k] * this._chipPolarity;
                                    sweepData[k] = sweepData[k] * this._chipPolarity;
                                    timeChain[k] = st + timeChain[k] * 1000;//s->ms
                                }

                                devList = CalcDevList(applyData, sweepData);

                                timeList.AddRange(timeChain);
                                srcList.AddRange(applyData);
                                msrtList.AddRange(sweepData);
                            }
                        }
                        this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].TimeChain = timeList.ToArray();	// t : time   (ms)
                        this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].ApplyData = srcList.ToArray();		// x : input  (A)
                        this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].SweepData = msrtList.ToArray();	// y : output (V) 
                    }
                }

                srcMeterItemIndex += (uint)item.ElecSetting.Length;
                sourceMeterReadData = this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].SweepData;//強迫回傳以免判斷為量測失敗
            }

            srcMeter.TurnOff();
            return sourceMeterReadData;
        
        }

        protected virtual double[] VISweep(TestItemData item, uint srcMeterItemIndex, uint srcChannel)
        {
            List<uint> TrigList = new List<uint>(new uint[] { srcChannel });
            return VISweep(item, srcMeterItemIndex, TrigList);
        }
        protected virtual double[] VISweep(TestItemData item, uint srcMeterItemIndex, List<uint> trigList = null, ISourceMeter srcMeter = null)
        {
            if (trigList == null)
            {
                trigList = this._srcSyncTrigger;
            }
            if (srcMeter == null)
            {
                srcMeter = this._srcMeter;
            }
            double[] sourceMeterReadData = null;
            uint dutChannel = this._acquireData.ChipInfo.Channel;

            if (item.IsEnable && srcMeter != null)
            {
                //++++++++++++++++++++++++++++++++
                // Get sweep data from source meter,
                // Copy to "chartData"
                //++++++++++++++++++++++++++++++++

                Thread.Sleep((int)item.ElecSetting[0].ForceDelayTime);
                if (item.ElecSetting != null && item.ElecSetting.Length > 0)
                {
                    for (int chIndex = 0; chIndex < trigList.Count; chIndex++)
                    {
                        
                        MPI.PerformanceTimer tm1 = new PerformanceTimer();
                        tm1.Start();
                        List<double> timeList = new List<double>();
                        List<double> srcList = new List<double>();
                        List<double> msrtList = new List<double>();
                        List<double> devList = new List<double>();
                        for (int j = 0; j < item.ElecSetting.Length; ++j)
                        {
                            uint tarSMUItemIndex = (uint)(srcMeterItemIndex + j);
                            double st = tm1.PeekTimeSpan(ETimeSpanUnit.MilliSecond);
                            srcMeter.MeterOutput(trigList.ToArray(), tarSMUItemIndex);

                            if (this._machineConfig.Enable.IsInstantGetData == true)
                            {

                                uint srcChannel = trigList[chIndex];
                                if (this._machineConfig.ChannelConfig.TesterSequenceType == ETesterSequenceType.Parallel)
                                {
                                    dutChannel = srcChannel;
                                }
                                else
                                {
                                    dutChannel = this._acquireData.ChipInfo.Channel - 1;
                                }
                                double[] timeChain = srcMeter.GetTimeChainFromMeter(srcChannel, tarSMUItemIndex).Clone() as double[];  // t : time   (ms)
                                double[] applyData = srcMeter.GetSweepPointFromMeter(srcChannel, tarSMUItemIndex).Clone() as double[]; // x : input  (V)
                                double[] sweepData = srcMeter.GetDataFromMeter(srcChannel, tarSMUItemIndex).Clone() as double[];       // y : output (A)
                                
                                for (int k = 0; k < applyData.Length; k++)
                                {
                                    applyData[k] = applyData[k] * this._chipPolarity;
                                    sweepData[k] = sweepData[k] * this._chipPolarity;
                                    timeChain[k] = st + timeChain[k]*1000;//s->ms                                   

                                }
                                devList = CalcDevList( applyData, sweepData);

                                timeList.AddRange(timeChain);
                                srcList.AddRange(applyData);
                                msrtList.AddRange(sweepData);
                            }                           
                        }
                        this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].TimeChain = timeList.ToArray();	// t : time   (ms)
                        this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].ApplyData = srcList.ToArray();		// x : input  (V)
                        this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].SweepData = msrtList.ToArray();	// y : output (A) 
                        this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].Derivative = devList.ToArray();
                        #region

                        #endregion

                        #region
                        item.MsrtResult[0].RawValue = 0;
                        double thresholddi = (item as VISweepTestItem).dIp * UnitMath.ToSIUnit(EAmpUnit.uA.ToString());
                        
                        if ((item as VISweepTestItem).IsCalcVp && item.MsrtResult!= null && item.MsrtResult.Length > 0)
                        {
                            double vp = 0;
                            vp = CalcVp(srcList, msrtList, thresholddi, vp);//獨立進行排序，以免分段掃描出現問題
                            item.MsrtResult[0].RawValue = vp;
                        }
                        
                        #endregion
                        SyncDataToChannel(item, dutChannel);
                    }
                }
               
                srcMeterItemIndex += (uint)item.ElecSetting.Length;
                sourceMeterReadData = this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].SweepData;//強迫回傳以免判斷為量測失敗
            }

            

            srcMeter.TurnOff();
            return sourceMeterReadData;
        }

        private static List<double> CalcDevList( double[] applyData, double[] sweepData)
        {
            List<double> devList = new List<double>();
            for (int k = 0; k < applyData.Length; k++)
            {
                if (k == 0)
                {
                    devList.Add(0);
                }
                else
                {
                    double devApp = applyData[k] - applyData[k - 1];
                    if (devApp == 0)
                    {
                        devList.Add(0);
                    }
                    else
                    {
                        double devSw = sweepData[k] - sweepData[k - 1];
                        double dsw_app = devSw / devApp;
                        devList.Add(dsw_app);
                    }
                }
            }
            return devList;
        }

        private static double CalcVp(List<double> srcList, List<double> msrtList, double thresholddi, double vp)
        {
            int length = srcList.Count;
            if (length > 3 && srcList.Max() <= 0 && srcList.Min() < 0)//只給負偏壓時進行計算
            {
                Dictionary<double, double> smDic = new Dictionary<double, double>();
                for (int i = 0; i < srcList.Count; ++i)
                {
                    if (smDic.ContainsKey(srcList[i]))
                    {
                        smDic[srcList[i]] = msrtList[i];
                    }
                    else
                    {
                        smDic.Add(srcList[i], msrtList[i]);
                    }
                }
                List<KeyValuePair<double, double>> smPairList = new List<KeyValuePair<double, double>>();
                smPairList = (from p in smDic
                              orderby p.Key descending
                              select new KeyValuePair<double, double>(p.Key, p.Value)).ToList();
                List<double> diList = new List<double>();

                for (int i = 1; i < smPairList.Count; ++i)
                {
                    double dv = smPairList[i].Key - smPairList[i - 1].Key;
                    if (dv < 0)//已排序過，需小於0
                    {
                        double di = smPairList[i].Value - smPairList[i - 1].Value;
                        double didv = di / dv;
                        diList.Add(di / dv);
                    }
                }

                int tarIndex = -1;
                double didvMax = diList.Max();

                if (didvMax > thresholddi)//有超過diMax才計算
                {
                    int indexMaxdi = diList.IndexOf(didvMax);//取第一組

                    for (int i = indexMaxdi; i < diList.Count; ++i)
                    {
                        if (diList[i] < thresholddi)
                        {
                            tarIndex = i + 1;
                            break;
                        }
                    }
                }
                if (tarIndex > 0 && tarIndex < smPairList.Count())
                {
                    vp = smPairList[tarIndex].Key;
                }
            }
            return vp;
        }

        protected virtual double[] THY(TestItemData item, uint srcMeterItemIndex, uint srcChannel)
        {
            List<uint> TrigList = new List<uint>(new uint[] { srcChannel });
            return THY(item, srcMeterItemIndex, TrigList);
        }
        protected virtual double[] THY(TestItemData item, uint srcMeterItemIndex, List<uint> trigList = null, ISourceMeter srcMeter = null)
        {
            if (trigList == null)
            {
                trigList = this._srcSyncTrigger;
            }
            if (srcMeter == null)
            {
                srcMeter = this._srcMeter;
            }
            uint dutChannel = this._acquireData.ChipInfo.Channel;

            if (item.IsEnable && srcMeter != null)
            {
                srcMeter.MeterOutput(trigList.ToArray(), srcMeterItemIndex);

                if (this._machineConfig.Enable.IsInstantGetData == true)
                {
                    for (int i = 0; i < trigList.Count; i++)
                    {
                        uint srcChannel = trigList[i];

                        if (this._machineConfig.ChannelConfig.TesterSequenceType == ETesterSequenceType.Parallel)
                        {
                            dutChannel = srcChannel;
                        }

                        //++++++++++++++++++++++++++++++++
                        // Get sweep data from source meter,
                        // Copy to "chartData"
                        //++++++++++++++++++++++++++++++++
                        this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].TimeChain = srcMeter.GetTimeChainFromMeter(srcChannel, srcMeterItemIndex).Clone() as double[];		// t : time (ms)
                        this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].ApplyData = srcMeter.GetSweepPointFromMeter(srcChannel, srcMeterItemIndex).Clone() as double[];		// x : input  (A)
                        this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].SweepData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex).Clone() as double[];			// y : output (V)

                        for (int k = 0; k < this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].TimeChain.Length; k++)
                        {
                            this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].ApplyData[k] = this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].ApplyData[k] * 1000 * this._chipPolarity;		// To mA
                            this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].SweepData[k] = this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].SweepData[k] * this._chipPolarity;		        // To V
                        }

                        //++++++++++++++++++++++++++++++++
                        // Get test result data from source meter
                        //++++++++++++++++++++++++++++++++
                        double[] sweepResult = srcMeter.GetSweepResultFromMeter(srcChannel, srcMeterItemIndex);
                        //item.MsrtResult[0].Unit = "V";
                        //item.MsrtResult[1].Unit = "V";
                        //item.MsrtResult[2].Unit = "V";
                        //-----------------------------------------------------------
                        // (**) No this._chipPolarity change issue,
                        //		The rerurn value is always positive.
                        //-----------------------------------------------------------
                        // Peak voltage
                        item.MsrtResult[0].RawValue = sweepResult[(int)ETHYResultItem.MaxPeak] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[0].Unit);
                        // Stable voltage
                        item.MsrtResult[1].RawValue = sweepResult[(int)ETHYResultItem.StableValue] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[1].Unit);
                        // Differential voltage
                        item.MsrtResult[2].RawValue = sweepResult[(int)ETHYResultItem.MaxToStable] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[2].Unit);

                        if (this._rdFunc.RDFuncData.IsEnableVFDGain)
                        {
                            item.MsrtResult[2].RawValue *= this._rdFunc.RDFuncData.VFDGain;
                        }

                        // Stable voltage
                        item.MsrtResult[3].RawValue = sweepResult[(int)ETHYResultItem.MTHYVDA] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[3].Unit);
                        // Differential voltage
                        item.MsrtResult[4].RawValue = sweepResult[(int)ETHYResultItem.MTHYVDB] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[4].Unit);

                        SyncDataToChannel(item, dutChannel, new int[] { 0, 1, 2 });

                    }
                }
            }
            srcMeterItemIndex++;
            return this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].SweepData;
        }

        protected virtual double[] DVF(TestItemData item, uint srcMeterItemIndex, uint srcChannel)
        {
            List<uint> TrigList = new List<uint>(new uint[] { srcChannel });
            return DVF(item, srcMeterItemIndex, TrigList);
        }
        protected virtual double[] DVF(TestItemData item, uint srcMeterItemIndex, List<uint> trigList = null, ISourceMeter srcMeter = null)
        {
            if (trigList == null)
            {
                trigList = this._srcSyncTrigger;
            }
            if (srcMeter == null)
            {
                srcMeter = this._srcMeter;
            }
            double[] sourceMeterReadData = null;

            uint srcChannel = 0;

            if (item.IsEnable && srcMeter != null)
            {
                uint dutChannel = this._acquireData.ChipInfo.Channel;
                //======================================
                // [1] The 1st force current
                //======================================
                srcMeter.MeterOutput(trigList.ToArray(), srcMeterItemIndex);

                if (this._machineConfig.Enable.IsInstantGetData == true)
                {
                    for (int i = 0; i < trigList.Count; i++)
                    {
                        srcChannel = trigList[i];

                        sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex);

                        //item.MsrtResult[0].Unit = "V";

                        item.MsrtResult[0].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[0].Unit) * this._chipPolarity;

                        SyncDataToChannel(item, srcChannel, 0);

                    }
                }

                //======================================
                // [2] The 2nd force current
                //======================================
                srcMeter.MeterOutput(trigList.ToArray(), srcMeterItemIndex + 1);

                if (this._machineConfig.Enable.IsInstantGetData == true)
                {
                    for (int i = 0; i < trigList.Count; i++)
                    {
                        srcChannel = trigList[i];

                        sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex + 1);

                        //item.MsrtResult[1].Unit = "V";

                        item.MsrtResult[1].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[1].Unit) * this._chipPolarity;

                        SyncDataToChannel(item, srcChannel, 1);

                    }
                }

                //======================================
                // [3] The 3rd force current
                //======================================
                srcMeter.MeterOutput(trigList.ToArray(), srcMeterItemIndex + 2);

                if (this._machineConfig.Enable.IsInstantGetData == true)
                {
                    for (int i = 0; i < trigList.Count; i++)
                    {
                        srcChannel = trigList[i];


                        sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex + 2);

                        //item.MsrtResult[2].Unit = "V";

                        item.MsrtResult[2].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[2].Unit) * this._chipPolarity;

                        //item.MsrtResult[3].Unit = "V";

                        item.MsrtResult[3].RawValue = item.MsrtResult[2].RawValue - item.MsrtResult[0].RawValue;

                        SyncDataToChannel(item, srcChannel, new int[] { 2, 3 });

                        //if (this._machineConfig.ChannelConfig.TesterSequenceType == ETesterSequenceType.Parallel)
                        //{
                        //    dutChannel = srcChannel;
                        //}

                        //if (this._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Die)
                        //{
                        //    this._condCtrl.Data.ChannelConditionTable.Channels[dutChannel][item.KeyName].MsrtResult[2].RawValue = item.MsrtResult[2].RawValue;

                        //    this._condCtrl.Data.ChannelConditionTable.Channels[dutChannel][item.KeyName].MsrtResult[3].RawValue =
                        //    this._condCtrl.Data.ChannelConditionTable.Channels[dutChannel][item.KeyName].MsrtResult[2].RawValue - this._condCtrl.Data.ChannelConditionTable.Channels[dutChannel][item.KeyName].MsrtResult[0].RawValue;

                        //}
                    }
                }
            }
            srcMeterItemIndex += 3;
            return sourceMeterReadData;
        }

        protected virtual double[] LOP(TestItemData item, uint srcMeterItemIndex, uint srcChannel)
        {
            List<uint> TrigList = new List<uint>(new uint[] { srcChannel });
            return LOP(item, srcMeterItemIndex, TrigList);
        }
        protected virtual double[] LOP(TestItemData item, uint srcMeterItemIndex, List<uint> trigList = null, ISourceMeter srcMeter = null)//,StageData sr = null)
        {
            EPDSensingMode PDMode = this._machineConfig.PDSensingMode;
            bool IsPDDetectorHwTrig = this._machineConfig.IsPDDetectorHwTrig;
            if (trigList == null)
            {
                trigList = this._srcSyncTrigger;
            }
            if (srcMeter == null)
            {
                srcMeter = this._srcMeter;
            }

            double[] sourceMeterReadData = null;

            uint srcChannel = 0;

            if (item.IsEnable && srcMeter != null)
            {
                //for (int i = 0; i < trigList.Count; i++)
                //{
                //    srcChannel = trigList[i];

                if (PDMode != EPDSensingMode.NONE)
                {
                    //----------------------------------------------------------------------------------
                    // (1) EMsrtType.LOP ( isAutoTurnOff == false )
                    //----------------------------------------------------------------------------------
                    srcMeter.MeterOutput(this._srcSyncTrigger.ToArray(), srcMeterItemIndex);

                    
                    for (int i = 0; i < trigList.Count; i++)
                    {
                        srcChannel = trigList[i];

                        sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex);

                        if (item.ElecSetting[0].MsrtType == EMsrtType.FIMVLOP)
                        {
                            item.MsrtResult[0].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[0].Unit) * this._chipPolarity; // PDMVFLA
                        }
                        else
                        {
                            item.MsrtResult[1].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EAmpUnit.A, item.MsrtResult[1].Unit) * this._chipPolarity;  // PDMIFLA
                        }

                        double pdCurrnet1 = 0.0d;
                        double pdCurrnet2 = 0.0d;

                        switch (PDMode)
                        {
                            case EPDSensingMode.DAQ:
                                {
                                    this._ADCard.DO(0, (uint)(item as LOPTestItem).AmpGainPower);

                                    item.MsrtResult[0].RawValue = this._ADCard.AD(0, 50);

                                    break;
                                }
                            case EPDSensingMode.SrcMeter_SMUB:
                            case EPDSensingMode.SrcMeter_2nd:
                                {
                                    // PD Current
                                    pdCurrnet1 = Math.Abs(sourceMeterReadData[1]);

                                    item.MsrtResult[2].RawValue = pdCurrnet1 * UnitMath.UnitConvertFactor(EAmpUnit.A, item.MsrtResult[2].Unit);  // PD current (uA)
                                    // PD count
                                    item.MsrtResult[3].RawValue = sourceMeterReadData[2];

                                    // PD Current
                                    pdCurrnet2 = Math.Abs(sourceMeterReadData[3]);

                                    item.MsrtResult[6].RawValue = pdCurrnet2 * UnitMath.UnitConvertFactor(EAmpUnit.A, item.MsrtResult[6].Unit);  // PD current (uA)
                                    // PD count
                                    item.MsrtResult[6].RawValue = sourceMeterReadData[4];

                                    break;
                                }
                        }

                        // mcd
                        item.MsrtResult[4].RawValue = pdCurrnet1 * _product.PdDetectorFactor * 1000.0d;  // mcd
                        // Watt
                        item.MsrtResult[5].RawValue = pdCurrnet1 * _product.PdDetectorFactor * 1000.0d;  // mW

                        // mcd
                        item.MsrtResult[8].RawValue = pdCurrnet2 * _product.PdDetectorFactor * 1000.0d;  // mcd
                        // Watt
                        item.MsrtResult[9].RawValue = pdCurrnet2 * _product.PdDetectorFactor * 1000.0d;  // mW

                        ////----------------------------------------------------------------------------------
                        //// (2) EMsrtType.LOP ( isAutoTurnOff == true )
                        ////----------------------------------------------------------------------------------
                        //srcMeter.MeterOutput(this._srcSyncTrigger.ToArray(), srcMeterItemIndex + 1);

                        //sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex + 1);

                        //if (item.ElecSetting[0].MsrtType == EMsrtType.FIMVLOP)
                        //{
                        //    item.MsrtResult[1].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[0].Unit) * this._chipPolarity;  // PDMVFLB
                        //}
                        //else
                        //{
                        //    item.MsrtResult[3].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EAmpUnit.A, item.MsrtResult[2].Unit) * this._chipPolarity;   // PDMIFLB
                        //}
                        SyncDataToChannel(item, srcChannel,new int[] {4,5,8,9});

                        if (PDMode == EPDSensingMode.SrcMeter_2nd && !IsPDDetectorHwTrig)
                        {
                            srcMeter.TurnOff();
                        }
                        //}
                    }
                }
            }

            return sourceMeterReadData;
        }

        protected virtual double[] LOPWL(TestItemData item, uint srcMeterItemIndex, uint sptMeterItemIndex, uint srcChannel)
        {
            List<uint> TrigList = new List<uint>(new uint[] { srcChannel });
            return LOPWL(item, srcMeterItemIndex, sptMeterItemIndex, TrigList);
        }
        protected virtual double[] LOPWL(TestItemData item, uint srcMeterItemIndex, uint sptMeterItemIndex, List<uint> trigList = null, ISourceMeter srcMeter = null, ISpectroMeter sptMeter = null)
        {
            if (trigList == null)
            {
                trigList = this._srcSyncTrigger;
            }

            if (srcMeter == null)
            {
                srcMeter = this._srcMeter;
            }

            if (sptMeter == null)
            {
                sptMeter = this._sptMeter;
            }
            double[] sourceMeterReadData = null;

            if (item.IsEnable == true && srcMeter != null && sptMeter != null)
            {
                double applyValue = 0.0d;
                uint srcChannel = trigList[0];
                uint dutChannel = this._acquireData.ChipInfo.Channel;

                #region >>calc apply value<< //multi Die會有問題
                if ((item as LOPWLTestItem).IsUseMsrtAsForceValue)
                {
                    string msrtKeyName = (item as LOPWLTestItem).RefMsrtKeyName;

                    TestResultData data = this._acquireData[msrtKeyName];

                    if (data == null)
                    {
                        return null;
                    }

                    applyValue = data.Value + (item as LOPWLTestItem).Offset;

                    double maxProtectApplyValue = (item as LOPWLTestItem).MaxProtectForceValue;

                    applyValue = applyValue <= maxProtectApplyValue ? applyValue : maxProtectApplyValue;
                }
                #endregion

                if ((item as LOPWLTestItem).IsUseMsrtAsForceValue)
                {
                    srcMeter.MeterOutput(trigList.ToArray(), srcMeterItemIndex, applyValue * this._chipPolarity);
                }
                else
                {
                    srcMeter.MeterOutput(trigList.ToArray(), srcMeterItemIndex);
                }


                if (this._machineConfig.Enable.IsInstantGetData == true)
                {
                    sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex);

                    // DC
                    if (item.ElecSetting[0].MsrtType == EMsrtType.FIMV)
                    {
                        item.MsrtResult[(int)EOptiMsrtType.MFILA].RawValue = srcMeter.GetApplyDataFromMeter(srcChannel, srcMeterItemIndex)[0];

                        //item.MsrtResult[(int)EOptiMsrtType.MVFLA].Unit = "V";

                        item.MsrtResult[(int)EOptiMsrtType.MVFLA].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[(int)EOptiMsrtType.MVFLA].Unit) * this._chipPolarity;
                    }
                    else
                    {
                        item.MsrtResult[(int)EOptiMsrtType.MFVLA].RawValue = srcMeter.GetApplyDataFromMeter(srcChannel, srcMeterItemIndex)[0];

                        //item.MsrtResult[(int)EOptiMsrtType.MIFLA].Unit = "mA";

                        item.MsrtResult[(int)EOptiMsrtType.MIFLA].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EAmpUnit.A, item.MsrtResult[(int)EOptiMsrtType.MIFLA].Unit) * this._chipPolarity;
                    }

                    //================================================================
                    // PD Detector Trigger
                    //================================================================
                    if ((item as LOPWLTestItem).EnablePDSensing)
                    {
                        switch (this._machineConfig.PDSensingMode)
                        {
                            case EPDSensingMode.SrcMeter_SMUB:
                            case EPDSensingMode.SrcMeter_2nd:
                                {
                                    item.MsrtResult[(int)EOptiMsrtType.LOPWLPDCURRENT].RawValue = sourceMeterReadData[1] * UnitMath.UnitConvertFactor(EAmpUnit.A, item.MsrtResult[(int)EOptiMsrtType.LOPWLPDCURRENT].Unit);  // PD current (uA)

                                    item.MsrtResult[(int)EOptiMsrtType.LOPWLPDCOUNT].RawValue = sourceMeterReadData[2];  // PD count
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        item.MsrtResult[(int)EOptiMsrtType.LOPWLPDCURRENT].RawValue = 0.0d;

                        item.MsrtResult[(int)EOptiMsrtType.LOPWLPDCOUNT].RawValue = 0.0d;
                    }



                    srcMeterItemIndex++;		// If the _srcMeter = null, the electronic test item also move to next index

                    //================================================================
                    // [2] Trigger the Spectrometer to get spectrum
                    //================================================================
                    if (sptMeter != null && (item as LOPWLTestItem).IsTestOptical == true && (item as LOPWLTestItem).IsTestElecDontTestLOPWL == false)
                    {
                        //================================================================
                        //  Skip Count Run
                        //================================================================
                        int rem = 0;

                        if (this._product.LOPWLSkipCount != 0)
                        {
                            Math.DivRem((int)this._skipCount, (int)this._product.LOPWLSkipCount, out rem);
                        }

                        if (rem == 0)
                        {
                            int rtnCode = sptMeter.Trigger(sptMeterItemIndex);

                            if (rtnCode > 0)
                            {
                                this._acquireData.SpectrumDataSet[dutChannel, item.KeyName].Wavelength = sptMeter.GetXWavelength();
                                this._acquireData.SpectrumDataSet[dutChannel, item.KeyName].Intensity = sptMeter.GetYSpectrumIntensity(sptMeterItemIndex);
                                // Create the dumy space for the array,
                                // The true absolute intensity data will calc by "Spt.Calculate()"
                                this._acquireData.SpectrumDataSet[dutChannel, item.KeyName].Absoluate = sptMeter.GetYAbsoluateSpectrum(sptMeterItemIndex);
                                this._acquireData.SpectrumDataSet[dutChannel, item.KeyName].Dark = sptMeter.DarkIntensityArray;
                            }
                            else
                            {
                                if (rtnCode == -23)
                                {
                                    this._isTriggerSptErr = true;
                                }  // Re Test
                                //--------------------------------------------------------
                                // Error : for spectrometer Trigger Function
                                //		   then break the all "Test Process"
                                //---------------------------------------------------------
                                srcMeter.TurnOff(0.0d, false);
                                //break;
                            }
                        }
                    }
                    else if (srcMeter != null && (item as LOPWLTestItem).IsTestOptical == false)
                    {
                        srcMeter.TurnOff(0.0d, false);
                    }

                    sptMeterItemIndex++;		// If the _sptMeter = null, the optical est item also move to next index
                    //================================================================
                    // [3] Continuous force the same current and measurement volt. (MV)
                    //================================================================
                    if (srcMeter != null && (item as LOPWLTestItem).IsOnlyTestMVFLA == false)
                    {
                        if (!this._rdFunc.RDFuncData.IsDisableMVFLB)
                        {
                            if ((item as LOPWLTestItem).IsUseMsrtAsForceValue)
                            {
                                srcMeter.MeterOutput(trigList.ToArray(), srcMeterItemIndex, applyValue * this._chipPolarity);
                            }
                            else
                            {
                                srcMeter.MeterOutput(trigList.ToArray(), srcMeterItemIndex);
                            }

                            if (this._machineConfig.Enable.IsInstantGetData == true)
                            {
                                sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex);

                                //item.MsrtResult[(int)EOptiMsrtType.MVFLB].Unit = "V";

                                item.MsrtResult[(int)EOptiMsrtType.MVFLB].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[(int)EOptiMsrtType.MVFLB].Unit) * this._chipPolarity;
                            }
                        }
                        else
                        {
                            srcMeter.TurnOff();
                        }
                    }

                    srcMeterItemIndex++;		// If the _srcMeter = null, the electronic test item also move to next index
                }
                else
                {
                    #region >>> Pulse Mode <<<

                    // SrcMeter in Pulse 只 Trigger 1次
                    if (srcMeter != null)
                    {
                        if ((item as LOPWLTestItem).IsUseMsrtAsForceValue)
                        {
                            srcMeter.MeterOutput(trigList.ToArray(), srcMeterItemIndex, applyValue * this._chipPolarity);
                        }
                        else
                        {
                            srcMeter.MeterOutput(trigList.ToArray(), srcMeterItemIndex);
                        }
                    }

                    if (sptMeter != null && (item as LOPWLTestItem).IsTestOptical == true)
                    {
                        //================================================================
                        //  Skip Count Run
                        //================================================================
                        int rem = 0;

                        if (this._product.LOPWLSkipCount != 0)
                        {
                            Math.DivRem((int)this._skipCount, (int)this._product.LOPWLSkipCount, out rem);
                        }

                        if (rem == 0)
                        {
                            int rtnCode = sptMeter.Trigger(sptMeterItemIndex);

                            if (rtnCode > 0)
                            {
                                int intensity = (int)sptMeter.Data[sptMeterItemIndex].MaxCount;

                                if ((intensity - this._darkMeam) < 80)
                                {
                                    if (this._machineConfig.Enable.IsInstantGetData == true)
                                    {
                                        sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex);

                                        // DC
                                        if (item.ElecSetting[0].MsrtType == EMsrtType.FIMV)
                                        {
                                            item.MsrtResult[(int)EOptiMsrtType.MFILA].RawValue = srcMeter.GetApplyDataFromMeter(srcChannel, srcMeterItemIndex)[0];

                                            //item.MsrtResult[(int)EOptiMsrtType.MVFLA].Unit = "V";

                                            item.MsrtResult[(int)EOptiMsrtType.MVFLA].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[(int)EOptiMsrtType.MVFLA].Unit) * this._chipPolarity;
                                        }
                                        else
                                        {
                                            item.MsrtResult[(int)EOptiMsrtType.MFVLA].RawValue = srcMeter.GetApplyDataFromMeter(srcChannel, srcMeterItemIndex)[0];

                                            //item.MsrtResult[(int)EOptiMsrtType.MIFLA].Unit = "mA";

                                            item.MsrtResult[(int)EOptiMsrtType.MIFLA].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EAmpUnit.A, item.MsrtResult[(int)EOptiMsrtType.MIFLA].Unit) * this._chipPolarity;
                                        }

                                        //================================================================
                                        // PD Detector Trigger
                                        //================================================================
                                        if ((item as LOPWLTestItem).EnablePDSensing)
                                        {
                                            switch (this._machineConfig.PDSensingMode)
                                            {
                                                case EPDSensingMode.SrcMeter_SMUB:
                                                case EPDSensingMode.SrcMeter_2nd:
                                                    {
                                                        item.MsrtResult[(int)EOptiMsrtType.LOPWLPDCURRENT].RawValue = sourceMeterReadData[1] * UnitMath.UnitConvertFactor(EAmpUnit.A, item.MsrtResult[(int)EOptiMsrtType.LOPWLPDCURRENT].Unit);  // PD current (uA)

                                                        item.MsrtResult[(int)EOptiMsrtType.LOPWLPDCOUNT].RawValue = sourceMeterReadData[2];  // PD count

                                                        break;
                                                    }
                                                default:
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            item.MsrtResult[(int)EOptiMsrtType.LOPWLPDCURRENT].RawValue = 0.0d;

                                            item.MsrtResult[(int)EOptiMsrtType.LOPWLPDCOUNT].RawValue = 0.0d;
                                        }
                                    }


                                }


                                this._acquireData.SpectrumDataSet[dutChannel, item.KeyName].Wavelength = sptMeter.GetXWavelength();
                                this._acquireData.SpectrumDataSet[dutChannel, item.KeyName].Intensity = sptMeter.GetYSpectrumIntensity(sptMeterItemIndex);
                                // Create the dumy space for the array,
                                // The true absolute intensity data will calc by "Spt.Calculate()"
                                this._acquireData.SpectrumDataSet[dutChannel, item.KeyName].Absoluate = sptMeter.GetYAbsoluateSpectrum(sptMeterItemIndex);
                                this._acquireData.SpectrumDataSet[dutChannel, item.KeyName].Dark = sptMeter.DarkIntensityArray;
                            }
                            else
                            {
                                if (rtnCode == -23)
                                {
                                    this._isTriggerSptErr = true;
                                }  // Re Test
                                //--------------------------------------------------------
                                // Error : for spectrometer Trigger Function
                                //		   then break the all "Test Process"
                                //---------------------------------------------------------
                                srcMeter.TurnOff(0.0d, false);
                                //break;
                            }
                        }
                    }
                    else if (srcMeter != null && (item as LOPWLTestItem).IsTestOptical == false)
                    {
                        srcMeter.TurnOff(0.0d, false);
                    }


                    if (srcMeter is Keithley2520)
                    {
                        (srcMeter as Keithley2520).StopOutput();
                    }

                    sptMeterItemIndex++;

                    if (this._machineConfig.Enable.IsInstantGetData == true)
                    {
                        sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex);

                        if (!(item as LOPWLTestItem).IsACSourceMeter)
                        {
                            // DC
                            if (item.ElecSetting[0].MsrtType == EMsrtType.FIMV)
                            {
                                item.MsrtResult[(int)EOptiMsrtType.MFILA].RawValue = srcMeter.GetApplyDataFromMeter(srcChannel, srcMeterItemIndex)[0];

                                //item.MsrtResult[(int)EOptiMsrtType.MVFLA].Unit = "V";

                                item.MsrtResult[(int)EOptiMsrtType.MVFLA].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[(int)EOptiMsrtType.MVFLA].Unit) * this._chipPolarity;
                            }
                            else
                            {
                                item.MsrtResult[(int)EOptiMsrtType.MFVLA].RawValue = srcMeter.GetApplyDataFromMeter(srcChannel, srcMeterItemIndex)[0];

                                //item.MsrtResult[(int)EOptiMsrtType.MIFLA].Unit = "mA";

                                item.MsrtResult[(int)EOptiMsrtType.MIFLA].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EAmpUnit.A, item.MsrtResult[(int)EOptiMsrtType.MIFLA].Unit) * this._chipPolarity;
                            }

                            //================================================================
                            // PD Detector Trigger
                            //================================================================
                            if ((item as LOPWLTestItem).EnablePDSensing)
                            {
                                switch (this._machineConfig.PDSensingMode)
                                {
                                    case EPDSensingMode.SrcMeter_SMUB:
                                    case EPDSensingMode.SrcMeter_2nd:
                                        {
                                            item.MsrtResult[(int)EOptiMsrtType.LOPWLPDCURRENT].RawValue = sourceMeterReadData[1] * UnitMath.UnitConvertFactor(EAmpUnit.A, item.MsrtResult[(int)EOptiMsrtType.LOPWLPDCURRENT].Unit);  // PD current (uA)

                                            item.MsrtResult[(int)EOptiMsrtType.LOPWLPDCOUNT].RawValue = sourceMeterReadData[2];  // PD count

                                            break;
                                        }
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                item.MsrtResult[(int)EOptiMsrtType.LOPWLPDCURRENT].RawValue = 0.0d;

                                item.MsrtResult[(int)EOptiMsrtType.LOPWLPDCOUNT].RawValue = 0.0d;
                            }

                        }
                    }

                    srcMeterItemIndex += (uint)item.ElecSetting.Length;

                    #endregion
                }
            }

            return sourceMeterReadData;
        }

        protected virtual double[] ESD(TestItemData item, uint srcMeterItemIndex, int esdItemIndex, uint srcChannel)
        {
            List<uint> TrigList = new List<uint>(new uint[] { srcChannel });
            return ESD(item, srcMeterItemIndex, esdItemIndex, TrigList);
        }
        protected virtual double[] ESD(TestItemData item, uint srcMeterItemIndex, int esdItemIndex, List<uint> trigList = null, ISourceMeter srcMeter = null,IESDDevice esdDev = null)
        {
            if (trigList == null)
            {
                trigList = this._srcSyncTrigger;
            }

            if (srcMeter == null)
            {
                srcMeter = this._srcMeter;
            }

            if (esdDev == null)
            {
                esdDev = this._esdDevice;
            }

            double[] sourceMeterReadData = null;

            uint srcChannel = 0;

            uint dutChannel = this._acquireData.ChipInfo.Channel;

            if (item.IsEnable && esdDev != null)
            {
                // 打 ESD前先 Turn-OFF (isOpenRelay = false)，消除ESD在第一道時，Tester Relay 關閉所造成的雜訊
                if (srcMeter != null)
                {
                    if (this._product.TestCondition.ChipPolarity == EPolarity.Cathode_N)
                    {
                        srcMeter.TurnOff(1.0d, false);  // Cathode 
                    }
                    else
                    {
                        srcMeter.TurnOff(0.5d, false);  // Andode 
                    }
                }

                //================================================================
                // [1] Trigger the ESD
                //================================================================
                if (esdDev.IsWorkingBusy == false)
                {
                    if (!this._passRateCheck.IsStopZapESD)
                    {
                        esdDev.Zap(trigList.ToArray(), esdItemIndex);

                        // MESD, Zap Value
                        item.MsrtResult[1].RawValue = (item as ESDTestItem).EsdSetting.ZapVoltage;

                        this._machineInfo.EsdHbmRelayCount = esdDev.HardwareInfo.HBMRelayCount;
                        this._machineInfo.EsdMmRelayCount = esdDev.HardwareInfo.MMRelayCount;
                        this._machineInfo.EsdHbmRelayCount2 = esdDev.HardwareInfo.HBMRelayCount2;
                        this._machineInfo.EsdMmRelayCount2 = esdDev.HardwareInfo.MMRelayCount2;
                        this._machineInfo.EsdHbmRelayCount3 = esdDev.HardwareInfo.HBMRelayCount3;
                        this._machineInfo.EsdMmRelayCount3 = esdDev.HardwareInfo.MMRelayCount3;
                        this._machineInfo.EsdHbmRelayCount4 = esdDev.HardwareInfo.HBMRelayCount4;
                        this._machineInfo.EsdMmRelayCount4 = esdDev.HardwareInfo.MMRelayCount4;

                        item.IsTested = true;
                    }
                }

                //================================================================  
                // [2] ESD Judge 
                //================================================================
                if (srcMeter != null && (item as ESDTestItem).IsEnableJudgeItem)
                {

                    srcMeter.MeterOutput(this._srcSyncTrigger.ToArray(), srcMeterItemIndex);

                    if (this._machineConfig.Enable.IsInstantGetData == true)
                    {
                        for (int i = 0; i < this._srcSyncTrigger.Count; i++)
                        {
                            srcChannel = trigList[i];                            

                            //item.MsrtResult[0].Unit = "uA";

                            sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex);

                            if (this._isTVSTesting)
                            {
                                item.MsrtResult[0].RawValue = item.MsrtResult[0].RawValue * -1;
                            }

                            item.MsrtResult[0].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EAmpUnit.A, item.MsrtResult[0].Unit) * this._chipPolarity * (-1.0d);

                            if (item.MsrtResult[0].RawValue > item.ElecSetting[0].MsrtProtection)
                            {
                                item.MsrtResult[0].RawValue = item.ElecSetting[0].MsrtProtection;
                            }

                            // RD Fun 
                            // 直接強迫Msrt IR 加上為 絕對值，使其不等於0 
                            if (this._rdFunc.RDFuncData.IsEnableAbsMsrtIR)
                            {
                                item.MsrtResult[0].RawValue = Math.Abs(item.MsrtResult[0].RawValue);
                            }

                            if (item.MsrtResult[0].RawValue < 0.0d)
                            {
                                item.MsrtResult[0].RawValue = 0.0d;
                            }

                            item.MsrtResult[0].RawValueArray = sourceMeterReadData;

                            item.MsrtResult[0].Value = item.MsrtResult[0].RawValue;

                            item.Calibrate();//這邊在MultiDie會有什麼問題還不清楚，後續記得處理

                            item.MsrtResult[2].RawValue = item.MsrtResult[1].RawValue;  // 1: ESD, 2: Finial

                            if (item.MsrtResult[0].IsPass)
                            {
                                item.MsrtResult[2].RawValue += 10;
                            }

                            srcMeter.TurnOff(0.0d, false);

                            SyncDataToChannel(item, dutChannel, new int[] { 0, 1, 2 });
                            
                        }
                    }
                }

                //isJudgeFailSkipESDItem = !item.MsrtResult[0].IsPass;
            }
            return sourceMeterReadData;
        }

        protected virtual double[] DIB(TestItemData item, uint srcMeterItemIndex, uint srcChannel)
        {
            List<uint> TrigList = new List<uint>(new uint[] { srcChannel });
            return DIB(item, srcMeterItemIndex, TrigList);
        }
        protected virtual double[] DIB(TestItemData item, uint srcMeterItemIndex, List<uint> trigList = null, ISourceMeter srcMeter = null)
        {
            if (trigList == null)
            {
                trigList = this._srcSyncTrigger;
            }
            if (srcMeter == null)
            {
                srcMeter = this._srcMeter;
            }
            double[] sourceMeterReadData = null;

            uint srcChannel = 0;
            if (item.IsEnable && srcMeter != null)
            {
                srcMeter.MeterOutput(this._srcSyncTrigger.ToArray(), srcMeterItemIndex);

                if (this._machineConfig.Enable.IsInstantGetData == true)
                {
                    for (int i = 0; i < this._srcSyncTrigger.Count; i++)
                    {
                        srcChannel = this._srcSyncTrigger[i];

                        sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex);

                        item.MsrtResult[0].Unit = "V";

                        // Gilbert, 20121222
                        for (int j = 0; j < sourceMeterReadData.Length; j++)
                        {
                            sourceMeterReadData[j] = sourceMeterReadData[j] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[0].Unit) * this._chipPolarity;

                            if (sourceMeterReadData[j] > item.ElecSetting[0].MsrtRange)
                            {
                                sourceMeterReadData[j] = item.ElecSetting[0].MsrtRange;
                            }
                        }

                        item.MsrtResult[0].RawValueArray = sourceMeterReadData;

                        item.MsrtResult[0].RawValue = sourceMeterReadData[0];


                        SyncDataToChannel(item, srcChannel);
                    }
                }
            }
            return sourceMeterReadData;
        }

        protected virtual double[] R(TestItemData item, uint srcMeterItemIndex, uint srcChannel)
        {
            List<uint> TrigList = new List<uint>(new uint[] { srcChannel });
            return R(item, srcMeterItemIndex, TrigList);
        }
        protected virtual double[] R(TestItemData item, uint srcMeterItemIndex, List<uint> trigList = null, ISourceMeter srcMeter = null)
        {
            if (trigList == null)
            {
                trigList = this._srcSyncTrigger;
            }
            if (srcMeter == null)
            {
                srcMeter = this._srcMeter;
            }
            double[] sourceMeterReadData = null;

            uint srcChannel = 0;

            if (item.IsEnable && srcMeter != null)
            {
                srcMeter.MeterOutput(this._srcSyncTrigger.ToArray(), srcMeterItemIndex);

                if (this._machineConfig.Enable.IsInstantGetData == true)
                {
                    for (int i = 0; i < this._srcSyncTrigger.Count; i++)
                    {
                        srcChannel = this._srcSyncTrigger[i];

                        sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex);

                        // (1) R = V/I
                        //item.MsrtResult[0].Unit = "Ohm";
                        item.MsrtResult[0].RawValue = sourceMeterReadData[0];
                        item.MsrtResult[0].Value = item.MsrtResult[0].RawValue;

                        SyncDataToChannel(item, srcChannel, new int[] { 0, 1, 2, 3 });
                    }
                }
            }
            return sourceMeterReadData;
        }

        protected virtual double[] RTH(TestItemData item, uint srcMeterItemIndex, uint srcChannel)
        {
            List<uint> TrigList = new List<uint>(new uint[] { srcChannel });
            return RTH(item, srcMeterItemIndex, TrigList);
        }
        protected virtual double[] RTH(TestItemData item, uint srcMeterItemIndex, List<uint> trigList = null, ISourceMeter srcMeter = null)
        {
            if (trigList == null)
            {
                trigList = this._srcSyncTrigger;
            }

            if (srcMeter == null)
            {
                srcMeter = this._srcMeter;
            }
            uint srcChannel = 0;
            if (item.IsEnable && srcMeter != null)
            {
                srcMeter.MeterOutput(this._srcSyncTrigger.ToArray(), srcMeterItemIndex);

                if (this._machineConfig.Enable.IsInstantGetData == true)
                {
                    for (int i = 0; i < this._srcSyncTrigger.Count; i++)
                    {
                        srcChannel = this._srcSyncTrigger[i];

                        this._acquireData.ElecSweepDataSet[srcChannel, item.KeyName].TimeChain = srcMeter.GetTimeChainFromMeter(srcChannel, srcMeterItemIndex).Clone() as double[];		// t : time (ms)
                        this._acquireData.ElecSweepDataSet[srcChannel, item.KeyName].ApplyData = srcMeter.GetSweepPointFromMeter(srcChannel, srcMeterItemIndex).Clone() as double[];		// x : input  (A)
                        this._acquireData.ElecSweepDataSet[srcChannel, item.KeyName].SweepData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex).Clone() as double[];			// y : output (V)

                        for (int k = 0; k < this._acquireData.ElecSweepDataSet[srcChannel, item.KeyName].TimeChain.Length; k++)
                        {
                            this._acquireData.ElecSweepDataSet[srcChannel, item.KeyName].ApplyData[k] = this._acquireData.ElecSweepDataSet[srcChannel, item.KeyName].ApplyData[k] * 1000 * this._chipPolarity;		// To mA
                            this._acquireData.ElecSweepDataSet[srcChannel, item.KeyName].SweepData[k] = this._acquireData.ElecSweepDataSet[srcChannel, item.KeyName].SweepData[k] * this._chipPolarity;		        // To V
                        }

                        double[] sweepResult = srcMeter.GetSweepResultFromMeter(srcChannel, srcMeterItemIndex);
                        //item.MsrtResult[0].Unit = "V";
                        //item.MsrtResult[1].Unit = "V";
                        //item.MsrtResult[2].Unit = "V";
                        // Peak voltage
                        item.MsrtResult[0].RawValue = sweepResult[0] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[0].Unit);
                        // Stable voltage
                        item.MsrtResult[1].RawValue = sweepResult[1] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[1].Unit);
                        // Differential voltage
                        item.MsrtResult[2].RawValue = sweepResult[2] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[2].Unit);

                        SyncDataToChannel(item, srcChannel, new int[] { 0, 1, 2 });

                    }
                }
            }
            return this._acquireData.ElecSweepDataSet[srcChannel, item.KeyName].SweepData;
        }

        protected virtual double[] ContactCheck(TestItemData item, uint srcMeterItemIndex, uint srcChannel)
        {
            List<uint> TrigList = new List<uint>(new uint[] { srcChannel });
            return ContactCheck(item, srcMeterItemIndex, TrigList);
        }
        protected virtual double[] ContactCheck(TestItemData item, uint srcMeterItemIndex, List<uint> trigList = null, ISourceMeter srcMeter = null)
        {
            if (trigList == null)
            {
                trigList = this._srcSyncTrigger;
            }
            if (srcMeter == null)
            {
                srcMeter = this._srcMeter;
            }
            double[] sourceMeterReadData = null;

            uint srcChannel = 0;

            if (item.IsEnable && srcMeter != null)
            {
                srcMeter.MeterOutput(this._srcSyncTrigger.ToArray(), srcMeterItemIndex);

                if (this._machineConfig.Enable.IsInstantGetData == true)
                {
                    sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex);

                    item.MsrtResult[0].Unit = "Ohm";
                    item.MsrtResult[0].RawValue = Math.Abs(sourceMeterReadData[0]);

                    item.MsrtResult[1].Unit = "Ohm";
                    item.MsrtResult[1].RawValue = Math.Abs(sourceMeterReadData[1]);
                }
            }
            return sourceMeterReadData;
        }

        protected virtual double[] IO(TestItemData item, uint srcMeterItemIndex, uint srcChannel)
        {
            List<uint> TrigList = new List<uint>(new uint[] { srcChannel });
            return IO(item, srcMeterItemIndex, TrigList);
        }
        protected virtual double[] IO(TestItemData item, uint srcMeterItemIndex,  List<uint> trigList = null, ISourceMeter srcMeter = null)
        {
            double[] sourceMeterReadData = null;

            if (trigList == null)
            {
                trigList = this._srcSyncTrigger;
            }
            if (srcMeter == null)
            {
                srcMeter = this._srcMeter;
            }

            uint srcChannel = 0;

            if (item.IsEnable && srcMeter != null)
            {
                srcMeter.MeterOutput(trigList.ToArray(), srcMeterItemIndex);

                if (this._machineConfig.Enable.IsInstantGetData == true)
                {
                    double waitTime = 0;

                    foreach (IOCmd dData in item.ElecSetting[0].IOSetting.CmdList)
                    {
                        if ((dData.HoldTime + dData.DelayTime) > waitTime)
                        {
                            waitTime = (dData.HoldTime + dData.DelayTime);
                        }

                    }

                    if (waitTime > 2)
                    {
                        System.Threading.Thread.Sleep(((int)waitTime) - 1);
                    }
                    sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex);//確認IO已執行完成
                }
            }

            return new double[] { 1 };
        }


        protected virtual double[] LaserSource(TestItemData item, LaserSourceSystem lsrc = null)
        {
            double[] sourceMeterReadData = new double[] { 1 };

            if (lsrc == null)
            {
                lsrc = this._laserSrcSys;

                LaserSourceTestItem lsItem = item as LaserSourceTestItem;

                if (lsItem.IsEnable && lsrc != null)
                {
                    int sysCh = lsItem.LaserSourceSet.SysChannel;
                    if (lsrc.OSManager != null)
                    {
                        Switch2OpticalCh(sysCh);
                        //lsrc.SetParamToOS(sysCh);
                    }

                    if (item.ElecSetting != null && item.ElecSetting.Length > 0 && item.ElecSetting[0].ForceDelayTime > 0)//強迫啟動，可能下一道IR就會用到
                    {
                        System.Threading.Thread.Sleep((int)item.ElecSetting[0].ForceDelayTime);
                    }

                    if (lsItem.LaserSourceSet.AttenuatorData != null &&lsItem.LaserSourceSet.AttenuatorData.IsRecordPower)
                    {
                        if (lsrc.AttManager != null)
                        {
                            double val = lsrc.AttManager.GetMsrtPower(sysCh,ELaserPowerUnit.dBm);

                            item.MsrtResult[2].RawValue = val;
                        }
                    }

                    if (lsItem.LaserSourceSet.PowerMeterSetting != null && lsItem.LaserSourceSet.PowerMeterSetting.RecordPower)
                    {
                        if (lsrc.PowerMeterManager != null)
                        {
                            IPowerMeter pm = lsrc.PowerMeterManager.GetPowerMeterFromSysCh(sysCh);
                            double val = 0;
                            if(pm != null)
                            {
                                val = lsrc.PowerMeterManager.GetMsrtPower(lsItem.LaserSourceSet.PowerMeterSetting, (int)this._chipPolarity);
                            }

                            item.MsrtResult[1].RawValue = val;

                            if (pm is MPI.Tester.Device.LaserSourceSys.PowerMeter.K2600PowerMeter && 
                                lsItem.LaserSourceSet.PowerMeterSetting.PDGain != 0)
                            {
                                item.MsrtResult[0].RawValue = val * lsItem.LaserSourceSet.PowerMeterSetting.PDGain;//unit of gain : A/W
                                if (lsItem.LaserSourceSet.PowerMeterSetting.SysGain != 0)
                                {
                                    item.MsrtResult[0].RawValue = val * lsItem.LaserSourceSet.PowerMeterSetting.PDGain / lsItem.LaserSourceSet.PowerMeterSetting.SysGain;//unit of gain : A/W
                                }
                                
                            }
                        }
                    }

                }
            }
            return new double[] { 1 };
        }
        // single Die Only

        protected virtual double[] VAC(TestItemData item, uint srcMeterItemIndex, uint srcChannel = 0, ISourceMeter srcMeter = null)
        {
            double[] sourceMeterReadData = null;

            if (srcMeter == null)
            {
                srcMeter = this._srcMeter;
            }
            if (item.IsEnable && srcMeter != null)
            {
                srcMeter.MeterOutput(this._srcSyncTrigger.ToArray(), srcMeterItemIndex);

                if (this._machineConfig.Enable.IsInstantGetData == true)
                {
                    sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex);

                    // (1) Current
                    item.MsrtResult[0].Unit = "mA";
                    item.MsrtResult[0].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EAmpUnit.A, item.MsrtResult[0].Unit) * this._chipPolarity;

                    // (2) Power
                    item.MsrtResult[1].Unit = "W";
                    item.MsrtResult[1].RawValue = sourceMeterReadData[1];// *UnitMath.UnitConvertFactor(EAmpUnit.A.ToString(), item.MsrtResult[0].Unit) * this._chipPolarity;

                    // (3) Apparent
                    item.MsrtResult[2].Unit = "VA";
                    item.MsrtResult[2].RawValue = sourceMeterReadData[2];// *UnitMath.UnitConvertFactor(EAmpUnit.A.ToString(), item.MsrtResult[0].Unit) * this._chipPolarity;

                    // (4) Power Factor
                    item.MsrtResult[3].Unit = "";
                    item.MsrtResult[3].RawValue = sourceMeterReadData[3];// *UnitMath.UnitConvertFactor(EAmpUnit.A.ToString(), item.MsrtResult[0].Unit) * this._chipPolarity;

                    // (5) Frequency
                    item.MsrtResult[4].Unit = "hz";
                    item.MsrtResult[4].RawValue = sourceMeterReadData[4];// *UnitMath.UnitConvertFactor(EAmpUnit.A.ToString(), item.MsrtResult[0].Unit) * this._chipPolarity;

                    // (6) Current Peak
                    item.MsrtResult[5].Unit = "mA";
                    item.MsrtResult[5].RawValue = sourceMeterReadData[5] * UnitMath.UnitConvertFactor(EAmpUnit.A, item.MsrtResult[0].Unit) * this._chipPolarity;

                    // (7) Current Peak Max
                    item.MsrtResult[6].Unit = "mA";
                    item.MsrtResult[6].RawValue = sourceMeterReadData[6] * UnitMath.UnitConvertFactor(EAmpUnit.A, item.MsrtResult[0].Unit) * this._chipPolarity;
                }
            }
            return sourceMeterReadData;
        }

        protected virtual double[] LIV(TestItemData item, uint srcMeterItemIndex, uint sptMeterItemIndex, uint srcChannel = 0, ISourceMeter srcMeter = null, ISpectroMeter sptMeter = null)
        {
            double[] sourceMeterReadData = null;

            if (srcMeter == null)
            {
                srcMeter = this._srcMeter;
            }
            if (sptMeter == null)
            {
                sptMeter = this._sptMeter;
            }
            LIVTestItem livItem = item as LIVTestItem;
            if (livItem.IsEnable)
            {
                livItem.ClearDataList();

                if (srcMeter != null)
                {
                    System.Threading.Thread.Sleep((int)livItem.LIVProcessDelayTime);
                }

                this._acquireData.LIVDataSet[item.KeyName].Type = livItem.LIVMsrtType.ToString();

                livItem.LIVProcessStart();

                for (uint i = 0; i < item.ElecSetting.Length; i++)
                {
                    this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVSETVALUE].DataArray[i] = (float)item.ElecSetting[i].ForceValue;

                    livItem.LIVFixedSITTimerStart();

                    //================================================================
                    //  [1] Source Meter force the current and measurement volt.
                    //================================================================
                    if (srcMeter != null)
                    {
                        srcMeter.MeterOutput(this._srcSyncTrigger.ToArray(), srcMeterItemIndex + i);

                        sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex + i);

                        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                        if (item.ElecSetting[i].MsrtType == EMsrtType.LIV)
                        {
                            // Apply I
                            this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVMsrtI].DataArray[i] = (float)item.ElecSetting[i].ForceValue;

                            // Msrt V
                            this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVMsrtV].DataArray[i] = (float)(sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[(int)ELIVOptiMsrtType.LIVMsrtV].Unit) * this._chipPolarity);
                        }
                        else
                        {
                            // Apply V
                            this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVMsrtV].DataArray[i] = (float)item.ElecSetting[i].ForceValue;

                            // Msrt I
                            this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVMsrtI].DataArray[i] = (float)(sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EAmpUnit.A, item.MsrtResult[(int)ELIVOptiMsrtType.LIVMsrtI].Unit) * this._chipPolarity);
                        }

                        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                        // Triger PD Sensing
                        if (this._machineConfig.PDSensingMode != EPDSensingMode.NONE && (item as LIVTestItem).LIVIsEnableDetector)
                        {
                            double pdCurrent = Math.Abs(sourceMeterReadData[1]);

                            this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVPDCURRENT].DataArray[i] = (float)(pdCurrent * UnitMath.UnitConvertFactor(EAmpUnit.A, item.MsrtResult[(int)ELIVOptiMsrtType.LIVPDCURRENT].Unit));

                            this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVPDWATT].DataArray[i] = (float)(pdCurrent * 1000.0d * _product.PdDetectorFactor * UnitMath.UnitConvertFactor(EWattUnit.mW, item.MsrtResult[(int)ELIVOptiMsrtType.LIVPDWATT].Unit));
                        }
                    }

                    //================================================================
                    // [2] Trigger the Spectrometer to get spectrum
                    //================================================================
                    if (sptMeter != null && (item as LIVTestItem).LIVIsTestOptical == true)
                    {
                        livItem.LIVLimitModeFixedSITTimerStart();

                        int rtnCode = sptMeter.Trigger(sptMeterItemIndex + i);

                        if (rtnCode > 0)
                        {
                            this._acquireData.LIVDataSet[item.KeyName].SpectrumDataData[(int)i].Wavelength = sptMeter.GetXWavelength();

                            this._acquireData.LIVDataSet[item.KeyName].SpectrumDataData[(int)i].Dark = sptMeter.DarkIntensityArray;

                            this._acquireData.LIVDataSet[item.KeyName].SpectrumDataData[(int)i].Intensity = sptMeter.GetYSpectrumIntensity(sptMeterItemIndex + i);

                            this._acquireData.LIVDataSet[item.KeyName].SpectrumDataData[(int)i].Absoluate = sptMeter.GetYAbsoluateSpectrum(sptMeterItemIndex + i);
                        }
                        else
                        {
                            if (rtnCode == -23)
                            {
                                // Re Test
                                //System.Threading.Thread.Sleep(50);
                                sptMeter.Trigger(sptMeterItemIndex + i);
                            }
                        }

                        livItem.LIVActiveLimitModeFixedSIT((int)(i));
                    }

                    //================================================================
                    // [3] Turn Off Time
                    //================================================================
                    if (srcMeter.ElecSetting != null && ((int)srcMeter.ElecSetting[srcMeterItemIndex + i].TurnOffTime) != 0)
                    {
                        srcMeter.TurnOff((int)srcMeter.ElecSetting[srcMeterItemIndex + i].TurnOffTime, false);
                    }

                    livItem.LIVActiveFixedSIT((int)(i));

                    this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVTIMEB].DataArray[i] = (float)livItem.LIVProcessTime();

                    //System.Threading.Thread.Sleep(0);
                }

                livItem.LIVProcessEnd();

                if (srcMeter != null)
                {
                    //System.Threading.Thread.Sleep(10);
                    srcMeter.TurnOff();
                }
            }
            return sourceMeterReadData;
        }

        protected virtual double[] VIScan(TestItemData item, uint srcMeterItemIndex, uint srcChannel = 0, ISourceMeter srcMeter = null)
        {
            uint dutChannel = 0;

            double[] sourceMeterReadData = null;

            if (srcMeter == null)
            {
                srcMeter = this._srcMeter;
            }
            if (item.IsEnable && srcMeter != null)
            {
                srcMeter.MeterOutput(this._srcSyncTrigger.ToArray(), srcMeterItemIndex);

                sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex);

                if (this._machineConfig.Enable.IsInstantGetData == true)
                {
                    //++++++++++++++++++++++++++++++++
                    // Get sweep data from source meter,
                    // Copy to "chartData"
                    //++++++++++++++++++++++++++++++++
                    this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].TimeChain = srcMeter.GetTimeChainFromMeter(srcChannel, srcMeterItemIndex).Clone() as double[];		// t : time (ms)
                    this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].ApplyData = srcMeter.GetSweepPointFromMeter(srcChannel, srcMeterItemIndex).Clone() as double[];		// x : input  (V)
                    this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].SweepData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex).Clone() as double[];			// y : output (A)

                    for (int k = 0; k < this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].TimeChain.Length; k++)
                    {
                        this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].ApplyData[k] = this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].ApplyData[k] * this._chipPolarity;		// To V
                        this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].SweepData[k] = this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].SweepData[k] * this._chipPolarity;   // To A
                        this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].TimeChain[k] *= 1000;//s -> ms
                    }

                    if(this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].SweepData.Length > 2)
                    {
                        List<double> valList = new List<double>();
                        valList.AddRange(this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].SweepData);

                        double zeroVal = valList[0];
                        double stableVal = valList[valList.Count - 1];

                        List<double> absList = (from v in valList
                                                orderby Math.Abs(v - zeroVal) descending
                                                    select v).ToList();

                        double peak = absList[0];
                        double diff = peak - stableVal;
                        double prak_90p = Math.Abs((stableVal - zeroVal) * 0.98);
                        int index09 = 0;
                        for (int i = 0; i < valList.Count; ++i)
                        {
                            double val = valList[i];
                            double aVAl = Math.Abs(val - zeroVal);
                            if (aVAl < prak_90p)
                            {
                                index09 = i;
                            } 
                        }

                        if (index09 == 0 && valList.Count > 1)
                        {
                            index09  = valList.Count  -1;
                        }

                        item.MsrtResult[0].RawValue = peak * UnitMath.UnitConvertFactor(EAmpUnit.A, item.MsrtResult[0].Unit) * this._chipPolarity ; //MSCANIPEAK_
                        item.MsrtResult[1].RawValue = stableVal * UnitMath.UnitConvertFactor(EAmpUnit.A, item.MsrtResult[1].Unit) * this._chipPolarity ;//MSCANISTABLE_
                        item.MsrtResult[2].RawValue = diff * UnitMath.UnitConvertFactor(EAmpUnit.A, item.MsrtResult[2].Unit) * this._chipPolarity ;//MSCANIDIFF_
                        item.MsrtResult[3].RawValue = this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].TimeChain[index09] *
                            UnitMath.UnitConvertFactor(ETimeUnit.ms, item.MsrtResult[3].Unit);//MSETTLING_
                    }
                    else
                    {
                        for (int i = 0; i < 4; ++i)
                        {
                            item.MsrtResult[i].RawValue = 0;
                        }
                    }
                    

                    /*
                    this._msrtResult[0].KeyName = "MSCANIPEAK_" + num.ToString();
                    this._msrtResult[0].Name = "IPeak" + num.ToString("D2");

                    this._msrtResult[1].KeyName = "MSCANISTABLE_" + num.ToString();
                    this._msrtResult[1].Name = "IStable" + num.ToString("D2");

                    this._msrtResult[2].KeyName = "MSCANIDIFF_" + num.ToString();
                    this._msrtResult[2].Name = "IDiff" + num.ToString("D2");

                    this._msrtResult[3].KeyName = "MSCANSTABLE_" + num.ToString();
                    this._msrtResult[3].Name = "Tstable" + num.ToString("D2");*/
                }
            }
            return this._acquireData.ElecSweepDataSet[dutChannel, item.KeyName].SweepData;
        }

        protected virtual double[] PIV(TestItemData item, uint srcMeterItemIndex, uint srcChannel = 0, ISourceMeter srcMeter = null)
        {
            if (srcMeter == null)
            {
                srcMeter = this._srcMeter;
            }
            if (item.IsEnable && srcMeter != null)
            {
                //++++++++++++++++++++++++++++++++
                // Get sweep data from source meter,
                // Copy to "chartData"
                //++++++++++++++++++++++++++++++++
                if (srcMeter.MeterOutput(this._srcSyncTrigger.ToArray(), srcMeterItemIndex))
                {
                    if (this._machineConfig.Enable.IsInstantGetData == true)
                    {
                        this._acquireData.PIVDataSet[item.KeyName].PowerData = srcMeter.GetSweepResultFromMeter(srcChannel, srcMeterItemIndex).Clone() as double[];		// 
                        this._acquireData.PIVDataSet[item.KeyName].CurrentData = srcMeter.GetSweepPointFromMeter(srcChannel, srcMeterItemIndex).Clone() as double[];		// x : input  (A)
                        this._acquireData.PIVDataSet[item.KeyName].VoltageData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex).Clone() as double[];			// y : output (V)

                        for (int k = 0; k < this._acquireData.PIVDataSet[item.KeyName].CurrentData.Length; k++)
                        {
                            this._acquireData.PIVDataSet[item.KeyName].PowerData[k] = this._acquireData.PIVDataSet[item.KeyName].PowerData[k] * _product.PdDetectorFactor * 1000.0d * this._detectorPolarity; ;

                            this._acquireData.PIVDataSet[item.KeyName].CurrentData[k] = this._acquireData.PIVDataSet[item.KeyName].CurrentData[k] * this._chipPolarity;

                            this._acquireData.PIVDataSet[item.KeyName].VoltageData[k] = this._acquireData.PIVDataSet[item.KeyName].VoltageData[k] * this._chipPolarity;
                        }
                    }

                    #region >>> Laser Characteristic Calculation <<<

                    this._laserPostCalc.CalibratedPowerFactor = _product.PdDetectorFactor;

                    this._laserPostCalc.SettingData = (item as PIVTestItem).CalcSetting.Clone() as LaserCalcSetting;


                    this._laserPostCalc.CalcParameter(this._acquireData.PIVDataSet[item.KeyName].PowerData,
                                                      this._acquireData.PIVDataSet[item.KeyName].CurrentData,
                                                      this._acquireData.PIVDataSet[item.KeyName].VoltageData);

                    item.MsrtResult[(int)ELaserMsrtType.Pop].RawValue = this._laserPostCalc.CharacteristicResults.Pop;
                    item.MsrtResult[(int)ELaserMsrtType.Iop].RawValue = this._laserPostCalc.CharacteristicResults.Iop * 1000.0d;     // A -> mA
                    item.MsrtResult[(int)ELaserMsrtType.Vop].RawValue = this._laserPostCalc.CharacteristicResults.Vop;  // V -> V
                    item.MsrtResult[(int)ELaserMsrtType.Pceop].RawValue = this._laserPostCalc.CharacteristicResults.Pceop;  // %
                    item.MsrtResult[(int)ELaserMsrtType.Imop].RawValue = this._laserPostCalc.CharacteristicResults.Imop * 1000.0d;     // A -> mA

                    item.MsrtResult[(int)ELaserMsrtType.Ipk].RawValue = this._laserPostCalc.CharacteristicResults.Ipk * 1000.0d;     // A -> mA
                    item.MsrtResult[(int)ELaserMsrtType.Ppk].RawValue = this._laserPostCalc.CharacteristicResults.Ppk;   // mW    
                    item.MsrtResult[(int)ELaserMsrtType.Vpk].RawValue = this._laserPostCalc.CharacteristicResults.Vpk;   // V   
                    item.MsrtResult[(int)ELaserMsrtType.Impk].RawValue = this._laserPostCalc.CharacteristicResults.Impk * 1000.0d;     // A -> mA
                    item.MsrtResult[(int)ELaserMsrtType.Pcepk].RawValue = this._laserPostCalc.CharacteristicResults.Pcepk;  // %

                    item.MsrtResult[(int)ELaserMsrtType.Pth].RawValue = this._laserPostCalc.CharacteristicResults.Pth; // mW
                    item.MsrtResult[(int)ELaserMsrtType.Ith].RawValue = this._laserPostCalc.CharacteristicResults.Ith * 1000.0d; // A -> mA
                    //item.MsrtResult[(int)ELaserMsrtType.Vth].RawValue = this._laserPostCalc.CharacteristicResults.Vth; // V
                    item.MsrtResult[(int)ELaserMsrtType.Vth].RawValue = 0; // V

                    item.MsrtResult[(int)ELaserMsrtType.SE].RawValue = this._laserPostCalc.CharacteristicResults.SE;   // W/A
                    item.MsrtResult[(int)ELaserMsrtType.SE2].RawValue = this._laserPostCalc.CharacteristicResults.SE2;   // W/A

                    item.MsrtResult[(int)ELaserMsrtType.RS].RawValue = this._laserPostCalc.CharacteristicResults.Rs;        // ohm      

                    item.MsrtResult[(int)ELaserMsrtType.Kink].RawValue = this._laserPostCalc.CharacteristicResults.Kink;
                    item.MsrtResult[(int)ELaserMsrtType.Ikink].RawValue = this._laserPostCalc.CharacteristicResults.Ikink * 1000.0d; // A -> mA
                    item.MsrtResult[(int)ELaserMsrtType.Pkink].RawValue = this._laserPostCalc.CharacteristicResults.Pkink;
                    // item.MsrtResult[(int)ELaserMsrtType.Icod].RawValue = this._laserPostCalc.CharacteristicResults.Icod * 1000.0d;  // A -> mA
                    // item.MsrtResult[(int)ELaserMsrtType.Pcod].RawValue = this._laserPostCalc.CharacteristicResults.Pcod;
                    item.MsrtResult[(int)ELaserMsrtType.Linearity].RawValue = this._laserPostCalc.CharacteristicResults.Linearity;  // %
                    item.MsrtResult[(int)ELaserMsrtType.Linearity2].RawValue = this._laserPostCalc.CharacteristicResults.Linearity2;  // %

                    item.MsrtResult[(int)ELaserMsrtType.Rollover].RawValue = this._laserPostCalc.CharacteristicResults.Rollover;  // %

                    item.MsrtResult[(int)ELaserMsrtType.Icod].RawValue = this._laserPostCalc.CharacteristicResults.Icod * 1000.0d; // A -> mA
                    item.MsrtResult[(int)ELaserMsrtType.Pcod].RawValue = this._laserPostCalc.CharacteristicResults.Pcod;  // mW

                    item.MsrtResult[(int)ELaserMsrtType.PfA].RawValue = this._laserPostCalc.CharacteristicResults.PfA;
                    item.MsrtResult[(int)ELaserMsrtType.VfA].RawValue = this._laserPostCalc.CharacteristicResults.VfA;  // V -> V
                    item.MsrtResult[(int)ELaserMsrtType.RdA].RawValue = this._laserPostCalc.CharacteristicResults.RdA;   // ohm 
                    item.MsrtResult[(int)ELaserMsrtType.PceA].RawValue = this._laserPostCalc.CharacteristicResults.PceA;  // %

                    item.MsrtResult[(int)ELaserMsrtType.PfB].RawValue = this._laserPostCalc.CharacteristicResults.PfB;
                    item.MsrtResult[(int)ELaserMsrtType.VfB].RawValue = this._laserPostCalc.CharacteristicResults.VfB;  // V -> V
                    item.MsrtResult[(int)ELaserMsrtType.RdB].RawValue = this._laserPostCalc.CharacteristicResults.RdB;   // ohm 
                    item.MsrtResult[(int)ELaserMsrtType.PceB].RawValue = this._laserPostCalc.CharacteristicResults.PceB;  // %

                    item.MsrtResult[(int)ELaserMsrtType.PfC].RawValue = this._laserPostCalc.CharacteristicResults.PfC;
                    item.MsrtResult[(int)ELaserMsrtType.VfC].RawValue = this._laserPostCalc.CharacteristicResults.VfC;  // V -> V
                    item.MsrtResult[(int)ELaserMsrtType.RdC].RawValue = this._laserPostCalc.CharacteristicResults.RdC;   // ohm 
                    item.MsrtResult[(int)ELaserMsrtType.PceC].RawValue = this._laserPostCalc.CharacteristicResults.PceC;  // %


                    this._acquireData.PIVDataSet[item.KeyName].SeData = this._laserPostCalc.Curve.SeData;
                    this._acquireData.PIVDataSet[item.KeyName].RsData = this._laserPostCalc.Curve.RsData;
                    this._acquireData.PIVDataSet[item.KeyName].PceData = this._laserPostCalc.Curve.PceData;

                    #endregion
                }
                else
                {
                    this.SetErrorCode(EDevErrorNumber.MeterOutput_Ctrl_Err);
                }
            }

            return this._acquireData.PIVDataSet[item.KeyName].PowerData;
        }


        protected virtual double[] OSA(TestItemData item, uint srcMeterItemIndex, uint osaItemIndex, uint srcChannel, ISourceMeter srcMeter = null, IOSA osa = null)
        {
            double[] sourceMeterReadData = null;
            if (srcMeter == null)
            {
                srcMeter = this._srcMeter;
            }
            if (osa == null)
            {
                osa = this._osaDevice;
            }
            //================================================================
            //  [1] Source Meter force the current and measurement volt.
            //================================================================
            if (item.IsEnable && srcMeter != null)
            {
                srcMeter.MeterOutput(this._srcSyncTrigger.ToArray(), srcMeterItemIndex);

                if (this._machineConfig.Enable.IsInstantGetData == true)
                {
                    sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex);

                    item.MsrtResult[(int)EOsaOptiMsrtType.OSAMsrtVs].Unit = "V";

                    item.MsrtResult[(int)EOsaOptiMsrtType.OSAMsrtVs].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[(int)EOptiMsrtType.MVFLA].Unit) * this._chipPolarity;
                }
            }


            //================================================================
            // [2] Trigger the Spectrometer to get spectrum
            //================================================================
            if (osa != null && (item as OSATestItem).IsTestOptical == true)
            {
                osa.Trigger(osaItemIndex);
            }
            else if (srcMeter != null && (item as OSATestItem).IsTestOptical == false)
            {
                srcMeter.TurnOff(0.0d, false);
            }


            //================================================================
            // [3] Continuous force the same current and measurement volt. (MV)
            //================================================================
            if (srcMeter != null)
            {
                srcMeter.MeterOutput(this._srcSyncTrigger.ToArray(), srcMeterItemIndex + 1);

                if (this._machineConfig.Enable.IsInstantGetData == true)
                {
                    sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex + 1);

                    item.MsrtResult[(int)EOsaOptiMsrtType.OSAMsrtVe].Unit = "V";

                    item.MsrtResult[(int)EOsaOptiMsrtType.OSAMsrtVe].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[(int)EOptiMsrtType.MVFLB].Unit) * this._chipPolarity;
                }
            }


            if (osa != null)
            {
                //this._osaDevice.CalculateMeasureResultData(osaItemIndex);

                OsaData od = osa.Data[osaItemIndex];

                item.MsrtResult[(int)EOsaOptiMsrtType.OSAMeanWl].RawValue = od.MeanWL;
                item.MsrtResult[(int)EOsaOptiMsrtType.OSAPeakWl].RawValue = od.PeakWL;
                item.MsrtResult[(int)EOsaOptiMsrtType.OSAPeakLvl].RawValue = od.PeakLevel;
                item.MsrtResult[(int)EOsaOptiMsrtType.OSA2ndPeak].RawValue = od.PeakWL2;
                item.MsrtResult[(int)EOsaOptiMsrtType.OSA2ndPeakLvl].RawValue = od.PeakLevel2;
                item.MsrtResult[(int)EOsaOptiMsrtType.OSAFWHMrms].RawValue = od.FWHM;
                item.MsrtResult[(int)EOsaOptiMsrtType.OSATotalPower].RawValue = od.TotalPower;
                item.MsrtResult[(int)EOsaOptiMsrtType.OSASMSR].RawValue = od.SMSR;
                item.MsrtResult[(int)EOsaOptiMsrtType.OSADeltaLamda].RawValue = od.DeltaLamda;

                item.MsrtResult[(int)EOsaOptiMsrtType.OSAStdev].RawValue = od.Stdev;
                item.MsrtResult[(int)EOsaOptiMsrtType.OSARMS].RawValue = od.RMS;

            }

            return sourceMeterReadData;
        }

        
        protected virtual double[] VLR(TestItemData item, uint srcMeterItemIndex, uint sptMeterItemIndex, double[][] chartData, uint srcChannel, ISourceMeter srcMeter = null)
        {
            if (srcMeter == null)
            {
                srcMeter = this._srcMeter;
            }
            if (item.IsEnable && srcMeter != null)
            {
                srcMeter.MeterOutput(this._srcSyncTrigger.ToArray(), srcMeterItemIndex);

                int rtnCode = this._sptMeter.Trigger(sptMeterItemIndex);

                if (rtnCode > 0)
                {
                    chartData[0] = this._sptMeter.GetXWavelength();
                    chartData[1] = this._sptMeter.GetYSpectrumIntensity(sptMeterItemIndex);
                    // Create the dumy space for the array,
                    // The true absolute intensity data will calc by "Spt.Calculate()"
                    chartData[2] = this._sptMeter.GetYAbsoluateSpectrum(sptMeterItemIndex);
                    chartData[3] = this._sptMeter.DarkIntensityArray;

                    // chartData.CopyTo(this._chart[item.KeyName], 4);
                }
                else
                {
                    if (rtnCode == -23)
                    {
                        this._isTriggerSptErr = true;
                    }  // Re Test
                    //--------------------------------------------------------
                    // Error : for spectrometer Trigger Function
                    //		   then break the all "Test Process"
                    //---------------------------------------------------------
                    srcMeter.TurnOff(0.0d, false);
                    //break;
                }

                //(srcMeter as Keithley2600).TurnOffVLR(srcMeterItemIndex);

                if (this._machineConfig.Enable.IsInstantGetData == true)
                {
                    chartData[0] = (srcMeter.GetTimeChainFromMeter(srcChannel, srcMeterItemIndex).Clone() as double[]);		// t : time (ms)
                    chartData[1] = (srcMeter.GetSweepPointFromMeter(srcChannel, srcMeterItemIndex).Clone() as double[]);		// x : input  (A)
                    chartData[2] = (srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex).Clone() as double[]);			// y : output (V)			// y : output (V)

                    for (int k = 0; k < chartData[0].Length; k++)
                    {
                        chartData[1][k] = chartData[1][k] * 1000 * this._chipPolarity;		// To mA
                        chartData[2][k] = chartData[2][k];// * this._chipPolarity;		    // To V
                    }

                    // chartData.CopyTo(this._chart[item.KeyName], 0);

                    double[] sweepResult = srcMeter.GetSweepResultFromMeter(srcChannel, srcMeterItemIndex);
                    item.MsrtResult[0].Unit = "V";
                    item.MsrtResult[1].Unit = "V";
                    item.MsrtResult[2].Unit = "V";
                    item.MsrtResult[3].Unit = "V";
                    item.MsrtResult[4].Unit = "V";
                    item.MsrtResult[5].Unit = "V";
                    item.MsrtResult[6].Unit = "V";
                    item.MsrtResult[7].Unit = "V";
                    item.MsrtResult[8].Unit = "V";
                    item.MsrtResult[9].Unit = "V";
                    item.MsrtResult[10].Unit = "V";
                    item.MsrtResult[(int)EVLROptiMsrtType.VLRVA].RawValue = sweepResult[0] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[0].Unit);
                    item.MsrtResult[(int)EVLROptiMsrtType.VLRVB].RawValue = sweepResult[1] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[1].Unit);
                    item.MsrtResult[(int)EVLROptiMsrtType.VLRVC].RawValue = sweepResult[2] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[2].Unit);
                    item.MsrtResult[(int)EVLROptiMsrtType.VLRVD].RawValue = sweepResult[3] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[3].Unit);
                    item.MsrtResult[(int)EVLROptiMsrtType.VLRDVA].RawValue = sweepResult[4] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[4].Unit);
                    item.MsrtResult[(int)EVLROptiMsrtType.VLRDVB].RawValue = sweepResult[5] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[5].Unit);
                    item.MsrtResult[(int)EVLROptiMsrtType.VLRDVC].RawValue = sweepResult[6] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[6].Unit);
                    item.MsrtResult[(int)EVLROptiMsrtType.VLRVAS].RawValue = sweepResult[7] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[7].Unit);
                    item.MsrtResult[(int)EVLROptiMsrtType.VLRVBS].RawValue = sweepResult[8] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[8].Unit);
                    item.MsrtResult[(int)EVLROptiMsrtType.VLRVCS].RawValue = sweepResult[9] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[9].Unit);
                    item.MsrtResult[(int)EVLROptiMsrtType.VLRVDS].RawValue = sweepResult[10] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[10].Unit);
                }
            }
            return new double[] { 1 };
        }

        protected virtual double[] LCR(TestItemData item, uint srcMeterItemIndex, uint lcrMeterItemIndex, List<uint> trigList = null, uint srcChannel = 0,ILCRMeter lcrMeter = null, ISourceMeter srcMeter = null)
        {
           
            if (trigList == null)
            {
                trigList = this._srcSyncTrigger;
            }
            if (srcMeter == null)
            {
                srcMeter = this._lcrBias;
            }

            if (lcrMeter == null)
            {
                lcrMeter = this._lcrMeter;
            }
            double[] sourceMeterReadData = null;

            srcChannel = 0;

            #region >>> LCR <<<

            if (item.IsEnable && this._lcrMeter != null)
            {
                
                srcMeter = this._lcrBias;

                uint biasIndex = lcrMeterItemIndex;

                if (this._machineConfig.LCRDCBiasType == ELCRDCBiasType.Ext_Master ||
                    (this._machineConfig.SourceMeterModel == ESourceMeterModel.K2600 &&
                    this._machineConfig.LCRDCBiasType == ELCRDCBiasType.Ext_Other &&
                    this._machineConfig.LCRDCBiasSource == ELCRDCBiasSource.K2600))
                {
                    srcMeter = this._srcMeter;

                    biasIndex = srcMeterItemIndex;
                }

                //pt2.Start();

                lcrMeter.PreSettingParamToMeter(lcrMeterItemIndex);
                //double t = pt2.PeekTimeSpan(ETimeSpanUnit.MilliSecond);
                //Console.WriteLine("[LernelBase_LCR],PreSettingParamToMeter," + t.ToString());

                if (srcMeter != null && item.ElecSetting[0].ForceValue != 0)
                {
                    srcMeter.MeterOutput(trigList.ToArray(), biasIndex);
                }

                lcrMeter.MeterOutput(trigList.ToArray(), lcrMeterItemIndex);
                //t = pt2.PeekTimeSpan(ETimeSpanUnit.MilliSecond);
                //Console.WriteLine("[LernelBase_LCR],MeterOutput," + t.ToString());

                lcrMeter.TurnOff(lcrMeterItemIndex);

                //t = pt2.PeekTimeSpan(ETimeSpanUnit.MilliSecond);
                //Console.WriteLine("[LernelBase_LCR],TurnOff," + t.ToString());

                if (srcMeter != null && item.ElecSetting[0].ForceValue != 0)
                {
                    srcMeter.TurnOff();
                }

                if (this._machineConfig.Enable.IsInstantGetData == true)
                {
                    sourceMeterReadData = lcrMeter.GetDataFromMeter(srcChannel, lcrMeterItemIndex);

                    sourceMeterReadData[(int)ELCRMsrtType.LCRB] = sourceMeterReadData[(int)ELCRMsrtType.LCRB] * UnitMath.UnitConvertFactor(ESieUnit.S, item.MsrtResult[(int)ELCRMsrtType.LCRB].Unit);
                    sourceMeterReadData[(int)ELCRMsrtType.LCRCP] = sourceMeterReadData[(int)ELCRMsrtType.LCRCP] * UnitMath.UnitConvertFactor(ECapUnit.F, item.MsrtResult[(int)ELCRMsrtType.LCRCP].Unit);
                    sourceMeterReadData[(int)ELCRMsrtType.LCRCS] = sourceMeterReadData[(int)ELCRMsrtType.LCRCS] * UnitMath.UnitConvertFactor(ECapUnit.F, item.MsrtResult[(int)ELCRMsrtType.LCRCS].Unit);
                    sourceMeterReadData[(int)ELCRMsrtType.LCRD] = sourceMeterReadData[(int)ELCRMsrtType.LCRD];
                    sourceMeterReadData[(int)ELCRMsrtType.LCRG] = sourceMeterReadData[(int)ELCRMsrtType.LCRG] * UnitMath.UnitConvertFactor(ESieUnit.S, item.MsrtResult[(int)ELCRMsrtType.LCRG].Unit);
                    sourceMeterReadData[(int)ELCRMsrtType.LCRIDC] = sourceMeterReadData[(int)ELCRMsrtType.LCRIDC] * UnitMath.UnitConvertFactor(EAmpUnit.A, item.MsrtResult[(int)ELCRMsrtType.LCRIDC].Unit);
                    sourceMeterReadData[(int)ELCRMsrtType.LCRLP] = sourceMeterReadData[(int)ELCRMsrtType.LCRLP] * UnitMath.UnitConvertFactor(EIndUnit.H, item.MsrtResult[(int)ELCRMsrtType.LCRLP].Unit);
                    sourceMeterReadData[(int)ELCRMsrtType.LCRLS] = sourceMeterReadData[(int)ELCRMsrtType.LCRLS] * UnitMath.UnitConvertFactor(EIndUnit.H, item.MsrtResult[(int)ELCRMsrtType.LCRLS].Unit);
                    sourceMeterReadData[(int)ELCRMsrtType.LCRQ] = sourceMeterReadData[(int)ELCRMsrtType.LCRQ];
                    sourceMeterReadData[(int)ELCRMsrtType.LCRR] = sourceMeterReadData[(int)ELCRMsrtType.LCRR] * UnitMath.UnitConvertFactor(EOhmUnit.Ohm, item.MsrtResult[(int)ELCRMsrtType.LCRR].Unit);
                    sourceMeterReadData[(int)ELCRMsrtType.LCRRDC] = sourceMeterReadData[(int)ELCRMsrtType.LCRRDC] * UnitMath.UnitConvertFactor(EOhmUnit.Ohm, item.MsrtResult[(int)ELCRMsrtType.LCRRDC].Unit);
                    sourceMeterReadData[(int)ELCRMsrtType.LCRRP] = sourceMeterReadData[(int)ELCRMsrtType.LCRRP] * UnitMath.UnitConvertFactor(EOhmUnit.Ohm, item.MsrtResult[(int)ELCRMsrtType.LCRRP].Unit);
                    sourceMeterReadData[(int)ELCRMsrtType.LCRRS] = sourceMeterReadData[(int)ELCRMsrtType.LCRRS] * UnitMath.UnitConvertFactor(EOhmUnit.Ohm, item.MsrtResult[(int)ELCRMsrtType.LCRRS].Unit);
                    sourceMeterReadData[(int)ELCRMsrtType.LCRTD] = sourceMeterReadData[(int)ELCRMsrtType.LCRTD];
                    sourceMeterReadData[(int)ELCRMsrtType.LCRTR] = sourceMeterReadData[(int)ELCRMsrtType.LCRTR];
                    sourceMeterReadData[(int)ELCRMsrtType.LCRVDC] = sourceMeterReadData[(int)ELCRMsrtType.LCRVDC] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[(int)ELCRMsrtType.LCRVDC].Unit);
                    sourceMeterReadData[(int)ELCRMsrtType.LCRX] = sourceMeterReadData[(int)ELCRMsrtType.LCRX] * UnitMath.UnitConvertFactor(EOhmUnit.Ohm, item.MsrtResult[(int)ELCRMsrtType.LCRX].Unit);
                    sourceMeterReadData[(int)ELCRMsrtType.LCRY] = sourceMeterReadData[(int)ELCRMsrtType.LCRY] * UnitMath.UnitConvertFactor(ESieUnit.S, item.MsrtResult[(int)ELCRMsrtType.LCRY].Unit);
                    sourceMeterReadData[(int)ELCRMsrtType.LCRZ] = sourceMeterReadData[(int)ELCRMsrtType.LCRZ] * UnitMath.UnitConvertFactor(EOhmUnit.Ohm, item.MsrtResult[(int)ELCRMsrtType.LCRZ].Unit);

                    for (int i = 0; i < item.MsrtResult.Length; i++)
                    {
                        //if (item.MsrtResult[i].IsEnable)
                        {
                            item.MsrtResult[i].RawValue = sourceMeterReadData[i];
                        }
                    }
                }
            }

            lcrMeterItemIndex++;

            if (this._lcrBias != null)
            {
                srcMeterItemIndex++;
            }



            #endregion
            return sourceMeterReadData;
        }

        protected virtual double[] LCRSweep(TestItemData item, uint lcrMeterItemIndex, List<uint> trigList = null, uint srcChannel = 0, ILCRMeter lcrMeter = null)
        {
            if (trigList == null)
            {
                trigList = this._srcSyncTrigger;
            }

            if (lcrMeter == null)
            {
                lcrMeter = this._lcrMeter;
            }

            List<double> outData = new List<double>();

            if (item.IsEnable && lcrMeter != null)
            {

                lcrMeter.PreSetBiasListToMeter(lcrMeterItemIndex);

                LCRSweepTestItem lcrItem = item as LCRSweepTestItem;

                if (this._machineConfig.Enable.IsInstantGetData == true)
                {
                    this._acquireData.LCRDataSet[item.KeyName].Type = lcrItem.Type.ToString();

                    lcrMeter.MeterOutput(this._srcSyncTrigger.ToArray(), lcrMeterItemIndex);

                    List<List<float>> MsrtLL = lcrMeter.GetDataFromMeter(lcrMeterItemIndex);

                    for (int i = 0; i < lcrMeter.LCRSetting[lcrMeterItemIndex].Point; ++i)
                    {
                        int lcrShiftPoint = this._acquireData.LIVDataSet.Count * i;

                        foreach (int index in Enum.GetValues(typeof(ELCRMsrtType)))
                        {
                            if (MsrtLL[index] != null && MsrtLL[index].Count > 0)
                            {
                                this._acquireData.LCRDataSet[lcrItem.KeyName][index].DataArray[i] = (float)(MsrtLL[index][i]);
                            }
                        }

                        this._acquireData.LCRDataSet[lcrItem.KeyName][(int)ELCRMsrtType.LCRCP].DataArray[i] *= (float)(UnitMath.UnitConvertFactor(ECapUnit.F, item.MsrtResult[(int)ELCRMsrtType.LCRCP].Unit));
                        this._acquireData.LCRDataSet[lcrItem.KeyName][(int)ELCRMsrtType.LCRCS].DataArray[i] *= (float)(UnitMath.UnitConvertFactor(ECapUnit.F, item.MsrtResult[(int)ELCRMsrtType.LCRCS].Unit));

                        this._acquireData.LCRDataSet[lcrItem.KeyName][(int)ELCRMsrtType.LCRLP].DataArray[i] *= (float)(UnitMath.UnitConvertFactor(EIndUnit.H, item.MsrtResult[(int)ELCRMsrtType.LCRLP].Unit));
                        this._acquireData.LCRDataSet[lcrItem.KeyName][(int)ELCRMsrtType.LCRLS].DataArray[i] *= (float)(UnitMath.UnitConvertFactor(EIndUnit.H, item.MsrtResult[(int)ELCRMsrtType.LCRLS].Unit));

                        this._acquireData.LCRDataSet[lcrItem.KeyName][(int)ELCRMsrtType.LCRG].DataArray[i] *= (float)(UnitMath.UnitConvertFactor(ESieUnit.S, item.MsrtResult[(int)ELCRMsrtType.LCRG].Unit));
                        this._acquireData.LCRDataSet[lcrItem.KeyName][(int)ELCRMsrtType.LCRR].DataArray[i] *= (float)(UnitMath.UnitConvertFactor(EOhmUnit.Ohm, item.MsrtResult[(int)ELCRMsrtType.LCRR].Unit));
                        this._acquireData.LCRDataSet[lcrItem.KeyName][(int)ELCRMsrtType.LCRRP].DataArray[i] *= (float)(UnitMath.UnitConvertFactor(EOhmUnit.Ohm, item.MsrtResult[(int)ELCRMsrtType.LCRRP].Unit));
                        this._acquireData.LCRDataSet[lcrItem.KeyName][(int)ELCRMsrtType.LCRRS].DataArray[i] *= (float)(UnitMath.UnitConvertFactor(EOhmUnit.Ohm, item.MsrtResult[(int)ELCRMsrtType.LCRRS].Unit));

                        this._acquireData.LCRDataSet[lcrItem.KeyName][(int)ELCRMsrtType.LCRX].DataArray[i] *= (float)(UnitMath.UnitConvertFactor(EOhmUnit.Ohm, item.MsrtResult[(int)ELCRMsrtType.LCRX].Unit));
                        this._acquireData.LCRDataSet[lcrItem.KeyName][(int)ELCRMsrtType.LCRY].DataArray[i] *= (float)(UnitMath.UnitConvertFactor(ESieUnit.S, item.MsrtResult[(int)ELCRMsrtType.LCRY].Unit));
                        this._acquireData.LCRDataSet[lcrItem.KeyName][(int)ELCRMsrtType.LCRZ].DataArray[i] *= (float)(UnitMath.UnitConvertFactor(EOhmUnit.Ohm, item.MsrtResult[(int)ELCRMsrtType.LCRZ].Unit));

                        this._acquireData.LCRDataSet[lcrItem.KeyName][(int)ELCRMsrtType.LCRB].DataArray[i] *= (float)(UnitMath.UnitConvertFactor(ESieUnit.S, item.MsrtResult[(int)ELCRMsrtType.LCRB].Unit));
                        this._acquireData.LCRDataSet[lcrItem.KeyName][(int)ELCRMsrtType.LCRIDC].DataArray[i] *= (float)(UnitMath.UnitConvertFactor(EAmpUnit.A, item.MsrtResult[(int)ELCRMsrtType.LCRIDC].Unit));
                        this._acquireData.LCRDataSet[lcrItem.KeyName][(int)ELCRMsrtType.LCRVDC].DataArray[i] *= (float)(UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[(int)ELCRMsrtType.LCRVDC].Unit));
                        this._acquireData.LCRDataSet[lcrItem.KeyName][(int)ELCRMsrtType.LCRRDC].DataArray[i] *= (float)(UnitMath.UnitConvertFactor(EOhmUnit.Ohm, item.MsrtResult[(int)ELCRMsrtType.LCRRDC].Unit));

                    }
                    outData = new List<double>();

                    for (int i = 0; i < item.MsrtResult.Length; i++)
                    {
                        int arrLast = this._acquireData.LCRDataSet[lcrItem.KeyName][i].DataArray.Length - 1;

                        item.MsrtResult[i].RawValue = this._acquireData.LCRDataSet[lcrItem.KeyName][i].DataArray[arrLast];
                    }
                }

                lcrMeterItemIndex++;
                //List<double> outData 
                
                foreach (float val in this._acquireData.LCRDataSet[lcrItem.KeyName][(int)ELCRMsrtType.LCRCP].DataArray)
                {
                    outData.Add((double)val);
                }
            }


            return outData.ToArray();
        }
        //protected virtual double[] FFP(TestItemData item, uint srcMeterItemIndex, uint srcChannel, ISourceMeter srcMeter = null)
        //{
        //    double[] sourceMeterReadData = null;

        //    if (srcMeter == null)
        //    {
        //        srcMeter = this._srcMeter;
        //    }

        //    if (item.IsEnable && srcMeter != null)
        //    {
        //        _timerrrr.Start();

        //        if ((item as FFPTestItem).Param.ROIArr != null)
        //        {
        //            this._sysSetting.FFPSetting.FfpArrayAnalysisParam.ROIArr = (item as FFPTestItem).Param.ROIArr.Clone() as ROI_U[];
        //        }

        //        //   this._camera.Snap();
        //        //----------------------------------------------------------------------------------
        //        // (1) EMsrtType.LOP ( isAutoTurnOff == false )
        //        //----------------------------------------------------------------------------------

        //        isGetCarmeraValue = false;

        //        srcMeter.MeterOutput(this._srcSyncTrigger.ToArray(), srcMeterItemIndex);

        //        sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex);

        //        item.MsrtResult[0].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[0].Unit) * this._chipPolarity; // PDMVFLA

        //        srcMeter.TurnOff();


        //        if (_camera2 != null)
        //        {
        //            _autoRestEvent_Camera2.WaitOne(2000);

        //            double spanTime = _timerrrr.PeekTimeSpan(ETimeSpanUnit.MilliSecond);

        //            //item.MsrtResult[0].RawValue = spanTime;

        //            if (spanTime > 2000)
        //            {
        //                Console.WriteLine("[HS_TesterKernel], FFP(), Camera2 TimeOut");

        //                //   this._camera.Snap();
        //                //----------------------------------------------------------------------------------
        //                // (1) EMsrtType.LOP ( isAutoTurnOff == false )
        //                //----------------------------------------------------------------------------------

        //                isGetCarmeraValue = false;

        //                srcMeter.MeterOutput(this._srcSyncTrigger.ToArray(), srcMeterItemIndex);

        //                sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex);

        //                item.MsrtResult[0].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[0].Unit) * this._chipPolarity; // PDMVFLA

        //                srcMeter.TurnOff();

        //                _autoRestEvent_Camera2.WaitOne(2000);

        //            }
        //        }
        //        else
        //        {
        //            // 只有 FFP 時, 只使用 Camera1

        //            _autoRestEvent_Camera.WaitOne(2000);

        //            double spanTime = _timerrrr.PeekTimeSpan(ETimeSpanUnit.MilliSecond);

        //            //item.MsrtResult[0].RawValue = spanTime;

        //            if (spanTime > 2000)
        //            {
        //                Console.WriteLine("[HS_TesterKernel], FFP(), Camera1 TimeOut");

        //                //   this._camera.Snap();
        //                //----------------------------------------------------------------------------------
        //                // (1) EMsrtType.LOP ( isAutoTurnOff == false )
        //                //----------------------------------------------------------------------------------

        //                isGetCarmeraValue = false;

        //                srcMeter.MeterOutput(this._srcSyncTrigger.ToArray(), srcMeterItemIndex);

        //                sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex);

        //                item.MsrtResult[0].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[0].Unit) * this._chipPolarity; // PDMVFLA

        //                srcMeter.TurnOff();

        //                _autoRestEvent_Camera.WaitOne(2000);

        //            }
        //        }
        //    }

        //    //if (this.FFresult != null)
        //    //{
        //    //    this._acquireData.FFPDataSet[item.KeyName].FFresult = this.FFresult.Clone() as PatternAnalysisResult<UInt32>;
        //    //}

        //    return sourceMeterReadData;

        //}

        //protected virtual double[] NFP(TestItemData item, uint srcMeterItemIndex, uint srcChannel, ISourceMeter srcMeter = null)
        //{
        //    double[] sourceMeterReadData = null;

        //    if (srcMeter == null)
        //    {
        //        srcMeter = this._srcMeter;
        //    }

        //    if (item.IsEnable && srcMeter != null)
        //    {
        //        _timerrrr.Start();

        //        //----------------------------------------------------------------------------------
        //        // (1) EMsrtType.LOP ( isAutoTurnOff == false )
        //        //----------------------------------------------------------------------------------
        //        srcMeter.MeterOutput(this._srcSyncTrigger.ToArray(), srcMeterItemIndex);

        //        sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex);

        //        item.MsrtResult[0].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[0].Unit) * this._chipPolarity; // PDMVFLA

        //        srcMeter.TurnOff();

        //        _autoRestEvent_Camera.WaitOne(2000);

        //        double spanTime2 = _timerrrr.PeekTimeSpan(ETimeSpanUnit.MilliSecond);

        //        if (spanTime2 > 2000)
        //        {
        //            Console.WriteLine("[HS_TesterKernel], NFP(), Camera TimeOut");


        //            isGetCarmeraValue = false;

        //            srcMeter.MeterOutput(this._srcSyncTrigger.ToArray(), srcMeterItemIndex);

        //            sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex);

        //            item.MsrtResult[0].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[0].Unit) * this._chipPolarity; // PDMVFLA

        //            srcMeter.TurnOff();

        //            _autoRestEvent_Camera.WaitOne(2000);
        //        }
        //        if (this.NFresult != null)
        //        {
        //            this._acquireData.NFPDataSet[item.KeyName].NFresult = this.NFresult.Clone() as PatternArrayAnalysisResult<UInt32>;
        //        }

        //    }
        //    return sourceMeterReadData;
        //    //_timerrrr.Start();

        //    //if (item.IsEnable && srcMeter != null)
        //    //{
        //    //    //----------------------------------------------------------------------------------
        //    //    // (1) EMsrtType.LOP ( isAutoTurnOff == false )
        //    //    //----------------------------------------------------------------------------------
        //    //    srcMeter.MeterOutput(this._srcSyncTrigger.ToArray(), srcMeterItemIndex);

        //    //    sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex);

        //    //    item.MsrtResult[0].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[0].Unit) * this._chipPolarity; // PDMVFLA

        //    //    srcMeter.TurnOff();
        //    //}
        //    //return sourceMeterReadData;
        //}

        //protected virtual double[] NFPBlinking(TestItemData item, uint srcMeterItemIndex, uint srcChannel, ISourceMeter srcMeter = null)
        //{
        //    double[] sourceMeterReadData = null;

        //    if (srcMeter == null)
        //    {
        //        srcMeter = this._srcMeter;
        //    }

        //    if (item.IsEnable && srcMeter != null)
        //    {
        //        this._isBlinkingTest = true;

        //        uint blinkingCnt = (item as NFPBlinkingTestItem).BlinkingCount;
        //        //(item as NFPBlinkingTestItem).MsrtResult

        //        List<EmitterList> eLList = new List<EmitterList>();

        //        for (int loop = 0; loop < blinkingCnt; loop++)
        //        {
        //            _timerrrr.Start();

        //            //----------------------------------------------------------------------------------
        //            // (1) EMsrtType.LOP ( isAutoTurnOff == false )
        //            //----------------------------------------------------------------------------------
        //            srcMeter.MeterOutput(this._srcSyncTrigger.ToArray(), srcMeterItemIndex);

        //            sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex);

        //            (item as NFPBlinkingTestItem).MsrtResult[(int)ENfpBlinkingMsrtType.BkgVolt].RawValue
        //                = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[0].Unit) * this._chipPolarity; // PDMVFLA

        //            srcMeter.TurnOff();

        //            if (!(item as NFPBlinkingTestItem).MsrtResult[(int)ENfpBlinkingMsrtType.BkgVolt].IsPass)
        //            {
        //                this._isBlinkingTest = false;

        //                break;
        //            }

        //            _autoRestEvent_Camera.WaitOne(1200);

        //            double spanTime2 = _timerrrr.PeekTimeSpan(ETimeSpanUnit.MilliSecond);

        //            if (spanTime2 > 1200)
        //            {
        //                Console.WriteLine("[HS_TesterKernel], NFP_Blinking(), Camera TimeOut");

        //                Console.WriteLine("[HS_TesterKernel], NFP_Blinking(), Camera TimeOut at loop = " + loop);

        //                srcMeter.MeterOutput(this._srcSyncTrigger.ToArray(), srcMeterItemIndex);

        //                sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex);

        //                (item as NFPBlinkingTestItem).MsrtResult[(int)ENfpBlinkingMsrtType.BkgVolt].RawValue
        //                    = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[0].Unit) * this._chipPolarity; // PDMVFLA

        //                srcMeter.TurnOff();
        //            }
        //            if (_camera != null)
        //            {
        //                if (this.NFresult != null)
        //                {
        //                    //(item as NFPTestItem).MsrtResult[(int)ENfpMsrtType.DeadDieCount].Value = this.NFresult.PatternCount;
        //                    eLList.Add(PAAR2EListConverter.Convert2(this.NFresult));
        //                }
        //            }

        //            //if (_sysSetting.FFPSetting.NFSysSetting.IsEnableSaveNFPImgPath &&
        //            //    IfSaveFFP_NFPData(this._acquireData.ChipInfo.TestCount, this._acquireData.ChipInfo.IsPass))
        //            //{
        //            //    string fileName = string.Format("{0}_NFPBlinking_X{1}Y{2}_{3}", _sysSetting.FFPSetting.NoExttenFileNameForImgage, this._acquireData.ChipInfo.ColX, this._acquireData.ChipInfo.RowY, (loop + 1));

        //            //    string subFolder = _sysSetting.FFPSetting.SaveDirectory;

        //            //    string path = Path.Combine(_sysSetting.FFPSetting.NFSysSetting.NFPImgSavePath, subFolder);

        //            //    AsyncBackupImage(_keyImgDic["NFP"], path, fileName, System.Drawing.Imaging.ImageFormat.Tiff);
        //            //}
        //            if (_sysSetting.FFPSetting.NFSysSetting.IsEnableSaveBlinkingImgPath &&
        //               IfSaveFFP_NFPData(this._acquireData.ChipInfo.TestCount,this._acquireData.ChipInfo.IsPass))
        //            {
        //                string path = string.Empty;
        //                string fileNameWithoutExt = string.Empty;

        //                GetBlinkingImgSavePath(loop + 1, out path, out fileNameWithoutExt);

        //                //非同步
        //                //string fileName = string.Format("{0}_NFPBlinking_X{1}Y{2}_{3}", _sysSetting.FFPSetting.NoExttenFileNameForImgage, this._acquireData.ChipInfo.ColX, this._acquireData.ChipInfo.RowY, (loop + 1));

        //                //string subFolder = _sysSetting.FFPSetting.SaveDirectory;

        //                //string path = Path.Combine(_sysSetting.FFPSetting.NFSysSetting.NFPImgSavePath, subFolder);

        //                //AsyncBackupImage(_keyImgDic["NFP"], path, fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
        //            }

        //            // add nfp data

        //        }

        //        if ((item as NFPBlinkingTestItem).MsrtResult[(int)ENfpBlinkingMsrtType.BkgVolt].IsPass)
        //        {
        //            //BlinkAnalysisResult BResult = new BlinkAnalysisResult(eLList, 9);
        //            BlinkAnalysisResult BResult = new BlinkAnalysisResult(eLList, this._sysSetting.FFPSetting.NfpArrayAnalysisParam.BlinkingMaxEmitterPitchDistance);
        //            (item as NFPBlinkingTestItem).MsrtResult[(int)ENfpBlinkingMsrtType.BkgDieCount].RawValue = this.NFresult.PatternCount;
        //            (item as NFPBlinkingTestItem).MsrtResult[(int)ENfpBlinkingMsrtType.BkgBadDieCount].RawValue = BResult.BadDieCount;
        //            (item as NFPBlinkingTestItem).MsrtResult[(int)ENfpBlinkingMsrtType.BkgDeadDieCount].RawValue = BResult.DeadDieCount;

        //            this._acquireData.BlinkingDataSet[item.KeyName].BResult = BResult.Clone() as BlinkAnalysisResult;//[ELIVOptiMsrtType.LIVMsrtI].DataArray[i] 
        //        }
        //    }

        //    return sourceMeterReadData;

        //}

        protected virtual void SyncDataToChannel(TestItemData item, uint srcChannel, int dataNum = 0)
        {
            uint dutChannel = this._acquireData.ChipInfo.Channel;
            if (this._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Die)
            {
                if (this._machineConfig.ChannelConfig.TesterSequenceType == ETesterSequenceType.Parallel)
                {
                    dutChannel = srcChannel;
                }

                this._condCtrl.Data.ChannelConditionTable.Channels[dutChannel][item.KeyName].MsrtResult[dataNum].RawValue = item.MsrtResult[dataNum].RawValue;
                this._condCtrl.Data.ChannelConditionTable.Channels[dutChannel][item.KeyName].MsrtResult[dataNum].IsTested = true;
            }
        }

        protected virtual void SyncDataToChannel(TestItemData item, uint srcChannel, int[] dataNumArr)
        {
            uint dutChannel = this._acquireData.ChipInfo.Channel;
            if (this._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Die)
            {
                if (this._machineConfig.ChannelConfig.TesterSequenceType == ETesterSequenceType.Parallel)
                {
                    dutChannel = srcChannel;
                }
                foreach (int num in dataNumArr)
                {
                    this._condCtrl.Data.ChannelConditionTable.Channels[dutChannel][item.KeyName].MsrtResult[num].RawValue = item.MsrtResult[num].RawValue;
                }
            }
        }


    }
}
