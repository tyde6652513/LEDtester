using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Gui
{
    internal class GraphCurveItem
    {
        private string _itemKeyName;
        private string _itemName;
        private List<AxisMsrtItem> _axisItemsX;
        private List<AxisMsrtItem> _axisItemsY;
        private string _selectedMsrtKeyNameX;
        private string _selectedMsrtKeyNameY;
        private int _dataCount;

        public GraphCurveItem(string itemKeyName, string name)
        {
            this._itemKeyName = itemKeyName;
            this._itemName = name;
            this._selectedMsrtKeyNameX = string.Empty;
            this._selectedMsrtKeyNameY = string.Empty;
            this._axisItemsX = new List<AxisMsrtItem>();
            this._axisItemsY = new List<AxisMsrtItem>();

            this._dataCount = 0;
        }

        public string ItemKeyName
        {
            get { return this._itemKeyName; }
            set { this._itemKeyName = value; }
        }

        public string ItemName
        {
            get { return this._itemName; }
            set { this._itemName = value; }
        }

        public List<AxisMsrtItem> AxisItemsX
        {
            get { return this._axisItemsX; }
            set { this._axisItemsX = value; }
        }

        public List<AxisMsrtItem> AxisItemsY
        {
            get { return this._axisItemsY; }
            set { this._axisItemsY = value; }
        }

        public string SelectedMsrtKeyNameX
        {
            get { return this._selectedMsrtKeyNameX; }
            set { this._selectedMsrtKeyNameX = value; }
        }

        public string SelectedMsrtKeyNameY
        {
            get { return this._selectedMsrtKeyNameY; }
            set { this._selectedMsrtKeyNameY = value; }
        }

        public int DataCount
        {
            get { return this._dataCount; }
            set { this._dataCount = value; }
        }
    }

    internal class AxisMsrtItem
    {
        private string _msrtKeyName;
        private string _msrtName;
        private string _unit;

        public AxisMsrtItem(string keyName, string name, string unit)
        {
            this._msrtKeyName = keyName;
            this._msrtName = name;
            this._unit = unit;
        }

        public string KeyName
        {
            get { return this._msrtKeyName; }
            set { this._msrtKeyName = value; }
        }

        public string Name
        {
            get { return this._msrtName; }
            set { this._msrtName = value; }
        }

        public string AxisLabel
        {
            get
            {
                if (this._unit == string.Empty)
                {
                    return this._msrtName;
                }
                else
                {
                    return string.Format("{0} ({1})", this._msrtName, this._unit);
                }
            }
        }
    }
}
