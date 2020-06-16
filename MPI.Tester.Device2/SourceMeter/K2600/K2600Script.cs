
//#define USE_MPI_PROTECT_MODULE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Device.SourceMeter.KeithleySMU
{
    /// <summary>
    /// 20170517 Roy
    /// 1. K26x1系列, 僅能使用 SMUA 的Script; K26x2系列, 才能使用 SMUB / DUAL 的Script
    /// 2. Script 有動用到雙SMU, 後面加DUAL;  
    /// 3. 牽涉到多台儀器, 請用 MASTER / SLAVE 區隔
    /// 4. K2600Script 僅 return string; 在上層呼叫 Script, 需要再 SendCommand
    /// 5. RTH 未驗證
    /// </summary>

    public static class K2600Script
    {
        private const bool WRITE_SCRIPT = false;

        

        private static int GetSrcMsrtMode(EK2600SrcMode srcMode, out string srcFunc, out string msrtFunc)
        {
            if (srcMode == EK2600SrcMode.I_Source)
            {
                srcFunc = "i";

                msrtFunc = "v";

                return 0;
            }
            else
            {
                srcFunc = "v";

                msrtFunc = "i";

                return 1;
            }

        }

        public static string MakeSetter_SMUA()
        {
            string cmd = string.Empty;

            cmd += "setLimitV = makesetter(smua.source, \"limitv\")\n";

            cmd += "setLimitI = makesetter(smua.source, \"limiti\")\n";

            cmd += "setSorceRangeV = makesetter(smua.source, \"rangev\")\n";

            cmd += "setSorceRangeI = makesetter(smua.source, \"rangei\")\n";

            cmd += "setLevelV = makesetter(smua.source, \"levelv\")\n";

            cmd += "setLevelI = makesetter(smua.source, \"leveli\")\n";

            cmd += "getFunc = makegetter(smua.source, \"func\")\n";

            cmd += "setFunc = makesetter(smua.source, \"func\")\n";

            cmd += "setMeasureRangeV = makesetter(smua.measure, \"rangev\")\n";

            cmd += "setMeasureRangeI = makesetter(smua.measure, \"rangei\")\n";

            cmd += "setMeasureDelay = makesetter(smua.measure, \"delay\")\n";

            cmd += "mrtA = smua.measure\n";

            cmd += "mrtIsyncA = smua.measure.overlappedi\n";

            cmd += "mrtVsyncA = smua.measure.overlappedv\n";

            cmd += "mrtIVsyncA = smua.measure.overlappediv\n";

            cmd += "setNPLC = makesetter(smua.measure, \"nplc\")\n";

            cmd += "setMsrtCount = makesetter(smua.measure, \"count\")";

            cmd += "setMsrtInterval = makesetter(smua.measure, \"interval\")\n";

            cmd += "setBufTimestamps = makesetter(smua.nvbuffer1, \"collecttimestamps\")\n";
            ///////////////////////////////////////////////////////////////////////////////////////////////
            cmd += "SetMrtIAutoRange = makesetter(smua.measure, \"autorangei\")\n";

            cmd += "SetMrtVAutoRange = makesetter(smua.measure, \"autorangev\")\n";

            cmd += "bufA1 = smua.nvbuffer1\n";

            //cmd += "smua.nvbuffer1.appendmode = 0";//20171218 David

            cmd += "bufA2 = smua.nvbuffer2\n";

            cmd += "setOutput = makesetter(smua.source, \"output\")\n";

            cmd += "getOutput = makegetter(smua.source, \"output\")\n";

            //---------------------------------------------------------------------------------------------------------------------------
            // Pulse Mode Command
            // Trigger / Arm Layer Setter
            cmd += "T1_Count = makesetter(trigger.timer[1], \"count\")\n";

            cmd += "T1_Delay = makesetter(trigger.timer[1], \"delay\")\n";

            cmd += "T1_Passthrough = makesetter(trigger.timer[1], \"passthrough\")\n";

            cmd += "T1_Stimulus = makesetter(trigger.timer[1], \"stimulus\")\n";

            cmd += "T2_Count = makesetter(trigger.timer[2], \"count\")\n";

            cmd += "T2_Delay = makesetter(trigger.timer[2], \"delay\")\n";

            cmd += "T2_Passthrough = makesetter(trigger.timer[2], \"passthrough\")\n";

            cmd += "T2_Stimulus = makesetter(trigger.timer[2], \"stimulus\")\n";

            cmd += "T3_Count = makesetter(trigger.timer[3], \"count\")\n";

            cmd += "T3_Delay = makesetter(trigger.timer[3], \"delay\")\n";

            cmd += "T3_Passthrough = makesetter(trigger.timer[3], \"passthrough\")\n";

            cmd += "T3_Stimulus = makesetter(trigger.timer[3], \"stimulus\")\n";

            cmd += "T4_Count = makesetter(trigger.timer[4], \"count\")\n";

            cmd += "T4_Delay = makesetter(trigger.timer[4], \"delay\")\n";

            cmd += "T4_Passthrough = makesetter(trigger.timer[4], \"passthrough\")\n";

            cmd += "T4_Stimulus = makesetter(trigger.timer[4], \"stimulus\")\n";

            cmd += "Trig_T1 = trigger.timer[1]\n";

            cmd += "Trig_T2 = trigger.timer[2]\n";

            cmd += "Trig_T3 = trigger.timer[3]\n";

            cmd += "Trig_T4 = trigger.timer[4]\n";

            //---------------------------------------------------------------------------------------------------------------------------
            // smua setter
            cmd += "TrigA_LimitV = makesetter(smua.trigger.source, \"limitv\")\n";

            cmd += "TrigA_LimitI = makesetter(smua.trigger.source, \"limiti\")\n";

            cmd += "TrigA_Action_Src = makesetter(smua.trigger.source, \"action\")\n";

            cmd += "TrigA_Action_Msrt = makesetter(smua.trigger.measure, \"action\")\n";

            cmd += "TrigA_Action_EndPulse = makesetter(smua.trigger.endpulse, \"action\")\n";

            cmd += "TrigA_Action_EndSweep = makesetter(smua.trigger.endsweep, \"action\")\n";

            cmd += "TrigA_Stimulus_Arm = makesetter(smua.trigger.arm, \"stimulus\")\n";

            cmd += "TrigA_Stimulus_Src = makesetter(smua.trigger.source, \"stimulus\")\n";

            cmd += "TrigA_Stimulus_Msrt = makesetter(smua.trigger.measure, \"stimulus\")\n";

            cmd += "TrigA_Stimulus_EndPulse = makesetter(smua.trigger.endpulse, \"stimulus\")\n";

            cmd += "TrigA_Count = makesetter(smua.trigger, \"count\")\n";

            cmd += "TrigA = smua.trigger\n";

            cmd += "TrigA_Src = smua.trigger.source\n";

            cmd += "TrigA_Msrt = smua.trigger.measure\n";

            return cmd;
        }

        public static string MakeSetter_SMUB()
        {
            string cmd = string.Empty;

            cmd += "setBLimitV = makesetter(smub.source, \"limitv\")\n";

            cmd += "setBLimitI = makesetter(smub.source, \"limiti\")\n";

            cmd += "setBSorceRangeV = makesetter(smub.source, \"rangev\")\n";

            cmd += "setBSorceRangeI = makesetter(smub.source, \"rangei\")\n";

            cmd += "setBLevelV = makesetter(smub.source, \"levelv\")\n";

            cmd += "setBLevelI = makesetter(smub.source, \"leveli\")\n";

            cmd += "getBFunc = makegetter(smub.source, \"func\")\n";

            cmd += "setBFunc = makesetter(smub.source, \"func\")\n";

            cmd += "setBMeasureRangeV = makesetter(smub.measure, \"rangev\")\n";

            cmd += "setBMeasureRangeI = makesetter(smub.measure, \"rangei\")\n";

            cmd += "setBMeasureDelay = makesetter(smub.measure, \"delay\")\n";

            cmd += "mrtB = smub.measure\n";

            cmd += "mrtIsyncB = smub.measure.overlappedi\n";

            cmd += "mrtVsyncB = smub.measure.overlappedv\n";

            cmd += "mrtIVsyncB = smub.measure.overlappediv\n";

            cmd += "setBNPLC = makesetter(smub.measure, \"nplc\")\n";

            cmd += "setBMsrtCount = makesetter(smub.measure, \"count\")";

            cmd += "setBMsrtInterval = makesetter(smub.measure, \"interval\")\n";

            cmd += "setBBufTimestamps = makesetter(smub.nvbuffer1, \"collecttimestamps\")\n";

            cmd += "bufB1 = smub.nvbuffer1\n";

            //cmd += "smub.nvbuffer1.appendmode = 0";//20171218 David

            cmd += "bufB2 = smub.nvbuffer2\n";

            cmd += "setBOutput = makesetter(smub.source, \"output\")\n";

            cmd += "getBOutput = makegetter(smub.source, \"output\")\n";

            // smua setter
            cmd += "TrigB_LimitV = makesetter(smub.trigger.source, \"limitv\")\n";

            cmd += "TrigB_LimitI = makesetter(smub.trigger.source, \"limiti\")\n";

            cmd += "TrigB_Action_Src = makesetter(smub.trigger.source, \"action\")\n";

            cmd += "TrigB_Action_Msrt = makesetter(smub.trigger.measure, \"action\")\n";

            cmd += "TrigB_Action_EndPulse = makesetter(smub.trigger.endpulse, \"action\")\n";

            cmd += "TrigB_Action_EndSweep = makesetter(smub.trigger.endsweep, \"action\")\n";

            cmd += "TrigB_Stimulus_Arm = makesetter(smub.trigger.arm, \"stimulus\")\n";

            cmd += "TrigB_Stimulus_Src = makesetter(smub.trigger.source, \"stimulus\")\n";

            cmd += "TrigB_Stimulus_Msrt = makesetter(smub.trigger.measure, \"stimulus\")\n";

            cmd += "TrigB_Stimulus_EndPulse = makesetter(smub.trigger.endpulse, \"stimulus\")\n";

            cmd += "TrigB_Count = makesetter(smub.trigger, \"count\")\n";

            cmd += "TrigB = smub.trigger\n";

            cmd += "TrigB_Src = smub.trigger.source\n";

            cmd += "TrigB_Msrt = smub.trigger.measure\n";

            return cmd;
        }

        public static string SetTimer(double pulseTrigDelay, double pulseWidth, double peroid, double msrtDelay, uint trigCount)
        {
            string script = string.Empty;

            if (pulseTrigDelay != 0.0d)
            {
                script += "T4_Count(1)\n";
                script += "T4_Delay(" + pulseTrigDelay.ToString() + ")\n";
                script += "T4_Passthrough(false)\n";                           // timer passthrough the trigger, 
                script += "T4_Stimulus(TrigA.ARMED_EVENT_ID)\n";

                script += "T1_Count(" + trigCount.ToString() + ")\n";
                script += "T1_Delay(" + peroid.ToString() + ")\n";
                script += "T1_Passthrough(true)\n";
                script += "T1_Stimulus(Trig_T4.EVENT_ID)\n";
            }
            else
            {
                script += "T1_Count(" + trigCount.ToString() + ")\n";
                script += "T1_Delay(" + peroid.ToString() + ")\n";
                script += "T1_Passthrough(true)\n";
                script += "T1_Stimulus(TrigA.ARMED_EVENT_ID)\n";
            }

            script += "T2_Count(1)\n";

            //script += string.Format("T2_Delay({0}-(1/localnode.linefreq)*{1}- 60e-6)\n", pulseWidth, nplc);
            script += string.Format("T2_Delay({0})\n", msrtDelay);

            // script += "T2_Delay(" + pulseWidth.ToString() + " - (1/localnode.linefreq)*" + nplc.ToString() + " - 60e-6)\n";
            script += "T2_Passthrough(false)\n";
            script += "T2_Stimulus(Trig_T1.EVENT_ID)\n";

            script += "T3_Count(1)\n";
            script += "T3_Delay(" + pulseWidth.ToString() + ")\n";
            script += "T3_Passthrough(false)\n";
            script += "T3_Stimulus(Trig_T1.EVENT_ID)\n";

            return script;
        }

        #region >>> DC Basic Script <<<

        private static string SetProtectionModule(EK2600ProtectionModule module, EK2600ProtectionModuleResistance resistance, EK2600ProtectionModuleState state)
        {
            string script = string.Empty;

            if (module == EK2600ProtectionModule.NONE)
            {
                return script;
            }

            string relay1 = string.Empty;
            string relay2 = string.Empty;

            SetProtectionModuleRelay(module, resistance, out relay1, out relay2);

            //  EK2600ProtectionModuleState.KeepTheLast : 保持 Relay 狀態, return string.Empty
            switch (state)
            {
                case EK2600ProtectionModuleState.ON:
                    {
                        script += "if getOutput() ~= 0 then setOutput(0) end\n";

                        script += "digio.writebit(" + K2600Const.IO_PM_RELAY1.ToString() + ", " + relay1 + ")\n";

                        script += "digio.writebit(" + K2600Const.IO_PM_RELAY2.ToString() + ", " + relay2 + ")\n";

                        script += "delay(0.001)\n";
                        
                        break;
                    }
                case EK2600ProtectionModuleState.OFF:
                    {
                        script += "if getOutput() ~= 0 then setOutput(0) end\n";

                        script += "digio.writebit(" + K2600Const.IO_PM_RELAY1.ToString() + ", 0)\n";

                        script += "digio.writebit(" + K2600Const.IO_PM_RELAY2.ToString() + ", 0)\n";

                        script += "delay(0.001)\n";

                        break;
                    }
            }
            
            return script;
        }

        public static string DC_SMUA(K2600ScriptSetting param)
        {
            string script = string.Empty;

            string srcRngAndComplScript = string.Empty;

            string srcFunc = string.Empty;

            string msrtFunc = string.Empty;

            int srcMode = GetSrcMsrtMode(param.SMUA.SrcMode, out srcFunc, out msrtFunc);    // [0] I Source;  [1] V Source

            double forceRange = param.SMUA.SrcRange;

            // ===========================================================================
            // Source Range < 1 uA, SMU 會有 20~30 ms 的前置時間, 才會打出訊號
            // ===========================================================================
            if (srcFunc == "i")
            {
                if (forceRange < 0.000001)
                {
                    forceRange = 0.000001;
                }
            }
            else
            {
                if (forceRange < 2.0d)
                {
                    script += "smua.measure.autozero = smua.AUTOZERO_ONCE\n";
                    //forceRange = 2.0d;
                }
            }

            // ===========================================================================
            // Fix SRC Current Range 利用 100uA Force Range 去量測 <100uA的Level
            // 降低under-shoot的產生
            // 高電壓檔位的量測，使用100uA的檔位去推動10uA，Rising會有問題產生
            // ===========================================================================

            if (param.SMUA.MsrtNPLC == 0.01d && param.SMUA.MsrtClamp < 20)
            {
                if (forceRange < 0.0001)
                {
                    forceRange = 0.0001;
                }
            }

            // IZ Clamp > 50.1，使用100uA檔位進行測試

            if (param.KeyName.Contains("IZ"))
            {
                if (param.SMUA.MsrtClamp > 50.1)
                {
                    if (forceRange < 0.0001)
                    {
                        forceRange = 0.0001;
                    }
                }

                if (param.IsSpReverseCurrentRange)
                {
                    double applyforceRange = param.SpReverseCurrentApplyRange * 0.001;

                    forceRange = applyforceRange;
                }
            }

            if (param.SMUA.MsrtClamp <= param.SMUA.MsrtBoundryLimit)
            {
                if (msrtFunc == "v")
                {
                    srcRngAndComplScript += "setLimitV(" + param.SMUA.MsrtClamp.ToString() + ")\n";

                    srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    srcRngAndComplScript += "setLimitI(" + param.SMUA.MsrtClamp.ToString() + ")\n";

                    srcRngAndComplScript += "setSorceRangeV(" + forceRange.ToString() + ")\n";

                }
            }
            else
            {
                if (msrtFunc == "v")
                {
                    srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";

                    srcRngAndComplScript += "setLimitV(" + param.SMUA.MsrtClamp.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    srcRngAndComplScript += "setSorceRangeV(" + forceRange.ToString() + ")\n";

                    srcRngAndComplScript += "setLimitI(" + param.SMUA.MsrtClamp.ToString() + ")\n";
                }
            }

            //-----------------------------------------------------------------------------------------
            // Test Sequence
            //-----------------------------------------------------------------------------------------
            script = "loadscript num_" + param.Index.ToString() + "\n";

#if USE_MPI_PROTECT_MODULE
            script += SetProtectionModule(param.ProtectionModuleSN, param.PMResistance, param.SetProtectionModuleStatus);
#endif
            script += "setNPLC(" + param.SMUA.MsrtNPLC.ToString() + ")\n";

            script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";

            if (param.SMUA.IsAutoMsrtRange)
            {
                script += "smua.measure.autorangev = 1\n";

                script += "smua.measure.autorangei = 1\n";
            }
            else
            {
                if (msrtFunc == "v")
                {
                    script += "setMeasureRangeV(" + param.SMUA.MsrtRange.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    script += "setMeasureRangeI(" + param.SMUA.MsrtRange.ToString() + ")\n";
                }
            }

            //script += "if getOutput() ~= 1 then setOutput(1) end\n";

            script += srcRngAndComplScript;

            //======================================
            // 切完Range後，在Set Range & Compliance
            //=======================================
            if (param.SMUA.WaitTime > 0)
            {
                script += "delay(" + param.SMUA.WaitTime.ToString() + ")\n";
            }

            if (srcFunc == "v")
            {
                script += "setLevelV(" + param.SMUA.SrcLevel.ToString() + ")\n";
            }
            else if (srcFunc == "i")
            {
                script += "setLevelI(" + param.SMUA.SrcLevel.ToString() + ")\n";
            }

            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            script += "delay(" + param.SMUA.srcTime.ToString() + ")\n";


            //======================================
            // 回傳量測值
            //=======================================
            if (param.SMUA.IsEnableMsrt)
            {
                if (param.SMUA.IsEnableMsrtSrcLevel)
                {
                    script += "mrtIVsyncA(bufA1, bufA2)\n";  // overlappediv
                    script += "waitcomplete()\n";

                    script += "printbuffer(1, 1, bufA1, bufA2)\n";  // print i and v
                }
                else
                {
                    script += "print(mrtA." + msrtFunc + "())\n";
                }
            }

            //-----------------------------------------------------------------------------------------
            // 若打IZ量VZ的Clamp電壓大於20，打完IZ後先降Clamp，以免多花20mS時間
            //-----------------------------------------------------------------------------------------
            if (param.SMUA.MsrtClamp > 20 && srcFunc == "i" && param.SMUA.IsAutoTurnOff)
            {
                script += "setLimitV(8)\n";
            }

            if (param.SMUA.IsAutoTurnOff)
            {
                if (srcFunc == "v")
                {
                    script += "setLevelV(0)\n";

                    //if (param.SMUA.MsrtRange <= 1E-6)//避免下一道變慢
                    //{
                    //    script += "setMeasureRangeI(0.000001)\n";
                    //    script += "setLimitI(0.01)\n";
                    //}
                }
                else if (srcFunc == "i")
                {
                    script += "setLevelI(0)\n";

                    if (param.IsTurnOffToZeroVolt)
                    {
                        script += "setFunc(1)\n";

                        script += "setLevelV(0)\n";
                    }

                    if (param.IsTurnOffToDefaultRange)
                    {
                        script += "setSorceRangeI(0.01)\n";
                       // script += "mrtIVsyncA(bufA1, bufA2)\n";  // overlappediv
                    }
                    else
                    {
                        if (forceRange >= 0.1)
                        {
                            // 防止切換 1A Range 時, 產生 Overshoot
                            script += "setSorceRangeI(0.01)\n";
                        }
                    }
                }
            }

            if (param.SMUA.IsAutoMsrtRange)
            {
                script += "smua.measure.autorangev = 0\n";
                script += "smua.measure.autorangei = 0\n";
            }

            if (param.SMUA.IsEnableMsrtSrcLevel)
            {
                // Clear Buffer
                script += "bufA1.clear()\n";
                script += "bufA2.clear()\n";
            }

            script += "endscript\n";

            script += "num_" + param.Index.ToString() + ".source = nil";

            //////////////////////////////////////////////////////
            if (WRITE_SCRIPT)
            {
                string fileName = "MPI\\LEDTester\\Log\\ScripLog\\" + "DC_" + param.KeyName;
                ExportScriptToTxt(script, fileName);
            }
            /////////////////////////////////////////////////////////////////////

            return script;
        }

        public static string DC_SMUB(K2600ScriptSetting param)
        {
            string script = string.Empty;

            string srcRngAndComplScript = string.Empty;

            string srcFunc = string.Empty;

            string msrtFunc = string.Empty;

            int srcMode = GetSrcMsrtMode(param.SMUB.SrcMode, out srcFunc, out msrtFunc);    // [0] I Source;  [1] V Source

            double forceRange = param.SMUB.SrcRange;

            // ===========================================================================
            // Source Range < 1 uA, SMU 會有 20~30 ms 的前置時間, 才會打出訊號
            // ===========================================================================
            if (srcFunc == "i")
            {
                if (forceRange < 0.000001)
                {
                    forceRange = 0.000001;
                }
            }
            else
            {
                if (forceRange < 2.0d)
                {
                    script += "smub.measure.autozero = smua.AUTOZERO_ONCE\n";
                    //forceRange = 2.0d;
                }
            }

            // ===========================================================================
            // Fix SRC Current Range 利用 100uA Force Range 去量測 <100uA的Level
            // 降低under-shoot的產生
            // 高電壓檔位的量測，使用100uA的檔位去推動10uA，Rising會有問題產生
            // ===========================================================================

            if (param.SMUB.MsrtNPLC == 0.01d && param.SMUB.MsrtClamp < 20)
            {
                if (forceRange < 0.0001)
                {
                    forceRange = 0.0001;
                }
            }

            // IZ Clamp > 50.1，使用100uA檔位進行測試

            if (param.KeyName.Contains("IZ"))
            {
                if (param.SMUB.MsrtClamp > 50.1)
                {
                    if (forceRange < 0.0001)
                    {
                        forceRange = 0.0001;
                    }
                }

                if (param.IsSpReverseCurrentRange)
                {
                    double applyforceRange = param.SpReverseCurrentApplyRange * 0.001;

                    forceRange = applyforceRange;
                }
            }


            if (param.SMUB.MsrtClamp <= param.SMUB.MsrtBoundryLimit)
            {
                if (msrtFunc == "v")
                {
                    srcRngAndComplScript += "setBLimitV(" + param.SMUB.MsrtClamp.ToString() + ")\n";

                    srcRngAndComplScript += "setBSorceRangeI(" + forceRange.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    srcRngAndComplScript += "setBLimitI(" + param.SMUB.MsrtClamp.ToString() + ")\n";

                    srcRngAndComplScript += "setBSorceRangeV(" + forceRange.ToString() + ")\n";
                }
            }
            else
            {
                if (msrtFunc == "v")
                {
                    srcRngAndComplScript += "setBSorceRangeI(" + forceRange.ToString() + ")\n";

                    srcRngAndComplScript += "setBLimitV(" + param.SMUB.MsrtClamp.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    srcRngAndComplScript += "setBSorceRangeV(" + forceRange.ToString() + ")\n";

                    srcRngAndComplScript += "setBLimitI(" + param.SMUB.MsrtClamp.ToString() + ")\n";
                }
            }

            //-----------------------------------------------------------------------------------------
            // Test Sequence
            //-----------------------------------------------------------------------------------------
            script = "loadscript num_" + param.Index.ToString() + "\n";
#if USE_MPI_PROTECT_MODULE
            script += SetProtectionModule(param.ProtectionModuleSN, param.PMResistance, param.SetProtectionModuleStatus);
#endif
            script += "setBNPLC(" + param.SMUB.MsrtNPLC.ToString() + ")\n";

            script += "if getBFunc() ~= " + srcMode.ToString() + " then setBFunc(" + srcMode.ToString() + ") end\n";

            if (param.SMUB.IsAutoMsrtRange)
            {
                script += "smub.measure.autorangev = 1\n";

                script += "smub.measure.autorangei = 1\n";
            }
            else
            {
                if (msrtFunc == "v")
                {
                    script += "setBMeasureRangeV(" + param.SMUB.MsrtRange.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    script += "setBMeasureRangeI(" + param.SMUB.MsrtRange.ToString() + ")\n";
                }
            }

            script += "if getBOutput() ~= 1 then setBOutput(1) end\n";

            script += srcRngAndComplScript;

            //======================================
            // 切完Range後，在Set Range & Compliance
            //=======================================
            if (param.SMUB.WaitTime > 0)
            {
                script += "delay(" + param.SMUB.WaitTime.ToString() + ")\n";
            }

            if (srcFunc == "v")
            {
                script += "setBLevelV(" + param.SMUB.SrcLevel.ToString() + ")\n";
            }
            else if (srcFunc == "i")
            {
                script += "setBLevelI(" + param.SMUB.SrcLevel.ToString() + ")\n";
            }

            script += "delay(" + param.SMUB.srcTime.ToString() + ")\n";

            //======================================
            // 回傳量測值
            //=======================================
            if (param.SMUB.IsEnableMsrt)
            {
                script += "print(mrtB." + msrtFunc + "())\n";
            }

            //-----------------------------------------------------------------------------------------
            // 若打IZ量VZ的Clamp電壓大於20，打完IZ後先降Clamp，以免多花20mS時間
            //-----------------------------------------------------------------------------------------
            if (param.SMUB.MsrtClamp > 20 && srcFunc == "i" && param.SMUB.IsAutoTurnOff)
            {
                script += "setBLimitV(8)\n";
            }

            if (param.SMUB.IsAutoTurnOff)
            {
                if (srcFunc == "v")
                {
                    script += "setBLevelV(0)\n";

                    //if (param.SMUA.MsrtRange <= 1E-8)//避免下一道變慢
                    //{
                    //    script += "setBMeasureRangeI(0.01)\n";
                    //    script += "setBLimitI(0.01)\n";
                    //}
                }
                else if (srcFunc == "i")
                {
                    script += "setBLevelI(0)\n";

                    if (param.IsTurnOffToZeroVolt)
                    {
                        script += "setBFunc(1)\n";

                        script += "setBLevelV(0)\n";
                    }

                    if (param.IsTurnOffToDefaultRange)
                    {
                        script += "setBSorceRangeI(0.01)\n";
                    }
                    else
                    {
                        if (forceRange >= 0.1)
                        {
                            // 防止切換 1A Range 時, 產生 Overshoot
                            script += "setBSorceRangeI(0.01)\n";
                        }
                    }
                }

            }

            if (param.SMUB.IsAutoMsrtRange)
            {
                script += "smub.measure.autorangev = 0\n";

                script += "smub.measure.autorangei = 0\n";
            }

            script += "endscript\n";

            script += "num_" + param.Index.ToString() + ".source = nil";

            return script;
        }

        /// <summary>
        /// SMUA / SMUB 可各自做不同的測試項目, Trigger時, SMUA & SMUB 皆會動作
        /// </summary>
        public static string DC_DUAL(K2600ScriptSetting param)
        {
            string script = string.Empty;

            string srcRngAndComplScriptA = string.Empty;
            string srcRngAndComplScriptB = string.Empty;

            string srcFuncA = string.Empty;
            string srcFuncB = string.Empty;

            string msrtFuncA = string.Empty;
            string msrtFuncB = string.Empty;

            // [0] I Source;  [1] V Source
            int srcModeA = GetSrcMsrtMode(param.SMUA.SrcMode, out srcFuncA, out msrtFuncA);
            int srcModeB = GetSrcMsrtMode(param.SMUB.SrcMode, out srcFuncB, out msrtFuncB);

            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            double forceRangeA = param.SMUA.SrcRange;
            double forceRangeB = param.SMUB.SrcRange;

            // ===========================================================================
            // Source Range < 1 uA, SMU 會有 20~30 ms 的前置時間, 才會打出訊號
            // ===========================================================================
            if (srcFuncA == "i")
            {
                if (forceRangeA < 0.000001)
                {
                    forceRangeA = 0.000001;
                }
            }
            else
            {
                if (forceRangeA < 2.0d)
                {
                    script += "smua.measure.autozero = smua.AUTOZERO_ONCE\n";
                    //forceRangeA = 2.0d;
                }
            }

            if (srcFuncB == "i")
            {
                if (forceRangeB < 0.000001)
                {
                    forceRangeB = 0.000001;
                }
            }
            else
            {
                if (forceRangeB < 2.0d)
                {
                    script += "smub.measure.autozero = smua.AUTOZERO_ONCE\n";
                    //forceRangeB = 2.0d;
                }
            }

            // ===========================================================================
            // Fix SRC Current Range 利用 100uA Force Range 去量測 <100uA的Level
            // 降低under-shoot的產生
            // 高電壓檔位的量測，使用100uA的檔位去推動10uA，Rising會有問題產生
            // ===========================================================================
            if (param.SMUA.MsrtNPLC == 0.01d && param.SMUA.MsrtClamp < 20)
            {
                if (forceRangeA < 0.0001)
                {
                    forceRangeA = 0.0001;
                }
            }

            if (param.SMUB.MsrtNPLC == 0.01d && param.SMUB.MsrtClamp < 20)
            {
                if (forceRangeB < 0.0001)
                {
                    forceRangeB = 0.0001;
                }
            }
            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            // IZ Clamp > 50.1，使用100uA檔位進行測試
            // 此段先移除


            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            if (param.SMUA.MsrtClamp <= param.SMUA.MsrtBoundryLimit)
            {
                if (msrtFuncA == "v")
                {
                    srcRngAndComplScriptA += "setLimitV(" + param.SMUA.MsrtClamp.ToString() + ")\n";

                    srcRngAndComplScriptA += "setSorceRangeI(" + forceRangeA.ToString() + ")\n";
                }
                else if (msrtFuncA == "i")
                {
                    srcRngAndComplScriptA += "setLimitI(" + param.SMUA.MsrtClamp.ToString() + ")\n";

                    srcRngAndComplScriptA += "setSorceRangeV(" + forceRangeA.ToString() + ")\n";
                }
            }
            else
            {
                if (msrtFuncA == "v")
                {
                    srcRngAndComplScriptA += "setSorceRangeI(" + forceRangeA.ToString() + ")\n";

                    srcRngAndComplScriptA += "setLimitV(" + param.SMUA.MsrtClamp.ToString() + ")\n";
                }
                else if (msrtFuncA == "i")
                {
                    srcRngAndComplScriptA += "setSorceRangeV(" + forceRangeA.ToString() + ")\n";

                    srcRngAndComplScriptA += "setLimitI(" + param.SMUA.MsrtClamp.ToString() + ")\n";
                }
            }

            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            if (param.SMUB.MsrtClamp <= param.SMUB.MsrtBoundryLimit)
            {
                if (msrtFuncB == "v")
                {
                    srcRngAndComplScriptB += "setBLimitV(" + param.SMUB.MsrtClamp.ToString() + ")\n";

                    srcRngAndComplScriptB += "setBSorceRangeI(" + forceRangeB.ToString() + ")\n";
                }
                else if (msrtFuncB == "i")
                {
                    srcRngAndComplScriptB += "setBLimitI(" + param.SMUB.MsrtClamp.ToString() + ")\n";

                    srcRngAndComplScriptB += "setBSorceRangeV(" + forceRangeB.ToString() + ")\n";
                }
            }
            else
            {
                if (msrtFuncB == "v")
                {
                    srcRngAndComplScriptB += "setBSorceRangeI(" + forceRangeB.ToString() + ")\n";

                    srcRngAndComplScriptB += "setBLimitV(" + param.SMUB.MsrtClamp.ToString() + ")\n";
                }
                else if (msrtFuncB == "i")
                {
                    srcRngAndComplScriptB += "setBSorceRangeV(" + forceRangeB.ToString() + ")\n";

                    srcRngAndComplScriptB += "setBLimitI(" + param.SMUB.MsrtClamp.ToString() + ")\n";
                }
            }

            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            // Test Sequence
            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            script = "loadscript num_" + param.Index.ToString() + "\n";
#if USE_MPI_PROTECT_MODULE
            script += SetProtectionModule(param.ProtectionModuleSN, param.PMResistance, param.SetProtectionModuleStatus);
#endif
            script += "setNPLC(" + param.SMUA.MsrtNPLC.ToString() + ")\n";
            script += "setBNPLC(" + param.SMUB.MsrtNPLC.ToString() + ")\n";

            script += "if getFunc() ~= " + srcModeA.ToString() + " then setFunc(" + srcModeA.ToString() + ") end\n";
            script += "if getBFunc() ~= " + srcModeB.ToString() + " then setBFunc(" + srcModeB.ToString() + ") end\n";

            // 這段應該是 Stanley 偷懶...
            if (param.SMUA.IsAutoMsrtRange)
            {
                script += "smua.measure.autorangev = 1\n";
                script += "smua.measure.autorangei = 1\n";

                script += "smub.measure.autorangev = 1\n";
                script += "smub.measure.autorangei = 1\n";
            }
            else
            {
                if (msrtFuncA == "v")
                {
                    script += "setMeasureRangeV(" + param.SMUA.MsrtRange.ToString() + ")\n";

                    //script += "setMeasureRangeI(" + item.ForceRange.ToString() + ")\n";
                }
                else if (msrtFuncA == "i")
                {
                    script += "setMeasureRangeI(" + param.SMUA.MsrtRange.ToString() + ")\n";

                    //script += "setMeasureRangeV(" + item.ForceRange.ToString() + ")\n";
                }

                if (msrtFuncB == "v")
                {
                    script += "setBMeasureRangeV(" + param.SMUB.MsrtRange.ToString() + ")\n";

                    //script += "setMeasureRangeI(" + item.ForceRange.ToString() + ")\n";
                }
                else if (msrtFuncB == "i")
                {
                    script += "setBMeasureRangeI(" + param.SMUB.MsrtRange.ToString() + ")\n";

                    //script += "setMeasureRangeV(" + item.ForceRange.ToString() + ")\n";
                }
            }

            script += "if getOutput() ~= 1 then setOutput(1) end\n";
            script += "if getBOutput() ~= 1 then setBOutput(1) end\n";

            script += srcRngAndComplScriptA;
            script += srcRngAndComplScriptB;
            //======================================
            // 切完Range後，在Set Range & Compliance
            //=======================================
            if (param.SMUA.WaitTime > 0)
            {
                script += "delay(" + param.SMUA.WaitTime.ToString() + ")\n";
            }

            if (srcFuncA == "v")
            {
                script += "setLevelV(" + param.SMUA.SrcLevel.ToString() + ")\n";
            }
            else if (srcFuncA == "i")
            {
                script += "setLevelI(" + param.SMUA.SrcLevel.ToString() + ")\n";
            }

            if (srcFuncB == "v")
            {
                script += "setBLevelV(" + param.SMUB.SrcLevel.ToString() + ")\n";
            }
            else if (srcFuncB == "i")
            {
                script += "setBLevelI(" + param.SMUB.SrcLevel.ToString() + ")\n";
            }

            script += "delay(" + param.SMUA.srcTime.ToString() + ")\n";

            //======================================
            // 回傳量測值
            //=======================================
            if (param.SMUA.IsEnableMsrt)
            {
                if (msrtFuncA == "v")
                {
                    script += "mrtVsyncA(bufA1)\n";  // overlappedV
                }
                else
                {
                    script += "mrtIsyncA(bufA1)\n";  // overlappedi
                }

                if (msrtFuncB == "v")
                {
                    script += "mrtVsyncB(bufB1)\n";  // overlappedV
                }
                else
                {
                    script += "mrtIsyncB(bufB1)\n";  // overlappedi
                }

                script += "waitcomplete()\n";

                script += "printbuffer(1, 1, bufA1, bufB1)\n";
            }

            //-----------------------------------------------------------------------------------------
            // 若打IZ量VZ的Clamp電壓大於20，打完IZ後先降Clamp，以免多花20mS時間
            //-----------------------------------------------------------------------------------------
            if (param.SMUA.MsrtClamp > 20 && srcFuncA == "i" && param.SMUA.IsAutoTurnOff)
            {
                script += "setLimitV(8)\n";
            }

            if (param.SMUB.MsrtClamp > 20 && srcFuncB == "i" && param.SMUB.IsAutoTurnOff)
            {
                script += "setBLimitV(8)\n";
            }


            if (param.SMUA.IsAutoTurnOff)
            {

                if (srcFuncA == "v")
                {
                    script += "setLevelV(0)\n";
                }
                else if (srcFuncA == "i")
                {

                    script += "setLevelI(0)\n";

                    if (!param.IsTurnOffToZeroVolt)
                    {
                        script += "setFunc(1)\n";

                        script += "setLevelV(0)\n";
                    }

                    if (param.IsTurnOffToDefaultRange)
                    {
                        script += "setSorceRangeI(0.01)\n";
                    }
                    else
                    {
                        if (forceRangeA >= 0.1)
                        {
                            // 防止切換 1A Range 時, 產生 Overshoot
                            script += "setSorceRangeI(0.01)\n";
                        }
                    }
                }

                if (srcFuncB == "v")
                {
                    script += "setBLevelV(0)\n";
                }
                else if (srcFuncB == "i")
                {
                    script += "setBLevelI(0)\n";

                    if (!param.IsTurnOffToZeroVolt)
                    {
                        script += "setBFunc(1)\n";

                        script += "setBLevelV(0)\n";
                    }

                    if (param.IsTurnOffToDefaultRange)
                    {
                        script += "setBSorceRangeI(0.01)\n";
                    }
                    else
                    {
                        if (forceRangeB >= 0.1)
                        {
                            // 防止切換 1A Range 時, 產生 Overshoot
                            script += "setBSorceRangeI(0.01)\n";
                        }
                    }
                }
            }
               

            script += "bufA1.clear()\n";

            script += "bufB1.clear()\n";

            if (param.SMUA.IsAutoMsrtRange)
            {
                script += "smua.measure.autorangev = 0\n";
                script += "smua.measure.autorangei = 0\n";

                script += "smub.measure.autorangev = 0\n";
                script += "smub.measure.autorangei = 0\n";
            }

            script += "endscript\n";

            script += "num_" + param.Index.ToString() + ".source = nil";

            return script;
        }

        /// <summary>
        /// SMUA / SMUB 各自做相同的測試項目 (SMUA copy to SMUB), Trigger時, 需動態決定 SMUA & SMUB 哪一個動作 (Dual Channel PMDT)
        /// </summary>
        public static string DC_PMDT_DUAL(K2600ScriptSetting param)
        {
            string script = string.Empty;

            string srcFunc = string.Empty;
            string msrtFunc = string.Empty;
            uint index = param.Index;
            double waitTime = param.SMUA.WaitTime;
            double forceTime = param.SMUA.srcTime;
            double forceRange = param.SMUA.SrcRange;


            ///////////////////////////////////////////////////////////////////////////////////////////////
            // SrcMode
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string srcModeScriptA = string.Empty;
            string srcModeScriptB = string.Empty;
            string srcModeScriptAB = string.Empty;

            SetSrcMode(param, ref  srcModeScriptA, ref  srcModeScriptB, ref  srcModeScriptAB);
            ///////////////////////////////////////////////////////////////////////////////////////////////
            // Force Range and Compliance Setting
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string srcRngAndComplScriptA = string.Empty;
            string srcRngAndComplScriptB = string.Empty;
            string srcRngAndComplScriptAB = string.Empty;

            SetPMDTSrcRange(param, ref srcRngAndComplScriptA, ref srcRngAndComplScriptB, ref srcRngAndComplScriptAB, ref forceRange);//srcFunc, msrtFunc, clampLimit, nplc, ref forceRange, clamp);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            // Range selected
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string msrtRangeScriptA = string.Empty;
            string msrtRangeScriptB = string.Empty;
            string msrtRangeScriptAB = string.Empty;

            SetPMDTMsrtRange(param, ref msrtRangeScriptA, ref msrtRangeScriptB, ref msrtRangeScriptAB);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            // source output
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string srcScriptA = string.Empty;
            string srcScriptB = string.Empty;
            string srcScriptAB = string.Empty;

            SetPMDTDC(param, ref srcScriptA, ref srcScriptB, ref srcScriptAB);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            // Msrt
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string msrtScriptA = string.Empty;
            string msrtScriptB = string.Empty;
            string msrtScriptAB = string.Empty;

            SetPMDTMsrtDC(param, ref msrtScriptA, ref msrtScriptB, ref msrtScriptAB);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            // Source Off
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string srcOffScriptA = string.Empty;
            string srcOffScriptB = string.Empty;
            string srcOffScriptAB = string.Empty;

            SetPMDTTurnOff(param, forceRange, ref srcOffScriptA, ref srcOffScriptB, ref srcOffScriptAB);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            // NPLC
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string NPLCScriptA = string.Empty;
            string NPLCScriptB = string.Empty;
            string NPLCScriptAB = string.Empty;

            SetPMDTNPLC(param, ref NPLCScriptA, ref NPLCScriptB, ref NPLCScriptAB);



            
            //////////////////////////////////////////////////////////////////////////////////////////////////////
            // Test Sequence
            //////////////////////////////////////////////////////////////////////////////////////////////////////
            script = "loadscript num_" + index.ToString() + "\n";

            //-------------------------------------------------------------------------------------------------
#if USE_MPI_PROTECT_MODULE
            script += SetProtectionModule(param.ProtectionModuleSN,param.PMResistance,param.SetProtectionModuleStatus);
#endif

            //--------------------------------------------------------------------------------------------------
            script += "if channel == 1 then\n"; // SMUA

            script += NPLCScriptA;

            //script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";
            script += srcModeScriptA;

            script += msrtRangeScriptA;

            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            script += srcRngAndComplScriptA;

            if (waitTime > 0)
            {
                script += "delay(" + waitTime.ToString() + ")\n";
            }

            script += srcScriptA;

            script += "delay(" + forceTime.ToString() + ")\n";

            script += msrtScriptA;

            script += srcOffScriptA;

            //--------------------------------------------------------------------------------------------------
            script += "elseif channel == 2 then\n"; // SMUB

            script += NPLCScriptB;
            //script += "if getBFunc() ~= " + srcMode.ToString() + " then setBFunc(" + srcMode.ToString() + ") end\n";
            script += srcModeScriptB;

            script += msrtRangeScriptB;

            script += "if getBOutput() ~= 1 then setBOutput(1) end\n";

            script += srcRngAndComplScriptB;

            if (waitTime > 0)
            {
                script += "delay(" + waitTime.ToString() + ")\n";
            }

            script += srcScriptB;

            script += "delay(" + forceTime.ToString() + ")\n";

            script += msrtScriptB;

            script += srcOffScriptB;

            //--------------------------------------------------------------------------------------------------
            script += "else\n"; // Both

            //script += "setNPLC(" + nplc.ToString() + ")\n";
            //script += "setBNPLC(" + nplc.ToString() + ")\n";
            script += NPLCScriptAB;

            //script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";
            //script += "if getBFunc() ~= " + srcMode.ToString() + " then setBFunc(" + srcMode.ToString() + ") end\n";
            script += srcModeScriptAB;

            script += msrtRangeScriptAB;

            script += "if getOutput() ~= 1 then setOutput(1) end\n";
            script += "if getBOutput() ~= 1 then setBOutput(1) end\n";

            script += srcRngAndComplScriptAB;

            if (waitTime > 0)
            {
                script += "delay(" + waitTime.ToString() + ")\n";
            }

            script += srcScriptAB;

            script += "delay(" + forceTime.ToString() + ")\n";

            script += msrtScriptAB;

            script += srcOffScriptAB;

            //script += "bufA1.clear()\n";
            //script += "bufB1.clear()\n";

            script += "end\n"; // if
            //--------------------------------------------------------------------------------------------------

            script += "channel = nil\n";
            script += "endscript\n";

            script += "num_" + index.ToString() + ".source = nil";

            //////////////////////////////////////////////////////
            if (WRITE_SCRIPT)
            {
                string fileName = "MPI\\LEDTester\\Log\\ScripLog\\" + "PMDT_DC_" + index;
                ExportScriptToTxt(script, fileName);
            }
            /////////////////////////////////////////////////////////////////////

            return script;
        }

        private static void SetSrcMode(K2600ScriptSetting param , ref string ScriptA, ref string ScriptB, ref string ScriptAB,int mode = -1)
        {
            string srcFunc = string.Empty;
            string msrtFunc = string.Empty;
            int srcAMode = GetSrcMsrtMode(param.SMUA.SrcMode, out srcFunc, out msrtFunc);
            if (mode != -1)
            {
                srcAMode = mode;
            }
            ScriptA = setSrcMode(param, srcAMode, "A");
            ScriptB = setSrcMode(param, srcAMode, "B");

            ScriptAB = ScriptA + ScriptB;
        }

            private static string setSrcMode(K2600ScriptSetting param, int srcMode, string smuCh)
            {

                bool isBrief = param.IsEnableBriefScript;
                string script = string.Empty;
                K2600SmuSetting smuInfo;
                string smu = smuCh.Trim().ToUpper();
                if (smu == "A")
                {
                    smuInfo = param.SMUA;
                    if (smuInfo.SrcModeSt != srcMode || !isBrief)
                    {
                        smuInfo.SrcModeSt = srcMode;
                        script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";
                    }
                }
                else
                {
                    smuInfo = param.SMUB;
                    if (smuInfo.SrcModeSt != srcMode || !isBrief)
                    {
                        smuInfo.SrcModeSt = srcMode;
                        script += "if getBFunc() ~= " + srcMode.ToString() + " then setBFunc(" + srcMode.ToString() + ") end\n";
                    }
                }
                return script;
            }

        private static void SetPMDTNPLC(K2600ScriptSetting param, ref string NPLCScriptA, ref string NPLCScriptB, ref string NPLCScriptAB)
        {
            NPLCScriptA = setNPLC(param, param.SMUA.MsrtNPLC, "A");
            NPLCScriptB = setNPLC(param, param.SMUB.MsrtNPLC, "B"); 

            NPLCScriptAB = NPLCScriptA + NPLCScriptB;
        }

            private static string setNPLC(K2600ScriptSetting param, double nplc, string smuCh)
            {
                bool isBrief = param.IsEnableBriefScript;
                string srcScript = string.Empty;
                K2600SmuSetting smuInfo;
                string smu = smuCh.Trim().ToUpper();
                if (smu == "A")
                {
                    smuInfo = param.SMUA;
                    if (smuInfo.NplcSt != nplc || !isBrief)
                    {
                        smuInfo.NplcSt = nplc;
                        srcScript += "setNPLC(" + nplc.ToString() + ")\n";
                    }
                }
                else
                {
                    smuInfo = param.SMUB;
                    if (smuInfo.NplcSt != nplc || !isBrief)
                    {
                        smuInfo.NplcSt = nplc;
                        srcScript += "setBNPLC(" + nplc.ToString() + ")\n";
                    }
                }
                return srcScript;
            }

        private static void SetPMDTDC(K2600ScriptSetting param, ref string srcScriptA, ref string srcScriptB, ref string srcScriptAB)
        {
            double forceValue = param.SMUA.SrcLevel;
            string srcFunc = string.Empty;
            string msrtFunc = string.Empty;
            int srcMode = GetSrcMsrtMode(param.SMUA.SrcMode, out srcFunc, out msrtFunc);

            if (srcFunc == "v")
            {
                srcScriptA += "setLevelV(" + forceValue.ToString() + ")\n";

                srcScriptB += "setBLevelV(" + forceValue.ToString() + ")\n";

                srcScriptAB += "setLevelV(" + forceValue.ToString() + ")\n";
                srcScriptAB += "setBLevelV(" + forceValue.ToString() + ")\n";
            }
            else if (srcFunc == "i")
            {
                srcScriptA += "setLevelI(" + forceValue.ToString() + ")\n";

                srcScriptB += "setBLevelI(" + forceValue.ToString() + ")\n";

                srcScriptAB += "setLevelI(" + forceValue.ToString() + ")\n";
                srcScriptAB += "setBLevelI(" + forceValue.ToString() + ")\n";
            }
        }

        private static void SetPMDTTurnOff(K2600ScriptSetting param, double forceRange, ref string srcOffScriptA, ref string srcOffScriptB, ref string srcOffScriptAB)
        {

            //-----------------------------------------------------------------------------------------
            // 若打IZ量VZ的Clamp電壓大於20，打完IZ後先降Clamp，以免多花20mS時間
            //-----------------------------------------------------------------------------------------


            bool isAutoTurnOff = param.SMUA.IsAutoTurnOff;
            double clamp = param.SMUA.MsrtClamp;
            string srcFunc = string.Empty;
            string msrtFunc = string.Empty;
            int srcMode = GetSrcMsrtMode(param.SMUA.SrcMode, out srcFunc, out msrtFunc);

            if (Math.Abs(clamp) > 20 && srcFunc == "i" && isAutoTurnOff)
            {
                string strA = setLimitV(param, 8, "A");                
                string strB = setLimitV(param, 8, "B");

                srcOffScriptA += strA;
                srcOffScriptB += strB;
                srcOffScriptAB += strA + strB;
            }

            if (isAutoTurnOff)
            {
                if (srcFunc == "v")
                {
                    srcOffScriptA += "setLevelV(0)\n";
                    srcOffScriptB += "setBLevelV(0)\n";

                    srcOffScriptAB += "setLevelV(0)\n";
                    srcOffScriptAB += "setBLevelV(0)\n";

                    if (param.SMUA.MsrtRange <= 1E-8)//避免下一道變慢
                    {
                        string strAr = setMeasureRangeI(param, 0.01, "A") ;
                        string strBr = setMeasureRangeI(param, 0.01, "B");

                        string strAl = setLimitI(param, 0.01, "A");
                        string strBl = setLimitI(param, 0.01, "B");

                        srcOffScriptA += strAr + strAl;
                        srcOffScriptB += strBr + strBl;
                        srcOffScriptAB += strAr + strBr + strAl + strBl;
                    }
                }
                else if (srcFunc == "i")
                {
                    srcOffScriptA += "setLevelI(0)\n";
                    srcOffScriptB += "setBLevelI(0)\n";

                    srcOffScriptAB += "setLevelI(0)\n";
                    srcOffScriptAB += "setBLevelI(0)\n";


                    if (param.IsTurnOffToZeroVolt)
                    {
                        string ScriptA = setSrcMode(param, 1, "A");
                        string ScriptB = setSrcMode(param, 1, "B");

                        string ScriptAB = ScriptA + ScriptB;


                        srcOffScriptA += ScriptA;
                        srcOffScriptA += "setLevelV(0)\n";

                        srcOffScriptB += ScriptB;
                        srcOffScriptB += "setBLevelV(0)\n";


                        srcOffScriptAB += ScriptA + ScriptB;
                        srcOffScriptAB += "setLevelV(0)\n";
                        srcOffScriptAB += "setBLevelV(0)\n";
                    }

                    if (param.IsTurnOffToDefaultRange || forceRange > 0.1)
                    {
                        string strA = setSorceRangeI(param, 0.01, "A");
                        string strB = setSorceRangeI(param, 0.01, "B");

                        srcOffScriptA += strA;
                        srcOffScriptB += strB;
                        srcOffScriptAB += strA + strB;
                    }
                }
            }
        }

            private static string setSorceRangeI(K2600ScriptSetting param, double currRange,string smuCh)
            {
                bool isBrief = param.IsEnableBriefScript;

                string srcScript = string.Empty;
                K2600SmuSetting smuInfo;
                string smu = smuCh.Trim().ToUpper();
                if (smu == "A")
                {
                    smuInfo = param.SMUA;
                    if (smuInfo.SrcCurrRangeSt != currRange || !isBrief)
                    {
                        smuInfo.SrcCurrRangeSt = currRange;
                        srcScript += "setSorceRangeI(" + currRange.ToString() + ")\n";
                    }
                }
                else
                {
                    smuInfo = param.SMUB;
                    if (smuInfo.SrcCurrRangeSt != currRange || !isBrief)
                    {
                        smuInfo.SrcCurrRangeSt = currRange;
                        srcScript += "setBSorceRangeI(" + currRange.ToString() + ")\n";
                    }
                }
                return srcScript;
            }

            private static string setSorceRangeV(K2600ScriptSetting param, double voltRange, string smuCh)
            {
                bool isBrief = param.IsEnableBriefScript;
                string srcScript = string.Empty;
                K2600SmuSetting smuInfo;
                string smu = smuCh.Trim().ToUpper();
                if (smu == "A")
                {
                    smuInfo = param.SMUA;
                    if (smuInfo.SrcVoltRangeSt != voltRange || param.KeyName.StartsWith("VR") || !isBrief)//20171218 David 目前發現在VR IZ VR的順序下，使用快速script時K2612會出現 5005: Value too big for range
                    {
                        smuInfo.SrcVoltRangeSt = voltRange;
                        srcScript += "setSorceRangeV(" + voltRange.ToString() + ")\n";
                    }
                }
                else
                {
                    smuInfo = param.SMUB;
                    if (smuInfo.SrcVoltRangeSt != voltRange || param.KeyName.StartsWith("VR") || !isBrief)
                    {
                        smuInfo.SrcVoltRangeSt = voltRange;
                        srcScript += "setBSorceRangeV(" + voltRange.ToString() + ")\n";
                    }
                }
                return srcScript;
            }

            private static string setMeasureRangeI(K2600ScriptSetting param, double currRange, string smuCh)
            {
                bool isBrief = param.IsEnableBriefScript;
                string msrtScript = string.Empty;
                K2600SmuSetting smuInfo;
                string smu = smuCh.Trim().ToUpper();
                if (smu == "A")
                {
                    smuInfo = param.SMUA;
                    if (smuInfo.MsrtCurrRangeSt != currRange || !isBrief)
                    {
                        smuInfo.MsrtCurrRangeSt = currRange;
                        msrtScript += "setMeasureRangeI(" + currRange.ToString() + ")\n";
                    }
                }
                else
                {
                    smuInfo = param.SMUB;
                    if (smuInfo.MsrtCurrRangeSt != currRange || !isBrief)
                    {
                        smuInfo.MsrtCurrRangeSt = currRange;
                        msrtScript += "setBMeasureRangeI(" + currRange.ToString() + ")\n";
                    }
                }
                return msrtScript;
            }

            private static string setMeasureRangeV(K2600ScriptSetting param, double voltRange, string smuCh)
            {
                bool isBrief = param.IsEnableBriefScript;
                string msrtScript = string.Empty;
                K2600SmuSetting smuInfo;
                string smu = smuCh.Trim().ToUpper();
                if (smu == "A")
                {
                    smuInfo = param.SMUA;
                    if (smuInfo.MsrtVoltRangeSt != voltRange || !isBrief)
                    {
                        smuInfo.MsrtVoltRangeSt = voltRange;
                        msrtScript += "setMeasureRangeV(" + voltRange.ToString() + ")\n";
                    }
                }
                else
                {
                    smuInfo = param.SMUB;
                    if (smuInfo.MsrtVoltRangeSt != voltRange || !isBrief)
                    {
                        smuInfo.MsrtVoltRangeSt = voltRange;
                        msrtScript += "setBMeasureRangeV(" + voltRange.ToString() + ")\n";
                    }
                }
                return msrtScript;
            }

            private static string setLimitI(K2600ScriptSetting param, double currLim, string smuCh)
            {
                bool isBrief = param.IsEnableBriefScript;
                string limScript = string.Empty;
                K2600SmuSetting smuInfo;
                string smu = smuCh.Trim().ToUpper();
                if (smu == "A")
                {
                    smuInfo = param.SMUA;
                    if (smuInfo.MsrtCurrClampSt != currLim || !isBrief)
                    {
                        smuInfo.MsrtCurrClampSt = currLim;
                        limScript += "setLimitI(" + currLim.ToString() + ")\n";
                    }
                }
                else
                {
                    smuInfo = param.SMUB;
                    if (smuInfo.MsrtCurrClampSt != currLim || !isBrief)
                    {
                        smuInfo.MsrtCurrClampSt = currLim;
                        limScript += "setBLimitI(" + currLim.ToString() + ")\n";
                    }
                }
                return limScript;
            }

            private static string setLimitV(K2600ScriptSetting param, double voltLim, string smuCh)
            {
                bool isBrief = param.IsEnableBriefScript;
                string limScript = string.Empty;
                K2600SmuSetting smuInfo;
                string smu = smuCh.Trim().ToUpper();
                if (smu == "A")
                {
                    smuInfo = param.SMUA;
                    if (smuInfo.MsrtVoltClampSt != voltLim || param.KeyName.StartsWith("VR") || !isBrief)
                    {
                        smuInfo.MsrtVoltClampSt = voltLim;
                        limScript += "setLimitV(" + voltLim.ToString() + ")\n";
                    }
                }
                else
                {
                    smuInfo = param.SMUB;
                    if (smuInfo.MsrtVoltClampSt != voltLim || param.KeyName.StartsWith("VR") || !isBrief)
                    {
                        smuInfo.MsrtVoltClampSt = voltLim;
                        limScript += "setBLimitV(" + voltLim.ToString() + ")\n";
                    }
                }
                return limScript;
            }

        private static void SetPMDTMsrtRange(K2600ScriptSetting param, ref string msrtRangeScriptA, ref string msrtRangeScriptB, ref string msrtRangeScriptAB)
        {
            string srcFunc = string.Empty;
            string msrtFunc = string.Empty;
            double msrtRange = param.SMUA.MsrtRange;
            GetSrcMsrtMode(param.SMUA.SrcMode, out srcFunc, out msrtFunc);

            if (msrtFunc == "v")
            {
                string strA = setMeasureRangeV(param, msrtRange, "A");
                string strB = setMeasureRangeV(param, msrtRange, "B");
                msrtRangeScriptA += strA;
                msrtRangeScriptB += strB;
                msrtRangeScriptAB += strA + strB;

                //msrtRangeScriptA += "setMeasureRangeV(" + msrtRange.ToString() + ")\n";
                //msrtRangeScriptB += "setBMeasureRangeV(" + msrtRange.ToString() + ")\n";
                //msrtRangeScriptAB += "setMeasureRangeV(" + msrtRange.ToString() + ")\n";
                //msrtRangeScriptAB += "setBMeasureRangeV(" + msrtRange.ToString() + ")\n";
            }
            else if (msrtFunc == "i")
            {
                string strA = setMeasureRangeI(param, msrtRange, "A");
                string strB = setMeasureRangeI(param, msrtRange, "B");
                msrtRangeScriptA += strA;
                msrtRangeScriptB += strB;
                msrtRangeScriptAB += strA + strB;
                //msrtRangeScriptA += "setMeasureRangeI(" + msrtRange.ToString() + ")\n";
                //msrtRangeScriptB += "setBMeasureRangeI(" + msrtRange.ToString() + ")\n";
                //msrtRangeScriptAB += "setMeasureRangeI(" + msrtRange.ToString() + ")\n";
                //msrtRangeScriptAB += "setBMeasureRangeI(" + msrtRange.ToString() + ")\n";
            }
        }

        private static void SetPMDTSrcRange(K2600ScriptSetting param, ref string srcRngAndComplScriptA, ref string srcRngAndComplScriptB, ref string srcRngAndComplScriptAB,
            ref double forceRange,string srcMode = "",string msrtMode = "")
        {
            string srcFunc = string.Empty;
            string msrtFunc = string.Empty;
            double nplc = param.SMUA.MsrtNPLC;
            double clamp = param.SMUA.MsrtClamp;
            double clampLimit = param.SMUA.MsrtBoundryLimit;
            GetSrcMsrtMode(param.SMUA.SrcMode, out srcFunc, out msrtFunc);

            if (srcMode != "")
            {
                srcFunc = srcMode;
            }
            if (msrtMode != "")
            {
                msrtFunc = msrtMode;
            }

            // ===========================================================================
            // Source Range < 1 uA, SMU 會有 20~30 ms 的前置時間, 才會打出訊號
            // ===========================================================================
            if (srcFunc == "i")
            {
                if (forceRange < 0.000001)
                {
                    forceRange = 0.000001;
                }
            }
            else
            {
                if (forceRange < 2.0d)
                {
                    forceRange = 2.0d;
                }
            }

            // ===========================================================================
            // Fix SRC Current Range 利用 100uA Force Range 去量測 <100uA的Level
            // 降低under-shoot的產生
            // 高電壓檔位的量測，使用100uA的檔位去推動10uA，Rising會有問題產生
            // ===========================================================================

            if (nplc == 0.01d && clamp < 20)
            {
                if (forceRange < 0.0001)
                {
                    forceRange = 0.0001;
                }
            }

            // IZ Clamp > 50.1，使用100uA檔位進行測試

            if (param.KeyName.Contains("IZ"))
            {
                if (clamp > 50.1)
                {
                    if (forceRange < 0.0001)
                    {
                        forceRange = 0.0001;
                    }
                }

                if (param.IsSpReverseCurrentRange)
                {
                    double applyforceRange = param.SpReverseCurrentApplyRange * 0.001;

                    forceRange = applyforceRange;
                }
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////
            // Range selected
            ///////////////////////////////////////////////////////////////////////////////////////////////
            if (clamp <= clampLimit)
            {
                if (msrtFunc == "v")
                {
                    string strAr = setSorceRangeI(param, forceRange, "A");
                    string strAl = setLimitV(param, clamp, "A");
                    string strBr = setSorceRangeI(param, forceRange, "B");
                    string strBl = setLimitV(param, clamp, "B");

                    srcRngAndComplScriptA += strAl + strAr  ;
                    srcRngAndComplScriptB += strBl + strBr  ;
                    srcRngAndComplScriptAB += strAl + strBl + strAr + strBr;
                    //srcRngAndComplScriptA += "setLimitV(" + clamp.ToString() + ")\n";
                    //srcRngAndComplScriptA += "setSorceRangeI(" + forceRange.ToString() + ")\n";

                    //srcRngAndComplScriptB += "setBLimitV(" + clamp.ToString() + ")\n";
                    //srcRngAndComplScriptB += "setBSorceRangeI(" + forceRange.ToString() + ")\n";

                    //srcRngAndComplScriptAB += "setLimitV(" + clamp.ToString() + ")\n";
                    //srcRngAndComplScriptAB += "setBLimitV(" + clamp.ToString() + ")\n";
                    //srcRngAndComplScriptAB += "setSorceRangeI(" + forceRange.ToString() + ")\n";
                    //srcRngAndComplScriptAB += "setBSorceRangeI(" + forceRange.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    string strAr = setSorceRangeV(param, forceRange, "A");
                    string strAl = setLimitI(param, clamp, "A");
                    string strBr = setSorceRangeV(param, forceRange, "B");
                    string strBl = setLimitI(param, clamp, "B");

                    srcRngAndComplScriptA += strAl + strAr;
                    srcRngAndComplScriptB += strBl + strBr;
                    srcRngAndComplScriptAB += strAl + strBl + strAr + strBr;
                    //srcRngAndComplScriptA += "setLimitI(" + clamp.ToString() + ")\n";
                    //srcRngAndComplScriptA += "setSorceRangeV(" + forceRange.ToString() + ")\n";

                    //srcRngAndComplScriptB += "setBLimitI(" + clamp.ToString() + ")\n";
                    //srcRngAndComplScriptB += "setBSorceRangeV(" + forceRange.ToString() + ")\n";

                    //srcRngAndComplScriptAB += "setLimitI(" + clamp.ToString() + ")\n";
                    //srcRngAndComplScriptAB += "setBLimitI(" + clamp.ToString() + ")\n";
                    //srcRngAndComplScriptAB += "setSorceRangeV(" + forceRange.ToString() + ")\n";
                    //srcRngAndComplScriptAB += "setBSorceRangeV(" + forceRange.ToString() + ")\n";
                }
            }
            else
            {
                if (msrtFunc == "v")
                {
                    string strAr = setSorceRangeI(param, forceRange, "A");
                    string strAl = setLimitV(param, clampLimit, "A");
                    string strBr = setSorceRangeI(param, forceRange, "B");
                    string strBl = setLimitV(param, clampLimit, "B");

                    srcRngAndComplScriptA += strAr + strAl;
                    srcRngAndComplScriptB += strBr + strBl;
                    srcRngAndComplScriptAB += strAr + strBr + strAl + strBl;

                    //srcRngAndComplScriptA += "setSorceRangeI(" + forceRange.ToString() + ")\n";
                    //srcRngAndComplScriptA += "setLimitV(" + clamp.ToString() + ")\n";

                    //srcRngAndComplScriptB += "setBSorceRangeI(" + forceRange.ToString() + ")\n";
                    //srcRngAndComplScriptB += "setBLimitV(" + clamp.ToString() + ")\n";

                    //srcRngAndComplScriptAB += "setSorceRangeI(" + forceRange.ToString() + ")\n";
                    //srcRngAndComplScriptAB += "setBSorceRangeI(" + forceRange.ToString() + ")\n";
                    //srcRngAndComplScriptAB += "setLimitV(" + clamp.ToString() + ")\n";
                    //srcRngAndComplScriptAB += "setBLimitV(" + clamp.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    string strAr = setSorceRangeV(param, forceRange, "A");
                    string strAl = setLimitI(param, clampLimit, "A");
                    string strBr = setSorceRangeV(param, forceRange, "B");
                    string strBl = setLimitI(param, clampLimit, "B");

                    srcRngAndComplScriptA += strAr + strAl;
                    srcRngAndComplScriptB += strBr + strBl;
                    srcRngAndComplScriptAB += strAr + strBr + strAl + strBl;
                    //srcRngAndComplScriptA += "setSorceRangeV(" + forceRange.ToString() + ")\n";
                    //srcRngAndComplScriptA += "setLimitI(" + clamp.ToString() + ")\n";

                    //srcRngAndComplScriptB += "setBSorceRangeV(" + forceRange.ToString() + ")\n";
                    //srcRngAndComplScriptB += "setBLimitI(" + clamp.ToString() + ")\n";

                    //srcRngAndComplScriptAB += "setSorceRangeV(" + forceRange.ToString() + ")\n";
                    //srcRngAndComplScriptAB += "setBSorceRangeV(" + forceRange.ToString() + ")\n";
                    //srcRngAndComplScriptAB += "setLimitI(" + clamp.ToString() + ")\n";
                    //srcRngAndComplScriptAB += "setBLimitI(" + clamp.ToString() + ")\n";
                }
            }
        }
        
        private static void SetPMDTMsrtDC(K2600ScriptSetting param, ref string msrtScriptA, ref string msrtScriptB, ref string msrtScriptAB)
        {
            string srcFunc = string.Empty;
            string msrtFunc = string.Empty;
            int srcMode = GetSrcMsrtMode(param.SMUA.SrcMode, out srcFunc, out msrtFunc);

            if (param.SMUA.IsEnableMsrt)
            {
                msrtScriptA += "print(mrtA." + msrtFunc + "())\n";

                msrtScriptB += "print(mrtB." + msrtFunc + "())\n";

                msrtScriptAB += string.Format("mrt{0}syncA(bufA1)\n", msrtFunc.ToUpper());
                msrtScriptAB += string.Format("mrt{0}syncB(bufB1)\n", msrtFunc.ToUpper());

                msrtScriptAB += "waitcomplete()\n";
                msrtScriptAB += "printbuffer(1, 1, bufA1, bufB1)\n";
                msrtScriptAB += "bufA1.clear()\n";
                msrtScriptAB += "bufB1.clear()\n";
            }
        }


        public static string LOP_DUAL(K2600ScriptSetting param, bool isHwTrigger)
        {
            string script = string.Empty;

            string srcRngAndComplScript = string.Empty;

            string srcFunc = string.Empty;

            string msrtFunc = string.Empty;

            int srcMode = GetSrcMsrtMode(param.SMUA.SrcMode, out srcFunc, out msrtFunc);    // [0] I Source;  [1] V Source

            double forceRange = param.SMUA.SrcRange;

            // ===========================================================================
            // Source Range < 1 uA, SMU 會有 20~30 ms 的前置時間, 才會打出訊號
            // ===========================================================================
            if (srcFunc == "i")
            {
                if (forceRange < 0.000001)
                {
                    forceRange = 0.000001;
                }
            }
            else
            {
                if (forceRange < 2.0d)
                {
                    forceRange = 2.0d;
                }
            }

            // ===========================================================================
            // Fix SRC Current Range 利用 100uA Force Range 去量測 <100uA的Level
            // 降低under-shoot的產生
            // 高電壓檔位的量測，使用100uA的檔位去推動10uA，Rising會有問題產生
            // ===========================================================================

            if (param.SMUA.MsrtNPLC == 0.01d && param.SMUA.MsrtClamp < 20)
            {
                if (forceRange < 0.0001)
                {
                    forceRange = 0.0001;
                }
            }

            if (param.SMUA.MsrtClamp <= param.SMUA.MsrtBoundryLimit)
            {
                if (msrtFunc == "v")
                {
                    srcRngAndComplScript += "setLimitV(" + param.SMUA.MsrtClamp.ToString() + ")\n";

                    srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    srcRngAndComplScript += "setLimitI(" + param.SMUA.MsrtClamp.ToString() + ")\n";

                    srcRngAndComplScript += "setSorceRangeV(" + forceRange.ToString() + ")\n";
                }
            }
            else
            {
                if (msrtFunc == "v")
                {
                    srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";

                    srcRngAndComplScript += "setLimitV(" + param.SMUA.MsrtClamp.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    srcRngAndComplScript += "setSorceRangeV(" + forceRange.ToString() + ")\n";

                    srcRngAndComplScript += "setLimitI(" + param.SMUA.MsrtClamp.ToString() + ")\n";
                }
            }

            //-----------------------------------------------------------------------------------------
            // Test Sequence
            //-----------------------------------------------------------------------------------------
            script = "loadscript num_" + param.Index.ToString() + "\n";

            script += "setNPLC(" + param.SMUA.MsrtNPLC.ToString() + ")\n";

            script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";

            if (msrtFunc == "v")
            {
                script += "setMeasureRangeV(" + param.SMUA.MsrtRange.ToString() + ")\n";
            }
            else if (msrtFunc == "i")
            {
                script += "setMeasureRangeI(" + param.SMUA.MsrtRange.ToString() + ")\n";
            }

            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            script += srcRngAndComplScript;

            if (param.SMUA.IsEnableSmu && param.SMUB.IsEnableSmu)
            {
                // SMUB -> Detector Channel
                double srcRangeSmuB = param.SMUB.SrcRange;

                if (srcRangeSmuB < 2.0d)  // 用太小的電壓位推, 可能會有問題
                {
                    srcRangeSmuB = 2.0d;
                }

                script += "if getBFunc() ~= 1 then setBFunc(1) end\n";

                script += "setBNPLC(" + param.SMUB.MsrtNPLC.ToString() + ")\n";

                script += "setBLimitI(" + param.SMUB.MsrtClamp.ToString() + ")\n";

                script += "setBSorceRangeV(" + srcRangeSmuB.ToString() + ")\n";

                script += "setBMeasureRangeI(" + param.SMUB.MsrtRange.ToString() + ")\n";

                script += "setBLevelV(" + param.SMUB.SrcLevel.ToString() + ")\n";

                script += "setBOutput(1)\n";
            }
            else if (param.SMUA.IsEnableSmu && !param.SMUB.IsEnableSmu && isHwTrigger)
            {
                script += "digio.trigger[" + K2600Const.IO_SMU_TRIG_IN.ToString() + "].wait(0.2)\n";  // Time Out 200ms
            }

            //======================================
            // 切完Range後，在Set Range & Compliance
            //=======================================
            if (param.SMUA.WaitTime > 0)
            {
                script += "delay(" + param.SMUA.WaitTime.ToString() + ")\n";
            }

            if (srcFunc == "v")
            {
                script += "setLevelV(" + param.SMUA.SrcLevel.ToString() + ")\n";
            }
            else if (srcFunc == "i")
            {
                script += "setLevelI(" + param.SMUA.SrcLevel.ToString() + ")\n";
            }

            script += "delay(" + param.SMUA.srcTime.ToString() + ")\n";

            //======================================
            // 回傳量測值
            //=======================================
            if (param.SMUB.IsEnableSmu)
            {
                // script += "local reading = string.format(\"%e, %e\", mrtA." + msrtFunc + "(), mrtB.i())\n";

                script += "mrtVsyncA(bufA1)\n";  // overlappedv

                script += "mrtIsyncB(bufB1)\n";  // overlappedi

                script += "waitcomplete()\n";
            }
            else
            {
                if (isHwTrigger)
                {
                    script += "digio.trigger[" + K2600Const.IO_PM_RELAY1.ToString() + "].assert()\n";

                    //script += "print(mrtA." + msrtFunc + "())\n";

                    script += "local reading = mrtA." + msrtFunc + "()\n";

                    script += "digio.trigger[" + K2600Const.IO_SMU_TRIG_IN.ToString() + "].wait(5)\n";  // Time Out 100ms
                }
                else
                {
                    script += "local reading = mrtA." + msrtFunc + "()\n";
                }
            }

            //-----------------------------------------------------------------------------------------
            // 若打IZ量VZ的Clamp電壓大於20，打完IZ後先降Clamp，以免多花20mS時間
            //-----------------------------------------------------------------------------------------
            if (param.SMUA.MsrtClamp > 20 && srcFunc == "i" && param.SMUA.IsAutoTurnOff)
            {
                script += "setLimitV(8)\n";
            }

            bool isForceDischarge = true;

            if (param.SMUA.IsAutoTurnOff)
            {
                if (!isForceDischarge)
                {
                    if (srcFunc == "v")
                    {
                        script += "setLevelV(0)\n";
                    }
                    else if (srcFunc == "i")
                    {
                        script += "setLevelI(0)\n";

                        if (param.IsTurnOffToDefaultRange)
                        {
                            script += "setSorceRangeI(0.01)\n";
                        }
                        else
                        {
                            if (forceRange >= 0.1)
                            {
                                // 防止切換 1A Range 時, 產生 Overshoot
                                script += "setSorceRangeI(0.01)\n";
                            }
                        }
                    }
                }
                else
                {
                    if (srcFunc == "v")
                    {
                        script += "setLevelV(0)\n";
                    }
                    else if (srcFunc == "i")
                    {
                        script += "setLevelI(0)\n";

                        script += "setFunc(1)\n";

                        script += "setLevelV(0)\n";

                        if (param.IsTurnOffToDefaultRange)
                        {
                            script += "setSorceRangeI(0.01)\n";
                        }
                        else
                        {
                            if (forceRange >= 0.1)
                            {
                                // 防止切換 1A Range 時, 產生 Overshoot
                                script += "setSorceRangeI(0.01)\n";
                            }
                        }
                    }
                }
            }

            if (param.SMUB.IsEnableSmu)
            {
                script += "setBLevelV(0)\n";
                script += "setBOutput(0)\n";
                script += "printbuffer(1, bufA1.n, bufA1, bufB1)\n";
                script += "bufA1.clear()\n";
                script += "bufA2.clear()\n";
                script += "bufB1.clear()\n";
            }
            else
            {
                script += "print(reading)\n";
            }

            script += "endscript\n";

            script += "num_" + param.Index.ToString() + ".source = nil";

            return script;
        }

        /// <summary>
        /// 支援雙 Detector-CH 量測
        /// </summary>
        public static string LOP_SLAVE(K2600ScriptSetting param, bool isHwTrigger)
        {
            string script = string.Empty;

            int srcMode = 1;    // [0] I Source;  [1] V Source

            string srcFunc = "v";

            string msrtFunc = "i";

            // ===========================================================================
            // Source Range < 1 uA, SMU 會有 20~30 ms 的前置時間, 才會打出訊號
            // ===========================================================================
            double forceRangeA = Math.Abs(param.SMUA.SrcLevel);

            double msrtRangeA = Math.Abs(param.SMUA.MsrtRange);

            if (forceRangeA < 2.0d)
            {
                forceRangeA = 2.0d;
            }

            double forceRangeB = Math.Abs(param.SMUB.SrcLevel);

            double msrtRangeB = Math.Abs(param.SMUB.SrcLevel);

            if (forceRangeB < 2.0d)
            {
                forceRangeB = 2.0d;
            }

            //-----------------------------------------------------------------------------------------------------------------------------
            if (isHwTrigger)
            {
                #region >>> H/W Trig <<<

                script = "loadscript num_" + param.Index.ToString() + "\n";

                if (param.SMUA.IsEnableSmu && param.SMUB.IsEnableSmu)
                {
                    script += "setSorceRangeV(" + forceRangeA.ToString() + ")\n";
                    script += "setBSorceRangeV(" + forceRangeB.ToString() + ")\n";

                    script += "setLimitI(" + msrtRangeA.ToString() + ")\n";
                    script += "setBLimitI(" + msrtRangeB.ToString() + ")\n";

                    script += "setNPLC(" + param.SMUA.MsrtNPLC.ToString() + ")\n";
                    script += "setBNPLC(" + param.SMUB.MsrtNPLC.ToString() + ")\n";

                    script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";
                    script += "if getBFunc() ~= " + srcMode.ToString() + " then setBFunc(" + srcMode.ToString() + ") end\n";

                    script += "setMeasureRangeI(" + msrtRangeA.ToString() + ")\n";
                    script += "setBMeasureRangeI(" + msrtRangeB.ToString() + ")\n";

                    script += "if getOutput() ~= 1 then setOutput(1) end\n";
                    script += "if getBOutput() ~= 1 then setBOutput(1) end\n";

                    script += "digio.trigger[" + K2600Const.IO_SMU_TRIG_IN.ToString() + "].assert()\n";  // 為了同步 先用 Trigger In 訊號讓兩個 Script 同步

                    script += "setLevelV(" + param.SMUA.SrcLevel.ToString() + ")\n";
                    script += "setBLevelV(" + param.SMUB.SrcLevel.ToString() + ")\n";

                    script += "digio.trigger[" + K2600Const.IO_PM_RELAY1.ToString() + "].wait(5)\n";

                    //-------------------------------------------------------------------------------------------------
                    // SMU Msrt
                    script += "mrtIsyncA(bufA1)\n";  // overlappedi
                    script += "mrtIsyncB(bufB1)\n";  // overlappedi
                    script += "waitcomplete()\n";

                    script += "digio.trigger[" + K2600Const.IO_SMU_TRIG_IN.ToString() + "].assert()\n";
                    //-------------------------------------------------------------------------------------------------

                    script += "printbuffer(1, 1, bufA1, bufB1)\n";

                    script += "setOutput(0)\n";
                    script += "setBOutput(0)\n";

                    script += "bufA1.clear()\n";
                    script += "bufB1.clear()\n";
                }
                else if (param.SMUB.IsEnableSmu)
                {
                    script += "setBSorceRangeV(" + forceRangeB.ToString() + ")\n";

                    script += "setBLimitI(" + msrtRangeB.ToString() + ")\n";

                    script += "setBNPLC(" + param.SMUB.MsrtNPLC.ToString() + ")\n";

                    script += "if getBFunc() ~= " + srcMode.ToString() + " then setBFunc(" + srcMode.ToString() + ") end\n";

                    script += "setBMeasureRangeI(" + msrtRangeB.ToString() + ")\n";

                    script += "if getBOutput() ~= 1 then setBOutput(1) end\n";

                    script += "digio.trigger[" + K2600Const.IO_SMU_TRIG_IN.ToString() + "].assert()\n";  // 為了同步 先用 Trigger In 訊號讓兩個 Script 同步

                    script += "setBLevelV(" + param.SMUB.SrcLevel.ToString() + ")\n";

                    script += "digio.trigger[" + K2600Const.IO_PM_RELAY1.ToString() + "].wait(5)\n";

                    //-------------------------------------------------------------------------------------------------
                    // SMU Msrt
                    script += "local reading = mrtB." + msrtFunc + "()\n";

                    script += "digio.trigger[" + K2600Const.IO_SMU_TRIG_IN.ToString() + "].assert()\n";
                    //-------------------------------------------------------------------------------------------------

                    script += "setBOutput(0)\n";

                    script += "print(reading)\n";
                }
                else if (param.SMUA.IsEnableSmu)
                {
                    script += "setSorceRangeV(" + forceRangeA.ToString() + ")\n";

                    script += "setLimitI(" + msrtRangeA.ToString() + ")\n";

                    script += "setNPLC(" + param.SMUA.MsrtNPLC.ToString() + ")\n";

                    script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";

                    script += "setMeasureRangeI(" + msrtRangeA.ToString() + ")\n";

                    script += "if getOutput() ~= 1 then setOutput(1) end\n";

                    script += "digio.trigger[" + K2600Const.IO_SMU_TRIG_IN.ToString() + "].assert()\n";  // 為了同步 先用 Trigger In 訊號讓兩個 Script 同步

                    script += "setLevelV(" + param.SMUA.SrcLevel.ToString() + ")\n";

                    script += "digio.trigger[" + K2600Const.IO_PM_RELAY1.ToString() + "].wait(5)\n";

                    //-------------------------------------------------------------------------------------------------
                    // SMU Msrt
                    script += "local reading = mrtA." + msrtFunc + "()\n";

                    script += "digio.trigger[" + K2600Const.IO_SMU_TRIG_IN.ToString() + "].assert()\n";
                    //-------------------------------------------------------------------------------------------------

                    script += "setOutput(0)\n";

                    script += "print(reading)\n";
                }

                script += "endscript\n";

                return script;

                #endregion
            }
            else
            {
                #region >>> S/W Trig <<<

                script = "loadscript num_" + param.Index.ToString() + "\n";

                if (param.SMUA.IsEnableSmu && param.SMUB.IsEnableSmu)
                {
                    script += "setNPLC(" + param.SMUA.MsrtNPLC.ToString() + ")\n";
                    script += "setBNPLC(" + param.SMUB.MsrtNPLC.ToString() + ")\n";

                    script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";
                    script += "if getBFunc() ~= " + srcMode.ToString() + " then setBFunc(" + srcMode.ToString() + ") end\n";

                    script += "setMeasureRangeI(" + msrtRangeA.ToString() + ")\n";
                    script += "setBMeasureRangeI(" + msrtRangeB.ToString() + ")\n";

                    //=============================
                    // 切完Range後，在Set Range & Compliance
                    //=============================
                    script += "if getOutput() ~= 1 then setOutput(1) end\n";
                    script += "if getBOutput() ~= 1 then setBOutput(1) end\n";

                    script += "setLimitI(" + msrtRangeA.ToString() + ")\n";
                    script += "setBLimitI(" + msrtRangeB.ToString() + ")\n";

                    script += "setSorceRangeV(" + forceRangeA.ToString() + ")\n";
                    script += "setBSorceRangeV(" + forceRangeB.ToString() + ")\n";

                    script += "setLevelV(" + param.SMUA.SrcLevel.ToString() + ")\n";
                    script += "setBLevelV(" + param.SMUB.SrcLevel.ToString() + ")\n";
                    //script += "delay(" + item.ForceTime.ToString() + ")\n";

                    //script += "mrtIsyncA(bufA1)\n";  // overlappedi
                    //script += "mrtIsyncB(bufB1)\n";  // overlappedi

                    //script += "waitcomplete()\n";

                    //// script += "setLevelV(0)\n";
                    //// script += "setBLevelV(0)\n";

                    //script += "printbuffer(1, 1, bufA1, bufB1)\n";

                    //script += "setOutput(0)\n";
                    //script += "setBOutput(0)\n";

                    //script += "bufA1.clear()\n";
                    //script += "bufB1.clear()\n";
                }
                else if (param.SMUB.IsEnableSmu)
                {
                    script += "setBNPLC(" + param.SMUB.MsrtNPLC.ToString() + ")\n";

                    script += "if getBFunc() ~= " + srcMode.ToString() + " then setBFunc(" + srcMode.ToString() + ") end\n";

                    script += "setBMeasureRangeI(" + msrtRangeB.ToString() + ")\n";

                    //=============================
                    // 切完Range後，在Set Range & Compliance
                    //=============================
                    script += "if getBOutput() ~= 1 then setBOutput(1) end\n";

                    script += "setBLimitI(" + msrtRangeB.ToString() + ")\n";

                    script += "setBSorceRangeV(" + forceRangeB.ToString() + ")\n";

                    script += "setBLevelV(" + param.SMUB.SrcLevel.ToString() + ")\n";

                    ////script += "delay(" + item.ForceTime.ToString() + ")\n";

                    //script += "print(mrtB." + msrtFunc + "())\n";

                    //// script += "setLevelV(0)\n";

                    //script += "setBOutput(0)\n";
                }
                else if (param.SMUA.IsEnableSmu)
                {
                    script += "setNPLC(" + param.SMUA.MsrtNPLC.ToString() + ")\n";

                    script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";

                    script += "setMeasureRangeI(" + msrtRangeA.ToString() + ")\n";

                    //=============================
                    // 切完Range後，在Set Range & Compliance
                    //=============================
                    script += "if getOutput() ~= 1 then setOutput(1) end\n";

                    script += "setLimitI(" + msrtRangeA.ToString() + ")\n";

                    script += "setSorceRangeV(" + forceRangeA.ToString() + ")\n";

                    script += "setLevelV(" + param.SMUA.SrcLevel.ToString() + ")\n";

                    ////script += "delay(" + item.ForceTime.ToString() + ")\n";

                    //script += "print(mrtA." + msrtFunc + "())\n";

                    //// script += "setLevelV(0)\n";

                    //script += "setOutput(0)\n";
                }


                script += "endscript\n";

                script += "num_" + param.Index.ToString() + ".source = nil";

                return script;


                #endregion
            }
        }

        #endregion

        #region >>> THY Script <<<

        public static string THY_PMDT_DUAL(K2600ScriptSetting param)
        {

            param.SMUA.SweepPoints = (uint)Math.Round((double)param.SMUA.SweepPoints / 100.0d) * 100;

            string srcFunc = string.Empty;
            string msrtFunc = string.Empty;

            // [0] I Source;  [1] V Source
            int srcMode = GetSrcMsrtMode(param.SMUA.SrcMode, out srcFunc, out msrtFunc);//[0] I Source;  [1] V Source
            bool isAutoTurnOff = param.SMUA.IsAutoTurnOff;

            double nplc = param.SMUA.MsrtNPLC;

            uint index = param.Index;
            string keyName = param.KeyName;
            double waitTime = param.SMUA.WaitTime;
            double forceTime = param.SMUA.srcTime;

            double forceValue = param.SMUA.SrcLevel;

            double forceRange = param.SMUA.SrcRange;
            double clamp = param.SMUA.MsrtClamp;

            string script = string.Empty;

            string srcRngAndComplScript = string.Empty;


            //srcFunc = "i";

            //msrtFunc = "v";
            ///////////////////////////////////////////////////////////////////////////////////////////////
            // NPLC
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string NPLCScriptA = string.Empty;
            string NPLCScriptB = string.Empty;
            string NPLCScriptAB = string.Empty;

            SetPMDTNPLC(param, ref NPLCScriptA, ref NPLCScriptB, ref NPLCScriptAB);
            ///////////////////////////////////////////////////////////////////////////////////////////////
            // SrcMode
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string srcModeScriptA = string.Empty;
            string srcModeScriptB = string.Empty;
            string srcModeScriptAB = string.Empty;

            SetSrcMode(param, ref  srcModeScriptA, ref  srcModeScriptB, ref  srcModeScriptAB);
            ///////////////////////////////////////////////////////////////////////////////////////////////
            // Force Range and Compliance Setting
            ///////////////////////////////////////////////////////////////////////////////////////////////

            string srcRngAndComplScriptA = string.Empty;
            string srcRngAndComplScriptB = string.Empty;
            string srcRngAndComplScriptAB = string.Empty;

            SetPMDTSrcRange(param, ref srcRngAndComplScriptA, ref srcRngAndComplScriptB, ref srcRngAndComplScriptAB, ref forceRange);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            // Range selected
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string msrtRangeScriptA = string.Empty;
            string msrtRangeScriptB = string.Empty;
            string msrtRangeScriptAB = string.Empty;

            SetPMDTMsrtRange(param, ref msrtRangeScriptA, ref msrtRangeScriptB, ref msrtRangeScriptAB);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            // Source Off
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string srcOffScriptA = string.Empty;
            string srcOffScriptB = string.Empty;
            string srcOffScriptAB = string.Empty;

            SetPMDTTurnOff(param, forceRange, ref srcOffScriptA, ref srcOffScriptB, ref srcOffScriptAB);
            ///////////////////////////////////////////////////////////////////////////////////////////////
            // source output
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string srcScriptA = string.Empty;
            string srcScriptB = string.Empty;
            string srcScriptAB = string.Empty;

            SetPMDTDC(param, ref srcScriptA, ref srcScriptB, ref srcScriptAB);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            // Msrt
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string msrtScriptA = string.Empty;
            string msrtScriptB = string.Empty;
            string msrtScriptAB = string.Empty;

            SetPMDTMsrtTHY(param, ref msrtScriptA, ref msrtScriptB, ref msrtScriptAB);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            // MsrtCnt
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string msrtCntScriptA = string.Empty;
            string msrtCntScriptB = string.Empty;
            string msrtCntScriptAB = string.Empty;

            SetPMDTMsrtTHYCount(param, ref msrtCntScriptA, ref  msrtCntScriptB, ref  msrtCntScriptAB);


            ///////////////////////////////////////////////////////////////////////////////////////////////
            // CalcTHY
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string calcScriptA = string.Empty;
            string calcScriptB = string.Empty;
            string calcScriptAB = string.Empty;
            SetPMDTCalcTHY(param, ref  calcScriptA, ref  calcScriptB, ref  calcScriptAB);


            //////////////////////////////////////////////////////////////////////////////////////////////////////
            // Test Sequence
            //////////////////////////////////////////////////////////////////////////////////////////////////////
            script = "loadscript num_" + index.ToString() + "\n";

            //--------------------------------------------------------------------------------------------------
            script += "if channel == 1 then\n"; // SMUA

            //script += "setNPLC(" + nplc.ToString() + ")\n";
            script += NPLCScriptA;
            //script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";
            script += srcModeScriptA;

            script += msrtCntScriptA;

            script += msrtRangeScriptA;

            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            script += srcRngAndComplScriptA;

            if (waitTime > 0)
            {
                script += "delay(" + waitTime.ToString() + ")\n";
            }

            script += srcScriptA;

            script += "delay(" + forceTime.ToString() + ")\n";

            script += msrtScriptA;

            script += calcScriptA;

            script += srcOffScriptA;

            //--------------------------------------------------------------------------------------------------
            script += "elseif channel == 2 then\n"; // SMUB

            //script += "setBNPLC(" + nplc.ToString() + ")\n";
            //script += "if getBFunc() ~= " + srcMode.ToString() + " then setBFunc(" + srcMode.ToString() + ") end\n";
            script += NPLCScriptB;

            script += srcModeScriptB;

            script += msrtCntScriptB;

            script += msrtRangeScriptB;

            script += "if getBOutput() ~= 1 then setBOutput(1) end\n";

            script += srcRngAndComplScriptB;

            if (waitTime > 0)
            {
                script += "delay(" + waitTime.ToString() + ")\n";
            }

            script += srcScriptB;

            script += "delay(" + forceTime.ToString() + ")\n";

            script += msrtScriptB;

            script += calcScriptB;

            script += srcOffScriptB;
            //--------------------------------------------------------------------------------------------------
            script += "else\n"; // Both

            //script += "setNPLC(" + nplc.ToString() + ")\n";
            //script += "setBNPLC(" + nplc.ToString() + ")\n";

            //script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";
            //script += "if getBFunc() ~= " + srcMode.ToString() + " then setBFunc(" + srcMode.ToString() + ") end\n";
            script += NPLCScriptAB;

            script += srcModeScriptAB;

            script += msrtCntScriptAB;

            script += msrtRangeScriptAB;

            script += "if getOutput() ~= 1 then setOutput(1) end\n";
            script += "if getBOutput() ~= 1 then setBOutput(1) end\n";

            script += srcRngAndComplScriptAB;

            if (waitTime > 0)
            {
                script += "delay(" + waitTime.ToString() + ")\n";
            }

            script += srcScriptAB;

            script += "delay(" + forceTime.ToString() + ")\n";

            script += msrtScriptAB;

            script += calcScriptAB;

            script += srcOffScriptAB;

            script += "end\n"; // if
            //--------------------------------------------------------------------------------------------------

            script += "channel = nil\n";
            script += "endscript\n";

            script += "num_" + param.Index.ToString() + ".source = nil";

            //////////////////////////////////////////////////////
            if (true)
            {
                string fileName = "MPI\\LEDTester\\Log\\ScripLog\\" + "PMDT_THY_" + index;
                ExportScriptToTxt(script, fileName);//20170628 David 紀錄Scrip
            }
            /////////////////////////////////////////////////////////////////////

            return script;

        }

        private static void SetPMDTMsrtTHYCount(K2600ScriptSetting param, ref string msrtCntScriptA, ref string msrtCntScriptB, ref string msrtCntScriptAB)
        {
            uint measureCount = (param.SMUA.SweepPoints) / 2;

            if (measureCount < 100)
            {
                measureCount = 100;
            }

            msrtCntScriptA += "setMsrtCount(" + measureCount.ToString() + ")\n";

            msrtCntScriptA += "setMsrtInterval(0.000001)\n";

            msrtCntScriptB += "setBMsrtCount(" + measureCount.ToString() + ")\n";

            msrtCntScriptB += "setBMsrtInterval(0.000001)\n";

            msrtCntScriptAB += "setMsrtCount(" + measureCount.ToString() + ")\n setBMsrtCount(" + measureCount.ToString() + ")\n";

            msrtCntScriptAB += "setMsrtInterval(0.000001)\n setBMsrtInterval(0.000001)\n";

        }

        private static void SetPMDTMsrtTHY(K2600ScriptSetting param, ref string msrtScriptA, ref string msrtScriptB, ref string msrtScriptAB)
        {
            string srcFunc = string.Empty;
            string msrtFunc = string.Empty;
            int srcMode = GetSrcMsrtMode(param.SMUA.SrcMode, out srcFunc, out msrtFunc);

            if (param.SMUA.IsEnableMsrt)
            {
                //msrtScriptA += "print(mrtA." + msrtFunc + "())\n";
                msrtScriptA += "mrtA." + msrtFunc + "(bufA1)\n";

                msrtScriptB += "mrtB." + msrtFunc + "(bufB1)\n";

                msrtScriptAB += "mrtA." + msrtFunc + "(bufA1)\n";
                msrtScriptAB += "mrtB." + msrtFunc + "(bufB1)\n";
            }

        }

        private static void SetPMDTCalcTHY(K2600ScriptSetting param, ref string calcScriptA, ref string calcScriptB, ref string calcScriptAB)
        {
            string srcFunc = string.Empty;
            string msrtFunc = string.Empty;
            int srcMode = GetSrcMsrtMode(param.SMUA.SrcMode, out srcFunc, out msrtFunc);

            uint measureCount = (param.SMUA.SweepPoints) / 2;

            if (measureCount < 100)
            {
                measureCount = 100;
            }

            if (param.IsSpFwCalcResult)
            {
                calcScriptA += "local maxPeakA = 0\n";

                calcScriptA += "local stableSumA = 0\n";

                calcScriptA += "for i = 1, " + measureCount.ToString() + " do\n";

                calcScriptA += "if bufA1.readings[i] > maxPeakA then\n";

                calcScriptA += "maxPeakA = bufA1.readings[i]\n";

                calcScriptA += "end\n";

                calcScriptA += "if " + measureCount.ToString() + " - i < 20 then\n";

                calcScriptA += "stableSumA = stableSumA + bufA1.readings[i]\n";

                calcScriptA += "end\n";

                calcScriptA += "end\n";

                calcScriptA += "local stableValueA = stableSumA / 20\n";

                calcScriptA += "local maxToStableA = maxPeakA - stableValueA\n";

                calcScriptA += "local OutpA = string.format(\"%.3f, %.3f\", maxPeakA, stableValueA)\n";



                calcScriptB += "local maxPeakB = 0\n";

                calcScriptB += "local stableSumB = 0\n";

                calcScriptB += "for i = 1, " + measureCount.ToString() + " do\n";

                calcScriptB += "if bufB1.readings[i] > maxPeakB then\n";

                calcScriptB += "maxPeakB = bufB1.readings[i]\n";

                calcScriptB += "end\n";

                calcScriptB += "if " + measureCount.ToString() + " - i < 20 then\n";

                calcScriptB += "stableSumB = stableSumB + bufB1.readings[i]\n";

                calcScriptB += "end\n";

                calcScriptB += "end\n";

                calcScriptB += "local stableValueB = stableSumB / 20\n";

                calcScriptB += "local maxToStableB = maxPeakB - stableValueB\n";

                calcScriptB += "local OutpB = string.format(\"%.3f, %.3f\", maxPeakB, stableValueB)\n";


                //calcScriptAB += calcScriptA;

                //calcScriptAB += calcScriptB;

                calcScriptAB += "local maxPeakB = 0\n";

                calcScriptAB += "local stableSumB = 0\n";

                calcScriptAB += "local maxPeakA = 0\n";

                calcScriptAB += "local stableSumA = 0\n";

                calcScriptAB += "for i = 1, " + measureCount.ToString() + " do\n";

                calcScriptAB += "if bufA1.readings[i] > maxPeakA then\n";

                calcScriptAB += "maxPeakA = bufA1.readings[i]\n";

                calcScriptAB += "end\n";

                calcScriptAB += "if bufB1.readings[i] > maxPeakB then\n";

                calcScriptAB += "maxPeakB = bufB1.readings[i]\n";

                calcScriptAB += "end\n";

                calcScriptAB += "if " + measureCount.ToString() + " - i < 20 then\n";

                calcScriptAB += "stableSumA = stableSumA + bufA1.readings[i]\n";

                calcScriptAB += "end\n";

                calcScriptAB += "if " + measureCount.ToString() + " - i < 20 then\n";

                calcScriptAB += "stableSumB = stableSumB + bufB1.readings[i]\n";

                calcScriptAB += "end\n";

                calcScriptAB += "end\n";

                calcScriptAB += "local stableValueA = stableSumA / 20\n";

                calcScriptAB += "local maxToStableA = maxPeakA - stableValueA\n";

                calcScriptAB += "local stableValueB = stableSumB / 20\n";

                calcScriptAB += "local maxToStableB = maxPeakB - stableValueB\n";

                calcScriptAB += "local OutpAB = string.format(\"%.3f, %.3f,%.3f, %.3f\", maxPeakA, stableValueA, maxPeakB, stableValueB)\n";




                calcScriptA += "print(OutpA)\n";

                calcScriptB += "print(OutpB)\n";

                calcScriptAB += "print(OutpAB)\n";

            }
            else
            {
                calcScriptA += "printbuffer(1, " + measureCount.ToString() + ", bufA1)\n";

                calcScriptB += "printbuffer(1, " + measureCount.ToString() + ", bufB1)\n";

                calcScriptAB += string.Format("mrt{0}syncA(bufA1)\n", msrtFunc.ToUpper());
                calcScriptAB += string.Format("mrt{0}syncB(bufB1)\n", msrtFunc.ToUpper());

                calcScriptAB += "waitcomplete()\n";
                calcScriptAB += "printbuffer(1," + measureCount.ToString() + " , bufA1, bufB1)\n";
            }

            calcScriptA += "setMsrtCount(1)\n bufA1.clear()\n";

            calcScriptA += "setBMsrtCount(1)\n bufB1.clear()\n";

            calcScriptAB += "setMsrtCount(1)\n setBMsrtCount(1)\n";

            calcScriptAB += "bufA1.clear()\n bufB1.clear()\n";

        }

        public static string THY_SrcOnly_PMDT_DUAL(K2600ScriptSetting param)
        {
            string script = string.Empty;

            double forceRange = param.SMUA.SrcRange;
            ///////////////////////////////////////////////////////////////////////////////////////////////
            // SrcMode1
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string srcMode1ScriptA = string.Empty;
            string srcMode1ScriptB = string.Empty;
            string srcMode1ScriptAB = string.Empty;

            SetSrcMode(param, ref  srcMode1ScriptA, ref  srcMode1ScriptB, ref  srcMode1ScriptAB, 1);
            ///////////////////////////////////////////////////////////////////////////////////////////////
            // SrcMode2
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string srcMode2ScriptA = string.Empty;
            string srcMode2ScriptB = string.Empty;
            string srcMode2ScriptAB = string.Empty;

            SetSrcMode(param, ref  srcMode2ScriptA, ref  srcMode2ScriptB, ref  srcMode2ScriptAB, 0);
            ///////////////////////////////////////////////////////////////////////////////////////////////
            // Force Range and Compliance Setting
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string srcRngAndComplScriptA = string.Empty;
            string srcRngAndComplScriptB = string.Empty;
            string srcRngAndComplScriptAB = string.Empty;

            SetPMDTSrcRange(param, ref srcRngAndComplScriptA, ref srcRngAndComplScriptB, ref srcRngAndComplScriptAB, ref forceRange);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            // Force Range and Compliance Setting
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string src1RangScriptA = string.Empty;
            string src1RangScriptB = string.Empty;
            string src1RangScriptAB = string.Empty;

            string strAr = setSorceRangeV(param, 0, "A");
            string strBr = setSorceRangeV(param, 0, "B");

            srcRngAndComplScriptA += strAr ;
            srcRngAndComplScriptB += strBr ;
            srcRngAndComplScriptAB += strAr + strBr ;

            //SetPMDTSrcRange(param, ref src1RangScriptA, ref src1RangScriptB, ref src1RangScriptAB, ref sRange,"v","i");



          

            ///////////////////////////////////////////////////////////////////////////////////////////////
            // Source Off
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string srcOffScriptA = string.Empty;
            string srcOffScriptB = string.Empty;
            string srcOffScriptAB = string.Empty;

            SetPMDTTurnOff(param, forceRange, ref srcOffScriptA, ref srcOffScriptB, ref srcOffScriptAB);
            ///////////////////////////////////////////////////////////////////////////////////////////////
            // IO Scrop1
            ///////////////////////////////////////////////////////////////////////////////////////////////

            #region >>io script1<<

            string IOScript1 = string.Empty;

            // Enable二極體:		PIN_RTH_EANBLE = 0  
            // Bypass 二極體:		PIN_RTH_EANBLE = 1
            // P:				_pinDAQEnable = 1, PIN_RTH_EANBLE = 0
            // N:				_pinDAQEnable = 0, PIN_RTH_EANBLE = 1
            // Open:			_pinDAQEnable = 0, PIN_RTH_EANBLE = 0
            //小電容:				PIN_CAP_SW = 0
            //大電容:				PIN_CAP_SW = 1

            // Bypass 二極體
            IOScript1 += "digio.writebit(" + K2600Const.IO_RTH_EANBLE.ToString() + ", 1)\n";

            // P極
            IOScript1 += "digio.writebit(" + K2600Const.IO_DAQ_ENABLE.ToString() + ", 1)\n";

            IOScript1 += "digio.writebit(" + K2600Const.IO_POLAR_SW.ToString() + ", 0)\n";

            //切換大小電容
            if (param.SMUA.SrcLevel >= 0.00001)
            {
                IOScript1 += "digio.writebit(" + K2600Const.IO_CAP_SW.ToString() + ", 1)\n";
            }
            else
            {
                IOScript1 += "digio.writebit(" + K2600Const.IO_CAP_SW.ToString() + ", 0)\n";
            }
            #endregion

            ///////////////////////////////////////////////////////////////////////////////////////////////
            // IO Scrop2
            ///////////////////////////////////////////////////////////////////////////////////////////////

            #region >>io script2<<

            string IOScript2 = string.Empty;

            IOScript2 += "digio.writebit(" + K2600Const.IO_RTH_EANBLE.ToString() + ", 1)\n";

            IOScript2 += "digio.writebit(" + K2600Const.IO_DAQ_ENABLE.ToString() + ", 0)\n";

            IOScript2 += "digio.writebit(" + K2600Const.IO_POLAR_SW.ToString() + ", 0)\n";

            #endregion

            //////////////////////////////////////////////////////////////////////////////////////////////////////
            // Test Sequence
            //////////////////////////////////////////////////////////////////////////////////////////////////////
            script = "loadscript num_" + param.Index.ToString() + "\n";

            //--------------------------------------------------------------------------------------------------
            script += "if channel == 1 then\n"; // SMUA

            //script += "if getFunc() ~= 1 then setFunc(1) end\n";
            script += srcMode1ScriptA;

            //script += "setSorceRangeV(0)\n";

            script += src1RangScriptA;

            script += "setLevelV(0)\n";

            script += "delay(0.005)\n";

            //script += "if getFunc() ~= 0 then setFunc(0) end\n";

            script += srcMode2ScriptA;

            script += IOScript1;

            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            script += srcRngAndComplScriptA;

            if (param.SMUA.WaitTime > 0)
            {
                script += "delay(" + param.SMUA.WaitTime.ToString() + ")\n";
            }

            script += "digio.trigger[" + K2600Const.IO_DAQ_TRIG_OUT.ToString() + "].assert()\n";

            script += "setLevelI(" + param.SMUA.SrcLevel.ToString() + ")\n";

            script += "delay(" + param.SMUA.srcTime.ToString() + ")" + "\n";

            script += "print(0)\n";

            script += srcOffScriptA;

            script += IOScript2;

            
            //--------------------------------------------------------------------------------------------------
            script += "elseif channel == 2 then\n"; // SMUB

            //script += "if getBFunc() ~= 1 then setBFunc(1) end\n";

            script += srcMode1ScriptB;

            //script += "setBSorceRangeV(0)\n";

            script += src1RangScriptB;

            script += "setBLevelV(0)\n";

            script += "delay(0.005)\n";

            //script += "if getBFunc() ~= 0 then setBFunc(0) end\n";

            script += srcMode2ScriptB;

            script += IOScript1;

            script += "if getBOutput() ~= 1 then setBOutput(1) end\n";

            script += srcRngAndComplScriptB;

            if (param.SMUA.WaitTime > 0)
            {
                script += "delay(" + param.SMUA.WaitTime.ToString() + ")\n";
            }

            script += "digio.trigger[" + K2600Const.IO_DAQ_TRIG_OUT.ToString() + "].assert()\n";

            script += "setBLevelI(" + param.SMUA.SrcLevel.ToString() + ")\n";

            script += "delay(" + param.SMUA.srcTime.ToString() + ")" + "\n";

            script += "print(0)\n";

            script += srcOffScriptB;

            script += IOScript2;

            
            //--------------------------------------------------------------------------------------------------
            script += "else\n"; // Both

            //script += "if getFunc() ~= 1 then setFunc(1) end\n";

            //script += "if getBFunc() ~= 1 then setBFunc(1) end\n";

            script += srcMode1ScriptAB;

            //script += "setSorceRangeV(0)\n";

            //script += "setBSorceRangeV(0)\n";

            script += src1RangScriptAB;

            script += "setLevelV(0)\n";

            script += "setBLevelV(0)\n";

            script += "delay(0.005)\n";//David 是否可以拿掉?

            //script += "if getFunc() ~= 0 then setFunc(0) end\n";

            //script += "if getBFunc() ~= 0 then setBFunc(0) end\n";

            script += srcMode2ScriptAB;

            script += IOScript1;

            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            script += "if getBOutput() ~= 1 then setBOutput(1) end\n";

            script += srcRngAndComplScriptAB;

            if (param.SMUA.WaitTime > 0)
            {
                script += "delay(" + param.SMUA.WaitTime.ToString() + ")\n";
            }

            script += "digio.trigger[" + K2600Const.IO_DAQ_TRIG_OUT.ToString() + "].assert()\n";

            script += "setLevelI(" + param.SMUA.SrcLevel.ToString() + ")\n";

            script += "setBLevelI(" + param.SMUA.SrcLevel.ToString() + ")\n";

            script += "delay(" + param.SMUA.srcTime.ToString() + ")" + "\n";

            script += "print(\"0,0\")\n";

            script += srcOffScriptAB;

            script += IOScript2;

            script += "end\n"; // if
            //--------------------------------------------------------------------------------------------------

            script += "channel = nil\n";
            script += "endscript\n";

            script += "num_" + param.Index.ToString() + ".source = nil";

            //////////////////////////////////////////////////////
            if (WRITE_SCRIPT)
            {
                string fileName = "MPI\\LEDTester\\Log\\ScripLog\\" + "THY_SrcOnly_PMDT_DUAL" + param.KeyName;
                ExportScriptToTxt(script, fileName);//20170628 David 紀錄Scrip
            }
            /////////////////////////////////////////////////////////////////////

            return script;
        }

        public static string THY_Trig_DAQ_PMDT_DUAL(K2600ScriptSetting param)
        {
            string script = string.Empty;

            double forceRange = param.SMUA.SrcRange;


            ///////////////////////////////////////////////////////////////////////////////////////////////
            // Force Range and Compliance Setting
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string srcRngAndComplScriptA = string.Empty;
            string srcRngAndComplScriptB = string.Empty;
            string srcRngAndComplScriptAB = string.Empty;

            SetPMDTSrcRange(param, ref srcRngAndComplScriptA, ref srcRngAndComplScriptB, ref srcRngAndComplScriptAB, ref forceRange);//srcFunc, msrtFunc, clampLimit, nplc, ref forceRange, clamp);



            ///////////////////////////////////////////////////////////////////////////////////////////////
            // SrcMode2
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string srcMode2ScriptA = string.Empty;
            string srcMode2ScriptB = string.Empty;
            string srcMode2ScriptAB = string.Empty;

            SetSrcMode(param, ref  srcMode2ScriptA, ref  srcMode2ScriptB, ref  srcMode2ScriptAB, 0);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            // Source Off
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string srcOffScriptA = string.Empty;
            string srcOffScriptB = string.Empty;
            string srcOffScriptAB = string.Empty;

            SetPMDTTurnOff(param, forceRange, ref srcOffScriptA, ref srcOffScriptB, ref srcOffScriptAB);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            // IO Scrop1
            ///////////////////////////////////////////////////////////////////////////////////////////////

            #region >>io script1<<

            string IOScript1 = string.Empty;

            IOScript1 += "digio.writebit(" + K2600Const.IO_FRAME_GROUND.ToString() + ", 1)\n";

            IOScript1 += "delay(0.003)\n";

            IOScript1 += "digio.writebit(" + K2600Const.IO_FRAME_GROUND.ToString() + ", 0)\n";


            // Enable二極體:		PIN_RTH_EANBLE = 0  
            // Bypass 二極體:		PIN_RTH_EANBLE = 1
            // P:				_pinDAQEnable = 1, PIN_RTH_EANBLE = 0
            // N:				_pinDAQEnable = 0, PIN_RTH_EANBLE = 1
            // Open:			_pinDAQEnable = 0, PIN_RTH_EANBLE = 0
            //小電容:				PIN_CAP_SW = 0
            //大電容:				PIN_CAP_SW = 1

            // Bypass 二極體
            IOScript1 += "digio.writebit(" + K2600Const.IO_RTH_EANBLE.ToString() + ", 1)\n";

            // P極
            IOScript1 += "digio.writebit(" + K2600Const.IO_DAQ_ENABLE.ToString() + ", 1)\n";

            IOScript1 += "digio.writebit(" + K2600Const.IO_RTH_EANBLE.ToString() + ", 0)\n";

            //切換大小電容
            if (param.SMUA.SrcLevel >= 0.00001)
            {
                IOScript1 += "digio.writebit(" + K2600Const.IO_CAP_SW.ToString() + ", 1)\n";
            }
            else
            {
                IOScript1 += "digio.writebit(" + K2600Const.IO_CAP_SW.ToString() + ", 0)\n";
            }
            #endregion
            ///////////////////////////////////////////////////////////////////////////////////////////////
            // IO Scrop2
            ///////////////////////////////////////////////////////////////////////////////////////////////

            #region >>io script2<<

            string IOScript2 = string.Empty;


            if (param.SMUA.WaitTime > 0)
            {
                IOScript2 += "delay(" + param.SMUA.WaitTime.ToString() + ")" + "\n";
            }

            IOScript2 += "delay(0.0003)\n";

            IOScript2 += "digio.trigger[" + K2600Const.IO_DAQ_ENABLE.ToString() + "].assert()\n";

            IOScript2 += "delay(0.0002)\n";

            IOScript2 += "delay(" + param.SMUA.srcTime.ToString() + ")" + "\n";

            #endregion

            ///////////////////////////////////////////////////////////////////////////////////////////////
            // IO Scrop3
            ///////////////////////////////////////////////////////////////////////////////////////////////

            #region >>io script3<<

            string IOScript3 = string.Empty;

            IOScript3 += "digio.writebit(" + K2600Const.IO_RTH_EANBLE.ToString() + ", 1)\n";

            IOScript3 += "digio.writebit(" + K2600Const.IO_DAQ_ENABLE.ToString() + ", 0)\n";

            IOScript3 += "digio.writebit(" + K2600Const.IO_POLAR_SW.ToString() + ", 0)\n";

            #endregion

            //////////////////////////////////////////////////////////////////////////////////////////////////////
            // Test Sequence
            //////////////////////////////////////////////////////////////////////////////////////////////////////
            script = "loadscript num_" + param.Index.ToString() + "_DAQTrigger\n";

            //--------------------------------------------------------------------------------------------------
            script += "if channel == 1 then\n"; // SMUA

            //script += "if getFunc() ~= 0 then setFunc(0) end\n";

            script += srcMode2ScriptA;

            script += IOScript1;

            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            script += srcRngAndComplScriptA;

            script += IOScript2;

            script += srcOffScriptA;

            script += "print(0)\n";

            script += IOScript3;

            //--------------------------------------------------------------------------------------------------
            script += "elseif channel == 2 then\n"; // SMUB

            //script += "if getBFunc() ~= 0 then setBFunc(0) end\n";

            script += srcMode2ScriptB;

            script += IOScript1;

            script += "if getBOutput() ~= 1 then setBOutput(1) end\n"; ;

            script += srcRngAndComplScriptB;

            script += IOScript2;

            script += srcOffScriptB;

            script += "print(0)\n";

            script += IOScript3;
            //--------------------------------------------------------------------------------------------------
            script += "else\n"; // Both

            //script += "if getFunc() ~= 0 then setFunc(0) end\n";

            //script += "if getBFunc() ~= 0 then setBFunc(0) end\n";

            script += srcMode2ScriptAB;

            script += IOScript1;

            script += "if getOutput() ~= 1 then setOutput(1) end\n"; ;

            script += "if getBOutput() ~= 1 then setBOutput(1) end\n"; ;

            script += srcRngAndComplScriptAB;

            script += IOScript2;

            script += srcOffScriptAB;

            script += IOScript3;

            script += "print(\"0,0\")\n";

            script += "end\n"; // if
            //--------------------------------------------------------------------------------------------------

            script += "channel = nil\n";
            script += "endscript\n";

            script += "num_" + param.Index.ToString() + "_DAQTrigger.source = nil";


            //////////////////////////////////////////////////////
            if (WRITE_SCRIPT)
            {
                string fileName = "MPI\\LEDTester\\Log\\ScripLog\\" + "THY_Trig_DAQ_PMDT_DUAL" + param.KeyName;
                ExportScriptToTxt(script, fileName);//20170628 David 紀錄Scrip
            }
            /////////////////////////////////////////////////////////////////////

            return script;
        }


        public static string THY(K2600ScriptSetting param)
        {
            param.SMUA.SweepPoints = (uint)Math.Round((double)param.SMUA.SweepPoints / 100.0d) * 100;

            string script = string.Empty;

            string srcRngAndComplScript = string.Empty;

            string srcFunc = string.Empty;

            string msrtFunc = string.Empty;

            // [0] I Source;  [1] V Source
            int srcMode = GetSrcMsrtMode(param.SMUA.SrcMode, out srcFunc, out msrtFunc);    // [0] I Source;  [1] V Source

            srcFunc = "i";

            msrtFunc = "v";

            //-----------------------------------------------------------------------------------------
            // Force Range and Compliance Setting
            //-----------------------------------------------------------------------------------------
            if (param.SMUA.MsrtClamp <= param.SMUA.MsrtBoundryLimit)
            {
                if (msrtFunc == "v")
                {
                    srcRngAndComplScript += "setLimitV(" + param.SMUA.MsrtClamp.ToString() + ")\n";

                    srcRngAndComplScript += "setSorceRangeI(" + param.SMUA.SrcRange.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    srcRngAndComplScript += "setLimitI(" + param.SMUA.MsrtClamp.ToString() + ")\n";

                    srcRngAndComplScript += "setSorceRangeV(" + param.SMUA.SrcRange.ToString() + ")\n";
                }
            }
            else
            {
                double forceRange = param.SMUA.SrcRange;

                if (msrtFunc == "v")
                {
                    if (Math.Abs(param.SMUA.SrcRange) < 0.000001)
                    {
                        forceRange = 0.000001;
                    }

                    srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";

                    srcRngAndComplScript += "setLimitV(" + param.SMUA.MsrtClamp.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    srcRngAndComplScript += "setSorceRangeV(" + forceRange.ToString() + ")\n";

                    srcRngAndComplScript += "setLimitI(" + param.SMUA.MsrtClamp.ToString() + ")\n";
                }
            }

            script = "loadscript num_" + param.Index.ToString() + "\n";

            script += "setNPLC(" + param.SMUA.MsrtNPLC.ToString() + ")\n";

            uint measureCount = (param.SMUA.SweepPoints) / 2;

            if (measureCount < 100)
            {
                measureCount = 100;
            }

            script += "setMsrtCount(" + measureCount.ToString() + ")\n";

            script += "setMsrtInterval(0.000001)\n";

            script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";

            if (msrtFunc == "v")
            {
                script += "setMeasureRangeV(" + param.SMUA.MsrtRange.ToString() + ")\n";

                script += "setMeasureRangeI(" + param.SMUA.SrcRange.ToString() + ")\n";
            }
            else if (msrtFunc == "i")
            {
                script += "setMeasureRangeI(" + param.SMUA.MsrtRange.ToString() + ")\n";

                script += "setMeasureRangeV(" + param.SMUA.SrcRange.ToString() + ")\n";
            }

            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            script += srcRngAndComplScript;

            if (param.SMUA.WaitTime > 0)
            {
                script += "delay(" + param.SMUA.WaitTime.ToString() + ")" + "\n";
            }

            if (srcFunc == "v")
            {
                script += "setLevelV(" + param.SMUA.SrcLevel.ToString() + ")\n";
            }
            else if (srcFunc == "i")
            {
                script += "setLevelI(" + param.SMUA.SrcLevel.ToString() + ")\n";
            }

            script += "mrtA." + msrtFunc + "(bufA1)\n";

            //-----------------------------------------------------------------------------------------
            // 若打IZ量VZ的Clamp電壓大於20，打完IZ後先降Clamp，以免多花20mS時間
            //-----------------------------------------------------------------------------------------
            if (Math.Abs(param.SMUA.MsrtClamp) > 20 && srcFunc == "i")
            {
                script += "setLimitV(8)\n";
            }

            if (param.SMUA.IsAutoTurnOff)
            {
                if (!param.IsTurnOffToZeroVolt)
                {
                    if (srcFunc == "v")
                    {
                        script += "setLevelV(0)\n";
                    }
                    else if (srcFunc == "i")
                    {
                        script += "setLevelI(0)\n";
                    }
                }
                else
                {
                    if (srcFunc == "v")
                    {
                        script += "setLevelV(0)\n";
                    }
                    else if (srcFunc == "i")
                    {
                        script += "setLevelI(0)\n";

                        script += "setFunc(1)\n";

                        script += "setLevelV(0)\n";
                    }
                }
            }

            if (param.IsSpFwCalcResult)
            {
                script += "local maxPeak = 0\n";

                script += "local stableSum = 0\n";

                script += "for i = 1, " + measureCount.ToString() + " do\n";

                script += "if bufA1.readings[i] > maxPeak then\n";

                script += "maxPeak = bufA1.readings[i]\n";

                script += "end\n";

                script += "if " + measureCount.ToString() + " - i < 20 then\n";

                script += "stableSum = stableSum + bufA1.readings[i]\n";

                script += "end\n";

                script += "end\n";

                script += "local stableValue = stableSum / 20\n";

                script += "local maxToStable = maxPeak - stableValue\n";

                script += "local Outp = string.format(\"%.3f, %.3f\", maxPeak, stableValue)\n";

                script += "print(Outp)\n";

                //script += "statistics = smua.buffer.getstats(smua.nvbuffer1)\n";

                //script += "local Outp = string.format(\"%.3f, %.3f\", statistics.max.reading, statistics.mean)\n";

                //script += "print(Outp)\n";
            }
            else
            {
                script += "printbuffer(1, " + measureCount.ToString() + ", bufA1)\n";
            }

            script += "setMsrtCount(1)\n";

            script += "bufA1.clear()\n";

            script += "endscript\n";

            script += "num_" + param.Index.ToString() + ".source = nil";



            return script;
        }

        public static string THY_SrcOnly(K2600ScriptSetting param)
        {
            string script = string.Empty;

            string srcRngAndComplScript = string.Empty;

            //-----------------------------------------------------------------------------------------
            // Force Range and Compliance Setting
            //-----------------------------------------------------------------------------------------
            if (param.SMUA.MsrtClamp <= param.SMUA.MsrtBoundryLimit)
            {
                srcRngAndComplScript += "setLimitV(" + param.SMUA.MsrtClamp.ToString() + ")\n";

                srcRngAndComplScript += "setSorceRangeI(" + param.SMUA.SrcRange.ToString() + ")\n";
            }
            else
            {
                double forceRange = param.SMUA.SrcRange;

                if (Math.Abs(param.SMUA.SrcRange) < 0.000001)
                {
                    forceRange = 0.000001;
                }

                srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";

                srcRngAndComplScript += "setLimitV(" + param.SMUA.MsrtClamp.ToString() + ")\n";
            }

            //-----------------------------------------------------------------------------------------
            // Test Sequence
            //-----------------------------------------------------------------------------------------
            script = "loadscript num_" + param.Index.ToString() + "\n";

            script += "if getFunc() ~= 1 then setFunc(1) end\n";

            script += "setSorceRangeV(0)\n";

            script += "setLevelV(0)\n";

            script += "delay(0.005)\n";

            script += "if getFunc() ~= 0 then setFunc(0) end\n";

            // Enable二極體:		PIN_RTH_EANBLE = 0  
            // Bypass 二極體:		PIN_RTH_EANBLE = 1
            // P:				_pinDAQEnable = 1, PIN_RTH_EANBLE = 0
            // N:				_pinDAQEnable = 0, PIN_RTH_EANBLE = 1
            // Open:			_pinDAQEnable = 0, PIN_RTH_EANBLE = 0
            //小電容:				PIN_CAP_SW = 0
            //大電容:				PIN_CAP_SW = 1

            // Bypass 二極體
            script += "digio.writebit(" + K2600Const.IO_RTH_EANBLE.ToString() + ", 1)\n";

            // P極
            script += "digio.writebit(" + K2600Const.IO_DAQ_ENABLE.ToString() + ", 1)\n";

            script += "digio.writebit(" + K2600Const.IO_POLAR_SW.ToString() + ", 0)\n";

            //切換大小電容
            if (param.SMUA.SrcLevel >= 0.00001)
            {
                script += "digio.writebit(" + K2600Const.IO_CAP_SW.ToString() + ", 1)\n";
            }
            else
            {
                script += "digio.writebit(" + K2600Const.IO_CAP_SW.ToString() + ", 0)\n";
            }

            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            script += srcRngAndComplScript;

            if (param.SMUA.WaitTime > 0)
            {
                script += "delay(" + param.SMUA.WaitTime.ToString() + ")" + "\n";
            }

            script += "digio.trigger[" + K2600Const.IO_DAQ_TRIG_OUT.ToString() + "].assert()\n";

            script += "setLevelI(" + param.SMUA.SrcLevel.ToString() + ")\n";

            script += "delay(" + param.SMUA.srcTime.ToString() + ")" + "\n";

            script += "print(0)\n";

            //-----------------------------------------------------------------------------------------
            // 若打IZ量VZ的Clamp電壓大於20，打完IZ後先降Clamp，以免多花20mS時間
            if (Math.Abs(param.SMUA.MsrtClamp) > 20)
            {
                script += "setLimitV(8)\n";
            }

            if (param.SMUA.IsAutoTurnOff)
            {
                if (!param.IsTurnOffToZeroVolt)
                {
                    script += "setLevelI(0)\n";
                }
                else
                {
                    script += "setLevelI(0)\n";

                    script += "setFunc(1)\n";

                    script += "setLevelV(0)\n";
                }
            }

            //關閉迴路
            script += "digio.writebit(" + K2600Const.IO_RTH_EANBLE.ToString() + ", 1)\n";

            script += "digio.writebit(" + K2600Const.IO_DAQ_ENABLE.ToString() + ", 0)\n";

            script += "digio.writebit(" + K2600Const.IO_POLAR_SW.ToString() + ", 0)\n";

            script += "endscript\n";

            script += "num_" + param.Index.ToString() + ".source = nil";

            //////////////////////////////////////////////////////
            if (WRITE_SCRIPT)
            {
                string fileName = "MPI\\LEDTester\\Log\\ScripLog\\" + "THY_SrcOnly" + param.Index.ToString();
                ExportScriptToTxt(script, fileName);//20170628 David 紀錄Scrip
            }
            /////////////////////////////////////////////////////////////////////

            return script;
        }

        public static string THY_Trig_DAQ(K2600ScriptSetting param)
        {
            string script = string.Empty;

            string srcRngAndComplScript = string.Empty;

            //-----------------------------------------------------------------------------------------
            // Force Range and Compliance Setting
            //-----------------------------------------------------------------------------------------
            if (param.SMUA.MsrtClamp <= param.SMUA.MsrtBoundryLimit)
            {
                srcRngAndComplScript += "setLimitV(" + param.SMUA.MsrtClamp.ToString() + ")\n";

                srcRngAndComplScript += "setSorceRangeI(" + param.SMUA.SrcRange.ToString() + ")\n";
            }
            else
            {
                double forceRange = param.SMUA.SrcRange;

                if (Math.Abs(param.SMUA.SrcRange) < 0.000001)
                {
                    forceRange = 0.000001;
                }

                srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";

                srcRngAndComplScript += "setLimitV(" + param.SMUA.MsrtClamp.ToString() + ")\n";
            }

            //-----------------------------------------------------------------------------------------
            // Test Sequence
            //-----------------------------------------------------------------------------------------
            script = "loadscript num_" + param.Index.ToString() + "_DAQTrigger\n";


            //----------------------------------------------
            // 打THY支前，碰一下，維持3ms
            //----------------------------------------------

            script += "if getFunc() ~= 0 then setFunc(0) end\n";

            script += "digio.writebit(" + K2600Const.IO_FRAME_GROUND.ToString() + ", 1)\n";

            script += "delay(0.003)\n";

            script += "digio.writebit(" + K2600Const.IO_FRAME_GROUND.ToString() + ", 0)\n";


            // Enable二極體:		PIN_RTH_EANBLE = 0  
            // Bypass 二極體:		PIN_RTH_EANBLE = 1
            // P:				_pinDAQEnable = 1, PIN_RTH_EANBLE = 0
            // N:				_pinDAQEnable = 0, PIN_RTH_EANBLE = 1
            // Open:			_pinDAQEnable = 0, PIN_RTH_EANBLE = 0
            //小電容:				PIN_CAP_SW = 0
            //大電容:				PIN_CAP_SW = 1

            // Bypass 二極體
            script += "digio.writebit(" + K2600Const.IO_RTH_EANBLE.ToString() + ", 1)\n";

            // P極
            script += "digio.writebit(" + K2600Const.IO_DAQ_ENABLE.ToString() + ", 1)\n";

            script += "digio.writebit(" + K2600Const.IO_RTH_EANBLE.ToString() + ", 0)\n";

            //切換大小電容
            if (param.SMUA.SrcLevel >= 0.00001)
            {
                script += "digio.writebit(" + K2600Const.IO_CAP_SW.ToString() + ", 1)\n";
            }
            else
            {
                script += "digio.writebit(" + K2600Const.IO_CAP_SW.ToString() + ", 0)\n";
            }

            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            script += srcRngAndComplScript;

            if (param.SMUA.WaitTime > 0)
            {
                script += "delay(" + param.SMUA.WaitTime.ToString() + ")" + "\n";
            }

            script += "delay(0.0003)\n";

            script += "digio.trigger[" + K2600Const.IO_DAQ_ENABLE.ToString() + "].assert()\n";

            script += "delay(0.0002)\n";

            //script += "setLevelI(" + item.ForceValue.ToString() + ")\n";

            script += "delay(" + param.SMUA.srcTime.ToString() + ")" + "\n";

            //script += "print(0)\n";

            //-----------------------------------------------------------------------------------------
            // 若打IZ量VZ的Clamp電壓大於20，打完IZ後先降Clamp，以免多花20mS時間
            //-----------------------------------------------------------------------------------------
            if (Math.Abs(param.SMUA.MsrtClamp) > 20)
            {
                script += "setLimitV(8)\n";
            }

            if (param.SMUA.IsAutoTurnOff)
            {
                if (!param.IsTurnOffToZeroVolt)// && !item.IsNextIsESDTestItem)
                {
                    script += "setLevelI(0)\n";
                }
                else
                {
                    script += "setLevelI(0)\n";

                    script += "setFunc(1)\n";

                    script += "setLevelV(0)\n";
                }
            }

            //關閉迴路
            script += "digio.writebit(" + K2600Const.IO_RTH_EANBLE.ToString() + ", 1)\n";

            script += "digio.writebit(" + K2600Const.IO_DAQ_ENABLE.ToString() + ", 0)\n";

            script += "digio.writebit(" + K2600Const.IO_RTH_EANBLE.ToString() + ", 0)\n";

            script += "endscript\n";

            script += "num_" + param.Index.ToString() + "_DAQTrigger.source = nil";

            //////////////////////////////////////////////////////
            if (WRITE_SCRIPT)
            {
                string fileName = "MPI\\LEDTester\\Log\\ScripLog\\" + "THY_Trig_DAQ" + param.Index.ToString();
                ExportScriptToTxt(script, fileName);//20170628 David 紀錄Scrip
            }
            /////////////////////////////////////////////////////////////////////

            return script;
        }

        #endregion

        #region >>> Sweep / Scan Script <<<

        public static string Sweep_DUAL(K2600ScriptSetting param)
        {
            int numPoints = 10;
            double pulsePeriod = 0.01d;
            double pulseWidth = 0.001d;
            double start = 0.001d;
            double stop = 0.021d;
            double limitV = 8;
            double nplc = 0.003;

            string srcFunc = string.Empty;
            string msrtFunc = string.Empty;

            int srcMode = GetSrcMsrtMode(param.SMUA.SrcMode, out srcFunc, out msrtFunc);    // [0] I Source;  [1] V Source

            double forceRange = Math.Max(Math.Abs(param.SMUA.SweepStop),Math.Abs(param.SMUA.SweepStart));

            string script = string.Empty;
            string listName = string.Format("List_{0}", param.Index);

            script += "loadscript num_" + param.Index.ToString() + "\n";

            //--------------------------------------------------------------
            // smua Config
            script += "smua.source.func	= smua.OUTPUT_DCAMPS\n";
            script += "smua.source.autorangei = smua.AUTORANGE_OFF\n";
            script += "smua.source.rangei = " + forceRange + "\n";
            script += "smua.source.leveli = 0\n";
            script += "smua.source.limitv = 1\n";

            script += "smua.measure.autozero = smua.AUTOZERO_OFF\n";
            script += "smua.measure.autorangev = smua.AUTORANGE_OFF\n";
            script += "smua.measure.rangev = " + limitV.ToString() + "\n";
            script += "smua.measure.nplc = " + nplc.ToString() + "\n";

            script += "smua.measure.delay = 0\n";

            script += "smua.nvbuffer1.clear()\n";
            script += "smua.nvbuffer1.collecttimestamps = 0\n";
            script += "smua.nvbuffer2.clear()\n";
            script += "smua.nvbuffer2.collecttimestamps = 0\n";
            //--------------------------------------------------------------
            // smub Config
            script += "smub.source.func	= smub.OUTPUT_DCVOLTS\n";
            script += "smub.source.autorangev = smua.AUTORANGE_OFF\n";
            script += "smub.source.rangev = " + forceRange + "\n";
            script += "smub.source.levelv " + param.SMUB.SrcLevel.ToString() + "\n";
            script += "smub.source.limiti " + param.SMUB.MsrtClamp.ToString() + "\n";

            script += "smub.measure.autozero = smub.AUTOZERO_OFF\n";
            script += "smub.measure.autorangei = smub.AUTORANGE_OFF\n";
            script += "smub.measure.rangei = " + param.SMUB.MsrtClamp.ToString() + "\n";
            script += "smub.measure.nplc = " + nplc.ToString() + "\n";

            script += "smub.measure.delay = 0\n";

            script += "smub.nvbuffer1.clear()\n";
            script += "smub.nvbuffer1.collecttimestamps = 0\n";
            script += "smub.nvbuffer2.clear()\n";
            script += "smub.nvbuffer2.collecttimestamps = 0\n";
            //--------------------------------------------------------------

            script += "trigger.timer[1].count = 1\n";
            script += "trigger.timer[1].delay = " + pulsePeriod.ToString() + "\n";
            script += "trigger.timer[1].passthrough	= true\n";
            script += "trigger.timer[1].stimulus = smua.trigger.ARMED_EVENT_ID\n";

            script += "trigger.timer[2].count = 1\n";
            script += "trigger.timer[2].delay = " + pulseWidth.ToString() + " - (1/localnode.linefreq)*" + nplc.ToString() + " - 60e-6\n";
            script += "trigger.timer[2].passthrough	= false\n";
            script += "trigger.timer[2].stimulus = trigger.timer[1].EVENT_ID\n";

            script += "trigger.timer[3].count = 1\n";
            script += "trigger.timer[3].delay = " + pulseWidth.ToString() + "\n";
            script += "trigger.timer[3].passthrough	= false\n";
            script += "trigger.timer[3].stimulus = trigger.timer[1].EVENT_ID\n";

            //--------------------------------------------------------------
            // smua
            script += "smua.trigger.source.list" + srcFunc + "(" + listName + ")\n"; // configure the source action
            //script += "smua.trigger.source.lineari(" + start.ToString() + ", " + stop.ToString() + ", " + numPoints.ToString() + ")\n";
            script += "smua.trigger.source.limit" + msrtFunc + " = smua.LIMIT_AUTO\n";
            script += "smua.trigger.measure.action = smua.ENABLE\n";
            script += "smua.trigger.measure." + msrtFunc + "(smua.nvbuffer1)\n";
            script += "smua.trigger.endpulse.action	= smua.SOURCE_IDLE\n";
            script += "smua.trigger.endsweep.action	= smua.SOURCE_IDLE\n";
            script += "smua.trigger.count = " + numPoints.ToString() + "\n";
            script += "smua.trigger.arm.stimulus = 0\n";
            script += "smua.trigger.source.stimulus	= trigger.timer[1].EVENT_ID\n";
            script += "smua.trigger.measure.stimulus = trigger.timer[2].EVENT_ID\n";
            script += "smua.trigger.endpulse.stimulus = trigger.timer[3].EVENT_ID\n";
            script += "smua.trigger.source.action = smua.ENABLE\n";
            //--------------------------------------------------------------
            //smub
            script += "smua.trigger.source.list" + srcFunc + "(" + listName + ")\n"; // configure the source action
            script += "smub.trigger.source.limiti = " + param.SMUB.MsrtRange.ToString() + "\n";
            script += "smub.trigger.measure.action = smub.ENABLE\n";
            script += "smub.trigger.measure.iv(smub.nvbuffer1, smub.nvbuffer2)\n";
            script += "smub.trigger.endpulse.action	= smub.SOURCE_IDLE\n";
            script += "smub.trigger.endsweep.action	= smub.SOURCE_IDLE\n";
            script += "smub.trigger.count = " + numPoints.ToString() + "\n";
            script += "smub.trigger.arm.stimulus = 0\n";
            script += "smub.trigger.source.stimulus	= trigger.timer[1].EVENT_ID\n";
            script += "smub.trigger.measure.stimulus = trigger.timer[2].EVENT_ID\n";
            script += "smub.trigger.endpulse.stimulus = trigger.timer[3].EVENT_ID\n";
            script += "smub.trigger.source.action = smub.ENABLE\n";

            //--------------------------------------------------------------
            // smua & smub Trigger
            script += "smua.source.output = smua.OUTPUT_ON\n";
            script += "smub.source.output = smub.OUTPUT_ON\n";

            script += "smua.trigger.initiate()\n";
            script += "smub.trigger.initiate()\n";

            script += "waitcomplete()\n";
            script += "smua.source.output = smua.OUTPUT_OFF\n";
            script += "smub.source.output = smub.OUTPUT_OFF\n";

            // smua & smub print result

            //script += "print(\"Time\tVoltage\tCurrent\")\n";
            //script += "for x=1,smua.nvbuffer1.n do\n";
            //script += "print(smua.nvbuffer2[x], smua.nvbuffer1[x])\n";
            //script += "end\n";

            script += "endscript\n";
            script += "num_" + param.Index.ToString() + ".source = nil\n";

            return script;
        }

        //public static string Sweep_DUAL(K2600ScriptSetting param)
        //{
        //    string script = string.Empty;
        //    string srcRngAndComplScript = string.Empty;
        //    string srcFunc = string.Empty;
        //    string msrtFunc = string.Empty;

        //    uint index = param.Index;
        //    double waitTime = param.SMUA.WaitTime;
        //    double forceTime = param.SMUA.srcTime;
        //    double forceRange = param.SMUA.SrcRange;


        //    ///////////////////////////////////////////////////////////////////////////////////////////////
        //    // SrcMode
        //    ///////////////////////////////////////////////////////////////////////////////////////////////
        //    string srcModeScriptA = string.Empty;
        //    string srcModeScriptB = string.Empty;
        //    string srcModeScriptAB = string.Empty;

        //    SetSrcMode(param, ref  srcModeScriptA, ref  srcModeScriptB, ref  srcModeScriptAB);
        //    ///////////////////////////////////////////////////////////////////////////////////////////////
        //    // Force Range and Compliance Setting
        //    ///////////////////////////////////////////////////////////////////////////////////////////////
        //    string srcRngAndComplScriptA = string.Empty;
        //    string srcRngAndComplScriptB = string.Empty;
        //    string srcRngAndComplScriptAB = string.Empty;

        //    SetPMDTSrcRange(param, ref srcRngAndComplScriptA, ref srcRngAndComplScriptB, ref srcRngAndComplScriptAB, ref forceRange);//srcFunc, msrtFunc, clampLimit, nplc, ref forceRange, clamp);

        //    ///////////////////////////////////////////////////////////////////////////////////////////////
        //    // Range selected
        //    ///////////////////////////////////////////////////////////////////////////////////////////////
        //    string msrtRangeScriptA = string.Empty;
        //    string msrtRangeScriptB = string.Empty;
        //    string msrtRangeScriptAB = string.Empty;

        //    SetPMDTMsrtRange(param, ref msrtRangeScriptA, ref msrtRangeScriptB, ref msrtRangeScriptAB);

        //    ///////////////////////////////////////////////////////////////////////////////////////////////
        //    // source output
        //    ///////////////////////////////////////////////////////////////////////////////////////////////
        //    string srcScriptA = string.Empty;
        //    string srcScriptB = string.Empty;
        //    string srcScriptAB = string.Empty;

        //    SetPMDTDC(param, ref srcScriptA, ref srcScriptB, ref srcScriptAB);

        //    ///////////////////////////////////////////////////////////////////////////////////////////////
        //    // Msrt
        //    ///////////////////////////////////////////////////////////////////////////////////////////////
        //    string msrtScriptA = string.Empty;
        //    string msrtScriptB = string.Empty;
        //    string msrtScriptAB = string.Empty;

        //    SetPMDTMsrtDC(param, ref msrtScriptA, ref msrtScriptB, ref msrtScriptAB);

        //    ///////////////////////////////////////////////////////////////////////////////////////////////
        //    // Source Off
        //    ///////////////////////////////////////////////////////////////////////////////////////////////
        //    string srcOffScriptA = string.Empty;
        //    string srcOffScriptB = string.Empty;
        //    string srcOffScriptAB = string.Empty;

        //    SetPMDTTurnOff(param, forceRange, ref srcOffScriptA, ref srcOffScriptB, ref srcOffScriptAB);

        //    ///////////////////////////////////////////////////////////////////////////////////////////////
        //    // NPLC
        //    ///////////////////////////////////////////////////////////////////////////////////////////////
        //    string NPLCScriptA = string.Empty;
        //    string NPLCScriptB = string.Empty;
        //    string NPLCScriptAB = string.Empty;

        //    SetPMDTNPLC(param, ref NPLCScriptA, ref NPLCScriptB, ref NPLCScriptAB);




        //    int srcMode = GetSrcMsrtMode(param.SMUA.SrcMode, out srcFunc, out msrtFunc);    // [0] I Source;  [1] V Source

            

        //    //-----------------------------------------------------------------------------------------
        //    // Sweep Sequence
        //    //-----------------------------------------------------------------------------------------
        //    script += "loadscript num_" + param.Index.ToString() + "\n";         // set the script name for the i-th parameter: num_i; i starts from 0

        //    //--------------------------------------------------------------------------------------------------
        //    script += "if channel == 1 then\n"; // SMUA

        //    script += NPLCScriptA;

        //    script += srcModeScriptA;

        //    script += "smua.source.level" + srcFunc + " = 0\n";

        //    if (param.SMUA.IsAutoMsrtRange)
        //    {
        //        script += "smua.measure.autorangei = smua.AUTORANGE_ON\n";
        //        script += "smua.measure.autorangev = smua.AUTORANGE_ON\n";
        //    }
        //    else
        //    {
        //        script += msrtRangeScriptA;
        //    }

        //    script += "if getOutput() ~= 1 then setOutput(1) end\n";

        //    script += srcRngAndComplScriptA;

        //    string listName = SetSweepScipt(param., ref script, srcFunc, msrtFunc);


        //    if (param.SMUA.WaitTime > 0)
        //    {
        //        script += "delay(" + param.SMUA.WaitTime.ToString() + ")\n";
        //    }
        //     //------------------------ Hold Time --------------------------------------

        //    if (param.SMUA.SweepStartHoldTime > 0.0d)
        //    {
        //        script += "smua.source.level" + srcFunc + " = " + listName + "[1]\n";
        //        script += "delay(" + param.SMUA.SweepStartHoldTime.ToString() + ")\n";
        //    }

        //    //-------------------------------------------------------------------------
        //    script += "smua.trigger.initiate()\n";
        //    script += "smua.trigger.source.set()\n";
        //    script += "waitcomplete()\n";


        //    // disable the measurement autorage after the sweep operation
        //    script += "smua.measure.autorangei = smua.AUTORANGE_OFF\n";
        //    script += "smua.measure.autorangev = smua.AUTORANGE_OFF\n";


        //    script += "printbuffer(1, smua.nvbuffer1.n, smua.nvbuffer1)\n";
        //    script += "smua.nvbuffer1.clear()\n";     // clear the reading buffers

        //    script += "endscript\n";





        //    script += "num_" + param.Index.ToString() + ".source = nil";

        //    return script;
        //}

        //private static void SetClearBufferScript(K2600ScriptSetting param, ref string scriptA, ref string scriptB, ref string scriptAB)
        //{
        //    string srcFunc = "";
        //    string msrtFunc = "";
        //    scriptA = "";
        //    scriptB = "";
        //    scriptAB = "";
        //    GetSrcMsrtMode(param.SMUA.SrcMode, out  srcFunc, out  msrtFunc);
        //    scriptA += "smua.nvbuffer1.clear()\n";
        //    scriptA += "smua.nvbuffer1.collecttimestamps = 0\n";
        //    scriptA += "smua.nvbuffer2.clear()\n";
        //    scriptA += "smua.nvbuffer2.collecttimestamps = 0\n";
        //    scriptB += "smua.nvbuffer1.clear()\n";
        //    scriptB += "smua.nvbuffer1.collecttimestamps = 0\n";
        //    scriptB += "smua.nvbuffer2.clear()\n";
        //    scriptB += "smua.nvbuffer2.collecttimestamps = 0\n";
        //    scriptAB = scriptA + scriptB;
        //}

        //private static void SetSweepScipt(K2600ScriptSetting param, ref string scriptA, ref string scriptB, ref string scriptAB)
        //{
        //    string srcFunc = "";
        //    string msrtFunc = "";
        //    GetSrcMsrtMode(param.SMUA.SrcMode, out  srcFunc, out  msrtFunc);
        //    string listName = string.Format("List_{0}", param.Index);
        //    //string script = "";

        //    #region >>trigger Script<<
        //    string triggerScript = "";

        //    triggerScript += "trigger.timer[1].count = 1\n";
        //    triggerScript += "trigger.timer[1].delay = " + param.SMUA.srcTime.ToString() + "\n";
        //    triggerScript += "trigger.timer[1].passthrough	= true\n";
        //    triggerScript += "trigger.timer[1].stimulus = smua.trigger.ARMED_EVENT_ID\n";

        //    if (param.SMUA.SweepEndPulseAction != EK2600EndPulseAction.SOURCE_HOLD)
        //    {
        //        triggerScript += "trigger.timer[2].count = 1\n";
        //        triggerScript += "trigger.timer[2].delay = " + param.SMUA.SweepEndPulseTurnOffTime.ToString() + "\n";
        //        triggerScript += "trigger.timer[2].passthrough	= false\n";
        //        triggerScript += "trigger.timer[2].stimulus = trigger.timer[1].EVENT_ID\n";
        //    }
        //    #endregion

        //    #region >>smua<<

        //    scriptA += "smua.trigger.source.limit" + msrtFunc + " = smua.LIMIT_AUTO\n";
        //    scriptA +="smua.trigger.measure." + msrtFunc + "(smua.nvbuffer1)\n";
        //    scriptA +="smua.trigger.measure.action = smua.ENABLE\n";
        //    scriptA +="smua.measure.count = 1\n";
        //    if (param.SMUA.SweepEndPulseAction == EK2600EndPulseAction.SOURCE_HOLD)
        //    {
        //        scriptA += "smua.trigger.source.stimulus = 0\n";
        //    }
        //    #endregion



        //    scriptA += "smua.trigger.source.list" + srcFunc + "(" + listName + ")\n"; // configure the source action
        //    scriptA += "smua.trigger.source.limit" + msrtFunc + " = smua.LIMIT_AUTO\n";
        //    scriptA += "smua.trigger.measure.action = smua.ENABLE\n";
        //    scriptA += "smua.trigger.measure." + msrtFunc + "(smua.nvbuffer1, smua.nvbuffer2)\n";
        //    scriptA += "smua.trigger.endpulse.action	= smua.SOURCE_IDLE\n";
        //    scriptA += "smua.trigger.endsweep.action	= smua.SOURCE_IDLE\n";
        //    scriptA += "smua.trigger.count = " + numPoints.ToString() + "\n";
        //    scriptA += "smua.trigger.arm.stimulus = 0\n";
        //    scriptA += "smua.trigger.source.stimulus	= trigger.timer[1].EVENT_ID\n";
        //    scriptA += "smua.trigger.measure.stimulus = trigger.timer[2].EVENT_ID\n";
        //    scriptA += "smua.trigger.endpulse.stimulus = trigger.timer[3].EVENT_ID\n";
        //    scriptA += "smua.trigger.source.action = smua.ENABLE\n";

            


        //    uint sweepCnt = param.SMUA.SweepPoints;

        //    script += "smua.trigger.source.list" + srcFunc + "(" + listName + ")\n" // configure the source action
        //               + "smua.trigger.source.action = smua.ENABLE\n"
        //               + "smua.trigger.endpulse.action = smua.SOURCE_HOLD\n" // configure the end pluse action 
        //               + "trigger.timer[1].reset()\n"                        // configure the timer triggering
        //               + "trigger.timer[1].stimulus = smua.trigger.SOURCE_COMPLETE_EVENT_ID\n"
        //               + "trigger.timer[1].delay = " + param.SMUA.srcTime.ToString() + "\n"
        //               + "trigger.timer[1].count = 1\n";


        //    if (param.SMUA.SweepEndPulseAction != EK2600EndPulseAction.SOURCE_IDEL)
        //    {
        //        script += "trigger.timer[2].reset()\n"
        //               + "trigger.timer[2].delay = " + param.SMUA.SweepEndPulseTurnOffTime.ToString() + "\n"
        //               + "trigger.timer[2].count = 1\n"
        //               + "trigger.timer[2].stimulus = smua.trigger.PULSE_COMPLETE_EVENT_ID\n";
        //    }
        //    script += +"smua.trigger.measure.stimulus = trigger.timer[1].EVENT_ID\n"
        //           + "smua.trigger.endpulse.stimulus = smua.trigger.MEASURE_COMPLETE_EVENT_ID\n";

        //    script += "smua.trigger.arm.count = 1\n"                      // configure the trigger count
        //               + "smua.trigger.count = " + sweepCnt.ToString() + "\n";
        //    return listName;
        //}


        public static string Sweep_SMUA(K2600ScriptSetting param,string listID = "")
        {
            string script = string.Empty;
            string srcRngAndComplScript = string.Empty;
            string srcFunc = string.Empty;
            string msrtFunc = string.Empty;

            int srcMode = GetSrcMsrtMode(param.SMUA.SrcMode, out srcFunc, out msrtFunc);    // [0] I Source;  [1] V Source

            if (listID == "")
            {
                listID = param.Index.ToString();
            }

            string listName = "List_" + listID;

            uint sweepCnt = param.SMUA.SweepPoints;

            //-----------------------------------------------------------------------------------------
            // Sweep Sequence
            //-----------------------------------------------------------------------------------------
            script += "loadscript num_" + listID + "\n"         // set the script name for the i-th parameter: num_i; i starts from 0
                    + "smua.source.func = " + srcMode.ToString() + "\n"
                    + "smua.source.level" + srcFunc + " = 0\n";            // set the source level in the idle status

            //script += "smua.measure.lowrange" + msrtFunc + " = 10e-6\n";

            // Force Range and Compliance Setting
            if (param.SMUA.MsrtClamp <= param.SMUA.MsrtBoundryLimit)
            {
                srcRngAndComplScript += "smua.source.limit" + msrtFunc + " = " + param.SMUA.MsrtClamp.ToString() + "\n"        // set the normal compliance
                                      + "smua.source.range" + srcFunc + " = " + param.SMUA.SrcRange.ToString() + "\n";            // Selects the range for the specified source
            }
            else
            {
                srcRngAndComplScript += "smua.source.range" + srcFunc + " = " + param.SMUA.SrcRange.ToString() + "\n"              // Selects the range for the specified source
                                      + "smua.source.limit" + msrtFunc + " = " + param.SMUA.MsrtClamp.ToString() + "\n";        // set the normal compliance
            }

            if (param.SMUA.IsAutoMsrtRange)
            {
                script += "smua.measure.autorangei = smua.AUTORANGE_ON\n";
                script += "smua.measure.autorangev = smua.AUTORANGE_ON\n";
            }
            else
            {
                if (msrtFunc == "v")
                {
                    script += "setMeasureRangeV(" + param.SMUA.MsrtClamp.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    script += "setMeasureRangeI(" + param.SMUA.MsrtClamp.ToString() + ")\n";
                }
            }

            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            script += srcRngAndComplScript
                    + "smua.trigger.source.limit" + msrtFunc + " = smua.LIMIT_AUTO\n"
                    + "smua.trigger.measure." + msrtFunc + "(smua.nvbuffer1)\n"
                    + "smua.trigger.measure.action = smua.ENABLE\n"
                    + "smua.measure.count = 1\n";

            script += "setNPLC(" + param.SMUA.MsrtNPLC.ToString() + ")\n";

            if (param.SMUA.WaitTime > 0)
            {
                script += "delay(" + param.SMUA.WaitTime.ToString() + ")\n";
            }

            if (param.SMUA.SweepEndPulseAction == EK2600EndPulseAction.SOURCE_HOLD)
            {
                // Continous DC Sweep
                script += "smua.trigger.arm.stimulus = 0\n" // Reset trigger model stimuli
                       + "smua.trigger.source.stimulus = 0\n"
                       + "smua.trigger.source.list" + srcFunc + "(" + listName + ")\n" // configure the source action
                       + "smua.trigger.source.action = smua.ENABLE\n"
                       + "smua.trigger.endpulse.action = smua.SOURCE_HOLD\n" // configure the end pluse action 
                       + "trigger.timer[1].reset()\n"                        // configure the timer triggering
                       + "trigger.timer[1].stimulus = smua.trigger.SOURCE_COMPLETE_EVENT_ID\n"
                       + "trigger.timer[1].delay = " + param.SMUA.srcTime.ToString() + "\n"
                       + "trigger.timer[1].count = 1\n"
                       + "smua.trigger.measure.stimulus = trigger.timer[1].EVENT_ID\n"
                       + "smua.trigger.endpulse.stimulus = smua.trigger.MEASURE_COMPLETE_EVENT_ID\n"
                       + "smua.trigger.arm.count = 1\n"                      // configure the trigger count
                       + "smua.trigger.count = " + sweepCnt.ToString() + "\n";
            }
            else
            {
                // Pulsed DC Sweep
                script += "smua.trigger.arm.stimulus = 0\n"                                         // Reset trigger model stimulus         
                       + "smua.trigger.source.list" + srcFunc + "(" + listName + ")\n" // configure the source action
                       + "smua.trigger.source.action = smua.ENABLE\n"
                       + "smua.trigger.endpulse.action = smua.SOURCE_IDLE\n" // configure the end pulse action
                       + "trigger.timer[1].reset()\n"                        // timer[1] is for the pulse output and the measurement
                       + "trigger.timer[1].stimulus = smua.trigger.SOURCE_COMPLETE_EVENT_ID\n"
                       + "trigger.timer[1].delay = " + param.SMUA.srcTime.ToString() + "\n"
                       + "trigger.timer[1].count = 1\n"
                       + "smua.trigger.measure.stimulus = trigger.timer[1].EVENT_ID\n"
                       + "smua.trigger.endpulse.stimulus = smua.trigger.MEASURE_COMPLETE_EVENT_ID\n"
                       + "trigger.timer[2].reset()\n"
                       + "trigger.timer[2].delay = " + param.SMUA.SweepEndPulseTurnOffTime.ToString() + "\n"
                       + "trigger.timer[2].count = 1\n"
                       + "trigger.timer[2].stimulus = smua.trigger.PULSE_COMPLETE_EVENT_ID\n"
                       + "smua.trigger.source.stimulus = trigger.timer[2].EVENT_ID\n"         // the timer[2] is for stimulating the pulses after the 1st pulse and defining the interval between two consecutive pulses
                       + "smua.trigger.arm.count = 1\n"                                       // configure the trigger count
                       + "smua.trigger.count = " + sweepCnt.ToString() + "\n";
            }

            //------------------------ Hold Time --------------------------------------

            if (param.SMUA.SweepStartHoldTime > 0.0d)
            {
                script += "smua.source.level" + srcFunc + " = " + listName + "[1]\n";
                script += "delay(" + param.SMUA.SweepStartHoldTime.ToString() + ")\n";
            }

            //-------------------------------------------------------------------------
            script += "smua.trigger.initiate()\n";
            script += "smua.trigger.source.set()\n";
            script += "waitcomplete()\n";


            // disable the measurement autorage after the sweep operation
            script += "smua.measure.autorangei = smua.AUTORANGE_OFF\n";
            script += "smua.measure.autorangev = smua.AUTORANGE_OFF\n";


            script += "printbuffer(1, smua.nvbuffer1.n, smua.nvbuffer1)\n";
            script += "smua.nvbuffer1.clear()\n";     // clear the reading buffers

            script += "endscript\n";

            script += "num_" + listID + ".source = nil";

            return script;
        }

        public static string Sweep_SMUAV2(K2600ScriptSetting param)
        {
            string script = string.Empty;
            string srcRngAndComplScript = string.Empty;
            string srcFunc = string.Empty;
            string msrtFunc = string.Empty;
            bool isDeviceMsrtTimeStamps = true;

            int srcMode = GetSrcMsrtMode(param.SMUA.SrcMode, out srcFunc, out msrtFunc);    // [0] I Source;  [1] V Source

            string ItemID = param.Index.ToString();

            string listName = "List_" + param.Index.ToString();

            uint sweepCnt = param.SMUA.SweepPoints;

            //-----------------------------------------------------------------------------------------
            // Sweep Sequence
            //-----------------------------------------------------------------------------------------
            script += "loadscript num_" + ItemID + "\n"         // set the script name for the i-th parameter: num_i; i starts from 0
                    + "smua.source.func = " + srcMode.ToString() + "\n";
                    //+ "smua.source.level" + srcFunc + " = 0\n";            // set the source level in the idle status
            if (isDeviceMsrtTimeStamps)
            {
                script += "setBufTimestamps(1)\n";
            }
            script += "bufA1.appendmode = 1\n";

            if (param.SMUA.MsrtClamp <= param.SMUA.MsrtBoundryLimit)
            {
                srcRngAndComplScript += "smua.source.limit" + msrtFunc + " = " + param.SMUA.MsrtClamp.ToString() + "\n"        // set the normal compliance
                                      + "smua.source.range" + srcFunc + " = " + param.SMUA.SrcRange.ToString() + "\n";            // Selects the range for the specified source
            }
            else
            {
                srcRngAndComplScript += "smua.source.range" + srcFunc + " = " + param.SMUA.SrcRange.ToString() + "\n"              // Selects the range for the specified source
                                      + "smua.source.limit" + msrtFunc + " = " + param.SMUA.MsrtClamp.ToString() + "\n";        // set the normal compliance
            }

            if (param.SMUA.IsAutoMsrtRange)
            {
                script += "smua.measure.autorange" + msrtFunc + " = smua.AUTORANGE_ON\n";
                script += "smua.measure.delay = -1\n";
                script += "smua.measure.delayfactor = 0.5\n";
            }
            else
            {
                script += "smua.measure.delay = 0\n";
                script += "setLimit" + msrtFunc .ToUpper()+ "(" + param.SMUA.MsrtClamp.ToString() + ")\n";
                script += "setMeasureRange" + msrtFunc.ToUpper() + "(" + param.SMUA.MsrtRange.ToString() + ")\n";
               
            }

            script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";
            script += srcRngAndComplScript;            
            script += "setNPLC(" + param.SMUA.MsrtNPLC.ToString() + ")\n";
            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            script += "for i = 1," + sweepCnt.ToString() + " do\n";
            script += "setLevel" + srcFunc.ToUpper() + "(" + listName + "[i])\n";
            script += "delay(" + param.SMUA.srcTime.ToString() + ")\n";
            script += "mrt" + msrtFunc.ToUpper() + "syncA(bufA1)\n";  // overlappediv
            script += "waitcomplete()\n";
            if (param.SMUA.SweepEndPulseAction == EK2600EndPulseAction.SOURCE_IDEL)
            {
                script += "setLevel" + srcFunc.ToUpper() + "(0)\n";
                script += "delay(" + param.SMUA.SweepEndPulseTurnOffTime.ToString() + ")\n";
            }
            script += "end\n";
            if (param.SMUA.IsAutoMsrtRange)
            {
                script += "smua.measure.autorange" + msrtFunc + " = smua.AUTORANGE_OFF\n";
                script += "smua.measure.delay = 0\n";
            }

            script += "printbuffer(1, " + param.SMUA.SweepPoints.ToString() + ", bufA1, bufA1.timestamps)\n";
            //script += "printbuffer(1, smua.nvbuffer1.n, smua.nvbuffer1,smua.nvbuffer1.timestamps)\n";
            script += "smua.nvbuffer1.clear()\n";     // clear the reading buffers

            if (isDeviceMsrtTimeStamps)
            {
                script += "setBufTimestamps(0)\n";
            }

            script += "endscript\n";

            script += "num_" + ItemID + ".source = nil";

            if (WRITE_SCRIPT)
            {
                string fileName = "MPI\\LEDTester\\Log\\ScripLog\\" + "SWEEP_" + param.KeyName + ItemID;
                ExportScriptToTxt(script, fileName);
            }

            return script;
        }


        public static string ScanI_SMUA(K2600ScriptSetting param)
        {
            bool isDeviceMsrtTimeStamps = true;

            string script = string.Empty;

            string srcRngAndComplScript = string.Empty;

            string srcFunc = string.Empty;

            string msrtFunc = string.Empty;

            int srcMode = GetSrcMsrtMode(param.SMUA.SrcMode, out srcFunc, out msrtFunc);    // [0] I Source;  [1] V Source

            //-----------------------------------------------------------------------------------------
            // Force Range and Compliance Setting
            //-----------------------------------------------------------------------------------------
            if (param.SMUA.MsrtClamp <= param.SMUA.MsrtBoundryLimit)
            {
                double forceRange = param.SMUA.SrcRange;

                if (msrtFunc == "v")
                {
                    srcRngAndComplScript += "setLimitV(" + param.SMUA.MsrtClamp.ToString() + ")\n";

                    srcRngAndComplScript += "setSorceRangeI(" + param.SMUA.SrcRange.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    if (Math.Abs(param.SMUA.SrcRange) < 0.2)
                    {
                        forceRange = 0.2d;
                    }

                    srcRngAndComplScript += "setLimitI(" + param.SMUA.MsrtClamp.ToString() + ")\n";

                    srcRngAndComplScript += "setSorceRangeV(" + forceRange.ToString() + ")\n";
                }
            }
            else
            {
                double forceRange = param.SMUA.SrcRange;

                if (msrtFunc == "v")
                {
                    //if (Math.Abs(param.SMUA.SrcRange) < 0.000001)
                    //{
                    //    forceRange = 0.000001;
                    //}

                    srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";

                    srcRngAndComplScript += "setLimitV(" + param.SMUA.MsrtClamp.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    //if (Math.Abs(param.SMUA.SrcRange) < 0.000001)
                    //{
                    //    forceRange = 2.0d;
                    //}

                    srcRngAndComplScript += "setSorceRangeV(" + forceRange.ToString() + ")\n";

                    srcRngAndComplScript += "setLimitI(" + param.SMUA.MsrtClamp.ToString() + ")\n";
                }
            }

            script = "loadscript num_" + param.Index.ToString() + "\n";

            script += "setNPLC(" + param.SMUA.MsrtNPLC.ToString() + ")\n";

            script += "setMsrtCount(" + param.SMUA.SweepPoints.ToString() + ")\n";

            script += "setMsrtInterval(0.000001)\n";

            if (isDeviceMsrtTimeStamps)
            {
                script += "setBufTimestamps(1)\n";
            }

            script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";

            if (msrtFunc == "v")
            {
                script += "setMeasureRangeV(" + param.SMUA.MsrtRange.ToString() + ")\n";

                script += "setMeasureRangeI(" + param.SMUA.SrcRange.ToString() + ")\n";
            }
            else if (msrtFunc == "i")
            {
                script += "setMeasureRangeI(" + param.SMUA.MsrtRange.ToString() + ")\n";

                double forceRange = Math.Abs(param.SMUA.SrcRange);

                //if (Math.Abs(param.SMUA.SrcRange) < 0.000001)
                //{
                //    forceRange = 2.0d;
                //}

                script += "setMeasureRangeV(" + forceRange.ToString() + ")\n";
            }

            script += "smua.measure.autozero = smua.AUTOZERO_ONCE\n";

            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            script += srcRngAndComplScript;

            if (param.SMUA.WaitTime > 0)
            {
                script += "delay(" + param.SMUA.WaitTime.ToString() + ")" + "\n";
            }

            if (srcFunc == "v")
            {
                script += "setLevelV(" + param.SMUA.SrcLevel.ToString() + ")\n";
            }
            else if (srcFunc == "i")
            {
                script += "setLevelI(" + param.SMUA.SrcLevel.ToString() + ")\n";
            }

            script += "mrtA." + msrtFunc + "(bufA1)\n";

            //-----------------------------------------------------------------------------------------
            // 若打IZ量VZ的Clamp電壓大於20，打完IZ後先降Clamp，以免多花20mS時間
            //-----------------------------------------------------------------------------------------
            if (Math.Abs(param.SMUA.MsrtClamp) > 20 && srcFunc == "i")
            {
                script += "setLimitV(8)\n";
            }

            if (param.SMUA.IsAutoTurnOff)
            {
                if (!param.IsTurnOffToZeroVolt)
                {
                    if (srcFunc == "v")
                    {
                        script += "setLevelV(0)\n";
                    }
                    else if (srcFunc == "i")
                    {
                        script += "setLevelI(0)\n";
                    }
                }
                else
                {
                    if (srcFunc == "v")
                    {
                        script += "setLevelV(0)\n";
                    }
                    else if (srcFunc == "i")
                    {
                        script += "setLevelI(0)\n";

                        script += "setFunc(1)\n";

                        script += "setLevelV(0)\n";
                    }
                }
            }

            if (isDeviceMsrtTimeStamps)
            {
                script += "printbuffer(1, " + param.SMUA.SweepPoints.ToString() + ", bufA1, bufA1.timestamps)\n";

                script += "bufA1.clear()\n";

                script += "setBufTimestamps(0)\n";
            }
            else
            {
                script += "printbuffer(1, " + param.SMUA.SweepPoints.ToString() + ", bufA1)\n";

                script += "bufA1.clear()\n";
            }

            script += "setMsrtCount(1)\n";

            script += "endscript\n";

            script += "num_" + param.Index.ToString() + ".source = nil";

            return script;
        }

        public static string PIV_DUAL(K2600ScriptSetting param)
        {
            string script = string.Empty;

            string srcRngAndComplScript = string.Empty;

            string srcFunc = string.Empty;

            string msrtFunc = string.Empty;

            double turnOffTime = 0.0d;

            //-----------------------------------------------------------------------------------------
            // Sweep List
            //-----------------------------------------------------------------------------------------
            string listName = string.Format("List_{0}", param.Index);

            uint sweepCnt = param.SMUA.SweepPoints;

            //---------------------------------------------------------------------------------------
            // [0] I Source;  [1] V Source
            int srcMode = GetSrcMsrtMode(param.SMUA.SrcMode, out srcFunc, out msrtFunc);    // [0] I Source;  [1] V Source

            double forceRange = Math.Abs(param.SMUA.SweepStop);

            // ===========================================================================
            // Source Range < 1 uA, SMU 會有 20~30 ms 的前置時間, 才會打出訊號
            // ===========================================================================
            if (forceRange < 0.000001)
            {
                forceRange = 0.000001;
            }

            // ===========================================================================
            // Fix SRC Current Range 利用 100uA Force Range 去量測 <100uA的Level
            // 降低under-shoot的產生
            // 高電壓檔位的量測，使用100uA的檔位去推動10uA，Rising會有問題產生
            // ===========================================================================
            if (param.SMUA.MsrtNPLC == 0.01d && param.SMUA.MsrtClamp < 20)
            {
                if (forceRange < 0.0001)
                {
                    forceRange = 0.0001;
                }
            }

            if (param.SMUA.MsrtClamp <= param.SMUA.MsrtBoundryLimit)
            {
                if (msrtFunc == "v")
                {
                    srcRngAndComplScript += "setLimitV(" + param.SMUA.MsrtClamp.ToString() + ")\n";

                    srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    srcRngAndComplScript += "setLimitI(" + param.SMUA.MsrtClamp.ToString() + ")\n";

                    srcRngAndComplScript += "setSorceRangeV(" + forceRange.ToString() + ")\n";
                }
            }
            else
            {
                if (msrtFunc == "v")
                {
                    srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";

                    srcRngAndComplScript += "setLimitV(" + param.SMUA.MsrtClamp.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    srcRngAndComplScript += "setSorceRangeV(" + forceRange.ToString() + ")\n";

                    srcRngAndComplScript += "setLimitI(" + param.SMUA.MsrtClamp.ToString() + ")\n";
                }
            }

            //-----------------------------------------------------------------------------------------
            // Test Sequence
            //-----------------------------------------------------------------------------------------
            script = "loadscript num_" + param.Index.ToString() + "\n";

            script += "setNPLC(" + param.SMUA.MsrtNPLC.ToString() + ")\n";

            if (msrtFunc == "v")
            {
                script += "setMeasureRangeV(" + param.SMUA.MsrtRange.ToString() + ")\n";

                //script += "setMeasureRangeI(" + item.ForceRange.ToString() + ")\n";
            }
            else if (msrtFunc == "i")
            {
                script += "setMeasureRangeI(" + param.SMUA.MsrtRange.ToString() + ")\n";

                //script += "setMeasureRangeV(" + item.ForceRange.ToString() + ")\n";
            }

            if (param.SMUB.IsEnableSmu)
            {
                #region >>> PIV For KEITHLEY 2612B <<<

                double srcRangeSmuB = param.SMUB.SrcRange;

                if (srcRangeSmuB < 2.0d)
                {
                    srcRangeSmuB = 2.0d;
                }

                script += "if getBFunc() ~= 1 then setBFunc(1) end\n";

                script += "setBNPLC(" + param.SMUB.MsrtNPLC.ToString() + ")\n";

                script += "setBLimitI(" + param.SMUB.MsrtClamp.ToString() + ")\n";

                script += "setBSorceRangeV(" + srcRangeSmuB.ToString() + ")\n";

                script += "setBMeasureRangeI(" + param.SMUB.MsrtRange.ToString() + ")\n";

                script += "setBLevelV(" + param.SMUB.SrcLevel.ToString() + ")\n";

                script += "setBOutput(1)\n";

                script += srcRngAndComplScript;
                //======================================
                // 切完Range後，在Set Range & Compliance
                //=======================================
                if (param.SMUA.WaitTime > 0)
                {
                    script += "delay(" + param.SMUA.WaitTime.ToString() + ")\n";
                }

                script += "if getOutput() ~= 1 then setOutput(1) end\n";

                script += "for i = 1, " + sweepCnt.ToString() + " do\n";

                script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";

                script += "setLevelI(" + listName + "[i])\n";

                script += "delay(" + param.SMUA.srcTime.ToString() + ")\n";

                script += "mrtIVsyncA(bufA1, bufA2)\n";  // overlappediv

                script += "mrtIsyncB(bufB1)\n";  // overlappedi

                script += "waitcomplete()\n";

                if (param.SMUA.SweepEndPulseTurnOffTime > 0)
                {
                    script += "setLevelI(0)\n";

                    script += "setFunc(1)\n";

                    script += "setLevelV(0)\n";

                    if (param.SMUA.SweepEndPulseTurnOffTime >= 0.001d)  // time unit = s
                    {
                        turnOffTime = param.SMUA.SweepEndPulseTurnOffTime - 0.0009d;
                    }

                    script += "delay(" + turnOffTime.ToString() + ")\n";
                }

                script += "end\n";

                script += "setBOutput(0)\n";

                if (param.SMUA.SweepEndPulseTurnOffTime == 0.0d)  // time unit = s
                {
                    script += "setLevelI(0)\n";

                    script += "setFunc(1)\n";

                    script += "setLevelV(0)\n";

                    if (forceRange >= 0.1)
                    {
                        // 防止切換 1A Range 時, 產生 Overshoot
                        script += "setSorceRangeI(0.01)\n";
                    }
                }

                //script += "setOutput(0)\n";

                script += "printbuffer(1, " + sweepCnt.ToString() + ", bufA1, bufA2, bufB1)\n";

                script += "bufA1.clear()\n";
                script += "bufA2.clear()\n";
                script += "bufB1.clear()\n";

                #endregion
            }
            else
            {
                #region >>> PIV For KEITHLEY Dual 2611B <<<

                script += srcRngAndComplScript;
                //======================================
                // 切完Range後，在Set Range & Compliance
                //=======================================
                if (param.SMUA.WaitTime > 0)
                {
                    script += "delay(" + param.SMUA.WaitTime.ToString() + ")\n";
                }

                script += "if getOutput() ~= 1 then setOutput(1) end\n";

                script += "digio.trigger[" + K2600Const.IO_SMU_TRIG_IN.ToString() + "].wait(0.2)\n";  // Time Out 200ms

                //script += "local listening = true\n";

                script += "for i = 1, " + sweepCnt.ToString() + " do\n";

                //script += "listening = true\n";

                script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";

                script += "setLevelI(" + listName + "[i])\n";

                script += "delay(" + param.SMUA.srcTime.ToString() + ")\n";

                script += "digio.trigger[" + K2600Const.IO_PM_RELAY1.ToString() + "].assert()\n";

                // script += "smua.measure.overlappediv(bufA1, bufA2)\n";

                script += "mrtIVsyncA(bufA1, bufA2)\n";

                script += "waitcomplete()\n";

                script += "digio.trigger[" + K2600Const.IO_SMU_TRIG_IN.ToString() + "].wait(0.2)\n";  // Time Out 100ms

                //script += "while listening do\n";

                //script += "listening = digio.trigger[" + K2600Const.IO_SMU_TRIG_IN.ToString() + "].wait(0.000001)\n";  // Time Out 100ms

                //script += "if listening then listening = false end\n";

                //script += "end\n";

                if (param.SMUA.SweepEndPulseTurnOffTime > 0)
                {
                    script += "setLevelI(0)\n";

                    script += "setFunc(1)\n";

                    script += "setLevelV(0)\n";

                    if (param.SMUA.SweepEndPulseTurnOffTime >= 0.001d)  // time unit = s
                    {
                        turnOffTime = param.SMUA.SweepEndPulseTurnOffTime - 0.0009d;
                    }

                    script += "delay(" + turnOffTime.ToString() + ")\n";
                }

                script += "end\n";

                // script += "digio.trigger[" + K2600Const.IO_PM_RELAY1.ToString() + "].assert()\n";

                script += "digio.trigger[" + K2600Const.IO_PM_RELAY1.ToString() + "].assert()\n";

                //script += "setOutput(0)\n";

                if (param.SMUA.SweepEndPulseTurnOffTime == 0.0d)  // time unit = s
                {
                    script += "setLevelI(0)\n";

                    script += "setFunc(1)\n";

                    script += "setLevelV(0)\n";

                    if (forceRange >= 0.1)
                    {
                        // 防止切換 1A Range 時, 產生 Overshoot
                        script += "setSorceRangeI(0.01)\n";
                    }
                }

                script += "printbuffer(1, bufA1.n, bufA1, bufA2)\n";

                script += "bufA1.clear()\n";

                script += "bufA2.clear()\n";

                //    script += "digio.trigger[" + K2600Const.IO_PM_RELAY1.ToString() + "].release()\n";

                // script += "digio.trigger[" + K2600Const.IO_PM_RELAY1.ToString() + "].release()\n";

                #endregion
            }

            script += "endscript\n";

            script += "num_" + param.Index.ToString() + ".source = nil";

            return script;
        }

        public static string PIV_SLAVE(K2600ScriptSetting param)
        {
            string script = string.Empty;

            string srcRngAndComplScript = string.Empty;

            string srcFunc = string.Empty;

            string msrtFunc = string.Empty;

            double complLimit = 0.0d;

            // [0] I Source;  [1] V Source
            int srcMode = 1;

            srcFunc = "v";

            msrtFunc = "i";

            srcMode = 1;

            complLimit = param.SMUA.MsrtBoundryLimit;

            //script += "smua.abort()\n";
            ////-----------------------------------------------------------------------------------------------------------------
            //// Parameter Setup
            //item.MsrtNPLC = 0.01d;

            //// script += srcRngAndComplScript;

            ////script += "smua.nvbuffer1.clear()\n";

            //script += "setMeasureRangeI(1e-6)\n";

            //script += "setNPLC(" + item.MsrtNPLC.ToString() + ")\n";

            //script += "if getOutput() ~= 1 then setOutput(1) end\n";

            ////-----------------------------------------------------------------------------------------------------------------
            //// Trigger Layer Setting

            //script += "smua.trigger.source.action = smua.DISABLE\n";
            //script += "smua.trigger.measure.action = smua.ENABLE\n";
            //script += "smua.trigger.endpulse.action = smua.DISABLE\n";   // SOURCE_IDLE
            //script += "smua.measure.count = 1\n";

            //script += "smua.trigger.measure." + msrtFunc + "(smua.nvbuffer1)\n";


            ////-----------------------------------------------------------------------------------------------------------------
            //// Arm & Trigger Layer Event Config
            //script += "smua.trigger.arm.count = 0\n";                      // configure the trigger count
            //script += "smua.trigger.count = 1\n";

            //script += "smua.trigger.arm.stimulus = digio.trigger[" + K2600Const.IO_PM_RELAY1.ToString() + "].EVENT_ID\n";

            ////  script += "smua.trigger.measure.stimulus = digio.trigger[" + K2600Const.IO_PM_RELAY1.ToString() + "].EVENT_ID\n";
            //script += "smua.trigger.endpulse.stimulus = smua.trigger.MEASURE_COMPLETE_EVENT_ID\n";
            //script += "digio.trigger[" + K2600Const.IO_SMU_TRIG_IN.ToString() + "].stimulus = smua.trigger.MEASURE_COMPLETE_EVENT_ID\n";

            //script += "smua.trigger.initiate()\n";

            //this._conn.SendCommand(script);

            //return;


            script = "loadscript num_" + param.Index.ToString() + "\n";

            script += "setSorceRangeV(" + param.SMUA.SrcRange.ToString() + ")\n";

            script += "setLimitI(" + param.SMUA.MsrtBoundryLimit.ToString() + ")\n";

            script += "setNPLC(" + param.SMUA.MsrtNPLC.ToString() + ")\n";

            script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";

            script += "setMeasureRangeI(" + param.SMUA.MsrtRange.ToString() + ")\n";

            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            script += "digio.trigger[" + K2600Const.IO_SMU_TRIG_IN.ToString() + "].assert()\n";  // 為了同步 先用 Trigger In 訊號讓兩個 Script 同步

            script += "local TrigIN = false\n";

            script += "local ioBreak\n";

            script += "setLevelV(" + param.SMUA.SrcLevel.ToString() + ")\n";

            script += "while true do\n";

            script += "ioBreak = digio.trigger[" + K2600Const.IO_PM_RELAY1.ToString() + "].wait(0.000001)\n";

            script += "if ioBreak then break end\n";

            script += "TrigIN = digio.trigger[" + K2600Const.IO_PM_RELAY1.ToString() + "].wait(0.000001)\n";

            //-------------------------------------------------------------------------------------------------
            // SMU Msrt
            script += "if TrigIN then\n";

            // script += "delay(" + item.ForceTime.ToString() + ")\n";

            script += "mrtA." + msrtFunc + "(bufA1)\n";

            script += "digio.trigger[" + K2600Const.IO_SMU_TRIG_IN.ToString() + "].assert()\n";

            script += "end\n";
            //-------------------------------------------------------------------------------------------------

            script += "end\n";

            script += "setOutput(0)\n";

            script += "printbuffer(1, bufA1.n, bufA1)\n";

            script += "bufA1.clear()\n";

            //  script += "digio.trigger[" + K2600Const.IO_SMU_TRIG_IN.ToString() + "].release()\n";

            script += "endscript\n";

            return script;
        }

        #endregion

        #region >>> Pulse Script <<<

        public static string PulsePIV_DUAL(K2600ScriptSetting param)
        {
            int numPoints = 10;
            double pulsePeriod = 0.01d;
            double pulseWidth = 0.001d;
            double start = 0.001d;
            double stop = 0.021d;
            double limitV = 8;
            double nplc = 0.003;

            double forceRange = Math.Abs(param.SMUA.SweepStop);

            string script = string.Empty;

            script += "loadscript num_" + param.Index.ToString() + "\n";

            //--------------------------------------------------------------
            // smua Config
            script += "smua.source.func	= smua.OUTPUT_DCAMPS\n";
            script += "smua.source.autorangei = smua.AUTORANGE_OFF\n";
            script += "smua.source.rangei = " + forceRange + "\n";
            script += "smua.source.leveli = 0\n";
            script += "smua.source.limitv = 1\n";

            script += "smua.measure.autozero = smua.AUTOZERO_OFF\n";
            script += "smua.measure.autorangev = smua.AUTORANGE_OFF\n";
            script += "smua.measure.rangev = " + limitV.ToString() + "\n";
            script += "smua.measure.nplc = " + nplc.ToString() + "\n";

            script += "smua.measure.delay = 0\n";

            script += "smua.nvbuffer1.clear()\n";
            script += "smua.nvbuffer1.collecttimestamps = 0\n";
            script += "smua.nvbuffer2.clear()\n";
            script += "smua.nvbuffer2.collecttimestamps = 0\n";
            //--------------------------------------------------------------
            // smub Config
            script += "smub.source.func	= smub.OUTPUT_DCVOLTS\n";
            script += "smub.source.autorangev = smua.AUTORANGE_OFF\n";
            script += "smub.source.rangev = " + forceRange + "\n";
            script += "smub.source.levelv " + param.SMUB.SrcLevel.ToString() + "\n";
            script += "smub.source.limiti " + param.SMUB.MsrtClamp.ToString() + "\n";

            script += "smub.measure.autozero = smub.AUTOZERO_OFF\n";
            script += "smub.measure.autorangei = smub.AUTORANGE_OFF\n";
            script += "smub.measure.rangei = " + param.SMUB.MsrtClamp.ToString() + "\n";
            script += "smub.measure.nplc = " + nplc.ToString() + "\n";

            script += "smub.measure.delay = 0\n";

            script += "smub.nvbuffer1.clear()\n";
            script += "smub.nvbuffer1.collecttimestamps = 0\n";
            script += "smub.nvbuffer2.clear()\n";
            script += "smub.nvbuffer2.collecttimestamps = 0\n";
            //--------------------------------------------------------------

            script += "trigger.timer[1].count = " + numPoints.ToString() + " > 1 and  " + numPoints.ToString() + " - 1 or 1\n";
            script += "trigger.timer[1].delay = " + pulsePeriod.ToString() + "\n";
            script += "trigger.timer[1].passthrough	= true\n";
            script += "trigger.timer[1].stimulus = smua.trigger.ARMED_EVENT_ID\n";

            script += "trigger.timer[2].count = 1\n";
            script += "trigger.timer[2].delay = " + pulseWidth.ToString() + " - (1/localnode.linefreq)*" + nplc.ToString() + " - 60e-6\n";
            script += "trigger.timer[2].passthrough	= false\n";
            script += "trigger.timer[2].stimulus = trigger.timer[1].EVENT_ID\n";

            script += "trigger.timer[3].count = 1\n";
            script += "trigger.timer[3].delay = " + pulseWidth.ToString() + "\n";
            script += "trigger.timer[3].passthrough	= false\n";
            script += "trigger.timer[3].stimulus = trigger.timer[1].EVENT_ID\n";

            //--------------------------------------------------------------
            // smua
            script += "smua.trigger.source.lineari(" + start.ToString() + ", " + stop.ToString() + ", " + numPoints.ToString() + ")\n";
            script += "smua.trigger.source.limitv = " + limitV.ToString() + "\n";
            script += "smua.trigger.measure.action = smua.ENABLE\n";
            script += "smua.trigger.measure.iv(smua.nvbuffer1, smua.nvbuffer2)\n";
            script += "smua.trigger.endpulse.action	= smua.SOURCE_IDLE\n";
            script += "smua.trigger.endsweep.action	= smua.SOURCE_IDLE\n";
            script += "smua.trigger.count = " + numPoints.ToString() + "\n";
            script += "smua.trigger.arm.stimulus = 0\n";
            script += "smua.trigger.source.stimulus	= trigger.timer[1].EVENT_ID\n";
            script += "smua.trigger.measure.stimulus = trigger.timer[2].EVENT_ID\n";
            script += "smua.trigger.endpulse.stimulus = trigger.timer[3].EVENT_ID\n";
            script += "smua.trigger.source.action = smua.ENABLE\n";
            //--------------------------------------------------------------
            //smub
            script += "smub.trigger.source.linearv(5, 5, " + numPoints.ToString() + ")\n";
            script += "smub.trigger.source.limiti = " + param.SMUB.MsrtRange.ToString() + "\n";
            script += "smub.trigger.measure.action = smub.ENABLE\n";
            script += "smub.trigger.measure.iv(smub.nvbuffer1, smub.nvbuffer2)\n";
            script += "smub.trigger.endpulse.action	= smub.SOURCE_IDLE\n";
            script += "smub.trigger.endsweep.action	= smub.SOURCE_IDLE\n";
            script += "smub.trigger.count = " + numPoints.ToString() + "\n";
            script += "smub.trigger.arm.stimulus = 0\n";
            script += "smub.trigger.source.stimulus	= trigger.timer[1].EVENT_ID\n";
            script += "smub.trigger.measure.stimulus = trigger.timer[2].EVENT_ID\n";
            script += "smub.trigger.endpulse.stimulus = trigger.timer[3].EVENT_ID\n";
            script += "smub.trigger.source.action = smub.ENABLE\n";

            //--------------------------------------------------------------
            // smua & smub Trigger
            script += "smua.source.output = smua.OUTPUT_ON\n";
            script += "smub.source.output = smub.OUTPUT_ON\n";

            script += "smua.trigger.initiate()\n";
            script += "smub.trigger.initiate()\n";

            script += "waitcomplete()\n";
            script += "smua.source.output = smua.OUTPUT_OFF\n";
            script += "smub.source.output = smub.OUTPUT_OFF\n";

            // smua & smub print result

            //script += "print(\"Time\tVoltage\tCurrent\")\n";
            //script += "for x=1,smua.nvbuffer1.n do\n";
            //script += "print(smua.nvbuffer2[x], smua.nvbuffer1[x])\n";
            //script += "end\n";

            script += "endscript\n";
            script += "num_" + param.Index.ToString() + ".source = nil\n";

            return script;
        }

        public static string PulseI_SMUA(K2600ScriptSetting param)
        {
            //region < 0, DC Mode
            double nplc = 0.003;
            double value = param.SMUA.SrcLevel;
            double limitV = param.SMUA.MsrtClamp;
            double duty = 0.0d;
            double pulseWidth = param.SMUA.srcTime;
            double period = pulseWidth * (1 - duty) / duty;

            string script = string.Empty;

            script += "loadscript num_" + param.Index.ToString() + "\n";

            script += "if getFunc() ~= 0 then setFunc(0) end\n";
            script += "setSorceAotoRangeI(smua.AUTORANGE_OFF)\n";
            script += "setSorceRangeI(" + value.ToString() + ")\n";
            script += "setLevelI(0)\n";
            script += "setLimitV(1)\n";

            script += "setMeasureRangeV(" + limitV.ToString() + ")\n";
            script += "setNPLC(" + nplc.ToString() + ")\n";

            script += "setMeasureDelay(0)\n";

            script += "T1_Count(1)\n";
            script += "T1_Delay(" + pulseWidth.ToString() + " - (1/localnode.linefreq)*" + nplc.ToString() + " - 60e-6)\n";
            script += "T1_Passthrough(false)\n";
            script += "T1_Stimulus(smua_Trigger.ARMED_EVENT_ID)\n";

            script += "T2_Count(1)\n";
            script += "T2_Delay(" + pulseWidth.ToString() + ")\n";
            script += "T2_Passthrough(false)\n";
            script += "T2_Passthrough(smua_Trigger.ARMED_EVENT_ID)\n";

            script += "smua_Trigger_Source.listi({" + value.ToString() + "})\n";
            script += "smua_Trigger_LimitV(" + limitV.ToString() + ")\n";
            script += "smua_Trigger_Action(smua.ENABLE)\n";
            script += "smua_Trigger_Measure.v(bufA1)\n";
            script += "smua_Trigger_EndPulse_Action(smua.SOURCE_IDLE)\n";
            script += "smua_Trigger_Count(1)\n";
            script += "smua_Trigger_Arm_Stimulus(0)\n";
            script += "smua_Trigger_Source_Stimulus(smua_Trigger.ARMED_EVENT_ID)\n";
            script += "smua_Trigger_Measure_Stimulus(trigger_Timer1.EVENT_ID)\n";
            script += "smua_Trigger_EndPulse_Stimulus(trigger_Timer2.EVENT_ID)\n";
            script += "smua_Trigger_Source_Action(smua.ENABLE)\n";
            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            if (param.SMUA.WaitTime > 0)
            {
                script += "delay(" + param.SMUA.WaitTime.ToString() + ")\n";
            }

            script += "smua_Trigger.initiate()\n";

            script += "waitcomplete()\n";
            script += "setOutput(0)\n";

            script += "delay(" + (period - pulseWidth).ToString() + ")\n";

            script += "printbuffer(1, bufA1.n, bufA1)\n";
            script += "bufA1.clear()\n";

            script += "endscript\n";
            script += "num_" + param.Index.ToString() + ".source = nil\n";

            return script;
        }

        public static string PulseContinuousI_SMUA(K2600ScriptSetting param)
        {
            //region < 0, DC Mode
            double nplc = 0.003;

            // double limitV = item.MsrtProtection;
            double tOn = param.SMUA.srcTime - (1 / 60.0d * nplc) - 450e-6;
            double tOff = param.SMUA.TurnOffTime / 1000.0d;
            uint pulseCnt = param.SMUA.SweepPoints;
            //double duty = region < 0 ? 1.0d : this._pulseDuty[region];
            //double pulsePeriod = region < 0 ? 0 : pulseWidth * (1 - duty) / duty;

            if (tOn <= 0.1e-3)
            {
                tOn = 0.1e-3;
            }

            string script = string.Empty;

            string srcRngAndComplScript = string.Empty;

            string srcFunc;

            string msrtFunc;

            int srcMode = GetSrcMsrtMode(param.SMUA.SrcMode, out srcFunc, out msrtFunc);      // [0] I Source;  [1] V Source

            double forceRange = param.SMUA.SrcRange;

            // ===========================================================================
            // Source Range < 1 uA, SMU 會有 20~30 ms 的前置時間, 才會打出訊號
            // ===========================================================================
            if (srcFunc == "i")
            {
                if (forceRange < 0.000001)
                {
                    forceRange = 0.000001;
                }
            }
            else
            {
                if (forceRange < 2.0d)
                {
                    forceRange = 2.0d;
                }
            }

            // ===========================================================================
            // Fix SRC Current Range 利用 100uA Force Range 去量測 <100uA的Level
            // 降低under-shoot的產生
            // 高電壓檔位的量測，使用100uA的檔位去推動10uA，Rising會有問題產生
            // ===========================================================================

            if (param.SMUA.MsrtNPLC == 0.01d && param.SMUA.MsrtClamp < 20)
            {
                if (forceRange < 0.0001)
                {
                    forceRange = 0.0001;
                }
            }

            if (param.SMUA.MsrtClamp <= param.SMUA.MsrtBoundryLimit)
            {
                if (msrtFunc == "v")
                {
                    srcRngAndComplScript += "setLimitV(" + param.SMUA.MsrtClamp.ToString() + ")\n";

                    srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    srcRngAndComplScript += "setLimitI(" + param.SMUA.MsrtClamp.ToString() + ")\n";

                    srcRngAndComplScript += "setSorceRangeV(" + forceRange.ToString() + ")\n";
                }
            }
            else
            {
                if (msrtFunc == "v")
                {
                    srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";

                    srcRngAndComplScript += "setLimitV(" + param.SMUA.MsrtClamp.ToString() + ")\n";
                }
                else if (msrtFunc == "i")
                {
                    srcRngAndComplScript += "setSorceRangeV(" + forceRange.ToString() + ")\n";

                    srcRngAndComplScript += "setLimitI(" + param.SMUA.MsrtClamp.ToString() + ")\n";
                }
            }

            //-----------------------------------------------------------------------------------------
            // Test Sequence
            //-----------------------------------------------------------------------------------------
            script = "loadscript num_" + param.Index.ToString() + "\n";

            script += "setNPLC(" + nplc.ToString() + ")\n";

            if (msrtFunc == "v")
            {
                script += "setMeasureRangeV(" + param.SMUA.MsrtRange.ToString() + ")\n";
            }
            else if (msrtFunc == "i")
            {
                script += "setMeasureRangeI(" + param.SMUA.MsrtRange.ToString() + ")\n";
            }


            script += srcRngAndComplScript;

            //======================================
            // 切完Range後，在Set Range & Compliance
            //=======================================

            script += "delay(0.001)\n";

            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            script += "for i = 1, " + pulseCnt.ToString() + " do\n";

            script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";

            if (srcFunc == "v")
            {
                script += "setLevelV(" + param.SMUA.SrcLevel.ToString() + ")\n";
            }
            else if (srcFunc == "i")
            {
                script += "setLevelI(" + param.SMUA.SrcLevel.ToString() + ")\n";
            }

            script += "delay(" + tOn.ToString() + ")\n";

            script += "mrtA." + msrtFunc + "(bufA1)\n";

            script += "setLevelI(0)\n";

            script += "setFunc(1)\n";

            script += "setLevelV(0)\n";

            script += "delay(" + (tOff - 0.0009d).ToString() + ")\n";

            script += "end\n";

            script += "printbuffer(1, " + pulseCnt.ToString() + ", bufA1)\n";

            script += "bufA1.clear()\n";

            if (forceRange >= 0.1)
            {
                // 防止切換 1A Range 時, 產生 Overshoot
                script += "setSorceRangeI(0.01)\n";
            }

            script += "endscript\n";

            script += "num_" + param.Index.ToString() + ".source = nil";

            return script;
        }

        public static string PulseTermSweep_SMUA(K2600ScriptSetting param, EK2600IOTriggerSynMode mode, uint devicePin, uint syschroneziPin, uint[] monitorPin)
        {
            uint numPoints = param.SMUA.SweepPoints;
            double pulsePeriod = param.SMUA.srcTime + param.SMUA.TurnOffTime;
            double pulseWidth = param.SMUA.srcTime;
            double start = param.SMUA.SweepStart;
            double stop = param.SMUA.SweepStop;
            double limiti = param.SMUA.MsrtClamp;
            double nplc = param.SMUA.MsrtNPLC;
            string script = string.Empty;

            script += "loadscript num_" + param.Index + "\n";

            //-----------------------------------------------------------------------------------------------
            // Common Script
            //-----------------------------------------------------------------------------------------------
            script += "smua.measure.autozero = smua.AUTOZERO_ONCE\n";
            script += "smua.measure.nplc = " + nplc.ToString() + "\n";
            script += "smua.measure.delay = 0\n";
            script += "smua.nvbuffer1.clear()\n";
            script += "smua.nvbuffer1.collecttimestamps= 0\n";
            script += "smua.nvbuffer2.clear()\n";
            script += "smua.nvbuffer2.collecttimestamps= 0\n";

            script += "trigger.timer[1].count = " + numPoints.ToString() + " > 1 and  " + numPoints.ToString() + " - 1 or 1\n";
            script += "trigger.timer[1].delay = " + pulsePeriod.ToString() + "\n";
            script += "trigger.timer[1].passthrough	= true\n";
            script += "trigger.timer[2].count = 1\n";
            script += "trigger.timer[2].delay = " + pulseWidth.ToString() + " - (1/localnode.linefreq)*" + nplc.ToString() + " - 60e-6\n";
            script += "trigger.timer[2].passthrough	= false\n";
            script += "trigger.timer[2].stimulus = trigger.timer[1].EVENT_ID\n";
            script += "trigger.timer[3].count = 1\n";
            script += "trigger.timer[3].delay = " + pulseWidth.ToString() + "\n";
            script += "trigger.timer[3].passthrough	= false\n";
            script += "trigger.timer[3].stimulus = trigger.timer[1].EVENT_ID\n";

            script += "smua.trigger.measure.action = smua.ENABLE\n";
            script += "smua.trigger.endsweep.action	= smua.SOURCE_IDLE\n";
            script += "smua.trigger.count = " + numPoints.ToString() + "\n";
            script += "smua.trigger.arm.stimulus = 0\n";
            script += "smua.trigger.source.stimulus	= trigger.timer[1].EVENT_ID\n";
            script += "smua.trigger.measure.stimulus = trigger.timer[2].EVENT_ID\n";
            script += "smua.trigger.endpulse.stimulus = trigger.timer[3].EVENT_ID\n";
            script += "smua.trigger.source.action = smua.ENABLE\n";

            //if (param.TermianlFuncType == ETermianlFuncType.Bias)
            //{
            //    script += "smua.trigger.endpulse.action	= smua.SOURCE_HOLD\n";
            //}
            //else if (param.TermianlFuncType == ETermianlFuncType.Sweep)
            //{
            //    script += "smua.trigger.endpulse.action	= smua.SOURCE_IDLE\n";
            //}

            script += "smua.trigger.endpulse.action	= " + ((uint)param.SMUA.SweepEndPulseAction).ToString() + "\n";

            //-----------------------------------------------------------------------------------------------
            // IO Trigger Synchronize Setting
            //-----------------------------------------------------------------------------------------------
            if (mode == EK2600IOTriggerSynMode.Single)
            {
                script += "trigger.timer[1].stimulus = smua.trigger.ARMED_EVENT_ID\n";
            }
            else if (mode == EK2600IOTriggerSynMode.Master)
            {
                script += "digio.trigger[" + devicePin.ToString() + "].stimulus = smua.trigger.ARMED_EVENT_ID\n";
                script += "trigger.timer[1].stimulus = smua.trigger.ARMED_EVENT_ID\n";
            }
            else if (mode == EK2600IOTriggerSynMode.Slave)
            {
                script += "digio.trigger[" + devicePin.ToString() + "].stimulus = smua.trigger.ARMED_EVENT_ID\n";
                script += "trigger.timer[1].stimulus = digio.trigger[" + syschroneziPin.ToString() + "].EVENT_ID\n";
            }

            //-----------------------------------------------------------------------------------------------
            // FIMV or FVMI Setting
            //-----------------------------------------------------------------------------------------------
            if (param.SMUA.SrcMode == EK2600SrcMode.V_Source)   //  FVMI
            {
                script += "smua.source.func	= smua.OUTPUT_DCVOLTS\n";
                script += "smua.source.autorangev = smua.AUTORANGE_OFF\n";
                script += "smua.source.rangev = math.max(math.abs(" + start.ToString() + "), math.abs(" + stop.ToString() + "))\n";
                script += "smua.source.levelv = 0\n";
                script += "smua.source.limiti = 0.1\n";

                script += "smua.measure.autorangei = smua.AUTORANGE_OFF\n";
                script += "smua.measure.rangei = " + limiti.ToString() + "\n";

                if (param.SMUA.SweepMode == EK2600SweepMode.Linear)
                {
                    script += "smua.trigger.source.linearv(" + start.ToString() + ", " + stop.ToString() + ", " + numPoints.ToString() + ")\n";
                }
                else if (param.SMUA.SweepMode == EK2600SweepMode.Log)
                {
                    script += "smua.trigger.source.logv(" + start.ToString() + ", " + stop.ToString() + ", " + numPoints.ToString() + ",0)\n";
                }
                else if (param.SMUA.SweepMode == EK2600SweepMode.Custom)
                {
                    script += "smua.trigger.source.listv({";

                    for (int i = 0; i < param.SMUA.SweepCustomList.Count; i++)
                    {
                        script += param.SMUA.SweepCustomList[i].ToString();

                        if (i != param.SMUA.SweepCustomList.Count - 1)
                        {
                            script += ",";
                        }
                    }

                    script += "})\n";
                }

                script += "smua.trigger.source.limiti = " + limiti.ToString() + "\n";

                script += "smua.trigger.measure.i(smua.nvbuffer1)\n";
            }
            else if (param.SMUA.SrcMode == EK2600SrcMode.I_Source)   // FIMV
            {
                script += "smua.source.func	= smua.OUTPUT_DCAMPS\n";
                script += "smua.source.autorangei = smua.AUTORANGE_OFF\n";
                script += "smua.source.rangei = math.max(math.abs(" + start.ToString() + "), math.abs(" + stop.ToString() + "))\n";
                script += "smua.source.leveli = 0\n";
                script += "smua.source.limitv = 1\n";

                script += "smua.measure.autorangev = smua.AUTORANGE_OFF\n";
                script += "smua.measure.rangev = " + limiti.ToString() + "\n";

                if (param.SMUA.SweepMode == EK2600SweepMode.Linear)
                {
                    script += "smua.trigger.source.lineari(" + start.ToString() + ", " + stop.ToString() + ", " + numPoints.ToString() + ")\n";
                }
                else if (param.SMUA.SweepMode == EK2600SweepMode.Log)
                {
                    script += "smua.trigger.source.logi(" + start.ToString() + ", " + stop.ToString() + ", " + numPoints.ToString() + ",0)\n";
                }
                else if (param.SMUA.SweepMode == EK2600SweepMode.Custom)
                {
                    script += "smua.trigger.source.listi({";

                    for (int i = 0; i < param.SMUA.SweepCustomList.Count; i++)
                    {
                        script += param.SMUA.SweepCustomList[i].ToString();

                        if (i != param.SMUA.SweepCustomList.Count - 1)
                        {
                            script += ",";
                        }
                    }

                    script += "})\n";
                }

                script += "smua.trigger.source.limitv = " + limiti.ToString() + "\n";

                script += "smua.trigger.measure.v(smua.nvbuffer1)\n";
            }

            //-----------------------------------------------------------------------------------------------
            // Trigger
            //-----------------------------------------------------------------------------------------------
            script += "smua.source.output = smua.OUTPUT_ON\n";

            if (mode == EK2600IOTriggerSynMode.Master)
            {
                script += "while true do\n";
                script += "local isAllReady = 0\n";

                for (uint i = 0; i < monitorPin.Length; i++)
                {
                    uint ioPin = monitorPin[i];
                    script += "isAllReady = isAllReady + digio.readbit(" + ioPin.ToString() + ")\n";
                }

                script += "if isAllReady == 0 then break end\n";
                script += "end\n";
            }
            else if (mode == EK2600IOTriggerSynMode.Slave)
            {
                script += "while true do\n";
                script += "if digio.readbit(" + syschroneziPin.ToString() + ") == 1 then break end\n";
                script += "end\n";
            }

            script += "smua.trigger.initiate()\n";
            script += "waitcomplete()\n";
            script += "smua.source.output = smua.OUTPUT_OFF\n";

            //-----------------------------------------------------------------------------------------------
            // Wiat Synchronize Complete
            //-----------------------------------------------------------------------------------------------
            if (mode == EK2600IOTriggerSynMode.Master)
            {
                script += "digio.trigger[" + devicePin.ToString() + "].release()\n";

                script += "while true do\n";
                script += "local isAllReady2 = 0\n";

                for (uint i = 0; i < monitorPin.Length; i++)
                {
                    script += "isAllReady2 = isAllReady2 + digio.readbit(" + monitorPin[i].ToString() + ")\n";
                }

                script += "if isAllReady2 == " + monitorPin.Length.ToString() + " then break end\n";
                script += "end\n";
            }
            else if (mode == EK2600IOTriggerSynMode.Slave)
            {
                script += "digio.trigger[" + devicePin.ToString() + "].release()\n";
            }

            //-----------------------------------------------------------------------------------------------
            // Print Data
            //-----------------------------------------------------------------------------------------------
            script += "printbuffer(1, smua.nvbuffer1.n, smua.nvbuffer1)\n";
            script += "collectgarbage()\n";
            script += "endscript\n";
            script += "num_" + param.Index + ".source = nil\n";

            return script;
        }

        public static string PulseTermSweep_SMUB(K2600ScriptSetting param, EK2600IOTriggerSynMode mode, uint devicePin, uint syschroneziPin, uint[] monitorPin)
        {
            uint numPoints = param.SMUB.SweepPoints;
            double pulsePeriod = param.SMUB.srcTime + param.SMUB.TurnOffTime;
            double pulseWidth = param.SMUB.srcTime;
            double start = param.SMUB.SweepStart;
            double stop = param.SMUB.SweepStop;
            double limiti = param.SMUB.MsrtClamp;
            double nplc = param.SMUB.MsrtNPLC;
            string script = string.Empty;

            script += "loadscript num_" + param.Index + "\n";

            //-----------------------------------------------------------------------------------------------
            // Common Script
            //-----------------------------------------------------------------------------------------------
            script += "smub.measure.autozero = smub.AUTOZERO_ONCE\n";
            script += "smub.measure.nplc = " + nplc.ToString() + "\n";
            script += "smub.measure.delay = 0\n";
            script += "smub.nvbuffer1.clear()\n";
            script += "smub.nvbuffer1.collecttimestamps= 0\n";
            script += "smub.nvbuffer2.clear()\n";
            script += "smub.nvbuffer2.collecttimestamps= 0\n";

            script += "trigger.timer[1].count = " + numPoints.ToString() + " > 1 and  " + numPoints.ToString() + " - 1 or 1\n";
            script += "trigger.timer[1].delay = " + pulsePeriod.ToString() + "\n";
            script += "trigger.timer[1].passthrough	= true\n";
            script += "trigger.timer[2].count = 1\n";
            script += "trigger.timer[2].delay = " + pulseWidth.ToString() + " - (1/localnode.linefreq)*" + nplc.ToString() + " - 60e-6\n";
            script += "trigger.timer[2].passthrough	= false\n";
            script += "trigger.timer[2].stimulus = trigger.timer[1].EVENT_ID\n";
            script += "trigger.timer[3].count = 1\n";
            script += "trigger.timer[3].delay = " + pulseWidth.ToString() + "\n";
            script += "trigger.timer[3].passthrough	= false\n";
            script += "trigger.timer[3].stimulus = trigger.timer[1].EVENT_ID\n";

            script += "smub.trigger.measure.action = smub.ENABLE\n";
            script += "smub.trigger.endsweep.action	= smub.SOURCE_IDLE\n";
            script += "smub.trigger.count = " + numPoints.ToString() + "\n";
            script += "smub.trigger.arm.stimulus = 0\n";
            script += "smub.trigger.source.stimulus	= trigger.timer[1].EVENT_ID\n";
            script += "smub.trigger.measure.stimulus = trigger.timer[2].EVENT_ID\n";
            script += "smub.trigger.endpulse.stimulus = trigger.timer[3].EVENT_ID\n";
            script += "smub.trigger.source.action = smub.ENABLE\n";

            //if (param.TermianlFuncType == ETermianlFuncType.Bias)
            //{
            //    script += "smub.trigger.endpulse.action	= smub.SOURCE_HOLD\n";
            //}
            //else if (param.TermianlFuncType == ETermianlFuncType.Sweep)
            //{
            //    script += "smub.trigger.endpulse.action	= smub.SOURCE_IDLE\n";
            //}

            script += "smub.trigger.endpulse.action	= " + ((uint)param.SMUB.SweepEndPulseAction).ToString() + "\n";

            //-----------------------------------------------------------------------------------------------
            // IO Trigger Synchronize Setting
            //-----------------------------------------------------------------------------------------------
            if (mode == EK2600IOTriggerSynMode.Single)
            {
                script += "trigger.timer[1].stimulus = smub.trigger.ARMED_EVENT_ID\n";
            }
            else if (mode == EK2600IOTriggerSynMode.Master)
            {
                script += "digio.trigger[" + devicePin.ToString() + "].stimulus = smub.trigger.ARMED_EVENT_ID\n";
                script += "trigger.timer[1].stimulus = smub.trigger.ARMED_EVENT_ID\n";
            }
            else if (mode == EK2600IOTriggerSynMode.Slave)
            {
                script += "digio.trigger[" + devicePin.ToString() + "].stimulus = smub.trigger.ARMED_EVENT_ID\n";
                script += "trigger.timer[1].stimulus = digio.trigger[" + syschroneziPin.ToString() + "].EVENT_ID\n";
            }

            //-----------------------------------------------------------------------------------------------
            // FIMV or FVMI Setting
            //-----------------------------------------------------------------------------------------------
            if (param.SMUB.SrcMode == EK2600SrcMode.V_Source)
            {
                script += "smub.source.func	= smub.OUTPUT_DCVOLTS\n";
                script += "smub.source.autorangev = smub.AUTORANGE_OFF\n";
                script += "smub.source.rangev = math.max(math.abs(" + start.ToString() + "), math.abs(" + stop.ToString() + "))\n";
                script += "smub.source.levelv = 0\n";
                script += "smub.source.limiti = 0.1\n";

                script += "smub.measure.autorangei = smub.AUTORANGE_OFF\n";
                script += "smub.measure.rangei = " + limiti.ToString() + "\n";

                if (param.SMUB.SweepMode == EK2600SweepMode.Linear)
                {
                    script += "smub.trigger.source.linearv(" + start.ToString() + ", " + stop.ToString() + ", " + numPoints.ToString() + ")\n";
                }
                else if (param.SMUB.SweepMode == EK2600SweepMode.Log)
                {
                    script += "smub.trigger.source.logv(" + start.ToString() + ", " + stop.ToString() + ", " + numPoints.ToString() + ",0)\n";
                }
                else if (param.SMUB.SweepMode == EK2600SweepMode.Custom)
                {
                    script += "smub.trigger.source.listv({";

                    for (int i = 0; i < param.SMUB.SweepCustomList.Count; i++)
                    {
                        script += param.SMUB.SweepCustomList[i].ToString();

                        if (i != param.SMUB.SweepCustomList.Count - 1)
                        {
                            script += ",";
                        }
                    }

                    script += "})\n";
                }

                script += "smub.trigger.source.limiti = " + limiti.ToString() + "\n";

                script += "smub.trigger.measure.i(smub.nvbuffer1)\n";
            }
            else if (param.SMUB.SrcMode == EK2600SrcMode.I_Source)
            {
                script += "smub.source.func	= smub.OUTPUT_DCAMPS\n";
                script += "smub.source.autorangei = smub.AUTORANGE_OFF\n";
                script += "smub.source.rangei = math.max(math.abs(" + start.ToString() + "), math.abs(" + stop.ToString() + "))\n";
                script += "smub.source.leveli = 0\n";
                script += "smub.source.limitv = 1\n";

                script += "smub.measure.autorangev = smub.AUTORANGE_OFF\n";
                script += "smub.measure.rangev = " + limiti.ToString() + "\n";

                if (param.SMUB.SweepMode == EK2600SweepMode.Linear)
                {
                    script += "smub.trigger.source.lineari(" + start.ToString() + ", " + stop.ToString() + ", " + numPoints.ToString() + ")\n";
                }
                else if (param.SMUB.SweepMode == EK2600SweepMode.Log)
                {
                    script += "smub.trigger.source.logi(" + start.ToString() + ", " + stop.ToString() + ", " + numPoints.ToString() + ",0)\n";
                }
                else if (param.SMUB.SweepMode == EK2600SweepMode.Custom)
                {
                    script += "smub.trigger.source.listi({";

                    for (int i = 0; i < param.SMUB.SweepCustomList.Count; i++)
                    {
                        script += param.SMUB.SweepCustomList[i].ToString();

                        if (i != param.SMUB.SweepCustomList.Count - 1)
                        {
                            script += ",";
                        }
                    }

                    script += "})\n";
                }

                script += "smub.trigger.source.limitv = " + limiti.ToString() + "\n";

                script += "smub.trigger.measure.v(smub.nvbuffer1)\n";
            }

            //-----------------------------------------------------------------------------------------------
            // Trigger
            //-----------------------------------------------------------------------------------------------
            script += "smub.source.output = smub.OUTPUT_ON\n";

            if (mode == EK2600IOTriggerSynMode.Master)
            {
                script += "while true do\n";
                script += "local isAllReady = 0\n";

                for (uint i = 0; i < monitorPin.Length; i++)
                {
                    uint ioPin = monitorPin[i];
                    script += "isAllReady = isAllReady + digio.readbit(" + ioPin.ToString() + ")\n";
                }

                script += "if isAllReady == 0 then break end\n";
                script += "end\n";
            }
            else if (mode == EK2600IOTriggerSynMode.Slave)
            {
                script += "while true do\n";
                script += "if digio.readbit(" + syschroneziPin.ToString() + ") == 1 then break end\n";
                script += "end\n";
            }

            script += "smub.trigger.initiate()\n";
            script += "waitcomplete()\n";
            script += "smub.source.output = smub.OUTPUT_OFF\n";

            //-----------------------------------------------------------------------------------------------
            // Wiat Synchronize Complete
            //-----------------------------------------------------------------------------------------------
            if (mode == EK2600IOTriggerSynMode.Master)
            {
                script += "digio.trigger[" + devicePin.ToString() + "].release()\n";

                script += "while true do\n";
                script += "local isAllReady2 = 0\n";

                for (uint i = 0; i < monitorPin.Length; i++)
                {
                    script += "isAllReady2 = isAllReady2 + digio.readbit(" + monitorPin[i].ToString() + ")\n";
                }

                script += "if isAllReady2 == " + monitorPin.Length.ToString() + " then break end\n";
                script += "end\n";
            }
            else if (mode == EK2600IOTriggerSynMode.Slave)
            {
                script += "digio.trigger[" + devicePin.ToString() + "].release()\n";
            }

            //-----------------------------------------------------------------------------------------------
            // Print Data
            //-----------------------------------------------------------------------------------------------
            script += "printbuffer(1, smub.nvbuffer1.n, smub.nvbuffer1)\n";
            script += "collectgarbage()\n";
            script += "endscript\n";
            script += "num_" + param.Index + ".source = nil\n";

            return script;
        }

        public static string PulseTermSweep_DUAL(K2600ScriptSetting param, EK2600IOTriggerSynMode mode, uint devicePin, uint syschroneziPin, uint[] monitorPin)
        {
            uint numPoints = param.SMUA.SweepPoints;
            double pulsePeriod = param.SMUA.srcTime + param.SMUA.TurnOffTime;
            double pulseWidth = param.SMUA.srcTime;
            double nplc = param.SMUA.MsrtNPLC;

            double startA = param.SMUA.SweepStart;
            double stopA = param.SMUA.SweepStop;
            double limitiA = param.SMUA.MsrtClamp;

            double startB = param.SMUB.SweepStart;
            double stopB = param.SMUB.SweepStop;
            double limitiB = param.SMUB.MsrtClamp;

            string script = string.Empty;

            script += "loadscript num_" + param.Index + "\n";

            //-----------------------------------------------------------------------------------------------
            // Common Script
            //-----------------------------------------------------------------------------------------------
            script += "smua.measure.autozero = smua.AUTOZERO_ONCE\n";
            script += "smub.measure.autozero = smub.AUTOZERO_ONCE\n";
            script += "smua.measure.nplc = " + nplc.ToString() + "\n";
            script += "smub.measure.nplc = " + nplc.ToString() + "\n";
            script += "smua.measure.delay = 0\n";
            script += "smub.measure.delay = 0\n";
            script += "smua.nvbuffer1.clear()\n";
            script += "smub.nvbuffer1.clear()\n";
            script += "smua.nvbuffer1.collecttimestamps= 0\n";
            script += "smub.nvbuffer1.collecttimestamps= 0\n";
            script += "smua.nvbuffer2.clear()\n";
            script += "smub.nvbuffer2.clear()\n";
            script += "smua.nvbuffer2.collecttimestamps= 0\n";
            script += "smub.nvbuffer2.collecttimestamps= 0\n";

            script += "trigger.timer[1].count = " + numPoints.ToString() + " > 1 and  " + numPoints.ToString() + " - 1 or 1\n";
            script += "trigger.timer[1].delay = " + pulsePeriod.ToString() + "\n";
            script += "trigger.timer[1].passthrough	= true\n";
            script += "trigger.timer[2].count = 1\n";
            script += "trigger.timer[2].delay = " + pulseWidth.ToString() + " - (1/localnode.linefreq)*" + nplc.ToString() + " - 60e-6\n";
            script += "trigger.timer[2].passthrough	= false\n";
            script += "trigger.timer[2].stimulus = trigger.timer[1].EVENT_ID\n";
            script += "trigger.timer[3].count = 1\n";
            script += "trigger.timer[3].delay = " + pulseWidth.ToString() + "\n";
            script += "trigger.timer[3].passthrough	= false\n";
            script += "trigger.timer[3].stimulus = trigger.timer[1].EVENT_ID\n";

            script += "smua.trigger.measure.action = smua.ENABLE\n";
            script += "smub.trigger.measure.action = smub.ENABLE\n";
            script += "smua.trigger.endsweep.action	= smua.SOURCE_IDLE\n";
            script += "smub.trigger.endsweep.action	= smub.SOURCE_IDLE\n";
            script += "smua.trigger.count = " + numPoints.ToString() + "\n";
            script += "smub.trigger.count = " + numPoints.ToString() + "\n";
            script += "smua.trigger.arm.stimulus = 0\n";
            script += "smub.trigger.arm.stimulus = 0\n";
            script += "smua.trigger.source.stimulus	= trigger.timer[1].EVENT_ID\n";
            script += "smub.trigger.source.stimulus	= trigger.timer[1].EVENT_ID\n";
            script += "smua.trigger.measure.stimulus = trigger.timer[2].EVENT_ID\n";
            script += "smub.trigger.measure.stimulus = trigger.timer[2].EVENT_ID\n";
            script += "smua.trigger.endpulse.stimulus = trigger.timer[3].EVENT_ID\n";
            script += "smub.trigger.endpulse.stimulus = trigger.timer[3].EVENT_ID\n";
            script += "smua.trigger.source.action = smua.ENABLE\n";
            script += "smub.trigger.source.action = smub.ENABLE\n";

            //if (param.TermianlFuncType == ETermianlFuncType.Bias)
            //{
            //    script += "smua.trigger.endpulse.action	= smua.SOURCE_HOLD\n";
            //}
            //else if (param.TermianlFuncType == ETermianlFuncType.Sweep)
            //{
            //    script += "smua.trigger.endpulse.action	= smua.SOURCE_IDLE\n";
            //}

            //if (itemB.TermianlFuncType == ETermianlFuncType.Bias)
            //{
            //    script += "smub.trigger.endpulse.action	= smub.SOURCE_HOLD\n";
            //}
            //else if (itemB.TermianlFuncType == ETermianlFuncType.Sweep)
            //{
            //    script += "smub.trigger.endpulse.action	= smub.SOURCE_IDLE\n";
            //}

            script += "smua.trigger.endpulse.action	= " + ((uint)param.SMUA.SweepEndPulseAction).ToString() + "\n";
            script += "smub.trigger.endpulse.action	= " + ((uint)param.SMUB.SweepEndPulseAction).ToString() + "\n";

            //-----------------------------------------------------------------------------------------------
            // IO Trigger Synchronize Setting
            //-----------------------------------------------------------------------------------------------
            if (mode == EK2600IOTriggerSynMode.Single)
            {
                script += "trigger.timer[1].stimulus = smua.trigger.ARMED_EVENT_ID\n";
            }
            else if (mode == EK2600IOTriggerSynMode.Master)
            {
                script += "digio.trigger[" + devicePin.ToString() + "].stimulus = smua.trigger.ARMED_EVENT_ID\n";
                script += "trigger.timer[1].stimulus = smua.trigger.ARMED_EVENT_ID\n";
            }
            else if (mode == EK2600IOTriggerSynMode.Slave)
            {
                script += "digio.trigger[" + devicePin.ToString() + "].stimulus = smua.trigger.ARMED_EVENT_ID\n";
                script += "trigger.timer[1].stimulus = digio.trigger[" + syschroneziPin.ToString() + "].EVENT_ID\n";
            }

            //-----------------------------------------------------------------------------------------------
            // FIMV or FVMI Setting
            //-----------------------------------------------------------------------------------------------
            if (param.SMUA.SrcMode == EK2600SrcMode.V_Source)
            {
                script += "smua.source.func	= smua.OUTPUT_DCVOLTS\n";
                script += "smua.source.autorangev = smua.AUTORANGE_OFF\n";
                script += "smua.source.rangev = math.max(math.abs(" + startA.ToString() + "), math.abs(" + stopA.ToString() + "))\n";
                script += "smua.source.levelv = 0\n";
                script += "smua.source.limiti = 0.1\n";

                script += "smua.measure.autorangei = smua.AUTORANGE_OFF\n";
                script += "smua.measure.rangei = " + limitiA.ToString() + "\n";

                if (param.SMUA.SweepMode == EK2600SweepMode.Linear)
                {
                    script += "smua.trigger.source.linearv(" + startA.ToString() + ", " + stopA.ToString() + ", " + numPoints.ToString() + ")\n";
                }
                else if (param.SMUA.SweepMode == EK2600SweepMode.Log)
                {
                    script += "smua.trigger.source.logv(" + startA.ToString() + ", " + stopA.ToString() + ", " + numPoints.ToString() + ",0)\n";
                }
                else if (param.SMUA.SweepMode == EK2600SweepMode.Custom)
                {
                    script += "smua.trigger.source.listv({";

                    for (int i = 0; i < param.SMUA.SweepCustomList.Count; i++)
                    {
                        script += param.SMUA.SweepCustomList[i].ToString();

                        if (i != param.SMUA.SweepCustomList.Count - 1)
                        {
                            script += ",";
                        }
                    }

                    script += "})\n";
                }

                script += "smua.trigger.source.limiti = " + limitiA.ToString() + "\n";

                script += "smua.trigger.measure.i(smua.nvbuffer1)\n";
            }
            else if (param.SMUA.SrcMode == EK2600SrcMode.I_Source)
            {
                script += "smua.source.func	= smua.OUTPUT_DCAMPS\n";
                script += "smua.source.autorangei = smua.AUTORANGE_OFF\n";
                script += "smua.source.rangei = math.max(math.abs(" + startA.ToString() + "), math.abs(" + stopA.ToString() + "))\n";
                script += "smua.source.leveli = 0\n";
                script += "smua.source.limitv = 1\n";

                script += "smua.measure.autorangev = smua.AUTORANGE_OFF\n";
                script += "smua.measure.rangev = " + limitiA.ToString() + "\n";

                if (param.SMUA.SweepMode == EK2600SweepMode.Linear)
                {
                    script += "smua.trigger.source.lineari(" + startA.ToString() + ", " + stopA.ToString() + ", " + numPoints.ToString() + ")\n";
                }
                else if (param.SMUA.SweepMode == EK2600SweepMode.Log)
                {
                    script += "smua.trigger.source.logi(" + startA.ToString() + ", " + stopA.ToString() + ", " + numPoints.ToString() + ",0)\n";
                }
                else if (param.SMUA.SweepMode == EK2600SweepMode.Custom)
                {
                    script += "smua.trigger.source.listi({";

                    for (int i = 0; i < param.SMUA.SweepCustomList.Count; i++)
                    {
                        script += param.SMUA.SweepCustomList[i].ToString();

                        if (i != param.SMUA.SweepCustomList.Count - 1)
                        {
                            script += ",";
                        }
                    }

                    script += "})\n";
                }

                script += "smua.trigger.source.limitv = " + limitiA.ToString() + "\n";

                script += "smua.trigger.measure.v(smua.nvbuffer1)\n";
            }

            if (param.SMUB.SrcMode == EK2600SrcMode.V_Source)  // FVMI
            {
                script += "smub.source.func	= smub.OUTPUT_DCVOLTS\n";
                script += "smub.source.autorangev = smub.AUTORANGE_OFF\n";
                script += "smub.source.rangev = math.max(math.abs(" + startB.ToString() + "), math.abs(" + stopB.ToString() + "))\n";
                script += "smub.source.levelv = 0\n";
                script += "smub.source.limiti = 0.1\n";

                script += "smub.measure.autorangei = smub.AUTORANGE_OFF\n";
                script += "smub.measure.rangei = " + limitiB.ToString() + "\n";

                if (param.SMUB.SweepMode == EK2600SweepMode.Linear)
                {
                    script += "smub.trigger.source.linearv(" + startB.ToString() + ", " + stopB.ToString() + ", " + numPoints.ToString() + ")\n";
                }
                else if (param.SMUB.SweepMode == EK2600SweepMode.Log)
                {
                    script += "smub.trigger.source.logv(" + startB.ToString() + ", " + stopB.ToString() + ", " + numPoints.ToString() + ",0)\n";
                }
                else if (param.SMUB.SweepMode == EK2600SweepMode.Custom)
                {
                    script += "smub.trigger.source.listv({";

                    for (int i = 0; i < param.SMUB.SweepCustomList.Count; i++)
                    {
                        script += param.SMUB.SweepCustomList[i].ToString();

                        if (i != param.SMUB.SweepCustomList.Count - 1)
                        {
                            script += ",";
                        }
                    }

                    script += "})\n";
                }

                script += "smub.trigger.source.limiti = " + limitiB.ToString() + "\n";

                script += "smub.trigger.measure.i(smub.nvbuffer1)\n";
            }
            else if (param.SMUB.SrcMode == EK2600SrcMode.I_Source)  // FIMV
            {
                script += "smub.source.func	= smub.OUTPUT_DCAMPS\n";
                script += "smub.source.autorangei = smub.AUTORANGE_OFF\n";
                script += "smub.source.rangei = math.max(math.abs(" + startB.ToString() + "), math.abs(" + stopB.ToString() + "))\n";
                script += "smub.source.leveli = 0\n";
                script += "smub.source.limitv = 1\n";

                script += "smub.measure.autorangev = smub.AUTORANGE_OFF\n";
                script += "smub.measure.rangev = " + limitiB.ToString() + "\n";

                if (param.SMUB.SweepMode == EK2600SweepMode.Linear)
                {
                    script += "smub.trigger.source.lineari(" + startB.ToString() + ", " + stopB.ToString() + ", " + numPoints.ToString() + ")\n";
                }
                else if (param.SMUB.SweepMode == EK2600SweepMode.Log)
                {
                    script += "smub.trigger.source.logi(" + startB.ToString() + ", " + stopB.ToString() + ", " + numPoints.ToString() + ",0)\n";
                }
                else if (param.SMUB.SweepMode == EK2600SweepMode.Custom)
                {
                    script += "smub.trigger.source.listi({";

                    for (int i = 0; i < param.SMUB.SweepCustomList.Count; i++)
                    {
                        script += param.SMUB.SweepCustomList[i].ToString();

                        if (i != param.SMUB.SweepCustomList.Count - 1)
                        {
                            script += ",";
                        }
                    }

                    script += "})\n";
                }

                script += "smub.trigger.source.limitv = " + limitiB.ToString() + "\n";

                script += "smub.trigger.measure.v(smub.nvbuffer1)\n";
            }

            //-----------------------------------------------------------------------------------------------
            // Trigger
            //-----------------------------------------------------------------------------------------------
            script += "smua.source.output = smua.OUTPUT_ON\n";
            script += "smub.source.output = smub.OUTPUT_ON\n";

            if (mode == EK2600IOTriggerSynMode.Master)
            {
                script += "while true do\n";
                script += "local isAllReady = 0\n";

                for (uint i = 0; i < monitorPin.Length; i++)
                {
                    uint ioPin = monitorPin[i];
                    script += "isAllReady = isAllReady + digio.readbit(" + ioPin.ToString() + ")\n";
                }

                script += "if isAllReady == 0 then break end\n";
                script += "end\n";
            }
            else if (mode == EK2600IOTriggerSynMode.Slave)
            {
                script += "while true do\n";
                script += "if digio.readbit(" + syschroneziPin.ToString() + ") == 1 then break end\n";
                script += "end\n";
            }

            script += "smub.trigger.initiate()\n";
            script += "smua.trigger.initiate()\n";
            script += "waitcomplete()\n";
            script += "smua.source.output = smua.OUTPUT_OFF\n";
            script += "smub.source.output = smub.OUTPUT_OFF\n";

            //-----------------------------------------------------------------------------------------------
            // Wiat Synchronize Complete
            //-----------------------------------------------------------------------------------------------
            if (mode == EK2600IOTriggerSynMode.Master)
            {
                script += "digio.trigger[" + devicePin.ToString() + "].release()\n";

                script += "while true do\n";
                script += "local isAllReady2 = 0\n";

                for (uint i = 0; i < monitorPin.Length; i++)
                {
                    script += "isAllReady2 = isAllReady2 + digio.readbit(" + monitorPin[i].ToString() + ")\n";
                }

                script += "if isAllReady2 == " + monitorPin.Length.ToString() + " then break end\n";
                script += "end\n";
            }
            else if (mode == EK2600IOTriggerSynMode.Slave)
            {
                script += "digio.trigger[" + devicePin.ToString() + "].release()\n";
            }

            //-----------------------------------------------------------------------------------------------
            // Print Data
            //-----------------------------------------------------------------------------------------------
            script += "printbuffer(1, smua.nvbuffer1.n, smua.nvbuffer1, smub.nvbuffer1)\n";
            script += "collectgarbage()\n";
            script += "endscript\n";
            script += "num_" + param.Index + ".source = nil\n";

            return script;
        }

        public static string PulseI_PMDT_DUAL(K2600ScriptSetting param)
        {
            string script = string.Empty;
            string timerConfig = string.Empty;
            string srcFunc = string.Empty;
            string msrtFunc = string.Empty;

            // [0] I Source;  [1] V Source
            int srcMode = GetSrcMsrtMode(param.SMUA.SrcMode, out srcFunc, out msrtFunc);
            bool isAutoTurnOff = param.SMUA.IsAutoTurnOff;
            //double clampLimit = param.SMUA.MsrtBoundryLimit;

            double nplc = param.SMUA.MsrtNPLC;

            uint index = param.Index;
            string keyName = param.KeyName;
            double waitTime = param.SMUA.WaitTime;
            //double msrtRange = param.SMUA.MsrtRange;
            double pulseWidth = param.SMUA.srcTime;
            double duty = 0.01d;
            double pulseLevel = param.SMUA.SrcLevel;

            double pulseRange = param.SMUA.SrcRange;
            double limitV = param.SMUA.MsrtClamp;

            double msrtDelay = 0.0d;

            uint trigCount = 1;

            double pulsePeriod = pulseWidth / duty;

            pulsePeriod = Math.Round(pulsePeriod, 6, MidpointRounding.AwayFromZero);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            // Force Range and Compliance Setting
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string srcRngAndComplScriptA = string.Empty;
            string srcRngAndComplScriptB = string.Empty;
            string srcRngAndComplScriptAB = string.Empty;

            SetPMDTSrcRange(param, ref srcRngAndComplScriptA, ref srcRngAndComplScriptB, ref srcRngAndComplScriptAB, ref pulseRange);//srcFunc, msrtFunc, clampLimit, nplc, ref forceRange, clamp);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            // Range selected
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string msrtRangeScriptA = string.Empty;
            string msrtRangeScriptB = string.Empty;
            string msrtRangeScriptAB = string.Empty;

            SetPMDTMsrtRange(param, ref msrtRangeScriptA, ref msrtRangeScriptB, ref msrtRangeScriptAB);


            ///////////////////////////////////////////////////////////////////////////////////////////////
            // Timer Config
            ///////////////////////////////////////////////////////////////////////////////////////////////
            if (pulseWidth < 200e-6d)
            {
                nplc = 0.001d;

                msrtDelay = 50e-6;
            }
            else if (pulseWidth < 300e-6d)
            {
                nplc = 0.005d;

                msrtDelay = 75e-6;
            }
            else
            {
                nplc = 0.01d;

                msrtDelay = Math.Round((pulseWidth - (1.0d / 60.0d) * nplc - 60e-6), 6, MidpointRounding.AwayFromZero);

                msrtDelay = msrtDelay >= 0.0d ? msrtDelay : 60e-6d;
            }

            timerConfig = SetTimer(0.0d, pulseWidth, pulsePeriod, msrtDelay, trigCount);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            // source output
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string srcScriptA = string.Empty;
            string srcScriptB = string.Empty;
            string srcScriptAB = string.Empty;

           // SetPMDTDC(param, ref srcScriptA, ref srcScriptB, ref srcScriptAB);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            // Msrt
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string msrtScriptA = string.Empty;
            string msrtScriptB = string.Empty;
            string msrtScriptAB = string.Empty;

           // SetPMDTMsrtDC(param, ref msrtScriptA, ref msrtScriptB, ref msrtScriptAB);


            ///////////////////////////////////////////////////////////////////////////////////////////////
            // Source Off
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string srcOffScriptA = string.Empty;
            string srcOffScriptB = string.Empty;
            string srcOffScriptAB = string.Empty;

           // SetPMDTTurnOff(param, forceRange, ref srcOffScriptA, ref srcOffScriptB, ref srcOffScriptAB);

            //////////////////////////////////////////////////////////////////////////////////////////////////////
            // Test Sequence
            //////////////////////////////////////////////////////////////////////////////////////////////////////
            script = "loadscript num_" + index.ToString() + "\n";

            //-------------------------------------------------------------------------------------------------
#if USE_MPI_PROTECT_MODULE
            script += SetProtectionModule(param.ProtectionModuleSN, param.PMResistance, param.SetProtectionModuleStatus);
#endif
            script += timerConfig;

            //--------------------------------------------------------------------------------------------------
            script += "if channel == 1 then\n"; // SMUA

            script += "setNPLC(" + nplc.ToString() + ")\n";
            script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";

            script += msrtRangeScriptA;

            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            script += srcRngAndComplScriptA;

            script += "TrigA_Src.lineari(" + pulseLevel.ToString() + "," + pulseLevel.ToString() + "," + trigCount.ToString() + ")\n";
            script += "TrigA_LimitV(" + limitV.ToString() + ")\n";
            script += "TrigA_Action_Msrt(smua.ENABLE)\n";
            script += "TrigA_Msrt.v(bufA1)\n";
            script += "TrigA_Action_EndPulse(smua.SOURCE_IDLE)\n";
            script += "TrigA_Count(" + trigCount.ToString() + ")\n";
            script += "TrigA_Stimulus_Arm(0)\n";

            script += "TrigA_Stimulus_Src(Trig_T1.EVENT_ID)\n";   // control period (Ton + Toff); stimulus by Arm
            script += "TrigA_Stimulus_Msrt(Trig_T2.EVENT_ID)\n";  // control Measure timeing;     stimulus by timer[1]
            script += "TrigA_Stimulus_EndPulse(Trig_T3.EVENT_ID)\n"; // control Ton;                 stimulus by timer[1]

            script += "TrigA_Action_Src(smua.ENABLE)\n";

            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            if (waitTime > 0)
            {
                script += "delay(" + waitTime.ToString() + ")\n";
            }

            script += "TrigA.initiate()\n";

            script += "waitcomplete()\n";
            script += "setOutput(0)\n";

            script += "printbuffer(1, " + trigCount + ", bufA1)\n";

            script += "bufA1.clear()\n";

            //--------------------------------------------------------------------------------------------------
            script += "elseif channel == 2 then\n"; // SMUB

            script += "T1_Stimulus(TrigB.ARMED_EVENT_ID)\n";

            script += "setBNPLC(" + nplc.ToString() + ")\n";
            script += "if getBFunc() ~= " + srcMode.ToString() + " then setBFunc(" + srcMode.ToString() + ") end\n";

            script += msrtRangeScriptB;

            script += "if getBOutput() ~= 1 then setBOutput(1) end\n";

            script += srcRngAndComplScriptB;

            script += "TrigB_Src.lineari(" + pulseLevel.ToString() + "," + pulseLevel.ToString() + "," + trigCount.ToString() + ")\n";
            script += "TrigB_LimitV(" + limitV.ToString() + ")\n";
            script += "TrigB_Action_Msrt(smub.ENABLE)\n";
            script += "TrigB_Msrt.v(bufB1)\n";
            script += "TrigB_Action_EndPulse(smub.SOURCE_IDLE)\n";
            script += "TrigB_Count(" + trigCount.ToString() + ")\n";
            script += "TrigB_Stimulus_Arm(0)\n";

            script += "TrigB_Stimulus_Src(Trig_T1.EVENT_ID)\n";   // control period (Ton + Toff); stimulus by Arm
            script += "TrigB_Stimulus_Msrt(Trig_T2.EVENT_ID)\n";  // control Measure timeing;     stimulus by timer[1]
            script += "TrigB_Stimulus_EndPulse(Trig_T3.EVENT_ID)\n"; // control Ton;                 stimulus by timer[1]

            script += "TrigB_Action_Src(smub.ENABLE)\n";

            script += "if getBOutput() ~= 1 then setBOutput(1) end\n";

            if (waitTime > 0)
            {
                script += "delay(" + waitTime.ToString() + ")\n";
            }

            script += "TrigB.initiate()\n";

            script += "waitcomplete()\n";

            script += "setBOutput(0)\n";

            script += "printbuffer(1, " + trigCount + ", bufB1)\n";

            script += "bufB1.clear()\n";

            //--------------------------------------------------------------------------------------------------
            script += "else\n"; // Both

            script += "setNPLC(" + nplc.ToString() + ")\n";
            script += "setBNPLC(" + nplc.ToString() + ")\n";

            script += "if getFunc() ~= " + srcMode.ToString() + " then setFunc(" + srcMode.ToString() + ") end\n";
            script += "if getBFunc() ~= " + srcMode.ToString() + " then setBFunc(" + srcMode.ToString() + ") end\n";

            script += msrtRangeScriptAB;

            script += "if getOutput() ~= 1 then setOutput(1) end\n";
            script += "if getBOutput() ~= 1 then setBOutput(1) end\n";

            script += srcRngAndComplScriptAB;

            script += "TrigA_Src.lineari(" + pulseLevel.ToString() + "," + pulseLevel.ToString() + "," + trigCount.ToString() + ")\n";
            script += "TrigB_Src.lineari(" + pulseLevel.ToString() + "," + pulseLevel.ToString() + "," + trigCount.ToString() + ")\n";

            script += "TrigA_LimitV(" + limitV.ToString() + ")\n";
            script += "TrigB_LimitV(" + limitV.ToString() + ")\n";

            script += "TrigA_Action_Msrt(smua.ENABLE)\n";
            script += "TrigB_Action_Msrt(smub.ENABLE)\n";

            script += "TrigA_Msrt.v(bufA1)\n";
            script += "TrigB_Msrt.v(bufB1)\n";

            script += "TrigA_Action_EndPulse(smua.SOURCE_IDLE)\n";
            script += "TrigB_Action_EndPulse(smub.SOURCE_IDLE)\n";

            script += "TrigA_Count(" + trigCount.ToString() + ")\n";
            script += "TrigB_Count(" + trigCount.ToString() + ")\n";

            script += "TrigA_Stimulus_Arm(0)\n";
            script += "TrigB_Stimulus_Arm(0)\n";

            script += "TrigA_Stimulus_Src(Trig_T1.EVENT_ID)\n";   // control period (Ton + Toff); stimulus by Arm
            script += "TrigB_Stimulus_Src(Trig_T1.EVENT_ID)\n";   // control period (Ton + Toff); stimulus by Arm

            script += "TrigA_Stimulus_Msrt(Trig_T2.EVENT_ID)\n";  // control Measure timeing;     stimulus by timer[1]
            script += "TrigB_Stimulus_Msrt(Trig_T2.EVENT_ID)\n";  // control Measure timeing;     stimulus by timer[1]

            script += "TrigA_Stimulus_EndPulse(Trig_T3.EVENT_ID)\n"; // control Ton;                 stimulus by timer[1]
            script += "TrigB_Stimulus_EndPulse(Trig_T3.EVENT_ID)\n"; // control Ton;                 stimulus by timer[1]

            script += "TrigA_Action_Src(smua.ENABLE)\n";
            script += "TrigB_Action_Src(smub.ENABLE)\n";

            script += "if getOutput() ~= 1 then setOutput(1) end\n";
            script += "if getBOutput() ~= 1 then setBOutput(1) end\n";

            if (waitTime > 0)
            {
                script += "delay(" + waitTime.ToString() + ")\n";
            }

            script += "TrigB.initiate()\n";
            script += "TrigA.initiate()\n";
          
            script += "waitcomplete()\n";

            script += "setOutput(0)\n";
            script += "setBOutput(0)\n";

            script += "printbuffer(1, " + trigCount + ", bufA1, bufB1)\n";

            script += "bufA1.clear()\n";
            script += "bufB1.clear()\n";

            script += "end\n"; // if
            //--------------------------------------------------------------------------------------------------

            script += "Trig_T1.reset()\n";
            script += "Trig_T2.reset()\n";
            script += "Trig_T3.reset()\n";
            script += "Trig_T4.reset()\n";


            if (pulseLevel >= 0.1)
            {
                // 防止切換 1A Range 時, 產生 Overshoot
                script += "setSorceRangeI(0.01)\n";
            }

            script += "channel = nil\n";
            script += "endscript\n";

            script += "num_" + index.ToString() + ".source = nil";

            return script;
        }

        #endregion

        #region >>> Other Script <<<

        public static string ROhm_SMUA(K2600ScriptSetting param)
        {
            string script = string.Empty;

            string srcRngAndComplScript = "";

            //-----------------------------------------------------------------------------------------
            // Force Range and Compliance Setting
            //-----------------------------------------------------------------------------------------
            if (param.SMUA.MsrtClamp <= param.SMUA.MsrtBoundryLimit)
            {
                srcRngAndComplScript += "setLimitV(" + param.SMUA.MsrtClamp.ToString() + ")\n";

                srcRngAndComplScript += "setSorceRangeI(" + param.SMUA.SrcRange.ToString() + ")\n";
            }
            else
            {
                if (Math.Abs(param.SMUA.SrcRange) < 0.000001)
                {
                    srcRngAndComplScript += "setSorceRangeI(0.000001)\n";
                }
                else
                {
                    srcRngAndComplScript += "setSorceRangeI(" + param.SMUA.SrcRange.ToString() + ")\n";
                }

                srcRngAndComplScript += "setLimitV(" + param.SMUA.MsrtClamp.ToString() + ")\n";
            }

            //-----------------------------------------------------------------------------------------
            // Test Sequence
            //-----------------------------------------------------------------------------------------
            script = "loadscript num_" + param.Index.ToString() + "\n";

            script += "setNPLC(" + param.SMUA.MsrtNPLC.ToString() + ")\n";

            script += "if getFunc() ~= 0 then setFunc(0) end\n";

            script += "setMeasureRangeV(" + param.SMUA.MsrtRange.ToString() + ")\n";

            script += "setMeasureRangeI(" + param.SMUA.MsrtRange.ToString() + ")\n";

            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            script += srcRngAndComplScript;

            if (param.SMUA.WaitTime > 0)
            {
                script += "delay(" + param.SMUA.WaitTime.ToString() + ")" + "\n";
            }

            script += "setLevelI(" + param.SMUA.SrcLevel.ToString() + ")\n";

            script += "delay(" + param.SMUA.srcTime.ToString() + ")" + "\n";

            script += "print(mrtA.v())\n";

            //-----------------------------------------------------------------------------------------
            // 若打IZ量VZ的Clamp電壓大於20，打完IZ後先降Clamp，以免多花20mS時間
            //-----------------------------------------------------------------------------------------
            if (Math.Abs(param.SMUA.MsrtClamp) > 20)
            {
                script += "setLimitV(8)\n";
            }

            if (param.SMUA.IsAutoTurnOff)
            {
                if (!param.IsTurnOffToZeroVolt)
                {
                    script += "setLevelI(0)\n";
                }
                else
                {
                    script += "setOutput(0)\n";
                }
            }

            script += "endscript";

            return script;
        }

        public static string ContactCheck_SMUA(K2600ScriptSetting param)
        {
            string script = "";

            script += "loadscript num_" + param.Index.ToString() + "\n";

            script += "if getFunc() ~= 0 then setFunc(0) end\n";

            script += "setSorceRangeI(0.001)\n";

            // 0: smuX.CONTACT_FAST, 1: smuX.CONTACT_MEDIUM, 2: smuX.CONTACT_SLOW
            script += "smua.contact.speed = " + ((uint)param.SMUA.ContactCheckSpeed).ToString() + "\n";

            script += "rhi, rlo = smua.contact.r()\n";

            script += "local Outp = string.format(\"%.3f, %.3f\", rhi, rlo)\n";

            script += "print(Outp)\n";

            script += "endscript\n";

            script += "num_" + param.Index.ToString() + ".source = nil";

            return script;
        }

        public static string ContactCheck_PMDT_DUAL(K2600ScriptSetting param)
        {
            string script = "";

            script += "loadscript num_" + param.Index.ToString() + "\n";

            script += "if channel == 1 then\n"; // SMUA

            //--------------------------------------------------------------------------------------------------------
            //SMU A
            script += "if getFunc() ~= 0 then setFunc(0) end\n";

            script += "setSorceRangeI(0.001)\n";

            // 0: smuX.CONTACT_FAST, 1: smuX.CONTACT_MEDIUM, 2: smuX.CONTACT_SLOW
            script += "smua.contact.speed = " + ((uint)param.SMUA.ContactCheckSpeed).ToString() + "\n";

            script += "rhi, rlo = smua.contact.r()\n";

            script += "local Outp = string.format(\"%.3f, %.3f\", rhi, rlo)\n";

            script += "print(Outp)\n";

            //--------------------------------------------------------------------------------------------------
            script += "elseif channel == 2 then\n"; // SMUB

            script += "if getBFunc() ~= 0 then setBFunc(0) end\n";

            script += "setBSorceRangeI(0.001)\n";

            // 0: smuX.CONTACT_FAST, 1: smuX.CONTACT_MEDIUM, 2: smuX.CONTACT_SLOW
            script += "smub.contact.speed = " + ((uint)param.SMUA.ContactCheckSpeed).ToString() + "\n";

            script += "rhi, rlo = smub.contact.r()\n";

            script += "local Outp = string.format(\"%.3f, %.3f\", rhi, rlo)\n";

            script += "print(Outp)\n";

            //--------------------------------------------------------------------------------------------------
            script += "else\n"; // Both

            script += "if getFunc() ~= 0 then setFunc(0) end\n";

            script += "if getBFunc() ~= 0 then setBFunc(0) end\n";

            script += "setSorceRangeI(0.001)\n";

            script += "setBSorceRangeI(0.001)\n";

            script += "smua.contact.speed = " + ((uint)param.SMUA.ContactCheckSpeed).ToString() + "\n";

            script += "smub.contact.speed = " + ((uint)param.SMUA.ContactCheckSpeed).ToString() + "\n";

            script += "rhiA, rloA = smua.contact.r()\n";

            script += "rhiB, rloB = smub.contact.r()\n";

            script += "local Outp = string.format(\"%.3f, %.3f, %.3f, %.3f\", rhiA,rhiB, rloA, rloB)\n";

            script += "print(Outp)\n";

            script += "end\n"; // if
            //--------------------------------------------------------------------------------------------------
            script += "channel = nil\n";
            script += "endscript\n";

            script += "num_" + param.Index.ToString() + ".source = nil";

            return script;
        }

        public static string LCR_Bias_SMUA(K2600ScriptSetting param)
        {
            string script = string.Empty;

            string srcRngAndComplScript = string.Empty;

            double forceRange = param.SMUA.SrcRange;

            if (forceRange < 2.0d)
            {
                forceRange = 2.0d;
            }

            if (param.SMUA.MsrtClamp <= param.SMUA.MsrtBoundryLimit)
            {
                srcRngAndComplScript += "setLimitI(" + param.SMUA.MsrtClamp.ToString() + ")\n";

                srcRngAndComplScript += "setSorceRangeV(" + forceRange.ToString() + ")\n";
            }
            else
            {
                srcRngAndComplScript += "setSorceRangeV(" + forceRange.ToString() + ")\n";

                srcRngAndComplScript += "setLimitI(" + param.SMUA.MsrtClamp.ToString() + ")\n";
            }

            script = "loadscript num_" + param.Index.ToString() + "\n";

            script += "if getFunc() ~= 1 then setFunc(1) end\n";

            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            script += srcRngAndComplScript;

            script += "setLevelV(" + param.SMUA.SrcLevel.ToString() + ")\n";

            script += "endscript\n";

            script += "num_" + param.Index.ToString() + ".source = nil";

            return script;
        }

        #endregion

        #region >>> RTH Script <<<

        public static string RTH_Heat(K2600ScriptSetting param, double imDelay, double ihSrcLevel, double ihDelay)
        {
            string script = string.Empty;

            string srcRngAndComplScript = string.Empty;

            //-----------------------------------------------------------------------------------------
            // Force Range and Compliance Setting
            //-----------------------------------------------------------------------------------------
            if (param.SMUA.MsrtClamp <= param.SMUA.MsrtBoundryLimit)
            {
                srcRngAndComplScript += "setSorceRangeI(" + (param.SMUA.SrcRange + Math.Abs(ihSrcLevel)).ToString() + ")\n";  // RTHIhForceValue
            }
            else
            {
                double forceRange = param.SMUA.SrcRange + ihSrcLevel;  // RTHIhForceValue

                if (Math.Abs(forceRange) < 0.000001)
                {
                    forceRange = 0.000001;
                }

                srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";
            }

            //-----------------------------------------------------------------------------------------
            // Test Sequence
            //-----------------------------------------------------------------------------------------
            script = "loadscript num_" + param.Index.ToString() + "\n";

            script += "if getFunc() ~= 0 then setFunc(0) end\n";

            script += "setLevelI(0)\n";

            script += srcRngAndComplScript;

            if (param.SMUA.WaitTime > 0)
            {
                script += "delay(" + param.SMUA.WaitTime.ToString() + ")\n";
            }

            if (imDelay > 0)  // RTHImForceTime
            {
                script += "delay(" + imDelay.ToString() + ")\n";  // RTHImForceTime
            }

            script += "setLevelI(" + ihSrcLevel.ToString() + ")\n";  // RTHIhForceValue

            script += "delay(" + ihDelay.ToString() + ")\n";  // RTHIhForceTime

            //-----------------------------------------------------------------------------------------
            // 若打IZ量VZ的Clamp電壓大於20，打完IZ後先降Clamp，以免多花20mS時間
            //-----------------------------------------------------------------------------------------
            if (Math.Abs(param.SMUA.MsrtClamp) > 20)
            {
                script += "setLimitV(8)\n";
            }

            if (param.SMUA.IsAutoTurnOff)
            {
                script += "setLevelI(0)\n";

                script += "setLevelV(0)\n";

                script += "setOutput(0)\n";
            }

            script += "endscript\n";

            script += "num_" + param.Index.ToString() + ".source = nil";

            return script;
        }

        public static string RTH(K2600ScriptSetting param)
        {
            string script = string.Empty;

            string srcRngAndComplScript = string.Empty;

            //-----------------------------------------------------------------------------------------
            // Force Range and Compliance Setting
            //-----------------------------------------------------------------------------------------
            if (param.SMUA.MsrtClamp <= param.SMUA.MsrtBoundryLimit)
            {
                srcRngAndComplScript += "setSorceRangeI(" + param.SMUA.SrcRange.ToString() + ")\n";
            }
            else
            {
                double forceRange = param.SMUA.SrcRange;

                if (Math.Abs(param.SMUA.SrcRange) < 0.000001)
                {
                    forceRange = 0.000001;
                }

                srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";
            }

            //-----------------------------------------------------------------------------------------
            // Test Sequence
            //-----------------------------------------------------------------------------------------
            script = "loadscript num_" + param.Index.ToString() + "\n";

            // Enable二極體:		PIN_RTH_EANBLE = 0  
            // Bypass 二極體:		PIN_RTH_EANBLE = 1
            // P:				_pinDAQEnable = 1, PIN_RTH_EANBLE = 0
            // N:				_pinDAQEnable = 0, PIN_RTH_EANBLE = 1
            // Open:			_pinDAQEnable = 0, PIN_RTH_EANBLE = 0
            //小電容:				PIN_CAP_SW = 0
            //大電容:				PIN_CAP_SW = 1

            // Enable 二極體
            script += "digio.writebit(" + K2600Const.IO_RTH_EANBLE.ToString() + ", 0)\n";

            // 切換P/N極		
            if (param.SMUA.SrcLevel >= 0)
            {
                script += "digio.writebit(" + K2600Const.IO_DAQ_ENABLE.ToString() + ", 1)\n";

                script += "digio.writebit(" + K2600Const.IO_POLAR_SW.ToString() + ", 0)\n";

            }
            else
            {
                script += "digio.writebit(" + K2600Const.IO_DAQ_ENABLE.ToString() + ", 0)\n";

                script += "digio.writebit(" + K2600Const.IO_POLAR_SW.ToString() + ", 1)\n";
            }

            //小電容迴路
            script += "digio.writebit(" + K2600Const.IO_CAP_SW.ToString() + ", 0)\n";

            script += "if getFunc() ~= 0 then setFunc(0) end\n";

            script += srcRngAndComplScript;

            if (param.SMUA.WaitTime > 0)
            {
                script += "delay(" + param.SMUA.WaitTime.ToString() + ")\n";
            }

            script += "digio.trigger[" + K2600Const.IO_DAQ_TRIG_OUT.ToString() + "].assert()\n";

            script += "setLevelI(" + param.SMUA.SrcLevel.ToString() + ")\n";

            script += "delay(" + param.SMUA.srcTime.ToString() + ")\n";

            //-----------------------------------------------------------------------------------------
            // 若打IZ量VZ的Clamp電壓大於20，打完IZ後先降Clamp，以免多花20mS時間
            //-----------------------------------------------------------------------------------------
            if (Math.Abs(param.SMUA.MsrtClamp) > 20)
            {
                script += "setLimitV(8)\n";
            }

            if (param.SMUA.IsAutoTurnOff)
            {
                if (!param.IsTurnOffToZeroVolt)
                {
                    script += "setLevelI(0)\n";
                }
                else
                {
                    script += "setLevelI(0)\n";

                    script += "setOutput(0)\n";
                }
            }

            //關閉迴路
            script += "digio.writebit(" + K2600Const.IO_RTH_EANBLE.ToString() + ", 1)\n";

            script += "digio.writebit(" + K2600Const.IO_DAQ_ENABLE.ToString() + ", 0)\n";

            script += "digio.writebit(" + K2600Const.IO_POLAR_SW.ToString() + ", 0)\n";

            script += "endscript\n";

            script += "num_" + param.Index.ToString() + ".source = nil";

            return script;
        }

        public static string RTH_SrcOnly(K2600ScriptSetting param)
        {
            param.SMUA.SweepPoints = (uint)Math.Round((double)param.SMUA.SweepPoints / 100.0d) * 100;  // SweepContCount

            string script = string.Empty;

            string srcRngAndComplScript = string.Empty;

            //-----------------------------------------------------------------------------------------
            // Force Range and Compliance Setting
            //-----------------------------------------------------------------------------------------
            if (param.SMUA.MsrtClamp <= param.SMUA.MsrtBoundryLimit)
            {
                srcRngAndComplScript += "setLimitV(" + param.SMUA.MsrtClamp.ToString() + ")\n";

                srcRngAndComplScript += "setSorceRangeI(" + param.SMUA.SrcRange.ToString() + ")\n";
            }
            else
            {
                double forceRange = param.SMUA.SrcRange;

                if (Math.Abs(param.SMUA.SrcRange) < 0.000001)
                {
                    forceRange = 0.000001;
                }

                srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";

                srcRngAndComplScript += "setLimitV(" + param.SMUA.MsrtClamp.ToString() + ")\n";
            }

            double time = 0.000053d;

            uint measureCount = (uint)(param.SMUA.srcTime / time);

            script = "loadscript num_" + param.Index.ToString() + "\n";

            script += "digio.writebit(" + K2600Const.IO_RTH_EANBLE.ToString() + ", 1)\n";

            script += "setNPLC(0.001)\n";

            script += "setMsrtCount(" + measureCount.ToString() + ")\n";

            script += "setMsrtInterval(0.000001)\n";

            script += "if getFunc() ~= 0 then setFunc(0) end\n";

            script += "setMeasureRangeV(" + param.SMUA.MsrtRange.ToString() + ")\n";

            script += "setMeasureRangeI(" + param.SMUA.SrcRange.ToString() + ")\n";

            script += "if getOutput() ~= 1 then setOutput(1) end\n";

            script += srcRngAndComplScript;

            if (param.SMUA.WaitTime > 0)
            {
                script += "delay(" + param.SMUA.WaitTime.ToString() + ")" + "\n";
            }

            script += "setLevelI(" + param.SMUA.SrcLevel.ToString() + ")\n";

            script += "mrtA.v(bufA1)\n";

            //-----------------------------------------------------------------------------------------
            // 若打IZ量VZ的Clamp電壓大於20，打完IZ後先降Clamp，以免多花20mS時間
            //-----------------------------------------------------------------------------------------
            if (Math.Abs(param.SMUA.MsrtClamp) > 20)
            {
                script += "setLimitV(8)\n";
            }

            if (param.SMUA.IsAutoTurnOff)
            {
                if (!param.IsTurnOffToZeroVolt)
                {
                    script += "setLevelI(0)\n";
                }
                else
                {
                    script += "setLevelI(0)\n";

                    script += "setOutput(0)\n";
                }
            }

            script += "printbuffer(1, " + measureCount.ToString() + ", bufA1)\n";

            script += "setMsrtCount(1)\n";

            script += "bufA1.clear()\n";

            script += "digio.writebit(" + K2600Const.IO_RTH_EANBLE.ToString() + ", 0)\n";

            script += "endscript\n";

            script += "num_" + param.Index.ToString() + ".source = nil";

            return script;
        }

        public static string VLR(K2600ScriptSetting param, double imDelay, double ihDelay)
        {
            string script = string.Empty;

            string srcRngAndComplScript = string.Empty;

            //-----------------------------------------------------------------------------------------
            // Force Range and Compliance Setting
            //-----------------------------------------------------------------------------------------
            double forceRange = param.SMUA.SrcLevel;

            if (forceRange < 0.000001)
            {
                forceRange = 0.000001;
            }

            if (param.SMUA.MsrtClamp <= param.SMUA.MsrtBoundryLimit)
            {
                srcRngAndComplScript += "setLimitV(" + param.SMUA.MsrtBoundryLimit.ToString() + ")\n";

                srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";
            }
            else
            {
                srcRngAndComplScript += "setSorceRangeI(" + forceRange.ToString() + ")\n";

                srcRngAndComplScript += "setLimitV(" + param.SMUA.MsrtBoundryLimit.ToString() + ")\n";
            }

            //-----------------------------------------------------------------------------------------
            // Test Sequence
            //-----------------------------------------------------------------------------------------
            script = "loadscript num_" + param.Index.ToString() + "\n";

            // Enable二極體:		PIN_RTH_EANBLE = 0  
            // Bypass 二極體:		PIN_RTH_EANBLE = 1
            // P:				_pinDAQEnable = 1, PIN_RTH_EANBLE = 0
            // N:				_pinDAQEnable = 0, PIN_RTH_EANBLE = 1
            // Open:			_pinDAQEnable = 0, PIN_RTH_EANBLE = 0
            //小電容:				PIN_CAP_SW = 0
            //大電容:				PIN_CAP_SW = 1

            // Enable 二極體
            script += "digio.writebit(" + K2600Const.IO_RTH_EANBLE.ToString() + ", 0)\n";

            // 切換P/N極		
            if (param.SMUA.SrcLevel >= 0)
            {
                script += "digio.writebit(" + K2600Const.IO_DAQ_ENABLE.ToString() + ", 1)\n";

                script += "digio.writebit(" + K2600Const.IO_POLAR_SW.ToString() + ", 0)\n";

            }
            else
            {
                script += "digio.writebit(" + K2600Const.IO_DAQ_ENABLE.ToString() + ", 0)\n";

                script += "digio.writebit(" + K2600Const.IO_POLAR_SW.ToString() + ", 1)\n";
            }

            //小電容迴路
            script += "digio.writebit(" + K2600Const.IO_CAP_SW.ToString() + ", 0)\n";

            //script += "if getFunc() ~= 1 then setFunc(1) end\n";

            //script += "setSorceRangeV(0)\n";

            //script += "setLevelV(0)\n";

            //script += "delay(0.005)\n";

            script += "if getFunc() ~= 0 then setFunc(0) end\n";

            script += srcRngAndComplScript;

            if (param.SMUA.WaitTime > 0)
            {
                script += "delay(" + param.SMUA.WaitTime.ToString() + ")\n";
            }

            script += "digio.trigger[" + K2600Const.IO_DAQ_TRIG_OUT.ToString() + "].assert()\n";

            script += "setLevelI(" + param.SMUA.SrcLevel.ToString() + ")\n";

            script += "delay(" + imDelay.ToString() + ")\n";  // RTHImForceTime

            script += "delay(" + ihDelay.ToString() + ")\n";  // RTHImForceTime

            //script += "delay(" + item.RTHIm2ForceTime.ToString() + ")\n";            

            script += "print(\"A\")\n";

            //-----------------------------------------------------------------------------------------
            // 若打IZ量VZ的Clamp電壓大於20，打完IZ後先降Clamp，以免多花20mS時間
            //-----------------------------------------------------------------------------------------
            if (Math.Abs(param.SMUA.MsrtClamp) > 20)
            {
                script += "setLimitV(8)\n";
            }

            if (param.SMUA.IsAutoTurnOff)
            {
                if (!param.IsTurnOffToZeroVolt)
                {
                    script += "setLevelI(0)\n";
                }
                else
                {
                    script += "setLevelV(0)\n";

                    script += "setOutput(0)\n";
                }
            }

            //關閉迴路
            //script += "digio.writebit(" + PIN_RTH_EANBLE.ToString() + ", 1)\n";

            //script += "digio.writebit(" + _pinDAQEnable.ToString() + ", 0)\n";

            //script += "digio.writebit(" + PIN_RTH_EANBLE.ToString() + ", 0)\n";

            script += "endscript\n";

            script += "num_" + param.Index.ToString() + ".source = nil";

            return script;
        }

        #endregion

        #region >>> Function <<<

        // ConfigFunc : 初始化時, 預載的功能
        // CallFunc : 叫用預載的功能
        // 注意, Config 與 Call 的 Function Name 需一致

        #region >>single F/M<<
        public static string ConfigFuncFIMV_A()
        {
            string func = string.Empty;

            func = "function funcFIMV_A(waitTime, srcValueA ,srcTime, msrtRange, nplc) ";

            func += "setNPLC(nplc) ";

            func += "if getFunc() ~= 0 then setFunc(0) end ";  // [0] I Source;  [1] V Source

            func += "setMeasureRangeV(msrtRange) ";

            func += "if getOutput() ~= 1 then setOutput(1) end ";

            func += "setLimitV(msrtRange) ";

            func += "setSorceRangeI(srcValueA) ";

            func += "delay(waitTime) ";

            func += "setLevelI(srcValueA) ";

            func += "delay(srcTime) ";

            func += "print(mrtA.v()) ";

            func += "setLevelI(0) ";

            func += "end";

            return func;
        }

        public static string ConfigFuncFVMI_A()
        {
            string func = string.Empty;

            func = "function funcFVMI_A(waitTime, srcValueA , srcTime, msrtRange, nplc) ";

            func += "setNPLC(nplc) ";

            func += "if getFunc() ~= 1 then setFunc(1) end ";  // [0] I Source;  [1] V Source

            func += "setMeasureRangeI(msrtRange) ";

            func += "setLimitI(msrtRange) ";

            func += "setSorceRangeV(srcValueA) ";

            func += "smua.measure.autozero = smua.AUTOZERO_ONCE ";

            func += "if getOutput() ~= 1 then setOutput(1) end ";      

            func += "delay(waitTime) ";

            func += "setLevelV(srcValueA) ";

            func += "delay(srcTime) ";

            func += "print(mrtA.i()) ";

            func += "setLevelV(0) ";
            
            func += "end";

            return func;
        }

        public static string ConfigFuncFIMV_B()
        {
            string func = string.Empty;

            func = "function funcFIMV_B(waitTime,  srcValeB, srcTime, msrtRange, nplc) ";

            func += "setBNPLC (nplc) ";

            func += "if getBFunc() ~= 0 then setBFunc(0) end ";  // [0] I Source;  [1] V Source

            func += "setBMeasureRangeV(msrtRange) ";

            func += "if getBOutput() ~= 1 then setBOutput(1) end ";

            func += "setBLimitV(msrtRange) ";

            func += "setBSorceRangeI(srcValeB) ";

            func += "delay(waitTime)  ";

            func += "setBLevelI(srcValeB) ";

            func += "delay(srcTime) ";

            func += "print(mrtB.v()) ";

            func += "setBLevelI(0) ";

            func += "end";

            return func;
        }

        public static string ConfigFuncFVMI_B()
        {
            string func = string.Empty;

            func = "function funcFVMI_B(waitTime,  srcValeB, srcTime, msrtRange, nplc) ";

            func += "setBNPLC (nplc) ";

            func += "if getBFunc() ~= 1 then setBFunc(1) end ";  // [0] I Source;  [1] V Source

            func += "setBMeasureRangeI(msrtRange) ";

            func += "setBLimitI(msrtRange) ";

            func += "setBSorceRangeV(srcValeB) ";

            func += "smub.measure.autozero = smua.AUTOZERO_ONCE ";

            func += "if getBOutput() ~= 1 then setBOutput(1) end ";

            func += "delay(waitTime) ";

            func += "setBLevelV(srcValeB) ";

            func += "delay(srcTime) ";

            func += "print(mrtB.i()) ";

            func += "setBLevelV(0) ";

            func += "end";

            return func;
        }

        public static string ConfigFuncFIMVDual()
        {
            string func = string.Empty;

            func = "function funcFIMV_DUAL(waitTime, srcValueA , srcValueB, srcTime, msrtRange, nplc) ";
            
            func += "setNPLC(nplc) ";
            func += "setBNPLC (nplc) ";

            func += "if getFunc() ~= 0 then setFunc(0) end ";  // [0] I Source;  [1] V Source
            func += "if getBFunc() ~= 0 then setBFunc(0) end ";  // [0] I Source;  [1] V Source

            func += "setMeasureRangeV(msrtRange) ";
            func += "setBMeasureRangeV(msrtRange) ";

            func += "if getOutput() ~= 1 then setOutput(1) end ";
            func += "if getBOutput() ~= 1 then setBOutput(1) end ";

            func += "setLimitV(msrtRange) ";
            func += "setBLimitV(msrtRange) ";

            func += "setSorceRangeI(srcValueA) ";
            func += "setBSorceRangeI(srcValueB) ";

            func += "delay(waitTime) ";

            func += "setLevelI(srcValueA) ";
            func += "setBLevelI(srcValueB) ";

            func += "delay(srcTime) ";

            func += "mrtVsyncA(bufA1) ";
            func += "mrtVsyncB(bufB1) ";

            func += "waitcomplete() ";
            func += "printbuffer(1, 1, bufA1, bufB1) ";
            func += "bufA1.clear() ";
            func += "bufB1.clear() ";

            func += "setLevelI(0) ";
            func += "setBLevelI(0) ";

            func += "end\n";

            return func;
        }

        public static string ConfigFuncFVMIDual()
        {
            string func = string.Empty;

            func = "function funcFVMI_DUAL(waitTime, srcValueA , srcValeB, srcTime, msrtRange, nplc) ";

            func += "setNPLC(nplc) ";
            func += "setBNPLC (nplc) ";

            func += "if getFunc() ~= 1 then setFunc(1) end ";  // [0] I Source;  [1] V Source
            func += "if getBFunc() ~= 1 then setBFunc(1) end ";  // [0] I Source;  [1] V Source

            func += "setMeasureRangeI(msrtRange) ";
            func += "setBMeasureRangeI(msrtRange) ";
            func += "setLimitI(msrtRange) ";
            func += "setBLimitI(msrtRange) ";

            func += "setSorceRangeV(srcValueA) ";
            func += "setBSorceRangeV(srcValueB) ";

            func += "smua.measure.autozero = smua.AUTOZERO_ONCE ";
            func += "smub.measure.autozero = smua.AUTOZERO_ONCE ";

            func += "if getOutput() ~= 1 then setOutput(1) end ";
            func += "if getBOutput() ~= 1 then setBOutput(1) end ";

       

            func += "delay(waitTime) ";

            func += "setLevelV(srcValueA) ";
            func += "setBLevelV(srcValueB) ";

            func += "delay(srcTime) ";

            func += "mrtIsyncA(bufA1)";
            func += "mrtIsyncA(bufB1)";

            func += "waitcomplete()";
            func += "printbuffer(1, 1, bufA1, bufB1)";
            func += "bufA1.clear()";
            func += "bufB1.clear()";

            func += "setLevelV(0) ";
            func += "setBLevelV(0) ";
            func += "end";

            return func;
        }

        public static string CallFuncFIMV(double waitTime, double forceValueA, double forceValueB, double forceTime, double msrtRange, double nplc, int ch)
        {
            if (ch == 1)
            {
                return string.Format("funcFIMV_A({0}, {1}, {2}, {3}, {4})", waitTime, forceValueA,  forceTime, msrtRange, nplc);
            }
            else if (ch == 2)
            {
                return string.Format("funcFIMV_B({0}, {1}, {2}, {3}, {4})", waitTime, forceValueB, forceTime, msrtRange, nplc);
            }
            else
            {
                return string.Format("funcFIMV_DUAL({0}, {1}, {2}, {3}, {4}, {5})", waitTime,forceValueA, forceValueB, forceTime, msrtRange, nplc);
            }
        }

        public static string CallFuncFVMI(double waitTime, double forceValueA,double forceValueB, double forceTime, double msrtRange, double nplc,int ch)
        {
            if (ch == 1)
            {
                return string.Format("funcFVMI_A({0}, {1}, {2}, {3}, {4})", waitTime, forceValueA, forceTime, msrtRange, nplc);
            }
            else if (ch == 2)
            {
                return string.Format("funcFVMI_B({0}, {1}, {2}, {3}, {4})", waitTime, forceValueB, forceTime, msrtRange, nplc);
            }
            else
            {
                return string.Format("funcFVMI_DUAL({0}, {1}, {2}, {3}, {4},{5})", waitTime, forceValueA, forceValueB, forceTime, msrtRange, nplc);
            }
        }

        #endregion

        private static void ExportScriptToTxt(string script, string fileName)
        {
            string path = System.IO.Path.Combine(@"C:\", fileName + ".txt");

            string dir = System.IO.Path.GetDirectoryName(path);

            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir); 
            }

            string thePath = string.Empty;

            string[] strArr = path.Split(new char[] { '\\' });

            for (int i = 0; i < strArr.Length - 1; ++i)
            {
                thePath += strArr[i];
                if (i != strArr.Length - 2)
                {
                    thePath += "\\";
                }
                if (!System.IO.Directory.Exists(thePath))
                {
                    System.IO.Directory.CreateDirectory(thePath);
                }
            }

            //if()path

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
        #endregion

        #region >>> Private Methods <<<

        private static void SetProtectionModuleRelay(EK2600ProtectionModule PM, EK2600ProtectionModuleResistance PMResistance, out string relay1, out string relay2)
        {
            switch (PM)
            {
                case EK2600ProtectionModule.ATV_PM2:

                    switch (PMResistance)
                    {
                        case EK2600ProtectionModuleResistance.R1: // 510 ohm
                            relay1 = "1"; 
                            relay2 = "0";
                            break;
                        case EK2600ProtectionModuleResistance.R2: // 1020 ohm
                            relay1 = "1"; 
                            relay2 = "1";
                            break;
                        default:
                            relay1 = "0"; 
                            relay2 = "0";
                            break;
                    }

                    break;
                case EK2600ProtectionModule.MPI_KPM:

                    switch (PMResistance)
                    {
                        case EK2600ProtectionModuleResistance.R1: // 100 ohm
                            relay1 = "1"; 
                            relay2 = "0";
                            break;
                        case EK2600ProtectionModuleResistance.R2: // 1000 ohm
                            relay1 = "0"; 
                            relay2 = "1";
                            break;
                        case EK2600ProtectionModuleResistance.R3: // 1100 ohm
                            relay1 = "1"; 
                            relay2 = "1";
                            break;
                        default:
                            relay1 = "0"; 
                            relay2 = "0";
                            break;
                    }

                    break;

                default:
                    relay1 = "0"; 
                    relay2 = "0";
                    break;
            }
        }

        #endregion
    }
}
