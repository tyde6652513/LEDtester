using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
    [Serializable]
    public class IOSetting : System.ICloneable
    {
        private object _lockObj;

        private List<IOCmd> IOCmdList;

        public IOSetting()
        {
            this._lockObj = new object();

            IOCmdList = new List<IOCmd>();


        }

        #region >>> Public Property <<<

        public IOCmd this[int num]
        {
            get
            {
                foreach (IOCmd ioData in IOCmdList)
                {
                    if (ioData.Pin == num)
                    {
                        return ioData;
                    }
                }
                return null;
            }
        }
        public List<IOCmd> CmdList
        {
            get
            {
                return this.IOCmdList;
            }
 
        }

        // public string ID
        //{
        //    get { return this._id; }
        //    set { lock (this._lockObj) { this._id = value; } }
        //}

        ///// <summary>
        ///// KeyName of Electrical Setting Item 
        ///// </summary>
        //public string KeyName
        //{
        //    get { return this._keyName; }
        //    set { lock (this._lockObj) { this._keyName = value; } }
        //}

        ///// <summary>
        ///// Name of Electrical Setting Item
        ///// </summary>
        //public string Name
        //{
        //    get { return this._name; }
        //    set { lock (this._lockObj) { this._name = value; } }
        //}

        //public uint Order
        //{
        //    get { return this._order; }
        //    set { lock (this._lockObj) { this._order = value; } }
        //}
    


        #endregion

        #region >>> Public Method <<<

        public object Clone()
        {
            IOSetting data = this.MemberwiseClone() as IOSetting;

            IOCmd[] cmdArr = this.IOCmdList.ToArray();

            data.IOCmdList = new List<IOCmd>();

            foreach (IOCmd ic in cmdArr)
            {
                data.IOCmdList.Add(ic.Clone() as IOCmd);
            }

            return data;
        }

        #endregion
    }
    [Serializable]
    public class IOCmd : System.ICloneable
    {
        private object _lockObj;
        private int _pin;
        //private bool _isTrig;
        private EIOTrig_Mode _mode;
        private EIOState _state;

        //private EIOAct _act;
        private double _holdTime;//IO保持狀態的時間
        private double _delayTime;//IO延後作動的時間

        #region >>Constructor<<
        public IOCmd()
        {
            this._lockObj = new object() ;
            this._pin = 0;
            //this._isTrig = false;
            this._holdTime = 0;
            this._delayTime = 0;
            this._mode = EIOTrig_Mode.TRIG_BYPASS;
            this._state = EIOState.NONE;
        }

        public IOCmd(IOData iData)
            : this()
        {
            this._pin = iData.PinNum;
            this._mode = iData.Mode;
            this._state = iData.State;//考量未來要參考前一次IO狀態的作法，不使用DState
            this._delayTime = iData.PulseWidth;
        }
        #endregion

        #region >>property<<
        public int Pin
        {
            get { return this._pin; }
            set { lock (this._lockObj) { this._pin = value; } }
        }
        //public bool IsTrig
        //{
        //    get { return this._isTrig; }
        //    set { lock (this._lockObj) { this._isTrig = value; } }
        //}
        //_isTrig
        public double HoldTime
        {
            get { return this._holdTime; }
            set { lock (this._lockObj) { this._holdTime = value; } }
        }

        public double DelayTime
        {
            get { return this._delayTime; }
            set { lock (this._lockObj) { this._delayTime = value; } }
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
        //public EIOAct Action
        //{
        //    get { return this._act; }
        //    set { lock (this._lockObj) { this._act = value; } }
        //}
        
        #endregion

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}