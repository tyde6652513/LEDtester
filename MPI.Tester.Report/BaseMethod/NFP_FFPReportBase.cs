using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;
using System.IO;
using MPI.Tester.TestServer;
using MPI.Tester.Maths;
using MPI.Tester.TestKernel;


using MPI.VCF.Imaging;
using MPI.VCF.Imaging.Buffer;
using MPI.VCF.Application.FieldPattern;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Report
{
    public abstract partial class ReportBase
    {
        #region >>>Sweep 01 ,NFP<<<

        protected virtual EErrorCode RunCommandByUserNFP01(EServerQueryCmd cmd)
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
                            return this.WriteReportHeadByUserNFP01(); // *.Temp
                        }
                    }
                case EServerQueryCmd.CMD_TESTER_END:
                    {
                        EErrorCode code = this.RewriteReportByUserNFP01();

                        if (code != EErrorCode.NONE)
                        {
                            return code;
                        }

                        return this.MoveFileToTargetByUseNFP01(cmd);
                    }
                case EServerQueryCmd.CMD_TESTER_ABORT:
                    {
                        if (this.UISetting.IsAbortSaveFile)
                        {
                            EErrorCode code = this.RewriteReportByUserNFP01();

                            if (code != EErrorCode.NONE)
                            {
                                return code;
                            }

                            return this.MoveFileToTargetByUseNFP01(cmd);
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

        protected virtual EErrorCode WriteReportHeadByUserNFP01()
        {

            return EErrorCode.NONE;
        }

        protected virtual EErrorCode PushDataByUserNFP01(Dictionary<string, double> data, bool isMS)
        {
            string pos = "_C(" + data["COL"].ToString() + ")_R(" + data["ROW"].ToString() + ")";

            if (this.UISetting.IsEnableSaveNFPDataPath)
            {
                if (this.AcquireData.NFPDataSet.DataList.Count != 0)
                {
                    NFPDataSet nfpSet = this.AcquireData.NFPDataSet;
                    if (isMS)
                    {
                        //livSet = 
                    }
                    foreach (NFPData nfpData in nfpSet.DataList)
                    {
                        if (EErrorCode.NONE != DefaultPushNFP(data,nfpData, pos, this.UISetting.NFPDataSavePath))
                        {
                            return EErrorCode.SaveFileFail;
                        }
                    }
                }
            }

            return EErrorCode.NONE;
        }

        protected virtual EErrorCode RewriteReportByUserNFP01()
        {
            return EErrorCode.NONE;
        }

        protected virtual EErrorCode MoveFileToTargetByUseNFP01(EServerQueryCmd cmd)
        {
            return EErrorCode.NONE;
        }

        protected EErrorCode DefaultPushNFP(Dictionary<string, double> data,NFPData nfpData, string pos, string outputPath)
        {
            if (!this.UISetting.IsEnableSaveNFPDataPath ||
                this.AcquireData.NFPDataSet.DataList == null||
                this.AcquireData.NFPDataSet.DataList.Count == 0)
            {
                return EErrorCode.NONE;
            }

            List<string[]> outData = new List<string[]>();


            if (!nfpData.IsEnable || nfpData.NFresult != null)
            {
                return EErrorCode.NONE;
            }

            StatisticsNumbersDistribution Intens= nfpData.NFresult.IntensityUniformity;
            PatternAnalysisResult<UInt32>[] PatternAnalysisResult = nfpData.NFresult.PatternAnalysisResult;
            float Ma_Avg = PatternAnalysisResult.Average(x => (float)x.AxisProfiles.Major.Points.Length);
            float Ma_Min = PatternAnalysisResult.Min(x => (float)x.AxisProfiles.Major.Points.Length);
            float Ma_Max = PatternAnalysisResult.Max(x => (float)x.AxisProfiles.Major.Points.Length);
            
            double MaSqrtDiff = PatternAnalysisResult.Select(val => (Math.Pow((double)((float)val.AxisProfiles.Major.Points.Length - Ma_Avg), 2))).Sum();
            double Ma_STD = Math.Sqrt(MaSqrtDiff / PatternAnalysisResult.Length);

            float Mi_Avg = PatternAnalysisResult.Average(x => (float)x.AxisProfiles.Minor.Points.Length);
            float Mi_Min = PatternAnalysisResult.Min(x => (float)x.AxisProfiles.Minor.Points.Length);
            float Mi_Max = PatternAnalysisResult.Max(x => (float)x.AxisProfiles.Minor.Points.Length);

            double MiSqrtDiff = PatternAnalysisResult.Select(val => (Math.Pow((double)((float)val.AxisProfiles.Minor.Points.Length - Mi_Avg), 2))).Sum();
            double Mi_STD = Math.Sqrt(MiSqrtDiff / PatternAnalysisResult.Length);

            float P_Avg = PatternAnalysisResult.Average(x => (float)x.AxisProfiles.Major.centerPeak.Value);
            float P_Min = PatternAnalysisResult.Min(x => (float)x.AxisProfiles.Major.centerPeak.Value);
            float P_Max = PatternAnalysisResult.Max(x => (float)x.AxisProfiles.Major.centerPeak.Value);

            double pSqrtDiff = PatternAnalysisResult.Select(val => (Math.Pow((double)((float)val.AxisProfiles.Major.centerPeak.Value - P_Avg), 2))).Sum();
            double p_STD = Math.Sqrt(pSqrtDiff / PatternAnalysisResult.Length);

            string[][] title = new string[][] { new string[] {"PosX", data["COL"].ToString(), "PosY", data["COL"].ToString(), data["ROW"].ToString() },
            new string[] {"Emitter", "", "DetectEmitter", nfpData.NFresult.PatternCount.ToString() , "NumberOfBadEmitter",nfpData.NFresult.IntensityUniformity.BadDieCount.ToString()},
            new string[] {"Item","AVG",	"MAX","Min","STDev"},
            new string[] {"Intensity",Intens.Mean.ToString() ,	Intens.Max.ToString(),Intens.Min.ToString(),Intens.StandardDeviation.ToString()},
            new string[] {"BeamDiameter Major",Ma_Avg.ToString(),Ma_Min.ToString(),Ma_Max.ToString(),Ma_STD.ToString()} ,//* resolution
            new string[] {"BeamDiameter Minor",Mi_Avg.ToString(),Mi_Min.ToString(),Mi_Max.ToString(),Mi_STD.ToString()} ,//* resolution
            new string[] {"PeakCount",P_Avg.ToString(),P_Min.ToString(),P_Max.ToString(),p_STD.ToString()},
            new string[] {""},
            new string[] {"NO.","Centroid_X","Centroid_Y","Total Intensity","Intensity deviation","D4σ_Major","D4σ_Minor","Peak Count"}};
            
            outData.AddRange(title);
            int counter = 1;
            foreach (var pData in PatternAnalysisResult)
            {
                string[] outStr = new string[] { 
                    counter .ToString(),
                pData.SpatialProperty.Centroid_x.ToString(),
                pData.SpatialProperty.Centroid_y.ToString(),
                pData.SpatialProperty.TotalIntensity.ToString(),
                ((double)pData.SpatialProperty.TotalIntensity - Intens.Mean).ToString(),
                pData.SpatialProperty.D4Sigma_Minor.ToString(),
                pData.SpatialProperty.D4Sigma_Minor.ToString(),
                pData.SpatialProperty.MaxIntensityPixel.ToString()};

                counter++;

                outData.Add(outStr);
            }

            string fileName = string.Empty;
            string fileNameWithoutExt = this.TestResultFileNameWithoutExt() + "_NF_";
            fileName = fileNameWithoutExt + pos + ".csv";

            string fileAndPath = Path.Combine(outputPath, fileName);

            if (!CSVUtil.WriteCSV(fileAndPath, outData))
            {
                Console.WriteLine("[ReportBase], DefaultPushNFPSweep(),outputPath = " + outputPath + " fail.");

                return EErrorCode.SaveFileFail;
            }


            return EErrorCode.NONE;
        }

        #endregion

        #region >>>Sweep 01 ,FFP<<<

        protected virtual EErrorCode RunCommandByUserFFP01(EServerQueryCmd cmd)
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
                            return this.WriteReportHeadByUserFFP01(); // *.Temp
                        }
                    }
                case EServerQueryCmd.CMD_TESTER_END:
                    {
                        EErrorCode code = this.RewriteReportByUserFFP01();

                        if (code != EErrorCode.NONE)
                        {
                            return code;
                        }

                        return this.MoveFileToTargetByUseFFP01(cmd);
                    }
                case EServerQueryCmd.CMD_TESTER_ABORT:
                    {
                        if (this.UISetting.IsAbortSaveFile)
                        {
                            EErrorCode code = this.RewriteReportByUserFFP01();

                            if (code != EErrorCode.NONE)
                            {
                                return code;
                            }

                            return this.MoveFileToTargetByUseFFP01(cmd);
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

        protected virtual EErrorCode WriteReportHeadByUserFFP01()
        {

            return EErrorCode.NONE;
        }

        protected virtual EErrorCode PushDataByUserFFP01(Dictionary<string, double> data, bool isMS)
        {
            string pos = "_C(" + data["COL"].ToString() + ")_R(" + data["ROW"].ToString() + ")";

            if (this.UISetting.IsEnableSaveFFPDataPath)
            {
                if (this.AcquireData.FFPDataSet.DataList.Count != 0)
                {
                    FFPDataSet FFPSet = this.AcquireData.FFPDataSet;
                    if (isMS)
                    {
                        //livSet = 
                    }
                    foreach (FFPData FFPData in FFPSet.DataList)
                    {
                        if (EErrorCode.NONE != DefaultPushFFP(data, FFPData, pos, this.UISetting.FFPDataSavePath))
                        {
                            return EErrorCode.SaveFileFail;
                        }
                    }
                }
            }

            return EErrorCode.NONE;
        }

        protected virtual EErrorCode RewriteReportByUserFFP01()
        {
            return EErrorCode.NONE;
        }

        protected virtual EErrorCode MoveFileToTargetByUseFFP01(EServerQueryCmd cmd)
        {
            return EErrorCode.NONE;
        }

        protected EErrorCode DefaultPushFFP(Dictionary<string, double> data, FFPData FFPData, string pos, string outputPath)
        {
            if (!this.UISetting.IsEnableSaveFFPDataPath ||
    this.AcquireData.FFPDataSet.DataList == null ||
    this.AcquireData.FFPDataSet.DataList.Count == 0)
            {
                return EErrorCode.NONE;
            }

            List<string[]> outData = new List<string[]>();


            if (!FFPData.IsEnable || FFPData.FFresult != null)
            {
                return EErrorCode.NONE;
            }

            //FFPData.FFresult

            //StatisticsNumbersDistribution Intens = FFPData.NFresult.IntensityUniformity;
            //PatternAnalysisResult<UInt32>[] PatternAnalysisResult = FFPData.NFresult.PatternAnalysisResult;
            //float Ma_Avg = PatternAnalysisResult.Average(x => (float)x.AxisProfiles.Major.Points.Length);
            //float Ma_Min = PatternAnalysisResult.Min(x => (float)x.AxisProfiles.Major.Points.Length);
            //float Ma_Max = PatternAnalysisResult.Max(x => (float)x.AxisProfiles.Major.Points.Length);

            //double MaSqrtDiff = PatternAnalysisResult.Select(val => (Math.Pow((double)((float)val.AxisProfiles.Major.Points.Length - Ma_Avg), 2))).Sum();
            //double Ma_STD = Math.Sqrt(MaSqrtDiff / PatternAnalysisResult.Length);

            //float Mi_Avg = PatternAnalysisResult.Average(x => (float)x.AxisProfiles.Minor.Points.Length);
            //float Mi_Min = PatternAnalysisResult.Min(x => (float)x.AxisProfiles.Minor.Points.Length);
            //float Mi_Max = PatternAnalysisResult.Max(x => (float)x.AxisProfiles.Minor.Points.Length);

            //double MiSqrtDiff = PatternAnalysisResult.Select(val => (Math.Pow((double)((float)val.AxisProfiles.Minor.Points.Length - Mi_Avg), 2))).Sum();
            //double Mi_STD = Math.Sqrt(MiSqrtDiff / PatternAnalysisResult.Length);

            //float P_Avg = PatternAnalysisResult.Average(x => (float)x.AxisProfiles.Major.centerPeak.Value);
            //float P_Min = PatternAnalysisResult.Min(x => (float)x.AxisProfiles.Major.centerPeak.Value);
            //float P_Max = PatternAnalysisResult.Max(x => (float)x.AxisProfiles.Major.centerPeak.Value);

            //double pSqrtDiff = PatternAnalysisResult.Select(val => (Math.Pow((double)((float)val.AxisProfiles.Major.centerPeak.Value - P_Avg), 2))).Sum();
            //double p_STD = Math.Sqrt(pSqrtDiff / PatternAnalysisResult.Length);

            string[][] title = new string[][] { new string[] {"PosX", data["COL"].ToString(), "PosY", data["COL"].ToString(), data["ROW"].ToString() },
            new string[] {"Degree","0",	"45","90","135"},
            //new string[] {"Divergence Angle",Intens.Mean.ToString() ,	Intens.Max.ToString(),Intens.Min.ToString(),Intens.StandardDeviation.ToString()},
            //new string[] {"DIP",Ma_Avg.ToString(),Ma_Min.ToString(),Ma_Max.ToString(),Ma_STD.ToString()} ,//* resolution

            new string[] {""},
            new string[] {"0degree_X","0degree_Y","45degree_X","45degree_Y","90degree_X","90degree_Y","135degree_X","135degree_Y"}};



            //outData.AddRange(title);
            //int counter = 1;
            //foreach (var pData in PatternAnalysisResult)
            //{
            //    string[] outStr = new string[] { 
            //        counter .ToString(),
            //    pData.SpatialProperty.Centroid_x.ToString(),
            //    pData.SpatialProperty.Centroid_y.ToString(),
            //    pData.SpatialProperty.TotalIntensity.ToString(),
            //    ((double)pData.SpatialProperty.TotalIntensity - Intens.Mean).ToString(),
            //    pData.SpatialProperty.D4Sigma_Minor.ToString(),
            //    pData.SpatialProperty.D4Sigma_Minor.ToString(),
            //    pData.SpatialProperty.MaxIntensityPixel.ToString()};

            //    counter++;

            //    outData.Add(outStr);
            //}

            ////foreach()



            string fileName = string.Empty;
            string fileNameWithoutExt = this.TestResultFileNameWithoutExt() + "_FF_";
            fileName = fileNameWithoutExt + pos + ".csv";

            string fileAndPath = Path.Combine(outputPath, fileName);

            if (!CSVUtil.WriteCSV(fileAndPath, outData))
            {
                Console.WriteLine("[ReportBase], DefaultPushFFPSweep(),outputPath = " + outputPath + " fail.");

                return EErrorCode.SaveFileFail;
            }


            return EErrorCode.NONE;
        }

        #endregion

        #region >>>NFP Image<<<

        protected virtual EErrorCode RunCommandByUserNFPImg01(EServerQueryCmd cmd)
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
                            return this.WriteReportHeadByUserNFPImg01(); // *.Temp
                        }
                    }
                case EServerQueryCmd.CMD_TESTER_END:
                    {
                        EErrorCode code = this.RewriteReportByUserNFPImg01();

                        if (code != EErrorCode.NONE)
                        {
                            return code;
                        }

                        return this.MoveFileToTargetByUseNFPImg01(cmd);
                    }
                case EServerQueryCmd.CMD_TESTER_ABORT:
                    {
                        if (this.UISetting.IsAbortSaveFile)
                        {
                            EErrorCode code = this.RewriteReportByUserNFPImg01();

                            if (code != EErrorCode.NONE)
                            {
                                return code;
                            }

                            return this.MoveFileToTargetByUseNFPImg01(cmd);
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

        protected virtual EErrorCode WriteReportHeadByUserNFPImg01()
        {

            return EErrorCode.NONE;
        }

        protected virtual EErrorCode PushDataByUserNFPImg01(Dictionary<string, double> data, bool isMS)
        {
            string pos = "_C(" + data["COL"].ToString() + ")_R(" + data["ROW"].ToString() + ")";

            if (this.UISetting.IsEnableSaveNFPDataPath)
            {
                if (this.AcquireData.NFPDataSet.DataList.Count != 0)
                {
                    NFPDataSet nfpSet = this.AcquireData.NFPDataSet;
                    if (isMS)
                    {
                        //livSet = 
                    }
                    foreach (NFPData nfpData in nfpSet.DataList)
                    {
                        if (EErrorCode.NONE != DefaultPushNFPImg(data, nfpData, pos, this.UISetting.NFPDataSavePath))
                        {
                            return EErrorCode.SaveFileFail;
                        }
                    }
                }
            }

            return EErrorCode.NONE;
        }

        protected virtual EErrorCode RewriteReportByUserNFPImg01()
        {
            return EErrorCode.NONE;
        }

        protected virtual EErrorCode MoveFileToTargetByUseNFPImg01(EServerQueryCmd cmd)
        {
            return EErrorCode.NONE;
        }

        protected EErrorCode DefaultPushNFPImg(Dictionary<string, double> data, NFPData nfpData, string pos, string outputPath)
        {
            if (!this.UISetting.IsEnableSaveNFPImgPath ||
                this.AcquireData.NFPDataSet.DataList == null ||
                this.AcquireData.NFPDataSet.DataList.Count == 0)
            {
                return EErrorCode.NONE;
            }

            List<string[]> outData = new List<string[]>();


            if (!nfpData.IsEnable || nfpData.Image != null)
            {
                return EErrorCode.NONE;
            }

            string fileName = string.Empty;
            string fileNameWithoutExt = this.TestResultFileNameWithoutExt() ;
            fileName = fileNameWithoutExt + pos + DateTime.Now.ToString("yyMMddHHMMss") + ".tif";

            string fileAndPath = Path.Combine(outputPath, fileName);

            try
            {
                nfpData.Image.SaveImage(fileAndPath);
            }
            catch (Exception e)
            {
                Console.WriteLine("[ReportBase], DefaultPushNFPImg(),Err:" + e.Message);
                return EErrorCode.SaveFileFail;
            }


            return EErrorCode.NONE;
        }

        #endregion

        #region >>>FFP Image<<<

        protected virtual EErrorCode RunCommandByUserFFPImg01(EServerQueryCmd cmd)
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
                            return this.WriteReportHeadByUserFFPImg01(); // *.Temp
                        }
                    }
                case EServerQueryCmd.CMD_TESTER_END:
                    {
                        EErrorCode code = this.RewriteReportByUserFFPImg01();

                        if (code != EErrorCode.NONE)
                        {
                            return code;
                        }

                        return this.MoveFileToTargetByUseFFPImg01(cmd);
                    }
                case EServerQueryCmd.CMD_TESTER_ABORT:
                    {
                        if (this.UISetting.IsAbortSaveFile)
                        {
                            EErrorCode code = this.RewriteReportByUserFFPImg01();

                            if (code != EErrorCode.NONE)
                            {
                                return code;
                            }

                            return this.MoveFileToTargetByUseFFPImg01(cmd);
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

        protected virtual EErrorCode WriteReportHeadByUserFFPImg01()
        {

            return EErrorCode.NONE;
        }

        protected virtual EErrorCode PushDataByUserFFPImg01(Dictionary<string, double> data, bool isMS)
        {
            string pos = "_C(" + data["COL"].ToString() + ")_R(" + data["ROW"].ToString() + ")";

            if (this.UISetting.IsEnableSaveFFPDataPath)
            {
                if (this.AcquireData.FFPDataSet.DataList.Count != 0)
                {
                    FFPDataSet FFPSet = this.AcquireData.FFPDataSet;
                    if (isMS)
                    {
                        //livSet = 
                    }
                    foreach (FFPData ffpData in FFPSet.DataList)
                    {
                        if (EErrorCode.NONE != DefaultPushFFPImg(data, ffpData, pos, this.UISetting.FFPDataSavePath))
                        {
                            return EErrorCode.SaveFileFail;
                        }
                    }
                }
            }

            return EErrorCode.NONE;
        }

        protected virtual EErrorCode RewriteReportByUserFFPImg01()
        {
            return EErrorCode.NONE;
        }

        protected virtual EErrorCode MoveFileToTargetByUseFFPImg01(EServerQueryCmd cmd)
        {
            return EErrorCode.NONE;
        }

        protected EErrorCode DefaultPushFFPImg(Dictionary<string, double> data, FFPData ffpData, string pos, string outputPath)
        {
            if (!this.UISetting.IsEnableSaveFFPImgPath ||
                this.AcquireData.FFPDataSet.DataList == null ||
                this.AcquireData.FFPDataSet.DataList.Count == 0)
            {
                return EErrorCode.NONE;
            }

            List<string[]> outData = new List<string[]>();


            if (!ffpData.IsEnable || ffpData.Image != null)
            {
                return EErrorCode.NONE;
            }

            string fileName = string.Empty;
            string fileNameWithoutExt = this.TestResultFileNameWithoutExt();
            fileName = fileNameWithoutExt + pos + DateTime.Now.ToString("yyMMddHHMMss") + ".tif";

            string fileAndPath = Path.Combine(outputPath, fileName);

            try
            {
                ffpData.Image.SaveImage(fileAndPath);
            }
            catch (Exception e)
            {
                Console.WriteLine("[ReportBase], DefaultPushFFPImg(),Err:" + e.Message);
                return EErrorCode.SaveFileFail;
            }


            return EErrorCode.NONE;
        }

        #endregion
    }
}
