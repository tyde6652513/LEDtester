using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using MPI.Tester.Data;
using MPI.Tester;
using MPI.UCF.Forms.Domain;
using DevComponents.DotNetBar.Controls;

namespace MPI.Tester.Gui
{
	public partial class frmBinSetting : System.Windows.Forms.Form
	{
		private delegate void UpdateDataHandler();
		private SmartBinning _smartBinning;
        private EnableItemsCollection _enableMsrtItem;

		public frmBinSetting()
		{
			InitializeComponent();

			this._smartBinning = new SmartBinning();

            this._enableMsrtItem = new EnableItemsCollection();

			//Wait For UI Event: true
			this.chkBoundaryAll.Tag = true;

			this.cmbSelectCIE.Font = new Font("Microsoft JhengHei", 12);

			this.InitBinTableDGV();
		}

        #region >>> Event Handler <<<

        #endregion

        #region >>> Private Method <<<

		private void InitBinTableDGV()
		{
			//BoundarySelectDGV
			this.dgvEnableBoundary.DataSource = null;
			this.dgvEnableBoundary.ReadOnly = false;
			this.dgvEnableBoundary.MultiSelect = true;
			this.dgvEnableBoundary.AllowUserToAddRows = false;
			this.dgvEnableBoundary.AllowUserToDeleteRows = false;
			this.dgvEnableBoundary.AllowUserToResizeRows = false;
			this.dgvEnableBoundary.AllowUserToResizeColumns = false;
			this.dgvEnableBoundary.AllowUserToOrderColumns = false;
			this.dgvEnableBoundary.EditMode = DataGridViewEditMode.EditOnKeystroke;
			this.dgvEnableBoundary.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvEnableBoundary.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvEnableBoundary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			this.dgvEnableBoundary.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			this.dgvEnableBoundary.SelectionMode = DataGridViewSelectionMode.CellSelect;
			this.dgvEnableBoundary.Font = new Font("Microsoft JhengHei", 10);

			for (int col = 0; col < this.dgvEnableBoundary.ColumnCount; col++)
			{
				this.dgvEnableBoundary.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
			}

			//Boundary2DSelectDGV
			this.dgvEnableBoundary2D.DataSource = null;
			this.dgvEnableBoundary2D.ReadOnly = false;
			this.dgvEnableBoundary2D.MultiSelect = true;
			this.dgvEnableBoundary2D.AllowUserToAddRows = false;
			this.dgvEnableBoundary2D.AllowUserToDeleteRows = false;
			this.dgvEnableBoundary2D.AllowUserToResizeRows = false;
			this.dgvEnableBoundary2D.AllowUserToResizeColumns = false;
			this.dgvEnableBoundary2D.AllowUserToOrderColumns = false;
			this.dgvEnableBoundary2D.EditMode = DataGridViewEditMode.EditOnKeystroke;
			this.dgvEnableBoundary2D.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvEnableBoundary2D.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvEnableBoundary2D.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			this.dgvEnableBoundary2D.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			this.dgvEnableBoundary2D.SelectionMode = DataGridViewSelectionMode.CellSelect;
			this.dgvEnableBoundary2D.Font = new Font("Microsoft JhengHei", 10);

			for (int col = 0; col < this.dgvEnableBoundary2D.ColumnCount; col++)
			{
				this.dgvEnableBoundary2D.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
			}

			//BoundaryDGV
			this.dgvBoundary.DataSource = null;
			this.dgvBoundary.ReadOnly = false;
			this.dgvBoundary.MultiSelect = true;
			this.dgvBoundary.AllowUserToAddRows = false;
			this.dgvBoundary.AllowUserToDeleteRows = false;
			this.dgvBoundary.AllowUserToResizeRows = false;
			this.dgvBoundary.AllowUserToResizeColumns = false;
			this.dgvBoundary.AllowUserToOrderColumns = false;
			this.dgvBoundary.EditMode = DataGridViewEditMode.EditOnKeystroke;
			this.dgvBoundary.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvBoundary.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvBoundary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			this.dgvBoundary.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			this.dgvBoundary.SelectionMode = DataGridViewSelectionMode.CellSelect;
			this.dgvBoundary.Font = new Font("Microsoft JhengHei", 10);

			//Boundary2DDGV
			this.dgvBoundary2D.DataSource = null;
			this.dgvBoundary2D.ReadOnly = false;
			this.dgvBoundary2D.MultiSelect = true;
			this.dgvBoundary2D.AllowUserToAddRows = false;
			this.dgvBoundary2D.AllowUserToDeleteRows = false;
			this.dgvBoundary2D.AllowUserToResizeRows = false;
			this.dgvBoundary2D.AllowUserToResizeColumns = false;
			this.dgvBoundary2D.AllowUserToOrderColumns = false;
			this.dgvBoundary2D.EditMode = DataGridViewEditMode.EditOnKeystroke;
			this.dgvBoundary2D.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvBoundary2D.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvBoundary2D.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			this.dgvBoundary2D.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			this.dgvBoundary2D.SelectionMode = DataGridViewSelectionMode.CellSelect;
			this.dgvBoundary2D.Font = new Font("Microsoft JhengHei", 10);

			//BinTableDGV
			this.dgvBinTable.DataSource = null;
			this.dgvBinTable.ReadOnly = false;
			this.dgvBinTable.MultiSelect = true;
			this.dgvBinTable.AllowUserToAddRows = false;
			this.dgvBinTable.AllowUserToDeleteRows = false;
			this.dgvBinTable.AllowUserToResizeRows = false;
			this.dgvBinTable.AllowUserToResizeColumns = false;
			this.dgvBinTable.AllowUserToOrderColumns = false;
			this.dgvBinTable.EditMode = DataGridViewEditMode.EditOnKeystroke;
			this.dgvBinTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvBinTable.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvBinTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			this.dgvBinTable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			this.dgvBinTable.SelectionMode = DataGridViewSelectionMode.CellSelect;
			this.dgvBinTable.Font = new Font("Microsoft JhengHei", 10);

			//NGBinTableDGV
			this.dgvNGBin.Columns[0].ValueType = typeof(bool);
			this.dgvNGBin.Columns[1].ValueType = typeof(string);
			this.dgvNGBin.Columns[2].ValueType = typeof(string);
			this.dgvNGBin.Columns[3].ValueType = typeof(int);
			this.dgvNGBin.Columns[4].ValueType = typeof(double);
			this.dgvNGBin.Columns[5].ValueType = typeof(double);
			this.dgvNGBin.DataSource = null;
			this.dgvNGBin.ReadOnly = false;
			this.dgvNGBin.MultiSelect = true;
			this.dgvNGBin.AllowUserToAddRows = false;
			this.dgvNGBin.AllowUserToDeleteRows = false;
			this.dgvNGBin.AllowUserToResizeRows = false;
			this.dgvNGBin.AllowUserToResizeColumns = false;
			this.dgvNGBin.AllowUserToOrderColumns = false;
			this.dgvNGBin.EditMode = DataGridViewEditMode.EditOnKeystroke;
			this.dgvNGBin.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvNGBin.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvNGBin.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			this.dgvNGBin.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			this.dgvNGBin.SelectionMode = DataGridViewSelectionMode.CellSelect;
			this.dgvNGBin.Font = new Font("Microsoft JhengHei", 10);

			for (int col = 0; col < this.dgvNGBin.ColumnCount; col++)
			{
				this.dgvNGBin.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
			}

			//SideBinTableDGV
			this.dgvSideBin.Columns[0].ValueType = typeof(string);
			this.dgvSideBin.Columns[1].ValueType = typeof(string);
			this.dgvSideBin.Columns[2].ValueType = typeof(int);
			this.dgvSideBin.DataSource = null;
			this.dgvSideBin.ReadOnly = false;
			this.dgvSideBin.MultiSelect = true;
			this.dgvSideBin.AllowUserToAddRows = false;
			this.dgvSideBin.AllowUserToDeleteRows = false;
			this.dgvSideBin.AllowUserToResizeRows = false;
			this.dgvSideBin.AllowUserToResizeColumns = false;
			this.dgvSideBin.AllowUserToOrderColumns = false;
			this.dgvSideBin.EditMode = DataGridViewEditMode.EditOnKeystroke;
			this.dgvSideBin.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvSideBin.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvSideBin.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			this.dgvSideBin.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			this.dgvSideBin.SelectionMode = DataGridViewSelectionMode.CellSelect;
			this.dgvSideBin.Font = new Font("Microsoft JhengHei", 10);

			for (int col = 0; col < this.dgvSideBin.ColumnCount; col++)
			{
				this.dgvSideBin.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;
			}

            if (DataCenter._rdFunc.RDFuncData.TesterConfigType == ETesterConfigType.PDTester)
            {
                tabiBinTable2D.Visible = false;
            }
		}

		private void UpdateDataToControls()
		{
			this._smartBinning = DataCenter._smartBinning.Clone() as SmartBinning;

            this.ResetEnableMsrtItem();   // Order will be reset
           
            /////////////////////////////////////////////////////////////////////////
            // update order by Smart Bin
            List<SmartBoundaryData> lstBoundary = new List<SmartBoundaryData>();

            foreach (var boundary in this._smartBinning.SmartBin.Boundary)
            {
                lstBoundary.Add(boundary.Clone() as SmartBoundaryData);
            }

            this._enableMsrtItem.UpdateOrderByBoundary(lstBoundary);

            /////////////////////////////////////////////////////////////////////////

			if (!this.IsHandleCreated)
			{
				return;
			}

            bool enable = true ;

			switch (DataCenter._uiSetting.UIOperateMode)
			{
				case (int)EUIOperateMode.Idle:
					this.ChangeAuthority();
					break;
				//-----------------------------------------------------------------------------
				case (int)EUIOperateMode.AutoRun:
				case (int)EUIOperateMode.ManulRun:
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
					break;
			}

            if (enable && (int)DataCenter._uiSetting.AuthorityLevel >= 20)
            {
                this.pnlRight.Enabled = true;
            }
            else
            {
                this.pnlRight.Enabled = false;
            }


			//Updtae Select DGV
			this.UpdateEnableBoundaryDGV();

			this.UpdateEnableBoundary2DDGV();

			this.UpDateSelectItem();

			//Updtae DGV
			this.UpdateBoundaryToDGV();

			this.UpdateBoundary2DToDGV();			

			this.UpdateBinTableToDGV();

			this.UpdateNGBinToDGV();

			this.UpdateSideBinToDGV();			
		}

        private void ChangeAuthority()
		{
			switch (DataCenter._uiSetting.AuthorityLevel)
			{
				case EAuthority.Operator:
				case EAuthority.QC:
					break;
				//-------------------------------------------------------------------
				case EAuthority.Engineer:
				case EAuthority.Admin:
				case EAuthority.Super:
					break;
				default:
					break;
			}
		}

		private void ResetEnableMsrtItem()
		{
            this._enableMsrtItem.Clear();

			ProductData product = DataCenter._product;

			if (product == null || product.TestCondition == null)
			{
				return;
			}

			if (product.TestCondition.TestItemArray == null || product.TestCondition.TestItemArray.Length == 0)
			{
				return;
			}

			foreach (var testItem in product.TestCondition.TestItemArray)
			{
				if (testItem.MsrtResult == null || testItem.MsrtResult.Length == 0)
				{
					continue;
				}

				if (!testItem.IsEnable)
				{
					continue;
				}

				//CIExy & CIEupvp
				Dictionary<string, string> binItemNameDic = DataCenter._uiSetting.UserDefinedData.BinItemNameDic;

				if (testItem.KeyName.Contains(ETestType.LOPWL.ToString()))
				{
					string cieXKey = testItem.KeyName.Replace(ETestType.LOPWL.ToString(), EOptiMsrtType.CIEx.ToString());

					string cieYKey = testItem.KeyName.Replace(ETestType.LOPWL.ToString(), EOptiMsrtType.CIEy.ToString());

					string cieUpKey = testItem.KeyName.Replace(ETestType.LOPWL.ToString(), EOptiMsrtType.Uprime.ToString());

					string cieVpKey = testItem.KeyName.Replace(ETestType.LOPWL.ToString(), EOptiMsrtType.Vprime.ToString());

					string xyFormat = "0.0000";

					string upvpFormat = "0.0000";

					bool cieXIsTester = false;

					bool cieYIsTester = false;

					bool cieUpIsTester = false;

					bool cieVpIsTester = false;

					foreach (var msrtItem in testItem.MsrtResult)
					{
						if (!msrtItem.IsEnable || !msrtItem.IsVision)
						{
							continue;
						}

						if (msrtItem.KeyName.Contains(cieXKey))
						{
							cieXIsTester = true;

							xyFormat = msrtItem.Formate;
						}
						else if (msrtItem.KeyName.Contains(cieYKey))
						{
							cieYIsTester = true;
						}
						else if (msrtItem.KeyName.Contains(cieUpKey))
						{
							upvpFormat = msrtItem.Formate;

							cieUpIsTester = true;
						}
						else if (msrtItem.KeyName.Contains(cieVpKey))
						{
							cieVpIsTester = true;
						}						
					}

					if (cieXIsTester && cieYIsTester)
					{
						TestResultData data = new TestResultData();

						data.KeyName = testItem.KeyName.Replace(ETestType.LOPWL.ToString(), SmartBinning.CIExyKEY);

						data.Name = data.KeyName;

						if (binItemNameDic.ContainsKey(data.KeyName))
						{
						data.Name = binItemNameDic[data.KeyName];
						}

						data.Formate = xyFormat;

                        this._enableMsrtItem.Add(data);
					}

					if (cieUpIsTester && cieVpIsTester)
					{
						TestResultData data = new TestResultData();

						data.KeyName = testItem.KeyName.Replace(ETestType.LOPWL.ToString(), SmartBinning.CIEupvpKEY);

						data.Name = data.KeyName;

						if (binItemNameDic.ContainsKey(data.KeyName))
						{
						data.Name = binItemNameDic[data.KeyName];
						}

						data.Formate = upvpFormat;

                        this._enableMsrtItem.Add(data);
					}
				}

				//Other Msrt Item
				foreach (var msrtItem in testItem.MsrtResult)
				{
					if (!msrtItem.IsEnable || !msrtItem.IsVision)
					{
						continue;
					}

                    this._enableMsrtItem.Add(msrtItem);
				}
			}
		}

		private void UpdateBoundaryToDGV()
		{
			if (!this.dgvBoundary.Visible)
			{
				return;
			}			

			this.dgvBoundary.SuspendLayout();

			this.dgvBoundary.Rows.Clear();

			this.dgvBoundary.Columns.Clear();

			////////////////////////////////////////////////////////////////////////////////////
			//Create Columns
			////////////////////////////////////////////////////////////////////////////////////
			SmartBoundary smartBoundary = this._smartBinning.SmartBin.Boundary;
            int shiftCol = DataCenter._sysSetting.BinSortingRule == EBinBoundaryRule.Various ? 1 : 0;
			for (int col = 0; col < smartBoundary.Count; col++)
			{
				if (smartBoundary[col].KeyName.Contains(SmartBinning.CIExyKEY) ||
					smartBoundary[col].KeyName.Contains(SmartBinning.CIEupvpKEY))
				{
					continue;
				}

                string format = "0.#############";
                int width = 150;

                var tItem =FindTestItemByMsrtKey(smartBoundary[col].KeyName);
                if (tItem != null &&tItem.Type == ETestType.LCR)
                {
                    format = "0.000000000000000";
                    width = 150;
                }

				//Add Column Boundary
				DataGridViewColumn column = new DataGridViewTextBoxColumn();

				column.SortMode = DataGridViewColumnSortMode.NotSortable;

				column.Tag = smartBoundary[col].KeyName;

				column.HeaderText = smartBoundary[col].Name;

                var item = FindMsrtDataFromMsrtKey(smartBoundary[col].KeyName);
                if (item != null)
                {
                    column.HeaderText = item.Name;
                    smartBoundary[col].Name = item.Name;
                }

				column.Width = 150;

				column.ValueType = typeof(string);
                
				this.dgvBoundary.Columns.Add(column);

				//Add Column Min
				column = new DataGridViewDoubleInputColumn();                

				column.SortMode = DataGridViewColumnSortMode.NotSortable;

				column.HeaderText = "Min";

                column.Width = width;

				column.ValueType = typeof(double);

				(column as DataGridViewDoubleInputColumn).DisplayFormat = smartBoundary[col].Format;

				(column as DataGridViewDoubleInputColumn).MinValue = -99999;

                (column as DataGridViewDoubleInputColumn).DisplayFormat = format;

               

   

				this.dgvBoundary.Columns.Add(column);

				//Add Column Max
				column = new DataGridViewDoubleInputColumn();

				column.SortMode = DataGridViewColumnSortMode.NotSortable;

				column.HeaderText = "Max";

                column.Width = width;

				column.ValueType = typeof(double);

				(column as DataGridViewDoubleInputColumn).DisplayFormat = smartBoundary[col].Format;

				(column as DataGridViewDoubleInputColumn).MinValue = 0;

                (column as DataGridViewDoubleInputColumn).DisplayFormat = format;

				this.dgvBoundary.Columns.Add(column);

                if (DataCenter._sysSetting.BinSortingRule == EBinBoundaryRule.Various)
                {
                    DataGridViewComboBoxColumn colR = new DataGridViewComboBoxColumn();

                    colR.ReadOnly = false;

                    colR.SortMode = DataGridViewColumnSortMode.NotSortable;

                    colR.HeaderText = smartBoundary[col].Name;

                    colR.Width = 100;

                    colR.ValueType = typeof(string);

                    colR.Items.Add("≦ Item <");
                    colR.Items.Add("≦ Item ≦");
                    colR.Items.Add("< Item ≦");
                    colR.Items.Add("< Item <");
                    this.dgvBoundary.Columns.Add(colR);
                }

				////////////////////////////////////////////////////////////////////////////////////
				//Create Rows
				////////////////////////////////////////////////////////////////////////////////////
                

				for (int row = 0; row < smartBoundary[col].Count; row++)
				{
					if (row >= this.dgvBoundary.RowCount)
					{
						this.dgvBoundary.Rows.Add();

                        this.dgvBoundary.Rows[row].HeaderCell.Value = (row + 1).ToString("000");
					}


                    this.dgvBoundary[this.dgvBoundary.ColumnCount - 3 - shiftCol, row].Value = smartBoundary[col][row].BoundaryCode;

                    this.dgvBoundary[this.dgvBoundary.ColumnCount - 2 - shiftCol, row].Value = (smartBoundary[col][row] as SmartLowUp).LowLimit;

                    this.dgvBoundary[this.dgvBoundary.ColumnCount - 1 - shiftCol, row].Value = (smartBoundary[col][row] as SmartLowUp).UpLimit;

                    if (shiftCol > 0)
                    {
                        string rStr = GetBoundaryRuleStr((smartBoundary[col][row] as SmartLowUp).BoundaryRule);
                        (this.dgvBoundary[this.dgvBoundary.ColumnCount - 1, row] as DataGridViewComboBoxCell).Value = rStr;
                    }
				}
			}

			////////////////////////////////////////////////////////////////////////////////////
			//Change Color for disable boundary or same Boundary code
			////////////////////////////////////////////////////////////////////////////////////
            for (int col = 0; col < this.dgvBoundary.ColumnCount; col +=( 3 + shiftCol))
			{
                //Disable Msrt Item
                if (!this._enableMsrtItem.ContainsKey(this.dgvBoundary.Columns[col].Tag.ToString()))
                {
                    this.dgvBoundary.Columns[col].DefaultCellStyle.BackColor = Color.LightSkyBlue;

                    this.dgvBoundary.Columns[col + 1].DefaultCellStyle.BackColor = Color.LightSkyBlue;

                    this.dgvBoundary.Columns[col + 2].DefaultCellStyle.BackColor = Color.LightSkyBlue;

                    //if (shiftCol > 0)
                    //{
                    //    this.dgvBoundary.Columns[col + 3].DefaultCellStyle.BackColor = Color.LightSkyBlue;
                    //}

                    continue;
                }

				for (int row = 0; row < this.dgvBoundary.RowCount; row++)
				{
					string boundaryCode = string.Empty;

					double min = 0.0d;

					double max = 0.0d;

					if (this.dgvBoundary[col, row].Value != null)
					{
						boundaryCode = this.dgvBoundary[col, row].Value.ToString();
					}

					if (this.dgvBoundary[col + 1, row].Value != null)
					{
						min = double.Parse(this.dgvBoundary[col + 1, row].Value.ToString());
					}

					if (this.dgvBoundary[col + 1, row].Value != null)
					{
						max = double.Parse(this.dgvBoundary[col + 2, row].Value.ToString());
					}

					if (min >= max)
					{
						//Warning Msrt Item
						DataGridViewCellStyle style = new DataGridViewCellStyle();

						style.BackColor = Color.LightPink;

						this.dgvBoundary[col + 1, row].Style = style;

						this.dgvBoundary[col + 2, row].Style = style;
					}
					else
					{
						for (int i = 0; i < this.dgvBoundary.RowCount; i++)
						{
							if (i == row)
							{
								continue;
							}

							string tempCode = string.Empty;

							double tempMin = 0;

							double tempMax = 0;

							if (this.dgvBoundary[col, i].Value != null)
							{
								tempCode = this.dgvBoundary[col, i].Value.ToString();
							}

							if (this.dgvBoundary[col + 1, i].Value != null)
							{
								tempMin = double.Parse(this.dgvBoundary[col + 1, i].Value.ToString());
							}

							if (this.dgvBoundary[col + 1, i].Value != null)
							{
								tempMax = double.Parse(this.dgvBoundary[col + 2, i].Value.ToString());
							}

							bool isWarning = false; 

							if ((i == row && boundaryCode == string.Empty) ||
								(i != row) && (tempCode == boundaryCode))
							{
								//Warning Msrt Item
								DataGridViewCellStyle style = new DataGridViewCellStyle();

								style.BackColor = Color.LightPink;

								this.dgvBoundary[col, row].Style = style;

								isWarning = true;
							}

							if ((i != row) && (tempMin == min && tempMax == max))
							{
								//Warning Msrt Item
								DataGridViewCellStyle style = new DataGridViewCellStyle();

								style.BackColor = Color.LightPink;

								this.dgvBoundary[col + 1, row].Style = style;

								this.dgvBoundary[col + 2, row].Style = style;

								isWarning = true;
							}

							if (isWarning)
							{
								break;
							}
						}
					}

					//No Setting Item
					if (min == 0 && max == 0)
					{
						DataGridViewCellStyle style = new DataGridViewCellStyle();

						style.BackColor = Color.LightGray;

						this.dgvBoundary[col, row].Style = style;

						this.dgvBoundary[col + 1, row].Style = style;

						this.dgvBoundary[col + 2, row].Style = style;
					}
				}
			}

			this.dgvBoundary.ResumeLayout();

			this.numGrade.Value = this.dgvBoundary.RowCount;
		}

		private void UpdateBoundary2DToDGV()
		{
			if (!this.dgvBoundary2D.Visible)
			{
				return;
			}

			this.dgvBoundary2D.SuspendLayout();

			this.dgvBoundary2D.Columns.Clear();

			this.dgvBoundary2D.Rows.Clear();

			SmartBoundary smartBoundary = this._smartBinning.SmartBin.Boundary;

			Dictionary<string, string> binItemNameDic = DataCenter._uiSetting.UserDefinedData.BinItemNameDic;

			////////////////////////////////////////////////////////////////////////////////////
			//Find the Select CIExy KeyName
			////////////////////////////////////////////////////////////////////////////////////
			string keyName = string.Empty;

			if (this.cmbSelectCIE.SelectedItem != null)
			{
				keyName = ((KeyValuePair<string, string>)this.cmbSelectCIE.SelectedItem).Key.ToString();
			}

			List<string> enableItem = new List<string>();

			for (int row = 0; row < this.dgvEnableBoundary2D.RowCount; row++)
			{
				if (this.dgvEnableBoundary2D[0, row].Value != null && (bool)this.dgvEnableBoundary2D[0, row].Value)
				{
					enableItem.Add(this.dgvEnableBoundary2D.Rows[row].Tag.ToString());
				}
			}

			if (keyName == string.Empty || !enableItem.Contains(keyName) || !this._smartBinning.SmartBin.ContainsBoundary(keyName))
			{
				this.dgvBoundary2D.ResumeLayout();

				return;
			}			

			////////////////////////////////////////////////////////////////////////////////////
			//Create Column
			////////////////////////////////////////////////////////////////////////////////////
			DataGridViewColumn nameColumn = new DataGridViewTextBoxColumn();

			nameColumn.SortMode = DataGridViewColumnSortMode.NotSortable;

			nameColumn.HeaderText = binItemNameDic[keyName].Replace("_", "");

			nameColumn.Width = 100;

			nameColumn.ValueType = typeof(string);

			SmartBoundaryData smartBoundaryData = smartBoundary[keyName];

			this.dgvBoundary2D.Columns.Add(nameColumn);

			int columnCount = this.numPolygon.Value * 2;// 8;

			if (smartBoundaryData.MaxDataLength > 8)
			{
				columnCount = smartBoundaryData.MaxDataLength;
			}

			for (int cnt = 0; cnt < columnCount; cnt++)
			{
				string key = "P";

				if (cnt % 2 == 0)
				{
					if (keyName.Contains(SmartBinning.CIExyKEY))
					{
						key += "x" + (cnt / 2 + 1).ToString();
					}
					else if (keyName.Contains(SmartBinning.CIEupvpKEY))
					{
						key += "up" + (cnt / 2 + 1).ToString();
					}
				}
				else
				{
					if (keyName.Contains(SmartBinning.CIExyKEY))
					{
						key += "y" + (cnt / 2 + 1).ToString();
					}
					else if (keyName.Contains(SmartBinning.CIEupvpKEY))
					{
						key += "vp" + (cnt / 2 + 1).ToString();
					}
				}

				DataGridViewColumn column = new DataGridViewDoubleInputColumn();

				column.SortMode = DataGridViewColumnSortMode.NotSortable;

				column.HeaderText = binItemNameDic[key].Replace("_", ""); ;

				column.Width = 100;

				(column as DataGridViewDoubleInputColumn).DisplayFormat = smartBoundary[keyName].Format;

				//(column as DataGridViewDoubleInputColumn).MinValue = 0;

				//(column as DataGridViewDoubleInputColumn).MaxValue = 1;

				column.ValueType = typeof(double);

				this.dgvBoundary2D.Columns.Add(column);
			}

			////////////////////////////////////////////////////////////////////////////////////
			//Create Row
			////////////////////////////////////////////////////////////////////////////////////
			for (int row = 0; row < smartBoundaryData.Count; row++)
			{
				SmartBoundaryBase boundaryBase = smartBoundaryData[row];

				this.dgvBoundary2D.Rows.Add();

                this.dgvBoundary2D.Rows[this.dgvBoundary2D.RowCount - 1].HeaderCell.Value = (row + 1).ToString("000");

				this.dgvBoundary2D[0, row].Value = boundaryBase.BoundaryCode;

				if (boundaryBase is SmartPolygon)
				{
					SmartPolygon polygon = boundaryBase as SmartPolygon;

					for (int col = 0; col < polygon.Coord.Count; col++)
					{
						this.dgvBoundary2D[col * 2 + 1, row].Value = polygon.Coord[col].X;

						this.dgvBoundary2D[col * 2 + 2, row].Value = polygon.Coord[col].Y;
					}
				}
				else if (boundaryBase is SmartEllipse)
				{
					SmartEllipse ellipse = boundaryBase as SmartEllipse;

					this.dgvBoundary2D[1, row].Value = ellipse.X;

					this.dgvBoundary2D[2, row].Value = ellipse.Y;

					this.dgvBoundary2D[3, row].Value = ellipse.a;

					this.dgvBoundary2D[4, row].Value = ellipse.b;

					this.dgvBoundary2D[5, row].Value = ellipse.Theta;
				}
			}

			////////////////////////////////////////////////////////////////////////////////////
			//Change Color for disable boundary or same Boundary code
			////////////////////////////////////////////////////////////////////////////////////
            if (this._enableMsrtItem.ContainsKey(keyName))
			{
				this.dgvBoundary2D.DefaultCellStyle.BackColor = Color.White;

				for (int row = 0; row < this.dgvBoundary2D.RowCount; row++)
				{
					string binCode = string.Empty;

					double x1Index = 0.0d;

					double y1Index = 0.0d;

					double y3Index = 0.0d;

					double xp = 0.0d;

					double yp = 0.0d;

					int disableIndex = this.dgvBoundary2D.ColumnCount;

					for (int col = 0; col < this.dgvBoundary2D.ColumnCount; col++)
					{
						if (col == 0 && this.dgvBoundary2D[col, row].Value != null)
						{
							binCode = this.dgvBoundary2D[col, row].Value.ToString();
						}
						else if (col == 1 && this.dgvBoundary2D[col, row].Value != null)
						{
							x1Index = double.Parse(this.dgvBoundary2D[col, row].Value.ToString());
						}
						else if (col == 2 && this.dgvBoundary2D[col, row].Value != null)
						{
							y1Index = double.Parse(this.dgvBoundary2D[col, row].Value.ToString());
						}
						else if (col == 6 && this.dgvBoundary2D[col, row].Value != null)
						{
							y3Index = double.Parse(this.dgvBoundary2D[col, row].Value.ToString());
						}

						if (col != 0 && this.dgvBoundary2D[col, row].Value != null)
						{
							if (col % 2 == 1)
							{
								xp = double.Parse(this.dgvBoundary2D[col, row].Value.ToString());
							}

							if (col % 2 == 0)
							{
								yp = double.Parse(this.dgvBoundary2D[col, row].Value.ToString());
							}
						}
						else
						{
							xp = 0;

							yp = 0;
						}

						//Check Warning
						bool isWarning = false;

						if (col == 0)
						{
							string temp = string.Empty;

							for (int i = 0; i < this.dgvBoundary2D.RowCount; i++)
							{
								if (this.dgvBoundary2D[0, i].Value != null)
								{
									temp = this.dgvBoundary2D[0, i].Value.ToString();
								}

								if (i == row && binCode == string.Empty)
								{
									isWarning = true;

									break;
								}
								else if (i != row && binCode == temp)
								{
									isWarning = true;

									break;
								}
							}
						}
						else if (col != 0 && col != 5)
						{
							if (col % 2 == 1 && (xp == 0 || xp > 1))
							{
								isWarning = true;
							}
							else if (col % 2 == 0 && (yp == 0 || yp >1))
							{
								isWarning = true;
							}
						}

						if (isWarning)
						{
							DataGridViewCellStyle style = new DataGridViewCellStyle();

							style.BackColor = Color.LightPink;

							this.dgvBoundary2D[col, row].Style = style;
						}

						if (col == 2)
						{
							if (x1Index == 0 && y1Index == 0)
							{
								disableIndex = 0;

								break;
							}
						}
						else if (col == 6 && y3Index == 0)
						{
							disableIndex = col;

							break;
						}
						else if (col > 6 && col % 2 == 0)
						{
							if (xp == 0 && yp == 0)
							{
								disableIndex = col - 1;

								break;
							}
						}
					}

					//Check Disable
					for (int col = disableIndex; col < this.dgvBoundary2D.ColumnCount; col++)
					{
						DataGridViewCellStyle style = new DataGridViewCellStyle();

						style.BackColor = Color.LightGray;

						this.dgvBoundary2D[col, row].Style = style;
					}
				}
			}
			else
			{
				//Disable Msrt Item
				this.dgvBoundary2D.DefaultCellStyle.BackColor = Color.LightSkyBlue;
			}

			this.dgvBoundary2D.ResumeLayout();

			this.numGrade2D.Value = this.dgvBoundary2D.RowCount;

			this.numPolygon.Value = columnCount / 2;
		}

		private void UpdateBinTableToDGV()
		{
			if (!this.dgvBinTable.Visible)
			{
				return;
			}

			this.dgvBinTable.SuspendLayout();

			this.dgvBinTable.Rows.Clear();

			this.dgvBinTable.Columns.Clear();

			////////////////////////////////////////////////////////////////////////////////////
			//Create Column Boundary
			////////////////////////////////////////////////////////////////////////////////////
			DataGridViewColumn column = new DataGridViewTextBoxColumn();

			column.SortMode = DataGridViewColumnSortMode.NotSortable;

			column.HeaderText = "Bin Code";

			column.Width = 250;

			column.ValueType = typeof(string);

			this.dgvBinTable.Columns.Add(column);

			column = new DataGridViewTextBoxColumn();

			column.SortMode = DataGridViewColumnSortMode.NotSortable;

			column.HeaderText = "Bin Number";

			column.Width = 100;

			column.ValueType = typeof(string);

			this.dgvBinTable.Columns.Add(column);

			SmartBoundary smartBoundary = this._smartBinning.SmartBin.Boundary;

			for (int col = 0; col < smartBoundary.Count; col++)
			{
				if (smartBoundary[col].Count == 0)
				{
					continue;
				}

				column = new DataGridViewTextBoxColumn();

				column.ReadOnly = true;

				//column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

				column.SortMode = DataGridViewColumnSortMode.NotSortable;

				column.HeaderText = smartBoundary[col].Name;

				column.Width = 200;

				column.ValueType = typeof(string);

				this.dgvBinTable.Columns.Add(column);
                
			}

			SmartBin smartBin = this._smartBinning.SmartBin;
      
            

			for (int row = 0; row < smartBin.Count; row++)
			{
				this.dgvBinTable.Rows.Add();

				DataGridViewRow dgrow = this.dgvBinTable.Rows[this.dgvBinTable.RowCount - 1];

                dgrow.HeaderCell.Value = (this.dgvBinTable.RowCount).ToString("000");

				dgrow.Cells[0].Value = smartBin[row].BinCode;

				dgrow.Cells[1].Value = smartBin[row].BinNumber;

				string boundarySN = smartBin[row].BoundarySN;

                for (int col = 2; col < this.dgvBinTable.ColumnCount; col ++)
				{
					bool isMeet = false;

					for (int i = 0; i < smartBoundary.Count; i++)
					{
						if (isMeet)
						{
							break;
						}

						for (int j = 0; j < smartBoundary[i].Count; j++)
						{
							if (boundarySN.Contains(smartBoundary[i][j].SerialNumber))
							{
								string boundaryCode = smartBoundary[i][j].BoundaryCode;

								string boundary = string.Empty;

                                //string rule = "";

								if (smartBoundary[i][j] is SmartLowUp)
								{
                                    string lowLimit = (smartBoundary[i][j] as SmartLowUp).LowLimit.ToString("0.#########");

                                    string upLimit = (smartBoundary[i][j] as SmartLowUp).UpLimit.ToString("0.#########");

                                    if (lowLimit == "0")
                                    {
                                        lowLimit = (smartBoundary[i][j] as SmartLowUp).LowLimit.ToString("E4");
                                    }

                                    if (upLimit == "0")
                                    {
                                        upLimit = (smartBoundary[i][j] as SmartLowUp).UpLimit.ToString("E4");
                                    }

									boundary = " (" + lowLimit + "/" + upLimit + ")";
								}
								else if (smartBoundary[i][j] is SmartEllipse)
								{
									boundary = " (E)";
								}
								else if (smartBoundary[i][j] is SmartPolygon)
								{
									boundary = " (P)";
								}

								this.dgvBinTable[col, row].Value = boundaryCode + boundary;
                                
								isMeet = true;

								boundarySN = boundarySN.Replace(smartBoundary[i][j].SerialNumber, "");

								break;
							}
						}
					}
				}
			}

			////////////////////////////////////////////////////////////////////////////////////
			//Change Color for same bin number
			////////////////////////////////////////////////////////////////////////////////////
			for (int row = 0; row < this.dgvBinTable.RowCount; row++)
			{
				for (int col = 0; col < 2; col++)
				{
					for (int i = 0; i < this.dgvBinTable.RowCount; i++)
					{
						if (i == row)
						{
							continue;
						}

						if (this.dgvBinTable[col, row].Value != null &&
							this.dgvBinTable[col, i].Value != null)
						{
							if (this.dgvBinTable[col, row].Value.ToString() == this.dgvBinTable[col, i].Value.ToString())
							{
								DataGridViewCellStyle style = new DataGridViewCellStyle();

								style.BackColor = Color.LightPink;

								this.dgvBinTable[col, row].Style = style;

								break;
							}
						}
					}
				}
			}

			this.dgvBinTable.ResumeLayout();

			this.chkIsEnableAutoBin.Checked = this._smartBinning.IsAutoBin;
		}        

		private void UpdateNGBinToDGV()
		{
			if (!this.dgvNGBin.Visible)
			{
				return;
			}

			this.dgvNGBin.SuspendLayout();

			this.dgvNGBin.Rows.Clear();

            bool islcr = false;
            foreach (var ngInfo in this._smartBinning.NGBin)
            {
                var tItem = FindTestItemByMsrtKey(ngInfo.KeyName);
                if (tItem != null && tItem.Type == ETestType.LCR)
                {
                    islcr = true;
                    break;
                }
            }

            if (islcr)
            {
                (this.dgvNGBin.Columns["colNGBinLowLimit"] as DataGridViewDoubleInputColumn).DisplayFormat = "0.000000000000000";
                (this.dgvNGBin.Columns["colNGBinUpLimit"] as DataGridViewDoubleInputColumn).DisplayFormat = "0.000000000000000";
            }
            else
            {
                (this.dgvNGBin.Columns["colNGBinLowLimit"] as DataGridViewDoubleInputColumn).DisplayFormat = "0.############";
                (this.dgvNGBin.Columns["colNGBinUpLimit"] as DataGridViewDoubleInputColumn).DisplayFormat = "0.############";
            }
            
			
			for (int row = 0; row < this._smartBinning.NGBin.Count; row++)
			{
				this.dgvNGBin.Rows.Add();

				DataGridViewRow dgrow = this.dgvNGBin.Rows[this.dgvNGBin.RowCount - 1];

				dgrow.Tag = this._smartBinning.NGBin[row].KeyName;

                dgrow.HeaderCell.Value = (this.dgvNGBin.RowCount).ToString("000");

				dgrow.Cells[0].Value = this._smartBinning.NGBin[row].IsEnable;

				dgrow.Cells[1].Value = this._smartBinning.NGBin[row].Name;

				dgrow.Cells[1].ReadOnly = true;

				dgrow.Cells[2].Value = this._smartBinning.NGBin[row].BinCode;

				dgrow.Cells[3].Value = this._smartBinning.NGBin[row].BinNumber;

				dgrow.Cells[4].Value = this._smartBinning.NGBin[row].NGLowLimit;				

				dgrow.Cells[5].Value = this._smartBinning.NGBin[row].NGUpLimit;

                if (DataCenter._sysSetting.IsSpecBinTableSync)
                {
                    //dgrow.Cells[0].ReadOnly = true;
                    dgrow.Cells[4].ReadOnly = true;
                    dgrow.Cells[5].ReadOnly = true;

                }
                else
                {
                    //dgrow.Cells[0].ReadOnly = false;
                    dgrow.Cells[4].ReadOnly = false;
                    dgrow.Cells[5].ReadOnly = false;
                }

                //dgrow.Cells[4].

                //(column as DataGridViewDoubleInputColumn).DisplayFormat = smartBoundary[col].Format

                if (!this._enableMsrtItem.ContainsKey(dgrow.Tag.ToString()))
				{
                    dgrow.DefaultCellStyle.BackColor = Color.LightSkyBlue;
				}
			}

			this.dgvNGBin.ResumeLayout();
		}

		private void UpdateSideBinToDGV()
		{
			if (!this.dgvSideBin.Visible)
			{
				return;
			}

			this.dgvSideBin.SuspendLayout();

			this.dgvSideBin.Rows.Clear();
			
			for (int row = 0; row < this._smartBinning.SideBin.Count; row++)
			{
				this.dgvSideBin.Rows.Add();

				DataGridViewRow dgrow = this.dgvSideBin.Rows[this.dgvSideBin.RowCount - 1];

				dgrow.Tag = this._smartBinning.SideBin[row].KeyName;

                dgrow.HeaderCell.Value = (this.dgvSideBin.RowCount).ToString("000");

				dgrow.Cells[0].Value = this._smartBinning.SideBin[row].Name;

				dgrow.Cells[0].ReadOnly = true;

				dgrow.Cells[1].Value = this._smartBinning.SideBin[row].BinCode;

				dgrow.Cells[2].Value = this._smartBinning.SideBin[row].BinNumber;

				if (!this._enableMsrtItem.ContainsKey(dgrow.Tag.ToString()))
				{
					dgrow.DefaultCellStyle.BackColor = Color.LightSkyBlue;
				}
			}

			this.dgvSideBin.ResumeLayout();
		}

		private void UpdateEnableBoundaryDGV()
		{
			if (!this.dgvEnableBoundary.Visible)
			{
				return;
			}

			this.dgvEnableBoundary.SuspendLayout();

			this.dgvEnableBoundary.Rows.Clear();

			bool isSelectAll = true;

            foreach (var item in this._enableMsrtItem.Data)
			{
				this.dgvEnableBoundary.Rows.Add();

				DataGridViewRow dgrow = this.dgvEnableBoundary.Rows[this.dgvEnableBoundary.RowCount - 1];

				dgrow.Tag = item.KeyName;

				dgrow.Cells[1].ReadOnly = true;

				dgrow.Cells[1].Value = item.Name;

				if (DataCenter._smartBinning.SmartBin.ContainsBoundary(item.KeyName))
				{
					dgrow.Cells[0].Value = true;
				}
				else
				{
					dgrow.Cells[0].Value = false;
				}

                if (item.KeyName.Contains(SmartBinning.CIExyKEY) ||
                    item.KeyName.Contains(SmartBinning.CIEupvpKEY))
				{
					dgrow.Visible = false;
				}

				isSelectAll &= (bool)dgrow.Cells[0].Value;
			}

			this.chkBoundaryAll.Tag = false;

			this.chkBoundaryAll.Checked = isSelectAll;

			this.chkBoundaryAll.Tag = true;

			this.dgvEnableBoundary.ResumeLayout();
		}

		private void UpdateEnableBoundary2DDGV()
		{
			if (!this.dgvEnableBoundary2D.Visible)
			{
				return;
			}

			this.dgvEnableBoundary2D.SuspendLayout();

			this.dgvEnableBoundary2D.Rows.Clear();

			bool isSelectAll = true;

			foreach (var item in this._enableMsrtItem.Data)
			{
				this.dgvEnableBoundary2D.Rows.Add();

				DataGridViewRow dgrow = this.dgvEnableBoundary2D.Rows[this.dgvEnableBoundary2D.RowCount - 1];

				dgrow.Tag = item.KeyName;

				dgrow.Cells[1].ReadOnly = true;

				dgrow.Cells[1].Value = item.Name;

                if (DataCenter._smartBinning.SmartBin.ContainsBoundary(item.KeyName))
				{
					dgrow.Cells[0].Value = true;
				}
				else
				{
					dgrow.Cells[0].Value = false;
				}

                if (!item.KeyName.Contains(SmartBinning.CIExyKEY) &&
                    !item.KeyName.Contains(SmartBinning.CIEupvpKEY))
				{
					dgrow.Visible = false;
				}

				isSelectAll &= (bool)dgrow.Cells[0].Value;
			}

			this.chkBoundary2DAll.Tag = false;

			this.chkBoundary2DAll.Checked = isSelectAll;

			this.chkBoundary2DAll.Tag = true;

			this.dgvEnableBoundary2D.ResumeLayout();
		}

		private void UpDateSelectItem()
		{
			if (!this.cmbSelectCIE.Visible)
			{
				return;
			}

			this.cmbSelectCIE.DisplayMember = "Value";

			this.cmbSelectCIE.ValueMember = "Key";

			string selectKeyName = string.Empty;

			if (this.cmbSelectCIE.SelectedItem != null)
			{
				selectKeyName = ((KeyValuePair<string, string>)this.cmbSelectCIE.SelectedItem).Key;
			}

			this.cmbSelectCIE.Items.Clear();

			for (int row = 0; row < this.dgvEnableBoundary2D.RowCount; row++)
			{
				if (!this.dgvEnableBoundary2D.Rows[row].Visible)
				{
					continue;
				}

				if (this.dgvEnableBoundary2D[0, row].Value != null)
				{
					string keyName = this.dgvEnableBoundary2D.Rows[row].Tag.ToString();

					string name = this.dgvEnableBoundary2D[1, row].Value.ToString();

					KeyValuePair<string, string> item = new KeyValuePair<string, string>(keyName, name);

					this.cmbSelectCIE.Items.Add(item);
				}
			}

			if (this.cmbSelectCIE.SelectedIndex < 0 && this.cmbSelectCIE.Items.Count > 0)
			{
				this.cmbSelectCIE.SelectedIndex = 0;
			}
		}

		private void SaveBoundary()
		{
			if (!this.dgvBoundary.Visible)
			{
				return;
			}

			//////////////////////////////////////////////////////////////////////
			//Clear Boundary
			//////////////////////////////////////////////////////////////////////
			SmartBoundary smartBoundary = this._smartBinning.SmartBin.Boundary;
			
			foreach (var item in smartBoundary)
			{
				if (item.KeyName.Contains(SmartBinning.CIExyKEY))
				{
					continue;
				}
				else if (item.KeyName.Contains(SmartBinning.CIEupvpKEY))
				{
					continue;
				}
				else
				{
					item.Clear();
				}
			}

			//////////////////////////////////////////////////////////////////////
			//Create Boundary
			//////////////////////////////////////////////////////////////////////
            int shiftCol = DataCenter._sysSetting.BinSortingRule == EBinBoundaryRule.Various ? 1 : 0;

            for (int col = 0; col < this.dgvBoundary.ColumnCount; col += (3 + shiftCol))
			{
				string keyName = this.dgvBoundary.Columns[col].Tag.ToString();

                EBinBoundaryRule bR = GetBoundaryRule(keyName);

				for (int row = 0; row < this.dgvBoundary.RowCount; row++)
				{
					string code = string.Empty;

					double min = 0.0d;

					double max = 0.0d;

					if (this.dgvBoundary[col, row].Value != null)
					{
						code = this.dgvBoundary[col, row].Value.ToString();
					}

					if (this.dgvBoundary[col + 1, row].Value != null)
					{
						min = double.Parse(this.dgvBoundary[col + 1, row].Value.ToString());
					}

					if (this.dgvBoundary[col + 2, row].Value != null)
					{
						max = double.Parse(this.dgvBoundary[col + 2, row].Value.ToString());
					}

					if (min == 0 && max == 0)
					{
						if (row == 0)
						{
							smartBoundary.Remove(keyName);
						}

						break;
					}

					SmartLowUp data = new SmartLowUp();

					data.BoundaryCode = code;

					data.LowLimit = min;

					data.UpLimit = max;

                    data.BoundaryRule = bR;
                    if (shiftCol == 1)
                    {
                        string rStr = this.dgvBoundary[col + 3, row].Value.ToString();
                        if (rStr != null && rStr != "")
                        {
                            bR = GetBoundaryRuleEnum(rStr);
                            data.BoundaryRule = bR;
                        }
                    }

					smartBoundary[keyName].Add(data);
				}

			}

			this._smartBinning.SmartBin.CreateBinTable();

			this.CreateNGBinAndSideBin();
		}

		private void SaveBoundary2D()
		{
			if (!this.dgvBoundary2D.Visible || this.cmbSelectCIE.SelectedItem == null)
			{
				return;
			}

			string keyName = ((KeyValuePair<string, string>)this.cmbSelectCIE.SelectedItem).Key.ToString();

			if (keyName == string.Empty || !this._smartBinning.SmartBin.ContainsBoundary(keyName))
			{
				return;
			}

			//////////////////////////////////////////////////////////////////////
			//Clear Boundary
			//////////////////////////////////////////////////////////////////////
			SmartBoundaryData smartBoundaryData = this._smartBinning.SmartBin.Boundary[keyName];

			smartBoundaryData.Clear();

			//////////////////////////////////////////////////////////////////////
			//Set Boundary
			//////////////////////////////////////////////////////////////////////
			int boundaryCodeIndex = 0;

			int x1Index = 1;

			int y1Index = 2;

			int y3Index = 6;

			for (int row = 0; row < this.dgvBoundary2D.RowCount; row++)
			{
				//if input data != null
				string boundaryCode = string.Empty;

				double data1 = 0;

				double data2 = 0;

				if (this.dgvBoundary2D[boundaryCodeIndex, row].Value != null)
				{
					boundaryCode = this.dgvBoundary2D[boundaryCodeIndex, row].Value.ToString();
				}

				if (this.dgvBoundary2D[x1Index, row].Value != null)
				{
					data1 = double.Parse(this.dgvBoundary2D[1, row].Value.ToString());
				}

				if (this.dgvBoundary2D[y1Index, row].Value != null)
				{
					data2 = double.Parse(this.dgvBoundary2D[2, row].Value.ToString());
				}

				if (data1 == 0 && data2 == 0)
				{
					continue;
				}

				SmartBoundaryBase boundary = null;
				
				if (this.dgvBoundary2D[y3Index, row].Value == null || double.Parse(this.dgvBoundary2D[y3Index, row].Value.ToString()) == 0)
				{
					//Ellipse
					if (keyName.Contains(SmartBinning.CIExyKEY))
					{
						boundary = new SmartEllipse(SmartEllipse.ECIEType.CIE1931);
					}
					else if (keyName.Contains(SmartBinning.CIEupvpKEY))
					{
						boundary = new SmartEllipse(SmartEllipse.ECIEType.CIE1976);
					}

					for (int col = 1; col < y3Index; col++)
					{
						double data = 0;

						if (this.dgvBoundary2D[col, row].Value != null)
						{
							data = double.Parse(this.dgvBoundary2D[col, row].Value.ToString());
						}

						if (col == 1)
						{
							(boundary as SmartEllipse).X = data;
						}
						else if (col == 2)
						{
							(boundary as SmartEllipse).Y = data;
						}
						else if (col == 3)
						{
							(boundary as SmartEllipse).a = data;
						}
						else if (col == 4)
						{
							(boundary as SmartEllipse).b = data;
						}
						else if (col == 5)
						{
							(boundary as SmartEllipse).Theta = data;
						}
					}
				}
				else
				{
					//Polygon
					boundary = new SmartPolygon();

					(boundary as SmartPolygon).Coord.Clear();

					for (int col = x1Index; col < this.dgvBoundary2D.ColumnCount; col += 2)
					{
						float xData = 0;

						float yData = 0;

						if (this.dgvBoundary2D[col, row].Value != null)
						{
							xData = float.Parse(this.dgvBoundary2D[col, row].Value.ToString());
						}

						if (this.dgvBoundary2D[col + 1, row].Value != null)
						{
							yData = float.Parse(this.dgvBoundary2D[col + 1, row].Value.ToString());
						}

						if (xData == 0 && yData == 0 && col > 8)
						{
							break;
						}

						(boundary as SmartPolygon).Coord.Add(new PointF(xData, yData));
					}
				}

				boundary.BoundaryCode = boundaryCode;

				smartBoundaryData.Add(boundary);
			}

			if (smartBoundaryData.Count == 0)
			{
				this._smartBinning.SmartBin.Boundary.Remove(keyName);
			}

			this._smartBinning.SmartBin.CreateBinTable();

			this.CreateNGBinAndSideBin();
		}

		private void SaveBinTable()
		{
			if (!this.dgvBinTable.Visible)
			{
				return;
			}

			this._smartBinning.IsAutoBin = this.chkIsEnableAutoBin.Checked;

			SmartBin smartBin = this._smartBinning.SmartBin;



			for (int row = 0; row < this.dgvBinTable.RowCount; row++)
			{
				DataGridViewRow dgrow = this.dgvBinTable.Rows[row];

				smartBin[row].BinCode = dgrow.Cells[0].Value.ToString();

				smartBin[row].BinNumber = int.Parse(dgrow.Cells[1].Value.ToString());

                //smartBin[row].BoundaryRule
			}
		}

		private void SaveNGBin()
		{
			if (!this.dgvNGBin.Visible)
			{
				return;
			}

			SmartNGBin smartNGBin = this._smartBinning.NGBin;

			for (int row = 0; row < this.dgvNGBin.RowCount; row++)
			{
				DataGridViewRow dgrow = this.dgvNGBin.Rows[row];

				string keyName = dgrow.Tag.ToString();

				if (smartNGBin.ContainsKey(keyName))
				{
					smartNGBin[keyName].IsEnable = bool.Parse(dgrow.Cells[0].Value.ToString());

					smartNGBin[keyName].BinCode = dgrow.Cells[2].Value.ToString();

					smartNGBin[keyName].BinNumber = int.Parse(dgrow.Cells[3].Value.ToString());

					smartNGBin[keyName].NGLowLimit = double.Parse(dgrow.Cells[4].Value.ToString());

					smartNGBin[keyName].NGUpLimit = double.Parse(dgrow.Cells[5].Value.ToString());
				}
			}
		}

		private void SaveSideBin()
		{
			if (!this.dgvSideBin.Visible)
			{
				return;
			}

			SmartSideBin smartSideBin = this._smartBinning.SideBin;

			for (int row = 0; row < this.dgvSideBin.RowCount; row++)
			{
				DataGridViewRow dgrow = this.dgvSideBin.Rows[row];

				string keyName = dgrow.Tag.ToString();

				if (smartSideBin.ContainsKey(keyName))
				{
					smartSideBin[keyName].BinCode = dgrow.Cells[1].Value.ToString();

					smartSideBin[keyName].BinNumber = int.Parse(dgrow.Cells[2].Value.ToString());
				}
			}
		}

		private void CreateNGBinAndSideBin()
		{
			/////////////////////////////////////////////////////////////
			//Add keyName to NG Bin
			/////////////////////////////////////////////////////////////
			SmartNGBin smartNGBin = new SmartNGBin();

			SmartSideBin smartSideBin = new SmartSideBin();
				 
			foreach (var item in this._smartBinning.SmartBin.Boundary)
			{
				string keyName = item.KeyName;

				//NG Bin
				if (this._smartBinning.NGBin.ContainsKey(keyName))
				{
					
					smartNGBin.Add(this._smartBinning.NGBin[keyName]);
				}
				else
				{
					SmartNGData smartNGData = new SmartNGData(this._enableMsrtItem[keyName].Name);

					smartNGData.KeyName = keyName;

					smartNGData.Name = this._enableMsrtItem[keyName].Name;

					smartNGData.Format = this._enableMsrtItem[keyName].Formate;

					smartNGData.NGLowLimit = this._enableMsrtItem[keyName].MinLimitValue;

					smartNGData.NGUpLimit = this._enableMsrtItem[keyName].MaxLimitValue;

					smartNGBin.Add(smartNGData);
				}

				//Side Bin
				if (this._smartBinning.SideBin.ContainsKey(keyName))
				{
					smartSideBin.Add(this._smartBinning.SideBin[keyName]);
				}
				else
				{
					SmartSideData smartSideData = new SmartSideData(this._enableMsrtItem[keyName].Name);

					smartSideData.KeyName = keyName;

					smartSideData.Name = this._enableMsrtItem[keyName].Name;

					smartSideBin.Add(smartSideData);
				}
			}

			/////////////////////////////////////////////////////////////
			//delete not exist item
			/////////////////////////////////////////////////////////////
			for (int i = 0; i < smartNGBin.Count; i++)
			{
				if (!this._smartBinning.SmartBin.ContainsBoundary(smartNGBin[i].KeyName))
				{
					smartNGBin.Remove(smartNGBin[i].KeyName);

					i--;
				}
			}

			for (int i = 0; i < smartSideBin.Count; i++)
			{
				if (!this._smartBinning.SmartBin.ContainsBoundary(smartSideBin[i].KeyName))
				{
					smartSideBin.Remove(smartSideBin[i].KeyName);

					i--;
				}
			}

			this._smartBinning.NGBin = smartNGBin;

			this._smartBinning.SideBin = smartSideBin;
		}

        private static EBinBoundaryRule GetBoundaryRule(string keyName)
        {
            EBinBoundaryRule bR = EBinBoundaryRule.LeValL;
            foreach (var tData in DataCenter._product.TestCondition.TestItemArray)
            {
                foreach (var rData in tData.MsrtResult)
                {
                    if (keyName == rData.KeyName)
                    {
                        bR = rData.BoundaryRule;
                        return bR;
                    }
                }
            }
            return bR;
        }

        private static string GetBoundaryRuleStr(EBinBoundaryRule bRule)
        {
            string sStr = "";
            switch (bRule)
            {
                default:
                case EBinBoundaryRule.LeValL:
                    sStr = "≦ Item <";
                    break;
                case EBinBoundaryRule.LeValLe:
                    sStr = "≦ Item ≦";
                    break;
                case EBinBoundaryRule.LValLe:
                    sStr = "< Item ≦";
                    break;
                case EBinBoundaryRule.LValL:
                    sStr = "< Item <";
                    break;
            }
            return sStr;
        }

        private static EBinBoundaryRule GetBoundaryRuleEnum(string sRule)
        {
            EBinBoundaryRule eRule = EBinBoundaryRule.LeValL;
            if (sRule.Contains("≦ Item <"))
            {
                eRule = EBinBoundaryRule.LeValL;
            }
            else if (sRule.Contains("≦ Item ≦"))
            {
                eRule = EBinBoundaryRule.LeValLe;
            }
            else if (sRule.Contains("< Item ≦"))
            {
                eRule = EBinBoundaryRule.LValLe;
            }
            else if (sRule.Contains("< Item <"))
            {
                eRule = EBinBoundaryRule.LValL;
            }

            return eRule;
        }


        private TestItemData FindTestItemByMsrtKey(string KeyName)
        {
            if (DataCenter._product.TestCondition == null &&
                DataCenter._product.TestCondition.TestItemArray == null)
            {
                return null;
            }
            return DataCenter._product.TestCondition.FindTestItemByMsrtKey(KeyName);
        }

        private TestResultData FindMsrtDataFromMsrtKey(string KeyName)
        {
            if (DataCenter._product.TestCondition == null &&
                DataCenter._product.TestCondition.TestItemArray == null)
            {
                return null;
            }
            return DataCenter._product.TestCondition.FindMsrtDataFromMsrtKey(KeyName);         
        }
        #endregion

        #region >>> Public Method <<<

        public void UpdateDataToUIForm()
		{
			if (this.InvokeRequired)
			{
				this.BeginInvoke(new UpdateDataHandler(UpdateDataToControls), null);
			}
			else
			{
				this.UpdateDataToControls();
			}
		}

		#endregion

		#region >>> UI Event Handler <<<

		private void btnSave_Click(object sender, EventArgs e)
		{
            this.SaveBoundary();

			this.SaveBoundary2D();

			this.SaveBinTable();

			this.SaveNGBin();

			this.SaveSideBin();			

			if (this._smartBinning.SmartBin.Count > SmartBinning.MaxBinCount)
			{
                TopMessageBox.ShowMessage((int)EMessageCode.Tools_BinCountTooMuch, "Bin Count > " + SmartBinning.MaxBinCount.ToString(),string.Empty);

				this._smartBinning = DataCenter._smartBinning;
			}
			else
			{
			DataCenter._smartBinning = this._smartBinning;
			}

			DataCenter.SaveBinFile();

			AppSystem.SetDataToSystem();

			Host.UpdateDataToAllUIForm();
		}

		private void btnReload_Click(object sender, EventArgs e)
		{
			this.UpdateDataToControls();
		}

		private void dgvBoundary_VisibleChanged(object sender, EventArgs e)
		{
			if (this.dgvBoundary.Visible && this.IsHandleCreated)
			{
				this.btnSave.Enabled = true;

				this.btnReload.Enabled = true;

				this.UpdateEnableBoundaryDGV();

				this.UpdateBoundaryToDGV();
			}
		}

		private void dgvBoundary2D_VisibleChanged(object sender, EventArgs e)
		{
			if (this.dgvBoundary2D.Visible && this.IsHandleCreated)
			{
				this.btnSave.Enabled = true;

				this.btnReload.Enabled = true;

				this.UpdateEnableBoundary2DDGV();

				this.UpDateSelectItem();

				this.UpdateBoundary2DToDGV();
			}
		}

		private void dgvBinTable_VisibleChanged(object sender, EventArgs e)
		{
			if (this.dgvBinTable.Visible && this.IsHandleCreated)
			{
				this.btnSave.Enabled = true;

				this.btnReload.Enabled = true;

				this.UpdateBinTableToDGV();
			}
		}

		private void dgvNGBin_VisibleChanged(object sender, EventArgs e)
		{
			if (this.dgvNGBin.Visible && this.IsHandleCreated)
			{
				this.btnSave.Enabled = true;

				this.btnReload.Enabled = true;

				this.UpdateNGBinToDGV();
			}
		}

		private void dgvSideBin_VisibleChanged(object sender, EventArgs e)
		{
			if (this.dgvSideBin.Visible && this.IsHandleCreated)
			{
				this.btnSave.Enabled = true;

				this.btnReload.Enabled = true;

				this.UpdateSideBinToDGV();
			}
		}

		private void dgv_KeyDown(object sender, KeyEventArgs e)
		{
			DataGridView dgv = (sender as DataGridView);

			if (e.Control && e.KeyCode == Keys.V)
			{
				string clipboard = Clipboard.GetText();

				string[] rows = clipboard.Replace('\r'.ToString(), "").Split('\n');

				List<string[]> rowList = new List<string[]>();

				for (int row = 0; row < rows.Length; row++)
				{
					string[] line = rows[row].Split('\t');

					rowList.Add(line);
				}

				int currentRow = dgv.CurrentCell.RowIndex;

				int currentCol = dgv.CurrentCell.ColumnIndex;

				int rowCount = dgv.RowCount;

				int colCount = dgv.ColumnCount;

				for (int i = 0; i < dgv.SelectedCells.Count; i++)
				{
					DataGridViewSelectedCellCollection cell = dgv.SelectedCells;

					if (cell[i].RowIndex < currentRow)
					{
						currentRow = cell[i].RowIndex;
					}

					if (cell[i].ColumnIndex < currentCol)
					{
						currentCol = cell[i].ColumnIndex;
					}
				}

				for (int row = 0; row < rowList.Count; row++)
				{
					for (int col = 0; col < rowList[row].Length; col++)
					{
						int nowCol = col + currentCol;

						int nowrow = row + currentRow;

						if (nowCol >= colCount || nowrow >= rowCount)
						{
							continue;
						}

						if (dgv[nowCol, nowrow].ReadOnly)
						{
							continue;
						}
							
						DataGridViewColumn cell = dgv.Columns[nowCol];

						if (cell.ValueType == typeof(bool))
						{
							bool value = false;

							int oth = 0;

							if (bool.TryParse(rowList[row][col], out value))
							{
								dgv[nowCol, nowrow].Value = value;
							}
							else if (int.TryParse(rowList[row][col], out oth))
							{
								if (oth == 0)
								{
									dgv[nowCol, nowrow].Value = false;
								}
								else
								{
									dgv[nowCol, nowrow].Value = true;
								}
							}
						}
						else if (cell.ValueType == typeof(int))
						{
							int value = 0;

							if (int.TryParse(rowList[row][col], out value))
							{
								dgv[nowCol, nowrow].Value = value;
							}
						}
						else if (cell.ValueType == typeof(double))
						{
							double value = 0;

							if (double.TryParse(rowList[row][col], out value))
							{
								dgv[nowCol, nowrow].Value = value;
							}
						}
						else
						{
							dgv[nowCol, nowrow].Value = rowList[row][col];
						}
						
					}
				}

			}
			else if (e.KeyCode == Keys.Delete)
			{
				for (int i = 0; i < dgv.SelectedCells.Count; i++)
				{
					if (dgv.SelectedCells[i].ReadOnly)
					{
						continue;
					}

					if (dgv.SelectedCells[i].ValueType == typeof(bool))
					{
						dgv.SelectedCells[i].Value = false;
					}
					else if (dgv.SelectedCells[i].ValueType == typeof(int))
					{
						dgv.SelectedCells[i].Value = 0;
					}
					else if (dgv.SelectedCells[i].ValueType == typeof(double))
					{
						dgv.SelectedCells[i].Value = 0;
					}
					else
					{
						dgv.SelectedCells[i].Value = string.Empty;
					}
				}
			}
		}

		private void chkBoundaryAll_CheckedChanged(object sender, EventArgs e)
		{
			if (!this.IsHandleCreated || !(bool)this.chkBoundaryAll.Tag)
			{
				return;
			}

			for (int row = 0; row < this.dgvEnableBoundary.RowCount; row++)
			{
				this.dgvEnableBoundary[0, row].Value = this.chkBoundaryAll.Checked;
			}
		}

		private void chkBoundary2DAll_CheckedChanged(object sender, EventArgs e)
		{
			if (!this.IsHandleCreated || !(bool)this.chkBoundary2DAll.Tag)
			{
				return;
			}

			for (int row = 0; row < this.dgvEnableBoundary2D.RowCount; row++)
			{
				this.dgvEnableBoundary2D[0, row].Value = this.chkBoundary2DAll.Checked;
			}
		}

		private void btnApplySelectBoundary_Click(object sender, EventArgs e)
		{			
			/////////////////////////////////////////////////
			//Get Enable Item
			/////////////////////////////////////////////////
			List<string> enableItem = new List<string>();
			
			for (int row = 0; row < this.dgvEnableBoundary.RowCount; row++)
			{
				if ((bool)this.dgvEnableBoundary[0, row].Value)
				{
					enableItem.Add(this.dgvEnableBoundary.Rows[row].Tag.ToString());
				}
			}

			/////////////////////////////////////////////////
			//By 優先順序排序 enableItem
			/////////////////////////////////////////////////


			/////////////////////////////////////////////////
			//Apply To DGV
			/////////////////////////////////////////////////

			//Create Columns
			SmartBoundary smartBoundary = new SmartBoundary();

			for (int col = 0; col < enableItem.Count; col++)
			{
				string keyName = enableItem[col];

				if (this._smartBinning.SmartBin.ContainsBoundary(keyName))
				{
					smartBoundary.Add(this._smartBinning.SmartBin.Boundary[keyName]);

					if (keyName.Contains(SmartBinning.CIExyKEY) ||
						keyName.Contains(SmartBinning.CIEupvpKEY))
					{
						continue;
					}

					//Create Rows
					while(this.numGrade.Value < smartBoundary[keyName].Count)
					{
						smartBoundary[keyName].Remove(smartBoundary[keyName].Count - 1);
					}

					while (this.numGrade.Value > smartBoundary[keyName].Count)
					{
						smartBoundary[keyName].Add(new SmartLowUp());
					}
				}
				else
				{
					SmartBoundaryData data = new SmartBoundaryData();

					data.KeyName = keyName;

					data.Name = this._enableMsrtItem[keyName].Name;

					data.Format = this._enableMsrtItem[keyName].Formate;

                   

					//Create Rows
					for (int row = 0; row < this.numGrade.Value; row++)
					{
						SmartLowUp boundaryBase = new SmartLowUp();

                        if (DataCenter._sysSetting.IsSpecBinTableSync &&
                            row == 0)
                        {

                            boundaryBase.LowLimit = this._enableMsrtItem[keyName].MinLimitValue;
                            boundaryBase.UpLimit = this._enableMsrtItem[keyName].MaxLimitValue;
                            //foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
                            //{
                            //    if (item.IsEnable == true && item.MsrtResult != null)
                            //    {
                            //        for (int i = 0; i < item.MsrtResult.Length; i++)
                            //        {
                            //            if (item.MsrtResult[i].KeyName == keyName)
                            //            {
                            //                boundaryBase.LowLimit = item.MsrtResult[i].MinLimitValue;
                            //                boundaryBase.UpLimit = item.MsrtResult[i].MaxLimitValue;
                            //            }
                            //        }
                            //    }
                            //}

 
                        }

						data.Add(boundaryBase);
					}

					smartBoundary.Add(data);
				}	
		
			}

			this._smartBinning.SmartBin.Boundary = smartBoundary;

			this.UpdateBoundaryToDGV();
		}

		private void btnApplySelectBoundary2D_Click(object sender, EventArgs e)
		{
			if (this.cmbSelectCIE.SelectedItem == null)
			{
				return;
			}

			/////////////////////////////////////////////////
			//Get Enable Item
			/////////////////////////////////////////////////
			List<string> enableItem = new List<string>();

			for (int row = 0; row < this.dgvEnableBoundary2D.RowCount; row++)
			{
				if ((bool)this.dgvEnableBoundary2D[0, row].Value)
				{
					enableItem.Add(this.dgvEnableBoundary2D.Rows[row].Tag.ToString());
				}
			}

			/////////////////////////////////////////////////
			//By 優先順序排序 enableItem
			/////////////////////////////////////////////////


			/////////////////////////////////////////////////
			//Apply To DGV
			/////////////////////////////////////////////////

			//Create Columns
			SmartBoundary smartBoundary = new SmartBoundary();

			for (int col = 0; col < enableItem.Count; col++)
			{
				string keyName = enableItem[col];

				if (this._smartBinning.SmartBin.ContainsBoundary(keyName))
				{
					smartBoundary.Add(this._smartBinning.SmartBin.Boundary[keyName]);

					if (!keyName.Contains(SmartBinning.CIExyKEY) &&
						!keyName.Contains(SmartBinning.CIEupvpKEY))
					{
						continue;
					}

					if (keyName != ((KeyValuePair<string, string>)this.cmbSelectCIE.SelectedItem).Key.ToString())
					{
						continue;
					}

					//Create Rows
					while (this.numGrade2D.Value < smartBoundary[keyName].Count)
					{
						smartBoundary[keyName].Remove(smartBoundary[keyName].Count - 1);
					}

					while (this.numGrade2D.Value > smartBoundary[keyName].Count)
					{
						smartBoundary[keyName].Add(new SmartPolygon());
					}
				}
				else
				{
					SmartBoundaryData data = new SmartBoundaryData();

					data.KeyName = keyName;

					data.Name = this._enableMsrtItem[keyName].Name;

					data.Format = this._enableMsrtItem[keyName].Formate;

					//Create Rows
					for (int row = 0; row < this.numGrade2D.Value; row++)
					{
						SmartLowUp boundaryBase = new SmartLowUp();

						data.Add(boundaryBase);
					}

					smartBoundary.Add(data);
				}
			}

			this._smartBinning.SmartBin.Boundary = smartBoundary;

			this.UpdateBoundary2DToDGV();
		}

		private void cmbSelectCIE_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.UpdateBoundary2DToDGV();
		}

		#endregion

        private void btnSeqOrderUp_Click(object sender, EventArgs e)
        {
            if (this.dgvEnableBoundary.CurrentRow == null)
                return;

            int selectRowIndex = this.dgvEnableBoundary.CurrentRow.Index;
            int selectColumnIndex = this.dgvEnableBoundary.CurrentCell.ColumnIndex;


            if (selectRowIndex == 0)
                return;

            string keyName = this.dgvEnableBoundary.CurrentRow.Tag.ToString();

            this._enableMsrtItem.ChangeOrderUp(keyName);

            this.UpdateEnableBoundaryDGV();

            this.dgvEnableBoundary.ClearSelection();

            if (selectRowIndex >= 1)
            {
                this.dgvEnableBoundary.CurrentCell = this.dgvEnableBoundary[selectColumnIndex, selectRowIndex - 1];
                this.dgvEnableBoundary.Rows[selectRowIndex - 1].Selected = true;
            }
        }

        private void btnSeqOrderDown_Click(object sender, EventArgs e)
        {
            if (this.dgvEnableBoundary.CurrentRow == null)
                return;

            int selectRowIndex = this.dgvEnableBoundary.CurrentRow.Index;

            int selectColumnIndex = this.dgvEnableBoundary.CurrentCell.ColumnIndex;

            if (selectRowIndex == (this.dgvEnableBoundary.Rows.Count - 1))
                return;


            string keyName = this.dgvEnableBoundary.CurrentRow.Tag.ToString();

            this._enableMsrtItem.ChangeOrderDown(keyName);

            this.UpdateEnableBoundaryDGV();

            this.dgvEnableBoundary.ClearSelection();

            if (selectRowIndex < (this.dgvEnableBoundary.Rows.Count - 1))
            {
                this.dgvEnableBoundary.CurrentCell = this.dgvEnableBoundary[selectColumnIndex, selectRowIndex + 1];
                this.dgvEnableBoundary.Rows[selectRowIndex + 1].Selected = true;
            }
        }
	}

    public class EnableItemsCollection
    {
        private List<TestResultData> _dataList;
        private int _order;

        public EnableItemsCollection()
        {
            this._dataList = new List<TestResultData>();

            this._order = 0;
        }

        #region >>> Public Property <<<

        public TestResultData this[string keyName]
        {
            get 
            {
                if (this._dataList.Count == 0)
                {
                    return null;
                }

                foreach (var item in this._dataList)
                {
                    if (item.KeyName == keyName)
                    {
                        return item;
                    }
                }

                return null;
            }
        }

        public List<TestResultData> Data
        {
            get { return this._dataList; }
        }

        public int Count
        {
            get { return this._dataList.Count; }
        }

        #endregion

        #region >>> Private Method <<<

        private void ChangeOrder(string keyName, int director)
        {
            TestResultData selectedData = this[keyName];

            

            if (selectedData != null)
            {
                for (int i = 0; _dataList != null && i < _dataList.Count; ++i)
                {
                    _dataList[i].Index = i;
                }
                int order = selectedData.Index;

                if (order >= 0 && order < this._dataList.Count)
                {
                    if (director == -1 && order >= 1)
                    {
                        this._dataList[order].Index += director;
                        this._dataList[order + director].Index += (director * (-1));
                    }

                    if (director == 1 && order < (this._dataList.Count - 1))
                    {
                        this._dataList[order].Index += director;
                        this._dataList[order + director].Index += (director * (-1));
                    }

                    this._dataList.Sort(delegate(TestResultData item01, TestResultData item02) {  return Comparer<int>.Default.Compare(item01.Index, item02.Index); } );
                }
            }
        }
               
        #endregion

        #region >>> Public Method <<<

        public bool ContainsKey(string keyName)
        {
            if (this._dataList.Count == 0)
            {
                return false;
            }

            foreach (var item in this._dataList)
            {
                if (item.KeyName == keyName)
                {
                    return true;
                }
            }

            return false;
        }

        public void Add(TestResultData data)
        {
            TestResultData cloneObj = data.Clone() as TestResultData;

            cloneObj.Index = this._order++;

            this._dataList.Add(cloneObj);
        }

        public void ChangeOrderUp(string keyName)
        {
            this.ChangeOrder(keyName, -1);
        }

        public void ChangeOrderDown(string keyName)
        {
            this.ChangeOrder(keyName, 1);
        }

        public void UpdateOrderByBoundary(List<SmartBoundaryData> lst)
        {
            if (lst.Count == 0)
            {
                return;
            }

            for (int order = 0; order < this._dataList.Count; order++)
            {
                string keyName = this._dataList[order].KeyName;
                int reOrder = order;

                for (int refOrder = 0; refOrder < lst.Count; refOrder++)
                {
                    if (lst[refOrder].KeyName == keyName)
                    {
                        reOrder = refOrder;
                        break;
                    }
                }

                this._dataList[order].Index = reOrder;
            }

            this._dataList.Sort(delegate(TestResultData item01, TestResultData item02) { return Comparer<int>.Default.Compare(item01.Index, item02.Index); } );
        }

        public void Clear()
        {
            this._order = 0;
            this._dataList.Clear();
        }

        #endregion
    }
}