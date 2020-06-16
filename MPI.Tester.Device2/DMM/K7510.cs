using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Device.DMM.Keithley
{
    public class K7510
    {
        private const int DEFAULT_READING_BUFFER_SIZE = 60;
        private int _bufferSize;

        private LANConnect _conn;

        private List<double> _acquireData;
        private List<string> _acquireStrArray;

        private string _errMsg;

        private bool _isTrigger;

        public K7510()
        {
            this._acquireData = new List<double>();

            this._acquireStrArray = new List<string>();

            this._isTrigger = false;

            this._bufferSize = DEFAULT_READING_BUFFER_SIZE;
        }


        #region >>> Public Proberty <<<

        public string ErrorMsg
        {
            get { return this._errMsg; }
        }

        public int ReadingBufferSize
        {
            get { return this._bufferSize; }
            set { this._bufferSize = value; }
        }

        #endregion

        #region >>> Private Method <<<

        private void ConfigDMM(EDmmMeasureFunc defaultFunc)
        {
            string script = string.Empty;
            
            script += "eventlog.clear()\n";
            script += "status.clear()\n";

            script += string.Format("dmm.measure.func = dmm.FUNC_{0}\n", defaultFunc.ToString());

            script += "digio.writeport(0)\n";

            script += "display.changescreen(display.SCREEN_HOME_LARGE_READING)\n";

            if (defaultFunc.ToString().Contains("VOLTAGE"))
            {
                script += "dmm.measure.range = 10\n";

                // Dio Trigger Output Config (For Camera)
                script += "digio.line[" + (int)EDmmDioTriggerOut.PIN1_FFP + "].reset()\n";
                script += "digio.line[" + (int)EDmmDioTriggerOut.PIN1_FFP + "].mode = digio.MODE_TRIGGER_OUT\n";
                script += "trigger.digout[" + (int)EDmmDioTriggerOut.PIN1_FFP + "].logic = trigger.LOGIC_POSITIVE\n";
                script += "trigger.digout[" + (int)EDmmDioTriggerOut.PIN1_FFP + "].pulsewidth = 50e-6\n";
                script += "trigger.digout[" + (int)EDmmDioTriggerOut.PIN1_FFP + "].stimulus = trigger.EVENT_NOTIFY2\n";


                script += "digio.line[" + (int)EDmmDioTriggerOut.PIN2_NFP + "].reset()\n";
                script += "digio.line[" + (int)EDmmDioTriggerOut.PIN2_NFP + "].mode = digio.MODE_TRIGGER_OUT\n";
                script += "trigger.digout[" + (int)EDmmDioTriggerOut.PIN2_NFP + "].logic = trigger.LOGIC_POSITIVE\n";
                script += "trigger.digout[" + (int)EDmmDioTriggerOut.PIN2_NFP + "].pulsewidth = 50e-6\n";
                script += "trigger.digout[" + (int)EDmmDioTriggerOut.PIN2_NFP + "].stimulus = trigger.EVENT_NOTIFY3\n";

                // Dio Trigger Output Config (For SpectroMeter)
                script += "digio.line[" + (int)EDmmDioTriggerOut.PIN3_SPT + "].reset()\n";
                script += "digio.line[" + (int)EDmmDioTriggerOut.PIN3_SPT + "].mode = digio.MODE_TRIGGER_OUT\n";
                script += "trigger.digout[" + (int)EDmmDioTriggerOut.PIN3_SPT + "].logic = trigger.LOGIC_POSITIVE\n";
                script += "trigger.digout[" + (int)EDmmDioTriggerOut.PIN3_SPT + "].pulsewidth = 50e-6\n";
                script += "trigger.digout[" + (int)EDmmDioTriggerOut.PIN3_SPT + "].stimulus = trigger.EVENT_NOTIFY4\n";

                script += "dmm.measure.inputimpedance = dmm.IMPEDANCE_AUTO\n";

            }
            else
            {
                script += "dmm.measure.range = 0.01\n";
            }

            script += "dmm.measure.autorange = dmm.OFF\n";
            script += "dmm.measure.autozero.enable = dmm.OFF\n";
            script += "dmm.measure.autodelay = dmm.DELAY_OFF\n";
           // script += "dmm.digitize.inputimpedance = dmm.IMPEDANCE_AUTO\n";
           // script += "dmm.measure.inputimpedance = dmm.IMPEDANCE_AUTO\n";

            script += "status.operation.setmap(10, 2731, 2732)\n";   // 2731(trigger model initiated); 2732 (trigger model idled)

            // Trigger Link Config (For SMU / Pulser)
            script += "trigger.extin.edge = trigger.EDGE_RISING\n";
            script += "trigger.extout.logic = trigger.LOGIC_POSITIVE\n";
            script += "trigger.extout.stimulus = trigger.EVENT_NOTIFY1\n";

           



            script += "acal.schedule(acal.ACTION_NONE, acal.INTERVAL_8HR)\n";

            this._conn.SendCommand(script);
        }

        private bool MeasuringScript(uint index, DmmSettingData setting)
        {
            string scriptName = "num_" + index.ToString();

            // You must delete an existing script before you can use the name of that script again.
            // Scripts are not automatically overwritten.
            this._conn.SendCommand(string.Format("script.delete(\"{0}\")", scriptName));

            string script = string.Empty;

            string modelScript = string.Empty;

            script = "loadscript " + scriptName + "\n";

            // DMM config
            modelScript = this.MeasuringTriggerModel(setting);

            if (modelScript == string.Empty)
            {
                this._errMsg = "Not Define TriggerOutMode";
                return false;
            }

            script += modelScript;

            script += "trigger.model.initiate()\n";

            // script += "waitcomplete()\n";

            //  script += "printbuffer(1, defbuffer1.n, defbuffer1)\n";

            script += "endscript\n";

            script += scriptName + ".source = nil";

            this._conn.SendCommand(script);

            return !this.GetErrorMsg();
        }

        private string MeasuringTriggerModel(DmmSettingData setting)
        {
            string script = string.Empty;
           
            //---------------------------------------------------------------------------------------------------------------------------
            // Config DIGITIZE / NPLC Measuring, Measure Function and Range
            if (setting.MeasureFunction.ToString().Contains("DIGITIZE"))
            {
                script += string.Format("dmm.digitize.func = dmm.FUNC_{0}\n", setting.MeasureFunction);
                script += string.Format("dmm.digitize.aperture = {0}\n", setting.MeasureApertureTime);
            }
            else
            {
                script += string.Format("dmm.measure.func = dmm.FUNC_{0}\n", setting.MeasureFunction);

                script += string.Format("dmm.measure.range = {0}\n", setting.MeasureRange);

                if (setting.MeasureIntegrationUnit == EDmmDcIntegrationUnit.Aperture)
                {
                    script += string.Format("dmm.measure.aperture = {0}\n", setting.MeasureApertureTime);
                }
                else
                {
                    script += string.Format("dmm.measure.nplc = {0}\n", setting.MeasureNPLC);
                }
            }

            //---------------------------------------------------------------------------------------------------------------------------
            // Config the Model
            int blockIdx = 1;

            switch (setting.TriggerOutMode)
            {
                case EDmmDioTriggerOut.NONE:
                    {
                        script += string.Format("trigger.model.load(\"{0}\")\n", "Empty");
                        script += string.Format("trigger.model.setblock({0}, trigger.BLOCK_BUFFER_CLEAR)\n", blockIdx++);   // could be removed
                        script += string.Format("trigger.model.setblock({0}, trigger.BLOCK_WAIT, trigger.EVENT_EXTERNAL)\n", blockIdx++); 
                        script += string.Format("trigger.model.setblock({0}, trigger.BLOCK_DELAY_CONSTANT, {1})\n", blockIdx++, setting.TriggerInputDelay);
                        script += string.Format("trigger.model.setblock({0}, trigger.BLOCK_MEASURE, defbuffer1, 1)\n", blockIdx++);
                        script += string.Format("trigger.model.setblock({0}, trigger.BLOCK_BRANCH_COUNTER, {1}, 2)\n", blockIdx++, setting.TriggerCount);
                        break;
                    }
                case EDmmDioTriggerOut.PIN1_FFP:
                    {
                        script += string.Format("trigger.model.load(\"{0}\")\n", "Empty");
                        script += string.Format("trigger.model.setblock({0}, trigger.BLOCK_BUFFER_CLEAR)\n", blockIdx++);   // could be removed

                        script += string.Format("trigger.model.setblock({0}, trigger.BLOCK_NOTIFY, trigger.EVENT_NOTIFY2)\n", blockIdx++);   // DIO TriggerOut NFP Camera with EVENT_NOTIFY2
                        script += string.Format("trigger.model.setblock({0}, trigger.BLOCK_DELAY_CONSTANT, {1})\n", blockIdx++, setting.TriggerOutDelay);  // Tsoe Delay for Camera

                        script += string.Format("trigger.model.setblock({0}, trigger.BLOCK_NOTIFY, trigger.EVENT_NOTIFY1)\n", blockIdx++);  // Ext.Trigger Out SMU with EVENT_NOTIFY1

                        script += string.Format("trigger.model.setblock({0}, trigger.BLOCK_WAIT, trigger.EVENT_EXTERNAL)\n", blockIdx++);   // Ext.Trigger IN for starting measure with EVENT_EXTERNAL
                        script += string.Format("trigger.model.setblock({0}, trigger.BLOCK_DELAY_CONSTANT, {1})\n", blockIdx++, setting.TriggerInputDelay);
                        script += string.Format("trigger.model.setblock({0}, trigger.BLOCK_MEASURE, defbuffer1, 1)\n", blockIdx++);
                        script += string.Format("trigger.model.setblock({0}, trigger.BLOCK_BRANCH_COUNTER, {1}, 2)\n", blockIdx++, setting.TriggerCount);
                        break;
                    }
                case EDmmDioTriggerOut.PIN2_NFP:
                    {
                        script += string.Format("trigger.model.load(\"{0}\")\n", "Empty");
                        script += string.Format("trigger.model.setblock({0}, trigger.BLOCK_BUFFER_CLEAR)\n", blockIdx++);   // could be removed

                        script += string.Format("trigger.model.setblock({0}, trigger.BLOCK_NOTIFY, trigger.EVENT_NOTIFY3)\n", blockIdx++);   // DIO TriggerOut FFP Camera with EVENT_NOTIFY3
                        script += string.Format("trigger.model.setblock({0}, trigger.BLOCK_DELAY_CONSTANT, {1})\n", blockIdx++, setting.TriggerOutDelay);  // Tsoe Delay for Camera

                        script += string.Format("trigger.model.setblock({0}, trigger.BLOCK_NOTIFY, trigger.EVENT_NOTIFY1)\n", blockIdx++);  // Ext.Trigger Out SMU with EVENT_NOTIFY1

                        script += string.Format("trigger.model.setblock({0}, trigger.BLOCK_WAIT, trigger.EVENT_EXTERNAL)\n", blockIdx++);   // Ext.Trigger IN for starting measure with EVENT_EXTERNAL
                        script += string.Format("trigger.model.setblock({0}, trigger.BLOCK_DELAY_CONSTANT, {1})\n", blockIdx++, setting.TriggerInputDelay);
                        script += string.Format("trigger.model.setblock({0}, trigger.BLOCK_MEASURE, defbuffer1, 1)\n", blockIdx++);
                        script += string.Format("trigger.model.setblock({0}, trigger.BLOCK_BRANCH_COUNTER, {1}, 2)\n", blockIdx++, setting.TriggerCount);
                        break;
                    }
                default:
                    {
                        script = string.Empty; 
                        break;
                    }
            }

            return script;
        }

        private bool GetErrorMsg()
        {
            this._errMsg = string.Empty;
            
            string script = string.Empty;

            script += "errorCode, message, severity, errorNode = errorqueue.next()\n";

            script += "print(message)";

            this._conn.SendCommand(script);

            string msg = string.Empty;

            this._conn.WaitAndGetData(out msg);

            if (msg == "No error\n")
            {
                return false;
            }

            this._errMsg = msg;

            return true;
        }

        private string[] GetDevicePrintValueToArray(char splitSymbol)
        {
            string rawStrData = string.Empty;

            if (this._conn.WaitAndGetData(out rawStrData))
            {
                rawStrData = rawStrData.TrimEnd('\n').Replace(" ", "");

                string[] rawStrDataArray = rawStrData.Split(splitSymbol);

                return rawStrDataArray;
            }
            else
            {
                return null;
            }
        }

        #endregion

        public bool Init(string ipAddress, EDmmMeasureFunc defaultFunc)
        {
            string rtnStr = string.Empty;

            LANSettingData lanSetting = new LANSettingData();

            lanSetting.IPAddress = ipAddress;

            this._conn = new LANConnect(lanSetting);

            if (!this._conn.Open(out rtnStr))
            {
                return false;
            }

            this._conn.SendCommand("reset()");

            this._conn.SendCommand("print(localnode.model, localnode.serialno, localnode.version)");

            this._conn.WaitAndGetData(out rtnStr);

            if (rtnStr == string.Empty)
            {
                return false;
            }
            /////////////////////////////////////////////////////////////////////
            string[] devinfo = rtnStr.Trim().Split('\t');

            if (devinfo.Length != 3)
            {
                return false;
            }

            string model = devinfo[0];
            string sn = devinfo[1];
            string hwVer = devinfo[2];

            this.ConfigDMM(defaultFunc);

            return true;
        }

        public bool SetParameterToDMM(uint index, DmmSettingData setting)
        {
            if (setting == null)
            {
                return true;
            }

            return this.MeasuringScript(index, setting);
        }

        public bool Trigger(uint index)
        {
            if (this._conn == null)
            {
                return false;
            }

            this._conn.SendCommand("num_" + index.ToString() + ".run()");

            this._isTrigger = true;

            return true;
        }

        public bool WaitTriggerReady()
        {
            if (this._conn == null)
            {
                return false;
            }

            if (!this._isTrigger)
            {
                return true;
            }

            string status = string.Empty;
            
            do
            {
               this._conn.SendCommand("print(status.operation.event)");

                // this._conn.SendCommand("print(trigger.model.state())");  // 使用 trigger.model.state() 反而比較慢 也會造成時序不穩

                this._conn.WaitAndGetData(out status);
                System.Threading.Thread.Sleep(0);

            } while (!status.Contains("1024"));// //while (!status.Contains("WAITING"));

            return true;
        }

        public bool WaitTriggerIdle()
        {
            if (this._conn == null)
            {
                return false;
            }

            if (!this._isTrigger)
            {
                return true;
            }

            string status = string.Empty;

            do
            {
                //this._conn.SendCommand("print(status.operation.event)");

                 this._conn.SendCommand("print(trigger.model.state())");
                this._conn.WaitAndGetData(out status);
                System.Threading.Thread.Sleep(0);

            } while (!status.Contains("IDLE")); // while (!status.Contains("0")) ;//while (!status.Contains("WAITING"));

            return true;
        }

        public void AbortTrigger()
        {
            if (this._conn != null)
            {
                this._conn.SendCommand("abort");

                this._isTrigger = false;
            }
        }

        public double[] ReadDataFromBuffer(uint readingcnt = 1)
        {
            if (this._conn == null)
            {
                return null;
            }

            this._isTrigger = false;

            this._acquireData.Clear();

            double tempValue = 0.0d;

            if (readingcnt <= this._bufferSize)
            {
                this._conn.SendCommand("printbuffer(1, defbuffer1.n, defbuffer1)");

                string[] strBuffer = this.GetDevicePrintValueToArray(',');

                if (strBuffer == null)
                {
                    return null;
                }

                for (int i = 0; i < strBuffer.Length; i++)
                {
                    Double.TryParse(strBuffer[i], out tempValue);

                    this._acquireData.Add(tempValue);
                }
            }
            else
            {
                int remainder = 0;
                int loop = Math.DivRem((int)readingcnt, this._bufferSize, out remainder);
                int startIdx = 1;
                int endIdx = 1;

                for (int i = 0; i <= loop; i++)
                {
                    startIdx = i * this._bufferSize + 1;

                    if (i == loop)
                    {
                        endIdx = startIdx + remainder - 1;
                    }
                    else
                    {
                        endIdx = (i + 1) * this._bufferSize;
                    }

                    this._conn.SendCommand(string.Format("printbuffer({0}, {1}, defbuffer1)", startIdx, endIdx));

                    string[] strBuffer = this.GetDevicePrintValueToArray(',');

                    if (strBuffer == null)
                    {
                        return null;
                    }

                    for (int j = 0; j < strBuffer.Length; j++)
                    {
                        Double.TryParse(strBuffer[j], out tempValue);

                        this._acquireData.Add(tempValue);
                    }
                }
            }

            return this._acquireData.ToArray(); 
        }

        public void ClearStatus()
        {
            this._acquireData.Clear();
            this._isTrigger = false;
            
            if (this._conn != null)
            {
                string script = string.Empty;

                script += "eventlog.clear()\n";
                script += "status.clear()";

                this._conn.SendCommand(script);
            }
        }

        public void Close()
        {
            if (this._conn != null)
            {
                this._conn.Close();

                this._conn = null;
            }
        }
    }
}
