using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPI.Tester.Data;

namespace MPI.Tester.Gui
{
    public interface IConditionUICtrl
    {
        void RefreshUI();
        bool CheckUI(out string msg);
        void UpdateCondtionDataToComponent(TestItemData data);
        TestItemData GetConditionDataFromComponent();
    }
}
