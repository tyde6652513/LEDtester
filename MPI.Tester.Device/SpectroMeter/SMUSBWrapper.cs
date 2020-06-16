using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;


namespace MPI.Tester.Device.SpectroMeter
{
    //-------------------------------------------------------------------------------
    //  Ver. 1.00 , Update Date = 20100715, Add new function interface. By Relife
    //  Ver. 0.80 , Update Date = 20091027, Add new function interface. By Relife
    //-------------------------------------------------------------------------------

	/// <summary>
	/// SMUSB driver wrapper class.
	/// </summary>
    public class SMUSBWrapper
    {
        public const int AUTO_MAX_COUNT = 60000;
        public const int AUTO_MIN_COUNT = 10000;
        public const int AUTO_INTERVAL_COUNT = 10000;

        #region >>> DLL Declaration( Login, Set accesor and Configure ) <<<

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "Login", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern Int32 LoginA(ref UInt16 pSerialNumber, Int32 nDeviceNumber);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "LoginFixtrue", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern Int32 LoginFixtrueA(ref UInt16 pSerialNumber);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "ReConnect", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern Int32 ReConnectA();

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "SetSimulateMode", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern Int32 SetSimulateModeA(int nmode, int nlevel);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "FixArguError", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern Int32 FixArguErrorA(int nStep);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "SetAutoITWorkSpace", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern Int32 SetAutoITWorkSpaceA(int nButton, int nTop);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "SetAutoITLimit", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern Int32 SetAutoITLimitA(int nIT);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "SetAutoITStart", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern Int32 SetAutoITStartA(int nIT);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "ConfigCIE1931Yxy", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern void ConfigCIE1931YxyA(int nSW);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "ConfigCIE1931PeakWavelength", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern void ConfigCIE1931PeakWavelengthA(int nSW);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "ConfigCIE1931LeltaLanda", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern void ConfigCIE1931LeltaLandaA(int nSW);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "ConfigCIE1931DominateWavelength", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern void ConfigCIE1931DominateWavelengthA(int nSW);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "ConfigCIE1931Purity", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern void ConfigCIE1931PurityA(int nSW);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "ConfigCIE1931CentroidWavelength", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern void ConfigCIE1931CentroidWavelengthA(int nSW);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "ConfigCIE1931CorrelatedColorTemperature", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern void ConfigCIE1931CorrelatedColorTemperatureA(int nSW);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "ConfigCIE1931ColorRenderingIndex", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern void ConfigCIE1931ColorRenderingIndexA(int nSW);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "ConfigCIE1931LuminousFlux", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern void ConfigCIE1931LuminousFluxA(int nSW);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "ConfigCIE1931Illuminance", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern void ConfigCIE1931IlluminanceA(int nSW);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "ConfigWatt", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern void ConfigWattA(int nSW);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "ConfigFixArguError", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern void ConfigFixArguErrorA(int nSW);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "ConfigPeakWavelengthInter", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern void ConfigPeakWavelengthInterA(int nSW);

			//-------------------------------------------------------------
			// 20100715 Added New Functions
			//--------------------------------------------------------------

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "SetLightProcessByMutiTrigger", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]              //LightProcessByMutiTrigger 改為 SetLightProcessByMutiTrigger
			private static extern int SetLightProcessByMutiTriggerA(int nState,int nIT);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "SetFixtrueFilterAddress", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern int SetFixtrueFilterAddressA(int nFilterAddress, ref double pFilterPersent);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "SetBoxcar", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern int SetBoxcarA(int nLevel);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "SetXaxisDPI", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]   
			private static extern int SetXaxisDPIA(int nOrder);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "SetYaxisDPI", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]   
			private static extern int SetYaxisDPIA(int nOrder);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "SetD", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]          // 1:Endble   0:Disable
			private static extern void SetDA(int nSw);

        #endregion

        #region >>> DLL Declaration( Trigger, Get aceesor, GetData... ) <<<

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "SDUSBTrigger", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern Int32 SDUSBTriggerA(Int32 nIT, ref double pData, ref Int32 pLS, ref Int32 pPeak);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "TriggerAgain", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern Int32 TriggerAgainA(Int32 nMode);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "GetWavelength", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern Int32 GetWavelengthA(ref double pData, ref double pMinWave, ref double pMaxWave, ref Int32 pWaveLength);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "GetCIE1931Yxy", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern Int32 GetCIE1931YxyA(ref double pY, ref double px, ref double py);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "GetCIE1931PeakWavelength", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern Int32 GetCIE1931PeakWavelengthA(ref double pLpV, ref double pLpIR);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "GetCIE1931LeltaLanda", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern Int32 GetCIE1931LeltaLandaA(ref double pLL);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "GetCIE1931DominateWavelength", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern Int32 GetCIE1931DominateWavelengthA(ref double pLd);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "GetCIE1931CentroidWavelength", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern Int32 GetCIE1931CentroidWavelengthA(int nMode, ref double pLC);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "GetCIE1931Purity", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern Int32 GetCIE1931PurityA(ref double pPurity);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "GetCIE1931CorrelatedColorTemperature", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern Int32 GetCIE1931CorrelatedColorTemperatureA(ref double pCCT);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "GetCIE1931ColorRenderingIndex", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern Int32 GetCIE1931ColorRenderingIndexA(ref double pRn, ref double pRa);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "GetCIE1931LuminousFlux", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern Int32 GetCIE1931LuminousFluxA(ref double pLm);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "GetCIE1931Illuminance", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern Int32 GetCIE1931IlluminanceA(ref double pLx);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "GetWatt", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern Int32 GetWattA(ref double pEnergy);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "GetAutoITInformation", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern Int32 GetAutoITInformationA(ref int pCurrentIT, ref int pCurrentPeak);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "GetPeakWavelengthInter", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern Int32 GetPeakWavelengthInterA(ref int nSW);

			//-------------------------------------------------------------
			// 20100715 Added New Functions
			//--------------------------------------------------------------

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "GetFixtrueFilterInitialInformation", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]        // filter count , filter percentage
			private static extern void GetFixtrueFilterInitialInformationA(ref int nCnt,ref double dFilter);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "GetFixtrueFilterAddress", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern void GetFixtrueFilterAddressA(ref int nFilterAddress, ref double pFilterPersent);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "GetXYasixData", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern int GetXYasixDataA(int nMode, ref double pXasix, ref double pYasix);

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "GetLastVersion", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern string GetLastVersionA();

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "GetFactoryCalibrationDate", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern string GetFactoryCalibrationDateA();

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "GetCustomerCalibrationDate", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
			private static extern string GetCustomerCalibrationDateA();

			[DllImport(@"C:\MPI\LEDTester\SDUSB.dll", EntryPoint = "GetD", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]          
			private static extern void GetDA();

        #endregion

        #region >>> Public Method <<<

        /// <summary>
        /// Login the spectrometer and confirm the serial number
        /// 0: success.
        /// 1: 光譜卡硬體尚未連接/驅動程式未安裝
        /// 2: 光譜卡通訊失敗
        /// 3. 光譜卡序號錯誤.
        /// 4. 光譜卡序號登入失敗[有找到端點]
        /// 5. 光譜卡序號登入失敗[沒有找到端點]
        /// 6. 治具序號錯誤
        /// </summary>         
        //public int Login(UInt16[] pSN,int nDeviceNumber)
        public int Login(string serielNumber, int nDeviceNumber)
        {
            Encoding ascii = Encoding.ASCII;
            Encoding unicode = Encoding.Unicode;

            if ( nDeviceNumber < 0 )
                return -1;

            if ( serielNumber.Length == 0)
                return -1;

            // Convert the string into a byte[].
            byte[] unicodeBytes = unicode.GetBytes(serielNumber);

            // Perform the conversion from one encoding to the other.
            byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);

            UInt16[] pSN = new UInt16[asciiBytes.Length];

            for (int i = 0; i < pSN.Length; i++)
            {
                pSN[i] = asciiBytes[i];
            }

            return LoginA(ref pSN[0], nDeviceNumber);
        }

        /// <summary>
        /// Login the spectrometer and confirm the serial number of sphere
        /// 0: success.
        /// 1: 光譜卡硬體尚未連接/驅動程式未安裝
        /// 2: 光譜卡通訊失敗
        /// 3. 光譜卡序號錯誤.
        /// 4. 治具序號登入失敗[有找到端點]
        /// 5. 治具序號登入失敗[沒有找到端點]
        /// 6. 治具序號錯誤
        /// </summary>      
        //public int LoginFixtrue(UInt16[] pSN)
        public int LoginFixtrue(string serielNumber)
        {
            Encoding ascii = Encoding.ASCII;
            Encoding unicode = Encoding.Unicode;

            if ( serielNumber.Length == 0 )
                return -1;

            // Convert the string into a byte[].
            byte[] unicodeBytes = unicode.GetBytes(serielNumber);

            // Perform the conversion from one encoding to the other.
            byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);

            UInt16[] pSN = new UInt16[asciiBytes.Length];

            for (int i = 0; i < pSN.Length; i++)
            {
                pSN[i] = asciiBytes[i];
            }

            int nlog = 9;
            nlog = LoginFixtrueA(ref pSN[0]);
            return nlog;
        }

        /// <summary>
        ///修正答案
        /// 0: success.
        /// 1: 誤動作，設定為不修正，請參照ConfigCIE1931Yxy()
        /// </summary>       
        public int FixArguError(int nStep)
        {
            nStep = FixArguErrorA(nStep);
            return 0;
        }

        /// <summary>
        /// Re-connect to the spectrometer
        /// 0: success.
        /// 1: Re-connection fail. Check hardware and re-start it
        /// </summary>     
        public int ReConnect()
        {
            return ReConnectA();
        }

        /// <summary>
        /// Get the wavelength ( x axis of spectrum ) ( unit:nm )
        /// 0: success.
        /// 1: 光譜卡尚未連接
        /// 2: 波長尚未定義 (Login中的參數pSerialNumber是否正確)
        /// </summary>     
        public int GetWavelength(double[] pWave, ref double pMinWave, ref double pMaxWave, ref int pWaveLength)
        {
            int nlog = 9;
            nlog = GetWavelengthA(ref pWave[0], ref pMinWave, ref pMaxWave, ref pWaveLength);
            return nlog;
        }

        /// <summary>
        /// Trigger the spectrometer and get the intensity data
        /// 0: success.
        /// 1: 光譜卡硬體尚未連接/驅動程式未安裝
        /// 2: 無法挽回的光譜卡通訊失敗，重新啟動。
        /// 3: 有機會挽回的光譜卡通訊失敗，呼叫 ReConnect()。
        /// </summary>      
        public int SDUSBTrigger(int nIT, double[] pData, ref int pLS, ref int pPeak)
        {
            return SDUSBTriggerA(nIT, ref pData[0], ref pLS, ref pPeak);
        }

        public int TriggerAgain(int nMode)
        {
            return TriggerAgainA(nMode);
        }

        /// <summary>
        /// Get CIE1931 x, y chromacity coordinates and Y 
        /// 0: success.
        /// 1: fail, disable the calculation, please refer to ConfigCIE1931Yxy()
        /// </summary>    
        public int xy(ref double pY, ref double px, ref double py)
        {
            return GetCIE1931YxyA(ref pY, ref px, ref py); 
        }

        /// <summary>
        /// Get the dominant wavelength, peak wavelength, Full Width Half Width ( FWHM, HW )
        /// 0: success
        /// 100: 誤動作，設定為不運算，請參照ConfigCIE1931PeakWavelength()
        /// 10: 誤動作，設定為不運算，請參照ConfigCIE1931LeltaLanda()
        /// 1: 誤動作，設定為不運算，請參照GetCIE1931DominateWavelength()
        /// </summary>   
        public int LpLdLL(ref double pLpV, ref double pLpIR, ref double pLd, ref double pLL)
        {
            Int32 a=0,b=0,c=0;
            a = GetCIE1931PeakWavelengthA(ref pLpV, ref pLpIR);
            b = GetCIE1931LeltaLandaA(ref pLL);
            c = GetCIE1931DominateWavelengthA(ref pLd);

            if (a != 0)
            {
                return a * 100;
            }
            else if (b != 0)
            {
                return b * 10;
            }
            else if (c != 0)
            {
                return c * 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Get purity
        /// 0: success
        /// 1: fail, disable the calculation, please refer toConfigCIE1931Purity()
        /// </summary>    
        public int Purity(ref double pPurity)
        {
            return GetCIE1931PurityA(ref pPurity);
        }

        /// <summary>
        /// Get CIE1931 Centroid Wavelength
        /// 0: success
        /// 1: fail, disable the calculation, please refer to ConfigCIE1931CentroidWavelength()
        /// </summary>    
        public int LC(int nMode, ref double pLC)
        {
            return GetCIE1931CentroidWavelengthA(nMode,ref pLC);
        }

        /// <summary>
        /// Get CIE1931 Correlated Color Temperature ( CCT )
        /// 0: success
        /// 1: fail, disable the calculation, please refer to ConfigCIE1931CorrelatedColorTemperature()
        /// </summary>    
        public int CCT(ref double pCCT)
        {
            return GetCIE1931CorrelatedColorTemperatureA(ref pCCT);
        }

        /// <summary>
        /// Get Color Rendering Index ( CRI ) Data, 
        /// Special color rendering index (Ri) is calculated for 14 reflective samples 
        /// General color rendering index (Ra) is simply the average of Ri  for the first eight samples
        /// 0: success.
        /// 1: fail, disable the calculation, please refer to  ConfigCIE1931ColorRenderingIndex()
        /// </summary>    
        public int CRI(double[] pRn, ref double pRa)
        {
            return GetCIE1931ColorRenderingIndexA(ref pRn[0], ref pRa);
        }

        /// <summary>
        /// Get CIE1931 Illuminace
        /// 0: success.
        /// 1: fail, disable the calculation, please refer to ConfigCIE1931Illuminance()
        /// </summary>    
        public int Lx(ref double pLx)
        {
            return GetCIE1931IlluminanceA(ref pLx);
        }

        /// <summary>
        /// Get CIE1931 Luminous Flux ( Lm )
        /// 0: success.
        /// 1: fail, disable the calculation, please refer to ConfigCIE1931LuminousFlux()
        /// </summary>    
        public int Lm(ref double pLm)
        {
            return GetCIE1931LuminousFluxA(ref pLm);
        }

        /// <summary>
        /// Get the lighting power ( radiant power )
        /// 0: success.
        /// 1: fail, disable the calculation, please refer to ConfigWatt()
        /// </summary>    
        public int Watt(ref double pEnergy)
        {
            return GetWattA(ref pEnergy);
        }

        /// <summary>
        ///取得波峰值是否採用內插增加解析度
        /// 0: success.
        /// 1: 誤動作，設定錯誤，請參照ConfigPeakWavelengthInter()
        /// </summary>    
        public int GetPWI(ref int pSW)  //PWLI : PeakWavelengthInter
        {
            return GetPeakWavelengthInterA(ref pSW);            
        }

        /// <summary>
        /// Set operation Mode
        /// 0: success.
        /// 1: nMode = 0, 1.
        /// 2: When nMode = 1, this mode have 3 level definition, nLevel = 1, 2, 3.
        /// </summary>    
        public int SimulateMode(int nMode, int nLevel)
        {
            if ( nMode < 0 || nMode > 1)
                return -1;

            if ( nMode == 1 )
            {
                if (nLevel < 0 || nLevel > 2)
                {
                    return -1;
                }
            }

            return SetSimulateModeA(nMode, nLevel);
        }

        /// <summary>
        /// Get cuurent integration time and max count at auto-trigger function
        /// 0: success.
        /// 1: fail, no trigger the action, please refer to SDUSBTrigger()
        /// </summary>    
        public int AutoIT(ref int pCurrentIT, ref int pCurrentPeak)
        {
            return GetAutoITInformationA(ref pCurrentIT, ref pCurrentPeak);
        }

        /// <summary>
        /// Set button count and top count for auto-trigger function.
        /// 0: success;
        /// 1: setting count is lower than  10000;
        /// 2: setting count is higher than 60000;
        /// 3: interval between button count and top count is less than 10000;
        /// </summary>
        public int AIWS(int nButton, int nTop)
        {
            if (nButton < AUTO_MIN_COUNT || nTop < AUTO_MIN_COUNT)
                return 1;

            if ( nButton > AUTO_MAX_COUNT || nTop > AUTO_MAX_COUNT) 
                return 2;

            if ( ( nTop - nButton ) < AUTO_INTERVAL_COUNT || (nTop - nButton) < 0 ) 
                return 3;

            if ( nButton < 0 || nTop < 0 )
                return 3;

            return SetAutoITWorkSpaceA(nButton, nTop);
        }

        public int SetAutoITLimit(int nIT)
        {
            return SetAutoITLimitA(nIT);
        }

        public int SetAutoITStart(int nIT)
        {
            return SetAutoITStartA(nIT);
        }

        /// <summary>
        /// Set configuration for CIExy,Wavelength information, CCT, CRI, mW, Lx, LM
        /// Enable or Disable
        /// </summary>
        /// <param name="nSW">1 = Enable, 0 = Disable</param>
        public void SetAllConfig(int nSW)
        {
            ConfigCIE1931YxyA(nSW);
            ConfigCIE1931PurityA(nSW);
            ConfigCIE1931DominateWavelengthA(nSW);

            ConfigCIE1931PeakWavelengthA(nSW);
            ConfigCIE1931LeltaLandaA(nSW);  
            ConfigCIE1931CentroidWavelengthA(nSW);

            ConfigCIE1931CorrelatedColorTemperatureA(nSW);
            ConfigCIE1931ColorRenderingIndexA(nSW);

            ConfigCIE1931LuminousFluxA(nSW);
            ConfigCIE1931IlluminanceA(nSW);
            ConfigWattA(nSW);

            //-----------------------------------------------------
            // Never use it. The function WAS NOT verified
            //-----------------------------------------------------
            // ConfigFixArguErrorA(nSW);

            //-----------------------------------------------------
            // Enable Interpolation Fuction for WLP calculation
            //-----------------------------------------------------
            ConfigPeakWavelengthInterA(nSW);
        }

        /// <summary>
        /// Set CRI calculation enable / disable
        /// </summary>
        /// <param name="nSW">1 = Enable, 0 = Disable</param>
        public void SetConfigCRI(int nSW)
        {
            ConfigCIE1931ColorRenderingIndexA(nSW);
        }

        #endregion

        #region >>> Public Method 20100715 <<<       
        //-------------------------------------------------------------
        // 20100715 Added New Functions
        //--------------------------------------------------------------

        public Int32 SetMutiTriggerProcess(int nState, int nParam01)
        {
            return SetLightProcessByMutiTriggerA(nState, nParam01);
        }

        public int SetFixtrueFilterAddress(int nFilterAddress, ref double pFilterPersent)
        {
            return SetFixtrueFilterAddressA(nFilterAddress, ref pFilterPersent);
        }

        public int SetBoxcar(int nLevel)
        {
            int n = SetBoxcarA(nLevel);
            return n;
        }

        public Int32 SetXaxisOrder(int nOrder)
        {
            return SetXaxisDPIA(nOrder);
        }

        public Int32 SetYaxisOrder(int nOrder)
        {
            return SetYaxisDPIA(nOrder);
        }

        public void SetEnableGetRawData(int nSw)
        {
            SetDA(nSw);
        }

        //------------------------------------------------------

        public void GetFilterInitInformation(ref int nCnt,double[] dFilter)
        {
            GetFixtrueFilterInitialInformationA(ref nCnt,ref dFilter[0]);
        }

        public void GetFixtrueFilterAddress(ref int nFilterAddress, ref double pFilterPersent)
        {
            GetFixtrueFilterAddressA(ref nFilterAddress, ref pFilterPersent);
        }

        /// <summary>
        /// Get spectrometer wavelength and y-count
        /// nMode = 1, raw data from spectrometer, 0 ~ 2048 pixel
        /// nMode = 2, multiply by gain compensation, 0 ~ 1501 points (default )
        /// nMode = 3, multiply by gain compensation and post filter process, 0 ~ 1501 points
        /// </summary>
        public int GetXYasixData(int nMode, double[] pXasix, double[] pYasix)
        {
            if ( nMode < 1 || nMode > 3 )
                return -1;

            return GetXYasixDataA(nMode, ref pXasix[0], ref pYasix[0]);
        }

        public string GetLastVersion()
        {
            return GetLastVersionA();
        }

        public string GetFactoryCalibrationDate()
        {
            return GetFactoryCalibrationDateA();
        }

        public string GetCustomerCalibrationDate()
        {
            return GetCustomerCalibrationDateA();
        }

        public void GetRawData()
        {
            GetDA();
        }

        #endregion


    }
}
