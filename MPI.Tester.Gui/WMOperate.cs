using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using MPI.Tester.Data;

namespace MPI.Tester.Gui
{
    public class WMOperate
    {
        private static string[][] _mkFileData = null;
		private static Dictionary<string,int> colDataIndex = new Dictionary<string,int>()
			{  {"SpecName" , -1 } , 
				{"TestSpec" , -1 },
				{"Calibration" , -1 },
				{"ResortingSpec" , -1 },
				{"ResortingModel" , -1 },
			};

		private static   int _specNameIndex = -1;
        private static   int _testSpecIndex = -1;
        private static   int _calibrationIndex = -1;
        private static   int _resortingSpecIndex = -1;
        private static   int _resortingModelIndex = -1;

        #region >>> Private Static Method <<<

        private static bool LoadWM_MKFileData(string pathAndFileName, ref string[][] dataArray)
        {
			//CSVUtil.ReadFromCSV(pathAndFileName, out dataArray);

			List<string[]> importData = new List<string[]>(100);
			string strLine = string.Empty;
			try
			{
				importData.Clear();
				using (StreamReader sr = new StreamReader(pathAndFileName))
				{
					while (!sr.EndOfStream)
					{
						strLine = sr.ReadLine();
						if (strLine == null || strLine.Length < 0)
							continue;

						importData.Add(strLine.Split(','));
					}
					sr.Close();
				}
			}
			catch ( IOException )
			{
				// Gilbert Error
				Console.WriteLine( "The file could not be opened because it was locked by another process." );
				return false;
			}
			catch ( Exception ex )
			{
				// Gilbert Error
				Console.WriteLine( ex.ToString() );
				return false;
			}

			dataArray = importData.ToArray();
			return true;
		}

		private static void  ParseWM_MKFileData(string searchLotNumber)
		{
			string pathAndFileNameWithExt = DataCenter._uiSetting.WeiminUIData.WM_MES_Path01 +
																DataCenter._uiSetting.MachineName + "." +
																DataCenter._uiSetting.WeiminUIData.WM_MES_FileExt01;

			if (LoadWM_MKFileData(pathAndFileNameWithExt, ref _mkFileData) == false)
			{
				Host.SetErrorCode(EErrorCode.WM_MES_FullyModeLoadMKFileFail);
				return;
			}

			if ( _mkFileData.Length <= 1 )
			{
				Host.SetErrorCode(EErrorCode.WM_MES_MKFileContentErr);
				return;
			}

			for (int i = 0; i < _mkFileData[0].Length; i++)
			{
				if (_mkFileData[0][i] == "SpecName")
				{
					_specNameIndex = i;
				}
				else if (_mkFileData[0][i] == "TestSpec")
				{
					_testSpecIndex = i;
				}
				else if (_mkFileData[0][i] == "Calibration")
				{
					_calibrationIndex = i;
				}
				else if (_mkFileData[0][i] == "ResortingSpec")
				{
					_resortingSpecIndex = i;
				}
				else if (_mkFileData[0][i] == "ResortingModel")
				{
					_resortingModelIndex = i;
				}
			}


			if ( _specNameIndex == -1 || _testSpecIndex == -1 || _calibrationIndex == -1 ||
				 _resortingSpecIndex == -1 || _resortingModelIndex == -1 )
			{
				Host.SetErrorCode(EErrorCode.WM_MES_MKFileContentErr);
				return;
			}

			//for (int i = 0; i < _mkFileData[0].Length; i++)
			//{
			//    foreach (string str in colDataIndex.Keys)
			//    {
			//        if (_mkFileData[0][i] == str)
			//        {
			//            colDataIndex[str] = i;
			//            break;
			//        }
			//    }
			//}

			//if (colDataIndex.Values.Contains(-1))
			//{
			//    Host.SetErrorCode(EErrorCode.WM_MES_MKFileContentErr;
			//    return;
			//}

			int row = 0;
			for (row = 0; row < _mkFileData.Length; row++)
			{
				if ( _mkFileData[row][ _specNameIndex ] == searchLotNumber)
					break;
			}

			if (row == 0 || row == _mkFileData.Length)
			{
				Host.SetErrorCode(EErrorCode.WM_MES_NotMatchLotNumber);
				return;
			}

			DataCenter._uiSetting.WeiminUIData.DeviceNumber = _mkFileData[row][_resortingSpecIndex];
			DataCenter._uiSetting.WeiminUIData.SerialNumber = _mkFileData[row][_resortingModelIndex];

			string[] caliStr01 = _mkFileData[row][ _calibrationIndex ].Split('_');

			if (DataCenter.LoadTaskSheet(_mkFileData[row][ _testSpecIndex ]) == true)
			{
				DataCenter._uiSetting.TaskSheetFileName = _mkFileData[row][ _testSpecIndex ];
				Host.UpdateDataToAllUIForm();
				string[] caliStr02 = DataCenter._uiSetting.WeiminUIData.CustomerRemark01.Split(',');

				if (caliStr01.Length != caliStr02.Length)
				{
					Host.SetErrorCode(EErrorCode.WM_MES_CaliValueNotMatch);
					return;
				}
				else
				{
					double value01 = 0.0d;
					double value02 = 0.0d;
					for (int k = 0; k < caliStr01.Length; k++)
					{
						if (Double.TryParse(caliStr01[k], out value01) == false || Double.TryParse(caliStr02[k], out value02) == false)
						{
							Host.SetErrorCode(EErrorCode.WM_MES_CaliValueNotMatch);
							return;
						}

						if (value01 != value02)
						{
							Host.SetErrorCode(EErrorCode.WM_MES_CaliValueNotMatch);
							return;
						}
					}
				}
			}
			else
			{
				Host.SetErrorCode(EErrorCode.LoadTaskSheetFail);
				Host.UpdateDataToAllUIForm();
				return;
			}
		}

        private static void WM_FullyTest()
        {
			switch (DataCenter._uiSetting.UserID)
			{
				case EUserID.Sanan:
					ParseWM_MKFileData(DataCenter._uiSetting.WeiminUIData.LotNumber);
					break;
				//-------------------------------------------------------------------------------------------
				default:
					break;
			}
        }

        private static void WM_SampleTest()
        {
            string[][] sampleData = null;
			int chIndex = DataCenter._uiSetting.WeiminUIData.KeyInFileName.IndexOf(DataCenter._uiSetting.WeiminUIData.WM_MES_Char02);
            string fileName = "";

            if (chIndex >= 0)
            {
                fileName = DataCenter._uiSetting.WeiminUIData.KeyInFileName.Remove(chIndex);
            }
            else
            {
				fileName = DataCenter._uiSetting.WeiminUIData.KeyInFileName;
            }

            string pathAndFileNameWithExt = DataCenter._uiSetting.WeiminUIData.WM_MES_Path02 +
                                                                fileName + "." +
                                                                DataCenter._uiSetting.WeiminUIData.WM_MES_FileExt02;

            if (LoadWM_MKFileData(pathAndFileNameWithExt, ref sampleData) == false)
            {
                Host.SetErrorCode(EErrorCode.WM_MES_SamplingModeLoadMKFileFail);
                return;
            }

            int row = 0;
            for (row = 0; row < sampleData.Length; row++)
            {
                if (sampleData[row][0] == DataCenter._uiSetting.WeiminUIData.KeyInFileName	)
                    break;
            }

            if (row == 0 || row == sampleData.Length)
            {
                Host.SetErrorCode(EErrorCode.WM_MES_NotMacthWaferNumber);
                return;
            }

            ParseWM_MKFileData( sampleData[row][1] );

            DataCenter._uiSetting.LotNumber = sampleData[row][1];
            DataCenter._uiSetting.WeiminUIData.SpecificationRemark = sampleData[row][2];
        }

		private static void SetCtrlDataByUser()
		{
			switch (DataCenter._uiSetting.UserID)
			{
				case EUserID.Sanan:
					if (DataCenter._uiSetting.WeiminUIData.WMTestMode == (int)EWMTestMode.FullyTest)
					{
						DataCenter._uiSetting.FileNameFormatPresent = (int)EOutputFileNamePresent.BarCode;
						DataCenter._uiSetting.IsEnablePath01 = true;
						DataCenter._uiSetting.IsEnablePath02 = true;
					}
					else if (DataCenter._uiSetting.WeiminUIData.WMTestMode == (int)EWMTestMode.SampleTest)
					{
						DataCenter._uiSetting.FileNameFormatPresent = (int)EOutputFileNamePresent.Customer01;
						DataCenter._uiSetting.IsEnablePath01 = true;
						DataCenter._uiSetting.IsEnablePath02 = true;
					}
					else if (DataCenter._uiSetting.WeiminUIData.WMTestMode == (int)EWMTestMode.ESDTest)
					{
						DataCenter._uiSetting.FileNameFormatPresent = (int)EOutputFileNamePresent.Customer02;
						DataCenter._uiSetting.IsEnablePath01 = true;
						DataCenter._uiSetting.IsEnablePath02 = true;					
					}
					else if (DataCenter._uiSetting.WeiminUIData.WMTestMode == (int)EWMTestMode.EngineerTest)
					{
						DataCenter._uiSetting.FileNameFormatPresent = (int)EOutputFileNamePresent.BarCode;
						DataCenter._uiSetting.IsEnablePath01 = true;
						DataCenter._uiSetting.IsEnablePath02 = false;
					}
					else
					{
						DataCenter._uiSetting.FileNameFormatPresent = (int)EOutputFileNamePresent.BarCode;
						DataCenter._uiSetting.IsEnablePath01 = true;
						DataCenter._uiSetting.IsEnablePath02 = false;
					}
					break;
				//-----------------------------------------------------------
				default :
					break;
			}

			DataCenter.SaveUISettingToFile();
		}

        #endregion

        #region >>> Public Static Method <<<

        public static bool WM_StartTest()
        {
            //bool rtn = true;
								
            //WM_ReadCalibrateParamFromSetting();

            //SetCtrlDataByUser();

            //if (DataCenter._uiSetting.WeiminUIData.WMTestMode == (int)EWMTestMode.FullyTest)
            //{
            //    WM_FullyTest();
            //}
            //else if (DataCenter._uiSetting.WeiminUIData.WMTestMode == (int)EWMTestMode.SampleTest)
            //{
            //    WM_SampleTest();
            //}

            //if (Host._UIErrorCode != EErrorCode.NONE)
            //{
            //    DataCenter._uiSetting.WeiminUIData.OutputFileName = DataCenter._uiSetting.WeiminUIData.KeyInFileName;

            //    WM_EndTest();
            //    rtn = false;
            //}
            //else
            //{
            //    Host._MPIStorage.GenerateOutputFileName();
            //    DataCenter._uiSetting.WeiminUIData.OutputFileName = DataCenter._uiSetting.TestResultFileName + "." + DataCenter._uiSetting.TestResultFileExt;
            //    Host.UpdateDataToAllUIForm();
            //    rtn = true;
            //}
            return true;
        }

        public static void WM_EndTest()
        {
            //string temp = DataCenter._uiSetting.WeiminUIData.CustomerNote01;
              DataCenter._uiSetting.WeiminUIData.ResetAllData();

            //DataCenter._uiSetting.WeiminUIData.CustomerNote01 = temp;
            //Host.UpdateDataToAllUIForm();
        }

		public static void WM_ReadCalibrateParamFromSetting()
		{
            //if (DataCenter._product != null)
            //{
            //    DataCenter._uiSetting.WeiminUIData.CustomerRemark01 = DataCenter._conditionCtrl.ParseWM617GainOffsetList(DataCenter._product.LOPSaveItem);
            //    DataCenter._uiSetting.WeiminUIData.CustomerNote01 = "SpecName:" + DataCenter._uiSetting.TaskSheetFileName +
            //                                                                                          "/FixLopByWLSpec:1,2,63," +
            //                                                                                          DataCenter._uiSetting.TaskSheetFileName;
            //    DataCenter.SaveUISettingToFile();
            //}
		}
		
        #endregion
    }
}
