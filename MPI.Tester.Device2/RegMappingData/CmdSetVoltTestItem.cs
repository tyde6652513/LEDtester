using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SourceMeter.TSE
{
    public class CmdSetVoltTestItem : RegCmdBase
    {
        private EPMU _pmu;
        private ERegMsrtType _msrtType;                   
        private int _forceRangeIndex;
        private int _MsrtRangeIndex;

        #region >>> Public Properties <<<

        public int TestItemNum
        {
            set { lock (this._lockObj) { this.SetParamData(0, Convert.ToUInt16(value)); } }
        }

        public uint PMU
        {
            set
            {
                lock (this._lockObj)
                {
                    this.SetParamData(1, Convert.ToUInt16(value));
                }
            }
        }

        public ERegMsrtType MsrtType
        {
            set
            {
                lock (this._lockObj)
                {
                    this.SetParamData(2, Convert.ToUInt16(value));
                    this._msrtType = value;
                }
            }
        }

        public int MsrtCount
        {
            set { lock (this._lockObj) { this.SetParamData(3, Convert.ToUInt16(value)); } }
        }

        public EPolarity ItemPolarity
        {
            set { lock (this._lockObj) { this.SetParamData(4, Convert.ToUInt16(value)); } }
        }

        public double DelayTime
        {
            set
            {
                lock (this._lockObj)
                {
                    this.SetParamData(5, (ushort)TSEConvert.TimeMSB(value));
                    this.SetParamData(6, (ushort)TSEConvert.TimeLSB(value));
                }
            }
        }

        public double ForceValue
        {
            set
            {
                lock (this._lockObj)
                {
                    if (
                            this._msrtType == ERegMsrtType.FIMV ||
                            this._msrtType == ERegMsrtType.FIMVSWEEP ||
                            this._msrtType == ERegMsrtType.THY)
                    {
                        this.SetParamData(7, TSEConvert.ValueToUInt16(value, (EIRange)this._forceRangeIndex));
                    }
                    else if (
                                this._msrtType == ERegMsrtType.FVMI ||
                                this._msrtType == ERegMsrtType.FVMISWEEP)
                    {
                        this.SetParamData(7, TSEConvert.ValueToUInt16(value, (EVRange)this._forceRangeIndex));
                    }
                }
            }


        }

        public int ForceRangeIndex
        {
            set
            {
                lock (this._lockObj)
                {
                    this.SetParamData(8, Convert.ToUInt16(value));
                    this._forceRangeIndex = value;
                }
            }
        }

        public double ApplyTime
        {
            set
            {
                lock (this._lockObj)
                {
                    this.SetParamData(9, (ushort)TSEConvert.TimeMSB(value));
                    this.SetParamData(10, (ushort)TSEConvert.TimeLSB(value));
                }
            }
        }

        public int MsrtRangeIndex
        {
            set
            {
                lock (this._lockObj)
                {
                    this.SetParamData(11, Convert.ToUInt16(value));
                    this._MsrtRangeIndex = value;
                }
            }
        }

        public EPolarity ClampPolarity
        {
            set { lock (this._lockObj) { this.SetParamData(12, Convert.ToUInt16(value)); } }
        }

        public double ClampValue  // MsrtRange
        {
            set
            {
                lock (this._lockObj)
                {
                    if (this._msrtType == ERegMsrtType.FIMV ||
                        this._msrtType == ERegMsrtType.FIMVSWEEP ||
                        this._msrtType == ERegMsrtType.THY)
                    {
                        this.SetParamData(13, TSEConvert.ValueToUInt16(value, (EVRange)this._MsrtRangeIndex));
                    }
                    else if (this._msrtType == ERegMsrtType.FVMI ||
                             this._msrtType == ERegMsrtType.FVMISWEEP)
                    {
                        this.SetParamData(13, TSEConvert.ValueToUInt16(value, (EIRange)this._MsrtRangeIndex));
                    }
                }
            }

            // set { lock (this._lockObj) { this.SetParamData(13, Convert.ToUInt16(value)); } }
        }

        public int FilterCount
        {
            set { lock (this._lockObj) { this.SetParamData(14, Convert.ToUInt16(value)); } }
        }

        public ESourceSpeed SourceSpeed
        {
            set { lock (this._lockObj) { this.SetParamData(15, Convert.ToUInt16(value)); } }
        }

        public bool WLTriggerEn
        {
            set { lock (this._lockObj) { this.SetParamData(16, Convert.ToUInt16(value)); } }
        }

        public bool IsAutoTurnOff
        {
            set { lock (this._lockObj) { this.SetParamData(17, Convert.ToUInt16(value)); } }
        }

        public double SweepStartValue
        {
            set
            {
                lock (this._lockObj)
                {
                    if (
                            this._msrtType == ERegMsrtType.FIMV ||
                            this._msrtType == ERegMsrtType.FIMVSWEEP ||
                            this._msrtType == ERegMsrtType.THY)
                    {
                        this.SetParamData(18, TSEConvert.ValueToUInt16(value, (EIRange)this._forceRangeIndex));
                    }
                    else if (
                                this._msrtType == ERegMsrtType.FVMI ||
                                this._msrtType == ERegMsrtType.FVMISWEEP)
                    {
                        this.SetParamData(18, TSEConvert.ValueToUInt16(value, (EVRange)this._forceRangeIndex));
                    }
                }
            }
        }

        public double SweepStepValue
        {
            set
            {
                lock (this._lockObj)
                {
                    if (
                            this._msrtType == ERegMsrtType.FIMV ||
                            this._msrtType == ERegMsrtType.FIMVSWEEP ||
                            this._msrtType == ERegMsrtType.THY)
                    {
                        this.SetParamData(19, TSEConvert.ValueToUInt16(value, (EIRange)this._forceRangeIndex));
                    }
                    else if (
                                this._msrtType == ERegMsrtType.FVMI ||
                                this._msrtType == ERegMsrtType.FVMISWEEP)
                    {
                        this.SetParamData(19, TSEConvert.ValueToUInt16(value, (EVRange)this._forceRangeIndex));
                    }
                }
            }
        }

        public uint SweepRiseCount
        {
            set { lock (this._lockObj) { this.SetParamData(20, Convert.ToUInt16(value)); } }
        }

        public double TurnOffTime
        {
            set
            {
                lock (this._lockObj)
                {
                    this.SetParamData(21, (ushort)TSEConvert.TimeMSB(value));
                    this.SetParamData(22, (ushort)TSEConvert.TimeLSB(value));
                }
            }
        }

        public uint SweepFlatCount
        {
            set { lock (this._lockObj) { this.SetParamData(23, Convert.ToUInt16(value)); } }
        }

        public ERegSweepMode SweepMode
        {
            set { lock (this._lockObj) { this.SetParamData(24, Convert.ToUInt16(value)); } }
        }

        public bool SweepTableEn
        {
            set { lock (this._lockObj) { this.SetParamData(25, Convert.ToUInt16(value)); } }
        }

        public double SweepTurnOffTime
        {
            set
            {
                lock (this._lockObj)
                {
                    this.SetParamData(26, (ushort)TSEConvert.TimeMSB(value));
                    this.SetParamData(27, (ushort)TSEConvert.TimeLSB(value));
                }
            }
        }

        #endregion

        #region >>> Private Method <<<

       
        #endregion

        #region >>> Public Method <<<

        public CmdSetVoltTestItem() : base(Convert.ToUInt16(EMeterCmdID.SetVoltTestItem), 28, 0)
        { 
        }

        public CmdSetVoltTestItem(int CmdID) : base(Convert.ToUInt16(CmdID), 28, 0)
        { 
        }

        #endregion
    }
}
