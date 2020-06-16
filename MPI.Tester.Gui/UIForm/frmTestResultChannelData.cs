using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data;
using MPI.Windows.Forms;
using MPI.Tester.Data;
using MPI.Tester.Report;

namespace MPI.Tester.Gui
{
    public partial class frmTestResultChannelData : Form
    {
        private delegate void ResetTestItemEvent(int channel, TestResultData trd, int idx);

        private event ResetTestItemEvent OnEachTestItem;

        private event MethodInvoker OnResetTestItem;

        private DataGridViewCellStyle _highlightCellStyle = new DataGridViewCellStyle();
        private DataGridViewCellStyle _normalCellStyle = new DataGridViewCellStyle();
        private DataGridViewCellStyle _errorCellStyle = new DataGridViewCellStyle();
        private DataGridViewCellStyle _emptyCellStyle = new DataGridViewCellStyle();
        private DataGridViewCellStyle _disableCellStyle = new DataGridViewCellStyle();
        
        public frmTestResultChannelData()
        {
            InitializeComponent();

            Host.OnTestItemChangeEvent += new EventHandler(this.OnTestItemChang);

			AppSystem.ShowMapDataEvent += new EventHandler<ShowMapDataEventArgs>(this.UpdatedgvHistory);

            OnResetTestItem += DataGrid_OnResetTestItem;

            OnEachTestItem += DataGrid_OnEachTestItem;

            initDataGrid();

            RegisterUpdateEvent();
        }

		private DataTable _dgvHistoryDataTable;
		private int _dgvItemCount;

        #region >>>  Private Method <<<

        private void OnTestItemChang(object o, EventArgs e)
        {
            resetTestItemData();
        }

        private void RegisterUpdateEvent()
        {
            frmTestResultInstantInfo form = (frmTestResultInstantInfo)FormAgent.RetrieveForm(typeof(frmTestResultInstantInfo));

            form.RegisterUpdateEvent(this.updateResultDataGrid);
        }

        private void DataGrid_OnResetTestItem()
        {
            this.dgvResult.Rows.Clear();
            this.dgvResult.Columns["colValueChannel1"].Visible = true;
            this.dgvResult.Columns["colValueChannel2"].Visible = true;
            this.dgvResult.Columns["colValueChannel3"].Visible = false;
            this.dgvResult.Columns["colValueChannel4"].Visible = false;
            this.dgvResult.Columns["colValueChannel5"].Visible = false;
            this.dgvResult.Columns["colValueChannel6"].Visible = false;
            this.dgvResult.Columns["colValueChannel7"].Visible = false;
            this.dgvResult.Columns["colValueChannel8"].Visible = false;
            this.dgvResult.Columns["colValueChannel9"].Visible = false;
        }

        private void DataGrid_OnEachTestItem(int channel, TestResultData resultData, int index)			// index : 0-base
        {
            if (resultData.IsEnable == false || resultData.IsVision == false)
                return;

            // show channel col
            string colKeyName = "colValueChannel" + (channel + 1).ToString();

            if (this.dgvResult.Columns[colKeyName].Visible == false)
            {
                this.dgvResult.Columns[colKeyName].Visible = true;
            }

            if (channel == 0)
            {
                this.dgvResult.Rows.Add();
     
                this.dgvResult["colNo", index].Value = index + 1;  // 1-base
                this.dgvResult["colName", index].Value = resultData.Name;
                this.dgvResult["colUnit", index].Value = resultData.Unit;
                this.dgvResult.Rows[index].Tag = resultData.KeyName;
            }

            string valStr = resultData.IsTested ? resultData.Value.ToString(resultData.Formate) : "----";

            this.dgvResult[colKeyName, index].Value = valStr;         // row := 0-base	

			this._dgvItemCount++;									
        }

        private void initDataGrid()
        {
            this._highlightCellStyle.BackColor = Color.LightBlue;
            this._normalCellStyle.BackColor = Color.White;
            this._errorCellStyle.BackColor = Color.Orange;
            this._emptyCellStyle.BackColor = Color.Empty;
            this._disableCellStyle.BackColor = Color.LightGray;
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
            this.dgvResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

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

            try
            {
                if (DataCenter._tempCond.TestItemArray == null || DataCenter._tempCond.TestItemArray.Length == 0)
                    return;

                int row = 0;
                int dispRowIndex = 0;
                int testItemIndex = 0;
                int resultItemIndex = 0;

                DataGridViewRow dgrow;

                this.dgvResult.SuspendLayout();

                string colKeyName = string.Empty;

                for (int channel = 0; channel < DataCenter._tempCond.ChannelConditionTable.Count; channel++)
                {
                    dispRowIndex = 0;
                    testItemIndex = 0;
                    row = 0;

                    colKeyName = "colValueChannel" + (channel + 1).ToString();

                    foreach (TestItemData mainItem in DataCenter._tempCond.TestItemArray)
                    {
                        if (mainItem.MsrtResult != null)
                        {
                            foreach (TestResultData data in DataCenter._tempCond.ChannelConditionTable.Channels[channel].Conditions[testItemIndex].MsrtResult)
                            {
                                //---------------------------------------------------------------------
                                // Update the result data on the "tabResult"
                                //---------------------------------------------------------------------
                                if (mainItem.IsEnable == true && data.IsEnable == true && data.IsVision == true)
                                {
                                    if (this.tabpResult.Visible == true)
                                    {
                                        dgrow = this.dgvResult.Rows[row];

                                        if (dgrow != null)
                                        {
                                            if (testItemIndex % 2 == 0)
                                            {
                                                dgrow.DefaultCellStyle = this._normalCellStyle;
                                            }
                                            else
                                            {
                                                dgrow.DefaultCellStyle = this._highlightCellStyle;
                                            }

                                            // dgrow.HeaderCell.Value = (row + 1).ToString();
                                            dgrow.Cells[0].Value = (dispRowIndex + 1).ToString();  // NO


                                            if (DataCenter._tempCond.ChannelConditionTable.Channels[channel].Conditions[testItemIndex].IsEnable)
                                            {
                                                string valStr = data.IsTested ? data.Value.ToString(data.Formate) : "----";
                                                dgrow.Cells[colKeyName].Value = valStr;

                                                if (data.IsPass == false)
                                                {
                                                    dgrow.Cells[colKeyName].Style = this._errorCellStyle;
                                                }
                                                else
                                                {
                                                    dgrow.Cells[colKeyName].Style = this._emptyCellStyle;
                                                }
                                            }
                                            else
                                            {
                                                dgrow.Cells[colKeyName].Value = string.Empty;
                                                dgrow.Cells[colKeyName].Style = this._disableCellStyle;
                                            }
                                        }


                                        if (this.dgvResult.Rows[row].Visible == true)
                                        {
                                            dispRowIndex++;
                                        }

                                        row++;
                                    }
                                }

                                resultItemIndex++;              // It must count the all "TestResultData", even the "TestItem" is Disable
                            }
                        }

                        testItemIndex++;
                    }
                }

                this.dgvResult.ResumeLayout();
            }
            catch (Exception exc)
            {
                Console.WriteLine("[frmTestResultChannelData],updateResultDataGrid(),Exception:" + exc.Message);
            }
        }

        private void frmTestResultChannelData_Load(object sender, EventArgs e)
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

            if (DataCenter._product.TestCondition.ChannelConditionTable == null || DataCenter._product.TestCondition.ChannelConditionTable.Channels == null)
            {
                this.DataGrid_OnResetTestItem();

                return;
            }

            int index = 0;

            for (int channel = 0; channel < DataCenter._product.TestCondition.ChannelConditionTable.Count; channel++)
            {
                index = 0;

                foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
                {
                    if (item.IsEnable == true && item.MsrtResult != null)
                    {
                        foreach (TestResultData resultData in item.MsrtResult)
                        {
                            if (resultData.IsVision && resultData.IsEnable)
                            {
                                if (channel == 0 && index == 0)
                                {
                                    this.OnResetTestItem.Invoke();
                                }

                                this.OnEachTestItem.Invoke(channel, resultData, index);

                                index++;
                            }
                        }

                    }
                }
            }

			this._dgvItemCount = 0;
        }

        #endregion

        private void dgvResult_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;

            e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
        }
    }
}
