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


        //SubRecipeInformation = 100101,  // P10T		
        PreSOTInformationForDP = 100102,  // LDP80V
        PreWaferINInformation = 100103,  // AWSC
        PreSOTInformationForProber = 100104,  // AWSC

        PreOverloadTestInfo = 100201,  // Emcore
        ChuckTemperatureInfo = 100202,

        PreWaferInForAMS = 100301,//AMS
        TesterOutputRelativePath = 100302,

        CheckLaserPower = 100401,
    }
}
