using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Drawing.Imaging;

using System.Data;
using MPI.Windows.Forms;
using MPI.Tester.Data;
using MPI.Tester.TestKernel;
using MPI.UCF.Forms.Domain;
using MPI.Tester.Report;

namespace MPI.Tester.Gui
{
	public partial class frmTestResult : System.Windows.Forms.Form
	{
		private frmWaferMap MainWaferMapForm;
		private BinGradeColorSet _colorBagSet;
		private Dictionary<string, frmPopWaferMap> PopWaferFormBag;
		private DataTable _dgvHistoryDataTable;
		private readonly Point[] ArrangePosition = new Point[]{	new Point(640,0), new Point(960,0), new Point( 640,380 ), new Point( 960, 380 ),    
																					  new Point(844,1), new Point(844,510)};
		private delegate void ResetTestItemEvent(TestResultData trd, int idx);
		private event ResetTestItemEvent OnEachTestItem;
		private event MethodInvoker OnResetTestItem;

		private delegate void UpdateDataHandler();

		private DataGridViewCellStyle _highlightCellStyle = new DataGridViewCellStyle();
		private DataGridViewCellStyle _normalCellStyle = new DataGridViewCellStyle();
		private DataGridViewCellStyle _errorCellStyle = new DataGridViewCellStyle();
		private DataGridViewCellStyle _emptyCellStyle = new DataGridViewCellStyle();
        private Form _frmUserID;

		public frmTestResult()
		{
			Console.WriteLine("[frmTestResult], frmTestResult()");

			InitializeComponent();

			this.initDataGrid();

			this._colorBagSet = new BinGradeColorSet();

			OnResetTestItem += BinGradeColor_OnResetTestItem;
			OnEachTestItem += BinGradeColor_OnEachTestItem;

			OnResetTestItem += DataGrid_OnResetTestItem;
			OnEachTestItem += DataGrid_OnEachTestItem;

			AppSystem.OnAppSystemRun += new EventHandler(this.AppSystem_OnAppSystemRun);

			AppSystem.ShowMapDataEvent += new EventHandler<ShowMapDataEventArgs>(this.UpdatedgvHistory);

            AppSystem.SaveBinMapEvnet += new EventHandler<SaveBinMapArgs>(this.SaveMapImg);
			this._dgvHistoryDataTable = new DataTable();
			this.dgvHistory.DataSource = this._dgvHistoryDataTable;

			this.InitWaferMap();
			//	this.loadBinColor();

			this.InitCIEChart();
		}

		#region >>> Private Method <<<

		private void initDataGrid()
		{

			this._highlightCellStyle.BackColor = Color.LightBlue;
			this._normalCellStyle.BackColor = Color.White;
			this._errorCellStyle.BackColor = Color.Orange;
            this._errorCellStyle.SelectionBackColor = Color.Orange;
			this._emptyCellStyle.BackColor = Color.Empty;
			//this._normalCellStyle.ForeColor = Color.LightGreen;
			//this._normalCellStyle.Format = "C";

			//--------------------------------------------------
			// DataGridView Setting for "Result" Table
			//--------------------------------------------------
			for (int i = 0; i < this.dgvResult.Columns.Count; i++)
			{
				this.dgvResult.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
				this.dgvResult.Columns[i].Resizable = DataGridViewTriState.False;
				this.dgvResult.Columns[i].ReadOnly = true;
			}
            
			this.dgvResult.DataSource = null;
			this.dgvResult.AutoGenerateColumns = false;
			this.dgvResult.ReadOnly = true;
			this.dgvResult.EditMode = DataGridViewEditMode.EditProgrammatically;
			this.dgvResult.AllowUserToAddRows = false;
			this.dgvResult.AllowUserToDeleteRows = false;
			this.dgvResult.AllowUserToResizeRows = false;
			this.dgvResult.AllowUserToResizeColumns = false;
			this.dgvResult.AllowUserToOrderColumns = false;
			this.dgvResult.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvResult.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			this.dgvResult.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			//this.dgvResult.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;

			//--------------------------------------------------
			// DataGridView Setting for "Statistic" Table
			//--------------------------------------------------
			for (int i = 0; i < this.dgvStat01.Columns.Count; i++)
			{
				this.dgvStat01.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
				this.dgvStat01.Columns[i].Resizable = DataGridViewTriState.False;
				this.dgvStat01.Columns[i].ReadOnly = true;
			}

			this.dgvStat01.ReadOnly = true;
			this.dgvStat01.EditMode = DataGridViewEditMode.EditProgrammatically;
			this.dgvStat01.AllowUserToAddRows = false;
			this.dgvStat01.AllowUserToDeleteRows = false;
			this.dgvStat01.AllowUserToResizeRows = false;
			this.dgvStat01.AllowUserToResizeColumns = false;

			this.dgvStat01.AllowUserToOrderColumns = false;
			this.dgvStat01.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvStat01.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvStat01.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			this.dgvStat01.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			this.dgvStat01.SelectionMode = DataGridViewSelectionMode.FullRowSelect;


			for (int i = 0; i < this.dgvStat02.Columns.Count; i++)
			{
				this.dgvStat02.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
				this.dgvStat02.Columns[i].Resizable = DataGridViewTriState.False;
				this.dgvStat02.Columns[i].ReadOnly = true;
			}

			this.dgvStat02.ReadOnly = true;
			this.dgvStat02.EditMode = DataGridViewEditMode.EditProgrammatically;
			this.dgvStat02.AllowUserToAddRows = false;
			this.dgvStat02.AllowUserToDeleteRows = false;
			this.dgvStat02.AllowUserToResizeRows = false;
			this.dgvStat02.AllowUserToResizeColumns = false;
			this.dgvStat02.AllowUserToOrderColumns = false;
			this.dgvStat02.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvStat02.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvStat02.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			this.dgvStat02.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			this.dgvStat02.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			for (int i = 0; i < this.dgvStat03.Columns.Count; i++)
			{
				this.dgvStat03.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
				this.dgvStat03.Columns[i].Resizable = DataGridViewTriState.False;
				this.dgvStat03.Columns[i].ReadOnly = true;
			}

			this.dgvStat03.ReadOnly = true;
			this.dgvStat03.EditMode = DataGridViewEditMode.EditProgrammatically;
			this.dgvStat03.AllowUserToAddRows = false;
			this.dgvStat03.AllowUserToDeleteRows = false;
			this.dgvStat03.AllowUserToResizeRows = false;
			this.dgvStat03.AllowUserToResizeColumns = false;
			this.dgvStat03.AllowUserToOrderColumns = false;
			this.dgvStat03.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvStat03.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvStat03.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			this.dgvStat03.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			this.dgvStat03.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			//
			this.dgvHistory.ReadOnly = true;
			this.dgvHistory.EditMode = DataGridViewEditMode.EditProgrammatically;
			this.dgvHistory.AllowUserToAddRows = false;
			this.dgvHistory.AllowUserToDeleteRows = false;
			this.dgvHistory.AllowUserToResizeRows = false;
			this.dgvHistory.AllowUserToResizeColumns = false;
			this.dgvHistory.AllowUserToOrderColumns = false;
			this.dgvHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvHistory.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			this.dgvHistory.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			this.dgvHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvHistory.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
		}

		private void ChangeAuthority()
		{
			switch (DataCenter._uiSetting.AuthorityLevel)
			{
				case EAuthority.Operator:
				case EAuthority.QC:
                    this.btnImportMap.Visible = false;
					this.btnImportMap.Enabled = false;
					this.btnBinColor.Enabled = false;
					this.btnSnapshot.Enabled = false;
					break;
				//-------------------------------------------------------------------
				case EAuthority.Engineer:
				case EAuthority.Admin:
				case EAuthority.Super:
                    this.btnImportMap.Visible = false;
                    this.btnImportMap.Enabled = true;
					this.btnBinColor.Enabled = true;
					this.btnSnapshot.Enabled = true;
					break;
				default:
                    this.btnImportMap.Visible = false;
					this.btnImportMap.Enabled = false;
					this.btnBinColor.Enabled = false;
					this.btnSnapshot.Enabled = false;
					break;
			}
		}

		private void DataGrid_OnResetTestItem()
		{
			this.dgvResult.Rows.Clear();
			this.dgvStat01.Rows.Clear();
			this.dgvStat02.Rows.Clear();
			this.dgvStat03.Rows.Clear();
		}

		private void DataGrid_OnEachTestItem(TestResultData resultData, int index)			// index : 0-base
		{
            if (resultData.IsEnable == false || resultData.IsVision == false || resultData.KeyName == ESysResultItem.BIN.ToString())
				return;

			this.dgvStat01.Rows.Add(index + 1, resultData.Name);
			this.dgvStat02.Rows.Add(index + 1, resultData.Name);
			this.dgvStat03.Rows.Add(index + 1, resultData.Name);
            string valStr = resultData.IsTested ? resultData.Value.ToString(resultData.Formate) : "----";
            this.dgvResult.Rows.Add(index + 1, resultData.Name, valStr, resultData.Unit);		// 1-base
			this.dgvResult.Rows[index].Tag = resultData.KeyName;																			// row := 0-base	
		}

		private void updateResultDataGrid(object sender, EventArgs e)
		{
			if (DataCenter._tempCond.TestItemArray == null || DataCenter._tempCond.TestItemArray.Length == 0)
				return;
			if (!this.Visible)
				return;
            try
            {
                int row = 0;
                int dispRowIndex = 0;
                int testItemIndex = 0;
                int resultItemIndex = 0;

                DataGridViewRow dgrow;
                DataGridViewRow dgrow2;
                DataGridViewRow dgrow3;

                this.dgvResult.SuspendLayout();
                this.dgvStat01.SuspendLayout();
                this.dgvStat02.SuspendLayout();
                this.dgvStat03.SuspendLayout();


                //foreach ( TestItemData item in DataCenter._conditionCtrl.Data.TestItemArray )
                foreach (TestItemData item in DataCenter._tempCond.TestItemArray)
                {
                    if (item.IsEnable == true && item.MsrtResult != null)
                    {
                        testItemIndex++;
                    }


                    if (item.MsrtResult != null)
                    {
                        foreach (TestResultData data in item.MsrtResult)
                        {
                            //---------------------------------------------------------------------
                            // Update the result data on the "tabResult"
                            //---------------------------------------------------------------------
                            if (item.IsEnable == true && data.IsEnable == true && data.IsVision == true)
                            {
                                if (this.tabpResult.Visible == true)
                                {
                                    dgrow = this.dgvResult.Rows[row];

                                    if (testItemIndex % 2 == 0)
                                    {
                                        dgrow.DefaultCellStyle = this._normalCellStyle;
                                    }
                                    else
                                    {
                                        dgrow.DefaultCellStyle = this._highlightCellStyle;
                                    }

                                    // dgrow.HeaderCell.Value = (row + 1).ToString();
                                    dgrow.Cells[0].Value = (dispRowIndex + 1).ToString();
                                    string valStr = data.IsTested ? data.Value.ToString(data.Formate) : "----";
                                    dgrow.Cells[2].Value = valStr;

                                    if (data.IsPass == false)
                                    {
                                        dgrow.Cells[2].Style = this._errorCellStyle;
                                    }
                                    else
                                    {
                                        dgrow.Cells[2].Style = this._emptyCellStyle;
                                    }
                                }

                                //----------------------------------------------
                                // Update the statistic data on the "tabStat01"
                                //----------------------------------------------
                                if (this.tabpStat01.Visible == true)
                                {
                                    dgrow = this.dgvStat01.Rows[row];

                                    if (testItemIndex % 2 == 0)
                                    {
                                        dgrow.DefaultCellStyle = this._normalCellStyle;
                                    }
                                    else
                                    {
                                        dgrow.DefaultCellStyle = this._highlightCellStyle;
                                    }

                                    dgrow.Cells[0].Value = (dispRowIndex + 1).ToString();

                                    string valStr = data.IsTested ? data.Value.ToString(data.Formate) : "----";

                                    dgrow.Cells[2].Value = valStr;

                                    dgrow.Cells[3].Value = ReportProcess.StatisticData(data.KeyName, EStatisticType.All01).Min.ToString(data.Formate);

                                    dgrow.Cells[4].Value = ReportProcess.StatisticData(data.KeyName, EStatisticType.All01).Max.ToString(data.Formate);

                                    dgrow.Cells[5].Value = ReportProcess.StatisticData(data.KeyName, EStatisticType.All01).Mean.ToString(data.Formate);

                                    dgrow.Cells[6].Value = ReportProcess.StatisticData(data.KeyName, EStatisticType.All01).STDEV.ToString(data.Formate);
                                }
                                else if (this.tabpStat02.Visible == true)
                                {
                                    dgrow2 = this.dgvStat02.Rows[row];

                                    if (testItemIndex % 2 == 0)
                                    {
                                        dgrow2.DefaultCellStyle = this._normalCellStyle;
                                    }
                                    else
                                    {
                                        dgrow2.DefaultCellStyle = this._highlightCellStyle;
                                    }

                                    dgrow2.Cells[0].Value = (dispRowIndex + 1).ToString();

                                    string valStr = data.IsTested ? data.Value.ToString(data.Formate) : "----";

                                    dgrow2.Cells[2].Value = valStr;

                                    dgrow2.Cells[3].Value = ReportProcess.StatisticData(data.KeyName, EStatisticType.Single01).Min.ToString(data.Formate);

                                    dgrow2.Cells[4].Value = ReportProcess.StatisticData(data.KeyName, EStatisticType.Single01).Max.ToString(data.Formate);

                                    dgrow2.Cells[5].Value = ReportProcess.StatisticData(data.KeyName, EStatisticType.Single01).Mean.ToString(data.Formate);

                                    dgrow2.Cells[6].Value = ReportProcess.StatisticData(data.KeyName, EStatisticType.Single01).STDEV.ToString(data.Formate);

                                    dgrow2.Cells[7].Value = ReportProcess.GoodRate01(data.KeyName).ToString("0.00");
                                }
                                else if (this.tabpStat03.Visible == true)
                                {
                                    dgrow3 = this.dgvStat03.Rows[row];

                                    dgrow3.Cells[0].Value = (dispRowIndex + 1).ToString();

                                    string valStr = data.IsTested ? data.Value.ToString(data.Formate) : "----";

                                    dgrow3.Cells[2].Value = valStr;

                                    dgrow3.Cells[3].Value = ReportProcess.StatisticData(data.KeyName, EStatisticType.Single02).Min.ToString(data.Formate);

                                    dgrow3.Cells[4].Value = ReportProcess.StatisticData(data.KeyName, EStatisticType.Single02).Max.ToString(data.Formate);

                                    dgrow3.Cells[5].Value = ReportProcess.StatisticData(data.KeyName, EStatisticType.Single02).Mean.ToString(data.Formate);

                                    dgrow3.Cells[6].Value = ReportProcess.StatisticData(data.KeyName, EStatisticType.Single02).STDEV.ToString(data.Formate);

                                    dgrow3.Cells[7].Value = ReportProcess.GoodRate02(data.KeyName).ToString("0.00");
                                }

                                if (this.dgvResult.Rows[row].Visible == true)
                                {
                                    dispRowIndex++;
                                }

                                row++;
                            }

                            resultItemIndex++;              // It must count the all "TestResultData", even the "TestItem" is Disable			
                        }
                    }

                }
                this.dgvResult.ResumeLayout();
                this.dgvStat01.ResumeLayout();
                this.dgvStat02.ResumeLayout();
            }
            catch (Exception exc)
            {
                Console.WriteLine("[frmTestResult],updateResultDataGrid(),Exception:" + exc.Message);
            }
		}

		private void ResetBinGradeData()
		{
            switch (DataCenter._uiSetting.UserID)
            {
                case EUserID.WAVETEK00://高度客製化Bin會很容易出狀況，因此直接隱藏
                    {
                        lstvBinGrade.Visible = false;
                    }
                    break;
                default:
                    lstvBinGrade.Visible = true;
                    break;
            }


			this.lstvBinGrade.Items.Clear();

			// Bin item
			SmartBin smartBin = DataCenter._smartBinning.SmartBin;

			for (int i = 0; i < smartBin.Count; i++)
			{
				ListViewItem item = new ListViewItem(smartBin[i].BinCode);

				item.Tag = EBinningType.IN_BIN.ToString();				

				if (DataCenter._smartBinning.IsAutoBin)
				{
					item.SubItems.Add("");
				}
				else
				{
					item.SubItems.Add(smartBin[i].BinNumber.ToString());
				}

				item.SubItems[0].Tag = smartBin[i].SerialNumber;

				item.SubItems.Add("0");

				item.SubItems.Add("0.00");

				this.lstvBinGrade.Items.Add(item);
			}

			// NG Bin item
			SmartNGBin smartNGBin = DataCenter._smartBinning.NGBin;

			for (int i = 0; i < smartNGBin.Count; i++)
			{
				if (!smartNGBin[i].IsEnable)
				{
					continue;
				}

				ListViewItem item = new ListViewItem(smartNGBin[i].BinCode);

				item.Tag = EBinningType.NG_BIN.ToString();

				item.BackColor = Color.LightSeaGreen;

				if (DataCenter._smartBinning.IsAutoBin)
				{
					item.SubItems.Add("");
				}
				else
				{
					item.SubItems.Add(smartNGBin[i].BinNumber.ToString());
				}

				item.SubItems[0].Tag = smartNGBin[i].KeyName;

				item.SubItems.Add("0");

				item.SubItems.Add("0.00");

				this.lstvBinGrade.Items.Add(item);
			}

			// Side Bin item
			SmartSideBin smartSideBin = DataCenter._smartBinning.SideBin;

			for (int i = 0; i < smartSideBin.Count; i++)
			{
				if (!smartBin.ContainsBoundary(smartSideBin[i].KeyName))
				{
					continue;
				}

				ListViewItem item = new ListViewItem(smartSideBin[i].BinCode);

				item.Tag = EBinningType.SIDE_BIN.ToString();

				item.BackColor = Color.LightBlue;

				if (DataCenter._smartBinning.IsAutoBin)
				{
					item.SubItems.Add("");
				}
				else
				{
					item.SubItems.Add(smartSideBin[i].BinNumber.ToString());
				}

				item.SubItems[0].Tag = smartSideBin[i].KeyName;

				item.SubItems.Add("0");

				item.SubItems.Add("0.00");

				this.lstvBinGrade.Items.Add(item);
			}
		}

        private void updateBinToListView(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < this.lstvBinGrade.Items.Count; i++)
                {
                    if (this.lstvBinGrade.Items[i].Tag.ToString() == EBinningType.IN_BIN.ToString())
                    {
                        int serialNumber = (int)this.lstvBinGrade.Items[i].SubItems[0].Tag;

                        uint binCount = 0;
                        if (DataCenter._smartBinning.GetBinFromSN(serialNumber) != null)
                        {
                            binCount = DataCenter._smartBinning.GetBinFromSN(serialNumber).ChipCount;
                        }

                        if (DataCenter._smartBinning.IsAutoBin)
                        {
                            if (binCount > 0)
                            {
                                this.lstvBinGrade.Items[i].SubItems[0].Text = DataCenter._smartBinning.SmartBin[i].AutoBinNumber.ToString();
                            }
                            else
                            {
                                this.lstvBinGrade.Items[i].SubItems[0].Text = string.Empty;
                            }
                        }

                        this.lstvBinGrade.Items[i].SubItems[1].Text = binCount.ToString();

                        if (DataCenter._smartBinning.ChipCount > 0)
                        {
                            double rate = (100 * (double)binCount) / DataCenter._smartBinning.ChipCount;

                            this.lstvBinGrade.Items[i].SubItems[2].Text = rate.ToString("0.00");
                        }
                    }
                    else if (this.lstvBinGrade.Items[i].Tag.ToString() == EBinningType.NG_BIN.ToString())
                    {
                        string keyName = this.lstvBinGrade.Items[i].SubItems[0].Tag.ToString();

                        uint binCount = 0;
                        if (DataCenter._smartBinning.NGBin[keyName] != null)
                        {
                            binCount = DataCenter._smartBinning.NGBin[keyName].ChipCount;
                        }

                        if (DataCenter._smartBinning.IsAutoBin)
                        {
                            if (binCount > 0)
                            {
                                this.lstvBinGrade.Items[i].SubItems[0].Text = DataCenter._smartBinning.NGBin[keyName].AutoBinNumber.ToString();
                            }
                            else
                            {
                                this.lstvBinGrade.Items[i].SubItems[0].Text = string.Empty;
                            }
                        }

                        this.lstvBinGrade.Items[i].SubItems[1].Text = binCount.ToString();

                        if (DataCenter._smartBinning.ChipCount > 0)
                        {
                            double rate = (100 * (double)binCount) / DataCenter._smartBinning.ChipCount;

                            this.lstvBinGrade.Items[i].SubItems[2].Text = rate.ToString("0.00");
                        }
                    }
                    else if (this.lstvBinGrade.Items[i].Tag.ToString() == EBinningType.SIDE_BIN.ToString())
                    {
                        string keyName = this.lstvBinGrade.Items[i].SubItems[0].Tag.ToString();

                        //uint binCount = DataCenter._smartBinning.SideBin[keyName].ChipCount;

                        uint binCount = 0;
                        if (DataCenter._smartBinning.SideBin[keyName] != null)
                        {
                            binCount = DataCenter._smartBinning.SideBin[keyName].ChipCount;
                        }

                        if (DataCenter._smartBinning.IsAutoBin)
                        {
                            if (binCount > 0)
                            {
                                this.lstvBinGrade.Items[i].SubItems[0].Text = DataCenter._smartBinning.SideBin[keyName].AutoBinNumber.ToString();
                            }
                            else
                            {
                                this.lstvBinGrade.Items[i].SubItems[0].Text = string.Empty;
                            }
                        }

                        this.lstvBinGrade.Items[i].SubItems[1].Text = binCount.ToString();

                        if (DataCenter._smartBinning.ChipCount > 0)
                        {
                            double rate = (100 * (double)binCount) / DataCenter._smartBinning.ChipCount;

                            this.lstvBinGrade.Items[i].SubItems[2].Text = rate.ToString("0.00");
                        }
                    }
                }
            }

            catch (Exception exc)
            {
                Console.WriteLine("[frmTestResult],updateBinToListView(),Exception:" + exc.Message);
            }
        }

		private void includeStatusForm()
		{
			frmTestResultInstantInfo form = (frmTestResultInstantInfo)FormAgent.RetrieveForm(typeof(frmTestResultInstantInfo));
			form.RegisterUpdateEvent(this.updateResultDataGrid);
			form.RegisterUpdateEvent(this.updateBinToListView);
			form.TopLevel = false;
		}

		private void includeWaferMapForm()
		{
			MainWaferMapForm.TopLevel = false;
			MainWaferMapForm.Parent = this.pnlWaferMap;
			MainWaferMapForm.Dock = DockStyle.Fill;
			MainWaferMapForm.FormBorderStyle = FormBorderStyle.None;

			MainWaferMapForm.Show();
		}

        private void includeBarcodeForm()
        {

            if (this._frmUserID != null)
			{
				this._frmUserID.Close();
				this._frmUserID.Dispose();
			}

            switch (DataCenter._uiSetting.UserID)
            {
                case EUserID.InfiniLED:
                    {
                        this._frmUserID = new frmInfiniLED();
                        
                        break;
                    }
				case EUserID.ChangeLight:
					{
						this._frmUserID = new frmChangeLight();

						break;
					}
                default:
                    {
                        this._frmUserID = new frmBarcodeSetting();

                        break;
                    }
            }

            this._frmUserID.TopLevel = false;
            this._frmUserID.Parent = this.plBarcode;
            this._frmUserID.Dock = DockStyle.Fill;
            this._frmUserID.FormBorderStyle = FormBorderStyle.None;
            this._frmUserID.Show();


            //frmBarcodeSetting form = (frmBarcodeSetting)FormAgent.RetrieveForm(typeof(frmBarcodeSetting));
            //form.TopLevel = false;
            //form.Parent = this.plBarcode;
            //form.Dock = DockStyle.Fill;
            //form.FormBorderStyle = FormBorderStyle.None;

          //  form.Show();
        }

		private void ChangeWaferMapShowItem()
		{
			if (DataCenter._uiSetting.ShowMapKeyName != string.Empty)
			{
				if (DataCenter._uiSetting.UserDefinedData.ResultItemNameDic.ContainsKey(DataCenter._uiSetting.ShowMapKeyName))
				{
					string name = DataCenter._uiSetting.UserDefinedData.ResultItemNameDic[DataCenter._uiSetting.ShowMapKeyName];

					if (DataCenter._uiSetting.ShowMapKeyName.Contains("LOP"))
					{
						name += "(mcd)";
					}
					else if (DataCenter._uiSetting.ShowMapKeyName.Contains("WATT"))
					{
						name += "(mW)";
					}
					else if (DataCenter._uiSetting.ShowMapKeyName.Contains("LM"))
					{
						name += "(lm)";
					}

					foreach (var item in this.cmbMsrtItem.Items)
					{
						if (item.ToString() == name)
						{
							this.cmbMsrtItem.SelectedItem = item;
						}
					}
				}
			}
		}

		private void InitWaferMap()
		{
			PopWaferFormBag = new Dictionary<string, frmPopWaferMap>(6);
			MainWaferMapForm = new frmWaferMap();
			AppSystem.ShowMapDataEvent += MainWaferMapForm.MsrtProcess_OnMapDataEvent;
			MainWaferMapForm.WaferMapPrepare();

			MainWaferMapForm.OnWaferMapDieClickEvent += this.WaferMap_DieClick;
		}

		private void AppSystem_OnAppSystemRun(object sender, EventArgs e)
		{
			if (this.IsHandleCreated == false)
				return;

			SetWaferMap();

			int lift = DataCenter._uiSetting.WaferMapLeft;
			int top = DataCenter._uiSetting.WaferMapTop;
			int right = DataCenter._uiSetting.WaferMapRight;
			int bottom = DataCenter._uiSetting.WaferMapBottom;

			//	this.MainWaferMapForm.SetWaferMapBoundary(lift, top, right, bottom, DataCenter._uiSetting.WaferMapGrowthDirection);

			foreach (frmPopWaferMap form in PopWaferFormBag.Values)
			{
				if (form.IsDisposed || form.Disposing)
					continue;

				form.Reset(lift, top, right, bottom);
			}
		}

		public void SetWaferMap()
		{
			MainWaferMapForm.Reset();

			int left = DataCenter._uiSetting.WaferMapLeft;
			int top = DataCenter._uiSetting.WaferMapTop;
			int right = DataCenter._uiSetting.WaferMapRight;
			int bottom = DataCenter._uiSetting.WaferMapBottom;

			this.MainWaferMapForm.SetWaferMapBoundary(left, top, right, bottom, DataCenter._uiSetting.WaferMapGrowthDirection);
		}

		private void WaferMapSelectable(bool isSelctable)
		{
			//this.MainWaferMapForm.SetWaferMapSelectable(isSelctable);

			//foreach (frmPopWaferMap form in PopWaferFormBag.Values)
			//{
			//    if (form.IsDisposed || form.Disposing)
			//        continue;
			//    form.SetWaferMapSelectable(isSelctable);
			//}
		}

		private void UpdateDataToControls()
		{
			if (Report.ReportProcess.IsImplement && DataCenter._rdFunc.RDFuncData.IsShowContinousProbing)
			{
				this.gbContinousProbing.Visible = true;
			}
			else
			{
				this.gbContinousProbing.Visible = false;
			}

            if (DataCenter._uiSetting.AuthorityLevel == EAuthority.Super)
            {
                this.tabiCIEChart.Visible = true;
            }
            else
            {
                if (DataCenter._rdFunc.RDFuncData.TesterConfigType != ETesterConfigType.LEDTester)
                {
                    this.tabiCIEChart.Visible = false;
                }
            }

			this.cmbMsrtItem.Enabled = false;

			this.resetTestItemData();

			this.cmbMsrtItem.DisplayMember = "Title";
			this.cmbMsrtItem.ValueMember = "Me";

			Array bgc = this._colorBagSet.ToArray();
			this.cmbMsrtItem.DataSource = bgc;
			//	this.loadBinColor();
			this.applyBinColor();

			this.cmbMsrtItem.Enabled = true;

			if (this.cmbMsrtItem.Items.Count == 0)
			{
				this.MainWaferMapForm.isEnableShowMap = false;
			}
			else
			{
				this.MainWaferMapForm.isEnableShowMap = true;
			}

			bool autoBoundery = DataCenter._uiSetting.WaferMapAutoBoundery;
			//this.MainWaferMapForm.AutoBoundaryWaferMap = autoBoundery;

			ChangeWaferMapShowItem();

            bool enable = true;

			switch (DataCenter._uiSetting.UIOperateMode)
			{
				case (int)EUIOperateMode.Idle:
					this.ChangeAuthority();
                    enable = true;
					break;
				//-----------------------------------------------------------------------------
				case (int)EUIOperateMode.AutoRun:
                case (int)EUIOperateMode.ManulRun:
					this.btnImportMap.Enabled = false;
					this.btnBinColor.Enabled = false;
					this.btnSnapshot.Enabled = false;
                    enable = false;
					break;
				//-----------------------------------------------------------------------------
                //case (int)EUIOperateMode.ManulRun:
                //    this.ChangeAuthority();
                //    enable = false;
                //    break;
				//-----------------------------------------------------------------------------
				default:
					this.ChangeAuthority();
                    enable = true;
					break;
			}


            if (enable && (int)DataCenter._uiSetting.AuthorityLevel >= 20)
            {
                this.btnImportMap.Enabled = true;
                this.btnBinColor.Enabled = true;
                this.btnSnapshot.Enabled = true;
                //this.btnConfirm.Enabled = true;
            }
            else
            {
                this.btnImportMap.Enabled = false;
                this.btnBinColor.Enabled = false;
                this.btnSnapshot.Enabled = false;
                //  this.btnConfirm.Enabled = false;
            }


			this.ResetDgvHistory();

			this.ResetcmbLopItem();

			this.UpdateBinRangeToCIEChart();

			this.ResetBinGradeData();

          //  this.includeBarcodeForm();
		}

		private void ResetDgvHistory()
		{
			if (!this.IsHandleCreated)
			{
				return;
			}

			//Check item is change
			string dgvKey = string.Empty;

			string testItemKey = "TEST" + "BIN";

			foreach (DataColumn column in this._dgvHistoryDataTable.Columns)
			{
				dgvKey += column.ColumnName;
			}

			if (DataCenter._tempCond.TestItemArray != null && DataCenter._tempCond.TestItemArray.Length != 0)
			{
				TestItemData[] testItemArray = DataCenter._tempCond.TestItemArray;

				foreach (var testItem in testItemArray)
				{
					if (!testItem.IsEnable || testItem.MsrtResult == null || testItem.MsrtResult.Length == 0)
					{
						continue;
					}

					TestResultData[] resultItemArray = testItem.MsrtResult;

					foreach (var resultItem in resultItemArray)
					{
						if (!resultItem.IsEnable || !resultItem.IsVision)
						{
							continue;
						}

						if (!DataCenter._uiSetting.UserDefinedData.ResultItemNameDic.ContainsKey(resultItem.KeyName))
						{
							continue;
						}

						testItemKey += resultItem.KeyName;
					}
				}
			}

			if (dgvKey == testItemKey)
			{
				return;
			}

			this._dgvHistoryDataTable.Rows.Clear();

			this._dgvHistoryDataTable.Columns.Clear();

			this._dgvHistoryDataTable.Columns.Add("TEST", typeof(string));

            this._dgvHistoryDataTable.Columns.Add("CHIP_INDEX", typeof(string));

            this._dgvHistoryDataTable.Columns.Add("COL", typeof(string));

            this._dgvHistoryDataTable.Columns.Add("ROW", typeof(string));

			this._dgvHistoryDataTable.Columns.Add("BIN", typeof(string));

			this._dgvHistoryDataTable.Columns["TEST"].Caption = "TEST";

            this._dgvHistoryDataTable.Columns["COL"].Caption = "COL";

            this._dgvHistoryDataTable.Columns["ROW"].Caption = "ROW";

            this._dgvHistoryDataTable.Columns["CHIP_INDEX"].Caption = "CHIP_INDEX";

			this._dgvHistoryDataTable.Columns["BIN"].Caption = "BIN";

			if (DataCenter._tempCond.TestItemArray != null && DataCenter._tempCond.TestItemArray.Length != 0)
			{
				TestItemData[] testItemArray = DataCenter._tempCond.TestItemArray;

				foreach (var testItem in testItemArray)
				{
					if (!testItem.IsEnable || testItem.MsrtResult == null || testItem.MsrtResult.Length == 0)
					{
						continue;
					}

					TestResultData[] resultItemArray = testItem.MsrtResult;

					foreach (var resultItem in resultItemArray)
					{
						if (!resultItem.IsEnable || !resultItem.IsVision)
						{
							continue;
						}

						if (!DataCenter._uiSetting.UserDefinedData.ResultItemNameDic.ContainsKey(resultItem.KeyName))
						{
							continue;
						}

						this._dgvHistoryDataTable.Columns.Add(resultItem.KeyName, typeof(string));

						this._dgvHistoryDataTable.Columns[resultItem.KeyName].Caption = resultItem.Name;
					}
				}
			}

			for (int i = 0; i < this.dgvHistory.Columns.Count; i++)
			{
                if (this.dgvHistory.Columns[i].Name == "TEST" ||
                    this.dgvHistory.Columns[i].Name == "COL" ||
                    this.dgvHistory.Columns[i].Name == "ROW")
				{
					this.dgvHistory.Columns[i].Width = 50;
                    this.dgvHistory.Columns[i].Frozen = true;
				}
                else if (this.dgvHistory.Columns[i].Name == "CHIP_INDEX")
                {
                    this.dgvHistory.Columns[i].Width = 80;
                    this.dgvHistory.Columns[i].Frozen = true;
                }
                else if (this.dgvHistory.Columns[i].Name == "BIN")
                {
                    this.dgvHistory.Columns[i].Width = 40;
                }
                else
                {
                    this.dgvHistory.Columns[i].Width = 150;
                }

				this.dgvHistory.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

				this.dgvHistory.Columns[i].Resizable = DataGridViewTriState.False;

				this.dgvHistory.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

				this.dgvHistory.Columns[i].ReadOnly = true;

				this.dgvHistory.Columns[i].HeaderText = this._dgvHistoryDataTable.Columns[i].Caption;
			}
		}

		private void UpdatedgvHistory(object sender, ShowMapDataEventArgs e)
		{
			if (!this.IsHandleCreated || DataCenter._tempCond.TestItemArray == null || DataCenter._tempCond.TestItemArray.Length == 0)
				return;

			if (this._dgvHistoryDataTable.Columns.Count == 0)
				return;

			this.dgvHistory.SuspendLayout();

			if (e.Values["TEST"] <= 1 && DataCenter._uiSetting.IsEnableClearHistoryWhenStartTest)
			{
				this._dgvHistoryDataTable.Rows.Clear();
			}

			TestItemData[] testItemArray = DataCenter._tempCond.TestItemArray;

			DataRow dataRow = this._dgvHistoryDataTable.NewRow();

			dataRow["TEST"] = e.Values["TEST"].ToString();
			dataRow["BIN"] = e.Values["BIN"].ToString();
            dataRow["CHIP_INDEX"] = e.Values["CHUCKINDEX"].ToString();
            dataRow["COL"] = e.Values["COL"].ToString();
            dataRow["ROW"] = e.Values["ROW"].ToString();

			foreach (var testItem in testItemArray)
			{
				if (!testItem.IsEnable || testItem.MsrtResult == null || testItem.MsrtResult.Length == 0)
					continue;

				TestResultData[] resultItemArray = testItem.MsrtResult;

				foreach (var resultItem in resultItemArray)
				{
					if (!resultItem.IsEnable || !resultItem.IsVision)
					{
						continue;
					}

					if (!DataCenter._uiSetting.UserDefinedData.ResultItemNameDic.ContainsKey(resultItem.KeyName))
					{
						continue;
					}

					float data = e.Values[resultItem.KeyName];

					if (this._dgvHistoryDataTable.Columns.Contains(resultItem.KeyName))
					{
						dataRow[resultItem.KeyName] = data.ToString(resultItem.Formate);
					}
				}
			}

			this._dgvHistoryDataTable.Rows.Add(dataRow);

            if (DataCenter._sysSetting.IsSpecBinTableSync)
			{
				int binSN = (int)e.Values[ESysResultItem.BINSN.ToString()];

                if (DataCenter._smartBinning.GetBinFromSN(binSN) != null)
                {
                if (DataCenter._smartBinning.GetBinFromSN(binSN).BinningType != EBinningType.IN_BIN)
				{
					int index = this._dgvHistoryDataTable.Rows.IndexOf(dataRow);

					this.dgvHistory.Rows[index].DefaultCellStyle.BackColor = this._errorCellStyle.BackColor;
				}

                this.lblBinResult.Text = DataCenter._smartBinning.GetBinFromSN(binSN).BinNumber.ToString();
			}
			}
			else
			{
				float isAllPass = e.Values[ESysResultItem.ISALLPASS.ToString()];

				if (isAllPass == 2.0f)
				{
					int index = this._dgvHistoryDataTable.Rows.IndexOf(dataRow);

					this.dgvHistory.Rows[index].DefaultCellStyle.BackColor = this._errorCellStyle.BackColor;
				}

				this.lblBinResult.Text = isAllPass.ToString();
			}

			if (this._dgvHistoryDataTable.Rows.Count > 200)
			{
				this._dgvHistoryDataTable.Rows.RemoveAt(0);
			}

			if (this.dgvHistory.Rows.Count > 0)
			{
				//this.dgvHistory.CurrentCell = this.dgvHistory[0, this.dgvHistory.Rows.Count - 1];

                this.dgvHistory.FirstDisplayedScrollingRowIndex = this.dgvHistory.Rows.Count - 1;
			}

			//this.dgvHistory.ResumeLayout();

            this.lblBinResult.Text = DataCenter._acquireData.ChipInfo.BinGrade.ToString();
		}

        private void SaveMapImg(object sender, SaveBinMapArgs e)
        {
            SaveMapImg(e._fileName, ImageFormat.Jpeg);
        }
		#endregion

		#region >>> Private CIE Method <<<

		private void InitCIEChart()
		{
			this.cmbCIETypeItem.Items.Add(MPI.UCF.Forms.Domain.CIE.EChartType.CIE1931);

			this.cmbCIETypeItem.Items.Add(MPI.UCF.Forms.Domain.CIE.EChartType.CIE1976);

			this.cmbCIETypeItem.SelectedIndex = 0;

			AppSystem.OnAppSystemRun += new EventHandler(this.ResetCIEChartEvent);

			AppSystem.ShowMapDataEvent += new EventHandler<ShowMapDataEventArgs>(this.SendCIExyDataEvent);
			
			this.CieChartPanel.Show();

			this.CieChartPanel.CreateHandle();

			this.ResetcmbLopItem();

			this.UpdateBinRangeToCIEChart();
		}

		private void UpdateCIEChart3GImage(object sender, EventArgs e)
		{
			if (!this.CieChartPanel.Created)
			{
				return;
			}

			MPI.UCF.Forms.Domain.CIE.EDrawItem format = 0;

			if (this.checkBoxCIEColor.Checked)
			{
				format |= MPI.UCF.Forms.Domain.CIE.EDrawItem.CEIChart;
			}

			if (this.checkBoxPlankian.Checked)
			{
				format |= MPI.UCF.Forms.Domain.CIE.EDrawItem.Planckian;
			}

			if (this.checkBoxGridLine.Checked)
			{
				format |= MPI.UCF.Forms.Domain.CIE.EDrawItem.GridLines;
			}

			if (this.checkBoxBinRange.Checked)
			{
				format |= MPI.UCF.Forms.Domain.CIE.EDrawItem.BinRange;
			}

			if (this.checkBoxXWaveLength.Checked)
			{
				format |= MPI.UCF.Forms.Domain.CIE.EDrawItem.WaveLength;
			}

			this.CieChartPanel.Clear();

			this.CieChartPanel.ChartType = (MPI.UCF.Forms.Domain.CIE.EChartType)cmbCIETypeItem.SelectedItem;

			this.CieChartPanel.DrawingItem = format;
		}

		private void AddCIExy(ShowMapDataEventArgs Data)
		{
			if (!DataCenter._sysSetting.OptiDevSetting.IsCalcCRIData)
			{
				return;
			}

			Dictionary<string, float> cieItem = Data.Values;

			TestItemData[] testItemArray = DataCenter._product.TestCondition.TestItemArray;

			if (testItemArray == null || testItemArray.Length == 0)
			{
				return;
			}

			foreach (var testItem in testItemArray)
			{
				if (!testItem.IsEnable)
				{
					continue;
				}

				if (!(testItem is LOPWLTestItem) && !(testItem is VLRTestItem))
				{
					continue;
				}

				string xkey = string.Empty;

				string ykey = string.Empty;

				foreach (var resultitem in testItem.MsrtResult)
				{
					if (!resultitem.IsEnable || !resultitem.IsVision)
					{
						continue;
					}

					if (resultitem.KeyName.Contains(EOptiMsrtType.CIEx.ToString()))
					{
						xkey = resultitem.KeyName;
					}

					if (resultitem.KeyName.Contains(EOptiMsrtType.CIEy.ToString()))
					{
						ykey = resultitem.KeyName;
					}
				}

				float ciex = 0.0f;

				float ciey = 0.0f;

				if (cieItem.TryGetValue(xkey, out ciex) && cieItem.TryGetValue(ykey, out ciey))
				{
					this.CieChartPanel.AddXy(testItem.Name, ciex, ciey);					
				}
			}
		}

		private void ResetcmbLopItem()
		{
			this.cmbLopItem.Items.Clear();

			this.cmbLopItem.BeginUpdate();

			TestItemData[] testItemArray = DataCenter._product.TestCondition.TestItemArray;

			if (testItemArray != null && testItemArray.Length != 0)
			{
				foreach (var testItem in testItemArray)
				{
					if (!testItem.IsEnable)
					{
						continue;
					}

					if (!(testItem is LOPWLTestItem) && !(testItem is VLRTestItem))
					{
						continue;
					}

					this.cmbLopItem.Items.Add(testItem.Name);
				}
			}

			this.cmbLopItem.EndUpdate();

			if (this.cmbLopItem.Items.Count > 0 && this.cmbLopItem.SelectedIndex < 0)
			{
				this.cmbLopItem.SelectedIndex = 0;
			}
		}

		private void UpdateBinRangeToCIEChart()
		{
			this.CieChartPanel.ClearBinRange();

			SmartBin smartBin = DataCenter._smartBinning.SmartBin;

			foreach (var item in smartBin.Boundary)
			{
				CIE.EChartType type = CIE.EChartType.CIE1931;

				if (item.KeyName.Contains(SmartBinning.CIExyKEY))
				{
					type = CIE.EChartType.CIE1931;
				}
				else if (item.KeyName.Contains(SmartBinning.CIEupvpKEY))
				{
					type = CIE.EChartType.CIE1976;
				}
				else
				{
					continue;
				}

				for (int col = 0; col < item.Count; col++)
				{
					if (item[col] is SmartPolygon)
					{
						List<PointF> binRange = new List<PointF>();

						SmartPolygon polygon = item[col] as SmartPolygon;

						for (int cnt = 0; cnt < polygon.Coord.Count; cnt++)
						{
							binRange.Add(new PointF(polygon.Coord[cnt].X, polygon.Coord[cnt].Y));
						}

						this.CieChartPanel.DefineBinRange(binRange.ToArray(), type);
					}
					else if (item[col] is SmartEllipse)
					{
						SmartEllipse ellipse = item[col] as SmartEllipse;
						
						float angle = (float)ellipse.Theta;

						float x = (float)ellipse.X;

						float y = (float)ellipse.Y;

						float width = (float)ellipse.a;

						float height = (float)ellipse.b;

						this.CieChartPanel.DefineEllipseBinRange(new EllipseBinRange(angle, x, y, width, height), type);
					}
				}
			}

			this.CieChartPanel.RedrawChart();
		}

		#endregion

		#region >>> Public Method <<<

		//------------------------------------------------------------------------------------------
		// When parameters of condition setting is changed, it will fire event
		// and call this UpdateDataToUIForm() function. The form just updates the measurement items in listBox
		//-------------------------------------------------------------------------------------------

		public void UpdateDataToUIForm()
		{
			if (this.InvokeRequired && this.IsHandleCreated)
			{
				this.BeginInvoke(new UpdateDataHandler(UpdateDataToControls), null);		// Run at other TestServer Thread
			}
			else if (this.IsHandleCreated)
			{
				this.UpdateDataToControls();			// Run at Main Thread
			}
		}

		private void resetTestItemData()
		{
			// Set tempCond, when no evet is fired
			DataCenter._tempCond = DataCenter._product.TestCondition.Clone() as ConditionData;

			this.BinGradeColor_OnResetTestItem();

			if (DataCenter._product.TestCondition == null || DataCenter._product.TestCondition.TestItemArray == null)
			{
				this.DataGrid_OnResetTestItem();

				return;
			}

			if (DataCenter._product.TestCondition.TestItemArray.Length == 0)
			{
				this.DataGrid_OnResetTestItem();

				return;
			}

			int index = 0;
			foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
			{
				if (item.IsEnable == true && item.MsrtResult != null)
				{
					foreach (TestResultData resultData in item.MsrtResult)
					{
						if (resultData.IsVision && resultData.IsEnable)
						{
							if (index == 0)
							{
								this.OnResetTestItem.Invoke();

								TestResultData bin = new TestResultData();

								bin.IsEnable = true;

								bin.KeyName = ESysResultItem.BIN.ToString();

								bin.Name = ESysResultItem.BIN.ToString();

								bin.Formate = "0";

								this.OnEachTestItem.Invoke(bin, index);
							}

							this.OnEachTestItem.Invoke(resultData, index);

							index++;
						}
					}

				}
			}

			foreach (KeyValuePair<string, frmPopWaferMap> item in PopWaferFormBag)
			{
				frmPopWaferMap form = item.Value;

				if (form.IsDisposed || form.Disposing)
				{
					continue;
				}

				bool isExist = false;

				foreach (TestItemData item2 in DataCenter._product.TestCondition.TestItemArray)
				{
					if (item2.IsEnable == true && item2.MsrtResult != null)
					{
						foreach (TestResultData resultData in item2.MsrtResult)
						{
							if (resultData.IsVision && resultData.IsEnable)
							{
								if (resultData.KeyName == form.WMSymbolId)
								{
									isExist = true;

									continue;
								}
							}
						}

					}
				}

				if (!isExist)
				{
					form.Dispose();

					form.Close();
				}
			}
		}

		public void autoArrangeWindow()
		{
			List<string> disposed_list = new List<string>();

			int idx = 0;
			foreach (KeyValuePair<string, frmPopWaferMap> item in PopWaferFormBag)
			{
				Form form = item.Value;
				if (form.IsDisposed || form.Disposing)
				{
					disposed_list.Add(item.Key);

					continue;
				}

				if (idx > 5)
					break;

				form.Location = ArrangePosition[idx];

				form.BringToFront();

				form.Show();

				form.TopMost = true;

				if (!DataCenter._uiSetting.IsEnableMapFormTopMost)
				{
					form.TopMost = false;
				}

				idx++;
			}

			foreach (string item in disposed_list)
			{
				PopWaferFormBag.Remove(item);
			}
		}

		public void ApplyBinColor()
		{
			this.applyBinColor();
		}

        public void SaveMapImg(string path, ImageFormat format )
        {
            try
            {
                Bitmap buffer = new Bitmap(this.pnlWaferMap.Width, this.pnlWaferMap.Height);

                this.pnlWaferMap.DrawToBitmap(buffer, new Rectangle(0, 0, this.pnlWaferMap.Width, this.pnlWaferMap.Height));

                //if (File.Exists(path))
                //{ MPIFile.DeleteFile(path); }

                buffer.Save(path, format);
            }
            catch (Exception e)
            {
                Console.WriteLine("[frmResult],SaveMapImg exception:" + e.Message);
            }
        }
		#endregion

		#region >>> Bin Grade Color <<<

		/// <summary>
		/// Reset Bin Grade Color
		/// </summary>
		void BinGradeColor_OnResetTestItem()
		{
			this._colorBagSet.Clear();
			this.cmbMsrtItem.DataSource = null;
			this.cmbMsrtItem.Items.Clear();
			this.btnShowMap.SubItems.Clear();
		}

		void BinGradeColor_OnEachTestItem(TestResultData data, int idx)
		{
			if (!DataCenter._mapData.WeferMapShowItem.Contains(data.KeyName))
			{
				return;
			}

			BinGradeColor bgc = new BinGradeColor();

			bgc.Title = data.Name;

			bgc.KeyName = data.KeyName;

			if (bgc.KeyName.Contains("LOP"))
			{
				bgc.Title += "(mcd)";
			}
			else if (bgc.KeyName.Contains("WATT"))
			{
				bgc.Title += "(mW)";
			}
			else if (bgc.KeyName.Contains("LM"))
			{
				bgc.Title += "(lm)";
			}

			bgc.DisplayFormat = data.Formate;

			if (data.MaxLimitValue >= data.MinLimitValue)
			{
				bgc.Max = (float)data.MaxLimitValue;

				bgc.Min = (float)data.MinLimitValue;
			}

			else if (data.MaxLimitValue < data.MinLimitValue)
			{
				bgc.Max = (float)data.MinLimitValue;

				bgc.Min = (float)data.MaxLimitValue;
			}

			this._colorBagSet.Add(bgc.KeyName, bgc);

			this.cmbMsrtItem.Items.Add(bgc);

			DevComponents.DotNetBar.ButtonItem bi = new DevComponents.DotNetBar.ButtonItem();

			bi.Text = data.Name;

			bi.Tag = data.KeyName;

			this.btnShowMap.SubItems.Add(bi);
		}

		/// <summary>
		/// apply config color table
		/// </summary>
		private void loadBinColor()
		{
			//if (DataCenter._product != null)
			//{
			//    BinGradeColorSet.Initialize(DataCenter._product.TestCondition.TestItemArray, DataCenter._product.LOPSaveItem);
			//}

			Dictionary<string, TestResultData> resultDataList = new Dictionary<string, TestResultData>();

			if (DataCenter._product != null)
			{
				if (DataCenter._product.TestCondition.TestItemArray != null)
				{
					foreach (var testItem in DataCenter._product.TestCondition.TestItemArray)
					{
						if (testItem.MsrtResult == null || !testItem.IsEnable)
						{
							continue;
						}

						foreach (var resultItem in testItem.MsrtResult)
						{
							if (!resultItem.IsEnable || !resultItem.IsVision)
								continue;

							if (resultItem.KeyName.Contains("LOP") && !DataCenter._product.LOPSaveItem.ToString().Contains("mcd"))
								continue;

							if (resultItem.KeyName.Contains("WATT") && !DataCenter._product.LOPSaveItem.ToString().Contains("watt"))
								continue;

							if (resultItem.KeyName.Contains("LM") && !DataCenter._product.LOPSaveItem.ToString().Contains("lm"))
								continue;

							resultDataList.Add(resultItem.KeyName, resultItem);
						}
					}
				}
			}

			List<ColorSettingData> dataList = DataCenter._mapData.ColorSetting.DataList;

			for (int i = 0; i < dataList.Count; i++)
			{
				if (!resultDataList.ContainsKey(dataList[i].KeyName))
				{
					DataCenter._mapData.ColorSetting.Remove(dataList[i].KeyName);

					i--;
				}
			}

			foreach (var item in resultDataList)
			{
				if (DataCenter._mapData.ColorSetting.ContainsKey(item.Key))
				{
					DataCenter._mapData.ColorSetting[item.Key].IsEnable = item.Value.IsEnable;
				}
				else
				{
					ColorSettingData data = new ColorSettingData();

					data.IsEnable = item.Value.IsEnable;

					data.KeyName = item.Value.KeyName;

					data.Name = item.Value.Name;

					data.Formate = item.Value.Formate;

					DataCenter._mapData.ColorSetting.DataList.Add(data);
				}
			}

			BinGradeColorSet.Initialize(DataCenter._mapData);

			//DataCenter.SaveMapDataFile();
		}

		private void applyBinColor()
		{
			foreach (KeyValuePair<string, frmPopWaferMap> item in PopWaferFormBag)
			{
				if (this._colorBagSet.ContainsKey(item.Key))
				{
					frmPopWaferMap form = item.Value;
					form.ApplyColorSetting(this._colorBagSet[item.Key]);
				}
			}

			string symbol = MainWaferMapForm.WMSymbolId;

			if (this._colorBagSet.ContainsKey(symbol))
				MainWaferMapForm.ApplyColorSetting(this._colorBagSet[symbol]);
		}

		#endregion

		#region >>> Wafer Map Handler <<<

		private void btnArrangeWindow_Click(object sender, EventArgs e)
		{
			//if (DataCenter._uiSetting.IsEnableMapFormTopMost)
			//this.autoArrangeWindow();
		}

		#endregion

		#region >>> CIE Event Handler <<<

		private void ResetCIEChartEvent(object sender, EventArgs e)
		{
			this.CieChartPanel.Release(false);
		}

		private void SendCIExyDataEvent(object sender, ShowMapDataEventArgs e)
		{
			this.AddCIExy(e);

			this.CieChartPanel.Redraw();
		}

		#endregion

		#region >>> UI Event Handler <<<

		private void frmTestResult_Load(object sender, EventArgs e)
		{
			this.UpdateDataToControls();
			this.includeWaferMapForm();
			this.includeStatusForm();
			this.ResetDgvHistory();
			this.tabcResultInfo.SelectedTab = this.tabiHistory;

            this.includeBarcodeForm();
		}

		private void frmTestResult_VisibleChanged(object sender, EventArgs e)
		{
			Form form = FormAgent.RetrieveForm(typeof(frmTestResultInstantInfo));
			form.Parent = this;

			if (this.Visible == true)
			{
				form.Dock = DockStyle.None;

				form.Location = new Point(0, 780);
				form.Show();
				form.BringToFront();
				this.tabcResultInfo.SelectedTabIndex = 0;
			}
			else
			{
				form.Hide();
			}
		}

		private void PopWaferMap(string symbol)
		{
			if (!DataCenter._mapData.WeferMapShowItem.Contains(symbol))
			{
				return;
			}

			frmPopWaferMap form = null;
			if (PopWaferFormBag.ContainsKey(symbol))
			{
				form = PopWaferFormBag[symbol];
				if (form.IsDisposed || form.IsHandleCreated == false)
				{
					PopWaferFormBag.Remove(symbol);
					form = null;
					return;
				}
				else
				{
					form.Show();
				}
			}
			else
			{
				form = new frmPopWaferMap();
				form.WMSymbolId = symbol;
				form.TopMost = true;
				form.BringToFront();
				form.Show();

				int lift = DataCenter._uiSetting.WaferMapLeft;
				int top = DataCenter._uiSetting.WaferMapTop;
				int right = DataCenter._uiSetting.WaferMapRight;
				int bottom = DataCenter._uiSetting.WaferMapBottom;

				form.Reset(lift, top, right, bottom);


				if (!DataCenter._uiSetting.IsEnableMapFormTopMost)
				{
					form.TopMost = false;
				}

				//form.TopMost = false;

				PopWaferFormBag.Add(symbol, form);

				if (this._colorBagSet.ContainsKey(symbol))
				{
					form.ApplyColorSetting(this._colorBagSet[symbol]);
				}

			}

		}

		private void dgvResult_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			//if (DataCenter._sysSetting.IsAutoPopFourMapForm)
			//      return;

			//if (GlobalFlag.IsEnableShowMap == false || e.RowIndex < 0)
			//   return;

			//object tag = this.dgvResult.Rows[e.RowIndex].Tag;

			//if ( tag == null )
			//   return;

			//string symbol = tag.ToString();

			//   PopWaferMap(symbol);



			//AppSystem.ShowMapDataEvent += form.MsrtProcess_OnMapDataEvent;
		}

		public void autoStartPopWaferMap()
		{
			for (int i = 0; i < DataCenter._mapData.WeferMapShowItem.Count; i++)
			{
				PopWaferMap(DataCenter._mapData.WeferMapShowItem[i]);
			}

			autoArrangeWindow();
		}

		public void ShowSelectPopMap()
		{
			if (PopWaferFormBag.Count == DataCenter._mapData.WeferMapShowItem.Count)
			{
				return;
			}

			foreach (string mapKey in DataCenter._mapData.WeferMapShowItem)
			{
				frmPopWaferMap form = null;
				if (PopWaferFormBag.ContainsKey(mapKey))
				{
					form = PopWaferFormBag[mapKey];
					if (form.IsDisposed || form.IsHandleCreated == false)
					{
						PopWaferFormBag.Remove(mapKey);
						form = null;
						return;
					}
					else
					{
						form.Show();
					}
				}
				else
				{
					form = new frmPopWaferMap();
					form.WMSymbolId = mapKey;
					form.TopMost = false;

					form.TopLevel = false;

					//  form.Parent = this.plResult;

					form.Dock = DockStyle.None;


					form.BringToFront();
					form.Show();

					int lift = DataCenter._uiSetting.WaferMapLeft;
					int top = DataCenter._uiSetting.WaferMapTop;
					int right = DataCenter._uiSetting.WaferMapRight;
					int bottom = DataCenter._uiSetting.WaferMapBottom;

					form.Reset(lift, top, right, bottom);

					if (!DataCenter._uiSetting.IsEnableMapFormTopMost)
					{
						form.TopMost = false;
					}

					//form.TopMost = false;

					PopWaferFormBag.Add(mapKey, form);

					if (this._colorBagSet.ContainsKey(mapKey))
					{
						form.ApplyColorSetting(this._colorBagSet[mapKey]);
					}

				}
			}

		}

		private void cmbMsrtItem_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cmbMsrtItem.SelectedIndex < 0)
				return;

			BinGradeColor bgc = (BinGradeColor)this.cmbMsrtItem.SelectedValue;

			MainWaferMapForm.WMSymbolId = bgc.KeyName;
			MainWaferMapForm.ApplyColorSetting(bgc);
		}

		private void btnBinColor_Click(object sender, EventArgs e)
		{
			frmWaferMapSetting frmWaferMapSetting = new frmWaferMapSetting();

			frmWaferMapSetting.ShowDialog();

			frmWaferMapSetting.Dispose();

			frmWaferMapSetting.Close();

			Host.UpdateDataToAllUIForm();

			//frmBinColorSet frmBinColorSet = new frmBinColorSet(DataCenter._mapData.MapBackColor);
			//frmBinColorSet.ShowDialog();
			//DataCenter._mapData.MapBackColor = frmBinColorSet.MapBackColor();
			//frmBinColorSet.Dispose();
			//this.applyBinColor();
			//DataCenter.SaveUISettingToFile();
		}

		private void frmTestResult_Deactivate(object sender, EventArgs e)
		{
			Form form = FormAgent.RetrieveForm(typeof(frmTestResultInstantInfo));
			form.Hide();
		}

		private void cmbMsrtItem_Validating(object sender, CancelEventArgs e)
		{
			if (this.cmbMsrtItem.SelectedIndex < 0)
				return;

			BinGradeColor bgc = (BinGradeColor)this.cmbMsrtItem.SelectedValue;

			DataCenter._uiSetting.ShowMapKeyName = bgc.KeyName;
		}

		private void btnSnapshot_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Title = "Save As Wafer Map to Image";
			saveFileDialog.Filter = "PNG files (*.png)|*.png";//|jpg files (*.jpg)|*.jpg;|bmp files (*.bmp)|*.bmp";
			saveFileDialog.FilterIndex = 1;    // default value = 1

			if (saveFileDialog.ShowDialog() != DialogResult.OK)
				return;

			//this.MainWaferMapForm.SaveToImage(saveFileDialog.FileName);

            SaveMapImg(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);

            //Bitmap buffer = new Bitmap(this.pnlWaferMap.Width, this.pnlWaferMap.Height);

            //this.pnlWaferMap.DrawToBitmap(buffer, new Rectangle(0, 0, this.pnlWaferMap.Width, this.pnlWaferMap.Height));

            //buffer.Save(saveFileDialog.FileName);
		}

		private void btnSnapCIEChart_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();

			saveFileDialog.Title = "Save As CIE Chart to Image";

			saveFileDialog.Filter = "PNG files (*.png)|*.png";//|jpg files (*.jpg)|*.jpg;|bmp files (*.bmp)|*.bmp";

			saveFileDialog.FilterIndex = 1;    // default value = 1

			if (saveFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			Bitmap image = new Bitmap(CieChartPanel.Width, CieChartPanel.Height);

			CieChartPanel.SaveToImage(image);

			image.Save(saveFileDialog.FileName);

			image.Dispose();
		}

		private void btnSettingContinousRowCol_Click(object sender, EventArgs e)
		{
			DataCenter._uiSetting.IsManualSettingContiousProbingRowCol = this.chkIsDefineContinousRowCol.Checked;
			DataCenter._uiSetting.ContiousProbingPosX = (int)this.dinPosX.Value;
			DataCenter._uiSetting.ContiousProbingPosY = (int)this.dinPosY.Value;


			switch (DataCenter._sysSetting.ProberCoord)
			{
				case (int)ECoordSet.Second:
					DataCenter._uiSetting.ContiousProbingPosX *= (-1);
					break;
				case (int)ECoordSet.Third:
					DataCenter._uiSetting.ContiousProbingPosX *= (-1);
					DataCenter._uiSetting.ContiousProbingPosY *= (-1);
					break;
				case (int)ECoordSet.Fourth:
					DataCenter._uiSetting.ContiousProbingPosY *= (-1);
					break;
				default:
					break;
			}

			switch (DataCenter._sysSetting.TesterCoord)
			{
				case (int)ECoordSet.Second:
					DataCenter._uiSetting.ContiousProbingPosX *= (-1);
					break;
				case (int)ECoordSet.Third:
					DataCenter._uiSetting.ContiousProbingPosX *= (-1);
					DataCenter._uiSetting.ContiousProbingPosY *= (-1);
					break;
				case (int)ECoordSet.Fourth:
					DataCenter._uiSetting.ContiousProbingPosY *= (-1);
					break;
				default:
					break;
			}
		}

		private void btnContinousProbing_Click(object sender, EventArgs e)
		{
			DialogResult result = TopMessageBox.ShowMessage((int)EMessageCode.linkupProbing, " 確認接續點測 ? ", "Qusetion");
			//   DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show(" Would you want to reset system state ? ", "Warn", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
			if (result != DialogResult.OK)
				return;

			if (!Directory.Exists(Constants.Paths.MPI_TEMP_DIR2))
			{
				Directory.CreateDirectory(Constants.Paths.MPI_TEMP_DIR2);
			}


			bool isOk = true;

			string datfileNameWithExt = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, DataCenter._uiSetting.TestResultFileName + ".dat");

			string backupDatfileNameWithExt = Path.Combine(Constants.Paths.MPI_TEMP_DIR2, DataCenter._uiSetting.TestResultFileName + ".dat");


			if (File.Exists(datfileNameWithExt))
			{
				if (File.Exists(backupDatfileNameWithExt))
				{
					File.Delete(backupDatfileNameWithExt);
				}

				MPIFile.CopyFile(datfileNameWithExt, backupDatfileNameWithExt);

				isOk &= true;
			}

			// DataCenter.MoveDATFileToBackup();

			string outPath01 = DataCenter._uiSetting.TestResultPath01;

			string fileNameWithExt = DataCenter._uiSetting.TestResultFileName + "." + DataCenter._uiSetting.TestResultFileExt;

			if (DataCenter._uiSetting.TesterResultCreatFolderType01 == ETesterResultCreatFolderType.ByLotNumber)
			{
				outPath01 = Path.Combine(outPath01, DataCenter._uiSetting.LotNumber);
			}
			else if (DataCenter._uiSetting.TesterResultCreatFolderType01 == ETesterResultCreatFolderType.ByMachineName)
			{
				outPath01 = Path.Combine(outPath01, DataCenter._uiSetting.MachineName);
			}
			else if (DataCenter._uiSetting.TesterResultCreatFolderType01 == ETesterResultCreatFolderType.ByDataTime)
			{
				outPath01 = Path.Combine(outPath01, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
			}

			string backupFileNameWithExt = DataCenter._uiSetting.TestResultFileName + "_" + DateTime.Now.ToString("yyMMddhhmmss") + "." + DataCenter._uiSetting.TestResultFileExt;

			backupFileNameWithExt = Path.Combine(outPath01, backupFileNameWithExt);

			outPath01 = Path.Combine(outPath01, fileNameWithExt);


			if (File.Exists(outPath01) && DataCenter._uiSetting.IsEnablePath01)
			{
				File.Copy(outPath01, backupFileNameWithExt);

				File.Delete(outPath01);

				isOk &= true;
			}

		}

		private void btnImport_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();

			if (openFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			if (ReportProcess.IsImplement)
			{
				if (!ReportProcess.OpenReport(openFileDialog.FileName))
				{
					DevComponents.DotNetBar.MessageBoxEx.Show("File is not Correct", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

					return;
				}

				/////////////////////////////////////////////////////
				// Add Data To map And CIE Chart
				/////////////////////////////////////////////////////
				if (sender == this.btnImportCIE)
				{
					this.ResetCIEChartEvent(null, null);

					while (true)
					{
						Dictionary<string, float> result = ReportProcess.ReadLine();

						if (result == null)
						{
							break;
						}

						int row = (int)result["ROW"];

						int col = (int)result["COL"];

						ShowMapDataEventArgs map_data = new ShowMapDataEventArgs(row, col, new Dictionary<string, float>(result));

						this.AddCIExy(map_data);
					}
				}
				else if (sender == this.btnImportMap)
				{
					this.MainWaferMapForm.Reset();

					this.MainWaferMapForm.DynamicMode = false;

					while (true)
					{
						Dictionary<string, float> result = ReportProcess.ReadLine();

						if (result == null)
						{
							break;
						}

						int row = (int)result["ROW"];

						int col = (int)result["COL"];

						//DataCenter.ChangeMapRowColOfTester(ref col, ref row);

						this.MainWaferMapForm.AddWaferDieFromFile(row, col, result);
					}

					this.MainWaferMapForm.WaferMap.SetLayout(frmWaferMap.WaferDB.Boundary);

					DataCenter._uiSetting.WaferMapLeft = frmWaferMap.WaferDB.Boundary.Left;

					DataCenter._uiSetting.WaferMapTop = frmWaferMap.WaferDB.Boundary.Top;

					DataCenter._uiSetting.WaferMapRight = frmWaferMap.WaferDB.Boundary.Right;

					DataCenter._uiSetting.WaferMapBottom = frmWaferMap.WaferDB.Boundary.Bottom;

					this.MainWaferMapForm.DrawAllWaferDie();

					this.MainWaferMapForm.DynamicMode = true;

					foreach (frmPopWaferMap form in PopWaferFormBag.Values)
					{
						if (form.IsDisposed || form.Disposing)
						{
							continue;
						}

						form.Reset();
					}
				}
			}
			else
			{
			List<string[]> report = CSVUtil.ReadCSV(openFileDialog.FileName);

			Dictionary<string, string> msrtDic = DataCenter._uiSetting.UserDefinedData.MsrtDisplayItemDic;

			Dictionary<string, string> resultDic = DataCenter._uiSetting.UserDefinedData.ResultItemNameDic;

			Dictionary<string, int> keyAndCol = new Dictionary<string, int>();

			/////////////////////////////////////////////////////
			// Find float compare string
			/////////////////////////////////////////////////////
			string floatCompare = string.Empty;

			List<string> sysKeyItem = new List<string>() { "TEST", "BIN", "POLAR", "ISALLPASS", "ROW", "COL" };

			foreach (var item in sysKeyItem)
			{
				if (msrtDic.ContainsKey(item))
				{
					floatCompare += msrtDic[item];
				}
				else
				{
					floatCompare += item;
				}
			}

			/////////////////////////////////////////////////////
			// Find fixed compare string
			/////////////////////////////////////////////////////
			string fixedCompare = string.Empty;

			foreach (var item in resultDic)
			{
				string[] lopSaveItem = DataCenter._product.LOPSaveItem.ToString().Split('_');

				if (item.Key.Contains("LOP"))
				{
					if (!lopSaveItem.Contains("mcd"))
					{
						continue;
					}
				}

				if (item.Key.Contains("WATT"))
				{
					if (!lopSaveItem.Contains("watt"))
					{
						continue;
					}
				}

				if (item.Key.Contains("LM"))
				{
					if (!lopSaveItem.Contains("lm"))
					{
						continue;
					}
				}

				fixedCompare += item.Value;
			}

			/////////////////////////////////////////////////////
			// Find Title Index
			/////////////////////////////////////////////////////
			int titleIndex = -1;

			string firstName = "TEST";

			if (msrtDic.ContainsKey(firstName))
			{
				firstName = msrtDic[firstName];
			}

			for (int row = 0; row < report.Count; row++)
			{
				if (report[row][0] == firstName)
				{
					string rowLine = string.Empty;

					foreach (var item in report[row])
					{
						rowLine += item;
					}

					if (rowLine.Contains(floatCompare))
					{
						floatCompare = "true";

						titleIndex = row;

						break;
					}

					if (rowLine == fixedCompare)
					{
						fixedCompare = "true";

						titleIndex = row;

						break;
					}
				}
			}

			if (titleIndex < 0)
			{
				DevComponents.DotNetBar.MessageBoxEx.Show("File is not Correct", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

				return;
			}

			/////////////////////////////////////////////////////
			// Find Report Title KeyName Item And Column Index
			/////////////////////////////////////////////////////
			if (floatCompare == "true")
			{
				for (int col = 0; col < report[titleIndex].Length; col++)
				{
					string itemName = report[titleIndex][col];

					if (msrtDic.ContainsValue(itemName))
					{
						foreach (var item in msrtDic)
						{
							if (item.Value == itemName)
							{
								keyAndCol.Add(item.Key, col);
							}
						}
					}
					else
					{
						for (int numIndex = 0; numIndex < itemName.Length; numIndex++)
						{
							int temp;

							if (int.TryParse(itemName[numIndex].ToString(), out temp))
							{
								string keyName = itemName.Insert(numIndex, "_");

								keyAndCol.Add(keyName, col);

								break;
							}
						}
					}
				}
			}
			else if (fixedCompare == "true")
			{
				int index = 0;

				string[] lopSaveItem = DataCenter._product.LOPSaveItem.ToString().Split('_');

				foreach (var item in resultDic)
				{
					if (item.Key.Contains("LOP"))
					{
						if (!lopSaveItem.Contains("mcd"))
						{
							continue;
						}
					}

					if (item.Key.Contains("WATT"))
					{
						if (!lopSaveItem.Contains("watt"))
						{
							continue;
						}
					}

					if (item.Key.Contains("LM"))
					{
						if (!lopSaveItem.Contains("lm"))
						{
							continue;
						}
					}

					keyAndCol.Add(item.Key, index);

					index++;
				}
			}

			/////////////////////////////////////////////////////
			// Add Data To map And CIE Chart
			/////////////////////////////////////////////////////
			this.progressBar.Location = new Point(700, 300);

			this.progressBar.Size = new Size(469, 23);

			this.progressBar.Maximum = report.Count;

			this.progressBar.Minimum = 0;

			this.progressBar.Visible = true;


            PreSamplingCheck pre = null;

            if (DataCenter._uiSetting.UserDefinedData.IsShowPreSamplingChecking)
            {
                pre = new PreSamplingCheck();

                pre.Start(DataCenter._uiSetting, DataCenter._product);
            }
            else
            {
                pre = null;
            }

				if (sender == this.btnImportCIE)
			{
				this.ResetCIEChartEvent(null, null);

				for (int line = titleIndex + 1; line < report.Count; line++)
				{
					Dictionary<string, float> result = new Dictionary<string, float>();

					foreach (var item in keyAndCol)
					{
						float value = 0.0f;

						if (float.TryParse(report[line][item.Value], out value))
						{
							result.Add(item.Key, value);
						}
					}

					int row = int.Parse(report[line][keyAndCol["ROW"]]);

					int col = int.Parse(report[line][keyAndCol["COL"]]);

					ShowMapDataEventArgs map_data = new ShowMapDataEventArgs(row, col, new Dictionary<string, float>(result));

					this.AddCIExy(map_data);

					this.progressBar.Value = line;
				}
			}
				else if (sender == this.btnImportMap)
			{
				this.MainWaferMapForm.Reset();

				this.MainWaferMapForm.DynamicMode = false;

				for (int line = titleIndex + 1; line < report.Count; line++)
				{
					Dictionary<string, float> result = new Dictionary<string, float>();

					foreach (var item in keyAndCol)
					{
						float value = 0.0f;

						if (float.TryParse(report[line][item.Value], out value))
						{
							result.Add(item.Key, value);
						}
					}

					int row = int.Parse(report[line][keyAndCol["ROW"]]);

					int col = int.Parse(report[line][keyAndCol["COL"]]);

					//DataCenter.ChangeMapRowColOfTester(ref col, ref row);

                    if (pre!=null)
                    {
                        pre.Push(col, row, result);
                    }
                   
					this.MainWaferMapForm.AddWaferDieFromFile(row, col, result);

					this.progressBar.Value = line;
				}

                if (pre != null)
                {
                    pre.End();
                }

				this.MainWaferMapForm.WaferMap.SetLayout(frmWaferMap.WaferDB.Boundary);

				DataCenter._uiSetting.WaferMapLeft = frmWaferMap.WaferDB.Boundary.Left;

				DataCenter._uiSetting.WaferMapTop = frmWaferMap.WaferDB.Boundary.Top;

				DataCenter._uiSetting.WaferMapRight = frmWaferMap.WaferDB.Boundary.Right;

				DataCenter._uiSetting.WaferMapBottom = frmWaferMap.WaferDB.Boundary.Bottom;

				this.MainWaferMapForm.DrawAllWaferDie();

				this.progressBar.Visible = false;

				this.MainWaferMapForm.DynamicMode = true;

				foreach (frmPopWaferMap form in PopWaferFormBag.Values)
				{
					if (form.IsDisposed || form.Disposing)
					{
						continue;
					}

					form.Reset();
				}
			}

			this.progressBar.Visible = false;
			}
		}

		private void WaferMap_DieClick(int row, int col)
		{
			ColRow selectCH1ColRow = ReportProcess.GetChannel1ColRow(col, row);

			if(selectCH1ColRow != null)
			{
				this.dinPosX.Value = selectCH1ColRow.Col;

				this.dinPosY.Value = selectCH1ColRow.Row;
			}
			else
			{
				this.dinPosX.Value = col;

				this.dinPosY.Value = row;
			}
		}

		#endregion
	}
}