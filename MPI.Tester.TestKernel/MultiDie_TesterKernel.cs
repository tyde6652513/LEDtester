using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;


using MPI.Tester.Data;
using MPI.Tester.CompoCommon;
using MPI.Tester.Compo.DIDOCard;
using MPI.Tester.Compo.ADCard;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Device.SourceMeter;
using MPI.Tester.Device.SpectroMeter;
using MPI.Tester.Device.ESD;
using MPI.Tester.Device.SwitchSystem;
using MPI.Tester.Device.IOCard;
using MPI.Tester.Maths.ColorMath;
using MPI.Tester.Device.PostCalc;
using MPI.Tester.Maths;
using MPI.Tester.Tools;
using MPI.Tester.Data.ChannelCoordTable;

namespace MPI.Tester.TestKernel
{

    public class MultiDie_TesterKernel : TesterKernelBase
    {

        private MPI.PerformanceTimer _pt;
        private MPI.PerformanceTimer _ptTest;

        private double time = 0;

        

        private bool[] _isChannelStopTest;

        private int[] _chipProbeBin;

        private int _channelArrangeRotateTheta;

        private bool[] _isChannelOpenShortFail;

        private bool[] _isOnlySkipIzTestItem;


        //private ILaserPostCalc _laserPostCalc;

        

        private LevelShiftTable<int> _basePosition = new LevelShiftTable<int>();


        public MultiDie_TesterKernel()
            : base()
        {
            this._lockObj = new object();

            this._acquireData = new AcquireData();

            this._machineInfo = new MachineInfoData();

            this._isTestSuccess = false;

            string[] strArray = Enum.GetNames(typeof(ESysResultItem));

            this._sysResultItem = new TestResultData[strArray.Length];

            for (int i = 0; i < this._sysResultItem.Length; i++)
            {
                this._sysResultItem[i] = new TestResultData(strArray[i], strArray[i], "", "0");
            }

            this._pt1 = new PerformanceTimer();
            this._pt = new PerformanceTimer();
            this._ptOpenShortTimeOut = new PerformanceTimer();
            _ptTest = new PerformanceTimer();

            this._chipPolarity = 1.0d;

            this._testSequencElecCount = 0;
            this._testSquenceOptCount = 0;

            this._binCalcData = new Dictionary<string, double>(50);
            this._skipGetDarkCounts = 200;
            this._adjacentCheck = new MultiDieAdjacentCheck();
            this._dataVerify = new DataVerify();
            this._isTriggerSptErr = false;
            this._repeatTestIndexs = new List<int>();
            this._skipCount = 0;

            //this._rdFunc = new RDFunc();

            this._srcSyncTrigger = new List<uint>();

            this._channelArrangeRotateTheta = 0;

            this._openShortContactItem = null;

            this._passRateCheck = new PassRateCheck();

            this._isQCSetting = false;

            this._adjacentCheck = new MultiDieAdjacentCheck();

        }

        #region >>> Private Methods <<<

        

        private void GetOptiMsrtResult(uint index, TestItemData testItem)
        {
            if (this._sptMeter == null)
                return;

            if (!testItem.IsTested)
                return;

            if (testItem is LOPWLTestItem)
            {
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

                if (testItem.MsrtResult != null && this._isChannelStopTest[this._acquireData.ChipInfo.Channel])
                {
                    if (testItem.IsTested)
                    {

                    }
                    else
                    {
                        foreach (TestResultData data in testItem.MsrtResult)
                        {
                            if (data.KeyName.Contains("MVFL"))
                            {
                                continue;
                            }

                            data.RawValue = 0.0d;
                            data.Value = 0.0d;
                        }
                    }
                }
            }
            else if (testItem is LIVTestItem)
            {
                if (testItem.IsEnable)
                {
                    for (int i = 0; i < (testItem as LIVTestItem).OptiSettingList.Count; i++)
                    {
                        LIVData livTestItemData = this._acquireData.LIVDataSet[testItem.KeyName];

                        if (this._sptMeter.CalculateParameters(index + (uint)i) && !this._isStopTest)
                        {
                            livTestItemData[ELIVOptiMsrtType.LIVCCT].DataArray[i] = 0;

                            livTestItemData[ELIVOptiMsrtType.LIVLOP].DataArray[i] = (float)this._sptMeter.Data[index + i].Lx;

                            livTestItemData[ELIVOptiMsrtType.LIVWATT].DataArray[i] = (float)this._sptMeter.Data[index + i].Watt;

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

                            livTestItemData[ELIVOptiMsrtType.LIVR01].DataArray[i] = (float)this._sptMeter.Data[index].SpecialCRI[0];

                            livTestItemData[ELIVOptiMsrtType.LIVR02].DataArray[i] = (float)this._sptMeter.Data[index].SpecialCRI[1];

                            livTestItemData[ELIVOptiMsrtType.LIVR03].DataArray[i] = (float)this._sptMeter.Data[index].SpecialCRI[2];

                            livTestItemData[ELIVOptiMsrtType.LIVR04].DataArray[i] = (float)this._sptMeter.Data[index].SpecialCRI[3];

                            livTestItemData[ELIVOptiMsrtType.LIVR05].DataArray[i] = (float)this._sptMeter.Data[index].SpecialCRI[4];

                            livTestItemData[ELIVOptiMsrtType.LIVR06].DataArray[i] = (float)this._sptMeter.Data[index].SpecialCRI[5];

                            livTestItemData[ELIVOptiMsrtType.LIVR07].DataArray[i] = (float)this._sptMeter.Data[index].SpecialCRI[6];

                            livTestItemData[ELIVOptiMsrtType.LIVR08].DataArray[i] = (float)this._sptMeter.Data[index].SpecialCRI[7];

                            livTestItemData[ELIVOptiMsrtType.LIVR09].DataArray[i] = (float)this._sptMeter.Data[index].SpecialCRI[8];

                            livTestItemData[ELIVOptiMsrtType.LIVR10].DataArray[i] = (float)this._sptMeter.Data[index].SpecialCRI[9];

                            livTestItemData[ELIVOptiMsrtType.LIVR11].DataArray[i] = (float)this._sptMeter.Data[index].SpecialCRI[10];

                            livTestItemData[ELIVOptiMsrtType.LIVR12].DataArray[i] = (float)this._sptMeter.Data[index].SpecialCRI[11];

                            livTestItemData[ELIVOptiMsrtType.LIVR13].DataArray[i] = (float)this._sptMeter.Data[index].SpecialCRI[12];

                            livTestItemData[ELIVOptiMsrtType.LIVR14].DataArray[i] = (float)this._sptMeter.Data[index].SpecialCRI[13];

                            livTestItemData[ELIVOptiMsrtType.LIVR15].DataArray[i] = (float)this._sptMeter.Data[index].SpecialCRI[14];
                        }
                        else
                        {
                            foreach (var livResultData in livTestItemData)
                            {
                                livResultData.DataArray[i] = 0.0f;
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

                                basicTime = i == 0 ? basicTime : basicTime - livTestItemData[ELIVOptiMsrtType.LIVTIMEA].DataArray[i - 1];
                            }
                            else
                            {
                                basicTime = basicTime > item.LIVSamplimgTime ? basicTime : item.LIVSamplimgTime;
                            }
                        }

                        timeA = i == 0 ? basicTime : basicTime + livTestItemData[ELIVOptiMsrtType.LIVTIMEA].DataArray[i - 1];

                        livTestItemData[ELIVOptiMsrtType.LIVTIMEA].DataArray[i] = (float)timeA;

                        //(testItem as LIVTestItem).AbsoluteSpectrumAarry.Add( this._sptMeter.GetYAbsoluateSpectrum(index));

                        //for (int i = 0; i < testItem.MsrtResult.Length; i++)
                        //{
                        //    testItem.MsrtResult[i].Value = testItem.MsrtResult[i].RawValue;
                        //}

                        //if (testItem.MsrtResult != null && this._isStopTest)
                        //{
                        //    foreach (TestResultData data in testItem.MsrtResult)
                        //    {
                        //        data.RawValue = 0.0d;
                        //        data.Value = 0.0d;
                        //    }
                        //}
                    }
                }
            }
        }

        private LevelShiftTable<int> SetBiasRowColCoord()
        {
            LevelShiftTable<int> pos = new LevelShiftTable<int>();
            int x = (int)(CmdData.DoubleData[(uint)EProberDataIndex.COL]);
            int y = (int)(CmdData.DoubleData[(uint)EProberDataIndex.ROW]);
            int sx = (int)(CmdData.DoubleData[(uint)EProberDataIndex.SubCOL]);
            int sy = (int)(CmdData.DoubleData[(uint)EProberDataIndex.SubROW]);
            int gx = (int)(CmdData.DoubleData[(uint)EProberDataIndex.GroupCOL]);
            int gy = (int)(CmdData.DoubleData[(uint)EProberDataIndex.GroupROW]);
            int subDieLayer = -1;
            if (_chShiftTable != null)
            {
                foreach (var p in _chShiftTable)
                {
                    subDieLayer = p.Value.BottomLayer;
                    break;
                }

            }

            subDieLayer = subDieLayer < 0 ? subDieLayer : -1;

            pos.Push(0, x, y);
            pos.Push(subDieLayer, sx, sy);
            pos.Push(1, gx, gy);
            return pos;
        }


        private void ChangeRowColCoord(LevelShiftTable<int> basePose, ChannelPosShiftTable<int> coordTable = null)
        {
            LevelShiftTable<int> chPose = basePose.Clone() as LevelShiftTable<int>;

            if (coordTable == null)
            {
                coordTable = _chShiftTable;
            }

            uint channel = this._acquireData.ChipInfo.Channel;

            this._acquireData.ChannelResultDataSet[channel][EProberDataIndex.JustTestOneChannel.ToString()].Value =
              this._cmdData.DoubleData[(uint)EProberDataIndex.JustTestOneChannel];

            // 當Multi-Die 抬腳功能啟動後 , Row Col 都直接接收Prober 給的資訊，因為其他顆會當成無die，不會測試
            if (this._cmdData.DoubleData[(uint)EProberDataIndex.JustTestOneChannel] == 1)
            {
                channel = 1;
            }

            chPose = basePose + coordTable[(int)channel];

            //--------------------------------------------------------------
            double X = chPose.GetX(0);
            double Y = chPose.GetY(0);
            double SX = chPose.GetX(chPose.BottomLayer);
            double SY = chPose.GetY(chPose.BottomLayer);
            double GX = chPose.GetX(1); //現階段"Group比Col/Row層數還高"跟"col/Row為最上層"互相矛盾
            double GY = chPose.GetY(1);

            if (!GlobalFlag.IsGetChShiftTableFromProber)    // 有MappingTable時，Follow Prober提供的座標系，Tester不轉置象限。
            {
                CoordTransferTool coord = new CoordTransferTool(this._sysSetting.ProberCoord, this._sysSetting.TesterCoord);

                double angleInDeg = this._channelArrangeRotateTheta * 90;

                double sCol = this.CmdData.DoubleData[(uint)EProberDataIndex.TransCOL];
                double sRow = this.CmdData.DoubleData[(uint)EProberDataIndex.TransROW];
                coord.SetMatrixShift(sCol, sRow);
                coord.SetMatrixRotate(angleInDeg);

                coord.Rotate(ref X, ref Y);
                coord.Rotate(ref SX, ref SY);
                coord.Rotate(ref GX, ref GY);
            }
            //--------------------------------------------------------------

            if (this._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Die)
            {
                this._acquireData.ChipInfo.ColX = (int)X;
                this._acquireData.ChipInfo.RowY = (int)Y;
                this._acquireData.ChipInfo.SubColX = (int)SX;
                this._acquireData.ChipInfo.SubRowY = (int)SY;

                this._acquireData.ChannelResultDataSet[channel].Col = this._acquireData.ChipInfo.ColX;
                this._acquireData.ChannelResultDataSet[channel].Row = this._acquireData.ChipInfo.RowY;
                this._acquireData.ChannelResultDataSet[channel].SubCol = this._acquireData.ChipInfo.SubColX;
                this._acquireData.ChannelResultDataSet[channel].SubRow = this._acquireData.ChipInfo.SubRowY;
                this._acquireData.ChannelResultDataSet[channel].GroupCol = (int)GX;
                this._acquireData.ChannelResultDataSet[channel].GroupRow = (int)GY;
            }
        }

        private void TestProcess(bool isRest)
        {
            this._isStopTest = false;
            this._preTypeState = -1;

            //this._IOPort.Reset(0x378);
            //this._IOPort.Output(0x378, 0, true);

            if (this._condCtrl.Data.TestItemArray == null || this._condCtrl.Data.TestItemArray.Length == 0)
                return;

            this._acquireData.ChipInfo.StartTime = DateTime.Now;

            _basePosition = this.SetBiasRowColCoord();

            switch (_machineConfig.TesterFunctionType)
            {
                case ETesterFunctionType.Single_Die:
                case ETesterFunctionType.Multi_Terminal:
                    {
                        this.Module_SingleDie();

                        break;
                    }
                case ETesterFunctionType.Multi_Die:
                    {
                        this.Module_MultiDie(isRest);

                        this.SWDelayTime(3);

                        break;
                    }
                case ETesterFunctionType.Multi_Pad:
                    {
                        this.Module_MultiPad();

                        break;
                    }
                default:
                    break;
            }

            //----------------------------------------------------------
            if (this._srcMeter != null)
            {
                if (this._srcMeter is Keithley2600)
                {
                    (this._srcMeter as Keithley2600).SetDefaultIO();
                }
            }
            //----------------------------------------------------------
            // Finish each item testing,
            // Finally turn off the source meter 
            // High Speed Mode => Disable Sleep(8)
            //----------------------------------------------------------

            this._acquireData.ChipInfo.EndTime = DateTime.Now;

            this._sysResultItem[(int)ESysResultItem.TIME_SPAN].Value = this._acquireData.ChipInfo.TimeSpan.TotalMilliseconds;
            this._sysResultItem[(int)ESysResultItem.TIME_SPAN].Unit = "ms";

            this._sysResultItem[(int)ESysResultItem.TEST_START_TIME].Value = this._acquireData.ChipInfo.StartTime.Ticks;
            this._sysResultItem[(int)ESysResultItem.TEST_START_TIME].Unit = "Ticks";
            this._sysResultItem[(int)ESysResultItem.TEST_END_TIME].Value = this._acquireData.ChipInfo.EndTime.Ticks;
            this._sysResultItem[(int)ESysResultItem.TEST_START_TIME].Unit = "Ticks";
            if (this._rdFunc.RDFuncData.HighSpeedModeDelayTime > 0)
            {
                System.Threading.Thread.Sleep((int)this._rdFunc.RDFuncData.HighSpeedModeDelayTime);
            }

            //this._IOPort.Output(0x378, 5, true);
            //this._IOPort.Reset(0x378);
        }

        private void ManaulRun()
        {
            int repeatIndex = 0;

            bool rtn = false;

            _isManualTest = true;

            int shiftColX = 1;

            int shiftRowY = 1;

            if (this._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Die)
            {
                shiftColX = this._machineConfig.ChannelConfig.ColXCount;

                shiftRowY = this._machineConfig.ChannelConfig.RowYCount;

                for (int i = 0; i < this._isChannelHasDie.Length; i++)
                {
                    this._isChannelHasDie[i] = true;

                    this._chipGroup[i] = 0;
                }
            }

            while (repeatIndex < this._cmdData.IntData[1])
            {
                this._cmdData.DoubleData[(uint)EProberDataIndex.COL] = this._cmdData.IntData[0] * shiftColX;
                this._cmdData.DoubleData[(uint)EProberDataIndex.ROW] = repeatIndex * shiftRowY + 1;

                //--------------------------------------------------------------------------------
                // In manul run loop, the first chip do "DarkIntensity Correct"
                //--------------------------------------------------------------------------------
                if (repeatIndex == 0)
                {
                    this._sysSetting.IsEnableDarkCorrect = true;
                    this._darkCorrectCount = 0;

                    //--------------------------------------------------------------------
                    // 20121205 Roy, 先拿掉 Delay Time, 因會影響 ESD 電容充電
                    //--------------------------------------------------------------------
                    //this.WaitTimeOut(250);          // Delay 250ms for the first chip run test
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

                //this._sysResultItem[(int)ESysResultItem.TEST].Value++;
                //this.ResetTesterCond();					// First Step , dummy function

                if ((repeatIndex % 2) == 0)
                {
                    this._acquireData.ChipInfo.IsDieTestStatePass = false;
                }

                this.GetTestedDataFromDevice();
                repeatIndex++;

                System.Windows.Forms.Application.DoEvents();
            }

            this.EndTestSequence();

            _isManualTest = false;
        }

        private void RunDeviceVerify()
        {
            uint channel = (uint)this._cmdData.IntData[0] + 1;  // channel-0 P/C 迴路, Channel-1, 2, 3 儀器監控迴路
            int repeatCount = this._cmdData.IntData[1];
            int repeatDelay = this._cmdData.IntData[2];

            int repeatIndex = 0;
            bool rtn = false;

            if (this._switchDevice != null)
            {
                // 需先關閉 Default Channel
                this._switchDevice.DisableCH();

                this._switchDevice.EnableCH(channel);

                this._machineConfig.TesterFunctionType = ETesterFunctionType.Single_Die;

                while (repeatIndex < repeatCount)
                {
                    GlobalFlag.IsDeviceVerifyMode = true;

                    rtn = this.RunTestSequence();

                    if (rtn == false)
                    {
                        return;
                    }

                    this.GetTestedDataFromDevice();

                    System.Threading.Thread.Sleep(repeatDelay);

                    repeatIndex++;
                }

                this._switchDevice.DisableCH();

                this._machineConfig.TesterFunctionType = ETesterFunctionType.Multi_Die;

                // 再切回 Default Channel
                this._switchDevice.EnableCH(0);
            }
        }

        private bool Module_SingleDie()
        {
            TestItemData item = null;
            uint testItemIndex = 0;

            this._sysResultItem[(int)ESysResultItem.TEST].Value++;

            this.ChangeRowColCoord(_basePosition);

            //--------------------------------------------------------------------
            // (1) Get current dark intensity data for each 200 chips
            //--------------------------------------------------------------------
            this.GetDarkIntensityData();

            this._acquireData.ChipInfo.IsPass = false;

            for (testItemIndex = 0; testItemIndex < this._condCtrl.Data.TestItemArray.Length; testItemIndex++)
            {
                if (this._isStopTest)
                {
                    break;
                }

                item = this._condCtrl.Data.TestItemArray[testItemIndex];

                this.RunTestItem(item);

                if (item.Type == ETestType.ESD)
                {
                    // ESD has already finished the follow step
                    continue;
                }

                this.CalibrateResistance(item);

                this.SetMeasuredRawValueToValue(item);

                this.CalibrateMeasureResult(item);

                this._isStopTest = this.CheckStateIsSkip(item);

                // suppend this thread to allow other waiting threads to execute
                System.Threading.Thread.Sleep(0);
            }

            if (this._isStopTest)
            {
                this.ResetItemMsrtResultValueToZero(this._condCtrl.Data.TestItemArray, testItemIndex);
            }

            if (this._srcMeter != null)
            {
                this._srcMeter.TurnOff();
            }

            return true;
        }

        private bool Module_MultiDie(bool isReTest)
        {
            TestItemData item = null;
            uint testItemIndex = 0;

            // Reset Status to non-test
            for (uint channel = 0; channel < this._machineConfig.ChannelConfig.ChannelCount; channel++)
            {
                for (testItemIndex = 0; testItemIndex < this._condCtrl.Data.TestItemArray.Length; testItemIndex++)
                {
                    item = this._condCtrl.Data.ChannelConditionTable.Channels[channel].Conditions[(int)testItemIndex];

                    item.IsTested = false;

                    if (item.MsrtResult != null)
                    {
                        foreach (var result in item.MsrtResult)
                        {
                            result.IsTested = false;
                        }
                    }
                }
            }

            switch (this._machineConfig.ChannelConfig.TesterSequenceType)
            {
                case ETesterSequenceType.Series:
                    {
                        #region >>> Series Type <<<
                        // Outer Loop : DutChannel
                        // Inner Loop : TestItem
                        this._acquireData.IsGetOpticalSamplingSource = false;
                        this._acquireData.OpticalSamplingSourceChannel = 0;

                        for (uint seqOrder = 0; seqOrder < this._machineConfig.ChannelConfig.ChannelCount; seqOrder++)
                        {
                            this._acquireData.ChipInfo.IsPass = true;

                            uint channel = this._condCtrl.Data.ChannelConditionTable.GetChannelByOrder(seqOrder);

                            this.ResetItemMsrtResultValueToZero(this._condCtrl.Data.ChannelConditionTable.Channels[channel].Conditions.ToArray(), testItemIndex);

                            this._acquireData.ChannelResultDataSet[channel].IsPass = false;

                            this._isChannelStopTest[channel] = false;

                            this._isChannelOpenShortFail[channel] = false;

                            this._acquireData.ChipInfo.Channel = channel;

                            this.ChangeRowColCoord(_basePosition);

                            // 判斷Channel是否有Die &  Channel是否啟動測試
                            if (!this._isChannelHasDie[channel] || !this._condCtrl.Data.ChannelConditionTable.Channels[channel].IsEnable)
                            {
                                this._acquireData.ChannelResultDataSet[channel].IsTested = false;

                                continue;
                            }

                            //if (this._isChannelHasDie[channel] &&
                            //            //item.IsEnable &&
                            //            this._condCtrl.Data.ChannelConditionTable.Channels[channel].IsEnable &&
                            //            !this._isChannelStopTest[channel])
                            //{

                            //    this._srcSyncTrigger.Clear();

                            //    this._srcSyncTrigger.Add(channel);
                            //}

                            if (!GlobalFlag.IsReSingleTestMode)
                            {
                                //this._acquireData.ChannelResultDataSet[channel].IsTested = true;

                                this._sysResultItem[(int)ESysResultItem.TEST].Value++;
                            }

                            this._acquireData.ChannelResultDataSet[channel].IsTested = true;

                            //this._sysResultItem[(int)ESysResultItem.TEST].Value++;

                            this._acquireData.ChannelResultDataSet[channel]["TEST"].Value = this._sysResultItem[(int)ESysResultItem.TEST].Value;

                            this.GetDarkIntensityData();

                            // Switch Box DUT-CH 開啟
                            if (this._switchDevice != null)
                            {
                                //this._switchDevice.EnableCH(this._machineConfig.ChannelConfig.AssignmentTable[(int)channel].);
                            }

                            // Run open/short Test
                            if (this._openShortContactItem != null)
                            {
                                bool[] isStopTest = this.RunOpenShortTestItem();

                                this._isChannelOpenShortFail[channel] = isStopTest[0];

                                this._isChannelStopTest[channel] = isStopTest[0];
                            }

                            for (testItemIndex = 0; testItemIndex < this._condCtrl.Data.TestItemArray.Length; testItemIndex++)
                            {
                                if (this._isChannelStopTest[channel])
                                {
                                    break;
                                }

                                item = this._condCtrl.Data.ChannelConditionTable.Channels[channel].Conditions[(int)testItemIndex];

                                item.IsTested = true;

                                if (this.IsGroupSkip(item, (int)channel))
                                {
                                    continue;
                                }

                                if (item.MsrtResult != null)
                                {
                                    foreach (var result in item.MsrtResult)
                                    {
                                        result.IsTested = true;
                                    }
                                }

                                if (this._isChannelHasDie[channel] &&
                                    item.IsEnable &&
                                        this._condCtrl.Data.ChannelConditionTable.Channels[channel].IsEnable &&
                                        !this._isChannelStopTest[channel])
                                {

                                    this._srcSyncTrigger.Clear();
                                
                                    this._srcSyncTrigger.Add(channel);
                                }
                                

                                this.RunTestItem(item);

                                if (item.Type == ETestType.ESD)
                                {
                                    // ESD has already finished the follow step
                                    continue;
                                }

                                this.SetMeasuredRawValueToValue(item);

                                this.CalibrateMeasureResult(item);

                                this._isChannelStopTest[channel] = this.CheckStateIsSkip(item);

                                // suppend this thread to allow other waiting threads to execute
                                System.Threading.Thread.Sleep(0);
                            }

                            // 啟動光複製機制
                            if (_sysSetting.IsEnableMultiDieOpticalSamplingTest)
                            {
                                if (this._acquireData.ChipInfo.IsPass)
                                {
                                    this._acquireData.IsGetOpticalSamplingSource = true;
                                }
                            }

                            if (this._srcMeter != null)
                            {
                                this._srcMeter.TurnOff();
                            }

                            // Switch Box DUT-CH 關閉
                            if (this._switchDevice != null)
                            {
                                this._switchDevice.DisableCH();  // Roy, Switch Box Channel Off
                            }

                            if (this._rdFunc.RDFuncData.MDSeriesTypeDelayTime > 0)
                            {
                                this.SWDelayTime(this._rdFunc.RDFuncData.MDSeriesTypeDelayTime);
                            }

                            this._acquireData.ChannelResultDataSet[channel].IsPass = this._acquireData.ChipInfo.IsPass;
                        }

                        //  Fill in the fake value to test item which open/short skip

                        this.ResetItemMsrtResultByOpenShortCheck();

                        #endregion

                        break;
                    }
                case ETesterSequenceType.Parallel:
                    {
                        #region >>> Parallel Type <<<

                        // 測試前資料處理
                        for (uint channel = 0; channel < this._machineConfig.ChannelConfig.ChannelCount; channel++)
                        {
                            this._acquireData.ChannelResultDataSet[channel].IsPass = false;

                            this._isChannelStopTest[channel] = false;

                            this._acquireData.ChipInfo.Channel = (uint)channel;

                            this.ResetItemMsrtResultValueToZero(this._condCtrl.Data.ChannelConditionTable.Channels[channel].Conditions.ToArray(), 0);

                            this.ChangeRowColCoord(_basePosition);

                            if (!this._isChannelHasDie[channel] || !this._condCtrl.Data.ChannelConditionTable.Channels[channel].IsEnable)
                            {
                                this._acquireData.ChannelResultDataSet[channel].IsTested = false;
                                continue;
                            }


                            this._acquireData.ChannelResultDataSet[channel].IsTested = true;

                            if (!GlobalFlag.IsReSingleTestMode && !isReTest)
                            {
                                // this._acquireData.ChannelResultDataSet[channel].IsTested = true;

                                this._sysResultItem[(int)ESysResultItem.TEST].Value++;

                                this._acquireData.ChannelResultDataSet[channel]["TEST"].Value = this._sysResultItem[(int)ESysResultItem.TEST].Value;
                            }


                            //this._sysResultItem[(int)ESysResultItem.TEST].Value++;

                            //this._acquireData.ChannelResultDataSet[channel]["TEST"].Value = this._sysResultItem[(int)ESysResultItem.TEST].Value;

                            this.GetDarkIntensityData();

                            if (this._rdFunc.RDFuncData.MDParallelTypeDelayTime > 0)
                            {
                                System.Threading.Thread.Sleep((int)this._rdFunc.RDFuncData.MDParallelTypeDelayTime);
                            }
                        }
                        /// =======================================


                        if (this._openShortContactItem != null)
                        {
                            this._isChannelOpenShortFail = this.RunOpenShortTestItem();

                            this._isChannelStopTest = this._isChannelOpenShortFail;
                        }

                        // Outer Loop : TestItem
                        // Inner Loop : DutChannel
                        for (testItemIndex = 0; testItemIndex < this._condCtrl.Data.TestItemArray.Length; testItemIndex++)
                        {
                            item = this._condCtrl.Data.TestItemArray[testItemIndex];

                            if (item is LOPWLTestItem || item is LOPTestItem || item is PIVTestItem || item is LOPTestItem || item is LIVTestItem)
                            {
                                // Optical Test Item, channel by channel test
                                for (uint channel = 0; channel < this._machineConfig.ChannelConfig.ChannelCount; channel++)
                                {
                                    item = this._condCtrl.Data.ChannelConditionTable.Channels[channel].Conditions[(int)testItemIndex];

                                    if (this.IsGroupSkip(item, (int)channel))
                                    {
                                        continue;
                                    }

                                    if (this._isChannelHasDie[channel] &&
                                        item.IsEnable &&
                                        this._condCtrl.Data.ChannelConditionTable.Channels[channel].IsEnable &&
                                        this._condCtrl.Data.ChannelConditionTable.Channels[channel].Conditions[(int)testItemIndex].IsEnable &&
                                        !this._isChannelStopTest[channel])
                                    {

                                        this._srcSyncTrigger.Clear();

                                        this._srcSyncTrigger.Add(channel);

                                        this._acquireData.ChipInfo.Channel = channel;

                                        this.RunTestItem(item);

                                        item.IsTested = true; // When complete the item test, change to status flag is true.

                                        if (item.MsrtResult != null)
                                        {
                                            foreach (var result in item.MsrtResult)
                                            {
                                                result.IsTested = true;
                                            }
                                        }

                                        this.SetMeasuredRawValueToValue(item);

                                        this.CalibrateMeasureResult(item);

                                        this._isChannelStopTest[channel] = this.CheckStateIsSkip(item);

                                        System.Threading.Thread.Sleep(1);
                                    }
                                }
                            }
                            else
                            {
                                // 判斷哪幾個smu是要進行測試的

                                this._srcSyncTrigger.Clear();

                                for (uint channel = 0; channel < this._machineConfig.ChannelConfig.ChannelCount; channel++)
                                {
                                    if (this._isChannelHasDie[channel] &&
                                        item.IsEnable &&
                                        this._condCtrl.Data.ChannelConditionTable.Channels[channel].IsEnable &&
                                        this._condCtrl.Data.ChannelConditionTable.Channels[channel].Conditions[(int)testItemIndex].IsEnable &&
                                        !this._isChannelStopTest[channel] &&
                                        !this.IsGroupSkip(item, (int)channel))
                                    {
                                        this._srcSyncTrigger.Add(channel);
                                    }
                                }

                                // 當使用者決定只ByPass IZ Test Item

                                if (this._openShortContactItem != null)
                                {
                                    if (item is IZTestItem)
                                    {
                                        for (uint channel = 0; channel < this._machineConfig.ChannelConfig.ChannelCount; channel++)
                                        {
                                            if (this._isOnlySkipIzTestItem[channel]) // Stop ==true -->SrcSynTrigger不add
                                            {
                                                this._srcSyncTrigger.Remove(channel);
                                            }
                                        }
                                    }
                                }

                                //===========================

                                if (this._srcSyncTrigger.Count > 0)
                                {
                                    this.RunTestItem(item);

                                    foreach (uint channel in this._srcSyncTrigger)
                                    {
                                        item = this._condCtrl.Data.ChannelConditionTable.Channels[channel].Conditions[(int)testItemIndex];

                                        item.IsTested = true; // When complete the item test, change to status flag is true.

                                        if (item.MsrtResult != null)
                                        {
                                            foreach (var result in item.MsrtResult)
                                            {
                                                result.IsTested = true;
                                            }
                                        }

                                        this._acquireData.ChipInfo.Channel = channel;

                                        this.SetMeasuredRawValueToValue(item);

                                        this.CalibrateMeasureResult(item);

                                        this._isChannelStopTest[channel] = this.CheckStateIsSkip(item);
                                    }
                                }
                            }
                        }

                        if (this._srcMeter != null)
                        {
                            this._srcMeter.TurnOff();
                        }

                        for (uint channel = 0; channel < this._machineConfig.ChannelConfig.ChannelCount; channel++)
                        {
                            this.ResetItemMsrtResultValueToZero(this._condCtrl.Data.ChannelConditionTable.Channels[channel].Conditions.ToArray(), 0, true);
                        }

                        //  Fill in the fake value to test item which open/short skip

                        this.ResetItemMsrtResultByOpenShortCheck();

                        #endregion

                        break;
                    }
                default:
                    return false;
            }

            return true;
        }

        private bool Module_MultiPad()
        {
            return true;
        }

        private void RunTestItem(TestItemData item)
        {
            bool isEsdSkip = false;

            uint srcChannel = 0;

            uint dutChannel = this._acquireData.ChipInfo.Channel;

            uint srcMeterItemIndex = 0;

            uint sptMeterItemIndex = 0;

            int esdItemIndex = 0;

            double[] sourceMeterReadData;

            switch (item.Type)
            {

                case ETestType.POLAR:
                    srcMeterItemIndex = item.ElecSetting[0].Order;
                    sourceMeterReadData = POLAR(item, srcMeterItemIndex);
                    break;
                //--------------------------------------------------------------------------------------------------------------
                case ETestType.IFH:
                    srcMeterItemIndex = item.ElecSetting[0].Order;
                    sourceMeterReadData = IFH(item, srcMeterItemIndex);
                    break;
                //--------------------------------------------------------------------------------------------------------------
                case ETestType.IF:
                    srcMeterItemIndex = item.ElecSetting[0].Order;
                    sourceMeterReadData = IF(item, srcMeterItemIndex);
                    break;
                //--------------------------------------------------------------------------------------------------------------
                case ETestType.IZ:
                    srcMeterItemIndex = item.ElecSetting[0].Order;
                    sourceMeterReadData = IZ(item, srcMeterItemIndex);
                    break;
                //--------------------------------------------------------------------------------------------------------------
                case ETestType.VF:
                    srcMeterItemIndex = item.ElecSetting[0].Order;
                    sourceMeterReadData = VF(item, srcMeterItemIndex);

                    break;
                //--------------------------------------------------------------------------------------------------------------
                case ETestType.VR:
                    srcMeterItemIndex = item.ElecSetting[0].Order;
                    sourceMeterReadData = VR(item, srcMeterItemIndex);
                    break;
                //--------------------------------------------------------------------------------------------------------------
                //case ETestType.IVSWEEP:
                //    srcMeterItemIndex = item.ElecSetting[0].Order;
                //    sourceMeterReadData = IVSweep(item, srcMeterItemIndex);
                //    break;
                ////--------------------------------------------------------------------------------------------------------------
                //case ETestType.VISWEEP:
                //    srcMeterItemIndex = item.ElecSetting[0].Order;
                //    sourceMeterReadData = VISweep(item, srcMeterItemIndex);
                //    break;
                //--------------------------------------------------------------------------------------------------------------
                case ETestType.THY:
                    srcMeterItemIndex = item.ElecSetting[0].Order;
                    sourceMeterReadData = THY(item, srcMeterItemIndex);
                    break;
                //--------------------------------------------------------------------------------------------------------------
                case ETestType.DVF:
                    srcMeterItemIndex = item.ElecSetting[0].Order;
                    sourceMeterReadData = DVF(item, srcMeterItemIndex);
                    break;
                //--------------------------------------------------------------------------------------------------------------
                case ETestType.LOP:
                    srcMeterItemIndex = item.ElecSetting[0].Order;
                    sourceMeterReadData = LOP(item, srcMeterItemIndex);
                    break;
                //--------------------------------------------------------------------------------------------------------------
                case ETestType.LOPWL:
                    srcMeterItemIndex = item.ElecSetting[0].Order;
                    sptMeterItemIndex = 0;
                    sourceMeterReadData = LOPWL(item, srcMeterItemIndex, sptMeterItemIndex);
                    break;
                //--------------------------------------------------------------------------------------------------------------
                case ETestType.ESD:
                    srcMeterItemIndex = item.ElecSetting[0].Order;
                    esdItemIndex = (int)(item as ESDTestItem).EsdSetting.Order;
                    sourceMeterReadData = ESD(item, srcMeterItemIndex, esdItemIndex);

                    break;               
                //--------------------------------------------------------------------------------------------------------------
                case ETestType.RTH:
                    srcMeterItemIndex = item.ElecSetting[0].Order;
                    sourceMeterReadData = RTH(item, srcMeterItemIndex);
                    break;
                //--------------------------------------------------------------------------------------------------------------               
                case ETestType.IO:
                    List<uint> TrigList = new List<uint>(new uint[] { 0 });
                    srcMeterItemIndex = item.ElecSetting[0].Order;
                    sourceMeterReadData = IO(item, srcMeterItemIndex, TrigList);
                    break;
                //--------------------------------------------------------------------------------------------------------------
                default:
                    break;
            }

            this._preTypeState = (int)item.Type;
        }

        private bool CheckChannelInfo()
        {
            this._isChannelHasDie[0] = true;

            //if(_sysSetting)

            // Multi-Die, check the info. for share memory with prober.
            if (GlobalFlag.IsSuccessCheckChannelConfig)
            {
                string tempStr = string.Empty;

                tempStr = this._cmdData.StringData[0];

                // Chnnel has Die
                if (tempStr == null)
                {
                    return false;
                }

                char[] data = tempStr.ToCharArray();

                for (int i = 0; i < _machineConfig.ChannelConfig.ChannelCount; i++)
                {
                    this._isChannelHasDie[i] = data[i].ToString() == "1" ? true : false;
                }

                // Test Chip Group
                if (this._cmdData.StringData[1] == null)
                {
                    return false;
                }

                string[] strData = this._cmdData.StringData[1].Split(',');

                for (int i = 0; i < this._chipGroup.Length; i++)
                {
                    if (strData.Length > i)
                    {
                        int.TryParse(strData[i], out this._chipGroup[i]);

                        this._acquireData.ChannelResultDataSet[(uint)i][EProberDataIndex.TestChipGroup.ToString()].Value = this._chipGroup[i];
                    }
                }

                // Probe Bin
                if (this._cmdData.StringData[2] == null)
                {
                    return false;
                }

                strData = this._cmdData.StringData[2].Split(',');

                for (int i = 0; i < strData.Length; i++)
                {
                    if (strData.Length > i)
                    {
                        int.TryParse(strData[i], out this._chipProbeBin[i]);
                    }
                }
            }

            return true;
        }

        private bool CheckStateIsSkip(TestItemData item)
        {
            if (item.MsrtResult != null)
            {
                for (int k = 0; k < item.MsrtResult.Length; k++)
                {
                    if (item.MsrtResult[k].IsEnable)
                    {
                        if (!item.MsrtResult[k].IsPass)
                        {
                            if (item.MsrtResult[k].IsVerify)
                            {
                                this._acquireData.ChipInfo.IsPass &= false;
                            }

                            if (item.MsrtResult[k].IsSkip)
                            {
                                return true;    // IsEnable = true && IsSkip = true && IsPass = false
                            }
                        }
                    }
                }
            }

            return false;
        }

        private void CheckAdjacentStatus()
        {
            if (!this._sysSetting.IsEnableAdjacentError)
            {
                return;
            }

            EAdjacentResult result = (_adjacentCheck as MultiDieAdjacentCheck).Push(this._condCtrl.Data.ChannelConditionTable, this._acquireData.ChannelResultDataSet);

            this._acquireData.ChipInfo.AdjacentResult = result;

            if (result == EAdjacentResult.RETEST)
            {
                //this._acquireData.IR[(int)EDataIR.IsAdjacentError] = 3; // Disable Call Prboer Clean needle
                _adjacentCheck.ChangeMapRowCol();  // Return To Prober 3rd Coord.
                this._acquireData.ChipInfo.ReTestColX = _adjacentCheck.ReTestCoordX;
                this._acquireData.ChipInfo.ReTestRowY = _adjacentCheck.ReTestCoordY;
            }
        }

        private void CheckPassRate()
        {
            // Pass Rate Check 

            this._passRateCheck.Push(this._condCtrl.Data.TestItemArray);

            if (this._passRateCheck.IsStopTest)
            {
                GlobalFlag.IsPassRateCheckSuccess = false;

                this.SetErrorCode(EErrorCode.PassRateCheckFail, this._passRateCheck.ErrorMsg);
            }
            else
            {
                GlobalFlag.IsPassRateCheckSuccess = true;
            }
        }

        private void SWDelayTime(double delayTime)
        {
            if (delayTime >= 30.0d)
            {
                System.Threading.Thread.Sleep((int)delayTime);
            }
            else
            {
                this._pt.Start();
                do
                {
                    if (this._pt.PeekTimeSpan(ETimeSpanUnit.MilliSecond) >= delayTime)
                    {
                        this._pt.Stop();
                        this._pt.Reset();
                        return;
                    }
                    System.Threading.Thread.Sleep(0);
                } while (this._pt.PeekTimeSpan(ETimeSpanUnit.MilliSecond) < delayTime);
                this._pt.Stop();
                this._pt.Reset();
            }
        }

        private void CopyOptiTestItemResult(TestItemData[] toTestItemArray)
        {
            uint fromChannel = this._acquireData.OpticalSamplingSourceChannel;
            uint toChannel = this._acquireData.ChipInfo.Channel;

            // Multi-Die 光抽測複製
            if (this._acquireData.ChannelResultDataSet[toChannel].IsPass)
            {
                foreach (TestItemData toItem in toTestItemArray)
                {
                    if (toItem is LOPWLTestItem)
                    {
                        foreach (var toResult in toItem.MsrtResult)
                        {
                            if (toResult.IsEnable && toResult.IsVision)
                            {
                                toResult.Value = this._acquireData.ChannelResultDataSet[fromChannel][toResult.KeyName].Value;
                            }
                        }
                    }
                }
            }

        }

        private void CalcOptiTestItemResult(TestItemData[] testItemArray)
        {
            uint LOPWLIndex = 0;
            double[] tempValue = null;
            double[] calcAbsIntensity = null;

            uint channel = this._acquireData.ChipInfo.Channel;

            foreach (TestItemData item in testItemArray)
            {
                if ((item is LOPWLTestItem || item is LIVTestItem) && this._machineConfig.Enable.IsSimulator == false)
                {
                    uint sptMeterItemIndex = channel * this._testSquenceOptCount + LOPWLIndex;

                    //---------------------------------------------------------------------------------------------
                    // (1) this.GetOptiMsrtResult() call the calculation of spectrometer
                    //---------------------------------------------------------------------------------------------
                    if (this._sptMeter == null)
                        continue;

                    this.GetOptiMsrtResult(sptMeterItemIndex, item);

                    if (item is LIVTestItem)
                    {
                        LOPWLIndex += (uint)(item as LIVTestItem).OptiSettingList.Count;

                        if (item.IsEnable)
                        {
                            int count = (item as LIVTestItem).DataLength;

                            for (int i = 0; i < count; i++)
                            {
                                float data = this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVWLD].DataArray[i] - this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVWLP].DataArray[i];

                                this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVDWDWP].DataArray[i] = data;

                                //data = this._acquireData.LIVData[item.KeyName][ELIVOptiMsrtType.LIVMVFLB.ToString()][i] - this._acquireData.LIVData[item.KeyName][ELIVOptiMsrtType.LIVMVFLA.ToString()][i];

                                //this._acquireData.LIVData[item.KeyName][ELIVOptiMsrtType.LIVMVFLD.ToString()].Add(data);

                                data = this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVMsrtI].DataArray[i] * this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVMsrtV].DataArray[i];

                                this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVEWATT].DataArray[i] = data;

                                data = this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVLM].DataArray[i] / this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVEWATT].DataArray[i];

                                this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVLE].DataArray[i] = data;

                                data = (0.001f * this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVWATT].DataArray[i]) / this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVEWATT].DataArray[i];

                                this._acquireData.LIVDataSet[item.KeyName][ELIVOptiMsrtType.LIVWPE].DataArray[i] = data;
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

                        continue;
                    }

                    //---------------------------------------------------------------------------------------------
                    // (2) After calculation of spectrometer, it can get absolute spectrum
                    //---------------------------------------------------------------------------------------------
                    calcAbsIntensity = this._sptMeter.GetYAbsoluateSpectrum(sptMeterItemIndex);

                    Array.Copy(calcAbsIntensity, 0, this._acquireData.SpectrumDataSet[channel, item.KeyName].Absoluate, 0, this._acquireData.SpectrumDataSet[channel, item.KeyName].Absoluate.Length);

                    if (this._darkSample != null)
                    {
                        Array.Copy(this._darkSample, 0, this._acquireData.SpectrumDataSet[channel, item.KeyName].Dark, 0, this._acquireData.SpectrumDataSet[channel, item.KeyName].Dark.Length);
                    }

                    //---------------------------------------------------------------------------------------------
                    // (3) Call the calibration for LOPWL test item
                    //---------------------------------------------------------------------------------------------
                    //===================================================================
                    // (3-a) Calibrate LOP value by chuck correction 
                    //===================================================================
                    this._condCtrl.CalibrateChuckLOP(((int)this._sysResultItem[(int)ESysResultItem.CHUCKINDEX].Value) - 1,
                                                                               this._product.ChuckLOPCorrectArray,
                                                                               item.MsrtResult);
                    //===================================================================
                    // (3-b) Calibrate LOP value by lookup table 
                    //===================================================================

                    if (this._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Die)
                    {
                        if (this._condCtrl.Data.ChannelConditionTable.IsApplyByChannelCompensate)
                        {
                            // By Channel 補完 再補大小係數表
                            item.CalibrateByChannel();

                            for (int i = 0; i < item.MsrtResult.Length; i++)
                            {
                                item.MsrtResult[i].RawValue = item.MsrtResult[i].Value;
                            }
                        }
                    }

                    //---------------------------------------------------------------------------------------------------------------------------
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
                        //===================================================================
                        // (3-c) Calibrate LOP value by "Gain/Offset"
                        //===================================================================
                        item.Calibrate();   // for Gain Offset calibration by RawValue, 	
                    }

                    //=========================
                    // 2016.06.02_解決光電同測時，Vf補償後會有負值的狀況
                    //=========================

                    if (item.MsrtResult[(int)EOptiMsrtType.MVFLA].Value < 0)
                    {
                        item.MsrtResult[(int)EOptiMsrtType.MVFLA].RawValue = 0;
                        item.MsrtResult[(int)EOptiMsrtType.MVFLA].Value = 0;
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
                    item.MsrtResult[(int)EOptiMsrtType.LE].Value = item.MsrtResult[(int)EOptiMsrtType.LM].Value / item.MsrtResult[(int)EOptiMsrtType.EWATT].Value;

                    item.MsrtResult[(int)EOptiMsrtType.WPE].RawValue = (0.001 * item.MsrtResult[(int)EOptiMsrtType.WATT].RawValue) / item.MsrtResult[(int)EOptiMsrtType.EWATT].RawValue;
                    item.MsrtResult[(int)EOptiMsrtType.WPE].Value = (0.001 * item.MsrtResult[(int)EOptiMsrtType.WATT].Value) / item.MsrtResult[(int)EOptiMsrtType.EWATT].Value;

                    // this._sysResultItem[(int)ESysResultItem.SEQUENCETIME].Value = this._acquireData.DR[(int)EDataDR.TestTime] - (item.MsrtResult[(int)EOptiMsrtType.ST].RawValue + 6 + conditionTime);

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

                    LOPWLIndex++;
                }
            }
        }

        private bool[] RunOpenShortTestItem()
        {
            if (this._machineConfig.ChannelConfig.TesterSequenceType == ETesterSequenceType.Series)
            {
                // this._srcSyncTrigger.Add(0);
            }
            else
            {
                this._srcSyncTrigger.Clear();

                for (uint channel = 0; channel < this._machineConfig.ChannelConfig.ChannelCount; channel++)
                {
                    if (this._isChannelHasDie[channel])
                    {
                        this._srcSyncTrigger.Add(channel);
                    }
                }
            }

            uint srcChannel = 0;

            uint dutChannel = this._acquireData.ChipInfo.Channel;

            double[] sourceMeterReadData;

            //bool[] isStopTestArray = new bool[this._srcSyncTrigger.Count];

            //this._isOnlySkipIzTestItem = new bool[this._srcSyncTrigger.Count];

            bool[] isStopTestArray = new bool[this._machineConfig.ChannelConfig.ChannelCount];

            this._isOnlySkipIzTestItem = new bool[this._machineConfig.ChannelConfig.ChannelCount];

            if (this._openShortContactItem != null)
            {
                if (this._openShortContactItem.IsEnable && this._srcMeter != null)
                {
                    this._srcMeter.MeterOutput(this._srcSyncTrigger.ToArray(), 0);

                    if (this._machineConfig.Enable.IsInstantGetData == true)
                    {
                        for (int i = 0; i < this._srcSyncTrigger.Count; i++)
                        {
                            bool isStopTest = false;

                            srcChannel = this._srcSyncTrigger[i];

                            sourceMeterReadData = this._srcMeter.GetDataFromMeter(srcChannel, 0);

                            this._openShortContactItem.MsrtResult[0].Unit = "V";

                            for (int j = 0; j < sourceMeterReadData.Length; j++)
                            {
                                sourceMeterReadData[j] = sourceMeterReadData[j] * UnitMath.UnitConvertFactor(EVoltUnit.V, this._openShortContactItem.MsrtResult[0].Unit) * this._chipPolarity;
                            }

                            this._openShortContactItem.MsrtResult[0].RawValueArray = sourceMeterReadData;

                            this._openShortContactItem.MsrtResult[0].RawValue = sourceMeterReadData[0];

                            if (this._machineConfig.ChannelConfig.TesterSequenceType == ETesterSequenceType.Parallel)
                            {
                                dutChannel = srcChannel;
                            }

                            double volt = this._openShortContactItem.MsrtResult[0].RawValue;

                            if (volt > this._sysSetting.contactCheckCFG._contactSpecMax)
                            {
                                isStopTest = true;
                            }

                            if (volt < this._sysSetting.contactCheckCFG._contactSpecMin)
                            {
                                isStopTest = true;
                            }

                            if (_isManualTest == true)
                            {
                                isStopTest = false;
                            }

                            if (this._sysSetting.contactCheckCFG._isDisableCheckAtPosX)
                            {
                                if (this._cmdData.DoubleData[(uint)EProberDataIndex.COL] == 0.0d)
                                {
                                    isStopTest = false;
                                }
                            }

                            this._isOnlySkipIzTestItem[i] = isStopTest; //  // 當Stop時，即是SkipIZ

                            if (this._sysSetting.IsOnlySkipIZItem)
                            {
                                isStopTest = false;
                            }

                            isStopTestArray[i] = isStopTest;

                        }
                    }
                }
            }

            return isStopTestArray;
        }

        private void ResetItemMsrtResultByOpenShortCheck()
        {
            if (this._openShortContactItem == null && !this._sysSetting.IsTakeFirstItemAsOpenShort)
            {
                return;
            }

            TestItemData[] testItemArray = null;

            for (uint channel = 0; channel < this._machineConfig.ChannelConfig.ChannelCount; channel++)
            {
                testItemArray = this._condCtrl.Data.ChannelConditionTable.Channels[channel].Conditions.ToArray();

                if (this._isChannelOpenShortFail[channel])
                {
                    //-------------------------------------------------------------------------------------
                    // Reset "TestResult" of the no tested item as ZERO By Open Short Check
                    //-------------------------------------------------------------------------------------
                    ResetItemMsrtResultValueToZero(testItemArray, 0, true);

                    FillFakeValueToResultItem(testItemArray);
                }

                if (this._isChannelStopTest[channel] && this._sysSetting.IsTakeFirstItemAsOpenShort)
                {
                    ResetItemMsrtResultValueToZero(testItemArray, 0, true);

                    FillFakeValueToResultItem(testItemArray);
                }
            }
        }

        private void CheckSrcMeterQCMode(TestItemData[] itemArray)
        {
            this._isQCSetting = false;

            if (_machineConfig.SourceMeterModel != ESourceMeterModel.LDT3A200 || !_sysSetting.IsEnableDeviceQcMode)
            {
                return;
            }

            int enCntVR = 0;
            int enCntIZ = 0;
            int enCntIF = 0;
            int enCntVF = 0;

            foreach (var item in itemArray)
            {
                if (item is THYTestItem || item is LOPWLTestItem || item is ESDTestItem)
                {
                    if (item.IsEnable)
                    {
                        return;
                    }
                }
                else if (item is IFTestItem)
                {
                    if (item.IsEnable)
                    {
                        enCntIF++;
                    }
                }
                else if (item is VFTestItem)
                {
                    if (item.IsEnable)
                    {
                        enCntVF++;
                    }
                }
                else if (item is VRTestItem)
                {
                    if (item.IsEnable)
                    {
                        enCntVR++;
                    }
                }
                else if (item is IZTestItem)
                {
                    if (item.IsEnable)
                    {
                        enCntIZ++;
                    }
                }
            }

            if (enCntIF <= 2 && enCntVF == 0 && enCntIZ == 0 && enCntVR == 0)
            {
                this._isQCSetting = true;
            }
            else if (enCntIF == 0 && enCntVF <= 2 && enCntIZ == 0 && enCntVR == 0)
            {
                this._isQCSetting = true;
            }
            else if (enCntIF == 0 && enCntVF == 0 && enCntIZ <= 2 && enCntVR == 0)
            {
                this._isQCSetting = true;
            }
            else if (enCntIF == 0 && enCntVF == 0 && enCntIZ == 0 && enCntVR <= 2)
            {
                this._isQCSetting = true;
            }
        }

        private List<double> GetFloatforceList(TestItemData item, string msrtKeyName, double unitFactor, int maxCh, double factor, double offset)
        {
            List<double> fValList = new List<double>();
            for (int i = 0; i < maxCh; ++i)
            {
                //this._condCtrl.Data.ChannelConditionTable.Channels[dutChannel][item.KeyName].MsrtResult[dataNum].RawValue
                double fVal = 0;
                bool isfound = false;
                try
                {
                    foreach (var tData in this._condCtrl.Data.ChannelConditionTable.Channels[i].Conditions)
                    {
                        foreach (var rData in tData.MsrtResult)
                        {
                            if (rData.KeyName == msrtKeyName)
                            {
                                fVal = Math.Abs((rData.Value * factor - offset)) * -1.0d / unitFactor;
                                isfound = true;
                                break;
                            }
                        }
                        if (isfound)
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(string.Format("[GetFloatforceList] ,Err in finding Channels[{0}][{1}] ", i, item.KeyName));
                }
                fValList.Add(fVal);
            }
            return fValList;
        }

       

        private void CreateChShiftTable()
        {
            _chShiftTable = new ChannelPosShiftTable<int>();

            int colCnt = this._machineConfig.ChannelConfig.ColXCount;
            int rowCnt = this._machineConfig.ChannelConfig.RowYCount;

            int channel = 0;
            int level = 0;
            for (int y = 0; y < rowCnt; ++y)
            {
                for (int x = 0; x < colCnt; ++x)
                {
                    double doubleX = (double)x;
                    double doubleY = (double)y;

                    _chShiftTable.Push(channel, level, (int)doubleX, (int)doubleY);

                    ++channel;
                }
            }
        }

       
        #endregion

        #region>>protected<<
        protected override void ResetKernelData()
        {
            int channelCount = _machineConfig.ChannelConfig.ChannelCount;

            //------------------------------------------------------------------------------------------------------
            // (1 Add TestResultData from "TestItemArray" of condtion data
            //------------------------------------------------------------------------------------------------------
            if (this._condCtrl.Data != null && this._condCtrl.Data.TestItemArray != null)
            {
                this._acquireData.SetData(this._condCtrl.Data, this._sysResultItem);
            }

            //--------------------------------------------------------------------------------------------------------
            // (2) Set the variable of system kernel  
            //--------------------------------------------------------------------------------------------------------
            this._darkCorrectCount = 0;
            this._isMoveDataToStorage = true;
            this._acquireData.ChipInfo.GoodDieCount = 0;
            this._acquireData.ChipInfo.FailDieCount = 0;
            this._acquireData.ChipInfo.TestCount = 0;
            // Paul Add
            this._sysResultItem[(int)ESysResultItem.TEST].Value = 0;

            GlobalFlag.OptimumStatus = 1;

            //---------------------------------------------------------------------------------------
            // (3) Reset  Multi-Die Required information
            //---------------------------------------------------------------------------------------
            this._isChannelHasDie = new bool[channelCount];

            this._isChannelStopTest = new bool[channelCount];

            this._isChannelOpenShortFail = new bool[channelCount];

            this._chipGroup = new int[channelCount];

            this._chipProbeBin = new int[channelCount];

            this._srcSyncTrigger.Clear();

            this._srcSyncTrigger.Add(0);  // Add Default srcMeter
        }

        protected override bool RunTestSequence(bool isReTest = false)
        {
            this._isTestSuccess = false;
            _isTriggerSptErr = false;

            //this.SetSrcMeterQCMode(_isManualTest);

            //if (this._status.State == EKernelState.Not_Ready)
            //{
            //    this.SetErrorCode(EErrorCode.System_Not_Ready);
            //    return false;
            //}

            //-----------------------------------------------------------------------------------
            // (1) Chang the ROW , COL by defined coordinate and
            //      Copy ProberData to this._acquireData.OutputTestResult;
            //-----------------------------------------------------------------------------------		
            this._sysResultItem[(int)ESysResultItem.CHUCKINDEX].Value = this._cmdData.DoubleData[(uint)EProberDataIndex.PROBE_INDEX];	// 1-Base

            

            //-----------------------------------------------------------------------------------
            // (2) Check past tested data has move to UI
            //-----------------------------------------------------------------------------------
            if (this._isMoveDataToStorage == false)
            {
                this.SetErrorCode(EErrorCode.NotFinishMoveTestedData);
                return false;
            }

            //-----------------------------------------------------------------------------------
            // (3) Check kernel Error Code
            //-----------------------------------------------------------------------------------
            if (this.Status.ErrorCode != (int)EErrorCode.NONE)
                return false;

            //-----------------------------------------------------------------------------------
            // (4) Check Die Status on each Channel(Prober->Tester, has die / no die on channel)
            //     Roy, 20140331
            //-----------------------------------------------------------------------------------
            if (!this.CheckChannelInfo())
                return false;

            this._ptTestTime.Start();

            lock (this)
            {
                //--------------------------------------------------------------------
                // (5) Run the test process
                //--------------------------------------------------------------------       
                this.TestProcess(isReTest);
            }

            this._isTestSuccess = this.CheckAllDeviceErrorState();//這個需要10ms左右

            time = this._ptTestTime.PeekTimeSpan(ETimeSpanUnit.MilliSecond);

            return this._isTestSuccess;
        }

        protected override double[] LOPWL(TestItemData item, uint srcMeterItemIndex,
            uint sptMeterItemIndex, List<uint> trigList = null, ISourceMeter srcMeter = null, ISpectroMeter sptMeter = null)
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

                uint srcChannel = trigList[0];
                uint dutChannel = this._acquireData.ChipInfo.Channel;

                #region >>> LOPWL <<<

                //================================================================
                //  [1] Source Meter force the current and measurement volt.
                //================================================================
                if (item.IsEnable)
                {
                    if (_sysSetting.IsEnableMultiDieOpticalSamplingTest)
                    {
                        if (this._acquireData.IsGetOpticalSamplingSource)
                        {
                            this._preTypeState = (int)item.Type;

                            return null;
                        }

                        this._acquireData.OpticalSamplingSourceChannel = this._acquireData.ChipInfo.Channel;
                    }

                    if (srcMeter != null)
                    {
                        srcMeterItemIndex = item.ElecSetting[0].Order;

                        srcMeter.MeterOutput(trigList.ToArray(), srcMeterItemIndex);

                        srcChannel = trigList[0];

                        if (this._machineConfig.Enable.IsInstantGetData == true)
                        {
                            sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex);

                            item.MsrtResult[(int)EOptiMsrtType.MFILA].RawValue = srcMeter.GetApplyDataFromMeter(srcChannel, srcMeterItemIndex)[0];

                            item.MsrtResult[(int)EOptiMsrtType.MVFLA].Unit = "V";

                            item.MsrtResult[(int)EOptiMsrtType.MVFLA].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[(int)EOptiMsrtType.MVFLA].Unit) * this._chipPolarity;

                            if (this._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Die)
                            {
                                if (this._machineConfig.ChannelConfig.TesterSequenceType == ETesterSequenceType.Parallel)
                                {
                                    dutChannel = srcChannel;
                                }

                                this._condCtrl.Data.ChannelConditionTable.Channels[dutChannel][item.KeyName].MsrtResult[(int)EOptiMsrtType.MVFLA].RawValue = item.MsrtResult[(int)EOptiMsrtType.MVFLA].RawValue;
                            }
                        }
                    }

                    //================================================================
                    // [2] Trigger the Spectrometer to get spectrum
                    //================================================================
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
                            // calc spt trigger index
                            sptMeterItemIndex = this._acquireData.ChipInfo.Channel * this._testSquenceOptCount + (item as LOPWLTestItem).OptiSetting.Order;

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
                    // 20180921
                    //else if (srcMeter != null && (item as LOPWLTestItem).IsTestOptical == false)
                    //{
                    //    srcMeter.TurnOff(0.0d, false);
                    //}

                    //================================================================
                    // [3] Continuous force the same current and measurement volt. (MV)
                    //================================================================
                    if (srcMeter != null)
                    {
                        if (!(item as LOPWLTestItem).IsACSourceMeter)
                        {
                            srcMeter.MeterOutput(trigList.ToArray(), srcMeterItemIndex + 1);

                            if (this._machineConfig.Enable.IsInstantGetData == true)
                            {
                                sourceMeterReadData = srcMeter.GetDataFromMeter(srcChannel, srcMeterItemIndex + 1);

                                item.MsrtResult[(int)EOptiMsrtType.MVFLB].Unit = "V";

                                item.MsrtResult[(int)EOptiMsrtType.MVFLB].RawValue = sourceMeterReadData[0] * UnitMath.UnitConvertFactor(EVoltUnit.V, item.MsrtResult[(int)EOptiMsrtType.MVFLB].Unit) * this._chipPolarity;

                                if (this._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Die)
                                {
                                    this._condCtrl.Data.ChannelConditionTable.Channels[dutChannel][item.KeyName].MsrtResult[(int)EOptiMsrtType.MVFLB].RawValue = item.MsrtResult[(int)EOptiMsrtType.MVFLB].RawValue;
                                }
                            }
                        }
                    }

                    //20180921
                    if (srcMeter != null)
                    {
                        srcMeter.TurnOff();
                    }

                }
                #endregion
            }

            return sourceMeterReadData;
        }

        protected override bool FloatIZ(TestItemData item, uint srcMeterItemIndex, List<uint> trigList, ISourceMeter srcMeter, bool isCanGetData)
        {
            TestResultData data = null;

            string msrtKeyName = (item as IZTestItem).RefIrKeyName;

            data = this._acquireData[msrtKeyName];
            double unitFactor = UnitMath.UnitConvertFactor(EAmpUnit.A, data.Unit);
            int maxCh = this._machineConfig.ChannelConfig.RowYCount * this._machineConfig.ChannelConfig.ColXCount;

            double factor = (item as IZTestItem).Factor;
            double offset = (item as IZTestItem).Offset;


            List<double> fValList = GetFloatforceList(item, msrtKeyName, unitFactor, maxCh, factor, offset);

            //double applyValue = Math.Abs((data.Value * (item as IZTestItem).Factor - (item as IZTestItem).Offset)) * -1.0d / unitFactor;

            if (srcMeter is Keithley2600)
            {
                (srcMeter as Keithley2600).MeterOutput(trigList.ToArray(), srcMeterItemIndex, fValList.ToArray());
            }
            else
            {
                Console.WriteLine("[IZ] ,_srcMeter is not Keithley2600");
                return false;
                //break;
            }
            return isCanGetData;
        }

        protected override bool FloatVR(TestItemData item, uint srcMeterItemIndex, List<uint> trigList, ISourceMeter srcMeter, bool isCanGetData)
        {
            string msrtKeyName = (item as VRTestItem).RefVzKeyName;
            TestResultData data = null;
            data = this._acquireData[msrtKeyName];

            if (data == null)
            {
                Console.WriteLine("[FloatForce] ,this._acquireData[] don't have " + msrtKeyName);
                return false;
            }
            int maxCh = this._machineConfig.ChannelConfig.RowYCount * this._machineConfig.ChannelConfig.ColXCount;
            double unitFactor = UnitMath.UnitConvertFactor(EVoltUnit.V, data.Unit);
            double factor = (item as VRTestItem).Factor;
            double offset = (item as VRTestItem).Offset;

            List<double> fValList = GetFloatforceList(item, msrtKeyName, unitFactor, maxCh, factor, offset);

            //double applyValue = Math.Abs((data.Value * (item as IZTestItem).Factor - (item as IZTestItem).Offset)) * -1.0d / unitFactor;

            if (srcMeter is Keithley2600)
            {
                (srcMeter as Keithley2600).MeterOutput(trigList.ToArray(), srcMeterItemIndex, fValList.ToArray());
            }
            else
            {
                Console.WriteLine("[IZ] ,_srcMeter is not Keithley2600");
                return false;
                //break;
            }
            return isCanGetData;
       
        }
        #endregion

        #region >>> Public Methods <<<


        public override void Init<T, K>(T config, K rdFunc)
        {
            //---------------------------------------------------------------------------------------
            // (1) Load the machine config file
            //---------------------------------------------------------------------------------------
            this.LoadMachineCfgFile();

            // this.LoadRDFuncParam();
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

                return;
            }

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
            // (7) Initialize AD Card and config the default setting 
            //---------------------------------------------------------------------------------------
            //if (this._machineConfig.PDSensingMode == EPDSensingMode.DAQ)
            //{
            //    this._ADCard = new PCI826LU();
            //    this._ADCard.Init();
            //    this._ADCard.Config();
            //}

            //---------------------------------------------------------------------------------------
            // (8) Initialize PrintPort 
            //---------------------------------------------------------------------------------------
            this._IOPort = new PortAccess();

            //---------------------------------------------------------------------------------------
            // (9) All hardware are created and finish intialization. 
            //      The kernel system transfer to read state
            //---------------------------------------------------------------------------------------

            //---------------------------------------------------------------------------------------
            // (11) Initialize LaserPostCalc 
            //---------------------------------------------------------------------------------------
            this._laserPostCalc = new MpiLaserPostCalc();

            this._status.State = EKernelState.Ready;

            return;
        }

        public override bool SetSysData<T, M>(T sysData, M sysCail)
        {
            this._sysSetting = (sysData as TesterSetting).Clone() as TesterSetting;

            this._sysCali = (sysCail as SystemCali).Clone() as SystemCali;

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

            if (!GlobalFlag.IsGetChShiftTableFromProber)
            {
                CreateChShiftTable();
            }

            //--------------------------------------------------------------------------------------------------------
            // Set configuration and parameters data to hardware device 
            // by recipe or condtion setting
            //--------------------------------------------------------------------------------------------------------
            if (this._product == null)
                return true;

            //--------------------------------------------------------------------------------------------------------
            // (1) Fisrt, set and transfer relative parameter to local variable 
            //--------------------------------------------------------------------------------------------------------
            SetPolarity();
            //--------------------------------------------------------------------------------------------------------
            // (2) Count the elecTestItem, opticalTestItem and ESDTestItem,
            //      then covert the positive and negative value by the chip polarity
            //--------------------------------------------------------------------------------------------------------	
            this._testSequencElecCount = 0;
            this._testSquenceOptCount = 0;
            uint electSettingOrder = 0;
            uint optiSettingOrder = 0;
            uint esdSettingOrder = 0;

            //=====================
            // Multi-Die Open/Short Check
            //=====================

            SetContactCheck(elecSettingList, ref electSettingOrder);

            #region>> modify setting data<<
            if (this._condCtrl.Data != null)
            {
                if (this._condCtrl.Data.TestItemArray != null)
                {
                    //-------------------------------------------------------------------------------------------------------
                    // (A) SpectroMeter Setting Data
                    //     將光的設定,在底層展開 i.e. opticalTestItem.Length = (channel count) * (TestItemArray 中有幾道光)
                    //-------------------------------------------------------------------------------------------------------

                    for (int i = 0; i < this._machineConfig.ChannelConfig.ChannelCount; i++)
                    {
                        optiSettingOrder = 0;

                        foreach (TestItemData item in this._condCtrl.Data.TestItemArray)
                        {
                            if (item is LOPWLTestItem)
                            {
                                (item as LOPWLTestItem).OptiSetting.Order = optiSettingOrder;

                                optiSettingList.Add((item as LOPWLTestItem).OptiSetting.Clone() as OptiSettingData);

                                optiSettingOrder++;

                                if (i == 0)
                                {
                                    this._testSquenceOptCount++;
                                }
                            }
                            else if (item is LIVTestItem)
                            {
                                //(item as LIVTestItem).OptiSettingList
                                if (i == 0)
                                {
                                    this._testSquenceOptCount++;
                                }


                                optiSettingList.AddRange((item as LIVTestItem).OptiSettingList);  // ??? LIV 的index需檢察, 可能會有問題
                            }

                        }
                    }

                    if (GlobalFlag.IsProductChannelConditionNotMatch)
                    {
                        this.Status.State = EKernelState.Not_Ready;
                    }
                    else
                    {
                        for (int i = 0; i < this._machineConfig.ChannelConfig.ChannelCount; i++)
                        {
                            uint tempOrder = electSettingOrder;

                            foreach (TestItemData item in this._condCtrl.Data.ChannelConditionTable.Channels[i].Conditions.ToArray())
                            {
                                if (item.ElecSetting != null)
                                {
                                    foreach (ElectSettingData data in item.ElecSetting)
                                    {
                                        data.Order = tempOrder;

                                        tempOrder++;
                                    }
                                }
                            }
                        }
                    }

                    this.CheckSrcMeterQCMode(this._condCtrl.Data.TestItemArray);

                    //-------------------------------------------------------------------
                    // 尋找 ESDTestItem上一道電，並將 IsNextIsESDTestItem = true
                    //-------------------------------------------------------------------
                    for (int i = 0; i < this._condCtrl.Data.TestItemArray.Length; i++)
                    {
                        TestItemData item = this._condCtrl.Data.TestItemArray[i];

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

                    foreach (TestItemData item in this._condCtrl.Data.TestItemArray)
                    {
                        //-------------------------------------------------------------------------------------------------------
                        // (B) SourceMeter Setting Data
                        //-------------------------------------------------------------------------------------------------------
                        if (item.ElecSetting != null)
                        {
                            foreach (ElectSettingData data in item.ElecSetting)
                            {
                                data.Order = electSettingOrder;

                                elecSettingList.Add(data.ConvertUnitTo(EAmpUnit.A.ToString(), EVoltUnit.V.ToString(), this._chipPolarity));

                                electSettingOrder++;
                            }
                        }

                        //-------------------------------------------------------------------------------------------------------
                        // (C) ESD Setting Data
                        //-------------------------------------------------------------------------------------------------------
                        if (item is ESDTestItem)
                        {
                            ESDTestItem esditem = item as ESDTestItem;
                            esditem.EsdSetting.Order = esdSettingOrder;
                            esdSettingOrder++;

                            ESDSettingData esdSetting = esditem.EsdSetting.Clone() as ESDSettingData;
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
                    }
                }
            }
            #endregion
            this._testSequencElecCount = (uint)elecSettingList.Count;

            //if (this._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Die)
            //{
            //    if (this._condCtrl.Data != null)
            //    {
            //        if (this._condCtrl.Data.TestItemArray != null)
            //        {
            //            this._condCtrl.Data.ChannelConditionTable.UpdateConditionTestItems(this._condCtrl.Data.TestItemArray);
            //        }
            //    }
            //}

            this.ResetKernelData();

            if (this.Status.State == EKernelState.Not_Ready)
                return false;

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
            if (this._machineConfig.SourceMeterModel != ESourceMeterModel.NONE && this._srcMeter != null)
            {
                if (this._srcMeter.SetParamToMeter(elecSettingList.ToArray()) == false)
                {
                    this.SetErrorCode(this._srcMeter.ErrorNumber);
                    return false;
                }
            }

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
                this._sysSetting.EsdDevSetting.ChannelAssignment = _machineConfig.ChannelConfig.AssignmentTable;

                this._esdDevice.SetConfigToMeter(this._sysSetting.EsdDevSetting);

                if (this._esdDevice.SetParamToMeter(esdSettingList.ToArray()) == false)
                {
                    this.SetErrorCode(this._esdDevice.ErrorNumber);
                    return false;
                }

                this._esdDevice.ResetToSafeStatus();
                //this._esdDevice.PreCharge();
            }

            (_adjacentCheck as MultiDieAdjacentCheck).Reset(this._sysSetting.IsEnableAdjacentError, this._product, this._sysSetting);

            this._passRateCheck.Start(this._sysSetting, this._condCtrl.Data.TestItemArray);

            this._repeatTestIndexs.Clear();

            this._dataVerify.Start();

            this._skipCount = 0;

            this._isTVSTesting = this._product.IsTVSProduct;
            #endregion
            //-----------------------------------------------------------------------------------
            //  20140127 Paul
            //  先補償大係數再補償小係數，並且使用修正後的波長值
            //-----------------------------------------------------------------------------------

            if (this._sysSetting.IsEnalbeCalcBigFactorBeforeSmall)
            {
                this._condCtrl.Data.CalLookupWave = ECalLookupWave.Corrected;
            }
            else
            {
                this._condCtrl.Data.CalLookupWave = ECalLookupWave.Original;
            }

            //-----------------------------------------------------------------------------------
            //  20140331 Roy
            //  區域補償設定
            //-----------------------------------------------------------------------------------

            _rManager = new ReTestManager();
            //--------------------------------------------------------------------------------------------------------
            // (6) Check all device error state 
            //--------------------------------------------------------------------------------------------------------
            Console.WriteLine("[MultiDie_TesterKernel], Enable Spectrometer State Check => " + this._sysSetting.IsEnableErrStateReTest.ToString());
            Console.WriteLine("[MultiDie_TesterKernel], Enable Log Detail Information  => " + this._sysSetting.IsEnableSaveDetailLog.ToString());
            Console.WriteLine("[MultiDie_TesterKernel], Enable Adjacent Error Check  => " + this._sysSetting.IsEnableAdjacentError.ToString());
            Console.WriteLine("[MultiDie_TesterKernel], Enable High Speed Mode  => " + this._rdFunc.RDFuncData.HighSpeedModeDelayTime.ToString());
            Console.WriteLine("[MultiDie_TesterKernel], Enable ESD High Speed Mode  => " + this._sysSetting.EsdDevSetting.IsHighSpeedMode.ToString());

            return (this.CheckAllDeviceErrorState());
        }

        public bool ResetMachineHW()
        {
            List<OptiSettingData> optiSettingList = new List<OptiSettingData>();

            if (this._condCtrl.Data != null)
            {
                if (this._condCtrl.Data.TestItemArray != null)
                {
                    //foreach (TestItemData item in this._condCtrl.Data.TestItemArray)
                    //{
                    //    if (item is LOPWLTestItem)
                    //    {
                    //        optiSettingList.Add((item as LOPWLTestItem).OptiSetting);
                    //    }
                    //}

                    //-------------------------------------------------------------------------------------------------------
                    // (A) SpectroMeter Setting Data
                    //     將光的設定,在底層展開 i.e. opticalTestItem.Length = (channel count) * (TestItemArray 中有幾道光)
                    //-------------------------------------------------------------------------------------------------------
                    uint optiSettingOrder = 0;
                    this._testSquenceOptCount = 0;

                    for (int i = 0; i < this._machineConfig.ChannelConfig.ChannelCount; i++)
                    {
                        optiSettingOrder = 0;

                        foreach (TestItemData item in this._condCtrl.Data.TestItemArray)
                        {
                            if (item is LOPWLTestItem)
                            {
                                (item as LOPWLTestItem).OptiSetting.Order = optiSettingOrder;

                                optiSettingList.Add((item as LOPWLTestItem).OptiSetting.Clone() as OptiSettingData);

                                optiSettingOrder++;

                                if (i == 0)
                                {
                                    this._testSquenceOptCount++;
                                }
                            }
                            else if (item is LIVTestItem)
                            {
                                //(item as LIVTestItem).OptiSettingList

                                optiSettingList.AddRange((item as LIVTestItem).OptiSettingList);  // ??? LIV 的index需檢察, 可能會有問題
                            }

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

        public override void GetTestedDataFromDevice()
        {
            if (this._condCtrl.Data == null || this._condCtrl.Data.TestItemArray == null)
                return;

            if (this._srcMeter != null)
            {
                if (this._srcMeter is Keithley2600)
                {
                    (this._srcMeter as Keithley2600).CollectGarbage();
                }
            }

            // For 1-base
            this._acquireData.ChipInfo.TestCount = (int)this._sysResultItem[(int)ESysResultItem.TEST].Value;

            if ((int)this._sysResultItem[(int)ESysResultItem.POLAR].Value == 1)
            {
                this._acquireData.ChipInfo.Polarity = EPolarity.Anode_P;
            }
            else
            {
                this._acquireData.ChipInfo.Polarity = EPolarity.Cathode_N;
            }

            this._acquireData.ChipInfo.TestTime = time;

            this._sysResultItem[(int)ESysResultItem.SEQUENCETIME].Value = this._acquireData.ChipInfo.TestTime;

            //bool isAllItemPass = true;

            //bool isAllItemPass02 = true;

            //bool isAllItemPass03 = true;

            switch (this._machineConfig.TesterFunctionType)
            {
                case ETesterFunctionType.Single_Die:
                case ETesterFunctionType.Multi_Terminal:
                    {
                        #region >>> Single Die <<<

                        //isAllItemPass = true;

                        //isAllItemPass02 = true;

                        //isAllItemPass03 = true;

                        this._acquireData.ChipInfo.Channel = 0;

                        this._sysResultItem[(int)ESysResultItem.CHANNEL].Value = 1;   // Channel display base = 1.

                        //------------------------------------------------------------------------------------------//
                        // (1) 計算 LOPWL Item Result
                        //------------------------------------------------------------------------------------------//
                        this.CalcOptiTestItemResult(this._condCtrl.Data.TestItemArray);

                        this._condCtrl.CalibCalcTestItem();  // CALC & DIB

                        //------------------------------------------------------------------------------------------//
                        // (2) 將量測結果加入 binCalcData  並分Bin
                        //------------------------------------------------------------------------------------------//
                        if (!GlobalFlag.IsReSingleTestMode || !GlobalFlag.IsDeviceVerifyMode)
                        {
                            this._binCalcData.Clear();

                            this.CalcBinGrade(this._condCtrl.Data.TestItemArray);
                        }


                        #endregion;

                        break;
                    }
                case ETesterFunctionType.Multi_Die:
                    {
                        #region >>> Multi Die <<<

                        for (uint seqOrder = 0; seqOrder < this._machineConfig.ChannelConfig.ChannelCount; seqOrder++)
                        {
                            uint channel = this._condCtrl.Data.ChannelConditionTable.GetChannelByOrder(seqOrder);

                            //isAllItemPass = true;

                            //isAllItemPass02 = true;

                            //isAllItemPass03 = true;

                            this._acquireData.ChipInfo.Channel = channel;

                            this._sysResultItem[(int)ESysResultItem.CHANNEL].Value = channel + 1;   // Channel display base = 1.

                            if (!this._acquireData.ChannelResultDataSet[channel].IsTested)
                                continue;

                            this._acquireData.ChipInfo.ColX = this._acquireData.ChannelResultDataSet[channel].Col;

                            this._acquireData.ChipInfo.RowY = this._acquireData.ChannelResultDataSet[channel].Row;

                            this._acquireData.ChipInfo.SubColX = this._acquireData.ChannelResultDataSet[channel].SubCol;

                            this._acquireData.ChipInfo.SubRowY = this._acquireData.ChannelResultDataSet[channel].SubRow;

                            //------------------------------------------------------------------------------------------//
                            // (1) 計算 LOPWL Item Result
                            //------------------------------------------------------------------------------------------//

                            if (_sysSetting.IsEnableMultiDieOpticalSamplingTest)
                            {
                                if (this._acquireData.IsGetOpticalSamplingSource)
                                {
                                    if (this._acquireData.OpticalSamplingSourceChannel == channel)
                                    {
                                        this.CalcOptiTestItemResult(this._condCtrl.Data.ChannelConditionTable.Channels[channel].Conditions.ToArray());
                                    }
                                    else
                                    {
                                        this.CopyOptiTestItemResult(this._condCtrl.Data.ChannelConditionTable.Channels[channel].Conditions.ToArray());
                                    }
                                }
                                else
                                {
                                    this.CalcOptiTestItemResult(this._condCtrl.Data.ChannelConditionTable.Channels[channel].Conditions.ToArray());
                                }
                            }
                            else
                            {
                                this.CalcOptiTestItemResult(this._condCtrl.Data.ChannelConditionTable.Channels[channel].Conditions.ToArray());
                            }

                            this._condCtrl.CalibCalcTestItem(this._condCtrl.Data.ChannelConditionTable.Channels[channel].Conditions.ToArray());  // CALC & DIB

                            this._condCtrl.CalibSystemCoef(this._condCtrl.Data.ChannelConditionTable.Channels[channel].Conditions.ToArray(),
                                                           this._sysCali.SystemCaliData.ToolFactor.ToArray(),
                                                           this._product.IsApplySystemCoef);


                            //------------------------------------------------------------------------------------------//
                            // (2) 將量測結果加入 binCalcData  並分Bin
                            //------------------------------------------------------------------------------------------//
                            if (!GlobalFlag.IsReSingleTestMode || !GlobalFlag.IsDeviceVerifyMode)
                            {
                                this._binCalcData.Clear();

                                this.CalcBinGrade(this._condCtrl.Data.ChannelConditionTable.Channels[channel].Conditions.ToArray());

                                this._acquireData.ChannelResultDataSet[channel].IsPass = this._acquireData.ChipInfo.IsPass;
                            }

                            //this._acquireData.ChannelResultDataSet[channel].BinGradeName = this.CalcNGBinGradeName(this._condCtrl.Data.ChannelConditionTable.Channels[channel].Conditions.ToArray());

                            this._acquireData.ChannelResultDataSet[channel].BinGradeName = "";

                            this._acquireData.ChannelResultDataSet[channel].BinGrade = (int)this._sysResultItem[(int)ESysResultItem.BIN].Value;

                            foreach (TestResultData data in this._sysResultItem)
                            {
                                if (data.KeyName == "TEST")
                                    continue;

                                this._acquireData.ChannelResultDataSet[channel][data.KeyName].Value = data.Value;
                            }
                        }

                        #endregion;

                        break;
                    }
                case ETesterFunctionType.Multi_Pad:
                    {
                        #region >>> Multi Pad <<<

                        this._binCalcData.Clear();

                        //isAllItemPass = true;

                        //isAllItemPass02 = true;

                        //isAllItemPass03 = true;

                        for (uint channel = 0; channel < _product.TestCondition.ChannelConditionTable.Count; channel++)
                        {
                            this._acquireData.ChipInfo.Channel = channel;

                            this._sysResultItem[(int)ESysResultItem.CHANNEL].Value = channel + 1;   // Channel display base = 1.

                            if (!this._acquireData.ChannelResultDataSet[channel].IsTested)
                                continue;

                            //------------------------------------------------------------------------------------------//
                            // (1) 計算 LOPWL Item Result
                            //------------------------------------------------------------------------------------------//
                            this.CalcOptiTestItemResult(this._condCtrl.Data.ChannelConditionTable.Channels[channel].Conditions.ToArray());

                            this._condCtrl.CalibCalcTestItem();  // CALC & DIB

                            //------------------------------------------------------------------------------------------//
                            // (2) 將量測結果加入 binCalcData  並分Bin
                            //------------------------------------------------------------------------------------------//
                            foreach (TestItemData data in this._condCtrl.Data.ChannelConditionTable.Channels[channel].Conditions)
                            {
                                if (!data.IsEnable)
                                    continue;

                                //if (data.MsrtResult != null)
                                //{
                                //    foreach (TestResultData result in data.MsrtResult)
                                //    {
                                //        if (result.IsEnable && result.IsVision)
                                //        {
                                //            string formatValue = result.Value.ToString(result.Formate);

                                //            this._binCalcData.Add(result.KeyName, result.Value);

                                //            if (result.IsEnable && result.IsVerify && result.IsVision)
                                //            {
                                //                isAllItemPass &= result.IsPass;
                                //            }

                                //            if (result.IsEnable && result.IsVerify && result.IsVision)
                                //            {
                                //                isAllItemPass02 &= result.IsPass02;
                                //            }

                                //            if (result.IsEnable && result.IsVerify && result.IsVision)
                                //            {
                                //                isAllItemPass03 &= result.IsPass03;
                                //            }
                                //        }
                                //    }
                                //}


                                if (!GlobalFlag.IsReSingleTestMode)
                                {
                                    this.CalcBinGrade(this._condCtrl.Data.ChannelConditionTable.Channels[channel].Conditions.ToArray());
                                }
                                
                            }
                            this._acquireData.ChannelResultDataSet[channel].BinGradeName = "";

                            this._acquireData.ChannelResultDataSet[channel].BinGrade = (int)this._sysResultItem[(int)ESysResultItem.BIN].Value;
                        }


                        #endregion;

                        break;
                    }
            }

            //this.CheckAllDeviceErrorState();

            this.CheckAdjacentStatus();

            this.CheckPassRate();

            this._acquireData.ChipInfo.IsDieTestStatePass = false;

            this._sysResultItem[(int)ESysResultItem.DIETESTSTATE].Value = Convert.ToInt32(this._acquireData.ChipInfo.IsDieTestStatePass);

            // Fire event
            this._isMoveDataToStorage = false;

            Fire_FinishTestAndCalcEvent(null);

            return;
        }

        public bool CheckChannelConfig(uint colXCount, uint rowYCount, int theta)
        {
            this._channelArrangeRotateTheta = theta;

            if (this._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Die)
            {
                //if (this._machineConfig.ChannelConfig.TesterSequenceType == ETesterSequenceType.Matrix)
                //{
                //    // 20191112 Jeemo, Matrix: Prober = Single Die, Tester = Multi-Die => Bypass Channel Check
                //    GlobalFlag.IsSuccessCheckChannelConfig = true;

                //    Console.WriteLine("[HS_TesterKernel], Enable Matrix Tester Sequence.");

                //    return true;
                //}

                if (colXCount == this._machineConfig.ChannelConfig.ColXCount &&
                    rowYCount == this._machineConfig.ChannelConfig.RowYCount)
                {
                    GlobalFlag.IsSuccessCheckChannelConfig = true;
                }
                else
                {
                    GlobalFlag.IsSuccessCheckChannelConfig = false;

                    //  this.SetErrorCode(EErrorCode.SystemChannelConfigNotMatch);

                    Console.WriteLine("[HS_TesterKernel], Channel Config Is Not Match");

                    return false;
                }

            }

            return true;
        }

        public bool PushChShiftTable(int channel, int layer, int x, int y, bool createNewTable = false)
        {
            if (createNewTable)
            {
                _chShiftTable = new ChannelPosShiftTable<int>();
            }

            _chShiftTable.Push(channel, layer, x, y);

            GlobalFlag.IsGetChShiftTableFromProber = true;

            return true;

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

            this._acquireData.ChipInfo.IsDieTestStatePass = true;

            this._acquireData.ChipInfo.ChuckTemp = this._cmdData.DoubleData[(uint)EProberDataIndex.Temprature];

            lock (this._lockObj)
            {
                switch (command)
                {
                    case (int)ETesterKernelCmd.RunTest:
                        this._sysSetting.IsEnableDarkCorrect = true;

                        rtn = this.RunTestSequence();

                        if (_isTriggerSptErr == true)
                        {
                            // this.ChangeRowColCoord();
                            this._darkCorrectCount--;
                            System.Threading.Thread.Sleep(50);
                            rtn = this.RunTestSequence(true);
                            this._repeatTestIndexs.Add((int)this._sysResultItem[(int)ESysResultItem.TEST].Value);
                            Console.WriteLine("[ Spetrometer State Check] , Die Index = " + this._sysResultItem[(int)ESysResultItem.TEST].Value.ToString() + " / " + this._repeatTestIndexs.Count.ToString());
                        }

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
                        this._acquireData.ChipInfo.IsSortTestOK = this.OpenShortIFTest(true, true);
                        break;
                    //---------------------------------------------------------
                    case (int)ETesterKernelCmd.OpenTestIF:
                        this._acquireData.ChipInfo.IsOpenTestOK = this.OpenShortIFTest(true, false);
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
                    case (int)ETesterKernelCmd.RunDeviceVerify:
                        this.RunDeviceVerify();
                        break;
                    //---------------------------------------------------------
                    case (int)ETesterKernelCmd.StopTest:
                        this.EndTestSequence();
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
