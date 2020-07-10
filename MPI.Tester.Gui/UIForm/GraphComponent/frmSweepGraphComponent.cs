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
    public partial class frmSweepGraphComponent : Form
    {
        private ElecSweepDataSet _dataSet;

        private uint _channel;
        private string _curveName;
        private string _xAxisName;
        private string _yAxisName;

        private string _xLabel;
        private string _yLabel;

        private string _defaultCurveName;

        public frmSweepGraphComponent()
        {
            InitializeComponent();
            this.splitContainer1.FixedPanel = FixedPanel.Panel1;

            this._channel = 0;
            this._curveName = string.Empty;
            this._xAxisName = string.Empty;
            this._yAxisName = string.Empty;
            this._xLabel = string.Empty;
            this._yLabel = string.Empty;

            this.cmbXAxis.Items.Add("Current");
            this.cmbXAxis.Items.Add("Voltage");
            
            this.cmbXAxis.Items.Add("Time");
            

            this.cmbYAxis.Items.Add("Voltage");
            this.cmbYAxis.Items.Add("Current");
            this.cmbYAxis.Items.Add("Derivative");

            this.cmbXAxis.SelectedIndex = 0;
            this.cmbYAxis.SelectedIndex = 0;

            this.cmbSelectItem.SelectedIndexChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.cmbXAxis.SelectedIndexChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.cmbYAxis.SelectedIndexChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.cmbChannel.SelectedIndexChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.chkyLog.CheckedChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.chkxLog.CheckedChanged += new System.EventHandler(this.UpdateDataEventHandler);

            this.lblChannel.Visible = false;
            this.cmbChannel.Visible = false;
        }

        public frmSweepGraphComponent(string defaultKeyName) : this()
        {
            this._defaultCurveName = defaultKeyName;
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

        #endregion

        #region >>> Private Method <<<

        private void UpdateDataEventHandler(object sender, EventArgs e)
        {
            this.UpdateSelectedItems();

            this.DrawCurveToGraph();
        }

        private void UpdateSelectedItems()
        {
            if (this.cmbSelectItem.SelectedIndex < 0 || this.cmbChannel.SelectedIndex < 0)
            {
                return;
            }

            this._channel = 0;
            this._curveName = this.cmbSelectItem.SelectedItem.ToString();
            this._xAxisName = this.cmbXAxis.SelectedItem.ToString();
            this._yAxisName = this.cmbYAxis.SelectedItem.ToString();

            this._xLabel = this.GetAxisLabel(this._xAxisName, _curveName);
            this._yLabel = this.GetAxisLabel(this._yAxisName, _curveName);

            uint.TryParse(this.cmbChannel.SelectedItem.ToString(), out this._channel);

            this._channel -= 1;  // Display base = 1; S/W channel base = 0;
        }

        private void UpdateLinearEq(double[] x, double[] y)
        {
            if (x == null || y == null)
            {
                return;
            }

            if (x.Length <= 1)
            {
                return;
            }

            double rsqure = 0.0d;
            double slope = 0.0d;
            double intercept = 0.0d;

            MPI.Tester.Maths.LinearRegress.Calculate(x, y, out slope, out intercept, out rsqure);

            this.lblLinearEq.Text = string.Format("R_square = {0}; y = {1}x + {2}", rsqure.ToString("0.000"), slope.ToString("0.000"), intercept.ToString("0.000"));
        }

        private void DrawCurveToGraph()
        {
            if (!this.Visible)
            {
                return;
            }

            this.lblLinearEq.Text = string.Empty;

            PlotGraph.Clear(this.zGraph);

            if (this._dataSet != null)
            {
                if (this._dataSet.ContainKeyName(this._curveName))
                {
                    PlotGraph.SetGrid(this.zGraph, false, Color.Silver, Color.Transparent);

                    PlotGraph.SetLabel(this.zGraph, "", this._xLabel, this._yLabel, 14);

                    double[] xData = this.GetAxisData(this._curveName, this._channel, this._xAxisName);

                    double[] yData = this.GetAxisData(this._curveName, this._channel, this._yAxisName);

                    PlotGraph.DrawPlot(this.zGraph, xData, yData, true, 1.0F, Color.Blue, ZedGraph.SymbolType.Square, true, true, true, this._curveName, xlogMode: chkxLog.Checked, ylogMode: chkyLog.Checked);

                    this.UpdateLinearEq(xData, yData);
                }
            }
        }

        private string GetAxisLabel(string axisName,string curveName)
        {
            string lbl = axisName;

            switch(axisName)
            {
                case "Current":
                    {
                        lbl += " (A)";
                        break;
                    }
                case "Voltage":
                    {
                        lbl += " (V)";
                        break;
                    }
                case "Time":
                    {
                        lbl += " (ms)";
                        break;
                    }
                case "Derivative":
                    {
                        if (curveName.Contains("VI"))
                        {
                            lbl += " (dV/dA)";
                        }
                        else if (curveName.Contains("IV"))
                        { lbl += " (dA/dV)"; }
                        break; 
                    }
            }

            return lbl;
        }

        private double[] GetAxisData(string curveName, uint channel, string axisName)
        {
            double[] dArray = null;

            if (this._dataSet == null || this._dataSet.Count == 0)
            {
                return null;
            }

            switch (axisName)
            {
                case "Current":
                    {
                        if (curveName.Contains("VI"))
                        {
                            dArray = this._dataSet[channel, curveName].SweepData;
                           
                        }
                        else
                        {
                            dArray = this._dataSet[channel, curveName].ApplyData;
                        }

                        break;
                    }
                case "Voltage":
                    {
                        if (curveName.Contains("VI"))
                        {
                            dArray = this._dataSet[channel, curveName].ApplyData;
                        }
                        else
                        {
                            dArray = this._dataSet[channel, curveName].SweepData;
                        }
                        break;
                    }
                case "Time":
                    {
                        dArray = this._dataSet[channel, curveName].TimeChain;
                        break;
                    }
                case "Derivative":
                    {
                        dArray = this._dataSet[channel, curveName].Derivative;
                        break;
                    }
            }

            return dArray;
        }

        #endregion

        #region >>> UI Ctrl <<<

        private void frmSweepGraphComponent_VisibleChanged(object sender, EventArgs e)
        {
            this.DrawCurveToGraph();
        }

        #endregion

        #region >>> Public Method <<<

        public void UpdateDataToControl(ElecSweepDataSet data)
        {
            this._dataSet = data;
            
            string[] itemKeyNames = this._dataSet.KeyNames;

            int channelCount = this._dataSet.ChannelCount;

            string oldSelect = string.Empty;

            if (this.cmbSelectItem.Items.Count > 0 && this.cmbSelectItem.SelectedItem != null)
            {
                oldSelect = this.cmbSelectItem.SelectedItem.ToString();
            }
            
            this.cmbChannel.Items.Clear();
            this.cmbSelectItem.Items.Clear();

            if(channelCount < 1)
            {
                channelCount = 1;
            }

            for (int i = 0; i < channelCount; i++)
            {
                this.cmbChannel.Items.Add((i + 1).ToString());
            }

            if (itemKeyNames == null || itemKeyNames.Length == 0)
            {
                return;
            }

            this.cmbSelectItem.Items.AddRange(itemKeyNames);

            this.cmbChannel.SelectedIndex = 0;

            if (oldSelect != string.Empty && this.cmbSelectItem.Items.Contains(oldSelect))
            {
                this.cmbSelectItem.SelectedItem = oldSelect;
            }
            else
            {
                if (this._defaultCurveName != string.Empty && this.cmbSelectItem.Items.Contains(this._defaultCurveName))
                {
                    this.cmbSelectItem.SelectedItem = this._defaultCurveName;
                }
            }

            this.UpdateSelectedItems();
        }

        public void UpdateDataToGraph(ElecSweepDataSet data)
        {
            this._dataSet = data;

            this.DrawCurveToGraph();
        }

        #endregion
    }
}
