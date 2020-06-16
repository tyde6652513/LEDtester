using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.DeviceCommon
{
    public interface IOpticalSwitch
    {
        int Slot { get; }
        int NowsysCh { get; }
        string SerialNumber { get; }
        string Address { get; }

        List<KeyValuePair<int, int>> SysDevOutChList { get; }//要考慮2ch input其中一組被拿去在轉接的可能,Ex:in1->out1-2->in2->out2-3
        List<KeyValuePair<int, int>> SysDevInChList { get; }
        List<int> SysChList { get; }
        EDevErrorNumber ErrorNumber { get; }
        IConnect Connect { get; }

        OpticalSwitchSpec Spec { get; }

        bool Init(string address, IConnect connect);
        bool Push(int sysCh, int inCh, int outCh);
        void TurnOff();
        void Close();
        bool SetToSysCh(int sysCh);
        void ForceSetRelay(Dictionary<string, object> devLog);
    }
}

