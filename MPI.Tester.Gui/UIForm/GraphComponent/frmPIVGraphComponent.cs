using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MPI.Tester.Data;
using ZedGraph;

namespace MPI.Tester.Gui
{
    public partial class frmPIVGraphComponent : Form
    {
        private PIVDataSet _dataSet;

        private string _curveName;

        private Dictionary<string, AxisDescription> _dicYAxis;
        //DrawCurveToGraph
        //private List<string> _yLabelNameList = new List<string>();
        //private List<Color> _yLabelColorList = new List<Color>();

        public frmPIVGraphComponent()
        {
            InitializeComponent();
            this.splitContainer1.FixedPanel = FixedPanel.Panel1;

            this._dicYAxis = new Dictionary<string, AxisDescription>();

            this.cmbSelectItem.SelectedIndexChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.chkIsShowIV.CheckedChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.chkIsShowSE.CheckedChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.chkIsShowRS.CheckedChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.chkIsShowPCE.CheckedChanged += new System.EventHandler(this.UpdateDataEventHandler);
        }

        #region >>> Public Property <<<

        public string SelectedItem
        {
            get
            {
                if (this.cmbSelectItem.SelectedIndex < 0)
                {
                    return null;
                }

                return this.cmbSelectItem.SelectedItem.ToString();
            }
        }

        public bool IsEnableSE
        {
            set
            {
                this.chkIsShowSE.Visible = value;
            }
        }

        public bool IsEnableRS
        {
            set
            {
                this.chkIsShowRS.Visible = value;
            }
        }

        public bool IsEnablePCE
        {
            set
            {
                this.chkIsShowPCE.Visible = value;
            }
        }

        #endregion

        #region >>> Private Method <<<

        private void UpdateDataEventHandler(object sender, EventArgs e)
        {
            this.UpdateSelectedItems();

            this.DrawCurveToGraph();
        }

        private void UpdateSelectedItems()
        {
            if (this.cmbSelectItem.SelectedIndex < 0)
            {
                return;
            }

            this._curveName = this.cmbSelectItem.SelectedItem.ToString();
            
            this._dicYAxis.Clear();

            int yAxisIndex = -1;
            int y2AxisIndex = -1;

            this._dicYAxis.Add("IP", new AxisDescription(++yAxisIndex, "Power", "mW", Color.Red, AxisIndex.Y1));

            if (this.chkIsShowIV.Checked)
            {
                this._dicYAxis.Add("IV", new AxisDescription(++yAxisIndex, "Voltage", "V", Color.Blue, AxisIndex.Y1));
            }

            if (this.chkIsShowSE.Checked)
            {
                this._dicYAxis.Add("SE", new AxisDescription(++y2AxisIndex, "SE", "W/A", Color.Green, AxisIndex.Y2));
            }

            if (this.chkIsShowRS.Checked)
            {
                this._dicYAxis.Add("RS", new AxisDescription(++y2AxisIndex, "RS", "Ohm", Color.Orange, AxisIndex.Y2));
            }

            if (this.chkIsShowPCE.Checked)
            {
                this._dicYAxis.Add("PCE", new AxisDescription(++y2AxisIndex, "PCE", "%", Color.Purple, AxisIndex.Y2));
            }

            if (this._dicYAxis.Keys.Count == 0)
            {
                return;
            }

            List<string> yLabelNameList = new List<string>();
            List<string> y2LabelNameList = new List<string>();

            List<Color> yLabelColorList = new List<Color>();
            List<Color> y2LabelColorList = new List<Color>();
 
            // Update Y Axis Name
            foreach (var kvp in this._dicYAxis)
            {
                string str = string.Format("{0} ({1})", kvp.Value.AxisName, kvp.Value.Unit);

                if (kvp.Value.AxisIndex == AxisIndex.Y2)
                {
                    y2LabelNameList.Add(str);
                    y2LabelColorList.Add(kvp.Value.CurveColor);
                }
                else
                {
                    yLabelNameList.Add(str);
                    yLabelColorList.Add(kvp.Value.CurveColor);
                }
            }

            PlotGraph.SetGrid(this.zGraph, false, Color.Silver, Color.Transparent);
            PlotGraph.SetLabel(this.zGraph, "", "Current (A)", yLabelNameList.ToArray(), y2LabelNameList.ToArray(), yLabelColorList.ToArray(), y2LabelColorList.ToArray(), 12);
        }

        private void DrawCurveToGraph()
        {
            if (!this.Visible)
            {
                return;
            }
            
            if (this._curveName == string.Empty)
            {
                return;
            }

            PlotGraph.Clear(this.zGraph);

            //PlotGraph.SetGrid(this.zGraph, false, Color.Silver, Color.Transparent);

            //if (this._dicYAxis.Keys.Count == 0)
            //{
            //    return;
            //}

            //this._yLabelNameList.Clear();
            //this._yLabelColorList.Clear();

            //// Update Y Axis Name
            //foreach (var kvp in this._dicYAxis)
            //{
            //    string str = string.Format("{0} ({1})", kvp.Value.AxisName, kvp.Value.Unit);

            //    this._yLabelNameList.Add(str);
            //    this._yLabelColorList.Add(kvp.Value.CurveColor);
            //}

            //PlotGraph.SetLabel(this.zGraph, "", "Current (A)", this._yLabelNameList.ToArray(), this._yLabelColorList.ToArray(), 14);

            double[] xData = this.GetAxisData(this._curveName, "Current");

            if (xData != null)
            {
                // Update Y Curve Data
                foreach (var kvp in this._dicYAxis)
                {
                    double[] yData = this.GetAxisData(this._curveName, kvp.Value.AxisName);

                    if (yData != null && yData.Length > 0)
                    {
                        if (kvp.Value.AxisIndex == AxisIndex.Y1)
                        {
                            PlotGraph.DrawPlot(this.zGraph, xData, yData, true, 1.0F, kvp.Value.CurveColor, ZedGraph.SymbolType.None, false, true, true, kvp.Key, kvp.Value.Index);
                        }
                        else
                        {
                            PlotGraph.DrawDeputyPlot(this.zGraph, xData, yData, true, 1.0F, kvp.Value.CurveColor, ZedGraph.SymbolType.None, false, true, true, kvp.Key, kvp.Value.Index);
                        }
                    }
                }
            }

            this.zGraph.Refresh();
        }

        private double[] GetAxisData(string curveName, string axisName)
        {
            double[] dArray = null;

            if (this._dataSet == null || this._dataSet.Count == 0)
            {
                return null;
            }

            switch (axisName)
            {
                case "Power":
                    {
                        dArray = this._dataSet[curveName].PowerData;
                        break;
                    }
                case "Current":
                    {
                        dArray = this._dataSet[curveName].CurrentData;
                        break;
                    }
                case "Voltage":
                    {
                        dArray = this._dataSet[curveName].VoltageData;
                        break;
                    }
                case "SE":
                    {
                        dArray = this._dataSet[curveName].SeData;
                        break;
                    }
                case "RS":
                    {
                        dArray = this._dataSet[curveName].RsData;
                        break;
                    }
                case "PCE":
                    {
                        dArray = this._dataSet[curveName].PceData;
                        break;
                    }
               
            }

            return dArray;
        }

        #endregion

        #region >>> Public Method <<<

        public void UpdateDataToControl(PIVDataSet data)
        {
            this._dataSet = data;

            string[] itemKeyNames = this._dataSet.KeyNames;
            
            this.cmbSelectItem.Items.Clear();

            if (itemKeyNames == null || itemKeyNames.Length == 0)
            {
                return;
            }

            this.cmbSelectItem.Items.AddRange(itemKeyNames);

            this.cmbSelectItem.SelectedIndex = 0;

            this.UpdateSelectedItems();
        }

        public void UpdateDataToGraph(PIVDataSet data)
        {
            this._dataSet = data;

            this.DrawCurveToGraph();
        }

        private void frmPIVGraphComponent_VisibleChanged(object sender, EventArgs e)
        {
            this.DrawCurveToGraph();
        }

        #endregion
    }

    internal class AxisDescription : ICloneable
    {
        private object _lockObj;
        private int _index;
        private string _axisName;
        private string _unit;
        private Color _color;

        private AxisIndex _axisY;

        public AxisDescription()
        {
            this._lockObj = new object();

            this._axisY = AxisIndex.Y1;
        }

        public AxisDescription(int index, string axisName, string unit, Color color, AxisIndex yAxisIndex)
            : this()
        {
            this._lockObj = new object();
            this._index = index;
            this._axisName = axisName;
            this._unit = unit;
            this._color = color;
            this._axisY = yAxisIndex;
        }

        #region >>> Public Property <<<

        public int Index
        {
            get { return this._index; }
            set { lock (this._lockObj) { this._index = value; } }
        }

        public string AxisName
        {
            get { return this._axisName; }
            set { lock (this._lockObj) { this._axisName = value; } }
        }

        public string Unit
        {
            get { return this._unit; }
            set { lock (this._lockObj) { this._unit = value; } }
        }

        public Color CurveColor
        {
            get { return this._color; }
            set { lock (this._lockObj) { this._color = value; } }
        }

        public AxisIndex AxisIndex
        {
            get { return this._axisY; }
            set { lock (this._lockObj) { this._axisY = value; } }
        }

        #endregion

        #region >>> Public Method <<<

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }

    internal enum AxisIndex : uint
    {
        Y1 = 1,
        Y2 = 2,
    }
}
