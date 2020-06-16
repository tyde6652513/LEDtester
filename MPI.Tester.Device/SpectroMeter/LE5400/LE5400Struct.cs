using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MPI.Tester.Device.SpectroMeter.LE5400
{
    /// <summary>
    /// Msrt CallBack Function
    /// </summary>
    public delegate int LPCALLBACK_MCPDSPECTRUM(ref MCPD_MEASE_SPECTRUM pMEASE_SPECTRUM);
    public delegate int LPCALLBACK_MCPDEVENT(int dwCode);

    /// <summary>
    /// Measurement Condition 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MMC_Cond
    {
        public int ColorCalcCondNo;                    // Color Calc Condtion No(not used)
        public int GateTime;                               // Gate Time LE-4000 series(8-20000)LE-5000 series(2-20000)
        public int Accum;                                    // Accumulation(1-256)
        public int Gain;                                       // Gain LE-4000 series(0:Normal 1:Midle 2:High 3:Super High)
        //      LE-5000 series(0:Normal 1:Midle 2:High)
        public int NdFilter;                           // ND Filter(0-99)
        public int NdAutoMode;                         // ND Filter auto change(0:not support 1:support)
        public int CorrectEng;                         // the number of Engery Correct Table(1-99)
        public int NorSkipCount;                       // Skip Count(0-255)
        public int DarkOpenSkipCount;                  // Shutter Open Skip Count(Dark Meas.)(0-255)
        public int DarkCloseSkipCount;                 // Shutter Close Skip Count(Dark Meas.)(0-255)
        public double LoWave;                           // Limit of wavelength(Lower Limt) (A proper value for measure)
        public double HiWave;                           // Limit of wavelength(Upper Limt) (A proper value for measure PS: >LoWave)
        public double LoData;                           // Limit of Data(Lower Limt)(0.0-1.0)
        public double HiData;                           // Limit of Data(Upper Limt)(0.0-1.0 PS: >LoData)
        public int SigMode;                           // Do Not Subtract Dark (0:Subtract Dark 1:do not subtract dark)
        public int IndexMode;                          // clear Index(0,1:do not clear index, 2,3: clear index)
        public int SyncSt;                             // synchronization start mode (0:Normal Mode 1:synchronization start mode) onle for LE-5000 series
        public int NoBinn;                            // For Inspection mode (0) Only for LE-5000 series
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        public int[] DUMMY;     // not used
        /// <summary>
        /// Meas Condition Init.
        /// dwConnectMode : 1:LE-4000series 2:LE-5000 series
        /// </summary>
        /// <param name="dwConnectMode">1:LE-4000series 2:LE-5000 series</param>
        public MMC_Cond(int dwConnectMode)
        {
            this.ColorCalcCondNo = 1;
            if (dwConnectMode == 2)
            {   //LE-5000 series
                this.GateTime = 2;
                this.DarkCloseSkipCount = 252;
                this.SyncSt = 1;
            }
            else
            {
                //LE-4000 series
                this.GateTime = 8;
                this.DarkCloseSkipCount = 65;
                this.SyncSt = 0;
            }
            this.Accum = 1;
            this.Gain = 0;
            this.NdFilter = 0;
            this.NdAutoMode = 0;
            this.CorrectEng = 1;
            this.NorSkipCount = 3;
            this.DarkOpenSkipCount = 3;

            this.LoWave = 360.0;
            this.HiWave = 830.0;
            this.LoData = 0.15;
            this.HiData = 1.0;
            this.SigMode = 1;
            this.IndexMode = 0;
            this.NoBinn = 0;
            this.DUMMY = new int[30];
        }

    }

    /// <summary>
    ///  Mcpd Luminous Analyzed Condition // Calculate Condition
    /// </summary>
    public struct MLAC_COND
    {
        // Under/Over process
        public int CalcUnderOver;                  // 0:Under/Over Error occur
        // 1:Ignore Under Error and calc. result
        // 2:Ignore Over Error and calc. result
        // 3:Ignore Under/Over Error and calc. result (Default)

        // Meas. Item(>1)
        public int Kind;                           // Set it with the following combinations value
        // 0:Nothing
        // 1:TriStimulusValues(X,Y,Z)
        // 2:Chromaticity Coordinates(x,y)
        // 4:Chromaticity Coordinates(u,v)
        // 8:Chromaticity Coordinates(u,v)
        // 16:Correlated Color Temnperature(Tc,Duv)
        // 32:Dominant,Purity
        // 64:Color Rendering Index

        public int VisionAngle;                    // Vision Angle(0:2deg. 1:10deg.) LE-3000 series : 0(2deg)
        public int StdLight;                       // StdLight(0:CIE daylight 1:black body)
        // User standard illuminants when Color temperature value is larger than 4600k and smaller than 5000k
        // LE-3000 series : 0:CIE daylight
        // Color Calc wavelength range(Required)
        public double WaveLo;                       // Color Calc wavelength range Lower Limit
        public double WaveHi;                       // Color Calc wavelength range Upper Limit

        // Peak wavelength dependence-colored operation range
        public int PeakDepend;                     // Peak wavelength dependence-colored operation range(1:Valid 0:Invalid)
        //Dummy1 As Long
        public double PeakDependLo;                 // Lower Limit (>10 and 5nm as the unit)
        public double PeakDependHi;                 // Upper Limit (>=PeakDependLo and 5nm as the unit)

        // Peak wavelength
        public int PeakNum;                        // the count of Peak(Sub Peak) Detected (0:Invalid 1-5)
        // public int PeakNum_Dummy;
        public double PeakLo;                       // Lower Limit
        public double PeakHi;                       // Upper Limit

        // Second Peak wavelength
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public int[] SecondPeak;            // Detect Second Peak(1:Valid 0:Invalid)
        //public int SecondPeak_Dummy As Long
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public double[] SecondPeakLo;        // Second Peak Wavelength Range(Lower Limit)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public double[] SecondPeakHi;        // Second Peak Wavelength Range(Upper Limit)

        // Summation
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public int[] IntegPeak;             // Summation wavelength Range(1:Valid 0:Invalid)
        //public int IntegPeak_Dummy As Long
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public double[] IntegPeakLo;         // Summation wavelength Range Lower Limit
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public double[] IntegPeakHi;         // Summation wavelength Range Upper Limit

        // Peak half area calc range
        public int PeakHalfArea;                   // Peak half area calc range (0:Invalid 1:lower side 2:upper side 3:both)
        //public int PeakHalfArea_Dummy;
        public double PeakHalfAreaLo;               // Peak half area calc wavelength range Lower Limit
        public double PeakHalfAreaHi;               // Peak half area calc wavelength range Upper Limit

        // Centroid wavelength
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public int[] Centroid;              // Centroid wavelength(1:Valid 0:Invalid)
        //public int Centroid_Dummy As Long
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public double[] CentroidWaveLo;      // Centroid wavelength Range Lower Limit
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public double[] CentroidWaveHi;      // Centroid wavelength Range Upper Limit

        // admin wavelength
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public int[] PointWave;             // Admin Wavelengtht Valid(1:Valid 0:Invalid)
        //public int PointWave_Dummy;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public double[] PointWavelength;     // admin wavelength
        [MarshalAs(UnmanagedType.ByValArray, SizeConst =  LE5400Wrapper.DUMMY_AREA_SIZE)]
        public int[] DUMMY;                // not used

        public MLAC_COND(bool bAllOn)
        {
            this.CalcUnderOver = 0; // 3:Ignore Under/Over Error and calc. result (Default)
            this.Kind = (int)((bAllOn) ? 127 : 1);
            this.VisionAngle = 0;
            this.StdLight = 0;
            this.WaveLo = 360;
            this.WaveHi = 830;
            this.PeakDepend = 0;    //1;
            this.PeakDependLo = 0;  //10;
            this.PeakDependHi = 0;  //200;

            // Peakwavelength
            this.PeakNum = (int)((bAllOn) ? 5 : 0);
            this.PeakLo = 420;
            this.PeakHi = 780;

            // Second Peakwavelength
            SecondPeak = new int[5];
            SecondPeakLo = new double[5];
            SecondPeakHi = new double[5];
            this.SecondPeak[0] = 1;
            this.SecondPeakLo[0] = 380;
            this.SecondPeakHi[0] = 780;
            this.SecondPeak[1] = (int)((bAllOn) ? 1 : 0);
            this.SecondPeakLo[1] = 380;
            this.SecondPeakHi[1] = 500;
            this.SecondPeak[2] = 0;
            this.SecondPeakLo[2] = 500;
            this.SecondPeakHi[2] = 780;
            this.SecondPeak[3] = (int)((bAllOn) ? 1 : 0);
            this.SecondPeakLo[3] = 750;
            this.SecondPeakHi[3] = 950;
            this.SecondPeak[4] = (int)((bAllOn) ? 1 : 0);
            this.SecondPeakLo[4] = 550;
            this.SecondPeakHi[4] = 750;

            // Summationwavelength
            IntegPeak = new int[5];
            IntegPeakLo = new double[5];
            IntegPeakHi = new double[5];
            this.IntegPeak[0] = (int)((bAllOn) ? 1 : 0);
            this.IntegPeakLo[0] = 350;
            this.IntegPeakHi[0] = 950;
            this.IntegPeak[1] = (int)((bAllOn) ? 1 : 0);
            this.IntegPeakLo[1] = 350;
            this.IntegPeakHi[1] = 550;
            this.IntegPeak[2] = (int)((bAllOn) ? 1 : 0);
            this.IntegPeakLo[2] = 360;
            this.IntegPeakHi[2] = 830;
            this.IntegPeak[3] = (int)((bAllOn) ? 1 : 0);
            this.IntegPeakLo[3] = 550;
            this.IntegPeakHi[3] = 750;
            this.IntegPeak[4] = (int)((bAllOn) ? 1 : 0);
            this.IntegPeakLo[4] = 750;
            this.IntegPeakHi[4] = 950;


            this.PeakHalfArea = 3;
            this.PeakHalfAreaLo = 350;
            this.PeakHalfAreaHi = 950;

            // Centroid wavelength
            Centroid = new int[5];
            CentroidWaveLo = new double[5];
            CentroidWaveHi = new double[5];
            this.Centroid[0] = (int)((bAllOn) ? 1 : 0);
            this.CentroidWaveLo[0] = 350;
            this.CentroidWaveHi[0] = 950;
            this.Centroid[1] = 0;
            this.CentroidWaveLo[1] = 360;
            this.CentroidWaveHi[1] = 830;
            this.Centroid[2] = (int)((bAllOn) ? 1 : 0);
            this.CentroidWaveLo[2] = 350;
            this.CentroidWaveHi[2] = 550;
            this.Centroid[3] = (int)((bAllOn) ? 1 : 0);
            this.CentroidWaveLo[3] = 550;
            this.CentroidWaveHi[3] = 750;
            this.Centroid[4] = (int)((bAllOn) ? 1 : 0);
            this.CentroidWaveLo[4] = 750;
            this.CentroidWaveHi[4] = 950;

            // admin wavelength
            PointWave = new int[5];
            PointWavelength = new double[5];
            //this.PointWave[0] = (int)((bAllOn) ? 1 : 0);
            //this.PointWavelength[0] = 500;
            //this.PointWave[1] = (int)((bAllOn) ? 1 : 0);
            //this.PointWavelength[1] = 500;
            //this.PointWave[2] = (int)((bAllOn) ? 1 : 0);
            //this.PointWavelength[2] = 600;
            //this.PointWave[3] = (int)((bAllOn) ? 1 : 0);
            //this.PointWavelength[3] = 700;
            //this.PointWave[4] = (int)((bAllOn) ? 1 : 0);
            //this.PointWavelength[4] = 800;

            this.PointWave[0] = 1;
            this.PointWavelength[0] = 450;
            DUMMY = new int[ LE5400Wrapper.DUMMY_AREA_SIZE];
        }
    }

    /// <summary>
    ///  Mcpd Luminous Analyzed Result 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MLAR_RESULT
    {
        public int PeakDepend;             //Peak wavelength dependence-colored operation range(1:Valid 0:Invalid [the same with color calc wavelength range])
        //PeakDepend_Dummy            As Long
        public double PeakDependLo;         //Peak dependence wavelength range, Lower Side
        public double PeakDependHi;         //Peak dependence wavelength range, Upper Side

        //TriStimulusValues(X,Y,Z)
        public int TriStimulusValues;      //TriStimulusValues(X,Y,Z)(1:Valid 0:Invalid)
        //public int TriStimulusValues_Dummy     As Long
        public double LX;                   //X
        public double LY;                   //Y
        public double LZ;                   //Z

        //Chromaticity Coordinates (x,y)
        public int Chromaticity;           //Chromaticity Coordinates(x,y) (1:Valid 0:Invalid)
        //public int Chromaticity_Dummy          As Long
        public double X;                   //x
        public double Y;                   //y

        //Chromaticity Coordinates(u,v)
        public int Chromaticity_uv;    //Chromaticity Coordinates(u,v) (1:Valid 0:Invalid)
        //public int Chromaticity_uv_Dummy       As Long
        public double u;                   //u
        public double v;                   //v

        //Chromaticity Coordinates(u,v)
        public int Chromaticity_uvDush;    //Chromaticity Coordinates(u,v) (1:Valid 0:Invalid)
        //public int Chromaticity_uvDush_Dummy   As Long
        public double uDush;                //uDush
        public double vDush;                //vDush

        //Correlated Color Temnperature(CCT)
        public int CorrelatedColorTemp;    //Correlated Color Temnperature (1:Valid 0:Invalid)
        //public int CorrelatedColorTemp_Dummy   As Long
        public double Tc;                   //Color Temperature
        public double Duv;                  //Duv

        //Dominant,Purity
        public int DominantPurity;         //Dominant,Purity (1:Valid 0:Invalid)
        //double int DominantPurity_Dummy        As Long
        public double Dominant;             //Dominant
        public double Purity;               //Purity

        //Color Rendering Index
        public int Rendering;              //Color Rendering Index (1:Valid 0:Invalid)
        public int StdLight;               //0:CIE daylight 1:black body(when CCT is more than 4600 and less than 5000)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public double[] R;                  //Color Rendering Ra,R1-R15 ([0]=Ra,[1]=R1-[15]=R15)

        //other

        //Peak Information(wavelength,Height,half width)
        public int Peak;                               //Detect Peak(0:Invalid,1-5:the number of Peak)
        //public int Peak_Dummy                  As Long
        [MarshalAs(UnmanagedType.ByValArray, SizeConst =  LE5400Wrapper.OCC_SIZE)]
        public double[] PeakWave;         //Peak wavelength([0]=Top Peak)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst =  LE5400Wrapper.OCC_SIZE)]
        public double[] PeakHeight;      //Peak Height([0]=Top Peak)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst =  LE5400Wrapper.OCC_SIZE)]
        public double[] PeakHalfWidth;    //half width([0]=Top Peak)

        //Summation
        [MarshalAs(UnmanagedType.ByValArray, SizeConst =  LE5400Wrapper.OCC_SIZE)]
        public int[] Summation;          //Summation(1:Valid 0:Invalid)
        //Summation_Dummy             As Long
        [MarshalAs(UnmanagedType.ByValArray, SizeConst =  LE5400Wrapper.OCC_SIZE)]
        public double[] Sum;              //Summation wavelength Range

        //Second Peak Information (wavelength,Height,half width)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst =  LE5400Wrapper.OCC_SIZE)]
        public int[] SecPeak;            //Second Peak Calculation(1:Valid 0:Invalid)
        //SecPeak_Dummy               As Long
        [MarshalAs(UnmanagedType.ByValArray, SizeConst =  LE5400Wrapper.OCC_SIZE)]
        public double[] SecPeakWave;      //Second Peak wavelength
        [MarshalAs(UnmanagedType.ByValArray, SizeConst =  LE5400Wrapper.OCC_SIZE)]
        public double[] SecPeakHeight;    //Second Peak Height
        [MarshalAs(UnmanagedType.ByValArray, SizeConst =  LE5400Wrapper.OCC_SIZE)]
        public double[] SecPeakHalfWidth; //half width

        //Integral calculus Peak half area
        public int PeakHalf_Before;                     //Lower side (1:Valid 0:Invalid)
        //PeakHalfB_Dummy             As Long
        public double PHBefore;                         //Integral calculus Peak half area(Lower Side)
        public int PeakHalf_After;                      //Upper Side (1:Valid 0:Invalid)
        //PeakHalfA_Dummy             As Long
        public double PHAfter;                          //Integral calculus Peak half area(Upper Side)

        //Centroid wavelength
        [MarshalAs(UnmanagedType.ByValArray, SizeConst =  LE5400Wrapper.OCC_SIZE)]
        public int[] Centroid;          //Centroid wavelength (1:Valid 0:Invalid)
        //public int Centroid_Dummy              As Long
        [MarshalAs(UnmanagedType.ByValArray, SizeConst =  LE5400Wrapper.OCC_SIZE)]
        public double[] CentroidWave;     //Centroid wavelength

        //admin wavelength
        [MarshalAs(UnmanagedType.ByValArray, SizeConst =  LE5400Wrapper.OCC_SIZE)]
        public int[] PointWave;          //Admin Wavelengtht Valid (1:Valid 0:Invalid)
        //public int PointWave_Dummy;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst =  LE5400Wrapper.OCC_SIZE)]
        public double[] PointData;        //Data

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public int[] DUMMY;                    //not used
    }

    /// <summary>
    ///  Mcpd Measurement Spectrum
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MCPD_MEASE_SPECTRUM
    {
        public int dwKind;                             // Kind Number(Normal:1)
        // 1:Measurement Data 2:Calc Spectrum 4:Dark Data 8:ND Filter 16:Correct Data
        public int dwIndex;                            // Index Number
        public int dwCount;                            // Array Count of pdWave
        public int dwUnderOver;                        // Under/Over (0:Normal 1:Under 2:Over )
        [MarshalAs(UnmanagedType.ByValArray, SizeConst =  LE5400Wrapper.DUMMY_AREA_SIZE)]
        public int[] DUMMY;                            // not use
        [MarshalAs(UnmanagedType.ByValArray, SizeConst =  LE5400Wrapper.MEAS_DATA_MAX)]
        public double[] pdWave;                         // Array of wavelength
        [MarshalAs(UnmanagedType.ByValArray, SizeConst =  LE5400Wrapper.MEAS_DATA_MAX)]
        public double[] pdValue;                        // Array of Data
    }

    /// <summary>
    ///  Adjust Fitting Integtaring Time
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AUTOADJUST_RESULT
    {
        public bool result;                              // Result(1:Success0:Failed) uint???
        public uint GateTime;                            // Adjusted Gete Time
        [MarshalAs(UnmanagedType.ByValArray, SizeConst =  LE5400Wrapper.DUMMY_AREA_SIZE)]
        public int[] DUMMY;                             // Dummy
        public double Value;                            // Adjusted Data(0.0-1.0)
        public double Wave;                              // Adjusted wavelength
    }

    /// <summary>
    ///  Dark Data
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DARKDATA
    {

        public int GateTime;                   // Gate Time
        public int Accum;                      // Accumulation
        public int ShutterMode;                // Shutter Mode(0:CLOSE 1:OPEN)
        public int Gain;                       // 0,1,2,3(L,M,H,SH)
        public int Index;                      // Data Index
        public int Count;                      // Valid Data Count(Usually,it is 512)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst =  LE5400Wrapper.DUMMY_AREA_SIZE)]
        public int[] DUMMY;                    // Dummy
        [MarshalAs(UnmanagedType.ByValArray, SizeConst =  LE5400Wrapper.MEAS_DATA_MAX)]
        public int[] Data;                     // the gain value from analog-digital converter(512)
    }

    /// <summary>
    ///  Correct Engery Config
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CORENGINFO
    {
        // Information
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = LE5400Wrapper.STRBUFF_SIZE)]
        public char[] CorEngName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = LE5400Wrapper.STRBUFF_SIZE)]
        public char[] UserID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = LE5400Wrapper.STRBUFF_SIZE)]
        public char[] MakeDate;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = LE5400Wrapper.STRBUFF_SIZE)]
        public char[] UpDate;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = LE5400Wrapper.STRBUFF_SIZE)]
        public char[] StdLampNameUV;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = LE5400Wrapper.STRBUFF_SIZE)]
        public char[] StdLampNameVIS;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = LE5400Wrapper.STRBUFF_SIZE)]
        public char[] NDFilterUV;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = LE5400Wrapper.STRBUFF_SIZE)]
        public char[] NDFilterVIS;
        public double JoinWaveMin;
        public double JoinWaveMax;
        public int STime;
        public int AGain;
        public double CorEngCoefficient;
        public double CorEngCoefficient2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = LE5400Wrapper.STRBUFF_SIZE)]
        public char[] CorEngUnit;
        public int DataCnt;
        public int DataCnt2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = LE5400Wrapper.STRBUFF_SIZE)]
        public char[] SlitName;
        public double SlitWaveWidth;
        public double StdValue;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = LE5400Wrapper.STRBUFF_SIZE)]
        public char[] Unit;
        public double Coefficient;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = LE5400Wrapper.STRBUFF_SIZE)]
        public char[] CorWaveMakeDate;
        public double Const;
        public double FstCoefficient;
        public double SndCoefficient;
        public double TrdCoefficient;
        public int CorSTimeFlg;
        public int CorSTimeAMax;
        public int CorSTimeMMax;
        public int CorGainFlg;
        public int CorGain;
        public int CorATime;
        public int DiaFlg;
        //Dummy_DiaFlg                    As Long
        public double CorEngWaveLo;
        public double CorEngWaveHi;
        public double CorEngWaveLo2;
        public double CorEngWaveHi2;
        public double CorEngWaveInt2;

        public ushort SpectroscopeNO;                   // Spectroscope number
        public ushort PointRespect;                     // Point,Surface,pin division(0:Point 1:Surface 2:pin)

        // wavelength Table and Corrected Value
        public int MCount;                             // Meas. Data Count(Usually,it is 512)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = LE5400Wrapper.MEAS_DATA_MAX)]
        public double[] MWave;      // Wave Table
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = LE5400Wrapper.MEAS_DATA_MAX)]
        public double[] MData;                         // Measurement Corrected Data Table
        public int CCount;                             // Calc. Data Count(Usually,it is 95)
        //Dummy_CCount As int
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = LE5400Wrapper.CALC_DATA_MAX)]
        public double[] CWave;      // Color Calc Wavelength Table
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = LE5400Wrapper.CALC_DATA_MAX)]
        public double[] CData;      // Color Calc Correct Data Table
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = LE5400Wrapper.DUMMY_AREA_SIZE)]
        public int[] DUMMY;       // Dummy
    }
}

