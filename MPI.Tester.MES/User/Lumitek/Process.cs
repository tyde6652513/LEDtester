using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using MPI.Tester.Data;
using MPI.Tester.MES.Data;
using System.Collections;

namespace MPI.Tester.MES.User.Lumitek
{
	public class MESProcess : ProcessBase
	{
        private UISetting _uiSetting;

        private TaskSheetCtrl _taskSheetCtrl;

        private string _tempRecipeName = string.Empty;

        private Dictionary<string, double> _dicCalibrationTable;

        private string _calibrationFileName = string.Empty;

        protected override Tester.Data.EErrorCode OpenFileAndParse(Tester.Data.UISetting uiSetting, MachineConfig machineConfig)
		{
			try
			{
                this._uiSetting = uiSetting;

                if (uiSetting.IsOpenRecipeOnServer)
                {
                    this.ReadCalibrationTable(uiSetting.TaskSheetFileName);

                    this._tempRecipeName = uiSetting.TaskSheetFileName;

                   // uiSetting.TaskSheetFileName = uiSetting.MachineName + "-" + uiSetting.TaskSheetFileName;

                    this._testerRecipeFileName = uiSetting.MachineName + "-" + uiSetting.TaskSheetFileName;

                    uiSetting.TaskSheetFileName = this._testerRecipeFileName;

                    uiSetting.IsOpenRecipeOnServer = false;

                    Console.WriteLine("[Lumitek Process], OpenRecipeOnServer()");

                    return EErrorCode.NONE;
                }

				Console.WriteLine("[Lumitek Process], OpenFileAndParse()");

				bool isDataOK = false;

				string filePath = Path.Combine(uiSetting.MESPath, "TesterCmpTable.txt");

				if (!File.Exists(filePath))
				{
					Console.WriteLine("[MESProcess], TesterCmpTable.txt File is not Exist:" + filePath);

					return EErrorCode.MES_CondDataNotExist;
				}

				List<string[]> testerCmpTable = CSVUtil.ReadCSV(filePath);

				if (testerCmpTable == null)
				{
					Console.WriteLine("[MESProcess], Read TAS File Fail:" + filePath);

					return EErrorCode.MES_OpenFileError;
				}

				if (testerCmpTable.Count == 0)
				{
					Console.WriteLine("[MESProcess], TAS File Format Error:" + filePath);

					return EErrorCode.MES_ParseFormatError;
				}

				// Barcode:	7522520112G080528D01.1
				// lot:		75225201
				string lot = uiSetting.Barcode.Substring(0, 8);

				string recipe = string.Empty;

                string proberRecipe = string.Empty;

                string alignmentKeyFileName = string.Empty;

                string reworkMode = string.Empty;

                string productionMode = string.Empty;

                //if (lot == "00000000" || lot == "AAAAAAAA" || lot == "AZ09WY1F" || lot == "QGX6Z97U")
                //{
                //    if (lot == "AZ09WY1F")
                //    {
                //        uiSetting.Barcode = uiSetting.Barcode + "#" + DateTime.Now.ToString("yyMMddHHmmss") + "#";
                //    }

                //    return EErrorCode.NONE;
                //}


				foreach (var item in testerCmpTable)
				{
                    //=========================
                    // 2014.02.24 Paul 
                    // 當Lot ID 和 Wafer ID都符合時，不進行破片模式的比對
                    // 輸入的 barcode=Lot ID+wafer ID+"破片碼" 才進行破片模式
                    //=========================

                    string compareStr = item[0] + item[1];

                    if (uiSetting.Barcode == compareStr)
                    {
                        recipe = item[2];

                        proberRecipe = item[3];

                        reworkMode = item[4];

                        productionMode = item[5];

                        alignmentKeyFileName = item[7];
           
                        isDataOK = true;

                        break;
                    }


                    //if (item.Length >= 4 && item[0] == lot)
                    //{
                    //    if (item[3] == "1")
                    //    {
                    //        //item[0]:	75225201
                    //        //item[1]:	12G080528D01
                    //        //item[2]:	281915
                    //        //item[3]:	1
                    //        // Barcode:		7522520112G080528D01-1
                    //        if (uiSetting.Barcode.Contains(compareStr))
                    //        {
                    //            //key:	-1
                    //            string key = uiSetting.Barcode.Replace(compareStr, "");

                    //            // item[3] = 1: '-' 1
                    //            if (key.Length == 2 && key[0] == '-')
                    //            {
                    //                int temp;

                    //                if (int.TryParse(key[1].ToString(), out temp))
                    //                {
                    //                    recipe = item[2];

                    //                    isDataOK = true;

                    //                    break;
                    //                }
                    //            }
                    //        }
                    //    }
                    //    else if (item[3] == "2")
                    //    {
                    //        //item[0]:	75225201
                    //        //item[1]:	12G080528D01
                    //        //item[2]:	281915
                    //        //item[3]:	2

                    //        //compareStr:	7522520112G080528D01
                    //        //string compareStr = item[0] + item[1];

                    //        // Barcode:		7522520112G080528D01.1
                    //        if (uiSetting.Barcode.Contains(compareStr))
                    //        {
                    //            //key:	.1
                    //            string key = uiSetting.Barcode.Replace(compareStr, "");

                    //            // item[3] = 1: '.' 1
                    //            if (key.Length == 2 && key[0] == '.')
                    //            {
                    //                int temp;

                    //                if (int.TryParse(key[1].ToString(), out temp))
                    //                {
                    //                    recipe = item[2];

                    //                    isDataOK = true;

                    //                    break;
                    //                }
                    //            }
                    //        }
                    //    }
                    //    else if (item[3] == "3")
                    //    {
                    //        //item[0]:	75225201
                    //        //item[1]:	12G080528D01
                    //        //item[2]:	281915
                    //        //item[3]:	3

                    //        // compareStr:	7522520112G080528D01
                    //        //string compareStr = item[0] + item[1];

                    //        // compareStr:	7522520112G080528D0
                    //        compareStr = compareStr.Remove(compareStr.Length - 1, 1);

                    //        // ex: Barcode:	7522520112G080528D0A
                    //        if (uiSetting.Barcode.Contains(compareStr))
                    //        {
                    //            //key: A
                    //            string key = uiSetting.Barcode.Replace(compareStr, "");

                    //            int temp;

                    //            if (key.Length == 1 && int.TryParse(key, out temp))
                    //            {
                    //                recipe = item[2];

                    //                isDataOK = true;

                    //                break;
                    //            }
                    //        }
                    //    }
                    //    else if (item[3] == "4")
                    //    {
                    //        //item[0]:	75225201
                    //        //item[1]:	12G080528D01
                    //        //item[2]:	281915
                    //        //item[3]:	4

                    //        //compareStr:	7522520112G080528D01
                    //        //string compareStr = item[0] + item[1];

                    //        // Barcode:		7522520112G080528D01A
                    //        if (uiSetting.Barcode.Contains(compareStr))
                    //        {
                    //            //key: A
                    //            string key = uiSetting.Barcode.Replace(compareStr, "");

                    //            // item[3] = 4: A
                    //            if (key.Length == 1)
                    //            {
                    //                if (key[0] >= 65 && key[0] <= 90)
                    //                {
                    //                    recipe = item[2];

                    //                    isDataOK = true;

                    //                    break;
                    //                }
                    //            }
                    //        }
                    //    }
                    //    else if (item[3] == "X")//模式'X'
                    //    {
                    //        //item[0]:	75225201
                    //        //item[1]:	12G080528D01
                    //        //item[2]:	281915
                    //        //item[3]:	

                    //        //compareStr:	7522520112G080528D01
                    //        //string compareStr = item[0] + item[1];

                    //        // Barcode:		7522520112G080528D01
                    //        if (uiSetting.Barcode == compareStr)
                    //        {
                    //            recipe = item[2];

                    //            isDataOK = true;

                    //            break;
                    //        }
                    //    }
                    //}
				}

                if(lot == "AZ09WY1F")
						{
							{
                        uiSetting.Barcode = uiSetting.Barcode + "#" + DateTime.Now.ToString("yyMMddHHmmss") + "#";
                    }
                }


				if (!isDataOK)
									{
					return EErrorCode.MES_BarcodeError;
				}

                if (reworkMode == "RW")
                {
                    uiSetting.IsReworkMode = true;

                    this.ReadReWorkCalibrationTable(recipe);
									}
                else
                {
                    uiSetting.IsReworkMode = false;
                }

                if (uiSetting.IsLoadProductFactor)
                {
                    this.ReadCalibrationTable(recipe);        
								}

				uiSetting.TaskSheetFileName = uiSetting.MachineName + "-" + recipe;				

                this._tempRecipeName = recipe;

                this._testerRecipeFileName = uiSetting.MachineName + "-" + recipe;

                if (productionMode != string.Empty)
                {
                    int mode = 0;

                    int.TryParse(productionMode, out mode);

                    GlobalData.ProberProductionMode = mode;
							}

                GlobalData.ProberRecipeName = proberRecipe;

                GlobalData.ProberAligenKeyFileName = alignmentKeyFileName;

				return EErrorCode.NONE;
						}
			catch(Exception e)
						{
				Console.WriteLine("[Process], OpenFileAndParse()," + e.ToString());

				return EErrorCode.MES_LoadTaskError;
			}
		}

		protected override Tester.Data.EErrorCode ConverterToMPIFormat()
							{
            EErrorCode rtn = EErrorCode.NONE;

				if (!this._uiSetting.IsDownRecipeFromServer)
								{
					return rtn;
				}
            // DownRecipefromServer and rename recipe

            if (!DownloadRecipeFromServer(this._uiSetting.RecipePathOnServer, this._tempRecipeName))
									{
                return EErrorCode.MES_TargetRecipeNoExist;
            }

            //Craete TaskSheet File  // PR01-ProductName
            rtn = this.CreateTaskSheetFile();

			return EErrorCode.NONE;
							}

		protected override Tester.Data.EErrorCode SaveRecipeToFile()
		{
			return EErrorCode.NONE;
						}

        private EErrorCode CreateTaskSheetFile()
        {
            try
						{
                this._taskSheetCtrl = new TaskSheetCtrl(); 

                this._taskSheetCtrl.FileName = this._testerRecipeFileName;

                this._taskSheetCtrl.SetProduct(this._testerRecipeFileName, Constants.Paths.PRODUCT_FILE);

                this._taskSheetCtrl.SetBinData(this._testerRecipeFileName, Constants.Paths.PRODUCT_FILE);

                this._taskSheetCtrl.SetMapData(this._testerRecipeFileName, Constants.Paths.PRODUCT_FILE);

                this._taskSheetCtrl.SetImportCalibrateData(this._uiSetting.MachineName+"-"+this._calibrationFileName, Constants.Paths.PRODUCT_FILE02);

                this._taskSheetCtrl.CreateTaskSheet(Constants.Paths.PRODUCT_FILE);

                return EErrorCode.NONE;
								}
            catch
            {
                return EErrorCode.MES_LoadTaskError;
							}
						}

        private void ReadReWorkCalibrationTable(string recipe)
        {
            //string outputFileName = Path.Combine(Constants.Paths.PRODUCT_FILE, "Gain2onProudction.csv");

            //if (!isEnable)
            //{
            //    if(File.Exists(outputFileName)==true)
            //    {
            //        File.Delete(outputFileName);
            //    }
            //}

            string calibTableFilePath = Path.Combine(this._uiSetting.MESPath3, "Tester_Recipe_Calib_ReWork_Table.csv");

            if (!File.Exists(calibTableFilePath))
						{
                return;
            }

            _dicCalibrationTable = new Dictionary<string, double>();

            List<string[]> calibTable = CSVUtil.ReadCSV(calibTableFilePath);

            List<string[]> outputCalibTable = new List<string[]>();

            string[] title = calibTable[0];

            for (int i = 0; i < calibTable.Count; i++)
							{
                string[] raw = calibTable[i];

                if (raw[0] == recipe)
								{
                    // get calibration file name
                    //this._calibrationFileName = raw[2];

                    outputCalibTable.Add(new string[3] { "KeyName", "Gain2", "Offset2" });

                    for (int k = 2; k < title.Length; k++)
									{
                        string[] write = new string[3];

                        write[0] = title[k];

                        if (title[k].Contains("LOP") || title[k].Contains("WATT"))
                        {
                            write[1] = raw[k];
                            write[2] = "0.000";
                        }
                        else
                        {
                            write[1] = "1.000";
                            write[2] = raw[k];
									}
                        outputCalibTable.Add(write);
								}
							}
						}

            CSVUtil.WriteCSV(Path.Combine(Constants.Paths.PRODUCT_FILE, "Gain2onProudction.csv"), outputCalibTable);
        }

        private void ReadCalibrationTable(string recipe)
        {
            string calibTableFilePath = Path.Combine(this._uiSetting.MESPath3, "Tester_Recipe_Calib_Table.csv");

            if (!File.Exists(calibTableFilePath))
						{
                return;
            }

            _dicCalibrationTable = new Dictionary<string, double>();

            List<string[]> calibTable=CSVUtil.ReadCSV(calibTableFilePath);

            List<string[]> outputCalibTable = new List<string[]>();

            string[] title = calibTable[0];

            for (int i = 0; i < calibTable.Count; i++)
							{
                string[] raw = calibTable[i];

                if (raw[0] == recipe)
                {
                    // get calibration file name
                    this._calibrationFileName = raw[1];

                    outputCalibTable.Add(new string[3] { "KeyName", "Gain2", "Offset2" });

                    for (int k = 2; k < title.Length; k++)
                    {
                        string[] write = new string[3];

                        write[0] = title[k];

                        if (title[k].Contains("LOP") || title[k].Contains("WATT"))
                        {
                            write[1] = raw[k];
                            write[2] = "0.000";
                        }
                        else
                        {
                            write[1] = "1.000";
                            write[2] = raw[k];
							}
                        outputCalibTable.Add(write);          
						}
					}
				}

            CSVUtil.WriteCSV(Path.Combine(Constants.Paths.PRODUCT_FILE, "Gain3onProudction.csv"), outputCalibTable);
				}

        private bool DownloadRecipeFromServer(string serverPath, string recipe)
        {
          //  string tsFilePath = Path.Combine(serverPath, recipe + Constants.Files.PRODUCT_FILE_EXTENSION);

            string pdFilePath = Path.Combine(serverPath, recipe +"."+ Constants.Files.PRODUCT_FILE_EXTENSION);

            string binFilePath = Path.Combine(serverPath, recipe +"." +Constants.Files.BIN_FILE_EXTENSION);

            string mapFilePath = Path.Combine(serverPath, recipe +"." +Constants.Files.MAPDATA_FILE_EXTENSION);

            string defineRecipeName=this._uiSetting.MachineName+"-"+recipe;

         //   string tsDestinationFilePath=Path.Combine(Constants.Paths.PRODUCT_FILE, defineRecipeName + Constants.Files.PRODUCT_FILE_EXTENSION);

            string pdDestinationFilePath = Path.Combine(Constants.Paths.PRODUCT_FILE, defineRecipeName + "." + Constants.Files.PRODUCT_FILE_EXTENSION);

            string binDestinationFilePath = Path.Combine(Constants.Paths.PRODUCT_FILE, defineRecipeName + "." + Constants.Files.BIN_FILE_EXTENSION);

            string mapDestinationFilePath = Path.Combine(Constants.Paths.PRODUCT_FILE, defineRecipeName + "." + Constants.Files.MAPDATA_FILE_EXTENSION);

            if (File.Exists(pdFilePath))
            {
                MPIFile.CopyFile(pdFilePath, pdDestinationFilePath);
			}
            else
			{
                return false;
            }

            if(File.Exists(binFilePath))
            {
                MPIFile.CopyFile(binFilePath, binDestinationFilePath);
			}
            else
            {
                return false;
		}

            if(File.Exists(mapFilePath))
		{
                MPIFile.CopyFile(mapFilePath, mapDestinationFilePath);
		}
            else
		{
                return false;
            }

            return true;
		}
	}
}
