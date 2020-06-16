using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using MPI.Tester;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Device.SwitchSystem
{
    public class K3706A : ISwitch
    {
        #region >>> private Constan Property <<<

        private const string SWITCH_CARD_CONFIG_PATH = @"C:\MPI\LEDTester\Data";
        private const int SYSTEM_MAX_SLOT_NUM = 6;

        #endregion

        private IConnect _conn;
        private LANSettingData _lanSetting;

        private const string CONFIG_FILE_EXTENSION = "kei";  // config file 副檔名 *.kei / *.csv

        private SwitchSettingData _config;
        private EDevErrorNumber _errorNum;

        private SlotStatus[] _slotStatus;
        Dictionary<int, string> _chMapping;
        private string _serialNum;
        private string _swVersion;
        private string _hwVersion;

        private MPI.PerformanceTimer _pt;

        public K3706A()
        {
            this._errorNum = EDevErrorNumber.Device_NO_Error;

            this._lanSetting = new LANSettingData();

            this._slotStatus = new SlotStatus[SYSTEM_MAX_SLOT_NUM];

            for (int i = 0; i < SYSTEM_MAX_SLOT_NUM; i++)
            {
                this._slotStatus[i] = new SlotStatus();
            }

            this._chMapping = new Dictionary<int, string>();

            this._pt = new PerformanceTimer();
        }

        public K3706A(SwitchSettingData config) : this()
        {
            this._config = config;
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
            get { return this._chMapping.Count; }
        }

        #endregion

        #region >>> Private Method <<<

        private bool GetDeviceInfomation()
        {
            string cmd = string.Empty;

            //------------------------------------------------------------------------------------------
            // Get Switch System Info.
            //------------------------------------------------------------------------------------------
            this._conn.SendCommand("reset()");

            this._conn.SendCommand("print(localnode.model, localnode.serialno, localnode.revision)");

            string result = string.Empty;

            this._conn.WaitAndGetData(out result);

            string[] devInfo = result.Replace("\n", "").Split('\t');

            if (devInfo == null)
            {
                this._errorNum = EDevErrorNumber.SwitchHWInitFail;
                Console.WriteLine("[K3706A], Can't get device information!");
                return false;
            }

            string model = devInfo[0];

            if(model == string.Empty)
            {
                this._errorNum = EDevErrorNumber.SwitchHWInitFail;
                Console.WriteLine("[K3706A], Can't get device model name!");
                return false;
            }

            string serialNum = devInfo[1];

            this._serialNum = string.Format("Keithley {0}_{1}", model, serialNum);

            this._hwVersion = model;

            //------------------------------------------------------------------------------------------
            // Get Switch Card Info from each slot
            //
            //   Return Infomation:
            //   3722, Dual 1x48 Multiplexer, 01.00a, <Module Serial Number>
            //   3721, Dual 1x20 Multiplexer, 01.02a, <Module Serial Number>
            //   Empty Slot
            //------------------------------------------------------------------------------------------
            uint cardInstallNum = 0;

            devInfo = null;

            cmd = string.Format("for x=1,{0} do print (slot[x].idn) end", SYSTEM_MAX_SLOT_NUM);

            this._conn.SendCommand(cmd);

            System.Threading.Thread.Sleep(50);

            this._conn.WaitAndGetData(out result);

            devInfo = result.Split('\n');

            for (int slotIndex = 0; slotIndex < SYSTEM_MAX_SLOT_NUM; slotIndex++)
            {
                if (devInfo[slotIndex] == "Empty Slot")
                {
                    this._slotStatus[slotIndex].Model = string.Empty;
                    this._slotStatus[slotIndex].SerialNum = string.Empty;
                }
                else
                {
                    string[] slotInfo = devInfo[slotIndex].Split(',');

                    this._slotStatus[slotIndex].Model = slotInfo[0];
                    this._slotStatus[slotIndex].SerialNum = slotInfo[3];

                    cardInstallNum++;
                }
            }

            if (cardInstallNum == 0)
            {
                this._errorNum = EDevErrorNumber.SwitchNoCardInstall;
                Console.WriteLine("[K3706A], No switch cards Installedl!!");
                return false;
            }

            return true;
        }

        private bool CheckSwitchCardConfig()
        {
            string[][] importData = null;

            string model = string.Empty;

            int dutChannel = -1;

            int slotNum = -1;  // base-1

            int devChannel = -1;

            string fileNameWithExt = string.Format("ConfigData_{0}.{1}", this._hwVersion, CONFIG_FILE_EXTENSION);

           //string fileNameWithExt = "ConfigData_3706A.csv";

            if (CSVUtil.ReadFromCSV(Path.Combine(SWITCH_CARD_CONFIG_PATH, fileNameWithExt), out importData) == false)
            {
                Console.WriteLine("[K3706A], Read config file fail!!, FileName = {0}", fileNameWithExt);
                return false;
            }

            string strChannel = string.Empty;

            int contentsLength = importData.Length;

            this._chMapping.Clear();

            for (int row = 0; row < contentsLength; row++)
            {
                string header = importData[row][0];

                if (header == string.Empty || !header.Contains("["))
                {
                    continue;
                }

                switch (header)
                {
                    case "[Description]":
                        {
                            break;
                        }
                    case "[Pin Assignment]":
                        {
                            for (int i = row + 2; i < contentsLength; i++)
                            {
                                if (importData[i][0].Contains("["))
                                {
                                    row = i - 1;
                                    break;
                                }
                                
                                // DUT Channel
                                if (!int.TryParse(importData[i][0], out dutChannel))
                                {
                                    continue;
                                }

                                dutChannel -= 1;

                                if (dutChannel < 0)
                                {
                                    return false;
                                }

                                // Switch Card Model
                                model = importData[i][1];

                                // Slot Number
                                if (!int.TryParse(importData[i][2], out slotNum))
                                {
                                    continue;
                                }

                                // Switch card Channel
                                if (!int.TryParse(importData[i][3], out devChannel))
                                {
                                    continue;
                                }

                                // Check Switch Card Config
                                if (slotNum > 0 && slotNum <= this._slotStatus.Length)
                                {
                                    string hwModel = this._slotStatus[slotNum - 1].Model;

                                    if (hwModel != string.Empty && hwModel == model)
                                    {
                                        strChannel = slotNum.ToString("D1") + devChannel.ToString("D3") + ",";

                                        if (this._chMapping.ContainsKey(dutChannel))
                                        {
                                            this._chMapping[dutChannel] += strChannel;
                                        }
                                        else
                                        {
                                            this._chMapping.Add(dutChannel, strChannel);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("[K3706A], switch card model not match, H/W = {0}; Config = {1}", hwModel, model);
                                        return false;
                                    }
                                }
                            }
                            
                            break;
                        }
                }
            }

            if (this._chMapping.Count == 0)
            {
                Console.WriteLine("[K3706A], No Config Data");
                return false;
            }

            return true;
        }

        private bool SetConfig()
        {
            string cmd = string.Empty;

            cmd += "beeper.enable = 0\n";

            cmd += "errorqueue.clear()\n";

            cmd += "display.clear()\n";

            cmd += "display.setcursor(1, 3)\n";

            cmd += "display.settext(\"MPI CORPORATION\")\n";

            cmd += "display.setcursor(2, 4)\n";

            cmd += "display.settext(\"" + this._lanSetting.IPAddress + "\")\n";

            cmd += "channel.connectrule = channel.OFF\n";

            this._conn.SendCommand(cmd);

            if (this.GetErrorMsg())
            {
                this.Close();

                return false;
            }

            return true;
        }

        private bool GetErrorMsg()
        {
            string cmd = string.Empty;

            cmd += "errorCode, message, severity, errorNode = errorqueue.next()\n";

            cmd += "print(message)";

            this._conn.SendCommand(cmd);

            string msg = string.Empty;

            this._conn.WaitAndGetData(out msg);

            if (msg == "Queue Is Empty\n")
            {
                return false;
            }

            Console.WriteLine("[K3706A], GetErrorMsg()," + msg);

            return true;
        }

        private void GetRelaySwitchingCount()
        {
            if (this._chMapping.Count == 0)
            {
                return;
            }

            string cmd;

            string relayCount;

            for (int i = 0; i < this._chMapping.Count; i++)
            {
                cmd = string.Empty;

                relayCount = string.Empty;

                cmd += "print(channel.getcount(\"" + this._chMapping[i] + "\"))";

                this._conn.SendCommand(cmd);

                this._conn.WaitAndGetData(out relayCount);
            }
        }

        private void DelayTime(double delayTime, bool isThreadSleep = false)
        {
            if (delayTime > 0.0d)
            {
                if (delayTime >= 30.0d)
                {
                    System.Threading.Thread.Sleep((int)delayTime);
                }
                else
                {
                    this._pt.Start();

                    do
                    {
                        if (isThreadSleep)
                        {
                            System.Threading.Thread.Sleep(0);
                        }

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
        }


        #endregion

        #region >>> Public Method <<<

        public bool Init(string switchSystemSN)
        {
            // Lan Connect Setting
            string connInfo = string.Empty;

            this._lanSetting.IPAddress = switchSystemSN;

            this._conn = new LANConnect(this._lanSetting);

            if (!this._conn.Open(out connInfo))
            {
                this._errorNum = EDevErrorNumber.SwitchHWInitFail;
                Console.WriteLine("[K3706A], LAN open fail!!");
                return false;
            }

            if (!this.GetDeviceInfomation())
            {
                // Error Code & Lod 在 GetDeviceInfomation() 內
                return false;
            }


            if (!this.CheckSwitchCardConfig())
            {
                this._errorNum = EDevErrorNumber.SwitchConfigDataMissing;

                return false;
            }

            if (!this.SetConfig())
            {
                this._errorNum = EDevErrorNumber.SwitchHWInitFail;
                
                return false;
            }

            //this.GetChannelSwitchCount();

            return true;
        }

        public void Reset()
        {
           
        }

        public bool EnableCH(uint index)
        {
            bool rtn = false;

            string cmd = string.Empty;

            cmd = "channel.exclusiveclose(\"" + this._chMapping[(int)index] + "\")";

            rtn = this._conn.SendCommand(cmd);

            this.DelayTime(20.0d);
           
            return rtn;
        }

        public bool DisableCH()
        {
            bool rtn = true;
           
            return rtn;
        }

        public void Close()
        {
            string cmd = string.Empty;

            cmd = "channel.open(\"allslots\")\n";

            cmd += "display.sendkey(75)";

            this._conn.SendCommand(cmd);

            this._conn.Close();
        } 

        #endregion
    }

    internal class SlotStatus
    {
        private string _model;
        private string _serialNum;

        public SlotStatus()
        {
            this._model = string.Empty;
            this._serialNum = string.Empty;
        }

        #region >>> Public Property <<<

        public string Model
        {
            get { return this._model; }
            set { this._model = value; }
        }

        public string SerialNum
        {
            get { return this._serialNum; }
            set { this._serialNum = value; }
        }

        #endregion
    }

}
