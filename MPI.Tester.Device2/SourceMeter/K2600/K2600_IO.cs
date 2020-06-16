using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MPI.Tester.DeviceCommon;
using MPI.Tester.Maths;
using MPI.Tester.Device.SourceMeter.KeithleySMU;

namespace MPI.Tester.Device.SourceMeter
{
    public partial class K2600Wrapper
	{
        //public bool SetIO(uint index, ElectSettingData item)
        //{
        //    this.SetIOScript(index, item);

        //    return !this.GetErrorMsg();
        //}

        public string SetIO(uint index, ElectSettingData item)
        {
            return this.SetIOScript(index, item);

        }

        public void SetDefaultIO(int ioPin, EIOTrig_Mode mode, EIOState state, EIOAct act, double pw)
        {
            IOData ioData = GetIO(ioPin);

            ioData.SetDefaultIO(mode, state, act, pw, ioData.Name);
        }

        public bool SetIO(int ioPin,EIOTrig_Mode mode,EIOState state)
        {
            IOData ioData = GetIO(ioPin);
            if (ioData.DMode != mode)
            {
                return false;
            }
            else
            {
                ioData.SetIO(mode, state);
            }
            return true;
        }

        public IOData GetIO(int ioPin)
        {
            return _elecDevSetting.IOSetting[ioPin];
        }

        public string SetDefaultIOScript()
        {
            string script = "";
            foreach (IOData ioData in _elecDevSetting.IOSetting.IOList)
            {
                script += SetDefaultIOScript(ioData.PinNum, ioData.DMode, ioData.DState, ioData.PulseWidth);
            }   

            return script;
 
        }

        public string SetDefaultIOScript(int ioPin, EIOTrig_Mode mode, EIOState state, double pw = 0)
        {
            string script = "";

            if (GetIO(ioPin) != null)
            {
                SetDefaultIO(ioPin, mode, state, EIOAct.NONE, pw);

                switch (mode)
                {
                    case EIOTrig_Mode.TRIG_BYPASS:
                        script += "digio.trigger[" + ioPin.ToString() + "].clear()\n";

                        script += "digio.writebit(" + ioPin.ToString() + ", " + ((int)state).ToString() + ")\n";

                        script += "digio.trigger[" + ioPin.ToString() + "].mode = digio.TRIG_BYPASS\n";
                        break;
                    case EIOTrig_Mode.TRIG_FALLING:
                        script += "digio.trigger[" + ioPin.ToString() + "].clear()\n";

                        script += "digio.trigger[" + ioPin.ToString() + "].mode = digio.TRIG_FALLING\n";

                        script += "digio.trigger[" + ioPin.ToString() + "].pulsewidth = " + pw.ToString() + "\n";
                        break;
                    case EIOTrig_Mode.TRIG_RISING:
                        script += "digio.writebit(" + ioPin.ToString() + ", 0)\n";

                        script += "digio.trigger[" + ioPin.ToString() + "].pulsewidth = " + pw.ToString() + "\n";

                        script += "digio.trigger[" + ioPin.ToString() + "].mode = digio.TRIG_RISING\n";
                        break;
                    case EIOTrig_Mode.READ:
                        //script += "digio.writebit(" + ioPin.ToString() + ", " + ((int)state).ToString() + ")\n";
                        //script += "digio.trigger[" + ioPin.ToString() + "].release()\n";

                        script += "digio.trigger[" + ioPin.ToString() + "].reset()\n";

                        script += "digio.trigger[" + ioPin.ToString() + "].clear()\n";

                        script += "digio.trigger[" + ioPin.ToString() + "].mode = digio.TRIG_BYPASS\n";
                        break;
                }
            }
            return script;
        }

        public string SetSingleIOScript(int ioPin, EIOState state = EIOState.NONE, EIOTrig_Mode mode = EIOTrig_Mode.TRIG_BYPASS, double pw = 0)
        {
            string script = "";
            IOData ioData = GetIO(ioPin);
            if (ioData != null )//&& ioData.IsShow)
            {
                switch (mode)
                {
                    case EIOTrig_Mode.TRIG_FALLING:
                    case EIOTrig_Mode.TRIG_RISING:

                        script += "digio.trigger[" + ioPin.ToString() + "].clear()\n";

                        if (state == EIOState.ASSERT)
                        {
                            script += "digio.trigger[" + ioPin.ToString() + "].assert()\n";
                        }
                        else if (state == EIOState.WAIT)
                        {
                            script += "digio.trigger[" + ioPin.ToString() + "].wait(" + pw.ToString("0.######")+ ")\n";
                        }
                        break;
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////
                    case EIOTrig_Mode.READ:

                        if (ioData.State != state && SetIO(ioPin, mode, state))
                        {
                            script = "print(digio.readbit(" + ioPin.ToString() + ") )\n";
                        }
                        break;
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////
                    default:
                    case EIOTrig_Mode.TRIG_BYPASS:

                        if (ioData.State != state && SetIO(ioPin, mode, state))
                        {
                            script += "digio.writebit(" + ioPin.ToString() + ", " + ((int)state).ToString() + ")\n";
                        }
                        break;
                }
            }
            return script;
        }

        public string IOWritebit(int ioPin, EIOState state)
        {
            return SetSingleIOScript( ioPin,  state); 
        }

        public string IOWritebit(uint ioPin, EIOState state)
        {
            return SetSingleIOScript((int)ioPin, state);
        }

        public string IOAssert(int ioPin)
        {
            return SetSingleIOScript(ioPin,EIOState.NONE,EIOTrig_Mode.TRIG_FALLING);
        }

        public string IOAssert(uint ioPin)
        {
            return SetSingleIOScript((int)ioPin, EIOState.NONE, EIOTrig_Mode.TRIG_FALLING);
        }

        public string IOWait(int ioPin,double pw)
        {
            return SetSingleIOScript(ioPin, EIOState.WAIT, EIOTrig_Mode.TRIG_FALLING, pw);
        }
        public string IOWait(uint ioPin, double pw)
        {
            return IOWait((int) ioPin, pw);
        }

        public byte GetIONState(int ioNum)
        {
            byte result = 0x00;

            string readStr = string.Empty;

            string script = "print(digio.readport())\n";
            
            this._conn.SendCommand(script);

            int shift = ioNum > 0 ? ioNum - 1 : 0;

            if (this._conn.WaitAndGetData(out readStr))
            {
                if (readStr != string.Empty)
                {
                    uint tempData = (uint)Convert.ToDouble(readStr);

                    byte tmp = (byte)(tempData >> (shift));
                    result = (byte)(tmp & 0x01);
                }
            }
            return result;
 
        }

        public byte[] GetIONState()
        {
            byte result = 0x00;

            string readStr = string.Empty;

            string script = "print(digio.readport())\n";

            this._conn.SendCommand(script);
            
            List<byte> byteList = new List<byte>();

            if (this._conn.WaitAndGetData(out readStr))
            {
                if (readStr != string.Empty)
                {
                    uint tempData = (uint)Convert.ToDouble(readStr);
                    
                    for (int i = 0; i < 14; ++i)
                    {
                        byte tmp = (byte)(tempData >> (i));
                        result = (byte)(tmp & 0x01);
                        byteList.Add(result);
                    }
                }
            }
            return byteList.ToArray();

        }

        string SetIOScript(uint index, ElectSettingData item)
        {

            string script = "loadscript num_" + index.ToString() + "\n";

            IOSetting _item = (item.IOSetting.Clone() as IOSetting);

            List<IOAction> actList = new List<IOAction>();

            foreach (IOCmd ioD in _item.CmdList)
            {
                IOData ioData = GetIO(ioD.Pin);
                if (ioData == null)
                {
                    continue;
                }
                switch (ioD.Mode)
                {
                    case EIOTrig_Mode.TRIG_BYPASS:
                        {
                            if (ioD.State != ioData.State)
                            {
                                actList.Add(new IOAction(ioD.Pin, ioD.DelayTime, ioD.Mode, ioD.State));
                                if (ioD.HoldTime > 0)//<0保持該狀態直至下一次修改狀態或該次測試結束
                                {
                                    actList.Add(new IOAction(ioD.Pin, ioD.DelayTime + ioD.HoldTime, ioD.Mode, ioData.State));
                                }
                            } 
                        }
                        break;

                    case EIOTrig_Mode.TRIG_FALLING:
                    case EIOTrig_Mode.TRIG_RISING:
                        {
                            if (ioD.State == EIOState.ASSERT)
                            {
                                actList.Add(new IOAction(ioD.Pin, ioD.DelayTime, ioD.Mode, ioD.State));
                            }
                            else if (ioD.State == EIOState.WAIT)
                            {
                                actList.Add(new IOAction(ioD.Pin, ioD.DelayTime, ioD.Mode, ioD.State,ioD.HoldTime));//
                            } 
                        }
                        break;
                }
            }

            actList.Sort((x, y) => { return x.delayTime.CompareTo(y.delayTime); });

            double nowDelay = 0;
            foreach (IOAction actData in actList)
            {                 
                if(nowDelay != actData.delayTime)
                {
                    double dTime = actData.delayTime - nowDelay;
                    script += "delay(" + dTime.ToString() + ")\n";
                    nowDelay = actData.delayTime;
                }

                script += SetSingleIOScript(actData.pin, actData.state, actData.mode, actData.holdTime);
            }

            script += "print(0)\n";

            script += "endscript\n";

            //this._conn.SendCommand(script);

            return script;
        }

        public void SetIOToDefaultScript()
        {
            string script = "";
            List<IOAction> actList = new List<IOAction>();

            foreach (IOData ioD in _elecDevSetting.IOSetting.IOList)
            {
                //script += SetSingleIOScript(ioD.PinNum, ioD.DState, ioD.Mode, ioD.PulseWidth);
                //IOData ioData = GetIO(ioPin);
                if (ioD != null)
                {
                    int ioPin = ioD.PinNum;
                    EIOState state = ioD.State;
                    if(state != ioD.DState )
                    switch (ioD.Mode)
                    {
                        case EIOTrig_Mode.TRIG_FALLING:
                        case EIOTrig_Mode.TRIG_RISING:

                            script += "digio.trigger[" + ioPin.ToString() + "].clear()\n";
                            break;
                        case EIOTrig_Mode.READ:

                            break;

                        default:
                        case EIOTrig_Mode.TRIG_BYPASS:

                            if (state != ioD.DState )
                            {
                                script += "digio.writebit(" + ioPin.ToString() + ", " + ((int)state).ToString() + ")\n";
                            }
                            break;
                    }
                }
            }       

            this._conn.SendCommand(script);
 
        }

        public void SetIOToDefaultState()
        {
            foreach (IOData ioD in _elecDevSetting.IOSetting.IOList)
            {
                ioD.Mode = ioD.DMode;
                ioD.State = ioD.DState;
            }
        }

        public static void SetDefaultIOConfig(IOConfigData ioCfg,int IO_QTY)
        {
            ioCfg.SetIOQty(IO_QTY);
            double ioPulseWidth = K2600Const.IO_DEFUALT_PULSE_WIDTH;
            double trigPw = 0.0001;

            ioCfg[(int)K2600Const.IO_SMU_TRIG_IN].SetDefaultIO(EIOTrig_Mode.TRIG_FALLING, EIOState.HIGH, EIOAct.FALLING, trigPw, "IO_SMU_TRIG_IN");
            ioCfg[(int)K2600Const.IO_PM_RELAY1].SetDefaultIO(EIOTrig_Mode.TRIG_FALLING, EIOState.HIGH, EIOAct.FALLING, trigPw, "IO_PM_RELAY1");
            ioCfg[(int)K2600Const.IO_SMU_ABORT_IN].SetDefaultIO(EIOTrig_Mode.TRIG_FALLING, EIOState.HIGH, EIOAct.FALLING, 0.0005, "IO_SMU_ABORT_IN");
            ioCfg[(int)K2600Const.IO_PM_RELAY2].SetDefaultIO(EIOTrig_Mode.TRIG_FALLING, EIOState.HIGH, EIOAct.FALLING, 0.0005, "IO_PM_RELAY2");

            ioCfg[(int)K2600Const.IO_RTH_EANBLE].SetDefaultIO(EIOTrig_Mode.TRIG_RISING, EIOState.LOW, EIOAct.RISING, 0.00005, "IO_RTH_EANBLE");
            ioCfg[(int)K2600Const.IO_DAQ_ENABLE].SetDefaultIO(EIOTrig_Mode.TRIG_RISING, EIOState.LOW, EIOAct.RISING, ioPulseWidth, "IO_DAQ_ENABLE");

            ioCfg[(int)K2600Const.IO_DAQ_TRIG_OUT].SetDefaultIO(EIOTrig_Mode.TRIG_RISING, EIOState.LOW, EIOAct.RISING, ioPulseWidth, "IO_DAQ_TRIG_OUT");
            ioCfg[(int)K2600Const.IO_POLAR_SW].SetDefaultIO(EIOTrig_Mode.TRIG_RISING, EIOState.LOW, EIOAct.RISING, 0.0001, "IO_POLAR_SW");


        }

        internal class IOAction
        {
            public IOAction(int p ,double d,EIOTrig_Mode m,EIOState s,double h = 0)
            {
                pin = p;
                delayTime = d;
                mode = m;
                state = s;
                holdTime = h;
            }
            public int pin;
            public double delayTime;
            public double holdTime;
            public EIOTrig_Mode mode;
            public EIOState state;
 
        }
    }
}
