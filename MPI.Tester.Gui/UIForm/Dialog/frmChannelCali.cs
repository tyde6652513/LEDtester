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
    public partial class frmChannelCali : Form
    {
        private ByChannelCalibrateCtrl _rcCtrl = new ByChannelCalibrateCtrl();

        private Color[] _ChannelColors = new Color[9] { Color.Blue, Color.Red, Color.Orange, Color.GreenYellow, Color.Brown, Color.SeaGreen, Color.DarkOliveGreen, Color.Black, Color.DarkMagenta };

        public frmChannelCali()
        {
            InitializeComponent();

            this.InitParamAndCompData();
        }

        #region >>> Private Methods <<<

        private void InitParamAndCompData()
        {
            this.cmbDisplayWave.Items.Clear();
            this.cmbDisplayWave.Items.AddRange(new string[4] { "WLP", "WLD", "WLC", "HW" });
            this.cmbDisplayLOP.Items.Clear();
            this.cmbDisplayLOP.Items.AddRange(new string[3] { "LOP", "WATT", "LM" });

            this.cmbDisplayWave.SelectedIndex = 0;

            this.cmbDisplayLOP.SelectedIndex = 0;
        }

        private void InitWaveAndLopChartLabel()
        {
            PlotGraph.Clear(this.zedWaveLength);
            PlotGraph.Clear(this.zedMcdGainValue);

            string LOPItem = this.cmbDisplayLOP.SelectedItem.ToString();
            string waveError = this.cmbDisplayWave.SelectedItem.ToString();
            string waveBase = DataCenter._product.TestCondition.CalByWave.ToString();

            //Initial Grapha
            PlotGraph.SetGrid(this.zedMcdGainValue, false, Color.Silver, Color.Transparent);
            PlotGraph.SetGrid(this.zedWaveLength, false, Color.Silver, Color.Transparent);
            PlotGraph.SetLabel(zedMcdGainValue, LOPItem, waveBase + " (nm)", LOPItem + "  Gain", 16);
            PlotGraph.SetLabel(zedWaveLength, waveError + " (nm)", waveBase + " (nm)", waveError + "  Error (nm)", 16);
        }

        private void InitVoltageChartLabel()
        {
            string name = string.Empty;

            PlotGraph.Clear(this.zedDisplayVolt1);
            PlotGraph.Clear(this.zedDisplayVolt2);
            PlotGraph.Clear(this.zedDisplayVolt3);
            PlotGraph.Clear(this.zedDisplayVolt4);

            //Initial Grapha
            PlotGraph.SetGrid(this.zedDisplayVolt1, false, Color.Silver, Color.Transparent);
            PlotGraph.SetGrid(this.zedDisplayVolt2, false, Color.Silver, Color.Transparent);
            PlotGraph.SetGrid(this.zedDisplayVolt3, false, Color.Silver, Color.Transparent);
            PlotGraph.SetGrid(this.zedDisplayVolt4, false, Color.Silver, Color.Transparent);

            if (this._rcCtrl.TitleName.ContainsKey("MVF_1"))
            {
                name = this._rcCtrl.TitleName["MVF_1"];
                PlotGraph.SetLabel(this.zedDisplayVolt1, name, "", "V", 16);
            }

            if (this._rcCtrl.TitleName.ContainsKey("MVF_2"))
            {
                name = this._rcCtrl.TitleName["MVF_2"];
                PlotGraph.SetLabel(this.zedDisplayVolt2, name, "", "V", 16);
            }

            if (this._rcCtrl.TitleName.ContainsKey("MVF_3"))
            {
                name = this._rcCtrl.TitleName["MVF_3"];
                PlotGraph.SetLabel(this.zedDisplayVolt3, name, "", "V", 16);
            }

            if (this._rcCtrl.TitleName.ContainsKey("MVF_4"))
            {
                name = this._rcCtrl.TitleName["MVF_4"];
                PlotGraph.SetLabel(this.zedDisplayVolt4, name, "", "V", 16);
            }
        }

        private void UpateSettingDataToUI()
        {
            string gainKeyName = string.Empty;

            this.dgvFilter.Rows.Clear();

            if (DataCenter._product.TestCondition == null || DataCenter._product.TestCondition.TestItemArray == null)
            {
                this.dgvFilter.Update();

                return;
            }

            TestItemData[] testItems = DataCenter._product.TestCondition.TestItemArray;

            //--------------------------------------------------------------
            // Update Gain-Offset Table data
            //--------------------------------------------------------------
            int itemIndex = 0;

            int gainIndex = 0;

            string[] gainRowData = new string[this.dgvFilter.ColumnCount];

            this.dgvFilter.AllowUserToAddRows = true;

            foreach (TestItemData item in testItems)
            {
                if (item.GainOffsetSetting != null)
                {
                    for (int i = 0; i < item.GainOffsetSetting.Length; i++)
                    {
                        if (item.GainOffsetSetting[i].IsEnable == false || item.GainOffsetSetting[i].IsVision == false)
                            continue;

                        gainRowData[2] = item.GainOffsetSetting[i].Name;

                        if ((item is LOPWLTestItem) && item.GainOffsetSetting[i].KeyName.IndexOf("_") > 0)
                        {
                            gainKeyName = item.GainOffsetSetting[i].KeyName.Remove(item.GainOffsetSetting[i].KeyName.IndexOf("_"));

                            switch (gainKeyName)
                            {
                                case "LOP":        // EOptiMsrtType.LOP :
                                    gainRowData[2] = item.GainOffsetSetting[i].Name + "(mcd)";
                                    break;
                                //------------------------------------------------
                                case "WATT":       // EOptiMsrtType.WATT
                                    gainRowData[2] = item.GainOffsetSetting[i].Name + "(mW)";
                                    break;
                                //------------------------------------------------
                                case "LM":         // EOptiMsrtType.LM
                                    gainRowData[2] = item.GainOffsetSetting[i].Name + "(lm)";
                                    break;
                                //------------------------------------------------
                                default:
                                    gainRowData[2] = item.GainOffsetSetting[i].Name;
                                    break;
                            }
                        }
                        else
                        {
                            gainRowData[2] = item.GainOffsetSetting[i].Name;
                        }

                        if (this._rcCtrl.FilterDic.ContainsKey(item.GainOffsetSetting[i].KeyName)) // Load Recipe
                        {
                            FilterData fd = this._rcCtrl.FilterDic[item.GainOffsetSetting[i].KeyName];
                            gainRowData[1] = fd.KeyName;
                            gainRowData[3] = fd.IsEnable.ToString();
                            gainRowData[4] = fd.Min.ToString();
                            gainRowData[5] = fd.Max.ToString();
                            gainRowData[6] = ((int)item.GainOffsetSetting[i].Type).ToString();
                            gainRowData[7] = item.MsrtResult[i].Formate;
                        }

                        this.dgvFilter.Rows.Add(gainRowData);

                        gainIndex++;
                    }
                }
                itemIndex++;
            }

            this.dgvFilter.AllowUserToAddRows = false;

            this.dgvFilter.Update();

            this.dgvFilter.Refresh();
        }

        private void UpdateDataToSysCoefDgv()
        {
            //--------------------------------------------------------------------------------------------
            // Update System Gain Offset DGV
            //--------------------------------------------------------------------------------------------
            ChannelConditionTable conditionTable = DataCenter._product.TestCondition.ChannelConditionTable;

            TestItemData[] testItems = DataCenter._product.TestCondition.TestItemArray;

            if (conditionTable != null && testItems != null)
            {
                this.dgvSysCoef.SuspendLayout();

                this.dgvSysCoef.Rows.Clear();

                int index = 0;

                string gainKeyName = string.Empty;

                int rowCount = 0;

                if (conditionTable.Channels.Length != 0)
                {
                    foreach (TestItemData item in testItems)
                    {
                        if (item.GainOffsetSetting == null || item.GainOffsetSetting.Length == 0)
                            continue;

                        foreach (GainOffsetData data in item.GainOffsetSetting)
                        {
                            if (!data.IsEnable || !data.IsVision)
                                continue;

                            for (uint channel = 0; channel < conditionTable.Count; channel++)
                            {
                                GainOffsetData coef = conditionTable.Channels[channel].GetByChannelGainOffsetData(data.KeyName);

                                this.dgvSysCoef.Rows.Add();

                                this.dgvSysCoef.Rows[rowCount].Cells[0].Value = (rowCount + 1).ToString();

                                if ((item is LOPWLTestItem) && data.KeyName.IndexOf("_") > 0)
                                {
                                    gainKeyName = data.KeyName.Remove(data.KeyName.IndexOf("_"));

                                    switch (gainKeyName)
                                    {
                                        case "LOP":        // EOptiMsrtType.LOP :
                                            this.dgvSysCoef.Rows[rowCount].Cells[1].Value = coef.Name + " (mcd)";
                                            break;
                                        //------------------------------------------------
                                        case "WATT":       // EOptiMsrtType.WATT
                                            this.dgvSysCoef.Rows[rowCount].Cells[1].Value = coef.Name + " (mW)";
                                            break;
                                        //------------------------------------------------
                                        case "LM":         // EOptiMsrtType.LM
                                            this.dgvSysCoef.Rows[rowCount].Cells[1].Value = coef.Name + " (lm)";
                                            break;
                                        //------------------------------------------------
                                        default:
                                            this.dgvSysCoef.Rows[rowCount].Cells[1].Value = coef.Name;
                                            break;
                                    }
                                }
                                else
                                {
                                    this.dgvSysCoef.Rows[rowCount].Cells[1].Value = coef.Name;
                                }

                                this.dgvSysCoef.Rows[rowCount].Cells[2].Value = (channel + 1).ToString();
                                this.dgvSysCoef.Rows[rowCount].Cells[3].Value = coef.Type;
                                this.dgvSysCoef.Rows[rowCount].Cells[4].Value = coef.Square;
                                this.dgvSysCoef.Rows[rowCount].Cells[5].Value = coef.Gain;
                                this.dgvSysCoef.Rows[rowCount].Cells[6].Value = coef.Offset;
                                this.dgvSysCoef.Rows[rowCount].Cells[7].Value = coef.KeyName;

                                if ((index % 2) != 0)
                                {
                                    this.dgvSysCoef.Rows[rowCount].DefaultCellStyle.BackColor = Color.AliceBlue;
                                }

                                rowCount++;
                            }

                            index++;
                        }
                    }
                }

                this.dgvSysCoef.ResumeLayout();
            }
        }

        private void UpdateDataToCalcCoefDgv()
        {
            //--------------------------------------------------------------------------------------------
            // Update Calc Coef DGV
            //--------------------------------------------------------------------------------------------
            if (this._rcCtrl.DicCalcGainOffset != null)
            {
                this.dgvCalcCoef.SuspendLayout();

                this.dgvCalcCoef.Rows.Clear();

                int index = 0;

                if (this._rcCtrl.DicCalcGainOffset.Count != 0)
                {
                    foreach (var item in this._rcCtrl.DicCalcGainOffset)
                    {
                        for (int i = 0; i < item.Value.Length; i++)
                        {
                            this.dgvCalcCoef.Rows.Add();

                            this.dgvCalcCoef.Rows[index].Cells[0].Value = (index + 1).ToString();  // No

                            this.dgvCalcCoef.Rows[index].Cells[1].Value = item.Value[i].Name;      // Name

                            this.dgvCalcCoef.Rows[index].Cells[2].Value = (i + 1).ToString();      // Channel

                            this.dgvCalcCoef.Rows[index].Cells[3].Value = item.Value[i].CalcType;  // Type

                            this.dgvCalcCoef.Rows[index].Cells[4].Value = item.Value[i].Sqaure;    // Square

                            this.dgvCalcCoef.Rows[index].Cells[5].Value = item.Value[i].Gain;      // Gain

                            this.dgvCalcCoef.Rows[index].Cells[6].Value = item.Value[i].Offset;    // Offset

                            string[] keyName = item.Value[i].KeyName.Split('@');

                            this.dgvCalcCoef.Rows[index].Cells[7].Value = keyName[0];              // KeyName

                            index++;
                        }
                    }
                }

                this.dgvCalcCoef.ResumeLayout();
            }
        }

        private void UpdateWaveAndLopChart()
        {
            if (this.cmbDisplayTable.SelectedIndex < 0)
            {
                return;
            }

            if (this.cmbDisplayWave.SelectedIndex < 0 || this.cmbDisplayLOP.SelectedIndex < 0)
            {
                return;
            }

            int index = this.cmbDisplayTable.SelectedIndex + 1;

            string keyNameWave = string.Format("{0}_{1}", this.cmbDisplayWave.SelectedItem.ToString(), index);

            string keyNameLop = string.Format("{0}_{1}", this.cmbDisplayLOP.SelectedItem.ToString(), index);

            this.InitWaveAndLopChartLabel();

            this.PlotCompareDataToChart(keyNameWave, this.zedWaveLength);

            this.PlotCompareDataToChart(keyNameLop, this.zedMcdGainValue);

        }

        private void UpdateVoltageChart()
        {
            this.InitVoltageChartLabel();

            this.PlotCompareDataToChart("MVF_1", this.zedDisplayVolt1);

            this.PlotCompareDataToChart("MVF_2", this.zedDisplayVolt2);

            this.PlotCompareDataToChart("MVF_3", this.zedDisplayVolt3);

            this.PlotCompareDataToChart("MVF_4", this.zedDisplayVolt4);
        }

        private void SaveGainOffsetSetting()
        {
            TestItemData[] testItems = DataCenter._product.TestCondition.TestItemArray;

            int row = 0;

            for (row = 0; row < this.dgvFilter.Rows.Count; row++)
            {
                foreach (TestItemData item in testItems)
                {
                    if (item.GainOffsetSetting != null)
                    {
                        GainOffsetData[] data = item.GainOffsetSetting;

                        for (int i = 0; i < data.Length; i++)
                        {
                            if (this.dgvFilter[1, row].Value == null)
                                continue;

                            if (this.dgvFilter[1, row].Value.ToString() == data[i].KeyName)
                            {
                                data[i].Type = (EGainOffsetType)Enum.Parse(typeof(EGainOffsetType), this.dgvFilter[6, row].Value.ToString(), true);
                            }
                        }
                    }
                }
            }
        }

        private void SaveFilterData()
        {
            //-------------------------------------------------------------
            // Save UI Filter Setting To System
            //-------------------------------------------------------------
            if (this.dgvFilter == null)
            {
                return;
            }
            for (int idx = 0; idx < this.dgvFilter.Rows.Count; idx++)
            {
                if (this.dgvFilter[1, idx].Value == null)
                {
                    continue;
                }

                string keyName = this.dgvFilter[1, idx].Value.ToString();

                if (this._rcCtrl.FilterDic.ContainsKey(keyName))
                {
                    FilterData filterData = this._rcCtrl.FilterDic[keyName];
                    filterData.IsEnable = bool.Parse(this.dgvFilter[3, idx].Value.ToString());
                    filterData.Max = double.Parse(this.dgvFilter[5, idx].Value.ToString());
                    filterData.Min = double.Parse(this.dgvFilter[4, idx].Value.ToString());
                }
            }

            //------------------------------------------------------------------------------
            // Save Filter Setting To File when tool setting  isn't use recipe Criterion
            //------------------------------------------------------------------------------
            if (!DataCenter._toolConfig.IsEnableUseRecipeCriterion)
            {
                string pathAndFileName = Path.Combine(Constants.Paths.TOOLS_DIR, "FilterCriteriaRC.xml");

                this._rcCtrl.SaveFilterDataToFile(pathAndFileName);
            }
        }

        private void PlotCompareDataToChart(string keyName, ZedGraphControl zedCtrl)
        {
            if (DataCenter._machineConfig.ChannelConfig.ChannelCount < 1 || DataCenter._machineConfig.ChannelConfig.ChannelCount > 9)
            {
                return;
            }

            for (int channel = 0; channel < DataCenter._machineConfig.ChannelConfig.ChannelCount; channel++)
            {
                string dicKey = string.Format("{0}@{1}", keyName, channel.ToString());

                if (this._rcCtrl.DicCalcGainOffset.ContainsKey(keyName))
                {
                    double[] y = this._rcCtrl.DicCompareData[dicKey];

                    double[] x = new double[y.Length];

                    double[] baseline = new double[y.Length];

                    double[] displayData = new double[y.Length];

                    double gain = this._rcCtrl.DicCalcGainOffset[keyName][channel].Gain;

                    double offset = this._rcCtrl.DicCalcGainOffset[keyName][channel].Offset;

                    for (int i = 0; i < y.Length; i++)
                    {
                        x[i] = i + 1;

                        baseline[i] = 0;

                        displayData[i] = y[i];
                    }

                    //if (key.Contains("WATT") || key.Contains("LM") || key.Contains("LOP"))
                    //{
                    //    displayData[i] = (y[i] / gain) - 1;
                    //}
                    //else
                    //{
                    //    displayData[i] = y[i] - offset;
                    //}

                    SymbolType pointType = SymbolType.Diamond;

                    PlotGraph.DrawPlot(zedCtrl, x, displayData, false, 1.5F, this._ChannelColors[channel], pointType, true, true, true, "CH-" + (channel + 1).ToString());

                    // PlotGraph.DrawPlot(zedWaveLength, this._calcCoefArray[tableIndex].TableBaseWave,
                    //                              this._calcCoefArray[tableIndex].CompareData[WLSlectedIndex],
                    //                             false, 2.0F, color, pointType, isFillPoint, true, false, "None");
                    //

                }
            }
        }

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

        private void UpdateDataToControls()
        {
            this.txtOpenStdFilePath.Text = DataCenter._toolConfig.StdChannelFileDir;
            this.txtOpenMsrtFilePath.Text = DataCenter._toolConfig.MsrtChannelFileDir;
        }

        private void CombineGainOffset()
        {
            if (this.dgvCalcCoef.CurrentCell == null)
                return;

            string fromKeyName = string.Empty;
            string toKeyName = string.Empty;

            int fromChannel = -1;
            int toChannel = -1;

            int gainDigit = 9;
            int offsetDigit = 9;

            List<CalcGainOffset> tempCoefCalcGainOffsetArray = new List<CalcGainOffset>();

            CalcGainOffset[] coefCalcGainOffsetArray = null;

            foreach (var pair in this._rcCtrl.DicCalcGainOffset)
            {
                tempCoefCalcGainOffsetArray.AddRange(pair.Value);
            }

            coefCalcGainOffsetArray = tempCoefCalcGainOffsetArray.ToArray();

            for (int i = 0; i < this.dgvCalcCoef.Rows.Count; i++)
            {
                fromKeyName = this.dgvCalcCoef.Rows[i].Cells[7].Value.ToString();

                fromChannel = Convert.ToInt32(this.dgvCalcCoef[2, i].Value);

                //for (int k = 0; k < this._rcCtrl.DicCalcGainOffset.Count; k++)
                //{
                //    if (this._rcCtrl.DicCalcGainOffset[].KeyName == fromKeyName)
                //    {
                //        offsetDigit = this._calcGainOffsetArray[k].Digit + this._calcGainOffsetArray[k].ExtOffsetDigit;

                //        gainDigit = this._calcGainOffsetArray[k].Digit + this._calcGainOffsetArray[k].ExtGainDigit;
                //    }
                //}

                //for (int k = 0; k < coefCalcGainOffsetArray.Length; k++)
                //{
                //    if (coefCalcGainOffsetArray[k].KeyName == fromKeyName)
                //    {
                //        offsetDigit = coefCalcGainOffsetArray[k].Digit + coefCalcGainOffsetArray[k].ExtOffsetDigit;

                //        gainDigit = coefCalcGainOffsetArray[k].Digit + coefCalcGainOffsetArray[k].ExtGainDigit;
                //    }
                //}

                //if (gainDigit == -1 || offsetDigit == -1)
                //    return;

                for (int j = 0; j < this.dgvSysCoef.Rows.Count; j++)
                {
                    if (Convert.ToInt32(this.dgvSysCoef[3, j].Value) == 0)   // Type = 0, None
                        continue;

                    toKeyName = this.dgvSysCoef.Rows[j].Cells[7].Value.ToString();

                    toChannel = Convert.ToInt32(this.dgvSysCoef[2, j].Value);

                    if (fromKeyName == toKeyName && fromChannel == toChannel)
                    {
                        // 重大Bug修正 coefficent combine 會出錯
                        this.CombineDGVData(this.dgvCalcCoef.Rows[i], this.dgvSysCoef.Rows[j], offsetDigit, gainDigit);
                    }
                }
            }
        }

        private void CombineDGVData(DataGridViewRow from, DataGridViewRow target, int offsetDigit, int gainDigit)
        {

            //====================================================================
            //	y = A * x + B , z = C * y + D , z = (A * C)x + C * B + D
            //  z = Slop * x + Intercept , Slop = A * C , Intercept = C * B + D
            //====================================================================

            double A = 0.0d;
            double B = 0.0d;
            double C = 0.0d;
            double D = 0.0d;

            A = Convert.ToDouble(target.Cells[5].Value);

            B = Convert.ToDouble(target.Cells[6].Value);

            C = Convert.ToDouble(from.Cells[5].Value);

            D = Convert.ToDouble(from.Cells[6].Value);

            target.Cells[5].Value = Math.Round(A * C, gainDigit, MidpointRounding.AwayFromZero);			// Slop , Gain

            target.Cells[6].Value = Math.Round(C * B + D, offsetDigit, MidpointRounding.AwayFromZero); ;	// Intercept , Offset

            target.Cells[5].Style.BackColor = Color.Orange;

            target.Cells[6].Style.BackColor = Color.Orange;

            from.Cells[5].Value = 1.0;

            from.Cells[6].Value = 0.0;

            from.Cells[5].Style.BackColor = Color.Orange;

            from.Cells[6].Style.BackColor = Color.Orange;
        }

        #endregion

        #region >>> UI Event Handler <<<

        private void frmChannelCali_Load(object sender, EventArgs e)
        {
            //------------------------------------------
            // Load C:\MPI\LEDTester current format
            //------------------------------------------
            string fileNameWithExt = string.Format("{0}{1}", "User", ((int)DataCenter._uiSetting.UserID).ToString("0000")) + ".xml";

            string pathAndFileName = Path.Combine(Constants.Paths.USER_DIR, fileNameWithExt);

            //-----------------------------------------------------
            // (1) Set parameters of current product from UI setting
            //-----------------------------------------------------
            this._rcCtrl.LOPSaveItem = DataCenter._product.LOPSaveItem;

            this._rcCtrl.CalBaseWave = DataCenter._product.TestCondition.CalByWave;

            this._rcCtrl.LoadCurrentFormat(pathAndFileName, DataCenter._uiSetting.FormatName.ToString());

            this._rcCtrl.LoadFilterData(DataCenter._product.TestCondition.TestItemArray,
                                                           Path.Combine(Constants.Paths.TOOLS_DIR, "FilterCriteriaRC.xml"),
                                                           DataCenter._toolConfig.IsEnableUseRecipeCriterion);

            this._rcCtrl.ParseTitleDataIndex(DataCenter._product.TestCondition.TestItemArray);

            this._rcCtrl.CreateGainOffset(DataCenter._product.TestCondition.TestItemArray);

            //-----------------------------------------------------
            // (1) Update Coef Item
            //-----------------------------------------------------
            this.cmbDisplayTable.Items.Clear();

            for (int index = 1; index <= this._rcCtrl.CoefTableCount; index++)
            {
                this.cmbDisplayTable.Items.Add("Coef_" + index.ToString());

                this.cmbDisplayTable.SelectedIndex = 0;
            }

            this.txtStdPathAndFile.Text = "";

            this.txtMsrtPathAndFile.Text = "";

            this.UpdateDataToControls();

            this.UpateSettingDataToUI();

            this.UpdateDataToSysCoefDgv();

            this.TopMost = false;
        }

        private void btnLoadStd_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Title = "Import CoefTable Data from File";

            openFileDialog.Filter = "CSV files (*.csv)|*.csv";

            openFileDialog.FilterIndex = 1;    // default value = 1

            openFileDialog.InitialDirectory = DataCenter._toolConfig.StdChannelFileDir;

            openFileDialog.FileName = "";

            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.txtStdPathAndFile.Text = openFileDialog.FileName;

                EErrorCode errCode;

                if (DataCenter._uiSetting.UserID == EUserID.Lextar)
                {
                    errCode = this._rcCtrl.LoadStdFromFile(openFileDialog.FileName, 0, true);
                }
                else
                {
                    errCode = this._rcCtrl.LoadStdFromFile(openFileDialog.FileName, 0);
                }

                if (errCode != EErrorCode.NONE)
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

            openFileDialog.Filter = "CSV files (*.csv)|*.csv";

            openFileDialog.FilterIndex = 1;    // default value = 1

            openFileDialog.InitialDirectory = DataCenter._toolConfig.MsrtChannelFileDir;

            openFileDialog.FileName = "";

            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.txtMsrtPathAndFile.Text = openFileDialog.FileName;

                EErrorCode errCode = this._rcCtrl.LoadMsrtFromFile(openFileDialog.FileName, 0);

                if (errCode != EErrorCode.NONE)
                {
                    this.txtMsrtPathAndFile.Text = "";

                    MessageBox.Show("UserData & Msrt. Title is Not Match", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSaveSetting_Click(object sender, EventArgs e)
        {
            this.SaveFilterData();

            this.SaveGainOffsetSetting();

            this._rcCtrl.LoadFilterData(DataCenter._product.TestCondition.TestItemArray,
                                                      Path.Combine(Constants.Paths.TOOLS_DIR, "FilterCriteriaRC.xml"),
                                                      DataCenter._toolConfig.IsEnableUseRecipeCriterion);

            this.UpateSettingDataToUI();

            this._rcCtrl.CreateGainOffset(DataCenter._product.TestCondition.TestItemArray);
        }

        private void btnCalcuate_Click(object sender, EventArgs e)
        {
            if (this._rcCtrl.StdTable == null || this._rcCtrl.MsrtTable == null)
            {
                MessageBox.Show("Not the same row/col data", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            this._rcCtrl.FilterOrignalData(DataCenter._product.TestCondition.TestItemArray);

            this._rcCtrl.CompareStdAndMsrt();

            this._rcCtrl.SetData(DataCenter._machineConfig.ChannelConfig.ChannelCount);

            this._rcCtrl.Calculate();

            this.UpdateDataToCalcCoefDgv();

            this.UpdateWaveAndLopChart();

            this.UpdateVoltageChart();
        }

        private void btnSaveCalibrationData_Click(object sender, EventArgs e)
        {
            if (this._rcCtrl.StdTable == null || this._rcCtrl.MsrtTable == null)
            {
                return;
            }

            TestItemData[] testItems = DataCenter._product.TestCondition.TestItemArray;

            string fromKeyName = string.Empty;

            string toKeyName = string.Empty;

            int fromChannel = -1;

            int toChannel = -1;

            for (toChannel = 0; toChannel < DataCenter._product.TestCondition.ChannelConditionTable.Count; toChannel++)
            {
                ChannelConditionData condi = DataCenter._product.TestCondition.ChannelConditionTable.Channels[toChannel];

                foreach (TestItemData item in testItems)
                {
                    TestResultData[] data = item.MsrtResult;

                    for (int i = 0; i < data.Length; i++)
                    {
                        toKeyName = data[i].KeyName;

                        for (int j = 0; j < this.dgvSysCoef.Rows.Count; j++)
                        {
                            if (Convert.ToInt32(this.dgvSysCoef[3, j].Value) == 0)   // Type = 0, None
                                continue;

                            fromKeyName = this.dgvSysCoef.Rows[j].Cells[7].Value.ToString();

                            fromChannel = Convert.ToInt32(this.dgvSysCoef[2, j].Value);

                            if (fromKeyName == toKeyName && fromChannel == (toChannel + 1))   // dgv display channel base=1, condition Talbe channel base=0
                            {
                                GainOffsetData coef = condi.GetByChannelGainOffsetData(toKeyName);

                                coef.Gain = Convert.ToDouble(this.dgvSysCoef.Rows[j].Cells[5].Value);

                                coef.Offset = Convert.ToDouble(this.dgvSysCoef.Rows[j].Cells[6].Value);

                                break;
                            }

                        }
                    }
                }
            }

            Host.UpdateDataToAllUIForm();

            DataCenter.SaveProductFile();
        }

        private void btnOpenStdFilePath_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("");

            if (path != string.Empty)
            {
                this.txtOpenStdFilePath.Text = path;
            }
        }

        private void btnOpenMsrtFilePath_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("");

            if (path != string.Empty)
            {
                this.txtOpenMsrtFilePath.Text = path;
            }
        }

        private void btnSaveToolsConfig_Click(object sender, EventArgs e)
        {
            DataCenter._toolConfig.StdChannelFileDir = this.txtOpenStdFilePath.Text;
            DataCenter._toolConfig.MsrtChannelFileDir = this.txtOpenMsrtFilePath.Text;

            DataCenter.SaveToolsConfig();

            this.UpdateDataToControls();
        }

        private void cmbDisplayTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateWaveAndLopChart();
        }

        private void cmbDisplayWave_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateWaveAndLopChart();
        }

        private void cmbDisplayLOP_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateWaveAndLopChart();
        }

        private void btnCombineAllParamToTable_Click(object sender, EventArgs e)
        {
            DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show("Combine Calc Coef into By Channel Gain/Offset？ ", "Combine ?", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);

            if (result != DialogResult.OK)
                return;

            this.CombineGainOffset();
        }

        #endregion

        private void lblSysGainOffset_Click(object sender, EventArgs e)
        {

        }

        private void dgvSysCoef_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


    }
}
