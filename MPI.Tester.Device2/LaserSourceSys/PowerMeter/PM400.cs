using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;

using MPI.Tester.Device;
using MPI.Tester.DeviceCommon;
using MPI.Tester.Data.LaserData.LaserSource;
using MPI.Tester.Device.SourceMeter;
using Thorlabs.PM100D_32.Interop;

namespace MPI.Tester.Device.LaserSourceSys.PowerMeter
{
    public class PM400 : IPowerMeter
    {
        PM100D _tlpm;
        bool _isUseAvgTime = true;
        #region >>constructor<<
        

        public PM400()
        {
            Address = "";
            Slot = 0;
            NowsysCh = 0;
            ErrorNumber = EDevErrorNumber.Device_NO_Error;
            SerialNumber = "";
            SysChDevDic = new Dictionary<int, int>();

            Spec = new PowerMeterSpec();
            Spec.Channel = new MinMaxValuePair<int>(1, 1);
            Connect = null;
        }

        public PM400(PowerMeterConfig PMCfg )
            : this()
        {

            Address = PMCfg.Address;
            Slot = PMCfg.Slot;
            ErrorNumber = EDevErrorNumber.Device_NO_Error;
            SerialNumber = "";
            //Push(PMCfg.LaserSysChannel, PMCfg.PowerMeterChannel);
            
        }

        #endregion

        #region >>public property<<

        public string Address { get; set; }
        public int Slot { get; set; }
        public int NowsysCh { get; set; }
        public string SerialNumber { get; private set; }
        public string SoftwareVersion { get; private set; }
        public string HardwareVersion { get; private set; }

        public Dictionary<int, int> SysChDevDic { get; set; }
        public EDevErrorNumber ErrorNumber { get; set; }
        public PowerMeterSpec Spec { set; get; }
        public IConnect Connect { get;set; }
        #endregion

        public bool Init(IConnect conect)
        {
            Connect = conect;

            if (Connect == null)
            {
                #region
                bool isInitSuccess = true;
                HandleRef Instrument_Handle = new HandleRef();
                PM100D searchDevice = new PM100D(Instrument_Handle.Handle);
                try
                {
                    uint devCount = 0;
                    int pInvokeResult = searchDevice.findRsrc(out devCount);
                    string firstPowermeterFound = Address.Trim();// "USB0::0x1313::0x8075::P5001497::INSTR";

                    if (devCount > 0)
                    {
                        for (int i = 0; i < devCount; ++i)
                        {
                            StringBuilder descr = new StringBuilder(1024);

                            searchDevice.getRsrcName((uint)i, descr);

                            StringBuilder Model_Name = new StringBuilder(1024);
                            StringBuilder Serial_Number = new StringBuilder(1024);
                            StringBuilder Manufacturer = new StringBuilder(1024);
                            bool Device_Available = false;
                            int v = searchDevice.getRsrcInfo((uint)i, Model_Name, Serial_Number, Manufacturer, out  Device_Available);

                            string str = Serial_Number.ToString().Trim();
                            
                            if (str == Address.Trim())
                            {
                                Console.WriteLine("[PM400]Init(),serial Num: " + str);
                                firstPowermeterFound = descr.ToString();
                                searchDevice.Dispose();
                                break;
                            }
                        }

                        if (firstPowermeterFound != "")
                        {
                            _tlpm = new PM100D(firstPowermeterFound, false, false);  //  For valid Ressource_Name see NI-Visa documentation.
                            //_tlpm.reset();
                            Connect = new DriverObjConnect(_tlpm);
                            _tlpm.setPowerRefState(false);
                            _tlpm.setPowerAutoRange(true);
                            if (_isUseAvgTime)
                            {
                                _tlpm.setAvgTime(0.1);
                            }
                            short unit = 0;//0 w,1 dBm
                            _tlpm.setPowerUnit(unit);//W
                        }
                        else
                        {
                            isInitSuccess = false;
                        }

                    }
                    else
                    {
                        isInitSuccess = false;
                    }

                    if (!isInitSuccess)
                    {
                        searchDevice.Dispose();
                        ErrorNumber = EDevErrorNumber.LASER_PowerMeter_Init_Err;
                        return false;
                    }
                }
                catch (Exception e)
                {
                    ErrorNumber = EDevErrorNumber.LASER_PowerMeter_Init_Err;
                    Console.WriteLine("[PM400]Init(),Exception" + e.Message);
                    return false;
                }

                System.GC.Collect();
                #endregion
            }
            else
            {
                try
                {
                    _tlpm = ((Connect as DriverObjConnect).DriverObject as PM100D);
                }
                catch (Exception e)
                {
                    ErrorNumber = EDevErrorNumber.LASER_PowerMeter_Init_Err;
                    Console.WriteLine("[PM400],Init(),Exception" + e.Message);
                    return false;
                }

            }

            foreach (var chPair in SysChDevDic)
            {
                if (!Spec.Channel.InRange(chPair.Value))
                {
                    ErrorNumber = EDevErrorNumber.LASER_PowerMeter_Init_Err;
                    return false;
                }
            }

            return true; 
        }

        public bool Push(int sysCh, int devCh)
        {
            if (Spec.Channel.InRange(devCh) && !SysChDevDic.ContainsKey(sysCh))
            {
                SysChDevDic.Add(sysCh, devCh);
            }
            else
            {
                ErrorNumber = EDevErrorNumber.LASER_PowerMeter_Init_Err;
                return false;
            }
            return true;
        }

        public double GetMsrtPower(PowerMeterSettingData pData, int Polarity)//return in W
        {
            double powerValue = 0;

            try
            {
                if (_tlpm != null)
                {
                    _tlpm.setWavelength(pData.WaveLength);
                    //System.Threading.Thread.Sleep(10);

                    //System.Threading.Thread.Sleep(50);


                    if (_isUseAvgTime)
                    { int err = _tlpm.measPower(out powerValue); }
                    else
                    {
                        List<double> vList = new List<double>();
                        for (int i = 0; i < 5; ++i)
                        {
                            System.Threading.Thread.Sleep(50);
                            int err = _tlpm.measPower(out powerValue);
                            vList.Add(powerValue);
                        }
                        powerValue = vList.Average();
                    }
                    powerValue *= pData.SysGain;
                    //powerValue = MPI.Tester.Maths.UnitMath.dBm2W(powerValue);
                }
            }
            catch(Exception e) 
            {
                Console.WriteLine("[PM400],GetMsrtPower(), err: " + e.Message);
                ErrorNumber = EDevErrorNumber.LASER_PowerMeter_Set_Err;
                powerValue = double.NaN;
            }
            return powerValue;
 
        }

        public void Close()
        {
            _tlpm.Dispose();
        }

        public void TurnOff()
        { }
    }
}
