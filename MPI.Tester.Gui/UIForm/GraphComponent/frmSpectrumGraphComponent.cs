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
    public partial class frmSpectrumGraphComponent : Form
    {
        private object _lockObj;

        private const string SAVE_PATH_AND_FILENAME = @"C:\MPI\LEDTester\Data\GraphData.dat";
        private string _componentID;

        private ETestType _displayTestType;
        private SpectrumDataSet _dataSetSpt;
        private LIVDataSet _dataSetLIV;  // Transistor / LIV
        
        private uint _channelIndex;

        private string _curveKeyName;
        private string _curveName;
        private string _xAxisName;
        private string _yAxisName;

        private string _xLabel;
        private string _yLabel;

        public frmSpectrumGraphComponent()
        {
            InitializeComponent();

            this._lockObj = new object();

            this._componentID = string.Empty;

            this.splitContainer1.FixedPanel = FixedPanel.Panel1;

            this._channelIndex = 0;
            this._curveKeyName = string.Empty;
            this._xAxisName = "Wavelength";
            this._yAxisName = string.Empty;
            this._xLabel = string.Empty;
            this._yLabel = string.Empty;

            this.rdbAbsoluate.Checked = false;
            this.rdbRelative.Checked = true;
            this._yAxisName = "Relative";

            this.cmbSelectItem.Tag = "Item";
            this.cmbChannel.Tag = "Channel";

            this.cmbSelectItem.SelectedIndexChanged += new System.EventHandler(this.UpdateDataEventHandler);
            this.cmbChannel.SelectedIndexChanged += new System.EventHandler(this.UpdateDataEventHandler);
   
        }

        public frmSpectrumGraphComponent(ETestType testType, string id) : this()
        {
            this._displayTestType = testType;
            this._componentID = id;

            this.LoadState();
        }

        #region >>> Public Property <<<

        public uint NowChannel
        {
            get { return this._channelIndex; }
            set { lock (this._lockObj) { this._channelIndex = value; } } 
        }

        public bool IsAbsoluteSpectrumVisible
        {
            set 
            {
                this.rdbAbsoluate.Visible = value;

                if (value == false)
                {
                    this.rdbAbsoluate.Enabled = false;
                }
            }
        }

        public bool IsSaveScaleVisible
        {
            set
            {
                this.btnSave.Visible = value;
            }
        }

        #endregion

        #region >>> Private Method <<<

        private void UpdateDataEventHandler(object sender, EventArgs e)
        {
            this.UpdateSelectedItems((sender as ComboBox).Tag.ToString());

            this.DrawCurveToGraph();
        }

        private void UpdateSelectedItems(string handler)
        {
            if (handler == this.cmbSelectItem.Tag.ToString())
            {
                if (this.cmbSelectItem.SelectedIndex < 0)
                {
                    return;
                }
                
                int oldIndex = this.cmbChannel.SelectedIndex;

                GraphCurveItem selectedItem = (this.cmbSelectItem.SelectedItem as GraphCurveItem);

                this._curveKeyName = selectedItem.ItemKeyName;
                this._curveName = selectedItem.ItemName;

                this.cmbChannel.Items.Clear();

                for (int index = 0; index < selectedItem.DataCount; index++)
                {
                    this.cmbChannel.Items.Add((index + 1).ToString());
                }

                if (oldIndex >= selectedItem.DataCount || oldIndex < 0)
                {
                    oldIndex = 0;
                }

                this.cmbChannel.SelectedIndex = oldIndex;

                //this._xLabel = this.GetAxisLabel(this._xAxisName);

                //this._yLabel = this.GetAxisLabel(this._yAxisName);
            }
            else if (handler == this.cmbChannel.Tag.ToString())
            {
                this._channelIndex = (uint)this.cmbChannel.SelectedIndex;
            }
        }

        private void DrawCurveToGraph()
        {
            if (!this.Visible)
            {
                return;
            }

            if (this._curveKeyName == string.Empty)
            {
                return;
            }

            PlotGraph.Clear(this.zGraph);

            PlotGraph.SetGrid(this.zGraph, false, Color.Silver, Color.Transparent);

            PlotGraph.SetLabel(this.zGraph, "", this._xLabel, this._yLabel, 14);

            //PlotGraph.SetXYAxis(this.zGraph, (double)this._xStartWL, (double)this._xEndWL, 0.0d, this._yEnd);

            double[] xData = this.GetAxisData(this._curveKeyName, this._channelIndex, this._xAxisName);

            double[] yData = this.GetAxisData(this._curveKeyName, this._channelIndex, this._yAxisName);

            if (xData != null && yData != null)
            {
                PlotGraph.DrawPlot(this.zGraph, xData, yData, true, 1.0F, Color.Blue, ZedGraph.SymbolType.None, false, true, true, this._curveName);
            }
        }

        private string GetAxisLabel(string axisName)
        {
            string lbl = axisName;

            switch(axisName)
            {
                case "Wavelength":
                    {
                        lbl += " (nm)";
                        break;
                    }
                case "Absoluate":
                    {
                        break;
                    }
                case "Relative":
                    {
                        break;
                    }
            }

            return lbl;
        }

        private double[] GetAxisData(string keyName, uint channel, string axisName)
        {
            double[] dArray = null;

            if (this._displayTestType == ETestType.LOPWL)
            {
                if (this._dataSetSpt == null || this._dataSetSpt.Count == 0)
                {
                    return null;
                }

                switch (axisName)
                {
                    case "Wavelength":
                        {
                            dArray = this._dataSetSpt[channel, keyName].Wavelength;

                            break;
                        }
                    case "Absoluate":
                        {
                            dArray = this._dataSetSpt[channel, keyName].Absoluate;

                            break;
                        }
                    case "Relative":
                        {
                            dArray = this._dataSetSpt[channel, keyName].Intensity;

                            break;
                        }
                }
            }
            else if (this._displayTestType == ETestType.LIV || this._displayTestType == ETestType.TRANSISTOR)
            {
                if (this._dataSetLIV == null || this._dataSetLIV.Count == 0)
                {
                    return null;
                }

                switch (axisName)
                {
                    case "Wavelength":
                        {
                            dArray = this._dataSetLIV[keyName].SpectrumDataData[(int)channel].Wavelength;

                            break;
                        }
                    case "Absoluate":
                        {
                            dArray = this._dataSetLIV[keyName].SpectrumDataData[(int)channel].Absoluate;

                            break;
                        }
                    case "Relative":
                        {
                            dArray = this._dataSetLIV[keyName].SpectrumDataData[(int)channel].Intensity;

                            break;
                        }
                }
            }

            return dArray;
        }

        private void SaveState()
        {
            if (this._componentID == string.Empty)
            {
                return;
            }
            
            double minX = this.zGraph.GraphPane.XAxis.Scale.Min;
            double maxX = this.zGraph.GraphPane.XAxis.Scale.Max;
            double maxY = this.zGraph.GraphPane.YAxis.Scale.Max;
            double minY = this.zGraph.GraphPane.YAxis.Scale.Min;

            GraphState status = (GraphState)Deserialize<GraphState>(SAVE_PATH_AND_FILENAME);

            if (status == null)
            {
                status = new GraphState();
            }

            if (status.StateDictionary.ContainsKey(this._componentID))
            {
                status.StateDictionary[this._componentID].Ymax = maxY;
                status.StateDictionary[this._componentID].Ymin = minY;
                status.StateDictionary[this._componentID].Xmax = maxX;
                status.StateDictionary[this._componentID].Xmin = minX;
            }
            else
            {
                State tempState = new State();
                tempState.Ymax = maxY;
                tempState.Ymin = minY;
                tempState.Xmax = maxX;
                tempState.Xmin = minX;

                status.StateDictionary.Add(this._componentID, tempState);
            }

            Serialize(SAVE_PATH_AND_FILENAME, status);

        }

        private void LoadState()
        {
            if (this._componentID == string.Empty)
            {
                return;
            }

            GraphState status = (GraphState)Deserialize<GraphState>(SAVE_PATH_AND_FILENAME);

            if (status != null)
            {
                if (status.StateDictionary.ContainsKey(this._componentID))
                {
                    double Ymax = status.StateDictionary[this._componentID].Ymax;
                    double Ymin = status.StateDictionary[this._componentID].Ymin;
                    double Xmax = status.StateDictionary[this._componentID].Xmax;
                    double Xmin = status.StateDictionary[this._componentID].Xmin;

                    PlotGraph.SetXYAxis(this.zGraph, Xmin, Xmax, Ymin, Ymax);
                }
            } 
        }

        #endregion

        #region >>> UI Ctrl <<<

        private void cmbSelectItem_VisibleChanged(object sender, EventArgs e)
        {
            this.DrawCurveToGraph();
        }

        private void rdbAbsoluate_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdbAbsoluate.Checked)
            {
                this._yAxisName = "Absoluate";
            }
            else
            {
                this._yAxisName = "Relative";
            }

            this._xLabel = this.GetAxisLabel(this._xAxisName);
            this._yLabel = this.GetAxisLabel(this._yAxisName);

			this.DrawCurveToGraph();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.SaveState();

            return;
            
            if (this._curveKeyName == string.Empty)
            {
                return;
            }

            double[] xData = this.GetAxisData(this._curveKeyName, this._channelIndex, this._xAxisName);

            double[] yData = this.GetAxisData(this._curveKeyName, this._channelIndex, this._yAxisName);

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

        #endregion

        #region >>> Public Method <<<

        public void UpdateDataToControl(SpectrumDataSet data)
        {
            this._dataSetSpt = data;

            int channelCount = data.ChannelCount;
            
            this.lblChannel.Text = "Channel";
            this.lblChannel.Visible = true;
            this.cmbChannel.Visible = true;

            //----------------------------------------------------------
            //  SpectrumDataSet, count為 multi-die channel 中的頻譜
            //----------------------------------------------------------
            GraphCurveItem oldSelectedItem = null;

            if (this.cmbSelectItem.Items.Count > 0)
            {
                oldSelectedItem = (this.cmbSelectItem.SelectedItem as GraphCurveItem);
            }

            this.cmbSelectItem.Items.Clear();
            this.cmbSelectItem.DisplayMember = "ItemName";

            if(channelCount < 1)
            {
                channelCount = 1;
                this.lblChannel.Visible = false;
                this.cmbChannel.Visible = false;
            }

            for (int i = 0; i < this._dataSetSpt.Count; i++)
            {
                SpectrumData item = this._dataSetSpt[i];

                GraphCurveItem newCurveItem = new GraphCurveItem(item.KeyName, item.Name);

                newCurveItem.DataCount = channelCount;

                this.cmbSelectItem.Items.Add(newCurveItem);
            }

            if (this.cmbSelectItem.Items.Count > 0)
            {
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
        }

        public void UpdateDataToControl(LIVDataSet data)
        {
            this._dataSetLIV = data;
            
            this.lblChannel.Text = "Point Index";
            
            //----------------------------------------------------------
            //  LIVDataSet, count為 sweep points 中的頻譜
            //----------------------------------------------------------
            GraphCurveItem oldSelectedItem = null;

            if (this.cmbSelectItem.Items.Count > 0)
            {
                oldSelectedItem = (this.cmbSelectItem.SelectedItem as GraphCurveItem);
            }

            this.cmbSelectItem.Items.Clear();

            this.cmbSelectItem.DisplayMember = "ItemName";

            for (int i = 0; i < this._dataSetLIV.Count; i++)
            {
               LIVData item = this._dataSetLIV[i];

               GraphCurveItem newCurveItem = new GraphCurveItem(item.KeyName, item.Name);

               newCurveItem.DataCount = item.SpectrumDataData.Count;

               this.cmbSelectItem.Items.Add(newCurveItem);
            }

           if (this.cmbSelectItem.Items.Count > 0)
           {
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
        }

        public void UpdateDataToGraph(SpectrumDataSet data)
        {
            this._dataSetSpt = data;

            this.DrawCurveToGraph();
        }

        public void UpdateDataToGraph(LIVDataSet data)
        {
            this._dataSetLIV = data;

            this.DrawCurveToGraph();
        }

        #endregion


        public static bool Serialize(string FileName, object Obj)
        {
            try
            {
                if (System.IO.Path.GetExtension(FileName).ToLower() == ".xml")
                {
                    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Obj.GetType());
                    System.Xml.XmlTextWriter xmlTextWriter = new System.Xml.XmlTextWriter(FileName, Encoding.ASCII);
                    x.Serialize(xmlTextWriter, Obj);
                    xmlTextWriter.Close();
                }
                else if (System.IO.Path.GetExtension(FileName).ToLower() == ".dat")
                {
                    using (System.IO.FileStream fileStream = new System.IO.FileStream(FileName, System.IO.FileMode.Create))
                    {
                        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        binaryFormatter.Serialize(fileStream, Obj);

                        fileStream.Close();
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static T Deserialize<T>(string FileName)
        {
            System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();

            try
            {
                object obj = new object();

                if (System.IO.Path.GetExtension(FileName).ToLower() == ".xml")
                {
                    xdoc.Load(FileName);
                    System.Xml.XmlNodeReader reader = new System.Xml.XmlNodeReader(xdoc.DocumentElement);
                    System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    obj = ser.Deserialize(reader);
                }
                else if (System.IO.Path.GetExtension(FileName).ToLower() == ".dat")
                {
                    using (System.IO.FileStream fileStream = new System.IO.FileStream(FileName, System.IO.FileMode.Open))
                    {
                        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        obj = binaryFormatter.Deserialize(fileStream);

                        fileStream.Close();
                    }
                }

                return (T)obj;
            }
            catch
            {
                return default(T);
            }
        }
    }

    [Serializable]
    internal class GraphState
    {
        public Dictionary<string, State> StateDictionary = new Dictionary<string, State>();
        
        public GraphState()
        {

        }
    }

    [Serializable]
    internal class State
    {
        public double Ymax;
        public double Ymin;
        public double Xmax;
        public double Xmin;
        
        public State()
        {
 
        }
    }


}

