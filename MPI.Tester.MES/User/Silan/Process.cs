using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using MPI.Tester.Data;
using MPI.Tester.MES.Data;
using System.Xml;

namespace MPI.Tester.MES.User.Silan
{
    public class MESProcess : ProcessBase
    {
		private bool ModifyCalFileName(string tsFileFullName, string calFileName, out string msg)
		{
			msg = string.Empty;

			XmlDocument xmlDoc = new XmlDocument();

			XmlElement root = null;

			if (File.Exists(tsFileFullName) == false)
			{
				msg = "[MESProcess], ModifyCalFileName(), File not Exist : " + tsFileFullName;

				Console.WriteLine("[MESProcess], ModifyCalFileName(), File not Exist : " + tsFileFullName);

				return false;
			}

			try
			{
				xmlDoc.Load(tsFileFullName);

				root = xmlDoc.DocumentElement;
			}
			catch
			{
				msg = "[MESProcess], ModifyCalFileName(), Load TS XLM Fail";

				Console.WriteLine("[MESProcess], ModifyCalFileName(), Load TS XLM Fail");

				return false;
			}

			if (root == null)
			{
				msg = "[MESProcess], ModifyCalFileName(), TS XLM is Empty";

				Console.WriteLine("[MESProcess], ModifyCalFileName(), TS XLM is Empty");

				return false;
			}

			XmlNode node = root.SelectSingleNode("FileInfo[Type='ImportCalibrateData']");

			if (node == null || node.SelectSingleNode("Name") == null)
			{
				msg = "[MESProcess], ModifyCalFileName(), ImportCalibrateData node is null";

				Console.WriteLine("[MESProcess], ModifyCalFileName(), ImportCalibrateData node is null");

				return false;
			}

			node.SelectSingleNode("Name").InnerText = calFileName;

			xmlDoc.Save(tsFileFullName);

			return true;

		}

		private bool ModifyProductFileLOPGain(string productFilFulleName, string calFilFulleName, double lopGain, out string msg)
		{
			msg = string.Empty;

			if (!System.IO.File.Exists(productFilFulleName))
			{
				msg = "[MESProcess], ModifyProductFileLOPGain(), File not Exist : " + productFilFulleName;

				return false;
			}

			if (!System.IO.File.Exists(calFilFulleName))
			{
				msg = "[MESProcess], ModifyProductFileLOPGain(), File not Exist : " + calFilFulleName;

				return false;
			}

			try
			{
				//------------------------------------------------------------------
				// Product File
				//------------------------------------------------------------------
				ProductData product = MPI.Xml.XmlFileSerializer.Deserialize(typeof(ProductData), productFilFulleName) as ProductData;

				if (product == null)
				{
					msg = "[MESProcess], ModifyProductFileLOPGain(), product == null";

					return false;
				}
				else
				{
					if (product.TestCondition != null &&
						product.TestCondition.TestItemArray != null &&
						product.TestCondition.TestItemArray.Length > 0)
					{
						foreach (var testItem in product.TestCondition.TestItemArray)
						{
							if (testItem.MsrtResult == null || testItem.MsrtResult.Length == 0 || !testItem.IsEnable || testItem.ElecSetting == null)
							{
								continue;
							}

							if (testItem.KeyName == "LOPWL_1")
							{
								testItem.GainOffsetSetting[(int)EOptiMsrtType.LOP].Gain2 = lopGain;
							}
						}
					}
						
					MPI.Xml.XmlFileSerializer.Serialize(product, productFilFulleName);

					
				}

				//------------------------------------------------------------------
				// Cal File
				//------------------------------------------------------------------
				List<string[]> calFile = CSVUtil.ReadCSV(calFilFulleName);

				if (calFile == null)
				{
					msg = "[MESProcess], ModifyProductFileLOPGain(), calFile == null";

					return false;
				}

				bool isGainOffsetSection = false;

				for (int i = 0; i < calFile.Count;i++ )
				{
					if (isGainOffsetSection && calFile[i][0] == "LOP_1")
					{
						calFile[i][5] = lopGain.ToString();
					}
					else if (calFile[i][0] == "GainOffset")
					{
						isGainOffsetSection = true;
					}
					else if (isGainOffsetSection && calFile[i][0] == "SectionEnd")
					{
						break;
					}
				}

				if(!CSVUtil.WriteCSV(calFilFulleName, calFile))
				{
					msg = "[MESProcess], ModifyProductFileLOPGain(), Write cal CSV file fail";

					return false;
				}

				return true;
			}
			catch(Exception e)
			{
				msg = "[MESProcess], ModifyProductFileLOPGain(), catch:" + e.ToString();

				return false;
			}
		}

        protected override Tester.Data.EErrorCode OpenFileAndParse(Tester.Data.UISetting uiSetting, MachineConfig machineConfig)
        {
            try
            {
                Console.WriteLine("[Silan Process], OpenFileAndParse()");

				if (uiSetting.UserID == EUserID.Silan)
				{
					bool isRepeat = false;

					isRepeat |= uiSetting.MESPath3 == uiSetting.TestResultPath01;

					isRepeat |= uiSetting.MESPath3 == uiSetting.TestResultPath02;

					isRepeat |= uiSetting.MESPath3 == uiSetting.TestResultPath03;

					if (isRepeat)
					{
						return EErrorCode.OutputFilePathRepeat;
					}
				}

                if (uiSetting.Barcode.Length < 3)
                {
					this._describe.AppendLine("Barcode len < 3");

                    Console.WriteLine("[MESProcess], Barcode len < 3:");

                    return EErrorCode.MES_BarcodeError;
                }

				//------------------------------------------------------------------
				// Description File
				//------------------------------------------------------------------
                string descFileName = uiSetting.Barcode.Substring(0, 3);

				string descFileFullName = Path.Combine(uiSetting.MESPath, descFileName + ".txt");

				string descLocalFileFullName = Path.Combine(Constants.Paths.LEDTESTER_TEMP_DIR, descFileName + ".txt");

                //DriveInfo driveInfo = new DriveInfo(descFileFullName);

                //if (!driveInfo.IsReady)
                //{
                //    this._describe.AppendLine("DriveInfo Is not Ready:" + descFileFullName);

                //    return EErrorCode.MES_ServerConnectFail;
                //}

				if (!File.Exists(descFileFullName))
				{
					this._describe.AppendLine("Description.txt File is not Exist:" + descFileFullName);

					Console.WriteLine("[MESProcess], Description.txt File is not Exist:" + descFileFullName);

					return EErrorCode.MES_CondDataNotExist;
				}

				if (!MPIFile.CopyFile(descFileFullName, descLocalFileFullName))
				{
					this._describe.AppendLine(descFileFullName + " Copy File Fail:" + descFileFullName + " to " + descLocalFileFullName);

					Console.WriteLine("[MESProcess],Copy File Fail:" + descFileFullName + " to " + descLocalFileFullName);

					return EErrorCode.MES_CondDataNotExist;
				}

				List<string[]> descFile = CSVUtil.ReadCSV(descLocalFileFullName);

                if (descFile == null)
                {
					this._describe.AppendLine("Read Description File Fail:" + descLocalFileFullName);

					Console.WriteLine("[MESProcess], Read Description File Fail:" + descLocalFileFullName);

                    return EErrorCode.MES_OpenFileError;
                }

                if (descFile.Count == 0)
                {
					this._describe.AppendLine("Description File Format Error:" + descLocalFileFullName);

					Console.WriteLine("[MESProcess], Description File Format Error:" + descLocalFileFullName);

                    return EErrorCode.MES_ParseFormatError;
                }

                if (descFile[0].Length < 2)
                {
					this._describe.AppendLine("Description File Format Error:" + descLocalFileFullName);

					Console.WriteLine("[MESProcess], Description File Format Error:" + descLocalFileFullName);

                    return EErrorCode.MES_ParseFormatError;
                }

				double productGain = 1.0d;

                if (descFile[0].Length > 2 && !double.TryParse(descFile[0][2], out productGain))
                {
					productGain = 1.0d;
                }

				//------------------------------------------------------------------
				// Recipe File
				//------------------------------------------------------------------
				string calFileName = Path.GetFileNameWithoutExtension(descFile[0][0]);

				string reicpeFileName = Path.GetFileNameWithoutExtension(descFile[0][1]);

				string recipeFileFullName_ts = Path.Combine(uiSetting.MESPath2, reicpeFileName + ".ts");

				string recipeFileFullName_pd = Path.Combine(uiSetting.MESPath2, reicpeFileName + ".pd");

				string recipeFileFullName_bin = Path.Combine(uiSetting.MESPath2, reicpeFileName + ".bin");

				string recipeFileFullName_map = Path.Combine(uiSetting.MESPath2, reicpeFileName + ".map");

				string loaclFileFullName_cal = Path.Combine(Constants.Paths.PRODUCT_FILE02, calFileName + ".cal");

				string loaclFileFullName_ts = Path.Combine(Constants.Paths.PRODUCT_FILE, reicpeFileName + ".ts");

				string loaclFileFullName_pd = Path.Combine(Constants.Paths.PRODUCT_FILE, reicpeFileName + ".pd");

				string loaclFileFullName_bin = Path.Combine(Constants.Paths.PRODUCT_FILE, reicpeFileName + ".bin");

				string loaclFileFullName_map = Path.Combine(Constants.Paths.PRODUCT_FILE, reicpeFileName + ".map");


				//------------------------------------------------------------------
				// Check Recipe File Exist
				//------------------------------------------------------------------
                //driveInfo = new DriveInfo(recipeFileFullName_ts);

                //if (!driveInfo.IsReady)
                //{
                //    this._describe.AppendLine("DriveInfo is not Ready:" + recipeFileFullName_ts);

                //    return EErrorCode.MES_ServerConnectFail;
                //}

				if (!File.Exists(loaclFileFullName_cal))
				{
					this._describe.AppendLine("Cal File is not Exist:" + loaclFileFullName_cal);

					Console.WriteLine("[MESProcess], Cal File is not Exist:" + loaclFileFullName_cal);

					return EErrorCode.MES_SaveRecipeToFileError;
				}

				if (!File.Exists(recipeFileFullName_ts))
				{
					this._describe.AppendLine("Recipe File is not Exist:" + recipeFileFullName_ts);

					Console.WriteLine("[MESProcess], Recipe File is not Exist:" + recipeFileFullName_ts);

					return EErrorCode.MES_SaveRecipeToFileError;
				}

				if (!File.Exists(recipeFileFullName_pd))
				{
					this._describe.AppendLine("Recipe File is not Exist:" + recipeFileFullName_pd);

					Console.WriteLine("[MESProcess], Recipe File is not Exist:" + recipeFileFullName_pd);

					return EErrorCode.MES_SaveRecipeToFileError;
				}

				if (!File.Exists(recipeFileFullName_bin))
				{
					this._describe.AppendLine("Recipe File is not Exist:" + recipeFileFullName_bin);

					Console.WriteLine("[MESProcess], Recipe File is not Exist:" + recipeFileFullName_bin);

					return EErrorCode.MES_SaveRecipeToFileError;
				}

				if (!File.Exists(recipeFileFullName_map))
				{
					this._describe.AppendLine("Recipe File is not Exist:" + recipeFileFullName_map);

					Console.WriteLine("[MESProcess], Recipe File is not Exist:" + recipeFileFullName_map);

					return EErrorCode.MES_SaveRecipeToFileError;
				}

				//------------------------------------------------------------------
				// Copy Recipe File to Local
				//------------------------------------------------------------------
				if (!MPIFile.CopyFile(recipeFileFullName_ts, loaclFileFullName_ts))
				{
					this._describe.AppendLine("Copy File Fail:" + recipeFileFullName_ts + " to " + loaclFileFullName_ts);

					Console.WriteLine("[MESProcess], Copy File Fail:" + recipeFileFullName_ts + " to " + loaclFileFullName_ts);

					return EErrorCode.MES_CondDataNotExist;
				}

				if (!MPIFile.CopyFile(recipeFileFullName_pd, loaclFileFullName_pd))
				{
					this._describe.AppendLine("Copy File Fail:" + recipeFileFullName_pd + " to " + loaclFileFullName_pd);

					Console.WriteLine("[MESProcess], Copy File Fail:" + recipeFileFullName_pd + " to " + loaclFileFullName_pd);

					return EErrorCode.MES_CondDataNotExist;
				}

				if (!MPIFile.CopyFile(recipeFileFullName_bin, loaclFileFullName_bin))
				{
					this._describe.AppendLine("Copy File Fail:" + recipeFileFullName_bin + " to " + loaclFileFullName_bin);

					Console.WriteLine("[MESProcess], Copy File Fail:" + recipeFileFullName_bin + " to " + loaclFileFullName_bin);

					return EErrorCode.MES_CondDataNotExist;
				}

				if (!MPIFile.CopyFile(recipeFileFullName_map, loaclFileFullName_map))
				{
					this._describe.AppendLine("Copy File Fail:" + recipeFileFullName_map + " to " + loaclFileFullName_map);

					Console.WriteLine("[MESProcess],Copy File Fail:" + recipeFileFullName_map + " to " + loaclFileFullName_map);

					return EErrorCode.MES_CondDataNotExist;
				}

				//------------------------------------------------------------------
				// Modify Cal File Name Write to TS File
				//------------------------------------------------------------------
				string err = string.Empty;

				if(!this.ModifyCalFileName(loaclFileFullName_ts, calFileName, out err))
				{
					Console.WriteLine(err);

					return EErrorCode.MES_LoadTaskError;
				}

				//------------------------------------------------------------------
				// Read Product Gain and Write to Product File LOP Gain2
				//------------------------------------------------------------------
				if (!this.ModifyProductFileLOPGain(loaclFileFullName_pd, loaclFileFullName_cal, productGain, out err))
				{
					Console.WriteLine(err);

					return EErrorCode.MES_LoadTaskError;
				}


				uiSetting.TaskSheetFileName = reicpeFileName;

				uiSetting.ImportCalibrateFileName = calFileName;

                return EErrorCode.NONE;
            }
            catch (Exception e)
            {
                Console.WriteLine("[Process], OpenFileAndParse()," + e.ToString());

                return EErrorCode.MES_LoadTaskError;
            }
        }

        protected override Tester.Data.EErrorCode ConverterToMPIFormat()
        {
            return EErrorCode.NONE;
        }

        protected override Tester.Data.EErrorCode SaveRecipeToFile()
        {
            return EErrorCode.NONE;
        }
    }
}
