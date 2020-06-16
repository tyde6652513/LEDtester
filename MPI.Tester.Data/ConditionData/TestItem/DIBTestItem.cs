using System;
using System.Collections.Generic;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Data
{
    [Serializable]
    public class DIBTestItem : TestItemData
    {
        private string _itemNameA;
        private string _itemNameB;
        private string _itemKeyNameA;
        private string _itemKeyNameB;
        private double _filterBase;
        private double _filterSpec;
        private int _flatCount;
        private bool _isOnlyJuadeSerious;

        private double _forceVoltBase;
        private double _forceVoltDistub;


        public DIBTestItem()
            : base()
        {
            this._lockObj = new object();
            this._type = ETestType.DIB;
            // Electrical Setting 


            this._elecSetting = new ElectSettingData[] { new ElectSettingData("mA", "V", "ms") };
            this._elecSetting[0].MsrtType = EMsrtType.FIMV;
            this._elecSetting[0].MsrtRange = 8;
            this._elecSetting[0].MsrtProtection = 8.0d;
            // Tested Result Setting

            this._msrtResult = new TestResultData[] {   new TestResultData("V", "0.000"),
														new TestResultData("V", "0.000"),
                                                        new TestResultData("V", "0.000"),
                                                        new TestResultData("", "0"),
														};

            // Gain Offset Setting
            //this._gainOffsetSetting = new GainOffsetData[] { new GainOffsetData(true, EGainOffsetType.Offset),
            //                                                 new GainOffsetData(true, EGainOffsetType.Offset),
            //                                                 new GainOffsetData(true, EGainOffsetType.Offset)};



            //this._msrtResult = new TestResultData[] { new TestResultData("V", "0.000") };
            //this._msrtResult[0].MinLimitValue = 0.0d;
            //this._msrtResult[0].MaxLimitValue = 8.0d;
            //this._msrtResult[0].MinLimitValue2 = 0.0d;
            //this._msrtResult[0].MaxLimitValue2 = 8.0d;

            // Gain Offset Setting
            this._gainOffsetSetting = null;

            //// Tested Result Setting
            //this._msrtResult = null;
            //this._gainOffsetSetting = null;  
            this._filterBase = 1.90d;
            this._filterSpec = 0.05d;
            this._flatCount = 100;
            this._forceVoltBase = 1.90d;
            this._forceVoltDistub = 0.05d;
            this._isOnlyJuadeSerious = false;
            this.ResetKeyName();
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

        public double FilterBase
        {
            get { return this._filterBase; }
            set { lock (this._lockObj) { this._filterBase = value; } }
        }

        public double FilterSpec
        {
            get { return this._filterSpec; }
            set { lock (this._lockObj) { this._filterSpec = value; } }
        }

        public int FlatCount
        {
            get { return this._flatCount; }
            set { lock (this._lockObj) { this._flatCount = value; } }
        }

        public bool IsOnlyJuadeSerious
        {
            get { return this._isOnlyJuadeSerious; }
            set { lock (this._lockObj) { this._isOnlyJuadeSerious = value; } }
        }

        public double ForceVoltBase
        {
            get { return this._forceVoltBase; }
            set { lock (this._lockObj) { this._forceVoltBase = value; } }
        }

        public double ForceVoltDistub
        {
            get { return this._forceVoltDistub; }
            set { lock (this._lockObj) { this._forceVoltDistub = value; } }
        }


		#endregion

        #region >>> Protected Methods <<<

        protected override void ResetKeyName()
        {
            base.ResetKeyName();
            int num = this._subItemIndex + 1;     // 0-bas

            this._elecSetting[0].KeyName = this.KeyName;

            // Reset Tested Result KeyName
            this._msrtResult[0].KeyName = "DIBEVF_" + num.ToString();
            this._msrtResult[0].Name = this._msrtResult[0].Name;
            this._msrtResult[1].KeyName = "DIBSVF_" + num.ToString();
            this._msrtResult[1].Name = this._msrtResult[1].Name;
            this._msrtResult[2].KeyName = "DIBDVF_" + num.ToString();
            this._msrtResult[2].Name = this._msrtResult[2].Name;
            this._msrtResult[3].KeyName = "DIBLEVEL_" + num.ToString();
            this._msrtResult[3].Name = this._msrtResult[3].Name;

            SetMsrtNameAsKey();
            // Reset Gain Offset Seeting KeyName
            //this.GainOffsetSetting[0].KeyName = this._msrtResult[0].KeyName;
            //this.GainOffsetSetting[0].Name = this._msrtResult[0].Name;
        }

        #endregion

    }
}