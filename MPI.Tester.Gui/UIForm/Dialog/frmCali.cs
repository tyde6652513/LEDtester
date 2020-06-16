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
using MPI.Tester.Tools;
using ZedGraph;
using MPI.Windows.Forms;
using MPI.Tester.Maths;
using MPI.Tester.Data.CalibrateData;

namespace MPI.Tester.Gui
{
    public partial class frmCaliCoeff : Form
    {
        private Form _frmMultiFile;
		private OpenFileDialog _openFileDialog; 
        private CalcGainOffset[] _calcGainOffsetArray;
        private CalcCoef[] _calcCoefArray;

        private Color[] randomColor;
		private int _colorIndex = 0;
        private double[][] _coeffTable;
		//private string _compFilePath = "";
        private string _stdFileName = "";
        private string _msrtFileName = "";

        private List<double>[] _selectedCalcData=new List<double>[2];

        private List<string> _currentTestArray = new List<string>();

        private Dictionary<string, GainOffset> _linearCalcResultData = new Dictionary<string, GainOffset>();

		private Dictionary<string, string> _filter = new Dictionary<string, string>();

		private SimpleLinearRegression _lstRegress;

        private int _lastSelectTableIndex = -1;

		private CalibrateChipValue _caliChipValue = new CalibrateChipValue();

        private frmChannelCali _frmCahannelCalib = new frmChannelCali();

        private ByChannelCalibrateCtrl _rcCtrl = new ByChannelCalibrateCtrl();

        public frmCaliCoeff()
        {
            InitializeComponent();
			InitParamAndCompData();
			this._openFileDialog = new OpenFileDialog();
			this._lstRegress = new SimpleLinearRegression();
        }

		#region >>> Private Method <<<

		private void InitParamAndCompData()
        {
            this.cmbDisplayWave.Items.Clear();
            this.cmbDisplayWave.Items.AddRange(new string[4] { "WLP", "WLD", "WLC","HW"});    
            this.cmbDisplayLOP.Items.Clear();
            this.cmbDisplayLOP.Items.AddRange(new string[3] { "LOP", "WATT", "LM" });
			this.cmbCalcCoefMode.Items.Clear();
#if ( !DebugVer )
			this.cmbCalcCoefMode.Items.AddRange(new string[2] { "Average", "Median"} );
            this.tabiFormat.Visible = false;
#else
			this.cmbCalcCoefMode.Items.AddRange(new string[3] { "Average", "Median", "Centroid" });
           this.tabiFormat.Visible = true;
#endif
            if (DataCenter._userManag.CurrentUserName == "simulator")
            {
                this.tabiFormat.Visible = true;
                this.btnGetStdAndCurrentFormat.Visible = true;
                this.lstFileTitle.Visible = true;
                this.plAdv.Visible = true;
            }
            this.cmbDrawPointType.Items.AddRange(Enum.GetNames(typeof(SymbolType)));
            this.randomColor = new Color[] { Color.Blue, Color.Red, Color.Green, Color.Purple, Color.Orange, Color.YellowGreen, Color.DeepPink, Color.SteelBlue, Color.DarkGreen };
		}

        private void InitChartLabel()
        {
            if (this.Visible == false)
                return;

            string LOPItem = this.cmbDisplayLOP.SelectedItem.ToString();
            string waveError = this.cmbDisplayWave.SelectedItem.ToString();
            string waveBase = DataCenter._product.TestCondition.CalByWave.ToString();
            //Initial Grapha
            PlotGraph.SetGrid(this.zedMcdGainValue, false, Color.Silver, Color.Transparent);
            PlotGraph.SetGrid(this.zedWaveLength, false, Color.Silver, Color.Transparent);
            PlotGraph.SetLabel(zedMcdGainValue, LOPItem, waveBase + " (nm)", LOPItem + "  Gain", 16);
            PlotGraph.SetLabel(zedWaveLength, waveError + " (nm)", waveBase + " (nm)", waveError + "  Error (nm)", 16);
            PlotGraph.SetGrid(this.zedLinearRegress, false, Color.Silver, Color.Transparent);
            PlotGraph.SetGrid(this.zedDisplayVolt1, false, Color.Silver, Color.Transparent);
            PlotGraph.SetGrid(this.zedDisplayVolt2, false, Color.Silver, Color.Transparent);
            PlotGraph.SetGrid(this.zedDisplayVolt3, false, Color.Silver, Color.Transparent);
            PlotGraph.SetGrid(this.zedDisplayVolt4, false, Color.Silver, Color.Transparent);

            PlotGraph.SetGrid(this.zgcCIEx, false, Color.Silver, Color.Transparent);
            PlotGraph.SetGrid(this.zgcCIEy, false, Color.Silver, Color.Transparent);
            PlotGraph.SetGrid(this.zgcCCT, false, Color.Silver, Color.Transparent);
            PlotGraph.SetGrid(this.zgcCRI, false, Color.Silver, Color.Transparent);

            PlotGraph.SetGrid(this.zedDisplayPow1, false, Color.Silver, Color.Transparent);
            PlotGraph.SetGrid(this.zedDisplayPow2, false, Color.Silver, Color.Transparent);

            PlotGraph.SetLabel(zgcCCT, "CCT ", "CCT (k)", "Delta CCT", 14);

            PlotGraph.SetLabel(zgcCIEx, " CIE1931 x ", "CCT (k)", "CIE x,y", 14);

            PlotGraph.SetLabel(zgcCIEy, " CIE1931 y ", "CCT (k)", "CIE x,y", 14);

            PlotGraph.SetLabel(zgcCRI, "CRI", "CCT (k)", "Delta CRI", 14);

        }

        private void InitialAllDGV()
        {
			//-----------------------------------------------------------------
			// Init the dgvDispCondition
			//-----------------------------------------------------------------
            dgvDispCondition.Rows.Clear();
            dgvDispCondition.ColumnCount = 4;
            dgvDispCondition.Columns[0].Width = 120;
            dgvDispCondition.Columns[1].Width = 80;
            dgvDispCondition.Columns[2].Width = 120;
            dgvDispCondition.Columns[3].Width = 80;

            dgvDispCondition.Rows.Add();
            dgvDispCondition.Rows[0].Cells[0].Value = "Std Counts";
            dgvDispCondition.Rows[0].Cells[1].Value = "xxx";
            dgvDispCondition.Rows[0].Cells[2].Value = "Msrt Counts";
            dgvDispCondition.Rows[0].Cells[3].Value = "xxx";
            dgvDispCondition.Rows.Add();
            dgvDispCondition.Rows[1].Cells[0].Value = "Filtered Counts";
            dgvDispCondition.Rows[1].Cells[1].Value = "xxx";
            dgvDispCondition.Rows[1].Cells[2].Value = "Filtered Percent";
            dgvDispCondition.Rows[1].Cells[3].Value = "xxx";
            dgvDispCondition.Rows.Add();
            dgvDispCondition.Rows[2].Cells[0].Value = "WLD in Spec ( ±" + DataCenter._toolConfig.WavelenghtSpec.ToString() + " nm)";
            dgvDispCondition.Rows[2].Cells[1].Value = "xxx";
            dgvDispCondition.Rows[2].Cells[2].Value = "LOP in Spec ( ±" + (DataCenter._toolConfig.LightPowerSpec * 100).ToString() + " %)";
            dgvDispCondition.Rows[2].Cells[3].Value = "xxx";

            dgvDispCondition.Rows[0].Cells[0].Style.BackColor = Color.WhiteSmoke;
            dgvDispCondition.Rows[1].Cells[0].Style.BackColor = Color.WhiteSmoke;
            dgvDispCondition.Rows[2].Cells[0].Style.BackColor = Color.WhiteSmoke;
            dgvDispCondition.Rows[0].Cells[2].Style.BackColor = Color.WhiteSmoke;
            dgvDispCondition.Rows[1].Cells[2].Style.BackColor = Color.WhiteSmoke;
            dgvDispCondition.Rows[2].Cells[2].Style.BackColor = Color.WhiteSmoke;

			dgvDispCondition.Update();

			//-----------------------------------------------------------------
			// Init the dgvGainOffset, dgvGainOffset2, and dgvGainOffset3
			//-----------------------------------------------------------------         

            for (int i = 0; i < this.dgvGainOffset.Columns.Count; i++)
            {
                this.dgvGainOffset.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dgvGainOffset2.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            this.dgvGainOffset.Columns[0].DefaultCellStyle.BackColor = Color.YellowGreen;

			this.dgvGainOffset.Rows.Clear();
			this.dgvGainOffset.Update();
			this.dgvGainOffset2.Rows.Clear();
			this.dgvGainOffset2.Update();
			//-----------------------------------------------------------------
			// Init the dgvCalcGainOffset
			//-----------------------------------------------------------------
			this.dgvCalcGainOffset.DataSource = null;
            this.dgvCalcGainOffset.AutoGenerateColumns = false;
			for (int i = 0; i < this.dgvCalcGainOffset.Columns.Count; i++)
			{
				this.dgvCalcGainOffset.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
				this.dgvCalcGainOffset.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
			}
			this.dgvCalcGainOffset.Columns[0].DefaultCellStyle.BackColor = Color.YellowGreen;

            for (int i = 0; i < this.dgvCalcGainOffset.ColumnCount; i++)
            {
                this.dgvCalcGainOffset.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                // this.dgvGainOffset.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                // this.dgvCalcGainOffset.Columns[i].ReadOnly = true;
            }

			this.dgvCalcGainOffset.Rows.Clear();
			this.dgvCalcGainOffset.Update();
			//-----------------------------------------------------------------
			// Init the dgvStdFilter and dgvCompareFilter
			//-----------------------------------------------------------------
            this.dgvStdFilter.DataSource = null;
            this.dgvStdFilter.AutoGenerateColumns = false;

            this.dgvStdFilter.AllowUserToAddRows = false;
            this.dgvStdFilter.AllowUserToDeleteRows = false;
            this.dgvStdFilter.AllowUserToResizeRows = false;
            this.dgvStdFilter.AllowUserToResizeColumns = false;
            this.dgvStdFilter.AllowUserToOrderColumns = false;

			this.dgvCompareFilter.DataSource = null;
			this.dgvCompareFilter.AutoGenerateColumns = false;

			this.dgvCompareFilter.AllowUserToAddRows = false;
			this.dgvCompareFilter.AllowUserToDeleteRows = false;
			this.dgvCompareFilter.AllowUserToResizeRows = false;
			this.dgvCompareFilter.AllowUserToResizeColumns = false;
			this.dgvCompareFilter.AllowUserToOrderColumns = false;

			//-----------------------------------------------------------------
			// Init the dgvCoef
			//-----------------------------------------------------------------
			this.dgvCoef.DataSource = null;
            this.dgvCoef.AutoGenerateColumns = false;
            for (int i = 0; i < this.dgvCoef.ColumnCount; i++)
            {
                this.dgvCoef.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                //this.dgvCoef.Columns[i].Width = 60;
                this.dgvCoef.Columns[i].ValueType = System.Type.GetType("System.Double");
            }		

            this.dgvCoef.AllowUserToAddRows = false;
            this.dgvCoef.AllowUserToDeleteRows = false;
            this.dgvCoef.AllowUserToResizeRows = false;
            this.dgvCoef.AllowUserToResizeColumns = false;
            this.dgvCoef.AllowUserToOrderColumns = false;

			this.dgvCoef.Rows.Clear();
			this.dgvCoef.Update();
        }

		private void UpdateDataToControls()
		{
			this.cmbDisplayWave.SelectedIndex = DataCenter._toolConfig.DispWaveItem;
			this.cmbDisplayLOP.SelectedIndex = DataCenter._toolConfig.DispLOPItem;

			this.dgvDispCondition.Rows[2].Cells[0].Value = " WLD in Spec (±" + DataCenter._toolConfig.WavelenghtSpec.ToString() + "nm)";
			this.dgvDispCondition.Rows[2].Cells[2].Value = " LOP in Spec (±" + (DataCenter._toolConfig.LightPowerSpec * 100).ToString() + "%)";

			this.lblDataFormat.Text = "User ID_Data Format : " +
										Convert.ToInt32(DataCenter._uiSetting.UserID).ToString("0000") + "_" +
										DataCenter._uiSetting.FormatName;
			this.lblCalByWave.Text = "Cal. Wave By : " + DataCenter._fileCompare.CalBaseWave.ToString();
			this.lblSaveLOPItem.Text = "LOP Save Item : " + DataCenter._fileCompare.LOPSaveItem.ToString();

			this.cmbDisplayTable.Items.Clear();
			for (int index = 1; index <= DataCenter._fileCompare.CoefTableCount; index++)
			{
				this.cmbDisplayTable.Items.Add("Coef_" + index.ToString());
                this.cmbDisplayTable.SelectedIndex = 0;
                //if (this._lastSelectTableIndex != -1)
                //{
                //    this.cmbDisplayTable.SelectedIndex = this._lastSelectTableIndex;
                //}
                //else
                //{
                //    this.cmbDisplayTable.SelectedIndex = 0;
                //}

			}

            if (DataCenter._sysSetting.IsEnalbeCalcBigFactorBeforeSmall)
            {
                DataCenter._toolConfig.LookTableByStdOrMsrtMode = 0;
            }
            else
            {
                DataCenter._toolConfig.LookTableByStdOrMsrtMode = 1;
            }


			this.cmbLookTabByStdOrMsrt.SelectedIndex = DataCenter._toolConfig.LookTableByStdOrMsrtMode;

			//-----------------------------------------------------------
			// for Tools Setting Panel
			//-----------------------------------------------------------
			this.dinLightPowerSpec.Value = DataCenter._toolConfig.LightPowerSpec;
			this.dinWavelengthSpec.Value = DataCenter._toolConfig.WavelenghtSpec;
			this.cmbVoltDisplayXBase.SelectedIndex = DataCenter._toolConfig.VoltChartXBaseSelect;
			this.chkIsAutoClearChart.Checked = DataCenter._toolConfig.IsAutoClearChart;

			this.cmbIsPlotCoeffLine.SelectedIndex = Convert.ToInt32(DataCenter._toolConfig.IsPlotCoefCurve);
			this.cmbIsPlotDataPoint.SelectedIndex = Convert.ToInt32(DataCenter._toolConfig.IsPlotDataPoints);
			this.cmbIsPlotBrundyLine.SelectedIndex = Convert.ToInt32(DataCenter._toolConfig.IsPlotBoundary);

			this.cmbDrawPointType.SelectedIndex = DataCenter._toolConfig.PlotPointType;
			this.cmbIsFillDataPoint.SelectedIndex = Convert.ToInt32(DataCenter._toolConfig.IsFillDataPoint);
			this.chkIsSetPlotColor.Checked = DataCenter._toolConfig.IsSetDataPointsColor;
			this.colorPicker.SelectedColor = Color.FromArgb(DataCenter._toolConfig.DataPointColor);

			this.txtOpenStdFilePath.Text = DataCenter._toolConfig.StdFileDir;
			this.txtOpenMsrtFilePath.Text = DataCenter._toolConfig.MsrtFileDir;
			this.txtOpenCompareFilePath.Text = DataCenter._toolConfig.CompareFileDir;

			this.chkEnableWLP.Checked = DataCenter._toolConfig.IsCaculateCoeff[0];
			this.chkEnableWLD.Checked = DataCenter._toolConfig.IsCaculateCoeff[1];
			this.chkEnableWLC.Checked = DataCenter._toolConfig.IsCaculateCoeff[2];
			this.chkEnableMCD.Checked = DataCenter._toolConfig.IsCaculateCoeff[3];
			this.chkEnableMW.Checked = DataCenter._toolConfig.IsCaculateCoeff[4];
			this.chkEnableLM.Checked = DataCenter._toolConfig.IsCaculateCoeff[5];
			this.chkEnableHW.Checked = DataCenter._toolConfig.IsCaculateCoeff[6];

            this.chkIsUseRecipeCriterion.Checked = DataCenter._toolConfig.IsEnableUseRecipeCriterion;

            this.chkCompensateChannel.Checked = DataCenter._toolConfig.IsEnableCompensateChannelFactor;

			//-----------------------------------------------------------
			// for Filter Condition Panel
			//-----------------------------------------------------------
			this.cmbCalcCoefMode.SelectedIndex = DataCenter._toolConfig.CalcCoefMode;

			if ((DataCenter._toolConfig.FilterStdevCount % 2) == 0)
			{
				this.numFilterStdevCount.Value = DataCenter._toolConfig.FilterStdevCount;
			}
			else
			{
				DataCenter._toolConfig.FilterStdevCount = 6;
				this.numFilterStdevCount.Value = 6;
			}
			this.cmbExtendWLMode.SelectedIndex = DataCenter._toolConfig.ExtendWLMode;
			this.numExtendWLPoint.Value = DataCenter._toolConfig.ExtendWLPoint;
			this.dinExtWLStart.Value = DataCenter._toolConfig.ExtendWLStart;
			this.dinExtWLEnd.Value = DataCenter._toolConfig.ExtendWLEnd;

			this.numWaveExtDigit.Value = DataCenter._toolConfig.WaveExtDigit;
			this.numLOPExtDigit.Value = DataCenter._toolConfig.LOPExtDigit;

            this.cmbCoefficientExtendType.SelectedIndex = DataCenter._toolConfig.CoeffExtendType;
            //this.chkT100OperatedMode.Checked = DataCenter._toolConfig.IsEnableT100OperationMode;


            //if (DataCenter._toolConfig.IsEnableT100OperationMode)
            if (DataCenter._uiSetting.CalibrationUIMode == ECalibrationUIMode.T200)
			{
			    this.tabiVFCheck.Visible = true;

                this.tabiWaveLOP.Visible = true;

                this.tabiCompareData.Visible = false;

                this.plAdvCondSetting.Visible = true;

                this.btnCompareCalcCoefTable.Visible = true;
			}
            else if (DataCenter._uiSetting.CalibrationUIMode == ECalibrationUIMode.T100)
            {
                this.tabiVFCheck.Visible = false;

                this.tabiWaveLOP.Visible = false;

                this.tabiCompareData.Visible = true;

                this.plAdvCondSetting.Visible = false;

                this.btnCompareCalcCoefTable.Visible = false;
            }
            else if (DataCenter._uiSetting.CalibrationUIMode == ECalibrationUIMode.Both)
            {
				this.tabiVFCheck.Visible = true;

				this.tabiWaveLOP.Visible = true;

				this.tabiCompareData.Visible = true;

				this.plAdvCondSetting.Visible = true;

				this.btnCompareCalcCoefTable.Visible = true;
            }

            if (DataCenter._toolConfig.IsEnableCompensateChannelFactor)
                tabiCalcChannelGain.Visible = true;
            else
                tabiCalcChannelGain.Visible = false;

//#if ( !DebugVer )
//            this.btnGetStdAndCurrentFormat.Visible = false;
//            this.lstFileTitle.Visible = false;
//            this.plAdv.Visible = false;
//#else
//            this.btnGetStdAndCurrentFormat.Visible = true;
//            this.lstFileTitle.Visible = true;
//            this.plAdv.Visible = true;
//#endif
        }

        private void UpdateSystemDataToToolsDGV()
        {
            string gaintKeyName = string.Empty;
            this.dgvGainOffset.Rows.Clear();
            this.dgvGainOffset2.Rows.Clear();
            this.dgvSetting.Rows.Clear();
			this._filter.Clear();
         
            if (DataCenter._product.TestCondition == null || DataCenter._product.TestCondition.TestItemArray == null)
            {
                this.dgvGainOffset.Update();
                this.dgvGainOffset2.Update();
                this.dgvSetting.Update ();
                this.dgvFilter.Update();
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
            string[] gainRowData3= new string[this.dgvSetting.ColumnCount];
            string[] gainRowData4 = new string[this.dgvFilter.ColumnCount];
            this.dgvGainOffset.AllowUserToAddRows = true;
            this.dgvGainOffset2.AllowUserToAddRows = true;
            this.dgvSetting.AllowUserToAddRows = true;
            this.dgvFilter.AllowUserToAddRows = true;

            foreach (TestItemData item in testItems)
            {
                if (item.GainOffsetSetting != null)
                {
                    for (int i = 0; i < item.GainOffsetSetting.Length; i++)
                    {
                        if (item.GainOffsetSetting[i].IsEnable == false || item.GainOffsetSetting[i].IsVision == false)
                            continue;

                        gainRowData[0] = (gainIndex + 1).ToString();
                        gainRowData2[0] = (gainIndex + 1).ToString();
                        gainRowData3[0] = (gainIndex + 1).ToString();
                    

                        gainRowData[2] = item.GainOffsetSetting[i].Name;
                        gainRowData3[2] = item.GainOffsetSetting[i].Name;
                        gainRowData4[2] = item.GainOffsetSetting[i].Name;

                        if ((item is LOPWLTestItem) && item.GainOffsetSetting[i].KeyName.IndexOf("_") > 0)
                        {
							gaintKeyName = item.GainOffsetSetting[i].KeyName.Remove(item.GainOffsetSetting[i].KeyName.IndexOf("_"));
							switch (gaintKeyName)
                            {
                                case "LOP":        // EOptiMsrtType.LOP :
                                   gainRowData[2] = item.GainOffsetSetting[i].Name + "(mcd)";
                                   gainRowData2[2] = item.GainOffsetSetting[i].Name + "(mcd)";
                                   gainRowData3[2] = item.GainOffsetSetting[i].Name + "(mcd)";
                                   gainRowData4[2] = item.GainOffsetSetting[i].Name + "(mcd)";
                                    break;
								//------------------------------------------------
                                case "WATT":       // EOptiMsrtType.WATT
                                    gainRowData[2] = item.GainOffsetSetting[i].Name + "(mW)";
                                    gainRowData2[2] = item.GainOffsetSetting[i].Name + "(mW)";
                                    gainRowData3[2] = item.GainOffsetSetting[i].Name + "(mW)";
                                    gainRowData4[2] = item.GainOffsetSetting[i].Name + "(mW)";
                                    break;
								//------------------------------------------------
                                case "LM":         // EOptiMsrtType.LM
                                    gainRowData[2] = item.GainOffsetSetting[i].Name + "(lm)";
                                    gainRowData2[2] = item.GainOffsetSetting[i].Name + "(lm)";
                                    gainRowData3[2] = item.GainOffsetSetting[i].Name + "(lm)";
                                    gainRowData4[2] = item.GainOffsetSetting[i].Name + "(lm)";
                                    break;
								//------------------------------------------------
                                default:
                                    gainRowData[2] = item.GainOffsetSetting[i].Name;
                                    gainRowData2[2] = item.GainOffsetSetting[i].Name;
                                    gainRowData3[2] = item.GainOffsetSetting[i].Name;
                                    gainRowData4[2] = item.GainOffsetSetting[i].Name;
                                    break;
                            }
                        }
                        else
                        {
                            gainRowData[2] = item.GainOffsetSetting[i].Name;
                            gainRowData2[2] = item.GainOffsetSetting[i].Name;
                            gainRowData3[2] = item.GainOffsetSetting[i].Name;
                            gainRowData4[2] = item.GainOffsetSetting[i].Name;
                        }

						gainRowData[1] = item.GainOffsetSetting[i].KeyName;
                        gainRowData[3] = ((int)item.GainOffsetSetting[i].Type).ToString();
                        gainRowData[4] = item.GainOffsetSetting[i].Square.ToString();
                        gainRowData[5] = item.GainOffsetSetting[i].Gain.ToString();
                        gainRowData[6] = item.GainOffsetSetting[i].Offset.ToString();

						gainRowData2[1] = item.GainOffsetSetting[i].KeyName;
                        gainRowData2[3] = ((int)item.GainOffsetSetting[i].Type).ToString();
                        gainRowData2[4] = item.GainOffsetSetting[i].Square2.ToString();
                        gainRowData2[5] = item.GainOffsetSetting[i].Gain2.ToString();
                        gainRowData2[6] = item.GainOffsetSetting[i].Offset2.ToString();

                        gainRowData3[1] = item.GainOffsetSetting[i].KeyName;
                        gainRowData3[3] = ((int)item.GainOffsetSetting[i].Type).ToString();
                        gainRowData3[4] = item.MsrtResult[i].Formate;
                      //  gainRowData3[4] = item.MsrtResult[i].IsVerify.ToString();
                     //   gainRowData3[5] = item.GainOffsetSetting[i].Square2.ToString();            
                        //gainRowData3[5] = item.MsrtResult[i].MinLimitValue.ToString();
                        //gainRowData3[6] = item.MsrtResult[i].MaxLimitValue.ToString();

                        gainRowData4[1] = item.MsrtResult[i].KeyName;
                        gainRowData4[3] = item.MsrtResult[i].IsVerify.ToString();
                        gainRowData4[4] = item.MsrtResult[i].MinLimitValue.ToString();
                        gainRowData4[5] = item.MsrtResult[i].MaxLimitValue.ToString();

                        this.dgvGainOffset.Rows.Add(gainRowData);
                        this.dgvGainOffset2.Rows.Add(gainRowData2);
                        this.dgvSetting.Rows.Add ( gainRowData3 );
                        this.dgvFilter.Rows.Add(gainRowData4);
						this._filter.Add(gainRowData4[1], gainRowData4[2]);

                        if ((itemIndex % 2) == 0)
                        {
                            this.dgvGainOffset.Rows[gainIndex].DefaultCellStyle.BackColor = Color.White;
                            this.dgvGainOffset.Rows[gainIndex].Cells[0].Style.BackColor = Color.YellowGreen;

							this.dgvGainOffset2.Rows[gainIndex].DefaultCellStyle.BackColor = Color.White;
							this.dgvGainOffset2.Rows[gainIndex].Cells[0].Style.BackColor = Color.YellowGreen;
                        }
                        else
                        {
                            this.dgvGainOffset.Rows[gainIndex].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                            this.dgvGainOffset.Rows[gainIndex].Cells[0].Style.BackColor = Color.YellowGreen;

							this.dgvGainOffset2.Rows[gainIndex].DefaultCellStyle.BackColor = Color.WhiteSmoke;
							this.dgvGainOffset2.Rows[gainIndex].Cells[0].Style.BackColor = Color.YellowGreen;
                        }
                        gainIndex++;
                    }
                }
                itemIndex++;
            }

            this.dgvGainOffset.AllowUserToAddRows = false;
            this.dgvGainOffset.Update ();
            this.dgvGainOffset.Refresh ();
            this.dgvGainOffset2.AllowUserToAddRows = false;
            this.dgvGainOffset2.Update ();
            this.dgvGainOffset2.Refresh ();
            this.dgvSetting.AllowUserToAddRows = false;
            this.dgvSetting.Update ();
            this.dgvSetting.Refresh ();
            this.dgvFilter.AllowUserToAddRows = false;
            this.dgvFilter.Update();
            this.dgvFilter.Refresh();
            //
        }

        private void LoadSystemTestItemToTools()
        {
            this._currentTestArray.Clear();
            if (DataCenter._product.TestCondition.TestItemArray == null || DataCenter._product.TestCondition.TestItemArray.Length == 0)
            {
                return;
            }
            //
            foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
            {
                if (item.Type == ETestType.IFH || item.Type == ETestType.IVSWEEP || item.Type == ETestType.VISWEEP || item.Type==ETestType.ESD)
                {
                    continue;
                }

                foreach (TestResultData data in item.MsrtResult)
                {
                    this._currentTestArray.Add(data.KeyName);
                }
            }
        }

        private void UpdateFilterCriterionToDGV()
        {
            //--------------------------------------------------------------
            // (01)Update System Filter Format From Sysyem
            //--------------------------------------------------------------
            string gaintKeyName = string.Empty;
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

            string[] gainRowData4 = new string[this.dgvFilter.ColumnCount];

            this.dgvFilter.AllowUserToAddRows = true;

            foreach (TestItemData item in testItems)
            {
                if (item.GainOffsetSetting != null)
                {
                    for (int i = 0; i < item.GainOffsetSetting.Length; i++)
                    {
                        if (item.GainOffsetSetting[i].IsEnable == false || item.GainOffsetSetting[i].IsVision == false)
                            continue;

                        if ((item is LOPWLTestItem) && item.GainOffsetSetting[i].KeyName.IndexOf("_") > 0)
                        {
                            gaintKeyName = item.GainOffsetSetting[i].KeyName.Remove(item.GainOffsetSetting[i].KeyName.IndexOf("_"));
                            switch (gaintKeyName)
                            {
                                case "LOP":        // EOptiMsrtType.LOP :
                                    gainRowData4[2] = item.GainOffsetSetting[i].Name + "(mcd)";
                                    break;
                                //------------------------------------------------
                                case "WATT":       // EOptiMsrtType.WATT
                                    gainRowData4[2] = item.GainOffsetSetting[i].Name + "(mW)";
                                    break;
                                //------------------------------------------------
                                case "LM":         // EOptiMsrtType.LM
                                    gainRowData4[2] = item.GainOffsetSetting[i].Name + "(lm)";
                                    break;
                                //------------------------------------------------
                                default:
                                    gainRowData4[2] = item.GainOffsetSetting[i].Name;
                                    break;
                            }
                        }
                        else
                        {
                            gainRowData4[2] = item.GainOffsetSetting[i].Name;
                        }


                        if (DataCenter._fileCompare.FilterDic.ContainsKey(item.GainOffsetSetting[i].KeyName)) // Load Recipe
                        {
                            FilterData fd = DataCenter._fileCompare.FilterDic[item.GainOffsetSetting[i].KeyName];
                            gainRowData4[1] = fd.KeyName;
                            gainRowData4[3] = fd.IsEnable.ToString();
                            gainRowData4[4] = fd.Min.ToString();
                            gainRowData4[5] = fd.Max.ToString();

                            // 當Filter Dic存在時，才把資料顯示出來設定。
                            // 否則會出現，當WLC Msrt Item不存在時，會抓到WLD的keyName。
                            // 造成存檔案異常

                            this.dgvFilter.Rows.Add(gainRowData4);

                        }
                        else
                        {
                            //gainRowData4[1] = item.MsrtResult[i].KeyName;
                            //gainRowData4[3] = item.MsrtResult[i].IsVerify.ToString();
                            //gainRowData4[4] = item.MsrtResult[i].MinLimitValue.ToString();
                            //gainRowData4[5] = item.MsrtResult[i].MaxLimitValue.ToString();
                        }

                      

                        if ((itemIndex % 2) == 0)
                        {
                            this.dgvGainOffset.Rows[gainIndex].DefaultCellStyle.BackColor = Color.White;
                            this.dgvGainOffset.Rows[gainIndex].Cells[0].Style.BackColor = Color.YellowGreen;

                            this.dgvGainOffset2.Rows[gainIndex].DefaultCellStyle.BackColor = Color.White;
                            this.dgvGainOffset2.Rows[gainIndex].Cells[0].Style.BackColor = Color.YellowGreen;
                        }
                        else
                        {
                            this.dgvGainOffset.Rows[gainIndex].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                            this.dgvGainOffset.Rows[gainIndex].Cells[0].Style.BackColor = Color.YellowGreen;

                            this.dgvGainOffset2.Rows[gainIndex].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                            this.dgvGainOffset2.Rows[gainIndex].Cells[0].Style.BackColor = Color.YellowGreen;
                        }
                        gainIndex++;
                    }
                }
                itemIndex++;
            }
            this.dgvFilter.AllowUserToAddRows = false;
            this.dgvFilter.Update();
            this.dgvFilter.Refresh();
        }

        private void UpdatAllDataToUI()
        {
            this.UpdateCalcGainOffsetDGV();
            this.UpdateCoefDGV();
			this.UpdateWaveAndLopChart();
            this.UpdateResultStatistics();
            this.UpdateVoltDisplay();
            this.UpdatePowDisplay();
            this.UpateCIEDisplay();
            this.AddItemToCalc();

            this.UpdateChannelData();
        }

        private void UpateCIEDisplay()
        {
            if (!this.tabpCIEColor.Visible)
            {
                return;
            }
            string lopTableStr = (this.cmbDisplayTable.SelectedIndex+1).ToString();

            Dictionary<string, int> cieIndex = new Dictionary<string, int>();

            Dictionary<string, double[]> cieShowData = new Dictionary<string, double[]>();

            Dictionary<string, int> compareIndex = new Dictionary<string, int>();

            Dictionary<string, CalcGainOffset> calcGainOffset = new Dictionary<string, CalcGainOffset>();

            string[] CIEKeyName = new string[6] { "M_CCT_" + lopTableStr, "D_CCT_" + lopTableStr, "D_CIEx_" + lopTableStr, "D_CIEy_" + lopTableStr, "D_CRI_" + lopTableStr ,"M_TEST"};

            int[] CIEKeyIndex = new int[4];

            List<string> item = new List<string>();

            for (int i = 1; i < DataCenter._fileCompare.CompareTable.Columns.Count; i++)
            {
                compareIndex.Add(DataCenter._fileCompare.CompareTable.Columns[i].ColumnName.ToString(),i);
            }

            foreach (string s in CIEKeyName)
            {
                if (compareIndex.ContainsKey(s))
                {
                    cieIndex.Add(s, compareIndex[s]);
                }
                else
                {
                    cieIndex.Add(s, -1);
                }
            }


            foreach (var kvp in cieIndex)
            {
                double[] data = new double[DataCenter._fileCompare.CompareTable.Rows.Count];

                for (int row = 0; row < DataCenter._fileCompare.CompareTable.Rows.Count; row++)
                {
                    Double.TryParse(DataCenter._fileCompare.CompareTable.Rows[row][kvp.Value].ToString(), out data[row]);
                }
                cieShowData.Add(kvp.Key, data);
            }

            foreach (CalcGainOffset gainoffset in _calcGainOffsetArray)
            {
                if (gainoffset.KeyName == "CCT_" + lopTableStr || gainoffset.KeyName == "CIEx_" + lopTableStr 
                    || gainoffset.KeyName == "CIEy_" + lopTableStr || gainoffset.KeyName=="CRI_"+ lopTableStr
                    || gainoffset.KeyName=="TEST")
                {
                    calcGainOffset.Add(gainoffset.KeyName, gainoffset);
                }

            }

            //for (int idx = 0; idx < CIEKeyName.Length; idx++)
            //{
            //    for (int i = 1; i < DataCenter._fileCompare.CompareTable.Columns.Count; i++)
            //    {
            //        string rowStr = DataCenter._fileCompare.CompareTable.Columns[i].Caption.ToString();

            //        if (rowStr == (CIEKeyName[idx]))
            //        {
            //            CIEKeyIndex[idx] = i;
            //            break;
            //        }
            //    }
            //}

            PlotGraph.Clear(zgcCIEx);

            PlotGraph.Clear(zgcCIEy);

            PlotGraph.Clear(zgcCCT);

            PlotGraph.Clear(zgcCRI);

            //List<string[]> report = new List<string[]>();

            //report.Add(new string[10]{"CIEx Yout","CIEx Xin","CIEx Xin_Calib","CIEy Yout","CIEy Xin","CIEy Xin_Calib",
            //                                             "CCT Yout","CCT Xin","CCT Xin_Y=ax+b","new CCT" });


            //for (int i = 0; i < CalcCCT.Length; i++)
            //{
            //    string[] row = new string[11];

            //    row[0]=calcGainOffset["CIEx_" + lopTableStr].Yout[i].ToString();

            //    row[1] = calcGainOffset["CIEx_" + lopTableStr].Xin[i].ToString();

            //    row[2] = calcGainOffset["CIEx_" + lopTableStr].YCalibration[i].ToString();

            //    row[3] = calcGainOffset["CIEy_" + lopTableStr].Yout[i].ToString();

            //    row[4] = calcGainOffset["CIEy_" + lopTableStr].Xin[i].ToString();

            //    row[5] = calcGainOffset["CIEy_" + lopTableStr].YCalibration[i].ToString();

            //    row[6] = calcGainOffset["CCT_" + lopTableStr].Yout[i].ToString();

            //    row[7] = calcGainOffset["CCT_" + lopTableStr].Xin[i].ToString();

            //    row[8] = calcGainOffset["CCT_" + lopTableStr].YCalibration[i].ToString();

            //    row[9] = CalcCCT[i].ToString();

            //    row[10] = CalcCCT2[i].ToString();

            //    report.Add(row);

            //}


           // CSVUtil.WriteCSV(Constants.Paths.MES_FILE_PATH+"2444.csv", report);

            if (DataCenter._userManag.CurrentAuthority == EAuthority.Admin || DataCenter._userManag.CurrentAuthority == EAuthority.Super
                || DataCenter._userManag.CurrentUserName=="simulator")
            {
                double[] CIEx = calcGainOffset["CIEx_" + lopTableStr].YCalibration;

                double[] CIEy = calcGainOffset["CIEy_" + lopTableStr].YCalibration;

                double[] CCTYout = calcGainOffset["CCT_" + lopTableStr].Yout;

            

                double[] CalcCCT = new double[CIEx.Length];

              //  double[] CalcCCT2 = new double[CIEx.Length];

                double[] DeltaCalcCCT = new double[CIEx.Length];

                for (int i = 0; i < CalcCCT.Length; i++)
                {
                    CalcCCT[i] = MPI.Tester.Maths.ColorMath.CommonCCTCalculate.Robertson31PointsMethod(CIEx[i], CIEy[i]);
                   // CalcCCT2[i] = MpiSpam.CommonCCTCalculate.McCamyMethod(CIEx[i], CIEy[i]);
                    DeltaCalcCCT[i] = CCTYout[i] - CalcCCT[i];
                }

                double dev = MPI.Maths.Statistic.StandardDeviation(calcGainOffset["CCT_" + lopTableStr].Delta);

                double devNew = MPI.Maths.Statistic.StandardDeviation(DeltaCalcCCT);

                double devCIEx = MPI.Maths.Statistic.StandardDeviation(calcGainOffset["CIEx_" + lopTableStr].Delta);

                double devCIEy = MPI.Maths.Statistic.StandardDeviation(calcGainOffset["CIEy_" + lopTableStr].Delta);

                this.txtCIExAvg.Text = MPI.Maths.Statistic.Average(calcGainOffset["CIEx_" + lopTableStr].Delta).ToString("0.00000");

                this.txtCIEyAvg.Text = MPI.Maths.Statistic.Average(calcGainOffset["CIEy_" + lopTableStr].Delta).ToString("0.00000");

                this.txtCCTAvg.Text = MPI.Maths.Statistic.Average(calcGainOffset["CCT_" + lopTableStr].Delta).ToString("0.00");

                this.txtCIExStdev.Text = devCIEx.ToString("0.00000");

                this.txtCIEyStdev.Text = devCIEy.ToString("0.00000");

                this.txtCCTStdev.Text = devNew.ToString("0.00");

                PlotGraph.DrawPlot(zgcCIEx, cieShowData[CIEKeyName[0]], cieShowData[CIEKeyName[2]], false, 2.0F, Color.Blue, SymbolType.Diamond, true, true, true, CIEKeyName[2]);

                PlotGraph.DrawPlot(zgcCIEy, cieShowData[CIEKeyName[0]], cieShowData[CIEKeyName[3]], false, 2.0F, Color.Blue, SymbolType.Diamond, true, true, true, CIEKeyName[3]);

                PlotGraph.DrawPlot(zgcCCT, cieShowData[CIEKeyName[0]], cieShowData[CIEKeyName[1]], false, 2.0F, Color.Blue, SymbolType.Diamond, true, true, true, CIEKeyName[1]);

                PlotGraph.DrawPlot(zgcCRI, cieShowData[CIEKeyName[0]], cieShowData[CIEKeyName[4]], false, 2.0F, Color.Blue, SymbolType.Diamond, true, true, true, CIEKeyName[4]);

                if (calcGainOffset.ContainsKey("CCT_" + lopTableStr) && calcGainOffset.ContainsKey("CIEx_" + lopTableStr))
                {
                    PlotGraph.DrawPlot(zgcCCT, calcGainOffset["CCT_" + lopTableStr].Xin, calcGainOffset["CCT_" + lopTableStr].Delta, false, 2.0F, Color.Red, SymbolType.Diamond, true, true, true, "CCT (y=ax+b) 殘差");

                    PlotGraph.DrawPlot(zgcCCT, calcGainOffset["CCT_" + lopTableStr].Xin, DeltaCalcCCT, false, 2.0F, Color.Green, SymbolType.Circle, true, true, true, "CCT (校正後CIExy)");

                    PlotGraph.DrawPlot(zgcCIEx, calcGainOffset["CCT_" + lopTableStr].Xin, calcGainOffset["CIEx_" + lopTableStr].Delta, false, 2.0F, Color.Red, SymbolType.Diamond, true, true, true, "CIEx (y=ax+b) 殘差");

                    PlotGraph.SetXYAxis(zgcCIEx, 0, 0, -0.005, 0.005);

                    PlotGraph.SetXYAxis(zgcCIEy, 0, 0, -0.005, 0.005);

                    PlotGraph.DrawPlot(zgcCIEy, calcGainOffset["CCT_" + lopTableStr].Xin, calcGainOffset["CIEy_" + lopTableStr].Delta, false, 2.0F, Color.Red, SymbolType.Diamond, true, true, true, "CIEy (y=ax+b) 殘差");

                    PlotGraph.DrawPlot(zgcCRI, calcGainOffset["CCT_" + lopTableStr].Xin, calcGainOffset["CRI_" + lopTableStr].Delta, false, 2.0F, Color.Red, SymbolType.Diamond, true, true, true, "CRI (y=ax+b) 殘差");
                }
            }
            else
            {
                PlotGraph.DrawPlot(zgcCIEx, cieShowData[CIEKeyName[5]], cieShowData[CIEKeyName[2]], false, 2.0F, Color.Blue, SymbolType.Diamond, true, true, true, CIEKeyName[2]);

                PlotGraph.DrawPlot(zgcCIEy, cieShowData[CIEKeyName[5]], cieShowData[CIEKeyName[3]], false, 2.0F, Color.Blue, SymbolType.Diamond, true, true, true, CIEKeyName[3]);

                PlotGraph.DrawPlot(zgcCCT, cieShowData[CIEKeyName[5]], cieShowData[CIEKeyName[1]], false, 2.0F, Color.Blue, SymbolType.Diamond, true, true, true, CIEKeyName[1]);

                PlotGraph.DrawPlot(zgcCRI, cieShowData[CIEKeyName[5]], cieShowData[CIEKeyName[4]], false, 2.0F, Color.Blue, SymbolType.Diamond, true, true, true, CIEKeyName[4]);
            }

        }

        private void UpdateVoltDisplay()
        {
			//if (DataCenter._toolConfig.IsEnableT100OperationMode)
			//{
			//    if (DataCenter._fileCompare.StdTable != null && DataCenter._fileCompare.MsrtTable != null)
			//    {
			//        this.tabiData.Visible = true;
			//        this.dgvCompData.DataSource = null;
			//        this.dgvCompData.DataSource = DataCenter._fileCompare.StdTable;
			//        this.dgvMsrtData.DataSource = null;
			//        this.dgvMsrtData.DataSource = DataCenter._fileCompare.MsrtTable;

			//        for (int i = 0; i < DataCenter._fileCompare.StdTable.Columns.Count; i++)
			//        {
			//            this.dgvCompData.Columns[i].HeaderCell.Value = DataCenter._fileCompare.StdTable.Columns[i].Caption;
			//            this.dgvMsrtData.Columns[i].HeaderCell.Value = DataCenter._fileCompare.MsrtTable.Columns[i].Caption;
			//        }
			//    }
			//}

            // Volt Clear
            this.zedDisplayVolt1.GraphPane.CurveList.Clear();
            this.zedDisplayVolt2.GraphPane.CurveList.Clear();
            this.zedDisplayVolt3.GraphPane.CurveList.Clear();
            this.zedDisplayVolt4.GraphPane.CurveList.Clear();
        
            //
            string XaxiskeyName = "M_TEST";
            string[] voltKeyName = new string[4];
            string[] voltForceItemName = new string[4];
            int XKeyIndex = 0;
            int[] voltKeyIndex = new int[4];
            double[][] yArray = new double[4][];
            double[] xArray;

          

            int index=0;

            foreach (KeyValuePair<string, string> kvp in DataCenter._fileCompare.TitleName)
            {
                if (this._currentTestArray.Contains(kvp.Key))
                {
                    if (kvp.Key.Contains("MVF"))
                    {
                        if (index >= 4)
                        {
                            continue;
                        }

                        voltKeyName[index] = kvp.Value;
                        voltForceItemName[index] = kvp.Key;
                        index++;
                    }          
                }
            }

			if (DataCenter._toolConfig.VoltChartXBaseSelect == 1)
			{
				XaxiskeyName = "M_WLD1";
			}
			else 
            {
				XaxiskeyName = "M_TEST";
            }

            // Paul check it
            if (DataCenter._product.TestCondition.TestItemArray == null)
                return;

            for (int i = 0; i < voltForceItemName.Length; i++)
            {
                foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
                {
                    if (item.Type == ETestType.IF)
                    {
                        foreach (TestResultData data in item.MsrtResult)
                        {
                            if (voltForceItemName[i] == data.KeyName)
                            {
                                voltForceItemName[i] = item.Name+"_"+item.ElecSetting[0].ForceValue + item.ElecSetting[0].ForceUnit;
                            }
                            break;
                        }

                    }
                }
            }
           
            for (int i = 0; i < voltKeyName.Length; i++)
            {
                voltKeyName[i] = "D_" + voltKeyName[i];
            }

			//----------------------------------
            // Search base x key Index
			//----------------------------------
			if (DataCenter._fileCompare.CompareTable == null ||  DataCenter._fileCompare.CompareTable.Rows.Count == 0)
				return;

            for (int i = 1; i < DataCenter._fileCompare.CompareTable.Columns.Count; i++)
            {
                string rowStr = DataCenter._fileCompare.CompareTable.Columns[i].Caption.ToString();
                if (rowStr == XaxiskeyName)
                {
                    XKeyIndex = i;
                    break;
                }
            }
            //----------------------------------
            // Search Volt1-Volt4 Key Index
            //----------------------------------
            for(int idx=0;idx<voltKeyName.Length;idx++)
            {
                 for (int i = 1; i < DataCenter._fileCompare.CompareTable.Columns.Count; i++)
                {
                        string rowStr = DataCenter._fileCompare.CompareTable.Columns[i].Caption.ToString();
                        if (rowStr == (voltKeyName[idx]))
                        {
                            voltKeyIndex[idx]=i ;
                            break;
                        }
                 }
            }

            xArray = new double[DataCenter._fileCompare.CompareTable.Rows.Count];
      
            for (int idx = 0; idx < voltKeyName.Length; idx++)
            {
                 yArray[idx] = new double[DataCenter._fileCompare.CompareTable.Rows.Count];

                 if (voltKeyIndex[idx]!=0)
                 {
                     for (int row = 0; row < DataCenter._fileCompare.CompareTable.Rows.Count; row++)
                     {
                         xArray[row] = 0;
                         yArray[idx][row] = 0;

                         Double.TryParse(DataCenter._fileCompare.CompareTable.Rows[row][XKeyIndex].ToString(), out xArray[row]);
                         Double.TryParse(DataCenter._fileCompare.CompareTable.Rows[row][voltKeyIndex[idx]].ToString(), out yArray[idx][row]);

                     }
                 }        
            }

            // Set Label 
            PlotGraph.SetLabel(this.zedDisplayVolt1, voltForceItemName[0], XaxiskeyName.Replace("M_", ""), voltKeyName[0], 16);
            PlotGraph.SetLabel(this.zedDisplayVolt2, voltForceItemName[1], XaxiskeyName.Replace("M_", ""), voltKeyName[1], 16);
            PlotGraph.SetLabel(this.zedDisplayVolt3, voltForceItemName[2], XaxiskeyName.Replace("M_", ""), voltKeyName[2], 16);
            PlotGraph.SetLabel(this.zedDisplayVolt4, voltForceItemName[3], XaxiskeyName.Replace("M_", ""), voltKeyName[3], 16);
            // Draw 
            PlotGraph.DrawPlot(zedDisplayVolt1,xArray, yArray[0], false, 2.0F, Color.Blue, SymbolType.Diamond, true, true, true, voltKeyName[0]);
            PlotGraph.DrawPlot(zedDisplayVolt2, xArray, yArray[1], false, 2.0F, Color.Blue, SymbolType.Diamond, true, true, true, voltKeyName[1]);
            PlotGraph.DrawPlot(zedDisplayVolt3, xArray, yArray[2], false, 2.0F, Color.Blue, SymbolType.Diamond, true, true, true, voltKeyName[2]);
            PlotGraph.DrawPlot(zedDisplayVolt4, xArray, yArray[3], false, 2.0F, Color.Blue, SymbolType.Diamond, true, true, true, voltKeyName[3]);

        }

        private void UpdatePowDisplay()
        {
            // Volt Clear
            this.zedDisplayPow1.GraphPane.CurveList.Clear();
            this.zedDisplayPow2.GraphPane.CurveList.Clear();

            //
            string XaxiskeyName = "M_TEST";
            string[] powKeyName = new string[4];
            string[] powForceItemName = new string[4];
            int XKeyIndex = 0;
            int[] powKeyIndex = new int[4];
            double[][] yArray = new double[4][];
            double[] xArray;


            int index = 0;

            foreach (KeyValuePair<string, string> kvp in DataCenter._fileCompare.TitleName)
            {
                if (this._currentTestArray.Contains(kvp.Key))
                {
                    if (kvp.Key.Contains("PDWATT"))
                    {
                        if (index >= 4)
                        {
                            continue;
                        }

                        powKeyName[index] = kvp.Value;
                        powForceItemName[index] = kvp.Key;
                        index++;
                    }
                }
            }

            XaxiskeyName = "M_TEST";

            //if (DataCenter._toolConfig.VoltChartXBaseSelect == 1)
            //{
            //    XaxiskeyName = "M_WLD1";
            //}
            //else
            //{
            //    XaxiskeyName = "M_TEST";
            //}

            // Paul check it
            if (DataCenter._product.TestCondition.TestItemArray == null)
                return;

            for (int i = 0; i < powForceItemName.Length; i++)
            {
                foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
                {
                    if (item.Type == ETestType.IF)
                    {
                        foreach (TestResultData data in item.MsrtResult)
                        {
                            if (powForceItemName[i] == data.KeyName)
                            {
                                powForceItemName[i] = item.Name + "_" + item.ElecSetting[0].ForceValue + item.ElecSetting[0].ForceUnit;
                            }
                            break;
                        }

                    }
                }
            }

            for (int i = 0; i < powKeyName.Length; i++)
            {
                powKeyName[i] = "D_" + powKeyName[i];
            }

            //----------------------------------
            // Search base x key Index
            //----------------------------------
            if (DataCenter._fileCompare.CompareTable == null || DataCenter._fileCompare.CompareTable.Rows.Count == 0)
                return;

            for (int i = 1; i < DataCenter._fileCompare.CompareTable.Columns.Count; i++)
            {
                string rowStr = DataCenter._fileCompare.CompareTable.Columns[i].Caption.ToString();
                if (rowStr == XaxiskeyName)
                {
                    XKeyIndex = i;
                    break;
                }
            }
            //----------------------------------
            // Search Volt1-Volt4 Key Index
            //----------------------------------
            for (int idx = 0; idx < powKeyName.Length; idx++)
            {
                for (int i = 1; i < DataCenter._fileCompare.CompareTable.Columns.Count; i++)
                {
                    string rowStr = DataCenter._fileCompare.CompareTable.Columns[i].Caption.ToString();
                    if (rowStr == (powKeyName[idx]))
                    {
                        powKeyIndex[idx] = i;
                        break;
                    }
                }
            }

            xArray = new double[DataCenter._fileCompare.CompareTable.Rows.Count];

            for (int idx = 0; idx < powKeyName.Length; idx++)
            {
                yArray[idx] = new double[DataCenter._fileCompare.CompareTable.Rows.Count];

                if (powKeyIndex[idx] != 0)
                {
                    for (int row = 0; row < DataCenter._fileCompare.CompareTable.Rows.Count; row++)
                    {
                        xArray[row] = 0;
                        yArray[idx][row] = 0;

                        Double.TryParse(DataCenter._fileCompare.CompareTable.Rows[row][XKeyIndex].ToString(), out xArray[row]);
                        Double.TryParse(DataCenter._fileCompare.CompareTable.Rows[row][powKeyIndex[idx]].ToString(), out yArray[idx][row]);

                    }
                }
            }

            // Set Label 
            PlotGraph.SetLabel(this.zedDisplayPow1, powForceItemName[0], XaxiskeyName.Replace("M_", ""), powKeyName[0], 16);
            PlotGraph.SetLabel(this.zedDisplayPow2, powForceItemName[1], XaxiskeyName.Replace("M_", ""), powKeyName[1], 16);

            // Draw Spec Boundary
            double[] xPoints = new double[xArray.Length];
            double[] ySpecUpLimit = new double[xArray.Length];
            double[] ySpecLowLimit = new double[xArray.Length];

            for (int i = 0; i < xArray.Length; i++)
            {
                xPoints[i] = i + 1;
                ySpecUpLimit[i] = this.dinLightPowerSpec.Value + 1;
                ySpecLowLimit[i] = 1 - this.dinLightPowerSpec.Value;
            }

            PlotGraph.DrawPlot(this.zedDisplayPow1, xPoints, ySpecUpLimit, true, 1.0F, Color.Tomato, SymbolType.None, true, true, false, "AA");
            PlotGraph.DrawPlot(this.zedDisplayPow1, xPoints, ySpecLowLimit, true, 1.0F, Color.Tomato, SymbolType.None, true, true, false, "AA");

            PlotGraph.DrawPlot(this.zedDisplayPow2, xPoints, ySpecUpLimit, true, 1.0F, Color.Tomato, SymbolType.None, true, true, false, "AA");
            PlotGraph.DrawPlot(this.zedDisplayPow2, xPoints, ySpecLowLimit, true, 1.0F, Color.Tomato, SymbolType.None, true, true, false, "AA");


            // Draw Data Point
            PlotGraph.DrawPlot(this.zedDisplayPow1, xArray, yArray[0], false, 2.0F, Color.Blue, SymbolType.Diamond, true, true, false, powKeyName[0]);
            PlotGraph.DrawPlot(this.zedDisplayPow2, xArray, yArray[1], false, 2.0F, Color.Blue, SymbolType.Diamond, true, true, false, powKeyName[1]);
        }

		private void UpdateWaveAndLopChart()
		{
			if (this._calcCoefArray == null)
				return;

			if (this.Visible == false)
				return;

			//--------------------------------------------------
			// (1) Clear chart and Init. the chart level
			//--------------------------------------------------
			if ( DataCenter._toolConfig.IsAutoClearChart)
			{
				this.ClearChart();
			}
			this.InitChartLabel();

			//--------------------------------------------------
			// (2) Plot the coef. line 
			//--------------------------------------------------
			this.PlotCoefToChart();

			//--------------------------------------------------
			// (3) Plot the boundary line
			//--------------------------------------------------
			this.PlotBoundary();

			//--------------------------------------------------
			// (4) Plot the Data Points, 
			//		*** Decide the final scale ***
			//--------------------------------------------------
			this.PlotCompareDataToChart();

		}

		private void UpdateCoefDGV()
		{
			for (int i = 0; i < DataCenter._toolConfig.IsCaculateCoeff.Length; i++)
			{
				this.dgvCoef.Columns[i + 1].Visible = DataCenter._toolConfig.IsCaculateCoeff[i];
			}
			
			if (this._calcCoefArray == null)
				return;

			if (this.cmbDisplayTable.SelectedIndex < 0)
				return;

			double[][] coef = this._calcCoefArray[this.cmbDisplayTable.SelectedIndex].CoefParamArray;	// [ col ][ row ] = [ 7 ][ count ] 
			CalcGainOffset[] calcGainOffset = this._calcCoefArray[this.cmbDisplayTable.SelectedIndex].GainOffsetArray;
			if (coef == null)
				return;

			if (this.dgvCoef.Rows.Count == 1)
				return;

			int digit = 0;
			string format = string.Empty;			
			double value;

			this.dgvCoef.Columns[0].DefaultCellStyle.BackColor = Color.YellowGreen;
			this.dgvCoef.Rows.Clear();
			for (int row = 0; row < coef[0].Length; row++)
			{
				this.dgvCoef.Rows.Add();

                for (int col = 0; col < coef.Length; col++)
                {
                    switch (col)
                    {
                        case 0:
                            format = "F1";
                            break;
                        //----------------------------------
                        case 1:
                            digit = calcGainOffset[col - 1].Digit + calcGainOffset[col - 1].ExtOffsetDigit;
                            format = "F" + digit.ToString();
                            this.colWLP.DisplayFormat = format;
                            break;
                        //----------------------------------
                        case 2:
                            digit = calcGainOffset[col - 1].Digit + calcGainOffset[col - 1].ExtOffsetDigit;
                            format = "F" + digit.ToString();
                            this.colWLD.DisplayFormat = format;
                            break;
                        //----------------------------------
                        case 3:
                            digit = calcGainOffset[col - 1].Digit + calcGainOffset[col - 1].ExtOffsetDigit;
                            format = "F" + digit.ToString();
                            this.colWLC.DisplayFormat = format;
                            break;
                        //----------------------------------
                        case 4:
                            digit = calcGainOffset[col - 1].Digit + calcGainOffset[col - 1].ExtGainDigit;
                            format = "F" + digit.ToString();
                            this.colLOP.DisplayFormat = format;
                            break;
                        //----------------------------------
                        case 5:
                            digit = calcGainOffset[col - 1].Digit + calcGainOffset[col - 1].ExtGainDigit;
                            format = "F" + digit.ToString();
                            this.colWATT.DisplayFormat = format;
                            break;
                        //----------------------------------
                        case 6:
                            digit = calcGainOffset[col - 1].Digit + calcGainOffset[col - 1].ExtGainDigit;
                            format = "F" + digit.ToString();
                            this.colLM.DisplayFormat = format;
                            break;
                        //----------------------------------
                        case 7:
                            digit = calcGainOffset[col - 1].Digit + calcGainOffset[col - 1].ExtOffsetDigit;
                            format = "F" + digit.ToString();
                            this.colHW.DisplayFormat = format;
                            break;
                        //----------------------------------
                        default:
                            break;

                    }
                    value = Math.Round(coef[col][row], digit, MidpointRounding.AwayFromZero);

                    if (value < double.MaxValue)
                    {
                        this.dgvCoef.Rows[row].Cells[col].Value = value.ToString(format);
                    }
                    else
                    {
                        this.dgvCoef.Rows[row].Cells[col].Value = 9999;
                    }


                  
                }
			}

			//--------------------------------------------------
			// Reset the colum data of its vision = Disable
			//--------------------------------------------------
			for ( int i = 0; i<(this.dgvCoef.Columns.Count - 1); i++ )
			{
				if (this.dgvCoef.Columns[i + 1].Visible == false &&
					 ((i + 1) >= 4) && ((i + 1) <= 6))
				{
					for (int row = 0; row < this.dgvCoef.Rows.Count; row++)
					{
						this.dgvCoef.Rows[row].Cells[i + 1].Value = 1.0d;
					}
				}
				else if (this.dgvCoef.Columns[i + 1].Visible == false)
				{
					for (int row = 0; row < this.dgvCoef.Rows.Count; row++)
					{
						this.dgvCoef.Rows[row].Cells[i + 1].Value = 0.0d;
					}
				}
			}

			this.dgvCoef.AllowUserToAddRows = false;
			this.dgvCoef.Update();
			this.dgvCoef.Refresh();
		}

		private void UpdateCalcGainOffsetDGV()
		{		

			if (this._calcGainOffsetArray == null)
				return;

			string keyName = string.Empty;			
			int row = 0;

			this.dgvCalcGainOffset.Rows.Clear();
			
			for( int index = 0; index<this.dgvGainOffset.Rows.Count; index++)
			{
				keyName = this.dgvGainOffset[1 , index].Value.ToString();
				//------------------------------------------------------------
				// Find "CalcGainOffset" in this._calcGainOffsetArray
				//------------------------------------------------------------
				foreach( CalcGainOffset oneCalcGainOffset in this._calcGainOffsetArray)
				{
					if (oneCalcGainOffset.Name == "NONE")
						continue;

					if (oneCalcGainOffset.KeyName == keyName)
					{
						this.dgvCalcGainOffset.Rows.Add();
						this.dgvCalcGainOffset.Rows[row].Cells[0].Value = (row + 1).ToString();								// NO
						this.dgvCalcGainOffset.Rows[row].Cells[1].Value = oneCalcGainOffset.KeyName;						// KeyName
						this.dgvCalcGainOffset.Rows[row].Cells[2].Value = oneCalcGainOffset.Name;							// Name
						this.dgvCalcGainOffset.Rows[row].Cells[3].Value = ((int)oneCalcGainOffset.CalcType).ToString();		// Type
						this.dgvCalcGainOffset.Rows[row].Cells[4].Value = oneCalcGainOffset.Sqaure;							// Square
						this.dgvCalcGainOffset.Rows[row].Cells[5].Value = oneCalcGainOffset.Gain;							// Gain
						this.dgvCalcGainOffset.Rows[row].Cells[6].Value = oneCalcGainOffset.Offset;							// Offset

						this.dgvCalcGainOffset[4, row].Style.BackColor = Color.White;
						this.dgvCalcGainOffset[5, row].Style.BackColor = Color.White;
						this.dgvCalcGainOffset[6, row].Style.BackColor = Color.White;

						row++;
						break;
					}
				}			

				//------------------------------------------------------------
				// Find "CalcGainOffset" in this._calcCoefArray
				//------------------------------------------------------------
				for ( int i = 0; i < this._calcCoefArray.Length; i++)
				{
					foreach (CalcGainOffset oneCalcGainOffset in this._calcCoefArray[i].GainOffsetArray)
					{
						if (oneCalcGainOffset.Name == "NONE")
							continue;

						if (keyName == oneCalcGainOffset.KeyName)
						{
							this.dgvCalcGainOffset.Rows.Add();
							this.dgvCalcGainOffset.Rows[row].Cells[0].Value = (row + 1).ToString();								// NO
							this.dgvCalcGainOffset.Rows[row].Cells[1].Value = oneCalcGainOffset.KeyName;						// KeyName
							this.dgvCalcGainOffset.Rows[row].Cells[2].Value = oneCalcGainOffset.Name;							// Name
							this.dgvCalcGainOffset.Rows[row].Cells[3].Value = ((int)oneCalcGainOffset.CalcType).ToString();		// Type
							this.dgvCalcGainOffset.Rows[row].Cells[4].Value = oneCalcGainOffset.Sqaure;							// Square
							this.dgvCalcGainOffset.Rows[row].Cells[5].Value = oneCalcGainOffset.Gain;							// Gain
							this.dgvCalcGainOffset.Rows[row].Cells[6].Value = oneCalcGainOffset.Offset;							// Offset							

							this.dgvCalcGainOffset[4 , row].Style.BackColor = Color.White;
							this.dgvCalcGainOffset[5, row].Style.BackColor = Color.White;
							this.dgvCalcGainOffset[6, row].Style.BackColor = Color.White;
							row++;
							break;
						}
					}
				}
			}
				
			this.dgvCalcGainOffset.Columns[0].DefaultCellStyle.BackColor = Color.YellowGreen;
		}

		private void UpdateResultStatistics()
		{
			this.dgvDispCondition.Rows[0].Cells[1].Value = DataCenter._fileCompare.LoadStdDataCount.ToString();			// RawDataCount = dtStd.Count
			this.dgvDispCondition.Rows[0].Cells[3].Value = DataCenter._fileCompare.LoadMsrtDataCount.ToString();
			this.dgvDispCondition.Rows[1].Cells[1].Value = DataCenter._fileCompare.FilterDataCount.ToString();
			this.dgvDispCondition.Rows[1].Cells[3].Value = DataCenter._fileCompare.FilterDataPercent.ToString("p");

			if (DataCenter._fileCompare.FilterDataPercent < 0.6)
			{
				DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show("刪除過多晶粒，請確認資料是否有誤 ？", "Error Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
				if (result != DialogResult.OK)
					return;
			}

			if (this._calcCoefArray != null)
			{
				// this.dgvDispCondition.Rows[1].Cells[1].Value = this._calcCoefArray[this.cmbDisplayTable.SelectedIndex].UnderSTDEVPercent.ToString("p");
				this.dgvDispCondition.Rows[2].Cells[1].Value = this._calcCoefArray[this.cmbDisplayTable.SelectedIndex].GetUnderSpecPercent(this.cmbDisplayWave.SelectedIndex, DataCenter._toolConfig.WavelenghtSpec).ToString("p");
				this.dgvDispCondition.Rows[2].Cells[3].Value = this._calcCoefArray[this.cmbDisplayTable.SelectedIndex].GetUnderSpecPercent(this.cmbDisplayLOP.SelectedIndex + 3, DataCenter._toolConfig.LightPowerSpec).ToString("p");
			}
		}

		private void UpdateStdFilterDGV()
		{
			this.dgvStdFilter.Rows.Clear();
			this._currentTestArray.Clear();

			if (DataCenter._product.TestCondition.TestItemArray == null || DataCenter._product.TestCondition.TestItemArray.Length == 0)
			{
				return;
			}

			foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
			{
                if (item.Type == ETestType.IFH || item.Type == ETestType.IVSWEEP || item.Type == ETestType.VISWEEP || item.Type == ETestType.ESD)
				{
					continue;
				}

				foreach (TestResultData data in item.MsrtResult)
				{
					this._currentTestArray.Add(data.KeyName);
				}
			}

			if (DataCenter._fileCompare.FilterDic == null)
				return;

			this.dgvStdFilter.AllowUserToAddRows = true;
			//this.dgvCompareFilter.AllowUserToAddRows = true;

			int row = 0;
			this.dgvStdFilter.Rows.Clear();

			foreach (FilterData fdata in DataCenter._fileCompare.FilterDic.Values)
			{
				this.dgvStdFilter.Rows.Add();
				this.dgvStdFilter[0, row].Value = (string)fdata.Name;
				this.dgvStdFilter[1, row].Value = (bool)fdata.IsEnable;
				this.dgvStdFilter[2, row].Value = (double)fdata.Min;
				this.dgvStdFilter[3, row].Value = (double)fdata.Max;
				this.dgvStdFilter[4, row].Value = fdata.Unit;

				if (_currentTestArray.Contains(fdata.KeyName))
				{
					this.dgvStdFilter.Rows[row].Visible = true;
				}
				else
				{
					this.dgvStdFilter.Rows[row].Visible = false;
				}
				if (fdata.Name == "null")
				{
					this.dgvStdFilter.Rows[row].Visible = false;
				}

				row++;
			}

            //row = 0;
            //this.dgvCompareFilter.Rows.Clear();
            //foreach (FilterData fdata in DataCenter._fileCompare.FilterCompareDic.Values)
            //{
            //    this.dgvCompareFilter.Rows.Add();
            //    this.dgvCompareFilter[0, row].Value = (string)fdata.Name;
            //    this.dgvCompareFilter[1, row].Value = (bool)fdata.IsEnable;
            //    this.dgvCompareFilter[2, row].Value = (double)fdata.Min;
            //    this.dgvCompareFilter[3, row].Value = (double)fdata.Max;
            //    this.dgvCompareFilter[4, row].Value = fdata.Unit;

            //    if (_currentTestArray.Contains(fdata.KeyName))
            //    {
            //        this.dgvCompareFilter.Rows[row].Visible = true;
            //    }
            //    else
            //    {
            //        this.dgvCompareFilter.Rows[row].Visible = false;
            //    }
            //    if (fdata.Name == "null")
            //    {
            //        this.dgvCompareFilter.Rows[row].Visible = false;
            //    }
            //    row++;
            //}

            //this.dgvCompareFilter.Columns[2].Visible = false;

			this.dgvStdFilter.AllowUserToAddRows = false;
		//	this.dgvCompareFilter.AllowUserToAddRows = false;

			this.dgvStdFilter.Update();
		//	this.dgvCompareFilter.Refresh();

			row = 0;
			dgvTitleName.Rows.Clear();


#if ( !DebugVer )
			this.dgvTitleName.Columns[1].Visible = false;
#else
			this.dgvTitleName.Columns[1].Visible = true;
#endif

			foreach (KeyValuePair<string, TitleData> kvp in DataCenter._fileCompare.TitleInfor)
			{
				dgvTitleName.Rows.Add();

				//this.dgvTitleName.Rows[row].HeaderCell.Value = row.ToString("00");
				this.dgvTitleName[0, row].Value = row + 1;
				this.dgvTitleName[1, row].Value = kvp.Value.KeyName;
				this.dgvTitleName[2, row].Value = kvp.Value.Name;
				this.dgvTitleName[3, row].Value = ((int)kvp.Value.GainOffsetType).ToString();
				this.dgvTitleName[4, row].Value = kvp.Value.Format;
				row++;
			}

		}

		private void UpdateLinearRegressDgv()
		{
			this.dgvLinearRegress.Rows.Clear();
			int row = 0;

			foreach (KeyValuePair<string, GainOffset> kvp in this._linearCalcResultData)
			{
				this.dgvLinearRegress.Rows.Add();
				this.dgvLinearRegress.Rows[row].Cells[2].Value = kvp.Value.Name;
				this.dgvLinearRegress.Rows[row].Cells[5].Value = kvp.Value.Gain.ToString("0.0000");
				this.dgvLinearRegress.Rows[row].Cells[6].Value = kvp.Value.Offset.ToString("0.000");
				this.dgvLinearRegress.Rows[row].Cells[7].Value = kvp.Value.RSqure.ToString("0.0000");
				row++;
			}
		}

		private void UpdateLinearRegressAndCalc()
		{
			this.zedLinearRegress.GraphPane.CurveList.Clear();
			string item = this.cmbCalcItem.SelectedItem.ToString();
			//Plot
			PlotGraph.SetLabel(this.zedLinearRegress, "", "M_" + item, "S_" + item, 14);
			//   PlotGraph.SetXYAxis(this.zedVF1, min, max, 0, 0);
			PlotGraph.DrawPlot(this.zedLinearRegress, this._selectedCalcData[0].ToArray(), this._selectedCalcData[1].ToArray(),
									false, 2.0F, Color.Blue, SymbolType.Diamond, true, true, true, item);

			this.zedLinearRegress.GraphPane.XAxis.Scale.Format = "#";
			this.zedLinearRegress.GraphPane.XAxis.Scale.Mag = 0;
			this.zedLinearRegress.GraphPane.XAxis.Scale.Format = "#";
			this.zedLinearRegress.GraphPane.YAxis.Scale.Mag = 0;

			this._lstRegress.Calculate(this._selectedCalcData[0].ToArray(), this._selectedCalcData[1].ToArray());
			
			StringBuilder sb = new StringBuilder();
			sb.Append("y =");
			sb.Append(this._lstRegress.Slop.ToString("0.0000"));

			if (this._lstRegress.Intercept > 0)
			{
				sb.Append(" x + ");
				sb.Append(this._lstRegress.Intercept.ToString("0.0000"));
			}
			else
			{
				sb.Append(" x - ");
				sb.Append(Math.Abs(this._lstRegress.Intercept).ToString("0.0000"));
			}
			sb.Append(" ;  Rsqure = ");
			sb.Append(this._lstRegress.Rsquare.ToString("0.0000"));

			this.lblLinearResult.Text = sb.ToString();

			foreach (KeyValuePair<string, string> kvp in DataCenter._fileCompare.TitleName)
			{
				if (kvp.Value == item)
				{
					GainOffset gainOffset = new GainOffset();
					gainOffset.Name = kvp.Value;
					gainOffset.KeyName = kvp.Key;
					gainOffset.Gain = this._lstRegress.Slop;
					gainOffset.Offset = this._lstRegress.Intercept;
					gainOffset.RSqure = this._lstRegress.Rsquare;
					if (this._linearCalcResultData.ContainsKey(kvp.Key))
					{
						this._linearCalcResultData.Remove(kvp.Key);
					}
					this._linearCalcResultData.Add(kvp.Key, gainOffset);
				}
			}

			double max = MPI.Maths.Statistic.Max(this._selectedCalcData[0].ToArray());
			double min = MPI.Maths.Statistic.Min(this._selectedCalcData[0].ToArray());

			double ymax = this._lstRegress.Slop * max + this._lstRegress.Intercept;
			double ymin = this._lstRegress.Slop * min + this._lstRegress.Intercept;

			PlotGraph.DrawPlot(this.zedLinearRegress, new double[2] { min, max }, new double[2] { ymin, ymax },
								 true, 2.0F, Color.Red, SymbolType.Diamond, true, true, false, "None");
		}

		private void ClearChart()
		{
			this.zedWaveLength.GraphPane.CurveList.Clear();
			this.zedMcdGainValue.GraphPane.CurveList.Clear();
			this.zedWaveLength.Refresh();
			this.zedMcdGainValue.Refresh();

			this.zedDisplayVolt1.GraphPane.CurveList.Clear();
			this.zedDisplayVolt2.GraphPane.CurveList.Clear();
			this.zedDisplayVolt3.GraphPane.CurveList.Clear();
			this.zedDisplayVolt4.GraphPane.CurveList.Clear();
			this.zedDisplayVolt1.Refresh();
			this.zedDisplayVolt2.Refresh();
			this.zedDisplayVolt3.Refresh();
			this.zedDisplayVolt4.Refresh();

            this.zedDisplayPow1.GraphPane.CurveList.Clear();
            this.zedDisplayPow2.GraphPane.CurveList.Clear();
            this.zedDisplayPow1.Refresh();
            this.zedDisplayPow2.Refresh();
		}

        private void PlotBoundary()
        {
            double[] Xvalue;

			if (DataCenter._toolConfig.IsPlotBoundary == false)
				return;

            if (this._calcCoefArray == null)
                return;

            int tableIndex = this.cmbDisplayTable.SelectedIndex;
            if (tableIndex >= 0)
            {
                Xvalue = new double[] { this._calcCoefArray[tableIndex].MinWave, this._calcCoefArray[tableIndex].MaxWave };
            }
            else
            {
                Xvalue = new double[] { this._calcCoefArray[0].MinWave, this._calcCoefArray[0].MaxWave };
            }

            double[] Boundary1 = new double[Xvalue.Length];
            double[] Boundary2 = new double[Xvalue.Length];
            double[] Boundary3 = new double[Xvalue.Length];
            double[] Boundary4 = new double[Xvalue.Length];
            double[] LopGainBase = new double[Xvalue.Length];

            for (int i = 0; i < Xvalue.Length; i++)
            {
                Boundary1[i] = this.dinWavelengthSpec.Value;
                Boundary2[i] = -1 * this.dinWavelengthSpec.Value;
                Boundary3[i] = this.dinLightPowerSpec.Value + 1;
                Boundary4[i] = 1 - this.dinLightPowerSpec.Value;
                LopGainBase[i] = 1;
            }
            PlotGraph.DrawPlot(this.zedWaveLength, Xvalue, Boundary1, true, 1.0F, Color.Tomato, SymbolType.None, true, true, false, "AA");
            PlotGraph.DrawPlot(this.zedWaveLength, Xvalue, Boundary2, true, 1.0F, Color.Tomato, SymbolType.None, true, true, false, "AA");
            PlotGraph.DrawPlot(this.zedMcdGainValue, Xvalue, Boundary3, true, 1.0F, Color.Tomato, SymbolType.None, true, true, false, "AA");
            PlotGraph.DrawPlot(this.zedMcdGainValue, Xvalue, Boundary4, true, 1.0F, Color.Tomato, SymbolType.None, true, true, false, "AA");
            //PlotGraph.DrawTwoPointsLine(Xvalue, LopGainBase, SymbolType.None, Color.Black, zedMcdGainValue, 1.0F);
        }

        private void PlotCompareDataToChart()
        {
            Color color = Color.Blue;
			int tableIndex = this.cmbDisplayTable.SelectedIndex;
			bool isFillPoint = DataCenter._toolConfig.IsFillDataPoint;

			if (DataCenter._toolConfig.IsPlotDataPoints == false)
				return;

			if (this.Visible == false)
				return;

			if (this._calcCoefArray == null)
				return;

			if (tableIndex == -1)
				return;

            if (!DataCenter._toolConfig.IsAutoClearChart)
            {
                if (DataCenter._toolConfig.IsSetDataPointsColor)
                {
                    color = Color.FromArgb(DataCenter._toolConfig.DataPointColor);  // colorPicker.SelectedColor; 
                }
                else
                {
                    int index = this._colorIndex % this.randomColor.Length;
                    color = randomColor[index];
                }
            }
            else
            {
                if (DataCenter._toolConfig.IsSetDataPointsColor)
                {
                    color = Color.FromArgb(DataCenter._toolConfig.DataPointColor);  // colorPicker.SelectedColor; 
                }
            }


			SymbolType pointType = SymbolType.Diamond;
			pointType = (SymbolType)Enum.Parse(typeof(SymbolType), this.cmbDrawPointType.SelectedItem.ToString(), true);
	
            PlotGraph.SetXYAxis(zedWaveLength, this._calcCoefArray[tableIndex].MinWave, this._calcCoefArray[tableIndex].MaxWave, 0, 0);
            PlotGraph.SetXYAxis(zedMcdGainValue, this._calcCoefArray[tableIndex].MinWave, this._calcCoefArray[tableIndex].MaxWave, 0, 0);

            int WLSlectedIndex = this.cmbDisplayWave.SelectedIndex;

            if (this.cmbDisplayWave.SelectedIndex == 3)
            {
				WLSlectedIndex = 6;
            }

            if (this._calcCoefArray[tableIndex].CompareData != null)
            {

                PlotGraph.DrawPlot(zedWaveLength, this._calcCoefArray[tableIndex].TableBaseWave,
                                                  this._calcCoefArray[tableIndex].CompareData[WLSlectedIndex],
                                                  false, 2.0F, color, pointType, isFillPoint, true, false, "None");

                PlotGraph.DrawPlot(zedMcdGainValue, this._calcCoefArray[tableIndex].TableBaseWave,
                this._calcCoefArray[tableIndex].CompareData[this.cmbDisplayLOP.SelectedIndex + 3],
                                                  false, 2.0F, color, pointType, isFillPoint, true, false, "None");
            }
            this.colorPicker.SelectedColor = color;
            this._colorIndex++;

        }

        private void PlotCoefToChart()
        {
            //--------------------------------------------------------------------			
            // Plot the coef from DateGridView to chart 
            //--------------------------------------------------------------------
            double[] wave = new double[this.dgvCoef.Rows.Count];
            double[] waveOffset = new double[this.dgvCoef.Rows.Count];
            double[] lopGain = new double[this.dgvCoef.Rows.Count];

			if (DataCenter._toolConfig.IsPlotCoefCurve == false)
				return;

            for (int row = 0; row < this.dgvCoef.Rows.Count; row++)
            {
				wave[row] = Convert.ToDouble(this.dgvCoef.Rows[row].Cells[0].Value);	// Wavelength
   
                if (this.cmbDisplayWave.SelectedIndex < 3)		// WLP =0 // WLD =1 // WLC=2							
                {
					waveOffset[row] = double.Parse(this.dgvCoef[this.cmbDisplayWave.SelectedIndex + 1, row].Value.ToString());	// WLP, WLD, WLC
                }
				else																		
                {
					waveOffset[row] = double.Parse(this.dgvCoef[7, row].Value.ToString());		//HW
                }

                lopGain[row] = Convert.ToDouble(this.dgvCoef[this.cmbDisplayLOP.SelectedIndex + 4, row].Value);		// LOP, WATT, LM

                if(lopGain[row] == 0)
                {
                    lopGain[row] = 1;
                }
            }


			if (DataCenter._toolConfig.CalcCoefMode == 0)
			{
				PlotGraph.DrawPlot(zedWaveLength, wave, waveOffset,
									true, 2.0F, Color.DeepPink, SymbolType.Square, true, true, false, "None");
				PlotGraph.DrawPlot(zedMcdGainValue, wave, lopGain,
									true, 2.0F, Color.DeepPink, SymbolType.Square, true, true, false, "None");
			}
			else
			{
				PlotGraph.DrawPlot(zedWaveLength, wave, waveOffset,
									true, 2.0F, Color.DeepPink, SymbolType.Square, false, true, false, "None");
				PlotGraph.DrawPlot(zedMcdGainValue, wave, lopGain,
									true, 2.0F, Color.DeepPink, SymbolType.Square, false, true, false, "None");
			}	
        }        

        private double[][] CopyDgvDataToCoeffArray()
        {
            int LOPWL_Count = DataCenter._conditionCtrl.GetSubItemCount(ETestType.LOPWL);
            double[][] coefTable =new double[this.dgvCoef.Rows.Count][];

            for (int row = 0; row < this.dgvCoef.Rows.Count; row++)
            {
                double[] rowArray = new double[this.dgvCoef.Columns.Count];
				for (int col = 0; col < this.dgvCoef.Columns.Count; col++)
				{
					rowArray[col] = Convert.ToDouble(this.dgvCoef.Rows[row].Cells[col].Value.ToString());

					if (col == 0)
					{
						this.dgvCoef[col, row].Style.BackColor = Color.YellowGreen;
					}
					else
					{
						this.dgvCoef[col, row].Style.BackColor = Color.Yellow;
					}
				}
				coefTable[row] = rowArray;
            }

            return coefTable;
        }

		private void RefreshChartCoef()
		{
			if (this.cmbDisplayTable.SelectedIndex == -1)
				return; 
	
			double[][] coef = this._calcCoefArray[this.cmbDisplayTable.SelectedIndex].CoefParamArray;
			if ( coef == null)
				return;

			double dd = -1;

			for (int row = 0; row < coef[0].Length && row < this.dgvCoef.Rows.Count ; row++)
			{
				for (int col = 0; col < (coef.Length - 1); col++)
				{
					if (this.dgvCoef.Rows[row].Cells[col].Value == null)
						break;

					double.TryParse(dgvCoef.Rows[row].Cells[col].Value.ToString(), out dd);
					coef[col + 1][row] = dd;
				}
			}
			this.UpdateCoefDGV();
			this.UpdateWaveAndLopChart();
		}

		private bool LoadFormate(EUserID user,string formatName)
        {
            //------------------------------------------
            // Load C:\MPI\LEDTester current format
            //------------------------------------------
            string fileNameWithExt = string.Format("{0}{1}", "User", ((int)user).ToString("0000")) + ".xml";
            string pathAndFileName = Path.Combine(Constants.Paths.USER_DIR, fileNameWithExt);


			//-----------------------------------------------------
			// (1) Set parameters of current product from UI setting
			//-----------------------------------------------------
			DataCenter._fileCompare.LOPSaveItem = DataCenter._product.LOPSaveItem;
			DataCenter._fileCompare.CalBaseWave = DataCenter._product.TestCondition.CalByWave;
				
			//-----------------------------------------------------
			// (2) Load format data from files 
			//-----------------------------------------------------
			if (DataCenter._fileCompare.LoadCurrentFormat(pathAndFileName, formatName) == false)
			{
				MessageBox.Show("Load Current Format Error");
				return false;
			}

			//-----------------------------------------------------
            // (3) Load filter data 
			//-----------------------------------------------------
            //  pathAndFileName = Path.Combine(Constants.Paths.TOOLS_DIR, "FilterCriteria.xml");

            // if (DataCenter._fileCompare.LoadFilterData(DataCenter._product.TestCondition.TestItemArray, pathAndFileName,DataCenter._toolConfig.IsEnableUseRecipeCriterion) == false)
            //{
            //    MessageBox.Show("Load Filter Data Error");
            //    return false;
            //}

			//-----------------------------------------------------
			// (4) Parser title data with test items
			//-----------------------------------------------------
			DataCenter._fileCompare.ParseTitleDataIndex(DataCenter._product.TestCondition.TestItemArray);			

            return true;
        }

		private void SaveCompareData()
		{
			//  string fileName = "Comp_" + Path.GetFileName(txtStdPathAndFile.Text) + " @ " + Path.GetFileName(txtMsrtPathAndFile.Text);
            string pathAndFileName = Path.Combine(DataCenter._toolConfig.CompareFileDir, this.lblComparedFileName.Text+".csv");

			if (Directory.Exists(Path.GetDirectoryName(pathAndFileName)) == false)
			{
				pathAndFileName = Path.Combine(Constants.Paths.MPI_TEMP_DIR, this.lblComparedFileName.Text);
			}
			DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show(pathAndFileName + " 存檔 ", "Save Message?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
			if (result == DialogResult.OK)
			{
				DataCenter._fileCompare.SaveCompareTable(pathAndFileName);
			}
		}

		private void SaveFilterData()
		{
            //----------------------------------------------------------
            // Save UI Filter Setting To System
            //-------------------------------------------------------------
            if (dgvFilter == null)
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

                if (DataCenter._fileCompare.FilterDic.ContainsKey(keyName))
                {
                    FilterData filterData = DataCenter._fileCompare.FilterDic[keyName];
                    filterData.IsEnable = bool.Parse(this.dgvFilter[3, idx].Value.ToString());
                    filterData.Max = double.Parse(this.dgvFilter[5, idx].Value.ToString());
                    filterData.Min = double.Parse(this.dgvFilter[4, idx].Value.ToString());
                }
            }
            //----------------------------------------------------------
            // Save Filter Setting To File when tool setting  isn't use recipe Criterion
            //-------------------------------------------------------------

            if (!DataCenter._toolConfig.IsEnableUseRecipeCriterion)
            {
                string pathAndFileName = Path.Combine(Constants.Paths.TOOLS_DIR, "FilterCriteria.xml");
                DataCenter._fileCompare.SaveFilterDataToFile(pathAndFileName);		
            }

		}

        //private void SaveFilterDataToFile()
        //{
        //    string pathAndFileName = Path.Combine(Constants.Paths.TOOLS_DIR, "FilterCriteria.xml");
        //    XmlDocument xmlFilter = new XmlDocument();
        //    string str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
        //                     "<?xml-stylesheet type=\"text/xsl\"" +
        //                     " href=\"FormatA.xslt\"?><Filter></Filter>";

        //    xmlFilter.LoadXml(str);
        //    XmlNode rootNode = xmlFilter.DocumentElement;

        //    //----------------------------------------------------------
        //    // Append "ItemData" element to XML
        //    //----------------------------------------------------------

        //    XmlElement stdElemSet = xmlFilter.CreateElement("Std");
        //    rootNode.AppendChild(stdElemSet);

        //    //XmlElement compareElemSet = xmlFilter.CreateElement("Compare");
        //    //rootNode.AppendChild(compareElemSet);

        //    //----------------------------------------------------------
        //    // Write Filter Setting To XML
        //    //-------------------------------------------------------------
        //    int index = 0;
        //    foreach (string keyName in DataCenter._fileCompare.FilterDic.Keys)
        //    {
        //        XmlElement stdElem = xmlFilter.CreateElement(this.GetHeader(index));
        //        stdElem.InnerText = keyName;
        //        FilterData filterData = DataCenter._fileCompare.FilterDic[keyName];
        //        stdElem.SetAttribute("name", filterData.Name.ToString());

        //        if (filterData.IsEnable)
        //        {
        //            stdElem.SetAttribute("enable", "1");
        //        }
        //        else
        //        {
        //            stdElem.SetAttribute("enable", "0");
        //        }
        //        stdElem.SetAttribute("max", filterData.Max.ToString());
        //        stdElem.SetAttribute("min", filterData.Min.ToString());
        //        stdElemSet.AppendChild(stdElem);
        //        index++;
        //    }
        //    XmlTextWriter xtw = new XmlTextWriter(pathAndFileName, null);
        //    xtw.Formatting = System.Xml.Formatting.Indented;
        //    xmlFilter.Save(xtw);
        //    xtw.Close();
        //}

		private void MoveCoefDataToSys()
		{
			if (this._calcCoefArray == null)
				return;

			string coeffTableName = this.cmbDisplayTable.SelectedItem.ToString().Replace("Coef_", "");
			int coeffTableNum = 0;
			int.TryParse(coeffTableName, out coeffTableNum);

			//--------------------------------------------------------------------
			// (1) Obtained each Coeff Table Maximum Wavelength range
			//--------------------------------------------------------------------
			double dispCoefStartWL = 350;
			double dispCoefEndWL = 1050;

			for (int i = 0; i < this._calcCoefArray.Length; i++)
			{
				if (this._calcCoefArray[i].MinWave > dispCoefStartWL)
				{
					dispCoefStartWL = this._calcCoefArray[i].MinWave;
				}
				if (this._calcCoefArray[i].MaxWave < dispCoefEndWL
					 && this._calcCoefArray[i].MaxWave > dispCoefStartWL)
				{
					dispCoefEndWL = this._calcCoefArray[i].MaxWave;
				}
			}

			DataCenter._product.DispCoefStartWL = dispCoefStartWL;
			DataCenter._product.DispCoefEndWL = dispCoefEndWL;

			//--------------------------------------------------------------------
			// (2) Create Coeff Table
			//--------------------------------------------------------------------
			if (DataCenter._product.TestCondition == null || DataCenter._product.TestCondition.TestItemArray == null)
				return;

			foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
			{
				if ((item is LOPWLTestItem) && item.KeyName == "LOPWL_" + (coeffTableNum).ToString())
				{
					for (int i = 0; i < this._calcCoefArray.Length; i++)
					{
						if (this._calcCoefArray[i].Number == coeffTableNum)
						{
							(item as LOPWLTestItem).CreateCoefTable(DataCenter._sysSetting.CoefTableStartWL,
								DataCenter._sysSetting.CoefTableEndWL, DataCenter._sysSetting.CoefTableResolution);
						}
					}

				}
			}

			//--------------------------------------------------------------------
			// (3) Set Tools Coeff To System
			//--------------------------------------------------------------------
			this._coeffTable = this.CopyDgvDataToCoeffArray();
			double[][] systemCoeff;

			foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
			{
				if (item is LOPWLTestItem && item.KeyName == "LOPWL_" + (coeffTableNum).ToString())
				{
					systemCoeff = (item as LOPWLTestItem).CoefTable;

					for (int m = 0; m < systemCoeff.Length; m++)
					{
						for (int n = 0; n < this._coeffTable.Length; n++)
						{
							if (Convert.ToInt32(systemCoeff[m][0] * 1000.0d) == Convert.ToInt32(_coeffTable[n][0] * 1000.0d))
							{
								for (int k = 1; k < _coeffTable[0].Length; k++)
								{
									systemCoeff[m][k] = _coeffTable[n][k];
								}
								break;
							}
						}
					}
				}
			}
		}

		private void MoveGainOffsetDataToSys()
		{
			TestItemData[] testItems = DataCenter._product.TestCondition.TestItemArray;

			int row = 0;
			for (row = 0; row < this.dgvGainOffset.Rows.Count; row++)
			{
				foreach (TestItemData item in testItems)
				{
					if (item.GainOffsetSetting != null)
					{
						GainOffsetData[] data = item.GainOffsetSetting;
						for (int i = 0; i < data.Length; i++)
						{
							if (this.dgvGainOffset[1, row].Value.ToString() == data[i].KeyName)
							{
								data[i].Square = Convert.ToDouble(this.dgvGainOffset[4, row].Value.ToString());
								data[i].Gain = Convert.ToDouble(this.dgvGainOffset[5, row].Value.ToString());
								data[i].Offset = Convert.ToDouble(this.dgvGainOffset[6, row].Value.ToString());

								data[i].Square2 = Convert.ToDouble(this.dgvGainOffset2[4, row].Value.ToString());
								data[i].Gain2 = Convert.ToDouble(this.dgvGainOffset2[5, row].Value.ToString());
								data[i].Offset2 = Convert.ToDouble(this.dgvGainOffset2[6, row].Value.ToString());

								dgvGainOffset[4, row].Style.BackColor = Color.Yellow;
								dgvGainOffset[5, row].Style.BackColor = Color.Yellow;
								dgvGainOffset[6, row].Style.BackColor = Color.Yellow;

								dgvGainOffset2[4, row].Style.BackColor = Color.Yellow;
								dgvGainOffset2[5, row].Style.BackColor = Color.Yellow;
								dgvGainOffset2[6, row].Style.BackColor = Color.Yellow;

								break;
							}
						}
					}
				}
			}
		}

		private void SetToolsConfig()
		{
			DataCenter._toolConfig.DispWaveItem = this.cmbDisplayWave.SelectedIndex;
			DataCenter._toolConfig.DispLOPItem = this.cmbDisplayLOP.SelectedIndex;
            DataCenter._toolConfig.LookTableByStdOrMsrtMode = this.cmbLookTabByStdOrMsrt.SelectedIndex;


//#if ( !DebugVer )			
//            DataCenter._toolConfig.LookTableByStdOrMsrtMode = 1;
//#else
//            DataCenter._toolConfig.LookTableByStdOrMsrtMode = this.cmbLookTabByStdOrMsrt.SelectedIndex;
//#endif



			//-----------------------------------------------------------
			// for Filter Condition Panel
			//-----------------------------------------------------------

            if (this.rdCompareByRowCol.Checked)
            {
                DataCenter._toolConfig.IsArrangeByRowCol = true;
            }
            else if (this.rdCompareDataBySeq.Checked)
            {
                 DataCenter._toolConfig.IsArrangeByRowCol = false;
            }
			//DataCenter._toolConfig.IsArrangeByRowCol = this.chkByRowCol.Checked;
			DataCenter._toolConfig.CalcCoefMode = this.cmbCalcCoefMode.SelectedIndex;
			DataCenter._toolConfig.FilterStdevCount = (int)this.numFilterStdevCount.Value;
			DataCenter._toolConfig.ExtendWLMode = this.cmbExtendWLMode.SelectedIndex;
			DataCenter._toolConfig.ExtendWLPoint = this.numExtendWLPoint.Value;
			DataCenter._toolConfig.ExtendWLStart = this.dinExtWLStart.Value;
			DataCenter._toolConfig.ExtendWLEnd = this.dinExtWLEnd.Value;

			DataCenter._toolConfig.WaveExtDigit = this.numWaveExtDigit.Value;
			DataCenter._toolConfig.LOPExtDigit = this.numLOPExtDigit.Value;
			//-----------------------------------------------------------
			// for Tools Setting Panel
			//-----------------------------------------------------------
			DataCenter._toolConfig.WavelenghtSpec = (double)this.dinWavelengthSpec.Value;
			DataCenter._toolConfig.LightPowerSpec = (double)this.dinLightPowerSpec.Value;
			DataCenter._toolConfig.VoltChartXBaseSelect = this.cmbVoltDisplayXBase.SelectedIndex;
			DataCenter._toolConfig.IsAutoClearChart = this.chkIsAutoClearChart.Checked;

			DataCenter._toolConfig.IsPlotCoefCurve = Convert.ToBoolean(this.cmbIsPlotCoeffLine.SelectedItem.ToString());
			DataCenter._toolConfig.IsPlotDataPoints = Convert.ToBoolean(this.cmbIsPlotDataPoint.SelectedItem.ToString());
			DataCenter._toolConfig.IsPlotBoundary = Convert.ToBoolean(this.cmbIsPlotBrundyLine.SelectedItem.ToString());

			DataCenter._toolConfig.PlotPointType = this.cmbDrawPointType.SelectedIndex;
			DataCenter._toolConfig.IsFillDataPoint = Convert.ToBoolean(this.cmbIsFillDataPoint.SelectedItem.ToString());
			DataCenter._toolConfig.IsSetDataPointsColor = this.chkIsSetPlotColor.Checked;
			DataCenter._toolConfig.DataPointColor = this.colorPicker.SelectedColor.ToArgb();

			DataCenter._toolConfig.CompareFileDir = this.txtOpenCompareFilePath.Text;
			DataCenter._toolConfig.StdFileDir = this.txtOpenStdFilePath.Text;
			DataCenter._toolConfig.MsrtFileDir = this.txtOpenMsrtFilePath.Text;

			DataCenter._toolConfig.IsCaculateCoeff[0] = this.chkEnableWLP.Checked;
			DataCenter._toolConfig.IsCaculateCoeff[1] = this.chkEnableWLD.Checked;
			DataCenter._toolConfig.IsCaculateCoeff[2] = this.chkEnableWLC.Checked;
			DataCenter._toolConfig.IsCaculateCoeff[3] = this.chkEnableMCD.Checked;
			DataCenter._toolConfig.IsCaculateCoeff[4] = this.chkEnableMW.Checked;
			DataCenter._toolConfig.IsCaculateCoeff[5] = this.chkEnableLM.Checked;
			DataCenter._toolConfig.IsCaculateCoeff[6] = this.chkEnableHW.Checked;

            DataCenter._toolConfig.IsEnableUseRecipeCriterion = this.chkIsUseRecipeCriterion.Checked;
            DataCenter._toolConfig.CoeffExtendType = this.cmbCoefficientExtendType.SelectedIndex;

            DataCenter._toolConfig.IsEnableCompensateChannelFactor = this.chkCompensateChannel.Checked;
            //DataCenter._toolConfig.IsEnableT100OperationMode = this.chkT100OperatedMode.Checked;
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

            target.Cells[5].Value = Math.Round (A * C, gainDigit, MidpointRounding.AwayFromZero);			// Slop , Gain

            target.Cells[6].Value = Math.Round(C * B + D, offsetDigit, MidpointRounding.AwayFromZero); ;	// Intercept , Offset

            target.Cells[5].Style.BackColor = Color.Orange;

            target.Cells[6].Style.BackColor = Color.Orange;

            from.Cells[5].Value = 1.0;

            from.Cells[6].Value = 0.0;

            from.Cells[5].Style.BackColor = Color.Orange;

            from.Cells[6].Style.BackColor = Color.Orange;
        }

		private void CombineGainAndOffset(int tableIndex,bool IsAllItem)
		{
			if (this.dgvCalcGainOffset.CurrentCell == null)
				return;

			string fromKeyName = string.Empty;
			string toKeyName = string.Empty;
            //double A = 0.0d;
            //double B = 0.0d;
            //double C = 0.0d;
            //double D = 0.0d;


            List<CalcGainOffset> tempCoefCalcGainOffsetArray = new List<CalcGainOffset>();

            CalcGainOffset[] coefCalcGainOffsetArray = null;

            for (int i = 0; i < this._calcCoefArray.Length; i++)
            {
                tempCoefCalcGainOffsetArray.AddRange(this._calcCoefArray[i].GainOffsetArray);
            }
          
            coefCalcGainOffsetArray = tempCoefCalcGainOffsetArray.ToArray();

            //CalcGainOffset[] coefCalcGainOffsetArray = this._calcCoefArray[this.cmbDisplayTable.SelectedIndex].GainOffsetArray;

            int gainDigit = -1;
            int offsetDigit = -1;

			//====================================================================
			//	y = A * x + B , z = C * y + D , z = (A * C)x + C * B + D
			//  z = Slop * x + Intercept , Slop = A * C , Intercept = C * B + D
			//====================================================================

            if (IsAllItem)
            {
				for (int i = 0; i < this.dgvCalcGainOffset.Rows.Count; i++)
				{
					fromKeyName = this.dgvCalcGainOffset.Rows[i].Cells[1].Value.ToString();

                    for (int k = 0; k < this._calcGainOffsetArray.Length; k++)
                    {
                        if (this._calcGainOffsetArray[k].KeyName == fromKeyName)
                        {
                            offsetDigit = this._calcGainOffsetArray[k].Digit + this._calcGainOffsetArray[k].ExtOffsetDigit;

                            gainDigit = this._calcGainOffsetArray[k].Digit + this._calcGainOffsetArray[k].ExtGainDigit;
                        }
                    }

                    for (int k = 0; k < coefCalcGainOffsetArray.Length; k++)
                    {
                        if (coefCalcGainOffsetArray[k].KeyName == fromKeyName)
                        {
                            offsetDigit = coefCalcGainOffsetArray[k].Digit + coefCalcGainOffsetArray[k].ExtOffsetDigit;

                            gainDigit = coefCalcGainOffsetArray[k].Digit + coefCalcGainOffsetArray[k].ExtGainDigit;
                        }
                    }

                    if (gainDigit == -1 || offsetDigit == -1)
                        return;

					for (int j = 0; j < this.dgvGainOffset.Rows.Count; j++)
					{
						if (Convert.ToInt32(this.dgvGainOffset[3, j].Value) == 0 )
							continue;
						
						toKeyName = this.dgvGainOffset.Rows[j].Cells[1].Value.ToString();
						if (fromKeyName == toKeyName)
						{
                            // 重大Bug修正 coefficent combine 會出錯

                            this.CombineDGVData(this.dgvCalcGainOffset.Rows[i],
                                                this.dgvGainOffset.Rows[j],
                                                offsetDigit, gainDigit);
						}							  
					}
				}

				this.dgvGainOffset.Update();
				this.dgvCalcGainOffset.Update();
				return;
            }

			int selectIndex = this.dgvCalcGainOffset.CurrentCell.RowIndex;

			string selectKeyName = this.dgvCalcGainOffset.CurrentRow.Cells[1].Value.ToString();		//cell[1] = KeyName;

            for (int k = 0; k < this._calcGainOffsetArray.Length; k++)
            {
                if (this._calcGainOffsetArray[k].KeyName == selectKeyName)
                {
                    offsetDigit = this._calcGainOffsetArray[k].Digit + this._calcGainOffsetArray[k].ExtOffsetDigit;
                    gainDigit = this._calcGainOffsetArray[k].Digit + this._calcGainOffsetArray[k].ExtGainDigit;
                }
            }


            for (int k = 0; k < coefCalcGainOffsetArray.Length; k++)
            {
                if (coefCalcGainOffsetArray[k].KeyName == selectKeyName)
                {
                    offsetDigit = coefCalcGainOffsetArray[k].Digit + coefCalcGainOffsetArray[k].ExtOffsetDigit;

                    gainDigit = coefCalcGainOffsetArray[k].Digit + coefCalcGainOffsetArray[k].ExtGainDigit;
                }
            }

            if (gainDigit == -1 || offsetDigit == -1)
                return;


            //if (tableIndex == 1)
            //{
                //for (int row = 0; row < this.dgvGainOffset.Rows.Count; row++)
                //{
                //    if (Convert.ToInt32(this.dgvGainOffset[3, row].Value) == 0)
                //        continue;

                //    if (this.dgvGainOffset[1 , row].Value.ToString() == selectKeyName)		// cell[1]=keyname
                //    {
                //        A = Convert.ToDouble(this.dgvGainOffset[5, row].Value);
                //        B = Convert.ToDouble(this.dgvGainOffset[6, row].Value);

                //        C = Convert.ToDouble(this.dgvCalcGainOffset[5 , selectIndex].Value);
                //        D = Convert.ToDouble(this.dgvCalcGainOffset[6 , selectIndex].Value);

                //        this.dgvGainOffset[5, row].Value = A * C;				// Slop , Gain
                //        this.dgvGainOffset[6, row].Value = C * B + D;			// Intercept , Offset
                //        this.dgvGainOffset[5, row].Style.BackColor = Color.Orange;
                //        this.dgvGainOffset[6, row].Style.BackColor = Color.Orange;

                //        this.dgvCalcGainOffset[5, selectIndex].Value = 1.0;
                //        this.dgvCalcGainOffset[6, selectIndex].Value = 0.0;
                //        this.dgvCalcGainOffset[5, selectIndex].Style.BackColor = Color.Orange;
                //        this.dgvCalcGainOffset[6, selectIndex].Style.BackColor = Color.Orange;

                //        break;
                //    }
                //}



                for (int row = 0; (tableIndex == 1) && row < this.dgvGainOffset.Rows.Count; row++)
                {
                    if (Convert.ToInt32(this.dgvGainOffset[3, row].Value) == 0)
                        continue;

                    if (this.dgvGainOffset[1, row].Value.ToString() == selectKeyName)		// cell[1]=keyname
                    {
						// Correct Serious Error 
      					// this.CombineDGVData(this.dgvCalcGainOffset.Rows[row],   // Error
                        //                   this.dgvGainOffset.Rows[selectIndex], // Error
                        this.CombineDGVData(this.dgvCalcGainOffset.Rows[selectIndex],
                                            this.dgvGainOffset.Rows[row],
                                            offsetDigit, gainDigit);
                        break;
                    }
                }

                for (int row = 0; (tableIndex == 2) && row < this.dgvGainOffset2.Rows.Count; row++)
                {
                    if (Convert.ToInt32(this.dgvGainOffset2[3, row].Value) == 0)
                        continue;

                    if (this.dgvGainOffset2[1, row].Value.ToString() == selectKeyName)		// cell[1]=keyname
                    {
                        this.CombineDGVData(this.dgvCalcGainOffset.Rows[selectIndex],
                                            this.dgvGainOffset2.Rows[row],
                                            gainDigit, offsetDigit);
                        break;
                    }
                }


			this.dgvGainOffset.Update();
			this.dgvCalcGainOffset.Update();
			
		}

		private void AddItemToCalc()
		{
			this.cmbCalcItem.Items.Clear();
			foreach (KeyValuePair<string, string> kvp in DataCenter._fileCompare.TitleName)
			{
				if (this._currentTestArray.Contains(kvp.Key))
				{
					this.cmbCalcItem.Items.Add(kvp.Value);
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

		private string GetHeader(int idx)
		{
			string sHeader = "";
			int idx1 = 0;
			int idx2 = 0;
			do
			{
				idx1 = idx % 26;
				sHeader = Convert.ToChar('A' + idx1) + sHeader;
				idx2 = idx / 26;
				idx = idx2 - 1;
			} while (idx2 >= 1);
			return sHeader;
		}

		private bool CompareDataAndCreateCalcObject()
		{
            if (!DataCenter._fileCompare.IsMultiFileMode)
            {
                //----------------------------------------------------------------
                // (1) Load Std File
                //----------------------------------------------------------------
                DataCenter._fileCompare.LoadStdDataCount = 0;
                DataCenter._fileCompare.IsArrangeByRowCol = DataCenter._toolConfig.IsArrangeByRowCol;

                EErrorCode rtn = EErrorCode.NONE;
                rtn = DataCenter._fileCompare.LoadStdFromFile(this._stdFileName, 0);

                if (rtn != EErrorCode.NONE)
                {
                    Host.SetErrorCode(rtn);
                    this.txtStdPathAndFile.Text = "";
                    this.txtMsrtPathAndFile.Text = "";
                    return false;
                }
                //----------------------------------------------------------------
                // (1) Load Msrt File
                //----------------------------------------------------------------
                DataCenter._fileCompare.LoadMsrtDataCount = 0;

                if (DataCenter._toolConfig.IsEnableCompensateChannelFactor)
                {
                    this.LoadChannelCalib();

                    this.RunChannelCalib();

                    rtn = DataCenter._fileCompare.LoadMsrtFromFile(this._msrtFileName, 0, this._rcCtrl.DicCalcGainOffset);
                }
                else
                {
                    DataCenter._fileCompare.LoadMsrtFromFile(this._msrtFileName, 0);
                }

               

             //   rtn = DataCenter._fileCompare.LoadMsrtFromFile(this._msrtFileName, 0);
                if (rtn != EErrorCode.NONE)
                {
                    this.txtMsrtPathAndFile.Text = "";
                    Host.SetErrorCode(EErrorCode.Tools_LoadMsrtDataFail);
                    return false;
                }
            }

            this.lblComparedFileName.Text = Path.GetFileNameWithoutExtension(this._stdFileName) + " @ " + Path.GetFileNameWithoutExtension(this._msrtFileName);

			//----------------------------------------------------------------
			// (1) Filter data
			//----------------------------------------------------------------

            DataCenter._fileCompare.FilterOrignalData(DataCenter._product.TestCondition.TestItemArray);

			DataCenter._fileCompare.CompareStdAndMsrt();


            if (DataCenter._fileCompare.CompareTable == null || DataCenter._fileCompare.CompareTable.Rows.Count == 0)
            {
                Host.SetErrorCode(EErrorCode.Tools_NoCompareData);   
                return false;
            }
            else
            {
                UpdateResultStatistics();
            }


			DataCenter._fileCompare.FilterCompareData();

			//----------------------------------------------------------------
			// (2) Create the "CalcGainOffset" objects and "CalcCoef" objects
			//-----------------------------------------------------------------
			DataCenter._fileCompare.CreateCalcObj(out this._calcGainOffsetArray, out this._calcCoefArray);

			if (this._calcGainOffsetArray == null || this._calcCoefArray == null)
			{
				Host.SetErrorCode(EErrorCode.Tools_CompareDataFail);
				return false;
			}

			//----------------------------------------------------------------
			// (3) Set parameters for "CalcGainOffse" objects
			//-----------------------------------------------------------------
			for (int i = 0; i < this._calcGainOffsetArray.Length; i++)
			{
				this._calcGainOffsetArray[i].ExtOffsetDigit = DataCenter._toolConfig.WaveExtDigit;
				this._calcGainOffsetArray[i].ExtGainDigit = DataCenter._toolConfig.LOPExtDigit;
			}

			//----------------------------------------------------------------
			// (4) Set parameters for "CalcCoef" objects
			//-----------------------------------------------------------------

			int index = this.cmbDisplayTable.SelectedIndex;
			for (int i = 0; i < this._calcCoefArray[index].GainOffsetArray.Length; i++)
			{
				this._calcCoefArray[index].GainOffsetArray[i].ExtOffsetDigit = DataCenter._toolConfig.WaveExtDigit;
				this._calcCoefArray[index].GainOffsetArray[i].ExtGainDigit = DataCenter._toolConfig.LOPExtDigit;
			}

			this._calcCoefArray[index].ExtWaveMode = DataCenter._toolConfig.ExtendWLMode;
			this._calcCoefArray[index].ExtWavePoint = DataCenter._toolConfig.ExtendWLPoint;
			this._calcCoefArray[index].ExtWaveStart = DataCenter._toolConfig.ExtendWLStart;
			this._calcCoefArray[index].ExtWaveEnd = DataCenter._toolConfig.ExtendWLEnd;

			this._calcCoefArray[index].CalcCoefMode = DataCenter._toolConfig.CalcCoefMode;
			this._calcCoefArray[index].FilterStdevCount = DataCenter._toolConfig.FilterStdevCount;
			this._calcCoefArray[index].LookTableByStdMsrt = DataCenter._toolConfig.LookTableByStdOrMsrtMode;

			return true;

		}

		private void UpdateCompareDateDGV()
		{
			this.ResetCompareDateDGV();

			this.numCalCount_ValueChanged(null, null);
		}

		private void ResetCompareDateDGV()
		{
			//Update dgvCompData
			this.dgvCompData.SuspendLayout();

			this.dgvCompData.Rows.Clear();

			this.dgvCompData.Columns.Clear();

			foreach (var titleName in DataCenter._fileCompare.TitleName)
			{
				int col = this.dgvCompData.ColumnCount;

				if (this._filter.ContainsKey(titleName.Key))
				{
					this.dgvCompData.Columns.Add(titleName.Key, this._filter[titleName.Key]);
				}
				else
				{
					this.dgvCompData.Columns.Add(titleName.Key, titleName.Value);

					this.dgvCompData.Columns[col].Visible = false;
				}

				this.dgvCompData.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;

				this.dgvCompData.Columns[col].Resizable = DataGridViewTriState.False;

				this.dgvCompData.Columns[col].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

				this.dgvCompData.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

				if (titleName.Key == "TEST")
				{
					this.dgvCompData.Columns[col].Visible = true;

					this.dgvCompData.Columns[col].Width = 50;
				}
				else
				{
					this.dgvCompData.Columns[col].Width = 100;
				}
			}

			this.dgvCompData.ResumeLayout();

			//Update dgvMsrtData
			this.dgvMsrtData.SuspendLayout();

			this.dgvMsrtData.Rows.Clear();

			this.dgvMsrtData.Columns.Clear();

			foreach (var titleName in DataCenter._fileCompare.TitleName)
			{
				int col = this.dgvMsrtData.ColumnCount;

				if (this._filter.ContainsKey(titleName.Key))
				{
					this.dgvMsrtData.Columns.Add(titleName.Key, this._filter[titleName.Key]);
				}
				else
				{
					this.dgvMsrtData.Columns.Add(titleName.Key, titleName.Value);

					this.dgvMsrtData.Columns[col].Visible = false;
				}

				this.dgvMsrtData.Columns[col].SortMode = DataGridViewColumnSortMode.NotSortable;

				this.dgvMsrtData.Columns[col].Resizable = DataGridViewTriState.False;

				this.dgvMsrtData.Columns[col].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

				this.dgvMsrtData.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

				if (titleName.Key == "TEST")
				{
					this.dgvMsrtData.Columns[col].Visible = true;

					this.dgvMsrtData.Columns[col].Width = 50;
				}
				else
				{
					this.dgvMsrtData.Columns[col].Width = 100;
				}
			}

			this.dgvMsrtData.ResumeLayout();
		}

		private void AddDataToCompareDate(Dictionary<string, string> rowData)
		{
			//Check Data
			if (rowData.Count != this.dgvCompData.ColumnCount)
			{
				return;
			}

			foreach (var item in rowData)
			{
				double value = 0.0d;

				if (item.Value != "" && !double.TryParse(item.Value, out value))
				{
					return;
				}
			}

			for (int col = 0; col < this.dgvCompData.ColumnCount; col++)
			{ 
				string keyName = this.dgvCompData.Columns[col].Name;

				if (!rowData.ContainsKey(keyName))
				{
					return;
				}
			}

			//Update dgvCompData
			this.dgvCompData.SuspendLayout();

			string[] compData = new string[this.dgvCompData.ColumnCount];

			for (int col = 0; col < this.dgvCompData.ColumnCount; col++)
			{
				string keyName = this.dgvCompData.Columns[col].Name;

				if (keyName == "TEST")
				{
					compData[col] = (this.dgvCompData.RowCount + 1).ToString();
				}
				else
				{
					compData[col] = rowData[keyName];
				}
			}

			this.dgvCompData.Rows.Add(compData);

			this.dgvCompData.ResumeLayout();

			//Update dgvMsrtData
			this.dgvMsrtData.SuspendLayout();

			string[] mstData = new string[this.dgvMsrtData.ColumnCount];

			for (int col = 0; col < this.dgvMsrtData.ColumnCount; col++)
			{
				if (this.dgvMsrtData.Columns[col].Name == "TEST")
				{
					mstData[col] = (this.dgvMsrtData.RowCount + 1).ToString();
				}
				else
				{
					mstData[col] = "0";
				}
			}

			this.dgvMsrtData.Rows.Add(mstData);

			this.dgvMsrtData.ResumeLayout();
		}

		private void DeleteCompareDate(int index)
		{
			if (index >= this.dgvCompData.RowCount)
			{
				return;
			}

			this.dgvCompData.Rows.RemoveAt(index);

			this.dgvMsrtData.Rows.RemoveAt(index);
		}

		private void SavedgvCompDataToFile(string stdPathAndFile)
		{
			List<string[]> reportData = new List<string[]>();

			List<string> nameList = new List<string>();

			for (int col = 0; col < this.dgvCompData.ColumnCount; col++)
			{
				string name = string.Empty;

				foreach (var item in DataCenter._fileCompare.TitleName)
				{
					if (this.dgvCompData.Columns[col].Name == item.Key)
					{
						name = item.Value;
					}
				}

				nameList.Add(name);
			}

			reportData.Add(nameList.ToArray());

			for (int row = 0; row < this.dgvCompData.RowCount; row++)
			{
				List<string> rowData = new List<string>();

				for (int col = 0; col < this.dgvCompData.ColumnCount; col++)
				{
					rowData.Add(this.dgvCompData[col, row].Value as string);
				}

				reportData.Add(rowData.ToArray());
			}

			CSVUtil.WriteCSV(stdPathAndFile, reportData);
		}

		private void SavedgvMsrtDataToFile(string mstPathAndFile)
		{
			List<string[]> reportData = new List<string[]>();

			List<string> nameList = new List<string>();

			for (int col = 0; col < this.dgvCompData.ColumnCount; col++)
			{
				string name = string.Empty;

				foreach (var item in DataCenter._fileCompare.TitleName)
				{
					if (this.dgvCompData.Columns[col].Name == item.Key)
					{
						name = item.Value;
					}
				}

				nameList.Add(name);
			}

			reportData.Add(nameList.ToArray());

			for (int row = 0; row < this.dgvMsrtData.RowCount; row++)
			{
				List<string> rowData = new List<string>();

				for (int col = 0; col < this.dgvMsrtData.ColumnCount; col++)
				{
					rowData.Add(this.dgvMsrtData[col, row].Value as string);
				}

				reportData.Add(rowData.ToArray());
			}

			CSVUtil.WriteCSV(mstPathAndFile, reportData);
		}

		private void SaveCaliChipValue()
		{
			_caliChipValue.ChipList.Clear();

			for (int row = 0; row < this.dgvCompData.RowCount; row++)
			{
				CaliChip chip = new CaliChip();				

				for (int col = 0; col < this.dgvCompData.ColumnCount; col++)
				{
					CaliValue value = new CaliValue();

					value.IsEnable = this._filter.ContainsKey(this.dgvCompData.Columns[col].Name);

					value.KeyName = this.dgvCompData.Columns[col].Name;

					value.Name = this.dgvCompData.Columns[col].HeaderText;

					value.StdValue = double.Parse(this.dgvCompData[col, row].Value.ToString());

					value.MsrtValue = double.Parse(this.dgvMsrtData[col, row].Value.ToString());

					chip.ValueList.Add(value);
				}

				_caliChipValue.ChipList.Add(chip);
			}
		}

        private void LoadChannelCalib()
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
                                                           Path.Combine(Constants.Paths.TOOLS_DIR, "FilterCriteria.xml"),
                                                           DataCenter._toolConfig.IsEnableUseRecipeCriterion);

            this._rcCtrl.ParseTitleDataIndex(DataCenter._product.TestCondition.TestItemArray);

            this._rcCtrl.CreateGainOffset(DataCenter._product.TestCondition.TestItemArray);
        }

        private void RunChannelCalib()
        {
            this._rcCtrl.LoadStdFromFile(txtStdPathAndFile.Text, 0);

            this._rcCtrl.LoadMsrtFromFile(txtMsrtPathAndFile.Text, 0);

            this._rcCtrl.FilterOrignalData(DataCenter._product.TestCondition.TestItemArray);

            this._rcCtrl.CompareStdAndMsrt();

            this._rcCtrl.SetData(DataCenter._machineConfig.ChannelConfig.ChannelCount);

            this._rcCtrl.Calculate();

            UpdateDataToCalcCoefDgv();
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

		#endregion

		#region >>>  UI Event Handling <<<

        /// <summary>
        /// Form Load
        /// </summary>
		private void frmCaliCoeff_Load(object sender, EventArgs e)
		{
			//-------------------------------------------------
			// (1) Load the Format by User ID
			//-------------------------------------------------
			this.LoadFormate(DataCenter._uiSetting.UserID, DataCenter._uiSetting.FormatName);

			//-------------------------------------------------
			// (2) Initialize All the DGV of the tool program
			//-------------------------------------------------
			this.InitialAllDGV();

			//-------------------------------------------------
			// (3) Set system data to the DGV of the tool program
			//-------------------------------------------------
			this.UpdateSystemDataToToolsDGV();        
            this.LoadSystemTestItemToTools();
			//-------------------------------------------------
			// (4) Update Std. Filter DGV
			//-------------------------------------------------
            DataCenter._fileCompare.LoadFilterData(DataCenter._product.TestCondition.TestItemArray,
                                                                Path.Combine(Constants.Paths.TOOLS_DIR, "FilterCriteria.xml"),
                                                                DataCenter._toolConfig.IsEnableUseRecipeCriterion);
            this.UpdateFilterCriterionToDGV();

			//-------------------------------------------------
			// (5) Update dgvCompData and dgvMsrtData
			//-------------------------------------------------
			this.UpdateCompareDateDGV();

			//-------------------------------------------------
			// (6) Update all parameters data to controls
			//-------------------------------------------------
			this.UpdateDataToControls();

			//this.InitChartLabel();

			this.TopMost = false;
		}

		private void btnSelectStdFile_Click(object sender, EventArgs e)
		{
			this._openFileDialog.InitialDirectory = DataCenter._toolConfig.StdFileDir;
			this._openFileDialog.FileName = "";
			this._openFileDialog.FilterIndex = 1;    // default value = 1
			this._openFileDialog.Multiselect = false;

			if (this._openFileDialog.ShowDialog() == DialogResult.OK)
			{
                this._stdFileName = this._openFileDialog.FileName;
                this.txtStdPathAndFile.Text = this._openFileDialog.FileName;
                DataCenter._fileCompare.IsMultiFileMode = false;
				//DataCenter._toolConfig.StdFileDir = Path.GetDirectoryName(this._openFileDialog.FileName);
				DataCenter.SaveToolsConfig();
			}
		}

		private void btnSelectMsrtFile_Click(object sender, EventArgs e)
		{
			this._openFileDialog.InitialDirectory = DataCenter._toolConfig.MsrtFileDir;
			this._openFileDialog.FileName = "";
			this._openFileDialog.FilterIndex = 1;    // default value = 1
			this._openFileDialog.Multiselect = false;

			if (this._openFileDialog.ShowDialog() == DialogResult.OK)
			{
                this._msrtFileName = this._openFileDialog.FileName;
                this.txtMsrtPathAndFile.Text = this._openFileDialog.FileName;
                DataCenter._fileCompare.IsMultiFileMode = false;
				//DataCenter._toolConfig.MsrtFileDir = Path.GetDirectoryName(this._openFileDialog.FileName);
				DataCenter.SaveToolsConfig();
			}
		}

		private void btnCompareCalcGainOffset_Click(object sender, EventArgs e)
		{
			this.btnRefreshCoef.Enabled = false;
			this.btnSaveCoefTableToSys.Enabled = false;
			this.dgvCoef.Visible = false;
			//--------------------------------------------------------------------------------
			// (1) Compare the data from Std and Msrt Data, then Create "CalcObject"
			//--------------------------------------------------------------------------------
            if (this.CompareDataAndCreateCalcObject() == false)
				return;

			//--------------------------------------------------------------------------------
			// (2) Run the calculation for "CalcGainOffset" objects  and BigGainOffset of Coef Table
			//--------------------------------------------------------------------------------
			foreach (CalcGainOffset calcObj in this._calcGainOffsetArray)
			{
                calcObj.RunCalculate();
			}

			//this._calcCoefArray[this.cmbDisplayTable.SelectedIndex].RunCalculateBigGainOffset();
			
			foreach (CalcCoef item in this._calcCoefArray)
			{
				item.RunCalculateBigGainOffset();
			}

			//--------------------------------------------------------------------------------
			// (3) Clear Linear calculate data
			//--------------------------------------------------------------------------------
			this._linearCalcResultData.Clear();

			//--------------------------------------------------------------------------------
			// (4) Update all data to UI
			//--------------------------------------------------------------------------------
			this.UpdatAllDataToUI();

			//if (DataCenter._toolConfig.IsEnableT100OperationMode)
            //if (DataCenter._uiSetting.CalibrationUIMode == ECalibrationUIMode.T200)
            //{
            //    this.tabmMain.SelectedTab = this.tabiWaveLOP;
            //}
            //else if (DataCenter._uiSetting.CalibrationUIMode == ECalibrationUIMode.T100)
            //{
            //    this.tabmMain.SelectedTab = this.tabiCalcGainOffset;
            //}
            //else if (DataCenter._uiSetting.CalibrationUIMode == ECalibrationUIMode.Both)
            //{
            //    this.tabmMain.SelectedTab = this.tabiCalcGainOffset;
            //}
		}

		private void btnCompareCalcCoefTable_Click(object sender, EventArgs e)
		{
			this.btnRefreshCoef.Enabled = true;
			this.btnSaveCoefTableToSys.Enabled = true;
			this.dgvCoef.Visible = true;
			//--------------------------------------------------------------------------------
			// (1) Compare the data from Std and Msrt Data, then Create "CalcObject"
			//--------------------------------------------------------------------------------
            if (this.CompareDataAndCreateCalcObject() == false)
				return;

			//--------------------------------------------------------------------------------
			// (2) Run the calculation for "CalcGainOffset" objects  and "CalcCoef" objects
			//--------------------------------------------------------------------------------
			foreach (CalcGainOffset calcObj in this._calcGainOffsetArray)
			{
                calcObj.RunCalculate();
			}

            this._calcCoefArray[this.cmbDisplayTable.SelectedIndex].RunCalculateCoefTable(DataCenter._toolConfig.CoeffExtendType);

			//--------------------------------------------------------------------------------
			// (3) Clear Linear calculate data
			//--------------------------------------------------------------------------------
			this._linearCalcResultData.Clear();

			//--------------------------------------------------------------------------------
			// (4) Update all data to UI
			//--------------------------------------------------------------------------------
			this.UpdatAllDataToUI();

			//tabmMain.SelectedTab = this.tabiWaveLOP;
		}

		private void btnConfirmFilterSets_Click(object sender, EventArgs e)
        {
			this.SetToolsConfig();
            DataCenter.SaveToolsConfig();
            this.SaveFilterData();
            System.Threading.Thread.Sleep(30);
			//this.ClearChart();
            //---------------------------------
            //  Save Type Setting
            //---------------------------------
            this.SaveGainOffsetSetting();
            DataCenter._fileCompare.ParseTitleDataIndex(DataCenter._product.TestCondition.TestItemArray);
			//---------------------------------
            //ReLoad FilterCriteria 
			//---------------------------------
            DataCenter._fileCompare.LoadFilterData(DataCenter._product.TestCondition.TestItemArray, 
                                                                                  Path.Combine(Constants.Paths.TOOLS_DIR, "FilterCriteria.xml"),
                                                                                  DataCenter._toolConfig.IsEnableUseRecipeCriterion);

            DataCenter.SaveProductFile();
            this.UpdateSystemDataToToolsDGV();
            this.UpdateFilterCriterionToDGV();
            this.UpdateStdFilterDGV();
            this.UpdateDataToControls();           
        }

        private void SaveGainOffsetSetting()
        {
            TestItemData[] testItems = DataCenter._product.TestCondition.TestItemArray;

            int row = 0;
            for(row = 0; row < this.dgvGainOffset.Rows.Count; row++)
            {
                foreach(TestItemData item in testItems)
                {
                    if(item.GainOffsetSetting != null)
                    {
                        GainOffsetData[] data = item.GainOffsetSetting;
                        for(int i = 0; i < data.Length; i++)
                        {
                            if(this.dgvSetting[1, row].Value.ToString () == data[i].KeyName)
                            {
                                data[i].Type = (EGainOffsetType)Enum.Parse(typeof(EGainOffsetType), this.dgvSetting[3, row].Value.ToString(), true);
                            }
                        }
                    }
                }
            }
        }

        private void dinWLDYaxisMax_ValueChanged(object sender, EventArgs e)
        {
            PlotGraph.SetXYAxis(zedWaveLength, this._calcCoefArray[0].MinWave, this._calcCoefArray[0].MaxWave, this.dinWLDYaxisMin.Value,this.dinWLDYaxisMax.Value);
        }

        private void dinWLDYaxisMin_ValueChanged(object sender, EventArgs e)
        {
            PlotGraph.SetXYAxis(zedWaveLength, this._calcCoefArray[0].MinWave, this._calcCoefArray[0].MaxWave, this.dinWLDYaxisMin.Value, this.dinWLDYaxisMax.Value);
        }

        private void dinLOPYaxisMax_ValueChanged(object sender, EventArgs e)
        {
            PlotGraph.SetXYAxis(zedMcdGainValue, this._calcCoefArray[0].MinWave, this._calcCoefArray[0].MaxWave, this.dinLOPYaxisMin.Value, this.dinLOPYaxisMax.Value);
        }

        private void dinLOPYaxisMin_ValueChanged(object sender, EventArgs e)
        {
            PlotGraph.SetXYAxis(zedMcdGainValue, this._calcCoefArray[0].MinWave, this._calcCoefArray[0].MaxWave, this.dinLOPYaxisMin.Value, this.dinLOPYaxisMax.Value);
        }

        private void cmbDisplayWave_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataCenter._toolConfig.DispWaveItem = this.cmbDisplayWave.SelectedIndex;

			if (DataCenter._toolConfig.IsAutoClearChart)
            {
                this._colorIndex = 0;
                this.zedWaveLength.GraphPane.CurveList.Clear();
            }
			this.UpdateWaveAndLopChart();
            this.UpdateCoefDGV();			
        }

        private void cmbDisplayLOP_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataCenter._toolConfig.DispLOPItem = this.cmbDisplayLOP.SelectedIndex;

			if (DataCenter._toolConfig.IsAutoClearChart)
            {
                this._colorIndex = 0;
                this.zedMcdGainValue.GraphPane.CurveList.Clear();
            }
			this.UpdateWaveAndLopChart();
            this.UpdateCoefDGV();
        }
		
        private void btnSaveToolsConfig_Click(object sender, EventArgs e)
        {
            this.SetToolsConfig();
            DataCenter.SaveToolsConfig();
            this.UpdateDataToControls();
			this.UpdateWaveAndLopChart();
			this.UpdateVoltDisplay();
            this.UpdatePowDisplay();
			this.UpdateCoefDGV();
        }

        private void btnCloseTools_Click(object sender, EventArgs e)
        {
			this.Close();
        }

		private void btnOpenStdFilePath_Click(object sender, EventArgs e)
        {
            string path = this.SelectPath("");
            if (path != string.Empty)
            {
                this.txtOpenStdFilePath.Text = path;
                //DataCenter._toolConfig.StdFileDir = path;
            }
        }

        private void btnOpenMsrtFilePath_Click(object sender, EventArgs e)
        {
			string path = this.SelectPath("");
			if (path != string.Empty)
			{
				this.txtOpenMsrtFilePath.Text = path;
				//DataCenter._toolConfig.MsrtFileDir = path;
			}
        }

		private void btnOpenCompareFilePath_Click(object sender, EventArgs e)
        {
			string path = this.SelectPath("");
			if (path != string.Empty)
			{
				this.txtOpenCompareFilePath.Text = path;
				//DataCenter._toolConfig.CompareFileDir = path;
			}
        }

		private void btnSaveGainOffsetToSys_Click(object sender, EventArgs e)
        {
            DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show("確定覆蓋產品(" + DataCenter._uiSetting.ProductFileName + ")的Gain/Offset修正係數？ ", "Combine ?", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
            if (result != DialogResult.OK)
                return;

            this.MoveGainOffsetDataToSys();

			DataCenter._product.CaliChipValue = this._caliChipValue;

            if (!Host._MPIStorage.SaveTestCoefficientToFile())
            {
                Host.SetErrorCode(EErrorCode.SaveWatchCoefficientFileFail);
            }

            if (DataCenter._uiSetting.ImportCalibrateFileName != "")
            {
                DataCenter.ExportCalibrateData(DataCenter._uiSetting.ProductPath02, DataCenter._uiSetting.ImportCalibrateFileName + ".cal");
            }
            Host.UpdateDataToAllUIForm();   
			DataCenter.SaveProductFile();
        }

		private void btnReloadGainOffset_Click(object sender, EventArgs e)
		{
			this.UpdateSystemDataToToolsDGV();
		}

		private void btnSaveCoefTableToSys_Click(object sender, EventArgs e)
        {
            DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show(" 確定把此次多波段校正係數設為此產品(" + DataCenter._uiSetting.ProductFileName + ")_"+this.cmbDisplayTable.SelectedItem.ToString()+"的校正係數？ ", "Setting", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result != DialogResult.OK)
                return;

            this.MoveCoefDataToSys(); 
            DataCenter.SaveProductFile();
            DataCenter._conditionCtrl.ReloadCoefTableList();
            Host.UpdateDataToAllUIForm();

        }

		private void btnCombineToGainOffset_Click(object sender, EventArgs e)
        {
            if (this.dgvCalcGainOffset.CurrentCell == null)
                return;

			string selectName = this.dgvCalcGainOffset.CurrentRow.Cells[2].Value.ToString();

			DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show("確定合併_" + selectName + "_Gain/Offset修正係數？ ", "Combine ?", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
            if (result != DialogResult.OK)
                return;

            this.CombineGainAndOffset(1,false);
        }

		private void btnCombineToGainOffset2_Click(object sender, EventArgs e)
		{
			if (this.dgvCalcGainOffset.CurrentCell == null)
			{
				return;
			}

			string selectName = this.dgvCalcGainOffset.CurrentRow.Cells[2].Value.ToString();

			DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show("確定合併_" + selectName + "_Gain/Offset修正係數？ ", "Combine ?", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
			if (result != DialogResult.OK)
				return;

			this.CombineGainAndOffset(2, false);

			//this.dgvGainOffset.Update();
			//this.dgvCalcGainOffset.Update();
			//this.UpdateCalcGainOffsetDGV();
		}

        private void btnCombineAllGainOffset_Click(object sender, EventArgs e)
        {
            DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show("確定合併此產品(" + DataCenter._uiSetting.ProductFileName + ")的Gain/Offset修正係數？ ", "Combine ?", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
            if (result != DialogResult.OK)
                return;

		   this.CombineGainAndOffset(1,true);

		   //this.UpdateCalcGainOffsetDGV();
		   //this.dgvGainOffset.Update();
        }

        private void btnResetSysGainOffset_Click(object sender, EventArgs e)
        {
            DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show("確認把Gain/Offset係數表歸零？", "Reset", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (result != DialogResult.OK)
                return;

            for (int row = 0; row < this.dgvGainOffset.Rows.Count; row++)
            {
                this.dgvGainOffset[4, row].Value = 0.0d;		// Square
                this.dgvGainOffset[5, row].Value = 1.0d;		// Gain
                this.dgvGainOffset[6, row].Value = 0.0d;		// Offset

				this.dgvGainOffset[4, row].Style.BackColor = Color.White;
				this.dgvGainOffset[5, row].Style.BackColor = Color.White;
				this.dgvGainOffset[6, row].Style.BackColor = Color.White;

				this.dgvGainOffset2[4, row].Value = 0.0d;		// Square
				this.dgvGainOffset2[5, row].Value = 1.0d;		// Gain
				this.dgvGainOffset2[6, row].Value = 0.0d;		// Offset

				this.dgvGainOffset2[4, row].Style.BackColor = Color.White;
				this.dgvGainOffset2[5, row].Style.BackColor = Color.White;
				this.dgvGainOffset2[6, row].Style.BackColor = Color.White;
            }
            this.dgvGainOffset.Update();
        }

        private void btnSaveCompareData_Click(object sender, EventArgs e)
        {
			if (DataCenter._fileCompare.CompareTable == null ||  DataCenter._fileCompare.CompareTable.Rows.Count == 0)
			{
				Host.SetErrorCode(EErrorCode.Tools_NoCompareData);
				return;
			}

            this.SaveCompareData();
        }

        private void btnGetStdAndCurrentFormat_Click(object sender, EventArgs e)
        {
            string [] result=DataCenter._fileCompare.GetStdAndCurrentFileTitleName();
            this.lstFileTitle.Items.Clear();
            this.lstFileTitle.Items.AddRange(result);
        }

        private void dgvCoef_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            DialogResult Result;
            if (this.dgvCoef.Rows[e.RowIndex].IsNewRow)
                return;

            this.dgvCoef.Rows[e.RowIndex].ErrorText = "";
            double newValue;

            if (!double.TryParse(e.FormattedValue.ToString(), out newValue))
            {
                Result = DevComponents.DotNetBar.MessageBoxEx.Show("Data Formate Error", "Delete Data ", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                e.Cancel = true;
                this.dgvCoef.Rows[e.RowIndex].ErrorText = " Data Format Error";
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
			//this.RefreshChartCoef();
			this.UpdateWaveAndLopChart();
        }

        private void btnWatchFilterState_Click(object sender, EventArgs e)
        {       
            StringBuilder updateInfoToUI = new StringBuilder();

            updateInfoToUI.Append("**** Filtered Chips  *****");
            updateInfoToUI.Append("\n");
            updateInfoToUI.Append("Item  Std Filter / Msrt Filter");
            updateInfoToUI.Append("\n");

            foreach (KeyValuePair<string, FilterData> kvp in DataCenter._fileCompare.FilterDic)
            {
                if (kvp.Value.IsEnable == true)
                {
                    updateInfoToUI.Append(kvp.Value.Name);
                    updateInfoToUI.Append("== ");
                    updateInfoToUI.Append(kvp.Value.StdFilterCounts.ToString());
                    updateInfoToUI.Append(" / ");
                    updateInfoToUI.Append(kvp.Value.MsrtFilterCounts.ToString());
                    updateInfoToUI.Append( "\n");
                }
            }
            DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show(updateInfoToUI.ToString(), "刪除晶粒", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
        }

        private void btnOpenMultiFileForm_Click(object sender, EventArgs e)
        {
            if (this._frmMultiFile == null)
            {
                this._frmMultiFile = new frmMultiFile();
                this._frmMultiFile.ShowDialog();
            }
            else
            {
                this._frmMultiFile.ShowDialog();
            }

            this.txtStdPathAndFile.Text = "";
            this.txtMsrtPathAndFile.Text = "";
        }

        private void mouse_Doubleclick(object sender, MouseEventArgs e)
        {
            CurveItem nearestPointPair;
            int nearestIndex;
            this.zedWaveLength.GraphPane.FindNearestPoint(e.Location, out nearestPointPair, out nearestIndex);
            int tableIndex = this.cmbDisplayTable.SelectedIndex;
            int wavelengthDisplaySlectedIndex = this.cmbDisplayWave.SelectedIndex;
            double w = this._calcCoefArray[tableIndex].TableBaseWave[nearestIndex];
            double we = this._calcCoefArray[tableIndex].CompareData[wavelengthDisplaySlectedIndex][nearestIndex];
            MessageBox.Show("Point : WD =" + w.ToString() + "WD Error=" + we.ToString());
        }

        private void mouse_doubleClick(object sender, MouseEventArgs e)
        {
            if (this._selectedCalcData == null)
            {
                return;
            }

            CurveItem nearestPointPair;
            int nearestIndex;
            this.zedLinearRegress.GraphPane.FindNearestPoint(e.Location, out nearestPointPair, out nearestIndex);
            double w = this._selectedCalcData[0][nearestIndex];
            double we =  this._selectedCalcData[1][nearestIndex];
            DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show(" Delete this " + "Point : X = " + w.ToString() + " ,Y = " + we.ToString(), "Setting", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result != DialogResult.OK)
                return;

            this._selectedCalcData[0].RemoveAt(nearestIndex);
            this._selectedCalcData[1].RemoveAt(nearestIndex);

            UpdateLinearRegressAndCalc();
            UpdateLinearRegressDgv();
        }

        private void btnCalcLineRegress_Click(object sender, EventArgs e)
        {
            if (this.cmbCalcItem.SelectedIndex == -1 || DataCenter._fileCompare.CompareTable.Rows.Count==0)
                return;

            string selectedItem= this.cmbCalcItem.SelectedItem.ToString();
            string selectedItemStd = "S_" + selectedItem;
            int xColIndex = -1;
            int yColIndex = -1;

			string keyStr = string.Empty;

            for (int i = 1; i < DataCenter._fileCompare.CompareTable.Columns.Count; i++)
            {
				keyStr = DataCenter._fileCompare.CompareTable.Columns[i].Caption.ToString();
				if (keyStr == (selectedItemStd))
                {
                    yColIndex = i;
                    xColIndex = i + 1;
                    break;
                }
            }

			if (xColIndex == -1 || yColIndex == -1)
				return;

            List<double> msrtX = new List<double>(DataCenter._fileCompare.CompareTable.Rows.Count);
            List<double> referenceY = new List<double>(DataCenter._fileCompare.CompareTable.Rows.Count);

            for (int row = 0; row < DataCenter._fileCompare.CompareTable.Rows.Count; row++)
            {
                msrtX.Add (Convert.ToDouble(DataCenter._fileCompare.CompareTable.Rows[row][xColIndex].ToString()));
                referenceY.Add(Convert.ToDouble(DataCenter._fileCompare.CompareTable.Rows[row][yColIndex].ToString()));
            }

            this.lblLinearResult.Visible = true;
            this._selectedCalcData[0] = msrtX;
            this._selectedCalcData[1] = referenceY;

            this.UpdateLinearRegressAndCalc();
            this.UpdateLinearRegressDgv();
        }

        private void btnCombineLinearResult_Click(object sender, EventArgs e)
        {
			if (this.dgvLinearRegress.CurrentCell == null)
				return;

            DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show("確定合併此產品(" + DataCenter._uiSetting.ProductFileName + ")的Gain/Offset修正係數？ ", "Combine ?", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
            if (result != DialogResult.OK)
                return;

			double A = 0.0d;
			double B = 0.0d;
			double C = 0.0d;
			double D = 0.0d;

			//====================================================================
			//	y = A * x + B , z = C * y + D , z = (A * C)x + C * B + D
			//  z = Slop * x + Intercept , Slop = A * C , Intercept = C * B + D
			//====================================================================
            foreach (KeyValuePair<string, GainOffset> kvp in this._linearCalcResultData)
            {
                if (this._linearCalcResultData.ContainsKey(kvp.Key) == false)
                    return;
                
                for (int i = 0; i < this.dgvGainOffset.Rows.Count; i++)
                {
                    if (this.dgvGainOffset.Rows[i].Cells[1].Value.ToString() == kvp.Key) 
                    {
						A = Convert.ToDouble(this.dgvGainOffset.Rows[i].Cells[5].Value);
						B = Convert.ToDouble(this.dgvGainOffset.Rows[i].Cells[6].Value);

						C = Convert.ToDouble(this._linearCalcResultData[kvp.Key].Gain);
						D = Convert.ToDouble(this._linearCalcResultData[kvp.Key].Offset);

						this.dgvGainOffset.Rows[i].Cells[5].Value = A * C;		// Slop , Gain
						this.dgvGainOffset.Rows[i].Cells[6].Value = C * B + D;	// Intecept , Offset	

						this._linearCalcResultData[kvp.Key].Gain = 1.000d;
						this._linearCalcResultData[kvp.Key].Offset = 0.000d;

						//this.dgvLinearRegress.Rows[ii].Cells[5].Value = 1;
						//this.dgvLinearRegress.Rows[ii].Cells[6].Value = 0;					
						
						break;
                    }
                }
            }

			//this.dgvGainOffset.Update();
			//this.UpdateCalcGainOffsetDGV();    
        
			this.UpdateLinearRegressDgv();
        }

		private void frmCaliCoeff_Shown(object sender, EventArgs e)
		{
			tabmMain.SelectedTab = this.tabiFilter;
		}

		private void frmCaliCoeff_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.txtStdPathAndFile.Text = "";
			this.txtMsrtPathAndFile.Text = "";
			this.btnRefreshCoef.Enabled = false;
			this.btnSaveCoefTableToSys.Enabled = false;
			this.ClearChart();
			this.Hide();

			DataCenter.SaveToolsConfig();
			DataCenter._fileCompare.ClearData();
			Host.UpdateDataToAllUIForm();
		}

		private void tabmMain_SelectedTabChanged(object sender, DevComponents.DotNetBar.SuperTabStripSelectedTabChangedEventArgs e)
		{
			this.UpdateDataToControls();
		}

		private void cmbExtendWLMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (this.cmbExtendWLMode.SelectedIndex)
			{
				case 0:
					numExtendWLPoint.Enabled = true;
					dinExtWLStart.Enabled = false;
					dinExtWLEnd.Enabled = false;
					break;
				case 1:
					numExtendWLPoint.Enabled = false;
					dinExtWLStart.Enabled = true;
					dinExtWLEnd.Enabled = true;
					break;
				default:
					numExtendWLPoint.Enabled = true;
					dinExtWLStart.Enabled = true;
					dinExtWLEnd.Enabled = true;
					break;

			}
		}

		private void rdCompareByRowCol_CheckedChanged(object sender, EventArgs e)
		{
			if (this.rdCompareByRowCol.Checked)
			{
				DataCenter._toolConfig.IsArrangeByRowCol = true;
			}
			else if (this.rdCompareDataBySeq.Checked)
			{
				DataCenter._toolConfig.IsArrangeByRowCol = false;
			}
			DataCenter.SaveToolsConfig();
		}

		private void rdCompareDataBySeq_CheckedChanged(object sender, EventArgs e)
		{
			if (this.rdCompareByRowCol.Checked)
			{
				DataCenter._toolConfig.IsArrangeByRowCol = true;
			}
			else if (this.rdCompareDataBySeq.Checked)
			{
				DataCenter._toolConfig.IsArrangeByRowCol = false;
			}
			DataCenter.SaveToolsConfig();
		}

		private void numCalCount_ValueChanged(object sender, EventArgs e)
		{
			if (this.numCalCount.Value > this.dgvCompData.RowCount)
			{
				int count = this.numCalCount.Value - this.dgvCompData.RowCount;

				for (int row = 0; row < count; row++)
				{
					Dictionary<string, string> data = new Dictionary<string, string>();

					for (int col = 0; col < this.dgvCompData.ColumnCount; col++)
					{
						string keyName = this.dgvCompData.Columns[col].Name;

						data.Add(keyName, "0");
					}

					this.AddDataToCompareDate(data);
				}
			}
			else if (this.numCalCount.Value < this.dgvCompData.RowCount)
			{
				int count = this.dgvCompData.RowCount - this.numCalCount.Value;

				for (int row = 0; row < count; row++)
				{
					this.DeleteCompareDate(this.dgvCompData.RowCount - 1);
				}
			}
		}

		private void dgvCompData_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
			double data = 0.0d;

			if (!double.TryParse(e.FormattedValue as string, out data))
			{
				DevComponents.DotNetBar.MessageBoxEx.Show("Data Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

				e.Cancel = true;
			}
		}

		private void dgvCompData_CellValidated(object sender, DataGridViewCellEventArgs e)
		{
			double data = double.Parse(this.dgvCompData[e.ColumnIndex, e.RowIndex].Value as string);

			this.dgvCompData[e.ColumnIndex, e.RowIndex].Value = data.ToString();
		}

		private void btnCompare_Click(object sender, EventArgs e)
		{
			//Write CalSTD File
			string stdPathAndFile = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, "CalSTD.ctmp");

			this.SavedgvCompDataToFile(stdPathAndFile);

			//Write CalMst File
			string mstPathAndFile = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, "CalMst.ctmp");

			this.SavedgvMsrtDataToFile(mstPathAndFile);

			//Save Calibration Chip Std and Msrt Value
			this.SaveCaliChipValue();

			//Clibration
			this._stdFileName = stdPathAndFile;

			this._msrtFileName = mstPathAndFile;

			DataCenter._fileCompare.IsMultiFileMode = false;

			DataCenter.SaveToolsConfig();

			DataCenter._toolConfig.IsArrangeByRowCol = false;

			this.rdCompareByRowCol.Checked = false;

			this.rdCompareDataBySeq.Checked = true;

			this.btnCompareCalcGainOffset.PerformClick();

			this.tabmMain.SelectedTab = this.tabiCalcGainOffset;
		}

		private void btnTest_Click(object sender, EventArgs e)
		{
			UILog.Log(this, sender, "btnTest_Click");

			AppSystem.ResetDataList();

			AppSystem.ManualRun(1, 1, DataCenter._sysSetting.DieRepeatTestDelay);

			foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
			{
				if (item.IsEnable != true || item.MsrtResult == null)
				{
					continue;
				}

				foreach (TestResultData resultData in item.MsrtResult)
				{
					if (!resultData.IsVision || !resultData.IsEnable)
					{
						continue;
					}

					string keyName = string.Empty;

					foreach (var titleName in DataCenter._fileCompare.TitleName)
					{
						if (resultData.Name == titleName.Value)
						{
							keyName = titleName.Key;
						}
					}

					for (int col = 0; col < this.dgvMsrtData.ColumnCount; col++)
					{
						if (keyName == this.dgvMsrtData.Columns[col].Name)
						{
							int row = this.dgvMsrtData.CurrentCell.RowIndex;

							string data = resultData.RawValue.ToString(resultData.Formate);

							this.dgvMsrtData[col, row].Value = data;

							break;
						}
					}

				}
			}
		}

		private void btnImportStdFile_Click(object sender, EventArgs e)
		{
			 this._openFileDialog.InitialDirectory = DataCenter._toolConfig.StdFileDir;
			this._openFileDialog.FileName = "";
			this._openFileDialog.FilterIndex = 1;
			this._openFileDialog.Multiselect = false;

			if (this._openFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			//Get titleStr
			string titleStr = string.Empty;

			List<string> keyNameList = new List<string>();

			foreach (var item in DataCenter._fileCompare.TitleName)
			{
				titleStr += item.Value;

				keyNameList.Add(item.Key);
			}

			List<string[]> report = CSVUtil.ReadCSV(this._openFileDialog.FileName);

			//Find the Tital
			while (report.Count > 0)
			{
				string compareStr = string.Empty;

				foreach (var item in report[0])
				{
					compareStr += item;
				}

				if (compareStr != titleStr)
				{
					report.RemoveAt(0);
				}
				else
				{
					break;
				}
			}

			if (report.Count < 2)
			{
				Host.SetErrorCode(EErrorCode.Tools_LoadStdDataFail);

				return;
			}

			//Check Data
			for (int row = 1; row < report.Count; row++)
			{
				for (int col = 0; col < report[row].Length; col++)
				{
					double value = 0.0d;

					if (report[row][col] != "" && !double.TryParse(report[row][col], out value))
					{
						Host.SetErrorCode(EErrorCode.Tools_LoadStdDataFail);

						return;
					}
				}
			}

			//Update CompareDateDGV
			this.ResetCompareDateDGV();

			for (int row = 1; row < report.Count; row++)
			{
				Dictionary<string, string> data = new Dictionary<string, string>();

				for (int col = 0; col < report[row].Length; col++)
				{
					string keyName = keyNameList[col];

					data.Add(keyName, report[row][col]);
				}

				this.AddDataToCompareDate(data);
			}

			this.numCalCount.Value = this.dgvCompData.RowCount;
		}

		private void btnExportStdFile_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();

			saveFileDialog.InitialDirectory = DataCenter._toolConfig.StdFileDir;
			saveFileDialog.FileName = "";
			saveFileDialog.FilterIndex = 1;
			saveFileDialog.Title = "Export Std. Data to File";
			saveFileDialog.Filter = "CSV files (*.csv)|*.csv";

			if (saveFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			this.SavedgvCompDataToFile(saveFileDialog.FileName);
		}

		private void btnExportMstFile_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();

			saveFileDialog.InitialDirectory = DataCenter._toolConfig.StdFileDir;
			saveFileDialog.FileName = "";
			saveFileDialog.FilterIndex = 1;
			saveFileDialog.Title = "Export Msrt. Data to File";
			saveFileDialog.Filter = "CSV files (*.csv)|*.csv";

			if (saveFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			this.SavedgvMsrtDataToFile(saveFileDialog.FileName);
		}

		private void btnClearSTDData_Click(object sender, EventArgs e)
		{
			DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show("確認清除資料 ？", "Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

			if (result != DialogResult.OK)
			{
				return;
			}

			this.dgvCompData.SuspendLayout();

			for (int col = 1; col < this.dgvCompData.ColumnCount; col++)
			{
				for (int row = 0; row < this.dgvCompData.RowCount; row++)
				{
					this.dgvCompData[col, row].Value = "0";
				}
			}

			this.dgvCompData.ResumeLayout();
		}

		private void btnClearMstData_Click(object sender, EventArgs e)
		{
			DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show("確認清除資料 ？", "Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

			if (result != DialogResult.OK)
			{
				return;
			}

			this.dgvMsrtData.SuspendLayout();

			for (int col = 1; col < this.dgvMsrtData.ColumnCount; col++)
			{
				for (int row = 0; row < this.dgvMsrtData.RowCount; row++)
				{
					this.dgvMsrtData[col, row].Value = "0";
				}
			}

			this.dgvMsrtData.ResumeLayout();
		}

		#endregion

        private void cmbDisplayTable_SelectedIndexChanged(object sender, EventArgs e)
        {
           // this._lastSelectTableIndex = this.cmbDisplayTable.SelectedIndex;
        }

        private void btnSaveChannelData_Click(object sender, EventArgs e)
        {

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

            DataCenter.SaveProductFile();
        }

        private void btnCombineAllParamToTable_Click(object sender, EventArgs e)
        {
            DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show("Combine Calc Coef into By Channel Gain/Offset？ ", "Combine ?", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);

            if (result != DialogResult.OK)
                return;

            this.CombineChannelGainOffset();
        }

        private void CombineChannelGainOffset()
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

        private void UpdateChannelData()
        {
            if (DataCenter._product.TestCondition.ChannelConditionTable == null)
            {
                return;
            }

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
								string name = string.Empty;

								EGainOffsetType type = EGainOffsetType.None;

								double square = 0.0d;

								double gain = 1.0d;

								double offset = 1.0d;

								string keyName = string.Empty;

								if (DataCenter._machineConfig.TesterFunctionType == ETesterFunctionType.Single_Die ||
                                    DataCenter._machineConfig.TesterFunctionType == ETesterFunctionType.Multi_Terminal)
								{
									name = data.Name;

									square = data.Square;

									gain = data.Gain;

									offset = data.Offset;

									keyName = data.KeyName;
								}
								else
								{
									GainOffsetData coef = conditionTable.Channels[channel].GetByChannelGainOffsetData(data.KeyName);

									name = coef.Name;

									square = coef.Square;

									gain = coef.Gain;

									offset = coef.Offset;

									keyName = coef.KeyName;
								}

                                this.dgvSysCoef.Rows.Add();

                                this.dgvSysCoef.Rows[rowCount].Cells[0].Value = (rowCount + 1).ToString();

                                if ((item is LOPWLTestItem) && data.KeyName.IndexOf("_") > 0)
                                {
                                    gainKeyName = data.KeyName.Remove(data.KeyName.IndexOf("_"));

                                    switch (gainKeyName)
                                    {
                                        case "LOP":        // EOptiMsrtType.LOP :
											this.dgvSysCoef.Rows[rowCount].Cells[1].Value = name + " (mcd)";
                                            break;
                                        //------------------------------------------------
                                        case "WATT":       // EOptiMsrtType.WATT
											this.dgvSysCoef.Rows[rowCount].Cells[1].Value = name + " (mW)";
                                            break;
                                        //------------------------------------------------
                                        case "LM":         // EOptiMsrtType.LM
											this.dgvSysCoef.Rows[rowCount].Cells[1].Value = name + " (lm)";
                                            break;
                                        //------------------------------------------------
                                        default:
											this.dgvSysCoef.Rows[rowCount].Cells[1].Value = name;
                                            break;
                                    }
                                }
                                else
                                {
                                    this.dgvSysCoef.Rows[rowCount].Cells[1].Value = name;
                                }

                                this.dgvSysCoef.Rows[rowCount].Cells[2].Value = (channel + 1).ToString();
                                this.dgvSysCoef.Rows[rowCount].Cells[3].Value = type;
                                this.dgvSysCoef.Rows[rowCount].Cells[4].Value = square;
                                this.dgvSysCoef.Rows[rowCount].Cells[5].Value = gain;
                                this.dgvSysCoef.Rows[rowCount].Cells[6].Value = offset;
                                this.dgvSysCoef.Rows[rowCount].Cells[7].Value = keyName;

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

	}
}