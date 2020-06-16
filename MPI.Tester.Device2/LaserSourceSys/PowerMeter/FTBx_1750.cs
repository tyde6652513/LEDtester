using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.Device;
using MPI.Tester.DeviceCommon;
using MPI.Tester.Data.LaserData.LaserSource;
using MPI.Tester.Device.SourceMeter;

namespace MPI.Tester.Device.LaserSourceSys.PowerMeter
{
    public class FTBx_1750 : IPowerMeter
    {

        protected string preString = "";
        #region >>constructor<<
        

        public FTBx_1750()
        {
            Address = "";
            Slot = 0;
            NowsysCh = 0;
            ErrorNumber = EDevErrorNumber.Device_NO_Error;
            SerialNumber = "";
            SysChDevDic = new Dictionary<int, int>();

            Spec = new PowerMeterSpec();
            Spec.Channel = new MinMaxValuePair<int>(1, 1);//先寫死
            Spec.WavelengthRange = new MinMaxValuePair<double>(200,1700);
            Connect = null;
        }

        public FTBx_1750(PowerMeterConfig PMCfg)
            : this()
        {

            Address = PMCfg.Address;
            Slot = PMCfg.Slot;
            ErrorNumber = EDevErrorNumber.Device_NO_Error;
            SerialNumber = "";
            preString = "LINS" + Slot.ToString();
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
            Connect = null;
            //INST:CAT?

            foreach (var chPair in SysChDevDic)
            {
                if (!Spec.Channel.InRange(chPair.Value))
                {
                    ErrorNumber = EDevErrorNumber.LASER_PowerMeter_Init_Err;
                    return false;
                }
            }

            string msg = "";
            bool isOnlyIPAddressNum = MPI.Tester.Device.LCRMeter.LCRBase.ISThisIPString(Address);

            if (conect == null)
            {
                if (isOnlyIPAddressNum)
                {
                    LANSettingData lanData = new LANSettingData();

                    lanData.IPAddress = Address;

                    this.Connect = new LANConnect(lanData);
                }
                else
                {
                    this.Connect = new IVIConnect(Address);
                }
            }
            else
            {
                this.Connect = conect;
            }


            if (!this.Connect.Open(out msg))
            {
                if (!this.Connect.Open(out msg))//因為N7761連線非常慢，有時會連線逾時，因此用這個方法來避免
                {
                    this.ErrorNumber = EDevErrorNumber.LASER_PowerMeter_Init_Err;
                }

                return false;
            }

            if (!TestIfSlotExist())
            {
                this.ErrorNumber = EDevErrorNumber.LASER_PowerMeter_Slot_Not_Exist_Err;
                return false;
            }

            foreach (var chPair in SysChDevDic)
            {
                Connect.SendCommand(preString + ":UNIT"+chPair.Value.ToString("0")+":POW W");//
                Connect.SendCommand(preString + ":SENS" + chPair.Value.ToString("0") + ":POW:DC:RANG:AUTO 1");//
                Connect.SendCommand(preString + ":SENS" + chPair.Value.ToString("0") + ":AVG:DC:COUN DEF");//
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
                int devCh = pData.SysChannel;
                if (SysChDevDic.ContainsKey(pData.SysChannel))
                {
                    devCh = pData.SysChannel;
                }
                else
                {
                    return 0;
                }
                string msg = "";
                string wlStr = preString + ":SENS" + devCh .ToString()+ ":POW:WAV " + pData.WaveLength.ToString("0") + " nm";


                Connect.SendCommand(wlStr);//shutter on/off
                string str = preString + ":SENS" + devCh.ToString() + ":POW:WAV? ";
                Connect.SendCommand(str);//shutter on/off
                Connect.WaitAndGetData(out msg);
                System.Threading.Thread.Sleep(10);
                //Connect.SendCommand(preString + ":READ:POW:DC?");
                Connect.SendCommand(preString + ":READ" + devCh.ToString() + ":POW:DC?");
                Connect.WaitAndGetData(out msg);
                string[] data = msg.TrimEnd().Split(',');

                if (data != null && data.Length > 0 &&
                    double.TryParse(data[0], out powerValue))
                {
                    return powerValue;
                }
               
            }
            catch (Exception e)
            {
                Console.WriteLine("[FTBx_1750],GetMsrtPower(), err: " + e.Message);
                ErrorNumber = EDevErrorNumber.LASER_PowerMeter_Set_Err;
                powerValue = double.NaN;
            }
            return 0;
        }

        public void Close()
        { }

        public void TurnOff()
        { }

        #region
        private bool TestIfSlotExist()
        {
            string msg = "";
            Connect.SendCommand("INST:CAT:FULL?");//編號會按照LINS設定

            Connect.WaitAndGetData(out msg);

            bool isSlotExist = false;

            if (msg != null && msg != "")
            {
                string[] sArr = msg.Split(',');
                foreach (string str1 in sArr)
                {
                    if (str1.Trim() == Slot.ToString())
                    {
                        isSlotExist = true;
                        break;
                    }
                }
            }
            return isSlotExist;
        }

        #endregion
    }
}
