using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using MPI.Windows.Forms;
using MPI.Tester.Data;
using MPI.Tester.TestKernel;
using ZedGraph;

namespace MPI.Tester.Gui
{
	public partial class frmTestResultSpectrum : System.Windows.Forms.Form
	{
		private PointPairList PointList;
		private delegate void UpdateDataHandler();
        private delegate void UpdateChartDataHandler();

		public frmTestResultSpectrum()
		{
			Console.WriteLine("[frmTestResultSpectrum], frmTestResultSpectrum()");

			InitializeComponent();

			PointList = new PointPairList();
			this.InitParamAndCompData();
		}

		private void InitParamAndCompData()
		{
		}

		private void frmTestResult_Load( object sender, EventArgs e )
		{
			this.UpdateDataToControls();

			InitialChart( this.gSptCount);
			InitialChart(this.gSptIntensity);
			this.includeStatusForm();
		}

		private void includeStatusForm()
		{
			frmTestResultInstantInfo form = ( frmTestResultInstantInfo ) FormAgent.RetrieveForm( typeof( frmTestResultInstantInfo ) );
			form.TopLevel = false;
		}

		//private void drawSpectrumGraph()
		//{
		//    if (this.cmbSpectroGraph.SelectedIndex < 0)
		//        return;

		//    string keyName = DataCenter._conditionCtrl.OpticalItemNames[0][this.cmbSpectroGraph.SelectedIndex];
		//    string Name = DataCenter._conditionCtrl.OpticalItemNames[1][this.cmbSpectroGraph.SelectedIndex];

		//    if (gSptCount.GraphPane.CurveList.Count != 0)
		//        gSptCount.GraphPane.CurveList.Clear();

		//    PointList.Clear();

		//    if (DataCenter._acquireData[keyName] == null)
		//        return;

		//    if (DataCenter._acquireData[keyName][0] == null || DataCenter._acquireData[keyName][1] == null)
		//        return;


		//    for (int p = 0; p < DataCenter._acquireData[keyName][0].Length; p++)
		//    {
		//        //if (DataCenter._acquireData[keyName][0][p] > 360.0d && DataCenter._acquireData[keyName][0][p] < 800.0d)
		//        //{
		//            PointList.Add(DataCenter._acquireData[keyName][0][p], DataCenter._acquireData[keyName][1][p]);
		//        //}
		//    }

		//    LineItem myCurve = gSptCount.GraphPane.AddCurve(Name, PointList, Color.Yellow, SymbolType.None);
		//    gSptCount.AxisChange();
		//}

		private void DrawSpectrumGraph()
		{
			if (this.cmbSpectroGraph.SelectedIndex < 0)
				return;

			string keyName = DataCenter._conditionCtrl.OpticalItemNames[0][this.cmbSpectroGraph.SelectedIndex];
			string Name = DataCenter._conditionCtrl.OpticalItemNames[1][this.cmbSpectroGraph.SelectedIndex];

            uint channel = (uint)this.numSpectroChannel.Value - 1;

            if (DataCenter._acquireData.SpectrumDataSet[channel, keyName] == null)
				return;

            if (DataCenter._acquireData.SpectrumDataSet[channel, keyName].Wavelength == null || DataCenter._acquireData.SpectrumDataSet[channel, keyName].Intensity == null || DataCenter._acquireData.SpectrumDataSet[channel, keyName].Absoluate == null)
				return;

			if (tabcSpectrum.SelectedTab.Text == tabpCount.Text)
			{
				//PlotGraph.SetXYAxis(this.gSptCount, this.dinCountMinX.Value, this.dinCountMaxX.Value, 0.0d, this.dinCountMaxY.Value);
                PlotGraph.DrawPlotRealTimeUpdate(this.gSptCount, DataCenter._acquireData.SpectrumDataSet[channel, keyName].Wavelength, DataCenter._acquireData.SpectrumDataSet[channel, keyName].Intensity,
												true, 2.0F, Color.Blue, SymbolType.None, true, true, false, "None");
			}
			else if ( tabcSpectrum.SelectedTab.Text == tabpAbsoluteIntensity.Text )
			{
				//PlotGraph.SetXYAxis(this.gSptIntensity, this.dinCountMinX.Value, this.dinCountMaxX.Value, 0.0d, this.dinCountMaxY.Value);
                PlotGraph.DrawPlotRealTimeUpdate(this.gSptIntensity, DataCenter._acquireData.SpectrumDataSet[channel, keyName].Wavelength, DataCenter._acquireData.SpectrumDataSet[channel, keyName].Absoluate,
												true, 2.0F, Color.DeepPink, SymbolType.None, true, true, false, "None");
			}
		}


		private void InitialChart( ZedGraphControl zgc  )
		{
			PlotGraph.SetLabel(this.gSptCount, "Spectrum", "Wave Length (nm)", "Relative Intensity", 16);
			PlotGraph.SetLabel(this.gSptIntensity, "Spectrum", "Wave Length (nm)", "Absolute Intensity", 16);
			PlotGraph.SetGrid(this.gSptCount, true, Color.LightGray, Color.White);
			PlotGraph.SetGrid(this.gSptIntensity, true, Color.LightGray, Color.White);

			//zgc.GraphPane.Title.FontSpec.FontColor = Color.Red;
			//zgc.GraphPane.Title.Text = "Spectrum";

			//AxisLabel XAxisLabel = new AxisLabel( "Wave Length (nm)", this.Font.ToString(), 16, Color.Blue, false, false, false );
			//AxisLabel YAxisLabel = new AxisLabel( "Relative Intensity", this.Font.ToString(), 16, Color.Blue, false, false, false );

			//PlotGraph.SetLabel(zgc, "Spectrum", "Wave Length (nm)", "Relative Intensity", 16);

			////Axis Label Property Adjust
			//XAxisLabel.FontSpec.Fill = new Fill( Color.FromArgb( 220, 220, 255 ) );
			//YAxisLabel.FontSpec.Fill = new Fill( Color.FromArgb( 220, 220, 255 ) );
			//XAxisLabel.FontSpec.Border = new Border( false, Color.Empty, 0 );
			//YAxisLabel.FontSpec.Border = new Border( false, Color.Empty, 0 );
			//YAxisLabel.FontSpec.Angle = 180;

			//zgc.GraphPane.XAxis.Title = XAxisLabel;
			//zgc.GraphPane.YAxis.Title = YAxisLabel;
			//zgc.GraphPane.Fill = new Fill( Color.FromArgb( 220, 220, 255 ) );

			//zgc.GraphPane.XAxis.IsAxisSegmentVisible = true;
			//zgc.GraphPane.XAxis.MajorGrid.IsVisible = true;
			//zgc.GraphPane.XAxis.MajorTic.Color = Color.LightGray;
			//zgc.GraphPane.XAxis.MinorTic.Color = Color.LightGray;
			//zgc.GraphPane.XAxis.MajorGrid.Color = Color.LightGray;

			//zgc.GraphPane.YAxis.IsAxisSegmentVisible = true;
			//zgc.GraphPane.YAxis.MajorGrid.IsVisible = true;
			//zgc.GraphPane.YAxis.MajorTic.Color = Color.LightGray;
			//zgc.GraphPane.YAxis.MinorTic.Color = Color.LightGray;
			//zgc.GraphPane.YAxis.MajorGrid.Color = Color.LightGray;

			////myPane.XAxis.Color = Color.Red;
			//zgc.GraphPane.Chart.Fill = new Fill( Color.Black );

		}

		private void frmTestResultSpectrum_VisibleChanged(object sender, EventArgs e)
		{
			if (this.Visible == false)
				return;

			Form form = FormAgent.RetrieveForm(typeof(frmTestResultInstantInfo));
			form.Parent = this;
			form.Dock = DockStyle.Bottom;
			form.Show();
			form.BringToFront();
		}

		private void UpdateDataToControls()
		{
			this.cmbSpectroGraph.Items.Clear();
			this.cmbSpectroGraph.BeginUpdate();
			this.cmbSpectroGraph.Items.AddRange( DataCenter._conditionCtrl.OpticalItemNames[1] );

			if (this.cmbSpectroGraph.Items.Count > 0)
			{
				this.cmbSpectroGraph.SelectedIndex = 0;
			}
			
			this.cmbSpectroGraph.EndUpdate();
		}

        private void UpdateChartDataToControls()
        {
            this.DrawSpectrumGraph();			// Run at Main Thread ( UI Thread )
			//this.drawSpectrumGraph();
        }

		//------------------------------------------------------------------------------------------------------------------------------------------
		// When parameters of condition setting is changed, it will fire event
		// and call this UpdateDataToUIForm() function. The form just updates the measurement items in listBox
		//------------------------------------------------------------------------------------------------------------------------------------------
		public void UpdateDataToUIForm()
		{
			if (this.InvokeRequired && this.IsHandleCreated)
			{
				this.BeginInvoke(new UpdateDataHandler(UpdateDataToControls), null);		// Run at other TestServer Thread
			}
			else if (this.IsHandleCreated)
			{
				this.UpdateDataToControls();			//  Run at Main Thread
			}
		}

        public void UpdateChartDataToUIForm(object sender, EventArgs e)
        {

            if (this.InvokeRequired && this.IsHandleCreated)
            {
                this.BeginInvoke(new UpdateChartDataHandler(UpdateChartDataToControls), null);		// Run at other TestServer Thread
            }
            else if (this.IsHandleCreated)
            {
                this.UpdateChartDataToControls();			// Run at Main Thread
            }
        }

		private void dinIntensityMinX_ValueChanged(object sender, EventArgs e)
		{
			if (this.dinIntensityMinX.Value > this.dinIntensityMaxX.Value)
				return;

			PlotGraph.SetXYAxis(this.gSptIntensity, this.dinIntensityMinX.Value, this.dinIntensityMaxX.Value, 0.0d, this.dinIntensityMaxY.Value);
		}

		private void dinIntensityMaxX_ValueChanged(object sender, EventArgs e)
		{
			if (this.dinIntensityMinX.Value > this.dinIntensityMaxX.Value)
				return;

			PlotGraph.SetXYAxis(this.gSptIntensity, this.dinIntensityMinX.Value, this.dinIntensityMaxX.Value, 0.0d, this.dinIntensityMaxY.Value);
		}

		private void dinIntensityMaxY_ValueChanged(object sender, EventArgs e)
		{
			PlotGraph.SetXYAxis(this.gSptIntensity, this.dinIntensityMinX.Value, this.dinIntensityMaxX.Value, 0.0d, this.dinIntensityMaxY.Value);
		}

		private void dinCountMinX_ValueChanged(object sender, EventArgs e)
		{
			if (this.dinCountMinX.Value > this.dinCountMaxX.Value)
				return;

			PlotGraph.SetXYAxis(this.gSptCount, this.dinCountMinX.Value, this.dinCountMaxX.Value, 0.0d, this.dinCountMaxY.Value);
		}

		private void dinCountMaxX_ValueChanged(object sender, EventArgs e)
		{
			if (this.dinCountMinX.Value > this.dinCountMaxX.Value)
				return;

			PlotGraph.SetXYAxis(this.gSptCount, this.dinCountMinX.Value, this.dinCountMaxX.Value, 0.0d, this.dinCountMaxY.Value);
		}

		private void dinCountMaxY_ValueChanged(object sender, EventArgs e)
		{
			PlotGraph.SetXYAxis(this.gSptCount, this.dinCountMinX.Value, this.dinCountMaxX.Value, 0.0d, this.dinCountMaxY.Value);
		}

		private void gSptCount_ZoomEvent(ZedGraphControl sender, ZoomState oldState, ZoomState newState)
		{
			this.dinCountMinX.Value =  this.gSptCount.GraphPane.XAxis.Scale.Min;
			this.dinCountMaxX.Value = this.gSptCount.GraphPane.XAxis.Scale.Max;
            this.dinCountMaxY.Value = this.gSptCount.GraphPane.YAxis.Scale.Max;                   
		}

		private void gSptIntensity_ZoomEvent(ZedGraphControl sender, ZoomState oldState, ZoomState newState)
		{
			this.dinIntensityMinX.Value = this.gSptIntensity.GraphPane.XAxis.Scale.Min;
			this.dinIntensityMaxX.Value = this.gSptIntensity.GraphPane.XAxis.Scale.Max;
			this.dinIntensityMaxY.Value = this.gSptIntensity.GraphPane.YAxis.Scale.Max;

		}

	}
}