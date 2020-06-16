using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Linq;

using MPI.Windows.Forms;
using MPI.Tester.Maths;
using MPI.Tester.Data;
using MPI.Tester.TestKernel;
using ZedGraph;

namespace MPI.Tester.Gui
{
	public partial class frmTestResultAnalyze2 : System.Windows.Forms.Form
	{
    	private delegate void UpdateDataHandler();

		private delegate void UpdateChartDataHandler();

		public frmTestResultAnalyze2()
		{
			Console.WriteLine("[frmTestResultAnalyze2], frmTestResultAnalyze2()");

			InitializeComponent();
			
			this.InitParamAndCompData();
		}

		#region >>> Private Methods <<<

		private void IncludeStatusForm()
		{
            //frmTestResultInstantInfo form = ( frmTestResultInstantInfo ) FormAgent.RetrieveForm( typeof( frmTestResultInstantInfo ) );
            //form.TopLevel = false;
		}

		private void InitParamAndCompData()
		{
			this.cmbChartADataSource.Items.AddRange(Enum.GetNames(typeof(EDisplayAxisSelection)));
			this.cmbChartADataSource.SelectedIndex = 0;
			this.cmbChartBDataSource.Items.AddRange(Enum.GetNames(typeof(EDisplayAxisSelection)));
			this.cmbChartBDataSource.SelectedIndex = 0;
		}
		
        private string ParseResultKeyNameToName(string KeyName)
        {
            string rtnName = KeyName;

            if (DataCenter._uiSetting.UserDefinedData.MsrtDisplayItemDic.ContainsKey(KeyName))
            {
                rtnName = DataCenter._uiSetting.UserDefinedData.MsrtDisplayItemDic[KeyName];
            }

            return rtnName;
        }

        private string ParseResultNameToKeyName(string name)
        {
            string rtnName = name;

            foreach (var item in DataCenter._uiSetting.UserDefinedData.MsrtDisplayItemDic)
            {
                if (item.Value == name)
                {
                    return item.Key;
                }
            }

            return rtnName;
        }

        private void SetChartAxisName(ZedGraphControl z, string xkeyName, string yKeyName)
        {
            string xAxisName = ParseResultKeyNameToName(xkeyName);

            string yAxisName = ParseResultKeyNameToName(yKeyName);

            PlotGraph.SetLabel(z, "LIV", xAxisName, yAxisName, 16.0f);
        }

        private void ChangeDataToChart(ZedGraphControl z, string xkeyName, string ykeyName)
        {
            if (AppSystem._outputBigData.OutputLIVList.Count == 0)
            {
                return;
            }

            PlotGraph.Clear(z);

            string itemKeyName = this.cmbLIVItem.SelectedItem.ToString();

            int index = AppSystem._outputBigData.OutputLIVList.Count;

            OutputLIVData data = AppSystem._outputBigData.OutputLIVList[index - 1];

            if (data.DataSet[itemKeyName][xkeyName] != null && data.DataSet[itemKeyName][ykeyName] != null)
            {
                List<float> xdata = data.DataSet[itemKeyName][xkeyName].DataArray.ToList();

                List<float> ydata = data.DataSet[itemKeyName][ykeyName].DataArray.ToList();

                double[] xAxisArray = new double[xdata.Count];

                double[] yAxisArray = new double[xdata.Count];

                for (int i = 0; i < xdata.Count; i++)
                {
                    xAxisArray[i] = Math.Round(xdata[i],4);

                    yAxisArray[i] = Math.Round(ydata[i], 4);
                }

                PlotGraph.DrawPlot(z, xAxisArray, yAxisArray, true, 1.5F, Color.Blue, SymbolType.Circle, true, true, false, "LIV");
            }
        }

        private void ChangeDataToChartByName(ZedGraphControl z, string xname,string yname)
        {
            string xkeyName = ParseResultNameToKeyName(xname);

            string ykeyName = ParseResultNameToKeyName(yname);

            ChangeDataToChart(z, xkeyName, ykeyName);      
        }

		private void UpdateChartDataToControls()
		{
            if (!this.Visible)
            {
                return;
            }

            DrawAchart();

            DrawBchart();
		}

        private void DrawAchart()
        {
            if (this.cmbChartAItems.SelectedIndex < 0 || this.cmbChartADataSource.SelectedIndex < 0)
                return;

            if (this.cmbChartABaseItems.SelectedIndex < 0 || this.cmbChartABaseItems.SelectedIndex < 0)
                return;

            this.ChangeDataToChart(this.gChartA, DataCenter._uiSetting.LIVDrawSetting.ChartA_xAxisName, DataCenter._uiSetting.LIVDrawSetting.ChartA_yAxisName);
        }

        private void DrawBchart()
        {
            if (this.cmbChartBItems.SelectedIndex < 0 || this.cmbChartBItems.SelectedIndex < 0)
                return;

            if (this.cmbChartABaseItems.SelectedIndex < 0 || this.cmbChartABaseItems.SelectedIndex < 0)
                return;

            this.ChangeDataToChart(this.gChartB, DataCenter._uiSetting.LIVDrawSetting.ChartB_xAxisName, DataCenter._uiSetting.LIVDrawSetting.ChartB_yAxisName);
        }

		private void InitChart()
		{
			this.gChartA.GraphPane.Title.FontSpec.FontColor = Color.Red;
			this.gChartB.GraphPane.Title.FontSpec.FontColor = Color.Red;

			this.gChartA.GraphPane.Title.Text = "Chart  A";
			this.gChartB.GraphPane.Title.Text = "Chart  B";

			AxisLabel XAxisLabel = new AxisLabel("Input", this.Font.ToString(), 16, Color.Blue, false, false, false);
			AxisLabel YAxisLabel = new AxisLabel("Output", this.Font.ToString(), 16, Color.Blue, false, false, false);

            AxisLabel XAxisLabelB = new AxisLabel("", this.Font.ToString(), 16, Color.Blue, false, false, false);
            AxisLabel YAxisLabelB = new AxisLabel("", this.Font.ToString(), 16, Color.Blue, false, false, false);


			//Axis Label Property Adjust
			XAxisLabel.FontSpec.Fill = new Fill(Color.FromArgb(220, 220, 255));
			YAxisLabel.FontSpec.Fill = new Fill(Color.FromArgb(220, 220, 255));
			XAxisLabel.FontSpec.Border = new Border(false, Color.Empty, 0);
			YAxisLabel.FontSpec.Border = new Border(false, Color.Empty, 0);
			YAxisLabel.FontSpec.Angle = 180;

			this.gChartA.GraphPane.XAxis.Title = XAxisLabel;
			this.gChartA.GraphPane.YAxis.Title = YAxisLabel;
			this.gChartA.GraphPane.Fill = new Fill(Color.FromArgb(220, 220, 255));

			this.gChartA.GraphPane.XAxis.IsAxisSegmentVisible = true;
			this.gChartA.GraphPane.XAxis.MajorGrid.IsVisible = true;
			this.gChartA.GraphPane.XAxis.MajorTic.Color = Color.Gray;
			this.gChartA.GraphPane.XAxis.MinorTic.Color = Color.Gray;
			this.gChartA.GraphPane.XAxis.MajorGrid.Color = Color.Gray;

			this.gChartA.GraphPane.YAxis.IsAxisSegmentVisible = true;
			this.gChartA.GraphPane.YAxis.MajorGrid.IsVisible = true;
			this.gChartA.GraphPane.YAxis.MajorTic.Color = Color.Gray;
			this.gChartA.GraphPane.YAxis.MinorTic.Color = Color.Gray;
			this.gChartA.GraphPane.YAxis.MajorGrid.Color = Color.Gray;

			this.gChartA.GraphPane.Chart.Fill = new Fill(Color.White);

			//---------------------------------------------------------------------------

            XAxisLabelB.FontSpec.Fill = new Fill(Color.FromArgb(220, 220, 255));
            YAxisLabelB.FontSpec.Fill = new Fill(Color.FromArgb(220, 220, 255));
            XAxisLabelB.FontSpec.Border = new Border(false, Color.Empty, 0);
            YAxisLabelB.FontSpec.Border = new Border(false, Color.Empty, 0);
            YAxisLabelB.FontSpec.Angle = 180;

			this.gChartB.GraphPane.XAxis.Title = XAxisLabelB;
			this.gChartB.GraphPane.YAxis.Title = YAxisLabelB;
			this.gChartB.GraphPane.Fill = new Fill(Color.FromArgb(220, 220, 255));

			this.gChartB.GraphPane.XAxis.IsAxisSegmentVisible = true;
			this.gChartB.GraphPane.XAxis.MajorGrid.IsVisible = true;
			this.gChartB.GraphPane.XAxis.MajorTic.Color = Color.Gray;
			this.gChartB.GraphPane.XAxis.MinorTic.Color = Color.Gray;
			this.gChartB.GraphPane.XAxis.MajorGrid.Color = Color.Gray;

			this.gChartB.GraphPane.YAxis.IsAxisSegmentVisible = true;
			this.gChartB.GraphPane.YAxis.MajorGrid.IsVisible = true;
			this.gChartB.GraphPane.YAxis.MajorTic.Color = Color.Gray;
			this.gChartB.GraphPane.YAxis.MinorTic.Color = Color.Gray;
			this.gChartB.GraphPane.YAxis.MajorGrid.Color = Color.Gray;

			this.gChartB.GraphPane.Chart.Fill = new Fill(Color.White);

            PlotGraph.SetGrid(this.gChartA, false, Color.Silver, Color.White);


		}

        private void UpdateDataToCombox()
        {
            this.Enabled = false;

            if (DataCenter._product.TestCondition.TestItemArray == null)
            {
                return;
            }

            this.cmbChartAItems.Items.Clear();

            this.cmbChartABaseItems.Items.Clear();

            this.cmbChartBItems.Items.Clear();

            this.cmbLIVItem.Items.Clear();

            List<string> itemName=new List<string>();
         
            List<string> itemBaseName=new List<string>();

            List<string> LivTesItems = new List<string>();

            foreach (TestItemData item in  DataCenter._product.TestCondition.TestItemArray)
            {
                if(item is LIVTestItem)
                {
                    LivTesItems.Add(item.KeyName);

                      if (item.MsrtResult == null)
                       continue;

                      this.Enabled = true;

                    foreach (TestResultData data in item.MsrtResult)
                    {
                        if (data.KeyName.Contains(ELIVOptiMsrtType.LIVSETVALUE.ToString()) ||
                            data.KeyName.Contains(ELIVOptiMsrtType.LIVTIMEB.ToString()) ||
                            data.KeyName.Contains(ELIVOptiMsrtType.LIVWATT.ToString()) ||
                            data.KeyName.Contains(ELIVOptiMsrtType.LIVLOP.ToString()) ||
                            data.KeyName.Contains(ELIVOptiMsrtType.LIVWLD.ToString()) ||
                            data.KeyName.Contains(ELIVOptiMsrtType.LIVWLCP.ToString()))
                        {
                            itemName.Add(data.Name);                         
                        }

                        if (data.KeyName.Contains(ELIVOptiMsrtType.LIVSETVALUE.ToString()) ||
                            data.KeyName.Contains(ELIVOptiMsrtType.LIVTIMEB.ToString()))
                        {
                            itemBaseName.Add(data.Name);
                        }
                    }
                }
            }

            this.cmbChartABaseItems.Items.AddRange(itemBaseName.ToArray());

            this.cmbChartAItems.Items.AddRange(itemName.ToArray());

            this.cmbChartBItems.Items.AddRange(itemName.ToArray());

            this.cmbLIVItem.Items.AddRange(LivTesItems.ToArray());

            if (this.cmbLIVItem.Items.Count > 0)
            {
                this.cmbLIVItem.SelectedIndex = 0;
            }
        }

        private void UpdateDataToControls()
		{
            this.UpdateDataToCombox();

            this.UpdateLIVSettingData();

            if (DataCenter._uiSetting.LIVDrawSetting.ChartA_xAxisName != string.Empty)
            {
                string xAxisName = ParseResultKeyNameToName(DataCenter._uiSetting.LIVDrawSetting.ChartA_xAxisName);

                if (this.cmbChartABaseItems.Items.Contains(xAxisName))
                {
                    this.cmbChartABaseItems.SelectedItem = xAxisName;
                }
            }

            if (DataCenter._uiSetting.LIVDrawSetting.ChartA_yAxisName != string.Empty)
            {
                string yAxisName = ParseResultKeyNameToName(DataCenter._uiSetting.LIVDrawSetting.ChartA_yAxisName);

                if (this.cmbChartAItems.Items.Contains(yAxisName))
                {
                    this.cmbChartAItems.SelectedItem = yAxisName;
                }
            }

            this.SetChartAxisName(this.gChartA, DataCenter._uiSetting.LIVDrawSetting.ChartA_xAxisName, DataCenter._uiSetting.LIVDrawSetting.ChartA_yAxisName);


            if (DataCenter._uiSetting.LIVDrawSetting.ChartB_yAxisName != string.Empty)
            {
                string yAxisName2 = ParseResultKeyNameToName(DataCenter._uiSetting.LIVDrawSetting.ChartB_yAxisName);

                if (this.cmbChartBItems.Items.Contains(yAxisName2))
                {
                    this.cmbChartBItems.SelectedItem = yAxisName2;
                }
            }

            this.SetChartAxisName(this.gChartB, DataCenter._uiSetting.LIVDrawSetting.ChartA_xAxisName, DataCenter._uiSetting.LIVDrawSetting.ChartB_yAxisName);


		}

#endregion

		#region >>> Public Methods <<<

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
				this.UpdateDataToControls();		// Run at Main Thread
			}
		}
		
		public void UpdateChartDataToUIForm(object sender, EventArgs e)
		{
			//// (A) coding style
			//this.Invoke(new UpdateChartDataHandler(UpdateChartDataToControls), null);	// Run at Thread of the Caller


			//// (B) coding style
			//if (this.InvokeRequired && this.IsHandleCreated )
			//{
			//   this.Invoke((MethodInvoker)delegate
			//   {
			//      this.UpdateChartDataToControls();		// Run at Main Thread <== Why ?????
			//   });
			//}
			//else if ( this.IsHandleCreated )
			//{
			//   this.UpdateChartDataToControls();			// Run at Main Thread
			//}

			// (C) coding style
			if (this.InvokeRequired && this.IsHandleCreated)
			{
				this.BeginInvoke(new UpdateChartDataHandler(UpdateChartDataToControls), null);		// Run at other TestServer Thread
			}
			else if (this.IsHandleCreated)
			{
				this.UpdateChartDataToControls();			// Run at Main Thread
			}

			//// (D) coding style
			//MethodInvoker method = delegate { this.UpdateChartDataToControls(); };
			//if (this.InvokeRequired && this.IsHandleCreated)
			//{
			//   this.BeginInvoke(method);				// Run at other TestServer Thread
			//}
			//else if (this.IsHandleCreated)
			//{
			//   method.Invoke();							// Run at Main Thread
			//}

		}

		#endregion

		#region >>> UI Event Handler <<<

		private void frmTestResultAnalyze_Load(object sender, EventArgs e)
		{
			// (1)
			this.InitChart();

			// (2)
			this.UpdateDataToControls();
			//this.IncludeStatusForm();
		}

		private void frmTestResultAnalyze_VisibleChanged(object sender, EventArgs e)
		{
			if (this.Visible == false)
				return;

			Form form = FormAgent.RetrieveForm(typeof(frmTestResultInstantInfo));
			form.Parent = this;
			form.Dock = DockStyle.Bottom;
			form.Show();
			form.BringToFront();
		}

		private void cmbChartADataSource_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.cmbChartAItems.SelectedIndex < 0 || this.cmbChartADataSource.SelectedIndex < 0)
				return;

		//	this.ChangeDataToChart(this.gChartA.GraphPane, this.cmbChartAItems.SelectedIndex, this.cmbChartADataSource.SelectedIndex);
		}

		private void cmbChartBDataSource_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.cmbChartBItems.SelectedIndex < 0 || this.cmbChartBDataSource.SelectedIndex < 0)
				return;

			//this.ChangeDataToChart(this.gChartB.GraphPane, this.cmbChartBItems.SelectedIndex, this.cmbChartBDataSource.SelectedIndex);
		}

		private void cmbChartAItems_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.cmbChartAItems.SelectedIndex < 0 || this.cmbChartADataSource.SelectedIndex < 0)
				return;

            if (this.cmbChartABaseItems.SelectedIndex < 0 || this.cmbChartABaseItems.SelectedIndex < 0)
				return;

            this.SetChartAxisName(this.gChartA, this.cmbChartABaseItems.SelectedItem.ToString(), this.cmbChartAItems.SelectedItem.ToString());

            this.ChangeDataToChartByName(this.gChartA, this.cmbChartABaseItems.SelectedItem.ToString(), this.cmbChartAItems.SelectedItem.ToString());
		}

		private void cmbChartBItems_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.cmbChartBItems.SelectedIndex < 0 || this.cmbChartBDataSource.SelectedIndex < 0)
				return;

            this.SetChartAxisName(this.gChartB, this.cmbChartABaseItems.SelectedItem.ToString(), this.cmbChartBItems.SelectedItem.ToString());

            this.ChangeDataToChartByName(this.gChartB, this.cmbChartABaseItems.SelectedItem.ToString(), this.cmbChartBItems.SelectedItem.ToString());
			//this.ChangeDataToChart(this.gChartB.GraphPane, this.cmbChartBItems.SelectedIndex, this.cmbChartBDataSource.SelectedIndex);
		}

        private void cmbChartABaseItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbChartAItems.SelectedIndex < 0 || this.cmbChartADataSource.SelectedIndex < 0)
                return;

            if (this.cmbChartABaseItems.SelectedIndex < 0 || this.cmbChartABaseItems.SelectedIndex < 0)
                return;

            this.SetChartAxisName(this.gChartA, this.cmbChartABaseItems.SelectedItem.ToString(), this.cmbChartAItems.SelectedItem.ToString());

            this.ChangeDataToChartByName(this.gChartA, this.cmbChartABaseItems.SelectedItem.ToString(), this.cmbChartAItems.SelectedItem.ToString());


            if (this.cmbChartAItems.SelectedIndex < 0 || this.cmbChartAItems.SelectedIndex < 0)
                return;

            this.SetChartAxisName(this.gChartB, this.cmbChartABaseItems.SelectedItem.ToString(), this.cmbChartBItems.SelectedItem.ToString());

            this.ChangeDataToChartByName(this.gChartB, this.cmbChartABaseItems.SelectedItem.ToString(), this.cmbChartBItems.SelectedItem.ToString());

        }

        private void btnChartSave01_Click_1(object sender, EventArgs e)
        {
            if (cmbChartABaseItems.SelectedIndex <0 || this.cmbChartAItems.SelectedIndex<0)
            {
                return;
            }

           string xKeyName = this.ParseResultNameToKeyName(this.cmbChartABaseItems.SelectedItem.ToString());

           string yKeyName = this.ParseResultNameToKeyName(this.cmbChartAItems.SelectedItem.ToString());

           string yKeyName2 = this.ParseResultNameToKeyName(this.cmbChartBItems.SelectedItem.ToString());

           DataCenter._uiSetting.LIVDrawSetting.ChartA_yAxisName = yKeyName;

           DataCenter._uiSetting.LIVDrawSetting.ChartA_xAxisName = xKeyName;

           DataCenter._uiSetting.LIVDrawSetting.ChartB_xAxisName = xKeyName;

           DataCenter._uiSetting.LIVDrawSetting.ChartB_yAxisName = yKeyName2;

            DataCenter.SaveUISettingToFile();

            this.UpdateDataToControls();
        }

        private void btnExportToCSV_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Export LIV Data to File";
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog.FilterIndex = 1;    // default value = 1
            saveFileDialog.InitialDirectory = Constants.Paths.MPI_TEMP_DIR;

            saveFileDialog.FileName = DataCenter._uiSetting.TaskSheetFileName;

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;


            string path = Path.GetDirectoryName(saveFileDialog.FileName);
            string fileName = Path.GetFileName(saveFileDialog.FileName);

            AppSystem._outputBigData.SaveLIVData(path, fileName, AppSystem._outputBigData.OutputLIVList.Count - 1, 1);
        }

        private void btnEditLIV_Click(object sender, EventArgs e)
        {
            int editIndex = -1;

            for(int i = 0 ;i<DataCenter._product.TestCondition.TestItemArray.Length;i++)
            {
                if(DataCenter._product.TestCondition.TestItemArray[i] is LIVTestItem)
                {
                    if (DataCenter._product.TestCondition.TestItemArray[i].KeyName == this.cmbLIVItem.SelectedItem.ToString())
                    {
                        editIndex = i;
                        break;
                    }
                }
            }

            if (editIndex >= 0)
            {
                FormAgent.ConditionItemSetting.DialogControl(EBtnActionMode.UpdateTestItem, editIndex);
            }  
        }

        private void UpdateLIVSettingData()
        {
            if (this.cmbLIVItem.SelectedIndex < 0)
            {
                return;
            }

            foreach (TestItemData item in DataCenter._product.TestCondition.TestItemArray)
            {
                if (item is LIVTestItem)
                {
                    if (item.KeyName != this.cmbLIVItem.SelectedItem.ToString())
                    {
                        continue;
                    }

                    LIVTestItem livItem = (item as LIVTestItem);

                    this.dinForceDealyLIV.Value = livItem.LIVForceDelayTime;

                    this.dinForceTimeLIV.Value = livItem.LIVForceTime;

                    this.dinTurnOffTimeLIV.Value = livItem.LIVTurnOffTime;

                    this.dinStartValueLIV.Value = livItem.LIVStartValue;

                    this.dinStepValueLIV.Value = livItem.LIVStepValue;

                    this.numRiseCountLIV.Value = (int)livItem.LIVSweepPoints;

                    this.dinMsrtRangeLIV.Value = livItem.LIVMsrtRange;

                    this.lblMaxValueLIV.Text = livItem.LIVStopValue.ToString();

                    return;
                }
            }
        }

        private void cmbLIVItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateLIVSettingData();
        }

        //private void btnChartSave01_Click(object sender, EventArgs e)
        //{
        //    //if (this.txtChartFileName01.Text == "")
        //    //{
        //    //    Host.SetErrorCode(EErrorCode.NoSaveFileName;
        //    //    return;
        //    //}

        //    if (DataCenter._conditionCtrl.SweepItemNames == null)
        //        return;

        //    if (this.cmbChartADataSource.SelectedIndex <= -1)
        //        return;

        //    if (this.cmbChartAItems.SelectedIndex >= DataCenter._conditionCtrl.SweepItemNames[0].Length)
        //        return;

        //    //string fileNameWithExt = Path.Combine( DataCenter._uiSetting.TestResultPath01 , this.txtChartFileName01.Text );
			
        //    // Write "Title" to file
        //    string[][] title = new string[][] { new string[] { "SweepData", "" },
        //                                                                new string[] { "Time(ms)", "Current(mA)", "Voltage(V)", "Reserved" } };
            
        //    title[0][1] = this.cmbChartAItems.SelectedItem.ToString();

        //    if (MPI.Tester.CSVUtil.WriteToCSV(fileNameWithExt, false, title) == false)
        //    {
        //        Host.SetErrorCode(EErrorCode.SaveFileFail;
        //        return;
        //    }

        //    string keyName = DataCenter._conditionCtrl.SweepItemNames[0][this.cmbChartAItems.SelectedIndex];
        //    // Write "data" to file

        //    double[][] data = new double [DataCenter._acquireData[keyName].Length ][];
        //    for( int i = 0; i < data.Length; i++)
        //    {
        //        if (DataCenter._acquireData[keyName][i] != null)
        //        {
        //            data[i] = new double[DataCenter._acquireData[keyName][i].Length];
        //            Array.Copy(DataCenter._acquireData[keyName][i], data[i], DataCenter._acquireData[keyName][i].Length);
        //        }
        //        else
        //        {
        //            data[i] = null;
        //        }

        //        if ( data[i] != null )
        //        {
        //            for ( int j = 0; j < data[i].Length; j++ )
        //            {
        //                if ( i ==0 )
        //                {
        //                    data[i][j] = Math.Round(data[i][j],2,MidpointRounding.AwayFromZero);
        //                }
        //                else if ( i == 1 )
        //                {
        //                    data[i][j] = Math.Round(data[i][j],4,MidpointRounding.AwayFromZero);
        //                }
        //                else if ( i== 2 )
        //                {
        //                    data[i][j] = Math.Round(data[i][j],4,MidpointRounding.AwayFromZero);
        //                }
        //                else
        //                {
        //                    ;
        //                }                   
        //            }
        //        }
        //    }


        //    if ( MPI.Tester.CSVUtil.WriteToCSV(fileNameWithExt,true, data ,true) == false )
        //    {
        //        Host.SetErrorCode(EErrorCode.SaveFileFail;
        //        return;
        //    }                                                                   
        //}

		#endregion
	}
}