using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Linq;

using System.Data;
using MPI.Windows.Forms;
using MPI.Tester.Data;
using MPI.Tester.Report;

namespace MPI.Tester.Gui
{
    public partial class frmTestResultData : Form
    {
        private delegate void ResetTestItemEvent(TestResultData trd, int idx);

        private event ResetTestItemEvent OnEachTestItem;

        private event MethodInvoker OnResetTestItem;

        private DataGridViewCellStyle _highlightCellStyle = new DataGridViewCellStyle();
        private DataGridViewCellStyle _normalCellStyle = new DataGridViewCellStyle();
        private DataGridViewCellStyle _errorCellStyle = new DataGridViewCellStyle();
        private DataGridViewCellStyle _emptyCellStyle = new DataGridViewCellStyle();

        private delegate void UpdateDataHandler();

		private DataTable _dgvHistoryDataTable;
		private int _dgvItemCount;

        public frmTestResultData()
        {
            InitializeComponent();

			Host.OnTestItemChangeEvent += new EventHandler(this.OnTestItemChang);

			AppSystem.ShowMapDataEvent += new EventHandler<ShowMapDataEventArgs>(this.UpdatedgvHistory);

            OnResetTestItem += DataGrid_OnResetTestItem;

            OnEachTestItem += DataGrid_OnEachTestItem;

            initDataGrid();

            RegisterUpdateEvent();
        }

        #region >>>  Private Method <<<

		private void OnTestItemChang(object o, EventArgs e)
		  {   
		     resetTestItemData();

			this.ResetDgvHistory();
		  }

        private void RegisterUpdateEvent()
        {
            frmTestResultInstantInfo form = (frmTestResultInstantInfo)FormAgent.RetrieveForm(typeof(frmTestResultInstantInfo));

            form.RegisterUpdateEvent(this.updateResultDataGrid);
        }

        private void DataGrid_OnResetTestItem()
        {
            this.dgvResult.Rows.Clear();
            this.dgvStat01.Rows.Clear();
            this.dgvStat02.Rows.Clear();
            this.dgvStat03.Rows.Clear();
			this._dgvItemCount = 0;
        }

        private void DataGrid_OnEachTestItem(TestResultData resultData, int index)			// index : 0-base
        {
			if (resultData.KeyName == ESysResultItem.BIN.ToString() || resultData.IsEnable == false || resultData.IsVision == false)
                return;

            this.dgvStat01.Rows.Add(index + 1, resultData.Name);
            this.dgvStat02.Rows.Add(index + 1, resultData.Name);
            this.dgvStat03.Rows.Add(index + 1, resultData.Name);
            string valStr = resultData.IsTested ? resultData.Value.ToString(resultData.Formate) : "----";
            this.dgvResult.Rows.Add(index + 1, resultData.Name, valStr, resultData.Unit);		// 1-base
			
            this.dgvResult.Rows[index].Tag = resultData.KeyName;																			// row := 0-base	

			this._dgvItemCount++;
        }

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
            this.dgvResult.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;

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
			this._dgvHistoryDataTable = new DataTable();
			this.dgvHistory.DataSource = this._dgvHistoryDataTable;
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
			this.dgvHistory.SelectionMode = DataGridViewSelectionMode.CellSelect;
			this.dgvHistory.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        private void updateResultDataGrid(object sender, EventArgs e)
        {
            if (this.Visible == false)
            {
                return;
            }

            //因此處使用timer 切換，若切換時剛好遇到recipe中切換會導致當機
            try
            {


                if (DataCenter._tempCond.TestItemArray == null || DataCenter._tempCond.TestItemArray.Length == 0)
                    return;

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
                Console.WriteLine("[frmTestResultData],updateResultDataGrid(),Exception:" + exc.Message);
            }
        }

        private void frmTestResultData_Load(object sender, EventArgs e)
        {
            resetTestItemData();

            this.ResetDgvHistory();

            //this.tabpResult.Width = this.dgvResult.Width ;

           // this.tabcResultInfo.Width = this.dgvResult.Width ;

         //   this.Width = this.dgvResult.Width;

            this.tabcResultInfo.SelectedPanel = this.tabpResult;

            //this.chkIsShowResult.Checked = true;

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

			this._dgvHistoryDataTable.Columns.Add("BIN", typeof(string));

			this._dgvHistoryDataTable.Columns["TEST"].Caption = "TEST";

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
				if (this.dgvHistory.Columns[i].Name == "TEST")
				{
					this.dgvHistory.Columns[i].Width = 50;
				}
				else if (this.dgvHistory.Columns[i].Name == "BIN")
				{
					this.dgvHistory.Columns[i].Width = 40;
				}
				else
				{
					this.dgvHistory.Columns[i].Width = 100;
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

			if (DataCenter._sysSetting.IsCountPassFailByBinGrade)
			{
				int binSN = (int)e.Values[ESysResultItem.BINSN.ToString()];

				if (DataCenter._smartBinning.GetBinFromSN(binSN) != null)
				{
					if (DataCenter._smartBinning.GetBinFromSN(binSN).BinningType != EBinningType.IN_BIN)
					{
						int index = this._dgvHistoryDataTable.Rows.IndexOf(dataRow);

						this.dgvHistory.Rows[index].DefaultCellStyle.BackColor = this._errorCellStyle.BackColor;
					}
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
			}

			if (this._dgvHistoryDataTable.Rows.Count > 1000)
			{
				this._dgvHistoryDataTable.Rows.RemoveAt(0);
			}

			if (this.dgvHistory.Rows.Count > 0)
			{
				this.dgvHistory.CurrentCell = this.dgvHistory[0, this.dgvHistory.Rows.Count - 1];
			}

			this.dgvHistory.ResumeLayout();
		}

        private void resetTestItemData()
        {
            // Set tempCond, when no evet is fired
            DataCenter._tempCond = DataCenter._product.TestCondition.Clone() as ConditionData;

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
                            }

                            this.OnEachTestItem.Invoke(resultData, index);

                            index++;
                        }
                    }

                }
            }
        }

        #endregion

        #region >>> UI Event Handler <<<

        private void tabcResultInfo_Click(object sender, EventArgs e)
        {

        }

        private void chkIsShowResult_CheckedChanged(object sender, EventArgs e)
        {
            this.tabpResult.Width = this.dgvResult.Width ;

            this.tabcResultInfo.Width = this.dgvResult.Width ;

            this.Width = this.dgvResult.Width;

            this.tabcResultInfo.SelectedPanel = this.tabpResult;
        }

        private void chkIsEnableShowStatistic01_CheckedChanged(object sender, EventArgs e)
        {
            this.tabcResultInfo.SelectedPanel = this.tabpStat02;
        }

        private void chkIsEnableShowBin_CheckedChanged(object sender, EventArgs e)
        {
            this.tabcResultInfo.SelectedPanel = this.tabpBin;
        }

        private void chkIsEnableShowBin_CheckedChanged_1(object sender, EventArgs e)
        {
            this.tabpResult.Width = this.lstvBinGrade.Width;

            this.tabcResultInfo.Width = this.lstvBinGrade.Width ;

            this.Width = this.lstvBinGrade.Width ;

            this.tabcResultInfo.SelectedPanel = this.tabpBin;
        }

        private void chkEnableAllStastic_CheckedChanged_1(object sender, EventArgs e)
        {
            this.tabpResult.Width = this.dgvStat01.Width ;

            this.tabcResultInfo.Width = this.dgvStat01.Width ;

            this.Width = this.dgvStat01.Width ;

            this.tabcResultInfo.SelectedPanel = this.tabpStat01;
        }

        private void chkIsShowResult_CheckedChanged_1(object sender, EventArgs e)
        {
            this.tabpResult.Width = this.dgvResult.Width;

            this.tabcResultInfo.Width = this.dgvResult.Width;

            this.Width = this.dgvResult.Width;

            this.tabcResultInfo.SelectedPanel = this.tabpResult;
        }

        #endregion
    }
}
