using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using System.IO;
using MPI.Tester.Data;
using ZedGraph;

namespace MPI.Tester.Gui
{
    public partial class frmViewPIVGraph : Form
    {

        #region >>> Private Properties <<<

        private frmPIVGraphComponent _frmPiv;

        private Dictionary<string, DataCollection> _dicDataCollection;
        private Dictionary<string, PIVData> _dicPIVData;

        private PIVDataSet _reloadPIVdataSet;

        private string _errMsg;

        #endregion
        
        public frmViewPIVGraph()
        {
            InitializeComponent();

            this._frmPiv = new frmPIVGraphComponent();
            this._frmPiv.IsEnableSE = false;
            this._frmPiv.IsEnableRS = false;
            this._frmPiv.IsEnablePCE = false;

            this._dicDataCollection = new Dictionary<string, DataCollection>();
            this._dicPIVData = new Dictionary<string, PIVData>();
            this._reloadPIVdataSet = new PIVDataSet();

            this.AttachFormToPanel(this._frmPiv, this.pnlPIVGraph);
        }

        #region >>> Private Methods <<<

        private bool DataParser(List<string> lstFileNameAndPath)
        {
            this._errMsg = string.Empty;

            if (lstFileNameAndPath.Count > 0)
            {
                try
                {
                    string fileName = string.Empty;

                    string filePath = string.Empty;

                    int fileIndex = 1;

                    this._dicDataCollection.Clear();
      
                    char[] splitCharArray = new char[] { '#', 'C', 'R' };

                    foreach (string fileNameAndPath in lstFileNameAndPath)
                    {
                        fileName = Path.GetFileNameWithoutExtension(fileNameAndPath);

                        filePath = Path.GetDirectoryName(fileNameAndPath);

                        this.lblSourceFilePath.Text = string.Format("Source File Path: {0}", filePath);
                        this.lblSourceFileName.Text = string.Format("Source File Name: {0}", fileName);

                        //------------------------------------------------------------------------------
                        // 讀取 File
                        //------------------------------------------------------------------------------
                        List<String[]> lstArray;

                        lstArray = CSVUtil.ReadCSV(fileNameAndPath);

                        if (lstArray == null)
                        {
                            this._errMsg = "Read File Error";

                            return false;
                        }

                        //------------------------------------------------------------------------------
                        // File數據 加入到 IVData
                        //------------------------------------------------------------------------------
                        int rowCount = 1;
                        int tempIndex;
                        int tempRow;
                        int tempCol;
                        double time = 0.0d;
                        double current = 0.0d;
                        double voltage = 0.0d;
                        double power = 0.0d;

                        //data.Index = this.progBarLoadFile.Value + 1;

                        for (int row = 0; row < lstArray.Count; row++)
                        {
                            if (row % 3 != 0)
                            {
                                continue;
                            }

                            DataCollection data;

                            string keyName = string.Format("#{0}{1}", fileIndex, lstArray[row][0]);

                            if (!this._dicDataCollection.ContainsKey(keyName))
                            {
                                this._dicDataCollection.Add(keyName, new DataCollection());
                            }
                     
                            //----------------------------------------------------------------------------

                            data = this._dicDataCollection[keyName];
                            data.IsVisible = true;
                            data.SourceFileName = fileNameAndPath;

                            string[] keyInfo = keyName.Split(splitCharArray);

                            int.TryParse(keyInfo[1], out tempIndex);
                            int.TryParse(keyInfo[2], out tempCol);
                            int.TryParse(keyInfo[3], out tempRow);

                            data.FileIndex = tempIndex;
                            data.Index = rowCount;
                            data.Col = tempCol;
                            data.Row = tempRow;

                            for (int col = 3; col < lstArray[row].Length; col++)
                            {
                                double.TryParse(lstArray[row][col], out current);      // A
                                double.TryParse(lstArray[row + 1][col], out voltage);  // V
                                double.TryParse(lstArray[row + 2][col], out power);    // mW

                                if (power < 0.0d)
                                {
                                    power = 0.0d;
                                }

                                if (current < 0.0d)
                                {
                                    current = 0.0d;
                                }

                                if (voltage < 0.0d)
                                {
                                    voltage = 0.0d;
                                }

                                current = Math.Round(current, 5, MidpointRounding.AwayFromZero);

                                data.Add(time, current, voltage, power);
                            }

                            rowCount++;
                        }

                        fileIndex++;
                    }
                }
                catch (Exception ex)
                {
                    this._errMsg = string.Format("Data Parse Exception\r\n{0}", ex.Message);

                    return false;
                }
            }

            return true;
        }

        private void UpdateDataToListBox()
        {
            this.listBoxFile.Items.Clear();

            if (this._dicDataCollection.Count > 0)
            {
                foreach (var kvp in this._dicDataCollection)
                {
                    this.listBoxFile.Items.Add(kvp.Key);
                }
            }
        }

        private void DataCollectionToPIVData()
        {
            foreach (string Data_key in _dicDataCollection.Keys)
            {
                PIVData data = new PIVData();
                data.PowerData = _dicDataCollection[Data_key].PowerRawArray;
                data.VoltageData = _dicDataCollection[Data_key].VoltageRawArray;
                data.CurrentData = _dicDataCollection[Data_key].CurrentRawArray;
                data.KeyName = "PIV";
                _dicPIVData.Add(Data_key, data);
            }
        }

        private void AttachFormToPanel(Form frm, Panel pnl)
        {
            frm.TopLevel = false;
            frm.Parent = pnl;
            frm.Dock = DockStyle.Fill;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Show();
        }
       
        #endregion

        #region >>> UI Events <<<

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Import PIV Data";
            dialog.Filter = "CSV files (*.csv)|*.csv";
            dialog.FileName = "";
            dialog.InitialDirectory = DataCenter._uiSetting.LIVDataSavePath;
            dialog.FilterIndex = 1;    // default value = 1
            dialog.Multiselect = false;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this._dicPIVData.Clear();

                if (dialog.FileNames.Length > 0)
                {
                    List<string> tempFileNameAndPath = new List<string>();

                    tempFileNameAndPath.AddRange(dialog.FileNames);

                    if (this.DataParser(tempFileNameAndPath))
                    {
                        this.DataCollectionToPIVData();

                        this.UpdateDataToListBox();
                    }
                    else
                    {
                        MessageBox.Show(this._errMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        private void listBoxFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxFile.Items.Count > 0)
            {
                if (this.listBoxFile.SelectedItem == null)
                {
                    return;
                }

                this._reloadPIVdataSet.Clear();

                string keyName = this.listBoxFile.SelectedItem.ToString();

                this._reloadPIVdataSet.Add(this._dicPIVData[keyName]);

                this._frmPiv.UpdateDataToControl(this._reloadPIVdataSet);

                this._frmPiv.UpdateDataToGraph(this._reloadPIVdataSet);
            }
        }

        #endregion
    }

    internal class DataCollection
    {
        private object _lockObj;

        private string _keyName;
        private string _sourceFileName;
        private int _fileIndex;
        private int _index;

        private int _row;
        private int _col;

        private bool _isVisible;

        private List<double> _lstTimeSpan;
        private List<double> _lstRawVoltage;
        private List<double> _lstRawCurrent;
        private List<double> _lstRawPower;

        private List<double> _lstSmoothVoltage;
        private List<double> _lstSmoothCurrent;
        private List<double> _lstSmoothPower;
        private List<double> _lstSmoothSE;
        private List<double> _lstSmoothRS;

        private string _voltageUnit = "V";
        private string _currentUnit = "A";
        private string _timeUnit = "ms";
        private string _powerUnit = "mW";

        private Dictionary<string, ResultDataSet> _dicResult;

        public DataCollection()
        {
            this._lockObj = new object();

            this._lstRawVoltage = new List<double>();
            this._lstRawCurrent = new List<double>();
            this._lstTimeSpan = new List<double>();
            this._lstRawPower = new List<double>();

            this._lstSmoothVoltage = new List<double>();
            this._lstSmoothCurrent = new List<double>();
            this._lstSmoothPower = new List<double>();
            this._lstSmoothSE = new List<double>();
            this._lstSmoothRS = new List<double>();

            this._dicResult = new Dictionary<string, ResultDataSet>();

            this._row = -1234;
            this._col = -1234;
        }

        #region >>> Public Property <<<

        public string KeyName
        {
            get { return this._keyName; }
            set { lock (this._lockObj) { this._keyName = value; } }
        }

        public string SourceFileName
        {
            get { return this._sourceFileName; }
            set { lock (this._lockObj) { this._sourceFileName = value; } }
        }

        public bool IsVisible
        {
            get { return this._isVisible; }
            set { lock (this._lockObj) { this._isVisible = value; } }
        }

        public double[] TimeSpanArray
        {
            get { return this._lstTimeSpan.ToArray(); }
        }

        public double[] VoltageRawArray
        {
            get { return this._lstRawVoltage.ToArray(); }
        }

        public double[] CurrentRawArray
        {
            get { return this._lstRawCurrent.ToArray(); }
        }

        public double[] PowerRawArray
        {
            get { return this._lstRawPower.ToArray(); }
        }

        public double[] VoltageSmoothArray
        {
            get { return this._lstSmoothVoltage.ToArray(); }
        }

        public double[] CurrentSmoothArray
        {
            get { return this._lstSmoothCurrent.ToArray(); }
        }

        public double[] PowerSmoothArray
        {
            get { return this._lstSmoothPower.ToArray(); }
        }

        public double[] SESmoothArray
        {
            get { return this._lstSmoothSE.ToArray(); }
        }

        public double[] RSSmoothArray
        {
            get { return this._lstSmoothRS.ToArray(); }
        }

        public int Count
        {
            get { return this._lstRawCurrent.Count; }
        }

        public string VoltageUnit
        {
            get { return this._voltageUnit; }
        }

        public string CurrentUnit
        {
            get { return this._currentUnit; }
        }

        public string TimeUnit
        {
            get { return this._timeUnit; }
        }

        public string PowerUnit
        {
            get { return this._powerUnit; }
        }

        public int FileIndex
        {
            get { return this._fileIndex; }
            set { lock (this._lockObj) { this._fileIndex = value; } }
        }

        public int Index
        {
            get { return this._index; }
            set { lock (this._lockObj) { this._index = value; } }
        }

        public int Row
        {
            get { return this._row; }
            set { lock (this._lockObj) { this._row = value; } }
        }

        public int Col
        {
            get { return this._col; }
            set { lock (this._lockObj) { this._col = value; } }
        }

        public Dictionary<string, ResultDataSet> ResultDictionary
        {
            get { return this._dicResult; }
        }

        #endregion

        #region >>> Public Method <<<

        public void Add(double time, double current, double voltage, double power)
        {
            this._lstTimeSpan.Add(time);
            this._lstRawVoltage.Add(voltage);
            this._lstRawCurrent.Add(current);
            this._lstRawPower.Add(power);
        }

        public void AddResult(string resultKeyName, ResultDataSet data)
        {
            this._dicResult.Add(resultKeyName, data.Clone() as ResultDataSet);
        }

        public void AddSmoothCurve(double[] iArray, double[] vArray, double[] pArray, double[] seArray, double[] rsArray)
        {
            this._lstSmoothCurrent.Clear();
            this._lstSmoothVoltage.Clear();
            this._lstSmoothPower.Clear();

            this._lstSmoothSE.Clear();
            this._lstSmoothRS.Clear();

            this._lstSmoothCurrent.AddRange(iArray);
            this._lstSmoothVoltage.AddRange(vArray);
            this._lstSmoothPower.AddRange(pArray);
            this._lstSmoothSE.AddRange(seArray);
            this._lstSmoothRS.AddRange(rsArray);
        }

        #endregion
    }

    internal class ResultDataSet
    {
        private object _lockObj;

        private bool _isVisible;
        private string _keyName;
        private double _value;
        private string _unit;
        private string _format;

        public ResultDataSet()
        {
            this._lockObj = new object();

            this._keyName = string.Empty;
            this._value = 0.0d;
            this._unit = string.Empty;
            this._format = "0.000";

            this._isVisible = true;
        }

        public bool IsVisible
        {
            get { return this._isVisible; }
            set { lock (this._lockObj) { this._isVisible = value; } }
        }

        public string KeyName
        {
            get { return this._keyName; }
            set { lock (this._lockObj) { this._keyName = value; } }
        }

        public double Value
        {
            get { return this._value; }
            set { lock (this._lockObj) { this._value = value; } }
        }

        public string Unit
        {
            get { return this._unit; }
            set { lock (this._lockObj) { this._unit = value; } }
        }

        public string Format
        {
            get { return this._format; }
            set { lock (this._lockObj) { this._format = value; } }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
