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
    public partial class frmViewWLGraph : Form
    {
        #region >>> Private Properties <<<

        frmSpectrumGraphComponent _frmSpectrum;

        private Dictionary<string, SpectrumDataCollection> _dicSpectrumDataCollection;
        private Dictionary<string, SpectrumData> _dicSpectrumData;

        private SpectrumDataSet _reLoadSpectrumDataSet;

        private string _errMsg;

        #endregion

        public frmViewWLGraph()
        {
            InitializeComponent();

            this._frmSpectrum = new frmSpectrumGraphComponent(ETestType.LOPWL, DataCenter._uiSetting.UserID.ToString());
            this._frmSpectrum.IsAbsoluteSpectrumVisible = false;
            this._frmSpectrum.IsSaveScaleVisible = false;

            this._dicSpectrumDataCollection = new Dictionary<string, SpectrumDataCollection>();
            this._dicSpectrumData = new Dictionary<string, SpectrumData>();
            this._reLoadSpectrumDataSet = new SpectrumDataSet();

            this.AttachFormToPanel(this._frmSpectrum, this.pnlWLGraph);
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

                    this._dicSpectrumDataCollection.Clear();

                    char[] splitCharArray = new char[] { '#', 'C', 'R', '_' };

                    foreach (string fileNameAndPath in lstFileNameAndPath)
                    {
                        fileName = Path.GetFileNameWithoutExtension(fileNameAndPath);

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

                        fileName = Path.GetFileNameWithoutExtension(fileNameAndPath);

                        filePath = Path.GetDirectoryName(fileNameAndPath);


                        this.lblSourceFilePath.Text = string.Format("Source File Path: {0}", filePath);
                        this.lblSourceFileName.Text = string.Format("Source File Name: {0}", fileName);

                        //------------------------------------------------------------------------------
                        // File數據 加入到 SpectrumDataCollection
                        //------------------------------------------------------------------------------
                        for (int row = 1; row < lstArray.Count; row++)
                        {
                            string[] tempdata = new string[] { };

                            SpectrumDataCollection data = new SpectrumDataCollection();

                            string keyName = string.Format("#{0}{1}_{2}", fileIndex, lstArray[row][0], lstArray[row][1]);

                            if (!this._dicSpectrumDataCollection.ContainsKey(keyName))
                            {
                                _dicSpectrumDataCollection.Add(keyName, data);
                            }

                            _dicSpectrumDataCollection[keyName].SourceFileName = fileNameAndPath;

                            string[] keyInfo = keyName.Split(splitCharArray);
                            _dicSpectrumDataCollection[keyName].Col = int.Parse(keyInfo[1]);
                            _dicSpectrumDataCollection[keyName].Row = int.Parse(keyInfo[2]);
                            _dicSpectrumDataCollection[keyName].WLName = keyInfo[3];

                            for (int col = 3; col < lstArray[0].Length; col++)
                            {
                                double tempwavelength = double.Parse(lstArray[0][col]);

                                double tempintensity = double.Parse(lstArray[row][col]);

                                _dicSpectrumDataCollection[keyName].Add(tempwavelength, tempintensity);
                            }
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

            if (this._dicSpectrumDataCollection.Count > 0)
            {
                foreach (var kvp in this._dicSpectrumDataCollection)
                {
                    this.listBoxFile.Items.Add(kvp.Key);
                }
            }
        }

        private void SpectrumDataCollectionToSpectrumData()
        {
            foreach (string Data_key in _dicSpectrumDataCollection.Keys)
            {
                SpectrumData data = new SpectrumData();
                data.Wavelength = _dicSpectrumDataCollection[Data_key].ListWavelength.ToArray();
                data.Intensity = _dicSpectrumDataCollection[Data_key].ListRawIntensity.ToArray();
                data.KeyName = Data_key;
                _dicSpectrumData.Add(Data_key, data);
            }
        }

        #endregion

        #region >>> Private Methods <<<

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
            dialog.Title = "Import Spectrum Data";
            dialog.Filter = "REL files (*.rel)|*.rel";
            dialog.FileName = "";
            dialog.InitialDirectory = DataCenter._uiSetting.RelativeSpectrumPath;
            dialog.FilterIndex = 1;    // default value = 1
            dialog.Multiselect = false;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this._dicSpectrumData.Clear();

                if (dialog.FileNames.Length > 0)
                {
                    List<string> tempFileNameAndPath = new List<string>();

                    tempFileNameAndPath.AddRange(dialog.FileNames);

                    if (this.DataParser(tempFileNameAndPath))
                    {
                        this.SpectrumDataCollectionToSpectrumData();
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

                this._reLoadSpectrumDataSet.Clear();
                string keyName = this.listBoxFile.SelectedItem.ToString();
                this._reLoadSpectrumDataSet.Add(this._dicSpectrumData[keyName]);
                this._frmSpectrum.UpdateDataToControl(this._reLoadSpectrumDataSet);
                this._frmSpectrum.UpdateDataToGraph(this._reLoadSpectrumDataSet);
            }
        }

        #endregion

    }

    internal class SpectrumDataCollection
    {
        private object _lockObj;

        private string _sourceFileName;
        private string _wlName;

        private int _row;
        private int _col;
              
        private List<double> _lstWavelength;
        private List<double> _lstRawIntensity;

        public SpectrumDataCollection()
        {
            this._lockObj = new object();

            this._sourceFileName = string.Empty;
            this.WLName = string.Empty;

            this._row = 999;
            this._col = 999;

            this._lstWavelength = new List<double>();
            this._lstRawIntensity = new List<double>();
        }

        #region >>> Public Property <<<

        public string SourceFileName
        {
            get { return this._sourceFileName; }
            set { lock (this._lockObj) { this._sourceFileName = value; } }
        }

        public string WLName
        {
            get { return this._wlName; }
            set { lock (this._lockObj) { this._wlName = value; } }
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

        public List<double> ListWavelength
        {
            get { return this._lstWavelength; }
            set { lock (this._lockObj) { this._lstWavelength = value; } }
        }

        public List<double> ListRawIntensity
        {
            get { return this._lstRawIntensity; }
            set { lock (this._lockObj) { this._lstRawIntensity = value; } }
        }

        #endregion

        #region >>> Public Methods <<<

        public void Add(double wavelength, double intensity)
        {
            this._lstWavelength.Add(wavelength);
            this._lstRawIntensity.Add(intensity);
        }

        #endregion
    }
}
