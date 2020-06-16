using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using MPI.Windows.Forms;
using MPI.Tester.Maths;
using MPI.Tester.Data;
using MPI.Tester.TestKernel;
using ZedGraph;

namespace MPI.Tester.Gui
{
	public partial class frmTestResultAnalyze : System.Windows.Forms.Form
	{
		private PointPairList PointListA;
		private PointPairList PointListB;

		private delegate void UpdateDataHandler();
		private delegate void UpdateChartDataHandler();

		public frmTestResultAnalyze()
		{
			Console.WriteLine("[frmTestResultAnalyze], frmTestResultAnalyze()");

			InitializeComponent();
			
			PointListA = new PointPairList();
			PointListB = new PointPairList();

			this.InitParamAndCompData();
		}

		#region >>> Private Methods <<<

		private void IncludeStatusForm()
		{
			frmTestResultInstantInfo form = ( frmTestResultInstantInfo ) FormAgent.RetrieveForm( typeof( frmTestResultInstantInfo ) );
			form.TopLevel = false;
		}

		private void InitParamAndCompData()
		{
			this.cmbChartADataSource.Items.AddRange(Enum.GetNames(typeof(EDisplayAxisSelection)));
			this.cmbChartADataSource.SelectedIndex = 0;
			this.cmbChartBDataSource.Items.AddRange(Enum.GetNames(typeof(EDisplayAxisSelection)));
			this.cmbChartBDataSource.SelectedIndex = 0;
		}
		
		private void ChangeDataToChart(GraphPane graphPane, int itemIndex, int dispAxis)
		{
			EDisplayAxisSelection select = (EDisplayAxisSelection)dispAxis;

			if ( itemIndex == DataCenter._conditionCtrl.SweepItemNames[0].Length )
			{
                graphPane.XAxis.Title.Text = select.ToString().Remove(select.ToString().IndexOf("_"));
                graphPane.YAxis.Title.Text = select.ToString().Substring(select.ToString().IndexOf("_") + 1).Trim();
			}
			else
			{
				string keyName = DataCenter._conditionCtrl.SweepItemNames[0][itemIndex];
                this.ChangeAxisLabel(graphPane, keyName, select);
			}

			this.DrawChartGraph();
		}

		private PointPairList GenerateCurve(uint channel, string keyName, EDisplayAxisSelection dispAxis )
		{
			PointPairList pointPair = new PointPairList();

			pointPair.Clear();

            if (AppSystem._outputBigData.OutputSweepList.Count == 0)
            {
                return pointPair;
            }

            int index = AppSystem._outputBigData.OutputSweepList.Count;

            OutputSweepData data = AppSystem._outputBigData.OutputSweepList[index - 1];

            if (data.DataSet[channel, keyName] == null)
            {
                return pointPair;
            }

            if (data.DataSet[channel, keyName].TimeChain == null || data.DataSet[channel, keyName].ApplyData == null)
				return pointPair;			

			switch (dispAxis)
			{
				case EDisplayAxisSelection.In_Out:
                    pointPair.Add(data.DataSet[channel, keyName].ApplyData, data.DataSet[channel, keyName].SweepData);
					break;
				case EDisplayAxisSelection.Out_In:
                    pointPair.Add(data.DataSet[channel, keyName].SweepData, data.DataSet[channel, keyName].ApplyData);
					break;
				case EDisplayAxisSelection.Time_In:
                    pointPair.Add(data.DataSet[channel, keyName].TimeChain, data.DataSet[channel, keyName].ApplyData);
					break;
				case EDisplayAxisSelection.Time_Out:
                    pointPair.Add(data.DataSet[channel, keyName].TimeChain, data.DataSet[channel, keyName].SweepData);
					break;
				default:
					break;
			}
			return pointPair;
		}
		
        private void ChangeAxisLabel(GraphPane graphPane, string keyName, EDisplayAxisSelection dispAxis)
		{
			string key = keyName.Remove(keyName.IndexOf("_"));
            AxisLabel XAxisLabel = null;
            AxisLabel YAxisLabel = null;
			
			switch ( key )
			{
				case "THY" :
					{
						if (dispAxis == EDisplayAxisSelection.In_Out)
						{
							XAxisLabel = new AxisLabel("mA", this.Font.ToString(), 16, Color.Blue, false, false, false);
							YAxisLabel = new AxisLabel("V", this.Font.ToString(), 16, Color.Blue, false, false, false);

						}
						else if (dispAxis == EDisplayAxisSelection.Out_In)
						{
							XAxisLabel = new AxisLabel("V", this.Font.ToString(), 16, Color.Blue, false, false, false);
							YAxisLabel = new AxisLabel("mA", this.Font.ToString(), 16, Color.Blue, false, false, false);
						}
						else if (dispAxis == EDisplayAxisSelection.Time_In)
						{
							XAxisLabel = new AxisLabel("ms", this.Font.ToString(), 16, Color.Blue, false, false, false);
							YAxisLabel = new AxisLabel("mA", this.Font.ToString(), 16, Color.Blue, false, false, false);
						}
						else if (dispAxis == EDisplayAxisSelection.Time_Out)
						{
							XAxisLabel = new AxisLabel("ms", this.Font.ToString(), 16, Color.Blue, false, false, false);
							YAxisLabel = new AxisLabel("V", this.Font.ToString(), 16, Color.Blue, false, false, false);
						}
					}
					break;
				//------------------------------------------------------------------------------------------------------------------------
				case "IVSWEEP":
                case "PIV":
					{
						if ( dispAxis == EDisplayAxisSelection.In_Out )
						{
                            XAxisLabel = new AxisLabel("mA", this.Font.ToString(), 16, Color.Blue, false, false, false);
                            YAxisLabel = new AxisLabel("V", this.Font.ToString(), 16, Color.Blue, false, false, false);

						}
						else if (  dispAxis == EDisplayAxisSelection.Out_In )
						{
                            XAxisLabel = new AxisLabel("V", this.Font.ToString(), 16, Color.Blue, false, false, false);
                            YAxisLabel = new AxisLabel("mA", this.Font.ToString(), 16, Color.Blue, false, false, false);
						}
						else if (  dispAxis == EDisplayAxisSelection.Time_In )
						{
                            XAxisLabel = new AxisLabel("ms", this.Font.ToString(), 16, Color.Blue, false, false, false);
                            YAxisLabel = new AxisLabel("mA", this.Font.ToString(), 16, Color.Blue, false, false, false);				
						}
						else if (  dispAxis == EDisplayAxisSelection.Time_Out )
						{
                            XAxisLabel = new AxisLabel("ms", this.Font.ToString(), 16, Color.Blue, false, false, false);
                            YAxisLabel = new AxisLabel("V", this.Font.ToString(), 16, Color.Blue, false, false, false);				
						}
					}
					break;
				//------------------------------------------------------------------------------------------------------------------------
				case "VISWEEP":
					{
						if ( dispAxis == EDisplayAxisSelection.In_Out )
						{
                            XAxisLabel = new AxisLabel("V", this.Font.ToString(), 16, Color.Blue, false, false, false);
                            YAxisLabel = new AxisLabel("mA", this.Font.ToString(), 16, Color.Blue, false, false, false);
						}
						else if (  dispAxis == EDisplayAxisSelection.Out_In )
						{
                            XAxisLabel = new AxisLabel("mA", this.Font.ToString(), 16, Color.Blue, false, false, false);
                            YAxisLabel = new AxisLabel("V", this.Font.ToString(), 16, Color.Blue, false, false, false);
						}
						else if (  dispAxis == EDisplayAxisSelection.Time_In )
						{
                            XAxisLabel = new AxisLabel("ms", this.Font.ToString(), 16, Color.Blue, false, false, false);
                            YAxisLabel = new AxisLabel("V", this.Font.ToString(), 16, Color.Blue, false, false, false);				
						}
						else if (  dispAxis == EDisplayAxisSelection.Time_Out )
						{
                            XAxisLabel = new AxisLabel("ms", this.Font.ToString(), 16, Color.Blue, false, false, false);
                            YAxisLabel = new AxisLabel("mA", this.Font.ToString(), 16, Color.Blue, false, false, false);				
						}
					}
					break;
                //------------------------------------------------------------------------------------------------------------------------
                case "RTH":
                    {
                        if (dispAxis == EDisplayAxisSelection.In_Out)
                        {
                            XAxisLabel = new AxisLabel("mA", this.Font.ToString(), 16, Color.Blue, false, false, false);
                            YAxisLabel = new AxisLabel("V", this.Font.ToString(), 16, Color.Blue, false, false, false);

                        }
                        else if (dispAxis == EDisplayAxisSelection.Out_In)
                        {
                            XAxisLabel = new AxisLabel("V", this.Font.ToString(), 16, Color.Blue, false, false, false);
                            YAxisLabel = new AxisLabel("mA", this.Font.ToString(), 16, Color.Blue, false, false, false);
                        }
                        else if (dispAxis == EDisplayAxisSelection.Time_In)
                        {
                            XAxisLabel = new AxisLabel("ms", this.Font.ToString(), 16, Color.Blue, false, false, false);
                            YAxisLabel = new AxisLabel("mA", this.Font.ToString(), 16, Color.Blue, false, false, false);
                        }
                        else if (dispAxis == EDisplayAxisSelection.Time_Out)
                        {
                            XAxisLabel = new AxisLabel("ms", this.Font.ToString(), 16, Color.Blue, false, false, false);
                            YAxisLabel = new AxisLabel("V", this.Font.ToString(), 16, Color.Blue, false, false, false);
                        }
                    }
                    break;
				//------------------------------------------------------------------------------------------------------------------------
                case "VLR":
                    {
                        if (dispAxis == EDisplayAxisSelection.In_Out)
                        {
                            XAxisLabel = new AxisLabel("mA", this.Font.ToString(), 16, Color.Blue, false, false, false);
                            YAxisLabel = new AxisLabel("V", this.Font.ToString(), 16, Color.Blue, false, false, false);

                        }
                        else if (dispAxis == EDisplayAxisSelection.Out_In)
                        {
                            XAxisLabel = new AxisLabel("V", this.Font.ToString(), 16, Color.Blue, false, false, false);
                            YAxisLabel = new AxisLabel("mA", this.Font.ToString(), 16, Color.Blue, false, false, false);
                        }
                        else if (dispAxis == EDisplayAxisSelection.Time_In)
                        {
                            XAxisLabel = new AxisLabel("ms", this.Font.ToString(), 16, Color.Blue, false, false, false);
                            YAxisLabel = new AxisLabel("mA", this.Font.ToString(), 16, Color.Blue, false, false, false);
                        }
                        else if (dispAxis == EDisplayAxisSelection.Time_Out)
                        {
                            XAxisLabel = new AxisLabel("ms", this.Font.ToString(), 16, Color.Blue, false, false, false);
                            YAxisLabel = new AxisLabel("V", this.Font.ToString(), 16, Color.Blue, false, false, false);
                        }
                    }
                    break;
                //------------------------------------------------------------------------------------------------------------------------
                case "VISCAN":
                    {
                        if (dispAxis == EDisplayAxisSelection.In_Out)
                        {
                            XAxisLabel = new AxisLabel("V", this.Font.ToString(), 16, Color.Blue, false, false, false);
                            YAxisLabel = new AxisLabel("A", this.Font.ToString(), 16, Color.Blue, false, false, false);

                        }
                        else if (dispAxis == EDisplayAxisSelection.Out_In)
                        {
                            XAxisLabel = new AxisLabel("uA", this.Font.ToString(), 16, Color.Blue, false, false, false);
                            YAxisLabel = new AxisLabel("V", this.Font.ToString(), 16, Color.Blue, false, false, false);
                        }
                        else if (dispAxis == EDisplayAxisSelection.Time_In)
                        {
                            XAxisLabel = new AxisLabel("ms", this.Font.ToString(), 16, Color.Blue, false, false, false);
                            YAxisLabel = new AxisLabel("V", this.Font.ToString(), 16, Color.Blue, false, false, false);
                        }
                        else if (dispAxis == EDisplayAxisSelection.Time_Out)
                        {
                            XAxisLabel = new AxisLabel("ms", this.Font.ToString(), 16, Color.Blue, false, false, false);
                            YAxisLabel = new AxisLabel("A", this.Font.ToString(), 16, Color.Blue, false, false, false);
                        }
                    }
                    break;
                //------------------------------------------------------------------------------------------------------------------------
				default:
					break;
			}

            graphPane.XAxis.Title = XAxisLabel;
            graphPane.YAxis.Title = YAxisLabel;
		}
		
		private void DrawChartGraph()
		{
            if (DataCenter._product.TestCondition.TestItemArray == null)
            {
                return;
            }

			int colorR = 0;
			int colorG = 0;
			int colorB = 0;
			double hue = 0.0d;

            uint channel = 0;
			string keyName = string.Empty;
			string Name = string.Empty;
			LineItem myCurve;

			if (this.cmbChartAItems.Items.Count == 0)
               return;

			if (this.cmbChartAItems.SelectedIndex < 0)
				return;

			if (this.gChartA.GraphPane.CurveList.Count != 0)
				this.gChartA.GraphPane.CurveList.Clear();

			EDisplayAxisSelection select = (EDisplayAxisSelection)this.cmbChartADataSource.SelectedIndex;

            channel = (uint)this.numChartAChannel.Value - 1; // UI display base-1

			if (this.cmbChartAItems.SelectedItem.ToString() != "ALL")
			{
				keyName = DataCenter._conditionCtrl.SweepItemNames[0][this.cmbChartAItems.SelectedIndex];
				Name = DataCenter._conditionCtrl.SweepItemNames[1][this.cmbChartAItems.SelectedIndex];
                myCurve = this.gChartA.GraphPane.AddCurve(Name, this.GenerateCurve(channel, keyName, select), Color.Red, SymbolType.None);

				foreach (TestItemData dd in DataCenter._product.TestCondition.TestItemArray)
				{
					if (dd.Type == ETestType.THY && dd.KeyName == keyName && select == EDisplayAxisSelection.Out_In )
					{
						PointPairList pointPair = new PointPairList();
						pointPair.Clear();

						pointPair.Add(dd.MsrtResult[0].Value, 5.0d);
						pointPair.Add(dd.MsrtResult[2].Value, 5.0d);

						myCurve = this.gChartB.GraphPane.AddCurve("VV", pointPair, Color.Blue, SymbolType.XCross);
					}
				}
			}
			else
			{

                Color[] colors = new Color[5] { Color.Red, Color.Orange, Color.GreenYellow, Color.Blue, Color.DeepPink };

				for (int i = 0; i < (this.cmbChartAItems.Items.Count - 1); i++)
				{
					if (i == DataCenter._conditionCtrl.SweepItemNames[0].Length)
						break;

					keyName = DataCenter._conditionCtrl.SweepItemNames[0][i];
					Name = DataCenter._conditionCtrl.SweepItemNames[1][i];
					hue = (double)(i % 10 ) / 10.0d;

					Maths.ColorHSL.HSL2RGB(hue, 0.99, 0.5, out colorR, out colorG, out colorB);
					//this.HSL2RGB( hue , 0.99, 0.5, out colorR, out colorG, out colorB);
					//myCurve = this.gChartA.GraphPane.AddCurve(Name, this.GenerateCurve(keyName, select), Color.FromArgb(colorR, colorG, colorB), SymbolType.None);
#if  ( DebugVer )
					myCurve = this.gChartA.GraphPane.AddCurve(Name, this.GenerateCurve(keyName, select), Color.FromArgb(colorR, colorG, colorB), (SymbolType)( i % 9));
#else
                    myCurve = this.gChartA.GraphPane.AddCurve(Name, this.GenerateCurve(channel, keyName, select), colors[i], SymbolType.None);
#endif
				}
			
			}
			this.gChartA.AxisChange();
			this.gChartA.Refresh();

			//---------------------------------------------------------------------------------
         if (this.cmbChartBItems.Items.Count == 0)
                return;

			if ( this.cmbChartBItems.SelectedIndex < 0 )
				return;

			if (this.gChartB.GraphPane.CurveList.Count != 0)
				this.gChartB.GraphPane.CurveList.Clear();

			select = (EDisplayAxisSelection)this.cmbChartBDataSource.SelectedIndex;

            channel = (uint)this.numChartBChannel.Value - 1;

			if (this.cmbChartBItems.SelectedItem.ToString() != "ALL")
			{
				keyName = DataCenter._conditionCtrl.SweepItemNames[0][this.cmbChartBItems.SelectedIndex];
				Name = DataCenter._conditionCtrl.SweepItemNames[1][this.cmbChartBItems.SelectedIndex];
                myCurve = this.gChartB.GraphPane.AddCurve(Name, this.GenerateCurve(channel, keyName, select), Color.Red, SymbolType.None);

				foreach (TestItemData dd in DataCenter._product.TestCondition.TestItemArray)
				{
					if (dd.Type == ETestType.THY && dd.KeyName == keyName && select == EDisplayAxisSelection.Out_In) 
					{
						PointPairList pointPair = new PointPairList();
						pointPair.Clear();
						
						pointPair.Add(dd.MsrtResult[0].Value, 5.0d);
						pointPair.Add(dd.MsrtResult[1].Value, 5.0d);
						myCurve = this.gChartB.GraphPane.AddCurve(Name, pointPair, Color.Blue, SymbolType.XCross);
					}
				}
			}
			else
			{
				for (int i = 0; i < (this.cmbChartBItems.Items.Count - 1); i++)
				{
					if (i == DataCenter._conditionCtrl.SweepItemNames[0].Length)
						break;

					keyName = DataCenter._conditionCtrl.SweepItemNames[0][i];
					Name = DataCenter._conditionCtrl.SweepItemNames[1][i];
					hue = (double)(i % 20) / 10.0d;

					Maths.ColorHSL.HSL2RGB(hue, 0.99, 0.5, out colorR, out colorG, out colorB);
					//this.HSL2RGB(hue, 0.99, 0.5, out colorR, out colorG, out colorB);
					//myCurve = this.gChartB.GraphPane.AddCurve(Name, this.GenerateCurve(keyName, select), Color.FromArgb(colorR, colorG, colorB), SymbolType.None);					
#if  ( DebugVer )
					myCurve = this.gChartB.GraphPane.AddCurve(Name, this.GenerateCurve(keyName, select), Color.FromArgb(colorR, colorG, colorB), (SymbolType)(i % 9));
#else
                    myCurve = this.gChartB.GraphPane.AddCurve(Name, this.GenerateCurve(channel, keyName, select), Color.FromArgb(colorR, colorG, colorB), SymbolType.None);
#endif
				}
			}
			this.gChartB.AxisChange();
			this.gChartB.Refresh();
		}

		private void UpdateChartDataToControls()
		{
			this.DrawChartGraph();			// Run at Main Thread ( UI Thread )

           
		}

		private void InitChart()
		{
			this.gChartA.GraphPane.Title.FontSpec.FontColor = Color.Red;
			this.gChartB.GraphPane.Title.FontSpec.FontColor = Color.Red;

			this.gChartA.GraphPane.Title.Text = "Chart  A";
			this.gChartB.GraphPane.Title.Text = "Chart  B";

			AxisLabel XAxisLabel = new AxisLabel("Input", this.Font.ToString(), 16, Color.Blue, false, false, false);
			AxisLabel YAxisLabel = new AxisLabel("Output", this.Font.ToString(), 16, Color.Blue, false, false, false);

			//Axis Label Property Adjust
			XAxisLabel.FontSpec.Fill = new Fill(Color.FromArgb(220, 220, 255));
			YAxisLabel.FontSpec.Fill = new Fill(Color.FromArgb(220, 220, 255));
			XAxisLabel.FontSpec.Border = new Border(false, Color.Empty, 0);
			YAxisLabel.FontSpec.Border = new Border(false, Color.Empty, 0);
			YAxisLabel.FontSpec.Angle = 180;

			this.gChartA.GraphPane.XAxis.Title = XAxisLabel;
			this.gChartA.GraphPane.YAxis.Title = YAxisLabel;
			//this.gChartA.GraphPane.Fill = new Fill(Color.FromArgb(220, 220, 255));

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
			this.gChartB.GraphPane.XAxis.Title = XAxisLabel;
			this.gChartB.GraphPane.YAxis.Title = YAxisLabel;
			//this.gChartB.GraphPane.Fill = new Fill(Color.FromArgb(220, 220, 255));

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
		
		private void UpdateDataToControls()
		{
			this.cmbChartAItems.Items.Clear();
			this.cmbChartBItems.Items.Clear();

			this.cmbChartAItems.BeginUpdate();
			this.cmbChartBItems.BeginUpdate();

			this.cmbChartAItems.Items.AddRange(DataCenter._conditionCtrl.SweepItemNames[1]);
			this.cmbChartBItems.Items.AddRange(DataCenter._conditionCtrl.SweepItemNames[1]);

			if (this.cmbChartAItems.Items.Count > 0)
			{
				this.cmbChartAItems.Items.Add("ALL");
				this.cmbChartBItems.Items.Add("ALL");
			}
			this.cmbChartAItems.EndUpdate();
			this.cmbChartBItems.EndUpdate();

			if (this.cmbChartAItems.Items.Count > 0 )			// && this.cmbChartAItems.SelectedIndex < 0)
			{
				this.cmbChartAItems.SelectedIndex = 0;		// reset ChartA index = 0 and ChartB index = 0 at the same time
				this.cmbChartBItems.SelectedIndex = 0;		// reset ChartA index = 0 and ChartB index = 0 at the same time
			}

            switch (DataCenter._uiSetting.UIOperateMode)
            {
                case (int)EUIOperateMode.Idle:
                    this.btnChartSave01.Enabled = true;
                    break;
                //-----------------------------------------------------------------------------
                case (int)EUIOperateMode.AutoRun:
                case (int)EUIOperateMode.ManulRun:
                    this.btnChartSave01.Enabled = false;
                    break;
                //-----------------------------------------------------------------------------
                //case (int)EUIOperateMode.ManulRun:
                //    this.btnChartSave01.Enabled = true;
                //    break;
                //-----------------------------------------------------------------------------
                default:
                    this.btnChartSave01.Enabled = true;
                    break;
            }
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

			this.ChangeDataToChart(this.gChartA.GraphPane, this.cmbChartAItems.SelectedIndex, this.cmbChartADataSource.SelectedIndex);
		}

		private void cmbChartBDataSource_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.cmbChartBItems.SelectedIndex < 0 || this.cmbChartBDataSource.SelectedIndex < 0)
				return;

			this.ChangeDataToChart(this.gChartB.GraphPane, this.cmbChartBItems.SelectedIndex, this.cmbChartBDataSource.SelectedIndex);
		}

		private void cmbChartAItems_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.cmbChartAItems.SelectedIndex < 0 || this.cmbChartADataSource.SelectedIndex < 0)
				return;

			this.ChangeDataToChart(this.gChartA.GraphPane, this.cmbChartAItems.SelectedIndex, this.cmbChartADataSource.SelectedIndex);
		}

		private void cmbChartBItems_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.cmbChartBItems.SelectedIndex < 0 || this.cmbChartBDataSource.SelectedIndex < 0)
				return;

			this.ChangeDataToChart(this.gChartB.GraphPane, this.cmbChartBItems.SelectedIndex, this.cmbChartBDataSource.SelectedIndex);
		}

		private void btnChartSave01_Click(object sender, EventArgs e)
		{
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Export Sweep Data to File";
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog.FilterIndex = 1;    // default value = 1
            saveFileDialog.InitialDirectory = Constants.Paths.MPI_TEMP_DIR;

            saveFileDialog.FileName = DataCenter._uiSetting.TaskSheetFileName;

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            string path = Path.GetDirectoryName(saveFileDialog.FileName);

            string fileName = Path.GetFileName(saveFileDialog.FileName);

            AppSystem._outputBigData.SaveSweepData(path, fileName, AppSystem._outputBigData.OutputSweepList.Count - 1, 1);        
		}

		#endregion
	}
}