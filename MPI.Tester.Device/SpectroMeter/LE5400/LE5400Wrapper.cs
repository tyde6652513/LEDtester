using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MPI.Tester.Device.SpectroMeter.LE5400
{
    class LE5400Wrapper
    {
        ///////////////////////////////////////////////////////////////////////
        // macro
        ///////////////////////////////////////////////////////////////////////
        // API function Return Value
        // size limit
        public const int MEAS_DATA_MAX = 512;                // Meas. Data Count 0-511
        public const int CALC_DATA_MAX = 95;                 // Calc. Data Count 0-94

        public const int OCC_SIZE = 5;                  // Sub Peak Count 0-4
        public const int STRBUFF_SIZE = 64;                 // String Size
        public const int DUMMY_AREA_SIZE = 32;                 // Dummy Area Array Size 0-31
        // API function Return Value
        public const int MCPDAPI_SUCCESS = 1;                           // Success(>=1)
        public const int MCPDAPI_ERROR = 0;                             // Other Error(<0 Error the following are Error Code List)
        public const int MCPDAPI_ERROR_PARAM = -1;                      // Parameter Error(Out of range or there is contradiction in relation)
        public const int MCPDAPI_ERROR_MCOND = -2;                      // Measurement Condition not exist(Measurement Condition not exist or  there are problem in condition,please check condition)
        public const int MCPDAPI_ERROR_ACOND = -3;                      // Calculation Condition not exist(Calculation Condition not exist or  there are problem in condition,please check condition)
        public const int MCPDAPI_ERROR_CALC_UNDER = -4;                 // Color Calc Error ->Light Under Error(CalcUnderOver<>3 or CalcUnderOver<>1)
        public const int MCPDAPI_ERROR_CALC_OVER = -5;                  // Color Calc Error ->Light Over Error (CalcUnderOver<>3 or CalcUnderOver<>2)
        public const int MCPDAPI_ERROR_UNSUPPORTED_FUNCTION = -6;       // Not Supported Function(for LE-5000 series)
        public const int MCPDAPI_ERROR_AADJUST = -10;                   // AutoAdjust failed
        public const int MCPDAPI_ERROR_EXEC = -20;                      // The process is called by another functionplease call it later.
        // if it not works please follow the solution of MCPDAPI_ERROR_MACHINE_FUNCTON.
        public const int MCPDAPI_ERROR_ABNORMAL = -21;                  // other error,function failed or The function is not accepted.


        // if it not works please follow the solution of MCPDAPI_ERROR_MACHINE_FUNCTON.
        public const int MCPDAPI_ERROR_MEMORY = -99;            // Out of memory please check the data type of structure


        public const int MCPDAPI_ERROR_MACHINE_CTRL = -100;       // Open Connection Error
        // Please check Port No,or make sure there are not other AP connected to this Machine
        // use GetLastError function to get detail

        public const int MCPDAPI_ERROR_MACHINE_FUNCTON = -101;     // An error occurred during MCPD machinery operation.
        // When it does not dissolve,please reconnect(close->open)
        // or restart pc
        // Please confirm a state of the MCPD machinery (wiring).

        // MSC_GetLastError() Return ValueHIWORD
        public const int MCPDAPI_ERROR_WAIT_TIME_OUT = -1;        // communication timeout,please retry again.
        // if it not works please follow the solution of MCPDAPI_ERROR_ABNORMAL
        public const int MCPDAPI_ERROR_MACHIN_DAMAGE = -2;        // Communication error. please reconnect(colse->open) or restart PC
        // if it not works please follow the solution of MCPDAPI_ERROR_ABNORMAL

        // MSC_GetLastError() Return ValueLOWWORD  
        // API function Detailed code
        //public const int MCPDAPI_ERROR_OPEN = 1;    // Open Error
        //public const int MCPDAPI_ERROR_CLOSE = 2;    // Close Error
        //public const int MCPDAPI_ERROR_SENDENABLE = 3;    // Sendenable Error
        //public const int MCPDAPI_ERROR_SENDSTART = 4;    // Sendstart Error
        //public const int MCPDAPI_ERROR_SENDCLEARINDEX = 5;    // Sendclearindex Error
        //public const int MCPDAPI_ERROR_SENDSETPARAMS = 6;    // Sendsetparams Error
        //public const int MCPDAPI_ERROR_SENDGETPARAMS = 7;    // Sendgetparams Error
        //public const int MCPDAPI_ERROR_SENDGETSTATUS = 8;    // Sendgetstatus Error
        //public const int MCPDAPI_ERROR_SENDSETCONFIGURATION = 9;    // Sendsetconfiguration Error
        //public const int MCPDAPI_ERROR_SENDGETCONFIGURATION = 10;   // Sendgetconfiguration Error
        //public const int MCPDAPI_ERROR_SENDHARDWAREDIRECT = 11;   // Sendhardwaredirect Error
        //public const int MCPDAPI_ERROR_SENDSETDARKDATA = 12;   // Sendsetdarkdata Error
        //public const int MCPDAPI_ERROR_SENDGETDARKDATA = 13;   // Sendgetdarkdata Error
        //public const int MCPDAPI_ERROR_SENDGETVERSION = 14;   // Sendgetversion Error
        //public const int MCPDAPI_ERROR_SENDMODEREQUEST = 15;   // Sendmoderequest Error
        //public const int MCPDAPI_ERROR_SENDGETMODESTATUS = 16;   // Sendgetmodestatus Error
        //public const int MCPDAPI_ERROR_SENDTRANSFERREQUEST = 17;   // Sendtransferrequest Error
        //public const int MCPDAPI_ERROR_SENDWRITEREQUEST = 18;   // Sendwriterequest Error
        //public const int MCPDAPI_ERROR_SENDBINARYDATA = 19;   // Sendbinarydata Error
        //public const int MCPDAPI_ERROR_SENDWRITEIO = 20;   // Sendwriteio Error
        //public const int MCPDAPI_ERROR_SENDREADIO = 21;   // Sendreadio Error
        //public const int MCPDAPI_ERROR_SETPACKETLISTENER = 22;   // Setpacketlistener Error
        //public const int MCPDAPI_ERROR_GETDEFERREDERROR = 23;   // Getdeferrederror Error

        public class DeviceHandling
        {
            #region >>> DLL Import <<<
              //-----------------------------------------------------------------------------
            // (Function Name) OpenMcpd
            // (Description)   Open MCPD Connection and get Connection No.
            // (Parameter)   dwPortNo    virtual serial port no or Serial number of machinery
            //                      LE-4000 series : virtual serial port no
            //                      LE-5000 series : Serial number of machinery or 0(control only one machine)
            //                      LE-7700 series :(when connectmode=3)
            //                                       0-9
            //                                       0:192.168.1.50,2000
            //                                       1:192.168.1.51,2001
            //                                       -
            //                                       9:192.168.1.59,2009
            //
            // (Return Value) =>1:Success, <=0:failed
            // (Remarks)
            //
            //-----------------------------------------------------------------------------
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll", EntryPoint = "OpenMcpd", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
            public static extern int OpenMcpd(int dwPortNo);
            //-----------------------------------------------------------------------------
            // (Function Name) CloseMcpd
            // (Description)   Close Mcpd Connection
            // (Parameter)    dwConnectNo:the the return value of OpenMcpd() function
            // (Return Value) =>1:Success, <=0:failed
            // (Remarks)
            //-----------------------------------------------------------------------------
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int CloseMcpd(int dwConnectNo);
                   
            //-----------------------------------------------------------------------------
            // (Function Name) ResetUSB
            // (Description)   Reset USB
            // (Parameter)     dwSerialNumber  Serial number of LE machine
            //                 if you control only one LE machine,please set this value to 0
            //
            // (Return Value) 1:Success0:Faild (not exist serial number)
            // (Remarks) you can use this function when you terminated abnormally
            //           without an application closing machinery(only for LE-5000 series)
            //-----------------------------------------------------------------------------
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int ResetUSB(int dwSerialNumber);
            //-----------------------------------------------------------------------------
            // (Function Name) GetMcpdMachineCode
            // (Description)   Get Mcpd Machine ID
            // (Parameter)   dwConnectNo: The connection number that acquired in OpenMcpd()
            // (Return Value) HiWord : Mcpd Machine ID
            //                LoWord : not use
            //
            // (Remarks)   Only for LE-4000 series
            //-----------------------------------------------------------------------------
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int GetMcpdMachineCode(int dwConnectNo);


            //-----------------------------------------------------------------------------
            // (Function Name) GetMcpdLastError
            // (Description)   Get Mcpd Error Code
            // (Parameter)   dwConnectNo the return value of OpenMcpd()
            // (Return Value) you can get McpdApiType3 System Error Code list in McpdApiType3.h(the end of file)
            // (Remarks)
            //-----------------------------------------------------------------------------
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int GetMcpdLastError(int dwConnectNo);


            //-----------------------------------------------------------------------------
            // (Function Name) GetCorEngInfo
            // (Description)   Get Energy Proofreading Information
            // (Parameter)   dwCorEngNo      Corr. Energy No.
            //               dwNdNo          ND Filter No(only for LE-4600,LE-7700 series, other series:0)
            //               lpCorEngInfo    Energy Proofreading Information
            // (Return Value) >=1 Success<=1 failed
            // (Remarks)
            //-----------------------------------------------------------------------------
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int GetCorEngInfo(int dwCorEngNo, int dwNdNo, ref CORENGINFO lpCorEngInfo);



            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int GetNDFilter(int dwConnectNo, ref int lpdwAuto, ref int lpdwNdFilter);




            //-----------------------------------------------------------------------------
            // (Function Name) SetConnectMode
            // (Description)   Set Connection Mode
            // (Parameter)   dwConnectMode   0:Virtual COM(LE-4000 series)
            //                               1:direct driver(not support)
            //                               2:High speed(LE-5000 series)
            //                               3:UPD(LE-7700)
            //          nProduct             0:Fixation(only for LE-5400 mode)
            // (Return Value) >=1 Success<= 0 failed
            // (Remarks)   dwConnectMode 1:cannot use the direct driver
            //-----------------------------------------------------------------------------
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int SetConnectMode(int dwConnectMode, int dwProduct);

            //-----------------------------------------------------------------------------
            // (Function Name) GetConnectMode
            // (Description)   Get Connection Mode
            // (Parameter)
            // (Return Value) 0:Virtual COM(only for LE-4000 series) 2:High speed(only for LE-5000 series)
            //                3:UPD(only for LE-7700)
            // (Remarks)    Return Value supports SetConnectMode().
            //-----------------------------------------------------------------------------
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int GetConnectMode();

            //-----------------------------------------------------------------------------
            // (Function Name) EnumMcpd
            // (Description)   Get the count of connected LE-5000 and Serial number List
            // (Parameter)   lpdwNoArg   Serial number List(Return Value)
            //                           Set it null,if you do not need serial number information
            //               dwNoArgNum  Array Count of  Serial number List
            // (Return Value) the count of connected LE-5000
            //
            // (Remarks)   only for LE-5000 series
            //-----------------------------------------------------------------------------
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int EnumMcpd(ref int lpdwNoArg, int dwNoArgNum);


            #endregion

            #region >>> Public Static Method <<<

            //public static int GetDeviceTypeName(EInterfaceType AInterfaceType, out string msg)
            //{
            //    StringBuilder sb = new StringBuilder(MessageStringLength);

            //    int ret = casGetDeviceTypeName(AInterfaceType, sb, sb.Capacity);

            //    msg = sb.ToString();

            //    return ret;
            //}

            //public static int GetDeviceTypeOptionName(EInterfaceType AInterfaceType, int AInterfaceOption, out string msg)
            //{
            //    StringBuilder sb = new StringBuilder(MessageStringLength);

            //    int ret = casGetDeviceTypeOptionName(AInterfaceType, AInterfaceOption, sb, sb.Capacity);

            //    msg = sb.ToString();

            //    return ret;
            //}

            #endregion
        }

        public class ApiHandling
        {
              // Device Handles and Interfaces
            //-----------------------------------------------------------------------------
            // (Function Name) McpdApiInit
            // (Description)   System Initialization
            // (Parameter)   lpdwParam
            //                      0:do not clear log file(Default)
            //                      1:clear log file
            // (Return Value) =>1:Success, <=0:failed
            // (Remarks)
            //          LogFile:App.path\McpdApiType3_Error.log
            //
            //-----------------------------------------------------------------------------
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int McpdApiInit(ref int lpdwParam);

            //-----------------------------------------------------------------------------
            // (Function Name) McpdApiExit
            // (Description)  End System
            // (Parameter)   lpdwParam  1:release energy proofreading table
            //
            // (Return Value) 1:Success
            // (Remarks)
            //-----------------------------------------------------------------------------
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int McpdApiExit(ref int lpdwParam);
        }

      	public class InstrumentProperty
		{
            //-----------------------------------------------------------------------------
            // (Function Name) SyncMode
            // (Description)   synchronization measurement mode
            // (Parameter)   dwConnectNo     the return value of OpenMcpd()
            //               dwEnable        1:synchronization measurement mode(Default)
            //                                    0:async measurement mode
            // (Return Value) 1:Success0:failed
            //-----------------------------------------------------------------------------
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int SyncMode(int dwConnectNo, int dwEnable);

            //-----------------------------------------------------------------------------
            // (Function Name) Enable
            // (Description)   Enable External Trigger
            //-----------------------------------------------------------------------------
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int Enable(int dwConnectNo, int dwCalcMode, int dwContMode, ref MLAC_COND lpMlacCond);

            //-----------------------------------------------------------------------------
            // (Function Name) Disable
            // (Description)   Disable External Trigger
            // (Parameter)   dwConnectNo the return value of OpenMcpd()
            // (Return Value) >=1 Success<=1 failed
            // (Remarks)
            //-----------------------------------------------------------------------------
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int Disable(int dwConnectNo);
        }

        public class MeasurementResult
        {
            //-----------------------------------------------------------------------------
            // (Function Name) SetMeasCondAll
            // (Description)   Set Measurement Condtion
            // (Parameter)   dwConnectNo the return value of OpenMcpd()
            //               lpMmcCond   Measurement Condition
            // (Return Value) >=1 Success<=1 failed
            // (Remarks)      After Mcpd Connection Created.Set Measurement Condtion before you do any operation
            //-----------------------------------------------------------------------------
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int SetMeasCondAll(int dwConnectNo, ref MMC_Cond lpMmcCond);

            //-----------------------------------------------------------------------------
            // (Function Name) AutoAdjust
            // (Description)   AutoAdjust Gate Time
            // (Parameter)   dwConnectNo the return value of OpenMcpd()
            //               lpAADResult Result of AUTOADJUST
            // (Return Value) 1:Success0:failed
            //-----------------------------------------------------------------------------
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int AutoAdjust(int dwConnectNo, out AUTOADJUST_RESULT lpAADResult);

             //-----------------------------------------------------------------------------
            // (Function Name) GetResult
            // (Description)   Get Calc Result(the newest)
            // (Parameter)   dwConnectNo the return value of OpenMcpd()
            //               lpMlarResult result
            // (Return Value) 1Success0failed
            //-----------------------------------------------------------------------------
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int GetResult(int dwConnectNo, ref MLAR_RESULT lpMlarResult);

            //-----------------------------------------------------------------------------
            // (Function Name) RegisterDataCallBack
            // (Description)   Data CallBack Function Register
            // (Parameter)     dwConnectNo the return value of OpenMcpd()
            //                 dwMode      1:Sample Measurement
            //                             2:Dark Measurement
            //                 lpCBFuncMcpdSpectrum    Address of Data CallBack Function
            // (Return Value) 0:failed 1:Sample Measurement Setting Success 2:Dark Measurement Setting Success
            // (Remarks)   Get Spectrum and do  pre-process(before result calc)
            //
            //          Set lpCBFuncMcpdSpectrum = NULL to unregister callback function
            //          Dark do not support pre-process function
            //-----------------------------------------------------------------------------

            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int RegisterDataCallBack(int dwConnectNo, int dwMode,
                 LPCALLBACK_MCPDSPECTRUM lpCBFuncMcpdSpectrum);
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int RegisterDataCallBack(int dwConnectNo, int dwMode,
                 int lpCBFuncMcpdEvent);

            //-----------------------------------------------------------------------------
            // (Function Name) RegisterEventCallBack
            // (Description)   register event callback function
            // (Parameter)   dwConnectNo the return value of OpenMcpd()
            //          dwMode      1:End Meas(A notice of end of PIO:The measurement by the START,or AQUMeas)
            //                      2:End Dark Meas.(A notice of end of PIO:The measurement by the DARK1,2,or AQUDark)
            //                      3:End of AutoAdjust(A notice of end of AutoAdjust)
            //                      4:End of Color Calculation (A notice of end of PIO:Analyze of after the measurement by the START or AQUMeasEx)
            //          lpCBFuncMcpdEvent the Address of Data CallBack Function
            // (Return Value) 0:failed 1:Measurement Setting Success 2:Dark Measurement Setting Success
            //          3:AutoAdjust Setting Success 4:Color Calc Setting Success
            //-----------------------------------------------------------------------------
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int RegisterEventCallBack(int dwConnectNo, int dwMode,
                 LPCALLBACK_MCPDEVENT lpCBFuncMcpdEvent);
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int RegisterEventCallBack(int dwConnectNo, int dwMode,
                 int lpCBFuncMcpdEvent);

            // dwMessageKind : 0 : Raw Data;
            // dwMessageKind : 1 : Abs Data;
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int GetSpectrum(int dwConnectNo, int dwMessageKind, ref MCPD_MEASE_SPECTRUM lpMaSpectrum);

             //-----------------------------------------------------------------------------
            // (Function Name) GetDark
            // (Description)   Get Dark Data(raw  A/D conversion value )
            // (Parameter)   dwConnectNo the return value of OpenMcpd()
            //               lpDarkData  result
            // (Return Value) >=1 Success<=1 failed
            // (Remarks)     the A/D conversion value that is non-revision of 0-65535 ranges
            // (Note)        newest Dark Data
            //-----------------------------------------------------------------------------
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int GetDark(int dwConnectNo, out DARKDATA lpDarkData);
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int SetDark(int dwConnectNo, ref DARKDATA lpDarkData);

            //-----------------------------------------------------------------------------
            // (Function Name) AQUMeas
            // (Description)   Start Measurement
            // (Parameter)   dwConnectNo the return value of OpenMcpd()
            // (Return Value) >=1 Success<=1 failed
            //-----------------------------------------------------------------------------
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int AQUMeas(int dwConnectNo);

            //-----------------------------------------------------------------------------
            // (Function Name) AQUDark
            // (Description)   Start Dark Meas
            // (Parameter)   dwConnectNo the return value of OpenMcpd()
            //               dwShutter   Shutter Mode(1:OPEN 0:CLOSE)
            // (Return Value) >=1 Success<=1 failed
            // (Remarks)   the same with dark current measurement
            // (Note)   After dark mesurement please wait at least 150m before you do measurement,if shutter closed.
            //-----------------------------------------------------------------------------
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int AQUDark(int dwConnectNo, int dwShutter);

            //-----------------------------------------------------------------------------
            // (Function Name) AQUMeasEx
            // (Description)   Measurement and Calc Result
            // (Parameter)     dwConnectNo  the return value of OpenMcpd()
            //                 lpMlacCond   Calculation condition
            //                 lpMlarResult result
            // (Return Value) >=1 Success<=1 failed
            // (Remarks)   please use this function if you do not perform External Trigger
            //-----------------------------------------------------------------------------
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int AQUMeasEx(int dwConnectNo, ref MLAC_COND lpMlacCond, ref MLAR_RESULT lpMlarResult);

            //EX:Calc dwConnectNo,lpMlacCond,0,result
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int Calc(int dwConnectNo, ref MLAC_COND lpMlacCond,
              ref MCPD_MEASE_SPECTRUM MlpMaSpectrum, ref MLAR_RESULT result);

            //EX: Calc dwConnectNo,lpMlacCond,spect,result
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int Calc(int dwConnectNo, ref MLAC_COND lpMlacCond,
              int MlpMaSpectrum, ref MLAR_RESULT result);

         
        }

        public class SaveLoad
        { 
             //-----------------------------------------------------------------------------
            // (Function Name) SaveCond
            // (Description)   Save measurement condition and Mcpd Luminous Analyzed Condition to file
            // (Parameter)   lpCond  Mcpd Meas Condition or Mcpd Luminous Analyzed Condition
            //          dwType      1:lpCond: Use LPMMC_COND
            //                          2:lpCond: Use LPMLAC_COND
            //          lpszFile    file name(full path)
            //          dwFlag      0:do not replace file when file exist
            //                      1:replace file when file exist (even readonly file)
            // (Return Value) 1:Success0:failed
            // (Remarks)
            //-----------------------------------------------------------------------------
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int SaveCond(ref MLAC_COND lpCond, int dwType, string lpszFile, int dwFlag);
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int SaveCond(ref MMC_Cond lpCond, int dwType, string lpszFile, int dwFlag);


            //-----------------------------------------------------------------------------
            // (Function Name) LoadCond
            // (Description)  Get measurement condition and Mcpd Luminous Analyzed Condition from file
            // (Parameter)   lpCond     Mcpd Meas Condition or Mcpd Luminous Analyzed Condition
            //
            //               dwType      1:lpCond: Use LPMMC_COND
            //                           2:lpCond: Use LPMLAC_COND
            //               lpszFile    file name(full path)
            // (Return Value) 1:Success0:failed
            //  (Remarks)  failed when data format mismatch(file) or file not exist
            //
            //-----------------------------------------------------------------------------
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            private static extern int LoadCond(IntPtr lpCond, int dwType, string lpszFile);

            //-----------------------------------------------------------------------------
            // (Function Name) SaveSpectrum
            // (Description)   Save Spectrum
            // (Parameter)   lpSpectrum  the Spectrum been Saved(LPMCPD_MEASE_SPECTRUM or LPMCPD_CALC_SPECTRUM)
            //          dwType      1:lpSpectrum:Use LPMCPD_MEASE_SPECTRUM
            //                      2:lpSpectrum:Use LPMCPD_CALC_SPECTRUM
            //          lpszFile    file name(full path)
            //          dwFlag      0:do not replace file when file exist
            //                      1:replace file when file exist (even readonly file)
            // (Return Value) 1:Success0:failed
            // (Remarks)
            //-----------------------------------------------------------------------------
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int SaveSpectrum(ref MCPD_MEASE_SPECTRUM lpSpectrum, int dwType,
                string lpszFile, int dwFlag);

            //-----------------------------------------------------------------------------
            // (Function Name) LoadSpectrum
            // (Description)    Get Spectrum from file
            // (Parameter)     lpSpectrum  LPMCPD_MEASE_SPECTRUM or LPMCPD_CALC_SPECTRUM
            //                      lpszFile    file name(full path)
            // (Return Value) 1:Success0:failed
            // (Remarks)   failed when data format mismatch(file) or file not exist
            //-----------------------------------------------------------------------------
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            //ref MCPD_MEASE_SPECTRUM lpSpectrum
            public static extern int LoadSpectrum(IntPtr lpSpectrum, string lpszFile);



            //-----------------------------------------------------------------------------
            // (Function Name) SetMessageCode
            // (Description)   Of the sake to notify that the measurement or analysis-colored operation was finished
            //
            // (Parameter)   dwConnectNo     the return value of OpenMcpd()
            //               hWnd            Form.hWnd
            //               dwMessageKind   0:Notify when measurement finished
            //                               1:Notify when Color Calc finished
            //               dwMessageNo     Message No(&H0400-&H7FF)
            // (Return Value) 1:Success0:failed
            //  (Remarks)  unregister: set hWnd with NULL or set message no. that less than &H0400 or larger than &H7FFF
            //          SendMessage: notify by Windows user message
            //          dwMessageKind=0
            //              WAPARM:Dark Meas(0)Sample Meas.(1)
            //              LPARAM:Index Number of Data
            //          dwMessageKind=1
            //              WAPARM: perform Color Calc operation (4),not perform Color Calc operation(5)
            //              LPARAM:Index Number of Data
            //              Not perform Color Calc operation meas error occur when color calculation(Ex:Light Over/Under Error)
            //-----------------------------------------------------------------------------
           
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int SetMessageCode(int dwConnectNo, int hWnd,
                int dwMessageKind, int dwMessageNo);



            // The following functions are the ban on use for inside function
            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int SetAreaMode(int dwConnectNo, int nSet);

            [DllImport(@"C:\MPI\LEDTester\Driver\LE5400\McpdApiType3.dll")]
            public static extern int ClearIndex(int dwConnectNo);

        }
    }
}
