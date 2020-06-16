using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;
using System.Runtime.InteropServices;
using System.Xml;
using System.Data;

using MPI.Tester.Data;
//using MPI.Tester.Device;
using MPI.Tester.Tools;
//using MPI.Tester.DeviceCommon;
using ZedGraph;
using MPI.Windows.Forms;
using System.Linq;

namespace MPI.Tester.Gui
{
    public partial class frmChuckCorrection : Form
    {
        ChuckCorrectionCtrl _chuckCorrection;
        private bool _isOverDailyWatchSpec=false;
        private string _juadgeItemName="WATT_1";

        public frmChuckCorrection()
        {           
            InitializeComponent();  
        }

        private void frmDailyCorrection_Load(object sender, EventArgs e)
        {
            this._chuckCorrection = new ChuckCorrectionCtrl(8);
            this.LoadUserFormat(DataCenter._uiSetting.UserID, DataCenter._uiSetting.FormatName);
            this.txtRecipeName.Text = DataCenter._uiSetting.TaskSheetFileName;
            InitialControlUI();
            PlotGraph.SetGrid(this.zedDailyWatch, false, Color.Silver, Color.Transparent);
            PlotGraph.SetLabel(this.zedDailyWatch, "", "Chuck", "Percent (%) ", 24);
            this.UpdateDataToControlUI();
            this.TopMost = false;
        }

        private void InitialControlUI()
        {
            this.txtStdPathAndFile.Text = "";
            this.txtMsrtPathAndFile.Text = "";
            this.dgvCalcChuckGainOffset.Rows.Clear();
            this.dgvDailyWatchSpec.Rows.Clear();
            this.btnReturn.Enabled = true;
            tabplPath.Visible = false;
            this.tabChuckCorrect.Visible = false;
            this.zedDailyWatch.GraphPane.CurveList.Clear();
        }


        private void UpdateGainErrorToGraph()
        {
            if (this._chuckCorrection.CalcChuckIndex[0] == null)
            {
                return;
            }

            double[] x = new double[8];
            double[] y = new double[8];
            double[] yError = new double[8];
            for (int i = 0; i < 8; i++)
            {
                x[i] = i + 1;
                y[i] = (this._chuckCorrection.CalcChuckIndex[i].CalcGainOffset[0].Gain-1)*100;        
            }

            this.zedDailyWatch.GraphPane.CurveList.Clear();
            PlotGraph.SetLabel(this.zedDailyWatch, "", "Chuck", "Percent (%) ", 24);
            PlotGraph.SetXYAxis(this.zedDailyWatch, 0.5d, 8.5d, -5,6);
            this.zedDailyWatch.GraphPane.XAxis.Scale.FontSpec.Size = 24;
            this.zedDailyWatch.GraphPane.YAxis.Scale.FontSpec.Size = 24;
            PlotGraph.DrawBarItem(this.zedDailyWatch, x, y, Color.Blue);
        }

        private bool LoadUserFormat(EUserID user, string formatName)
        {
            //------------------------------------------
            // Load C:\MPI\LEDTester current format
            //------------------------------------------
            string fileNameWithExt = string.Format("{0}{1}", "User", ((int)user).ToString("0000")) + ".xml";
            string pathAndFileName = Path.Combine(Constants.Paths.USER_DIR, fileNameWithExt);

			//-----------------------------------------------------
			// (1) Set parameters of current product from UI setting
			//-----------------------------------------------------
           this. _chuckCorrection.LOPSaveItem = DataCenter._product.LOPSaveItem;
           this._chuckCorrection.CalBaseWave = DataCenter._product.TestCondition.CalByWave;
			//-----------------------------------------------------
			// (2) Load format data from files 
			//-----------------------------------------------------
            if (this._chuckCorrection.LoadCurrentFormat(pathAndFileName, formatName) == false)
            {
                MessageBox.Show("Load Current Format Error");
                return false;
            }
			//-----------------------------------------------------
			// (3) Parser title data with test items
			//-----------------------------------------------------
			DataCenter._fileCompare.ParseTitleDataIndex(DataCenter._product.TestCondition.TestItemArray);

            return true;
        }

        private void UpdateDataToControlUI()
        {
            this.txtOpenStandardPath.Text = DataCenter._toolConfig.Daily_OpenFilePath;
            this.txtSaveReportPath.Text = DataCenter._toolConfig.Daily_UpLoadFilePath;
        }

        private void UpdateSpecToUI()
        {
            this.dgvDailyWatchSpec.Rows.Clear();

            int  row=0;

              foreach (StdSpec spec in this._chuckCorrection.ItemSpec.Values)
            {
                this.dgvDailyWatchSpec.Rows.Add();
                this.dgvDailyWatchSpec[0, row].Value = row.ToString();
                this.dgvDailyWatchSpec[1, row].Value = (string)spec.Name;
                this.dgvDailyWatchSpec[2, row].Value = (string)spec.KeyName;
                this.dgvDailyWatchSpec[3, row].Value = (double)spec.DailyWatchSpec;
                this.dgvDailyWatchSpec[4, row].Value = (double)spec.ReCalibSpec;
                row++;
            }
        }

        private void UpdateChuckDataToUI()
        {
            this.dgvCalcChuckGainOffset.Rows.Clear();
             int  row=0;

             for (int i = 0; i < this._chuckCorrection.NumbersOfChuck; i++)
            {
                if (this._chuckCorrection.CalcChuckIndex[i] == null)
                {
                    continue;
                }

                if (this._chuckCorrection.CalcChuckIndex[i].MsrtData == null)
                {
                    continue;
                }

                this.dgvCalcChuckGainOffset.Rows.Add();
                this.dgvCalcChuckGainOffset[0, row].Value = this._chuckCorrection.CalcChuckIndex[i].Index.ToString();
                this.dgvCalcChuckGainOffset[1, row].Value = this._chuckCorrection.CalcChuckIndex[i].Name.ToString();
                this.dgvCalcChuckGainOffset[2, row].Value = this._chuckCorrection.CalcChuckIndex[i].CalcGainOffset[0].YArray[0];
                this.dgvCalcChuckGainOffset[3, row].Value = this._chuckCorrection.CalcChuckIndex[i].CalcGainOffset[1].YArray[0];
                this.dgvCalcChuckGainOffset[4, row].Value = this._chuckCorrection.CalcChuckIndex[i].CalcGainOffset[2].YArray[0];
                this.dgvCalcChuckGainOffset[5, row].Value = this._chuckCorrection.CalcChuckIndex[i].CalcGainOffset[0].XArray[0];
                this.dgvCalcChuckGainOffset[6, row].Value = this._chuckCorrection.CalcChuckIndex[i].CalcGainOffset[1].XArray[0];
                this.dgvCalcChuckGainOffset[7, row].Value =this._chuckCorrection.CalcChuckIndex[i].CalcGainOffset[2].XArray[0];
                //Lop
                this.dgvCalcChuckGainOffset[8, row].Value = this._chuckCorrection.CalcChuckIndex[i].CalcGainOffset[0].Gain;
                if (this._chuckCorrection.CalcChuckIndex[i].CalcGainOffset[0].EState == EDailyWacthState.ReCalibration)
                {
                    this.dgvCalcChuckGainOffset[8, row].Style.BackColor = Color.Yellow;
                }
                else if (this._chuckCorrection.CalcChuckIndex[i].CalcGainOffset[0].EState == EDailyWacthState.OverDailyWacth)
                {
                    this.dgvCalcChuckGainOffset[8, row].Style.BackColor = Color.Pink;
                }
                //VF
                this.dgvCalcChuckGainOffset[9, row].Value = this._chuckCorrection.CalcChuckIndex[i].CalcGainOffset[1].Offset;

                if (this._chuckCorrection.CalcChuckIndex[i].CalcGainOffset[1].EState == EDailyWacthState.ReCalibration)
                {
                    this.dgvCalcChuckGainOffset[9, row].Style.BackColor = Color.Yellow;
                }
                else if (this._chuckCorrection.CalcChuckIndex[i].CalcGainOffset[1].EState == EDailyWacthState.OverDailyWacth)
                {
                    this.dgvCalcChuckGainOffset[9, row].Style.BackColor = Color.Pink;
                }
                //WD
                this.dgvCalcChuckGainOffset[10, row].Value = this._chuckCorrection.CalcChuckIndex[i].CalcGainOffset[2].Offset;
                if (this._chuckCorrection.CalcChuckIndex[i].CalcGainOffset[2].EState == EDailyWacthState.ReCalibration)
                {
                    this.dgvCalcChuckGainOffset[10, row].Style.BackColor = Color.Yellow;
                }
                else if (this._chuckCorrection.CalcChuckIndex[i].CalcGainOffset[2].EState == EDailyWacthState.OverDailyWacth)
                {
                    this.dgvCalcChuckGainOffset[10, row].Style.BackColor = Color.Pink;
                }
                row++;
            }

             bool isCheckAllItem = false;

            // Check In SPEC
             for (int i = 0; i < this._chuckCorrection.NumbersOfChuck; i++)
             {
                 if (this._chuckCorrection.CalcChuckIndex[i] == null)
                 {
                     continue;
                 }

                 foreach(DailyGainOffset dailyGainOffset in this._chuckCorrection.CalcChuckIndex[i].CalcGainOffset)
                 {
                     if (!isCheckAllItem)
                     {
                         if (dailyGainOffset.KeyName != this._juadgeItemName)
                         {
                             continue;
                         }
                     }

                     if (dailyGainOffset.EState == EDailyWacthState.ReCalibration)
                     {
                         this._chuckCorrection.CalcChuckIndex[i].EState = EDailyWacthState.ReCalibration;
                         this._isOverDailyWatchSpec = true;
                     }
                 }
             }
        }

        private Dictionary<string, string> PushDataToDic()
        {
            Dictionary<string, string> dirdata = new Dictionary<string, string>();
            dirdata.Add("DateTime", DateTime.Now.ToString());
            dirdata.Add("DateTime2", DateTime.Now.ToString("yyyy-MM-dd"));
            dirdata.Add("mechineName", DataCenter._uiSetting.MachineName.ToString());
            dirdata.Add("productName", this.txtStdPathAndFile.Text);
            dirdata.Add("chipIndex", this.dinChipIndex.Value.ToString());
            dirdata.Add("operator", DataCenter._uiSetting.OperatorName.ToString());
            dirdata.Add("TesterRecipe", DataCenter._uiSetting.TaskSheetFileName);

            foreach (StdSpec spec in this._chuckCorrection.ItemSpec.Values)
            {
                dirdata.Add("DSpec_" + spec.KeyName, spec.DailyWatchSpec.ToString());
                dirdata.Add("RSpec_" + spec.KeyName, spec.ReCalibSpec.ToString());
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < this._chuckCorrection.NumbersOfChuck; i++)
            {
                if (this._chuckCorrection.CalcChuckIndex[i] == null)
                {
                    continue;
                }

                for (int k = 0; k < this._chuckCorrection.CalcChuckIndex[i].CalcGainOffset.Length; k++)
                {
                    // Gain
                    sb.Append("C_");
                    sb.Append(this._chuckCorrection.CalcChuckIndex[i].CalcGainOffset[k].KeyName);  //C_WATT_1_C1
                    sb.Append("_Gain_C");
                    sb.Append((i + 1).ToString());
                    dirdata.Add(sb.ToString(), this._chuckCorrection.CalcChuckIndex[i].CalcGainOffset[k].Gain.ToString());
                    sb.Clear();
                    // Error Percent
                    sb.Append("C_");
                    sb.Append(this._chuckCorrection.CalcChuckIndex[i].CalcGainOffset[k].KeyName);  //C_WATT_1_C1
                    sb.Append("_Error_C");
                    sb.Append((i + 1).ToString());
                    if (this._chuckCorrection.CalcChuckIndex[i].CalcGainOffset[k].Gain != 999)
                    {
                        string gainPercent = (this._chuckCorrection.CalcChuckIndex[i].CalcGainOffset[k].Gain - 1).ToString("0.0000");
                        dirdata.Add(sb.ToString(), gainPercent);
                        sb.Clear();
                    }
                    else
                    {
                        string gainPercent = "0.0000";
                        dirdata.Add(sb.ToString(), gainPercent);
                        sb.Clear();
                    }
                    // Offset
                    sb.Append("C_");
                    sb.Append(this._chuckCorrection.CalcChuckIndex[i].CalcGainOffset[k].KeyName);  //C_WATT_1_Gain_C1
                    sb.Append("_Offset_C");
                    sb.Append((i + 1).ToString());
                    dirdata.Add(sb.ToString(), this._chuckCorrection.CalcChuckIndex[i].CalcGainOffset[k].Offset.ToString());
                    sb.Clear();
                }
            }

            sb.Clear();

            string[] exportKeyName = new string[8] { "MVF_1", "MVF_2", "MVF_3", "MVF_4", "MIR_1", "WLP_1" ,"WATT_1","WLD_1"};

            exportKeyName = this._chuckCorrection.TitleName.Keys.ToArray();

            for (int i = 0; i < this._chuckCorrection.CalcChuckIndex.Length; i++)
            {
                if (this._chuckCorrection.CalcChuckIndex[i] == null)
                {
                    continue;
                }
                
                DataRow msrtData = this._chuckCorrection.CalcChuckIndex[i].MsrtData;
                DataRow stdData = this._chuckCorrection.CalcChuckIndex[i].StdData;

                if (this._chuckCorrection.CalcChuckIndex[i].EState == EDailyWacthState.ReCalibration)
                {
                    dirdata.Add("Chuck_" + (i + 1).ToString(), "NG");
                }
                else
                {
                    dirdata.Add("Chuck_" + (i + 1).ToString(), "PASS");
                }

                for (int k = 0; k < exportKeyName.Length; k++)
                {
                    if (this._chuckCorrection.TitleIndex.ContainsKey(exportKeyName[k]))
                    {
                        string key = exportKeyName[k];
                        sb.Clear();
                        sb.Append("S_");
                        sb.Append(exportKeyName[k]);  //S_WATT_1_C1
                        sb.Append("_C");
                        sb.Append((i + 1).ToString());
                        dirdata.Add(sb.ToString(), stdData[this._chuckCorrection.TitleIndex[key] + 1].ToString());
                        sb.Clear();
                        sb.Append("T_");
                        sb.Append(exportKeyName[k]);  //S_WATT_1_
                        sb.Append("_C");
                        sb.Append((i + 1).ToString());
                        dirdata.Add(sb.ToString(), msrtData[this._chuckCorrection.TitleIndex[key] + 1].ToString());
                    }
                }
            }
            return dirdata;
        }

        private void SaveChuckCheckReort()
        {
            Dictionary<string, string> dirdata = PushDataToDic();
            string formatFileName = "DReport-8022.drf";
            string formatFileAndPath=Path.Combine(Constants.Paths.USER_DIR,formatFileName);
            string saveFileName = DataCenter._uiSetting.MachineName + "calcheck.txt";
            string saveFilenameAndPath = Path.Combine(DataCenter._toolConfig.Daily_UpLoadFilePath, saveFileName);
            CSVUtil.ConverterUesrReport(formatFileAndPath, saveFilenameAndPath, (char)0x01, (char)0x02, dirdata, "");
        }

        private void SaveChuckGainAndOffset()
        {
            if (DataCenter._product.ChuckLOPCorrectArray == null)
                return;
            // row=chuck Index
            for (int row = 0; row < this._chuckCorrection.NumbersOfChuck; row++)
            {
                GainOffsetData[] data = DataCenter._product.ChuckLOPCorrectArray[row];
                ChuckIndexState calcChuck = this._chuckCorrection.CalcChuckIndex[row];
                if (data == null)
                    continue;
                if (calcChuck == null)
                {
                    continue;
                }

                DailyGainOffset dailyData = calcChuck.CalcGainOffset[0];

                if (dailyData.ISAutoCalibData == true && dailyData.EState==EDailyWacthState.OverDailyWacth)
                {
                    string keyname= dailyData.KeyName.Remove(dailyData.KeyName.IndexOf("_"));

                    double factor=0.0d;
                    double ifactor = 0.0d;

                    switch(keyname)
                    {
                        case "LOP" :
                            factor=dailyData.Gain;
                            factor=Math.Round(factor,2);
                            data[0].Gain = (data[0].Gain * factor);
                            data[0].Offset = (data[0].Offset + dailyData.Offset);
                            break;
                        case "WATT":
                            factor=dailyData.Gain;
                            ifactor = Math.Floor(100 * factor)*0.01;
                            data[1].Gain = (data[1].Gain * factor);
                            data[1].Offset = (data[1].Offset + dailyData.Offset);
                            break;  
                        default:
                            break;
                      }                     
                }     
            }
        }

        private void btnLoadStdFile_Click(object sender, EventArgs e)
        {
             OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //openFileDialog1.Filter = "CSV文件|*.CSV";
            openFileDialog1.InitialDirectory = this.txtOpenStandardPath.Text;
            openFileDialog1.FileName = "";
            openFileDialog1.FilterIndex = 1;    // default value = 1
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (this._chuckCorrection.LoadDailyWatchStdFile(openFileDialog1.FileName) == false)
                {
                    Host.SetErrorCode(EErrorCode.Tools_LoadStdDataFail);
                    return;
                }

                if (this._chuckCorrection.StdTable == null)
                {
                    Host.SetErrorCode(EErrorCode.Tools_LoadStdDataFail);              
                    return;
                }
                this.txtStdPathAndFile.Text = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                this.dinChipIndex.MaxValue = this._chuckCorrection.StdTable.Rows.Count;
                UpdateSpecToUI();
            }
        }

        private void btnLoadMsrtFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = this.txtOpenStandardPath.Text;
            openFileDialog1.FileName = "";
            openFileDialog1.FilterIndex = 1;    // default value = 1
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this._chuckCorrection.LoadDailyWatchMsrtFile(openFileDialog1.FileName);
                this.txtMsrtPathAndFile.Text = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
            }
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {   
              int index = (int)(this.dinChipIndex.Value-1);
              this._isOverDailyWatchSpec = false;

              if (this._chuckCorrection.CalcChuckFactor(index))
              {
                  this.UpdateChuckDataToUI();
                  this.UpdateGainErrorToGraph();
              }
              else
              {
                  DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show(" 請確認載入資料正確性 ", " 每日校正結果 ", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                  return;
              }

            if (_isOverDailyWatchSpec == true)
            {
                this.btnReturn.Enabled = false;
                DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show("超出每日校正SPEC，請工程師確認 !!! ", " 每日校正結果 ", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else
            {
                this.btnReturn.Enabled = true;
                DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show("確認是否更改校正係數，並輸出報表 !! ", " 每日校正結果 ", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (result != DialogResult.OK)
                {
                    return;
                }
                this.SaveChuckGainAndOffset();
                this.SaveChuckCheckReort();
                Host.UpdateDataToAllUIForm();
                this.Close();
            }
        }

        private void btnSaveCoeffToSystem_Click(object sender, EventArgs e)
        {
          
        }

        private void btnOpenFilePath_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("");
            if (path != string.Empty)
            {
                this.txtOpenStandardPath.Text = path;         
            }
        }

        private string SelectPath(string title)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = title;
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                return folderBrowserDialog.SelectedPath;
            }
            else
            {
                return string.Empty;
            }

        }

        private void btnSaveDailyReportPath_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("");
            if (path != string.Empty)
            {
                this.txtSaveReportPath.Text = path;
                //DataCenter._toolConfig.Daily_UpLoadFilePath = path;
            }
        }

        private void btnSettingConfirm_Click(object sender, EventArgs e)
        {
            DataCenter._toolConfig.Daily_OpenFilePath = this.txtOpenStandardPath.Text;
            DataCenter._toolConfig.Daily_UpLoadFilePath = this.txtSaveReportPath.Text;
            DataCenter.SaveToolsConfig();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.btnReturn.Enabled = false;
            this.Close();    
        }

        private void btnUnLack_Click(object sender, EventArgs e)
        {
            if (this.txtPassWord.Text == "5551771" || this.txtPassWord.Text == "g" )
            {
                this.btnReturn.Enabled = true;
                this.txtPassWord.Text = "";
                tabplPath.Visible = true;
                this.tabChuckCorrect.Visible = true;
            }
            else
            {
                this.btnReturn.Enabled = false;
                this.txtPassWord.Text = "";
                tabplPath.Visible = false;
                this.tabChuckCorrect.Visible = false;
            }
        }

    }
}
