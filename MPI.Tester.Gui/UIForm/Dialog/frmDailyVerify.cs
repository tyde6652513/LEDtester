using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using MPI.Tester.Tools;
using MPI.Tester.Data;
using System.IO;
using ZedGraph;
using System.Xml.Xsl;
using MPI.Xml;
using System.Xml.Serialization;


namespace MPI.Tester.Gui
{
    public partial class frmDailyVerify : Form
    {
        private DailyCheckCtrl _dailyCheckCtrl;
        private LogDailyCheck _log;
        public bool isAlreadyShow = false;
        private XmlDocument _xmlDoc;

        EErrorCode _errCode = EErrorCode.NONE;

        double[] LogData;
        string[] LogDate;
        private string[] eti = new string[9] { "WATT_1", "WLD_1", "MVF_4", "MVF_3", "MVF_2", "MVF_1", "MIR_1", "MVZ_1", "MTHYVD_1" };


        public frmDailyVerify()
        {
            InitializeComponent();
        }

        private void frmDailyVerify_Load(object sender, EventArgs e)
        {
            this.TopMost = true;

            this.TopMost = false;

            switch (DataCenter._uiSetting.UserDefinedData.DailyCheckControlsMode)
            {
                case 0:
                    this.tabcChartInfo.SelectedTabIndex = 1;
                    break;
                case 1:
                    this.tabcChartInfo.SelectedTabIndex = 0;
                    break;
                default:
                    this.tabcChartInfo.SelectedTabIndex = 0;
                    break;
            }

            this.UpdateDataToControls();

            this._log = new LogDailyCheck(DataCenter._uiSetting.TaskSheetFileName);

            this.UpdateLog();

            this.isAlreadyShow = true;

            this.lstState.Items.Clear();

            eti = DataCenter._uiSetting.UserDefinedData.DCheckSaveLogFormat;

            if (this.LoadUserFormatAndSpec())
            {
                PlotGraph.Clear(this.zgcShowItemResult);

                gpOperate.Enabled = true;

                this.tabDailyCheck.Enabled = true;

                this.cmbSelectStdFile.Items.Clear();

                cmbSelectStdFile.Items.AddRange(DataCenter.GetAllFilesList(DataCenter._toolConfig.DCheck.StdFileDir, DataCenter._uiSetting.TestResultFileExt));

                this.cmbSelectMsrtFile.Items.Clear();

                cmbSelectMsrtFile.Items.AddRange(DataCenter.GetAllFilesList(DataCenter._toolConfig.DCheck.MsrtFileDir, DataCenter._uiSetting.TestResultFileExt));
                //  Auto Run Sequence
                this.AutoRunSequence();
            }
            else
            {
                gpOperate.Enabled = false;

                this.tabDailyCheck.Enabled = false;
            }
        }

        #region >>> Private Method <<<

        private void PushDataToListBox(string msg)
        {
            this.lstState.Items.Add(msg);
            this.lstState.SelectedIndex = this.lstState.Items.Count - 1;
        }

        private bool LoadUserFormatAndSpec()
        {
            if (!this.LoadUserFormat(DataCenter._uiSetting.UserID, DataCenter._uiSetting.FormatName))
            {
                this.PushDataToListBox(" Load User Format  FAIL");
                return false;
            }

            if (DataCenter._toolConfig.DCheck.CriterionBy == EDailyCheckSpecBy.RECIPE)
            {
                if (this.LoadCriterion(DataCenter._uiSetting.TaskSheetFileName, "123"))
                {
                    this.UpdateSpecToUI();
                    this.PushDataToListBox("Spec:" + DataCenter._uiSetting.TaskSheetFileName + " Load Success");
                    this.tabDailyCheck.Enabled = true;
                }
                else
                {
                    this.PushDataToListBox("Load Criteria FAIL");
                    this.tabDailyCheck.Enabled = false;
                    return false;
                }
            }
            return true;
        }

        private void UpdateLog()
        {
            PlotGraph.Clear(this.zedLog);
            this.dgvLogResult.Rows.Clear();

            List<double> xData = new List<double>();
            List<string> xTitle = new List<string>();
            List<double> yData = new List<double>();
            int index = 1;
            int row = 0;

            foreach (LogDailyItem item in this._log.LogData.Items)
            {
                this.dgvLogResult.Rows.Add();
                this.dgvLogResult[0, row].Value = index.ToString();
                this.dgvLogResult[1, row].Value = item.CheckTime;
                this.dgvLogResult[2, row].Value = item.CheckFileName;
                this.dgvLogResult[3, row].Value = item.PowerGain;
                this.dgvLogResult[4, row].Value = item.IsCheckResult;
                yData.Add(item.PowerGain);
                xData.Add(index);
                index++;
                xTitle.Add(item.CheckTime);
                row++;
            }

            double[] Boundary1 = new double[xData.Count];
            double[] Boundary2 = new double[xData.Count];
            double[] Boundary3 = new double[xData.Count];
            double[] Boundary4 = new double[xData.Count];

            for (int i = 0; i < xData.Count; i++)
            {
                Boundary1[i] = 3;
                Boundary2[i] = -3;
                Boundary3[i] = 1;
                Boundary4[i] = -1;
            }

            LogData = yData.ToArray();
            LogDate = xTitle.ToArray();

            PlotGraph.SetGrid(this.zedLog, false, Color.Silver, Color.Transparent);
            PlotGraph.SetLabel(this.zedLog, "", "", "LOP Bias (%)", 16);
            PlotGraph.DrawPlot(this.zedLog, xTitle.ToArray(), xData.ToArray(), yData.ToArray(), true, 1.5F, Color.Blue, SymbolType.Square, true, true, false, "AA");
            PlotGraph.DrawPlot(this.zedLog, xTitle.ToArray(), xData.ToArray(), Boundary1, true, 1.5F, Color.Tomato, SymbolType.None, true, true, false, "AA");
            PlotGraph.DrawPlot(this.zedLog, xTitle.ToArray(), xData.ToArray(), Boundary2, true, 1.5F, Color.Tomato, SymbolType.None, true, true, false, "AA");
            PlotGraph.DrawPlot(this.zedLog, xTitle.ToArray(), xData.ToArray(), Boundary3, true, 1.5F, Color.Green, SymbolType.None, true, true, false, "AA");
            PlotGraph.DrawPlot(this.zedLog, xTitle.ToArray(), xData.ToArray(), Boundary4, true, 1.5F, Color.Green, SymbolType.None, true, true, false, "AA");
            this.zedLog.Refresh();
        }

        private bool AutoRunSequence()
        {
            //if (!DataCenter._uiSetting.WaferNumber.ToUpper().Contains("STD") || !DataCenter._uiSetting.WaferNumber.ToUpper().Contains("CK"))
            //    return;

            if (DataCenter._toolConfig.DCheck.AutoRunLevel == 0)
                return false;

            if (DataCenter._toolConfig.DCheck.AutoRunLevel >= 1) //  0 
            {
                //------------------------------------------
                // (01) Copy Std File From Server
                //------------------------------------------
                string stdWaferNumber = DataCenter._uiSetting.WaferNumber + "." + DataCenter._uiSetting.TestResultFileExt;
                string serverStdPath = Path.Combine(DataCenter._toolConfig.DCheck.StdFileDir, stdWaferNumber);
                string loaclStdPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, stdWaferNumber);

                if (File.Exists(serverStdPath))
                {
                    if (File.Exists(loaclStdPath))
                    {
                        File.Delete(loaclStdPath);
                    }
                    try
                    {
                        File.Copy(serverStdPath, loaclStdPath);
                    }
                    catch
                    {
                        this._errCode = EErrorCode.DailyCheck_SpeFileIsNotExist;
                        this.PushDataToListBox("Server is Not Conntect....");
                        return false;
                    }
                }
                else
                {
                    _errCode = EErrorCode.DailyCheck_SpeFileIsNotExist;
                    this.PushDataToListBox("Std File is Not Existence....");
                    return false;
                }

                // Load Std File
                if (!this.LoadStdFile(loaclStdPath))
                {
                    //SaveCheckResultFile();
                    return false;
                }
                //------------------------------------------
                // (02) Open Local Std & Msrt File 
                //------------------------------------------              
                string msrtFile = Path.Combine(DataCenter._toolConfig.DCheck.MsrtFileDir, DataCenter._uiSetting.WaferNumber + "." + DataCenter._uiSetting.TestResultFileExt);

                //  Load Msrt File
                this._dailyCheckCtrl.LoadMsrtDataCount = 0;

                if (this._dailyCheckCtrl.LoadMsrtFromFile(msrtFile, 0) == EErrorCode.NONE)
                {
                    this.lblOperateID.Text = File.GetLastWriteTime(msrtFile).ToString();

                    this.PushDataToListBox("Load Msrt File OK....");

                    TimeSpan ts = DateTime.Now - File.GetLastWriteTime(msrtFile);

                    if (ts.TotalHours > DataCenter._toolConfig.DCheck.TestFileOverdueHours && DataCenter._toolConfig.DCheck.TestFileOverdueHours > 0)
                    {
                        //this.PushDataToListBox("Msrt File is overdue");
                    }

                    RenameMsrtFileNameByCreateTime(msrtFile);
                }
                else
                {
                    this.PushDataToListBox("Load Msrt File ERROR !! ");
                    this.lblOperateID.Text = string.Empty;
                    return false;
                }
            }
            //------------------------------------------
            // (03) Run Auto Check 
            // (04) Auto Save Dialy Report
            //------------------------------------------         
            if (DataCenter._toolConfig.DCheck.AutoRunLevel >= 2) //  0 
                this.RunCalculate();

            return true;

        }

        private bool LoadUserFormat(EUserID user, string formatName)
        {
            //------------------------------------------
            // Load C:\MPI\LEDTester current format
            //------------------------------------------
            string fileNameWithExt = string.Format("{0}{1}", "User", ((int)user).ToString("0000")) + ".xml";
            string pathAndFileName = Path.Combine(Constants.Paths.USER_DIR, fileNameWithExt);

            if (DataCenter._uiSetting.UserDefinedData.DCheckFormat.Length == 0)
            {
                this.lstState.Items.Add("Load DailyCheck Format Error");
                return false;
            }
            //-----------------------------------------------------
            // (1) Load format data from files 
            //-----------------------------------------------------
            this._dailyCheckCtrl = new DailyCheckCtrl(DataCenter._uiSetting.UserDefinedData.DCheckFormat);
            this._dailyCheckCtrl.LOPSaveItem = DataCenter._product.LOPSaveItem;
            this._dailyCheckCtrl.CalBaseWave = DataCenter._product.TestCondition.CalByWave;

            //-----------------------------------------------------
            // (2) Set parameters of current product from UI setting
            //-----------------------------------------------------         
            if (this._dailyCheckCtrl.Init(pathAndFileName, formatName) == false)
            {
                this.lstState.Items.Add("Load Current Format Error");
                return false;
            }
            else
            {
                this.CrateResultDgvHeader();
            }
            return true;
        }

        private bool LoadCriterion(string recipeFileName, string testFileName)
        {
            try
            {
                string serverConditonFullPath = DataCenter._toolConfig.DCheck.CriterionFileAndPath;
                string loaclConditonFullPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, Path.GetFileName(DataCenter._toolConfig.DCheck.CriterionFileAndPath));

                if (this._dailyCheckCtrl.LoadCriterion(DataCenter._toolConfig.DCheck.CriterionBy,
                    serverConditonFullPath, loaclConditonFullPath, recipeFileName,
                     testFileName, DataCenter._uiSetting.UserID) == false)
                {
                    Host.SetErrorCode(EErrorCode.DailyCheck_LoadSpecByRecipeFail);
                    return false;
                }
                return true;
            }
            catch
            {
                Console.WriteLine("[DailyCheck], Open Criterion Not Defined ERROR");
                return false;
            }
        }

        private void btnLoadStdFile_Click(object sender, EventArgs e)
        {
            if (this._dailyCheckCtrl == null)
                return;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //openFileDialog1.Filter = "CSV文件|*.CSV";
            openFileDialog1.InitialDirectory = DataCenter._toolConfig.DCheck.StdFileDir;
            openFileDialog1.FileName = "";
            openFileDialog1.FilterIndex = 1;    // default value = 1
            openFileDialog1.Multiselect = false;
            this._dailyCheckCtrl.IsArrangeByRowCol = this.rdAutoCheck.Checked;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (this.LoadStdFile(openFileDialog1.FileName))
                {
                    this.lstState.Items.Add("Load Std File OK !! ");
                }
                else
                {
                    //this.lstState.Items.Add("Load Std File OK !! ");
                }
            }
        }

        private bool LoadStdFile(string fileName)
        {
            string file = Path.GetFileNameWithoutExtension(fileName);

            if (DataCenter._toolConfig.DCheck.CriterionBy == EDailyCheckSpecBy.TestFileName)
            {
                if (LoadCriterion("123", file))
                {
                    this.UpdateSpecToUI();
                    this.btnCalc.Enabled = true;
                    this.btnRunCalc.Enabled = true;
                    this.PushDataToListBox("Load Spec => " + file + " OK ");
                }
                else
                {
                    _errCode = EErrorCode.DailyCheck_LoadSpecByFileNamFail;
                    this.PushDataToListBox("Load Spec =>" + file + " FAIL ");
                    this.btnCalc.Enabled = false;
                    this.btnRunCalc.Enabled = false;
                    return false;
                }
            }

            this._dailyCheckCtrl.LoadStdDataCount = 0;

            if (DataCenter._uiSetting.UserID == EUserID.Lextar)
            {
                if (this._dailyCheckCtrl.LoadStdFromFile(fileName, 0, true) == EErrorCode.NONE)
                {
                    this.lblTestFileName.Text = file;
                    this.PushDataToListBox("Load Std File OK !! ");
                    return true;
                }
                else
                {
                    _errCode = EErrorCode.DailyCheck_LoadStdFileFail;
                    this.PushDataToListBox("Load Std File FAIL !! ");
                    return false;
                }
            }
            else
            {
                if (this._dailyCheckCtrl.LoadStdFromFile(fileName, 0) == EErrorCode.NONE)
                {
                    this.lblTestFileName.Text = file;
                    this.PushDataToListBox("Load Std File OK !! ");
                    return true;
                }
                else
                {
                    _errCode = EErrorCode.DailyCheck_LoadStdFileFail;
                    this.PushDataToListBox("Load Std File FAIL !! ");
                    return false;
                }
            }
        }

        private void CrateResultDgvHeader()
        {
            this.dgvResultData.Columns.Clear();
            this.dgvResultData.ColumnCount = this._dailyCheckCtrl.UserDefineName.Length;

            for (int i = 0; i < this._dailyCheckCtrl.UserDefineName.Length; i++)
            {
                this.dgvResultData.Columns[i].HeaderText = this._dailyCheckCtrl.UserDefineName[i];
                this.dgvResultData.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void UpdateDataToControls()
        {
            this.lblCriterionBy.Text = "By " + DataCenter._toolConfig.DCheck.CriterionBy.ToString();
            this.lblRecipeFileName.Text = DataCenter._uiSetting.TaskSheetFileName;
            this.lblTestFileName.Text = DataCenter._uiSetting.TestResultFileName;
            this.lblDailyWatchResult.Text = "NONE";
            lblOperateID.Text = string.Empty;
            this.lblDailyWatchResult.BackColor = Color.AntiqueWhite;
        }

        private void UpdateSpecToUI()
        {
            this.dgvDailyWatchSpec.Rows.Clear();
            int row = 0;

            foreach (DailyWatchSpec spec in this._dailyCheckCtrl.SpecInfo.Data.Values)
            {
                this.dgvDailyWatchSpec.Rows.Add();
                this.dgvDailyWatchSpec[0, row].Value = (int)spec.CriteriaType;
                this.dgvDailyWatchSpec[1, row].Value = (string)spec.Name;
                this.dgvDailyWatchSpec[2, row].Value = (string)spec.KeyName;
                this.dgvDailyWatchSpec[3, row].Value = (bool)spec.IsEnbaleFilter;
                this.dgvDailyWatchSpec[4, row].Value = (double)spec.MinValue;
                this.dgvDailyWatchSpec[5, row].Value = (double)spec.MaxValue;
                this.dgvDailyWatchSpec[6, row].Value = (double)spec.DailyWatchSpec;  // auto tune Spec
                this.dgvDailyWatchSpec[7, row].Value = (double)spec.ReCalibSpec;   // 
                this.dgvDailyWatchSpec[10, row].Value = (double)spec.DataColIndex;

                this.dgvDailyWatchSpec[11, row].Value = spec.EverDieLowerSpec;
                this.dgvDailyWatchSpec[12, row].Value = spec.EverDieHighSpec;
                this.dgvDailyWatchSpec[14, row].Value = spec.CriteriaUnit;

                if (spec.IsEnable == false && spec.IsEnbaleFilter == false)
                {
                    this.dgvDailyWatchSpec.Rows[row].Visible = false;
                }
                row++;
            }
        }

        private void AddItemToCombox()
        {
            this.cmbCheckItem.Items.Clear();

            if (this._dailyCheckCtrl.Data == null)
            {
                return;
            }

            foreach (KeyValuePair<string, DailyResultData> kvp in this._dailyCheckCtrl.Data)
            {
                if (kvp.Value.IsRunCalculate || kvp.Key == "D_MIR_1")
                {
                    if (kvp.Value.IsEnable)
                    {
                        cmbCheckItem.Items.Add(kvp.Value.Name);
                    }
                }
            }

            if (this.cmbCheckItem.Items.Count <= 0)
            {
                return;
            }


            bool isShowWatt1 = false;
            bool isShowWatt2 = false;

            if (this._dailyCheckCtrl.Data.ContainsKey("D_WATT_1"))
            {
                if (this._dailyCheckCtrl.Data["D_WATT_1"].IsEnable)
                {
                    this.cmbCheckItem.SelectedItem = this._dailyCheckCtrl.Data["D_WATT_1"].Name;
                }
                else
                {
                    if (this._dailyCheckCtrl.Data["D_WATT_2"].IsEnable)
                    {
                        this.cmbCheckItem.SelectedItem = this._dailyCheckCtrl.Data["D_WATT_2"].Name;
                    }
                    else
                    {
                        this.cmbCheckItem.SelectedIndex = 0;
                    }
                }
            }
            else
            {
                this.cmbCheckItem.SelectedIndex = 0;
            }
        }

        private void UpdateResultToDgv()
        {
            if (this._dailyCheckCtrl.Data == null)
            {
                return;
            }

            foreach (KeyValuePair<string, DailyResultData> kvp in this._dailyCheckCtrl.Data)
            {
                if (kvp.Value.IsRunCalculate)
                {
                    for (int i = 0; i < this.dgvDailyWatchSpec.Rows.Count; i++)
                    {
                        if (this.dgvDailyWatchSpec.Rows[i].Cells[2].Value == kvp.Value.KeyName)
                        {
                            this.dgvDailyWatchSpec.Rows[i].Cells[13].Value = kvp.Value.OverSpecCounts;
                            this.dgvDailyWatchSpec.Rows[i].Cells[8].Value = kvp.Value.Avg.ToString("0.000") + kvp.Value.DeltaUnit;

                            if (kvp.Value.IsPASS)
                            {
                                this.dgvDailyWatchSpec.Rows[i].Cells[9].Value = "PASS";
                                this.dgvDailyWatchSpec.Rows[i].Cells[9].Style.BackColor = Color.GreenYellow;
                            }
                            else
                            {
                                this.dgvDailyWatchSpec.Rows[i].Cells[9].Value = "NG";
                                this.dgvDailyWatchSpec.Rows[i].Cells[9].Style.BackColor = Color.Pink; ;
                            }

                            if (kvp.Value.OverSpecCounts == 0)
                            {
                                this.dgvDailyWatchSpec.Rows[i].Cells[13].Style.BackColor = Color.GreenYellow;
                            }
                            else
                            {
                                this.dgvDailyWatchSpec.Rows[i].Cells[13].Style.BackColor = Color.Pink;
                            }

                            continue;
                        }
                    }
                }
            }
        }

        private void UpdateResultToUI(EDailyCheckResult result)
        {
            this.lblDailyWatchResult.Text = "NG";
            this.lblDailyWatchResult.BackColor = Color.Red;
            this.txtToleranceOutOfSpec.BackColor = Color.White;
            this.txtMinCountAccept.BackColor = Color.White;

            switch (result)
            {
                case EDailyCheckResult.PASS:
                    this.lblDailyWatchResult.Text = "PASS";
                    this.lblDailyWatchResult.BackColor = Color.Green;
                    break;
                case EDailyCheckResult.AvgBiasOutSpec:
                    Host.SetErrorCode(EErrorCode.DailyCheck_AvgBiasOutSpec);
                    break;
                case EDailyCheckResult.BoundayOutSpec:
                    Host.SetErrorCode(EErrorCode.DailyCheck_BoundaryOutSpec);
                    this.txtToleranceOutOfSpec.BackColor = Color.Pink;
                    break;
                case EDailyCheckResult.LessThanMinAcceptDies:
                    Host.SetErrorCode(EErrorCode.DailyCheck_LessThanMinAcceptDies);
                    this.txtMinCountAccept.BackColor = Color.Pink;
                    break;
                default:
                    break;
            }

            this.numTotalChips.Value = this._dailyCheckCtrl.LoadStdDataCount;
            this.numFilterChips.Value = (this._dailyCheckCtrl.LoadStdDataCount - this._dailyCheckCtrl.CompareTable.Rows.Count);
            this.txtToleranceOutOfSpec.Text = this._dailyCheckCtrl.EveryDieOutSpecCounts.ToString();
            this.txtMinCountAccept.Text = this._dailyCheckCtrl.CompareTable.Rows.Count.ToString();
            this.numMsrtChips.Value = this._dailyCheckCtrl.LoadMsrtDataCount;

            this.AddItemToCombox();
            this.UpdateResultToDgv();
            this.UpdateResultData();
        }

        private void UpdateResultData()
        {
            this.dgvResultData.Rows.Clear();

            if (this._dailyCheckCtrl.NumbersOfDie == 0)
            {
                this.PushDataToListBox("No The Same Row/Col Data");
                return;
            }
            // Show Raw Data
            string emptyStr = "---";

            for (int i = 0; i < this._dailyCheckCtrl.NumbersOfDie; i++)
            {
                this.dgvResultData.Rows.Add();
                this.dgvResultData.Rows[i].Cells[0].Value = this._dailyCheckCtrl.EveryDieName[i];
            }

            for (int idx = 1; idx < this._dailyCheckCtrl.UserDefineKeyName.Length; idx++)
            {
                string itemName = this._dailyCheckCtrl.UserDefineKeyName[idx];

                // 處理LOP_1(mcd)量測遇到的問題
                if (itemName.Contains("WATT") && DataCenter._product.LOPSaveItem == ELOPSaveItem.mcd)
                {
                    char preChar = itemName[0];
                    char endChar = itemName[itemName.Length - 1];
                    itemName = preChar + "_LOP_" + endChar;
                }

                if (this._dailyCheckCtrl.Data.ContainsKey(itemName))
                {
                    DailyResultData item = this._dailyCheckCtrl.Data[itemName];

                    double[] array = item.DataArray;
                    bool[] bResult = item.IsOutSpec;
                    string unit = item.DeltaUnit;

                    for (int i = 0; i < this._dailyCheckCtrl.NumbersOfDie; i++)
                    {
                        this.dgvResultData.Rows[i].Cells[idx].Value = array[i].ToString("0.000") + unit;

                        if (item.IsOutSpec[i])
                        {
                            this.dgvResultData.Rows[i].Cells[idx].Style.BackColor = Color.Pink;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < this._dailyCheckCtrl.NumbersOfDie; i++)
                    {
                        this.dgvResultData.Rows[i].Cells[idx].Value = emptyStr;
                    }
                }
            }

            int currentRows = this.dgvResultData.Rows.Count;

            this.dgvResultData.Rows.Add();
            this.dgvResultData.Rows[currentRows].Cells[0].Value = "Fail";
            this.dgvResultData.Rows.Add();
            this.dgvResultData.Rows[currentRows + 1].Cells[0].Value = "Max";
            this.dgvResultData.Rows.Add();
            this.dgvResultData.Rows[currentRows + 2].Cells[0].Value = "Avg";
            this.dgvResultData.Rows.Add();
            this.dgvResultData.Rows[currentRows + 3].Cells[0].Value = "Min";

            for (int idx = 1; idx < this._dailyCheckCtrl.UserDefineKeyName.Length; idx++)
            {
                string str = this._dailyCheckCtrl.UserDefineKeyName[idx];

                if (this._dailyCheckCtrl.Data.ContainsKey(str))
                {
                    string unit = this._dailyCheckCtrl.Data[str].DeltaUnit;

                    this.dgvResultData.Rows[currentRows].Cells[idx].Value = this._dailyCheckCtrl.Data[str].OverSpecCounts;
                    this.dgvResultData.Rows[currentRows + 1].Cells[idx].Value = this._dailyCheckCtrl.Data[str].Max.ToString("0.000") + unit;
                    this.dgvResultData.Rows[currentRows + 2].Cells[idx].Value = this._dailyCheckCtrl.Data[str].Avg.ToString("0.000") + unit;
                    this.dgvResultData.Rows[currentRows + 3].Cells[idx].Value = this._dailyCheckCtrl.Data[str].Min.ToString("0.000") + unit;
                }
                else
                {
                    this.dgvResultData.Rows[currentRows].Cells[idx].Value = 0;
                    this.dgvResultData.Rows[currentRows + 1].Cells[idx].Value = "0.000";
                    this.dgvResultData.Rows[currentRows + 2].Cells[idx].Value = "0.000";
                    this.dgvResultData.Rows[currentRows + 3].Cells[idx].Value = "0.000";
                }
            }

            // Move to Dgv End
            this.dgvResultData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            int endIndex = this.dgvResultData.Rows.Count;
            this.dgvResultData.FirstDisplayedScrollingRowIndex = endIndex - 1;

        }

        private void SaveReportByXsltMethod()
        {
            //-----------------------------------------------------------------------------------------------------
            // (1) Save  xml to File
            //-----------------------------------------------------------------------------------------------------
            this._xmlDoc = new XmlDocument();
            string str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                         "<?xml-stylesheet type=\"text/xsl\"" +
                         " href=\"File.xslt\"?><DailyCheck></DailyCheck>";
            this._xmlDoc.LoadXml(str);
            XmlNode rootNode = this._xmlDoc.DocumentElement;

            XmlElement header = this._xmlDoc.CreateElement("Head");
            rootNode.AppendChild(header);

            XmlElement itemData = this._xmlDoc.CreateElement("FileName");
            itemData.InnerText = this.lblTestFileName.Text;
            header.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("OperatorName");
            itemData.InnerText = DataCenter._uiSetting.OperatorName;
            header.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("Time");
            itemData.InnerText = DateTime.Now.ToString();
            header.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("Date");
            itemData.InnerText = DateTime.Now.ToString("yyyy-MM-dd");
            header.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("Time2");
            itemData.InnerText = DateTime.Now.ToString("yyyyMMddhhmmss");
            header.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("Recipe");
            itemData.InnerText = DataCenter._uiSetting.TaskSheetFileName;
            header.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("FResult");

            if (this._dailyCheckCtrl.FinalResult == true)
            {
                itemData.InnerText = "PASS";
            }
            else
            {
                itemData.InnerText = "NG";
            }

            header.AppendChild(itemData);

            itemData = this._xmlDoc.CreateElement("FResult2");
            itemData.InnerText = "PASS";

            header.AppendChild(itemData);

            XmlElement bodyData = this._xmlDoc.CreateElement("Data");
            rootNode.AppendChild(bodyData);

            XmlElement bodyData2 = this._xmlDoc.CreateElement("Data2");
            rootNode.AppendChild(bodyData2);

            for (int i = 0; i < this._dailyCheckCtrl.EveryDieResult.Length; i++)
            {
                XmlElement indexData = this._xmlDoc.CreateElement("row");
                XmlElement indexData2 = this._xmlDoc.CreateElement("row");

                foreach (KeyValuePair<string, DailyResultData> kvp in this._dailyCheckCtrl.Data)
                {
                    indexData.SetAttribute(kvp.Key, kvp.Value.DataArray[i].ToString());
                    indexData2.SetAttribute(kvp.Key, kvp.Value.DataArray[i].ToString());
                }

                if (this._dailyCheckCtrl.EveryDieResult[i] == true)
                {
                    indexData.SetAttribute("Result", "NG");
                    indexData2.SetAttribute("Result", "NG");
                }
                else
                {
                    indexData.SetAttribute("Result", "PASS");
                    indexData2.SetAttribute("Result", "PASS");
                }

                bodyData.AppendChild(indexData);

                if (this._dailyCheckCtrl.EveryDieResult[i] == false)
                {
                    bodyData2.AppendChild(indexData2);
                }
            }

            string xmlResultOutPathAndFile = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, "DailyCheck.temp"); // XML File
            DirectoryInfo coeffDir = new DirectoryInfo(Constants.Paths.LEDTESTER_TEMP_DIR);

            if (!coeffDir.Exists)
            {
                coeffDir.Create();
            }

            XmlTextWriter xtw = new XmlTextWriter(xmlResultOutPathAndFile, null);
            xtw.Formatting = System.Xml.Formatting.None;
            this._xmlDoc.Save(xtw);
            xtw.Close();

            //-----------------------------------------------------------------------------------------------------
            // (2) Transfer  xml to Csv
            //-----------------------------------------------------------------------------------------------------

            string LoaclpathAndFileWithExt = string.Format("{0}{1}", "DCheck-", ((int)DataCenter._uiSetting.UserID).ToString("0000")) + "-Local.xslt";
            string LoaclxsltFileName = Path.Combine(Constants.Paths.USER_DIR, LoaclpathAndFileWithExt); // XML File
            string LoaclcsvResultOutPathAndFile = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, Constants.Files.OUTPUT_CSV_TEMP + "6"); // OUTPUT File

            string pathAndFileWithExt = string.Format("{0}{1}", "DCheck-", ((int)DataCenter._uiSetting.UserID).ToString("0000")) + ".xslt";
            string xsltFileName = Path.Combine(Constants.Paths.USER_DIR, pathAndFileWithExt); // XML File
            string csvResultOutPathAndFile = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, Constants.Files.OUTPUT_CSV_TEMP + "4"); // OUTPUT File

            string pathAndFileWithExt2 = string.Format("{0}{1}", "DCheck-", ((int)DataCenter._uiSetting.UserID).ToString("0000")) + "-B.xslt";
            string xsltFileName2 = Path.Combine(Constants.Paths.USER_DIR, pathAndFileWithExt2); // XML File
            string csvResultOutPathAndFile2 = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, Constants.Files.OUTPUT_CSV_TEMP + "5"); // OUTPUT File


            XslCompiledTransform xsl = new XslCompiledTransform();

            if (File.Exists(LoaclxsltFileName))
            {
                xsl.Load(LoaclxsltFileName);
                xsl.Transform(xmlResultOutPathAndFile, LoaclcsvResultOutPathAndFile);
            }

            if (File.Exists(xsltFileName))
            {
                xsl.Load(xsltFileName);
                xsl.Transform(xmlResultOutPathAndFile, csvResultOutPathAndFile);
            }

            if (File.Exists(xsltFileName2))
            {
                xsl.Load(xsltFileName2);
                xsl.Transform(xmlResultOutPathAndFile, csvResultOutPathAndFile2);
            }

            //-----------------------------------------------------------------------------------------------------
            // (2) Save Csv To File
            //-----------------------------------------------------------------------------------------------------

            DirectoryInfo dirinfo = new DirectoryInfo(DataCenter._toolConfig.DCheck.OutputFileDir);

            if (!dirinfo.Exists)
            {
                dirinfo.Create();
            }

            dirinfo = new DirectoryInfo(DataCenter._toolConfig.DCheck.OutputFileDir2);

            if (!dirinfo.Exists)
            {
                dirinfo.Create();
            }

            string LoaclSaveDCheckResultFilePath = Path.Combine(DataCenter._toolConfig.DCheck.OutputFileDir, DataCenter._uiSetting.MachineName + "calcheck.txt");
            string SaveDCheckResultFilePath2 = Path.Combine(DataCenter._toolConfig.DCheck.OutputFileDir2, DataCenter._uiSetting.MachineName + "calcheck.txt");

            // Save Report Data (1) : File already exist and append 
            if (File.Exists(LoaclSaveDCheckResultFilePath))
            {
                List<string[]> data = CSVUtil.ReadCSV(LoaclSaveDCheckResultFilePath);

                if (data != null)
                {
                    List<string[]> currentData = CSVUtil.ReadCSV(LoaclcsvResultOutPathAndFile);
                    List<string[]> combineData = new List<string[]>();
                    combineData.AddRange(currentData);
                    combineData.AddRange(data);
                    CSVUtil.WriteCSV(LoaclcsvResultOutPathAndFile, combineData);
                }
                else
                {
                    return;
                }
            }

            MPIFile.CopyFile(LoaclcsvResultOutPathAndFile, LoaclSaveDCheckResultFilePath);

            if (File.Exists(LoaclcsvResultOutPathAndFile))
            {
                File.Delete(LoaclcsvResultOutPathAndFile);
            }

            // Save Report Data (2) : IsFilter  : csvResultOutPathAndFile2  NoFilter  : csvResultOutPathAndFile
            if (DataCenter._toolConfig.DCheck.IsFilterOutputFile2)
            {
                MPIFile.CopyFile(csvResultOutPathAndFile2, SaveDCheckResultFilePath2);
            }
            else
            {
                MPIFile.CopyFile(csvResultOutPathAndFile, SaveDCheckResultFilePath2);
            }

        }

        private void SaveLog(bool isPass)
        {
            LogDailyItem item = new LogDailyItem(DateTime.Now.ToString("yy/MM/dd-HH:mm"));
            item.CheckFileName = DataCenter._uiSetting.TestResultFileName;
            item.IsCheckResult = this._dailyCheckCtrl.FinalResult;
            item.RecipeFileName = DataCenter._uiSetting.TaskSheetFileName;

            string[] keyName = this._dailyCheckCtrl.UserDefineKeyName;

            for (int idx = 0; idx < keyName.Length; idx++)
            {
                string key = keyName[idx];

                if (key.Contains("D_") && this._dailyCheckCtrl.Data.ContainsKey(key))
                {
                    if (key.Contains("WATT"))
                    {
                        item.PowerGain = this._dailyCheckCtrl.Data[key].Avg;
                    }
                    else if (key.Contains("WLD"))
                    {
                        item.WLOffset = this._dailyCheckCtrl.Data[key].Avg;
                    }
                    else if (key.Contains("MVF"))
                    {
                        item.VoltOffset = this._dailyCheckCtrl.Data[key].Avg;
                    }
                }
            }

            foreach (DailyResultData data in this._dailyCheckCtrl.Data.Values)
            {
                if (data.IsRunCalculate)
                {
                    ResultData resData = new ResultData();
                    resData.KeyName = data.KeyName;
                    resData.Name = data.Name;
                    resData.AvgBias = data.Avg;
                    resData.Mode = (int)data.Type;
                    item.Data.Add(resData);
                }
            }

            this._log.Add(item);
            this._log.Save();
            this.UpdateLog();
        }

        private void ClearUIData()
        {
            this.lblDailyWatchResult.Text = "NONE";
            this._dailyCheckCtrl.Data = null;
            this.UpdateSpecToUI();
            this.dgvResultData.Rows.Clear();
            PlotGraph.Clear(this.zgcShowItemResult);
        }

        private void RunCalculate()
        {
            this.ClearUIData();

            this._dailyCheckCtrl.IsArrangeByRowCol = DataCenter._toolConfig.DCheck.IsArrangeByRowCol;

            // (1) Filter Data --------------------------------------------------------
            this._dailyCheckCtrl.FilterOrignalData();

            // (2) Compare Data --------------------------------------------------------
            this._dailyCheckCtrl.CompareStdAndMsrt();

            if (this._dailyCheckCtrl.CompareTable == null)
            {
                this.PushDataToListBox("Compare Data Error !! ");
                return;
            }

            // (3)Create Daily Item and Calculate --------------------------------------------------------
            this._dailyCheckCtrl.CrateItemAndCaculate();

            EDailyCheckResult result = this._dailyCheckCtrl.GetCalcResult(DataCenter._toolConfig.DCheck.IsCheckEveryDieInSpec);

            if (this._dailyCheckCtrl.Data == null)
            {
                Host.SetErrorCode(EErrorCode.DailyCheck_NotFinishingTheWork);
                this.PushDataToListBox("No Data , Check Std/Test Data ");
                return;
            }

            if (result != EDailyCheckResult.PASS)
            {
                switch (result)
                {
                    case EDailyCheckResult.AvgBiasOutSpec:
                        Host.SetErrorCode(EErrorCode.DailyCheck_AvgBiasOutSpec);
                        break;
                    case EDailyCheckResult.BoundayOutSpec:
                        Host.SetErrorCode(EErrorCode.DailyCheck_BoundaryOutSpec);
                        break;
                    case EDailyCheckResult.LessThanMinAcceptDies:
                        Host.SetErrorCode(EErrorCode.DailyCheck_LessThanMinAcceptDies);
                        break;
                    default:
                        break;
                }
            }

            // (4) Update Result To UI --------------------------------------------------------
            this.UpdateResultToUI(result);
            // (4) Save Data --------------------------------------------------------
            this.SaveLog(this._dailyCheckCtrl.FinalResult);
            this.SaveCheckResultFile();
            this.SaveDailyReport();
            DataCenter.SaveUISettingToFile();
        }

        private void SaveDailyReport()
        {
            if (this._dailyCheckCtrl.EveryDieName == null)
            {
                return;
            }
            if (this._dailyCheckCtrl.EveryDieName.Length == 0)
                return;

            switch (DataCenter._uiSetting.UserID)
            {
                case EUserID.AquaLite:
                case EUserID.EPITOP:
                case EUserID.Eti:
                case EUserID.NJ000:
                    SaveWMReportFile();
                    break;
                case EUserID.EpiStar:
                    SaveReportByXsltMethod();
                    break;
                default:
                    SaveReportByXsltMethod();
                    break;
            }
        }

        private void SaveWMReportFile()
        {
            string fileName = ParseOutputFileName();

            string outputPathAndFile = Path.Combine(DataCenter._toolConfig.DCheck.OutputFileDir, fileName + ".csv");

            string outputPathAndFile2 = Path.Combine(DataCenter._toolConfig.DCheck.OutputFileDir2, fileName + ".csv");

            if (DataCenter._toolConfig.DCheck.IsCrateMachimeNameFolderToSaveFile)
            {
                string mechineAndFileName = Path.Combine(DataCenter._uiSetting.MachineName, fileName + ".csv");
                outputPathAndFile2 = Path.Combine(DataCenter._toolConfig.DCheck.OutputFileDir2, mechineAndFileName);
            }

            List<string[]> data = new List<string[]>();

            if (this.dgvResultData.Rows.Count == 0)
                return;

            string[] header = new string[this.dgvResultData.Rows[0].Cells.Count];

            for (int col = 0; col < header.Length; col++)
            {
                header[col] = this.dgvResultData.Columns[col].HeaderText;
            }

            data.Add(header);

            for (int row = 0; row < this.dgvResultData.Rows.Count; row++)
            {
                string[] rowData = new string[this.dgvResultData.Rows[0].Cells.Count];

                for (int col = 0; col < rowData.Length; col++)
                {
                    rowData[col] = this.dgvResultData.Rows[row].Cells[col].Value.ToString();
                }
                data.Add(rowData);
            }

            // Path 1 :
            DriveInfo driveInfo;
            driveInfo = new DriveInfo(DataCenter._toolConfig.DCheck.OutputFileDir);

            if (driveInfo.IsReady == false)
            {
                return;
            }

            if (!File.Exists(DataCenter._toolConfig.DCheck.OutputFileDir))
            {
                Directory.CreateDirectory(DataCenter._toolConfig.DCheck.OutputFileDir);
            }

            CSVUtil.WriteCSV(outputPathAndFile, data);

            this.lstState.Items.Add(outputPathAndFile);

            // Path 2 :

            if (outputPathAndFile == outputPathAndFile2)
            {
                this.lstState.Items.Add("Save Completed");
                return;
            }

            driveInfo = new DriveInfo(DataCenter._toolConfig.DCheck.OutputFileDir2);

            if (driveInfo.IsReady == false)
            {
                return;
            }

            if (!File.Exists(DataCenter._toolConfig.DCheck.OutputFileDir2))
            {
                Directory.CreateDirectory(DataCenter._toolConfig.DCheck.OutputFileDir2);
            }

            CSVUtil.WriteCSV(outputPathAndFile2, data);
            this.lstState.Items.Add(outputPathAndFile2);
            this.lstState.Items.Add("Save Completed");

        }

        private string ParseOutputFileName()
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();

            string[] srtArray;

            string rule = DataCenter._uiSetting.UserDefinedData.DailyCheckingSaveReportName;

            srtArray = rule.Split('_');

            foreach (string str in srtArray)
            {
                if (str.ToUpper() == "TestFileName".ToUpper())
                {
                    sb.Append(DataCenter._uiSetting.TestResultFileName);
                    continue;
                }
                else if (str.ToUpper() == "LotNum".ToUpper())
                {
                    sb.Append(DataCenter._uiSetting.LotNumber);
                    continue;
                }
                else if (str.ToUpper() == "WaferNum".ToUpper())
                {
                    sb.Append(DataCenter._uiSetting.WaferNumber);
                    continue;
                }
                else if (str.ToUpper() == "Time".ToUpper())
                {
                    sb.Append(DataCenter._sysSetting.StartTestTime.ToString("yyyyMMddHHmmss"));
                    continue;
                }
                else if (str.ToUpper() == "MachineName".ToUpper())
                {
                    sb.Append(DataCenter._uiSetting.MachineName);
                    continue;
                }
                else if (str.ToUpper() == "EndTestTime".ToUpper())
                {
                    sb.Append(DataCenter._sysSetting.EndTestTime.ToString("yyyyMMddHHmmss"));
                    continue;
                }
                else
                {
                    sb.Append(str);
                }
            }
            return sb.ToString().Trim();
        }

        private void DrawItemResult(string name)
        {
            string xKeyName = "S_" + name;
            string yKeyName = "D_" + name;

            if (_dailyCheckCtrl.EveryDieName == null || _dailyCheckCtrl.EveryDieName.Length == 0)
            {
                return;
            }

            double[] xValue = new double[this._dailyCheckCtrl.EveryDieName.Length];

            double[] yVaule = new double[this._dailyCheckCtrl.EveryDieName.Length];

            string unit = string.Empty;

            double criteriorHigh = 0;

            double criteriorLower = 0;

            double avgSpecHigh = 0;

            double avgSpecLow = 0;

            string showUIWL = "WLD (nm)";

            if (DataCenter._product.TestCondition.CalByWave == ECalBaseWave.By_WLD)
            {
                xValue = this.GetCheckItemValues("S_WLD_1");

                if (xValue == null)
                {
                    xValue = this.GetCheckItemValues("S_WLD_2");
                    showUIWL = "WLD (nm)";
                }
            }
            else
            {
                xValue = this.GetCheckItemValues("S_WLP_1");

                if (xValue == null)
                {
                    xValue = this.GetCheckItemValues("S_WLP_2");
                }

                showUIWL = "WLP (nm)";
            }

            if (xValue == null)
            {
                return;
            }

            if (this._dailyCheckCtrl.Data.ContainsKey(yKeyName))
            {
                yVaule = this._dailyCheckCtrl.Data[yKeyName].DataArray;

                unit = this._dailyCheckCtrl.Data[yKeyName].DeltaUnit;

                criteriorHigh = this._dailyCheckCtrl.Data[yKeyName].EverDieHighSpec;

                criteriorLower = this._dailyCheckCtrl.Data[yKeyName].EverDieLowerSpec;

                this.numOutOfSpec.Value = this._dailyCheckCtrl.Data[yKeyName].OverSpecCounts;

                this.numberChips.Value = this._dailyCheckCtrl.Data[yKeyName].DataArray.Length;

                avgSpecHigh = this._dailyCheckCtrl.Data[yKeyName].AvgHighSpec;

                avgSpecLow = this._dailyCheckCtrl.Data[yKeyName].AvgLowSpec;

                yKeyName = this._dailyCheckCtrl.Data[yKeyName].Name;

            }

            double[] specXvalue = new double[2] { MPI.Maths.Statistic.Min(xValue), MPI.Maths.Statistic.Max(xValue) };
            double[] Boundary1 = new double[2];
            double[] Boundary2 = new double[2];
            double[] Boundary3 = new double[2];
            double[] Boundary4 = new double[2];
            double[] avg = new double[2];

            for (int i = 0; i < specXvalue.Length; i++)
            {
                Boundary1[i] = criteriorHigh;
                Boundary2[i] = criteriorLower;
                Boundary3[i] = avgSpecHigh;
                Boundary4[i] = avgSpecLow;
                avg[i] = MPI.Maths.Statistic.Average(yVaule);
            }

            PlotGraph.Clear(this.zgcShowItemResult);

            PlotGraph.DrawPlot(this.zgcShowItemResult, specXvalue, Boundary3, true, 2.0F, Color.Green, SymbolType.None, true, true, false, "Avg spec");

            PlotGraph.DrawPlot(this.zgcShowItemResult, specXvalue, Boundary4, true, 2.0F, Color.Green, SymbolType.None, true, true, false, "AA");

            PlotGraph.DrawPlot(this.zgcShowItemResult, specXvalue, avg, true, 2.0F, Color.DeepPink, SymbolType.Circle, true, true, false, "AA");

            if (DataCenter._toolConfig.DCheck.IsCheckEveryDieInSpec)
            {
                PlotGraph.DrawPlot(this.zgcShowItemResult, specXvalue, Boundary1, true, 2.0F, Color.Tomato, SymbolType.None, true, true, false, "Every Die spec");

                PlotGraph.DrawPlot(this.zgcShowItemResult, specXvalue, Boundary2, true, 2.0F, Color.Tomato, SymbolType.None, true, true, false, "Every Die spec");
            }

            PlotGraph.SetLabel(zgcShowItemResult, "AAAA", showUIWL, yKeyName + "  " + unit, 14);

            PlotGraph.SetGrid(this.zgcShowItemResult, false, Color.Silver, Color.Transparent);

            PlotGraph.DrawPlot(zgcShowItemResult, xValue, yVaule, false, 2.0F, Color.Blue, SymbolType.Diamond, true, true, false, "Die");
        }


        private double[] GetCheckItemValues(string itemName)
        {
            double[] rtn = null;

            if (this._dailyCheckCtrl.Data.ContainsKey(itemName))
            {
                rtn = this._dailyCheckCtrl.Data[itemName].DataArray;

                double avg = MPI.Maths.Statistic.Average(rtn);

                if (avg < 280.0d)
                {
                    return null;
                }
                return this._dailyCheckCtrl.Data[itemName].DataArray;
            }

            return null;
        }


        private void SaveCheckResultFile()
        {
            if (this._dailyCheckCtrl.Data == null)
            {
                return;
            }

            DriveInfo driveInfo;

            driveInfo = new DriveInfo(DataCenter._toolConfig.DCheck.SaveResultLogDir);

            if (driveInfo.IsReady == false)
            {
                Host.SetErrorCode(EErrorCode.DailyCheck_SaveResultLogFail);
                return;
            }
            else
            {
                string path = Path.Combine(DataCenter._toolConfig.DCheck.SaveResultLogDir, DataCenter._uiSetting.MachineName + "_DailyCheckLog.csv");
                this.SaveLogFile(path);

                if (!Directory.Exists(DataCenter._toolConfig.DCheck.SaveResultLogDir))
                {
                    Directory.CreateDirectory(DataCenter._toolConfig.DCheck.SaveResultLogDir);
                }
            }

            string backupPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, DataCenter._uiSetting.MachineName + "_DailyCheckLog.csv");
            DataCenter._uiSetting.DailyCheckLogFileName = backupPath;
            this.SaveLogFile(backupPath);
        }

        private void SaveLogFile(string path)
        {
            List<string> lagFile = new List<string>();

            if (File.Exists(path))
            {
                try
                {
                    using (StreamReader fileReader = new StreamReader(path, Encoding.Default))
                    {
                        while (!fileReader.EndOfStream)
                        {
                            string strLine = fileReader.ReadLine();
                            lagFile.Add(strLine);
                        }
                        fileReader.Close();
                    }
                }
                catch (IOException)
                {
                    // Console.WriteLine("The file could not be opened because it was locked by another process.");
                    return;
                }
                catch (Exception)
                {
                    // Console.WriteLine(ex.ToString());
                    return;
                }
            }

            StringBuilder sbTitle = new StringBuilder();

            StringBuilder sb = new StringBuilder();

            switch (DataCenter._uiSetting.UserID)
            {
                case EUserID.Eti:
                case EUserID.MPI_Standard:
                case EUserID.EPITOP:
                case EUserID.HCSemiTek:
                case EUserID.Lumitek:

                    sbTitle.Append("TestTime");
                    sbTitle.Append(",");
                    sbTitle.Append("Date");
                    sbTitle.Append(",");
                    sbTitle.Append("Time");
                    sbTitle.Append(",");
                    sbTitle.Append("Machine Name");
                    sbTitle.Append(",");
                    sbTitle.Append("Recipe");
                    sbTitle.Append(",");
                    sbTitle.Append("File Name");
                    sbTitle.Append(",");
                    sbTitle.Append("Op ID");
                    sbTitle.Append(",");
                    sbTitle.Append("Result");
                    sbTitle.Append(",");
                    sbTitle.Append("ProbingCount");
                    sbTitle.Append(",");
                    sbTitle.Append("LopItem");
                    sbTitle.Append(",");

                    sb.Append(DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    sb.Append(",");
                    sb.Append(DateTime.Now.ToString("yyyy-MM-dd"));
                    sb.Append(",");
                    sb.Append(DateTime.Now.ToString("HH:mm:ss"));
                    sb.Append(",");
                    sb.Append(DataCenter._uiSetting.MachineName);
                    sb.Append(",");
                    sb.Append(DataCenter._uiSetting.TaskSheetFileName);
                    sb.Append(",");
                    sb.Append(DataCenter._uiSetting.TestResultFileName);
                    sb.Append(",");
                    sb.Append(DataCenter._uiSetting.OperatorName);
                    sb.Append(",");
                    sb.Append(this._dailyCheckCtrl.FinalResult.ToString());
                    sb.Append(",");
                    sb.Append(DataCenter._uiSetting.ProbingCount1);
                    sb.Append(",");
                    sb.Append(DataCenter._product.LOPSaveItem.ToString());
                    sb.Append(",");

                    // fix mcd measure issue
                    string[] logItemName = new string[eti.Length];

                    for (int idx = 0; idx < eti.Length; idx++)
                    {
                        string itemName = eti[idx];

                        if (itemName.Contains("WATT") && DataCenter._product.LOPSaveItem == ELOPSaveItem.mcd)
                        {
                            char endChar = itemName[itemName.Length - 1];
                            itemName = "LOP_" + endChar;
                            logItemName[idx] = itemName;
                        }
                        else
                        {
                            logItemName[idx] = itemName;
                        }
                    }


                    for (int idx = 0; idx < logItemName.Length; idx++)
                    {
                        bool ishaveItem = false;

                        foreach (TestItemData tid in DataCenter._product.TestCondition.TestItemArray)
                        {
                            if (tid.MsrtResult == null)
                                continue;

                            if (tid.GainOffsetSetting == null)
                                continue;

                            for (int i = 0; i < tid.MsrtResult.Length; i++)
                            {
                                TestResultData result = tid.MsrtResult[i];

                                if (logItemName[idx] == result.KeyName)
                                {
                                    string name = string.Empty;

                                    if (this._dailyCheckCtrl.TitleName.ContainsKey(result.KeyName))
                                    {
                                        name = this._dailyCheckCtrl.TitleName[result.KeyName];
                                    }
                                    else
                                    {
                                        name = result.KeyName;
                                    }

                                    sbTitle.Append(name + "_gain");
                                    sbTitle.Append(",");
                                    sbTitle.Append(name + "_offset");
                                    sbTitle.Append(",");

                                    sb.Append(tid.GainOffsetSetting[i].Gain.ToString());
                                    sb.Append(",");
                                    sb.Append(tid.GainOffsetSetting[i].Offset.ToString());
                                    sb.Append(",");

                                    if (this._dailyCheckCtrl.Data.ContainsKey("D_" + result.KeyName))
                                    {
                                        DailyResultData data = this._dailyCheckCtrl.Data["D_" + result.KeyName];
                                        //sb.Clear();
                                        sb.Append(data.Avg.ToString("0.000"));
                                        sb.Append(",");
                                        sb.Append(data.DataArray.Max().ToString("0.000"));
                                        sb.Append(",");
                                        sb.Append(data.DataArray.Min().ToString("0.000"));
                                        sb.Append(",");

                                        sbTitle.Append(name + "_avg");
                                        sbTitle.Append(",");
                                        sbTitle.Append(name + "_max");
                                        sbTitle.Append(",");
                                        sbTitle.Append(name + "_min");
                                        sbTitle.Append(",");

                                    }
                                    else
                                    {
                                        sb.Append("0.000");
                                        sb.Append(",");
                                        sb.Append("0.000");
                                        sb.Append(",");
                                        sb.Append("0.000");
                                        sb.Append(",");
                                        sbTitle.Append(name + "_avg");
                                        sbTitle.Append(",");
                                        sbTitle.Append(name + "_max");
                                        sbTitle.Append(",");
                                        sbTitle.Append(name + "_min");
                                        sbTitle.Append(",");
                                    }

                                    ishaveItem = true;

                                    continue;
                                }
                            }
                        }

                        if (!ishaveItem)
                        {
                            sb.Append("1.000");
                            sb.Append(",");
                            sb.Append("0.000");
                            sb.Append(",");
                            sb.Append("0.000");
                            sb.Append(",");
                            sb.Append("0.000");
                            sb.Append(",");
                            sb.Append("0.000");
                            sb.Append(",");

                            sbTitle.Append(eti[idx] + "_gain");
                            sbTitle.Append(",");
                            sbTitle.Append(eti[idx] + "_offset");
                            sbTitle.Append(",");
                            sbTitle.Append(eti[idx] + "_avg");
                            sbTitle.Append(",");
                            sbTitle.Append(eti[idx] + "_max");
                            sbTitle.Append(",");
                            sbTitle.Append(eti[idx] + "_min");
                            sbTitle.Append(",");
                        }
                    }

                    if (lagFile.Count == 0)
                    {
                        lagFile.Add(sbTitle.ToString());
                    }
                    lagFile.Add(sb.ToString());
                    break;

                //----------------------------------------------------------------------------
                case EUserID.EpiStar:

                    sb.Append(DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    sb.Append(",");
                    sb.Append(DataCenter._uiSetting.TaskSheetFileName);
                    sb.Append(",");
                    sb.Append(DataCenter._uiSetting.TestResultFileName);
                    sb.Append(",");
                    sb.Append(DataCenter._uiSetting.OperatorName);
                    sb.Append(",");
                    sb.Append(this._dailyCheckCtrl.FinalResult.ToString());
                    sb.Append(",");
                    sb.Append("AcceptRecipe");
                    sb.Append(",");

                    foreach (string s in this._dailyCheckCtrl.SpecInfo.TheOtherAcceptRecipes)
                    {
                        sb.Append(s);
                        sb.Append(",");
                    }

                    foreach (DailyResultData data in this._dailyCheckCtrl.Data.Values)
                    {
                        if (data.IsRunCalculate)
                        {
                            sb.Append(data.KeyName);
                            sb.Append(",");
                            sb.Append(data.Avg.ToString("0.000"));
                            sb.Append(",");
                        }
                    }

                    lagFile.Add(sb.ToString());

                    break;

                //----------------------------------------------------------------------------
                default:

                    sb.Append(DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    sb.Append(",");
                    sb.Append(DataCenter._uiSetting.TaskSheetFileName);
                    sb.Append(",");
                    sb.Append(DataCenter._uiSetting.TestResultFileName);
                    sb.Append(",");
                    sb.Append(DataCenter._uiSetting.OperatorName);
                    sb.Append(",");
                    sb.Append(this._dailyCheckCtrl.FinalResult.ToString());
                    lagFile.Add(sb.ToString());
                    break;

            }

            try
            {
                using (StreamWriter sw = new StreamWriter(path, false))
                {
                    foreach (string str in lagFile)
                    {
                        sw.WriteLine(str);
                    }
                    // sw.WriteLine(exportSptXYCalibrationPath);
                    sw.Close();
                }
            }
            catch
            {
                Host.SetErrorCode(EErrorCode.DailyCheck_SaveResultLogFail);
                return;
            }
        }

        #endregion

        #region >>> UI Event Handler <<<

        private void btnCalc_Click(object sender, EventArgs e)
        {
            if (this._dailyCheckCtrl == null)
                return;
            this.RunCalculate();
        }

        private void btnOpenFilePath_Click(object sender, EventArgs e)
        {
            //string path = this.SelectPath("");
            //if (path != string.Empty)
            //{
            //    this.txtStdFileDir.Text = path;
            //}
        }

        private void btnLoadMsrtFile_Click(object sender, EventArgs e)
        {
            if (this._dailyCheckCtrl == null)
                return;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.FileName = "";
            openFileDialog1.FilterIndex = 1;    // default value = 1
            openFileDialog1.Multiselect = false;
            openFileDialog1.InitialDirectory = DataCenter._toolConfig.DCheck.MsrtFileDir;
            this._dailyCheckCtrl.IsArrangeByRowCol = this.rdAutoCheck.Checked;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (this._dailyCheckCtrl.LoadMsrtFromFile(openFileDialog1.FileName, 0) != EErrorCode.NONE)
                {
                    this.btnCalc.Enabled = false;
                    btnRunCalc.Enabled = false;
                    this.lstState.Items.Add("Load Criterion File FAIL !! ");
                    return;
                }
                else
                {
                    this.btnCalc.Enabled = true;
                    btnRunCalc.Enabled = true;
                }
            }
        }

        private void btnSaveDailyReport_Click(object sender, EventArgs e)
        {
            if (this._dailyCheckCtrl == null)
                return;
            this.SaveDailyReport();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDailyVerify_Closing(object sender, FormClosingEventArgs e)
        {
            isAlreadyShow = false;
        }

        private void cmbCheckItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._dailyCheckCtrl.Data == null)
                return;

            foreach (DailyResultData data in this._dailyCheckCtrl.Data.Values)
            {
                if (data.Name == this.cmbCheckItem.SelectedItem)
                {
                    DrawItemResult(data.KeyName);
                    return;
                }
                else
                {
                    continue;
                }
            }
        }

        private void Mouse_Doubleclick(object sender, MouseEventArgs e)
        {
            CurveItem nearestPointPair;
            int nearestIndex;
            this.zedLog.GraphPane.FindNearestPoint(e.Location, out nearestPointPair, out nearestIndex);
            double we = LogData[nearestIndex];
            string dss = LogDate[nearestIndex];
            lblLogCheckDate.Text = "Date : " + dss + "       WATT_1 :  " + we.ToString();
        }

        private void cmbSelectMsrtFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbSelectMsrtFile.SelectedItem == null)
                return;

            string fileAndPath = Path.Combine(DataCenter._toolConfig.DCheck.MsrtFileDir, this.cmbSelectMsrtFile.SelectedItem.ToString() + "." + DataCenter._uiSetting.TestResultFileExt);

            this._dailyCheckCtrl.LoadMsrtDataCount = 0;


            if (this._dailyCheckCtrl.LoadMsrtFromFile(fileAndPath, 0) != EErrorCode.NONE)
            {
                return;
            }

            DateTime createTime = File.GetLastWriteTime(fileAndPath);
            this.lblOperateID.Text = createTime.ToString();
            this.btnCalc.Enabled = true;
            this.btnRunCalc.Enabled = true;
            this.lstState.Items.Add("Load Msrt File OK !! ");

            TimeSpan ts = DateTime.Now - createTime;

            if (ts.TotalHours > DataCenter._toolConfig.DCheck.TestFileOverdueHours && DataCenter._toolConfig.DCheck.TestFileOverdueHours > 0)
            {
                this.lstState.Items.Add("Msrt File is overdue !! ");
            }

            RenameMsrtFileNameByCreateTime(fileAndPath);
        }

        private void RenameMsrtFileNameByCreateTime(string msrtFileNameAndPath)
        {
            if (!DataCenter._toolConfig.DCheck.IsRenameMsrtFileByCreateTime)
            {
                return;
            }

            if (DataCenter._uiSetting.WaferNumber.ToUpper().Contains("STD") || DataCenter._uiSetting.WaferNumber.ToUpper().Contains("CK") ||
                DataCenter._uiSetting.WaferNumber.ToUpper().Contains("GLD"))
            {
                string fileNameAndCreateTime = Path.GetFileNameWithoutExtension(msrtFileNameAndPath) + "_" + File.GetLastWriteTime(msrtFileNameAndPath).ToString("yyyyMMddHHmm");
                string backupFileAndPath = Path.Combine(DataCenter._toolConfig.DCheck.MsrtFileDir, fileNameAndCreateTime + "." + DataCenter._uiSetting.TestResultFileExt);

                if (File.Exists(msrtFileNameAndPath))
                {
                    if (File.Exists(backupFileAndPath))
                    {
                        File.Delete(backupFileAndPath);
                    }

                    File.Copy(msrtFileNameAndPath, backupFileAndPath);
                    File.Delete(msrtFileNameAndPath);
                }
            }
        }

        private void cmbSelectStdFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbSelectStdFile.SelectedItem == null)
                return;

            string fileAndPath = Path.Combine(DataCenter._toolConfig.DCheck.StdFileDir, this.cmbSelectStdFile.SelectedItem.ToString() + "." + DataCenter._uiSetting.TestResultFileExt);
            this.LoadStdFile(fileAndPath);
        }

        private void btnRunCalc_Click(object sender, EventArgs e)
        {
            if (this._dailyCheckCtrl == null)
                return;
            this.RunCalculate();
        }

        private void tabcChartInfo_Click(object sender, EventArgs e)
        {
            cmbSelectStdFile.Text = string.Empty;
        }

        private void tabVisbChange(object sender, EventArgs e)
        {
            cmbSelectStdFile.Text = string.Empty;
            cmbSelectMsrtFile.Text = string.Empty;
        }

        private void btnLoadStd_Click(object sender, EventArgs e)
        {
            if (this._dailyCheckCtrl == null)
                return;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //openFileDialog1.Filter = "CSV文件|*.CSV";
            openFileDialog1.InitialDirectory = DataCenter._toolConfig.DCheck.StdFileDir;
            openFileDialog1.FileName = "";
            openFileDialog1.FilterIndex = 1;    // default value = 1
            openFileDialog1.Multiselect = false;
            this._dailyCheckCtrl.IsArrangeByRowCol = this.rdAutoCheck.Checked;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (this.LoadStdFile(openFileDialog1.FileName))
                {
                    this.lstState.Items.Add("Load Std File OK !! ");
                }
                else
                {
                    //this.lstState.Items.Add("Load Std File OK !! ");
                }
            }
        }

        private void btnLoadMsrt_Click(object sender, EventArgs e)
        {
            if (this._dailyCheckCtrl == null)
                return;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.FileName = "";
            openFileDialog1.FilterIndex = 1;    // default value = 1
            openFileDialog1.Multiselect = false;
            openFileDialog1.InitialDirectory = DataCenter._toolConfig.DCheck.MsrtFileDir;
            this._dailyCheckCtrl.IsArrangeByRowCol = this.rdAutoCheck.Checked;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (this._dailyCheckCtrl.LoadMsrtFromFile(openFileDialog1.FileName, 0) != EErrorCode.NONE)
                {
                    this.btnCalc.Enabled = false;
                    btnRunCalc.Enabled = false;
                    this.lstState.Items.Add("Load Criterion File FAIL !! ");
                    return;
                }
                else
                {
                    if (this._dailyCheckCtrl.LoadMsrtFromFile(openFileDialog1.FileName, 0) != EErrorCode.NONE)
                    {
                        return;
                    }

                    DateTime createTime = File.GetLastWriteTime(openFileDialog1.FileName);
                    this.lblOperateID.Text = createTime.ToString();
                    this.btnCalc.Enabled = true;
                    this.btnRunCalc.Enabled = true;
                    this.lstState.Items.Add("Load Msrt File OK !! ");

                    TimeSpan ts = DateTime.Now - createTime;

                    if (ts.TotalHours > DataCenter._toolConfig.DCheck.TestFileOverdueHours && DataCenter._toolConfig.DCheck.TestFileOverdueHours > 0)
                    {
                        this.lstState.Items.Add("Msrt File is overdue !! ");
                    }

                    RenameMsrtFileNameByCreateTime(openFileDialog1.FileName);

                    this.btnCalc.Enabled = true;
                    btnRunCalc.Enabled = true;
                }
            }
        }

        #endregion

    }
}
