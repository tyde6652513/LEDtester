using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MPI.Tester.DeviceCommon
{

    public enum EOSAModel
    {
        [XmlEnum(Name = "0")]
        NONE = 0,
        [XmlEnum(Name = "1")]
        MS9740A = 1,
        [XmlEnum(Name = "2")]
        AQ6370D = 2,
    }

    public enum ESenseMode : int
    {
        NHLD = 2,
        NAUT = 3,
        NORMal = 4,
        MID = 5,
        HIGH1 = 6,
        HIGH2 = 7,
        HIGH3 = 8,
    }

    //SWTHresh|SWENvelope|SWRMs|SWPKrms|  7-48
    //            NOTCh|DFBLd| FPLD|LED|SMSR|POWer|
    //            PMD|WDM|NF|FILPk|FILBtm|WFPeak|
    //            WFBtm|OSNR|COLor
    public enum EAQ6370DAnalysisMode : int
    {
        SWTHresh = 1, // THRESH
        SWENvelope = 2,
        SWRMs = 3,
        SWPKrms = 4,
        DFBLd = 5,
        FPLD = 6,
        SMSR = 8,
    }

    //SWTHresh|SWENvelope|SWRMs|SWPKrms|  7-48
    //            NOTCh|DFBLd| FPLD|LED|SMSR|POWer|
    //            PMD|WDM|NF|FILPk|FILBtm|WFPeak|
    //            WFBtm|OSNR|COLor
    public enum EMS9740AnalysisMode : int
    {
        NONE = 0,
        ENV = 1, // Envelop
        SMSR = 2,
        RMS = 3,
        PWR = 4,
        FP_LD = 10,
        DFB_LD = 11,
    }

    public enum EMS9740DfbSideMode : int
    {
        Second_Peak  = 0,
        Left  = 1,
        Right = 2,
    }

}
