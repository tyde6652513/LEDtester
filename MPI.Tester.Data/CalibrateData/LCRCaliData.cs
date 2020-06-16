using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
    [Serializable]
    public class LCRCaliData : System.ICloneable
    {
        private int _nowDataNum;
        //private ELCRMsrtSpeed _msrtSpeed;

        private ELCRTestType _testType; 
      
        private string _cableLen;

        private List<LoadingRefData> _loadingList;

        private bool _isEnableOpen;

        private bool _isEnableshort;

        private bool _isEnableLoad;

        private double _level;

        private double _bias;

        private List<ELCRTestType> _caliTypeList;

        public LCRCaliData()
        {
            this._nowDataNum = 1;
            this._testType = ELCRTestType.CPD;

            this._cableLen = "";

            this._level = 1;

            this._bias = 0;

            this._isEnableOpen = false;
            this._isEnableshort = false;
            this._isEnableLoad = false;
            this._loadingList = new List<LoadingRefData>();
            this._caliTypeList = new List<ELCRTestType>();
        }


        public LCRCaliData( int loadQty )
        {
            this._nowDataNum = loadQty;
            this._testType = ELCRTestType.CPD;
            
            this._cableLen = "";

            this._isEnableOpen = false;
            this._isEnableshort = false;
            this. _isEnableLoad = false;
            this._loadingList = new List<LoadingRefData>();
            this._caliTypeList = new List<ELCRTestType>();

            this._loadingList.Clear();

            for (int i = 0; i < loadQty; ++i)
            {
                this._loadingList.Add(new LoadingRefData());
            }
        }



        #region  >>>property<<<
        public int NowDataNum
        {
            get { return _nowDataNum; }
            set { _nowDataNum = value; }

        }

        public ELCRTestType TestType
        {
            get{return this._testType;}
            set{this._testType = value;}
        }

        public string CableLength
        {
            get { return this._cableLen; }
            set { this._cableLen = value; }
        }

        public List<LoadingRefData> LoadingList
        {
            get { return this._loadingList; }
            set { this._loadingList = value; }
        }

        public bool EnableOpen
        {
            get { return this._isEnableOpen; }
            set { this._isEnableOpen = value; }
        }

        public bool EnableShort
        {
            get { return this._isEnableshort; }
            set { this._isEnableshort = value; }
        }

        public bool EnableLoad
        {
            get { return this._isEnableLoad; }
            set { this._isEnableLoad = value; }
        }

        public List<ELCRTestType> CaliTypeList
        {
            get { return _caliTypeList; }
            set { this._caliTypeList = value; }
        }

        public double Level
        {
            get { return _level; }
            set { _level = value; }

        }

        public double Bias
        {
            get { return _bias; }
            set { _bias = value; }

        }
        


        #endregion

        public object Clone()
        {
            LCRCaliData cloneObj = new LCRCaliData();

            cloneObj._nowDataNum = this._nowDataNum;

            cloneObj._cableLen = this._cableLen;

            cloneObj._testType = this._testType;

            cloneObj._isEnableOpen = this._isEnableOpen;

            cloneObj._isEnableshort = this._isEnableshort;

            cloneObj._isEnableLoad = this._isEnableLoad;

            cloneObj._level = this._level;

            cloneObj._bias = this._bias;


            cloneObj._loadingList.Clear();

            foreach (LoadingRefData ld in _loadingList)
            {
                cloneObj._loadingList.Add(ld.Clone() as LoadingRefData); 
            }

            cloneObj._caliTypeList = this._caliTypeList;

            return cloneObj;
        }


    }
    [Serializable]
    public class LoadingRefData : ICloneable
    {
        private bool _isEnable;
        private int _frequency;
        private double _refA;
        private double _refB;
        private ECapUnit _refAUnit;


        public LoadingRefData()
        {
            _isEnable = false;
            this._frequency = 1000;
            this._refA = 0;
            this._refB = 0;
            this._refAUnit = ECapUnit.F;
        }

        #region >>>property<<<

        public bool Enable
        {
            get { return this._isEnable; }
            set { this._isEnable = value; }
        }

        public int Freq
        {
            get { return this._frequency; }
            set { this._frequency = value; }
        }


        public ECapUnit RefUnit
        {
            get { return this._refAUnit; }
            set { this._refAUnit = value; }
        }

        public double RefA
        {
            get { return this._refA; }
            set { this._refA = value; }
        }
        public double RefB
        {
            get { return this._refB; }
            set { this._refB = value; }
        }

        #endregion

        public object Clone()
        {
            LoadingRefData cloneObj = new LoadingRefData();

            cloneObj._isEnable = this._isEnable;

            cloneObj._frequency = this._frequency;

            cloneObj._refA = this._refA;

            cloneObj._refB = this._refB;

            cloneObj._refAUnit = this._refAUnit;

            return cloneObj;
        }

    }

}
