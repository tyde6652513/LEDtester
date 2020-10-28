using System;
using System.Collections.Generic;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
    [Serializable]
    public class CALCTestItem : TestItemData
    {
        private string _itemNameA;
        private string _itemNameB;
        private string _itemKeyNameA;
        private string _itemKeyNameB;
        private ECalcType _clacType;
        private double _gain;
        private double _valB;
        private bool _isBConstVal;
        private bool _isAdvanceMode;
        private string _userCommands;
        private string _localAssemble;
        //private SimpleCompiler _sc;

        public CALCTestItem()
            : base()
        {
            this._lockObj = new object();
            this._type = ETestType.CALC;

            this.ElecSetting = null;
            //// Tested Result Setting
            this._msrtResult = new TestResultData[] { new TestResultData("V", "0.000") };
            this._msrtResult[0].IsVision = true;
            this._msrtResult[0].IsEnable = true;
            //// Gain Offset Setting
            this._gainOffsetSetting = new GainOffsetData[] { new GainOffsetData(true, EGainOffsetType.GainAndOffest) };
            this._gainOffsetSetting[0].IsEnable = true;
            this._clacType = ECalcType.Subtract;
            this.ResetKeyName();
            this._gain = 1;
            ValA = 0;
            IsAConst = false ;
            this._valB = 0;
            this._isBConstVal = false;
            this._isAdvanceMode = false;
            this._userCommands = "";
            this._localAssemble = "";
            this.Remark = "";
        }

       #region >>> Public Methods <<<

        public string ItemNameA
        {
            get { return this._itemNameA; }
            set { lock (this._lockObj) { this._itemNameA = value; } }          
        }

        public string ItemNameB
        {
            get { return this._itemNameB; }
            set { lock (this._lockObj) { this._itemNameB= value; } }
        }

        public string ItemKeyNameA
        {
            get { return this._itemKeyNameA; }
            set { lock (this._lockObj) { this._itemKeyNameA = value; } }
        }

        public string ItemKeyNameB
        {
            get { return this._itemKeyNameB; }
            set { lock (this._lockObj) { this._itemKeyNameB = value; } }
        }

        public ECalcType CalcType
        {
            get { return this._clacType; }
            set { lock (this._lockObj) { this._clacType = value; } }
        }
        public double Gain
        {
            get { return this._gain; }
            set { lock (this._lockObj) { this._gain = value; } }
        }

        public double ValA{get;set;}
        public bool IsAConst{get;set;}

        public double ValB
        {
            get { return this._valB; }
            set { lock (this._lockObj) { this._valB = value; } }
        }
        public bool IsBConst
        {
            get { return this._isBConstVal; }
            set { lock (this._lockObj) { this._isBConstVal = value; } }
        }
        public bool IsAdvanceMode
        {
            get { return this._isAdvanceMode; }
            set { lock (this._lockObj) { this._isAdvanceMode = value; } }

        }
        public string UserCommand
        {
            get { return this._userCommands; }
            set { lock (this._lockObj) { this._userCommands = value; } }
        }
        public string LocalAssemble
        {
            get { return this._localAssemble; }
            set { lock (this._lockObj) { this._localAssemble = value; } }

        }

        public string Remark { get; set; }
        //_localAssemble
        
		#endregion

        #region >>> Protected Methods <<<

        protected override void ResetKeyName()
        {
            base.ResetKeyName();
            int num = this._subItemIndex + 1;     // 0-base
            // this._elecSetting[0].KeyName = this.KeyName;
            // Reset Tested Result KeyName
            this._msrtResult[0].KeyName = "MCALC_" + num.ToString();
            //this._msrtResult[0].Name = this._msrtResult[0].KeyName;

            SetMsrtNameAsKey();
            // Reset Gain Offset Seeting KeyName
            this.GainOffsetSetting[0].KeyName = this._msrtResult[0].KeyName;
            this.GainOffsetSetting[0].Name = this._msrtResult[0].Name;
        }

        #endregion

    }
}