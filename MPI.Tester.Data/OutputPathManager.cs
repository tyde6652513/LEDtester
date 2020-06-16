using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.IO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

using Newtonsoft.Json;

namespace MPI.Tester.Data
{
    
    public class OutputPathManager
    {
        private object _lockObj;

        #region >>normal output<<
        //protected bool _isTestResultPathByTaskSheet;//改到UISetting，否則覆寫時會被一起覆蓋
        
        protected int _fileNameFormatPresent;

        //private PathInfo p1 = new PathInfo();
        public PathInfo OutPathInfo01;
        public PathInfo OutPathInfo02;
        public PathInfo OutPathInfo03;
        public PathInfo ManualOutPathInfo01;
        public PathInfo ManualOutPathInfo02;
        public PathInfo ManualOutPathInfo03;

        #endregion

        #region >>sweep output<<
        public PathInfo SweepPathInfo01;
        #endregion

        #region >>SAF output<<       
        public PathInfo WAFPathInfo01;
        public PathInfo WAFPathInfo02;
        public PathInfo WAFPathInfo03;
        #endregion

        #region >>STAT output<<
        public PathInfo STATPathInfo01;
        public PathInfo STATPathInfo02;
        public PathInfo STATPathInfo03;

        #endregion

        #region >>Spetrometer output<<
        public PathInfo AbsSpcPathInfo01;
        public PathInfo AbsSpcPathInfo02;
        public PathInfo AbsSpcPathInfo03;
        public PathInfo RelSpcPathInfo01;
        public PathInfo RelSpcPathInfo02;
        public PathInfo RelSpcPathInfo03;

        protected bool _isEnableSaveDarkSpectrum;
        protected uint _saveSpectrumMaxCount;
        #endregion

        #region >>LIV/PIV output<<
        public PathInfo LIVPathInfo01;
        public PathInfo LIVPathInfo02;
        public PathInfo LIVPathInfo03;
        #endregion
        
        #region >>Sys path<<
        protected PathInfo _mapPathInfo;

        protected PathInfo _productPathInfo;

        protected PathInfo _productPathInfo2;

        protected PathInfo _coefTablePathInfo;

        protected PathInfo _coefBackupPathInfo;
        #endregion

        #region>>public mathod<<
        public OutputPathManager()
        {
            _lockObj = new object();

            OutPathInfo01 = new PathInfo(true, Constants.Paths.MPI_TEMP_DIR, ETesterResultCreatFolderType.None,"csv");
            OutPathInfo02 = new PathInfo(false, Constants.Paths.MPI_TEMP_DIR, ETesterResultCreatFolderType.None, "csv");
            OutPathInfo03 = new PathInfo(false, Constants.Paths.MPI_TEMP_DIR, ETesterResultCreatFolderType.None, "csv");
            ManualOutPathInfo01 = new PathInfo(true, Constants.Paths.MPI_TEMP_DIR, ETesterResultCreatFolderType.None, "csv");
            ManualOutPathInfo02 = new PathInfo(false, Constants.Paths.MPI_TEMP_DIR, ETesterResultCreatFolderType.None, "csv");
            ManualOutPathInfo03 = new PathInfo(false, Constants.Paths.MPI_TEMP_DIR, ETesterResultCreatFolderType.None, "csv");

            SweepPathInfo01 = new PathInfo(false, Constants.Paths.MPI_TEMP_DIR, ETesterResultCreatFolderType.None);

            //this._sweepOutputPath02 = Constants.Paths.MPI_TEMP_DIR;
            //this._sweepOutputPath03 = Constants.Paths.MPI_TEMP_DIR;

            // WAF PATH SETTING
            WAFPathInfo01 = new PathInfo(true, Constants.Paths.MPI_TEMP_DIR, ETesterResultCreatFolderType.None, "WAF");
            WAFPathInfo02 = new PathInfo(false, Constants.Paths.MPI_TEMP_DIR, ETesterResultCreatFolderType.None, "WAF");
            WAFPathInfo03 = new PathInfo(false, Constants.Paths.MPI_TEMP_DIR, ETesterResultCreatFolderType.None, "WAF");

            // Statistic PATH SETTING
            STATPathInfo01 = new PathInfo(true, Constants.Paths.MPI_TEMP_DIR, ETesterResultCreatFolderType.None, "txt");
            STATPathInfo02 = new PathInfo(false, Constants.Paths.MPI_TEMP_DIR, ETesterResultCreatFolderType.None, "txt");
            STATPathInfo03 = new PathInfo(false, Constants.Paths.MPI_TEMP_DIR, ETesterResultCreatFolderType.None, "txt"); 

            this._fileNameFormatPresent = (int)EOutputFileNamePresent.BarCode;


            AbsSpcPathInfo01 = new PathInfo(false, Constants.Paths.MPI_TEMP_DIR2, ETesterResultCreatFolderType.None, "abs");
            AbsSpcPathInfo02 = new PathInfo(false, Constants.Paths.MPI_TEMP_DIR2, ETesterResultCreatFolderType.None, "abs");
            AbsSpcPathInfo03 = new PathInfo(false, Constants.Paths.MPI_TEMP_DIR2, ETesterResultCreatFolderType.None, "abs");
            RelSpcPathInfo01 = new PathInfo(false, Constants.Paths.MPI_TEMP_DIR2, ETesterResultCreatFolderType.None, "rel");
            RelSpcPathInfo02 = new PathInfo(false, Constants.Paths.MPI_TEMP_DIR2, ETesterResultCreatFolderType.None, "rel");
            RelSpcPathInfo03 = new PathInfo(false, Constants.Paths.MPI_TEMP_DIR2, ETesterResultCreatFolderType.None, "rel");

            this._isEnableSaveDarkSpectrum = false;
            this._saveSpectrumMaxCount = 1;

            LIVPathInfo01 = new PathInfo(false, Constants.Paths.MPI_TEMP_DIR2, ETesterResultCreatFolderType.None, "csv");
            LIVPathInfo02 = new PathInfo(false, Constants.Paths.MPI_TEMP_DIR2, ETesterResultCreatFolderType.None, "csv");
            LIVPathInfo03 = new PathInfo(false, Constants.Paths.MPI_TEMP_DIR2, ETesterResultCreatFolderType.None, "csv");
            
            this.PathInfoArr = new PathInfo[20];
            for (int i = 0; i < 20; ++i)
            {
                PathInfoArr[i] = (new PathInfo());
            }

            _mapPathInfo = new PathInfo(false,Constants.Paths.MPI_TEMP_DIR2,fileExt:"pmap");

            _productPathInfo = new PathInfo(true, Constants.Paths.PRODUCT_FILE);

            _productPathInfo2 = new PathInfo(true, Constants.Paths.PRODUCT_FILE02);

            _coefTablePathInfo = new PathInfo(true, Constants.Paths.MPI_TEMP_DIR);

            _coefBackupPathInfo = new PathInfo(true, Constants.Paths.PRODUCT_FILE02);

            UIMapPathInfo = new PathInfo(false, Constants.Paths.MPI_TEMP_DIR);

            MergeFilePath = new PathInfo(false, Constants.Paths.MPI_TEMP_DIR);

            LaserPowerLogPath = new PathInfo(false, Constants.Paths.MPI_TEMP_DIR);

            OptoTechKeyInDataPath = "";//Constants.Paths.MPI_TEMP_DIR

        }
        #endregion

        #region>> public property<<

        public string TestResultPath01
        {
            get { return this.OutPathInfo01.TestResultPath; }
            set { lock (this._lockObj) { this.OutPathInfo01.TestResultPath = value; } }
        }

        public string TestResultPath02
        {
            get { return this.OutPathInfo02.TestResultPath; }
            set { lock (this._lockObj) { this.OutPathInfo02.TestResultPath = value; } }
        }

        public string TestResultPath03
        {
            get { return this.OutPathInfo03.TestResultPath; }
            set { lock (this._lockObj) { this.OutPathInfo03.TestResultPath = value; } }
        }

        public string ManualOutputPath01
        {
            get { return this.ManualOutPathInfo01.TestResultPath; }
            set { lock (this._lockObj) { this.ManualOutPathInfo01.TestResultPath = value; } }
        }

        public string ManualOutputPath02
        {
            get { return this.ManualOutPathInfo02.TestResultPath; }
            set { lock (this._lockObj) { this.ManualOutPathInfo02.TestResultPath = value; } }
        }

        public string ManualOutputPath03
        {
            get { return this.ManualOutPathInfo03.TestResultPath; }
            set { lock (this._lockObj) { this.ManualOutPathInfo03.TestResultPath = value; } }
        }
        public ETesterResultCreatFolderType TesterResultCreatFolderType01
        {
            get { return this.OutPathInfo01.FolderType; }
            set { lock (this._lockObj) { this.OutPathInfo01.FolderType = value; } }
        }

        public ETesterResultCreatFolderType TesterResultCreatFolderType02
        {
            get { return this.OutPathInfo02.FolderType; }
            set { lock (this._lockObj) { this.OutPathInfo03.FolderType = value; } }
        }

        public ETesterResultCreatFolderType TesterResultCreatFolderType03
        {
            get { return this.OutPathInfo03.FolderType; }
            set { lock (this._lockObj) { this.OutPathInfo03.FolderType = value; } }
        }

        public ETesterResultCreatFolderType ManualOutputPathType01
        {
            get { return this.ManualOutPathInfo01.FolderType; }
            set { lock (this._lockObj) { this.ManualOutPathInfo01.FolderType = value; } }
        }

        public ETesterResultCreatFolderType ManualOutputPathType02
        {
            get { return this.ManualOutPathInfo02.FolderType; }
            set { lock (this._lockObj) { this.ManualOutPathInfo02.FolderType = value; } }
        }

        public ETesterResultCreatFolderType ManualOutputPathType03
        {
            get { return this.ManualOutPathInfo03.FolderType; }
            set { lock (this._lockObj) { this.ManualOutPathInfo03.FolderType = value; } }
        }

        public string SweepOutputPath
        {
            get { return this.SweepPathInfo01.TestResultPath; }
            set { lock (this._lockObj) { this.SweepPathInfo01.TestResultPath = value; } }
        }

        public ETesterResultCreatFolderType SweepOutputPathType
        {
            get { return this.SweepPathInfo01.FolderType; }
            set { lock (this._lockObj) { this.SweepPathInfo01.FolderType = value; } }
        }

        
        //public string SweepOutputPath02
        //{
        //    get { return this._sweepOutputPath02; }
        //    set { lock (this._lockObj) { this._sweepOutputPath02 = value; } }
        //}
        //public string SweepOutputPath03
        //{
        //    get { return this._sweepOutputPath03; }
        //    set { lock (this._lockObj) { this._sweepOutputPath03 = value; } }
        //}

        public bool IsEnablePath01
        {
            get { return this.OutPathInfo01.EnablePath; }
            set { lock (this._lockObj) { this.OutPathInfo01.EnablePath = value; } }
        }

        public bool IsEnablePath02
        {
            get { return this.OutPathInfo02.EnablePath; }
            set { lock (this._lockObj) { this.OutPathInfo02.EnablePath = value; } }
        }

        public bool IsEnablePath03
        {
            get { return this.OutPathInfo03.EnablePath; }
            set { lock (this._lockObj) { this.OutPathInfo03.EnablePath = value; } }
        }

        public bool IsEnableManualPath01
        {
            get { return this.ManualOutPathInfo01.EnablePath; }
            set { lock (this._lockObj) { this.ManualOutPathInfo01.EnablePath = value; } }
        }

        public bool IsEnableManualPath02
        {
            get { return this.ManualOutPathInfo02.EnablePath; }
            set { lock (this._lockObj) { this.ManualOutPathInfo02.EnablePath = value; } }
        }

        public bool IsEnableManualPath03
        {
            get { return this.ManualOutPathInfo03.EnablePath; }
            set { lock (this._lockObj) { this.ManualOutPathInfo03.EnablePath = value; } }
        }

        public bool IsEnableSweepPath
        {
            get { return this.SweepPathInfo01.EnablePath; }
            set { lock (this._lockObj) { this.SweepPathInfo01.EnablePath = value; } }
        }
        //public bool IsEnableSweepPath02
        //{
        //    get { return this._isEnableSweepPath02; }
        //    set { lock (this._lockObj) { this._isEnableSweepPath02 = value; } }
        //}
        //public bool IsEnableSweepPath03
        //{
        //    get { return this._isEnableSweepPath03; }
        //    set { lock (this._lockObj) { this._isEnableSweepPath03 = value; } }
        //}

        // WAF Public Setting

        public bool IsEnableWAFPath01
        {
            get { return this.WAFPathInfo01.EnablePath; }
            set { lock (this._lockObj) { this.WAFPathInfo01.EnablePath = value; } }
        }

        public bool IsEnableWAFPath02
        {
            get { return this.WAFPathInfo02.EnablePath; }
            set { lock (this._lockObj) { this.WAFPathInfo02.EnablePath = value; } }
        }

        public bool IsEnableWAFPath03
        {
            get { return this.WAFPathInfo03.EnablePath; }
            set { lock (this._lockObj) { this.WAFPathInfo03.EnablePath = value; } }
        }

        public string WAFOutputPath01
        {
            get { return this.WAFPathInfo01.TestResultPath; }
            set { lock (this._lockObj) { this.WAFPathInfo01.TestResultPath = value; } }
        }

        public string WAFOutputPath02
        {
            get { return this.WAFPathInfo02.TestResultPath; }
            set { lock (this._lockObj) { this.WAFPathInfo02.TestResultPath = value; } }
        }

        public string WAFOutputPath03
        {
            get { return this.WAFPathInfo03.TestResultPath; }
            set { lock (this._lockObj) { this.WAFPathInfo03.TestResultPath = value; } }
        }

        public bool IsEnableSTATPath01
        {
            get { return this.STATPathInfo01.EnablePath; }
            set { lock (this._lockObj) { this.STATPathInfo01.EnablePath = value; } }
        }

        public bool IsEnableSTATPath02
        {
            get { return this.STATPathInfo02.EnablePath; }
            set { lock (this._lockObj) { this.STATPathInfo02.EnablePath = value; } }
        }

        public bool IsEnableSTATPath03
        {
            get { return this.STATPathInfo03.EnablePath; }
            set { lock (this._lockObj) { this.STATPathInfo03.EnablePath = value; } }
        }

        public string STATOutputPath01
        {
            get { return this.STATPathInfo01.TestResultPath; }
            set { lock (this._lockObj) { this.STATPathInfo01.TestResultPath = value; } }
        }

        public string STATOutputPath02
        {
            get { return this.STATPathInfo02.TestResultPath; }
            set { lock (this._lockObj) { this.STATPathInfo02.TestResultPath = value; } }
        }

        public string STATOutputPath03
        {
            get { return this.STATPathInfo03.TestResultPath; }
            set { lock (this._lockObj) { this.STATPathInfo03.TestResultPath = value; } }
        }


        public string TestResultFileExt
        {
            get { return this.OutPathInfo01.FileExt; }
            set
            {
                lock (this._lockObj)
                {
                    this.OutPathInfo01.FileExt = value;
                    this.OutPathInfo02.FileExt = value;
                    this.OutPathInfo03.FileExt = value;
                    this.ManualOutPathInfo01.FileExt = value;
                    this.ManualOutPathInfo02.FileExt = value;
                    this.ManualOutPathInfo03.FileExt = value;
                }
            }
        }

        public int FileNameFormatPresent
        {
            get { return this._fileNameFormatPresent; }
            set { lock (this._lockObj) { this._fileNameFormatPresent = value; } }
        }

        public EOutputFileNamePresent EFileNameFormatPresent
        {
            get { return (EOutputFileNamePresent)this._fileNameFormatPresent; }
            set { lock (this._lockObj) { this._fileNameFormatPresent = (int)value; } }
        }

        public ETesterResultCreatFolderType WAFTesterResultCreatFolderType01
        {
            get { return this.WAFPathInfo01.FolderType; }
            set { lock (this._lockObj) { this.WAFPathInfo01.FolderType = value; } }
        }

        public ETesterResultCreatFolderType WAFTesterResultCreatFolderType02
        {
            get { return this.WAFPathInfo02.FolderType; }
            set { lock (this._lockObj) { this.WAFPathInfo02.FolderType = value; } }
        }

        public ETesterResultCreatFolderType WAFTesterResultCreatFolderType03
        {
            get { return this.WAFPathInfo03.FolderType; }
            set { lock (this._lockObj) { this.WAFPathInfo03.FolderType = value; } }
        }

        public string WAFTestResultFileExt
        {
            get { return this.WAFPathInfo01.FileExt; }
            set
            {
                lock (this._lockObj)
                {
                    this.WAFPathInfo01.FileExt = value;
                    this.WAFPathInfo02.FileExt = value;
                    this.WAFPathInfo02.FileExt = value;
                }
            }
        }

        public ETesterResultCreatFolderType STATTesterResultCreatFolderType01
        {
            get { return this.STATPathInfo01.FolderType; }
            set { lock (this._lockObj) { this.STATPathInfo01.FolderType = value; } }
        }

        public ETesterResultCreatFolderType STATTesterResultCreatFolderType02
        {
            get { return this.STATPathInfo02.FolderType; }
            set { lock (this._lockObj) { this.STATPathInfo02.FolderType = value; } }
        }

        public ETesterResultCreatFolderType STATTesterResultCreatFolderType03
        {
            get { return this.STATPathInfo03.FolderType; }
            set { lock (this._lockObj) { this.STATPathInfo03.FolderType = value; } }
        }

        public string STATTestResultFileExt
        {
            get { return this.STATPathInfo01.FileExt; }
            set
            {
                lock (this._lockObj)
                {
                    this.STATPathInfo01.FileExt = value;
                    this.STATPathInfo02.FileExt = value;
                    this.STATPathInfo03.FileExt = value;
                }
            }
        }


        public bool IsEnableSaveRelativeSpectrum
        {
            get { return this.RelSpcPathInfo01.EnablePath; }
            set { lock (this._lockObj) { this.RelSpcPathInfo01.EnablePath = value; } }
        }

        public bool IsEnableSaveRelativeSpectrum02
        {
            get { return this.RelSpcPathInfo02.EnablePath; }
            set { lock (this._lockObj) { this.RelSpcPathInfo02.EnablePath = value; } }
        }

        public bool IsEnableSaveRelativeSpectrum03
        {
            get { return this.RelSpcPathInfo03.EnablePath; }
            set { lock (this._lockObj) { this.RelSpcPathInfo03.EnablePath = value; } }
        }

        public bool IsEnableSaveAbsoluteSpectrum
        {
            get { return this.AbsSpcPathInfo01.EnablePath; }
            set { lock (this._lockObj) { this.AbsSpcPathInfo01.EnablePath = value; } }
        }

        public bool IsEnableSaveAbsoluteSpectrum02
        {
            get { return this.AbsSpcPathInfo02.EnablePath; }
            set { lock (this._lockObj) { this.AbsSpcPathInfo02.EnablePath = value; } }
        }

        public bool IsEnableSaveAbsoluteSpectrum03
        {
            get { return this.AbsSpcPathInfo03.EnablePath; }
            set { lock (this._lockObj) { this.AbsSpcPathInfo03.EnablePath = value; } }
        }

        public bool IsEnableSaveDarkSpectrum
        {
            get { return this._isEnableSaveDarkSpectrum; }
            set { lock (this._lockObj) { this._isEnableSaveDarkSpectrum = value; } }
        }

        public string AbsoluteSpectrumPath
        {
            get { return this.AbsSpcPathInfo01.TestResultPath; }
            set { lock (this._lockObj) { this.AbsSpcPathInfo01.TestResultPath = value; } }
        }

        public string AbsoluteSpectrumPath02
        {
            get { return this.AbsSpcPathInfo02.TestResultPath; }
            set { lock (this._lockObj) { this.AbsSpcPathInfo02.TestResultPath = value; } }
        }

        public string AbsoluteSpectrumPath03
        {
            get { return this.AbsSpcPathInfo03.TestResultPath; }
            set { lock (this._lockObj) { this.AbsSpcPathInfo03.TestResultPath = value; } }
        }

        public string RelativeSpectrumPath
        {
            get { return this.RelSpcPathInfo01.TestResultPath; }
            set { lock (this._lockObj) { this.RelSpcPathInfo01.TestResultPath = value; } }
        }

        public string RelativeSpectrumPath02
        {
            get { return this.RelSpcPathInfo02.TestResultPath; }
            set { lock (this._lockObj) { this.RelSpcPathInfo02.TestResultPath = value; } }
        }

        public string RelativeSpectrumPath03
        {
            get { return this.RelSpcPathInfo03.TestResultPath; }
            set { lock (this._lockObj) { this.RelSpcPathInfo03.TestResultPath = value; } }
        }

        public ETesterResultCreatFolderType SptRelCreatFolderType
        {
            get { return this.RelSpcPathInfo01.FolderType; }
            set
            {
                lock (this._lockObj)
                {
                    this.RelSpcPathInfo01.FolderType = value;
                    this.RelSpcPathInfo02.FolderType = value;
                    this.RelSpcPathInfo03.FolderType = value;
                }
            }
        }
        public ETesterResultCreatFolderType SptAbsCreatFolderType
        {
            get { return this.AbsSpcPathInfo01.FolderType; }
            set
            {
                lock (this._lockObj)
                {
                    this.AbsSpcPathInfo01.FolderType = value;
                    this.AbsSpcPathInfo02.FolderType = value;
                    this.AbsSpcPathInfo03.FolderType = value;
                }
            }
        }
        public uint SaveSpectrumMaxCount
        {
            get { return this._saveSpectrumMaxCount; }
            set { lock (this._lockObj) { this._saveSpectrumMaxCount = value; } }
        }



        public bool IsEnableSaveLIVData
        {
            get { return LIVPathInfo01.EnablePath; }
            set { lock (this._lockObj) { LIVPathInfo01.EnablePath = value; } }
        }

        public bool IsEnableSaveLIVDataPath02
        {
            get { return LIVPathInfo02.EnablePath; }
            set { lock (this._lockObj) { LIVPathInfo02.EnablePath = value; } }
        }

        public bool IsEnableSaveLIVDataPath03
        {
            get { return LIVPathInfo03.EnablePath; }
            set { lock (this._lockObj) { LIVPathInfo03.EnablePath = value; } }
        }

        public string LIVDataSavePath
        {
            get { return this.LIVPathInfo01.PathName; }
            set { lock (this._lockObj) { this.LIVPathInfo01.PathName = value; } }
        }

        public string LIVDataSavePath02
        {
            get { return this.LIVPathInfo02.PathName; }
            set { lock (this._lockObj) { this.LIVPathInfo02.PathName = value; } }
        }

        public string LIVDataSavePath03
        {
            get { return this.LIVPathInfo03.PathName; }
            set { lock (this._lockObj) { this.LIVPathInfo03.PathName = value; } }
        }


        public ETesterResultCreatFolderType LIVCreatFolderType
        {
            get { return this.LIVPathInfo01.FolderType; }
            set { lock (this._lockObj) { this.LIVPathInfo01.FolderType = value; } }
        }
        public ETesterResultCreatFolderType LIVCreatFolderType02
        {
            get { return this.LIVPathInfo02.FolderType; }
            set { lock (this._lockObj) { this.LIVPathInfo02.FolderType = value; } }
        }
        public ETesterResultCreatFolderType LIVCreatFolderType03
        {
            get { return this.LIVPathInfo03.FolderType; }
            set { lock (this._lockObj) { this.LIVPathInfo03.FolderType = value; } }
        }

        public PathInfo MapPathInfo
        {
            get { return this._mapPathInfo; }
            set { lock (this._lockObj) { this._mapPathInfo = value; } }
        }
        public PathInfo ProductPathInfo
        {
            get { return this._productPathInfo; }
            set { lock (this._lockObj) { this._productPathInfo = value; } }
        }
        public PathInfo ProductPathInfo2
        {
            get { return this._productPathInfo2; }
            set { lock (this._lockObj) { this._productPathInfo2 = value; } }
        }
        public PathInfo CoefTablePathInfo
        {
            get { return this._coefTablePathInfo; }
            set { lock (this._lockObj) { this._coefTablePathInfo = value; } }
        }
        public PathInfo CoefBackupPathInfo
        {
            get { return this._coefBackupPathInfo; }
            set { lock (this._lockObj) { this._coefBackupPathInfo = value; } }
        }
        
        public string MapPath//not using
        {
            get { return this._mapPathInfo.TestResultPath; }
            set { lock (this._lockObj) { this._mapPathInfo.TestResultPath = value; } }
        }
        public bool IsEnableSaveMap//not using
        {
            get { return this._mapPathInfo.EnablePath; }
            set { lock (this._lockObj) { this._mapPathInfo.EnablePath = value; } }
        }


        [TypeConverter(typeof(ExpandableObjectConverter))]
        public PathInfo[] PathInfoArr
        { set; get; }

        public PathInfo UIMapPathInfo
        { set; get; }

        public PathInfo MergeFilePath
        { set; get; }

        public PathInfo LaserPowerLogPath
        { set; get; }
        public string OptoTechKeyInDataPath
        { set; get; }
        //PathInfo


        #endregion

        #region >>public method<<
        public virtual void Overwrite( OutputPathManager obj)
        {

            this.OutPathInfo01 = (obj.OutPathInfo01.Clone() as PathInfo);
            this.OutPathInfo02 = (obj.OutPathInfo02.Clone() as PathInfo);
            this.OutPathInfo03 = (obj.OutPathInfo03.Clone() as PathInfo);
            this.ManualOutPathInfo01 = (obj.ManualOutPathInfo01.Clone() as PathInfo);
            this.ManualOutPathInfo02 = (obj.ManualOutPathInfo02.Clone() as PathInfo);
            this.ManualOutPathInfo03 = (obj.ManualOutPathInfo03.Clone() as PathInfo);

            this.SweepPathInfo01 = (obj.SweepPathInfo01.Clone() as PathInfo);

            // WAF PATH SETTING
            this.WAFPathInfo01 = (obj.WAFPathInfo01.Clone() as PathInfo);
            this.WAFPathInfo02 = (obj.WAFPathInfo02.Clone() as PathInfo);
            this.WAFPathInfo03 = (obj.WAFPathInfo03.Clone() as PathInfo);

            // Statistic PATH SETTING
            this.STATPathInfo01 = (obj.STATPathInfo01.Clone() as PathInfo);
            this.STATPathInfo02 = (obj.STATPathInfo02.Clone() as PathInfo);
            this.STATPathInfo03 = (obj.STATPathInfo03.Clone() as PathInfo);

            this._fileNameFormatPresent = obj._fileNameFormatPresent;


            this.AbsSpcPathInfo01 = (obj.AbsSpcPathInfo01.Clone() as PathInfo);
            this.AbsSpcPathInfo02 = (obj.AbsSpcPathInfo02.Clone() as PathInfo);
            this.AbsSpcPathInfo03 = (obj.AbsSpcPathInfo03.Clone() as PathInfo);
            this.RelSpcPathInfo01 = (obj.RelSpcPathInfo01.Clone() as PathInfo);
            this.RelSpcPathInfo02 = (obj.RelSpcPathInfo02.Clone() as PathInfo);
            this.RelSpcPathInfo03 = (obj.RelSpcPathInfo03.Clone() as PathInfo);

            this._isEnableSaveDarkSpectrum = obj._isEnableSaveDarkSpectrum;
            this._saveSpectrumMaxCount = obj._saveSpectrumMaxCount;

            this.LIVPathInfo01 = (obj.LIVPathInfo01.Clone() as PathInfo);
            this.LIVPathInfo02 = (obj.LIVPathInfo02.Clone() as PathInfo);
            this.LIVPathInfo03 = (obj.LIVPathInfo03.Clone() as PathInfo);

            int arrLength = obj.PathInfoArr == null ? 0 : obj.PathInfoArr.Length;
            if (arrLength == 0)
            {
                obj.PathInfoArr = new PathInfo[20];
                arrLength = 20;
            }

            if (obj.PathInfoArr != null)
            {
                for (int i = 0; i < this.PathInfoArr.Length && i < arrLength; ++i)
                {
                    this.PathInfoArr[i] = (obj.PathInfoArr[i].Clone() as PathInfo);
                }
            }


            this._mapPathInfo = (obj._mapPathInfo.Clone() as PathInfo);

            this._productPathInfo = (obj._productPathInfo.Clone() as PathInfo);

            this._productPathInfo2 = (obj._productPathInfo2.Clone() as PathInfo);

            this._coefTablePathInfo = (obj._coefTablePathInfo.Clone() as PathInfo);

            this._coefBackupPathInfo = (obj._coefBackupPathInfo.Clone() as PathInfo);

            this.UIMapPathInfo = (obj.UIMapPathInfo.Clone() as PathInfo);

            this.MergeFilePath = (obj.MergeFilePath.Clone() as PathInfo);

            this.LaserPowerLogPath=(obj.LaserPowerLogPath.Clone() as PathInfo);

            this.OptoTechKeyInDataPath = obj.OptoTechKeyInDataPath;
           
        }
        
        public virtual object Clone()
        {
            OutputPathManager obj = new OutputPathManager();

            //Overwrite(ref obj);

            obj._lockObj = new object();
            return (object)obj;
        }

        #endregion
    }

    [TypeConverter(typeof(PathInfoConverter))]
    [Serializable]
    public class PathInfo :ICloneable
    {
        #region
        public bool EnablePath { set; get; }

        [EditorAttribute(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string TestResultPath { set; get; }

        public ETesterResultCreatFolderType FolderType { set; get; }

        public string FileExt { set; get; }
        [BrowsableAttribute(false)]
        public string PathName { set; get; }
        #endregion

        #region
        public PathInfo()
        {
            EnablePath = false;
            TestResultPath = Constants.Paths.MPI_TEMP_DIR;
            FolderType = ETesterResultCreatFolderType.None;
            FileExt = "csv";
            PathName = "Path";
        }
        public PathInfo(bool enable,string path,ETesterResultCreatFolderType fType = ETesterResultCreatFolderType.None,string fileExt = "csv")
        {
            EnablePath = enable;
            TestResultPath = path;
            FolderType = fType;
            FileExt = fileExt;
        }

        #endregion

        public object Clone()
        {
            PathInfo obj = new PathInfo();
            //obj = this.MemberwiseClone() as PathInfo;
            obj.FileExt = this.FileExt;
            obj.TestResultPath = this.TestResultPath;
            obj.PathName = this.PathName;
            obj.EnablePath = this.EnablePath;
            obj.FolderType = this.FolderType;
            return obj;
        }
    }

    internal class PathInfoConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context,
                               System.Globalization.CultureInfo culture,
                               object value, Type destType)
        {
            if (destType == typeof(string) && value is PathInfo)
            {
                PathInfo item = (PathInfo)value;
                return item.PathName;
            }
            return base.ConvertTo(context, culture, value, destType);
        }
    }
}
