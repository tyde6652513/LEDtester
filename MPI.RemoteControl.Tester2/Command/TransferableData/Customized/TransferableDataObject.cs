using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace MPI.RemoteControl2.Tester.Mpi.Command.Base
{
    public enum ETransferableCommonObject : int
    {
        None = -1,
        D76Data              = 100001,
        SubRecipeInformation = 100002,
        TesterInformation    = 100003,
		LaserBarProberInfo   = 100004,
        ProcessInformation   = 100005,
        TestingProperties    = 100006,
		MappingTable         = 100007,
    }
}
