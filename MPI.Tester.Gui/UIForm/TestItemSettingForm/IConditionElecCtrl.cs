using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPI.Tester.Gui
{
    interface IConditionElecCtrl
    {
        bool IsAutoSelectForceRange { get; set; }
        bool IsAutoSelectMsrtRange { get; set; }
        bool IsVisibleNPLC { get; set; }
        bool IsVisibleFilterCount { get; set; }
        bool IsEnableSwitchChannel { get; set; }
        bool IsEnableMsrtForceValue { get; set; }
        uint MaxSwitchingChannelCount { get; set; }
    }
}
