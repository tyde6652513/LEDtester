using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Device.SpectroMeter.LE5400
{
    public enum EConnectMode
    {
        VirtualCOM =1,
        Highspeed =2,
        UPD=3,
    }

    public enum EMcpdApiErrorCode
    {
        SUCCESS = 1,
        ERROR = 0,
        AADJUST = -10,
        ABNORMAL = -21,
        ACOND = -3,
        CALCOVER = -5,
        CALCUNDER = -4,
        CLOSE = 2,
        EXEC = -20,
        GETDEFERREDERROR = 23,
        MACHINDAMAGE = -2,
        MACHINECTRL = -100,
        MACHINEFUNCTON = -101,
        MCOND = -2,
        MEMORY = -99,
        OPEN = 1,
        PARAM = -1,
        SENDBINARYDATA = 19,
        SENDCLEARINDEX = 5,
        SENDENABLE = 3,
        SENDGETCONFIGURATION = 10,
        SENDGETDARKDATA = 13,
        SENDGETMODESTATUS = 16,
        SENDGETPARAMS = 7,
        SENDGETSTATUS = 8,
        SENDGETVERSION = 14,
        SENDHARDWAREDIRECT = 11,
        SENDMODEREQUEST = 15,
        SENDREADIO = 21,
        SENDSETCONFIGURATION = 9,
        SENDSETDARKDATA = 12,
        SENDSETPARAMS = 6,
        SENDSTART = 4,
        SENDTRANSFERREQUEST = 17,
        SENDWRITEIO = 20,
        SENDWRITEREQUEST = 18,
        SETPACKETLISTENER = 22,
        UNSUPPORTEDFUNCTION = -6,
        WAITTIMEOUT = -1,
    }
}
