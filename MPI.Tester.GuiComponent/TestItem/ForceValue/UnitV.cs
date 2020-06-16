using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//using MPI.Tester.Maths;
using MPI.Tester.GuiComponent.Unit;

namespace MPI.Tester.GuiComponent
{
    public partial class UnitV : UserControl
    {
        UnitVoltData _currentUnitData = new UnitVoltData();

        UnitVoltData _d4UnitData = new UnitVoltData();

        public delegate void UnitChangeDel(object sender, EventArgs e);

        public event UnitChangeDel Unit_Change;//像LIV這種有多個輸入的時候作為同步用

        public UnitV()
        {
            InitializeComponent();

            cmbForceValueUnit.Items.Clear();

            var uList = Enum.GetNames(typeof(EVoltUnit));

            var orderList = (from u in uList
                             orderby (int)(Enum.Parse(typeof(EVoltUnit), u)) descending
                             select u).ToList();

            cmbForceValueUnit.Items.AddRange(orderList.ToArray());

            _currentUnitData = this.GetDataFromUI();

            _d4UnitData = _currentUnitData.Clone() as UnitVoltData;
        }

        #region >>> Public Method <<<

        public void DescriptionPropertyUpload(UnitAmpData descriptionProperty)
        {
            dinForceValue.MaxValue = descriptionProperty.Maximum;

            dinForceValue.MinValue = descriptionProperty.Minimum;

            dinForceValue.Value = descriptionProperty.Value;

            dinForceValue.DisplayFormat = descriptionProperty.DisplayFormat;

            _d4UnitData = new UnitVoltData();

            _d4UnitData = descriptionProperty.Clone() as UnitVoltData;
        }

        public void DescriptionPropertyUpload(double max,double min,double d4Value,string format,string unit)
        {
            EVoltUnit uni = EVoltUnit.V;//切單位要優先執行
            if (Enum.TryParse(unit, out uni))
            {
                cmbForceValueUnit.Text = unit;
            }

            dinForceValue.MaxValue = max;

            dinForceValue.MinValue = min;

            dinForceValue.Value = d4Value;

            dinForceValue.DisplayFormat = format;

            _d4UnitData = new UnitVoltData();

            _d4UnitData.UnitEA = uni;

            _d4UnitData.Maximum = max;
            _d4UnitData.Minimum = min;
            _d4UnitData.DisplayFormat = format;
            _d4UnitData.Value = d4Value;
        }

        public void UploadDataToUI(double forceValue, string forceUnit)
        {
            cmbForceValueUnit.Text = forceUnit;

            dinForceValue.Value = forceValue;
        }

        public UnitVoltData GetDataFromUI()
        {
            UnitVoltData unitData = new UnitVoltData();

            unitData.Maximum = dinForceValue.MaxValue;

            unitData.Minimum = dinForceValue.MinValue;

            unitData.Value = dinForceValue.Value;

            unitData.DisplayFormat = dinForceValue.DisplayFormat;

            foreach (var item in cmbForceValueUnit.Items)
            {
                if (cmbForceValueUnit.Text != item.ToString())
                {
                    continue;
                }

                string unitStr = item.ToString();
                EVoltUnit unit = EVoltUnit.V;
                if (Enum.TryParse<EVoltUnit>(unitStr, out unit))
                {
                    unitData.UnitEA = unit;
                }

            }
            return unitData;
        }

        #endregion

        #region >>public property<<
        public bool EnableModifyUnit 
        {
            get { return cmbForceValueUnit.Enabled; }
            set { cmbForceValueUnit.Enabled = value; }
        }
        #endregion

        #region >>> Private Method <<<

        private void UnitConversionUploadUI()
        {
            UnitVoltData newUnitData = new UnitVoltData();

            newUnitData = this.GetDataFromUI();

            if (_currentUnitData.UnitEA == newUnitData.UnitEA)
            {
                return;
            }

            double conversion = UnitMath.UnitConvertFactor(_currentUnitData.UnitEA, newUnitData.UnitEA.ToString());

            double convertfromD4 = UnitMath.UnitConvertFactor(this._d4UnitData.UnitEA, newUnitData.UnitEA.ToString());

            newUnitData.Maximum = _d4UnitData.Maximum * convertfromD4;

            newUnitData.Minimum = _d4UnitData.Minimum * convertfromD4;

            newUnitData.Value = _currentUnitData.Value * conversion;

            dinForceValue.MaxValue = newUnitData.Maximum;

            dinForceValue.MinValue = newUnitData.Minimum;

            dinForceValue.Value = newUnitData.Value;

        }


        private void OnUnitchange(object sender, EventArgs e)
        {
            if (Unit_Change != null)
                Unit_Change(this, e);
        }
        #endregion        

        private void cmbForceValueUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UnitConversionUploadUI();

            OnUnitchange(sender,e);
        }

        private void dinForceValue_ValueChanged(object sender, EventArgs e)
        {
            _currentUnitData = this.GetDataFromUI();
        }
    }
}
