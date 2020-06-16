using System;
using System.Collections.Generic;
using MPI.Tester.DeviceCommon;
using System.Text;
using System.Xml;
using System.IO;
using System.Linq;

using System.Threading;
using System.Diagnostics;

using MPI.Tester.Maths;

using MPI.Tester.Data;

using MPI.Tester.Device.SourceMeter.KeithleySMU;

namespace MPI.Tester.Device.SourceMeter
{
    public class Keithley2600 : ISourceMeter
    {
        #region >>> Private Proberty <<<

        private const int IO_QTY = 14;

        private DevAssignment _devAsgmt;
        private ElecDevSetting _elecDevSetting;
        private EDevErrorNumber _errorNum;
        private ElectSettingData[] _elcSetting;

        private const double MAX_DELAY_TIME = 999000.0d;	// Unit: ms
        private const double MIN_DELAY_TIME = 0.0d;	// Unit: ms

        private List<double[][]> _applyData;
        private List<double[][]> _acquireData;
        private List<double[][]> _timeChain;
        private List<double[][]> _sweepResult;

        private SavitzkyGolayFilter2 SGFilterTool;
        private uint _sgWindows = 50;

        private SourceMeterSpec _spec;
        private double _pdDarkCurrent;
        private double _pdDarkCurrent2;

        private PerformanceTimer _delayTimer;
        int _pdMonitorSmuId = -1;
        string _pdMoniterSMUCh = "A";

        #endregion

        #region >>> Constructor / Disposor <<<

        public Keithley2600()
        {
            this._errorNum = EDevErrorNumber.Device_NO_Error;

            this._acquireData = new List<double[][]>();

            this._timeChain = new List<double[][]>();

            this._sweepResult = new List<double[][]>();

            this._applyData = new List<double[][]>();

            this.SGFilterTool = new SavitzkyGolayFilter2(this._sgWindows, this._sgWindows, 3, 0, 1);

            this._pdDarkCurrent = 0.0d;
            this._pdDarkCurrent2 = 0.0d;

            this._devAsgmt = new DevAssignment();

            this._delayTimer = new PerformanceTimer();
        }

        public Keithley2600(ElecDevSetting setting)
            : this()
        {
            setting.IsDevicePeakFiltering = false;

            this._elecDevSetting = setting;


            if (this._elecDevSetting.IOSetting == null || this._elecDevSetting.IOSetting.IOList == null || this._elecDevSetting.IOSetting.IOList.Count == 0)
            {
                this._elecDevSetting.IOSetting = new IOConfigData(IO_QTY);

                K2600Wrapper.SetDefaultIOConfig(this._elecDevSetting.IOSetting, IO_QTY);//
            }

            this._elecDevSetting.IOSetting.SetIOQty(IO_QTY);//20171228 David


            switch (this._elecDevSetting.DAQModel)
            {
                case EDAQModel.NONE:
                    {
                        this._devAsgmt.DAQ = null;

                        break;
                    }
                case EDAQModel.DAQ9527:
                    {
                        this._devAsgmt.DAQ = new DAQ9527();

                        break;
                    }
                case EDAQModel.PCI9222:
                    {
                        this._devAsgmt.DAQ = new PCI9222();

                        break;
                    }
                case EDAQModel.PCI9111HR:
                    {
                        this._devAsgmt.DAQ = new PCI9111HR();

                        break;
                    }
                case EDAQModel.DAQ2213:
                    {
                        this._devAsgmt.DAQ = new DAQ2213();

                        break;
                    }
                //case EDAQModel.NI4461:
                //case EDAQModel.NI6280:
                //    {
                //        this._daqCard = new NIDAQ(this._elecDevSetting.DAQModel.ToString());

                //        break;
                //    }
                default:
                    {
                        this._devAsgmt.DAQ = null;

                        break;
                    }
            }
        }

        #endregion

        #region >>> Public Proberty <<<

        public string SerialNumber
        {
            get { return this._devAsgmt[DevDefine.MASTER].SerialNumber; }
        }

        public string SoftwareVersion
        {
            get { return this._devAsgmt[DevDefine.MASTER].SoftwareVersion; }
        }

        public string HardwareVersion
        {
            get { return "Keithley " + this._devAsgmt[DevDefine.MASTER].HardwareVersion; }
        }

        public EDevErrorNumber ErrorNumber
        {
            get { return _errorNum; }
        }

        public ElectSettingData[] ElecSetting
        {
            get
            {
                if (this._elcSetting == null)
                {
                    return null;
                }

                ElectSettingData[] data = new ElectSettingData[this._elcSetting.Length];

                for (int i = 0; i < this._elcSetting.Length; i++)
                {
                    data[i] = this._elcSetting[i].Clone() as ElectSettingData;
                }

                return data;
            }
        }

        public SourceMeterSpec Spec
        {
            get { return this._spec; }
        }

        #endregion

        #region >>> Private Method <<<

        private double[] CalcTHYStartIndex(double[] data, int maxShiftPoint, out int shiftPoint)
        {
            if (data == null || data.Length == 0 || maxShiftPoint >= data.Length)
            {
                shiftPoint = 0;

                return data;
            }

            shiftPoint = 0;

            double sum = 0;

            for (int i = 0; i < maxShiftPoint; i++)
            {
                sum += data[i];

                double avg = sum / (i + 1);

                if (data[i + 1] > 1000 * avg)
                {
                    shiftPoint = i;

                    break;
                }
            }

            for (int i = 0; i < data.Length - shiftPoint; i++)
            {
                data[i] = data[i + shiftPoint];
            }

            return data;
        }

        private double[] MovingAverage(double[] data, int windows)
        {
            if (data == null || data.Length == 0 || windows <= 0)
            {
                return data;
            }

            double sum = 0;

            int index = 0;

            int startIndex = 0;

            for (int i = 0; i < data.Length; i++)
            {
                sum += data[i];

                double avg = sum / (i + 1);

                if (data[i] > 3 * avg)
                {
                    index = i;

                    break;
                }
            }

            double[] afterFilter = new double[data.Length];

            int maxIndex = Array.IndexOf(data, data.Max());

            int minIndex = Array.IndexOf(data, data.Min());

            int targerIndex = maxIndex > minIndex ? maxIndex : minIndex;

            for (int i = index + 1; i < data.Length; i++)
            {
                if (i == targerIndex)
                {
                    startIndex = i;

                    break;
                }
                else if (i > 0 && (Math.Abs(data[i]) > Math.Abs(data[i - 1])) && (Math.Abs(data[i]) > Math.Abs(data[i + 1])))
                {
                    startIndex = i;

                    break;
                }
            }

            for (int i = 0; i < data.Length; i++)
            {
                if (i >= startIndex && i < data.Length - 1 - windows)
                {
                    double sumWindows = 0;

                    for (int j = 0; j <= windows - 1; j++)
                    {
                        sumWindows += data[i + j];
                    }

                    afterFilter[i] = sumWindows / windows;
                }
                else
                {
                    afterFilter[i] = data[i];
                }
            }

            return afterFilter;
        }

        private bool SetPDDarkSampleTestItem()
        {
            if (this._elecDevSetting.DetectorMsrtDevice == EPDSensingMode.SrcMeter_2nd)
            {
                ElectSettingData elec2nd = new ElectSettingData();

                elec2nd.MsrtType = EMsrtType.FVMI;
                elec2nd.ForceUnit = EVoltUnit.V.ToString();
                elec2nd.ForceDelayTime = 0.0d;
                elec2nd.ForceTime = 0.0d;
                elec2nd.ForceValue = 0.0d;

                elec2nd.ForceTimeUnit = ETimeUnit.ms.ToString();

                elec2nd.MsrtRange = 1e-6;
                elec2nd.MsrtProtection = 1e-6;

                elec2nd.MsrtFilterCount = 0;
                elec2nd.MsrtUnit = EAmpUnit.A.ToString();

                elec2nd.MsrtNPLC = 1;

                if (!this._devAsgmt[DevDefine.SLAVE_PD].SetTestItemScripts_Slave(9999, elec2nd))
                {
                    this._errorNum = EDevErrorNumber.ParameterSetting_Err;

                    return false;
                }
            }

            return true;
        }

        private void CalculateTHY(uint channel, uint index, string[] strResultData)
        {
            double[] data = new double[strResultData.Length];

            for (int i = 0; i < strResultData.Length; i++)
            {
                Double.TryParse(strResultData[i], out data[i]);
            }

            this.CalculateTHY(channel, index, data);
        }

        private void CalculateTHY(uint channel, uint index, double[] resultData)
        {
            //////////////////////////////////////////////////////////////////////////////////////////////////////
            // (0) 去除負數
            //////////////////////////////////////////////////////////////////////////////////////////////////////
            for (int i = 0; i < resultData.Length; i++)
            {
                if (this._elcSetting[index].ForceValue > 0)
                {
                    if (resultData[i] < 0)
                    {
                        resultData[i] = 0.00d;
                    }
                }
                else
                {
                    if (resultData[i] > 0)
                    {
                        resultData[i] = 0.00d;
                    }
                }
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////
            // (1) SG Filter
            //////////////////////////////////////////////////////////////////////////////////////////////////////
            if (this._elcSetting[index].ThySGFilterCount > 0)
            {
                int halfWindow = (this._elcSetting[index].ThySGFilterCount - 1) / 2;

                double[] temp = new double[resultData.Length + halfWindow * 2];

                Array.Copy(resultData, 0, temp, halfWindow, resultData.Length);

                for (int i = 0; i < halfWindow; i++)
                {
                    temp[i] = temp[halfWindow];

                    temp[temp.Length - i - 1] = temp[temp.Length - halfWindow - 1];
                }

                temp = MPIFilter.DoSavitzkyGolay(temp, (uint)this._elcSetting[index].ThySGFilterCount, 3);

                for (int i = 1; i < resultData.Length - 1; i++)
                {
                    resultData[i] = temp[i + halfWindow];
                }
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////
            // (2) Moving Average
            //////////////////////////////////////////////////////////////////////////////////////////////////////
            resultData = this.MovingAverage(resultData, this._elcSetting[index].ThyMovingAverageWindow);

            //////////////////////////////////////////////////////////////////////////////////////////////////////
            // (3) Interpolation
            //////////////////////////////////////////////////////////////////////////////////////////////////////
            if (this._devAsgmt.DAQ == null)
            {
                resultData[0] = 0.0d;

                for (int i = 1; i < resultData.Length - 2; i++)
                {
                    resultData[i] = resultData[i + 1];
                }

                resultData = this.Interpolation(resultData);
            }
            else
            {
                //50: Max Shift Point
                int shiftPoint = 0;

                this.CalcTHYStartIndex(resultData, 50, out shiftPoint);

                //for (int i = 0; i < 50; i++)
                //{
                //    if (resultData[i + 1] > resultData[i] * 2 && resultData[i + 1] > 0.2d)
                //    {
                //        resultData[i] = 0.0d;

                //        shiftPoint = i;

                //        break;
                //    }
                //}

                //for (int i = 0; i < resultData.Length - shiftPoint; i++)
                //{
                //    resultData[i] = resultData[i + shiftPoint];
                //}
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////
            // (4) Calculate THY
            //////////////////////////////////////////////////////////////////////////////////////////////////////
            for (int i = 0; i < this._acquireData[(int)channel][index].Length; i++)
            {
                this._acquireData[(int)channel][index][i] = resultData[i];
            }

            if (this._elecDevSetting.IsDevicePeakFiltering)
            {
                this._sweepResult[(int)channel][index][(int)ETHYResultItem.MaxPeak] = this._acquireData[(int)channel][index][0];

                this._sweepResult[(int)channel][index][(int)ETHYResultItem.StableValue] = this._acquireData[(int)channel][index][1];

                this._sweepResult[(int)channel][index][(int)ETHYResultItem.MaxToStable] = this._acquireData[(int)channel][index][2];

                this._acquireData[(int)channel][index][0] = 0.0d;

                this._acquireData[(int)channel][index][1] = 0.0d;

                this._acquireData[(int)channel][index][2] = 0.0d;
            }
            else
            {
                // Calculate the stable value
                int startIndex = this._acquireData[(int)channel][index].Length - 1;

                int endIndex = startIndex - 10;

                double sumStable = 0.0d;

                this._sweepResult[(int)channel][index][(int)ETHYResultItem.MaxPeak] = 0.0d;

                this._sweepResult[(int)channel][index][(int)ETHYResultItem.MinPeak] = 0.0d;

                for (int i = 0; i < this._acquireData[(int)channel][index].Length; i++)
                {
                    // Found the Max Peak Value
                    if (Math.Abs(this._acquireData[(int)channel][index][i]) > this._sweepResult[(int)channel][index][(int)ETHYResultItem.MaxPeak])
                    {
                        this._sweepResult[(int)channel][index][(int)ETHYResultItem.MaxPeak] = Math.Abs(this._acquireData[(int)channel][index][i]);
                    }

                    // Found the Min Peak Value
                    if (Math.Abs(this._acquireData[(int)channel][index][i]) < this._sweepResult[(int)channel][index][(int)ETHYResultItem.MinPeak])
                    {
                        this._sweepResult[(int)channel][index][(int)ETHYResultItem.MinPeak] = Math.Abs(this._acquireData[(int)channel][index][i]);
                    }
                }

                // Found the Stable Value
                int count = 0;

                for (count = startIndex; count >= 0 && count > endIndex; count--)
                {
                    sumStable += this._acquireData[(int)channel][index][count];
                }

                if (count != 0)
                {
                    this._sweepResult[(int)channel][index][(int)ETHYResultItem.StableValue] = Math.Abs(sumStable) / 10.0d;
                }
                else if (count == 0 && this._acquireData[(int)channel][index].Length >= 1)
                {
                    this._sweepResult[(int)channel][index][(int)ETHYResultItem.StableValue] = Math.Abs(sumStable) / ((double)this._acquireData[(int)channel][index].Length);
                }

                this._sweepResult[(int)channel][index][(int)ETHYResultItem.MaxToStable] = this._sweepResult[(int)channel][index][(int)ETHYResultItem.MaxPeak] - this._sweepResult[(int)channel][index][(int)ETHYResultItem.StableValue];

                if (this._sweepResult[(int)channel][index][(int)ETHYResultItem.MaxPeak] > this._sweepResult[(int)channel][index][(int)ETHYResultItem.StableValue])
                {
                    this._sweepResult[(int)channel][index][(int)ETHYResultItem.OverShoot] = this._sweepResult[(int)channel][index][(int)ETHYResultItem.MaxPeak] - this._sweepResult[(int)channel][index][(int)ETHYResultItem.StableValue];
                }
                else
                {
                    this._sweepResult[(int)channel][index][(int)ETHYResultItem.OverShoot] = 0.0d;
                }

                // Found the MTHYVDA & MTHYVDB
                if (this._acquireData[(int)channel][index].Length >= 1200)
                {
                    double section1 = 0;

                    double section2 = 0;

                    for (int i = 450; i < 470; i++)
                    {
                        section1 += this._acquireData[(int)channel][index][i];
                    }

                    section1 = Math.Abs(section1 / 20);

                    for (int i = 980; i < 1000; i++)
                    {
                        section2 += this._acquireData[(int)channel][index][i];
                    }

                    section2 = Math.Abs(section2 / 20);

                    double mthyda = Math.Abs(this._sweepResult[(int)channel][index][(int)ETHYResultItem.MinPeak] - section2);

                    double mthydb = Math.Abs(section1 - section2);

                    if (mthyda > mthydb)
                    {
                        this._sweepResult[(int)channel][index][(int)ETHYResultItem.MTHYVDA] = mthyda;

                        this._sweepResult[(int)channel][index][(int)ETHYResultItem.MTHYVDB] = mthydb;
                    }
                    else
                    {
                        this._sweepResult[(int)channel][index][(int)ETHYResultItem.MTHYVDA] = mthydb;

                        this._sweepResult[(int)channel][index][(int)ETHYResultItem.MTHYVDB] = mthyda;
                    }
                }
            }
        }

        private void CalculateTHYTimeChart(uint channel, uint index, ElectSettingData data)
        {
            if (this._devAsgmt.DAQ == null)
            {
                string[] resultStrData = this._devAsgmt[channel].GetTHYTimestanps(index, data);

                double[] time = new double[resultStrData.Length];

                for (int i = 0; i < resultStrData.Length; i++)
                {
                    Double.TryParse(resultStrData[i], out time[i]);

                    time[i] = time[i] * 1000;
                }

                time = this.Interpolation(time);

                this._timeChain[(int)channel][index] = new double[data.SweepContCount];

                for (int i = 0; i < this._timeChain[(int)channel][index].Length; i++)
                {
                    if (i != this._timeChain[(int)channel][index].Length - 1)
                    {
                        this._timeChain[(int)channel][index][i] = time[i];
                    }
                    else
                    {
                        this._timeChain[(int)channel][index][i] = 2 * this._timeChain[(int)channel][index][i - 1] - this._timeChain[(int)channel][index][i - 2];
                    }
                }
            }
            else
            {
                this._timeChain[(int)channel][index] = new double[data.SweepContCount];

                double time = (1000.0d / (double)this._elecDevSetting.DAQSampleRate);

                for (int i = 0; i < this._timeChain[(int)channel][index].Length; i++)
                {
                    this._timeChain[(int)channel][index][i] = time * i;
                }
            }
        }

        private double[] Interpolation(double[] data)
        {
            double[] retData = new double[data.Length * 2];

            for (int i = 0; i < retData.Length; i++)
            {
                if (i % 2 == 0)
                {
                    retData[i] = data[i / 2];
                }
                else
                {
                    if (i == retData.Length - 1)
                    {
                        retData[i] = retData[i - 1];
                    }
                    else
                    {
                        retData[i] = (data[i / 2] + data[i / 2 + 1]) / 2;
                    }
                }
            }

            return retData;
        }

        private bool AcquireMsrtData(uint[] channels, uint index, int terminalIndex = -1)
        {
            //-----------------------------------------------------------------------------------------
            // Acquire Master
            //-----------------------------------------------------------------------------------------
            foreach (var dev in this._devAsgmt.Devices)
            {
                foreach (var smu in dev.SyncTrigSMU)
                {
                    int channel = this._devAsgmt.GetChannel(dev.ID, smu);

                    int smuIndex = this._devAsgmt.GetSmuIndex(dev.Name);

                    EMsrtType type = this._elcSetting[index].MsrtType;

                    if (type == EMsrtType.R)
                    {
                        //if (this._elcSetting[index].IsEnableK26ContactFuction)
                        //{
                        type = EMsrtType.CONTACTCHECK;
                        //}
                    }

                    int msrtDelayTime = (int)(this._elcSetting[index].ForceTime + this._elcSetting[index].ForceDelayTime);

                    switch (type)
                    {
                        #region >>> FI FV LCR <<<

                        case EMsrtType.FI:
                        case EMsrtType.FV:
                        case EMsrtType.LCR:
                            {
                                break;
                            }
                        //-----------------------------------------------------------------------------//
                        #endregion

                        #region >>> FIMV <<<

                        case EMsrtType.FIMV:
                            {
                                string[] strData = dev.AcquireMsrtData(smu, msrtDelayTime);

                                if (strData == null)
                                {
                                    this._errorNum = EDevErrorNumber.SourceMeterDevice_Disconnect_Err;

                                    return false;
                                }

                                if (this._elcSetting[index].IsEnableMsrtForceValue && strData.Length == 2)
                                {
                                    Double.TryParse(strData[1], out this._acquireData[(int)channel][index][0]); // Msrt V

                                    Double.TryParse(strData[0], out this._applyData[(int)channel][index][0]);   // Msrt ForceI
                                }
                                else
                                {
                                    Double.TryParse(strData[0], out this._acquireData[(int)channel][index][0]);
                                }

                                break;
                            }

                        #endregion

                        #region >>> FVMI <<<

                        case EMsrtType.FVMI:
                            {
                                string[] strData = dev.AcquireMsrtData(smu, msrtDelayTime);

                                if (strData == null)
                                {
                                    this._errorNum = EDevErrorNumber.SourceMeterDevice_Disconnect_Err;

                                    return false;
                                }

                                if (this._elcSetting[index].IsEnableMsrtForceValue)
                                {
                                    Double.TryParse(strData[0], out this._acquireData[(int)channel][index][0]); // Msrt V

                                    Double.TryParse(strData[1], out this._applyData[(int)channel][index][0]);   // Msrt ForceI
                                }
                                else
                                {
                                    Double.TryParse(strData[0], out this._acquireData[(int)channel][index][0]);
                                }

                                break;
                            }

                        #endregion

                        #region >>> THY <<<

                        case EMsrtType.THY:
                            {
                                string[] strData = dev.AcquireMsrtData(smu);

                                if (this._devAsgmt.DAQ == null)
                                {
                                    if (strData == null)
                                    {
                                        this._errorNum = EDevErrorNumber.SourceMeterDevice_Disconnect_Err;

                                        return false;
                                    }

                                    this.CalculateTHY((uint)channel, index, strData);
                                }
                                else
                                {
                                    double[] data = this._devAsgmt.DAQ.GetDataFromDAQ((uint)channel, index);

                                    if (data == null)
                                    {
                                        this._errorNum = EDevErrorNumber.DAQAcquireDataTimeout_Err;

                                        return false;
                                    }

                                    this.CalculateTHY((uint)channel, index, data);
                                }

                                break;
                            }
                        //-----------------------------------------------------------------------------//
                        #endregion

                        #region >>> R <<<

                        case EMsrtType.R:
                            {
                                string[] strData = dev.AcquireMsrtData(smu);

                                if (strData == null)
                                {
                                    this._errorNum = EDevErrorNumber.SourceMeterDevice_Disconnect_Err;

                                    return false;
                                }

                                double MAX_DOUBLE_VALUE = 9999999999.0d;

                                // this._acquireData[index][0]: R = V / I
                                this._acquireData[(int)channel][index][0] = Math.Abs(double.Parse(strData[0]) / this._elcSetting[index].ForceValue);

                                if (this._acquireData[(int)channel][index][0] > MAX_DOUBLE_VALUE)
                                {
                                    this._acquireData[(int)channel][index][0] = MAX_DOUBLE_VALUE;
                                }

                                break;
                            }
                        //-----------------------------------------------------------------------------//
                        #endregion

                        #region >>> CONTACTCHECK <<<

                        case EMsrtType.CONTACTCHECK:
                            {
                                string[] strData = dev.AcquireMsrtData(smu);

                                if (strData == null)
                                {
                                    this._errorNum = EDevErrorNumber.SourceMeterDevice_Disconnect_Err;

                                    return false;
                                }

                                double HERESHOLD = 100.0d;

                                double MAX_DOUBLE_VALUE = 9999999999.0d;

                                // this._acquireData[index][2]: MRL
                                this._acquireData[(int)channel][index][2] = Math.Abs(double.Parse(strData[1]));

                                // this._acquireData[index][1]: MRH
                                this._acquireData[(int)channel][index][1] = Math.Abs(double.Parse(strData[0]));

                                if (this._acquireData[(int)channel][index][1] > HERESHOLD)
                                {
                                    this._acquireData[(int)channel][index][1] = MAX_DOUBLE_VALUE;
                                }

                                if (this._acquireData[(int)channel][index][2] > HERESHOLD)
                                {
                                    this._acquireData[(int)channel][index][2] = MAX_DOUBLE_VALUE;
                                }

                                break;
                            }
                        //-----------------------------------------------------------------------------//
                        #endregion

                        #region >>> LIV LVI FVMILOP FIMVLOP <<<

                        case EMsrtType.LIV:
                        case EMsrtType.LVI:
                        case EMsrtType.FVMILOP:
                        case EMsrtType.FIMVLOP:
                            {
                                string[] strData = dev.AcquireMsrtData(smu);

                                if (strData == null)
                                {
                                    this._errorNum = EDevErrorNumber.SourceMeterDevice_Disconnect_Err;

                                    return false;
                                }

                                for (int i = 0; i < strData.Length; i++)
                                {
                                    Double.TryParse(strData[i], out this._acquireData[(int)channel][index][i]);
                                }

                                //-----------------------------------------------------------------.------------------------------
                                // Master/Slave Asynchronous Trigger: (LOP, LIV)
                                // Master Trigger --> Acquire Master MsrtData --> Slave Trigger --> Acquire Slave MsrtData
                                //-----------------------------------------------------------------------------------------------
                                double pdCurr = 0.0d;

                                double pdCnt = 0.0d;

                                double pdCurr2 = 0.0d;

                                double pdCnt2 = 0.0d;

                                if (!this.AcquirerDetectorMsrtData((uint)channel, index, out pdCurr, out pdCnt, out pdCurr2, out pdCnt2))
                                {
                                    return false;
                                }

                                this._acquireData[(int)channel][index][1] = pdCurr;

                                this._acquireData[(int)channel][index][2] = pdCnt;

                                this._acquireData[(int)channel][index][3] = pdCurr2;

                                this._acquireData[(int)channel][index][4] = pdCnt2;

                                break;
                            }
                        //-----------------------------------------------------------------------------//
                        #endregion

                        #region >>> FVMISCAN <<<

                        case EMsrtType.FVMISCAN:
                            {
                                bool isDeviceMsrtTimeStamps = true;

                                double expFt_in_ms = (0.001 + this._elcSetting[index].MsrtNPLC * 1000 / 60) *  (this._elcSetting[index].SweepContCount );

                                if (expFt_in_ms > 2500)//確保長時間量測不會被咬住
                                {
                                    System.Threading.Thread.Sleep((int)(expFt_in_ms - 2000));
                                }

                                string[] strData = dev.AcquireMsrtData(smu);

                                double deviceTime = 0.0d;

                                double msrtData = 0.0d;

                                if (strData == null)
                                {
                                    this._errorNum = EDevErrorNumber.SourceMeterDevice_Disconnect_Err;

                                    return false;
                                }

                                if (isDeviceMsrtTimeStamps)
                                {
                                    for (int i = 0; i < strData.Length / 2; i++)
                                    {
                                        Double.TryParse(strData[i * 2], out msrtData);

                                        Double.TryParse(strData[i * 2 + 1], out deviceTime);

                                        if (Math.Abs(msrtData) < 1e-20)
                                        {
                                            msrtData = 0.0d;
                                        }

                                        this._acquireData[(int)channel][index][i] = msrtData;

                                        this._timeChain[(int)channel][index][i] = Math.Round((deviceTime ), 6, MidpointRounding.AwayFromZero); // s to ms
                                        //this._timeChain[(int)channel][index][i] = Math.Round((deviceTime * 1000.0d), 6, MidpointRounding.AwayFromZero); // s to ms
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < strData.Length; i++)
                                    {
                                        Double.TryParse(strData[i], out msrtData);

                                        if (Math.Abs(msrtData) < 1e-20)
                                        {
                                            msrtData = 0.0d;
                                        }

                                        this._acquireData[(int)channel][index][i] = msrtData;
                                    }
                                }

                                break;
                            }
                        //-----------------------------------------------------------------------------//
                        #endregion

                        #region >>> PIV <<<

                        case EMsrtType.PIV:
                            {
                                double[] iArray = null;
                                double[] vArray = null;
                                double[] pArray = null;

                                int sweepCnt = 0;

                                string[] strData = dev.AcquireMsrtData(smu);

                                sweepCnt = strData.Length / 3;
                                iArray = new double[sweepCnt];
                                vArray = new double[sweepCnt];
                                pArray = new double[sweepCnt];

                                for (int i = 0; i < sweepCnt; i++)
                                {
                                    Double.TryParse(strData[i * 3], out iArray[i]);
                                    Double.TryParse(strData[i * 3 + 1], out vArray[i]);
                                    Double.TryParse(strData[i * 3 + 2], out pArray[i]);
                                }

                                //this._elcSetting[index].SweepCustomValue = iArray;
                                this._acquireData[(int)channel][index] = vArray;
                                this._sweepResult[(int)channel][index] = pArray;

                                break;
                            }
                        //-----------------------------------------------------------------------------//
                        #endregion

                        #region >>> SWEEP <<<

                        case EMsrtType.FVMISWEEP:
                        case EMsrtType.FIMVSWEEP:
                            {
                                double t = 1000 / 60;
                                double autoRangeTime = 0;
                                if (this._elcSetting[index].IsAutoMsrtRange)
                                {
                                    autoRangeTime = 200;
                                }

                                double expFt_in_ms = (this._elcSetting[index].ForceTime + this._elcSetting[index].TurnOffTime + this._elcSetting[index].MsrtNPLC * t + autoRangeTime) *
                                    (this._elcSetting[index].SweepContCount + this._elcSetting[index].SweepRiseCount);


                                //if (expFt_in_ms > 2500)//確保長時間量測不會被咬住
                                //{
                                //    System.Threading.Thread.Sleep((int)(expFt_in_ms - 2000));
                                //}

                                //terminalIndex

                                string[] strData = dev.AcquireMsrtData(smu, (int)expFt_in_ms);

                                if (strData == null)
                                {
                                    this._errorNum = EDevErrorNumber.SourceMeterDevice_Disconnect_Err;

                                    return false;
                                }

                                //Double.TryParse(strData[smuIndex], out this._acquireData[channel][index][0]);

                                List<double> vList = new List<double>();
                                List<double> tList = new List<double>();
                                int len = (int)(this._elcSetting[index].SweepContCount + this._elcSetting[index].SweepRiseCount);
                                for (int i = 0; i < len; i++)
                                {
                                    double val = 0;
                                    double.TryParse(strData[i*2], out val);
                                    vList.Add(val);
                                    double tval = 0;
                                    double.TryParse(strData[i*2+1], out tval);
                                    tList.Add(tval);
                                }
                                this._acquireData[channel][index] = vList.ToArray();
                                this._timeChain[channel][index] = tList.ToArray();
                                break;
                            }
                            
                        #endregion


                        //System.Threading.Thread.Sleep((int)delayTime);
                        #region >>> default <<<

                        default:
                            {
                                string[] strData = dev.AcquireMsrtData(smu, msrtDelayTime);

                                if (strData == null)
                                {
                                    this._errorNum = EDevErrorNumber.SourceMeterDevice_Disconnect_Err;

                                    return false;
                                }

                                //Double.TryParse(strData[smuIndex], out this._acquireData[channel][index][0]);

                                for (int i = 0; i < strData.Length; i++)
                                {
                                    Double.TryParse(strData[i], out this._acquireData[channel][index][i]);
                                }

                                break;
                            }
                        #endregion
                    }

                    if (this._errorNum != EDevErrorNumber.Device_NO_Error)
                    {
                        GlobalFlag.IsSourceMeterDisconnect = true;
                    }
                }
            }

            ////-----------------------------------------------------------------------------------------
            //// Acquire PD
            ////-----------------------------------------------------------------------------------------
            //if (this._elecDevSetting.DetectorMsrtDevice == EPDSensingMode.SrcMeter_2nd)
            //{
            //    bool isTrgSmuA = this._elcSetting[index].IsTrigDetector;
            //    bool isTrgSmuB = this._elcSetting[index].IsTrigDetector2;

            //    if (isTrgSmuA && isTrgSmuB)
            //    {
            //        string[] strData = this._devAsgmt[DevDefine.SLAVE_PD].AcquireMsrtDataAtOnce_Dual(EMsrtType.MI, EMsrtType.MI);

            //        double pdCurr = 0.0d;

            //        double pdCurr2 = 0.0d;

            //        double.TryParse(strData[0], out pdCurr);

            //        double.TryParse(strData[1], out pdCurr2);

            //        pdCurr -= this._pdDarkCurrent;

            //        pdCurr2 -= this._pdDarkCurrent2;

            //        this._acquireData[(int)0][index][0] = pdCurr;

            //        this._acquireData[(int)0][index][1] = pdCurr2;
            //    }
            //    else if (isTrgSmuB)
            //    {
            //        string[] strData = this._devAsgmt[DevDefine.SLAVE_PD].AcquireMsrtDataAtOnce_B(EMsrtType.MI);

            //        double pdCurr2 = 0.0d;

            //        double.TryParse(strData[0], out pdCurr2);

            //        pdCurr2 -= this._pdDarkCurrent2;

            //        this._acquireData[(int)0][index][1] = pdCurr2;
            //    }
            //    else if (isTrgSmuA)
            //    {
            //        string[] strData = this._devAsgmt[DevDefine.SLAVE_PD].AcquireMsrtDataAtOnce(EMsrtType.MI);

            //        double pdCurr = 0.0d;

            //        double.TryParse(strData[0], out pdCurr);

            //        pdCurr -= this._pdDarkCurrent;

            //        this._acquireData[(int)0][index][0] = pdCurr;
            //    }
            //}

            return true;
        }



        private double CalcPDCnt(double range, double pdCurr)
        {
            double pdCnt = 0.0d;

            pdCurr = Math.Abs(pdCurr);

            if (pdCurr >= range)
            {
                pdCnt = 1.0d;
            }
            else
            {
                pdCnt = (pdCurr / range);
            }

            return pdCnt;
        }

        private bool AcquirerDetectorMsrtData(uint id, uint index, out double pdCurr, out double pdCnt, out double pdCurr2, out double pdCnt2)
        {
            pdCurr = 0.0d;
            pdCnt = 0.0d;

            pdCurr2 = 0.0d;
            pdCnt2 = 0.0d;

            if (this._elecDevSetting.DetectorMsrtDevice == EPDSensingMode.SrcMeter_2nd &&
                (this._elcSetting[index].IsTrigDetector || this._elcSetting[index].IsTrigDetector2))
            {
                //--------------------------------------------------------------------------------
                // AcquireData
                string[] strData = null;

                if (!this._elecDevSetting.IsDetectorHwTrig)
                {
                    // S/W Trigger

                    if (this._elcSetting[index].IsTrigDetector && this._elcSetting[index].IsTrigDetector2)
                    {
                        strData = this._devAsgmt[DevDefine.SLAVE_PD].AcquireMsrtDataAtOnce_Dual(EMsrtType.MI, EMsrtType.MI);
                    }
                    else if (this._elcSetting[index].IsTrigDetector)
                    {
                        strData = this._devAsgmt[DevDefine.SLAVE_PD].AcquireMsrtDataAtOnce(EMsrtType.MI);
                    }
                    else if (this._elcSetting[index].IsTrigDetector)
                    {
                        strData = this._devAsgmt[DevDefine.SLAVE_PD].AcquireMsrtDataAtOnce_B(EMsrtType.MI);
                    }
                }
                else
                {
                    // H/W Trigger
                    strData = this._devAsgmt[DevDefine.SLAVE_PD].AcquireMsrtData();
                }
                //--------------------------------------------------------------------------------

                if (strData == null)
                {
                    this._errorNum = EDevErrorNumber.SourceMeterDevice_Disconnect_Err;

                    return false;
                }

                if (this._elcSetting[index].IsTrigDetector && this._elcSetting[index].IsTrigDetector2)
                {
                    Double.TryParse(strData[0], out pdCurr);

                    Double.TryParse(strData[1], out pdCurr2);

                    pdCurr = pdCurr - this._pdDarkCurrent;

                    pdCurr2 = pdCurr2 - this._pdDarkCurrent2;

                    pdCnt = this.CalcPDCnt(this._elcSetting[index].DetectorMsrtRange, pdCurr);

                    pdCnt2 = this.CalcPDCnt(this._elcSetting[index].DetectorMsrtRange2, pdCurr2);
                }
                else if (this._elcSetting[index].IsTrigDetector)
                {
                    Double.TryParse(strData[0], out pdCurr);

                    pdCurr = pdCurr - this._pdDarkCurrent;

                    pdCnt = this.CalcPDCnt(this._elcSetting[index].DetectorMsrtRange, pdCurr);
                }
                else if (this._elcSetting[index].IsTrigDetector2)
                {
                    Double.TryParse(strData[0], out pdCurr2);

                    pdCurr2 = pdCurr2 - this._pdDarkCurrent2;

                    pdCnt2 = this.CalcPDCnt(this._elcSetting[index].DetectorMsrtRange2, pdCurr2);
                }

                //this._devAsgmt[DevDefine.SLAVE_PD].TurnOff();
            }
            else if (this._elecDevSetting.DetectorMsrtDevice == EPDSensingMode.SrcMeter_SMUB && this._elcSetting[index].IsTrigDetector)
            {
                pdCurr = this._acquireData[(int)id][index][1];

                pdCnt = this.CalcPDCnt(this._elcSetting[index].DetectorMsrtRange, pdCurr);
            }

            return true;
        }

        private bool IsSupportPulseModeItem(ElectSettingData item)
        {
            if (item.IsPulseMode)
            {
                string type = item.KeyName.Remove(item.KeyName.IndexOf('_'));

                if (type == "IF" || type == "IFWLA" || type == "IFWLB" || type == "PIV")
                {
                    return true;
                }
            }

            return false;
        }


        private bool CheckSwitchCardStatus()
        {
            return true;
        }

        private void DelayTime(double delayTime)
        {
            if (delayTime >= 100.0d)
            {
                System.Threading.Thread.Sleep((int)delayTime);
            }
            else
            {
                this._delayTimer.Start();
                do
                {
                    if (this._delayTimer.PeekTimeSpan(ETimeSpanUnit.MilliSecond) >= delayTime)
                    {
                        this._delayTimer.Stop();
                        this._delayTimer.Reset();
                        return;
                    }
                    System.Threading.Thread.Sleep(0);
                } while (this._delayTimer.PeekTimeSpan(ETimeSpanUnit.MilliSecond) < delayTime);
                this._delayTimer.Stop();
                this._delayTimer.Reset();
            }
        }

        #endregion

        #region >>> Public Method <<<

        public bool Init(int deviceNum, string sourceMeterSN)
        {
            switch (this._elecDevSetting.SrcTriggerMode)
            {
                case ESMUTriggerMode.Single:
                    {
                        //----------------------------------------------------------------------------------------
                        // (1) Init Source Meter
                        //----------------------------------------------------------------------------------------
                        this._devAsgmt.Add(new SmuAssignmentData(DevDefine.MASTER, "K2600", K2600Const.NAME_SMUA, sourceMeterSN));

                        // 第二台 SrcMeter 指定為 PD Detector
                        if (this._elecDevSetting.DetectorMsrtDevice == EPDSensingMode.SrcMeter_2nd)
                        {
                            this._devAsgmt.Add(new SmuAssignmentData(DevDefine.SLAVE_PD, this._elecDevSetting.DetectorDeviceIP));
                        }
                        else if (this._elecDevSetting.DetectorMsrtDevice == EPDSensingMode.SrcMeter_SMUB)
                        {
                            this._devAsgmt.Add(new SmuAssignmentData(DevDefine.SLAVE_PD, "K2600", K2600Const.NAME_SMUB, sourceMeterSN));
                        }

                        // 第三台 SrcMeter 指定為 LCR DC Bias
                        bool isInitSLAVE = this._elecDevSetting.SourceMeterModel == ESourceMeterModel.K2600 & this._elecDevSetting.LCRDCBiasType == ELCRDCBiasType.Ext_Other & this._elecDevSetting.LCRDCBiasSource == ELCRDCBiasSource.K2600;

                        isInitSLAVE |= this._elecDevSetting.SourceMeterModel == ESourceMeterModel.K2600 & this._elecDevSetting.LCRDCBiasType == ELCRDCBiasType.Other & this._elecDevSetting.LCRDCBiasSource == ELCRDCBiasSource.K2600;

                        if (isInitSLAVE)
                        {
                            this._devAsgmt.Add(new SmuAssignmentData(DevDefine.SLAVE_LCR, this._elecDevSetting.LCRDCBiasSourceIP));
                        }

                        break;
                    }
                case ESMUTriggerMode.PMDT:  // Parallel Multi-Die
                case ESMUTriggerMode.Multiple: // Terminal
                    {
                        foreach (var devSetting in this._elecDevSetting.Assignment)//Assignment)
                        {
                            string model = devSetting.Model;

                            string smu = devSetting.SMU;

                            string ipAddr = devSetting.ConnectionPort;

                            if (smu == string.Empty)
                            {
                                smu = K2600Const.NAME_SMUA;
                            }

                            this._devAsgmt.Add(new SmuAssignmentData(DevDefine.MASTER, model, smu, ipAddr));
                        }

                        if (this._elecDevSetting.DetectorMsrtDevice == EPDSensingMode.SrcMeter_2nd)
                        {
                            if (this._elecDevSetting.IsPDDualChannel)
                            {
                                this._devAsgmt.Add(new SmuAssignmentData(DevDefine.SLAVE_PD, "K2600", K2600Const.NAME_SMUA, this._elecDevSetting.DetectorDeviceIP));
                                this._devAsgmt.Add(new SmuAssignmentData(DevDefine.SLAVE_PD, "K2600", K2600Const.NAME_SMUB, this._elecDevSetting.DetectorDeviceIP));
                            }
                            else
                            {
                                this._devAsgmt.Add(new SmuAssignmentData(DevDefine.SLAVE_PD, this._elecDevSetting.DetectorDeviceIP));
                            }
                        }

                        break;
                    }
                default:
                    return false;
            }


            //----------------------------------------------------------------------------------------
            // (2) Init DAQ Card
            //----------------------------------------------------------------------------------------
            DAQSettingData data = new DAQSettingData();

            data.CardNumber = 0;

            data.SampleRate = (uint)this._elecDevSetting.DAQSampleRate;

            if (this._elecDevSetting.IsEnableRTH && this._devAsgmt.DAQ == null)
            {
                this._errorNum = EDevErrorNumber.DAQDevice_Init_Err;

                return false;
            }

            

            if (!this._devAsgmt.CreateAssignmentTable(this._elecDevSetting))
            {
                this._errorNum = EDevErrorNumber.SourceMeterDevice_Init_Err;

                return false;
            }

            //_devAsgmt[DevDefine.MASTER].
            //_spec

            if (_elecDevSetting.PDMonitorSMU != null &&_elecDevSetting.PDMonitorSMU.ConnectionPort != null && _elecDevSetting.PDMonitorSMU.ConnectionPort != "")
            {
                string ip = _elecDevSetting.PDMonitorSMU.ConnectionPort;
                string smu = _elecDevSetting.PDMonitorSMU.SMU;
                _pdMonitorSmuId = this._devAsgmt.GetDeviceID(_elecDevSetting.PDMonitorSMU.ConnectionPort);
                Console.WriteLine("[K2600],Init PDMonitorSMU.ip: " + ip + ",SMU:" + smu);
                if (_pdMonitorSmuId < 0)
                {
                    Console.WriteLine("[K2600],Init PDMonitorSMU not exist, create new smu link. ");
                    this._devAsgmt.Add(new SmuAssignmentData(DevDefine.SLAVE_PD, "K2600", _elecDevSetting.PDMonitorSMU.SMU, _elecDevSetting.PDMonitorSMU.ConnectionPort));
                    if (!this._devAsgmt.CreateAssignmentTable(this._elecDevSetting))
                    {
                        Console.WriteLine("[K2600],Init PDMonitorSMU initial fail");
                        this._errorNum = EDevErrorNumber.SourceMeterDevice_Init_Err;

                        return false;
                    }
                }

                _pdMoniterSMUCh = _elecDevSetting.PDMonitorSMU.SMU;
            }

            if (this._devAsgmt.DAQ != null)
            {
                if (!this._devAsgmt.DAQ.Init(data, (uint)this._elecDevSetting.Assignment.Count))
                //if(!this._devAsgmt.DAQ.Init(data, (uint)counter))
                {
                    this._errorNum = EDevErrorNumber.DAQDevice_Init_Err;

                    return false;
                }
            }

            //----------------------------------------------------------------------------------------
            
            foreach (var smu in this._devAsgmt.Devices)
            {
                //System.Threading.Thread.Sleep(500);
                if (!smu.Init())
                {
                    this._errorNum = EDevErrorNumber.SourceMeterDevice_Init_Err;

                    return false;
                }

            }

          
            //---------------------------------------------------------------------------------------------------------------------------------
            // Check SMU Control Switch System
            //if (this._elecDevSetting.IsCtrlSwitchSys)
            //{
            //    foreach (var smu in this._devAsgmt.Devices)
            //    {
            //        byte switchCardToSmuA = 0x00;
            //        byte switchCardToSmuB = 0x00;

            //        uint switchCardStatus = smu.DioRead();

            //        if (!smu.IsDualSMU)
            //        {
            //            switchCardToSmuA = (byte)(switchCardStatus & 0x07);

            //            Console.WriteLine(string.Format("[K2600], IP: {0}, Switch Card Type: (SMU-A) = {1}", smu.IpAddress, switchCardToSmuA));

            //        }
            //        else
            //        {
            //            switchCardToSmuA = (byte)(switchCardStatus & 0x07);
            //            switchCardToSmuB = (byte)((switchCardStatus >> 7) & 0x07);

            //            Console.WriteLine(string.Format("[K2600], IP: {0}, Switch Card Type: (SMU-A) = {1}, (SMU-B) = {2}", smu.IpAddress, switchCardToSmuA, switchCardToSmuB));
            //        }
            //    }
            //}

            //---------------------------------------------------------------------------------------------------------------------------------

            //////////////////////////////////////////////////////////////////////////////////////
            // M版 暫不支援 Device Spec
            //////////////////////////////////////////////////////////////////////////////////////
            if (this._devAsgmt.ContainsName(DevDefine.MASTER))
            {
                this._spec = new SourceMeterSpec(this._devAsgmt[DevDefine.MASTER].CurrentRange, this._devAsgmt[DevDefine.MASTER].VoltageRange);
                
            }
            else
            {
                this._spec = new SourceMeterSpec();
            }
            _spec.MaxForceTime = 100000;

            this._spec.IOQty = 14;

            this._spec.IsAutoForceRange = true;

            this._spec.IsSupportedNPLC = true;

            if (this._elecDevSetting.DetectorMsrtDevice == EPDSensingMode.SrcMeter_2nd)
            {
                this._spec.IsSupportedDualDetectorCH = this._devAsgmt[DevDefine.SLAVE_PD].IsDualSMU;

            }

            GlobalFlag.IsSourceMeterDisconnect = false;

            return true;
        }

        public void Close()
        {
            foreach (var dev in this._devAsgmt.Devices)
            {
                dev.Close();
            }

            if (this._devAsgmt.DAQ != null)
            {
                this._devAsgmt.DAQ.Close();
            }
        }

        public void Reset()
        {
            foreach (var dev in this._devAsgmt.Devices)
            {
                dev.Reset();
            }
        }

        public bool SetConfigToMeter(ElecDevSetting devSetting)
        {
            return true;
        }

        public bool SetParamToMeter(ElectSettingData[] eleSetting)
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            // (1) Set Param To DAQ Card
            ////////////////////////////////////////////////////////////////////////////////////////
            if (this._devAsgmt.DAQ != null)
            {
                if (!this._devAsgmt.DAQ.SetParamToDAQ(eleSetting))
                {
                    this._errorNum = EDevErrorNumber.ParameterSetting_Err;

                    return false;
                }
            }

            ////////////////////////////////////////////////////////////////////////////////////////
            // (1) Reset Error Code
            ////////////////////////////////////////////////////////////////////////////////////////
            this._errorNum = EDevErrorNumber.Device_NO_Error;

            if (eleSetting == null || eleSetting.Length == 0)
            {
                return true;
            }

            this._elcSetting = eleSetting;

            ////////////////////////////////////////////////////////////////////////////////////////
            // (2) Create Buffer
            ////////////////////////////////////////////////////////////////////////////////////////
            foreach (var dev in this._devAsgmt.Devices)
            {
                dev.ClearBuffer();
            }

            this._applyData.Clear();

            this._acquireData.Clear();

            this._sweepResult.Clear();

            this._timeChain.Clear();

            for (uint channel = 0; channel < this._devAsgmt.MasterAssignmentCount; channel++)
            {
                this._applyData.Add(new double[eleSetting.Length][]);

                this._acquireData.Add(new double[eleSetting.Length][]);

                this._sweepResult.Add(new double[eleSetting.Length][]);

                this._timeChain.Add(new double[eleSetting.Length][]);
            }

            ////////////////////////////////////////////////////////////////////////////////////////
            // (3) Set Test Item to Meter
            ////////////////////////////////////////////////////////////////////////////////////////
            bool isMsrt = true;

            for (uint channel = 0; channel < this._devAsgmt.MasterAssignmentCount; channel++)
            {
                this._devAsgmt[channel].SetIOToDefaultState();

                uint tarChannel = channel;//為了解決LCR在Multi Die模式會將目標channel強迫轉乘為 SLAVE_LCR ID，進而造成無限循環

                for (uint index = 0; index < this._elcSetting.Length; index++)
                {
                    ElectSettingData item = this._elcSetting[index].Clone() as ElectSettingData;

                    // Check Time Limit
                    if (item.ForceTime > MAX_DELAY_TIME || item.ForceTime < MIN_DELAY_TIME)
                    {
                        return false;
                    }

                    if (item.ForceDelayTime > MAX_DELAY_TIME || item.ForceDelayTime < MIN_DELAY_TIME)
                    {
                        return false;
                    }


                    //  Master test item setting
                    //if (item.MsrtType == EMsrtType.TERMINAL)  // TERMINAL屬特殊測試條件
                    //{
                    //}
                    //else
                    {
                        isMsrt = true;

                        if (item.MsrtType == EMsrtType.THY)
                        {
                            if (this._devAsgmt.DAQ == null)
                            {
                                isMsrt = true;
                            }
                            else
                            {
                                isMsrt = false;
                                if (this._devAsgmt.AssignmentCount < 2)
                                {
                                    item.ForceTime = (item.SweepContCount * 1e-2) + 1.0d;// 一點10us
                                }
                                else
                                {
                                    double share = (double)this._devAsgmt.AssignmentCount * 4.0d / 10.0d;

                                    item.ForceTime = (item.SweepContCount * 1e-2 * share) + 1.0d; // 乘上多ch share的時間
                                }
                            }
                        }
                        else if (item.MsrtType == EMsrtType.LCR)
                        {
                            uint devID = DevDefine.GetID(DevDefine.MASTER);

                            bool isInitSLAVE = this._elecDevSetting.SourceMeterModel == ESourceMeterModel.K2600 & this._elecDevSetting.LCRDCBiasType == ELCRDCBiasType.Ext_Other & this._elecDevSetting.LCRDCBiasSource == ELCRDCBiasSource.K2600;

                            isInitSLAVE |= this._elecDevSetting.SourceMeterModel == ESourceMeterModel.K2600 & this._elecDevSetting.LCRDCBiasType == ELCRDCBiasType.Other & this._elecDevSetting.LCRDCBiasSource == ELCRDCBiasSource.K2600;

                            if (isInitSLAVE)
                            {
                                devID = DevDefine.GetID(DevDefine.SLAVE_LCR);
                            }

                            tarChannel = devID;
                        }

                        // Master Setting
                        if (!this._devAsgmt[tarChannel].SetTestItemScripts_Master(index, item, isMsrt))
                        {
                            this._errorNum = EDevErrorNumber.ParameterSetting_Err;
                            return false;
                        }
                    }

                    // Slave test item setting
                    if (this._elecDevSetting.DetectorMsrtDevice == EPDSensingMode.SrcMeter_2nd)
                    {
                        if (item.IsTrigDetector || item.IsTrigDetector2)
                        {
                            if (!this._devAsgmt[DevDefine.SLAVE_PD].SetTestItemScripts_Slave(index, item))
                            {
                                this._errorNum = EDevErrorNumber.ParameterSetting_Err;

                                return false;
                            }
                        }
                    }

                    #region >>> Create _acquireData / _applyData Array <<<

                    uint acquireCnt = 1;

                    // Create ApplyData
                    this._applyData[(int)tarChannel][index] = new double[1] { item.ForceValue };

                    // Create AcquireData
                    switch (item.MsrtType)
                    {
                        case EMsrtType.FI:
                        case EMsrtType.FV:
                        case EMsrtType.POLAR:
                            {
                                acquireCnt = 1;

                                break;
                            }
                        case EMsrtType.FIMV:
                        case EMsrtType.FVMI:
                            {
                                acquireCnt = 1;

                                if (this.IsSupportPulseModeItem(item))
                                {
                                    acquireCnt = item.PulseCount;
                                }

                                break;
                            }
                        case EMsrtType.FIMVSWEEP:
                        case EMsrtType.FVMISWEEP:
                            {
                                acquireCnt = (uint)Math.Max(item.SweepCustomValue.Length, (item.SweepRiseCount + item.SweepContCount));

                                this._timeChain[(int)tarChannel][index] = new double[acquireCnt];

                                //for (int i = 0; i < (item.SweepRiseCount + item.SweepContCount); i++)
                                //{
                                //    if (i == 0)
                                //    {
                                //        this._timeChain[(int)channel][index][i] = item.ForceTime * 1000;  // ms
                                //    }
                                //    else
                                //    {
                                //        this._timeChain[(int)channel][index][i] = (item.ForceTime + i * (item.ForceTime + item.SweepTurnOffTime)) *1000;  // ms
                                //    }
                                //}

                                item.SweepCustomValue = this._devAsgmt[tarChannel].SweepPoints;

                                this._timeChain[(int)tarChannel][index] = this._devAsgmt[tarChannel].SweepTimeSpan.ToArray();//20190521 David

                                this._elcSetting[index].SweepCustomValue = item.SweepCustomValue;

                                break;
                            }
                        case EMsrtType.THY:
                            {
                                if (!isMsrt)
                                {
                                    item.ForceTime = item.SweepContCount * (1.0d / this._elecDevSetting.DAQSampleRate * 1000.0d) + 1.0d;
                                }

                                this.CalculateTHYTimeChart(tarChannel, index, item);

                                acquireCnt = item.SweepContCount;

                                this._sweepResult[(int)tarChannel][index] = new double[Enum.GetValues(typeof(ETHYResultItem)).Length];

                                double[] sweepCustomValue = new double[acquireCnt];

                                if (!this._elecDevSetting.IsDevicePeakFiltering)
                                {
                                    //for (int i = 0; i < this._acquireData[(int)channel][index].Length; i++)
                                    for (int i = 0; i < acquireCnt; i++)
                                    {
                                        sweepCustomValue[i] = item.ForceValue;
                                    }
                                }

                                item.SweepCustomValue = sweepCustomValue;

                                this._elcSetting[index].SweepCustomValue = sweepCustomValue;

                                break;
                            }
                        case EMsrtType.FIMVLOP:
                        case EMsrtType.FVMILOP:
                        case EMsrtType.LIV:
                        case EMsrtType.LVI:
                            {
                                // DUT_Volt, Ipd-CHA, Icnt-CHA, Ipd-CHB, Ipd-CHB
                                acquireCnt = 5;
                                break;
                            }
                        case EMsrtType.PIV:
                            {
                                acquireCnt = item.SweepRiseCount;

                                this._timeChain[(int)tarChannel][index] = new double[acquireCnt];

                                this._sweepResult[(int)tarChannel][index] = new double[acquireCnt];  // Pow

                                //for (int i = 0; i < acquireCnt; i++)
                                //{
                                //    if (i == 0)
                                //    {
                                //        this._timeChain[(int)channel][index][i] = item.ForceTime * 1000;  // ms
                                //    }
                                //    else
                                //    {
                                //        this._timeChain[(int)channel][index][i] = (item.ForceTime + i * (item.ForceTime + item.SweepTurnOffTime)) * 1000;  // ms
                                //    }
                                //}

                                this._elcSetting[index].SweepCustomValue = this._devAsgmt[tarChannel].SweepPoints.Clone() as double[];

                                break;
                            }
                        case EMsrtType.FVMISCAN:
                            {
                                acquireCnt = item.SweepContCount;

                                double[] sweepCustomValue = new double[acquireCnt];

                                this._timeChain[(int)tarChannel][index] = new double[acquireCnt];

                                for (int i = 0; i < sweepCustomValue.Length; i++)
                                {
                                    sweepCustomValue[i] = item.ForceValue;
                                }

                                item.SweepCustomValue = sweepCustomValue;

                                this._elcSetting[index].SweepCustomValue = sweepCustomValue;

                                break;
                            }
                        //case EMsrtType.TERMINAL:
                        //    {
                        //        int len = Enum.GetNames(typeof(ETRAcquireAddress)).Length;

                        //        if (this._elcSetting[index].IsPulseMode)
                        //        {
                        //            acquireCnt = (uint)len * (this._elcSetting[index].SweepRiseCount);
                        //        }
                        //        else
                        //        {
                        //            acquireCnt = (uint)len;
                        //        }

                        //        break;
                        //    }
                        case EMsrtType.LCR:
                        case EMsrtType.LCRSWEEP:
                            break;
                        case EMsrtType.R:
                        case EMsrtType.CONTACTCHECK:
                            {
                                acquireCnt = 3;
                                break;
                            }
                        case EMsrtType.IO:
                            {
                                acquireCnt = 2;
                                break;
                            }
                        default:
                            {
                                return false;
                            }
                    }

                    this._acquireData[(int)tarChannel][index] = new double[acquireCnt];

                    #endregion

                } // For TestItems
            }  // For deviceID

            return true;
        }

        public bool MeterOutput(uint[] activateChannels, uint index)
        {
            #region >>> Check Error <<<

            if (this._errorNum != EDevErrorNumber.Device_NO_Error)
            {
                return false;
            }

            if (this._elcSetting == null || this._elcSetting.Length == 0 || index > this._elcSetting.Length - 1)
            {
                return false;
            }

            #endregion

            #region >>> Trigger DAQ <<<

            if (this._elcSetting[index].MsrtType == EMsrtType.RTH ||
                this._elcSetting[index].MsrtType == EMsrtType.THY ||
                this._elcSetting[index].MsrtType == EMsrtType.VLR)
            {
                if (this._devAsgmt.DAQ != null)
                {
                    this._devAsgmt.DAQ.SetTrigger(index);
                }
            }

            //-----------------------------------------------------------------------------------------------
            // Multi diode模式時，當測試快速THY時，是由第一台K26發出Trigger訊號給DAQ(包在THY Script)
            // 當CH0無diode時，需直接由第一台K26送出Trigger給DAQ
            //-----------------------------------------------------------------------------------------------
            bool isOnlyDAQ = true;

            isOnlyDAQ &= this._elcSetting[index].MsrtType == EMsrtType.THY; // 只有THY測試項需要

            isOnlyDAQ &= this._devAsgmt.DAQ != null;	// DAQ card必須啟用

            isOnlyDAQ &= !activateChannels.Contains((uint)0); // ch0無diode

            if (isOnlyDAQ)
            {
                this._devAsgmt[DevDefine.MASTER].TriggerDAQ(index);
            }

            #endregion

            //-----------------------------------------------------------------------------------------------
            // SLAVE_PD Trigger:
            //-----------------------------------------------------------------------------------------------
            if (this._elecDevSetting.DetectorMsrtDevice == EPDSensingMode.SrcMeter_2nd &&
                this._elcSetting[index].IsTrigDetector)
            {
                this._devAsgmt[DevDefine.SLAVE_PD].MeterOutput(index);   // H/W Trig, Slave start listening
            }

            //-----------------------------------------------------------------------------------------------
            // Master Parallel Trigger:
            //-----------------------------------------------------------------------------------------------
            this._devAsgmt.SetTrigAssignment(activateChannels);

            //if ((this._elcSetting[index].MsrtType == EMsrtType.FVMISWEEP || 
            //    this._elcSetting[index].MsrtType == EMsrtType.FIMVSWEEP)
            //    && this._elcSetting[index].ElecTerminalSetting != null)
            //{
            //    this._elcSetting[index].IsPulseMode = true;//避開重複取資料

            //    foreach (var dev in this._devAsgmt.Devices)
            //    {
            //        int sleepTime = (int)Math.Round(this._elcSetting[index].ForceDelayTime);
            //        Thread.Sleep(sleepTime);

            //        if (dev.SyncTrigSMU.Count != 0)
            //        {
            //            for (int i = 0; i < this._elcSetting[index].ElecTerminalSetting.Length; ++i)
            //            {
                            
            //                string id = index.ToString() + "_" + i.ToString();
            //                dev.MeterOutput(id);
            //                if (!this.AcquireMsrtData(activateChannels, index,i))
            //                {
            //                    return false;
            //                }
            //            }
            //        }
            //    }

            //}
            //else
            {

                foreach (var dev in this._devAsgmt.Devices)
                {
                    if (dev.SyncTrigSMU.Count != 0)
                    {
                        dev.MeterOutput(index);
                    }
                }
            }

            //if (this._elcSetting[index].IsEnableLongTimeTest)
            //{
            //    return true;
            //}

            //-----------------------------------------------------------------------------------------------
            // AcquireMsrtData:
            //-----------------------------------------------------------------------------------------------

            if (!this._elcSetting[index].IsPulseMode)
            {
                if (!this.AcquireMsrtData(activateChannels, index))
                {
                    return false;
                }
            }

            return true;
        }

        public byte GetIONState(int ioNum,string ip = "")
        {            
            foreach (var dev in this._devAsgmt.Devices)
            {
                if (ip == "" || ip == dev.IpAddress)
                {
                    return dev.GetIONState(ioNum);
                }
            }

            return 0;
        }

        public byte[] GetIONState(string ip = "")
        {
            foreach (var dev in this._devAsgmt.Devices)
            {
                if (ip == "" || ip == dev.IpAddress)
                {
                    return dev.GetIONState();
                }
            }

            return null;
        }

        public void SetIONState(int ioNum, bool SetHight, string ip = "")
        {
            foreach (var dev in this._devAsgmt.Devices)
            {
                if (ip == "" || ip == dev.IpAddress)
                {
                    EIOState ioState = SetHight ? EIOState.HIGH : EIOState.LOW;
                    string str = dev.IOWritebit(ioNum, ioState);
                    dev.SendCommand(str);
                }
            }
        }

        public void SetIOInitState(int ioNum, bool SetHight, string ip = "")
        {
            foreach (var dev in this._devAsgmt.Devices)
            {
                if (ip == "" || ip == dev.IpAddress)
                {
                    EIOState ioState = SetHight ? EIOState.HIGH : EIOState.LOW;
                    string str = dev.SetDefaultIOScript(ioNum, EIOTrig_Mode.TRIG_BYPASS, ioState);
                    dev.SendCommand(str);
                }
            }
        }

        public bool MeterOutput(uint[] activateChannels, uint index, double applyValue)
        {
            double waitTime = 0.0d;
            double forceValue = 0.0d;
            double forceTime = 0.0d;
            double msrtRange = 0.0d;
            double nplc = 0.0d;

            waitTime = Math.Round((this._elcSetting[index].ForceDelayTime / 1000.0d), 6, MidpointRounding.AwayFromZero); // ms=>s 
            forceTime = Math.Round((this._elcSetting[index].ForceTime / 1000.0d), 6, MidpointRounding.AwayFromZero); // ms=>s 
            msrtRange = this._elcSetting[index].MsrtProtection;
            nplc = this._elcSetting[index].MsrtNPLC;

            this._devAsgmt.SetTrigAssignment(activateChannels);

            switch (this._elcSetting[index].MsrtType)
            {
                case EMsrtType.FVMI:
                    {
                        forceValue = Math.Round(applyValue, 6, MidpointRounding.AwayFromZero);

                        foreach (var deviceID in activateChannels)
                        {
                            this._applyData[(int)deviceID][index][0] = forceValue;

                            this._devAsgmt.Devices[deviceID].RunFunctionFVMI(waitTime, forceValue, 0, forceTime, msrtRange, nplc, 1);
                        }

                        break;
                    }
                case EMsrtType.FIMV:
                    {
                        forceValue = Math.Round(applyValue, 12, MidpointRounding.AwayFromZero);

                        foreach (var deviceID in activateChannels)
                        {
                            this._applyData[(int)deviceID][index][0] = forceValue;

                            this._devAsgmt.Devices[deviceID].RunFunctionFIMV(waitTime, forceValue, 0, forceTime, msrtRange, nplc, 1);
                        }


                        break;
                    }
            }

            return this.AcquireMsrtData(activateChannels, index);
        }

        public bool MeterOutput(uint[] activateChannels, uint index, double[] applyValue)
        {
            double waitTime = 0.0d;
            double forceValue = 0.0d;
            double forceTime = 0.0d;
            double msrtRange = 0.0d;
            double nplc = 0.0d;


            waitTime = Math.Round((this._elcSetting[index].ForceDelayTime / 1000.0d), 6, MidpointRounding.AwayFromZero); // ms=>s 
            forceTime = Math.Round((this._elcSetting[index].ForceTime / 1000.0d), 6, MidpointRounding.AwayFromZero); // ms=>s 
            msrtRange = this._elcSetting[index].MsrtProtection;
            nplc = this._elcSetting[index].MsrtNPLC;

            foreach (uint ch in activateChannels)
            {
                applyValue[ch] = Math.Round(applyValue[ch], 12, MidpointRounding.AwayFromZero);
                this._applyData[(int)ch][index][0] = applyValue[ch];
            }

            double fA = 0;
            double fB = 0;

            foreach (var dev in this._devAsgmt.Devices)
            {
                foreach (var smu in dev.SyncTrigSMU)
                {
                    if (smu == K2600Const.NAME_SMUA)
                    {
                        int channel = this._devAsgmt.GetChannel(dev.ID, smu);
                        fA = applyValue[channel];
                    }
                    else if (smu == K2600Const.NAME_SMUB)
                    {
                        int channel = this._devAsgmt.GetChannel(dev.ID, smu);
                        fB = applyValue[channel];
                    }
                }

                switch (this._elcSetting[index].MsrtType)
                {
                    case EMsrtType.FVMI:
                        {
                            if (dev.SyncTrigSMU.Count == 2)
                            {
                                dev.RunFunctionFVMI(waitTime, fA, fB, forceTime, msrtRange, nplc, 0);
                            }
                            else if (dev.SyncTrigSMU.Count == 1)
                            {
                                if (dev.SyncTrigSMU[0] == K2600Const.NAME_SMUA)
                                {
                                    dev.RunFunctionFVMI(waitTime, fA, 0, forceTime, msrtRange, nplc, 1);
                                }
                                else if (dev.SyncTrigSMU[0] == K2600Const.NAME_SMUB)
                                {
                                    dev.RunFunctionFVMI(waitTime, 0, fB, forceTime, msrtRange, nplc, 2);
                                }
                            }
                            break;
                        }
                    case EMsrtType.FIMV:
                        {
                            if (dev.SyncTrigSMU.Count == 2)
                            {
                                dev.RunFunctionFIMV(waitTime, fA, fB, forceTime, msrtRange, nplc, 0);
                            }
                            else if (dev.SyncTrigSMU.Count == 1)
                            {
                                if (dev.SyncTrigSMU[0] == K2600Const.NAME_SMUA)
                                {
                                    dev.RunFunctionFIMV(waitTime, fA, 0, forceTime, msrtRange, nplc, 1);
                                }
                                else if (dev.SyncTrigSMU[0] == K2600Const.NAME_SMUB)
                                {
                                    dev.RunFunctionFIMV(waitTime, fB, 0, forceTime, msrtRange, nplc, 2);
                                }
                            }
                            break;
                        }
                }
            }
            bool result = this.AcquireMsrtData(activateChannels, index);

            return result;

        }

        public double[] GetDataFromMeter(uint channel, uint index)
        {
            if (index > this._elcSetting.Length - 1 || this._elcSetting == null)
            {
                return null;
            }

            return this._acquireData[(int)channel][index];
        }

        public double[] GetApplyDataFromMeter(uint channel, uint index)
        {
            if (this._elcSetting == null || index > this._elcSetting.Length)
            {
                double[] data = new double[0];

                this._errorNum = EDevErrorNumber.SourceMeterIndexSetting_Err;

                return data;
            }

            return this._applyData[(int)channel][index];
        }

        public double[] GetSweepPointFromMeter(uint channel, uint index)
        {
            if (index > this._elcSetting.Length - 1 || this._elcSetting == null)
            {
                return null;
            }

            return this._elcSetting[index].SweepCustomValue;
        }

        public double[] GetSweepResultFromMeter(uint channel, uint index)
        {
            if (index > this._elcSetting.Length - 1 || this._elcSetting == null)
            {
                this._errorNum = EDevErrorNumber.SourceMeterIndexSetting_Err;

                return null;
            }

            return this._sweepResult[(int)channel][index];
        }

        public double[] GetTimeChainFromMeter(uint channel, uint index)
        {
            if (index > this._elcSetting.Length - 1 || this._elcSetting == null)
            {
                this._errorNum = EDevErrorNumber.SourceMeterIndexSetting_Err;

                return null;
            }

            return this._timeChain[(int)channel][index];
        }

        public void TurnOff()
        {
            foreach (var smu in this._devAsgmt.Devices)
            {
                smu.TurnOff();
            }
        }

        public void SetDefaultIO()
        {
            foreach (var smu in this._devAsgmt.Devices)
            {
                smu.SetIOToDefaultScript();
            }
        }

        public void TurnOff(double delay, bool isOpenRelay)
        {
            foreach (var smu in this._devAsgmt.Devices)
            {
                smu.TurnOff(delay, isOpenRelay);

                //smu.TurnOff();
            }
            foreach (var smu in this._devAsgmt.Devices)
            {
                smu.CheckIfFin();

            }
            //CheckIfFin
        }

        public void Output(uint point, bool active)
        {
            this._devAsgmt[DevDefine.MASTER].FilterWheelMoveToTarget(point);
        }

        public byte InputB(uint point)
        {
            uint data = this._devAsgmt[DevDefine.MASTER].DioRead();

            return (byte)((data >> 7) & 0x07);
        }

        public byte Input(uint point)
        {
            throw new NotImplementedException();
        }

        public void TurnOffByChannel(double delay, uint[] activateChannels)
        {
            this._devAsgmt.TurnOff(activateChannels, delay, false);
        }

        public double GetPDDarkSample(int count)
        {
            if (count > 0)
            {
                List<double> dackAvg = new List<double>();

                if (this._devAsgmt.ContainsName(DevDefine.SLAVE_PD))
                {
                    if (this._elecDevSetting.DetectorMsrtDevice == EPDSensingMode.SrcMeter_2nd)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            this._devAsgmt[DevDefine.SLAVE_PD].MeterOutput(9999);

                            string[] raw = this._devAsgmt[DevDefine.SLAVE_PD].AcquireMsrtData();

                            dackAvg.Add(double.Parse(raw[0]));
                        }
                    }
                    else if (_elecDevSetting.DetectorMsrtDevice == EPDSensingMode.SrcMeter_SMUB)
                    {

                    }
                    else
                    { }
                }

                this._pdDarkCurrent = dackAvg.Average();
            }
            else
            {
                this._pdDarkCurrent = 0.0d;
            }

            return this._pdDarkCurrent;
        }

        public void CollectGarbage()
        {
            foreach (var smu in this._devAsgmt.Devices)
            {
                smu.CollectGarbage();
            }
        }

        public bool CheckInterLock()
        {
            return false;
        }
        //public void DioOutput(SourceMeterDioCtrl[] activateChannels)
        //{
        //    if (activateChannels == null)
        //    {
        //        // 若沒有 switch active channel data 的話, 關閉Switch所有通道  
        //        foreach (var dev in this._devAsgmt.Devices)
        //        {
        //            dev.DioWrite(0);
        //        }
        //    }
        //    else
        //    {
        //        this._devAsgmt.OutputIOAssignment(activateChannels);

        //        this.DelayTime(3.0d);
        //    }
        //}

        public bool ESDDetection()
        {
            return this._devAsgmt[DevDefine.MASTER].ESDDetection();
        }

        public double MsrtPdMonitor(ElectSettingData elecS)//PD200專用
        {
            string ch = _pdMoniterSMUCh;
            int id = _pdMonitorSmuId;
            double waitTime = 0.0d;
            double forceValue = 0.0d;
            double forceTime = 0.0d;
            double msrtRange = 0.0d;
            double nplc = 0.0d;

            forceValue = elecS.ForceValue;
            forceTime = Math.Round((elecS.ForceTime / 1000.0d), 6, MidpointRounding.AwayFromZero); // ms=>s 
            msrtRange = elecS.MsrtProtection;
            nplc = elecS.MsrtNPLC;

            switch(ch)
            {
                case "A":
                    this._devAsgmt.Devices[id].RunFunctionFVMI(waitTime, forceValue, 0, forceTime, msrtRange, nplc, 1);
                    break;
                case "B":
                    this._devAsgmt.Devices[id].RunFunctionFVMI(waitTime, 0, forceValue, forceTime, msrtRange, nplc, 2);
                    break;
            }
            

            string[] strArr = this._devAsgmt.Devices[id].AcquireMsrtData(ch);
            double outVal = 0;
            if (strArr != null && strArr.Length >= 0)
            {
                if (double.TryParse(strArr[0], out outVal))
                {
                    return outVal;
                }
            }
            return 0;
        }
        #endregion
    }

    #region >>> Class DevDefine <<<

    internal static class DevDefine
    {
        public const string MASTER = "MASTER";
        public const string SLAVE_PD = "SLAVE_PD";
        public const string SLAVE_RTH = "SLAVE_RTH";
        public const string SLAVE_LCR = "SLAVE_LCR";

        private const uint ID_MASTER = 0;   // Master 0~9
        private const uint ID_SLAVE_PD = 101;
        private const uint ID_SLAVE_RTH = 201;
        private const uint ID_SLAVE_LCR = 202;

        #region >>> Public Method <<<

        public static uint GetID(string name)
        {
            switch (name)
            {
                case SLAVE_PD:
                    {
                        return ID_SLAVE_PD;
                    }
                case SLAVE_RTH:
                    {
                        return ID_SLAVE_RTH;
                    }
                case SLAVE_LCR:
                    {
                        return ID_SLAVE_LCR;
                    }
                default:
                    {
                        return ID_MASTER;
                    }
            }
        }

        #endregion
    }

    #endregion

    #region >>> Class DevAssignment <<<

    internal class DevAssignment
    {
        private List<SmuAssignmentData> _assignmentData;  　      // List 的 Order 直接對應到 Channel
        private Dictionary<uint, K2600Wrapper> _devResource;  // key = IP; Value = K2600Wrapper 
        private IDAQ _daqCard;

        private int _masterCount;

        public DevAssignment()
        {
            this._assignmentData = new List<SmuAssignmentData>();
            this._devResource = new Dictionary<uint, K2600Wrapper>();
        }

        #region >>> Public Property <<<

        public int AssignmentCount
        {
            get { return this._assignmentData.Count; }
        }

        public int MasterAssignmentCount
        {
            get { return this._masterCount; }
        }

        public IDAQ DAQ
        {
            get { return this._daqCard; }
            set { this._daqCard = value; }
        }

        public K2600Wrapper this[string devAsgmtName]
        {
            get
            {
                uint id = DevDefine.GetID(devAsgmtName);

                if (this._devResource.ContainsKey(id))
                {
                    return this._devResource[id];
                }
                else
                {
                    return null;
                }
            }
        }

        public K2600Wrapper this[uint channel]
        {
            get
            {
                uint id = this._assignmentData[(int)channel].DeviceID;

                if (this._devResource.ContainsKey(id))
                {
                    return this._devResource[id];
                }
                else
                {
                    return null;
                }
            }
        }

        public K2600Wrapper[] Devices
        {
            get
            {
                if (this._devResource.Count != 0)
                {
                    return this._devResource.Values.ToArray();
                }

                return null;
            }
        }

        #endregion

        #region >>> Private Method <<<
        
        private void MeterOutputDCMode(Dictionary<ESMU, ElectSettingData> smuDic, ElectSettingData pdItem)
        {
            foreach (var dev in this._devResource)
            {
                dev.Value.SyncTrigSMU.Clear();
            }

            foreach (var smu in smuDic)
            {
                int ch = (int)smu.Key;

                uint devID = this._assignmentData[(int)ch].DeviceID;

                string smuName = this._assignmentData[(int)ch].SMU;

                this._devResource[devID].SyncTrigSMU.Add(smuName);
            }

            foreach (var dev in this._devResource)
            {
                if (dev.Value.SyncTrigSMU.Count == 1)
                {
                    string smuName = dev.Value.SyncTrigSMU[0];

                    int ch = this.GetChannel(dev.Key, smuName);

                    ElectSettingData item = smuDic[(ESMU)ch];

                    if (smuName == K2600Const.NAME_SMUA)
                    {
                        dev.Value.SetTerminalDCModeScriptSMUA(item);
                    }
                    else if (smuName == K2600Const.NAME_SMUB)
                    {
                        dev.Value.SetTerminalDCModeScriptSMUB(item);
                    }
                }
                else if (dev.Value.SyncTrigSMU.Count == 2)
                {
                    string smuAName = dev.Value.SyncTrigSMU[0];

                    string smuBName = dev.Value.SyncTrigSMU[1];

                    int chA = this.GetChannel(dev.Key, smuAName);

                    int chB = this.GetChannel(dev.Key, smuBName);

                    ElectSettingData itemA = smuDic[(ESMU)chA];

                    ElectSettingData itemB = smuDic[(ESMU)chB];

                    if (smuAName == K2600Const.NAME_SMUB && smuBName == K2600Const.NAME_SMUA)
                    {
                        ElectSettingData tmp = itemA;

                        itemA = itemB;

                        itemB = tmp;
                    }

                    dev.Value.SetTerminalDCModeScriptDual(itemA, itemB);
                }
            }

            foreach (var dev in this._devResource)
            {
                if (dev.Value.SyncTrigSMU.Count == 0)
                {
                    continue;
                }

                dev.Value.MeterOutTerminalScript();
            }

            if (pdItem.IsTrigDetector || pdItem.IsTrigDetector2)
            {
                uint id = DevDefine.GetID(DevDefine.SLAVE_PD);

                if (this._devResource.ContainsKey(id))
                {
                    this._devResource[id].SetTerminalDetector_Slave(pdItem);

                    this._devResource[id].MeterOutTerminalPDScript();
                }
            }
        }

        private void MeterOutputPulseMode(Dictionary<ESMU, ElectSettingData> smuDic, ElectSettingData pdItem)
        {
            uint syschroneziPin = 0;

            List<uint> monitorPin = new List<uint>();

            foreach (var dev in this._devResource)
            {
                dev.Value.SyncTrigSMU.Clear();
            }

            bool isFirstSMU = true;

            foreach (var smu in smuDic)
            {
                int ch = (int)smu.Key;

                uint devID = this._assignmentData[(int)ch].DeviceID;

                uint devPin = devID + 1;

                string smuName = this._assignmentData[(int)ch].SMU;

                this._devResource[devID].SyncTrigSMU.Add(smuName);

                if (isFirstSMU)
                {
                    syschroneziPin = devPin;

                    isFirstSMU = false;
                }
                else if (devPin != syschroneziPin)
                {
                    monitorPin.Add(devPin);
                }
            }

            if (pdItem.IsTrigDetector || pdItem.IsTrigDetector2)
            {
                uint devID = DevDefine.GetID(DevDefine.SLAVE_PD);

                uint devPin = (uint)this._devResource.Count;

                if (this._devResource.ContainsKey(devID))
                {
                    monitorPin.Add(devPin);
                }

                foreach (var data in this._assignmentData)
                {
                    if (data.DeviceID != DevDefine.GetID(DevDefine.SLAVE_PD))
                    {
                        continue;
                    }

                    if ((pdItem.IsTrigDetector && data.SMU == K2600Const.NAME_SMUA) || (pdItem.IsTrigDetector2 && data.SMU == K2600Const.NAME_SMUB))
                    {
                        this._devResource[devID].SyncTrigSMU.Add(data.SMU);
                    }
                }
            }

            foreach (var dev in this._devResource)
            {
                uint devPin = dev.Key + 1;

                if (dev.Value.SyncTrigSMU.Count == 1)
                {
                    string smuName = dev.Value.SyncTrigSMU[0];

                    int ch = this.GetChannel(dev.Key, smuName);

                    ElectSettingData item = null;

                    if (dev.Key == DevDefine.GetID(DevDefine.SLAVE_PD) && (pdItem.IsTrigDetector || pdItem.IsTrigDetector2))
                    {
                        item = pdItem.Clone() as ElectSettingData;

                        //item.TermianlFuncType = ETermianlFuncType.Bias;

                        item.MsrtType = EMsrtType.FVMI;

                        item.SweepStart = pdItem.DetectorBiasValue;

                        item.SweepStop = pdItem.DetectorBiasValue;

                        item.MsrtProtection = pdItem.DetectorMsrtRange;

                        devPin = (uint)this._devResource.Count;
                    }
                    else
                    {
                        item = smuDic[(ESMU)ch];
                    }

                    if (smuName == K2600Const.NAME_SMUA)
                    {
                        if (smuDic.Count == 1)
                        {
                            dev.Value.SetTerminalPulseModeScriptSMUA(item, EK2600IOTriggerSynMode.Single, devPin, 0, null);
                        }
                        else
                        {
                            if (devPin == syschroneziPin)
                            {
                                dev.Value.SetTerminalPulseModeScriptSMUA(item, EK2600IOTriggerSynMode.Master, devPin, syschroneziPin, monitorPin.ToArray());
                            }
                            else
                            {
                                dev.Value.SetTerminalPulseModeScriptSMUA(item, EK2600IOTriggerSynMode.Slave, devPin, syschroneziPin, null);
                            }
                        }
                    }
                    else if (smuName == K2600Const.NAME_SMUB)
                    {
                        if (smuDic.Count == 1)
                        {
                            dev.Value.SetTerminalPulseModeScriptSMUB(item, EK2600IOTriggerSynMode.Single, devPin, 0, null);
                        }
                        else
                        {
                            if (devPin == syschroneziPin)
                            {
                                dev.Value.SetTerminalPulseModeScriptSMUB(item, EK2600IOTriggerSynMode.Master, devPin, syschroneziPin, monitorPin.ToArray());
                            }
                            else
                            {
                                dev.Value.SetTerminalPulseModeScriptSMUB(item, EK2600IOTriggerSynMode.Slave, devPin, syschroneziPin, null);
                            }
                        }
                    }
                }
                else if (dev.Value.SyncTrigSMU.Count == 2)
                {
                    string smuAName = dev.Value.SyncTrigSMU[0];

                    string smuBName = dev.Value.SyncTrigSMU[1];

                    int chA = this.GetChannel(dev.Key, smuAName);

                    int chB = this.GetChannel(dev.Key, smuBName);

                    ElectSettingData itemA = null;

                    ElectSettingData itemB = null;

                    if (dev.Key == DevDefine.GetID(DevDefine.SLAVE_PD) && (pdItem.IsTrigDetector || pdItem.IsTrigDetector2))
                    {
                        itemA = pdItem.Clone() as ElectSettingData;

                        itemB = pdItem.Clone() as ElectSettingData;

                        //itemA.TermianlFuncType = ETermianlFuncType.Bias;

                        itemA.MsrtType = EMsrtType.FVMI;

                        itemA.SweepStart = pdItem.DetectorBiasValue;

                        itemA.SweepStop = pdItem.DetectorBiasValue;

                        itemA.MsrtProtection = pdItem.DetectorMsrtRange;

                        //itemB.TermianlFuncType = ETermianlFuncType.Bias;

                        itemB.MsrtType = EMsrtType.FVMI;

                        itemB.SweepStart = pdItem.DetectorBiasValue2;

                        itemB.SweepStop = pdItem.DetectorBiasValue2;

                        itemB.MsrtProtection = pdItem.DetectorMsrtRange2;

                        devPin = (uint)this._devResource.Count;
                    }
                    else
                    {
                        itemA = smuDic[(ESMU)chA];

                        itemB = smuDic[(ESMU)chB];
                    }

                    if (smuAName == K2600Const.NAME_SMUB && smuBName == K2600Const.NAME_SMUA)
                    {
                        ElectSettingData tmp = itemA;

                        itemA = itemB;

                        itemB = tmp;
                    }

                    if (smuDic.Count == 1 || monitorPin.Count == 0)
                    {
                        // smuDic.Count == 1, Terminal只設定擊發單一smu
                        // monitorPin.Count == 0, 單一台2612兩個smu擊發不須做IO同步
                        dev.Value.SetTerminalPulseModeScriptDUAL(itemA, itemB, EK2600IOTriggerSynMode.Single, devPin, 0, null);
                    }
                    else
                    {
                        if (devPin == syschroneziPin)
                        {
                            dev.Value.SetTerminalPulseModeScriptDUAL(itemA, itemB, EK2600IOTriggerSynMode.Master, devPin, syschroneziPin, monitorPin.ToArray());
                        }
                        else
                        {
                            dev.Value.SetTerminalPulseModeScriptDUAL(itemA, itemB, EK2600IOTriggerSynMode.Slave, devPin, syschroneziPin, null);
                        }
                    }
                }
            }

            foreach (var dev in this._devResource)
            {
                if (dev.Value.SyncTrigSMU.Count == 0)
                {
                    continue;
                }

                dev.Value.MeterOutTerminalScript();
            }
        }

        #endregion

        #region >>> Public Method <<<

        public void Add(SmuAssignmentData data)
        {
            this._assignmentData.Add(data);
        }

        public int GetDeviceID(string ip)
        {
            if (this._assignmentData.Count != 0)
            {
                foreach (var data in this._assignmentData)
                {
                    if (data.IP == ip)
                    {
                        return (int)data.DeviceID;
                    }
                }
            }

            return -1;
        }

        public bool CreateAssignmentTable(ElecDevSetting devSetting)
        {
            if (this._assignmentData.Count == 0)
            {
                return false;
            }

            this._devResource.Clear();

            // 若出現 Multi-Master的情況, 重新排序 MASTER 的 DeviceID
            uint masterCount = 0;

            foreach (var data in this._assignmentData)
            {

                data.DeviceID = masterCount;

                // 合併重覆的IP 至一個 DeviceID
                int rtnID = this.GetDeviceID(data.IP);

                if (rtnID >= 0)
                {
                    data.DeviceID = (uint)rtnID;
                }
                if (data.DeviceName == DevDefine.MASTER)
                {
                    masterCount++;
                }
            }

            this._masterCount = (int)masterCount;

            // 將 SMU Channel 對應到 Device dict
            foreach (var data in this._assignmentData)
            {
                if (!this._devResource.ContainsKey(data.DeviceID))
                {
                    this._devResource.Add(data.DeviceID, new K2600Wrapper(data.DeviceID, data.DeviceName, data.DeviceModel, devSetting, data.IP, devSetting.SrcTriggerMode));
                }
            }

            if (this._devResource.Count == 0)
            {
                return false;
            }

            return true;
        }

        public void SetTrigAssignment(uint[] channels)
        {
            foreach (var dev in this._devResource)
            {
                dev.Value.SyncTrigSMU.Clear();
            }

            foreach (var ch in channels)
            {
                uint id = this._assignmentData[(int)ch].DeviceID;
                string smu = this._assignmentData[(int)ch].SMU;

                this._devResource[id].SyncTrigSMU.Add(smu);
            }
        }

        //public void OutputIOAssignment(SourceMeterDioCtrl[] channels)
        //{
        //    if (channels == null)
        //    {
        //        return;
        //    }

        //    // Reset io output data fore each device
        //    foreach (var kvp in this._devResource)
        //    {
        //        kvp.Value.DioWriteBuffer = 0x0;  
        //    }

        //    foreach (var ch in channels)
        //    {
        //        int srcChannel = (int)ch.SrcChannel;
        //        ushort ioData = ch.DioData;

        //        uint id = this._assignmentData[srcChannel].DeviceID;
        //        string smu = this._assignmentData[srcChannel].SMU;

        //        if (smu == K2600Const.NAME_SMUB)
        //        {
        //            this._devResource[id].DioWriteBuffer = (ushort)(this._devResource[id].DioWriteBuffer | ((ioData & 0xF) << 10));
        //        }
        //        else
        //        {
        //            this._devResource[id].DioWriteBuffer = (ushort)(this._devResource[id].DioWriteBuffer | ((ioData & 0xF) << 3));
        //        }
        //    }

        //    foreach (var kvp in this._devResource)
        //    {
        //        kvp.Value.DioWrite(kvp.Value.DioWriteBuffer);  // 14 bit for Keithley Dio
        //    }

        //}

        public int GetChannel(uint id, string smu)
        {
            for (int channel = 0; channel < this._assignmentData.Count; channel++)
            {
                if (id == this._assignmentData[channel].DeviceID && smu == this._assignmentData[channel].SMU)
                {
                    return channel;
                }
            }

            return -1;
        }



        public int GetSmuIndex(string smu)
        {
            if (smu == "b")
            {
                return 1;
            }

            return 0;
        }

        public bool ContainsName(string name)
        {
            uint id = DevDefine.GetID(name);

            return this._devResource.ContainsKey(id);
        }

        public bool ContainsChannel(uint channel)
        {
            if (this._assignmentData.Count > channel)
            {
                uint id = this._assignmentData[(int)channel].DeviceID;

                return this._devResource.ContainsKey(id);
            }
            else
            {
                return false;
            }
        }

        public void MeterOutput(bool isPulseMode, Dictionary<ESMU, ElectSettingData> smuDic, ElectSettingData pdItem)
        {
            if (isPulseMode)
            {
                this.MeterOutputPulseMode(smuDic, pdItem);
            }
            else
            {
                this.MeterOutputDCMode(smuDic, pdItem);
            }
        }

        public void TurnOff(uint[] activateChannels, double delay, bool isOpenRelay)
        {
            Dictionary<uint, List<string>> data = new Dictionary<uint, List<string>>();

            foreach (var ch in activateChannels)
            {
                uint id = this._assignmentData[(int)ch].DeviceID;

                string smuName = this._assignmentData[(int)ch].SMU;

                if (data.ContainsKey(id))
                {
                    data[id].Add(smuName);
                }
                else
                {
                    data.Add(id, new List<string>() { smuName });
                }
            }

            foreach (var devID in data)
            {
                if (devID.Value.Count == 1)
                {
                    if (devID.Value[0] == K2600Const.NAME_SMUA)
                    {
                        this._devResource[devID.Key].TurnOffSMUA(delay, isOpenRelay);
                    }
                    else if (devID.Value[0] == K2600Const.NAME_SMUB)
                    {
                        this._devResource[devID.Key].TurnOffSMUB(delay, isOpenRelay);
                    }
                }
                else if (devID.Value.Count == 2)
                {
                    this._devResource[devID.Key].TurnOff(delay, isOpenRelay);
                }
            }
        }

        #endregion
    }

    #endregion

    #region >>> Class SmuAssignmentData <<<

    internal class SmuAssignmentData
    {
        public uint DeviceID;
        public string DeviceModel;
        public string DeviceName;
        public string SMU;
        public string IP;

        // Single die config.  (Master + Slave)
        public SmuAssignmentData(string devAsgmtName, string ip)
        {
            DeviceName = devAsgmtName;
            DeviceID = DevDefine.GetID(devAsgmtName);
            DeviceModel = "K2600";
            SMU = K2600Const.NAME_SMUA;
            IP = ip;
        }

        // Multi die config. (Multi Master)
        public SmuAssignmentData(string devAsgmtName, string devModel, string smu, string ip)
        {
            DeviceName = devAsgmtName;
            DeviceModel = devModel;
            DeviceID = DevDefine.GetID(devAsgmtName);
            SMU = smu;
            IP = ip;
        }
    }

    #endregion
}