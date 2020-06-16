using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
    [Serializable]
    public class IOConfigData : ICloneable
    {
        private List<IOData> ioList;

        #region >>> Constructor / Disposor <<<

        public IOConfigData()
        {
            ioList = new List<IOData>();
        }

        public IOConfigData(int ioQty):this()
        {
            for (int i = 1; i <= ioQty; ++i)
            {
                ioList.Add(new IOData(i));
            }

            if (ioQty == 14)//可能是 keithky
            {
                this[1].SetDefaultIO(EIOTrig_Mode.TRIG_BYPASS, EIOState.LOW, EIOAct.NONE, 0, "PIN_SPT_TRIG_OUT");
                this[2].SetDefaultIO(EIOTrig_Mode.TRIG_FALLING, EIOState.HIGH, EIOAct.NONE, 0.0001, "PIN_SMU_TRIG_OUT");
                this[3].SetDefaultIO(EIOTrig_Mode.TRIG_FALLING, EIOState.HIGH, EIOAct.NONE, 0.0005, "PIN_SMU_ABORT_OUT");
                this[4].SetDefaultIO(EIOTrig_Mode.TRIG_FALLING, EIOState.HIGH, EIOAct.NONE, 0.0005, "PIN_SMU_ABORT_IN");
                this[11].SetDefaultIO(EIOTrig_Mode.TRIG_FALLING, EIOState.HIGH, EIOAct.NONE, 0.0001, "PIN_SMU_TRIG_IN");

                this[6].SetDefaultIO(EIOTrig_Mode.TRIG_RISING, EIOState.HIGH, EIOAct.NONE, 0.005, "PIN_DAQ_ENABLE");
                this[7].SetDefaultIO(EIOTrig_Mode.TRIG_RISING, EIOState.HIGH, EIOAct.NONE, 0.005, "PIN_RTH_EANBLE");
                this[13].SetDefaultIO(EIOTrig_Mode.TRIG_RISING, EIOState.HIGH, EIOAct.NONE, 0.005, "PIN_DAQ_TRIG_OUT");
                this[14].SetDefaultIO(EIOTrig_Mode.TRIG_RISING, EIOState.HIGH, EIOAct.NONE, 0.0001, "PIN_POLAR_SW");
            }
        }

        #endregion

        #region >>> Public Property <<<

        public List<IOData> IOList
        {
            get { return this.ioList; }
        }
        
        public IOData this[int num]
        {
            get
            {
                foreach (IOData ioData in ioList)
                {
                    if (ioData.PinNum == num)
                    {
                        return ioData;
                    }
                }
                return null;
            }
        }
        #endregion

        #region >>> Public Method <<<

        public void SetIOQty(int num)
        {            
            if (ioList == null)
            {
                ioList = new List<IOData>();
            }
            int startIndex = ioList.Count;

            for (int i = startIndex + 1; i <= num; ++i)
            {
                if (!ContainPin(i))
                {
                    ioList.Add(new IOData(i));
                }
            }
            ioList.Sort((x, y) => { return x.PinNum.CompareTo(y.PinNum); });
        }

        public bool ContainPin(int pin)
        {
            if (ioList != null)
            {
                foreach (IOData ioD in ioList)
                {
                    if (ioD.PinNum == pin)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public object Clone()
        {
            IOConfigData cloneObj = new IOConfigData();

            foreach (IOData data in this.ioList)
            {
                cloneObj.ioList.Add(data.Clone() as IOData);
            }

            return cloneObj;
        }

        public bool Load(string fileFullName)
        {
            bool rtn = true;

            if (!System.IO.File.Exists(fileFullName))
            {
                return false;
            }

            try
            {
                this.ioList = MPI.Xml.XmlFileSerializer.Deserialize(typeof(List<IOData>), fileFullName) as List<IOData>;
            }
            catch
            {
                return false;
            }

            return rtn;
        }

        public bool Save(string fileFullName)
        {
            bool rtn = true;

            try
            {
                MPI.Xml.XmlFileSerializer.Serialize(this.ioList, fileFullName);
            }
            catch
            {
                return false;
            }

            return rtn;
        }
        
        #endregion
    }


    [Serializable]
    public class IOData
    {
        private object _lockObj;
        private int _pinNum;
        private EIOTrig_Mode _mode;
        private EIOState _state;
        private EIOTrig_Mode _defaultMode;
        private EIOState _defaulState;
        private EIOAct _action;
        private double _pulsewidth; //unit S
        private string _pinName;
        private bool _isShow;

        
        #region >>> Constructor / Disposor <<<
        public IOData()
        {
            this._lockObj = new object();
            _pinNum = 0;
            _mode = EIOTrig_Mode.TRIG_BYPASS;
            _state = EIOState.LOW;
            _defaultMode = EIOTrig_Mode.TRIG_BYPASS;
            _defaulState = EIOState.LOW;
            _pulsewidth = 0;
            _pinName = "";
            _isShow = false;
            _action = EIOAct.NONE;
        }
        public IOData(int pin):this()
        {
            _pinNum = pin;
        }
        #endregion

        #region >>> Public Property <<<

        public int PinNum
        {
            get { return this._pinNum; }
            set { lock (this._lockObj) { this._pinNum = value; } }
        }

        public EIOTrig_Mode DMode
        {
            get { return this._defaultMode; }
            set { lock (this._lockObj) 
            { 
                this._defaultMode = value;
                this._mode = value;
            } }
        }

        public EIOState DState
        {
            get { return this._defaulState; }
            set { lock (this._lockObj) 
            { 
                this._defaulState = value;
                this._state = value;
            } 
            }
        }

        public EIOTrig_Mode Mode
        {
            get { return this._mode; }
            set { lock (this._lockObj) { this._mode = value; } }
        }

        public EIOState State
        {
            get { return this._state; }
            set { lock (this._lockObj) { this._state = value; } }
        }

        public double PulseWidth
        {
            get { return this._pulsewidth; }
            set { lock (this._lockObj) { this._pulsewidth = value; } }
        }

        public string Name
        {
            get { return this._pinName; }
            set { lock (this._lockObj) { this._pinName = value; } }
        }
        public bool IsShow
        {
            get { return this._isShow; }
            set { lock (this._lockObj) { this._isShow = value; } }
        }

        public EIOAct Action
        {
            get { return this._action; }
            set { lock (this._lockObj) { this._action = value; } }
        }
        //_isShow

        #endregion

        #region >>> Public Method <<<

        public void SetDefaultIO(EIOTrig_Mode dMode, EIOState dState,EIOAct act, double pw = 0,string Name = "")
        {
            this._defaultMode = dMode;
            this._defaulState = dState;
            this._mode = dMode;
            this._state = dState;
            this._pulsewidth = pw;
            this._pinName = Name;
            this._action = act;

        }

        public bool SetIO(EIOTrig_Mode mode, EIOState state)
        {
            bool isChanged = false;//若有改變，回傳true

            if (_mode != mode || _state != state)
            {
                isChanged = true;
            }

            _mode = mode;

            _state = state;

            return isChanged;

        }

        public object Clone()
        {

            
            return this.MemberwiseClone();
        }
        
        #endregion
    }
}
