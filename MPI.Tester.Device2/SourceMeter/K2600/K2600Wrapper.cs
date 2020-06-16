using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Maths;

using MPI.Tester.Device.SourceMeter.KeithleySMU;

namespace MPI.Tester.Device.SourceMeter
{
    //PD->2W!!
	public partial class K2600Wrapper
	{
		#region >>> Private Proberty <<<

        private const bool WRITE_SCRIPT = false;
        private uint _id;
        private string _name;
        private string _devModel;
        private string _hwVersion;
        private string _swVersion;
        private string _serialNum;

		private IConnect _conn;
        private LANSettingData _lanSetting;

        private string _errMsg;

		private double[][] _voltRange;
		private double[][] _currRange;

		private ElecDevSetting _elecDevSetting;
        private int _srcSettling;
        private ESMUTriggerMode _srcTriggerMode;

        private List<double> _sweepList;
        private List<double> _sweepTimeSpan;

        private double[] _voltPulseRange;
        private double[] _currPulseRange;
        private double[] _pulseMaxTime;
        private double[] _pulseDuty;

        private List<string> _syncTrigSMU;
        private bool _isDualSMU;

        private bool _isQuary;
        private string[][] _acquireBuff;

        private Dictionary<int, List<ElectSettingData>> _parameterBuff = new Dictionary<int, List<ElectSettingData>>();  // key = smu, value elecSetting[]

        private K2600ScriptSetting _scriptParam;

        private ushort _dioOutputData;

		private bool _isProtectionModuleState;

        private const uint _pinESDDetection = 7; 

		#endregion

		#region >>> Constructor / Disposor <<<

		private K2600Wrapper()
		{
            this._name = string.Empty;

            this._srcSettling = (int)EK2600SrcSettling.FAST_ALL;

            this._sweepList = new List<double>();
            this._sweepTimeSpan = new List<double>();
            this._syncTrigSMU = new List<string>();

            this._isQuary = false;
            this._acquireBuff = new string[2][];

            this._scriptParam = new K2600ScriptSetting();

            this._dioOutputData = 0x0;

            this._isProtectionModuleState = false;
		}

        public K2600Wrapper(uint id, string name, string devModel, ElecDevSetting setting, string ipAddress, ESMUTriggerMode srcTriggerMode) : this()
		{
            this._id = id;

            this._name = name;

            this._devModel = devModel;

			this._elecDevSetting = setting;

            this._lanSetting = new LANSettingData();

            this._lanSetting.IPAddress = ipAddress;

            this._srcTriggerMode = srcTriggerMode;

            this._conn = new LANConnect(this._lanSetting);
		}

		#endregion

        #region >>> Private Method <<<

        #region >>> Keithley 2600 Config <<<

        private bool GetDeviceInfomation()
		{
            this._isDualSMU = false;
            
            this._conn.SendCommand("reset()");

			this._conn.SendCommand("print(localnode.model, localnode.serialno, localnode.revision)");

			string result = string.Empty;

            string[] info = this.GetDevicePrintValueToArray('\t');

			if (info == null || info.Length < 3)
			{
				return false;
			}

            string deviceModel = info[0];

            this._serialNum = info[1];

            this._swVersion = info[2];

			this._hwVersion = deviceModel;

            if (this._devModel != "K2600")
            {
                if (!deviceModel.Contains(this._devModel.Replace("K", "")))
                {
                    return false;
                }
            }

            if (deviceModel.Contains("2601") || deviceModel.Contains("2602"))
            {
                this._voltRange = K2600Spec.DC_V_RANGE_K2601;

                this._currRange = K2600Spec.DC_I_RANGE_K2601;
            }
            else if (deviceModel.Contains("2611") || deviceModel.Contains("2612"))
            {
                //----------------------------------------------------------------------------------------------                
                if (deviceModel.Contains("L"))
                {
                    // K2600-L Remove 100 nA Range 
                    this._currRange = new double[2][];
                    this._currRange[0] = new double[K2600Spec.DC_I_RANGE_K2611[0].Length - 1];
                    this._currRange[1] = new double[K2600Spec.DC_I_RANGE_K2611[1].Length - 1];

                    Array.Copy(K2600Spec.DC_I_RANGE_K2611[0], 1, this._currRange[0], 0, K2600Spec.DC_I_RANGE_K2611[0].Length - 1);
                    Array.Copy(K2600Spec.DC_I_RANGE_K2611[1], 1, this._currRange[1], 0, K2600Spec.DC_I_RANGE_K2611[1].Length - 1);
                }
                else
                {
                    this._currRange = K2600Spec.DC_I_RANGE_K2611;
                }
                //----------------------------------------------------------------------------------------------

                this._voltRange = K2600Spec.DC_V_RANGE_K2611;

                this._voltPulseRange = K2600Spec.PULSE_V_RANGE_K2611;

                this._currPulseRange = K2600Spec.PULSE_I_RANGE_K2611;

                this._pulseMaxTime = K2600Spec.MAX_PULSE_WIDTH_K2611;

                this._pulseDuty = K2600Spec.MAX_PULSE_DUTY_K2611;
            }
            else if (deviceModel.Contains("2635") || deviceModel.Contains("2636"))
            {
                this._voltRange = K2600Spec.DC_V_RANGE_K2635;

                this._currRange = K2600Spec.DC_I_RANGE_K2635;

                this._voltPulseRange = K2600Spec.PULSE_V_RANGE_K2635;

                this._currPulseRange = K2600Spec.PULSE_I_RANGE_K2635;

                this._pulseMaxTime = K2600Spec.MAX_PULSE_WIDTH_K2635;

                this._pulseDuty = K2600Spec.MAX_PULSE_DUTY_K2635;

            }
            else if (deviceModel.Contains("2651"))
            {
                this._voltRange = K2600Spec.DC_V_RANGE_K2651;

                this._currRange = K2600Spec.DC_I_RANGE_K2651;

                this._voltPulseRange = K2600Spec.PULSE_V_RANGE_K2651;

                this._currPulseRange = K2600Spec.PULSE_I_RANGE_K2651;

                this._pulseMaxTime = K2600Spec.MAX_PULSE_WIDTH_K2651;

                this._pulseDuty = K2600Spec.MAX_PULSE_DUTY_K2651;
            }
            else
			{
                return false;
			}

            if (deviceModel.Contains("2612") || deviceModel.Contains("2602") || deviceModel.Contains("2636"))
            {
                this._isDualSMU = true;
            }

			return true;
		}

		private bool SetConfig()
		{
            if (this._name == "SLAVE_PD")
            {
                // Slave Config: 支援 K2611B as Detector-CH / K2612B as dual Detector-CH 
                this.SetSlaveDetectorConfigScript();
            }
            else
            {
                // Master Config
                this.SetMasterConfigScript();
            }

            //----------------------------------------------------------
            // Config Script Setter
            this.SetScriptSetterConfig();

            //----------------------------------------------------------
            // Config Functions

            this._conn.SendCommand(K2600Script.ConfigFuncFIMV_A());
            this._conn.SendCommand(K2600Script.ConfigFuncFVMI_A());

            if (_isDualSMU)
            {
                this._conn.SendCommand(K2600Script.ConfigFuncFIMVDual());
                this._conn.SendCommand(K2600Script.ConfigFuncFVMIDual());
                this._conn.SendCommand(K2600Script.ConfigFuncFIMV_B());
                this._conn.SendCommand(K2600Script.ConfigFuncFVMI_B());

            }

            //----------------------------------------------------------
            // Check Error
			if (this.GetErrorMsg())
			{
				this.Close();

				return false;
			}

			return true;
		}

        private void SetScriptSetterConfig()
        {
            string cmd = string.Empty;

            cmd += K2600Script.MakeSetter_SMUA();

            //---------------------------------------------------------------------------------------------------------------------------

            if (this._isDualSMU)
            {
                cmd += K2600Script.MakeSetter_SMUB();
            }


            //if (this._srcTriggerMode == ESMUTriggerMode.Multiple)  // Roy, 這會和 MD 混淆 // Multi-Die -> ESMUTriggerMode.PMDT
            //{
            //    for (int ioPin = 1; ioPin < 7; ioPin++)
            //    {
            //        cmd += "digio.trigger[" + ioPin + "].clear()\n";

            //        cmd += "digio.writebit(" + ioPin + ", 1)\n";

            //        cmd += "digio.trigger[" + ioPin + "].pulsewidth = 0.1\n";

            //        cmd += "digio.trigger[" + ioPin + "].mode = digio.TRIG_FALLING\n";
            //    }
            //}

            this._conn.SendCommand(cmd);
        }

        private void SetMasterConfigScript()
        {
            string cmd = string.Empty;

            // turn the beeper off
            cmd += "beeper.enable = 0\n";

            cmd += "display.clear()\n";

            cmd += "display.setcursor(1, 3)\n";

            cmd += "display.settext(\"MPI CORPORATION\")\n";

            cmd += "display.setcursor(2, 4)\n";

            cmd += "display.settext(\"" + this._lanSetting.IPAddress + "\")\n";

            // Disable automatic display of errors - leave  error messages in queue and enable  Error Prompt.
            cmd += "localnode.showerrors = 0\n";

            cmd += "localnode.prompts = 0\n";

            //---------------------------------------------------------------------------------------------------------------------------
            // Autozero disabled: 0 = disabled, 1 = once, 2 = enabled
            cmd += "smua.measure.autozero = 0\n";
            
            // Mode: [0] 2-wire; [1] 4-wire; [3] calibration sense mode
            if (this._elecDevSetting.SrcSensingMode == ESrcSensingMode._2wire)
            {
                cmd += "smua.sense = " + ((int)EK2600SenseMode.LOCAL).ToString() + "\n";
            }
            else
            {
                cmd += "smua.sense = " + ((int)EK2600SenseMode.REMOTE).ToString() + "\n";
            }

            // Disables the filter
            cmd += "smua.measure.filter.enable = 0\n";

            cmd += "smua.measure.autorangev = 0\n";

            cmd += "smua.measure.autorangei = 0\n";

            cmd += "smua.source.autorangei = 0\n";

            cmd += "smua.source.autorangev = 0\n";
            
            cmd += "smua.source.delay = 0\n";

            cmd += "smua.measure.delay = 0\n";

            cmd += "smua.source.offmode	= 0\n";

            cmd += "format.data = 1\n";

            cmd += "format.asciiprecision = 6\n";

            cmd += "smua.source.levelv = 0\n";

            cmd += "smua.source.leveli = 0\n";

            cmd += "smua.source.rangei = 0.01\n";

            cmd += "smua.source.rangev = 0.01\n";

            cmd += "smua.source.limitv = 10\n";

            cmd += "smua.source.limiti = 0.01\n";

            //20171215 David

            cmd += "smua.measure.nplc = 0.01\n";

            cmd += "smua.source.settling = " + this._srcSettling.ToString() + "\n";

            //20190124 David
            if (this._hwVersion.Contains("2634") || this._hwVersion.Contains("2635") || this._hwVersion.Contains("2636"))
            {
                cmd += "smua.measure.analogfilter= 0\n";
            }

            
            if (this._isDualSMU)
            {
                // Autozero disabled: 0 = disabled, 1 = once, 2 = enabled
                cmd += "smub.measure.autozero = 0\n";
              
                // Mode: [0] 2-wire; [1] 4-wire; [3] calibration sense mode
                if (this._elecDevSetting.DetectorMsrtDevice == EPDSensingMode.SrcMeter_SMUB)
                {
                    cmd += "smub.sense = " + ((int)EK2600SenseMode.LOCAL).ToString() + "\n";  // PD量測 : 2-wire
                }
                else
                {

                    if (this._elecDevSetting.SrcSensingMode == ESrcSensingMode._2wire)
                    {
                        cmd += "smub.sense = " + ((int)EK2600SenseMode.LOCAL).ToString() + "\n";
                    }
                    else
                    {
                        cmd += "smub.sense = " + ((int)EK2600SenseMode.REMOTE).ToString() + "\n";
                    }
                }

                // Disables the filter
                cmd += "smub.measure.filter.enable = 0\n";

                cmd += "smub.measure.autorangev = 0\n";

                cmd += "smub.measure.autorangei = 0\n";

                cmd += "smub.source.autorangei = 0\n";

                cmd += "smub.source.autorangev = 0\n";

                cmd += "smub.source.delay = 0\n";

                cmd += "smub.measure.delay = 0\n";

                cmd += "smub.source.offmode	= 0\n";

                cmd += "smub.source.levelv = 0\n";

                cmd += "smub.source.leveli = 0\n";

                cmd += "smub.source.rangei = 0.01\n";

                cmd += "smub.source.rangev = 0.01\n";

                cmd += "smub.source.limitv = 10\n";

                cmd += "smub.source.limiti = 0.01\n";

                cmd += "smub.measure.nplc =0.01\n";

                cmd += "smub.source.settling = " + this._srcSettling.ToString() + "\n";

                if (this._hwVersion.Contains("2634") || this._hwVersion.Contains("2635") || this._hwVersion.Contains("2636"))
                {
                    cmd += "smub.measure.analogfilter= 0\n";
                }
            }



            //---------------------------------------------------------------------------------------------------------------------------
            //if (!this._elecDevSetting.IsCtrlSwitchSys)
            //{
            //    #region >>> Normal I/O Config <<<
            //    // Roy H/W Trig
            //    //double trigPw = 10e-6;
            //    double trigPw = 0.0001;
            //    // cmd += "digio.writebit(" + K2600Const.IO_SMU_TRIG_IN.ToString() + ", 0)\n";

            //    cmd += "digio.trigger[" + K2600Const.IO_SMU_TRIG_IN + "].clear()\n";

            //    cmd += "digio.trigger[" + K2600Const.IO_SMU_TRIG_IN.ToString() + "].mode = digio.TRIG_FALLING\n";

            //    cmd += "digio.trigger[" + K2600Const.IO_SMU_TRIG_IN.ToString() + "].pulsewidth = " + trigPw.ToString() + "\n";

            //    //  cmd += "digio.writebit(" + K2600Const.IO_SMU_TRIG_OUT.ToString() + ", 0)\n";

            //    cmd += "digio.trigger[" + K2600Const.IO_PM_RELAY1.ToString() + "].clear()\n";

            //    cmd += "digio.trigger[" + K2600Const.IO_PM_RELAY1.ToString() + "].mode = digio.TRIG_FALLING\n";

            //    cmd += "digio.trigger[" + K2600Const.IO_PM_RELAY1.ToString() + "].pulsewidth = " + trigPw.ToString() + "\n";

            //    //cmd += "digio.writebit(" + K2600Const.IO_SMU_ABORT.ToString() + ", 0)\n";

            //    //cmd += "digio.trigger[" + K2600Const.IO_SMU_ABORT_IN.ToString() + "].clear()\n";

            //    //cmd += "digio.trigger[" + K2600Const.IO_SMU_ABORT_IN.ToString() + "].mode = digio.TRIG_FALLING\n";

            //    //cmd += "digio.trigger[" + K2600Const.IO_SMU_ABORT_IN.ToString() + "].pulsewidth = 0.0005\n";

            //    cmd += "digio.trigger[" + K2600Const.IO_PM_RELAY2.ToString() + "].clear()\n";

            //    cmd += "digio.trigger[" + K2600Const.IO_PM_RELAY2.ToString() + "].mode = digio.TRIG_FALLING\n";

            //    cmd += "digio.trigger[" + K2600Const.IO_PM_RELAY2.ToString() + "].pulsewidth = 0.0005\n";

            //    //---------------------------------------------------------------------------------------------------------------------------
            //    //Active hight

            //    cmd += "digio.writebit(" + K2600Const.IO_FRAME_GROUND.ToString() + ", 0)\n";

            //    cmd += "digio.trigger[" + K2600Const.IO_FRAME_GROUND.ToString() + "].pulsewidth = " + K2600Const.IO_DEFUALT_PULSE_WIDTH.ToString() + "\n";

            //    cmd += "digio.trigger[" + K2600Const.IO_FRAME_GROUND.ToString() + "].mode = digio.TRIG_RISING\n";

            //    cmd += "digio.writebit(" + K2600Const.IO_DAQ_ENABLE.ToString() + ", 0)\n";

            //    cmd += "digio.trigger[" + K2600Const.IO_DAQ_ENABLE.ToString() + "].pulsewidth = " + K2600Const.IO_DEFUALT_PULSE_WIDTH.ToString() + "\n";

            //    cmd += "digio.trigger[" + K2600Const.IO_DAQ_ENABLE.ToString() + "].mode = digio.TRIG_RISING\n";

            //    cmd += "digio.writebit(" + K2600Const.IO_DAQ_ENABLE.ToString() + ", 0)\n";

            //    cmd += "digio.trigger[" + K2600Const.IO_DAQ_ENABLE.ToString() + "].pulsewidth = " + K2600Const.IO_DEFUALT_PULSE_WIDTH.ToString() + "\n";

            //    cmd += "digio.trigger[" + K2600Const.IO_DAQ_ENABLE.ToString() + "].mode = digio.TRIG_RISING\n";

            //    cmd += "digio.writebit(" + K2600Const.IO_RTH_EANBLE.ToString() + ", 0)\n";

            //    cmd += "digio.trigger[" + K2600Const.IO_RTH_EANBLE.ToString() + "].pulsewidth = " + K2600Const.IO_DEFUALT_PULSE_WIDTH.ToString() + "\n";

            //    cmd += "digio.trigger[" + K2600Const.IO_RTH_EANBLE.ToString() + "].mode = digio.TRIG_RISING\n";

            //    cmd += "digio.writebit(" + K2600Const.IO_DAQ_TRIG_OUT.ToString() + ", 0)\n";

            //    cmd += "digio.trigger[" + K2600Const.IO_DAQ_TRIG_OUT.ToString() + "].pulsewidth = " + K2600Const.IO_DEFUALT_PULSE_WIDTH.ToString() + "\n";

            //    cmd += "digio.trigger[" + K2600Const.IO_DAQ_TRIG_OUT.ToString() + "].mode = digio.TRIG_RISING\n";

            //    cmd += "digio.writebit(" + K2600Const.IO_POLAR_SW.ToString() + ", 0)\n";

            //    cmd += "digio.trigger[" + K2600Const.IO_POLAR_SW.ToString() + "].pulsewidth = 0.0001\n";

            //    cmd += "digio.trigger[" + K2600Const.IO_POLAR_SW.ToString() + "].mode = digio.TRIG_RISING\n";

            //    //---------------------------------------------------------------------------------------------------------------------------

            //    #endregion
            //}
            //else
            //{
            //cmd += "digio.writeport(0)\n";
            //}

            cmd += SetDefaultIOScript();

            this._conn.SendCommand(cmd);
        }

        private void SetSlaveDetectorConfigScript()
        {
            string cmd = string.Empty;

            // turn the beeper off
            cmd += "beeper.enable = 0\n";

            cmd += "display.clear()\n";

            cmd += "display.setcursor(1, 3)\n";

            cmd += "display.settext(\"SLAVE - LISTENING\")\n";

            cmd += "display.setcursor(2, 4)\n";

            cmd += "display.settext(\"" + this._lanSetting.IPAddress + "\")\n";

            // Disable automatic display of errors - leave  error messages in queue and enable  Error Prompt.
            cmd += "localnode.showerrors = 0\n";

            cmd += "localnode.prompts = 0\n";

            //---------------------------------------------------------------------------------------------------------------------------
            // Autozero disabled: 0 = disabled, 1 = once, 2 = enabled
            cmd += "smua.measure.autozero = 0\n";

            // Mode: [0] 2-wire; [1] 4-wire; [3] calibration sense mode
            if (this._elecDevSetting.SrcSensingMode == ESrcSensingMode._2wire)
            {
                cmd += "smua.sense = " + ((int)EK2600SenseMode.LOCAL).ToString() + "\n";
            }
            else
            {
                cmd += "smua.sense = " + ((int)EK2600SenseMode.REMOTE).ToString() + "\n";
            }

            // Disables the filter
            cmd += "smua.measure.filter.enable = 0\n";

            cmd += "smua.source.settling = " + this._srcSettling.ToString() + "\n";

            cmd += "smua.measure.autorangev = 0\n";

            cmd += "smua.source.autorangei = 0\n";

            cmd += "smua.source.autorangev = 0\n";

            cmd += "smua.measure.autorangei = 0\n";

            cmd += "smua.source.delay = 0\n";

            cmd += "smua.measure.delay = 0\n";

            cmd += "smua.source.offmode	= 0\n";

            cmd += "format.data = 1\n";

            cmd += "format.asciiprecision = 6\n";

            cmd += "smua.source.levelv = 0\n";

            cmd += "smua.source.leveli = 0\n";

            cmd += "smua.source.rangei = 0.01\n";

            cmd += "smua.source.rangev = 2\n";

            cmd += "smua.source.limitv = 10\n";

            //---------------------------------------------------------------------------------------------------------------------------
            // Roy H/W Trig (Active High)
            //double trigPw = 10e-6;
            //double trigPw = 0.0001;
            // cmd += "digio.writebit(" + K2600Const.IO_SMU_TRIG_IN.ToString() + ", 0)\n";

            //cmd += "digio.trigger[" + K2600Const.IO_SMU_TRIG_IN.ToString() + "].clear()\n";

            //cmd += "digio.trigger[" + K2600Const.IO_SMU_TRIG_IN.ToString() + "].mode = digio.TRIG_FALLING\n";

            //cmd += "digio.trigger[" + K2600Const.IO_SMU_TRIG_IN.ToString() + "].pulsewidth = " + trigPw.ToString() + "\n";

            ////  cmd += "digio.writebit(" + K2600Const.IO_SMU_TRIG_OUT.ToString() + ", 0)\n";

            //cmd += "digio.trigger[" + K2600Const.IO_PM_RELAY1.ToString() + "].clear()\n";

            //cmd += "digio.trigger[" + K2600Const.IO_PM_RELAY1.ToString() + "].mode = digio.TRIG_FALLING\n";

            //cmd += "digio.trigger[" + K2600Const.IO_PM_RELAY1.ToString() + "].pulsewidth = " + trigPw.ToString() + "\n";

            ////  cmd += "digio.writebit(" + K2600Const.IO_SMU_ABORT_IN.ToString() + ", 0)\n";

            ////cmd += "digio.trigger[" + K2600Const.IO_SMU_ABORT_IN.ToString() + "].clear()\n";

            ////cmd += "digio.trigger[" + K2600Const.IO_SMU_ABORT_IN.ToString() + "].mode = digio.TRIG_FALLING\n";

            ////cmd += "digio.trigger[" + K2600Const.IO_SMU_ABORT_IN.ToString() + "].pulsewidth = 0.0005\n";

            //cmd += "digio.trigger[" + K2600Const.IO_PM_RELAY2.ToString() + "].clear()\n";

            //cmd += "digio.trigger[" + K2600Const.IO_PM_RELAY2.ToString() + "].mode = digio.TRIG_FALLING\n";

            //cmd += "digio.trigger[" + K2600Const.IO_PM_RELAY2.ToString() + "].pulsewidth = 0.0005\n";

            double trigPw = 0.0001;//由於Slave理論上不會進行IO控制，因此直接寫死

            cmd += SetDefaultIOScript((int)K2600Const.IO_SMU_TRIG_IN, EIOTrig_Mode.TRIG_FALLING, EIOState.NONE, trigPw);

            cmd += SetDefaultIOScript((int)K2600Const.IO_PM_RELAY1, EIOTrig_Mode.TRIG_FALLING, EIOState.NONE, trigPw);

            cmd += SetDefaultIOScript((int)K2600Const.IO_PM_RELAY2, EIOTrig_Mode.TRIG_FALLING, EIOState.NONE, 0.0005);


            //---------------------------------------------------------------------------------------------------------------------------

            this._conn.SendCommand(cmd);
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
                this._errMsg = string.Empty;
                return false;
            }

            this._errMsg = msg;

            Console.WriteLine("[K2600Wrapper], GetErrorMsg()," + msg);

            return true;
        }

        private bool SetSweepListScript(uint index, ElectSettingData item)
        {
            this._sweepList.Clear();

            this._sweepTimeSpan.Clear();

            string script = string.Empty;

            double start = item.SweepStart;

            double stop = item.SweepStop;

            double step = 0;

            uint points = 0;

            string listName = string.Format("List_{0}", index);

            script += "loadandrunscript makeSweepList_" + index.ToString() + "\n";

            switch (item.MsrtType)
            {
                case EMsrtType.FIMVSWEEP:
                case EMsrtType.FVMISWEEP:
                    {
                        #region >>> Sweep Create List Script <<<


                        switch (item.SweepMode)
                        {

                            case ESweepMode.Linear:
                                {
                                    script += CreateLinearList(listName, item.SweepStart, item.SweepStop, (int)item.SweepRiseCount);

                                    break;
                                }
                            case ESweepMode.Log:
                                {
                                    script += CreateLog10List(listName, item.SweepStart, item.SweepStop, (int)item.SweepRiseCount);
                                    break;
                                }
                            default:
                                return false;
                        }
                        

                        #endregion

                        break;
                    }
                case EMsrtType.PIV:
                    {
                        #region >>> PIV Create List Script <<<

                        //points = (uint)(Math.Ceiling((item.SweepStop - item.SweepStart) / item.SweepStep)) + 1;

                        points = (uint)((item.SweepStop - item.SweepStart) / item.SweepStep) + 1;

                        step = item.SweepStep;

                        // Set value into the array
                        script += listName + " = {}\n"
                                + "for i = 1," + points.ToString() + " do\n"
                                + listName + "[i] = " + start.ToString() + " + " + step.ToString() + " * (i - 1)\n"
                                + "if " + listName + "[i] > " + stop.ToString() + " then " + listName + "[i] =" + stop.ToString() + " end\n"
                                + "end\n";

                        #endregion

                        break;
                    }
                case EMsrtType.FVMISCAN:
                    {
                        #region
                        points = item.SweepContCount;

                        //step = (stop - start) / (points - 1);

                        // Set value into the array
                        script += listName + " = {}\n"
                                + "for i = 1," + points.ToString() + " do\n"
                                + listName + "[i] = " + item.ForceValue.ToString() + "\n"
                                + "end\n";
                        #endregion
                    }
                    break;
                default:
                    return false;
            }

            // print Sweep List 
            script += "print(table.concat(" + listName + ", \",\"))\n"
                    + "endscript";

            this._conn.SendCommand(script);

            //---------------------------------------------------------------------------------------------------
            // Get F/W Created Sweep List
            string[] scriptCreateList = this.GetDevicePrintValueToArray(',');

            double tempValue = 0.0d;

            for (int i = 0; i < scriptCreateList.Length; i++)
            {
                Double.TryParse(scriptCreateList[i], out tempValue);

                this._sweepList.Add(tempValue);

                this._sweepTimeSpan.Add(item.ForceTime * i *1000); // s -> ms
            }

            return !this.GetErrorMsg();
        }

        private string CreateLog10List(string listName, double start, double stop, int points)
        {
            string script = "";

            if (start == 0.0d)
            {
                start = 1e-12;
            }

            if (stop == 0.0d)
            {
                stop = 1e-12;
            }

            int polar = 1;

            if (start < 0 || stop < 0)
            {
                polar = -1;
            }

            start = Math.Abs(start);

            stop = Math.Abs(stop);

            double logStepSize = (Math.Log10(stop) - Math.Log10(start)) / (points - 1);

            script += "local start =" + start.ToString() + "\n"
                    + "local polar =" + polar.ToString() + "\n"
                    + "local logStepSize =" + logStepSize.ToString() + "\n";

            // Set value into the array
            script += listName + " = {}\n"
                    + "for i = 1," + points.ToString() + " do\n"
                    + "logStep = logStepSize * (i - 1)\n"
                    + listName + "[i] = math.pow(10, logStep) * start * polar\n"
                    + "end\n";

            return script;
        }

        private string CreateLinearList(string listName, double start, double stop, int points)
        {
            string script = "";

            double step = (stop - start) / (points - 1);
            if (points == 1)
            {
                step = 0;

                script += listName + " = {}\n"
                    + "for i = 1,1 do\n"
                    + listName + "[i] = " + stop.ToString() + "\n"
                    + "end\n";
            }

            // Set value into the array
            script += listName + " = {}\n"
                    + "for i = 1," + points.ToString() + " do\n"
                    + listName + "[i] = " + start.ToString() + " + " + step.ToString() + " * (i - 1)\n"
                    + "end\n";
            return script;
        }

        private bool SetCustomerSweepScript(uint index, ElectSettingData item)
        {
            this._sweepList.Clear();

            this._sweepTimeSpan.Clear();

            switch (item.MsrtType)
            {
                case EMsrtType.FIMVSWEEP:
                case EMsrtType.FVMISWEEP:
                    {
                        #region >>> Sweep Create Sweep List Script <<<

                        double usedTimeInms = 0;

                        if (item.ElecTerminalSetting != null && 0 < item.ElecTerminalSetting.Length)
                        {
                            for (int i = 0; i < item.ElecTerminalSetting.Length; ++i)
                            {                                
                                ElecTerminalSetting ets = item.ElecTerminalSetting[i];
                                if (ets == null )
                                {
                                    return false;
                                }

                                if (ets.SweepRiseCount < 1)
                                {
                                    return true;
                                }
                                string script = "";
                                double start = ets.SweepStart;
                                double stop = ets.SweepStop;

                                string listID = index.ToString() + "_" + i.ToString();
                                string listName = string.Format("List_" + listID);

                                script += "loadandrunscript makeSweepList_" + listID + "\n";

                                switch (ets.SweepMode)
                                {
                                    case ESweepMode.Linear:
                                        {
                                            script += CreateLinearList(listName, ets.SweepStart, ets.SweepStop, (int)ets.SweepRiseCount);
                                            break;
                                        }
                                    case ESweepMode.Log:
                                        {
                                            script += CreateLog10List(listName, ets.SweepStart, ets.SweepStop, (int)ets.SweepRiseCount);
                                         
                                            break;
                                        }
                                    default:
                                        return false;
                                }

                                script += "print(table.concat(" + listName + ", \",\"))\n" + "endscript";
                                this._conn.SendCommand(script);
                                //---------------------------------------------------------------------------------------------------
                                // Get F/W Created Sweep List
                                string[] scriptCreateList = this.GetDevicePrintValueToArray(',');

                                double tempValue = 0.0d;

                                for (int j = 0; j < scriptCreateList.Length; j++)
                                {
                                    Double.TryParse(scriptCreateList[j], out tempValue);

                                    this._sweepList.Add(tempValue);

                                    this._sweepTimeSpan.Add(((ets.ForceTime + ets.SweepTurnOffTime) * j + usedTimeInms) * 1000.0d); // s -> ms
                                }

                                usedTimeInms += ((double)ets.SweepRiseCount * (ets.SweepTurnOffTime + ets.ForceTime));

                            }
                        }
                        #endregion
                    }
                    break;

                default:
                    return false;
            }

            return !this.GetErrorMsg();
        }


        private string[] GetDevicePrintValueToArray(char splitSymbol,int delayInms=0)
        {
            string rawStrData = string.Empty;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            do
            {
                if (this._conn.WaitAndGetData(out rawStrData))
                {
                    rawStrData = rawStrData.TrimEnd('\n');

                    rawStrData = rawStrData.Replace(" ", "");

                    string[] rawStrDataArray = rawStrData.Split(splitSymbol);

                    return rawStrDataArray;
                }

            } while (delayInms < stopwatch.ElapsedMilliseconds);
            return null;
        }

		#endregion

        private bool CheckDCModeElectSetting(ElectSettingData item)
        {
            bool rtn = true;

            rtn &= this.FindSrcAndMsrtRange(item);

            rtn &= this.CheckProtectionModuleSpec(item);

            return rtn;
        }

        private bool CheckPulseModeElectSetting(ElectSettingData item)
        {
            int region = -1;

            if (this.FindSrcAndMsrtRange(item))
            {
                //DC範圍下的Pulse
                item.Duty = 1.0d;
                return true;
            }
            else
            {
                //Pulse Mode範圍下的Pulse
                if (!this.FindSrcAndMsrtPulseRange(item, out region))
                {
                    return false;
                }
                     
                item.Duty = region < 0 ? 1.0d : this._pulseDuty[region];
                     
                return true;
            }

            //需確認Protection Module 能否承受 Pulse Mode
            //rtn &= this.CheckProtectionModuleSpec(item);
        }

        private bool FindSrcAndMsrtRange(ElectSettingData item)
        {
            bool isCurrDrive = true;

            double setCurrRange = 0.0d;

            double setVoltRange = 0.0d;

            switch (item.MsrtType)
            {
                case EMsrtType.FI:
                case EMsrtType.FIMV:
                case EMsrtType.THY:
                case EMsrtType.POLAR:
                case EMsrtType.R:
                case EMsrtType.RTH:
                case EMsrtType.VLR:
                case EMsrtType.FIMVLOP:
                    {
                        setCurrRange = GetCurrDCRange(Math.Abs(item.ForceValue));

                        setVoltRange = GetVoltDCRange(Math.Abs(item.MsrtRange));

                        isCurrDrive = true;

                        break;
                    }
                //-----------------------------------------------------------------------------//
                case EMsrtType.LIV:
                    {
                        double sweepMax = Math.Max(Math.Abs(item.SweepStart), Math.Abs(item.SweepStop));

                        setCurrRange = GetCurrDCRange(Math.Abs(sweepMax));

                        setVoltRange = GetVoltDCRange(Math.Abs(item.MsrtRange));

                        isCurrDrive = true;
                        
                        break;
                    }
                //-----------------------------------------------------------------------------//
                case EMsrtType.FVMI:
                case EMsrtType.FV:
                case EMsrtType.FVMILOP:
                case EMsrtType.FVMISCAN:
				case EMsrtType.LCR:
                    {

                        setVoltRange = GetVoltDCRange(Math.Abs(item.ForceValue));

                        setCurrRange = GetCurrDCRange(Math.Abs(item.MsrtRange));

                        isCurrDrive = false;

                        break;
                    }
                //-----------------------------------------------------------------------------//
                case EMsrtType.LVI:
                    {
                        double sweepMax = Math.Max(Math.Abs(item.SweepStart), Math.Abs(item.SweepStop));

                        setVoltRange = GetVoltDCRange(Math.Abs(sweepMax));

                        setCurrRange = GetCurrDCRange(Math.Abs(item.MsrtRange));

                        isCurrDrive = false;

                        break;
                    }
                //-----------------------------------------------------------------------------//
                case EMsrtType.FIMVSWEEP:
                case EMsrtType.PIV:
                    {
                        double sweepMax = Math.Max(Math.Abs(item.SweepStart), Math.Abs(item.SweepStop));

                        setCurrRange = GetCurrDCRange(Math.Abs(sweepMax));

                        setVoltRange = GetVoltDCRange(Math.Abs(item.MsrtRange));


                        isCurrDrive = true;

                        break;
                    }
                //-----------------------------------------------------------------------------//
                case EMsrtType.FVMISWEEP:
                    {
                        double sweepMax = Math.Max(Math.Abs(item.SweepStart), Math.Abs(item.SweepStop));

                        setVoltRange = GetVoltDCRange(Math.Abs(sweepMax));

                        setCurrRange = GetCurrDCRange(Math.Abs(item.MsrtRange));

                        //setVoltRange = Math.Max(Math.Abs(item.SweepStart), Math.Abs(item.SweepStop));

                        //setCurrRange = Math.Abs(item.MsrtRange);

                        isCurrDrive = false;

                        break;
                    }
                //-----------------------------------------------------------------------------//
                case EMsrtType.IO:
                    {
                        setVoltRange = 2;

                        setCurrRange = 0.001;

                        isCurrDrive = false;
                    }
                    break;
                //-----------------------------------------------------------------------------//
                
                case EMsrtType.LCRSWEEP:
                    break;
                default:
                    return false;
            }

            //-----------------------------------------------------------------------------------------
            int index;
  
            // Current Range Check
            for (index = this._currRange.Length - 1; index >= 0; index--)
            {
                if (setCurrRange <= this._currRange[index][this._currRange[index].Length - 1]) // Force Value <= Device Max Current
                {
                    if (isCurrDrive)
                    {
                        item.ForceRange = setCurrRange;
                    }
                    else
                    {
                        item.MsrtRange = setCurrRange;
                    }
                    break;
                }
            }

            // Voltage Range Check
            if (index >= 0)
            {
                if (setVoltRange <= this._voltRange[index][this._voltRange[index].Length - 1])
                {
                    if (isCurrDrive)
                    {
                        item.MsrtRange = setVoltRange;
                    }
                    else
                    {
                        item.ForceRange = setVoltRange;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        private double GetCurrDCRange(double srcCurrRange)
        {
            double currRange = srcCurrRange;
            for (int i = 0; i < this._currRange[0].Length; ++i)
            {
                if (srcCurrRange <= this._currRange[0][i])
                {
                    currRange = this._currRange[0][i];
                    break;
                }
            }
            return currRange;
        }

        private double GetVoltDCRange(double srcVoltRange)
        {
            double voltRange = srcVoltRange;
            bool found = false;
            for (int j = 0;!found && j < this._voltRange.Length; ++j)
            {
                for (int i = 0; i < this._voltRange[j].Length; ++i)
                {
                    if (srcVoltRange <= this._voltRange[j][i])
                    {
                        voltRange = this._voltRange[j][i];
                        found = true;
                        break;
                    }
                }
            }
            return voltRange;
        }

        private bool FindSrcAndMsrtPulseRange(ElectSettingData item, out int region)
        {
            region = -1;

            switch (item.MsrtType)
            {
                case EMsrtType.FIMV:
                    {
                        for (int i = 0; i < this._currPulseRange.Length; i++)
                        {
                            if (Math.Abs(item.ForceValue) > this._currPulseRange[i] || Math.Abs(item.MsrtRange) > this._voltPulseRange[i])
                            {
                                continue;
                            }

                            if (item.ForceTime > this._pulseMaxTime[i])
                            {
                                continue;
                            }

                            if (item.IsAutoForceRange)
                            {
                                item.ForceRange = Math.Abs(item.ForceValue);
                            }

                            item.MsrtProtection = Math.Abs(item.MsrtRange);

                            region = i;

                            return true;
                        }

                        return false;
                    }
                case EMsrtType.FVMI:
                    {
                        for (int i = 0; i < this._voltPulseRange.Length; i++)
                        {
                            if (Math.Abs(item.ForceValue) > this._voltPulseRange[i] || Math.Abs(item.MsrtRange) > this._currPulseRange[i])
                            {
                                continue;
                            }

                            if (item.ForceTime > this._pulseMaxTime[i])
                            {
                                continue;
                            }

                            if (item.IsAutoForceRange)
                            {
                                item.ForceRange = Math.Abs(item.ForceValue);
                            }

                            item.MsrtProtection = Math.Abs(item.MsrtRange);

                            region = i;

                            return true;
                        }

                        return false;
                    }
                default:
                    {
                        return false;
                    }
            }
        }

		private void ExportScriptToTxt(string script, string fileName)
		{
			string path = System.IO.Path.Combine(@"C:\", fileName + "_KeiScript.txt");

			using (System.IO.StreamWriter sWriter = new System.IO.StreamWriter(path))
			{
				int rowIdx = 0;

				StringBuilder strBuilder = new StringBuilder();

				string[] strArry = script.Split('\n');

				foreach (string str in strArry)
				{
					strBuilder.Append(str).AppendLine();

					rowIdx++;
				}

				sWriter.Write(strBuilder.ToString());
			}
		}

        private bool CheckProtectionModuleSpec(ElectSettingData item)
        {
            // Time unit: ms
            if (!this._elecDevSetting.IsEnableProtectionModule)
            {
                return true;
            }

            if (item.KeyName.Contains("IF"))
            {
                if (item.ForceValue > 1.0d)
                {
                    return false;
                }

                if (item.ForceValue > 0.50d && item.ForceTime > 5.0d) // 0.0050d
                {
                    return false;
                }
            }

            if (item.KeyName.Contains("IFWLA"))
            {
                if (item.ForceValue > 1.0d)
                {
                    return false;
                }

                if (item.ForceValue > 0.50d && (item.ForceTime + item.ForceTimeExt) > 30.0d)
                {
                    return false;
                }
            }

            return true;
        }

        private void SetScriptParamter(uint index, ElectSettingData itemA, ElectSettingData itemB)
        {
            this._scriptParam.Reset();
            
            this._scriptParam.Index = index;
            this._scriptParam.KeyName = itemA.KeyName;
            if (index == 0)
                this._scriptParam.ResetState();
                   
            if (this._elecDevSetting.SrcTurnOffType == ESrcTurnOffType.TestEnd ||
                this._elecDevSetting.SrcTurnOffType == ESrcTurnOffType.EOT)
            {
                this._scriptParam.IsTurnOffToZeroVolt = false;
            }
            else
            {
                this._scriptParam.IsTurnOffToZeroVolt = true;
            }

            //this._scriptParam.IsTurnOffToZeroVolt = true;//20190118 David

            this._scriptParam.IsTurnOffToDefaultRange = this._elecDevSetting.TurnOffRangeIBackToDefault;
            this._scriptParam.IsSpFwCalcResult = this._elecDevSetting.IsDevicePeakFiltering;
            this._scriptParam.IsSpReverseCurrentRange = this._elecDevSetting.IsSettingReverseCurrentRange;
            this._scriptParam.SpReverseCurrentApplyRange = this._elecDevSetting.ReverseCurrentApplyRange;
            this._scriptParam.IsEnableBriefScript = this._elecDevSetting.IsEnableBriefScript;

            // SMU-A
            this.TransElecSettingIntoScriptParameter(itemA, ref this._scriptParam.SMUA);

            //---------------------------------------------------------------------------------------------------------------------------------------
            // SMU-B
            this._scriptParam.SMUB.IsEnableSmu = false;

            if (itemB != null)
            {
                this.TransElecSettingIntoScriptParameter(itemB, ref this._scriptParam.SMUB);

                this._scriptParam.SMUB.IsEnableSmu = this._isDualSMU;
            }
        }

        private void TransElecSettingIntoScriptParameter(ElectSettingData item, ref K2600SmuSetting smuSetting)
        {
            if (item == null)
            {
                smuSetting.IsEnableSmu = false;
                return;
            }

            smuSetting.IsEnableSmu = true;

            switch (item.ConatctCheckSpeed)
            {
                case EContactCheckSpeed.FAST:
                    smuSetting.ContactCheckSpeed = EK2600ContactCheckSpeed.FAST;
                    break;
                case EContactCheckSpeed.MEDIUM:
                    smuSetting.ContactCheckSpeed = EK2600ContactCheckSpeed.MEDIUM;
                    break;
                case EContactCheckSpeed.SLOW:
                    smuSetting.ContactCheckSpeed = EK2600ContactCheckSpeed.SLOW;
                    break;
            }

            smuSetting.WaitTime = Math.Round((item.ForceDelayTime / 1000.0d), 6, MidpointRounding.AwayFromZero);

            //-----------------------------------------------------------------------------------------
            // Paul 2014.03.14 取絕對值，IF>120mA IZ=10uA，會出現 Error Code 5007 
            // Force Range and Compliance Setting
            //-----------------------------------------------------------------------------------------
            // DC
            if (item.MsrtType == EMsrtType.FIMV || item.MsrtType == EMsrtType.POLAR || item.MsrtType == EMsrtType.LIV || item.MsrtType == EMsrtType.FI ||
                item.MsrtType == EMsrtType.FIMVLOP || item.MsrtType == EMsrtType.PIV || item.MsrtType == EMsrtType.THY || item.MsrtType == EMsrtType.FIMVSWEEP)
            {
                smuSetting.SrcMode = EK2600SrcMode.I_Source;
                smuSetting.MsrtBoundryLimit = this._voltRange[0][this._voltRange[0].Length - 1];
                if (item.MsrtType == EMsrtType.FIMV && item.KeyName.StartsWith("IZ") && 
                    Math.Abs(item.ForceValue ) < 0.1)//IZ
                {
                    smuSetting.MsrtBoundryLimit = this._voltRange[1][this._voltRange[1].Length - 1];
                }
            }
            else
            {
                smuSetting.SrcMode = EK2600SrcMode.V_Source;
                smuSetting.MsrtBoundryLimit = this._currRange[1][this._currRange[1].Length - 1];
            }

            smuSetting.SrcRange = Math.Abs(item.ForceRange);
            smuSetting.SrcLevel = item.ForceValue;

            smuSetting.srcTime = Math.Round((item.ForceTime / 1000.0d), 6, MidpointRounding.AwayFromZero);

            if (item.MsrtType != EMsrtType.FI && item.MsrtType != EMsrtType.FV && item.MsrtType != EMsrtType.RTH)
            {
                smuSetting.IsEnableMsrt = true;
            }
            else
            {
                smuSetting.IsEnableMsrt = false;
            }

            smuSetting.MsrtRange = Math.Abs(item.MsrtRange);
            smuSetting.MsrtClamp = Math.Abs(item.MsrtProtection);
            smuSetting.MsrtNPLC = item.MsrtNPLC < 0.001d ? 0.01d : item.MsrtNPLC;
            smuSetting.TurnOffTime = Math.Round((item.TurnOffTime / 1000.0d), 6, MidpointRounding.AwayFromZero);

            smuSetting.IsEnableMsrtSrcLevel = item.IsEnableMsrtForceValue;

            smuSetting.IsAutoMsrtRange = item.IsAutoMsrtRange;
            smuSetting.IsAutoTurnOff = item.IsAutoTurnOff;

            // Sweep
            smuSetting.SweepStartHoldTime = Math.Round((item.SweepHoldTime / 1000.0d), 6, MidpointRounding.AwayFromZero);

            smuSetting.SweepPoints = (uint)this._sweepList.Count;

            if (item.MsrtType == EMsrtType.FIMVSWEEP ||
                item.MsrtType == EMsrtType.FVMISWEEP)
            {
                smuSetting.IsAutoMsrtRange = item.IsSweepAutoMsrtRange;
            }

            switch (item.SweepMode)
            {
                case ESweepMode.Linear:
                    smuSetting.SweepMode = EK2600SweepMode.Linear;
                    break;
                case ESweepMode.Log:
                    smuSetting.SweepMode = EK2600SweepMode.Log;
                    break;
                case ESweepMode.Custom:
                    smuSetting.SweepMode = EK2600SweepMode.Custom;
                    smuSetting.SweepCustomList = item.SweepCustomValue.ToList();
                    smuSetting.SweepPoints = (uint)smuSetting.SweepCustomList.Count;
                    break;
            }

            smuSetting.SweepStart = item.SweepStart;
            smuSetting.SweepStep = item.SweepStep;
            smuSetting.SweepStop = item.SweepStop;
            smuSetting.IsSweepFirstElec = item.IsSweepFirstElec;
            smuSetting.IsSweepEnd = item.IsSweepEnd;

            smuSetting.SweepEndPulseTurnOffTime = item.SweepTurnOffTime;// Math.Round((item.SweepTurnOffTime / 1000.0d), 6, MidpointRounding.AwayFromZero);

            if (smuSetting.SweepEndPulseTurnOffTime == 0.0d)
            {
                smuSetting.SweepEndPulseAction = EK2600EndPulseAction.SOURCE_HOLD;
            }
            else
            {
                smuSetting.SweepEndPulseAction = EK2600EndPulseAction.SOURCE_IDEL;
            }

            smuSetting.Duty = item.Duty;
        }

        private ElectSettingData TransDetecotrElecSetting(ElectSettingData item, int smuID = K2600Const.ID_SMUA)
        {
            ElectSettingData trans = null;

            if (smuID == K2600Const.ID_SMUA)
            {
                trans = new ElectSettingData();

                trans.MsrtType = EMsrtType.FVMI;
                trans.ForceDelayTime = 0.0d;
                trans.ForceTimeUnit = ETimeUnit.ms.ToString();

                trans.ForceRange = item.DetectorBiasValue;
                trans.ForceTime = 0.0d;
                trans.ForceValue = item.DetectorBiasValue;
                trans.ForceUnit = EVoltUnit.V.ToString();

                trans.MsrtRange = item.DetectorMsrtRange;
                trans.MsrtProtection = item.DetectorMsrtRange;
                trans.MsrtFilterCount = 0;
                trans.MsrtNPLC = item.DetectorMsrtNPLC;
                trans.MsrtUnit = EAmpUnit.A.ToString();
            }
            else if (smuID == K2600Const.ID_SMUB)
            {
                trans = new ElectSettingData();

                trans.MsrtType = EMsrtType.FVMI;
                trans.ForceDelayTime = 0.0d;
                trans.ForceTimeUnit = ETimeUnit.ms.ToString();

                trans.ForceRange = item.DetectorBiasValue2;
                trans.ForceTime = 0.0d;
                trans.ForceValue = item.DetectorBiasValue2;
                trans.ForceUnit = EVoltUnit.V.ToString();

                trans.MsrtRange = item.DetectorMsrtRange2;
                trans.MsrtProtection = item.DetectorMsrtRange2;
                trans.MsrtFilterCount = 0;
                trans.MsrtNPLC = item.DetectorMsrtNPLC2;
                trans.MsrtUnit = EAmpUnit.A.ToString();
            }

            return trans;
        }

		#endregion

        #region >>> Public Method <<<

        public bool Init()
		{
			string connInfo = string.Empty;

            if (this._conn != null)
            {
                this._conn.Close();
            }

            bool connectSuccess = false;

            for (int i = 0; i < 3; ++i)
            {
                if (this._conn.Open(out connInfo))
                {
                    connectSuccess = true;
                    break;
                }
            }

            if (!connectSuccess)
            {
                Console.WriteLine("[K2600Device], Open(), Open Fail");
                return false;
            }


			if (!this.GetDeviceInfomation())
			{
				Console.WriteLine("[K2600Device], QueryIDN(), QueryIDN Fail");

				return false;
			}

            this.ClearBuffer();

			if (!this.SetConfig())
			{
				Console.WriteLine("[K2600Device], ConfigDevice(), ConfigDevice Fail");

				return false;
			}

            //string script = SetSingleIOScript(7, EIOState.LOW, EIOTrig_Mode.TRIG_RISING, 0);

            //this._conn.SendCommand(script);
            //if (!this.SetAutoContactItemScript())
            //{
            //    Console.WriteLine("[K2600Device], SetAutoContactItemScript(), SetAutoContactItemScript Fail");

            //    return false;
            //}

			return true;
		}

		public void Close()
		{
			string cmd = string.Empty;

			cmd = "setOutput(0)\n";

            // Local
			cmd += "display.sendkey(75)";

			this._conn.SendCommand(cmd);

			this._conn.Close();
		}

		public void Reset()
		{
			string cmd = string.Empty;

			cmd += "status.reset()\n";

			cmd += "opc()\n";

			this._conn.SendCommand(cmd);

			this.ClearBuffer();
		}

        public bool ESDDetection()
        {
            string cmd = string.Empty;

            string outstr = string.Empty;

            cmd += "print(digio.readbit(" + _pinESDDetection.ToString() + "))\n";

            this._conn.SendCommand(cmd);

            this._conn.WaitAndGetData(out outstr);

            outstr = outstr.TrimEnd('\n');

            double bitstate = double.Parse(outstr);

            if (bitstate == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SendCommand(string cmd)
        {
            this._conn.SendCommand(cmd);
            return true;
        }
        #region >>> Set Parameter <<<

        /// <summary>
        /// Set Master SMU-A test item script, ElectSettingData中, 包含SMU-B的描述(Detector-CH etc.)
        /// 不支援 TERMINAL
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool SetTestItemScripts_Master(uint index, ElectSettingData item, bool isMsrtResult = true)
        {
            this._errMsg = string.Empty;

            string cmd = string.Empty;

            if (!this.CheckDCModeElectSetting(item))
            {
                return false;
            }

            EMsrtType type = item.MsrtType;

            if (type == EMsrtType.R)
            {
                //if (item.IsEnableK26ContactFuction)
                //{
                    type = EMsrtType.CONTACTCHECK;  // 
                //}
            }

            switch (type)
            {
                case EMsrtType.FIMV:
                case EMsrtType.FI:
                case EMsrtType.FVMI:
                case EMsrtType.POLAR:
                    {
                        
                        this.SetScriptParamter(index, item, null);
#if USE_MPI_PROTECT_MODULE
                        #region >>> IZ item "Protection Module" <<<

                        this._scriptParam.PMResistance = (EK2600ProtectionModuleResistance)item.PMSelectResistance;

                        switch (this._elecDevSetting.ProtectionModule)
                        {
                            case EProtectionModule.MPI_KPM:
                                {
                                    this._scriptParam.ProtectionModuleSN = EK2600ProtectionModule.MPI_KPM;
                                    break;
                                }
                            case EProtectionModule.ATV_PM2:
                                {
                                    this._scriptParam.ProtectionModuleSN = EK2600ProtectionModule.ATV_PM2;
                                    break;
                                }
                            default:
                                {
                                    this._scriptParam.ProtectionModuleSN = EK2600ProtectionModule.NONE;
                                    break;
                                }
                        }


                        if (item.KeyName.Contains("IZ"))
                        {
                            if (this._isProtectionModuleState)
                            {
                                // IZ item, PM 已開啟, 保持Relay 目前開啟狀態
                                this._scriptParam.SetProtectionModuleStatus = EK2600ProtectionModuleState.KeepTheLast;
                            }
                            else
                            {
                                // IZ item, PM 未開啟, 開啟Relay
                                this._scriptParam.SetProtectionModuleStatus = EK2600ProtectionModuleState.ON;
                                this._isProtectionModuleState = true;
                            }
                        }
                        else
                        {
                            if (this._isProtectionModuleState)
                            {
                                // 非IZ item, PM 已開啟, 關閉Relay
                                this._scriptParam.SetProtectionModuleStatus = EK2600ProtectionModuleState.OFF;
                                this._isProtectionModuleState = false;
                            }
                            else
                            {
                                // 非IZ item, PM 已關閉, 保持Relay 目前關閉狀態
                                this._scriptParam.SetProtectionModuleStatus = EK2600ProtectionModuleState.KeepTheLast;
                            }
                        }

                        #endregion
#endif
                        if (this._elecDevSetting.SrcTriggerMode == ESMUTriggerMode.PMDT && this._isDualSMU)
                        {
                            this.SetScriptParamter(index, item, item);
                            if (item.IsPulseMode)
                            {
                                cmd = K2600Script.PulseI_PMDT_DUAL(this._scriptParam);
                            }
                            else
                            {
                                cmd = K2600Script.DC_PMDT_DUAL(this._scriptParam);
                            }
                        }
                        else
                        {
                            cmd = K2600Script.DC_SMUA(this._scriptParam);
                        }

                        break;
                    }
                case EMsrtType.R:
                    {
                        this.SetScriptParamter(index, item, null);

                        cmd = K2600Script.ROhm_SMUA(this._scriptParam);
                        
                        break;
                    }
                case EMsrtType.CONTACTCHECK:
                    {
                        this.SetScriptParamter(index, item, null);
                        
                        if (this._elecDevSetting.SrcTriggerMode == ESMUTriggerMode.PMDT && this._isDualSMU)
                        {
                            cmd = K2600Script.ContactCheck_PMDT_DUAL(this._scriptParam);
                        }
                        else
                        {
                            cmd = K2600Script.ContactCheck_SMUA(this._scriptParam);
                        }
   
                        break;
                    }

                case EMsrtType.FIMVSWEEP:
                case EMsrtType.FVMISWEEP:
                    {
                        if (!this.SetSweepListScript(index, item))
                        {
                            return false;
                        }

                        //if (item.ElecTerminalSetting != null)
                        //{
                        //    if( !SetCustomerSweepScript(index, item))
                        //    {
                        //        return false;
                        //    }
                        //}

                        this.SetScriptParamter(index, item, null);

                        K2600ScriptSetting tempSSetting = this._scriptParam.Clone() as K2600ScriptSetting;

                        //cmd = K2600Script.Sweep_SMUA(this._scriptParam);
                        cmd = K2600Script.Sweep_SMUAV2(this._scriptParam);
                        this._conn.SendCommand(cmd);

                        
                        break;
                    }
                case EMsrtType.THY:
                    {
                        if (isMsrtResult) 
                        {
                            if (this._elecDevSetting.SrcTriggerMode == ESMUTriggerMode.PMDT && this._isDualSMU)
                            {
                                this.SetScriptParamter(index, item, null);

                                cmd = K2600Script.THY_PMDT_DUAL(this._scriptParam);
                            }
                            else
                            {
                                // THY, Keithley sampling
                                this.SetScriptParamter(index, item, null);

                                cmd = K2600Script.THY(this._scriptParam);
                            }
                        } 
                        else 
                        {
                            if (this._elecDevSetting.SrcTriggerMode == ESMUTriggerMode.PMDT && this._isDualSMU)
                            {
                                // THY, DAQ sampling
                                this.SetScriptParamter(index, item, null);

                                cmd = K2600Script.THY_SrcOnly_PMDT_DUAL(this._scriptParam);
                                this._conn.SendCommand(cmd);

                                cmd = K2600Script.THY_Trig_DAQ_PMDT_DUAL(this._scriptParam);
                                this._conn.SendCommand(cmd);

                            }
                            else
                            {
                                // THY, DAQ sampling
                                this.SetScriptParamter(index, item, null);

                                cmd = K2600Script.THY_SrcOnly(this._scriptParam);
                                this._conn.SendCommand(cmd);

                                cmd = K2600Script.THY_Trig_DAQ(this._scriptParam);
                                this._conn.SendCommand(cmd);
                            }
                        }
                        break;
                    }
                case EMsrtType.FIMVLOP:
                case EMsrtType.FVMILOP:
                case EMsrtType.LIV:
                case EMsrtType.LVI:
                    {
                        ElectSettingData itemB = null;

                        if (this._elecDevSetting.DetectorMsrtDevice == EPDSensingMode.SrcMeter_SMUB)
                        {
                            itemB = this.TransDetecotrElecSetting(item); // Detector Setting的描述, 在ElecSettingData內

                            item.IsAutoTurnOff = true;
                        }
                        else
                        {
                            // 需 S/W 或 H/W Trigger Detector-CH, 須等Detector量測結束後, 再由外部關閉
                            // Detector設定在, SetTestItemScripts_Slave() 
                            item.IsAutoTurnOff = false;
                        }

                        this.SetScriptParamter(index, item, itemB);

                        cmd = K2600Script.LOP_DUAL(this._scriptParam, this._elecDevSetting.IsDetectorHwTrig);
                        
                        break;
                    }
                case EMsrtType.PIV:
                    {
                        if (!this.SetSweepListScript(index, item))
                        {
                            return false;
                        }

                        ElectSettingData itemB = null;

                        if (this._elecDevSetting.DetectorMsrtDevice == EPDSensingMode.SrcMeter_SMUB)
                        {
                            itemB = this.TransDetecotrElecSetting(item); // Detector Setting的描述, 在ElecSettingData內
                        }

                        this.SetScriptParamter(index, item, itemB);   

                        cmd = K2600Script.PIV_DUAL(this._scriptParam);

                        break;
                    }
                case EMsrtType.FVMISCAN:
                    {
                        if (!this.SetSweepListScript(index, item))
                        {
                            return false;
                        }
                        this.SetScriptParamter(index, item, null);

                        cmd = K2600Script.ScanI_SMUA(this._scriptParam);

                        break;
                    }
                case EMsrtType.LCR:
                case EMsrtType.LCRSWEEP:               
                    {
                        this.SetScriptParamter(index, item, null);

                        cmd = K2600Script.LCR_Bias_SMUA(this._scriptParam);
                        
                        break;
                    }
                case EMsrtType.IO:
                    {
                        cmd = SetIO(index, item);
                    }
                    break;


                default:
                    {
                        this._errMsg = "SetTestItems, Master Not support MsrtType " + item.MsrtType.ToString();
                        return false;
                    }
            }

            if (cmd == string.Empty)
            {
                this._errMsg = "SetTestItems, Master No Match Command, " + item.MsrtType.ToString();
                return false;
            }

            this._conn.SendCommand(cmd);

            return !this.GetErrorMsg();
        }

        private static void OverriTerminal2ScriptParam(K2600SmuSetting smuaSet, ElecTerminalSetting ets)
        {
            smuaSet.SweepPoints = ets.SweepRiseCount;
            smuaSet.srcTime = ets.ForceTime;
            smuaSet.MsrtClamp = ets.MsrtProtection;
            smuaSet.MsrtRange = ets.MsrtRange;
            smuaSet.SrcRange = ets.ForceRange;
            smuaSet.MsrtNPLC = ets.MsrtNPLC;
            smuaSet.WaitTime = 0;
            smuaSet.SweepEndPulseTurnOffTime = ets.SweepTurnOffTime;
            smuaSet.IsAutoMsrtRange = ets.MsrtAutoRange;
            if (ets.SweepTurnOffTime == 0)
            {
                smuaSet.SweepEndPulseAction = EK2600EndPulseAction.SOURCE_HOLD;
            }
            else
            {
                smuaSet.SweepEndPulseAction = EK2600EndPulseAction.SOURCE_IDEL;
            }
            smuaSet.SweepStartHoldTime = 0;
        }

       // public bool 

        /// <summary>
        /// Set Slave SMU-A test item script, ElectSettingData中, 包含SMU-B的描述(Dual Detector-CH etc.)
        /// 支援 LOP, LIV, PIV
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool SetTestItemScripts_Slave(uint index, ElectSettingData item, bool isMsrtResult = true)
        {
             this._errMsg = string.Empty;

            string cmd = string.Empty;

            if (!this.CheckDCModeElectSetting(item))
            {
                return false;
            }

            switch (item.MsrtType)
            {
                case EMsrtType.FIMVLOP:
                case EMsrtType.FVMILOP:
                case EMsrtType.LIV:
                case EMsrtType.LVI:
                    {
                        ElectSettingData itemA = this.TransDetecotrElecSetting(item);

                        ElectSettingData itemB = this.TransDetecotrElecSetting(item);

                        this.SetScriptParamter(index, itemA, itemB);

                        this._scriptParam.SMUA.IsEnableSmu = item.IsTrigDetector;

                        this._scriptParam.SMUB.IsEnableSmu = item.IsTrigDetector2 & this._isDualSMU;

                        cmd = K2600Script.LOP_SLAVE(this._scriptParam, this._elecDevSetting.IsDetectorHwTrig);

                        break;
                    }
                case EMsrtType.PIV:
                    {
                        ElectSettingData itemA = this.TransDetecotrElecSetting(item);

                        this.SetScriptParamter(index, itemA, null);

                        this._scriptParam.SMUA.IsEnableSmu = item.IsTrigDetector;

                        cmd = K2600Script.PIV_SLAVE(this._scriptParam);

                        break;
                    }
                default:
                    {
                        this._errMsg = "SetTestItems, Slave Not support MsrtType " + item.MsrtType.ToString();
                        return false;
                    }
            }

            this._conn.SendCommand(cmd);

            return !this.GetErrorMsg();
        }

        #region >>> Terminal DC / Pulse Mode <<<

        public void SetTerminalDCModeScriptSMUA(ElectSettingData item)
        {
            this.CheckDCModeElectSetting(item); //Modify Force Time

            this.SetScriptParamter(K2600Const.INDEX_TERMINAL, item, null);

            string cmd = K2600Script.DC_SMUA(this._scriptParam);
           
            this._conn.SendCommand(cmd);
        }

        public void SetTerminalDCModeScriptSMUB(ElectSettingData item)
        {
            this.CheckDCModeElectSetting(item); //Modify Force Time

            this.SetScriptParamter(K2600Const.INDEX_TERMINAL, null, item);

            string cmd = K2600Script.DC_SMUB(this._scriptParam);

            this._conn.SendCommand(cmd);
        }

        public void SetTerminalDCModeScriptDual(ElectSettingData itemA, ElectSettingData itemB)
        {
            this.CheckDCModeElectSetting(itemA); //Modify Force Time

            this.CheckDCModeElectSetting(itemB);

            this.SetScriptParamter(K2600Const.INDEX_TERMINAL, itemA, itemB);

            string cmd = K2600Script.DC_DUAL(this._scriptParam);

            this._conn.SendCommand(cmd);
        }

        public void SetTerminalPulseModeScriptSMUA(ElectSettingData item, EK2600IOTriggerSynMode mode, uint devicePin, uint syschroneziPin, uint[] monitorPin)
        {
            this.CheckDCModeElectSetting(item); //Modify Force Time

            this.SetScriptParamter(K2600Const.INDEX_TERMINAL, item, null);

            string cmd = K2600Script.PulseTermSweep_SMUA(this._scriptParam, mode, devicePin, syschroneziPin, monitorPin);

            this._conn.SendCommand(cmd);
        }

        public void SetTerminalPulseModeScriptSMUB(ElectSettingData item, EK2600IOTriggerSynMode mode, uint devicePin, uint syschroneziPin, uint[] monitorPin)
        {
            this.CheckDCModeElectSetting(item); //Modify Force Time

            this.SetScriptParamter(K2600Const.INDEX_TERMINAL, null, item);

            string cmd = K2600Script.PulseTermSweep_SMUB(this._scriptParam, mode, devicePin, syschroneziPin, monitorPin);

            this._conn.SendCommand(cmd);
        }

        public void SetTerminalPulseModeScriptDUAL(ElectSettingData itemA, ElectSettingData itemB, EK2600IOTriggerSynMode mode, uint devicePin, uint syschroneziPin, uint[] monitorPin)
        {
            this.CheckDCModeElectSetting(itemA); //Modify Force Time

            this.CheckDCModeElectSetting(itemB); //Modify Force Time

            this.SetScriptParamter(K2600Const.INDEX_TERMINAL, itemA, itemB);

            string cmd = K2600Script.PulseTermSweep_DUAL(this._scriptParam, mode, devicePin, syschroneziPin, monitorPin);

            this._conn.SendCommand(cmd);
        }

        public bool SetTerminalDetector_Slave(ElectSettingData item)
        {
            this.SetScriptParamter(K2600Const.INDEX_TERMINAL_PD, item, item);

            this._scriptParam.SMUA.IsEnableSmu = item.IsTrigDetector;

            this._scriptParam.SMUB.IsEnableSmu = item.IsTrigDetector2 & this._isDualSMU;

            string cmd = K2600Script.LOP_SLAVE(this._scriptParam, false);

            this._conn.SendCommand(cmd);

            return !this.GetErrorMsg();
        }

        public bool AddSettingDataIntoBuffer(int smuIdex, ElectSettingData item)
        {
            if (smuIdex != K2600Const.ID_SMUA && smuIdex != K2600Const.ID_SMUB)
            {
                return false;
            }

            ElectSettingData data = item.Clone() as ElectSettingData;

            if (!this._parameterBuff.ContainsKey(smuIdex))
            {
                this._parameterBuff.Add(smuIdex, new List<ElectSettingData> { data });
            }
            else
            {
                this._parameterBuff[smuIdex].Add(data);
            }

            return true;
        }

        public bool SetDCItemFromBuffer()
        {
            if (this._parameterBuff.Count > 0)
            {
                bool isSetSmuA = false;
                bool isSetSmuB = false;

                int lengthSmuA = 0;
                int lengthSmuB = 0;

                if (this._parameterBuff.ContainsKey(K2600Const.ID_SMUA))
                {
                    lengthSmuA = this._parameterBuff[K2600Const.ID_SMUA].Count;
                    isSetSmuA = true;
                }

                if (this._parameterBuff.ContainsKey(K2600Const.ID_SMUB))
                {
                    lengthSmuB = this._parameterBuff[K2600Const.ID_SMUB].Count;
                    isSetSmuB = true;
                }

                ElectSettingData dataA;
                ElectSettingData dataB;

                if (isSetSmuA && isSetSmuB)
                {
                    #region >>> Set Parameter to SMU-A & SMU-B <<<

                    if (lengthSmuA != lengthSmuB)
                    {
                        return false;
                    }

                    for (int i = 0; i < lengthSmuA; i++)
                    {
                        dataA = this._parameterBuff[K2600Const.ID_SMUA][i];
                        dataB = this._parameterBuff[K2600Const.ID_SMUB][i];

                        if (!this.CheckDCModeElectSetting(dataA) || !this.CheckDCModeElectSetting(dataB))
                        {
                            return false;
                        }

                        //this.SetDCMsrtItemScript_Dual((uint)i, dataA, dataB);

                        //if (this.GetErrorMsg())
                        //{
                        //    return false;
                        //}
                    }

                    #endregion
                }
                else if (isSetSmuB)
                {
                    #region >>> Set Parameter to SMU-B <<<

                    for (int i = 0; i < lengthSmuB; i++)
                    {
                        dataB = this._parameterBuff[K2600Const.ID_SMUB][i];

                        if (!this.CheckDCModeElectSetting(dataB))
                        {
                            return false;
                        }

                        //this.SetDCMsrtItemScript_B((uint)i, dataB);

                        //if (this.GetErrorMsg())
                        //{
                        //    return false;
                        //}
                    }

                    #endregion
                }
                else
                {
                    #region >>> Set Parameter to SMU-A <<<

                    for (int i = 0; i < lengthSmuA; i++)
                    {
                        dataA = this._parameterBuff[K2600Const.ID_SMUA][i];

                        if (!this.CheckDCModeElectSetting(dataA))
                        {
                            return false;
                        }

                        //this.SetDCMsrtItemScript((uint)i, dataA);

                        //if (this.GetErrorMsg())
                        //{
                        //    return false;
                        //}
                    }

                    #endregion
                }
            }

            return true;
        }

        #endregion

        #endregion

        #region >>> Trigger <<<

        public void MeterOutput(uint index)
        {
            string cmd = string.Empty;

            if (this._elecDevSetting.SrcTriggerMode == ESMUTriggerMode.PMDT && this._isDualSMU)
            {
                string channel = string.Empty;

                if (this._syncTrigSMU.Count == 1)
                {
                    if (this._syncTrigSMU.Contains(K2600Const.NAME_SMUA))
                    {
                        channel = "channel=1\n";  // SMU-A Trigger
                    }
                    else
                    {
                        channel = "channel=2\n"; // SMU-B Trigger
                    }
                }
                else
                {
                    channel = "channel=0\n"; // SMU-A & SMU-B Trigger
                }

                cmd += "num_" + index.ToString() + ".run()";

                this._conn.SendCommand(channel + cmd);
            }
            else
            {
                cmd += "num_" + index.ToString() + ".run()";

                this._conn.SendCommand(cmd);
            }

            this._isQuary = false;
        }

        public void MeterOutput(string id)
        {
            string cmd = string.Empty;

            if (this._elecDevSetting.SrcTriggerMode == ESMUTriggerMode.PMDT && this._isDualSMU)
            {
                string channel = string.Empty;

                if (this._syncTrigSMU.Count == 1)
                {
                    if (this._syncTrigSMU.Contains(K2600Const.NAME_SMUA))
                    {
                        channel = "channel=1\n";  // SMU-A Trigger
                    }
                    else
                    {
                        channel = "channel=2\n"; // SMU-B Trigger
                    }
                }
                else
                {
                    channel = "channel=0\n"; // SMU-A & SMU-B Trigger
                }

                cmd += "num_" + id + ".run()";

                this._conn.SendCommand(channel + cmd);
            }
            else
            {
                cmd += "num_" + id + ".run()";

                this._conn.SendCommand(cmd);
            }

            this._isQuary = false;
        }


        public void MeterOutTerminalScript()
        {
            string script = string.Empty;

            script += "num_" + K2600Const.INDEX_TERMINAL + ".run()";

            this._conn.SendCommand(script);

            this._isQuary = false;
        }

        public void MeterOutTerminalPDScript()
        {
            string script = string.Empty;

            script += "num_" + K2600Const.INDEX_TERMINAL_PD + ".run()";

            this._conn.SendCommand(script);

            this._isQuary = false;
        }

        public void TriggerDAQ(uint index)
        {
            this._conn.SendCommand("num_" + index.ToString() + "_DAQTrigger.run()");
        }

        #endregion

        #region >>> Acquire Data <<<

        public string[] AcquireMsrtData()
        {
            return this.AcquireMsrtData("");
        }

        public string[] AcquireMsrtData(string smu,int delayInms=0)
        {
            ////////////////////////////////////////////////////////////////////////////////
            List<double> timeList = new List<double>();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Reset();
            stopwatch.Start();
            ////////////////////////////////////////////////////////////////////////////////
            timeList.Add(stopwatch.ElapsedMilliseconds);
            
            if ((this._elecDevSetting.SrcTriggerMode == ESMUTriggerMode.Multiple || this._elecDevSetting.SrcTriggerMode == ESMUTriggerMode.PMDT) && this._isDualSMU)
            {

                if (this._syncTrigSMU.Count == 1)
                {
                    string[] rtn = this.GetDevicePrintValueToArray(',', delayInms);
                    timeList.Add(stopwatch.ElapsedMilliseconds);
                    return rtn;
                }
                else
                {
                    if (!this._isQuary)
                    {
                        string[] rtn = this.GetDevicePrintValueToArray(',', delayInms);

                        this._acquireBuff[0] = new string[rtn.Length / 2];

                        this._acquireBuff[1] = new string[rtn.Length / 2];

                        for (int i = 0; i < rtn.Length; i += 2)
                        {
                            this._acquireBuff[0][i / 2] = rtn[i];

                            this._acquireBuff[1][i / 2] = rtn[i + 1];
                        }

                        this._isQuary = true;
                    }

                    if (smu == K2600Const.NAME_SMUA)
                    {
                        return this._acquireBuff[0];
                    }
                    else
                    {
                        return this._acquireBuff[1];
                    }
                }
            }
            else
            {
                ////////////////////////////////////////////////////////////////////////////////
                string[] rtn = this.GetDevicePrintValueToArray(',');
                timeList.Add(stopwatch.ElapsedMilliseconds);
                return rtn;

            }
        }

        public string[] AcquireMsrtDataAtOnce(EMsrtType type)
        {
            string msrtFunc;

            if (type == EMsrtType.MI)
            {
                msrtFunc = "i";
            }
            else
            {
                msrtFunc = "v";
            }

            string script = "print(mrtA." + msrtFunc + "())";

            this._conn.SendCommand(script);

            return GetDevicePrintValueToArray(',');
        }

        public string[] AcquireMsrtDataAtOnce_B(EMsrtType type)
        {
            string msrtFunc;

            if (type == EMsrtType.MI)
            {
                msrtFunc = "i";
            }
            else
            {
                msrtFunc = "v";
            }

            string script = "print(mrtB." + msrtFunc + "())";

            this._conn.SendCommand(script);

            return GetDevicePrintValueToArray(',');
        }

        public string[] AcquireMsrtDataAtOnce_Dual(EMsrtType typeSmuA, EMsrtType typeSmuB)
        {
            string script = string.Empty;

            if (typeSmuA == EMsrtType.MI)
            {
                script += "mrtAsynci(bufA1)\n";  // overlappedi
            }
            else if (typeSmuA == EMsrtType.MV)
            {
                script += "mrtAsyncv(bufA1)\n";  // overlappedV
            }
            else
            {
                return null;
            }

            if (typeSmuB == EMsrtType.MI)
            {
                script += "mrtBsynci(bufB1)\n";  // overlappedi
            }
            else if (typeSmuB == EMsrtType.MV)
            {
                script += "mrtBsyncv(bufB1)\n";  // overlappedV
            }
            else
            {
                return null;
            }

            script += "waitcomplete()\n";

            script += "printbuffer(1, 1, bufA1, bufB1)\n";

            script += "bufA1.clear()\n";

            script += "bufB1.clear()\n";

            this._conn.SendCommand(script);

            return GetDevicePrintValueToArray(',');
        }

        public string[] GetTHYTimestanps(uint index, ElectSettingData item)
        {
            string script = "";

            string srcRngAndComplScript = "";

            double complLimit = 0.0d;

            if (item.MsrtType == EMsrtType.THY)
            {
                complLimit = this._voltRange[0][this._voltRange[0].Length - 1];
            }

            //-----------------------------------------------------------------------------------------
            // Force Range and Compliance Setting
            //-----------------------------------------------------------------------------------------
            if (item.MsrtProtection <= complLimit)
            {
                srcRngAndComplScript += "setLimitV(" + item.MsrtProtection.ToString() + ")\n";

                srcRngAndComplScript += "setSorceRangeI(0.000001)\n";
            }
            else
            {
                srcRngAndComplScript += "setSorceRangeI(0.000001)\n";

                srcRngAndComplScript += "setLimitV(" + item.MsrtProtection.ToString() + ")\n";
            }

            script = "loadscript num_" + index.ToString() + "Timestamps\n";

            script += "setNPLC(" + item.MsrtNPLC.ToString() + ")\n";

            int measureCount = ((int)item.SweepContCount + 1) / 2;

            if (measureCount < 100)
            {
                measureCount = 100;
            }

            script += "setMsrtCount(" + measureCount.ToString() + ")\n";

            script += "setMsrtInterval(0.000001)\n";

            script += "setBufTimestamps(1)\n";

            script += "if getFunc ~= 0 then setFunc(0) end\n";

            script += "setMeasureRangeV(" + item.MsrtRange.ToString() + ")\n";

            script += "setMeasureRangeI(0.000001)\n";

            script += srcRngAndComplScript;

            script += "setLevelI(0.00000)\n";

            script += "mrtA.v(bufA1)\n";

            script += "for i=1, " + measureCount.ToString() + " do\n";

            script += "print(bufA1.timestamps[i])\n";

            script += "end\n";

            script += "setNPLC(0.01)\n";

            script += "setMsrtCount(1)\n";

            script += "bufA1.clear()\n";

            script += "setBufTimestamps(0)\n";

            script += "endscript\n";

            script += "num_" + index.ToString() + "Timestamps.source = nil";

            this._conn.SendCommand(script);

            this._conn.SendCommand("num_" + index.ToString() + "Timestamps.run()");

            string returnString = string.Empty;

            string[] returnStrData = null;

            if ((this._conn as LANConnect).WaitAndGetData(out returnString, measureCount))
            {
                returnString = returnString.TrimEnd('\n');

                returnStrData = returnString.Split('\n');
            }

            return returnStrData;
        }

        #endregion

        #region >>> Turn On/Off <<<

        public void TurnOff()
		{
			string cmd = string.Empty;

            cmd += "setLevelI(0)\n";

            cmd += "if getFunc() ~= 1 then setFunc(1) end\n";

            cmd += "setLevelV(0)\n";

            cmd += "setOutput(0)";

            cmd += "setSorceRangeI(0.01)\n";

            cmd += "setSorceRangeV(0.01)\n";

            if (this._isDualSMU)
            {
                cmd += "\nsetBLevelI(0)\n";

                cmd += "if getBFunc() ~= 1 then setBFunc(1) end\n";

                cmd += "setBLevelV(0)\n";

                cmd += "setBOutput(0)";
            }

			this._conn.SendCommand(cmd);
		}

        public void TurnOff(double delay, bool isOpenRelay)
		{
			string cmd = string.Empty;

            cmd += "setLevelI(0)\n";

            cmd += "if getFunc() ~= 1 then setFunc(1) end\n";

            cmd += "setLevelV(0)\n";

            cmd += "setOutput(0)\n";

            if (this._isDualSMU)
            {
                cmd += "setBLevelI(0)\n";

                cmd += "if getBFunc() ~= 1 then setBFunc(1) end\n";

                cmd += "setBLevelV(0)\n";

                cmd += "setBOutput(0)\n";
            }

            cmd += "delay(" + (delay / 1000.0d).ToString() + ")\n";			

            cmd += "setSorceRangeI(0.01)\n";

            cmd += "setSorceRangeV(0.01)\n";

            cmd += "print(digio.readport())";

			this._conn.SendCommand(cmd);

            //string readStr;

            //this._conn.WaitAndGetData(out readStr);
		}
        public string CheckIfFin()
        {
            string readStr;

            this._conn.WaitAndGetData(out readStr);

            return readStr;
 
		}

        public void TurnOffSMUA(double delay, bool isOpenRelay)
        {
            string cmd = string.Empty;

            cmd += "setLevelI(0)\n";

            cmd += "if getFunc() ~= 1 then setFunc(1) end\n";

            cmd += "setLevelV(0)\n";

            cmd += "setOutput(0)";

            cmd += "setSorceRangeI(0.01)\n";

            cmd += "setSorceRangeV(0.01)\n";

            cmd += "delay(" + (delay / 1000.0d).ToString() + ")";

            this._conn.SendCommand(cmd);
        }

        public void TurnOffSMUB(double delay, bool isOpenRelay)
        {
            if (this._isDualSMU)
            {
                string cmd = string.Empty;

                cmd += "\nsetBLevelI(0)\n";

                cmd += "if getBFunc() ~= 1 then setBFunc(1) end\n";

                cmd += "setBLevelV(0)\n";

                cmd += "setBOutput(0)";

                cmd += "delay(" + (delay / 1000.0d).ToString() + ")";

                this._conn.SendCommand(cmd);
            }
        }

        public void OutputOn()
        {
            //if (_elecDevSetting.SrcTurnOffType == ESrcTurnOffType.TestEnd)
            //{
            //    this._conn.SendCommand("setOutput(1)");
            //}
        }

        public void OutputOff()
        {
            if (_elecDevSetting.SrcTurnOffType == ESrcTurnOffType.TestEnd)
            {
                this._conn.SendCommand("setOutput(0)");
            }
        }

        public void TurnOffVLRLoop()
        {
            string script = string.Empty;

            script += "digio.writebit(" + K2600Const.IO_RTH_EANBLE.ToString() + ", 1)\n";

            script += "digio.writebit(" + K2600Const.IO_DAQ_ENABLE.ToString() + ", 0)\n";

            script += "digio.writebit(" + K2600Const.IO_POLAR_SW.ToString() + ", 0)";

            this._conn.SendCommand(script);
        }

        #endregion

        public bool CheckElectSetting(ElectSettingData item)
        {
            if (item.IsPulseMode)
            {
                return this.CheckPulseModeElectSetting(item);
            }
            else
            {
                return this.CheckDCModeElectSetting(item);
            }
        }

        public void ClearBuffer()
        {
           this._parameterBuff.Clear();
            
            string cmd = string.Empty;

            cmd += "errorqueue.clear()\n";

            cmd += "smua.nvbuffer1.clear()\n";

            cmd += "smua.nvbuffer2.clear()\n";

            cmd += "smua.nvbuffer1.appendmode = 1\n";

            cmd += "smua.nvbuffer2.appendmode = 1\n";

            cmd += "smua.nvbuffer1.collecttimestamps = 0\n";

            cmd += "smua.nvbuffer2.collecttimestamps = 0\n";

            cmd += "smua.nvbuffer1.collectsourcevalues = 0\n";

            cmd += "smua.nvbuffer2.collectsourcevalues = 0";

            if (this._isDualSMU)
            {
                cmd += "\n smub.nvbuffer1.clear()\n";

                cmd += "smub.nvbuffer2.clear()\n";

                cmd += "smub.nvbuffer1.appendmode = 1\n";

                cmd += "smub.nvbuffer2.appendmode = 1\n";

                cmd += "smub.nvbuffer1.collecttimestamps = 0\n";

                cmd += "smub.nvbuffer2.collecttimestamps = 0\n";

                cmd += "smub.nvbuffer1.collectsourcevalues = 0\n";

                cmd += "smub.nvbuffer2.collectsourcevalues = 0";
            }

            this._conn.SendCommand(cmd);
        }

        public bool ReadPortStatus(uint pin)
        {
            // True --> High
            // False --> Low
            byte result = 0x00;

            string script = string.Empty;

            string readStr = string.Empty;

            script = "print(digio.readport())";

            this._conn.SendCommand(script);

            if (this._conn.WaitAndGetData(out readStr))
            {
                if (readStr != string.Empty)
                {
                    uint tempData = (uint)Convert.ToDouble(readStr);

                    result = (byte)((tempData >> (int)pin) & 0x01);
                }
            }

            if (result > 0)
            {
                return true;
            }

            return false;
        }

        public void Abort()
        {
            string cmd = "smua.abort()";
            
            this._conn.SendCommand(cmd);
        }

        public void CollectGarbage()
        {
            string cmd = "collectgarbage()";

            this._conn.SendCommand(cmd);
        }

        public void RunFunctionFVMI(double waitTime, double forceValueA, double forceValueB, double forceTime, double msrtRange, double nplc,int ch)
        {
            string func = string.Empty;

            func = K2600Script.CallFuncFVMI(waitTime, forceValueA, forceValueB, forceTime, msrtRange, nplc, ch);

            this._conn.SendCommand(func);

            this._isQuary = false;
        }
        public void RunFunctionFIMV(double waitTime, double forceValueA, double forceValueB, double forceTime, double msrtRange, double nplc, int ch)
        {
            string func = string.Empty;

            func = K2600Script.CallFuncFIMV(waitTime, forceValueA, forceValueB, forceTime, msrtRange, nplc, ch);

            this._conn.SendCommand(func);

            this._isQuary = false;
        }

        public uint DioRead()
        {
            uint result = 0;

            string script = string.Empty;

            string readStr = string.Empty;

            script = "print(digio.readport())";

            this._conn.SendCommand(script);

            if (this._conn.WaitAndGetData(out readStr))
            {
                if (readStr != string.Empty)
                {
                    result = (uint)Convert.ToDouble(readStr);
                }
            }

            return result;
        }

        public void DioWrite(uint data)
        {
            string cmd = string.Empty;

            cmd += string.Format("digio.writeport({0})\n", data);

            this._conn.SendCommand(cmd);
        }

        /// <summary>
        /// [20180227_Porter]
        /// Auto Shift Filter Wheel To Target position
        /// </summary>
        public void FilterWheelMoveToTarget(uint TargetPos)
        {
            uint data = this.DioRead();

            byte startPos = (byte)((data >> 7) & 0x07);

            string moveOneStep = "digio.writebit(" + K2600Const.IO_AutoShiftFilter.ToString()+ ", 1)\n"
                               + "delay(0.500)\n"
                               + "digio.writebit(" + K2600Const.IO_AutoShiftFilter.ToString() + ", 0)\n"
                               + "delay(0.500)\n";

            string scrip = string.Empty;

            if (startPos == 0x00)
            {
                scrip = moveOneStep.TrimEnd('\n');

                this._conn.SendCommand(scrip);

                data = this.DioRead();

                startPos = (byte)((data >> 7) & 0x07);
            }

            int stepCnt = 0;
            scrip = string.Empty;

            if ((int)TargetPos > (int)startPos)
            {
                stepCnt = (int)TargetPos - (int)startPos;
            }
            else if ((int)TargetPos < (int)startPos)
            {
                stepCnt = (int)TargetPos - (int)startPos + 5;
            }

            for (int i = 0; i < stepCnt * 2; i++)
            {
                scrip += moveOneStep;
            }

            scrip = scrip.TrimEnd('\n');

            this._conn.SendCommand(scrip);
        }

		#endregion

		#region >>> Public Proberty <<<

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

        public string Name
        {
            get { return this._name; }
        }

        public double[] SweepPoints
        {
            get { return this._sweepList.ToArray(); }
        }

        public List<double> SweepTimeSpan 
        {
            get { return _sweepTimeSpan; }
        }

        public double[][] CurrentRange
        {
            get { return this._currRange; }
        }

        public double[][] VoltageRange
        {
            get { return this._voltRange; }
        }

        public List<string> SyncTrigSMU
        {
            get { return this._syncTrigSMU; }
            set { this._syncTrigSMU = value; }
        }

        public uint ID
        {
            get { return this._id; }
        }

        public bool IsDualSMU
        {
            get { return this._isDualSMU; }
        }

        public string ErrorMsg
        {
            get { return this._errMsg; }
        }

        public string IpAddress
        {
            get { return this._lanSetting.IPAddress; }
        }

        public ushort DioWriteBuffer
        {
            get { return this._dioOutputData; }
            set { this._dioOutputData = value; }
        }

		#endregion
    }
}