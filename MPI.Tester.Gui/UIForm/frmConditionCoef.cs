using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;

using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Gui
{
	public partial class frmConditionCoef : System.Windows.Forms.Form
	{
		public event EventHandler<EventArgs> TestItemDataChangeEvent;

		private BindingSource _bindSource;
		private DataTable _dataTable;
		private DataRow _dataRow;

		private delegate void UpdateDataHandler();

        private Form _frmChannelCali;

        private frmCaliCoeff _frmCaliCoeff;
        private frmChuckCorrection _frmChuckCorrection;
        private frmDailyWatch _frmSptCalibData;
      //  private frmChannelCali _frmChannelCali;

	    private Dictionary<string, FactorBoundary> _dicFatorBoundary = null;

		public frmConditionCoef()
		{
			Console.WriteLine("[frmConditionCoef], frmConditionCoef()");

			InitializeComponent();

			this.InitParamAndCompData();
			this.InitGainOffsetDGV();
			this.InitChuckDGV();
			this.InitOptiCoefTableDGV();

			this.TestItemDataChangeEvent += new EventHandler<EventArgs>(Host.TestItemDataChangeEventHandler);
			this.dgvGainOffset.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.DGVInputCell_CellValidating);
			this.dgvGainOffset.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGVCell_CellEnter);
			this.dgvGainOffset.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGVCell_CellLeave);

			this.dgvGainOffset2.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.DGVInputCell_CellValidating);
			this.dgvGainOffset2.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGVCell_CellEnter);
			this.dgvGainOffset2.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGVCell_CellLeave);

			this.dgvGainOffset3.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.DGVInputCell_CellValidating);
			this.dgvGainOffset3.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGVCell_CellEnter);
			this.dgvGainOffset3.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGVCell_CellLeave);
		}

		#region >>> Private Methods <<<

		private void InitParamAndCompData()
		{
			this.lblCoefTablePathAndFile.Text = "NONE";
			this.cmbCalMode.Items.Clear();
			this.cmbCalMode.Items.AddRange(Enum.GetNames(typeof(ECalMode)));
			this.cmbCalMode.SelectedItem = ECalMode.LookTable.ToString();
			DataCenter._product.TestCondition.CalMode = (ECalMode)Enum.Parse(typeof(ECalMode), this.cmbCalMode.SelectedItem.ToString(), true);
			this.cmbCalMode.Enabled = false;

			this.cmbCalByWave.Items.Clear();
			this.cmbCalByWave.Items.AddRange(Enum.GetNames(typeof(ECalBaseWave)));
	
			//---------------------------------------
			// UI component initialization
			//---------------------------------------
			// First, remove ValueChanged event, set initial data to component
			this.dinStartWL.Value = DataCenter._product.DispCoefStartWL;
			this.dinStartWL.Increment = DataCenter._sysSetting.CoefTableResolution;

			this.dinEndWL.Value = DataCenter._product.DispCoefEndWL;
			this.dinEndWL.Increment = DataCenter._sysSetting.CoefTableResolution;
			
			//---------------------------------------
			// FactorBoundarySetting 
			//---------------------------------------

			 string fileNameAndPath=Path.Combine(Constants.Paths.DATA_FILE,"FactorBoundarySetting.ini");

            if(File.Exists(fileNameAndPath))
            {
                this._dicFatorBoundary = new Dictionary<string, FactorBoundary>();

                List<string[]> data = CSVUtil.ReadCSV(fileNameAndPath);

                bool isStartLoad = false;

                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i][0] == "KeyName" && data[i][1] == "Name")
                    {
                        isStartLoad = true;
                        continue;
                    }

                    if (data[i][0] == "SectionEnd")
                    {
                        break;
                    }

                    if (isStartLoad)
                    {
                        string[] row = data[i];

                        string keyName = row[0];

                        string Name = row[1];

                        int enable = 0;

                        bool isEnable = false; ;

                        int.TryParse(row[2], out enable);

                        int mode = 0;

                        int.TryParse(row[3], out mode);

                        if (enable == 1)
                        {
                            isEnable = true;
                        }

                        double low = 0.0d;

                        double high = 0.0d;


                        double.TryParse(row[4], out low);

                        double.TryParse(row[5], out high);

                        FactorBoundary fb = new FactorBoundary(Name, keyName, isEnable);

                        fb.High = high;

                        fb.Low = low;

                        if (isEnable)
                        {
                            this._dicFatorBoundary.Add(keyName, fb);
                        }
                    }
                }
            }


            if (DataCenter._machineConfig.TesterFunctionType != ETesterFunctionType.Multi_Die)
            {
                this.tabiByChannelCoefTable.Visible = false;
            }

            if (DataCenter._rdFunc.RDFuncData.TesterConfigType == ETesterConfigType.PDTester)
            {
                btnCaliTool.Visible = false;
                tabpChuck.Visible = false;
                dgvChuck.Visible = false;
                labelX1.Visible = false;
            }
		}

		private void InitOptiCoefTableDGV()
		{
			//-----------------------------------------------------------------------
			// DataGridView Setting for OptiCoef Table
			//-----------------------------------------------------------------------
			this._bindSource = new BindingSource();
			this._dataTable = new DataTable();
          
            foreach(string str in Enum.GetNames(typeof(ECoefTableItem)) )
            {
                this._dataTable.Columns.Add(str, Type.GetType("System.Double"));
            }

			this._bindSource.DataSource = this._dataTable;
			this.dgvOpticCoefTable.DataSource = this._bindSource;

			for (int i = 0; i < this.dgvOpticCoefTable.Columns.Count; i++)
			{
				this.dgvOpticCoefTable.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
				this.dgvOpticCoefTable.Columns[i].Width = 75;
				this.dgvOpticCoefTable.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			}

			this.dgvOpticCoefTable.Columns[0].ReadOnly = true;
			this.dgvOpticCoefTable.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			this.dgvOpticCoefTable.Columns[0].DefaultCellStyle.Format = "0.0";
			this.dgvOpticCoefTable.Columns[0].DefaultCellStyle.BackColor = Color.YellowGreen;
            this.dgvOpticCoefTable.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
		}

        private void InitSingleGainOffset(DataGridView dgv, Color backColor)
        {
            //------------------------------------------------------------------------
            // DataGridView Setting for GainOffset Table
            //------------------------------------------------------------------------
            //dgv.Columns[0].ValueType = System.Type.GetType("System.System.Int32");
            //dgv.Columns[1].ValueType = System.Type.GetType("System.String");
            //dgv.Columns[2].ValueType = System.Type.GetType("System.System.Int32");
            //dgv.Columns[3].ValueType = System.Type.GetType("System.Double");
            //dgv.Columns[4].ValueType = System.Type.GetType("System.Double");
            //dgv.Columns[5].ValueType = System.Type.GetType("System.Double");
            //dgv.Columns[6].ValueType = System.Type.GetType("System.String");

            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dgv.Columns[0].DefaultCellStyle.BackColor = backColor;
        }

		private void InitGainOffsetDGV()
		{
			//------------------------------------------------------------------------
			// DataGridView Setting for GainOffset Table
			//------------------------------------------------------------------------
            this.InitSingleGainOffset(this.dgvGainOffset, Color.YellowGreen);
            this.InitSingleGainOffset(this.dgvGainOffset2, Color.SkyBlue);
            this.InitSingleGainOffset(this.dgvGainOffset3, Color.Gold);
            this.InitSingleGainOffset(this.dgvSysCoef, Color.LightGoldenrodYellow);
            this.InitSingleGainOffset(this.dgvPIVCoef, Color.SkyBlue);
		}

		private void InitChuckDGV()
		{
			//------------------------------------------------------------------------
			// DataGridView Setting for Chuck Gain Offset Table
			//------------------------------------------------------------------------
			this.dgvChuck.Columns[0].ValueType = System.Type.GetType("System.Int32");
            this.dgvChuck.Columns[1].ValueType = System.Type.GetType("System.Double");
            this.dgvChuck.Columns[2].ValueType = System.Type.GetType("System.Double");
			this.dgvChuck.Columns[3].ValueType = System.Type.GetType("System.Double");
			this.dgvChuck.Columns[4].ValueType = System.Type.GetType("System.Double");
			this.dgvChuck.Columns[5].ValueType = System.Type.GetType("System.Double");
            this.dgvChuck.Columns[6].ValueType = System.Type.GetType("System.Double");

			for (int i = 0; i < this.dgvChuck.Columns.Count; i++)
			{
				this.dgvChuck.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
			}
			this.dgvChuck.Columns[0].DefaultCellStyle.BackColor = Color.YellowGreen;
		}
        
		private void SetOptiCoefTableEvent( bool isEnable )
		{
			if (isEnable)
			{
				this.dgvOpticCoefTable.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGVCell_CellEnter);
				this.dgvOpticCoefTable.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGVCell_CellLeave);
				this.dgvOpticCoefTable.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.DGVInputCell_CellValidating);
			}
			else
			{
				this.dgvOpticCoefTable.CellEnter -= new System.Windows.Forms.DataGridViewCellEventHandler(this.DGVCell_CellEnter);
				this.dgvOpticCoefTable.CellLeave -= new System.Windows.Forms.DataGridViewCellEventHandler(this.DGVCell_CellLeave);
				this.dgvOpticCoefTable.CellValidating -= new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.DGVInputCell_CellValidating);
			}

		}

        private void UpdateESDGainOffset()
        {
            this.dgvESDGainOffset.Rows.Clear();

            if (DataCenter._product.TestCondition == null || DataCenter._product.TestCondition.TestItemArray == null)
            {
                this.dgvESDGainOffset.Rows.Clear();

                return;
            }

            TestItemData[] testItems = DataCenter._product.TestCondition.TestItemArray;

            foreach (TestItemData item in testItems)
            {
                if (item is ESDTestItem)
                {
                    string[] gainRowData = new string[this.dgvGainOffset.ColumnCount];

                    string[] gainRowData2 = new string[this.dgvGainOffset.ColumnCount];

                      gainRowData[0] = item.KeyName;

                      gainRowData[1] = item.Name;

						gainRowData[2] = (item as ESDTestItem).EsdSetting.GainVolt.ToString();

						gainRowData[3] = (item as ESDTestItem).EsdSetting.OffsetVolt.ToString();

                      this.dgvESDGainOffset.Rows.Add(gainRowData);

                    //多道ESD只有1組產品Gain Offset
                    break;
                }
            }

            this.dgvESDGainOffset.AllowUserToAddRows = false;

            this.dgvESDGainOffset.Update();

            this.dgvESDGainOffset.Refresh();
        }

		private void UpdateDataToGainOffsetDGV()
		{
			this.dgvGainOffset.Rows.Clear();
            this.dgvGainOffset2.Rows.Clear();
            this.dgvGainOffset3.Rows.Clear();

			if (DataCenter._product.TestCondition == null || DataCenter._product.TestCondition.TestItemArray == null)
			{
                this.dgvGainOffset.Rows.Clear();
                this.dgvGainOffset2.Rows.Clear();
                this.dgvGainOffset3.Rows.Clear();
				return;
			}

			TestItemData[] testItems = DataCenter._product.TestCondition.TestItemArray;

			//--------------------------------------------------------------
			// Update Gain-Offset Table data
			//--------------------------------------------------------------
			int itemIndex = 0;
			int gainIndex = 0;

			string[] gainRowData = new string[this.dgvGainOffset.ColumnCount];
            string[] gainRowData2 = new string[this.dgvGainOffset2.ColumnCount];
            string[] gainRowData3 = new string[this.dgvGainOffset3.ColumnCount];

			this.dgvGainOffset.AllowUserToAddRows = true;

			foreach (TestItemData item in testItems)
			{
				if (item.GainOffsetSetting != null)
				{
						for (int i = 0; i < item.GainOffsetSetting.Length; i++)
						{
							if (item.GainOffsetSetting[i].IsEnable == false || item.GainOffsetSetting[i].IsVision == false)
								continue;

							gainRowData[0] = (gainIndex + 1).ToString();
							gainRowData[1] = item.GainOffsetSetting[i].Name;
                            gainRowData2[0] = (gainIndex + 1).ToString();
                            gainRowData2[1] = item.GainOffsetSetting[i].Name;
                            gainRowData3[0] = (gainIndex + 1).ToString();
                            gainRowData3[1] = item.GainOffsetSetting[i].Name;

                            if  (item is LOPWLTestItem )
                            {
                                switch ( UserData.ExtractKeyNameLetter(item.GainOffsetSetting[i].KeyName))
                                {
                                    case "LOP":				// EOptiMsrtType.LOP :
                                        gainRowData[1] = item.GainOffsetSetting[i].Name + "(mcd)";
                                        gainRowData2[1] = item.GainOffsetSetting[i].Name + "(mcd)";
                                        gainRowData3[1] = item.GainOffsetSetting[i].Name + "(mcd)";
                                        break;
									//--------------------------------------------------------------
                                    case "WATT":			// EOptiMsrtType.WATT
                                        gainRowData[1] = item.GainOffsetSetting[i].Name + "(mW)";
                                        gainRowData2[1] = item.GainOffsetSetting[i].Name + "(mW)";
                                        gainRowData3[1] = item.GainOffsetSetting[i].Name + "(mW)";
                                        break;
									//--------------------------------------------------------------
                                    case "LM":				// EOptiMsrtType.LM
                                        gainRowData[1] = item.GainOffsetSetting[i].Name + "(lm)";
                                        gainRowData2[1] = item.GainOffsetSetting[i].Name + "(lm)";
                                        gainRowData3[1] = item.GainOffsetSetting[i].Name + "(lm)";
                                        break;
									//--------------------------------------------------------------
                                    default:
                                        gainRowData[1] = item.GainOffsetSetting[i].Name;
                                        gainRowData2[1] = item.GainOffsetSetting[i].Name;
                                        gainRowData3[1] = item.GainOffsetSetting[i].Name;
                                        break;
                                }
                            }
                            else
                            {
                                gainRowData[1] = item.GainOffsetSetting[i].Name;
                                gainRowData2[1] = item.GainOffsetSetting[i].Name;
                                gainRowData3[1] = item.GainOffsetSetting[i].Name;
                            }

							//--------------------------------------------------------------
                            //gain offset 1
							//--------------------------------------------------------------
							gainRowData[2] = ((Int32)item.GainOffsetSetting[i].Type).ToString();
							gainRowData[3] = item.GainOffsetSetting[i].Square.ToString();
							gainRowData[4] = item.GainOffsetSetting[i].Gain.ToString();
							gainRowData[5] = item.GainOffsetSetting[i].Offset.ToString();
							gainRowData[6] = item.GainOffsetSetting[i].KeyName;

							//--------------------------------------------------------------
                            //gain offset 2
							//--------------------------------------------------------------
							gainRowData2[2] = ((Int32)item.GainOffsetSetting[i].Type2).ToString();
                            gainRowData2[3] = item.GainOffsetSetting[i].Square2.ToString();
                            gainRowData2[4] = item.GainOffsetSetting[i].Gain2.ToString();
                            gainRowData2[5] = item.GainOffsetSetting[i].Offset2.ToString();
                            gainRowData2[6] = item.GainOffsetSetting[i].KeyName;

							//--------------------------------------------------------------
                            //gain offset 3
							//--------------------------------------------------------------
							gainRowData3[2] = ((Int32)item.GainOffsetSetting[i].Type3).ToString();
                            gainRowData3[3] = item.GainOffsetSetting[i].Square3.ToString();
                            gainRowData3[4] = item.GainOffsetSetting[i].Gain3.ToString();
                            gainRowData3[5] = item.GainOffsetSetting[i].Offset3.ToString();
                            gainRowData3[6] = item.GainOffsetSetting[i].KeyName;

							this.dgvGainOffset.Rows.Add(gainRowData);
                            this.dgvGainOffset2.Rows.Add(gainRowData2);
                            this.dgvGainOffset3.Rows.Add(gainRowData3);

                            //if ((itemIndex % 2) == 0)
                            //{
                            //    this.dgvGainOffset.Rows[gainIndex].DefaultCellStyle.BackColor = Color.White;
                            //    this.dgvGainOffset.Rows[gainIndex].Cells[0].Style.BackColor = Color.YellowGreen;
                            //}
                            //else
                            //{
                            //    this.dgvGainOffset.Rows[gainIndex].DefaultCellStyle.BackColor = Color.LightGray;
                            //    this.dgvGainOffset.Rows[gainIndex].Cells[0].Style.BackColor = Color.YellowGreen;
                            //}
							gainIndex++;
					}
				}
				itemIndex++;
			}

			//this.dgvGainOffset.Columns[0].DefaultCellStyle.BackColor = Color.GreenYellow;

			this.dgvGainOffset.AllowUserToAddRows = false;
            this.dgvGainOffset2.AllowUserToAddRows = false;
            this.dgvGainOffset3.AllowUserToAddRows = false;

			this.dgvGainOffset.Update();
            this.dgvGainOffset2.Update();
            this.dgvGainOffset3.Update();

			this.dgvGainOffset.Refresh();
            this.dgvGainOffset2.Refresh();
            this.dgvGainOffset3.Refresh();
		
		}

		private void UpdateDataToChuckDGV()
		{
			this.dgvChuck.Rows.Clear();
            this.dgvChuckResistance.Rows.Clear();

			if (DataCenter._product.ChuckLOPCorrectArray == null )
			{
				this.dgvChuck.Update();
				return;
			}

            if (DataCenter._uiSetting.UserID == EUserID.EpiStar)
            {
                this.tabGainOffset3.Visible = true;
            }


			//--------------------------------------------------------------
			// Update Chuck Gain Offset Table data
			//--------------------------------------------------------------
			int gainIndex = 0;

			string[] rowData = new string[this.dgvChuck.ColumnCount];
			this.dgvChuck.AllowUserToAddRows = true;

			foreach (GainOffsetData[] data in DataCenter._product.ChuckLOPCorrectArray)
			{
				if ( data == null )
					continue;

				if ( gainIndex > 7 )
					break;

				rowData[0] = (gainIndex + 1).ToString();
				rowData[1] = data[0].Gain.ToString();
				rowData[2] = data[0].Offset.ToString();
                rowData[3] = data[1].Gain.ToString();
                rowData[4] = data[1].Offset.ToString();
                rowData[5] = data[2].Gain.ToString();
                rowData[6] = data[2].Offset.ToString();

				this.dgvChuck.Rows.Add(rowData);
				gainIndex++;
			}

			this.dgvChuck.Columns[0].DefaultCellStyle.BackColor = Color.GreenYellow;
			this.dgvChuck.AllowUserToAddRows = false;
			this.dgvChuck.Update();
			this.dgvChuck.Refresh();

            gainIndex = 0;

			if (DataCenter._product.ChuckResistanceCorrectArray != null)
			{
				foreach (double data in DataCenter._product.ChuckResistanceCorrectArray)
				{

					if (gainIndex > 7)
						break;

					rowData[0] = (gainIndex + 1).ToString();
					rowData[1] = data.ToString();

					this.dgvChuckResistance.Rows.Add(rowData);
					gainIndex++;
				}
			}

            this.dgvChuckResistance.Columns[0].DefaultCellStyle.BackColor = Color.GreenYellow;
            this.dgvChuckResistance.AllowUserToAddRows = false;
            this.dgvChuckResistance.Update();
            this.dgvChuckResistance.Refresh();
		}
		
		private void UpdateDataToCoefTable()
		{
            string keyNmae=string.Empty;

            if (this.cmbLopItems.SelectedIndex >= 0)
            {
                string name = this.cmbLopItems.SelectedItem.ToString();

                if (DataCenter._uiSetting.UserDefinedData.TestItemNameDic.ContainsValue(name))
                {
                    foreach (var item in DataCenter._uiSetting.UserDefinedData.TestItemNameDic)
                    {
                        if (item.Value == name)
                        {
                            keyNmae = item.Key;
                            break;
                        }
                    }
                }
            }

			if (DataCenter._product.TestCondition == null || DataCenter._product.TestCondition.TestItemArray == null)
			{
                this._dataTable.Clear();
				this.dgvOpticCoefTable.Update();
				return;
			}

			TestItemData[] testItems = DataCenter._product.TestCondition.TestItemArray;

			//--------------------------------------------------------------
			// Update Optic Coef. Table data
			//--------------------------------------------------------------
			int LOPWL_count = DataCenter._conditionCtrl.GetSubItemCount(ETestType.LOPWL);
			object[] rowData = new string[this._dataTable.Columns.Count];
			double[][] coefTable = null;

				this._dataTable.Clear();

            //if (this.numTableNum.Value >= LOPWL_count)  // this.numTableNum.Value is 1-base
            //{
            //    this.numTableNum.Value = LOPWL_count;
            //}

			this.SetOptiCoefTableEvent(false);
			this.dgvOpticCoefTable.SuspendLayout();

			for (int i = 0; i < testItems.Length; i++)
			{
				if (testItems[i] is LOPWLTestItem)
				{
                   // if (testItems[i].KeyName == "LOPWL_" + this.numTableNum.Value.ToString("0"))
				   // if ((LOPWL_num + 1) == this.numTableNum.Value)	// 1- base
                    if (testItems[i].KeyName==keyNmae)
					{
						coefTable = (testItems[i] as LOPWLTestItem).CoefTable;
						break;
					}
				}
			}

			this._bindSource.RaiseListChangedEvents = false;

			if (coefTable != null)
			{
				this._dataTable.Clear();

				for (int row = 0; row < coefTable.Length; row++)
				{
					this._dataRow = this._dataTable.NewRow();

					for (int k = 0; k < rowData.Length; k++)
					{
						rowData[k] = coefTable[row][k].ToString();
					}
					this._dataRow.ItemArray= rowData;
					this._dataTable.Rows.Add(rowData);

					if ((row % 300) == 0)
					{
						this._bindSource.RaiseListChangedEvents = true;
						this._bindSource.ResetBindings(false);
						//System.Windows.Forms.Application.DoEvents();
						this._bindSource.RaiseListChangedEvents = false;
					}
				}
			}
			else
			{
				return;
			}

			this._bindSource.RaiseListChangedEvents = true;
			//this.dgvOpticCoefTable.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

			// Change DataGridView Range by filter function
			if (DataCenter._product.DispCoefStartWL < (DataCenter._product.DispCoefEndWL + DataCenter._sysSetting.CoefTableResolution))
			{
				this._bindSource.RemoveFilter();
				this._bindSource.Filter = "Wave >=" + DataCenter._product.DispCoefStartWL.ToString() + " AND Wave <=" + DataCenter._product.DispCoefEndWL.ToString();
			}

			this.SetOptiCoefTableEvent(true);
			this.dgvOpticCoefTable.ResumeLayout();
			this.dgvOpticCoefTable.Refresh();
		}

		private void UpdateDataToDGV()
		{
			this.UpdateDataToGainOffsetDGV();

			this.UpdateDataToChuckDGV();

            this.UpdateESDGainOffset();

			this.UpdateDataToCoefTable();

            this.UpdateDataToByChannelCoefDGV();  // Roy, 20140410 Multi-Die By Channel Calibration

            this.UpdateDataToSystemCoefDGV();

            this.UpdateDataToPIVCoefDGV();
		}
   
        private void SaveESDGainOffset()
        {
            if (DataCenter._product.TestCondition == null || DataCenter._product.TestCondition.TestItemArray == null)
                return;

            TestItemData[] testItems = DataCenter._product.TestCondition.TestItemArray;

            for (int row = 0; row < this.dgvESDGainOffset.Rows.Count; row++)
            {
                foreach (TestItemData item in testItems)
                {
                    if (item is ESDTestItem)
                    {
                        if (this.dgvESDGainOffset[0, row].Value.ToString() == item.KeyName)
                        {
                            (item as ESDTestItem).EsdSetting.GainVolt = Convert.ToDouble(this.dgvESDGainOffset[2, row].Value.ToString());

                            (item as ESDTestItem).EsdSetting.OffsetVolt = Convert.ToInt16(this.dgvESDGainOffset[3, row].Value.ToString());

                            //多道ESD只有1組產品Gain Offset
                            break;
                        }
                    }
                }
            }
        }

        private void SaveGainAndTableData()
		{
			if (DataCenter._product.TestCondition == null || DataCenter._product.TestCondition.TestItemArray == null)
				return;

			TestItemData[] testItems = DataCenter._product.TestCondition.TestItemArray;

			int row = 0;
			double[][] coefTable = null;		
			int LOPWL_Count = DataCenter._conditionCtrl.GetSubItemCount(ETestType.LOPWL );

			//--------------------------------------------------------------------
			// Confirm and Save OptiCoefTable Data
			//--------------------------------------------------------------------

            string keyName = string.Empty;

            if (this.cmbLopItems.SelectedIndex >= 0)
            {
                string name = this.cmbLopItems.SelectedItem.ToString();

                if (DataCenter._uiSetting.UserDefinedData.TestItemNameDic.ContainsValue(name))
                {
                    foreach (var s in DataCenter._uiSetting.UserDefinedData.TestItemNameDic)
                    {
                        if (s.Value == name)
                        {
                            keyName = s.Key;
                        }
                    }
                }
            }


            if(keyName!=string.Empty)
            {

            for (int i = 0; i < testItems.Length; i++)
            {
                if (testItems[i] is LOPWLTestItem)
                {
                        if (testItems[i].KeyName == keyName)
                    {
                        coefTable = (testItems[i] as LOPWLTestItem).CoefTable;
                        for (row = 0; row < this._dataTable.Rows.Count; row++)
                        {
                            for (int col = 0; col < this._dataTable.Columns.Count; col++)
                            {
                                coefTable[row][col] = double.Parse(this._dataTable.Rows[row].ItemArray[col].ToString());
                            }
                        }
                    }

                }
            }

			DataCenter._conditionCtrl.ReloadCoefTableList();

            }

			//--------------------------------------------------------------------
			// Confirm and Save GainAndOffset Data
			//--------------------------------------------------------------------
			row = 0;

			for (row = 0; row < this.dgvGainOffset.Rows.Count; row++)
			{
		    	foreach (TestItemData item in testItems)
				{
					if (item.GainOffsetSetting != null)
					{
						GainOffsetData[] data = item.GainOffsetSetting;
						for (int i = 0; i < data.Length; i++)
						{
							if (this.dgvGainOffset[6, row].Value.ToString() == data[i].KeyName)
							{
								data[i].Type = (EGainOffsetType)Enum.Parse(typeof(EGainOffsetType), this.dgvGainOffset[2, row].Value.ToString(), true);
								data[i].Square = Convert.ToDouble(this.dgvGainOffset[3, row].Value.ToString());
								data[i].Gain = Convert.ToDouble(this.dgvGainOffset[4, row].Value.ToString());
								data[i].Offset = Convert.ToDouble(this.dgvGainOffset[5, row].Value.ToString());
								break;
							}
						}
                        //Gain Offset 2
  
                        for (int i = 0; i < data.Length; i++)
                        {
                            if (this.dgvGainOffset2[6, row].Value.ToString() == data[i].KeyName)
                            {
								data[i].Type2 = (EGainOffsetType)Enum.Parse(typeof(EGainOffsetType), this.dgvGainOffset2[2, row].Value.ToString(), true);
                                data[i].Square2 = Convert.ToDouble(this.dgvGainOffset2[3, row].Value.ToString());
                                data[i].Gain2 = Convert.ToDouble(this.dgvGainOffset2[4, row].Value.ToString());
                                data[i].Offset2= Convert.ToDouble(this.dgvGainOffset2[5, row].Value.ToString());
                                break;
                            }
                        }

                        //Gain Offset 3
                        for (int i = 0; i < data.Length; i++)
                        {
                            if (this.dgvGainOffset3[6, row].Value.ToString() == data[i].KeyName)
                            {
								data[i].Type3 = (EGainOffsetType)Enum.Parse(typeof(EGainOffsetType), this.dgvGainOffset3[2, row].Value.ToString(), true);
                                data[i].Square3 = Convert.ToDouble(this.dgvGainOffset3[3, row].Value.ToString());
                                data[i].Gain3 = Convert.ToDouble(this.dgvGainOffset3[4, row].Value.ToString());
                                data[i].Offset3 = Convert.ToDouble(this.dgvGainOffset3[5, row].Value.ToString());
                                break;
                            }
                        }

					}
				}
			}


            DataCenter._product.Resistance = (double)this.dinResistence.Value;

			//// Fire event
			//this.Fire_TestItemDataChangeEvent();
		}

		private void SaveChuckGainAndOffset()
		{
			if (DataCenter._product.ChuckLOPCorrectArray == null )
				return;
			
			//--------------------------------------------------------------------
			// Confirm and Save GainAndOffset Data
			//--------------------------------------------------------------------
			int row = 0;

			for (row = 0; row < this.dgvChuck.Rows.Count; row++)
			{
				GainOffsetData[] data = DataCenter._product.ChuckLOPCorrectArray[row];
				if ( data == null )
					continue;

                data[0].Gain = Convert.ToDouble(this.dgvChuck[1, row].Value.ToString());
				data[0].Offset = Convert.ToDouble(this.dgvChuck[2, row].Value.ToString());
                data[1].Gain = Convert.ToDouble(this.dgvChuck[3, row].Value.ToString());
				data[1].Offset = Convert.ToDouble(this.dgvChuck[4, row].Value.ToString());
                data[2].Gain = Convert.ToDouble(this.dgvChuck[5, row].Value.ToString());
				data[2].Offset = Convert.ToDouble(this.dgvChuck[6, row].Value.ToString());
			}

            if (DataCenter._product.ChuckResistanceCorrectArray == null)
                return;

            for (row = 0; row < this.dgvChuckResistance.Rows.Count; row++)
            {
                DataCenter._product.ChuckResistanceCorrectArray[row] = Convert.ToDouble(this.dgvChuckResistance[1, row].Value.ToString());
            }

		}

        private void SaveDataToFile()
        {
            this.SaveGainAndTableData();

            this.SaveESDGainOffset();

            this.SaveChuckGainAndOffset();

            this.SaveByChannelCoefTable();

            this.SaveSystemCoefTable();

            this.SavePIVGainOffset();
        }
		
		private void ChangeAuthority()
		{
			switch (DataCenter._uiSetting.AuthorityLevel)
			{
				case EAuthority.Operator:
                    this.gplTop.Enabled = false;
                    this.btnResetGainOffset.Enabled = false;
                    this.btnRestChuckGainOffset.Enabled = false;
                    this.btnDailyWatch.Enabled = false;
                    this.btnResetCoef.Enabled = false;
                    this.btnSetCoefRange.Enabled = false;
                                        
                    this.btnResetRcCoefTable.Enabled = false;

                    this.chkEnableByChannelCompensate.Enabled = false;
					break;
				//-------------------------------------------------------------------
				case EAuthority.Engineer:
				case EAuthority.QC:
                case EAuthority.Admin:
				case EAuthority.Super:
                    this.gplTop.Enabled = true;
					this.btnResetGainOffset.Enabled = true;
					this.btnRestChuckGainOffset.Enabled = true;
					this.btnDailyWatch.Enabled = true;
					this.btnResetCoef.Enabled = true;
					this.btnSetCoefRange.Enabled = true;


                    this.btnResetRcCoefTable.Enabled = true;
     
                    this.chkEnableByChannelCompensate.Enabled = true;
					break;
				default:
                    this.gplTop.Enabled = false;
                    this.btnResetGainOffset.Enabled = false;
                    this.btnRestChuckGainOffset.Enabled = false;
                    this.btnDailyWatch.Enabled = false;
                    this.btnResetCoef.Enabled = false;
                    this.btnSetCoefRange.Enabled = false;

                    this.btnResetRcCoefTable.Enabled = false;
          
                    this.chkEnableByChannelCompensate.Enabled = false;
					break;

			}

			///////////////////////////////////////////////////////////////////
			// 2013.06.10 Stanley, 光磊OP權限要能夠做校正
			///////////////////////////////////////////////////////////////////
			if (DataCenter._uiSetting.UserID == EUserID.OptoTech)
			{
				this.gplTop.Enabled = this.CheckBtnCaliToolEnableStatus();
				this.btnCaliTool.Enabled = this.CheckBtnCaliToolEnableStatus();
				this.btnImportCoef.Enabled = this.CheckBtnCaliToolEnableStatus();
				this.btnExportCoef.Enabled = this.CheckBtnCaliToolEnableStatus();
			}
		}

		private bool CheckBtnCaliToolEnableStatus()
		{
			if (DataCenter._userManag.CurrentUserName == "simulator")
			{
				return true;
			}

			if (	DataCenter._machineConfig.Enable.IsSimulator == false &&
					DataCenter._machineConfig.SpectrometerModel != ESpectrometerModel.NONE &&
					DataCenter._machineConfig.SourceMeterModel != ESourceMeterModel.NONE)
			{
				// Gilbert Error
				if (AppSystem._MPITesterKernel.Status.State == TestKernel.EKernelState.Not_Ready)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
			else
			{
				return false;
			}
		}

		private void UpdateDataToControls()
		{
            //--------------------------------------------
            // Update data to Controls
            //--------------------------------------------
			this.btnCaliTool.Enabled = this.CheckBtnCaliToolEnableStatus();
			this.cmbCalMode.Text = DataCenter._product.TestCondition.CalMode.ToString();
			this.cmbCalByWave.Text = DataCenter._product.TestCondition.CalByWave.ToString();
            this.dinStartWL.Value = DataCenter._product.DispCoefStartWL;
            this.dinEndWL.Value = DataCenter._product.DispCoefEndWL;
            this.dinResistence.Value = DataCenter._product.Resistance;

 			this.lblCoefTablePathAndFile.Text = DataCenter._uiSetting.ImportCalibrateFileName;

            this.chkEnableSystemCoefCompensate.Checked = DataCenter._product.IsApplySystemCoef;

            if (DataCenter._product.TestCondition.ChannelConditionTable != null)
            {
                this.chkEnableByChannelCompensate.Checked = DataCenter._product.TestCondition.ChannelConditionTable.IsApplyByChannelCompensate;

                this.dinNormalizedByChannel.MaxValue = DataCenter._product.TestCondition.ChannelConditionTable.Count;
            }
            else
            {
                this.chkEnableByChannelCompensate.Checked = false;
            }

			//--------------------------------------------
			// Update the "Coef. Table Tab"
			//--------------------------------------------
			int count = DataCenter._conditionCtrl.GetSubItemCount(ETestType.LOPWL);

			if ((count != 0) && (this.numTableNum.MaxValue < count))
			{
				this.numTableNum.MaxValue = count;
				this.numTableNum.MinValue = 1;
			}
			else if ((count != 0) && (this.numTableNum.MaxValue > count))
			{
				this.numTableNum.MaxValue = count;
				this.numTableNum.MinValue = 1;
				this.numTableNum.Value = count;
			}
			else if (count == 0)
			{
				this.numTableNum.MaxValue = 0;
				this.numTableNum.MinValue = 0;
				this.numTableNum.Value = 0;
			}

            List<string> createLopItem = new List<string>();

            if (DataCenter._product.TestCondition.TestItemArray != null)
            {
                foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
                {
                    if (item is LOPWLTestItem)
                    {
                        createLopItem.Add(item.Name);
                    }
                }
            }

            this.cmbLopItems.Items.Clear();

            this.cmbLopItems.Items.AddRange(createLopItem.ToArray());

            if (this.cmbLopItems.Items.Count > 0)
            {
                this.cmbLopItems.SelectedIndex = 0;
            }

            //----------------------------------------
            // update ChuckTable 
            //----------------------------------------
            if (DataCenter._machineConfig.TesterCommMode == ETesterCommMode.TCPIP)
            {
                this.tabiChuck.Visible = true;
            }
            else
            {
                this.tabiChuck.Visible = false;
            }
			//----------------------------------------
			// Update the data grid view
			//----------------------------------------
			this.UpdateDataToDGV();

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
                    this.gplTop.Enabled = false;
                    this.btnResetGainOffset.Enabled = false;
                    this.btnRestChuckGainOffset.Enabled = false;
                    this.btnDailyWatch.Enabled = false;
                    this.btnResetCoef.Enabled = false;
                    this.btnSetCoefRange.Enabled = false;
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
                this.gplTop.Enabled = true;
                //this.btnConfirm.Enabled = true;
            }
            else
            {
                this.gplTop.Enabled = false;
              //  this.btnConfirm.Enabled = false;
            }

			if (DataCenter._uiSetting.UserDefinedData.IsEnableShowCalibrateFileLink) //1 :Emable //0 Disable
            {
                lblCalibrateFileName.Visible = true;
                btnUpdateCalibrationFile.Visible = true;
                lblCoefTablePathAndFile.Visible = true;
            }
            else
            {
             //   lblCalibrateFileName.Visible = false;
                btnUpdateCalibrationFile.Visible = false;
             //   lblCoefTablePathAndFile.Visible = false;
            }

			
		  if (DataCenter._uiSetting.ImportCalibrateFileName == string.Empty)
            {
                this.lblCoefTablePathAndFile.Text = "NONE";
            }

              
            if (DataCenter._uiSetting.AuthorityLevel == EAuthority.Super)
            {
                this.tabiPIV.Visible = true;
                this.tabiESD.Visible = true;

                this.tabiByChannelCoefTable.Visible = (DataCenter._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Die ||
                                DataCenter._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Pad);
                this.tabcCoefTable.Visible = this.tabiByChannelCoefTable.Visible;
            } 
            else 
            {
                switch (DataCenter._rdFunc.RDFuncData.TesterConfigType)
                {
                    case ETesterConfigType.LDTester:
                        {
                            this.tabiPIV.Visible = true;
                            this.tabcCoefTable.Visible = false;
                            break;
                        }
                    case ETesterConfigType.PDTester:
                        {
                            this.tabiCoefTable.Visible = false;
                            this.tabiByChannelCoefTable.Visible = (DataCenter._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Die ||
                                DataCenter._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Pad) ;//20200207 David

                            this.tabcCoefTable.Visible = this.tabiByChannelCoefTable.Visible;
                            

                            this.tabiPIV.Visible = false;
                            this.tabiESD.Visible = false;
                            break;
                        }
                    default:
                        {
                            this.tabiPIV.Visible = false;
                            break;
                        }
                  }
            }
		}
     		
		private void ResetOptiCoefTable()
		{
			int index = 0;

			if (DataCenter._product.TestCondition == null || DataCenter._product.TestCondition.TestItemArray == null)
				return;

            if (this.cmbLopItems.SelectedIndex >= 0)
            {
                string name = this.cmbLopItems.SelectedItem.ToString();

			foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
			{
				if ((item is LOPWLTestItem))
				{
                        if (item.Name == name)		// this.numTableNum is 1-base
					{
						(item as LOPWLTestItem).CreateCoefTable(DataCenter._sysSetting.CoefTableStartWL,
																		DataCenter._sysSetting.CoefTableEndWL,
																		DataCenter._sysSetting.CoefTableResolution);
					}
					index++;
				}
			}
            }

			this.UpdateDataToCoefTable();
			this.SaveGainAndTableData();
			//this.Fire_UpdateConditionDataEvent();
		}

        private void UpdateDataToByChannelCoefDGV()
        {
            this.dgvByChannelCoefTable.Rows.Clear();

            if (!this.chkEnableByChannelCompensate.Checked || this.tabiByChannelCoefTable.Visible == false)
            {
                return;
            }

            ChannelConditionTable conditionTable = DataCenter._product.TestCondition.ChannelConditionTable;

            if (conditionTable == null)
            {
                return;
            }

            if (conditionTable.Channels.Length == 0)
            {
                return;
            }

            // If Talbe Channel Row/Col Count is not match to Machine setting
            if (conditionTable.ColXCount != DataCenter._machineConfig.ChannelConfig.ColXCount || conditionTable.RowYCount != DataCenter._machineConfig.ChannelConfig.RowYCount)
            {
                return;
            }

            if (!conditionTable.IsEnable)
            {
                return;
            }

            TestItemData[] testItems = DataCenter._product.TestCondition.TestItemArray;

            if (testItems == null)
            {
                return;
            }

            this.dgvByChannelCoefTable.SuspendLayout();

            string gainKeyName = string.Empty;

            int rowCount = 0;

            int index = 0;

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

                        this.dgvByChannelCoefTable.Rows.Add();

                        this.dgvByChannelCoefTable.Rows[rowCount].Cells[0].Value = (rowCount + 1).ToString();

                        if ((item is LOPWLTestItem) && data.KeyName.IndexOf("_") > 0)
                        {
                            gainKeyName = data.KeyName.Remove(data.KeyName.IndexOf("_"));

                            switch (gainKeyName)
                            {
                                case "LOP":        // EOptiMsrtType.LOP :
                                    this.dgvByChannelCoefTable.Rows[rowCount].Cells[1].Value = coef.Name + " (mcd)";
                                    break;
                                //------------------------------------------------
                                case "WATT":       // EOptiMsrtType.WATT
                                    this.dgvByChannelCoefTable.Rows[rowCount].Cells[1].Value = coef.Name + " (mW)";
                                    break;
                                //------------------------------------------------
                                case "LM":         // EOptiMsrtType.LM
                                    this.dgvByChannelCoefTable.Rows[rowCount].Cells[1].Value = coef.Name + " (lm)";
                                    break;
                                //------------------------------------------------
                                default:
                                    this.dgvByChannelCoefTable.Rows[rowCount].Cells[1].Value = coef.Name;
                                    break;
                            }
                        }
                        else
                        {
                            this.dgvByChannelCoefTable.Rows[rowCount].Cells[1].Value = coef.Name;
                        }

                        this.dgvByChannelCoefTable.Rows[rowCount].Cells[2].Value = (channel + 1).ToString();
                        this.dgvByChannelCoefTable.Rows[rowCount].Cells[3].Value = coef.Type;
                        this.dgvByChannelCoefTable.Rows[rowCount].Cells[4].Value = coef.Square;
                        this.dgvByChannelCoefTable.Rows[rowCount].Cells[5].Value = coef.Gain;
                        this.dgvByChannelCoefTable.Rows[rowCount].Cells[6].Value = coef.Offset;
                        this.dgvByChannelCoefTable.Rows[rowCount].Cells[7].Value = coef.KeyName;

                        if ((index % 2) != 0)
                        {
                            this.dgvByChannelCoefTable.Rows[rowCount].DefaultCellStyle.BackColor = Color.AliceBlue;
                        }

                        rowCount++;
                    }

                    index++;
                }
            }

            this.dgvByChannelCoefTable.ResumeLayout();
        }

        private void SaveByChannelCoefTable()
        {
            if (DataCenter._product.TestCondition == null || DataCenter._product.TestCondition.TestItemArray == null || this.tabiByChannelCoefTable.Visible == false)
                return;

            int row = 0;

            ChannelConditionTable saveTable = DataCenter._product.TestCondition.ChannelConditionTable;

            saveTable.IsApplyByChannelCompensate = this.chkEnableByChannelCompensate.Checked;

            if (!saveTable.IsApplyByChannelCompensate)
            {
                return;
            }

            for (row = 0; row < this.dgvByChannelCoefTable.Rows.Count; row++)
            {
                string keyName = this.dgvByChannelCoefTable.Rows[row].Cells[7].Value.ToString();
                uint channel = Convert.ToUInt32(this.dgvByChannelCoefTable.Rows[row].Cells[2].Value.ToString()) - 1;

                GainOffsetData coef = saveTable.Channels[channel].GetByChannelGainOffsetData(keyName);

                if (coef != null)
                {
                    coef.Gain = Convert.ToDouble(this.dgvByChannelCoefTable.Rows[row].Cells[5].Value);
                    coef.Offset = Convert.ToDouble(this.dgvByChannelCoefTable.Rows[row].Cells[6].Value);
                }
            }
        }

        private void SaveSystemCoefTable()
        {
            DataCenter._product.IsApplySystemCoef = this.chkEnableSystemCoefCompensate.Checked;
        }

        private void UpdateDataToSystemCoefDGV()
        {
            this.dgvSysCoef.Rows.Clear();

            if (!this.chkEnableSystemCoefCompensate.Checked || this.tabiSysCoef.Visible == false)
            {
                return;
            }

            TestItemData[] testItems = DataCenter._product.TestCondition.TestItemArray;

            if (testItems == null || testItems.Length == 0)
            {
                return;
            }

            SystemCaliData sysCalData = DataCenter._sysCali.SystemCaliData;

            if (sysCalData == null || sysCalData.ToolFactor == null || sysCalData.ToolFactor.Count == 0)
            {
                return;
            }

            this.dgvSysCoef.SuspendLayout();

            int rowCount = 0;

            foreach (TestItemData item in testItems)
            {
                if (item.GainOffsetSetting == null || item.GainOffsetSetting.Length == 0)
                    continue;

                foreach (GainOffsetData data in item.GainOffsetSetting)
                {
                    if (!data.IsEnable || !data.IsVision)
                        continue;

                    if (sysCalData.ContainsKey(data.KeyName))
                    {
                        GainOffsetData coef = sysCalData[data.KeyName];
                        
                        this.dgvSysCoef.Rows.Add();

                        this.dgvSysCoef.Rows[rowCount].Cells[0].Value = (rowCount + 1).ToString();  // No.
                        this.dgvSysCoef.Rows[rowCount].Cells[1].Value = coef.Name;    // Name
                        this.dgvSysCoef.Rows[rowCount].Cells[2].Value = coef.Type;    // Type
                        this.dgvSysCoef.Rows[rowCount].Cells[3].Value = coef.Square;  // Square
                        this.dgvSysCoef.Rows[rowCount].Cells[4].Value = coef.Gain;    // Gain
                        this.dgvSysCoef.Rows[rowCount].Cells[5].Value = coef.Offset;  // Offset
                        this.dgvSysCoef.Rows[rowCount].Cells[6].Value = coef.KeyName; // KeyName

                        rowCount++;
                    }
                }
            }

            this.dgvSysCoef.ResumeLayout();
        }

        private void UpdateDataToPIVCoefDGV()
        {
            this.dgvPIVCoef.Rows.Clear();

            if (this.tabiPIV.Visible == false)
            {
                return;
            }

            TestItemData[] testItems = DataCenter._product.TestCondition.TestItemArray;

            if (testItems == null || testItems.Length == 0)
            {
                return;
            }

            this.dgvPIVCoef.SuspendLayout();

            int rowCount = 0;

            foreach (TestItemData item in testItems)
            {
                if (item is PIVTestItem)
                {
                    this.dgvPIVCoef.Rows.Add();
                    this.dgvPIVCoef.Rows[rowCount].Cells[0].Value = (rowCount + 1).ToString();  // No.
                    this.dgvPIVCoef.Rows[rowCount].Cells[1].Value = item.Name;    // Name
                    this.dgvPIVCoef.Rows[rowCount].Cells[2].Value = "Power";    // Type                 
                    this.dgvPIVCoef.Rows[rowCount].Cells[3].Value = (item as PIVTestItem).CalcSetting.GainPower;    // Gain
                    this.dgvPIVCoef.Rows[rowCount].Cells[4].Value = (item as PIVTestItem).CalcSetting.OffsetPower;  // Offset
                    this.dgvPIVCoef.Rows[rowCount].Cells[5].Value = item.KeyName; // KeyName

                    rowCount++;

                    this.dgvPIVCoef.Rows.Add();
                    this.dgvPIVCoef.Rows[rowCount].Cells[0].Value = (rowCount + 1).ToString();  // No.
                    this.dgvPIVCoef.Rows[rowCount].Cells[1].Value = item.Name;    // Name
                    this.dgvPIVCoef.Rows[rowCount].Cells[2].Value = "Current";    // Type                 
                    this.dgvPIVCoef.Rows[rowCount].Cells[3].Value = (item as PIVTestItem).CalcSetting.GainCurrent;    // Gain
                    this.dgvPIVCoef.Rows[rowCount].Cells[4].Value = (item as PIVTestItem).CalcSetting.OffsetCurrent;  // Offset
                    this.dgvPIVCoef.Rows[rowCount].Cells[5].Value = item.KeyName; // KeyName

                    rowCount++;

                    this.dgvPIVCoef.Rows.Add();
                    this.dgvPIVCoef.Rows[rowCount].Cells[0].Value = (rowCount + 1).ToString();  // No.
                    this.dgvPIVCoef.Rows[rowCount].Cells[1].Value = item.Name;    // Name
                    this.dgvPIVCoef.Rows[rowCount].Cells[2].Value = "Voltage";    // Type                 
                    this.dgvPIVCoef.Rows[rowCount].Cells[3].Value = (item as PIVTestItem).CalcSetting.GainVoltage;    // Gain
                    this.dgvPIVCoef.Rows[rowCount].Cells[4].Value = (item as PIVTestItem).CalcSetting.OffsetVoltage;  // Offset
                    this.dgvPIVCoef.Rows[rowCount].Cells[5].Value = item.KeyName; // KeyName

                    rowCount++;
                }
            }

            this.dgvPIVCoef.ResumeLayout();
        }

        private void SavePIVGainOffset()
        {
            if (DataCenter._product.TestCondition == null || DataCenter._product.TestCondition.TestItemArray == null)
                return;

            TestItemData[] testItems = DataCenter._product.TestCondition.TestItemArray;

            for (int row = 0; row < this.dgvPIVCoef.Rows.Count; row += 3)
            {
                foreach (TestItemData item in testItems)
                {
                    if (item is PIVTestItem)
                    {
                        if (this.dgvPIVCoef[5, row].Value.ToString() == item.KeyName)
                        {
                            // P  
                            (item as PIVTestItem).CalcSetting.GainPower = Convert.ToDouble(this.dgvPIVCoef[3, row].Value.ToString());
                            (item as PIVTestItem).CalcSetting.OffsetPower = Convert.ToDouble(this.dgvPIVCoef[4, row].Value.ToString());
                            // I
                            (item as PIVTestItem).CalcSetting.GainCurrent = Convert.ToDouble(this.dgvPIVCoef[3, row + 1].Value.ToString());
                            (item as PIVTestItem).CalcSetting.OffsetCurrent = Convert.ToDouble(this.dgvPIVCoef[4, row + 1].Value.ToString());
                            // V
                            (item as PIVTestItem).CalcSetting.GainVoltage = Convert.ToDouble(this.dgvPIVCoef[3, row + 2].Value.ToString());
                            (item as PIVTestItem).CalcSetting.OffsetVoltage = Convert.ToDouble(this.dgvPIVCoef[4, row + 2].Value.ToString());
                        }
                    }
                }
            }
        }

		#endregion

		#region >>> Public Methods <<<

		public void Fire_TestItemDataChangeEvent()
		{
			if (this.TestItemDataChangeEvent != null)
			{
				this.TestItemDataChangeEvent(new object(), new EventArgs());
			}
		}

		//------------------------------------------------------------------------
		// Parameters of condition will have add, insert, remove, and update action. 
		// These actions will fire event, then call this function
		// to update data to this form.
		//-------------------------------------------------------------------------
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

		#endregion

		#region >>> UI Event Handler <<<

		private void frmConditionCoef_Load(object sender, EventArgs e)
		{
			this.UpdateDataToControls();
		}

		private void frmConditionCoef_VisibleChanged(object sender, EventArgs e)
		{
			if (this.Visible == true)
			{
				this.UpdateDataToControls();
			}
		}

		private void cmbCalMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.cmbCalMode.SelectedItem != null)
			{
				DataCenter._product.TestCondition.CalMode = (ECalMode)Enum.Parse(typeof(ECalMode), this.cmbCalMode.SelectedItem.ToString(), true);
			}

			DataCenter.SaveProductFile();
		}

		private void cmbCalByWave_SelectedIndexChanged(object sender, EventArgs e)
		{
            //if (this.cmbCalByWave.SelectedItem != null)
            //{
            //    DataCenter._product.TestCondition.CalByWave = (ECalBaseWave)Enum.Parse(typeof(ECalBaseWave), this.cmbCalByWave.SelectedItem.ToString(), true);
            //}

			//DataCenter.SaveProductFile();

		}

		private void btnConfirmCoef_Click(object sender, EventArgs e)
		{
            DialogResult result = TopMessageBox.ShowMessage(000, "Would you Save Gain / Offset & Coeff Table?", "Save Gain / Offset Table & Coeff Table?");
            // Host.onShowMessage.Invoke(001, "Would you Reset Gain / Offset Table?", "Reset Coeff");
            //DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show("Would you Reset Gain / Offset Table?", "Reset Chuck", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (result != DialogResult.OK)
                return;

            UILog.Log(this, sender, "btnConfirmCoef_Click");

            if (this.checkFactorIsInBoundary()==false)
            {
				TopMessageBox.PopWarning("Gain2/Offset2 修改數值超出規範");
                return;
            }

			this.SaveDataToFile();

            if (this.cmbCalByWave.SelectedItem != null)
            {
                DataCenter._product.TestCondition.CalByWave = (ECalBaseWave)Enum.Parse(typeof(ECalBaseWave), this.cmbCalByWave.SelectedItem.ToString(), true);
            }

  			   if (DataCenter._uiSetting.ImportCalibrateFileName != "")
                {
                    DataCenter.ExportCalibrateData(DataCenter._uiSetting.ProductPath02, DataCenter._uiSetting.ImportCalibrateFileName + ".cal");
                }

                if (DataCenter._uiSetting.IsEnableSaveBackupCoef)
                {
                    DataCenter.ExportCalibrateData(DataCenter._uiSetting.BackupCoefPath, DataCenter._uiSetting.TaskSheetFileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".cal");
                }

             DataCenter._conditionCtrl.CopyTestItemArrayToEachChannel();

          //  DataCenter.SaveProductFile();

            DataCenter.SaveProductFile();

            if (!Host._MPIStorage.SaveTestCoefficientToFile())
            {
                Host.SetErrorCode(EErrorCode.SaveWatchCoefficientFileFail);
            }
			// Fire event
			this.Fire_TestItemDataChangeEvent();
		}

		private void btnResetGainOffset_Click(object sender, EventArgs e)
		{
            DialogResult result=TopMessageBox.ShowMessage(001, "Would you Reset Gain / Offset Table?", "Reset Coeff");
           // Host.onShowMessage.Invoke(001, "Would you Reset Gain / Offset Table?", "Reset Coeff");
            //DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show("Would you Reset Gain / Offset Table?", "Reset Chuck", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (result != DialogResult.OK)
                return;

			int row = 0;
			for ( row = 0; row < this.dgvGainOffset.Rows.Count; row++)
			{
				this.dgvGainOffset[3, row].Value = 0.0d;		// Square
				this.dgvGainOffset[4, row].Value = 1.0d;		// Gain
				this.dgvGainOffset[5, row].Value = 0.0d;		// Offset
			}
            
            for (row = 0; row < this.dgvGainOffset2.Rows.Count; row++)
            {
                this.dgvGainOffset2[3, row].Value = 0.0d;		// Square
                this.dgvGainOffset2[4, row].Value = 1.0d;		// Gain
                this.dgvGainOffset2[5, row].Value = 0.0d;		// Offset
            }

			for (row = 0; row < this.dgvGainOffset3.Rows.Count; row++)
			{
				this.dgvGainOffset3[3, row].Value = 0.0d;		// Square
				this.dgvGainOffset3[4, row].Value = 1.0d;		// Gain
				this.dgvGainOffset3[5, row].Value = 0.0d;		// Offset
			}

			this.dgvGainOffset.Update();
            this.dgvGainOffset2.Update();
			this.dgvGainOffset3.Update();
            this.SaveGainAndTableData();
		}		

		private void btnResetCoef_Click(object sender, EventArgs e)
		{
            DialogResult result=TopMessageBox.ShowMessage((int)EMessageCode.CheckResetTableCoeff, "Would you Reset Coef. Table?", "Reset");
          
			//DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show("Would you Reset Coef. Table?", "Reset", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (result != DialogResult.OK)
                return;

			this.ResetOptiCoefTable();
            DataCenter._conditionCtrl.ReloadCoefTableList();
		}

		private void btnSetCoefRange_Click_1(object sender, EventArgs e)
		{
			if (this.dinStartWL.Value  > this.dinEndWL.Value)
			{
				this.dinStartWL.Value = DataCenter._product.DispCoefStartWL;
			}
			else if (this.dinStartWL.Value < DataCenter._sysSetting.CoefTableStartWL)
			{
				this.dinStartWL.Value = DataCenter._sysSetting.CoefTableStartWL;
			}

			if (this.dinEndWL.Value  < this.dinStartWL.Value)
			{
				this.dinEndWL.Value = DataCenter._product.DispCoefEndWL;
			}
			else if (this.dinEndWL.Value > DataCenter._sysSetting.CoefTableEndWL)
			{
				this.dinEndWL.Value = DataCenter._sysSetting.CoefTableEndWL;
			}

			DataCenter._product.DispCoefStartWL = this.dinStartWL.Value;
			DataCenter._product.DispCoefEndWL = this.dinEndWL.Value;
			this.UpdateDataToCoefTable();
			DataCenter.SaveProductFile();
		}

		private void DGVInputCell_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
			DevComponents.DotNetBar.Controls.DataGridViewX dgv = (DevComponents.DotNetBar.Controls.DataGridViewX)sender;

			if (dgv.Rows[e.RowIndex].IsNewRow)
				return;

			dgv.Rows[e.RowIndex].ErrorText = "";

			if ( dgv.Name != "dgvOpticCoefTable")
			{
				if (e.ColumnIndex < (dgv.ColumnCount - 3))	// last 3 items is double type
					return;
			}

			double inputValue = 0.0d;

			if (double.TryParse(e.FormattedValue.ToString(), out inputValue) == false)
			{
				// Gilbert								
				DialogResult Result = MessageBox.Show(" Validate Data Formate Error", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
				e.Cancel = true;
				dgv.Rows[e.RowIndex].ErrorText = " Data Format Error";
			}

		}

		private void DGVCell_CellEnter(object sender, DataGridViewCellEventArgs e)
		{
			DevComponents.DotNetBar.Controls.DataGridViewX dgv = (DevComponents.DotNetBar.Controls.DataGridViewX)sender;

			if (false == dgv.Columns[e.ColumnIndex].ReadOnly)
				dgv[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Red;
		}

		private void DGVCell_CellLeave(object sender, DataGridViewCellEventArgs e)
		{
			DevComponents.DotNetBar.Controls.DataGridViewX dgv = (DevComponents.DotNetBar.Controls.DataGridViewX)sender;

			dgv[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Empty;
		}

		//private void dgvOpticCoefTable_CellEnter(object sender, DataGridViewCellEventArgs e)
		//{
		//    if (false == this.dgvOpticCoefTable.Columns[e.ColumnIndex].ReadOnly)
		//        this.dgvOpticCoefTable[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Red;
		//}

		//private void dgvOpticCoefTable_CellLeave(object sender, DataGridViewCellEventArgs e)
		//{
		//    this.dgvOpticCoefTable[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Empty;
		//}

		//private void dgvOpticCoefTable_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		//{
		//    DialogResult Result;
		//    if (this.dgvOpticCoefTable.Rows[e.RowIndex].IsNewRow)
		//        return;

		//    this.dgvOpticCoefTable.Rows[e.RowIndex].ErrorText = "";
		//    double newValue;

		//    if (!double.TryParse(e.FormattedValue.ToString(), out newValue))
		//    {
		//        Result = DevComponents.DotNetBar.MessageBoxEx.Show("Data Formate Error", "Delete Data ", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
		//        e.Cancel = true;
		//        this.dgvOpticCoefTable.Rows[e.RowIndex].ErrorText = " Data Format Error";
		//    }
		//}

        private void btnCaliTool_Click(object sender, EventArgs e)
        {
            if (this._frmCaliCoeff == null)
            {
                this._frmCaliCoeff = new frmCaliCoeff();
                this._frmCaliCoeff.ShowDialog();
            }
            else
            {
                this._frmCaliCoeff.ShowDialog();
            }
        }

		private void numTableNum_ValueChanged(object sender, EventArgs e)
		{
			//this.UpdateDataToCoefTable();
		}

		private void btnImportCoef_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Title = "Import CoefTable Data from File";
			openFileDialog.Filter = "CSV files (*.csv)|*.cal";
			openFileDialog.FilterIndex = 1;    // default value = 1

            openFileDialog.InitialDirectory = DataCenter._uiSetting.ProductPath02;
			openFileDialog.FileName = "";
			openFileDialog.Multiselect = false;

			if (openFileDialog.ShowDialog() != DialogResult.OK)
				return;

			DataCenter.ImportCalibrateData(Path.GetDirectoryName(openFileDialog.FileName), Path.GetFileName(openFileDialog.FileName));

			this.dinStartWL.Value = DataCenter._product.DispCoefStartWL;
			this.dinEndWL.Value = DataCenter._product.DispCoefEndWL;

			this.UpdateDataToDGV();
            this.SaveDataToFile();

		}

        private void btnExportCoef_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Export CoefTable Data to File";
            saveFileDialog.Filter = "CSV files (*.cal)|*.cal";
            saveFileDialog.FilterIndex = 1;    // default value = 1
            saveFileDialog.InitialDirectory = DataCenter._uiSetting.ProductPath02;

            saveFileDialog.FileName = DataCenter._uiSetting.TaskSheetFileName;


            if (DataCenter._uiSetting.ImportCalibrateFileName != "")
            {
                saveFileDialog.FileName = DataCenter._uiSetting.ImportCalibrateFileName;
            }

            if (DataCenter._uiSetting.UserID == EUserID.HCSemiTek)
            {
                string calibrationFileName=string.Empty;

                if(DataCenter._uiSetting.TaskSheetFileName.Contains("-"))
                {
                    int index = DataCenter._uiSetting.TaskSheetFileName.IndexOf("-");

                    calibrationFileName = DataCenter._uiSetting.TaskSheetFileName.Remove(index);

                    saveFileDialog.FileName = calibrationFileName;
                }
            }

            //if (DataCenter._uiSetting.ImportCalibrateFileName != "")
            //{
            //    saveFileDialog.FileName = DataCenter._uiSetting.ImportCalibrateFileName;
            //}

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            DataCenter.ExportCalibrateData(Path.GetDirectoryName(saveFileDialog.FileName), Path.GetFileName(saveFileDialog.FileName));
		}

        private void btnDailyWatch_Click(object sender, EventArgs e)
        {
            if (this._frmChuckCorrection == null)
            {
                this._frmChuckCorrection = new frmChuckCorrection();
                this._frmChuckCorrection.ShowDialog();
            }
            else
            {
                this._frmChuckCorrection.ShowDialog();
            }
        }

        private void btnRestChuckGainOffset_Click(object sender, EventArgs e)
        {
            DialogResult result = TopMessageBox.ShowMessage((int) EMessageCode.CheckResetChuckGainOffset,"Would you Reset Chuck Gain / Offset Table?", "Reset", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
          //  DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show("Would you Reset Chuck Gain / Offset Table?", "Reset", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (result != DialogResult.OK)
                return;

            foreach (GainOffsetData[] dataArray in DataCenter._product.ChuckLOPCorrectArray)
            {
                foreach (GainOffsetData data in dataArray)
                {
                    data.Gain = 1.0d;
                    data.Offset = 0.0d;
                }
            }
            this.UpdateDataToChuckDGV();
            this.SaveChuckGainAndOffset();
        }

        private void btnShowSptCalibData_Click(object sender, EventArgs e)
        {
            if (this._frmSptCalibData == null)
            {
                this._frmSptCalibData = new frmDailyWatch();
                this._frmSptCalibData.ShowDialog();
            }
            else
            {
                this._frmSptCalibData.ShowDialog();
            }
		}

        private void btnResetRcCoefTable_Click(object sender, EventArgs e)
        {
            if (this.chkEnableByChannelCompensate.Checked == false)
            {
                return;
            }

            if (DataCenter._product.TestCondition.ChannelConditionTable == null)
            {
                return;
            }

            DialogResult result = TopMessageBox.ShowMessage(001, "Would you Reset Gain / Offset Table?", "Reset Coeff");
            // Host.onShowMessage.Invoke(001, "Would you Reset Gain / Offset Table?", "Reset Coeff");
            //DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show("Would you Reset Gain / Offset Table?", "Reset Chuck", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (result != DialogResult.OK)
                return;

            int row = 0;

            for (row = 0; row < this.dgvByChannelCoefTable.Rows.Count; row++)
            {
                this.dgvByChannelCoefTable[4, row].Value = 0.0d;		// Square
                this.dgvByChannelCoefTable[5, row].Value = 1.0d;		// Gain
                this.dgvByChannelCoefTable[6, row].Value = 0.0d;		// Offset
            }

            this.dgvByChannelCoefTable.Update();

            this.SaveByChannelCoefTable();
        }

        private void chkEnableByChannelCompensate_CheckedChanged(object sender, EventArgs e)
        {
            bool isEnable = this.chkEnableByChannelCompensate.Checked;

            this.btnResetRcCoefTable.Enabled = isEnable;
            this.btnNormalizedByChannel.Enabled = isEnable;
            this.dinNormalizedByChannel.Enabled = isEnable;

            this.UpdateDataToByChannelCoefDGV();
        }

        private void btnChannelCaliTool_Click(object sender, EventArgs e)
        {
            if (this._frmChannelCali!=null)
            {
                _frmChannelCali.Close();

                _frmChannelCali.Dispose();
            }

            if (DataCenter._uiSetting.UserID == EUserID.TSB)
            {
                this._frmChannelCali = new frmChannelCali2();
            }
            else
            {
                this._frmChannelCali = new frmChannelCali();
            }

            this._frmChannelCali.Show();
        }

        private void btnNormalizedByChannel_Click(object sender, EventArgs e)
        {
            DialogResult result = TopMessageBox.ShowMessage((int)EMessageCode.CheckNormalizedByChannel, "Normalized by Channel?", "Normalized");

            if (result != DialogResult.OK)
                return;

            if (DataCenter._product.TestCondition == null || DataCenter._product.TestCondition.TestItemArray == null)
            {
                return;
            }

            string stdKeyName = string.Empty;

            uint stdChannel = (uint)this.dinNormalizedByChannel.Value - 1;

            double normalizedGain = 1.0d;

            int row = 0;

            TestItemData[] testItems = DataCenter._product.TestCondition.TestItemArray;

            ChannelConditionTable conditionTable = DataCenter._product.TestCondition.ChannelConditionTable;

            foreach (TestItemData item in testItems)
            {
                if (item is LOPWLTestItem)
                {
                    if (item.GainOffsetSetting == null || item.GainOffsetSetting.Length == 0)
                        continue;
                    
                    foreach (GainOffsetData data in item.GainOffsetSetting)
                    {
                        if (!data.IsEnable || !data.IsVision)
                            continue;

                        normalizedGain = 1.0d;

                        stdKeyName = data.KeyName;

                        // 找 Normalized Channel Gain
                        for (row = 0; row < this.dgvByChannelCoefTable.Rows.Count; row++)
                        {
                            if (stdKeyName == this.dgvByChannelCoefTable.Rows[row].Cells[7].Value.ToString() &&
                                stdChannel == Convert.ToUInt32(this.dgvByChannelCoefTable.Rows[row].Cells[2].Value.ToString()) - 1)
                            {
                                normalizedGain = Convert.ToDouble(this.dgvByChannelCoefTable.Rows[row].Cells[5].Value);

                                this.dgvByChannelCoefTable.Rows[row].Cells[5].Value = 1.0d;

                                break;
                            }
                        }

                        // 對剩餘的Channel 做標準化
                        for (row = 0; row < this.dgvByChannelCoefTable.Rows.Count; row++)
                        {
                            if (stdKeyName == this.dgvByChannelCoefTable.Rows[row].Cells[7].Value.ToString() &&
                                stdChannel != Convert.ToUInt32(this.dgvByChannelCoefTable.Rows[row].Cells[2].Value.ToString()) - 1)
                            {
                                double gain = Convert.ToDouble(this.dgvByChannelCoefTable.Rows[row].Cells[5].Value);

                                this.dgvByChannelCoefTable.Rows[row].Cells[5].Value = gain / normalizedGain;
                            }
                        }

                        // 將 Normalized Channel Gain 合併入 大係數表
                        for (row = 0; row < this.dgvGainOffset.Rows.Count; row++)
                        {
                            if (stdKeyName == this.dgvGainOffset.Rows[row].Cells[6].Value.ToString())
                            {
                                double gain = Convert.ToDouble(this.dgvGainOffset.Rows[row].Cells[4].Value);

                                this.dgvGainOffset.Rows[row].Cells[4].Value = gain * normalizedGain;

                                break;
                            }
                        }
                    }
                }
            }
        }

        private void chkEnableSystemCoefCompensate_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateDataToSystemCoefDGV();
        }

        private void btnResetPIVCoef_Click(object sender, EventArgs e)
        {
            DialogResult result = TopMessageBox.ShowMessage(001, "Would you Reset PIV Coef. Table?", "Reset Coef.");
            // Host.onShowMessage.Invoke(001, "Would you Reset Gain / Offset Table?", "Reset Coeff");
            //DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show("Would you Reset Gain / Offset Table?", "Reset Chuck", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (result != DialogResult.OK)
                return;

            int row = 0;

            for (row = 0; row < this.dgvPIVCoef.Rows.Count; row++)
            {
                this.dgvPIVCoef[3, row].Value = 1.0d;		// Gain
                this.dgvPIVCoef[4, row].Value = 0.0d;		// Offset
            }

            this.dgvPIVCoef.Update();

            this.SavePIVGainOffset();
        }

        #endregion

        private void btnUpdateCalibrationFile_Click(object sender, EventArgs e)
        {
            DataCenter._uiSetting.ControlTaskSettingUI = EControlTaskSetting.UPDATE;

            frmOpRecipe._frmSetTaskSheet.ShowDialog();

            if (DataCenter._uiSetting.SendTaskFileName == "")
                return;

            WMOperate.WM_ReadCalibrateParamFromSetting();
            // Import Calibration // BIN
            DataCenter.CreateTaskSheet(DataCenter._uiSetting.SendTaskFileName);

            if (DataCenter._uiSetting.ImportCalibrateFileName != "")
            {
                lblCoefTablePathAndFile.Text = DataCenter._uiSetting.ImportCalibrateFileName;

                DataCenter.ImportCalibrateData(DataCenter._uiSetting.ProductPath02, DataCenter._uiSetting.ImportCalibrateFileName + ".cal");
            }
            //if (DataCenter._uiSetting.ImportBinFileName != "")
            //{
            //    DataCenter.ImportBinTable(DataCenter._uiSetting.ProductPath02, DataCenter._uiSetting.ImportBinFileName + ".sr2");
            //}
            DataCenter.Save();

            Host.UpdateDataToAllUIForm();
        }

        private void cmbLopItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateDataToCoefTable();
        }

        private bool checkFactorIsInBoundary()
        {
            if (this._dicFatorBoundary == null)
            {
                return true;
            }

            int row = 0;

            for (row = 0; row < this.dgvGainOffset2.Rows.Count; row++)
            {
                foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
                {
                    if (item.GainOffsetSetting != null)
                    {
                        GainOffsetData[] data = item.GainOffsetSetting;

                        for (int i = 0; i < data.Length; i++)
                        {
                            if (this._dicFatorBoundary.ContainsKey(data[i].KeyName))
                            {
                                FactorBoundary fb = this._dicFatorBoundary[data[i].KeyName];

                                double Gain2 = Convert.ToDouble(this.dgvGainOffset2[4, row].Value.ToString());

                                double Offset2 = Convert.ToDouble(this.dgvGainOffset2[5, row].Value.ToString());

                                if (fb.IsEnable)
                                {
                                    if (Gain2 > fb.High || Gain2 < fb.Low)
                                    {
                                        return false;
                                    }
                                }

                            }
                        }

                    }
                }
            }

            return true;
        }

        private void dgvCoeff_KeyDown(object sender, KeyEventArgs e)
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
        }
    }
}