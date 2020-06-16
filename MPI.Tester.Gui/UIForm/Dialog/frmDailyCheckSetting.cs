using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MPI.Tester.Data;
using System.IO;
using System.Xml;
using MPI.Tester.Tools;

namespace MPI.Tester.Gui
{
    public partial class frmDailyCheckSetting : Form
    {
        private DailyCheckSpecInfo _dailySpecInfo = new DailyCheckSpecInfo();
        private DcSpecRecipeManger _dailyRecipeManger = new DcSpecRecipeManger();

        private bool _isShowDailyRecipeSetting;


        public frmDailyCheckSetting()
        {
            InitializeComponent();
            this.cmbCirteriorBy.Items.Clear();
            this.cmbCirteriorBy.Items.AddRange(Enum.GetNames(typeof(EDailyCheckSpecBy)));

            _isShowDailyRecipeSetting = false;
        }

        private void UpdateDirFile()
        {
            cmbFileName.Items.Clear();

            if (DataCenter._toolConfig.DCheck.CriterionBy == EDailyCheckSpecBy.TestFileName)
            {
                cmbFileName.Items.AddRange(DataCenter.GetAllFilesList(DataCenter._toolConfig.DCheck.StdFileDir, DataCenter._uiSetting.TestResultFileExt));
            }
            else
            {
                cmbFileName.Items.AddRange(DataCenter.GetAllFilesList(DataCenter._uiSetting.ProductPath, Constants.Files.PRODUCT_FILE_EXTENSION));
                this.cmbFileName.SelectedItem =DataCenter._uiSetting.TaskSheetFileName ;
                this.cmbFileName.Enabled = false;
                string[] aa=DataCenter.GetAllFilesList(DataCenter._uiSetting.ProductPath, Constants.Files.PRODUCT_FILE_EXTENSION);

               _dailyRecipeManger.Create(DataCenter._uiSetting.TaskSheetFileName,aa, _dailySpecInfo.TheOtherAcceptRecipes);
                this.updateRecipeInfo();
            }

            this.cmbCompareRecipe.Items.Clear();
            cmbCompareRecipe.Items.AddRange(DataCenter.GetAllFilesList(Constants.Paths.TOOLS_DC_DIR, "xml"));
        }

        private void UpdateDataToControls()
        {
            this.txtConditionPath.Text = DataCenter._toolConfig.DCheck.CriterionFileAndPath;
            this.txtStdFileDir.Text = DataCenter._toolConfig.DCheck.StdFileDir;
            this.txtSaveReportPath.Text = DataCenter._toolConfig.DCheck.UploadDir;
            this.chkIsShowAllItem.Checked = DataCenter._toolConfig.DCheck.IsShowAllResultItem;
            this.txtSaveReportPath.Text = DataCenter._toolConfig.DCheck.OutputFileDir;
            this.txtSaveReportPath2.Text = DataCenter._toolConfig.DCheck.OutputFileDir2;
            this.cmbAutoLevel.SelectedIndex = DataCenter._toolConfig.DCheck.AutoRunLevel;
            this.chkIsEnableSaveFileByMechineName.Checked = DataCenter._toolConfig.DCheck.IsCrateMachimeNameFolderToSaveFile;
            this.txtMsrFileDir.Text = DataCenter._toolConfig.DCheck.MsrtFileDir;
            this.chkIsCheckEveryDieInSpec.Checked = DataCenter._toolConfig.DCheck.IsCheckEveryDieInSpec;
            this.txtRecipeName.Text = DataCenter._uiSetting.TaskSheetFileName;
            this.chkIsFilterOutput2Data.Checked = DataCenter._toolConfig.DCheck.IsFilterOutputFile2;
            this.chkIsEnableReNameMsrtFile.Checked = DataCenter._toolConfig.DCheck.IsRenameMsrtFileByCreateTime;

            this.txtOuputFileNameRule.Text = DataCenter._uiSetting.UserDefinedData.DailyCheckingSaveReportName;


            if (DataCenter._toolConfig.DCheck.TestFileOverdueHours > 0)
            {
                this.chkCheckTestFileIsOverDue.Checked = true;
            }
            else
            {
                this.chkCheckTestFileIsOverDue.Checked = false;
            }
            
            this.numCheckTestFileOverDue.Value = DataCenter._toolConfig.DCheck.TestFileOverdueHours;

            if (DataCenter._toolConfig.DCheck.CriterionBy == EDailyCheckSpecBy.RECIPE)
            {
                this.lblSelctSpeItem.Text = "Recipe Name";
            }
            else
            {
                this.lblSelctSpeItem.Text = "Test File Name";
            }
          
            this.cmbCirteriorBy.SelectedIndex = (int)DataCenter._toolConfig.DCheck.CriterionBy;
            this.txtDailyCheckResultDir.Text = DataCenter._toolConfig.DCheck.SaveResultLogDir;
            this.chkIsEnableCheckValid.Checked = DataCenter._uiSetting.IsEnableCheckDailyDataIsValid;

            List<int> times = DataCenter._uiSetting.DailyCheckTime;

            if (times.Count <= 4)
            {
                this.numDailyCheckTime1.Value = -1;
                this.numDailyCheckTime2.Value = -1;
                this.numDailyCheckTime3.Value = -1;
                this.numDailyCheckTime4.Value = -1;

                switch (times.Count)
                {
                    case 0:
                        break;
                    case 1:
                        this.numDailyCheckTime1.Value = times[0];
                        break;
                    case 2:
                        this.numDailyCheckTime1.Value = times[0];
                        this.numDailyCheckTime2.Value = times[1];
                        break;
                    case 3:
                        this.numDailyCheckTime1.Value = times[0];
                        this.numDailyCheckTime2.Value = times[1];
                        this.numDailyCheckTime3.Value = times[2];
                        break;
                    case 4:
                        this.numDailyCheckTime1.Value = times[0];
                        this.numDailyCheckTime2.Value = times[1];
                        this.numDailyCheckTime3.Value = times[2];
                        this.numDailyCheckTime4.Value = times[3];
                        break;
                }
            }

            this.UpdteDailyCheckInfoToUI();
            this.updateRecipeInfo();

            if (DataCenter._userManag.CurrentAuthority == EAuthority.Operator)
            {
                this.tabcChartInfo.Enabled = false;
            }
            else
            {
                this.tabcChartInfo.Enabled = true;
            }
        }

        private bool OpenDailyCheckSpec(string pathAndFile)
        {
            if (File.Exists(pathAndFile) == false)
            {
               // string name = Path.GetFileNameWithoutExtension(fileNameAndPath);
              //  fileNameAndPath = Path.Combine(Constants.Paths.TOOLS_DC_DIR, name+".xml"); // XML File
                DialogResult result = TopMessageBox.ShowMessage(201, "Would you Reset Gain / Offset Table?", "Setting Spec !!");
                CreateDefaultRecipe(pathAndFile);
                return false;
            }
            else
            {
                this._dailySpecInfo = MPI.Xml.XmlFileSerializer.Deserialize(typeof(DailyCheckSpecInfo), pathAndFile) as DailyCheckSpecInfo;

                if (this._dailySpecInfo == null)
                    return false;
                
                Dictionary<string, string> title = DataCenter._uiSetting.UserDefinedData.ResultItemNameDic;
                this._dailySpecInfo.PushData(title);
                return true;
            }
        }

        private void UpdteDailyCheckInfoToUI()
        {
            this.txtRecipeName.Text = _dailySpecInfo.RecipeName;
            this.numToleranceOutOfSpec.Value = _dailySpecInfo.ToleranceOutSpecCount;
            this.numAlreadyOutOfSpec.Value = _dailySpecInfo.AlreadyOutSpecCount;
            this.numMinCountAccept.Value = _dailySpecInfo.MinCountAccept;

            this._dailyRecipeManger.RenewTarget(_dailySpecInfo.TheOtherAcceptRecipes);
            //
            this.dgvDailyWatchSpec.Rows.Clear();
            int row = 0;

            foreach (DailyWatchSpec spec in this._dailySpecInfo.Data.Values)
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
                this.dgvDailyWatchSpec[8, row].Value = (int)spec.DataColIndex;
                this.dgvDailyWatchSpec[9, row].Value = (double)spec.EverDieLowerSpec;
                this.dgvDailyWatchSpec[10, row].Value = (double)spec.EverDieHighSpec;
                this.dgvDailyWatchSpec[11, row].Value = spec.CriteriaUnit;
                this.dgvDailyWatchSpec[13, row].Value = (bool)spec.IsEnable;
                row++;
            }
        }

        private void CreateDefaultRecipe(string fileName)
        {
            TestItemData[] testItemArray = DataCenter._product.TestCondition.TestItemArray;

            if (testItemArray == null)
            {
                return;
            }

              this._dailySpecInfo = new DailyCheckSpecInfo();

              foreach (var testItem in testItemArray)
              {
                  if (!testItem.IsEnable || testItem.MsrtResult == null || testItem.MsrtResult.Length == 0)
                      continue;

                  TestResultData[] resultItemArray = testItem.MsrtResult;

                  foreach (var resultItem in resultItemArray)
                  {
                      if (!resultItem.IsEnable || !resultItem.IsVision)
                          continue;

                      string key = resultItem.KeyName;
                      DailyWatchSpec dwc = new DailyWatchSpec(key, resultItem.Name);
                      string str = key.Remove(key.IndexOf("_"));
                      string strNum = key.Substring(key.IndexOf("_") + 1);

                      switch (str)
                      {
                          case "MVF":
                          case "MVFLA":
                              dwc.MaxValue = resultItem.MaxLimitValue;
                              dwc.MinValue = resultItem.MinLimitValue;
                              dwc.ReCalibSpec = 0.03;
                              dwc.EverDieHighSpec = 0.03;
                              dwc.EverDieLowerSpec = -0.03;
                              dwc.CriteriaType = 1;
                              dwc.IsEnbaleFilter = true;
                              this._dailySpecInfo.Data.Add(key, dwc);
                              dwc.CriteriaUnit = "V";
                              dwc.IsEnable = true;

                            //  if (strNum == "1" || strNum == "2")
                           //   {
                                  dwc.IsEnable = true;
                                  dwc.IsEnbaleFilter = false;
                                  dwc.CriteriaType = 1;
                          //    }
                              //else
                              //{
                              //    dwc.IsEnable = false;
                              //    dwc.IsEnbaleFilter = false;
                              //    dwc.CriteriaType = 0;
                              //}
                              continue;
                          //---------------------------------------------------------
                          case "MIR":
                              dwc.MaxValue = resultItem.MaxLimitValue;
                              dwc.MinValue = resultItem.MinLimitValue;
                              dwc.CriteriaType = 0;
                              dwc.IsEnbaleFilter = resultItem.IsVerify;
                              dwc.IsEnable = true;
                              dwc.CriteriaUnit = "uA";
                              this._dailySpecInfo.Data.Add(key, dwc);
                              continue;
                          //---------------------------------------------------------
                          case "MVZ":
                              dwc.MaxValue = resultItem.MaxLimitValue;
                              dwc.MinValue = resultItem.MinLimitValue;
                              dwc.CriteriaType = 0;
                              dwc.IsEnbaleFilter = resultItem.IsVerify;
                              dwc.IsEnable = true;
                              this._dailySpecInfo.Data.Add(key, dwc);
                              continue;
                          //---------------------------------------------------------
                          case "WLP":
                          case "WLD":
                              dwc.MaxValue = 780;
                              dwc.MinValue = 380;
                              dwc.CriteriaType = 1;
                              dwc.IsEnbaleFilter = true;
                              dwc.ReCalibSpec = 0.3;
                              dwc.EverDieHighSpec = 0.3;
                              dwc.EverDieLowerSpec = -0.3;
                              dwc.IsEnable = true;
                               dwc.IsEnbaleFilter = true;
                               dwc.CriteriaType = 1;
                               dwc.CriteriaUnit = "nm";
                              this._dailySpecInfo.Data.Add(key, dwc);
                              continue;
                          //---------------------------------------------------------
                          case "WATT":
                              dwc.MaxValue = resultItem.MaxLimitValue;
                              dwc.MinValue = resultItem.MinLimitValue;
                              dwc.CriteriaType = 2;
                              dwc.IsEnbaleFilter = true;
                              dwc.ReCalibSpec = 3;
                              dwc.EverDieHighSpec = 3;
                              dwc.EverDieLowerSpec = -3;
                              dwc.CriteriaUnit = "%";
                              //dwc.IsEnable = true;
                              //dwc.IsEnbaleFilter = false;
                              //dwc.CriteriaType = 2;                            

                              if ( DataCenter._product.LOPSaveItem.ToString().Contains("watt"))
                              {
                                  dwc.Name = resultItem.Name + "(mw)";
                                  dwc.IsEnable = true;
                                  dwc.IsEnbaleFilter = false;
                                  dwc.CriteriaType = 2;
                                  this._dailySpecInfo.Data.Add(key, dwc);
                              }

                              break;
                          //---------------------------------------------------------
                          case "LOP":
                              dwc.MaxValue = 9999;
                              dwc.MinValue = 0;
                              dwc.ReCalibSpec = 3;
                              dwc.EverDieHighSpec = 3;
                              dwc.EverDieLowerSpec = -3;
                              dwc.CriteriaUnit = "%";

                              if (DataCenter._product.LOPSaveItem.ToString().Contains("mcd"))
                              {
                                  dwc.Name = resultItem.Name + "(mcd)";
                                  dwc.IsEnable = true;
                                  dwc.IsEnbaleFilter = false;
                                  dwc.CriteriaType = 2;
                                  this._dailySpecInfo.Data.Add(key, dwc);
                              }
                              continue;
                          //---------------------------------------------------------
                          default:
                              continue;

                      }
                  }
              }

            //this._dailySpecInfo = new DailyCheckSpecInfo();

            //foreach (KeyValuePair<string, string> kvp in DataCenter._uiSetting.UserDefinedData.ResultItemNameDic)
            //{
            //    string key = kvp.Key;

            //    if (key.IndexOf("_") >= 0)
            //    {
            //        DailyWatchSpec dwc = new DailyWatchSpec(kvp.Key,kvp.Value);
            //        string str = key.Remove(key.IndexOf("_"));
            //        string strNum = key.Substring(key.IndexOf("_") + 1);

            //        switch (str)
            //        {
            //            case "MVF":
            //            case "MVFLA":
            //                dwc.MaxValue = 8;
            //                dwc.MinValue = 0;
            //                dwc.ReCalibSpec = 0.03;
            //                dwc.EverDieHighSpec = 0.03;
            //                dwc.EverDieLowerSpec = -0.03;
            //                dwc.CriteriaType = 1;
            //                dwc.IsEnbaleFilter = true;
            //                this._dailySpecInfo.Data.Add(kvp.Key, dwc);
            //                dwc.IsEnable = true;

            //                if (strNum == "1" || strNum == "2")
            //                {
            //                    dwc.IsEnable = true;
            //                    dwc.IsEnbaleFilter = true;
            //                    dwc.CriteriaType = 1;
            //                }
            //                else
            //                {
            //                    dwc.IsEnable = false;
            //                    dwc.IsEnbaleFilter = false;
            //                    dwc.CriteriaType = 0;
            //                }
            //                continue;
            //            //---------------------------------------------------------
            //            case "MIR":
            //                dwc.MaxValue = 1;
            //                dwc.MinValue = 0;
            //                dwc.CriteriaType = 0;
            //                dwc.IsEnbaleFilter = true;
            //                this._dailySpecInfo.Data.Add(kvp.Key, dwc);
            //                dwc.IsEnable = false;

            //                if (strNum == "1" || strNum == "2")
            //                {
            //                    dwc.IsEnbaleFilter = true;
            //                }
            //                else
            //                {
            //                    dwc.IsEnbaleFilter = false;
            //                }


            //                continue;
            //            //---------------------------------------------------------
            //            case "WLP":
            //            case "WLD":
            //                dwc.MaxValue = 780;
            //                dwc.MinValue = 380;
            //                dwc.CriteriaType = 1;
            //                dwc.IsEnbaleFilter = true;
            //                dwc.ReCalibSpec = 0.1;
            //                dwc.EverDieHighSpec = 0.3;
            //                dwc.EverDieLowerSpec = -0.3;
            //                this._dailySpecInfo.Data.Add(kvp.Key, dwc);

            //                if (strNum == "1")
            //                {
            //                    dwc.IsEnable = true;
            //                    dwc.IsEnbaleFilter = true;
            //                    dwc.CriteriaType = 1;
            //                }
            //                else
            //                {
            //                    dwc.IsEnable = false;
            //                    dwc.IsEnbaleFilter = false;
            //                    dwc.CriteriaType = 0;
            //                }
            //                continue;
            //            //---------------------------------------------------------
            //            case "WATT":
            //                dwc.MaxValue = 999;
            //                dwc.MinValue = 0;
            //                dwc.CriteriaType = 2;
            //                dwc.IsEnbaleFilter = true;
            //                dwc.ReCalibSpec = 3;
            //                dwc.EverDieHighSpec = 3;
            //                dwc.EverDieLowerSpec = -3;
            //                this._dailySpecInfo.Data.Add(kvp.Key, dwc);

            //                if (strNum == "1" && DataCenter._product.LOPSaveItem.ToString().Contains("watt"))
            //                {
            //                    dwc.IsEnable = true;
            //                    dwc.IsEnbaleFilter = true;
            //                    dwc.CriteriaType = 2;
            //                }
            //                else
            //                {
            //                    dwc.IsEnable = false;
            //                    dwc.IsEnbaleFilter = false;
            //                    dwc.CriteriaType = 0;
            //                }
            //                break;
            //            //---------------------------------------------------------
            //            case "LOP":
            //                dwc.MaxValue = 999;
            //                dwc.MinValue = 0;
            //                dwc.CriteriaType = 2;
            //                dwc.IsEnbaleFilter = true;
            //                dwc.ReCalibSpec = 3;
            //                dwc.EverDieHighSpec = 3;
            //                dwc.EverDieLowerSpec = -3;
            //                this._dailySpecInfo.Data.Add(kvp.Key, dwc);

            //                if (strNum == "1" &&  DataCenter._product.LOPSaveItem.ToString().Contains("watt"))
            //                {
            //                    dwc.IsEnable = true;
            //                    dwc.IsEnbaleFilter = true;
            //                    dwc.CriteriaType = 2;
            //                }
            //                else
            //                {
            //                    dwc.IsEnable = false;
            //                    dwc.IsEnbaleFilter = false;
            //                    dwc.CriteriaType = 0;
            //                }
            //                continue;
            //            //---------------------------------------------------------
            //            default:
            //                continue;

            //        }
            //    }
            //}

            MPI.Xml.XmlFileSerializer.Serialize(this._dailySpecInfo, fileName);

        }

        private void SaveDailyCheckSpec(string pathAndFile)
        {
            if (this.dgvDailyWatchSpec.Rows.Count == 0)
            {
                return;
            }

            for (int row = 0; row < this.dgvDailyWatchSpec.Rows.Count; row++)
            {
                string key = this.dgvDailyWatchSpec[2, row].Value.ToString();

                if (_dailySpecInfo.Data.ContainsKey(key))
                {
                    DailyWatchSpec spec = _dailySpecInfo.Data[key];

                    spec.CriteriaType = Convert.ToInt16(this.dgvDailyWatchSpec[0, row].Value);
                    spec.Name = this.dgvDailyWatchSpec[1, row].Value.ToString();
                    spec.KeyName = this.dgvDailyWatchSpec[2, row].Value.ToString();
                    spec.IsEnbaleFilter = Convert.ToBoolean(this.dgvDailyWatchSpec[3, row].Value);
                    spec.MinValue = Convert.ToDouble(this.dgvDailyWatchSpec[4, row].Value);
                    spec.MaxValue = Convert.ToDouble(this.dgvDailyWatchSpec[5, row].Value);
                    spec.DailyWatchSpec = Convert.ToDouble(this.dgvDailyWatchSpec[6, row].Value);
                    spec.ReCalibSpec = Convert.ToDouble(this.dgvDailyWatchSpec[7, row].Value);
                    spec.DataColIndex = Convert.ToInt16(this.dgvDailyWatchSpec[8, row].Value);
                    spec.EverDieLowerSpec = Convert.ToDouble(this.dgvDailyWatchSpec[9, row].Value);
                    spec.EverDieHighSpec = Convert.ToDouble(this.dgvDailyWatchSpec[10, row].Value);
                    spec.IsEnable= Convert.ToBoolean(this.dgvDailyWatchSpec[13, row].Value);
                }
            }

            _dailySpecInfo.RecipeName = this.txtRecipeName.Text;
            _dailySpecInfo.ToleranceOutSpecCount = this.numToleranceOutOfSpec.Value;
            _dailySpecInfo.AlreadyOutSpecCount = numAlreadyOutOfSpec.Value;
            _dailySpecInfo.MinCountAccept = numMinCountAccept.Value;
            _dailySpecInfo.TheOtherAcceptRecipes = this._dailyRecipeManger.TargetRecipes;

            MPI.Xml.XmlFileSerializer.Serialize(this._dailySpecInfo, pathAndFile);
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (this.cmbFileName.SelectedIndex >= 0)
            {
                string pathAndFile = string.Empty;

                if (DataCenter._toolConfig.DCheck.CriterionBy == EDailyCheckSpecBy.TestFileName)
                {
                    pathAndFile = Path.Combine(Constants.Paths.TOOLS_DC_FILE_SPEC_DIR, cmbFileName.SelectedItem.ToString() + ".xml");
                }
                else
                {
                    pathAndFile = Path.Combine(Constants.Paths.TOOLS_DC_DIR, cmbFileName.SelectedItem.ToString() + ".xml");
                }

                SaveDailyCheckSpec(pathAndFile);
                this.OpenDailyCheckSpec(pathAndFile);
              
            }

            DataCenter._toolConfig.DCheck.CriterionFileAndPath = this.txtConditionPath.Text;
            DataCenter._toolConfig.DCheck.StdFileDir = this.txtStdFileDir.Text;
            DataCenter._toolConfig.DCheck.IsShowAllResultItem = this.chkIsShowAllItem.Checked;
            DataCenter._toolConfig.DCheck.OutputFileDir = this.txtSaveReportPath.Text;
            DataCenter._toolConfig.DCheck.OutputFileDir2 = this.txtSaveReportPath2.Text;
            DataCenter._toolConfig.DCheck.UploadDir = this.txtSaveReportPath.Text;
            DataCenter._toolConfig.DCheck.AutoRunLevel = this.cmbAutoLevel.SelectedIndex;
            DataCenter._toolConfig.DCheck.IsCrateMachimeNameFolderToSaveFile = this.chkIsEnableSaveFileByMechineName.Checked;
            DataCenter._toolConfig.DCheck.MsrtFileDir = this.txtMsrFileDir.Text;
            DataCenter._toolConfig.DCheck.IsCheckEveryDieInSpec = this.chkIsCheckEveryDieInSpec.Checked;
            DataCenter._toolConfig.DCheck.SaveReportRule = this.txtOuputFileNameRule.Text;
            DataCenter._toolConfig.DCheck.CriterionBy = (EDailyCheckSpecBy)Enum.Parse(typeof(EDailyCheckSpecBy), this.cmbCirteriorBy.SelectedItem.ToString(), true);

            DataCenter._toolConfig.DCheck.SaveResultLogDir= this.txtDailyCheckResultDir.Text;
            DataCenter._uiSetting.IsEnableCheckDailyDataIsValid = chkIsEnableCheckValid.Checked;

            DataCenter._toolConfig.DCheck.IsFilterOutputFile2 = this.chkIsFilterOutput2Data.Checked;
            DataCenter._toolConfig.DCheck.TestFileOverdueHours = this.numCheckTestFileOverDue.Value;
            DataCenter._toolConfig.DCheck.IsRenameMsrtFileByCreateTime = this.chkIsEnableReNameMsrtFile.Checked;

            List<int> times = new List<int>();

            if (this.numDailyCheckTime1.Value >= 0)
            {
                if (!times.Contains(this.numDailyCheckTime1.Value))
                times.Add(this.numDailyCheckTime1.Value);
            }

            if (this.numDailyCheckTime2.Value >= 0)
            {
                if (!times.Contains(this.numDailyCheckTime2.Value))
                times.Add(this.numDailyCheckTime2.Value);
            }

            if (this.numDailyCheckTime3.Value >= 0)
            {
                if (!times.Contains(this.numDailyCheckTime3.Value))
                times.Add(this.numDailyCheckTime3.Value);
            }

            if (this.numDailyCheckTime4.Value >= 0)
            {
                if (!times.Contains(this.numDailyCheckTime4.Value))
                times.Add(this.numDailyCheckTime4.Value);
            }

            times.Sort();

            DataCenter._uiSetting.DailyCheckTime = times;

            if (this.rdAutoCheck.Checked)
            {
                DataCenter._toolConfig.DCheck.IsArrangeByRowCol = true;
            }
            else
            {
                DataCenter._toolConfig.DCheck.IsArrangeByRowCol = false;
            }

            DataCenter.SaveToolsConfig();
            DataCenter.SaveUISettingToFile();

            this.UpdateDataToControls();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDailyCheckSetting_Load(object sender, EventArgs e)
        {
            //this.Text = DataCenter._uiSetting.TaskSheetFileName;

            switch (DataCenter._uiSetting.UserDefinedData.DailyCheckControlsMode)
            {
                case 0 :
                    this.tabiSpecSetting.Visible = false;
                    this.tabiRecipeLink.Visible = false;
                    break;
                case 1:
                    this.tabiSpecSetting.Visible = true;
                    this.tabiRecipeLink.Visible = true;
                    break;
                default:
                    this.tabiSpecSetting.Visible = false;
                    this.tabiRecipeLink.Visible = false;
                    break;
            }

            if (this._isShowDailyRecipeSetting)
            {
                this.tabcChartInfo.SelectedTabIndex = 1;
                this.tabiSpecSetting.Visible = true;
                this.tabiDailySetting.Visible = false;
            }
            else
            {
                this.tabcChartInfo.SelectedTabIndex = 0;
                this.tabiSpecSetting.Visible = false;
                this.tabiDailySetting.Visible = true;
            }

            tabControlPanel2.Visible = false;

            this.TopMost = true;
            this.TopMost = false;
            UpdateDirFile();
            UpdateDataToControls();
        }

        private void btnSetCriterionFileName_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.FileName = "";
            openFileDialog1.FilterIndex = 1;    // default value = 1
            openFileDialog1.Multiselect = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txtConditionPath.Text = openFileDialog1.FileName;
            }
        }

        private void btnOpenFilePath_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("");
            if (path != string.Empty)
            {
                this.txtStdFileDir.Text = path;
            }
        }

        private void btnMsrtFilePath_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("");
            if (path != string.Empty)
            {
                this.txtMsrFileDir.Text = path;
            }
        }

        private void btnSaveDailyReportPath_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("");
            if (path != string.Empty)
            {
                this.txtSaveReportPath.Text = path;
            }
        }

        private void btnSaveDailyReportPath2_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("");
            if (path != string.Empty)
            {
                this.txtSaveReportPath2.Text = path;
            }
        }

        private string SelectPath(string title)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = title;
            folderBrowserDialog.SelectedPath = DataCenter._toolConfig.DCheck.StdFileDir;
         //   folderBrowserDialog.RootFolder = Environment.SpecialFolder.us;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                return folderBrowserDialog.SelectedPath;
            }
            else
            {
                return string.Empty;
            }

        }

        private void cmbFileName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.cmbFileName.SelectedIndex<0)
            return;

            string pathAndFile=string.Empty;

             if (DataCenter._toolConfig.DCheck.CriterionBy == EDailyCheckSpecBy.TestFileName)
             {
                  pathAndFile = Path.Combine(Constants.Paths.TOOLS_DC_FILE_SPEC_DIR, cmbFileName.SelectedItem.ToString() + ".xml");

                 if (File.Exists(pathAndFile))
                 {
                     this.OpenDailyCheckSpec(pathAndFile);
                     this.cmbCompareRecipe.Visible = false;
                 }
                 else
                 {
                     this.cmbCompareRecipe.Visible = true;
                 }
             }
             else
             {
                 pathAndFile = Path.Combine(Constants.Paths.TOOLS_DC_DIR, cmbFileName.SelectedItem.ToString() + ".xml");
                 this.OpenDailyCheckSpec(pathAndFile);
                 this.cmbCompareRecipe.Visible = false;
             }

             this.UpdteDailyCheckInfoToUI();
        }

        private void tabControlPanel1_Click(object sender, EventArgs e)
        {
        
        }

        private void btnSetCalibrateFactorPath_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("");
            if (path != string.Empty)
            {
                this.txtDailyCheckResultDir.Text = path;
            }
        }

        private void cmbCirteriorBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DataCenter._toolConfig.DCheck.CriterionBy = (EDailyCheckSpecBy)Enum.Parse(typeof(EDailyCheckSpecBy), this.cmbCirteriorBy.SelectedItem.ToString(), true);
            DataCenter._toolConfig.DCheck.CriterionBy = EDailyCheckSpecBy.RECIPE;
            this.UpdateDirFile();
        }

        private void cmbAutoLevel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabControlPanel1_VisibleChanged(object sender, EventArgs e)
        {
            if (this.cmbFileName.SelectedItem == null)
            {
                this.lblSelectFilName.Text = "NO SELECT";
            }
            else
            {

                this.lblSelectFilName.Text = this.cmbFileName.SelectedItem.ToString();
            }
        }

        private void cmbCompareRecipe_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbCompareRecipe.SelectedIndex < 0)
                return;

            string pathAndFile = Path.Combine(Constants.Paths.TOOLS_DC_DIR, cmbCompareRecipe.SelectedItem.ToString() + ".xml");

            if (!File.Exists(pathAndFile))
            {
                MessageBox.Show("Data Is Not Exist");
                return;
            }

            if (!this.OpenDailyCheckSpec(pathAndFile))
            {
                DialogResult result = TopMessageBox.ShowMessage(201, "Would you Reset Gain / Offset Table?", "Setting Spec !!");
            }

            this.UpdteDailyCheckInfoToUI();
            this.txtRecipeName.Text = cmbCompareRecipe.SelectedItem.ToString();
        }

        private void btnReCreate_Click(object sender, EventArgs e)
        {
            if (cmbFileName.SelectedItem == null)
            {
                return;
            }
            string pathAndFile = Path.Combine(Constants.Paths.TOOLS_DC_DIR, cmbFileName.SelectedItem.ToString() + ".xml");

            if (File.Exists(pathAndFile))
            {
                File.Delete(pathAndFile);
            }
    
            this.OpenDailyCheckSpec(pathAndFile);  // Create New File
            this.cmbCompareRecipe.Visible = false;
            this.SaveDailyCheckSpec(pathAndFile);        // Save  Old Data  to File
            this.OpenDailyCheckSpec(pathAndFile);  // Open New File
            this.UpdteDailyCheckInfoToUI();             // update
        }

        private void updateRecipeInfo()
        {
            this.lstSelectRecipe.Items.Clear();
            this.lstSelectRecipe.Items.AddRange(this._dailyRecipeManger.TargetRecipes);
            this.lstSystemRecipe.Items.Clear();
            this.lstSystemRecipe.Items.AddRange(this._dailyRecipeManger.SourceRecipes);    
        }

        private void btnAddRecipe_Click(object sender, EventArgs e)
        {
            if (this.lstSystemRecipe.SelectedIndex == -1)
                return;

            if (this.lstSystemRecipe.SelectedIndex > this._dailyRecipeManger.SourceRecipes.Length)
                return;
            this._dailyRecipeManger.Add(this.lstSystemRecipe.SelectedItem.ToString());
            this.updateRecipeInfo();
        }

        private void btnRemoveRecipe_Click(object sender, EventArgs e)
        {
            if (this.lstSelectRecipe.SelectedIndex == -1)
                return;

            if (this.lstSelectRecipe.SelectedIndex > this._dailyRecipeManger.TargetRecipes.Length)
                return;
            this._dailyRecipeManger.Delete(this.lstSelectRecipe.SelectedItem.ToString());
            this.updateRecipeInfo();
        }

        public void InitialFormType(bool isShwoRecipeUI)
        {
            if (isShwoRecipeUI)
            {
                _isShowDailyRecipeSetting = true;
            }
            else
            {
                _isShowDailyRecipeSetting = false;
            }
        }

    }
}
