using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
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
using MPI.Tester.Device.LaserSourceSys;
using MPI.Tester.Maths;

using MPI.Tester.Device.PostCalc;

namespace MPI.Tester.TestKernel
{
    public class HS_TesterKernel : TesterKernelBase
    {
        private double _sequenceDelayTimne;


        public HS_TesterKernel()
            : base()
        {
            this._adjacentCheck = new AdjacentCheck();


            this._sequenceDelayTimne = 8.0d;

            this._srcSyncTrigger = new List<uint>();

            this._sysDelayTimer = new PerformanceTimer();


        }

        #region >>> Private Methods <<<

        private void GetOptiMsrtResult(uint index, TestItemData testItem)
        {
            if (this._sptMeter == null)
            {
                return;
            }

            if (!testItem.IsTested)
                return;

            if (testItem is LOPWLTestItem)
            {
                #region >>> LOPWL <<<

                if (this._sptMeter.CalculateParameters(index))
                {
                    testItem.MsrtResult[(int)EOptiMsrtType.LOP].RawValue = this._sptMeter.Data[index].Lx;
                    testItem.MsrtResult[(int)EOptiMsrtType.WATT].RawValue = this._sptMeter.Data[index].Watt;
                    testItem.MsrtResult[(int)EOptiMsrtType.LM].RawValue = this._sptMeter.Data[index].Lm;

                    testItem.MsrtResult[(int)EOptiMsrtType.WLP].RawValue = this._sptMeter.Data[index].WLP;
                    testItem.MsrtResult[(int)EOptiMsrtType.WLD].RawValue = this._sptMeter.Data[index].WLD;
                    testItem.MsrtResult[(int)EOptiMsrtType.WLC].RawValue = this._sptMeter.Data[index].WLCv;
                    testItem.MsrtResult[(int)EOptiMsrtType.HW].RawValue = this._sptMeter.Data[index].FWHM;

                    testItem.MsrtResult[(int)EOptiMsrtType.PURITY].RawValue = this._sptMeter.Data[index].Purity;
                    testItem.MsrtResult[(int)EOptiMsrtType.CIEx].RawValue = this._sptMeter.Data[index].CIE1931x;
                    testItem.MsrtResult[(int)EOptiMsrtType.CIEy].RawValue = this._sptMeter.Data[index].CIE1931y;
                    testItem.MsrtResult[(int)EOptiMsrtType.CIEz].RawValue = this._sptMeter.Data[index].CIE1931z;
                    testItem.MsrtResult[(int)EOptiMsrtType.CCT].RawValue = this._sptMeter.Data[index].CCT;

                    testItem.MsrtResult[(int)EOptiMsrtType.WLCP].RawValue = (double)this._sptMeter.Data[index].WLCp;

                    testItem.MsrtResult[(int)EOptiMsrtType.ST].RawValue = this._sptMeter.Data[index].IntegralTime;
                    testItem.MsrtResult[(int)EOptiMsrtType.INT].RawValue = (double)this._sptMeter.Data[index].MaxCount;
                    testItem.MsrtResult[(int)EOptiMsrtType.STR].RawValue = (double)this._sptMeter.Data[index].TriggerTime - (double)this._sptMeter.Data[index].IntegralTime;
                    testItem.MsrtResult[(int)EOptiMsrtType.INTP].RawValue = (double)this._sptMeter.Data[index].CountPercent;
                    testItem.MsrtResult[(int)EOptiMsrtType.INTSS].RawValue = (double)this._sptMeter.Data[index].WLPNIR;

                    testItem.MsrtResult[(int)EOptiMsrtType.DARKA].RawValue = (double)this._sptMeter.Data[index].DarkAvg;
                    testItem.MsrtResult[(int)EOptiMsrtType.DARKB].RawValue = (double)this._sptMeter.Data[index].DarkSTDev;
                    testItem.MsrtResult[(int)EOptiMsrtType.DARKC].RawValue = (double)this._sptMeter.Data[index].ChipDarkAvg;

                    testItem.MsrtResult[(int)EOptiMsrtType.CRI].RawValue = (double)this._sptMeter.Data[index].GeneralCRI;

                    testItem.MsrtResult[(int)EOptiMsrtType.Uprime].RawValue = (double)this._sptMeter.Data[index].u_prime;
                    testItem.MsrtResult[(int)EOptiMsrtType.Vprime].RawValue = (double)this._sptMeter.Data[index].v_prime;
                    testItem.MsrtResult[(int)EOptiMsrtType.Duv].RawValue = (double)this._sptMeter.Data[index].ColorDelta;

                    testItem.MsrtResult[(int)EOptiMsrtType.ConLOP].RawValue = this._sptMeter.Data[index].Watt * 10;


                    for (int k = 0; k < this._sptMeter.Data[index].SpecialCRI.Length; k++)
                    {
                        testItem.MsrtResult[(int)EOptiMsrtType.R01 + k].RawValue = this._sptMeter.Data[index].SpecialCRI[k];
                    }
                }
                else
                {
                    for (int i = 0; i < testItem.MsrtResult.Length; i++)
                    {
                        testItem.MsrtResult[i].RawValue = 0.0d;
                    }

                    testItem.MsrtResult[(int)EOptiMsrtType.ST].RawValue = this._sptMeter.Data[index].IntegralTime;
                    testItem.MsrtResult[(int)EOptiMsrtType.INT].RawValue = (double)this._sptMeter.Data[index].MaxCount;
                    testItem.MsrtResult[(int)EOptiMsrtType.STR].RawValue = (double)this._sptMeter.Data[index].Ratio;
                    testItem.MsrtResult[(int)EOptiMsrtType.INTP].RawValue = (double)this._sptMeter.Data[index].CountPercent;
                    testItem.MsrtResult[(int)EOptiMsrtType.INTSS].RawValue = (double)this._sptMeter.Data[index].TriggerTime;
                }

                for (int i = 0; i < testItem.MsrtResult.Length; i++)
                {
                    testItem.MsrtResult[i].Value = testItem.MsrtResult[i].RawValue;
                }


                if (testItem.MsrtResult != null && this._isStopTest)
                {
                    foreach (TestResultData data in testItem.MsrtResult)
                    {
                        if (data.KeyName.Contains("MVFL"))
                        {
                            continue;
                        }

                        // 如果啟動此功能，當有ItemSkip時，也不清楚光資料

                        if (this._sysSetting.IsJudgeFailKeepOpticsResult)
                        {

                        }
                        else
                        {
                            data.RawValue = 0.0d;
                            data.Value = 0.0d;
                        }
                    }


                    if ((testItem as LOPWLTestItem).OptiSetting.SensingMode == ESensingMode.Fixed)
                    {
                        testItem.MsrtResult[(int)EOptiMsrtType.ST].RawValue = (testItem as LOPWLTestItem).OptiSetting.FixIntegralTime;
                        testItem.MsrtResult[(int)EOptiMsrtType.ST].Value = (testItem as LOPWLTestItem).OptiSetting.FixIntegralTime;
                    }
                    else
                    {
                        testItem.MsrtResult[(int)EOptiMsrtType.ST].RawValue = (testItem as LOPWLTestItem).OptiSetting.LimitIntegralTime;
                        testItem.MsrtResult[(int)EOptiMsrtType.ST].Value = (testItem as LOPWLTestItem).OptiSetting.LimitIntegralTime;
                    }

                    testItem.MsrtResult[(int)EOptiMsrtType.INT].RawValue = MPI.Maths.Statistic.Average(this._sptMeter.DarkIntensityArray);

                    testItem.MsrtResult[(int)EOptiMsrtType.INT].Value = MPI.Maths.Statistic.Average(this._sptMeter.DarkIntensityArray);
                }

                #endregion
            }
            else if (testItem is VLRTestItem)
            {
                #region >>> VLR <<<

                if (this._sptMeter.CalculateParameters(index))
                {
                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRLOP].RawValue = this._sptMeter.Data[index].Lx;
                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRWATT].RawValue = this._sptMeter.Data[index].Watt;
                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRLM].RawValue = this._sptMeter.Data[index].Lm;

                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRWLP].RawValue = this._sptMeter.Data[index].WLP;
                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRWLD].RawValue = this._sptMeter.Data[index].WLD;
                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRWLC].RawValue = this._sptMeter.Data[index].WLCv;
                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRHW].RawValue = this._sptMeter.Data[index].FWHM;

                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRPURITY].RawValue = this._sptMeter.Data[index].Purity;
                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRCIEx].RawValue = this._sptMeter.Data[index].CIE1931x;
                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRCIEy].RawValue = this._sptMeter.Data[index].CIE1931y;
                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRCIEz].RawValue = this._sptMeter.Data[index].CIE1931z;
                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRCCT].RawValue = this._sptMeter.Data[index].CCT;

                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRWLCP].RawValue = (double)this._sptMeter.Data[index].WLCp;

                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRST].RawValue = this._sptMeter.Data[index].IntegralTime;
                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRINT].RawValue = (double)this._sptMeter.Data[index].MaxCount;
                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRSTR].RawValue = (double)this._sptMeter.Data[index].TriggerTime - (double)this._sptMeter.Data[index].IntegralTime;
                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRINTP].RawValue = (double)this._sptMeter.Data[index].CountPercent;
                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRINTSS].RawValue = (double)this._sptMeter.Data[index].WLPNIR;

                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRDARKA].RawValue = (double)this._sptMeter.Data[index].DarkAvg;
                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRDARKB].RawValue = (double)this._sptMeter.Data[index].DarkSTDev;
                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRDARKC].RawValue = (double)this._sptMeter.Data[index].ChipDarkAvg;

                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRCRI].RawValue = (double)this._sptMeter.Data[index].GeneralCRI;

                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRUprime].RawValue = (double)this._sptMeter.Data[index].u_prime;
                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRVprime].RawValue = (double)this._sptMeter.Data[index].v_prime;
                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRDuv].RawValue = (double)this._sptMeter.Data[index].ColorDelta;

                    for (int k = 0; k < this._sptMeter.Data[index].SpecialCRI.Length; k++)
                    {
                        testItem.MsrtResult[(int)EVLROptiMsrtType.VLRR01 + k].RawValue = this._sptMeter.Data[index].SpecialCRI[k];
                    }
                }
                else
                {
                    for (int i = 0; i < testItem.MsrtResult.Length; i++)
                    {
                        testItem.MsrtResult[i].RawValue = 0.0d;
                    }

                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRST].RawValue = this._sptMeter.Data[index].IntegralTime;
                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRINT].RawValue = (double)this._sptMeter.Data[index].MaxCount;
                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRSTR].RawValue = (double)this._sptMeter.Data[index].Ratio;
                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRINTP].RawValue = (double)this._sptMeter.Data[index].CountPercent;
                    testItem.MsrtResult[(int)EVLROptiMsrtType.VLRINTSS].RawValue = (double)this._sptMeter.Data[index].TriggerTime;
                }

                for (int i = 0; i < testItem.MsrtResult.Length; i++)
                {
                    testItem.MsrtResult[i].Value = testItem.MsrtResult[i].RawValue;
                }


                if (testItem.MsrtResult != null && this._isStopTest)
                {
                    foreach (TestResultData data in testItem.MsrtResult)
                    {
                        data.RawValue = 0.0d;
                        data.Value = 0.0d;
                    }
                }

                #endregion
            }
            else if (testItem is LIVTestItem)
            {
                #region >>> LIV <<<

                LIVData livTestItemData = this._acquireData.LIVDataSet[testItem.KeyName];

                for (int i = 0; i < (testItem as LIVTestItem).OptiSettingList.Count; i++)
                {
                    if (this._sptMeter.CalculateParameters(index + (uint)i) && !this._isStopTest)
                    {
                        livTestItemData[ELIVOptiMsrtType.LIVLOP].DataArray[i] = (float)this._sptMeter.Data[index + i].Lx;

                        livTestItemData[ELIVOptiMsrtType.LIVWATT].DataArray[i] = (float)(this._sptMeter.Data[index + i].Watt * UnitMath.UnitConvertFactor(EWattUnit.mW, testItem.MsrtResult[(int)ELIVOptiMsrtType.LIVPDWATT].Unit));

                        livTestItemData[ELIVOptiMsrtType.LIVLM].DataArray[i] = (float)this._sptMeter.Data[index + i].Lm;

                        livTestItemData[ELIVOptiMsrtType.LIVWLP].DataArray[i] = (float)this._sptMeter.Data[index + i].WLP;

                        livTestItemData[ELIVOptiMsrtType.LIVWLD].DataArray[i] = (float)this._sptMeter.Data[index + i].WLD;

                        livTestItemData[ELIVOptiMsrtType.LIVWLC].DataArray[i] = (float)this._sptMeter.Data[index + i].WLCv;

                        livTestItemData[ELIVOptiMsrtType.LIVHW].DataArray[i] = (float)this._sptMeter.Data[index + i].FWHM;

                        livTestItemData[ELIVOptiMsrtType.LIVPURITY].DataArray[i] = (float)this._sptMeter.Data[index + i].Purity;

                        livTestItemData[ELIVOptiMsrtType.LIVCIEx].DataArray[i] = (float)this._sptMeter.Data[index + i].CIE1931x;

                        livTestItemData[ELIVOptiMsrtType.LIVCIEy].DataArray[i] = (float)this._sptMeter.Data[index + i].CIE1931y;

                        livTestItemData[ELIVOptiMsrtType.LIVCIEz].DataArray[i] = (float)this._sptMeter.Data[index + i].CIE1931z;

                        livTestItemData[ELIVOptiMsrtType.LIVCCT].DataArray[i] = (float)this._sptMeter.Data[index + i].CCT;

                        livTestItemData[ELIVOptiMsrtType.LIVWLCP].DataArray[i] = (float)this._sptMeter.Data[index + i].WLCp;

                        livTestItemData[ELIVOptiMsrtType.LIVST].DataArray[i] = (float)this._sptMeter.Data[index + i].IntegralTime;

                        livTestItemData[ELIVOptiMsrtType.LIVINT].DataArray[i] = (float)this._sptMeter.Data[index + i].MaxCount;

                        livTestItemData[ELIVOptiMsrtType.LIVSTR].DataArray[i] = (float)this._sptMeter.Data[index + i].TriggerTime - (float)this._sptMeter.Data[index + i].IntegralTime;

                        livTestItemData[ELIVOptiMsrtType.LIVINTP].DataArray[i] = (float)this._sptMeter.Data[index + i].CountPercent;

                        livTestItemData[ELIVOptiMsrtType.LIVINTSS].DataArray[i] = (float)this._sptMeter.Data[index + i].WLPNIR;

                        livTestItemData[ELIVOptiMsrtType.LIVDARKA].DataArray[i] = (float)this._sptMeter.Data[index + i].DarkAvg;

                        livTestItemData[ELIVOptiMsrtType.LIVDARKB].DataArray[i] = (float)this._sptMeter.Data[index + i].DarkSTDev;

                        livTestItemData[ELIVOptiMsrtType.LIVDARKC].DataArray[i] = (float)this._sptMeter.Data[index + i].ChipDarkAvg;

                        livTestItemData[ELIVOptiMsrtType.LIVCRI].DataArray[i] = (float)this._sptMeter.Data[index + i].GeneralCRI;

                        livTestItemData[ELIVOptiMsrtType.LIVUprime].DataArray[i] = (float)this._sptMeter.Data[index + i].u_prime;

                        livTestItemData[ELIVOptiMsrtType.LIVVprime].DataArray[i] = (float)this._sptMeter.Data[index + i].v_prime;

                        livTestItemData[ELIVOptiMsrtType.LIVDuv].DataArray[i] = (float)this._sptMeter.Data[index + i].ColorDelta;

                        for (int j = 0; j < this._sptMeter.Data[index].SpecialCRI.Length; j++)
                        {
                            livTestItemData[ELIVOptiMsrtType.LIVR01].DataArray[i] = (float)this._sptMeter.Data[index].SpecialCRI[j];
                        }
                    }
                    else
                    {
                        for (int k = 0; k < testItem.MsrtResult.Length; k++)
                        {
                            testItem.MsrtResult[k].DataList.Add(0.0f);
                        }

                        livTestItemData[ELIVOptiMsrtType.LIVST].DataArray[i] = (float)this._sptMeter.Data[index].IntegralTime;

                        livTestItemData[ELIVOptiMsrtType.LIVINT].DataArray[i] = (float)this._sptMeter.Data[index].MaxCount;

                        livTestItemData[ELIVOptiMsrtType.LIVSTR].DataArray[i] = (float)this._sptMeter.Data[index].Ratio;

                        livTestItemData[ELIVOptiMsrtType.LIVINTP].DataArray[i] = (float)this._sptMeter.Data[index].CountPercent;

                        livTestItemData[ELIVOptiMsrtType.LIVINTSS].DataArray[i] = (float)this._sptMeter.Data[index].TriggerTime;
                    }

                    //===========================================================
                    // Calculate TimeA
                    //===========================================================
                    LIVTestItem item = (testItem as LIVTestItem);

                    double timeA = 0.0d;

                    double basicTime = item.LIVForceDelayTime + item.LIVForceTime + item.LIVTurnOffTime;

                    if (item.LIVSensingMode == ESensingMode.Limit && item.LIVIsLimitModeFixedSIT)
                    {
                        basicTime += item.LIVLimitIntegralTime;
                    }
                    else
                    {
                        basicTime += this._sptMeter.Data[index + i].IntegralTime;
                    }

                    if (item.LIVIsLifeTest)
                    {
                        if (item.LIVIsLogScale)
                        {
                            basicTime = basicTime > item.LIVLogScale[i] ? basicTime : item.LIVLogScale[i];

                            basicTime = i == 0 ? basicTime : basicTime - this._acquireData.LIVDataSet[testItem.KeyName][ELIVOptiMsrtType.LIVTIMEB].DataArray[i - 1];
                        }
                        else
                        {
                            basicTime = basicTime > item.LIVSamplimgTime ? basicTime : item.LIVSamplimgTime;
                        }
                    }

                    timeA = i == 0 ? basicTime : basicTime + this._acquireData.LIVDataSet[testItem.KeyName][ELIVOptiMsrtType.LIVTIMEA].DataArray[i - 1];

                    this._acquireData.LIVDataSet[testItem.KeyName][ELIVOptiMsrtType.LIVTIMEA].DataArray[i] = (float)timeA;
                }

                #endregion
            }
            else if (testItem is TransistorTestItem)
            {
                #region >>> Transistor <<<

                LIVData livTestItemData = this._acquireData.LIVDataSet[testItem.KeyName];

                for (int i = 0; i < (testItem as TransistorTestItem).OptiSettingList.Count; i++)
                {
                    if (this._sptMeter.CalculateParameters(index + (uint)i) && !this._isStopTest)
                    {
                        livTestItemData[ETransistorOptiMsrtType.TRLOP].DataArray[i] = (float)this._sptMeter.Data[index + i].Lx;

                        livTestItemData[ETransistorOptiMsrtType.TRWATT].DataArray[i] = (float)(this._sptMeter.Data[index + i].Watt * UnitMath.UnitConvertFactor(EWattUnit.mW, testItem.MsrtResult[(int)ETransistorOptiMsrtType.TRPDWATT].Unit));

                        livTestItemData[ETransistorOptiMsrtType.TRLM].DataArray[i] = (float)this._sptMeter.Data[index + i].Lm;

                        livTestItemData[ETransistorOptiMsrtType.TRWLP].DataArray[i] = (float)this._sptMeter.Data[index + i].WLP;

                        livTestItemData[ETransistorOptiMsrtType.TRWLD].DataArray[i] = (float)this._sptMeter.Data[index + i].WLD;

                        livTestItemData[ETransistorOptiMsrtType.TRWLC].DataArray[i] = (float)this._sptMeter.Data[index + i].WLCv;

                        livTestItemData[ETransistorOptiMsrtType.TRHW].DataArray[i] = (float)this._sptMeter.Data[index + i].FWHM;

                        livTestItemData[ETransistorOptiMsrtType.TRPURITY].DataArray[i] = (float)this._sptMeter.Data[index + i].Purity;

                        livTestItemData[ETransistorOptiMsrtType.TRCIEx].DataArray[i] = (float)this._sptMeter.Data[index + i].CIE1931x;

                        livTestItemData[ETransistorOptiMsrtType.TRCIEy].DataArray[i] = (float)this._sptMeter.Data[index + i].CIE1931y;

                        livTestItemData[ETransistorOptiMsrtType.TRCIEz].DataArray[i] = (float)this._sptMeter.Data[index + i].CIE1931z;

                        livTestItemData[ETransistorOptiMsrtType.TRCCT].DataArray[i] = (float)this._sptMeter.Data[index + i].CCT;

                        livTestItemData[ETransistorOptiMsrtType.TRWLCP].DataArray[i] = (float)this._sptMeter.Data[index + i].WLCp;

                        livTestItemData[ETransistorOptiMsrtType.TRST].DataArray[i] = (float)this._sptMeter.Data[index + i].IntegralTime;

                        livTestItemData[ETransistorOptiMsrtType.TRINT].DataArray[i] = (float)this._sptMeter.Data[index + i].MaxCount;

                        livTestItemData[ETransistorOptiMsrtType.TRSTR].DataArray[i] = (float)this._sptMeter.Data[index + i].TriggerTime - (float)this._sptMeter.Data[index + i].IntegralTime;

                        livTestItemData[ETransistorOptiMsrtType.TRINTP].DataArray[i] = (float)this._sptMeter.Data[index + i].CountPercent;

                        livTestItemData[ETransistorOptiMsrtType.TRINTSS].DataArray[i] = (float)this._sptMeter.Data[index + i].WLPNIR;

                        livTestItemData[ETransistorOptiMsrtType.TRDARKA].DataArray[i] = (float)this._sptMeter.Data[index + i].DarkAvg;

                        livTestItemData[ETransistorOptiMsrtType.TRDARKB].DataArray[i] = (float)this._sptMeter.Data[index + i].DarkSTDev;

                        livTestItemData[ETransistorOptiMsrtType.TRDARKC].DataArray[i] = (float)this._sptMeter.Data[index + i].ChipDarkAvg;

                        livTestItemData[ETransistorOptiMsrtType.TRCRI].DataArray[i] = (float)this._sptMeter.Data[index + i].GeneralCRI;

                        livTestItemData[ETransistorOptiMsrtType.TRUprime].DataArray[i] = (float)this._sptMeter.Data[index + i].u_prime;

                        livTestItemData[ETransistorOptiMsrtType.TRVprime].DataArray[i] = (float)this._sptMeter.Data[index + i].v_prime;

                        livTestItemData[ETransistorOptiMsrtType.TRDuv].DataArray[i] = (float)this._sptMeter.Data[index + i].ColorDelta;

                        for (int j = 0; j < this._sptMeter.Data[index].SpecialCRI.Length; j++)
                        {
                            livTestItemData[ETransistorOptiMsrtType.TRR01].DataArray[i] = (float)this._sptMeter.Data[index].SpecialCRI[j];
                        }
                    }
                    else
                    {
                        for (int k = 0; k < testItem.MsrtResult.Length; k++)
                        {
                            testItem.MsrtResult[k].DataList.Add(0.0f);
                        }

                        livTestItemData[ETransistorOptiMsrtType.TRST].DataArray[i] = (float)this._sptMeter.Data[index].IntegralTime;

                        livTestItemData[ETransistorOptiMsrtType.TRINT].DataArray[i] = (float)this._sptMeter.Data[index].MaxCount;

                        livTestItemData[ETransistorOptiMsrtType.TRSTR].DataArray[i] = (float)this._sptMeter.Data[index].Ratio;

                        livTestItemData[ETransistorOptiMsrtType.TRINTP].DataArray[i] = (float)this._sptMeter.Data[index].CountPercent;

                        livTestItemData[ETransistorOptiMsrtType.TRINTSS].DataArray[i] = (float)this._sptMeter.Data[index].TriggerTime;
                    }
                }

                #endregion
            }
        }

        private void ChangeRowColCoord()
        {

            if (P2TcoordTransTool != null)
            {
                int X = (int)this._cmdData.DoubleData[(uint)EProberDataIndex.COL];
                int Y = (int)this._cmdData.DoubleData[(uint)EProberDataIndex.ROW];

                Matrix mat = P2TcoordTransTool.TransCoord(X, Y);

                this._cmdData.DoubleData[(uint)EProberDataIndex.COL] = mat[0, 0];
                this._cmdData.DoubleData[(uint)EProberDataIndex.ROW] = mat[1, 0];
            }
            else
            {
                double deltaColX = this._cmdData.DoubleData[(uint)EProberDataIndex.COL] - this._cmdData.DoubleData[(uint)EProberDataIndex.TransCOL];
                double deltaRowY = this._cmdData.DoubleData[(uint)EProberDataIndex.ROW] - this._cmdData.DoubleData[(uint)EProberDataIndex.TransROW];

                this._acquireData.ChipInfo.TransCol = (int)this._cmdData.DoubleData[(uint)EProberDataIndex.TransCOL];
                this._acquireData.ChipInfo.TransRow = (int)this._cmdData.DoubleData[(uint)EProberDataIndex.TransROW];

                switch (this._sysSetting.ProberCoord)
                {
                    case (int)ECoordSet.Second:
                        deltaColX *= (-1);
                        break;
                    case (int)ECoordSet.Third:
                        deltaColX *= (-1);
                        deltaRowY *= (-1);
                        break;
                    case (int)ECoordSet.Fourth:
                        deltaRowY *= (-1);
                        break;
                    default:
                        break;
                }

                switch (this._sysSetting.TesterCoord)
                {
                    case (int)ECoordSet.Second:
                        deltaColX *= (-1);
                        break;
                    case (int)ECoordSet.Third:
                        deltaColX *= (-1);
                        deltaRowY *= (-1);
                        break;
                    case (int)ECoordSet.Fourth:
                        deltaRowY *= (-1);
                        break;
                    default:
                        break;
                }


                this._cmdData.DoubleData[(uint)EProberDataIndex.COL] = deltaColX + this._cmdData.DoubleData[(uint)EProberDataIndex.TransCOL];
                this._cmdData.DoubleData[(uint)EProberDataIndex.ROW] = deltaRowY + this._cmdData.DoubleData[(uint)EProberDataIndex.TransROW];
            }


            string[] strProberKeyName = Enum.GetNames(typeof(EProberDataIndex));

            foreach (TestResultData data in this._acquireData.OutputTestResult)
            {
                for (uint p = 0; p < strProberKeyName.Length; p++)
                {
                    if (strProberKeyName[p] == data.KeyName)
                    {
                        data.Value = this._cmdData.DoubleData[p];

                        if (strProberKeyName[p] == EProberDataIndex.COL.ToString())
                        {
                            this._acquireData.ChipInfo.ColX = (int)data.Value;
                        }
                        else if (strProberKeyName[p] == EProberDataIndex.ROW.ToString())
                        {
                            this._acquireData.ChipInfo.RowY = (int)data.Value;
                        }
                        break;
                    }
                }
            }

        }

        private void TestProcess()
        {
            this._isStopTest = false;
            bool isStopTest = this._isStopTest;
            bool isJudgeFailSkipESDItem = false;
            uint testItemIndex = 0;
            TestItemData item = null;
            uint srcMeterItemIndex = 0;
            uint sptMeterItemIndex = 0;
            uint osaItemIndex = 0;
            uint lcrMeterItemIndex = 0;
            int esdItemIndex = 0;

            double[] sourceMeterReadData = null;
            double[][] chartData = new double[4][];

            this._preTypeState = -1;

            bool isContactCheckUpperBoundary = false;

            uint srcChannel = 0;

            uint dutChannel = 0;

            bool isOpenShortCheckFail = false;

            bool isOnlySkipReverseCurrent = this._sysSetting.IsOnlySkipIZItem;
            
            

            if (this._condCtrl.Data.TestItemArray == null || this._condCtrl.Data.TestItemArray.Length == 0)
                return;
            SetOpticalSwitchToDefault();

            #region >>set test result as 0<<
            for (testItemIndex = 0; testItemIndex < this._condCtrl.Data.TestItemArray.Length; testItemIndex++)
            {
                item = this._condCtrl.Data.TestItemArray[testItemIndex];

                item.IsTested = false;

                if (item.MsrtResult == null)
                {
                    continue;
                }

                foreach (var result in item.MsrtResult)
                {

                    result.ResetResult();
                    //result.IsTested = false;

                    //result.RawValue = 0.0d;

                    //result.Value = 0.0d;
                }
            }
            #endregion

            #region <<< Open/Short Check >>>

            if (this._sysSetting.contactCheckCFG._isEnableContactCheck)
            {
                this._srcMeter.MeterOutput(this._srcSyncTrigger.ToArray(), 0);

                sourceMeterReadData = this._srcMeter.GetDataFromMeter(srcChannel, 0);

                double volt = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EVoltUnit.V, "V") * this._chipPolarity;

                if (volt > this._sysSetting.contactCheckCFG._contactSpecMax)
                {
                    isOpenShortCheckFail = true;
                    isContactCheckUpperBoundary = true;
                }

                if (volt < this._sysSetting.contactCheckCFG._contactSpecMin)
                {
                    isOpenShortCheckFail = true;
                }

                if (this._sysSetting.contactCheckCFG._isDisableCheckAtPosX)
                {
                    if (this._cmdData.DoubleData[(uint)EProberDataIndex.COL] == 0.0d)
                    {
                        isOpenShortCheckFail = false;
                    }
                }

                isStopTest = isOpenShortCheckFail;

                if (_isManualTest == true)
                {
                    isStopTest = false;
                }

                srcMeterItemIndex++;
            }

            if (isOnlySkipReverseCurrent)
            {
                isStopTest = false;
            }

            #endregion

            #region >>> InterLock Check <<<


            //if (this._switchDevice != null)
            //{
            //    if (!_sysSetting.IsDisableInterlockCheck)
            //    {
            //        if (!this._switchDevice.CheckInterLock())
            //        {
            //            Console.WriteLine("[HS_TestKernel], TestProcess Interlock Check Error");

            //            this.SetErrorCode(EErrorCode.SourceOutput_Interlock_Err);

            //            return;
            //        }
            //    }
            //}
            //else
            //{
            //if (this._srcMeter != null)
            //{
            //        if (!_sysSetting.IsDisableInterlockCheck)
            //        {
            //if (!this._srcMeter.CheckInterLock())
            //{
            //                Console.WriteLine("[HS_TestKernel], TestProcess Interlock Check Error");

            //                this.SetErrorCode(EErrorCode.SourceOutput_Interlock_Err);

            //    return;
            //}
            //}
            //    }
            //}


            //if (this._srcMeter != null)
            //{
            //    if (this._srcMeter is SS400)
            //    {
            //        (this._srcMeter as SS400).ResetContactStatus();
            //    }
            //}

            #endregion

            #region  >>check io state<<

            MPI.PerformanceTimer pt = new PerformanceTimer();

            //pt.Start();
            
            CheckIOState(0);//目前測試的結果，花費時間約為 1.5 -3ms

            //string str = pt.PeekTimeSpan(ETimeSpanUnit.MilliSecond).ToString();

            //Console.WriteLine(str);
            //pt.Stop();
            #endregion

            for (testItemIndex = 0; testItemIndex < this._condCtrl.Data.TestItemArray.Length; testItemIndex++)
            {
                item = this._condCtrl.Data.TestItemArray[testItemIndex];

                if (item.Type != ETestType.IO)//如果在兩道IO中間skip，會造成IO驅動對象無法在EOT前回到正常狀態
                //如果IO啟動，中間的Index++都會被跳過，根本是錯的
                {
                    if (isStopTest == true && !this._product.IsJudgeFailSkipESDItem)
                    {
                        #region
                        switch (item.Type)
                        {
                            default:
                                if (item.ElecSetting != null)//calc
                                {
                                    srcMeterItemIndex += (uint)item.ElecSetting.Length;
                                }
                                break;                           
                            //--------------------------------------------------------------------------------------------------------------
                            case ETestType.LOPWL:
                                srcMeterItemIndex += (uint)item.ElecSetting.Length;
                                sptMeterItemIndex++;
                                break;
                            //--------------------------------------------------------------------------------------------------------------
                            case ETestType.ESD:
                                srcMeterItemIndex += (uint)item.ElecSetting.Length;
                                esdItemIndex++;
                                break;
                            //--------------------------------------------------------------------------------------------------------------                       
                            case ETestType.VLR:
                                srcMeterItemIndex++;
                                sptMeterItemIndex++;
                                break;
                            //--------------------------------------------------------------------------------------------------------------
                            case ETestType.LIV:
                                srcMeterItemIndex += (uint)item.ElecSetting.Length;
                                sptMeterItemIndex += (uint)(item as LIVTestItem).OptiSettingList.Count;
                                break;                      
                            //--------------------------------------------------------------------------------------------------------------                    
                            case ETestType.LCR:
                                if (this._lcrBias != null)
                                {
                                    srcMeterItemIndex++;
                                }
                                lcrMeterItemIndex++;

                                break;
                            //--------------------------------------------------------------------------------------------------------------
                            case ETestType.LCRSWEEP:
                                lcrMeterItemIndex++;

                                break;
                            //--------------------------------------------------------------------------------------------------------------
                            case ETestType.OSA:
                                srcMeterItemIndex += (uint)item.ElecSetting.Length;
                                osaItemIndex++;

                                break;
                        }
                        #endregion
                        continue;
                       
                    }
                }

                if (isStopTest && item is ESDTestItem)
                {
                    srcMeterItemIndex += (uint)item.ElecSetting.Length;
                    continue;
                }


                #region <<< Modify is Tested >>>

                item.IsTested = item.IsEnable;

                if (this._condCtrl.Data.TestItemArray[testItemIndex].MsrtResult != null)
                {
                    foreach (var result in this._condCtrl.Data.TestItemArray[testItemIndex].MsrtResult)
                    {
                        result.IsTested = item.IsEnable;
                    }
                }

                #endregion

                #region <<< Tester Group >>>

                //==================================================
                // Tester Group
                //==================================================
                if (this.IsGroupSkip(item))
                {
                    if (item is LOPWLTestItem || item is VLRTestItem)
                    {
                        srcMeterItemIndex += (uint)item.ElecSetting.Length;

                        sptMeterItemIndex++;
                    }
                    else if (item is LIVTestItem)
                    {
                        srcMeterItemIndex += (uint)item.ElecSetting.Length;

                        sptMeterItemIndex += (uint)(item as LIVTestItem).OptiSettingList.Count;
                    }
                    else if (item is TransistorTestItem)
                    {
                        srcMeterItemIndex += (uint)item.ElecSetting.Length;

                        sptMeterItemIndex += (uint)(item as TransistorTestItem).OptiSettingList.Count;
                    }
                    else if (item is ESDTestItem)
                    {
                        esdItemIndex++;

                        srcMeterItemIndex += (uint)item.ElecSetting.Length;
                    }
                    else
                    {
                        if (item.ElecSetting != null)
                        {
                            srcMeterItemIndex += (uint)item.ElecSetting.Length;
                        }
                    }

                    if (this._condCtrl.Data.TestItemArray[testItemIndex].MsrtResult != null)
                    {
                        foreach (var result in this._condCtrl.Data.TestItemArray[testItemIndex].MsrtResult)
                        {
                            result.IsTested = false;
                        }
                    }

                    continue;
                }

                #endregion

                #region >>> Switch Channel <<<

                // Switch Box DUT-CH 開啟
                if (item.IsEnable && this._switchDevice != null)
                {
                    this._switchDevice.EnableCH(item.SwitchingChannel);
                }

                #endregion
                
                switch (item.Type)
                {
                    case ETestType.POLAR:
                        sourceMeterReadData = POLAR(item, srcMeterItemIndex);
                        srcMeterItemIndex += (uint)item.ElecSetting.Length;
                        break;
                    //--------------------------------------------------------------------------------------------------------------
                    case ETestType.IFH:
                        sourceMeterReadData = IFH(item, srcMeterItemIndex);
                        srcMeterItemIndex += (uint)item.ElecSetting.Length;
                        break;
                    //--------------------------------------------------------------------------------------------------------------
                    case ETestType.IF:
                        sourceMeterReadData = IF(item, srcMeterItemIndex);
                        srcMeterItemIndex += (uint)item.ElecSetting.Length;
                        break;
                    //--------------------------------------------------------------------------------------------------------------
                    case ETestType.IZ:
                        sourceMeterReadData = IZ(item, srcMeterItemIndex);
                        srcMeterItemIndex += (uint)item.ElecSetting.Length;
                        break;
                    //--------------------------------------------------------------------------------------------------------------
                    case ETestType.VF:
                        sourceMeterReadData = VF(item, srcMeterItemIndex);
                        srcMeterItemIndex += (uint)item.ElecSetting.Length;
                        break;
                    //--------------------------------------------------------------------------------------------------------------
                    case ETestType.VR:
                        sourceMeterReadData = VR(item, srcMeterItemIndex);
                        srcMeterItemIndex += (uint)item.ElecSetting.Length;
                        break;
                    //--------------------------------------------------------------------------------------------------------------
                    case ETestType.IVSWEEP:
                        sourceMeterReadData = IVSweep(item, srcMeterItemIndex);
                        srcMeterItemIndex += (uint)item.ElecSetting.Length;
                        break;
                    //--------------------------------------------------------------------------------------------------------------
                    case ETestType.VISWEEP:
                        sourceMeterReadData = VISweep(item, srcMeterItemIndex);
                        srcMeterItemIndex += (uint)item.ElecSetting.Length;
                        break;
                    //--------------------------------------------------------------------------------------------------------------
                    case ETestType.THY:
                        sourceMeterReadData = THY(item, srcMeterItemIndex);
                        srcMeterItemIndex += (uint)item.ElecSetting.Length;
                        break;
                    //--------------------------------------------------------------------------------------------------------------
                    case ETestType.DVF:
                        sourceMeterReadData = DVF(item, srcMeterItemIndex);
                        srcMeterItemIndex += (uint)item.ElecSetting.Length;
                        break;
                    //--------------------------------------------------------------------------------------------------------------
                    case ETestType.LOP:
                        sourceMeterReadData = LOP(item, srcMeterItemIndex);
                        srcMeterItemIndex += (uint)item.ElecSetting.Length;

                        break;
                    //--------------------------------------------------------------------------------------------------------------
                    case ETestType.LOPWL:

                        sourceMeterReadData =LOPWL(item, srcMeterItemIndex,sptMeterItemIndex);
                        srcMeterItemIndex += (uint)item.ElecSetting.Length;
                        sptMeterItemIndex++;

                        break;
                    //--------------------------------------------------------------------------------------------------------------
                    case ETestType.ESD:

                        sourceMeterReadData = ESD(item, srcMeterItemIndex, esdItemIndex);
                        srcMeterItemIndex += (uint)item.ElecSetting.Length;
                        esdItemIndex++;
                        if((item as ESDTestItem).IsEnableJudgeItem)
                            isJudgeFailSkipESDItem = !item.MsrtResult[0].IsPass;

                        break;
                    //--------------------------------------------------------------------------------------------------------------
                    case ETestType.DIB:
                        sourceMeterReadData = DIB(item, srcMeterItemIndex);
                        srcMeterItemIndex += (uint)item.ElecSetting.Length;
                        break;
                    //--------------------------------------------------------------------------------------------------------------
                    case ETestType.VAC:
                        sourceMeterReadData = VAC(item, srcMeterItemIndex, srcChannel);
                        srcMeterItemIndex += (uint)item.ElecSetting.Length;
                        break;
                    //--------------------------------------------------------------------------------------------------------------
                    case ETestType.R:
                        sourceMeterReadData = R(item, srcMeterItemIndex);
                        srcMeterItemIndex += (uint)item.ElecSetting.Length;
                        break;
                    //--------------------------------------------------------------------------------------------------------------
                    case ETestType.ContactCheck:
                        sourceMeterReadData = ContactCheck(item, srcMeterItemIndex);
                        srcMeterItemIndex += (uint)item.ElecSetting.Length;

                        break;
                    //--------------------------------------------------------------------------------------------------------------
                    case ETestType.RTH:
                        sourceMeterReadData = RTH(item, srcMeterItemIndex);
                        srcMeterItemIndex += (uint)item.ElecSetting.Length;

                        break;
                    //--------------------------------------------------------------------------------------------------------------
                    case ETestType.VLR:
                        sourceMeterReadData = VLR(item, srcMeterItemIndex, sptMeterItemIndex, chartData, srcChannel);
                        srcMeterItemIndex++;
                        sptMeterItemIndex++;

                        break;
                    //--------------------------------------------------------------------------------------------------------------
                    case ETestType.LIV:

                        sourceMeterReadData = LIV(item, srcMeterItemIndex, sptMeterItemIndex);

                        srcMeterItemIndex += (uint)item.ElecSetting.Length;

                        sptMeterItemIndex += (uint)(item as LIVTestItem).OptiSettingList.Count;
                        break;
                    //--------------------------------------------------------------------------------------------------------------
                    case ETestType.VISCAN:
                        sourceMeterReadData = VIScan(item, srcMeterItemIndex);
                        srcMeterItemIndex += (uint)item.ElecSetting.Length;
                        break;

                    //--------------------------------------------------------------------------------------------------------------
                    case ETestType.PIV:
                        sourceMeterReadData = PIV(item, srcMeterItemIndex);
                        srcMeterItemIndex += (uint)item.ElecSetting.Length;
                        break;
                    //--------------------------------------------------------------------------------------------------------------
                    case ETestType.TRANSISTOR://先不動，必要的時候另外開一個Kernel來處理

                        #region >>> TRANSISTOR <<<

                        TransistorTestItem trItem = item as TransistorTestItem;

                        if (trItem.IsEnable)
                        {
                            trItem.ClearDataList();

                            if (this._srcMeter != null)
                            {
                                System.Threading.Thread.Sleep((int)trItem.TRProcessDelayTime);
                            }

                            this._acquireData.LIVDataSet[item.KeyName].Type = trItem.TRMsrtType.ToString();

                            trItem.TRProcessStart();

                            for (uint i = 0; i < item.ElecSetting.Length; i++)
                            {
                                //================================================================
                                //  [1] Source Meter force the current and measurement volt.
                                //================================================================
                                if (this._srcMeter != null)
                                {
                                    this._srcMeter.MeterOutput(this._srcSyncTrigger.ToArray(), srcMeterItemIndex + i);

                                    sourceMeterReadData = this._srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex + i);

                                    // Get PD Msrt Data
                                    if (this._machineConfig.PDSensingMode != EPDSensingMode.NONE && trItem.TRIsEnableDetector)
                                    {
                                        this._acquireData.LIVDataSet[item.KeyName][ETransistorOptiMsrtType.TRPDCURRENT].DataArray[i] = (float)(sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EAmpUnit.A, item.MsrtResult[(int)ETransistorOptiMsrtType.TRPDCURRENT].Unit));

                                        this._acquireData.LIVDataSet[item.KeyName][ETransistorOptiMsrtType.TRPDWATT].DataArray[i] = (float)(sourceMeterReadData[0] * _product.PdDetectorFactor * UnitMath.UnitConvertFactor(EWattUnit.mW, item.MsrtResult[(int)ETransistorOptiMsrtType.TRPDWATT].Unit));
                                    }

                                    // Get SMU 0~3 Msrt Data
                                    ElecTerminalSetting[] trSetting = trItem.ElecSetting[i].ElecTerminalSetting;

                                    if (trSetting[0].MsrtType == EMsrtType.FIMV || trSetting[0].MsrtType == EMsrtType.FIMVSWEEP)
                                    {
                                        this._acquireData.LIVDataSet[trItem.KeyName][ETransistorOptiMsrtType.TRMsrtDrainI].DataArray[i] = (float)(trSetting[0].ForceValue * UnitMath.UnitConvertFactor(EAmpUnit.mA, trItem.MsrtResult[(int)ETransistorOptiMsrtType.TRMsrtDrainI].Unit));

                                        this._acquireData.LIVDataSet[trItem.KeyName][ETransistorOptiMsrtType.TRMsrtDrainV].DataArray[i] = (float)(sourceMeterReadData[1] * UnitMath.UnitConvertFactor(EVoltUnit.V, trItem.MsrtResult[(int)ETransistorOptiMsrtType.TRMsrtDrainV].Unit) * this._chipPolarity);
                                    }
                                    else
                                    {
                                        this._acquireData.LIVDataSet[trItem.KeyName][ETransistorOptiMsrtType.TRMsrtDrainV].DataArray[i] = (float)(trSetting[0].ForceValue * UnitMath.UnitConvertFactor(EVoltUnit.V, trItem.MsrtResult[(int)ETransistorOptiMsrtType.TRMsrtDrainV].Unit));

                                        this._acquireData.LIVDataSet[trItem.KeyName][ETransistorOptiMsrtType.TRMsrtDrainI].DataArray[i] = (float)(sourceMeterReadData[1] * UnitMath.UnitConvertFactor(EAmpUnit.A, trItem.MsrtResult[(int)ETransistorOptiMsrtType.TRMsrtDrainI].Unit) * this._chipPolarity);
                                    }

                                    if (trSetting[1].MsrtType == EMsrtType.FIMV || trSetting[1].MsrtType == EMsrtType.FIMVSWEEP)
                                    {
                                        this._acquireData.LIVDataSet[trItem.KeyName][ETransistorOptiMsrtType.TRMsrtSourceI].DataArray[i] = (float)(trSetting[1].ForceValue * UnitMath.UnitConvertFactor(EAmpUnit.mA, trItem.MsrtResult[(int)ETransistorOptiMsrtType.TRMsrtSourceI].Unit));

                                        this._acquireData.LIVDataSet[trItem.KeyName][ETransistorOptiMsrtType.TRMsrtSourceV].DataArray[i] = (float)(sourceMeterReadData[2] * UnitMath.UnitConvertFactor(EVoltUnit.V, trItem.MsrtResult[(int)ETransistorOptiMsrtType.TRMsrtSourceV].Unit) * this._chipPolarity);
                                    }
                                    else
                                    {
                                        this._acquireData.LIVDataSet[trItem.KeyName][ETransistorOptiMsrtType.TRMsrtSourceV].DataArray[i] = (float)(trSetting[1].ForceValue * UnitMath.UnitConvertFactor(EVoltUnit.V, trItem.MsrtResult[(int)ETransistorOptiMsrtType.TRMsrtSourceV].Unit));

                                        this._acquireData.LIVDataSet[trItem.KeyName][ETransistorOptiMsrtType.TRMsrtSourceI].DataArray[i] = (float)(sourceMeterReadData[2] * UnitMath.UnitConvertFactor(EAmpUnit.A, trItem.MsrtResult[(int)ETransistorOptiMsrtType.TRMsrtSourceI].Unit) * this._chipPolarity);
                                    }

                                    if (trSetting[2].MsrtType == EMsrtType.FIMV || trSetting[2].MsrtType == EMsrtType.FIMVSWEEP)
                                    {
                                        this._acquireData.LIVDataSet[trItem.KeyName][ETransistorOptiMsrtType.TRMsrtGateI].DataArray[i] = (float)(trSetting[2].ForceValue * UnitMath.UnitConvertFactor(EAmpUnit.mA, trItem.MsrtResult[(int)ETransistorOptiMsrtType.TRMsrtGateI].Unit));

                                        this._acquireData.LIVDataSet[trItem.KeyName][ETransistorOptiMsrtType.TRMsrtGateV].DataArray[i] = (float)(sourceMeterReadData[3] * UnitMath.UnitConvertFactor(EVoltUnit.V, trItem.MsrtResult[(int)ETransistorOptiMsrtType.TRMsrtGateV].Unit) * this._chipPolarity);
                                    }
                                    else
                                    {
                                        this._acquireData.LIVDataSet[trItem.KeyName][ETransistorOptiMsrtType.TRMsrtGateV].DataArray[i] = (float)(trSetting[2].ForceValue * UnitMath.UnitConvertFactor(EVoltUnit.V, trItem.MsrtResult[(int)ETransistorOptiMsrtType.TRMsrtGateV].Unit));

                                        this._acquireData.LIVDataSet[trItem.KeyName][ETransistorOptiMsrtType.TRMsrtGateI].DataArray[i] = (float)(sourceMeterReadData[3] * UnitMath.UnitConvertFactor(EAmpUnit.A, trItem.MsrtResult[(int)ETransistorOptiMsrtType.TRMsrtGateI].Unit) * this._chipPolarity);
                                    }

                                    if (trSetting[3].MsrtType == EMsrtType.FIMV || trSetting[3].MsrtType == EMsrtType.FIMVSWEEP)
                                    {
                                        this._acquireData.LIVDataSet[trItem.KeyName][ETransistorOptiMsrtType.TRMsrtBlukI].DataArray[i] = (float)(trSetting[3].ForceValue * UnitMath.UnitConvertFactor(EAmpUnit.mA, trItem.MsrtResult[(int)ETransistorOptiMsrtType.TRMsrtBlukI].Unit));

                                        this._acquireData.LIVDataSet[trItem.KeyName][ETransistorOptiMsrtType.TRMsrtBlukV].DataArray[i] = (float)(sourceMeterReadData[4] * UnitMath.UnitConvertFactor(EVoltUnit.V, trItem.MsrtResult[(int)ETransistorOptiMsrtType.TRMsrtBlukV].Unit) * this._chipPolarity);
                                    }
                                    else
                                    {
                                        this._acquireData.LIVDataSet[trItem.KeyName][ETransistorOptiMsrtType.TRMsrtBlukV].DataArray[i] = (float)(trSetting[3].ForceValue * UnitMath.UnitConvertFactor(EVoltUnit.V, trItem.MsrtResult[(int)ETransistorOptiMsrtType.TRMsrtBlukV].Unit));

                                        this._acquireData.LIVDataSet[trItem.KeyName][ETransistorOptiMsrtType.TRMsrtBlukI].DataArray[i] = (float)(sourceMeterReadData[4] * UnitMath.UnitConvertFactor(EAmpUnit.A, trItem.MsrtResult[(int)ETransistorOptiMsrtType.TRMsrtBlukI].Unit) * this._chipPolarity);
                                    }

                                }

                                //================================================================
                                // [2] Trigger the Spectrometer to get spectrum
                                //================================================================
                                if (this._sptMeter != null && trItem.TRIsTestOptical == true)
                                {
                                    int rtnCode = this._sptMeter.Trigger(sptMeterItemIndex + i);

                                    if (rtnCode > 0)
                                    {
                                        this._acquireData.LIVDataSet[item.KeyName].SpectrumDataData[(int)i].Wavelength = this._sptMeter.GetXWavelength();

                                        this._acquireData.LIVDataSet[item.KeyName].SpectrumDataData[(int)i].Dark = this._sptMeter.DarkIntensityArray;

                                        this._acquireData.LIVDataSet[item.KeyName].SpectrumDataData[(int)i].Intensity = this._sptMeter.GetYSpectrumIntensity(sptMeterItemIndex + i);

                                        this._acquireData.LIVDataSet[item.KeyName].SpectrumDataData[(int)i].Absoluate = this._sptMeter.GetYAbsoluateSpectrum(sptMeterItemIndex + i);
                                    }
                                    else
                                    {
                                        if (rtnCode == -23)
                                        {
                                            // Re Test
                                            this._sptMeter.Trigger(sptMeterItemIndex + i);
                                        }
                                    }
                                }

                                //================================================================
                                // [3] Turn Off Time
                                //================================================================
                                List<uint> turnOffCH = new List<uint>();

                                for (int j = 0; j < trItem.ElecSetting[i].ElecTerminalSetting.Length; j++)
                                {
                                    if (trItem.ElecSetting[i].ElecTerminalSetting[j].TermianlFuncType == ETermianlFuncType.Sweep)
                                    {
                                        turnOffCH.Add((uint)trItem.ElecSetting[i].ElecTerminalSetting[j].SMU);
                                    }
                                }

                                if (this._srcMeter.ElecSetting != null && ((int)this._srcMeter.ElecSetting[srcMeterItemIndex + i].TurnOffTime) != 0)
                                {
                                    (this._srcMeter as Keithley2600).TurnOffByChannel(this._srcMeter.ElecSetting[srcMeterItemIndex + i].TurnOffTime, turnOffCH.ToArray());
                                }

                                this._acquireData.LIVDataSet[item.KeyName][ETransistorOptiMsrtType.TRTIME].DataArray[i] = (float)trItem.TRProcessTime();
                            }

                            trItem.TRProcessEnd();

                            if (this._srcMeter != null)
                            {
                                this._srcMeter.TurnOff();
                            }

                        }

                        srcMeterItemIndex += (uint)trItem.ElecSetting.Length;

                        sptMeterItemIndex += (uint)trItem.OptiSettingList.Count;

                        System.Threading.Thread.Sleep(1);


                        #endregion

                        break;
                    //--------------------------------------------------------------------------------------------------------------
                    case ETestType.LCR:
                        sourceMeterReadData = LCR(item, srcMeterItemIndex, lcrMeterItemIndex);

                        srcMeterItemIndex++;
                        lcrMeterItemIndex++;

                        break;
                    //--------------------------------------------------------------------------------------------------------------
                    case ETestType.LCRSWEEP:

                        sourceMeterReadData =LCRSweep( item,  lcrMeterItemIndex);
                        lcrMeterItemIndex++;

                        break;
                    //--------------------------------------------------------------------------------------------------------------
                    case ETestType.OSA:

                        sourceMeterReadData = OSA(item, srcMeterItemIndex, osaItemIndex, srcChannel);

                        srcMeterItemIndex += (uint)item.ElecSetting.Length;

                        osaItemIndex++;

                        break;
                    //--------------------------------------------------------------------------------------------------------------
                    case ETestType.IO:

                        //System.Threading.Thread.Sleep(10);//保險起見，避免IO連續觸發
                        
                        sourceMeterReadData = IO(item, srcMeterItemIndex, srcChannel);

                        srcMeterItemIndex += (uint)item.ElecSetting.Length;

                        break;
                    //--------------------------------------------------------------------------------------------------------------
                    case ETestType.LaserSource:

                        sourceMeterReadData = LaserSource(item);
                        srcMeterItemIndex += (uint)item.ElecSetting.Length;
                        break;


                    //LaserSource
                    //--------------------------------------------------------------------------------------------------------------
                    default:
                        break;
                }

                CalibrateResistance( item);
                
                //----------------------------------------------------------------------
                // Copy meaureed raw value to value
                //----------------------------------------------------------------------
                if (item.MsrtResult != null)
                {
                    for (int i = 0; i < item.MsrtResult.Length; i++)
                    {
                        //----------------------------------------------------------------------
                        // TestItem is not enable, reset meaureed raw value = 0.0d
                        //----------------------------------------------------------------------
                       
                        if (item.IsEnable == false)
                        {
                            item.MsrtResult[i].RawValue = 0.0d;
                        }
                        item.MsrtResult[i].Value = item.MsrtResult[i].RawValue;
                    }
                }
                //----------------------------------------------------------------------
                // Calibrate the measured data and decide the next step. 
                // If the test resuult enable "skip function",
                //----------------------------------------------------------------------
                item.Calibrate();

                // 20161121, Roy Modifiy
                if (item.Type == ETestType.TRANSISTOR || item.Type == ETestType.LIV)
                {
                    item.CalibrateLIVSweepData(this._acquireData.LIVDataSet[item.KeyName]);
                }

                if (item.MsrtResult != null && !_isTVSTesting)
                {
                    //if (item.Type != ETestType.DVF && item.Type != ETestType.LCR && item.Type != ETestType.OSA)//20170904 David
                    if (item.Type != ETestType.DVF && item.Type != ETestType.LCR && item.Type != ETestType.LCRSWEEP && item.Type != ETestType.OSA && item.Type != ETestType.VISCAN)
                    {
                        foreach (TestResultData data in item.MsrtResult)
                        {
                            if (data.Value < 0.0d)
                            {
                                data.Value = 0.0d;
                            }
                        }
                    }
                }

                //----------------------------------------------------------------------
                // Calculate Contact Check Pass / Fail
                //----------------------------------------------------------------------
                if (item.Type == ETestType.ContactCheck)
                {
                    item.MsrtResult[2].Unit = "";
                    item.MsrtResult[2].Value = item.MsrtResult[0].IsPass & item.MsrtResult[1].IsPass ? 1.0d : 2.0d;
                    item.MsrtResult[2].RawValue = item.MsrtResult[0].IsPass & item.MsrtResult[1].IsPass ? 1.0d : 2.0d;
                }

                if (item.MsrtResult != null)
                {
                    for (int k = 0; k < item.MsrtResult.Length; k++)
                    {
                        if (item.MsrtResult[k].IsEnable == true && item.MsrtResult[k].IsSkip == true && item.MsrtResult[k].IsPass == false)
                        {
                            isStopTest = true;
                            this._isStopTest = true;
                        }
                    }
                }

                // suppend this thread to allow other waiting threads to execute
                System.Threading.Thread.Sleep(0);

                if (this._switchDevice != null)
                {
                    if (this._srcMeter != null)
                    {
                        this._srcMeter.TurnOff();
                    }

                    this._switchDevice.DisableCH();
                }

                //check machine State
                LogLastErrrMsg(testItemIndex);
            }

            //----------------------------------------------------------
            // Finish each item testing,
            // Finally turn off the source meter 
            // High Speed Mode => Disable Sleep(8)
            //----------------------------------------------------------
            if (this._srcMeter != null)
            {
                this._srcMeter.TurnOff();
//20190620 David

                if(this._srcMeter is Keithley2600)
                {
                    (this._srcMeter as Keithley2600).SetDefaultIO();
                }
            }

            if (this._sequenceDelayTimne > 0)
            {
                System.Threading.Thread.Sleep((int)this._sequenceDelayTimne);
            }

            //----------------------------------------------------------
            // Reset "TestResult" of the no tested item as ZERO
            //----------------------------------------------------------
            if (isStopTest)
            {
                if (this._esdDevice != null)
                {
                    // if skip, ESD need to precharge for 1st zap value
                    this._esdDevice.PreCharge(0);
                }

                this._isStopTest = true;

                for (testItemIndex = 0; testItemIndex < this._condCtrl.Data.TestItemArray.Length; testItemIndex++)
                {
                    item = this._condCtrl.Data.TestItemArray[testItemIndex];
                    //------------------------------------------------------
                    // (1) First, Reset the testresult data to zero
                    //------------------------------------------------------
                    if (item.MsrtResult != null)
                    {
                        foreach (TestResultData data in item.MsrtResult)
                        {
                            if (!item.IsTested)
                            {
                                data.RawValue = 0.0d;
                                data.Value = 0.0d;
                            }
                        }
                        // 當Contact Check功能開啟後
                        // 填入開路和短路時的預設數值。

                        if (this._sysSetting.contactCheckCFG._isEnableContactCheck && isOpenShortCheckFail)
                        {
                            if (item.Type == ETestType.IF || item.Type == ETestType.LOPWL)
                            {
                                if (isContactCheckUpperBoundary)
                                {
                                    item.MsrtResult[0].RawValue = item.ElecSetting[0].MsrtProtection;
                                    item.MsrtResult[0].Value = item.ElecSetting[0].MsrtProtection;
                                }
                                else
                                {
                                    item.MsrtResult[0].RawValue = 0;
                                    item.MsrtResult[0].Value = 0;
                                }
                            }

                            if (item.Type == ETestType.IZ)
                            {
                                if (this._sysSetting.contactCheckCFG._isEnableVzFillRandomValue)
                                {
                                    Random rd = new Random();

                                    double randomVz = 5 * rd.NextDouble();

                                    item.MsrtResult[0].RawValue = randomVz;

                                    item.MsrtResult[0].Value = randomVz;
                                }
                                else
                                {
                                    if (isContactCheckUpperBoundary)
                                    {
                                        item.MsrtResult[0].RawValue = 0;
                                        item.MsrtResult[0].Value = 0;
                                        //item.MsrtResult[0].RawValue = item.ElecSetting[0].MsrtProtection;
                                        //item.MsrtResult[0].Value = item.ElecSetting[0].MsrtProtection;
                                    }
                                    else
                                    {
                                        item.MsrtResult[0].RawValue = 0;
                                        item.MsrtResult[0].Value = 0;
                                    }
                                }
                            }

                            if (item.Type == ETestType.VR)
                            {
                                item.IsTested = true;
                                item.MsrtResult[0].IsTested = true;

                                if (isContactCheckUpperBoundary)
                                {
                                    item.MsrtResult[0].RawValue = item.ElecSetting[0].MsrtProtection;
                                    item.MsrtResult[0].Value = item.ElecSetting[0].MsrtProtection;

                                }
                                else
                                {
                                    item.MsrtResult[0].RawValue = item.ElecSetting[0].MsrtProtection;
                                    item.MsrtResult[0].Value = item.ElecSetting[0].MsrtProtection;
                                }
                            }

                        }
                    }
                }
            }

            SetOpticalSwitchToDefault();
        }

        private void LogLastErrrMsg(uint testItemIndex)
        {
            if (!CheckAllDeviceErrorState(false))
            {
                string errStr = "Err at item " + testItemIndex +
                    "," + this._condCtrl.Data.TestItemArray[testItemIndex].KeyName;
                Console.WriteLine("[HS_TesterKernel],TestProcess," + errStr);
                if (this._srcMeter != null)
                {
                    string smuErrStr = "SMU Err is " + this._srcMeter.ErrorNumber.ToString();
                    if (this._condCtrl.Data.TestItemArray[testItemIndex].ElecSetting != null &&
                        this._condCtrl.Data.TestItemArray[testItemIndex].ElecSetting.Length > 0)
                    {
                        ElectSettingData es = this._condCtrl.Data.TestItemArray[testItemIndex].ElecSetting[0];
                        smuErrStr += ",Force Time:" + es.ForceTime;
                        smuErrStr += ",Force Value:" + es.ForceValue;
                    }
                    Console.WriteLine("[HS_TesterKernel],TestProcess," + smuErrStr);
                }

                if (this._sptMeter != null)
                { 
                }                
            }
        }
        
        protected override bool RunTestSequence(bool isReTest = false)
        {
            int rem = 0;

            this._isTestSuccess = false;
            _isTriggerSptErr = false;

            this._acquireData.ChipInfo.StartTime = DateTime.Now;
            //-----------------------------------------------------------------------------------
            // (1) Chang the ROW , COL by defined coordinate and
            //      Copy ProberData to this._outputTestResult;
            //-----------------------------------------------------------------------------------		
            //this._sysResultItem[(int)ESysResultItem.CHUCKINDEX].Value = this._cmdData.DoubleData[(uint)EProberDataIndex.PROBE_INDEX];	// 1-Base
            //this._acquireData.ChipInfo.ChipIndex = (int)this._cmdData.DoubleData[2];
            //this._acquireData.ChipInfo.ChuckIndex = (int)this._cmdData.DoubleData[(uint)EProberDataIndex.PROBE_INDEX];

            this._sysResultItem[(int)ESysResultItem.CHUCKINDEX].Value = this._cmdData.DoubleData[(uint)EProberDataIndex.PROBE_INDEX];	// 1-Base

            this._acquireData.ChipInfo.ChipIndex = (int)this._cmdData.DoubleData[2];

            this._sysResultItem[(int)ESysResultItem.CHIP_INDEX].Value = this._cmdData.DoubleData[2];

            this._acquireData.ChipInfo.Channel = 1; // 會導致dutChannel = this._acquireData.ChipInfo.Channel; 出事

            foreach (var sysItem in _sysResultItem)
            {
                sysItem.IsTested = true;
            }


            //-----------------------------------------------------------------------------------
            // (2) Check past tested data has move to UI
            //-----------------------------------------------------------------------------------
            if (this._isMoveDataToStorage == false)
            {
                this.SetErrorCode(EErrorCode.NotFinishMoveTestedData);
                return false;
            }
            //-----------------------------------------------------------------------------------
            // (3) Check Filter Wheel Position
            //-----------------------------------------------------------------------------------

            // Paul 20120731, Disable the function
            // this.CheckMachineHW();

            if (this.Status.ErrorCode != (int)EErrorCode.NONE)
                return false;

            this._ptTestTime.Start();

            this._acquireData.ChipInfo.IsPass = false;

            //   this._DataIR[(int)EDataIR.IsPassFail] = 2;			// 1: Good , 2 : Fail

            this.ChangeRowColCoord();
            //double t = this._ptTestTime.PeekTimeSpan(ETimeSpanUnit.MilliSecond);
            //Console.WriteLine("[HS_Kernel],ChangeRowColCoord," + t.ToString());
            lock (this)
            {
                //--------------------------------------------------------------------
                // (3) Get current dark intensity data for each 200 chips
                //--------------------------------------------------------------------
                if (this._sysSetting.IsEnableDarkCorrect == true)
                {
                    Math.DivRem(this._darkCorrectCount, this._skipGetDarkCounts, out rem);
                    if (this._sptMeter != null && rem == 0)
                    {
                        this._darkCorrectCount = 0;
                        this._darkSample = this._sptMeter.GetDarkSample(5, 8000);
                    }
                }

                //--------------------------------------------------------------------
                // (4) Get PD current dark intensity data for each 200 chips
                //--------------------------------------------------------------------
                this.GetPDDarkCurrent();
                //t = this._ptTestTime.PeekTimeSpan(ETimeSpanUnit.MilliSecond);
                //Console.WriteLine("[HS_Kernel],GetPDDarkCurrent," + t.ToString());
                if (this._sysSetting.IsEnableDarkCorrect || this._sysSetting.PDDarkCorrectMode != EPDDarkCorrectMode.None)
                {
                    this._darkCorrectCount++;
                }
                //--------------------------------------------------------------------
                // (5) Run the test process
                //--------------------------------------------------------------------      

                foreach (var rItem in this._sysResultItem)
                {
                    rItem.IsTested = true;
                }
                this.TestProcess();
            }

            this._acquireData.ChipInfo.EndTime = DateTime.Now;

            this._sysResultItem[(int)ESysResultItem.TIME_SPAN].Value = this._acquireData.ChipInfo.TimeSpan.TotalMilliseconds;
            this._sysResultItem[(int)ESysResultItem.TIME_SPAN].Unit = "ms";

            this._sysResultItem[(int)ESysResultItem.TEST_START_TIME].Value = this._acquireData.ChipInfo.StartTime.Ticks;
            this._sysResultItem[(int)ESysResultItem.TEST_START_TIME].Unit = "Ticks";
            this._sysResultItem[(int)ESysResultItem.TEST_END_TIME].Value = this._acquireData.ChipInfo.EndTime.Ticks;
            this._sysResultItem[(int)ESysResultItem.TEST_START_TIME].Unit = "Ticks";
            this._isTestSuccess = this.CheckAllDeviceErrorState();

            return this._isTestSuccess;
        }

        private void ManaulRun()
        {
            int repeatIndex = 0;
            bool rtn = false;

            _isManualTest = true;

            int colX = 0;
            int rowY = 0;
            int len = (int)Math.Floor(Math.Sqrt(this._cmdData.IntData[1]));

           // bool istestGroup = false;
            bool isBaseOn1_1 = true;
            /////////////////////////////////////////////////////////////////group simulation
            if (isBaseOn1_1)
            {
                colX = 1;
                rowY = 1;
            }
            /////////////////////////////////////////////////////////////////group simulation
            //if (istestGroup)
            //{
            //    this._cmdData.DoubleData[(uint)EProberDataIndex.TestChipGroup] = 1;
            //}
            //////////////////////////////////////////////////////////////////////////////////////////
            while (repeatIndex < this._cmdData.IntData[1])
            {
                /////////////////////////////////////////////////////////////////group simulation
                //if (istestGroup && repeatIndex > 0 && repeatIndex == 10)
                //{
                //    GlobalFlag.TestMode = ETesterTestMode.Overload;
                //    //colX = 0;
                //    this._cmdData.DoubleData[(uint)EProberDataIndex.TestChipGroup] = 2;
                //    colX = 0;
                //    rowY = 0;
                //}

                //////////////////////////////////////////////////////////////////////////////////////////
                this._cmdData.DoubleData[(uint)EProberDataIndex.COL] = colX;
                this._cmdData.DoubleData[(uint)EProberDataIndex.ROW] = rowY;
                this.ChangeRowColCoord();
                //--------------------------------------------------------------------------------
                // In manul run loop, the first chip do "DarkIntensity Correct"
                //--------------------------------------------------------------------------------
                if (repeatIndex == 0)
                {
                    this._sysSetting.IsEnableDarkCorrect = true;
                    this._darkCorrectCount = 0;
                }
                else
                {
                    this._sysSetting.IsEnableDarkCorrect = false;

                    if (this._srcMeter == null)
                    {
                        this.WaitTimeOut(this._sysSetting.DieRepeatTestDelay);		// for Simulator
                    }
                    else
                    {
                        this._srcMeter.TurnOff(0.5, false);
                        System.Threading.Thread.Sleep((int)this._sysSetting.DieRepeatTestDelay);
                    }
                }

                rtn = this.RunTestSequence();

                //double t = this._ptTestTime.PeekTimeSpan(ETimeSpanUnit.MilliSecond);
                //Console.WriteLine("[HS_Kernel],RunTestSequence," + t.ToString());

                if (_isTriggerSptErr == true)
                {
                    this.ChangeRowColCoord();

                    this._darkCorrectCount--;

                    System.Threading.Thread.Sleep(100);

                    rtn = this.RunTestSequence();
                }

                if (rtn == false)
                    break;

                //--------------------------------------------------------------------------------
                // In manul run loop, the parameter data is reseted,
                // this._count.Value will reset to 0
                //--------------------------------------------------------------------------------
                if (repeatIndex != 0 && this._sysResultItem[(int)ESysResultItem.TEST].Value == 0)
                {
                    repeatIndex = 0;
                    continue;
                }

                this._sysResultItem[(int)ESysResultItem.TEST].Value++;

                this.GetTestedDataFromDevice();
                repeatIndex++;

                System.Windows.Forms.Application.DoEvents();

                ///////////////////////////////////////////////
                if (isBaseOn1_1)
                {
                    if (colX < len)
                    {
                        colX++;
                    }
                    else
                    {
                        rowY++;
                        colX = 1;
                    }
                }
                    /////////////////////////////////////////
                else
                {
                    if (colX < len - 1)
                    {
                        colX++;
                    }
                    else
                    {
                        rowY++;
                        colX = 0;
                    }
                }
            }

            GlobalFlag.TestMode = ETesterTestMode.Normal;

            this.EndTestSequence();

            _isManualTest = false;
        }

        private void RunLcrCalibration(ELCRCaliMode mode)
        {
            //this._lcrMeter.SetCaliData(_product.LCRCaliData);

            //_sysCali
            //this._lcrMeter.SetCaliData(_condCtrl.LCRCaliData);
            if (this._lcrMeter != null)
            {
                this._lcrMeter.SetCaliData(_sysCali.SystemCaliData.LCRCaliData);
                //SystemCali

                //ELCRCaliMode.DataCollect
                switch (mode)
                {
                    case ELCRCaliMode.Set:
                        break;
                    case ELCRCaliMode.Open:
                    case ELCRCaliMode.Short:
                    case ELCRCaliMode.Load:
                        this._lcrMeter.LCRCali(mode);
                        break;
                    case ELCRCaliMode.DataCollect:
                        this._lcrMeter.GetRawDataOfMeterCali();
                        break;
                }
            }

        }

        #endregion

        #region >>> Public Methods <<<

        public override void Init<T, K>(T config, K rdFunc)
        {
            //---------------------------------------------------------------------------------------
            // (1) Load the machine config file
            //---------------------------------------------------------------------------------------
            this.LoadMachineCfgFile();

            //this.LoadRDFuncParam();
            this._rdFunc = rdFunc as RDFunc;
            //---------------------------------------------------------------------------------------
            // (2) System set to simulator mode
            //---------------------------------------------------------------------------------------
            if (this._machineConfig.Enable.IsSimulator == true)
            {
                this._machineInfo.TesterSN = "Sim. Tester SN";
                this._machineInfo.SpectrometerSN = "Sim. Spectrometer SN";
                this._machineInfo.SphereSN = "Sim. Sphere SN";

                this._status.State = EKernelState.Ready;
                this._sptMeter = null;
                this._srcMeter = null;
                this._lcrMeter = null;
                this._lcrBias = null;

                return;
            }





            //---------------------------------------------------------------------------------------


            //---------------------------------------------------------------------------------------
            // (3) Create Source Meter Instance and Initialize it 
            //---------------------------------------------------------------------------------------

            ElecDevSetting devSetting = SetElecDevsetting();

            if (!SetSrcMeter(devSetting))
            {
                if (this._srcMeter != null)
                    this.SetErrorCode(this._srcMeter.ErrorNumber);
                return;
            }

            //---------------------------------------------------------------------------------------
            // (4) Create the spectrometer instance and initialize it, 
            //      Then test the trigger function, and get data from sepectrometer
            //		Get the wavelength of each pixel at spectrometer 
            //---------------------------------------------------------------------------------------

            if (!SetSptMeter())
            {
                if (this._sptMeter != null)
                    this.SetErrorCode(this._sptMeter.ErrorNumber);
                return;
            }
            //---------------------------------------------------------------------------------------
            // (5) Create ESDCtrl Instance and Initialize it 
            //---------------------------------------------------------------------------------------
            
            ESDDevSetting esdSetting = SetESDDevSetting();

            if (!SetEsdDevice(esdSetting))
            {
                if (this._esdDevice != null)
                    this.SetErrorCode(this._esdDevice.ErrorNumber);
                return;
            }
            //---------------------------------------------------------------------------------------
            // (6) Create SwitchBox Instance and Initialize it 
            //---------------------------------------------------------------------------------------
            if (!SetSwitchBox())
            {
                if (this._switchDevice != null)
                    this.SetErrorCode(this._switchDevice.ErrorNumber);
                return;
            }
            //---------------------------------------------------------------------------------------
            // (7) Initialize LCR Meter and config the default setting 
            //---------------------------------------------------------------------------------------

            #region >>LCR<<
            LCRDevSetting lcrDevSetting = SetLCRDevSetting();

            if (!SetLcrMeter(lcrDevSetting, devSetting))
            {
                if (this._lcrMeter != null)
                {
                    if (this._lcrMeter.ErrorNumber != EDevErrorNumber.Device_NO_Error)
                    {
                        this.SetErrorCode(this._lcrMeter.ErrorNumber);
                    }
                }

                if (this._lcrBias != null)
                {
                    if (this._lcrBias.ErrorNumber != EDevErrorNumber.Device_NO_Error)
                    {
                        this.SetErrorCode(this._lcrBias.ErrorNumber);
                    }
                }
                return;
            }

            #endregion
            //---------------------------------------------------------------------------------------
            // (8) Create OSA Instance and Initialize it 
            //---------------------------------------------------------------------------------------
            #region >>OSA<<
            if (!SetOSA())
            {
                if (this._osaDevice != null)
                    this.SetErrorCode(this._osaDevice.ErrorNumber);
                return;

            }
            #endregion
            //---------------------------------------------------------------------------------------
            // (9) Initialize AD Card and config the default setting 
            //---------------------------------------------------------------------------------------
            //if (this._machineConfig.PDSensingMode == EPDSensingMode.DAQ)
            //{
            //    this._ADCard = new PCI826LU();
            //    this._ADCard.Init();
            //    this._ADCard.Config();
            //}

            //---------------------------------------------------------------------------------------
            // (10) Initialize PrintPort 
            //---------------------------------------------------------------------------------------
            this._IOPort = new PortAccess();

            //---------------------------------------------------------------------------------------
            // (11) Initialize LaserPostCalc 
            //---------------------------------------------------------------------------------------
            this._laserPostCalc = new MpiLaserPostCalc();

            //---------------------------------------------------------------------------------------
            // (12) Initialize LaserSource System,  need other device io to init
            //---------------------------------------------------------------------------------------

            //---------------------------------------------------------------------------------------
            // (3) Initialize LaserSource System
            //---------------------------------------------------------------------------------------
            this._laserSrcSys = new LaserSourceSystem();
            if (!SetLaserSource())
                return;

            //if (!SetLaserSource2())
            //    return;
            //this._laserSrcSys = new LaserSourceSystem();

            //if (!SetLaserSource())
            //    return;
            //---------------------------------------------------------------------------------------
            //  All hardware are created and finish intialization. 
            //      The kernel system transfer to read state
            //---------------------------------------------------------------------------------------
            this._status.State = EKernelState.Ready;

            return;
        }

        public override void Close()
        {
            base.Close();

            if (this._lcrMeter != null)
            {
                this._lcrMeter.Close();
            }

            if (this._lcrBias != null)
            {
                this._lcrBias.Close();
            }

            if (this._osaDevice != null)
            {
                this._osaDevice.Close();
            }

            this._status.State = EKernelState.Not_Ready;
        }

        public override bool SetSysData<T, M>(T sysData, M sysCail)
        {
            this._sysSetting = (sysData as TesterSetting).Clone() as TesterSetting;

            //this._sysCali = (sysCail as SystemCali).Clone() as SystemCali;

            this._sysCali = sysCail as SystemCali;

            return true;
        }

        public override bool SetCondionData<T, K, M>(T cd, K product, M binSort)
        {
            //--------------------------------------------------------------------------------------------------------
            // Move UI data to system kernal  
            //--------------------------------------------------------------------------------------------------------
            List<ElectSettingData> elecSettingList = new List<ElectSettingData>();
            List<OptiSettingData> optiSettingList = new List<OptiSettingData>();
            List<ESDSettingData> esdSettingList = new List<ESDSettingData>();
            List<LCRSettingData> lcrSettingList = new List<LCRSettingData>();
            List<ElectSettingData> lcrBiasList = new List<ElectSettingData>();
            List<OsaSettingData> osaSettingList = new List<OsaSettingData>();

            this._condCtrl = cd as ConditionCtrl;
            this._product = product as ProductData;
            this._binCtrl = binSort as SmartBinning;

            this._condCtrl.Data.OpenShortIFTestItem.ElecSetting[0].ForceValue = 0.0001;
            this._condCtrl.Data.OpenShortIFTestItem.ElecSetting[0].ForceTime = 0.5;
            this._condCtrl.Data.OpenShortIFTestItem.ElecSetting[0].IsAutoForceRange = true;
            this._condCtrl.Data.OpenShortIFTestItem.ElecSetting[0].IsAutoTurnOff = false;
            this._condCtrl.Data.OpenShortIFTestItem.MsrtResult[0].MinLimitValue = 1;

            if (this._status.State == EKernelState.Running)
                return false;

            elecSettingList.Clear();
            optiSettingList.Clear();
            esdSettingList.Clear();
            lcrSettingList.Clear();

            //--------------------------------------------------------------------------------------------------------
            // Set configuration and parameters data to hardware device 
            // by recipe or condtion setting
            //--------------------------------------------------------------------------------------------------------
            if (this._product == null)
                return true;

            //--------------------------------------------------------------------------------------------------------
            // (1) Fisrt, set and transfer relative parameter to local variable 
            //--------------------------------------------------------------------------------------------------------
            #region
            SetPolarity();
            #endregion
            //--------------------------------------------------------------------------------------------------------
            // (2) Count the elecTestItem, opticalTestItem and ESDTestItem,
            //      then covert the positive and negative value by the chip polarity
            //--------------------------------------------------------------------------------------------------------
            

            uint electSettingOrder = 0;

            SetContactCheck(elecSettingList, ref electSettingOrder);

            ModifySMUSetting();

            #region >>setting List<<

            uint electOrder = 0;

            if (this._condCtrl.Data != null)
            {
                if (this._condCtrl.Data.TestItemArray != null)
                {
                    foreach (TestItemData item in this._condCtrl.Data.TestItemArray)
                    {
                        if (item.ElecSetting != null)
                        {
                            foreach (ElectSettingData data in item.ElecSetting)
                            {
                                data.Order = electOrder;
                                elecSettingList.Add(data.ConvertUnitTo(EAmpUnit.A.ToString(), EVoltUnit.V.ToString(), this._chipPolarity));
                                electOrder++;
                            }
                        }

                        if (item is LOPWLTestItem)
                        {
                            optiSettingList.Add((item as LOPWLTestItem).OptiSetting);
                        }
                        else if (item is LIVTestItem)
                        {
                            optiSettingList.AddRange((item as LIVTestItem).OptiSettingList);
                        }
                        else if (item is TransistorTestItem)
                        {
                            optiSettingList.AddRange((item as TransistorTestItem).OptiSettingList);
                        }
                        else if (item is VLRTestItem)
                        {
                            optiSettingList.Add((item as VLRTestItem).OptiSetting);
                        }
                        else if (item is LCRTestItem)
                        {
                            lcrSettingList.Add((item as LCRTestItem).LCRSetting);

                            foreach (ElectSettingData data in item.ElecSetting)
                            {
                                lcrBiasList.Add(data.ConvertUnitTo(EAmpUnit.A.ToString(), EVoltUnit.V.ToString(), this._chipPolarity));
                            }
                        }//LCRSweepTestItem
                        else if (item is LCRSweepTestItem)
                        {
                            lcrSettingList.Add((item as LCRSweepTestItem).LCRSetting);

                            foreach (ElectSettingData data in item.ElecSetting)
                            {
                                lcrBiasList.Add(data.ConvertUnitTo(EAmpUnit.A.ToString(), EVoltUnit.V.ToString(), this._chipPolarity));
                            }
                        }

                        if (item is ESDTestItem)
                        {
                            ESDSettingData esdSetting = (item as ESDTestItem).EsdSetting.Clone() as ESDSettingData;
                            esdSetting.IsEnable = item.IsEnable;

                            // Chage the ESD polarity, if the chip polarity is changed
                            if (this._product.TestCondition.ChipPolarity == EPolarity.Cathode_N)
                            {
                                if (esdSetting.Polarity == EESDPolarity.P)
                                {
                                    esdSetting.Polarity = EESDPolarity.N;
                                }
                                else
                                {
                                    esdSetting.Polarity = EESDPolarity.P;
                                }
                            }

                            esdSettingList.Add(esdSetting.Clone() as ESDSettingData);
                        }

                        if (item is OSATestItem)
                        {
                            (item as OSATestItem).OsaSettingData.IsTrigger = item.IsEnable;

                            osaSettingList.Add((item as OSATestItem).OsaSettingData);
                        }
                    }
                }
            }

            
                SetIOCheck();
            
            this._testSequencElecCount = (uint)elecSettingList.Count;

            this.ResetKernelData();

            if (this.Status.State == EKernelState.Not_Ready)
                return false;

            #endregion
            //--------------------------------------------------------------------------------------------------------
            // (3) Set "Spectrometer" config and parameters data 
            //--------------------------------------------------------------------------------------------------------
            this._sysSetting.OptiDevSetting.AttenuatorPos = this._product.ProductFilterWheelPos;

            //--------------------------------------------------------------------------------------------------------
            // (3-1) Set "Spectrometer" XY Calibration Array 
            //--------------------------------------------------------------------------------------------------------

            if (!SetSpectrometerXYCali(optiSettingList))
            {
                return false;
            }
            //--------------------------------------------------------------------------------------------------------
            // (4) Set "SourceMeter" config and parameters data 
            //--------------------------------------------------------------------------------------------------------
            #region

            if (this._machineConfig.SourceMeterModel != ESourceMeterModel.NONE && this._srcMeter != null)
            {
                if (this._srcMeter.SetParamToMeter(elecSettingList.ToArray()) == false &&
                    this._srcMeter.ErrorNumber != EDevErrorNumber.Device_NO_Error)//LCR Meter
                {
                    this.SetErrorCode(this._srcMeter.ErrorNumber);
                    return false;
                }
            }

            #endregion
            //--------------------------------------------------------------------------------------------------------
            // (5) Set "ESD" config and parameters data 
            //--------------------------------------------------------------------------------------------------------
            #region

            if (this._machineConfig.ESDModel != EESDModel.NONE && this._esdDevice != null)
            {
                //-----------------------------------------------------------------------------------
                //  20131205 Roy
                //  新增 ESD 正常版 與 快速版(含 ESD 放慢之 Delay Time) 切換
                //-----------------------------------------------------------------------------------
                this._esdDevice.SetConfigToMeter(this._sysSetting.EsdDevSetting);

                if (this._esdDevice.SetParamToMeter(esdSettingList.ToArray()) == false)
                {
                    this.SetErrorCode(this._esdDevice.ErrorNumber);
                    return false;
                }

                this._esdDevice.ResetToSafeStatus();

                //this._esdDevice.PreCharge();
            }

            #endregion
            //--------------------------------------------------------------------------------------------------------
            // (6) Set "OSA" config and parameters data 
            //--------------------------------------------------------------------------------------------------------
            #region

            if (this._machineConfig.OSAModel != EOSAModel.NONE && this._osaDevice != null)
            {
                this._osaDevice.SetConfigToMeter(this._sysSetting.OsaDevSetting);

                if (this._osaDevice.SetParaToMeter(osaSettingList.ToArray()) == false)
                {
                    this.SetErrorCode(this._osaDevice.ErrorNumber);
                    return false;
                }
            }

            this._adjacentCheck.Reset(this._sysSetting.ProberCoord, this._sysSetting.TesterCoord, (int)this._product.AdjacentConsecutiveErrorCount, this._sysSetting.IsEnableSaveDetailLog);

            this._passRateCheck.Start(this._sysSetting, this._condCtrl.Data.TestItemArray);

            this._repeatTestIndexs.Clear();

            this._dataVerify.Start();

            this._skipCount = 0;

            this._isTVSTesting = this._product.IsTVSProduct;


            #endregion
            //--------------------------------------------------------------------------------------------------------
            // (7) Set "LCR Meter" config and parameters data 
            //--------------------------------------------------------------------------------------------------------
            #region
            if (this._machineConfig.LCRModel != ELCRModel.NONE && this._lcrMeter != null)
            {
                if (this._lcrMeter.SetCaliData(_sysCali.SystemCaliData.LCRCaliData))
                {
                    this.SetErrorCode(this._lcrMeter.ErrorNumber);
                }

                if (this._lcrMeter.SetParamToMeter(lcrSettingList.ToArray()) == false)
                {
                    if (this._lcrMeter.ErrorNumber != EDevErrorNumber.Device_NO_Error)
                    {
                        this.SetErrorCode(this._lcrMeter.ErrorNumber);
                    }

                    return false;
                }
            }

            if (this._lcrMeter != null && this._lcrBias != null)
            {
                if (this._lcrBias.SetParamToMeter(lcrBiasList.ToArray()) == false)
                {
                    this.SetErrorCode(this._lcrBias.ErrorNumber);

                    return false;
                }
            }
            #endregion

            //--------------------------------------------------------------------------------------------------------
            // (8) Set "Attenuator" config and parameters data 
            //--------------------------------------------------------------------------------------------------------
            #region >>Attenuator<<
            if (this._machineConfig.LaserSrcSysConfig != null &&
                _laserSrcSys != null && _laserSrcSys.AttManager != null && this._product.TestCondition.TestItemArray != null)
            {
                List<AttenuatorSettingData> attSet = new List<AttenuatorSettingData>();
                //Emcore 版操作手法
                if (this._product.LaserSrcSetting != null && this._machineConfig.LaserSrcSysConfig.Attenuator != null &&
               this._machineConfig.LaserSrcSysConfig.Attenuator.AttenuatorModel == ELaserAttenuatorModel.N7760A &&
               this._product.LaserSrcSetting.AttenuatorData != null)
                {
                    attSet.Add(this._product.LaserSrcSetting.AttenuatorData);
                }

                foreach (var item in this._product.TestCondition.TestItemArray)
                {
                    if (item.Type == ETestType.LaserSource && item.IsEnable)
                    {
                        LaserSourceTestItem lsItem = (LaserSourceTestItem)item;
                        if (lsItem.LaserSourceSet.AttenuatorData != null)
                        {
                            attSet.Add(lsItem.LaserSourceSet.AttenuatorData);
                        }
                    }
                }

                if (!_laserSrcSys.SetParamToAttenuator(attSet))
                {
                    this.SetErrorCode(this._laserSrcSys.ErrorNumber);
                    return false;
                }
            }

            #endregion
            //-----------------------------------------------------------------------------------
            //  20140127 Paul
            //  先補償大係數再補償小係數，並且使用修正後的波長值
            //-----------------------------------------------------------------------------------
            #region
            if (this._sysSetting.IsEnalbeCalcBigFactorBeforeSmall)
            {
                this._condCtrl.Data.CalLookupWave = ECalLookupWave.Corrected;
            }
            else
            {
                this._condCtrl.Data.CalLookupWave = ECalLookupWave.Original;
            }

            if (this._sysSetting.IsEnableHighSpeedMode)
            {
                _sequenceDelayTimne = 0;
            }
            else
            {
                _sequenceDelayTimne = this._rdFunc.RDFuncData.HighSpeedModeDelayTime;
            }
            #endregion
            //-----------------------------------------------------------------------------------
            //  20180925 David
            //  起始化 HS_Kernal local variable
            //-----------------------------------------------------------------------------------
            //P2TcoordTransTool = null;
            _rManager = new ReTestManager();

            CheckIOState(0);

            SetOpticalSwitchToDefault();
            //--------------------------------------------------------------------------------------------------------
            // Check all device error state 
            //--------------------------------------------------------------------------------------------------------
            Console.WriteLine("[HS_TesterKernel], Enable Spectrometer State Check => " + this._sysSetting.IsEnableErrStateReTest.ToString());
            Console.WriteLine("[HS_TesterKernel], Enable Log Detail Information  => " + this._sysSetting.IsEnableSaveDetailLog.ToString());
            Console.WriteLine("[HS_TesterKernel], Enable Adjacent Error Check  => " + this._sysSetting.IsEnableAdjacentError.ToString());
            Console.WriteLine("[HS_TesterKernel], Enable High Speed Mode  => " + this._rdFunc.RDFuncData.HighSpeedModeDelayTime.ToString());
            Console.WriteLine("[HS_TesterKernel], Enable ESD High Speed Mode  => " + this._sysSetting.EsdDevSetting.IsHighSpeedMode.ToString());
            Console.WriteLine("[HS_TesterKernel], Enable SrcMeter QC Check => " + this._isQCSetting);
            return (this.CheckAllDeviceErrorState());

        }
     
        private void ModifySMUSetting()
        {
            #region
            if (this._condCtrl.Data != null)
            {
                if (this._condCtrl.Data.TestItemArray != null)
                {
                    //foreach (TestItemData item in this._condCtrl.Data.TestItemArray)
                    for (int i = 0; i < this._condCtrl.Data.TestItemArray.Length; i++)
                    {
                        TestItemData item = this._condCtrl.Data.TestItemArray[i];
                        //-------------------------------------------------------------------
                        // K26xx時，強迫Thy TestItem的 NPLC=0.03，Move Averag=5
                        //-------------------------------------------------------------------

                        if (item is THYTestItem)
                        {
                            if (this._machineConfig.SourceMeterModel == ESourceMeterModel.K2600)
                            {
                                (item as THYTestItem).ElecSetting[0].MsrtNPLC = 0.003;

                                (item as THYTestItem).ElecSetting[0].ThyMovingAverageWindow = 5;

                                (item as THYTestItem).ElecSetting[0].ThySGFilterCount = 0;
                            }
                        }
                        else if (item is VRTestItem)
                        {
                            (item as VRTestItem).ElecSetting[0].IsFloatForceValue = (item as VRTestItem).IsUseVzAsForceValue;
                        }
                        else if (item is IZTestItem)
                        {
                            (item as IZTestItem).ElecSetting[0].IsFloatForceValue = (item as IZTestItem).IsUseIrAsForceValue;
                        }

                        //-------------------------------------------------------------------
                        // 尋找 ESDTestItem上一道電，並將 IsNextIsESDTestItem = true
                        //-------------------------------------------------------------------
                        if (item is ESDTestItem)
                        {
                            if (this._condCtrl.Data.TestItemArray.Length < 2 || i - 1 < 0)
                            {
                                continue;
                            }

                            for (int j = i - 1; j >= 0; j--)
                            {
                                TestItemData last = this._condCtrl.Data.TestItemArray[j];

                                if (!last.IsEnable || last.ElecSetting == null)
                                {
                                    continue;
                                }

                                if (last is ESDTestItem && !(last as ESDTestItem).IsEnableJudgeItem)
                                {
                                    continue;
                                }

                                for (int k = 0; k < last.ElecSetting.Length; k++)
                                {
                                    if (k == last.ElecSetting.Length - 1)
                                    {
                                        last.ElecSetting[k].IsNextIsESDTestItem = true;
                                    }
                                }

                                break;
                            }
                        }
                        else
                        {
                            if (item.ElecSetting != null)
                            {
                                foreach (var data in item.ElecSetting)
                                {
                                    data.IsNextIsESDTestItem = false;
                                }
                            }
                        }
                    }
                }
            }
            #endregion
        }

        public bool ResetMachineHW()
        {
            List<OptiSettingData> optiSettingList = new List<OptiSettingData>();

            if (this._condCtrl.Data != null)
            {
                if (this._condCtrl.Data.TestItemArray != null)
                {
                    foreach (TestItemData item in this._condCtrl.Data.TestItemArray)
                    {
                        if (item is LOPWLTestItem)
                        {
                            optiSettingList.Add((item as LOPWLTestItem).OptiSetting);
                        }
                        else if (item is VLRTestItem)
                        {
                            optiSettingList.Add((item as VLRTestItem).OptiSetting);
                        }
                    }
                }
            }


            if (this._machineConfig.SpectrometerModel != ESpectrometerModel.NONE && this._sptMeter != null)
            {
                //-----------------------------------------------------------------------------------
                // Re-Initialize the spectrometer for every cycle run. Gilbert
                //-----------------------------------------------------------------------------------
                if (this._machineConfig.SpectrometerModel == ESpectrometerModel.USB2000P)
                {
                    this._skipGetDarkCounts = 200;
                    this._sptMeter.Init(0, this._machineConfig.SpectrometerSN, this._machineConfig.SphereSN);
                }
                else
                {
                    this._skipGetDarkCounts = 50000;
                }

                //--------------------------------------------------------------------------------------------------------
                // Roy 改變 Spt設定順續, 原先 SetConfigToMeter -> SetParamToMeter, 改為 SetParamToMeter -> SetConfigToMeter

                if (this._sptMeter.SetParamToMeter(optiSettingList.ToArray()) == false)
                {
                    this.SetErrorCode(this._sptMeter.ErrorNumber);
                    return false;
                }

                if (this._sptMeter.SetConfigToMeter(this._sysSetting.OptiDevSetting) == false)
                {
                    this.SetErrorCode(this._sptMeter.ErrorNumber);
                    return false;
                }

            }

            return true;
        }

        public override void ResetTesterCond()
        {
            this.CheckAllDeviceErrorState();
            return;
        }

        public override void GetTestedDataFromDevice()
        {
            double[] calcAbsIntensity = null;

            if (this._condCtrl.Data == null || this._condCtrl.Data.TestItemArray == null)
                return;


            if (this._srcMeter != null)
            {
                if (this._srcMeter is Keithley2600)
                {
                    (this._srcMeter as Keithley2600).CollectGarbage();
                }
            }
            this._sysResultItem[(int)ESysResultItem.CHANNEL].Value = 1;
            
            this._acquireData.ChipInfo.TestCount = (int)this._sysResultItem[(int)ESysResultItem.TEST].Value;

            if ((int)this._sysResultItem[(int)ESysResultItem.POLAR].Value == 1)
            {
                this._acquireData.ChipInfo.Polarity = EPolarity.Anode_P;
            }
            else
            {
                this._acquireData.ChipInfo.Polarity = EPolarity.Cathode_N;
            }

            this._acquireData.ChipInfo.TestTime = this._ptTestTime.PeekTimeSpan(ETimeSpanUnit.MilliSecond);
            
            this._sysResultItem[(int)ESysResultItem.SEQUENCETIME].Value = this._acquireData.ChipInfo.TestTime;

            uint LOPWLIndex = 0;
            uint osaIndex = 0;
            uint channel = 0;

            double[] tempValue = null;

            foreach (TestItemData item in this._condCtrl.Data.TestItemArray)
            {
                if (this._machineConfig.Enable.IsSimulator)
                {
                    continue;
                }


                if (this._osaDevice != null)
                {
                    if (item is OSATestItem)
                    {
                        #region >>> OSA <<<

                        if (this._osaDevice.CalculateMeasureResultData(osaIndex))
                        {
                            OsaData od = this._osaDevice.Data[osaIndex];

                            this._acquireData.SpectrumDataSet[channel, item.KeyName].Wavelength = od.Wavelength.ToArray();
                            this._acquireData.SpectrumDataSet[channel, item.KeyName].Absoluate = od.Spectrum.ToArray();
                            this._acquireData.SpectrumDataSet[channel, item.KeyName].Intensity = od.Spectrum.ToArray();
                        }
                        osaIndex++;

                        #endregion
                    }
                }

                //---------------------------------------------------------------------------------------------
                // (1) this.GetOptiMsrtResult() call the calculation of spectrometer
                //---------------------------------------------------------------------------------------------
                if (this._sptMeter == null)
                    continue;

                this.GetOptiMsrtResult(LOPWLIndex, item);

                if (item is LOPWLTestItem)
                {
                    #region >>> LOPWL Test Item <<<

                    //---------------------------------------------------------------------------------------------
                    // (2) After calculation of spectrometer, it can get absolute spectrum
                    //---------------------------------------------------------------------------------------------
                    calcAbsIntensity = this._sptMeter.GetYAbsoluateSpectrum(LOPWLIndex);

                    Array.Copy(calcAbsIntensity, 0, this._acquireData.SpectrumDataSet[channel, item.KeyName].Absoluate, 0, this._acquireData.SpectrumDataSet[channel, item.KeyName].Absoluate.Length);

                    if (this._darkSample != null)
                    {
                        Array.Copy(this._darkSample, 0, this._acquireData.SpectrumDataSet[channel, item.KeyName].Dark, 0, this._acquireData.SpectrumDataSet[channel, item.KeyName].Dark.Length);
                    }

                    //---------------------------------------------------------------------------------------------
                    // (3) Call the calibration for LOPWL test item
                    //---------------------------------------------------------------------------------------------
                    //==================================
                    // (3-a) Calibrate LOP value by chuck correction 
                    //==================================
                    this._condCtrl.CalibrateChuckLOP(((int)this._sysResultItem[(int)ESysResultItem.CHUCKINDEX].Value) - 1,
                                                                                this._product.ChuckLOPCorrectArray,
                                                                                item.MsrtResult);

                    //==================================
                    // (3-b) Calibrate LOP value by lookup table 
                    //==================================
                    if (this._sysSetting.IsEnalbeCalcBigFactorBeforeSmall)
                    {
                        item.Calibrate();

                        this._condCtrl.CalibrateLOPWL(item, true);
                    }
                    else
                    {
                        this._condCtrl.CalibrateLOPWL(item, true);

                        tempValue = new double[item.MsrtResult.Length];

                        for (int i = 0; i < tempValue.Length; i++)
                        {
                            tempValue[i] = item.MsrtResult[i].RawValue;
                            item.MsrtResult[i].RawValue = item.MsrtResult[i].Value;     // Move value of lookup table correction to "RawValue" field
                        }
                        //==================================
                        // (3-c) Calibrate LOP value by "Gain/Offset"
                        //==================================
                        item.Calibrate();   // for Gain Offset calibration by RawValue, 	

                        for (int i = 0; i < tempValue.Length; i++)
                        {
                            item.MsrtResult[i].RawValue = tempValue[i];
                        }
                    }

                    //==================================
                    // (3-d) Calibrate the other mesaured result
                    //==================================
                    item.MsrtResult[(int)EOptiMsrtType.DWDWP].RawValue = item.MsrtResult[(int)EOptiMsrtType.WLD].RawValue - item.MsrtResult[(int)EOptiMsrtType.WLP].RawValue;
                    item.MsrtResult[(int)EOptiMsrtType.DWDWP].Value = item.MsrtResult[(int)EOptiMsrtType.WLD].Value - item.MsrtResult[(int)EOptiMsrtType.WLP].Value;
                    item.MsrtResult[(int)EOptiMsrtType.CIEz].RawValue = 1.0d - item.MsrtResult[(int)EOptiMsrtType.CIEx].RawValue - item.MsrtResult[(int)EOptiMsrtType.CIEy].RawValue;
                    item.MsrtResult[(int)EOptiMsrtType.CIEz].Value = 1.0d - item.MsrtResult[(int)EOptiMsrtType.CIEx].Value - item.MsrtResult[(int)EOptiMsrtType.CIEy].Value;

                    item.MsrtResult[(int)EOptiMsrtType.MVFLD].RawValue = item.MsrtResult[(int)EOptiMsrtType.MVFLB].RawValue - item.MsrtResult[(int)EOptiMsrtType.MVFLA].RawValue;
                    item.MsrtResult[(int)EOptiMsrtType.MVFLD].Value = item.MsrtResult[(int)EOptiMsrtType.MVFLB].Value - item.MsrtResult[(int)EOptiMsrtType.MVFLA].Value;
                    //==================================
                    // Paul, 20130221
                    // 新增電功率(EWATT), 發光效率 lm/EWATT (LE) , 發光效率 watt/EWATT (WPE) ;
                    //==================================
                    item.MsrtResult[(int)EOptiMsrtType.EWATT].RawValue = item.MsrtResult[(int)EOptiMsrtType.MFILA].RawValue * item.MsrtResult[(int)EOptiMsrtType.MVFLA].RawValue;
                    item.MsrtResult[(int)EOptiMsrtType.EWATT].Value = item.MsrtResult[(int)EOptiMsrtType.MFILA].Value * item.MsrtResult[(int)EOptiMsrtType.MVFLA].Value;

                    item.MsrtResult[(int)EOptiMsrtType.LE].RawValue = item.MsrtResult[(int)EOptiMsrtType.LM].RawValue / item.MsrtResult[(int)EOptiMsrtType.EWATT].RawValue;
                    item.MsrtResult[(int)EOptiMsrtType.WPE].RawValue = (0.001 * item.MsrtResult[(int)EOptiMsrtType.WATT].RawValue) / item.MsrtResult[(int)EOptiMsrtType.EWATT].RawValue;

                    if (item.MsrtResult[(int)EOptiMsrtType.EWATT].Value == 0)
                    {
                        item.MsrtResult[(int)EOptiMsrtType.LE].Value = 0;
                        item.MsrtResult[(int)EOptiMsrtType.WPE].Value = 0;
                    }
                    else
                    {
                        item.MsrtResult[(int)EOptiMsrtType.LE].Value = item.MsrtResult[(int)EOptiMsrtType.LM].Value / item.MsrtResult[(int)EOptiMsrtType.EWATT].Value;
                        item.MsrtResult[(int)EOptiMsrtType.WPE].Value = (0.001 * item.MsrtResult[(int)EOptiMsrtType.WATT].Value) / item.MsrtResult[(int)EOptiMsrtType.EWATT].Value;
                    }

                    // this._sysResultItem[(int)ESysResultItem.SEQUENCETIME].Value = this._DataDR[(int)EDataDR.TestTime] - (item.MsrtResult[(int)EOptiMsrtType.ST].RawValue + 6 + conditionTime);

                    //==================================
                    // Stanley, 20130313
                    // 新增CCT計算，分為:(1)由校正前CIExy計算出CCT，再做CCT校正 (2)直接由校正後CIExy計算出CCT後不再做CCT校正。
                    //==================================
                    if (this._sysSetting.OptiDevSetting.IsCalcCCTByCaliCIExy)
                    {
                        double cieX = item.MsrtResult[(int)EOptiMsrtType.CIEx].Value;

                        double cieY = item.MsrtResult[(int)EOptiMsrtType.CIEy].Value;


                        if (this._sysSetting.CCTcaculationType == ECCTCaculationType.McCamy)
                        {
                            item.MsrtResult[(int)EOptiMsrtType.CCT].Value = MpiSPAM.ComputeCorrelatedColorTemperature(cieX, cieY);
                        }
                        else
                        {
                            item.MsrtResult[(int)EOptiMsrtType.CCT].Value = CommonCCTCalculate.Robertson31PointsMethod(cieX, cieY);
                        }
                    }

                    //==================================
                    //限制最大位數為七位
                    //==================================
                    if (item.MsrtResult[(int)EOptiMsrtType.CCT].Value > 9999999)
                    {
                        item.MsrtResult[(int)EOptiMsrtType.CCT].Value = 9999999.0d;
                    }

                    LOPWLIndex++;

                    #endregion
                }
                else if (item is VLRTestItem)
                {
                    #region >>> VLR Test Item <<<

                    //---------------------------------------------------------------------------------------------
                    // (2) After calculation of spectrometer, it can get absolute spectrum
                    //---------------------------------------------------------------------------------------------					
                    calcAbsIntensity = this._sptMeter.GetYAbsoluateSpectrum(LOPWLIndex);

                    //   Array.Copy(calcAbsIntensity, 0, this._chart[item.KeyName][6], 0, this._chart[item.KeyName][6].Length);

                    if (this._darkSample != null)
                    {
                        //       Array.Copy(this._darkSample, 0, this._chart[item.KeyName][7], 0, this._chart[item.KeyName][7].Length);
                    }

                    //---------------------------------------------------------------------------------------------
                    // (3) Call the calibration for LOPWL test item
                    //---------------------------------------------------------------------------------------------
                    //==================================
                    // (3-a) Calibrate LOP value by chuck correction 
                    //==================================
                    this._condCtrl.CalibrateChuckLOP(((int)this._sysResultItem[(int)ESysResultItem.CHUCKINDEX].Value) - 1,
                                                                                this._product.ChuckLOPCorrectArray,
                                                                                item.MsrtResult);

                    //==================================
                    // (3-b) Calibrate LOP value by lookup table 
                    //==================================
                    if (this._sysSetting.IsEnalbeCalcBigFactorBeforeSmall)
                    {
                        item.Calibrate();

                        this._condCtrl.CalibrateLOPWL(item, true);
                    }
                    else
                    {
                        this._condCtrl.CalibrateLOPWL(item, true);

                        tempValue = new double[item.MsrtResult.Length];

                        for (int i = 0; i < tempValue.Length; i++)
                        {
                            tempValue[i] = item.MsrtResult[i].RawValue;
                            item.MsrtResult[i].RawValue = item.MsrtResult[i].Value;     // Move value of lookup table correction to "RawValue" field
                        }
                        //==================================
                        // (3-c) Calibrate LOP value by "Gain/Offset"
                        //==================================
                        item.Calibrate();   // for Gain Offset calibration by RawValue, 	

                        for (int i = 0; i < tempValue.Length; i++)
                        {
                            item.MsrtResult[i].RawValue = tempValue[i];
                        }
                    }

                    //==================================
                    // (3-d) Calibrate the other mesaured result
                    //==================================
                    item.MsrtResult[(int)EVLROptiMsrtType.VLRDWDWP].RawValue = item.MsrtResult[(int)EVLROptiMsrtType.VLRWLD].RawValue - item.MsrtResult[(int)EVLROptiMsrtType.VLRWLP].RawValue;
                    item.MsrtResult[(int)EVLROptiMsrtType.VLRDWDWP].Value = item.MsrtResult[(int)EVLROptiMsrtType.VLRWLD].Value - item.MsrtResult[(int)EVLROptiMsrtType.VLRWLP].Value;
                    item.MsrtResult[(int)EVLROptiMsrtType.VLRCIEz].RawValue = 1.0d - item.MsrtResult[(int)EVLROptiMsrtType.VLRCIEx].RawValue - item.MsrtResult[(int)EVLROptiMsrtType.VLRCIEy].RawValue;
                    item.MsrtResult[(int)EVLROptiMsrtType.VLRCIEz].Value = 1.0d - item.MsrtResult[(int)EVLROptiMsrtType.VLRCIEx].Value - item.MsrtResult[(int)EVLROptiMsrtType.VLRCIEy].Value;

                    //==================================
                    // Paul, 20130221
                    // 新增電功率(EWATT), 發光效率 lm/EWATT (LE) , 發光效率 watt/EWATT (WPE) ;
                    //==================================
                    item.MsrtResult[(int)EVLROptiMsrtType.VLRLE].RawValue = item.MsrtResult[(int)EVLROptiMsrtType.VLRLM].RawValue / item.MsrtResult[(int)EVLROptiMsrtType.VLREWATT].RawValue;
                    item.MsrtResult[(int)EVLROptiMsrtType.VLRLE].Value = item.MsrtResult[(int)EVLROptiMsrtType.VLRLM].Value / item.MsrtResult[(int)EVLROptiMsrtType.VLREWATT].Value;

                    item.MsrtResult[(int)EVLROptiMsrtType.VLRWPE].RawValue = (0.001 * item.MsrtResult[(int)EVLROptiMsrtType.VLRWATT].RawValue) / item.MsrtResult[(int)EVLROptiMsrtType.VLREWATT].RawValue;
                    item.MsrtResult[(int)EVLROptiMsrtType.VLRWPE].Value = (0.001 * item.MsrtResult[(int)EVLROptiMsrtType.VLRWATT].Value) / item.MsrtResult[(int)EVLROptiMsrtType.VLREWATT].Value;

                    // this._sysResultItem[(int)ESysResultItem.SEQUENCETIME].Value = this._DataDR[(int)EDataDR.TestTime] - (item.MsrtResult[(int)EVLROptiMsrtType.VLRST].RawValue + 6 + conditionTime);

                    //==================================
                    // Stanley, 20130313
                    // 新增CCT計算，分為:(1)由校正前CIExy計算出CCT，再做CCT校正 (2)直接由校正後CIExy計算出CCT後不再做CCT校正。
                    //==================================
                    if (this._sysSetting.OptiDevSetting.IsCalcCCTByCaliCIExy)
                    {
                        double cieX = item.MsrtResult[(int)EVLROptiMsrtType.VLRCIEx].Value;

                        double cieY = item.MsrtResult[(int)EVLROptiMsrtType.VLRCIEy].Value;


                        if (this._sysSetting.CCTcaculationType == ECCTCaculationType.McCamy)
                        {
                            item.MsrtResult[(int)EVLROptiMsrtType.VLRCCT].Value = MpiSPAM.ComputeCorrelatedColorTemperature(cieX, cieY);
                        }
                        else
                        {
                            item.MsrtResult[(int)EVLROptiMsrtType.VLRCCT].Value = CommonCCTCalculate.Robertson31PointsMethod(cieX, cieY);
                        }
                    }

                    LOPWLIndex++;

                    #endregion
                }
                else if (item is LIVTestItem)
                {
                    #region >>> LIV Test Item <<<

                    if (item.IsEnable)
                    {
                        int count = (item as LIVTestItem).DataLength;

                        for (int i = 0; i < count; i++)
                        {
                            calcAbsIntensity = this._sptMeter.GetYAbsoluateSpectrum(LOPWLIndex + (uint)i);

                            Array.Copy(calcAbsIntensity, 0, this._acquireData.LIVDataSet[item.KeyName].SpectrumDataData[(int)i].Absoluate, 0, this._acquireData.LIVDataSet[item.KeyName].SpectrumDataData[(int)i].Absoluate.Length);

                            this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVDWDWP].DataArray[i] = this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVWLD].DataArray[i] - this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVWLP].DataArray[i];

                            //item.MsrtResult[(int)ELIVOptiMsrtType.LIVCIEz].DataList.Add(1.0f - item.MsrtResult[(int)ELIVOptiMsrtType.LIVCIEx].DataList[i] - item.MsrtResult[(int)ELIVOptiMsrtType.LIVCIEy].DataList[i]);
                            //item.MsrtResult[(int)ELIVOptiMsrtType.LIVMVFLD].DataList.Add(item.MsrtResult[(int)ELIVOptiMsrtType.LIVMVFLB].DataList[i] - item.MsrtResult[(int)ELIVOptiMsrtType.LIVMVFLA].DataList[i]);

                            this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVEWATT].DataArray[i] = this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVMsrtI].DataArray[i] * this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVMsrtV].DataArray[i];

                            this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVLE].DataArray[i] = this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVLM].DataArray[i] / this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVEWATT].DataArray[i];

                            this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVWPE].DataArray[i] = 0.001f * this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVWATT].DataArray[i] / this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVEWATT].DataArray[i];
                        }

                        item.CalibrateLIVSweepData(this._acquireData.LIVDataSet[item.KeyName]);

                        if (count > 1)
                        {
                            double watttd = this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVWATT].DataArray[count - 1] - this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVWATT].DataArray[0];

                            double lmtd = this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVLM].DataArray[count - 1] - this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVLM].DataArray[0];

                            item.MsrtResult[(int)ELIVOptiMsrtType.LIVWATTTD].Value = watttd;
                            item.MsrtResult[(int)ELIVOptiMsrtType.LIVWATTTD].RawValue = watttd;
                            item.MsrtResult[(int)ELIVOptiMsrtType.LIVLMTD].Value = lmtd;
                            item.MsrtResult[(int)ELIVOptiMsrtType.LIVLMTD].RawValue = lmtd;
                        }
                    }

                    LOPWLIndex += (uint)(item as LIVTestItem).OptiSettingList.Count;

                    #endregion
                }
                else if (item is TransistorTestItem)
                {
                    #region >>> Transistor Test Item <<<

                    if (item.IsEnable)
                    {
                        int count = (item as TransistorTestItem).DataLength;

                        for (int i = 0; i < count; i++)
                        {
                            calcAbsIntensity = this._sptMeter.GetYAbsoluateSpectrum(LOPWLIndex + (uint)i);

                            Array.Copy(calcAbsIntensity, 0, this._acquireData.LIVDataSet[item.KeyName].SpectrumDataData[(int)i].Absoluate, 0, this._acquireData.LIVDataSet[item.KeyName].SpectrumDataData[(int)i].Absoluate.Length);

                            this._acquireData.LIVDataSet[item.KeyName][ETransistorOptiMsrtType.TRDWDWP].DataArray[i] = this._acquireData.LIVDataSet[item.KeyName][ETransistorOptiMsrtType.TRWLD].DataArray[i] - this._acquireData.LIVDataSet[item.KeyName][ETransistorOptiMsrtType.TRWLP].DataArray[i];

                            //item.MsrtResult[(int)ELIVOptiMsrtType.TRCIEz].DataList.Add(1.0f - item.MsrtResult[(int)ELIVOptiMsrtType.TRCIEx].DataList[i] - item.MsrtResult[(int)ELIVOptiMsrtType.TRCIEy].DataList[i]);
                            //item.MsrtResult[(int)ELIVOptiMsrtType.TRMVFLD].DataList.Add(item.MsrtResult[(int)ELIVOptiMsrtType.TRMVFLB].DataList[i] - item.MsrtResult[(int)ELIVOptiMsrtType.TRMVFLA].DataList[i]);

                            this._acquireData.LIVDataSet[item.KeyName][ETransistorOptiMsrtType.TRDrainEWATT].DataArray[i] = this._acquireData.LIVDataSet[item.KeyName][ETransistorOptiMsrtType.TRMsrtDrainI].DataArray[i] * this._acquireData.LIVDataSet[item.KeyName][ETransistorOptiMsrtType.TRMsrtDrainV].DataArray[i];

                            this._acquireData.LIVDataSet[item.KeyName][ETransistorOptiMsrtType.TRLE].DataArray[i] = this._acquireData.LIVDataSet[item.KeyName][ETransistorOptiMsrtType.TRLM].DataArray[i] / this._acquireData.LIVDataSet[item.KeyName][ETransistorOptiMsrtType.TRDrainEWATT].DataArray[i];

                            this._acquireData.LIVDataSet[item.KeyName][ETransistorOptiMsrtType.TRWPE].DataArray[i] = 0.001f * this._acquireData.LIVDataSet[item.KeyName][ETransistorOptiMsrtType.TRWATT].DataArray[i] / this._acquireData.LIVDataSet[item.KeyName][ETransistorOptiMsrtType.TRDrainEWATT].DataArray[i];
                        }

                        item.CalibrateLIVSweepData(this._acquireData.LIVDataSet[item.KeyName]);

                        if (count > 1)
                        {
                            double watttd = this._acquireData.LIVDataSet[item.KeyName][ETransistorOptiMsrtType.TRWATT].DataArray[count - 1] - this._acquireData.LIVDataSet[item.KeyName][ETransistorOptiMsrtType.TRWATT].DataArray[0];

                            double lmtd = this._acquireData.LIVDataSet[item.KeyName][ETransistorOptiMsrtType.TRLM].DataArray[count - 1] - this._acquireData.LIVDataSet[item.KeyName][ETransistorOptiMsrtType.TRLM].DataArray[0];

                            item.MsrtResult[(int)ETransistorOptiMsrtType.TRWATTTD].Value = watttd;
                            item.MsrtResult[(int)ETransistorOptiMsrtType.TRWATTTD].RawValue = watttd;
                            item.MsrtResult[(int)ETransistorOptiMsrtType.TRLMTD].Value = lmtd;
                            item.MsrtResult[(int)ETransistorOptiMsrtType.TRLMTD].RawValue = lmtd;
                        }
                    }

                    LOPWLIndex += (uint)(item as TransistorTestItem).OptiSettingList.Count;

                    #endregion
                }
            }

            GetTestGroupStr();//20180126 David

            this._condCtrl.CalibCalcTestItem();

            if (!GlobalFlag.IsReSingleTestMode)
            {
                this.CalcBinGrade();
            }
            this.CheckAllDeviceErrorState();


            //   this._sysResultItem[(int)ESysResultItem.DIETESTSTATE].Value = this._DataIR[(int)EDataIR.DieTestState];

            // Fire event
            this._isMoveDataToStorage = false;

            Fire_FinishTestAndCalcEvent(null);

            return;
        }

        public override bool RunCommand(int command)
        {
            bool rtn = false;

            if (this._status.State == EKernelState.Not_Ready && command != (int)ETesterKernelCmd.SimulatorRun)
            {
                this.SetErrorCode(EErrorCode.System_Not_Ready);
                return false;
            }
            else if (command == (int)ETesterKernelCmd.SimulatorRun)
            {
                this.SimulatorRun();
                return true;
            }

            this._status.State = EKernelState.Running;

            lock (this._lockObj)
            {
                switch (command)
                {
                    case (int)ETesterKernelCmd.RunTest:

                        this._sysSetting.IsEnableDarkCorrect = true;

                        rtn = this.RunTestSequence();

                        if (_isTriggerSptErr == true)
                        {
                            this.ChangeRowColCoord();

                            this._darkCorrectCount--;

                            System.Threading.Thread.Sleep((int)this._sysSetting.OptiDevSetting.Limit02TurnOffTime);

                            rtn = this.RunTestSequence();

                            this._repeatTestIndexs.Add((int)this._sysResultItem[(int)ESysResultItem.TEST].Value);

                            Console.WriteLine("[ Spetrometer State Check] , Die Index = " + this._sysResultItem[(int)ESysResultItem.TEST].Value.ToString() + " / " + this._repeatTestIndexs.Count.ToString());
                        }
                        this._sysResultItem[(int)ESysResultItem.TEST].Value++;

                        break;
                    //---------------------------------------------------------
                    case (int)ETesterKernelCmd.ManualRun:
                        this.ManaulRun();
                        break;
                    //---------------------------------------------------------
                    case (int)ETesterKernelCmd.ConfirmDataReceived:
                        this._isMoveDataToStorage = true;
                        rtn = true;
                        break;
                    //---------------------------------------------------------
                    case (int)ETesterKernelCmd.ResetKernelData:
                        //this.ResetKernelData();
                        break;
                    //---------------------------------------------------------
                    case (int)ETesterKernelCmd.CheckMachineHW:
                        this.CheckMachineHW();
                        break;
                    //---------------------------------------------------------
                    case (int)ETesterKernelCmd.GetDarkDataAndSave:
                        this.GetDarkDataAndSave();
                        break;
                    //---------------------------------------------------------
                    case (int)ETesterKernelCmd.ConfirmErrorMsg:
                        this.SetErrorCode(EErrorCode.NONE);

                        if (this._status.State == EKernelState.Error)
                        {
                            this._status.State = EKernelState.Ready;
                        }
                        break;
                    //---------------------------------------------------------
                    case (int)ETesterKernelCmd.ShortTestIF:
                        // this._acquireData.IR[(int)EDataIR.SortTestIFResult] = Convert.ToInt32(this.OpenShortIFTest(true, true));
                        break;
                    //---------------------------------------------------------
                    case (int)ETesterKernelCmd.OpenTestIF:
                        //  this._acquireData.IR[(int)EDataIR.OpenTestIFResult] = Convert.ToInt32(this.OpenShortIFTest(true, false));
                        break;
                    //---------------------------------------------------------
                    case (int)ETesterKernelCmd.AbortOpenShortTestIF:
                        this.OpenShortIFTest(false, true);
                        break;
                    //---------------------------------------------------------
                    case (int)ETesterKernelCmd.EndTest:
                        this.EndTestSequence();
                        break;
                    //---------------------------------------------------------
                    case (int)ETesterKernelCmd.ResetMachineHW:
                        this.ResetMachineHW();
                        break;
                    //---------------------------------------------------------
                    case (int)ETesterKernelCmd.RunSingleRetest:
                        this.RunSingleRetest();
                        break;
                    //---------------------------------------------------------
                    case (int)ETesterKernelCmd.GetPDDarkCurrent:
                        this.GetPDDarkCurrent();
                        break;
                    //---------------------------------------------------------
                    case (int)ETesterKernelCmd.RunLcrCalibration:

                        ELCRCaliMode mode;

                        if (Enum.TryParse<ELCRCaliMode>(this._cmdData.IntData[0].ToString(), out mode))
                        {
                            this.RunLcrCalibration(mode);
                        }

                        break;
                    //---------------------------------------------------------
                    case (int)ETesterKernelCmd.RunOsaCoupling:

                        if (this._cmdData.IntData[0] == 1)
                        {
                            if (this._osaDevice is MPI.Tester.Device.OSA.MS9740A)
                            {
                                (this._osaDevice as MPI.Tester.Device.OSA.MS9740A).SweepRepeat();
                            }
                        }
                        else
                        {
                            if (this._osaDevice is MPI.Tester.Device.OSA.MS9740A)
                            {
                                (this._osaDevice as MPI.Tester.Device.OSA.MS9740A).SweepStop();
                            }
                        }

                        break;
                    //---------------------------------------------------------
                    case (int)ETesterKernelCmd.RunSrcOutput:

                        if (this._cmdData.IntData[0] == 1)
                        {
                            double srcI = this._cmdData.DoubleData[0];
                            double msrtV = this._cmdData.DoubleData[1];

                            //(this._srcMeter as Keithley2600).BiasI(srcI, msrtV);
                        }
                        else
                        {
                            if (this._srcMeter is Keithley2600)
                            {
                                (this._srcMeter as Keithley2600).TurnOff();
                                //(this._srcMeter as Keithley2600).OutputOff();
                            }
                        }

                        break;
                    //---------------------------------------------------------
                    case (int)ETesterKernelCmd.RunAttenuator:

                        switch (this._cmdData.IntData[0])
                        {
                            case (int)ELaserSourceSysAction.ATTENUATOR_MSRT:
                                {
                                    RunAttenuatorMsrt();
                                }
                                break;
                             //case (int)ELaserSourceSysAction.MoniterPD_MSRT:
                             //   {
                             //       int ft = (int)this._cmdData.DoubleData[0];//ms
                             //       double bias =this._cmdData.DoubleData[1];//v
                             //       double clamp =this._cmdData.DoubleData[2];//A
                             //       //RunPdMonitorMsrt(ft, bias, clamp);
                             //   }
                             //   break;
                        }

                        break;
                    //---------------------------------------------------------
                    default:
                        break;
                }

                this._status.State = EKernelState.Ready;
                rtn &= this.CheckAllDeviceErrorState();

                return rtn;
            }

        }

        #endregion
    }

    
}
