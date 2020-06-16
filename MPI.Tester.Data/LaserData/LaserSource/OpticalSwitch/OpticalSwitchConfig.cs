using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections;
using System.ComponentModel;

using MPI.Tester.DeviceCommon;


namespace MPI.Tester.Data.LaserData.LaserSource
{
    [Serializable]
    public class OpticalSwitchConfig :ICloneable
    {
        public OpticalSwitchConfig()
        {
            OpticalSwitchModel = EOpticalSwitchModel.NONE;
            Address = "";
            LaserSysChannel = 0;
            OpticalInputChannel = 0;
            OpticalOutputChannel = 0;
            Enable = true;
            Slot = 0;
            IOState = 0;
            WatchPinList = new List<int>();
        }

        #region
        private int _inputChannel = 0;
        #endregion

        #region >>public property<<
        [DisplayName("Model ")]
        public EOpticalSwitchModel OpticalSwitchModel { set; get; }

        [ReadOnly(true)]
        public int LaserSysChannel { set; get; }

        public bool Enable { set; get; }
        /// <summary>
        /// 在2611 IO模式，作為檢測OS狀態的腳位
        /// </summary>
        [DisplayName("Input Channel")]
        [Description("This parameter is trigger IO pin In OSW12 ")]
        public int OpticalInputChannel 
        {
            set
            {
                if (WatchPinList != null )
                {
                    if (WatchPinList.Count > 0)
                    {
                        WatchPinList[0] = _inputChannel;
                    }
                    else
                    {
                        WatchPinList.Add(_inputChannel);
                    }
                }
                _inputChannel = value;
            }
            get { return _inputChannel; }
        }
         /// <summary>
        /// 在2611 IO模式，作為驅動OS的腳位
        /// </summary>  
        [DisplayName("Output Channel")]
        [Description("This parameter is IO state in OSW12 ")]
        public int OpticalOutputChannel { set; get; }
        

        [DisplayName("Address")]
        public string Address { set; get; }
        
        public int Slot { set; get; }


        /// <summary>
        /// 在2611 IO模式，作為檢測OS狀態的腳位
        /// </summary>
        [Description("It is the state IO pin In OSW12 or OSW1xN ")]
        public int IOState{ set; get; }

        /// <summary>
        /// 在2611 IO模式，按順序列舉出bit0 = pin7, bit1 = pin8...etc
        /// </summary>
         [DisplayName("Read Pin List")]
        [Description("It is the IO pin read list In OSW1xN. If you set pin7 as bit1 and pin10 as bit2, state = pin7 * 2^0 + pin10 * 2^1")]
        public List<int> WatchPinList { set; get; }

         [Browsable(false)]
         public int[] BitPinArr
         {
             get
             {
                 if (WatchPinList != null)
                 { return WatchPinList.ToArray(); }
                 return null;
             }
             set
             {
                 if (value != null)
                 {
                     WatchPinList = new List<int>();
                 }
                 WatchPinList.AddRange(value);
             }
         }

        #endregion

        #region >>public method<<

        public object Clone()
        {
            OpticalSwitchConfig obj = new OpticalSwitchConfig();
            obj.Address = Address;
            obj.Enable = Enable;
            obj.IOState = IOState;
            obj.WatchPinList = new List<int>();
            obj.WatchPinList.AddRange(BitPinArr);
            obj.OpticalInputChannel = OpticalInputChannel;
            obj.OpticalOutputChannel = OpticalOutputChannel;
            obj.OpticalSwitchModel = OpticalSwitchModel;
            obj.LaserSysChannel = LaserSysChannel;
            obj.Slot = Slot;
            
            return obj;
        }
        #endregion
    }

}
