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

using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;
using ZedGraph;
using MPI.Windows.Forms;


namespace MPI.Tester.Gui
{
    public partial class frmSetParameter : Form
    {
		private double[] _caliSpectrum;
        private double[] _darkArray;

        public frmSetParameter()
        {
            InitializeComponent();
        }

        private void frmSetParameter_Load(object sender, EventArgs e)
        {
            InitUIControl();
            UpdateDataToControls();      
        }

        private void InitUIControl()
        {
			PlotGraph.SetGrid(this.zedCalibration, false, Color.Silver, Color.Transparent);
			PlotGraph.SetLabel(this.zedCalibration, "Calibration nW/cm2", "Pixel", "nJ/cm2", 16);
            PlotGraph.SetGrid(this.zedDarkArray, false, Color.Silver, Color.Transparent);
            PlotGraph.SetLabel(this.zedDarkArray, "Dark Intensity", "Pixel", "Count", 16);
            LoadCaliFile();
        }

        private bool LoadCaliFile()
        {
            string CaliFilePath = @"C:\MPI\LEDTester\Spectrometer";
            string pathAndFile = Path.Combine(CaliFilePath, "CaliData_" + DataCenter._machineInfo.SpectrometerSN + ".xml");


            
            if (File.Exists(pathAndFile))
            {
                SpectroCaliData spectroCaliData = MPI.Xml.XmlFileSerializer.Deserialize(typeof(SpectroCaliData), pathAndFile) as SpectroCaliData;
                PlotGraph.DrawPlot(this.zedCalibration,spectroCaliData.CaliSpectrumArray[0], true, 2.0F, Color.Blue, SymbolType.None, false, true, "Attenuator_1");
                PlotGraph.DrawPlot(this.zedCalibration, spectroCaliData.CaliSpectrumArray[1], true, 2.0F, Color.Red, SymbolType.None, false, true, "Attenuator_2");
                PlotGraph.DrawPlot(this.zedCalibration, spectroCaliData.CaliSpectrumArray[2], true, 2.0F, Color.Green, SymbolType.None, false, true, "Attenuator_3");
                PlotGraph.DrawPlot(this.zedCalibration, spectroCaliData.CaliSpectrumArray[3], true, 2.0F, Color.DarkGoldenrod, SymbolType.None, false, true, "Attenuator_4");
                PlotGraph.DrawPlot(this.zedCalibration, spectroCaliData.CaliSpectrumArray[4], true, 2.0F, Color.DeepPink, SymbolType.None, false, true, "Attenuator_5");
                PlotGraph.SetXYAxis(this.zedCalibration,0,2048,0,000);
                return true;
            }
            else
            {
                return false;
            }
        }

		private void UpdateDataToControls()
        {
			this.txtSptOperationMode.Text = DataCenter._sysSetting.OptiDevSetting.OperationMode.ToString();
			this.txtStartWave.Text = DataCenter._sysSetting.OptiDevSetting.StartWavelength.ToString();
			this.txtEndWave.Text = DataCenter._sysSetting.OptiDevSetting.EndWavelength.ToString();
			this.txtBoxCar.Text = DataCenter._sysSetting.OptiDevSetting.BoxCar.ToString();
			this.txtMinCatchCount.Text = DataCenter._sysSetting.OptiDevSetting.MinCatchPeakCount.ToString();
			//------------------------------------
			this.txtAutoMinCount.Text = DataCenter._sysSetting.OptiDevSetting.AutoLowCount.ToString();
			this.txtAutoMaxCount.Text = DataCenter._sysSetting.OptiDevSetting.AutoHighCount.ToString();
			this.txtLimitTargetCount.Text = DataCenter._sysSetting.OptiDevSetting.LimitTargetCount.ToString();

			this.chkGetRowData.Checked = DataCenter._sysSetting.OptiDevSetting.IsGetRawData;
			this.chkSptMeterTrigger.Checked = DataCenter._sysSetting.OptiDevSetting.IsEnableTrigger;
			this.chkSptMeterCalc.Checked = DataCenter._sysSetting.OptiDevSetting.IsEnableCalc;
			//----------------------------------------------

			this.chkCorrectForNonlineatity.Checked = DataCenter._sysSetting.OptiDevSetting.IsCorrectForNonlinearity;
			this.radSphere.Checked = DataCenter._sysSetting.OptiDevSetting.IsUseSphere;
			this.dinLumensCoeff.Value = DataCenter._sysSetting.OptiDevSetting.LumensCoeff;
			this.dinWattCoeff.Value = DataCenter._sysSetting.OptiDevSetting.WattCoeff;

			//PlotGraph.DrawPlot(this.zedCalibration, DataCenter._sysSetting.OptiDevSetting.CaliSpectrumArray, true, 1.0F, Color.Red, SymbolType.None, false, false, "aa");
			PlotGraph.DrawPlot(this.zedDarkArray, DataCenter._sysSetting.OptiDevSetting.DarkArray, true, 1.0F, Color.Blue, SymbolType.None, false, false, "current Sys");
			this.chkEnableAutoGetdark.Checked = DataCenter._sysSetting.OptiDevSetting.IsAutoGetDark;
            this.CaculateDarkArrayAvg();
            this.GetCurrentDarkArray();

        }

        private void SetSystemConfig()
        {

			DataCenter._sysSetting.OptiDevSetting.IsCorrectForNonlinearity = this.chkCorrectForNonlineatity.Checked;
			DataCenter._sysSetting.OptiDevSetting.LumensCoeff = this.dinLumensCoeff.Value;
			DataCenter._sysSetting.OptiDevSetting.WattCoeff = this.dinWattCoeff.Value;

			if (radSphere.Checked == true)
			{
				DataCenter._sysSetting.OptiDevSetting.IsUseSphere = true;
				DataCenter._sysSetting.OptiDevSetting.SurfaceAreaCmSquared = 1;
			}
			else
			{
				DataCenter._sysSetting.OptiDevSetting.IsUseSphere = false;
				DataCenter._sysSetting.OptiDevSetting.SurfaceAreaCmSquared = CaculateSurfaceAreaCmSquared(this.dinFiberDiameter.Value);
			}

			DataCenter._sysSetting.OptiDevSetting.IsAutoGetDark = this.chkEnableAutoGetdark.Checked;
        }

        private void CaculateDarkArrayAvg()
        {
			this.lblDarkAvg.Text = MPI.Maths.Statistic.Average(DataCenter._sysSetting.OptiDevSetting.DarkArray).ToString("0.0000");
           if(this._darkArray!=null)
           this.lblCurrentDark.Text = MPI.Maths.Statistic.Average(this._darkArray).ToString("0.0000");
        }

		private double CaculateSurfaceAreaCmSquared(double diameter)
		 {
			 double d = 0;
			 return d = (Math.PI) * (diameter / 20000) * (diameter / 20000);
		 }

        private void btnSetParameter_Click(object sender, EventArgs e)
        {
            this.chkEnableAutoGetdark.Checked = false;
			SetSystemConfig();

			//AppSystem.SetDataToSystem();
			DataCenter.Save();   
            this.Hide();
        }

		 private void btnCancel_Click(object sender, EventArgs e)
		 {
			 this.Hide();
		 }

		 private void btnLoadCaliFile_Click(object sender, EventArgs e)
		 {
			 List<string[]> data = new List<string[]>();
			 if (openFileDialog1.ShowDialog() == DialogResult.OK)
			 {
				 if (File.Exists(openFileDialog1.FileName))
				 {
					 this.zedCalibration.GraphPane.CurveList.Clear();
					 data = this.ReadCSV(openFileDialog1.FileName);
				 }
			 }
			 if (data.Count == 0)
			 { return; }

			 data.RemoveRange(0, 8);

			 if (data.Count != 2048)
			 { return; }
			 _caliSpectrum = new double[data.Count];

			 for (int i = 0; i < data.Count; i++)
			 {
				 string[] row = data[i];
                 _caliSpectrum[i] = double.Parse(row[0]);
			 }
			 PlotGraph.DrawPlot(this.zedCalibration, _caliSpectrum, true, 1.0F, Color.Red, SymbolType.None, false, false, "aa");	 
		 }

		 private List<String[]> ReadCSV(string filePathName)
		 {
             List<String[]> ls = new List<String[]>();
             StreamReader fileReader = new StreamReader(filePathName);    
			 string strLine = "";
			 while (strLine != null)
			 {
				 strLine = fileReader.ReadLine();
				 if (strLine == null && strLine.Length < 0)
					 continue;

				 ls.Add(strLine.Split(','));
			 }
			 fileReader.Close();
			 return ls;
		 }

         private  bool IsOpenedFile(string file)
        {
            bool result = false;
            try
            {
                FileStream fs = File.OpenWrite(file);
                fs.Close();
            }
            catch(IOException)
            {
                result = true;
            }    
            return result;
        }

		 private void btnSaveCaliSpectrumToSystem_Click(object sender, EventArgs e)
		 {
			 if(this._caliSpectrum==null || this._caliSpectrum.Length!=2048)
			 {
				 MessageBox.Show("No Cali Spectrum", "Error Message");
				 return;
             }
			 //DataCenter._sysSetting.OptiDevSetting.CaliSpectrumArray = this._caliSpectrum;
		 }

         private void btnLoadDarkArray_Click(object sender, EventArgs e)
         {
             this.ReadDarkArrayToUI();
         }

         private void btnSaveDarkArrayToSystem_Click(object sender, EventArgs e)
         {
             this.SetCurrentDarkArrayToSystem();
         }

         private void btnGetCurrentDark_Click(object sender, EventArgs e)
         {             
             this.GetCurrentDarkArray();
         }

         //-----------------------
         //     Dark Array 
         //-----------------------

         private void ReadDarkArrayToUI()
         {
             List<string[]> readData = new List<string[]>();

             if (openFileDialog1.ShowDialog() == DialogResult.OK)
             {
                 if (File.Exists(openFileDialog1.FileName))
                 {
                     if (this.IsOpenedFile(openFileDialog1.FileName) == false)
                     {
                         readData = this.ReadCSV(openFileDialog1.FileName);
                     }
                     else
                     {
                         MessageBox.Show("The File is being used by another program", "Error Message");
                         return;
                     }
                 }
             }
             if (readData.Count == 0 || readData.Count != 2048)
             { return; }

             this._darkArray = new double[2048];

             for (int i = 0; i < readData.Count; i++)
             {
                 string[] Data = readData[i];
                 this._darkArray[i] = double.Parse(Data[0]);
             }
             this.CaculateDarkArrayAvg();
             PlotGraph.DrawPlot(this.zedDarkArray, _darkArray, true, 1.0F, Color.DarkOrange, SymbolType.None, false, false, "aa");
         }

         private void SetCurrentDarkArrayToSystem()
         {
             DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show("Would you Set Current Dark To System ？", "Replace", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
             if (result != DialogResult.OK)
                return;

             if (this._darkArray == null || this._darkArray.Length != 2048)
             {
                 MessageBox.Show("No Cali Spectrum", "Error Message");
                 return;
             }
             this.chkEnableAutoGetdark.Checked = false;
			 DataCenter._sysSetting.OptiDevSetting.DarkArray = this._darkArray;
             this.zedDarkArray.GraphPane.CurveList.Clear();
			 PlotGraph.DrawPlot(this.zedDarkArray, DataCenter._sysSetting.OptiDevSetting.DarkArray, true, 1.0F, Color.Blue, SymbolType.None, false, false, "current Sys");
             this.CaculateDarkArrayAvg();
             DataCenter.Save();
         }

         private void GetCurrentDarkArray()
         {
             this.chkEnableAutoGetdark.Checked = true;
             this.SetSystemConfig();
             //AppSystem.SetDataToSystem();

             AppSystem.RunCommand(TestKernel.ETesterKernelCmd.GetDarkDataAndSave);
             string pathAndFileNameWithExt = @"C:\MPI\LEDTester\Spectrometer\DarkArray.dat";

             if (File.Exists(pathAndFileNameWithExt) == false)
             {
                 return;
             }

             this._darkArray = new double[2048];
             int index = 0;

             IEnumerable<string> readData = File.ReadLines(pathAndFileNameWithExt);
             foreach(var var in readData)
             {
                 this._darkArray[index] = double.Parse(var);
                 index++;
             }

             PlotGraph.DrawPlot(this.zedDarkArray, _darkArray, true, 1.0F, Color.Red, SymbolType.None, false, false, "current dark Array");
             this.CaculateDarkArrayAvg();
         }
    }
}