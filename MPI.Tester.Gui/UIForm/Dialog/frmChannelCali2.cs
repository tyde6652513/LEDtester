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
using MPI.Tester.Tools;
using ZedGraph;

namespace MPI.Tester.Gui
{
    public partial class frmChannelCali2 : Form
    {
        private Color[] _ChannelColors = new Color[9] { Color.Blue, Color.Red, Color.Orange, Color.GreenYellow, Color.Brown, Color.SeaGreen, Color.DarkOliveGreen, Color.Black, Color.DarkMagenta };

        private ChannelCompCore _fileCore = new ChannelCompCore();

        UIForm.frmBoxPlot frmBoxPlot = null;

        GainOffsetData[] chGainOffsetData = null;

        private string _itemKeyName = string.Empty;

        public frmChannelCali2()
        {
            InitializeComponent();
        }

        #region >>> Private Methods <<<

        private string SelectPath(string title)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = title;
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                return folderBrowserDialog.SelectedPath;
            }
            else
            {
                return string.Empty;
            }

        }

        private void LoadSysGainOffset(string itenName)
        {
            // string keyname = string.Empty;

            _itemKeyName = string.Empty;

            foreach (var testItem in DataCenter._product.TestCondition.TestItemArray)
            {
                if (testItem.MsrtResult == null || testItem.MsrtResult.Length == 0 || !testItem.IsEnable)
                {
                    continue;
                }

                foreach (var msrtItem in testItem.MsrtResult)
                {
                    if (!msrtItem.IsEnable || !msrtItem.IsVision)
                    {
                        continue;
                    }

                    if (msrtItem.Index > 0)
                    {
                        string name = "[" + msrtItem.Index.ToString() + "]" + msrtItem.Name;

                        if (name == itenName)
                        {
                            _itemKeyName = msrtItem.KeyName;
                            break;
                        }

                    }
                }
            }

            chGainOffsetData = new GainOffsetData[DataCenter._product.TestCondition.ChannelConditionTable.Count];

           // 重新建立空間，不要使用Reference Type。否則數值會差錯

            for (int i = 0; i < chGainOffsetData.Length; i++)
            {
                chGainOffsetData[i] = new GainOffsetData();
            }


            if (_itemKeyName == string.Empty)
            {
                for (int i = 0; i < chGainOffsetData.Length; i++)
                {
                    chGainOffsetData[i] = new GainOffsetData();
                }

                UpdateProductChannelGainDgv();

                MessageBox.Show("No Match Item");

                return;

            }


            TestItemData[] testItems = DataCenter._product.TestCondition.TestItemArray;

            int toChannel = -1;

            for (toChannel = 0; toChannel < DataCenter._product.TestCondition.ChannelConditionTable.Count; toChannel++)
            {
                ChannelConditionData condi = DataCenter._product.TestCondition.ChannelConditionTable.Channels[toChannel];

                foreach (TestItemData item in testItems)
                {
                    TestResultData[] data = item.MsrtResult;

                    for (int i = 0; i < data.Length; i++)
                    {
                        if (_itemKeyName == data[i].KeyName)
                        {
                            GainOffsetData gainOffset = condi.GetByChannelGainOffsetData(_itemKeyName);

                            chGainOffsetData[toChannel].Gain = gainOffset.Gain;

                            chGainOffsetData[toChannel].Offset = gainOffset.Offset;

                            break;
                        }
                    }
                }
            }

            UpdateProductChannelGainDgv();
        }

        private void UpdateProductChannelGainDgv()
        {
            int count = 0;

            this.dgvSysChannelFactor.Rows.Clear();

            foreach (var result in chGainOffsetData)
            {
                this.dgvSysChannelFactor.Rows.Add();

                this.dgvSysChannelFactor.Rows[count].Cells[0].Value = count + 1;

                this.dgvSysChannelFactor.Rows[count].Cells[1].Value = result.Gain;

                this.dgvSysChannelFactor.Rows[count].Cells[2].Value = result.Offset;

                count++;
            }
        }

        private void DgvCHInfoRefresh()
        {
            int count = 0;

            this.dgvCHInfo.Rows.Clear();

            foreach (var result in this._fileCore.CalcChannelGainOffset)
            {
                this.dgvCHInfo.Rows.Add();

                this.dgvCHInfo.Rows[count].Cells[0].Value = count+1;

                this.dgvCHInfo.Rows[count].Cells[1].Value = this._fileCore.CalcChannelGainOffset[count].Gain;

                this.dgvCHInfo.Rows[count].Cells[2].Value = this._fileCore.CalcChannelGainOffset[count].Offset;

                count++;
            }
        }

        private void UpdateFilterDgv()
        {
            this.dgvFilterSpec.Rows.Clear();

            int index = 0;

            foreach (var var in this._fileCore.DicFilterData)
            {
                dgvFilterSpec.Rows.Add();

                dgvFilterSpec.Rows[index].Cells[0].Value = var.Value._filterData.IsEnable;

                dgvFilterSpec.Rows[index].Cells[1].Value = var.Key;

                dgvFilterSpec.Rows[index].Cells[2].Value = var.Value._filterData.Min;

                dgvFilterSpec.Rows[index].Cells[3].Value = var.Value._filterData.Max;

                dgvFilterSpec.Rows[index].Cells[4].Value = (int)var.Value._gainData.CalcType;

                index++;
            }
        }

        private void UpdateSysChannelFator()
        {
            int count = 0;

            this.dgvSysChannelFactor.Rows.Clear();

            foreach (var result in chGainOffsetData)
            {
                this.dgvSysChannelFactor.Rows.Add();

                this.dgvSysChannelFactor.Rows[count].Cells[0].Value = count + 1;

                this.dgvSysChannelFactor.Rows[count].Cells[1].Value = result.Gain;

                this.dgvSysChannelFactor.Rows[count].Cells[2].Value = result.Offset;

                this.dgvSysChannelFactor.Rows[count].Cells[1].Style.BackColor = Color.Yellow;

                this.dgvSysChannelFactor.Rows[count].Cells[2].Style.BackColor = Color.Yellow;

                count++;
            }
        }

        #endregion

        #region >>> UI Event Handler <<<

        private void frmChannelCali_Load(object sender, EventArgs e)
        {
            this.txtStdPathAndFile.Text = "";

            this.txtMsrtPathAndFile.Text = "";

            this.TopMost = false;
        }

        private void btnLoadStd_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Title = "Import CoefTable Data from File";

         //   openFileDialog.Filter = "CSV files (*.csv)|*.csv";

            openFileDialog.FilterIndex = 1;    // default value = 1

          //  openFileDialog.InitialDirectory = DataCenter._toolConfig.StdChannelFileDir;

            openFileDialog.FileName = "";

            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.txtStdPathAndFile.Text = openFileDialog.FileName;

                this._fileCore.LoadStdFile(openFileDialog.FileName);

               // EErrorCode errCode = this._rcCtrl.LoadStdFromFile(openFileDialog.FileName, 0);

                listTitle.Items.Clear();

                listTitle.Items.AddRange(_fileCore.StdTitleData);

                if (_fileCore.StdTitleData==null)
                {
                    this.txtStdPathAndFile.Text = "";

                    MessageBox.Show("UserData & Std Title is Not Match", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLoadMsrt_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Title = "Import CoefTable Data from File";

          //  openFileDialog.Filter = "CSV files (*.csv)|*.csv";

            openFileDialog.FilterIndex = 1;    // default value = 1

          //  openFileDialog.InitialDirectory = DataCenter._toolConfig.MsrtChannelFileDir;

            openFileDialog.FileName = "";

            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.txtMsrtPathAndFile.Text = openFileDialog.FileName;

                this._fileCore.LoadMsrtFile(openFileDialog.FileName);
            }
            
            if(this._fileCore.StdTitleData.Length!=this._fileCore.MsrtTitleData.Length)
            {
                MessageBox.Show("Test file title not match the standard file title");
                return;
            }

            UpdateFilterDgv();
        }

        private void btnCalcuate_Click(object sender, EventArgs e)
        {
            _fileCore.CompareData();

            cmbItemSelect.Items.Clear();

            cmbItemSelect.Items.AddRange(_fileCore.MsrtItemNameArray);

            this.txtSameRowColCount.Text = this._fileCore.DicCompareData.Count.ToString();

            this.UpdateFilterDgv();
        }

        private void btnCalcChannelGain_Click(object sender, EventArgs e)
        {
            if (this.cmbItemSelect.SelectedIndex < 0)
            {
                return;
            }

            string itemHeader = this.cmbItemSelect.SelectedItem.ToString();

            int proudctChannel = DataCenter._product.TestCondition.ChannelConditionTable.Count;

            if (this._fileCore.DicFilterData.ContainsKey(itemHeader))
            {
                _fileCore.SortingByChannel(proudctChannel, this._fileCore.DicFilterData[itemHeader], "1");
            }

            //_fileCore.SortingByChannel(proudctChannel, itemHeader, this.numNormalizeCH.Value.ToString(),);

            this.DgvCHInfoRefresh();

            if (this._fileCore.CalcChannelGainOffset == null)
            {
                return;
            }

            if (this._fileCore.CalcChannelGainOffset.Length == 0)
            {
                return;
            }

            if (frmBoxPlot != null)
            {
                frmBoxPlot.Close();

                frmBoxPlot.Dispose();
            }

            frmBoxPlot = new UIForm.frmBoxPlot();
        
            frmBoxPlot.TopLevel = false;

            frmBoxPlot.Parent = this.plShowBoxPlot;

            frmBoxPlot.Dock = DockStyle.Fill;

            frmBoxPlot.FormBorderStyle = FormBorderStyle.None;

            List<string> name = new List<string>();

            List<double[]> data = new List<double[]>();

            foreach (CalcGainOffset cgo in this._fileCore.CalcChannelGainOffset)
            {
                name.Add(cgo.Name + "-Std");

                name.Add(cgo.Name + "-Msrt");

                data.Add(cgo.Yout);

                data.Add(cgo.Xin);
            }


            frmBoxPlot._Names = name;

            frmBoxPlot._Data = data;

            frmBoxPlot.Show();

        }

        private void btnSetFilter_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dgvFilterSpec.RowCount; i++)
            {
                if ((bool)this.dgvFilterSpec.Rows[i].Cells[0].Value == true)
                {
                    bool isEanble = (bool)this.dgvFilterSpec.Rows[i].Cells[0].Value;

                    string name = CsvQuoteMark.Unescape(this.dgvFilterSpec.Rows[i].Cells[1].Value.ToString());

                    string minStr = this.dgvFilterSpec.Rows[i].Cells[2].Value.ToString();

                    string maxStr = this.dgvFilterSpec.Rows[i].Cells[3].Value.ToString();

                    double min = double.Parse(minStr);

                    double max = double.Parse(maxStr);

                    if (_fileCore.DicFilterData.ContainsKey(name))
                    {
                        _fileCore.DicFilterData[name]._filterData.IsEnable = isEanble;

                        _fileCore.DicFilterData[name]._filterData.Min = min;

                        _fileCore.DicFilterData[name]._filterData.Max = max;
                    }
                }
            }

            this._fileCore.Filter();

            this.txtFilteredCount.Text = this._fileCore.DicCompareData.Count.ToString();
        }

        private void cmbItemSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbItemSelect.SelectedIndex >= 0)
            {
                LoadSysGainOffset(cmbItemSelect.SelectedItem.ToString());
            }
        }

        private void btnCombine_Click(object sender, EventArgs e)
        {
            for (int channel = 0; channel < chGainOffsetData.Length; channel++)
            {
                chGainOffsetData[channel].Gain = chGainOffsetData[channel].Gain * this._fileCore.CalcChannelGainOffset[channel].Gain;

                this._fileCore.CalcChannelGainOffset[channel].Gain = 1.0d;

                chGainOffsetData[channel].Offset = chGainOffsetData[channel].Offset * this._fileCore.CalcChannelGainOffset[channel].Offset;

                this._fileCore.CalcChannelGainOffset[channel].Offset= 0.0d;
            }

            UpdateSysChannelFator();

            DgvCHInfoRefresh();
        }

        private void btnSaveToProduct_Click(object sender, EventArgs e)
        {
            if (_itemKeyName == string.Empty)
            {
                return;
            }

            TestItemData[] testItems = DataCenter._product.TestCondition.TestItemArray;

            int toChannel = -1;

            for (toChannel = 0; toChannel < DataCenter._product.TestCondition.ChannelConditionTable.Count; toChannel++)
            {
                 ChannelConditionTable saveTable = DataCenter._product.TestCondition.ChannelConditionTable;

                foreach (TestItemData item in testItems)
                {
                    TestResultData[] data = item.MsrtResult;

                    for (int i = 0; i < data.Length; i++)
                    {
                        if (_itemKeyName == data[i].KeyName)
                        {
                            GainOffsetData coef = saveTable.Channels[toChannel].GetByChannelGainOffsetData(_itemKeyName);

                            coef.Gain = this.chGainOffsetData[toChannel].Gain;

                            coef.Offset = this.chGainOffsetData[toChannel].Offset;

                            break;
                        }
                    }
                }
            }

          // DataCenter._conditionCtrl.CopyTestItemArrayToEachChannel();

            DataCenter.SaveProductFile();

            Host.UpdateDataToAllUIForm();

        }

        #endregion

        private void frmChannelCali_Closed(object sender, FormClosedEventArgs e)
        {
          //  this.Dispose();
        }



    }
}
