using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPI.Tester.Data;
using System.IO;

namespace MPI.Tester.MES.User.EPITop
{
    class MESProcess : ProcessBase
    {
        public const string COND_FILE_NANE = "EPItopTestCondition.CSV";
        private string _sortPageProberRecipeName;
        private bool _isSortPageTestMode;
        private bool _isRunDailyCheckMode;
        private string _proberRecipefileName;

        public MESProcess() :base()
        {
            this._sortPageProberRecipeName = String.Empty;
            this._isSortPageTestMode = false;
            this._isRunDailyCheckMode = false;
            this._proberRecipefileName = String.Empty;
        }


        protected override EErrorCode OpenFileAndParse(UISetting uiSetting, MachineConfig machineConfig)
        {
            EErrorCode rtn = EErrorCode.NONE;

            if (!uiSetting.IsDeliverProberRecipe)
            {
                return rtn;
            }
            
            string serverConditonFullPath = Path.Combine(uiSetting.MESPath, COND_FILE_NANE);
            string loaclConditonFullPath = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, COND_FILE_NANE);
            GlobalData.ProberRecipeName = "";
           
             DriveInfo driveInfo = new DriveInfo(serverConditonFullPath);

             if (!driveInfo.IsReady)
             {
                 rtn= EErrorCode.MES_ServerConnectFail;
                 return rtn;
             }

             if (File.Exists(serverConditonFullPath))
             {
				 MPIFile.DeleteFile(loaclConditonFullPath);
                 MPIFile.CopyFile(serverConditonFullPath, loaclConditonFullPath);
             }
             else
             {
                 Console.WriteLine("[MESProcess], OpenFileAndParse, " + EErrorCode.MES_CondDataNotExist.ToString());
                 rtn = EErrorCode.MES_CondDataNotExist;
                 return rtn;
             }

             try
             {
                 List<string[]> FileNames = Tool.ToolBox.ReadCSV(loaclConditonFullPath);

                 if (FileNames == null)
                 {
                     Console.WriteLine("[MESProcess], OpenFileAndParse, " + EErrorCode.MES_OpenFileError.ToString());
                     return EErrorCode.MES_OpenFileError;
                 }

                 string referenceStr = uiSetting.Barcode;

				 if (referenceStr.Length < 6)
				 {
					 rtn = EErrorCode.MES_BarcodeError;

					 return rtn;
				 }

				 string targetKey = string.Empty;

				 //================================================
				 // Check Is Square Sort Page Mode
				 //================================================
				 // Normal Barcode:		PW09AAS01P3229A01
				 // Sort Page Barcode:	W09AA-DJK026009004
				 if (referenceStr[5] == '-')
				 {
					 this._isSortPageTestMode = true;

					 targetKey = referenceStr.Substring(0, 5);
				 }
				 else
				 {
					 this._isSortPageTestMode = false;

					 targetKey = referenceStr.Substring(1, 5);
				 }

				 // 6080 Auto Load Recipe Rule 
				 // TestCondID,TestSpecName,ProberSpecName,REProberSpecName
				 // PAA,09A-B-MCD,09A-B-MCD,09A-B-MCD-R
				 // PAB,09A-B-MCD,09B-B-MCD,09B-B-MCD-R
				 // PAD,W-15A-MW,W-15A-MW,W-15A-MW-R
                 foreach (string[] key in FileNames)
                 {
                     if (key[0] == targetKey)
                     {
                         if (key.Length >= 3)
                         {
                             this._testerRecipeFileName = key[1];
                             this._proberRecipefileName = key[2];

                             if (key.Length > 3)
                             {
                                 this._sortPageProberRecipeName = key[3];
                             }                          
                             break;
                         }
                     }
                 }

				 //================================================
                 // Check is Daily Check Mode
				 //================================================
                 if (referenceStr.ToLower().Contains("gld"))
                 {
                     this._isRunDailyCheckMode = true;
                 }
                 else
                 {
                     this._isRunDailyCheckMode = false;
                 }

                 if (!this._isRunDailyCheckMode && this._testerRecipeFileName == String.Empty)
                 {
                     Console.WriteLine("[MESProcess], OpenFileAndParse, " + EErrorCode.MES_NotMatchRecipe.ToString());
                     Console.WriteLine("[MESProcess], Target Tester Recipe File Name is Empty");
                     rtn = EErrorCode.MES_NotMatchRecipe;
                     return rtn;
                 }
             }
             catch
             {
                 rtn = EErrorCode.MES_BarcodeError;
                 return rtn;
             }

             if (!this._isRunDailyCheckMode)
             {
                 if (!File.Exists(Path.Combine(uiSetting.ProductPath, this._testerRecipeFileName + "." + Constants.Files.TASK_SHEET_EXTENSION)))
                 {
                     Console.WriteLine("[MESProcess], OpenFileAndParse, " + EErrorCode.MES_TargetRecipeNoExist.ToString());
                     Console.WriteLine("[MESProcess], Target Tester Recipe : " + this._testerRecipeFileName + " Not Exist");
                     return EErrorCode.MES_TargetRecipeNoExist;
                 }
             }
             else
             {
                 this._testerRecipeFileName = uiSetting.TaskSheetFileName;
             }

            // Update Data To UI Setting

             switch (rtn)
             {
                 case EErrorCode.NONE :
                   Console.WriteLine("[MESProcess], OpenFileAndParse, SUCCESS ");
                    if (this._isSortPageTestMode)
                    {                   
                        uiSetting.ProberRecipeName = this._sortPageProberRecipeName;
                    }
                    else
                    {
                        uiSetting.ProberRecipeName = this._proberRecipefileName;
                    }
                    //
                    uiSetting.IsRunReTestSortPageMode = this._isSortPageTestMode;
                    uiSetting.TaskSheetFileName = this._testerRecipeFileName;
                    uiSetting.IsRunDailyCheckMode = this._isRunDailyCheckMode;
                    GlobalData.ProberRecipeName = uiSetting.ProberRecipeName;
                    Console.WriteLine("[MESProcess], TesterRecipeFileName, " + this._testerRecipeFileName);
                    Console.WriteLine("[MESProcess], ProberRecipeName, " + this._proberRecipeFileName);
                    Console.WriteLine("[MESProcess], RunReTestSortPageMode, " + this._isSortPageTestMode.ToString());
                    Console.WriteLine("[MESProcess], RunDailyCheckMode, " + this._isRunDailyCheckMode.ToString());
                     break;
                 default:
                     break;
             }

             return rtn;

        }

        protected override EErrorCode ConverterToMPIFormat()
        {
            return EErrorCode.NONE;
        }

        protected override EErrorCode SaveRecipeToFile()
        {
            return EErrorCode.NONE;
        }
    }
}
