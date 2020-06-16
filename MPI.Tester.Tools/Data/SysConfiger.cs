using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MPI.Tester.Data;

namespace MPI.Tester.Tools
{
    public class SysConfig
    {
        public string ParamDir;
        public string CompareFileDir;
        public string StdFileDir;
        public string MsrtFileDir;
        public string LoadFormatPath;

        public bool IsModifyGainOffset;
        [XmlIgnore]
        public EUserID UserID;

        public bool IsSingleLOPItem;

        public int DispWaveItem;
        public int DispLOPItem;

        public int PlotPointType;
        public bool IsPlotBoundary;
        public bool IsPlotCoefCurve;
        public bool IsPlotDataPoints;
        public bool IsFillDataPoint;
        public bool IsSetDataPointsColor;
        public int DataPointColor;

        public double WavelenghtSpec;
        public double LightPowerSpec;
        public int VoltChartXBaseSelect;
        public bool IsAutoClearChart;

        public Boolean[] IsCaculateCoeff;

        public bool IsArrangeByRowCol;

        public int LookTableByStdOrMsrtMode;

        public int FilterStdevCount;

        public string FileExtension;
        public int CalcCoefMode;
        public int ExtendWLMode;
        public int ExtendWLPoint;
        public double ExtendWLStart;
        public double ExtendWLEnd;

        public int WaveExtDigit;
        public int LOPExtDigit;

        public bool IsEnableUseRecipeCriterion;
        public bool IsEnableCalibrationMode;
        public int IsCoeffExtendType;
        //-----------------------------------------
        // DailyWatch Use
        //-----------------------------------------
        public string Daily_OpenFilePath;
        public string Daily_UpLoadFilePath;
      //  public bool IsShowAllResultItem;
       // public string DailyWatch_STDFilePath;
        public string DailyWatch_STDFileName = "";
       // public string DailyWatch_OutputFilePath = "";
       // public string DailyWatch_OutputFilePath2 = "";
      //  public int DailyWatch_AutoLevel;
        //public bool DailyWathc_IsCrateMachimeNameFolderToSaveFile = false;
        //public bool DailyWathc_IsFilterData = false;
        //public bool DailyWathc_IsCheckEveryDieInSpec = false;
        //public string DailyWatch_MsrtFileDir = "";
        public int CoeffExtendType = 0;

        public DailyCheckConfig DCheck;
        public bool IsEnableT100OperationMode;

        //-----------------------------------------
        // By Channel Calibration Use
        //-----------------------------------------
        public string StdChannelFileDir;
        public string MsrtChannelFileDir;

        public bool IsEnableCompensateChannelFactor;

        public DeviceVerifyConfig DeviceVerify;

        public SysConfig()
        {
            this.LoadFormatPath = System.IO.Path.Combine(Constants.Paths.TOOLS_DIR, "OutputTemp");
            this.IsModifyGainOffset = false;
            this.UserID = EUserID.MPI;

            this.IsFillDataPoint = true;
            this.PlotPointType = 1;
            this.IsPlotBoundary = true;
            this.IsPlotCoefCurve = true;
            this.IsPlotDataPoints = true;
            this.IsSetDataPointsColor = false;
            this.IsAutoClearChart = true;

            this.WavelenghtSpec = 0.3;
            this.LightPowerSpec = 0.03;

            this.IsCaculateCoeff = new Boolean[7] { false, true, false, false, true, false, false };
            this.IsArrangeByRowCol = true;
            this.LookTableByStdOrMsrtMode = 1;
            this.FilterStdevCount = 6;
            this.FileExtension = ".csv";

            this.CompareFileDir = Constants.Paths.MPI_TEMP_DIR;
            this.MsrtFileDir = Constants.Paths.MPI_TEMP_DIR;
            this.StdFileDir = Constants.Paths.MPI_TEMP_DIR;
            //-----------------------
            // Daily 
            //-----------------------
            this.Daily_OpenFilePath = Constants.Paths.LEDTESTER_TEMP_DIR;
            this.Daily_UpLoadFilePath = Constants.Paths.LEDTESTER_TEMP_DIR;

            this.CalcCoefMode = 0;
            this.ExtendWLMode = 0;
            this.ExtendWLPoint = 5;
            this.ExtendWLStart = 430.0;
            this.ExtendWLEnd = 470.0;
            this.CoeffExtendType = 0;

            this.WaveExtDigit = 1;
            this.LOPExtDigit = 2;

            this.VoltChartXBaseSelect = 0;
            this.IsEnableUseRecipeCriterion = false;
            this.IsEnableCalibrationMode = true;
        //    this.DailyWatch_MsrtFileDir = Constants.Paths.LEDTESTER_TEMP_DIR;
            this.IsEnableT100OperationMode = false;
            this.DCheck = new DailyCheckConfig();

            //-----------------------
            // By Channel Calibration
            //-----------------------
            this.StdChannelFileDir = Constants.Paths.LEDTESTER_TEMP_DIR;
            this.MsrtChannelFileDir = Constants.Paths.LEDTESTER_TEMP_DIR;

            this.IsEnableCompensateChannelFactor = false;

            this.DeviceVerify = new DeviceVerifyConfig();
        }

        public int UserIDNumber
        {
            get { return ((int)this.UserID); }
            set
            {
                if (Enum.IsDefined(typeof(EUserID), value))
                {
                    this.UserID = (EUserID)value;
                }
                else
                {
                    this.UserID = EUserID.MPI;
                }
            }
        }
    }

    public class DailyCheckConfig
    {
        public string StdFileDir;
        public string MsrtFileDir = "";
        public string CriterionFileAndPath;
        public string CriterionFileName = "";
        public string UploadDir;
        public string OutputFileDir = "";
        public string OutputFileDir2 = "";
        public int AutoRunLevel;
        public bool IsCrateMachimeNameFolderToSaveFile = false;
        public bool IsFilterData = false;
        public bool IsCheckEveryDieInSpec = false;
        public bool IsShowAllResultItem;
        public ETesterResultCreatFolderType CreateFolderToSaveFile;
        public bool IsAutoRun ;
        public bool IsArrangeByRowCol;
        public string SaveReportRule = "";
        public bool IsCheckValid;
        public int[] ValidPeriod;
        public EDailyCheckSpecBy CriterionBy;
        public string SaveResultLogDir;
        public bool IsFilterOutputFile2;
        public int TestFileOverdueHours;
        public bool IsRenameMsrtFileByCreateTime;

         public DailyCheckConfig()
         {
             this.UploadDir = Constants.Paths.LEDTESTER_TEMP_DIR;
             this.IsShowAllResultItem = true;
             this.StdFileDir = Constants.Paths.LEDTESTER_TEMP_DIR;
             this.OutputFileDir = Constants.Paths.MPI_TEMP_DIR;
             this.OutputFileDir2 = Constants.Paths.MPI_TEMP_DIR;
             this.AutoRunLevel = 0;
             this.CriterionFileAndPath = "";
             this.IsCrateMachimeNameFolderToSaveFile = false;
             this.IsFilterData = false;
             this.IsCheckEveryDieInSpec = false;
             this.IsShowAllResultItem = false;
             this.CreateFolderToSaveFile = ETesterResultCreatFolderType.ByMachineName;
             this.MsrtFileDir = Constants.Paths.LEDTESTER_TEMP_DIR;
             this.IsAutoRun = false;
             IsArrangeByRowCol = true;
             SaveReportRule = "";
             IsCheckValid = false;
             ValidPeriod = new int[5];
             CriterionBy = EDailyCheckSpecBy.RECIPE;
             SaveResultLogDir = Constants.Paths.LEDTESTER_TEMP_DIR;
             IsFilterOutputFile2 = true;
             TestFileOverdueHours = -1;
             IsRenameMsrtFileByCreateTime = false;
         }
        
    }

    public class DeviceVerifyConfig
    {
        private const int DEFAULT_COUNT = 3;

        public string TaskSheetFileName;
        public uint TestRepeatCount;
        public uint TestRepeatDelay;

        public string BiasValueFilePath;
        public string ResultFilePath;

        public DeviceVerifyChannelConfig Channel;

        public DeviceVerifyConfig()
        {
            this.TaskSheetFileName = string.Empty;

            this.TestRepeatCount = 10;   // cnt
            this.TestRepeatDelay = 50;   // ms

            this.BiasValueFilePath = Constants.Paths.TOOLS_DIR;

            this.ResultFilePath = Constants.Paths.MPI_TEMP_DIR;

            Channel = new DeviceVerifyChannelConfig();

            Channel.Data.Clear();

            for (int i = 0; i < DEFAULT_COUNT; i++)
            {
                Channel.Data.Add(new DeviceVerifyChannelData());
            }
        }

    }
}

