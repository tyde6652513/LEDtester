using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.Tester.Data
{

	/// <summary>
	/// Constant Data Class.
	/// </summary>
    public class Constants
    {
        
        public const string UI_TITLE = "LEDTester";

		public const string UI_AUTHOR = "Test.Wan";
		public const string I18N_FIND_KEY = "_I18N";
		public const string I18N_STR_HEADER = "MPI::";
		// message Bin
		public const string ERROR_MSG_HEADER = "ERROR_MSG_ID=";
		public const string INFO_MSG_HEADER = "INFO_MSG_ID=";
		public const string UI_MSG_HEADER = "UI_MSG_ID=";
		public const string LOG_MSG_HEADER = "LOG_MSG_ID=";

		/// <summary>
		/// System Paths Information Class
		/// </summary>
        public class Paths
        {
			//[System.Obsolete]
            public const string ROOT = @"C:\MPI\LEDTester";
            public const string DATA_FILE = @"C:\MPI\LEDTester\Data";
			public const string PRODUCT_FILE = @"C:\MPI\LEDTester\Product";
			public const string PRODUCT_FILE02 = @"C:\MPI\LEDTester\Product02";			
			public const string LEDTESTER_TEMP_DIR = @"C:\MPI\LEDTester\Temp";
            public const string LEDTESTER_TEMP_DIR2 = @"C:\MPI\LEDTester\Temp2";
			public const string MPI_TEMP_DIR = @"C:\MPI\Temp";
            public const string MPI_TEMP_DIR2 = @"C:\MPI\Temp2";


            public const string DiskD_TEMP_DIR = @"D:\MPI\Temp";

            public const string LOG_FILE = @"C:\MPI\LEDTester\Log";
            public const string USER_DIR = @"C:\MPI\LEDTester\User";

			public const string RESOURCE_FILE_DIR_NAME = "Resource";						
			public const string RESOURCE_FILE_DIR = @"C:\MPI\LEDTester\Resource";
			public const string DEBUG_FILE = @"C:\MPI\LEDTester\Debug";

			public const string ADVANCED_LOG_FILE = @"C:\MPI\LEDTester\Log\Adv";

            public const string TOOLS_DIR = @"C:\MPI\LEDTester\Tools";
            public const string TOOLS_USER_DIR = @"C:\MPI\LEDTester\Tools\User";

			public const string MPI_SHARE_DIR = @"C:\MPI\Share";
			public const string BARCODE_PRINT_DIR = @"C:\MPI\LEDTester\Print";
			public const string BARCODE_PRINT_BACKUP_DIR = @"C:\MPI\Temp\PrintBackup";
            public const string COEFFICIENT_DIR = @"C:\MPI\LEDTester\Coefficent";
            public const string MES_FILE_PATH = @"C:\MPI\LEDTester\MES";

			public const string MPI_BACKUP_DIR = @"C:\MPI\Backup";
            public const string TOOLS_DC_DIR = @"C:\MPI\LEDTester\Tools\Data";
            public const string TOOLS_DC_FILE_SPEC_DIR = @"C:\MPI\LEDTester\Tools\Spec";

            public const string SPECTROMETER_CALIBRATION_DATA = @"C:\MPI\LEDTester\Spectrometer";

            public const string PROBER_PRODUCT_DIR = @"C:\FAE\Product";

			public const string CAMERA_CALIBRATION_DATA = @"C:\MPI\LEDTester\Camera";
            public const string LASER_LOG  =@"C:\MPI\LEDTester\Laser";

        }


		/// <summary>
		/// Files Information class. Default file name, file extension, 
		/// </summary>

        public class Files
        {
            public const string DEVICE_CONFIG = "DeviceConfig.xml";
			public const string DEVICE_SPEC = "DeviceSpec.xml"; 
            public const string MACHINE_DATA = "MachineConfig.xml";
            public const string TESTER_SETTING = "TesterSetting.xml";
            public const string BIN_COLOR_FILE = "WaferMap.BinColor.xml";
			public const string UI_SETTING = "UISetting.xml";
			public const string USER_MANAGE_FILE = "UserTable.bin";
            public const string SYSCAL_FILE = "SysCal.xml";

			public const string DEFAULT_FILENAME = "Default";

            public const string RDFUNC_FILENAME = "RDFunc";
            public const string RDFUNC_FILE_EXTENSION = "dat";

			public const string TASK_SHEET_EXTENSION = "ts";
			public const string TASK_SHEET_FILTER = "*.ts";
            public const string BIN_FILE_EXTENSION = "bin";
            public const string BIN_FILE_FILTER = "*.bin";
			public const string PRODUCT_FILE_EXTENSION = "pd";
			public const string PRODUCT_FILE_FILTER = "*.pd";
            public const string CONDITION_FILE_EXTENSION = "cond";
            public const string CONDITION_FILE_FILTER = "*.cond";
			public const string MAPDATA_FILE_EXTENSION = "map";
			public const string MAPDATA_FILE_FILTER = "*.map";

            public const string CALIBRATE_FILE_EXTENSION = ".cal";
            public const string CALIBRATE_FILE_FILTER = "*.cal";

            public const string USER_DEFINE_FILENAME = "UserDefine";
            public const string OUTPUT_XML_TEMP = "OutputTemp.xtmp";
			public const string OUTPUT_CSV_TEMP = "OutputTemp.otmp";
			public const string OUTPUT_CSV_TEMP_SORTER = "OutputTemp.otmpS";
            public const string STATISTICS_TEMP = "OutputTemp.stmp";
			//---------------------------------------------
			// System files
			public const string SYSTEM_USER_TABLE = "Users.doug";
			// data files
			public const string REGISTRY_DATA = "Registry.dat";
			public const string USER_MASK = "UserMask.xml";
			public const string GRABBER_SETTING = "FrameGrabberSetting.xml";
			// resource file
			public const string RESOURCE_PAK_FILE = "Resource.zip";
			public const string STRING_FILE_FILTER = "STR_*.xml";
			// recipe file
			//public const string PRODUCT_RECIPE_EXTENSION = ".xml";
			public const string RECIPT_FILE_PATTERN_EXTENSION = "bmp";
			public const string RECIPT_FILE_FILTER = "*.xml";
			public const string BIN_TABLE_RECIPT_FILE_EXTENSION = "zip";
			public const string BIN_TABLE_RECIPT_FILE_FILTER = "*.Zip";
			// System execute file
			public const string SYSTEM_FILE = "System.rtss";
			// AOI
			public const string AOI_PROGRAM = "AutoMotion.exe";
			public const string AOI_BAD_DIE_FRAME_FILE = "BadDieFrame.txt";
			public const string AOI_BAD_DIE_FILE = "BadDie.txt";
			// barcode print
			public const string BARCODE_PRINTER_SETTING_FILE = "PrinterSetting.xml";
			public const string BARCODE_PRINTER_PROGRAM = "MPIBarcodePrinter.exe";
			public const string BARCODE_PRINTER_PROGRAM_LIB = "tsclib.dll";
        }
    }
}
