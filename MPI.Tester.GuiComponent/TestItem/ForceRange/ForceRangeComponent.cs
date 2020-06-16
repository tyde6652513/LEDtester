using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security;

using MPI.Tester.GuiComponent.Unit;

namespace MPI.Tester.GuiComponent
{
    public partial class ForceRangeComponent : UserControl
    {
        List<EAmpUnit> cList;
        List<EVoltUnit> vList;
        Dictionary<string, double> rangeValDic = new Dictionary<string, double>();

        public ForceRangeComponent()
        {
            //ForceType = EForceType.Current;
            InitializeComponent();
            cmbForceRange.Items.Add("Best Fit Range");
            cmbForceRange.SelectedIndex = 0;
            
            cList = EnumHelper.GetOrderByList<EAmpUnit>();
            vList = EnumHelper.GetOrderByList<EVoltUnit>();
        }
        #region >>pbulic property<<
        public EForceType ForceType { set; get; }
        #endregion

        #region  >>public  method<<
        public void SetItemList(List<double> rangeList, bool ableToAutoRange = false)
        {
            cmbForceRange.Items.Clear();
            cmbForceRange.Items.Add("Best Fit Range");
            cmbForceRange.SelectedIndex = 0;
            if (ableToAutoRange)
            {
                cmbForceRange.Items.Add("Auto Range");
            }


            if (rangeList != null)
            {
                Dictionary<string, double> rvDic = new Dictionary<string, double>();
                foreach (double val in rangeList)
                {
                    if (ForceType == EForceType.Current)
                    {
                        AddForceRange2Dic(rvDic, val);

                    }
                    else if (ForceType == EForceType.Voltage)
                    {
                        AddForceRange2Dic(rvDic, val);
                    }
                }
                rangeValDic = rvDic;
            }
            //cmbForceRange.Refresh();

        }

        public void SetRange(double range, bool isAuto, bool isBestFit)
        {
            if (isAuto && cmbForceRange.Items.Contains("Auto Range"))//Auto range優先
            {
                cmbForceRange.SelectedIndex = cmbForceRange.FindString("Auto Range");
            }
            else if(isBestFit)
            {
                cmbForceRange.SelectedIndex = cmbForceRange.FindString("Best Fit Range");
            }             
            else
            {
                foreach (var p in rangeValDic)
                {
                    if (p.Value == range)
                    {
                        cmbForceRange.SelectedIndex = cmbForceRange.FindString(p.Key);
                        return;
                    }
                }
                cmbForceRange.SelectedIndex = cmbForceRange.FindString("Best Fit Range");
            }
            
        }

        public double GetForceRange(out bool isAuto, out bool isBestFit)
        {
            isAuto = false;
            isBestFit = false;
            string str = cmbForceRange.SelectedItem.ToString();
            switch (str)
            {
                case "Best Fit Range":
                    {
                        isAuto = false;
                        isBestFit = true;
                        return -1; }
                    break;
                case "Auto Range":
                    {
                        isAuto = true;
                        isBestFit = false;
                        return -2; }
                    break;
                default:
                    {
                        if (rangeValDic.ContainsKey(str))
                        {
                            return rangeValDic[str];
                        }
                        else
                        {
                            isAuto = false;
                            isBestFit = true;
                        }
                    }
                    break;
            }
            Console.WriteLine("[ForceRangeComponent]GetForceRange(),not found any fit range");
            return -1;
        }

        public double GetForceRange(string unit,out bool isAuto, out bool isBestFit)
        {
            isAuto = false;
            isBestFit = false;
            string str = cmbForceRange.SelectedItem.ToString();
            double factor = UnitMath.ToSIUnit(unit);
            switch (str)
            {
                case "Best Fit Range":
                    {
                        isAuto = false;
                        isBestFit = true;
                        return -1; }
                    break;
                case "Auto Range":
                    {
                        isAuto = true;
                        isBestFit = false;
                        return -2; }
                    break;
                default:
                    {
                        if (rangeValDic.ContainsKey(str) && factor != 0)
                        {
                            return rangeValDic[str] / factor;
                        }
                        else
                        {
                            isAuto = false;
                            isBestFit = true;
                        }
                    }
                    break;
            }
            Console.WriteLine("[ForceRangeComponent]GetForceRange(),not found any fit range");
            return -1;
        }
        #endregion

        #region  >>private mathod<<

        private void AddForceRange2Dic(Dictionary<string, double> rvDic, double val)
        {
            string strOut = "";
            strOut = FindRangeName(val);

            if (!rvDic.ContainsKey(strOut))
                rvDic.Add(strOut, val);
            cmbForceRange.Items.Add(strOut);
        }

        private string FindRangeName(double val)
        {
            string strOut = val.ToString("0.##########") + EAmpUnit.A.ToString();
            if (ForceType == EForceType.Current)
            {
                foreach (var u in cList)
                {
                    double refVal = Math.Pow(10, (int)u);
                    if (refVal <= val)
                    {
                        double newUnitVal = (val / refVal);
                        strOut = newUnitVal.ToString("0.#") + u.ToString();
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else if (ForceType == EForceType.Voltage)
            {
                strOut = val.ToString("0.##########") + EVoltUnit.V.ToString();
                foreach (var u in vList)
                {
                    double refVal = Math.Pow(10, (int)u);
                    if (refVal <= val)
                    {
                        double newUnitVal = (val / refVal);
                        strOut = newUnitVal.ToString("0.#") + u.ToString();
                    }
                    else
                    {
                        break;
                    }
                }
            }
            

            return strOut;
        }

        #endregion
    }

   
    public static class EnumHelper
    {
        public static List<T> ToList<T>() 
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList<T>();
        }

        public static List<T> GetOrderByList<T>() where T :  IComparable, IFormattable, IConvertible
        {
            List<T> uList = ToList<T>();
            uList = (from u in uList
                     orderby u
                     select u).ToList();
            return uList;
        }
        public static IEnumerable<T> ToEnumerable<T>() 
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

    }

}
