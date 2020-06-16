using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPI;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Device.SwitchSystem
{
    public class MultiFuncBox : ISwitch
    {
        #region >>> private Constan Property <<<

        private const int PCI_1756 = 1;
        private const int PIO_821H = 2;

        private const int DI = 1;
        private const int DO = 2;
        private const int AI = 3;
        private const int AO = 4;

        private const int MAX_SWITCH_CARD_CH_NUM   = 16;
        private const ushort IC74138_EN_CTRL_BYTES = 0x1000;
        private const ushort CH_INIT_CTRL_BYTES    = 0x0000;
        
        //---------------------------------------------------------------------------------------------------------------------------------------------
        // Device Ctrl Description                                       Card Ctrl      PCI      Port ID     Mask             CMD ID  
        //---------------------------------------------------------------------------------------------------------------------------------------------
        private static int[][] _moduleCtrlAsg = new int[][] { new int[] {  PCI_1756,     DO,        2,      0x1FFF  },      // 0: Ctrl Channel Switch
                                                              new int[] {  PIO_821H,     DO,        0,      0x000F  },      // 1: Ctrl Polar Switch
                                                              new int[] {  PIO_821H,     DO,        1,      0x00FF  },      // 2: Ctrl Contact
                                                              new int[] {  PCI_1756,     DO,        0,      0x0001  },      // 3: Ctrl SPT Trigger-1
                                                              new int[] {  PCI_1756,     DO,        0,      0x0002  },      // 4: Ctrl SPT Trigger-2
                                                              new int[] {  PIO_821H,     DO,        0,      0x0040  },      // 5: Ctrl SPT Trigger-3
                                                              new int[] {  PIO_821H,     DO,        0,      0x0080  },      // 6: Ctrl SPT Trigger-4
                                                            };

        //---------------------------------------------------------------------------------------------------------------------------------------------------------
        // Device Status Description                                       Card Ctrl     PCI      Port ID     Mask        offset       CMD ID  
        //---------------------------------------------------------------------------------------------------------------------------------------------------------
        private static int[][] _moduleStateAsg = new int[][] {  new int[] {  PCI_1756,    DI,      0,        0x0001,        0  },      // 0: IO-PowerON
                                                                new int[] {  PCI_1756,    DI,      0,        0x000E,        1  },      // 1: Dev1-Sensor
                                                                new int[] {  PCI_1756,    DI,      0,        0x0070,        4  },      // 2: Dev2-Sensor
                                                                new int[] {  PCI_1756,    DI,      1,        0xFFFF,        7  },      // 3: ExtIO
                                                                new int[] {  PCI_1756,    DI,      2,        0x0007,        0  },      // 4: Switch Card HW Ver.
                                                                new int[] {  PIO_821H,    AI,      0,        0x0001,        0  },      // 5: Polar Detector +
                                                                new int[] {  PIO_821H,    AI,      8,        0x0001,        0  },      // 6: Polar Detector -
                                                                new int[] {  PIO_821H,    DI,      0,        0x0001,        0  },      // 7: DA-PowerON
                                                                new int[] {  PIO_821H,    DI,      0,        0x0002,        1  },      // 8: Needle ON (4W4P Contact)
                                                                new int[] {  PIO_821H,    DI,      0,        0x001C,        2  },      // 9: Polar Switch HW Ver.
                                                                new int[] {  PIO_821H,    DI,      0,        0x00E0,        5  },      // 10: Attenuator HW Ver.
                                                                new int[] {  PIO_821H,    DI,      1,        0x0007,        0  },      // 11: Dev3-Sensor
                                                                new int[] {  PIO_821H,    DI,      1,        0x0038,        3  },      // 12: Dev4-Sensor
                                                             };

        //----------------------------------------------------------------------------------------------------------------------------
        // PCI-1756 Ctrl Switch Card                                                 CH OFF (Hex)    CH ON (Hex)           CH ID  
        //----------------------------------------------------------------------------------------------------------------------------
        private static ushort[][] _chCtrlAsgTable = new ushort[][] {    new ushort[] { 0x0000,       0x0001 },            // CH1
                                                                        new ushort[] { 0x0002,       0x0003 },            // CH2
                                                                        new ushort[] { 0x0004,       0x0005 },            // CH3
                                                                        new ushort[] { 0x0006,       0x0007 },            // CH4
                                                                        new ushort[] { 0x0000 << 3,  0x0001 << 3 },       // CH5
                                                                        new ushort[] { 0x0002 << 3,  0x0003 << 3 },       // CH6
                                                                        new ushort[] { 0x0004 << 3,  0x0005 << 3 },       // CH7
                                                                        new ushort[] { 0x0006 << 3,  0x0007 << 3 },       // CH8
                                                                        new ushort[] { 0x0000 << 6,  0x0001 << 6 },       // CH9
                                                                        new ushort[] { 0x0002 << 6,  0x0003 << 6 },       // CH10
                                                                        new ushort[] { 0x0004 << 6,  0x0005 << 6 },       // CH11
                                                                        new ushort[] { 0x0006 << 6,  0x0007 << 6 },       // CH12
                                                                        new ushort[] { 0x0000 << 9,  0x0001 << 9 },       // CH13
                                                                        new ushort[] { 0x0002 << 9,  0x0003 << 9 },       // CH14
                                                                        new ushort[] { 0x0004 << 9,  0x0005 << 9 },       // CH15
                                                                        new ushort[] { 0x0006 << 9,  0x0007 << 9 },       // CH16
                                                                    };

        //---------------------------------------------------------------------------------------------------------------------------------------------------
        // Multi-Func Mainframe Module                      IO_Card     DA_Card     SwitchCard      PolarSwitch     Needle      Attenutor       FilterWheel
        //---------------------------------------------------------------------------------------------------------------------------------------------------
        private static bool[] _MFB_A_ModuleEn = new bool[] { true,      true,       true,           true,           true,       true,           true };
        private static bool[] _MFB_B_ModuleEn = new bool[] { true,      false,      true,           false,          false,      false,          true };
        private static bool[] _MFB_C_ModuleEn = new bool[] { false,     true,       false,          true,           true,       true,           true };
        private static bool[] _MFB_D_ModuleEn = new bool[] { true,      false,      false,          false,          false,      false,          true };

        #endregion

        private PCI1756Wrapper _ioCard;
        private PIO821HWrapper _daqCard;
        private PerformanceTimer _pt;

        private SwitchSettingData _mfbConfig;
        private EDevErrorNumber _errorNum;

        private bool[] _isHwInit;

        private uint _chRecordIdx;   // 0-base
        private int _devSeningMode;

        private double _dataReadyDelay;   // ms
        private double _onIC74138EnDelay;

        private int _sbVer;
        private int _polarSwitchVer;
        private int _attenuatorVer;

        private string _serialNum;
        private string _swVersion;
        private string _hwVersion;

        public MultiFuncBox()
        {
            this._pt = new PerformanceTimer();

            this._isHwInit = new bool[Enum.GetNames(typeof(EDevHwInfo)).Length];

            this._devSeningMode = (int)ESrcSensingMode._4wire;

            this._errorNum = EDevErrorNumber.Device_NO_Error;

            this._dataReadyDelay = 0.5d;   // 目前設定 1.0, 最快可用 0.5
            this._onIC74138EnDelay = 2.5d; // 目前設定 3.0, 最快可用 2.5

            this._mfbConfig = new SwitchSettingData();
        }

        public MultiFuncBox(SwitchSettingData config)
            : this()
        {
            this._mfbConfig = config;
            this._devSeningMode = (int)config.MsrtSensingMode;
        }

        #region >>> Public Property <<<

        public string SerialNumber
        {
            get { return this._serialNum; }
        }

        public string SoftwareVersion
        {
            get { return this._swVersion; }
        }

        public string HardwareVersion
        {
            get { return this._hwVersion; }
        }

        public EDevErrorNumber ErrorNumber
        {
            get { return this._errorNum; }
        }

        public int MaxSwitchingChannelCount
        {
            get { return 8; }
        }

        // Additional Property
        public int DevSensingMode
        {
            get { return (int)this._devSeningMode; }
            set { this._devSeningMode = value; }
        }

        public double ChOnDelay
        {
            get { return this._dataReadyDelay; }
            set { this._dataReadyDelay = value; }
        }

        public double Relay17OnDelay
        {
            get { return this._onIC74138EnDelay; }
            set { this._onIC74138EnDelay = value; }
        }


        #endregion

        #region >>> Private Method <<<

        private bool RunDevCtrlCmd(EDevCtrl cmd, ushort data)
        {
            int cardNum = _moduleCtrlAsg[(int)cmd][(int)EDevCtrlDesc.CardName];
            int pciCtrl = _moduleCtrlAsg[(int)cmd][(int)EDevCtrlDesc.PCIFunc];
            int portID = _moduleCtrlAsg[(int)cmd][(int)EDevCtrlDesc.PortID];
            int mask = _moduleCtrlAsg[(int)cmd][(int)EDevCtrlDesc.Mask];

            switch (cardNum)
            {
                case PCI_1756:
                    if (pciCtrl == DO)
                    {
                        this._ioCard.WriteDO(portID, (ushort)(data & mask));
                    }
                    else
                        return false;
                    break;
                case PIO_821H:
                    if (pciCtrl == DO)
                    {
                        this._daqCard.WriteDO(portID, (uint)(data & mask));
                    }
                    else if (pciCtrl == AO)
                    {
                        //this._daqCard.WriteAO(portID, (uint)data);
                    }
                    else
                        return false;
                    break;
                default:
                    return false;
            }

            return true;
        }

        private bool GetDevStateCmd(EDevState cmd, out double data)
        {
            data = 0;
            byte tempData01 = 0x00;
            uint tempData02 = 0;

            int cardName = _moduleStateAsg[(int)cmd][(int)EDevCtrlDesc.CardName];
            int pciFunc  = _moduleStateAsg[(int)cmd][(int)EDevCtrlDesc.PCIFunc];
            int portID   = _moduleStateAsg[(int)cmd][(int)EDevCtrlDesc.PortID];
            int mask     = _moduleStateAsg[(int)cmd][(int)EDevCtrlDesc.Mask];
            int offset   = _moduleStateAsg[(int)cmd][(int)EDevCtrlDesc.Offset];

            switch (cardName)
            {
                case PCI_1756:
                    if (pciFunc == DI)
                    {
                        this._ioCard.ReadDI(portID, out tempData01);
                        data = Convert.ToInt32((tempData01 & mask) >> offset);
                    }
                    else
                        return false;
                    break;
                case PIO_821H:
                    if (pciFunc == DI)
                    {
                        this._daqCard.ReadDI(portID, out tempData02);
                        data = (int)(tempData02 & mask) >> offset;
                    }
                    else if (pciFunc == AI)
                    {
                        this._daqCard.ReadAI(portID, out data);
                    }
                    else
                        return false;
                    break;
                default:
                    return false;
            }

            return true;
        }

        private bool CheckHardwareConfig()
        {
            string tempStr = string.Empty;
            double data;

            for (int i = 0; i < this._isHwInit.Length; i++)
            {
                this._isHwInit[i] = false;
            }

            if (this._ioCard != null)
            {
                // Power On, Module connected to PCI1756 
                this.GetDevStateCmd(EDevState.IoPowerOn, out data);

                this._isHwInit[(int)EDevHwInfo.IO_Card] = Convert.ToBoolean(data);

                if (this._isHwInit[(int)EDevHwInfo.IO_Card])
                {
                    // Switch Card Version
                    this.GetDevStateCmd(EDevState.SbHwVer, out data);
                    this._sbVer = (int)data;

                    if (this._sbVer != 0)
                    {
                        this._isHwInit[(int)EDevHwInfo.SwitchCard] = true;

                        tempStr = string.Format("SB-{0}", this._sbVer);
                    }

                    this._isHwInit[(int)EDevHwInfo.FilterWheel] = true;
                }
            }

            if (this._daqCard != null)
            {
                // Power On, Module connected to PIO821H
                this.GetDevStateCmd(EDevState.DaPowerOn, out data);
       
                this._isHwInit[(int)EDevHwInfo.DA_Card] = Convert.ToBoolean(data);

                if (this._isHwInit[(int)EDevHwInfo.DA_Card])
                {
                    // Needle On (4W4P Contact)
                    this.GetDevStateCmd(EDevState.NeedleON, out data);

                    this._isHwInit[(int)EDevHwInfo.Needle] = Convert.ToBoolean(data);
                    
                    tempStr += string.Format("/ N-{0}", data);

                    // Polar Switch Ver.
                    this.GetDevStateCmd(EDevState.PolarHwVer, out data);
                    this._polarSwitchVer = (int)data;

                    if (this._polarSwitchVer != 0)
                    {
                        this._isHwInit[(int)EDevHwInfo.PolarSwitch] = true;

                        tempStr += string.Format("/ PS-{0}", this._polarSwitchVer);
                    }

                    // Attenuator Ver. (Polar Detector)
                    this.GetDevStateCmd(EDevState.AttenuHwVer, out data);
                    this._attenuatorVer = (int)data;

                    if (this._attenuatorVer != 0)
                    {
                        this._isHwInit[(int)EDevHwInfo.Attenutor] = true;

                        tempStr += string.Format("/ PD-{0}", this._attenuatorVer);
                    }
                }
            }

            if (!this.CheckModuleState())
            {
                return false;
            }

            this._serialNum = string.Format("MFB [{0}]", tempStr);

            return true; 
        }

        private bool CheckModuleState()
        {
            bool[] moduleStateArray;

            moduleStateArray = _MFB_B_ModuleEn;

            //switch (this._mfbConfig.SwitchCardModel)
            //{
            //    case (int)EMFBSwitchCards.Model_A:
            //        moduleStateArray = _MFB_A_ModuleEn;
            //        break;
            //    case (int)EMFBSwitchCards.Model_B:
            //        moduleStateArray = _MFB_B_ModuleEn;
            //        break;
            //    case (int)EMFBSwitchCards.Model_C:
            //        moduleStateArray = _MFB_C_ModuleEn;
            //        break;
            //    case (int)EMFBSwitchCards.Model_D:
            //        moduleStateArray = _MFB_D_ModuleEn;
            //        break;
            //    default:
            //        return false;
            //}

            if (this._isHwInit.Length != moduleStateArray.Length)
            {
                return false;
            }

            for (int i = 0; i < this._isHwInit.Length; i++)
            {
                if (this._isHwInit[i] != moduleStateArray[i])
                {
                    return false;
                }
            }
   
            return true;
        }

        private bool RunChSwitchCtrl(uint chIdx, bool isOpen)
        {
            ushort chCtrlLSB = 0x0000;
            ushort chCtrlMSB = 0x0000;
            ushort chCtrlData;

            //-----------------------------------------------------------
            // check the index in the range of 2-wire / 4-wire channels
            //-----------------------------------------------------------
            if (this._devSeningMode == (int)ESrcSensingMode._2wire)
            {
                if (chIdx >= MAX_SWITCH_CARD_CH_NUM)
                    return false;
            }
            else
            {
                if (chIdx >= MAX_SWITCH_CARD_CH_NUM / 2)
                    return false;
            }

            //-----------------------------------------------------------
            // (A) Channel Hex Data
            //-----------------------------------------------------------
            chCtrlLSB = _chCtrlAsgTable[chIdx][Convert.ToInt32(isOpen)];

            if (this._devSeningMode == (int)ESrcSensingMode._4wire)
            {
                chCtrlMSB = _chCtrlAsgTable[chIdx + (MAX_SWITCH_CARD_CH_NUM / 2)][Convert.ToInt32(isOpen)];
            }

            chCtrlData = (ushort)(chCtrlLSB | chCtrlMSB);

            this.RunDevCtrlCmd(EDevCtrl.ChCtrl, chCtrlData);

            this.DelayTime(this._dataReadyDelay);

            //-----------------------------------------------------------
            // (B) IC74138En  -> High
            //-----------------------------------------------------------
           // this._ioCard.WriteDO(_moduleCtrlAsg[(int)EDevCtrl.ChCtrl][(int)EDevCtrlDesc.PortID], IC74138_EN_CTRL_BYTES);
            this.RunDevCtrlCmd(EDevCtrl.ChCtrl, (ushort)(IC74138_EN_CTRL_BYTES | chCtrlData));
            this.DelayTime(this._onIC74138EnDelay);
           
            //-----------------------------------------------------------
            // (C) IC74138En -> Low, ready for stating test sequence
            //-----------------------------------------------------------
            this.RunDevCtrlCmd(EDevCtrl.ChCtrl, CH_INIT_CTRL_BYTES);
            //this._ioCard.WriteDO(_moduleCtrlAsg[(int)EDevCtrl.ChCtrl][(int)EDevCtrlDesc.PortID], CH_INIT_CTRL_BYTES);
            
            return true;
        }

        private void DelayTime(double delayTime)
        {
            if (delayTime >= 100.0d)
            {
                System.Threading.Thread.Sleep((int)delayTime);
            }
            else
            {
                this._pt.Start();
                do
                {
                    if (this._pt.PeekTimeSpan(ETimeSpanUnit.MilliSecond) >= delayTime)
                    {
                        this._pt.Stop();
                        this._pt.Reset();
                        return;
                    }
                    System.Threading.Thread.Sleep(0);
                } while (this._pt.PeekTimeSpan(ETimeSpanUnit.MilliSecond) < delayTime);
                this._pt.Stop();
                this._pt.Reset();
            }
        }

        #endregion

        #region >>> Public Method <<<

        public bool Init(string switchSystemSN)
        {
            if (this._ioCard == null)
            {
                this._ioCard = new PCI1756Wrapper();
            }

            if (this._daqCard == null)
            {
                this._daqCard = new PIO821HWrapper();
            }

            try
            {
                if (!this._ioCard.Init())
                {
                    this._ioCard = null;

                    this._errorNum = EDevErrorNumber.SwitchHWInitFail;

                    return false;
                }

                //if (!this._daqCard.Init())
                //    this._daqCard = null;
            }
            catch
            {
                this._errorNum = EDevErrorNumber.SwitchHWInitFail;

                return false;
            }

            if (!this.CheckHardwareConfig())
            {
                this._errorNum = EDevErrorNumber.SwitchNoCardInstall;
                return false;
            }

            this.Reset();

            return true;
        }

        public void Reset()
        {
            if (this._isHwInit[(int)EDevHwInfo.SwitchCard])
            {
                for (uint i = 0; i < MAX_SWITCH_CARD_CH_NUM / 2; i++)
                {
                    this.RunChSwitchCtrl(i, false);
                }
            }
        }

        public bool EnableCH(uint index)
        {
            bool rtn = false;

            if (this._ioCard == null)
                return false;
            
            this._chRecordIdx = index;

            rtn =  this.RunChSwitchCtrl(this._chRecordIdx, true);

            return rtn;
        }

        public bool DisableCH()
        {
            bool rtn = false;

            if (this._ioCard == null)
                return false;

            rtn = this.RunChSwitchCtrl(this._chRecordIdx, false);

            return rtn;
        }

        public void Close()
        {
            if (this._ioCard != null)
            {
                this._ioCard.Close();
            }
        }

        #endregion

        #region >>> Enumeration <<<

        private enum EDevHwInfo : int
        {
            IO_Card     = 0,
            DA_Card     = 1,
            SwitchCard  = 2,
            PolarSwitch = 3,
            Needle      = 4,
            Attenutor   = 5,
            FilterWheel = 6,
        }

        private enum EDevCtrl : int
        {
            ChCtrl      = 0,
            PolarSwitch = 1,
            Contact     = 2,
            SptTrig1    = 3,
            SptTrig2    = 4,
            SptTrig3    = 5,
            SptTrig4    = 6,  
        }

        private enum EDevState : int
        {
            IoPowerOn    = 0,
            Dev1_Sensor  = 1,
            Dev2_Sensor  = 2,
            ExtIO        = 3,
            SbHwVer      = 4,
            PD_Anode     = 5,
            PD_Cathode   = 6,
            DaPowerOn    = 7,
            NeedleON     = 8,
            PolarHwVer   = 9,
            AttenuHwVer  = 10,
            Dev3_Sensor  = 11,
            Dev4_Sensor  = 12,

        }

        private enum EDevCtrlDesc : int
        {
            CardName = 0,
            PCIFunc  = 1,
            PortID   = 2,
            Mask     = 3,
            Offset   = 4,
        }          

        private enum EPolarState : int
        {
            Anode   = 1,
            Normal  = 0,
            Cathode = -1
        }

        #endregion
    }
}
