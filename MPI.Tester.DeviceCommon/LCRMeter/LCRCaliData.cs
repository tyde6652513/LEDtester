using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using MPI.Tester.Data;

namespace MPI.Tester.DeviceCommon
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



        //private List<ELCRTestType> _caliTypeList;

        public LCRCaliData()
        {
            this._nowDataNum = 1;

            this._testType = ELCRTestType.CPD;

            this._cableLen = "";

            this._level = 0.03;

            this._bias = 0;

            this._isEnableOpen = false;
            this._isEnableshort = false;
            this._isEnableLoad = false;
            this._loadingList = new List<LoadingRefData>();


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
            //this._caliTypeList = new List<ELCRTestType>();

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

        #region

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

            //cloneObj._caliTypeList = this._caliTypeList;

            return cloneObj;
        }

        public override string ToString()
        {
            string str = "";

            if (_loadingList != null &&
                _loadingList.Count > _nowDataNum)
            {
                LoadingRefData refData = _loadingList[_nowDataNum - 1];
                if (_isEnableOpen)
                {
                    str += "Open:Enable \nG:" + refData.OpenRaw.MeterRowValA.ToString("E3") + " S, B:" + refData.OpenRaw.MeterRowValB.ToString("E3") + " S\n";
                    //double c =
                    str += "Equivalent C:" + refData.OpenRaw.ValA.ToString("E3") + " F, D:" + refData.OpenRaw.ValB.ToString("E3") + "\n";
                }
                else
                {
                    str += "Open:Disable ";
                }

                if (_isEnableshort)
                {
                    str += "Short:Enable \nR:" + refData.ShortRaw.MeterRowValA.ToString("E3") + " ohms, X:" + refData.ShortRaw.MeterRowValB.ToString("E3") + " ohms\n";

                    str += "Equivalent L:" + refData.ShortRaw.ValA.ToString("E3") + " H, D:" + refData.ShortRaw.ValB.ToString("E3") + "\n";
                }

                if (_isEnableLoad)
                {

                    //目前設計成只能使用CpD,CsD
                    double rawC = refData.LoadRaw.MeterRowValA / UnitConvertFactor(refData.RefUnit, "F");
                    str += "Load " + refData.LoadRaw.CaliLCRTestType.ToString() + ":Enable \nSet:" + refData.RefA.ToString("E3") + refData.RefUnit +
                        ", " + refData.RefB.ToString("E3") + refData.LoadRaw.MeterUnitB + "\n";
                    str += "Measure Raw:" + rawC.ToString("E3") + refData.LoadRaw.MeterUnitA + " , " +
                        refData.LoadRaw.MeterRowValB.ToString("E3") + "\n";
                }

            }


            return str;

        }


        public static double UnitConvertFactor(ECapUnit fromUnit, string toUnit)
        {
            double scale = 0.0d;

            if (Enum.IsDefined(typeof(ECapUnit), toUnit))
            {
                ECapUnit tmp = (ECapUnit)Enum.Parse(typeof(ECapUnit), toUnit, false);

                scale = Math.Pow(10.0d, fromUnit - tmp);
            }

            return scale;
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

        CaliRowData _openRaw;
        CaliRowData _shortRaw;
        CaliRowData _loadRaw;


        public LoadingRefData()
        {
            _isEnable = false;
            this._frequency = 1000;
            this._refA = 0;
            this._refB = 0;
            this._refAUnit = ECapUnit.F;
            this._openRaw = new CaliRowData();
            this._shortRaw = new CaliRowData();
            this._loadRaw = new CaliRowData();
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

        public CaliRowData OpenRaw
        {
            get { return this._openRaw; }
            set { this._openRaw = value; }
        }

        public CaliRowData ShortRaw
        {
            get { return this._shortRaw; }
            set { this._shortRaw = value; }
        }

        public CaliRowData LoadRaw
        {
            get { return this._loadRaw; }
            set { this._loadRaw = value; }
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

            cloneObj._openRaw = this._openRaw;
            cloneObj._shortRaw = this._shortRaw;
            cloneObj._loadRaw = this._loadRaw;

            return cloneObj;
        }

    }
    [Serializable]
    public class CaliRowData
    {
        public ELCRTestType CaliLCRTestType;
        public double ValA;
        public double ValB;
        public double MeterRowValA;
        public double MeterRowValB;
        public string UnitA;
        public string UnitB;
        public string MeterUnitA;
        public string MeterUnitB;

        public CaliRowData()
        {
            CaliLCRTestType = ELCRTestType.CPD;
            ValA = 0;
            ValB = 0;
            MeterRowValA = 0;
            MeterRowValB = 0;
            UnitA = "";
            UnitB = "";
            MeterUnitA = "";
            MeterUnitB = "";
        }

        
    }

}
