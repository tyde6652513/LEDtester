using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Data;
using System.IO;
using MPI.Tester.MES;

namespace MPI.Tester.Gui
{
    class MESCtrl
    {
        public static EErrorCode LoadRecipe()
        {
            // 由Wafer ID 決定是否執行日校正的動作

            //if (DataCenter._uiSetting.IsEnableJudgeRunDailyCheck)
            //{
            //    DataCenter._uiSetting.IsRunDailyCheckMode = false;

            //    if (DataCenter._uiSetting.WaferNumber.ToUpper().Contains(DataCenter._uiSetting.RunDailyCheckingKeyWord) )
            //    {
            //        Console.WriteLine("[MESCtrl], RunDailyCheck Mode => wafer ID : " + DataCenter._uiSetting.WaferNumber.ToString());

            //        DataCenter._uiSetting.IsRunDailyCheckMode = true;
            //    }
            //    else
            //    {
            //        DataCenter._uiSetting.IsRunDailyCheckMode = false;
            //    }
            //}

            if (DataCenter._uiSetting.IsEnableRunMesSystem)
            {
                EErrorCode rtnCode;

                DataCenter._uiSetting.IsConverterTasksheet = false;

                string message = string.Empty;

                rtnCode = MESProcess.LoadRecipe(DataCenter._uiSetting, DataCenter._machineConfig, out message);

                Host._alarmDescribe.Append(message);

                if (rtnCode == EErrorCode.NONE)
                {
                    if (DataCenter.LoadTaskSheet(DataCenter._uiSetting.TaskSheetFileName))
                    {           
                        AppSystem.SetDataToSystem();

                        AppSystem.CheckMachineHW();

                        Host.UpdateDataToAllUIForm();
                    }
                    else
                    {
                        Console.WriteLine("[MESCtrl], LoadRecipe, " + EErrorCode.MES_LoadTaskError.ToString());

                        return EErrorCode.MES_LoadTaskError;
                    }
                }
                else
                {
                    Console.WriteLine("[MESCtrl], LoadRecipe, " + rtnCode.ToString());

                    Host.SetErrorCode(rtnCode);
                }
                return rtnCode;
            }

            return EErrorCode.NONE;
        }
    }



    //    public static EErrorCode MESRun()
    //    {
    //        if (DataCenter._uiSetting.IsEnableRunMesSystem==false)
    //        {
    //            return EErrorCode.NONE;
    //        }

    //        EErrorCode rtnCode;

    //        rtnCode = MESControl.MESFileOpen();

    //        if (rtnCode != EErrorCode.NONE)
    //            return rtnCode;

    //        rtnCode = MESControl.MESFileConvert();

    //        if (rtnCode != EErrorCode.NONE)
    //            return rtnCode;

    //        return EErrorCode.NONE;
    //    }

    //    public static EErrorCode MESFileOpen()
    //    {
    //        EUserID UserID = DataCenter._uiSetting.UserID;
    //        switch (UserID)
    //        {
    //            case EUserID.AquaLite:
    //                return AquaLiteOpen();
    //            case EUserID.Epileds:
    //                return EpiLEDsOpen();
    //            case EUserID.EnRayTek:
    //                return EnRayTekOpen();
    //            case EUserID.EPITOP:
    //                return EPITopOpen();
    //            default:
    //                return EErrorCode.NONE;
    //        }
    //    }

    //    public static EErrorCode MESFileConvert()
    //    {
    //        EUserID UserID = DataCenter._uiSetting.UserID;

    //        switch (UserID)
    //        {
    //            default:
    //                return EErrorCode.NONE;
    //        }
    //    }

    //    private static EErrorCode AquaLiteOpen()
    //    {
    //        string serverConditonFullPath = Path.Combine(DataCenter._uiSetting.MESPath, "WATestCondition.CSV");
    //        string loaclConditonFullPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, "WATestCondition.CSV");

    //        DriveInfo driveInfo = new DriveInfo(serverConditonFullPath);

    //        if (!driveInfo.IsReady)
    //        {
    //            return EErrorCode.MES_CondDataNotExist;
    //        }

    //        if (File.Exists(serverConditonFullPath) == false)
    //            return EErrorCode.MES_CondDataNotExist;

    //        if (DataCenter._uiSetting.WaferNumber.Length < 5)
    //            return EErrorCode.MES_CondDataNotExist;

    //        if (File.Exists(serverConditonFullPath))
    //        {
    //            if (File.Exists(loaclConditonFullPath))
    //            {
    //                File.Delete(loaclConditonFullPath);
    //            }
    //            File.Copy(serverConditonFullPath, loaclConditonFullPath);
    //        }

    //        //Ex: QC2FC1202-885_RP-H222904 -> Get "FC"
    //       // string fileKey = DataCenter._uiSetting.Barcode.Substring(0, 2);

    //       // string fileKey = DataCenter._uiSetting.Barcode.Remove(3);

    //        List<string[]> FileNames = CSVUtil.ReadCSV(loaclConditonFullPath);

    //        string targetfileName = String.Empty;

    //        foreach (string[] key in FileNames)
    //        {
    //            if (key[0] == "SpecID" || DataCenter._uiSetting.WaferNumber.Length < key[0].Length)
    //                continue;

    //            string fileKey = DataCenter._uiSetting.WaferNumber.Remove(key[0].Length);

    //            if (fileKey == key[0])
    //            {
    //                targetfileName = key[1];
    //            }
    //        }

    //        if (targetfileName == String.Empty)
    //        {
    //            if (DataCenter._uiSetting.WaferNumber.Contains("GLD") || DataCenter._uiSetting.WaferNumber.Contains("Golden"))
    //            {
    //                return EErrorCode.NONE;
    //            }
    //            else
    //            {
    //                return EErrorCode.MES_CondDataNotExist;
    //            }
    //        }
              
    //        if (DataCenter.LoadTaskSheet(targetfileName) == false)
    //            return EErrorCode.MES_LoadTaskError;

    //        return EErrorCode.NONE;
    //    }

    //    private static EErrorCode EpiLEDsOpen()
    //    {
    //        // Copy File From MES
    //        DataCenter._uiSetting.MESOpenFileName = DataCenter._uiSetting.WaferNumber;
           
    //        string serverMesPathAndFile = Path.Combine(DataCenter._uiSetting.MESPath, DataCenter._uiSetting.MESOpenFileName + ".sys");
    //        string localMesPathAndFile = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, DataCenter._uiSetting.MESOpenFileName + ".sys");

    //        if (File.Exists(serverMesPathAndFile))
    //        {
    //            if (File.Exists(localMesPathAndFile))
    //            {
    //                File.Delete(localMesPathAndFile);
    //            }
    //            File.Copy(serverMesPathAndFile, localMesPathAndFile);
    //        }
    //        else
    //        {
    //            return EErrorCode.MES_CondDataNotExist;
    //        }
    //        // Open Local File 
    //        List<string[]> mesFile = CSVUtil.ReadCSV(localMesPathAndFile);

    //        string testRecipeFileNameExt = ".rm2";
    //        //
    //        string mesProductFileName = "";
    //        string mesCaliFileName = "";
    //        string mesBinFileName = "";
    //        string mesMapFileName = "";
    //        int mesIsSetOutputPath = 0;
    //        string mesOutputPath = "";
    //        //transform File Name
    //        string WaferID = mesFile[0][0];
    //        string LotID = mesFile[0][1];
    //        string pcs = mesFile[0][2];
    //        string productType = mesFile[0][3];
    //        string recipeFileName = mesFile[0][4];
    //        string OPName = mesFile[0][5];

    //        DataCenter._uiSetting.WaferNumber = WaferID;
    //        DataCenter._uiSetting.LotNumber = LotID;
    //        DataCenter._uiSetting.OperatorName = OPName;
    //        DataCenter._uiSetting.ProductType = productType;
    //        DataCenter._uiSetting.RM2FileName = recipeFileName;
    //        //pcs
    //        int waferpcs = 0;
    //        int.TryParse(pcs, out waferpcs);
    //        DataCenter._uiSetting.WaferPcs = waferpcs;
    //        //

    //        recipeFileName = recipeFileName + testRecipeFileNameExt;

    //        string serverRecipeFileName = Path.Combine(DataCenter._uiSetting.AutoRm2Path, recipeFileName);
    //        string localRecipePathAndFile = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, recipeFileName);

    //        if (File.Exists(serverRecipeFileName))
    //        {
    //            if (File.Exists(localRecipePathAndFile))
    //            {
    //                File.Delete(localRecipePathAndFile);
    //            }
    //            File.Copy(serverRecipeFileName, localRecipePathAndFile);
    //        }
    //        else
    //        {
    //            DataCenter._uiSetting.RM2FileName = "";
    //        }

    //        if (File.Exists(localRecipePathAndFile))
    //        {
    //            List<string[]> recipeFile = CSVUtil.ReadCSV(localRecipePathAndFile);
    //            //Calib
    //            mesCaliFileName = recipeFile[0][0];
    //            mesCaliFileName = mesCaliFileName.Substring(mesCaliFileName.IndexOf("=") + 1);
    //            // ts
    //            mesProductFileName = recipeFile[1][0];
    //            mesProductFileName = mesProductFileName.Substring(mesProductFileName.IndexOf("=") + 1);
    //            // bin
    //            mesBinFileName = recipeFile[3][0];
    //            mesBinFileName = mesBinFileName.Substring(mesBinFileName.IndexOf("=") + 1);
    //            // map
    //            mesMapFileName = recipeFile[4][0];
    //            mesMapFileName = mesMapFileName.Substring(mesMapFileName.IndexOf("=") + 1);
    //            //
    //            string isSetoutputPath = "";
    //            isSetoutputPath = recipeFile[6][0];
    //            isSetoutputPath = isSetoutputPath.Substring(isSetoutputPath.IndexOf("=") + 1);
    //            int.TryParse(isSetoutputPath, out mesIsSetOutputPath);
    //            mesOutputPath = recipeFile[7][0];
    //            mesOutputPath = mesOutputPath.Substring(mesOutputPath.IndexOf("=") + 1);
    //        }
    //        File.Delete(localRecipePathAndFile);

    //        mesProductFileName = mesProductFileName.Replace(".mc2", "");

    //        if (DataCenter.LoadTaskSheet(mesProductFileName) == false)
    //        {
    //            return EErrorCode.MES_LoadCalibFileError;
    //        }

    //        if (mesCaliFileName != "")
    //        {
    //            DataCenter._uiSetting.ImportCalibrateFileName = mesCaliFileName.Replace(".cal", "");
    //            DataCenter.ImportCalibrateData(DataCenter._uiSetting.ProductPath02, DataCenter._uiSetting.ImportCalibrateFileName + ".cal");
    //        }
    //        else
    //        {
    //            return EErrorCode.MES_LoadCalibFileError;
    //        }

    //        if (Host._UIErrorCode != EErrorCode.NONE)
    //        {
    //            return Host._UIErrorCode;
    //        }

    //        // C&M rm2. File 
    //        //
    //        //CalFile=R350BE.cal
    //        //MapSetupFile=M0NR35000000000000000CA.mc2
    //        //MapGradeFile=R350BE.gc2
    //        //MapSortFile=LED.sr2
    //        //chkCalPath= 0
    //        //CalPath=C:\Program Files\LED Auto Tester\Setup\
    //        //chkMapSetupPath= 1
    //        //MapSetupPath=F:\Probe_Para\setup\
    //        //chkMapGradePath= 0
    //        //MapGradePath=F:\Probe_Para\setup\
    //        //chkMapSortPath= 0
    //        //MapSortPath=C:\Program Files\LED Auto Tester\Setup
    //        return EErrorCode.NONE;
    //    }

    //    private static EErrorCode EPITopOpen()
    //    {
    //        string serverConditonFullPath = Path.Combine(DataCenter._uiSetting.MESPath, "EPItopTestCondition.CSV");
    //        string loaclConditonFullPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, "EPItopTestCondition.CSV");

    //        DriveInfo driveInfo = new DriveInfo(serverConditonFullPath);

    //        if (!driveInfo.IsReady)
    //        {
    //            return EErrorCode.MES_ServerConnectFail;
    //        }

    //        if (File.Exists(serverConditonFullPath) == false)
    //            return EErrorCode.MES_CondDataNotExist;

    //        if (File.Exists(serverConditonFullPath))
    //        {
    //            if (File.Exists(loaclConditonFullPath))
    //            {
    //                File.Delete(loaclConditonFullPath);
    //            }
    //            File.Copy(serverConditonFullPath, loaclConditonFullPath);
    //        }

    //        try
    //        {
    //            //Ex: C23B02C01333#15-A2023791Q -> Get "C01333"
    //            List<string[]> FileNames = CSVUtil.ReadCSV(loaclConditonFullPath);
    //            string testerRecipefileName = String.Empty;
    //            string proberRecipefileName = String.Empty;
    //            string reProberRecipeName = String.Empty;
    //            string barcode = DataCenter._uiSetting.Barcode;        

    //           // 6080 Auto Load Recipe Rule 
    //           // TestCondID,TestSpecName,ProberSpecName,REProberSpecName
    //           // PAA,09A-B-MCD,09A-B-MCD,09A-B-MCD-R
    //           // PAB,09A-B-MCD,09B-B-MCD,09B-B-MCD-R
    //           // PAD,W-15A-MW,W-15A-MW,W-15A-MW-R

    //            string targetKey = barcode.Substring(0, 3);

    //            foreach (string[] key in FileNames)
    //            {
    //                if (key[0] == targetKey)
    //                {
    //                    if (key.Length >= 3)
    //                    {
    //                        testerRecipefileName = key[1];
    //                        proberRecipefileName = key[2];
    //                        reProberRecipeName = key[3];
    //                        break;
    //                    }
    //                }
    //            }

    //            //========================
    //            // Check Is Square Sotert Page Mode
    //            //========================
    //            bool isSquareSortPageMode = false;
    //            char[] cc = barcode.ToCharArray();

    //            if (cc[3] == '-')
    //            {
    //                isSquareSortPageMode = true;
    //                DataCenter._uiSetting.ProberRecipeName = reProberRecipeName;
    //                GlobalData.ProberRecipeName = reProberRecipeName;
    //            }
    //            else
    //            {
    //                isSquareSortPageMode = false;
    //                DataCenter._uiSetting.ProberRecipeName = proberRecipefileName;
    //                GlobalData.ProberRecipeName = proberRecipefileName;
    //            }

    //            // Check is Daily Check Mode

    //            if (barcode.ToLower().Contains("gld"))
    //            {
    //                DataCenter._uiSetting.IsRunDailyCheckMode = true;
    //            }
    //            else
    //            {
    //                DataCenter._uiSetting.IsRunDailyCheckMode = false;
    //            }

    //            // recipe =null 

    //            if (testerRecipefileName == String.Empty)
    //            {
    //                if (DataCenter._uiSetting.IsRunDailyCheckMode)
    //                {
    //                    return EErrorCode.NONE;
    //                }
    //                else
    //                {
    //                    return EErrorCode.MES_TargetRecipeNoExist;
    //                }
    //            }

    //            if (DataCenter.LoadTaskSheet(testerRecipefileName) == false)
    //                return EErrorCode.MES_LoadTaskError;

    //            return EErrorCode.NONE;
    //        }
    //        catch
    //        {
    //            return EErrorCode.MES_BarcodeError;
    //        }


    //    }

    //    private static EErrorCode EnRayTekOpen()
    //    {
    //        string serverConditonFullPath = Path.Combine(DataCenter._uiSetting.MESPath, "EnrayTestCondition.CSV");
    //        string loaclConditonFullPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, "EnrayTestCondition.CSV");

    //        DriveInfo driveInfo = new DriveInfo(serverConditonFullPath);

    //        if (!driveInfo.IsReady)
    //        {
    //            return EErrorCode.MES_ServerConnectFail;
    //        }

    //        if (File.Exists(serverConditonFullPath) == false)
    //            return EErrorCode.MES_CondDataNotExist;

    //        if (DataCenter._uiSetting.Barcode.Length < 8)
    //            return EErrorCode.MES_BarcodeError;

    //        if (File.Exists(serverConditonFullPath))
    //        {
    //            if (File.Exists(loaclConditonFullPath))
    //            {
    //                File.Delete(loaclConditonFullPath);
    //            }
    //            File.Copy(serverConditonFullPath, loaclConditonFullPath);
    //        }

    //        try
    //        {
    //            //Ex: C23B02C01333#15-A2023791Q -> Get "C01333"
    //            List<string[]> FileNames = CSVUtil.ReadCSV(loaclConditonFullPath);
    //            string testerRecipefileName = String.Empty;
    //            string proberRecipefileName = String.Empty;
    //            string targerStr = DataCenter._uiSetting.Barcode.Substring(0, 6);
    //            string WaferID = DataCenter._uiSetting.Barcode.Remove(0, DataCenter._uiSetting.Barcode.IndexOf("-") + 1);
    //            string LotID = DataCenter._uiSetting.Barcode.Substring(6, DataCenter._uiSetting.Barcode.IndexOf("#") - 6);

    //            foreach (string[] key in FileNames)
    //            {
    //                if (key[0] == targerStr)
    //                {
    //                    if (key.Length >= 3)
    //                    {
    //                        testerRecipefileName = key[1];
    //                        proberRecipefileName = key[2];
    //                    }
    //                }
    //            }

    //            DataCenter._uiSetting.ProberRecipeName = proberRecipefileName;
    //            DataCenter._uiSetting.LotNumber = LotID;
    //            DataCenter._uiSetting.WaferNumber = WaferID;

    //            GlobalData.ProberRecipeName = proberRecipefileName;

    //            if (DataCenter.LoadTaskSheet(testerRecipefileName) == false)
    //                return EErrorCode.MES_LoadTaskError;

    //            return EErrorCode.NONE;
    //        }
    //        catch
    //        {
    //            return EErrorCode.MES_BarcodeError;
    //        }
    //    }
    //}
}

