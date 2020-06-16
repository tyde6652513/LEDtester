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
    public partial class frmLCRGraphComponent : Form
    {
        private LCRDataSet _dataSet;

        private ETestType _displayTestType;

        private string _curveKeyName;
        private string _curveName;
        private string _xAxisKeyName;
        private string _yAxisKeyName;
        private string _xLabel;
        private string _yLabel;

        private Color[] _color = new Color[] { Color.Blue, Color.Red, Color.Green, Color.Brown, Color.Black };

        public frmLCRGraphComponent()
        {
            InitializeComponent();
            this.splitContainer1.FixedPanel = FixedPanel.Panel1;

            this._curveKeyName = string.Empty;
            this._curveName = string.Empty;
            this._xAxisKeyName = string.Empty;
            this._yAxisKeyName = string.Empty;
            this._xLabel = string.Empty;
            this._yLabel = string.Empty;

            this._displayTestType = ETestType.LIV;

            this.cmbSelectItem.Tag = "Item";
            this.cmbXAxis.Tag = "XAxis";
            this.cmbYAxis.Tag = "YAxis";

            this.cmbSelectItem.SelectedIndexChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.cmbXAxis.SelectedIndexChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.cmbYAxis.SelectedIndexChanged += new System.EventHandler(this.UpdateDataEventHandler);
        }

        public frmLCRGraphComponent(ETestType testType)
            : this()
        {
            this._displayTestType = testType;
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

        private void UpdateSelectedItems(string handler)
        {
            if (this.cmbSelectItem.SelectedIndex < 0 || handler == string.Empty)
            {
                return;
            }

            GraphCurveItem selectedItem = (this.cmbSelectItem.SelectedItem as GraphCurveItem);

            if (handler == this.cmbSelectItem.Tag.ToString())
            {
                this._curveKeyName = selectedItem.ItemKeyName;
                this._curveName = selectedItem.ItemName;
                this._xAxisKeyName = string.Empty;
                this._yAxisKeyName = string.Empty;

                //-------------------------------------------------------------------------------
                // Update cmbXAxis
                int oldAxisIndexX = 0;
                int oldAxisIndexY = 0;

                this.cmbXAxis.Items.Clear();
                this.cmbXAxis.DisplayMember = "Name";

                if (selectedItem.AxisItemsX.Count > 0)
                {
                    this.cmbXAxis.Items.AddRange(selectedItem.AxisItemsX.ToArray());

                    if (selectedItem.SelectedMsrtKeyNameX != string.Empty)
                    {
                        foreach (var obj in this.cmbXAxis.Items)
                        {
                            if ((obj as AxisMsrtItem).KeyName == selectedItem.SelectedMsrtKeyNameX)
                            {
                                break;
                            }

                            oldAxisIndexX++;
                        }
                    }

                    this.cmbXAxis.SelectedIndex = oldAxisIndexX;
                }

                //-------------------------------------------------------------------------------
                // Update cmbYAxis
                this.cmbYAxis.Items.Clear();
                this.cmbYAxis.DisplayMember = "Name";

                if (selectedItem.AxisItemsY.Count > 0)
                {
                    this.cmbYAxis.Items.AddRange(selectedItem.AxisItemsY.ToArray());

                    if (selectedItem.SelectedMsrtKeyNameY != string.Empty)
                    {
                        foreach (var obj in this.cmbYAxis.Items)
                        {
                            if ((obj as AxisMsrtItem).KeyName == selectedItem.SelectedMsrtKeyNameY)
                            {
                                break;
                            }

                            oldAxisIndexY++;
                        }
                    }

                    this.cmbYAxis.SelectedIndex = oldAxisIndexY;
                }
            }
            else if (handler == this.cmbXAxis.Tag.ToString())
            {
                if (this.cmbXAxis.SelectedItem != null)
                {
                    this._xAxisKeyName = (this.cmbXAxis.SelectedItem as AxisMsrtItem).KeyName;
                    this._xLabel = (this.cmbXAxis.SelectedItem as AxisMsrtItem).AxisLabel;
                    selectedItem.SelectedMsrtKeyNameX = this._xAxisKeyName;
                }
            }
            else if (handler == this.cmbYAxis.Tag.ToString())
            {
                if (this.cmbYAxis.SelectedItem != null)
                {
                    this._yAxisKeyName = (this.cmbYAxis.SelectedItem as AxisMsrtItem).KeyName;
                    this._yLabel = (this.cmbYAxis.SelectedItem as AxisMsrtItem).AxisLabel;
                    selectedItem.SelectedMsrtKeyNameY = this._yAxisKeyName;
                }
            }
        }

        private void DrawCurveToGraph()
        {
            if (!this.Visible)
            {
                return;
            }

            if (this._curveKeyName == string.Empty || this._xAxisKeyName == string.Empty || this._yAxisKeyName == string.Empty)
            {
                return;
            }

            PlotGraph.Clear(this.zGraph);

            PlotGraph.SetGrid(this.zGraph, false, Color.Silver, Color.Transparent);

            if (this._curveKeyName != "ALL")
            {
                PlotGraph.SetLabel(this.zGraph, "", this._xLabel, this._yLabel, 14);

                double[] xData = this.GetAxisData(this._curveKeyName, this._xAxisKeyName);

                double[] yData = this.GetAxisData(this._curveKeyName, this._yAxisKeyName);

                if (xData != null && yData != null)
                {
                    PlotGraph.DrawPlot(this.zGraph, xData, yData, true, 1.0F, Color.Blue, ZedGraph.SymbolType.None, false, true, true, this._curveName);
                }
            }

        }

        private double[] GetAxisData(string curveName, string axisName)
        {
            double[] dArray = null;
            float[] fArray = null;

            if (this._dataSet == null || this._dataSet.Count == 0)
            {
                return null;
            }

            if (this._dataSet[curveName][axisName] == null)
            {
                return null;
            }

            fArray = this._dataSet[curveName][axisName].DataArray.ToArray();

            if (fArray != null)
            {
                dArray = Array.ConvertAll(fArray, x => (double)x);
            }

            return dArray;
        }

        private Color GetColor(int index)
        {
            Color color;

            if (index < this._color.Length)
            {
                color = this._color[index];
            }
            else
            {
                color = Color.Bisque;
            }

            return color;
        }

        #endregion

        #region >>> Public Method <<<

        public void UpdateDataToControl(LCRDataSet data)
        {
            this._dataSet = data;

            GraphCurveItem oldSelectedItem = null;

            //-------------------------------------------------------------------------------
            // Update cmbSelectItem
            if (this.cmbSelectItem.Items.Count > 0)
            {
                oldSelectedItem = (this.cmbSelectItem.SelectedItem as GraphCurveItem);
            }

            this.cmbSelectItem.Items.Clear();

            this.cmbSelectItem.DisplayMember = "ItemName";

            for (int i = 0; i < this._dataSet.Count; i++)
            {
                LCRData item = this._dataSet[i];

                GraphCurveItem newCurveItem = null;


                if (this._displayTestType == ETestType.LCRSWEEP && item.KeyName.Contains(ETestType.LCRSWEEP.ToString()))
                {
                    newCurveItem = new GraphCurveItem(item.KeyName, item.Name);
                }

                if (newCurveItem != null)
                {
                    if (oldSelectedItem != null)
                    {
                        if (newCurveItem.ItemKeyName == oldSelectedItem.ItemKeyName)
                        {
                            newCurveItem.SelectedMsrtKeyNameX = oldSelectedItem.SelectedMsrtKeyNameX;
                            newCurveItem.SelectedMsrtKeyNameY = oldSelectedItem.SelectedMsrtKeyNameY;
                        }
                    }

                    foreach (var result in item)
                    {
                        if (result.KeyName.Contains("SETVALUE") || result.KeyName.Contains("TIME"))
                        {
                            // Update X Axis
                            newCurveItem.AxisItemsX.Add(new AxisMsrtItem(result.KeyName, result.Name, result.Unit));
                        }
                        else
                        {
                            // Update Y Axis or Both

                            if (result.KeyName.Contains("LCRVDC") || result.KeyName.Contains("LCRIDC"))
                            {
                                // Update X Axis
                                newCurveItem.AxisItemsX.Add(new AxisMsrtItem(result.KeyName, result.Name, result.Unit));
                            }


                            if (result.IsEnable)
                            {
                                // Update Y Axis
                                newCurveItem.AxisItemsY.Add(new AxisMsrtItem(result.KeyName, result.Name, result.Unit));
                            }
                        }
                    }

                    this.cmbSelectItem.Items.Add(newCurveItem);
                }
            }

            if (this.cmbSelectItem.Items.Count > 0)
            {
                if (this.cmbSelectItem.Items.Count > 1)
                {
                    //this.cmbSelectItem.Items.Add(new GraphCurveItem("ALL", "ALL"));
                }

                int oldItemIndex = 0;

                if (oldSelectedItem != null)
                {
                    foreach (var obj in this.cmbSelectItem.Items)
                    {
                        if ((obj as GraphCurveItem).ItemKeyName == oldSelectedItem.ItemKeyName)
                        {
                            break;
                        }

                        oldItemIndex++;
                    }
                }

                if (oldItemIndex >= this.cmbSelectItem.Items.Count)
                {
                    oldItemIndex = this.cmbSelectItem.Items.Count - 1;
                }

                this.cmbSelectItem.SelectedIndex = oldItemIndex;
            }
            else
            {
                this.cmbXAxis.Items.Clear();
                this.cmbYAxis.Items.Clear();
            }
        }

        public void UpdateDataToGraph(LCRDataSet data)
        {
            this._dataSet = data;

            if (this.Visible)
            {
                this.DrawCurveToGraph();
            }
        }

        private void zGraph_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.DrawCurveToGraph();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this._curveKeyName == string.Empty || this._xAxisKeyName == string.Empty || this._yAxisKeyName == string.Empty)
            {
                return;
            }

            double[] xData = this.GetAxisData(this._curveKeyName, this._xAxisKeyName);

            double[] yData = this.GetAxisData(this._curveKeyName, this._yAxisKeyName);

            if (xData != null && yData != null)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Title = "Save selected data";
                dialog.Filter = "CSV files (*.csv)|*.csv";
                //dialog.InitialDirectory = @"D:\";
                dialog.FileName = this._curveName;
                dialog.FilterIndex = 1;    // default value = 1
                dialog.RestoreDirectory = true;
                dialog.OverwritePrompt = true;

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                string fileNameAndPath = dialog.FileName;

                List<string[]> reportData = new List<string[]>();

                string[] colHeader = new string[2];
                colHeader[0] = this._xLabel;
                colHeader[1] = this._yLabel;

                reportData.Add(colHeader);

                for (int rowCount = 0; rowCount < xData.Length; rowCount++)
                {
                    List<string> lstRowData = new List<string>();

                    lstRowData.Add(xData[rowCount].ToString());
                    lstRowData.Add(yData[rowCount].ToString());

                    reportData.Add(lstRowData.ToArray());
                }

                CSVUtil.WriteCSV(fileNameAndPath, reportData);
            }
        }

        private void frmLCRGraphComponent_VisibleChanged(object sender, EventArgs e)
        {

            if (this.Visible)
            {
                this.DrawCurveToGraph();
            }
        }

        private void UpdateDataEventHandler(object sender, EventArgs e)
        {
            this.UpdateSelectedItems((sender as ComboBox).Tag.ToString());

            this.DrawCurveToGraph();
        }

        #endregion
    }
}
