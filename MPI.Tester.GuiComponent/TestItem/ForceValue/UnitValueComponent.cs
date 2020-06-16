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
    public partial class UnitValueComponent : UserControl 
    {
        UnitValueData _currentUnitData = new UnitValueData();

        UnitValueData _d4UnitData = new UnitValueData();

        public delegate void UnitChangeDel(object sender, EventArgs e);

        public event UnitChangeDel Unit_Change;//像LIV這種有多個輸入的時候作為同步用

        //public delegate void UnitChangeDel(object sender, EventArgs e);
        public event System.EventHandler Value_Change;//像LIV這種有多個輸入的時候作為同步用

        //public UnitType _utype = UnitType.Current;

        public EForceType UType {
            get { return _d4UnitData.Type; }

            set { //_d4UnitData.Type = value;
                SetUIToType(value);
            }
        }

        public UnitValueComponent()
        {
            InitializeComponent();

            UType = EForceType.Current;

            cmbForceValueUnit.Items.Clear();

            var uList = Enum.GetNames(typeof(EAmpUnit));

            var orderList = (from u in uList
                             orderby (int)(Enum.Parse(typeof(EAmpUnit), u)) descending
                             select u).ToList();

            cmbForceValueUnit.Items.AddRange(orderList.ToArray());

            cmbForceValueUnit.SelectedItem = EAmpUnit.mA.ToString();

            _d4UnitData = new UnitValueData(EForceType.Current);

            //SetUIToType( _d4UnitData);//似乎會造成無限迴圈

            _currentUnitData = this.GetDataFromUI();

            //_d4UnitData = _currentUnitData.Clone() as UnitValueData;

            
        }

        #region >>> Public Method <<<

        public void SetUIToType(EForceType type)
        {
            if (UType != type)
            {
                cmbForceValueUnit.Items.Clear();
                switch (type)
                {
                    case EForceType.Current:
                        {
                            var uList = Enum.GetNames(typeof(EAmpUnit));

                            var orderList = (from u in uList
                                             orderby (int)(Enum.Parse(typeof(EAmpUnit), u)) descending
                                             select u).ToList();

                            cmbForceValueUnit.Items.AddRange(orderList.ToArray());

                            cmbForceValueUnit.SelectedItem = EAmpUnit.mA.ToString();

                            _d4UnitData = new UnitValueData(EForceType.Current);
                        }
                        break;
                    case EForceType.Voltage:
                        {
                            var uList = Enum.GetNames(typeof(EVoltUnit));

                            var orderList = (from u in uList
                                             orderby (int)(Enum.Parse(typeof(EVoltUnit), u)) descending
                                             select u).ToList();

                            cmbForceValueUnit.Items.AddRange(orderList.ToArray());

                            cmbForceValueUnit.SelectedItem = EVoltUnit.V.ToString();

                            _d4UnitData = new UnitValueData(EForceType.Voltage);
                        }
                        break;
                }

                dinForceValue.MaxValue = _d4UnitData.Maximum;

                dinForceValue.MinValue = _d4UnitData.Minimum;

                dinForceValue.Value = _d4UnitData.Value;

                dinForceValue.DisplayFormat = _d4UnitData.DisplayFormat;
                //DescriptionPropertyUpload(_d4UnitData);//避免出現自我呼叫的無限迴圈

            }
        }

        public void SetUIToType( UnitValueData descriptionProperty)
        {
            if (UType != descriptionProperty.Type)
            {
                SetUIToType(descriptionProperty.Type);

                _d4UnitData = descriptionProperty.Clone() as UnitValueData;

                //dinForceValue.MaxValue = _d4UnitData.Maximum;

                //dinForceValue.MinValue = _d4UnitData.Minimum;

                //dinForceValue.Value = _d4UnitData.Value;

                //dinForceValue.DisplayFormat = _d4UnitData.DisplayFormat;
                DescriptionPropertyUpload(_d4UnitData);//避免出現自我呼叫的無限迴圈
            }
            
        }


        public void DescriptionPropertyUpload(UnitValueData descriptionProperty)
        {
            _d4UnitData = descriptionProperty.Clone() as UnitValueData;

            SetUIToType(_d4UnitData);

            dinForceValue.MaxValue = descriptionProperty.Maximum;

            dinForceValue.MinValue = descriptionProperty.Minimum;

            dinForceValue.Value = descriptionProperty.Value;

            dinForceValue.DisplayFormat = descriptionProperty.DisplayFormat;
            
        }

    
        public void DescriptionPropertyUpload(EForceType type,double max,double min,double d4Value,string format,string unit)
        {
            EVoltUnit vuni = EVoltUnit.V;//切單位要優先執行
            EAmpUnit iuni = EAmpUnit.mA;//切單位要優先執行
            if (Enum.TryParse(unit, out vuni))
            {
                cmbForceValueUnit.SelectedItem = unit;
            }
            else if (Enum.TryParse(unit, out iuni))
            {
                cmbForceValueUnit.SelectedItem = unit;
            }

            dinForceValue.MaxValue = max;

            dinForceValue.MinValue = min;

            dinForceValue.Value = d4Value;

            dinForceValue.DisplayFormat = format;

            _d4UnitData = new UnitValueData();

            _d4UnitData.Unit = unit;

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

        public UnitValueData GetDataFromUI()
        {
            UnitValueData unitData = new UnitValueData(_d4UnitData.Type);

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
                if (UType == EForceType.Current)
                {
                    EAmpUnit unit = EAmpUnit.mA;
                    if (Enum.TryParse<EAmpUnit>(unitStr, out unit))
                    {
                        unitData.Unit = unitStr;
                    }
                }
                else if (UType == EForceType.Voltage)
                {
                    EVoltUnit unit = EVoltUnit.V;
                    if (Enum.TryParse<EVoltUnit>(unitStr, out unit))
                    {
                        unitData.Unit = unitStr;
                    }

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
            UnitValueData newUnitData = new UnitValueData();

            newUnitData = this.GetDataFromUI();

            if (_currentUnitData.Unit == newUnitData.Unit)
            {
                return;
            }

            //double fac = UnitMath.ToSIUnit("mA");

            double now2SI = UnitMath.ToSIUnit(_currentUnitData.Unit);

            double d42SI = UnitMath.ToSIUnit(_d4UnitData.Unit);

            double new2SI = UnitMath.ToSIUnit(newUnitData.Unit);

            double conversion = now2SI / new2SI;

            double convertfromD4 = d42SI / new2SI;

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
        private void OnValueChange(object sender, EventArgs e)
        {
            if (Value_Change != null)
                Value_Change(this, e);
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
