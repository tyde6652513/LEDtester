using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;
using System.IO;
using MPI.Tester.TestServer;
using MPI.Tester.Maths;
using MPI.Tester.TestKernel;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Report
{
    public abstract partial class ReportBase
    {
        #region >>>Spectrometer Relative  <<<

        private StreamWriter _swSpR;

        private List<double> _lstRWL = new List<double>();

        private string _fileFullNameTmpSpR = "";

        private string _fileFullNameRepSpR = "";

        protected virtual EErrorCode RunCommandByUserSpR(EServerQueryCmd cmd)
        {
            switch (cmd)
            {
                case EServerQueryCmd.CMD_TESTER_START:
                    {
                        if (this._isAppend)
                        {
                            return EErrorCode.NONE;
                        }
                        else
                        {
                            return this.WriteReportHeadByUserSpR(); // *.Temp
                        }
                    }
                case EServerQueryCmd.CMD_TESTER_END:
                    {
                        EErrorCode code = this.RewriteReportByUserSpR();

                        if (code != EErrorCode.NONE)
                        {
                            return code;
                        }

                        return this.MoveFileToTargetByUseSpR(cmd);
                    }
                case EServerQueryCmd.CMD_TESTER_ABORT:
                    {
                        if (this.UISetting.IsAbortSaveFile)
                        {
                            EErrorCode code = this.RewriteReportByUserSpR();

                            if (code != EErrorCode.NONE)
                            {
                                return code;
                            }

                            return this.MoveFileToTargetByUseSpR(cmd);
                        }
                        else
                        {
                            return EErrorCode.NONE;
                        }
                    }
                default:
                    {
                        return EErrorCode.NONE;
                    }
            }
        }

        protected virtual EErrorCode WriteReportHeadByUserSpR()
        {
            if (!this.UISetting.IsEnableSaveRelativeSpectrum)
            {
                return EErrorCode.NONE;
            }

            _lstRWL = new List<double>();

            this.WriteLineSpR("Chip,Name,Wavelength");

            return EErrorCode.NONE;
        }

        protected virtual EErrorCode PushDataByUserSpR(Dictionary<string, double> data, bool isMS)
        {
            if (!this.UISetting.IsEnableSaveRelativeSpectrum)
            {
                return EErrorCode.NONE;
            }

            string col = data["COL"].ToString();

            string row = data["ROW"].ToString();

            string key = string.Format("C{0}R{1}", col, row);

            SpectrumDataSet sptSet = this.AcquireData.SpectrumDataSet;
            if (isMS)
            {

            }

            foreach (var spectrum in sptSet)
            {
                if (!spectrum.IsEnable)
                {
                    continue;
                }

                uint startWl = this.TesterSetting.OptiDevSetting.StartWavelength;
                uint endWl = this.TesterSetting.OptiDevSetting.EndWavelength;

                this._lstRWL.Clear();

                string nameStr = "," + spectrum.Name;

                string IntLine = key + nameStr + ",Intensity";

                for (int i = 0; i < spectrum.Wavelength.Length; i++)
                {
                    double wl = spectrum.Wavelength[i];

                    if (wl >= startWl && wl <= endWl)
                    {
                        this._lstRWL.Add(wl);

                        IntLine += "," + spectrum.Intensity[i].ToString();

                    }
                }

                this.WriteLineSpR(IntLine);
            }

            return EErrorCode.NONE;
        }

        protected virtual EErrorCode RewriteReportByUserSpR()
        {
            if (!this.UISetting.IsEnableSaveRelativeSpectrum)
            {
                return EErrorCode.NONE;
            }

            if (!File.Exists(FileFullNameTmpSpR))
            {
                return EErrorCode.NONE;
            }

            string title = "Chip,Name,Wavelength";

            if (this._lstRWL != null || this._lstRWL.Count > 0)
            {
                foreach (var item in this._lstRWL)
                {
                    title += "," + item.ToString();
                }
            }

            Dictionary<string, string> replaceData = new Dictionary<string, string>();

            replaceData.Add("Chip,Name,Wavelength", title);

            using (StreamReader sr = new StreamReader(this.FileFullNameTmpSpR, Encoding.Default))
            {
                using (StreamWriter sw = new StreamWriter(this.FileFullNameRepSpR))
                {
                    while (sr.Peek() >= 0)
                    {
                        string line = sr.ReadLine();

                        if (replaceData.ContainsKey(line))
                        {
                            line = replaceData[line];
                        }

                        sw.WriteLine(line);
                    }
                }
            }

            return EErrorCode.NONE;
        }

        protected virtual EErrorCode MoveFileToTargetByUseSpR(EServerQueryCmd cmd)
        {

            if (!this.UISetting.IsEnableSaveRelativeSpectrum)
            {
                return EErrorCode.NONE;
            }

            string outPath = this.UISetting.RelativeSpectrumPath;

            string fileNameWithoutExt = this.TestResultFileNameWithoutExt();

            //Abort
            if (cmd == EServerQueryCmd.CMD_TESTER_ABORT)
            {
                fileNameWithoutExt = fileNameWithoutExt + "_" + DateTime.Now.ToString("yyMMddhhmmss");
            }

            string fileNameWithExt = fileNameWithoutExt + ".rel";

            outPath = GetFullPathWithFolder(outPath, this.UISetting.SptRelCreatFolderType);

            string outputPathAndFile = Path.Combine(outPath, fileNameWithExt);

            MPIFile.CopyFile(this.FileFullNameRepSpR, outputPathAndFile);

            return EErrorCode.NONE;
        }

        protected void WriteLineSpR(string line)
        {
            if (this._swSpR != null)
            {
                this._swSpR.WriteLine(line);

                this._swSpR.Flush();
            }
        }

        protected void ReplaceReportSpR(Dictionary<string, string> replaceData)
        {
            if (File.Exists(this._fileFullNameTmpSpR))
            {
                this.ReplaceReportData(replaceData, this._fileFullNameTmpSpR, this._fileFullNameRepSpR);
            }
        }

        protected string FileFullNameRepSpR
        {
            get { return this._fileFullNameRepSpR; }
            set { this._fileFullNameRepSpR = value; }
        }

        protected string FileFullNameTmpSpR
        {
            get { return this._fileFullNameTmpSpR; }
            set { this._fileFullNameTmpSpR = value; }
        }
        #endregion

        #region >>>Spectrometer Absolute  <<<

        private StreamWriter _swSpA;

        private List<double> _lstAWL = new List<double>();

        private string _fileFullNameTmpSpA = "";

        private string _fileFullNameRepSpA = "";

        protected virtual EErrorCode RunCommandByUserSpA(EServerQueryCmd cmd)
        {
            switch (cmd)
            {
                case EServerQueryCmd.CMD_TESTER_START:
                    {
                        if (this._isAppend)
                        {
                            return EErrorCode.NONE;
                        }
                        else
                        {
                            return this.WriteReportHeadByUserSpA(); // *.Temp
                        }
                    }
                case EServerQueryCmd.CMD_TESTER_END:
                    {
                        EErrorCode code = this.RewriteReportByUserSpA();

                        if (code != EErrorCode.NONE)
                        {
                            return code;
                        }

                        return this.MoveFileToTargetByUseSpA(cmd);
                    }
                case EServerQueryCmd.CMD_TESTER_ABORT:
                    {
                        if (this.UISetting.IsAbortSaveFile)
                        {
                            EErrorCode code = this.RewriteReportByUserSpA();

                            if (code != EErrorCode.NONE)
                            {
                                return code;
                            }

                            return this.MoveFileToTargetByUseSpA(cmd);
                        }
                        else
                        {
                            return EErrorCode.NONE;
                        }
                    }
                default:
                    {
                        return EErrorCode.NONE;
                    }
            }
        }

        protected virtual EErrorCode WriteReportHeadByUserSpA()
        {
            if (!this.UISetting.IsEnableSaveAbsoluteSpectrum)
            {
                return EErrorCode.NONE;
            }

            _lstAWL = new List<double>();

            this.WriteLineSpA("Chip,Name,Wavelength");

            return EErrorCode.NONE;
        }

        protected virtual EErrorCode PushDataByUserSpA(Dictionary<string, double> data,bool isMS)
        {
            if (!this.UISetting.IsEnableSaveRelativeSpectrum)
            {
                return EErrorCode.NONE;
            }

            string col = data["COL"].ToString();

            string row = data["ROW"].ToString();

            string key = string.Format("C{0}R{1}", col, row);

            SpectrumDataSet sptSet = this.AcquireData.SpectrumDataSet;
            if (isMS)
            {
 
            }

            foreach (var spectrum in sptSet)
            {
                if (!spectrum.IsEnable)
                {
                    continue;
                }

                uint startWl = this.TesterSetting.OptiDevSetting.StartWavelength;
                uint endWl = this.TesterSetting.OptiDevSetting.EndWavelength;

                this._lstAWL.Clear();

                string nameStr = "," + spectrum.Name;

                string IntLine = key + nameStr + ",Intensity";

                for (int i = 0; i < spectrum.Wavelength.Length; i++)
                {
                    double wl = spectrum.Wavelength[i];

                    if (wl >= startWl && wl <= endWl)
                    {
                        this._lstAWL.Add(wl);

                        IntLine += "," + spectrum.Absoluate[i].ToString();
                    }
                }

                this.WriteLineSpA(IntLine);
            }

            return EErrorCode.NONE;
        }

        protected virtual EErrorCode RewriteReportByUserSpA()
        {
            if (!this.UISetting.IsEnableSaveRelativeSpectrum)
            {
                return EErrorCode.NONE;
            }

            if (!File.Exists(FileFullNameTmpSpA))
            {
                return EErrorCode.NONE;
            }

            string title = "Chip,Name,Wavelength";

            if (this._lstAWL != null || this._lstAWL.Count > 0)
            {
                foreach (var item in this._lstAWL)
                {
                    title += "," + item.ToString();
                }
            }

            Dictionary<string, string> replaceData = new Dictionary<string, string>();

            replaceData.Add("Chip,Name,Wavelength", title);

            using (StreamReader sr = new StreamReader(this.FileFullNameTmpSpA, Encoding.Default))
            {
                using (StreamWriter sw = new StreamWriter(this.FileFullNameRepSpA))
                {
                    while (sr.Peek() >= 0)
                    {
                        string line = sr.ReadLine();

                        if (replaceData.ContainsKey(line))
                        {
                            line = replaceData[line];
                        }

                        sw.WriteLine(line);
                    }
                }
            }

            return EErrorCode.NONE;
        }

        protected virtual EErrorCode MoveFileToTargetByUseSpA(EServerQueryCmd cmd)
        {

            if (!this.UISetting.IsEnableSaveAbsoluteSpectrum)
            {
                return EErrorCode.NONE;
            }

            string outPath = this.UISetting.AbsoluteSpectrumPath;

            string fileNameWithoutExt = this.TestResultFileNameWithoutExt();

            //Abort
            if (cmd == EServerQueryCmd.CMD_TESTER_ABORT)
            {
                fileNameWithoutExt = fileNameWithoutExt + "_" + DateTime.Now.ToString("yyMMddhhmmss");
            }

            string fileNameWithExt = fileNameWithoutExt + ".abs";

            outPath = GetFullPathWithFolder(outPath, this.UISetting.SptAbsCreatFolderType);

            string outputPathAndFile = Path.Combine(outPath, fileNameWithExt);

            MPIFile.CopyFile(this.FileFullNameRepSpA, outputPathAndFile);

            return EErrorCode.NONE;
        }

        protected void WriteLineSpA(string line)
        {
            if (this._swSpA != null)
            {
                this._swSpA.WriteLine(line);

                this._swSpA.Flush();
            }
        }

        protected void ReplaceReportSpA(Dictionary<string, string> replaceData)
        {
            if (File.Exists(this._fileFullNameTmpSpA))
            {
                this.ReplaceReportData(replaceData, this._fileFullNameTmpSpA, this._fileFullNameRepSpA);
            }
        }

        protected string FileFullNameRepSpA
        {
            get { return this._fileFullNameRepSpA; }
            set { this._fileFullNameRepSpA = value; }
        }

        protected string FileFullNameTmpSpA
        {
            get { return this._fileFullNameTmpSpA; }
            set { this._fileFullNameTmpSpA = value; }
        }
        #endregion




        #region >>private method<<
       

        #endregion
    }
}
