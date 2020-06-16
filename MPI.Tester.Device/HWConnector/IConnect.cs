using System;
using System.Collections.Generic;
using System.Text;

namespace MPI.Tester.Device
{
    public interface IConnect
    {
        int LastErrorNum { get; }
        string LastErrorStr { get; }
        string BufferData { get; }
     
        bool SendCommand(string command);
        bool QueryCommand(string command);
        bool WaitAndGetData(out string result);
        bool Open(out string Information);
        void Close();
    }
}
